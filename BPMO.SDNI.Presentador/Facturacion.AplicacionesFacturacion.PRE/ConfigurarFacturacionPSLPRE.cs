using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using BPMO.Basicos.BO;
using BPMO.eFacturacion.Procesos.BO;
using BPMO.eFacturacion.Procesos.Enumeradores;
using BPMO.Facade.SDNI.BR;
using BPMO.Facade.SDNI.eFacturacion.BR;
using BPMO.Patterns.Creational.DataContext;
using BPMO.Patterns.Structural.Composite;
using BPMO.Primitivos.Enumeradores;
using BPMO.SDNI.Comun.BO;
using BPMO.SDNI.Comun.BR;
using BPMO.SDNI.Contratos.BO;
using BPMO.SDNI.Contratos.PSL.BO;
using BPMO.SDNI.Contratos.PSL.BR;
using BPMO.SDNI.Equipos.BO;
using BPMO.SDNI.Equipos.BR;
using BPMO.SDNI.Facturacion.AplicacionesFacturacion.BR;
using BPMO.SDNI.Facturacion.AplicacionesFacturacion.VIS;
using BPMO.SDNI.Facturacion.GeneradorPaquetesFacturacion.BO;
using BPMO.SDNI.Facturacion.GeneradorPaquetesFacturacion.BR;
using BPMO.SDNI.Facturacion.InfraestructuraPagos.BO;
using BPMO.SDNI.Facturacion.InfraestructuraPagos.BOF;
using BPMO.SDNI.Facturacion.InfraestructuraPagos.BR;

namespace BPMO.SDNI.Facturacion.AplicacionesFacturacion.PRE {
    /// <summary>
    /// Presentador para la vista de configuración de una prefactura de contrato
    /// </summary>
    public class ConfigurarFacturacionPSLPRE {
        #region Constantes
        /// <summary>
        /// Nombre de la clase para agregar a los mensajes de Error
        /// </summary>
        private static readonly string nombreClase = typeof(ConfigurarFacturacionPSLPRE).Name;
        #endregion

        #region Atributos
        /// <summary>
        /// Objeto auxiliar para la sincronización de facturaciones
        /// </summary>
        private static readonly object syncFacturacion = new object();

        /// <summary>
        /// Moneda a facturar para cada línea factura
        /// </summary>
        public string MonedaPesos {
            get {
                try {
                    return ConfigurationManager.AppSettings["MonedaPesos"];
                } catch (Exception) {
                    return null;
                }
            }
        }

        /// <summary>
        /// Mensaje que se presenta del cobro de Seguro Automático
        /// </summary>
        private string MensajeCobroSeguro {
            get {
                try {
                    return ConfigurationManager.AppSettings["MensajeCobroSeguro"];
                } catch (Exception) {
                    return null;
                }
            }
        }

        /// <summary>
        /// Objeto que almacena el pago contrato que se están procesando durante el tiempo de vida del sitio
        /// </summary>
        private volatile static Dictionary<int, Object> pagoContrato;

        public List<LineaContratoPSLBO> LineasContrato = new List<LineaContratoPSLBO>();

        /// <summary>
        /// El DataContext que proveerá acceso a la base de datos
        /// </summary>
        private IDataContext dctx;

        /// <summary>
        /// La vista que esta mostrando los datos procesados en el presentador
        /// </summary>
        private IConfigurarFacturacionPSLVIS vista;

        private IucCostosAdicionalesFacturaContratoVIS vistaCargos;

        private ucCostosAdicionalesFacturaContratoPRE vistaCargosAdicionales;

        private ucInformacionGeneralPSLPRE vistaFacturaContrato;

        /// <summary>
        /// Controlador para los pagos de unidad
        /// </summary>
        private PagoContratoPSLBR pagoContratoPSLBR;

        /// <summary>
        /// Controlador para las Unidades
        /// </summary>
        private UnidadBR unidadBR;

        /// <summary>
        /// Controlador para la bitácora de erros
        /// </summary>
        private LogFacturacionBR logFacturacionBR;

        /// <summary>
        /// Controlador para de las cuentas de usuario de idealese
        /// </summary>
        private CuentaClienteIdealeaseBR cuentaClienteIdealeaseBR;

        /// <summary>
        /// Controlador para los departamentos relacionados con el proceso de facturación
        /// </summary>
        private DepartamentoFacturacionBR departamentoFacturacionBR;

        private ContratoPSLBR contratoPSLBR;
        #endregion

        #region Constructores
        /// <summary>
        /// Constructor por default del presentador
        /// </summary>
        /// <param name="vista">Vista que será gestionada por el presentador</param>
        public ConfigurarFacturacionPSLPRE(IConfigurarFacturacionPSLVIS vista, IucInformacionGeneralPSLVIS vistaFactura, IucCostosAdicionalesFacturaContratoVIS vistaCargos) {
            try {
                this.vista = vista;
                this.dctx = FacadeBR.ObtenerConexion();
                this.logFacturacionBR = new LogFacturacionBR();
                this.unidadBR = new UnidadBR();
                this.cuentaClienteIdealeaseBR = new CuentaClienteIdealeaseBR();
                this.departamentoFacturacionBR = new DepartamentoFacturacionBR();
                this.pagoContratoPSLBR = new PagoContratoPSLBR();
                this.contratoPSLBR = new ContratoPSLBR();

                this.vistaCargosAdicionales = new ucCostosAdicionalesFacturaContratoPRE(vistaCargos);
                this.vistaFacturaContrato = new ucInformacionGeneralPSLPRE(vistaFactura);
            } catch (Exception ex) {
                this.vista.MostrarMensaje("Inconsistencias en la configuración", ETipoMensajeIU.ERROR, nombreClase + ".ConfigurarFacturacionPSLPRE: " + ex.GetBaseException().Message);
            }
        }

        #endregion

        #region Métodos
        /// <summary>
        /// Crea un objeto de seguridad con los datos actuales de la sesión de usuario en curso
        /// </summary>
        /// <returns>Objeto de tipo seguridad</returns>
        private SeguridadBO CrearObjetoSeguridad() {
            UsuarioBO usuario = new UsuarioBO() { Id = this.vista.UsuarioID };
            AdscripcionBO adscripcion = new AdscripcionBO() { UnidadOperativa = new UnidadOperativaBO() { Id = this.vista.UnidadOperativaID } };
            SeguridadBO seguridadBO = new SeguridadBO(Guid.Empty, usuario, adscripcion);

            return seguridadBO;
        }

        /// <summary>
        /// Crea un objeto de auditoria con los datos actuales de la sesión de usuario en curso
        /// </summary>
        /// <returns>Objeto de tipo auditoria</returns>
        private AuditoriaBO CrearObjetoAuditoria() {
            AuditoriaBO auditoria = new AuditoriaBO
            {
                FC = DateTime.Now,
                FUA = DateTime.Now,
                UC = this.vista.UsuarioID,
                UUA = this.vista.UsuarioID
            };

            return auditoria;
        }

        /// <summary>
        /// Valida el acceso a la sección en curso
        /// </summary>
        public void ValidarAcceso() {
            try {
                //Valida que usuario y la unidad operativa no sean nulos
                if (this.vista.UsuarioID == null) throw new Exception("El identificador del usuario no debe ser nulo");
                if (this.vista.UnidadOperativaID == null) throw new Exception("El identificador de la Unidad Operativa no debe ser nulo");
                //Se crea el objeto de seguridad                
                SeguridadBO seguridadBO = this.CrearObjetoSeguridad();

                //Se valida si el usuario tiene permiso a la acción principal
                if (!FacadeBR.ExisteAccion(this.dctx, "CONSULTAR", seguridadBO) ||
                    !FacadeBR.ExisteAccion(this.dctx, "REGISTRARTRANSACCIONCOMPLETO", seguridadBO) ||
                    !FacadeBR.ExisteAccion(this.dctx, "ACTUALIZARCOMPLETO", seguridadBO))
                    this.vista.RedirigirSinPermisoAcceso();
            } catch (Exception ex) {
                throw new Exception(nombreClase + ".ValidarAcceso: " + ex.GetBaseException().Message);
            }
        }

        /// <summary>
        /// Proceso que se ejecuta cuando se crea una nueva factura
        /// </summary>
        public void PrepararNuevo() {
            this.vista.PrepararNuevo();
            this.vista.PermitirRegresar(false);

            this.vista.PermitirContinuar(true);
            this.IrAPagina(1);
            this.EstablecerInformacionInicial();
        }

        /// <summary>
        /// Establece la página a ser visualizada
        /// </summary>
        /// <param name="numeroPagina">Número de pagina a ser visualizada, comenzando por el número 1</param>
        private void EstablecerPagina(int numeroPagina) {
            switch (numeroPagina) {
                case 1:
                    this.vista.OcultarAnterior(true);
                    this.vista.OcultarCancelar(true);

                    break;

                case 2:
                    if (this.vista.PagoActual == null) {
                        PagoContratoPSLBO pagoCntPSL = this.ConsultarPagos(this.vista.PagoContratoID, null, true).FirstOrDefault();
                        this.AcompletarPago(pagoCntPSL, this.vista.Contrato);
                        this.vista.PagoActual = pagoCntPSL;
                    }

                    PagoContratoPSLBO pagoUnidadContrato = this.vista.PagoActual;

                    pagoUnidadContrato.Divisa.MonedaDestino = this.vista.InformacionCabeceraView.CodigoMonedaDestino != "-1"
                                                                    ? new MonedaBO { Codigo = this.vista.InformacionCabeceraView.CodigoMonedaDestino }
                                                                    : pagoUnidadContrato.Divisa.MonedaOrigen;
                    this.vista.PagoActual = pagoUnidadContrato;

                    this.RegenerarTransaccion();

                    this.vista.OcultarCancelar(false);
                    this.vista.OcultarAnterior(false);

                    break;
            }

            this.vista.EstablecerPagina(numeroPagina);
        }

        /// <summary>
        /// Redirige o visualiza una página especifica
        /// </summary>
        /// <param name="numeroPagina">Número de página a visualizar</param>
        private void IrAPagina(int numeroPagina) {
            if (numeroPagina < 1 || numeroPagina > 2)
                throw new Exception("La paginación va de 1 al 2.");

            this.EstablecerOpcionesSegunPagina(numeroPagina);
            this.EstablecerPagina(numeroPagina);
        }

        /// <summary>
        /// Recupera la cuenta de cliente asociada a un pago de contrato
        /// </summary>
        /// <param name="pagoContrato">Objeto de pago a evaluar</param>
        /// <returns>Objeto de tipo CuentaClienteBO recuperado</returns>
        private CuentaClienteBO ResolverCuentaCliente(PagoContratoPSLBO pagoContrato) {
            StringBuilder messagesAdvertencia = new StringBuilder();
            List<CuentaClienteBO> cuentaClientes = FacadeBR.ConsultarCuentaClienteCompleto(
                this.dctx,
                new CuentaClienteBO
                {
                    Id = pagoContrato.ReferenciaContrato.CuentaCliente.Id,
                    UnidadOperativa = new UnidadOperativaBO { Id = pagoContrato.ReferenciaContrato.UnidadOperativa.Id }
                }
            );
            if (cuentaClientes.Count == 0)
                throw new Exception(String.Format("La cuenta cliente {0} que esta asociada al pago, no fue encontrado", pagoContrato.ReferenciaContrato.CuentaCliente.Id));

            CuentaClienteBO cuentaCliente = cuentaClientes.First();

            var mismaDireccion = cuentaCliente.Direcciones.Any(x => x.Id == pagoContrato.ReferenciaContrato.DireccionCliente.Id && x.Direccion == pagoContrato.ReferenciaContrato.DireccionCliente.Direccion.ToUpper());

            if (!mismaDireccion) {
                messagesAdvertencia.AppendLine(string.Format("La dirección del cliente {0} asociada al contrato y la de ORACLE son distintas. La dirección que se utilizará es la de ORACLE.", pagoContrato.ReferenciaContrato.DireccionCliente.Direccion.ToUpper()));
                if (messagesAdvertencia.Length > 0)
                    this.ShowInitError(false, messagesAdvertencia.ToString());
            }

            DireccionClienteBO direccion = cuentaCliente.Direcciones.First(x => x.Id == pagoContrato.ReferenciaContrato.DireccionCliente.Id);

            do {
                cuentaCliente.EliminarElementoDireccion(0);
            } while (cuentaCliente.Direcciones.Any());
            cuentaCliente.Agregar(direccion);

            return cuentaCliente;
        }



