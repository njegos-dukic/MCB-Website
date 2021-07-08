using Core.DTO;
using Core.Entity;
using Core.ErrorHandling;
using Core.Managers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using System.Threading.Tasks;
using Tests.Common;

namespace Tests.ManagerTest
{
    [TestClass]
    public class CompanyManager
    {
        public TestContext TestContext { get; set; }

        [DataRow(DataPool.ValidEmail, OperationStatus.Success)]
        [DataRow(DataPool.ValidEmail, OperationStatus.NotFound, DataPool.WrongNegativeId)]
        [DataRow(DataPool.WrongEmail, OperationStatus.InvalidData)]
        [DataTestMethod]
        public async Task UpdateCompany(string email, OperationStatus expectedOutcome, int? id = null)
        {
            // Configure
            (MasterCraftBreweryContext context, DataPool dataPool) = DbUtilities.CreateNewContext(TestContext);
            ICompanyManager companyManager = ManagersFactory.GetCompanyManager(context);

            // Arrange
            DetailedCompanyDTO dto = dataPool.ExistingCompany;
            dto.CompanyId = id ?? dto.CompanyId;
            dto.Name = "Master Craft Brewery 2";
            dto.Email = email;

            dto.Wholesales = dataPool.WholesaleDTOs.OrderBy(x => new Guid()).Take(3).ToList();
            dto.SocialMedias = dataPool.SocialMediaDTOs.OrderBy(x => new Guid()).Take(1).ToList();

            // Act
            ResultMessage<DetailedCompanyDTO> result = await companyManager.Update(dto);

            // Assert
            Assert.AreEqual(expectedOutcome, result.Status, result.Message);
            if (expectedOutcome == OperationStatus.Success)
            {
                Assert.IsNotNull(result.Result);
                DataValidator.CheckAllProperties(result.Result, dto, new string[] { "SocialMedias", "Wholesales", "Phones" });
                DataValidator.CheckCollections(result.Result.SocialMedias, dto.SocialMedias, new string[] { "SocialMediaId" });
                DataValidator.CheckCollections(result.Result.Wholesales, dto.Wholesales, new string[] { "WholesaleId" });
                DataValidator.CheckCollections(result.Result.Phones, dto.Phones, new string[] { "PhoneId" });
            }
        }


        [DataRow(OperationStatus.Success)]
        [DataRow(OperationStatus.NotFound, DataPool.WrongNegativeId)]
        [DataRow(OperationStatus.NotFound, DataPool.WrongPositiveId)]
        [DataTestMethod]
        public async Task GetCompanyById(OperationStatus expectedOutcome, int? uniqueIdentifier = null)
        {
            // Configure
            (MasterCraftBreweryContext context, DataPool dataPool) = DbUtilities.CreateNewContext(TestContext);
            ICompanyManager companyManager = ManagersFactory.GetCompanyManager(context);

            // Arrange
            int lookupId = uniqueIdentifier ?? dataPool.ExistingCompany.CompanyId;

            // Act
            ResultMessage<DetailedCompanyDTO> result = await companyManager.GetById(lookupId);

            // Assert
            Assert.AreEqual(expectedOutcome, result.Status, result.Message);
            if (expectedOutcome == OperationStatus.Success)
            {
                Assert.IsNotNull(result.Result);
                Assert.AreEqual(lookupId, result.Result.CompanyId);
            }
        }
    }
}
