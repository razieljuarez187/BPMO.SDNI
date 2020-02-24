//Satisface al caso de uso CU071 - Reporte de Auditorias Realizadas
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using BPMO.Basicos.BO;
using BPMO.Facade.SDNI.BOF;
using BPMO.Facade.SDNI.BR;
using BPMO.Patterns.Creational.DataContext;
using BPMO.Primitivos.Enumeradores;
using BPMO.SDNI.Mantenimiento.Reportes.BR;
using BPMO.SDNI.Mantenimiento.Reportes.VIS;

namespace BPMO.SDNI.Mantenimientos.Reportes.PRE
{
    /// <summary>
    /// Presentador utilizado para el Reporte de auditorias realizadas
    /// </summary>
    public class ReporteAuditoriasRealizadasPRE
    {
        #region Atributos
        /// <summary>
        /// El DataContext que proveerá acceso a la base de datos
        /// </summary>
        private IDataContext dataContext;
        /// <summary>
        /// Nombre de la clase para agregar a los mensajes de Error
        /// </summary>
        private string nombreClase = typeof(ReporteAuditoriasRealizadasPRE).Name;
        /// <summary>
        /// Vista que se esta gestionando
        /// </summary>
        IReporteAuditoriasRealizadasVIS vista;
        /// <summary>
        /// Controlador del reporte general de renta diaria
        /// </summary>
        private ReporteAuditoriasRealizadasBR controlador;
        #endregion

        #region Constructor
        /// <summary>
        /// Constructod del reporte de auditorias Realizadas
        /// </summary>
        /// <param name="vista"></param>
        public ReporteAuditoriasRealizadasPRE(IReporteAuditoriasRealizadasVIS vista)
        {
            try
            {
                this.vista = vista;
                this.dataContext = FacadeBR.ObtenerConexion();
                this.controlador = new ReporteAuditoriasRealizadasBR();
            }
            catch (Exception ex)
            {
                this.vista.MostrarMensaje("Inconsistencias en la configuracion", ETipoMensajeIU.ERROR, nombreClase + ex.Message);
            }
        } 
        #endregion

        #region Metodos
        public void PreparaConsulta()
        {

        }

        public void Consultar()
        {
            Hashtable parameters = new Hashtable();
            parameters["UnidadOperativaID"] = this.vista.UnidadOperativaID;
            parameters["ModuloID"] = this.vista.ModuloID;
            if(this.vista.FechaInicio != null)
                parameters["FechaInicio"] = this.vista.FechaInicio;
            if(this.vista.FechaFin != null)
                parameters["FechaFin"] = this.vista.FechaFin;
            
            if (this.vista.SucursalID.HasValue)
                parameters["SucursalID"] = new Int32[] { this.vista.SucursalID.Value };
            else
            {
                SucursalBOF sucursal = new SucursalBOF();
                sucursal.UnidadOperativa = new UnidadOperativaBO { Id = this.vista.UnidadOperativaID };
                sucursal.Usuario = new UsuarioBO { Id = this.vista.UsuarioID };
                sucursal.Activo = true;

                List<SucursalBOF> sucursalesPermitidas = FacadeBR.ConsultarSucursalesSeguridad(this.dataContext, sucursal);
                if (sucursalesPermitidas.Count > 0)
                {
                    parameters["SucursalID"] = sucursalesPermitidas
                                                .Select(x => x.Id.Value)
                                                .ToArray();
                }
                else //Sino tiene sucursales asignadas al usuario en curso se manda una sucursal no existente
                    parameters["SucursalID"] = new Int32[] { -1000 };
            }
            if (this.vista.TecnicoID != null)
                parameters["TecnicoID"] = this.vista.TecnicoID;

            Dictionary<String, Object> reportParameters = this.controlador.ConsultarReporteAuditoriasRealizadas(this.dataContext, parameters);
            if (!reportParameters.ContainsKey("Count"))
            {
                this.vista.MostrarMensaje("No se encontraron coincidencias con los filtros especificados", ETipoMensajeIU.ADVERTENCIA);
                return;
            }
            else if ((reportParameters["Count"] as Int32?) == 0)
            {
                this.vista.MostrarMensaje("No se encontraron coincidencias con los filtros especificados", ETipoMensajeIU.ADVERTENCIA);
                return;
            }

            this.vista.EstablecerPaqueteNavegacionImprimir("PLEN.BEP.15.MODMTTO.CU071", reportParameters);
            this.vista.IrAImprimir();
        }

        /// <summary>
        /// Crea un objeto de seguridad con los datos actuales de la sesión de usuario en curso
        /// </summary>
        /// <returns>Objeto de tipo seguridad</returns>
        private SeguridadBO CrearObjetoSeguridad()
        {
            UsuarioBO usuario = new UsuarioBO() { Id = this.vista.UsuarioID };
            AdscripcionBO adscripcion = new AdscripcionBO() { UnidadOperativa = new UnidadOperativaBO() { Id = this.vista.UnidadOperativaID } };
            SeguridadBO seguridadBO = new SeguridadBO(Guid.Empty, usuario, adscripcion);

            return seguridadBO;
        }

        /// <summary>
        /// Valida el acceso a la sección en curso
        /// </summary>
        public void ValidarAcceso()
        {
            try
            {
                //Valida que usuario y la unidad operativa no sean nulos
                if (this.vista.UsuarioID == null) throw new Exception("El identificador del usuario no debe ser nulo");
                if (this.vista.UnidadOperativaID == null) throw new Exception("El identificador de la Unidad Operativa no debe ser nulo");
                //Se crea el objeto de seguridad                
                SeguridadBO seguridadBO = this.CrearObjetoSeguridad();

                //Se valida si el usuario tiene permiso a la acción principal
                if (!FacadeBR.ExisteAccion(this.dataContext, "CONSULTAR", seguridadBO))
                    this.vista.RedirigirSinPermisoAcceso();
            }
            catch (Exception ex)
            {
                throw new Exception(nombreClase + ".ValidarAcceso: " + ex.GetBaseException().Message);
            }
        } 
        #endregion
    }
}
