using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using BPMO.Basicos.BO;
using BPMO.Facade.SDNI.BR;
using BPMO.Patterns.Creational.DataContext;
using BPMO.Primitivos.Enumeradores;
using BPMO.SDNI.Comun.BO;
using BPMO.SDNI.Comun.BR;
using BPMO.SDNI.Comun.PRE;
using BPMO.SDNI.Contratos.BO;
using BPMO.SDNI.Contratos.PSL.BO;
using BPMO.SDNI.Contratos.PSL.BR;
using BPMO.SDNI.Contratos.PSL.RPT;
using BPMO.SDNI.Contratos.PSL.VIS;
using BPMO.SDNI.Equipos.BO;
using BPMO.SDNI.Equipos.BR;
using BPMO.SDNI.Flota.BO;
using BPMO.SDNI.Flota.BOF;
using BPMO.SDNI.Flota.BR;
using BPMO.SDNI.Flota.PRE;
using BPMO.SDNI.Mantenimiento.BR;
using BPMO.SDNI.Tramites.BO;

namespace BPMO.SDNI.Contratos.PSL.PRE {
    /// <summary>
    /// Presentador para la página de registro de recepción de unidad
    /// </summary>
    public class RegistrarRecepcionPSLPRE {
        #region Atributos
        /// <summary>
        /// Vista para la página de registro de entrega de unidad
        /// </summary>
        private readonly IRegistrarRecepcionPSLVIS vista;
        /// <summary>
        /// Provee la conexión a la BD
        /// </summary>
        private readonly IDataContext dctx;
        /// <summary>
        /// Nombre de la clase que se usará en los mensajes de error
        /// </summary>
        private const string nombreClase = "RegistrarRecepcionUnidadPSLPRE";
        /// <summary>
        /// Controlador que ejecutará las accciones
        /// </summary>
        private readonly ContratoPSLBR controlador;
        #region SC0001
        /// <summary>
        /// Controlador de orden de servicio lavado
        /// </summary>
        private readonly RegistrarOrdenServicioLavadoBR ctrlOrdenLavado;
        #endregion
        /// <summary>
        /// Presentador del catálogo de documentos de entrega
        /// </summary>
        private ucCatalogoDocumentosPRE presentadorDocumentosEntrega;
        /// <summary>
        /// Presentador para el catálogo de documentos de entrega.
        /// </summary>
        public ucCatalogoDocumentosPRE PresentadorDocumentosEntrega {
            get { return presentadorDocumentosEntrega; }
            set { presentadorDocumentosEntrega = value; }
        }

        /// <summary>
        /// Presentador del catálogo de documentos
        /// </summary>
        private ucCatalogoDocumentosPRE presentadorDocumentosRecepcion;

        /// Presentador para el catálogo de documentos de entrega.
        /// </summary>
        public ucCatalogoDocumentosPRE PresentadorDocumentosRecepcion {
            get { return presentadorDocumentosRecepcion; }
            set { presentadorDocumentosRecepcion = value; }
        }

        /// <summary>
        /// Presentador para los equipos aliados de la unidad
        /// </summary>
        private ucEquiposAliadosUnidadPRE presentadorEquiposAliados;

        /// <summary>
        /// Presentador para los equipos aliados de la unidad
        /// </summary>
        public ucEquiposAliadosUnidadPRE PresentadorEquiposAliados {
            get { return presentadorEquiposAliados; }
            set { presentadorEquiposAliados = value; }
        }

        /// <summary>
        /// Presentador para los puntos de verificación de Excavadora Entrega
        /// </summary>
        private ucPuntosVerificacionExcavadoraPSLPRE presentadorExcavadoraEntrega;

        /// <summary>
        /// Presentador para los puntos de verificación de Excavadora Entrega
        /// </summary>
        public ucPuntosVerificacionExcavadoraPSLPRE PresentadorExcavadoraEntrega {
            get { return presentadorExcavadoraEntrega; }
            set { presentadorExcavadoraEntrega = value; }
        }

        /// <summary>
        /// Presentador para los puntos de verificación de Excavadora Recepción
        /// </summary>
        private ucPuntosVerificacionExcavadoraPSLPRE presentadorExcavadoraRecepcion;

        /// <summary>
        /// Presentador para los puntos de verificación de Excavadora Recepción
        /// </summary>
        public ucPuntosVerificacionExcavadoraPSLPRE PresentadorExcavadoraRecepcion {
            get { return presentadorExcavadoraRecepcion; }
            set { presentadorExcavadoraRecepcion = value; }
        }

        /// <summary>
        /// Presentador para los puntos de verificación de RetroExcavadora Entrega
        /// </summary>
        private ucPuntosVerificacionRetroExcavadoraPSLPRE presentadorRetroExcavadoraEntrega;

        /// <summary>
        /// Presentador para los puntos de verificación de RetroExcavadora Entrega
        /// </summary>
        public ucPuntosVerificacionRetroExcavadoraPSLPRE PresentadorRetroExcavadoraEntrega {
            get { return presentadorRetroExcavadoraEntrega; }
            set { presentadorRetroExcavadoraEntrega = value; }
        }

        /// <summary>
        /// Presentador para los puntos de verificación de RetroExcavadora Recepción
        /// </summary>
        private ucPuntosVerificacionRetroExcavadoraPSLPRE presentadorRetroExcavadoraRecepcion;

        /// <summary>
        /// Presentador para los puntos de verificación de RetroExcavadora Recepción
        /// </summary>
        public ucPuntosVerificacionRetroExcavadoraPSLPRE PresentadorRetroExcavadoraRecepcion {
            get { return presentadorRetroExcavadoraRecepcion; }
            set { presentadorRetroExcavadoraRecepcion = value; }
        }
        //
        /// <summary>
        /// Presentador para los puntos de verificación de MartilloHidraulico Entrega
        /// </summary>
        private ucPuntosVerificacionMartilloHidraulicoPSLPRE presentadorMartilloHidraulicoEntrega;

        /// <summary>
        /// Presentador para los puntos de verificación de MartilloHidraulico Entrega
        /// </summary>
        public ucPuntosVerificacionMartilloHidraulicoPSLPRE PresentadorMartilloHidraulicoEntrega {
            get { return presentadorMartilloHidraulicoEntrega; }
            set { presentadorMartilloHidraulicoEntrega = value; }
        }

        /// <summary>
        /// Presentador para los puntos de verificación de MartilloHidraulico Recepción
        /// </summary>
        private ucPuntosVerificacionMartilloHidraulicoPSLPRE presentadorMartilloHidraulicoRecepcion;

        /// <summary>
        /// Presentador para los puntos de verificación de MartilloHidraulico Recepción
        /// </summary>
        public ucPuntosVerificacionMartilloHidraulicoPSLPRE PresentadorMartilloHidraulicoRecepcion {
            get { return presentadorMartilloHidraulicoRecepcion; }
            set { presentadorMartilloHidraulicoRecepcion = value; }
        }

        /// <summary>
        /// Presentador para los puntos de verificación de TorresLuz Entrega
        /// </summary>
        private ucPuntosVerificacionTorresLuzPSLPRE presentadorTorresLuzEntrega;

        /// <summary>
        /// Presentador para los puntos de verificación de TorresLuz Entrega
        /// </summary>
        public ucPuntosVerificacionTorresLuzPSLPRE PresentadorTorresLuzEntrega {
            get { return presentadorTorresLuzEntrega; }
            set { presentadorTorresLuzEntrega = value; }
        }

        /// <summary>
        /// Presentador para los puntos de verificación de TorresLuz Recepción
        /// </summary>
        private ucPuntosVerificacionTorresLuzPSLPRE presentadorTorresLuzRecepcion;

        /// <summary>
        /// Presentador para los puntos de verificación de TorresLuz Recepción
        /// </summary>
        public ucPuntosVerificacionTorresLuzPSLPRE PresentadorTorresLuzRecepcion {
            get { return presentadorTorresLuzRecepcion; }
            set { presentadorTorresLuzRecepcion = value; }
        }

        /// <summary>
        /// Presentador para los puntos de verificación de VibroCompactador Entrega
        /// </summary>
        private ucPuntosVerificacionVibroCompactadorPSLPRE presentadorVibroCompactadorEntrega;

        /// <summary>
        /// Presentador para los puntos de verificación de VibroCompactador Entrega
        /// </summary>
        public ucPuntosVerificacionVibroCompactadorPSLPRE PresentadorVibroCompactadorEntrega {
            get { return presentadorVibroCompactadorEntrega; }
            set { presentadorVibroCompactadorEntrega = value; }
        }

        /// <summary>
        /// Presentador para los puntos de verificación de VibroCompactador Recepción
        /// </summary>
        private ucPuntosVerificacionVibroCompactadorPSLPRE presentadorVibroCompactadorRecepcion;

        /// <summary>
        /// Presentador para los puntos de verificación de VibroCompactador Recepción
        /// </summary>
        public ucPuntosVerificacionVibroCompactadorPSLPRE PresentadorVibroCompactadorRecepcion {
            get { return presentadorVibroCompactadorRecepcion; }
            set { presentadorVibroCompactadorRecepcion = value; }
        }

        /// <summary>
        /// Presentador para los puntos de verificación de Pistola Neumática Entrega
        /// </summary>
        private ucPuntosVerificacionPistolaNeumaticaPSLPRE presentadorPistolaNeumaticaEntrega;

        /// <summary>
        /// Presentador para los puntos de verificación de Pistola Neumática Entrega
        /// </summary>
        public ucPuntosVerificacionPistolaNeumaticaPSLPRE PresentadorPistolaNeumaticaEntrega {
            get { return presentadorPistolaNeumaticaEntrega; }
            set { presentadorPistolaNeumaticaEntrega = value; }
        }

        /// <summary>
        /// Presentador para los puntos de verificación de Pistola Neumática Recepción
        /// </summary>
        private ucPuntosVerificacionPistolaNeumaticaPSLPRE presentadorPistolaNeumaticaRecepcion;

        /// <summary>
        /// Presentador para los puntos de verificación de Pistola Neumática Recepción
        /// </summary>
        public ucPuntosVerificacionPistolaNeumaticaPSLPRE PresentadorPistolaNeumaticaRecepcion {
            get { return presentadorPistolaNeumaticaRecepcion; }
            set { presentadorPistolaNeumaticaRecepcion = value; }
        }

        /// <summary>
        /// Presentador para los puntos de verificación Genérico Entrega y Recepción Generación (Entrega).
        /// </summary>
        private ucPuntosVerificacionEntregaRecepcionPSLPRE presentadorEntregaRecepcionE;

        /// <summary>
        /// Presentador para los puntos de verificación Genérico Entrega y Recepción Generación (Entrega).
        /// </summary>
        public ucPuntosVerificacionEntregaRecepcionPSLPRE PresentadorEntregaRecepcionE {
            get { return presentadorEntregaRecepcionE; }
            set { presentadorEntregaRecepcionE = value; }
        }

        /// <summary>
        /// Presentador para los puntos de verificación Genérico Entrega y Recepción Generación (Recepción).
        /// </summary>
        private ucPuntosVerificacionEntregaRecepcionPSLPRE presentadorEntregaRecepcionR;

        /// <summary>
        /// Presentador para los puntos de verificación Genérico Entrega y Recepción Generación (Recepción).
        /// </summary>
        public ucPuntosVerificacionEntregaRecepcionPSLPRE PresentadorEntregaRecepcionR {
            get { return presentadorEntregaRecepcionR; }
            set { presentadorEntregaRecepcionR = value; }
        }

        /// <summary>
        /// Presentador para los puntos de verificación de MontaCarga Entrega
        /// </summary>
        private ucPuntosVerificacionMontaCargaPSLPRE presentadorMontaCargaEntrega;

        /// <summary>
        /// Presentador para los puntos de verificación de MontaCarga Entrega
        /// </summary>
        public ucPuntosVerificacionMontaCargaPSLPRE PresentadorMontaCargaEntrega {
            get { return presentadorMontaCargaEntrega; }
            set { presentadorMontaCargaEntrega = value; }
        }

        /// <summary>
        /// Presentador para los puntos de verificación de MontaCarga Recepción
        /// </summary>
        private ucPuntosVerificacionMontaCargaPSLPRE presentadorMontaCargaRecepcion;

        /// <summary>
        /// Presentador para los puntos de verificación de Montacarga Recepción
        /// </summary>
        public ucPuntosVerificacionMontaCargaPSLPRE PresentadorMontacargaRecepcion {
            get { return presentadorMontaCargaRecepcion; }
            set { presentadorMontaCargaRecepcion = value; }
        }

        /// <summary>
        /// Presentador para los puntos de verificación de presentadorMiniCargadorEntrega
        /// </summary>
        private ucPuntosVerificacionMiniCargadorPSLPRE presentadorMiniCargadorEntrega;

        /// <summary>
        /// Presentador para los puntos de verificación de presentadorMiniCargadorEntrega
        /// </summary>
        public ucPuntosVerificacionMiniCargadorPSLPRE PresentadorMiniCargadorEntrega {
            get { return presentadorMiniCargadorEntrega; }
            set { presentadorMiniCargadorEntrega = value; }
        }

        /// <summary>
        /// Presentador para los puntos de verificación de presentadorMiniCargadorEntrega
        /// </summary>
        private ucPuntosVerificacionMiniCargadorPSLPRE presentadorMiniCargadorRecepcion;

        /// <summary>
        /// Presentador para los puntos de verificación de presentadorMiniCargadorEntrega
        /// </summary>
        public ucPuntosVerificacionMiniCargadorPSLPRE PresentadorMiniCargadorRecepcion {
            get { return presentadorMiniCargadorRecepcion; }
            set { presentadorMiniCargadorRecepcion = value; }
        }

        /// <summary>
        /// Presentador para los puntos de verificación de Compresor Portatil (Entrega).
        /// </summary>
        private ucPuntosVerificacionCompresoresPortatilesPSLPRE presentadorCompresoresPortatilesE;

        /// <summary>
        /// Presentador para los puntos de verificación de Compresor Portatil (Entrega).
        /// </summary>
        public ucPuntosVerificacionCompresoresPortatilesPSLPRE PresentadorCompresoresPortatilesE {
            get { return presentadorCompresoresPortatilesE; }
            set { presentadorCompresoresPortatilesE = value; }
        }

        /// <summary>
        /// Presentador para los puntos de verificación de Compresor Portatil (Recepcion).
        /// </summary>
        private ucPuntosVerificacionCompresoresPortatilesPSLPRE presentadorCompresoresPortatilesR;

        /// <summary>
        /// Presentador para los puntos de verificación de Compresor Portatil (Recepcion).
        /// </summary>
        public ucPuntosVerificacionCompresoresPortatilesPSLPRE PresentadorCompresoresPortatilesR {
            get { return presentadorCompresoresPortatilesR; }
            set { presentadorCompresoresPortatilesR = value; }
        }

        /// <summary>
        /// Presentador para los puntos de verificación de equipos sub arrendados (Entrega).
        /// </summary>
        private ucPuntosVerificacionSubArrendadoPSLPRE presentadorSubArrendadosE;

        /// <summary>
        /// Presentador para los puntos de verificación de equipos sub arrendados (Entrega).
        /// </summary>
        public ucPuntosVerificacionSubArrendadoPSLPRE PresentadorSubArrendadosE {
            get { return this.presentadorSubArrendadosE; }
            set { this.presentadorSubArrendadosE = value; }
        }

        /// <summary>
        /// Presentador para los puntos de verificación de equipos sub arrendados (Recepción).
        /// </summary>
        private ucPuntosVerificacionSubArrendadoPSLPRE presentadorSubArrendadosR;

        /// <summary>
        /// Presentador para los puntos de verificación de equipos sub arrendados (Recepción).
        /// </summary>
        public ucPuntosVerificacionSubArrendadoPSLPRE PresentadorSubArrendadosR {
            get { return this.presentadorSubArrendadosR; }
            set { this.presentadorSubArrendadosR = value; }
        }
        /// <summary>
        /// Presentador para los puntos de verificación de Plataforma Tijeras (Entrega).
        /// </summary>
        private ucPuntosVerificacionPlataformaTijerasPSLPRE presentadorPlataformaTijerasE;

        /// <summary>
        /// Presentador para los puntos de verificación de Plataforma Tijeras (Entrega).
        /// </summary>
        public ucPuntosVerificacionPlataformaTijerasPSLPRE PresentadorPlataformaTijerasE {
            get { return presentadorPlataformaTijerasE; }
            set { presentadorPlataformaTijerasE = value; }
        }

