using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
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
using BPMO.SDNI.Facturacion.AplicacionesPago.BR;
using BPMO.SDNI.Facturacion.InfraestructuraPagos.BO;
using BPMO.SDNI.Facturacion.InfraestructuraPagos.BR;
using BPMO.SDNI.Flota.BO;
using BPMO.SDNI.Flota.BOF;
using BPMO.SDNI.Flota.BR;
using BPMO.SDNI.Flota.PRE;
using BPMO.SDNI.Tramites.BO;

namespace BPMO.SDNI.Contratos.PSL.PRE {
    public class RegistrarEntregaPSLPRE {
        #region Atributos

        /// <summary>
        /// Provee la conexión a la BD.
        /// </summary>
        private readonly IDataContext dctx;
        /// <summary>
        /// Nombre de la clase que se usará en los mensajes de error.
        /// </summary>
        private const string nombreClase = "RegistrarEntregaPSLPRE";

        /// <summary>
        /// Vista para la página de registro de entrega de unidad
        /// </summary>
        private readonly IRegistrarEntregaPSLVIS vista;
        /// <summary>
        /// Controlador que ejecutará las acciones para Contratos PSL.
        /// </summary>
        private readonly ContratoPSLBR controlador;

        private ucCatalogoDocumentosPRE presentadorDocumentos;

        /// <summary>
        /// Presentador para el catálogo de documentos.
        /// </summary>
        public ucCatalogoDocumentosPRE PresentadorDocumentos {
            get { return presentadorDocumentos; }
            set { presentadorDocumentos = value; }
        }


        private ucEquiposAliadosUnidadPRE presentadorEquipoAliados;

        /// <summary>
        /// Presentador para los equipos aliados de la unidad.
        /// </summary>
        public ucEquiposAliadosUnidadPRE PresentadorEquipoAliados {
            get { return presentadorEquipoAliados; }
            set { presentadorEquipoAliados = value; }
        }

        private ucPuntosVerificacionExcavadoraPSLPRE presentadorExcavadora;

        /// <summary>
        /// Presentador checklist Excavadora.
        /// </summary>
        public ucPuntosVerificacionExcavadoraPSLPRE PresentadorExcavadora {
            get { return presentadorExcavadora; }
            set { presentadorExcavadora = value; }
        }

        private ucPuntosVerificacionEntregaRecepcionPSLPRE presentadorEntregaRecepcion;

        /// <summary>
        /// Presentador checklist Entrega Recepción.
        /// </summary>
        public ucPuntosVerificacionEntregaRecepcionPSLPRE PresentadorEntregaRecepcion {
            get { return presentadorEntregaRecepcion; }
            set { presentadorEntregaRecepcion = value; }
        }
        private ucPuntosVerificacionRetroExcavadoraPSLPRE presentadorRetroExcavadora;

        /// <summary>
        /// Presentador checklist RetroExcavadora.
        /// </summary>
        public ucPuntosVerificacionRetroExcavadoraPSLPRE PresentadorRetroExcavadora {
            get { return presentadorRetroExcavadora; }
            set { presentadorRetroExcavadora = value; }
        }
        private ucPuntosVerificacionMotoNiveladoraPSLPRE presentadorMotoNiveladora;
        /// <summary>
        /// Presentador checklist MotoNiveladora
        /// </summary>
        public ucPuntosVerificacionMotoNiveladoraPSLPRE PresentadorMotoNiveladora {
            get { return presentadorMotoNiveladora; }
            set { this.presentadorMotoNiveladora = value; }
        }

        private ucPuntosVerificacionVibroCompactadorPSLPRE presentadorVibroCompactador;

        /// <summary>
        /// Presentador checklist VibroCompactador.
        /// </summary>
        public ucPuntosVerificacionVibroCompactadorPSLPRE PresentadorVibroCompactador {
            get { return presentadorVibroCompactador; }
            set { presentadorVibroCompactador = value; }
        }


        private ucPuntosVerificacionTorresLuzPSLPRE presentadorTorresLuz;

        /// <summary>
        /// Presentador checklist TorresLuz.
        /// </summary>
        public ucPuntosVerificacionTorresLuzPSLPRE PresentadorTorresLuz {
            get { return presentadorTorresLuz; }
            set { presentadorTorresLuz = value; }
        }

        private ucPuntosVerificacionMartilloHidraulicoPSLPRE presentadorMartilloHidraulico;

        /// <summary>
        /// Presentador checklist Martillo Hidráulico.
        /// </summary>
        public ucPuntosVerificacionMartilloHidraulicoPSLPRE PresentadorMartilloHidraulico {
            get { return presentadorMartilloHidraulico; }
            set { presentadorMartilloHidraulico = value; }
        }

        private ucPuntosVerificacionMontaCargaPSLPRE presentadorMontaCarga;

        public ucPuntosVerificacionMontaCargaPSLPRE PresentadorMontaCarga {
            get { return presentadorMontaCarga; }
            set { presentadorMontaCarga = value; }
        }

        private ucPuntosVerificacionMiniCargadorPSLPRE presentadorMiniCargador;

        public ucPuntosVerificacionMiniCargadorPSLPRE PresentadorMiniCargador {
            get { return presentadorMiniCargador; }
            set { presentadorMiniCargador = value; }
        }

        private ucPuntosVerificacionCompresoresPortatilesPSLPRE presentadorCompresorPortatil;

        /// <summary>
        /// Presentador checklist CompresorPortatil.
        /// </summary>
        public ucPuntosVerificacionCompresoresPortatilesPSLPRE PresentadorCompresorPortatil {
            get { return presentadorCompresorPortatil; }
            set { presentadorCompresorPortatil = value; }
        }

        private ucPuntosVerificacionPistolaNeumaticaPSLPRE presentadorPistolaNeumatica;

        /// <summary>
        /// Presentador checklist Pistola Neumática.
        /// </summary>
        public ucPuntosVerificacionPistolaNeumaticaPSLPRE PresentadorPistolaNeumatica {
            get { return presentadorPistolaNeumatica; }
            set { presentadorPistolaNeumatica = value; }
        }

        private ucPuntosVerificacionSubArrendadoPSLPRE presentadorSubArrendados;
        /// <summary>
        /// Presentador checklist genérico para equipos sub arrendados.
        /// </summary>
        public ucPuntosVerificacionSubArrendadoPSLPRE PresentadorSubArrendados {
            get { return presentadorSubArrendados; }
            set { presentadorSubArrendados = value; }
        }

        private ucPuntosVerificacionPlataformaTijerasPSLPRE presentadorPlataformaTijeras;

        /// <summary>
        /// Presentador checklist CompresorPortatil.
        /// </summary>
        public ucPuntosVerificacionPlataformaTijerasPSLPRE PresentadorPlataformaTijeras {
            get { return presentadorPlataformaTijeras; }
            set { presentadorPlataformaTijeras = value; }
        }

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor del presentador
        /// </summary>
        /// /// <param name="vista">Vista del User Control de Entrega PSL</param>
        /// <param name="viewDocumentos">Vista del User Control de Documentos</param>
        /// <param name="viewEquiposAliado">Vista del User Control de Equipos Aliados </param>
        public RegistrarEntregaPSLPRE(IRegistrarEntregaPSLVIS vista) {
            try {
                if (ReferenceEquals(vista, null))
                    throw new Exception(String.Format("{0}: La vista proporcionada no puede ser nula", nombreClase));

                this.vista = vista;
                this.controlador = new ContratoPSLBR();
                this.dctx = FacadeBR.ObtenerConexion();

            } catch (Exception ex) {
                this.vista.MostrarMensaje("Inconsistencia en los parámetros de configuración", ETipoMensajeIU.ERROR, nombreClase + ".RegistrarEntregaPSLPRE" + Environment.NewLine + ex.Message);
            }
        }

        #endregion

