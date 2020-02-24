// Satisface al CU013 - Cerrar Contrato Renta Diaria
// Satisface a la solicitud de Cambio SC0001
// Satisface a la solicitud de Cambio SC0010
//Satisface a la RI0014
// BEP1401 Satisface a la SC0026
// BEP1401 Satisface a la SC0032

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using BPMO.Basicos.BO;
using BPMO.Facade.SDNI.BR;
using BPMO.Patterns.Creational.DataContext;
using BPMO.Primitivos.Enumeradores;
using BPMO.SDNI.Comun.BO;
using BPMO.SDNI.Comun.BR;
using BPMO.SDNI.Contratos.BO;
using BPMO.SDNI.Contratos.RD.BO;
using BPMO.SDNI.Contratos.RD.BR;
using BPMO.SDNI.Contratos.RD.VIS;
using BPMO.SDNI.Equipos.BO;
using BPMO.SDNI.Facturacion.AplicacionesPago.BR;
using BPMO.SDNI.Facturacion.InfraestructuraPagos.BO;
using BPMO.SDNI.Facturacion.InfraestructuraPagos.BR;
using BPMO.SDNI.Flota.BO;
using BPMO.SDNI.Flota.PRE;
using BPMO.SDNI.Flota.VIS;
using BPMO.SDNI.Tramites.BO;
using BPMO.SDNI.Tramites.BR;
using BPMO.SDNI.Facturacion.InfraestructuraPagos.BOF;

namespace BPMO.SDNI.Contratos.RD.PRE
{
    public class CerrarContratoRDPRE
    {
        #region Atributos
        /// <summary>
        /// Nombre de la clase para agregar a los mensajes de Error
        /// </summary>
        private string nombreClase = "CerrarContratoRDPRE";

        /// <summary>
        /// El DataContext que proveerá acceso a la base de datos
        /// </summary>
        private readonly IDataContext dctx;

        /// <summary>
        /// Presentador de la Barra de Herramientas
        /// </summary>
        private readonly ucHerramientasRDPRE presentadorHerramientas;

        /// <summary>
        /// Presentador de la Información Contrato
        /// </summary>
        private readonly ucResumenContratoRDPRE presentadorResumen;

        /// <summary>
        /// Presentador del UC de datoas generales
        /// </summary>
        private readonly ucDatosGeneralesElementoPRE presentadorDG;

        /// <summary>
        /// Presentador del UC de euipos aliados
        /// </summary>
        private readonly ucEquiposAliadosUnidadPRE presentadorEA;

        /// <summary>
        /// Vista sobre la que actua la interfaz
        /// </summary>
        private readonly ICerrarContratoRDVIS vista;

        /// <summary>
        /// Controlador de contratos de RD
        /// </summary>
        private ContratoRDBR controlador;

        /// <summary>
        /// Controlador de PagosUnidadContrato
        /// </summary>
        PagoUnidadContratoBR pagosBr;
        #endregion

        #region Constructores
        /// <summary>
        /// Constructor que recibe la vista sobre la que actuará el presentador
        /// </summary>
        /// <param name="vistaActual">vista sobre la que actuará el presentador</param>
        /// <param name="herramientas">Presentador de la barra de herramientas</param>
        /// /// <param name="infoContrato">Presentador de la Informacion General</param>
        /// <param name="vistadg">Vista de los datos generales de la unidad</param>
        /// <param name="vistaea">Vista de los datos de los equipos aliados</param>
        public CerrarContratoRDPRE(ICerrarContratoRDVIS vistaActual, IucHerramientasRDVIS vistaHerramientas, IucResumenContratoRDVIS vistaInfoContrato, IucDatosGeneralesElementoVIS vistadg, IucEquiposAliadosUnidadVIS vistaea)
        {
            try
            {
                this.vista = vistaActual;

                this.presentadorResumen = new ucResumenContratoRDPRE(vistaInfoContrato);
                this.presentadorDG = new ucDatosGeneralesElementoPRE(vistadg);
                this.presentadorEA = new ucEquiposAliadosUnidadPRE(vistaea);
                this.presentadorHerramientas = new ucHerramientasRDPRE(vistaHerramientas);

                this.controlador = new ContratoRDBR();
                this.pagosBr = new PagoUnidadContratoBR();
                this.dctx = FacadeBR.ObtenerConexion();
            }
            catch (Exception ex)
            {
                this.vista.MostrarMensaje("Inconsistencia en los parámetros de configuración", ETipoMensajeIU.ERROR, this.nombreClase + ".CerrarContratoRDPRE:" + ex.Message);
            }
        }
        #endregion

