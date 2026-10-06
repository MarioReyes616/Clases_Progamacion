using Microsoft.Data.SqlClient;
using RegistroEstudiantes.Modelos;

namespace RegistroEstudiantes.Datos
{
    public class EstudianteRepository
    {
        private static Estudiante MapearEstudiante(SqlDataReader lector)
        {
            string nombreArea = lector.GetString(
 lector.GetOrdinal("Area"));
            AreaConocimiento area = Enum.TryParse(
            nombreArea,
            true,
            out AreaConocimiento areaSeleccionada)
            ? areaSeleccionada
            : AreaConocimiento.Otra;
            return new Estudiante
            {
                Id = lector.GetGuid(lector.GetOrdinal("Id")),
                Carnet = lector.GetString(lector.GetOrdinal("Carnet")),
                Cedula = lector.IsDBNull(lector.GetOrdinal("Cedula"))
 ? string.Empty
 : lector.GetString(lector.GetOrdinal("Cedula")),
                Nombres = lector.GetString(lector.GetOrdinal("Nombres")),
                Apellidos = lector.GetString(lector.GetOrdinal("Apellidos")),
                Sexo = Enum.Parse<Sexo>(lector.GetString(lector.GetOrdinal("Sexo"))),
                FechaNacimiento = lector.GetDateTime(
 lector.GetOrdinal("FechaNacimiento")),
                Nacionalidad = lector.GetString(lector.GetOrdinal("Nacionalidad")),
                NivelAcademico = lector.GetString(lector.GetOrdinal("NivelAcademico")),
                TieneTutor = lector.GetBoolean(lector.GetOrdinal("TieneTutor")),
                IdCarrera = lector.GetInt32(lector.GetOrdinal("IdCarrera")),
                Carrera = lector.GetString(lector.GetOrdinal("Carrera")),
                AreaConocimiento = area,
                IdMunicipio = lector.GetInt32(lector.GetOrdinal("IdMunicipio")),
                Municipio = lector.GetString(lector.GetOrdinal("Municipio")),
                Departamento = lector.GetString(lector.GetOrdinal("Departamento")),
                Etnia = lector.IsDBNull(lector.GetOrdinal("Etnia"))
 ? string.Empty
 : lector.GetString(lector.GetOrdinal("Etnia")),
                Correo = lector.GetString(lector.GetOrdinal("Correo")),
                Promedio = lector.GetDecimal(lector.GetOrdinal("Promedio")),
                TieneDiscapacidadFisica = lector.GetBoolean(
 lector.GetOrdinal("TieneDiscapacidadFisica")),
                DescripcionDiscapacidad = lector.IsDBNull(
 lector.GetOrdinal("DescripcionDiscapacidad"))
 ? string.Empty
 : lector.GetString(
 lector.GetOrdinal("DescripcionDiscapacidad")),
                EsInterno = lector.GetBoolean(lector.GetOrdinal("EsInterno")),
                Activo = lector.GetBoolean(lector.GetOrdinal("Activo"))
            };
        }
        public async Task<List<Estudiante>> ListarAsync(
CancellationToken token = default)
        {
            const string sql = @"SELECT
            e.Id,
            e.Carnet,
            e.Cedula,
            e.Nombres,
            e.Apellidos,
            e.Sexo,
            e.FechaNacimiento,
            e.Nacionalidad,
            e.NivelAcademico,
            e.TieneTutor,

            e.IdCarrera,
            c.Nombre AS Carrera,
            a.Nombre AS Area,
            e.IdMunicipio,
            m.Nombre AS Municipio,
            d.Nombre AS Departamento,

            e.Etnia,
            e.Correo,
            e.Promedio,
            e.TieneDiscapacidadFisica,
            e.DescripcionDiscapacidad,
            e.EsInterno,
            e.Activo

        FROM Estudiantes e

        INNER JOIN Carreras c
            ON c.IdCarrera = e.IdCarrera

        INNER JOIN AreasConocimiento a
            ON a.IdArea = c.IdArea

        INNER JOIN Municipios m
            ON m.IdMunicipio = e.IdMunicipio

        INNER JOIN Departamentos d
            ON d.IdDepartamento = m.IdDepartamento

        WHERE e.Activo = 1

        ORDER BY e.Apellidos, e.Nombres;
 
 ";
            List<Estudiante> lista = new();
            using SqlConnection conexion = ConexionBD.CrearConexion();
            using SqlCommand comando = new(sql, conexion);
            await conexion.OpenAsync(token);
            using SqlDataReader lector =
            await comando.ExecuteReaderAsync(token);
            while (await lector.ReadAsync(token))
            {
                token.ThrowIfCancellationRequested();
                lista.Add(MapearEstudiante(lector));
            }
            return lista;
        }

