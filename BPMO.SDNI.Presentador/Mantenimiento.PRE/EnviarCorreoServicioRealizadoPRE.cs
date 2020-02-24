// Satisface al CU064 - Enviar Correo Servicio Realizado
// Satisface al CU059 - Reporte Mantenimiento Realizado Unidad
// Satisface al CU074 - Reporte Mantenimiento Correctivo Realizado Unidad
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BPMO.SDNI.Mantenimiento.VIS;
using BPMO.Basicos.BO;
using BPMO.SDNI.Comun.BR;
using BPMO.Patterns.Creational.DataContext;
using BPMO.Facade.SDNI.BR;
using BPMO.SDNI.Comun.BO;
using BPMO.SDNI.Mantenimiento.BR;
using BPMO.SDNI.Mantenimiento.BO;
using BPMO.Primitivos.Enumeradores;
using System.Configuration;
using BPMO.SDNI.Mantenimiento.BO.BOF;
using BPMO.Servicio.Procesos.BO;
using System.Globalization;
using BPMO.SDNI.Mantenimiento.Reportes.BR;
using BPMO.SDNI.Mantenimiento.RPT;
using System.IO;

namespace BPMO.SDNI.Mantenimiento.PRE {

    /// <summary>
    /// Presentador aplicable para la gestión de las peticiones de la Vista Enviar Correo Servicio Realizado.
    /// </summary>
    public class EnviarCorreoServicioRealizadoPRE {

        #region Atributos

        /// <summary>
        /// La vista que esta mostrando los datos procesados en el presentador.
        /// </summary>
        private IEnviarCorreoServicioRealizadoVIS vista;

        /// <summary>
        /// El DataContext que proveerá acceso a la base de datos.
        /// </summary>
        private IDataContext dataContext;

        /// <summary>
        /// Mantenimiento Unidad Idealease seleccionado.
        /// </summary>
        private MantenimientoUnidadBO mantenimientoSeleccionado;

        /// <summary>
        /// Controlador Mantenimiento Unidad.
        /// </summary>
        private MantenimientoUnidadBR ctrlMantenimiento;

        /// <summary>
        /// Controlador Enviar Correo Servicio Realizado.
        /// </summary>
        private EnviarCorreoServicioRealizadoBR ctrlEnviarCorreoServicioRealizado;

        /// <summary>
        /// Controlador Reporte Mantenimiento Realizado.
        /// </summary>
        private ReporteMantenimientoRealizadoUnidadBR reporteMantenimientoRealizadoUnidad;

        /// <summary>
        /// Nombre de la clase en curso.
        /// </summary>
        private readonly string nombreClase = typeof(EnviarCorreoServicioRealizadoPRE).Name;

        /// <summary>
        /// Obtiene o establece el mensaje de Error al enviar el Correo Servicio Realizado.
        /// </summary>
        private string mensajeError;

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor predeterminado para el presentador.
        /// </summary>
        /// <param name="vista">Vista que será gestionada por el presentador.</param>
        public EnviarCorreoServicioRealizadoPRE(IEnviarCorreoServicioRealizadoVIS vista) {
            this.vista = vista;
            dataContext = FacadeBR.ObtenerConexion();
            ctrlEnviarCorreoServicioRealizado = new EnviarCorreoServicioRealizadoBR();
            this.reporteMantenimientoRealizadoUnidad = new ReporteMantenimientoRealizadoUnidadBR();
            mensajeError = "";
        }

        #endregion

        #region Métodos

            #region Seguridad

        /// <summary>
        /// Verifica que el Usuario logueado tenga los permisos para realizar la acción de Enviar el Correo del Servicio Realizado.
        /// </summary>
        public void PrepararSeguridad() {
            EstablecerInformacionInicial();
            EstablecerSeguridad();
        }

        /// <summary>
        /// Establece la información Inicial en la Vista.
        /// </summary>
        private void EstablecerInformacionInicial() { 
            try {
                #region Configuraciones de Unidad Operativa
                //Obtener las configuraciones de la unidad operativa
                List<ConfiguracionUnidadOperativaBO> lstConfigUO = new ModuloBR().ConsultarConfiguracionUnidadOperativa(
                                                                                        this.dataContext, getConfiguracionUnidadOperativa(),  this.vista.ModuloID
                                                                                    );
                if (lstConfigUO.Count <= 0)
                    throw new Exception("No se encontraron las configuraciones del sistema para la unidad operativa en la que trabaja.");

                //Establecer las configuraciones de la unidad operativa
                this.vista.LibroActivos = lstConfigUO[0].Libro;
                #endregion
            } catch (Exception ex) {
                throw new Exception(nombreClase + " .EstablecerInformacionInicial: " + ex.Message);
            }
        }

