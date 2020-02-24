using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using BPMO.Basicos.BO;
using BPMO.Facade.SDNI.BR;
using BPMO.Patterns.Creational.DataContext;
using BPMO.Primitivos.Enumeradores;
using BPMO.SDNI.Facturacion.AplicacionesFacturacion.VIS;
using BPMO.SDNI.Facturacion.InfraestructuraPagos.BO;
using BPMO.SDNI.Facturacion.InfraestructuraPagos.BOF;
using BPMO.SDNI.Facturacion.InfraestructuraPagos.BR;
using BPMO.SDNI.Contratos.BO;
using BPMO.SDNI.Equipos.BO;
using BPMO.SDNI.Comun.BO;
using System.Data;

namespace BPMO.SDNI.Facturacion.AplicacionesFacturacion.PRE
{
    /// <summary>
    /// Presentador de la Consulta de Pagos por Facturar
    /// </summary>
    public class ConsultarPagosFacturarPRE
    {

        #region Atributos
        /// <summary>
        /// Codigo de la Impresión del Histórico
        /// </summary>
        private const string codigoImprimirHistorico = "BEP1401.CU006";
        /// <summary>
        /// Nombre de la Clase
        /// </summary>
        private const string nombreClase = "ConsultarPagosFacturarPRE";
        /// <summary>
        /// Vista de la Consulta
        /// </summary>
        private readonly IConsultarPagosFacturarVIS Vista;
        /// <summary>
        /// Contexto de conexion de datos
        /// </summary>
        private readonly IDataContext dataContext;
        /// <summary>
        /// Controlador Principal
        /// </summary>
        private readonly PagoUnidadContratoBR Controlador;

        /// <summary>
        /// Controlador de PSL
        /// </summary>
        private readonly PagoContratoPSLBR ControladorPSL;
        /// <summary>
        /// Nombre del Permisos de Configurar Facturación
        /// </summary>
        private const string PermisoConfigurarFacturacion = "UI CONFIGURARFACTURACION";
        /// <summary>
        /// Nombre del Permisos de Obtener Datos Historicos de Pagos
        /// </summary>
        private const string PermisoHistoricoPagos = "OBTENERDATOSHISTORICOPAGOS";
        /// <summary>
        /// Nombre del Permisos de la Consulta
        /// </summary>
        private const string PermisoConsultar = "CONSULTAR";
        /// <summary>
        /// Nombre del Permisos de Poder Cancelar Pagos
        /// </summary>
        private const string PermisoCancelarPagos = "CANCELARPAGOSUI";

        #endregion

        #region Constructores
        /// <summary>
        /// Constructor por defecto de la Consulta
        /// </summary>
        /// <param name="vista">Vista de la Consulta</param>
        public ConsultarPagosFacturarPRE(IConsultarPagosFacturarVIS vista)
        {
            if (vista == null) throw new ArgumentNullException("vista");

            Vista = vista;            

            dataContext = FacadeBR.ObtenerConexion();

            Controlador= new PagoUnidadContratoBR();
            ControladorPSL = new PagoContratoPSLBR();
        }

        #endregion

        #region Metodos

