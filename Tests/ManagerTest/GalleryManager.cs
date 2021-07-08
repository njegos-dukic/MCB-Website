using Core.AutoMapper.ExtensionMethods;
using Core.DTO;
using Core.Entity;
using Core.ErrorHandling;
using Core.Managers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tests.Common;

namespace Tests.ManagerTest
{
    [TestClass]
    public class GalleryManager
    {
        public TestContext TestContext { get; set; }

        [DataRow("NewGallery.json", OperationStatus.Success, true)]
        [DataRow("InvalidGallery.json", OperationStatus.InvalidData)]
        [DataTestMethod]
        public async Task AddGallery(string fileName, OperationStatus expectedOutcome, bool expectedSuccess = false)
        {
            // Configure
            (MasterCraftBreweryContext context, DataPool dataPool) = DbUtilities.CreateNewContext(TestContext);
            IGalleryManager galleryManager = ManagersFactory.GetGalleryManager(context);

            // Arrange
            GalleryBaseDTO galleryBaseDTO = DataGenerator.Deserialize<GalleryBaseDTO>(fileName);

            // Act
            ResultMessage<GalleryBaseDTO> result = await galleryManager.Add(galleryBaseDTO);

            // Assert
            Assert.AreEqual(expectedOutcome, result.Status, result.Message);
            if (expectedSuccess)
            {
                Assert.IsNotNull(result.Result);
                DataValidator.CheckAllProperties<GalleryBaseDTO>(result.Result, galleryBaseDTO, new string[] { "GalleryId", "CreatedOn" });
            }
        }

        [TestMethod]
        public async Task GetAllGalleries()
        {
            // Configure
            (MasterCraftBreweryContext context, DataPool dataPool) = DbUtilities.CreateNewContext(TestContext);
            IGalleryManager galleryManager = ManagersFactory.GetGalleryManager(context);

            // Arrange
            List<GalleryBaseDTO> expectedResult = dataPool.InputGalleries;

            // Act
            ResultMessage<List<GalleryWithThumbnailDTO>> result = await galleryManager.GetAll();
            List<GalleryWithThumbnailDTO> galleries = result.Result.ToList();

            // Asserts
            Assert.AreEqual(OperationStatus.Success, result.Status, result.Message);
            Assert.AreEqual(expectedResult.Count, galleries.Count);
            DataValidator.CheckCollectionEquality(galleries, expectedResult, (actual, expected) => actual.Name == expected.Name
                                                                                                          && actual.Description == expected.Description
                                                                                                          && actual.GalleryId == expected.GalleryId
                                                                                                          && actual.CreatedOn == expected.CreatedOn);

        }
    }
}
