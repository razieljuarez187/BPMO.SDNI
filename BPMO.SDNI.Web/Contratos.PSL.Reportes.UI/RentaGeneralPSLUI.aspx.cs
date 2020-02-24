using System;
using BPMO.SDNI.Contratos.PSL.Reportes.PRE;
using BPMO.SDNI.Contratos.PSL.Reportes.VIS;
using ReportPage = BPMO.SDNI.Reportes.UI.Page;
using BPMO.Primitivos.Enumeradores;

namespace BPMO.SDNI.Contratos.PSL.Reportes.UI {

    public partial class RentaGeneralPSLUI : ReportPage, IRentaGeneralPSLVIS {
        #region Constantes
        /// <summary>
        /// Nombre de la clase en curso
        /// </summary>
        private static readonly String nombreClase = typeof(RentaGeneralPSLUI).Name;
        #endregion

        #region Campos
        /// <summary>
        /// Presentador que esta atendiendo las peticiones de la vista
        /// </summary>
        private RentaGeneralPSLPRE presentador;
        #endregion

        #region Propiedades
        /// <summary>
        /// Obtiene o establece un valor que representa el identificador de sucursal
        /// </summary>
        public int? SucursalID
        {
            get { return this.Master.SucursalID; }
            set { this.Master.SucursalID = value; }
        }

        /// <summary>
        /// Obtiene o establece un valor que representa el nombre de la sucursal
        /// </summary>
        public String SucursalNombre
        {
            get { return this.Master.SucursalNombre; }
            set { this.Master.SucursalNombre = value; }
        }

        /// <summary>
        /// Obtiene el Anio
        /// </summary>
        public int? Anio
        {
            get { return this.Master.Anio; }
            set { this.Master.Anio = value; }
        }

        /// <summary>
        /// Obtiene el Tipo de Reporte que se generará
        /// </summary>
        public int? TipoReporte
        {
            get { return this.Master.TipoReporte; }
            set { this.Master.TipoReporte = value; }
        }

        /// <summary>
        /// Obtiene el Periodo del Reporte a presentar
        /// </summary>
        public int? PeriodoReporte
        {
            get { return this.Master.PeriodoReporte; }
            set { this.Master.PeriodoReporte = value; }
        }

        /// <summary>
        /// Obtiene el dia final a presentar
        /// </summary>
        public int? DiaCorte
        {
            get { return this.Master.DiaCorte; } 
            set { this.Master.DiaCorte = value; }
        }
        #endregion
        
        #region Métodos
        /// <summary>
        /// Método para consultar el reporte
        /// </summary>
        public override void Consultar()
        {
            try
            {
                this.presentador.Consultar();
            }
            catch(Exception ex)
            {
                this.MostrarMensaje("Inconsistencias en la configuracion", ETipoMensajeIU.ERROR, nombreClase + ex.Message);
            }
        }
        #endregion

        #region Controladores de Eventos
        /// <summary>
        /// Evento que se ejecuta cuando se carga la página
        /// </summary>
        /// <param name="sender">Objeto que genero el evento</param>
        /// <param name="e">Argumentos asociados al evento</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            try {
                this.presentador = new RentaGeneralPSLPRE(this);
                if (!this.IsPostBack) {
                    //this.presentador.ValidarAcceso();
                    this.Master.AnioEtiqueta = "Año";
                    //Se definen los filtros Visibles                
                    this.Master.SucursalFiltroVisible = true;
                    this.Master.AnioFiltroVisible = true;
                    this.Master.AnioFiltroRequerido = true;
                    this.Master.DiaCorteFiltroVisible = true;
                    this.Master.PeriodoReporteFiltroVisible = true;
                    this.Master.PeriodoReporteFiltroRequerido = true;
                }
            } catch (Exception ex) {
                this.MostrarMensaje("Inconsistencia al crear la página", ETipoMensajeIU.ERROR, string.Format("{0}.Page_Load:{1}", nombreClase, ex.Message));
            }
        }
        #endregion
    }
}