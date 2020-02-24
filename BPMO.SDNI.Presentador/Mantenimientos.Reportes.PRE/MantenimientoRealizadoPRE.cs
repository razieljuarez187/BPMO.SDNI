using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections;
using BPMO.Basicos.BO;
using BPMO.Facade.SDNI.BO;
using BPMO.Facade.SDNI.BOF;
using BPMO.Servicio.Catalogos.BO;
using BPMO.SDNI.Comun.BO;
using BPMO.SDNI.Comun.BR;
using BPMO.Primitivos.Enumeradores;
using BPMO.SDNI.Mantenimiento.Reportes.VIS;
using BPMO.Patterns.Creational.DataContext;
using BPMO.Facade.SDNI.BR;
using BPMO.SDNI.Mantenimiento.Reportes.BR;
using BPMO.SDNI.Mantenimiento.Reportes.DA;

namespace BPMO.SDNI.Mantenimientos.Reportes.PRE
{
    public class MantenimientoRealizadoPRE
    {
        
        #region Propiedades
        private IMantenimientoRealizadoVIS vista;
        private ReporteMantenimientosRealizadosBR ctrReporteMantenimiento;
        private IDataContext dataContext = null;
        private string nombreClase;
        #endregion

        public MantenimientoRealizadoPRE(IMantenimientoRealizadoVIS vista)
        {
            try
            {
                this.vista = vista;
                this.nombreClase = this.GetType().Name;
                this.ctrReporteMantenimiento = new ReporteMantenimientosRealizadosBR();
                this.dataContext = FacadeBR.ObtenerConexion();
               

            }
            catch (Exception)
            {
            }
        }

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

        #region Metodos Privados
        private void CrearTalleres(SucursalBO sucursal)
        {
            List<TallerBO> talleres = FacadeBR.ConsultarTalleresPorSucursal(dataContext, sucursal);
            this.vista.enlazarControles(talleres);
        }
        #endregion

        #region Metodos buscador

        public object PrepararBOBuscador(string catalogo)
        {
            object obj = null;

            switch (catalogo)
            {
                case "Unidad":
                    BPMO.SDNI.Equipos.BO.UnidadBO unidad = new BPMO.SDNI.Equipos.BO.UnidadBO();
                    unidad.NumeroSerie = this.vista.NumeroVIN;
                    obj = unidad;
                    break;

                case "Sucursal":
                    SucursalBOF sucursal = new SucursalBOF();
                    sucursal.Nombre = this.vista.SucursalNombre;
                    sucursal.UnidadOperativa = new UnidadOperativaBO { Id = this.vista.UnidadOperativaId };
                    sucursal.Usuario = new UsuarioBO { Id = this.vista.UsuarioAutenticado };
                    obj = sucursal;
                    break;
            }

            return obj;
        }

        public void DesplegarResultadoBuscador(string catalogo, object selecto)
        {
            switch (catalogo)
            {
                case "Unidad":
                    BPMO.SDNI.Equipos.BO.UnidadBO unidad = (BPMO.SDNI.Equipos.BO.UnidadBO)selecto;

                    if (unidad != null && unidad.UnidadID != null)
                    {
                        this.vista.NumeroVIN = unidad.NumeroSerie;
                        this.vista.UnidadID = unidad.UnidadID;

                    }
                    else
                        this.vista.NumeroVIN = null;

                    break;
                case "Sucursal":
                    SucursalBO sucursal = (SucursalBO)selecto;

                    if (sucursal != null && sucursal.Id != null)
                    {
                        this.vista.SucursalNombre = sucursal.Nombre;
                        this.vista.SucursalID = sucursal.Id;
                        this.CrearTalleres(sucursal);
                    }
                    else
                        this.vista.SucursalNombre = null;

                    break;
  

            }
        }

        /// <summary>
        /// Realiza la consulta para el reporte
        /// </summary>
        public void Consultar()
        {
            if (this.vista.SucursalID != null)
            {
                if (this.vista.TallerID == null)
                {
                    this.vista.MostrarMensaje("Se debe seleccionar un Taller", ETipoMensajeIU.ADVERTENCIA);
                    return;
                }
            }
            if (this.vista.FechaInicio == null || this.vista.FechaFin == null)
            {
                this.vista.MostrarMensaje("Se debe seleccionar el rango de fechas", ETipoMensajeIU.ADVERTENCIA);
                return;
            }

            Hashtable parameters = new Hashtable();
            parameters["SucursalID"] = this.vista.SucursalID;
            parameters["TallerID"] = this.vista.TallerID;
            parameters["UnidadID"] = this.vista.UnidadID;
            if (this.vista.Departamento != null)
               parameters["AreaID"] = (int)this.vista.Departamento.Value;
            parameters["FechaInicio"] = this.vista.FechaInicio;
            parameters["FechaFin"] = this.vista.FechaFin;
            parameters["UnidadOperativaID"] = this.vista.UnidadOperativaID;
            parameters["ModuloID"] = this.vista.ModuloID;
            parameters["NombreSucursal"] = this.vista.SucursalNombre;
            parameters["VIN"] = this.vista.NumeroVIN;

            DataSet resultado = this.ctrReporteMantenimiento.ConsultarMantenimientos(dataContext, parameters);
            if (resultado.Tables["Unidad"].Rows.Count > 0)
            {
                Dictionary<String, Object> reportParameters = new Dictionary<String, Object>();
                reportParameters["DataSource"] = resultado;
                this.vista.EstablecerPaqueteNavegacionImprimir(this.vista.IdentificadorReporte, reportParameters);
                this.vista.IrAImprimir();

                return;
            }
            else
            {
                this.vista.MostrarMensaje("No se encontraron resultatos con los parametros seleccionados", ETipoMensajeIU.ADVERTENCIA);
            }
        }

        #endregion

        public void ConsultarReporte()
        {
            Hashtable parameters = new Hashtable();
            parameters["SucursalID"] = this.vista.SucursalID;
            parameters["TallerID"] = this.vista.TallerID;
            parameters["UnidadID"] = this.vista.UnidadID;
            parameters["AreaID"] = (int)this.vista.Departamento.Value;
            parameters["FechaInicio"] = this.vista.FechaInicio;
            parameters["FechaFin"] = this.vista.FechaFin;

            DataSet resultado = this.ctrReporteMantenimiento.ConsultarMantenimientos(dataContext, parameters);
            Dictionary<String, Object> reportParameters = new Dictionary<String, Object>();
            reportParameters["DataSource"] = resultado;
            this.vista.EstablecerPaqueteNavegacionImprimir(this.vista.IdentificadorReporte, reportParameters);
            this.vista.IrAImprimir();

        }

        #endregion
    }
}