        #region Métodos
        /// <summary>
        /// Prepara la vista para el registro de entrega de la unidad
        /// </summary>
        public void PrepararNuevo() {
            this.LimpiarCampos();
            this.vista.PrepararNuevo();
            this.EstablecerInformacionInicial();
            this.vista.EstablecerPuntosVerificacionCheckList();
            this.vista.LimpiarPaqueteNavegacion();
            this.presentadorDocumentos.ModoEditable(true);
            this.presentadorEquipoAliados.Inicializar();
            this.presentadorEntregaRecepcion.PrepararNuevo();
            this.presentadorRetroExcavadora.PrepararNuevo();
            this.EstablecerSeguridad();
            this.EstablecerAcciones();
        }

        /// <summary>
        /// Limpia los campos de la vista
        /// </summary>
        private void LimpiarCampos() {
            this.vista.ContratoID = null;
            this.vista.EquipoID = null;
            this.vista.EstatusContratoID = null;
            this.vista.FechaContrato = null;
            this.vista.NombreUsuarioEntrega = string.Empty;
            this.vista.PlacasEstatales = string.Empty;
            this.vista.NumeroContrato = string.Empty;
            this.vista.NumeroEconomico = string.Empty;
            this.vista.NumeroSerie = string.Empty;
            this.vista.HoraContrato = null;
            this.vista.LineaContratoID = null;
            this.vista.Observaciones = string.Empty;
            this.vista.NumeroLicencia = string.Empty;
        }

        /// <summary>
        /// Establece en la vista la información inicial para el registro
        /// </summary>
        public void EstablecerInformacionInicial() {
            try {
                //Se obtiene el contrato el cual se le hará el check list
                ContratoPSLBO cto = null;
                object objContrato = this.vista.ObtenerPaqueteContrato();
                if (objContrato is ContratoPSLBO)
                    cto = (ContratoPSLBO)objContrato;
                else if (objContrato is int)
                    cto = new ContratoPSLBO() { ContratoID = (int)objContrato };

                int? linea = (int)this.vista.ObtenerPaqueteLineaContrato();

                //Valida si existe la información del contrato
                if (cto == null)
                    throw new Exception("No es posible recuperar la información del contrato necesario para el registro del check list. Intente de nuevo por favor.");

                this.vista.ContratoID = cto.ContratoID;
                this.vista.LineaContratoID = linea;

                //Se establecen los tipos de archivos que serán permitidos para adjuntar al contrato
                List<TipoArchivoBO> lstTipoArchivo = new TipoArchivoBR().Consultar(this.dctx, new TipoArchivoBO());
                var o = new object();
                this.presentadorDocumentos.Vista.Identificador = o.GetHashCode().ToString();
                this.presentadorDocumentos.EstablecerTiposArchivo(lstTipoArchivo);

                #region Consultar Contrato
                if (cto.FC.HasValue) {
                    this.vista.UltimoObjeto = cto;
                    this.DesplegarInformacionGeneral(cto);
                } else {
                    ContratoPSLBR contratoBR = new ContratoPSLBR();
                    List<ContratoPSLBO> lstContratos = contratoBR.ConsultarCompleto(this.dctx, cto, false);

                    if (ReferenceEquals(lstContratos, null))
                        throw new Exception("No es posible recuperar la información del contrato necesaria para el registro del check list. Intente de nuevo por favor.");
                    if (lstContratos.Count() <= 0)
                        throw new Exception("No es posible recuperar la información del contrato necesaria para el registro del check list. Intente de nuevo por favor.");

                    //Se desplega la información general del contrato
                    this.DesplegarInformacionGeneral(lstContratos[0]);
                    this.vista.UltimoObjeto = lstContratos[0];
                }
                #endregion /Consultar Contrato

                #region Información del usuario
                CatalogoBaseBO ubase = new UsuarioBO { Activo = true, Id = this.vista.UsuarioID };
                List<UsuarioBO> usuarios = FacadeBR.ConsultarUsuario(this.dctx, ubase);
                if (!ReferenceEquals(usuarios, null)) {
                    if (usuarios.Count > 0) {
                        this.vista.NombreUsuarioEntrega = !string.IsNullOrEmpty(usuarios[0].Nombre) && !string.IsNullOrWhiteSpace(usuarios[0].Nombre)
                            ? usuarios[0].Nombre.Trim().ToUpper() : string.Empty;
                    }
                }
                #endregion

                #region Información check list
                this.vista.HoraContrato = DateTime.Now.TimeOfDay;
                this.vista.TipoListado = (int)ETipoListadoVerificacion.ENTREGA;
                #endregion
            } catch (Exception ex) {
                throw new Exception(nombreClase + ".EstablecerInformacionInicial: " + ex.Message);
            }
        }

