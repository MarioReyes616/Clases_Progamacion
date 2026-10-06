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
    public partial class FrmListaEstudiantes : Form
    {
        private CancellationTokenSource? cts;

        private readonly EstudianteRepository estudianteRepository = new();
        public FrmListaEstudiantes()
        {
            InitializeComponent();

         

            ConfigurarGrid();

        }
        private async void btnResumenPLINQ_Click(object sender, EventArgs e)
        {
            List<Estudiante> estudiantes = ObtenerEstudiantesVisibles();

            if (estudiantes.Count == 0)
            {
                MessageBox.Show("No hay estudiantes para analizar.");
                return;
            }

            lblEstado.Text = "Calculando resumen...";

            List<object> resumen = await CrearResumenPorCarreraAsync(estudiantes);

            // Aquí ya estás de vuelta en el hilo UI, es seguro tocar controles
            EscribirLog($"Resumen PLINQ: {resumen.Count} carreras.");
            foreach (var item in resumen)
                EscribirLog(item.ToString()!);

            lblEstado.Text = "Resumen finalizado.";
        }
        private async Task<List<object>> CrearResumenPorCarreraAsync(
 List<Estudiante> estudiantes)
        {
            return await Task.Run(() =>
            estudiantes
            .AsParallel()
            .Where(e => e.Activo)
            .GroupBy(e => e.Carrera)
            .Select(grupo => (object)new
            {
                Carrera = grupo.Key,
                Cantidad = grupo.Count(),
                Promedio = grupo.Average(e => e.Promedio)
            })
            .ToList());
        }
        private void ConfigurarGrid()
        {
            dgvEstudiantes.AutoGenerateColumns = false;
            Carnet.DataPropertyName = "Carnet";
            Nombrecompleto.DataPropertyName = "NombreCompleto";
            Sexo.DataPropertyName = "Sexo";
            Carrera.DataPropertyName = "Carrera";
            Nivel.DataPropertyName = "NivelAcademico";
            Correo.DataPropertyName = "Correo";
            Promedio.DataPropertyName = "Promedio";
        }




        private void CargarEstudiantes()
        {
            dgvEstudiantes.AutoGenerateColumns = false;

            Carnet.DataPropertyName = "Carnet";
            Nombrecompleto.DataPropertyName = "NombreCompleto";
            Sexo.DataPropertyName = "Sexo";
            Carrera.DataPropertyName = "Carrera";
            Nivel.DataPropertyName = "NivelAcademico";
            Correo.DataPropertyName = "Correo";
            Promedio.DataPropertyName = "Promedio";

            dgvEstudiantes.DataSource = null;
            dgvEstudiantes.DataSource =
                estudianteRepository.Listar();
        }

        private Estudiante? ObtenerSeleccionado()
        {
            return dgvEstudiantes.CurrentRow?.DataBoundItem as Estudiante;
        }



        private void btnVerDetalle_Click(object sender, EventArgs e)
        {
            Estudiante? estudiante = ObtenerSeleccionado();

            if (estudiante is null)
            {
                MessageBox.Show("Seleccione un estudiante.");
                return;
            }

            using FrmDetalleEstudiante detalle = new(estudiante);
            detalle.ShowDialog(this);
        }

        private async void btnBuscar_Click(object sender, EventArgs e)
        {
            string texto = txtBuscar.Text.Trim();
            if (string.IsNullOrWhiteSpace(texto))
            {
                await CargarEstudiantesAsync();
                return;
            }
            try
            {
              
            lblEstado.Text = "Buscando...";
                List<Estudiante> resultados =
                await estudianteRepository.BuscarAsync(texto);
                dgvEstudiantes.DataSource = null;
                dgvEstudiantes.DataSource = resultados;
                lblEstado.Text = $"Resultados: {resultados.Count}";
            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.Message, "Error de búsqueda");
            }
        }
        

        private async void btnDesactivar_Click(object sender, EventArgs e)
        {
            if (dgvEstudiantes.CurrentRow == null)
            {
                MessageBox.Show("Seleccione un estudiante.");
                return;
            }

            Estudiante? estudiante =
                dgvEstudiantes.CurrentRow.DataBoundItem as Estudiante;

            if (estudiante == null)
            {
                MessageBox.Show("No se pudo obtener el estudiante seleccionado.");
                return;
            }

            DialogResult resultado = MessageBox.Show(
                $"¿Desea desactivar al estudiante {estudiante.NombreCompleto}?",
                "Confirmar desactivación",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (resultado != DialogResult.Yes)
                return;

            try
            {
                await estudianteRepository.DesactivarAsync(estudiante.Id);

                MessageBox.Show(
                    "Estudiante desactivado correctamente.",
                    "Éxito",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                await CargarEstudiantesAsync();
            }
            catch (SqlException ex)
            {
                MessageBox.Show(
                    $"Error de base de datos:\n{ex.Message}",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
            Estudiante? estudiante = ObtenerSeleccionado();

            if (estudiante == null)
            {
                MessageBox.Show(
                    "Seleccione un estudiante para editar.",
                    "Editar estudiante",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                return;
            }

            using FrmRegistroEstudiante formulario =
                new FrmRegistroEstudiante(estudiante);

            formulario.ShowDialog(this);

            // Refrescar la lista cuando se cierre
            CargarEstudiantes();

        }
        private List<Estudiante> ObtenerEstudiantesVisibles()
        {
            return dgvEstudiantes.Rows
                .Cast<DataGridViewRow>()
                .Select(fila => fila.DataBoundItem as Estudiante)
                .Where(estudiante => estudiante is not null)
                .Cast<Estudiante>()
                .ToList();
        }

        private void btnBloqueante_Click(object sender, EventArgs e)
        {
            List<Estudiante> estudiantes = ObtenerEstudiantesVisibles();
            for (int i = 0; i < estudiantes.Count; i++)
            {
                Thread.Sleep(300);//Solo simula una operación lenta
                lblEstado.Text = $"Procesando {i + 1} de {estudiantes.Count}";
            }
            MessageBox.Show("Proceso finalizado");
        }
        private void EscribirLog(string mensaje)
        {
            txtLog.AppendText(
            $"[{DateTime.Now:HH:mm:ss}] {mensaje}{Environment.NewLine}");
        }
        private void PrepararInicio(int total)
        {
            prgProceso.Minimum = 0;
            prgProceso.Maximum = Math.Max(1, total);
            prgProceso.Value = 0;
            btnProcesar.Enabled = false;
            btnCancelar.Enabled = true;
            lblEstado.Text = "Iniciando proceso...";
        }
        private void PrepararFin()
        {
            btnProcesar.Enabled = true;
            btnCancelar.Enabled = false;
        }
        private static void ProcesarEnSegundoPlano(
IReadOnlyList<Estudiante> estudiantes,
IProgress<ProgresoTarea> progreso,
CancellationToken token)
        {
            for (int i = 0; i < estudiantes.Count; i++)
            {
                token.ThrowIfCancellationRequested();
                Thread.Sleep(250); // Simulación de trabajo bloqueante.
                Estudiante estudiante = estudiantes[i];
                progreso.Report(new ProgresoTarea(
                i + 1,
                estudiantes.Count,
                $"Procesado: {estudiante.Carnet} - {estudiante.NombreCompleto}"));
            }
        }

        private async void btnProcesar_Click(object sender, EventArgs e)
        {
            if (cts is not null)
                return;
            List<Estudiante> estudiantes = ObtenerEstudiantesVisibles();
            if (estudiantes.Count == 0)
            {
                MessageBox.Show("No hay estudiantes para procesar.");
                return;
            }
            cts = new CancellationTokenSource();
            CancellationToken token = cts.Token;
            PrepararInicio(estudiantes.Count);
            Progress<ProgresoTarea> progreso = new(p =>
            {
                prgProceso.Value = p.Actual;
                lblEstado.Text = $"Procesando {p.Actual} de {p.Total}";
                EscribirLog(p.Mensaje);
            });
            try
            {
                await Task.Run(
                () => ProcesarEnSegundoPlano(
                estudiantes,
                progreso,
                token),
                token);
                lblEstado.Text = "Proceso finalizado correctamente.";
                EscribirLog("La tarea terminó correctamente.");
            }
            catch (OperationCanceledException)
            {
                lblEstado.Text = "Proceso cancelado por el usuario.";
                EscribirLog("La tarea fue cancelada.");
            }
            finally
            {
                cts.Dispose();
                cts = null;
                PrepararFin();
            }
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            cts?.Cancel();
        }

        private async void FrmListaEstudiantes_Load(object sender, EventArgs e)
        {
            await CargarEstudiantesAsync();
        }
        private async Task CargarEstudiantesAsync(
 CancellationToken token = default)
        {
            try
            {
                UseWaitCursor = true;
                lblEstado.Text = "Cargando estudiantes...";
                List<Estudiante> estudiantes =
                await estudianteRepository.ListarAsync(token);
                dgvEstudiantes.DataSource = null;
                dgvEstudiantes.DataSource = estudiantes;
                lblEstado.Text = $"{estudiantes.Count} estudiantes cargados.";
            }
            catch (OperationCanceledException)
            {
                lblEstado.Text = "Carga cancelada.";
            }
         
            catch (SqlException ex)
           {
                MessageBox.Show(
                $"Error de base de datos:\n{ex.Message}",
                "Error",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
            }
        finally
            {
                UseWaitCursor = false;
            }
        }
    }
}
