//Satisface al CU019 - Reporte de Flota Activa de RD Registrados

using System;
using System.Collections;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using BPMO.Basicos.BO;
using BPMO.Facade.SDNI.BOF;
using BPMO.Facade.SDNI.BR;
using BPMO.Patterns.Creational.DataContext;
using BPMO.Primitivos.Enumeradores;
using BPMO.SDNI.Contratos.BO;
using BPMO.SDNI.Reportes.VIS;
using BPMO.Servicio.Catalogos.BO;
using BPMO.SDNI.Comun.BOF;
using BPMO.SDNI.Equipos.BOF;

namespace BPMO.SDNI.Reportes.PRE
{
    /// <summary>
    /// Representar un representador para una pagina maestra de reportes
    /// </summary>
    public class ReportesPRE
    {
        #region Atributos
        /// <summary>
        /// El DataContext que proveerá acceso a la base de datos
        /// </summary>
        private IDataContext dctx;

        /// <summary>
        /// La vista que esta mostrando los datos procesados en el presentador
        /// </summary>
        private IReporteVIS vista;

        /// <summary>
        /// Nombre de la clase para agregar a los mensajes de Error
        /// </summary>
        private const string nombreClase = "ReportesPRE";
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor por default del presentador
        /// </summary>
        /// <param name="vista">Vista que será gestionada por el presentador</param>
        public ReportesPRE(IReporteVIS vista)
        {
            try
            {
                this.vista = vista;
                this.dctx = FacadeBR.ObtenerConexion();                
            }
            catch (Exception ex)
            {
                this.vista.MostrarMensaje("Inconsistencias en la configuracion", ETipoMensajeIU.ERROR, nombreClase + ex.Message);
            }
        }
        #endregion

        #region Métodos para el Buscador
        /// <summary>
        /// Prepara un BO para la Busqueda en su respectivo catalogo
        /// </summary>
        /// <param name="catalogo">catalogo donde se realizara la busqueda</param>
        /// <returns>Objeto que define el filtro a aplicar al buscador</returns>
        public object PrepararBOBuscador(string catalogo)
        {
            object obj = null;

            switch (catalogo)
            {
                case "Sucursal":
                    SucursalBOF sucursal = new SucursalBOF();
                    sucursal.UnidadOperativa = new UnidadOperativaBO { Id = this.vista.UnidadOperativaID };
                    sucursal.Usuario = new UsuarioBO { Id = this.vista.UsuarioID };

                    sucursal.Nombre = this.vista.SucursalNombre;
                    sucursal.Activo = true;

                    obj = sucursal;
                    break;

                case "Modelo":
                    ModeloBO modelo = new ModeloBO();                    
                    modelo.Nombre = this.vista.ModeloNombre;
                    modelo.Activo = true;

                    obj = modelo;
                    break;

                case "CuentaClienteIdealease":                  
                    CuentaClienteIdealeaseBOF cliente = new CuentaClienteIdealeaseBOF { Cliente = new ClienteBO() };

                    cliente.Nombre = this.vista.ClienteNombre;
                    cliente.UnidadOperativa = new UnidadOperativaBO();
                    cliente.UnidadOperativa.Id = this.vista.UnidadOperativaID;

                    obj = cliente;
                    break;
                case "UnidadIdealease":
                    UnidadBOF unidad = new UnidadBOF();

                    if (!string.IsNullOrEmpty(vista.NumeroSerie))
                        unidad.NumeroSerie = vista.NumeroSerie;

                    obj = unidad;
                    break;
                case "Tecnico":
                    TecnicoBO tecnico = new TecnicoBO() { Empleado = new EmpleadoBO(), AdscripcionServicio = new AdscripcionServicioBO() { UnidadOperativa = new UnidadOperativaBO() { Id = this.vista.UnidadOperativaID } } };
                    
                    if (!String.IsNullOrEmpty(vista.TecnicoNombre))
                        tecnico.Empleado.NombreCompleto = vista.TecnicoNombre;
                    if (vista.TecnicoID != null)
                        tecnico.Id = vista.TecnicoID;

                    obj = tecnico;
                    break;
            }

            return obj;
        }

