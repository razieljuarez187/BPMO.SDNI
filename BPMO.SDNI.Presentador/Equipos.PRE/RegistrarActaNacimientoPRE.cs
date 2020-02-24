//Satisface al CU077 - Registrar Acta de Nacimiento de una Unidad
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using BPMO.Basicos.BO;
using BPMO.Facade.SDNI.BO;
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
using System.Configuration;
using System.Web.UI.WebControls;
using BPMO.SDNI.Comun.PRE;

namespace BPMO.SDNI.Equipos.PRE
{
    public class RegistrarActaNacimientoPRE
    {
        #region Atributos
        private UnidadBR controlador;
        private IDataContext dctx = null;

        private IRegistrarActaNacimientoVIS vista;
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
        private string nombreClase = "RegistrarActaNacimientoPRE";

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
        public RegistrarActaNacimientoPRE(IRegistrarActaNacimientoVIS view, IucDatosGeneralesVIS viewPage1, IucDatosTecnicosVIS viewPage2, IucNumerosSerieVIS viewPage3, IucAsignacionLlantasVIS viewPage4, IucAsignacionEquiposAliadosVIS viewPage5, IucTramitesActivosVIS viewPage6, IucResumenActaNacimientoVIS viewPage7)
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
        public void PrepararNuevo()
        {
            this.LimpiarSesion();

            this.vista.PrepararNuevo();
            this.presentadorPagina1.PrepararNuevo();
           
            this.presentadorPagina2.PrepararNuevo();
            this.presentadorPagina3.PrepararNuevo();
            this.presentadorPagina4.PrepararNuevo();
            this.presentadorPagina5.PrepararNuevo();
            this.presentadorPagina6.OcultarRedireccionTramites(true);
            this.presentadorPagina6.HabilitarPedimento(false);
            this.presentadorPagina6.HabilitarCargaArchivo(false);
            this.presentadorPagina6.CargarTramites(null, null, null);
            this.presentadorPagina7.PrepararNuevo();

            #region[REQ: 13285, Integración Generación y Construcción]
            if (this.vista.UnidadOperativaId == (int)ETipoEmpresa.Construccion || this.vista.UnidadOperativaId == (int)ETipoEmpresa.Generacion
                || this.vista.UnidadOperativaId == (int)ETipoEmpresa.Equinova)
            {
                TipoArchivoBR tipoArchivoBR = new TipoArchivoBR();
                List<TipoArchivoBO> lstTipos = tipoArchivoBR.Consultar(dctx, new TipoArchivoBO { Estatus = true, Extension="pdf" });
                this.presentadorPagina1.DesplegarTiposArchivos(lstTipos);
                this.presentadorPagina1.HabilitarCargaArchivoOC(false);
            }

            #endregion

            this.presentadorPagina4.EstablecerConfiguracionInicial(this.vista.UC);
            this.EstablecerInformacionInicial();
         
            this.vista.PermitirRegresar(false);
            this.vista.PermitirContinuar(true);
            this.IrAPagina(0);
            this.EstablecerSeguridad();
            //REQ: 13285, Modificaciones registro acta nacimiento
            this.EstablecerAcciones();     
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
                this.vista.LibroActivos = lstConfigUO[0].Libro;
                this.vista.NombreClienteUnidadOperativa = lstConfigUO[0].NombreCliente;

                if (string.IsNullOrEmpty(this.vista.LibroActivos) || string.IsNullOrWhiteSpace(this.vista.LibroActivos))
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
        public void EstablecerConfiguracionEspecialVista()
        {
            this.presentadorPagina4.EstablecerConfiguracionEspecialVista();
        }
        public void EstablecerSeguridad()
        {
            try
            {
                #region SC0008
                //Valida que el usuario y la unidad operativa no sean nulos
                if (this.vista.UC == null) throw new Exception("El identificador del usuario no debe ser nulo");
                if (this.vista.UnidadOperativaId == null)
                    throw new Exception("El identificador de la Unidad Operativa no debe ser nulo");
                // Creación del objeto seguridad
                UsuarioBO usuario = new UsuarioBO { Id = this.vista.UC };
                AdscripcionBO adscripcion = new AdscripcionBO
                    {
                        UnidadOperativa = new UnidadOperativaBO { Id = this.vista.UnidadOperativaId }
                    };
                SeguridadBO seguridad = new SeguridadBO(Guid.Empty, usuario, adscripcion);

                //consulta de acciones a la cual el usuario tiene permisos
                List<CatalogoBaseBO> acciones = FacadeBR.ConsultarAccion(dctx, seguridad);
                this.vista.ListaAcciones = acciones;
                this.ValidarPermisoTab();

                //Se valida si el usuario tiene permisos para registrar un nuevo acta de nacimiento
                if (!this.ExisteAccion(acciones, "REGISTRARDOCUMENTO"))
                    this.vista.RedirigirSinPermisoAcceso();

                //Se valida si el usuario tiene permisos para terminar un acta de nacimiento
                if (!this.ExisteAccion(acciones, "UI TERMINARDOCUMENTO"))
                    this.vista.PermitirGuardarTerminada(false);
                #endregion
                //Establecer las sucursales permitidas en las vistas correspondientes
                List<SucursalBO> lstSucursales = FacadeBR.ConsultarSucursalesSeguridad(this.dctx, new SeguridadBO(Guid.Empty, new UsuarioBO() { Id = this.vista.UC }, new AdscripcionBO { UnidadOperativa = new UnidadOperativaBO() { Id = this.vista.UnidadOperativaId } }));
                if (lstSucursales != null && lstSucursales.Count > 0)
                    this.vista.SucursalesSeguridad =
                        lstSucursales.Where(p => p != null && p.Id != null).ToList().ConvertAll(s => s.Id);
            }
            catch (Exception ex)
            {

                throw new Exception(this.nombreClase + ".EstablecerAccionesSeguridad:" + ex.Message);
            }
        }
        #region SC0008
        private bool ExisteAccion(List<CatalogoBaseBO> acciones, string nombreAccion)
        {
            if (acciones != null && acciones.Count > 0 &&
                acciones.Exists(
                    p => p != null && p.Nombre != null && p.Nombre.Trim().ToUpper() == nombreAccion.Trim().ToUpper()))
                return true;

            return false;
        }
        #endregion

        public void ValidarPermisoTab()
        {
            UsuarioBO usuario = new UsuarioBO { Id = this.vista.UC };
            AdscripcionBO adscripcion = new AdscripcionBO
                {
                    UnidadOperativa = new UnidadOperativaBO { Id = this.vista.UnidadOperativaId }
                };
            SeguridadBO seguridad = new SeguridadBO(Guid.Empty, usuario, adscripcion);

            //consulta de acciones a la cual el usuario tiene permisos
            List<CatalogoBaseBO> acciones = FacadeBR.ConsultarAccion(dctx, seguridad);
            this.vista.ListaAcciones = acciones;
            
            //Se obtienen las acciones a las cuales el usuario tiene permiso en este proceso
            #region[REQ: 13285, modificación del registro del acta de nacimiento]
            this.vista.ListaAcciones = acciones;
            this.vista.verTabDT = 1;
            this.vista.verTabNS = 1;
            this.vista.verTabLL = 1;
            this.vista.verTabEA = 1;
            //Validación de permisos de visualización de pestañas en el registro del acta de nacimiento
            if (!this.ExisteAccion(acciones, "UI DATOS TECNICOS"))
                this.vista.verTabDT = 0;
            if (!this.ExisteAccion(acciones, "UI NUMEROSERIE"))
                this.vista.verTabNS = 0;
            if (!this.ExisteAccion(acciones, "UI LLANTAS"))
                this.vista.verTabLL = 0;
            if (!this.ExisteAccion(acciones, "UI EQUIPOALIADO"))
                this.vista.verTabEA = 0;
            #endregion        
        }

        public void RetrocederPagina()
        {
            //RQM 14150, variables para determinar los tabs que no deben de ir en generación y construcción o que están ocultos
            int paginaRetroceder = 0;
            int paginaActualValor = 0;

            int? paginaActual = this.vista.PaginaActual;
            if (paginaActual == null)
                throw new Exception("La página actual es nula.");
            if (paginaActual <= 0)
                throw new Exception("La página actual es menor o igual a cero y, por lo tanto, no se puede retroceder.");

            this.EstablecerOpcionesSegunPagina(paginaActual.Value - 1);

            paginaActualValor = paginaActual.Value - 1;
            paginaRetroceder = paginaActualValor - 1;

            //RQM 13285, modificación para determinar si al retroceder no se tiene acceso o esta oculto el tab.
            if (this.vista.ValoresTabs.Contains(paginaRetroceder.ToString()))
            {
                this.vista.EstablecerPagina(paginaActual.Value - 1);
                RetrocederPagina();
            }
            else
            {
                this.vista.EstablecerPagina(paginaActual.Value - 1);
            }            
        }

        public void AvanzarPagina()
        {
            int? paginaActual = this.vista.PaginaActual;
            if (paginaActual == null)
                throw new Exception("La página actual es nula.");
            if (paginaActual >= 6)
                throw new Exception("La página actual es mayor o igual a 6 y, por lo tanto, no se puede avanzar.");

            #region RI0001
            if (paginaActual == 0)
            {
                string s = string.Empty;
                if (!(String.IsNullOrEmpty(s = this.ValidarCamposSeleccionUnidad())))
                {
                    this.vista.MostrarMensaje(s, ETipoMensajeIU.INFORMACION);
                    return;
                }
            }
            #endregion

            this.EstablecerOpcionesSegunPagina(paginaActual.Value + 1);

            //RQM 13285, modificación para determinar si al avanzar no se tiene acceso o esta oculto el tab.
            if (this.vista.ValoresTabs.Contains(paginaActual.Value.ToString()))
            {
                this.vista.EstablecerPagina(paginaActual.Value + 1);
                AvanzarPagina();
            }
            else
            {
                this.vista.EstablecerPagina(paginaActual.Value + 1);
            }            
        }

        public void IrAPagina(int numeroPagina)
        {
            if (numeroPagina < 0 || numeroPagina > 7)
                throw new Exception("La paginación va de 0 al 7.");

            this.EstablecerOpcionesSegunPagina(numeroPagina);
            this.vista.SucursalId = this.vistaPagina1.SucursalId;
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

            if (numeroPagina <= 0)
            {
                this.vista.PermitirRegresar(false);
                this.vista.PermitirGuardarBorrador(false);
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

        public void CancelarRegistro()
        {
            this.LimpiarSesion();
            this.vista.RedirigirAConsulta();
        }
        public int RegistrarBorrador()
        {
            int paquete = 0;
            //Primero se valida que no se haya insertado ya ese número de serie
            if (this.VerificarExistenciaActaNacimiento(new Servicio.Catalogos.BO.UnidadBO() { NumeroSerie = this.vista.NumeroSerie }))
            {
                this.vista.MostrarMensaje("La unidad que seleccionó ya tiene un acta de nacimiento", ETipoMensajeIU.INFORMACION, null);
                return paquete;
            }
            #region SC_0027
            //Se verifica que el código de la refacción no esté repetido
            string s;
            if ((s = this.presentadorPagina4.VerificarExistenciaCodigo(true)) != null)
            {
                this.vista.MostrarMensaje(s, ETipoMensajeIU.CONFIRMACION, "BORRADOR");
                return paquete;
            }
            #endregion

            //Antes de realizar cualquier otra cosa, se tiene que insertar en LIDER
            if (!this.RegistrarLider()) return paquete;

            if ((s = this.ValidarCamposBorrador()) != null)
            {
                this.vista.MostrarMensaje(s, ETipoMensajeIU.ADVERTENCIA, null);
                return paquete;
            }

            this.vista.EstatusUnidad = EEstatusUnidad.NoDisponible;
            string observaciones = "Borrador guardado. Sucursal: " + (this.vista.SucursalNombre ?? "");

            this.Registrar(observaciones);

            this.IrAPagina(7);
            string mensaje = this.ComprobarPaquetesMantenimiento();
            if (mensaje == string.Empty)
                this.vista.MostrarMensaje("Registro Exitoso", ETipoMensajeIU.EXITO, null);
            else
            {
                paquete = 1;
                this.vista.Mensaje = mensaje;
                //this.vista.MostrarMensaje("Registro Exitoso", ETipoMensajeIU.ERROR, mensaje);
            }
            //this.vista.MostrarMensaje("Registro Exitoso", ETipoMensajeIU.EXITO, null);
            return paquete;
        }
        public int RegistrarTerminada()
        {
            int paquete = 0;
            //Primero se valida que no se haya insertado ya ese número de serie
            if (this.VerificarExistenciaActaNacimiento(new Servicio.Catalogos.BO.UnidadBO() { NumeroSerie = this.vista.NumeroSerie }))
            {
                this.vista.MostrarMensaje("La unidad que seleccionó ya tiene un acta de nacimiento", ETipoMensajeIU.INFORMACION, null);
                return paquete;
            }

            #region SC_0027
            //Se verifica que el código de la refacción no esté repetido
            string s;
            if ((s = this.presentadorPagina4.VerificarExistenciaCodigo(true)) != null)
            {
                this.vista.MostrarMensaje(s, ETipoMensajeIU.CONFIRMACION, "TERMINAR");
                return paquete;
            }
            #endregion

            //Antes de realizar cualquier cosa, se tiene que insertar en LIDER
            if (!this.RegistrarLider()) return paquete;

            if ((s = this.ValidarCamposRegistro()) != null)
            {
                this.vista.MostrarMensaje(s, ETipoMensajeIU.ADVERTENCIA, null);
                return paquete;
            }

            //Si la unidad no tiene sus chunches configuradas no puede quedar terminada... es decir que siempre se va a guardar en borrador?
            //Se puede manejar un estatus mas que sea CONFIGURADA
            this.vista.EstatusUnidad = EEstatusUnidad.Terminada;
            string observaciones = "Sucursal: " + (this.vista.SucursalNombre != null ? this.vista.SucursalNombre : "");

            this.Registrar(observaciones);

            this.IrAPagina(7);
            #region SC0002
            string mensaje = this.ComprobarPaquetesMantenimiento();
            if (mensaje == string.Empty)
                this.vista.MostrarMensaje("Registro Exitoso", ETipoMensajeIU.EXITO, null);
            else
            {
                paquete = 1;
                this.vista.Mensaje = mensaje;
            }
            #endregion

            return paquete;
        }
        private void Registrar(string observaciones)
        {
            try
            {
                //Se obtiene la información a partir de la Interfaz de Usuario
                BO.UnidadBO bo = (BO.UnidadBO)this.InterfazUsuarioADato();
                
                if (bo.EstatusActual != null && (bo.EstatusActual == EEstatusUnidad.Terminada))
                    bo.ActaNacimiento = bo.ObtenerXml();

                //Se crea el objeto de seguridad
                UsuarioBO usuario = new UsuarioBO() { Id = this.vista.UC };
                AdscripcionBO adscripcion = new AdscripcionBO() { UnidadOperativa = new UnidadOperativaBO() { Id = this.vista.UnidadOperativaId } };
                SeguridadBO seguridadBO = new SeguridadBO(Guid.Empty, usuario, adscripcion);

                //Se inserta en la base de datos
                this.controlador.RegistrarDocumento(this.dctx, bo, observaciones, seguridadBO);

                //Se consulta lo insertado para recuperar los ID
                DataSet ds = this.controlador.ConsultarSet(this.dctx, bo);
                if (ds.Tables[0].Rows.Count <= 0)
                    throw new Exception("Al consultar lo insertado no se encontraron coincidencias.");
                if (ds.Tables[0].Rows.Count > 1)
                    throw new Exception("Al consultar lo insertado se encontró más de una coincidencia.");

                bo.EquipoID = this.controlador.DataRowToUnidadBO(ds.Tables[0].Rows[0]).EquipoID;
                bo.UnidadID = this.controlador.DataRowToUnidadBO(ds.Tables[0].Rows[0]).UnidadID;

                //Se despliega la información en la Interfaz de Usuario
                this.DatoAInterfazUsuario(bo);
            }
            catch (Exception ex)
            {
                throw new Exception(this.nombreClase + ".Registrar:" + ex.Message);
            }
        }

        /// <summary>
        /// Registra la unidad en LIDER, si no existe.
        /// </summary>
        /// <returns>Indica si la operación se llevó con éxito o no</returns>
        private bool RegistrarLider()
        {
            //Únicamente se ejecuta esto si no hay un LiderID
            if (this.vista.LiderID == null)
            {
                try
                {
                    #region Validar Existencia en Lider
                    List<Servicio.Catalogos.BO.UnidadBO> lstTemp = FacadeBR.ConsultarUnidad(dctx, (CatalogoBaseBO)(new Servicio.Catalogos.BO.UnidadBO() { NumeroSerie = this.vista.NumeroSerie }));
                    if (lstTemp.Count > 0)
                    {
                        this.vista.MostrarMensaje("El número de serie proporcionado existe en Líder, favor de seleccionarlo en la página inicial", ETipoMensajeIU.ADVERTENCIA);
                        return false;
                    }
                    #endregion

                    #region Validar Campos para Lider
                    string s = "";

                    if (this.vista.NumeroSerie.Trim().CompareTo("") == 0)
                        s += "VIN, ";
                    if (this.vista.NumeroEconomico.Trim().CompareTo("") == 0)
                        s += "# Económico, ";
                    if (this.vista.ClienteId == null)
                        s += "Cliente, ";
                    if (this.vista.FabricanteId == null)
                        s += "Fabricante, ";
                    if (this.vista.ModeloId == null)
                        s += "Modelo, ";
                    if (this.vista.MotorizacionId == null)
                        s += "Motorización, ";
                    if (this.vista.AplicacionId == null)
                        s += "Aplicación, ";
                    if (this.vista.TipoUnidadId == null)
                        s += "Tipo de Unidad, ";
                    if (this.vista.FechaProximoServicio == null)
                        s += "Fecha de Próximo Servicio, ";
                    if (this.vista.KilometrajeProximoServicio == null)
                        s += "Próximo Servicio (Km), ";
                    if (this.vista.KilometrajeInicial == null)
                        s += "Inicial (Km), ";

                    if (s.Trim().CompareTo("") != 0)
                    {
                        this.vista.MostrarMensaje("Los siguientes campos no pueden estar vacíos para registrar la unidad en LIDER: \n" + s.Substring(0, s.Length - 2), ETipoMensajeIU.ADVERTENCIA);
                        return false;
                    }
                    #endregion

                    Servicio.Catalogos.BO.UnidadBO bo = new Servicio.Catalogos.BO.UnidadBO();

                    #region Interfaz de Usuario a Unidad
                    bo.NumeroSerie = this.vista.NumeroSerie;
                    bo.Clave = this.vista.NumeroEconomico;
                    bo.Activo = true;

                    bo.Auditoria = new AuditoriaBO();
                    bo.Auditoria.FC = this.vista.FC;
                    bo.Auditoria.FUA = this.vista.FUA;
                    bo.Auditoria.UC = this.vista.UC;
                    bo.Auditoria.UUA = this.vista.UUA;

                    bo.ClasificadorAplicacion = new ClasificadorAplicacionBO();
                    bo.ClasificadorAplicacion.Id = this.vista.AplicacionId;
                    bo.ClasificadorAplicacion.Nombre = this.vista.AplicacionNombre;

                    bo.Cliente = new ClienteBO();
                    bo.Cliente.Id = this.vista.ClienteId;
                    bo.Cliente.Nombre = this.vista.Cliente;

                    bo.ConfiguracionModeloMotorizacion = new ConfiguracionModeloMotorizacionBO();
                    bo.ConfiguracionModeloMotorizacion.ClasificadorMotorizacion = new ClasificadorMotorizacionBO();
                    bo.ConfiguracionModeloMotorizacion.ClasificadorMotorizacion.Id = this.vista.MotorizacionId;
                    bo.ConfiguracionModeloMotorizacion.ClasificadorMotorizacion.Nombre = this.vista.MotorizacionNombre;
                    bo.ConfiguracionModeloMotorizacion.Modelo = new ModeloBO();
                    bo.ConfiguracionModeloMotorizacion.Modelo.Id = this.vista.ModeloId;
                    bo.ConfiguracionModeloMotorizacion.Modelo.Nombre = this.vista.ModeloNombre;

                    bo.Distribuidor = new DistribuidorBO();
                    bo.Distribuidor.Id = this.vista.FabricanteId;
                    bo.Distribuidor.Nombre = this.vista.FabricanteNombre;

                    bo.FechaProximoServicio = this.vista.FechaProximoServicio;
                    bo.KmHrsInicial = this.vista.KilometrajeInicial;
                    bo.KmHrsProximoServicio = this.vista.KilometrajeProximoServicio;
                    bo.KmHrs = false; //false = KM, true = HRS

                    bo.TipoUnidad = new TipoUnidadBO();
                    bo.TipoUnidad.Id = this.vista.TipoUnidadId;
                    bo.TipoUnidad.Nombre = this.vista.TipoUnidadNombre;
                    #endregion

                    #region Generar Características de Unidad
                    CaracteristicaPlantillaBO caracteristicaPlantilla = new CaracteristicaPlantillaBO();
                    caracteristicaPlantilla.TipoUnidad = bo.TipoUnidad;

                    List<CaracteristicaPlantillaBO> caracteristicas = FacadeBR.ConsultarCaracteristicaPlantilla(this.dctx, caracteristicaPlantilla);
                    foreach (CaracteristicaPlantillaBO caracteristica in caracteristicas)
                    {
                        CaracteristicaUnidadBO cu = new CaracteristicaUnidadBO();
                        cu.Caracteristica = caracteristica.Caracteristica;
                        cu.Valor = "PENDIENTE";
                        bo.Agregar(cu);
                    }
                    #endregion

                    //Se crea el objeto de seguridad
                    UsuarioBO usuario = new UsuarioBO() { Id = this.vista.UC };
                    AdscripcionBO adscripcion = new AdscripcionBO() { UnidadOperativa = new UnidadOperativaBO() { Id = this.vista.UnidadOperativaId } };
                    SeguridadBO seguridadBO = new SeguridadBO(Guid.Empty, usuario, adscripcion);

                    FacadeBR.InsertarUnidadCompleta(dctx, bo, seguridadBO);

                    List<Servicio.Catalogos.BO.UnidadBO> lst = FacadeBR.ConsultarUnidad(dctx, (CatalogoBaseBO)bo);

                    if (lst.Count != 1)
                        throw new Exception("No se encontró la unidad insertada en LIDER. Favor de contactar a un administrador.");

                    bo = lst[0];
                    this.vista.LiderID = bo.Id;
                }
                catch (Exception ex)
                {
                    throw new Exception(this.nombreClase + ".RegistrarLider: " + ex.Message);
                }
            }

            return true;
        }

        

        private object InterfazUsuarioADato()
        {
            BO.UnidadBO bo = new BO.UnidadBO();
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

            bo.UnidadID = this.vista.UnidadId;
            bo.EquipoID = this.vista.EquipoId;
            bo.IDLider = this.vista.LiderID;
            bo.ClaveActivoOracle = this.vista.ClaveActivoOracle;
            bo.Anio = this.vista.Anio;
            bo.Modelo.Nombre = this.vista.ModeloNombre;
            bo.Modelo.Id = this.vista.ModeloId;
            bo.Modelo.Marca.Nombre = this.vista.MarcaNombre;
            bo.Modelo.Marca.Id = this.vista.MarcaId;
            bo.NumeroEconomico = this.vista.NumeroEconomico;
            bo.NumeroSerie = this.vista.NumeroSerie;
            bo.Fabricante = this.vista.FabricanteNombre;
            bo.TipoEquipoServicio.Nombre = this.vista.TipoUnidadNombre;

            #region SC0001
            bo.KilometrajeInicial = this.vista.KilometrajeInicial;
            bo.HorasIniciales = this.vista.HorasIniciales;
            bo.CombustibleConsumidoTotal = this.vista.CombustibleTotal;
            #endregion
            #region SC0002
            bo.EntraMantenimiento = this.vista.EntraMantenimiento;
            #endregion

            #region [RQM 13285 Integración de generación y construcción]
            bo.Accesorio = this.vista.Accesorio;
            bo.ActivoFijo.FechaInicioDepreciacion = this.vista.FechaInicioDepreciacion;
            bo.ActivoFijo.FechaIdealDesflote      = this.vista.FechaIdealDesflote;
            bo.ActivoFijo.PorcentajeDepreciacion  = this.vista.TasaDepreciacion;
            bo.ActivoFijo.MesesVidaUtil           = this.vista.VidaUtil;
            bo.ActivoFijo.PorcentajeResidual      = this.vista.PorcentajeValorResidual.GetValueOrDefault();
            bo.ActivoFijo.ImporteResidual         = this.vista.ValorResidual;
            bo.ActivoFijo.ValorLibro              = this.vista.SaldoPorDepreciar;
            #endregion

            bo.Area = this.vista.Area;
            if (bo.Area != null)
            {
                if (this.vista.UnidadOperativaId == (int)ETipoEmpresa.Construccion)
                {
                    bo.Area = this.vistaPagina1.ETipoRentaConstruccion;
                    if (EAreaConstruccion.RE == (EAreaConstruccion)bo.Area)
                    {
                        bo.Arrendamientos = (List<ArrendamientoBO>)this.InterfazUsuarioADatoArrendamiento();
                    }
                }
                if (this.vista.UnidadOperativaId == (int)ETipoEmpresa.Generacion)
                {
                    bo.Area = this.vistaPagina1.ETipoRentaGeneracion;
                    if (EAreaGeneracion.RE == (EAreaGeneracion)bo.Area)
                    {
                        bo.Arrendamientos = (List<ArrendamientoBO>)this.InterfazUsuarioADatoArrendamiento();
                    }
                }
                if (this.vista.UnidadOperativaId == (int)ETipoEmpresa.Equinova) {
                    bo.Area = this.vistaPagina1.ETipoRentaEquinova;
                    if (EAreaEquinova.RE == (EAreaEquinova)bo.Area) {
                        bo.Arrendamientos = (List<ArrendamientoBO>)this.InterfazUsuarioADatoArrendamiento();
                    }
                }
            }
            bo.Propietario = this.vista.Propietario;
            bo.Sucursal.Nombre = this.vista.SucursalNombre;
            bo.Sucursal.Id = this.vista.SucursalId;
            bo.Sucursal.UnidadOperativa.Id = this.vista.UnidadOperativaId;
            bo.Cliente.Nombre = this.vista.Cliente;
            bo.Cliente.Id = this.vista.ClienteId;
            bo.EquipoBloqueado = vistaPagina1.UnidadBloqueada;

            #region

            #endregion

            bo.Mediciones.Horometros = this.vista.Horometros;
            bo.Mediciones.Odometros = this.vista.Odometros;
            bo.CaracteristicasUnidad.CapacidadTanque = this.vista.CapacidadTanque;
            bo.CaracteristicasUnidad.RendimientoTanque = this.vista.RendimientoTanque;
            bo.CaracteristicasUnidad.PBCMaximoRecomendado = this.vista.PBCMaximoRecomendado;
            bo.CaracteristicasUnidad.PBVMaximoRecomendado = this.vista.PBVMaximoRecomendado;

            bo.CaracteristicasUnidad.Radiador = this.vista.Radiador;
            bo.CaracteristicasUnidad.PostEnfriador = this.vista.PostEnfriador;
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

            bo.AgregarLlantas(this.vista.Llantas);
            foreach (var llanta in bo.Llantas) {
                if (llanta.Sucursal == null || llanta.Sucursal.Id == null) {
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
                    refaccion.Auditoria.FC = this.vista.FC;
                    refaccion.Auditoria.UC = this.vista.UC;
                }
                refaccion.Auditoria.FUA = this.vista.FC;
                refaccion.Auditoria.UUA = this.vista.UC;

                bo.AgregarLlanta(refaccion);
            }
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
        
            
            bo.NumerosSerie = this.vista.NumerosSerie;
            
            if (this.vista.ClaveActivoOracle != null)
                bo.EsActivo = true;
            else
                bo.EsActivo = false;
            bo.FC = this.vista.FC;
            bo.FUA = this.vista.FUA;
            bo.UC = this.vista.UC;
            bo.UUA = this.vista.UUA;
            bo.EstatusActual = this.vista.EstatusUnidad;

            return bo;
        }

        private object InterfazUsuarioADatoArrendamiento()
        {
            List<ArrendamientoBO> lstArrendamiento = new List<ArrendamientoBO>();
            BO.ArrendamientoBO arrendamientoBO = new BO.ArrendamientoBO();
            arrendamientoBO.Proveedor = new ProveedorBO();
            arrendamientoBO.Proveedor.Id = this.vista.ProveedorID;
            arrendamientoBO.MontoArrendamiento = this.vista.MontoArrendamiento;
            arrendamientoBO.NumeroOrdenCompra = this.vista.OrdenCompraProveedor;
            arrendamientoBO.CodigoMoneda = this.vista.CodigoMoneda;
            arrendamientoBO.FechaInicioArrendamiento = this.vista.FechaInicioArrendamiento;
            arrendamientoBO.FechaFinArrendamiento = this.vista.FechaFinArrendamiento;
            arrendamientoBO.EquipoID = this.vista.UnidadId;
            arrendamientoBO.Adjuntos = (List<ArchivoBO>)this.InterfazUsuarioADatoOC();
            arrendamientoBO.Activo = true;
            arrendamientoBO.UC = this.vista.UC;
            arrendamientoBO.FC = this.vista.FC;
            arrendamientoBO.UUA = this.vista.UUA;
            arrendamientoBO.FUA = this.vista.FUA;
            lstArrendamiento.Add(arrendamientoBO);
            return lstArrendamiento;
        }
        private object InterfazUsuarioADatoOC()
        {
            List<ArchivoBO> archivos = new List<ArchivoBO>();
            if (this.vista.ArchivosOC != null)
            {
                //Asignamos valores de auditoria
                foreach (ArchivoBO archivo in this.vista.ArchivosOC)
                {
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

        private void DatoAInterfazUsuario(object obj)
        {
            BO.UnidadBO bo = (BO.UnidadBO)obj;

            this.vista.EquipoId = bo.EquipoID;
            this.vista.UnidadId = bo.UnidadID;

            this.presentadorPagina7.DesplegarInformacion(bo);
        }

        /// <summary>
        /// Indica, en base a la información de una unidad, si existe ya un acta de nacimiento para dicha unidad o no
        /// </summary>
        /// <param name="unidadBO">Unidad de la que se usarpa la información para comparar</param>
        /// <returns>Verdadero en caso de que exista, Falso en caso contrario</returns>
        public bool VerificarExistenciaActaNacimiento(object unidadBO)
        {
            string numeroSerie = null;

            if (unidadBO == null) return false;
            if (!(unidadBO is EquipoBepensaBO) && !(unidadBO is Servicio.Catalogos.BO.UnidadBO) && !(unidadBO is ActivoFijoBO))
                throw new Exception("Se esperaba un EquipoBepensa, una Unidad de Servicio o un Activo Fijo.");

            if (unidadBO is EquipoBepensaBO)
                numeroSerie = ((EquipoBepensaBO)unidadBO).NumeroSerie;
            if (unidadBO is Servicio.Catalogos.BO.UnidadBO)
                numeroSerie = ((Servicio.Catalogos.BO.UnidadBO)unidadBO).NumeroSerie;
            if (unidadBO is ActivoFijoBO)
                numeroSerie = ((ActivoFijoBO)unidadBO).NumeroSerie;

            if (numeroSerie == null)
                throw new Exception("El número de serie es requerido.");

            #region RI0041
            bool existe = this.controlador.VerificarExistenciaEquipo(this.dctx, numeroSerie, null);
            #endregion RI0041
            return existe;
        }

        public string ValidarCamposBorrador()
        {
            string s = null;

            if ((s = this.ValidarCamposSeleccionUnidad()) != null)
                return "Selección de Unidad:" + s;

            if ((s = this.presentadorPagina1.ValidarCamposBorrador()) != null)
                return "Paso 1:" + s;
            if (string.IsNullOrEmpty(this.vista.ClaveActivoOracle) && this.vista.Area != null && (this.vista.Area == (Enum)EArea.RD || this.vista.Area == (Enum)EArea.FSL))
                return "Paso 1:La unidad seleccionada no es un activo fijo y, por lo tanto, no puede estar en Renta Diaria o Full Service Leasing";

            if ((s = this.presentadorPagina2.ValidarCamposBorrador()) != null)
                return "Paso 2:" + s;
            if ((s = this.presentadorPagina3.ValidarCamposBorrador()) != null)
                return "Paso 3:" + s;
            if ((s = this.presentadorPagina4.ValidarCamposBorrador()) != null)
                return "Paso 4:" + s;
            if ((s = this.presentadorPagina5.ValidarCamposBorrador()) != null)
                return "Paso 5:" + s;

            return null;
        }
        public string ValidarCamposRegistro()
        {
            string s = null;

            if ((s = this.ValidarCamposSeleccionUnidad()) != null)
                return "Selección de Unidad:" + s;

            if ((s = this.presentadorPagina1.ValidarCamposRegistro()) != null)
                return "Paso 1:" + s;
            if (string.IsNullOrEmpty(this.vista.ClaveActivoOracle) && this.vista.Area != null && (this.vista.Area == (Enum)EArea.RD || this.vista.Area == (Enum)EArea.FSL))
                return "Paso 1:La unidad seleccionada no es un activo fijo y, por lo tanto, no puede estar en Renta Diaria o Full Service Leasing";

            if ((s = this.presentadorPagina2.ValidarCamposRegistro()) != null && this.ExisteAccion(this.vista.ListaAcciones, "UI DATOS TECNICOS"))
                return "Paso 2:" + s;
            if ((s = this.presentadorPagina3.ValidarCamposRegistro()) != null && this.ExisteAccion(this.vista.ListaAcciones, "UI NUMEROSERIE"))
                return "Paso 3:" + s;
            if ((s = this.presentadorPagina4.ValidarCamposRegistro()) != null && this.ExisteAccion(this.vista.ListaAcciones, "UI LLANTAS"))
                return "Paso 4:" + s;
            if ((s = this.presentadorPagina5.ValidarCamposRegistro()) != null && this.ExisteAccion(this.vista.ListaAcciones, "UI EQUIPOALIADO"))
                return "Paso 5:" + s;

            //Validación que solo aplica para idealease
            if (this.vista.Area != null && (this.vista.Area == (Enum)EArea.RD || this.vista.Area == (Enum)EArea.FSL) && ((ETipoEmpresa)this.vista.UnidadOperativaId == ETipoEmpresa.Idealease))
            {
                if(this.vistaPagina5.EquiposAliados.Any())
                {
                    foreach(var bo in this.vistaPagina5.EquiposAliados)
                    {
                        if(!(bo.ActivoFijo != null && !String.IsNullOrEmpty(bo.ActivoFijo.NumeroActivo)))
                        {
                            s = String.IsNullOrEmpty(s) ? String.Empty : s + ", ";
                            s = s + bo.NumeroSerie;
                        }
                    }
                }
                if(!String.IsNullOrEmpty(s))
                    return "Paso 5: Los siguientes Equipos Aliados NO son Activos Fijos, por lo tanto, no pueden estar en Renta Diaria o Full Service Leasing: " + s;
            }
            if ((s = this.ValidarCamposRegistroTramites()) != null && ((ETipoEmpresa)this.vista.UnidadOperativaId == ETipoEmpresa.Idealease))
                return "Paso 6:" + s;

            return null;
        }

        /// <summary>
        /// Valida la información del resource para mostrar u ocultar un tab del registro
        /// </summary>
        /// <param name="etiqueta"></param>
        /// <returns></returns>
        private bool ValidarTab(string etiqueta)
        {
            bool validartab = true;
            int activotab = 0;
            if (ETipoEmpresa.Idealease != (ETipoEmpresa)this.vista.UnidadOperativaId)
            {
                activotab = this.vista.ValidarTab(etiqueta);
                validartab = activotab == 1 ? true : false;
            }
            return validartab;
        }

        private string ValidarCamposRegistroTramites()
        {
            string s = "";

            #region[REQ: 13285, Integración Generación y Construcción]
            bool EsIdealease = true;
            if ((int)ETipoEmpresa.Generacion == this.vista.UnidadOperativaId || (int)ETipoEmpresa.Construccion == this.vista.UnidadOperativaId
                || (int)ETipoEmpresa.Equinova == this.vista.UnidadOperativaId)
                EsIdealease = false;
            #endregion

            if (!this.presentadorPagina6.ExisteTramite(ETipoTramite.PLACA_ESTATAL) && !this.presentadorPagina6.ExisteTramite(ETipoTramite.PLACA_FEDERAL) && EsIdealease)
                s += "Placa Estatal y/o Federal, ";

            if (this.vista.Area == (Enum)EArea.FSL || this.vista.Area == (Enum)EArea.RD)
            {
                if (!this.presentadorPagina6.ExisteTramite(ETipoTramite.SEGURO) && EsIdealease)
                    s += "Seguro, ";
                if (!this.presentadorPagina6.ExisteTramite(ETipoTramite.GPS) && EsIdealease)
                    s += "GPS, ";
                if (!this.presentadorPagina6.ExisteTramite(ETipoTramite.FILTRO_AK) && EsIdealease)
                    s += "Filtro AK, ";
                if (!this.presentadorPagina6.ExisteTramite(ETipoTramite.VERIFICACION_AMBIENTAL) && EsIdealease)
                    s += "Verificación Ambiental, ";
                if (!this.presentadorPagina6.ExisteTramite(ETipoTramite.VERIFICACION_FISICO_MECANICA) && EsIdealease)
                    s += "Verificación Físico-Mecánica, ";
                if (!this.presentadorPagina6.ExisteTramite(ETipoTramite.TENENCIA) && EsIdealease)
                    s += "Tenencia, ";
            }

            if (s.Trim().CompareTo("") != 0)
                return "Los siguientes trámites son requeridos: \n" + s.Substring(0, s.Length - 2);

            return null;
        }

        public string ValidarCamposSeleccionUnidad()
        {
            string s = "";

            if (!(this.vista.NumeroSerie != null && this.vista.NumeroSerie.Trim().CompareTo("") != 0))
                s += "Número de Serie, ";

            if (this.vista.LiderID == null)
            {
                if (String.IsNullOrEmpty(this.vista.NumeroEconomico))
                    s += "Número Económico, ";
                if (this.vista.FabricanteId == null)
                    s += "Fabricante, ";
                if (this.vista.ModeloId == null)
                    s += "Modelo, ";
                if (this.vista.MotorizacionId == null)
                    s += "Motorización, ";
                if (this.vista.AplicacionId == null)
                    s += "Aplicación, ";
                if (this.vista.TipoUnidadId == null)
                    s += "Tipo de Unidad, ";
                if (this.vista.FechaProximoServicio == null)
                    s += "Próximo Servicio, ";
                if (this.vista.KilometrajeProximoServicio == null)
                    s += "Km para el Próximo Servicio, ";
                if (this.vista.UnidadOperativaId == (int)ETipoEmpresa.Idealease)
                {
                    if (this.vista.KilometrajeInicial == null)
                        s += "Kilometraje Inicial, ";
                }
            }

            if (s.Trim().CompareTo("") != 0)
                return "Los siguientes campos no pueden estar vacíos: \n" + s.Substring(0, s.Length - 2);

            return null;
        }
        public string ValidarCamposConsultaUnidad()
        {
            string s = "";

            if (!(this.vista.NumeroSerie != null && this.vista.NumeroSerie.Trim().CompareTo("") != 0))
                s += "Número de Serie, ";

            if (s.Trim().CompareTo("") != 0)
                return "Los siguientes campos no pueden estar vacíos: \n" + s.Substring(0, s.Length - 2);

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
        #region Métodos para el Buscador
        public object PrepararBOBuscador(string catalogo)
        {
            object obj = null;

            switch (catalogo)
            {
                case "Unidad":
                    EquipoBepensaBO ebBO = new EquipoBepensaBO();
                    ebBO.ActivoFijo = new ActivoFijoBO();
                    ebBO.Unidad = new Servicio.Catalogos.BO.UnidadBO();

                    ebBO.Unidad.NumeroSerie = this.vista.NumeroSerie;
                    ebBO.Unidad.ConfiguracionModeloMotorizacion = new ConfiguracionModeloMotorizacionBO();
                    ebBO.Unidad.ConfiguracionModeloMotorizacion.Modelo = new ModeloBO();
                    ebBO.Unidad.ConfiguracionModeloMotorizacion.Modelo.Marca = new MarcaBO();
                    ebBO.Unidad.ConfiguracionModeloMotorizacion.Modelo.Id = this.vista.ModeloId;
                    ebBO.Unidad.ConfiguracionModeloMotorizacion.Modelo.Marca.Id = this.vista.MarcaId;
                    ebBO.Unidad.Activo = true;

                    ebBO.Unidad.NumeroSerie = this.vista.NumeroSerie;
                    ebBO.ActivoFijo.NumeroSerie = this.vista.NumeroSerie;
                    ebBO.ActivoFijo.Activo = true;
                    ebBO.ActivoFijo.Libro = this.vista.LibroActivos;

                    obj = ebBO;
                    break;
                case "TipoUnidad":
                    TipoUnidadBO tipoUnidad = new TipoUnidadBO();

                    tipoUnidad.Nombre = this.vista.TipoUnidadNombre;
                    tipoUnidad.Activo = true;

                    obj = tipoUnidad;
                    break;
                case "Marca":
                    MarcaBO marca = new MarcaBO();

                    marca.Nombre = this.vista.MarcaNombre;
                    marca.Activo = true;

                    obj = marca;
                    break;
                case "Modelo":
                    ModeloBO modelo = new ModeloBO();
                    modelo.Auditoria = new AuditoriaBO();
                    modelo.Marca = new MarcaBO();

                    modelo.Marca.Id = this.vista.MarcaId;
                    modelo.Nombre = this.vista.ModeloNombre;
                    modelo.Activo = true;

                    obj = modelo;
                    break;
                case "Distribuidor":
                    DistribuidorBO distribuidor = new DistribuidorBO();
                    distribuidor.Auditoria = new AuditoriaBO();

                    distribuidor.Nombre = this.vista.FabricanteNombre;
                    distribuidor.Activo = true;

                    obj = distribuidor;
                    break;
                case "Motorizacion":
                    ConfiguracionModeloMotorizacionBO motorizacion = new ConfiguracionModeloMotorizacionBO();
                    motorizacion.Modelo = new ModeloBO();
                    motorizacion.ClasificadorMotorizacion = new ClasificadorMotorizacionBO();
                    motorizacion.Auditoria = new AuditoriaBO();

                    motorizacion.Modelo.Id = this.vista.ModeloId;
                    motorizacion.ClasificadorMotorizacion.Nombre = this.vista.MotorizacionNombre;
                    motorizacion.Activo = true;
                    motorizacion.ClasificadorMotorizacion.Activo = true;

                    obj = motorizacion;
                    break;
                case "Aplicacion":
                    ClasificadorAplicacionBO aplicacion = new ClasificadorAplicacionBO();
                    aplicacion.Auditoria = new AuditoriaBO();

                    aplicacion.Nombre = this.vista.AplicacionNombre;
                    aplicacion.Activo = true;

                    obj = aplicacion;
                    break;
            }

            return obj;
        }
        public void DesplegarResultadoBuscador(string catalogo, object selecto)
        {
            switch (catalogo)
            {
                case "Unidad":
                    #region Desplegar Unidad
                    EquipoBepensaBO ebBO = new EquipoBepensaBO();
                    if (selecto != null)
                        ebBO = (EquipoBepensaBO)selecto;

                    if (ebBO.NumeroSerie != null)
                    {
                        this.vista.NumeroSerie = ebBO.NumeroSerie;
                        this.vista.HabilitarUnidad(false);
                        this.vista.HabilitarActivoFijo(false);
                    }
                    else
                    {
                        this.vista.NumeroSerie = null;
                        this.vista.HabilitarUnidad(true);
                        this.vista.HabilitarActivoFijo(true);
                     
                        
                    }

                 
                    #region Unidad
                    if (ebBO.Unidad != null)
                    {
                        if (ebBO.Unidad.Id != null)
                        {
                            this.vista.LiderID = ebBO.Unidad.Id;
                            this.vista.HabilitarUnidad(false);
                            this.vistaPagina1.UnidadBloqueada = FacadeBR.UnidadBloqueada(dctx, null, ebBO.Unidad.Id);
                            if(this.vistaPagina1.UnidadBloqueada.Value)
                                vistaPagina1.HabilitarUnidadBloqueada(false);
                            if (ebBO.Unidad.KmHrsInicial != null)
                            {
                                if (ebBO.Unidad.KmHrs == true)
                                {
                                    this.vista.HabilitarHRSInicial(false);
                                    this.vista.HabilitarKMInicial(true);
                                }
                                else if (ebBO.Unidad.KmHrs == false)
                                {
                                    this.vista.HabilitarKMInicial(false);
                                    this.vista.HabilitarHRSInicial(true);
                                }

                            }
                        }
                        else
                        {
                            this.vista.LiderID = null;
                            this.vista.HabilitarUnidad(true);
                            this.vistaPagina1.UnidadBloqueada = false;
                            if(this.vistaPagina1.UnidadBloqueada.Value)
                                vistaPagina1.HabilitarUnidadBloqueada(true);
                            this.vista.HabilitarHRSInicial(true);
                            this.vista.HabilitarKMInicial(true);
                        }
                        if (this.vista.UnidadOperativaId != (int)ETipoEmpresa.Idealease)
                            this.vista.HabilitarHRSInicial(true);

                        //RI083
                        this.vista.NumeroEconomico = !string.IsNullOrEmpty(ebBO.Unidad.Clave) && !string.IsNullOrWhiteSpace(ebBO.Unidad.Clave) ? ebBO.Unidad.Clave.Trim().ToUpper() : string.Empty;

                        if (ebBO.Unidad.FechaProximoServicio != null)
                            this.vista.FechaProximoServicio = ebBO.Unidad.FechaProximoServicio;
                        else
                            this.vista.FechaProximoServicio = null;

                        if (ebBO.Unidad.KmHrsProximoServicio != null)
                            this.vista.KilometrajeProximoServicio = ebBO.Unidad.KmHrsProximoServicio;
                        else
                            this.vista.KilometrajeProximoServicio = null;

                        if (ebBO.Unidad.KmHrsInicial != null)
                        {
                            if (ebBO.Unidad.KmHrs == true)
                            {
                                this.vista.HorasIniciales = ebBO.Unidad.KmHrsInicial;
                            }
                            else if (ebBO.Unidad.KmHrs == false)
                            {
                                this.vista.KilometrajeInicial = ebBO.Unidad.KmHrsInicial;
                            }
                        }
                        else
                        {
                            this.vista.KilometrajeInicial = null;
                            this.vista.HorasIniciales = null;
                        }

                        #region MyRegion
                        
                        #endregion

                        #region TipoUnidad
                        if (ebBO.Unidad.TipoUnidad != null)
                        {
                            if (ebBO.Unidad.TipoUnidad.Id != null)
                                this.vista.TipoUnidadId = ebBO.Unidad.TipoUnidad.Id;
                            else
                                this.vista.TipoUnidadId = null;

                            if (ebBO.Unidad.TipoUnidad.Nombre != null)
                                this.vista.TipoUnidadNombre = ebBO.Unidad.TipoUnidad.Nombre;
                            else
                                this.vista.TipoUnidadNombre = null;
                        }
                        else
                        {
                            this.vista.TipoUnidadId = null;
                            this.vista.TipoUnidadNombre = null;
                        }
                        #endregion

                        #region ConfiguracionModeloMotorizacion
                        if (ebBO.Unidad.ConfiguracionModeloMotorizacion != null)
                        {
                            #region Modelo
                            if (ebBO.Unidad.ConfiguracionModeloMotorizacion.Modelo != null)
                            {
                                if (ebBO.Unidad.ConfiguracionModeloMotorizacion.Modelo.Id != null)
                                    this.vista.ModeloId = ebBO.Unidad.ConfiguracionModeloMotorizacion.Modelo.Id;
                                else
                                    this.vista.ModeloId = null;

                                if (ebBO.Unidad.ConfiguracionModeloMotorizacion.Modelo.Nombre != null)
                                    this.vista.ModeloNombre = ebBO.Unidad.ConfiguracionModeloMotorizacion.Modelo.Nombre;
                                else
                                    this.vista.ModeloNombre = null;

                                #region Marca
                                if (ebBO.Unidad.ConfiguracionModeloMotorizacion.Modelo.Marca != null)
                                {
                                    if (ebBO.Unidad.ConfiguracionModeloMotorizacion.Modelo.Marca.Id != null)
                                    {
                                        this.vista.MarcaId = ebBO.Unidad.ConfiguracionModeloMotorizacion.Modelo.Marca.Id;
                                        if (ebBO.Unidad.Id == null)
                                            this.vista.HabilitarModelo(true);
                                    }
                                    else
                                    {
                                        this.vista.MarcaId = null;
                                        this.vista.HabilitarModelo(false);
                                    }

                                    if (ebBO.Unidad.ConfiguracionModeloMotorizacion.Modelo.Marca.Nombre != null)
                                        this.vista.MarcaNombre = ebBO.Unidad.ConfiguracionModeloMotorizacion.Modelo.Marca.Nombre;
                                    else
                                        this.vista.MarcaNombre = null;
                                }
                                else
                                {
                                    this.vista.MarcaId = null;
                                    this.vista.MarcaNombre = null;
                                    this.vista.HabilitarModelo(false);
                                }
                                #endregion
                            }
                            else
                            {
                                this.vista.ModeloId = null;
                                this.vista.ModeloNombre = null;
                                this.vista.MarcaId = null;
                                this.vista.MarcaNombre = null;
                            }
                            #endregion

                            #region ClasificadorMotorizacion
                            if (ebBO.Unidad.ConfiguracionModeloMotorizacion.ClasificadorMotorizacion != null)
                            {
                                if (ebBO.Unidad.ConfiguracionModeloMotorizacion.ClasificadorMotorizacion.Id != null)
                                    this.vista.MotorizacionId = ebBO.Unidad.ConfiguracionModeloMotorizacion.ClasificadorMotorizacion.Id;
                                else
                                    this.vista.MotorizacionId = null;

                                if (ebBO.Unidad.ConfiguracionModeloMotorizacion.ClasificadorMotorizacion.Nombre != null)
                                    this.vista.MotorizacionNombre = ebBO.Unidad.ConfiguracionModeloMotorizacion.ClasificadorMotorizacion.Nombre;
                                else
                                    this.vista.MotorizacionNombre = null;
                            }
                            else
                            {
                                this.vista.MotorizacionId = null;
                                this.vista.MotorizacionNombre = null;
                            }
                            #endregion
                        }
                        else
                        {
                            this.vista.ModeloId = null;
                            this.vista.ModeloNombre = null;
                            this.vista.MarcaId = null;
                            this.vista.MarcaNombre = null;
                            this.vista.MotorizacionId = null;
                            this.vista.MotorizacionNombre = null;
                        }

                        #region Anio
                        this.vista.Anio = ebBO.Unidad.Anio.HasValue ? ebBO.Unidad.Anio : null;
                        #endregion
                        #endregion

                        #region Distribuidor
                        if (ebBO.Unidad.Distribuidor != null)
                        {
                            if (ebBO.Unidad.Distribuidor.Id != null)
                                this.vista.FabricanteId = ebBO.Unidad.Distribuidor.Id;
                            else
                                this.vista.FabricanteId = null;

                            if (ebBO.Unidad.Distribuidor.Nombre != null)
                                this.vista.FabricanteNombre = ebBO.Unidad.Distribuidor.Nombre;
                            else
                                this.vista.FabricanteNombre = null;
                        }
                        else
                        {
                            this.vista.FabricanteId = null;
                            this.vista.FabricanteNombre = null;
                        }
                        #endregion

                        #region ClasificadorAplicacion
                        if (ebBO.Unidad.ClasificadorAplicacion != null)
                        {
                            if (ebBO.Unidad.ClasificadorAplicacion.Id != null)
                                this.vista.AplicacionId = ebBO.Unidad.ClasificadorAplicacion.Id;
                            else
                                this.vista.AplicacionId = null;

                            if (ebBO.Unidad.ClasificadorAplicacion.Nombre != null)
                                this.vista.AplicacionNombre = ebBO.Unidad.ClasificadorAplicacion.Nombre;
                            else
                                this.vista.AplicacionNombre = null;
                        }
                        else
                        {
                            this.vista.AplicacionId = null;
                            this.vista.AplicacionNombre = null;
                        }
                        #endregion

                        #region SerieMotor
                        if(SerieMotorId == null)
                            throw new Exception("No se encuentra configurado el valor de la propiedad 'SERIE DE MOTOR' ");
                        String serieMotor = String.Empty;
                        if(ebBO.Unidad.CaracteristicasUnidad != null)
                        {
                            CaracteristicaUnidadBO caracteristica = ebBO.Unidad.CaracteristicasUnidad.FirstOrDefault(x => x.Caracteristica.Id == SerieMotorId);
                            serieMotor = caracteristica != null ? caracteristica.Valor : String.Empty;
                        }
                        this.vistaPagina3.SerieMotor = serieMotor;
                        #endregion
                    }
                    else
                    {
                        this.vista.LiderID = null;
                        this.vista.FechaProximoServicio = null;
                        this.vista.KilometrajeProximoServicio = null;
                        this.vista.KilometrajeInicial = null;
                        this.vista.TipoUnidadId = null;
                        this.vista.TipoUnidadNombre = null;
                        this.vista.ModeloId = null;
                        this.vista.ModeloNombre = null;
                        this.vista.MarcaId = null;
                        this.vista.MarcaNombre = null;
                        this.vista.MotorizacionId = null;
                        this.vista.MotorizacionNombre = null;
                        this.vista.FabricanteId = null;
                        this.vista.FabricanteNombre = null;
                        this.vista.AplicacionId = null;
                        this.vista.AplicacionNombre = null;
                        this.vista.NumeroEconomico = null;
                        this.vistaPagina3.SerieMotor = null;
                        this.vistaPagina1.UnidadBloqueada = null;
                        vistaPagina1.HabilitarUnidadBloqueada(true);
                    }
                    #endregion

                    #region Activo
                    if (ebBO.ActivoFijo != null)
                    {
                        if (ebBO.ActivoFijo.NumeroActivo != null && ebBO.ActivoFijo.NumeroActivo.Trim().CompareTo("") != 0)
                            this.vista.ClaveActivoOracle = ebBO.ActivoFijo.NumeroActivo;
                        else
                            this.vista.ClaveActivoOracle = null;

                        if (ebBO.ActivoFijo.FechaFacturaCompra != null)
                            this.vista.FechaCompra = ebBO.ActivoFijo.FechaFacturaCompra;
                        else
                            this.vista.FechaCompra = null;

                        if (ebBO.ActivoFijo.CostoSinIva != null)
                            this.vista.MontoFactura = ebBO.ActivoFijo.CostoSinIva;
                        else
                            this.vista.MontoFactura = null;

                        #region[REQ 13285 Registro de acta de nacimiento]
                        if (ebBO.ActivoFijo.FechaInicioDepreciacion != null)
                            this.vista.FechaInicioDepreciacion = ebBO.ActivoFijo.FechaInicioDepreciacion;
                        else
                            this.vista.FechaInicioDepreciacion = null;

                        if (ebBO.ActivoFijo.FechaIdealDesflote != null)
                            this.vista.FechaIdealDesflote = ebBO.ActivoFijo.FechaIdealDesflote;
                        else
                            this.vista.FechaIdealDesflote = null;

                        if (ebBO.ActivoFijo.PorcentajeDepreciacion != null)
                            this.vista.TasaDepreciacion = ebBO.ActivoFijo.PorcentajeDepreciacion;
                        else
                            this.vista.TasaDepreciacion = null;

                        if (ebBO.ActivoFijo.MesesVidaUtil != null)
                            this.vista.VidaUtil = ebBO.ActivoFijo.MesesVidaUtil / 12;
                        else
                            this.vista.VidaUtil = null;

                        if (ebBO.ActivoFijo.PorcentajeResidual != null)
                            this.vista.PorcentajeValorResidual = ebBO.ActivoFijo.PorcentajeResidual;
                        else
                            this.vista.PorcentajeValorResidual = null;

                        if (ebBO.ActivoFijo.ValorRecuperacion != null)
                            this.vista.ValorResidual = ebBO.ActivoFijo.ValorRecuperacion;
                        else
                            this.vista.ValorResidual = null;

                        if (ebBO.ActivoFijo.ValorLibro != null)
                            this.vista.SaldoPorDepreciar = ebBO.ActivoFijo.ValorLibro;
                        else
                            this.vista.SaldoPorDepreciar = null;
                        #endregion
                    }
                    else
                    {
                        this.vista.ClaveActivoOracle = null;
                        this.vista.FechaCompra = null;
                        this.vista.MontoFactura = null;
                    }
                    #endregion                    

                    #endregion
                    break;
                case "TipoUnidad":
                    #region Desplegar TipoUnidad
                    TipoUnidadBO tipoUnidad = (TipoUnidadBO)selecto;

                    if (tipoUnidad != null && tipoUnidad.Id != null)
                        this.vista.TipoUnidadId = tipoUnidad.Id;
                    else
                        this.vista.TipoUnidadId = null;

                    if (tipoUnidad != null && tipoUnidad.Nombre != null)
                        this.vista.TipoUnidadNombre = tipoUnidad.Nombre;
                    else
                        this.vista.TipoUnidadNombre = null;
                    #endregion
                    break;
                case "Marca":
                    #region Desplegar Marca
                    MarcaBO marca = (MarcaBO)selecto;

                    if (marca != null && marca.Id != null)
                    {
                        this.vista.MarcaId = marca.Id;
                        this.vista.HabilitarModelo(true);
                    }
                    else
                    {
                        this.vista.MarcaId = null;
                        this.vista.HabilitarModelo(false);
                    }

                    if (marca != null && marca.Nombre != null)
                        this.vista.MarcaNombre = marca.Nombre;
                    else
                        this.vista.MarcaNombre = null;

                    this.vista.ModeloId = null;
                    this.vista.ModeloNombre = null;
                    #endregion
                    break;
                case "Modelo":
                    #region Desplegar Modelo
                    ModeloBO modelo = (ModeloBO)selecto;

                    if (modelo != null && modelo.Id != null)
                        this.vista.ModeloId = modelo.Id;
                    else
                        this.vista.ModeloId = null;

                    if (modelo != null && modelo.Nombre != null)
                        this.vista.ModeloNombre = modelo.Nombre;
                    else
                        this.vista.ModeloNombre = null;

                    this.vista.MotorizacionId = null;
                    this.vista.MotorizacionNombre = null;
                    #endregion
                    break;
                case "Distribuidor":
                    #region Desplegar Distribuidor
                    DistribuidorBO distribuidor = (DistribuidorBO)selecto;

                    if (distribuidor != null && distribuidor.Id != null)
                        this.vista.FabricanteId = distribuidor.Id;
                    else
                        this.vista.FabricanteId = null;

                    if (distribuidor != null && distribuidor.Nombre != null)
                        this.vista.FabricanteNombre = distribuidor.Nombre;
                    else
                        this.vista.FabricanteNombre = null;
                    #endregion
                    break;
                case "Motorizacion":
                    #region Desplegar Motorizacion
                    ConfiguracionModeloMotorizacionBO configuracionModeloMotorizacion = new ConfiguracionModeloMotorizacionBO();
                    if (selecto == null)
                        configuracionModeloMotorizacion = new ConfiguracionModeloMotorizacionBO();
                    else
                        configuracionModeloMotorizacion = (ConfiguracionModeloMotorizacionBO)selecto;

                    if (configuracionModeloMotorizacion.ClasificadorMotorizacion != null)
                        this.vista.MotorizacionId = configuracionModeloMotorizacion.ClasificadorMotorizacion.Id;
                    else
                        this.vista.MotorizacionId = null;

                    if (configuracionModeloMotorizacion.ClasificadorMotorizacion != null)
                        this.vista.MotorizacionNombre = configuracionModeloMotorizacion.ClasificadorMotorizacion.Nombre;
                    else
                        this.vista.MotorizacionNombre = null;
                    #endregion
                    break;
                case "Aplicacion":
                    #region Desplegar Aplicacion
                    ClasificadorAplicacionBO aplicacion = (ClasificadorAplicacionBO)selecto;

                    if (aplicacion != null && aplicacion.Id != null)
                        this.vista.AplicacionId = aplicacion.Id;
                    else
                        this.vista.AplicacionId = null;

                    if (aplicacion != null && aplicacion.Nombre != null)
                        this.vista.AplicacionNombre = aplicacion.Nombre;
                    else
                        this.vista.AplicacionNombre = null;
                    #endregion
                    break;
            }
        }
        #endregion

        #region SC0002

        private string ComprobarPaquetesMantenimiento()
        {
            string mensaje = string.Empty;
            ModeloBO modeloUnidad = new ModeloBO() { Id = this.vista.ModeloId };

            bool existeUnidad = ValidarPaquetesMantenimiento(modeloUnidad);
            if (existeUnidad == false)
            {
                modeloUnidad = FacadeBR.ConsultarModelo(dctx,modeloUnidad).FirstOrDefault();
                mensaje = "La unidad con modelo "+modeloUnidad.Nombre+" no tiene configurado Paquetes de mantenimiento.\n";
            }
            if (this.vista.EquiposAliados != null && this.vista.EquiposAliados.Count > 0)
            {
                foreach (var item in this.vista.EquiposAliados)
                {
                    bool existeAliado = ValidarPaquetesMantenimiento(item.Modelo);
                    if (existeAliado == false)
                    {
                        mensaje += "El equipo Aliado con modelo "+item.Modelo.Nombre+" no tiene configurado paquetes de mantenimiento. \n";
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
                 if (configPosicion != null)
                     paquetesConfigurados.Add(configPosicion);
            }

            if (paquetesConfigurados != null && paquetesConfigurados.Count > 0)
            {
                if (paquetesConfigurados.Exists(x => x.DescriptorTrabajo.Nombre == "PMA" || x.DescriptorTrabajo.Nombre == "PMB" || x.DescriptorTrabajo.Nombre == "PMC") == true)
                    PaquetesAsignados = true;
            }
            
            return PaquetesAsignados;
 
        }

        ///// <summary>
        ///// Establecer acciones para la captura del acta de nacimiento
        ///// </summary>
        public void EstablecerAcciones()
        {
            this.presentadorPagina1.EstablecerAcciones(this.vista.ListaAcciones, "R");
            this.presentadorPagina7.EstablecerAcciones(this.vista.ListaAcciones);
            this.vista.EstablecerAcciones();
        }

        #endregion

        #endregion
    }
}
