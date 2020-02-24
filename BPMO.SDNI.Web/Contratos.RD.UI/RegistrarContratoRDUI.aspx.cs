// Satisface al CU001 - Registrar Contrato de Renta Diaria
// Satisface a la RI0004
// BEP1401 Satisface a la SC0026
using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections.Generic;
using BPMO.Primitivos.Enumeradores;
using BPMO.SDNI.Comun.BO;
using BPMO.SDNI.Contratos.BO;
using BPMO.SDNI.Contratos.RD.PRE;
using BPMO.SDNI.Contratos.RD.VIS;
using BPMO.SDNI.MapaSitio.UI;

namespace BPMO.SDNI.Contratos.RD.UI
{
    public partial class RegistrarContratoRDUI : Page, IRegistrarContratoRDVIS
    {
        #region Atributos
        /// <summary>
        /// Presentador de la página de registro
        /// </summary>
        private RegistrarContratoRDPRE presentador;
        /// <summary>
        /// Nombre de la página que  se esta desplegando
        /// </summary>
        private const string nombreClase = "RegistrarContratoRDUI";
        #endregion

        #region Propiedades
        /// <summary>
        /// Obtiene o establece el identificador del contrato
        /// </summary>
        public int? ContratoID
        {
            get { return this.ucucInformacionGeneralRDUI.ContratoID; }
            set { this.ucucInformacionGeneralRDUI.ContratoID = value; }
        }
        /// <summary>
        /// Retorna la fecha de creación del contrato
        /// </summary>
        public DateTime? FC
        {
            get { return DateTime.Now; }
        }
        /// <summary>
        /// Retorna la fecha de la ultima modificación del contrato
        /// </summary>
        public DateTime? FUA
        {
            get { return FC; }
        }
        /// <summary>
        /// Retorna a el usuario que crea el contrato
        /// </summary>
        public int? UC
        {
            get
            {
                int? id = null;
                var masterMsj = (Site)Page.Master;

                if (masterMsj != null && masterMsj.Usuario != null)
                    id = masterMsj.Usuario.Id;
                return id;
            }
        }
        /// <summary>
        /// Retorn a el usuario que actualiza por ultima vez el contrato
        /// </summary>
        public int? UUA
        {
            get { return UC; }
        }
        /// <summary>
        /// Obtiene o estabece el estatus del contrato
        /// </summary>
        public int? EstatusID
        {
            get { return ucucInformacionGeneralRDUI.EstatusID; }
            set { this.ucucInformacionGeneralRDUI.EstatusID = value; }
        }
        /// <summary>
        /// Usuario que se ha identificado en el Sistema
        /// </summary>
        public int? UsuarioID
        {
            get
            {
                var masterMsj = (Site)Page.Master;

                if (masterMsj != null && masterMsj.Usuario != null)
                    return masterMsj.Usuario.Id.Value;
                return null;
            }
        }
        /// <summary>
        /// Obtiene la Unidad operativa a la cual pertenece el usuario autenticado
        /// </summary> 
        public int? UnidadOperativaID
        {
            get
            {
                Site masterMsj = (Site)Page.Master;
                return masterMsj != null && masterMsj.Adscripcion != null && masterMsj.Adscripcion.UnidadOperativa != null
                        ? masterMsj.Adscripcion.UnidadOperativa.Id.Value : (int?)null;
            }
        }
        /// <summary>
        /// Obtiene la el nombre de la Unidad operativa a la cual pertenece el usuario autenticado
        /// </summary> 
        public string UnidadOperativaNombre
        {
            get
            {
                Site masterMsj = (Site)Page.Master;
                return masterMsj != null && masterMsj.Adscripcion != null && masterMsj.Adscripcion.UnidadOperativa != null
                    ? masterMsj.Adscripcion.UnidadOperativa.Nombre:string.Empty;
            }
        }
        /// <summary>
        /// Fecha en la que se ejecuta el contrato
        /// </summary>
        public DateTime? FechaContrato
        {
            get
            {
                DateTime? date = null;
                if (this.ucucInformacionGeneralRDUI.FechaContrato.HasValue)
                    date = this.ucucInformacionGeneralRDUI.FechaContrato;

                if (date.HasValue && this.ucucInformacionGeneralRDUI.HoraContrato.HasValue)
                    date = date.Value.Add(this.ucucInformacionGeneralRDUI.HoraContrato.Value);

                return date.HasValue ? date : null;
            }
            set { this.ucucInformacionGeneralRDUI.FechaContrato = value; }
        }
        /// <summary>
        /// Obtiene o establece el número del contrato
        /// </summary>
        public string NumeroContrato
        {
            get { return this.ucucInformacionGeneralRDUI.NumeroContrato; }
            set { this.ucucInformacionGeneralRDUI.NumeroContrato = value; }
        }
        /// <summary>
        /// Obtiene o establece el identificador de la sucursal de la cual se desean filtrar las unidades
        /// </summary>
        public int? SucursalID
        {
            get { return this.ucucInformacionGeneralRDUI.SucursalID; }
            set { this.ucucInformacionGeneralRDUI.SucursalID = value; }
        }
        /// <summary>
        /// Obtiene o establece el nombre de la sucursal seleccionada para el filtro de consulta
        /// </summary>
        public string SucursalNombre
        {
            get { return this.ucucInformacionGeneralRDUI.SucursalNombre; }
            set { this.ucucInformacionGeneralRDUI.SucursalNombre = value; }
        }
        /// <summary>
        /// Obtiene o establece el número de cuenta del cliente
        /// </summary>
        public int? CuentaClienteID
        {
            get { return this.ucucInformacionGeneralRDUI.CuentaClienteID; }
            set { this.ucucInformacionGeneralRDUI.CuentaClienteID = value; }
        }
        /// <summary>
        /// Obtiene o establece el nombre del cliente
        /// </summary>
        public string CuentaClienteNombre
        {
            get { return this.ucucInformacionGeneralRDUI.CuentaClienteNombre; }
            set { this.ucucInformacionGeneralRDUI.CuentaClienteNombre = value; }
        }
        /// <summary>
        /// Obtiene o establece el tipo de cuenta del cliente
        /// </summary>
        public int? CuentaClienteTipoID
        {
            get { return this.ucucInformacionGeneralRDUI.CuentaClienteTipoID; }
            set { this.ucucInformacionGeneralRDUI.CuentaClienteTipoID = value; }
        }
        /// <summary>
        /// Id de la Direccion del Cliente
        /// </summary>
        public int? ClienteDireccionId 
        {
            get { return this.ucucInformacionGeneralRDUI.ClienteDireccionClienteID; }
            set { this.ucucInformacionGeneralRDUI.ClienteDireccionClienteID = value; }
        }
        /// <summary>
        /// Obtiene o establece la calle de la dirección del cliente
        /// </summary>
        public string ClienteDireccionCalle
        {
            get { return this.ucucInformacionGeneralRDUI.ClienteDireccionCalle; }
            set { this.ucucInformacionGeneralRDUI.ClienteDireccionCalle = value; }
        }
        /// <summary>
        /// Obtiene o establece el código postal de la direccion del cliente
        /// </summary>
        public string ClienteDireccionCodigoPostal
        {
            get { return this.ucucInformacionGeneralRDUI.ClienteDireccionCodigoPostal; }
            set { this.ucucInformacionGeneralRDUI.ClienteDireccionCodigoPostal = value; }
        }
        /// <summary>
        /// Obtiene o establece la ciudad de la dirección del cliente
        /// </summary>
        public string ClienteDireccionCiudad
        {
            get { return this.ucucInformacionGeneralRDUI.ClienteDireccionCiudad; }
            set { this.ucucInformacionGeneralRDUI.ClienteDireccionCiudad = value; }
        }
        /// <summary>
        /// Obtiene o establece el estado para la dirección del cliente
        /// </summary>
        public string ClienteDireccionEstado
        {
            get { return this.ucucInformacionGeneralRDUI.ClienteDireccionEstado; }
            set { this.ucucInformacionGeneralRDUI.ClienteDireccionEstado = value; }
        }
        /// <summary>
        /// Obtiene o establece el municipio para la dirección del cliente
        /// </summary>
        public string ClienteDireccionMunicipio
        {
            get { return this.ucucInformacionGeneralRDUI.ClienteDireccionMunicipio; }
            set { this.ucucInformacionGeneralRDUI.ClienteDireccionMunicipio = value; }
        }
        /// <summary>
        /// Obtiene o establece el país para la dirección del cliente
        /// </summary>
        public string ClienteDireccionPais
        {
            get { return this.ucucInformacionGeneralRDUI.ClienteDireccionPais; }
            set { this.ucucInformacionGeneralRDUI.ClienteDireccionPais = value; }
        }
        /// <summary>
        /// Obtiene o establece la colonia para al direccion del cliente
        /// </summary>
        public string ClienteDireccionColonia
        {
            get { return this.ucucInformacionGeneralRDUI.ClienteDireccionColonia; }
            set { this.ucucInformacionGeneralRDUI.ClienteDireccionColonia = value; }
        }
        /// <summary>
        /// Obtiene o establece los representantes legales seleccionados para el contrato
        /// </summary>
        public List<RepresentanteLegalBO> RepresentantesLegales
        {
            get { return this.ucucInformacionGeneralRDUI.RepresentantesSeleccionados; }
            set { this.ucucInformacionGeneralRDUI.RepresentantesSeleccionados = value; }
        }

