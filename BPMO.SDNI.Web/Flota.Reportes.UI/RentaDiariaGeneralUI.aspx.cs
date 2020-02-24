//Satisface al caso de uso CU016 - Reporte de renta diaria general
using System;
using BPMO.SDNI.Flota.Reportes.PRE;
using BPMO.SDNI.Flota.Reportes.VIS;
using ReportPage = BPMO.SDNI.Reportes.UI.Page;
using BPMO.Primitivos.Enumeradores;

namespace BPMO.SDNI.Flota.Reportes.UI
{
    /// <summary>
    /// Interfaz con los filtros para el reporte general de renta diaria
    /// </summary>
    public partial class RentaDiariaGeneralUI : ReportPage, IRentaDiariaGeneralVIS
    {
        #region Constantes
        /// <summary>
        /// Nombre de la clase en curso
        /// </summary>
        private static readonly String nombreClase = typeof(RentaDiariaGeneralUI).Name;
        #endregion

        #region Campos
        /// <summary>
        /// Presentador que esta atendiendo las peticiones de la vista
        /// </summary>
        private RentaDiariaGeneralPRE presentador;
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
            this.presentador = new RentaDiariaGeneralPRE(this);
            if(!this.IsPostBack)
            {
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
        }
        #endregion
    }
}