        /// <summary>
        /// Despliega en la vista las unidades seleccionada en el contrato
        /// </summary>
        /// <param name="contratoPSLBO">Unidad la cual se desplegará</param>
        public void DesplegarInformacionGeneral(ContratoPSLBO contratoPSLBO) {
            if (ReferenceEquals(contratoPSLBO, null))
                throw new Exception("No es posible recuperar la información del contrato necesaria para el registro del check list. Intente de nuevo por favor.");

            #region Información del contrato
            this.vista.ContratoID = contratoPSLBO.ContratoID ?? null;
            this.vista.FechaContrato = contratoPSLBO.FechaContrato ?? null;
            this.vista.HoraContrato = contratoPSLBO.FechaContrato.HasValue
                                          ? (TimeSpan?)contratoPSLBO.FechaContrato.Value.TimeOfDay
                                          : null;
            this.vista.EstatusContratoID = contratoPSLBO.Estatus.HasValue ? (int?)contratoPSLBO.Estatus : null;
            this.vista.NumeroContrato = !string.IsNullOrEmpty(contratoPSLBO.NumeroContrato) && !string.IsNullOrWhiteSpace(contratoPSLBO.NumeroContrato)
                                            ? contratoPSLBO.NumeroContrato.Trim().ToUpper()
                                            : string.Empty;
            vista.FechaListado = DateTime.Now;
            vista.HoraContrato = DateTime.Now.TimeOfDay;
            vista.TipoContrato = (int)contratoPSLBO.Tipo;
            #endregion

            #region Información Cliente
            if (!ReferenceEquals(contratoPSLBO.Cliente, null)) {
                this.vista.NombreCliente = !string.IsNullOrEmpty(contratoPSLBO.Cliente.Nombre) && !string.IsNullOrWhiteSpace(contratoPSLBO.Cliente.Nombre)
                                           ? contratoPSLBO.Cliente.Nombre.Trim().ToUpper()
                                           : string.Empty;
            }
            #endregion

            if (ReferenceEquals(contratoPSLBO.LineasContrato, null))
                throw new Exception("No es posible recuperar la información de la unidad necesaria para el registro del check list. Intente de nuevo por favor.");
            if (contratoPSLBO.LineasContrato.Count <= 0)
                throw new Exception("No es posible recuperar la información de la unidad necesaria para el registro del check list. Intente de nuevo por favor.");

            int? linea = (int)this.vista.ObtenerPaqueteLineaContrato();
            this.vista.LineaContratoID = contratoPSLBO.LineasContrato.FirstOrDefault(x => x.LineaContratoID == linea).LineaContratoID;
            this.DesplegarInformacionUnidad(contratoPSLBO.LineasContrato.FirstOrDefault(x => x.LineaContratoID == linea).Equipo as UnidadBO);
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
            bof.Unidad.Sucursal = new SucursalBO { UnidadOperativa = new UnidadOperativaBO { Id = this.vista.UnidadOperativaID } };

            SeguimientoFlotaBR flotaBR = new SeguimientoFlotaBR();
            FlotaBO flota = new FlotaBO();
            flota = flotaBR.ConsultarFlotaPSL(dctx, bof);

            if (ReferenceEquals(flota, null))
                throw new Exception("No es posible recuperar la información de la unidad necesaria para el registro del check list. Intente de nuevo por favor.");

            if (ReferenceEquals(flota.ElementosFlota, null))
                throw new Exception("No es posible recuperar la información de la unidad necesaria para el registro del check list. Intente de nuevo por favor.");

            if (flota.ElementosFlota.Count <= 0)
                throw new Exception("No es posible recuperar la información de la unidad necesaria para el registro del check list. Intente de nuevo por favor.");

            ElementoFlotaBO elemento = flota.ElementosFlota[0];
            #region Unidad
            if (!ReferenceEquals(elemento.Unidad, null)) {
                DesplegarUltimosCombustibleHrs(ObtenerUltimosCombustibleHrs(elemento.Unidad.UnidadID, ETipoListadoVerificacion.RECEPCION));

                this.vista.UnidadID = elemento.Unidad.UnidadID.HasValue ? elemento.Unidad.UnidadID : null;
                this.vista.EquipoID = elemento.Unidad.EquipoID.HasValue ? elemento.Unidad.EquipoID : null;
                this.vista.NumeroEconomico = !string.IsNullOrEmpty(elemento.Unidad.NumeroEconomico) && !string.IsNullOrWhiteSpace(elemento.Unidad.NumeroEconomico)
                                                 ? elemento.Unidad.NumeroEconomico.Trim().ToUpper()
                                                 : string.Empty;

                this.vista.NumeroSerie = !string.IsNullOrEmpty(elemento.Unidad.NumeroSerie) && !string.IsNullOrWhiteSpace(elemento.Unidad.NumeroSerie)
                                             ? elemento.Unidad.NumeroSerie.Trim().ToUpper()
                                             : string.Empty;
                this.vista.Area = (EArea)elemento.Unidad.Area;
                //Aquí se obtendrá el tipo de check list que se mostrará
                switch (this.vista.UnidadOperativaID) {
                    case (int)ETipoEmpresa.Construccion:
                        if (((EAreaConstruccion)elemento.Unidad.Area) == EAreaConstruccion.RO || ((EAreaConstruccion)elemento.Unidad.Area) == EAreaConstruccion.ROC) {
                            this.vista.TipoListadoVerificacionPSL = (int)controlador.ObtenerTipoUnidadPorClave(this.dctx, (unidadBO).TipoEquipoServicio.NombreCorto, unidadBO);
                        }
                        if (((EAreaConstruccion)elemento.Unidad.Area) == EAreaConstruccion.RE) {
                            this.vista.TipoListadoVerificacionPSL = (int)ETipoUnidad.LV_SUBARRENDADO;
                        }
                        break;
                    case (int)ETipoEmpresa.Generacion:
                    case (int)ETipoEmpresa.Equinova:
                        this.vista.TipoListadoVerificacionPSL = (int)ETipoUnidad.LV_ENTREGA_RECEPCION;
                        break;
                    default:
                        throw new Exception("No es posible recuperar la información de la unidad necesaria para el registro del check list. Intente de nuevo por favor.");

                }


                #region Placas
                TramiteBO federal = elemento.ObtenerTramitePorTipo(ETipoTramite.PLACA_FEDERAL);
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
            this.presentadorEquipoAliados.DatoAInterfazUsuario(elemento);
            this.presentadorEquipoAliados.CargarEquiposAliados();
            #endregion

            #endregion
        }

        public void CancelarRegistro() {
            this.vista.LimpiarSesion();
            this.vista.RedirigirAConsulta();
        }

        public void RegistrarEntrega() {
            try {
                #region Validar campos
                string datosFaltantes;

                if (this.FaltanDatosObligatorios(out datosFaltantes)) {
                    this.vista.MostrarMensaje(string.Format("Es necesario capturar los siguientes datos: \n {0}", datosFaltantes), ETipoMensajeIU.ADVERTENCIA, null);
                    return;
                }

                string mensajeDatosFaltantes = "Es necesario capturar observaciones para los siguientes puntos de verificación: \n {0}";
                string s = "";
                switch (this.vista.UnidadOperativaID) {
                    case (int)ETipoEmpresa.Construccion:
                        if ((EAreaConstruccion)this.vista.Area == EAreaConstruccion.RO || (EAreaConstruccion)this.vista.Area == EAreaConstruccion.ROC) {
                            switch ((ETipoUnidad)Enum.Parse(typeof(ETipoUnidad), this.vista.TipoListadoVerificacionPSL.ToString())) {
                                case ETipoUnidad.LV_EXCAVADORA:
                                    if ((s = this.presentadorExcavadora.ValidarCampos()) != null) {
                                        if (string.IsNullOrEmpty(this.vista.Observaciones)) {
                                            this.vista.MostrarMensaje(s, ETipoMensajeIU.ADVERTENCIA, null);
                                            return;
                                        }
                                    }
                                    break;
                                case ETipoUnidad.LV_MONTACARGA:
                                    if ((s = this.presentadorMontaCarga.ValidarCampos()) != null) {
                                        if (string.IsNullOrEmpty(this.vista.Observaciones)) {
                                            this.vista.MostrarMensaje(s, ETipoMensajeIU.ADVERTENCIA, null);
                                            return;
                                        }
                                    }
                                    break;

                                case ETipoUnidad.LV_MINICARGADOR:
                                    if ((s = this.presentadorMiniCargador.ValidarCampos()) != null) {
                                        if (string.IsNullOrEmpty(this.vista.Observaciones)) {
                                            this.vista.MostrarMensaje(s, ETipoMensajeIU.ADVERTENCIA, null);
                                            return;
                                        }
                                    }
                                    break;



                                case ETipoUnidad.LV_MOTONIVELADORA:
                                    if ((s = this.presentadorMotoNiveladora.ValidarCampos()) != null) {
                                        if (string.IsNullOrEmpty(this.vista.Observaciones)) {
                                            this.vista.MostrarMensaje(s, ETipoMensajeIU.ADVERTENCIA, null);
                                            return;
                                        }
                                    }
                                    break;
                                case ETipoUnidad.LV_PISTOLA_NEUMATICA:
                                    if ((s = this.presentadorPistolaNeumatica.ValidarCampos()) != null) {
                                        if (string.IsNullOrEmpty(this.vista.Observaciones)) {
                                            this.vista.MostrarMensaje(s, ETipoMensajeIU.ADVERTENCIA, null);
                                            return;
                                        }
                                    }
                                    break;

                                case ETipoUnidad.LV_RETRO_EXCAVADORA:
                                    string datosFaltantesRetroExcavadora;
                                    if (this.presentadorRetroExcavadora.FaltanDatosObligatorios(out datosFaltantesRetroExcavadora) && string.IsNullOrEmpty(this.vista.Observaciones)) {
                                        this.vista.MostrarMensaje(string.Format(mensajeDatosFaltantes, datosFaltantesRetroExcavadora), ETipoMensajeIU.ADVERTENCIA, null);
                                        return;
                                    }
                                    break;

                                case ETipoUnidad.LV_COMPRESOR:
                                    if ((s = this.presentadorCompresorPortatil.ValidarCampos()) != null && string.IsNullOrEmpty(this.vista.Observaciones)) {
                                        this.vista.MostrarMensaje(s, ETipoMensajeIU.ADVERTENCIA, null);
                                        return;
                                    }
                                    break;

                                case ETipoUnidad.LV_MARTILLO_HIDRAULICO:
                                    if ((s = this.presentadorMartilloHidraulico.ValidarCampos()) != null) {
                                        if (string.IsNullOrEmpty(this.vista.Observaciones)) {
                                            this.vista.MostrarMensaje(s, ETipoMensajeIU.ADVERTENCIA, null);
                                            return;
                                        }
                                    }
                                    break;
                                case ETipoUnidad.LV_TORRES_LUZ:
                                    if ((s = this.presentadorTorresLuz.ValidarCampos()) != null) {
                                        if (string.IsNullOrEmpty(this.vista.Observaciones)) {
                                            this.vista.MostrarMensaje(s, ETipoMensajeIU.ADVERTENCIA, null);
                                            return;
                                        }
                                    }
                                    break;
                                case ETipoUnidad.LV_VIBRO_COMPACTADOR:
                                    if ((s = this.presentadorVibroCompactador.ValidarCampos()) != null) {
                                        if (string.IsNullOrEmpty(this.vista.Observaciones)) {
                                            this.vista.MostrarMensaje(s, ETipoMensajeIU.ADVERTENCIA, null);
                                            return;
                                        }
                                    }
                                    break;

                                case ETipoUnidad.LV_PLATAFORMA_TIJERAS:
                                    if ((s = this.presentadorPlataformaTijeras.ValidarCampos()) != null && string.IsNullOrEmpty(this.vista.Observaciones)) {
                                        this.vista.MostrarMensaje(s, ETipoMensajeIU.ADVERTENCIA, null);
                                        return;
                                    }
                                    break;
                                case ETipoUnidad.LV_SUBARRENDADO:
                                    if ((s = this.presentadorSubArrendados.ValidarCampos()) != null && string.IsNullOrEmpty(this.vista.Observaciones)) {
                                        this.vista.MostrarMensaje(s, ETipoMensajeIU.ADVERTENCIA, null);
                                        return;
                                    }
                                    break;
                            }
                        }

                        //Equipos sub arrendados
                        if ((EAreaConstruccion)this.vista.Area == EAreaConstruccion.RE) {
                            if ((s = this.presentadorSubArrendados.ValidarCampos()) != null) {
                                if (string.IsNullOrEmpty(this.vista.Observaciones)) {
                                    this.vista.MostrarMensaje(s, ETipoMensajeIU.ADVERTENCIA, null);
                                    return;
                                }
                            }
                        }
                        break;
                    case (int)ETipoEmpresa.Generacion:
                    case (int)ETipoEmpresa.Equinova:
                        if ((s = this.presentadorEntregaRecepcion.ValidarCampos()) != null && string.IsNullOrEmpty(this.vista.Observaciones)) {
                            this.vista.MostrarMensaje(s, ETipoMensajeIU.ADVERTENCIA, null);
                            return;
                        }
                        break;
                }
                #endregion

                //Asignamos el tipo de listado
                this.vista.TipoListado = (int)ETipoListadoVerificacion.ENTREGA;
                this.Registrar();

                ContratoPSLBO ctoReporte = new ContratoPSLBO((ContratoPSLBO)this.vista.UltimoObjeto);

                Dictionary<string, object> datos = this.controlador.ObtenerDatosCheckList(
                    this.dctx, ctoReporte, (int)this.vista.ModuloID, ((ETipoUnidad)this.vista.TipoListadoVerificacionPSL), this.vista.LineaContratoID);

                //Limpiamos las variables de session
                this.vista.LimpiarSesion();
                this.vista.LimpiarPaqueteNavegacion();
                this.vista.LimpiarVariableIntercambio();
                this.vista.EstablecerPaqueteNavegacion("CheckListEntregaRO", datos);
                string error = string.Empty;
                this.vista.RedirigirAImprimir(error);
            } catch (Exception ex) {
                throw new Exception(nombreClase + ".RegistrarEntregaPSL: " + Environment.NewLine + ex.Message);
            }

        }

        #region Generar Reporte Adjunto
        /// <summary>
        /// Genera el array de bytes del reporte de checklist de Entrega
        /// </summary>
        /// <param name="datosReporte">Diccionario de datos con la información del reporte.</param>
        /// <param name="tipoReporte">Tipo de Reporte que se generará.</param>
        /// <returns></returns>
        private byte[] GenerarReporteAdjuntar(Dictionary<string, object> datosReporte, ETipoUnidad tipoReporte) {
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
                throw new Exception("Se encontraron inconsistencias al insertar el Contrato.");
            }
            #endregion

            try {
                //Se obtiene la información a partir de la Interfaz de usuario.
                ContratoPSLBO bo = (ContratoPSLBO)this.InterfazUsuarioADato();
                LineaContratoPSLBO lin = (LineaContratoPSLBO)bo.LineasContrato.FirstOrDefault(l => l.LineaContratoID == this.vista.LineaContratoID);

                UsuarioBO usuario = new UsuarioBO() { Id = this.vista.UsuarioID };
                AdscripcionBO adscripcion = new AdscripcionBO() { UnidadOperativa = new UnidadOperativaBO() { Id = this.vista.UnidadOperativaID } };
                SeguridadBO seguridadBO = new SeguridadBO(Guid.Empty, usuario, adscripcion);

                //Se inserta en la BD
                bo = this.controlador.RegistrarEntrega(this.dctx, bo, lin, seguridadBO, this.vista.EsIntercambio);
                if (bo.Estatus == EEstatusContrato.EnCurso) {
                    //Verificar que no se hayan creado previamente los pagos (compatibilidad hacia a tras con el proceso anterior)
                    ReferenciaContratoBR referenciaContratoBR = new ReferenciaContratoBR();
                    List<ReferenciaContratoBO> referencias = referenciaContratoBR.Consultar(this.dctx, new ReferenciaContratoBO { ReferenciaContratoID = bo.ContratoID });

                    if (referencias.Count == 0) {
                        GeneradorPagoPSLBR generadorPago = new GeneradorPagoPSLBR();
                        generadorPago.GenerarPagos(this.dctx, bo, seguridadBO, true);
                    }
                }

                this.vista.UltimoObjeto = bo;

                this.dctx.SetCurrentProvider("Outsourcing");
                this.dctx.CommitTransaction(firma);
            } catch (Exception ex) {
                this.dctx.SetCurrentProvider("Outsourcing");
                this.dctx.RollbackTransaction(firma);

                throw new Exception(nombreClase + ".Registrar:" + ex.Message);
            } finally {
                if (this.dctx.ConnectionState == ConnectionState.Open)
                    this.dctx.CloseConnection(firma);
            }

        }

