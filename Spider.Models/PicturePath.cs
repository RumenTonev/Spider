using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spider.Models
{
   public  class PicturePath
    {
        public int PicturePathId { get; set; }
        public string Content { get; set; }
        public string ProductId { get; set; }
        public MainProduct Product { get; set; }      
        public int VariationColorId { get; set; }
        public VariationColor VariationColor { get; set; }
    }
}
