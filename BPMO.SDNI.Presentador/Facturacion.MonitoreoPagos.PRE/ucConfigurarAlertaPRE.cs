//Satisface el caso de uso CU009 – Configuración Notificación de facturación

using System;
using BPMO.Basicos.BO;
using BPMO.Facade.SDNI.BR;
using BPMO.Patterns.Creational.DataContext;
using BPMO.SDNI.Facturacion.MonitoreoPagos.VIS;
using BPMO.Facade.SDNI.BOF;

namespace BPMO.SDNI.Facturacion.MonitoreoPagos.PRE
{
    /// <summary>
    /// Presentador para la vista que despliega los campos un registro
    /// </summary>
    public class ucConfigurarAlertaPRE
    {
        #region Atributos
        /// <summary>
        /// Nombre de la clase para agregar a los mensajes de Error
        /// </summary>
        private const String nombreClase = "ucConfigurarAlertaPRE";

        /// <summary>
        /// El DataContext que proveerá acceso a la base de datos
        /// </summary>
        private IDataContext dctx = null;

        /// <summary>
        /// La vista que esta mostrando los datos procesados en el presentador
        /// </summary>
        private IucConfigurarAlertaVIS vista;
        #endregion

        #region Propiedades
        /// <summary>
        /// Vista sobre la que actua el presentador
        /// </summary>
        internal IucConfigurarAlertaVIS Vista { get { return vista; } }
        #endregion

        #region Constructor
        /// <summary>
        /// Contsructor por default del presentador
        /// </summary>
        /// <param name="iUcConfigurarNotificacionVis">Vista que será gestionada por el presentador</param>
        public ucConfigurarAlertaPRE(IucConfigurarAlertaVIS iUcConfigurarNotificacionVis)
		{
            this.vista = iUcConfigurarNotificacionVis;
            this.dctx = FacadeBR.ObtenerConexion();
		}
        #endregion

        #region Métodos
        /// <summary>
        /// Realiza el proceso de inicializar el visor para capturar un nuevo registro
        /// </summary>
        public void PrepararNuevo()
        {
            this.LimpiarSesion();
            this.vista.PrepararNuevo();            
        }

        /// <summary>
        /// Realiza el proceso de inicializar el visor para editar un registro existente
        /// </summary>
        public void PrepararEdicion()
        {
            this.vista.PrepararEdicion();              
        }

        /// <summary>
        /// Realiza el proceso de inicializar el visor para mostrar los datos de un registro
        /// </summary>
        public void PrepararVisualizacion()
        {
            this.vista.PrepararVisualizacion();            
        }

        /// <summary>
        /// Libera o elimina los valores en sesión guardados en el visor
        /// </summary>
        public void LimpiarSesion()
        {
            this.vista.LimpiarSesion();
        }

        /// <summary>
        /// Prepara un BO para la Busqueda en su respectivo catalogo
        /// </summary>
        /// <param name="catalogo">catalogo donde se realizara la busqueda</param>
        /// <returns>Objeto que define el filtro a aplicar al buscador</returns>
        public object PrepararBOBuscador(string catalogo)
        {
            object obj = null;

            switch (catalogo)
            {
                case "Sucursal":
                    {
                        SucursalBOF sucursal = new SucursalBOF();
                        sucursal.UnidadOperativa = new UnidadOperativaBO { Id = this.vista.UnidadOperativaID };
                        sucursal.Usuario = new UsuarioBO { Id = this.vista.UsuarioID };
                        sucursal.Nombre = this.vista.SucursalNombre;                        
                        sucursal.Activo = true;

                        obj = sucursal;
                    }
                    break;

                case "Empleado":
                    {
                        EmpleadoBO empleado = new EmpleadoBO();
                        empleado.Asignacion = new OrganizacionBO { Sucursal = this.vista.ClaveSucursal };
                        empleado.NombreCompleto = this.vista.EmpleadoNombre;
                        empleado.Activo = true;

                        obj = empleado;
                    }
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
                    if (sucursal != null)
                    {
                        if (sucursal.Id != null)
                        {
                            if (this.vista.SucursalID != sucursal.Id)
                            {
                                this.vista.EmpleadoID = null;
                                this.vista.EmpleadoNombre = null;
                            }

                            this.vista.SucursalID = sucursal.Id;
                        }
                        else
                        {
                            this.vista.EmpleadoID = null;
                            this.vista.EmpleadoNombre = null;

                            this.vista.SucursalID = null;
                        }

                        if (sucursal.Nombre != null)
                            this.vista.SucursalNombre = sucursal.Nombre;
                        else
                            this.vista.SucursalNombre = null;

                        if (sucursal.NombreCorto != null)
                        {
                            this.vista.ClaveSucursal = sucursal.NombreCorto;
                            this.vista.PermitirSeleccionarEmpleado(true);
                        }
                        else
                        {
                            this.vista.ClaveSucursal = null;
                            this.vista.PermitirSeleccionarEmpleado(false);
                        }
                    }
                    break;

                case "Empleado":
                    {
                        EmpleadoBO empleado = (EmpleadoBO)selecto;

                        if (empleado != null)
                        {
                            if (empleado.Id != null)
                                this.vista.EmpleadoID = empleado.Id;
                            else
                                this.vista.EmpleadoID = null;

                            if (empleado.NombreCompleto != null)
                                this.vista.EmpleadoNombre = empleado.NombreCompleto;
                            else
                                this.vista.EmpleadoNombre = null;

                            if (empleado.Email != null)
                                this.vista.CorreoElectronico = empleado.Email;
                            else
                                this.vista.CorreoElectronico = null;
                        }
                    }
                    break;
            }
        }

        /// <summary>
        /// Realiza la validación de los datos capturados para detectar errores o inconsistencias
        /// </summary>
        /// <returns>Objeto de tipo String que contiene el error detectado, en caso contrario devolverá nulo</returns>
        public String ValidarCampos()
        {
            return this.vista.ValidarCampos();
        }
        #endregion
    }
}