        /// <summary>
        /// Crea y obtiene un nuevo Filtro de la Configuración Unidad Operativa, para realizar la búsqueda de las 
        /// Configuraciones de Unidades Operativas.
        /// </summary>
        /// <returns>Objeto de Tipo ConfiguracionUnidadOperativaBO</returns>
        private ConfiguracionUnidadOperativaBO getConfiguracionUnidadOperativa() { 
            return new ConfiguracionUnidadOperativaBO() { UnidadOperativa = new UnidadOperativaBO() { Id = this.vista.UnidadOperativaID } };
        }

        /// <summary>
        /// Permite configurar el comportamiento de los controles de la vista de acuerdo a los permisos configurados del Usuario.
        /// </summary>
        private void EstablecerSeguridad() { 
            try {
                //se valida que los datos del usuario y la unidad operativa no sean nulos
                if (this.vista.UsuarioAutenticado == null) throw new Exception("El identificador del usuario no debe ser nulo");
                if (this.vista.UnidadOperativaID == null) throw new Exception("El identificador de la Unidad Operativa no debe ser nulo ");

                //consulta lista de acciones permitidas
                List<CatalogoBaseBO> acciones = FacadeBR.ConsultarAccion(dataContext, getSeguridad());

                //Se valida si el usuario tiene permiso para Enviar el Correo de Servicio Realizado
                if (!ExisteAccion(acciones, "UI CONSULTAR") || !ExisteAccion(acciones, "CONSULTAR") || !ExisteAccion(acciones, "CONSULTARCOMPLETO"))
                    this.vista.RedirigirSinPermisoAcceso();
                       
            }
            catch (Exception ex) {
                throw new Exception(nombreClase + " .EstablecerSeguridad: " + ex.Message);
            }
        }

        /// <summary>
        /// Crea y obtiene una nueva instancia de Seguridad.
        /// </summary>
        /// <returns>Objeto de tipo SeguridadBO.</returns>
        private SeguridadBO getSeguridad() {
            UsuarioBO usuario = new UsuarioBO() { Id = this.vista.UsuarioAutenticado };
            AdscripcionBO adscripcion = new AdscripcionBO() { UnidadOperativa = new UnidadOperativaBO() { Id = this.vista.UnidadOperativaID } };
            return new SeguridadBO(Guid.Empty, usuario, adscripcion);
        }

        /// <summary>
        /// Verifica si existe la acción en la lista de acciones permitidas.
        /// </summary>
        /// <param name="acciones">Lista de Acciones permitidas.</param>
        /// <param name="nombreAccion">Acción a verificar.</param>
        /// <returns>Retorna True si encontró resultados, en caso contrario retorna False.</returns>
        private bool ExisteAccion(List<CatalogoBaseBO> acciones, string nombreAccion){
            if (acciones != null && 
                acciones.Count > 0 &&
                acciones.Exists(
                    p => p != null && p.Nombre != null && p.Nombre.Trim().ToUpper() == nombreAccion.Trim().ToUpper()))
                return true;

            return false;
        }

            #endregion

        /// <summary>
        /// Realiza la carga de la información a desplegar antes de enviar el Correo. Consulta los Contactos Clientes 
        /// de la Unidad Idealease Seleccionada con la Sucursal de la Unidad.
        /// </summary>
        public void CargarDatosMensaje() {
            try {
                ctrlMantenimiento = new MantenimientoUnidadBR();
                CargarMensaje();
                CargarDatosUnidad();
                CargarDatosContactoCliente();
            } catch(Exception e) {
                vista.MostrarMensaje("Error al obtener la información para el envío de Correo de Servicio Realizado.", ETipoMensajeIU.ERROR, e.Message);
            }
        }

        /// <summary>
        /// Realiza la carga de la información del Asunto, Mensaje de Inicio y el Cuerpo del Mensaje a enviar.
        /// </summary>
        private void CargarMensaje() {
            vista.Datos.Add("asunto", ConfigurationManager.AppSettings["AsuntoCorreoServicioRealizado"]);
            vista.Datos.Add("mensajeInicio", ConfigurationManager.AppSettings["CuerpoCorreoServicioRealizado"]);
            vista.Datos.Add("mensaje", ConfigurationManager.AppSettings["MensajeCorreo"]);
        }

