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
    internal class Utilizer
    {
        private static void AddColorInputs(SpiderContext db)
        {

            db.Colors.Add(new VariationColor() { VariationColorId = 5, ColorName = "Black", });
            db.Colors.Add(new VariationColor() { VariationColorId = 6, ColorName = "Red", });
            db.Colors.Add(new VariationColor() { VariationColorId = 4, ColorName = "Grey", });
            db.Colors.Add(new VariationColor() { VariationColorId = 7, ColorName = "Purple", });
            db.Colors.Add(new VariationColor() { VariationColorId = 9, ColorName = "Aqua", });
            db.Colors.Add(new VariationColor() { VariationColorId = 18, ColorName = "Navy", });

            db.Colors.Add(new VariationColor() { VariationColorId = 16, ColorName = "Tangerine", });
            db.Colors.Add(new VariationColor() { VariationColorId = 17, ColorName = "Espresso", });
            db.Colors.Add(new VariationColor() { VariationColorId = 15, ColorName = "Green", });
            db.Colors.Add(new VariationColor() { VariationColorId = 14, ColorName = "Fuschia", });
            db.Colors.Add(new VariationColor() { VariationColorId = 3, ColorName = "Chocolate", });
            db.Colors.Add(new VariationColor() { VariationColorId = 1, ColorName = "Default", });
            db.SaveChanges();
        }
       internal static HtmlDocument LoadCurrentHtmlDocument(WebClient wc, string currentMatchURI)
        {
            var responseData = wc.DownloadData(currentMatchURI);
            String source = Encoding.GetEncoding("utf-8").GetString(responseData, 0, responseData.Length - 1);
            source = WebUtility.HtmlDecode(source);
            HtmlDocument documentResult = new HtmlDocument();
            documentResult.LoadHtml(source);
            return documentResult;
        }
    }
}