        /// <summary>
        /// Despliega el Resultado del Buscador
        /// </summary>
        /// <param name="catalogo">Catalogo en el que se realizo la busqueda</param>
        /// <param name="selecto">Objeto Resultante</param>
        public void DesplegarResultadoBuscador(string catalogo, object selecto)
        {
            switch (catalogo)
            {
                case "Sucursal":
                    SucursalBO sucursal = (SucursalBO)selecto;
                    if (sucursal != null)
                    {
                        if (sucursal.Id != null)
                            this.vista.SucursalID = sucursal.Id;
                        else                            
                            this.vista.SucursalID = null;                        

                        if (sucursal.Nombre != null)
                            this.vista.SucursalNombre = sucursal.Nombre;
                        else
                            this.vista.SucursalNombre = null;                        
                    }
                    break;

                case "Modelo":
                    ModeloBO modelo = (ModeloBO)selecto;
                    if (modelo != null)
                    {
                        if (modelo.Id != null)
                            this.vista.ModeloID = modelo.Id;
                        else
                            this.vista.ModeloID = null;

                        if (modelo.Nombre != null)
                            this.vista.ModeloNombre = modelo.Nombre;
                        else
                            this.vista.ModeloNombre = null;
                    }
                    break;

                case "CuentaClienteIdealease":
                    CuentaClienteIdealeaseBOF cuentaCliente = (CuentaClienteIdealeaseBOF)selecto ?? new CuentaClienteIdealeaseBOF();
                    if (cuentaCliente != null)
                    {
                        this.vista.CuentaClienteID = cuentaCliente.Id;
                        this.vista.ClienteID = cuentaCliente.Cliente != null ? cuentaCliente.Cliente.Id : null;
                        this.vista.ClienteNombre = cuentaCliente.Nombre;
                    }
                    break;
                case "UnidadIdealease":
                    UnidadBOF unidad = (UnidadBOF)selecto ?? new UnidadBOF();
                    if (unidad.NumeroSerie != null)
                    {
                        vista.NumeroSerie = unidad.NumeroSerie;
                        vista.UnidadID = unidad.UnidadID;
                    }
                    else
                    {
                        vista.NumeroSerie = string.Empty;
                        vista.UnidadID = null;
                    }

                    break;
                case "Tecnico":
                    TecnicoBO tecnico = (TecnicoBO)selecto ?? new TecnicoBO();

                    if (tecnico != null)
                    {
                        this.vista.TecnicoID = tecnico.Id != null ? tecnico.Id : null;
                        this.vista.TecnicoNombre = tecnico.Empleado != null && !String.IsNullOrEmpty(tecnico.Empleado.NombreCompleto) ? tecnico.Empleado.NombreCompleto : String.Empty;
                    }
                    else
                    {
                        vista.TecnicoNombre = String.Empty;
                        vista.TecnicoID = null;
                    }
                    break;
            }
        }
        #endregion

        #region Metodos
        /// <summary>
        /// Inicializa la página maestra
        /// </summary>
        public void Inicializar()
        {
            this.vista.ModeloFiltroVisible = false;
            this.vista.SucursalFiltroVisible = false;
            this.vista.ClienteFiltroVisible = false;
            this.vista.DepartamentoFiltroVisible = false;
            this.vista.PeriodoReporteFiltroVisible = false;
            this.vista.AnioFiltroVisible = false;
            this.vista.MesFiltroVisible = false;
            this.vista.DiaCorteFiltroVisible = false;
            this.vista.FechaInicioContratoFiltroVisible = false;
            this.vista.FechaFinContratoFiltroVisible = false;
            this.vista.FechaInicioFiltroVisible = false;
            this.vista.FechaFinFiltroVisible = false;
            this.vista.UnidadFiltroVisible = false;
            this.vista.TecnicoFiltroVisible = false;
            this.vista.AreaUnidadFiltroVisible = false;
        }

