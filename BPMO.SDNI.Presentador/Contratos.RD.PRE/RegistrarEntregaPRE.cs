// Satisface al CU007 - Registrar entrega recepción de unidad
// Satisface al CU012 - Imprimir Check List de Entrega Recepción de Unidad
// Satisface al Reporte de Inconsistencia RI0016
// Satisface a la solicitud de cambio SC0035

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using BPMO.Basicos.BO;
using BPMO.Facade.SDNI.BR;
using BPMO.Patterns.Creational.DataContext;
using BPMO.Primitivos.Enumeradores;
using BPMO.SDNI.Comun.BO;
using BPMO.SDNI.Comun.BR;
using BPMO.SDNI.Comun.PRE;
using BPMO.SDNI.Comun.VIS;
using BPMO.SDNI.Contratos.BO;
using BPMO.SDNI.Contratos.RD.BO;
using BPMO.SDNI.Contratos.RD.BR;
using BPMO.SDNI.Contratos.RD.VIS;
using BPMO.SDNI.Equipos.BO;
using BPMO.SDNI.Equipos.BR;
using BPMO.SDNI.Facturacion.AplicacionesPago.BR;
using BPMO.SDNI.Facturacion.InfraestructuraPagos.BO;
using BPMO.SDNI.Facturacion.InfraestructuraPagos.BR;
using BPMO.SDNI.Flota.BO;
using BPMO.SDNI.Flota.BOF;
using BPMO.SDNI.Flota.BR;
using BPMO.SDNI.Flota.PRE;
using BPMO.SDNI.Flota.VIS;
using BPMO.SDNI.Tramites.BO;

namespace BPMO.SDNI.Contratos.RD.PRE
{
    /// <summary>
    /// Presentador para la página de registro de entrega de unidad
    /// </summary>
    public class RegistrarEntregaPRE
    {
        #region Atributos
        /// <summary>
        /// Vista para la página de registro de entrega de unidad
        /// </summary>
        private readonly IRegistrarEntregaVIS vista;
        /// <summary>
        /// Provee la conexión a la BD
        /// </summary>
        private readonly IDataContext dctx;
        /// <summary>
        /// Nombre de la clase que se usará en los mensajes de error
        /// </summary>
        private const string nombreClase = "RegistrarEntregaUnidadPRE";
        /// <summary>
        /// Controlador que ejecutará las accciones
        /// </summary>
        private readonly ContratoRDBR controlador;
        /// <summary>
        /// Presentador del catálogo de documentos
        /// </summary>
        private ucCatalogoDocumentosPRE presentadorDocumentos;
        /// <summary>
        /// Presentador para los equipso aliados de la unidad
        /// </summary>
        private ucEquiposAliadosUnidadPRE presentadorEqipoAlidos;
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor del presentador
        /// </summary>
        /// <param name="vista">Vista de la página principal</param>
        /// <param name="viewDocumentos">vista del User Control de documentos</param>
        /// <param name="viewEqipoAliado">vista del User Control de Equipos Aliados</param>
        public RegistrarEntregaPRE(IRegistrarEntregaVIS vista, IucCatalogoDocumentosVIS viewDocumentos, IucEquiposAliadosUnidadVIS viewEqipoAliado)
        {
            try
            {
                if (ReferenceEquals(vista, null))
                    throw new Exception(String.Format("{0}: La vista proporcionada no puede ser nula", nombreClase));

                this.vista = vista;
                this.controlador = new ContratoRDBR();
                this.dctx = FacadeBR.ObtenerConexion();
                this.presentadorDocumentos = new ucCatalogoDocumentosPRE(viewDocumentos);
                this.presentadorEqipoAlidos = new ucEquiposAliadosUnidadPRE(viewEqipoAliado);
            }
            catch (Exception ex)
            {
                this.vista.MostrarMensaje("Inconsistencia en los parámetros de configuración", ETipoMensajeIU.ERROR, nombreClase + ".RegistrarEntregaUnidadPRE" + Environment.NewLine + ex.Message);
            }
        }
        #endregion

