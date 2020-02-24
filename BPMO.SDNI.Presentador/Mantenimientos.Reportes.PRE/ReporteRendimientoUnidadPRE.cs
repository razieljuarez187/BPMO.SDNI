using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BPMO.SDNI.Mantenimiento.Reportes.VIS;
using BPMO.Patterns.Creational.DataContext;
using BPMO.SDNI.Mantenimiento.Reportes.BR;
using BPMO.Facade.SDNI.BR;
using BPMO.Primitivos.Enumeradores;
using BPMO.Basicos.BO;
using System.Collections;
using System.Globalization;
using BPMO.Facade.SDNI.BOF;

namespace BPMO.SDNI.Mantenimientos.Reportes.PRE
{
    /// <summary>
    /// Presentador para la UI de consulta de reporte de rendimiento por unidad
    /// </summary>
    public class ReporteRendimientoUnidadPRE
    {
        #region Atributos
        /// <summary>
        /// El DataContext que proveerá acceso a la base de datos
        /// </summary>
        private IDataContext dataContext;
        /// <summary>
        /// Nombre de la clase para agregar a los mensajes de Error
        /// </summary>
        private string nombreClase = typeof(ReporteRendimientoUnidadPRE).Name;
        /// <summary>
        /// Vista que se esta gestionando
        /// </summary>
        IReporteRendimientoUnidadVIS vista;
        /// <summary>
        /// Controlador del reporte general de renta diaria
        /// </summary>
        private ReporteRendimientoUnidadBR controlador;
        #endregion
        #region Constructor
        public ReporteRendimientoUnidadPRE(IReporteRendimientoUnidadVIS vista)
        {
            try
            {
                this.vista = vista;
                this.dataContext = FacadeBR.ObtenerConexion();
                this.controlador = new ReporteRendimientoUnidadBR();
            }
            catch (Exception ex)
            {
                this.vista.MostrarMensaje("Inconsistencias en la configuracion", ETipoMensajeIU.ERROR, nombreClase + ex.Message);
            }
        }
        #endregion
        #region Metodos
        /// <summary>
        /// Prepara los elementos para presentar cuando se carga la pagina
        /// </summary>
        public void PrepararConsulta()
        {
            this.BindReporteGlobal();
            this.BinMesFinal();
        }

        /// <summary>
        /// Consulta los datos del reporte
        /// </summary>
        public void Consultar()
        {
            Hashtable parameters = new Hashtable();
            parameters["UnidadOperativaID"] = this.vista.UnidadOperativaID;
            parameters["ModuloID"] = this.vista.ModuloID;
            if (vista.UnidadID != null)
                parameters["UnidadID"] = vista.UnidadID;
            if (vista.ClienteID != null)
                parameters["ClienteID"] = vista.ClienteID;
            if (vista.AreaUnidad != null)
                parameters["AreaID"] = (Int32)vista.AreaUnidad.Value;
            if (vista.ReporteGlobal != null)
            {
                parameters["ReporteGlobal"] = this.vista.ReporteGlobal;

                if (!this.vista.ReporteGlobal.Value && vista.Mes == null)
                    throw new Exception("Se debe indicar un Mes inicial cuando el reporte es de tipo Mensual");
            }
            if (vista.Mes != null)
                parameters["FechaInicio"] = this.vista.Mes == 1 ?
                    (this.vista.ReporteGlobal.Value == true ? new DateTime(1901, 1, 1) : new DateTime(this.vista.Anio.Value - 1, 12, 1))
                    : new DateTime(this.vista.Anio.Value, this.vista.Mes.Value, 1);
            else
                parameters["FechaInicio"] = new DateTime(1901, 1, 1);
            if (vista.MesFinal != null)
            {
                var fechaFin = new DateTime(this.vista.Anio.Value, this.vista.MesFinal.Value, 1,23,59,59);
                fechaFin = fechaFin.AddMonths(1).AddDays(-1);
                parameters["FechaFin"] = fechaFin;
            }
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

            Dictionary<String, Object> reportParameters = this.controlador.ConsultarReporteRendimientoUnidad(this.dataContext, parameters);
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

            this.vista.EstablecerPaqueteNavegacionImprimir("PLEN.BEP.15.MODMTTO.CU060", reportParameters);
            this.vista.IrAImprimir();
        }

        /// <summary>
        /// Coloca el tipo de reporte en la interfaz
        /// </summary>
        public void BindReporteGlobal()
        {
            var lista = new ArrayList
            {
                new {Value = 0, Text = "MENSUAL"},
                new {Value = 1, Text = "GLOBAL"}
            };

            this.vista.BindTipoReporte(lista);
        }

        /// <summary>
        /// Coloca los campos del mes final
        /// </summary>
        public void BinMesFinal()
        {
            ArrayList items = this.ObtenerMeses();
            this.vista.BindMesFinal(items);
        }

        /// <summary>
        /// Obtener la relación de mese aplicables para un filtro
        /// </summary>
        /// <returns>Lista de meses a aplicar</returns>
        private ArrayList ObtenerMeses()
        {
            ArrayList items = new ArrayList();

            for (int i = 0; i < 12 && i < CultureInfo.CurrentCulture.DateTimeFormat.MonthNames.Length; i++)
                items.Add(
                    new
                    {
                        value = i + 1,
                        Text = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(CultureInfo.CurrentCulture.DateTimeFormat.MonthNames[i])
                    }
                );

            return items;
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
