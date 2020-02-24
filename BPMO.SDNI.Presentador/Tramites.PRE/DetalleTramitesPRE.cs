//Satisface al CU087 – Catálogo Tramite Unidad
using System;
using System.Collections.Generic;
using BPMO.Basicos.BO;
using BPMO.Facade.SDNI.BR;
using BPMO.Patterns.Creational.DataContext;
using BPMO.Primitivos.Enumeradores;
using BPMO.SDNI.Equipos.BO;
using BPMO.SDNI.Equipos.BR;
using BPMO.SDNI.Tramites.BO;
using BPMO.SDNI.Tramites.BR;
using BPMO.SDNI.Tramites.VIS;
using System.Linq;

namespace BPMO.SDNI.Tramites.PRE
{
    public class DetalleTramitesPRE
    {
        #region Atributos
        private string nombreClase = "DetalleTramitesPRE";
        private IDataContext dctx;
        private SeguroBR seguroBR;
        private PlacaBR placaBR;
        private VerificacionBR verificacionBR;
        private GPSBR gpsBR;
        private TenenciaBR tenenciaBR;
        private FiltroAKBR filtroBR;
        private UnidadBR unidadBR;
        private IDetalleTramitesVIS vista;
        private ucTramiteFiltroAKPRE presentadorFiltro;
        private ucTramiteGPSPRE presentadorGPS;
        private ucTramitePlacaPRE presentadorPlacaEstatal;
        private ucTramitePlacaPRE presentadorPlacaFederal;
        private ucTramiteTenenciaPRE presentadorTenencia;
        private ucTramiteVerificacionPRE presentadorAmbiental;
        private ucTramiteVerificacionPRE presentadorMecanico;
        #endregion

        #region Constructor
        public DetalleTramitesPRE(IDetalleTramitesVIS vista, IucTramiteFiltroAKVIS vistaFiltro, IucTramiteGPSVIS vistaGPS, IucTramitePlacaVIS vistaPlacaEstatal, IucTramitePlacaVIS vistaPlacaFederal, IucTramiteTenenciaVIS vistaTenencia, IucTramiteVerificacionVIS vistaAmbiental, IucTramiteVerificacionVIS vistaMecanico)
        {
            try
            {
                this.vista = vista;
                presentadorFiltro = new ucTramiteFiltroAKPRE(vistaFiltro);
                presentadorGPS = new ucTramiteGPSPRE(vistaGPS);
                presentadorMecanico = new ucTramiteVerificacionPRE(vistaMecanico);
                presentadorAmbiental = new ucTramiteVerificacionPRE(vistaAmbiental);
                presentadorPlacaFederal = new ucTramitePlacaPRE(vistaPlacaFederal);
                presentadorPlacaEstatal = new ucTramitePlacaPRE(vistaPlacaEstatal);
                presentadorTenencia = new ucTramiteTenenciaPRE(vistaTenencia);
                tenenciaBR = new TenenciaBR();
                placaBR = new PlacaBR();
                verificacionBR = new VerificacionBR();
                gpsBR = new GPSBR();
                filtroBR = new FiltroAKBR();
                seguroBR = new SeguroBR();
                unidadBR = new UnidadBR();
                dctx = FacadeBR.ObtenerConexion();


            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencias en la configuración", ETipoMensajeIU.ERROR, nombreClase + ".DetalleTramitesPRE: " + ex.Message);
            }
        }
        #endregion

        #region Métodos
        public void MostrarMensaje(string mensaje, ETipoMensajeIU tipo, string detalle = null)
        {
            this.vista.MostrarMensaje(mensaje, tipo, detalle);
        }
        public void Inicializar()
        {
            this.LimpiarSesion();
            presentadorAmbiental.Inicializar(ETipoVerificacion.AMBIENTAL);
            presentadorFiltro.Inicializar();
            presentadorGPS.Inicializar();
            presentadorMecanico.Inicializar(ETipoVerificacion.FISICO_MECANICO);
            presentadorPlacaEstatal.Inicializar(ETipoTramite.PLACA_ESTATAL);
            presentadorPlacaFederal.Inicializar(ETipoTramite.PLACA_FEDERAL);
            presentadorTenencia.Inicializar();
            this.ObtenerDatosNavegacion();
            this.ConsultarTramites();
            this.EstablecerSeguridad();
        }

