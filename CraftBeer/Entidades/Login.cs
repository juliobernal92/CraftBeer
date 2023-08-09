using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CraftBeer.Entidades
{
    public class Login
    {
        public int Id_User { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Email { get; set; }
        public string Usuario { get; set; }
        public string Password { get; set; }
        /*
        public Login(long pIdUser, string pNombre, string pApellido, string pEmail, string pUsuario, string pPassword)
        {
            this.Id_User = pIdUser;
            this.Nombre = pNombre;
            this.Apellido = pApellido;
            this.Email = pEmail;
            this.Usuario = pUsuario;
            this.Password = pPassword;
        }

        public override string ToString()
        {
            return Id_User + "   " + Nombre + "   " + Apellido + "   " + Email + "   " + Usuario + "   " + Password;
        }*/
    }
}