        public async Task<List<Estudiante>> BuscarAsync(
 string texto,
 CancellationToken token = default)
        {
            const string sql = @"
 SELECT 
            e.Id,
            e.Carnet,
            e.Cedula,
            e.Nombres,
            e.Apellidos,
            e.Sexo,
            e.FechaNacimiento,
            e.Nacionalidad,
            e.NivelAcademico,
            e.TieneTutor,

            e.IdCarrera,
            c.Nombre AS Carrera,
            a.Nombre AS Area,
            e.IdMunicipio,
            m.Nombre AS Municipio,
            d.Nombre AS Departamento,

            e.Etnia,
            e.Correo,
            e.Promedio,
            e.TieneDiscapacidadFisica,
            e.DescripcionDiscapacidad,
            e.EsInterno,
            e.Activo

        FROM Estudiantes e

        INNER JOIN Carreras c
            ON c.IdCarrera = e.IdCarrera

        INNER JOIN AreasConocimiento a
            ON a.IdArea = c.IdArea

        INNER JOIN Municipios m
            ON m.IdMunicipio = e.IdMunicipio

        INNER JOIN Departamentos d
            ON d.IdDepartamento = m.IdDepartamento

        WHERE e.Activo = 1

        AND (
            e.Carnet LIKE @Texto
            OR e.Nombres LIKE @Texto
            OR e.Apellidos LIKE @Texto
        )

        ORDER BY e.Apellidos, e.Nombres;
    ";
 
            List<Estudiante> lista = new();
            using SqlConnection conexion = ConexionBD.CrearConexion();
            using SqlCommand comando = new(sql, conexion);
            comando.Parameters.AddWithValue(
            "@Texto",
            $"%{texto.Trim()}%");
            await conexion.OpenAsync(token);
            using SqlDataReader lector =
            await comando.ExecuteReaderAsync(token);
            while (await lector.ReadAsync(token))
                lista.Add(MapearEstudiante(lector));
            return lista;
        }
        public List<Estudiante> Buscar(string texto)
        {
            const string sql = @"
        SELECT 
            e.Id,
            e.Carnet,
            e.Cedula,
            e.Nombres,
            e.Apellidos,
            e.Sexo,
            e.FechaNacimiento,
            e.Nacionalidad,
            e.NivelAcademico,
            e.TieneTutor,

            e.IdCarrera,
            c.Nombre AS Carrera,
            a.Nombre AS Area,
            e.IdMunicipio,
            m.Nombre AS Municipio,
            d.Nombre AS Departamento,

            e.Etnia,
            e.Correo,
            e.Promedio,
            e.TieneDiscapacidadFisica,
            e.DescripcionDiscapacidad,
            e.EsInterno,
            e.Activo

        FROM Estudiantes e

        INNER JOIN Carreras c
            ON c.IdCarrera = e.IdCarrera

        INNER JOIN AreasConocimiento a
            ON a.IdArea = c.IdArea

        INNER JOIN Municipios m
            ON m.IdMunicipio = e.IdMunicipio

        INNER JOIN Departamentos d
            ON d.IdDepartamento = m.IdDepartamento

        WHERE e.Activo = 1

        AND (
            e.Carnet LIKE @Texto
            OR e.Nombres LIKE @Texto
            OR e.Apellidos LIKE @Texto
        )

        ORDER BY e.Apellidos, e.Nombres;
    ";

            List<Estudiante> lista = new();

            using SqlConnection conexion =
                ConexionBD.CrearConexion();

            using SqlCommand comando =
                new(sql, conexion);

            comando.Parameters.AddWithValue(
                "@Texto",
                $"%{texto.Trim()}%");

            conexion.Open();

            using SqlDataReader lector =
                comando.ExecuteReader();

            while (lector.Read())
            {
                string nombreArea =
    lector.GetString(
        lector.GetOrdinal("Area"));

                AreaConocimiento area;

                if (!Enum.TryParse(
                        nombreArea,
                        true,
                        out area))
                {
                    area = AreaConocimiento.Otra;
                }
                Estudiante estudiante = new()
                {
                    Id = lector.GetGuid(
                        lector.GetOrdinal("Id")),

                    Carnet = lector.GetString(
                        lector.GetOrdinal("Carnet")),

                    Cedula = lector.IsDBNull(
                        lector.GetOrdinal("Cedula"))
                        ? string.Empty
                        : lector.GetString(
                            lector.GetOrdinal("Cedula")),

                    Nombres = lector.GetString(
                        lector.GetOrdinal("Nombres")),

                    Apellidos = lector.GetString(
                        lector.GetOrdinal("Apellidos")),

                    Sexo = Enum.Parse<Sexo>(
                        lector.GetString(
                            lector.GetOrdinal("Sexo"))),

                    FechaNacimiento =
                        lector.GetDateTime(
                            lector.GetOrdinal(
                                "FechaNacimiento")),

                    Nacionalidad =
                        lector.GetString(
                            lector.GetOrdinal(
                                "Nacionalidad")),

                    NivelAcademico =
                        lector.GetString(
                            lector.GetOrdinal(
                                "NivelAcademico")),

                    TieneTutor =
                        lector.GetBoolean(
                            lector.GetOrdinal(
                                "TieneTutor")),

                    IdCarrera =
                        lector.GetInt32(
                            lector.GetOrdinal(
                                "IdCarrera")),

                    Carrera =
                        lector.GetString(
                            lector.GetOrdinal(
                                "Carrera")),

                    AreaConocimiento = area,

                    IdMunicipio =
                        lector.GetInt32(
                            lector.GetOrdinal(
                                "IdMunicipio")),

                    Municipio =
                        lector.GetString(
                            lector.GetOrdinal(
                                "Municipio")),

                    Departamento =
                        lector.GetString(
                            lector.GetOrdinal(
                                "Departamento")),

                    Etnia = lector.IsDBNull(
                        lector.GetOrdinal("Etnia"))
                        ? string.Empty
                        : lector.GetString(
                            lector.GetOrdinal("Etnia")),

                    Correo =
                        lector.GetString(
                            lector.GetOrdinal("Correo")),

                    Promedio =
                        lector.GetDecimal(
                            lector.GetOrdinal(
                                "Promedio")),

                    TieneDiscapacidadFisica =
                        lector.GetBoolean(
                            lector.GetOrdinal(
                                "TieneDiscapacidadFisica")),

                    DescripcionDiscapacidad =
                        lector.IsDBNull(
                            lector.GetOrdinal(
                                "DescripcionDiscapacidad"))
                        ? string.Empty
                        : lector.GetString(
                            lector.GetOrdinal(
                                "DescripcionDiscapacidad")),

                    EsInterno =
                        lector.GetBoolean(
                            lector.GetOrdinal(
                                "EsInterno")),

                    Activo =
                        lector.GetBoolean(
                            lector.GetOrdinal(
                                "Activo"))
                };

                lista.Add(estudiante);
            }

            return lista;
        }

