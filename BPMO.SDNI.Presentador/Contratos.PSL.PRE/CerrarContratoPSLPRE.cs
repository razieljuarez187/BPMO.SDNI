
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
using BPMO.SDNI.Contratos.PSL.BO;
using BPMO.SDNI.Contratos.PSL.BR;
using BPMO.SDNI.Contratos.PSL.VIS;
using BPMO.SDNI.Equipos.BO;
using BPMO.SDNI.Facturacion.AplicacionesPago.BR;
using BPMO.SDNI.Facturacion.InfraestructuraPagos.BO;
using BPMO.SDNI.Facturacion.InfraestructuraPagos.BR;

namespace BPMO.SDNI.Contratos.PSL.PRE {
    public class CerrarContratoPSLPRE {
        #region Atributos
        /// <summary>
        /// Nombre de la clase para agregar a los mensajes de Error
        /// </summary>
        private string nombreClase = "CerrarContratoPSLPRE";

        /// <summary>
        /// El DataContext que proveerá acceso a la base de datos
        /// </summary>
        private readonly IDataContext dctx;

        /// <summary>
        /// Presentador de la Barra de Herramientas
        /// </summary>
        private readonly ucHerramientasPSLPRE presentadorHerramientas;

        /// <summary>
        /// Presentador de la Información Contrato
        /// </summary>
        private readonly ucResumenContratoPSLPRE presentadorResumen;

        /// <summary>
        /// Vista sobre la que actua la interfaz
        /// </summary>
        private readonly ICerrarContratoPSLVIS vista;

        /// <summary>
        /// Controlador de contratos de RD
        /// </summary>
        private ContratoPSLBR controlador;

        /// <summary>
        /// Controlador de PagosUnidadContrato
        /// </summary>
        PagoUnidadContratoBR pagosBr;

        /// <summary>
        /// Presentador del UC de cargos adicionales
        /// </summary>
        private readonly ucCargosAdicionalesCierrePSLPRE presentadorCargosA;

        //PagoContratoPSLBR pagosBr;
        #endregion

        #region Constructores
        /// <summary>
        /// Constructor que recibe la vista sobre la que actuará el presentador
        /// </summary>
        /// <param name="vistaActual">vista sobre la que actuará el presentador</param>
        /// <param name="herramientas">Presentador de la barra de herramientas</param>
        /// /// <param name="infoContrato">Presentador de la Información General</param>
        /// <param name="vistadg">Vista de los datos generales de la unidad</param>
        /// <param name="vistaea">Vista de los datos de los equipos aliados</param>
        public CerrarContratoPSLPRE(ICerrarContratoPSLVIS vistaActual, IucHerramientasPSLVIS vistaHerramientas, IucResumenContratoPSLVIS vistaInfoContrato, IucCargosAdicionalesCierrePSLVIS viewCA) {
            try {
                this.vista = vistaActual;

                this.presentadorResumen = new ucResumenContratoPSLPRE(vistaInfoContrato);
                this.presentadorHerramientas = new ucHerramientasPSLPRE(vistaHerramientas);
                this.presentadorCargosA = new ucCargosAdicionalesCierrePSLPRE(viewCA);

                this.controlador = new ContratoPSLBR();
                this.pagosBr = new PagoUnidadContratoBR();
                this.dctx = FacadeBR.ObtenerConexion();
            } catch (Exception ex) {
                this.vista.MostrarMensaje("Inconsistencia en los parámetros de configuración", ETipoMensajeIU.ERROR, this.nombreClase + ".CerrarContratoRDPRE:" + ex.Message);
            }
        }
        #endregion

