// Satisface al CU080 – Editar Acta de Nacimiento de una Unidad
//Satisface la solicitud de cambio SC0006

using System;
using System.Collections.Generic;
using System.Linq;
using BPMO.Basicos.BO;
using BPMO.Facade.SDNI.BR;
using BPMO.Patterns.Creational.DataContext;
using BPMO.Primitivos.Enumeradores;
using BPMO.SDNI.Comun.BO;
using BPMO.SDNI.Comun.BR;
using BPMO.SDNI.Equipos.BO;
using BPMO.SDNI.Equipos.BR;
using BPMO.SDNI.Equipos.VIS;
using BPMO.SDNI.Tramites.BO;
using BPMO.SDNI.Tramites.PRE;
using BPMO.SDNI.Tramites.VIS;
using BPMO.Servicio.Catalogos.BO;
using System.Collections;
using BPMO.SDNI.Equipos.BOF;
using System.Configuration;
using BPMO.Facade.SDNI.BO;
using System.Web.UI.WebControls;
using BPMO.SDNI.Tramites.BR;
using System.Data;
using BPMO.SDNI.Comun.PRE;

namespace BPMO.SDNI.Equipos.PRE
{
    public class EditarActaNacimientoPRE
    {
        #region Atributos
        private UnidadBR controlador;
        private IDataContext dctx = null;

        private IEditarActaNacimientoVIS vista;
        private IucDatosGeneralesVIS vistaPagina1;
        private IucDatosTecnicosVIS vistaPagina2;
        private IucNumerosSerieVIS vistaPagina3;
        private IucAsignacionLlantasVIS vistaPagina4;
        private IucAsignacionEquiposAliadosVIS vistaPagina5;
        private IucTramitesActivosVIS vistaPagina6;
        private IucResumenActaNacimientoVIS vistaPagina7;

        private ucDatosGeneralesPRE presentadorPagina1;
        private ucDatosTecnicosPRE presentadorPagina2;
        private ucNumerosSeriePRE presentadorPagina3;
        private ucAsignacionLlantasPRE presentadorPagina4;
        private ucAsignacionEquiposAliadosPRE presentadorPagina5;
        private ucTramitesActivosPRE presentadorPagina6;
        private ucResumenActaNacimientoPRE presentadorPagina7;
        private ucCatalogoDocumentosPRE documentos;
        private TipoArchivoBR tipoArchivoBR = new TipoArchivoBR(); //RQM 13285
        private ArchivoBR archivoBR = new ArchivoBR();
        private string nombreClase = "EditarActaNacimientoPRE";

        /// <summary>
        /// Identificador de la Caracteristica SERIE  DE MOTOR
        /// </summary>
        public Int32? SerieMotorId
        {
            get
            {
                try
                {
                    return int.Parse(ConfigurationManager.AppSettings["SerieMotorId"]);
                }
                catch(Exception)
                {
                    return null;
                }
            }
        }
        #endregion

        #region Constructores
        public EditarActaNacimientoPRE(IEditarActaNacimientoVIS view, IucDatosGeneralesVIS viewPage1, IucDatosTecnicosVIS viewPage2, IucNumerosSerieVIS viewPage3, IucAsignacionLlantasVIS viewPage4, IucAsignacionEquiposAliadosVIS viewPage5, IucTramitesActivosVIS viewPage6, IucResumenActaNacimientoVIS viewPage7)
        {
            try
            {
                this.vista = view;
                this.vistaPagina1 = viewPage1;
                this.vistaPagina2 = viewPage2;
                this.vistaPagina3 = viewPage3;
                this.vistaPagina4 = viewPage4;
                this.vistaPagina5 = viewPage5;
                this.vistaPagina6 = viewPage6;
                this.vistaPagina7 = viewPage7;

                this.presentadorPagina1 = new ucDatosGeneralesPRE(viewPage1, documentos);
                this.presentadorPagina2 = new ucDatosTecnicosPRE(viewPage2);
                this.presentadorPagina3 = new ucNumerosSeriePRE(viewPage3);
                this.presentadorPagina4 = new ucAsignacionLlantasPRE(viewPage4);
                this.presentadorPagina5 = new ucAsignacionEquiposAliadosPRE(viewPage5);
                this.presentadorPagina6 = new ucTramitesActivosPRE(viewPage6);
                this.presentadorPagina7 = new ucResumenActaNacimientoPRE(viewPage7);

                this.controlador = new UnidadBR();
                this.dctx = FacadeBR.ObtenerConexion();
            }
            catch (Exception ex)
            {
                this.vista.MostrarMensaje("Inconsistencias en los parámetros de configuración", ETipoMensajeIU.ERROR, this.nombreClase + ".RegistrarActaNacimientoPRE:" + ex.Message);
            }
        }
        #endregion

        #region Métodos
        public void RealizarPrimeraCarga()
        {
            this.ObtenerAcciones();
            this.EstablecerDatosNavegacion(this.vista.ObtenerDatosNavegacion());
            this.LimpiarSesion();
            this.PrepararEdicion();

            this.EstablecerInformacionInicial();
            this.ConsultarCompleto();
            this.EstablecerSeguridad();
            this.EstablecerAcciones();
        }
        private void EstablecerDatosNavegacion(object paqueteNavegacion)
        {
            IDictionary parameters = new Hashtable();
            try
            {
                if (paqueteNavegacion == null)
                    throw new Exception("Se esperaba un objeto en la navegación. No se puede identificar qué acta de nacimiento se desea consultar.");
                if (!(paqueteNavegacion is BO.UnidadBO))
                    throw new Exception("Se esperaba una Unidad de Idealease.");

                #region SC0006 - Adición de datos de siniestro
                BO.UnidadBO bo = (BO.UnidadBO)paqueteNavegacion;
                parameters["UnidadBO"] = bo;
                parameters["SiniestroUnidadBO"] = this.RecuperarHistorialSiniestro(bo);                
                this.DatoAInterfazUsuario(parameters);
                #endregion
            }
            catch (Exception ex)
            {
                #region SC0006 - Adición de datos de siniestro
                parameters["UnidadBO"] = new BO.UnidadBO();
                this.DatoAInterfazUsuario(parameters);
                #endregion
                throw new Exception(this.nombreClase + ".EstablecerDatosNavegacion: " + ex.Message);
            }
        }
        private void EstablecerInformacionInicial()
        {
            try
            {
                #region Configuraciones de Unidad Operativa
                //Obtener las configuraciones de la unidad operativa
                List<ConfiguracionUnidadOperativaBO> lstConfigUO = new ModuloBR().ConsultarConfiguracionUnidadOperativa(this.dctx, new ConfiguracionUnidadOperativaBO() { UnidadOperativa = new UnidadOperativaBO() { Id = this.vista.UnidadOperativaId } }, this.vista.ModuloID);
                if (lstConfigUO.Count <= 0)
                    throw new Exception("No se encontraron las configuraciones del sistema para la unidad operativa en la que trabaja.");

                //Establecer las configuraciones de la unidad operativa
                this.vista.NombreClienteUnidadOperativa = lstConfigUO[0].NombreCliente;
                this.vista.LibroActivos = lstConfigUO[0].Libro;

                if(string.IsNullOrEmpty(this.vista.LibroActivos) || string.IsNullOrWhiteSpace(this.vista.LibroActivos))
                    throw new Exception("No se encuentra configurado el nombre del libro correspondiente a la unidad operativa.");
                if (string.IsNullOrEmpty(this.vista.NombreClienteUnidadOperativa) || string.IsNullOrWhiteSpace(this.vista.NombreClienteUnidadOperativa))
                    throw new Exception("No se encuentra configurado el nombre del cliente de la unidad operativa.");
                #endregion
            }
            catch (Exception ex)
            {
                throw new Exception(this.nombreClase + ".EstablecerInformacionInicial: " + ex.Message);
            }
        }
        public void PrepararEdicion()
        {
            this.LimpiarSesion();

            this.vista.PrepararEdicion();
            this.presentadorPagina1.PrepararEdicion();
            this.presentadorPagina2.PrepararEdicion();
            this.presentadorPagina3.PrepararEdicion();
            this.presentadorPagina4.PrepararEdicion();
            this.presentadorPagina5.PrepararEdicion();
            this.presentadorPagina6.OcultarRedireccionTramites(true);
            this.presentadorPagina6.HabilitarPedimento(true);
            this.presentadorPagina7.PrepararEdicion();

            #region[REQ: 13285, Integración Generación y Construcción]
            if (this.vista.UnidadOperativaId == (int)ETipoEmpresa.Idealease)
            {
                this.presentadorPagina1.HabilitarCargaArchivoOC(false);
                this.presentadorPagina6.HabilitarCargaArchivo(false);
            }
            if (this.vista.UnidadOperativaId == (int)ETipoEmpresa.Construccion || this.vista.UnidadOperativaId == (int)ETipoEmpresa.Generacion || this.vista.UnidadOperativaId == (int)ETipoEmpresa.Equinova)
            {
                List<TipoArchivoBO> lstTipos = tipoArchivoBR.Consultar(dctx, new TipoArchivoBO { Estatus = true });
                this.presentadorPagina1.DesplegarTiposArchivos(lstTipos);
                this.presentadorPagina1.HabilitarCargaArchivoOC(true);
                this.presentadorPagina6.EstablecerTiposdeArchivo(lstTipos);                
                this.presentadorPagina6.HabilitarCargaArchivo(true);
            }

            #endregion

            this.vista.PermitirRegresar(false);
            this.vista.PermitirContinuar(true);

            this.IrAPagina(1);
        }
        public void EstablecerConfiguracionEspecialVista()
        {
            this.presentadorPagina4.EstablecerConfiguracionEspecialVista();
        }
        public void EstablecerSeguridad()
        {
            try
            {
                #region SC0008

                //Valida que el usuario y la unidad operativva no sean nulos
                if (this.vista.UUA == null) throw new Exception("El identificador del usuario no debe ser nulo");
                if (this.vista.UnidadOperativaId == null)
                    throw new Exception("El identificador de la Unidad Operativa no debe ser nulo");

                if (this.vista.ListaAcciones == null)
                {
                    // Creación del objeto seguridad
                    UsuarioBO usuario = new UsuarioBO { Id = this.vista.UUA };
                    AdscripcionBO adscripcion = new AdscripcionBO
                        {
                            UnidadOperativa = new UnidadOperativaBO { Id = this.vista.UnidadOperativaId }
                        };
                    SeguridadBO seguridad = new SeguridadBO(Guid.Empty, usuario, adscripcion);

                    //Consulta de lista de acciones a las que tiene permiso el usuario
                    this.vista.ListaAcciones = FacadeBR.ConsultarAccion(dctx, seguridad);
                }
                // se valida si el usuario tiene permisos para registrar un nuevo acta de nacimiento
                if (!this.ExisteAccion(this.vista.ListaAcciones, "REGISTRARDOCUMENTO"))
                    this.vista.PermitirRegistrar(false);
                //Se valida si el usuario tiene permisos para terminar un acta de nacimiento
                if (!this.ExisteAccion(this.vista.ListaAcciones, "UI TERMINARDOCUMENTO"))
                    this.vista.PermitirGuardarTerminada(false);

                #endregion

                //Establecer las sucursales permitidas en las vistas correspondientes
                this.vista.SucursalesSeguridad =
                    FacadeBR.ConsultarSucursalesSeguridad(this.dctx,
                                                          new SeguridadBO(Guid.Empty,
                                                                          new UsuarioBO() { Id = this.vista.UUA },
                                                                          new AdscripcionBO
                                                                              {
                                                                                  UnidadOperativa =
                                                                                      new UnidadOperativaBO()
                                                                                          {
                                                                                              Id =
                                                                                                  this.vista
                                                                                                      .UnidadOperativaId
                                                                                          }
                                                                              })).ConvertAll(s => s.Id);
            }
            catch (Exception ex)
            {
                throw new Exception(this.nombreClase + ".EstablecerSeguridad:" + ex.Message);
            }

        }
        #region SC0008
        public void ValidarAcceso()
        {
            try
            {
                //Valida que usuario y la unidad operativa no sean nulos
                if (this.vista.UUA == null) throw new Exception("El identificador del usuario no debe ser nulo");
                if (this.vista.UnidadOperativaId == null) throw new Exception("El identificador de la Unidad Operativa no debe ser nulo");
                //Se crea el objeto de seguridad
                UsuarioBO usuario = new UsuarioBO() { Id = this.vista.UUA };
                AdscripcionBO adscripcion = new AdscripcionBO() { UnidadOperativa = new UnidadOperativaBO() { Id = this.vista.UnidadOperativaId } };
                SeguridadBO seguridadBO = new SeguridadBO(Guid.Empty, usuario, adscripcion);

                //Se valida si el usuario tiene permiso a la acción principal
                if (!FacadeBR.ExisteAccion(this.dctx, "ACTUALIZARDOCUMENTO", seguridadBO))
                    this.vista.RedirigirSinPermisoAcceso();
            }
            catch (Exception ex)
            {
                throw new Exception(this.nombreClase + ".ValidarAcceso: " + ex.Message);
            }
        }
        private bool ExisteAccion(List<CatalogoBaseBO> acciones, string nombreAccion)
        {
            if (acciones != null && acciones.Count > 0 &&
                acciones.Exists(
                    p => p != null && p.Nombre != null && p.Nombre.Trim().ToUpper() == nombreAccion.Trim().ToUpper()))
                return true;

            return false;
        }
        #endregion