        /// <summary>
        /// Presentador para los puntos de verificación de Plataforma Tijeras (Recepcion).
        /// </summary>
        private ucPuntosVerificacionPlataformaTijerasPSLPRE presentadorPlataformaTijerasR;

        /// <summary>
        /// Presentador para los puntos de verificación de Plataforma Tijeras (Recepcion).
        /// </summary>
        public ucPuntosVerificacionPlataformaTijerasPSLPRE PresentadorPlataformaTijerasR {
            get { return presentadorPlataformaTijerasR; }
            set { presentadorPlataformaTijerasR = value; }
        }

        private ucPuntosVerificacionMotoNiveladoraPSLPRE presentadorMotoNiveladoraE;
        /// <summary>
        /// Presentador checklist MotoNiveladora
        /// </summary>
        public ucPuntosVerificacionMotoNiveladoraPSLPRE PresentadorMotoNiveladoraE {
            get { return presentadorMotoNiveladoraE; }
            set { this.presentadorMotoNiveladoraE = value; }
        }

        private ucPuntosVerificacionMotoNiveladoraPSLPRE presentadorMotoNiveladoraR;
        /// <summary>
        /// Presentador checklist MotoNiveladora
        /// </summary>
        public ucPuntosVerificacionMotoNiveladoraPSLPRE PresentadorMotoNiveladoraR {
            get { return presentadorMotoNiveladoraR; }
            set { this.presentadorMotoNiveladoraR = value; }
        }
        /// <summary>
        /// Controlador de la clase ModuloBR
        /// </summary>
        ModuloBR moduloBR;
        #endregion

        #region Constructor
        public RegistrarRecepcionPSLPRE(IRegistrarRecepcionPSLVIS vista) {
            try {
                if (ReferenceEquals(vista, null))
                    throw new Exception(String.Format("{0}: La vista proporcionada no puede ser nula", nombreClase));
                this.vista = vista;
                this.controlador = new ContratoPSLBR();
                this.dctx = FacadeBR.ObtenerConexion();
            } catch (Exception ex) {
                this.vista.MostrarMensaje("Inconsistencia en los parámetros de configuración", ETipoMensajeIU.ERROR, nombreClase + ".RegistrarRecepcionUnidadPRE" + Environment.NewLine + ex.Message);
            }
        }
        #endregion

        #region Métodos
        /// <summary>
        /// Retrocede la página
        /// </summary>
        public void RetrocederPagina() {
            int? paginaActual = this.vista.PaginaActual;
            if (paginaActual == null)
                throw new Exception("La página actual es nula.");
            if (paginaActual <= 0)
                throw new Exception("La página actual es menor o igual a cero y, por lo tanto, no se puede retroceder.");

            this.EstablecerOpcionesPaginas(paginaActual.Value - 1);

            this.vista.EstablecerPagina(paginaActual.Value - 1);
        }

        /// <summary>
        /// Avanza la página
        /// </summary>
        public void AvanzarPagina() {
            int? paginaActual = this.vista.PaginaActual;
            if (paginaActual == null)
                throw new Exception("La página actual es nula.");
            if (paginaActual >= 4)
                throw new Exception("La página actual es mayor o igual a 6 y, por lo tanto, no se puede avanzar.");

            if (paginaActual == 1) {
                string s = string.Empty;
                if (!(String.IsNullOrEmpty(s = this.ValidarFechasListado()))) {
                    this.vista.MostrarMensaje(s, ETipoMensajeIU.INFORMACION);
                    return;
                }
                if (!(String.IsNullOrEmpty(s = this.ValidarKilometraje()))) {
                    this.vista.MostrarMensaje(s, ETipoMensajeIU.INFORMACION);
                    return;
                }
                if (!(String.IsNullOrEmpty(s = this.ValidarHoja1()))) {
                    this.vista.MostrarMensaje(s, ETipoMensajeIU.INFORMACION);
                    return;
                }
            }

            if (paginaActual == 2) {
                #region Validar campos
                string s;

                //Validación de campos obligatorios            
                if ((s = this.ValidarCampos()) != null) {
                    this.vista.MostrarMensaje(s, ETipoMensajeIU.ADVERTENCIA, null);
                    return;
                }

                //Validamos los kilometrajes de la unidad
                if ((s = this.ValidarKilometraje()) != null) {
                    this.vista.MostrarMensaje(s, ETipoMensajeIU.ADVERTENCIA, null);
                    return;
                }

                string mensajeDatosFaltantes = "Es necesario capturar observaciones para los siguientes puntos de verificación: \n {0}";
                switch (this.vista.UnidadOperativaID) {
                    case (int)ETipoEmpresa.Construccion:
                        if ((EAreaConstruccion)this.vista.Area == EAreaConstruccion.RO || (EAreaConstruccion)this.vista.Area == EAreaConstruccion.ROC) {
                            switch ((ETipoUnidad)Enum.Parse(typeof(ETipoUnidad), this.vista.TipoListadoVerificacionPSL.ToString())) {
                                case ETipoUnidad.LV_EXCAVADORA:
                                    if ((s = this.presentadorExcavadoraRecepcion.ValidarCampos()) != null) {
                                        if (string.IsNullOrEmpty(this.vista.ObservacionesRecepcion)) {
                                            this.vista.MostrarMensaje(s, ETipoMensajeIU.ADVERTENCIA, null);
                                            return;
                                        }
                                    }
                                    break;
                                case ETipoUnidad.LV_RETRO_EXCAVADORA:
                                    string datosFaltantesRetroExcavadora;
                                    if (this.presentadorRetroExcavadoraRecepcion.FaltanDatosObligatorios(out datosFaltantesRetroExcavadora) && string.IsNullOrEmpty(this.vista.ObservacionesRecepcion)) {
                                        this.vista.MostrarMensaje(string.Format(mensajeDatosFaltantes, datosFaltantesRetroExcavadora), ETipoMensajeIU.ADVERTENCIA, null);
                                        return;
                                    }
                                    break;
                                case ETipoUnidad.LV_COMPRESOR:
                                    if ((s = this.presentadorCompresoresPortatilesR.ValidarCampos()) != null && string.IsNullOrEmpty(this.vista.ObservacionesRecepcion)) {
                                        this.vista.MostrarMensaje(s, ETipoMensajeIU.ADVERTENCIA, null);
                                        return;
                                    }
                                    break;
                                case ETipoUnidad.LV_VIBRO_COMPACTADOR:
                                    if (this.presentadorVibroCompactadorRecepcion.ValidarCampos() != null && string.IsNullOrEmpty(this.vista.ObservacionesRecepcion)) {
                                        s = this.presentadorVibroCompactadorRecepcion.ValidarCampos();
                                        this.vista.MostrarMensaje(s, ETipoMensajeIU.ADVERTENCIA, null);
                                        return;
                                    }
                                    break;

                                case ETipoUnidad.LV_PISTOLA_NEUMATICA:
                                    if (this.presentadorPistolaNeumaticaRecepcion.ValidarCampos() != null && string.IsNullOrEmpty(this.vista.ObservacionesRecepcion)) {
                                        s = this.presentadorPistolaNeumaticaRecepcion.ValidarCampos();
                                        this.vista.MostrarMensaje(s, ETipoMensajeIU.ADVERTENCIA, null);
                                        return;
                                    }
                                    break;
                                case ETipoUnidad.LV_MARTILLO_HIDRAULICO:
                                    if (this.presentadorMartilloHidraulicoRecepcion.ValidarCampos() != null && string.IsNullOrEmpty(this.vista.ObservacionesRecepcion)) {
                                        s = this.presentadorMartilloHidraulicoRecepcion.ValidarCampos();
                                        this.vista.MostrarMensaje(s, ETipoMensajeIU.ADVERTENCIA, null);
                                        return;
                                    }
                                    break;
                                case ETipoUnidad.LV_TORRES_LUZ:
                                    if (this.presentadorTorresLuzRecepcion.ValidarCampos() != null && string.IsNullOrEmpty(this.vista.ObservacionesRecepcion)) {
                                        s = this.presentadorTorresLuzRecepcion.ValidarCampos();
                                        this.vista.MostrarMensaje(s, ETipoMensajeIU.ADVERTENCIA, null);
                                        return;
                                    }
                                    break;
                                case ETipoUnidad.LV_MOTONIVELADORA:
                                    if (this.presentadorMotoNiveladoraR.ValidarCampos() != null && string.IsNullOrEmpty(this.vista.ObservacionesRecepcion)) {
                                        s = this.presentadorMotoNiveladoraR.ValidarCampos();
                                        this.vista.MostrarMensaje(s, ETipoMensajeIU.ADVERTENCIA, null);
                                        return;
                                    }
                                    break;

                                case ETipoUnidad.LV_MONTACARGA:
                                    if (this.presentadorMontaCargaRecepcion.ValidarCampos() != null && string.IsNullOrEmpty(this.vista.ObservacionesRecepcion)) {
                                        s = this.presentadorMontaCargaRecepcion.ValidarCampos();
                                        this.vista.MostrarMensaje(s, ETipoMensajeIU.ADVERTENCIA, null);
                                        return;
                                    }
                                    break;

                                case ETipoUnidad.LV_MINICARGADOR:
                                    if (this.presentadorMiniCargadorRecepcion.ValidarCampos() != null && string.IsNullOrEmpty(this.vista.ObservacionesRecepcion)) {
                                        s = this.presentadorMiniCargadorRecepcion.ValidarCampos();
                                        this.vista.MostrarMensaje(s, ETipoMensajeIU.ADVERTENCIA, null);
                                        return;
                                    }
                                    break;
                                case ETipoUnidad.LV_PLATAFORMA_TIJERAS:
                                    if ((s = this.presentadorPlataformaTijerasR.ValidarCampos()) != null && string.IsNullOrEmpty(this.vista.ObservacionesRecepcion)) {
                                        s = this.presentadorPlataformaTijerasR.ValidarCampos();
                                        this.vista.MostrarMensaje(s, ETipoMensajeIU.ADVERTENCIA, null);
                                        return;
                                    }
                                    break;
                            }
                        }
                        if ((EAreaConstruccion)this.vista.Area == EAreaConstruccion.RE) {
                            if ((s = this.presentadorSubArrendadosR.ValidarCampos()) != null) {
                                if (string.IsNullOrEmpty(this.vista.ObservacionesRecepcion)) {
                                    this.vista.MostrarMensaje(s, ETipoMensajeIU.ADVERTENCIA, null);
                                    return;
                                }
                            }
                        }
                        break;
                    case (int)ETipoEmpresa.Generacion:
                    case (int)ETipoEmpresa.Equinova:
                        if ((s = this.presentadorEntregaRecepcionR.ValidarCampos()) != null) {
                            if (string.IsNullOrEmpty(this.vista.ObservacionesRecepcion)) {
                                this.vista.MostrarMensaje(s, ETipoMensajeIU.ADVERTENCIA, null);
                                return;
                            }
                        }
                        break;
                }

                //Validación de campos obligatorios            
                if ((s = this.ValidarCampos()) != null) {
                    this.vista.MostrarMensaje(s, ETipoMensajeIU.ADVERTENCIA, null);
                    return;
                }

                #endregion
            }

            this.EstablecerOpcionesPaginas(paginaActual.Value + 1);

            this.vista.EstablecerPagina(paginaActual.Value + 1);
        }

        /// <summary>
        /// Establece la página que se desea visualizar
        /// </summary>
        /// <param name="numeroPagina">Número de página</param>
        public void IrAPagina(int numeroPagina) {
            if (numeroPagina < 0 || numeroPagina > 4)
                throw new Exception("La paginación va de 0 al 4.");

            this.EstablecerOpcionesPaginas(numeroPagina);

            this.vista.EstablecerPagina(numeroPagina);
        }

        /// <summary>
        /// Configura las acciones que se pueden ejecutar en la vista
        /// </summary>
        /// <param name="numeroPagina">Número de página visible</param>
        private void EstablecerOpcionesPaginas(int numeroPagina) {
            this.vista.PermitirRegresar(true);
            this.vista.PermitirContinuar(true);
            this.vista.PermitirCancelar(true);

            this.vista.OcultarContinuar(false);
            this.vista.OcultarTerminar(true);

            if (numeroPagina <= 0) {
                this.vista.PermitirRegresar(false);
            }

            if (numeroPagina == 3) {
                this.vista.PermitirContinuar(false);

                this.vista.OcultarContinuar(true);
                this.vista.OcultarTerminar(false);
            }

            if (numeroPagina >= 6) {
                this.vista.PermitirRegresar(false);
                this.vista.PermitirContinuar(false);
                this.vista.PermitirCancelar(false);

                this.vista.OcultarContinuar(false);
                this.vista.OcultarTerminar(true);
            }
        }

        /// <summary>
        /// Valida los datos de la hoja 1 para continuar con el registro del check list
        /// </summary>
        /// <returns>Campos que presenten inconsistencias</returns>
        private string ValidarHoja1() {
            StringBuilder s = new StringBuilder();

            if (!vista.LineaContratoID.HasValue)
                s.Append("Contrato, ");
            if (!vista.UsuarioID.HasValue)
                s.Append("Usuario que entrega la unidad, ");
            if (!vista.Horometro.HasValue)
                s.Append("Horómetro de entrega, ");
            if (!vista.Horometro.HasValue)
                s.Append("Horas de entrega, ");
            if (!vista.Combustible.HasValue)
                s.Append("Nivel de combustible, ");

            if (s.Length > 0)
                return "Los siguientes campos no pueden estar vacíos: \n" + s.ToString().Substring(0, s.Length - 2);
            return null;
        }

        /// <summary>
        /// Valida la extensión de un archivo para confirmar que sea una imagen
        /// </summary>
        /// <param name="extencion">Extensión que se desea validar</param>
        /// <returns>Verdadero si cumple con las extensiones configuradas, falso si no</returns>
        private bool ValidaExtensionImagen(string extencion) {
            switch (extencion) {
                case "jpg":
                case "jpeg":
                case "png":
                case "gif":
                case "bmp":
                    return true;
                default:
                    return false;
            }
        }

        /// <summary>
        /// Valida que el tipo de archivo seleccionado este permitido dentro de los tipos configurados
        /// </summary>
        /// <param name="tipo">Tipo de archivo que desea validar</param>
        /// <returns>Verdadero si la extensión se encuentra, falso si no</returns>
        private TipoArchivoBO ValidarArchivo(String tipo) {
            List<TipoArchivoBO> tiposArchivo = (List<TipoArchivoBO>)this.vista.TiposArchivoImagen;
            if (tiposArchivo != null) {
                TipoArchivoBO tipoArchivoTemp = tiposArchivo.Find(delegate(TipoArchivoBO ta) { return ta.Extension == tipo; });
                if (tipoArchivoTemp != null) {
                    return tipoArchivoTemp;
                } else {
                    this.vista.MostrarMensaje("El archivo no fué cargado.", ETipoMensajeIU.ERROR,
                        "La extensión del archivo no se encuentra en la lista de tipos permitidos");
                }
            } else {
                this.vista.MostrarMensaje("El archivo no fué cargado.", ETipoMensajeIU.ERROR, "No hay una lista de tipos de archivo cargada");
            }
            return null;
        }

        /// <summary>
        /// Valida el kilometraje de la unidad al momento de la recepción
        /// </summary>
        /// <returns>Inconsistencias en la información</returns>
        public string ValidarKilometraje() {
            if (!this.vista.HorometroEntrega.HasValue)
                return " Es necesario proporcionar el horómetro de entrega de la unidad al cliente. ";

            if (!this.vista.Horometro.HasValue)
                return " Es necesario proporcionar el horómetro de recepción de la unidad. ";

            if (this.vista.Horometro.Value < this.vista.HorometroEntrega.Value)
                return " El horómetro de recepción es menor al horómetro de entrega. ";

            if (!this.vista.HorometroEntrega.HasValue)
                return " Es necesario proporcionar las horas de entrega de la unidad al cliente. ";

            if (!this.vista.Horometro.HasValue)
                return " Es necesario proporcionar las horas de recepción de la unidad. ";

            if (this.vista.Horometro.Value < this.vista.HorometroEntrega.Value)
                return " Las horas de recepción son menores a las horas de entrega. ";

            if (this.vista.HorometroEntrega.Value == this.vista.Horometro.Value)
                this.PrepararCancelacion();

            return null;
        }