        #region Métodos
        public void RealizarPrimeraCarga() {
            try {   
                this.PrepararEdicion();

                this.EstablecerDatosNavegacion(this.vista.ObtenerPaqueteNavegacion("UltimoContratoRDBO"));                               
                this.EstablecerConfiguracionInicial();

                this.presentadorHerramientas.vista.OcultarImprimirPlantilla();
                this.presentadorHerramientas.vista.OcultarImprimirPlantillaCheckList();
                this.presentadorHerramientas.vista.DeshabilitarOpcionesEditarContratoRD();
                this.presentadorHerramientas.DeshabilitarMenuEditar();
                this.presentadorHerramientas.DeshabilitarMenuCerrar();
                this.presentadorHerramientas.DeshabilitarMenuBorrar();
                this.presentadorHerramientas.DeshabilitarMenuImprimir();
                this.presentadorHerramientas.DeshabilitarMenuDocumentos();
                this.presentadorHerramientas.vista.MarcarOpcionCerrarContrato();
                this.presentadorHerramientas.vista.OcultarPlantillas();

                this.EstablecerTipoCierre();
                this.EstablecerSeguridad();
            }
            catch (Exception ex) {
                throw new Exception(this.nombreClase + ".RealizarPrimeraCarga: " + ex.Message);
            }
        }
        /// <summary>
        /// Valida el contratoRDBO en session para mostrarlo en la UI
        /// </summary>
        /// <param name="paqueteNavegacion"></param>
        private void EstablecerDatosNavegacion(object paqueteNavegacion) {
            try {
                if (paqueteNavegacion == null)
                    throw new Exception("Se esperaba un objeto en la navegación. No se puede identificar qué contrato se desea cerrar.");
                if (!(paqueteNavegacion is ContratoRDBO))
                    throw new Exception("Se esperaba un Contrato de Renta Diaria.");

                ContratoRDBO bo = (ContratoRDBO)paqueteNavegacion;
                
                if (!bo.FC.HasValue)
                    this.ConsultarCompleto();

                this.DatoAInterfazUsuario(bo);
                this.vista.UltimoObjeto = bo;
            }
            catch (Exception ex) {
                throw new Exception(this.nombreClase + ".EstablecerDatosNavegacion: " + ex.Message);
            }
        }
        private void ConsultarCompleto() {
            try {
                //Se consulta la información del contrato
                ContratoRDBO bo = (ContratoRDBO)this.InterfazUsuarioADato();

                List<ContratoRDBO> lst = this.controlador.ConsultarCompleto(this.dctx, bo, true);

                if (lst.Count < 1)
                    throw new Exception("No se encontró ningún registro que corresponda a la información proporcionada.");
                if (lst.Count > 1)
                    throw new Exception("La consulta devolvió más de un registro.");

                this.DatoAInterfazUsuario(lst[0]);
                this.vista.UltimoObjeto = lst[0];

                //Se consulta la información de la flota
                if (lst[0].ObtenerLineaContrato() != null && lst[0].ObtenerLineaContrato().Equipo != null && lst[0].ObtenerLineaContrato().Equipo is UnidadBO && ((UnidadBO)lst[0].ObtenerLineaContrato().Equipo).UnidadID != null) {
                    ElementoFlotaBO elemento = new ElementoFlotaBO() { Unidad = (UnidadBO)lst[0].ObtenerLineaContrato().Equipo };
                    elemento.Tramites = new TramiteBR().ConsultarCompleto(this.dctx, new TramiteProxyBO() { Activo = true, Tramitable = elemento.Unidad }, false);
                    if (elemento != null && elemento.Unidad != null && elemento.Unidad.Sucursal == null) elemento.Unidad.Sucursal = new SucursalBO();

                    this.presentadorDG.DatoAInterfazUsuario(elemento as object);
                    this.presentadorEA.DatoAInterfazUsuario(elemento as object);
                    this.presentadorEA.CargarEquiposAliados();
                } else {
                    this.presentadorDG.Inicializar();
                    this.presentadorEA.Inicializar();
                }
            } catch (Exception ex) {
                this.DatoAInterfazUsuario(new ContratoRDBO());
                throw new Exception(this.nombreClase + ".ConsultarCompleto:" + ex.Message);
            }
        }
        private void EstablecerConfiguracionInicial()
        {
            try
            {
                this.vista.FUA = DateTime.Now;
                this.vista.UUA = this.vista.UsuarioID;
                this.vista.FechaCierre = this.vista.FUA;

                //Se obtienen las configuraciones de la unidad operativa
                if (this.vista.UnidadOperativaID == null)
                    throw new Exception("No se identificó en qué unidad operativa trabaja.");
                List<ConfiguracionUnidadOperativaBO> lst = new ModuloBR().ConsultarConfiguracionUnidadOperativa(dctx, new ConfiguracionUnidadOperativaBO() { UnidadOperativa = new UnidadOperativaBO() { Id = this.vista.UnidadOperativaID } }, this.vista.ModuloID);
                if (lst.Count > 0)
                    this.vista.ImporteUnidadCombustible = lst[0].PrecioUnidadCombustible;
                else
                    this.vista.ImporteUnidadCombustible = null;
            }
            catch (Exception ex)
            {
                throw new Exception(this.nombreClase + ".EstablecerConfiguracionInicial: " + ex.Message);
            }
        }
        private void EstablecerTipoCierre()
        {
            int? kmRecorridos = this.vista.KmRecorrido;
            EEstatusContrato? estatus = null;
            if (this.vista.EstatusID != null)
                estatus = (EEstatusContrato)Enum.Parse(typeof(EEstatusContrato), this.vista.EstatusID.ToString());

            //En cualquiera de estos casos, es Cancelación
            bool casoPermitido1 = estatus != null && estatus == EEstatusContrato.EnPausa;
            bool casoPermitido2 = estatus != null && estatus == EEstatusContrato.PendientePorCerrar && kmRecorridos != null && kmRecorridos == 0;
            //En este caso, es Cierre
            bool casoPermitido3 = estatus != null && estatus == EEstatusContrato.PendientePorCerrar && kmRecorridos != null && kmRecorridos > 0;

            bool cancelable = casoPermitido1 || casoPermitido2;
            bool cerrable = casoPermitido3;

            if (cancelable)
            {
                this.LimpiarSesion();
                this.vista.EstablecerPaqueteNavegacion("UltimoContratoRDBO", this.vista.UltimoObjeto);
                this.vista.RedirigirACancelar();
            }
            this.vista.PermitirCerrar(cerrable);
        }

