using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BPMO.SDNI.Mantenimiento.VIS;
using BPMO.SDNI.Mantenimiento.BR;
using BPMO.Patterns.Creational.DataContext;
using BPMO.Facade.SDNI.BR;
using BPMO.SDNI.Mantenimiento.BO;
using BPMO.Basicos.BO;
using BPMO.Primitivos.Enumeradores;
using BPMO.Servicio.Catalogos.BO;

namespace BPMO.SDNI.Mantenimiento.PRE
{
    public class DetalleConfiguracionParametroMantenimientoPRE
    {
        #region Propiedades
        private string nombreClase;
        private IDetalleConfiguracionParametroMantenimientoVIS vista;
        private ConfiguracionMantenimientoBR ctrConfiguracionMantenimiento;
        private IDataContext dataContext = null;
        #endregion

        #region Constructores

        public DetalleConfiguracionParametroMantenimientoPRE(IDetalleConfiguracionParametroMantenimientoVIS vista)
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

            }
            catch (Exception ex)
            {
                throw new Exception(nombreClase + ".ValidarAcceso: " + ex.GetBaseException().Message);
            }
        }

        public void EstablecerSeguridad()
        {
            try
            {
                //Valida que usuario y la unidad operativa no sean nulos
                if (this.vista.UsuarioAutenticado == null) throw new Exception("El identificador del usuario no debe ser nulo");
                if (this.vista.UnidadOperativaId == null) throw new Exception("El identificador de la Unidad Operativa no debe ser nulo");
                //Se crea el objeto de seguridad
                SeguridadBO seguridad = this.CrearObjetoSeguridad();

                //Se valida si el usuario tiene permiso a la acción principal
                if (!FacadeBR.ExisteAccion(this.dataContext, "UI ACTUALIZAR", seguridad))
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
                bool extistencia = list.Exists(x => x.TipoMantenimiento == bo.TipoMantenimiento && x.UnidadMedida == bo.UnidadMedida && x.Intervalo == bo.Intervalo && x.EnUso == bo.EnUso);
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
           
                bool extistencia = list.Exists(x => x.TipoMantenimiento == bo.TipoMantenimiento && x.UnidadMedida == bo.UnidadMedida && x.Intervalo == bo.Intervalo && x.EnUso == bo.EnUso);

                return extistencia;
        }
        #endregion

        #region Guardar Configuracion
        /// <summary>
        /// 
        /// </summary>
        public void GuardarConfiguracion()
        {
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
                    List<ConfiguracionMantenimientoBO> list = ctrConfiguracionMantenimiento.Consultar(dataContext, bo);
                    if (list != null && list.Count > 0)
                    {
                        List<bool> exist = this.CompararObjetos(list,this.vista.configuraciones);
                        this.vista.configuraciones.RemoveAll(x => x.Estatus == true);
                        
                        this.vista.GridConfiguracionesMantenimiento.DataSource = this.vista.configuraciones;
                        this.vista.GridConfiguracionesMantenimiento.DataBind();

                        if (exist.Exists(x => x == true) == true)
                            this.vista.MostrarMensaje("Algunas configuraciones ya existen para este modelo, favor de verificar", ETipoMensajeIU.ADVERTENCIA);
                        else
                        {
                            var configGuardar = this.vista.configuraciones;
                            configGuardar.ForEach(x => x.Modelo = bo.Modelo);
                            ctrConfiguracionMantenimiento.Insertar(dataContext, configGuardar, seguridad);
                            this.vista.MostrarMensaje("Guardado con exito", ETipoMensajeIU.EXITO);
                        }

                    }
                    else
                    {
                        var configGuardar = this.vista.configuraciones;
                        configGuardar.ForEach(x => x.Modelo = bo.Modelo);
                        ctrConfiguracionMantenimiento.Insertar(dataContext, configGuardar, seguridad);
                        this.vista.MostrarMensaje("Guardado con exito", ETipoMensajeIU.EXITO);
                    }
                }
                else
                {
                    this.vista.MostrarMensaje("Debe seleccionar un modelo:", ETipoMensajeIU.ADVERTENCIA);
                }         

            }
            catch (Exception)
            {
                throw;
            }
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

        public void editarGrid(ConfiguracionMantenimientoBO GridBO)
        {
            var bo = this.IterfazAObjeto();
            this.vista.configuraciones.Add(bo);
            this.ObjetoAInterfaz(GridBO);
            this.vista.configuraciones.Remove(GridBO);
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

            bo.ConfiguracionMantenimientoId = this.vista.ConfiguracionID;
            bo.Modelo = new ModeloBO
            {
                Nombre = this.vista.Modelo,
                Id = this.vista.ModeloID
            };
            bo.TipoMantenimiento = this.vista.TipoMantenimiento;
            bo.UnidadMedida = this.vista.UnidadMedida;
            bo.EnUso = this.vista.EnUso;
            bo.Intervalo = this.vista.Intervalo;
            bo.Estatus = this.vista.Estatus;

            return bo;
        }

        #endregion

        #region Objeto A Interfaz

        private void ObjetoAInterfaz(ConfiguracionMantenimientoBO bo)
        {
            this.vista.ConfiguracionID = bo.ConfiguracionMantenimientoId;
            this.vista.Modelo = bo.Modelo.Nombre;
            this.vista.ModeloID = bo.Modelo.Id;
            this.vista.TipoMantenimiento = bo.TipoMantenimiento;
            this.vista.UnidadMedida = bo.UnidadMedida;
            this.vista.Intervalo = bo.Intervalo;
            this.vista.EnUso = (bool)bo.EnUso;
            this.vista.Estatus = (bool)bo.Estatus;
        }

        #endregion

        #region Metodos buscador

        #endregion

        #endregion

        public void ConsultarDetalle(ConfiguracionMantenimientoBO bo)
        {
            var filtro = new ConfiguracionMantenimientoBO() { Modelo = new ModeloBO { Id = bo.Modelo.Id }, Estatus = true }; 
            List<ConfiguracionMantenimientoBO> list = ctrConfiguracionMantenimiento.ConsultarCompleto(dataContext,filtro);
            if (list != null && list.Count > 0)
            {                
                this.vista.configuraciones = list;
                this.vista.configuraciones.Remove(bo);
                this.ObjetoAInterfaz(bo);

            }
        }

        public void redireccionar()
        {
            var bo = this.IterfazAObjeto();
            this.vista.ConfiguracionRecibida = bo;
        }

        public void ConsultarDetalleInactivo(ConfiguracionMantenimientoBO bo)
        {
            var filtro = new ConfiguracionMantenimientoBO() { ConfiguracionMantenimientoId = bo.ConfiguracionMantenimientoId };
            List<ConfiguracionMantenimientoBO> list = ctrConfiguracionMantenimiento.ConsultarCompleto(dataContext, filtro);
            if (list != null && list.Count > 0)
            {
                this.vista.configuraciones = list;
                this.ObjetoAInterfaz(bo);

            }
        }

        public void activarConfiguracion()
        {
            var seguridad = this.CrearObjetoSeguridad();
            var bo = this.IterfazAObjeto();
            var filtro = new ConfiguracionMantenimientoBO()
            {
                Modelo = bo.Modelo,
                Estatus = true
            };
            var list = ctrConfiguracionMantenimiento.Consultar(dataContext, filtro);
            bool existemcia = list.Exists(x => x.TipoMantenimiento == bo.TipoMantenimiento && x.UnidadMedida == bo.UnidadMedida && x.EnUso == bo.EnUso);
            if (existemcia == false)
            {
                bo.Estatus = true;
                ctrConfiguracionMantenimiento.Actualizar(dataContext, bo, seguridad);
            }
            else if (existemcia == true)
            {
                this.vista.MostrarMensaje("YA EXISTE UNA CONFIGURACION ACTIVA CON ESTOS PARAMETROS, NO ES POSIBLE REACTIVAR", ETipoMensajeIU.ADVERTENCIA);
            }
        }
    }
}