        /// <summary>
        /// Obtiene el Mantenimiento Idealease, si el Mantenimiento Idealease es de Tipo Unidad se obtiene la información completa,
        /// si es de Tipo Equipo Aliado se obtiene el Mantenimiento Unidad.
        /// </summary>
        private void CargarDatosUnidad() {
            if (vista.Mantenimiento != null) {
                if(vista.Mantenimiento.MantenimientoUnidad != null && vista.Mantenimiento.MantenimientoUnidad.MantenimientoUnidadId != null) {
                    mantenimientoSeleccionado = getMantenimientoCompleto(vista.Mantenimiento.MantenimientoUnidad);
                } else if(vista.Mantenimiento != null) {
                    MantenimientoEquipoAliadoBO seleccionado = vista.Mantenimiento.MantenimientoAliado;
                    if(seleccionado != null && seleccionado.MantenimientoEquipoAliadoId != null) {
                        MantenimientoEquipoAliadoBR ctrlMantenimientoEquipoAliado = new MantenimientoEquipoAliadoBR();
                        mantenimientoSeleccionado = ctrlMantenimientoEquipoAliado.ConsultarMantenimientoUnidadPorMantenimientoEquipoAliado(dataContext, seleccionado.MantenimientoEquipoAliadoId);
                        mantenimientoSeleccionado = getMantenimientoCompleto(mantenimientoSeleccionado);
                        if(mantenimientoSeleccionado.MantenimientoUnidadId == null){
                            throw new Exception("No se encontró un Mantenimiento Unidad para el Mantenimiento Equipo Aliado");
                        }
                    } else {
                        throw new Exception("No se encontró un Mantenimiento Unidad o Mantenimiento Equipo Aliado");
                    }
                } 
                if (mantenimientoSeleccionado != null) {
                    ConsultarMantenimientoProgramadoBR ctrlMantenimientoProgramado = new ConsultarMantenimientoProgramadoBR();
                    int EquipoID = mantenimientoSeleccionado.IngresoUnidad.Unidad.EquipoID.Value;
                    MantenimientoProgramadoBO mantenimientoProgramado = ctrlMantenimientoProgramado.ConsultarUltimoMantenimientoProgramado(dataContext, EquipoID, true, true);
                    vista.Taller = mantenimientoSeleccionado.Taller;
                    if (mantenimientoProgramado != null && mantenimientoProgramado.MantenimientoProgramadoID != null) {
                        vista.Datos.Add("unidad", mantenimientoSeleccionado.IngresoUnidad.Unidad.NumeroSerie);
                        vista.Datos.Add("tipoProximoServicio", mantenimientoProgramado.TipoMantenimiento.ToString());
                        vista.Datos.Add("fechaProximoServicio", mantenimientoProgramado.Fecha.Value.ToString("dd-MM-yyyy", CultureInfo.InvariantCulture));
                        vista.Datos.Add("kilometraje", mantenimientoSeleccionado.KilometrajeEntrada.ToString());
                        vista.Datos.Add("horas", mantenimientoSeleccionado.HorasEntrada.ToString());
                        vista.CargarDatosUnidad();
                    } else {
                        vista.Datos.Add("unidad", mantenimientoSeleccionado.IngresoUnidad.Unidad.NumeroSerie);
                        vista.Datos.Add("tipoProximoServicio","SIN SERVICIO PROGRAMADO");
                        vista.Datos.Add("fechaProximoServicio", "SIN FECHA DE PROXIMO SERVICIO");
                        vista.Datos.Add("kilometraje", mantenimientoSeleccionado.KilometrajeEntrada.ToString());
                        vista.Datos.Add("horas", mantenimientoSeleccionado.HorasEntrada.ToString());
                        vista.CargarDatosUnidad();
                        //vista.MostrarMensaje("No se encontró un mantenimiento programado para la unidad", ETipoMensajeIU.ERROR);
                    }
                }
            } else {
                throw new Exception("No se encontró un Mantenimiento Unidad o Mantenimiento Equipo Aliado");
            }
        }

