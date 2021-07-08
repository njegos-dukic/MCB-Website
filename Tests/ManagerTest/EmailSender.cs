using Core.DTO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Tests.Common;

namespace Tests.ManagerTest
{
    [TestClass]
    public class EmailSender
    {
        [TestMethod]
        public async Task SendOrderEmail()
        {
            OutputOrderDTO outputOrder = new OutputOrderDTO()
            {
                FullName = "Marija Novakovic",
                Email = "marija@gmail.com",
                DeliveryCost = 4,
                TotalCost = 44,
                Street = "Banjalukca 2",
                City = "Banja Luka",
                CountryName = "Republika Srpska",
                PostalCode = "78000",
                OrderedOn = DateTime.Now,
                PhoneNumber = "012/123-123",
                IsDelivered = false,
                ProductOrders = new List<OutputProductOrderDTO>()
                {
                    new OutputProductOrderDTO()
                    {
                        Details = new OutputShopProductServingDTO()
                        {
                            ProductId = 1,
                            ProductName = "Pica",
                            Price = 10,
                            ServingName= "Velika"
                        },
                        Price = 10,
                        TotalAmount = 2
                    },
                    new OutputProductOrderDTO()
                    {
                        Details = new OutputShopProductServingDTO()
                        {
                            ProductName = "Banjalucki Kraft",
                            Price = 5,
                            ServingName = "0.33l",
                            ProductId = 2
                        },
                        Price = 5,
                        TotalAmount = 4
                    }
                }
            };
            await ManagersFactory.GetEmailSender().EmailOrder(outputOrder);
        }
    }
}