        /// <summary>
        /// Mapea los datos de la vista al objeto del contrato
        /// </summary>
        /// <returns></returns>
        private object InterfazUsuarioADato() {
            ContratoPSLBO contrato = new ContratoPSLBO();
            if (this.vista.UltimoObjeto != null)
                contrato = new ContratoPSLBO((ContratoPSLBO)this.vista.UltimoObjeto);
            else {
                if (this.vista.ContratoID.HasValue) {
                    contrato.ContratoID = this.vista.ContratoID.Value;
                }
                if (this.vista.TipoContrato.HasValue)
                    contrato.Tipo = (ETipoContrato)this.vista.TipoContrato.Value;
                if (this.vista.UnidadOperativaID.HasValue) {
                    SucursalBO sucursal = new SucursalBO { UnidadOperativa = new UnidadOperativaBO { Id = this.vista.UnidadOperativaID } };
                    contrato.Sucursal = sucursal;
                }
            }

            // Se iniciializa el listado de Lineas
            if (ReferenceEquals(contrato.LineasContrato, null))
                contrato.LineasContrato = new List<ILineaContrato>();

            // Se obtiene la línea del contrato en sesión. Si no hay contrato o no se encuentra la línea, se crea una nueva
            LineaContratoPSLBO linea = (LineaContratoPSLBO)contrato.LineasContrato.FirstOrDefault(l => l.LineaContratoID == this.vista.LineaContratoID);
            if (linea == null) {
                linea = new LineaContratoPSLBO();

                if (this.vista.LineaContratoID.HasValue)
                    linea.LineaContratoID = this.vista.LineaContratoID.Value;

                linea.Equipo = this.InterfazUsuarioADatoUnidad();

                // Se agrega la línea al contrato
                contrato.AgregarLineaContrato(linea);
            }