        /// <summary>
        /// Valida el acceso a la página de edición
        /// </summary>
        public void ValidarAcceso()
        {
            try
            {
                //Valida que usuario y la unidad operativa no sean nulos
                if (this.vista.UsuarioID == null) throw new Exception("El identificador del usuario no debe ser nulo");
                if (this.vista.UnidadOperativaID == null) throw new Exception("El identificador de la Unidad Operativa no debe ser nulo");
                //Se crea el objeto de seguridad
                UsuarioBO usuario = new UsuarioBO() { Id = this.vista.UsuarioID };
                AdscripcionBO adscripcion = new AdscripcionBO() { UnidadOperativa = new UnidadOperativaBO() { Id = this.vista.UnidadOperativaID } };
                SeguridadBO seguridadBO = new SeguridadBO(Guid.Empty, usuario, adscripcion);

                //Se valida si el usuario tiene permiso a la acción principal
                if (!FacadeBR.ExisteAccion(this.dctx, "TERMINAR", seguridadBO))
                    this.vista.RedirigirSinPermisoAcceso();
            }
            catch (Exception ex)
            {
                throw new Exception(this.nombreClase + ".ValidarAcceso: " + ex.Message);
            }
        }
        /// <summary>
        /// Permite configurar el comportamiento de los controles de la vista de acuerdo a los permisos configurados
        /// </summary>
        private void EstablecerSeguridad()
        {
            try
            {
                //Valida que el usuario y la unidad operativa no sean nulos
                if (this.vista.UsuarioID == null) throw new Exception("El identificador del usuario no debe ser nulo");
                if (this.vista.UnidadOperativaID == null)
                    throw new Exception("El identificador de la Unidad Operativa no debe ser nulo");
                // Creación del objeto seguridad
                UsuarioBO usuario = new UsuarioBO { Id = this.vista.UsuarioID };
                AdscripcionBO adscripcion = new AdscripcionBO
                {
                    UnidadOperativa = new UnidadOperativaBO { Id = this.vista.UnidadOperativaID }
                };
                SeguridadBO seguridad = new SeguridadBO(Guid.Empty, usuario, adscripcion);

                //consulta de acciones a la cual el usuario tiene permisos
                List<CatalogoBaseBO> acciones = FacadeBR.ConsultarAccion(dctx, seguridad);

                // se valida si el usuario tiene permisos para registrar
                if (!this.ExisteAccion(acciones, "UI INSERTAR"))
                    this.vista.PermitirRegistrar(false);
            }
            catch (Exception ex)
            {

                throw new Exception(this.nombreClase + ".EstablecerSeguridad:" + ex.Message);
            }
        }
        /// <summary>
        /// Valida una acción en especifico dentro de la lista de acciones permitidas para la pagina
        /// </summary>
        /// <param name="acciones">Listado de acciones permitidas para la página</param>
        /// <param name="accion">Acción que se desea validar</param>
        /// <returns>si la acción a evaluar se encuentra dentro de la lista de acciones permitidas se devuelve true. En caso contario false. bool</returns>
        private bool ExisteAccion(List<CatalogoBaseBO> acciones, string nombreAccion)
        {
            if (acciones != null && acciones.Count > 0 &&
                acciones.Exists(
                    p => p != null && p.Nombre != null && p.Nombre.Trim().ToUpper() == nombreAccion.Trim().ToUpper()))
                return true;

            return false;
        }

        private void PrepararEdicion()
        {
            this.vista.PrepararEdicion();
            this.presentadorDG.Inicializar();
            this.presentadorEA.Inicializar();
            this.presentadorHerramientas.Inicializar();
        }

