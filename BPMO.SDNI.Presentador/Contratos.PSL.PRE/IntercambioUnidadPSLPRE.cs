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
using BPMO.SDNI.Comun.PRE;
using BPMO.SDNI.Comun.VIS;
using BPMO.SDNI.Contratos.BO;
using BPMO.SDNI.Contratos.PSL.BO;
using BPMO.SDNI.Contratos.PSL.BR;
using BPMO.SDNI.Contratos.PSL.VIS;
using BPMO.SDNI.Equipos.BO;
using BPMO.SDNI.Equipos.BOF;
using BPMO.SDNI.Equipos.BR;
using BPMO.SDNI.Tramites.BO;
using BPMO.SDNI.Tramites.BR;

namespace BPMO.SDNI.Contratos.PSL.PRE {
    public class IntercambioUnidadPSLPRE {

        #region Atributos
        /// <summary>
        /// Nombre de la clase para agregar a los mensajes de Error
        /// </summary>
        private string nombreClase = "IntercambioUnidadPSLPRE";

        /// <summary>
        /// El DataContext que proveerá acceso a la base de datos
        /// </summary>
        private readonly IDataContext dctx;

        /// <summary>
        /// Vista sobre la que actual la interfaz
        /// </summary>
        private readonly IIntercambioUnidadPSLVIS vista;

        /// <summary>
        /// Controlador de contratos de RO
        /// </summary>
        private ContratoPSLBR controlador;

        /// <summary>
        /// Controlador de Unidades 
        /// </summary>
        private UnidadBR controladorUnidades;

        /// Presentador del Control de Documentos
        /// </summary>
        private ucCatalogoDocumentosPRE presentadorDocumentos;

        /// <summary>
        /// Presentador de la Barra de Herramientas
        /// </summary>
        private readonly ucHerramientasPSLPRE presentadorHerramientas;

        /// <summary>
        /// Identificador de la línea de contrato que es intercambiada
        /// </summary>
        public int? lineaContratoID;
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
        public IntercambioUnidadPSLPRE(IIntercambioUnidadPSLVIS vistaActual, IucCatalogoDocumentosVIS viewDocumentos, IucHerramientasPSLVIS vistaHerramientas) {
            try {
                this.vista = vistaActual;
                this.dctx = FacadeBR.ObtenerConexion();
                this.controlador = new ContratoPSLBR();
                this.controladorUnidades = new UnidadBR();
                this.presentadorDocumentos = new ucCatalogoDocumentosPRE(viewDocumentos);
                this.presentadorHerramientas = new ucHerramientasPSLPRE(vistaHerramientas);
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

                this.DesplegarSucursalesAutorizadas();
                this.EstablecerConfiguracionInicial();
                this.ConsultarCompleto();

                this.EstablecerSeguridad();
                this.presentadorDocumentos.ModoEditable(true);
                this.DeshabilitaBarraHerramientas();
            } catch (Exception ex) {
                throw new Exception(this.nombreClase + ".RealizarPrimeraCarga: " + ex.Message);
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
                if (!this.ExisteAccion(acciones, "UI INTERCAMBIAR"))
                    this.vista.PermitirIntercambiar(false);
            } catch (Exception ex) {

                throw new Exception(this.nombreClase + ".EstablecerSeguridad:" + ex.Message);
            }
        }

