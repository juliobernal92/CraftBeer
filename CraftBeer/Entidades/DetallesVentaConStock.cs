using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CraftBeer.Entidades
{
    public class DetallesVentaConStock
    {
        public int IdEstilo { get; set; }
        public int Cantidad { get; set; }
        public decimal PrecioUnitario { get; set; }
        public decimal Valor { get; set; }
        public string Estilo { get; set; }
        public string Descripcion { get; set; }
    }
}
