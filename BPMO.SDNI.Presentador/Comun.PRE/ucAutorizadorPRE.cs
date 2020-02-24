//Satisface al caso de uso CU098 - Configurar Autorizadores para los Contratos
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using BPMO.Basicos.BO;
using BPMO.Facade.SDNI.BR;
using BPMO.Patterns.Creational.DataContext;
using BPMO.Primitivos.Enumeradores;
using BPMO.SDNI.Comun.BO;
using BPMO.SDNI.Comun.BR;
using BPMO.SDNI.Comun.VIS;

namespace BPMO.SDNI.Comun.PRE
{
    public class ucAutorizadorPRE
    {
        #region Atributos
        private IDataContext dctx = null;
        private IucAutorizadorVIS vista;
        private string nombreClase = "ucAutorizadorPRE";
        #endregion

        #region Constructor
        public ucAutorizadorPRE(IucAutorizadorVIS view)
        {
            try
            {
                this.vista = view;

                this.dctx = FacadeBR.ObtenerConexion();
            }
            catch (Exception ex)
            {
                this.vista.MostrarMensaje("Inconsistencia en los parámetros de configuración", ETipoMensajeIU.ERROR, this.nombreClase + ".ucAutorizadorPRE:" + ex.Message);
            }
        }
        #endregion

        #region Métodos
        public void PrepararNuevo()
        {
            this.vista.PrepararNuevo();

            this.vista.HabilitarSoloNotificacion(true);
            this.vista.HabilitarEstatus(true);

            this.vista.MostrarDatosActualizacion(false);
            this.vista.MostrarDatosRegistro(false);
            this.vista.MostrarEstatus(false);

            this.vista.PermitirSeleccionarSucursal(true);
            this.vista.PermitirSeleccionarEmpleado(true);

            this.EstablecerConfiguracionInicial(null);
        }
        public void PrepararEdicion()
        {
            this.vista.PrepararEdicion();

            this.vista.HabilitarSoloNotificacion(true);
            this.vista.HabilitarEstatus(true);

            this.vista.MostrarDatosActualizacion(false);
            this.vista.MostrarDatosRegistro(false);
            this.vista.MostrarEstatus(true);

            this.vista.PermitirSeleccionarSucursal(false);
            this.vista.PermitirSeleccionarEmpleado(false);

            this.EstablecerConfiguracionInicial(null);
        }
        public void PrepararVisualizacion()
        {
            this.vista.PrepararVisualizacion();

            this.vista.HabilitarSoloNotificacion(false);
            this.vista.HabilitarEstatus(false);
            
            this.vista.MostrarDatosActualizacion(true);
            this.vista.MostrarDatosRegistro(true);
            this.vista.MostrarEstatus(false);

            this.vista.PermitirSeleccionarSucursal(false);
            this.vista.PermitirSeleccionarEmpleado(false);
            this.AsignarModoRegistro("DET");
            this.EstablecerConfiguracionInicial(null);
        }

        public void AsignarModoRegistro(string Modo)
        {
            this.vista.ModoRegistro = Modo;
        }

        public void EstablecerConfiguracionInicial(int? unidadOperativaID)
        {
            try
            {
                this.vista.UnidadOperativaID = unidadOperativaID;

                #region Tipos de Autorización
                this.vista.EstablecerOpcionesTiposAutorizacion(this.ObtenerTiposAutorizacion());
                #endregion
            }
            catch (Exception ex)
            {
                throw new Exception(this.nombreClase + ".EstablecerInformacionInicial: " + ex.Message);
            }
        }

        /// <summary>
        /// Método para obtener un diccionario con los valores de los Tipos de Autorizacion que se envía como parámetro para el llenado del combo correspondiente
        /// </summary>
        /// <returns>Diccionario de tipo string,string</returns>
        private Dictionary<int, string> ObtenerTiposAutorizacion()
        {
            try
            {
                Dictionary<int, string> listaTiposAutorizacion = new Dictionary<int, string>();
                listaTiposAutorizacion.Add(-1, "SELECCIONA UNA OPCIÓN");
                Type type = typeof(ETipoAutorizacion);
                switch (this.vista.UnidadOperativaID)
                {
                    case (int)ETipoEmpresa.Generacion:
                        type = typeof(ETipoAutorizacionGeneracion);
                        break;
                    case (int)ETipoEmpresa.Construccion:
                        type = typeof(ETipoAutorizacionConstruccion);
                        break;
                    case (int)ETipoEmpresa.Equinova:
                        type = typeof(ETipoAutorizacionEquinova);
                        break;
                    default:
                        type = typeof(ETipoAutorizacion);
                        break;
                }
                Array values = Enum.GetValues(type);
                foreach (int value in values)
                {
                    var memInfo = type.GetMember(type.GetEnumName(value));
                    var display = memInfo[0]
                        .GetCustomAttributes(typeof(DescriptionAttribute), false)
                        .FirstOrDefault() as DescriptionAttribute;

                    if (display != null)
                    {
                        listaTiposAutorizacion.Add(value, display.Description);
                    }
                }

                return listaTiposAutorizacion;
            }
            catch (Exception ex)
            {

                throw new Exception(this.nombreClase + ".ListaTiposAutorizacion:Error al consultar los Tipos de Autorización");
            }
        }
        
