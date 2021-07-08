using Core.Entity;

namespace MasterCraftBreweryAPI.Wrapper.Company
{
    public class SocialMediaWrapper
    {
        /// <summary>
        /// Url for the social media
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// Type of the social media as an number
        /// [0 - Facebook, 1 - Twitter, 2 - LinkedIn, 3 - Instagram, 4 - YouTube, 5 -  OLX]
        /// </summary>
        public SocialMediaType Type { get; set; }
    }
}
