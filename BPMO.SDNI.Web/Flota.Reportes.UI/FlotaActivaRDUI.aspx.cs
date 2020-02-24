//Satisface el CU019 - Reporte de Flota Activa de RD Registrados
using System;
using BPMO.Primitivos.Enumeradores;
using BPMO.SDNI.Flota.Reportes.VIS;
using BPMO.SDNI.Flota.Reportes.PRE;
using ReportPage = BPMO.SDNI.Reportes.UI.Page;

namespace BPMO.SDNI.Flota.Reportes.UI
{
    /// <summary>
    /// Vista de consulta para el reporte de Flota Activa de RD Registrados
    /// </summary>
    public partial class FlotaActivaRDUI : ReportPage, IFlotaActivaRDVIS
    {
        #region Constantes
        /// <summary>
        /// Nombre de la clase en curso
        /// </summary>
        private static readonly String nombreClase = typeof(FlotaActivaRDUI).Name;
        #endregion

        #region Campos
        /// <summary>
        /// Presentador que esta atendiendo las peticiones de la vista
        /// </summary>
        private FlotaActivaRDPRE presentador;
        #endregion

        #region Propiedades
        /// <summary>
        /// Obtiene o establece un valor que representa el identificador del ´modelo
        /// </summary>
        /// <value>Objeto de tipo Nullable de Int32</value>
        public int? ModeloID 
        {
            get { return this.Master.ModeloID; }
            set { this.Master.ModeloID = value; }
        }

        /// <summary>
        /// Obtiene o establece un valor que representa el nombre del modelo
        /// </summary>
        /// <value>Objeto de tipo String</value>
        public String ModeloNombre 
        {
            get { return this.Master.ModeloNombre; }
            set { this.Master.ModeloNombre = value; }
        }

        /// <summary>
        /// Obtiene o establece un valor que representa el identificador de sucursal
        /// </summary>
        /// <value>Objeto de tipo Nullable de Int32</value>
        public int? SucursalID
        {
            get { return this.Master.SucursalID; }
            set { this.Master.SucursalID = value; }
        }

        /// <summary>
        /// Obtiene o establece un valor que representa el nombre de la sucursal
        /// </summary>
        /// <value>Objeto de tipo String</value>
        public String SucursalNombre
        {
            get { return this.Master.SucursalNombre; }
            set { this.Master.SucursalNombre = value; }
        }

        /// <summary>
        /// Obtiene o establece un valor que representa el año
        /// </summary>
        /// <value>Valor de tipo ETipoContrato</value>
        public int? Anio
        {
            get { return this.Master.Anio; }
            set { this.Master.Anio = value; }
        }
        #endregion        

        #region Métodos
        /// <summary>
        /// Método abstract que se ejecuta cuando 
        /// </summary>
        public override void Consultar()
        {
            try
            {
                this.presentador.Consultar();               
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencias en la configuracion", ETipoMensajeIU.ERROR, nombreClase + ex.Message);
            }
        }
        #endregion

        #region Controladores de eventos
        /// <summary>
        /// Evento que se ejecuta cuando se carga la página
        /// </summary>
        /// <param name="sender">Objeto que genero el evento</param>
        /// <param name="e">Argumentos asociados al evento</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            this.presentador = new FlotaActivaRDPRE(this);
            if (!this.IsPostBack)
            {
                this.presentador.ValidarAcceso();

                //Se definen los filtros Visibles                
                this.Master.SucursalFiltroVisible = true;
                this.Master.ModeloFiltroVisible = true;
                this.Master.AnioFiltroVisible = true;                
            }
        }
        #endregion
    }
}