        /// <summary>
        /// Recupera el crédito de cliente asociada a un pago de contrato
        /// </summary>
        /// <param name="pagoContrato">Objeto de pago a evaluar</param>       
        /// <returns>Objeto de tipo CreditoClienteBO recuperado</returns>
        private CreditoClienteBO ResolverCreditoCliente(PagoContratoPSLBO pagoContrato) {
            CreditoClienteBO result = null;

            CreditoClienteBO filter = new CreditoClienteBO
            {
                CuentaCliente = new CuentaClienteBO
                {
                    Id = pagoContrato.ReferenciaContrato.CuentaCliente.Id
                },
                Moneda = new MonedaBO
                {
                    Codigo = pagoContrato.Divisa.MonedaDestino.Codigo
                }
            };

            switch (pagoContrato.ReferenciaContrato.TipoContratoID.Value) {
                case ETipoContrato.RO:
                case ETipoContrato.RE:
                    filter.TipoCredito = ETipoCredito.REVOLVENTE;
                    filter.Referencia = pagoContrato.ReferenciaContrato.FolioContrato;
                    break;

                case ETipoContrato.ROC:
                    filter.TipoCredito = ETipoCredito.ACUERDO_DE_VENTAS;
                    filter.Referencia = pagoContrato.ReferenciaContrato.FolioContrato;
                    break;
            }

            var creditosClientes = FacadeEFacturacionBR.ConsultarCreditoCliente(this.dctx, filter);
            if (creditosClientes.Count == 0)
                throw new Exception(String.Format("No se ha podido recuperar el crédito del cliente {0} con la moneda {1}, {2}", pagoContrato.ReferenciaContrato.CuentaCliente.Id, pagoContrato.Divisa.MonedaDestino.Codigo, filter.TipoCredito.ToString()));

            result = creditosClientes[0];

            return result;
        }

        /// <summary>
        /// Obtiene el tipo de divisa por default
        /// </summary>
        /// <returns>Objeto divisa configurada por default</returns>
        private TipoDivisaBO ResolverTipoDivisaPorDefault() {
            return new TipoDivisaBO { NombreCorto = this.vista.TipoTasaCambiarioPorDefault, Nombre = this.vista.TipoTasaCambiarioPorDefault };
        }

        /// <summary>
        /// Recupera la divisa por default
        /// </summary>
        /// <param name="monedaOrigen">Clave de la moneda origen a evaluar</param>
        /// <param name="monedaDestino">Clave de la moneda destino a evaluar</param>
        /// <returns>Objeto Divisa por default</returns>
        private DivisaBO ResolverDivisaPorDefault(String monedaOrigen, String monedaDestino) {
            DivisaBO divisa = new DivisaBO
            {
                MonedaOrigen = new MonedaBO { Codigo = monedaOrigen },
                MonedaDestino = new MonedaBO { Codigo = monedaDestino },
                TipoCambio = 1M,
                TipoDivisa = this.ResolverTipoDivisaPorDefault()
            };

            return divisa;
        }

        /// <summary>
        /// Recupera la divisa correspondiente a un pago de contrato
        /// </summary>
        /// <param name="pagoUnidadContrato">Objeto de pago a evaluar</param>
        /// <returns>Divisa encontrada, de lo contrario devolverá nulo</returns>
        private DivisaBO ResolverDivisa(PagoContratoPSLBO pagoContrato) {
            return this.ResolverDivisa(pagoContrato.Divisa.MonedaOrigen.Codigo, pagoContrato.Divisa.MonedaDestino.Codigo);
        }

        /// <summary>
        /// Recupera la divisa correspondiente a un pago de contrato
        /// </summary>
        /// <param name="monedaOrigen">Clave de la moneda origen a evaluar</param>
        /// <param name="monedaDestino">Clave de la moneda destino a evaluar</param>
        /// <returns>Divisa encontrada, de lo contrario devolverá nulo</returns>
        private DivisaBO ResolverDivisa(String monedaOrigen, String monedaDestino) {
            DivisaBO divisa = null;
            List<DivisaBO> divisas = FacadeBR.ConsultarTipoCambio(this.dctx,
                new DivisaBO { MonedaOrigen = new MonedaBO { Codigo = monedaOrigen }, MonedaDestino = new MonedaBO { Codigo = monedaDestino }, FechaTipoCambio = DateTime.Today }
            );

            if (divisas.Count > 0) {
                divisa = divisas.OrderByDescending(x => x.FechaTipoCambio).FirstOrDefault(x => DateTime.Now >= x.FechaTipoCambio && DateTime.Now <= x.FechaTipoCambio.Value.AddDays(1));

                if (divisa.TipoDivisa == null || !String.Equals(divisa.TipoDivisa.NombreCorto, this.vista.TipoTasaCambiarioPorDefault, StringComparison.InvariantCultureIgnoreCase))
                    divisa.TipoDivisa = this.ResolverTipoDivisaPorDefault();
            } else {
                divisas = FacadeBR.ConsultarTipoCambio(this.dctx, new DivisaBO { MonedaOrigen = new MonedaBO { Codigo = monedaOrigen }, MonedaDestino = new MonedaBO { Codigo = monedaDestino } });
                if (divisas.Count > 0) {
                    divisa = divisas.OrderByDescending(x => x.FechaTipoCambio).FirstOrDefault(x => DateTime.Now >= x.FechaTipoCambio && DateTime.Now <= x.FechaTipoCambio.Value.AddDays(1));

                    if (divisa == null) {
                        divisa = divisas.OrderByDescending(x => x.FechaTipoCambio).FirstOrDefault();
                        vista.MostrarMensaje("No existe un tipo de Cambio Configurado de " + monedaOrigen + " a " + monedaDestino + " para el día de Hoy, se usará el Tipo de cambio del día " + divisa.FechaTipoCambio.Value.ToShortDateString(), ETipoMensajeIU.ADVERTENCIA);
                    }

                    if (divisa.TipoDivisa == null || !String.Equals(divisa.TipoDivisa.NombreCorto, this.vista.TipoTasaCambiarioPorDefault, StringComparison.InvariantCultureIgnoreCase))
                        divisa.TipoDivisa = this.ResolverTipoDivisaPorDefault();
                } else
                    divisa = this.ResolverDivisaPorDefault(monedaOrigen, monedaDestino);
            }
            return divisa;
        }

        /// <summary>
        /// Proceso de lanzar una excepción inicial
        /// </summary>
        /// <param name="exception">Determina si el mensaje se mostrará como una excepción</param>
        /// <param name="message">Mensaje a enviar</param>
        /// <param name="parameters">Parámetros asociados al mensaje</param>
        private void ShowInitError(bool exception, String message, params Object[] parameters) {
            if (parameters != null && parameters.Length > 0)
                message = String.Format(message, parameters);

            if (exception)
                throw new Exception(message);
            else
                this.vista.MostrarMensaje(message, ETipoMensajeIU.ADVERTENCIA);
        }

        /// <summary>
        /// Ejecuta el proceso de ir a la página siguiente
        /// </summary>
        public void AvanzarPagina() {
            int? paginaActual = this.vista.PaginaActual;
            if (paginaActual == null)
                throw new Exception("La página actual es nula.");
            if (paginaActual >= 2)
                throw new Exception("La página actual es mayor o igual a 2 y, por lo tanto, no se puede avanzar.");

            this.EstablecerOpcionesSegunPagina(paginaActual.Value + 1);
            this.EstablecerPagina(paginaActual.Value + 1);
        }



        /// <summary>
        /// Acompleta la información de un pago
        /// </summary>
        /// <param name="pagoContrato">Pago que requiere acompletar</param>
        private StringBuilder AcompletarPago(PagoContratoPSLBO pagoContrato, ContratoPSLBO contrato) {
            StringBuilder messagesAdvertencia = new StringBuilder();
            if (!pagoContrato.Activo.GetValueOrDefault())
                this.ShowInitError(true, "El pago del contrato {0} no se encuentra activo", pagoContrato.ReferenciaContrato.FolioContrato);

            if (pagoContrato.ReferenciaContrato.Sucursal == null)
                this.ShowInitError(true, "No se cuenta puede obtener información para consultar la unidad operativa");

            // Sucursal
            pagoContrato.ReferenciaContrato.Sucursal = contrato.Sucursal;

            // Cuenta Cliente
            pagoContrato.ReferenciaContrato.CuentaCliente = contrato.Cliente;
            pagoContrato.ReferenciaContrato.DireccionCliente = contrato.Cliente.Direcciones.FirstOrDefault();

            // Unidades
            foreach (PagoUnidadContratoPSLBO pagoUnidadContrato in pagoContrato.ObtenerPagosUnidadesContratos()) {
                LineaContratoPSLBO _linea = contrato.LineasContrato.ConvertAll(l => (LineaContratoPSLBO)l)
                    .FirstOrDefault(l => ((l as LineaContratoPSLBO).Equipo as UnidadBO).UnidadID == pagoUnidadContrato.Unidad.UnidadID);

                if (_linea != null && _linea.Equipo != null)
                    pagoUnidadContrato.Unidad = (UnidadBO)_linea.Equipo;
            }

            return messagesAdvertencia;
        }

