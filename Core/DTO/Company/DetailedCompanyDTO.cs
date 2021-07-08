using System.Collections.Generic;

namespace Core.DTO
{
    public class DetailedCompanyDTO : CompanyDTO
    {
        public List<SocialMediaDTO> SocialMedias { get; set; }

        public List<WholesaleDTO> Wholesales { get; set; }

        public List<PhoneDTO> Phones { get; set; }
    }
}
