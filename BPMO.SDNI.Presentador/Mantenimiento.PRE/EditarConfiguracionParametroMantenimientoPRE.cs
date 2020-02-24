using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BPMO.SDNI.Mantenimiento.VIS;
using BPMO.SDNI.Mantenimiento.BR;
using BPMO.Patterns.Creational.DataContext;
using BPMO.Facade.SDNI.BR;
using BPMO.Primitivos.Enumeradores;
using BPMO.Basicos.BO;
using BPMO.SDNI.Mantenimiento.BO;
using BPMO.Servicio.Catalogos.BO;

namespace BPMO.SDNI.Mantenimiento.PRE
{
    public class EditarConfiguracionParametroMantenimientoPRE
    {
        #region Propiedade
        private string nombreClase;
        private IEditarConfiguracionParametroMantenimientoVIS vista;
        private ConfiguracionMantenimientoBR ctrConfiguracionMantenimiento;
        private IDataContext dataContext = null;
        #endregion

        #region Constructores

        public EditarConfiguracionParametroMantenimientoPRE(IEditarConfiguracionParametroMantenimientoVIS vista)
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
                if (!FacadeBR.ExisteAccion(this.dataContext, "UI ACTUALIZAR", seguridad))
                    this.vista.RedirigirSinPermisoAcceso();

                //Se valida si el usuario tiene permiso a la acción principal
                if (!FacadeBR.ExisteAccion(this.dataContext, "ACTUALIZAR", seguridad))
                    this.vista.RedirigirSinPermisoAcceso();

                //Se valida si el usuario tiene permiso a la acción principal
                if (!FacadeBR.ExisteAccion(this.dataContext, "CONSULTAR", seguridad))
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
        public void GuardarConfiguracion()
        {
            try
            {
                ConfiguracionMantenimientoBO bo = new ConfiguracionMantenimientoBO();
                SeguridadBO seguridad = this.obtenerUsuario();
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
                }

            }
            else
            {
                this.vista.MostrarMensaje("Deba agregar un intervalo", ETipoMensajeIU.ADVERTENCIA);           
            }
            
 
        }

        public void editarGrid(ConfiguracionMantenimientoBO GridBO)
        {
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
            if (this.vista.TipoMantenimiento != 0)
                bo.TipoMantenimiento = this.vista.TipoMantenimiento;
            if (this.vista.UnidadMedida != 0)
                bo.UnidadMedida = this.vista.UnidadMedida;
            bo.EnUso = this.vista.EnUso;
            bo.Intervalo = this.vista.Intervalo;
            bo.Estatus = this.vista.Activo;

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
            this.vista.Activo = (bool)bo.Estatus;
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
        public int ActualizarConfiguracion()
        {
            int error = 0;
            var bo = this.IterfazAObjeto();
            var filtro = new ConfiguracionMantenimientoBO()
            {
                Modelo = new ModeloBO { Id = bo.Modelo.Id},
                Estatus = true
            };
            var list = ctrConfiguracionMantenimiento.Consultar(dataContext,filtro);
            bool existemcia = list.Exists(x => x.TipoMantenimiento == bo.TipoMantenimiento && x.UnidadMedida == bo.UnidadMedida && x.EnUso == bo.EnUso && x.Intervalo == bo.Intervalo);
            if (existemcia == false)
            {
                 if (bo.Intervalo != null)
                {
                    SeguridadBO seguridad = this.obtenerUsuario();
                    ctrConfiguracionMantenimiento.Actualizar(dataContext, bo, seguridad);
                    this.vista.ConfiguracionRecibida = bo;
                }
                else
                {
                    error = 1;
                }
            }
            else if (existemcia == true)
            {
                error = 2;
            }
           
            return error;
        }

        public void EliminarConfiguracion()
        {
            var bo = this.IterfazAObjeto();
            SeguridadBO seguridad = this.obtenerUsuario();
            ctrConfiguracionMantenimiento.Actualizar(dataContext, bo, seguridad);
        }



        public void ConsultarDetalle(ConfiguracionMantenimientoBO bo)
        {    
                this.ObjetoAInterfaz(bo);
        }


        public void Regresar()
        {
            throw new NotImplementedException();
        }
    }
}