        #region CU006 Ver Historico de Pagos
        /// <summary>
        /// Realiza la impresión del Histórico de Pagos para una referencia de Contrato Seleccionada
        /// </summary>
        public void ImprimirHistoricoPagos(int pagoId)
        {
            try
            {
                ReferenciaContratoBO referenciaContrato = new ReferenciaContratoBO();
                if (this.Vista.UnidadOperativaID != (int)ETipoEmpresa.Idealease)
                {
                    var pagoGN = Vista.PagosConsultadosPSL.Find(x => x.PagoContratoID == pagoId);
                    referenciaContrato = pagoGN.ReferenciaContrato;
                    
                }
                else
                {
                    var pagoI = Vista.PagosConsultados.Find(x => x.PagoID == pagoId);
                    referenciaContrato = pagoI.ReferenciaContrato;
                }
                // Validar la Información de la Referencia de Contrato Seleccionada
                Vista.ReferenciaContratoSeleccionada = referenciaContrato;
                var msg = ValidarDatosImprimirHistorico(Vista.ReferenciaContratoSeleccionada);

                if (!string.IsNullOrEmpty(msg))
                {
                    Vista.MostrarMensaje(msg, ETipoMensajeIU.ADVERTENCIA);
                    return;
                }

                var referenciaBR = new ReferenciaContratoBR();
                Vista.ReferenciaContratoSeleccionada = referenciaBR.Consultar(dataContext, Vista.ReferenciaContratoSeleccionada).FirstOrDefault();

                // Crear Objeto de Seguridad
                var usuario = new UsuarioBO { Id = Vista.UC };
                var adscripcion = new AdscripcionBO { UnidadOperativa = Vista.UnidadOperativa };
                var seguridad = new SeguridadBO(Guid.Empty, usuario, adscripcion);

                //Obtener Datos de Reporte
                var imprmirPagosBR = new ImprimirPagosBR();
                var datosReporte = imprmirPagosBR.ObtenerDatosHistoricoPagos(dataContext,
                    Vista.ReferenciaContratoSeleccionada, Vista.UnidadOperativa, seguridad, this.Vista.URLLogoEmpresa);

                Vista.EstablecerPaqueteNavegacionImprimir(codigoImprimirHistorico, datosReporte);
                Vista.IrAImprimirHistorico();
            }
            catch (Exception ex)
            {
                var strMetodo = new StackFrame().GetMethod().Name;
                var strMsg = string.Format("{0}.{1}: {2}", nombreClase, strMetodo, ex.Message);
                Vista.MostrarMensaje("Inconsistencias al imprimir el Histórico de Pagos", ETipoMensajeIU.ERROR, strMsg);
            }
        }

        /// <summary>
        /// Realiza la Validación de los Datos de la Referencia de Contrato
        /// </summary>
        /// <param name="referencia">Referencia de Contrato que contiene la información a Validar</param>
        /// <returns></returns>
        private string ValidarDatosImprimirHistorico(ReferenciaContratoBO referencia)
        {
            if (referencia == null) return "Proporcione una Referencia de Contrato";

            if (referencia.ReferenciaContratoID == null) return "Proporcione el Identificador de la Referencia de Contrato";
            return string.Empty;
        } 
        #endregion

        #region CU004 Consulta de Pagos a Facturar
        /// <summary>
        /// Primera carga de información de la consulta
        /// </summary>
        public void PrimeraCarga()
        {
            Vista.LimpiarSession();
            Vista.MarcarPagoNoFacturados();
            EstablecerSeguridad();

            var bof = ObtenerBaseBOF();
            if (bof is PagoUnidadContratoBOF)
            {
                ((PagoUnidadContratoBOF)bof).Sucursales = Vista.SucursalesUsuario;
                ((PagoUnidadContratoBOF)bof).FechaVencimientoFinal = DateTime.Now;
                Consultar((PagoUnidadContratoBOF)bof);
            }
            else
            {
                ((PagoContratoPSLBOF)bof).Sucursales = Vista.SucursalesUsuario;
                ((PagoContratoPSLBOF)bof).FechaVencimientoFinal = DateTime.Now;
                Consultar((PagoContratoPSLBOF)bof);
            }
        }
        /// <summary>
        /// Consultar los Pagos
        /// </summary>
        public void Consultar()
        {
            var bof = InterfazUsuarioADato();
            if (bof is PagoUnidadContratoBOF)
            {
                Consultar((PagoUnidadContratoBOF)bof);
            }
            else
            {
                Consultar((PagoContratoPSLBOF)bof);
            }
        }