        private void LimpiarSesion()
        {
            this.vista.LimpiarSesion();
        }
        private void DatoAInterfazUsuario()
        {
            if (this.vista.VerificacionAmbiental.TramiteID != null)
            {
                presentadorAmbiental.DatoAInterfazUsuario(this.vista.VerificacionAmbiental);

                presentadorAmbiental.ModoEdicion(false);
            }
            else
            {
                presentadorAmbiental.ModoEdicion(false);
            }
            if (this.vista.VerificacionMecanica.TramiteID != null)
            {
                presentadorMecanico.DatoAInterfazUsuario(this.vista.VerificacionMecanica);
                presentadorMecanico.ModoEdicion(false);
            }
            else
            {
                presentadorMecanico.ModoEdicion(false);
            }
            if (this.vista.PlacaEstatal.TramiteID != null)
            {
                presentadorPlacaEstatal.DatoAInterfazUsuario(this.vista.PlacaEstatal);
                presentadorPlacaEstatal.ModoEdicion(false);
            }
            else
            {
                presentadorPlacaEstatal.ModoEdicion(false);
            }
            if (this.vista.PlacaFederal.TramiteID != null)
            {
                presentadorPlacaFederal.DatoAInterfazUsuario(this.vista.PlacaFederal);
                presentadorPlacaFederal.ModoEdicion(false);
            }
            else
            {
                presentadorPlacaFederal.ModoEdicion(false);
            }
            if (this.vista.Tenencia.TramiteID != null)
            {
                presentadorTenencia.DatoAInterfazUsuario(this.vista.Tenencia);
                presentadorTenencia.ModoEdicion(false);
            }
            else
            {
                presentadorTenencia.ModoEdicion(false);
            }
            if (this.vista.GPS.TramiteID != null)
            {
                presentadorGPS.DatoAInterfazUsuario(this.vista.GPS);
                presentadorGPS.ModoEdicion(false);
            }
            else
            {
                presentadorGPS.ModoEdicion(false);
            }
            if (this.vista.FiltroAK.TramiteID != null)
            {
                presentadorFiltro.DatoAInterfazUsuario(this.vista.FiltroAK);
                presentadorFiltro.ModoEdicion(false);
            }
            else
            {
                presentadorFiltro.ModoEdicion(false);
            }
        }
        private void ConsultarTramites()
        {
            try
            {
                UnidadBO unidadTemp = new UnidadBO
                {
                    UnidadID = this.vista.Tramitable.TramitableID,
                    NumeroSerie = this.vista.Tramitable.DescripcionTramitable

                };
                List<UnidadBO> lstunidad = unidadBR.Consultar(dctx, unidadTemp);
                if (lstunidad.Count != 0)
                {
                    if (lstunidad.Count == 1)
                    {
                        UnidadBO unidad = lstunidad[0];
                        if (unidad.UnidadID != null)
                        {
                            if (unidad.Modelo != null)
                            {
                                this.vista.Modelo = unidad.Modelo.Nombre;
                                if (unidad.Modelo.Marca != null)
                                    this.vista.Marca = unidad.Modelo.Marca.Nombre;

                            }
                            this.vista.NumeroSerie = unidad.NumeroSerie;
                            #region Tramites
                            //Consulta de Trámites
                            List<TenenciaBO> lstTenencia = tenenciaBR.ConsultarCompleto(dctx, new TenenciaBO { Activo = true, Tramitable = this.vista.Tramitable });
                            if (lstTenencia.Count == 1)
                                this.vista.Tenencia = lstTenencia[0];
                            if (lstTenencia.Count > 1)
                                throw new Exception("Inconsistencia en el tramites de tenencias, la consulta devolvió mas de una tenencia activa");

                            List<VerificacionBO> lstVerificacionAmbiental = verificacionBR.ConsultarCompleto(dctx, new VerificacionBO { Activo = true, Tipo = ETipoTramite.VERIFICACION_AMBIENTAL, Tramitable = this.vista.Tramitable });
                            if (lstVerificacionAmbiental.Count == 1)
                                this.vista.VerificacionAmbiental = lstVerificacionAmbiental[0];
                            if (lstVerificacionAmbiental.Count > 1)
                                throw new Exception("Inconsistencia en el tramites de Verificacion Ambiental, la consulta devolvió mas de una Verificación Ambiental activa");

                            List<VerificacionBO> lstVerificacionMecanico = verificacionBR.ConsultarCompleto(dctx, new VerificacionBO { Activo = true, Tipo = ETipoTramite.VERIFICACION_FISICO_MECANICA, Tramitable = this.vista.Tramitable });
                            if (lstVerificacionMecanico.Count == 1)
                                this.vista.VerificacionMecanica = lstVerificacionMecanico[0];
                            if (lstVerificacionMecanico.Count > 1)
                                throw new Exception("Inconsistencia en el tramites de Verificación Físico - Mecánico, la consulta devolvió mas de una Verificación Fisico- Mecánica activa");

                            List<PlacaEstatalBO> lstPlacaEstatal = placaBR.Consultar(dctx, new PlacaEstatalBO { Activo = true, Tramitable = this.vista.Tramitable, Tipo = ETipoTramite.PLACA_ESTATAL });
                            if (lstPlacaEstatal.Count == 1)
                                this.vista.PlacaEstatal = lstPlacaEstatal[0];
                            if (lstPlacaEstatal.Count > 1)
                                throw new Exception("Inconsistencia en el tramites de Placa Estatal, la consulta devolvió mas de una Placa Estatal activa");                            
                            List<PlacaFederalBO> lstPlacaFederal = placaBR.Consultar(dctx, new PlacaFederalBO { Tramitable = this.vista.Tramitable, Tipo = ETipoTramite.PLACA_FEDERAL });
                            if (lstPlacaFederal != null && lstPlacaFederal.Count > 0)
                            {
                                var lstPlacasFederalesActivas = lstPlacaFederal.Where(p => p.Activo == true).ToList();
                                if (lstPlacasFederalesActivas.Count > 1)
                                    throw new Exception("Inconsistencia en el tramites de Placa Federal, la consulta devolvió mas de una Placa Federal activa");
                                if (lstPlacasFederalesActivas.Count == 1)
                                    this.vista.PlacaFederal = lstPlacasFederalesActivas[0];
                                else
                                    this.vista.PlacaFederal = lstPlacaFederal.FirstOrDefault(p => p.FechaRecepcion.Value == lstPlacaFederal.Max(p1 => p1.FechaRecepcion).Value);
                            }                    

                            List<GPSBO> lstGPS = gpsBR.Consultar(dctx, new GPSBO { Tramitable = this.vista.Tramitable, Activo = true });
                            if (lstGPS.Count == 1)
                                this.vista.GPS = lstGPS[0];
                            if (lstGPS.Count > 1)
                                throw new Exception("Inconsistencia en el tramites de GPS, la consulta devolvió mas de un GPS activo");
                            List<FiltroAKBO> lstFiltro = filtroBR.Consultar(dctx, new FiltroAKBO { Tramitable = this.vista.Tramitable, Activo = true });
                            if (lstFiltro.Count == 1)
                                this.vista.FiltroAK = lstFiltro[0];
                            if (lstFiltro.Count > 1)
                                throw new Exception("Inconsistencia en el tramites de Filtro AK, la consulta devolvió mas de un Filtro AK  activo");

                            List<SeguroBO> lstSeguros = seguroBR.ConsultarCompleto(dctx, new SeguroBO { Tramitable = this.vista.Tramitable, Tipo = ETipoTramite.SEGURO, Activo = true });
                            if (lstSeguros != null)
                            {
                                if (lstSeguros.Count == 1)
                                {
                                    this.vista.Poliza = lstSeguros[0].NumeroPoliza;
                                    this.vista.Aseguradora = lstSeguros[0].Aseguradora;
                                    this.vista.FechaInicial = lstSeguros[0].VigenciaInicial;
                                    this.vista.FechaFinal = lstSeguros[0].VigenciaFinal;
                                    this.vista.Seguro = lstSeguros[0];
                                }
                                if (lstSeguros.Count > 1)
                                    throw new Exception("Inconsistencia en el tramites de Seguro, la consulta devolvió mas de un Seguro  activo");
                            }
                            #endregion
                            this.DatoAInterfazUsuario();
                        }
                        else
                        {
                            this.MostrarMensaje("Error al intentar obtener los datos de la unidad", ETipoMensajeIU.ERROR, nombreClase + ".ConsultarTramites(): La unidad es nulo");
                        }
                    }
                    else
                    {
                        this.MostrarMensaje("Error al intentar obtener los datos de la unidad ", ETipoMensajeIU.ERROR, nombreClase + ".ConsultarTramites(): La consulta devolvió más de una unidad");
                    }
                }
                else
                {
                    this.MostrarMensaje("Error al intentar obtener los datos de la unidad ", ETipoMensajeIU.ERROR, nombreClase + "ConsultarTramites(): La consulta no devolvió ningún resultado");
                }

            }

            catch (Exception ex)
            {
                this.MostrarMensaje("Error al intentar consultar los trámites", ETipoMensajeIU.ERROR, nombreClase + ".ConsultarTramites: " + ex.Message);
            }
        }
        public void ObtenerDatosNavegacion()
        {
            if (this.vista.Tramitable != null)
            {
                if (this.vista.Tramitable.TramitableID == null)
                    throw new Exception("Se esperaba una ITamitable");
            }
        }
        public void EstablecerPagina(int p)
        {
            this.vista.EstablecerPagina(p);

        }

