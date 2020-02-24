//Satisface al CU087 – Catálogo Tramite Unidad
using System;
using System.Collections.Generic;
using BPMO.Basicos.BO;
using BPMO.Facade.SDNI.BR;
using BPMO.Patterns.Creational.DataContext;
using BPMO.Primitivos.Enumeradores;
using BPMO.SDNI.Comun.BO;
using BPMO.SDNI.Comun.BR;
using BPMO.SDNI.Equipos.BO;
using BPMO.SDNI.Equipos.BR;
using BPMO.SDNI.Tramites.BO;
using BPMO.SDNI.Tramites.BR;
using BPMO.SDNI.Tramites.VIS;

namespace BPMO.SDNI.Tramites.PRE
{
    public class RegistrarTramitesPRE
    {
        #region Atributos
        private readonly string nombreClase = "RegistrarTramitesPRE.cs";
        private readonly TipoArchivoBR tipoArchivoBR;
        private readonly UnidadBR unidadBR;
        private readonly IRegistrarTramitesVIS vista;
        private readonly IDataContext dctx = null;
        private readonly ucTramiteFiltroAKPRE presentadorAK;
        private readonly ucTramiteGPSPRE presentadorGPS;
        private readonly ucTramitePlacaPRE presentadorPlaca;
        private readonly ucTramiteTenenciaPRE presentadorTenencia;
        private readonly ucTramiteVerificacionPRE presentadorVerificacion;
        private readonly ucTramiteVerificacionPRE presentadorVerificacionAmbiental;
        private readonly ucTramitePlacaPRE presentadorPlacaEstatal;
        #endregion

        #region Constructores
        public RegistrarTramitesPRE(IRegistrarTramitesVIS vistaActual, ucTramiteFiltroAKPRE vistaAK, ucTramiteGPSPRE vistaGPS,
            ucTramitePlacaPRE vistaPlaca, ucTramiteTenenciaPRE vistaTenencia, ucTramiteVerificacionPRE vistaVeri, 
            ucTramiteVerificacionPRE vistaAmbiental, ucTramitePlacaPRE vistaPlacaEstatal)
        {

            this.vista = vistaActual;
            tipoArchivoBR = new TipoArchivoBR();
            dctx = FacadeBR.ObtenerConexion();
            unidadBR = new UnidadBR();
            presentadorAK = vistaAK;
            presentadorGPS = vistaGPS;
            presentadorPlaca = vistaPlaca;
            presentadorTenencia = vistaTenencia;
            presentadorVerificacion = vistaVeri;
            presentadorPlacaEstatal = vistaPlacaEstatal;
            presentadorVerificacionAmbiental = vistaAmbiental;

        }

        #endregion