        /// <summary>
        /// Agrega todos los campos actuales de la vista a un objeto de transacción
        /// </summary>
        /// <param name="pagoContrato">Objeto de pago de unidad tomada como pivote</param>
        /// <param name="creditoCliente">Crédito del cliente asociada a la transacción</param>
        /// <param name="transaccion">Transacción a a completar</param>
        private void AcompletarTransaccion(PagoContratoPSLBO pagoContrato, CreditoClienteBO creditoCliente, TransaccionBO transaccion) {
            SeguridadBO seguridad = this.CrearObjetoSeguridad();
            AuditoriaBO auditoria = this.CrearObjetoAuditoria();

            List<DetalleTransaccionBO> costosAdicionales = new List<DetalleTransaccionBO>();
            this.ReajustarDetallesTransaccion(transaccion, costosAdicionales, pagoContrato, creditoCliente);

            if (transaccion.DatosAdicionales == null)
                transaccion.DatosAdicionales = new List<DatosAdicionalesFacturaBO>();

            transaccion.TransaccionId = pagoContrato.PagoContratoID;
            transaccion.Fecha = DateTime.Now;
            transaccion.Observaciones = this.vista.InformacionCabeceraView.Observaciones != null ? this.vista.InformacionCabeceraView.Observaciones.ToUpper() : null;
            transaccion.Auditoria = auditoria;

            DepartamentoBO departamento = this.departamentoFacturacionBR.Consultar(pagoContrato.ReferenciaContrato.TipoContratoID.Value, this.vista.ObtenerCarpetaRaiz(), pagoContrato.ReferenciaContrato.UnidadOperativa.Id);
            transaccion.Adscripcion = new AdscripcionBO
            {
                UnidadOperativa = new UnidadOperativaBO
                {
                    Id = this.vista.UnidadOperativaID
                },
                Sucursal = new SucursalBO
                {
                    Id = pagoContrato.ReferenciaContrato.Sucursal.Id
                },
                Departamento = departamento
            };

            transaccion.SistemaOrigen = this.vista.SistemaOrigen;
            transaccion.RFC = pagoContrato.ReferenciaContrato.CuentaCliente.Cliente.RFC;
            transaccion.RazonSocial = pagoContrato.ReferenciaContrato.CuentaCliente.Cliente.NombreCompleto;
            transaccion.Auditoria = auditoria;
            transaccion.Credito = this.vista.InformacionCabeceraView.FormaPago == this.vista.FormaPagoPorDefault;

            ETipoTransaccion tipoTransaccion = ETipoTransaccion.CONTRATO_RENTA_DIARIA;
            switch (pagoContrato.ReferenciaContrato.TipoContratoID.GetValueOrDefault()) {
                case ETipoContrato.RO:
                    tipoTransaccion = (ETipoEmpresa)(this.vista.UnidadOperativaID) == ETipoEmpresa.Generacion ? ETipoTransaccion.CONTRATO_RENTA_ORDINARIA_GENERACION : 
                        (ETipoEmpresa)(this.vista.UnidadOperativaID) == ETipoEmpresa.Construccion ? ETipoTransaccion.CONTRATO_RENTA_ORDINARIA_CONSTRUCCION :
                        ETipoTransaccion.CONTRATO_RENTA_ORDINARIA_EQUINOVA;
                    break;

                case ETipoContrato.ROC:
                    tipoTransaccion = (ETipoEmpresa)(this.vista.UnidadOperativaID) == ETipoEmpresa.Generacion ? ETipoTransaccion.CONTRATO_RENTA_OPCION_COMPRA_GENERACION : 
                        (ETipoEmpresa)(this.vista.UnidadOperativaID) == ETipoEmpresa.Construccion ? ETipoTransaccion.CONTRATO_RENTA_OPCION_COMPRA_CONSTRUCCION :
                        ETipoTransaccion.CONTRATO_RENTA_OPCION_COMPRA_EQUINOVA;
                    break;

                case ETipoContrato.RE:
                    tipoTransaccion = (ETipoEmpresa)(this.vista.UnidadOperativaID) == ETipoEmpresa.Generacion ? ETipoTransaccion.CONTRATO_RENTA_EXTRAORDINARIA_GENERACION : 
                        (ETipoEmpresa)(this.vista.UnidadOperativaID) == ETipoEmpresa.Construccion ? ETipoTransaccion.CONTRATO_RENTA_EXTRAORDINARIA_CONSTRUCCION :
                        ETipoTransaccion.CONTRATO_RENTA_EXTRAORDINARIA_EQUINOVA;
                    break;
            }

            transaccion.TasaIVA = 0;
            if (pagoContrato.ReferenciaContrato != null && pagoContrato.ReferenciaContrato.Sucursal != null &&
                pagoContrato.ReferenciaContrato.Sucursal.Impuesto != null && pagoContrato.ReferenciaContrato.Sucursal.Impuesto.PorcentajeImpuesto.HasValue)
                transaccion.TasaIVA = pagoContrato.ReferenciaContrato.Sucursal.Impuesto.PorcentajeImpuesto;
            transaccion.Referencia = this.vista.InformacionCabeceraView.NumeroReferencia;
            transaccion.TipoTransaccion = tipoTransaccion;

            DivisaBO divisa = this.ResolverDivisa(pagoContrato);

            transaccion.Divisa = divisa;
            transaccion.Divisa.MonedaOrigen = transaccion.Divisa.MonedaDestino;
            transaccion.Divisa.MonedaDestino = null;

            // Se agrega el Tipo de cambio en base al código de moneda de la empresa
            DivisaBO divisaEmpresa = this.ResolverDivisa(transaccion.Divisa.MonedaOrigen.Codigo, this.vista.CodigoMonedaEmpresa);
            transaccion.Divisa.TipoCambio = divisaEmpresa.TipoCambio;

            transaccion.DireccionCliente = pagoContrato.ReferenciaContrato.DireccionCliente.Direccion;
            transaccion.CuentaCliente = this.ResolverCuentaCliente(pagoContrato);
            transaccion.CuentaCliente.UnidadOperativa = new UnidadOperativaBO { Id = pagoContrato.ReferenciaContrato.UnidadOperativa.Id };

            if (creditoCliente != null) {
                transaccion.DiasCredito = creditoCliente.DiasCredito;
                transaccion.DiasFactura = creditoCliente.DiasFactura;
            }
        }

        /// <summary>
        /// Genera un objeto de transacción con todos los datos a facturar
        /// </summary>
        /// <param name="pagoUnidadContrato">Objeto de pago de unidad tomada como pivote</param>       
        /// <returns>Objeto de tipo transaccion</returns>
        private TransaccionBO ArmarTransaccion(PagoContratoPSLBO pagoContrato, ContratoPSLBO contrato)  //agregar contrato
        {
            SeguridadBO seguridad = this.CrearObjetoSeguridad();
            AuditoriaBO auditoria = this.CrearObjetoAuditoria();

            IGeneradorPaquetesFacturacionPSLBR generador = new GeneradorPaqueteFacturacionPSLBR();
            generador.MonedaFacturar = pagoContrato.Divisa.MonedaDestino != null && !String.IsNullOrEmpty(pagoContrato.Divisa.MonedaDestino.Codigo) ? pagoContrato.Divisa.MonedaDestino.Codigo : generador.MonedaPesos;
            TransaccionBO transaccion = generador.Procesar(this.dctx, pagoContrato, contrato, seguridad);  //el resultado Setear a la sesión

            return transaccion;
        }

        /// <summary>
        /// Genera un clone de un objeto de transaccion
        /// </summary>
        /// <param name="transaccion">Objeto de transaccion original</param>
        /// <returns>Objeto de transacción clonado</returns>
        private TransaccionBO ClonarTransaccion(TransaccionBO transaccion) {
            TransaccionBO clone = transaccion.Clonar();

            IEnumerable<DetalleTransaccionBO> detallesTransaccion = transaccion.GetChildren().OfType<DetalleTransaccionBO>();
            foreach (DetalleTransaccionBO detalle in detallesTransaccion) {
                DetalleTransaccionBO nDetalle = new DetalleTransaccionBO();

                PropertyInfo[] properties = typeof(DetalleTransaccionBO).GetProperties(BindingFlags.Instance | BindingFlags.Public);
                foreach (PropertyInfo property in properties) {
                    if (!property.CanRead || !property.CanWrite)
                        continue;

                    Object value = property.GetValue(detalle, null);
                    property.SetValue(nDetalle, value, null);
                }

                clone.Add(nDetalle);
            }

            if (transaccion.DatosAdicionales != null) {
                clone.DatosAdicionales = new List<DatosAdicionalesFacturaBO>();
                foreach (DatosAdicionalesFacturaBO adicional in transaccion.DatosAdicionales) {
                    DatosAdicionalesFacturaBO nAdicional = new DatosAdicionalesFacturaBO();

                    PropertyInfo[] properties = typeof(DatosAdicionalesFacturaBO).GetProperties(BindingFlags.Instance | BindingFlags.Public);
                    foreach (PropertyInfo property in properties) {
                        if (!property.CanRead || !property.CanWrite)
                            continue;

                        Object value = property.GetValue(adicional, null);
                        property.SetValue(nAdicional, value, null);
                    }

                    clone.DatosAdicionales.Add(nAdicional);
                }
            }

            return clone;
        }

        /// <summary>
        /// Genera un objeto de bloqueo para los bloqueo de pagos
        /// </summary>
        /// <param name="pagoContrato">pago a bloquear</param>
        /// <returns>Objeto para generar el bloqueo</returns>
        private static Object ResolverObjetoBloqueoPagoContrato(PagoContratoPSLBO pagoContrato) {
            Object syncObject = null;
            if (pagoContrato == null)
                throw new Exception(ConfigurarFacturacionPSLPRE.nombreClase + ".ResolverObjetoBloqueoPagoUnidadContrato: El pago no puede ser nulo");

            int pagoID = pagoContrato.PagoContratoID.GetValueOrDefault();
            lock (ConfigurarFacturacionPSLPRE.syncFacturacion) {
                if (ConfigurarFacturacionPSLPRE.pagoContrato == null)
                    ConfigurarFacturacionPSLPRE.pagoContrato = new Dictionary<int, Object>();

                if (!ConfigurarFacturacionPSLPRE.pagoContrato.TryGetValue(pagoID, out syncObject)) {
                    syncObject = pagoID.GetHashCode();
                    ConfigurarFacturacionPSLPRE.pagoContrato.Add(pagoID, syncObject);
                }
            }

            return syncObject;
        }