        public void RetrocederPagina()
        {
            int? paginaActual = this.vista.PaginaActual;
            if (paginaActual == null)
                throw new Exception("La página actual es nula.");
            if (paginaActual <= 1)
                throw new Exception("La página actual es menor o igual a uno y, por lo tanto, no se puede retroceder.");

            this.EstablecerOpcionesSegunPagina(paginaActual.Value - 1);

            this.vista.EstablecerPagina(paginaActual.Value - 1);

            //RQM 13285, modificación para determinar si al retroceder no se tiene acceso o esta oculto el tab.
            if (this.vista.ValoresTabs.Contains((paginaActual.Value - 2).ToString()))
            {
                RetrocederPagina();
            }

        }
        public void AvanzarPagina()
        {
            int? paginaActual = this.vista.PaginaActual;
            if (paginaActual == null)
                throw new Exception("La página actual es nula.");
            if (paginaActual >= 6)
                throw new Exception("La página actual es mayor o igual a 6 y, por lo tanto, no se puede avanzar.");

            this.EstablecerOpcionesSegunPagina(paginaActual.Value + 1);

            this.vista.EstablecerPagina(paginaActual.Value + 1);

            //RQM 13285, modificación para determinar si al avanzar no se tiene acceso o esta oculto el tab.
            if (this.vista.ValoresTabs.Contains(paginaActual.Value.ToString()))
            {
                AvanzarPagina();
            }
        }
        public void IrAPagina(int numeroPagina)
        {
            if (numeroPagina < 1 || numeroPagina > 7)
                throw new Exception("La paginación va de 1 al 7.");

            this.EstablecerOpcionesSegunPagina(numeroPagina);

            this.vista.EstablecerPagina(numeroPagina);
        }
        private void EstablecerOpcionesSegunPagina(int numeroPagina)
        {
            this.vista.PermitirRegresar(true);
            this.vista.PermitirContinuar(true);
            this.vista.PermitirGuardarBorrador(true);
            this.vista.PermitirCancelar(true);
            this.vista.OcultarContinuar(false);
            this.vista.OcultarTerminar(true);

            if (numeroPagina <= 1)
            {
                this.vista.PermitirRegresar(false);
            }

            if (numeroPagina == 6)
            {
                this.vista.PermitirContinuar(false);
                this.vista.OcultarContinuar(true);
                this.vista.OcultarTerminar(false);
            }

            if (numeroPagina >= 7)
            {
                this.vista.PermitirRegresar(false);
                this.vista.PermitirContinuar(false);
                this.vista.PermitirGuardarBorrador(false);
                this.vista.PermitirCancelar(false);
                this.vista.OcultarContinuar(false);
                this.vista.OcultarTerminar(true);
            }

            if (numeroPagina == 5)
            {
                this.vistaPagina5.ActualizarEquiposAliados();
                this.vistaPagina1.EstablecerEntraMantenimeinto();
            }
        }

        public void CancelarEdicion()
        {
            this.LimpiarSesion();
            this.vista.RedirigirAConsulta();
        }
        public int EditarBorrador()
        {
            string s;
            int paquete = 0;
            #region SC_0027
            //Se verifica que el código de la refacción no esté repetido
            if ((s = this.presentadorPagina4.VerificarExistenciaCodigo(true)) != null)
            {
                this.vista.MostrarMensaje(s, ETipoMensajeIU.CONFIRMACION, "BORRADOR");
                return paquete;
            }
            #endregion

            if ((s = this.ValidarCamposBorrador()) != null)
            {
                this.vista.MostrarMensaje(s, ETipoMensajeIU.ADVERTENCIA, null);
                return paquete;
            }

            this.vista.EstatusUnidad = EEstatusUnidad.NoDisponible;
            string observaciones = "Borrador guardado. Sucursal: " + (this.vista.SucursalNombre != null ? this.vista.SucursalNombre : "");

            this.Editar(observaciones);

            this.IrAPagina(7);
            #region SC0002
            string mensaje = this.ComprobarPaquetesMantenimiento();
            if (mensaje == string.Empty)
                this.vista.MostrarMensaje("Edición Exitosa", ETipoMensajeIU.EXITO, null);
            else
            {
                paquete = 1;
                this.vista.Mensaje = mensaje;
                //this.vista.MostrarMensaje("Registro Exitoso", ETipoMensajeIU.ERROR, mensaje);
            }
            #endregion
            return paquete;
            //this.vista.MostrarMensaje("Edición Exitosa", ETipoMensajeIU.EXITO, null);
        }
        public int EditarTerminada()
        {
            string s;
            int paquete = 0;
            #region SC_0027
            //Se verifica que el código de la refacción no esté repetido
            if ((s = this.presentadorPagina4.VerificarExistenciaCodigo(true)) != null)
            {
                this.vista.MostrarMensaje(s, ETipoMensajeIU.CONFIRMACION, "TERMINAR");
                return paquete;
            }
            #endregion

            if ((s = this.ValidarCamposRegistro()) != null)
            {
                this.vista.MostrarMensaje(s, ETipoMensajeIU.ADVERTENCIA, null);
                return paquete;
            }

            //Si la unidad no tiene sus chunches configuradas no puede quedar terminada... es decir que siempre se va a guardar en borrador?
            //Se puede manejar un estatus mas que sea CONFIGURADA
            if (this.vista.UltimoObjeto == null)
                throw new Exception("Es necesario especificar la información inicial de la unidad que se esta editando.");
            if (!this.vista.UltimoObjeto.EstatusActual.HasValue)
                throw new Exception("Es necesario especificar la información inicial de la unidad que se esta editando.");

            this.vista.EstatusUnidad = this.vista.UltimoObjeto.EstatusActual.Value == EEstatusUnidad.NoDisponible ? EEstatusUnidad.Terminada : this.vista.UltimoObjeto.EstatusActual;

            string observaciones = "Sucursal: " + (this.vista.SucursalNombre ?? "");

            this.Editar(observaciones);

            this.IrAPagina(7);
            #region SC0002
            string mensaje = this.ComprobarPaquetesMantenimiento();
            if (mensaje == string.Empty)
                this.vista.MostrarMensaje("Edición Exitosa", ETipoMensajeIU.EXITO, null);
            else
            {
                paquete = 1;
                this.vista.Mensaje = mensaje;
                //this.vista.MostrarMensaje("Registro Exitoso", ETipoMensajeIU.ERROR, mensaje);
            }
            #endregion
            return paquete;
            //this.vista.MostrarMensaje("Edición Exitosa", ETipoMensajeIU.EXITO, null);
        }

