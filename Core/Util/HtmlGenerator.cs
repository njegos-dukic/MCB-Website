using Core.DTO;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Core.Util
{
    /// <summary>
    /// Builds HTML. 
    /// </summary>
    public static class HtmlGenerator
    {
        /// <summary>
        /// Based on HTML specified in configuration file, generates table with ordered products.
        /// </summary>
        /// <param name="items">Ordered products</param>
        /// <param name="htmlTableFormat">File name for the .json file that contains information neccessary 
        /// for generating HTML table</param>
        /// <returns></returns>
        public static string GenerateTable(List<OutputProductOrderDTO> items, string htmlTableFormat)
        {
            OrderTableConfiguration htmlConfig = JsonConvert.DeserializeObject<OrderTableConfiguration>
                                                (StreamUtil.GetManifestResourceString(htmlTableFormat));
            return string.Format(htmlConfig.TableTagFormat, htmlConfig.TableColumns, items.Select(item =>
            {
                string rowContent = string.Format(htmlConfig.TableRowContentFormat, item.Details.ProductName,
                                                                         item.Details.ServingName,
                                                                         item.TotalAmount,
                                                                         item.Price * item.TotalAmount);
                return string.Format(htmlConfig.TableRowTagFormat, rowContent);
            }).Aggregate((first, second) => first + second));
        }

        /// <summary>
        /// Creates body of an email using HTML template.
        /// </summary>
        /// <param name="contactInfo">Contact message and the information about the sender</param>
        /// <param name="htmlFormat">File name of the HTML template used for the body</param>
        /// <returns></returns>
        public static string CreateContactEmailBody(ContactDTO contactInfo, string htmlFormat)
        {
            using Stream stream = StreamUtil.GetManifestResourceStream(htmlFormat);
            using StreamReader reader = new StreamReader(stream);
            string body = reader.ReadToEnd();
            body = string.Format(@body, contactInfo.FirstName
                                                    , contactInfo.LastName, contactInfo.Email
                                                    , contactInfo.Message, string.Empty);
            return body;
        }

        /// <summary>
        /// Creates body of an email using HTML template.
        /// </summary>
        /// <param name="orderInfo">Information about the order</param>
        /// <param name="htmlFormat">File name of the HTML template used for body</param>
        /// <param name="htmlTableFormat">File name of the HTML template used for table of ordered products</param>
        /// <returns></returns>
        public static string CreateOrderEmailBody(OutputOrderDTO orderInfo, string htmlFormat, string htmlTableFormat)
        {
            using Stream stream = StreamUtil.GetManifestResourceStream(htmlFormat);
            using StreamReader reader = new StreamReader(stream);
            string body = reader.ReadToEnd();
            body = string.Format(@body, orderInfo.FullName, orderInfo.Email
                                      , orderInfo.PhoneNumber, orderInfo.CountryName
                                      , orderInfo.City, orderInfo.Street, orderInfo.PostalCode
                                      , orderInfo.DeliveryCost, orderInfo.TotalCost
                                      , GenerateTable(orderInfo.ProductOrders, htmlTableFormat), string.Empty);
            return body;
        }
    }
}
