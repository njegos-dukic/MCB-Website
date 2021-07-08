using Core.AutoMapper.ExtensionMethods;
using Core.DTO;
using Core.Entity;
using Core.ErrorHandling;
using Core.Util;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Core.Managers
{
    public class EventManager : IEventManager
    {
        private readonly IApiKeyManager apiKeyManager;
        private readonly IFileManager fileManager;
        private readonly MasterCraftBreweryContext context;

        public EventManager(IApiKeyManager apiKeyManager, IFileManager fileManager, MasterCraftBreweryContext context)
           => (this.apiKeyManager, this.fileManager, this.context) = (apiKeyManager, fileManager, context);

        /// <summary>
        /// Saves event to the database. Event is related to specific company.
        /// </summary>
        /// <param name="eventDTO">Information about the event</param>
        /// <returns></returns>
        public async Task<ResultMessage<EventDTO>> Add(EventDTO eventDTO)
        {
            try
            {
                Event entity = eventDTO.ToEntity();
                entity.CompanyId = await apiKeyManager.GetRelatedCompanyId();
                await context.Events.AddAsync(entity);
                await context.SaveChangesAsync();
                return new ResultMessage<EventDTO>(entity.ToDTO());
            }
            catch (DbUpdateException ex)
            {
                (OperationStatus status, string message) = DbUpdateExceptionHandler.HandleException(ex);
                return new ResultMessage<EventDTO>(status, message);
            }
        }

        /// <summary>
        /// Updates information about the event, everything except the image.
        /// </summary>
        /// <param name="eventDTO">New information about the event</param>
        /// <returns></returns>
        public async Task<ResultMessage<EventDTO>> Update(EventDTO eventDTO)
        {
            try
            {
                Event entity = await GetByFilter(x => x.EventId == eventDTO.EventId);
                if (entity == null)
                    return new ResultMessage<EventDTO>(OperationStatus.NotFound);

                if (await NotAuthenticated(entity))
                    throw new ForbiddenAccessException();

                Event newEntity = eventDTO.ToEntity();
                newEntity.PhotoUri = entity.PhotoUri;
                newEntity.CompanyId = entity.CompanyId;
                context.Entry<Event>(entity).CurrentValues.SetValues(newEntity);
                await context.SaveChangesAsync();

                return new ResultMessage<EventDTO>(newEntity.ToDTO());
            }
            catch (DbUpdateException ex)
            {
                (OperationStatus status, string message) = DbUpdateExceptionHandler.HandleException(ex);
                return new ResultMessage<EventDTO>(status, message);
            }
        }

        /// <summary>
        /// Deletes event from database, based on specified unique identifier.
        /// </summary>
        /// <param name="eventId">Unique identifier for the event</param>
        /// <returns>True if deleted, false if not.</returns>
        public async Task<ResultMessage<bool>> Delete(int eventId)
        {
            try
            {
                Event entity = await GetByFilter(x => x.EventId == eventId);
                if (entity == null)
                    return new ResultMessage<bool>(OperationStatus.NotFound);

                if (await NotAuthenticated(entity))
                    throw new ForbiddenAccessException();

                context.Events.Remove(entity);
                await context.SaveChangesAsync();
                return new ResultMessage<bool>(true);
            }
            catch (DbUpdateException ex)
            {
                (OperationStatus status, string message) = DbUpdateExceptionHandler.HandleException(ex);
                return new ResultMessage<bool>(status, message);
            }
        }

        /// <summary>
        /// Returns all events related to a specific company.
        /// </summary>
        /// <returns>Enumeration of events</returns>
        public async Task<ResultMessage<IAsyncEnumerable<EventDTO>>> GetAll()
        {
            int companyId = await apiKeyManager.GetRelatedCompanyId();
            IAsyncEnumerable<EventDTO> events = context.Events
                                               .Where(x => x.CompanyId == companyId)
                                               .Select(x => x.ToDTO())
                                               .AsAsyncEnumerable();
            return new ResultMessage<IAsyncEnumerable<EventDTO>>(events);
        }

        /// <summary>
        /// Finds and returns one event from the database, based 
        /// on specified unique identifier.
        /// </summary>
        /// <param name="eventId">Unique identifier for the event</param>
        /// <returns></returns>
        public async Task<ResultMessage<EventDTO>> GetById(int eventId)
        {
            Event entity = await GetByFilter(x => x.EventId == eventId);
            if (entity == null)
                return new ResultMessage<EventDTO>(OperationStatus.NotFound);

            if (await NotAuthenticated(entity))
                throw new ForbiddenAccessException();
            return new ResultMessage<EventDTO>(entity.ToDTO());
        }

        /// <summary>
        /// Enables user to change image that represents certain event.
        /// </summary>
        /// <param name="eventId">Unique identifier for the event</param>
        /// <param name="basicFile">Image's data</param>
        /// <returns></returns>
        public async Task<ResultMessage<bool>> ChangeImage(int eventId, BasicFileInfo basicFile)
        {
            try
            {
                Event entity = await GetByFilter(x => x.EventId == eventId);
                if (entity == null)
                    return new ResultMessage<bool>(OperationStatus.NotFound);

                if (await NotAuthenticated(entity))
                    throw new ForbiddenAccessException();

                string relativePathOfImage = PathBuilder.BuildRelativePathForEventImage(basicFile.FileName);
                ResultMessage<bool> savedImage = await fileManager.UploadFile(basicFile.FileData, relativePathOfImage);
                if (!savedImage)
                    return savedImage;

                entity.PhotoUri = relativePathOfImage;
                await context.SaveChangesAsync();
                return new ResultMessage<bool>(true, OperationStatus.Success);
            }
            catch (DbUpdateException ex)
            {
                (OperationStatus status, string message) = DbUpdateExceptionHandler.HandleException(ex);
                return new ResultMessage<bool>(status, message);
            }
        }

        /// <summary>
        /// Downloads image with specified dimensions, that belongs to an event specified with unique identifier.
        /// </summary>
        /// <param name="eventId">Unique identifier for the event</param>
        /// <param name="thumbnailDimensions">Dimensions for the image (optional)</param>
        /// <returns></returns>
        public async Task<ResultMessage<BasicFileInfo>> DownloadImage(int eventId, ThumbnailDimensions thumbnailDimensions = null)
        {
            var eventInfo = await context.Events.Select(x => new { x.EventId, x.CompanyId, x.PhotoUri })
                                                .SingleOrDefaultAsync(x => x.EventId == eventId);
            if (eventInfo == null)
                return new ResultMessage<BasicFileInfo>(OperationStatus.NotFound);

            if (eventInfo.CompanyId != await apiKeyManager.GetRelatedCompanyId())
                throw new ForbiddenAccessException();

            return new ResultMessage<BasicFileInfo>(await fileManager.DownloadFile(eventInfo.PhotoUri, thumbnailDimensions));
        }

        private async Task<bool> NotAuthenticated(Event entity)
           => entity.CompanyId != await apiKeyManager.GetRelatedCompanyId();

        private async Task<Event> GetByFilter(Expression<Func<Event, bool>> filter)
            => await context.Events.SingleOrDefaultAsync(filter);
    }
}