        /// <summary>
        /// Establece los datos iniciales de la prefactura
        /// </summary>
        private void EstablecerInformacionInicial() {
            this.vista.PermitirTerminar(false);
            this.vista.PertimirCapturar(false);
            this.vista.EnvioFactura = "NO";

            if (!ConfiguracionFacturacionBR.IniciarConsulta(this.dctx))   //validar conexión a oracle
                this.ShowInitError(true, "No se ha podido iniciar la consulta.");

            #region Monedas
            //SC0034
            List<MonedaBO> lstMonedas = FacadeBR.ConsultarMoneda(this.dctx, new MonedaBO() { Activo = true });
            this.vista.InformacionCabeceraView.EstablecerOpcionesMoneda(lstMonedas.ToDictionary(p => p.Codigo, p => p.Nombre));
            #endregion

            StringBuilder messagesAdvertencia = new StringBuilder(), messagesAdvertenciaDireccion = new StringBuilder();
            StringBuilder mensajeAviso = new StringBuilder();

            if (!this.vista.PagoContratoID.HasValue)
                this.ShowInitError(true, "No se ha enviado ningún identificador de contrato para procesar");

            SeguridadBO seguridad = this.CrearObjetoSeguridad();
            AuditoriaBO auditoria = this.CrearObjetoAuditoria();

            //SC0026, Cambio de objeto PagoUnidadContratoBOF para búsquedas
            // Se incluye el costo del seguro de las lineas
                        
            this.vista.PagoActual = this.ConsultarPagos(this.vista.PagoContratoID, null, true).FirstOrDefault();
            if (this.vista.PagoActual == null)
                this.ShowInitError(true, "El pago {0} no fue encontrado", this.vista.PagoContratoID);

            #region[Consulta del contrato]
            if (this.vista.PagoActual.ReferenciaContrato != null && this.vista.PagoActual.ReferenciaContrato.ReferenciaContratoID.HasValue) {
                List<ILineaContrato> listaLineas = new List<ILineaContrato>();
                ContratoPSLBR contratoBr = new ContratoPSLBR();
                List<ContratoPSLBO> listaContrato = contratoBr.ConsultarSimple(this.dctx, new ContratoPSLBO { ContratoID = this.vista.PagoActual.ReferenciaContrato.ReferenciaContratoID.Value });
                this.vista.Contrato = listaContrato.FirstOrDefault();
                this.ArmarModeloVista(this.vista.PagoActual, this.vista.Contrato);
                this.vista.InformacionCabeceraView.MostrarUnidadesContrato(this.vista.InformacionLineasFacturaModel);   // eviar modelo y pintar toda la información del cabecero, recibe : contrato y pago, guardar en sesión el modelo
            } else
                this.ShowInitError(true, "No se pudo recuperar información de las unidades del pago {0} ", this.vista.PagoContratoID);
            #endregion

            PagoContratoPSLBO pagoContrato = this.vista.PagoActual;
            #region LineaDescripcion
            Object listaModelo = this.vista.InformacionLineasFacturaModel;
            List<LineasFacturaModel> listaPagosUnidad = listaModelo as List<LineasFacturaModel>;
            foreach (PagoUnidadContratoPSLBO pago in pagoContrato.PagosUnidad)
                pago.LineaDescripcion = listaPagosUnidad.FirstOrDefault(x => x.PagoContratoPSLID == pago.PagoUnidadContratoID).DescripcionLinea;
            #endregion
            Object lockValue = ConfigurarFacturacionPSLPRE.ResolverObjetoBloqueoPagoContrato(pagoContrato);
            try {
                Monitor.Enter(lockValue);
                messagesAdvertenciaDireccion = this.AcompletarPago(pagoContrato, this.vista.Contrato);

                #region SC0028, Para la visualización de la Divisa se usa la moneda origen del contrato y la moneda destino que por default es MXN

                IGeneradorPaquetesFacturacionPSLBR generador = new GeneradorPaqueteFacturacionPSLBR();
                String monedaAFacturar = pagoContrato.Divisa.MonedaDestino != null && !String.IsNullOrEmpty(pagoContrato.Divisa.MonedaDestino.Codigo) ? pagoContrato.Divisa.MonedaDestino.Codigo : generador.MonedaPesos;
                generador.MonedaFacturar = monedaAFacturar;
                pagoContrato.Divisa.MonedaDestino = new MonedaBO { Codigo = pagoContrato.Divisa.MonedaOrigen.Codigo };

                pagoContrato.Divisa = this.ResolverDivisa(pagoContrato);
                #endregion

                DepartamentoBO departamento = this.departamentoFacturacionBR.Consultar(pagoContrato.ReferenciaContrato.TipoContratoID.Value, this.vista.ObtenerCarpetaRaiz(), pagoContrato.ReferenciaContrato.UnidadOperativa.Id);

                bool esCredito = true;
                CreditoClienteBO creditoCliente = null;
                if (esCredito) {
                    try {
                        creditoCliente = this.ResolverCreditoCliente(pagoContrato);
                    } catch (Exception ex) {
                        messagesAdvertenciaDireccion.AppendLine(
                                                string.Format("Se presentó el siguiente errror al consultar el crédito del Cliente: {0} .",
                                                    ex.Message));
                    }
                }

                //Crea metodo de establecer información del cliente
                this.vista.HerramientasPrefacturaView.NumeroContrato = pagoContrato.ReferenciaContrato.FolioContrato;
                this.vista.HerramientasPrefacturaView.NumeroPago = pagoContrato.NumeroPago;

                this.vista.RFCCliente = pagoContrato.ReferenciaContrato.CuentaCliente.Cliente.RFC;
                this.vista.NombreCliente = pagoContrato.ReferenciaContrato.CuentaCliente.Cliente.NombreCompleto;
                this.vista.NumeroCliente = pagoContrato.ReferenciaContrato.CuentaCliente.Numero;
                this.vista.DireccionCliente = pagoContrato.ReferenciaContrato.DireccionCliente.Direccion;
                this.vista.FechaInicio = pagoContrato.ReferenciaContrato.FechaInicio;
                this.vista.UsuarioGenerador = this.vista.UsuarioNombre;
                //this.vista.Observaciones = pagoContrato.Observaciones;

                this.vista.InformacionCabeceraView.SucursalID = pagoContrato.ReferenciaContrato.Sucursal.Id;
                this.vista.InformacionCabeceraView.SucursalNombre = pagoContrato.ReferenciaContrato.Sucursal.Nombre;
                this.vista.InformacionCabeceraView.SistemaOrigen = this.vista.SistemaOrigen;
                this.vista.InformacionCabeceraView.TipoTransaccion = pagoContrato.ReferenciaContrato.TipoContratoID;
                this.vista.InformacionCabeceraView.NumeroReferencia = String.Format("{0}-{1}", pagoContrato.ReferenciaContrato.FolioContrato, pagoContrato.NumeroPago);
                this.vista.InformacionCabeceraView.CodigoMoneda = pagoContrato.Divisa.MonedaDestino.Codigo != null ? pagoContrato.Divisa.MonedaDestino.Codigo : pagoContrato.Divisa.MonedaOrigen.Codigo;
                this.vista.InformacionCabeceraView.FormaPago = FacadeBR.ObtenerDescripcionEnumerador(this.vista.Contrato.FormaPago);
                #region SC0035 Se coloca el tipo de cambio a la moneda de la empresa para fines solamente de visualización
                this.vista.InformacionCabeceraView.TipoCambio = this.ResolverDivisa(pagoContrato.Divisa.MonedaDestino.Codigo, this.vista.CodigoMonedaEmpresa).TipoCambio;
                #endregion
                this.vista.InformacionCabeceraView.TipoTasaCambiario = this.vista.TipoTasaCambiarioPorDefault;
                this.vista.InformacionCabeceraView.Departamento = departamento != null ? departamento.Nombre : "";
                this.vista.InformacionCabeceraView.BanderaCores = false;
                this.vista.InformacionCabeceraView.CodigoMonedaDestino = pagoContrato.Divisa.MonedaOrigen.Codigo;
                
                //this.vista.PagoActual
                if (creditoCliente != null) {
                    this.vista.InformacionCabeceraView.DiasCredito = creditoCliente.DiasCredito;
                    this.vista.InformacionCabeceraView.DiasFactura = creditoCliente.DiasFactura;
                    this.vista.InformacionCabeceraView.LimiteCredito = creditoCliente.LimiteCredito;
                    this.vista.InformacionCabeceraView.CreditoDisponible = FacadeEFacturacionBR.ConsultarCreditoDisponible(this.dctx, creditoCliente);
                }

                if (pagoContrato.EnviadoFacturacion.GetValueOrDefault())
                    messagesAdvertencia.AppendLine(String.Format("El pago del contrato {0} ya se encuentra enviado a facturar", pagoContrato.ReferenciaContrato.FolioContrato));
                else {
                    if (!this.EsPagoValido(pagoContrato))
                        messagesAdvertencia.AppendLine(String.Format("El pago del contrato {0} no puede ser enviado a facturar porque el pago anterior aun no ha sido enviado a facturar", pagoContrato.ReferenciaContrato.FolioContrato));

                    if (pagoContrato.BloqueadoCredito.GetValueOrDefault())
                        messagesAdvertencia.AppendLine(String.Format("El pago del contrato {0} se encuentra bloqueado por falta de crédito", pagoContrato.ReferenciaContrato.ReferenciaContratoID));
                }

                this.vista.PagoActual = pagoContrato;

                if (messagesAdvertencia.Length > 0) {
                    messagesAdvertencia.AppendLine(messagesAdvertenciaDireccion.ToString());
                    ShowInitError(false, messagesAdvertencia.ToString());
                } else if (messagesAdvertenciaDireccion.Length >= 0) {
                    if (messagesAdvertenciaDireccion.Length > 0) ShowInitError(false, messagesAdvertenciaDireccion.ToString());
                    vista.PermitirTerminar(true);
                    vista.PertimirCapturar(true);

                    if (!String.IsNullOrEmpty(mensajeAviso.ToString().Trim()) || !String.IsNullOrWhiteSpace(mensajeAviso.ToString().Trim()))
                        ShowInitError(false, mensajeAviso.ToString());
                }
            } finally {
                ConfiguracionFacturacionBR.TerminarConsulta(this.dctx);
                Monitor.Exit(lockValue);
            }
        }

        /// <summary>
        /// Regenera la transacción en curso refrescando los conceptos de facturación
        /// </summary>
        private void RegenerarTransaccion() {
            PagoContratoPSLBO pagoContrato = this.vista.PagoActual;
            StringBuilder mensajeAviso = new StringBuilder();

            #region RI0017 - Validación del crédito del cliente
            bool esCredito = this.vista.InformacionCabeceraView.FormaPago == this.vista.FormaPagoPorDefault;
            CreditoClienteBO creditoCliente = null;

            if (esCredito)
                try {
                    creditoCliente = this.ResolverCreditoCliente(pagoContrato);
                } catch {
                    creditoCliente = null;
                }
            #endregion

            var transaccionOriginal = this.ArmarTransaccion(this.vista.PagoActual, this.vista.Contrato);  //sunday 
            this.vista.TransaccionActual = transaccionOriginal;

            TransaccionBO transaccion = this.ClonarTransaccion(this.vista.TransaccionActual);
            List<DetalleTransaccionBO> costosAdicionales = new List<DetalleTransaccionBO>();
            List<DetalleTransaccionBO> lineasContrato = this.ReajustarDetallesTransaccion(transaccion, costosAdicionales, pagoContrato, creditoCliente);

            //programar evento cuando se le de siguiente para cargar lineas factura view
            this.vista.LineasFacturaView.MostrarLineasContrato(lineasContrato);
            this.vista.LineasFacturaView.MostrarTotales(transaccion);

            pagoContrato.TotalFactura = this.vista.TransaccionActual.TotalFactura;
            pagoContrato.IvaFacturado = this.vista.TransaccionActual.Impuestos;

            if (!String.IsNullOrEmpty(mensajeAviso.ToString().Trim()) || !String.IsNullOrWhiteSpace(mensajeAviso.ToString().Trim()))
                ShowInitError(false, mensajeAviso.ToString());
        }

        /// <summary>
        /// Valida la moneda que se esta seleccionando
        /// </summary>
        public void ValidarMonedaFacturacion(bool? desdeInterfaz = false) {
            PagoContratoPSLBO pagoContrato = this.vista.PagoActual;

            pagoContrato.Divisa.MonedaDestino = this.vista.InformacionCabeceraView.CodigoMonedaDestino != "-1"
                                                            ? new MonedaBO { Codigo = this.vista.InformacionCabeceraView.CodigoMonedaDestino }
                                                            : this.vista.PagoActual.Divisa.MonedaOrigen;

            pagoContrato.Divisa = this.ResolverDivisa(this.vista.PagoActual);

            this.vista.PagoActual = pagoContrato;

           
            #region SC0035 Se coloca el tipo de cambio a la moneda de la empresa para fines solamente de visualización
            this.vista.InformacionCabeceraView.TipoCambio = this.ResolverDivisa(pagoContrato.Divisa.MonedaDestino.Codigo, this.vista.CodigoMonedaEmpresa).TipoCambio;
            #endregion
            this.vista.InformacionCabeceraView.CodigoMoneda = pagoContrato.Divisa.MonedaDestino.Codigo;

            this.ValidarFormaPago(desdeInterfaz);
        }

        /// <summary>
        /// Valida la forma de pago que se esta seleccionado
        /// </summary>
        public void ValidarFormaPago(bool? validacionUI = false) {
            vista.InformacionCabeceraView.DiasCredito = null;
            vista.InformacionCabeceraView.DiasFactura = null;
            vista.InformacionCabeceraView.LimiteCredito = null;
            vista.InformacionCabeceraView.CreditoDisponible = null;

            PagoContratoPSLBO pagoContrato = this.vista.PagoActual;
            AuditoriaBO auditoria = this.CrearObjetoAuditoria();
            SeguridadBO seguridad = this.CrearObjetoSeguridad();

            bool esCredito = this.vista.InformacionCabeceraView.FormaPago == this.vista.FormaPagoPorDefault;

            if (esCredito) {
                CreditoClienteBO creditoCliente = null;

                try {
                    creditoCliente = this.ResolverCreditoCliente(pagoContrato);
                } catch (Exception ex) {
                    creditoCliente = null;
                }

                if (creditoCliente != null) {
                    this.vista.InformacionCabeceraView.DiasCredito = creditoCliente.DiasCredito;
                    this.vista.InformacionCabeceraView.DiasFactura = creditoCliente.DiasFactura;
                    this.vista.InformacionCabeceraView.LimiteCredito = creditoCliente.LimiteCredito;
                    this.vista.InformacionCabeceraView.CreditoDisponible = FacadeEFacturacionBR.ConsultarCreditoDisponible(this.dctx, creditoCliente);
                }

                if (!this.HayCreditoDisponible(this.vista.TransaccionActual, creditoCliente)) {
                    String tipoCredito = this.ResolverTipoCredito(pagoContrato).ToString();
                    String errorMessage = String.Format("El cliente no cuenta con crédito disponible suficiente o el crédito se encuentra inactivo: {0}", tipoCredito);

                    if (validacionUI != null && validacionUI == true) {
                        errorMessage = "El Cliente no cuenta con crédito en la moneda seleccionada: " + pagoContrato.Divisa.MonedaDestino.Codigo;
                        errorMessage = errorMessage + ". Mover el Pago al Visor de Pagos NO FACTURADOS?";
                        this.vista.MostrarMensaje(errorMessage, ETipoMensajeIU.CONFIRMACION);
                    } else {
                        this.vista.MostrarMensaje(errorMessage, ETipoMensajeIU.ADVERTENCIA);
                        MoverPagoFaltaCredito(errorMessage, auditoria, pagoContrato, seguridad);
                    }
                } else
                    this.vista.PermitirTerminar(true);
            }
        }

