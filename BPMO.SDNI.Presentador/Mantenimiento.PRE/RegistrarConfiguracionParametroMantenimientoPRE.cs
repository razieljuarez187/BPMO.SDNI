using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BPMO.SDNI.Mantenimiento.BO;
using System.Web.UI.WebControls;
using BPMO.Primitivos.Enumeradores;
using BPMO.SDNI.Mantenimiento.VIS;
using BPMO.SDNI.Mantenimiento.BR;
using BPMO.Patterns.Creational.DataContext;
using BPMO.Facade.SDNI.BR;
using BPMO.Servicio.Catalogos.BO;
using BPMO.Basicos.BO;

namespace BPMO.SDNI.Mantenimiento.PRE
{
    public class RegistrarConfiguracionParametroMantenimientoPRE
    {
        #region Propiedades
        private string nombreClase;
        private IRegistrarConfiguracionParametroMantenimientoVIS vista;
        private ConfiguracionMantenimientoBR ctrConfiguracionMantenimiento;
        private IDataContext dataContext = null;
        #endregion

        #region Constructores

        public RegistrarConfiguracionParametroMantenimientoPRE(IRegistrarConfiguracionParametroMantenimientoVIS vista)
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
                //valida que usuario y la unidad operativa no sean nulos
                if (this.vista.UsuarioAutenticado == null) throw new Exception("el identificador del usuario no debe ser nulo");
                if (this.vista.UnidadOperativaId == null) throw new Exception("el identificador de la unidad operativa no debe ser nulo");
                //se crea el objeto de seguridad
                SeguridadBO seguridad = this.CrearObjetoSeguridad();

                //se valida si el usuario tiene permiso a la acción principal
                if (!FacadeBR.ExisteAccion(this.dataContext, "UI INSERTAR", seguridad))
                    this.vista.RedirigirSinPermisoAcceso();

                //se valida si el usuario tiene permiso a la acción principal
                if (!FacadeBR.ExisteAccion(this.dataContext, "CONSULTAR", seguridad))
                    this.vista.RedirigirSinPermisoAcceso();

