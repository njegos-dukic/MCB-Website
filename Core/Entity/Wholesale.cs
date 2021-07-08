namespace Core.Entity
{
    public class Wholesale
    {
        public int WholesaleId { get; set; }

        public int CompanyId { get; set; }

        public string Name { get; set; }

        public string Address { get; set; }

        public string Coordinates { get; set; }

        #region NavigationProperties

        public Company Company { get; set; }

        #endregion
    }
}