        /// <summary>
        /// Crea y obtiene el Mantenimiento Unidad Idealease completo.
        /// </summary>
        /// <param name="mantenimiento">El filtro para obtener el Mantenimiento Unidad Idealease.</param>
        /// <returns>Objeto de tipo MantenimientoUnidadBO</returns>
        private MantenimientoUnidadBO getMantenimientoCompleto(MantenimientoUnidadBO mantenimiento) {
            MantenimientoUnidadBO mantenimientoCompleto = ctrlMantenimiento.Consultar(dataContext, new MantenimientoUnidadBO() { MantenimientoUnidadId = mantenimiento.MantenimientoUnidadId }).FirstOrDefault();
            mantenimientoCompleto.IngresoUnidad.Unidad = new BPMO.SDNI.Equipos.BR.UnidadBR().ConsultarCompleto(dataContext, mantenimientoCompleto.IngresoUnidad.Unidad).FirstOrDefault();
            if (mantenimientoCompleto.FechaSalida == null) {
                mantenimiento.FechaSalida = DateTime.Now;
            }
            else {
                mantenimiento.FechaSalida = mantenimientoCompleto.FechaSalida;
            }
            mantenimientoCompleto.MantenimientoEquiposAliados = ctrlMantenimiento.ConsultarMantenimientosEquiposAliados(dataContext, new MantenimientoUnidadBO() { MantenimientoUnidadId = mantenimiento.MantenimientoUnidadId }).MantenimientoEquiposAliados;
            foreach (MantenimientoEquipoAliadoBO equipoAliado in mantenimientoCompleto.MantenimientoEquiposAliados) {
                if (equipoAliado.TipoServicio.Nombre == null) {
                    equipoAliado.TipoServicio.Nombre = "N/A";
                }
            }
            return mantenimientoCompleto;
        }

        /// <summary>
        /// Obtiene la lista de Contactos Cliente Idealease de acuerdo al Cliente Idealease de la Unidad, la Sucursal de la 
        /// Unidad Idealease y si recibe correo. En caso de no encontrar Contactos Cliente Idealease se toma el Correo configurado
        /// del Cliente Idealease, en caso de no tener un correo configurado despliega un mensaje de error.
        /// </summary>
        private void CargarDatosContactoCliente() {
            ClienteBO cliente = mantenimientoSeleccionado.IngresoUnidad.Unidad.Cliente;
            CuentaClienteIdealeaseBR ctrlCuentaIdealease = new CuentaClienteIdealeaseBR();
            CuentaClienteIdealeaseBO cuentaIdealease = ctrlCuentaIdealease.Consultar(dataContext, getFiltroCuentaClienteIdealease(cliente)).FirstOrDefault();
            string nombreCliente = cliente.NombreCompleto != null ? (" " + cliente.NombreCompleto) : "";
            if(cuentaIdealease != null && cuentaIdealease.Id != null) {
                ContactoClienteBR ctrlContactoCliente = new ContactoClienteBR();
                List<ContactoClienteBO> contactos = ctrlContactoCliente.Consultar(dataContext, getFiltroContactoCliente(cuentaIdealease, mantenimientoSeleccionado.IngresoUnidad.Unidad.Sucursal));
                if (contactos.Count > 0) {
                    List<DetalleContactoClienteBO> detalles = new List<DetalleContactoClienteBO>();
                    foreach (ContactoClienteBO contacto in contactos) {
                        ContactoClienteBO c = ctrlContactoCliente.ConsultarCompleto(dataContext, contacto).FirstOrDefault();
                        foreach (DetalleContactoClienteBO detalle in c.Detalles) {
                            if (detalle.RecibeCorreoElectronico.Value) {
                                detalles.Add(detalle);
                            }
                        }
                    }
                    if (detalles.Count == 0) {
                        vista.MostrarMensaje("El Cliente Idealease " + nombreCliente + " no tiene algún contacto cliente para recibir correos electronicos.", ETipoMensajeIU.ADVERTENCIA);
                    } else {
                        vista.ContactoClienteSeleccionado = detalles.First();
                    }
                    vista.ContactosCliente = detalles;
                } else {
                    if (cuentaIdealease.Correo == null || cuentaIdealease.Correo.Trim() == null || cuentaIdealease.Correo.Equals("")) {
                        vista.MostrarMensaje("El Cliente Idealease" + nombreCliente + " no tiene un Correo configurado.", ETipoMensajeIU.ADVERTENCIA);
                    }
                    DetalleContactoClienteBO contacto = new DetalleContactoClienteBO() {
                        Nombre = cuentaIdealease.Nombre != null ? cuentaIdealease.Nombre : "",
                        Correo = cuentaIdealease.Correo,
                    };
                    vista.ContactoClienteSeleccionado = contacto;
                }
                vista.CargarDatosContactoCliente();
            } else {
                vista.ContactoClienteSeleccionado = null;
                vista.MostrarMensaje("El Cliente" + nombreCliente + " no tiene una Cuenta Cliente Idealease.", ETipoMensajeIU.ADVERTENCIA);
            }
        }

