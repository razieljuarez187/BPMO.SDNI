//Satisface el CU021 - Reporte de Contratos de Mantenimiento Registrados
using System;
using BPMO.Primitivos.Enumeradores;
using BPMO.SDNI.Contratos.BO;
using BPMO.SDNI.Contratos.Mantto.Reportes.PRE;
using BPMO.SDNI.Contratos.Mantto.Reportes.VIS;
using ReportPage = BPMO.SDNI.Reportes.UI.Page;

namespace BPMO.SDNI.Contratos.Mantto.CM.Reportes.UI
{
    /// <summary>
    /// Vista de consulta para el reporte de Contratos de Mantenimiento Registrados
    /// </summary>
    public partial class ContratosMantenimientoRegistradosUI : ReportPage, IContratosRegistradosManttoVIS
    {
        #region Constantes
        /// <summary>
        /// Nombre de la clase en uso
        /// </summary>
        private static readonly String nombreClase = typeof(ContratosMantenimientoRegistradosUI).Name;
        #endregion

        #region Propiedades
        /// <summary>
        /// Presentador que esta atendiendo las peticiones de la vista
        /// </summary>
        private ContratosRegistradosManttoPRE presentador;

        /// <summary>
        /// Obtiene o establece un valor que representa el identificador del modelo
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
        /// Obtiene o establece un valor que representa el identificador de cliente
        /// </summary>
        /// <value>Objeto de tipo Nullable de Int32</value>
        public int? ClienteID
        {
            get { return this.Master.ClienteID; }
            set { this.Master.ClienteID = value; }
        }

        /// <summary>
        /// Obtiene o establece un valor que representa el identificador de la cuenta cliente
        /// </summary>
        /// <value>Objeto de tipo Nullable de Int32</value>
        public int? CuentaClienteID
        {
            get { return this.Master.CuentaClienteID; }
            set { this.Master.CuentaClienteID = value; }
        }

        /// <summary>
        /// Obtiene o establece un valor que representa el nombre del cliente
        /// </summary>
        /// <value>Objeto de tipo String</value>
        public String ClienteNombre
        {
            get { return this.Master.ClienteNombre; }
            set { this.Master.ClienteNombre = value; }
        }

        /// <summary>
        /// Obtiene o establece un valor que representa la fecha 1 de inicio de contrato
        /// </summary>
        /// <value>Objeto de tipo DateTime</value>
        public DateTime? FechaInicioContrato1
        {
            get { return this.Master.FechaInicioContrato1; }
            set { this.Master.FechaInicioContrato1 = value; }
        }

        /// <summary>
        /// Obtiene o establece un valor que representa la fecha 2 de inicio de contrato
        /// </summary>
        /// <value>Objeto de tipo DateTime</value>
        public DateTime? FechaInicioContrato2
        {
            get { return this.Master.FechaInicioContrato2; }
            set { this.Master.FechaInicioContrato2 = value; }
        }

        /// <summary>
        /// Obtiene o establece un valor que representa la fecha 1 de fin de contrato
        /// </summary>
        /// <value>Objeto de tipo DateTime</value>
        public DateTime? FechaFinContrato1
        {
            get { return this.Master.FechaFinContrato1; }
            set { this.Master.FechaFinContrato1 = value; }
        }

        /// <summary>
        /// Obtiene o establece un valor que representa la fecha 2 de fin de contrato
        /// </summary>
        /// <value>Objeto de tipo DateTime</value>
        public DateTime? FechaFinContrato2
        {
            get { return this.Master.FechaFinContrato2; }
            set { this.Master.FechaFinContrato2 = value; }
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

        /// <summary>
        /// Obtiene un valor que establece el tipo de contrato que la vista esta procesando
        /// </summary>
        /// <value>Objeto de tipo ETipoContrato</value>
        public ETipoContrato? TipoContrato
        {
            get { return ETipoContrato.CM; }
        }

        /// <summary>
        /// Obtiene o establece un valor que representa el identificador del reporte a visualizar
        /// </summary>
        /// <value>Objeto de tipo String</value>
        public String IdentificadorReporte
        {
            get { return "BEP1401.CU021"; }
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
            this.presentador = new ContratosRegistradosManttoPRE(this);
            if(!this.IsPostBack)
            {
                this.presentador.ValidarAcceso();

                //Se definen los filtros Visibles                
                this.Master.SucursalFiltroVisible = true;
                this.Master.ModeloFiltroVisible = true;
                this.Master.ClienteFiltroVisible = true;
                this.Master.FechaInicioContratoFiltroVisible = true;
                this.Master.FechaFinContratoFiltroVisible = true;
                this.Master.AnioFiltroVisible = true;
            }
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
            catch(Exception ex)
            {
                this.MostrarMensaje("Inconsistencias en la configuracion", ETipoMensajeIU.ERROR, nombreClase + ex.Message);
            }
        }
        #endregion
    }
}