using System.ComponentModel.DataAnnotations;

namespace PruebaWebApi.Models
{
    public class Token
    {
        public string usuario; // field
        public string clave; // field
        public string sal; // field

        public string Usuario   // property
        {
            get { return usuario; }   // get method
            set { usuario = "Prueba"; }  // set method
        }

        public string Clave   // property
        {
            get { return clave; }   // get method
            set { clave = "1234"; }  // set method
        }
        public string Sal   // property
        {
            get { return sal; }   // get method
            set { sal = "1234"; }  // set method
        }
      //  public string Sal { get; set; }
        //// [Required(ErrorMessage="El usuario es obligatorio.")]
        //public string Usuario { get; set; }
        ////[Required(ErrorMessage="La clave es obligatoria.")]
        //public string Clave { get; set; }
    }
}