        /// <summary>
        /// Consultar los pagos en base a filtro proporcionados
        /// </summary>
        /// <param name="bof"></param>
        private void Consultar(PagoUnidadContratoBOF bof)
        {
            List<PagoUnidadContratoBO> listaPagos = new List<PagoUnidadContratoBO>();

            listaPagos = Controlador.ConsultarFiltroSinCuentas(dataContext, bof);
            
            if (Vista.UltimosPagos == null)
            {
                Vista.UltimosPagos = new List<PagoUnidadContratoBO>();
                Vista.UltimosPagos.AddRange(listaPagos);
            }
            Vista.PagosConsultados = listaPagos;

            Vista.CargarPagosConsultados();
        }
        /// <summary>
        /// Consultar los pagos en base a filtro proporcionados
        /// </summary>
        /// <param name="bof"></param>
        private void Consultar(PagoContratoPSLBOF bof)
        {
            List<PagoContratoPSLBO> listaPagos = new List<PagoContratoPSLBO>();

            listaPagos = ControladorPSL.ConsultarFiltroSinCuentas(dataContext, bof, this.Vista.ModuloID);

            if (Vista.UltimosPagosPSL == null)
            {
                Vista.UltimosPagosPSL = new List<PagoContratoPSLBO>();
                Vista.UltimosPagosPSL.AddRange(listaPagos);
            }
            Vista.PagosConsultadosPSL = listaPagos;

            Vista.CargarPagosConsultados();
        }
        /// <summary>
        /// Decide que controlador usar para consultar pagos
        /// </summary>
        private List<PagoUnidadContratoBO> ConsultarPagos(Int32? pagoId, Int32? referenciaContratoId, Int32? unidadId, ETipoContrato? tipoContrato, Boolean esCompleto) {
            var listaPagos = new List<PagoUnidadContratoBO>();
            PagoUnidadContratoBR pagoBr = null;
            PagoUnidadContratoBO pago = new PagoUnidadContratoBOF();
            switch (tipoContrato) {
                case ETipoContrato.FSL:
                    pagoBr = new PagoUnidadContratoFSLBR();
                    pago = new PagoUnidadContratoFSLBO() { Tarifa = new TarifaPagoEquipoBO() };
                    break;
                case ETipoContrato.RD:
                    pagoBr = new PagoUnidadContratoRDBR();
                    pago = new PagoUnidadContratoRDBO() { Tarifa = new TarifaPagoEquipoBO() };
                    break;
                case ETipoContrato.CM:
                case ETipoContrato.SD:
                    pagoBr = new PagoUnidadContratoBR();
                    pago = new PagoUnidadContratoManttoBO(tipoContrato.Value) { Tarifa = new TarifaPagoEquipoBO() };
                    break;
            }

            if (pagoId != null)
                pago.PagoID = pagoId;
            if (referenciaContratoId != null)
                pago.ReferenciaContrato = new ReferenciaContratoBO() { ReferenciaContratoID = referenciaContratoId };
            if (unidadId != null)
                pago.Unidad = new UnidadBO() { UnidadID = unidadId };

            switch (tipoContrato) {
                case ETipoContrato.FSL:
                    listaPagos.AddRange(esCompleto ? ((PagoUnidadContratoFSLBR)pagoBr).ConsultarCompleto(this.dataContext, (PagoUnidadContratoFSLBO)pago, true) : ((PagoUnidadContratoFSLBR)pagoBr).Consultar(this.dataContext, (PagoUnidadContratoFSLBO)pago));
                    break;
                case ETipoContrato.RD:
                    listaPagos.AddRange(esCompleto ? ((PagoUnidadContratoRDBR)pagoBr).ConsultarCompleto(this.dataContext, (PagoUnidadContratoRDBO)pago) : ((PagoUnidadContratoRDBR)pagoBr).Consultar(this.dataContext, (PagoUnidadContratoRDBO)pago));
                    break;
                case ETipoContrato.CM:
                case ETipoContrato.SD:
                    listaPagos.AddRange(esCompleto ? pagoBr.ConsultarCompleto(this.dataContext, pago) : pagoBr.Consultar(this.dataContext, pago));
                    break;
            }

            return listaPagos;
        }
        /// <summary>
        /// Decide que controlador usar para consultar pagos
        /// </summary>
        private List<PagoContratoPSLBO> ConsultarPagos(Int32? pagoId, Int32? referenciaContratoId, Boolean esCompleto)
        {
            var listaPagos = new List<PagoContratoPSLBO>();
            PagoContratoPSLBR pagoBr = new PagoContratoPSLBR();
            PagoContratoPSLBO pago = new PagoContratoPSLBOF();
           

            if (pagoId != null)
                pago.PagoContratoID = pagoId;
            if (referenciaContratoId != null)
                pago.ReferenciaContrato = new ReferenciaContratoBO() { ReferenciaContratoID = referenciaContratoId };

            listaPagos.AddRange(esCompleto ? pagoBr.ConsultarCompleto(this.dataContext, pago) : pagoBr.Consultar(this.dataContext, pago));
            return listaPagos;
        }
        /// <summary>
        /// Obtiene el BO Base de Filtros
        /// </summary>
        /// <returns></returns>
        private object ObtenerBaseBOF()
        {
            if (ETipoEmpresa.Idealease == (ETipoEmpresa)this.Vista.UnidadOperativaID)
            {
                return new PagoUnidadContratoBOF
                {
                    Activo = true,
                    EnviadoFacturacion = false,
                    Facturado = false,
                    ReferenciaContrato = new ReferenciaContratoBO
                    {
                        UnidadOperativa = new UnidadOperativaBO { Id = Vista.UnidadOperativaID },
                        CuentaCliente = new Comun.BO.CuentaClienteIdealeaseBO()
                    },
                    FacturaEnCeros = false,
                    BloqueadoCredito = false
                };
            }
            else
            {
                return new PagoContratoPSLBOF
                {
                    Activo = true,
                    EnviadoFacturacion = false,
                    Facturado = false,
                    ReferenciaContrato = new ReferenciaContratoBO
                    {
                        UnidadOperativa = new UnidadOperativaBO { Id = Vista.UnidadOperativaID },
                        CuentaCliente = new Comun.BO.CuentaClienteIdealeaseBO()
                    },
                    FacturaEnCeros = false,
                    BloqueadoCredito = false
                };
            }
        }
        /// <summary>
        /// Obtiene los datos de la Vista
        /// </summary>
        /// <returns></returns>
        private object InterfazUsuarioADato()
        {
            var bof = ObtenerBaseBOF();
            if (bof is PagoUnidadContratoBOF)
            {
                ((PagoUnidadContratoBOF)bof).Sucursales = Vista.SucursalSeleccionadaID != null ? new List<SucursalBO> { new SucursalBO { Id = Vista.SucursalSeleccionadaID } } : Vista.SucursalesUsuario;
                #region SC0035
                ((PagoUnidadContratoBOF)bof).ReferenciaContrato.FolioContrato = Vista.NumeroContrato != null ? Vista.NumeroContrato : null;
                ((PagoUnidadContratoBOF)bof).ReferenciaContrato.CuentaCliente.Id = Vista.CuentaClienteID != null ? Vista.CuentaClienteID : null;
                ((PagoUnidadContratoBOF)bof).ReferenciaContrato.CuentaCliente.Nombre = Vista.NombreCuentaCliente != null ? Vista.NombreCuentaCliente : null;
                ((PagoUnidadContratoBOF)bof).Referencia = Vista.VinNumeroEconomico != null ? Vista.VinNumeroEconomico : null;
                #endregion
                ((PagoUnidadContratoBOF)bof).Departamento = Vista.DepartamentoSeleccionado;
                if (Vista.FechaVencimientoInicio == null && Vista.FechaVencimientoFin == null)
                    ((PagoUnidadContratoBOF)bof).FechaVencimientoFinal = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day, 23, 59, 59);
                else
                {
                    #region RI0008
                    if (Vista.FechaVencimientoFin != null)
                    {
                        var fechaFinal = new DateTime(Vista.FechaVencimientoFin.Value.Year, Vista.FechaVencimientoFin.Value.Month, Vista.FechaVencimientoFin.Value.Day, 23, 59, 59, 999);
                        ((PagoUnidadContratoBOF)bof).FechaVencimientoFinal = fechaFinal;
                    }
                    if (Vista.FechaVencimientoInicio != null)
                    {
                        ((PagoUnidadContratoBOF)bof).FechaVencimientoInicial = Vista.FechaVencimientoInicio;
                    }
                    #endregion
                }
            }
            else
            {
                ((PagoContratoPSLBOF)bof).Sucursales = Vista.SucursalSeleccionadaID != null ? new List<SucursalBO> { new SucursalBO { Id = Vista.SucursalSeleccionadaID } } : Vista.SucursalesUsuario;
                ((PagoContratoPSLBOF)bof).ReferenciaContrato.FolioContrato = Vista.NumeroContrato != null ? Vista.NumeroContrato : null;
                ((PagoContratoPSLBOF)bof).ReferenciaContrato.CuentaCliente.Id = Vista.CuentaClienteID != null ? Vista.CuentaClienteID : null;
                ((PagoContratoPSLBOF)bof).ReferenciaContrato.CuentaCliente.Nombre = Vista.NombreCuentaCliente != null ? Vista.NombreCuentaCliente : null;
                ((PagoContratoPSLBOF)bof).ReferenciaFiltro = Vista.VinNumeroEconomico != null ? Vista.VinNumeroEconomico : null;
                ((PagoContratoPSLBOF)bof).Departamento = Vista.DepartamentoSeleccionado;
                if (Vista.FechaVencimientoInicio == null && Vista.FechaVencimientoFin == null)
                    ((PagoContratoPSLBOF)bof).FechaVencimientoFinal = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day, 23, 59, 59);
                else
                {
                    if (Vista.FechaVencimientoFin != null)
                    {
                        var fechaFinal = new DateTime(Vista.FechaVencimientoFin.Value.Year, Vista.FechaVencimientoFin.Value.Month, Vista.FechaVencimientoFin.Value.Day, 23, 59, 59, 999);
                        ((PagoContratoPSLBOF)bof).FechaVencimientoFinal = fechaFinal;
                    }
                    if (Vista.FechaVencimientoInicio != null)
                    {
                        ((PagoContratoPSLBOF)bof).FechaVencimientoInicial = Vista.FechaVencimientoInicio;
                    }
                }
            }

