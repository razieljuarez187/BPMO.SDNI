//Satisface al CU068 - Reporte Mantenimiento Realizado Contra Programado

using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using BPMO.Basicos.BO;
using BPMO.Facade.SDNI.BO;
using BPMO.Facade.SDNI.BR;
using BPMO.Patterns.Creational.DataContext;
using BPMO.SDNI.Comun.BO;
using BPMO.SDNI.Comun.BR;
using BPMO.SDNI.Mantenimiento.Reportes.BR;
using BPMO.SDNI.Mantenimiento.Reportes.DA;
using BPMO.SDNI.Mantenimiento.Reportes.VIS;
using BPMO.Servicio.Catalogos.BO;

namespace BPMO.SDNI.Mantenimientos.Reportes.PRE
{
    /// <summary>
    /// Presentador para el reporte
    /// </summary>
    public class ReporteMantenimientoRealizadoContraProgramadoPRE
    {
        #region Propiedades
        /// <summary>
        /// Vista
        /// </summary>
        private IMantenimientoRealizadoContraProgramadoVIS vista = null;
        /// <summary>
        /// Contexto de conexion
        /// </summary>
        private IDataContext dataContext = null;
        /// <summary>
        /// Nombre de la clase
        /// </summary>
        private string nombreClase = typeof(ReporteMantenimientoRealizadoContraProgramadoPRE).Name;
        #endregion

        #region Constructores
        /// <summary>
        /// Constructor por defecto
        /// </summary>
        /// <param name="vista"></param>
        public ReporteMantenimientoRealizadoContraProgramadoPRE(IMantenimientoRealizadoContraProgramadoVIS vista)
        {
            this.vista = vista;
            this.dataContext = FacadeBR.ObtenerConexion();
        }
        #endregion

        #region Métodos

        /// <summary>
        /// Prepara el buscador general
        /// </summary>
        /// <param name="catalogo"></param>
        /// <returns></returns>
        public object PrepararBOBuscador(string catalogo)
        {
            object obj = null;

            switch (catalogo)
            {
                case "Unidad":
                    EquipoBepensaBO ebBO = new EquipoBepensaBO();
                    ebBO.ActivoFijo = new ActivoFijoBO();
                    ebBO.ActivoFijo.Auditoria = new AuditoriaBO();
                    ebBO.Unidad = new Servicio.Catalogos.BO.UnidadBO();
                    ebBO.Unidad.Auditoria = new AuditoriaBO();
                    ebBO.Unidad.ClasificadorAplicacion = new ClasificadorAplicacionBO();
                    ebBO.Unidad.ClasificadorAplicacion.Auditoria = new AuditoriaBO();
                    ebBO.Unidad.Cliente = new ClienteBO();
                    ebBO.Unidad.Cliente.Auditoria = new AuditoriaBO();
                    ebBO.Unidad.ConfiguracionModeloMotorizacion = new ConfiguracionModeloMotorizacionBO();
                    ebBO.Unidad.ConfiguracionModeloMotorizacion.Auditoria = new AuditoriaBO();
                    ebBO.Unidad.ConfiguracionModeloMotorizacion.ClasificadorMotorizacion = new ClasificadorMotorizacionBO();
                    ebBO.Unidad.ConfiguracionModeloMotorizacion.ClasificadorMotorizacion.Auditoria = new AuditoriaBO();
                    ebBO.Unidad.ConfiguracionModeloMotorizacion.Modelo = new ModeloBO();
                    ebBO.Unidad.ConfiguracionModeloMotorizacion.Modelo.Auditoria = new AuditoriaBO();
                    ebBO.Unidad.ConfiguracionModeloMotorizacion.Modelo.Marca = new MarcaBO();
                    ebBO.Unidad.ConfiguracionModeloMotorizacion.Modelo.Marca.Auditoria = new AuditoriaBO();
                    ebBO.Unidad.Distribuidor = new DistribuidorBO();
                    ebBO.Unidad.Distribuidor.Auditoria = new AuditoriaBO();
                    ebBO.Unidad.TipoUnidad = new TipoUnidadBO();
                    ebBO.Unidad.TipoUnidad.Auditoria = new AuditoriaBO();

                    ebBO.Unidad.NumeroSerie = this.vista.NumeroVIN;
                    ebBO.Unidad.Activo = true;
                    ebBO.ActivoFijo.NumeroSerie = this.vista.NumeroVIN;
                    ebBO.ActivoFijo.Libro = this.vista.LibroActivos;
                    obj = ebBO;
                    break;
            }