        public void CancelarEdicion()
        {
            ContratoRDBO bo = (ContratoRDBO)this.vista.UltimoObjeto;
            this.LimpiarSesion();
            this.vista.LimpiarPaqueteNavegacion("UltimoContratoRDBO");
            this.vista.EstablecerPaqueteNavegacion("ContratoRDBO", bo);

            this.vista.RedirigirADetalles();
        }
        /// <summary>
        /// Actualiza el cierre de un contrato de RD
        /// </summary>
        public void CerrarContrato() {
            string s;
            if ((s = this.ValidarCampos()) != null) {
                this.vista.MostrarMensaje(s, ETipoMensajeIU.ADVERTENCIA, null);
                return;
            }
            Guid firma = Guid.NewGuid();
            try {
                #region Transaccion

                dctx.SetCurrentProvider("Outsourcing");
                dctx.OpenConnection(firma);
                dctx.BeginTransaction(firma);

                #endregion

                //Se obtiene la información a partir de la Interfaz de Usuario
                ContratoRDBO bo = (ContratoRDBO) this.InterfazUsuarioADato();
                IGeneradorPagosBR GenerarPagosBr = new GeneradorPagoRDBR();
                var pagos = ObtenerPagosContrato(bo.ContratoID);
                //Se crea el objeto de seguridad
                UsuarioBO usuario = new UsuarioBO() {Id = this.vista.UsuarioID};
                AdscripcionBO adscripcion = new AdscripcionBO() {
                    UnidadOperativa = new UnidadOperativaBO() {Id = this.vista.UnidadOperativaID}
                };
                SeguridadBO seguridadBO = new SeguridadBO(Guid.Empty, usuario, adscripcion);

                //Crear pago de adicional
                var ultimoPago = (int) ObtenerUltimoNumeroPago(pagos);
                GenerarPagosBr.GenerarPagoAdicional(dctx, bo, (ultimoPago + 1), seguridadBO, true,true);
                //Se actualiza en la base de datos
                this.controlador.Terminar(this.dctx, bo, (ContratoRDBO) this.vista.UltimoObjeto, seguridadBO);
                #region RI0014
                dctx.CommitTransaction(firma);
                #endregion

                this.LimpiarSesion();
                this.vista.LimpiarPaqueteNavegacion("UltimoContratoRDBO");
                this.vista.EstablecerPaqueteNavegacion("ContratoRDBO", bo);
                this.vista.RedirigirADetalles();
            }
            catch (Exception ex) {
                dctx.RollbackTransaction(firma);
                throw new Exception(nombreClase + ".CerrarContrato:" + ex.Message);
            }
            finally {
                if(dctx.ConnectionState == ConnectionState.Open)
                dctx.CloseConnection(firma);
            }
        }

        private object InterfazUsuarioADato()
        {
            ContratoRDBO bo = new ContratoRDBO();
            if (this.vista.UltimoObjeto != null)
                bo = new ContratoRDBO((ContratoRDBO)this.vista.UltimoObjeto);

            if (bo.CierreContrato == null) {
                bo.CierreContrato = new CierreContratoRDBO();
                bo.CierreContrato.Usuario = new UsuarioBO();
            }

            bo.ContratoID = this.vista.ContratoID;
            bo.UUA = this.vista.UUA;
            bo.FUA = this.vista.FUA;
            bo.Tipo = ETipoContrato.RD;

            if (this.vista.EstatusID != null)
                bo.Estatus = (EEstatusContrato)Enum.Parse(typeof(EEstatusContrato), this.vista.EstatusID.ToString());
            else
                bo.Estatus = null;

            bo.CierreContrato.Fecha = this.vista.FechaCierre;
            bo.CierreContrato.Observaciones = this.vista.ObservacionesCierre;
            bo.CierreContrato.Usuario.Id = this.vista.UsuarioID;

            if (bo.CierreContrato is CierreContratoRDBO)
            {
                ((CierreContratoRDBO)bo.CierreContrato).CargoAbusoOperacion = this.vista.CargoAbusoOperacion;
                ((CierreContratoRDBO)bo.CierreContrato).CargoDisposicionBasura = this.vista.CargoDisposicionBasura;
                ((CierreContratoRDBO)bo.CierreContrato).ImporteReembolso = this.vista.ImporteReembolso;
                ((CierreContratoRDBO)bo.CierreContrato).PersonaRecibeReembolso = this.vista.PersonaRecibeReembolso;
                ((CierreContratoRDBO)bo.CierreContrato).PrecioUnidadCombustible = this.vista.ImporteUnidadCombustible;
            }
            else
            {
                ((CierreContratoRDBO)bo.CierreContrato).CargoAbusoOperacion = null;
                ((CierreContratoRDBO)bo.CierreContrato).CargoDisposicionBasura = null;
                ((CierreContratoRDBO)bo.CierreContrato).ImporteReembolso = null;
                ((CierreContratoRDBO)bo.CierreContrato).PersonaRecibeReembolso = null;
                ((CierreContratoRDBO)bo.CierreContrato).PrecioUnidadCombustible = null;
            }

