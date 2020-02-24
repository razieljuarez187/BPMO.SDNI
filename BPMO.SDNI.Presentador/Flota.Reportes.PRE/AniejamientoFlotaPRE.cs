//Satisface al CU030 - Reporte de Añejamiento de Flota
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using BPMO.Basicos.BO;
using BPMO.Facade.SDNI.BOF;
using BPMO.Facade.SDNI.BR;
using BPMO.Patterns.Creational.DataContext;
using BPMO.Primitivos.Enumeradores;
using BPMO.SDNI.Flota.Reportes.BR;
using BPMO.SDNI.Flota.Reportes.VIS;

namespace BPMO.SDNI.Flota.Reportes.PRE
{
    /// <summary>
    /// Presentador usado para presentar el reporte de Añejamiento de Flota
    /// </summary>
    public class AniejamientoFlotaPRE
    {
        #region Constantes
        /// <summary>
        /// Nombre de la clase para agregar a los mensajes de Error
        /// </summary>
        private static readonly string NombreClase = typeof(AniejamientoFlotaPRE).Name;
        #endregion 

        #region Atributos
        /// <summary>
        /// El DataContext que proveerá acceso a la base de datos
        /// </summary>
        private IDataContext dctx;

        /// <summary>
        /// Vista que se esta gestionando
        /// </summary>
        private IAniejamientoFlotaVIS vista;

        /// <summary>
        /// Controlador del reporte general de renta diaria
        /// </summary>
        private AniejamientoFlotaBR controlador;
        #endregion

        #region Contructor
        /// <summary>
        /// Constructor por default del presentador
        /// </summary>
        /// <param name="vista">Vista que será gestionada por el presentador</param>
        public AniejamientoFlotaPRE(IAniejamientoFlotaVIS vista)
        {
            try
            {
                this.vista = vista;
                this.dctx = FacadeBR.ObtenerConexion();
                this.controlador = new AniejamientoFlotaBR();
            }
            catch (Exception ex)
            {
                this.vista.MostrarMensaje("Inconsistencias en la configuracion", ETipoMensajeIU.ERROR, NombreClase + ex.Message);
            }
        }
        #endregion

        #region Metodos

        /// <summary>
        /// Prepara los elementos para presentar cuando se carga la pagina
        /// </summary>
        public void PrepararConsulta()
        {
            this.BindReporteDetallado();
            this.BindTipoUnidad();
            this.BindArea();
        }

        /// <summary>
        /// Consulta los Registros para el Reporte de Añejamiento de Flota
        /// </summary>
        public void Consultar()
        {
             Hashtable parameters = new Hashtable();
            parameters["UnidadOperativaID"] = this.vista.UnidadOperativaID;
            parameters["ModuloID"] = this.vista.ModuloID;
            if (vista.Area != null)
                parameters["Area"] = vista.Area;
            if (vista.CuentaClienteID != null)
                parameters["CuentaClienteID"] = this.vista.CuentaClienteID;
            if (vista.ModeloID != null)
                parameters["ModeloID"] = this.vista.ModeloID;
            if (!String.IsNullOrEmpty(vista.EtiquetaReporte) || !String.IsNullOrWhiteSpace(vista.EtiquetaReporte))
                parameters["EtiquetaReporte"] = vista.EtiquetaReporte.ToUpper();
            if (vista.ReporteDetallado != null)
                parameters["ReporteDetallado"] = vista.ReporteDetallado;
            if(vista.TipoUnidad != null)
                parameters["TipoUnidad"] = vista.TipoUnidad;
            
            if(this.vista.SucursalID.HasValue)
                parameters["SucursalID"] = new Int32[] { this.vista.SucursalID.Value };
            else
            {
                SucursalBOF sucursal = new SucursalBOF();
                sucursal.UnidadOperativa = new UnidadOperativaBO { Id = this.vista.UnidadOperativaID };
                sucursal.Usuario = new UsuarioBO { Id = this.vista.UsuarioID };
                sucursal.Activo = true;

                List<SucursalBOF> sucursalesPermitidas = FacadeBR.ConsultarSucursalesSeguridad(this.dctx, sucursal);
                if(sucursalesPermitidas.Count > 0)
                {
                    parameters["SucursalID"] = sucursalesPermitidas
                                                .Select(x => x.Id.Value)
                                                .ToArray();
                }
                else //Sino tiene sucursales asignadas al usuario en curso se manda una sucursal no existente
                    parameters["SucursalID"] = new Int32[] { -1000 };
            }

            Dictionary<String, Object> reportParameters = this.controlador.ConsultarReporteAniejamientoFlota(this.dctx, parameters);
            if(!reportParameters.ContainsKey("Count"))
            {
                this.vista.MostrarMensaje("No se encontraron coincidencias con los filtros especificados", ETipoMensajeIU.ADVERTENCIA);
                return;
            }
            else if((reportParameters["Count"] as Int32?) == 0)
            {
                this.vista.MostrarMensaje("No se encontraron coincidencias con los filtros especificados", ETipoMensajeIU.ADVERTENCIA);
                return;
            }

            this.vista.EstablecerPaqueteNavegacionImprimir("BEP1401.CU030", reportParameters);
            this.vista.IrAImprimir();
        }

        /// <summary>
        /// Crea la lista que sera usada en la interfaz y la envia a la lista
        /// </summary>
        private void BindReporteDetallado()
        {
            var lista =  new ArrayList
            {
                new {Value = 0, Text = "NO"},
                new {Value = 1, Text = "SI"}
            };

            vista.BindReporteDetallato(lista);
        }

        /// <summary>
        /// Crea la lista para el tipo de Activo Fijo Unidad/EquipoAliado
        /// </summary>
        private void BindTipoUnidad()
        {
            var lista = new ArrayList
            {
                new {Value = -1, Text = "TODOS"},
                new {Value = 0, Text = "UNIDAD"},
                new {Value = 1, Text = "EQUIPOS ALIADOS"}
            };

            vista.BindTipoUnidad(lista);
        }

        /// <summary>
        /// Crea la lista para el tipo de Area/Departamento
        /// </summary>
        private void BindArea()
        {
            var lista = new ArrayList
            {
                new {Value = -1, Text = "TODOS"},
                new {Value = 0, Text = "RD"},
                new {Value = 1, Text = "FSL"}
            };

            this.vista.BindArea(lista);
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
                if(this.vista.UsuarioID == null) throw new Exception("El identificador del usuario no debe ser nulo");
                if(this.vista.UnidadOperativaID == null) throw new Exception("El identificador de la Unidad Operativa no debe ser nulo");
                //Se crea el objeto de seguridad                
                SeguridadBO seguridadBO = this.CrearObjetoSeguridad();

                //Se valida si el usuario tiene permiso a la acción principal
                if(!FacadeBR.ExisteAccion(this.dctx, "CONSULTAR", seguridadBO))
                    this.vista.RedirigirSinPermisoAcceso();
            }
            catch(Exception ex)
            {
                throw new Exception(NombreClase + ".ValidarAcceso: " + ex.GetBaseException().Message);
            }
        }
        #endregion
    }
}