        /// <summary>
        /// Recupera las sucursales a las que el usuario autenticado tiene permiso e acceder 
        /// </summary>
        private void DesplegarSucursalesAutorizadas() {
            if (this.vista.SucursalesAutorizadas == null || this.vista.SucursalesAutorizadas.Count == 0) {
                SeguridadBO seguridad = new SeguridadBO(Guid.Empty, new UsuarioBO() { Id = this.vista.UsuarioID },
                        new AdscripcionBO { UnidadOperativa = new UnidadOperativaBO() { Id = this.vista.UnidadOperativaID } });
                this.vista.SucursalesAutorizadas = FacadeBR.ConsultarSucursalesSeguridadSimple(this.dctx, seguridad);
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

        private void EstablecerDatosNavegacion(object paqueteNavegacion) {
            try {
                if (paqueteNavegacion == null)
                    throw new Exception("Se esperaba un objeto en la navegación. No se puede identificar qué reservación se desea consultar.");
                if (!(paqueteNavegacion is ContratoPSLBO))
                    throw new Exception("Se esperaba un Contrato de Renta Diaria.");

                ContratoPSLBO bo = (ContratoPSLBO)paqueteNavegacion;

                this.DatoAInterfazUsuario(bo);
            } catch (Exception ex) {
                this.DatoAInterfazUsuario(new ContratoPSLBO());
                throw new Exception(this.nombreClase + ".EstablecerDatosNavegacion: " + ex.Message);
            }
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
                if (!FacadeBR.ExisteAccion(this.dctx, "INTERCAMBIAR", seguridadBO))
                    this.vista.RedirigirSinPermisoAcceso();
            } catch (Exception ex) {
                throw new Exception(this.nombreClase + ".ValidarAcceso: " + ex.Message);
            }
        }

        private void EstablecerConfiguracionInicial() {
            try {
                this.vista.FUA = DateTime.Now;
                this.vista.UUA = this.vista.UsuarioID;
                this.vista.FechaIntercambio = this.vista.FUA;

                //Se obtienen las configuraciones de la unidad operativa
                if (this.vista.UnidadOperativaID == null)
                    throw new Exception("No se identificó en qué unidad operativa trabaja.");
                List<ConfiguracionUnidadOperativaBO> lst = new ModuloBR().ConsultarConfiguracionUnidadOperativa(dctx, new ConfiguracionUnidadOperativaBO() { UnidadOperativa = new UnidadOperativaBO() { Id = this.vista.UnidadOperativaID } }, this.vista.ModuloID);
                //Por si se necesita configurar algo de la unidad operativa   

                //Se establecen los tipos de archivos permitidos para adjuntar al contrato
                List<TipoArchivoBO> lstTiposArchivo = new TipoArchivoBR().Consultar(this.dctx, new TipoArchivoBO());
                this.presentadorDocumentos.EstablecerTiposArchivo(lstTiposArchivo);
                this.presentadorDocumentos.RequiereObservaciones(false);
            } catch (Exception ex) {
                throw new Exception(this.nombreClase + ".EstablecerConfiguracionInicial: " + ex.Message);
            }
        }

        /// <summary>
        /// Deshabilita la barra de herramientas
        /// </summary>
        private void DeshabilitaBarraHerramientas() {
            //Ocultamos la barra de herramientas
            this.presentadorHerramientas.DeshabilitarMenuImprimir();
            this.presentadorHerramientas.DeshabilitarMenuCerrar();
            this.presentadorHerramientas.DeshabilitarMenuBorrar();
            this.presentadorHerramientas.vista.OcultarImprimirPlantillaCheckList();
            this.presentadorHerramientas.vista.OcultarPlantillas();
            this.presentadorHerramientas.vista.OcultarSolicitudPago();
            this.presentadorHerramientas.DeshabilitarMenuEditar();
        }

        /// <summary>
        /// Cancela el intercambio de la unidad
        /// </summary>
        public void CancelarRegistro() {
            ContratoPSLBO cto = (ContratoPSLBO)this.vista.UltimoObjeto;
            this.LimpiarSesion();
            this.vista.EstablecerPaqueteNavegacion("ContratoPSLBO", cto);
            this.vista.RedirigirACancelar();
        }

        /// <summary>
        /// Limpia los datos de la memoria de la Vista
        /// </summary>
        private void LimpiarSesion() {
            this.vista.LimpiarSesion();
            this.presentadorDocumentos.LimpiarSesion();
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

                ContratoPSLBO contrato = lst[0];
                this.DatoAInterfazUsuario(contrato);
                this.vista.UltimoObjeto = contrato;
                this.presentadorHerramientas.DatosAInterfazUsuario(contrato);
                this.vista.lstUnidades = null;

                List<UnidadBO> lstUnidadActivas = new List<UnidadBO>();
                foreach (LineaContratoPSLBO linea in contrato.LineasContrato.Where(a => a.Activo.HasValue && a.Activo.Value == true)) {
                    lstUnidadActivas.Add(new UnidadBO(linea.Equipo as UnidadBO));
                }
                
                this.vista.lstUnidades = lstUnidadActivas;
                this.DesplegarListadoSeries(lstUnidadActivas);
            } catch (Exception ex) {
                this.DatoAInterfazUsuario(new ContratoPSLBO());
                throw new Exception(this.nombreClase + ".ConsultarCompleto:" + ex.Message);
            }
        }

        /// <summary>
        /// Carga y muestra el listados de series de contratos en la vista
        /// </summary>
        private void DesplegarListadoSeries(List<UnidadBO> lstUnidadesSerie) {
            Dictionary<string, object> equipos = new Dictionary<string, object>();
            foreach (UnidadBO unidadBO in lstUnidadesSerie) {
                EquipoBO equipoBo = (unidadBO as EquipoBO);
                equipos.Add(equipoBo.NumeroSerie, equipoBo.EquipoID);
            }

            this.vista.CargarSerie(equipos);
        }

