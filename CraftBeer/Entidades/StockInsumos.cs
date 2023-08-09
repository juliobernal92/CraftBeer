using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CraftBeer.Entidades
{
    public class StockInsumos
    {
        public int IdInsumo { get; set; }
        public string TipoInsumo { get; set; }
        public string Nombre { get; set; }
        public decimal Kilos { get; set; }
        public decimal CostoKilos { get; set; }
    }

}