        #region Métodos
        public void AsignarNumVIN(ITramitable tramitable)
        {
            try
            {
                if (tramitable != null)
                {
                    UnidadBO unidadTemp = new UnidadBO ();
                    if (unidadTemp.TipoTramitable == tramitable.TipoTramitable)
                    {
                        unidadTemp.UnidadID = tramitable.TramitableID;
                        unidadTemp.NumeroSerie = tramitable.DescripcionTramitable;
                        List<UnidadBO> lstUnidad = unidadBR.ConsultarCompleto(dctx, unidadTemp, true);
                        if (lstUnidad.Count != 0)
                        {
                            if (lstUnidad.Count == 1)
                            {
                                UnidadBO unidad = lstUnidad[0];
                                if (unidad.UnidadID != null)
                                {

                                    if (unidad.Modelo != null)
                                    {
                                        this.vista.Modelo = unidad.Modelo.Nombre;
                                        if (unidad.Modelo.Marca != null)
                                            this.vista.Marca = unidad.Modelo.Marca.Nombre;

                                    }
                                    vista.NumSerie = unidad.NumeroSerie;
                                }
                                else
                                {
                                    this.MostrarMensaje("Error al intentar obtener datos de la unidad", ETipoMensajeIU.ERROR, nombreClase + ".AsignarNumVIN: la consulta regreso una unidad nula");

                                }
                            }
                            else
                            {
                                this.MostrarMensaje("Error al intentar obtener los datos de la unidad", ETipoMensajeIU.ERROR, nombreClase + ".AsignarNumVIN: no devolvió más de una unidad");
                            }
                        }
                        else
                        {
                            this.MostrarMensaje("Error al intentar obtener los datos de la unidad", ETipoMensajeIU.ERROR, nombreClase + ".AsignarNumVIN: no devolvió ningún registro");
                        }
                    }
                    else
                    {
                        this.MostrarMensaje("Se esperaba una unidad", ETipoMensajeIU.ERROR, nombreClase + ".AsignarNumVIN: ");
                    }
                }
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Error al intentar obtener los datos de la unidad", ETipoMensajeIU.ERROR, nombreClase + ".AsignarNumVIN:" + ex.Message);
            }
        }
        public void EstablecerListaArchivos()
        {
            List<TipoArchivoBO> lstTipos = new List<TipoArchivoBO>();
            lstTipos = tipoArchivoBR.Consultar(dctx, new TipoArchivoBO { Estatus=true});
            this.vista.TiposArchivo = lstTipos;
        }
        public void RegistrarTramite(ETipoTramite tipoTramite)
        {
            #region SC0008
            //obtener objeto SeguridadBO
            SeguridadBO seguridad = ObtenerSeguridad();
            if (seguridad == null) throw new Exception(nombreClase + ".RegistrarTramite():El objeto de SeguridadBO no debe se nulo");
            #endregion
            switch (tipoTramite)
            {
                case ETipoTramite.FILTRO_AK:
                        RegistrarTramiteAK(seguridad);
                    break;

                case ETipoTramite.GPS:
                    RegistrarTramiteGPS(seguridad);
                    break;

                case ETipoTramite.PLACA_FEDERAL:
                    RegistrarTramitePlacaFed(seguridad);
                    break;
                case ETipoTramite.PLACA_ESTATAL:
                    RegistrarTramitePlacaEstatal(seguridad);
                    break;

                case ETipoTramite.TENENCIA:
                    RegistrarTramiteTenencia(seguridad);
                    break;

                case ETipoTramite.VERIFICACION_AMBIENTAL:
                    RegistrarTramiteVerificacionAmbiental(seguridad);
                    break;
                case ETipoTramite.VERIFICACION_FISICO_MECANICA:
                    RegistrarTramiteVerificacionMecanica(seguridad);
                    break;
                default:
                    this.MostrarMensaje("No se encontró el tipo de trámite", ETipoMensajeIU.ERROR, nombreClase + ".RegistrarTramite()");
                    break;
            }
        }
        protected void RegistrarTramiteAK(SeguridadBO seguridad)
        {
            try
            {
                string s="";
                if (String.IsNullOrEmpty(s = presentadorAK.ValidarDatos()))
                {

                    FiltroAKBO tramiteAK = presentadorAK.InterfazUsuarioADato();
                    tramiteAK.Tramitable = vista.Tramitable;
                    FiltroAKBR filtroAKBr = new FiltroAKBR();
                    filtroAKBr.InsertarCompleto(dctx, tramiteAK, seguridad);
                    this.presentadorAK.Inicializar();
                    this.presentadorAK.LimpiarSesion();
                    this.RedirigirADetalle();

                }
                else
                {
                    this.MostrarMensaje("Es necesario proporcionar la siguiente información " + s, ETipoMensajeIU.ADVERTENCIA);
                }
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Error al guardar el trámite", ETipoMensajeIU.ERROR, nombreClase + ".RegistrarTramiteAK: " + ex.Message);
            }
        }
        protected void RegistrarTramiteGPS(SeguridadBO seguridad)
        {
            try
            {
                string s="";
                if (String.IsNullOrEmpty(s = presentadorGPS.ValidarDatos()))
                {
                    GPSBO tramiteGPS = presentadorGPS.InterfazUsuarioADato();
                    tramiteGPS.Tramitable = vista.Tramitable;
                    GPSBR gpsBr = new GPSBR();

                    gpsBr.InsertarCompleto(dctx, tramiteGPS,seguridad);
                    this.presentadorGPS.Inicializar();
                    this.presentadorGPS.LimpiarSesion();
                    this.RedirigirADetalle();
                }
                else
                {
                    this.MostrarMensaje("Es necesario proporcional la siguiente información " + s, ETipoMensajeIU.ADVERTENCIA);
                }
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Error al guardar el trámite", ETipoMensajeIU.ERROR, nombreClase + ".RegistrarTramiteGPS: " + ex.Message);
            }
        }
        protected void RegistrarTramitePlacaFed(SeguridadBO seguridad)
        {
            try
            {
                string s = "";
                if (String.IsNullOrEmpty(s = presentadorPlaca.ValidarDatos()))
                {
                    PlacaFederalBO tramitePlaca = (PlacaFederalBO)presentadorPlaca.InterfazUsuarioADato();
                    tramitePlaca.Tramitable = vista.Tramitable;
                    PlacaBR placaBr = new PlacaBR();

                    placaBr.InsertarCompleto(dctx, tramitePlaca,seguridad);
                    this.presentadorPlaca.Inicializar(vista.TipoTramite);
                    this.presentadorPlaca.LimpiarSesion();
                    this.RedirigirADetalle();
                }
                else
                {
                    this.MostrarMensaje("Es necesario proporcional la siguiente información " + s, ETipoMensajeIU.ADVERTENCIA);
                }
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Error al guardar el trámite", ETipoMensajeIU.ERROR, nombreClase + ".RegistrarTramitePlacaFed: " + ex.Message);
            }
        }
        protected void RegistrarTramitePlacaEstatal(SeguridadBO seguridad)
        {
            try
            {
                string s = "";
                if (String.IsNullOrEmpty(s = presentadorPlacaEstatal.ValidarDatos()))
                {
                    PlacaEstatalBO tramitePlaca = (PlacaEstatalBO)presentadorPlacaEstatal.InterfazUsuarioADato();
                    tramitePlaca.Tramitable = vista.Tramitable;
                    PlacaBR placaBr = new PlacaBR();

                    placaBr.InsertarCompleto(dctx, tramitePlaca, seguridad);
                    this.presentadorPlacaEstatal.Inicializar(vista.TipoTramite);
                    this.presentadorPlacaEstatal.LimpiarSesion();
                    this.RedirigirADetalle();
                }
                else
                {
                    this.MostrarMensaje("Es necesario proporcional la siguiente información " + s, ETipoMensajeIU.ADVERTENCIA);
                }
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Error al guardar el trámite", ETipoMensajeIU.ERROR, nombreClase + ".RegistrarTramitePlacaEstatal: " + ex.Message);
            }
        }
        protected void RegistrarTramiteTenencia(SeguridadBO seguridad)
        {
            try
            {
                string s="";
                if (String.IsNullOrEmpty(s = presentadorTenencia.validarDatos()))
                {
                    TenenciaBO tramiteTenencia = presentadorTenencia.InterfazUsuarioADato();
                    tramiteTenencia.Tramitable = vista.Tramitable;
                    TenenciaBR tenenciaBr = new TenenciaBR();
                    tenenciaBr.InsertarCompleto(dctx, tramiteTenencia, seguridad);
                    this.presentadorTenencia.Inicializar();
                    this.presentadorTenencia.LimpiarSesion();
                    this.RedirigirADetalle();

                }
                else
                {
                    this.MostrarMensaje("Es necesario proporcional la siguiente información " + s, ETipoMensajeIU.ADVERTENCIA);
                }
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Error al guardar el trámite", ETipoMensajeIU.ERROR, nombreClase + ".RegistrarTramiteTenencia: " + ex.Message);
            }
        }
        protected void RegistrarTramiteVerificacionMecanica(SeguridadBO seguridad)
        {
            try
            {
                string s ="";
                if (String.IsNullOrEmpty(s = presentadorVerificacion.validarDatos()))
                {
                    VerificacionBO tramiteVerificacion = presentadorVerificacion.InterfazUsuarioADato();
                    tramiteVerificacion.Tramitable = vista.Tramitable;
                    VerificacionBR verificacionBr = new VerificacionBR();
                    verificacionBr.InsertarCompleto(dctx, tramiteVerificacion, seguridad);
                    this.presentadorVerificacion.Inicializar(tramiteVerificacion.TipoVerificacion.Value);
                    this.presentadorVerificacion.LimpiarSesion();
                    this.RedirigirADetalle();
                }
                else
                {
                    this.MostrarMensaje("Es necesario proporcional la siguiente información " + s, ETipoMensajeIU.ADVERTENCIA);
                }
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Error al guardar el trámite", ETipoMensajeIU.ERROR, nombreClase + ".RegistrarTramiteVerificacionMecanica: " + ex.Message);
            }
        }
        protected void RegistrarTramiteVerificacionAmbiental(SeguridadBO seguridad)
        {
            try
            {
                string s = "";
                if (String.IsNullOrEmpty(s = presentadorVerificacionAmbiental.validarDatos()))
                {
                    VerificacionBO tramiteVerificacion = presentadorVerificacionAmbiental.InterfazUsuarioADato();
                    tramiteVerificacion.Tramitable = vista.Tramitable;
                    VerificacionBR verificacionBr = new VerificacionBR();
                    verificacionBr.InsertarCompleto(dctx, tramiteVerificacion, seguridad);
                    this.presentadorVerificacionAmbiental.Inicializar(tramiteVerificacion.TipoVerificacion.Value);
                    this.presentadorVerificacionAmbiental.LimpiarSesion();
                    this.RedirigirADetalle();
                }
                else
                {
                    this.MostrarMensaje("Es necesario proporcional la siguiente información " + s, ETipoMensajeIU.ADVERTENCIA);
                }
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Error al guardar el trámite", ETipoMensajeIU.ERROR, nombreClase + ".RegistrarTramiteVerificacionAmbiental: " + ex.Message);
            }
        }
        protected void MostrarMensaje(string mensaje, ETipoMensajeIU tipo, string detalle = null)
        {
            this.vista.MostrarMensaje(mensaje, tipo, detalle);
        }
        public void LimpiarSesionTramite()
        {
            this.vista.LimpiarSesionTramite();
        }
        public void LimpiarSesionTramitable()
        {
            this.vista.LimpiarSesionTramitable();
        }
        public void RedirigirADetalle()
        {
            
            this.vista.EstablecerPaqueteNavegacion("DatosTramitable", this.vista.Tramitable);
            this.LimpiarSesionTramitable();
            this.LimpiarSesionTramite();
            this.vista.RedirigirADetalle();
        }