        #region Métodos
        /// <summary>
        /// proporciona la colección de secciones configurados para las observaciones
        /// </summary>
        /// <returns>Colección de secciones de la unidad</returns>
        private Dictionary<int, string> ObtenerSecciones()
        {
            var tipos = new Dictionary<int, string>();
            try
            {
                if (string.IsNullOrEmpty(this.vista.SeccionesUnidades) || string.IsNullOrWhiteSpace(this.vista.SeccionesUnidades))
                    throw new Exception("Aun no se configuran las secciones de las unidades.");

                string[] secciones = this.vista.SeccionesUnidades.Split('|');

                if (secciones.Length > 0)
                {
                    foreach (var seccion in secciones)
                    {
                        if (!string.IsNullOrEmpty(seccion) && !string.IsNullOrWhiteSpace(seccion))
                        {
                            var val = 0;
                            if (Int32.TryParse(seccion, out val))
                                tipos.Add(val, seccion);                            
                        }
                    }
                }
                else
                    throw new Exception("Aun no se configuran las secciones de las unidades.");
            }
            catch (Exception ex)
            {
                this.vista.MostrarMensaje(nombreClase + ".ObtenerSecciones: " + Environment.NewLine + ex.Message, ETipoMensajeIU.ADVERTENCIA, null);
            }  
            return tipos;            
        }
        /// <summary>
        /// Carga en la vista los tipos de archivos configurados para las imagenes
        /// </summary>
        public void CargarTipoArchivosImagen()
        {
            var tipoBR = new TipoArchivoBR();
            var tipo = new TipoArchivoBO { EsImagen = true };
            vista.TiposArchivoImagen = tipoBR.Consultar(this.dctx, tipo);
        }
        /// <summary>
        /// Prepara la vista para el registro de entrega de la unidad
        /// </summary>
        public void PrepararNuevo()
        {
            this.vista.LimpiarSesion();
            this.LimpiarCampos();
            this.vista.PrepararNuevo();
            this.presentadorDocumentos.ModoEditable(true);
            this.presentadorEqipoAlidos.Inicializar();
            this.vista.EstablecerOpcionesSeccion(this.ObtenerSecciones());
            this.CargarTipoArchivosImagen();
            this.EstablecerInformacionInicial();
            this.vista.LimpiarPaqueteNavegacion();

            if(DateTime.Compare(((DateTime)vista.FechaContrato).Date, DateTime.Today) != 0)
            {
                vista.BloquearRegistro();
                vista.MostrarMensaje("La fecha del contrato no coincide con la fecha actual, no es posible registrar el check list", ETipoMensajeIU.ADVERTENCIA);
            }

            this.EstablecerSeguridad();
        }
        /// <summary>
        /// Limpia los campos de la vista
        /// </summary>
        private void LimpiarCampos()
        {
            this.vista.BateriasCorrectas = null;            
            this.vista.Combustible = null;
            this.vista.ContratoID = null;
            this.vista.EquipoID = null;
            this.vista.EstatusContratoID = null;
            this.vista.FechaContrato = null;
            this.vista.FechaListado = null;
            this.vista.NombreCliente = string.Empty;
            this.vista.NombreOperador = string.Empty;
            this.vista.NombreUsuarioEntrega = string.Empty;
            this.vista.PlacasFederales = string.Empty;
            this.vista.PlacasEstatales = string.Empty;
            this.vista.NumeroContrato = string.Empty;
            this.vista.NumeroEconomico = string.Empty;
            this.vista.NumeroSerie = string.Empty;
            this.vista.HoraListado = null;
            this.vista.HoraContrato = null;
            this.vista.Kilometraje = null;
            this.vista.Horometro = null;
            this.vista.Kilometraje = null;
            this.vista.LineaContratoID = null;            
            this.vista.ObservacionesLlantas = string.Empty;
            this.vista.ObservacionesBaterias = string.Empty;
            this.vista.ObservacionesDocumentacionCompleta = string.Empty;
            this.vista.ObservacionesSeccion = string.Empty;
            this.vista.RefaccionCodigo = string.Empty;
            this.vista.RefaccionEstado = null;
            this.vista.RefaccionID = null;
            this.vista.TieneAlarmaReversa = null;
            this.vista.TieneDocumentacionCompleta = null;
            this.vista.TieneEncendedor = null;
            this.vista.TieneEspejosCompletos = null;
            this.vista.TieneExtinguidor = null;
            this.vista.TieneGPS = null;
            this.vista.TieneGatoLlaveTuerca = null;
            this.vista.TieneInteriorLimpio = null;
            this.vista.TieneLimpiezaInteriorCaja = null;
            this.vista.TieneLlaveOriginal = null;
            this.vista.TieneStereoBocinas = null;
            this.vista.TieneTapetes = null;
            this.vista.TieneTresReflejantes = null;
            this.vista.TieneVestidurasLimpias = null;
            this.vista.TipoListado = null;            
            this.vista.VerificacionesLlanta = null;
            this.vista.VerificacionesSeccion = null;
            this.vista.UnidadID = null;
            this.vista.NumeroLicencia = string.Empty;
        }
        /// <summary>
        /// Establece en la vista la información inicial para el registro
        /// </summary>
        public void EstablecerInformacionInicial()
        {
            try
            {
                //Obtenemos el contrato al cual le haremos el check list
                int? val = (int?)this.vista.ObtenerPaqueteContrato();

                if (!val.HasValue)
                    throw new Exception("No es posible recuperar la información del contrato necesaria para el registro del check list. Intente de nuevo por favor.");
                
                this.vista.ContratoID = val;

                //Se establecen los tipos de archivos permitidos para adjuntar al contrato
                List<TipoArchivoBO> lstTiposArchivo = new TipoArchivoBR().Consultar(this.dctx, new TipoArchivoBO());                
                var o = new object();
                this.presentadorDocumentos.Vista.Identificador = o.GetHashCode().ToString();
                this.presentadorDocumentos.EstablecerTiposArchivo(lstTiposArchivo);

                #region Contrato
                ContratoRDBR contratoBR = new ContratoRDBR();
                List<ContratoRDBO> contratos = contratoBR.ConsultarParcial(this.dctx, new ContratoRDBO {ContratoID = this.vista.ContratoID}, false, false, false);

                if(ReferenceEquals(contratos, null))
                    throw new Exception("No es posible recuperar la información del contrato necesaria para el registro del check list. Intente de nuevo por favor.");
                if(contratos.Count <= 0)
                    throw new Exception("No es posible recuperar la información del contrato necesaria para el registro del check list. Intente de nuevo por favor.");

                //Desplegamos la información generla del contrato
                this.DesplegarInformacionGeneral(contratos[0]); 
                #endregion

                #region Usuario
                CatalogoBaseBO ubase = new UsuarioBO {Activo = true, Id = this.vista.UsuarioID};
                List<UsuarioBO> usuarios = FacadeBR.ConsultarUsuario(this.dctx, ubase);
                if (!ReferenceEquals(usuarios, null))
                {
                    if (usuarios.Count > 0)
                    {
                        this.vista.NombreUsuarioEntrega = !string.IsNullOrEmpty(usuarios[0].Nombre) && !string.IsNullOrWhiteSpace(usuarios[0].Nombre)
                                                              ? usuarios[0].Nombre.Trim().ToUpper()
                                                              : string.Empty;
                    }
                }
                #endregion

                #region Check List

                this.vista.HoraListado = DateTime.Now.TimeOfDay;
                this.vista.TipoListado = (int)ETipoListadoVerificacion.ENTREGA;
                #endregion                
            }
            catch (Exception ex)
            {
                throw new Exception(nombreClase + ".EstablecerInformacionInicial: " + ex.Message);
            }
        }
        /// <summary>
        /// Despliega en la vista las llantas de la unidad seleccionada en el contrato
        /// </summary>
        /// <param name="unidadBO">Unidad de la cual deseamos desplegar las llantas</param>
        private void DesplegarInformacionLLantas(UnidadBO unidadBO)
        {
            UnidadBR unidadBR = new UnidadBR();

            List<LlantaBO> llantas = unidadBR.ConsultarLlantas(this.dctx, unidadBO);

            var refacciones = llantas.Where(p => p.EsRefaccion == true).ToList();
            LlantaBO refaccion = null;
            if (refacciones.Any())
            {
                refaccion = refacciones[0];
                llantas.Remove(refaccion);                
            }

            unidadBO.AgregarLlantas(llantas);            
            this.vista.CargarLlantas(unidadBO);

            #region Refacción
            if (!ReferenceEquals(refaccion, null))
            {                
                this.vista.RefaccionCodigo = !string.IsNullOrEmpty(refaccion.Codigo) && !string.IsNullOrWhiteSpace(refaccion.Codigo)
                                                 ? refaccion.Codigo.Trim().ToUpper()
                                                 : string.Empty;

                this.vista.RefaccionID = refaccion.LlantaID.HasValue ? refaccion.LlantaID : null;
                this.vista.HabilitarRefaccion(true);
            }else this.vista.HabilitarRefaccion(false);

            #endregion
        }
        /// <summary>
        /// Despliega en la vista las unidad seleccionada en el contrato
        /// </summary>
        /// <param name="unidadBO">Unidad que será desplegada</param>
        private void DesplegarInformacionUnidad(UnidadBO unidadBO)
        {
            if(ReferenceEquals(unidadBO, null))
                throw new Exception("No es posible recuperar la información de la unidad necesaria para el registro del check list. Intente de nuevo por favor.");
            
            FlotaBOF bof = new FlotaBOF();
            bof.Unidad = unidadBO;

            SeguimientoFlotaBR flotaBR = new SeguimientoFlotaBR();
            FlotaBO flota = flotaBR.ConsultarFlotaRentaDiaria(dctx, bof);

            if(ReferenceEquals(flota, null))
                throw new Exception("No es posible recuperar la información de la unidad necesaria para el registro del check list. Intente de nuevo por favor.");

            if(ReferenceEquals(flota.ElementosFlota, null))
                throw new Exception("No es posible recuperar la información de la unidad necesaria para el registro del check list. Intente de nuevo por favor.");

            if(flota.ElementosFlota.Count <= 0)
                throw new Exception("No es posible recuperar la información de la unidad necesaria para el registro del check list. Intente de nuevo por favor.");

            ElementoFlotaBO elemento = flota.ElementosFlota[0];
            #region Unidad
            if (!ReferenceEquals(elemento.Unidad, null))
            {
                DesplegarUltimosKmHrs(ObtenerUltimosKmHrs(elemento.Unidad.UnidadID));

                this.vista.UnidadID = elemento.Unidad.UnidadID.HasValue ? elemento.Unidad.UnidadID : null;
                this.vista.EquipoID = elemento.Unidad.EquipoID.HasValue ? elemento.Unidad.EquipoID : null;
                this.vista.NumeroEconomico = !string.IsNullOrEmpty(elemento.Unidad.NumeroEconomico) && !string.IsNullOrWhiteSpace(elemento.Unidad.NumeroEconomico)
                                                 ? elemento.Unidad.NumeroEconomico.Trim().ToUpper()
                                                 : string.Empty;

                this.vista.NumeroSerie = !string.IsNullOrEmpty(elemento.Unidad.NumeroSerie) && !string.IsNullOrWhiteSpace(elemento.Unidad.NumeroSerie)
                                             ? elemento.Unidad.NumeroSerie.Trim().ToUpper()
                                             : string.Empty;
                #region Placas
                TramiteBO federal = elemento.ObtenerTramitePorTipo(ETipoTramite.PLACA_FEDERAL);
                TramiteBO estatal = elemento.ObtenerTramitePorTipo(ETipoTramite.PLACA_ESTATAL);

                if (!ReferenceEquals(federal, null))
                {
                    this.vista.PlacasFederales = !string.IsNullOrEmpty(federal.Resultado) && !string.IsNullOrWhiteSpace(federal.Resultado)
                                                ? federal.Resultado.Trim().ToUpper()
                                                : string.Empty;
                   
                }
                if (!ReferenceEquals(estatal, null))
                {
                    this.vista.PlacasEstatales = !string.IsNullOrEmpty(estatal.Resultado) && !string.IsNullOrWhiteSpace(estatal.Resultado)
                                                ? estatal.Resultado.Trim().ToUpper()
                                                : string.Empty;
                }
                #endregion                

                #region Capacidad Tanque
                this.vista.CapacidadTanque = elemento.Unidad.CaracteristicasUnidad != null ? (elemento.Unidad.CaracteristicasUnidad.CapacidadTanque.HasValue ? elemento.Unidad.CaracteristicasUnidad.CapacidadTanque : null ) : null;
                #endregion
            }

            #region EquiposAliados
            this.presentadorEqipoAlidos.DatoAInterfazUsuario(elemento); 
            this.presentadorEqipoAlidos.CargarEquiposAliados();   
            #endregion

            #region Llantas
            this.DesplegarInformacionLLantas(unidadBO);
            #endregion
            #endregion
        }
        /// <summary>
        /// Despliega la información general necesaria para el registro del check list
        /// </summary>
        /// <param name="contratoRDBO"></param>
        private void DesplegarInformacionGeneral(ContratoRDBO contratoRDBO)
        {
            if(ReferenceEquals(contratoRDBO, null))
                throw new Exception("No es posible recuperar la información del contrato necesaria para el registro del check list. Intente de nuevo por favor.");

            this.vista.ContratoID = contratoRDBO.ContratoID ?? null;
            this.vista.FechaContrato = contratoRDBO.FechaContrato ?? null;
            this.vista.HoraContrato = contratoRDBO.FechaContrato.HasValue
                                          ? (TimeSpan?)contratoRDBO.FechaContrato.Value.TimeOfDay
                                          : null;
            this.vista.EstatusContratoID = contratoRDBO.Estatus.HasValue ? (int?)contratoRDBO.Estatus : null;
            this.vista.NumeroContrato = !string.IsNullOrEmpty(contratoRDBO.NumeroContrato) && !string.IsNullOrWhiteSpace(contratoRDBO.NumeroContrato)
                                            ? contratoRDBO.NumeroContrato.Trim().ToUpper()
                                            : string.Empty;
			vista.FechaListado = contratoRDBO.FechaContrato ?? null;
			vista.HoraContrato = DateTime.Now.TimeOfDay;
			

            #region Cliente
            if (!ReferenceEquals(contratoRDBO.Cliente, null))
            {
                this.vista.NombreCliente = !string.IsNullOrEmpty(contratoRDBO.Cliente.Nombre) && !string.IsNullOrWhiteSpace(contratoRDBO.Cliente.Nombre)
                                               ? contratoRDBO.Cliente.Nombre.Trim().ToUpper()
                                               : string.Empty;
            }
            #endregion

            #region Operador
            if (!ReferenceEquals(contratoRDBO.Operador, null))
            {
                this.vista.NombreOperador = !string.IsNullOrEmpty(contratoRDBO.Operador.Nombre) && !string.IsNullOrWhiteSpace(contratoRDBO.Operador.Nombre)
                                                ? contratoRDBO.Operador.Nombre.Trim().ToUpper()
                                                : string.Empty;
                if (contratoRDBO.Operador.Licencia != null)
                {
                    this.vista.NumeroLicencia = !string.IsNullOrEmpty(contratoRDBO.Operador.Licencia.Numero) && !string.IsNullOrWhiteSpace(contratoRDBO.Operador.Licencia.Numero)
                                                    ? contratoRDBO.Operador.Licencia.Numero.Trim().ToUpper()
                                                    : string.Empty;
                }
            }
            #endregion

            if (ReferenceEquals(contratoRDBO.LineasContrato, null))
                throw new Exception("No es posible recuperar la información de la unidad necesaria para el registro del check list. Intente de nuevo por favor.");
            
            if(contratoRDBO.LineasContrato.Count <= 0)
                throw new Exception("No es posible recuperar la información de la unidad necesaria para el registro del check list. Intente de nuevo por favor.");
            
            //Asignamos el identificador de la linea de contrato
            this.vista.LineaContratoID = contratoRDBO.LineasContrato[0].LineaContratoID;

            //Desplegamos la información de la unidad                
            this.DesplegarInformacionUnidad(contratoRDBO.LineasContrato[0].Equipo as UnidadBO);
        }
        /// <summary>
        /// Coloca el valor de los ultimos km y hrs que ha tenido la unidad
        /// </summary>
        /// <param name="ultimosKmHrs">Lista con los valores de los kilometrajes y horas</param>
        private void DesplegarUltimosKmHrs(List<Int32> ultimosKmHrs)
        {
            if (ultimosKmHrs.Count == 0)
            {
                this.vista.KilometrajeAnterior = 0;
                this.vista.HorometroAnterior = 0;
            }
            else
            {
                this.vista.KilometrajeAnterior = ultimosKmHrs[0];
                this.vista.HorometroAnterior = ultimosKmHrs[1];
            }
        }
        /// <summary>
        /// Obtiene los Ultimos KM y HRS de la Unidad
        /// </summary>
        /// <param name="unidadId"></param>
        /// <returns></returns>
        private List<Int32> ObtenerUltimosKmHrs(Int32? unidadId)
        {
            var contratoRdBr = new ContratoRDBR();
            var kmHrs = contratoRdBr.ConsultarUltimosKilometrajeHoras(this.dctx, unidadId);
            if(kmHrs == null) throw new Exception("No se obtuvieron los Kilometrajes y Horas anteriores de la Unidad. ");

            return kmHrs;
        } 
        /// <summary>
        /// Cancela el registro del check list
        /// </summary>
        public void CancelarRegistro()
        {
            this.vista.LimpiarSesion();
            this.vista.RedirigirAConsulta();
        }
        /// <summary>
        /// Registra la entrega de la unidad al cliente
        /// </summary>
        public void RegistrarEntrega()
        {
            try
            {
                string s;
                if ((s = this.ValidarCampos()) != null)
                {
                    this.vista.MostrarMensaje(s, ETipoMensajeIU.ADVERTENCIA, null);
                    return;
                }

                //Asignamos el tipo de listado
                this.vista.TipoListado = (int) ETipoListadoVerificacion.ENTREGA;
                this.Registrar();

                //Limpiamos las variables de session
                this.vista.LimpiarSesion();
                this.vista.LimpiarPaqueteNavegacion();

                //Establecemos el paquete de navegación para el imprimir check list
                var contrato = new ContratoRDBO {ContratoID = this.vista.ContratoID};

                AppSettingsReader n = new AppSettingsReader();
                int moduloID = Convert.ToInt32(n.GetValue("ModuloID", typeof(int)));

                var datos = this.controlador.ObtenerDatosCheckList(this.dctx, contrato, moduloID);
                datos.Add("ContratoRDBO", datos);

                this.vista.EstablecerPaqueteNavegacion("CU012", datos);

                this.vista.RedirigirAImprimir();
            }
            catch (Exception ex)
            {
                throw new Exception(nombreClase + ".RegistrarEntrega: " + Environment.NewLine + ex.Message);
            }
        }
        /// <summary>
        /// Registra el Check list
        /// </summary>
        private void Registrar()
        {
            #region RI0016 - Adición de proceso de generación de pagos

            #region Se inicia la Transaccion
            this.dctx.SetCurrentProvider("Outsourcing");
            Guid firma = Guid.NewGuid();
            try
            {
                this.dctx.OpenConnection(firma);
                this.dctx.BeginTransaction(firma);
            }

            catch (Exception)
            {
                if (this.dctx.ConnectionState == ConnectionState.Open)
                    this.dctx.CloseConnection(firma);
                throw new Exception("Se encontraron inconsistencias al insertar el Contrato.");
            }
            #endregion

            try
            {
                //Se obtiene la información a partir de la Interfaz de Usuario
                ContratoRDBO bo = (ContratoRDBO)this.InterfazUsuarioADato();

                //Se crea el objeto de seguridad
                UsuarioBO usuario = new UsuarioBO() { Id = this.vista.UsuarioID };
                AdscripcionBO adscripcion = new AdscripcionBO() { UnidadOperativa = new UnidadOperativaBO() { Id = this.vista.UnidadOperativaID } };
                SeguridadBO seguridadBO = new SeguridadBO(Guid.Empty, usuario, adscripcion);

                //Se inserta en la BD
                bo = this.controlador.RegistrarEntrega(this.dctx, bo, seguridadBO);

                if (bo.Estatus == EEstatusContrato.EnCurso)
                {
                    //Verificar que no se hayan creado previamente los pagos (compatibilidad hacia atras con el proceso anterior)
                    ReferenciaContratoBR referenciaContratoBR = new ReferenciaContratoBR();
                    List<ReferenciaContratoBO> referencias = referenciaContratoBR.Consultar(this.dctx, new ReferenciaContratoBO { ReferenciaContratoID = bo .ContratoID });

                    if (referencias.Count == 0)
                    {
                        GeneradorPagoRDBR generadorPago = new GeneradorPagoRDBR();
                        generadorPago.GenerarPagos(this.dctx, bo, seguridadBO, true);
                    }
                }

                this.dctx.SetCurrentProvider("Outsourcing");
                this.dctx.CommitTransaction(firma);
            }
            catch (Exception ex)
            {
                this.dctx.SetCurrentProvider("Outsourcing");
                this.dctx.RollbackTransaction(firma);

                throw new Exception(nombreClase + ".Registrar:" + ex.Message);
            }
            finally
            {
                if (this.dctx.ConnectionState == ConnectionState.Open)
                    this.dctx.CloseConnection(firma);
            }

            #endregion
        }
        /// <summary>
        /// Obtiene los valores seleccionados en la vista para las verificaciones de la llanta
        /// </summary>
        /// <returns>Listado de verificaciones de llanta</returns>
        private List<VerificacionLlantaBO> InterfazUsuarioADatoLlantas()
        {
            List<VerificacionLlantaBO> lista =  ( List<VerificacionLlantaBO>)this.vista.ObtenerValoresLlanta();

            VerificacionLlantaBO verificacion = null;

            if (this.vista.RefaccionID.HasValue)
            {
                verificacion = new VerificacionLlantaBO();
                verificacion.Llanta = new LlantaBO {LlantaID = this.vista.RefaccionID.Value, EsRefaccion = true};

                if (this.vista.RefaccionEstado.HasValue)
                {
                    verificacion.Correcto = this.vista.RefaccionEstado.Value;
                }
                lista.Add(verificacion);
            }

            return lista;
        }
        /// <summary>
        /// Obtiene la información de la unidad seleccionada
        /// </summary>
        /// <returns>Unidad a la que se le realiza el check list</returns>
        private EquipoBO InterfazUsuarioADatoUnidad()
        {
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
        /// Obtiene la información del chekc list proporcionada por el usuario
        /// </summary>
        /// <returns>Check List que se va a registrar</returns>
        private object InterfazUsuarioADato()
        {
            ContratoRDBO obj = new ContratoRDBO();
            LineaContratoRDBO linea = new LineaContratoRDBO();
            ListadoVerificacionBO bo = new ListadoVerificacionBO();

            if (this.vista.ContratoID.HasValue)
            {
                obj.ContratoID = this.vista.ContratoID.Value;
                bo.TieneContratoRenta = true;
            }
            if (this.vista.LineaContratoID.HasValue)
                linea.LineaContratoID = this.vista.LineaContratoID.Value;
            if (this.vista.TipoListado.HasValue)
                bo.Tipo = (ETipoListadoVerificacion) this.vista.TipoListado.Value;
            if (this.vista.FechaListado.HasValue)
                bo.Fecha = this.vista.FechaListado.Value;
            if (this.vista.FechaListado.HasValue && this.vista.HoraListado.HasValue)
                bo.Fecha = bo.Fecha.Value.Add(this.vista.HoraListado.Value);
            if (this.vista.UsuarioID.HasValue)
                bo.UsuarioVerifica = new UsuarioBO {Id = this.vista.UsuarioID.Value};
            if (this.vista.Kilometraje.HasValue)
                bo.Kilometraje = this.vista.Kilometraje.Value;
            if (this.vista.Horometro.HasValue)
                bo.Horometro = this.vista.Horometro.Value;
            if (this.vista.Combustible.HasValue)
                bo.Combustible = this.vista.Combustible.Value;
            if (this.vista.TieneAlarmaReversa.HasValue)
                bo.TieneAlarmasReversa = this.vista.TieneAlarmaReversa.Value;
            if (this.vista.TieneDocumentacionCompleta.HasValue)
                bo.TieneDocumentacionCompleta = this.vista.TieneDocumentacionCompleta.Value;
            if (this.vista.TieneEncendedor.HasValue)
                bo.TieneEncendedor = this.vista.TieneEncendedor.Value;
            if (this.vista.TieneEspejosCompletos.HasValue)
                bo.TieneEspejosCompletos = this.vista.TieneEspejosCompletos.Value;
            if (this.vista.TieneExtinguidor.HasValue)
                bo.TieneExtinguidor = this.vista.TieneExtinguidor.Value;
            if (this.vista.TieneGPS.HasValue)
                bo.TieneGPS = this.vista.TieneGPS.Value;
            if (this.vista.TieneGatoLlaveTuerca.HasValue)
                bo.TieneGatoLlaveTuerca = this.vista.TieneGatoLlaveTuerca.Value;
            if (this.vista.TieneInteriorLimpio.HasValue)
                bo.TieneInteriorLimpio = this.vista.TieneInteriorLimpio.Value;
            if (this.vista.TieneLimpiezaInteriorCaja.HasValue)
                bo.TieneLimpiezaInteriorCaja = this.vista.TieneLimpiezaInteriorCaja.Value;
            if (this.vista.TieneLlaveOriginal.HasValue)
                bo.TieneLlaveOriginal = this.vista.TieneLlaveOriginal.Value;
            if (this.vista.TieneStereoBocinas.HasValue)
                bo.TieneStereoBocinas = this.vista.TieneStereoBocinas.Value;
            if (this.vista.TieneTapetes.HasValue)
                bo.TieneTapetes = this.vista.TieneTapetes.Value;
            if (this.vista.TieneTresReflejantes.HasValue)
                bo.TieneTresReflejantes = this.vista.TieneTresReflejantes.Value;
            if (this.vista.TieneVestidurasLimpias.HasValue)
                bo.TieneVestidurasLimpias = this.vista.TieneVestidurasLimpias.Value;
            if (this.vista.BateriasCorrectas.HasValue)
                bo.BateriasCorrectas = this.vista.BateriasCorrectas.Value;
            if (!string.IsNullOrEmpty(this.vista.ObservacionesLlantas) && !string.IsNullOrWhiteSpace(this.vista.ObservacionesLlantas))
                bo.ObservacionesLlantas = this.vista.ObservacionesLlantas.Trim().ToUpper();
            if (!string.IsNullOrEmpty(this.vista.ObservacionesDocumentacionCompleta) &&
                !string.IsNullOrWhiteSpace(this.vista.ObservacionesDocumentacionCompleta))
                bo.ObservacionesDocumentacionCompleta = this.vista.ObservacionesDocumentacionCompleta.Trim().ToUpper();
            if (!string.IsNullOrEmpty(this.vista.ObservacionesBaterias) && !string.IsNullOrWhiteSpace(this.vista.ObservacionesBaterias))
                bo.ObservacionesBaterias = this.vista.ObservacionesBaterias.Trim().ToUpper();
            if (this.vista.TieneGolpesGeneral.HasValue)
                bo.TieneGolpesGeneral = this.vista.TieneGolpesGeneral.Value;

            bo.Auditoria = new AuditoriaBO
                {
                    FC = DateTime.Now,
                    FUA = DateTime.Now,
                    UC = this.vista.UsuarioID,
                    UUA = this.vista.UsuarioID
                };
            //Agregamos las llantas al listado de verificación
            bo.AgregarVerificacionesLlanta(this.InterfazUsuarioADatoLlantas());
            //Agregamos las secciones al listado de verificación
            if (!ReferenceEquals(this.vista.VerificacionesSeccion, null))
                if(this.vista.VerificacionesSeccion.Count > 0)
                    bo.AgregarVerificacionesSeccion(this.vista.VerificacionesSeccion.ConvertAll(x => (VerificacionSeccionBO)x));
            
            //agregamos los archivos adjuntos al listado
            bo.Adjuntos = this.presentadorDocumentos.Vista.NuevosArchivos;
            
            if(ReferenceEquals(linea.ListadosVerificacion, null))
                linea.ListadosVerificacion = new List<ListadoVerificacionBO>();

            if(!bo.Tipo.HasValue)
                bo.Tipo = ETipoListadoVerificacion.ENTREGA;

            //Agregamos el listado de verificación a la linea
            linea.AgregarListadoVerificacion(bo);
            
            //Agregamos la unidad a la linea
            linea.Equipo = this.InterfazUsuarioADatoUnidad();

            if(ReferenceEquals(obj.LineasContrato, null))
                obj.LineasContrato = new List<ILineaContrato>();

            //Agregamos la línea al contrato
            obj.AgregarLineaContrato(linea);

            return obj;
        }        
        /// <summary>
        /// Valida la información proporcionada por el usuario
        /// </summary>
        /// <returns>Inconsistencias en la información</returns>
        private string ValidarCampos()
        {
            StringBuilder s = new StringBuilder();
            
            string sf = null;
            if ((sf = this.ValidarFechasListado()) != null)
                return sf;

            if ((sf = this.ValidarObservacionesBaterias()) != null)
                return sf;

            if ((sf = this.ValidarObservacionesDocumentacion()) != null)
                return sf;

            if ((sf = this.ValidarObservacionesSeccion()) != null)
                return sf;

            if ((sf = this.ValidarTanque()) != null)
                return sf;

            if((sf = this.ValidarKilometros()) != null)
                return sf;

            if (!vista.LineaContratoID.HasValue)
                s.Append("Contrato, ");
            if (!vista.UsuarioID.HasValue)
                s.Append("Usuario que entrega la unidad, ");
            if (!vista.Kilometraje.HasValue)
                s.Append("Kilometraje de entrega, ");
            if (!vista.Horometro.HasValue)
                s.Append("Horas de refrigeración de entrega, ");
            if (!vista.Combustible.HasValue)
                s.Append("Nivel de combustible, ");
            if (!vista.TieneAlarmaReversa.HasValue)
                s.Append("Alarma de reversa, ");
            if (!vista.TieneDocumentacionCompleta.HasValue)
                s.Append("Documentación completa, ");
            if (!vista.TieneEncendedor.HasValue)
                s.Append("Encendedor, ");
            if (!vista.TieneEspejosCompletos.HasValue)
                s.Append("Espejos completos, ");
            if (!vista.TieneExtinguidor.HasValue)
                s.Append("Extinguidor, ");
            if (!vista.TieneGPS.HasValue)
                s.Append("GPS, ");
            if (!vista.TieneGatoLlaveTuerca.HasValue)
                s.Append("Gato y llaves de tuerca, ");
            if (!vista.TieneInteriorLimpio.HasValue)
                s.Append("Interior limpio, ");
            if (!vista.TieneLimpiezaInteriorCaja.HasValue)
                s.Append("Limpieza interior cajas, ");
            if (!vista.TieneLlaveOriginal.HasValue)
                s.Append("Llave original, ");
            if (!vista.TieneStereoBocinas.HasValue)
                s.Append("Estereo y Bbcinas, ");
            if (!vista.TieneTapetes.HasValue)
                s.Append("Tapetes, ");
            if (!vista.TieneTresReflejantes.HasValue)
                s.Append("Tres reflejantes, ");
            if (!vista.TieneVestidurasLimpias.HasValue)
                s.Append("Vestiduras limpias, ");
            if(!vista.BateriasCorrectas.HasValue)
                s.Append("Baterias correctas, ");

            if(s.Length > 0 )
                return "Los siguientes campos no pueden estar vacíos: \n" + s.ToString().Substring(0, s.Length - 2);
            return null;
        }
        /// <summary>
        /// Valida las fechas proporcionadas por el usuario para el check list
        /// </summary>
        /// <returns>Campos que presenten inconsistencias</returns>
        private string ValidarFechasListado()
        {
            if(!this.vista.FechaListado.HasValue)
                return " Es necesario proporcionar la fecha en la que se realiza el check list. ";

            if(!this.vista.HoraListado.HasValue)
                return " Es necesario proporcionar la hora en la que se realiza el check list.  ";

            if(!this.vista.FechaContrato.HasValue)
                return " Es necesario proporcionar la fecha del contrato para el registro del check list. ";

            if(!this.vista.HoraContrato.HasValue)
                return " Es necesario proporcionar la hora del contrato para el registro del check list. ";

            var date = this.vista.FechaListado;
            date = date.Value.Add(this.vista.HoraListado.Value);

            if(date.Value < this.vista.FechaContrato.Value)
                return " La fecha de registro del check list no puede ser menor a la fecha de registro del contrato. ";
            
            return null;
        }
        /// <summary>
        /// Valida las observaciones de la sección
        /// </summary>
        /// <returns>Campos que presenten inconsistencias</returns>
        private string ValidarObservacionesSeccion()
        {
            if (this.vista.TieneGolpesGeneral.HasValue)
            {
                if (this.vista.TieneGolpesGeneral.Value)
                {
                    if (ReferenceEquals(this.vista.VerificacionesSeccion, null))
                        return " Si la unidad presenta golpes en general es necesario registrar una observación en el área de observaciones de sección de la unidad. ";
                    if(this.vista.VerificacionesSeccion.Count <= 0)
                        return " Si la unidad presenta golpes en general es necesario registrar una observación en el área de observaciones de sección de la unidad. ";
                }
            }

            return null;
        }
        /// <summary>
        /// Valida la observación realizada sobre la documentación
        /// </summary>
        /// <returns>Campos que presenten inconsistencias</returns>
        private string ValidarObservacionesDocumentacion()
        {
            if (this.vista.TieneDocumentacionCompleta.HasValue)
            {
                if (!this.vista.TieneDocumentacionCompleta.Value &&
                    (string.IsNullOrEmpty(this.vista.ObservacionesDocumentacionCompleta) ||
                     string.IsNullOrWhiteSpace(this.vista.ObservacionesDocumentacionCompleta)))
                    return " Sí la documentación no se encuentra completa, es necesario proporcionar una observación para continuar con el registro. ";
            }

            return null;
        }
        /// <summary>
        /// Valida las observaciones realizadas a las baterias
        /// </summary>
        /// <returns>Campos que presenten inconsistencias</returns>
        private string ValidarObservacionesBaterias()
        {
            if (this.vista.BateriasCorrectas.HasValue)
            {
                if (!this.vista.BateriasCorrectas.Value &&
                    (string.IsNullOrEmpty(this.vista.ObservacionesBaterias) ||
                     string.IsNullOrWhiteSpace(this.vista.ObservacionesBaterias)))
                    return " Sí las baterias no se encuentran en buen estado, es necesario proporcionar una observación para continuar con el registro. ";
            }

            return null;
        }
        /// <summary>
        /// Valida los campos de observación para las secciones de la unidad
        /// </summary>
        /// <returns>Campos que presenten inconsistencias</returns>
        private string ValidarSeccionUnidad()
        {
            StringBuilder s = new StringBuilder();
            if (!this.vista.SeccionUnidadID.HasValue)
                s.Append("Sección, ");
            if (string.IsNullOrEmpty(this.vista.ObservacionesSeccion) || string.IsNullOrWhiteSpace(this.vista.ObservacionesSeccion))
                s.Append("Observaciones de Sección, ");

            if(s.Length > 0 )
            return "Los siguientes campos no pueden estar vacíos: \n" + s.ToString().Substring(0, s.Length - 2) + ".";
            return null;
        }        
        /// <summary>
        /// Valida la capacidad del tanque de la unidad respecto al combustible proporcionado
        /// </summary>
        /// <returns>Mensaje de advertencia</returns>
        public string ValidarTanque()
        {
            if (!this.vista.CapacidadTanque.HasValue)
                return "Es necesario proporcionar la capacidad total del tanque de combustible de la unidad que se esta entregando.";
            if (!this.vista.Combustible.HasValue)
                return "Es necesario proporcionar la cantidad actual de combustible en la unidad que se esta entregando";
            if(this.vista.Combustible.Value < 0)
                return "Es necesario proporcionar la cantidad actual de combustible en la unidad que se esta entregando";
            if (this.vista.Combustible.Value > this.vista.CapacidadTanque.Value)
                return string.Format(" La cantidad actual de combustible es superior a la capacidad total del tanque de la unidad. La capacidad total es: {0}", string.Format("{0:#,##0.00##}",this.vista.CapacidadTanque.Value));    

            return null;
        }
        /// <summary>
        /// Valida que la cantidad de kilometraje proporcionado no sea Menor que el ultimo registrado para esa unidad
        /// </summary>
        public String ValidarKilometros()
        {
            if (!this.vista.Kilometraje.HasValue)
                return null;
            if (this.vista.Kilometraje < this.vista.KilometrajeAnterior)
                return String.Format("La cantidad de kilometraje proporcionada no puede ser menor que el Último Kilometraje registrado para la Unidad. Kilometraje anterior: {0}", this.vista.KilometrajeAnterior);

            return null;
        }
        /// <summary>
        /// Valida que la cantidad de Horas proporcionado no sea Menor que el ultimo registrado para esa unidad
        /// </summary>
        public String ValidarHoras()
        {
            if(!this.vista.Horometro.HasValue)
                return null;
            if(this.vista.Horometro < this.vista.HorometroAnterior)
                return String.Format("Estas ingresando una cantidad de horas MENOR a las del Último Contrato para la Unidad, el cual fue de: {0}", this.vista.HorometroAnterior);

            return null;
        }
        /// <summary>
        /// Valida si el usuario tiene permiso para registrar el check list
        /// </summary>
        private void EstablecerSeguridad()
        {
            try
            {
                //se valida que los datos del usuario y la unidad operativa no sean nulos
                if (this.vista.UsuarioID == null) throw new Exception("El identificador del usuario no debe ser nulo. ");
                if (this.vista.UnidadOperativaID == null) throw new Exception("El identificador de la Unidad Operativa no debe ser nulo. ");

                //Se crea el objeto de seguridad
                UsuarioBO usr = new UsuarioBO { Id = this.vista.UsuarioID };
                AdscripcionBO sdscripcion = new AdscripcionBO { UnidadOperativa = new UnidadOperativaBO { Id = this.vista.UnidadOperativaID } };
                SeguridadBO seguridadBO = new SeguridadBO(Guid.Empty, usr, sdscripcion);

                //Se obtienen las acciones a las cuales el usuario tiene permiso en este proceso
                List<CatalogoBaseBO> lst = FacadeBR.ConsultarAccion(this.dctx, seguridadBO);

                //Se valida si el usuario tiene permiso para registrar el check list
                if (!this.ExisteAccion(lst, "REGISTRARENTREGA") ||
                    !this.ExisteAccion(lst, "GENERARPAGOS"))
                    this.vista.RedirigirSinPermisoAcceso();               
            }
            catch (Exception ex)
            {
                throw new Exception(nombreClase + ".EstablecerSeguridad:" + Environment.NewLine + ex.Message);
            }
        }
        /// <summary>
        /// Consulta en el listado de acciones configuradas una acción especifica
        /// </summary>
        /// <param name="acciones">Acciones configuradas</param>
        /// <param name="nombreAccion">Accion que se desea validar</param>
        /// <returns>Verdadero si existe,  falso si no existe</returns>
        private bool ExisteAccion(List<CatalogoBaseBO> acciones, string nombreAccion)
        {
            if (acciones != null && acciones.Count > 0 && acciones.Exists(p => p != null && p.Nombre != null && p.Nombre.Trim().ToUpper() == nombreAccion.Trim().ToUpper()))
                return true;

            return false;
        }
        /// <summary>
        /// Valida la extención de un archivo para confirmar que sea una imagen
        /// </summary>
        /// <param name="extencion">Extención que se desea validar</param>
        /// <returns>Verdadero si cumple con las extenciones configuradas, falso si no</returns>
        private bool ValidaExtensionImagen(string extencion)
        {
            switch (extencion)
            {
                case "jpg": case "jpeg": case "png": case "gif": case "bmp":
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
        private TipoArchivoBO ValidarArchivo(String tipo)
        {
            List<TipoArchivoBO> tiposArchivo = (List<TipoArchivoBO>)this.vista.TiposArchivoImagen;
            if (tiposArchivo != null)
            {
                TipoArchivoBO tipoArchivoTemp = tiposArchivo.Find(delegate(TipoArchivoBO ta) { return ta.Extension == tipo; });
                if (tipoArchivoTemp != null)
                {
                    return tipoArchivoTemp;
                }
                else
                {
                    this.vista.MostrarMensaje("El archivo no fue cargado.", ETipoMensajeIU.ERROR,
                        "La extensión del archivo no se encuentra en la lista de tipos de archivo permitidos.");
                }
            }
            else
            {
                this.vista.MostrarMensaje("El archivo no fue cargado.", ETipoMensajeIU.ERROR, "No hay una lista de tipos de archivo cargada.");
            }
            return null;
        }
        /// <summary>
        /// Agrega la imagen seleccionada al listado de imagenes del check list
        /// </summary>
        public void AgregarArchivoImagen()
        {            
            if (ValidaExtensionImagen(this.vista.ExtencionArchivoImagen))
            {
                ArchivoBO bo = new ArchivoBO();
                bo.Nombre = this.vista.NombreArchivoImagen;
                bo.Activo = true;
                bo.TipoAdjunto = ETipoAdjunto.VerificacionSeccionRD;
                bo.Archivo = this.vista.ObtenerArchivosBytes();
                bo.Observaciones = "Imagen Sección " + this.vista.SeccionUnidadID;
                TipoArchivoBO tipoArchivo = ValidarArchivo(this.vista.ExtencionArchivoImagen);
                if (tipoArchivo != null)
                    bo.TipoArchivo = tipoArchivo;

                List<ArchivoBO> archivos = (List<ArchivoBO>)this.vista.NuevosArchivos;
                
                if(ReferenceEquals(archivos, null))
                    archivos = new List<ArchivoBO>();

                archivos.Add(bo);
                
                this.vista.NuevosArchivos = archivos;
                this.vista.CargarImagenes();
            }
            else
            {
                this.vista.MostrarMensaje("El archivo seleccionado no es de tipo imagen.", ETipoMensajeIU.ADVERTENCIA, null);   
            }
        }
        /// <summary>
        /// Agrega la sección seleccionada al listado de secciones del check list
        /// </summary>
        public void AgregarVerificacionSeccion()
        {
            if (!this.vista.SeccionUnidadID.HasValue)
                return;

            if (!this.ValidarNuevaSeccionUnidad(this.vista.SeccionUnidadID.Value))
            {
                var secciones = this.vista.VerificacionesSeccion.ConvertAll(x => (VerificacionSeccionBO)x);

                string s;
                if ((s = this.ValidarSeccionUnidad()) != null)
                {
                    this.vista.MostrarMensaje(s, ETipoMensajeIU.ADVERTENCIA, null);
                    return;
                }

                VerificacionSeccionBO obj = new VerificacionSeccionBO();

                if (this.vista.SeccionUnidadID.HasValue)
                    obj.Numero = this.vista.SeccionUnidadID.Value;
                if (!string.IsNullOrEmpty(this.vista.ObservacionesSeccion) && !string.IsNullOrWhiteSpace(this.vista.ObservacionesSeccion))
                    obj.Observacion = this.vista.ObservacionesSeccion.Trim().ToUpper();
                if (!ReferenceEquals(this.vista.NuevosArchivos, null))
                {
                    var archivoBos = this.vista.NuevosArchivos as List<ArchivoBO>;
                    if (archivoBos != null)
                    {
                        foreach (var archivo in archivoBos)
                        {
                            ArchivoBO item = new ArchivoBO();
                            item.Activo = archivo.Activo;
                            item.Archivo = archivo.Archivo;
                            item.Auditoria = archivo.Auditoria;
                            item.Id = archivo.Id;
                            item.Nombre = archivo.Nombre;
                            item.NombreCorto = archivo.NombreCorto;
                            item.Observaciones = archivo.Observaciones;
                            item.TipoAdjunto = archivo.TipoAdjunto;
                            item.TipoArchivo = archivo.TipoArchivo;

                            obj.AgregarArchivo(item);
                        }
                    }
                }

                secciones.Add(obj);

                this.vista.VerificacionesSeccion = secciones.ConvertAll(x => (object)x);
                this.vista.LimpiarDatosSeccionUnidad();
                this.vista.CargarSeccionesVerificacion();
            }
            else
            {
                this.vista.MostrarMensaje("Ya fue agregada una observación para la sección seleccionada.", ETipoMensajeIU.ADVERTENCIA, null);
            }
        }
        /// <summary>
        /// Valida que una seccion no sea agregada mas de una vez al check list
        /// </summary>
        /// <param name="seccionID">Identificador de la sección que se desea agregar</param>
        /// <returns>verdadero si la encuentra, falso si no</returns>
        public bool ValidarNuevaSeccionUnidad(int seccionID)
        {
            var secciones = this.vista.VerificacionesSeccion.ConvertAll(x => (VerificacionSeccionBO)x);
            var seccion = secciones.FirstOrDefault(x => x.Numero.Value == seccionID);

            if (ReferenceEquals(seccion, null))
                return false;

            if (!seccion.Numero.HasValue)
                return false;

            return true;
        }
        /// <summary>
        /// Elimina un archivo del listado de imagenes seleccionadas
        /// </summary>
        /// <param name="index">Indice de la imagen que se desa eliminar</param>
        public void EliminaArchivo(int index)
        {
            var archivos = this.vista.NuevosArchivos as List<ArchivoBO>;
            
            if (ReferenceEquals(archivos, null)) return;

            if(index > archivos.Count)
                throw new Exception(nombreClase + ".EliminaArchivo: El archivo que se intenta eliminar no se encuentra entre el listado de Imagenes seleccionadas.");

            var archivo = archivos[index];

            if (archivo != null)
            {
                if (archivos.Contains(archivo))
                {
                    if (archivo.Id.HasValue)
                        archivo.Activo = false;
                    else
                        archivos.Remove(archivo);

                    this.vista.NuevosArchivos = archivos;
                    this.vista.CargarImagenes();   
                }
            }
        }
        /// <summary>
        /// Elimina una sección del listado de secciones agregadas al check list
        /// </summary>
        /// <param name="index">Indice de la sección que se desea eliminar</param>
        public void EliminarSeccion(int index)
        {
            if (this.vista.VerificacionesSeccion != null)
            {
                if (this.vista.VerificacionesSeccion.Count > 0)
                {
                    if(index > this.vista.VerificacionesSeccion.Count)
                        throw new Exception(nombreClase + ".EliminarSeccion: La sección que se intenta eliminar no se encuentra entre el listado de secciones agregadas.");
                    var secciones = this.vista.VerificacionesSeccion.ConvertAll(x => (VerificacionSeccionBO)x);
                    var seccion = secciones[index];

                    if (seccion != null)
                    {
                        if (secciones.Contains(seccion))
                        {
                            secciones.Remove(seccion);
                            this.vista.VerificacionesSeccion = secciones.ConvertAll(x => (object) x);
                            this.vista.CargarSeccionesVerificacion();
                        }
                    }                    
                }
            }            
        }
        /// <summary>
        /// Despliega en la vista el listado de imagenes cargadas a la sección
        /// </summary>
        /// <param name="index">Indice de la sección que se desea visualizar</param>
        public void MostrarDetalleImagenesSeccion(int index)
        {
            if (index > this.vista.VerificacionesSeccion.Count || index < 0)
            {
                this.vista.MostrarMensaje("No se pueden desplegar los detalles de las imagenes para la sección.", ETipoMensajeIU.ADVERTENCIA, null);
                return;
            }
            var seccion = (VerificacionSeccionBO)this.vista.VerificacionesSeccion[index];
            this.vista.CargarDetalleImagenSeccion((object)seccion.Adjuntos);
        }
        /// <summary>
        /// Cambia la página en la lista de secciones
        /// </summary>
        /// <param name="p">Página que se desea visualizar</param>
        public void CambiarPaginaSecciones(int p)
        {
            this.vista.IndicePaginaSecciones = p;
            this.vista.ActualizarSecciones();
        }
        #endregion
    }
}