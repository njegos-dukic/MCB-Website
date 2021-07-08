namespace Core.Entity
{
    public class SocialMedia
    {
        public int SocialMediaId { get; set; }

        public int CompanyId { get; set; }

        public string Url { get; set; }

        public SocialMediaType Type { get; set; }

        #region NavigationProperties

        public Company Company { get; set; }

        #endregion
    }
}
