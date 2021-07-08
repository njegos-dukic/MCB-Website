using Core.DTO;
using Core.ErrorHandling;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Core.Managers
{
    public interface IQuoteManager
    {
        Task<ResultMessage<QuoteDTO>> Add(QuoteDTO quoteDTO);

        Task<ResultMessage<QuoteDTO>> Update(QuoteDTO quoteDTO);

        Task<ResultMessage<QuoteDTO>> GetById(int quoteId);

        Task<ResultMessage<bool>> Delete(int quoteId);

        Task<ResultMessage<IAsyncEnumerable<QuoteDTO>>> GetAll();

    }
}