        #region SC0008
        public void ValidarAcceso()
        {
            try
            {
                //se valida que los datos del usuario y la unidad operativa no sean nulos
                if (this.vista.UC == null) throw new Exception("El identificador del usuario no debe ser nulo");
                if (this.vista.UnidadOperativaId == null) throw new Exception("El identificador de la Unidad Operativa no debe ser nulo ");

                //Se crea el objeto de seguridad
                UsuarioBO usuario = new UsuarioBO() { Id = this.vista.UC };
                AdscripcionBO adscripcion = new AdscripcionBO() { UnidadOperativa = new UnidadOperativaBO() { Id = this.vista.UnidadOperativaId } };
                SeguridadBO seguridadBO = new SeguridadBO(Guid.Empty, usuario, adscripcion);

                //Se valida si el usuario tiene permiso a la acción principal
                if (!FacadeBR.ExisteAccion(this.dctx, "INSERTARCOMPLETO", seguridadBO))
                    this.vista.RedirigirSinPermisoAcceso();
            }
            catch (Exception ex)
            {
                throw new Exception(nombreClase + ".ValidarAcceso:" + ex.Message);
            }
        }

        private SeguridadBO ObtenerSeguridad()
        {
            try
            {
                SeguridadBO seguridad = null;
                if (this.vista.UsuarioId == null)
                    throw new Exception(nombreClase + ".ObtenerSeguridad(): Error al intentar obtener el usuario autenticado");
                if (this.vista.UnidadOperativaId == null)
                    throw new Exception(nombreClase + ".ObtenerSeguridad(): Error al intentar obtener la Unidad Operativa de la Adscripción");

                var unidadOperativaAdscripcion = new AdscripcionBO
                {
                    UnidadOperativa = new UnidadOperativaBO { Id = this.vista.UnidadOperativaId }
                };

                var usuarioLogueado = new UsuarioBO { Id = this.vista.UsuarioId };
                seguridad = new SeguridadBO(Guid.Empty, usuarioLogueado, unidadOperativaAdscripcion);
                return seguridad;
            }
            catch (Exception ex)
            {
                throw new Exception(nombreClase + ".Obtener Seguridad:" + ex.Message);
            }
            
        }
        #endregion
        #endregion
    }
}
