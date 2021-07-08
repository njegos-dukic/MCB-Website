using System.Collections.Generic;

namespace MasterCraftBreweryAPI.Wrapper.Company
{
    public class DetailedCompanyPutWrapper
    {
        /// <summary>
        /// Unique name of a company
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Address of a company
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// Postal code for the company
        /// </summary>
        public string PostalCode { get; set; }

        /// <summary>
        /// Coordinates that specify company's location
        /// </summary>
        public string Coordinates { get; set; }

        /// <summary>
        /// Email for a company
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Fax for a company
        /// </summary>
        public string Fax { get; set; }

        /// <summary>
        /// Online shop description
        /// </summary>
        public string ShopDescription { get; set; }

        /// <summary>
        /// List of social medias related to the company
        /// </summary>
        public List<SocialMediaWrapper> SocialMedias { get; set; }

        /// <summary>
        /// List of wholesales related to the company
        /// </summary>
        public List<WholesaleWrapper> Wholesales { get; set; }

        /// <summary>
        /// List of company's phone numbers
        /// </summary>
        public List<PhoneWrapper> Phones { get; set; }
    }
}
