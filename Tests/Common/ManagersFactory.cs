
using Core.Entity;
using Core.Managers;
using Core.Util;
using Tests.Configuration;

namespace Tests.Common
{
    /// <summary>
    /// Factory class that produces managers
    /// </summary>
    public static class ManagersFactory
    {
        public static IEmailSender GetEmailSender()
            => new EmailSender();

        public static IApiKeyManager GetApiKeyManager(MasterCraftBreweryContext context)
            => new ApiKeyManager(TestConfigurationManager.GetValue("apiKey"), context);
        public static ICompanyManager GetCompanyManager(MasterCraftBreweryContext context)
            => new CompanyManager(GetApiKeyManager(context), context);

        public static IProductManager GetProductManager(MasterCraftBreweryContext context)
            => new ProductManager(GetFileManager(), GetApiKeyManager(context), context);

        public static IFileManager GetFileManager()
            => new LocalFileManager(TestConfigurationManager.GetValue("fileStorageLocation"));

        public static IMenuManager GetMenuManager(MasterCraftBreweryContext context)
            => new MenuManager(GetApiKeyManager(context), context);

        public static IQuoteManager GetQuoteManager(MasterCraftBreweryContext context)
            => new QuoteManager(GetApiKeyManager(context), context);

        public static IGalleryManager GetGalleryManager(MasterCraftBreweryContext context)
            => new GalleryManager(GetFileManager(), GetApiKeyManager(context), context);

        public static IEventManager GetEventManager(MasterCraftBreweryContext context)
            => new EventManager(GetApiKeyManager(context), GetFileManager(), context);
    }
}