            // Inicializar los listados de verificación
            if (ReferenceEquals(linea.ListadosVerificacion, null))
                linea.ListadosVerificacion = new List<AVerificacionLineaPSLBO>();

            switch (this.vista.UnidadOperativaID) {
                case (int)ETipoEmpresa.Construccion:
                    if ((EAreaConstruccion)this.vista.Area == EAreaConstruccion.RO || (EAreaConstruccion)this.vista.Area == EAreaConstruccion.ROC) {
                        switch ((ETipoUnidad)Enum.Parse(typeof(ETipoUnidad), this.vista.TipoListadoVerificacionPSL.Value.ToString())) {
                            case ETipoUnidad.LV_EXCAVADORA:
                                ListadoVerificacionExcavadoraBO checkExcavadora = new ListadoVerificacionExcavadoraBO();

                                checkExcavadora = (ListadoVerificacionExcavadoraBO)this.presentadorExcavadora.InterfazUsuarioADato();
                                checkExcavadora.LineaContratoPSLID = this.vista.LineaContratoID.Value;
                                if (this.vista.TipoListado.HasValue)
                                    checkExcavadora.Tipo = (ETipoListadoVerificacion)this.vista.TipoListado.Value;
                                if (this.vista.FechaListado.HasValue)
                                    checkExcavadora.Fecha = this.vista.FechaListado.Value;
                                if (this.vista.FechaListado.HasValue && this.vista.HoraListado.HasValue)
                                    checkExcavadora.Fecha = checkExcavadora.Fecha.Value.Add(this.vista.HoraListado.Value);
                                if (this.vista.HorometroSalida.HasValue)
                                    checkExcavadora.Horometro = this.vista.HorometroSalida.Value;
                                if (this.vista.Combustible.HasValue)
                                    checkExcavadora.Combustible = this.vista.Combustible.Value;

                                if (!string.IsNullOrEmpty(this.vista.Observaciones))
                                    checkExcavadora.Observaciones = this.vista.Observaciones;

                                checkExcavadora.Auditoria = new AuditoriaBO {
                                    FC = DateTime.Now,
                                    FUA = DateTime.Now,
                                    UC = this.vista.UsuarioID,
                                    UUA = this.vista.UsuarioID
                                };

                                //agregamos los archivos adjuntos al listado
                                checkExcavadora.Adjuntos = this.presentadorDocumentos.Vista.NuevosArchivos;

                                if (!checkExcavadora.Tipo.HasValue)
                                    checkExcavadora.Tipo = ETipoListadoVerificacion.ENTREGA;

                                linea.AgregarListadoVerificacion(checkExcavadora);
                                break;

                            case ETipoUnidad.LV_RETRO_EXCAVADORA:
                                ListadoVerificacionRetroExcavadoraBO checkRetroExcavadora = this.presentadorRetroExcavadora.InterfazUsuarioADato();
                                checkRetroExcavadora.LineaContratoPSLID = this.vista.LineaContratoID.Value;
                                checkRetroExcavadora.Tipo = (ETipoListadoVerificacion)this.vista.TipoListado.Value;
                                if (this.vista.FechaListado.HasValue)
                                    checkRetroExcavadora.Fecha = this.vista.FechaListado.Value;
                                if (this.vista.FechaListado.HasValue && this.vista.HoraListado.HasValue)
                                    checkRetroExcavadora.Fecha = checkRetroExcavadora.Fecha.Value.Add(this.vista.HoraListado.Value);
                                checkRetroExcavadora.Horometro = this.vista.HorometroSalida;
                                checkRetroExcavadora.Combustible = this.vista.Combustible;
                                if (!string.IsNullOrEmpty(this.vista.Observaciones))
                                    checkRetroExcavadora.Observaciones = this.vista.Observaciones;

                                checkRetroExcavadora.Auditoria = new AuditoriaBO {
                                    FC = DateTime.Now,
                                    FUA = DateTime.Now,
                                    UC = this.vista.UsuarioID,
                                    UUA = this.vista.UsuarioID
                                };

                                //agregamos los archivos adjuntos al listado
                                checkRetroExcavadora.Adjuntos = this.presentadorDocumentos.Vista.NuevosArchivos;

                                linea.AgregarListadoVerificacion(checkRetroExcavadora);
                                break;

                            case ETipoUnidad.LV_COMPRESOR:
                                ListadoVerificacionCompresorPortatilBO checkCompresorPortatil = (ListadoVerificacionCompresorPortatilBO)this.presentadorCompresorPortatil.InterfazUsuarioADato();
                                checkCompresorPortatil.LineaContratoPSLID = this.vista.LineaContratoID.Value;
                                checkCompresorPortatil.Tipo = (ETipoListadoVerificacion)this.vista.TipoListado.Value;
                                if (this.vista.FechaListado.HasValue)
                                    checkCompresorPortatil.Fecha = this.vista.FechaListado.Value;
                                if (this.vista.FechaListado.HasValue && this.vista.HoraListado.HasValue)
                                    checkCompresorPortatil.Fecha = checkCompresorPortatil.Fecha.Value.Add(this.vista.HoraListado.Value);
                                checkCompresorPortatil.Horometro = this.vista.HorometroSalida;
                                checkCompresorPortatil.Combustible = this.vista.Combustible;
                                if (!string.IsNullOrEmpty(this.vista.Observaciones))
                                    checkCompresorPortatil.Observaciones = this.vista.Observaciones;
                                checkCompresorPortatil.Auditoria = new AuditoriaBO {
                                    FC = DateTime.Now,
                                    FUA = DateTime.Now,
                                    UC = this.vista.UsuarioID,
                                    UUA = this.vista.UsuarioID
                                };

                                //agregamos los archivos adjuntos al listado
                                checkCompresorPortatil.Adjuntos = this.presentadorDocumentos.Vista.NuevosArchivos;

                                linea.AgregarListadoVerificacion(checkCompresorPortatil);
                                break;

                            case ETipoUnidad.LV_MONTACARGA:
                                ListadoVerificacionMontaCargaBO checkMontaCarga = (ListadoVerificacionMontaCargaBO)this.presentadorMontaCarga.InterfazUsuarioADato();
                                checkMontaCarga.LineaContratoPSLID = this.vista.LineaContratoID.Value;
                                checkMontaCarga.Tipo = (ETipoListadoVerificacion)this.vista.TipoListado.Value;
                                if (this.vista.FechaListado.HasValue)
                                    checkMontaCarga.Fecha = this.vista.FechaListado.Value;
                                if (this.vista.FechaListado.HasValue && this.vista.HoraListado.HasValue)
                                    checkMontaCarga.Fecha = checkMontaCarga.Fecha.Value.Add(this.vista.HoraListado.Value);
                                checkMontaCarga.Horometro = this.vista.HorometroSalida;
                                checkMontaCarga.Combustible = this.vista.Combustible;
                                if (!string.IsNullOrEmpty(this.vista.Observaciones))
                                    checkMontaCarga.Observaciones = this.vista.Observaciones;

                                checkMontaCarga.Auditoria = new AuditoriaBO {
                                    FC = DateTime.Now,
                                    FUA = DateTime.Now,
                                    UC = this.vista.UsuarioID,
                                    UUA = this.vista.UsuarioID
                                };

                                //agregamos los archivos adjuntos al listado
                                checkMontaCarga.Adjuntos = this.presentadorDocumentos.Vista.NuevosArchivos;

                                linea.AgregarListadoVerificacion(checkMontaCarga);
                                break;

                            case ETipoUnidad.LV_MINICARGADOR:
                                ListadoVerificacionMiniCargadorBO checkMiniCargador = (ListadoVerificacionMiniCargadorBO)this.PresentadorMiniCargador.InterfazUsuarioADato();
                                checkMiniCargador.LineaContratoPSLID = this.vista.LineaContratoID.Value;
                                checkMiniCargador.Tipo = (ETipoListadoVerificacion)this.vista.TipoListado.Value;
                                if (this.vista.FechaListado.HasValue)
                                    checkMiniCargador.Fecha = this.vista.FechaListado.Value;
                                if (this.vista.FechaListado.HasValue && this.vista.HoraListado.HasValue)
                                    checkMiniCargador.Fecha = checkMiniCargador.Fecha.Value.Add(this.vista.HoraListado.Value);
                                checkMiniCargador.Horometro = this.vista.HorometroSalida;
                                checkMiniCargador.Combustible = this.vista.Combustible;
                                if (!string.IsNullOrEmpty(this.vista.Observaciones))
                                    checkMiniCargador.Observaciones = this.vista.Observaciones;

                                checkMiniCargador.Auditoria = new AuditoriaBO {
                                    FC = DateTime.Now,
                                    FUA = DateTime.Now,
                                    UC = this.vista.UsuarioID,
                                    UUA = this.vista.UsuarioID
                                };
                                //agregamos los archivos adjuntos al listado
                                checkMiniCargador.Adjuntos = this.presentadorDocumentos.Vista.NuevosArchivos;

                                linea.AgregarListadoVerificacion(checkMiniCargador);
                                break;

                            case ETipoUnidad.LV_MOTONIVELADORA:
                                ListadoVerificacionMotoNiveladoraBO checkMotoNiveladora = (ListadoVerificacionMotoNiveladoraBO)this.presentadorMotoNiveladora.InterfazUsuarioADato();
                                checkMotoNiveladora.LineaContratoPSLID = this.vista.LineaContratoID.Value;
                                checkMotoNiveladora.Tipo = (ETipoListadoVerificacion)this.vista.TipoListado.Value;
                                if (this.vista.FechaListado.HasValue)
                                    checkMotoNiveladora.Fecha = this.vista.FechaListado.Value;
                                if (this.vista.FechaListado.HasValue && this.vista.HoraListado.HasValue)
                                    checkMotoNiveladora.Fecha = checkMotoNiveladora.Fecha.Value.Add(this.vista.HoraListado.Value);
                                checkMotoNiveladora.Horometro = this.vista.HorometroSalida;
                                checkMotoNiveladora.Combustible = this.vista.Combustible;
                                if (!string.IsNullOrEmpty(this.vista.Observaciones))
                                    checkMotoNiveladora.Observaciones = this.vista.Observaciones;

                                checkMotoNiveladora.Auditoria = new AuditoriaBO {
                                    FC = DateTime.Now,
                                    FUA = DateTime.Now,
                                    UC = this.vista.UsuarioID,
                                    UUA = this.vista.UsuarioID
                                };

                                //agregamos los archivos adjuntos al listado
                                checkMotoNiveladora.Adjuntos = this.presentadorDocumentos.Vista.NuevosArchivos;

                                linea.AgregarListadoVerificacion(checkMotoNiveladora);
                                break;
                            case ETipoUnidad.LV_PISTOLA_NEUMATICA:
                                ListadoVerificacionPistolaNeumaticaBO checkPistolaNeumatica = (ListadoVerificacionPistolaNeumaticaBO)this.presentadorPistolaNeumatica.InterfazUsuarioADato();
                                checkPistolaNeumatica.LineaContratoPSLID = this.vista.LineaContratoID.Value;
                                checkPistolaNeumatica.Tipo = (ETipoListadoVerificacion)this.vista.TipoListado.Value;
                                if (this.vista.FechaListado.HasValue)
                                    checkPistolaNeumatica.Fecha = this.vista.FechaListado.Value;
                                if (this.vista.FechaListado.HasValue && this.vista.HoraListado.HasValue)
                                    checkPistolaNeumatica.Fecha = checkPistolaNeumatica.Fecha.Value.Add(this.vista.HoraListado.Value);
                                checkPistolaNeumatica.Horometro = this.vista.HorometroSalida;
                                checkPistolaNeumatica.Combustible = this.vista.Combustible;
                                if (!string.IsNullOrEmpty(this.vista.Observaciones))
                                    checkPistolaNeumatica.Observaciones = this.vista.Observaciones;

                                checkPistolaNeumatica.Auditoria = new AuditoriaBO {
                                    FC = DateTime.Now,
                                    FUA = DateTime.Now,
                                    UC = this.vista.UsuarioID,
                                    UUA = this.vista.UsuarioID
                                };

                                //agregamos los archivos adjuntos al listado
                                checkPistolaNeumatica.Adjuntos = this.presentadorDocumentos.Vista.NuevosArchivos;

                                linea.AgregarListadoVerificacion(checkPistolaNeumatica);
                                break;


                            case ETipoUnidad.LV_MARTILLO_HIDRAULICO:
                                ListadoVerificacionMartilloHidraulicoBO checkMartilloHidraulico = (ListadoVerificacionMartilloHidraulicoBO)this.presentadorMartilloHidraulico.InterfazUsuarioADato();
                                checkMartilloHidraulico.LineaContratoPSLID = this.vista.LineaContratoID.Value;
                                checkMartilloHidraulico.Tipo = (ETipoListadoVerificacion)this.vista.TipoListado.Value;
                                if (this.vista.FechaListado.HasValue)
                                    checkMartilloHidraulico.Fecha = this.vista.FechaListado.Value;
                                if (this.vista.FechaListado.HasValue && this.vista.HoraListado.HasValue)
                                    checkMartilloHidraulico.Fecha = checkMartilloHidraulico.Fecha.Value.Add(this.vista.HoraListado.Value);
                                checkMartilloHidraulico.Horometro = this.vista.HorometroSalida;
                                checkMartilloHidraulico.Combustible = this.vista.Combustible;
                                if (!string.IsNullOrEmpty(this.vista.Observaciones))
                                    checkMartilloHidraulico.Observaciones = this.vista.Observaciones;

                                checkMartilloHidraulico.Auditoria = new AuditoriaBO {
                                    FC = DateTime.Now,
                                    FUA = DateTime.Now,
                                    UC = this.vista.UsuarioID,
                                    UUA = this.vista.UsuarioID
                                };

                                //agregamos los archivos adjuntos al listado
                                checkMartilloHidraulico.Adjuntos = this.presentadorDocumentos.Vista.NuevosArchivos;

                                linea.AgregarListadoVerificacion(checkMartilloHidraulico);
                                break;

                            case ETipoUnidad.LV_TORRES_LUZ:
                                ListadoVerificacionTorresLuzBO checkTorresLuz = (ListadoVerificacionTorresLuzBO)this.presentadorTorresLuz.InterfazUsuarioADato();
                                checkTorresLuz.LineaContratoPSLID = this.vista.LineaContratoID.Value;
                                checkTorresLuz.Tipo = (ETipoListadoVerificacion)this.vista.TipoListado.Value;
                                if (this.vista.FechaListado.HasValue)
                                    checkTorresLuz.Fecha = this.vista.FechaListado.Value;
                                if (this.vista.FechaListado.HasValue && this.vista.HoraListado.HasValue)
                                    checkTorresLuz.Fecha = checkTorresLuz.Fecha.Value.Add(this.vista.HoraListado.Value);
                                checkTorresLuz.Horometro = this.vista.HorometroSalida;
                                checkTorresLuz.Combustible = this.vista.Combustible;
                                if (!string.IsNullOrEmpty(this.vista.Observaciones))
                                    checkTorresLuz.Observaciones = this.vista.Observaciones;

                                checkTorresLuz.Auditoria = new AuditoriaBO {
                                    FC = DateTime.Now,
                                    FUA = DateTime.Now,
                                    UC = this.vista.UsuarioID,
                                    UUA = this.vista.UsuarioID
                                };

                                //agregamos los archivos adjuntos al listado
                                checkTorresLuz.Adjuntos = this.presentadorDocumentos.Vista.NuevosArchivos;

                                linea.AgregarListadoVerificacion(checkTorresLuz);
                                break;

                            case ETipoUnidad.LV_VIBRO_COMPACTADOR:
                                ListadoVerificacionVibroCompactadorBO checkVibroCompactador = (ListadoVerificacionVibroCompactadorBO)this.presentadorVibroCompactador.InterfazUsuarioADato();
                                checkVibroCompactador.LineaContratoPSLID = this.vista.LineaContratoID.Value;
                                checkVibroCompactador.Tipo = (ETipoListadoVerificacion)this.vista.TipoListado.Value;
                                if (this.vista.FechaListado.HasValue)
                                    checkVibroCompactador.Fecha = this.vista.FechaListado.Value;
                                if (this.vista.FechaListado.HasValue && this.vista.HoraListado.HasValue)
                                    checkVibroCompactador.Fecha = checkVibroCompactador.Fecha.Value.Add(this.vista.HoraListado.Value);
                                checkVibroCompactador.Horometro = this.vista.HorometroSalida;
                                checkVibroCompactador.Combustible = this.vista.Combustible;
                                if (!string.IsNullOrEmpty(this.vista.Observaciones))
                                    checkVibroCompactador.Observaciones = this.vista.Observaciones;

                                checkVibroCompactador.Auditoria = new AuditoriaBO {
                                    FC = DateTime.Now,
                                    FUA = DateTime.Now,
                                    UC = this.vista.UsuarioID,
                                    UUA = this.vista.UsuarioID
                                };

                                //agregamos los archivos adjuntos al listado
                                checkVibroCompactador.Adjuntos = this.presentadorDocumentos.Vista.NuevosArchivos;

                                linea.AgregarListadoVerificacion(checkVibroCompactador);
                                break;

                            case ETipoUnidad.LV_PLATAFORMA_TIJERAS:
                                ListadoVerificacionPlataformaTijerasBO checkPlataformaTijeras = (ListadoVerificacionPlataformaTijerasBO)this.presentadorPlataformaTijeras.InterfazUsuarioADato();
                                checkPlataformaTijeras.LineaContratoPSLID = this.vista.LineaContratoID.Value;
                                checkPlataformaTijeras.Tipo = (ETipoListadoVerificacion)this.vista.TipoListado.Value;
                                if (this.vista.FechaListado.HasValue)
                                    checkPlataformaTijeras.Fecha = this.vista.FechaListado.Value;
                                if (this.vista.FechaListado.HasValue && this.vista.HoraListado.HasValue)
                                    checkPlataformaTijeras.Fecha = checkPlataformaTijeras.Fecha.Value.Add(this.vista.HoraListado.Value);
                                checkPlataformaTijeras.Horometro = this.vista.HorometroSalida;
                                checkPlataformaTijeras.Combustible = this.vista.Combustible;
                                if (!string.IsNullOrEmpty(this.vista.Observaciones))
                                    checkPlataformaTijeras.Observaciones = this.vista.Observaciones;
                                checkPlataformaTijeras.Auditoria = new AuditoriaBO {
                                    FC = DateTime.Now,
                                    FUA = DateTime.Now,
                                    UC = this.vista.UsuarioID,
                                    UUA = this.vista.UsuarioID
                                };

                                //agregamos los archivos adjuntos al listado
                                checkPlataformaTijeras.Adjuntos = this.presentadorDocumentos.Vista.NuevosArchivos;

                                linea.AgregarListadoVerificacion(checkPlataformaTijeras);
                                break;

                            case ETipoUnidad.LV_SUBARRENDADO:
                                ListadoVerificacionSubArrendadoBO checkSubArrendado = new ListadoVerificacionSubArrendadoBO();
                                checkSubArrendado.LineaContratoPSLID = this.vista.LineaContratoID.Value;
                                checkSubArrendado = (ListadoVerificacionSubArrendadoBO)this.presentadorSubArrendados.InterfazUsuarioADato();
                                if (this.vista.TipoListado.HasValue)
                                    checkSubArrendado.Tipo = (ETipoListadoVerificacion)this.vista.TipoListado.Value;
                                if (this.vista.FechaListado.HasValue)
                                    checkSubArrendado.Fecha = this.vista.FechaListado.Value;
                                if (this.vista.FechaListado.HasValue && this.vista.HoraListado.HasValue)
                                    checkSubArrendado.Fecha = checkSubArrendado.Fecha.Value.Add(this.vista.HoraListado.Value);
                                if (this.vista.HorometroSalida.HasValue)
                                    checkSubArrendado.Horometro = this.vista.HorometroSalida.Value;
                                if (!string.IsNullOrEmpty(this.vista.Observaciones))
                                    checkSubArrendado.Observaciones = this.vista.Observaciones;
                                if (this.vista.Combustible.HasValue)
                                    checkSubArrendado.Combustible = this.vista.Combustible.Value;

                                checkSubArrendado.Auditoria = new AuditoriaBO {
                                    FC = DateTime.Now,
                                    FUA = DateTime.Now,
                                    UC = this.vista.UsuarioID,
                                    UUA = this.vista.UsuarioID
                                };

                                //agregamos los archivos adjuntos al listado
                                checkSubArrendado.Adjuntos = this.presentadorDocumentos.Vista.NuevosArchivos;

                                linea.AgregarListadoVerificacion(checkSubArrendado);
                                break;
                        }
                    }
                    //Equipos sub arrendados
                    if ((EAreaConstruccion)this.vista.Area == EAreaConstruccion.RE) {
                        ListadoVerificacionSubArrendadoBO checkSubArrendadoRE = new ListadoVerificacionSubArrendadoBO();
                        checkSubArrendadoRE.LineaContratoPSLID = this.vista.LineaContratoID.Value;
                        checkSubArrendadoRE = (ListadoVerificacionSubArrendadoBO)this.presentadorSubArrendados.InterfazUsuarioADato();
                        if (this.vista.TipoListado.HasValue)
                            checkSubArrendadoRE.Tipo = (ETipoListadoVerificacion)this.vista.TipoListado.Value;
                        if (this.vista.FechaListado.HasValue)
                            checkSubArrendadoRE.Fecha = this.vista.FechaListado.Value;
                        if (this.vista.FechaListado.HasValue && this.vista.HoraListado.HasValue)
                            checkSubArrendadoRE.Fecha = checkSubArrendadoRE.Fecha.Value.Add(this.vista.HoraListado.Value);
                        if (this.vista.HorometroSalida.HasValue)
                            checkSubArrendadoRE.Horometro = this.vista.HorometroSalida.Value;
                        if (!string.IsNullOrEmpty(this.vista.Observaciones))
                            checkSubArrendadoRE.Observaciones = this.vista.Observaciones;
                        if (this.vista.Combustible.HasValue)
                            checkSubArrendadoRE.Combustible = this.vista.Combustible.Value;

                        checkSubArrendadoRE.Auditoria = new AuditoriaBO {
                            FC = DateTime.Now,
                            FUA = DateTime.Now,
                            UC = this.vista.UsuarioID,
                            UUA = this.vista.UsuarioID
                        };

                        //agregamos los archivos adjuntos al listado
                        checkSubArrendadoRE.Adjuntos = this.presentadorDocumentos.Vista.NuevosArchivos;

                        linea.AgregarListadoVerificacion(checkSubArrendadoRE);
                    }
                    break;
                case (int)ETipoEmpresa.Generacion:
                case (int)ETipoEmpresa.Equinova:
                    ListadoVerificacionEntregaRecepcionBO checkEntregaRecepcion = new ListadoVerificacionEntregaRecepcionBO();
                    checkEntregaRecepcion.LineaContratoPSLID = this.vista.LineaContratoID.Value;
                    checkEntregaRecepcion = (ListadoVerificacionEntregaRecepcionBO)this.presentadorEntregaRecepcion.InterfazUsuarioADato();
                    if (this.vista.TipoListado.HasValue)
                        checkEntregaRecepcion.Tipo = (ETipoListadoVerificacion)this.vista.TipoListado.Value;
                    if (this.vista.FechaListado.HasValue)
                        checkEntregaRecepcion.Fecha = this.vista.FechaListado.Value;
                    if (this.vista.FechaListado.HasValue && this.vista.HoraListado.HasValue)
                        checkEntregaRecepcion.Fecha = checkEntregaRecepcion.Fecha.Value.Add(this.vista.HoraListado.Value);
                    if (this.vista.HorometroSalida.HasValue)
                        checkEntregaRecepcion.Horometro = this.vista.HorometroSalida.Value;
                    if (!string.IsNullOrEmpty(this.vista.Observaciones))
                        checkEntregaRecepcion.Observaciones = this.vista.Observaciones;
                    if (this.vista.Combustible.HasValue)
                        checkEntregaRecepcion.Combustible = this.vista.Combustible.Value;

                    checkEntregaRecepcion.Auditoria = new AuditoriaBO {
                        FC = DateTime.Now,
                        FUA = DateTime.Now,
                        UC = this.vista.UsuarioID,
                        UUA = this.vista.UsuarioID
                    };

                    //agregamos los archivos adjuntos al listado
                    checkEntregaRecepcion.Adjuntos = this.presentadorDocumentos.Vista.NuevosArchivos;

                    if (!checkEntregaRecepcion.Tipo.HasValue)
                        checkEntregaRecepcion.Tipo = ETipoListadoVerificacion.ENTREGA;

                    linea.AgregarListadoVerificacion(checkEntregaRecepcion);
                    break;
            }

