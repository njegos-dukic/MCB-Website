using Core.AutoMapper;
using Core.DTO;
using Core.DTO.Menu;
using Core.Entity;
using Core.Util;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
namespace Tests.Common
{
    public class DataPool
    {
        #region Constants
        public const int MockAndReplaceLaterId = 1;

        public const int WrongPositiveId = 100000;

        public const int WrongNegativeId = -1;

        public const string WrongEmail = "mcb-project.gmail.com";

        public const string ValidEmail = "mcb-project@gmail.com";

        public const string WrongName = "asfasfasf";

        public const string ProductImage = "Common/Data/Images/MasterPlate.jpg";

        public static readonly DateTime DateTimeInPast = new DateTime(2019, 11, 10, 10, 0, 0);

        #endregion

        private readonly MasterCraftBreweryContext context;

        public DataPool(MasterCraftBreweryContext context) => this.context = context;

        public DetailedCompanyDTO ExistingCompany { get; private set; }

        public AdministratorDTO Administrator { get; private set; }

        public InputProductDTO ExistingProduct { get; private set; }

        public MenuDTO ExistingMenu { get; private set; }

        public List<MenuDTO> InputMenus { get; private set; }

        public List<ServingDTO> Servings { get; private set; }

        public List<ProductTypeDTO> ProductTypes { get; private set; }

        public List<OutputProductDTO> Products { get; private set; }

        public List<InputProductDTO> InputProducts { get; private set; }

        public List<QuoteDTO> InputQuotes { get; private set; }

        public QuoteDTO ExistingQuote { get; private set; }

        public List<EventDTO> Events { get; private set; }

        public List<GalleryBaseDTO> InputGalleries { get; set; }

        public GalleryBaseDTO ExistingGallery { get; set; }

        #region NotAddedToDatabase
        public List<SocialMediaDTO> SocialMediaDTOs { get; private set; }

        public List<WholesaleDTO> WholesaleDTOs { get; private set; }

        public List<int> ProductServingIds { get; private set; }
        #endregion

        public void ImportFromJson()
        {
            SocialMediaDTOs = DataGenerator.Deserialize<List<SocialMediaDTO>>("SocialMedias.json");
            WholesaleDTOs = DataGenerator.Deserialize<List<WholesaleDTO>>("Wholesales.json");
            ExistingCompany = DataGenerator.Deserialize<DetailedCompanyDTO>("ExistingCompany.json");
            InputProducts = DataGenerator.Deserialize<List<InputProductDTO>>("Products.json");
            ProductTypes = DataGenerator.Deserialize<List<ProductTypeDTO>>("ProductTypes.json");
            Servings = DataGenerator.Deserialize<List<ServingDTO>>("Servings.json");
            ExistingProduct = DataGenerator.Deserialize<InputProductDTO>("ExistingProduct.json");
            InputMenus = DataGenerator.Deserialize<List<MenuDTO>>("Menus.json");
            ExistingMenu = DataGenerator.Deserialize<MenuDTO>("ExistingMenu.json");
            Administrator = DataGenerator.Deserialize<AdministratorDTO>("RootAdministrator.json");
            InputQuotes = DataGenerator.Deserialize<List<QuoteDTO>>("Quotes.json");
            ExistingQuote = DataGenerator.Deserialize<QuoteDTO>("ExistingQuote.json");
            Events = DataGenerator.Deserialize<List<EventDTO>>("Events.json");
            InputGalleries = DataGenerator.Deserialize<List<GalleryBaseDTO>>("Galleries.json");
            ExistingGallery = DataGenerator.Deserialize<GalleryBaseDTO>("ExistingGallery.json");
        }

        /// <summary>
        /// Imports data from JSON files, converts this data to 
        /// entity objects and saves those objects to database.
        /// </summary>
        public void ExportToDatabase()
        {
            ImportFromJson();
            Company company = (Map<DetailedCompanyDTO, Company>(ExistingCompany));
            company.ApiKey = "0f8fad5b-d9cb-469f-a165-70867728950e";
            context.Companies.Add(company);
            context.ProductTypes.AddRange(Map<ProductTypeDTO, ProductType>(ProductTypes));
            context.Servings.AddRange(Map<ServingDTO, Serving>(Servings));
            context.SaveChanges();
            ExistingCompany = Map<Company, DetailedCompanyDTO>(context.Companies.First());


            ExpordAdministrator();
            ExportProducts();
            ExportMenu();
            ExportEvents();

            ExportQuotes();
            ExportGalleries();
        }

        private void ExportEvents()
        {
            List<Event> events = Map<EventDTO, Event>(Events);
            events.ForEach(x => x.CompanyId = ExistingCompany.CompanyId);
            context.Events.AddRange(events);
            context.SaveChanges();
            Events = Map<Event, EventDTO>(context.Events.ToList());
        }

