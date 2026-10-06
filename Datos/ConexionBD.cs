using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RegistroEstudiantes.Datos
{
    public static class ConexionBD
    {
        private const string CadenaConexion =
            "Server=LOCALHOST;" +
            "Database=RegistroEstudiantesDB_V2;" +
            "Trusted_Connection=True;" +
            "TrustServerCertificate=True;";

        public static SqlConnection CrearConexion()
        {
            return new SqlConnection(CadenaConexion);
        }
    }
}
