// Satisface CU005 - Armar paquetes facturación
// Satisface a la solicitud de cambio SC0016
// Satisface a la solicitud de cambio SC0017
// Satisface a la solicitud de cambio SC0021
// Satisface a la solicitud de cambio SC0028
// Satisface al Reporte de Inconsistencia RI0008
// Satisface al Reporte de Inconsistencia RI0017
// Satisface al Reporte de Inconsistencia RI0024
// BEP1401 Satisface a la SC0026
// BEP1401 Satisface a la SC0034
// Satisface a la SC0037
// Satisface a la solicitud de cambio SC0035
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
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
using BPMO.SDNI.Comun.BOF;
using BPMO.SDNI.Comun.BR;
using BPMO.SDNI.Contratos.BO;
using BPMO.SDNI.Equipos.BO;
using BPMO.SDNI.Equipos.BR;
using BPMO.SDNI.Facturacion.AplicacionesFacturacion.BR;
using BPMO.SDNI.Facturacion.AplicacionesFacturacion.VIS;
using BPMO.SDNI.Facturacion.GeneradorPaquetesFacturacion.BO;
using BPMO.SDNI.Facturacion.GeneradorPaquetesFacturacion.BR;
using BPMO.SDNI.Facturacion.InfraestructuraPagos.BO;
using BPMO.SDNI.Facturacion.InfraestructuraPagos.BOF;
using BPMO.SDNI.Facturacion.InfraestructuraPagos.BR;
using BPMO.Generales.BR;
using BPMO.SDNI.Contratos.RD.BO;
using BPMO.SDNI.Contratos.RD.BR;

namespace BPMO.SDNI.Facturacion.AplicacionesFacturacion.PRE
{   
    /// <summary>
    /// Presentador para la vista de configuración de una prefactura de contrato
    /// </summary>
    public class ConfigurarFacturacionPRE
    {
        #region Constantes
        /// <summary>
        /// Nombre de la clase para agregar a los mensajes de Error
        /// </summary>
        private static readonly string nombreClase = typeof(ConfigurarFacturacionPRE).Name;
        #endregion

        #region Atributos
        /// <summary>
        /// Objeto auxiliar para la sincronización de facturaciones
        /// </summary>
        private static readonly object syncFacturacion = new object();

