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
    public class EventManager
    {
        public TestContext TestContext { get; set; }

        [DataRow("NewEvent.json", OperationStatus.Success)]
        [DataTestMethod]
        public async Task AddEvent(string fileName, OperationStatus expectedOutcome)
        {
            // Configure
            (MasterCraftBreweryContext context, DataPool dataPool) = DbUtilities.CreateNewContext(TestContext);
            IEventManager eventManager = ManagersFactory.GetEventManager(context);

            // Arrange
            EventDTO @event = DataGenerator.Deserialize<EventDTO>(fileName);

            // Act
            ResultMessage<EventDTO> result = await eventManager.Add(@event);

            // Assert
            Assert.AreEqual(expectedOutcome, result.Status, result.Status.ToString());
            if (expectedOutcome == OperationStatus.Success)
            {
                DataValidator.CheckAllProperties<EventDTO>(result.Result, @event, new string[] { "EventId", "PhotoUri" });
            }
        }

        [DataRow(OperationStatus.Success)]
        [DataRow(OperationStatus.NotFound, DataPool.WrongNegativeId)]
        [DataTestMethod]
        public async Task UpdateEvent(OperationStatus expectedOutcome, int? id = null)
        {
            // Configure
            (MasterCraftBreweryContext context, DataPool dataPool) = DbUtilities.CreateNewContext(TestContext);
            IEventManager eventManager = ManagersFactory.GetEventManager(context);

            // Arrange
            EventDTO updated = dataPool.Events[0];
            updated.EventId = id ?? updated.EventId;
            updated.Title = "New updated event";
            updated.BeginOn = DateTime.UtcNow;
            updated.EndOn = DateTime.UtcNow.AddDays(1);

            // Act
            ResultMessage<EventDTO> result = await eventManager.Update(updated);

            // Assert
            Assert.AreEqual(expectedOutcome, result.Status, result.Status.ToString());
            if (expectedOutcome == OperationStatus.Success)
            {
                DataValidator.CheckAllProperties<EventDTO>(result.Result, updated);
            }
        }

        [DataRow(OperationStatus.Success)]
        [DataRow(OperationStatus.NotFound, DataPool.WrongNegativeId)]
        [DataRow(OperationStatus.NotFound, DataPool.WrongPositiveId)]
        [DataTestMethod]
        public async Task GetEventById(OperationStatus expectedOutcome, int? uniqueIdentifier = null)
        {
            // Configure
            (MasterCraftBreweryContext context, DataPool dataPool) = DbUtilities.CreateNewContext(TestContext);
            IEventManager eventManager = ManagersFactory.GetEventManager(context);

            // Arrange
            int lookupId = uniqueIdentifier ?? dataPool.Events[0].EventId;

            // Act
            ResultMessage<EventDTO> result = await eventManager.GetById(lookupId);

            // Assert
            Assert.AreEqual(expectedOutcome, result.Status, result.Message);
            if (expectedOutcome == OperationStatus.Success)
            {
                Assert.IsNotNull(result.Result);
                Assert.AreEqual(lookupId, result.Result.EventId);
            }
        }

        [TestMethod]
        public async Task GetAllEvents()
        {
            // Configure
            (MasterCraftBreweryContext context, DataPool dataPool) = DbUtilities.CreateNewContext(TestContext);
            IEventManager eventManager = ManagersFactory.GetEventManager(context);

            // Arrange 
            List<EventDTO> expectedEvents = dataPool.Events;

            // Act
            ResultMessage<IAsyncEnumerable<EventDTO>> result = await eventManager.GetAll();
            List<EventDTO> actualEvents = await result.Result.ToListAsync();

            // Assert
            Assert.AreEqual(OperationStatus.Success, result.Status, result.Message);
            Assert.AreEqual(expectedEvents.Count, actualEvents.Count);
            DataValidator.CheckCollections(actualEvents, expectedEvents);
        }
    }
}
