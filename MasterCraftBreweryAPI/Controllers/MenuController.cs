using AutoMapper;
using Core.DTO;
using Core.DTO.Menu;
using Core.ErrorHandling;
using Core.Managers;
using MasterCraftBreweryAPI.ErrorHandling;
using MasterCraftBreweryAPI.Wrapper.Menu;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MasterCraftBreweryAPI.Controllers
{
    [Route("menu")]
    [ApiController]
    [Authorize(AuthenticationSchemes = Authentication.Authentication.ApiKeyScheme)]
    public class MenuController : MasterCraftBreweryControllerBase
    {
        private readonly IMenuManager menuManager;

        public MenuController(IMenuManager menuManager, IApiKeyManager apiKeyManager, IMapper mapper) : base(apiKeyManager, mapper)
             => this.menuManager = menuManager;

        /// <summary>
        /// Adds new menu to the database, including menus with product items (specific serving of the product).
        /// </summary>
        /// <param name="menu">Information about the menu</param>
        /// <returns></returns>
        [HttpPost]
        [Authorize(Policy = "TokenRequired")]
        public async Task<ActionResult> Add([FromBody] MenuPostWrapper menu)
        {
            ResultMessage<MenuDTO> result = await menuManager.Add(mapper.Map<MenuDTO>(menu));
            return HttpResultMessage.FilteredResult(result);
        }

        /// <summary>
        /// Updates specific menu. If menu with provided unique identifier does 
        /// not exist, no content will be returned. 
        /// menus on the menu are also updates. If menu does not exist, 
        /// a new menu will be created.
        /// </summary>
        /// <param name="menu">Informations about the menu</param>
        /// <returns></returns>
        [HttpPut]
        [Authorize(Policy = "TokenRequired")]
        public async Task<ActionResult> Update([FromBody] MenuPutWrapper menu)
        {
            ResultMessage<MenuDTO> result = await menuManager.Update(mapper.Map<MenuDTO>(menu));
            return HttpResultMessage.FilteredResult(result);
        }

        /// <summary>
        /// Deletes menu with menus from the database.
        /// </summary>
        /// <param name="menuId">Unique identifier for the menu</param>
        /// <returns></returns>
        [HttpDelete("{menuId}")]
        [Authorize(Policy = "TokenRequired")]
        public async Task<ActionResult> Delete([FromRoute] int menuId)
        {
            ResultMessage<bool> result = await menuManager.Delete(menuId);
            return HttpResultMessage.FilteredResult<MenuDeleteWrapper, bool>(result);
        }

        /// <summary>
        /// Finds, maps and returns list of menus from the database.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult> GetAll()
        {
            ResultMessage<IAsyncEnumerable<OutputMenuDTO>> result = await menuManager.GetAll();
            return HttpResultMessage.FilteredResult(result);
        }

        /// <summary>
        /// Finds and returns menu based on specified unique identifier. 
        /// If no menu is found, no content will be returned.
        /// </summary>
        /// <param name="menuId">Unique identifier for the menu</param>
        /// <returns></returns>
        [HttpGet("{menuId}")]
        public async Task<ActionResult> GetById([FromRoute] int menuId)
        {
            ResultMessage<OutputMenuDTO> result = await menuManager.GetById(menuId);
            return HttpResultMessage.FilteredResult(result);
        }
    }
}
