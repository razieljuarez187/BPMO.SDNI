//Satisface el CU024 - Reporte de Dollar Utilization
using System;
using BPMO.Primitivos.Enumeradores;
using BPMO.SDNI.Flota.Reportes.VIS;
using BPMO.SDNI.Flota.Reportes.PRE;
using ReportPage = BPMO.SDNI.Reportes.UI.Page;

namespace BPMO.SDNI.Flota.Reportes.UI
{
    /// <summary>
    /// Interfaz con los filtros para obtener el reporte de Dollar Utilization
    /// </summary>
    public partial class DollarUtilizationUI : ReportPage, IDollarUtilizationVIS
    {
        #region Constantes
        /// <summary>
        /// Nombre de la clase UI
        /// </summary>
        private static readonly String nombreClase = typeof(DollarUtilizationUI).Name;
        #endregion

        #region Campos
        /// <summary>
        /// Presentador que atiende las peticiones de la vista
        /// </summary>
        private DollarUtilizationPRE presentador;
        #endregion

        #region Propiedades
        /// <summary>
        /// Obtiene o establece un valor que representa el Identificador de la Sucursal
        /// </summary>
        /// <value>Valor del identificador de la Sucursal</value>
        public int? SucursalID
        {
            get { return this.Master.SucursalID; }
            set { this.Master.SucursalID = value; }
        }

        /// <summary>
        /// Obtiene o establece un valor que representa el año de la fecha de inicio que se aplicará al reporte.
        /// </summary>
        /// <value>Valor del año de fecha de inicio</value>
        public int? AnioFechaInicio
        {
            get { return this.Master.AnioFechaInicio; }
            set { this.Master.AnioFechaInicio = value; }
        }

        /// <summary>
        /// Obtiene o establece un valor que representa el año de la fecha de fin que se aplicará al reporte.
        /// </summary>
        /// <value>Valor del año de fecha de fin</value>
        public int? AnioFechaFin
        {
            get { return this.Master.AnioFechaFin; }
            set { this.Master.AnioFechaFin = value; }
        }

        /// <summary>
        /// Obtiene o establece un valor que representa el mes de la fecha de inicio que se aplicará al reporte.
        /// </summary>
        /// <value>Valor del mes de fecha de inicio</value>
        public int? MesFechaInicio
        {
            get { return this.Master.MesFechaInicio; }
            set { this.Master.MesFechaInicio = value; }
        }

        /// <summary>
        /// Obtiene o establece un valor que representa el mes de la fecha de fin que se aplicará al reporte.
        /// </summary>
        /// <value>Valor del mes de fecha de fin</value>
        public int? MesFechaFin
        {
            get { return this.Master.MesFechaFin; }
            set { this.Master.MesFechaFin = value; }
        }
        #endregion

        #region Métodos
        /// <summary>
        /// Método abstracto que se ejecuta cuando se consulta el reporte
        /// </summary>
        public override void Consultar()
        {
            try
            {
                this.presentador.Consultar();
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencias en la configuración", ETipoMensajeIU.ERROR, nombreClase + ex.Message);
            }
        }
        #endregion

        #region Controladores de eventos
        /// <summary>
        /// Evento que se ejecuta cuando carga la página.
        /// </summary>
        /// <param name="sender">Objeto que generó el evento</param>
        /// <param name="e">Argumentos asociados al evento.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            this.presentador = new DollarUtilizationPRE(this);
            if (!this.IsPostBack)
            {
                //this.presentador.ValidarAcceso();

                //Se definen los filtros visibles.
                this.Master.SucursalFiltroVisible = true;
                this.Master.FechaInicioFiltroVisible = true;
                this.Master.FechaFinFiltroVisible = true;
                //Se definen los filtros requeridos.
                this.Master.SucursalFiltroRequerido = false;
                this.Master.FechaInicioFiltroRequerido = true;
                this.Master.FechaFinFiltroRequerido = true;
            }
        }
        #endregion
    }
}