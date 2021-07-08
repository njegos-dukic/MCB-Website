namespace MasterCraftBreweryAPI.Wrapper.Company
{
    public class CompanyDeleteWrapper
    {
        /// <summary>
        /// Is company deleted?
        /// </summary>
        public bool IsDeleted { get; }

        public CompanyDeleteWrapper(bool isDeleted)
        {
            this.IsDeleted = isDeleted;
        }
    }
}