            return bo;
        }

        /// <summary>
        /// Enlaza los datos a la interfaz de usuario
        /// </summary>
        /// <param name="bo"> objeto con los datos necesarios</param>
        private void DatoAInterfazUsuario(ContratoRDBO bo) {

            this.vista.ContratoID = bo.ContratoID;
            this.vista.FechaContrato = bo.FechaContrato;
            if (bo.Estatus != null)
                this.vista.EstatusID = (int)bo.Estatus;
            else
                this.vista.EstatusID = null;
            
            if (bo.CierreContrato == null) bo.CierreContrato = new CancelacionContratoRDBO();
            if (bo.CierreContrato.Usuario == null) bo.CierreContrato.Usuario = new UsuarioBO();

            LineaContratoRDBO linea = bo.ObtenerLineaContrato();
            if (linea != null) {
                if (linea.Equipo != null) {

                    this.vista.EquipoID = linea.Equipo.EquipoID;
                    if (linea.Equipo is UnidadBO){
                        this.vista.UnidadID = ((UnidadBO)linea.Equipo).UnidadID;                        
                         ElementoFlotaBO elemento = new ElementoFlotaBO() { Unidad = (UnidadBO)linea.Equipo };
                         elemento.Tramites = new TramiteBR().ConsultarCompleto(this.dctx, new TramiteProxyBO() { Activo = true, Tramitable = elemento.Unidad }, false);
                        if (elemento != null){                             
                            if(elemento.Unidad != null && elemento.Unidad.Sucursal == null) 
                                elemento.Unidad.Sucursal = linea.Equipo.Sucursal;

                            this.presentadorDG.DatoAInterfazUsuario(elemento as object);
                            this.presentadorEA.DatoAInterfazUsuario(elemento as object);
                            this.presentadorEA.CargarEquiposAliados();
                        }
                    }else
                        this.vista.UnidadID = null;
                }
                if (linea.Equipo.ActivoFijo != null && linea.Equipo.ActivoFijo.CostoSinIva != null) {
                    Decimal? montoDeducibleCalcuado = 0;
                    montoDeducibleCalcuado = linea.Equipo.ActivoFijo.CostoSinIva;
                    var unidad = (UnidadBO)linea.Equipo;
                    if (unidad.EquiposAliados.Count > 0) {
                        montoDeducibleCalcuado = unidad.EquiposAliados.Aggregate(montoDeducibleCalcuado, (monto, equipoAliado) => equipoAliado.ActivoFijo != null ? equipoAliado.ActivoFijo.CostoSinIva != null ? monto + equipoAliado.ActivoFijo.CostoSinIva : monto : monto);
                    }
                    this.vista.ImporteDeposito = bo.CalcularMontoDeposito(montoDeducibleCalcuado.Value);
                } else
                    this.vista.ImporteDeposito = null;
                //Tarifas
                if (linea.Cobrable == null) {
                    var tarifaTemporal = ((TarifaContratoRDBO)linea.Cobrable);
                    this.vista.TarifaKmLibres = tarifaTemporal.KmsLibres;
                    this.vista.TarifaKmExcedido = tarifaTemporal.RangoTarifas != null && tarifaTemporal.RangoTarifas.Any() ? tarifaTemporal.RangoTarifas.First().CargoKm ?? 0 : 0;
                    this.vista.TarifaHrsLibres = tarifaTemporal.HrsLibres;
                    this.vista.TarifaHrsExcedidas = tarifaTemporal.RangoTarifas != null && tarifaTemporal.RangoTarifas.Any() ? tarifaTemporal.RangoTarifas.First().CargoHr ?? 0 : 0;

                }
                //Listados de Verificación
                ListadoVerificacionBO entrega = linea.ObtenerListadoVerificacionPorTipo(ETipoListadoVerificacion.ENTREGA);
                if (entrega == null) entrega = new ListadoVerificacionBO();
                ListadoVerificacionBO recepcion = linea.ObtenerListadoVerificacionPorTipo(ETipoListadoVerificacion.RECEPCION);
                if (recepcion == null) recepcion = new ListadoVerificacionBO();
                this.vista.FechaRecepcion = recepcion.Fecha;

                this.vista.KmRecorrido = linea.CalcularKilometrajeRecorrido();
                this.vista.KmRecorrido = linea.CalcularKilometrajeRecorrido();
                this.vista.KmExcedido = linea.CalcularKilometrosExcedidos();
                this.vista.MontoKmExcedido = linea.CalcularMontoPorKilometrosExcedidos();
                this.vista.HrsExcedidas = linea.CalcularHorasExcedidas();
                this.vista.HrsConsumidas = linea.CalcularHorasConsumidas();
                this.vista.MontoHrsExcedidas = linea.CalcularMontoPorHorasExcedidas();
                this.vista.KmEntrega = entrega.Kilometraje;
                this.vista.HrsEntrega = entrega.Horometro;
                this.vista.KmRecepcion = recepcion.Kilometraje;
                this.vista.HrsRecepcion = recepcion.Horometro;
                this.vista.DiferenciaCombustible = linea.CalcularDiferenciaCombustible();

                if (this.vista.ImporteUnidadCombustible != null)
                    this.vista.ImporteTotalCombustible = linea.CalcularMontoCombustible(this.vista.ImporteUnidadCombustible.Value);
                else
                    this.vista.ImporteUnidadCombustible = null;

                //Cierre del Contrato
                this.vista.ObservacionesCierre = bo.CierreContrato.Observaciones;
                if (bo.CierreContrato is CierreContratoRDBO) {
                    this.vista.ImporteReembolso = ((CierreContratoRDBO)bo.CierreContrato).ImporteReembolso;
                    this.vista.CargoAbusoOperacion = ((CierreContratoRDBO)bo.CierreContrato).CargoAbusoOperacion;
                    this.vista.CargoDisposicionBasura = ((CierreContratoRDBO)bo.CierreContrato).CargoDisposicionBasura;
                    this.vista.PersonaRecibeReembolso = ((CierreContratoRDBO)bo.CierreContrato).PersonaRecibeReembolso;
                } else {
                    this.vista.ImporteReembolso = null;
                    this.vista.CargoAbusoOperacion = null;
                    this.vista.CargoDisposicionBasura = null;
                    this.vista.PersonaRecibeReembolso = null;
                }

                //Cálculos de Días de Renta
                this.vista.DiasRentaProgramada = bo.CalcularDiasPrometidosRenta();
                this.vista.DiasEnTaller = 0; 
                this.vista.DiasRealesRenta = bo.CalcularDiasTranscurridosRenta();
                if (this.vista.DiasRentaProgramada != null && this.vista.DiasRealesRenta != null)
                    this.vista.DiasAdicionales = this.vista.DiasRealesRenta.Value - this.vista.DiasRentaProgramada.Value;
                else
                    this.vista.DiasAdicionales = null;
                if (this.vista.DiasAdicionales != null && ((TarifaContratoRDBO)linea.Cobrable).TarifaDiaria != null)
                    this.vista.MontoTotalDiasAdicionales = this.vista.DiasAdicionales * ((TarifaContratoRDBO)linea.Cobrable).TarifaDiaria;
                else
                    this.vista.MontoTotalDiasAdicionales = null;
            }

            this.presentadorHerramientas.DatosAInterfazUsuario(bo);
            this.presentadorResumen.DatosAInterfazUsuario(bo);
        }