		/// <summary>
		/// Obtiene o establece si el contrato solo tendrá representantes legales
		/// </summary>
		public bool? SoloRepresentantes { 
			get { return ucucInformacionGeneralRDUI.SoloRepresentantes; } 
			set { ucucInformacionGeneralRDUI.SoloRepresentantes = value; } }

		/// <summary>
		/// Obtiene o establece los avales seleccionados para el contrato
		/// </summary>
		public List<AvalBO> Avales
		{
			get { return this.ucucInformacionGeneralRDUI.AvalesSeleccionados; }
			set { this.ucucInformacionGeneralRDUI.AvalesSeleccionados = value; }
		}
        /// <summary>
        /// Codigo de la moneda selecionada
        /// </summary>
        public string CodigoMoneda
        {
            get { return this.ucucInformacionGeneralRDUI.CodigoMoneda; }
            set { this.ucucInformacionGeneralRDUI.CodigoMoneda = value; }
        }
        /// <summary>
        /// Obtiene o establece el tipo de autorización para el pago a crédito
        /// </summary>
        public int? TipoConfirmacionID
        {
            get { return this.ucucInformacionGeneralRDUI.TipoConfirmacionID; }
            set { this.ucucInformacionGeneralRDUI.TipoConfirmacionID = value; }
        }
        /// <summary>
        /// Obtiene o establece el tipo de lector de kilometraje de al unidad
        /// </summary>
        public int? LectorKilometrajeID
        {
            get { return this.ucucInformacionGeneralRDUI.LectorKilometrajeID; }
            set { this.ucucInformacionGeneralRDUI.LectorKilometrajeID = value; }
        }
        /// <summary>
        /// Obtiene o establece la forma de pago del contrato
        /// </summary>
        public int? FormaPagoID
        {
            get { return this.ucucInformacionGeneralRDUI.FormaPagoID; }
            set { this.ucucInformacionGeneralRDUI.FormaPagoID = value; }
        }
        /// <summary>
        /// Obtiene o establece el motivo de la renta de la unidad
        /// </summary>
        public int? MotivoRentaID
        {
            get { return this.ucucInformacionGeneralRDUI.MotivoRentaID; }
            set { this.ucucInformacionGeneralRDUI.MotivoRentaID = value; }
        }
        /// <summary>
        /// Obtiene o establece la frecuencia de facturación para el contrato
        /// </summary>
        public int? FrecuenciaFacturacionID
        {
            get { return this.ucucInformacionGeneralRDUI.FrecuenciaFacturacionID; }
            set { this.ucucInformacionGeneralRDUI.FrecuenciaFacturacionID = value; }
        }
        /// <summary>
        /// Nombre de la persona que autoriza el pago a crédito
        /// </summary>
        public string AutorizadorTipoConfirmacion
        {
            get { return this.ucucInformacionGeneralRDUI.AutorizadorTipoConfirmacion; }
            set { this.ucucInformacionGeneralRDUI.AutorizadorTipoConfirmacion = value; }
        }
        /// <summary>
        /// Obtiene o estabece el autorizador de la orden de compra
        /// </summary>
        public string AutorizadorOrdenCompra
        {
            get { return this.ucucInformacionGeneralRDUI.AutorizadorOrdenCompra; }
            set { this.ucucInformacionGeneralRDUI.AutorizadorOrdenCompra = value; }
        }
        /// <summary>
        /// Obtiene o establece la mercancia que se va a transportar en la unidad
        /// </summary>
        public string MercanciaTransportar
        {
            get { return this.ucucInformacionGeneralRDUI.MercanciaTransportar; }
            set { this.ucucInformacionGeneralRDUI.MercanciaTransportar = value; }
        }
        /// <summary>
        /// Obtiene o establece el destino o área de operación para la unidad
        /// </summary>
        public string DestinoAreaOperacion
        {
            get { return this.ucucInformacionGeneralRDUI.DestinoAreaOperacion; }
            set { this.ucucInformacionGeneralRDUI.DestinoAreaOperacion = value; }
        }
        /// <summary>
        /// Porcentaje Deducible para el seguro de la unidad
        /// </summary>
        public decimal? PorcentajeDeducible
        {
            get { return this.ucucInformacionGeneralRDUI.PorcentajeDeducible; }
            set { this.ucucInformacionGeneralRDUI.PorcentajeDeducible = value; }
        }
        /// <summary>
        /// Obtiene o establece si tiene habilitada la bitacora de viaje para el conductor
        /// </summary>
        public bool? BitacoraViajeConductor
        {
            get { return this.ucucInformacionGeneralRDUI.BitacoraViajeConductor; }
            set { this.ucucInformacionGeneralRDUI.BitacoraViajeConductor = value; }
        }
        /// <summary>
        /// Obtiene o establece la fecha de promesa de devolución de la unidad
        /// </summary>
        public DateTime? FechaPromesaDevolucion
        {
            get
            {
                DateTime? date = null;
                if (this.ucucInformacionGeneralRDUI.FechaPromesaDevolucion.HasValue)
                    date = this.ucucInformacionGeneralRDUI.FechaPromesaDevolucion;

                if (date.HasValue && this.ucucInformacionGeneralRDUI.HoraPromesaDevolucion.HasValue)
                    date = date.Value.Add(this.ucucInformacionGeneralRDUI.HoraPromesaDevolucion.Value);

                return date.HasValue ? date : null;
            }
            set { this.ucucInformacionGeneralRDUI.FechaPromesaDevolucion = value; }
        }
        /// <summary>
        /// Obtiene o establece las observaciones realizadas al contrato
        /// </summary>
        public string Observaciones
        {
            get { return this.ucucInformacionGeneralRDUI.Observaciones; }
            set { this.ucucInformacionGeneralRDUI.Observaciones = value; }
        }
        #region BEP1401.SC0026
        /// <summary>
        /// Número de días que se cobrarán en la primera Factura
        /// </summary>
        public Int32? DiasFacturar
        {
            get { return this.ucucInformacionGeneralRDUI.DiasFacturar; }
            set { this.ucucInformacionGeneralRDUI.DiasFacturar = value; }
        }
        #endregion
        #region SC_0013
        /// <summary>
        /// Obtiene o establece el identificador del ooperador
        /// </summary>
        public int? OperadorID
        {
            get { return this.ucucInformacionGeneralRDUI.OperadorID; }
            set { this.ucucInformacionGeneralRDUI.OperadorID = value; }
        }
        #endregion
        /// <summary>
        /// Obtiene o establece el nombre del operador
        /// </summary>
        public string OperadorNombre
        {
            get { return this.ucucInformacionGeneralRDUI.OperadorNombre; }
            set { this.ucucInformacionGeneralRDUI.OperadorNombre = value; }
        }
        /// <summary>
        /// Obtiene o establece la fecha de nacimiento del operador
        /// </summary>
        public DateTime? OperadorFechaNacimiento
        {
            get { return this.ucucInformacionGeneralRDUI.OperadorFechaNacimiento; }
            set { this.ucucInformacionGeneralRDUI.OperadorFechaNacimiento = value; }
        }
        /// <summary>
        /// Obtiene o establece los años de experiencia del operador
        /// </summary>
        public int? OperadorAniosExperiencia
        {
            get { return this.ucucInformacionGeneralRDUI.OperadorAniosExperiencia; }
            set { this.ucucInformacionGeneralRDUI.OperadorAniosExperiencia = value; }
        }
        /// <summary>
        /// Obtiene o establece el la calle de la dirección del operador
        /// </summary>
        public string OperadorDireccionCalle
        {
            get { return this.ucucInformacionGeneralRDUI.OperadorDireccionCalle; }
            set { this.ucucInformacionGeneralRDUI.OperadorDireccionCalle = value; }
        }
        /// <summary>
        /// Obtiene o establece la ciudad de la dirección del operador
        /// </summary>
        public string OperadorDireccionCiudad
        {
            get { return this.ucucInformacionGeneralRDUI.OperadorDireccionCiudad; }
            set { this.ucucInformacionGeneralRDUI.OperadorDireccionCiudad = value; }
        }
        /// <summary>
        /// Obtiene o establece el estado de la dirección del operador
        /// </summary>
        public string OperadorDireccionEstado
        {
            get { return this.ucucInformacionGeneralRDUI.OperadorDireccionEstado; }
            set { this.ucucInformacionGeneralRDUI.OperadorDireccionEstado = value; }
        }
        /// <summary>
        /// Obtiene o establece el codigo postal de la dirección del operador
        /// </summary>
        public string OperadorDireccionCP
        {
            get { return this.ucucInformacionGeneralRDUI.OperadorDireccionCP; }
            set { this.ucucInformacionGeneralRDUI.OperadorDireccionCP = value; }
        }
        /// <summary>
        /// Obtiene o establece el tipo de la licencia del operador
        /// </summary>
        public int? OperadorLicenciaTipoID
        {
            get { return this.ucucInformacionGeneralRDUI.OperadorLicenciaTipoID; }
            set { this.ucucInformacionGeneralRDUI.OperadorLicenciaTipoID = value; }
        }
        /// <summary>
        /// Obtiene o establece el número de la licencia del operador
        /// </summary>
        public string OperadorLicenciaNumero
        {
            get { return this.ucucInformacionGeneralRDUI.OperadorLicenciaNumero; }
            set { this.ucucInformacionGeneralRDUI.OperadorLicenciaNumero = value; }
        }
        /// <summary>
        /// Fecha de expiración de la licencia
        /// </summary>
        public DateTime? OperadorLicenciaFechaExpiracion
        {
            get { return this.ucucInformacionGeneralRDUI.OperadorLicenciaFechaExpiracion; }
            set { this.ucucInformacionGeneralRDUI.OperadorLicenciaFechaExpiracion = value; }
        }
        /// <summary>
        /// Obtiene o establece el estado de expedición de la licencia
        /// </summary>
        public string OperadorLicenciaEstado
        {
            get { return this.ucucInformacionGeneralRDUI.OperadorLicenciaEstado; }
            set { this.ucucInformacionGeneralRDUI.OperadorLicenciaEstado = value; }
        }
        /// <summary>
        /// Obtiene o establece el identificador de la unidad
        /// </summary>
        public int? UnidadID
        {
            get { return this.ucucInformacionGeneralRDUI.UnidadID; }
            set { this.ucucInformacionGeneralRDUI.UnidadID = value; }
        }
        /// <summary>
        /// obtiene o establece el identificador del equipo
        /// </summary>
        public int? EquipoID
        {
            get { return this.ucucInformacionGeneralRDUI.EquipoID; }
            set { this.ucucInformacionGeneralRDUI.EquipoID = value; }
        }
        /// <summary>
        /// Obtiene o establece el número de serie de la unidad que será rentada
        /// </summary>
        public string VIN
        {
            get { return this.ucucInformacionGeneralRDUI.VIN; }
            set { this.ucucInformacionGeneralRDUI.VIN = value; }
        }
        /// <summary>
        /// Obtiene o establece el número económico de la unidad que será rentada
        /// </summary>
        public string NumeroEconomico
        {
            get { return this.ucucInformacionGeneralRDUI.NumeroEconomico; }
            set { this.ucucInformacionGeneralRDUI.NumeroEconomico = value; }
        }
        /// <summary>
        /// Obtiene o establece el identificador del modelo
        /// </summary>
        public int? ModeloID
        {
            get { return this.ucucInformacionGeneralRDUI.ModeloID; }
            set { this.ucucInformacionGeneralRDUI.ModeloID = value; }
        }
        /// <summary>
        /// Obtiene o establece el nombre del modelo de la unidad
        /// </summary>
        public string ModeloNombre
        {
            get { return this.ucucInformacionGeneralRDUI.ModeloNombre; }
            set { this.ucucInformacionGeneralRDUI.ModeloNombre = value; }
        }
        /// <summary>
        /// Identificador de Producto o Servicio (SAT)
        /// </summary>
        public int? ProductoServicioId {
            get { return this.ucucInformacionGeneralRDUI.ProductoServicioId; }
            set { this.ucucInformacionGeneralRDUI.ProductoServicioId = value; }
        }
        /// <summary>
        /// Clave de Producto o Servicio (SAT)
        /// </summary>
        public string ClaveProductoServicio {
            get { return this.ucucInformacionGeneralRDUI.ClaveProductoServicio; }
            set { this.ucucInformacionGeneralRDUI.ClaveProductoServicio = value; }
        }
        /// <summary>
        /// Descripción de Producto o Servicio (SAT)
        /// </summary>
        public string DescripcionProductoServicio {
            get { return (string.IsNullOrEmpty(this.ucucInformacionGeneralRDUI.DescripcionProductoServicio)) ? null
                    : this.ucucInformacionGeneralRDUI.DescripcionProductoServicio.Trim().ToUpper(); }
            set { this.ucucInformacionGeneralRDUI.DescripcionProductoServicio = (value != null) ? value.ToUpper() : string.Empty; }
        }
        /// <summary>
        /// Obtiene o establece el identificador de la tarifa que aplicará a la unidad
        /// </summary>
        public int? TipoTarifaID
        {
            get { return this.ucucInformacionGeneralRDUI.TipoTarifaID; }
            set { this.ucucInformacionGeneralRDUI.TipoTarifaID = value; }
        }
        /// <summary>
        /// Obtiene o establece la capacidad de carga para la tarifa seleccionada
        /// </summary>
        public int? CapacidadCarga
        {
            get { return this.ucucInformacionGeneralRDUI.CapacidadCarga; }
            set { this.ucucInformacionGeneralRDUI.CapacidadCarga = value; }
        }
        /// <summary>
        /// Obtiene o establece el importe de la tarifa diaria
        /// </summary>
        public decimal? TarifaDiaria
        {
            get { return this.ucucInformacionGeneralRDUI.TarifaDiaria; }
            set { this.ucucInformacionGeneralRDUI.TarifaDiaria = value; }
        }
        /// <summary>
        /// Obtiene o establece los kilometros libres para la tarifa seleccionada
        /// </summary>
        public int? KmsLibres
        {
            get { return this.ucucInformacionGeneralRDUI.KmsLibres; }
            set { this.ucucInformacionGeneralRDUI.KmsLibres = value; }
        }
        /// <summary>
        /// Obtiene o establece el importe por kilometro adicional para la tarifa seleccionada
        /// </summary>
        public decimal? TarifaKmAdicional
        {
            get { return this.ucucInformacionGeneralRDUI.TarifaKmAdicional; }
            set { this.ucucInformacionGeneralRDUI.TarifaKmAdicional = value; }
        }
        /// <summary>
        /// Obtiene o establece las horas libres para la tarifa seleccionada
        /// </summary>
        public int? HrsLibres
        {
            get { return this.ucucInformacionGeneralRDUI.HrsLibres; }
            set { this.ucucInformacionGeneralRDUI.HrsLibres = value; }
        }
        /// <summary>
        /// Obtiene o establece el importe por hora adicional en la tarifa seleccionada
        /// </summary>
        public decimal? TarifaHrAdicional
        {
            get { return this.ucucInformacionGeneralRDUI.TarifaHrAdicional; }
            set { this.ucucInformacionGeneralRDUI.TarifaHrAdicional = value; }
        }
        /// <summary>
        /// obtiene o establece la descripcion de la tarifa seleccionada
        /// </summary>
        public string TarifaDescripcion {
            get { return this.ucucInformacionGeneralRDUI.TarifaDescripcion; }
            set { this.ucucInformacionGeneralRDUI.TarifaDescripcion = value; }
        }
        /// <summary>
        /// Obtiene  o establece el RFC del cliente
        /// </summary>
        public string ClienteRFC {
            get {  return ucucInformacionGeneralRDUI.ClienteRFC;   }
            set { this.ucucInformacionGeneralRDUI.ClienteRFC = value;  }
        }
        /// <summary>
        /// Número de Cuenta de Oracle CuentaClienteBO.Numero
        /// </summary>
        public String CuentaClienteNumeroCuenta {
            get { return this.ucucInformacionGeneralRDUI.CuentaClienteNumeroCuenta; }
            set { this.ucucInformacionGeneralRDUI.CuentaClienteNumeroCuenta = value; }
        }

       
        #endregion