        private void Editar(string observaciones)
        {
            try
            {
                
                //Se obtiene la información a partir de la Interfaz de Usuario
                BO.UnidadBO bo = (BO.UnidadBO)this.InterfazUsuarioADato();

                //Sólo se genera el XML una vez en la vida del acta de nacimiento, cuando pasa de NODISPONIBLE a TERMINADA
                if (this.vista.UltimoObjeto != null && this.vista.UltimoObjeto.EstatusActual != null && this.vista.UltimoObjeto.EstatusActual == EEstatusUnidad.NoDisponible)
                {
                    //Por el parche para que se puedan usar las unidades es que se puso DISPONIBLE
                    if (bo.EstatusActual != null && (bo.EstatusActual == EEstatusUnidad.Terminada))
                        bo.ActaNacimiento = bo.ObtenerXml();
                }     

                //Editar el registro
                this.Editar(bo, observaciones);

                #region SC0006 - Adición de datos de siniestro
                //Se despliega la información en la Interfaz de Usuario
                Hashtable parameters = new Hashtable();
                parameters["UnidadBO"] = bo;
                parameters["SiniestroUnidadBO"] = this.RecuperarHistorialSiniestro(bo);
                this.DatoAInterfazUsuario(parameters);
                #endregion

                this.vista.UltimoObjeto = bo;
            }
            catch (Exception ex)
            {
                throw new Exception(this.nombreClase + ".Editar:" + ex.Message);
            }
        }

        /// <summary>
        /// Método que ejecuta el guardado del editar con transacción
        /// </summary>
        /// <param name="bo">Objeto de tipo UnidadBO</param>
        /// <param name="observaciones">Cadena con las observaciones a almacenar</param>
        private void Editar(BO.UnidadBO bo, string observaciones)
        {
            Guid firma = Guid.NewGuid();
            try
            {
                dctx.SetCurrentProvider("Outsourcing");
                dctx.OpenConnection(firma);
                dctx.BeginTransaction(firma);

                //Se crea el objeto de seguridad
                UsuarioBO usuario = new UsuarioBO() { Id = this.vista.UUA };
                AdscripcionBO adscripcion = new AdscripcionBO() { UnidadOperativa = new UnidadOperativaBO() { Id = this.vista.UnidadOperativaId } };
                SeguridadBO seguridadBO = new SeguridadBO(Guid.Empty, usuario, adscripcion);

                //Se inserta en la base de datos
                this.controlador.ActualizarDocumento(this.dctx, bo, this.vista.UltimoObjeto, observaciones, seguridadBO);

                //Se inserta el la base el Pedimento
                if (this.vistaPagina6.CambioPedimento)
                {
                    PedimentoBO pedimentoBO = (PedimentoBO)this.InterfazUsuarioADatoPedimento(bo);
                    PedimentoBR pedimentoBR = new PedimentoBR();
                    pedimentoBR.InsertarCompleto(dctx, pedimentoBO, seguridadBO);
                }

                dctx.SetCurrentProvider("Outsourcing");
                dctx.CommitTransaction(firma);
                
            }
            catch (Exception ex)
            {
                dctx.SetCurrentProvider("Outsourcing");
                dctx.RollbackTransaction(firma);
                throw new Exception(this.nombreClase + ".Editar:" + ex.Message);
            }
            finally
            {
                if (dctx.ConnectionState == ConnectionState.Open)
                    dctx.CloseConnection(firma);
            }
        }

        /// <summary>
        /// Método que procesa los datos de la UI a un objeto de tipo PedimentoBO.
        /// </summary>
        /// <param name="BO">Objeto de tipo UnidadBO.</param>
        /// <returns>Devuelve un objeto de tipo PedimentoBO.</returns>
        private object InterfazUsuarioADatoPedimento(object BO)
        {
            BO.UnidadBO unidadBO = (BO.UnidadBO)BO;
            PedimentoBO tramitePedimento = new PedimentoBO();
            tramitePedimento.Tipo=ETipoTramite.N_PEDIMENTO;
            tramitePedimento.Pedimento = this.vista.NumeroPedimento;
            tramitePedimento.Auditoria = new AuditoriaBO();
            tramitePedimento.Auditoria.UC = this.vista.UUA;
            tramitePedimento.Auditoria.UUA = this.vista.UUA;
            tramitePedimento.Auditoria.FC = this.vista.FUA;
            tramitePedimento.Auditoria.FUA = this.vista.FUA;
            tramitePedimento.Activo = true;
            ITramitable iTramitable = new TramitableProxyBO() { TramitableID = unidadBO.TramitableID, TipoTramitable = unidadBO.TipoTramitable, DescripcionTramitable = unidadBO.DescripcionTramitable };
            tramitePedimento.Tramitable = iTramitable;

            return tramitePedimento;
        }

        /// <summary>
        /// Método que procesa los datos de la UI a un objeto de lista de archivos OC.
        /// </summary>
        /// <returns>Devuelve un objeto con la lista de archivos.</returns>
        private object InterfazUsuarioADatoOC()
        {
            return this.InterfazUsuarioADatoOC(false);
        }

        /// <summary>
        /// Método que procesa los datos de la UI a un objeto de lista de archivos OC.
        /// </summary>
        /// <nuevosArchivo>Indica si los archivo serán nuevos.</returns>
        /// <returns>Devuelve un objeto con la lista de archivos.</returns>
        private object InterfazUsuarioADatoOC(bool nuevosArchivo)
        {
            List<ArchivoBO> archivos = new List<ArchivoBO>();
            ArchivoBR archivoBR = new ArchivoBR();
            ArchivoBO archivoBO = new ArchivoBO();
            if (this.vista.ArchivosOC != null)
            {
                //Asignamos valores de auditoria
                foreach (ArchivoBO archivo in this.vista.ArchivosOC)
                {
                    if (nuevosArchivo)
                    {
                        //Cuando se desea insertar archivos nuevos, es necesario realizar un ConsultarCompleto ya que la propiedad archivo por rendimiento no se devuelve a la interfaz.
                        if (archivo.Id != null)
                        {
                            archivoBO = archivoBR.ConsultarCompleto(dctx, (int)archivo.Id);
                            if (archivoBO != null)
                            {
                                archivo.Archivo = archivoBO.Archivo;
                            }
                        }
                        archivo.Id = null;
                    }
                    if (archivo.Id == null)
                    {
                        archivo.Auditoria = new AuditoriaBO { FC = this.vista.FUA, UUA = this.vista.UUA, FUA = this.vista.FUA, UC = this.vista.UUA };
                    }
                    else
                    {
                        archivo.Auditoria = new AuditoriaBO { FUA = this.vista.FUA, UUA = this.vista.UUA };
                    }
                    archivos.Add(archivo);
                }
            }
            return archivos;
        }

