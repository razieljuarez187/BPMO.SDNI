using System;
using System.Collections.Generic;
using System.Data;
using BPMO.Basicos.BO;
using BPMO.Facade.SDNI.BR;
using BPMO.Patterns.Creational.DataContext;
using BPMO.Primitivos.Enumeradores;
using BPMO.SDNI.Comun.BO;
using BPMO.SDNI.Contratos.BO;
using BPMO.SDNI.Contratos.PSL.BO;
using BPMO.SDNI.Contratos.PSL.BR;
using BPMO.SDNI.Contratos.PSL.VIS;
using BPMO.SDNI.Equipos.BO;
using BPMO.SDNI.Flota.BO;
using BPMO.SDNI.Flota.PRE;
using BPMO.SDNI.Flota.VIS;
using BPMO.SDNI.Tramites.BO;
using BPMO.SDNI.Tramites.BR;

namespace BPMO.SDNI.Contratos.PSL.PRE {
    public class CancelarContratoPSLPRE {
        #region Atributos
        /// <summary>
        /// Nombre de la clase para agregar a los mensajes de Error
        /// </summary>
        private string nombreClase = "CancelarContratoPSLPRE";

        /// <summary>
        /// El DataContext que proveerá acceso a la base de datos
        /// </summary>
        private readonly IDataContext dctx;

        /// <summary>
        /// Presentador de la Barra de Herramientas
        /// </summary>
        private readonly ucHerramientasPSLPRE presentadorHerramientas;

        /// <summary>
        /// Presentador del UC de Datos Generales
        /// </summary>
        private readonly ucDatosGeneralesElementoPRE presentadorDG;

        /// <summary>
        /// Presentador del UC de Equipos Aliados
        /// </summary>
        private readonly ucEquiposAliadosUnidadPRE presentadorEA;

        /// <summary>
        /// Presentador de la Información Contrato
        /// </summary>
        private readonly ucResumenContratoPSLPRE presentadorResumen;

        /// <summary>
        /// Vista sobre la que actúa la interfaz
        /// </summary>
        private readonly ICancelarContratoPSLVIS vista;

        /// <summary>
        /// Controlador de contratos de PSL
        /// </summary>
        private ContratoPSLBR controlador;

        #endregion

        #region Constructores

        public CancelarContratoPSLPRE(ICancelarContratoPSLVIS vistaActual, IucHerramientasPSLVIS vistaHerramientas, IucResumenContratoPSLVIS vistaInfoContrato, IucDatosGeneralesElementoVIS vistadg, IucEquiposAliadosUnidadVIS vistaea) {
            try {
                this.vista = vistaActual;

                this.presentadorResumen = new ucResumenContratoPSLPRE(vistaInfoContrato);
                this.presentadorDG = new ucDatosGeneralesElementoPRE(vistadg);
                this.presentadorEA = new ucEquiposAliadosUnidadPRE(vistaea);
                this.presentadorHerramientas = new ucHerramientasPSLPRE(vistaHerramientas);

                this.controlador = new ContratoPSLBR();
                this.dctx = FacadeBR.ObtenerConexion();
            } catch (Exception ex) {
                this.vista.MostrarMensaje("Inconsistencia en los parámetros de configuración", ETipoMensajeIU.ERROR, this.nombreClase + ".CancelarContratoPSLPRE:" + ex.Message);
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

                this.PrepararEdicion();
                this.EstablecerConfiguracionInicial();
                this.EstablecerTipoCierre();
                this.ConsultarCompleto();

                this.presentadorHerramientas.vista.OcultarImprimirPlantilla();
                this.presentadorHerramientas.vista.OcultarImprimirPlantillaCheckList();
                this.presentadorHerramientas.vista.DeshabilitarOpcionesEditarContratoPSL();
                this.presentadorHerramientas.DeshabilitarMenuEditar();
                this.presentadorHerramientas.DeshabilitarMenuCerrar();
                this.presentadorHerramientas.DeshabilitarMenuBorrar();
                this.presentadorHerramientas.DeshabilitarMenuImprimir();
                this.presentadorHerramientas.DeshabilitarMenuDocumentos();
                this.presentadorHerramientas.vista.MarcarOpcionCerrarContrato();
                this.presentadorHerramientas.vista.OcultarPlantillas();

                this.EstablecerSeguridad();
                this.EstablecerAcciones();
            } catch (Exception ex) {
                throw new Exception(this.nombreClase + ".RealizarPrimeraCarga: " + ex.Message);
            }
        }

