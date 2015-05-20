using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spider.Models
{
    public class Category
    {
        public Category()
        {
            this.Products = new HashSet<MainProduct>();
        }
        public int CategoryId { get; set; }
        public string CategoryXmlId { get; set; }     
        public string Name { get; set; }                    
        public virtual ICollection<MainProduct> Products { get; set; }
    }
}