        /// <summary>
        /// Prepara la Ui para cancelar el contrato
        /// </summary>
        private void PrepararCancelacion() {
            this.vista.MostrarMensaje("El contrato al que pertenece el check list, va a ser cancelado.", ETipoMensajeIU.ADVERTENCIA, null);
        }

        /// <summary>
        /// Valida las fechas proporcionadas por el usuario para el check list
        /// </summary>
        /// <returns>Campos que presenten inconsistencias</returns>
        private string ValidarFechasListado() {
            if (!this.vista.FechaListado.HasValue)
                return " Es necesario proporcionar la fecha en la que se realiza el check list. ";

            if (this.vista.FechaListado.Value > DateTime.Now.Date)
                return " La fecha en la que se realiza el check list no debe ser posterior a hoy. ";

            if (!this.vista.HoraListado.HasValue)
                return " Es necesario proporcionar la hora en la que se realiza el check list.  ";

            if (!this.vista.FechaContrato.HasValue)
                return " Es necesario proporcionar la fecha del contrato para el registro del check list. ";

            if (!this.vista.HoraContrato.HasValue)
                return " Es necesario proporcionar la hora del contrato para el registro del check list. ";

            if (!this.vista.FechaListadoEntrega.HasValue)
                return " Es necesario proporcionar la fecha de entrega de la unidad antes de continuar con el registro del check list. ";

            if (!this.vista.HoraListadoEntrega.HasValue)
                return " Es necesario proporcionar la hora de entrega de la unidad antes de continuar con el registro del check list. ";

            var dateCK = this.vista.FechaListadoEntrega;
            dateCK = dateCK.Value.Add(this.vista.HoraListadoEntrega.Value);

            var date = this.vista.FechaListado;
            date = date.Value.Add(this.vista.HoraListado.Value);

            if (date < dateCK)
                return " La fecha del check list de recepción no puede ser menor a la fecha del check list de entrega. ";

            if (date.Value < this.vista.FechaContrato.Value)
                return " La fecha de registro del check list no puede ser menor a la fecha de registro del contrato. ";

            return null;
        }

        /// <summary>
        /// Valida la información proporcionada por el usuario
        /// </summary>
        /// <returns>Inconsistencias en la información</returns>
        private string ValidarCampos() {
            StringBuilder s = new StringBuilder();

            string sf = null;

            if ((sf = this.ValidarKilometraje()) != null)
                return sf;

            if ((sf = this.ValidarFechasListado()) != null)
                return sf;

            if ((sf = this.ValidarTanque()) != null)
                return sf;

            if (!vista.LineaContratoID.HasValue)
                s.Append("Contrato, ");
            if (!vista.UsuarioID.HasValue)
                s.Append("Usuario que recibe la unidad, ");
            if (!vista.Horometro.HasValue)
                s.Append("Horómetro de recepción, ");
            if (!vista.Horometro.HasValue)
                s.Append("Horas de refrigeración de recepción, ");
            if (!vista.Combustible.HasValue)
                s.Append("Nivel de combustible, ");

            if (s.Length > 0)
                return "Los siguientes campos no pueden estar vacíos: \n" + s.ToString().Substring(0, s.Length - 2);
            return null;
        }

        /// <summary>
        /// Valida la capacidad del tanque de la unidad respecto al combustible proporcionado
        /// </summary>
        /// <returns>Mensaje de advertencia</returns>
        public string ValidarTanque() {
            if (!this.vista.Combustible.HasValue)
                return "Es necesario proporcionar la cantidad actual de combustible en la unidad que se esta entregando";
            if (this.vista.Combustible.Value < 0)
                return "Es necesario proporcionar la cantidad actual de combustible en la unidad que se esta entregando";
            if (this.vista.CapacidadTanque.HasValue) {
                if (this.vista.Combustible.Value > this.vista.CapacidadTanque.Value)
                    return string.Format(" La cantidad actual de combustible es superior a la capacidad total del tanque de la unidad. La capacidad total es: {0}", string.Format("{0:#,##0.00##}", this.vista.CapacidadTanque.Value));
            }

            return null;
        }

        /// <summary>
        /// Obtiene la información de la unidad seleccionada
        /// </summary>
        /// <returns>Unidad a la que se le realiza el check list</returns>
        private EquipoBO InterfazUsuarioADatoUnidad() {
            UnidadBO unidad = new UnidadBO();

            if (this.vista.UnidadID.HasValue)
                unidad.UnidadID = this.vista.UnidadID;
            if (this.vista.EquipoID.HasValue)
                unidad.EquipoID = this.vista.EquipoID;
            if (!string.IsNullOrEmpty(this.vista.NumeroSerie) && !string.IsNullOrWhiteSpace(this.vista.NumeroSerie))
                unidad.NumeroSerie = this.vista.NumeroSerie;
            if (!string.IsNullOrEmpty(this.vista.NumeroEconomico) && !string.IsNullOrWhiteSpace(this.vista.NumeroEconomico))
                unidad.NumeroEconomico = this.vista.NumeroEconomico;

            return unidad;
        }

        private object InterfazUsuarioADato() {
            ContratoPSLBO obj = new ContratoPSLBO();
            LineaContratoPSLBO linea = new LineaContratoPSLBO();

            if (this.vista.ContratoID.HasValue) {
                obj.ContratoID = this.vista.ContratoID.Value;
            }
            if (this.vista.TipoContrato.HasValue)
                obj.Tipo = (ETipoContrato)this.vista.TipoContrato.Value;
            if (this.vista.UnidadOperativaID.HasValue) {
                var sucursal = new SucursalBO { UnidadOperativa = new UnidadOperativaBO { Id = this.vista.UnidadOperativaID } };
                obj.Sucursal = new SucursalBO();
                obj.Sucursal = sucursal;
            }
            if (this.vista.LineaContratoID.HasValue)
                linea.LineaContratoID = this.vista.LineaContratoID.Value;