        /// <summary>
        /// Determina si existe crédito disponible para una transacción de facturación
        /// </summary>
        /// <param name="transaccion">Objeto que contiene la transacción a aplicar</param>
        /// <param name="creditoCliente">Objeto que contiene los datos del crédito del cliente</param>
        /// <returns>Devuelve true si hay crédito disponible, de lo contrario devolverá false</returns>
        private bool HayCreditoDisponible(TransaccionBO transaccion, CreditoClienteBO creditoCliente) {
            if (creditoCliente == null || !creditoCliente.Activo.GetValueOrDefault())
                return false;

            decimal creditoDisponible = FacadeEFacturacionBR.ConsultarCreditoDisponible(this.dctx, creditoCliente);
            return (creditoDisponible >= this.vista.LineasFacturaView.TotalFactura.GetValueOrDefault());
        }

        /// <summary>
        /// Ejecuta el proceso de registrar la factura en curso
        /// </summary>
        public void RegistrarFacturacion() {
            #region Validaciones
            if (!this.vista.UsoCFDIID.HasValue || string.IsNullOrWhiteSpace(this.vista.DescripcionUsoCFDI)) {
                this.vista.MostrarMensaje("Los siguientes campos son obligatorios: Uso de CFDI.", ETipoMensajeIU.ADVERTENCIA);
                return;
            }
            #endregion /Validaciones

            #region Mapeo UsoCFDI
            UsoCFDIBO usoTransaccion = new UsoCFDIBO() { 
                Id = this.vista.UsoCFDIID, NombreCorto = this.vista.ClaveUsoCFDI, Nombre = this.vista.DescripcionUsoCFDI
            };
            this.vista.TransaccionActual.UsoCFDI = usoTransaccion;
            #endregion

            Guid firma = Guid.NewGuid();
            SeguridadBO seguridad = this.CrearObjetoSeguridad();
            AuditoriaBO auditoria = this.CrearObjetoAuditoria();
            bool esCredito = this.vista.InformacionCabeceraView.FormaPago == this.vista.FormaPagoPorDefault;

            PagoContratoPSLBO pagoContrato = this.vista.PagoActual;
            ConfiguracionFacturacionBR.IniciarConsulta(this.dctx);

            Object lockValue = ConfigurarFacturacionPSLPRE.ResolverObjetoBloqueoPagoContrato(pagoContrato);
            try {
                Monitor.Enter(lockValue);

                pagoContrato.Divisa.MonedaDestino = this.vista.InformacionCabeceraView.CodigoMoneda != "-1"
                                                ? new MonedaBO { Codigo = this.vista.InformacionCabeceraView.CodigoMoneda }
                                                : pagoContrato.Divisa.MonedaOrigen;

                //validar
                TransaccionBO transaccion = this.ClonarTransaccion(this.vista.TransaccionActual);

                CreditoClienteBO creditoCliente = null;
                if (esCredito)
                    creditoCliente = this.ResolverCreditoCliente(pagoContrato);

                this.AcompletarTransaccion(pagoContrato, creditoCliente, transaccion);


                #region SC0037 Validacion de los montos de la transaccion

                Decimal? subtotalTransaccion = 0M;
                Decimal? subtotalImpuestos = 0M;

                foreach (DetalleTransaccionBO detalle in transaccion.GetChildren()) {
                    subtotalTransaccion += detalle.PrecioUnitario;
                    subtotalImpuestos += detalle.ImpuestoUnitario;
                }
                if (subtotalTransaccion + subtotalImpuestos != transaccion.TotalFactura)
                    throw new Exception("La suma de los precios de las líneas no coincide con el monto Total de la Factura.");

                if (pagoContrato.ReferenciaContrato != null && pagoContrato.ReferenciaContrato.Sucursal != null && pagoContrato.ReferenciaContrato.Sucursal.Impuesto != null && pagoContrato.ReferenciaContrato.Sucursal.Impuesto.PorcentajeImpuesto != null) {
                    if ((subtotalTransaccion * (pagoContrato.ReferenciaContrato.Sucursal.Impuesto.PorcentajeImpuesto / 100M) - transaccion.Impuestos) >= 0.01M) {
                        while ((subtotalTransaccion * (pagoContrato.ReferenciaContrato.Sucursal.Impuesto.PorcentajeImpuesto / 100M) - transaccion.Impuestos) >= 0.01M) {
                            transaccion.Impuestos += 0.01M;
                            var detalle = transaccion.GetChildren().OfType<DetalleTransaccionBO>().FirstOrDefault();
                            if (detalle != null) {
                                detalle.ImpuestoUnitario += 0.01M;
                            }
                        }
                    }
                }

                #endregion

                ReservaCreditoBO reservaCredito = null;

                if (esCredito)
                    reservaCredito = new ReservaCreditoBO {
                        TipoTransaccion = transaccion.TipoTransaccion,
                        TransaccionId = transaccion.TransaccionId,
                        Fecha = transaccion.Fecha,
                        CuentaCliente = transaccion.CuentaCliente,
                        Sucursal = pagoContrato.ReferenciaContrato.Sucursal,
                        Moneda = pagoContrato.Divisa.MonedaDestino,
                        Importe = transaccion.TotalFactura,
                        Auditoria = auditoria
                    };

                pagoContrato.TotalFactura = transaccion.TotalFactura;
                pagoContrato.IvaFacturado = transaccion.Impuestos;
                pagoContrato.Observaciones = this.vista.InformacionCabeceraView.Observaciones;
                pagoContrato.BloqueadoCredito = false;
                pagoContrato.Auditoria.FUA = DateTime.Now;
                pagoContrato.Auditoria.UUA = seguridad.Usuario.Id;

                if (pagoContrato.TotalFactura - pagoContrato.IvaFacturado > 0) {
                    if (esCredito && !this.HayCreditoDisponible(transaccion, creditoCliente)) {
                        String tipoCredito = this.ResolverTipoCredito(pagoContrato).ToString();
                        String errorMessage = String.Format("El cliente no cuenta con crédito disponible suficiente o el crédito se encuentra inactivo: {0}", tipoCredito);
                        pagoContrato.BloqueadoCredito = true;
                        this.vista.PermitirTerminar(false);
                        this.vista.MostrarMensaje(errorMessage, ETipoMensajeIU.ADVERTENCIA);
                    }

                    ConfiguracionFacturacionBR.TerminarConsulta(this.dctx);

                    //Guardamos en sesión
                    this.vista.TransaccionActual = transaccion;

                    #region Conexion a BD
                    //Es necesario redireccionar antes de invocar la transacción para que funcione correctamente.
                    this.dctx.SetCurrentProvider("LIDER");
                    try {
                        this.dctx.OpenConnection(firma);
                        this.dctx.BeginTransaction(firma);
                    } catch (Exception) {
                        if (this.dctx.ConnectionState == ConnectionState.Open)
                            this.dctx.CloseConnection(firma);
                        throw new Exception("Se encontraron inconsistencias al acceso al origen de datos durante el Registrar Transacción.");
                    }
                    #endregion
                    this.ActualizarPagos(pagoContrato, pagoContrato, seguridad, this.vista.TipoPagoContrato);

                    bool sended = false;
                    if (!pagoContrato.BloqueadoCredito.GetValueOrDefault()) {
                        FacadeEFacturacionBR.RegistrarTransaccionCompleto(this.dctx, reservaCredito, transaccion, seguridad);

                        pagoContrato.EnviadoFacturacion = true;
                        pagoContrato.FechaEnvioFacturacion = transaccion.Fecha;
                        this.ActualizarPagos(pagoContrato, pagoContrato, seguridad, this.vista.TipoPagoContrato);
                        sended = true;
                    }

                    this.dctx.SetCurrentProvider("LIDER");
                    this.dctx.CommitTransaction(firma);

                    if (sended)
                        this.vista.MostrarMensaje("El envío de la factura se ha realizado satisfactoriamente", ETipoMensajeIU.EXITO);

                    this.vista.PermitirTerminar(false);
                    this.vista.PermitirCancelar(false);
                    this.vista.PertimirCapturar(false);
                    this.vista.InformacionCabeceraView.permitirCaptura(false);
                    this.vista.EnvioFactura = "SI";
                } else {
                    pagoContrato.FacturaEnCeros = true;
                    pagoContrato.EnviadoFacturacion = true;
                    this.ActualizarPagos(pagoContrato, pagoContrato, seguridad, this.vista.TipoPagoContrato);

                    this.dctx.SetCurrentProvider("LIDER");
                    this.dctx.CommitTransaction(firma);

                    this.vista.MostrarMensaje("El monto de la factura se encuentra en Ceros", ETipoMensajeIU.ADVERTENCIA);
                    this.vista.PermitirTerminar(false);
                    this.vista.PermitirCancelar(false);
                }
            } catch (Exception ex) {
                this.dctx.SetCurrentProvider("LIDER");
                this.dctx.RollbackTransaction(firma);

                try {
                    LogFacturacionBO log = new LogFacturacionBO();
                    log.Auditoria = auditoria;
                    log.MensajeError = ex.GetBaseException().Message;
                    log.NumeroReferencia = pagoContrato.Referencia;
                    //PARCHE YA QUE SOLO SOPORTA PAGO UNIDAD CONTRATO PARA GENERAR EL CONTRATO
                    log.Pago = new PagoUnidadContratoRDBO() { PagoID = pagoContrato.PagoContratoID };
                    log.ReferenciaContrato = pagoContrato.ReferenciaContrato;
                    this.logFacturacionBR.Insertar(this.dctx, log, seguridad);
                } catch { }

                throw;
            } finally {
                Monitor.Exit(lockValue);
                this.dctx.SetCurrentProvider("LIDER");
                if (dctx.ConnectionState == ConnectionState.Open)
                    dctx.CloseConnection(firma);
            }
        }

        /// <summary>
        /// Calcula los impuestos de una transacción y devuelve la lista de detalles procesados
        /// </summary>
        /// <param name="transaccion">Transacción a procesar</param>
        /// <param name="pagoContrato">Objeto de pago que contiene información para obtención de los impuestos</param>
        /// <param name="creditoCliente">Crédito del cliente en curso</param>
        /// <returns>Lista de lineas de concepto</returns>
        private IEnumerable<DetalleTransaccionBO> CalcularImpuestos(IEnumerable<IComponentBase> detallesTransaccion, PagoContratoPSLBO pagoContrato, CreditoClienteBO creditoCliente) {
            if (detallesTransaccion == null || detallesTransaccion.Count() == 0)
                yield break;

            AuditoriaBO auditoria = this.CrearObjetoAuditoria();
            DepartamentoBO departamento = this.departamentoFacturacionBR.Consultar(pagoContrato.ReferenciaContrato.TipoContratoID.Value, this.vista.ObtenerCarpetaRaiz(), pagoContrato.ReferenciaContrato.UnidadOperativa.Id);

            foreach (var childrem in detallesTransaccion) {
                if (!(childrem is DetalleTransaccionBO))
                    continue;

                DetalleTransaccionBO detalle = childrem as DetalleTransaccionBO;

                if (!detalle.DescuentoUnitario.HasValue)
                    detalle.DescuentoUnitario = 0M;

                if (!detalle.Cantidad.HasValue)
                    detalle.Cantidad = 1;

                if (!detalle.AplicaIVA.HasValue)
                    detalle.AplicaIVA = true;

                if (detalle.UnidadMedida == null)
                    detalle.UnidadMedida = new UnidadMedidaBO { Id = this.vista.UnidadMedidaIDPorDefault, Nombre = this.vista.UnidadMedidaPorDefault };

                Decimal truncadoSubtotalLinea = Decimal.Truncate(detalle.PrecioUnitario.GetValueOrDefault() * 100M) / 100M;
                detalle.PrecioUnitario = truncadoSubtotalLinea;

                decimal subtotalLinea = detalle.PrecioUnitario.GetValueOrDefault() * detalle.Cantidad.GetValueOrDefault();
                subtotalLinea = Decimal.Truncate((subtotalLinea * 100M)) / 100M;
                decimal impuestoLinea = detalle.AplicaIVA.GetValueOrDefault() && pagoContrato.ReferenciaContrato.Sucursal.Impuesto != null && pagoContrato.ReferenciaContrato.Sucursal.Impuesto.PorcentajeImpuesto.HasValue
                                        ? subtotalLinea * pagoContrato.ReferenciaContrato.Sucursal.Impuesto.PorcentajeImpuesto.GetValueOrDefault() / 100
                                        : 0M;

                if (!detalle.RetencionUnitaria.HasValue)
                    detalle.RetencionUnitaria = 0M;

                detalle.ImpuestoUnitario = Decimal.Truncate(impuestoLinea * 100M) / 100M;

                detalle.Departamento = departamento;
                detalle.Auditoria = auditoria;
                detalle.Activo = true;

                if (creditoCliente != null) {
                    detalle.DiasCredito = creditoCliente.DiasCredito;
                    detalle.DiasFactura = creditoCliente.DiasFactura;
                }

                yield return detalle;
            }
        }