        /// <summary>
        /// Crea y obtiene un nuevo Filtro de Cuenta Cliente Idealease, para realizar la búsqueda de la Cuenta Cliente Idealease.
        /// </summary>
        /// <param name="cliente">El filtro para obtener la Cuenta Cliente Idealease.</param>
        /// <returns>Objeto de tipo CuentaClienteIdealeaseBO.</returns>
        private CuentaClienteIdealeaseBO getFiltroCuentaClienteIdealease(ClienteBO cliente) { 
            CuentaClienteIdealeaseBO filtroCuentaIdealease = new CuentaClienteIdealeaseBO() {
                UnidadOperativa = new UnidadOperativaBO() {
                    Id = vista.UnidadOperativaID
                },
                Cliente = cliente
            };
            return filtroCuentaIdealease;
        }

        /// <summary>
        /// Crea y obtiene un nuevo Filtro de Contacto Cliente Idealease, para realizar la búsqueda de la Contacto Cliente Idealease.
        /// </summary>
        /// <param name="cuentaIdealease">El filtro Cuenta Cliente Idealease para obtener el Contacto Cliente Idealease.</param>
        /// <param name="sucursal">El filtro Sucursal para obtener el Contacto Cliente Idealease.</param>
        /// <returns>Objeto de tipo ContactoClienteBO.</returns>
        private ContactoClienteBO getFiltroContactoCliente(CuentaClienteIdealeaseBO cuentaIdealease, SucursalBO sucursal) { 
            ContactoClienteBO filtroContactoCliente = new ContactoClienteBO() {
                CuentaClienteIdealease = cuentaIdealease, 
                Sucursal = sucursal,
                Activo = true
            };
            return filtroContactoCliente;
        }

        /// <summary>
        /// Envia el Correo de Servicio Realizado. Valida que exista un Correo Electrónico para enviar el Correo, 
        /// en caso de no encontrar un Correo Electrónico el Correo no se envia. Se envia una copia al Jefe del Taller, 
        /// si el Jefe de Taller no tiene configurado un Correo, se despliega un mensaje de advertencia indicando que 
        /// no llego el Correo al Jefe de Taller, esta validación no impide que llegue el Correo a los Contactos Clientes.
        /// </summary>
        public void EnviarCorreo() {
            bool enviarAJefeDeTaller = true;
            try{
                byte[] reporte = getReporte();
                if (vista.ContactosCliente != null && vista.ContactosCliente.Count > 0) {
                    foreach (DetalleContactoClienteBO contacto in vista.ContactosCliente) {
                        EnviarCorreo(contacto.Correo, enviarAJefeDeTaller, reporte);
                        enviarAJefeDeTaller = false;
                    }
                    if (mensajeError.Equals("")) {
                        vista.MostrarMensaje("El correo ha sido enviado", ETipoMensajeIU.EXITO);
                    } else {
                        mensajeError = "El correo ha sido enviado. " + mensajeError;
                        vista.MostrarMensaje("Advertencia.", ETipoMensajeIU.ERROR, mensajeError);
                    }
                } else if(vista.ContactoClienteSeleccionado != null) {
                    if(!ValidarString(vista.ContactoClienteSeleccionado.Correo)) {
                        EnviarCorreo(vista.ContactoClienteSeleccionado.Correo, enviarAJefeDeTaller, reporte);
                        if (mensajeError.Equals("")) {
                            vista.MostrarMensaje("El correo ha sido enviado", ETipoMensajeIU.EXITO);
                        } else {
                            mensajeError = "El correo ha sido enviado. " + mensajeError;
                            vista.MostrarMensaje("Advertencia.", ETipoMensajeIU.ERROR, mensajeError);
                        }
                    } else {
                        vista.MostrarMensaje("Error al intentar enviar el correo de Servicio Realizado.", ETipoMensajeIU.ERROR, "El correo de Servicio Realizado no pudo ser enviado, el Cliente Idealease no tiene un Correo configurado.");
                    }
                } else 
                {
                    var correoJefe = this.getCorreoJefeTaller();
                    if (correoJefe != null && correoJefe != string.Empty)
                    {
                        EnviarCorreo(correoJefe, false, reporte);
                        if (mensajeError.Equals(""))
                        {
                            vista.MostrarMensaje("El correo ha sido enviado", ETipoMensajeIU.ERROR, "El correo de Servicio Realizado no pudo ser enviado al cliente, el Cliente no tiene una Cuenta Cliente Idealease. El correo ha sido enviado al jefe de Taller.");  
                        }
                        else
                        {
                            mensajeError = "El correo no ha sido enviado. " + mensajeError;
                            vista.MostrarMensaje("Advertencia. El correo de Servicio Realizado no pudo ser enviado al cliente, el Cliente no tiene una Cuenta Cliente Idealease", ETipoMensajeIU.ERROR, mensajeError);
                        }
                    }
                    else
                        vista.MostrarMensaje("Error al intentar enviar el correo de Servicio Realizado.",ETipoMensajeIU.ERROR,"El correo de Servicio Realizado no pudo ser enviado al cliente, el Cliente no tiene una Cuenta Cliente Idealease. El correo del jefe de taller no esta configurado");  
                    
                }
            } catch(Exception e) {
                vista.MostrarMensaje("Error al intentar enviar el Correo de Servicio Realizado.", ETipoMensajeIU.ERROR, e.Message);
            }
        }