        private void ExpordAdministrator()
        {
            Administrator admin = new Administrator()
            {
                Email = Administrator.Email,
                CompanyId = ExistingCompany.CompanyId
            };

            (admin.Salt, admin.Password) = Security.ComputePassword(Administrator.Password);
            context.Administrators.Add(admin);
            context.SaveChanges();
            Administrator = Map<Administrator, AdministratorDTO>(context.Administrators.First());
        }

        private void ExportProducts()
        {
            ProductTypes = Map<ProductType, ProductTypeDTO>(context.ProductTypes.ToList());
            Servings = Map<Serving, ServingDTO>(context.Servings.ToList());
            InputProducts.Add(ExistingProduct);
            InputProducts.ForEach(product =>
            {
                List<int> servingIds = Servings.Select(x => x.ServingId).ToList();
                foreach (ProductServingDTO productServing in product.ProductServings)
                {
                    productServing.ServingId = servingIds.Random();
                    servingIds.Remove(productServing.ServingId);
                }
                product.ProductTypeId = ProductTypes.Random().ProductTypeId;
            });
            List<Product> products = Map<InputProductDTO, Product>(InputProducts);
            products.ForEach(product => product.CompanyId = ExistingCompany.CompanyId);
            context.Products.AddRange(products);
            context.SaveChanges();

            Products = Map<Product, OutputProductDTO>(context.Products.ToList());
            ExistingProduct.ProductId = context.Products.Single(x => x.Name == ExistingProduct.Name).ProductId;
            ExistingProduct.ProductServings = Map<ProductServing, ProductServingDTO>(context.ProductServings
                                                                                            .AsQueryable()
                                                                                            .Where(x => x.ProductId == ExistingProduct.ProductId)
                                                                                            .ToList());
            ProductServingIds = Products.SelectMany(x => x.ProductServings).Select(x => x.ProductServingId).ToList();
        }

        private void ExportGalleries()
        {
            Gallery gallery = Map<GalleryBaseDTO, Gallery>(ExistingGallery);
            List<Gallery> galleries = Map<GalleryBaseDTO, Gallery>(InputGalleries);
            galleries.Add(gallery);
            galleries.ForEach(gallery =>
                            {
                                gallery.CompanyId = ExistingCompany.CompanyId;
                                gallery.CreatedOn = DateTime.UtcNow;
                            });
            context.Galleries.AddRange(galleries);
            context.SaveChanges();

            ExistingGallery.GalleryId = context.Galleries.Single(g => g.Name == ExistingGallery.Name).GalleryId;
            ExistingGallery.CreatedOn = context.Galleries.Single(g => g.Name == ExistingGallery.Name).CreatedOn;
            InputGalleries = Map<Gallery, GalleryBaseDTO>(context.Galleries.ToList());
        }

        private void ExportQuotes()
        {
            Quote quote = Map<QuoteDTO, Quote>(ExistingQuote);

            List<Quote> quotes = Map<QuoteDTO, Quote>(InputQuotes);
            quotes.Add(quote);
            quotes.ForEach(quote =>
                            {
                                quote.CompanyId = ExistingCompany.CompanyId;
                                quote.CreatedOn = DateTime.UtcNow;
                            });

            context.Quotes.AddRange(quotes);
            context.SaveChanges();

            ExistingQuote.QuoteId = context.Quotes.Single(q => q.QuoteText == ExistingQuote.QuoteText && q.Author == ExistingQuote.Author).QuoteId;
            ExistingQuote.CreatedOn = context.Quotes.Single(q => q.QuoteText == ExistingQuote.QuoteText && q.Author == ExistingQuote.Author).CreatedOn;
            InputQuotes = Map<Quote, QuoteDTO>(context.Quotes.ToList());
        }

        private void ExportMenu()
        {
            // Relationship with servings of the products
            List<Menu> menus = Map<MenuDTO, Menu>(InputMenus);
            menus.Add(Map<MenuDTO, Menu>(ExistingMenu));
            menus.ForEach(menu =>
            {
                foreach (MenuItem menuItem in menu.MenuItems)
                    menuItem.ProductServingId = ProductServingIds.Random();
            });
            menus.ForEach(x => x.CompanyId = ExistingCompany.CompanyId);
            context.Menus.AddRange(menus);
            context.SaveChanges();
            ExistingMenu = Map<Menu, MenuDTO>(context.Menus.Include(x => x.MenuItems)
                                                           .ThenInclude(x => x.ProductServing)
                                                           .ThenInclude(x => x.Product)
                                                           .Single(x => x.Name == ExistingMenu.Name));
            InputMenus = Map<Menu, MenuDTO>(context.Menus.Include(x => x.MenuItems)
                                                           .ThenInclude(x => x.ProductServing)
                                                           .ThenInclude(x => x.Product)
                                                           .ToList());
        }

        private Mapped Map<Original, Mapped>(Original original)
            => Mapping.Mapper.Map<Original, Mapped>(original);

        private List<Mapped> Map<Original, Mapped>(List<Original> original)
            => Mapping.Mapper.Map<List<Original>, List<Mapped>>(original);
    }
}
