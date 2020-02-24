//Satisface al caso de uso CU098 - Configurar Autorizadores para los Contratos
using System;
using System.Collections.Generic;
using System.Linq;

using BPMO.Facade.SDNI.BR;

using BPMO.Basicos.BO;
using BPMO.Patterns.Creational.DataContext;
using BPMO.Primitivos.Enumeradores;

using BPMO.SDNI.Comun.BO;
using BPMO.SDNI.Comun.BR;
using BPMO.SDNI.Comun.VIS;
using System.ComponentModel;

namespace BPMO.SDNI.Comun.PRE
{
    public class ConsultarAutorizadorPRE
    {
        #region Atributos
        private IDataContext dctx = null;
        private IConsultarAutorizadorVIS vista;
        private AutorizadorBR controlador;
        private string nombreClase = "ConsultarAutorizadorPRE";
        #endregion

        #region Constructor
        public ConsultarAutorizadorPRE(IConsultarAutorizadorVIS vista)
        {
            try
            {
                this.vista = vista;
                dctx = FacadeBR.ObtenerConexion();

                this.controlador = new AutorizadorBR();
            }
            catch (Exception ex)
            {
                this.vista.MostrarMensaje("Inconsistencia en los parámetros de configuración", ETipoMensajeIU.ERROR, this.nombreClase + ".ConsultarAutorizadorPRE: " + ex.Message);         
            }
        }
        #endregion

        #region Métodos
        public void PrepararBusqueda()
        {
            this.vista.LimpiarSesion();

            this.vista.EmpleadoID = null;
            this.vista.EmpleadoNombre = null;
            this.vista.Estatus = null;
            this.vista.SucursalID = null;
            this.vista.SucursalNombre = null;
            this.vista.TipoAutorizacion = null;

            this.EstablecerConfiguracionInicial();

            this.EstablecerFiltros();

            this.EstablecerSeguridad();
        }

