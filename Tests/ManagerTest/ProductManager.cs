#nullable enable
using Core.DTO;
using Core.Entity;
using Core.ErrorHandling;
using Core.Managers;
using Core.Util;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Tests.Common;

namespace Tests.ManagerTest
{
    [TestClass]
    public class ProductManager
    {
        public TestContext TestContext { get; set; }

        [DataRow("NewProduct.json", OperationStatus.Success, true)]
        [DataRow("ExistingProduct.json", OperationStatus.Exists)]
        [DataRow("InvalidProduct.json", OperationStatus.InvalidData)]
        [DataTestMethod]
        public async Task AddProduct(string fileName, OperationStatus expectedOutcome, bool expectSuccess = false)
        {
            // Configure
            (MasterCraftBreweryContext context, DataPool dataPool) = DbUtilities.CreateNewContext(TestContext);
            IProductManager productManager = ManagersFactory.GetProductManager(context);

            // Arrange
            InputProductDTO product = DataGenerator.Deserialize<InputProductDTO>(fileName);

            product.ProductServings.ForEach(x => x.ServingId = dataPool.Servings.Random().ServingId);
            product.ProductTypeId = dataPool.ProductTypes.Random().ProductTypeId;

            // Act
            ResultMessage<OutputProductDTO> result = await productManager.Add(product);

            // Assert
            Assert.AreEqual(expectedOutcome, result.Status, result.Message);
            if (expectSuccess)
            {
                Assert.IsNotNull(result.Result);
                DataValidator.CheckAllProperties<BaseProductDTO>(result.Result, product, new string[] { "ProductId", "HexColor" });
                DataValidator.CheckCollectionEquality(result.Result.ProductServings, product.ProductServings, (x, y) => x.ServingId == y.ServingId);
                Assert.AreEqual(product.ProductTypeId, result.Result.ProductType.ProductTypeId, "Type of product is not equal expected value");
            }
        }

        [DataRow("MCB biftek", OperationStatus.Success)]
        [DataRow("MCB biftek", OperationStatus.NotFound, DataPool.WrongNegativeId)]
        [DataTestMethod]
        public async Task UpdateProduct(string newName, OperationStatus expectedOutcome, int? id = null)
        {
            // Configure
            (MasterCraftBreweryContext context, DataPool dataPool) = DbUtilities.CreateNewContext(TestContext);
            IProductManager productManager = ManagersFactory.GetProductManager(context);

            // Arrange
            InputProductDTO product = dataPool.ExistingProduct;
            product.ProductId = id ?? product.ProductId;
            product.Name = newName;

            product.ProductServings.ForEach(x => x.ServingId = dataPool.Servings.Random().ServingId);
            product.ProductTypeId = dataPool.ProductTypes.Random().ProductTypeId;

            // Act
            ResultMessage<OutputProductDTO> result = await productManager.Update(product);

            // Assert
            Assert.AreEqual(expectedOutcome, result.Status, result.Message);
            if (expectedOutcome == OperationStatus.Success)
            {
                Assert.IsNotNull(result.Result);
                DataValidator.CheckAllProperties<BaseProductDTO>(result.Result, product);
                DataValidator.CheckCollectionEquality(result.Result.ProductServings, product.ProductServings,
                                                        (x, y) => x.ServingId == y.ServingId && x.ProductId == y.ProductId);

                Assert.AreEqual(product.ProductTypeId, result.Result.ProductType.ProductTypeId, "Type of product is not equal expected value");
            }
        }

        [DataRow()]
        [DataRow(DataPool.WrongName)]
        [DataTestMethod]
        public async Task GetAllProductsOfSameType(string? name = null)
        {
            // Configure
            (MasterCraftBreweryContext context, DataPool dataPool) = DbUtilities.CreateNewContext(TestContext);
            IProductManager productManager = ManagersFactory.GetProductManager(context);

            // Arrange
            string productTypeName = name ?? dataPool.ProductTypes.Random().Name;
            List<OutputProductDTO> expectedProducts = dataPool.Products.Where(x => x.ProductType.Name == productTypeName).ToList();

            // Act
            ResultMessage<IAsyncEnumerable<OutputProductDTO>> result = await productManager.GetAll(productTypeName);
            List<OutputProductDTO> actualProducts = await result.Result.ToListAsync();

            // Assert
            Assert.AreEqual(OperationStatus.Success, result.Status, result.Message);
            Assert.IsNotNull(actualProducts);
            Assert.AreEqual(expectedProducts.Count, actualProducts.Count);
            Assert.IsTrue(actualProducts.All(x => x.ProductType.Name == productTypeName));
            DataValidator.CheckCollectionEquality(actualProducts, expectedProducts, (actual, expected) => actual.Name == expected.Name
                                                                                                          && actual.ProductId == expected.ProductId
                                                                                                          && actual.Description == expected.Description
                                                                                                          && actual.ProductServings.Count == expected.ProductServings.Count);
        }


