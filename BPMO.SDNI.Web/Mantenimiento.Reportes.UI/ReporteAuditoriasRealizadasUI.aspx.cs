//Satisface al caso de uso CU071 - Reporte de Auditorias Realizadas
using System;
using BPMO.Primitivos.Enumeradores;
using BPMO.SDNI.Mantenimiento.Reportes.VIS;
using BPMO.SDNI.Mantenimientos.Reportes.PRE;
using ReportPage = BPMO.SDNI.Reportes.UI.Page;
using System.Web.UI;

namespace BPMO.SDNI.Mantenimiento.Reportes.UI
{
    /// <summary>
    /// Reporte de auditorias realizadas
    /// </summary>
    public partial class ReporteAuditoriasRealizadasUI : ReportPage, IReporteAuditoriasRealizadasVIS
    {
        #region Atributos
        /// <summary>
        /// Nombre de la clase en curso
        /// </summary>
        private static readonly String NombreClase = typeof(ReporteAuditoriasRealizadasUI).Name;
        /// <summary>
        /// Presentador que esta atendiendo las peticiones de la vista
        /// </summary>
        private ReporteAuditoriasRealizadasPRE presentador;
        #endregion
        #region Propiedades
        /// <summary>
        /// Identificador de la Sucursal
        /// </summary>
        public int? SucursalID
        {
            get { return this.Master.SucursalID; }
            set { this.Master.SucursalID = value; }
        }

        /// <summary>
        /// Nombre de la sucursal
        /// </summary>
        public string SucursalNombre
        {
            get { return this.Master.SucursalNombre; }
            set { this.Master.SucursalNombre = value; }
        } 

        /// <summary>
        /// Fecha inicial
        /// </summary>
        public DateTime? FechaInicio
        {
            get { return this.Master.FechaInicioContrato1; }
            set { this.Master.FechaInicioContrato1 = value; }
        }

        /// <summary>
        /// Fecha Fin
        /// </summary>
        public DateTime? FechaFin
        {
            get { return this.Master.FechaInicioContrato2; }
            set { this.Master.FechaInicioContrato2 = value; }
        }

        /// <summary>
        /// Identificador del Tecnico
        /// </summary>
        public int? TecnicoID 
        {
            get
            {
                return this.Master.TecnicoID;
            }
            set
            {
                this.Master.TecnicoID = value;
            }
        }

        /// <summary>
        /// Nombre del Tecnico
        /// </summary>
        public string TecnicoNombre
        {
            get
            {
                return this.Master.TecnicoNombre;
            }
            set
            {
                this.Master.TecnicoNombre = value;
            }
        }

        #endregion
        #region Constructor
        protected void Page_Load(object sender, EventArgs e)
        {
            this.presentador = new ReporteAuditoriasRealizadasPRE(this);
            if (!this.IsPostBack)
            {
                //this.presentador.ValidarAcceso();
                //Se definen los filtros Visibles                
                this.Master.SucursalFiltroVisible = true;
                this.Master.FechaInicioContratoFiltroVisible = true;
                this.Master.FechaInicioContratoEtiqueta = "Rango Fechas";
                this.Master.FechaInicioContratoFiltroRequerido = true;

                this.Master.TecnicoFiltroVisible = true;
                this.Master.TecnicoFiltroRequerido = false;

                this.presentador.PreparaConsulta();
            }
        } 
        #endregion
        #region Metodos
        /// <summary>
        /// Método para consultar el reporte
        /// </summary>
        public override void Consultar()
        {
            try
            {
                this.presentador.Consultar();
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencias al consultar el Reporte", ETipoMensajeIU.ERROR, NombreClase + ex.Message);
            }
        } 
        #endregion
        #region Eventos
        #endregion
    }
}