        public void EstablecerConfiguracionInicial()
        {
            try
            {
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
                listaTiposAutorizacion.Add(-1, "TODOS");
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

        private void EstablecerFiltros()
        {
            try
            {
                Dictionary<string, object> paquete = this.vista.ObtenerPaqueteNavegacion("FiltrosAutorizador") as Dictionary<string, object>;
                if (paquete != null)
                {
                    if (paquete.ContainsKey("ObjetoFiltro"))
                    {
                        if (paquete["ObjetoFiltro"].GetType() == typeof(AutorizadorBO))
                            this.DatoAInterfazUsuario(paquete["ObjetoFiltro"]);
                        else
                            throw new Exception("Se esperaba un objeto AutorizadorBO, el objeto proporcionado no cumple con esta característica, intente de nuevo por favor.");
                    }
                    if (paquete.ContainsKey("Bandera"))
                    {
                        if ((bool)paquete["Bandera"])
                            this.ConsultarAutorizadores();
                    }
                }
                this.vista.LimpiarPaqueteNavegacion("FiltrosAutorizador");
            }
            catch (Exception ex)
            {
                throw new Exception(this.nombreClase + ".EstablecerFiltros: " + ex.Message);
            }
        }

        private void EstablecerSeguridad()
        {
            try
            {
                //se valida que los datos del usuario y la unidad operativa no sean nulos
                if (this.vista.UsuarioID == null) throw new Exception("El identificador del usuario no debe ser nulo");
                if (this.vista.UnidadOperativaID == null) throw new Exception("El identificador de la Unidad Operativa no debe ser nulo ");

                UsuarioBO usr = new UsuarioBO { Id = this.vista.UsuarioID };
                AdscripcionBO adscripcion = new AdscripcionBO { UnidadOperativa = new UnidadOperativaBO { Id = this.vista.UnidadOperativaID } };
                SeguridadBO seguridadBO = new SeguridadBO(Guid.Empty, usr, adscripcion);

                //Se obtienen las acciones a las cuales el usuario tiene permiso en este proceso
                List<CatalogoBaseBO> lst = FacadeBR.ConsultarAccion(dctx, seguridadBO);

                //Se valida si el usuario tiene permiso para consultar
                if (!this.ExisteAccion(lst, "CONSULTAR"))
                    this.vista.RedirigirSinPermisoAcceso();
                //Se valida si el usuario tiene permiso para registrar
                if (!this.ExisteAccion(lst, "INSERTAR"))
                    this.vista.PermitirRegistrar(false);
            }
            catch (Exception ex)
            {
                throw new Exception(nombreClase + ".EstablecerSeguridad:" + ex.Message);
            }
        }
        private bool ExisteAccion(List<CatalogoBaseBO> acciones, string accion)
        {
            if (acciones != null && acciones.Count > 0 && acciones.Exists(p => p != null && p.Nombre != null && p.Nombre.Trim().ToUpper() == accion.Trim().ToUpper()))
                return true;

            return false;
        }

        private Object InterfazUsuarioADato()
        {
            AutorizadorBO bo = new AutorizadorBO();
            bo.Sucursal = new SucursalBO();
            bo.Sucursal.UnidadOperativa = new UnidadOperativaBO();
            bo.Empleado = new EmpleadoBO();

            bo.Empleado.Id = this.vista.EmpleadoID;
            bo.Sucursal.Id = this.vista.SucursalID;
            bo.Sucursal.UnidadOperativa.Id = this.vista.UnidadOperativaID;
            bo.Estatus = this.vista.Estatus;
            if (this.vista.TipoAutorizacion != null)
            {
                switch (this.vista.UnidadOperativaID)
                {
                    case (int)ETipoEmpresa.Generacion:
                        bo.TipoAutorizacion = (ETipoAutorizacionGeneracion)Enum.Parse(typeof(ETipoAutorizacionGeneracion), this.vista.TipoAutorizacion.ToString());
                        break;
                    case (int)ETipoEmpresa.Construccion:
                        bo.TipoAutorizacion = (ETipoAutorizacionConstruccion)Enum.Parse(typeof(ETipoAutorizacionConstruccion), this.vista.TipoAutorizacion.ToString());
                        break;
                    case (int)ETipoEmpresa.Equinova:
                        bo.TipoAutorizacion = (ETipoAutorizacionEquinova)Enum.Parse(typeof(ETipoAutorizacionEquinova), this.vista.TipoAutorizacion.ToString());
                        break;
                    default:
                        bo.TipoAutorizacion = (ETipoAutorizacion)Enum.Parse(typeof(ETipoAutorizacion), this.vista.TipoAutorizacion.ToString());
                        break;
                }
            }

            return bo;
        }
        private void DatoAInterfazUsuario(Object obj)
        {
            AutorizadorBO bo = (AutorizadorBO)obj;
            if (bo.Sucursal == null)
                bo.Sucursal = new SucursalBO();
            if (bo.Empleado == null)
                bo.Empleado = new EmpleadoBO();

            this.vista.SucursalID = bo.Sucursal.Id;
            this.vista.SucursalNombre = bo.Sucursal.Nombre;
            this.vista.EmpleadoID = bo.Empleado.Id;
            this.vista.EmpleadoNombre = bo.Empleado.Nombre;
            this.vista.Estatus = bo.Estatus;
            if (bo.TipoAutorizacion != null)
            {
                switch (this.vista.UnidadOperativaID)
                {
                    case (int)ETipoEmpresa.Generacion:
                        this.vista.TipoAutorizacion = bo.TipoAutorizacion;
                        break;
                    case (int)ETipoEmpresa.Construccion:
                        this.vista.TipoAutorizacion = bo.TipoAutorizacion;
                        break;
                    case (int)ETipoEmpresa.Equinova:
                        this.vista.TipoAutorizacion = bo.TipoAutorizacion;
                        break;
                    default:
                        this.vista.TipoAutorizacion = bo.TipoAutorizacion;
                        break;
                }
            }
            else
                this.vista.TipoAutorizacion = null;
        }

        public void ConsultarAutorizadores()
        {
            string s;
            if ((s = this.ValidarCampos()) != null)
            {
                this.vista.MostrarMensaje(s, ETipoMensajeIU.ADVERTENCIA, null);
                return;
            }

            try
            {
                AutorizadorBO bo = (AutorizadorBO)this.InterfazUsuarioADato();
                List<AutorizadorBO> lst = this.controlador.ConsultarCompleto(this.dctx, bo);

                if (lst.Count < 1)
                    this.vista.MostrarMensaje("La búsqueda no produjo resultados", ETipoMensajeIU.INFORMACION,
                        "No se encontraron coincidencias con los criterios ingresados. Verifique sus datos");

                this.vista.EstablecerResultado(lst);
            }
            catch (Exception ex)
            {
                throw new Exception(this.nombreClase + ".ConsultarAutorizadores: " + ex.Message);
            }
        }

        private string ValidarCampos()
        {
            string s = "";

            if (this.vista.UnidadOperativaID == null)
                s += "UnidadOperativaID, ";

            if (s != null && s.Trim().CompareTo("") != 0)
                return "Los siguientes campos no pueden estar vacíos: \n" + s.Substring(0, s.Length - 2);

            return null;
        }

        public void IrADetalle(int? autorizadorID)
        {
            try
            {
                if (autorizadorID == null)
                    throw new Exception("No se encontró el registro seleccionado.");

                AutorizadorBO bo = new AutorizadorBO { AutorizadorID = autorizadorID };

                this.vista.LimpiarSesion();

                Dictionary<string, object> paquete = new Dictionary<string, object>();
                paquete.Add("ObjetoFiltro", this.InterfazUsuarioADato());
                paquete.Add("Bandera", true);

                this.vista.EstablecerPaqueteNavegacion("FiltrosAutorizador", paquete);
                this.vista.EstablecerPaqueteNavegacion("AutorizadorBO", bo);

                this.vista.RedirigirADetalle();
            }
            catch (Exception ex)
            {
                throw new Exception(this.nombreClase + ".IrADetalle: " + ex.Message);
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

                    if (empleado != null && empleado.Id != null)
                        this.vista.EmpleadoID = empleado.Id;
                    else
                        this.vista.EmpleadoID = null;

                    if (empleado != null && empleado.NombreCompleto != null)
                        this.vista.EmpleadoNombre = empleado.NombreCompleto;
                    else
                        this.vista.EmpleadoNombre = null;
                    break;
            }
        }
        #endregion
        #endregion
    }
}