        public void MostrarListaTenencias()
        {
            try
            {
                List<TenenciaBO> lstTenencias = tenenciaBR.ConsultarCompleto(dctx, new TenenciaBO { Tramitable = this.vista.Tramitable });
                if (lstTenencias.Count == 0)
                    this.MostrarMensaje("No se hay registros de tenencias", ETipoMensajeIU.INFORMACION);
                else
                {
                    this.vista.Tenencias = lstTenencias;
                    this.vista.MostrarTenencias();
                }
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Error al intentar consultar la Tenencia", ETipoMensajeIU.ERROR, nombreClase + ".MostrarListaTenencias(): " + ex.Message);
            }
        }
        public void EstablecerDatosNavegacion(ETipoTramite tipo)
        {
            object Tramite = null;
            if (tipo == ETipoTramite.FILTRO_AK)
                Tramite = this.vista.FiltroAK;
            if (tipo == ETipoTramite.GPS)
                Tramite = this.vista.GPS;
            if (tipo == ETipoTramite.PLACA_ESTATAL)
                Tramite = this.vista.PlacaEstatal;
            if (tipo == ETipoTramite.PLACA_FEDERAL)
                Tramite = this.vista.PlacaFederal;
            if (tipo == ETipoTramite.TENENCIA)
                Tramite = this.vista.Tenencia;
            if (tipo == ETipoTramite.VERIFICACION_AMBIENTAL)
                Tramite = this.vista.VerificacionAmbiental;
            if (tipo == ETipoTramite.VERIFICACION_FISICO_MECANICA)
                Tramite = this.vista.VerificacionMecanica;


            this.vista.EstablecerDatosNavegacion("Unidad", this.vista.Tramitable, "Tramite", Tramite, "TipoTramite", tipo);
            this.vista.RedirigirARegistrar();
        }
        public void EstablecerDatosNavegacionSeguro(string nombre, object objeto)
        {
            this.vista.EstablecerDatosNavegacionSeguro(nombre, objeto);
        }
        public void RedirigirAEdicion(ETipoTramite tipo)
        {
            object Tramite = null;
            if (tipo == ETipoTramite.FILTRO_AK)
                Tramite = this.vista.FiltroAK;
            if (tipo == ETipoTramite.GPS)
                Tramite = this.vista.GPS;
            if (tipo == ETipoTramite.PLACA_ESTATAL)
                Tramite = this.vista.PlacaEstatal;
            if (tipo == ETipoTramite.PLACA_FEDERAL)
                Tramite = this.vista.PlacaFederal;
            if (tipo == ETipoTramite.TENENCIA)
                Tramite = this.vista.Tenencia;
            if (tipo == ETipoTramite.VERIFICACION_AMBIENTAL)
                Tramite = this.vista.VerificacionAmbiental;
            if (tipo == ETipoTramite.VERIFICACION_FISICO_MECANICA)
                Tramite = this.vista.VerificacionMecanica;


            this.vista.EstablecerDatosNavegacion("Unidad", this.vista.Tramitable, "Tramite", Tramite, "TipoTramite", tipo);
            this.LimpiarSesion();
            presentadorAmbiental.Inicializar(ETipoVerificacion.AMBIENTAL);
            presentadorAmbiental.LimpiarSesion();

            presentadorFiltro.Inicializar();
            presentadorFiltro.LimpiarSesion();

            presentadorGPS.Inicializar();
            presentadorGPS.LimpiarSesion();

            presentadorMecanico.Inicializar(ETipoVerificacion.FISICO_MECANICO);
            presentadorMecanico.LimpiarSesion();

            presentadorPlacaEstatal.Inicializar(ETipoTramite.PLACA_ESTATAL);
            presentadorPlacaEstatal.LimpiarSesion();

            presentadorPlacaFederal.Inicializar(ETipoTramite.PLACA_FEDERAL);
            presentadorPlacaFederal.LimpiarSesion();
            presentadorTenencia.LimpiarSesion();
            presentadorTenencia.Inicializar();

            this.vista.RedigirAEdicion();
        }
        public void RedirigirARegistrar(ETipoTramite tipo)
        {
            object Tramite = null;
            this.vista.EstablecerDatosNavegacion("Tramitable", this.vista.Tramitable, "Tramite", Tramite, "TipoTramite", tipo);
            this.LimpiarSesion();
            this.vista.RedirigirARegistrar();
        }
        public void RedirigirARegistrarSeguro()
        {
            SeguroBO seguroBO = new SeguroBO { Tramitable = this.vista.Tramitable };
            this.EstablecerDatosNavegacionSeguro("REGISTRARSEGURO", seguroBO);
            this.vista.RedirigirARegistrarSeguro();
        }
        public void RedirigirAEditarSeguro()
        {
            this.EstablecerDatosNavegacionSeguro("EDITARSEGURO", this.vista.Seguro);
            this.vista.RedirigirAEditarSeguro();
        }
        public void MostrarListaVerificacionMecanica()
        {
            try
            {
                List<VerificacionBO> lstVerificacion = verificacionBR.ConsultarCompleto(dctx, new VerificacionBO { TipoVerificacion = ETipoVerificacion.FISICO_MECANICO, Tramitable = this.vista.Tramitable, Tipo = ETipoTramite.VERIFICACION_FISICO_MECANICA });
                if (lstVerificacion.Count == 0)
                    this.MostrarMensaje("No hay registros de Verificacion Ambiental", ETipoMensajeIU.INFORMACION);
                else
                {
                    this.vista.VerificacionesMecanicas = lstVerificacion;
                    this.vista.MostrarVerificacionMecanico();
                }
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Error al intentar consultar la Verificación Fisico - Mecanica", ETipoMensajeIU.ERROR, nombreClase + ".MostrarListaVerificacionMecanica(): " + ex.Message);
            }
        }

