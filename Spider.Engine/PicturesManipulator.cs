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
    public class PicturesManipulator
    {
        internal static void DownloadPictureAndAddPictureSmallPath(MainProduct product, SpiderContext db, string smallPath, string imgSource)
        {
            string localPath = new Uri(smallPath).LocalPath;
            using (WebClient wc = new WebClient())
                wc.DownloadFile(imgSource, localPath);
        }
        internal static void DownloadPictureAndAddPictureNormalPath(string normalPath, string imgSource)
        {
            if (imgSource.Contains("thumbnail"))
            {
                int starInd = imgSource.IndexOf("thumbnail/") + 10;
                int endInd = imgSource.IndexOf("/", starInd) + 1;
                string res = imgSource.Substring(0, imgSource.IndexOf("thumbnail/"));
                string res1 = imgSource.Substring(endInd);
                string allOk = res + "image/" + res1;
                string localPath = new Uri(normalPath).LocalPath;
                using (WebClient wc = new WebClient())
                    wc.DownloadFile(allOk, localPath);
            }
            else
            {
                int starInd = imgSource.IndexOf("image/") + 6;
                int endInd = imgSource.IndexOf("/", starInd) + 1;
                string res = imgSource.Substring(0, starInd);
                string res1 = imgSource.Substring(endInd);
                string allOk = res + res1;
                string localPath = new Uri(normalPath).LocalPath;
                using (WebClient wc = new WebClient())
                    wc.DownloadFile(allOk, localPath);
            }
        }
        internal static void DownloadPictureAndAddPicturePath(MainProduct product, SpiderContext db, string pathSource, string imageSourcePath)
        {
            string localPath = new Uri(pathSource).LocalPath;
            using (WebClient wc = new WebClient())
                wc.DownloadFile(imageSourcePath, localPath);
            var mediumPath = new PicturePath()
            {
                Content = "medium/" + product.MainProductId + ".jpg",
                Product = product,
                ProductId = product.MainProductId,
                VariationColor = db.Colors.Find(1),
                VariationColorId = 1
            };
            db.Paths.Add(mediumPath);
            db.SaveChanges();
        }
        internal static void GetSetAllPDPPictures(MainProduct product, SpiderContext db, WebClient wb)
        {
            string URI = "http://www.someuri";
            string myParameters = null;
            var colors = product.Colors.ToList();
            string HtmlResult = null;
            for (int i = 0; i < colors.Count; i++)
            {
                myParameters = @"attribute_id=128&option_id=" + colors[i].VariationColorId + @"&product_id=" + product.MainProductId + @"&selection=[{""attribute_id"":""128"",""option_id"":""" + colors[i].VariationColorId + @""",""order"":0,""selected"":true}]&img_width=636&img_height=636&image_selector=%23image";
                var uri = new Uri(URI);
                var servicePoint = ServicePointManager.FindServicePoint(uri);
                servicePoint.Expect100Continue = false;
                var webClient = new WebClient();
                IWebProxy defaultProxy = WebRequest.DefaultWebProxy;
                if (defaultProxy != null)
                {
                    defaultProxy.Credentials = CredentialCache.DefaultCredentials;
                    webClient.Proxy = defaultProxy;
                }
                System.Net.ServicePointManager.Expect100Continue = false;
                webClient.Headers[HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded";
                HtmlResult = webClient.UploadString(URI, myParameters);
                int startIndex = HtmlResult.IndexOf("$(li).update");
                int firstImageStart = 0;
                int firstImageEnd = 0;
                string firstLink = null;
                List<string> smallLinks = new List<string>();
                while (true)
                {
                    firstImageStart = HtmlResult.IndexOf("<img", startIndex) + 11;
                    if (HtmlResult.IndexOf("<img", startIndex) < 0) { break; }
                    firstImageEnd = HtmlResult.IndexOf(".jpg", firstImageStart) + 4;
                    firstLink = HtmlResult.Substring(firstImageStart, firstImageEnd - firstImageStart);
                    startIndex = firstImageEnd;
                    smallLinks.Add(firstLink);
                }

                SetPicturesParticularColor(smallLinks, product, colors[i].VariationColorId, db);
            }
        }
        private static void SetPicturesParticularColor(List<string> smallLinks, MainProduct product, int colorId, SpiderContext db)
        {
            var color = InfoParser.AddNewSlaveProduct(product, colorId, db);
            for (int i = 0; i < smallLinks.Count; i++)
            {
                var path = new PicturePath()
                {
                    Content = "small/" + product.MainProductId + "-" + color.ColorName.ToLower() + "-p" + i + ".jpg",
                    Product = product,
                    ProductId = product.MainProductId,
                    VariationColor = color,
                    VariationColorId = color.VariationColorId
                };
                var normalPathobj = new PicturePath()
                {
                    Content = "normal/" + product.MainProductId + "-" + color.ColorName.ToLower() + "-p" + i + ".jpg",
                    Product = product,
                    ProductId = product.MainProductId,
                    VariationColor = color,
                    VariationColorId = color.VariationColorId
                };
                db.Paths.Add(path);
                db.Paths.Add(normalPathobj);
                db.SaveChanges();
                var smallPath = @"C:\Users\Rumen\Desktop\sample\small\" + product.MainProductId + "-" + color.ColorName.ToLower() + "-p" + i + ".jpg";
                PicturesManipulator.DownloadPictureAndAddPictureSmallPath(product, db, smallPath, smallLinks[i]);
                var normalPath = @"C:\Users\Rumen\Desktop\sample\normal\" + product.MainProductId + "-" + color.ColorName.ToLower() + "-p" + i + ".jpg";
                PicturesManipulator.DownloadPictureAndAddPictureNormalPath(normalPath, smallLinks[i]);
            }
        }
    }
}

