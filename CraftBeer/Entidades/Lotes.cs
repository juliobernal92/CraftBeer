using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CraftBeer.Entidades
{
    public class Lotes
    {
        public int IdLote { get; set; }
        public string Maestro { get; set; }
        public string Estilo { get; set; }
        public DateTime FechaInicio { get; set; }
        public decimal DensidadInicial { get; set; }
        public string Estado { get; set; }
        public decimal DensidadFinal { get; set; }
        public DateTime FechaFin { get; set; }
        public decimal LitrosFinales { get; set; }
        public decimal ABV { get; set; }
        public float Resta { get; set; }
    }

}