        #region Constructor
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                presentador = new RegistrarContratoRDPRE(this, this.ucucInformacionGeneralRDUI, this.ucCatalogoDocumentos);
                if (!Page.IsPostBack)
                    presentador.PrepararNuevo();
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencia al crear la página", ETipoMensajeIU.ERROR, nombreClase + ".Page_Load:" + ex.Message);
            }
        }
        #endregion

        #region Métodos
        /// <summary>
        /// Prepara la interfaz para el registro de un nuevo contrato
        /// </summary>
        public void PrepararNuevo()
        {
            this.ucucInformacionGeneralRDUI.PrepararNuevo();
            this.ucCatalogoDocumentos.LimpiaCampos();
        }
        /// <summary>
        /// Habilita o deshabilita el botón de guardar borrador
        /// </summary>
        /// <param name="permitir">Parametro que controla el estatus del control</param>
        public void PermitirGuardarBorrador(bool permitir)
        {
            this.btnGuardar.Enabled = permitir;
        }
        /// <summary>
        /// Habilit ao deshabilita el boton de guardar contrato terminado
        /// </summary>
        /// <param name="permitir">Parametro que controla el estatus del control</param>
        public void PermitirGuardarTerminado(bool permitir)
        {
            this.btnTermino.Enabled = permitir;
        }
        /// <summary>
        /// Envia al usuario a la página de advertencia por falta de permisos
        /// </summary>
        public void RedirigirSinPermisoAcceso()
        {
            this.Response.Redirect(this.ResolveUrl("~/MapaSitio.UI/PaginaSinAccesoUI.aspx"));
        }
        /// <summary>
        /// Envia al usuario a la página de consulta de contratos
        /// </summary>
        public void RedirigirAConsulta()
        {
            Response.Redirect("ConsultarContratoRDUI.aspx", false);
        }
        /// <summary>
        /// Envia al usuario a la página de detalle despues del registro del contrato
        /// </summary>
        public void RedirigirADetalles()
        {
            Response.Redirect("DetalleContratoRDUI.aspx", false);
        }
        /// <summary>
        /// Establece el Paquete de Navegacion para el Detalle del Contrato Seleccionado
        /// </summary>
        /// <param name="Clave">Clave del Paquete</param>
        /// <param name="value">Valor del paquete</param>
        public void EstablecerPaqueteNavegacion(string key, object value)
        {
            if (string.IsNullOrEmpty(key) || string.IsNullOrWhiteSpace(key))
                throw new Exception(nombreClase + ".EstablecerPaqueteNavegacion: El paquete de navegación no puede tener una llave nula o vacía.");
            if (value == null)
                throw new Exception(nombreClase + ".EstablecerPaqueteNavegacion: El paquete de navegación no puede ser nulo.");

            Session[key] = value;
        }
        /// <summary>
        /// Limpia las variables usadas para el registro de la session
        /// </summary>
        public void LimpiarSesion()
        {
            this.ucCatalogoDocumentos.LimpiarSesion();
            this.ucucInformacionGeneralRDUI.LimpiarSesion();
        }
        /// <summary>
        /// Este método despliega un mensaje en pantalla
        /// </summary>
        /// <param name="mensaje">Mensaje a desplegar</param>
        /// <param name="tipo">Tipo de mensaje a desplegar</param>
        /// <param name="detalle">Muestra una cadena de texto que contiene los datalles del mensaje a desplegar</param>
        public void MostrarMensaje(string mensaje, Primitivos.Enumeradores.ETipoMensajeIU tipo, string msjDetalle = null)
        {
            var master = (Site)Master;
            if (tipo == ETipoMensajeIU.ERROR)
            {
                if (master != null) master.MostrarMensaje(mensaje, tipo, msjDetalle);
            }
            else
            {
                if (master != null) master.MostrarMensaje(mensaje, tipo);
            }
        }
        /// <summary>
        /// Actualiza el listado de docuemntos en caso de ocurrir un error
        /// </summary>
        private void ActualizarDocumentos()
        {
            this.ucCatalogoDocumentos.InicializarControl(new List<ArchivoBO>(), new List<TipoArchivoBO>());
            this.ucCatalogoDocumentos.LimpiarSesion();
            this.ucCatalogoDocumentos.LimpiaCampos();
        }

        /// <summary>
        /// Registra Script en el cliente
        /// </summary>
        /// <param name="key">Key del script</param>
        /// <param name="script">script a registrar</param>
        private void RegistrarScript(string key, string script)
        {
            ScriptManager.RegisterStartupScript(Page, typeof(Page), key, script, true);
        }
        #endregion

        #region Eventos
        protected void btnActualizarPrevio_Click(object sender, EventArgs e)
        {
            try
            {
                string origen = ((Button)sender).ID;

                if (this.presentador.UnidadTieneReservacion())
                    this.RegistrarScript("UnidadTieneReservacion", "confirmarRentaUnidadReservada('" + origen + "');");
                else
                {
                    switch (origen)
                    {
                        case "btnTerminoPrevio":
                            this.presentador.RegistrarTerminada();
                            break;
                        case "btnGuardarPrevio":
                            this.presentador.RegistrarBorrador();
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                this.ActualizarDocumentos();
                this.MostrarMensaje("Inconsistencia al guardar el contrato.", ETipoMensajeIU.ERROR, nombreClase + ".btnActualizarPrevio_Click:" + ex.Message);
            }
        }

        protected void btnTermino_Click(object sender, EventArgs e)
        {
            try
            {
                presentador.RegistrarTerminada();
            }
            catch (Exception ex)
            {
                this.ActualizarDocumentos();
                MostrarMensaje("Inconsistencia al guardar el contrato.", ETipoMensajeIU.ERROR, nombreClase + ".btnTermino_Click:" + ex.Message);
            }
        }
        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                presentador.RegistrarBorrador();
            }
            catch (Exception ex)
            {
                this.ActualizarDocumentos();
                MostrarMensaje("Inconsistencia al guardar el contrato.", ETipoMensajeIU.ERROR, nombreClase + ".btnGuardar_Click:" + ex.Message);
            }

        }
        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            try
            {
                presentador.CancelarRegistro();
            }
            catch (Exception ex)
            {
                MostrarMensaje("Inconsistencias al cancelar el registro del contrato.", ETipoMensajeIU.ERROR, nombreClase + ".btnCancelar_Click: " + ex.Message);
            }
        }
        #endregion
    }
}