                //se valida si el usuario tiene permiso a la acción principal
                if (!FacadeBR.ExisteAccion(this.dataContext, "INSERTAR", seguridad))
                    this.vista.RedirigirSinPermisoAcceso();
            }
            catch (Exception ex)
            {
                throw new Exception(nombreClase + ".validaracceso: " + ex.GetBaseException().Message);
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

        #region CrearObjetoSeguridad
        /// <summary>
        /// Crea el objeto de seguridad
        /// </summary>
        /// <returns></returns>
        private SeguridadBO obtenerUsuario()
        {
            UsuarioBO usuario = new UsuarioBO { Id = this.vista.UsuarioAutenticado };

            AdscripcionBO adscripcion = new AdscripcionBO
            {
                UnidadOperativa = new UnidadOperativaBO
                {
                    Activo = true,
                    Id = this.vista.UnidadOperativaId
                },
            };

            SeguridadBO seguridadBO = new SeguridadBO(Guid.NewGuid(), usuario, adscripcion);

            return seguridadBO;
        }

        #endregion     

        #region CompararObjetos
        /// <summary>
        /// 
        /// </summary>
        /// <param name="list"></param>
        /// <param name="bo"></param>
        /// <returns></returns>
        private List<bool> CompararObjetos(List<ConfiguracionMantenimientoBO> list, List<ConfiguracionMantenimientoBO> configuraciones)
        {
            List<bool> result = new List<bool>();

            foreach (var bo in configuraciones)
            {
                bool extistencia = list.Exists(x => x.TipoMantenimiento == bo.TipoMantenimiento && x.UnidadMedida == bo.UnidadMedida && x.EnUso == bo.EnUso);
                result.Add(extistencia);
                if (extistencia == true)
                    bo.Estatus = true;
                if (extistencia == false)
                    bo.Estatus = false;
            }

            return result;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="list"></param>
        /// <param name="bo"></param>
        /// <returns></returns>
        private bool CompararObjetos(List<ConfiguracionMantenimientoBO> list, ConfiguracionMantenimientoBO bo)
        {
           
                bool extistencia = list.Exists(x => x.TipoMantenimiento == bo.TipoMantenimiento && x.UnidadMedida == bo.UnidadMedida && x.EnUso == bo.EnUso);

                return extistencia;
        }
        #endregion

        #region Guardar Configuracion
        /// <summary>
        /// 
        /// </summary>
        public int GuardarConfiguracion()
        {
            int Error = 0;
            try
            {
                ConfiguracionMantenimientoBO bo = new ConfiguracionMantenimientoBO();
                SeguridadBO seguridad = this.CrearObjetoSeguridad();
                if (this.vista.Modelo != string.Empty && this.vista.ModeloID != null)
                {
                    bo.Modelo = new ModeloBO
                    {
                        Id = this.vista.ModeloID,
                        Nombre = this.vista.Modelo
                    };
                    bo.Estatus = true;
                    List<ConfiguracionMantenimientoBO> list = ctrConfiguracionMantenimiento.Consultar(dataContext, bo);
                    if (list != null && list.Count > 0)
                    {
                        List<bool> exist = this.CompararObjetos(list, this.vista.configuraciones);
                        this.vista.configuraciones.RemoveAll(x => x.Estatus == true);

                        this.vista.GridConfiguracionesMantenimiento.DataSource = this.vista.configuraciones;
                        this.vista.GridConfiguracionesMantenimiento.DataBind();

                        if (exist.Exists(x => x == true) == true)
                        {
                            this.vista.MostrarMensaje("Algunas configuraciones ya existen para este modelo, favor de verificar", ETipoMensajeIU.ADVERTENCIA);
                            Error = 1;
                        }
                        else
                        {
                            var configGuardar = this.vista.configuraciones;
                            configGuardar.ForEach(x => x.Modelo = bo.Modelo);
                            ctrConfiguracionMantenimiento.Insertar(dataContext, configGuardar, seguridad);
                        }

                    }
                    else
                    {
                        var configGuardar = this.vista.configuraciones;
                        configGuardar.ForEach(x => x.Modelo = bo.Modelo);
                        ctrConfiguracionMantenimiento.Insertar(dataContext, configGuardar, seguridad);
                    }
                }
                else
                {
                    this.vista.MostrarMensaje("Debe seleccionar un modelo:", ETipoMensajeIU.ADVERTENCIA);
                    Error = 1;
                }

            }
            catch (Exception)
            {
                throw;
            }
          
            return Error;
           
        }
        #endregion

        public void agregarAGrid()
        {
            ConfiguracionMantenimientoBO bo = new ConfiguracionMantenimientoBO();
            bo = this.IterfazAObjeto();
            if (bo.Intervalo != null)
            {
                bool exist = this.CompararObjetos(this.vista.configuraciones, bo);
                if (exist == true)
                {
                    this.vista.MostrarMensaje("No se puede agregar dos configuraciones iguales", ETipoMensajeIU.ADVERTENCIA);
                }
                else
                {
                    this.vista.configuraciones.Add(bo);
                    this.vista.GridConfiguracionesMantenimiento.DataSource = this.vista.configuraciones;
                    this.vista.GridConfiguracionesMantenimiento.DataBind();
                }

            }
            else
            {
                this.vista.MostrarMensaje("Deba agregar un intervalo", ETipoMensajeIU.ADVERTENCIA);           
            }
            
 
        }

        public void editarGrid(ConfiguracionMantenimientoBO GridBO, int index)
        {
            //var bo = this.IterfazAObjeto();
            this.ObjetoAInterfaz(GridBO);
            this.vista.configuraciones.RemoveAt(index);
            //this.vista.configuraciones.Add(bo);       
            this.vista.GridConfiguracionesMantenimiento.DataSource = this.vista.configuraciones;
            this.vista.GridConfiguracionesMantenimiento.DataBind();
        }

        #region Interfaz a Objeto
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private ConfiguracionMantenimientoBO IterfazAObjeto()
        {
            ConfiguracionMantenimientoBO bo = new ConfiguracionMantenimientoBO();

            //bo.Modelo = new ModeloBO
            //{
            //    Nombre = this.vista.Modelo,
            //    Id = this.vista.ModeloID
            //};
            if (this.vista.TipoMantenimiento != 0)
                bo.TipoMantenimiento = this.vista.TipoMantenimiento;
            if (this.vista.UnidadMedida != 0)
                bo.UnidadMedida = this.vista.UnidadMedida;
            bo.EnUso = this.vista.EnUso;
            bo.Intervalo = this.vista.Intervalo;
            bo.Estatus = true;

            return bo;
        }

        #endregion

        #region Objeto A Interfaz

        private void ObjetoAInterfaz(ConfiguracionMantenimientoBO bo)
        {
            this.vista.TipoMantenimiento = bo.TipoMantenimiento;
            this.vista.UnidadMedida = bo.UnidadMedida;
            this.vista.Intervalo = bo.Intervalo;
            this.vista.EnUso = (bool)bo.EnUso;
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
                        this.vista.BtnModelo.Enabled = false;
                        this.vista.TxbModelo.Enabled = false;
                      
                    }
                    else
                        this.vista.Modelo = null;

                    break;
             
            }
        }

        #endregion

        #endregion

        public ConfiguracionMantenimientoBO ConsultarInsertado()
        {
            var bo = this.vista.configuraciones.FirstOrDefault();
            bo = ctrConfiguracionMantenimiento.ConsultarCompleto(dataContext,bo).FirstOrDefault();
            return bo;
            
        }
    }
}
