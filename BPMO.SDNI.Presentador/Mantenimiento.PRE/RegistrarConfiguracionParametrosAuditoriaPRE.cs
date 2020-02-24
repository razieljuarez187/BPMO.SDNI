using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BPMO.SDNI.Mantenimiento.VIS;
using BPMO.SDNI.Mantenimiento.BR;
using BPMO.Patterns.Creational.DataContext;
using BPMO.Facade.SDNI.BR;
using BPMO.Primitivos.Enumeradores;
using BPMO.SDNI.Mantenimiento.BO;
using BPMO.Servicio.Catalogos.BO;
using System.Web.UI.WebControls;
using BPMO.Basicos.BO;
using System.Data;

namespace BPMO.SDNI.Mantenimiento.PRE
{
    public class RegistrarConfiguracionParametrosAuditoriaPRE
    {
        #region Propiedades
        private string nombreClase;
        private IRegistrarConfiguracionParametrosAuditoriaVIS vista;
        private IucConfiguracionParametrosAuditoriaVIS ucvista;
        private ConfiguracionAuditoriaMantenimientoBR ctrConfiguracionAuditoria;
        private IDataContext dataContext = null;
        #endregion

        #region Constructores

        public RegistrarConfiguracionParametrosAuditoriaPRE(IRegistrarConfiguracionParametrosAuditoriaVIS vista, IucConfiguracionParametrosAuditoriaVIS ucvista)
        {
            try
            {
                this.vista = vista;
                this.nombreClase = GetType().Name;
                this.ucvista = ucvista;
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
      
        #region Accesos y permisos
        /// <summary>
        /// Valida el acceso a la sección en curso
        /// </summary>
        public void ValidarAcceso()
        {
            try
            {
                //Valida que usuario y la unidad operativa no sean nulos
                if (this.ucvista.UsuarioAutenticado == null) throw new Exception("El identificador del usuario no debe ser nulo");
                if (this.ucvista.UnidadOperativaId == null) throw new Exception("El identificador de la Unidad Operativa no debe ser nulo");
                //Se crea el objeto de seguridad
                SeguridadBO seguridad = this.CrearObjetoSeguridad();

                //Se valida si el usuario tiene permiso a la acción principal
                if (!FacadeBR.ExisteAccion(this.dataContext, "UI INSERTAR", seguridad))
                    this.ucvista.RedirigirSinPermisoAcceso();

                //Se valida si el usuario tiene permiso a la acción principal
                if (!FacadeBR.ExisteAccion(this.dataContext, "CONSULTAR", seguridad))
                    this.ucvista.RedirigirSinPermisoAcceso();

                //Se valida si el usuario tiene permiso a la acción principal
                if (!FacadeBR.ExisteAccion(this.dataContext, "INSERTAR", seguridad))
                    this.ucvista.RedirigirSinPermisoAcceso();

                //Se valida si el usuario tiene permiso a la acción principal
                if (!FacadeBR.ExisteAccion(this.dataContext, "INSERTARCOMPLETO", seguridad))
                    this.ucvista.RedirigirSinPermisoAcceso();
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


        public void consultarActividadesPM()
        {
            if (this.ucvista.Modelo != null && this.ucvista.ModeloID != null)
            {
                var configuracionPosicionTrabajo = new ConfiguracionPosicionTrabajoBO()
                {
                    ConfiguracionModeloMotorizacion = new ConfiguracionModeloMotorizacionBO()
                    {
                        Modelo = new ModeloBO { Id = this.ucvista.ModeloID}
                    },
                    DescriptorTrabajo = new DescriptorTrabajoBO 
                    {
                        Nombre = this.vista.TipoMantenimiento.ToString().Trim()
                    }
                };

               var configPosicion = FacadeBR.ConsultarConfiguracionPosicionTrabajoDetalle(dataContext, configuracionPosicionTrabajo);

               if (configPosicion != null && configPosicion.Count > 0)
               {
                   var posicion = FacadeBR.ConsultarConfiguracionPosicionTrabajoDetallePaquete(dataContext, configPosicion.FirstOrDefault(),null);
                   var detalleConfiguracion = new List<DetalleConfiguracionAuditoriaMantenimientoBO>();
                   foreach (var item in posicion.ConfiguracionPosicionTrabajoPaquete)
                   {
                       var detalle = new DetalleConfiguracionAuditoriaMantenimientoBO()
                       {
                           ConfiguracionPosicionTrabajo = item.ConfiguracionPosicionTrabajo,
                           Obligatorio = false
                       };

                       detalleConfiguracion.Add(detalle);
                   }

                   this.vista.ActividadesAuditoria = detalleConfiguracion;
                   this.vista.GridActividadesAuditoria.PageIndex = 0;
                   this.vista.GridActividadesAuditoria.DataSource = this.vista.ActividadesAuditoria; 
                   this.vista.GridActividadesAuditoria.DataBind();
                   if (ucvista.TallerID != null)
                   {
                       this.ucvista.HabilitarSucursal(false);
                       this.ucvista.HabilitarTaller(false);
                   }
                   if (ucvista.ModeloID != null)
                       this.ucvista.HabilitarModelo(false);
               }
               else
               {
                   this.vista.MostrarMensaje("ESTE MODELO NO TIENE CONFIGURADO EN E-SERVICE UN PAQUETE DE ACTIVIDADES " + this.vista.TipoMantenimiento.ToString(), ETipoMensajeIU.INFORMACION);
               }

            }
            else
            {
                this.vista.MostrarMensaje("DEBE SELECCIONAR UN MODELO", ETipoMensajeIU.ADVERTENCIA);
            }
        }

        public void registrarConfiguracionesAuditoria()
        {
           
        }

        #endregion

        private ConfiguracionAuditoriaMantenimientoBO InterfazUsuarioAObjeto()
        {
            var bo = new ConfiguracionAuditoriaMantenimientoBO();
            bo.Sucursal.Id = this.ucvista.SucursalID;
            bo.Sucursal.Nombre = this.ucvista.SucursalNombre;
            bo.Taller.Id = this.ucvista.TallerID;
            bo.TipoMantenimiento = this.vista.TipoMantenimiento;
            bo.Modelo.Id = this.ucvista.ModeloID;
            bo.NumeroActividadesAleatorias = this.vista.Aleatorias;
            foreach (GridViewRow item in this.vista.GridActividadesAuditoria.Rows)
            {
                DetalleConfiguracionAuditoriaMantenimientoBO detalle = new DetalleConfiguracionAuditoriaMantenimientoBO();
                detalle.ConfiguracionPosicionTrabajo = new ConfiguracionPosicionTrabajoBO();

                Label NombreActividad = item.FindControl("lbActividadAuditoria") as Label;
                Label idActividad = item.FindControl("lbActividadID") as Label;
                TextBox txtCriterio = item.FindControl("txbCriterioActividad") as TextBox;
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

        private void ReiniciarInterfaz()
        {
            this.vista.LimpiarSession();
            this.ucvista.HabilitarModelo(true);
            this.ucvista.HabilitarSucursal(true);
            this.ucvista.HabilitarTaller(true);
            DataTable ds = new DataTable();
            ds = null;
            this.vista.GridActividadesAuditoria.DataSource = ds;
            this.vista.GridActividadesAuditoria.DataBind();
            this.vista.GridConfiguracionesAuditoria.DataSource = ds;
            this.vista.GridConfiguracionesAuditoria.DataBind();
        }
        
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

        public void agregarGrid()
        {
            if (this.vista.ActividadesAuditoria != null)
            {
                if (this.ucvista.TallerID != null)
                {
                    this.respaldarSelecciones();
                    var bo = this.InterfazUsuarioAObjeto();
                    bo.DetalleConfiguracion = this.vista.ActividadesAuditoria;
                    int num = bo.DetalleConfiguracion.Count(x => x.Obligatorio == false);

                    if (bo.NumeroActividadesAleatorias == null)
                    {
                        this.vista.MostrarMensaje("Debe establecer un numero de actividades aleatorias o ingrese 0 para nimguna", ETipoMensajeIU.ADVERTENCIA);
                    }
                    else if (bo.NumeroActividadesAleatorias > num)
                    {
                        this.vista.MostrarMensaje("El numero de actividades aleatorias no puede ser mayor que el numero de actividades no obligatorias :" + num.ToString(), ETipoMensajeIU.ADVERTENCIA);
                    }
                    else
                    {
                        if (this.vista.ConfiguracionesAuditoria == null)
                        {
                            this.vista.ConfiguracionesAuditoria = new List<ConfiguracionAuditoriaMantenimientoBO>();
                            this.vista.ConfiguracionesAuditoria.Add(bo);
                            this.vista.GridConfiguracionesAuditoria.DataSource = this.vista.ConfiguracionesAuditoria;
                            this.vista.GridConfiguracionesAuditoria.DataBind();
                            this.vista.Aleatorias = null;
                            if (this.ucvista.TallerID != null)
                            {
                                this.ucvista.HabilitarTaller(false);
                                this.ucvista.HabilitarSucursal(false);
                            }
                            
                        }
                        else
                        {
                            if (this.vista.ConfiguracionesAuditoria.Exists(x => x.TipoMantenimiento == bo.TipoMantenimiento))
                            {
                                this.vista.MostrarMensaje("No puede agregar dos configuraciones para el servicio: " + bo.TipoMantenimiento.ToString(), ETipoMensajeIU.ADVERTENCIA);
                            }
                            else
                            {
                                this.vista.ConfiguracionesAuditoria.Add(bo);
                                this.vista.GridConfiguracionesAuditoria.DataSource = this.vista.ConfiguracionesAuditoria;
                                this.vista.GridConfiguracionesAuditoria.DataBind();
                                this.vista.Aleatorias = null;
                                if (this.ucvista.TallerID != null)
                                {
                                    this.ucvista.HabilitarTaller(false);
                                    this.ucvista.HabilitarSucursal(false);
                                }

                            }
                        }
                    }
                }
                else
                {
                    this.vista.MostrarMensaje("DEBE SELECCIONAR UN TALLER",ETipoMensajeIU.ADVERTENCIA);
                }
                             
            }           
        }

        public int guardarConfiguraciones()
        {
            int exito = 0;
            if (this.vista.ConfiguracionesAuditoria != null && this.vista.ConfiguracionesAuditoria.Count > 0)
            {
                var filtro = new ConfiguracionAuditoriaMantenimientoBO()
                {
                    Sucursal = new SucursalBO { Id = this.vista.ConfiguracionesAuditoria.FirstOrDefault().Sucursal.Id},
                    Taller = new TallerBO { Id = this.vista.ConfiguracionesAuditoria.FirstOrDefault().Taller.Id },
                    Modelo = new ModeloBO { Id = this.vista.ConfiguracionesAuditoria.FirstOrDefault().Modelo.Id },
                    Activo = true
                };

                var configActuales = ctrConfiguracionAuditoria.Consultar(dataContext, filtro);
                if (configActuales != null && configActuales.Count > 0)
                {
                    string seriviciosExistentes = null;
                    bool exist = false;
                    foreach (var item in configActuales)
                    {
                        if (this.vista.ConfiguracionesAuditoria.Exists(x => x.TipoMantenimiento == item.TipoMantenimiento) == true)
                        {
                            var existencia = this.vista.ConfiguracionesAuditoria.Find(x => x.TipoMantenimiento == item.TipoMantenimiento);
                            seriviciosExistentes += ","+existencia.TipoMantenimiento.ToString();
                            existencia.Activo = false;
                        }
                    }
                    exist = this.vista.ConfiguracionesAuditoria.Exists(x => x.Activo == false);
                    if (exist == true)
                    {
                        this.vista.ConfiguracionesAuditoria.RemoveAll(x => x.Activo == false);
                        this.vista.GridConfiguracionesAuditoria.DataSource = this.vista.ConfiguracionesAuditoria;
                        this.vista.GridConfiguracionesAuditoria.DataBind();
                        this.vista.MostrarMensaje("El Modelo, Taller y Sucursal ya tienen configurados los siguientes servicios :" + seriviciosExistentes, ETipoMensajeIU.ADVERTENCIA);
                    }
                    else
                    {
                        foreach (var item in  this.vista.ConfiguracionesAuditoria)
                        {
                            var seguridad = this.obtenerUsuario();
                            ctrConfiguracionAuditoria.Insertar(dataContext, item, seguridad);
                            exito = 1;
                        }
 
                    }
                }
                else
                {
                    foreach (var item in this.vista.ConfiguracionesAuditoria)
                    {
                        var seguridad = this.obtenerUsuario();
                        ctrConfiguracionAuditoria.Insertar(dataContext, item, seguridad);
                        exito = 1;
                    }
                }
            }
            else
            {
                this.vista.MostrarMensaje("debe agregar una configuracion para guardar",ETipoMensajeIU.ADVERTENCIA);
            }

            return exito;
        }

        public ConfiguracionAuditoriaMantenimientoBO consultarGuardado()
        {
            var bo = this.vista.ConfiguracionesAuditoria.FirstOrDefault();
            bo.NumeroActividadesAleatorias = null;
            bo.Activo = true;
            bo = ctrConfiguracionAuditoria.ConsultarEncabezado(dataContext, bo).FirstOrDefault();
            return bo;
        }
    }
}