        private void DatoAInterfazUsuario(object obj)
        {
            ContratoRDBO bo = (ContratoRDBO)obj;
            if (bo == null) bo = new ContratoRDBO();
            if (bo.Cliente == null) bo.Cliente = new CuentaClienteIdealeaseBO();
            if (bo.Divisa == null) bo.Divisa = new DivisaBO();
            if (bo.Divisa.MonedaDestino == null) bo.Divisa.MonedaDestino = new MonedaBO();
            if (bo.Operador == null) bo.Operador = new OperadorBO();
            if (bo.Operador.Direccion == null) bo.Operador.Direccion = new DireccionPersonaBO();
            if (bo.Operador.Direccion.Ubicacion == null) bo.Operador.Direccion.Ubicacion = new UbicacionBO();
            if (bo.Operador.Direccion.Ubicacion.Ciudad == null) bo.Operador.Direccion.Ubicacion.Ciudad = new CiudadBO();
            if (bo.Operador.Direccion.Ubicacion.Estado == null) bo.Operador.Direccion.Ubicacion.Estado = new EstadoBO();
            if (bo.Operador.Direccion.Ubicacion.Pais == null) bo.Operador.Direccion.Ubicacion.Pais = new PaisBO();
            if (bo.Operador.Licencia == null) bo.Operador.Licencia = new LicenciaBO();
            if (bo.Sucursal == null) bo.Sucursal = new SucursalBO();
            if (bo.Sucursal.UnidadOperativa == null) bo.Sucursal.UnidadOperativa = new UnidadOperativaBO();
            if (bo.CierreContrato == null) bo.CierreContrato = new CancelacionContratoRDBO();
            if (bo.CierreContrato.Usuario == null) bo.CierreContrato.Usuario = new UsuarioBO();

            LineaContratoRDBO linea = bo.ObtenerLineaContrato();
            if (linea == null) linea = new LineaContratoRDBO();
            if (linea.Equipo == null) linea.Equipo = new UnidadBO();
            if (linea.Cobrable == null) linea.Cobrable = new TarifaContratoRDBO();

            this.vista.ContratoID = bo.ContratoID;
            if (bo.Estatus != null)
                this.vista.EstatusID = (int)bo.Estatus;
            else
                this.vista.EstatusID = null;

            this.vista.EquipoID = linea.Equipo.EquipoID;
            if (linea.Equipo is UnidadBO)
                this.vista.UnidadID = ((UnidadBO)linea.Equipo).UnidadID;
            else
                this.vista.UnidadID = null;
            if (linea.Equipo.ActivoFijo != null && linea.Equipo.ActivoFijo.CostoSinIva != null)
            {
                Decimal? montoDeducibleCalcuado = 0;
                montoDeducibleCalcuado = linea.Equipo.ActivoFijo.CostoSinIva;
                var unidad = (UnidadBO)linea.Equipo;
                if(unidad.EquiposAliados.Count > 0)
                {
                    montoDeducibleCalcuado = unidad.EquiposAliados.Aggregate(montoDeducibleCalcuado, (monto, equipoAliado) => equipoAliado.ActivoFijo != null ? equipoAliado.ActivoFijo.CostoSinIva != null ? monto + equipoAliado.ActivoFijo.CostoSinIva : monto : monto);
                }
                this.vista.ImporteDeposito = bo.CalcularMontoDeposito(montoDeducibleCalcuado.Value);
            }
                
            else
                this.vista.ImporteDeposito = null;

            //Tarifas
            var tarifaTemporal = ((TarifaContratoRDBO) linea.Cobrable);
            this.vista.TarifaKmLibres = tarifaTemporal.KmsLibres;
            this.vista.TarifaKmExcedido = tarifaTemporal.RangoTarifas != null && tarifaTemporal.RangoTarifas.Any() ? tarifaTemporal.RangoTarifas.First().CargoKm ?? 0 : 0;
            this.vista.TarifaHrsLibres = tarifaTemporal.HrsLibres;
            this.vista.TarifaHrsExcedidas = tarifaTemporal.RangoTarifas != null && tarifaTemporal.RangoTarifas.Any() ? tarifaTemporal.RangoTarifas.First().CargoHr ?? 0 : 0;

            //Listados de Verificación
            ListadoVerificacionBO entrega = linea.ObtenerListadoVerificacionPorTipo(ETipoListadoVerificacion.ENTREGA);
            if (entrega == null) entrega = new ListadoVerificacionBO();
            ListadoVerificacionBO recepcion = linea.ObtenerListadoVerificacionPorTipo(ETipoListadoVerificacion.RECEPCION);
            if (recepcion == null) recepcion = new ListadoVerificacionBO();

            this.vista.KmRecorrido = linea.CalcularKilometrajeRecorrido();
            this.vista.KmRecorrido = linea.CalcularKilometrajeRecorrido();
            this.vista.KmExcedido = linea.CalcularKilometrosExcedidos();
            this.vista.MontoKmExcedido = linea.CalcularMontoPorKilometrosExcedidos();
            this.vista.HrsExcedidas = linea.CalcularHorasExcedidas();
            this.vista.HrsConsumidas = linea.CalcularHorasConsumidas();
            this.vista.MontoHrsExcedidas = linea.CalcularMontoPorHorasExcedidas();
            this.vista.KmEntrega = entrega.Kilometraje;
            this.vista.HrsEntrega = entrega.Horometro;
            this.vista.KmRecepcion = recepcion.Kilometraje;
            this.vista.HrsRecepcion = recepcion.Horometro;
            this.vista.DiferenciaCombustible = linea.CalcularDiferenciaCombustible();
            if (this.vista.ImporteUnidadCombustible != null)
                this.vista.ImporteTotalCombustible = linea.CalcularMontoCombustible(this.vista.ImporteUnidadCombustible.Value);
            else
                this.vista.ImporteUnidadCombustible = null;
            this.vista.FechaRecepcion = recepcion.Fecha;

            //Cierre del Contrato
            this.vista.ObservacionesCierre = bo.CierreContrato.Observaciones;
            if (bo.CierreContrato is CierreContratoRDBO)
            {
                this.vista.ImporteReembolso = ((CierreContratoRDBO)bo.CierreContrato).ImporteReembolso;
                this.vista.CargoAbusoOperacion = ((CierreContratoRDBO)bo.CierreContrato).CargoAbusoOperacion;
                this.vista.CargoDisposicionBasura = ((CierreContratoRDBO)bo.CierreContrato).CargoDisposicionBasura;
                this.vista.PersonaRecibeReembolso = ((CierreContratoRDBO)bo.CierreContrato).PersonaRecibeReembolso;
            }
            else
            {
                this.vista.ImporteReembolso = null;
                this.vista.CargoAbusoOperacion = null;
                this.vista.CargoDisposicionBasura = null;
                this.vista.PersonaRecibeReembolso = null;
            }

            //Cálculos de Días de Renta
            this.vista.DiasRentaProgramada = bo.CalcularDiasPrometidosRenta();
            this.vista.DiasEnTaller = 0; //Integración con módulo de mantenimiento
            this.vista.DiasRealesRenta = bo.CalcularDiasTranscurridosRenta();
            if (this.vista.DiasRentaProgramada != null && this.vista.DiasRealesRenta != null)
                this.vista.DiasAdicionales = this.vista.DiasRealesRenta.Value - this.vista.DiasRentaProgramada.Value;
            else
                this.vista.DiasAdicionales = null;
            if (this.vista.DiasAdicionales != null && ((TarifaContratoRDBO)linea.Cobrable).TarifaDiaria != null)
                this.vista.MontoTotalDiasAdicionales = this.vista.DiasAdicionales * ((TarifaContratoRDBO)linea.Cobrable).TarifaDiaria;
            else
                this.vista.MontoTotalDiasAdicionales = null;
            vista.FechaContrato = bo.FechaContrato;
            this.presentadorHerramientas.DatosAInterfazUsuario(bo);
            this.presentadorResumen.DatosAInterfazUsuario(bo);
        }