            return contrato;
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

        /// <summary>
        /// Valida la información proporcionada por el usuario
        /// </summary>
        /// <returns></returns>
        public bool FaltanDatosObligatorios(out string datosFaltantes) {
            datosFaltantes = "";

            datosFaltantes += this.ValidarFechasListado();
            datosFaltantes += this.ValidarTanque();

            if (!vista.LineaContratoID.HasValue)
                datosFaltantes += "Contrato, ";
            if (!vista.UsuarioID.HasValue)
                datosFaltantes += "Usuario que entrega la unidad, ";
            if (!vista.HorometroSalida.HasValue)
                datosFaltantes += "Horómetro, ";

            if (datosFaltantes.Length > 0) {
                datosFaltantes = datosFaltantes.Substring(0, datosFaltantes.Length - 2);
            }
            return !string.IsNullOrEmpty(datosFaltantes);
        }

        /// <summary>
        /// Valida si el usuario tiene permiso para registrar el check list
        /// </summary>
        public void EstablecerSeguridad() {
            try {
                //se valida que los datos del usuario y la unidad operativa no sean nulos
                if (this.vista.UsuarioID == null) throw new Exception("El identificador del usuario no debe ser nulo. ");
                if (this.vista.UnidadOperativaID == null) throw new Exception("El identificador de la Unidad Operativa no debe ser nulo. ");

                UsuarioBO usr = new UsuarioBO { Id = this.vista.UsuarioID };
                AdscripcionBO adscripcion = new AdscripcionBO { UnidadOperativa = new UnidadOperativaBO { Id = this.vista.UnidadOperativaID } };
                SeguridadBO seguridadBO = new SeguridadBO(Guid.Empty, usr, adscripcion);

                //Se obtienen las acciones a las que el usuario tiene permisos en este proceso
                List<CatalogoBaseBO> lst = FacadeBR.ConsultarAccion(this.dctx, seguridadBO);

                //consulta lista de acciones permitidas
                this.vista.ListaAcciones = lst;

                //Se valida si el usuario tiene permiso para registrar el check list         
                if (!this.ExisteAccion(lst, "REGISTRARENTREGA")
                    || !this.ExisteAccion(lst, "GENERARPAGOS"))
                    this.vista.RedirigirSinPermisoAcceso();
            } catch (Exception ex) {
                throw new Exception(nombreClase + ".EstablecerSeguridad:" + Environment.NewLine + ex.Message);
            }
        }