            switch (this.vista.UnidadOperativaID) {
                case (int)ETipoEmpresa.Construccion:
                    if ((EAreaConstruccion)this.vista.Area == EAreaConstruccion.RO || (EAreaConstruccion)this.vista.Area == EAreaConstruccion.ROC) {
                        switch ((ETipoUnidad)Enum.Parse(typeof(ETipoUnidad), this.vista.TipoListadoVerificacionPSL.Value.ToString())) {
                            case ETipoUnidad.LV_EXCAVADORA:
                                #region Datos Excavadora
                                ListadoVerificacionExcavadoraBO bo = new ListadoVerificacionExcavadoraBO();
                                bo.LineaContratoPSLID = this.vista.LineaContratoID.Value;
                                bo = (ListadoVerificacionExcavadoraBO)this.presentadorExcavadoraRecepcion.InterfazUsuarioADato();
                                if (this.vista.TipoListado.HasValue)
                                    bo.Tipo = (ETipoListadoVerificacion)this.vista.TipoListado.Value;
                                if (this.vista.FechaListado.HasValue)
                                    bo.Fecha = this.vista.FechaListado.Value;
                                if (this.vista.FechaListado.HasValue && this.vista.HoraListado.HasValue)
                                    bo.Fecha = bo.Fecha.Value.Add(this.vista.HoraListado.Value);
                                if (this.vista.Horometro.HasValue)
                                    bo.Horometro = this.vista.Horometro.Value;
                                if (this.vista.Combustible.HasValue)
                                    bo.Combustible = this.vista.Combustible.Value;

                                if (!string.IsNullOrEmpty(this.vista.ObservacionesRecepcion))
                                    bo.Observaciones = this.vista.ObservacionesRecepcion;

                                bo.Auditoria = new AuditoriaBO
                                {
                                    FC = DateTime.Now,
                                    FUA = DateTime.Now,
                                    UC = this.vista.UsuarioID,
                                    UUA = this.vista.UsuarioID
                                };

                                //agregamos los archivos adjuntos al listado
                                bo.Adjuntos = this.presentadorDocumentosRecepcion.Vista.NuevosArchivos;

                                if (ReferenceEquals(linea.ListadosVerificacion, null))
                                    linea.ListadosVerificacion = new List<AVerificacionLineaPSLBO>();

                                if (!bo.Tipo.HasValue)
                                    bo.Tipo = ETipoListadoVerificacion.RECEPCION;

                                //Agregamos la unidad a la linea
                                linea.Equipo = this.InterfazUsuarioADatoUnidad();

                                linea.AgregarListadoVerificacion(bo);
                                #endregion
                                break;
                            case ETipoUnidad.LV_VIBRO_COMPACTADOR:
                                #region Datos VibroCompactador
                                ListadoVerificacionVibroCompactadorBO boVibroCompactador = new ListadoVerificacionVibroCompactadorBO();
                                boVibroCompactador.LineaContratoPSLID = this.vista.LineaContratoID.Value;
                                boVibroCompactador = (ListadoVerificacionVibroCompactadorBO)this.presentadorVibroCompactadorRecepcion.InterfazUsuarioADato();
                                if (this.vista.TipoListado.HasValue)
                                    boVibroCompactador.Tipo = (ETipoListadoVerificacion)this.vista.TipoListado.Value;
                                if (this.vista.FechaListado.HasValue)
                                    boVibroCompactador.Fecha = this.vista.FechaListado.Value;
                                if (this.vista.FechaListado.HasValue && this.vista.HoraListado.HasValue)
                                    boVibroCompactador.Fecha = boVibroCompactador.Fecha.Value.Add(this.vista.HoraListado.Value);
                                if (this.vista.Horometro.HasValue)
                                    boVibroCompactador.Horometro = this.vista.Horometro.Value;
                                if (this.vista.Combustible.HasValue)
                                    boVibroCompactador.Combustible = this.vista.Combustible.Value;

                                if (!string.IsNullOrEmpty(this.vista.ObservacionesRecepcion))
                                    boVibroCompactador.Observaciones = this.vista.ObservacionesRecepcion;

                                boVibroCompactador.Auditoria = new AuditoriaBO
                                {
                                    FC = DateTime.Now,
                                    FUA = DateTime.Now,
                                    UC = this.vista.UsuarioID,
                                    UUA = this.vista.UsuarioID
                                };

                                //agregamos los archivos adjuntos al listado
                                boVibroCompactador.Adjuntos = this.presentadorDocumentosRecepcion.Vista.NuevosArchivos;

                                if (ReferenceEquals(linea.ListadosVerificacion, null))
                                    linea.ListadosVerificacion = new List<AVerificacionLineaPSLBO>();

                                if (!boVibroCompactador.Tipo.HasValue)
                                    boVibroCompactador.Tipo = ETipoListadoVerificacion.RECEPCION;

                                //Agregamos la unidad a la linea
                                linea.Equipo = this.InterfazUsuarioADatoUnidad();

                                linea.AgregarListadoVerificacion(boVibroCompactador);
                                #endregion
                                break;
                            case ETipoUnidad.LV_PISTOLA_NEUMATICA:
                                #region Datos PistolaNeumatica
                                ListadoVerificacionPistolaNeumaticaBO boPistolaNeumatica = new ListadoVerificacionPistolaNeumaticaBO();
                                boPistolaNeumatica.LineaContratoPSLID = this.vista.LineaContratoID.Value;
                                boPistolaNeumatica = (ListadoVerificacionPistolaNeumaticaBO)this.presentadorPistolaNeumaticaRecepcion.InterfazUsuarioADato();
                                if (this.vista.TipoListado.HasValue)
                                    boPistolaNeumatica.Tipo = (ETipoListadoVerificacion)this.vista.TipoListado.Value;
                                if (this.vista.FechaListado.HasValue)
                                    boPistolaNeumatica.Fecha = this.vista.FechaListado.Value;
                                if (this.vista.FechaListado.HasValue && this.vista.HoraListado.HasValue)
                                    boPistolaNeumatica.Fecha = boPistolaNeumatica.Fecha.Value.Add(this.vista.HoraListado.Value);
                                if (this.vista.Horometro.HasValue)
                                    boPistolaNeumatica.Horometro = this.vista.Horometro.Value;
                                if (this.vista.Combustible.HasValue)
                                    boPistolaNeumatica.Combustible = this.vista.Combustible.Value;

                                if (!string.IsNullOrEmpty(this.vista.ObservacionesRecepcion))
                                    boPistolaNeumatica.Observaciones = this.vista.ObservacionesRecepcion;

                                boPistolaNeumatica.Auditoria = new AuditoriaBO
                                {
                                    FC = DateTime.Now,
                                    FUA = DateTime.Now,
                                    UC = this.vista.UsuarioID,
                                    UUA = this.vista.UsuarioID
                                };

                                //agregamos los archivos adjuntos al listado
                                boPistolaNeumatica.Adjuntos = this.presentadorDocumentosRecepcion.Vista.NuevosArchivos;

                                if (ReferenceEquals(linea.ListadosVerificacion, null))
                                    linea.ListadosVerificacion = new List<AVerificacionLineaPSLBO>();

                                if (!boPistolaNeumatica.Tipo.HasValue)
                                    boPistolaNeumatica.Tipo = ETipoListadoVerificacion.RECEPCION;

                                //Agregamos la unidad a la linea
                                linea.Equipo = this.InterfazUsuarioADatoUnidad();

                                linea.AgregarListadoVerificacion(boPistolaNeumatica);
                                #endregion
                                break;
                            case ETipoUnidad.LV_MARTILLO_HIDRAULICO:
                                #region Datos MartilloHidraulico
                                ListadoVerificacionMartilloHidraulicoBO boMartilloHidraulico = new ListadoVerificacionMartilloHidraulicoBO();
                                boMartilloHidraulico.LineaContratoPSLID = this.vista.LineaContratoID.Value;
                                boMartilloHidraulico = (ListadoVerificacionMartilloHidraulicoBO)this.presentadorMartilloHidraulicoRecepcion.InterfazUsuarioADato();
                                if (this.vista.TipoListado.HasValue)
                                    boMartilloHidraulico.Tipo = (ETipoListadoVerificacion)this.vista.TipoListado.Value;
                                if (this.vista.FechaListado.HasValue)
                                    boMartilloHidraulico.Fecha = this.vista.FechaListado.Value;
                                if (this.vista.FechaListado.HasValue && this.vista.HoraListado.HasValue)
                                    boMartilloHidraulico.Fecha = boMartilloHidraulico.Fecha.Value.Add(this.vista.HoraListado.Value);
                                if (this.vista.Horometro.HasValue)
                                    boMartilloHidraulico.Horometro = this.vista.Horometro.Value;
                                if (this.vista.Combustible.HasValue)
                                    boMartilloHidraulico.Combustible = this.vista.Combustible.Value;

                                if (!string.IsNullOrEmpty(this.vista.ObservacionesRecepcion))
                                    boMartilloHidraulico.Observaciones = this.vista.ObservacionesRecepcion;

                                boMartilloHidraulico.Auditoria = new AuditoriaBO
                                {
                                    FC = DateTime.Now,
                                    FUA = DateTime.Now,
                                    UC = this.vista.UsuarioID,
                                    UUA = this.vista.UsuarioID
                                };

                                //agregamos los archivos adjuntos al listado
                                boMartilloHidraulico.Adjuntos = this.presentadorDocumentosRecepcion.Vista.NuevosArchivos;

                                if (ReferenceEquals(linea.ListadosVerificacion, null))
                                    linea.ListadosVerificacion = new List<AVerificacionLineaPSLBO>();

                                if (!boMartilloHidraulico.Tipo.HasValue)
                                    boMartilloHidraulico.Tipo = ETipoListadoVerificacion.RECEPCION;

                                //Agregamos la unidad a la línea
                                linea.Equipo = this.InterfazUsuarioADatoUnidad();

                                linea.AgregarListadoVerificacion(boMartilloHidraulico);
                                #endregion
                                break;
                            case ETipoUnidad.LV_TORRES_LUZ:
                                #region Datos TorresLuz
                                ListadoVerificacionTorresLuzBO boTorresLuz = new ListadoVerificacionTorresLuzBO();
                                boTorresLuz.LineaContratoPSLID = this.vista.LineaContratoID.Value;
                                boTorresLuz = (ListadoVerificacionTorresLuzBO)this.presentadorTorresLuzRecepcion.InterfazUsuarioADato();
                                if (this.vista.TipoListado.HasValue)
                                    boTorresLuz.Tipo = (ETipoListadoVerificacion)this.vista.TipoListado.Value;
                                if (this.vista.FechaListado.HasValue)
                                    boTorresLuz.Fecha = this.vista.FechaListado.Value;
                                if (this.vista.FechaListado.HasValue && this.vista.HoraListado.HasValue)
                                    boTorresLuz.Fecha = boTorresLuz.Fecha.Value.Add(this.vista.HoraListado.Value);
                                if (this.vista.Horometro.HasValue)
                                    boTorresLuz.Horometro = this.vista.Horometro.Value;
                                if (this.vista.Combustible.HasValue)
                                    boTorresLuz.Combustible = this.vista.Combustible.Value;

                                if (!string.IsNullOrEmpty(this.vista.ObservacionesRecepcion))
                                    boTorresLuz.Observaciones = this.vista.ObservacionesRecepcion;

                                boTorresLuz.Auditoria = new AuditoriaBO
                                {
                                    FC = DateTime.Now,
                                    FUA = DateTime.Now,
                                    UC = this.vista.UsuarioID,
                                    UUA = this.vista.UsuarioID
                                };

                                //agregamos los archivos adjuntos al listado
                                boTorresLuz.Adjuntos = this.presentadorDocumentosRecepcion.Vista.NuevosArchivos;

                                if (ReferenceEquals(linea.ListadosVerificacion, null))
                                    linea.ListadosVerificacion = new List<AVerificacionLineaPSLBO>();

                                if (!boTorresLuz.Tipo.HasValue)
                                    boTorresLuz.Tipo = ETipoListadoVerificacion.RECEPCION;

                                //Agregamos la unidad a la línea
                                linea.Equipo = this.InterfazUsuarioADatoUnidad();

                                linea.AgregarListadoVerificacion(boTorresLuz);
                                #endregion
                                break;
                            case ETipoUnidad.LV_MOTONIVELADORA:
                                #region Datos Motoniveladora
                                ListadoVerificacionMotoNiveladoraBO boMotoniveladora = new ListadoVerificacionMotoNiveladoraBO();
                                boMotoniveladora.LineaContratoPSLID = this.vista.LineaContratoID.Value;
                                boMotoniveladora = (ListadoVerificacionMotoNiveladoraBO)this.presentadorMotoNiveladoraR.InterfazUsuarioADato();
                                if (this.vista.TipoListado.HasValue)
                                    boMotoniveladora.Tipo = (ETipoListadoVerificacion)this.vista.TipoListado.Value;
                                if (this.vista.FechaListado.HasValue)
                                    boMotoniveladora.Fecha = this.vista.FechaListado.Value;
                                if (this.vista.FechaListado.HasValue && this.vista.HoraListado.HasValue)
                                    boMotoniveladora.Fecha = boMotoniveladora.Fecha.Value.Add(this.vista.HoraListado.Value);
                                if (this.vista.Horometro.HasValue)
                                    boMotoniveladora.Horometro = this.vista.Horometro.Value;
                                if (this.vista.Combustible.HasValue)
                                    boMotoniveladora.Combustible = this.vista.Combustible.Value;

                                if (!string.IsNullOrEmpty(this.vista.ObservacionesRecepcion))
                                    boMotoniveladora.Observaciones = this.vista.ObservacionesRecepcion;
                                boMotoniveladora.Auditoria = new AuditoriaBO
                                {
                                    FC = DateTime.Now,
                                    FUA = DateTime.Now,
                                    UC = this.vista.UsuarioID,
                                    UUA = this.vista.UsuarioID
                                };

                                //agregamos los archivos adjuntos al listado
                                boMotoniveladora.Adjuntos = this.presentadorDocumentosRecepcion.Vista.NuevosArchivos;

                                if (ReferenceEquals(linea.ListadosVerificacion, null))
                                    linea.ListadosVerificacion = new List<AVerificacionLineaPSLBO>();

                                if (!boMotoniveladora.Tipo.HasValue)
                                    boMotoniveladora.Tipo = ETipoListadoVerificacion.RECEPCION;

                                //Agregamos la unidad a la línea
                                linea.Equipo = this.InterfazUsuarioADatoUnidad();

                                linea.AgregarListadoVerificacion(boMotoniveladora);
                                #endregion
                                break;
                            case ETipoUnidad.LV_MONTACARGA:
                                #region Datos MontaCarga
                                ListadoVerificacionMontaCargaBO boMontaCarga = new ListadoVerificacionMontaCargaBO();
                                boMontaCarga.LineaContratoPSLID = this.vista.LineaContratoID.Value;
                                boMontaCarga = (ListadoVerificacionMontaCargaBO)this.presentadorMontaCargaRecepcion.InterfazUsuarioADato();
                                if (this.vista.TipoListado.HasValue)
                                    boMontaCarga.Tipo = (ETipoListadoVerificacion)this.vista.TipoListado.Value;
                                if (this.vista.FechaListado.HasValue)
                                    boMontaCarga.Fecha = this.vista.FechaListado.Value;
                                if (this.vista.FechaListado.HasValue && this.vista.HoraListado.HasValue)
                                    boMontaCarga.Fecha = boMontaCarga.Fecha.Value.Add(this.vista.HoraListado.Value);
                                if (this.vista.Horometro.HasValue)
                                    boMontaCarga.Horometro = this.vista.Horometro.Value;
                                if (this.vista.Combustible.HasValue)
                                    boMontaCarga.Combustible = this.vista.Combustible.Value;

                                if (!string.IsNullOrEmpty(this.vista.ObservacionesRecepcion))
                                    boMontaCarga.Observaciones = this.vista.ObservacionesRecepcion;

                                boMontaCarga.Auditoria = new AuditoriaBO
                                {
                                    FC = DateTime.Now,
                                    FUA = DateTime.Now,
                                    UC = this.vista.UsuarioID,
                                    UUA = this.vista.UsuarioID
                                };

                                //agregamos los archivos adjuntos al listado
                                boMontaCarga.Adjuntos = this.presentadorDocumentosRecepcion.Vista.NuevosArchivos;

                                if (ReferenceEquals(linea.ListadosVerificacion, null))
                                    linea.ListadosVerificacion = new List<AVerificacionLineaPSLBO>();

                                if (!boMontaCarga.Tipo.HasValue)
                                    boMontaCarga.Tipo = ETipoListadoVerificacion.RECEPCION;

                                //Agregamos la unidad a la linea
                                linea.Equipo = this.InterfazUsuarioADatoUnidad();

                                linea.AgregarListadoVerificacion(boMontaCarga);
                                #endregion
                                break;
                            case ETipoUnidad.LV_PLATAFORMA_TIJERAS:
                                #region Datos PlataformaTijeras
                                ListadoVerificacionPlataformaTijerasBO boPlataforma = new ListadoVerificacionPlataformaTijerasBO();
                                boPlataforma.LineaContratoPSLID = this.vista.LineaContratoID.Value;
                                boPlataforma = (ListadoVerificacionPlataformaTijerasBO)this.presentadorPlataformaTijerasR.InterfazUsuarioADato();
                                if (this.vista.TipoListado.HasValue)
                                    boPlataforma.Tipo = (ETipoListadoVerificacion)this.vista.TipoListado.Value;
                                if (this.vista.FechaListado.HasValue)
                                    boPlataforma.Fecha = this.vista.FechaListado.Value;
                                if (this.vista.FechaListado.HasValue && this.vista.HoraListado.HasValue)
                                    boPlataforma.Fecha = boPlataforma.Fecha.Value.Add(this.vista.HoraListado.Value);
                                if (this.vista.Horometro.HasValue)
                                    boPlataforma.Horometro = this.vista.Horometro.Value;
                                if (this.vista.Combustible.HasValue)
                                    boPlataforma.Combustible = this.vista.Combustible.Value;

                                if (!string.IsNullOrEmpty(this.vista.ObservacionesRecepcion))
                                    boPlataforma.Observaciones = this.vista.ObservacionesRecepcion;

                                boPlataforma.Auditoria = new AuditoriaBO
                                {
                                    FC = DateTime.Now,
                                    FUA = DateTime.Now,
                                    UC = this.vista.UsuarioID,
                                    UUA = this.vista.UsuarioID
                                };

                                //agregamos los archivos adjuntos al listado
                                boPlataforma.Adjuntos = this.presentadorDocumentosRecepcion.Vista.NuevosArchivos;

                                if (ReferenceEquals(linea.ListadosVerificacion, null))
                                    linea.ListadosVerificacion = new List<AVerificacionLineaPSLBO>();

                                if (!boPlataforma.Tipo.HasValue)
                                    boPlataforma.Tipo = ETipoListadoVerificacion.RECEPCION;

                                //Agregamos la unidad a la linea
                                linea.Equipo = this.InterfazUsuarioADatoUnidad();

                                linea.AgregarListadoVerificacion(boPlataforma);
                                #endregion
                                break;
                            case ETipoUnidad.LV_MINICARGADOR:
                                #region Datos MiniCargador
                                ListadoVerificacionMiniCargadorBO boMiniCargador = new ListadoVerificacionMiniCargadorBO();
                                boMiniCargador.LineaContratoPSLID = this.vista.LineaContratoID.Value;
                                boMiniCargador = (ListadoVerificacionMiniCargadorBO)this.presentadorMiniCargadorRecepcion.InterfazUsuarioADato();
                                if (this.vista.TipoListado.HasValue)
                                    boMiniCargador.Tipo = (ETipoListadoVerificacion)this.vista.TipoListado.Value;
                                if (this.vista.FechaListado.HasValue)
                                    boMiniCargador.Fecha = this.vista.FechaListado.Value;
                                if (this.vista.FechaListado.HasValue && this.vista.HoraListado.HasValue)
                                    boMiniCargador.Fecha = boMiniCargador.Fecha.Value.Add(this.vista.HoraListado.Value);
                                if (this.vista.Horometro.HasValue)
                                    boMiniCargador.Horometro = this.vista.Horometro.Value;
                                if (this.vista.Combustible.HasValue)
                                    boMiniCargador.Combustible = this.vista.Combustible.Value;

                                if (!string.IsNullOrEmpty(this.vista.ObservacionesRecepcion))
                                    boMiniCargador.Observaciones = this.vista.ObservacionesRecepcion;

                                boMiniCargador.Auditoria = new AuditoriaBO
                                {
                                    FC = DateTime.Now,
                                    FUA = DateTime.Now,
                                    UC = this.vista.UsuarioID,
                                    UUA = this.vista.UsuarioID
                                };

                                //agregamos los archivos adjuntos al listado
                                boMiniCargador.Adjuntos = this.presentadorDocumentosRecepcion.Vista.NuevosArchivos;

                                if (ReferenceEquals(linea.ListadosVerificacion, null))
                                    linea.ListadosVerificacion = new List<AVerificacionLineaPSLBO>();

                                if (!boMiniCargador.Tipo.HasValue)
                                    boMiniCargador.Tipo = ETipoListadoVerificacion.RECEPCION;

                                //Agregamos la unidad a la linea
                                linea.Equipo = this.InterfazUsuarioADatoUnidad();

                                linea.AgregarListadoVerificacion(boMiniCargador);
                                #endregion
                                break;
                            case ETipoUnidad.LV_RETRO_EXCAVADORA:
                                #region Datos RetroExcavadora
                                ListadoVerificacionRetroExcavadoraBO boRetro = new ListadoVerificacionRetroExcavadoraBO();
                                boRetro.LineaContratoPSLID = this.vista.LineaContratoID.Value;
                                boRetro = (ListadoVerificacionRetroExcavadoraBO)this.presentadorRetroExcavadoraRecepcion.InterfazUsuarioADato();
                                if (this.vista.TipoListado.HasValue)
                                    boRetro.Tipo = (ETipoListadoVerificacion)this.vista.TipoListado.Value;
                                if (this.vista.FechaListado.HasValue)
                                    boRetro.Fecha = this.vista.FechaListado.Value;
                                if (this.vista.FechaListado.HasValue && this.vista.HoraListado.HasValue)
                                    boRetro.Fecha = boRetro.Fecha.Value.Add(this.vista.HoraListado.Value);
                                if (this.vista.Horometro.HasValue)
                                    boRetro.Horometro = this.vista.Horometro.Value;
                                if (this.vista.Combustible.HasValue)
                                    boRetro.Combustible = this.vista.Combustible.Value;

                                if (!string.IsNullOrEmpty(this.vista.ObservacionesRecepcion))
                                    boRetro.Observaciones = this.vista.ObservacionesRecepcion;

                                boRetro.Auditoria = new AuditoriaBO
                                {
                                    FC = DateTime.Now,
                                    FUA = DateTime.Now,
                                    UC = this.vista.UsuarioID,
                                    UUA = this.vista.UsuarioID
                                };

                                //agregamos los archivos adjuntos al listado
                                boRetro.Adjuntos = this.presentadorDocumentosRecepcion.Vista.NuevosArchivos;

                                if (ReferenceEquals(linea.ListadosVerificacion, null))
                                    linea.ListadosVerificacion = new List<AVerificacionLineaPSLBO>();

                                if (!boRetro.Tipo.HasValue)
                                    boRetro.Tipo = ETipoListadoVerificacion.RECEPCION;

                                //Agregamos la unidad a la linea
                                linea.Equipo = this.InterfazUsuarioADatoUnidad();

                                linea.AgregarListadoVerificacion(boRetro);
                                #endregion
                                break;
                            case ETipoUnidad.LV_COMPRESOR:
                                #region Datos Compresor
                                ListadoVerificacionCompresorPortatilBO boCompresor = new ListadoVerificacionCompresorPortatilBO();
                                boCompresor.LineaContratoPSLID = this.vista.LineaContratoID.Value;
                                boCompresor = (ListadoVerificacionCompresorPortatilBO)this.presentadorCompresoresPortatilesR.InterfazUsuarioADato();
                                if (this.vista.TipoListado.HasValue)
                                    boCompresor.Tipo = (ETipoListadoVerificacion)this.vista.TipoListado.Value;
                                if (this.vista.FechaListado.HasValue)
                                    boCompresor.Fecha = this.vista.FechaListado.Value;
                                if (this.vista.FechaListado.HasValue && this.vista.HoraListado.HasValue)
                                    boCompresor.Fecha = boCompresor.Fecha.Value.Add(this.vista.HoraListado.Value);
                                if (this.vista.Horometro.HasValue)
                                    boCompresor.Horometro = this.vista.Horometro.Value;
                                if (this.vista.Combustible.HasValue)
                                    boCompresor.Combustible = this.vista.Combustible.Value;

                                if (!string.IsNullOrEmpty(this.vista.ObservacionesRecepcion))
                                    boCompresor.Observaciones = this.vista.ObservacionesRecepcion;

                                boCompresor.Auditoria = new AuditoriaBO
                                {
                                    FC = DateTime.Now,
                                    FUA = DateTime.Now,
                                    UC = this.vista.UsuarioID,
                                    UUA = this.vista.UsuarioID
                                };

                                //agregamos los archivos adjuntos al listado
                                boCompresor.Adjuntos = this.presentadorDocumentosRecepcion.Vista.NuevosArchivos;

                                if (ReferenceEquals(linea.ListadosVerificacion, null))
                                    linea.ListadosVerificacion = new List<AVerificacionLineaPSLBO>();

                                if (!boCompresor.Tipo.HasValue)
                                    boCompresor.Tipo = ETipoListadoVerificacion.RECEPCION;

                                //Agregamos la unidad a la linea
                                linea.Equipo = this.InterfazUsuarioADatoUnidad();

                                linea.AgregarListadoVerificacion(boCompresor);
                                #endregion
                                break;

                        }
                    }
                    if ((EAreaConstruccion)this.vista.Area == EAreaConstruccion.RE) {
                        #region Datos SubArrendados
                        ListadoVerificacionSubArrendadoBO boSA = new ListadoVerificacionSubArrendadoBO();
                        boSA.LineaContratoPSLID = this.vista.LineaContratoID.Value;
                        boSA = (ListadoVerificacionSubArrendadoBO)this.presentadorSubArrendadosR.InterfazUsuarioADato();
                        if (this.vista.TipoListado.HasValue)
                            boSA.Tipo = (ETipoListadoVerificacion)this.vista.TipoListado.Value;
                        if (this.vista.FechaListado.HasValue)
                            boSA.Fecha = this.vista.FechaListado.Value;
                        if (this.vista.FechaListado.HasValue && this.vista.HoraListado.HasValue)
                            boSA.Fecha = boSA.Fecha.Value.Add(this.vista.HoraListado.Value);
                        if (this.vista.Horometro.HasValue)
                            boSA.Horometro = this.vista.Horometro.Value;
                        if (this.vista.Combustible.HasValue)
                            boSA.Combustible = this.vista.Combustible.Value;

                        if (!string.IsNullOrEmpty(this.vista.ObservacionesRecepcion))
                            boSA.Observaciones = this.vista.ObservacionesRecepcion;

                        boSA.Auditoria = new AuditoriaBO
                        {
                            FC = DateTime.Now,
                            FUA = DateTime.Now,
                            UC = this.vista.UsuarioID,
                            UUA = this.vista.UsuarioID
                        };

                        //agregamos los archivos adjuntos al listado
                        boSA.Adjuntos = this.presentadorDocumentosRecepcion.Vista.NuevosArchivos;

                        if (ReferenceEquals(linea.ListadosVerificacion, null))
                            linea.ListadosVerificacion = new List<AVerificacionLineaPSLBO>();

                        if (!boSA.Tipo.HasValue)
                            boSA.Tipo = ETipoListadoVerificacion.RECEPCION;

                        //Agregamos la unidad a la linea
                        linea.Equipo = this.InterfazUsuarioADatoUnidad();

                        linea.AgregarListadoVerificacion(boSA);
                        #endregion
                    }
                    break;
                case (int)ETipoEmpresa.Generacion:
                case (int)ETipoEmpresa.Equinova:
                    #region Datos EntregaRecepcion
                    ListadoVerificacionEntregaRecepcionBO boER = new ListadoVerificacionEntregaRecepcionBO();
                    boER.LineaContratoPSLID = this.vista.LineaContratoID.Value;
                    boER = (ListadoVerificacionEntregaRecepcionBO)this.presentadorEntregaRecepcionR.InterfazUsuarioADato();
                    if (this.vista.TipoListado.HasValue)
                        boER.Tipo = (ETipoListadoVerificacion)this.vista.TipoListado.Value;
                    if (this.vista.FechaListado.HasValue)
                        boER.Fecha = this.vista.FechaListado.Value;
                    if (this.vista.FechaListado.HasValue && this.vista.HoraListado.HasValue)
                        boER.Fecha = boER.Fecha.Value.Add(this.vista.HoraListado.Value);
                    if (this.vista.Horometro.HasValue)
                        boER.Horometro = this.vista.Horometro.Value;
                    if (this.vista.Combustible.HasValue)
                        boER.Combustible = this.vista.Combustible.Value;
                    if (!string.IsNullOrEmpty(this.vista.ObservacionesRecepcion))
                        boER.Observaciones = this.vista.ObservacionesRecepcion;

                    boER.Auditoria = new AuditoriaBO
                    {
                        FC = DateTime.Now,
                        FUA = DateTime.Now,
                        UC = this.vista.UsuarioID,
                        UUA = this.vista.UsuarioID
                    };
                    //agregamos los archivos adjuntos al listado
                    boER.Adjuntos = this.presentadorDocumentosRecepcion.Vista.NuevosArchivos;

                    if (ReferenceEquals(linea.ListadosVerificacion, null))
                        linea.ListadosVerificacion = new List<AVerificacionLineaPSLBO>();

                    if (!boER.Tipo.HasValue)
                        boER.Tipo = ETipoListadoVerificacion.RECEPCION;
                    //Agregamos la unidad a la linea
                    linea.Equipo = this.InterfazUsuarioADatoUnidad();
                    linea.AgregarListadoVerificacion(boER);
                    #endregion
                    break;
            }
            if (ReferenceEquals(obj.LineasContrato, null))
                obj.LineasContrato = new List<ILineaContrato>();

