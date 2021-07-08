using Core.AutoMapper.ExtensionMethods;
using Core.DTO;
using Core.Entity;
using Core.ErrorHandling;
using Core.Managers;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Managers
{
    public class QuoteManager : IQuoteManager
    {
        private readonly IApiKeyManager apiKeyManager;
        private readonly MasterCraftBreweryContext context;

        public QuoteManager(IApiKeyManager apiKeyManager, MasterCraftBreweryContext context)
        {
            this.apiKeyManager = apiKeyManager;
            this.context = context;
        }

        /// <summary>
        /// Adds new quote to the database.
        /// </summary>
        /// <param name="quoteDTO">Information about the quote</param>
        /// <returns></returns>
        public async Task<ResultMessage<QuoteDTO>> Add(QuoteDTO quoteDTO)
        {
            try
            {
                if (string.IsNullOrEmpty(quoteDTO.QuoteText))
                    return new ResultMessage<QuoteDTO>(OperationStatus.InvalidData);

                if (await context.Quotes.AnyAsync(q => q.QuoteText == quoteDTO.QuoteText && q.Author == quoteDTO.Author))
                    return new ResultMessage<QuoteDTO>(OperationStatus.Exists);

                Quote quote = quoteDTO.ToEntity();
                quote.CompanyId = await apiKeyManager.GetRelatedCompanyId();
                quote.CreatedOn = DateTime.UtcNow;
                await context.AddAsync(quote);
                await context.SaveChangesAsync();

                return new ResultMessage<QuoteDTO>(quote.ToDTO());
            }
            catch (DbUpdateException ex)
            {
                (OperationStatus status, string message) = DbUpdateExceptionHandler.HandleException(ex);
                return new ResultMessage<QuoteDTO>(status, message);
            }
        }

        /// <summary>
        /// Updates existing quote from database.
        /// </summary>
        /// <param name="quoteDTO">Information about quote that will be updated</param>
        /// <returns></returns>
        public async Task<ResultMessage<QuoteDTO>> Update(QuoteDTO quoteDTO)
        {
            try
            {
                if (string.IsNullOrEmpty(quoteDTO.QuoteText))
                    return new ResultMessage<QuoteDTO>(OperationStatus.InvalidData);

                if (await context.Quotes.AnyAsync(q => q.QuoteText == quoteDTO.QuoteText && q.Author == quoteDTO.Author))
                    return new ResultMessage<QuoteDTO>(OperationStatus.Exists);

                Quote quote = await context.Quotes.SingleOrDefaultAsync(x => x.QuoteId == quoteDTO.QuoteId);

                if (quote == null)
                    return new ResultMessage<QuoteDTO>(OperationStatus.NotFound);

                if (await NotAuthenticated(quote))
                    throw new ForbiddenAccessException();

                Quote replacingQuote = quoteDTO.ToEntity();
                replacingQuote.CompanyId = quote.CompanyId;
                replacingQuote.CreatedOn = DateTime.UtcNow;
                context.Entry(quote).CurrentValues.SetValues(replacingQuote);
                await context.SaveChangesAsync();

                return new ResultMessage<QuoteDTO>(quote.ToDTO());
            }
            catch (DbUpdateException ex)
            {
                (OperationStatus status, string message) = DbUpdateExceptionHandler.HandleException(ex);
                return new ResultMessage<QuoteDTO>(status, message);
            }
        }

        /// <summary>
        /// Completly deletes a quote from the database.
        /// </summary>
        /// <param name="quoteId">Unique identifier for the quote</param>
        /// <returns></returns>
        public async Task<ResultMessage<bool>> Delete(int quoteId)
        {
            try
            {
                Quote quote = await context.Quotes.SingleOrDefaultAsync(x => x.QuoteId == quoteId);

                if (quote == null)
                    return new ResultMessage<bool>(OperationStatus.NotFound);

                if (await NotAuthenticated(quote))
                    throw new ForbiddenAccessException();

                context.Quotes.Remove(quote);
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
        /// Returns all quotes for company
        /// </summary>
        /// <returns></returns>
        public async Task<ResultMessage<IAsyncEnumerable<QuoteDTO>>> GetAll()
        {
            int companyId = await apiKeyManager.GetRelatedCompanyId();
            IQueryable<Quote> quotes = context.Quotes.Where(x => x.CompanyId == companyId)
                                                      .OrderByDescending(x => x.CreatedOn);

            return new ResultMessage<IAsyncEnumerable<QuoteDTO>>(quotes.Select(x => x.ToDTO()).AsAsyncEnumerable());
        }

        /// <summary>
        /// Returns quote by unique identifier if exists. If do not exists returns Not Found operation status.
        /// </summary>
        /// <param name="quoteId"></param>
        /// <returns></returns>
        public async Task<ResultMessage<QuoteDTO>> GetById(int quoteId)
        {
            Quote quote = await context.Quotes.SingleOrDefaultAsync(x => x.QuoteId == quoteId);

            if (quote == null)
                return new ResultMessage<QuoteDTO>(OperationStatus.NotFound);

            if (await NotAuthenticated(quote))
                throw new ForbiddenAccessException();

            QuoteDTO quoteDTO = quote.ToDTO();
            return new ResultMessage<QuoteDTO>(quoteDTO);
        }

        private async Task<bool> NotAuthenticated(Quote quote)
        {
            int companyId = await apiKeyManager.GetRelatedCompanyId();
            return quote.CompanyId != companyId;
        }
    }
}