        #region RI0024
        /// <summary>
        /// Realiza un ajuste de conceptos de transacción verificando que los conceptos sean consistentes
        /// </summary>
        /// <param name="transaccion">Objeto de transacción a procesar</param>
        /// <param name="costosAdicionales">Costos adicionales a agregar</param>
        /// <param name="creditoCliente">Objeto de crédito de cliente</param>
        /// <param name="pagoContrato">Objeto de pago</param>
        private List<DetalleTransaccionBO> ReajustarDetallesTransaccion(TransaccionBO transaccion, IList<DetalleTransaccionBO> costosAdicionales, PagoContratoPSLBO pagoContrato, CreditoClienteBO creditoCliente) {
            List<DetalleTransaccionBO> detallesTransaccionBase = transaccion.GetChildren()
                                                                    .OfType<DetalleTransaccionBO>()
                                                                    .ToList();

            #region[Actualización de la clave del articulo]
            PagoContratoPSLBO pagoActualCnt = this.vista.PagoActual;
            foreach (var detalleTransaccionBo in detallesTransaccionBase) {
                if (pagoActualCnt != null && pagoActualCnt.ProductoServicio != null) {
                    if (detalleTransaccionBo.Articulo != null) {
                        if (string.IsNullOrEmpty(detalleTransaccionBo.Articulo.NombreCorto))
                            detalleTransaccionBo.Articulo.NombreCorto = pagoActualCnt.ProductoServicio.NombreCorto;
                    }
                }
                detalleTransaccionBo.DiasCredito = this.vista.InformacionCabeceraView.DiasCredito;
            }
            #endregion

            #region[Obtenemos los costos adicionales en sesión para agregar a la transacción]
            object listPagosUnidad = this.vista.InformacionLineasFacturaModel;  //Obtenemos los registros del modelo de la sesión
            List<LineasFacturaModel> listaPagosUnidad = listPagosUnidad as List<LineasFacturaModel>;
            costosAdicionales = new List<DetalleTransaccionBO>();
            DetalleTransaccionBO detTransaccionSeguro = new DetalleTransaccionBO();
            foreach (LineasFacturaModel lineaFactura in listaPagosUnidad) {
                if (lineaFactura.detalleTransaccion != null && lineaFactura.detalleTransaccion.Count > 0) {
                    foreach (DetalleTransaccionBO detTran in lineaFactura.detalleTransaccion) {
                        if (detTran.DiasCredito == null)
                            detTran.DiasCredito = this.vista.InformacionCabeceraView.DiasCredito;
                        #region mapea detalle Seguro
                        if (detTran.TipoRenglon == ETipoRenglon.SEGUROS) {
                            if (detTransaccionSeguro == null || detTransaccionSeguro.Id == null) {
                                detTransaccionSeguro.Id = detTran.Id;
                                detTransaccionSeguro.AplicaIVA = detTran.AplicaIVA;
                                detTransaccionSeguro.Articulo = new ArticuloBO();
                                detTransaccionSeguro.Articulo = detTran.Articulo;
                                detTransaccionSeguro.PrecioUnitario = detTran.PrecioUnitario;
                                detTransaccionSeguro.CostoUnitario = detTran.CostoUnitario;
                                detTransaccionSeguro.DescuentoUnitario = 0M;
                                detTransaccionSeguro.RetencionUnitaria = 0M;
                                detTransaccionSeguro.ImpuestoUnitario = detTran.ImpuestoUnitario;
                                detTransaccionSeguro.Departamento = new DepartamentoBO();
                                detTransaccionSeguro.Departamento = detTran.Departamento;
                                detTransaccionSeguro.EstablecerTipoRenglon(ETipoRenglon.SEGUROS.ToInt32(CultureInfo.InvariantCulture));
                                detTransaccionSeguro.Cantidad = detTran.Cantidad;
                                detTransaccionSeguro.UnidadMedida = new UnidadMedidaBO();
                                detTransaccionSeguro.UnidadMedida = detTran.UnidadMedida;
                                detTransaccionSeguro.ProductoServicio = new ProductoServicioBO();
                                detTransaccionSeguro.ProductoServicio = detTran.ProductoServicio;
                                detTransaccionSeguro.Activo = true;
                            }else {
                                detTransaccionSeguro.PrecioUnitario += detTran.PrecioUnitario;
                                detTransaccionSeguro.ImpuestoUnitario += detTran.ImpuestoUnitario;
                            }
                        #endregion
                        } else {
                            costosAdicionales.Add(detTran);
                        }
                    }
                }
            }
            if (detTransaccionSeguro != null && detTransaccionSeguro.Id != null) {
                costosAdicionales.Add(detTransaccionSeguro);
            }
            #endregion

            if (costosAdicionales.Count > 0) {
                bool bandera = true;
                foreach (DetalleTransaccionBO detalle in costosAdicionales)
                    if (detalle.TipoRenglon == ETipoRenglon.SEGUROS) {
                        if (bandera) {
                            transaccion.Add(detTransaccionSeguro);
                            bandera = false;
                        }
                    } else {
                        transaccion.Add(detalle);
                    }
            }

            List<DetalleTransaccionBO> lineasContrato = this.CalcularImpuestos(transaccion, pagoContrato, creditoCliente);

            bool lineasBaseEnCero = detallesTransaccionBase
                                    .All(x => x.PrecioUnitario <= 0);

            if (lineasBaseEnCero && costosAdicionales.Count > 0) {
                DetalleTransaccionBO detalleTransaccion = costosAdicionales.First();
                String[] observaciones = detallesTransaccionBase
                                            .Select(x => x.Articulo.Nombre)
                                            .ToArray();

                detalleTransaccion.Observaciones = String.Join("\n", observaciones);

                //Se acorta la longitud de las observaciones
                if (detalleTransaccion.Observaciones.Length > 499)
                    detalleTransaccion.Observaciones = detalleTransaccion.Observaciones.Substring(0, 499);

                foreach (DetalleTransaccionBO detalleTransaccionCero in detallesTransaccionBase) {
                    transaccion.Remove(detalleTransaccionCero);
                    lineasContrato.Remove(detalleTransaccionCero);
                }
            }

            foreach (var detalleTransaccionBo in lineasContrato.Where(detalleTransaccionBo => !String.IsNullOrEmpty(detalleTransaccionBo.Observaciones) && detalleTransaccionBo.Observaciones.Length > 499)) {
                detalleTransaccionBo.Observaciones = detalleTransaccionBo.Observaciones.Substring(0, 499);
            }

            return lineasContrato;
        }
        #endregion

        /// <summary>
        /// Calcula los impuestos de una transacción y devuelve la lista de detalles procesados
        /// </summary>
        /// <param name="transaccion">Transacción a procesar</param>
        /// <param name="pagoContrato">Pago contrato</param>
        /// <param name="creditoCliente">Crédito del cliente en curso</param>
        /// <returns>Lista de lineas de concepto</returns>
        private List<DetalleTransaccionBO> CalcularImpuestos(TransaccionBO transaccion, PagoContratoPSLBO pagoContrato, CreditoClienteBO creditoCliente) {
            List<DetalleTransaccionBO> detalles = new List<DetalleTransaccionBO>();

            transaccion.Subtotal = 0M;
            transaccion.Impuestos = 0M;
            transaccion.Descuentos = 0M;
            transaccion.Retenciones = 0M;

            foreach (var detalle in this.CalcularImpuestos(transaccion.GetChildren(), pagoContrato, creditoCliente)) {
                decimal subtotalLinea = detalle.PrecioUnitario.GetValueOrDefault() * detalle.Cantidad.GetValueOrDefault();

                transaccion.Subtotal += subtotalLinea;
                transaccion.Impuestos += detalle.ImpuestoUnitario;
                transaccion.Descuentos += detalle.DescuentoUnitario;
                transaccion.Retenciones += detalle.RetencionUnitaria;
                detalles.Add(detalle);
            }

            return detalles;
        }

        /// <summary>
        /// Realiza el proceso de cancelar 
        /// </summary>
        public void CancelarRegistro() {
            this.vista.RedirigirACancelacion();
        }

        /// <summary>
        /// Ejecuta el proceso de regresar a la página anterior
        /// </summary>
        public void RetrocederPagina() {
            int? paginaActual = this.vista.PaginaActual;
            if (paginaActual == null)
                throw new Exception("La página actual es nula.");
            if (paginaActual <= 0)
                throw new Exception("La página actual es menor o igual a cero y, por lo tanto, no se puede retroceder.");

            this.EstablecerOpcionesSegunPagina(paginaActual.Value - 1);
            this.EstablecerPagina(paginaActual.Value - 1);
        }

        /// <summary>
        /// Invoca al proceso de consultar otra factura
        /// </summary>
        public void ConsultarOtraFacturacion() {
            vista.RedirigirAConsulta();
        }

        /// <summary>
        /// SC0034, Determina si un pago es válido para ser facturado verificando que los pagos anteriores hayan sido enviados a facturar
        /// </summary>
        /// <param name="pagoContrato">Pago a ser evaluado</param>
        /// <returns>Devuelve true si el pago es el siguiente a ser enviado a facturar, de lo contrario devolverá false</returns>
        private bool EsPagoValido(PagoContratoPSLBO pagoContrato) {
            bool result = true;
            var pagoUnidadContratoLs = this.ConsultarPagos(null, pagoContrato.ReferenciaContrato.ReferenciaContratoID, false);
            if (pagoUnidadContratoLs.Count == 0)
                throw new Exception(String.Format("No se han encontrado pagos para la referencia {0}", pagoContrato.ReferenciaContrato.ReferenciaContratoID));

            var pagosAnteriores = pagoUnidadContratoLs
                                        .Where(x => x.PagoContratoID < pagoContrato.PagoContratoID && x.Activo.GetValueOrDefault())
                                        .OrderBy(x => x.PagoContratoID)
                                        .ToList();

            if (pagosAnteriores.Count > 0)
                result = pagosAnteriores.Last().EnviadoFacturacion.GetValueOrDefault();

            return result;
        }

        /// <summary>
        /// Establece las operación permitidas según un número de página
        /// </summary>
        /// <param name="numeroPagina">Número de página a evaluar</param>
        private void EstablecerOpcionesSegunPagina(int numeroPagina) {
            this.vista.PermitirRegresar(true);
            this.vista.PermitirContinuar(true);
            this.vista.OcultarContinuar(false);
            this.vista.OcultarTerminar(true);

            if (numeroPagina == 1) {
                this.vista.PermitirRegresar(false);
            }

            if (numeroPagina == 2) {
                this.vista.PermitirContinuar(false);

                this.vista.OcultarContinuar(true);
                this.vista.OcultarTerminar(false);
            }
        }

