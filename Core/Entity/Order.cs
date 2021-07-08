using System;
using System.Collections.Generic;

namespace Core.Entity
{
    public class Order
    {
        public int OrderId { get; set; }

        public int CompanyId { get; set; }

        public DateTime OrderedOn { get; set; }

        public bool IsDelivered { get; set; }

        public string FullName { get; set; }

        public string PhoneNumber { get; set; }

        public string Email { get; set; }

        public string CountryName { get; set; }

        public string City { get; set; }

        public string Street { get; set; }

        public string PostalCode { get; set; }

        public double DeliveryCost { get; set; }

        public double TotalCost { get; set; }

        #region NavigationProperties
        public Company Company { get; set; }

        public ICollection<ProductOrder> ProductOrders { get; set; }
        #endregion
    }
}