            return bof;
        }
        /// <summary>
        /// Redirige a la configuración de facturación
        /// </summary>
        /// <param name="pagoId"></param>
        public void ConfigurarFacturacion(int pagoId)
        {
            var pago = Vista.PagosConsultados.Find(x => x.PagoID == pagoId);
            Vista.EstablecerPaqueteNavegacionFacturacion(pago);
            Vista.IrAConfigurarFacturacion(false);
        }

        /// <summary>
        /// Redirige a la configuración de facturación PSL
        /// </summary>
        /// <param name="pagoId"></param>
        public void ConfigurarFacturacionPSL(int pagoId)
        {
            var pago = Vista.PagosConsultadosPSL.Find(x => x.PagoContratoID == pagoId);
            Vista.EstablecerPaqueteNavegacionFacturacion(pago);
            Vista.IrAConfigurarFacturacion(true);
        }

        /// <summary>
        /// SC0034, Determina si un pago es válido para ser facturado verificando que los pagos anteriores hayan sido enviados a facturar
        /// </summary>
        /// <param name="pagoUnidadContrato">Pago a ser evaluado</param>
        /// <returns>Devuelve true si el pago es el siguiente a ser enviado a facturar, de lo contrario devolverá false</returns>
        public bool EsPagoValido(object pagoUnidadContrato) {
            bool result = true;
            if (pagoUnidadContrato.GetType() == typeof(PagoContratoPSLBO))
            {
                var pagoUnidadContratoLs = this.ConsultarPagos(null, ((PagoContratoPSLBO)pagoUnidadContrato).ReferenciaContrato.ReferenciaContratoID,  false);
                var pagosAnteriores = pagoUnidadContratoLs
                                        .Where(x => x.PagoContratoID < ((PagoContratoPSLBO)pagoUnidadContrato).PagoContratoID && x.Activo.GetValueOrDefault())
                                        .OrderBy(x => x.PagoContratoID)
                                        .ToList();
                if (pagoUnidadContratoLs.Count == 0)
                    throw new Exception(String.Format("No se han encontrado pagos para la referencia {0}", ((PagoContratoPSLBO)pagoUnidadContrato).ReferenciaContrato.ReferenciaContratoID));
                if (pagosAnteriores.Count > 0)
                    result = pagosAnteriores.Last().EnviadoFacturacion.GetValueOrDefault();
            }
            else
            {
                var pagoUnidadContratoLs = this.ConsultarPagos(null, ((PagoUnidadContratoBO)pagoUnidadContrato).ReferenciaContrato.ReferenciaContratoID, ((PagoUnidadContratoBO)pagoUnidadContrato).Unidad.UnidadID, ((PagoUnidadContratoBO)pagoUnidadContrato).TipoContrato, false);
                var pagosAnteriores = pagoUnidadContratoLs
                                        .Where(x => x.PagoID < ((PagoUnidadContratoBO)pagoUnidadContrato).PagoID && x.Activo.GetValueOrDefault())
                                        .OrderBy(x => x.PagoID)
                                        .ToList();
                if (pagoUnidadContratoLs.Count == 0)
                    throw new Exception(String.Format("No se han encontrado pagos para la referencia {0}", ((PagoUnidadContratoBO)pagoUnidadContrato).ReferenciaContrato.ReferenciaContratoID));
                if (pagosAnteriores.Count > 0)
                    result = pagosAnteriores.Last().EnviadoFacturacion.GetValueOrDefault();
            }
         
            return result;
        }

        /// <summary>
        /// Establece las opciones permitidas en base a la seguridad del usuario
        /// </summary>
        private void EstablecerSeguridad()
        {
            try
            {
                //se valida que los datos del usuario y la unidad operativa no sean nulos
                if (Vista.UsuarioID == null) throw new Exception("El identificador del usuario no debe ser nulo");
                if (Vista.UnidadOperativaID == null) throw new Exception("El identificador de la Unidad Operativa no debe ser nulo ");

                var usr = new UsuarioBO { Id = Vista.UsuarioID };
                var adscripcion = new AdscripcionBO { UnidadOperativa = new UnidadOperativaBO { Id = Vista.UnidadOperativaID } };
                var seguridadBO = new SeguridadBO(Guid.Empty, usr, adscripcion);

                //Se obtienen las acciones a las cuales el usuario tiene permiso en este proceso
                List<CatalogoBaseBO> lst = FacadeBR.ConsultarAccion(dataContext, seguridadBO);

                //Se valida si el usuario tiene permiso para consultar
                if (!ExisteAccion(lst, PermisoConsultar))
                    Vista.RedirigirSinPermisoAcceso();
                //Se valida si el usuario tiene permiso para Configurar la Facturación
                if (!ExisteAccion(lst, PermisoConfigurarFacturacion))
                    Vista.PermitirFacturar(false);
                // Se Valida si el Usuario tiene permisos para Generar el Histórico de Pagos
                if (!ExisteAccion(lst, PermisoHistoricoPagos))                
                    Vista.PermitirHistorico(false);
                //Se valida si el usuario tiene permiso para Cancelar la Facturación
                if (!ExisteAccion(lst, PermisoCancelarPagos))
                    Vista.PermitirCancelarPago(false);
            }
            catch (Exception ex)
            {
                throw new Exception(nombreClase + ".EstablecerSeguridad:" + ex.Message);
            }
        }
        /// <summary>
        /// Verifica si existe una acción en una lista de acciones proporcionada
        /// </summary>
        /// <param name="acciones">Lista de Acciones</param>
        /// <param name="accion">Acción a verificar</param>
        /// <returns></returns>
        private bool ExisteAccion(List<CatalogoBaseBO> acciones, string accion)
        {
            if (acciones != null && acciones.Count > 0 && acciones.Exists(p => p != null && p.Nombre != null && p.Nombre.Trim().ToUpper() == accion.Trim().ToUpper()))
                return true;

            return false;
        }

        #region Métodos para la cancelación de pagos
        public void SolicitarAutorizacion()
        {
            string s;
            if ((s = this.ValidarCamposCancelarPago()) != null)
            {
                this.Vista.MostrarMensaje(s, ETipoMensajeIU.ADVERTENCIA, null);
                this.Vista.PermitirValidarCodigoAutorizacion(false);
                this.Vista.PermitirSolicitarCodigoAutorizacion(true);
                return;
            }

            try
            {
                this.Vista.CancelaPagoCodigoAutorizacion = null;
                this.Vista.CancelaPagoCodigoAutorizacion = this.ControladorPSL.SolicitarAutorizacionCancelarPago(this.dataContext, this.Vista.PagoACancelarID, this.Vista.ModuloID, this.Vista.UnidadOperativaID, this.Vista.UsuarioID, this.Vista.MotivoCancelarPago, this.Vista.Adscripcion.UnidadOperativa.Empresa.Nombre);
                this.Vista.PermitirValidarCodigoAutorizacion(true);
                this.Vista.PermitirSolicitarCodigoAutorizacion(false);
            }
            catch (Exception ex)
            {
                throw new Exception(nombreClase + ".SolicitarAutorizacionTarifaPersonalizada: " + ex.Message);
            }
        }

        private string ValidarCamposCancelarPago()
        {
            string s = string.Empty;

            if (this.Vista.MotivoCancelarPago == null)
                s += "Motivo, ";
            

            if (s.Trim().CompareTo("") != 0)
                return "Los siguientes campos no pueden estar vacíos: \n" + s.Substring(0, s.Length - 2);

            return null;
        }

        /// <summary>
        /// Valida la confirmación del código con el código enviado en la autorización
        /// </summary>
        /// <param name="confirmacionCodigo">Confirmación del código de autorización</param>
        /// <returns>True en caso de que sea correcto, False en caso contrario</returns>
        public bool ValidarCodigoAutorizacion(string confirmacionCodigo)
        {
            if (confirmacionCodigo.Trim().CompareTo(this.Vista.CancelaPagoCodigoAutorizacion) == 0)
                return true;
            return false;
        }

        /// <summary>
        /// Limpia los campos del dialogo y 
        /// Copia los datos de tarifa de la interfaz de usuario
        /// </summary>
        public void PrepararDialogo()
        {

            this.Vista.CancelaPagoCodigoAutorizacion = null;
            this.Vista.MotivoCancelarPago = null;
            this.Vista.PagoACancelarID = null;

            this.Vista.PermitirValidarCodigoAutorizacion(false);
            this.Vista.PermitirSolicitarCodigoAutorizacion(true);

        }

        /// <summary>
        /// Cancela el pago
        /// </summary>
        public void CancelarPago()
        {
            #region Se inicia la Transaccion
            dataContext.SetCurrentProvider("Outsourcing");
            Guid firma = Guid.NewGuid();
            try
            {
                dataContext.OpenConnection(firma);
                dataContext.BeginTransaction(firma);
            }

            catch(Exception)
            {
                if (dataContext.ConnectionState == ConnectionState.Open)
                    dataContext.CloseConnection(firma);
                throw new Exception("Se encontraron inconsistencias al cancelar el pago.");
            }
            #endregion

            try
            {
                var usr = new UsuarioBO { Id = Vista.UsuarioID };
                var adscripcion = new AdscripcionBO { UnidadOperativa = new UnidadOperativaBO { Id = Vista.UnidadOperativaID } };
                var seguridadBO = new SeguridadBO(Guid.Empty, usr, adscripcion);
                PagoContratoPSLBR pagoBR = new PagoContratoPSLBR();
                pagoBR.CancelarPago(dataContext, this.Vista.PagoACancelarID, seguridadBO);                
                dataContext.CommitTransaction(firma);
                this.Vista.MostrarMensaje("Se ha eliminado el pago con éxito", ETipoMensajeIU.EXITO);
                //Se actualiza el grid de pagos
                var pago = this.Vista.PagosConsultadosPSL.Where(x => x.PagoContratoID == this.Vista.PagoACancelarID).FirstOrDefault();
                this.Vista.PagosConsultadosPSL.Remove(pago);
                this.Vista.CargarPagosConsultados();
            }
            catch (Exception ex)
            {
                dataContext.RollbackTransaction(firma);
                throw new Exception(nombreClase + ".CancelarPago:" + ex.Message);
            }
            finally
            {
                if (dataContext.ConnectionState == ConnectionState.Open)
                    dataContext.CloseConnection(firma);
            }

        }
        /// <summary>
        /// Establece el identificador del pago a cancelar
        /// </summary>
        /// <param name="pagoACancelarID">Identificar del pago a cancelar</param>
        public void EstablecerPagoACancelar(int? pagoACancelarID)
        {
            this.Vista.PagoACancelarID = pagoACancelarID;
        }
        #endregion

        #endregion

        #endregion
    }
}