        private object InterfazUsuarioADato() {
            ContratoPSLBO bo = new ContratoPSLBO();
            if (this.vista.UltimoObjeto != null)
                bo = new ContratoPSLBO((ContratoPSLBO)this.vista.UltimoObjeto);

            bo.ContratoID = this.vista.ContratoID;
            if (bo.AnexosContrato == null) bo.AnexosContrato = new List<AnexosContratoPSLBO>();

            return bo;
        }

        private void DatoAInterfazUsuario(object obj) {
            ContratoPSLBO bo = (ContratoPSLBO)obj;
            if (bo == null) bo = new ContratoPSLBO();

            this.vista.ContratoID = bo.ContratoID;
            this.vista.EstatusID = (int)bo.Estatus;
            this.vista.TipoContrato = bo.Tipo;
        }

        public void ObtenerDatosUnidad() {
            if (this.vista.lstUnidades != null) {
                if (this.vista.lstUnidades.Count > 0) {
                    int? equipoId = this.vista.EquipoID.Value != null ? this.vista.EquipoID.Value : 0;
                    UnidadBO unidad = this.vista.lstUnidades.Where(r => r.EquipoID == this.vista.EquipoID).FirstOrDefault();

                    if (unidad == null)
                        throw new Exception("La unidad seleccionada no es valida. Favor de recargar el contrato.");

                    //revisamos que tenga checklist de recepción
                    ContratoPSLBO bo = (ContratoPSLBO)this.vista.ObtenerPaqueteNavegacion("UltimoContratoPSLBO");
                    ContratoPSLBR contratoPSLBR = new ContratoPSLBR();

                    //Obtenemos el Horómetro
                    if (bo != null) {
                        if (bo.LineasContrato != null) {
                            LineaContratoPSLBO lineaContrato = (LineaContratoPSLBO)bo.LineasContrato.Where(r => r.Equipo.EquipoID == equipoId).FirstOrDefault();
                            

                            //Verificamos que exista un checkList de recepción
                            if (lineaContrato.ListadosVerificacion.Count < 2) {
                                bool existeEntrega = lineaContrato.ListadosVerificacion.Where(r => r.Tipo == ETipoListadoVerificacion.ENTREGA).Any();
                                if (existeEntrega) {
                                    //Obtiene los últimos valores de Combustible y Horómetro
                                    List<Int32> linea = contratoPSLBR.ConsultarUltimosCombustibleHorometro(this.dctx, unidad.UnidadID, ETipoListadoVerificacion.ENTREGA);
                                    this.vista.PorcentajeUnidadCliente = linea.Count > 0 ? linea[0].ToString() : Convert.ToString(0);
                                    this.vista.HorometroUnidadCliente = linea.Count > 0 ? linea[1].ToString() : Convert.ToString(0);
                                }
                            } else {
                                bool existeRecepcion = lineaContrato.ListadosVerificacion.Where(r => r.Tipo == ETipoListadoVerificacion.RECEPCION).Any();
                                if (existeRecepcion) {
                                    if (lineaContrato != null) {
                                        //LineaContratoPSLBO lineaContratoBO = new LineaContratoPSLBO();
                                        AVerificacionLineaPSLBO listado = lineaContrato.ObtenerListadoVerificacionPorTipo<AVerificacionLineaPSLBO>(ETipoListadoVerificacion.RECEPCION);

                                        //Obtiene los últimos valores de Combustible y Horómetro
                                        List<Int32> linea = contratoPSLBR.ConsultarUltimosCombustibleHorometro(this.dctx, unidad.UnidadID, ETipoListadoVerificacion.RECEPCION);
                                        this.vista.PorcentajeUnidadCliente = linea.Count > 0 ? linea[0].ToString() : Convert.ToString(0);
                                        this.vista.HorometroUnidadCliente = linea.Count > 0 ? linea[1].ToString() : Convert.ToString(0);
                                    }
                                }
                            }
                        } else {
                            throw new Exception(this.nombreClase + ".ObtenerDatosUnidad: no se pueden obtener líneas del contrato de esta unidad ");
                        }
                    } else {
                        throw new Exception(this.nombreClase + ".ObtenerDatosUnidad: no se pueden obtener datos del contrato ");
                    }

                    this.vista.ECodeCliente = unidad.NumeroEconomico;
                    //consultamos el modelo                   
                    this.vista.ModeloCliente = ConsultarModeloUnidad(unidad.IDLider).ConfiguracionModeloMotorizacion.Modelo.Nombre;
                    this.vista.EquipoID = unidad.EquipoID;
                    this.vista.UnidadID = unidad.UnidadID;
                    this.vista.SucursalID = unidad.Sucursal.Id;
                }
            } else {
                throw new Exception(this.nombreClase + ".ObtenerDatosUnidad: no se pueden determinar la unidad del contrato ");
            }
        }

