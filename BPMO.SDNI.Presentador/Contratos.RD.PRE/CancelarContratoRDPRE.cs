// Satisface al CU013 - Cerrar Contrato Renta Diaria
// BEP1401 Satisface a la SC0032
using System;
using System.Collections.Generic;
using System.Data;
using BPMO.Facade.SDNI.BR;

using BPMO.Basicos.BO;
using BPMO.Primitivos.Enumeradores;
using BPMO.Patterns.Creational.DataContext;

using BPMO.SDNI.Comun.BO;
using BPMO.SDNI.Equipos.BO;
using BPMO.SDNI.Flota.BO;
using BPMO.SDNI.Flota.PRE;
using BPMO.SDNI.Flota.VIS;
using BPMO.SDNI.Tramites.BO;
using BPMO.SDNI.Tramites.BR;
using BPMO.SDNI.Contratos.BO;
using BPMO.SDNI.Contratos.RD.BO;
using BPMO.SDNI.Contratos.RD.BR;
using BPMO.SDNI.Contratos.RD.VIS;

namespace BPMO.SDNI.Contratos.RD.PRE
{
    public class CancelarContratoRDPRE
    {
        #region Atributos
        /// <summary>
        /// Nombre de la clase para agregar a los mensajes de Error
        /// </summary>
        private string nombreClase = "CancelarContratoRDPRE";

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
        private readonly ICancelarContratoRDVIS vista;

        /// <summary>
        /// Controlador de contratos de RD
        /// </summary>
        private ContratoRDBR controlador;

        #endregion

        #region Constructores
        /// <summary>
        /// Constructor que recibe la vista sobre la que actuará el presentador
        /// </summary>
        /// <param name="vistaActual">vista sobre la que actuará el presentador</param>
        /// <param name="vistaHerramientas">Vista de la barra de herramientas</param>
        /// <param name="vistaInfoContrato">Vista de la Informacion General</param>
        /// <param name="vistadg">Vista de los datos generales de la unidad</param>
        /// <param name="vistaea">Vista de los datos de los equipos aliados</param>
        public CancelarContratoRDPRE(ICancelarContratoRDVIS vistaActual, IucHerramientasRDVIS vistaHerramientas, IucResumenContratoRDVIS vistaInfoContrato, IucDatosGeneralesElementoVIS vistadg, IucEquiposAliadosUnidadVIS vistaea)
        {
            try
            {
                this.vista = vistaActual;

                this.presentadorResumen = new ucResumenContratoRDPRE(vistaInfoContrato);
                this.presentadorDG = new ucDatosGeneralesElementoPRE(vistadg);
                this.presentadorEA = new ucEquiposAliadosUnidadPRE(vistaea);
                this.presentadorHerramientas = new ucHerramientasRDPRE(vistaHerramientas);

                this.controlador = new ContratoRDBR();
                this.dctx = FacadeBR.ObtenerConexion();
            }
            catch (Exception ex)
            {
                this.vista.MostrarMensaje("Inconsistencia en los parámetros de configuración", ETipoMensajeIU.ERROR, this.nombreClase + ".CancelarContratoRDPRE:" + ex.Message);
            }
        }
        #endregion