        public void MostrarListaPlacaEstatal()
        {
            try
            {
                List<PlacaEstatalBO> lstPlaca = placaBR.Consultar(dctx, new PlacaEstatalBO { Tramitable = this.vista.Tramitable, Tipo = ETipoTramite.PLACA_ESTATAL });
                if (lstPlaca.Count == 0)
                    this.MostrarMensaje("No hay registros de Placa Estatal", ETipoMensajeIU.INFORMACION);
                else
                {
                    this.vista.PlacasEstatales = lstPlaca;
                    this.vista.MostrarPlacaEstatales();
                }
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Error al intentar consultar la Placa Estatal", ETipoMensajeIU.ERROR, nombreClase + ".MostrarListaPlacaEstatal(): " + ex.Message);
            }
        }
        public void MostrarListaPlacaFederal()
        {
            try
            {
                List<PlacaFederalBO> lstPlaca = placaBR.Consultar(dctx, new PlacaFederalBO { Tramitable = this.vista.Tramitable, Tipo = ETipoTramite.PLACA_FEDERAL });
                if (lstPlaca.Count == 0)
                    this.MostrarMensaje("No hay registros de Placa Federal", ETipoMensajeIU.INFORMACION);
                else
                {
                    this.vista.PlacasFederales = lstPlaca;
                    this.vista.MostrarPlacasFederales();
                }
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Error al consultar la Placa Federal", ETipoMensajeIU.ERROR, nombreClase + ".MostrarListaPlacasFederal(): " + ex.Message);
            }
        }
        public void MostrarListaGPS()
        {
            try
            {
                List<GPSBO> lstGPS = gpsBR.Consultar(dctx, new GPSBO { Tramitable = this.vista.Tramitable, Tipo = ETipoTramite.GPS });
                if (lstGPS.Count == 0)
                    this.MostrarMensaje("No hay registros GPS", ETipoMensajeIU.INFORMACION);
                else
                {
                    this.vista.ListaGPS = lstGPS;
                    this.vista.MostrarGPSs();
                }
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Error al consultar GPS", ETipoMensajeIU.ERROR, nombreClase + ".MostrarListaGPS(): " + ex.Message);
            }
        }
        public void MostrarListaFiltro()
        {
            try
            {
                List<FiltroAKBO> lstFiltro = filtroBR.Consultar(dctx, new FiltroAKBO { Tramitable = this.vista.Tramitable, Tipo = ETipoTramite.FILTRO_AK });
                if (lstFiltro.Count == 0)
                    this.MostrarMensaje("No hay registros Filtro AK ", ETipoMensajeIU.INFORMACION);
                else
                {
                    this.vista.FiltroAKs = lstFiltro;
                    this.vista.MostrarFiltros();
                }
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Error al intentar consultar Filtro AK", ETipoMensajeIU.ERROR, nombreClase + ".MostrarListaFiltro(): " + ex.Message);
            }
        }
        public void MostrarListaVerificacionAmbiental()
        {

            try
            {
                List<VerificacionBO> lstVerificacion = verificacionBR.Consultar(dctx, new VerificacionBO { Tramitable = this.vista.Tramitable, TipoVerificacion = ETipoVerificacion.AMBIENTAL, Tipo = ETipoTramite.VERIFICACION_AMBIENTAL });
                if (lstVerificacion.Count == 0)
                    this.MostrarMensaje("No hay registros Verificación Ambiental", ETipoMensajeIU.INFORMACION);
                else
                {
                    this.vista.VerificacionesAmbientales = lstVerificacion;
                    this.vista.MostrarVerificacionAmbiental();
                }
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Error al consultar Verificación Ambiental", ETipoMensajeIU.ERROR, nombreClase + ".MostrarListaVerificacionAmbiental(): " + ex.Message);
            }

        }