        public async Task<bool> ExisteCarnetAsync(string carnet,
 CancellationToken token = default)
        {
            const string sql = "SELECT COUNT(*) FROM Estudiantes WHERE Carnet = @Carnet;";
            using SqlConnection conexion = ConexionBD.CrearConexion();
            using SqlCommand comando = new(sql, conexion);
            comando.Parameters.AddWithValue("@Carnet", carnet);
            conexion.Open();
            int cantidad = Convert.ToInt32(comando.ExecuteScalarAsync());
            return cantidad > 0;
        }
        public bool ExisteCarnet(string carnet)
        {
            const string sql = "SELECT COUNT(*) FROM Estudiantes WHERE Carnet = @Carnet;";
            using SqlConnection conexion = ConexionBD.CrearConexion();
            using SqlCommand comando = new(sql, conexion);
            comando.Parameters.AddWithValue("@Carnet", carnet);
            conexion.Open();
            int cantidad = Convert.ToInt32(comando.ExecuteScalar());
            return cantidad > 0;
        }




        public void Insertar(Estudiante e)
        {
            const string sql = @"  INSERT INTO Estudiantes 
        (Id, Carnet, Cedula, Nombres, Apellidos, Sexo, 
         FechaNacimiento, Nacionalidad, NivelAcademico, TieneTutor, 
         IdCarrera, IdMunicipio, Etnia, Correo, Promedio, 
         TieneDiscapacidadFisica, DescripcionDiscapacidad, EsInterno, Activo) 
        VALUES 
        (@Id, @Carnet, @Cedula, @Nombres, @Apellidos, @Sexo, 
         @FechaNacimiento, @Nacionalidad, @NivelAcademico, @TieneTutor, 
         @IdCarrera, @IdMunicipio, @Etnia, @Correo, @Promedio, 
         @TieneDiscapacidadFisica, @DescripcionDiscapacidad, @EsInterno, @Activo);";

            using SqlConnection conexion = ConexionBD.CrearConexion();
            using SqlCommand comando = new(sql, conexion);

            comando.Parameters.AddWithValue("@Id", e.Id);
            comando.Parameters.AddWithValue("@Carnet", e.Carnet);
            comando.Parameters.AddWithValue("@Cedula", string.IsNullOrWhiteSpace(e.Cedula) ? DBNull.Value : e.Cedula);
            comando.Parameters.AddWithValue("@Nombres", e.Nombres);
            comando.Parameters.AddWithValue("@Apellidos", e.Apellidos);
            comando.Parameters.AddWithValue("@Sexo", e.Sexo.ToString());
            comando.Parameters.AddWithValue("@FechaNacimiento", e.FechaNacimiento.Date);
            comando.Parameters.AddWithValue("@Nacionalidad", e.Nacionalidad);
            comando.Parameters.AddWithValue("@NivelAcademico", e.NivelAcademico);
            comando.Parameters.AddWithValue("@TieneTutor", e.TieneTutor);
            comando.Parameters.AddWithValue("@IdCarrera", e.IdCarrera);
            comando.Parameters.AddWithValue("@IdMunicipio", e.IdMunicipio);
            comando.Parameters.AddWithValue("@Etnia", string.IsNullOrWhiteSpace(e.Etnia) ? DBNull.Value : e.Etnia);
            comando.Parameters.AddWithValue("@Correo", e.Correo);
            comando.Parameters.AddWithValue("@Promedio", e.Promedio);
            comando.Parameters.AddWithValue("@TieneDiscapacidadFisica", e.TieneDiscapacidadFisica);
            comando.Parameters.AddWithValue("@DescripcionDiscapacidad",
        string.IsNullOrWhiteSpace(e.DescripcionDiscapacidad) ? DBNull.Value : e.DescripcionDiscapacidad);
            comando.Parameters.AddWithValue("@EsInterno", e.EsInterno);
            comando.Parameters.AddWithValue("@Activo", e.Activo);

            conexion.Open();
            comando.ExecuteNonQuery();
        }
        public List<Estudiante> Listar()
        {
            const string sql = @"
        SELECT
            e.Id,
            e.Carnet,
            e.Cedula,
            e.Nombres,
            e.Apellidos,
            e.Sexo,
            e.FechaNacimiento,
            e.Nacionalidad,
            e.NivelAcademico,
            e.TieneTutor,

            e.IdCarrera,
            c.Nombre AS Carrera,
            a.Nombre AS Area,
            e.IdMunicipio,
            m.Nombre AS Municipio,
            d.Nombre AS Departamento,

            e.Etnia,
            e.Correo,
            e.Promedio,
            e.TieneDiscapacidadFisica,
            e.DescripcionDiscapacidad,
            e.EsInterno,
            e.Activo

        FROM Estudiantes e

        INNER JOIN Carreras c
            ON c.IdCarrera = e.IdCarrera

        INNER JOIN AreasConocimiento a
            ON a.IdArea = c.IdArea

        INNER JOIN Municipios m
            ON m.IdMunicipio = e.IdMunicipio

        INNER JOIN Departamentos d
            ON d.IdDepartamento = m.IdDepartamento

        WHERE e.Activo = 1

        ORDER BY e.Apellidos, e.Nombres;
    ";

            List<Estudiante> lista = new();

            using SqlConnection conexion =
                ConexionBD.CrearConexion();

            using SqlCommand comando =
                new(sql, conexion);

            conexion.Open();

            using SqlDataReader lector =
                comando.ExecuteReader();

            while (lector.Read())
            {
                string nombreArea =
    lector.GetString(
        lector.GetOrdinal("Area"));

                AreaConocimiento area;

                if (!Enum.TryParse(
                        nombreArea,
                        true,
                        out area))
                {
                    area = AreaConocimiento.Otra;
                }
                Estudiante estudiante = new()
                {
                    Id = lector.GetGuid(
                        lector.GetOrdinal("Id")),

                    Carnet = lector.GetString(
                        lector.GetOrdinal("Carnet")),

                    Cedula = lector.IsDBNull(
                        lector.GetOrdinal("Cedula"))
                        ? string.Empty
                        : lector.GetString(
                            lector.GetOrdinal("Cedula")),

                    Nombres = lector.GetString(
                        lector.GetOrdinal("Nombres")),

                    Apellidos = lector.GetString(
                        lector.GetOrdinal("Apellidos")),

                    Sexo = Enum.Parse<Sexo>(
                        lector.GetString(
                            lector.GetOrdinal("Sexo"))),

                    FechaNacimiento =
                        lector.GetDateTime(
                            lector.GetOrdinal("FechaNacimiento")),

                    Nacionalidad =
                        lector.GetString(
                            lector.GetOrdinal("Nacionalidad")),

                    NivelAcademico =
                        lector.GetString(
                            lector.GetOrdinal("NivelAcademico")),

                    TieneTutor =
                        lector.GetBoolean(
                            lector.GetOrdinal("TieneTutor")),

                    IdCarrera =
                        lector.GetInt32(
                            lector.GetOrdinal("IdCarrera")),

                    Carrera =
                        lector.GetString(
                            lector.GetOrdinal("Carrera")),


                    IdMunicipio =
                        lector.GetInt32(
                            lector.GetOrdinal("IdMunicipio")),

                    Municipio =
                        lector.GetString(
                            lector.GetOrdinal("Municipio")),

                    AreaConocimiento = area,

                    Departamento =
                        lector.GetString(
                            lector.GetOrdinal("Departamento")),

                    Etnia = lector.IsDBNull(
                        lector.GetOrdinal("Etnia"))
                        ? string.Empty
                        : lector.GetString(
                            lector.GetOrdinal("Etnia")),

                    Correo =
                        lector.GetString(
                            lector.GetOrdinal("Correo")),

                    Promedio =
                        lector.GetDecimal(
                            lector.GetOrdinal("Promedio")),

                    TieneDiscapacidadFisica =
                        lector.GetBoolean(
                            lector.GetOrdinal(
                                "TieneDiscapacidadFisica")),

                    DescripcionDiscapacidad =
                        lector.IsDBNull(
                            lector.GetOrdinal(
                                "DescripcionDiscapacidad"))
                        ? string.Empty
                        : lector.GetString(
                            lector.GetOrdinal(
                                "DescripcionDiscapacidad")),

                    EsInterno =
                        lector.GetBoolean(
                            lector.GetOrdinal("EsInterno")),

                    Activo =
                        lector.GetBoolean(
                            lector.GetOrdinal("Activo"))
                };

                lista.Add(estudiante);
            }

            return lista;
        }
        public void Actualizar(Estudiante estudiante)
        {
            const string sql = @"
        UPDATE Estudiantes
        SET
            Cedula = @Cedula,
            Nombres = @Nombres,
            Apellidos = @Apellidos,
            Sexo = @Sexo,
            FechaNacimiento = @FechaNacimiento,
            Nacionalidad = @Nacionalidad,
            NivelAcademico = @NivelAcademico,
            TieneTutor = @TieneTutor,
            IdCarrera = @IdCarrera,
            IdMunicipio = @IdMunicipio,
            Etnia = @Etnia,
            Correo = @Correo,
            Promedio = @Promedio,
            TieneDiscapacidadFisica = @TieneDiscapacidadFisica,
            DescripcionDiscapacidad = @DescripcionDiscapacidad,
            EsInterno = @EsInterno
        WHERE Id = @Id;
    ";

            using SqlConnection conexion =
                ConexionBD.CrearConexion();

            using SqlCommand comando =
                new(sql, conexion);

            comando.Parameters.Add(
                "@Id",
                System.Data.SqlDbType.UniqueIdentifier)
                .Value = estudiante.Id;

            comando.Parameters.Add(
                "@Cedula",
                System.Data.SqlDbType.NVarChar,
                30)
                .Value =
                string.IsNullOrWhiteSpace(estudiante.Cedula)
                ? DBNull.Value
                : estudiante.Cedula;

            comando.Parameters.Add(
                "@Nombres",
                System.Data.SqlDbType.NVarChar,
                100)
                .Value = estudiante.Nombres;

            comando.Parameters.Add(
                "@Apellidos",
                System.Data.SqlDbType.NVarChar,
                100)
                .Value = estudiante.Apellidos;

            comando.Parameters.Add(
                "@Sexo",
                System.Data.SqlDbType.Char,
                1)
                .Value = estudiante.Sexo.ToString();

            comando.Parameters.Add(
                "@FechaNacimiento",
                System.Data.SqlDbType.Date)
                .Value = estudiante.FechaNacimiento.Date;

            comando.Parameters.Add(
                "@Nacionalidad",
                System.Data.SqlDbType.NVarChar,
                60)
                .Value = estudiante.Nacionalidad;

            comando.Parameters.Add(
                "@NivelAcademico",
                System.Data.SqlDbType.NVarChar,
                60)
                .Value = estudiante.NivelAcademico;

            comando.Parameters.Add(
                "@TieneTutor",
                System.Data.SqlDbType.Bit)
                .Value = estudiante.TieneTutor;

            comando.Parameters.Add(
                "@IdCarrera",
                System.Data.SqlDbType.Int)
                .Value = estudiante.IdCarrera;

            comando.Parameters.Add(
                "@IdMunicipio",
                System.Data.SqlDbType.Int)
                .Value = estudiante.IdMunicipio;

            comando.Parameters.Add(
                "@Etnia",
                System.Data.SqlDbType.NVarChar,
                60)
                .Value =
                string.IsNullOrWhiteSpace(estudiante.Etnia)
                ? DBNull.Value
                : estudiante.Etnia;

            comando.Parameters.Add(
                "@Correo",
                System.Data.SqlDbType.NVarChar,
                150)
                .Value = estudiante.Correo;

            comando.Parameters.Add(
                "@Promedio",
                System.Data.SqlDbType.Decimal)
                .Value = estudiante.Promedio;

            comando.Parameters.Add(
                "@TieneDiscapacidadFisica",
                System.Data.SqlDbType.Bit)
                .Value = estudiante.TieneDiscapacidadFisica;

            comando.Parameters.Add(
                "@DescripcionDiscapacidad",
                System.Data.SqlDbType.NVarChar,
                300)
                .Value =
                string.IsNullOrWhiteSpace(
                    estudiante.DescripcionDiscapacidad)
                ? DBNull.Value
                : estudiante.DescripcionDiscapacidad;

            comando.Parameters.Add(
                "@EsInterno",
                System.Data.SqlDbType.Bit)
                .Value = estudiante.EsInterno;

            conexion.Open();

            comando.ExecuteNonQuery();
        }
        public async Task DesactivarAsync(
 Guid id,
 CancellationToken token = default)
        {
            const string sql = @"
 UPDATE Estudiantes
 SET Activo = 0
 WHERE Id = @Id;";
            using SqlConnection conexion = ConexionBD.CrearConexion();
            using SqlCommand comando = new(sql, conexion);
            comando.Parameters.AddWithValue("@Id", id);
            await conexion.OpenAsync(token);
            await comando.ExecuteNonQueryAsync(token);
        }
        public void Desactivar(Guid id)
        {
            const string sql = "UPDATE Estudiantes SET Activo = 0 WHERE Id = @Id;";
            using SqlConnection conexion = ConexionBD.CrearConexion();
            using SqlCommand comando = new(sql, conexion);
            comando.Parameters.AddWithValue("@Id", id);
            conexion.Open();
            comando.ExecuteNonQuery();
        }
        public async Task InsertarAsync(
    Estudiante estudiante,
    CancellationToken token = default)
        {
            const string sql = @"
        INSERT INTO Estudiantes
        (
            Id,
            Carnet,
            Cedula,
            Nombres,
            Apellidos,
            Sexo,
            FechaNacimiento,
            Nacionalidad,
            NivelAcademico,
            TieneTutor,
            IdCarrera,
            IdMunicipio,
            Etnia,
            Correo,
            Promedio,
            TieneDiscapacidadFisica,
            DescripcionDiscapacidad,
            EsInterno,
            Activo
        )
        VALUES
        (
            @Id,
            @Carnet,
            @Cedula,
            @Nombres,
            @Apellidos,
            @Sexo,
            @FechaNacimiento,
            @Nacionalidad,
            @NivelAcademico,
            @TieneTutor,
            @IdCarrera,
            @IdMunicipio,
            @Etnia,
            @Correo,
            @Promedio,
            @TieneDiscapacidadFisica,
            @DescripcionDiscapacidad,
            @EsInterno,
            @Activo
        );";

            using SqlConnection conexion = ConexionBD.CrearConexion();
            using SqlCommand comando = new(sql, conexion);

            comando.Parameters.AddWithValue("@Id", estudiante.Id);
            comando.Parameters.AddWithValue("@Carnet", estudiante.Carnet);
            comando.Parameters.AddWithValue("@Cedula", estudiante.Cedula);
            comando.Parameters.AddWithValue("@Nombres", estudiante.Nombres);
            comando.Parameters.AddWithValue("@Apellidos", estudiante.Apellidos);
            comando.Parameters.AddWithValue("@Sexo", estudiante.Sexo.ToString());
            comando.Parameters.AddWithValue("@FechaNacimiento", estudiante.FechaNacimiento);
            comando.Parameters.AddWithValue("@Nacionalidad", estudiante.Nacionalidad);
            comando.Parameters.AddWithValue("@NivelAcademico", estudiante.NivelAcademico);
            comando.Parameters.AddWithValue("@TieneTutor", estudiante.TieneTutor);
            comando.Parameters.AddWithValue("@IdCarrera", estudiante.IdCarrera);
            comando.Parameters.AddWithValue("@IdMunicipio", estudiante.IdMunicipio);
            comando.Parameters.AddWithValue("@Etnia", estudiante.Etnia);
            comando.Parameters.AddWithValue("@Correo", estudiante.Correo);
            comando.Parameters.AddWithValue("@Promedio", estudiante.Promedio);
            comando.Parameters.AddWithValue(
                "@TieneDiscapacidadFisica",
                estudiante.TieneDiscapacidadFisica);

            comando.Parameters.AddWithValue(
                "@DescripcionDiscapacidad",
                estudiante.DescripcionDiscapacidad);

            comando.Parameters.AddWithValue(
                "@EsInterno",
                estudiante.EsInterno);

            comando.Parameters.AddWithValue(
                "@Activo",
                estudiante.Activo);

            await conexion.OpenAsync(token);

            await comando.ExecuteNonQueryAsync(token);
        }

