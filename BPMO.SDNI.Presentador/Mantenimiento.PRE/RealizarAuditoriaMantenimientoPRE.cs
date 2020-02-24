//Satisface al CU020 - Imprimir Auditoria Realizada

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using BPMO.Basicos.BO;
using BPMO.Facade.SDNI.BR;
using BPMO.Patterns.Creational.DataContext;
using BPMO.Primitivos.Enumeradores;
using BPMO.SDNI.Mantenimiento.BO;
using BPMO.SDNI.Mantenimiento.BR;
using BPMO.SDNI.Mantenimiento.Reportes.BR;
using BPMO.SDNI.Mantenimiento.VIS;
using BPMO.Servicio.Catalogos.BO;
using BPMO.Servicio.Procesos.BO;

namespace BPMO.SDNI.Mantenimiento.PRE
{
    public class RealizarAuditoriaMantenimientoPRE
    {
        #region Propiedades
        private IRealizarAuditoriaMantenimientoVIS vista;
        private IDataContext dataContext = null;
        private MantenimientoUnidadBR ctrMantenimientoUnidad;
        private MantenimientoEquipoAliadoBR ctrMantenimientoEquipoAliado;
        private AuditoriaMantenimientoBR ctrAuditoriaMantenimiento;
        private ConfiguracionAuditoriaMantenimientoBR ctrConfiguracionAuditoria;
        private string nombreClase = typeof(RealizarAuditoriaMantenimientoPRE).Name;        
        #endregion

