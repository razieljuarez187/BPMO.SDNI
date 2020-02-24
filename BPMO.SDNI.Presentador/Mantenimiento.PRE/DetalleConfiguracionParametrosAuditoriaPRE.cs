///Satisface al caso de uso CU070 - Configurar Parametros de auditoria
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BPMO.SDNI.Mantenimiento.VIS;
using BPMO.SDNI.Mantenimiento.BR;
using BPMO.Facade.SDNI.BR;
using BPMO.Patterns.Creational.DataContext;
using BPMO.Primitivos.Enumeradores;
using BPMO.SDNI.Mantenimiento.BO;
using BPMO.Servicio.Catalogos.BO;
using System.Web.UI.WebControls;
using BPMO.Basicos.BO;

namespace BPMO.SDNI.Mantenimiento.PRE
{
    public class DetalleConfiguracionParametrosAuditoriaPRE
    {
        #region Propiedades
        private IDetalleConfiguracionParametrosAuditoriaVIS vista;
        private IDataContext dataContext = null;
        private ConfiguracionAuditoriaMantenimientoBR ctrConfiguracionAuditoria;
        private string nombreClase = typeof(DetalleConfiguracionParametrosAuditoriaPRE).Name;
        #endregion

        #region Constructores
        /// <summary>
        /// Inicializa los componentes principales
        /// </summary>
        /// <param name="vista"></param>
        public DetalleConfiguracionParametrosAuditoriaPRE(IDetalleConfiguracionParametrosAuditoriaVIS vista)
        {
            try
            {
                this.vista = vista;
                this.ctrConfiguracionAuditoria = new ConfiguracionAuditoriaMantenimientoBR();
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
                if (!FacadeBR.ExisteAccion(this.dataContext, "CONSULTARCOMPLETO", seguridad))
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
            SeguridadBO seguridadBO = new SeguridadBO(Guid.Empty, usuario, adscripcion);

            return seguridadBO;
        }

        #endregion

        #region CrearObjetoSeguridad
        /// <summary>
        /// Crea el objeto de seguridad
        /// </summary>
        /// <returns></returns>
        private SeguridadBO obtenerSeguridad()
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

        /// <summary>
        /// Mapea la interfaz a un objeto
        /// </summary>
        /// <returns></returns>
        private ConfiguracionAuditoriaMantenimientoBO InterfazUsuarioAObjeto()
        {
            var bo = new ConfiguracionAuditoriaMantenimientoBO();
            bo.Taller.Id = this.vista.TallerID;
            bo.Taller.Nombre = this.vista.Taller;
            bo.TipoMantenimiento = (ETipoMantenimiento)Enum.ToObject(typeof(ETipoMantenimiento), this.vista.TipoMantenimientoID);
            bo.Modelo.Id = this.vista.ModeloID;
            bo.Modelo.Nombre = this.vista.Modelo;
            bo.NumeroActividadesAleatorias = this.vista.Aleatorias;
            foreach (GridViewRow item in this.vista.GridActividadesAuditoria.Rows)
            {
                DetalleConfiguracionAuditoriaMantenimientoBO detalle = new DetalleConfiguracionAuditoriaMantenimientoBO();
                detalle.ConfiguracionPosicionTrabajo = new ConfiguracionPosicionTrabajoBO();

                Label NombreActividad = item.FindControl("lbActividadAuditoria") as Label;
                Label idActividad = item.FindControl("lbActividadID") as Label;
                Label txtCriterio = item.FindControl("lbCriterioActividad") as Label;
                CheckBox chbxObligatoria = item.FindControl("chbxObligatoria") as CheckBox;

                detalle.ConfiguracionPosicionTrabajo.DescriptorTrabajo = new DescriptorTrabajoBO { Nombre = NombreActividad.Text };
                detalle.ConfiguracionPosicionTrabajo.Id = int.Parse(idActividad.Text);
                detalle.Criterio = txtCriterio.Text;

                if (chbxObligatoria.Checked == true)
                    detalle.Obligatorio = true;
                else if (chbxObligatoria.Checked == false)
                    detalle.Obligatorio = false;

                bo.DetalleConfiguracion.Add(detalle);
            }
            return bo;

        }

        /// <summary>
        /// Hace una copia del las actividades del grid de actividades 
        /// </summary>
        public void respaldarSelecciones()
        {
            var bo = this.InterfazUsuarioAObjeto();
            foreach (var item in bo.DetalleConfiguracion)
            {
                var actividad = this.vista.ActividadesAuditoria.Find(x => x.ConfiguracionPosicionTrabajo.Id == item.ConfiguracionPosicionTrabajo.Id);
                actividad.Criterio = item.Criterio;
                actividad.Obligatorio = item.Obligatorio;           
            }
        }

        /// <summary>
        /// Complementa los datos de las actividades
        /// </summary>
        /// <param name="configuraciones"></param>
        /// <returns></returns>
        private List<ConfiguracionAuditoriaMantenimientoBO> CompletarDatos(List<ConfiguracionAuditoriaMantenimientoBO> configuraciones)
        {
            foreach (var item in configuraciones)
	        {
                 var configuracionPosicionTrabajo = new ConfiguracionPosicionTrabajoBO()
                {
                    ConfiguracionModeloMotorizacion = new ConfiguracionModeloMotorizacionBO()
                    {
                        Modelo = item.Modelo
                    },
                    DescriptorTrabajo = new DescriptorTrabajoBO 
                    {
                        Nombre = item.TipoMantenimiento.ToString()
                    }
                };

                 var configPosicion = FacadeBR.ConsultarConfiguracionPosicionTrabajoDetalle(dataContext, configuracionPosicionTrabajo);

                 if (configPosicion != null && configPosicion.Count > 0)
                 {
                     var posicion = FacadeBR.ConsultarConfiguracionPosicionTrabajoDetallePaquete(dataContext, configPosicion.FirstOrDefault(), null);
                     foreach (var itemPaquete in posicion.ConfiguracionPosicionTrabajoPaquete)
                     {
                         var pmEncontrado = item.DetalleConfiguracion.Find(x => x.ConfiguracionPosicionTrabajo.Id == itemPaquete.ConfiguracionPosicionTrabajo.Id);
                         if (pmEncontrado != null)
                         {
                             pmEncontrado.ConfiguracionPosicionTrabajo = itemPaquete.ConfiguracionPosicionTrabajo;
                         }
                         else if (pmEncontrado == null)
                         {
                             var ActividadNueva = new DetalleConfiguracionAuditoriaMantenimientoBO
                             {
                                 ConfiguracionPosicionTrabajo = itemPaquete.ConfiguracionPosicionTrabajo
                             };

                             item.DetalleConfiguracion.Add(ActividadNueva);
                         }

                     }
                     foreach (var itemActividades in item.DetalleConfiguracion)
                     {
                         if (itemActividades.ConfiguracionPosicionTrabajo.DescriptorTrabajo == null)
                             itemActividades.ConfiguracionPosicionTrabajo.DescriptorTrabajo = new DescriptorTrabajoBO { Nombre = "ACTIVIDAD ELIMINADA EN SERVICIO" };
                     }
                 }
                 else
                 {
                     configuraciones = null;
                 }
		 
	        }

            return configuraciones;
        }

        /// <summary>
        /// Agrega una  configuracion al grid
        /// </summary>
        /// <param name="bo"></param>
        public void agregarGrid(ConfiguracionAuditoriaMantenimientoBO bo)
        {
            this.vista.ConfiguracionID = bo.ConfiguracionAuditoriaMantenimientoId;
            this.vista.TipoMantenimiento = bo.TipoMantenimiento.ToString();
            this.vista.TipoMantenimientoID = (int)bo.TipoMantenimiento;
            this.vista.Aleatorias = bo.NumeroActividadesAleatorias;
        }

        /// <summary>
        /// Consulta y muestra el detalle de la configuracion recibida
        /// </summary>
        public void mostrarDetalles()
        {
            var recibida = this.vista.ConfiguracionRecibida;
            this.vista.ConfiguracionID = recibida.ConfiguracionAuditoriaMantenimientoId;
            this.vista.SucursalID = recibida.Sucursal.Id;
            this.vista.SucursalNombre = recibida.Sucursal.Nombre;
            this.vista.Taller = recibida.Taller.Nombre;
            this.vista.TallerID = recibida.Taller.Id;
            this.vista.Modelo = recibida.Modelo.Nombre;
            this.vista.ModeloID = recibida.Modelo.Id;
            this.vista.TipoMantenimiento = recibida.TipoMantenimiento.ToString();
            this.vista.TipoMantenimientoID = (int)recibida.TipoMantenimiento;
            this.vista.Aleatorias = recibida.NumeroActividadesAleatorias;

            var filtro = new ConfiguracionAuditoriaMantenimientoBO()
            {
                Sucursal = new SucursalBO { Id = recibida.Sucursal.Id },
                Taller = new TallerBO { Id = recibida.Taller.Id },
                Modelo = new ModeloBO { Id = recibida.Modelo.Id },
                Activo = true
            };

            var configuraciones = ctrConfiguracionAuditoria.Consultar(dataContext, filtro);

            if (configuraciones != null && configuraciones.Count > 0)
            {

                configuraciones = this.CompletarDatos(configuraciones);
                this.vista.ConfiguracionesAuditoria = configuraciones;
                this.vista.GridConfiguracionesAuditoria.DataSource = this.vista.ConfiguracionesAuditoria;
                this.vista.GridConfiguracionesAuditoria.DataBind();

                var configDetalle = this.vista.ConfiguracionesAuditoria.Find(x => x.TipoMantenimiento == recibida.TipoMantenimiento);
                this.vista.ActividadesAuditoria = configDetalle.DetalleConfiguracion;
                this.vista.GridActividadesAuditoria.DataSource = this.vista.ActividadesAuditoria;
                this.vista.GridActividadesAuditoria.DataBind();
            }
            else
            {
                this.vista.MostrarMensaje("NO SE ENCONTRO EL PAQUETE DE MANTENIMIENTO DE ESTA CONFIGURACION DE AUDITORIA EN LIDER, EL PAQUETE SE ELIMINO O CAMBIO DE NOMBRE?", ETipoMensajeIU.ERROR);
            }
        }

        /// <summary>
        /// Consulta y muestra los detalles de una actividad inactiva
        /// </summary>
        public void mostrarInactivo()
        {
            var recibida = this.vista.ConfiguracionRecibida;
            this.vista.ConfiguracionID = recibida.ConfiguracionAuditoriaMantenimientoId;
            this.vista.SucursalID = recibida.Sucursal.Id;
            this.vista.SucursalNombre = recibida.Sucursal.Nombre;
            this.vista.Taller = recibida.Taller.Nombre;
            this.vista.TallerID = recibida.Taller.Id;
            this.vista.Modelo = recibida.Modelo.Nombre;
            this.vista.ModeloID = recibida.Modelo.Id;
            this.vista.TipoMantenimiento = recibida.TipoMantenimiento.ToString();
            this.vista.TipoMantenimientoID = (int)recibida.TipoMantenimiento;
            this.vista.Aleatorias = recibida.NumeroActividadesAleatorias;

            var filtro = new ConfiguracionAuditoriaMantenimientoBO()
            {
                ConfiguracionAuditoriaMantenimientoId = recibida.ConfiguracionAuditoriaMantenimientoId,
                Sucursal = new SucursalBO { Id = recibida.Sucursal.Id },
                Taller = new TallerBO { Id = recibida.Taller.Id },
                Modelo = new ModeloBO { Id = recibida.Modelo.Id },
            };

            var configuraciones = ctrConfiguracionAuditoria.Consultar(dataContext, filtro);

            if (configuraciones != null && configuraciones.Count > 0)
            {

                configuraciones = this.CompletarDatos(configuraciones);
                this.vista.ConfiguracionesAuditoria = configuraciones;
                this.vista.GridConfiguracionesAuditoria.DataSource = this.vista.ConfiguracionesAuditoria;
                this.vista.GridConfiguracionesAuditoria.DataBind();

                var configDetalle = this.vista.ConfiguracionesAuditoria.Find(x => x.TipoMantenimiento == recibida.TipoMantenimiento);
                this.vista.ActividadesAuditoria = configDetalle.DetalleConfiguracion;
                this.vista.GridActividadesAuditoria.DataSource = this.vista.ActividadesAuditoria;
                this.vista.GridActividadesAuditoria.DataBind();
            }
            else
            {
                this.vista.MostrarMensaje("NO SE ENCONTRO EL PAQUETE DE MANTENIMIENTO DE ESTA CONFIGURACION DE AUDITORIA EN LIDER, EL PAQUETE SE ELIMINO O CAMBIO DE NOMBRE?", ETipoMensajeIU.ERROR);
            }
        }

        /// <summary>
        /// Activa una configuracion inactiva
        /// </summary>
        public void activarConfiguracion()
        {
            var seguridad = this.CrearObjetoSeguridad();
            var bo = this.vista.ConfiguracionRecibida;
            var filtro = new ConfiguracionAuditoriaMantenimientoBO()
            {
                Sucursal = bo.Sucursal,
                Modelo = bo.Modelo,
                Taller = bo.Taller,
                Activo = true
            };
            var list = ctrConfiguracionAuditoria.Consultar(dataContext, filtro);
            var encontrado = list.Exists(x => x.Sucursal.Id == bo.Sucursal.Id && x.Taller.Id == bo.Taller.Id && x.Modelo.Id == bo.Modelo.Id && x.TipoMantenimiento == bo.TipoMantenimiento);
            if (encontrado == false)
            {
                ctrConfiguracionAuditoria.Activar(dataContext, bo, seguridad);
            }
            else if (encontrado == true)
            {
                this.vista.MostrarMensaje("YA EXISTE UNA CONFIGURACION CON LOS MISMOS PARAMETROS, NO ES POSIBLE REACTIVAR", ETipoMensajeIU.ADVERTENCIA);
            }
        }

        #endregion       
        
    }
}
