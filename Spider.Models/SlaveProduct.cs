using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spider.Models
{
    public class SlaveProduct
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string SlaveProductId { get; set; }     
        public string RefinementColor { get; set; }
        public string MainProductId { get; set; }
        public MainProduct MainProduct { get; set; }
        public int VariationColorId { get; set; }
        public VariationColor VariationColor { get; set; }
    }
}
