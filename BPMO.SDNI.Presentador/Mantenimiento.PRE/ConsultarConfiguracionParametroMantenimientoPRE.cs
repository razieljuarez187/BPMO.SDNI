using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BPMO.SDNI.Mantenimiento.VIS;
using BPMO.Primitivos.Enumeradores;
using BPMO.Patterns.Creational.DataContext;
using BPMO.Facade.SDNI.BR;
using BPMO.Servicio.Catalogos.BO;
using BPMO.SDNI.Mantenimiento.BO;
using BPMO.SDNI.Mantenimiento.BR;
using BPMO.Basicos.BO;


namespace BPMO.SDNI.Mantenimientos.PRE
{
    public class ConsultarConfiguracionParametroMantenimientoPRE
    {
        #region Propiedades
        private string nombreClase;
        private IConsultarConfiguracionParametroMantenimientoVIS vista;
        private ConfiguracionMantenimientoBR ctrConfiguracionMantenimiento;
        private IDataContext dataContext = null;
        #endregion

        #region Constructores

        public ConsultarConfiguracionParametroMantenimientoPRE(IConsultarConfiguracionParametroMantenimientoVIS vista)
        {
            try
            {
                this.vista = vista;
                this.nombreClase = GetType().Name;
                this.ctrConfiguracionMantenimiento = new ConfiguracionMantenimientoBR();
                this.dataContext = FacadeBR.ObtenerConexion();

            }
            catch (Exception ex)
            {
                this.vista.MostrarMensaje("No se pudieron obtener los datos de conexion", ETipoMensajeIU.ERROR,
                   "No se encontraron los parametros de conexion en la fuente de datos, póngase en contacto con el administrador del sistema. " + ex.ToString());
            }

        }

        #endregion

        #region Metodos

        #region Accesos y permisos
        /// <summary>
        /// Valida el acceso a la sección en curso
        /// </summary>
        public void ValidarAcceso()
        {
            try
            {
                //Valida que usuario y la unidad operativa no sean nulos
                if (this.vista.UsuarioAutenticado == null) throw new Exception("El identificador del usuario no debe ser nulo");
                if (this.vista.UnidadOperativaId == null) throw new Exception("El identificador de la Unidad Operativa no debe ser nulo");
                //Se crea el objeto de seguridad
                SeguridadBO seguridad = this.CrearObjetoSeguridad();

                //Se valida si el usuario tiene permiso a la acción principal
                if (!FacadeBR.ExisteAccion(this.dataContext, "CONSULTAR", seguridad))
                    this.vista.RedirigirSinPermisoAcceso();

                //Se valida si el usuario tiene permiso a la acción principal
                if (!FacadeBR.ExisteAccion(this.dataContext, "UI CONSULTAR", seguridad))
                    this.vista.RedirigirSinPermisoAcceso();
            }
            catch (Exception ex)
            {
                throw new Exception(nombreClase + ".ValidarAcceso: " + ex.GetBaseException().Message);
            }
        }

        /// <summary>
        /// Evalua si una acción se cuentra definida
        /// </summary>
        /// <param name="acciones">Lista de acciones donde realizará la búsqueda</param>
        /// <param name="accion">Acción a evaluar</param>
        /// <returns>Devuelve true si la acción está definida, de lo contrario devolverá false</returns>
        private bool ExisteAccion(List<CatalogoBaseBO> acciones, string nombreAccion)
        {
            if (acciones != null && acciones.Count > 0 && acciones.Exists(p => p != null && p.Nombre != null && p.Nombre.Trim().ToUpper() == nombreAccion.Trim().ToUpper()))
                return true;

            return false;
        }

        /// <summary>
        /// Obtiene un objeto de seguridad para validación de permisos
        /// </summary>
        /// <returns></returns>
        private SeguridadBO CrearObjetoSeguridad()
        {
            //Se crea el objeto de seguridad
            UsuarioBO usuario = new UsuarioBO() { Id = this.vista.UsuarioAutenticado };
            AdscripcionBO adscripcion = new AdscripcionBO() { UnidadOperativa = new UnidadOperativaBO() { Id = this.vista.UnidadOperativaId } };
            SeguridadBO seguridadBO = new SeguridadBO(Guid.Empty, usuario, adscripcion);

            return seguridadBO;
        }

        #endregion


        #region buscar Configuracion

        public void buscarConfiguracion()
        {
            try
            {
                ConfiguracionMantenimientoBO bo = new ConfiguracionMantenimientoBO();
                bo = IterfazAObjeto();
                if (bo.Modelo.Nombre == string.Empty)
                {
                    bo.Modelo.Nombre = null;
                    bo.Modelo.Id = null;
                }
                List<ConfiguracionMantenimientoBO> list = ctrConfiguracionMantenimiento.ConsultarCompleto(dataContext, bo);
                if (list != null && list.Count > 0)
                {
                    this.vista.configuraciones = list;
                    this.vista.GridConfiguracionesMantenimiento.DataSource = list;
                    this.vista.GridConfiguracionesMantenimiento.DataBind();
                }
                else
                {
                    this.vista.GridConfiguracionesMantenimiento.DataSource = null;
                    this.vista.GridConfiguracionesMantenimiento.DataBind();
                    this.vista.MostrarMensaje("No se encontraron coincidencias, favor de verificar", ETipoMensajeIU.ADVERTENCIA);
                }
                
            }
            catch (Exception)
            {
                
                throw;
            }
        }

        #endregion

        #region Interfaz a Objeto

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private ConfiguracionMantenimientoBO IterfazAObjeto()
        {
            ConfiguracionMantenimientoBO bo = new ConfiguracionMantenimientoBO();

            bo.Modelo = new ModeloBO
            {
                Nombre = this.vista.Modelo,
                Id = this.vista.ModeloID
            };
            if (this.vista.TipoMantenimiento != 0)
                bo.TipoMantenimiento = this.vista.TipoMantenimiento;
            if (this.vista.UnidadMedida != 0)
                bo.UnidadMedida = this.vista.UnidadMedida;
            bo.Estatus = this.vista.Estatus;

            return bo;
        }

        #endregion

        #region Metodos buscador

        public object PrepararBOBuscador(string catalogo)
        {
            object obj = null;

            switch (catalogo)
            {
                case "Modelo":
                    ModeloBO modelo = new ModeloBO();
                    modelo.Nombre = this.vista.Modelo;
                    obj = modelo;
                    break;
            }

            return obj;
        }

        public void DesplegarResultadoBuscador(string catalogo, object selecto)
        {
            switch (catalogo)
            {
                case "Modelo":
                    ModeloBO modelo = (ModeloBO)selecto;

                    if (modelo != null && modelo.Id != null)
                    {
                        this.vista.Modelo = modelo.Nombre;
                        this.vista.ModeloID = (int)modelo.Id;
                      
                    }
                    else
                        this.vista.Modelo = null;

                    break;
             
            }
        }

        #endregion

        #endregion
    }
}
