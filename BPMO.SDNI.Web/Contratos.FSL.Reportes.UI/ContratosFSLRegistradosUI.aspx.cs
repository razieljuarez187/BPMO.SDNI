// Satisface al caso de uso CU020 - Reporte de Contratos FSL Registrados
using System;
using BPMO.Primitivos.Enumeradores;
using BPMO.SDNI.Contratos.FSL.Reportes.PRE;
using BPMO.SDNI.Contratos.FSL.Reportes.VIS;
using ReportPage = BPMO.SDNI.Reportes.UI.Page;

namespace BPMO.SDNI.Contratos.FSL.Reportes.UI
{
	/// <summary>
    /// Clase que contien los metodos para la presentacion de la informacion al usuario
    /// </summary>
    public partial class ContratosFSLRegistradosUI : ReportPage, IContratosFSLRegistradosVIS
    {
        #region Constantes

        /// <summary>
        /// Nombre de la clase en curso
        /// </summary>
        private static readonly String nombreClase = typeof(ContratosFSLRegistradosUI).Name;

        /// <summary>
        /// Codigo de Navegación del Reporte
        /// </summary>
        public string CodigoReporte
        {
            get { return "BEP1401.CU020"; }
        }

        #endregion

        #region Atributos
        /// <summary>
        /// Presentador que esta atendiendo las peticiones de la vista
        /// </summary>
        private ContratosFSLRegistradosPRE presentador; 

        #endregion

        #region Propiedades
		/// <summary>
        /// Identificador del modelo de la Unidad
        /// </summary>
        public int? ModeloID
        {
            get { return Master.ModeloID; }
            set { Master.ModeloID = value; }
        }
		/// <summary>
        /// Nombre del Modelo de la Unidad
        /// </summary>
        public string ModeloNombre
        {
            get { return Master.ModeloNombre; }
            set { Master.ModeloNombre = value; }
        }
		/// <summary>
        /// Identificador de la Sucursal
        /// </summary>
        public int? SucursalID
        {
            get { return Master.SucursalID; }
            set { Master.SucursalID = value; }
        }
		/// <summary>
        /// Nombre de la Sucursal
        /// </summary>
        public string SucursalNombre
        {
            get { return Master.SucursalNombre; }
            set { Master.SucursalNombre = value; }
        }
		/// <summary>
        /// Año de la Unidad
        /// </summary>
        public int? Anio
        {
            get { return Master.Anio; }
            set { Master.Anio = value; }
        }
		/// <summary>
        /// Identificador de CuentaCliente del Cliente
        /// </summary>
        public int? CuentaClienteID
        {
            get { return Master.CuentaClienteID; }
            set { Master.CuentaClienteID = value; }
        }
		/// <summary>
        /// Nombre del cliente
        /// </summary>
        public string CuentaClienteNombre
        {
            get { return Master.ClienteNombre; }
            set { Master.ClienteNombre = value; }
        }
		/// <summary>
        /// Fecha Incial del Rango de Inicio de contrato
        /// </summary>
        public DateTime? FechaInicioContratoInicial
        {
            get { return Master.FechaInicioContrato1; }
            set { Master.FechaInicioContrato1 = value; }
        }
		/// <summary>
        /// Fecha final del rango de inicio de contrato
        /// </summary>
        public DateTime? FechaInicioContratoFinal
        {
            get { return Master.FechaInicioContrato2; }
            set { Master.FechaInicioContrato2 = value; }
        }
		/// <summary>
        /// Fecha inicial del rango de fin del contrato
        /// </summary>
        public DateTime? FechaFinContratoInicial
        {
            get { return Master.FechaFinContrato1; }
            set { Master.FechaFinContrato1 = value; }
        }
		/// <summary>
        /// Fecha final del rango de fin del contrato
        /// </summary>
        public DateTime? FechaFinContratoFinal
        {
            get { return Master.FechaFinContrato2; }
            set { Master.FechaFinContrato2 = value; }
        } 

        #endregion

        #region Eventos
        /// <summary>
        /// Carga Inicial de la Pagina
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            presentador = new ContratosFSLRegistradosPRE(this);

            if (!IsPostBack)
            {
                presentador.ValidarAcceso();

                //Se definen los filtros Visibles                
                Master.SucursalFiltroVisible = true;
                Master.ModeloFiltroVisible = true;
                Master.AnioFiltroVisible = true;
                Master.ClienteFiltroVisible = true;
                Master.AnioFiltroVisible = true;
                Master.FechaInicioContratoFiltroVisible = true;
                Master.FechaFinContratoFiltroVisible = true;
            }
        } 
        #endregion

        #region Métodos
		/// <summary>
        /// Metodo usado para consultar los contratos
        /// </summary>
        public override void Consultar()
        {
            try
            {
                presentador.Consultar();
            }
            catch (Exception ex)
            {
                MostrarMensaje("Inconsistencias en la configuracion", ETipoMensajeIU.ERROR, nombreClase + ex.Message);
            }
        }        
        #endregion
    }
}