        #region Métodos
        public void RealizarPrimeraCarga() {
            try {
                ContratoPSLBO contratoSesion = (ContratoPSLBO)this.vista.ObtenerPaqueteNavegacion("UltimoContratoPSLBO");
                if (contratoSesion != null) {
                    this.LimpiarSesion();
                    this.vista.ContratoID = contratoSesion.ContratoID;
                }

                this.EstablecerConfiguracionInicial();
                this.ConsultarCompleto();

                this.presentadorHerramientas.vista.OcultarImprimirPlantilla();
                this.presentadorHerramientas.vista.OcultarImprimirPlantillaCheckList();
                this.presentadorHerramientas.vista.OcultarSolicitudPago();
                this.presentadorHerramientas.DeshabilitarMenuEditar();
                this.presentadorHerramientas.DeshabilitarMenuCerrar();
                this.presentadorHerramientas.DeshabilitarMenuBorrar();
                this.presentadorHerramientas.DeshabilitarMenuImprimir();
                this.presentadorHerramientas.DeshabilitarMenuDocumentos();
                this.presentadorHerramientas.vista.MarcarOpcionCerrarContrato();
                this.presentadorHerramientas.vista.OcultarPlantillas();

                this.EstablecerTipoCierre();
                this.EstablecerSeguridad();
                this.vista.EstablecerEtiquetas();
            } catch (Exception ex) {
                throw new Exception(this.nombreClase + ".RealizarPrimeraCarga: " + ex.Message);
            }
        }
        private void EstablecerDatosNavegacion(object paqueteNavegacion) {
            try {
                if (paqueteNavegacion == null)
                    throw new Exception("Se esperaba un objeto en la navegación. No se puede identificar qué reservación se desea consultar.");
                if (!(paqueteNavegacion is ContratoPSLBO))
                    throw new Exception("Se esperaba un Contrato de Renta Ordinaria.");

                ContratoPSLBO bo = (ContratoPSLBO)paqueteNavegacion;

                this.DatoAInterfazUsuario(bo);
            } catch (Exception ex) {
                this.DatoAInterfazUsuario(new ContratoPSLBO());
                throw new Exception(this.nombreClase + ".EstablecerDatosNavegacion: " + ex.Message);
            }
        }
        private void ConsultarCompleto() {
            try {
                //Se consulta la información del contrato
                ContratoPSLBO bo = (ContratoPSLBO)this.InterfazUsuarioADato();
                List<ContratoPSLBO> lst = this.controlador.ConsultarCompleto(this.dctx, bo, true);

                if (lst.Count < 1)
                    throw new Exception("No se encontró ningún registro que corresponda a la información proporcionada.");
                if (lst.Count > 1)
                    throw new Exception("La consulta devolvió más de un registro.");

                this.DatoAInterfazUsuario(lst[0]);
                this.vista.UltimoObjeto = lst[0];
            } catch (Exception ex) {
                this.DatoAInterfazUsuario(new ContratoPSLBO());
                throw new Exception(this.nombreClase + ".ConsultarCompleto:" + ex.Message);
            }
        }
        private void EstablecerConfiguracionInicial() {
            try {
                this.vista.FUA = DateTime.Now;
                this.vista.UUA = this.vista.UsuarioID;
                this.vista.FechaCierre = this.vista.FUA;
            } catch (Exception ex) {
                throw new Exception(this.nombreClase + ".EstablecerConfiguracionInicial: " + ex.Message);
            }
        }
        private void EstablecerTipoCierre() {
            EEstatusContrato? estatus = null;
            if (this.vista.EstatusID != null)
                estatus = (EEstatusContrato)Enum.Parse(typeof(EEstatusContrato), this.vista.EstatusID.ToString());

            this.vista.PermitirCerrar(true);
        }

        /// <summary>
        /// Valida el acceso a la página de edición
        /// </summary>
        public void ValidarAcceso() {
            try {
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
            } catch (Exception ex) {
                throw new Exception(this.nombreClase + ".ValidarAcceso: " + ex.Message);
            }
        }
        /// <summary>
        /// Permite configurar el comportamiento de los controles de la vista de acuerdo a los permisos configurados
        /// </summary>
        private void EstablecerSeguridad() {
            try {
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
            } catch (Exception ex) {

                throw new Exception(this.nombreClase + ".EstablecerSeguridad:" + ex.Message);
            }
        }
        /// <summary>
        /// Valida una acción en especifico dentro de la lista de acciones permitidas para la pagina
        /// </summary>
        /// <param name="acciones">Listado de acciones permitidas para la página</param>
        /// <param name="accion">Acción que se desea validar</param>
        /// <returns>si la acción a evaluar se encuentra dentro de la lista de acciones permitidas se devuelve true. En caso contario false. bool</returns>
        private bool ExisteAccion(List<CatalogoBaseBO> acciones, string nombreAccion) {
            if (acciones != null && acciones.Count > 0 &&
                acciones.Exists(
                    p => p != null && p.Nombre != null && p.Nombre.Trim().ToUpper() == nombreAccion.Trim().ToUpper()))
                return true;

            return false;
        }