        private Servicio.Catalogos.BO.UnidadBO ConsultarModeloUnidad(int? idLider) {
            List<Servicio.Catalogos.BO.UnidadBO> lstUnidades = FacadeBR.ConsultarUnidadDetalle(this.dctx, new Servicio.Catalogos.BO.UnidadBO { Id = idLider });
            return lstUnidades[0];
        }

        /// <summary>
        /// Registra el contrato con estatus Borrador
        /// </summary>
        public void RegistrarIntercambio() {
            try {
                string s;
                if ((s = this.ValidarCamposBorrador()) != null) {
                    this.vista.MostrarMensaje(s, ETipoMensajeIU.ADVERTENCIA, null);
                    return;
                }

                this.vista.EstatusID = (int)EEstatusContrato.EnCurso;
                this.Registrar();

                ContratoPSLBO ctoCheckList = new ContratoPSLBO((ContratoPSLBO)this.vista.UltimoObjeto);
                this.vista.UltimoObjeto = null;

                this.vista.EstablecerPaqueteNavegacion("RegistrarEntregaPSLUI", ctoCheckList);
                this.vista.EstablecerPaqueteNavegacion("LineaContratoPSLID", this.lineaContratoID);
                this.vista.EstablecerPaqueteNavegacion("EsIntercambio", true);
                this.vista.RedirigirADetalles();
            } catch (Exception ex) {
                throw new Exception(nombreClase + ".IntercambioUnidadPSLPRE: " + Environment.NewLine + ex.Message);
            }
        }

        /// <summary>
        /// Valida los campos requeridos para un intercambio de unidad
        /// </summary>
        /// <returns></returns>
        public string ValidarCamposBorrador() {
            string s = string.Empty;

            if (this.vista.FC == null)
                s += "Fecha de Creación, ";
            if (this.vista.FUA == null)
                s += "Fecha de Última Modificación, ";
            if (this.vista.UC == null)
                s += "Usuario de Creación, ";
            if (this.vista.UUA == null)
                s += "Usuario de Última Modificación, ";
            if (this.vista.UnidadID == null)
                s += "Unidad a regresar, ";
            if (this.vista.IntercambioUnidadID == null)
                s += "Unidad a enviar, ";
            if (this.vista.FechaIntercambio == null)
                s += "Fecha de intercambiar, ";

            if (s != null && s.Trim().CompareTo("") != 0)
                return "Los siguientes campos no pueden estar vacíos: \n" + s.Substring(0, s.Length - 2);

            return null;
        }

