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
    public class EditarTramitesPRE
    {
        #region Atributos
        private string nombreClase = "EditarTramitesPRE";
        private IDataContext dctx;
        private TipoArchivoBR tipoArchivoBR;
        private FiltroAKBR filtroBR;
        private TenenciaBR tenenciaBR;
        private PlacaBR placaBR;
        private GPSBR gpsBR;
        private VerificacionBR verificacionBR;
        private UnidadBR unidadBR;
        private IEditarTramitesVIS vista;
        private ucTramiteFiltroAKPRE presentadorFiltro;
        private ucTramiteGPSPRE presentadorGPS;
        private ucTramitePlacaPRE presentadorPlacaEstatal;
        private ucTramitePlacaPRE presentadorPlacaFederal;
        private ucTramiteTenenciaPRE presentadorTenencia;
        private ucTramiteVerificacionPRE presentadorVerificacionAmbiental;
        private ucTramiteVerificacionPRE presentandorVerificacionMecanico;
        #endregion

        #region Constructor
        public EditarTramitesPRE(IEditarTramitesVIS vista,IucTramiteFiltroAKVIS vistaFiltro, IucTramiteGPSVIS vistaGPS, IucTramitePlacaVIS vistaPlacaEstatal, IucTramitePlacaVIS vistaPlacaFederal,IucTramiteTenenciaVIS vistaTenencia, IucTramiteVerificacionVIS vistaVerificacionAmbiental, IucTramiteVerificacionVIS vistaVerificacionMecanico)
        {
            try
            {
                this.vista = vista;
                dctx = FacadeBR.ObtenerConexion();
                tenenciaBR = new TenenciaBR();
                placaBR = new PlacaBR();
                gpsBR = new GPSBR();
                verificacionBR = new VerificacionBR();
                filtroBR = new FiltroAKBR();
                tipoArchivoBR = new TipoArchivoBR();
                unidadBR = new UnidadBR();

                presentadorFiltro = new ucTramiteFiltroAKPRE(vistaFiltro);
                presentadorGPS = new ucTramiteGPSPRE(vistaGPS);
                presentadorPlacaEstatal = new ucTramitePlacaPRE(vistaPlacaEstatal);
                presentadorPlacaFederal = new ucTramitePlacaPRE(vistaPlacaFederal);
                presentadorTenencia = new ucTramiteTenenciaPRE(vistaTenencia);
                presentadorVerificacionAmbiental = new ucTramiteVerificacionPRE(vistaVerificacionAmbiental);
                presentandorVerificacionMecanico = new ucTramiteVerificacionPRE(vistaVerificacionMecanico);
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Error en la configuracion de la página", ETipoMensajeIU.ERROR, nombreClase + ".EditarTramitesPRE():" +ex.Message);
            }
        }
        #endregion 

        #region Métodos
        private void MostrarMensaje(string mensaje, ETipoMensajeIU tipo, string detalle=null)
        {
            this.vista.MostrarMensaje(mensaje,tipo,detalle);
        }
        public void Inicializar()
        {
            this.LimpiarSesion(false);
            presentadorFiltro.Inicializar();
            presentadorGPS.Inicializar();
            presentadorPlacaEstatal.Inicializar(ETipoTramite.PLACA_ESTATAL);
            presentadorTenencia.Inicializar();
            presentadorVerificacionAmbiental.Inicializar(ETipoVerificacion.AMBIENTAL);
            presentadorPlacaFederal.Inicializar(ETipoTramite.PLACA_FEDERAL);
            presentandorVerificacionMecanico.Inicializar(ETipoVerificacion.FISICO_MECANICO);
           
            if (this.vista.Tramitable != null)
            {
                if (this.vista.Tramitable is ITramitable)
                {
                    if (this.vista.Tipo != null)
                    {
                        this.ConsultarEquipoBO();
                        this.ConfigurarEdicion();
                    }
                }
                else
                    throw new Exception("Se esperaba un objeto tramitable");
            }
            else
            {
                this.MostrarMensaje("Error en la configuración", ETipoMensajeIU.ERROR, nombreClase+".Inicializar()");
            }
        }

        #region Métodos para habilitar modo edición
        public void EditarFiltro()
        {
            this.vista.MostrarFiltroAK();
            if (this.vista.UltimoTramite != null && this.vista.UltimoTramite is FiltroAKBO)
            {
                FiltroAKBO filtro = new FiltroAKBO();
                filtro.Auditoria = new Basicos.BO.AuditoriaBO();
                FiltroAKBO temp = (FiltroAKBO)this.vista.UltimoTramite;
                filtro.Activo = temp.Activo;
                filtro.Auditoria.FC = temp.Auditoria.FC;
                filtro.Auditoria.FUA = temp.Auditoria.FUA;
                filtro.Auditoria.UC = temp.Auditoria.UC;
                filtro.Auditoria.UUA = temp.Auditoria.UUA;
                filtro.FechaInstalacion = temp.FechaInstalacion;
                filtro.NumeroSerie = temp.NumeroSerie;
                filtro.Resultado = temp.Resultado;
                filtro.Tipo = temp.Tipo;
                filtro.TramiteID = temp.TramiteID;
                this.presentadorFiltro.DatoAInterfazUsuario(filtro);
                this.presentadorFiltro.ModoEdicion(true);
            }
            else throw new Exception("Se esparaba un objeto FiltroAKBO");
        }
        public void EditarGPS()
        {
            this.vista.MostrarGPS();
            if (this.vista.UltimoTramite != null && this.vista.UltimoTramite is GPSBO)
            {
                GPSBO gps = new GPSBO();
                gps.Auditoria = new Basicos.BO.AuditoriaBO();
                GPSBO temp = this.vista.UltimoTramite as GPSBO;
                gps.Activo = temp.Activo;
                gps.Auditoria.FC = temp.Auditoria.FC;
                gps.Auditoria.FUA = temp.Auditoria.FUA;
                gps.Auditoria.UC = temp.Auditoria.UC;
                gps.Auditoria.UUA = temp.Auditoria.UUA;
                gps.Compania = temp.Compania;
                gps.FechaInstalacion = temp.FechaInstalacion;
                gps.Numero = temp.Numero;
                gps.Resultado = temp.Resultado;
                gps.Tipo = temp.Tipo;
                gps.TramiteID = temp.TramiteID;

                this.presentadorGPS.DatoAInterfazUsuario(gps);
                this.presentadorGPS.ModoEdicion(true);
            }
            else throw new Exception("Se esparaba un objeto GPSBO");
        }
        public void EditarPlacaEstatal()
        {
            this.vista.MostrarPlacaEstatal();
            if (this.vista.UltimoTramite != null && this.vista.UltimoTramite is PlacaEstatalBO)
            {
                PlacaEstatalBO placa = new PlacaEstatalBO();
                placa.Auditoria = new Basicos.BO.AuditoriaBO();
                PlacaEstatalBO temp = (PlacaEstatalBO)this.vista.UltimoTramite;
                placa.Activo = temp.Activo;
                placa.Auditoria.FC = temp.Auditoria.FC;
                placa.Auditoria.FUA = temp.Auditoria.FUA;
                placa.Auditoria.UC = temp.Auditoria.UC;
                placa.Auditoria.UUA = temp.Auditoria.UUA;
                placa.Numero = temp.Numero;
                placa.Resultado = temp.Resultado;
                placa.Tipo = temp.Tipo;
                placa.TramiteID = temp.TramiteID;
                this.presentadorPlacaEstatal.DatoAInterfazUsuario(placa);
                this.presentadorPlacaEstatal.ModoEdicion(true);
            }
            else throw new Exception("Se esparaba un objeto PlacaEstatalBO");
        }
        public void EditarPlacaFederal()
        {
            this.vista.MostrarPlacaFederal();
            if (this.vista.UltimoTramite != null && this.vista.UltimoTramite is PlacaFederalBO)
            {
                PlacaFederalBO placa = new PlacaFederalBO();
                placa.Auditoria = new Basicos.BO.AuditoriaBO();
                PlacaFederalBO temp = (PlacaFederalBO)this.vista.UltimoTramite;
                placa.Activo = temp.Activo;
                placa.Auditoria.FC = temp.Auditoria.FC;
                placa.Auditoria.FUA = temp.Auditoria.FUA;
                placa.Auditoria.UC = temp.Auditoria.UC;
                placa.Auditoria.UUA = temp.Auditoria.UUA;
                placa.FechaEnvioDocumentos = temp.FechaEnvioDocumentos;
                placa.FechaRecepcion = temp.FechaRecepcion;
                placa.Numero = temp.Numero;
                placa.NumeroGuia = temp.NumeroGuia;
                placa.Resultado = temp.Resultado;
                placa.Tipo = temp.Tipo;
                placa.TramiteID = temp.TramiteID;
                this.presentadorPlacaFederal.DatoAInterfazUsuario(placa);
                this.presentadorPlacaFederal.ModoEdicion(true);
            }
            else throw new Exception("Se esparaba un objeto PlacaFederalBO");
        }
        public void EditarTenencia()
        {
            this.vista.MostrarTenencia();
            if (this.vista.UltimoTramite != null && this.vista.UltimoTramite is TenenciaBO)
            {
                TenenciaBO tenencia = new TenenciaBO();
                tenencia.Auditoria = new Basicos.BO.AuditoriaBO();
                tenencia.Archivos = new List<ArchivoBO>();
                TenenciaBO temp = (TenenciaBO)this.vista.UltimoTramite;
                tenencia.Activo = temp.Activo;
                tenencia.Archivos = temp.Archivos;
                tenencia.Auditoria.FC = temp.Auditoria.FC;
                tenencia.Auditoria.FUA = temp.Auditoria.FUA;
                tenencia.Auditoria.UC = temp.Auditoria.UC;
                tenencia.Auditoria.UUA = temp.Auditoria.UUA;
                tenencia.FechaPago = temp.FechaPago;
                tenencia.Folio = temp.Folio;
                tenencia.Importe = temp.Importe;
                tenencia.Resultado = temp.Resultado;
                tenencia.Tipo = temp.Tipo;
                tenencia.TramiteID = temp.TramiteID;
                List<TipoArchivoBO> lstTipos = tipoArchivoBR.Consultar(dctx, new TipoArchivoBO { Estatus = true });
                this.presentadorTenencia.DatoAInterfazUsuario(tenencia);
                this.presentadorTenencia.ModoEdicion(true);
                this.presentadorTenencia.EstablecerTiposdeArchivo(lstTipos);
            }
            else throw new Exception("Se esparaba un objeto TenenciaBO");
        }
        public void EditarVerificacionAmbiental()
        {
            this.vista.MostrarVerificacionAmbiental();
            if (this.vista.UltimoTramite != null && this.vista.UltimoTramite is VerificacionBO)
            {
                VerificacionBO verificacion = new VerificacionBO();
                verificacion.Auditoria = new Basicos.BO.AuditoriaBO();
                verificacion.Archivos = new List<ArchivoBO>();
                VerificacionBO temp = (VerificacionBO)this.vista.UltimoTramite;
                verificacion.Activo = temp.Activo;
                verificacion.Archivos = temp.Archivos;
                verificacion.Auditoria.FC = temp.Auditoria.FC;
                verificacion.Auditoria.FUA = temp.Auditoria.FUA;
                verificacion.Auditoria.UC = temp.Auditoria.UC;
                verificacion.Auditoria.UUA = temp.Auditoria.UUA;
                verificacion.Folio = temp.Folio;
                verificacion.Resultado = temp.Resultado;
                verificacion.Tipo = temp.Tipo;
                verificacion.TipoVerificacion = temp.TipoVerificacion;
                verificacion.TramiteID = temp.TramiteID;
                verificacion.VigenciaFinal = temp.VigenciaFinal;
                verificacion.VigenciaInicial = temp.VigenciaInicial;

                List<TipoArchivoBO> lstTipos = tipoArchivoBR.Consultar(dctx, new TipoArchivoBO { Estatus = true });
                this.presentadorVerificacionAmbiental.DatoAInterfazUsuario(verificacion);
                this.presentadorVerificacionAmbiental.ModoEdicion(true);
                this.presentadorVerificacionAmbiental.EstablecerTiposdeArchivo(lstTipos);
            }
            else throw new Exception("Se esparaba un objeto VerificacionBO");
        }
        public void EditarVerificacionMecanico()
        {
            this.vista.MostrarVerificacionMecanico();
            if (this.vista.UltimoTramite != null && this.vista.UltimoTramite is VerificacionBO)
            {
                VerificacionBO verificacion = new VerificacionBO();
                verificacion.Auditoria = new Basicos.BO.AuditoriaBO();
                verificacion.Archivos = new List<ArchivoBO>();
                VerificacionBO temp = (VerificacionBO)this.vista.UltimoTramite;
                verificacion.Activo = temp.Activo;
                verificacion.Archivos = temp.Archivos;
                verificacion.Auditoria.FC = temp.Auditoria.FC;
                verificacion.Auditoria.FUA = temp.Auditoria.FUA;
                verificacion.Auditoria.UC = temp.Auditoria.UC;
                verificacion.Auditoria.UUA = temp.Auditoria.UUA;
                verificacion.Folio = temp.Folio;
                verificacion.Resultado = temp.Resultado;
                verificacion.Tipo = temp.Tipo;
                verificacion.TipoVerificacion = temp.TipoVerificacion;
                verificacion.TramiteID = temp.TramiteID;
                verificacion.VigenciaFinal = temp.VigenciaFinal;
                verificacion.VigenciaInicial = temp.VigenciaInicial;

                List<TipoArchivoBO> lstTipos = tipoArchivoBR.Consultar(dctx, new TipoArchivoBO { Estatus = true });
                this.presentandorVerificacionMecanico.DatoAInterfazUsuario(verificacion);
                this.presentandorVerificacionMecanico.ModoEdicion(true);
                this.presentandorVerificacionMecanico.EstablecerTiposdeArchivo(lstTipos);
            }
            else throw new Exception("Se esparaba un objeto VerificacionBO");
        }
        private void ConfigurarEdicion()
        {
            try
            {
                if (this.vista.Tipo == ETipoTramite.FILTRO_AK)
                {
                    this.EditarFiltro();
                }
                if (this.vista.Tipo == ETipoTramite.GPS)
                {
                    this.EditarGPS();
                }
                if (this.vista.Tipo == ETipoTramite.PLACA_ESTATAL)
                {
                    this.EditarPlacaEstatal();
                }
                if (this.vista.Tipo == ETipoTramite.PLACA_FEDERAL)
                {
                    this.EditarPlacaFederal();
                }
                if (this.vista.Tipo == ETipoTramite.TENENCIA)
                {
                    this.EditarTenencia();
                }
                if (this.vista.Tipo == ETipoTramite.VERIFICACION_AMBIENTAL)
                {
                    this.EditarVerificacionAmbiental();
                }
                if (this.vista.Tipo == ETipoTramite.VERIFICACION_FISICO_MECANICA)
                {
                    this.EditarVerificacionMecanico();
                }
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Error al intentar configurar la Edición del trámite", ETipoMensajeIU.ERROR, nombreClase + ".ConfigurarEdicion(): " +ex.Message);
            }
        }
        #endregion
        #region Métodos para obtener datos de los controles de usuario
        private void ActualizarDatosTenencia()
        {
            if (this.vista.Tipo == ETipoTramite.TENENCIA)
            {
                string s =String.Empty;

                if (String.IsNullOrEmpty(s=this.presentadorTenencia.validarDatos()))
                {
                    TenenciaBO tenenciaBO = (TenenciaBO)this.presentadorTenencia.InterfazUsuarioADato();
                    tenenciaBO.Tramitable = this.vista.Tramitable;
                    TenenciaBO anteriorBO = (TenenciaBO)this.vista.UltimoTramite;
                    anteriorBO.Tramitable = this.vista.Tramitable;

                    if (tenenciaBO.TramiteID == anteriorBO.TramiteID)
                    {
                        #region SC0008
                        //obtener objeto SeguridadBO
                        SeguridadBO seguridad = ObtenerSeguridad();
                        if (seguridad == null) throw new Exception(nombreClase + ".ActualizarDatosTenencia():El objeto de SeguridadBO no debe se nulo");
                        #endregion
                        this.ActualizarTenencia(anteriorBO, tenenciaBO, seguridad);
                    }
                    else
                    {
                        throw new Exception("Error al actualizar los datos del trámite el Id anterior no coincide con el Id actual");
                    }
                }
                else
                {
                    this.MostrarMensaje("Es necesario proporcionar la siguiente información " + s,ETipoMensajeIU.ADVERTENCIA);
                }
            }
            else
            {
                throw new Exception("ActualizarDatosTenencia():El tipo de trámite no de Tenencia");
            }
        }
        private void ActualizarDatosAmbiental()
        {
            if (this.vista.Tipo == ETipoTramite.VERIFICACION_AMBIENTAL)
            {
                string s = "";
                if (String.IsNullOrEmpty(s = this.presentadorVerificacionAmbiental.validarDatos()))
                {
                    VerificacionBO verificacionBO = (VerificacionBO)this.presentadorVerificacionAmbiental.InterfazUsuarioADato();
                    verificacionBO.Tramitable = this.vista.Tramitable;
                    VerificacionBO anteriorBO = (VerificacionBO)this.vista.UltimoTramite;
                    anteriorBO.Tramitable = this.vista.Tramitable;
                    if (anteriorBO.TramiteID == verificacionBO.TramiteID)
                    {
                        #region SC0008
                        //obtener objeto SeguridadBO
                        SeguridadBO seguridad = ObtenerSeguridad();
                        if (seguridad == null) throw new Exception(nombreClase + ".ActualizarDatosAmbiental():El objeto de SeguridadBO no debe se nulo");
                        #endregion
                        this.ActualizarVerificacion(anteriorBO, verificacionBO, seguridad);
                    }
                    else
                    {
                        throw new Exception("Error al actualizar los datos del trámite el Id anterior no coincide con el Id actual");
                    }
                }
                else
                {
                    this.MostrarMensaje("Es necesario proporcionar la siguiente información " + s, ETipoMensajeIU.ADVERTENCIA);
                }
            }
            else
            {
                throw new Exception("ActualizarDatosAmbiental():El tipo de trámite no de Verificación Ambiental");
            }
        }
        private void ActualizarDatosMecanico()
        {
            if (this.vista.Tipo == ETipoTramite.VERIFICACION_FISICO_MECANICA)
            {
                string s = "";
                if (String.IsNullOrEmpty(s = this.presentandorVerificacionMecanico.validarDatos()))
                {
                    VerificacionBO verificacionBO = (VerificacionBO)this.presentandorVerificacionMecanico.InterfazUsuarioADato();
                    verificacionBO.Tramitable = this.vista.Tramitable;
                    VerificacionBO anteriorBO = (VerificacionBO)this.vista.UltimoTramite;
                    anteriorBO.Tramitable = this.vista.Tramitable;
                    if (anteriorBO.TramiteID == verificacionBO.TramiteID)
                    {
                        #region SC0008
                        //obtener objeto SeguridadBO
                        SeguridadBO seguridad = ObtenerSeguridad();
                        if (seguridad == null) throw new Exception(nombreClase + ".ActualizarDatosMecanico():El objeto de SeguridadBO no debe se nulo");
                        #endregion
                        this.ActualizarVerificacion(anteriorBO, verificacionBO, seguridad);
                    }
                    else
                    {
                        throw new Exception("Error al actualizar los datos del trámite el Id anterior no coincide con el Id actual");
                    }
                }
                else
                {
                    this.MostrarMensaje("Es necesario proporcionar la siguiente información " + s, ETipoMensajeIU.ADVERTENCIA);
                }
            }
            else
            {
                throw new Exception("ActualizarDatosMecanico():El tipo de trámite no de Verificación Físico Mecánico");
            }
        }
        private void ActualizarDatosFiltro()
        {
            if (this.vista.Tipo == ETipoTramite.FILTRO_AK)
            {
                string s = "";
                if (String.IsNullOrEmpty(s = presentadorFiltro.ValidarDatos()))
                {
                    FiltroAKBO filtroBO = (FiltroAKBO)presentadorFiltro.InterfazUsuarioADato();
                    filtroBO.Tramitable = this.vista.Tramitable;
                    FiltroAKBO anteriorBO = (FiltroAKBO)this.vista.UltimoTramite;
                    anteriorBO.Tramitable = this.vista.Tramitable;
                    if (filtroBO.TramiteID == anteriorBO.TramiteID)
                    {
                        #region SC0008
                        //obtener objeto SeguridadBO
                        SeguridadBO seguridad = ObtenerSeguridad();
                        if (seguridad == null) throw new Exception(nombreClase + ".ActualizarDatosFiltro():El objeto de SeguridadBO no debe se nulo");
                        #endregion
                        this.ActualizarTramiteFiltro(anteriorBO, filtroBO,seguridad);
                    }
                    else
                    {
                        throw new Exception("Error al actualizar los datos del trámite el Id anterior no coincide con el Id actual");
                    }
                }
                else
                {
                    this.MostrarMensaje("Es necesario proporcionar la siguiente información " + s, ETipoMensajeIU.ADVERTENCIA);
                }
            }
            else
            {
                throw new Exception("ActualizarDatosFiltro():El tipo de trámite no de Filtro AK");
            }

        }
        private void ActualizarDatosPlacaFederal()
        {
            string s = String.Empty;
            if (this.vista.Tipo == ETipoTramite.PLACA_FEDERAL)
            {
                if (String.IsNullOrEmpty(s = this.presentadorPlacaFederal.ValidarDatos()))
                {
                    PlacaFederalBO placaBO = (PlacaFederalBO)this.presentadorPlacaFederal.InterfazUsuarioADato();
                    placaBO.Tramitable = this.vista.Tramitable;
                    PlacaFederalBO anteriorBO = (PlacaFederalBO)this.vista.UltimoTramite;
                    anteriorBO.Tramitable = this.vista.Tramitable;
                    if (anteriorBO.TramiteID == placaBO.TramiteID)
                    {
                        #region SC0008
                        //obtener objeto SeguridadBO
                        SeguridadBO seguridad = ObtenerSeguridad();
                        if (seguridad == null) throw new Exception(nombreClase + ".ActualizarDatosPlacaFederal():El objeto de SeguridadBO no debe se nulo");
                        #endregion
                        this.ActualizarPlacaFederal(anteriorBO, placaBO, seguridad);
                    }
                    else
                    {
                        throw new Exception("Error al actualizar los datos del trámite el Id anterior no coincide con el Id actual");
                    }
                }
                else
                {
                    this.MostrarMensaje("Es necesario proporcionar la siguiente información " + s, ETipoMensajeIU.ADVERTENCIA);
                }
            }
            else
            {
                throw new Exception("ActualizarDatosPlacaFederal():El tipo de trámite no de Placa Federal");
            }
            
        }
        private void ActualizarDatosPlacaEstatal()
        {
            if (this.vista.Tipo == ETipoTramite.PLACA_ESTATAL)
            {
                string s = "";
                if (String.IsNullOrEmpty(s = this.presentadorPlacaEstatal.ValidarDatos()))
                {
                    PlacaEstatalBO placaBO = (PlacaEstatalBO)this.presentadorPlacaEstatal.InterfazUsuarioADato();
                    placaBO.Tramitable = this.vista.Tramitable;
                    PlacaEstatalBO anteriorBO = (PlacaEstatalBO)this.vista.UltimoTramite;
                    anteriorBO.Tramitable = this.vista.Tramitable;
                    if (anteriorBO.TramiteID == placaBO.TramiteID)
                    {
                        #region SC0008
                        //obtener objeto SeguridadBO
                        SeguridadBO seguridad = ObtenerSeguridad();
                        if (seguridad == null) throw new Exception(nombreClase + ".ActualizarDatosPlacaFederal():El objeto de SeguridadBO no debe se nulo");
                        #endregion
                        this.ActualizarPlacaEstatal(anteriorBO, placaBO, seguridad);
                    }
                    else
                    {
                        throw new Exception("Error al actualizar los datos del trámite el Id anterior no coincide con el Id actual");
                    }
                }
                else
                {
                    this.MostrarMensaje("Es necesario proporcionar la siguiente información " + s, ETipoMensajeIU.ADVERTENCIA);
                    return;
                }
            }
            else
            {
                throw new Exception("ActualizarDatosPlacaEstatal():El tipo de trámite no de Placa Estatal");
            }
        }
        private void ActualizarDatosGPS()
        {
            if (this.vista.Tipo == ETipoTramite.GPS)
            {
                string s = String.Empty;
                if (String.IsNullOrEmpty(s = this.presentadorGPS.ValidarDatos()))
                {
                    GPSBO gpsBO = (GPSBO)this.presentadorGPS.InterfazUsuarioADato();
                    GPSBO anteriorBO = (GPSBO)this.vista.UltimoTramite;
                    gpsBO.Tramitable = this.vista.Tramitable;
                    anteriorBO.Tramitable = this.vista.Tramitable;
                    if (gpsBO.TramiteID == anteriorBO.TramiteID)
                    {
                        #region SC0008
                        //obtener objeto SeguridadBO
                        SeguridadBO seguridad = ObtenerSeguridad();
                        if (seguridad == null) throw new Exception(nombreClase + ".ActualizarDatosPlacaFederal():El objeto de SeguridadBO no debe se nulo");
                        #endregion
                        this.ActualizarGPS(anteriorBO, gpsBO, seguridad);

                    }
                    else
                    {
                        throw new Exception("Error al actualizar los datos del trámite el Id anterior no coincide con el Id actual");
                    }
                }
                else
                {
                    this.MostrarMensaje("Es necesario proporcionar la siguiente información " + s, ETipoMensajeIU.ADVERTENCIA);
                }
            }
            else
            {
                throw new Exception("ActualizarDatosGPS():El tipo de trámite no de GPS");
            }
        }
        #endregion
        #region Métodos para actualizar los registros
        private void ActualizarVerificacion(VerificacionBO anteriorBO, VerificacionBO verificacionBO, SeguridadBO seguridad)
        {
            try
            {
                verificacionBR.ActualizarCompleto(dctx, verificacionBO, anteriorBO, seguridad);
                this.RedirigirADetalle();
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Ocurrio un error al intentar actualizar los datos", ETipoMensajeIU.ERROR, nombreClase +".ActualizarVerificacion(): " +ex.Message);
            }
        }
        private void ActualizarTenencia(TenenciaBO anteriorBO, TenenciaBO tenenciaBO, SeguridadBO seguridad)
        {
            try
            {
                tenenciaBR.ActualizarCompleto(dctx, tenenciaBO, anteriorBO, seguridad);
                this.RedirigirADetalle();
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Ocurrio un error al intentar actualizar los datos", ETipoMensajeIU.ERROR, nombreClase + ".ActualizarTenencia(): " + ex.Message);
            }
        }
        private void ActualizarPlacaFederal(PlacaFederalBO anteriorBO, PlacaFederalBO placaBO, SeguridadBO seguridad)
        {
            try
            {
                placaBR.ActualizarCompleto(dctx, placaBO, anteriorBO, seguridad);
                this.RedirigirADetalle();
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Ocurrio un error al intentar actualizar los datos", ETipoMensajeIU.ERROR, nombreClase +".ActualizarPlacaFederal():" + ex.Message);
            }
        }
        private void ActualizarPlacaEstatal(PlacaEstatalBO anteriorBO, PlacaEstatalBO placaBO, SeguridadBO seguridad)
        {
            try
            {
                placaBR.ActualizarCompleto(dctx, placaBO, anteriorBO, seguridad);
                this.RedirigirADetalle();
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Ocurrio un error al intentar actualizar los datos", ETipoMensajeIU.ERROR, nombreClase + ".ActualizarPlacaEstatal(): " + ex.Message);
            }
        }
        private void ActualizarTramiteFiltro(FiltroAKBO anteriorBO, FiltroAKBO filtroBO, SeguridadBO seguridad)
        {
            try
            {
                filtroBR.ActualizarCompleto(dctx, filtroBO, anteriorBO, seguridad);
                this.RedirigirADetalle();
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Ocurrio un error al intentanr actualizar los datos", ETipoMensajeIU.ERROR, nombreClase +".ActualizarTramiteFiltro(): " + ex.Message);
            }
        }
        private void ActualizarGPS(GPSBO anteriorBO, GPSBO gpsBO, SeguridadBO seguridad)
        {
            try
            {
                gpsBR.ActualizarCompleto(dctx, gpsBO, anteriorBO, seguridad);
                this.RedirigirADetalle();
            }
            catch(Exception ex)
            {
                this.MostrarMensaje("Ocurrio un error al intentar actualizar los datos", ETipoMensajeIU.ERROR, nombreClase +".ActualizarGPS(): "+ ex.Message);
            }
        }
        #endregion
        #region Métodos que son llamados por botones de guardar
        public void GuardarFiltro()
        {
            this.ActualizarDatosFiltro();
        }
        public void GuardarTenencia()
        {
            try
            {
                this.ActualizarDatosTenencia();
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Error al actualizar los datos", ETipoMensajeIU.ERROR, nombreClase + ".GuardarTenencia(): "+ ex.Message);
            }
        }
        public void GuardarAmbiental()
        {
            this.ActualizarDatosAmbiental();
            
        }
        public void GuardarMecanico()
        {
            this.ActualizarDatosMecanico();
            
        }
        public void GuardarPlacaEstatal()
        {
            this.ActualizarDatosPlacaEstatal();
            
        }
        public void GuardarPlacaFederal()
        {
            this.ActualizarDatosPlacaFederal();
            
        }
        public void GuardarGPS()
        {
            this.ActualizarDatosGPS();
        }
        #endregion

        public void LimpiarSesion(bool todo)
        {
            if (todo)
            {
                this.vista.LimpiarSesion();
                this.vista.LimpiarSesionDatosNavegacion();
            }
            else
            {
                this.vista.LimpiarSesion();
            }
        }
        public void ConsultarEquipoBO()
        {
            try
            {
                UnidadBO unidadTemp = new UnidadBO();
                if (this.vista.Tramitable.TipoTramitable == unidadTemp.TipoTramitable)
                {
                    unidadTemp.NumeroSerie = this.vista.Tramitable.DescripcionTramitable;
                    unidadTemp.UnidadID = this.vista.Tramitable.TramitableID;

                    List<UnidadBO> lstUnidad = unidadBR.ConsultarCompleto(dctx, unidadTemp, true);

                    if (lstUnidad.Count != 0)
                    {
                        if (lstUnidad.Count == 1)
                        {
                            UnidadBO unidad = lstUnidad[0];
                            if (unidad.UnidadID != null)
                            {
                                this.vista.NumeroSerie = unidad.NumeroSerie;

                                if (unidad.Modelo != null)
                                {
                                    this.vista.Modelo = unidad.Modelo.Nombre;
                                    if (unidad.Modelo.Marca != null)
                                    {
                                        this.vista.Marca = unidad.Modelo.Marca.Nombre;
                                    }

                                }
                            }
                            else
                                throw new Exception("La consulta devolvió una unidad nula");
                        }
                        else
                            throw new Exception("La consulta devolvió más una unidad");
                    }
                    else
                    {
                        throw new Exception("La consulta no devolvió ningún registro");
                    }
                }
                else
                {
                    throw new Exception("Se esperaba una unidad");
                }
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Error al consultar los datos de la unidad", ETipoMensajeIU.ERROR, nombreClase + ".ConsultarEquipoBO:" + ex.Message);
            }
        }
        public void RedirigirADetalle()
        {
            this.vista.EstablecerDatosNavegacion("DatosTramitable", this.vista.Tramitable);
            this.LimpiarSesion(true);
            this.vista.RedirigirADetalle();
        }

        #region SC0008
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
                if (!FacadeBR.ExisteAccion(this.dctx, "ACTUALIZARCOMPLETO", seguridadBO))
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
