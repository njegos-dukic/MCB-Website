namespace Core.Entity
{
    public class Administrator
    {
        public int AdministratorId { get; set; }

        public int CompanyId { get; set; }

        public string Email { get; set; }

        public byte[] Password { get; set; }

        public byte[] Salt { get; set; }

        #region NavigationProperties

        public Company Company { get; set; }

        #endregion
    }
}