        /// <summary>
        /// Registra un intercambio de unidad
        /// </summary>
        private void Registrar() {
            #region Se inicia la Transaccion
            dctx.SetCurrentProvider("Outsourcing");
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
                //Se crea el objeto de seguridad
                UsuarioBO usuario = new UsuarioBO() { Id = this.vista.UsuarioID };
                AdscripcionBO adscripcion = new AdscripcionBO() { UnidadOperativa = new UnidadOperativaBO() { Id = this.vista.UnidadOperativaID } };
                SeguridadBO seguridadBO = new SeguridadBO(Guid.Empty, usuario, adscripcion);

                //Se consulta la información del contrato
                ContratoPSLBO bo = (ContratoPSLBO)this.InterfazUsuarioADato();

                UnidadBO unidadNueva = new UnidadBO() { UnidadID = this.vista.IntercambioUnidadID };
                unidadNueva = controladorUnidades.ConsultarCompleto(this.dctx, unidadNueva, true).FirstOrDefault();
                
                ContratoPSLBO contratoPrevio = new ContratoPSLBO(bo);
                if (bo.ObtenerLineaContrato() != null && bo.ObtenerLineaContrato().Equipo != null && bo.ObtenerLineaContrato().Equipo is UnidadBO
                    && ((UnidadBO)bo.ObtenerLineaContrato().Equipo).UnidadID != null) {
                    
                    //Se obtiene el ID de la unidad que será intercambiada
                    UnidadBO unidadActual = new UnidadBO() { UnidadID = this.vista.UnidadID };
                    LineaContratoPSLBO linea = bo.ObtenerLineaContrato(unidadActual);

                    UnidadBO unidadBO = (UnidadBO)linea.Equipo;
                    if (unidadBO.UnidadID == this.vista.UnidadID) {
                        linea.Activo = false;

                        //Agregar nueva línea al contrato
                        bo.LineasContrato.Add(this.ObtenerLineaContrato(linea, unidadNueva, bo));
                    }
                }
                ContratoPSLBO contratoModificado = bo;
                
                //Generamos objeto tipo anexo
                AnexosContratoPSLBO anexo = new AnexosContratoPSLBO();
                anexo.FechaIntercambioUnidad = this.vista.FechaIntercambio;
                anexo.MontoTotalArrendamiento = contratoModificado.MontoTotalArrendamiento;
                anexo.TipoAnexoContrato = ETipoAnexoContrato.Intercambio;
                anexo.Vigente = true;
                anexo.ContratoPSLID = contratoModificado.ContratoID;
                anexo.FC = this.vista.FC;
                anexo.FUA = this.vista.FUA;
                anexo.UC = this.vista.UC;
                anexo.UUA = this.vista.UUA;
                contratoModificado.AnexosContrato.Add(anexo);

                #region Archivos Adjuntos
                List<ArchivoBO> adjuntos = presentadorDocumentos.Vista.ObtenerArchivos() ?? new List<ArchivoBO>();
                foreach (ArchivoBO adjunto in adjuntos) {
                    if (contratoModificado.Tipo == ETipoContrato.RO)
                        adjunto.TipoAdjunto = ETipoAdjunto.ContratoRO;
                    if (contratoModificado.Tipo == ETipoContrato.ROC)
                        adjunto.TipoAdjunto = ETipoAdjunto.ContratoROC;
                    if (contratoModificado.Tipo == ETipoContrato.RE)
                        adjunto.TipoAdjunto = ETipoAdjunto.ContratoRE;
                    if (contratoModificado.Tipo == ETipoContrato.RD)
                        adjunto.TipoAdjunto = ETipoAdjunto.ContratoRD;
                    if (contratoModificado.Tipo == ETipoContrato.FSL)
                        adjunto.TipoAdjunto = ETipoAdjunto.ContratoFSL;
                    if (contratoModificado.Tipo == ETipoContrato.CM)
                        adjunto.TipoAdjunto = ETipoAdjunto.ContratoMantenimiento;
                    if (contratoModificado.Tipo == ETipoContrato.SD)
                        adjunto.TipoAdjunto = ETipoAdjunto.ContratoServicioDedicado;

                    adjunto.Auditoria = new AuditoriaBO
                    {

                        FC = this.vista.FC,
                        UC = this.vista.UC,
                        FUA = this.vista.FUA,
                        UUA = this.vista.UUA
                    };
                }
                contratoModificado.DocumentosAdjuntos = adjuntos;
                #endregion

                this.controlador.ActualizarCompleto(this.dctx, contratoModificado, contratoPrevio, seguridadBO);
                this.vista.UltimoObjeto = contratoModificado;

                #region Recuperar ID del la línea agregada
                LineaContratoPSLBO lineaNueva = contratoModificado.LineasContrato
                            .ConvertAll(l => (LineaContratoPSLBO)l)
                            .FirstOrDefault(lc => lc.Equipo.EquipoID == this.vista.IntercambioEquipoID);

                if (lineaNueva != null)
                    this.lineaContratoID = lineaNueva.LineaContratoID; 
                #endregion /Recuperar ID

                dctx.CommitTransaction(firma);
            } catch (Exception ex) {
                dctx.RollbackTransaction(firma);
                throw new Exception(this.nombreClase + ".Registrar:" + ex.Message);
            } finally {
                if (dctx.ConnectionState == ConnectionState.Open)
                    dctx.CloseConnection(firma);
            }
        }

