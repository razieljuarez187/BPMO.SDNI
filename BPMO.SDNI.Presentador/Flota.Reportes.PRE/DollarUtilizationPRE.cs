//Satisface el CU024 - Reporte de Dollar Utilization
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using BPMO.Basicos.BO;
using BPMO.Facade.SDNI.BOF;
using BPMO.Facade.SDNI.BR;
using BPMO.Patterns.Creational.DataContext;
using BPMO.Primitivos.Enumeradores;
using BPMO.SDNI.Flota.Reportes.BR;
using BPMO.SDNI.Flota.Reportes.VIS;
using System.Linq;

namespace BPMO.SDNI.Flota.Reportes.PRE
{
    /// <summary>
    /// Presentador que gestiona las peticiones de la Vista de Reporte de Dollar Utilization
    /// </summary>
    public class DollarUtilizationPRE
    {
        #region Constantes
        /// <summary>
        /// Nombre de la clase para agregar a los mensajes de Error
        /// </summary>
        private static readonly string nombreClase = typeof(DollarUtilizationPRE).Name;
        #endregion

        #region Atributos
        /// <summary>
        /// El DataContext que proveerá acceso a la base de datos
        /// </summary>
        private IDataContext dctx;

        /// <summary>
        /// Vista que se gestiona
        /// </summary>
        private IDollarUtilizationVIS vista;

        /// <summary>
        /// Controlador de consulta de dollar utilization
        /// </summary>
        private ReporteDollarUtilizationBR controlador;
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor por default del presentador
        /// </summary>
        /// <param name="vista">Vista que será gestionada por el presentador</param>
        public DollarUtilizationPRE(IDollarUtilizationVIS vista)
        {
            try
            {
                this.vista = vista;
                this.dctx = FacadeBR.ObtenerConexion();
                this.controlador = new ReporteDollarUtilizationBR();
            }
            catch (Exception ex)
            {
                this.vista.MostrarMensaje("Inconsistencias en la configuración", ETipoMensajeIU.ERROR, nombreClase + ex.Message);
            }
        }
        #endregion

        #region Métodos
        /// <summary>
        /// Método abstracto que se ejecuta cuando se consulta el reporte
        /// </summary>
        public void Consultar()
        {
            Hashtable parameters = new Hashtable();
            parameters["UnidadOperativaID"] = this.vista.UnidadOperativaID;
            parameters["ModuloID"] = this.vista.ModuloID;

            parameters["FechaInicio"] = ObtenerFecha(this.vista.AnioFechaInicio.Value, this.vista.MesFechaInicio.Value, true);
            parameters["MesInicio"] = this.vista.MesFechaInicio.Value;
            parameters["AnioInicio"] = this.vista.AnioFechaInicio.Value;
            parameters["FechaFin"] = ObtenerFecha(this.vista.AnioFechaFin.Value, this.vista.MesFechaFin.Value, false);
            parameters["MesFin"] = this.vista.MesFechaFin.Value;
            parameters["AnioFin"] = this.vista.AnioFechaFin.Value;
            parameters["Anios"] = ObtenerAnios(this.vista.AnioFechaFin.Value, this.vista.AnioFechaInicio.Value);

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

            Dictionary<String, Object> reportParameters = this.controlador.ConsultarReporteDollarUtilizationRD(dctx, parameters);

            if(!reportParameters.ContainsKey("Count"))
            {
                this.vista.MostrarMensaje("No se encontraron coincidencias con los filtros especificados", ETipoMensajeIU.ADVERTENCIA);
                return;
            }
            else if ((reportParameters["Count"] as Int32?) == 0)
            {
                this.vista.MostrarMensaje("No se encontraron coincidencias con los filtros especificados", ETipoMensajeIU.ADVERTENCIA);
                return;
            }
            
            this.vista.EstablecerPaqueteNavegacionImprimir("BEP1401.CU024", reportParameters);
            this.vista.IrAImprimir();
        }

        /// <summary>
        /// Obtiene una fecha tomando como base el anio y mes seleccionado
        /// </summary>
        /// <param name="anio">Año de la fecha</param>
        /// <param name="mes">Mes de la fecha</param>
        /// <param name="primerDia">Indica si se generará la fecha con el primer día del mes, de lo contrario se generará con el último</param>
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
        /// Obtiene los Años que existen entre un rango de anio seleccionado en el filtro
        /// </summary>
        /// <param name="anioFin">Año final</param>
        /// <param name="anioInicio">Año inicial</param>
        /// <returns>Cadena con los años existentes entre el rango dado.</returns>
        private string ObtenerAnios(Int32 anioFin, Int32 anioInicio)
        {
            string anios = string.Empty;
            Int32 menorAnio = anioInicio;

            if (anioInicio == anioFin)
                anios = anioInicio.ToString();
            else
            {
                anios = anioInicio.ToString() +",";
                do
                {
                    menorAnio = menorAnio + 1;
                    anios += menorAnio.ToString() + ",";
                }
                while (menorAnio != anioFin);
            }

            if (anios.EndsWith(","))
                anios = anios.Substring(0, anios.Length - 1);
            
            return anios;
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
        /// Valida el acceso a la sesión en curso
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
                throw new Exception(nombreClase + ".ValidarAcceso:" + ex.GetBaseException().Message);
            }
        }
        #endregion
    }
}
