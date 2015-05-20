using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Spider.Models
{
    public class MainProduct
    {
          public MainProduct()
        {
            this.PicturesPaths = new HashSet<PicturePath>();
            this.Colors= new HashSet<VariationColor>();
            this.SlaveProducts = new HashSet<SlaveProduct>();                     
        }
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string MainProductId { get; set; }
        public string ProductName { get; set; }

        public string ShortDesc { get; set; }
        public string LongDesc { get; set; }
        public string PriceWithVat { get; set; }
        public string PriceWithoutVat { get; set; }
        public int DefaultColorId { get; set; }
        public virtual ICollection<PicturePath> PicturesPaths { get; set; }
        public virtual ICollection<VariationColor> Colors { get; set; }
        public virtual ICollection<SlaveProduct> SlaveProducts { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; }
    }
}
