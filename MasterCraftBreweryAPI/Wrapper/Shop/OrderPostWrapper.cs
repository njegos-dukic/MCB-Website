using System.Collections.Generic;

namespace MasterCraftBreweryAPI.Wrapper.Shop
{
    public class OrderPostWrapper
    {
        public string FullName { get; set; }

        public string PhoneNumber { get; set; }

        public string Email { get; set; }

        public string CountryName { get; set; }

        public string City { get; set; }

        public string Street { get; set; }

        public string PostalCode { get; set; }

        public double DeliveryCost { get; set; }

        public List<ProductOrderWrapper> ProductOrders { get; set; }
    }
}