        [DataRow(OperationStatus.Success)]
        [DataRow(OperationStatus.NotFound, DataPool.WrongNegativeId)]
        [DataRow(OperationStatus.NotFound, DataPool.WrongPositiveId)]
        [DataTestMethod]
        public async Task GetProductById(OperationStatus expectedOutcome, int? uniqueIdentifier = null)
        {
            // Configure
            (MasterCraftBreweryContext context, DataPool dataPool) = DbUtilities.CreateNewContext(TestContext);
            IProductManager productManager = ManagersFactory.GetProductManager(context);

            // Arrange
            int lookupId = uniqueIdentifier ?? dataPool.ExistingProduct.ProductId;

            // Act
            ResultMessage<OutputProductDTO> result = await productManager.GetById(lookupId);

            // Assert
            Assert.AreEqual(expectedOutcome, result.Status, result.Message);
            if (expectedOutcome == OperationStatus.Success)
            {
                Assert.IsNotNull(result.Result);
                Assert.AreEqual(lookupId, result.Result.ProductId);
            }
        }

        [TestMethod]
        public async Task GetAllProducts()
        {
            // Configure
            (MasterCraftBreweryContext context, DataPool dataPool) = DbUtilities.CreateNewContext(TestContext);
            IProductManager productManager = ManagersFactory.GetProductManager(context);

            // Arrange 
            List<OutputProductDTO> expectedProducts = dataPool.Products;

            // Act
            ResultMessage<IAsyncEnumerable<OutputProductDTO>> result = await productManager.GetAll();
            List<OutputProductDTO> actualProducts = await result.Result.ToListAsync();

            // Assert
            Assert.AreEqual(OperationStatus.Success, result.Status, result.Message);
            Assert.AreEqual(expectedProducts.Count, actualProducts.Count);
            DataValidator.CheckCollectionEquality(actualProducts, expectedProducts, (actual, expected) => actual.Name == expected.Name
                                                                                                          && actual.ProductId == expected.ProductId
                                                                                                          && actual.Description == expected.Description
                                                                                                          && actual.ProductServings.Count == expected.ProductServings.Count);
        }

        [DataRow(OperationStatus.Success)]
        [DataRow(OperationStatus.NotFound, DataPool.WrongNegativeId)]
        [DataTestMethod]
        public async Task DeleteProduct(OperationStatus expectedOutcome, int? uniqueIdentifier = null)
        {
            // Configure
            (MasterCraftBreweryContext context, DataPool dataPool) = DbUtilities.CreateNewContext(TestContext);
            IProductManager productManager = ManagersFactory.GetProductManager(context);

            // Arrange
            int lookupId = uniqueIdentifier ?? dataPool.ExistingProduct.ProductId;

            // Act
            ResultMessage<bool> result = await productManager.Delete(lookupId);
            OutputProductDTO companyThatWasDeleted = await productManager.GetById(lookupId);

            // Assert
            Assert.AreEqual(expectedOutcome, result.Status, result.Message);
            Assert.IsNull(companyThatWasDeleted);
        }

        [DataRow(OperationStatus.Success, DataPool.ProductImage)]
        [DataRow(OperationStatus.NotFound, DataPool.ProductImage, DataPool.WrongNegativeId)]
        [DataTestMethod]
        public async Task ChangeImage(OperationStatus expectedOutcome, string fileName, int? productId = null)
        {
            // Configure
            (MasterCraftBreweryContext context, DataPool dataPool) = DbUtilities.CreateNewContext(TestContext);
            IProductManager productManager = ManagersFactory.GetProductManager(context);

            // Arrange
            int lookupId = productId ?? dataPool.ExistingProduct.ProductId;
            BasicFileInfo fileInfo = new BasicFileInfo()
            {
                FileData = File.ReadAllBytes(fileName),
                FileName = Path.GetFileName(fileName)
            };

            // Act
            ResultMessage<bool> result = await productManager.ChangeImage(lookupId, fileInfo);

            // Assert
            Assert.AreEqual(expectedOutcome, result.Status, result.Message);
        }
    }
}

