namespace Core.Entity
{
    public class Phone
    {
        public int PhoneId { get; set; }

        public string PhoneNumber { get; set; }

        public int CompanyId { get; set; }

        #region NavigationProperties

        public Company Company { get; set; }
        #endregion
    }
}