        public void CancelarEdicion() {
            ContratoPSLBO bo = (ContratoPSLBO)this.vista.UltimoObjeto;
            this.LimpiarSesion();

            this.vista.LimpiarPaqueteNavegacion("UltimoContratoPSLBO");
            this.vista.EstablecerPaqueteNavegacion("ContratoPSLBO", bo);

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

                ////Se obtiene la información a partir de la Interfaz de Usuario
                ContratoPSLBO bo = (ContratoPSLBO)this.InterfazUsuarioADato();

                GeneradorPagoPSLBR GenerarPagosBr = new GeneradorPagoPSLBR();
                var pagos = ObtenerPagosContrato(bo.ContratoID);
                ////Se crea el objeto de seguridad
                UsuarioBO usuario = new UsuarioBO() { Id = this.vista.UsuarioID };
                AdscripcionBO adscripcion = new AdscripcionBO()
                {
                    UnidadOperativa = new UnidadOperativaBO() { Id = this.vista.UnidadOperativaID }
                };
                SeguridadBO seguridadBO = new SeguridadBO(Guid.Empty, usuario, adscripcion);

                //Crear pago de adicional
                var ultimoPago = 0;
                if (pagos.Count > 0)
                    ultimoPago = (int)ObtenerUltimoNumeroPago(pagos);
                GenerarPagosBr.ModuloID = this.vista.ModuloID;
                GenerarPagosBr.GenerarPagoAdicional(dctx, bo, (ultimoPago + 1), seguridadBO, true, true);

                //Se actualiza en la base de datos
                this.controlador.Terminar(this.dctx, bo, (ContratoPSLBO)this.vista.UltimoObjeto, seguridadBO);                
                dctx.CommitTransaction(firma);
                
                this.LimpiarSesion();
                this.vista.LimpiarPaqueteNavegacion("UltimoContratoPSLBO");
                this.vista.EstablecerPaqueteNavegacion("ContratoPSLBO", bo);
                this.vista.RedirigirADetalles();
            } catch (Exception ex) {
                dctx.RollbackTransaction(firma);
                throw new Exception(nombreClase + ".CerrarContrato:" + ex.Message);
            } finally {
                if (dctx.ConnectionState == ConnectionState.Open)
                    dctx.CloseConnection(firma);
            }
        }

        private object InterfazUsuarioADato() {
            ContratoPSLBO bo = new ContratoPSLBO();
            if (this.vista.UltimoObjeto != null)
                bo = new ContratoPSLBO((ContratoPSLBO)this.vista.UltimoObjeto);

            bo.CierreContrato = new CierreContratoPSLBO();
            bo.CierreContrato.Usuario = new UsuarioBO();
            bo.ContratoID = this.vista.ContratoID;
            bo.UUA = this.vista.UUA;
            bo.FUA = this.vista.FUA;

            if (this.vista.EstatusID != null)
                bo.Estatus = (EEstatusContrato)Enum.Parse(typeof(EEstatusContrato), this.vista.EstatusID.ToString());
            else
                bo.Estatus = null;

            bo.CierreContrato.Fecha = this.vista.FechaCierre;
            bo.CierreContrato.Observaciones = this.vista.ObservacionesCierre;
            bo.CierreContrato.Usuario.Id = this.vista.UsuarioID;

            if (bo.CierreContrato is CierreContratoPSLBO) {
                ((CierreContratoPSLBO)bo.CierreContrato).ImporteReembolso = this.vista.ImporteReembolso;
                ((CierreContratoPSLBO)bo.CierreContrato).PersonaRecibeReembolso = this.vista.PersonaRecibeReembolso;
            } else {
                ((CierreContratoPSLBO)bo.CierreContrato).ImporteReembolso = null;
                ((CierreContratoPSLBO)bo.CierreContrato).PersonaRecibeReembolso = null;
            }

            return bo;
        }
        private void DatoAInterfazUsuario(object obj) {
            ContratoPSLBO bo = (ContratoPSLBO)obj;
            if (bo == null) bo = new ContratoPSLBO();
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
            if (bo.CierreContrato == null) bo.CierreContrato = new CancelacionContratoPSLBO();
            if (bo.CierreContrato.Usuario == null) bo.CierreContrato.Usuario = new UsuarioBO();

            if (bo.LineasContrato != null)
                this.vista.LineasContrato = bo.LineasContrato.ConvertAll(a => (LineaContratoPSLBO)a).Where(x => x.Activo == true).ToList();

            //Cierre del Contrato
            if (bo.CierreContrato is CierreContratoPSLBO) {
                this.vista.ImporteReembolso = ((CierreContratoPSLBO)bo.CierreContrato).ImporteReembolso;
                this.vista.PersonaRecibeReembolso = ((CierreContratoPSLBO)bo.CierreContrato).PersonaRecibeReembolso;
            } else {
                this.vista.ImporteReembolso = null;
                this.vista.PersonaRecibeReembolso = null;
            }

            foreach (var item in bo.LineasContrato) {
                if (item.Activo.HasValue && item.Activo == true) {

                    LineaContratoPSLBO linea = new LineaContratoPSLBO((LineaContratoPSLBO)item);

                    if (linea == null) linea = new LineaContratoPSLBO();
                    if (linea.Equipo == null) linea.Equipo = new UnidadBO();
                    if (linea.Cobrable == null) linea.Cobrable = new TarifaContratoPSLBO();

                    this.vista.EquipoID = linea.Equipo.EquipoID;
                    if (linea.Equipo is UnidadBO)
                        this.vista.UnidadID = ((UnidadBO)linea.Equipo).UnidadID;
                    else
                        this.vista.UnidadID = null;

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
                }

            }
            this.vista.ContratoID = bo.ContratoID;
            if (bo.Estatus != null)
                this.vista.EstatusID = (int)bo.Estatus;
            else
                this.vista.EstatusID = null;

            this.vista.ContratoID = bo.ContratoID;
            this.vista.TipoContrato = (int)bo.Tipo;
            vista.FechaContrato = bo.FechaContrato;
            this.vista.UltimoObjeto = bo;
            this.presentadorHerramientas.DatosAInterfazUsuario(bo);
            this.presentadorResumen.DatosAInterfazUsuario(bo);
        }

