using AutoMapper;
using Core.DTO;
using Core.ErrorHandling;
using Core.Managers;
using Core.Util;
using MasterCraftBreweryAPI.ErrorHandling;
using MasterCraftBreweryAPI.Mapper;
using MasterCraftBreweryAPI.Util;
using MasterCraftBreweryAPI.Wrapper;
using MasterCraftBreweryAPI.Wrapper.Event;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MasterCraftBreweryAPI.Controllers
{
    [Route("events")]
    [ApiController]
    [Authorize(AuthenticationSchemes = Authentication.Authentication.ApiKeyScheme)]
    public class EventController : MasterCraftBreweryControllerBase
    {
        private readonly IEventManager eventManager;

        public EventController(IEventManager eventManager, IApiKeyManager apiKeyManager, IMapper mapper) : base(apiKeyManager, mapper)
        {
            this.eventManager = eventManager;
        }

        /// <summary>
        /// Adds new event into the database if there is no such event already 
        /// saved.
        /// </summary>
        /// <param name="eventWrapper">The information about the event</param>
        /// <returns></returns>
        [HttpPost]
        [Authorize(Policy = "TokenRequired")]
        public async Task<ActionResult> Add([FromBody] EventPostWrapper eventWrapper)
        {
            ResultMessage<EventDTO> result = await eventManager.Add(mapper.Map<EventDTO>(eventWrapper));
            return HttpResultMessage.FilteredResult(result);
        }

        /// <summary>
        /// Updates the whole event object. 
        /// Event needs to be identified using its unique identifier.
        /// If there is no such event in the database, OperationStatus.NotFound is returned.
        /// </summary>
        /// <param name="eventWrapper">New information about the event with existing unique identifier</param>
        /// <returns></returns>
        [HttpPut]
        [Authorize(Policy = "TokenRequired")]
        public async Task<ActionResult> Update([FromBody] EventPutWrapper eventWrapper)
        {
            ResultMessage<EventDTO> result = await eventManager.Update(mapper.Map<EventDTO>(eventWrapper));
            return HttpResultMessage.FilteredResult(result);
        }

        /// <summary>
        /// Completely deletes an event from the database, based on specific unique identifier.
        /// </summary>
        /// <param name="eventId">Unique identifier for the event</param>
        /// <returns>True if deleted, false if not</returns>
        [HttpDelete("{eventId}")]
        [Authorize(Policy = "TokenRequired")]
        public async Task<ActionResult> Delete([FromRoute] int eventId)
        {
            ResultMessage<bool> result = await eventManager.Delete(eventId);
            return HttpResultMessage.FilteredResult<EventDeleteResponseWrapper, bool>(result);
        }

        /// <summary>
        /// Finds and returns a specific event from database.
        /// </summary>
        /// <param name="eventId">Unique identifier for the event</param>
        /// <returns></returns>
        [HttpGet("{eventId}")]
        public async Task<ActionResult> GetById([FromRoute] int eventId)
        {
            ResultMessage<EventDTO> result = await eventManager.GetById(eventId);
            return HttpResultMessage.FilteredResult(result);
        }

        /// <summary>
        /// Returns all current events from database.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult> GetAll()
        {
            ResultMessage<IAsyncEnumerable<EventDTO>> result = await eventManager.GetAll();
            return HttpResultMessage.FilteredResult(result);
        }

        /// <summary>
        /// Changes image of an event.
        /// </summary>
        /// <param name="eventId">Unique identifier for the event</param>
        /// <param name="file">Information about the file: filename and data</param>
        /// <returns></returns>
        [HttpPut("{eventId}/image")]
        [Authorize(Policy = "TokenRequired")]
        public async Task<ActionResult> ChangeImage([FromRoute] int eventId, IFormFile file)
        {
            ResultMessage<bool> result = await eventManager.ChangeImage(eventId, file.AsBasicFileInfo());
            return HttpResultMessage.FilteredResult<ChangeImageResponseWrapper, bool>(result);
        }

        /// <summary>
        /// Downloads image for specified event, if such event exists. If the event does not exist,
        /// method returns no content. If specified image is not found or it is not set, method returns null.
        /// </summary>
        /// <param name="eventId">Unique identifier for the event</param>
        /// <param name="dimensions">Dimensions for the event's image</param>
        /// <returns></returns>
        [HttpGet("{eventId}/image")]
        public async Task<ActionResult> DownloadImage([FromRoute] int eventId, [FromQuery] ThumbnailDimensionsWrapper dimensions)
        {
            ResultMessage<BasicFileInfo> result = await eventManager.DownloadImage(eventId, dimensions?.ToCoreObject(mapper));
            return HttpResultMessage.FilteredResult(result, ContentType.File);
        }
    }
}