        private void ConsultarCompleto()
        {
            Hashtable parameters = new Hashtable();
            try
            {
                BO.UnidadBO bo = (BO.UnidadBO)this.InterfazUsuarioADato();

                List<BO.UnidadBO> lst = this.controlador.ConsultarCompleto(this.dctx, bo, true);

                if (lst.Count < 1)
                    throw new Exception("No se encontró ningún registro que corresponda a la información proporcionada.");
                if (lst.Count > 1)
                    throw new Exception("La consulta devolvió más de un registro.");

                BO.UnidadBO unidadAnterior = new BO.UnidadBO(lst.First());
                BO.UnidadBO unidadConsultada = lst.First();

                #region Sincronizacion de Datos de la Unidad con LIDER
                List<Servicio.Catalogos.BO.UnidadBO> lstUnidades = FacadeBR.ConsultarUnidadDetalle(dctx, new Servicio.Catalogos.BO.UnidadBO { Id = unidadConsultada.IDLider });
                if(!lstUnidades.Any())
                    throw new Exception("No se encontró registro de la Unidad en LIDER");

                Servicio.Catalogos.BO.UnidadBO unidadLider = lstUnidades.First();
                if(unidadConsultada.CaracteristicasUnidad != null && unidadConsultada.CaracteristicasUnidad.Motor != null)
                {
                    #region Se consulta el número de SERIE DE MOTOR
                    if(SerieMotorId == null)
                        throw new Exception("No se encuentra configurado el valor de la propiedad 'SERIE DE MOTOR' ");

                    String serieMotor = String.Empty;
                    if(unidadLider.CaracteristicasUnidad != null)
                    {
                        Servicio.Catalogos.BO.CaracteristicaUnidadBO caracteristica = unidadLider.CaracteristicasUnidad.FirstOrDefault(x=>x.Caracteristica.Id == SerieMotorId);
                        serieMotor = caracteristica != null ? caracteristica.Valor : String.Empty;
                    }
                    if(!String.IsNullOrEmpty(serieMotor))
                        unidadConsultada.CaracteristicasUnidad.Motor.SerieMotor = serieMotor;
                    #endregion

                    unidadConsultada.NumeroEconomico = unidadLider.Clave;
                }
                #endregion

                #region Se validan las Claves de Activo de Oracle
                EquipoBepensaBO boTemp = new EquipoBepensaBO() { ActivoFijo = new ActivoFijoBO(), Unidad = new Servicio.Catalogos.BO.UnidadBO() };
                boTemp.Unidad.NumeroSerie = boTemp.ActivoFijo.NumeroSerie = "%" + unidadConsultada.NumeroSerie + "%";
                boTemp.ActivoFijo.Libro = this.vista.LibroActivos;

                //Se consultan los activos de Oracle
                List<ActivoFijoBO> activos = FacadeBR.ConsultarActivoFijo(dctx, boTemp.ActivoFijo);
                if(!activos.Any() && !(unidadConsultada.EstatusActual == EEstatusUnidad.NoDisponible || (unidadConsultada.Area != null && ((EArea)unidadConsultada.Area == EArea.CM || (EArea)unidadConsultada.Area == EArea.SD))) && ((int)ETipoEmpresa.Idealease == this.vista.UnidadOperativaId))
                    throw new Exception("No se encontró en ORACLE ninguna Unidad con Número Serie: " + unidadConsultada.NumeroSerie);
                if(activos.Count > 1)
                    this.vista.MostrarMensaje("Existe en ORACLE más de una Unidad con Número de Serie: " + unidadConsultada.NumeroSerie + ", se utilizará la unidad con Clave de Activo: " + activos.First().NumeroActivo, ETipoMensajeIU.ADVERTENCIA);

                EquipoBepensaBO unidadOracle = new EquipoBepensaBO() { ActivoFijo = activos.FirstOrDefault() ?? new ActivoFijoBO() };

                this.vista.ClaveOracleUnidad = !String.IsNullOrEmpty(unidadOracle.ActivoFijo.NumeroActivo) ? unidadOracle.ActivoFijo.NumeroActivo.Trim().ToUpper() : "";
                this.vista.ClaveOracleUnidadLider = unidadConsultada.ActivoFijo != null ? !String.IsNullOrEmpty(unidadConsultada.ActivoFijo.NumeroActivo) ? unidadConsultada.ActivoFijo.NumeroActivo.Trim().ToUpper() : "" : "";

                if (!String.IsNullOrEmpty(this.vista.ClaveOracleUnidad) && !String.IsNullOrEmpty(this.vista.ClaveOracleUnidadLider) && !this.vista.ClaveOracleUnidad.Equals(this.vista.ClaveOracleUnidadLider))
                {
                    this.vista.MostrarMensaje("El 'NÚMERO DE ACTIVO de ORACLE' es DIFERENTE en ORACLE e IDEALEASE.", ETipoMensajeIU.ADVERTENCIA);
                    unidadConsultada.ActivoFijo = unidadOracle.ActivoFijo;
                    unidadConsultada.ClaveActivoOracle = unidadOracle.ActivoFijo.NumeroActivo;
                }
                else
                {
                    if (!String.IsNullOrEmpty(this.vista.ClaveOracleUnidad) && String.IsNullOrEmpty(this.vista.ClaveOracleUnidadLider))
                    {
                        unidadConsultada.ActivoFijo = unidadOracle.ActivoFijo;
                        unidadConsultada.ClaveActivoOracle = unidadOracle.ActivoFijo.NumeroActivo;
                    }
                }
                #endregion

                //Se obtiene los archivos de orden de compra
                

                #region SC0006 - Adición de datos de siniestro
                parameters["UnidadBO"] = unidadConsultada;
                parameters["SiniestroUnidadBO"] = this.RecuperarHistorialSiniestro(unidadConsultada);
                this.DatoAInterfazUsuario(parameters);
                #endregion
                this.vista.UltimoObjeto = unidadAnterior;

                //Si la Unidad se encuentra Bloqueada de Lider para su edicion, no es posible editarla desde Idealease
                if(unidadConsultada.EquipoBloqueado != null && unidadConsultada.EquipoBloqueado.Value)
                    vistaPagina1.HabilitarUnidadBloqueada(false);

                //Se obtiene los archivos de orden de compra
                ArrendamientoBO arrendamiento = unidadConsultada.ObtenerArrendamientoVigente();

                this.vista.ArchivosOC = arrendamiento.Adjuntos; 
            }
            catch (Exception ex)
            {
                #region SC0006 - Adición de datos de siniestro
                parameters["UnidadBO"] = new BO.UnidadBO();
                this.DatoAInterfazUsuario(parameters);
                #endregion
                throw new Exception(this.nombreClase + ".ConsultarCompleto:" + ex.Message);
            }
        }

        private object InterfazUsuarioADato()
        {
            BO.UnidadBO bo = new BO.UnidadBO();
            #region Inicialización de Propiedades
            bo.CaracteristicasUnidad = new CaracteristicasUnidadBO();
            bo.CaracteristicasUnidad.Ejes = new List<EjeBO>();
            bo.CaracteristicasUnidad.Motor = new MotorBO();
            bo.CaracteristicasUnidad.SistemaElectrico = new SistemaElectricoBO();
            bo.CaracteristicasUnidad.Transmision = new TransmisionBO();
            bo.Cliente = new ClienteBO();
            bo.EquiposAliados = new List<EquipoAliadoBO>();
            
            bo.NumerosSerie = new List<NumeroSerieBO>();
                      
            bo.LimpiarLlantas();
            bo.Mediciones = new MedicionesBO();
            bo.Modelo = new ModeloBO();
            bo.Modelo.Marca = new MarcaBO();
            bo.Sucursal = new SucursalBO();
            bo.Sucursal.UnidadOperativa = new UnidadOperativaBO();
            bo.TipoEquipoServicio = new TipoUnidadBO();
            bo.ActivoFijo = new ActivoFijoBO();
            bo.Arrendamientos = new List<ArrendamientoBO>(); 
            #endregion

            bo.UnidadID = this.vista.UnidadId;
            bo.EquipoID = this.vista.EquipoId;
            bo.IDLider = this.vista.LiderID;
            bo.ClaveActivoOracle = this.vista.ClaveActivoOracle;
            bo.Anio = this.vista.Anio;
            bo.Modelo.Nombre = this.vista.ModeloNombre;
            bo.Modelo.Id = this.vista.ModeloId;
            bo.Modelo.Marca.Id = this.vista.MarcaId;
            bo.NumeroEconomico = this.vista.NumeroEconomico;
            bo.NumeroSerie = this.vista.NumeroSerie;
            bo.Fabricante = this.vista.FabricanteNombre;
            bo.TipoEquipoServicio.Nombre = this.vista.TipoUnidadNombre;

            #region REQ. 13285 Acta de nacimiento.
            if (this.vista.Area != null)
            {
                switch (this.vista.UnidadOperativaId)
                {
                    case (int)ETipoEmpresa.Construccion:                        
                        bo.Area = (EAreaConstruccion)this.vista.Area;

                        if ((EAreaConstruccion)bo.Area == EAreaConstruccion.RE)
                        {
                            //Datos del arrendamiento
                            bo.Arrendamientos = (List<ArrendamientoBO>)InterfazUsuarioADatoArrendamiento();
                        }
                        break;
                    case (int)ETipoEmpresa.Generacion:
                        bo.Area = (EAreaGeneracion)this.vista.Area;

                        if ((EAreaGeneracion)bo.Area == EAreaGeneracion.RE)
                        {
                            //Datos del arrendamiento
                            bo.Arrendamientos = (List<ArrendamientoBO>)InterfazUsuarioADatoArrendamiento();
                        }
                        break;
                    case (int)ETipoEmpresa.Equinova:
                        bo.Area = (EAreaEquinova)this.vista.Area;

                        if ((EAreaEquinova)bo.Area == EAreaEquinova.RE) {
                            //Datos del arrendamiento
                            bo.Arrendamientos = (List<ArrendamientoBO>)InterfazUsuarioADatoArrendamiento();
                        }
                        break;
                    default:
                        bo.Area = (EArea)this.vista.Area;
                        bo.ActivoFijo = null;
                        break;
                }
            }
            #endregion

            bo.Propietario = this.vista.Propietario;
            bo.Sucursal.Nombre = this.vista.SucursalNombre;
            bo.Sucursal.Id = this.vista.SucursalId;
            bo.Sucursal.UnidadOperativa.Id = this.vista.UnidadOperativaId;
            bo.Cliente.Nombre = this.vista.Cliente;
            bo.Cliente.Id = this.vista.ClienteId;
            bo.EquipoBloqueado = this.vistaPagina1.UnidadBloqueada;

            #region SC0002
            bo.EntraMantenimiento = this.vista.EntraMantenimiento;
            #endregion

            if (!this.vista.ValoresTabs.Contains("1"))
            {
                bo.Mediciones.Horometros = this.vista.Horometros;
                bo.Mediciones.Odometros = this.vista.Odometros;
                bo.CaracteristicasUnidad.CapacidadTanque = this.vista.CapacidadTanque;
                bo.CaracteristicasUnidad.RendimientoTanque = this.vista.RendimientoTanque;
                bo.CaracteristicasUnidad.PBCMaximoRecomendado = this.vista.PBCMaximoRecomendado;
                bo.CaracteristicasUnidad.PBVMaximoRecomendado = this.vista.PBVMaximoRecomendado;

                bo.CaracteristicasUnidad.Radiador = this.vista.Radiador;
                bo.CaracteristicasUnidad.PostEnfriador = this.vista.PostEnfriador;
            }

