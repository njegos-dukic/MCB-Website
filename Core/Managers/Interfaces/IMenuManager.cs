using Core.DTO;
using Core.DTO.Menu;
using Core.ErrorHandling;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Core.Managers
{
    public interface IMenuManager
    {
        Task<ResultMessage<MenuDTO>> Add(MenuDTO menuDTO);

        Task<ResultMessage<MenuDTO>> Update(MenuDTO menuDTO);

        Task<ResultMessage<bool>> Delete(int menuId);

        Task<ResultMessage<OutputMenuDTO>> GetById(int menuId);

        Task<ResultMessage<IAsyncEnumerable<OutputMenuDTO>>> GetAll();
    }
}
