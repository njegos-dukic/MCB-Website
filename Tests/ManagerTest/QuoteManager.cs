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
    public class QuoteManager
    {
        public TestContext TestContext { get; set; }



        [DataRow("NewQuote.json", OperationStatus.Success, true)]
        [DataRow("InvalidQuote.json", OperationStatus.InvalidData)]
        [DataTestMethod]
        public async Task AddQuote(string fileName, OperationStatus expectedOutcome, bool expectedSuccess = false)
        {
            // Configure
            (MasterCraftBreweryContext context, DataPool dataPool) = DbUtilities.CreateNewContext(TestContext);
            IQuoteManager quoteManager = ManagersFactory.GetQuoteManager(context);

            // Arrange
            QuoteDTO quoteDTO = DataGenerator.Deserialize<QuoteDTO>(fileName);

            // Act
            ResultMessage<QuoteDTO> result = await quoteManager.Add(quoteDTO);

            // Assert
            Assert.AreEqual(expectedOutcome, result.Status, result.Message);
            if(expectedSuccess)
            {
                Assert.IsNotNull(result.Result);
                DataValidator.CheckAllProperties<QuoteDTO>(result.Result, quoteDTO, new string[] { "QuoteId", "CompanyId", "CreatedOn" });
            }
        }

        [TestMethod]
        public async Task GetAllQuotes()
        {
            // Configure
            (MasterCraftBreweryContext context, DataPool dataPool) = DbUtilities.CreateNewContext(TestContext);
            IQuoteManager quoteManager = ManagersFactory.GetQuoteManager(context);

            // Arrange
            List<QuoteDTO> expectedResult = dataPool.InputQuotes;

            // Act
            ResultMessage<IAsyncEnumerable<QuoteDTO>> result = await quoteManager.GetAll();
            List<QuoteDTO> quotes = await result.Result.ToListAsync();

            // Asserts
            Assert.AreEqual(OperationStatus.Success, result.Status, result.Message);
            Assert.AreEqual(expectedResult.Count, quotes.Count);
            DataValidator.CheckCollections(quotes, expectedResult);

        }

        [DataRow("", OperationStatus.InvalidData)]
        [DataRow("QuoteText kojim se mijenja", OperationStatus.Success)]
        [DataRow("QuoteText kojim se mijenja", OperationStatus.NotFound, DataPool.WrongNegativeId)]
        [DataTestMethod]
        public async Task UpdateQuote(string newValue, OperationStatus expectedOutcome, int? id = null)
        {
            // Configure
            (MasterCraftBreweryContext context, DataPool dataPool) = DbUtilities.CreateNewContext(TestContext);
            IQuoteManager quoteManager = ManagersFactory.GetQuoteManager(context);

            // Arrange
            QuoteDTO quoteDTO = dataPool.ExistingQuote;
            quoteDTO.QuoteId = id ?? quoteDTO.QuoteId;
            quoteDTO.QuoteText = newValue;

            // Act
            ResultMessage<QuoteDTO> result = await quoteManager.Update(quoteDTO);

            // Assert
            Assert.AreEqual(expectedOutcome, result.Status, result.Message);
            if (expectedOutcome == OperationStatus.Success)
            {
                Assert.IsNotNull(result.Result);
                DataValidator.CheckAllProperties<QuoteDTO>(result.Result, quoteDTO, new string[] { "CompanyId", "CreatedOn" });
            }
        }


        [DataRow(OperationStatus.Success)]
        [DataRow(OperationStatus.NotFound, DataPool.WrongNegativeId)]
        [DataRow(OperationStatus.NotFound, DataPool.WrongPositiveId)]
        [DataTestMethod]
        public async Task GetByIdQuote(OperationStatus expectedOutcome, int? id = null)
        {
            // Configure
            (MasterCraftBreweryContext context, DataPool dataPool) = DbUtilities.CreateNewContext(TestContext);
            IQuoteManager quoteManager = ManagersFactory.GetQuoteManager(context);

            // Arrange
            int quoteId = id ?? dataPool.ExistingQuote.QuoteId;

            // Act 
            ResultMessage<QuoteDTO> result = await quoteManager.GetById(quoteId);

            // Assert
            Assert.AreEqual(expectedOutcome, result.Status, result.Message);
            if(expectedOutcome == OperationStatus.Success)
            {
                Assert.IsNotNull(result.Result);
                Assert.AreEqual(quoteId, result.Result.QuoteId);
            }
        }

        [DataRow(OperationStatus.Success)]
        [DataRow(OperationStatus.NotFound, DataPool.WrongNegativeId)]
        [DataTestMethod]
        public async Task DeleteQuote(OperationStatus expectedOutput, int? id = null)
        {
            // Configure
            (MasterCraftBreweryContext context, DataPool dataPool) = DbUtilities.CreateNewContext(TestContext);
            IQuoteManager quoteManager = ManagersFactory.GetQuoteManager(context);

            // Arrange
            int quoteId = id ?? dataPool.ExistingQuote.QuoteId;

            // Act
            ResultMessage<bool> result = await quoteManager.Delete(quoteId);
            QuoteDTO deletedDTO = await quoteManager.GetById(quoteId);

            // Asserts
            Assert.AreEqual(expectedOutput, result.Status, result.Message);
            Assert.IsNull(deletedDTO);
        }

    }
}