        /// <summary>
        /// Obtiene la relación de años aplicables para un filtro
        /// </summary>
        /// <returns>Lista de años a aplicar</returns>
        private ArrayList ObtenerAnios()
        {
            ArrayList items = new ArrayList();
            int minYear = 1950;
            int maxYear = DateTime.Now.Year + 20;

            for (int i = minYear; i < maxYear; i++)
                items.Add(
                    new
                    {
                        value = i,
                        Text = i
                    }
                );

            return items;
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
        /// Liga las áreas departamentos al control asignado
        /// </summary>
        public void BindAreasDepartamentos()
        {
            ArrayList items = new ArrayList();
            foreach (ETipoContrato it in Enum.GetValues(typeof(ETipoContrato)))
            {
                items.Add(new 
                {
                    Value = (int)it,
                    Text = new Func<String>(() =>
                    {
                        String value = Enum.GetName(typeof(ETipoContrato), it);

                        DescriptionAttribute descriptor = typeof(ETipoContrato)
                                                             .GetField(value)
                                                             .GetCustomAttributes(typeof(DescriptionAttribute), true)
                                                             .Cast<DescriptionAttribute>()
                                                             .FirstOrDefault();

                        if (descriptor != null)
                            value = descriptor.Description;

                        return value;
                    }).Invoke()
                });
            };

            if (!this.vista.DepartamentoFiltroRequerido)
                items.Insert(0, new { Value = "-1", Text = "-- Seleccione un Departamento --" });

            this.vista.BindAreasDepartamentos(items);
        }

        /// <summary>
        /// Liga las áreas departamentos al control asignado
        /// </summary>
        public void BindAreasUnidad()
        {
            ArrayList items = new ArrayList();
            foreach (EArea it in Enum.GetValues(typeof(EArea)))
            {
                items.Add(new
                {
                    Value = (int)it,
                    Text = new Func<String>(() =>
                    {
                        String value = Enum.GetName(typeof(EArea), it);

                        DescriptionAttribute descriptor = typeof(EArea)
                                                             .GetField(value)
                                                             .GetCustomAttributes(typeof(DescriptionAttribute), true)
                                                             .Cast<DescriptionAttribute>()
                                                             .FirstOrDefault();

                        if (descriptor != null)
                            value = descriptor.Description;

                        return value;
                    }).Invoke()
                });
            };

            if (!this.vista.AreaUnidadFiltroRequerido)
                items.Insert(0, new { Value = "-1", Text = "-- Seleccione un Área --" });

            this.vista.BindAreasUnidad(items);
        }

        /// <summary>
        /// liga los tipos reportes al control asignado
        /// </summary>
        public void BindTiposReportes()
        {
            var items = new ArrayList
            {
                new { Value = 1, Text = "Mensual" },
                new { Value = 3, Text = "Trimestral" },
                new { Value = 6, Text = "Semestral" }
            };

            this.vista.BindTiposReportes(items);
        }

        /// <summary>
        /// Liga los periodos de reportes al control asignado
        /// </summary>
        public void BindPeriodosReportes()
        {
            ArrayList items = new ArrayList();           
            if (this.vista.TipoReporte != null)
            {
                int meses = this.vista.TipoReporte.GetValueOrDefault();
                switch (meses)
                {
                    case 1:
                        for (int i = 1; i < 13; i++)
                            items.Add(
                                new
                                {
                                    value = i,
                                    Text = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(i))
                                }
                            );
                        break;

                    default:
                        String title = "Periodo";
                        switch (meses)
                        {
                            case 3:
                                title = "Timestre";
                                break;

                            case 6:
                                title = "Semestre";
                                break;
                        }

                        int sections = 12 / meses;
                        for (int i = 1; i < sections + 1; i++)
                            items.Add(
                                new
                                {
                                    value = i,
                                    Text = new Func<String>(() =>
                                    {
                                        StringBuilder sb = new StringBuilder();
                                        sb.Append(title);
                                        sb.Append(" ");
                                        sb.Append(i);
                                        sb.Append(" (");

                                        int init = 1 + ((i - 1) * meses);
                                        for (int j = init; j < init + meses; j++)
                                        {
                                            if (j > init)
                                                sb.Append(", ");

                                            if (j > 0 && j < 13)
                                            {
                                                String monthName = CultureInfo.CurrentCulture.DateTimeFormat.AbbreviatedMonthNames[j - 1];
                                                sb.Append(CultureInfo.CurrentCulture.TextInfo.ToTitleCase(monthName.Replace(".", "")));
                                            }
                                        }

                                        sb.Append(")");
                                        return sb.ToString();
                                    }).Invoke()
                                }
                            );
                        break;
                }
            }

            bool isEmpty = items.Count > 0;
            if (!this.vista.PeriodoReporteFiltroRequerido)
                items.Insert(0, new { Value = -1, Text = "-- Seleccione un Periodo --" });

            this.vista.BindPeriodosReportes(items, isEmpty);
        }

        /// <summary>
        /// Lisga los anños del reporte al controla asignado
        /// </summary>
        public void BindAnios()
        {
            ArrayList items = this.ObtenerAnios();
            if (!this.vista.AnioFiltroRequerido)
                items.Insert(0, new { Value = "-1", Text = "----" });

            this.vista.BindAnios(items);
        }

        /// <summary>
        /// Liga los meses del reporte al control asignado
        /// </summary>
        public void BindMeses()
        {
            ArrayList items = this.ObtenerMeses();
            if (!this.vista.MesFiltroRequerido)
                items.Insert(0, new { Value = "-1", Text = "---------" });

            this.vista.BindMeses(items);
        }
        /// <summary>
        /// Liga los años de fecha de inicio del reporte al control asignado
        /// </summary>
        public virtual void BindAniosFechaInicio()
        {
            ArrayList items = this.ObtenerAnios();
            if (!this.vista.FechaInicioFiltroRequerido)
                items.Insert(0, new { Value = "-1", Text = "----" });

            this.vista.BindAniosFechaInicio(items);
        }

        /// <summary>
        /// Liga los meses de fecha de inicio del reporte al control asignado
        /// </summary>
        public void BindMesesFechaInicio()
        {
            ArrayList items = this.ObtenerMeses();
            if (!this.vista.FechaInicioFiltroRequerido)
                items.Insert(0, new { Value = "-1", Text = "---------" });

            this.vista.BindMesesFechaInicio(items);
        }

        /// <summary>
        /// Liga los años de fecha de fin del reporte al control asignado
        /// </summary>
        public void BindAniosFechaFin()
        {
            ArrayList items = this.ObtenerAnios();
            if (!this.vista.FechaFinFiltroRequerido)
                items.Insert(0, new { Value = "-1", Text = "----" });

            this.vista.BindAniosFechaFin(items);
        }

        /// <summary>
        /// Liga los meses de fecha de fin del reporte al control asignado
        /// </summary>
        public void BindMesesFechaFin()
        {
            ArrayList items = this.ObtenerMeses();
            if (!this.vista.FechaFinFiltroRequerido)
                items.Insert(0, new { Value = "-1", Text = "---------" });

            this.vista.BindMesesFechaFin(items);
        }
        #endregion
    }
}
