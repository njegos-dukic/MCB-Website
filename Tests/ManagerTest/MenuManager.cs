using Core.DTO;
using Core.DTO.Menu;
using Core.Entity;
using Core.ErrorHandling;
using Core.Managers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;
using Tests.Common;

namespace Tests.ManagerTest
{
    [TestClass]
    public class MenuManager
    {
        public TestContext TestContext { get; set; }

        [DataRow("NewMenu.json", OperationStatus.Success)]
        [DataRow("ExistingMenu.json", OperationStatus.Exists)]
        [DataTestMethod]
        public async Task AddMenu(string fileName, OperationStatus expectedOutcome)
        {
            // Configure
            (MasterCraftBreweryContext context, DataPool dataPool) = DbUtilities.CreateNewContext(TestContext);
            IMenuManager menuManager = ManagersFactory.GetMenuManager(context);

            // Arrange
            MenuDTO menu = DataGenerator.Deserialize<MenuDTO>(fileName);
            menu.MenuItems.ForEach(x => x.ProductServingId = dataPool.ProductServingIds.Random());

            // Act
            ResultMessage<MenuDTO> result = await menuManager.Add(menu);

            // Assert
            Assert.AreEqual(expectedOutcome, result.Status, result.Status.ToString());
            if (expectedOutcome == OperationStatus.Success)
            {
                DataValidator.CheckAllProperties<MenuDTO>(result.Result, menu, new string[] { "MenuId" });
                DataValidator.CheckCollectionEquality(menu.MenuItems, result.Result.MenuItems, (x, y) => x.ProductServingId == y.ProductServingId);
            }
        }

        [DataRow("MCB novi meni", OperationStatus.Success)]
        [DataRow("MCB novi meni", OperationStatus.NotFound, DataPool.WrongNegativeId)]
        [DataTestMethod]
        public async Task UpdateMenu(string newName, OperationStatus expectedOutcome, int? id = null)
        {
            // Configure
            (MasterCraftBreweryContext context, DataPool dataPool) = DbUtilities.CreateNewContext(TestContext);
            IMenuManager menuManager = ManagersFactory.GetMenuManager(context);

            // Arrange
            MenuDTO menu = dataPool.ExistingMenu;
            menu.MenuId = id ?? dataPool.ExistingMenu.MenuId;
            menu.Name = newName;

            // Act
            ResultMessage<MenuDTO> result = await menuManager.Update(menu);

            // Assert
            Assert.AreEqual(expectedOutcome, result.Status, result.Status.ToString());
        }


        [DataRow(OperationStatus.Success)]
        [DataRow(OperationStatus.NotFound, DataPool.WrongNegativeId)]
        [DataRow(OperationStatus.NotFound, DataPool.WrongPositiveId)]
        [DataTestMethod]
        public async Task GetMenuById(OperationStatus expectedOutcome, int? uniqueIdentifier = null)
        {
            // Configure
            (MasterCraftBreweryContext context, DataPool dataPool) = DbUtilities.CreateNewContext(TestContext);
            IMenuManager menuManager = ManagersFactory.GetMenuManager(context);

            // Arrange
            int lookupId = uniqueIdentifier ?? dataPool.ExistingMenu.MenuId;

            // Act
            ResultMessage<OutputMenuDTO> result = await menuManager.GetById(lookupId);

            // Assert
            Assert.AreEqual(expectedOutcome, result.Status, result.Message);
            if (expectedOutcome == OperationStatus.Success)
            {
                Assert.IsNotNull(result.Result);
                Assert.AreEqual(lookupId, result.Result.MenuId);
            }
        }


        [DataRow(OperationStatus.Success)]
        [DataRow(OperationStatus.NotFound, DataPool.WrongNegativeId)]
        [DataTestMethod]
        public async Task DeleteMenu(OperationStatus expectedOutcome, int? uniqueIdentifier = null)
        {
            // Configure
            (MasterCraftBreweryContext context, DataPool dataPool) = DbUtilities.CreateNewContext(TestContext);
            IMenuManager menuManager = ManagersFactory.GetMenuManager(context);

            // Arrange
            int lookupId = uniqueIdentifier ?? dataPool.ExistingMenu.MenuId;

            // Act
            ResultMessage<bool> result = await menuManager.Delete(lookupId);
            OutputMenuDTO companyThatWasDeleted = await menuManager.GetById(lookupId);

            // Assert
            Assert.AreEqual(expectedOutcome, result.Status, result.Message);
            Assert.IsNull(companyThatWasDeleted);
        }
    }
}
