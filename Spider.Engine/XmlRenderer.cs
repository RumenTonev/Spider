using Spider.Data;
using Spider.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Spider.Engine
{
   internal class XmlRenderer
    {
        internal static void CreateCatalog(SpiderContext db, List<MainProduct> products)
        {
            XDocument document = new XDocument(
                new XDeclaration("1.0", "UTF-8", ""), new XElement("catalog",
                     new XAttribute("catalog-id", "sample-catalog"),
                        new XElement("header")
                        )
                );
            document.Root.Add(new XElement("category", new XAttribute("category-id", "root"),

                           new XElement("display-name", "Sample Catalog", new XAttribute(XNamespace.Xml + "lang", "x-default")),
                           new XElement("online-flag", true),

                           new XElement("template"),
                           new XElement("page-attributes")));
            var categories = db.Categories.Where(x => x.CategoryXmlId != "root").ToList();
            AttachCategoriesToXml(document, categories);
            AttachProductsToXmlCatalog(products, document);
            document.Save("sapmleCatalogOK.xml");
        }
        private static void AttachProductsToXmlCatalog(List<MainProduct> products, XDocument document)
        {
            for (int i = 0; i < products.Count; i++)
            {
                AddMainProductXMLBody(products, document, i);
                AttachImageGroups(products, document, i);
            };
        }
        private static void AddMainProductXMLBody(List<MainProduct> products, XDocument document, int i)
        {
            document.Root.Add(new XElement("product", new XAttribute("product-id", products[i].MainProductId),
                 new XElement("ean"),
                 new XElement("upc"),
                 new XElement("unit"),
                  new XElement("min-order-quantity", 1),
                   new XElement("step-quantity", 1),
                       new XElement("display-name", products[i].ProductName, new XAttribute(XNamespace.Xml + "lang", "x-default")),
                          new XElement("short-description", products[i].ShortDesc, new XAttribute(XNamespace.Xml + "lang", "x-default")),
                        new XElement("online-flag", true),
                         new XElement("available-flag", true),
                          new XElement("searchable-flag", true),
                        new XElement("images"),
                         new XElement("tax-class-id", "standard"),
                           new XElement("page-attributes"),
                             new XElement("custom-attributes")
                              ));
        }
        private static void AttachImageGroups(List<MainProduct> products, XDocument document, int i)
        {
            var picturePaths = products[i].PicturesPaths.ToList();
            var currentProductObject = document.Root.Elements().FirstOrDefault(x => x.HasAttributes
                && x.Attribute("product-id") != null && x.Attribute("product-id").Value == products[i].MainProductId);
            AttachSingleImgGroupByType(picturePaths, currentProductObject, "large");
            AttachSingleImgGroupByType(picturePaths, currentProductObject, "medium");
            AttachSingleImgGroupByType(picturePaths, currentProductObject, "small");
        }
        private static void AttachSingleImgGroupByType(List<PicturePath> picturePaths, XElement currentProductObject, string viewType)
        {
            var imgGroupXMLElement = new XElement("image-group", new XAttribute("view-type", viewType));
            List<PicturePath> viewTypePaths = new List<PicturePath>();
            if (viewType == "large")
            {
                viewTypePaths = picturePaths.Where(x => x.Content.Contains("_large")).ToList();
            }
            else if (viewType == "small")
            {
                viewTypePaths = picturePaths.Where(x => x.Content.Contains("_small")).ToList();
            }
            else
            {
                viewTypePaths = picturePaths.Where(x => !x.Content.Contains("_small") && !x.Content.Contains("_large")).ToList();
            }
            if (viewTypePaths.Count > 0)
            {

                for (int k = 0; k < viewTypePaths.Count; k++)
                {
                    imgGroupXMLElement.Add(new XElement("image", new XAttribute("path", viewTypePaths[k].Content)));
                }
            }
            currentProductObject.Element("images").Add(imgGroupXMLElement);
        }
        private static void AttachCategoriesToXml(XDocument document, List<Category> categories)
        {
            for (int i = 0; i < categories.Count; i++)
            {
                document.Root.Add(new XElement("category", new XAttribute("category-id", categories[i].CategoryXmlId),

                            new XElement("display-name", categories[i].Name, new XAttribute(XNamespace.Xml + "lang", "x-default")),
                            new XElement("online-flag", true),
                            new XElement("parent", "root"),
                            new XElement("template"),
                            new XElement("page-attributes"),
                     new XElement("attribute-groups", new XElement("custom-attribute", true,
                                new XAttribute("attribute-id", "showInMenu")))
                            ));
            };
        }

    }
}
