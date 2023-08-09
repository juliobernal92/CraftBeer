using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CraftBeer.Entidades
{
    public class DetallesVenta
    {
        public int IdDetalle { get; set; }
        public int IdVenta { get; set; }
        public int IdEstilo { get; set; }
        public int Cantidad { get; set; }
        public int PrecioUnitario { get; set; }
        public int Valor { get; set; }
    }

}