        public string ValidarCampos() {
            string s = string.Empty;

            if (this.vista.FUA == null)
                s += "Fecha de Última Modificación, ";
            if (this.vista.UUA == null)
                s += "Usuario de Última Modificación, ";
            if (this.vista.EstatusID == null)
                s += "Estatus, ";
            if (this.vista.ContratoID == null)
                s += "Contrato, ";
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

            return null;
        }
        public string ValidarContrato() {
            string s = string.Empty;

            if (this.vista.EstatusID == null)
                s += "Estatus, ";
            
            if (s != null && s.Trim().CompareTo("") != 0)
                return "Los siguientes campos no pueden estar vacíos: \n" + s.Substring(0, s.Length - 2);

            EEstatusContrato estatus = (EEstatusContrato)Enum.Parse(typeof(EEstatusContrato), this.vista.EstatusID.ToString());
            
            return null;
        }

        private void LimpiarSesion() {
            this.vista.LimpiarSesion();
        }

        #region Obtener los pagos del contrato
        private List<PagoContratoPSLBO> ObtenerPagosContrato(int? contratoId) {
            var result = new List<PagoContratoPSLBO>();

            PagoContratoPSLBR pagosBr = new PagoContratoPSLBR();

            result = pagosBr.Consultar(dctx, new PagoContratoPSLBO { ReferenciaContrato = new ReferenciaContratoBO { ReferenciaContratoID = contratoId } });

            return result.Cast<PagoContratoPSLBO>().ToList();
        }

        private short? ObtenerUltimoNumeroPago(List<PagoContratoPSLBO> pagos) {
            return pagos.OrderByDescending(x => x.NumeroPago).First().NumeroPago;
        }
        #endregion

        /// <summary>
        /// Prepara la vista de cargos adicionales
        /// </summary>
        /// <param name="linea">Línea de Contrato que contiene los datos a mostrar</param>
        public void PrepararCargoAdicional(LineaContratoPSLBO linea) {
            try {
                var contrato = (ContratoPSLBO)this.vista.UltimoObjeto;
                presentadorCargosA.Inicializar(linea, contrato);
                vista.CambiarACargoAdicional();
            } catch (Exception ex) {
                throw new Exception(nombreClase + ".PrepararLinea:" + ex.Message);
            }
        }

        /// <summary>
        /// Prepara la vista de cargos adicionales
        /// </summary>
        public void PrepararCierre() {
            vista.CambiarACierre();
        }

        public void AgregarCargoCierre() {
            try {
                this.AgregarCargoACierre(presentadorCargosA.InterfazUsuarioADatos());
                this.vista.CambiarACierre();
            } catch (Exception ex) {
                throw new Exception(nombreClase + ".AgregarCargoCierre: " + ex.Message);
            }
        }

        public void AgregarCargoACierre(LineaContratoPSLBO linea) {
            try {
                if (linea != null) {
                    var unidad = linea.Equipo as SDNI.Equipos.BO.UnidadBO;
                    if (unidad != null && unidad.UnidadID != null) {
                        // Verificar Unidad en Líneas de Contrato
                        LineaContratoPSLBO lineaRepetida =
                            vista.LineasContrato.Find(li => ((SDNI.Equipos.BO.UnidadBO)li.Equipo).UnidadID == unidad.UnidadID && li.Activo == true);
                        if (lineaRepetida != null) {
                            linea.LineaContratoID = lineaRepetida.LineaContratoID;
                            linea.CargoAbusoOperacion = linea.CargoAbusoOperacion;
                            linea.CargoDisposicionBasura = linea.CargoDisposicionBasura;
                        }

                        vista.LineasContrato = this.vista.LineasContrato;
                    } else
                        throw new Exception("Se requiere una Unidad valida para agregarla al detalle del contrato.");
                } else
                    throw new Exception("No se ha proporcionado una línea de contrato");
            } catch (Exception ex) {
                vista.MostrarMensaje("Inconsistencias al Agregar una Unidad al contrato.", ETipoMensajeIU.ERROR, nombreClase + ".AgregarLineaContrato: " + ex.Message);
            }
        }
        #endregion
    }
}