        /// <summary>
        /// Verifica que el campo no sea nulo o contenga el carácter vacío.
        /// </summary>
        /// <param name="parametro">El campo a validar.</param>
        /// <returns>Retorna True si es nulo o tiene el carácter vacío.</returns>
        private bool ValidarString(string param) {
            return param == null || param.Trim() == null || param.Equals("");
        }

        /// <summary>
        /// Obtiene el Reporte de acuerdo al Tipo de Servicio Realizado.
        /// </summary>
        /// <returns>Retorna el arreglo de btyes del archivo generado.</returns>
        private byte[] getReporte() {
            try {
                Dictionary<string, object> datosReporte = new Dictionary<string, object>();
                datosReporte.Add("MantenimientoUnidad", this.vista.Mantenimiento.MantenimientoUnidad);
                datosReporte.Add("MantenimientoEquiposAliados", this.vista.Mantenimiento.MantenimientoUnidad.MantenimientoEquiposAliados);
                datosReporte.Add("UnidadOperativaID", vista.UnidadOperativaID.ToString());
                datosReporte.Add("RootPath", this.vista.RootPath);

                int tipoServicio = this.vista.Mantenimiento.MantenimientoUnidad.TipoServicio.Id.Value;

                byte[] reporte = this.GenerarReporteMantenimientoRealizadoUnidad(datosReporte, tipoServicio);
                if (reporte == null)
                    throw new Exception("No hay mantenimientos finalizados para esta unidad");

                return reporte;
            } catch(Exception e) {
                throw new Exception("Ocurrió un error al momento de obtener el reporte." + e.Message);
            }
        }

        /// <summary>
        /// Envia el Correo de Servicio Realizado a cada Contacto Cliente Idealease de la Lista. Se envia una 
        /// copia al Jefe del Taller.
        /// </summary>
        /// <param name="correoElectronico">Correo del Contacto Cliente Idealease.</param>
        /// <param name="enviarAJefeDeTaller">Indica que se envia una copia al Jefe del Taller.</param>
        /// <param name="reporte">El reporte generado.</param>
        private void EnviarCorreo(string correoElectronico, bool enviarAJefeDeTaller, byte[] reporte) { 
            try{
                Dictionary<string, object> datos = new Dictionary<string, object>();
                datos.Add("asuntoMensaje", vista.Datos["asunto"]);
                datos.Add("tituloMensaje", vista.TituloMensaje);
                datos.Add("cuerpoMensaje", vista.CuerpoMensaje);
                datos.Add("unidad", vista.Datos["unidad"]);
                datos.Add("tipoProximoServicio",vista.Datos["tipoProximoServicio"]);
                datos.Add("fechaProximoServicio", vista.Datos["fechaProximoServicio"]);
                datos.Add("kilometraje", vista.Datos["kilometraje"]);
                datos.Add("horas", vista.Datos["horas"]);
                datos.Add("correo", correoElectronico);
                if(enviarAJefeDeTaller){
                    string correoJefeTaller = getCorreoJefeTaller();
                    if(correoJefeTaller != null && correoJefeTaller.Trim() != null &&!correoJefeTaller.Equals(string.Empty)){
                        datos.Add("copiaCorreo", correoJefeTaller);
                    } else {
                        mensajeError += "El correo no pudo ser enviado al Jefe de taller ya que no tiene un E-mail configurado.";
                    }
                }
                datos.Add("unidadOperativa", vista.UnidadOperativaID.ToString());

                #region CU059
                datos.Add("Reporte", reporte);
                #endregion
            
                ctrlEnviarCorreoServicioRealizado.EnviarCorreo(datos);
            } catch(Exception e) {
                vista.MostrarMensaje("Ocurrió un error al momento de envíar el correo de servicio realizado.", ETipoMensajeIU.ERROR, e.Message);
            } 
        }