        private LineaContratoPSLBO ObtenerLineaContrato(LineaContratoPSLBO lineaAnterior, UnidadBO unidadNueva, ContratoPSLBO bo) {

            LineaContratoPSLBO lineaBO = new LineaContratoPSLBO();

            lineaBO.Equipo = unidadNueva;

            lineaBO.TipoTarifa = lineaAnterior.TipoTarifa;

            lineaBO.Cobrable = new TarifaContratoPSLBO
            {
                PeriodoTarifa = ((TarifaContratoPSLBO)lineaAnterior.Cobrable).PeriodoTarifa,
                Tarifa = ((TarifaContratoPSLBO)lineaAnterior.Cobrable).Tarifa,
                TarifaHrAdicional = ((TarifaContratoPSLBO)lineaAnterior.Cobrable).TarifaHrAdicional,
                TarifaTurno = ((TarifaContratoPSLBO)lineaAnterior.Cobrable).TarifaTurno,
                TipoTarifaID = ((TarifaContratoPSLBO)lineaAnterior.Cobrable).TipoTarifaID,
                Maniobra = ((TarifaContratoPSLBO)lineaAnterior.Cobrable).Maniobra,
                TarifaPSLID = ((TarifaContratoPSLBO)lineaAnterior.Cobrable).TarifaPSLID,
                DuracionDiasPeriodo = ((TarifaContratoPSLBO)lineaAnterior.Cobrable).DuracionDiasPeriodo,
                MaximoHrsTurno = ((TarifaContratoPSLBO)lineaAnterior.Cobrable).MaximoHrsTurno,
                TarifaDiaria = ((TarifaContratoPSLBO)lineaAnterior.Cobrable).TarifaDiaria,
                TarifaRealAcumulada = ((TarifaContratoPSLBO)lineaAnterior.Cobrable).TarifaRealAcumulada,
                TarifaCobradaAcumulada = ((TarifaContratoPSLBO)lineaAnterior.Cobrable).TarifaCobradaAcumulada,
                TarifaCobradaEnPago = ((TarifaContratoPSLBO)lineaAnterior.Cobrable).TarifaCobradaEnPago,
                TarifaDevueltaNotaC = ((TarifaContratoPSLBO)lineaAnterior.Cobrable).TarifaDevueltaNotaC,
                TarifaDevueltaNotaCAcumulada = ((TarifaContratoPSLBO)lineaAnterior.Cobrable).TarifaDevueltaNotaCAcumulada,
                Activo = ((TarifaContratoPSLBO)lineaAnterior.Cobrable).Activo,
                PorcentajeDescuento = ((TarifaContratoPSLBO)lineaAnterior.Cobrable).PorcentajeDescuento,
                PorcentajeDescuentoMaximo = ((TarifaContratoPSLBO)lineaAnterior.Cobrable).PorcentajeDescuentoMaximo,
                EtiquetaDescuento = ((TarifaContratoPSLBO)lineaAnterior.Cobrable).EtiquetaDescuento,
                TarifaConDescuento = ((TarifaContratoPSLBO)lineaAnterior.Cobrable).TarifaConDescuento,
                PorcentajeSeguro = ((TarifaContratoPSLBO)lineaAnterior.Cobrable).PorcentajeSeguro
            };
            lineaBO.Activo = true;

            //Si el tipo de contrato es RO, significa que ya paso al menos por aquí y no será necesario realizar las validaciones de nuevo
            if (bo.Tipo != ETipoContrato.RO) {
                switch (vista.UnidadOperativaID) {
                    case (int)ETipoEmpresa.Construccion:
                        if ((EAreaConstruccion)((UnidadBO)lineaAnterior.Equipo).Area == EAreaConstruccion.RO) {
                            bo.Tipo = ETipoContrato.RO;
                        }
                        break;
                    case (int)ETipoEmpresa.Generacion:
                        if ((EAreaGeneracion)((UnidadBO)lineaAnterior.Equipo).Area == EAreaGeneracion.RO) {
                            bo.Tipo = ETipoContrato.RO;
                        }
                        break;
                    case (int)ETipoEmpresa.Equinova:
                        if ((EAreaEquinova)((UnidadBO)lineaAnterior.Equipo).Area == EAreaEquinova.RO) {
                            bo.Tipo = ETipoContrato.RO;
                        }
                        break;
                    default:
                        break;
                }
            }

            return lineaBO;
        }

        #region Métodos para el Buscador
        public object PrepararBOBuscador(string catalogo) {
            object obj = null;

