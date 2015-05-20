using HtmlAgilityPack;
using Spider.Data;
using Spider.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Spider.Engine
{
    internal class InfoParser
    {
        const string currentDomain = "http://www.somedomain";
        private static void ParsingCategoriesPage(SpiderContext db, WebClient wb)
        {
            HtmlDocument docu = Utilizer.LoadCurrentHtmlDocument(wb, currentDomain);
            var mainContainer = docu.DocumentNode.SelectSingleNode("//div[@id='content_slider_links']");
            var shopCategoriesLinks = mainContainer.Descendants().Where(x => x.Name == "a").ToList();
            for (int i = 0; i < shopCategoriesLinks.Count; i++)
            {
                if (shopCategoriesLinks[i].InnerText.Trim().Contains("New Arrivals") || shopCategoriesLinks[i].InnerText.Trim().Contains("Sale"))
                { continue; }

                var category = new Category()
                {
                    Name = shopCategoriesLinks[i].InnerText.Trim()
                };
                SetXmlCategory(category);
                db.Categories.Add(category);
                db.SaveChanges();
                var hrefLinkHolder = shopCategoriesLinks[i].GetAttributeValue("href", "000");
                HtmlDocument categoryPage = Utilizer.LoadCurrentHtmlDocument(wb, hrefLinkHolder);
                ExtractProductsCategoriesPage(categoryPage, wb, category, db);
            }
        }
        private static void SetXmlCategory(Category category)
        {
            if (category.Name.Contains("Cosmetic"))
            {
                category.CategoryXmlId = "accessories";
            }
            else if (category.Name.Contains("Business"))
            {
                category.CategoryXmlId = "business";
            }
            else if (category.Name.Contains("Luggage"))
            {
                category.CategoryXmlId = "luggage";
            }
            else if (category.Name.Contains("Bags"))
            {
                category.CategoryXmlId = "bags";
            }
        }
        private static void ExtractProductsCategoriesPage(HtmlDocument categoryPage, WebClient wb, Category category, SpiderContext db)
        {
            HtmlNode mainProductContainer = categoryPage.DocumentNode.Descendants().FirstOrDefault(x => x.Name == "div"
                           && x.Attributes["class"] != null && x.GetAttributeValue("class", "000") == "category-products");
            var productLinkTiles = mainProductContainer.Descendants().Where(x => x.Name == "li"
                           && x.Attributes["class"] != null && x.GetAttributeValue("class", "000").Contains("item")).ToList();

            for (int k = 0; k < productLinkTiles.Count; k++)
            {
                string id = GetProductId(productLinkTiles[k].GetAttributeValue("class", "000").Trim());
                string name = productLinkTiles[k].Descendants().FirstOrDefault(x => x.Name == "a"
                && x.Attributes["class"] != null && x.GetAttributeValue("class", "000").Contains("product-name")).InnerText.Trim();
                if (db.MainProducts.Find(id) != null)
                {
                    continue;
                }
                var product = new MainProduct()
                {
                    MainProductId = id,
                    ProductName = name,
                    Category = category,
                    CategoryId = category.CategoryId,
                };
                HtmlNode actions = productLinkTiles[k].Descendants().FirstOrDefault(x => x.Name == "div"
                && x.Attributes["class"] != null && x.GetAttributeValue("class", "000").Contains("actions"));
                db.MainProducts.Add(product);
                db.SaveChanges();
                SetAvailableColors(product, actions, db);
                HtmlNode picNode = productLinkTiles[k].Descendants().FirstOrDefault(x => x.Name == "a"
               && x.Attributes["class"] != null && x.GetAttributeValue("class", "000").Contains("product-image"))
               .Descendants().FirstOrDefault(x => x.Name == "img");
                var imgSource = picNode.GetAttributeValue("src", "000");
                var mediumPath = @"C:\Users\Rumen\Desktop\sample\medium\" + product.MainProductId + ".jpg";
                PicturesManipulator.DownloadPictureAndAddPicturePath(product, db, mediumPath, imgSource);
                ParsePDP(wb, productLinkTiles, k, product, db);
                PicturesManipulator.GetSetAllPDPPictures(product, db, wb);
            }
        }

        internal static VariationColor AddNewSlaveProduct(MainProduct product, int colorId, SpiderContext db)
        {

            var color = db.Colors.Find(colorId);
            var slaveProduct = new SlaveProduct()
            {
                SlaveProductId = product.MainProductId + "-" + color.ColorName.ToLower(),
                MainProduct = product,
                MainProductId = product.MainProductId,
                VariationColor = color,
                VariationColorId = colorId
            };
            db.SlaveProducts.Add(slaveProduct);
            db.SaveChanges();
            return color;
        }

        private static void SetAvailableColors(MainProduct product, HtmlNode actions, SpiderContext db)
        {
            string[] stringSeparators = new string[] { "-", " " };
            string[] result;
            var liOptions = actions.Descendants().Where(x => x.Name == "li"
                           && x.Attributes["class"] != null && x.GetAttributeValue("class", "000").Contains("colorswatch"))
                          .Select(x => x.GetAttributeValue("class", "000")).ToList();
            for (int i = 0; i < liOptions.Count; i++)
            {
                result = liOptions[i].Split(stringSeparators, StringSplitOptions.RemoveEmptyEntries);
                var colorCode = result[2].Trim();
                var color = db.Colors.Find(Int32.Parse(colorCode));
                product.Colors.Add(color);
            }
            db.SaveChanges();
        }

        private static string GetProductId(string idCont)
        {
            string[] stringSeparators = new string[] { "-", };
            string[] result;
            result = idCont.Split(stringSeparators, StringSplitOptions.RemoveEmptyEntries);
            string pid = result[result.Length - 1];
            return pid;
        }
        private static void ParsePDP(WebClient wb, List<HtmlNode> productLinkTiles, int k, MainProduct product, SpiderContext db)
        {
            var ttt = productLinkTiles[k].Descendants().FirstOrDefault(x => x.Name == "a" && x.Attributes["class"] != null
                && x.GetAttributeValue("class", "000") == "product-name").GetAttributeValue("href", "000");
            HtmlDocument productDetailsPage = Utilizer.LoadCurrentHtmlDocument(wb, ttt);

            HtmlNode pdpInfo = productDetailsPage.DocumentNode.Descendants()
                .FirstOrDefault(x => x.Name == "div" & x.Attributes["class"] != null && x.GetAttributeValue("class", "000") == "short-description");
            if (pdpInfo != null && pdpInfo.InnerText != null) { product.ShortDesc = pdpInfo.InnerText.Trim(); }
            else
            {
                product.ShortDesc = "None";

            }
            db.SaveChanges();
        }
    }
}