        /// <summary>
        /// Obtiene el Correo del Jefe de Taller.
        /// </summary>
        /// <returns>El Correo del Jefe de Taller.</returns>
        private string getCorreoJefeTaller() {
            BPMO.Servicio.Catalogos.BO.JefeTallerBO jefeTaller = FacadeBR.ConsultarJefeTaller(dataContext, new BPMO.Servicio.Catalogos.BO.JefeTallerBO()
            {
                AdscripcionServicio = new Servicio.Catalogos.BO.AdscripcionServicioBO()
                {
                    Taller = vista.Taller,
                    UnidadOperativa = new UnidadOperativaBO { Id = this.vista.UnidadOperativaID },
                    Sucursal = this.vista.Mantenimiento.MantenimientoUnidad.Sucursal

                },
                Activo = true
            });

            EmpleadoBO empleado = FacadeBR.ConsultarEmpleado(dataContext, new EmpleadoBO { Numero = jefeTaller.Empleado.Numero}).FirstOrDefault();
            return empleado.Email;
        }

        /// <summary>
        /// Genera el array de bytes del reporte de mantenimiento realizado unidad de acuerdo al Tipo de Servicio Realizado.
        /// </summary>
        /// <param name="datosReporte">Diccionario de datos para realizar la búsqueda.</param>
        /// <returns></returns>
        private byte[] GenerarReporteMantenimientoRealizadoUnidad(Dictionary<string, object> datosReporte, int tipoServicio)
        {
            Dictionary<string, object> dataSources = reporteMantenimientoRealizadoUnidad.Consultar(dataContext, datosReporte);

            if (dataSources["UpTime"] != null && dataSources["UpTime"].ToString().Equals("NA")) {
                this.vista.MostrarMensaje("No se encontraron configuraciones de días y horas uso, para el cliente", ETipoMensajeIU.ADVERTENCIA);
            }
            switch (tipoServicio)
            {
                case 1:
                    ReporteMantenimientoCorrectivoRealizadoUnidadRPT reporteCorrectivo = new ReporteMantenimientoCorrectivoRealizadoUnidadRPT(dataSources);
                    if (this.vista.ImprimirPendientes && this.vista.ListaTareasPendientes != null)
                        reporteCorrectivo.TareasPendientes = this.ObtenerListaPendientes();
                    using (MemoryStream stream = new MemoryStream())
                    {
                        reporteCorrectivo.CreateDocument();
                        reporteCorrectivo.ExportToPdf(stream);
                        return stream.GetBuffer();
                    }

                case 2:
                    ReporteMantenimientoRealizadoUnidadRPT reporte = new ReporteMantenimientoRealizadoUnidadRPT(dataSources);
                    if (this.vista.ImprimirPendientes && this.vista.ListaTareasPendientes != null)
                        reporte.TareasPendientes = this.ObtenerListaPendientes();
                    using (MemoryStream stream = new MemoryStream())
                    {
                        reporte.CreateDocument();
                        reporte.ExportToPdf(stream);
                        return stream.GetBuffer();
                    }
                default:
                    return null;
            }
            
        }
        /// <summary>
        /// Obtiene la lista de Tareas Pendientes
        /// </summary>
        /// <returns>Cadena de texto con la lista</returns>
        private string ObtenerListaPendientes() {
            StringBuilder sTareasPendientes = new StringBuilder();
            this.vista.ListaTareasPendientes.ForEach(t => sTareasPendientes.AppendLine(t.Descripcion));
            return sTareasPendientes.ToString();
        }
        #endregion
    }
}