            switch (catalogo) {
                case "Unidad":
                    UnidadBOF unidad = new UnidadBOF();
                    unidad.Sucursal = new SucursalBO();
                    if (!string.IsNullOrEmpty(vista.NumeroSerie))
                        unidad.NumeroSerie = vista.NumeroSerie;

                    unidad.EstatusActual = EEstatusUnidad.Disponible;
                    switch (this.vista.UnidadOperativaID) {
                        case (int)ETipoEmpresa.Construccion:
                            unidad.TiposContrato = new List<int>();
                            if (this.vista.TipoContrato == ETipoContrato.ROC)
                                unidad.TiposContrato.Add((int)EAreaConstruccion.ROC);
                            else {
                                unidad.TiposContrato.Add((int)EAreaConstruccion.RO);
                                unidad.TiposContrato.Add((int)EAreaConstruccion.RE);
                            }
                            break;
                        case (int)ETipoEmpresa.Generacion:
                            unidad.TiposContrato = new List<int>();
                            if (this.vista.TipoContrato == ETipoContrato.ROC)
                                unidad.TiposContrato.Add((int)EAreaGeneracion.ROC);
                            else {
                                unidad.TiposContrato.Add((int)EAreaGeneracion.RO);
                                unidad.TiposContrato.Add((int)EAreaGeneracion.RE);
                            }
                            break;
                        case (int)ETipoEmpresa.Equinova:
                            unidad.TiposContrato = new List<int>();
                            if (this.vista.TipoContrato == ETipoContrato.ROC)
                                unidad.TiposContrato.Add((int)EAreaEquinova.ROC);
                            else {
                                unidad.TiposContrato.Add((int)EAreaEquinova.RO);
                                unidad.TiposContrato.Add((int)EAreaEquinova.RE);
                            }
                            break;
                        case (int)ETipoEmpresa.Idealease:
                            unidad.Area = EArea.RD;
                            break;
                    }
                    unidad.Sucursal.Id = this.vista.SucursalID;
                    obj = unidad;
                    break;
                case "Modelo":
                    BPMO.Servicio.Catalogos.BO.ModeloBO modelo = new BPMO.Servicio.Catalogos.BO.ModeloBO();
                    modelo.Auditoria = new AuditoriaBO();
                    modelo.Nombre = this.vista.ModeloNombre;
                    modelo.Activo = true;
                    obj = modelo;
                    break;
            }
            this.LimpiarCampos();
            return obj;
        }
        public void DesplegarResultadoBuscador(string catalogo, object selecto) {
            switch (catalogo) {
                case "Unidad":
                    Equipos.BO.UnidadBO unidadBO = (Equipos.BO.UnidadBO)selecto;
                    this.SeleccionarUnidad(unidadBO);

                    if (selecto != null && (unidadBO.UnidadID != null)) {
                        this.vista.NumeroSerie = unidadBO.NumeroSerie ?? string.Empty;
                        this.vista.IntercambioUnidadID = unidadBO.UnidadID;
                        this.vista.IntercambioEquipoID = unidadBO.EquipoID;

                        //Obtenemos el modelo de la unidad
                        this.vista.ModeloNombre = ConsultarModeloUnidad(unidadBO.IDLider).ConfiguracionModeloMotorizacion.Modelo.Nombre;
                        this.vista.ECode = unidadBO.NumeroEconomico;

                        //Obtiene los últimos valores de Combustible y Horómetro
                        ContratoPSLBR contratoPSLBR = new ContratoPSLBR();
                        List<Int32> linea = contratoPSLBR.ConsultarUltimosCombustibleHorometro(this.dctx, unidadBO.UnidadID, ETipoListadoVerificacion.RECEPCION);

                        int hrsInicial = 0;
                        if (linea.Count == 0) {
                            hrsInicial = ObtenerHrsInicialEquipo(this.vista.NumeroSerie);
                        }

                        this.vista.PorcentajeCombustibleIntercambio = linea.Count > 0 ? linea[0].ToString() : Convert.ToString(0);
                        this.vista.HorometroUnidadIntercambio = linea.Count > 0 ? linea[1].ToString() : Convert.ToString(hrsInicial);
                    }

                    break;
                case "Modelo":
                    BPMO.Servicio.Catalogos.BO.ModeloBO modelo = (BPMO.Servicio.Catalogos.BO.ModeloBO)selecto;

                    if (modelo != null && modelo.Id != null) {
                        this.vista.ModeloID = modelo.Id;

                        UnidadBO unidadBo = new UnidadBO();
                        unidadBo.Modelo = new Servicio.Catalogos.BO.ModeloBO();
                        unidadBo.Modelo.Id = modelo.Id;
                        List<UnidadBO> lstUnidades = controladorUnidades.ConsultarCompleto(this.dctx, unidadBo);
                        if (lstUnidades.Any()) {
                            unidadBo = lstUnidades[0];
                            vista.NumeroSerie = unidadBo.NumeroSerie ?? string.Empty;
                            this.vista.IntercambioUnidadID = unidadBo.UnidadID;
                            this.vista.IntercambioEquipoID = unidadBo.EquipoID;
                            this.vista.ECode = unidadBo.NumeroEconomico;

                            ContratoPSLBR contratoPSLBR = new ContratoPSLBR();
                            List<Int32> linea = contratoPSLBR.ConsultarUltimosCombustibleHorometro(this.dctx, unidadBo.UnidadID, ETipoListadoVerificacion.RECEPCION);

                            int hrsInicial = 0;
                            if (linea.Count == 0) {
                                hrsInicial = ObtenerHrsInicialEquipo(this.vista.NumeroSerie);
                            }

                            this.vista.PorcentajeCombustibleIntercambio = linea.Count > 0 ? linea[0].ToString() : Convert.ToString(0);
                            this.vista.HorometroUnidadIntercambio = linea.Count > 0 ? linea[1].ToString() : Convert.ToString(hrsInicial);

                        } else {
                            throw new Exception(this.nombreClase + ".DesplegarResultadoBuscador: no se puede usar este modelo de unidad para el intercambio ");
                        }
                    } else
                        this.vista.ModeloID = null;

                    if (modelo != null && modelo.Nombre != null)
                        this.vista.ModeloNombre = modelo.Nombre;
                    else
                        this.vista.ModeloNombre = null;
                    break;
            }
        }