        #region Métodos
        public void RealizarPrimeraCarga()
        {
            try
            {
                this.PrepararEdicion();
                this.EstablecerDatosNavegacion(this.vista.ObtenerPaqueteNavegacion("UltimoContratoRDBO"));

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

                this.EstablecerConfiguracionInicial();
                this.EstablecerTipoCierre();
                this.EstablecerSeguridad();
            }
            catch (Exception ex)
            {
                throw new Exception(this.nombreClase + ".RealizarPrimeraCarga: " + ex.Message);
            }
        }
        private void EstablecerDatosNavegacion(object paqueteNavegacion)
        {
            try
            {
                if (paqueteNavegacion == null)
                    throw new Exception("Se esperaba un objeto en la navegación. No se puede identificar qué reservación se desea consultar.");
                if (!(paqueteNavegacion is ContratoRDBO))
                    throw new Exception("Se esperaba un Contrato de Renta Diaria.");

                ContratoRDBO bo = (ContratoRDBO)paqueteNavegacion;

                if (!bo.FC.HasValue)
                    this.ConsultarCompleto();

                this.DatoAInterfazUsuario(bo);
            }
            catch (Exception ex)
            {
                throw new Exception(this.nombreClase + ".EstablecerDatosNavegacion: " + ex.Message);
            }
        }
        private void ConsultarCompleto()
        {
            try
            {
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
                if (lst[0].ObtenerLineaContrato() != null && lst[0].ObtenerLineaContrato().Equipo != null && lst[0].ObtenerLineaContrato().Equipo is UnidadBO && ((UnidadBO)lst[0].ObtenerLineaContrato().Equipo).UnidadID != null)
                {
                    ElementoFlotaBO elemento = new ElementoFlotaBO() { Unidad = (UnidadBO)lst[0].ObtenerLineaContrato().Equipo };
                    elemento.Tramites = new TramiteBR().ConsultarCompleto(this.dctx, new TramiteProxyBO() { Activo = true, Tramitable = elemento.Unidad }, false);
                    if (elemento != null && elemento.Unidad != null && elemento.Unidad.Sucursal == null) elemento.Unidad.Sucursal = new SucursalBO();

                    this.presentadorDG.DatoAInterfazUsuario(elemento as object);
                    this.presentadorEA.DatoAInterfazUsuario(elemento as object);
                    this.presentadorEA.CargarEquiposAliados();
                }
                else
                {
                    this.presentadorDG.Inicializar();
                    this.presentadorEA.Inicializar();
                }
            }
            catch (Exception ex)
            {
                this.DatoAInterfazUsuario(new ContratoRDBO());
                throw new Exception(this.nombreClase + ".ConsultarCompleto:" + ex.Message);
            }
        }
        private void EstablecerConfiguracionInicial()
        {
            this.vista.FUA = DateTime.Now;
            this.vista.UUA = this.vista.UsuarioID;
            this.vista.FechaCancelacion = this.vista.FUA;
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

            if (cerrable)
            {
                this.LimpiarSesion();
                this.vista.EstablecerPaqueteNavegacion("ContratoRDBO", this.vista.UltimoObjeto);
                this.vista.RedirigirACerrar();
            }
            this.vista.PermitirCancelar(cancelable);
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
                if (!FacadeBR.ExisteAccion(this.dctx, "CANCELAR", seguridadBO))
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
        /// Actualiza la Cancelación de un contrato de RD
        /// </summary>
        public void CancelarContrato()
        {
            string s;
            if ((s = this.ValidarCampos()) != null)
            {
                this.vista.MostrarMensaje(s, ETipoMensajeIU.ADVERTENCIA, null);
                return;
            }
            Guid firma = Guid.NewGuid();
            try
            {
                #region Transaccion

                dctx.SetCurrentProvider("Outsourcing");
                dctx.OpenConnection(firma);
                dctx.BeginTransaction(firma);

                #endregion

                //Se obtiene la información a partir de la Interfaz de Usuario
                ContratoRDBO bo = (ContratoRDBO) this.InterfazUsuarioADato();

                //Se crea el objeto de seguridad
                UsuarioBO usuario = new UsuarioBO() {Id = this.vista.UsuarioID};
                AdscripcionBO adscripcion = new AdscripcionBO() {UnidadOperativa = new UnidadOperativaBO() {Id = this.vista.UnidadOperativaID}};
                SeguridadBO seguridadBO = new SeguridadBO(Guid.Empty, usuario, adscripcion);

                //Se actualiza en la base de datos
                this.controlador.Cancelar(this.dctx, bo, (ContratoRDBO) this.vista.UltimoObjeto, seguridadBO);
                
                //Se concluye la transacción.
                dctx.CommitTransaction(firma);

                this.LimpiarSesion();
                this.vista.LimpiarPaqueteNavegacion("UltimoContratoRDBO");
                this.vista.EstablecerPaqueteNavegacion("ContratoRDBO", bo);
                this.vista.RedirigirADetalles();
            }
            catch (Exception ex)
            {
                dctx.RollbackTransaction(firma);
                throw new Exception(nombreClase + ".CancelarContrato:" + ex.Message);
            }
            finally
            {
                if(dctx.ConnectionState == ConnectionState.Open)
                    dctx.CloseConnection(firma);
            }
        }

        private object InterfazUsuarioADato()
        {
            ContratoRDBO bo = new ContratoRDBO();
            if (this.vista.UltimoObjeto != null)
                bo = new ContratoRDBO((ContratoRDBO)this.vista.UltimoObjeto);

            bo.CierreContrato = new CancelacionContratoRDBO();
            bo.CierreContrato.Usuario = new UsuarioBO();

            bo.ContratoID = this.vista.ContratoID;
            bo.UUA = this.vista.UUA;
            bo.FUA = this.vista.FUA;
            bo.Tipo = ETipoContrato.RD;

            if (this.vista.EstatusID != null)
                bo.Estatus = (EEstatusContrato)Enum.Parse(typeof(EEstatusContrato), this.vista.EstatusID.ToString());
            else
                bo.Estatus = null;

            bo.CierreContrato.Fecha = this.vista.FechaCancelacion;
            bo.CierreContrato.Observaciones = this.vista.ObservacionesCancelacion;
            bo.CierreContrato.Usuario.Id = this.vista.UsuarioID;

            ((CancelacionContratoRDBO)bo.CierreContrato).Motivo = this.vista.MotivoCancelacion;

            return bo;
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
            if (linea == null) {
                linea = new LineaContratoRDBO();
                if (linea.Equipo == null) linea.Equipo = new UnidadBO();
                if (linea.Cobrable == null) linea.Cobrable = new TarifaContratoRDBO();
            } else {
                this.vista.KmRecorrido = linea.CalcularKilometrajeRecorrido();
                if (linea.Equipo != null) {
                    this.vista.EquipoID = linea.Equipo.EquipoID;
                    //Se consulta la información de la flota
                    if (linea.Equipo is UnidadBO) {
                        this.vista.UnidadID = ((UnidadBO)linea.Equipo).UnidadID;
                        ElementoFlotaBO elemento = new ElementoFlotaBO() { Unidad = (UnidadBO)linea.Equipo };
                        elemento.Tramites = new TramiteBR().ConsultarCompleto(this.dctx, new TramiteProxyBO() { Activo = true, Tramitable = elemento.Unidad }, false);
                        if (elemento != null && elemento.Unidad != null && elemento.Unidad.Sucursal == null) elemento.Unidad.Sucursal = new SucursalBO();

                        this.presentadorDG.DatoAInterfazUsuario(elemento as object);
                        this.presentadorEA.DatoAInterfazUsuario(elemento as object);
                        this.presentadorEA.CargarEquiposAliados();
                    } else {
                        this.presentadorDG.Inicializar();
                        this.presentadorEA.Inicializar();
                    }
                }
            }
            this.vista.ContratoID = bo.ContratoID;
            if (bo.Estatus != null)
                this.vista.EstatusID = (int)bo.Estatus;
            else
                this.vista.EstatusID = null;
            this.vista.FechaCancelacion = bo.CierreContrato.Fecha;
            this.vista.ObservacionesCancelacion = bo.CierreContrato.Observaciones;

            if (bo.CierreContrato is CancelacionContratoRDBO)
                this.vista.MotivoCancelacion = ((CancelacionContratoRDBO)bo.CierreContrato).Motivo;
            else
                this.vista.MotivoCancelacion = null;

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
            if (!(estatus == EEstatusContrato.EnPausa || estatus == EEstatusContrato.PendientePorCerrar && this.vista.KmRecorrido == 0))
                return "El contrato no puede cancelarse a menos que esté En Pausa o Pendiente por Cerrar pero con 0 kilómetros recorridos entre la entrega y recepción de la unidad.";

            return null;
        }

        private void LimpiarSesion()
        {
            this.vista.LimpiarSesion();
        }
        #endregion 
    }
}
