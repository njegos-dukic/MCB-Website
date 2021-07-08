using System.Collections.Generic;

namespace Core.Entity
{
    public class Company
    {
        public int CompanyId { get; set; }

        public string ApiKey { get; set; }

        public string Name { get; set; }

        public string Address { get; set; }

        public string PostalCode { get; set; }

        public string Coordinates { get; set; }

        public string Email { get; set; }

        public string Fax { get; set; }

        public string ShopDescription { get; set; }

        #region NavigationProperties

        public ICollection<SocialMedia> SocialMedias { get; set; }

        public ICollection<Wholesale> Wholesales { get; set; }

        public ICollection<Menu> Menus { get; set; }

        public ICollection<Phone> Phones { get; set; }

        public ICollection<Administrator> Administrators { get; set; }

        public ICollection<Product> Products { get; set; }

        public ICollection<Gallery> Galleries { get; set; }

        public ICollection<Quote> Quotes { get; set; }

        public ICollection<Event> Events { get; set; }
        #endregion
    }
}
