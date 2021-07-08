using Core.AutoMapper.ExtensionMethods;
using Core.DTO;
using Core.DTO.Menu;
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
    public class MenuManager : IMenuManager
    {
        private readonly IApiKeyManager apiKeyManager;
        private readonly MasterCraftBreweryContext context;

        public MenuManager(IApiKeyManager apiKeyManager, MasterCraftBreweryContext context)
        {
            this.apiKeyManager = apiKeyManager;
            this.context = context;
        }

        /// <summary>
        /// Adds new menu to the database, including product items (specific serving of the product).
        /// </summary>
        /// <param name="menuDTO">Information about the menu</param>
        /// <returns></returns>
        public async Task<ResultMessage<MenuDTO>> Add(MenuDTO menuDTO)
        {
            try
            {
                if (await context.Menus.AnyAsync(x => x.Name == menuDTO.Name))
                    return new ResultMessage<MenuDTO>(OperationStatus.Exists);

                if (!IsValid(menuDTO))
                    return new ResultMessage<MenuDTO>(OperationStatus.InvalidData);

                Menu menu = menuDTO.ToEntity();
                menu.CompanyId = await apiKeyManager.GetRelatedCompanyId();
                await context.Menus.AddAsync(menu);
                await context.SaveChangesAsync();
                return new ResultMessage<MenuDTO>(menu.ToDTO());
            }
            catch (DbUpdateException ex)
            {
                (OperationStatus status, string message) = DbUpdateExceptionHandler.HandleException(ex);
                return new ResultMessage<MenuDTO>(status, message);
            }
        }

        /// <summary>
        /// Updates specific menu. If menu with provided unique identifier does 
        /// not exist, OperationStatus.NotFound will be returned. 
        /// </summary>
        /// <param name="menuDTO">Informations about the menu</param>
        /// <returns></returns>
        public async Task<ResultMessage<MenuDTO>> Update(MenuDTO menuDTO)
        {
            try
            {
                Menu menu = await context.Menus.Include(x => x.MenuItems)
                                                .SingleOrDefaultAsync(x => x.MenuId == menuDTO.MenuId);

                if (menu == null)
                    return new ResultMessage<MenuDTO>(OperationStatus.NotFound);

                if (await NotAuthenticated(menuDTO.MenuId))
                    throw new ForbiddenAccessException();

                if (!IsValid(menuDTO))
                    return new ResultMessage<MenuDTO>(OperationStatus.InvalidData);

                Menu newMenu = menuDTO.ToEntity();
                newMenu.CompanyId = menu.CompanyId;
                menu.MenuItems = newMenu.MenuItems;
                context.Entry(menu).CurrentValues.SetValues(newMenu);
                await context.SaveChangesAsync();

                return new ResultMessage<MenuDTO>(menu.ToDTO());
            }
            catch (DbUpdateException ex)
            {
                (OperationStatus status, string message) = DbUpdateExceptionHandler.HandleException(ex);
                return new ResultMessage<MenuDTO>(status, message);
            }
        }

        /// <summary>
        /// Deletes menu with menus from the database.
        /// </summary>
        /// <param name="menuId">Unique identifier for the menu</param>
        /// <returns></returns>
        public async Task<ResultMessage<bool>> Delete(int menuId)
        {
            try
            {
                Menu menu = await context.Menus.SingleOrDefaultAsync(x => x.MenuId == menuId);
                if (menu == null)
                    return new ResultMessage<bool>(OperationStatus.NotFound);

                if (await NotAuthenticated(menuId))
                    throw new ForbiddenAccessException();

                context.Menus.Remove(menu);
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
        /// Finds and returns menu based on specified unique identifier. 
        /// If no menu is found, OperationStatus.NotFound <see cref="OperationStatus"/> will be returned. 
        /// </summary>
        /// <param name="menuId">Unique identifier for the menu</param>
        /// <returns></returns>
        public async Task<ResultMessage<OutputMenuDTO>> GetById(int menuId)
        {
            try
            {
                OutputMenuDTO menuDto = await GetByFilter(x => x.MenuId == menuId);
                if (menuDto == null)
                    return new ResultMessage<OutputMenuDTO>(OperationStatus.NotFound);

                return new ResultMessage<OutputMenuDTO>(menuDto);
            }
            catch (DbUpdateException ex)
            {
                (OperationStatus status, string message) = DbUpdateExceptionHandler.HandleException(ex);
                return new ResultMessage<OutputMenuDTO>(status, message);
            }
        }

        /// <summary>
        /// Finds, maps and returns list of menus with basic information from the database.
        /// </summary>
        /// <returns></returns>
        public async Task<ResultMessage<IAsyncEnumerable<OutputMenuDTO>>> GetAll()
        {
            int companyId = await apiKeyManager.GetRelatedCompanyId();
            return new ResultMessage<IAsyncEnumerable<OutputMenuDTO>>(context.Menus
                                                                             .Where(x => x.CompanyId == companyId)
                                                                             .Select(x => x.ToOutputDTO())
                                                                             .AsAsyncEnumerable());
        }

        private async Task<OutputMenuDTO> GetByFilter(Expression<Func<Menu, bool>> filter)
        {
            Menu menu = await context.Menus.Include(x => x.MenuItems)
                                             .ThenInclude(menuItem => menuItem.ProductServing)
                                                 .ThenInclude(ps => ps.Product)
                                                      .ThenInclude(p => p.Ingredients)
                                           .Include(x => x.MenuItems)
                                             .ThenInclude(menuItem => menuItem.ProductServing)
                                                 .ThenInclude(ps => ps.Serving)
                                           .SingleOrDefaultAsync(filter);

            if (menu == null)
                return null;

            if (await NotAuthenticated(menu.MenuId))
                throw new ForbiddenAccessException();

            OutputMenuDTO menuDTO = menu.ToOutputDTO();
            menuDTO.MenuItems = menu.MenuItems
                                                // Two or more menu items can have same product information, but different serving size, so group them
                                                .GroupBy(si => si.ProductServing.Product, e => new { e.MenuItemId, e.ProductServing.Product, e.ProductServing })
                                                // Map anonymous type from previous operation into appropriate DTO object
                                                .Select(anonymous =>
                                                {
                                                    OutputMenuItemDTO menuItemDTO = anonymous.Key.ForMenu();
                                                    menuItemDTO.Servings = anonymous.Select(value =>
                                                                                        {
                                                                                            OutputMenuItemServingDTO dto = value.ProductServing.ForMenu();
                                                                                            dto.MenuItemId = value.MenuItemId;
                                                                                            return dto;
                                                                                        })
                                                                                    .OrderBy(value => value.Price)
                                                                                    .ToList();
                                                    return menuItemDTO;
                                                }).ToList();

            return menuDTO;
        }

        private async Task<bool> NotAuthenticated(int menuId)
        {
            int companyId = await apiKeyManager.GetRelatedCompanyId();
            return !await context.Menus.AnyAsync(x => x.MenuId == menuId && x.CompanyId == companyId);
        }

        private bool IsValid(MenuDTO menu)
            => InputValidator.IsValidString(menu.Name) && InputValidator.IsValidString(menu.Description);
    }
}