            obj.AgregarLineaContrato((ILineaContrato)linea);
            return obj;
        }

        /// <summary>
        /// Carga en la vista los tipos de archivo imagen
        /// </summary>
        public void CargarTipoArchivosImagen() {
            var tipoBR = new TipoArchivoBR();
            var tipo = new TipoArchivoBO { EsImagen = true };
            vista.TiposArchivoImagen = tipoBR.Consultar(this.dctx, tipo);
        }

        /// <summary>
        /// Elimina los posibles valores almacenados en los controles
        /// </summary>
        private void LimpiarCampos() {
            this.vista.Combustible = null;
            this.vista.ContratoID = null;
            this.vista.EquipoID = null;
            this.vista.EstatusContratoID = null;
            this.vista.FechaContrato = null;
            this.vista.FechaListado = null;
            this.vista.NombreCliente = string.Empty;
            this.vista.NombreUsuarioRecibe = string.Empty;
            this.vista.NombreUsuarioEntrega = string.Empty;
            this.vista.PlacasEstatales = string.Empty;
            this.vista.NumeroContrato = string.Empty;
            this.vista.NumeroEconomico = string.Empty;
            this.vista.NumeroSerie = string.Empty;
            this.vista.HoraListado = null;
            this.vista.HoraContrato = null;
            this.vista.Horometro = null;
            this.vista.LineaContratoID = null;
            this.vista.CheckListEntregaID = null;
            this.vista.ObservacionesRecepcion = string.Empty;
        }

        /// <summary>
        /// Prepara la aplicación para el registro de un nuevo Check List
        /// </summary>
        public void PrepararNuevo() {
            this.vista.LimpiarSesion();
            this.LimpiarCampos();
            this.vista.PrepararNuevo();
            this.presentadorDocumentosEntrega.ModoEditable(false);
            this.presentadorDocumentosRecepcion.ModoEditable(true);
            this.presentadorEquiposAliados.Inicializar();
            this.EstablecerInformacionInicial();
            this.vista.EstablecerPuntosVerificacionCheckList();
            this.vista.LimpiarPaqueteNavegacion();
            this.vista.PermitirRegresar(false);
            this.vista.PermitirContinuar(true);
            //this.vista.PrepararCancelacion(false);
            this.IrAPagina(0);

            this.EstablecerSeguridad();
            this.EstablecerAcciones();
        }

        public void EstablecerInformacionInicial() {
            try {
                //Obtenemos el contrato al cual le haremos el check list
                var val = (int?)this.vista.ObtenerPaqueteContrato();
                var linea = (int?)this.vista.ObtenerPaqueteLineaContrato();
                if (!val.HasValue || !linea.HasValue)
                    throw new Exception("No es posible recuperar la información del contrato necesaria para el registro del check list. Intente de nuevo por favor.");

                this.vista.ContratoID = val;
                this.vista.LineaContratoID = linea;
                //Se establecen los tipos de archivos permitidos para adjuntar al contrato                
                var o = new object();
                var o1 = new object();

                List<TipoArchivoBO> lstTiposArchivoE = new TipoArchivoBR().Consultar(this.dctx, new TipoArchivoBO { EsImagen = true });
                this.presentadorDocumentosEntrega.EstablecerTiposArchivo(lstTiposArchivoE);
                this.presentadorDocumentosEntrega.Vista.Identificador = this.vista.CheckListEntregaID.HasValue ? this.vista.CheckListEntregaID.Value.ToString() : o.GetHashCode().ToString();
                this.presentadorDocumentosRecepcion.Vista.Identificador = o1.GetHashCode().ToString();
                List<TipoArchivoBO> lstTiposArchivo = new TipoArchivoBR().Consultar(this.dctx, new TipoArchivoBO());
                this.presentadorDocumentosRecepcion.EstablecerTiposArchivo(lstTiposArchivo);

                #region Contrato
                ContratoPSLBR contratoPSL = new ContratoPSLBR();
                List<ContratoPSLBO> contratos = contratoPSL.ConsultarParcial(this.dctx, new ContratoPSLBO { ContratoID = this.vista.ContratoID }, true, true);

                if (ReferenceEquals(contratos, null))
                    throw new Exception("No es posible recuperar la información del contrato necesaria para el registro del check list. Intente de nuevo por favor.");
                if (contratos.Count <= 0)
                    throw new Exception("No es posible recuperar la información del contrato necesaria para el registro del check list. Intente de nuevo por favor.");

                //Desplegamos la información general del contrato
                this.DesplegarInformacionGeneral(contratos[0]);
                #endregion

                #region Usuario
                CatalogoBaseBO ubase = new UsuarioBO { Activo = true, Id = this.vista.UsuarioID };
                List<UsuarioBO> usuarios = FacadeBR.ConsultarUsuario(this.dctx, ubase);

                if (!ReferenceEquals(usuarios, null)) {
                    if (usuarios.Count > 0) {
                        this.vista.NombreUsuarioRecibe = !string.IsNullOrEmpty(usuarios[0].Nombre) && !string.IsNullOrWhiteSpace(usuarios[0].Nombre)
                                                              ? usuarios[0].Nombre.Trim().ToUpper()
                                                              : string.Empty;
                    }
                }
                #endregion

                #region Check List
                this.vista.FechaListado = DateTime.Now;
                this.vista.HoraListado = DateTime.Now.AddMinutes(5).TimeOfDay;
                this.vista.TipoListado = (int)ETipoListadoVerificacion.RECEPCION;
                this.EstablecerToleranciaKMS();
                #endregion
            } catch (Exception ex) {
                throw new Exception(nombreClase + ".EstablecerInformacionInicial: " + ex.Message);
            }

        }

        /// <summary>
        /// Consulta en el listado de acciones configuradas una acción especifica
        /// </summary>
        /// <param name="acciones">Acciones configuradas</param>
        /// <param name="nombreAccion">Accion que se desea validar</param>
        /// <returns>Verdadero si existe,  falso si no existe</returns>
        private bool ExisteAccion(List<CatalogoBaseBO> acciones, string nombreAccion) {
            if (acciones != null && acciones.Count > 0 && acciones.Exists(p => p != null && p.Nombre != null && p.Nombre.Trim().ToUpper() == nombreAccion.Trim().ToUpper()))
                return true;

            return false;
        }

        /// <summary>
        /// Valida si el usuario tiene permiso para registrar el check list
        /// </summary>
        private void EstablecerSeguridad() {
            try {
                //se valida que los datos del usuario y la unidad operativa no sean nulos
                if (this.vista.UsuarioID == null) throw new Exception("El identificador del usuario no debe ser nulo. ");
                if (this.vista.UnidadOperativaID == null) throw new Exception("El identificador de la Unidad Operativa no debe ser nulo. ");

                //Se crea el objeto de seguridad
                UsuarioBO usr = new UsuarioBO { Id = this.vista.UsuarioID };
                AdscripcionBO sdscripcion = new AdscripcionBO { UnidadOperativa = new UnidadOperativaBO { Id = this.vista.UnidadOperativaID } };
                SeguridadBO seguridadBO = new SeguridadBO(Guid.Empty, usr, sdscripcion);

                //Se obtienen las acciones a las cuales el usuario tiene permiso en este proceso
                List<CatalogoBaseBO> lst = FacadeBR.ConsultarAccion(this.dctx, seguridadBO);

                //consulta lista de acciones permitidas
                this.vista.ListaAcciones = lst;

                //Se valida si el usuario tiene permiso para registrar el check list
                if (!this.ExisteAccion(lst, "REGISTRARRECEPCION"))
                    this.vista.RedirigirSinPermisoAcceso();
            } catch (Exception ex) {
                throw new Exception(nombreClase + ".EstablecerSeguridad:" + Environment.NewLine + ex.Message);
            }
        }

        /// <summary>
        /// Establece la tolerancia de kilometraje
        /// </summary>
        private void EstablecerToleranciaKMS() {
            try {
                if (this.vista.UnidadOperativaID == null)
                    throw new Exception("El indentificador de la unidad operativa no debe ser nulo");
                AppSettingsReader n = new AppSettingsReader();
                ConfiguracionUnidadOperativaBO configUO = null;
                int moduloID = (int)this.vista.ModuloID;

                ModuloBO modulo = new ModuloBO() { ModuloID = moduloID };
                this.moduloBR = new ModuloBR();
                List<ModuloBO> modulos = moduloBR.ConsultarCompleto(dctx, modulo);
                if (modulos.Count > 0) {
                    modulo = modulos[0];
                    configUO = modulo.ObtenerConfiguracionUO(new UnidadOperativaBO { Id = this.vista.UnidadOperativaID });
                }
                if (configUO != null)
                    this.vista.KilometrajeDiario = configUO.KilometrajeDiarioAproximado;
            } catch (Exception ex) {
                throw new Exception(nombreClase + ".EstablecerToleranciaKMS: Error al consultar la tolerancia de kilometraje. " + ex.Message);
            }
        }

        /// <summary>
        /// Despliega en la vista las unidad seleccionada en el contrato
        /// </summary>
        /// <param name="unidadBO">Unidad que será desplegada</param>
        private void DesplegarInformacionUnidad(UnidadBO unidadBO) {
            if (ReferenceEquals(unidadBO, null))
                throw new Exception("No es posible recuperar la información de la unidad necesaria para el registro del check list. Intente de nuevo por favor.");

            FlotaBOF bof = new FlotaBOF();
            bof.Unidad = unidadBO;

            SeguimientoFlotaBR flotaBR = new SeguimientoFlotaBR();
            FlotaBO flota = new FlotaBO();
            switch ((ETipoEmpresa)this.vista.UnidadOperativaID) {
                case ETipoEmpresa.Construccion:
                    flota = flotaBR.ConsultarFlotaPSL(dctx, bof);
                    break;
                case ETipoEmpresa.Generacion:
                    flota = flotaBR.ConsultarFlotaPSL(dctx, bof);
                    break;
                case ETipoEmpresa.Equinova:
                    flota = flotaBR.ConsultarFlotaPSL(dctx, bof);
                    break;
                default:
                    break;
            }
            this.vista.Area = (EArea)unidadBO.Area;

            if (ReferenceEquals(flota, null))
                throw new Exception("No es posible recuperar la información de la unidad necesaria para el registro del check list. Intente de nuevo por favor.");

            if (ReferenceEquals(flota.ElementosFlota, null))
                throw new Exception("No es posible recuperar la información de la unidad necesaria para el registro del check list. Intente de nuevo por favor.");

            if (flota.ElementosFlota.Count <= 0)
                throw new Exception("No es posible recuperar la información de la unidad necesaria para el registro del check list. Intente de nuevo por favor.");

            ElementoFlotaBO elemento = flota.ElementosFlota[0];

            #region Unidad
            if (!ReferenceEquals(elemento.Unidad, null)) {
                this.vista.UnidadID = elemento.Unidad.UnidadID.HasValue ? elemento.Unidad.UnidadID : null;
                this.vista.EquipoID = elemento.Unidad.EquipoID.HasValue ? elemento.Unidad.EquipoID : null;
                this.vista.NumeroEconomico = !string.IsNullOrEmpty(elemento.Unidad.NumeroEconomico) && !string.IsNullOrWhiteSpace(elemento.Unidad.NumeroEconomico)
                                                 ? elemento.Unidad.NumeroEconomico.Trim().ToUpper()
                                                 : string.Empty;

                this.vista.NumeroSerie = !string.IsNullOrEmpty(elemento.Unidad.NumeroSerie) && !string.IsNullOrWhiteSpace(elemento.Unidad.NumeroSerie)
                                             ? elemento.Unidad.NumeroSerie.Trim().ToUpper()
                                             : string.Empty;
                #region Placas
                TramiteBO estatal = elemento.ObtenerTramitePorTipo(ETipoTramite.PLACA_ESTATAL);

                if (!ReferenceEquals(estatal, null)) {
                    this.vista.PlacasEstatales = !string.IsNullOrEmpty(estatal.Resultado) && !string.IsNullOrWhiteSpace(estatal.Resultado)
                                                ? estatal.Resultado.Trim().ToUpper()
                                                : string.Empty;
                }
                #endregion

                #region Capacidad Tanque
                this.vista.CapacidadTanque = unidadBO != null ? (unidadBO.CombustibleConsumidoTotal.HasValue ? unidadBO.CombustibleConsumidoTotal : null) : null;
                #endregion
            }

            #region EquiposAliados
            this.presentadorEquiposAliados.DatoAInterfazUsuario(elemento);
            this.presentadorEquiposAliados.CargarEquiposAliados();
            #endregion

            #endregion
        }

