///Satisface el caso de uso CU070 - Configurar Parametros Auditoria
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BPMO.SDNI.Mantenimiento.VIS;
using BPMO.Patterns.Creational.DataContext;
using BPMO.Facade.SDNI.BR;
using BPMO.Primitivos.Enumeradores;
using BPMO.SDNI.Mantenimiento.BR;
using BPMO.SDNI.Mantenimiento.BO;
using BPMO.Basicos.BO;

namespace BPMO.SDNI.Mantenimiento.PRE
{
    public class ConsultarConfiguracionParametrosAuditoriaPRE
    {
        #region Propiedades
        private string nombreClase;
        private IConsultarConfiguracionParametrosAuditoriaVIS vista;
        private IucConfiguracionParametrosAuditoriaVIS ucvista;
        private ConfiguracionAuditoriaMantenimientoBR ctrConfiguracionAuditoria;
        private IDataContext dataContext = null;
        #endregion

        #region Constructores
        
        public ConsultarConfiguracionParametrosAuditoriaPRE(IConsultarConfiguracionParametrosAuditoriaVIS vista, IucConfiguracionParametrosAuditoriaVIS ucvista)
        {
            try
            {
                this.vista = vista;
                this.ucvista = ucvista;
                this.nombreClase = GetType().Name;
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
                if (this.ucvista.UsuarioAutenticado == null) throw new Exception("El identificador del usuario no debe ser nulo");
                if (this.ucvista.UnidadOperativaId == null) throw new Exception("El identificador de la Unidad Operativa no debe ser nulo");
                //Se crea el objeto de seguridad
                SeguridadBO seguridad = this.CrearObjetoSeguridad();

                //Se valida si el usuario tiene permiso a la acción principal
                if (!FacadeBR.ExisteAccion(this.dataContext, "CONSULTAR", seguridad))
                    this.ucvista.RedirigirSinPermisoAcceso();

                //Se valida si el usuario tiene permiso a la acción principal
                if (!FacadeBR.ExisteAccion(this.dataContext, "UI CONSULTAR", seguridad))
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

        /// <summary>
        /// Mapea los valores de la interfaz a un objeto
        /// </summary>
        /// <returns></returns>
        private ConfiguracionAuditoriaMantenimientoBO InterfazAObjeto()
        {
            var bo = new ConfiguracionAuditoriaMantenimientoBO();
                bo.Sucursal.Nombre = this.ucvista.SucursalNombre;
                if (bo.Sucursal.Nombre != string.Empty)
                bo.Sucursal.Id = this.ucvista.SucursalID;
                bo.Taller.Id = this.ucvista.TallerID;
                bo.Modelo.Nombre = this.ucvista.Modelo;
                if (bo.Modelo.Nombre != string.Empty)
                bo.Modelo.Id = this.ucvista.ModeloID;
                bo.Activo = this.vista.Estatus;
            return bo;
 
        }

        /// <summary>
        /// Busca las configuraciones y las despliega en el Grid
        /// </summary>
        public void buscarConfiguracionesAuditoria()
        {
            var bo = this.InterfazAObjeto();
            var list = ctrConfiguracionAuditoria.ConsultarEncabezado(dataContext, bo);
            if (list != null && list.Count > 0)
            {
                this.vista.ConfiguracionesAuditoria = list;
                this.vista.GridConfiguracines.DataSource = this.vista.ConfiguracionesAuditoria;
                this.vista.GridConfiguracines.DataBind();
            }
            else
            {
                this.vista.MostrarMensaje("La busqueda no produjo resultados", ETipoMensajeIU.INFORMACION);
            }
        }

        #endregion

    }
}