        /// <summary>
        /// Consulta en el listado de acciones configuradas una acción especifica
        /// </summary>
        /// <param name="acciones">Acciones configuradas</param>
        /// <param name="nombreAccion">Acción que se desea validar</param>
        /// <returns>Verdadero si existe acción,  falso si no existe la acción</returns>
        private bool ExisteAccion(List<CatalogoBaseBO> acciones, string nombreAccion) {
            if (acciones != null && acciones.Count > 0 && acciones.Exists(p => p != null && p.Nombre != null && p.Nombre.Trim().ToUpper() == nombreAccion.Trim().ToUpper()))
                return true;

            return false;
        }

        /// <summary>
        /// Valida que el tipo de archivo seleccionado este permitido dentro de los tipos configurados
        /// </summary>
        /// <param name="tipo">Tipo de archivo que desea validar</param>
        /// <returns>Verdadero si la extensión se encuentra, falso si no</returns>
        private TipoArchivoBO ValidarArchivo(String tipo) {
            List<TipoArchivoBO> tiposArchivo = (List<TipoArchivoBO>)this.vista.TiposArchivo;
            if (tiposArchivo != null) {
                TipoArchivoBO tipoArchivoTemp = tiposArchivo.Find(delegate(TipoArchivoBO ta) { return ta.Extension == tipo; });
                if (tipoArchivoTemp != null) {
                    return tipoArchivoTemp;
                } else {
                    this.vista.MostrarMensaje("El archivo no fue cargado.", ETipoMensajeIU.ERROR,
                        "La extensión del archivo no se encuentra en la lista de tipos de archivo permitidos.");
                }
            } else {
                this.vista.MostrarMensaje("El archivo no fue cargado.", ETipoMensajeIU.ERROR, "No hay una lista de tipos de archivo cargada.");
            }
            return null;
        }