        #region Constructores
        /// <summary>
        /// Inicializa los controladores y vista
        /// </summary>
        /// <param name="vista"></param>
        public RealizarAuditoriaMantenimientoPRE(IRealizarAuditoriaMantenimientoVIS vista)
        {
            try
            {
                this.vista = vista;
                this.ctrMantenimientoUnidad = new MantenimientoUnidadBR();
                this.ctrMantenimientoEquipoAliado = new MantenimientoEquipoAliadoBR();
                this.ctrAuditoriaMantenimiento = new AuditoriaMantenimientoBR();
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

        #region Métodos

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
                if (!FacadeBR.ExisteAccion(this.dataContext, "UI INSERTAR", seguridad))
                    this.vista.RedirigirSinPermisoAcceso();

                //Se valida si el usuario tiene permiso a la acción principal
                if (!FacadeBR.ExisteAccion(this.dataContext, "INSERTAR", seguridad))
                    this.vista.RedirigirSinPermisoAcceso();

                //Se valida si el usuario tiene permiso a la acción principal
                if (!FacadeBR.ExisteAccion(this.dataContext, "INSERTARCOMPLETO", seguridad))
                    this.vista.RedirigirSinPermisoAcceso();

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

        #region Consultar
        /// <summary>
        /// Obtiene el maantenimiento que se va a auditar
        /// </summary>
        public void Consultar()
        {
            try
            {
                AuditoriaMantenimientoBO bo = new AuditoriaMantenimientoBO { OrdenServicio = this.vista.MantenimientoRecibido };
                MantenimientoUnidadBO mantenimientoUnidad = new MantenimientoUnidadBO() { OrdenServicio = bo.OrdenServicio };
                MantenimientoEquipoAliadoBO mantenimientoAliado = new MantenimientoEquipoAliadoBO() { OrdenServicio = bo.OrdenServicio };
                List<MantenimientoUnidadBO> listMantenimientosUnidad = ctrMantenimientoUnidad.Consultar(dataContext, mantenimientoUnidad);
                if (listMantenimientosUnidad == null || listMantenimientosUnidad.Count == 0)
                {
                    List<MantenimientoEquipoAliadoBO> listMantenimientoAliado = ctrMantenimientoEquipoAliado.Consultar(dataContext, mantenimientoAliado);
                    if (listMantenimientoAliado != null && listMantenimientoAliado.Count > 0)
                        bo.TipoMantenimiento = (ETipoMantenimiento)listMantenimientoAliado.FirstOrDefault().TipoMantenimiento;
                    else
                    {
                        this.vista.MostrarMensaje("La búsqueda no produjo resultados", ETipoMensajeIU.INFORMACION,
                            "No se encontraron coincidencias con los criterios ingresados. Verifique sus datos");
                    }
                }
                else
                {
                    bo.TipoMantenimiento = (ETipoMantenimiento)listMantenimientosUnidad.FirstOrDefault().TipoMantenimiento;
                }
                bo = this.ComplementarDatos(bo);
                this.vista.Resultado = bo;
                this.DesplegarResultado(bo);
            }
            catch (Exception ex)
            {
                throw new Exception(this.nombreClase + ".Consultar:" + ex.Message);
            }
        }
        #endregion

        #region Complementar Datos
        /// <summary>
        /// Complementa los datos del mantenimiento para la crearcion del objeto de auditoria 
        /// </summary>
        /// <param name="AuditoriaMantenimiento"></param>
        /// <returns></returns>
        private AuditoriaMantenimientoBO ComplementarDatos(AuditoriaMantenimientoBO AuditoriaMantenimiento)
        {
            AuditoriaMantenimiento = this.ctrAuditoriaMantenimiento.ComplementarAuditoria(dataContext, AuditoriaMantenimiento);
            if (AuditoriaMantenimiento.OrdenServicio.Unidad.ConfiguracionModeloMotorizacion.Modelo != null)
            {
                ConfiguracionAuditoriaMantenimientoBO configuracionAuditoria = new ConfiguracionAuditoriaMantenimientoBO()
                {
                    Sucursal = AuditoriaMantenimiento.OrdenServicio.AdscripcionServicio.Sucursal,
                    Modelo = AuditoriaMantenimiento.OrdenServicio.Unidad.ConfiguracionModeloMotorizacion.Modelo,
                    Taller = AuditoriaMantenimiento.OrdenServicio.AdscripcionServicio.Taller,
                    TipoMantenimiento = AuditoriaMantenimiento.TipoMantenimiento
                };
                configuracionAuditoria = this.ctrConfiguracionAuditoria.Consultar(dataContext, configuracionAuditoria).FirstOrDefault();
                if (configuracionAuditoria != null)
                {
                    foreach (var item in AuditoriaMantenimiento.DetalleAuditoria)
                    {
                        var DetalleConfig = configuracionAuditoria.DetalleConfiguracion.Find(x => x.ConfiguracionPosicionTrabajo.Id == item.PosicionTrabajoPaquete.ConfiguracionPosicionTrabajo.Id);

                        if (DetalleConfig != null)
                        {
                            item.Criterio = DetalleConfig.Criterio;
                        }
                    }
                    List<DetalleAuditoriaMantenimientoBO> listaPosicionesAleatorias = new List<DetalleAuditoriaMantenimientoBO>();
                    List<DetalleAuditoriaMantenimientoBO> DetalleObligatorias = new List<DetalleAuditoriaMantenimientoBO>();
                    foreach (var item in AuditoriaMantenimiento.DetalleAuditoria)
                    {
                        if (configuracionAuditoria.DetalleConfiguracion.Exists(x => x.ConfiguracionPosicionTrabajo.Id == item.PosicionTrabajoPaquete.ConfiguracionPosicionTrabajo.Id && x.Obligatorio == false) == true)
                        {
                            listaPosicionesAleatorias.Add(item);

                        }
                        else if (configuracionAuditoria.DetalleConfiguracion.Exists(x => x.ConfiguracionPosicionTrabajo.Id == item.PosicionTrabajoPaquete.ConfiguracionPosicionTrabajo.Id && x.Obligatorio == true) == true)
                        {
                            DetalleObligatorias.Add(item);
                        }
                    }
                    AuditoriaMantenimiento.DetalleAuditoria = DetalleObligatorias;
                    if (listaPosicionesAleatorias != null && listaPosicionesAleatorias.Count > 0)
                    {
                        Random IndexRandom = new Random();
                        int indexMaximo = listaPosicionesAleatorias.Count;
                        int iteraciones = (int)configuracionAuditoria.NumeroActividadesAleatorias;
                        for (int i = 0; i < iteraciones; i++)
                        {
                            bool bandera;
                            int index;
                            do
                            {
                                index = IndexRandom.Next(0, indexMaximo);
                                bandera = AuditoriaMantenimiento.DetalleAuditoria.Exists(x => x.PosicionTrabajoPaquete.ConfiguracionPosicionTrabajo.Id == listaPosicionesAleatorias[index].PosicionTrabajoPaquete.ConfiguracionPosicionTrabajo.Id);

                            } while (bandera == true);

                            AuditoriaMantenimiento.DetalleAuditoria.Add(listaPosicionesAleatorias[index]);
                        }
                    }
                }
                else
                {
                   
                    this.vista.MostrarMensaje("No se encontro una configuracion de auditoria para el modelo, taller y tipo de mantenimiento de esta unidad", ETipoMensajeIU.ERROR);
                }

            }
            else
            {
                this.vista.MostrarMensaje("No se encontro la unidad de esta orden de servicio", ETipoMensajeIU.ERROR);

            }

            return AuditoriaMantenimiento;
        }
        #endregion

        #region Desplegar Resultado
        /// <summary>
        /// Muestra el objeto Auditoria en la interfaz
        /// </summary>
        /// <param name="resultado"></param>
        private void DesplegarResultado(AuditoriaMantenimientoBO resultado)
        {
            this.vista.OrdenServicioID = resultado.OrdenServicio.Id;
            this.vista.TipoMantenimiento = resultado.TipoMantenimiento.ToString();
            this.vista.Tecnicos = resultado.Tecnicos;
            this.vista.DetalleAuditoria = resultado.DetalleAuditoria;
            this.vista.ActividadesAuditoria.DataSource = resultado.DetalleAuditoria;
            this.vista.ActividadesAuditoria.DataBind();
        }
        #endregion

        #region InterfazUsuario a Dato
        /// <summary>
        /// Mapea la interfaz de usuario al Objeto Auditoria 
        /// </summary>
        /// <returns></returns>
        private AuditoriaMantenimientoBO InterfazUsuarioADato()
        {
            if (this.vista.MantenimientoRecibido == null) throw new Exception(".InterfazUsuarioADato: El folio de la orden de servicio no puede ser nulo");
            AuditoriaMantenimientoBO bo = new AuditoriaMantenimientoBO();
            bo.EvidenciaMantenimiento = new EvidenciaAuditoriaMantenimientoBO();
            bo.OrdenServicio = this.vista.Resultado.OrdenServicio;
            if (this.vista.TipoMantenimiento != null)
            {
                if (this.vista.TipoMantenimiento.Trim() == ETipoMantenimiento.PMA.ToString())
                    bo.TipoMantenimiento = ETipoMantenimiento.PMA;
                if (this.vista.TipoMantenimiento.Trim() == ETipoMantenimiento.PMB.ToString())
                    bo.TipoMantenimiento = ETipoMantenimiento.PMB;
                if (this.vista.TipoMantenimiento.Trim() == ETipoMantenimiento.PMC.ToString())
                    bo.TipoMantenimiento = ETipoMantenimiento.PMC;
            }
            if (this.vista.Tecnicos != null && this.vista.Tecnicos.Count > 0)
                bo.Tecnicos = this.vista.Tecnicos;

            foreach (GridViewRow item in this.vista.ActividadesAuditoria.Rows)
            {
                DetalleAuditoriaMantenimientoBO detalle = new DetalleAuditoriaMantenimientoBO();
                detalle.PosicionTrabajoPaquete = new PosicionTrabajoPaqueteBO();
                detalle.PosicionTrabajoPaquete.ConfiguracionPosicionTrabajo = new ConfiguracionPosicionTrabajoBO();
                detalle.PosicionTrabajoPaquete.ConfiguracionPosicionTrabajo.DescriptorTrabajo = new DescriptorTrabajoBO();
                Label NombreActividad = item.FindControl("lbActividadAuditoria") as Label;
                Label idActividad = item.FindControl("lbActividadID") as Label;
                Label lblCriterio = item.FindControl("lbCriterioAuditoria") as Label;
                CheckBox chbxSatifactorio = item.FindControl("chbxSatisfactorio") as CheckBox;
                CheckBox chbxReparar = item.FindControl("chbxReparar") as CheckBox;
                CheckBox chbxAjustado = item.FindControl("chbxAjustado") as CheckBox;
                TextBox comentarioactividad = item.FindControl("txbComentarioActividad") as TextBox;
                if (chbxSatifactorio.Checked == true)
                    detalle.ResultadoAuditoria = EResultadoAuditoria.Satisfactoria;
                if (chbxReparar.Checked == true)
                    detalle.ResultadoAuditoria = EResultadoAuditoria.Reparar;
                if (chbxAjustado.Checked == true)
                    detalle.ResultadoAuditoria = EResultadoAuditoria.Ajustado;
                detalle.Comentarios = comentarioactividad.Text.ToUpper();
                detalle.Criterio = lblCriterio.Text.ToUpper();
                detalle.PosicionTrabajoPaquete.ConfiguracionPosicionTrabajo.DescriptorTrabajo.Nombre = NombreActividad.Text;
                detalle.PosicionTrabajoPaquete.ConfiguracionPosicionTrabajo.Id = int.Parse(idActividad.Text);
                bo.DetalleAuditoria.Add(detalle);
                bo.EvidenciaMantenimiento.DocumentoEvidencia = this.vista.Evidencia.HasFile ? this.vista.Evidencia.FileBytes : null;
                bo.EvidenciaMantenimiento.Nombre = this.vista.Evidencia.HasFile ? this.vista.Evidencia.FileName : string.Empty; ;
                bo.Observaciones = this.vista.Observaciones.ToUpper();
            }

            return bo;
        }

        #endregion

        #region Guardar Auditoria
        /// <summary>
        /// llama al AuditoriaBR para guardar la auditoria realizada y despliega un mensaje de error si no se completo el guardado
        /// </summary>
        /// <returns></returns>
        public int GuardarAuditoria()
        {
            AuditoriaMantenimientoBO AuditoriaRecibida = this.InterfazUsuarioADato();
            int error = 0;
            int formatoCorrecto = 0;
            bool resultado = AuditoriaRecibida.DetalleAuditoria.Exists(x => x.ResultadoAuditoria == 0);
            List<bool> existFormat = new List<bool>();
            List<string> formatos = this.vista.ObtenerConfiguracionFormatos();

            if (AuditoriaRecibida.EvidenciaMantenimiento.DocumentoEvidencia != null)
            {
                foreach (var item in formatos)
                {
                    bool format = this.vista.Evidencia.PostedFile.FileName.EndsWith(item);
                    existFormat.Add(format);
                };               
            }
            if (existFormat.Exists(x => x == true))
                formatoCorrecto = 1;	

            if (resultado == true)
            {
                error = 1;

            }
            else if (AuditoriaRecibida.EvidenciaMantenimiento.DocumentoEvidencia == null)
            {
                error = 2;

            }
            else if (AuditoriaRecibida.Observaciones == string.Empty)
            {
                error = 3;
            }
            else if (formatoCorrecto == 0)
            {
                error = 4;
            }
            else
            {
                SeguridadBO seguridad = this.obtenerUsuario();
                try
                {
                    new AuditoriaMantenimientoBR().Insertar(dataContext, AuditoriaRecibida, seguridad);
                }
                catch (Exception)
                {

                    throw;
                }

            }

            return error;
        }

        #endregion

        #region Imprimir Auditoria
        
        /// <summary>
        /// Metodo para imprimir la auditoria
        /// </summary>
        public void ImprimirAuditoria()
        {
            AuditoriaMantenimientoBO auditoriaRecibida = this.InterfazUsuarioADato();

            if (this.ValidarCampos(auditoriaRecibida))
            {
                Dictionary<string, object> datosReporte = new ImprimirAuditoriaRealizadaBR().EstablecerDatosReporte(dataContext, auditoriaRecibida);

                this.vista.EstablecerValoresImpresion("PLEN.BEP.15.MODMTTO.CU020", datosReporte);

                this.vista.RedirigirAImprimir();

            }
        }

        /// <summary>
        /// Verifica que la auditoria este completa para poder imprimir
        /// </summary>
        /// <param name="auditoriaRecibida"></param>
        /// <returns></returns>
        private bool ValidarCampos(AuditoriaMantenimientoBO auditoriaRecibida, bool esGuardado = false)
        {
            int error = 0;

            bool resultado = auditoriaRecibida.DetalleAuditoria.Exists(x => x.ResultadoAuditoria == 0);

            if (resultado)
            {
                error = 1;
            }
            if (esGuardado && auditoriaRecibida.EvidenciaMantenimiento.DocumentoEvidencia == null)
            {
                error = 2;
            }
            if (string.IsNullOrEmpty(auditoriaRecibida.Observaciones))
            {
                error = 3;
            }

            switch (error)
            {
                case 1:
                    this.vista.MostrarMensaje("Todo las actividades de la auditoria deben tener una calificación", ETipoMensajeIU.ADVERTENCIA);
                    break;
                case 2:
                    this.vista.MostrarMensaje("Debe adjuntar un documento de evidencia a la auditoria", ETipoMensajeIU.ADVERTENCIA);
                    break;
                case 3:
                    this.vista.MostrarMensaje("Debe llenar el campo de observaciones de auditoria", ETipoMensajeIU.ADVERTENCIA);
                    break;
            }

            return error == 0;
        }
        #endregion

        #endregion
    }
}