        private void EstablecerDatosNavegacion(object paqueteNavegacion) {
            try {
                if (paqueteNavegacion == null)
                    if (paqueteNavegacion == null)
                        throw new Exception("Se esperaba un objeto en la navegación. No se puede identificar qué reservación se desea consultar.");
                if (!(paqueteNavegacion is ContratoPSLBO))
                    throw new Exception("Se esperaba un Contrato PSL.");

                ContratoPSLBO bo = (ContratoPSLBO)paqueteNavegacion;

                this.DatoAInterfazUsuario(bo);

            } catch (Exception ex) {
                this.DatoAInterfazUsuario(new ContratoPSLBO());
                throw new Exception(this.nombreClase + ".EstablecerDatosNavegacion: " + ex.Message);
            }
        }

        private void ConsultarCompleto() {
            try {
                ContratoPSLBO bo = (ContratoPSLBO)this.InterfazUsuarioADato();
                List<ContratoPSLBO> lst = this.controlador.ConsultarCompleto(this.dctx, bo, true);

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
                this.DatoAInterfazUsuario(new ContratoPSLBO());
                throw new Exception(this.nombreClase + ".ConsultarCompleto:" + ex.Message);
            }
        }

        private void EstablecerConfiguracionInicial() {
            this.vista.FUA = DateTime.Now;
            this.vista.UUA = this.vista.UsuarioID;
            this.vista.FechaCancelacion = this.vista.FUA;
        }