            return obj;
        }

        /// <summary>
        /// Despliega el resultado seleccionado del buscador
        /// </summary>
        /// <param name="catalogo"></param>
        /// <param name="selecto"></param>
        public void DesplegarResultadoBuscador(string catalogo, object selecto)
        {
            switch (catalogo)
            {
                case "Unidad":
                    EquipoBepensaBO ebBO = (EquipoBepensaBO)selecto;
                    if (ebBO == null) ebBO = new EquipoBepensaBO();

                    if (ebBO.NumeroSerie != null)
                    {
                        this.vista.NumeroVIN = ebBO.NumeroSerie;
                    }
                    else
                    {
                        this.vista.NumeroVIN = null;
                    }
                    break;
            }
        }

        /// <summary>
        /// Prepara el buscador general
        /// </summary>
        public void PrepararBusqueda()
        {
            this.vista.LimpiarSesion();
            this.vista.PrepararBusqueda();
            this.EstablecerInformacionInicial();
        }

        /// <summary>
        /// Establece la informacion adicional
        /// </summary>
        private void EstablecerInformacionInicial()
        {
            try
            {
                #region Configuraciones de Unidad Operativa
                //Obtener las configuraciones de la unidad operativa
                List<ConfiguracionUnidadOperativaBO> lstConfigUO = new ModuloBR().ConsultarConfiguracionUnidadOperativa(this.dataContext, new ConfiguracionUnidadOperativaBO() { UnidadOperativa = new UnidadOperativaBO() { Id = this.vista.UnidadOperativaId } }, this.vista.ModuloID);
                if (lstConfigUO.Count <= 0)
                    throw new Exception("No se encontraron las configuraciones del sistema para la unidad operativa en la que trabaja.");

                //Establecer las configuraciones de la unidad operativa
                this.vista.LibroActivos = lstConfigUO[0].Libro;
                #endregion
            }
            catch (Exception ex)
            {
                throw new Exception(this.nombreClase + ".EstablecerInformacionInicial: " + ex.Message);
            }
        }

        /// <summary>
        /// Establece los meses
        /// </summary>
        public void BindMeses()
        {
            ArrayList items = this.ObtenerMeses();
            items.Insert(0, new { Value = "-1", Text = "---------" });
            this.vista.BindMeses(items);
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
        #endregion

        /// <summary>
        /// Realiza la consulta para el reporte
        /// </summary>
        public void Consultar()
        {
            Hashtable parameters = new Hashtable();
            if (this.vista.SucursalID != null)
                parameters["SucursalID"] = this.vista.SucursalID;
            if (this.vista.SucursalNombre!= null)
                parameters["NombreSucursal"] = this.vista.SucursalNombre;
            if (this.vista.ClienteID.HasValue)
                parameters["ClienteID"] = this.vista.ClienteID;
            if (this.vista.Anio != null)
                parameters["Anio"] = this.vista.Anio;
            if (this.vista.MesInicio != null)
                parameters["MesInicio"] = this.vista.MesInicio;
            if (this.vista.MesFin != null)
                parameters["MesFin"] = this.vista.MesFin;
            if (!string.IsNullOrEmpty(this.vista.Vin))
                parameters["Vin"] = this.vista.Vin;
            if (this.vista.ModuloID != null)
                parameters["ModuloID"] = this.vista.ModuloID;
            if (this.vista.UnidadOperativaId != null)
                parameters["UnidadOperativaID"] = this.vista.UnidadOperativaId;

            Dictionary<String, Object> reportParameters = new ReporteMantenimientoRealizadoContraProgramadoBR().Consultar(dataContext, parameters);

            this.vista.EstablecerPaqueteNavegacionImprimir(this.vista.IdentificadorReporte, reportParameters);
            this.vista.IrAImprimir();
        }


        

        
    }
}