        /// <summary>
        /// Decide que controlador usar para consultar pagos
        /// </summary>
        private List<PagoContratoPSLBO> ConsultarPagos(Int32? pagoContratoId, Int32? referenciaContratoId, Boolean esCompleto) {
            PagoContratoPSLBOF pagoFilter = new PagoContratoPSLBOF();

            if (pagoContratoId != null)
                pagoFilter.PagoContratoID = pagoContratoId;
            if (referenciaContratoId != null)
                pagoFilter.ReferenciaContrato = new ReferenciaContratoBO() { ReferenciaContratoID = referenciaContratoId };

            List<PagoContratoPSLBO> pagoContratoList = esCompleto ? this.pagoContratoPSLBR.ConsultarCompleto(this.dctx, pagoFilter) : this.pagoContratoPSLBR.Consultar(this.dctx, pagoFilter);

            return pagoContratoList;
        }

        /// <summary>
        /// Utilizar el controlador adecuado para actualizar los pagos
        /// </summary>
        private void ActualizarPagos(PagoContratoPSLBO pago, PagoContratoPSLBO pagoAnterior, SeguridadBO seguridadBo, ETipoContrato? tipoContrato) {
            this.pagoContratoPSLBR.ActualizarCompleto(this.dctx, pago, pagoAnterior, seguridadBo);
        }

        /// <summary>
        /// Obtiene información del Pago y lo envía al Visor de Pagos No Facturados.
        /// </summary>
        public void ConfirmarMoverPagoSinCredito() {
            try {
                PagoContratoPSLBO pagoContrato = this.vista.PagoActual;

                pagoContrato.Divisa.MonedaDestino = this.vista.InformacionCabeceraView.CodigoMonedaDestino != "-1"
                                                                ? new MonedaBO { Codigo = this.vista.InformacionCabeceraView.CodigoMonedaDestino }
                                                                : this.vista.PagoActual.Divisa.MonedaOrigen;

                pagoContrato.Divisa = this.ResolverDivisa(this.vista.PagoActual);
                AuditoriaBO auditoria = this.CrearObjetoAuditoria();
                SeguridadBO seguridad = this.CrearObjetoSeguridad();
                String tipoCredito = this.ResolverTipoCredito(pagoContrato).ToString();
                String errorMessage = String.Format("El cliente no cuenta con crédito disponible suficiente o el crédito se encuentra inactivo: {0}", tipoCredito);
                MoverPagoFaltaCredito(errorMessage, auditoria, pagoContrato, seguridad);
            } catch (Exception ex) {
                throw new Exception(nombreClase + "ConfirmarMoverPagoSinCredito(): " + ex.Message);
            }
        }

        /// <summary>
        /// Mueve el Pago al visor de Pagos NO Facturados por Falta de Credito
        /// </summary>
        /// <param name="errorMessage">Mensaje de error para el Log</param>
        /// <param name="auditoria">Objeto de Auditoria</param>
        /// <param name="pagoContrato">Pago sin crédito</param>
        /// <param name="seguridad">Objeto de Seguridad</param>
        private void MoverPagoFaltaCredito(string errorMessage, AuditoriaBO auditoria, PagoContratoPSLBO pagoContrato, SeguridadBO seguridad) {
            this.vista.PermitirTerminar(false);
            this.vista.PertimirCapturar(false);

            LogFacturacionBO log = new LogFacturacionBO();
            log.Auditoria = auditoria;
            log.MensajeError = errorMessage;
            log.NumeroReferencia = pagoContrato.Referencia;
            //PARCHE PARA MANDAR A LOG
            log.Pago = new PagoUnidadContratoRDBO() { PagoID = pagoContrato.PagoContratoID };
            log.ReferenciaContrato = pagoContrato.ReferenciaContrato;
            this.logFacturacionBR.Insertar(this.dctx, log, seguridad);

            var pagoAnterior = this.ConsultarPagos(pagoContrato.PagoContratoID, pagoContrato.ReferenciaContrato.ReferenciaContratoID, true).FirstOrDefault();
            if (pagoAnterior == null)
                throw new Exception("No se encontró el pago para actualizar.");

            pagoContrato.BloqueadoCredito = true;
            this.vista.PagoActual = pagoContrato;
            this.ActualizarPagos(pagoContrato, pagoAnterior, seguridad, pagoContrato.TipoContrato);
        }

        /// <summary>
        /// Calcula el monto para el seguro
        /// </summary>
        /// <param name="pagoUnidadContrato">Pago que contiene la unidad para buscar el seguro</param>
        /// <param name="generador">Generador utilizado para configurar la Transacción</param>
        /// <returns>Retorna una observación en caso de que exista alguna inconsistencia</returns> 
        private void CalcularCobraSeguro(PagoContratoPSLBO pagoContrato, IGeneradorPaquetesFacturacionPSLBR generador, decimal? porcentajeSeguro) {
            try {            
                object listPagosUnidad = this.vista.InformacionLineasFacturaModel;
                    List<LineasFacturaModel> lstPagos = listPagosUnidad as List<LineasFacturaModel>;
                    foreach (LineasFacturaModel lineaModelo in lstPagos) {
                        IList<DetalleTransaccionBO> detalleTran = this.vista.CostosAdicionalesView.CostosAdicionales;
                        lineaModelo.detalleTransaccion = new List<DetalleTransaccionBO>();
                        lineaModelo.MontoCargo = 0;
                        if (detalleTran != null && detalleTran.Count > 0) {                            
                            foreach (DetalleTransaccionBO detTran in detalleTran) {
                                lineaModelo.detalleTransaccion.Add(detTran);
                                lineaModelo.MontoCargo += detTran.PrecioUnitario;
                            }
                        }
                        #region Costo adicional seguro
                        int? id = this.vista.CostosAdicionalesView.CostosAdicionales
                                .Select(it => (int?)it.Id)
                                .Max();

                        if (!id.HasValue)
                            id = 0;
                        id++;

                        DetalleTransaccionBO detalle = new DetalleTransaccionBO();
                        detalle.Id = id;
                        detalle.AplicaIVA = true; //Se cambia a que sí aplique IVA para calcular importe y PCTJE
                        detalle.Articulo = new ArticuloBO { Nombre = MensajeCobroSeguro };
                        detalle.PrecioUnitario = Math.Round( (decimal)(lineaModelo.CargoFijo * porcentajeSeguro / 100) , 2); 
                        detalle.CostoUnitario = generador.Costo;
                        detalle.DescuentoUnitario = 0M;
                        detalle.RetencionUnitaria = 0M;
                        detalle.EstablecerTipoRenglon(ETipoRenglon.SEGUROS.ToInt32(CultureInfo.InvariantCulture));
                        detalle.Cantidad = generador.Cantidad;
                        detalle.UnidadMedida = generador.UnidadMedida;
                        detalle.ProductoServicio = new ProductoServicioBO();
                        detalle.ProductoServicio = pagoContrato.ProductoServicio;
                        detalle.Activo = true;
                        lineaModelo.detalleTransaccion.Add(detalle);
                        lineaModelo.MontoCargo += detalle.PrecioUnitario;
                        #endregion
                        lineaModelo.MontoCargo = lineaModelo.CargoFijo + lineaModelo.MontoCargo + lineaModelo.CargosAdicionales.Select(d => d.PrecioUnitario).Sum();
                    }
            } catch (Exception ex) {
                throw new Exception("CalcularCobroSeguro: Hubo un error al crear el cobro. " + ex.Message );
            }
        }

        /// <summary>
        /// Obtiene el tipo de crédito para un pago de unidad de contrato
        /// </summary>
        /// <param name="pagoContrato">Objeto de pago a evaluar</param>       
        /// <returns>Objeto de tipo de crédito</returns>
        private ETipoCredito ResolverTipoCredito(PagoContratoPSLBO pagoContrato) {
            ETipoCredito result = ETipoCredito.REVOLVENTE;

            switch (pagoContrato.ReferenciaContrato.TipoContratoID.Value) {
                case ETipoContrato.RO:
                case ETipoContrato.RE:
                    result = ETipoCredito.REVOLVENTE;
                    break;

                case ETipoContrato.ROC:
                    result = ETipoCredito.ACUERDO_DE_VENTAS;
                    break;
            }

            return result;
        }