        private void DesplegarInformacionListadoEntrega(AVerificacionLineaPSLBO listadoVerificacion) {

            if (ReferenceEquals(listadoVerificacion, null))
                throw new Exception("No es posible recuperar la información de la entrega de la unidad necesaria para el registro del check list. Intente de nuevo por favor.");

            #region Información general
            this.vista.CheckListEntregaID = listadoVerificacion.VerificacionLineaPSLID.HasValue ? listadoVerificacion.VerificacionLineaPSLID : null;
            if (this.vista.LineaContratoID == null)
                this.vista.LineaContratoID = listadoVerificacion.LineaContratoPSLID;
            this.vista.FechaListadoEntrega = listadoVerificacion.Fecha.HasValue ? (DateTime?)listadoVerificacion.Fecha.Value.Date : null;
            this.vista.HoraListadoEntrega = listadoVerificacion.Fecha.HasValue ? (TimeSpan?)listadoVerificacion.Fecha.Value.TimeOfDay : null;
            this.vista.HorometroEntrega = listadoVerificacion.Horometro.HasValue ? listadoVerificacion.Horometro : null;
            this.vista.CombustibleEntrega = listadoVerificacion.Combustible.HasValue ? (int?)listadoVerificacion.Combustible : null;
            this.vista.ObservacionesEntrega = !string.IsNullOrEmpty(listadoVerificacion.Observaciones) ? listadoVerificacion.Observaciones : string.Empty;
            #endregion

            #region Usuario entrega
            if (!ReferenceEquals(listadoVerificacion.Auditoria.UC, null)) {
                if (listadoVerificacion.Auditoria.UC.HasValue) {
                    CatalogoBaseBO ubase = new UsuarioBO { Activo = true, Id = listadoVerificacion.Auditoria.UC.Value };
                    List<UsuarioBO> usuarios = FacadeBR.ConsultarUsuario(this.dctx, ubase);
                    if (!ReferenceEquals(usuarios, null)) {
                        if (usuarios.Count > 0) {
                            this.vista.NombreUsuarioEntrega = !string.IsNullOrEmpty(usuarios[0].Nombre) && !string.IsNullOrWhiteSpace(usuarios[0].Nombre)
                                                                  ? usuarios[0].Nombre.Trim().ToUpper()
                                                                  : string.Empty;
                        }
                    }
                }
            }
            #endregion

            #region Documentos Check List
            if (listadoVerificacion.Adjuntos != null)
                this.DesplegarDocumentosCheckList(listadoVerificacion.Adjuntos);
            #endregion

            switch (this.vista.UnidadOperativaID) {
                case (int)ETipoEmpresa.Construccion:
                    if ((EAreaConstruccion)this.vista.Area == EAreaConstruccion.RO || (EAreaConstruccion)this.vista.Area == EAreaConstruccion.ROC) {

                        switch (this.vista.TipoListadoVerificacionPSL) {
                            case (int)ETipoUnidad.LV_EXCAVADORA:
                                this.presentadorExcavadoraEntrega.DatoToInterfazUsuario((ListadoVerificacionExcavadoraBO)listadoVerificacion);
                                break;
                            case (int)ETipoUnidad.LV_RETRO_EXCAVADORA:
                                this.presentadorRetroExcavadoraEntrega.DatoToInterfazUsuario((ListadoVerificacionRetroExcavadoraBO)listadoVerificacion);
                                break;
                            case (int)ETipoUnidad.LV_MONTACARGA:
                                this.PresentadorMontaCargaEntrega.DatoToInterfazUsuario((ListadoVerificacionMontaCargaBO)listadoVerificacion);
                                break;

                            case (int)ETipoUnidad.LV_PISTOLA_NEUMATICA:
                                this.presentadorPistolaNeumaticaEntrega.DatoToInterfazUsuario((ListadoVerificacionPistolaNeumaticaBO)listadoVerificacion);
                                break;

                            case (int)ETipoUnidad.LV_COMPRESOR:
                                this.presentadorCompresoresPortatilesE.DatoToInterfazUsuario((ListadoVerificacionCompresorPortatilBO)listadoVerificacion);
                                break;

                            case (int)ETipoUnidad.LV_MINICARGADOR:
                                this.presentadorMiniCargadorEntrega.DatoToInterfazUsuario((ListadoVerificacionMiniCargadorBO)listadoVerificacion);
                                break;

                            case (int)ETipoUnidad.LV_VIBRO_COMPACTADOR:
                                this.presentadorVibroCompactadorEntrega.DatoToInterfazUsuario((ListadoVerificacionVibroCompactadorBO)listadoVerificacion);
                                break;

                            case (int)ETipoUnidad.LV_MARTILLO_HIDRAULICO:
                                this.presentadorMartilloHidraulicoEntrega.DatoToInterfazUsuario((ListadoVerificacionMartilloHidraulicoBO)listadoVerificacion);
                                break;

                            case (int)ETipoUnidad.LV_TORRES_LUZ:
                                this.presentadorTorresLuzEntrega.DatoToInterfazUsuario((ListadoVerificacionTorresLuzBO)listadoVerificacion);
                                break;

                            case (int)ETipoUnidad.LV_PLATAFORMA_TIJERAS:
                                ListadoVerificacionPlataformaTijerasBO listadoPlataforma = (ListadoVerificacionPlataformaTijerasBO)listadoVerificacion;
                                #region Información general
                                this.vista.CheckListEntregaID = listadoPlataforma.VerificacionLineaPSLID.HasValue ? listadoPlataforma.VerificacionLineaPSLID : null;
                                this.vista.FechaListadoEntrega = listadoPlataforma.Fecha.HasValue ? (DateTime?)listadoPlataforma.Fecha.Value.Date : null;
                                this.vista.HoraListadoEntrega = listadoPlataforma.Fecha.HasValue ? (TimeSpan?)listadoPlataforma.Fecha.Value.TimeOfDay : null;

                                this.vista.HorometroEntrega = listadoPlataforma.Horometro.HasValue ? listadoPlataforma.Horometro : null;
                                this.vista.CombustibleEntrega = listadoPlataforma.Combustible.HasValue ? (int?)listadoPlataforma.Combustible : null;
                                #endregion

                                #region Cuestionario
                                this.presentadorPlataformaTijerasE.DatoToInterfazUsuario(listadoPlataforma);
                                this.vista.ObservacionesEntrega = !string.IsNullOrEmpty(listadoPlataforma.Observaciones) ? listadoPlataforma.Observaciones : string.Empty;
                                #endregion

                                #region Documentos Check List
                                if (listadoPlataforma.Adjuntos != null)
                                    this.DesplegarDocumentosCheckList(listadoPlataforma.Adjuntos);
                                #endregion
                                break;

                            case (int)ETipoUnidad.LV_MOTONIVELADORA:
                                ListadoVerificacionMotoNiveladoraBO listadoMotoNiveladora = (ListadoVerificacionMotoNiveladoraBO)listadoVerificacion;
                                #region Información general
                                this.vista.CheckListEntregaID = listadoMotoNiveladora.VerificacionLineaPSLID.HasValue ? listadoMotoNiveladora.VerificacionLineaPSLID : null;
                                this.vista.FechaListadoEntrega = listadoMotoNiveladora.Fecha.HasValue ? (DateTime?)listadoMotoNiveladora.Fecha.Value.Date : null;
                                this.vista.HoraListadoEntrega = listadoMotoNiveladora.Fecha.HasValue ? (TimeSpan?)listadoMotoNiveladora.Fecha.Value.TimeOfDay : null;

                                this.vista.HorometroEntrega = listadoMotoNiveladora.Horometro.HasValue ? listadoMotoNiveladora.Horometro : null;
                                this.vista.CombustibleEntrega = listadoMotoNiveladora.Combustible.HasValue ? (int?)listadoMotoNiveladora.Combustible : null;
                                #endregion

                                #region Cuestionario
                                this.presentadorMotoNiveladoraE.DatoToInterfazUsuario(listadoMotoNiveladora);
                                this.vista.ObservacionesEntrega = !string.IsNullOrEmpty(listadoMotoNiveladora.Observaciones) ? listadoMotoNiveladora.Observaciones : string.Empty;

                                #endregion

                                #region Documentos Check List

                                if (listadoMotoNiveladora.Adjuntos != null)
                                    this.DesplegarDocumentosCheckList(listadoMotoNiveladora.Adjuntos);
                                #endregion
                                break;
                        }
                    }
                    if ((EAreaConstruccion)this.vista.Area == EAreaConstruccion.RE) {
                        this.presentadorSubArrendadosE.DatoToInterfazUsuario((ListadoVerificacionSubArrendadoBO)listadoVerificacion);
                    }
                    break;
                case (int)ETipoEmpresa.Generacion:
                case (int)ETipoEmpresa.Equinova:
                    this.presentadorEntregaRecepcionE.DatoToInterfazUsuario((ListadoVerificacionEntregaRecepcionBO)listadoVerificacion);
                    break;
            }
        }

        /// <summary>
        /// Despliega en la vista los docuemntos cargados al check list de entrega
        /// </summary>
        /// <param name="lista">Lista de documentos del check list</param>
        private void DesplegarDocumentosCheckList(List<ArchivoBO> lista) {
            if (lista == null)
                return;

            this.presentadorDocumentosEntrega.Vista.EstablecerListasArchivos(lista, new List<TipoArchivoBO>());
            this.presentadorDocumentosEntrega.ModoEditable(false);
        }