            if (!this.vista.ValoresTabs.Contains("2"))
            {
                bo.NumerosSerie = this.vista.NumerosSerie;

                #region SC0030
                bo.CaracteristicasUnidad.Motor.SerieMotor = this.vista.SerieMotor;
                #endregion
                bo.CaracteristicasUnidad.Motor.SerieCompresorAire = this.vista.SerieCompresorAire;
                bo.CaracteristicasUnidad.Motor.SerieECM = this.vista.SerieECM;
                bo.CaracteristicasUnidad.Motor.SerieTurboCargador = this.vista.SerieTurboCargador;
                bo.CaracteristicasUnidad.SistemaElectrico.SerieAlternador = this.vista.SerieAlternador;
                bo.CaracteristicasUnidad.SistemaElectrico.SerieBaterias = this.vista.SerieBaterias;
                bo.CaracteristicasUnidad.SistemaElectrico.SerieMarcha = this.vista.SerieMarcha;
                bo.CaracteristicasUnidad.Transmision.Serie = this.vista.TransmisionSerie;
                bo.CaracteristicasUnidad.Transmision.Modelo = this.vista.TransmisionModelo;

                EjeBO eje = null;
                if (this.vista.EjeDireccionSerie != null || this.vista.EjeDireccionModelo != null)
                {
                    eje = new EjeBO();
                    eje.Posicion = EPosicionEje.Delantero;
                    eje.Serie = this.vista.EjeDireccionSerie;
                    eje.Modelo = this.vista.EjeDireccionModelo;
                    bo.CaracteristicasUnidad.Ejes.Add(eje);
                }
                if (this.vista.EjeTraseroDelanteroSerie != null || this.vista.EjeTraseroDelanteroModelo != null)
                {
                    eje = new EjeBO();
                    eje.Posicion = EPosicionEje.TraseroDelantero;
                    eje.Serie = this.vista.EjeTraseroDelanteroSerie;
                    eje.Modelo = this.vista.EjeTraseroDelanteroModelo;
                    bo.CaracteristicasUnidad.Ejes.Add(eje);
                }
                if (this.vista.EjeTraseroTraseroSerie != null || this.vista.EjeTraseroTraseroModelo != null)
                {
                    eje = new EjeBO();
                    eje.Posicion = EPosicionEje.TraseroTrasero;
                    eje.Serie = this.vista.EjeTraseroTraseroSerie;
                    eje.Modelo = this.vista.EjeTraseroTraseroModelo;
                    bo.CaracteristicasUnidad.Ejes.Add(eje);
                }
            }

            if (!this.vista.ValoresTabs.Contains("3"))
            {
                bo.AgregarLlantas(this.vista.Llantas);
                foreach (var llanta in bo.Llantas)
                {
                    if (llanta.Sucursal == null || llanta.Sucursal.Id == null)
                    {
                        llanta.Sucursal = bo.Sucursal;
                        llanta.Sucursal.Auditoria = new AuditoriaBO();
                        llanta.Sucursal.Auditoria.FUA = this.vista.FUA;
                        llanta.Sucursal.Auditoria.UUA = this.vista.UUA;
                    }
                }

                if (this.vista.RefaccionCodigo != null && this.vista.RefaccionCodigo.Trim().CompareTo("") != 0)
                {
                    LlantaBO refaccion = new LlantaBO() { Auditoria = new AuditoriaBO(), Sucursal = new SucursalBO() { UnidadOperativa = new UnidadOperativaBO() } };
                    refaccion.Sucursal = bo.Sucursal;
                    refaccion.LlantaID = this.vista.RefaccionID;
                    refaccion.Activo = this.vista.RefaccionActivo;
                    refaccion.Codigo = this.vista.RefaccionCodigo;
                    refaccion.EsRefaccion = true;
                    refaccion.Marca = this.vista.RefaccionMarca;
                    refaccion.Medida = this.vista.RefaccionMedida;
                    refaccion.Modelo = this.vista.RefaccionModelo;
                    refaccion.Profundidad = this.vista.RefaccionProfundidad;
                    refaccion.Revitalizada = this.vista.RefaccionRevitalizada;
                    refaccion.Stock = false;
                    refaccion.MontadoEn = new BO.UnidadBO() { UnidadID = bo.UnidadID, NumeroSerie = bo.NumeroSerie };

                    if (this.vista.RefaccionUC != null)
                    {
                        refaccion.Auditoria.FC = this.vista.RefaccionFC;
                        refaccion.Auditoria.UC = this.vista.RefaccionUC;
                    }
                    else
                    {
                        refaccion.Auditoria.UC = this.vista.UUA;
                        refaccion.Auditoria.FC = this.vista.FUA;
                    }
                    refaccion.Auditoria.FUA = this.vista.FUA;
                    refaccion.Auditoria.UUA = this.vista.UUA;

                    bo.AgregarLlanta(refaccion);
                }
            }

            if (!this.vista.ValoresTabs.Contains("4"))
            {
                #region SC0002

                foreach (GridViewRow item in this.vista.GridAliados.Rows)
                {
                    EquipoAliadoBO equipoAliado = new EquipoAliadoBO();


                    Label lbID = item.FindControl("lbEquipoAliadoID") as Label;
                    CheckBox chbxEntrada = item.FindControl("chbxEntraMantenimiento") as CheckBox;

                    equipoAliado.EquipoAliadoID = int.Parse(lbID.Text);

                    var equipoEncontrado = this.vista.EquiposAliados.Find(x => x.EquipoAliadoID == equipoAliado.EquipoAliadoID);
                    if (this.vista.EntraMantenimiento == true)
                    {
                        if (chbxEntrada.Checked == true)
                            equipoEncontrado.EntraMantenimiento = true;
                        else if (chbxEntrada.Checked == false)
                            equipoEncontrado.EntraMantenimiento = false;
                    }
                    else if (this.vista.EntraMantenimiento == false)
                    {
                        equipoEncontrado.EntraMantenimiento = false;
                    }
                }

                bo.EquiposAliados = this.vista.EquiposAliados;

                #endregion
            }


            if (this.vista.ClaveActivoOracle != null)
                bo.EsActivo = true;
            else
                bo.EsActivo = false;
            bo.FC = this.vista.FC;
            bo.FUA = this.vista.FUA;
            bo.UC = this.vista.UC;
            bo.UUA = this.vista.UUA;
            bo.EstatusActual = this.vista.EstatusUnidad;

            #region REQ 13285 Acta de Nacimiento.
            //Agregar archivos pedimento
            bo.AgregarPedimento(this.vista.Archivos);
            foreach (var pedimento in bo.Pedimentos)
            {
                if (pedimento != null)
                {
                    pedimento.Auditoria = new AuditoriaBO();
                    pedimento.Auditoria.FUA = this.vista.FUA;
                    pedimento.Auditoria.UUA = this.vista.UUA;
                }
            }
            #endregion

            if (vista.UltimoObjeto != null)
                bo.ActaNacimiento = vista.UltimoObjeto.ActaNacimiento;

            return bo;
        }

        /// <summary>
        /// Método que procesa los datos de la UI a un objeto de tipo ArrendamientoBO.
        /// </summary>
        /// <returns>Devuelve una lista de tipo ArrendamientoBO.</returns>
        private object InterfazUsuarioADatoArrendamiento()
        {
            List<ArrendamientoBO> lstArrendamientos = new List<ArrendamientoBO>();
            ArrendamientoBO arrendamientoBO = new ArrendamientoBO();
            arrendamientoBO.Proveedor = new ProveedorBO();
            arrendamientoBO.ArrendamientoID = this.vista.ArrendamientoId;
            arrendamientoBO.Proveedor.Id = this.vista.ProveedorID;
            arrendamientoBO.NumeroOrdenCompra = this.vista.OrdenCompraProveedor;
            arrendamientoBO.MontoArrendamiento = this.vista.MontoArrendamiento;
            arrendamientoBO.CodigoMoneda = this.vista.CodigoMoneda;
            arrendamientoBO.FechaInicioArrendamiento = this.vista.FechaInicioArrendamiento;
            arrendamientoBO.FechaFinArrendamiento = this.vista.FechaFinArrendamiento;
            arrendamientoBO.Activo = true;
            if (arrendamientoBO.ArrendamientoID != null)
            {
                arrendamientoBO.UC = this.vista.UUA;
                arrendamientoBO.FC = this.vista.FUA;
            }
            arrendamientoBO.UUA = this.vista.UUA;
            arrendamientoBO.FUA = this.vista.FUA;
            //Si no se ha creado un arrendamiento y se intenta la renovación no será permitido.
            if (arrendamientoBO.ArrendamientoID != null && this.vistaPagina1.ArrendamientoNuevo)
            {
                arrendamientoBO.Activo = false;
                arrendamientoBO.Adjuntos = null;
                lstArrendamientos.Add(arrendamientoBO);

                //Si cambia algo menos la fecha solo es actualizar
                arrendamientoBO = new ArrendamientoBO();
                arrendamientoBO.Proveedor = new ProveedorBO();
                arrendamientoBO.ArrendamientoID = null;
                arrendamientoBO.Proveedor.Id = this.vista.ProveedorID;
                arrendamientoBO.NumeroOrdenCompra = this.vista.OrdenCompraProveedor;
                arrendamientoBO.MontoArrendamiento = this.vista.MontoArrendamiento;
                arrendamientoBO.CodigoMoneda = this.vista.CodigoMoneda;
                arrendamientoBO.FechaInicioArrendamiento = this.vista.FechaInicioArrendamientoNuevo;
                arrendamientoBO.FechaFinArrendamiento = this.vista.FechaFinArrendamientoNuevo;
                arrendamientoBO.Adjuntos = (List<ArchivoBO>)this.InterfazUsuarioADatoOC(true);
                arrendamientoBO.Activo = true;
                arrendamientoBO.UC = this.vista.UUA;
                arrendamientoBO.FC = this.vista.FUA;
                arrendamientoBO.UUA = this.vista.UUA;
                arrendamientoBO.FUA = this.vista.FUA;
                lstArrendamientos.Add(arrendamientoBO);
            }
            else
            {
                arrendamientoBO.Adjuntos = (List<ArchivoBO>)this.InterfazUsuarioADatoOC();
                lstArrendamientos.Add(arrendamientoBO);
            }
            return lstArrendamientos;
        }

        #region SC0006
        private List<SiniestroUnidadBO> RecuperarHistorialSiniestro(BO.UnidadBO bo)
        {
            if (bo.UnidadID.HasValue)
            {
                SiniestroUnidadBR SiniestroUnidadBR = new SiniestroUnidadBR();
                HistorialBR historialBR = new HistorialBR();
                HistorialBOF filter = new HistorialBOF
                {
                    Unidad = new BO.UnidadBO { UnidadID = bo.UnidadID },
                    Movimiento = EMovimiento.BAJA_DE_LA_FLOTA_POR_SINIESTRO
                };
                List<HistorialBOF> historial = historialBR.ConsultarCompleto(this.dctx, filter);

                return SiniestroUnidadBR.ConsultarCompleto(this.dctx, historial.Cast<HistorialBO>().ToList());
            }

            return null;
        }
        #endregion