        private void EstablecerTipoCierre() {
            EEstatusContrato? estatus = null;

            if (this.vista.EstatusID != null)
                estatus = (EEstatusContrato)Enum.Parse(typeof(EEstatusContrato), this.vista.EstatusID.ToString());

            //En cualquiera de estos casos, es Cancelación
            bool casoPermitido1 = estatus != null && estatus == EEstatusContrato.EnPausa;
            bool casoPermitido2 = estatus != null && estatus == EEstatusContrato.PendientePorCerrar;

            bool cancelable = casoPermitido1 || casoPermitido2;

            this.vista.PermitirCancelar(cancelable);
        }

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
                if (!FacadeBR.ExisteAccion(this.dctx, "CANCELAR", seguridadBO))
                    this.vista.RedirigirSinPermisoAcceso();
            } catch (Exception ex) {
                throw new Exception(this.nombreClase + ".ValidarAcceso: " + ex.Message);

            }
        }

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

        public void CancelarContrato() {
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

                ContratoPSLBO bo = (ContratoPSLBO)this.InterfazUsuarioADato();

                //Se crea el objeto de seguridad
                UsuarioBO usuario = new UsuarioBO() { Id = this.vista.UsuarioID };
                AdscripcionBO adscripcion = new AdscripcionBO() { UnidadOperativa = new UnidadOperativaBO() { Id = this.vista.UnidadOperativaID } };
                SeguridadBO seguridadBO = new SeguridadBO(Guid.Empty, usuario, adscripcion);

                this.controlador.CancelarCompleto(this.dctx, bo, (ContratoPSLBO)this.vista.UltimoObjeto, seguridadBO);

                //Se concluye la transacción.
                dctx.CommitTransaction(firma);

                this.LimpiarSesion();
                this.vista.LimpiarPaqueteNavegacion("UltimoContratoPSLBO");
                this.vista.EstablecerPaqueteNavegacion("ChecklistRecepcionPSL", this.vista.DatosReporte);
                this.vista.RedirigirAImprimir();
            } catch (Exception ex) {
                dctx.RollbackTransaction(firma);
                throw new Exception(nombreClase + ".CancelarContratoPSLPRE:" + ex.Message);
            } finally {
                if (dctx.ConnectionState == ConnectionState.Open)
                    dctx.CloseConnection(firma);
            }
        }

        private object InterfazUsuarioADato() {
            ContratoPSLBO bo = new ContratoPSLBO();
            if (this.vista.UltimoObjeto != null)
                bo = new ContratoPSLBO((ContratoPSLBO)this.vista.UltimoObjeto);

            bo.CierreContrato = new CancelacionContratoPSLBO();
            bo.CierreContrato.Usuario = new UsuarioBO();
            bo.ContratoID = this.vista.ContratoID;
            bo.UUA = this.vista.UUA;
            bo.FUA = this.vista.FUA;
            bo.Tipo = (ETipoContrato)this.vista.TipoContrato;

            if (this.vista.EstatusID != null)
                bo.Estatus = (EEstatusContrato)Enum.Parse(typeof(EEstatusContrato), this.vista.EstatusID.ToString());
            else
                bo.Estatus = null;

            bo.CierreContrato.Fecha = this.vista.FechaCancelacion;
            bo.CierreContrato.Observaciones = this.vista.ObservacionesCancelacion;
            bo.CierreContrato.Usuario.Id = this.vista.UsuarioID;

            ((CancelacionContratoPSLBO)bo.CierreContrato).Motivo = this.vista.MotivoCancelacion;
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

            LineaContratoPSLBO linea = bo.ObtenerLineaContrato();
            if (linea == null) linea = new LineaContratoPSLBO();
            if (linea.Equipo == null) linea.Equipo = new UnidadBO();
            if (linea.Cobrable == null) linea.Cobrable = new TarifaContratoPSLBO();

            this.vista.ContratoID = bo.ContratoID;
            if (bo.Estatus != null)
                this.vista.EstatusID = bo.Estatus;
            else
                this.vista.EstatusID = null;
            this.vista.FechaCancelacion = bo.CierreContrato.Fecha;
            this.vista.ObservacionesCancelacion = bo.CierreContrato.Observaciones;

            if (bo.CierreContrato is CancelacionContratoPSLBO)
                this.vista.MotivoCancelacion = ((CancelacionContratoPSLBO)bo.CierreContrato).Motivo;
            else
                this.vista.MotivoCancelacion = null;

            this.vista.EquipoID = linea.Equipo.EquipoID;
            if (linea.Equipo is UnidadBO)
                this.vista.UnidadID = ((UnidadBO)linea.Equipo).UnidadID;
            else
                this.vista.UnidadID = null;

            this.vista.TipoContrato = (int)bo.Tipo;
            if (bo.Tipo is ETipoContrato)
                this.vista.TipoContrato = (int)bo.Tipo;
            else
                this.vista.TipoContrato = null;

            vista.FechaContrato = bo.FechaContrato;

            this.presentadorHerramientas.DatosAInterfazUsuario(bo);
            this.presentadorResumen.DatosAInterfazUsuario(bo);

        }

        private void PrepararEdicion() {
            this.vista.PrepararEdicion();
            this.presentadorDG.Inicializar();
            this.presentadorEA.Inicializar();
            this.presentadorHerramientas.Inicializar();
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
            if (string.IsNullOrEmpty(this.vista.ObservacionesCancelacion) || string.IsNullOrWhiteSpace(this.vista.ObservacionesCancelacion))
                s += "Observaciones Cancelación, ";
            if (this.vista.FechaCancelacion == null)
                s += "Fecha Cancelación, ";
            if (string.IsNullOrEmpty(this.vista.MotivoCancelacion) || string.IsNullOrWhiteSpace(this.vista.MotivoCancelacion))
                s += "Motivo Cancelación, ";

            if (s != null && s.Trim().CompareTo("") != 0)
                return "Los siguientes campos no pueden estar vacíos: \n" + s.Substring(0, s.Length - 2);

            if (vista.FechaCancelacion < DateTime.Today)
                return "La fecha de cierre no puede ser menor a la fecha actual";
            if (vista.FechaCancelacion < vista.FechaContrato)
                return "La fecha de cierre no puede ser menor a la fecha del contrato";

            if ((s = this.ValidarContrato()) != null)
                return s;

            return null;
        }

        public string ValidarContrato() {
            string s = string.Empty;
            EEstatusContrato estatus = (EEstatusContrato)Enum.Parse(typeof(EEstatusContrato), this.vista.EstatusID.ToString());

            if (this.vista.EstatusID == null)
                s += "Estatus, ";
            if (s != null && s.Trim().CompareTo("") != 0)
                return "Los siguientes campos no pueden estar vacíos: \n" + s.Substring(0, s.Length - 2);

            if (!(estatus == EEstatusContrato.EnPausa || estatus == EEstatusContrato.PendientePorCerrar))
                return "El contrato no puede cancelarse a menos que esté En Pausa o Pendiente por Cerrar pero con 0 kilómetros recorridos entre la entrega y recepción de la unidad.";

            return null;
        }

        private void LimpiarSesion() {
            this.vista.LimpiarSesion();
        }

        #region REQ 13285 Método relacionado con las acciones dependiendo de la unidad operativa.
        /// <summary>
        /// Invoca el método EstablecerAcciones de la vista  IConsultarActaNacimientoVIS.
        /// </summary>
        public void EstablecerAcciones() {
            ETipoEmpresa EmpresaConPermiso = ETipoEmpresa.Idealease;
            switch (this.vista.UnidadOperativaID) {
                case (int)ETipoEmpresa.Generacion:
                    EmpresaConPermiso = ETipoEmpresa.Generacion;
                    break;
                case (int)ETipoEmpresa.Construccion:
                    EmpresaConPermiso = ETipoEmpresa.Construccion;
                    break;
                case (int)ETipoEmpresa.Equinova:
                    EmpresaConPermiso = ETipoEmpresa.Equinova;
                    break;
            }
            this.vista.EstablecerAcciones(EmpresaConPermiso);
        }
        #endregion
        #endregion
    }
}