        #region SC_0008
        public void ValidarAcceso()
        {
            try
            {
                //se valida que los datos del usuario y la unidad operativa no sean nulos
                if (this.vista.UsuarioId == null) throw new Exception("El identificador del usuario no debe ser nulo");
                if (this.vista.UnidadOperativaId == null) throw new Exception("El identificador de la Unidad Operativa no debe ser nulo ");

                //Se crea el objeto de seguridad
                UsuarioBO usuario = new UsuarioBO() { Id = this.vista.UsuarioId };
                AdscripcionBO adscripcion = new AdscripcionBO() { UnidadOperativa = new UnidadOperativaBO() { Id = this.vista.UnidadOperativaId } };
                SeguridadBO seguridadBO = new SeguridadBO(Guid.Empty, usuario, adscripcion);

                //Se valida si el usuario tiene permiso a la acción principal
                if (!FacadeBR.ExisteAccion(this.dctx, "CONSULTAR", seguridadBO))
                    this.vista.RedirigirSinPermisoAcceso();
            }
            catch (Exception ex)
            {
                throw new Exception(nombreClase + ".ValidarAcceso:" + ex.Message);
            }
        }
        private void EstablecerSeguridad()
        {
            try
            {
                //se valida que los datos del usuario y la unidad operativa no sean nulos
                if (this.vista.UsuarioId == null) throw new Exception("El identificador del usuario no debe ser nulo");
                if (this.vista.UnidadOperativaId == null) throw new Exception("El identificador de la Unidad Operativa no debe ser nulo ");

                //Se crea el objeto de seguridad
                UsuarioBO usuario = new UsuarioBO() { Id = this.vista.UsuarioId };
                AdscripcionBO adscripcion = new AdscripcionBO() { UnidadOperativa = new UnidadOperativaBO() { Id = this.vista.UnidadOperativaId } };
                SeguridadBO seguridadBO = new SeguridadBO(Guid.Empty, usuario, adscripcion);

                //Se obtienen las acciones a las cuales el usuario tiene permiso en este proceso
                List<CatalogoBaseBO> lst = FacadeBR.ConsultarAccion(this.dctx, seguridadBO);

                //Se valida si el usuario tiene permiso para registrar cuenta cliente
                if (!this.ExisteAccion(lst, "INSERTARCOMPLETO"))
                    this.vista.PermitirRegistrar(false);

                if (!this.ExisteAccion(lst, "ACTUALIZARCOMPLETO"))
                    this.vista.PermitirEditar(false);
            }
            catch (Exception ex)
            {
                throw new Exception(nombreClase + ".EstablecerSeguridad:" + ex.Message);
            }
        }

        private bool ExisteAccion(List<CatalogoBaseBO> acciones, string nombreAccion)
        {
            if (acciones != null && acciones.Count > 0 && acciones.Exists(p => p != null && p.Nombre != null && p.Nombre.Trim().ToUpper() == nombreAccion.Trim().ToUpper()))
                return true;

            return false;
        }
        #endregion
        #endregion
    }
}