        public string ValidarCampos()
        {
            string s = "";

            if (this.vista.UnidadOperativaID == null)
                s += "Unidad Operatia, ";
            if (this.vista.SucursalID == null)
                s += "Sucursal, ";
              if (this.vista.TipoAutorizacion == null)
                s += "Tipo de Autorización, ";
            if (this.vista.EmpleadoID == null)
                s += "Empleado, ";
            if (this.vista.Email == null)
                s += "Correo Electrónico, ";
            if (this.vista.Telefono == null)
                s += "Teléfono, ";
            if (this.vista.SoloNotificacion == null)
                s += "Notificación, ";
            if (this.vista.Estatus == null)
                s += "Estatus, ";

            if (s.Trim().CompareTo("") != 0)
                return "Los siguientes campos no pueden estar vacíos: \n" + s.Substring(0, s.Length - 2);

            //Se verifica la existencia del autorizador en la BD
            AutorizadorBO bo = new AutorizadorBO();
            #region Obtener el tipo de Autorización
            Type TipoEnum = typeof(ETipoAutorizacion);
            TipoEnum = bo.ObtenerETipoAutorizacion((ETipoEmpresa)this.vista.UnidadOperativaID);
            bo.TipoAutorizacion = (Enum)Enum.ToObject(TipoEnum, this.vista.TipoAutorizacion);
            #endregion
            bo.Sucursal = new SucursalBO { Id = this.vista.SucursalID, UnidadOperativa = new UnidadOperativaBO() { Id = this.vista.UnidadOperativaID } };
            bo.Empleado = new EmpleadoBO { Id = this.vista.EmpleadoID };
            List<AutorizadorBO> lst = new AutorizadorBR().Consultar(this.dctx, bo);

            if (lst.Count > 0)
                return "Ya se encuentra registrada la configuración de tipo de autorización, sucursal y empleado.";

            return null;
        }

        public void LimpiarSesion()
        {
            this.vista.LimpiarSesion();
        }

        public void ConfigurarSoloNotificacion()
        {
            switch (this.vista.UnidadOperativaID)
            {
                case (int)ETipoEmpresa.Generacion:
                case (int)ETipoEmpresa.Construccion:
                case (int)ETipoEmpresa.Equinova:
                    this.vista.HabilitarSoloNotificacion(false);
                    break;
                default:
                    this.vista.HabilitarSoloNotificacion(true);
                    break;
            }
        }

        #region Métodos para el Buscador
        /// <summary>
        /// Prepara un BO para la Busqueda en su respectivo catalogo
        /// </summary>
        /// <param name="catalogo">catalogo donde se realizara la busqueda</param>
        /// <returns></returns>
        public object PrepararBOBuscador(string catalogo)
        {
            object obj = null;

            switch (catalogo)
            {
                case "Sucursal":
                    SucursalBO sucursal = new SucursalBO();
                    sucursal.UnidadOperativa = new UnidadOperativaBO();

                    sucursal.Nombre = this.vista.SucursalNombre;
                    sucursal.UnidadOperativa.Id = this.vista.UnidadOperativaID;
                    sucursal.Activo = true;

                    obj = sucursal;
                    break;
                case "Empleado":
                    EmpleadoBO empleado = new EmpleadoBO();

                    empleado.NombreCompleto = this.vista.EmpleadoNombre;
                    empleado.Activo = true;

                    obj = empleado;
                    break;
            }

            return obj;
        }
        /// <summary>
        /// Despliega el Resultado del Buscador
        /// </summary>
        /// <param name="catalogo">Catalogo en el que se realizo la busqueda</param>
        /// <param name="selecto">Objeto Resultante</param>
        public void DesplegarResultadoBuscador(string catalogo, object selecto)
        {
            switch (catalogo)
            {
                case "Sucursal":
                    SucursalBO sucursal = (SucursalBO)selecto;
                    if (sucursal != null && sucursal.Id != null)
                        this.vista.SucursalID = sucursal.Id;
                    else
                        this.vista.SucursalID = null;

                    if (sucursal != null && sucursal.Nombre != null)
                        this.vista.SucursalNombre = sucursal.Nombre;
                    else
                        this.vista.SucursalNombre = null;
                    break;
                case "Empleado":
                    EmpleadoBO empleado = (EmpleadoBO)selecto;

                    if (!(empleado != null && empleado.Activo != null && empleado.Activo == true))
                    {
                        this.vista.MostrarMensaje("El empleado seleccionado no se encuentra activo", ETipoMensajeIU.ADVERTENCIA, null);
                        empleado = new EmpleadoBO();
                    }

                    if (empleado != null && empleado.Id != null)
                        this.vista.EmpleadoID = empleado.Id;
                    else
                        this.vista.EmpleadoID = null;

                    if (empleado != null && empleado.NombreCompleto != null)
                        this.vista.EmpleadoNombre = empleado.NombreCompleto;
                    else
                        this.vista.EmpleadoNombre = null;
                    if (empleado != null && empleado.Email != null)
                        this.vista.Email = empleado.Email;
                    else
                        this.vista.Email = null;
                    if (empleado.Id != null)
                    {
                        List<EmpleadoBO> lst = FacadeBR.ConsultarEmpleadoCompleto(this.dctx, empleado);
                        if (lst.Count > 0)
                            empleado = lst[0];
                    }
                    if (empleado != null && empleado.Telefonos != null && empleado.Telefonos.Count>0)
                        this.vista.Telefono = empleado.Telefonos[0].Numero;
                    else
                        this.vista.Telefono = null;
                    break;
            }
        }
        #endregion
        #endregion
    }
}
