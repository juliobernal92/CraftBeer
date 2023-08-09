using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CraftBeer.Entidades
{
    public class StockProducto
    {
        public int IdEstilo { get; set; }
        public string Estilo { get; set; }
        public int Cantidad { get; set; }
        public string Descripcion { get; set; }
        public int IdLote { get; set; }
    }

}