        /// <summary>
        /// Arma la información para desplegar en el grid de unidades en el grid del wizard
        /// </summary>
        /// <param name="pagoContrato">Información del pago del contrato</param>
        /// <param name="contrato">Información del contrato</param>
        private void ArmarModeloVista(PagoContratoPSLBO pagoContrato, ContratoPSLBO contrato) {
            List<LineasFacturaModel> listaUnidadesModel = new List<LineasFacturaModel>();
            IGeneradorPaquetesFacturacionPSLBR generador = new GeneradorPaqueteFacturacionPSLBR();
            bool banderaSeguro = false;
            if (contrato != null) {
                List<LineaContratoPSLBO> lstLineasContrato = contrato.LineasContrato.ConvertAll(s => (LineaContratoPSLBO)s);
                List<PagoUnidadContratoPSLBO> lstPagosUnidad = pagoContrato.ObtenerPagosUnidadesContratos();

                decimal? porcentajeSeguro = 0;
                foreach (LineaContratoPSLBO lineaCto in lstLineasContrato) {                    
                    if (((TarifaContratoPSLBO)lineaCto.Cobrable).PorcentajeSeguro != null) 
                        porcentajeSeguro = ((TarifaContratoPSLBO)lineaCto.Cobrable).PorcentajeSeguro.GetValueOrDefault();
                }

                foreach (PagoUnidadContratoPSLBO pagoUnidad in lstPagosUnidad) {
                    LineasFacturaModel LineaModelo = new LineasFacturaModel();
                    LineaModelo.CargosAdicionales = new List<CargoAdicionalModel>();

                    LineaContratoPSLBO _linea = contrato.LineasContrato.ConvertAll(l => (LineaContratoPSLBO)l)
                    .FirstOrDefault(l => ((l as LineaContratoPSLBO).Equipo as UnidadBO).UnidadID == pagoUnidad.Unidad.UnidadID);

                    if (_linea != null && _linea.Equipo != null)
                        pagoUnidad.Unidad = (UnidadBO)_linea.Equipo;

                    string descripcionLinea = string.Empty;

                    #region[Obtener descripción de las líneas]
                    if (string.IsNullOrEmpty(pagoUnidad.LineaDescripcion))
                    {
                        ETipoEquipoAliado? tipoEquipoAliado = null;
                        if (pagoUnidad.Unidad.EquiposAliados.Any())
                        {
                            var tipos = pagoUnidad.Unidad.EquiposAliados
                                        .Where(x => x.TipoEquipoAliado.HasValue)
                                        .Select(x => x.TipoEquipoAliado)
                                        .ToList();

                            if (tipos.Count > 0)
                                tipoEquipoAliado = tipos.Any(x => x.Value == ETipoEquipoAliado.Refrigerado) ? ETipoEquipoAliado.Refrigerado : ETipoEquipoAliado.Seco;
                            else
                                tipoEquipoAliado = ETipoEquipoAliado.Seco;
                        }
                        else tipoEquipoAliado = ETipoEquipoAliado.Seco;

                        String tipoEquipoAliadoDescripcion = this.ResolverDescripcionTipoEquipoAliado(tipoEquipoAliado);

                        string fechaInicioFactura = contrato.FechaInicioActual.GetValueOrDefault().ToString("dd/MM/yyyy");
                        string fechaFinalFactura = contrato.FechaPromesaActual.GetValueOrDefault().ToString("dd/MM/yyyy");
                        int diasFactura = (contrato.FechaPromesaActual.Value.Date - contrato.FechaInicioActual.Value.Date).Days + 1;
                        Dictionary<string, string> valores = new Dictionary<string, string>();

                        valores.Add("Contrato", pagoContrato.ReferenciaContrato != null && !string.IsNullOrEmpty(pagoContrato.ReferenciaContrato.FolioContrato) ? pagoContrato.ReferenciaContrato.FolioContrato : "N/A");
                        valores.Add("TipoEquipoAliado", tipoEquipoAliadoDescripcion);
                        valores.Add("Modelo", !string.IsNullOrEmpty(pagoUnidad.Unidad.Modelo.Nombre) ? pagoUnidad.Unidad.Modelo.Nombre : "N/A");
                        valores.Add("Serie", !string.IsNullOrEmpty(pagoUnidad.Unidad.NumeroSerie) ? pagoUnidad.Unidad.NumeroSerie : "N/A");
                        valores.Add("NumeroEconomico", !string.IsNullOrEmpty(pagoUnidad.Unidad.NumeroEconomico) ? pagoUnidad.Unidad.NumeroEconomico : "N/A");
                        valores.Add("FechaInicialFactura", fechaInicioFactura);
                        valores.Add("POR", "POR");
                        valores.Add("DiasFacturar", !string.IsNullOrEmpty(diasFactura.ToString()) ? diasFactura.ToString() : "N/A");
                        valores.Add("DiasUHoras", "DIAS");
                        valores.Add("FechaFinalFactura", fechaFinalFactura);
                        var descripciones = generador.ObtenerDescripcionLinea(contrato.Tipo, valores, false);
                        descripcionLinea = descripciones["RentaFija"];
                    }
                    else
                        descripcionLinea = pagoUnidad.LineaDescripcion;
                    #endregion

                    LineaModelo.PagoContratoPSLID = pagoUnidad.PagoUnidadContratoID;
                    LineaModelo.DescripcionhoraAdicional = pagoUnidad.HorasExcedidasDescripcion;
                    LineaModelo.DescripcionLinea = descripcionLinea;
                   
                    LineaModelo.ModeloUnidad = !string.IsNullOrEmpty(pagoUnidad.Unidad.Modelo.Nombre) ? pagoUnidad.Unidad.Modelo.Nombre : "";
                    LineaModelo.CargoFijo = pagoUnidad.Tarifas.CargoFijo;
                    LineaModelo.MontoCargo = pagoUnidad.Tarifas.CargoFijo + pagoUnidad.LineasCostoAdicional.Sum(s => s.PrecioUnitario);
                    LineaModelo.NumeroCargo = pagoUnidad.LineasCostoAdicional.Count;
                    LineaModelo.NumeroSerie = pagoUnidad.Referencia;
                    LineaModelo.Tarifa = 0;
                    LineaModelo.TipoTarifa = "";
                    LineaModelo.Turno = "";
                    LineaModelo.UnidadID = pagoUnidad.Unidad.UnidadID;                    
                    listaUnidadesModel.Add(LineaModelo);
                    foreach (LineaCostoAdicionalBO adicional in pagoUnidad.LineasCostoAdicional) {
                        CargoAdicionalModel cargo = new CargoAdicionalModel();
                        cargo.LineaCostoAdicionalID = adicional.LineaCostoAdicionalID;
                        cargo.TipoRenglon = adicional.TipoRenglon;
                        cargo.Observaciones = adicional.Descripcion;
                        cargo.PrecioUnitario = adicional.PrecioUnitario;
                        cargo.Modificable = false;
                        cargo.Activo = adicional.Activo;
                        cargo.ClaveProductoServicio = adicional.ProductoServicio.Nombre;
                        cargo.Descripcion = adicional.Descripcion;
                        cargo.Estatus = "E";
                        LineaModelo.CargosAdicionales.Add(cargo);
                        if (adicional.TipoRenglon == ETipoRenglon.SEGUROS.ToInt32(CultureInfo.InvariantCulture)) {
                            banderaSeguro = true;
                        }
                    }

                }
                //Guarda en sesión la información del modelo
                this.vista.InformacionLineasFacturaModel = listaUnidadesModel;

                #region Seguro
                if (!banderaSeguro && porcentajeSeguro > 0) {
                   this.CalcularCobraSeguro(pagoContrato, generador, porcentajeSeguro);                    
                }
                #endregion
               
            }
        }


        public void DesplegarInfoBusqueda(string catalogo, object selecto) {
            this.vistaCargosAdicionales.DesplegarResultadoBuscador(catalogo, selecto);
        }

        /// <summary>
        /// Realiza el cambio de vista a cargos adicionales
        /// </summary>
        /// <param name="pagoUnidadPSLID"></param>
        public void PrepararVistaCargoAdicional(int pagoUnidadPSLID) {
            this.vista.CambiarCapturarCargo(pagoUnidadPSLID);
            this.vistaCargosAdicionales.PrepararCostoAdicionalPSL(pagoUnidadPSLID);
            this.vistaCargosAdicionales.permitirCaptura(this.vista.EnvioFactura == "SI" ? false : true);
        }

        /// <summary>
        /// Realiza el cambio a la vista de configuración afctura
        /// </summary>
        public void PrepararVistaCargoFactura() {
            this.vista.CambiarCapturarFactura();
            this.vistaCargosAdicionales.LimpiarSesion();
        }

        /// <summary>
        /// Guarda la información capturada en la vista de cargos adicionales
        /// </summary>
        public void GuardarVistaCargoFactura() {
            this.vista.CambiarCapturarFactura();
            this.AsignarCargosAdicionalesATransaccion();
            this.vistaCargosAdicionales.LimpiarSesion();
        }

        /// <summary>
        /// Actualiza la descripción capturada en la vista de unidades
        /// </summary>
        /// <param name="idLinea">Identificador del pago</param>
        /// <param name="tipoactualiza">Tipo de actualización : LINEA / HORA</param>
        /// <param name="descripcion">Descripción que se actualizará</param>
        public void ActualizaDescripcionLineaPago(int idLinea, string tipoactualiza, string descripcion) {
            PagoUnidadContratoPSLBR pagoUnidadbr = new PagoUnidadContratoPSLBR();
            PagoUnidadContratoPSLBO detPagoBO = new PagoUnidadContratoPSLBO();
            detPagoBO.PagoUnidadContratoID = idLinea;

            byte[] encodedDataAsBytes = System.Convert.FromBase64String(descripcion);
            string descripcionenvio = System.Text.Encoding.UTF8.GetString(encodedDataAsBytes);

            SeguridadBO seguridadUsr = CrearObjetoSeguridad();
            List<PagoUnidadContratoPSLBO> lstDetPago = pagoUnidadbr.Consultar(this.dctx, detPagoBO, null);
            if (lstDetPago.Any()) {
                detPagoBO = lstDetPago[0];
                if (tipoactualiza == "LINEA")
                    detPagoBO.LineaDescripcion = descripcionenvio;
                else
                    detPagoBO.HorasExcedidasDescripcion = descripcionenvio;
                pagoUnidadbr.Actualizar(this.dctx, detPagoBO, detPagoBO, seguridadUsr);
            }

            //Actualizar el importe de la sesión
            object listPagosUnidad = this.vista.InformacionLineasFacturaModel;  //Obtenemos los registros del modelo de la sesión
            List<LineasFacturaModel> listaPagosUnidad = listPagosUnidad as List<LineasFacturaModel>;
            foreach (LineasFacturaModel lineaFactura in listaPagosUnidad.Where(d => d.PagoContratoPSLID == idLinea))  //Obtenemos el modelo que se selecciono
            {
                if (tipoactualiza == "LINEA")
                    lineaFactura.DescripcionLinea = descripcionenvio;
                else
                    lineaFactura.DescripcionhoraAdicional = descripcionenvio;
            }

            this.vista.InformacionLineasFacturaModel = listaPagosUnidad;  //Guardamos la lista en sesión
        }

        /// <summary>
        /// Agrega los cargos adicionales a la transacción en sesión
        /// </summary>
        public void AsignarCargosAdicionalesATransaccion() {
            object listPagosUnidad = this.vista.InformacionLineasFacturaModel;  //Obtenemos los registros del modelo de la sesión
            List<LineasFacturaModel> listaPagosUnidad = listPagosUnidad as List<LineasFacturaModel>;

            //Se busca la línea capturada
            foreach (LineasFacturaModel lineaFactura in listaPagosUnidad.Where(d => d.PagoContratoPSLID == this.vista.PagoUnidadContratoID))  //Obtenemos el modelo que se selecciono
            {
                IList<DetalleTransaccionBO> detalleTran = this.vista.CostosAdicionalesView.CostosAdicionales;  //Obtenemos los costos adicionales capturados

                lineaFactura.detalleTransaccion = null;
                lineaFactura.detalleTransaccion = new List<DetalleTransaccionBO>();
                lineaFactura.MontoCargo = 0;

                if (detalleTran != null && detalleTran.Count > 0) {
                    foreach (DetalleTransaccionBO detTran in detalleTran) {
                        lineaFactura.detalleTransaccion.Add(detTran);
                        lineaFactura.MontoCargo += detTran.PrecioUnitario;
                    }
                }
                lineaFactura.MontoCargo = lineaFactura.CargoFijo + lineaFactura.MontoCargo + lineaFactura.CargosAdicionales.Select(d => d.PrecioUnitario).Sum();
            }

            //Actualización de las líneas en el grid de unidades
            this.vista.InformacionCabeceraView.MostrarUnidadesContrato(listaPagosUnidad);

            this.vista.InformacionLineasFacturaModel = listaPagosUnidad;  //Guardamos la lista en sesión
        }


        /// <summary>
        /// Obtiene la descripción de un Tipo de Equipo Aliado
        /// </summary>
        /// <param name="tipoEquipoAliado">Valor de Tipo de Equipo Aliado</param>
        /// <returns>Descripción de un Tipo de Equipo Aliado</returns>
        private String ResolverDescripcionTipoEquipoAliado(ETipoEquipoAliado? tipoEquipoAliado) {
            String descripcion = null;

            if (tipoEquipoAliado.HasValue) {
                descripcion = Enum.GetName(typeof(ETipoEquipoAliado), tipoEquipoAliado.Value);

                DescriptionAttribute descriptor = typeof(ETipoEquipoAliado).GetField(descripcion)
                                                        .GetCustomAttributes(typeof(DescriptionAttribute), true)
                                                        .Cast<DescriptionAttribute>()
                                                        .FirstOrDefault();

                if (descriptor != null && !String.IsNullOrEmpty(descriptor.Description))
                    descripcion = descriptor.Description;

                descripcion = descripcion.ToUpper();
            }

            return descripcion;
        }
        #endregion
    }

    public class LineasFacturaModel {
        public int? PagoContratoPSLID { get; set; }
        public int? LineaContratoPSLID { get; set; }
        public int? UnidadID { get; set; }
        public string NumeroSerie { get; set; }
        public string ModeloUnidad { get; set; }
        public string TipoTarifa { get; set; }
        public string Turno { get; set; }
        public decimal? Tarifa { get; set; }
        public string DescripcionhoraAdicional { get; set; }
        public string DescripcionLinea { get; set; }
        public int NumeroCargo { get; set; }
        public decimal? CargoFijo { get; set; }
        public decimal? MontoCargo { get; set; }
        public List<CargoAdicionalModel> CargosAdicionales { get; set; }
        public List<DetalleTransaccionBO> detalleTransaccion { get; set; }
    }

    public class CargoAdicionalModel {
        public int? LineaCostoAdicionalID { get; set; }
        public int? TipoRenglon { get; set; }
        public string Observaciones { get; set; }
        public decimal? PrecioUnitario { get; set; }
        public decimal? DescuentoUnitario { get; set; }
        public decimal? RetencionUnitario { get; set; }
        public int Cantidad { get; set; }
        public bool? AplicaIVA { get; set; }
        public bool Modificable { get; set; }
        public bool? Activo { get; set; }
        public string ClaveProductoServicio { get; set; }
        public string Descripcion { get; set; }
        public string Estatus { get; set; }
    }
}
