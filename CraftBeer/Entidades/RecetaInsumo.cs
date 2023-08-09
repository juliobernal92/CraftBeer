using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CraftBeer.Entidades
{
    public class RecetaInsumo
    {
        public int IdRecetaInsumo { get; set; }
        public int IdReceta { get; set; }
        public int IdInsumo { get; set; }
        public decimal Kilos { get; set; }
    }

}
