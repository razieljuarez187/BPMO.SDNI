// Satisface el CU028 – Reporte Días de Renta
// Satisface el CU029 – Reporte Días de Renta por Tipo de Unidad

using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
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
    /// Presentador aplicable para la gestión de las peticiones de una vista de Reporte de Dias de Renta
    /// </summary>
    public class DiasRentaPRE
    {
        #region Constantes
        /// <summary>
        /// Nombre de la clase para agregar a los mensajes de Error
        /// </summary>
        private static readonly string nombreClase = typeof(DiasRentaPRE).Name;
        #endregion

        #region Atributos
        /// <summary>
        /// El DataContext que proveerá acceso a la base de datos
        /// </summary>
        private IDataContext dctx;

        /// <summary>
        /// Vista que se esta gestionando
        /// </summary>
        private IDiasRentaVIS vista;

        /// <summary>
        /// Controlador de Consulta de Contratos de Mantenimiento y Servicio Dedicado
        /// </summary>
        private RentaDiariaGeneralBR controlador;
        #endregion

        #region Contructor
        /// <summary>
        /// Constructor por default del presentador
        /// </summary>
        /// <param name="vista">Vista que será gestionada por el presentador</param>        
        public DiasRentaPRE(IDiasRentaVIS vista)
        {
            try
            {               
                this.vista = vista;
                this.dctx = FacadeBR.ObtenerConexion();
                this.controlador = new RentaDiariaGeneralBR();
            }
            catch (Exception ex)
            {
                this.vista.MostrarMensaje("Inconsistencias en la configuracion", ETipoMensajeIU.ERROR, nombreClase + ex.Message);
            }
        }
        #endregion

        #region Métodos
        /// <summary>
        /// Método abstract que se ejecuta cuando 
        /// </summary>
        public void Consultar()
        {
            Hashtable parameters = new Hashtable();
            parameters["UnidadOperativaID"] = this.vista.UnidadOperativaID;
            parameters["ModuloID"] = this.vista.ModuloID;           
         
            if (this.vista.SucursalID.HasValue)
                parameters["SucursalID"] = new Int32[] { this.vista.SucursalID.Value };
            else
            {
                SucursalBOF sucursal = new SucursalBOF();
                sucursal.UnidadOperativa = new UnidadOperativaBO { Id = this.vista.UnidadOperativaID };
                sucursal.Usuario = new UsuarioBO { Id = this.vista.UsuarioID };
                sucursal.Activo = true;

                List<SucursalBOF> sucursalesPermitidas = FacadeBR.ConsultarSucursalesSeguridad(this.dctx, sucursal);
                if (sucursalesPermitidas.Count > 0)
                {
                    parameters["SucursalID"] = sucursalesPermitidas
                                                .Select(x => x.Id.Value)
                                                .ToArray();
                }
                else //Sino tiene sucursales asignadas al usuario en curso se manda una sucursal no existente
                    parameters["SucursalID"] = new Int32[] { -1000 };
            }

            parameters["FechaInicio"] = this.ObtenerFecha(this.vista.AnioFechaInicio.Value, this.vista.MesFechaInicio.Value, true);
            parameters["FechaFin"] = this.ObtenerFecha(this.vista.AnioFechaFin.Value, this.vista.MesFechaFin.Value, false);

            Dictionary<String, Object> reportParameters = this.controlador.ConsultarReporteRDGeneral(this.dctx, parameters);
            if (((DataSet)reportParameters["DataSource"]).Tables["ReporteRDSucursal"].Rows.Count <= 0)
            {
                this.vista.MostrarMensaje("No se encontraron coincidencias con los filtros especificados", ETipoMensajeIU.ADVERTENCIA);
                return;
            }

            this.vista.EstablecerPaqueteNavegacionImprimir(this.vista.IdentificadorReporte, reportParameters);
            this.vista.IrAImprimir();
        }

        /// <summary>
        /// Obtiene una fecha tomando como base el anio y mes seleccionado
        /// </summary>
        /// <param name="anio">Año de la fecha</param>
        /// <param name="mes">Mes de la fecha</param>
        /// <param name="primerDia">Indica si se generará la fecha con el primer día del mes, de lo contrario se generará con la última</param>
        /// <returns>Objeto de fecha generado</returns>
        private DateTime ObtenerFecha(int anio, int mes, bool primerDia)
        {
            if (primerDia)
                return new DateTime(anio, mes, 1, 0, 0, 0);
            else
            {
                int dia = CultureInfo.CurrentCulture.Calendar.GetDaysInMonth(anio, mes);
                return new DateTime(anio, mes, dia, 23, 59, 59);
            }
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
                if (!FacadeBR.ExisteAccion(this.dctx, "CONSULTAR", seguridadBO))
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