        /// <summary>
        /// Despliega la información general necesaria para el registro del check list
        /// </summary>
        /// <param name="contratoRDBO"></param>
        private void DesplegarInformacionGeneral(ContratoPSLBO contratoPSLBO) {
            if (ReferenceEquals(contratoPSLBO, null))
                throw new Exception("No es posible recuperar la información del contrato necesaria para el registro del check list. Intente de nuevo por favor.");

            this.vista.ContratoID = contratoPSLBO.ContratoID ?? null;
            this.vista.FechaContrato = contratoPSLBO.FechaContrato ?? null;
            this.vista.HoraContrato = contratoPSLBO.FechaContrato.HasValue
                                          ? (TimeSpan?)contratoPSLBO.FechaContrato.Value.TimeOfDay
                                          : null;
            this.vista.EstatusContratoID = contratoPSLBO.Estatus.HasValue ? (int?)contratoPSLBO.Estatus : null;
            this.vista.NumeroContrato = !string.IsNullOrEmpty(contratoPSLBO.NumeroContrato) && !string.IsNullOrWhiteSpace(contratoPSLBO.NumeroContrato)
                                            ? contratoPSLBO.NumeroContrato.Trim().ToUpper()
                                            : string.Empty;
            vista.FechaListado = DateTime.Today;
            vista.HoraListado = DateTime.Now.TimeOfDay;
            vista.TipoContrato = (int)contratoPSLBO.Tipo;

            #region Cliente
            if (!ReferenceEquals(contratoPSLBO.Cliente, null)) {
                this.vista.NombreCliente = !string.IsNullOrEmpty(contratoPSLBO.Cliente.Nombre) && !string.IsNullOrWhiteSpace(contratoPSLBO.Cliente.Nombre)
                                               ? contratoPSLBO.Cliente.Nombre.Trim().ToUpper()
                                               : string.Empty;
            }
            #endregion

            if (ReferenceEquals(contratoPSLBO.LineasContrato, null))
                throw new Exception("No es posible recuperar la información de la unidad necesaria para el registro del check list. Intente de nuevo por favor.");

            if (contratoPSLBO.LineasContrato.Count() <= 0)
                throw new Exception("No es posible recuperar la información de la unidad necesaria para el registro del check list. Intente de nuevo por favor.");

            //Asignamos el identificador de la linea de contrato
            int? linea = (int)this.vista.ObtenerPaqueteLineaContrato();
            this.vista.LineaContratoID = contratoPSLBO.LineasContrato.FirstOrDefault(x => x.LineaContratoID == linea).LineaContratoID;

            #region Obtener Unidad Completa
            UnidadBR unidadBR = new UnidadBR();
            List<UnidadBO> unidades = unidadBR.ConsultarDetalle(dctx, (UnidadBO)contratoPSLBO.LineasContrato.FirstOrDefault(x => x.LineaContratoID == linea).Equipo, true);
            var lstSucursales = new List<SucursalBO>();
            if (unidades.Count > 0) {
                UnidadBO unResult = unidades.FindLast(unid => unid.UnidadID == ((UnidadBO)contratoPSLBO.LineasContrato.FirstOrDefault(x => x.LineaContratoID == linea).Equipo).UnidadID);

                if (unResult != null) {

                    CatalogoBaseBO catalogoBase = unResult.TipoEquipoServicio;
                    unResult.TipoEquipoServicio = FacadeBR.ConsultarTipoUnidad(dctx, catalogoBase).FirstOrDefault();

                    if (lstSucursales.Count <= 0)
                        lstSucursales = FacadeBR.ConsultarSucursal(dctx, unResult.Sucursal);

                    var sucTemp = lstSucursales.FirstOrDefault(x => x.Id == unResult.Sucursal.Id);

                    if (sucTemp == null)
                        lstSucursales.AddRange(FacadeBR.ConsultarSucursal(dctx, unResult.Sucursal));

                    sucTemp = lstSucursales.FirstOrDefault(x => x.Id == unResult.Sucursal.Id);

                    unResult.Sucursal = sucTemp;

                    contratoPSLBO.LineasContrato.FirstOrDefault(x => x.LineaContratoID == linea).Equipo = unResult;
                }
            }
            #endregion

            //Desplegamos la información de la unidad                
            this.DesplegarInformacionUnidad(contratoPSLBO.LineasContrato.FirstOrDefault(x => x.LineaContratoID == linea).Equipo as UnidadBO);

            //Consultamos los listados de verificacion de entrega posibles para la linea
            ContratoPSLBR contratoBR = new ContratoPSLBR();
            List<AVerificacionLineaPSLBO> lista = new List<AVerificacionLineaPSLBO>();

            switch (this.vista.UnidadOperativaID) {
                case (int)ETipoEmpresa.Construccion:
                    if ((EAreaConstruccion)this.vista.Area == EAreaConstruccion.RO || (EAreaConstruccion)this.vista.Area == EAreaConstruccion.ROC) {
                        //Aquí se obtendrá el tipo de check list que se mostrará
                        this.vista.TipoListadoVerificacionPSL = (int)controlador.ObtenerTipoUnidadPorClave(this.dctx, (contratoPSLBO.LineasContrato.FirstOrDefault(x => x.LineaContratoID == linea).Equipo as UnidadBO).TipoEquipoServicio.NombreCorto, contratoPSLBO.LineasContrato.FirstOrDefault(x => x.LineaContratoID == linea).Equipo as UnidadBO);
                        switch (this.vista.TipoListadoVerificacionPSL) {
                            case (int)ETipoUnidad.LV_EXCAVADORA:
                                lista.AddRange(contratoBR.ConsultarListadoVerificacion<ListadoVerificacionExcavadoraBO>(this.dctx, new ListadoVerificacionExcavadoraBO { Tipo = ETipoListadoVerificacion.ENTREGA, LineaContratoPSLID = this.vista.LineaContratoID }, (int)this.vista.LineaContratoID.Value));
                                if (ReferenceEquals(lista, null))
                                    throw new Exception("No es posible recuperar la información de la entrega de la unidad necesaria para el registro del check list. Intente de nuevo por favor.");

                                if (lista.Count <= 0)
                                    throw new Exception("No es posible recuperar la información de la entrega de la unidad necesaria para el registro del check list. Intente de nuevo por favor.");

                                if (((LineaContratoPSLBO)contratoPSLBO.LineasContrato.FirstOrDefault(x => x.LineaContratoID == linea)).ListadosVerificacion == null)
                                    ((LineaContratoPSLBO)contratoPSLBO.LineasContrato.FirstOrDefault(x => x.LineaContratoID == linea)).ListadosVerificacion = new List<AVerificacionLineaPSLBO>();

                                ((LineaContratoPSLBO)contratoPSLBO.LineasContrato.FirstOrDefault(x => x.LineaContratoID == linea)).AgregarListadosVerificacion(lista);

                                this.DesplegarInformacionListadoEntrega((ListadoVerificacionExcavadoraBO)lista[0]);

                                break;

                            case (int)ETipoUnidad.LV_RETRO_EXCAVADORA:
                                lista.AddRange(contratoBR.ConsultarListadoVerificacion<ListadoVerificacionRetroExcavadoraBO>(this.dctx, new ListadoVerificacionRetroExcavadoraBO { Tipo = ETipoListadoVerificacion.ENTREGA }, (int)this.vista.LineaContratoID.Value));
                                if (ReferenceEquals(lista, null))
                                    throw new Exception("No es posible recuperar la información de la entrega de la unidad necesaria para el registro del check list. Intente de nuevo por favor.");

                                if (lista.Count <= 0)
                                    throw new Exception("No es posible recuperar la información de la entrega de la unidad necesaria para el registro del check list. Intente de nuevo por favor.");

                                if (((LineaContratoPSLBO)contratoPSLBO.LineasContrato.FirstOrDefault(x => x.LineaContratoID == linea)).ListadosVerificacion == null)
                                    ((LineaContratoPSLBO)contratoPSLBO.LineasContrato.FirstOrDefault(x => x.LineaContratoID == linea)).ListadosVerificacion = new List<AVerificacionLineaPSLBO>();

                                ((LineaContratoPSLBO)contratoPSLBO.LineasContrato.FirstOrDefault(x => x.LineaContratoID == linea)).AgregarListadosVerificacion(lista);

                                this.DesplegarInformacionListadoEntrega(lista[0]);

                                break;
                            case (int)ETipoUnidad.LV_COMPRESOR:
                                lista.AddRange(contratoBR.ConsultarListadoVerificacion<ListadoVerificacionCompresorPortatilBO>(this.dctx, new ListadoVerificacionCompresorPortatilBO { Tipo = ETipoListadoVerificacion.ENTREGA, LineaContratoPSLID = this.vista.LineaContratoID }, (int)this.vista.LineaContratoID.Value));
                                if (ReferenceEquals(lista, null))
                                    throw new Exception("No es posible recuperar la información de la entrega de la unidad necesaria para el registro del check list. Intente de nuevo por favor.");

                                if (lista.Count <= 0)
                                    throw new Exception("No es posible recuperar la información de la entrega de la unidad necesaria para el registro del check list. Intente de nuevo por favor.");

                                if (((LineaContratoPSLBO)contratoPSLBO.LineasContrato[0]).ListadosVerificacion == null)
                                    ((LineaContratoPSLBO)contratoPSLBO.LineasContrato[0]).ListadosVerificacion = new List<AVerificacionLineaPSLBO>();

                                ((LineaContratoPSLBO)contratoPSLBO.LineasContrato[0]).AgregarListadosVerificacion(lista);

                                this.DesplegarInformacionListadoEntrega((ListadoVerificacionCompresorPortatilBO)lista[0]);

                                break;

                            case (int)ETipoUnidad.LV_VIBRO_COMPACTADOR:
                                lista.AddRange(contratoBR.ConsultarListadoVerificacion<ListadoVerificacionVibroCompactadorBO>(this.dctx, new ListadoVerificacionVibroCompactadorBO { Tipo = ETipoListadoVerificacion.ENTREGA, LineaContratoPSLID = this.vista.LineaContratoID }, (int)this.vista.LineaContratoID.Value));
                                if (ReferenceEquals(lista, null))
                                    throw new Exception("No es posible recuperar la información de la entrega de la unidad necesaria para el registro del check list. Intente de nuevo por favor.");

                                if (lista.Count <= 0)
                                    throw new Exception("No es posible recuperar la información de la entrega de la unidad necesaria para el registro del check list. Intente de nuevo por favor.");

                                if (((LineaContratoPSLBO)contratoPSLBO.LineasContrato.FirstOrDefault(x => x.LineaContratoID == linea)).ListadosVerificacion == null)
                                    ((LineaContratoPSLBO)contratoPSLBO.LineasContrato.FirstOrDefault(x => x.LineaContratoID == linea)).ListadosVerificacion = new List<AVerificacionLineaPSLBO>();

                                ((LineaContratoPSLBO)contratoPSLBO.LineasContrato.FirstOrDefault(x => x.LineaContratoID == linea)).AgregarListadosVerificacion(lista);

                                this.DesplegarInformacionListadoEntrega((ListadoVerificacionVibroCompactadorBO)lista[0]);

                                break;
                            case (int)ETipoUnidad.LV_PISTOLA_NEUMATICA:
                                lista.AddRange(contratoBR.ConsultarListadoVerificacion<ListadoVerificacionPistolaNeumaticaBO>(this.dctx, new ListadoVerificacionPistolaNeumaticaBO { Tipo = ETipoListadoVerificacion.ENTREGA, LineaContratoPSLID = this.vista.LineaContratoID }, (int)this.vista.LineaContratoID.Value));
                                if (ReferenceEquals(lista, null))
                                    throw new Exception("No es posible recuperar la información de la entrega de la unidad necesaria para el registro del check list. Intente de nuevo por favor.");

                                if (lista.Count <= 0)
                                    throw new Exception("No es posible recuperar la información de la entrega de la unidad necesaria para el registro del check list. Intente de nuevo por favor.");

                                if (((LineaContratoPSLBO)contratoPSLBO.LineasContrato.FirstOrDefault(x => x.LineaContratoID == linea)).ListadosVerificacion == null)
                                    ((LineaContratoPSLBO)contratoPSLBO.LineasContrato.FirstOrDefault(x => x.LineaContratoID == linea)).ListadosVerificacion = new List<AVerificacionLineaPSLBO>();

                                ((LineaContratoPSLBO)contratoPSLBO.LineasContrato.FirstOrDefault(x => x.LineaContratoID == linea)).AgregarListadosVerificacion(lista);

                                this.DesplegarInformacionListadoEntrega((ListadoVerificacionPistolaNeumaticaBO)lista[0]);

                                break;
                            case (int)ETipoUnidad.LV_MARTILLO_HIDRAULICO:
                                lista.AddRange(contratoBR.ConsultarListadoVerificacion<ListadoVerificacionMartilloHidraulicoBO>(this.dctx, new ListadoVerificacionMartilloHidraulicoBO { Tipo = ETipoListadoVerificacion.ENTREGA, LineaContratoPSLID = this.vista.LineaContratoID }, (int)this.vista.LineaContratoID.Value));
                                if (ReferenceEquals(lista, null))
                                    throw new Exception("No es posible recuperar la información de la entrega de la unidad necesaria para el registro del check list. Intente de nuevo por favor.");

                                if (lista.Count <= 0)
                                    throw new Exception("No es posible recuperar la información de la entrega de la unidad necesaria para el registro del check list. Intente de nuevo por favor.");

                                if (((LineaContratoPSLBO)contratoPSLBO.LineasContrato.FirstOrDefault(x => x.LineaContratoID == linea)).ListadosVerificacion == null)
                                    ((LineaContratoPSLBO)contratoPSLBO.LineasContrato.FirstOrDefault(x => x.LineaContratoID == linea)).ListadosVerificacion = new List<AVerificacionLineaPSLBO>();

                                ((LineaContratoPSLBO)contratoPSLBO.LineasContrato.FirstOrDefault(x => x.LineaContratoID == linea)).AgregarListadosVerificacion(lista);

                                this.DesplegarInformacionListadoEntrega((ListadoVerificacionMartilloHidraulicoBO)lista[0]);

                                break;
                            case (int)ETipoUnidad.LV_TORRES_LUZ:
                                lista.AddRange(contratoBR.ConsultarListadoVerificacion<ListadoVerificacionTorresLuzBO>(this.dctx, new ListadoVerificacionTorresLuzBO { Tipo = ETipoListadoVerificacion.ENTREGA, LineaContratoPSLID = this.vista.LineaContratoID }, (int)this.vista.LineaContratoID.Value));
                                if (ReferenceEquals(lista, null))
                                    throw new Exception("No es posible recuperar la información de la entrega de la unidad necesaria para el registro del check list. Intente de nuevo por favor.");

                                if (lista.Count <= 0)
                                    throw new Exception("No es posible recuperar la información de la entrega de la unidad necesaria para el registro del check list. Intente de nuevo por favor.");

                                if (((LineaContratoPSLBO)contratoPSLBO.LineasContrato.FirstOrDefault(x => x.LineaContratoID == linea)).ListadosVerificacion == null)
                                    ((LineaContratoPSLBO)contratoPSLBO.LineasContrato.FirstOrDefault(x => x.LineaContratoID == linea)).ListadosVerificacion = new List<AVerificacionLineaPSLBO>();

                                ((LineaContratoPSLBO)contratoPSLBO.LineasContrato.FirstOrDefault(x => x.LineaContratoID == linea)).AgregarListadosVerificacion(lista);

                                this.DesplegarInformacionListadoEntrega((ListadoVerificacionTorresLuzBO)lista[0]);

                                break;
                            case (int)ETipoUnidad.LV_MOTONIVELADORA:
                                lista.AddRange(contratoBR.ConsultarListadoVerificacion<ListadoVerificacionMotoNiveladoraBO>(this.dctx, new ListadoVerificacionMotoNiveladoraBO { Tipo = ETipoListadoVerificacion.ENTREGA, LineaContratoPSLID = this.vista.LineaContratoID }, (int)this.vista.LineaContratoID.Value));
                                if (ReferenceEquals(lista, null))
                                    throw new Exception("No es posible recuperar la información de la entrega de la unidad necesaria para el registro del check list. Intente de nuevo por favor.");

                                if (lista.Count <= 0)
                                    throw new Exception("No es posible recuperar la información de la entrega de la unidad necesaria para el registro del check list. Intente de nuevo por favor.");

                                if (((LineaContratoPSLBO)contratoPSLBO.LineasContrato.FirstOrDefault(x => x.LineaContratoID == linea)).ListadosVerificacion == null)
                                    ((LineaContratoPSLBO)contratoPSLBO.LineasContrato.FirstOrDefault(x => x.LineaContratoID == linea)).ListadosVerificacion = new List<AVerificacionLineaPSLBO>();

                                ((LineaContratoPSLBO)contratoPSLBO.LineasContrato.FirstOrDefault(x => x.LineaContratoID == linea)).AgregarListadosVerificacion(lista);

                                this.DesplegarInformacionListadoEntrega((ListadoVerificacionMotoNiveladoraBO)lista[0]);

                                break;

                            case (int)ETipoUnidad.LV_MONTACARGA:
                                lista.AddRange(contratoBR.ConsultarListadoVerificacion<ListadoVerificacionMontaCargaBO>(this.dctx, new ListadoVerificacionMontaCargaBO { Tipo = ETipoListadoVerificacion.ENTREGA, LineaContratoPSLID = this.vista.LineaContratoID }, (int)this.vista.LineaContratoID.Value));
                                if (ReferenceEquals(lista, null))
                                    throw new Exception("No es posible recuperar la información de la entrega de la unidad necesaria para el registro del check list. Intente de nuevo por favor.");

                                if (lista.Count <= 0)
                                    throw new Exception("No es posible recuperar la información de la entrega de la unidad necesaria para el registro del check list. Intente de nuevo por favor.");

                                if (((LineaContratoPSLBO)contratoPSLBO.LineasContrato.FirstOrDefault(x => x.LineaContratoID == linea)).ListadosVerificacion == null)
                                    ((LineaContratoPSLBO)contratoPSLBO.LineasContrato.FirstOrDefault(x => x.LineaContratoID == linea)).ListadosVerificacion = new List<AVerificacionLineaPSLBO>();

                                ((LineaContratoPSLBO)contratoPSLBO.LineasContrato.FirstOrDefault(x => x.LineaContratoID == linea)).AgregarListadosVerificacion(lista);

                                this.DesplegarInformacionListadoEntrega((ListadoVerificacionMontaCargaBO)lista[0]);

                                break;

                            case (int)ETipoUnidad.LV_MINICARGADOR:
                                lista.AddRange(contratoBR.ConsultarListadoVerificacion<ListadoVerificacionMiniCargadorBO>(this.dctx, new ListadoVerificacionMiniCargadorBO { Tipo = ETipoListadoVerificacion.ENTREGA, LineaContratoPSLID = this.vista.LineaContratoID }, (int)this.vista.LineaContratoID.Value));
                                if (ReferenceEquals(lista, null))
                                    throw new Exception("No es posible recuperar la información de la entrega de la unidad necesaria para el registro del check list. Intente de nuevo por favor.");

                                if (lista.Count <= 0)
                                    throw new Exception("No es posible recuperar la información de la entrega de la unidad necesaria para el registro del check list. Intente de nuevo por favor.");

                                if (((LineaContratoPSLBO)contratoPSLBO.LineasContrato.FirstOrDefault(x => x.LineaContratoID == linea)).ListadosVerificacion == null)
                                    ((LineaContratoPSLBO)contratoPSLBO.LineasContrato.FirstOrDefault(x => x.LineaContratoID == linea)).ListadosVerificacion = new List<AVerificacionLineaPSLBO>();

                                ((LineaContratoPSLBO)contratoPSLBO.LineasContrato.FirstOrDefault(x => x.LineaContratoID == linea)).AgregarListadosVerificacion(lista);

                                this.DesplegarInformacionListadoEntrega((ListadoVerificacionMiniCargadorBO)lista[0]);

                                break;

                            case (int)ETipoUnidad.LV_PLATAFORMA_TIJERAS:
                                lista.AddRange(contratoBR.ConsultarListadoVerificacion<ListadoVerificacionPlataformaTijerasBO>(this.dctx, new ListadoVerificacionPlataformaTijerasBO { Tipo = ETipoListadoVerificacion.ENTREGA, LineaContratoPSLID = this.vista.LineaContratoID }, (int)this.vista.LineaContratoID.Value));
                                if (ReferenceEquals(lista, null))
                                    throw new Exception("No es posible recuperar la información de la entrega de la unidad necesaria para el registro del check list. Intente de nuevo por favor.");

                                if (lista.Count <= 0)
                                    throw new Exception("No es posible recuperar la información de la entrega de la unidad necesaria para el registro del check list. Intente de nuevo por favor.");

                                if (((LineaContratoPSLBO)contratoPSLBO.LineasContrato[0]).ListadosVerificacion == null)
                                    ((LineaContratoPSLBO)contratoPSLBO.LineasContrato[0]).ListadosVerificacion = new List<AVerificacionLineaPSLBO>();

                                ((LineaContratoPSLBO)contratoPSLBO.LineasContrato[0]).AgregarListadosVerificacion(lista);

                                this.DesplegarInformacionListadoEntrega((ListadoVerificacionPlataformaTijerasBO)lista[0]);
                                break;
                        }
                    }
                    if ((EAreaConstruccion)this.vista.Area == EAreaConstruccion.RE) {
                        this.vista.TipoListadoVerificacionPSL = (int)ETipoUnidad.LV_SUBARRENDADO;
                        lista.AddRange(contratoBR.ConsultarListadoVerificacion<ListadoVerificacionSubArrendadoBO>(this.dctx, new ListadoVerificacionSubArrendadoBO { Tipo = ETipoListadoVerificacion.ENTREGA }, (int)this.vista.LineaContratoID.Value));

                        if (ReferenceEquals(lista, null))
                            throw new Exception("No es posible recuperar la información de la entrega de la unidad necesaria para el registro del check list. Intente de nuevo por favor.");

                        if (lista.Count <= 0)
                            throw new Exception("No es posible recuperar la información de la entrega de la unidad necesaria para el registro del check list. Intente de nuevo por favor.");

                        if (((LineaContratoPSLBO)contratoPSLBO.LineasContrato.FirstOrDefault(x => x.LineaContratoID == linea)).ListadosVerificacion == null)
                            ((LineaContratoPSLBO)contratoPSLBO.LineasContrato.FirstOrDefault(x => x.LineaContratoID == linea)).ListadosVerificacion = new List<AVerificacionLineaPSLBO>();

                        ((LineaContratoPSLBO)contratoPSLBO.LineasContrato.FirstOrDefault(x => x.LineaContratoID == linea)).AgregarListadosVerificacion(lista);

                        this.DesplegarInformacionListadoEntrega((ListadoVerificacionSubArrendadoBO)lista[0]);
                    }
                    break;

                case (int)ETipoEmpresa.Generacion:
                case (int)ETipoEmpresa.Equinova:
                    this.vista.TipoListadoVerificacionPSL = (int)ETipoUnidad.LV_ENTREGA_RECEPCION;
                    lista.AddRange(contratoBR.ConsultarListadoVerificacion<ListadoVerificacionEntregaRecepcionBO>(this.dctx, new ListadoVerificacionEntregaRecepcionBO { Tipo = ETipoListadoVerificacion.ENTREGA }, (int)this.vista.LineaContratoID.Value));
                    if (ReferenceEquals(lista, null))
                        throw new Exception("No es posible recuperar la información de la entrega de la unidad necesaria para el registro del check list. Intente de nuevo por favor.");

                    if (lista.Count <= 0)
                        throw new Exception("No es posible recuperar la información de la entrega de la unidad necesaria para el registro del check list. Intente de nuevo por favor.");

                    if (((LineaContratoPSLBO)contratoPSLBO.LineasContrato.FirstOrDefault(x => x.LineaContratoID == linea)).ListadosVerificacion == null)
                        ((LineaContratoPSLBO)contratoPSLBO.LineasContrato.FirstOrDefault(x => x.LineaContratoID == linea)).ListadosVerificacion = new List<AVerificacionLineaPSLBO>();

                    ((LineaContratoPSLBO)contratoPSLBO.LineasContrato.FirstOrDefault(x => x.LineaContratoID == linea)).AgregarListadosVerificacion(lista);

                    this.DesplegarInformacionListadoEntrega((ListadoVerificacionEntregaRecepcionBO)lista[0]);
                    break;
            }
        }

