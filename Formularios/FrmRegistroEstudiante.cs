using Microsoft.Data.SqlClient;
using RegistroEstudiantes.Datos;
using RegistroEstudiantes.Modelos;
using RegistroEstudiantes.ServiciosTemporales;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RegistroEstudiantes.Formularios
{


    public partial class FrmRegistroEstudiante : Form
    {
        private readonly EstudianteRepository estudianteRepository = new();
        private readonly CatalogoRepository catalogoRepository = new();
        private Estudiante? estudianteEnEdicion;
        public FrmRegistroEstudiante()
        {
            InitializeComponent();
            CargarSexos();
            CargarNacionalidades();
            CargarNivelesAcademicos();

            CargarAreas();
            CargarDepartamentos();

            cboCarrera.DataSource = null;
            cboCarrera.Enabled = false;

            cboMunicipio.DataSource = null;
            cboMunicipio.Enabled = false;

            txtDescripcionDiscapacidad.Enabled = false;

        }

        public FrmRegistroEstudiante(Estudiante estudiante) : this()
        {
            estudianteEnEdicion = estudiante;
            CargarDatosEstudiante();
        }

        private void CargarDatosEstudiante()
        {
            if (estudianteEnEdicion == null)
                return;

            // Datos personales
            txtCarnet.Text = estudianteEnEdicion.Carnet;
            txtCedula.Text = estudianteEnEdicion.Cedula;
            txtNombres.Text = estudianteEnEdicion.Nombres;
            txtApellidos.Text = estudianteEnEdicion.Apellidos;

            // Sexo
            cboSexo.Text =
                estudianteEnEdicion.Sexo.ToString();

            // Fecha de nacimiento
            dtpFechaNacimiento.Value =
                estudianteEnEdicion.FechaNacimiento;

            // Datos académicos
            cboNacionalidad.Text =
                estudianteEnEdicion.Nacionalidad;

            cboNivelAcademico.Text =
                estudianteEnEdicion.NivelAcademico;

            chkTieneTutor.Checked =
                estudianteEnEdicion.TieneTutor;

            int indiceArea =
     cboArea.FindStringExact(
         estudianteEnEdicion.AreaConocimiento.ToString());

            if (indiceArea >= 0)
            {
                cboArea.SelectedIndex = indiceArea;
            }
            // Carrera
            cboCarrera.SelectedValue =
            estudianteEnEdicion.IdCarrera;

            // Departamento
            int indiceDepartamento =
                cboDepartamento.FindStringExact(
                    estudianteEnEdicion.Departamento);

            if (indiceDepartamento >= 0)
            {
                cboDepartamento.SelectedIndex =
                    indiceDepartamento;
            }

            // IMPORTANTE:
            // al seleccionar Departamento se cargan
            // automáticamente sus municipios.

            cboMunicipio.SelectedValue =
                estudianteEnEdicion.IdMunicipio;

            // Otros datos
            cboEtnia.Text =
                estudianteEnEdicion.Etnia;

            txtCorreo.Text =
                estudianteEnEdicion.Correo;

            nudPromedio.Value =
                estudianteEnEdicion.Promedio;

            chkDiscapacidad.Checked =
                estudianteEnEdicion.TieneDiscapacidadFisica;

            txtDescripcionDiscapacidad.Text =
                estudianteEnEdicion.DescripcionDiscapacidad;

            // El carnet no se modifica durante la edición
            txtCarnet.ReadOnly = true;
        }
        private async void CargarAreasAsync()
        {
            List<OpcionCatalogo> areas =
              await  catalogoRepository.ListarAreasasync();

            cboArea.DataSource = null;

            cboArea.DisplayMember = "Nombre";
            cboArea.ValueMember = "Id";
            cboArea.DataSource = areas;

            cboArea.SelectedIndex = -1;
        }
        private void CargarAreas()
        {
            List<OpcionCatalogo> areas =
                catalogoRepository.ListarAreas();

            cboArea.DataSource = null;

            cboArea.DisplayMember = "Nombre";
            cboArea.ValueMember = "Id";
            cboArea.DataSource = areas;

            cboArea.SelectedIndex = -1;
        }
        private void CargarNivelesAcademicos()
        {
            cboNivelAcademico.Items.Clear();

            cboNivelAcademico.Items.AddRange(new object[]
            {
        "Secundaria",
        "Técnico",
        "Universitario",
        "Egresado",
        "Posgrado"
            });

            cboNivelAcademico.SelectedIndex = -1;
            cboNivelAcademico.DropDownStyle =
                ComboBoxStyle.DropDownList;
        }
        private void CargarNacionalidades()
        {
            cboNacionalidad.Items.Clear();

            cboNacionalidad.Items.AddRange(new object[]
            {
        "Nicaragüense",
        "Costarricense",
        "Hondureña",
        "Salvadoreña",
        "Guatemalteca",
        "Panameña",
        "Otra"
            });

            cboNacionalidad.DropDownStyle =
                ComboBoxStyle.DropDown;

            cboNacionalidad.AutoCompleteMode =
                AutoCompleteMode.SuggestAppend;

            cboNacionalidad.AutoCompleteSource =
                AutoCompleteSource.ListItems;

            cboNacionalidad.SelectedIndex = -1;
        }
        private void CargarSexos()
        {
            cboSexo.DataSource = Enum.GetValues<Sexo>();
            cboSexo.SelectedIndex = -1;
            cboSexo.DropDownStyle = ComboBoxStyle.DropDownList;
        }

        private void CargarCarreras(int idArea)
        {
            List<OpcionCatalogo> carreras =
                catalogoRepository.ListarCarreras(idArea);

            cboCarrera.DataSource = null;

            cboCarrera.DisplayMember = "Nombre";
            cboCarrera.ValueMember = "Id";
            cboCarrera.DataSource = carreras;

            cboCarrera.SelectedIndex = -1;

            cboCarrera.Enabled =
                carreras.Count > 0;
        }
        private void CargarDepartamentos()
        {
            List<OpcionCatalogo> departamentos =
                catalogoRepository.ListarDepartamentos();

            cboDepartamento.DataSource = null;

            cboDepartamento.DisplayMember = "Nombre";
            cboDepartamento.ValueMember = "Id";
            cboDepartamento.DataSource = departamentos;

            cboDepartamento.SelectedIndex = -1;
        }

        private void CargarMunicipios(int idDepartamento)
        {
            List<OpcionCatalogo> municipios =
                catalogoRepository.ListarMunicipios(idDepartamento);

            cboMunicipio.DataSource = null;

            cboMunicipio.DisplayMember = "Nombre";
            cboMunicipio.ValueMember = "Id";
            cboMunicipio.DataSource = municipios;

            cboMunicipio.SelectedIndex = -1;

            cboMunicipio.Enabled = municipios.Count > 0;
        }

        private Estudiante ConstruirEstudianteDesdeFormulario()
        {
            Sexo sexo =
                (Sexo)cboSexo.SelectedItem!;

            AreaConocimiento area =
                Enum.Parse<AreaConocimiento>(
                    cboArea.Text,
                    true);

            int idCarrera =
                Convert.ToInt32(
                    cboCarrera.SelectedValue);

            int idMunicipio =
                Convert.ToInt32(
                    cboMunicipio.SelectedValue);

            return new Estudiante
            {
                Carnet = txtCarnet.Text.Trim(),
                Cedula = txtCedula.Text.Trim(),

                Nombres = txtNombres.Text.Trim(),
                Apellidos = txtApellidos.Text.Trim(),

                Sexo = sexo,

                FechaNacimiento =
                    dtpFechaNacimiento.Value.Date,

                Nacionalidad =
                    cboNacionalidad.Text.Trim(),

                NivelAcademico =
                    cboNivelAcademico.Text.Trim(),

                TieneTutor =
                    chkTieneTutor.Checked,

                AreaConocimiento = area,

                IdCarrera = idCarrera,
                Carrera = cboCarrera.Text.Trim(),

                Departamento =
                    cboDepartamento.Text.Trim(),

                IdMunicipio = idMunicipio,
                Municipio =
                    cboMunicipio.Text.Trim(),

                Etnia =
                    cboEtnia.Text.Trim(),

                Correo =
                    txtCorreo.Text.Trim(),

                Promedio =
                    nudPromedio.Value,

                TieneDiscapacidadFisica =
                    chkDiscapacidad.Checked,

                DescripcionDiscapacidad =
                    txtDescripcionDiscapacidad.Text.Trim()
            };
        }
        private bool ValidarFormulario(
     Estudiante estudiante)
        {
            List<string> errores =
                ValidadorEstudiante.Validar(estudiante);

            if (errores.Count == 0)
                return true;

            MessageBox.Show(
                string.Join(
                    Environment.NewLine,
                    errores),
                "Revise la información",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning);

            return false;
        }

        private bool ValidarControlesFormulario()
        {
            errorProvider1.Clear();

            List<string> errores = new();

            if (string.IsNullOrWhiteSpace(txtCarnet.Text))
            {
                errorProvider1.SetError(
                    txtCarnet,
                    "Ingrese el carnet.");

                errores.Add("Debe ingresar el carnet.");
            }

            if (string.IsNullOrWhiteSpace(txtNombres.Text))
            {
                errorProvider1.SetError(
                    txtNombres,
                    "Ingrese los nombres.");

                errores.Add("Debe ingresar los nombres.");
            }

            if (string.IsNullOrWhiteSpace(txtApellidos.Text))
            {
                errorProvider1.SetError(
                    txtApellidos,
                    "Ingrese los apellidos.");

                errores.Add("Debe ingresar los apellidos.");
            }

            if (cboSexo.SelectedIndex == -1)
            {
                errorProvider1.SetError(
                    cboSexo,
                    "Seleccione el sexo.");

                errores.Add("Debe seleccionar el sexo.");
            }

            if (string.IsNullOrWhiteSpace(cboNacionalidad.Text))
            {
                errorProvider1.SetError(
                    cboNacionalidad,
                    "Ingrese o seleccione la nacionalidad.");

                errores.Add("Debe indicar la nacionalidad.");
            }

            if (cboNivelAcademico.SelectedIndex == -1)
            {
                errorProvider1.SetError(
                    cboNivelAcademico,
                    "Seleccione el nivel académico.");

                errores.Add("Debe seleccionar el nivel académico.");
            }

            if (cboArea.SelectedIndex == -1)
            {
                errorProvider1.SetError(
                    cboArea,
                    "Seleccione un área.");

                errores.Add(
                    "Debe seleccionar un área de conocimiento.");
            }

            if (cboCarrera.SelectedIndex == -1)
            {
                errorProvider1.SetError(
                    cboCarrera,
                    "Seleccione una carrera.");

                errores.Add("Debe seleccionar una carrera.");
            }

            if (cboDepartamento.SelectedIndex == -1)
            {
                errorProvider1.SetError(
                    cboDepartamento,
                    "Seleccione un departamento.");

                errores.Add("Debe seleccionar un departamento.");
            }

            if (cboMunicipio.SelectedIndex == -1)
            {
                errorProvider1.SetError(
                    cboMunicipio,
                    "Seleccione un municipio.");

                errores.Add("Debe seleccionar un municipio.");
            }

            if (errores.Count == 0)
                return true;

            MessageBox.Show(
                string.Join(Environment.NewLine, errores),
                "Revise la información",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning);

            return false;
        }

        private void chkTieneTutor_CheckedChanged(object sender, EventArgs e)
        {
            // grpTutor.Enabled = chkTieneTutor.Checked;
        }

        private void chkDiscapacidad_CheckedChanged(object sender, EventArgs e)
        {
            txtDescripcionDiscapacidad.Enabled = chkDiscapacidad.Checked;

            if (!chkDiscapacidad.Checked)
                txtDescripcionDiscapacidad.Clear();
        }

        private void cboArea_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboArea.SelectedValue is int idArea)
            {
                CargarCarreras(idArea);
            }
            else
            {
                cboCarrera.DataSource = null;
                cboCarrera.Enabled = false;
            }
        }
        private async void btnGuardar_Click_2(object sender, EventArgs e)
        {
            bool cerrarFormulario = false;

            try
            {
                // 1. Validar controles
                if (!ValidarControlesFormulario())
                    return;

                // 2. Construir el objeto
                Estudiante estudiante = ConstruirEstudianteDesdeFormulario();

                // 3. Validar reglas del modelo
                if (!ValidarFormulario(estudiante))
                    return;

                btnGuardar.Enabled = false;
                UseWaitCursor = true;

                if (estudianteEnEdicion == null)
                {
                    // NUEVO ESTUDIANTE
                    if (await estudianteRepository.ExisteCarnetAsync(estudiante.Carnet))
                    {
                        errorProvider1.SetError(
                            txtCarnet,
                            "El carnet ya existe en la base de datos.");
                        return;
                    }

                    await estudianteRepository.InsertarAsync(estudiante);

                    MessageBox.Show("Estudiante guardado correctamente.", "Registro",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);

                    LimpiarFormulario();
                }
                else
                {
                    estudiante.Id = estudianteEnEdicion.Id;
                    estudiante.EsInterno = estudianteEnEdicion.EsInterno;
                    estudiante.Activo = estudianteEnEdicion.Activo;

                    await estudianteRepository.ActualizarAsync(estudiante);

                    MessageBox.Show("Estudiante actualizado correctamente.", "Actualización",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);

                    cerrarFormulario = true;
                }
            }
            catch (SqlException ex)
            {
                MostrarErrorBD(ex);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                UseWaitCursor = false;
                btnGuardar.Enabled = true;
            }

            if (cerrarFormulario)
                Close();   // se cierra después del finally para no tocar controles de un form ya cerrado
        }
        private static void MostrarErrorBD(SqlException ex)
        {
            MessageBox.Show($"Error de base de datos:\n{ex.Message}", "Error",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private async void btnGuardar_Click(object sender, EventArgs e)
        {


            try
            {
                // 1. Validar los controles
                if (!ValidarControlesFormulario())
                    return;

                // 2. Construir el objeto
                Estudiante estudiante =
                    ConstruirEstudianteDesdeFormulario();

                // 3. Validar las reglas del modelo
                if (!ValidarFormulario(estudiante))
                    return;

                // NUEVO ESTUDIANTE
                if (estudianteEnEdicion == null)
                {
                    if (estudianteRepository
                        .ExisteCarnet(estudiante.Carnet))
                    {
                        errorProvider1.SetError(
                            txtCarnet,
                            "El carnet ya existe en la base de datos.");

                        return;
                    }

                    estudianteRepository.Insertar(
                        estudiante);

                    MessageBox.Show(
                        "Estudiante guardado correctamente.",
                        "Registro",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);

                    LimpiarFormulario();
                }
                else
                {
                    estudiante.Id =
                        estudianteEnEdicion.Id;

                    estudiante.EsInterno =
                        estudianteEnEdicion.EsInterno;

                    estudiante.Activo =
                        estudianteEnEdicion.Activo;

                    estudianteRepository.Actualizar(
                        estudiante);

                    MessageBox.Show(
                        "Estudiante actualizado correctamente.",
                        "Actualización",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);

                    Close();
                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show(
                    $"Error de base de datos:\n{ex.Message}",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    ex.Message,
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void LimpiarFormulario()
        {
            // Datos personales
            txtCarnet.Clear();
            txtCedula.Clear();
            txtNombres.Clear();
            txtApellidos.Clear();

            cboSexo.SelectedIndex = -1;

            dtpFechaNacimiento.Value =
                DateTime.Today.AddYears(-18);

            // Nacionalidad
            cboNacionalidad.SelectedIndex = -1;
            cboNacionalidad.Text = string.Empty;

            // Nivel académico
            cboNivelAcademico.SelectedIndex = -1;

            // Tutor
            chkTieneTutor.Checked = false;

            // Área y Carrera
            cboArea.SelectedIndex = -1;

            cboCarrera.DataSource = null;
            cboCarrera.Enabled = false;

            // Departamento y Municipio
            cboDepartamento.SelectedIndex = -1;

            cboMunicipio.DataSource = null;
            cboMunicipio.Enabled = false;

            // Etnia
            cboEtnia.SelectedIndex = -1;
            cboEtnia.Text = string.Empty;

            // Datos adicionales
            txtCorreo.Clear();

            nudPromedio.Value =
                nudPromedio.Minimum;

            chkDiscapacidad.Checked = false;

            txtDescripcionDiscapacidad.Clear();
            txtDescripcionDiscapacidad.Enabled = false;

            // Validaciones
            errorProvider1.Clear();

            // Por seguridad, si el método se reutiliza
            txtCarnet.ReadOnly = false;

            txtCarnet.Focus();
        }

        private void cboDepartamento_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboDepartamento.SelectedValue is int idDepartamento)
            {
                CargarMunicipios(idDepartamento);
            }
            else
            {
                cboMunicipio.DataSource = null;
            }
        }
        private async Task CargarCatalogosInicialesAsync()
        {
            Task<List<OpcionCatalogo>> tareaAreas =
            catalogoRepository.ListarAreasasync();
            Task<List<OpcionCatalogo>> tareaDepartamentos =
            catalogoRepository.ListarDepartamentosAsync();
            List<OpcionCatalogo>[] resultados =
            await Task.WhenAll(
            tareaAreas,
            tareaDepartamentos);
            cboArea.DisplayMember = "Nombre";
            cboArea.ValueMember = "Id";
            cboArea.DataSource = resultados[0];
            cboArea.SelectedIndex = -1;
            cboDepartamento.DisplayMember = "Nombre";
            cboDepartamento.ValueMember = "Id";
            cboDepartamento.DataSource = resultados[1];
            cboDepartamento.SelectedIndex = -1;
        }

       /* private async void FrmRegistroEstudiante_Load(object sender, EventArgs e)
        {
            try
            {
                UseWaitCursor = true;
                cargandoCatalogos = true;   // evita que los SelectedIndexChanged disparen cargas

                await CargarCatalogosInicialesAsync();

                if (estudianteEnEdicion != null)
                    await CargarDatosEstudianteAsync();
            }
            catch (SqlException ex)
            {
                MostrarErrorBD(ex);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                cargandoCatalogos = false;
                UseWaitCursor = false;
        }
            }*/
    }
}
