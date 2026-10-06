using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RegistroEstudiantes.Modelos
{
   

public class OpcionCatalogo
    {
        public int Id { get; set; }

        public string Nombre { get; set; }
            = string.Empty;

        public override string ToString()
        {
            return Nombre;
        }
       
      
    }
}
