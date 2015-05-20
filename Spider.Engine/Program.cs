using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Spider.Data.Migrations;
using System.Data.Entity;
using Spider.Data;
using Spider.Models;
using System.IO;
using System.Drawing;
using System.Xml.Linq;

namespace Spider.Engine
{
    public class Program
    {
       




static void Main(string[] args)
{
    WebClient wb = new WebClient();
    SpiderContext db = new SpiderContext();
    var products = db.MainProducts.ToList();
    XmlRenderer.CreateCatalog(db, products);
}

    
    }
}
