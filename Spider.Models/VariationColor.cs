using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spider.Models
{
   public class VariationColor
    {
        public VariationColor()
        {
            this.MainProducts = new HashSet<MainProduct>();
            this.SlaveProducts = new HashSet<SlaveProduct>();   
        }
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
       public int VariationColorId { get; set; }      
       public string ColorName { get; set; }
       public virtual ICollection<MainProduct> MainProducts { get; set; }
       public virtual ICollection<SlaveProduct> SlaveProducts { get; set; }
    }
}
