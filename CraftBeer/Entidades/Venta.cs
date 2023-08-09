using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CraftBeer.Entidades
{
    public class Venta
    {
        public int IdVenta { get; set; }
        public DateTime Fecha { get; set; }
        public int IdCliente { get; set; }
        public int? TotalIva { get; set; }
        public int? MontoTotal { get; set; }
    }

}
