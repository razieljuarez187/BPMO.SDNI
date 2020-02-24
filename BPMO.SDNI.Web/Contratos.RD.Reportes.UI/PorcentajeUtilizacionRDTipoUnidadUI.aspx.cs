// Satisface el CU025 – Reporte Porcentaje Utilización de Renta Diaria por Tipo de Unidad

using System;
using BPMO.Primitivos.Enumeradores;
using BPMO.SDNI.Contratos.RD.Reportes.PRE;
using BPMO.SDNI.Contratos.RD.Reportes.VIS;
using ReportPage = BPMO.SDNI.Reportes.UI.Page;

namespace BPMO.SDNI.Contratos.RD.Reportes.UI
{
    /// <summary>
    /// Vista de consulta para el Reporte de Porcentaje de utilización de Renta Diaria por Tipo de Unidad
    /// </summary>
    public partial class PorcentajeUtilizacionRDTipoUnidadUI : ReportPage, IPorcentajeUtilizacionRDVIS
    {
        #region Constantes
        /// <summary>
        /// Nombre de la clase en curso
        /// </summary>
        private static readonly String nombreClase = typeof(PorcentajeUtilizacionRDTipoUnidadUI).Name;
        #endregion

        #region Campos
        /// <summary>
        /// Presentador que esta atendiendo las peticiones de la vista
        /// </summary>
        private PorcentajeUtilizacionRDPRE presentador;
        #endregion

        #region Propiedades
        /// <summary>
        /// Obtiene o establece un valor que representa el identificador de sucursal
        /// </summary>
        /// <value>Objeto de tipo Nullable de Int32</value>
        public int? SucursalID
        {
            get
            {
                return this.Master.SucursalID;
            }
            set
            {
                this.Master.SucursalID = value;
            }
        }

        /// <summary>
        /// Obtiene o establece un valor que representa el nombre de la sucursal para las que aplica la configuración
        /// </summary>
        /// <value>Objeto de tipo String</value>
        public string SucursalNombre
        {
            get
            {
                return this.Master.SucursalNombre;
            }
            set
            {
                this.Master.SucursalNombre = value;
            }
        }

        /// <summary>
        /// Obtiene o establece un valor que representa el año de la fecha de inicio
        /// </summary>
        /// <value>Objeto de tipo Int32</value>
        public int? AnioFechaInicio
        {
            get
            {
                return this.Master.AnioFechaInicio;
            }
            set
            {
                this.Master.AnioFechaInicio = value;
            }
        }

        /// <summary>
        /// Obtiene o establece un valor que representa el mes de la fecha de inicio
        /// </summary>
        /// <value>Objeto de tipo Int32</value>
        public int? MesFechaInicio
        {
            get
            {
                return this.Master.MesFechaInicio;
            }
            set
            {
                this.Master.MesFechaInicio = value;
            }
        }

        /// <summary>
        /// Obtiene o establece un valor que representa el año de la fecha de fin
        /// </summary>
        /// <value>Objeto de tipo Int32</value>
        public int? AnioFechaFin
        {
            get
            {
                return this.Master.AnioFechaFin;
            }
            set
            {
                this.Master.AnioFechaFin = value;
            }
        }

        /// <summary>
        /// Obtiene o establece un valor que representa el mes de la fecha de fin
        /// </summary>
        /// <value>Objeto de tipo Int32</value>
        public int? MesFechaFin
        {
            get
            {
                return this.Master.MesFechaFin;
            }
            set
            {
                this.Master.MesFechaFin = value;
            }
        }

        /// <summary>
        /// Obtiene o establece un valor que representa el identificador del reporte a visualizar
        /// </summary>
        /// <value>Objeto de tipo String</value>
        public String IdentificadorReporte
        {
            get { return "BEP1401.CU025"; }
        }

        /// <summary>
        /// Obtiene o establece un valor que representa el tipo de equipo aliado que debe de tener las unidades a mostrar
        /// </summary>
        /// <value>Objeto de tipo ETipoEquipoAliado</value>
        public ETipoEquipoAliado? TipoEquipoAliado 
        {
            get { return null; }
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
            this.presentador = new PorcentajeUtilizacionRDPRE(this);
            if (!this.IsPostBack)
            {
                //this.presentador.ValidarAcceso();

                //Se definen los filtros Visibles                
                this.Master.SucursalFiltroVisible = true;                
                this.Master.FechaInicioFiltroVisible = true;
                this.Master.FechaFinFiltroVisible = true;

                this.Master.FechaInicioFiltroRequerido = true;
                this.Master.FechaFinFiltroRequerido = true;
            }
        }
        #endregion
    }
}