        /// <summary>
        /// Moneda a facturar para cada linea factura
        /// </summary>
        public string MonedaPesos
        {
            get
            {
                try
                {
                    return ConfigurationManager.AppSettings["MonedaPesos"];
                }
                catch(Exception)
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// Mensaje que se presenta del cobro de Seguro Automatico
        /// </summary>
        private string MensajeCobroSeguro
        {
            get
            {
                try
                {
                    return ConfigurationManager.AppSettings["MensajeCobroSeguro"];
                }
                catch(Exception)
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// Objeto que almacena los pagos que se estan procesando durante el tiempo de vida del sitio
        /// </summary>
        private volatile static Dictionary<int, Object> pagosUnidadContratoList;

        /// <summary>
        /// El DataContext que proveerá acceso a la base de datos
        /// </summary>
        private IDataContext dctx;

        /// <summary>
        /// La vista que esta mostrando los datos procesados en el presentador
        /// </summary>
        private IConfigurarFacturacionVIS vista;

        /// <summary>
        /// Controlador para los pagos de unidad
        /// </summary>
        private PagoUnidadContratoBR pagoUnidadContratoBR;

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
        #endregion       

        #region Constructores
        /// <summary>
        /// Constructor por default del presentador
        /// </summary>
        /// <param name="vista">Vista que será gestionada por el presentador</param>
        public ConfigurarFacturacionPRE(IConfigurarFacturacionVIS vista)
        {
            try
            {
                this.vista = vista;                
                this.dctx = FacadeBR.ObtenerConexion();
                this.logFacturacionBR = new LogFacturacionBR();
                this.unidadBR = new UnidadBR();
                this.cuentaClienteIdealeaseBR = new CuentaClienteIdealeaseBR();
                this.departamentoFacturacionBR = new DepartamentoFacturacionBR();
                switch (vista.TipoPagoUnidadContrato)
                {
                        case ETipoContrato.FSL:
                        this.pagoUnidadContratoBR = new PagoUnidadContratoFSLBR();
                        break;
                        case ETipoContrato.RD:
                        this.pagoUnidadContratoBR = new PagoUnidadContratoRDBR();
                        break;
                        case ETipoContrato.CM:
                        case ETipoContrato.SD:
                        this.pagoUnidadContratoBR = new PagoUnidadContratoBR();
                        break;
                    default:
                        this.pagoUnidadContratoBR = new PagoUnidadContratoBR();
                        break;
                }
            }
            catch (Exception ex)
            {
                this.vista.MostrarMensaje("Inconsistencias en la configuración", ETipoMensajeIU.ERROR, nombreClase + ".ConfigurarFacturacionPRE: " + ex.GetBaseException().Message);
            }
        }

        #endregion

        #region Métodos
        /// <summary>
        /// Crea un objeto de seguridad con los datos actuales de la sesión de usuario en curso
        /// </summary>
        /// <returns>Objeto de tipo seguridad</returns>
        private SeguridadBO CrearObjetoSeguridad()
        {
            UsuarioBO usuario = new UsuarioBO() { Id = this.vista.UsuarioID };
            AdscripcionBO adscripcion = new AdscripcionBO() { UnidadOperativa = new UnidadOperativaBO() { Id = this.vista.UnidadOperativaID } };
            SeguridadBO seguridadBO = new SeguridadBO(Guid.Empty, usuario, adscripcion);

            return seguridadBO;
        }

        /// <summary>
        /// Crea un objeto de auditoria con los datos actuales de la sesión de usuario en curso
        /// </summary>
        /// <returns>Objeto de tipo auditoria</returns>
        private AuditoriaBO CrearObjetoAuditoria()
        {
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
        public void ValidarAcceso()
        {
            try
            {
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
            }
            catch (Exception ex)
            {
                throw new Exception(nombreClase + ".ValidarAcceso: " + ex.GetBaseException().Message);
            }
        }

        /// <summary>
        /// Proceso que se ejecuta cuando se crea una nueva factura
        /// </summary>
        public void PrepararNuevo()
        {
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
        private void EstablecerPagina(int numeroPagina)
        {
            switch (numeroPagina)
            {
                case 5:
                    if (this.vista.PagoActual != null)
                    {
                        this.RegenerarTransaccion();
                    }

                    break;
            }

            this.vista.EstablecerPagina(numeroPagina);
        }

        /// <summary>
        /// Redirige o visualiza una página especifica
        /// </summary>
        /// <param name="numeroPagina">Número de página a visualizar</param>
        private void IrAPagina(int numeroPagina)
        {
            if (numeroPagina < 1 || numeroPagina > 5)
                throw new Exception("La paginación va de 1 al 5.");

            this.EstablecerOpcionesSegunPagina(numeroPagina);
            this.EstablecerPagina(numeroPagina);
        }

        /// <summary>
        /// Recupera la cuenta de cliente asociada a un pago de contrato
        /// </summary>
        /// <param name="pagoUnidadContrato">Objeto de pago a evaluar</param>
        /// <returns>Objeto de tipo CuentaClienteBO recuperado</returns>
        private CuentaClienteBO ResolverCuentaCliente(PagoUnidadContratoBO pagoUnidadContrato)
        {
            StringBuilder messagesAdvertencia = new StringBuilder();
            List<CuentaClienteBO> cuentaClientes = FacadeBR.ConsultarCuentaClienteCompleto(
                this.dctx,
                new CuentaClienteBO
                {
                    Id = pagoUnidadContrato.ReferenciaContrato.CuentaCliente.Id,
                    UnidadOperativa = new UnidadOperativaBO { Id = pagoUnidadContrato.ReferenciaContrato.UnidadOperativa.Id }
                }
            );
            if (cuentaClientes.Count == 0)
                throw new Exception(String.Format("La cuenta cliente {0} que esta asociada al pago, no fue encontrado", pagoUnidadContrato.ReferenciaContrato.CuentaCliente.Id));

            CuentaClienteBO cuentaCliente = cuentaClientes.First();

            var mismaDireccion = cuentaCliente.Direcciones.Any(x => x.Id == pagoUnidadContrato.ReferenciaContrato.DireccionCliente.Id && x.Direccion == pagoUnidadContrato.ReferenciaContrato.DireccionCliente.Direccion.ToUpper());
            
            if (!mismaDireccion)
            {
                messagesAdvertencia.AppendLine(string.Format("La dirección del cliente {0} asociada al contrato y la de ORACLE son distintas. La dirección que se utilizará es la de ORACLE.", pagoUnidadContrato.ReferenciaContrato.DireccionCliente.Direccion.ToUpper()));
                if (messagesAdvertencia.Length > 0)
                    this.ShowInitError(false, messagesAdvertencia.ToString());
            }

            DireccionClienteBO direccion = cuentaCliente.Direcciones.First(x => x.Id == pagoUnidadContrato.ReferenciaContrato.DireccionCliente.Id);

            do
            {
                cuentaCliente.EliminarElementoDireccion(0);
            } while(cuentaCliente.Direcciones.Any());
            cuentaCliente.Agregar(direccion);

            return cuentaCliente;
        }

        /// <summary>
        /// Obtiene el tipo de crédito para un pago de unidad de contrato
        /// </summary>
        /// <param name="pagoUnidadContrato">Objeto de pago a evaluar</param>       
        /// <returns>Objeto de tipo de crédito</returns>
        private ETipoCredito ResolverTipoCredito(PagoUnidadContratoBO pagoUnidadContrato)
        {
            ETipoCredito result = ETipoCredito.REVOLVENTE;

            switch (pagoUnidadContrato.ReferenciaContrato.TipoContratoID.Value)
            {
                case ETipoContrato.FSL:
                    result = ETipoCredito.ACUERDO_DE_VENTAS;                   
                    break;

                case ETipoContrato.CM:
                    result = ETipoCredito.ACUERDO_DE_VENTAS;                  
                    break;

                case ETipoContrato.SD:
                    result = ETipoCredito.ACUERDO_DE_VENTAS;                  
                    break;

                case ETipoContrato.RD:
                    result = ETipoCredito.REVOLVENTE;
                    break;
            }

            return result;
        }

        /// <summary>
        /// Recupera el crédito de cliente asociada a un pago de contrato
        /// </summary>
        /// <param name="pagoUnidadContrato">Objeto de pago a evaluar</param>       
        /// <returns>Objeto de tipo CreditoClienteBO recuperado</returns>
        private CreditoClienteBO ResolverCreditoCliente(PagoUnidadContratoBO pagoUnidadContrato)
        {
            CreditoClienteBO result = null;

            CreditoClienteBO filter = new CreditoClienteBO
            {
                CuentaCliente = new CuentaClienteBO
                {
                    Id = pagoUnidadContrato.ReferenciaContrato.CuentaCliente.Id
                },
                Moneda = new MonedaBO
                {
                    Codigo = pagoUnidadContrato.Divisa.MonedaDestino.Codigo
                }
            };

            switch (pagoUnidadContrato.ReferenciaContrato.TipoContratoID.Value)
            {
                case ETipoContrato.FSL:
                    filter.TipoCredito = ETipoCredito.ACUERDO_DE_VENTAS;
                    filter.Referencia = pagoUnidadContrato.ReferenciaContrato.FolioContrato;
                    break;

                case ETipoContrato.CM:
                    filter.TipoCredito = ETipoCredito.ACUERDO_DE_VENTAS;
                    filter.Referencia = pagoUnidadContrato.ReferenciaContrato.FolioContrato;
                    break;

                case ETipoContrato.SD:
                    filter.TipoCredito = ETipoCredito.ACUERDO_DE_VENTAS;
                    filter.Referencia = pagoUnidadContrato.ReferenciaContrato.FolioContrato;
                    break;

                case ETipoContrato.RD:
                    filter.TipoCredito = ETipoCredito.REVOLVENTE;
                    break;
            }

            var creditosClientes = FacadeEFacturacionBR.ConsultarCreditoCliente(this.dctx, filter);
            if (creditosClientes.Count == 0)
                throw new Exception(String.Format("No se ha podido recuperar el crédito del cliente {0} con la moneda {1}, {2}", pagoUnidadContrato.ReferenciaContrato.CuentaCliente.Id, pagoUnidadContrato.Divisa.MonedaDestino.Codigo, filter.TipoCredito.ToString()));

            result = creditosClientes[0];

            return result;
        }

        /// <summary>
        /// Obtiene el tipo de divisa por default
        /// </summary>
        /// <returns>Objeto divisa configurada por default</returns>
        private TipoDivisaBO ResolverTipoDivisaPorDefault()
        {            
            return new TipoDivisaBO { NombreCorto = this.vista.TipoTasaCambiarioPorDefault, Nombre = this.vista.TipoTasaCambiarioPorDefault };    
        }

        /// <summary>
        /// Recupera la divisa por default
        /// </summary>
        /// <param name="monedaOrigen">Clave de la moneda origen a evaluar</param>
        /// <param name="monedaDestino">Clave de la moneda destino a evaluar</param>
        /// <returns>Objeto Divisa por default</returns>
        private DivisaBO ResolverDivisaPorDefault(String monedaOrigen, String monedaDestino)
        {
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
        private DivisaBO ResolverDivisa(PagoUnidadContratoBO pagoUnidadContrato)
        {
            return this.ResolverDivisa(pagoUnidadContrato.Divisa.MonedaOrigen.Codigo, pagoUnidadContrato.Divisa.MonedaDestino.Codigo);
        }

        /// <summary>
        /// Recupera la divisa correspondiente a un pago de contrato
        /// </summary>
        /// <param name="monedaOrigen">Clave de la moneda origen a evaluar</param>
        /// <param name="monedaDestino">Clave de la moneda destino a evaluar</param>
        /// <returns>Divisa encontrada, de lo contrario devolverá nulo</returns>
        private DivisaBO ResolverDivisa(String monedaOrigen, String monedaDestino)
        {
            DivisaBO divisa = null;
            List<DivisaBO> divisas = FacadeBR.ConsultarTipoCambio(this.dctx,
                new DivisaBO { MonedaOrigen = new MonedaBO { Codigo = monedaOrigen }, MonedaDestino = new MonedaBO { Codigo = monedaDestino }, FechaTipoCambio = DateTime.Today }
            );

            if (divisas.Count > 0)
            {
                divisa = divisas.OrderByDescending(x => x.FechaTipoCambio).FirstOrDefault(x => DateTime.Now >= x.FechaTipoCambio && DateTime.Now <= x.FechaTipoCambio.Value.AddDays(1));

                if (divisa.TipoDivisa == null || !String.Equals(divisa.TipoDivisa.NombreCorto, this.vista.TipoTasaCambiarioPorDefault, StringComparison.InvariantCultureIgnoreCase))
                    divisa.TipoDivisa = this.ResolverTipoDivisaPorDefault();
            }
            else
            {
                divisas = FacadeBR.ConsultarTipoCambio(this.dctx, new DivisaBO { MonedaOrigen = new MonedaBO { Codigo = monedaOrigen }, MonedaDestino = new MonedaBO { Codigo = monedaDestino } });
                if (divisas.Count > 0)
                {
                    divisa = divisas.OrderByDescending(x => x.FechaTipoCambio).FirstOrDefault(x => DateTime.Now >= x.FechaTipoCambio && DateTime.Now <= x.FechaTipoCambio.Value.AddDays(1));

                    if (divisa == null)
                    {
                        divisa = divisas.OrderByDescending(x => x.FechaTipoCambio).FirstOrDefault();
                        vista.MostrarMensaje("No existe un tipo de Cambio Configurado de " + monedaOrigen + " a " + monedaDestino + " para el día de Hoy, se usará el Tipo de cambio del día " + divisa.FechaTipoCambio.Value.ToShortDateString(), ETipoMensajeIU.ADVERTENCIA);
                    }
                    
                    if (divisa.TipoDivisa == null || !String.Equals(divisa.TipoDivisa.NombreCorto, this.vista.TipoTasaCambiarioPorDefault, StringComparison.InvariantCultureIgnoreCase))
                        divisa.TipoDivisa = this.ResolverTipoDivisaPorDefault();
                }
                else
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
        private void ShowInitError(bool exception, String message, params Object[] parameters)
        {
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
        public void AvanzarPagina()
        {
            int? paginaActual = this.vista.PaginaActual;
            if (paginaActual == null)
                throw new Exception("La página actual es nula.");
            if (paginaActual >= 5)
                throw new Exception("La página actual es mayor o igual a 5 y, por lo tanto, no se puede avanzar.");

            this.EstablecerOpcionesSegunPagina(paginaActual.Value + 1);
            this.EstablecerPagina(paginaActual.Value + 1);
        }

        /// <summary>
        /// Obtiene el paquete de facturación según un tipo de comprobante
        /// </summary>
        /// <param name="tipoContrato">Tipo de comporbante</param>
        /// <returns>Generador de paquetes de facturación</returns>
        private IGeneradorPaquetesFacturacionBR ObtenerGeneradorPaquetes(ETipoContrato tipoContrato, String monedaFacturacion)
        {
            IGeneradorPaquetesFacturacionBR generador = null;
            switch (tipoContrato)
            {
                case ETipoContrato.FSL:
                    generador = new GeneradorPaqueteFacturacionFSLBR();
                    break;

                case ETipoContrato.RD:
                    generador = new GeneradorPaqueteFacturacionRDBR();
                    break;

                case ETipoContrato.CM:
                    generador = new GeneradorPaqueteFacturacionManttoBR();
                    break;

                case ETipoContrato.SD:
                    generador = new GeneradorPaqueteFacturacionManttoBR();
                    break;
            }

            generador.MonedaFacturar = !String.IsNullOrEmpty(monedaFacturacion) ? monedaFacturacion : generador.MonedaPesos;

            return generador;
        }

        /// <summary>
        /// Acompleta la información de un pago
        /// </summary>
        /// <param name="pagoUnidadContrato">Pago a ser acompletado</param>
        private StringBuilder AcompletarPago(PagoUnidadContratoBO pagoUnidadContrato)
        {
            StringBuilder messagesAdvertencia = new StringBuilder();
            if (!pagoUnidadContrato.Activo.GetValueOrDefault())
                this.ShowInitError(true, "El pago del contrato {0} no se encuentra activo", pagoUnidadContrato.ReferenciaContrato.FolioContrato);
            
            var sucursales = FacadeBR.ConsultarSucursalCompleto(this.dctx, new SucursalBO { Id = pagoUnidadContrato.ReferenciaContrato.Sucursal.Id });
            if (sucursales.Count == 0)
                this.ShowInitError(true, "La sucursal {0} no fue encontrada", pagoUnidadContrato.ReferenciaContrato.Sucursal.Id);

            pagoUnidadContrato.ReferenciaContrato.Sucursal = sucursales[0];

            List<UnidadBO> unidades = this.unidadBR.Consultar(this.dctx, new UnidadBO { UnidadID = pagoUnidadContrato.Unidad.UnidadID });
            if (unidades.Count == 0)
                this.ShowInitError(true, "La unidad con el identificador {0} no fue encontrada", pagoUnidadContrato.Unidad.UnidadID);

            pagoUnidadContrato.Unidad = unidades[0];

            pagoUnidadContrato.ReferenciaContrato.CuentaCliente.UnidadOperativa = new UnidadOperativaBO { Id = pagoUnidadContrato.ReferenciaContrato.UnidadOperativa.Id };
            var cuentasClientes = this.cuentaClienteIdealeaseBR.ConsultarCompleto(this.dctx, pagoUnidadContrato.ReferenciaContrato.CuentaCliente);
            if (cuentasClientes.Count == 0)
                this.ShowInitError(true, "No se ha podido consultar al cliente {0}", pagoUnidadContrato.ReferenciaContrato.CuentaCliente.Id);

            var mismaDireccion = cuentasClientes.First().Direcciones.Any(x => x.Id == pagoUnidadContrato.ReferenciaContrato.DireccionCliente.Id && x.Direccion == pagoUnidadContrato.ReferenciaContrato.DireccionCliente.Direccion.ToUpper());
            if (!mismaDireccion)
            {
                messagesAdvertencia.AppendLine(String.Format("La dirección del cliente {0} asociada al contrato y la de ORACLE son distintas. La dirección que se utilizará es la de ORACLE.", pagoUnidadContrato.ReferenciaContrato.DireccionCliente.Direccion.ToUpper()));
            }

            DireccionClienteBO direccion = cuentasClientes.First().Direcciones.First(x => x.Id == pagoUnidadContrato.ReferenciaContrato.DireccionCliente.Id);

            CuentaClienteIdealeaseBO cuentaCliente = cuentasClientes.First();
            do
            {
                cuentaCliente.EliminarElementoDireccion(0);
            } while (cuentaCliente.Direcciones.Any());
            cuentaCliente.Agregar(direccion);

            pagoUnidadContrato.ReferenciaContrato.CuentaCliente = cuentaCliente;
            pagoUnidadContrato.ReferenciaContrato.DireccionCliente = direccion;

            return messagesAdvertencia;
        }
       
        /// <summary>
        /// Agrega todos los campos actuales de la vista a un objeto de transacción
        /// </summary>
        /// <param name="pagoUnidadContrato">Objeto de pago de unidad tomada como pivote</param>
        /// <param name="creditoCliente">Crédito del cliente asociada a la transacción</param>
        /// <param name="transaccion">Transacción a acompletar</param>
        private void AcompletarTransaccion(PagoUnidadContratoBO pagoUnidadContrato, CreditoClienteBO creditoCliente, TransaccionBO transaccion)
        {
            SeguridadBO seguridad = this.CrearObjetoSeguridad();
            AuditoriaBO auditoria = this.CrearObjetoAuditoria();

            this.ReajustarDetallesTransaccion(transaccion, this.vista.CostosAdicionalesView.CostosAdicionales, pagoUnidadContrato, creditoCliente);
                       
            if (transaccion.DatosAdicionales == null)
                transaccion.DatosAdicionales = new List<DatosAdicionalesFacturaBO>();

            transaccion.DatosAdicionales.AddRange(this.vista.InformacionAdicionalView.DatosAdicionales);

            transaccion.TransaccionId = pagoUnidadContrato.PagoID;
            transaccion.Fecha = DateTime.Now;
            transaccion.Observaciones = this.vista.Observaciones != null ?  this.vista.Observaciones.ToUpper() : null;
            transaccion.Auditoria = auditoria;

            DepartamentoBO departamento = this.departamentoFacturacionBR.Consultar(pagoUnidadContrato.ReferenciaContrato.TipoContratoID.Value, this.vista.ObtenerCarpetaRaiz());
            transaccion.Adscripcion = new AdscripcionBO
            {
                UnidadOperativa = new UnidadOperativaBO
                {
                    Id = this.vista.UnidadOperativaID
                },
                Sucursal = new SucursalBO
                {
                    Id = pagoUnidadContrato.ReferenciaContrato.Sucursal.Id
                },
                Departamento = departamento
            };

            transaccion.SistemaOrigen = this.vista.SistemaOrigen;
            transaccion.RFC = pagoUnidadContrato.ReferenciaContrato.CuentaCliente.Cliente.RFC;
            transaccion.RazonSocial = pagoUnidadContrato.ReferenciaContrato.CuentaCliente.Cliente.NombreCompleto;
            transaccion.Auditoria = auditoria;
            transaccion.Credito = this.vista.InformacionCabeceraView.FormaPago == "CRÉDITO"; //Valor de Configuraciones.FormaPago.XML

            ETipoTransaccion tipoTransaccion = null;
            switch (pagoUnidadContrato.ReferenciaContrato.TipoContratoID.GetValueOrDefault())
            {
                case ETipoContrato.CM:
                    tipoTransaccion = ETipoTransaccion.CONTRATO_MANTENIMIENTO;
                    break;

                case ETipoContrato.FSL:
                    tipoTransaccion = ETipoTransaccion.CONTRATO_FULLSERVICE;
                    break;

                case ETipoContrato.RD:
                    tipoTransaccion = ETipoTransaccion.CONTRATO_RENTA_DIARIA;
                    break;

                case ETipoContrato.SD:
                    tipoTransaccion = ETipoTransaccion.CONTRATO_SERVICIO_DEDICADO;
                    break;
            }

            transaccion.TasaIVA = pagoUnidadContrato.ReferenciaContrato.Sucursal.Impuesto.PorcentajeImpuesto;           
            transaccion.Referencia = this.vista.InformacionCabeceraView.NumeroReferencia;
            transaccion.TipoTransaccion = tipoTransaccion;
            
            //SC0028, Para la divisa de la transacción se le envia la moneda de facturación que es la moneda destino del Pago
            //SC0034, Se envia la moneda de destino vacio
            DivisaBO divisa = this.ResolverDivisa(pagoUnidadContrato);
            
            transaccion.Divisa = divisa;
            transaccion.Divisa.MonedaOrigen = transaccion.Divisa.MonedaDestino;
            transaccion.Divisa.MonedaDestino = null;

            // Se agrega el Tipo de cambio en base al codigo de moneda de la empresa
            DivisaBO divisaEmpresa = this.ResolverDivisa(transaccion.Divisa.MonedaOrigen.Codigo, this.vista.CodigoMonedaEmpresa);
            transaccion.Divisa.TipoCambio = divisaEmpresa.TipoCambio;

            transaccion.DireccionCliente = pagoUnidadContrato.ReferenciaContrato.DireccionCliente.Direccion;            
            transaccion.CuentaCliente = this.ResolverCuentaCliente(pagoUnidadContrato);
            transaccion.CuentaCliente.UnidadOperativa = new UnidadOperativaBO { Id = pagoUnidadContrato.ReferenciaContrato.UnidadOperativa.Id };

            if (creditoCliente != null)
            {
                transaccion.DiasCredito = creditoCliente.DiasCredito;
                transaccion.DiasFactura = creditoCliente.DiasFactura;
            }
        }

        /// <summary>
        /// Genera un objeto de transacción con todos los datos a facturar
        /// </summary>
        /// <param name="pagoUnidadContrato">Objeto de pago de unidad tomada como pivote</param>       
        /// <returns>Objeto de tipo transaccion</returns>
        private TransaccionBO ArmarTransaccion(PagoUnidadContratoBO pagoUnidadContrato)
        {            
            SeguridadBO seguridad = this.CrearObjetoSeguridad();
            AuditoriaBO auditoria = this.CrearObjetoAuditoria();

            IGeneradorPaquetesFacturacionBR generador = this.ObtenerGeneradorPaquetes(pagoUnidadContrato.ReferenciaContrato.TipoContratoID.Value, pagoUnidadContrato.Divisa.MonedaDestino.Codigo);
            TransaccionBO transaccion = generador.Procesar(this.dctx, pagoUnidadContrato, seguridad);
            
            return transaccion;
        }

        /// <summary>
        /// Genera un clone de un objeto de transaccion
        /// </summary>
        /// <param name="transaccion">Objeto de transaccion original</param>
        /// <returns>Objeto de transacción clonado</returns>
        private TransaccionBO ClonarTransaccion(TransaccionBO transaccion)
        {
            TransaccionBO clone = transaccion.Clonar();

            IEnumerable<DetalleTransaccionBO> detallesTransaccion = transaccion.GetChildren().OfType<DetalleTransaccionBO>();
            foreach (DetalleTransaccionBO detalle in detallesTransaccion)
            {
                DetalleTransaccionBO nDetalle = new DetalleTransaccionBO();

                PropertyInfo[] properties = typeof(DetalleTransaccionBO).GetProperties(BindingFlags.Instance | BindingFlags.Public);
                foreach (PropertyInfo property in properties)
                {
                    if (!property.CanRead || !property.CanWrite)
                        continue;

                    Object value = property.GetValue(detalle, null);
                    property.SetValue(nDetalle, value, null);
                }

                clone.Add(nDetalle);
            }

            if (transaccion.DatosAdicionales != null)
            {
                clone.DatosAdicionales = new List<DatosAdicionalesFacturaBO>();
                foreach (DatosAdicionalesFacturaBO adicional in transaccion.DatosAdicionales)
                {
                    DatosAdicionalesFacturaBO nAdicional = new DatosAdicionalesFacturaBO();

                    PropertyInfo[] properties = typeof(DatosAdicionalesFacturaBO).GetProperties(BindingFlags.Instance | BindingFlags.Public);
                    foreach (PropertyInfo property in properties)
                    {
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
        /// <param name="pagoUnidadContrato">pago a bloquear</param>
        /// <returns>Objeto para generar el bloqueo</returns>
        private static Object ResolverObjetoBloqueoPagoUnidadContrato(PagoUnidadContratoBO pagoUnidadContrato)
        {
            Object syncObject = null;
            if (pagoUnidadContrato == null)
                throw new Exception(ConfigurarFacturacionPRE.nombreClase + ".ResolverObjetoBloqueoPagoUnidadContrato: El pago no puede ser nulo");

            int pagoID = pagoUnidadContrato.PagoID.GetValueOrDefault();
            lock (ConfigurarFacturacionPRE.syncFacturacion)
            {
                if (ConfigurarFacturacionPRE.pagosUnidadContratoList == null)
                    ConfigurarFacturacionPRE.pagosUnidadContratoList = new Dictionary<int, Object>();

                if (!ConfigurarFacturacionPRE.pagosUnidadContratoList.TryGetValue(pagoID, out syncObject))
                {
                    syncObject = pagoID.GetHashCode();
                    ConfigurarFacturacionPRE.pagosUnidadContratoList.Add(pagoID, syncObject);
                }                
            }

            return syncObject;
        }

        /// <summary>
        /// Establece los datos iniciales de la prefactura
        /// </summary>
        private void EstablecerInformacionInicial()
        {
            this.vista.PermitirTerminar(false);
            this.vista.PertimirCapturar(false);

            if (!ConfiguracionFacturacionBR.IniciarConsulta(this.dctx))
                this.ShowInitError(true, "No se ha podido iniciar la consulta.");

            #region Monedas
            //SC0034
            List<MonedaBO> lstMonedas = FacadeBR.ConsultarMoneda(this.dctx, new MonedaBO() { Activo = true });
            this.vista.CostosAdicionalesView.EstablecerOpcionesMoneda(lstMonedas.ToDictionary(p => p.Codigo, p => p.Nombre));
            #endregion

            StringBuilder messagesAdvertencia = new StringBuilder(), messagesAdvertenciaDireccion = new StringBuilder(), messagesCredito = new StringBuilder();
            StringBuilder mensajeAviso = new StringBuilder();

            if (!this.vista.PagoUnidadContratoID.HasValue)
                this.ShowInitError(true, "No se ha enviado ningún identificador de contrato para procesar");

            SeguridadBO seguridad = this.CrearObjetoSeguridad();
            AuditoriaBO auditoria = this.CrearObjetoAuditoria();

            //SC0026, Cambio de objeto PagoUnidadContratoBOF para búsquedas
            var pagoUnidadContratoLs = this.ConsultarPagos(this.vista.PagoUnidadContratoID, null, null, this.vista.TipoPagoUnidadContrato, true);
            
            if (pagoUnidadContratoLs.Count == 0)
                this.ShowInitError(true, "El pago {0} no fue encontrado", this.vista.PagoUnidadContratoID);

            PagoUnidadContratoBO pagoUnidadContrato = pagoUnidadContratoLs[0];
            Object lockValue = ConfigurarFacturacionPRE.ResolverObjetoBloqueoPagoUnidadContrato(pagoUnidadContrato);
            try
            {
                Monitor.Enter(lockValue);
              
                messagesAdvertenciaDireccion = this.AcompletarPago(pagoUnidadContrato);

                #region SC0028, Para la visualización de la Divisa se usa la moneda origen del contrato y la moneda destino que por default es MXN
                String monedaAFacturar = pagoUnidadContrato.Divisa.MonedaDestino != null && !String.IsNullOrEmpty(pagoUnidadContrato.Divisa.MonedaDestino.Codigo) ? pagoUnidadContrato.Divisa.MonedaDestino.Codigo : null;
                IGeneradorPaquetesFacturacionBR generador = this.ObtenerGeneradorPaquetes(pagoUnidadContrato.ReferenciaContrato.TipoContratoID.Value, monedaAFacturar);

                if (monedaAFacturar == null)
                    pagoUnidadContrato.Divisa.MonedaDestino = new MonedaBO { Codigo = pagoUnidadContrato.Divisa.MonedaOrigen.Codigo };

                pagoUnidadContrato.Divisa = this.ResolverDivisa(pagoUnidadContrato);
                #endregion

                DepartamentoBO departamento = this.departamentoFacturacionBR.Consultar(pagoUnidadContrato.ReferenciaContrato.TipoContratoID.Value, this.vista.ObtenerCarpetaRaiz());

                bool esCredito = true;
                string _formaPago = this.vista.FormaPagoPorDefault;
                #region Consultar la Condición de Pago del Contrato
                if (pagoUnidadContrato.ReferenciaContrato.TipoContratoID.GetValueOrDefault() == ETipoContrato.RD) {
                    ContratoRDBO contratoRD = new ContratoRDBR()
                        .Consultar(this.dctx, new ContratoRDBO() { ContratoID = pagoUnidadContrato.ReferenciaContrato.ReferenciaContratoID })
                        .FirstOrDefault();

                    if (contratoRD != null && contratoRD.FormaPago != null) {
                        esCredito = (contratoRD.FormaPago == BPMO.SDNI.Contratos.RD.BO.EFormaPago.CREDITO);
                        _formaPago = esCredito ? "CRÉDITO" : "CONTADO"; //Valores de Configuraciones.FormaPago.XML
                    }
                }
                #endregion
                                
                CreditoClienteBO creditoCliente = null;
                if (esCredito)
                {
                    try
                    {
                        creditoCliente = this.ResolverCreditoCliente(pagoUnidadContrato);
                    }
                    catch
                    {}
                }

                this.vista.HerramientasPrefacturaView.NumeroContrato = pagoUnidadContrato.ReferenciaContrato.FolioContrato;
                this.vista.HerramientasPrefacturaView.NumeroPago = pagoUnidadContrato.NumeroPago;

                this.vista.RFCCliente = pagoUnidadContrato.ReferenciaContrato.CuentaCliente.Cliente.RFC;
                this.vista.NombreCliente = pagoUnidadContrato.ReferenciaContrato.CuentaCliente.Cliente.NombreCompleto;
                this.vista.NumeroCliente = pagoUnidadContrato.ReferenciaContrato.CuentaCliente.Numero;
                this.vista.DireccionCliente = pagoUnidadContrato.ReferenciaContrato.DireccionCliente.Direccion;
                this.vista.FechaInicio = pagoUnidadContrato.ReferenciaContrato.FechaInicio;
                this.vista.UsuarioGenerador = this.vista.UsuarioNombre;
                this.vista.Observaciones = pagoUnidadContrato.Observaciones;

                this.vista.InformacionCabeceraView.SucursalID = pagoUnidadContrato.ReferenciaContrato.Sucursal.Id;
                this.vista.InformacionCabeceraView.SucursalNombre = pagoUnidadContrato.ReferenciaContrato.Sucursal.Nombre;
                this.vista.InformacionCabeceraView.SistemaOrigen = this.vista.SistemaOrigen;
                this.vista.InformacionCabeceraView.TipoTransaccion = pagoUnidadContrato.ReferenciaContrato.TipoContratoID;
                this.vista.InformacionCabeceraView.NumeroReferencia = String.Format("{0}-{1}", pagoUnidadContrato.ReferenciaContrato.FolioContrato, pagoUnidadContrato.NumeroPago);
                this.vista.InformacionCabeceraView.CodigoMoneda = pagoUnidadContrato.Divisa.MonedaDestino.Codigo;
                this.vista.InformacionCabeceraView.FormaPago = _formaPago;
                #region SC0035 Se coloca el tipo de cambio a la moneda de la empresa para fines solamente de visualización
                this.vista.InformacionCabeceraView.TipoCambio = this.ResolverDivisa(pagoUnidadContrato.Divisa.MonedaDestino.Codigo, this.vista.CodigoMonedaEmpresa).TipoCambio;
                #endregion
                this.vista.InformacionCabeceraView.TipoTasaCambiario = this.vista.TipoTasaCambiarioPorDefault;
                this.vista.InformacionCabeceraView.Departamento = departamento.Nombre;
                this.vista.InformacionCabeceraView.BanderaCores = false;

                this.vista.CostosAdicionalesView.CodigoMoneda = pagoUnidadContrato.Divisa.MonedaDestino.Codigo;
                List<DetalleTransaccionBO> costosAdicionales = new List<DetalleTransaccionBO>();
                if (pagoUnidadContrato.LineasCostoAdicional != null)
                {
                    foreach (var adicional in pagoUnidadContrato.LineasCostoAdicional)
                    {
                        DetalleTransaccionBO detalle = new DetalleTransaccionBO();
                        detalle.ProductoServicio = new ProductoServicioBO();
                        detalle.Id = adicional.LineaCostoAdicionalID;
                        detalle.AplicaIVA = true; 
                        detalle.Articulo = new ArticuloBO { Nombre = adicional.Descripcion };
                        detalle.PrecioUnitario = adicional.PrecioUnitario;
                        detalle.CostoUnitario = generador.Costo;
                        detalle.EstablecerTipoRenglon(adicional.TipoRenglon.GetValueOrDefault());
                        detalle.Cantidad = generador.Cantidad;
                        detalle.UnidadMedida = generador.UnidadMedida;
                        #region Producto o Servicio (SAT)
                        if (adicional.ProductoServicio != null) {
                            detalle.ProductoServicio.Id = adicional.ProductoServicio.Id;
                            detalle.ProductoServicio.NombreCorto = adicional.ProductoServicio.NombreCorto;
                        }
                        #endregion
                        detalle.Activo = true;

                        this.vista.CostosAdicionalesView.CostosAdicionales.Add(detalle);
                        costosAdicionales.Add(detalle);
                    }

                    this.vista.CostosAdicionalesView.MostrarListaCostosAdicionales();
                }

                if (pagoUnidadContrato.LineasDatoAdicional != null)
                {
                    foreach (var adicional in pagoUnidadContrato.LineasDatoAdicional)
                    {
                        DatosAdicionalesFacturaBO detalle = new DatosAdicionalesFacturaBO();
                        detalle.Etiqueta = adicional.Etiqueta;
                        detalle.Valor = adicional.Valor;
                        this.vista.InformacionAdicionalView.DatosAdicionales.Add(detalle);
                    }

                    this.vista.InformacionAdicionalView.MostrarListaDatosAdicionales();
                }

                //Se recalcula el Cargo Fijo de acuerdo a los INPC
                INPCPreFacturaBR inpcPreFacturaBr = new INPCPreFacturaBR();
                if (pagoUnidadContrato.Divisa.MonedaOrigen.Codigo == MonedaPesos)
                    mensajeAviso.AppendLine(inpcPreFacturaBr.RevisarInpcPago(this.dctx, pagoUnidadContrato, seguridad));

                var transaccionOriginal = this.ArmarTransaccion(pagoUnidadContrato);
                this.vista.TransaccionActual = transaccionOriginal;

                TransaccionBO transaccion = this.ClonarTransaccion(this.vista.TransaccionActual);

                string observacionesSeguro = CalcularCobraSeguro(pagoUnidadContrato, generador);
                if (!String.IsNullOrEmpty(observacionesSeguro)) mensajeAviso.AppendLine(observacionesSeguro);

                List<DetalleTransaccionBO> lineasContrato = this.ReajustarDetallesTransaccion(transaccion, costosAdicionales, pagoUnidadContrato, creditoCliente);

                this.vista.LineasFacturaView.MostrarLineasContrato(lineasContrato);
                this.vista.LineasFacturaView.MostrarTotales(transaccion);

                pagoUnidadContrato.TotalFactura = transaccion.TotalFactura;
                pagoUnidadContrato.IvaFacturado = transaccion.Impuestos;

                if (creditoCliente != null)
                {
                    this.vista.InformacionCabeceraView.DiasCredito = creditoCliente.DiasCredito;
                    this.vista.InformacionCabeceraView.DiasFactura = creditoCliente.DiasFactura;
                    this.vista.InformacionCabeceraView.LimiteCredito = creditoCliente.LimiteCredito;
                    this.vista.InformacionCabeceraView.CreditoDisponible = FacadeEFacturacionBR.ConsultarCreditoDisponible(this.dctx, creditoCliente);
                }

                this.vista.PagoActual = pagoUnidadContrato;

                if (pagoUnidadContrato.EnviadoFacturacion.GetValueOrDefault())
                    messagesAdvertencia.AppendLine(String.Format("El pago del contrato {0} ya se encuentra enviado a facturar", pagoUnidadContrato.ReferenciaContrato.FolioContrato));
                else
                {
                    //SC0034
                    if (!this.EsPagoValido(pagoUnidadContrato))
                        messagesAdvertencia.AppendLine(String.Format("El pago del contrato {0} no puede ser enviado a facturar porque el pago anterior aun no ha sido enviado a facturar", pagoUnidadContrato.ReferenciaContrato.FolioContrato));

                    if (pagoUnidadContrato.BloqueadoCredito.GetValueOrDefault())
                        messagesCredito.AppendLine(String.Format("El pago del contrato {0} se encuentra bloqueado por falta de crédito", pagoUnidadContrato.ReferenciaContrato.ReferenciaContratoID));
                }

                #region SC0021
                // Valida la Direccion Facturable del Cliente
                var clienteBR = new CuentaClienteIdealeaseBR();
                var cuentaCliente = new CuentaClienteIdealeaseBO
                {
                    UnidadOperativa = pagoUnidadContrato.ReferenciaContrato.UnidadOperativa,
                    Cliente = pagoUnidadContrato.ReferenciaContrato.CuentaCliente.Cliente,
                    Id = pagoUnidadContrato.ReferenciaContrato.CuentaCliente.Id
                };
                var direcciones = FacadeBR.ConsultarDirecionesCliente(dctx, cuentaCliente);
                var facturable = direcciones.Any(x => x.Id == pagoUnidadContrato.ReferenciaContrato.DireccionCliente.Id);

                if (!facturable) {
                    messagesAdvertencia.AppendLine(
                        string.Format("La Dirección del Cliente en el Contrato {0} No es Facturable.",
                            pagoUnidadContrato.ReferenciaContrato.FolioContrato));
                }
                #endregion

                if (messagesAdvertencia.Length > 0)
                {
                    messagesAdvertencia.AppendLine(messagesAdvertenciaDireccion.ToString());
                    messagesAdvertencia.AppendLine(messagesCredito.ToString());
                    ShowInitError(false, messagesAdvertencia.ToString());
                } else if (messagesCredito.Length > 0) {
                    ShowInitError(false, messagesCredito.ToString());
                    vista.PertimirCapturar(true);
                    vista.PermitirTerminar(false);
                }
                else if (messagesAdvertenciaDireccion.Length >= 0)
                {
                    if (messagesAdvertenciaDireccion.Length > 0 ) ShowInitError(false, messagesAdvertenciaDireccion.ToString());
                    vista.PermitirTerminar(true);
                    vista.PertimirCapturar(true);

                    if(!String.IsNullOrEmpty(mensajeAviso.ToString().Trim()) || !String.IsNullOrWhiteSpace(mensajeAviso.ToString().Trim()))
                        ShowInitError(false, mensajeAviso.ToString());
                }
            }
            finally {
                ConfiguracionFacturacionBR.TerminarConsulta(this.dctx);
                Monitor.Exit(lockValue);
            }
        }        

        /// <summary>
        /// Regenera la transacción en curso refrescando los conceptos de facturación
        /// </summary>
        private void RegenerarTransaccion()
        {
            PagoUnidadContratoBO pagoUnidadContrato = this.vista.PagoActual;
            IList<DetalleTransaccionBO> costosAdicionales = this.vista.CostosAdicionalesView.CostosAdicionales;

            StringBuilder mensajeAviso = new StringBuilder();

            //Se recalcula el Cargo Fijo de acuerdo a los INPC
            INPCPreFacturaBR inpcPreFacturaBr = new INPCPreFacturaBR();
            if(pagoUnidadContrato.Divisa.MonedaOrigen.Codigo == MonedaPesos)
                mensajeAviso.AppendLine(inpcPreFacturaBr.RevisarInpcPago(this.dctx, pagoUnidadContrato, this.CrearObjetoSeguridad()));

            this.vista.TransaccionActual = this.ArmarTransaccion(pagoUnidadContrato);
           
            #region RI0017 - Validación del crédito del cliente
            bool esCredito = this.vista.InformacionCabeceraView.FormaPago == this.vista.FormaPagoPorDefault;
            CreditoClienteBO creditoCliente = null;

            if (esCredito)
                try
                {
                    creditoCliente = this.ResolverCreditoCliente(pagoUnidadContrato);
                }
                catch
                {
                    creditoCliente = null;
                }
            #endregion

            TransaccionBO transaccion = this.ClonarTransaccion(this.vista.TransaccionActual);
            List<DetalleTransaccionBO> lineasContrato = this.ReajustarDetallesTransaccion(transaccion, costosAdicionales, pagoUnidadContrato, creditoCliente);

            this.vista.LineasFacturaView.MostrarLineasContrato(lineasContrato);
            this.vista.LineasFacturaView.MostrarTotales(transaccion);

            pagoUnidadContrato.TotalFactura = transaccion.TotalFactura;
            pagoUnidadContrato.IvaFacturado = transaccion.Impuestos;

            if(!String.IsNullOrEmpty(mensajeAviso.ToString().Trim()) || !String.IsNullOrWhiteSpace(mensajeAviso.ToString().Trim()))
                ShowInitError(false, mensajeAviso.ToString());
        }

        /// <summary>
        /// Valida la moneda que se esta seleccionando
        /// </summary>
        public void ValidarMonedaFacturacion(bool? desdeInterfaz = false)
        {
            PagoUnidadContratoBO pagoUnidadContrato = this.vista.PagoActual;

            pagoUnidadContrato.Divisa.MonedaDestino = this.vista.CostosAdicionalesView.CodigoMoneda != "-1"
                                                            ? new MonedaBO { Codigo = this.vista.CostosAdicionalesView.CodigoMoneda }
                                                            : this.vista.PagoActual.Divisa.MonedaOrigen;

            pagoUnidadContrato.Divisa = this.ResolverDivisa(this.vista.PagoActual);

            this.RegenerarTransaccion();
            #region SC0035 Se coloca el tipo de cambio a la moneda de la empresa para fines solamente de visualización
            this.vista.InformacionCabeceraView.TipoCambio = this.ResolverDivisa(pagoUnidadContrato.Divisa.MonedaDestino.Codigo, this.vista.CodigoMonedaEmpresa).TipoCambio;
            #endregion
            this.vista.InformacionCabeceraView.CodigoMoneda = pagoUnidadContrato.Divisa.MonedaDestino.Codigo;
            this.vista.CostosAdicionalesView.CodigoMoneda = pagoUnidadContrato.Divisa.MonedaDestino.Codigo;

            this.ValidarFormaPago(desdeInterfaz);
        }

        /// <summary>
        /// Valida la forma de pago que se esta seleccionado
        /// </summary>
        public void ValidarFormaPago(bool? validacionUI = false)
        {
            vista.InformacionCabeceraView.DiasCredito = null;
            vista.InformacionCabeceraView.DiasFactura = null;
            vista.InformacionCabeceraView.LimiteCredito = null;
            vista.InformacionCabeceraView.CreditoDisponible = null;

            PagoUnidadContratoBO pagoUnidadContrato = this.vista.PagoActual;
            AuditoriaBO auditoria = this.CrearObjetoAuditoria();
            SeguridadBO seguridad = this.CrearObjetoSeguridad();

            bool esCredito = this.vista.InformacionCabeceraView.FormaPago == "CRÉDITO";

            if (esCredito) {
                CreditoClienteBO creditoCliente = null;

                try {
                    creditoCliente = this.ResolverCreditoCliente(pagoUnidadContrato);
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
                    string _moneda = pagoUnidadContrato.Divisa.MonedaDestino.Codigo;
                    string tipoCredito = this.ResolverTipoCredito(pagoUnidadContrato).ToString();
                    string errorMessage = string.Format("El cliente no cuenta con crédito disponible suficiente o el crédito se encuentra inactivo: {0}", tipoCredito);

                    if (validacionUI != null && validacionUI == true) {
                        errorMessage = string.Format("El cliente no cuenta con crédito disponible suficiente en la moneda seleccionada ({0})" 
                            + " o el crédito se encuentra inactivo: {1}.  ¿Mover el Pago al Visor de Pagos NO FACTURADOS?", _moneda, tipoCredito);
                        this.vista.MostrarMensaje(errorMessage, ETipoMensajeIU.CONFIRMACION);
                    } else {
                        this.vista.MostrarMensaje(errorMessage, ETipoMensajeIU.ADVERTENCIA);
                        MoverPagoFaltaCredito(errorMessage, auditoria, pagoUnidadContrato, seguridad);
                    }
                    this.vista.PermitirTerminar(false);
                } else
                    this.vista.PermitirTerminar(true);
            } else {
                this.vista.PermitirCancelar(true);
                this.vista.PermitirTerminar(true);
            }
        }

        /// <summary>
        /// Determina si existe crédito disponible para una transacción de facturación
        /// </summary>
        /// <param name="transaccion">Objeto que contiene la transacción a aplicar</param>
        /// <param name="creditoCliente">Objeto que contiene los datos del crédito del cliente</param>
        /// <returns>Devuelve true si hay crédito disponible, de lo contrario devolverá false</returns>
        private bool HayCreditoDisponible(TransaccionBO transaccion, CreditoClienteBO creditoCliente)
        {
            if (creditoCliente == null || !creditoCliente.Activo.GetValueOrDefault())
                return false;

            decimal creditoDisponible = FacadeEFacturacionBR.ConsultarCreditoDisponible(this.dctx, creditoCliente);
            return (creditoDisponible >= this.vista.LineasFacturaView.TotalFactura.GetValueOrDefault());
        }

        /// <summary>
        /// Ejecuta el proceso de registrar la factura en curso
        /// </summary>
        public void RegistrarFacturacion()
        {
            Guid firma = Guid.NewGuid();
            SeguridadBO seguridad = this.CrearObjetoSeguridad();
            AuditoriaBO auditoria = this.CrearObjetoAuditoria();
            bool esCredito = this.vista.InformacionCabeceraView.FormaPago == "CRÉDITO"; //Valor de Configuraciones.FormaPago.XML

            PagoUnidadContratoBO pagoUnidadContrato = this.vista.PagoActual;
            ConfiguracionFacturacionBR.IniciarConsulta(this.dctx);

            Object lockValue = ConfigurarFacturacionPRE.ResolverObjetoBloqueoPagoUnidadContrato(pagoUnidadContrato);
            try
            {
                Monitor.Enter(lockValue);

                //SC0026, Cambio de objeto PagoUnidadContratoBOF para búsquedas
                var ls = this.ConsultarPagos(pagoUnidadContrato.PagoID, null, null, this.vista.TipoPagoUnidadContrato, true);
                PagoUnidadContratoBO previous = ls[0];

                StringBuilder messagesAdvertencia = new StringBuilder();
                if (previous.EnviadoFacturacion.GetValueOrDefault())
                    messagesAdvertencia.AppendLine(String.Format("El pago del contrato {0} ya se encuentra enviado a facturar", pagoUnidadContrato.ReferenciaContrato.FolioContrato));
                else
                {                    
                    //SC0034
                    if (!this.EsPagoValido(previous))
                        messagesAdvertencia.AppendLine(String.Format("El pago del contrato {0} no puede ser enviado a facturar porque el pago anterior aun no ha sido enviado a facturar", pagoUnidadContrato.ReferenciaContrato.FolioContrato));
                    if (previous.BloqueadoCredito.GetValueOrDefault())
                        messagesAdvertencia.AppendLine(String.Format("El pago del contrato {0} se encuentra bloqueado por falta de crédito", pagoUnidadContrato.ReferenciaContrato.ReferenciaContratoID));
                }

                if (messagesAdvertencia.Length > 0)
                    throw new Exception("Se han detectado la(s) siguiente(s) inconsistenci(as). verifique que la factura no haya sido generada por otro usuario desde otra terminal. \n" + messagesAdvertencia.ToString());

                pagoUnidadContrato.Divisa.MonedaDestino = this.vista.CostosAdicionalesView.CodigoMoneda != "-1"
                                                ? new MonedaBO { Codigo = this.vista.CostosAdicionalesView.CodigoMoneda }
                                                : pagoUnidadContrato.Divisa.MonedaOrigen;
                
                TransaccionBO transaccion = this.ClonarTransaccion(this.vista.TransaccionActual);

                CreditoClienteBO creditoCliente = null;
                if (esCredito)
                    creditoCliente = this.ResolverCreditoCliente(pagoUnidadContrato);

                this.AcompletarTransaccion(pagoUnidadContrato, creditoCliente, transaccion);

                #region SC0037 Validacion de los montos de la transaccion

                Decimal? subtotalTransaccion = 0M;
                Decimal? subtotalImpuestos = 0M;

                foreach (DetalleTransaccionBO detalle in transaccion.GetChildren())
                {
                    subtotalTransaccion += detalle.PrecioUnitario;
                    subtotalImpuestos += detalle.ImpuestoUnitario;
                }
                if(subtotalTransaccion + subtotalImpuestos != transaccion.TotalFactura)
                    throw new Exception("La suma de los precios de las lineas no coincide con el monto Total de la Factura.");

                if((subtotalTransaccion * (pagoUnidadContrato.ReferenciaContrato.Sucursal.Impuesto.PorcentajeImpuesto / 100M) - transaccion.Impuestos) >= 0.01M)
                {
                    while ((subtotalTransaccion*(pagoUnidadContrato.ReferenciaContrato.Sucursal.Impuesto.PorcentajeImpuesto/100M) - transaccion.Impuestos) >= 0.01M)
                    {
                        transaccion.Impuestos += 0.01M;
                        var detalle = transaccion.GetChildren().OfType<DetalleTransaccionBO>().FirstOrDefault();
                        if(detalle != null)
                        {
                            detalle.ImpuestoUnitario += 0.01M;
                        }
                    }
                }

                #endregion

                ReservaCreditoBO reservaCredito = null;

                if (esCredito)
                    reservaCredito = new ReservaCreditoBO
                     {
                         TipoTransaccion = transaccion.TipoTransaccion,
                         TransaccionId = transaccion.TransaccionId,
                         Fecha = transaccion.Fecha,
                         CuentaCliente = transaccion.CuentaCliente,
                         Sucursal = pagoUnidadContrato.ReferenciaContrato.Sucursal,
                         Moneda = pagoUnidadContrato.Divisa.MonedaDestino,
                         Importe = transaccion.TotalFactura,
                         Auditoria = auditoria
                     };

                pagoUnidadContrato.LineasDatoAdicional = new List<LineaDatoAdicionalBO>();
                foreach (var adicional in transaccion.DatosAdicionales)
                    pagoUnidadContrato.LineasDatoAdicional.Add(new LineaDatoAdicionalBO { Etiqueta = adicional.Etiqueta, Valor = adicional.Valor });

                pagoUnidadContrato.LineasCostoAdicional = new List<LineaCostoAdicionalBO>();
                foreach (var adicional in this.vista.CostosAdicionalesView.CostosAdicionales)
                    pagoUnidadContrato.LineasCostoAdicional.Add(new LineaCostoAdicionalBO {
                        Descripcion = adicional.Articulo.Nombre,
                        PrecioUnitario = adicional.PrecioUnitario.GetValueOrDefault(),
                        TipoRenglon = adicional.TipoRenglon.ToInt32(CultureInfo.CurrentCulture),
                        ProductoServicio = new ProductoServicioBO() { Id = adicional.ProductoServicio.Id, NombreCorto = adicional.ProductoServicio.NombreCorto }
                    });

                pagoUnidadContrato.TotalFactura = transaccion.TotalFactura;
                pagoUnidadContrato.IvaFacturado = transaccion.Impuestos;
                pagoUnidadContrato.Observaciones = this.vista.Observaciones;
                pagoUnidadContrato.BloqueadoCredito = false;
                pagoUnidadContrato.Auditoria.FUA = DateTime.Now;
                pagoUnidadContrato.Auditoria.UUA = seguridad.Usuario.Id;

                if (pagoUnidadContrato.TotalFactura - pagoUnidadContrato.IvaFacturado > 0)
                {
                    if (esCredito && !this.HayCreditoDisponible(transaccion, creditoCliente))
                    {
                        String tipoCredito = this.ResolverTipoCredito(pagoUnidadContrato).ToString();
                        String errorMessage = String.Format("El cliente no cuenta con crédito disponible suficiente o el crédito se encuentra inactivo: {0}", tipoCredito);
                        pagoUnidadContrato.BloqueadoCredito = true;
                        this.vista.PermitirTerminar(false);
                        this.vista.MostrarMensaje(errorMessage, ETipoMensajeIU.ADVERTENCIA);

                        LogFacturacionBO log = new LogFacturacionBO();
                        log.Auditoria = auditoria;
                        log.MensajeError = errorMessage;
                        log.NumeroReferencia = !String.IsNullOrEmpty(pagoUnidadContrato.Unidad.NumeroSerie) ? pagoUnidadContrato.Unidad.NumeroSerie : pagoUnidadContrato.Unidad.NumeroEconomico;
                        log.Pago = pagoUnidadContrato;
                        log.ReferenciaContrato = pagoUnidadContrato.ReferenciaContrato;
                        this.logFacturacionBR.Insertar(this.dctx, log, seguridad);
                    }

                    ConfiguracionFacturacionBR.TerminarConsulta(this.dctx);

                    #region Conexion a BD
                    //Es necesario redireccinar antes de invocar la transacción para que funcione correctamente.
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
                    this.ActualizarPagos(pagoUnidadContrato, previous, seguridad, this.vista.TipoPagoUnidadContrato);

                    bool sended = false;
                    if (!pagoUnidadContrato.BloqueadoCredito.GetValueOrDefault())
                    {
                        FacadeEFacturacionBR.RegistrarTransaccionCompleto(this.dctx, reservaCredito, transaccion, seguridad);

                        pagoUnidadContrato.EnviadoFacturacion = true;
                        pagoUnidadContrato.FechaEnvioFacturacion = transaccion.Fecha;
                        this.ActualizarPagos(pagoUnidadContrato, previous, seguridad, this.vista.TipoPagoUnidadContrato);
                        sended = true;
                    }

                    this.dctx.SetCurrentProvider("LIDER");
                    this.dctx.CommitTransaction(firma);

                    if (sended)
                        this.vista.MostrarMensaje("El envío de la factura se ha realizado satisfactoriamente", ETipoMensajeIU.EXITO);

                    this.vista.PermitirTerminar(false);
                    this.vista.PermitirCancelar(false);
                }
                else
                {
                    pagoUnidadContrato.FacturaEnCeros = true;
                    pagoUnidadContrato.EnviadoFacturacion = true;
                    this.ActualizarPagos(pagoUnidadContrato, previous, seguridad, this.vista.TipoPagoUnidadContrato);

                    this.vista.MostrarMensaje("El monto de la factura se encuentra en Ceros", ETipoMensajeIU.ADVERTENCIA);
                    this.vista.PermitirTerminar(false);
                    this.vista.PermitirCancelar(false);
                }
            }
            catch (Exception ex)
            {
                this.dctx.SetCurrentProvider("LIDER");
                this.dctx.RollbackTransaction(firma);

                try
                {
                    LogFacturacionBO log = new LogFacturacionBO();
                    log.Auditoria = auditoria;
                    log.MensajeError = ex.GetBaseException().Message;
                    log.NumeroReferencia = !String.IsNullOrEmpty(pagoUnidadContrato.Unidad.NumeroSerie) ? pagoUnidadContrato.Unidad.NumeroSerie : pagoUnidadContrato.Unidad.NumeroEconomico;
                    log.Pago = pagoUnidadContrato;
                    log.ReferenciaContrato = pagoUnidadContrato.ReferenciaContrato;
                    this.logFacturacionBR.Insertar(this.dctx, log, seguridad);
                }
                catch
                { }

                throw;
            }
            finally
            {
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
        /// <param name="pagoUnidadContrato">Objeto de pago que contiene información para obtención de los impuestos</param>
        /// <param name="creditoCliente">Crédito del cliente en curso</param>
        /// <returns>Lista de lineas de concepto</returns>
        private IEnumerable<DetalleTransaccionBO> CalcularImpuestos(IEnumerable<IComponentBase> detallesTransaccion, PagoUnidadContratoBO pagoUnidadContrato, CreditoClienteBO creditoCliente)
        {
            if (detallesTransaccion == null || detallesTransaccion.Count() == 0)
                yield break;

            AuditoriaBO auditoria = this.CrearObjetoAuditoria();
            DepartamentoBO departamento = this.departamentoFacturacionBR.Consultar(pagoUnidadContrato.ReferenciaContrato.TipoContratoID.Value, this.vista.ObtenerCarpetaRaiz());

            foreach (var childrem in detallesTransaccion)
            {
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
                subtotalLinea = Decimal.Truncate((subtotalLinea*100M))/100M;
                decimal impuestoLinea = detalle.AplicaIVA.GetValueOrDefault() && pagoUnidadContrato.ReferenciaContrato.Sucursal.Impuesto.PorcentajeImpuesto.HasValue
                                        ? subtotalLinea * pagoUnidadContrato.ReferenciaContrato.Sucursal.Impuesto.PorcentajeImpuesto.GetValueOrDefault() / 100
                                        : 0M;

                if (!detalle.RetencionUnitaria.HasValue)
                    detalle.RetencionUnitaria = 0M;

                detalle.ImpuestoUnitario = Decimal.Truncate(impuestoLinea*100M)/100M;

                detalle.Departamento = departamento;
                detalle.Auditoria = auditoria;
                detalle.Activo = true;

                if (creditoCliente != null)
                {
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
        /// <param name="pagoUnidadContrato">Objeto de pago</param>
        private List<DetalleTransaccionBO>  ReajustarDetallesTransaccion(TransaccionBO transaccion, IList<DetalleTransaccionBO> costosAdicionales, PagoUnidadContratoBO pagoUnidadContrato, CreditoClienteBO creditoCliente)
        {
            List<DetalleTransaccionBO> detallesTransaccionBase = transaccion.GetChildren()
                                                                    .OfType<DetalleTransaccionBO>()
                                                                    .ToList();            

            if (costosAdicionales.Count > 0)
            {
                foreach (DetalleTransaccionBO detalle in costosAdicionales)
                    transaccion.Add(detalle);
            }

            List<DetalleTransaccionBO> lineasContrato = this.CalcularImpuestos(transaccion, pagoUnidadContrato, creditoCliente);

            bool lineasBaseEnCero = detallesTransaccionBase
                                    .All(x => x.PrecioUnitario <= 0);

            if (lineasBaseEnCero && costosAdicionales.Count > 0)
            {
                DetalleTransaccionBO detalleTransaccion = costosAdicionales.First();
                String[] observaciones = detallesTransaccionBase
                                            .Select(x => x.Articulo.Nombre)
                                            .ToArray();

                detalleTransaccion.Observaciones = String.Join("\n", observaciones);

                //Se acorta la longitud de las observaciones
                if (detalleTransaccion.Observaciones.Length > 499)
                    detalleTransaccion.Observaciones = detalleTransaccion.Observaciones.Substring(0, 499);

                foreach (DetalleTransaccionBO detalleTransaccionCero in detallesTransaccionBase)
                {
                    transaccion.Remove(detalleTransaccionCero);
                    lineasContrato.Remove(detalleTransaccionCero);
                }
            }

            foreach (var detalleTransaccionBo in lineasContrato.Where(detalleTransaccionBo => !String.IsNullOrEmpty(detalleTransaccionBo.Observaciones) && detalleTransaccionBo.Observaciones.Length > 499))
            {
                detalleTransaccionBo.Observaciones = detalleTransaccionBo.Observaciones.Substring(0, 499);
            }

            return lineasContrato;
        }
        #endregion

        /// <summary>
        /// Calcula los impuestos de una transacción y devuelve la lista de detalles procesados
        /// </summary>
        /// <param name="transaccion">Transacción a procesar</param>
        /// <param name="detallesTransaccion">Lista de detalles de transaccion</param>
        /// <param name="creditoCliente">Crédito del cliente en curso</param>
        /// <returns>Lista de lineas de concepto</returns>
        private List<DetalleTransaccionBO> CalcularImpuestos(TransaccionBO transaccion, PagoUnidadContratoBO pagoUnidadContrato, CreditoClienteBO creditoCliente)
        {
            List<DetalleTransaccionBO> detalles = new List<DetalleTransaccionBO>();

            transaccion.Subtotal = 0M;
            transaccion.Impuestos = 0M;
            transaccion.Descuentos = 0M;
            transaccion.Retenciones = 0M;

            foreach (var detalle in this.CalcularImpuestos(transaccion.GetChildren(), pagoUnidadContrato, creditoCliente))
            {
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
        public void CancelarRegistro()
        {
            this.vista.RedirigirACancelacion();
        }

        /// <summary>
        /// Ejecuta el proceso de regresar a la página anterior
        /// </summary>
        public void RetrocederPagina()
        {
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
        public void ConsultarOtraFacturacion()
        {
            vista.RedirigirAConsulta();
        }

        /// <summary>
        /// SC0034, Determina si un pago es válido para ser facturado verificando que los pagos anteriores hayan sido enviados a facturar
        /// </summary>
        /// <param name="pagoUnidadContrato">Pago a ser evaluado</param>
        /// <returns>Devuelve true si el pago es el siguiente a ser enviado a facturar, de lo contrario devolverá false</returns>
        private bool EsPagoValido(PagoUnidadContratoBO pagoUnidadContrato)
        {
            bool result = true;
            var pagoUnidadContratoLs = this.ConsultarPagos(null, pagoUnidadContrato.ReferenciaContrato.ReferenciaContratoID, pagoUnidadContrato.Unidad.UnidadID, this.vista.TipoPagoUnidadContrato, false);
            if (pagoUnidadContratoLs.Count == 0)
                throw new Exception(String.Format("No se han encontrado pagos para la referencia {0}", pagoUnidadContrato.ReferenciaContrato.ReferenciaContratoID));

            var pagosAnteriores = pagoUnidadContratoLs
                                        .Where(x => x.PagoID < pagoUnidadContrato.PagoID && x.Activo.GetValueOrDefault())
                                        .OrderBy(x => x.PagoID)
                                        .ToList();

            if (pagosAnteriores.Count > 0)
                result = pagosAnteriores.Last().EnviadoFacturacion.GetValueOrDefault();

            return result;
        }

        /// <summary>
        /// Establece las operación permitidas según un número de página
        /// </summary>
        /// <param name="numeroPagina">Número de página a evaluar</param>
        private void EstablecerOpcionesSegunPagina(int numeroPagina)
        {
            this.vista.PermitirRegresar(true);
            this.vista.PermitirContinuar(true);
            this.vista.OcultarContinuar(false);
            this.vista.OcultarTerminar(true);

            if (numeroPagina == 1)
            {
                this.vista.PermitirRegresar(false);
            }

            if (numeroPagina == 5)
            {
                this.vista.PermitirContinuar(false);

                this.vista.OcultarContinuar(true);
                this.vista.OcultarTerminar(false);
            }
        }

        /// <summary>
        /// Decide que controlador usar para consultar pagos
        /// </summary>
		private List<PagoUnidadContratoBO> ConsultarPagos(Int32? pagoId, Int32? referenciaContratoId, Int32? unidadId, ETipoContrato? tipoContrato, Boolean esCompleto)
        {
            var listaPagos = new List<PagoUnidadContratoBO>();
            PagoUnidadContratoBO pago = new PagoUnidadContratoBOF();
            switch (tipoContrato)
            {
                case ETipoContrato.FSL:
                    pago = new PagoUnidadContratoFSLBO(){Tarifa = new TarifaPagoEquipoBO()};
                    break;
                case ETipoContrato.RD:
                    pago = new PagoUnidadContratoRDBO(){ Tarifa = new TarifaPagoEquipoBO()};
                    break;
                case ETipoContrato.CM:
                case ETipoContrato.SD:
                    pago = new PagoUnidadContratoManttoBO(tipoContrato.Value){ Tarifa = new TarifaPagoEquipoBO()};
                    break;
            }

            if (pagoId != null)
                pago.PagoID = pagoId;
            if(referenciaContratoId != null)
                pago.ReferenciaContrato = new ReferenciaContratoBO(){ReferenciaContratoID = referenciaContratoId};
            if (unidadId != null)
                pago.Unidad = new UnidadBO(){UnidadID = unidadId};

            switch(tipoContrato)
            {
                case ETipoContrato.FSL:
                    listaPagos.AddRange(esCompleto ? ((PagoUnidadContratoFSLBR) this.pagoUnidadContratoBR).ConsultarCompleto(this.dctx, (PagoUnidadContratoFSLBO) pago, true) : ((PagoUnidadContratoFSLBR) this.pagoUnidadContratoBR).Consultar(this.dctx, (PagoUnidadContratoFSLBO) pago));
                    break;
                case ETipoContrato.RD:
                    listaPagos.AddRange(esCompleto ? ((PagoUnidadContratoRDBR) this.pagoUnidadContratoBR).ConsultarCompleto(this.dctx, (PagoUnidadContratoRDBO) pago) : ((PagoUnidadContratoRDBR) this.pagoUnidadContratoBR).Consultar(this.dctx, (PagoUnidadContratoRDBO) pago));
                    break;
                case ETipoContrato.CM:
                case ETipoContrato.SD:
                    listaPagos.AddRange(esCompleto ? this.pagoUnidadContratoBR.ConsultarCompleto(this.dctx, pago) : this.pagoUnidadContratoBR.Consultar(this.dctx, pago));
                    break;
            }

            return listaPagos;
        }

        /// <summary>
        /// Utilizar el controlador adecuado para actualizar los pagos
        /// </summary>
		private void ActualizarPagos(PagoUnidadContratoBO pago, PagoUnidadContratoBO pagoAnterior, SeguridadBO seguridadBo, ETipoContrato? tipoContrato)
        {
            switch (tipoContrato)
            {
                case ETipoContrato.FSL:
                    ((PagoUnidadContratoFSLBR)this.pagoUnidadContratoBR).ActualizarCompleto(this.dctx, (PagoUnidadContratoFSLBO)pago, (PagoUnidadContratoFSLBO)pagoAnterior, seguridadBo);
                    break;
                case ETipoContrato.RD:
                    ((PagoUnidadContratoRDBR)pagoUnidadContratoBR).ActualizarCompleto(this.dctx, pago as PagoUnidadContratoRDBO, pagoAnterior as PagoUnidadContratoRDBO, seguridadBo);
                    break;
                case ETipoContrato.CM:
                case ETipoContrato.SD:
                    pagoUnidadContratoBR.ActualizarCompleto(this.dctx, pago, pagoAnterior, seguridadBo);
                    break;
            }
        }

        /// <summary>
        /// Obtiene información del Pago y lo envía al Visor de Pagos No Facturados.
        /// </summary>
        public void ConfirmarMoverPagoSinCredito()
        {
            try
            {
                PagoUnidadContratoBO pagoUnidadContrato = this.vista.PagoActual;

                pagoUnidadContrato.Divisa.MonedaDestino = this.vista.CostosAdicionalesView.CodigoMoneda != "-1"
                                                                ? new MonedaBO { Codigo = this.vista.CostosAdicionalesView.CodigoMoneda }
                                                                : this.vista.PagoActual.Divisa.MonedaOrigen;

                pagoUnidadContrato.Divisa = this.ResolverDivisa(this.vista.PagoActual);
                AuditoriaBO auditoria = this.CrearObjetoAuditoria();
                SeguridadBO seguridad = this.CrearObjetoSeguridad();
                String tipoCredito = this.ResolverTipoCredito(pagoUnidadContrato).ToString();
                String errorMessage = String.Format("El cliente no cuenta con crédito disponible suficiente o el crédito se encuentra inactivo: {0}", tipoCredito);
                MoverPagoFaltaCredito(errorMessage,auditoria, pagoUnidadContrato, seguridad);
            }
            catch (Exception ex)
            {
                throw new Exception(nombreClase + "ConfirmarMoverPagoSinCredito(): " + ex.Message);
            }
        }

        /// <summary>
        /// Mueve el Pago al visor de Pagos NO Facturados por Falta de Credito
        /// </summary>
        /// <param name="errorMessage">Mensaje de error para el Log</param>
        /// <param name="auditoria">Objeto de Auditoria</param>
        /// <param name="pagoUnidadContrato">Pago sin credito</param>
        /// <param name="seguridad">Objeto de Seguridad</param>
        private void MoverPagoFaltaCredito(string errorMessage, AuditoriaBO auditoria, PagoUnidadContratoBO pagoUnidadContrato, SeguridadBO seguridad)
        {
            this.vista.PermitirTerminar(false);
            this.vista.PertimirCapturar(false);
            
            LogFacturacionBO log = new LogFacturacionBO();
            log.Auditoria = auditoria;
            log.MensajeError = errorMessage;
            log.NumeroReferencia = !String.IsNullOrEmpty(pagoUnidadContrato.Unidad.NumeroSerie) ? pagoUnidadContrato.Unidad.NumeroSerie : pagoUnidadContrato.Unidad.NumeroEconomico;
            log.Pago = pagoUnidadContrato;
            log.ReferenciaContrato = pagoUnidadContrato.ReferenciaContrato;
            this.logFacturacionBR.Insertar(this.dctx, log, seguridad);

            var pagoAnterior = this.ConsultarPagos(pagoUnidadContrato.PagoID, pagoUnidadContrato.ReferenciaContrato.ReferenciaContratoID, pagoUnidadContrato.Unidad.UnidadID, pagoUnidadContrato.TipoContrato, true).FirstOrDefault();
            if(pagoAnterior == null)
                throw new Exception("No se encontro el pago para actualizar.");

            pagoUnidadContrato.BloqueadoCredito = true;
            this.vista.PagoActual = pagoUnidadContrato;
            this.ActualizarPagos(pagoUnidadContrato, pagoAnterior, seguridad, pagoUnidadContrato.TipoContrato);
        }

        /// <summary>
        /// Calcula el monto para el seguro
        /// </summary>
        /// <param name="pago">Pago que contiene la unidad para buscar el seguro</param>
        /// <param name="generador">Generador utilizado para configurar la Transaccion</param>
        /// <returns>Retorna una observacion en caso de que exista alguna inconsistencia</returns>
        private string CalcularCobraSeguro(PagoUnidadContratoBO pago, IGeneradorPaquetesFacturacionBR generador)
        {
            var seguroUnidadBr = new CalcularSeguroUnidadBR();
            Contratos.FSL.BO.EFrecuenciaSeguro? frecuenciaSeguro = null;
            int? porcentajeSeguro = 1;
            if (seguroUnidadBr.CobrarSeguroUnidad(dctx, pago.NumeroPago, pago.ReferenciaContrato.ReferenciaContratoID, pago.TipoContrato, ref frecuenciaSeguro, ref porcentajeSeguro) == false) return "";

            if (pago.LineasCostoAdicional.Any(x => x.TipoRenglon == ETipoRenglon.SEGUROS.ToInt32(CultureInfo.InvariantCulture)))
                return "";

            var observaciones = "";
            var montoSeguro = seguroUnidadBr.CalcularMontoSeguro(dctx, pago.Unidad, ref observaciones, frecuenciaSeguro, porcentajeSeguro);
            if (montoSeguro != null && montoSeguro > 0)
            {
                if (pago.LineasCostoAdicional == null)
                    pago.LineasCostoAdicional = new List<LineaCostoAdicionalBO>();

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
                detalle.PrecioUnitario = montoSeguro;
                detalle.CostoUnitario = generador.Costo;
                detalle.DescuentoUnitario = 0M;
                detalle.RetencionUnitaria = 0M;
                detalle.EstablecerTipoRenglon(ETipoRenglon.SEGUROS.ToInt32(CultureInfo.InvariantCulture));
                detalle.Cantidad = generador.Cantidad;
                detalle.UnidadMedida = generador.UnidadMedida;
                #region Producto Servicio
                if (pago.ProductoServicio != null && !String.IsNullOrWhiteSpace(pago.ProductoServicio.NombreCorto)) { 
                    ProductoServicioBR prodServBR = new ProductoServicioBR();
                    CatalogoBaseBO producto = prodServBR.Consultar(dctx, pago.ProductoServicio).FirstOrDefault();
                    if (producto != null)
                        detalle.ProductoServicio = (ProductoServicioBO)producto;
                    else
                        detalle.ProductoServicio = new ProductoServicioBO();                    
                }
	            #endregion
                detalle.Activo = true;

                this.vista.CostosAdicionalesView.CostosAdicionales.Add(detalle);

                this.vista.CostosAdicionalesView.MostrarListaCostosAdicionales();
            }

            return observaciones;
        }
        #endregion
    }
}