        private void DatoAInterfazUsuario(IDictionary parameters)
        {
            if (!parameters.Contains("UnidadBO") || !(parameters["UnidadBO"] is BO.UnidadBO))
                return;
            ArrendamientoBO boArrendamiento = new ArrendamientoBO();
            BO.UnidadBO bo = (BO.UnidadBO)parameters["UnidadBO"];
            #region Inicialización de Propiedades
            if (bo.ActivoFijo == null)
                bo.ActivoFijo = new ActivoFijoBO();


                if (bo.CaracteristicasUnidad == null)
                    bo.CaracteristicasUnidad = new CaracteristicasUnidadBO();
                if (bo.CaracteristicasUnidad.Ejes == null)
                    bo.CaracteristicasUnidad.Ejes = new List<EjeBO>();
                if (bo.CaracteristicasUnidad.Motor == null)
                    bo.CaracteristicasUnidad.Motor = new MotorBO();
                if (bo.CaracteristicasUnidad.SistemaElectrico == null)
                    bo.CaracteristicasUnidad.SistemaElectrico = new SistemaElectricoBO();
                if (bo.CaracteristicasUnidad.Transmision == null)
                    bo.CaracteristicasUnidad.Transmision = new TransmisionBO();
            

            if (bo.Cliente == null)
                bo.Cliente = new ClienteBO();
            if (bo.EquiposAliados == null)
                bo.EquiposAliados = new List<EquipoAliadoBO>();
                      
            if (bo.NumerosSerie == null)
                bo.NumerosSerie = new List<NumeroSerieBO>();
                       
            if (bo.Llantas == null)
                bo.LimpiarLlantas();


                if (bo.Mediciones == null)
                    bo.Mediciones = new MedicionesBO();
                if (bo.Mediciones.Horometros == null)
                    bo.Mediciones.Horometros = new List<HorometroBO>();
                if (bo.Mediciones.Odometros == null)
                    bo.Mediciones.Odometros = new List<OdometroBO>();
            


                if (bo.Modelo == null)
                    bo.Modelo = new ModeloBO();
                if (bo.Modelo.Marca == null)
                    bo.Modelo.Marca = new MarcaBO();
                if (bo.Sucursal == null)
                    bo.Sucursal = new SucursalBO();
                if (bo.Sucursal.UnidadOperativa == null)
                    bo.Sucursal.UnidadOperativa = new UnidadOperativaBO();


            if (bo.TipoEquipoServicio == null)
                bo.TipoEquipoServicio = new TipoUnidadBO();

            if (bo.Arrendamientos == null)
                bo.Arrendamientos = new List<ArrendamientoBO>();

            if (bo.Pedimentos == null)
                bo.Pedimentos = new List<ArchivoBO>();
            #endregion

            this.vista.EquipoId = bo.EquipoID;
            this.vista.UnidadId = bo.UnidadID;
            this.vista.NumeroSerie = bo.NumeroSerie;

            this.vista.ClaveActivoOracle = bo.ClaveActivoOracle;
            this.vista.LiderID = bo.IDLider;
            this.vista.Anio = bo.Anio;
            this.vista.ModeloNombre = bo.Modelo.Nombre;
            this.vista.ModeloId = bo.Modelo.Id;
            this.vista.MarcaId = bo.Modelo.Marca.Id;
            this.vista.NumeroEconomico = bo.NumeroEconomico;
            this.vista.FabricanteNombre = bo.Fabricante;
            this.vista.TipoUnidadNombre = bo.TipoEquipoServicio.Nombre;
            this.vista.FechaCompra = bo.ActivoFijo.FechaFacturaCompra;
            this.vista.MontoFactura = bo.ActivoFijo.CostoSinIva;

            if (bo.Area != null)
            {
                this.vista.Area = bo.Area;
                switch (this.vista.UnidadOperativaId)
                {
                    case (int)ETipoEmpresa.Construccion:
                        this.presentadorPagina1.SeleccionarTipoRentaEmpresas(bo.Area);

                        if ((EAreaConstruccion)bo.Area == EAreaConstruccion.RE)
                        {
                            // Se asignan los valores de la propiedad Arrendamiento
                            boArrendamiento = bo.ObtenerArrendamientoVigente();
                            this.vista.ArrendamientoId = boArrendamiento.ArrendamientoID;
                            this.vista.ProveedorID = boArrendamiento.Proveedor.Id; //En lugar del propietario, va el proveedor id, el control debe de obtener la información.
                            this.vista.OrdenCompraProveedor = boArrendamiento.NumeroOrdenCompra;
                            this.vista.MontoArrendamiento = boArrendamiento.MontoArrendamiento;
                            this.vista.CodigoMoneda = boArrendamiento.CodigoMoneda;
                            this.vista.FechaInicioArrendamiento = boArrendamiento.FechaInicioArrendamiento;
                            this.vista.FechaFinArrendamiento = boArrendamiento.FechaFinArrendamiento;     
                        }
                        this.vista.Archivos = bo.ObtenerPedimento();

                        break;
                    case (int)ETipoEmpresa.Generacion:
                        this.presentadorPagina1.SeleccionarTipoRentaEmpresas(bo.Area);

                        if ((EAreaGeneracion)bo.Area == EAreaGeneracion.RE)
                        {
                            // Se asignan los valores de la propiedad Arrendamiento
                            boArrendamiento = bo.ObtenerArrendamientoVigente();
                            this.vista.ArrendamientoId = boArrendamiento.ArrendamientoID;
                            this.vista.ProveedorID = boArrendamiento.Proveedor.Id; //En lugar del propietario, va el proveedor id, el control debe de obtener la información.
                            this.vista.OrdenCompraProveedor = boArrendamiento.NumeroOrdenCompra;
                            this.vista.MontoArrendamiento = boArrendamiento.MontoArrendamiento;
                            this.vista.CodigoMoneda = boArrendamiento.CodigoMoneda;
                            this.vista.FechaInicioArrendamiento = boArrendamiento.FechaInicioArrendamiento;
                            this.vista.FechaFinArrendamiento = boArrendamiento.FechaFinArrendamiento;
                        }
                        this.vista.Archivos = bo.ObtenerPedimento();

                        break;
                    case (int)ETipoEmpresa.Equinova:
                        this.presentadorPagina1.SeleccionarTipoRentaEmpresas(bo.Area);

                        if ((EAreaEquinova)bo.Area == EAreaEquinova.RE) {
                            // Se asignan los valores de la propiedad Arrendamiento
                            boArrendamiento = bo.ObtenerArrendamientoVigente();
                            this.vista.ArrendamientoId = boArrendamiento.ArrendamientoID;
                            this.vista.ProveedorID = boArrendamiento.Proveedor.Id; //En lugar del propietario, va el proveedor id, el control debe de obtener la información.
                            this.vista.OrdenCompraProveedor = boArrendamiento.NumeroOrdenCompra;
                            this.vista.MontoArrendamiento = boArrendamiento.MontoArrendamiento;
                            this.vista.CodigoMoneda = boArrendamiento.CodigoMoneda;
                            this.vista.FechaInicioArrendamiento = boArrendamiento.FechaInicioArrendamiento;
                            this.vista.FechaFinArrendamiento = boArrendamiento.FechaFinArrendamiento;
                        }
                        this.vista.Archivos = bo.ObtenerPedimento();

                        break;
                    default:
                        this.presentadorPagina1.SeleccionarArea((EArea)bo.Area);
                        this.vista.Area = (EArea)bo.Area;
                        break;
                }
            }
            this.vista.Propietario = bo.Propietario;
            this.vista.SucursalNombre = bo.Sucursal.Nombre;
            this.vista.SucursalId = bo.Sucursal.Id;
            this.vista.Cliente = bo.Cliente.Nombre;
            this.vista.ClienteId = bo.Cliente.Id;
            this.vistaPagina1.UnidadBloqueada = bo.EquipoBloqueado;


            #region SC0002
            this.vista.EntraMantenimiento = bo.EntraMantenimiento;
            this.vistaPagina1.EstablecerEntraMantenimeinto();
            #endregion

            if (!this.vista.ValoresTabs.Contains("1"))
            {
                this.presentadorPagina2.AgregarHorometros(new List<HorometroBO>(bo.Mediciones.Horometros.ConvertAll(s => new HorometroBO(s.ObtenerXml()))));
                this.presentadorPagina2.AgregarOdometros(new List<OdometroBO>(bo.Mediciones.Odometros.ConvertAll(s => new OdometroBO(s.ObtenerXml()))));
                this.vista.CapacidadTanque = bo.CaracteristicasUnidad.CapacidadTanque;
                this.vista.RendimientoTanque = bo.CaracteristicasUnidad.RendimientoTanque;
                this.vista.PBCMaximoRecomendado = bo.CaracteristicasUnidad.PBCMaximoRecomendado;
                this.vista.PBVMaximoRecomendado = bo.CaracteristicasUnidad.PBVMaximoRecomendado;

                this.vista.Radiador = bo.CaracteristicasUnidad.Radiador;
                this.vista.PostEnfriador = bo.CaracteristicasUnidad.PostEnfriador;

                this.vista.NumerosSerie = bo.NumerosSerie;
            }

            if (!this.vista.ValoresTabs.Contains("2"))
            {
                this.presentadorPagina3.AgregarNumerosSerie(bo.NumerosSerie);

                #region SC0030
                this.vista.SerieMotor = bo.CaracteristicasUnidad.Motor.SerieMotor;
                #endregion
                this.vista.SerieCompresorAire = bo.CaracteristicasUnidad.Motor.SerieCompresorAire;
                this.vista.SerieECM = bo.CaracteristicasUnidad.Motor.SerieECM;
                this.vista.SerieTurboCargador = bo.CaracteristicasUnidad.Motor.SerieTurboCargador;
                this.vista.SerieAlternador = bo.CaracteristicasUnidad.SistemaElectrico.SerieAlternador;
                this.vista.SerieBaterias = bo.CaracteristicasUnidad.SistemaElectrico.SerieBaterias;
                this.vista.SerieMarcha = bo.CaracteristicasUnidad.SistemaElectrico.SerieMarcha;
                this.vista.TransmisionModelo = bo.CaracteristicasUnidad.Transmision.Modelo;
                this.vista.TransmisionSerie = bo.CaracteristicasUnidad.Transmision.Serie;

                EjeBO eje = bo.CaracteristicasUnidad.ObtenerEjePorPosicion(EPosicionEje.Delantero);
                if (eje != null)
                {
                    this.vista.EjeDireccionModelo = eje.Modelo;
                    this.vista.EjeDireccionSerie = eje.Serie;
                }
                eje = bo.CaracteristicasUnidad.ObtenerEjePorPosicion(EPosicionEje.TraseroDelantero);
                if (eje != null)
                {
                    this.vista.EjeTraseroDelanteroModelo = eje.Modelo;
                    this.vista.EjeTraseroDelanteroSerie = eje.Serie;
                }
                eje = bo.CaracteristicasUnidad.ObtenerEjePorPosicion(EPosicionEje.TraseroTrasero);
                if (eje != null)
                {
                    this.vista.EjeTraseroTraseroModelo = eje.Modelo;
                    this.vista.EjeTraseroTraseroSerie = eje.Serie;
                }
            }

            if (!this.vista.ValoresTabs.Contains("3"))
            {
                this.vista.EnllantableID = bo.EnllantableID;
                this.vista.TipoEnllantable = (int)bo.TipoEnllantable;
                this.vista.DescripcionEnllantable = bo.DescripcionEnllantable;
                this.presentadorPagina4.AgregarLlantas(bo.ObtenerLlantas());
                LlantaBO refaccion = bo.ObtenerRefaccion();
                if (refaccion == null) refaccion = new LlantaBO() { EsRefaccion = true, Activo = true, Stock = true };
                if (refaccion.Auditoria == null) refaccion.Auditoria = new AuditoriaBO();
                if (refaccion.Sucursal == null) refaccion.Sucursal = new SucursalBO();
                this.vista.RefaccionSucursalID = refaccion.Sucursal.Id;
                this.vista.RefaccionSucursalNombre = refaccion.Sucursal.Nombre;
                this.vista.RefaccionID = refaccion.LlantaID;
                this.vista.RefaccionCodigo = refaccion.Codigo;
                this.vista.RefaccionMarca = refaccion.Marca;
                this.vista.RefaccionModelo = refaccion.Modelo;
                this.vista.RefaccionActivo = refaccion.Activo;
                this.vista.RefaccionFC = refaccion.Auditoria.FC;
                this.vista.RefaccionFUA = refaccion.Auditoria.FUA;
                this.vista.RefaccionMedida = refaccion.Medida;
                this.vista.RefaccionProfundidad = refaccion.Profundidad;
                this.vista.RefaccionRevitalizada = refaccion.Revitalizada;
                this.vista.RefaccionStock = refaccion.Stock;
                this.vista.RefaccionUC = refaccion.Auditoria.UC;
                this.vista.RefaccionUUA = refaccion.Auditoria.UUA;
            }

            if (!this.vista.ValoresTabs.Contains("4"))
                this.presentadorPagina5.AgregarEquiposAliado(bo.EquiposAliados);


            this.vista.EstatusUnidad = bo.EstatusActual;

            this.presentadorPagina6.CargarTramites(bo.TramitableID, bo.TipoTramitable, bo.DescripcionTramitable);

            this.presentadorPagina7.DesplegarInformacion(bo);

            List<SiniestroUnidadBO> historial = parameters.Contains("SiniestroUnidadBO") && parameters["SiniestroUnidadBO"] is List<SiniestroUnidadBO> ? (List<SiniestroUnidadBO>)parameters["SiniestroUnidadBO"] : null;
            this.presentadorPagina7.DesplegarInformacionSiniestro(historial);

            this.presentadorPagina1.BloquearCamposPorEstatus(bo.EstatusActual);
            this.presentadorPagina5.BloquearCamposPorEstatus(bo.EstatusActual);

            if (bo.EstatusActual != null && bo.EstatusActual != EEstatusUnidad.NoDisponible)
                this.vista.OcultarBorrador(true);
            else
                this.vista.OcultarBorrador(false);
        }