        /// <summary>
        /// Cancela el registro del check list
        /// </summary>
        public void CancelarRegistro() {
            this.vista.LimpiarSesion();
            this.vista.LimpiarPaqueteNavegacion();
            this.vista.RedirigirAConsulta();
        }

        /// <summary>
        /// Cancela el contrato al que se le registro el check list
        /// </summary>
        public bool CancelarContrato(Dictionary<string, Object> datos) {
            //Se crea el objeto de seguridad
            UsuarioBO usuario = new UsuarioBO() { Id = this.vista.UsuarioID };
            AdscripcionBO adscripcion = new AdscripcionBO() { UnidadOperativa = new UnidadOperativaBO() { Id = this.vista.UnidadOperativaID } };
            SeguridadBO seguridadBO = new SeguridadBO(Guid.NewGuid(), usuario, adscripcion);

            bool redirigir = false;
            if (this.vista.HorometroEntrega.Value == this.vista.Horometro.Value) {
                this.vista.MostrarMensaje("El contrato al que pertenece el check list, va a ser cancelado.", ETipoMensajeIU.ADVERTENCIA, null);
                this.vista.LimpiarSesion();
                this.vista.LimpiarPaqueteNavegacion();

                #region Consulta contrato
                if (this.vista.ContratoID.HasValue) {
                    ContratoPSLBO contrato = new ContratoPSLBO { ContratoID = this.vista.ContratoID };
                    ContratoPSLBR contratoBR = new ContratoPSLBR();
                    List<ContratoPSLBO> contratos = contratoBR.ConsultarCompleto(this.dctx, contrato);

                    if (!ReferenceEquals(contratos, null))
                        if (contratos.Count > 0)
                            contrato = contratos[0];

                    if (contrato.LineasContrato.Count > 1) {
                        int? lineaID = this.vista.LineaContratoID;
                        ILineaContrato lineaContrato = contrato.LineasContrato.Find(x => x.LineaContratoID == lineaID);
                        contratoBR.CancelarLinea(dctx, lineaContrato as LineaContratoPSLBO, lineaContrato as LineaContratoPSLBO, contrato, seguridadBO);
                        string mensajeError = this.EnviarAutorizador(datos, (int)this.vista.ModuloID, contrato);
                        this.vista.RedirigirAImprimir(mensajeError);
                    } else {
                        string mensajeError = this.EnviarAutorizador(datos, (int)this.vista.ModuloID, contrato);
                        this.vista.EstablecerPaqueteNavegacionCancelar("UltimoContratoPSLBO", contrato);
                        this.vista.RedirigirACancelarContrato();
                    }
                }

                #endregion
            } else
                redirigir = true;

            return redirigir;
        }
        /// <summary>
        /// Registramos la recepción de la unidad
        /// </summary>
        public bool RegistrarRecepcion() {
            bool redirigir = false;

            //Asignamos el tipo de listado
            this.vista.TipoListado = (int)ETipoListadoVerificacion.RECEPCION;

            //Registramos en Check list
            this.Registrar();

            //Limpiamos las variables de session
            this.vista.LimpiarSesion();
            this.vista.LimpiarPaqueteNavegacion();

            //Establecemos el paquete de navegación para el imprimir check list
            var contrato = new ContratoPSLBO { ContratoID = this.vista.ContratoID };

            int moduloID = (int)this.vista.ModuloID;
            int? linea = this.vista.LineaContratoID;
            this.vista.LineaContratoID = linea;

            var datos = this.controlador.ObtenerDatosCheckList(this.dctx, contrato, moduloID, ((ETipoUnidad)this.vista.TipoListadoVerificacionPSL), linea);
            #region Asignar al contrato
            if (datos.ContainsKey("Contrato") && datos["Contrato"] != null)
                contrato = (ContratoPSLBO)datos["Contrato"];
            #endregion
            datos.Add("ContratoPSLBO", datos);
            this.vista.EstablecerPaqueteNavegacion("CheckListEntregaRO", datos);

            #region Validación de cancelación
            //Valida si es el caso de una cancelación
            redirigir = this.CancelarContrato(datos);
            if (redirigir) {
                string mensajeError = this.EnviarAutorizador(datos, moduloID, contrato);
                this.vista.RedirigirAImprimir(mensajeError);
            }
            #endregion

            return redirigir;
        }

        #region Generar Reporte Adjunto
        /// <summary>
        /// Genera el array de bytes del reporte de checklist de Entrega
        /// </summary>
        /// <param name="datosReporte">Diccionario de datos con la información del reporte.</param>
        /// <param name="tipoReporte">Tipo de Reporte que se generará.</param>
        /// <returns></returns>
        public byte[] GenerarReporteAdjuntar(Dictionary<string, object> datosReporte, ETipoUnidad tipoReporte) {
            switch (tipoReporte) {
                case ETipoUnidad.LV_COMPRESOR:
                    using (ListadoVerificacionCompresoresPortatilesRPT oReporte = new ListadoVerificacionCompresoresPortatilesRPT(datosReporte)) {
                        using (MemoryStream stream = new MemoryStream()) {
                            oReporte.CreateDocument();
                            oReporte.ExportToPdf(stream);
                            return stream.GetBuffer();
                        }
                    }
                case ETipoUnidad.LV_ENTREGA_RECEPCION:
                    using (ListadoVerificacionEntregaRecepcionRPT oReporte = new ListadoVerificacionEntregaRecepcionRPT(datosReporte)) {
                        using (MemoryStream stream = new MemoryStream()) {
                            oReporte.CreateDocument();
                            oReporte.ExportToPdf(stream);
                            return stream.GetBuffer();
                        }
                    }
                case ETipoUnidad.LV_EXCAVADORA:
                    using (ListadoVerificacionExcavadoraRPT oReporte = new ListadoVerificacionExcavadoraRPT(datosReporte)) {
                        using (MemoryStream stream = new MemoryStream()) {
                            oReporte.CreateDocument();
                            oReporte.ExportToPdf(stream);
                            return stream.GetBuffer();
                        }
                    }
                case ETipoUnidad.LV_MARTILLO_HIDRAULICO:
                    using (ListadoVerificacionMartilloHidraulicoRPT oReporte = new ListadoVerificacionMartilloHidraulicoRPT(datosReporte)) {
                        using (MemoryStream stream = new MemoryStream()) {
                            oReporte.CreateDocument();
                            oReporte.ExportToPdf(stream);
                            return stream.GetBuffer();
                        }
                    }
                case ETipoUnidad.LV_MINICARGADOR:
                    using (ListadoVerificacionMiniCargadorRPT2 oReporte = new ListadoVerificacionMiniCargadorRPT2(datosReporte)) {
                        using (MemoryStream stream = new MemoryStream()) {
                            oReporte.CreateDocument();
                            oReporte.ExportToPdf(stream);
                            return stream.GetBuffer();
                        }
                    }
                case ETipoUnidad.LV_MONTACARGA:
                    using (ListadoVerificacionMontaCargasRPT oReporte = new ListadoVerificacionMontaCargasRPT(datosReporte)) {
                        using (MemoryStream stream = new MemoryStream()) {
                            oReporte.CreateDocument();
                            oReporte.ExportToPdf(stream);
                            return stream.GetBuffer();
                        }
                    }
                case ETipoUnidad.LV_MOTONIVELADORA:
                    using (ListadoVerificacionMotoNiveladoraRPT oReporte = new ListadoVerificacionMotoNiveladoraRPT(datosReporte)) {
                        using (MemoryStream stream = new MemoryStream()) {
                            oReporte.CreateDocument();
                            oReporte.ExportToPdf(stream);
                            return stream.GetBuffer();
                        }
                    }
                case ETipoUnidad.LV_PISTOLA_NEUMATICA:
                    using (ListadoVerificacionPistolaNeumaticaRPT oReporte = new ListadoVerificacionPistolaNeumaticaRPT(datosReporte)) {
                        using (MemoryStream stream = new MemoryStream()) {
                            oReporte.CreateDocument();
                            oReporte.ExportToPdf(stream);
                            return stream.GetBuffer();
                        }
                    }
                case ETipoUnidad.LV_PLATAFORMA_TIJERAS:
                    using (ListadoVerificacionPlataformaTijerasRPT oReporte = new ListadoVerificacionPlataformaTijerasRPT(datosReporte)) {
                        using (MemoryStream stream = new MemoryStream()) {
                            oReporte.CreateDocument();
                            oReporte.ExportToPdf(stream);
                            return stream.GetBuffer();
                        }
                    }
                case ETipoUnidad.LV_RETRO_EXCAVADORA:
                    using (ListadoVerificacionRetroExcavadoraRPT oReporte = new ListadoVerificacionRetroExcavadoraRPT(datosReporte)) {
                        using (MemoryStream stream = new MemoryStream()) {
                            oReporte.CreateDocument();
                            oReporte.ExportToPdf(stream);
                            return stream.GetBuffer();
                        }
                    }
                case ETipoUnidad.LV_SUBARRENDADO:
                    using (ListadoVerificacionSubArrendadosRPT oReporte = new ListadoVerificacionSubArrendadosRPT(datosReporte)) {
                        using (MemoryStream stream = new MemoryStream()) {
                            oReporte.CreateDocument();
                            oReporte.ExportToPdf(stream);
                            return stream.GetBuffer();
                        }
                    }
                case ETipoUnidad.LV_TORRES_LUZ:
                    using (ListadoVerificacionTorresLuzRPT oReporte = new ListadoVerificacionTorresLuzRPT(datosReporte)) {
                        using (MemoryStream stream = new MemoryStream()) {
                            oReporte.CreateDocument();
                            oReporte.ExportToPdf(stream);
                            return stream.GetBuffer();
                        }
                    }
                case ETipoUnidad.LV_VIBRO_COMPACTADOR:
                    using (ListadoVerificacionVibroCompactadorRPT oReporte = new ListadoVerificacionVibroCompactadorRPT(datosReporte)) {
                        using (MemoryStream stream = new MemoryStream()) {
                            oReporte.CreateDocument();
                            oReporte.ExportToPdf(stream);
                            return stream.GetBuffer();
                        }
                    }
                default:
                    return null;
            }
        }
        #endregion

        /// <summary>
        /// Registra el Check list
        /// </summary>
        public void Registrar() {
            #region Se inicia la Transaccion
            this.dctx.SetCurrentProvider("Outsourcing");
            Guid firma = Guid.NewGuid();
            try {
                this.dctx.OpenConnection(firma);
                this.dctx.BeginTransaction(firma);
            } catch (Exception) {
                if (this.dctx.ConnectionState == ConnectionState.Open)
                    this.dctx.CloseConnection(firma);
                throw new Exception("Se encontraron inconsistencias registrar el Check de Recepción.");
            }
            #endregion

            try {
                //Se obtiene la información a partir de la Interfaz de Usuario
                ContratoPSLBO bo = (ContratoPSLBO)this.InterfazUsuarioADato();

                //Se crea el objeto de seguridad
                UsuarioBO usuario = new UsuarioBO() { Id = this.vista.UsuarioID };
                AdscripcionBO adscripcion = new AdscripcionBO() { UnidadOperativa = new UnidadOperativaBO() { Id = this.vista.UnidadOperativaID } };
                SeguridadBO seguridadBO = new SeguridadBO(Guid.NewGuid(), usuario, adscripcion);

                //Se inserta en la BD
                this.controlador.RegistrarRecepcion(this.dctx, bo, seguridadBO);

                //Se obtiene la fecha del check List
                int? lineaID = this.vista.LineaContratoID;
                var linea = bo.LineasContrato.FirstOrDefault(x => x.LineaContratoID == lineaID) as LineaContratoPSLBO;
                var listadoRecepcion = linea.ListadosVerificacion.First(x => x.Tipo == ETipoListadoVerificacion.RECEPCION);

                var contrato = this.controlador.Consultar(this.dctx, new ContratoPSLBO() { ContratoID = bo.ContratoID }).First();

                //Se determina si se deben inactivar pagos.
                bool inactivarPagos = DateTime.Compare((DateTime)listadoRecepcion.ObtenerFechaDevolucion(), (DateTime)contrato.FechaPromesaDevolucion) < 0;


                #region Cerrar Transaccion
                this.dctx.SetCurrentProvider("Outsourcing");
                this.dctx.CommitTransaction(firma);
                #endregion
            } catch (Exception ex) {
                this.dctx.SetCurrentProvider("Outsourcing");
                this.dctx.RollbackTransaction(firma);

                throw new Exception(nombreClase + ".Registrar:" + Environment.NewLine + ex.Message);
            } finally {
                if (this.dctx.ConnectionState == ConnectionState.Open)
                    this.dctx.CloseConnection(firma);
            }
        }

        #region SC0001
        public string RegistrarOrdenServicioLavado() {
            string mensaje = string.Empty;
            var contrato = this.controlador.Consultar(this.dctx, new ContratoPSLBO() { ContratoID = this.vista.ContratoID }).First();
            //Se crea el objeto de seguridad
            UsuarioBO usr = new UsuarioBO { Id = this.vista.UsuarioID };
            AdscripcionBO sdscripcion = new AdscripcionBO
            {
                UnidadOperativa = new UnidadOperativaBO { Id = this.vista.UnidadOperativaID },
                Sucursal = contrato.Sucursal
            };
            SeguridadBO seguridad = new SeguridadBO(Guid.NewGuid(), usr, sdscripcion);
            string Msj = this.ctrlOrdenLavado.RegistrarLavado(this.dctx, (int)this.vista.UnidadID, seguridad, (int)this.vista.KilometrajeDiario, (int)this.vista.Horometro, (int)contrato.ContratoID);
            mensaje = Msj;
            return mensaje;
        }
        #endregion

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

        #region Generar el correo para enviar a los autorizadores

        /// <summary>
        /// Genera el correo para ser enviado a los autorizadores
        /// </summary>
        /// <param name="datos">Diccionario con los datos del checklist</param>
        /// <param name="moduloID">Módulo ID</param>
        /// <param name="contrato">Objeto con los datos del contrato</param>
        public string EnviarAutorizador(Dictionary<string, Object> datos, int moduloID, ContratoPSLBO contrato) {
            byte[] reporte = this.GenerarReporteAdjuntar(datos, (ETipoUnidad)this.vista.TipoListadoVerificacionPSL);
            Enum TipoAutorizacion = this.vista.UnidadOperativaID == (int)ETipoEmpresa.Generacion ? (Enum)ETipoAutorizacionGeneracion.Recepcion_Unidad :
                this.vista.UnidadOperativaID == (int)ETipoEmpresa.Construccion ? (Enum)ETipoAutorizacionConstruccion.Recepcion_Unidad :
                (Enum)ETipoAutorizacionEquinova.Recepcion_Unidad;

            AutorizadorBR oAutorizador = new AutorizadorBR();
            #region Crear el Diccionario para reemplazar los valores del formato con los de la entrega.
            Dictionary<string, string> datosAutorizacion = new Dictionary<string, string>();
            datosAutorizacion.Add("serieUnidad", this.vista.NumeroSerie);
            #endregion
            string error = string.Empty;
            try {
                oAutorizador.SolicitarAutorizacion(dctx, TipoAutorizacion, datosAutorizacion, moduloID, this.vista.UnidadOperativaID, contrato.Sucursal.Id, reporte);
            } catch (Exception ex) {
                error = ex.Message;
            }

            return error;
        }
        #endregion

        #endregion
    }
}