        public string ValidarCampos()
        {
            string s = string.Empty;

            if (this.vista.FUA == null)
                s += "Fecha de Última Modificación, ";
            if (this.vista.UUA == null)
                s += "Usuario de Última Modificación, ";
            if (this.vista.EstatusID == null)
                s += "Estatus, ";
            if (this.vista.ContratoID == null)
                s += "Contrato, ";
            if (this.vista.ImporteUnidadCombustible == null)
                s += "Importe Unidad Combustible, ";
            if (this.vista.CargoAbusoOperacion == null)
                s += "Cargo Abuso Operación, ";
            if (this.vista.CargoDisposicionBasura == null)
                s += "Cargo Disposición Basura, ";
            if (this.vista.ImporteReembolso == null)
                s += "Importe Reembolso, ";
            if (string.IsNullOrEmpty(this.vista.PersonaRecibeReembolso) || string.IsNullOrWhiteSpace(this.vista.PersonaRecibeReembolso))
                s += "Persona Recibe Reembolso, ";
            if (string.IsNullOrEmpty(this.vista.ObservacionesCierre) || string.IsNullOrWhiteSpace(this.vista.ObservacionesCierre))
                s += "Observaciones Cierre, ";
            if (this.vista.FechaCierre == null)
                s += "Fecha Cierre, ";

            if (s != null && s.Trim().CompareTo("") != 0)
                return "Los siguientes campos no pueden estar vacíos: \n" + s.Substring(0, s.Length - 2);

            if (this.vista.FechaCierre < DateTime.Today)
                return "La fecha de cierre no puede ser menor a la fecha actual";
            if (this.vista.FechaCierre < vista.FechaContrato)
                return "La fecha de cierre no puede ser menor a la fecha del contrato";
            if (this.vista.FechaCierre < vista.FechaRecepcion)
                return "La fecha de cierre no puede ser menor a la fecha de recepción de la unidad";

            if ((s = this.ValidarContrato()) != null)
                return s;

            return null;
        }
        public string ValidarContrato()
        {
            string s = string.Empty;

            if (this.vista.EstatusID == null)
                s += "Estatus, ";
            if (this.vista.EstatusID != null && (EEstatusContrato)Enum.Parse(typeof(EEstatusContrato), this.vista.EstatusID.ToString()) == EEstatusContrato.PendientePorCerrar && this.vista.KmRecorrido == null)
                s += "Kilómetros recorridos, ";

            if (s != null && s.Trim().CompareTo("") != 0)
                return "Los siguientes campos no pueden estar vacíos: \n" + s.Substring(0, s.Length - 2);

            EEstatusContrato estatus = (EEstatusContrato)Enum.Parse(typeof(EEstatusContrato), this.vista.EstatusID.ToString());
            if (!(estatus == EEstatusContrato.PendientePorCerrar && this.vista.KmRecorrido > 0))
                return "El contrato no puede cancelarse a menos que esté Pendiente por Cerrar pero con más de 0 kilómetros recorridos entre la entrega y recepción de la unidad.";

            return null;
        }

        private void LimpiarSesion()
        {
            this.vista.LimpiarSesion();
        }
        #region SC_0010
        private List<PagoUnidadContratoRDBO> ObtenerPagosContrato(int? contratoId)
        {
            var result = new List<PagoUnidadContratoBO>();

            //SC0026, Utilización de clase concreta segun el tipo de contrato
            result = pagosBr.Consultar(dctx,
                              new PagoUnidadContratoBOF
                                  {ReferenciaContrato = new ReferenciaContratoBO {ReferenciaContratoID = contratoId}});

            return result.Cast<PagoUnidadContratoRDBO>().ToList();
        }

        private short? ObtenerUltimoNumeroPago(List<PagoUnidadContratoRDBO> pagos )
        {
            return pagos.OrderByDescending(x => x.NumeroPago).First().NumeroPago;
        }

        #endregion
        #endregion
    }
}