        public async Task ActualizarAsync(
    Estudiante estudiante,
    CancellationToken token = default)
        {
            const string sql = @"
        UPDATE Estudiantes
        SET
            Carnet = @Carnet,
            Cedula = @Cedula,
            Nombres = @Nombres,
            Apellidos = @Apellidos,
            Sexo = @Sexo,
            FechaNacimiento = @FechaNacimiento,
            Nacionalidad = @Nacionalidad,
            NivelAcademico = @NivelAcademico,
            TieneTutor = @TieneTutor,
            IdCarrera = @IdCarrera,
            IdMunicipio = @IdMunicipio,
            Etnia = @Etnia,
            Correo = @Correo,
            Promedio = @Promedio,
            TieneDiscapacidadFisica = @TieneDiscapacidadFisica,
            DescripcionDiscapacidad = @DescripcionDiscapacidad,
            EsInterno = @EsInterno
        WHERE Id = @Id;";

            using SqlConnection conexion = ConexionBD.CrearConexion();
            using SqlCommand comando = new(sql, conexion);

            comando.Parameters.AddWithValue("@Id", estudiante.Id);
            comando.Parameters.AddWithValue("@Carnet", estudiante.Carnet);
            comando.Parameters.AddWithValue("@Cedula", estudiante.Cedula);
            comando.Parameters.AddWithValue("@Nombres", estudiante.Nombres);
            comando.Parameters.AddWithValue("@Apellidos", estudiante.Apellidos);
            comando.Parameters.AddWithValue("@Sexo", estudiante.Sexo.ToString());
            comando.Parameters.AddWithValue("@FechaNacimiento", estudiante.FechaNacimiento);
            comando.Parameters.AddWithValue("@Nacionalidad", estudiante.Nacionalidad);
            comando.Parameters.AddWithValue("@NivelAcademico", estudiante.NivelAcademico);
            comando.Parameters.AddWithValue("@TieneTutor", estudiante.TieneTutor);
            comando.Parameters.AddWithValue("@IdCarrera", estudiante.IdCarrera);
            comando.Parameters.AddWithValue("@IdMunicipio", estudiante.IdMunicipio);
            comando.Parameters.AddWithValue("@Etnia", estudiante.Etnia);
            comando.Parameters.AddWithValue("@Correo", estudiante.Correo);
            comando.Parameters.AddWithValue("@Promedio", estudiante.Promedio);
            comando.Parameters.AddWithValue(
                "@TieneDiscapacidadFisica",
                estudiante.TieneDiscapacidadFisica);

            comando.Parameters.AddWithValue(
                "@DescripcionDiscapacidad",
                estudiante.DescripcionDiscapacidad);

            comando.Parameters.AddWithValue(
                "@EsInterno",
                estudiante.EsInterno);

            await conexion.OpenAsync(token);

            await comando.ExecuteNonQueryAsync(token);
        }


    }

}
