using System;

namespace Core.DTO
{
    public class BaseOrderDTO
    {
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
    }
}