        public string ValidarCamposBorrador()
        {
            string s = "";

            if (this.vista.UnidadId == null)
                s += "UnidadID, ";
            if (this.vista.EquipoId == null)
                s += "EquipoID, ";

            if (s != null && s.Trim().CompareTo("") != 0)
                return "Los siguientes campos no pueden estar vacíos: \n" + s.Substring(0, s.Length - 2);

            if ((s = this.presentadorPagina1.ValidarCamposBorrador()) != null)
                return "Paso 1:" + s;
            #region SC0040
            if (this.vista.UnidadOperativaId == (int)ETipoEmpresa.Idealease)
            {
                if (string.IsNullOrEmpty(this.vista.ClaveActivoOracle) && this.vista.Area != null && ((EArea)this.vista.Area == EArea.RD || (EArea)this.vista.Area == EArea.FSL))
                    return "Paso 1:La unidad seleccionada no es un activo fijo y, por lo tanto, no puede estar en Renta Diaria o Full Service Leasing";
            }
            #endregion SC0040
            if (!this.vista.ValoresTabs.Contains("1"))
            {
                if ((s = this.presentadorPagina2.ValidarCamposBorrador()) != null)
                    return "Paso 2:" + s;
            }
            if (!this.vista.ValoresTabs.Contains("2"))
            {
                if ((s = this.presentadorPagina3.ValidarCamposBorrador()) != null)
                    return "Paso 3:" + s;
            }
            if (!this.vista.ValoresTabs.Contains("3"))
            {
                if ((s = this.presentadorPagina4.ValidarCamposBorrador()) != null)
                    return "Paso 4:" + s;
            }
            if (!this.vista.ValoresTabs.Contains("4"))
            {
                if ((s = this.presentadorPagina5.ValidarCamposBorrador()) != null)
                    return "Paso 5:" + s;
            }

            return null;
        }
        public string ValidarCamposRegistro()
        {
            string s = "";

            if (this.vista.UnidadId == null)
                s += "UnidadID, ";
            if (this.vista.EquipoId == null)
                s += "EquipoID, ";

            if (s != null && s.Trim().CompareTo("") != 0)
                return "Los siguientes campos no pueden estar vacíos: \n" + s.Substring(0, s.Length - 2);

            if ((s = this.presentadorPagina1.ValidarCamposRegistro()) != null)
                return "Paso 1:" + s;
            #region RI0040
            if (this.vista.UnidadOperativaId == (int)ETipoEmpresa.Idealease)
            {
                if (string.IsNullOrEmpty(this.vista.ClaveActivoOracle) && this.vista.Area != null && ((EArea)this.vista.Area == EArea.RD || (EArea)this.vista.Area == EArea.FSL))
                    return "Paso 1:La unidad seleccionada no es un activo fijo y, por lo tanto, no puede estar en Renta Diaria o Full Service Leasing";
            }
            #endregion RI0040

            if (!this.vista.ValoresTabs.Contains("1"))
            {
                if ((s = this.presentadorPagina2.ValidarCamposRegistro()) != null)
                    return "Paso 2:" + s;
            }
            if (!this.vista.ValoresTabs.Contains("2"))
            {
                if ((s = this.presentadorPagina3.ValidarCamposRegistro()) != null)
                    return "Paso 3:" + s;
            }
            if (!this.vista.ValoresTabs.Contains("3"))
            {
                if ((s = this.presentadorPagina4.ValidarCamposRegistro()) != null)
                    return "Paso 4:" + s;
            }
            if (!this.vista.ValoresTabs.Contains("4"))
            {
                if ((s = this.presentadorPagina5.ValidarCamposRegistro()) != null)
                    return "Paso 5:" + s;
            }
            if (this.vista.UnidadOperativaId == (int)ETipoEmpresa.Idealease)
            {
                if (this.vista.Area != null && ((EArea)this.vista.Area == EArea.RD || (EArea)this.vista.Area == EArea.FSL))
                {
                    if (this.vistaPagina5.EquiposAliados.Any())
                    {
                        foreach (var bo in this.vistaPagina5.EquiposAliados)
                        {
                            if (!(bo.ActivoFijo != null && !String.IsNullOrEmpty(bo.ActivoFijo.NumeroActivo)))
                            {
                                s = String.IsNullOrEmpty(s) ? String.Empty : s + ", ";
                                s = s + bo.NumeroSerie;
                            }
                        }
                    }
                    if (!String.IsNullOrEmpty(s))
                        return "Paso 5: Los siguientes Equipos Aliados NO son Activos Fijos, por lo tanto, no pueden estar en Renta Diaria o Full Service Leasing: " + s;
                }
            }
            if ((s = this.ValidarCamposRegistroTramites()) != null && ((ETipoEmpresa)this.vista.UnidadOperativaId == ETipoEmpresa.Idealease))
                return "Paso 6:" + s;

            return null;
        }
        private string ValidarCamposRegistroTramites()
        {
            string s = "";

            if (!this.presentadorPagina6.ExisteTramite(ETipoTramite.PLACA_ESTATAL) && !this.presentadorPagina6.ExisteTramite(ETipoTramite.PLACA_FEDERAL))
                s += "Placa Estatal y/o Federal, ";

            if(this.vista.UnidadOperativaId==(int)ETipoEmpresa.Idealease)
            {
                switch ((EArea)this.vista.Area)
                {
                    case EArea.FSL:
                    case EArea.RD:
                        if (!this.presentadorPagina6.ExisteTramite(ETipoTramite.SEGURO))
                            s += "Seguro, ";
                        if (!this.presentadorPagina6.ExisteTramite(ETipoTramite.GPS))
                            s += "GPS, ";
                        if (!this.presentadorPagina6.ExisteTramite(ETipoTramite.FILTRO_AK))
                            s += "Filtro AK, ";
                        if (!this.presentadorPagina6.ExisteTramite(ETipoTramite.VERIFICACION_AMBIENTAL))
                            s += "Verificación Ambiental, ";
                        if (!this.presentadorPagina6.ExisteTramite(ETipoTramite.VERIFICACION_FISICO_MECANICA))
                            s += "Verificación Físico-Mecánica, ";
                        if (!this.presentadorPagina6.ExisteTramite(ETipoTramite.TENENCIA))
                            s += "Tenencia, ";
                        break;
                }
            }

            if (s.Trim().CompareTo("") != 0)
                return "Los siguientes trámites son requeridos: \n" + s.Substring(0, s.Length - 2);

            return null;
        }