        private void LimpiarCampos() {
            this.vista.LimpiarCampos();
        }

        public void SeleccionarUnidad(Equipos.BO.UnidadBO unidad) {
            try {
                #region Se obtiene la información completa de la unidad y sus trámites
                List<TramiteBO> lstTramites = new List<TramiteBO>();

                if (unidad != null && (unidad.UnidadID != null || unidad.EquipoID != null)) {
                    List<Equipos.BO.UnidadBO> lst = new UnidadBR().ConsultarCompleto(this.dctx, new Equipos.BO.UnidadBO() { UnidadID = unidad.UnidadID, EquipoID = unidad.EquipoID }, true);
                    if (lst.Count <= 0)
                        throw new Exception("No se encontró la información completa de la unidad seleccionada.");

                    unidad = lst[0];

                    lstTramites = new TramiteBR().ConsultarCompleto(this.dctx, new TramiteProxyBO() { Activo = true, Tramitable = unidad }, false);

                    bool esValido = ((EAreaConstruccion)unidad.Area == EAreaConstruccion.RO || (EAreaConstruccion)unidad.Area == EAreaConstruccion.ROC);

                    if (this.vista.UnidadOperativaID == (int)ETipoEmpresa.Construccion && esValido) {
                        CatalogoBaseBO catalogoBase = unidad.TipoEquipoServicio;
                        unidad.TipoEquipoServicio = FacadeBR.ConsultarTipoUnidad(dctx, catalogoBase).FirstOrDefault();

                        ContratoPSLBR Contratobr = new ContratoPSLBR();
                        ETipoUnidad? tipo = Contratobr.ObtenerTipoUnidadPorClave(dctx, unidad.TipoEquipoServicio.NombreCorto, null);

                        if (tipo == null)
                        {
                            this.vista.HabilitarBotonTerminar(false);
                            this.vista.MostrarMensaje(this.nombreClase + ".SeleccionarUnidad: " +  "No es posible seleccionar la unidad " + unidad.NumeroSerie + " debido a que no existe un checklist para su tipo (" + unidad.TipoEquipoServicio.NombreCorto + ")", ETipoMensajeIU.ADVERTENCIA, null);
                            return;
                        }
                        else
                            this.vista.HabilitarBotonTerminar(true);
                    }
                    else
                        this.vista.HabilitarBotonTerminar(true);
                }
                #endregion

                #region Dato a Interfaz de Usuario
                //Información de la Unidad
                if (unidad == null) unidad = new Equipos.BO.UnidadBO();
                if (unidad.Modelo == null) unidad.Modelo = new BPMO.Servicio.Catalogos.BO.ModeloBO();
                if (unidad.Modelo.Marca == null) unidad.Modelo.Marca = new BPMO.Servicio.Catalogos.BO.MarcaBO();
                if (unidad.Sucursal == null) unidad.Sucursal = new SucursalBO();
                if (unidad.CaracteristicasUnidad == null) unidad.CaracteristicasUnidad = new CaracteristicasUnidadBO();

                #endregion

            } catch (Exception ex) {
                throw new Exception(this.nombreClase + ".SeleccionarUnidad: " + ex.Message);
            }
        }
        #endregion

        /// <summary>
        /// Método para obtener las HorasInicial del equipo
        /// </summary>
        /// <param name="Serie">Número de serie de la unidad</param>
        /// <returns>Devuelve las hrs iniciales del equipo</returns>
        public int ObtenerHrsInicialEquipo(string Serie) {
            UnidadBOF unidadBOF = new UnidadBOF();
            DataSet dtsUnidad = new DataSet();
            UnidadBR unidadbr = new UnidadBR();

            int HrsInicial = 0;
            unidadBOF.NumeroSerie = Serie;
            dtsUnidad = unidadbr.ConsultarUnidad(this.dctx, unidadBOF);
            HrsInicial = Convert.ToInt32(dtsUnidad.Tables[0].Rows[0]["HorasInicial"].ToString());

            return HrsInicial;
        }
        #endregion
    }
}