        /// <summary>
        /// Valida las fechas del listado
        /// </summary>
        /// <returns></returns>
        private string ValidarFechasListado() {
            string s = "";
            if (!this.vista.FechaListado.HasValue)
                s += "Fecha entrega, ";

            if (!this.vista.FechaContrato.HasValue)
                s += "Fecha contrato, ";

            if (!this.vista.HoraContrato.HasValue)
                s += "Hora contrato, ";
            if (!this.vista.HoraListado.HasValue)
                s += "Hora entrega, ";

            return s;
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

        private List<Int32> ObtenerUltimosCombustibleHrs(Int32? unidadId, ETipoListadoVerificacion Etipo) {
            var contratoPSLBR = new ContratoPSLBR();
            var combustibleHrs = contratoPSLBR.ConsultarUltimosCombustibleHorometro(dctx, unidadId, Etipo);
            if (combustibleHrs == null) throw new Exception("No se obtuvieron el Combustible y Horas anteriores de la Unidad. ");
            return combustibleHrs;
        }

        private void DesplegarUltimosCombustibleHrs(List<Int32> ultimosCombustibleHrs) {
            if (ultimosCombustibleHrs.Count == 0) {
                this.vista.HorometroAnterior = 0;
            } else {
                this.vista.HorometroAnterior = ultimosCombustibleHrs[1];
            }
        }

        public String ValidarHorometro() {
            if (!this.vista.HorometroSalida.HasValue)
                return null;
            if (this.vista.HorometroSalida < this.vista.HorometroAnterior)
                return String.Format("Estas ingresando una cantidad de horas MENOR a las del Último Contrato para la Unidad, el cual fue de: {0}", this.vista.HorometroAnterior);

            return null;
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
                case (int)ETipoEmpresa.Equinova:
                    EmpresaConPermiso = ETipoEmpresa.Equinova;
                    break;
                case (int)ETipoEmpresa.Construccion:
                    EmpresaConPermiso = ETipoEmpresa.Construccion;
                    break;
            }
            this.vista.EstablecerAcciones(EmpresaConPermiso);
        }
        #endregion

        #endregion
    }
}