        private void LimpiarSesion()
        {
            this.vista.LimpiarSesion();
            this.presentadorPagina1.LimpiarSesion();
            this.presentadorPagina2.LimpiarSesion();
            this.presentadorPagina3.LimpiarSesion();
            this.presentadorPagina4.LimpiarSesion();
            this.presentadorPagina5.LimpiarSesion();
        }

        #region SC0002

        private string ComprobarPaquetesMantenimiento()
        {
            string mensaje = string.Empty;
            ModeloBO modeloUnidad = new ModeloBO() { Id = this.vista.ModeloId };

            bool existeUnidad = ValidarPaquetesMantenimiento(modeloUnidad);
            if (existeUnidad == false)
            {
                modeloUnidad = FacadeBR.ConsultarModelo(dctx, modeloUnidad).FirstOrDefault();
                mensaje = "La unidad con modelo " + modeloUnidad.Nombre + " no tiene configurado Paquetes de mantenimiento.\n";
            }
            if (this.vista.EquiposAliados != null && this.vista.EquiposAliados.Count > 0)
            {
                foreach (var item in this.vista.EquiposAliados)
                {
                    bool existeAliado = ValidarPaquetesMantenimiento(item.Modelo);
                    if (existeAliado == false)
                    {
                        mensaje += "El equipo Aliado con modelo " + item.Modelo.Nombre + " no tiene configurado paquetes de mantenimiento. \n";
                    }
                }
            }

            return mensaje;
        }

        private bool ValidarPaquetesMantenimiento(ModeloBO modelo)
        {
            bool PaquetesAsignados = false;

            List<string> paquetes = Enum.GetNames(typeof(ETipoMantenimiento)).ToList();
            List<ConfiguracionPosicionTrabajoBO> paquetesConfigurados = new List<ConfiguracionPosicionTrabajoBO>();

            foreach (var item in paquetes)
            {
                var configuracionPosicionTrabajo = new ConfiguracionPosicionTrabajoBO()
                {
                    ConfiguracionModeloMotorizacion = new ConfiguracionModeloMotorizacionBO()
                    {
                        Modelo = modelo
                    },
                    DescriptorTrabajo = new DescriptorTrabajoBO
                    {
                        Nombre = item
                    }
                };

                var configPosicion = FacadeBR.ConsultarConfiguracionPosicionTrabajoDetalle(dctx, configuracionPosicionTrabajo).FirstOrDefault();
                if(configPosicion != null)
                   paquetesConfigurados.Add(configPosicion);
            }

            if (paquetesConfigurados != null && paquetesConfigurados.Count > 0)
            {
                if (paquetesConfigurados.Exists(x => x.DescriptorTrabajo.Nombre == "PMA" || x.DescriptorTrabajo.Nombre == "PMB" || x.DescriptorTrabajo.Nombre == "PMC") == true)
                    PaquetesAsignados = true;
            }
            else
            {
                PaquetesAsignados = false;
            }

            return PaquetesAsignados;

        }

        #endregion

        #region REQ 13285 Acta de Nacimiento.
        /// <summary>
        /// Método que obtiene la lista de acciones configuradas para el usuario.
        /// </summary>
        public void ObtenerAcciones()
        {
            //Valida que el usuario y la unidad operativa no sean nulos
            if (this.vista.UUA == null) throw new Exception("El identificador del usuario no debe ser nulo");
            if (this.vista.UnidadOperativaId == null)
                throw new Exception("El identificador de la Unidad Operativa no debe ser nulo");

            if (this.vista.ListaAcciones == null)
            {
                // Creación del objeto seguridad
                UsuarioBO usuario = new UsuarioBO { Id = this.vista.UUA };
                AdscripcionBO adscripcion = new AdscripcionBO
                {
                    UnidadOperativa = new UnidadOperativaBO { Id = this.vista.UnidadOperativaId }
                };
                SeguridadBO seguridad = new SeguridadBO(Guid.Empty, usuario, adscripcion);

                //Consulta de lista de acciones a las que tiene permiso el usuario
                this.vista.ListaAcciones = FacadeBR.ConsultarAccion(dctx, seguridad);
            }

            this.vista.EmpresaConPermiso = ETipoEmpresa.Idealease;

            switch (this.vista.UnidadOperativaId)
            {
                case (int)ETipoEmpresa.Generacion:
                    if (ExisteAccion(this.vista.ListaAcciones, "UI ACTA GENERACION"))
                        this.vista.EmpresaConPermiso = ETipoEmpresa.Generacion;
                    break;
                case (int)ETipoEmpresa.Equinova:
                    if (ExisteAccion(this.vista.ListaAcciones, "UI ACTA GENERACION"))
                        this.vista.EmpresaConPermiso = ETipoEmpresa.Equinova;
                    break;
                case (int)ETipoEmpresa.Construccion:
                    if (ExisteAccion(this.vista.ListaAcciones, "UI ACTA CONSTRUCCION"))
                        this.vista.EmpresaConPermiso = ETipoEmpresa.Construccion;
                    break;
                default:
                   
                    break;
            }
            this.vista.ValoresTabs = string.Empty;

            if (!ExisteAccion(this.vista.ListaAcciones, "UI DATOS TECNICOS"))
            {
                this.vista.ValoresTabs = "1,";
            }
            if (!ExisteAccion(this.vista.ListaAcciones, "UI NUMEROSERIE"))
            {
                this.vista.ValoresTabs += "2,";
            }
            if (!ExisteAccion(this.vista.ListaAcciones, "UI LLANTAS"))
            {
                this.vista.ValoresTabs += "3,";
            }
            if (!ExisteAccion(this.vista.ListaAcciones, "UI EQUIPOALIADO"))
            {
                this.vista.ValoresTabs += "4";
            }

            this.vista.ValoresTabs = this.vista.ValoresTabs.TrimEnd(',');
        }
        /// <summary>
        /// Invoca el método EstablecerAcciones de la presentadoras y de la vista IEditarActaNacimientoVIS.
        /// </summary>
        public void EstablecerAcciones()
        {
            //Se establecen las acciones por cada control de usuario.
            this.presentadorPagina1.EstablecerAcciones(this.vista.ListaAcciones, "E");
            this.presentadorPagina5.EstablecerAcciones(this.vista.ListaAcciones);
            this.presentadorPagina6.EstablecerAcciones(this.vista.ListaAcciones);
            this.presentadorPagina7.EstablecerAcciones(this.vista.ListaAcciones);

            if (this.vista.UnidadOperativaId != (int)this.vista.EmpresaConPermiso)
            {
                this.vista.EmpresaConPermiso = ETipoEmpresa.Idealease;
                switch (this.vista.UnidadOperativaId)
                {
                    case (int)ETipoEmpresa.Generacion:
                        if (ExisteAccion(this.vista.ListaAcciones, "UI ACTA GENERACION"))
                            this.vista.EmpresaConPermiso = ETipoEmpresa.Generacion; 
                        break;
                    case (int)ETipoEmpresa.Equinova:
                        if (ExisteAccion(this.vista.ListaAcciones, "UI ACTA GENERACION"))
                            this.vista.EmpresaConPermiso = ETipoEmpresa.Equinova;
                        break;
                    case (int)ETipoEmpresa.Construccion:
                        if (ExisteAccion(this.vista.ListaAcciones, "UI ACTA CONSTRUCCION"))
                            this.vista.EmpresaConPermiso = ETipoEmpresa.Construccion;
                        break;
                    
                }
                this.vista.ValoresTabs = string.Empty;

                if (!ExisteAccion(this.vista.ListaAcciones, "UI DATOS TECNICOS"))
                {
                    this.vista.ValoresTabs = "1,";
                }
                if (!ExisteAccion(this.vista.ListaAcciones, "UI NUMEROSERIE"))
                {
                    this.vista.ValoresTabs += "2,";
                }
                if (!ExisteAccion(this.vista.ListaAcciones, "UI LLANTAS"))
                {
                    this.vista.ValoresTabs += "3,";
                }
                if (!ExisteAccion(this.vista.ListaAcciones, "UI EQUIPOALIADO"))
                {
                    this.vista.ValoresTabs += "4";
                }

                this.vista.ValoresTabs = this.vista.ValoresTabs.TrimEnd(',');
            }

            //Se agrega validación para habilitar o deshabilitar el botón de cargar fechas si es que se encuentra en estatus terminada al acta y es de tipo RE para generación o construcción
            if (this.vista.UnidadOperativaId != (int)ETipoEmpresa.Idealease)
            {
                vistaPagina1.MostrarBotonAgregarFechas(false);
                if (((EAreaGeneracion)this.vista.Area == EAreaGeneracion.RE || (EAreaConstruccion)this.vista.Area == EAreaConstruccion.RE || (EAreaEquinova)this.vista.Area == EAreaEquinova.RE) && (vista.EstatusUnidad == EEstatusUnidad.Terminada))
                    vistaPagina1.MostrarBotonAgregarFechas(true);
            }

            this.vista.EstablecerAcciones();
 
        }

        #endregion
        #endregion
    }
}
