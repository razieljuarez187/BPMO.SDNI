//Satisface al CU001 - Ingregar Mantenimiento
//Satisface al CU002 - Calcular Proximom Mantenimiento
//Satisface al CU009 - Iniciar Mantenimiento
//Satisface al CU016 - Realizar Auditoria Mantenimiento
//Satisface al CU017 - Imprimir Calcomania Mantenimiento
//Satisface al CU051 - Obtener Orden Salida
//Satisface al CU062 - Ver Orden Servicio Idealease
//Satisface al CU063 - Administrar Tareas Pendientes
//Satisface al CU064 - Enviar Correo Servicio Realizado

using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;
using System.Xml;
using System.Xml.Linq;
using BPMO.Basicos.BO;
using BPMO.Facade.SDNI.BOF;
using BPMO.Facade.SDNI.BR;
using BPMO.Generales.BR;
using BPMO.Patterns.Creational.DataContext;
using BPMO.Primitivos.Enumeradores;
using BPMO.SDNI.Comun.BO;
using BPMO.SDNI.Comun.BR;
using BPMO.SDNI.Equipos.BO;
using BPMO.SDNI.Equipos.BOF;
using BPMO.SDNI.Mantenimiento.BO;
using BPMO.SDNI.Mantenimiento.BO.BOF;
using BPMO.SDNI.Mantenimiento.BR;
using BPMO.SDNI.Mantenimiento.Reportes.BR;
using BPMO.SDNI.Mantenimiento.VIS;
using BPMO.Servicio.Catalogos.BO;
using BPMO.Servicio.Procesos.BO;
using CatalogosBO = BPMO.Servicio.Catalogos.BO;
using EquiposBO = BPMO.SDNI.Equipos.BO;
using EquiposBR = BPMO.SDNI.Equipos.BR;
using BPMO.SDNI.Mantenimiento.BOF;
using BPMO.SDNI.Mantenimiento.Reportes.DA;
using BPMO.SDNI.Mantenimiento.RPT;
using DevExpress.XtraPrinting;


namespace BPMO.SDNI.Mantenimiento.PRE
{
    /// <summary>
    /// 
    /// </summary>
    public class RegistrarUnidadPRE
    {
        #region Propiedades
        private IRegisitrarUnidadVIS vista;
        private IDataContext dataContext = null;
        private RegistrarOrdenServicioBR registrarOrdenServicioBR = null;
        private MantenimientoBR mantenimientoBR = null;
        private MantenimientoUnidadBR mantenimientoUnidadBR = null;
        private MantenimientoEquipoAliadoBR mantenimientoAliadoBR = null;
        private ConsultarMantenimientoProgramadoBR consultarMantenimientoProgramadoBR = null;
        private ConsultarAuditoriaMantenimientoBR auditoriaMantenimimentoBR = null;
        private EquiposBR.UnidadBR unidadBR;
        private ModuloBR moduloBR = null;
        private UsuarioBR usuarioBR = null;
        private string nombreClase = null;

        #region Lectura Inline
        public enum TipoLectura
        {
            COMBUSTIBLE = 1,
            KM = 2,
            HORAS = 3,
        }
        
        /// <summary>
        /// Nombre imagen defecto para combustible vacio
        /// </summary>
        private string NombreImagenCombustibleDefecto
        {
            get
            {
                return ConfigurationManager.AppSettings["NombreImagenDefectoTanqueVacio"];
            }
        }
        
        /// <summary>
        /// Ruta de lectura para archivos inline
        /// </summary>
        private string RutaLecturaArchivosInline
        {
            get
            {
                return ConfigurationManager.AppSettings["RutaArchivosInline"];
            }
        }

        /// <summary>
        /// Ruta de imagenes
        /// </summary>
        private string RutaImagenes
        {
            get
            {
                return ConfigurationManager.AppSettings["UbicacionImagenes"];
            }
        }

        /// <summary>
        /// Ruta de plantilla XML
        /// </summary>
        private string RutaPlantillaXML
        {
            get
            {
                return ConfigurationManager.AppSettings["UbicacionReportesXML"];
            }
        }
        
        /// <summary>
        /// Nombre de plantilla XML
        /// </summary>
        private string PlantillaXML 
        {
            get
            {
                return ConfigurationManager.AppSettings["NombrePlantillaChecklist"]; ;
            }
        }
        /// <summary>
        /// ID de la Marca International
        /// </summary>
        public int? MarcaInternationalID
        {
            get
            {
                var marcaId = ConfigurationManager.AppSettings["MarcaInternationalID"];
                if (String.IsNullOrEmpty(marcaId))
                    return null;

                int value;
                if (int.TryParse(marcaId, out value))
                    return value;

                return null;
            }
        }
        /// <summary>
        /// Tiempo minimo para el proximo mantto
        /// </summary>
        private int? TiempoMinimoProximoMantenimiento
        {
            get
            {
                var tiempoMinimo = ConfigurationManager.AppSettings["TiempoMinimoProximoMantto"];
                if (String.IsNullOrEmpty(tiempoMinimo))
                    return null;

                int value;
                if (int.TryParse(tiempoMinimo, out value))
                    return value;

                return null;
            }
        }
        #endregion
        

        #endregion

        #region Constructores

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        /// <param name="vista"></param>
        public RegistrarUnidadPRE(IRegisitrarUnidadVIS vista)
        {
            this.vista = vista;
            this.nombreClase = this.GetType().Name;
            this.dataContext = FacadeBR.ObtenerConexion();
            this.registrarOrdenServicioBR = new RegistrarOrdenServicioBR();
            this.mantenimientoUnidadBR = new MantenimientoUnidadBR();
            this.mantenimientoAliadoBR = new MantenimientoEquipoAliadoBR();
            this.mantenimientoBR = new MantenimientoBR();
            this.consultarMantenimientoProgramadoBR = new ConsultarMantenimientoProgramadoBR();
            this.auditoriaMantenimimentoBR = new ConsultarAuditoriaMantenimientoBR();
            this.unidadBR = new BPMO.SDNI.Equipos.BR.UnidadBR();
            this.moduloBR = new ModuloBR();
            this.usuarioBR = new UsuarioBR();
        }
        #endregion

        #region Métodos


        #region Filtros Unidades
        
        /// <summary>
        /// Establece los valores iniciales del modulos
        /// </summary>
        public void InicializaModulo()
        {
            this.vista.PrepararBusqueda();
            this.EstablecerInformacionInicial();
            this.CargarDatos();
        }

        /// <summary>
        /// Prepara el objeto para el buscador general
        /// </summary>
        /// <param name="catalogo"></param>
        /// <returns></returns>
        public object PrepararBOBuscador(string catalogo)
        {
            object obj = null;

            switch (catalogo)
            {
                case "Sucursal":
                    SucursalBOF sucursal = new SucursalBOF();
                    sucursal.UnidadOperativa = new UnidadOperativaBO();
                    sucursal.UnidadOperativa.Id = this.vista.UnidadOperativaId;
                    sucursal.Nombre = this.vista.SucursalNombre;
                    sucursal.Usuario = new UsuarioBO() { Id = this.vista.UsuarioAutenticado };
                    obj = sucursal;
                    break;
                case "Cliente":
                    ClienteBO cliente = new ClienteBO();
                    cliente.Nombre = this.vista.ClienteNombre;
                    obj = cliente;
                    break;
                case "Modelo":
                    ModeloBO modelo = new ModeloBO();
                    modelo.Nombre = this.vista.ModeloNombre;
                    obj = modelo;
                    break;
                case "Tecnico":
                    TecnicoBO tecnico = new TecnicoBO();
                    obj = tecnico;
                    break;
            }

            return obj;
        }
        
        /// <summary>
        /// Establece el resultado seleccionado del buscador general
        /// </summary>
        /// <param name="catalogo"></param>
        /// <param name="selecto"></param>
        public void DesplegarResultadoBuscador(string catalogo, object selecto)
        {
            switch (catalogo)
            {
                case "Sucursal":
                    SucursalBO sucursal = (SucursalBO)selecto;
                    if (sucursal != null && sucursal.Id != null)
                        this.vista.SucursalID = sucursal.Id;
                    else
                        this.vista.SucursalID = null;

                    if (sucursal != null && sucursal.Nombre != null)
                        this.vista.SucursalNombre = sucursal.Nombre;
                    else
                        this.vista.SucursalNombre = null;

                    this.vista.CargarTalleres();
                    break;
            }
        }
        
        /// <summary>
        /// Consulta una unidad por su VIN, Numero Economico, Cliente o Modelo
        /// </summary>
        /// <param name="catalogo"></param>
        /// <param name="parametroBusqueda"></param>
        public void Consultar(string catalogo, string parametroBusqueda, string guid)
        {
            UnidadBOF bo = (UnidadBOF)this.InterfazUsuarioADatoUnidad();
            List<Equipos.BO.UnidadBO> result = new List<Equipos.BO.UnidadBO>();

            switch (catalogo)
            {
                case "NumeroSerie":
                    bo.NumeroSerie = parametroBusqueda;
                    result = unidadBR.ConsultarUnidadPorModelo(dataContext, bo).ConvertAll(s => (Equipos.BO.UnidadBO)s);
                    break;
                case "NumeroEconomico":
                    bo.NumeroEconomico = parametroBusqueda;
                    result = unidadBR.ConsultarUnidadPorModelo(dataContext, bo).ConvertAll(s => (Equipos.BO.UnidadBO)s);
                    break;
                case "Cliente":
                    bo.Cliente.Id = Int32.Parse(parametroBusqueda);
                    result = unidadBR.ConsultarUnidadPorModelo(dataContext, bo).ConvertAll(s => (Equipos.BO.UnidadBO)s);
                    break;
                case "Modelo":
                    bo.Modelo.Id = Int32.Parse(parametroBusqueda);
                    result = unidadBR.ConsultarUnidadPorModelo(dataContext, bo).ConvertAll(s => (Equipos.BO.UnidadBO)s);
                    break;
            }

            if (result != null)
            {
                if (result.Count == 0)
                {
                    this.vista.MostrarMensaje("No se encontraron coincidencias con la información proporcionada. Favor de verificar", ETipoMensajeIU.ADVERTENCIA, "");
                }
                else
                {
                    //Quitamos las unidades que esten dadas de baja, no disponibles y que esten en siniestro
                    result.RemoveAll(x => x.EstatusActual == EEstatusUnidad.Baja);
                    result.RemoveAll(x => x.EstatusActual == EEstatusUnidad.Siniestro);

                    if (result.Count == 0)
                    {
                        this.vista.MostrarMensaje("Unidad no disponible. Favor de verificar", ETipoMensajeIU.ADVERTENCIA, "");
                    }
                    else
                    {
                        List<SucursalBO> sucursales = new List<SucursalBO>();
                        List<ClienteBO> clientes = new List<ClienteBO>();

                        if (result.Count == 1)
                        {
                            Equipos.BO.UnidadBO unidad = result.FirstOrDefault();
                            MantenimientoUnidadBO mantenimiento = new MantenimientoUnidadBO();
                            this.ComplementarDatosUnidad(unidad, sucursales, clientes);
                            bool? esUltimoMantto = mantenimiento.FechaSalida == null && mantenimiento.FechaProximoServicio == null;

                            mantenimiento.IngresoUnidad.FechaIngreso = DateTime.Now;
                            mantenimiento.IngresoUnidad.Unidad = unidad;
                            mantenimiento.IngresoUnidad.NumeroEconomico = unidad.NumeroEconomico;
                            mantenimiento.IngresoUnidad.Vin = unidad.NumeroSerie;
                            mantenimiento.MantenimientoProgramado = this.consultarMantenimientoProgramadoBR.ConsultarUltimoMantenimientoProgramado(this.dataContext, unidad.EquipoID, true, esUltimoMantto);
                            this.CargarIngresoAliados(mantenimiento);
                            mantenimiento.IngresoUnidad.Controlista = this.ConsultarUsuario(new UsuarioBO() { Id = this.vista.UsuarioAutenticado });

                            this.AgregarFila(mantenimiento, guid);
                            this.CargarDatos();
                        }
                        else
                        {
                            this.ComplementarDatosUnidades(result);
                            this.vista.ResultadoBusquedaUnidades = result;
                            this.CargarGridViewUnidades();
                            this.vista.MostrarResultadoUnidades();
                        }
                    }
                }
            }
        }
        
        /// <summary>
        /// Construye el objeto UnidadBOF para la consulta
        /// </summary>
        /// <returns></returns>
        private UnidadBOF InterfazUsuarioADatoUnidad()
        {
            if (this.vista.UnidadOperativaId == null) throw new Exception(".InterfazUsuarioADato: La Unidad Operativa no puede ser nulo");
            UnidadBOF bo = new UnidadBOF() { EntraMantenimiento = true };
            bo.Sucursal = new SucursalBO();
            bo.Sucursal.UnidadOperativa = new UnidadOperativaBO { Id = this.vista.UnidadOperativaId };
            bo.Cliente = new ClienteBO();
            bo.ActivoFijo = new ActivoFijoBO();
            bo.Modelo = new Servicio.Catalogos.BO.ModeloBO();
            return bo;
        }
        
        /// <summary>
        /// Carga los equipos aliados de una unidad en mantenimiento
        /// </summary>
        /// <param name="mantenimiento"></param>
        private void CargarIngresoAliados(MantenimientoUnidadBO mantenimiento)
        {
            List<MantenimientoEquipoAliadoBO> mantenimientosEquiposAliados = new List<MantenimientoEquipoAliadoBO>();
            List<IngresoEquipoAliadoBO> ingresosEquiposAliados = new List<IngresoEquipoAliadoBO>();
            var unidad = mantenimiento.IngresoUnidad.Unidad;
            if (unidad.EquiposAliados != null && unidad.EquiposAliados.Count > 0)
            {
                unidad.EquiposAliados.ForEach(ea =>
                {
                    IngresoEquipoAliadoBO child = new IngresoEquipoAliadoBO();

                    child.FechaIngreso = DateTime.Now;
                    child.EquipoAliado = ea;

                    MantenimientoEquipoAliadoBO mantenimientoEquipoAliado = new MantenimientoEquipoAliadoBO();
                    mantenimientoEquipoAliado.IngresoEquipoAliado = child;
                    mantenimientoEquipoAliado.MantenimientoProgramado = this.consultarMantenimientoProgramadoBR.ConsultarUltimoMantenimientoProgramado(this.dataContext, ea.EquipoID, false, true);
                    ingresosEquiposAliados.Add(child);

                    mantenimientosEquiposAliados.Add(mantenimientoEquipoAliado);
                });

                mantenimiento.IngresoUnidad.IngresoEquiposAliados = ingresosEquiposAliados;
                mantenimiento.MantenimientoEquiposAliados = mantenimientosEquiposAliados;
            }
        }
        
        /// <summary>
        /// Muestra las unidades consultadas
        /// </summary>
        public void CargarGridViewUnidades() 
        {
            this.vista.GvUnidadesSeleccion.DataSource = this.vista.ResultadoBusquedaUnidades;
            this.vista.GvUnidadesSeleccion.DataBind();
        }

        /// <summary>
        /// Agrega la unidad seleccionada del buscador al grid
        /// </summary>
        public void EstablecerUnidadSeleccion(string numeroSerie)
        {
            List<SucursalBO> sucursales = new List<SucursalBO>();
            List<ClienteBO> clientes = new List<ClienteBO>();

            var unidad = this.vista.ResultadoBusquedaUnidades.FirstOrDefault(x => x.NumeroSerie == numeroSerie);
            if (unidad != null)
            {
                GridViewRow row = this.vista.RowEnSesion;
                String guid = ((Label)row.FindControl("lblGuid")).Text;
                MantenimientoUnidadBO mantenimientoActual = this.GetByGuid(guid).MantenimientoUnidad;
                if (unidad.Modelo == null && unidad.ActaNacimiento != string.Empty)
                    unidad.Modelo.Id = new EquiposBO.UnidadBO(unidad.ActaNacimiento).Modelo.Id;
                this.ComplementarDatosUnidad(unidad, sucursales, clientes);
                bool? esUltimoMantto = mantenimientoActual.FechaSalida == null && mantenimientoActual.FechaProximoServicio == null;

                mantenimientoActual.IngresoUnidad.FechaIngreso = DateTime.Now;
                mantenimientoActual.IngresoUnidad.Unidad = unidad;
                mantenimientoActual.IngresoUnidad.NumeroEconomico = unidad.NumeroEconomico;
                mantenimientoActual.IngresoUnidad.Vin = unidad.NumeroSerie;
                mantenimientoActual.MantenimientoProgramado = this.consultarMantenimientoProgramadoBR.ConsultarUltimoMantenimientoProgramado(this.dataContext, unidad.EquipoID, true, esUltimoMantto);
                this.CargarIngresoAliados(mantenimientoActual);
                mantenimientoActual.IngresoUnidad.Controlista = this.ConsultarUsuario(new UsuarioBO() { Id = this.vista.UsuarioAutenticado });

                this.AgregarFila(mantenimientoActual, guid);
                this.CargarDatos();
            }
            else
                throw new Exception("No se encontró la unidad con el Número de Serie " + numeroSerie + " dentro de los datos consultados.");            
        }
        
        /// <summary>
        /// Consulta el nombre del operador mediante su ID
        /// </summary>
        /// <param name="operadorID">Identificador del operador</param>
        /// <returns>Nombre del operador</returns>
        private UsuarioBO ConsultarUsuario(UsuarioBO usuario)
        {
            return this.usuarioBR.Consultar(dataContext, usuario).ConvertAll(item => (UsuarioBO)item).FirstOrDefault();
        }

        /// <summary>
        /// Agrega una con un mantenimiento creado
        /// </summary>
        /// <param name="mantenimiento"></param>
        /// <param name="guid"></param>
        private void AgregarFila(MantenimientoUnidadBO mantenimiento, string guid)
        {
            MantenimientoBOF posicion = this.vista.Mantenimientos.Where(x => x.Guid == guid).FirstOrDefault();
            var index = this.vista.Mantenimientos.IndexOf(posicion);

            List<MantenimientoBOF> detalle = new List<MantenimientoBOF>();
            foreach (var aliado in mantenimiento.MantenimientoEquiposAliados)
            {
                MantenimientoBOF aliadoBof = new MantenimientoBOF();
                aliadoBof.Guid = this.GetGuid();
                aliadoBof.MantenimientoUnidad = mantenimiento;
                aliadoBof.MantenimientoAliado = aliado;
                detalle.Add(aliadoBof);
            }

            posicion.Detalles = detalle;

            if (MarcaInternationalID != null)
            {

                if (mantenimiento.IngresoUnidad.Unidad.Modelo.Marca.Id != null
                    && mantenimiento.IngresoUnidad.Unidad.Modelo.Marca.Id.Equals(this.MarcaInternationalID))
                {
                    posicion.LeerInline = this.vista.LeerInLine != null ? this.vista.LeerInLine.Value : false;
                }
            }
            else
                posicion.LeerInline = false;

            posicion.MantenimientoUnidad = mantenimiento;

            this.vista.Mantenimientos[index] = posicion;

            this.CargarDatos();
        }
        
        /// <summary>
        /// Agrega una fila vacía al grid
        /// </summary>
        public void AgregarNuevaFila()
        {
            MantenimientoBOF mantenimiento = new MantenimientoBOF();
            mantenimiento.Guid = this.GetGuid();

            this.vista.Mantenimientos.Add(mantenimiento);
            this.CargarDatos();
        }
        #endregion

        #region Filtros Mantenimientos
        /// <summary>
        /// Muestra los mantenimientos en el grid
        /// </summary>
        public void CargarDatos()
        {
            this.vista.GvMantenimientos.DataSource = this.vista.Mantenimientos;
            this.vista.GvMantenimientos.DataBind();
            this.vista.CrearCabeceraComplemento();
        }
        /// <summary>
        /// Realiza la busqueda de mantenimientos segun los filtros establecidos
        /// </summary>
        public void CargarMantenimientos()
        {
            var tallerId = this.vista.TallerID;
            var fechaInicio = this.vista.FechaInicio;
            var fechaFin = this.vista.FechaFin;

            if (tallerId == null || fechaInicio == null || fechaFin == null)
            {
                this.vista.MostrarMensaje("Favor de verificar los campos obligatorios", ETipoMensajeIU.ADVERTENCIA, null);
            }
            else
            {
                MantenimientoUnidadBO mantenimiento = new MantenimientoUnidadBO();
                mantenimiento.OrdenServicio = new OrdenServicioBO();
                mantenimiento.IngresoUnidad = new IngresoUnidadBO();

                mantenimiento.Taller.Id = tallerId;
                mantenimiento.Activo = true;
                mantenimiento.OrdenServicio.Id = this.vista.OrdenServicioID;
                mantenimiento.IngresoUnidad.Vin = this.vista.NumeroSerie;

                if (fechaInicio != null)
                    mantenimiento.IngresoUnidad.FechaIngreso = fechaInicio;
                if (fechaFin != null)
                    mantenimiento.FechaSalida = fechaFin;

                this.ConsultarIngresosPorTaller(mantenimiento);
            }
        }
                
        /// <summary>
        /// Consulta los ingresos de unidades por taller
        /// </summary>
        /// <param name="mantenimiento">Contiene los filtros aplicados para la busqueda</param>
        /// <returns></returns>
        private void ConsultarIngresosPorTaller(MantenimientoUnidadBO mantenimiento)
        {
            List<MantenimientoUnidadBO> mantenimientos = new List<MantenimientoUnidadBO>();
            var sucursal = this.vista.SucursalID;
            try
            {
                //Consulta de Mantenimientos
                var result = this.mantenimientoUnidadBR.Consultar(dataContext, mantenimiento);
                if (result != null && result.Count > 0)
                {
                    var estatus = this.vista.Estatus;
                    if (estatus == 0) //Filtramos los mantenimimentos que no tengan OrdenServicio.Id, ya que solo estan ingresadas
                    {
                        var filtered = result.Where(item => item.OrdenServicio.Id == null && item.Sucursal.Id == sucursal).ToList();
                        result = filtered;
                    }
                    else
                    {
                        var filtered = result.Where(item => item.OrdenServicio.Id != null).ToList();
                        result = filtered;
                    }

                    List<SucursalBO> sucursales = new List<SucursalBO>();
                    List<ClienteBO> clientes = new List<ClienteBO>();
                    List<UsuarioBO> usuarios = new List<UsuarioBO>();

                    foreach (MantenimientoUnidadBO mantUnidadBO in result)
                    {
                        if (estatus == 0)
                        {
                            goto Consultar;
                        }
                        else
                        {
                            OrdenServicioBO os = FacadeBR.ConsultarOrdenServicio(dataContext, mantUnidadBO.OrdenServicio).FirstOrDefault();
                            if (os != null && os.Estatus.Id == estatus && os.AdscripcionServicio.Sucursal.Id == sucursal)
                            {
                                mantUnidadBO.OrdenServicio = os;
                                goto Consultar;
                            }
                            else
                                continue;
                        }

                    Consultar:
                        {
                            Equipos.BO.UnidadBO unidad = this.unidadBR.Consultar(dataContext, new Equipos.BO.UnidadBO() { UnidadID = mantUnidadBO.IngresoUnidad.Unidad.UnidadID }).ConvertAll(x => (Equipos.BO.UnidadBO)x).FirstOrDefault();
                            this.ComplementarDatosUnidad(unidad, sucursales, clientes); 

                            mantUnidadBO.IngresoUnidad.Unidad = unidad;
                            mantUnidadBO.IngresoUnidad.Vin = unidad.NumeroSerie;
                            mantUnidadBO.IngresoUnidad.NumeroEconomico = unidad.NumeroEconomico;
                            mantUnidadBO.MantenimientoEquiposAliados = new List<MantenimientoEquipoAliadoBO>();

                            mantUnidadBO.IngresoUnidad.Controlista = FacadeBR.ConsultarUsuario(dataContext, mantUnidadBO.IngresoUnidad.Controlista).FirstOrDefault();
                           
                            bool ultimoManttoUnidad = mantUnidadBO.FechaSalida == null && mantUnidadBO.FechaProximoServicio == null;

                            foreach (EquipoAliadoBO equipoAliado in unidad.EquiposAliados)
                            {
                                MantenimientoEquipoAliadoBO filter = new MantenimientoEquipoAliadoBO();
                                filter.IngresoEquipoAliado = new IngresoEquipoAliadoBO();
                                filter.IngresoEquipoAliado.EquipoAliado = equipoAliado;

                                MantenimientoEquipoAliadoBO mantEquipoAliado = this.mantenimientoAliadoBR.Consultar(dataContext, filter, mantUnidadBO.MantenimientoUnidadId).FirstOrDefault();
                       
                            
                                bool ingresoEAliado = false;
                                //Validamos si se realizo el registro de ingreso
                                if (mantEquipoAliado != null && mantEquipoAliado.MantenimientoEquipoAliadoId != null)
                                {
                                    mantEquipoAliado.IngresoEquipoAliado.EquipoAliado = equipoAliado;
                                    ingresoEAliado = true;
                                }
                                else // Si no se realizo el registro cargamos un pre-registro
                                {
                                    IngresoEquipoAliadoBO ingresoEquipoAliado = new IngresoEquipoAliadoBO();

                                    ingresoEquipoAliado.FechaIngreso = DateTime.Now;
                                    ingresoEquipoAliado.EquipoAliado = equipoAliado;

                                    mantEquipoAliado = new MantenimientoEquipoAliadoBO();
                                    mantEquipoAliado.IngresoEquipoAliado = ingresoEquipoAliado;
                                    mantEquipoAliado.Auditoria = mantUnidadBO.Auditoria;
                                }
                                bool? ultimoManttEAliado = null;
                                if (ingresoEAliado)
                                {
                                    ultimoManttEAliado = mantEquipoAliado.FechaSalida == null && mantEquipoAliado.FechaProximoServicio == null;
                                }

                                if (estatus != 0)
                                {
                                    OrdenServicioBO os = new OrdenServicioBO();
                                    if (mantEquipoAliado.OrdenServicio != null && mantEquipoAliado.OrdenServicio.Id != null) 
                                    os = FacadeBR.ConsultarOrdenServicio(dataContext, mantEquipoAliado.OrdenServicio).FirstOrDefault();
                                    if (os != null && os.Id != null)
                                    {
                                        if (os.Estatus.Id == estatus)
                                        {
                                            mantEquipoAliado.OrdenServicio = os;
                                        }
                                        
                                    }
                                }

                                var ManteProgram = this.consultarMantenimientoProgramadoBR.ConsultarUltimoMantenimientoProgramado(this.dataContext, equipoAliado.EquipoID, false, ultimoManttEAliado);
                                if (ManteProgram != null && ManteProgram.MantenimientoProgramadoID != null)
                                {
                                    if (ManteProgram.EstatusMantenimientoProgramado == EEstatusMantenimientoProgramado.PROGRAMADO && ManteProgram.Auditoria.FC >= mantEquipoAliado.Auditoria.FC && ManteProgram.Auditoria.FC.Value.Second > mantEquipoAliado.Auditoria.FC.Value.Second)
                                    {
                                        mantEquipoAliado.MantenimientoProgramado = ManteProgram;
                                    }
                                    
                                }
                                //mantEquipoAliado.MantenimientoProgramado = this.consultarMantenimientoProgramadoBR.ConsultarUltimoMantenimientoProgramado(this.dataContext, equipoAliado.EquipoID, false, ultimoManttEAliado);
                                mantUnidadBO.MantenimientoEquiposAliados.Add(mantEquipoAliado);
                            }
                            var ManteProgramAlia = this.consultarMantenimientoProgramadoBR.ConsultarUltimoMantenimientoProgramado(this.dataContext, mantUnidadBO.IngresoUnidad.Unidad.EquipoID, true, ultimoManttoUnidad);
                            if (ManteProgramAlia != null && ManteProgramAlia.MantenimientoProgramadoID != null)
                            {
                                if (ManteProgramAlia.EstatusMantenimientoProgramado == EEstatusMantenimientoProgramado.PROGRAMADO && ManteProgramAlia.Auditoria.FC >= mantUnidadBO.Auditoria.FC && mantUnidadBO.Auditoria.FC.Value.Second > mantUnidadBO.Auditoria.FC.Value.Second)
                                {
                                    mantUnidadBO.MantenimientoProgramado = ManteProgramAlia;
                                }

                            }
                            //mantUnidadBO.MantenimientoProgramado = this.consultarMantenimientoProgramadoBR.ConsultarUltimoMantenimientoProgramado(this.dataContext, mantUnidadBO.IngresoUnidad.Unidad.EquipoID, true, ultimoManttoUnidad);
                             mantenimientos.Add(mantUnidadBO);
                            
                        }
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }

            this.vista.Mantenimientos = this.MapToBof(mantenimientos);
            this.CargarDatos();
        }

        private List<MantenimientoBOF> MapToBof(List<BO.MantenimientoUnidadBO> mantenimientos)
        {
            List<MantenimientoBOF> result = new List<MantenimientoBOF>();
            foreach (var mantenimiento in mantenimientos)
            {
                MantenimientoBOF mantenimientoBof = new MantenimientoBOF();
                mantenimientoBof.Guid = this.GetGuid();
                mantenimientoBof.MantenimientoUnidad = mantenimiento;

                List<MantenimientoBOF> detalle = new List<MantenimientoBOF>();
                foreach (var aliado in mantenimiento.MantenimientoEquiposAliados)
                {
                    MantenimientoBOF aliadoBof = new MantenimientoBOF();
                    aliadoBof.Guid = this.GetGuid();
                    aliadoBof.MantenimientoUnidad = mantenimiento;
                    aliadoBof.MantenimientoAliado = aliado;
                    detalle.Add(aliadoBof);
                }

                mantenimientoBof.Detalles = detalle;

                if (mantenimiento.IngresoUnidad.Unidad.Modelo.Marca.Id != null && mantenimiento.IngresoUnidad.Unidad.Modelo.Marca.Id.Equals(this.MarcaInternationalID))
                {
                    mantenimientoBof.LeerInline = this.vista.LeerInLine != null ? this.vista.LeerInLine.Value : false;
                }
                else
                    mantenimientoBof.LeerInline = false;

                result.Add(mantenimientoBof);
            }

            return result;
        }
        
        /// <summary>
        /// Consulta talleres por sucursal
        /// </summary>
        /// <param name="sucuralID"></param>
        /// <returns></returns>
        public List<AdscripcionServicioBO> ConsultarTalleres(AdscripcionBO adscripcion)
        {
            return FacadeBR.ConsultarTalleresDestinoPorSucursal(dataContext, adscripcion);
        }
        #endregion

        #region Ingresar Equipos
        /// <summary>
        /// Ingresa un equipo al taller de mantenimiento
        /// </summary>
        public void IngresarUnidad()
        {
            GridViewRow row = this.vista.RowEnSesion;
            string guid = ((Label)row.FindControl("lblGuid")).Text;
            string controlista = ((Label)row.FindControl("lblControlista")).Text;
            var mantenimiento = this.GetByGuid(guid).MantenimientoUnidad;

            if (mantenimiento.MantenimientoUnidadId == null)
            {
                this.EstableceValoresIngresoUnidad(mantenimiento, row);
                this.InsertarIngresoUnidad(mantenimiento);
                var Mtto = this.vista.Mantenimientos.Find(x => x.Guid == guid);
                Mtto.MantenimientoUnidad.IngresoUnidad.Controlista.NombreCorto = controlista;
                this.CargarDatos();
            }
            else
            {
                this.vista.MostrarMensaje("La unidad ha sido registrada con anterioridad", ETipoMensajeIU.ADVERTENCIA, null);
            }
        }
        public void IngresarEquipoAliado()
        {
            GridViewRow row = this.vista.RowEnSesion;
            string guid = ((Label)row.FindControl("lblAliadoGuid")).Text;
            var mantenimiento = this.GetAliadoByGuid(guid).MantenimientoUnidad;
            if (mantenimiento.MantenimientoUnidadId == null)
            {
                this.vista.MostrarMensaje("Es necesario registrar la unidad para continuar", ETipoMensajeIU.ADVERTENCIA, null);
            }
            else
            {
                var equipoAliado = this.GetAliadoByGuid(guid).MantenimientoAliado;
                if (equipoAliado.MantenimientoEquipoAliadoId == null)
                {
                    this.EstableceValoresIngresoEquipoAliado(equipoAliado, row);
                    this.InsertarIngresoEquipoAliado(equipoAliado, mantenimiento.MantenimientoUnidadId);
                    this.CargarDatos();
                }
                else
                {
                    this.vista.MostrarMensaje("El equipo ha sido registrado con anterioridad", ETipoMensajeIU.ADVERTENCIA, null);
                }
            }
        }
        public bool ValidarIngresoUnidad()
        {
            bool result = false;
            GridViewRow row = this.vista.RowEnSesion;

            string guid = ((Label)row.FindControl("lblGuid")).Text;
            MantenimientoBOF mantenimiento = this.GetByGuid(guid);

            if (mantenimiento.MantenimientoUnidad.IngresoUnidad.Unidad.UnidadID == null)
            {
                this.vista.MostrarMensaje("Es necesario establecer la unidad para poder continuar.", ETipoMensajeIU.ADVERTENCIA, null);
                result = false;
            }
            else
            {
                Label lblControlista = row.FindControl("lblControlista") as Label;
                if (lblControlista.Text.Trim().Length < 1)
                {
                    this.vista.MostrarMensaje("Es necesario establecer al controlista para poder continuar.", ETipoMensajeIU.ADVERTENCIA, null);
                    result = false;
                }
                else
                {
                    TextBox txtOperador = row.FindControl("txtOperador") as TextBox;
                    if (txtOperador.Text.Trim().Length < 1)
                    {
                        this.vista.MostrarMensaje("Es necesario establecer el operador para poder continuar.", ETipoMensajeIU.ADVERTENCIA, null);
                        result = false;
                    }
                    else
                    {
                        TextBox txtObservaciones = row.FindControl("txtObservaciones") as TextBox;
                        if (txtObservaciones.Text.Trim().Length < 1)
                        {
                            this.vista.MostrarMensaje("Es necesario establecer las observaciones para poder continuar.", ETipoMensajeIU.ADVERTENCIA, null);
                            result = false;
                        }
                        else
                        {
                            result = true;
                        }
                    }
                }
            }

            return result;
        }
        public bool ValidarIngresoEquipoAliado()
        {
            bool result = false;
            GridViewRow row = this.vista.RowEnSesion;

            string guid = ((Label)row.FindControl("lblAliadoGuid")).Text;
            MantenimientoBOF mantenimiento = this.GetAliadoByGuid(guid);

            if (mantenimiento.MantenimientoUnidad.MantenimientoUnidadId == null)
            {
                this.vista.MostrarMensaje("Es necesario ingresar la unidad para poder continuar.", ETipoMensajeIU.ADVERTENCIA, null);
                result = false;
            }
            else
            {
                TextBox txtObservaciones = row.FindControl("txtEObservaciones") as TextBox;
                if (txtObservaciones.Text.Trim().Length < 1)
                {
                    this.vista.MostrarMensaje("Es necesario establecer las observaciones para poder continuar.", ETipoMensajeIU.ADVERTENCIA, null);
                    result = false;
                }
                else
                {
                    result = true;
                }
            }

            return result;
        }
        private void EstableceValoresIngresoUnidad(MantenimientoUnidadBO mantenimiento, GridViewRow row)
        {
            if (this.vista.DdTalleres.SelectedItem != null)
            {
                mantenimiento.Taller.Id = Convert.ToInt32(this.vista.DdTalleres.SelectedItem.Value);
                mantenimiento.Sucursal.Id = this.vista.SucursalID;
                TextBox txtObservaciones = row.FindControl("txtObservaciones") as TextBox;
                TextBox txtOperador = row.FindControl("txtOperador") as TextBox;
                Label lblControlista = row.FindControl("lblControlista") as Label;

                mantenimiento.IngresoUnidad.ObservacionesOperador = txtObservaciones.Text;
                mantenimiento.IngresoUnidad.Operador = txtOperador.Text;
                mantenimiento.IngresoUnidad.Controlista.NombreCorto = lblControlista.Text;
            }
        }
        private void EstableceValoresIngresoEquipoAliado(MantenimientoEquipoAliadoBO mantenimiento, GridViewRow row)
        {
            if (this.vista.DdTalleres.SelectedItem != null)
            {
                mantenimiento.Taller.Id = Convert.ToInt32(this.vista.DdTalleres.SelectedItem.Value);
                TextBox txtObservaciones = row.FindControl("txtEObservaciones") as TextBox;

                mantenimiento.IngresoEquipoAliado.ObservacionesOperador = this.ClearValue(txtObservaciones.Text);
            }
        }
        public void InsertarIngresoUnidad(MantenimientoUnidadBO mantenimiento)
        {
            if (mantenimiento.MantenimientoUnidadId == null)
            {
                if (mantenimiento.Taller.Id == null)
                {
                    this.vista.MostrarMensaje("Es necesario seleccionar un taller", ETipoMensajeIU.ERROR, null);
                }
                else
                {
                    try
                    {

                        this.mantenimientoUnidadBR.InsertarCompleto(dataContext, mantenimiento, new SeguridadBO(Guid.NewGuid(), new UsuarioBO() { Id = this.vista.UC }, new AdscripcionBO { UnidadOperativa = new UnidadOperativaBO() { Id = this.vista.UnidadOperativaId } }));
                        this.vista.MostrarMensaje("Registro Exitoso", ETipoMensajeIU.EXITO, null);
                    }
                    catch (Exception ex)
                    {
                        this.vista.MostrarMensaje("Error al guardar", ETipoMensajeIU.ERROR, ex.Message);
                    }
                }
            }
            else
            {
                this.vista.MostrarMensaje("La unidad ya ha sido registrada con anterioridad", ETipoMensajeIU.ADVERTENCIA, null);
            }

        }

        #region Determinar si la unidad esta en un contacto

        /// <summary>
        /// Determina si la unidad o equipo aliado se encuentran en contrato.
        /// </summary>
        /// <param name="dctx">El datacontext que proporciona el acceso a la base de datos</param>
        /// <param name="unidad">Unidad a revisar</param>
        /// <param name="equipoAliado">Equipo aliado a revisar</param>
        /// <returns>True si la unidad o equipo aliado se encuentran en un contrato</returns>
        private bool? EnContrato(IDataContext dctx, Equipos.BO.UnidadBO unidad, Equipos.BO.EquipoAliadoBO equipoAliado)
        {
            try
            {
                if (unidad == null && equipoAliado == null)
                    throw new Exception("La unidad o equipo aliado no pueden ser nulos");
                if (unidad != null && unidad.UnidadID == null && equipoAliado == null)
                    throw new Exception("El identificador de la unidad no puede ser nulo");
                if (unidad == null && equipoAliado != null && equipoAliado.EquipoAliadoID == null)
                    throw new Exception("El identificador del equipo aliado no puede ser nulo");

                int? unidadId = unidad == null ? null : unidad.UnidadID;
                int? equipoAliadoId = equipoAliado == null ? null : equipoAliado.EquipoAliadoID;

                return new MantenimientoBR().ConsultarUsoEquipo(dctx, unidadId, equipoAliadoId);

            }
            catch (Exception ex)
            {
                throw new Exception("No se pudo determinar si la unidad/equipo aliado se encuentra en un contrato", ex.InnerException);
            }
        }

        #endregion
        #region Determinar si la unidad esta en un contacto pendiente por cerrar.
        /// <summary>
        /// Determina si la unidad o equipo aliado se encuentran en contrato pendiente por cerrar.
        /// </summary>
        /// <param name="dctx">El datacontext que proporciona el acceso a la base de datos</param>
        /// <param name="unidad">Unidad a revisar</param>
        /// <param name="equipoAliado">Equipo aliado a revisar</param>
        /// <param name="pendientePorCerrar">Para saber si es pendiente por cerrar.</param>
        /// <returns>True si la unidad o equipo aliado se encuentran en un contrato</returns>
        private bool? EnContratoPendientePorCerrar(IDataContext dctx, Equipos.BO.UnidadBO unidad, Equipos.BO.EquipoAliadoBO equipoAliado, bool? pendientePorCerrar)
        {
            try
            {
                if (unidad == null && equipoAliado == null)
                    throw new Exception("La unidad o equipo aliado no pueden ser nulos");
                if (unidad != null && unidad.UnidadID == null && equipoAliado == null)
                    throw new Exception("El identificador de la unidad no puede ser nulo");
                if (unidad == null && equipoAliado != null && equipoAliado.EquipoAliadoID == null)
                    throw new Exception("El identificador del equipo aliado no puede ser nulo");

                int? unidadId = unidad == null ? null : unidad.UnidadID;
                int? equipoAliadoId = equipoAliado == null ? null : equipoAliado.EquipoAliadoID;

                return new MantenimientoBR().ConsultarUsoEquipoPendientePorCerrar(dctx, unidadId, equipoAliadoId, pendientePorCerrar);
            }
            catch (Exception ex)
            {
                throw new Exception("No se pudo determinar si la unidad/equipo aliado se encuentra en un contrato", ex.InnerException);
            }
        }
        #endregion


        /// <summary>
        /// Cancela el ingreso de una unidad
        /// </summary>
        /// <param name="mantenimiento"></param>
        private bool CancelarIngresoUnidad(MantenimientoUnidadBO mantenimiento)
        {
            bool cancelado = false;
            try
            {
                var seguridad = new SeguridadBO(Guid.NewGuid(), new UsuarioBO() { Id = this.vista.UC }, new AdscripcionBO { UnidadOperativa = new UnidadOperativaBO() { Id = this.vista.UnidadOperativaId } });
                
               
                BPMO.SDNI.Equipos.BO.UnidadBO unidadNueva = this.unidadBR.ConsultarCompleto(dataContext, new BPMO.SDNI.Equipos.BO.UnidadBO() { UnidadID = mantenimiento.IngresoUnidad.Unidad.UnidadID }).FirstOrDefault();
                BPMO.SDNI.Equipos.BO.UnidadBO unidadAnterior = new BPMO.SDNI.Equipos.BO.UnidadBO(unidadNueva);
                if (unidadNueva != null)
                {
                    
                    Hashtable parameters = new Hashtable();
                    parameters["Observaciones"] = "CANCELACION DE INGRESO DE LA UNIDAD DEL TALLER DE MANTENIMIENTO";
                    if (this.EnContrato(dataContext,unidadAnterior,null) == true)
                    {
                        if(this.EnContratoPendientePorCerrar(dataContext,unidadAnterior,null,true) == true)
                            unidadNueva.EstatusActual = EEstatusUnidad.Entregada;
                        else
                            unidadNueva.EstatusActual = EEstatusUnidad.ConCliente;
                    }
                    else
                    {
                        unidadNueva.EstatusActual = EEstatusUnidad.Disponible;
                    }
                    unidadBR.ActualizarUnidad(dataContext, unidadNueva, unidadAnterior, EMovimiento.SALIDA_DE_MANTENIMIENTO, parameters, seguridad);
                    this.mantenimientoUnidadBR.Actualizar(dataContext, mantenimiento, seguridad);
                }
                cancelado = true;
                this.vista.MostrarMensaje("Ingreso Cancelado", ETipoMensajeIU.EXITO, null);
            }
            catch (Exception ex)
            {
                this.vista.MostrarMensaje("Error al cancelar el ingreso", ETipoMensajeIU.ERROR, ex.Message);
            }

            return cancelado;
        }

        public void InsertarIngresoEquipoAliado(MantenimientoEquipoAliadoBO mantenimiento, int? mantenimientoUnidadId)
        {
            if (mantenimiento.MantenimientoEquipoAliadoId == null)
            {
                try
                {
                    var seguridad = new SeguridadBO(Guid.Empty, new UsuarioBO() { Id = this.vista.UC }, new AdscripcionBO { UnidadOperativa = new UnidadOperativaBO() { Id = this.vista.UnidadOperativaId } });
                    this.mantenimientoUnidadBR.Insertar(dataContext, mantenimiento, mantenimientoUnidadId, seguridad);
                    this.vista.MostrarMensaje("Registro Exitoso", ETipoMensajeIU.EXITO, null);
                }
                catch (Exception ex)
                {
                    this.vista.MostrarMensaje("Error al guardar", ETipoMensajeIU.ERROR, ex.Message);
                }
            }
            else
            {
                this.vista.MostrarMensaje("La unidad ya ha sido registrada con anterioridad", ETipoMensajeIU.ADVERTENCIA, null);
            }
        }
        public void ActualizarIngresoEquipoAliado(MantenimientoEquipoAliadoBO mantenimiento)
        {
            try
            {
                var seguridad = new SeguridadBO(Guid.Empty, new UsuarioBO() { Id = this.vista.UC }, new AdscripcionBO { UnidadOperativa = new UnidadOperativaBO() { Id = this.vista.UnidadOperativaId } });
                this.mantenimientoUnidadBR.Actualizar(dataContext, mantenimiento, seguridad);
                this.vista.MostrarMensaje("Registro Exitoso", ETipoMensajeIU.EXITO, null);
            }
            catch (Exception ex)
            {
                this.vista.MostrarMensaje("Error al guardar", ETipoMensajeIU.ERROR, ex.Message);
            }
        }
        public void CancelarIngreso()
        {
            int countSeleccionados = 0;
            GridViewRow rowSeleccionado = null;
            foreach (GridViewRow row in this.vista.GvMantenimientos.Rows)
            {
                if (row.RowType == DataControlRowType.DataRow)
                {
                    CheckBox chkRow = (row.Cells[0].FindControl("chkSeleccionar") as CheckBox);
                    if (chkRow.Checked)
                    {
                        countSeleccionados += 1;
                    }
                }
            }

            if (countSeleccionados > 1)
            {
                this.vista.MostrarMensaje("No se permite selección de múltiples registros al cancelar.", ETipoMensajeIU.ADVERTENCIA, null);
            }
            else
            {
                foreach (GridViewRow row in this.vista.GvMantenimientos.Rows)
                {
                    if (row.RowType == DataControlRowType.DataRow)
                    {
                        CheckBox chkRow = (row.Cells[0].FindControl("chkSeleccionar") as CheckBox);
                        if (chkRow.Checked)
                        {
                            rowSeleccionado = row;
                        }
                    }
                }

                if (rowSeleccionado != null)
                {
                    string guid = (rowSeleccionado.Cells[0].FindControl("lblGuid") as Label).Text;
                    MantenimientoUnidadBO mantenimiento = this.GetByGuid(guid).MantenimientoUnidad;
                    if (mantenimiento.OrdenServicio.Id == null)
                    {
                        if (mantenimiento.MantenimientoUnidadId == null)
                        {
                            this.RemoveByGuid(guid);
                        }
                        else
                        {
                            mantenimiento.Activo = false;
                            mantenimiento.MotivoCancelacion = this.vista.MotivoCancelacion;
                            var cancelado = this.CancelarIngresoUnidad(mantenimiento);
                            if (cancelado == true)
                                this.RemoveByGuid(guid);
                        }

                        this.CargarDatos();
                    }
                    else
                    {
                        this.vista.MostrarMensaje("El registro no puede ser cancelado cuando ya ha iniciado el mantenimiento", ETipoMensajeIU.ADVERTENCIA, null);
                    }
                }
            }
        }
        #endregion
        
        #region Ordenes de Servicio
        public bool ValidarInicioMantenimientoUnidad()
        {
            bool result = false;
            GridViewRow row = this.vista.RowEnSesion;
            string mensajeAdvertencia = "";

            Label lblId = row.FindControl("lblId") as Label;
            Label capacidadTanque = row.FindControl("lblCapacidadTanque") as Label;
            if (String.IsNullOrEmpty(capacidadTanque.Text) && this.vista.UnidadOperativaId == (int)ETipoEmpresa.Idealease)
            {
                mensajeAdvertencia += "No se encuentra la capacidad del tanque de la unidad.";
            }
            if (string.IsNullOrEmpty(lblId.Text))
            {
                mensajeAdvertencia += " Es necesario registrar el ingreso de la unidad para poder continuar.";
                this.vista.MostrarMensaje(mensajeAdvertencia, ETipoMensajeIU.ADVERTENCIA, null);
            }
            else
            {
                Label lblOS = row.FindControl("lblOrdenServicio") as Label;
                if (string.IsNullOrEmpty(lblOS.Text))
                {
                    string guid = ((Label)row.FindControl("lblGuid")).Text;
                    MantenimientoBOF mantenimiento = this.GetByGuid(guid);
                    if (mantenimiento.LeerInline)
                    {
                        result = this.ValidarEquipoInternacional(row);
                    }
                    else
                    {
                        result = this.validarHorasKmEntrada(row, "lblKmCalc", "lblHorometroCalc");
                    }
                }
                else
                {
                    mensajeAdvertencia += " La Orden de Servicio ya ha sido creada para la Unidad seleccionada.";
                    this.vista.MostrarMensaje(mensajeAdvertencia, ETipoMensajeIU.ADVERTENCIA, null);
                }
            }
            if (mensajeAdvertencia.Length > 0 && mensajeAdvertencia.Length < 54)
                this.vista.MostrarMensaje(mensajeAdvertencia, ETipoMensajeIU.ADVERTENCIA, null);
            
            return result;
        }
        public bool ValidarInicioMantenimientoAliado()
        {
            bool result = false;
            GridViewRow row = this.vista.RowEnSesion;

            string guid = ((Label)row.FindControl("lblAliadoGuid")).Text;
            var mantAliado = this.GetAliadoByGuid(guid);
            if (mantAliado.MantenimientoUnidad.OrdenServicio.Id != null)
            {
                Label lblEId = row.FindControl("lblEId") as Label;
                if (string.IsNullOrEmpty(lblEId.Text))
                {
                    this.vista.MostrarMensaje("Es necesario registrar el ingreso del equipo aliado para poder continuar.", ETipoMensajeIU.ADVERTENCIA, null);
                }
                else
                {
                    Label lblOS = row.FindControl("lblEOrdenServicio") as Label;
                    if (string.IsNullOrEmpty(lblOS.Text))
                    {
                        if (this.validarHorasKmEntrada(row, "txtEKmCalc", "txtEHorometroCalc"))
                        {
                            result = true;
                        }
                    }
                    else
                    {
                        this.vista.MostrarMensaje("La Orden de Servicio ya ha sido creada para el equipo seleccionado", ETipoMensajeIU.ADVERTENCIA, null);
                    }
                }
            }
            else
            {
                this.vista.MostrarMensaje("La orden de servicio de la unidad no ha sido creada. Favor de verificar", ETipoMensajeIU.ADVERTENCIA, null);
            }

            return result;
        }
        private bool ValidarEquipoInternacional(GridViewRow row)
        {
            bool result = true;
            string guid = ((Label)row.FindControl("lblGuid")).Text;
            string serie = ((TextBox)row.FindControl("txtVin")).Text;

            try
            {
                int km = this.LeerArchivo(TipoLectura.KM, serie);
                int hrs = this.LeerArchivo(TipoLectura.HORAS, serie);
                this.CalcularMantenimientoReal(ETipoEquipo.Unidad, guid, km, hrs);
                this.CargarDatos();
            }
            catch (Exception ex)
            {
                this.vista.MostrarMensaje("Error de lectura", ETipoMensajeIU.ERROR, ex.Message);
                result = false;
            }

            return result;
        }
        public void CalcularMantenimientoReal(ETipoEquipo tipoEquipo, string guid, int? km = null, int? hrs = null)
        {
            switch (tipoEquipo)
            {
                case ETipoEquipo.Unidad:
                    MantenimientoUnidadBO mantenimientoUnidad = this.GetByGuid(guid).MantenimientoUnidad;
                    if (km.HasValue)
                    {
                        mantenimientoUnidad.KilometrajeEntrada = km.Value;
                    }
                    if (hrs.HasValue)
                    {
                        mantenimientoUnidad.HorasEntrada = hrs.Value;
                    }
                    mantenimientoUnidad.TipoMantenimiento = this.mantenimientoBR.CalcularMantenimimentoReal(dataContext, mantenimientoUnidad);
                    break;
                case ETipoEquipo.EquipoAliado:
                    MantenimientoUnidadBO mantenimientoUnidadBOF = this.GetAliadoByGuid(guid).MantenimientoUnidad;
                    if (mantenimientoUnidadBOF != null && mantenimientoUnidadBOF.MantenimientoEquiposAliados != null)
                    {
                        MantenimientoEquipoAliadoBO mantenimientoAliado = this.GetAliadoByGuid(guid).MantenimientoAliado;
                        if (km.HasValue)
                        {
                            mantenimientoAliado.KilometrajeEntrada = km.Value;
                        }
                        if (hrs.HasValue)
                        {
                            mantenimientoAliado.HorasEntrada = hrs.Value;
                        }
                        mantenimientoAliado.TipoMantenimiento = this.mantenimientoBR.CalcularMantenimimentoReal(dataContext, mantenimientoAliado);
                    }
                    break;
            }
        }
        /// <summary>
        /// Registra una orden de servicio para una unidad o para un equipo aliado
        /// </summary>
        /// <param name="tipoEquipo"></param>
        public void RegistrarOrdenServicio(ETipoEquipo tipoEquipo)
        {
            #region Validaciones
            if (tipoEquipo == ETipoEquipo.Unidad)
            {
                if (this.vista.CombustibleEntrada == null || this.vista.CombustibleEntrada == 0)
                {
                    throw new Exception("Combustible Entrada es requerido");
                }
            }
            #endregion

            #region Otencion de Datos

            OrdenServicioBO ordenServicio = null;
            UsuarioBO usuario = new UsuarioBO { Id = this.vista.UC };
            MantenimientoUnidadBO mantenimientoUnidadBO=null;

            Object mantenimiento = null;

            if (tipoEquipo == ETipoEquipo.EquipoAliado)
            {
                Object mantenimientoUnidad = this.InterfazADatosMantenimientoUnidad(true);
                mantenimiento = this.InterfazADatosMantenimientoAliado();
                mantenimientoUnidadBO=(MantenimientoUnidadBO)mantenimientoUnidad;
                ordenServicio = this.IntefazADatosOrdenServicio(mantenimientoUnidadBO, (MantenimientoEquipoAliadoBO)mantenimiento);
            }
            else
            {
                mantenimiento = this.InterfazADatosMantenimientoUnidad(false);
                mantenimientoUnidadBO=(MantenimientoUnidadBO)mantenimiento;
                ordenServicio = this.IntefazADatosOrdenServicio(mantenimientoUnidadBO);
            }

            SeguridadBO seguridad = new SeguridadBO(Guid.NewGuid(), usuario, ordenServicio.Adscripcion);

            string descriptorTrabajo = this.ObtenerDescriptorTrabajo(mantenimiento);

            #endregion

            #region Conexion a BD
            
            this.dataContext.SetCurrentProvider("LIDER");

            Guid firma = Guid.NewGuid();
            try
            {
                this.dataContext.OpenConnection(firma);
                this.dataContext.BeginTransaction(firma);
            }
            catch (Exception)
            {
                if (this.dataContext.ConnectionState == ConnectionState.Open)
                    this.dataContext.CloseConnection(firma);
                throw new Exception("Se encontraron inconsistencias al acceso al origen de datos durante el Registrar Transacción.");
            }

            #endregion

            #region Lógica de Guardado
            
            try
            {
                ordenServicio = this.registrarOrdenServicioBR.RegistrarDocumento(this.dataContext, seguridad, ordenServicio, descriptorTrabajo);
                
                this.mantenimientoBR.ActualizarMantenimiento(dataContext, mantenimiento, ordenServicio, seguridad);
                
                dataContext.SetCurrentProvider("LIDER");
                dataContext.CommitTransaction(firma);

                #region Generar el correo para enviar a los autorizadores

                if (this.vista.UnidadOperativaId == (int)ETipoEmpresa.Generacion || this.vista.UnidadOperativaId == (int)ETipoEmpresa.Construccion || this.vista.UnidadOperativaId == (int)ETipoEmpresa.Equinova)
                {
                    int moduloID = (int)this.vista.ModuloID;
                    Enum TipoAutorizacion = this.vista.UnidadOperativaId == (int)ETipoEmpresa.Generacion ? (Enum)ETipoAutorizacionGeneracion.Orden_Servicio : 
                        this.vista.UnidadOperativaId == (int)ETipoEmpresa.Construccion ? (Enum)ETipoAutorizacionConstruccion.Orden_Servicio :
                        (Enum)ETipoAutorizacionEquinova.Orden_Servicio;

                    AutorizadorBR oAutorizador = new AutorizadorBR();
                    #region Crear el Diccionario para reemplazar los valores del formato con los de la entrega.
                    Dictionary<string, string> datosOrdenServicio = new Dictionary<string, string>();
                    #region Obtener información del Equipo
                    EquiposBO.UnidadBO unidadE = new EquiposBO.UnidadBO();
                    unidadE = unidadBR.ConsultarCompleto(dataContext, new EquiposBO.UnidadBO() { UnidadID = mantenimientoUnidadBO.IngresoUnidad.Unidad.UnidadID }, true).FirstOrDefault();
                    #endregion
                    #region Obtener Información de la sucursal
                    SucursalBO sucursalDestino = FacadeBR.ConsultarSucursal(dataContext, new SucursalBO() { Id = mantenimientoUnidadBO.Sucursal.Id }).FirstOrDefault();
                    #endregion
                    datosOrdenServicio["unidad"] = unidadE.TipoEquipo + " " + unidadE.Modelo.Nombre + " " + unidadE.NumeroSerie;
                    datosOrdenServicio["horometro"] = ordenServicio.KmHrsLlegar.ToString();
                    datosOrdenServicio["origenSucursal"] = unidadE.Sucursal.Nombre;
                    datosOrdenServicio["destinoSucursal"] = sucursalDestino.Nombre;
                    #region Tipo de Servicio
                    string tipoServicio = string.Empty;
                    switch (mantenimientoUnidadBO.TipoServicio.Id ?? 0)
                    {
                        case 1:
                            tipoServicio = "CORRECTIVO";
                            break;
                        case 2:
                            tipoServicio = "PREVENTIVO";
                            break;
                        case 3:
                            tipoServicio = "DAÑOS";
                            break;
                        default:
                            tipoServicio = string.Empty;
                            break;
                    }
                    #endregion
                    datosOrdenServicio["tipoServicio"] = tipoServicio;
                    datosOrdenServicio["descripcionServicio"] = mantenimientoUnidadBO.DescripcionFalla;
                    #endregion
                    try
                    {
                        oAutorizador.SolicitarAutorizacion(dataContext, TipoAutorizacion, datosOrdenServicio, moduloID, this.vista.UnidadOperativaId, mantenimientoUnidadBO.Sucursal.Id);
                    }
                    catch (Exception ex)
                    {
                        this.vista.MostrarMensaje("No se envió el correo configurado.", ETipoMensajeIU.ADVERTENCIA, ex.Message);
                    }
                }
                #endregion

                this.vista.MostrarMensaje("Registro de Orden de Servicio exitoso", ETipoMensajeIU.EXITO, null);
            }
            catch (Exception ex)
            {
                this.vista.MostrarMensaje(nombreClase + ".RegistrarOrdenServicio: Ocurrio un error al guardar la orden de servicio", ETipoMensajeIU.ERROR, ex.Message);
                this.dataContext.SetCurrentProvider("LIDER");
                this.dataContext.RollbackTransaction(firma);
            }
            finally
            {
                this.dataContext.SetCurrentProvider("LIDER");
                if (this.dataContext.ConnectionState == ConnectionState.Open)
                    this.dataContext.CloseConnection(firma);
            }
            #endregion
        }

        private string ObtenerDescriptorTrabajo(object mantenimiento)
        {
            string descriptorTrabajo = string.Empty;
            string configPTCorrectivo = ConfigurationManager.AppSettings["ConfigPTCorrectivo"];
            int tipoServicioCorrectivoId = Int32.Parse(ConfigurationManager.AppSettings["TipoServicioCorrectivo"]);
            if (mantenimiento is MantenimientoUnidadBO)
            {
                MantenimientoUnidadBO mantenimientoUnidad = ((MantenimientoUnidadBO)mantenimiento);
                if (mantenimientoUnidad.TipoServicio.Id == tipoServicioCorrectivoId) 
                {
                    descriptorTrabajo = configPTCorrectivo;
                }
                else
                {
                    descriptorTrabajo = mantenimientoUnidad.TipoMantenimiento.ToString();
                }
            }
            else
            {
                MantenimientoEquipoAliadoBO mantenimientoAliado = ((MantenimientoEquipoAliadoBO)mantenimiento);
                if (mantenimientoAliado.TipoServicio.Id == tipoServicioCorrectivoId)
                {
                    descriptorTrabajo = configPTCorrectivo;
                }
                else
                {
                    descriptorTrabajo = mantenimientoAliado.TipoMantenimiento.ToString();
                }
            }

            return descriptorTrabajo;
        }

        /// <summary>
        /// Establece los datos de mantenimiento segun el tipo, unidad o equipo aliado
        /// </summary>
        /// <param name="tipoEquipo"></param>
        /// <returns></returns>
        private Object InterfazADatosMantenimientoUnidad(bool isEquipoAliado)
        {
            GridViewRow row = this.vista.RowEnSesion;
            string guid = string.Empty;
            Object mantenimiento = null;
            if (isEquipoAliado)
            {
                guid = ((Label)row.FindControl("lblAliadoGuid")).Text;
                var mantenimientoUnidad = this.GetAliadoByGuid(guid).MantenimientoUnidad;
                this.EstableceValoresUnidad(mantenimientoUnidad);
                mantenimiento = mantenimientoUnidad;
            }
            else
            {
                guid = ((Label)row.FindControl("lblGuid")).Text;
                var mantenimientoUnidad = this.GetByGuid(guid).MantenimientoUnidad;
                this.EstableceValoresUnidad(mantenimientoUnidad);
                mantenimiento = mantenimientoUnidad;
            }
            
            return mantenimiento;
        }

        private Object InterfazADatosMantenimientoAliado()
        {
            GridViewRow row = this.vista.RowEnSesion;
            string guid = ((Label)row.FindControl("lblAliadoGuid")).Text;
            var mantenimientoAliado = this.GetAliadoByGuid(guid).MantenimientoAliado;
            this.EstableceValoresEquipoAliado(mantenimientoAliado);
            Object mantenimiento = mantenimientoAliado;
            return mantenimiento;
        }

        /// <summary>
        /// EStablece los valores de la unidad
        /// </summary>
        /// <param name="mantenimiento"></param>
        private void EstableceValoresUnidad(MantenimientoUnidadBO mantenimiento)
        {
            if (this.vista.DdTalleres.SelectedItem != null)
            {
                mantenimiento.CodigosFalla = this.vista.CodigosFalla;
                mantenimiento.DescripcionFalla = this.vista.DescripcionFalla;
                mantenimiento.CombustibleEntrada = this.vista.CombustibleEntrada;
                mantenimiento.CombustibleTotal = this.MarcaInternationalID == mantenimiento.IngresoUnidad.Unidad.Modelo.Id && this.vista.LeerInLine == true ? this.LeerArchivo(TipoLectura.COMBUSTIBLE, mantenimiento.IngresoUnidad.Vin) : this.vista.CombustibleTotal;
                mantenimiento.Inventario = this.vista.Inventario;
                mantenimiento.FechaArranque = DateTime.Now;
                mantenimiento.TipoServicio.Id = int.Parse(this.vista.DdTipoOrdenServicio.SelectedValue);
            }
        }

        /// <summary>
        /// Establece los valores del equipo aliado
        /// </summary>
        /// <param name="mantenimiento"></param>
        private void EstableceValoresEquipoAliado(MantenimientoEquipoAliadoBO mantenimiento)
        {
            if (this.vista.DdTalleres.SelectedItem != null)
            {
                mantenimiento.Taller.Id = int.Parse(this.vista.DdTalleres.SelectedValue);
                mantenimiento.CodigosFalla = this.vista.CodigosFalla;
                mantenimiento.DescripcionFalla = this.vista.DescripcionFalla;
                mantenimiento.Inventario = this.vista.Inventario;
                mantenimiento.FechaArranque = DateTime.Now;
                mantenimiento.TipoServicio.Id = int.Parse(this.vista.DdTipoOrdenServicio.SelectedValue);

                GridViewRow row = this.vista.RowEnSesion;

                TextBox txtEKmCalc = row.FindControl("txtEKmCalc") as TextBox;
                string km = this.ClearValue(txtEKmCalc.Text);
                if (km == string.Empty)
                    mantenimiento.KilometrajeEntrada = 0;
                else
                    mantenimiento.KilometrajeEntrada = int.Parse(km);

                TextBox txtEHorometroCalc = row.FindControl("txtEHorometroCalc") as TextBox;
                string hr = this.ClearValue(txtEHorometroCalc.Text);
                if (hr == string.Empty)
                    mantenimiento.HorasEntrada = 0;
                else
                    mantenimiento.HorasEntrada = int.Parse(hr);

            }
        }
        /// <summary>
        /// Genera el objeto OrdenServicioBO
        /// </summary>
        /// <param name="tipoEquipo">Si es una Unidad o un Equipo Aliado</param>
        /// <returns></returns>
        private OrdenServicioBO IntefazADatosOrdenServicio(MantenimientoUnidadBO mantenimiento, MantenimientoEquipoAliadoBO mantenimientoEquipoAliado = null)
        {
            int? horasEntrada = null;
            int? kilometrajeEntrada = null;
            int? mantenimientoId = null;
            String observacionesOperador = null;
            TipoServicioBO tipoServicio = null;
            TallerBO taller = null;
            AuditoriaBO auditoria = null;
            CatalogosBO.UnidadBO UnidadLider = null;
            ETipoMantenimiento? tipoMantto = null;
            DateTime? fechaIngreso = null;

            var ordenServicioCliente = new OrdenServicioClienteBO
            {
            };

            var modelosNoAuditables = FacadeBR.ConsultarModelosNoAuditables();

            if (mantenimiento is MantenimientoUnidadBO && mantenimientoEquipoAliado == null)
            {
                MantenimientoUnidadBO mantenimientoUnidad = (MantenimientoUnidadBO)mantenimiento;
                fechaIngreso = mantenimientoUnidad.IngresoUnidad.FechaIngreso;
                horasEntrada = mantenimientoUnidad.HorasEntrada;
                kilometrajeEntrada = mantenimientoUnidad.KilometrajeEntrada;
                mantenimientoId = mantenimientoUnidad.MantenimientoUnidadId;
                ordenServicioCliente.Operador = mantenimientoUnidad.IngresoUnidad.Operador;
                observacionesOperador = mantenimientoUnidad.IngresoUnidad.ObservacionesOperador;
                tipoServicio = new TipoServicioBO() { Id = mantenimientoUnidad.TipoServicio.Id };
                taller = new TallerBO() { Id = mantenimientoUnidad.Taller.Id };
                auditoria = mantenimientoUnidad.Auditoria;
                UnidadLider = new CatalogosBO.UnidadBO
                {
                    Id = mantenimientoUnidad.IngresoUnidad.Unidad.IDLider,
                    NumeroSerie = mantenimientoUnidad.IngresoUnidad.Unidad.NumeroSerie
                };

                ordenServicioCliente.AgrupadorCliente = new AgrupadorClienteBO()
                {
                    Cliente = mantenimientoUnidad.IngresoUnidad.Unidad.Cliente
                };

                mantenimiento.RequiereAuditoria = false;
                mantenimiento.ModeloAuditable = !modelosNoAuditables.Any(x => x.Id == mantenimientoUnidad.IngresoUnidad.Unidad.Modelo.Id);
                tipoMantto = mantenimiento.TipoMantenimiento;
            }
            else
            {
                MantenimientoUnidadBO mantenimientoUnidad = (MantenimientoUnidadBO)mantenimiento;
                MantenimientoEquipoAliadoBO mantenimientoAliado = (MantenimientoEquipoAliadoBO)mantenimientoEquipoAliado;

                fechaIngreso = mantenimientoEquipoAliado.IngresoEquipoAliado.FechaIngreso;
                horasEntrada = mantenimientoAliado.HorasEntrada;
                kilometrajeEntrada = mantenimientoAliado.KilometrajeEntrada;
                mantenimientoId = mantenimientoAliado.MantenimientoEquipoAliadoId;
                ordenServicioCliente.Operador = mantenimientoUnidad.IngresoUnidad.Operador;
                observacionesOperador = mantenimientoAliado.IngresoEquipoAliado.ObservacionesOperador;
                tipoServicio = new TipoServicioBO() { Id = mantenimientoEquipoAliado.TipoServicio.Id };
                taller = new TallerBO() { Id = mantenimientoUnidad.Taller.Id };
                auditoria = mantenimientoAliado.Auditoria;
                var AliadoLider = new CatalogosBO.UnidadBO
                {
                    Id = mantenimientoAliado.IngresoEquipoAliado.EquipoAliado.IDLider,
                    NumeroSerie = mantenimientoAliado.IngresoEquipoAliado.EquipoAliado.NumeroSerie
                };
                UnidadLider = AliadoLider;
                ordenServicioCliente.AgrupadorCliente = new AgrupadorClienteBO()
                {
                    Cliente = mantenimientoUnidad.IngresoUnidad.Unidad.Cliente
                };
                mantenimiento.RequiereAuditoria = false;
                mantenimiento.ModeloAuditable = !modelosNoAuditables.Any(x => x.Id == mantenimientoAliado.IngresoEquipoAliado.EquipoAliado.Modelo.Id);
                tipoMantto = mantenimientoAliado.TipoMantenimiento;
            }

            OrdenServicioBO ordenServicio = new OrdenServicioBO();

            AdscripcionBO adscripcion = new AdscripcionBO
            {
                UnidadOperativa = new UnidadOperativaBO
                {
                    Id = this.vista.UnidadOperativaId
                },
                //Sucursal = new SucursalBO() { Id = this.vista.SucursalID }
                Sucursal = new SucursalBO() { Id = mantenimiento.Sucursal.Id }
            };

            var adscripcionServicio = new CatalogosBO.AdscripcionServicioBO
            {
                UnidadOperativa = new UnidadOperativaBO(){ Id = this.vista.UnidadOperativaDestinoID },
                Sucursal = new SucursalBO() { Id = this.vista.SucursalDestinoID },
                Taller = new TallerBO() { Id = this.vista.TallerID }
            };

            int MedicionUnidad = 0;
            UnidadLider = FacadeBR.ConsultarUnidadDetalle(dataContext, UnidadLider).FirstOrDefault();
            if (UnidadLider == null)
                throw new Exception(this.nombreClase + ": No se encontró la Unidad en servicio");

            if (UnidadLider.KmHrs.HasValue)
            {
                if (UnidadLider.KmHrs.Value)
                {
                    MedicionUnidad = horasEntrada.Value;
                    UnidadLider.KmHrsProximoServicio = horasEntrada;
                }
                else
                {
                    if ((int)ETipoEmpresa.Idealease != this.vista.UnidadOperativaId)
                        throw new Exception(string.Format("No se permite utilizar la Unidad {0} en mantenimiento, ya que solo se pueden utilizar unidades con Horómetro.", UnidadLider.NumeroSerie));
                    MedicionUnidad = kilometrajeEntrada.Value;
                    UnidadLider.KmHrsProximoServicio = kilometrajeEntrada;
                }
            }
 
            UnidadLider.FechaProximoServicio = fechaIngreso;
            this.CalcularCantidadProximoMantenimientoLider(UnidadLider, true, UnidadLider.KmHrs == false, UnidadLider.KmHrs == true, tipoMantto);

            ordenServicio.Adscripcion = adscripcionServicio;
            ordenServicio.Auditoria = auditoria;
            ordenServicio.AdscripcionServicio = adscripcionServicio;
            ordenServicio.KmHrsLlegar = MedicionUnidad;
            ordenServicio.OrdenServicioCliente = ordenServicioCliente;
            ordenServicio.TipoServicio = new TipoServicioBO() { Id = tipoServicio.Id };
            ordenServicio.Unidad = UnidadLider;
            ordenServicio.Referencia = String.IsNullOrEmpty(UnidadLider.Clave) ? "N/A" : UnidadLider.Clave;
            ordenServicio.Observaciones = observacionesOperador;
            ordenServicio.Fallas = this.vista.CodigosFalla;
            ordenServicio.Inventario = this.vista.Inventario;
            return ordenServicio;
        }

        private void CalcularCantidadProximoMantenimientoLider(BPMO.Servicio.Catalogos.BO.UnidadBO unidadLider, bool dias, bool km, bool horas, ETipoMantenimiento? tipoMantenimiento)
        {
            var configuracionMantenimientoBR = new ConfiguracionMantenimientoBR();

            var configuracion = configuracionMantenimientoBR.Consultar(dataContext, new ConfiguracionMantenimientoBO() { Modelo = new ModeloBO() { Id = unidadLider.ConfiguracionModeloMotorizacion.Modelo.Id } });
            if (configuracion == null || !configuracion.Any())
            {
                unidadLider.FechaProximoServicio = this.TiempoMinimoProximoMantenimiento != null ? unidadLider.FechaProximoServicio.Value.AddDays(this.TiempoMinimoProximoMantenimiento.Value) : DateTime.Now.AddDays(1);
                unidadLider.KmHrsProximoServicio = unidadLider.KmHrsProximoServicio + 1;
            }
            else
            {
                if (dias)
                {
                    var configDias = configuracion.Where(x=>x.UnidadMedida == EUnidaMedida.Dias && x.TipoMantenimiento == tipoMantenimiento).ToList();
                    if (configDias.Any())
                    {
                        var config = configDias.Any(x => x.EnUso == true) ? configDias.FirstOrDefault(x => x.EnUso == true) : configDias.FirstOrDefault(x => x.EnUso == false);
                        if(config != null)
                            unidadLider.FechaProximoServicio = unidadLider.FechaProximoServicio.Value.AddDays(config.Intervalo.Value);
                        else
                            unidadLider.FechaProximoServicio = this.TiempoMinimoProximoMantenimiento != null ? unidadLider.FechaProximoServicio.Value.AddDays(this.TiempoMinimoProximoMantenimiento.Value) : DateTime.Now.AddDays(1);
                    }
                    else
                        unidadLider.FechaProximoServicio = this.TiempoMinimoProximoMantenimiento != null ? unidadLider.FechaProximoServicio.Value.AddDays(this.TiempoMinimoProximoMantenimiento.Value) : DateTime.Now.AddDays(1);
                }
                if (km)
                {
                    var configKm = configuracion.Where(x => x.UnidadMedida == EUnidaMedida.Kilometros && x.TipoMantenimiento == tipoMantenimiento).ToList();
                    if (configKm.Any())
                    {
                        var config = configKm.Any(x => x.EnUso == true) ? configKm.FirstOrDefault(x => x.EnUso == true) : configKm.FirstOrDefault(x => x.EnUso == false);
                        if (config != null)
                            unidadLider.KmHrsProximoServicio = config.Intervalo != null ? unidadLider.KmHrsProximoServicio + config.Intervalo : unidadLider.KmHrsProximoServicio + 1;
                        else
                            unidadLider.KmHrsProximoServicio = unidadLider.KmHrsProximoServicio + 1;
                    }
                    else
                        unidadLider.KmHrsProximoServicio = unidadLider.KmHrsProximoServicio + 1;
                }
                if (horas)
                {
                    var configHoras = configuracion.Where(x => x.UnidadMedida == EUnidaMedida.Horas && x.TipoMantenimiento == tipoMantenimiento).ToList();
                    if (configHoras.Any())
                    {
                        var config = configHoras.Any(x => x.EnUso == true) ? configHoras.FirstOrDefault(x => x.EnUso == true) : configHoras.FirstOrDefault(x => x.EnUso == false);
                        if (config != null)
                            unidadLider.KmHrsProximoServicio = config.Intervalo != null ? unidadLider.KmHrsProximoServicio + config.Intervalo : unidadLider.KmHrsProximoServicio + 1;
                        else
                            unidadLider.KmHrsProximoServicio = unidadLider.KmHrsProximoServicio + 1;
                    }
                    else
                        unidadLider.KmHrsProximoServicio = unidadLider.KmHrsProximoServicio + 1;
                }
            }
            return;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="guid"></param>
        public void VerOrdenServicio(string guid)
        {
            MantenimientoUnidadBO mantenimiento = this.GetByGuid(guid).MantenimientoUnidad;
            this.vista.PonerEnSesion("FolioOrdenServicio", mantenimiento.OrdenServicio.Id);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="guid"></param>
        public void VerOrdenServicioAliado(string guid)
        {
            MantenimientoEquipoAliadoBO mantenimiento = this.GetAliadoByGuid(guid).MantenimientoAliado;
            this.vista.PonerEnSesion("FolioOrdenServicio", mantenimiento.OrdenServicio.Id);
        }

        #region Imagen Combustible
        
        public void CargarImagenDefecto()
        {
            this.vista.ImgCombustible.ImageUrl = this.RutaImagenes + this.NombreImagenCombustibleDefecto;
        }

        public void CargarImagen()
        {
            try
            {
                var row = this.vista.RowEnSesion;
                string guid = string.Empty;
                EquiposBO.UnidadBO unidad = null;

                if (this.vista.IsUnidad())
                {
                    guid = ((Label)row.FindControl("lblGuid")).Text;
                    unidad = this.GetByGuid(guid).MantenimientoUnidad.IngresoUnidad.Unidad;
                }
                else
                {
                    guid = ((Label)row.FindControl("lblAliadoGuid")).Text;
                    unidad = this.GetAliadoByGuid(guid).MantenimientoUnidad.IngresoUnidad.Unidad;
                }


                var capacidad = unidad.CaracteristicasUnidad.CapacidadTanque != null ? unidad.CaracteristicasUnidad.CapacidadTanque.Value : 0;

                string ruta = this.ParsePath(this.vista.RootPath) + this.ParsePath(this.RutaPlantillaXML);
                string plantillaXml = this.PlantillaXML;

                var xmlDocument = this.ObtenerXMLContenido(ruta + plantillaXml);

                var imagenes = this.CargarImagenes(ruta + plantillaXml);
                XmlNodeList txtFracciones = xmlDocument.GetElementsByTagName("fracciones");
                if (txtFracciones.Count < 1) throw new Exception("el formato del archivo XML es incorrecto");
                var fraccion = (int)Convert.ChangeType(txtFracciones[0].InnerText, typeof(int));
                int? indexPic = -1;
                if (unidad != null)
                    if (unidad.CaracteristicasUnidad != null)
                        if (unidad.CaracteristicasUnidad.CapacidadTanque.HasValue)
                            indexPic = this.ObtenerFraccionMedidorCombustible(capacidad, fraccion);

                var urlFraccion = imagenes.FirstOrDefault(x => string.Compare(x.Key, indexPic.ToString(), System.StringComparison.Ordinal) == 0).Value;
                this.vista.ImgCombustible.ImageUrl = urlFraccion;
            }
            catch (Exception ex)
            {
                this.vista.MostrarMensaje("Ocurrió un error al procesar la solicitud.", ETipoMensajeIU.ERROR, ex.Message);
            }
        }

        private string ParsePath(String urlXML)
        {
            String separadorDirectorio = Path.DirectorySeparatorChar.ToString();
            string path = string.Empty;
            if (urlXML.StartsWith("~"))
            {
                path = urlXML.Replace("~", "");
                path = path.Replace("/", separadorDirectorio);
            }
            else
            {
                path = urlXML.Replace("'\'", separadorDirectorio);
            }
            return path;
        }

        public string ClearValue(string text)
        {
            var val = "";
            if (text.Length > 0)
            {
                string[] arrayText = text.Split(',');
                val = arrayText[arrayText.Length - 1];
            }
            return val;
        }

        /// <summary>
        /// Obtiene la plantilla XML para el reporte
        /// </summary>
        /// <param name="urlXML">url del reporte</param>
        /// <returns>plantilla del XML</returns>
        private XmlDocument ObtenerXMLContenido(string urlXML)
        {
            try
            {
                XmlDocument xDoc = new XmlDocument();
                xDoc.Load(urlXML);
                return xDoc;
            }
            catch (Exception ex)
            {
                throw new Exception(nombreClase + ".ObtenerXMLContenido: Error al cargar el archivo XML." + ex.Message);
            }
        }

        /// <summary>
        /// Obtiene las imagenes para la valvula de gasolina
        /// </summary>
        /// <param name="path">url del xml</param>
        /// <returns>Lista con las direcciones de las imagenes</returns>
        private Dictionary<string, string> CargarImagenes(string path)
        {
            if (string.IsNullOrEmpty(path) || string.IsNullOrWhiteSpace(path))
                throw new Exception("El archivo no existe en la ubicación proorcionada.");

            Dictionary<string, string> datos = new Dictionary<string, string>();

            if (File.Exists(path))
            {
                XDocument xmlDoc = XDocument.Load(path);

                var imagenes = xmlDoc.Descendants("imagenes").Descendants("imagen").Select(reps => new
                {
                    id = reps.Element("id").Value,
                    url = reps.Element("url").Value
                });

                foreach (var imagen in imagenes)
                {
                    datos.Add(imagen.id, imagen.url);
                }
            }

            return datos;
        }

        public int? ObtenerFraccionMedidorCombustible(decimal capacidad, int fracciones)
        {
            if (capacidad <= new decimal(0))
            {
                return null;
            }
            if (fracciones <= 0)
            {
                return null;
            }
            int? nullable = this.vista.CombustibleEntrada;
            if ((nullable.GetValueOrDefault() > 0 ? false : nullable.HasValue))
            {
                return new int?(0);
            }
            int? nullable1 = this.vista.CombustibleEntrada;
            if ((nullable1.GetValueOrDefault() < capacidad ? false : nullable1.HasValue))
            {
                return new int?(fracciones);
            }
            return new int?((int)Math.Truncate(this.vista.CombustibleEntrada * fracciones / capacidad));
        }

        public decimal? CalcularPorcentajeCombustible(decimal capacidadTanque)
        {

            int? nullable;
            if (capacidadTanque <= new decimal(0))
            {
                return null;
            }
            int? combustible = this.vista.CombustibleEntrada;
            if (combustible.HasValue)
            {
                nullable = new int?(combustible.GetValueOrDefault() * 100);
            }
            else
            {
                nullable = null;
            }
            int? nullable1 = nullable;
            decimal num = capacidadTanque;
            if (!nullable1.HasValue)
            {
                return null;
            }
            return new decimal?(nullable1.GetValueOrDefault() / num);
        }

        
        #endregion

        #region Leer Inline
        #region Posible No Usado
        /// <summary>
        /// Valida si realiza la lectura inline
        /// </summary>
        /// <returns></returns>
        private bool LeerInline()
        {
            ConfiguracionUnidadOperativaBO configuracionUnidadOperativa = new ConfiguracionUnidadOperativaBO();
            configuracionUnidadOperativa.UnidadOperativa = new UnidadOperativaBO() { Id = this.vista.UnidadOperativaId };
            ConfiguracionUnidadOperativaBO result = this.moduloBR.ConsultarConfiguracionUnidadOperativa(dataContext, configuracionUnidadOperativa, this.vista.ModuloID).FirstOrDefault();

            if (result != null)
            {
                return result.KmHrsInternational != null ? result.KmHrsInternational.Value : false;
            }
            else
                return false;
        } 
        #endregion

        public int LeerArchivo(TipoLectura tipo, string vin)
        {
            string[] filePaths = Directory.GetFiles(this.RutaLecturaArchivosInline, vin + ".*");

            if (filePaths == null || filePaths.Length == 0)
                throw new Exception("El archivo que intenta buscar no existe");

            string file = filePaths[0];

            if (file.EndsWith(".txt") || file.EndsWith(".csv"))
            {
                return this.LeerArchivoIsite(tipo, file);
            }
            else
            {
                return this.LeerArchivoInline(tipo, file);
            }
        }

        private int LeerArchivoInline(TipoLectura tipo, string file)
        {
            int valor = 0;
            string medida = "";
            char[] delimiterChars = { '\n' };

            TextReader tr = new StreamReader(file);
            string fileContents = tr.ReadToEnd();
            string[] fileContentArray = fileContents.Split(delimiterChars);

            if (fileContentArray == null || fileContentArray.Length == 0)
                throw new Exception("El archivo no contiene datos");

            string etiqueta = "";
            switch (tipo)
            {
                case TipoLectura.COMBUSTIBLE:
                    etiqueta = "Total Fuel Used";
                    break;
                case TipoLectura.KM:
                    etiqueta = "Odometer";
                    break;
                case TipoLectura.HORAS:
                    etiqueta = "Engine Hours";
                    break;
            }

            foreach (var line in fileContentArray)
            {
                if (line.StartsWith(etiqueta))
                {
                    string value = line.Split(',')[1].Trim();
                    valor = Convert.ToInt32(double.Parse(value));
                    medida = line.Split(',')[2].Trim().ToUpper();
                    break;
                }
            }
            switch (tipo)
            {
                case TipoLectura.COMBUSTIBLE:
                    valor = medida != "LT" ? Convert.ToInt32(valor * 3.785) : Convert.ToInt32(valor);
                    break;
                case TipoLectura.KM:
                    valor = medida != "KM" ? Convert.ToInt32(valor * 1.609) : Convert.ToInt32(valor);
                    break;
            }
            return valor;
        }

        private int LeerArchivoIsite(TipoLectura tipo, string file)
        {
            Dictionary<string, int> indices = new Dictionary<string, int>();
            int valorLeido = 0;
            string medida = "";

            char[] delimiterChars = { '\n' };

            TextReader tr = new StreamReader(file, Encoding.GetEncoding(1252));
            string fileContents = tr.ReadToEnd();
            string[] fileContentArray = fileContents.Split(delimiterChars);

            for (int i = 0; i < fileContentArray.Length; i++)
            {
                var line = fileContentArray[i];
                if (line.StartsWith("Todos los Viajes (Acumulativo) - Combustible Utilizado"))
                {
                    indices.Add("combustible", i);
                }

                if (line.StartsWith("Todos los Viajes (Acumulativo) - Distancia"))
                {
                    indices.Add("km", i);
                }

                if (line.StartsWith("Todos los Viajes (Acumulativo) - Tiempo"))
                {
                    indices.Add("hrs", i);
                }
            }

            switch (tipo)
            {
                case TipoLectura.COMBUSTIBLE:
                    int iCombustible = indices["combustible"];
                    for (int i = iCombustible; i < fileContentArray.Length; i++)
                    {
                        var line = fileContentArray[i].Trim();

                        if (line.StartsWith("Combustible Utilizado,"))
                        {
                            var value = line.Split(',')[1].Trim();
                            valorLeido = Convert.ToInt32(double.Parse(value));
                            medida = line.Split(',')[2].Trim().ToUpper();
                            break;
                        }
                    }
                    break;
                case TipoLectura.KM:
                    int iKm = indices["km"];
                    for (int i = iKm; i < fileContentArray.Length; i++)
                    {
                        var line = fileContentArray[i].Trim();

                        if (line.StartsWith("Distancia del ECM"))
                        {
                            var value = line.Split(',')[1].Trim();
                            valorLeido = Convert.ToInt32(double.Parse(value));
                            medida = line.Split(',')[2].Trim().ToUpper();
                            break;
                        }
                    }
                    break;
                case TipoLectura.HORAS:
                    int iHrs = indices["hrs"];
                    for (int i = iHrs; i < fileContentArray.Length; i++)
                    {
                        var line = fileContentArray[i].Trim();

                        if (line.StartsWith("Tiempo de Operación del Motor"))
                        {
                            var value = line.Split(',')[1].Trim().Split(':')[0];
                            valorLeido = Convert.ToInt32(double.Parse(value));
                            break;
                        }
                    }
                    break;
            }

            switch (tipo)
            {
                case TipoLectura.COMBUSTIBLE:
                    valorLeido = medida != "LT" ? Convert.ToInt32(valorLeido * 3.785) : Convert.ToInt32(valorLeido);
                    break;
                case TipoLectura.KM:
                    valorLeido = medida != "KM" ? Convert.ToInt32(valorLeido * 1.609) : Convert.ToInt32(valorLeido);
                    break;
            }

            return valorLeido;
        }
        #endregion
        
        #endregion

        #region Auditorias
        
        /// <summary>
        /// Verifica si los tecnicos han sido asignados a la orden de servicio
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        public bool ValidarAuditoria(string guid)
        {
            MantenimientoUnidadBO mantenimiento = this.GetByGuid(guid).MantenimientoUnidad;
            if (mantenimiento.OrdenServicio.Id == null)
            {
                this.vista.MostrarMensaje("El mantenimiento no cuenta con Orden de Servicio", ETipoMensajeIU.ADVERTENCIA, null);
                return false;
            }

            var auditoria = auditoriaMantenimimentoBR.Consultar(this.dataContext, new AuditoriaMantenimientoBO() { OrdenServicio = new OrdenServicioBO() { Id = mantenimiento.OrdenServicio.Id } }).FirstOrDefault();
            if (auditoria != null)
            {
                this.vista.MostrarMensaje("Ya se ha realizado la auditoria del mantenimiento", ETipoMensajeIU.ADVERTENCIA, null);
                return false;
            }

            AsignacionPosicionTrabajoBO asignacion = new AsignacionPosicionTrabajoBO { OrdenId = mantenimiento.OrdenServicio.Id };
            List<AsignacionPosicionTrabajoBO> asignaciones = FacadeBR.ConsultarAsignacionPosicionTrabajo(dataContext, asignacion);
            if (!asignaciones.Any())
            {
                this.vista.MostrarMensaje("No se encontraron técnicos asignados. Favor de verificar", ETipoMensajeIU.ADVERTENCIA, null);
                return false;
            }

            return true;
        }
        /// <summary>
        /// Verifica si el Mantenimiento ya cuenta con auditoria
        /// </summary>
        /// <param name="mantenimiento">Mantenimiento Unidad a validar</param>
        /// <returns>Indica si el mantenimiento ya se le realizó la Auditoria</returns>
        public bool CuentaConAuditoria(MantenimientoUnidadBO mantenimiento) {
            if (mantenimiento.OrdenServicio.Id == null) {
                this.vista.MostrarMensaje("El mantenimiento no cuenta con Orden de Servicio", ETipoMensajeIU.ADVERTENCIA, null);
                return false;
            }
            
            AuditoriaMantenimientoBO auditoria = auditoriaMantenimimentoBR.Consultar(this.dataContext, new AuditoriaMantenimientoBO() { OrdenServicio = new OrdenServicioBO() { Id = mantenimiento.OrdenServicio.Id } }).FirstOrDefault();
            if (auditoria != null && auditoria.EvidenciaMantenimiento != null && auditoria.EvidenciaMantenimiento.EvidenciaMantenimientoId != null)
                return true;

            return false;
        }
        
        /// <summary>
        /// Metodo para realizar la auditoria de una orden de servicio
        /// </summary>
        /// <param name="guid"></param>
        public void RealizarAuditoria(string guid)
        {
            MantenimientoUnidadBO mantenimiento = this.GetByGuid(guid).MantenimientoUnidad;
            this.vista.PonerEnSesion("MantenimientoAuditoria", mantenimiento.OrdenServicio);
        }

        #endregion

        #region Privados
        
        private void EstablecerInformacionInicial()
        {
            try
            {
                #region Configuraciones de Unidad Operativa
                //Obtener las configuraciones de la unidad operativa
                ConfiguracionUnidadOperativaBO config = new ModuloBR().ConsultarConfiguracionUnidadOperativa(this.dataContext,
                    new ConfiguracionUnidadOperativaBO() { UnidadOperativa = new UnidadOperativaBO() { Id = this.vista.UnidadOperativaId } },
                    this.vista.ModuloID).FirstOrDefault();
                if (config == null)
                    throw new Exception("No se encontraron las configuraciones del sistema para la unidad operativa en la que trabaja.");

                //Establecer las configuraciones de la unidad operativa
                this.vista.LibroActivos = config.Libro;
                this.vista.LeerInLine = config.KmHrsInternational;

                this.vista.CargarEstatus();

                #endregion
            }
            catch (Exception ex)
            {
                throw new Exception(this.nombreClase + ".EstablecerInformacionInicial: " + ex.Message);
            }
        }
        private bool validarHorasKmEntrada(GridViewRow row, string txtKm, string txtHrs)
        {
            bool result = false;

            TextBox txtKms = row.FindControl(txtKm) as TextBox;
            TextBox txtHoras = row.FindControl(txtHrs) as TextBox;

            if (string.IsNullOrEmpty(this.ClearValue(txtKms.Text)) && string.IsNullOrEmpty(this.ClearValue(txtHoras.Text)))
            {
                this.vista.MostrarMensaje("Es necesario establecer KMs y HRS de entrada", ETipoMensajeIU.ADVERTENCIA, null);
            }
            else
            {
                result = true;
            }

            return result;
        }
        /// <summary>
        /// Consulta los datos complemento de una unidad
        /// </summary>
        /// <param name="unidadBO">Unidad consultada</param>
        /// <param name="sucursales">Sucursales consultadas</param>
        /// <param name="clientes">Clientes consultados</param>
        private void ComplementarDatosUnidad(Equipos.BO.UnidadBO unidadBO, List<SucursalBO> sucursales, List<ClienteBO> clientes)
        {

            unidadBO.EquiposAliados = unidadBR.ConsultarEquipoAliado(dataContext, unidadBO);

            //Se hace la consulta una vez de TODAS las sucursales para optimizar el tiempo
            if (!sucursales.Any(x => x.Id == unidadBO.Sucursal.Id))
            {
                sucursales.Add(FacadeBR.ConsultarSucursal(dataContext, new SucursalBO() { Id = unidadBO.Sucursal.Id }).FirstOrDefault());
            }
            unidadBO.Sucursal.Nombre = sucursales.FirstOrDefault(x => x.Id == unidadBO.Sucursal.Id).Nombre;

            //Obtener los clientes
            
            if (unidadBO.Cliente.Id != null)
            {
                if (!clientes.Any(x => x.Id == unidadBO.Cliente.Id))
                {
                    ClienteBO clienteTemp = new ClienteBO() { Id = unidadBO.Cliente.Id };
                    clientes.Add(FacadeBR.ConsultarCliente(dataContext, clienteTemp).FirstOrDefault());
                }
                unidadBO.Cliente = clientes.FirstOrDefault(x => x.Id == unidadBO.Cliente.Id);
            }
        }
        private void ComplementarDatosUnidades(List<Equipos.BO.UnidadBO> result)
        {
            List<SucursalBO> sucursales = new List<SucursalBO>();
            List<ClienteBO> clientes = new List<ClienteBO>();
            foreach (var unidad in result)
            {
                if (!sucursales.Any(x => x.Id == unidad.Sucursal.Id))
                {
                    sucursales.Add(FacadeBR.ConsultarSucursal(dataContext, new SucursalBO() { Id = unidad.Sucursal.Id }).FirstOrDefault());
                }
                unidad.Sucursal = sucursales.FirstOrDefault(s => s.Id == unidad.Sucursal.Id);

                if (unidad.Cliente.Id != null)
                {
                    if (!clientes.Any(x => x.Id == unidad.Cliente.Id))
                    {
                        ClienteBO clienteTemp = new ClienteBO() { Id = unidad.Cliente.Id };
                        clientes.Add(FacadeBR.ConsultarCliente(dataContext, clienteTemp).FirstOrDefault());
                    }
                    unidad.Cliente = clientes.FirstOrDefault(x => x.Id == unidad.Cliente.Id);
                }
            }
        }
        
        #endregion

        #region CU064
        public void EnviarInformacionCliente()
        {
            var row = this.vista.RowEnSesion;
            string guid = ((Label)row.FindControl("lblGuid")).Text;
            CheckBox chkAgregarTareas = row.FindControl("chkAgregarTareas") as CheckBox;
            bool bEnviar = this.vista.ListaTareasPendientes != null && chkAgregarTareas.Checked;

            MantenimientoBOF mantenimiento = this.GetByGuid(guid);
            this.vista.PonerEnSesion("mantenimientoBOF", mantenimiento);
            this.vista.redirigeCU064(bEnviar);
        }
        #endregion

        #region CU051
        /// <summary>
        /// Genera el pase de salida
        /// </summary>
        public void GenerarPaseSalida()
        {
            Hashtable parameters = new Hashtable();
            if (this.vista.ModuloID != null)
                parameters["ModuloID"] = this.vista.ModuloID;
            if (this.vista.UnidadOperativaId != null)
                parameters["UnidadOperativaID"] = this.vista.UnidadOperativaId;

            var row = this.vista.RowEnSesion;
            string guid = ((Label)row.FindControl("lblGuid")).Text;
            MantenimientoUnidadBO mantenimiento = this.GetByGuid(guid).MantenimientoUnidad;
            parameters["FolioServicio"] = mantenimiento.MantenimientoUnidadId;
            parameters["NumeroSerie"] = mantenimiento.IngresoUnidad.Unidad.NumeroSerie;
          

            if (mantenimiento.OrdenServicio.Id != null)
            {
                if (mantenimiento.OrdenServicio != null && (mantenimiento.OrdenServicio.Estatus.Id == 2 || mantenimiento.OrdenServicio.Estatus.Id == 6))
                {
                    var manttoConCombustible = this.mantenimientoUnidadBR.Consultar(this.dataContext, new MantenimientoUnidadBO() { MantenimientoUnidadId = mantenimiento.MantenimientoUnidadId }).FirstOrDefault();
                    if (manttoConCombustible == null)
                    {
                        this.vista.MostrarMensaje("No se pudo consultar el mantenimiento para verificar el combustible de salida", ETipoMensajeIU.ADVERTENCIA);
                        return;
                    }
                  
                    if (manttoConCombustible.CombustibleSalida == null)
                    {
                        this.vista.MostrarMensaje("Se debe imprimir la calcomania antes de generar el pase de salida", ETipoMensajeIU.ADVERTENCIA);
                        return;
                    }

                    if (manttoConCombustible.FechaSalida == null) {
                        //Se crea el objeto de seguridad
                        UsuarioBO usuario = new UsuarioBO() { Id = vista.UsuarioAutenticado };
                        AdscripcionBO adscripcion = new AdscripcionBO() { UnidadOperativa = new UnidadOperativaBO() { Id = this.vista.UnidadOperativaId } };
                        SeguridadBO seguridadBO = new SeguridadBO(Guid.NewGuid(), usuario, adscripcion);
                        Dictionary<String, Object> reportParameters = new ObtenerOrdenSalidaBR().ConsultarOrdenSalida(this.dataContext, parameters, seguridadBO);
                        if (reportParameters["DataSource"] != null) {
                            this.vista.EstablecerPaqueteNavegacionImprimir(this.vista.IdentificadorReporteCU051, reportParameters, true);
                            this.vista.redirigePaseSalida();

                            this.EnviarInformacionCliente();
                        } else {
                            this.vista.MostrarMensaje("Sin resultados para mostrar", ETipoMensajeIU.ADVERTENCIA);
                        }
                    } else { 
                        // Ya se imprimió Pase Salida. Enviar información a Cliente
                        this.EnviarInformacionCliente();
                    }                    
                }
                else
                {
                    this.vista.MostrarMensaje("Solo se pueden generar el pase de salida para mantenimientos asignados o facturados", ETipoMensajeIU.ADVERTENCIA);
                }
              
            }
            else
            {
                this.vista.MostrarMensaje("no se puede generar el pase de salida para una unidad sin mantenimiento", ETipoMensajeIU.ADVERTENCIA);
            }            
        }
        #endregion

        #region Obtener Unidades Pendientes Por Entrar

        public void ObtenerUnidadesPendientesPorEntrar() {
            DataTable table = new DataTable();
            table.Columns.Add("NumeroSerie");
            table.Columns.Add("NumeroEconomico");
            table.Columns.Add("Modelo");
            table.Columns.Add("Cliente");
            table.Columns.Add("Fecha");
            DateTime fechaActual = DateTime.Now;
            DateTime fechaInicio = new DateTime(fechaActual.Year, fechaActual.Month, fechaActual.Day, 0, 0, 0);
            DateTime fechaFin = new DateTime(fechaActual.Year, fechaActual.Month, fechaActual.Day, 23, 59, 59);
            List<MantenimientoProgramadoBO> mantenimientosProgramados = consultarMantenimientoProgramadoBR.ConsultarRangoFechas(dataContext, fechaInicio, fechaFin);
            List<CitaMantenimientoBO> citasActuales = getListaCitasActuales(fechaInicio, fechaFin);
            if(mantenimientosProgramados != null && mantenimientosProgramados.Count > 0) {
                foreach (MantenimientoProgramadoBO mantenimientoProgramado in mantenimientosProgramados) {
                    if(mantenimientoProgramado.EsUnidad.Value) {
                        BPMO.SDNI.Equipos.BR.UnidadBR ctrlUnidad = new BPMO.SDNI.Equipos.BR.UnidadBR();
                        BPMO.SDNI.Equipos.BO.UnidadBO unidad = ctrlUnidad.Consultar(dataContext, getFiltroUnidad(mantenimientoProgramado.EquipoID.Value)).FirstOrDefault();
                        if(unidad != null && unidad.UnidadID != null) {
                            unidad.Cliente = getClienteCompleto(unidad.Cliente);
                            CitaMantenimientoBR ctrlCitaMantenimiento = new CitaMantenimientoBR();
                            CitaMantenimientoBO cita = ctrlCitaMantenimiento.Consultar(dataContext, getFiltroCita(mantenimientoProgramado)).FirstOrDefault();
                            if (cita != null && cita.CitaMantenimientoID != null) {
                                if(cita.FechaHoraCita.Value.Date.Equals(fechaActual.Date) && ((int)cita.EstatusCita.Value == (int)EEstatusCita.CALENDARIZADA || (int)cita.EstatusCita.Value == (int)EEstatusCita.RECALENDARIZADA)) {
                                    MantenimientoUnidadBO mantenimiento = mantenimientoUnidadBR.Consultar(dataContext, getFiltroMantenimiento(unidad, fechaActual)).FirstOrDefault();
                                    if (mantenimiento == null || mantenimiento.MantenimientoUnidadId == null) {
                                        agregarNuevaFila(table, unidad, mantenimientoProgramado.Fecha.Value);
                                    }
                                    if(citasActuales != null && citasActuales.Count > 0){
                                        CitaMantenimientoBO citaEncontrada = citasActuales.Find(x => x.CitaMantenimientoID == cita.CitaMantenimientoID);
                                        if (citaEncontrada != null) {
                                            citasActuales.Remove(citaEncontrada);
                                        }
                                    }
                                }
                            } else {
                                agregarNuevaFila(table, unidad, mantenimientoProgramado.Fecha.Value);
                            }
                        }
                    }
                }
            }
            if (citasActuales != null && citasActuales.Count > 0) {
                foreach (CitaMantenimientoBO cita in citasActuales) {
                    MantenimientoProgramadoBO mantenimientoProgramado = consultarMantenimientoProgramadoBR.Consultar(dataContext, cita.MantenimientoProgramado).FirstOrDefault();
                    if (mantenimientoProgramado != null && mantenimientoProgramado.MantenimientoProgramadoID != null) {
                        if (mantenimientoProgramado.EsUnidad.Value) { 
                            BPMO.SDNI.Equipos.BR.UnidadBR ctrlUnidad = new BPMO.SDNI.Equipos.BR.UnidadBR();
                            BPMO.SDNI.Equipos.BO.UnidadBO unidad = ctrlUnidad.Consultar(dataContext, getFiltroUnidad(mantenimientoProgramado.EquipoID.Value)).FirstOrDefault();
                            if(unidad != null && unidad.UnidadID != null) {
                                unidad.Cliente = getClienteCompleto(unidad.Cliente);
                                MantenimientoUnidadBO mantenimiento = mantenimientoUnidadBR.Consultar(dataContext, getFiltroMantenimiento(unidad, fechaActual)).FirstOrDefault();
                                if (mantenimiento == null || mantenimiento.MantenimientoUnidadId == null) {
                                    agregarNuevaFila(table, unidad, mantenimientoProgramado.Fecha.Value);
                                }
                            }
                        }
                    }
                }
            }
            DataSet data = new DataSet();
            data.Tables.Add(table);
            vista.MantenimientosProgramadosPendientes = data;
            if (table.Rows.Count > 0) {
                vista.DesplegarListaMantenimientosProgramadosPendientes();
            } else {
                vista.MostrarMensaje("No se encontraron Equipos Programados Pendientes por Ingresar.", ETipoMensajeIU.ADVERTENCIA);
            }
        }

        private List<CitaMantenimientoBO> getListaCitasActuales(DateTime fechaInicio, DateTime fechaFin) {
            CitaMantenimientoBOF bof = new CitaMantenimientoBOF(){
                FechaInicio = fechaInicio,
                FechaFin = fechaFin,
                Estatus = EEstatusCita.CALENDARIZADA
            };
            
            List<CitaMantenimientoBO> citasCalendarizadas = new CitaMantenimientoBR().ConsultarPorRangoFechas(dataContext, bof);
            bof.Estatus = EEstatusCita.RECALENDARIZADA;
            List<CitaMantenimientoBO> citasRecalendarizadas = new CitaMantenimientoBR().ConsultarPorRangoFechas(dataContext, bof);
            List<CitaMantenimientoBO> citas = new List<CitaMantenimientoBO>();
            if (citasCalendarizadas != null && citasCalendarizadas.Count > 0) {
                citas.AddRange(citasCalendarizadas);
            }
            if (citasRecalendarizadas != null && citasRecalendarizadas.Count > 0) {
                citas.AddRange(citasRecalendarizadas);
            }
            return citas;
        }

        private BPMO.SDNI.Equipos.BO.UnidadBO getFiltroUnidad(int EquipoID) { 
             BPMO.SDNI.Equipos.BO.UnidadBO unidad = new BPMO.SDNI.Equipos.BO.UnidadBO() {
                EquipoID = EquipoID
            };
             return unidad;
        }

        private MantenimientoUnidadBO getFiltroMantenimiento(BPMO.SDNI.Equipos.BO.UnidadBO unidad, DateTime fechaActual) { 
            MantenimientoUnidadBO filtroMantenimiento = new MantenimientoUnidadBO() {
                IngresoUnidad = new IngresoUnidadBO() {
                    Unidad = unidad,
                    FechaIngreso = new DateTime(fechaActual.Year, fechaActual.Month, fechaActual.Day, 1, 0, 0)
                },
                FechaSalida = new DateTime(fechaActual.Year, fechaActual.Month, fechaActual.Day, 23, 59, 59)
            };
            return filtroMantenimiento;
        }

        private CitaMantenimientoBO getFiltroCita(MantenimientoProgramadoBO mantenimientoProgramado) { 
            CitaMantenimientoBO filtroCita = new CitaMantenimientoBO() {
                MantenimientoProgramado = mantenimientoProgramado
            };
            return filtroCita;
        }

        private void agregarNuevaFila(DataTable table, BPMO.SDNI.Equipos.BO.UnidadBO unidad, DateTime fecha) {
            DataRow row = table.NewRow();
            row.ItemArray = new object[] { unidad.NumeroSerie, unidad.NumeroEconomico, unidad.Modelo.Nombre, unidad.Cliente.NombreCompleto, fecha };
            table.Rows.Add(row);
        }

        private ClienteBO getClienteCompleto(ClienteBO cliente) {
            ClienteBO clienteCompleto = new ClienteBO();
            if(cliente.Id != null) {
                ClienteBO filtroCliente = new ClienteBO() {
                    Id = cliente.Id
                };
                clienteCompleto = FacadeBR.ConsultarCliente(dataContext, filtroCliente).FirstOrDefault();
            }
            return clienteCompleto;
        }

        #endregion

        #region CU017
        //Redirige al CU017
        public void ImprimirCalcomania()
        {
            bool seImprimioCalca = false;
            Hashtable parameters = new Hashtable();
            if (this.vista.ModuloID != null)
                parameters["ModuloID"] = this.vista.ModuloID;
            if (this.vista.UnidadOperativaId != null)
                parameters["UnidadOperativaID"] = this.vista.UnidadOperativaId;

            var row = this.vista.RowEnSesion;
            string guid = ((Label)row.FindControl("lblGuid")).Text;
            MantenimientoUnidadBO mantenimiento = this.GetByGuid(guid).MantenimientoUnidad;
            mantenimiento.CombustibleSalida = this.vista.CombustibleSalida;
            string serie = mantenimiento.IngresoUnidad.Unidad.NumeroSerie;
            parameters["MantenimientoUnidad"] = mantenimiento;

            //Se crea el objeto de seguridad
            UsuarioBO usuario = new UsuarioBO() { Id = vista.UsuarioAutenticado };
            AdscripcionBO adscripcion = new AdscripcionBO() { UnidadOperativa = new UnidadOperativaBO() { Id = this.vista.UnidadOperativaId } };
            SeguridadBO seguridadBO = new SeguridadBO(Guid.NewGuid(), usuario, adscripcion);
           
            Dictionary<String, Object> reportParameters = new ReporteCalcomaniaBR().GenerarReporte(this.dataContext, serie, parameters, seguridadBO, out seImprimioCalca);

            if (((CalcomaniaMantenimientoDS)reportParameters["DataSource"]).Mantenimientos.Rows.Count > 0)
            {
                    foreach (DataRow r in ((DataSet)reportParameters["DataSource"]).Tables["Mantenimientos"].Rows)
                    {
                        if (r["TipoEquipo"].ToString().Equals(Convert.ToString((int)ETipoEquipo.Unidad)))
                        {
                            if ((r["StatusID"].ToString() == "2" || r["StatusID"].ToString() == "6"))
                            {
                                this.vista.EstablecerPaqueteNavegacionImprimir("PLEN.BEP.15.MODMTTO.CU017", reportParameters);
                                this.vista.redirigeCU017();

                                this.GenerarPaseSalida();
                            }
                            else
                            {
                                this.vista.MostrarMensaje("La orden de orden de servicio no está asignada o facturada", ETipoMensajeIU.ADVERTENCIA);
                            }
                        }
                    }
            }
            else
            {
                this.vista.MostrarMensaje("Ya se imprimió el pase de salida de la unidad", ETipoMensajeIU.ADVERTENCIA);
                return;
            }
        }
        #endregion

        #region CU063

        /// <summary>
        /// Verifica si una unidad en mantenimiento o modelo tiene tareas pendientes
        /// </summary>
        public void VerificarTareasPendientes()
        {
            var row = this.vista.RowEnSesion;
            string guid = ((Label)row.FindControl("lblGuid")).Text;
            MantenimientoUnidadBO mantenimiento = this.GetByGuid(guid).MantenimientoUnidad;
            if (String.IsNullOrEmpty(mantenimiento.IngresoUnidad.Unidad.NumeroSerie) && mantenimiento.IngresoUnidad.Unidad.Modelo == null)
                return;
            if (String.IsNullOrEmpty(mantenimiento.IngresoUnidad.Unidad.NumeroSerie) && mantenimiento.IngresoUnidad.Unidad.Modelo != null && mantenimiento.IngresoUnidad.Unidad.Modelo.Id != null)
                return;

            List<TareaPendienteBO> tareas = new TareaPendienteBR().Consultar(this.dataContext, new TareaPendienteBO()
            {
                Unidad =
                    new BPMO.SDNI.Equipos.BO.UnidadBO()
                    {
                        UnidadID = mantenimiento.IngresoUnidad.Unidad.UnidadID,
                        NumeroSerie = mantenimiento.IngresoUnidad.Unidad.NumeroSerie
                    },
                Activo = true
            });
            if (tareas.Count != 0)
            {
                this.vista.ListaTareasPendientes = tareas;
                this.vista.MostrarMensaje("Esta unidad y/o modelo tiene tareas pendientes", ETipoMensajeIU.INFORMACION, "Tarea pendiente disponible");
                TareaPendienteBOF tareaPendiente = new TareaPendienteBOF() {
                    Serie = mantenimiento.IngresoUnidad.Unidad.NumeroSerie,
                    Activo = true
                };
                this.vista.PonerEnSesion("TareaPendienteBOF", tareaPendiente);
                this.vista.HabilitarTareasPendientes();
            }
        }
        #endregion

        /// <summary>
        /// Calcula si el equipo se calcula con km u horas
        /// </summary>
        /// <param name="equipo">Equipo con el ID a consultar</param>
        /// <returns>Retorna true si las OS son por KM</returns>
        public bool? IngresarMedida(EquipoBO equipo, EUnidaMedida? unidadMedida)
        {
            bool? ingresa = false;
            if (equipo.Modelo == null)
                return false;
            if (equipo.Modelo.Id == null)
                return false;
            List<ConfiguracionMantenimientoBO> listaConfiguraciones = this.vista.ListaConfiguracionesMantenimiento ?? new List<ConfiguracionMantenimientoBO>();
            if (listaConfiguraciones.Count == 0)
            {
                var configManttoBR = new ConfiguracionMantenimientoBR();
                var configuraciones = configManttoBR.Consultar(dataContext, new ConfiguracionMantenimientoBO() { Modelo = new ModeloBO() { Id = equipo.Modelo.Id }, Estatus = true });
                if (configuraciones.Any())
                    listaConfiguraciones.AddRange(configuraciones);
            }
            else
            {
                if (!listaConfiguraciones.Any(x => x.Modelo.Id == equipo.Modelo.Id && x.UnidadMedida == unidadMedida))
                {
                    var configManttoBR = new ConfiguracionMantenimientoBR();
                    var configuraciones = configManttoBR.Consultar(dataContext, new ConfiguracionMantenimientoBO() { Modelo = new ModeloBO() { Id = equipo.Modelo.Id }, UnidadMedida = unidadMedida, Estatus = true });
                    if (configuraciones.Any())
                        listaConfiguraciones.AddRange(configuraciones);
                }
            }

            this.vista.ListaConfiguracionesMantenimiento = listaConfiguraciones;

            if (listaConfiguraciones.Any(x => x.Modelo.Id == equipo.Modelo.Id && x.UnidadMedida == unidadMedida))
                return true;

            return ingresa;
        }

        #endregion


        /// <summary>
        /// Verifica si al mantenimiento le toca auditoría
        /// </summary>
        /// <returns>True si le toca auditoria, false de lo contrario</returns>
        public bool RealizarAuditoria(MantenimientoUnidadBO mantenimiento) {
            var requiereAuditoria = mantenimiento.RequiereAuditoria != null ? mantenimiento.RequiereAuditoria.Value : false;
            var auditable = mantenimiento.ModeloAuditable;
            
            if (auditable == null || auditable.Value)
                return requiereAuditoria;
            else {
                return false;
            }
        }
        
        private MantenimientoBOF GetByGuid(string guid)
        {
            return this.vista.Mantenimientos.Where(x => x.Guid == guid).FirstOrDefault();
        }

        private MantenimientoBOF GetAliadoByGuid(string guid)
        {
            foreach (var mantenimimento in this.vista.Mantenimientos)
            {
                foreach (var aliado in mantenimimento.Detalles)
                {
                    if (aliado.Guid.Equals(guid))
                    {
                        return aliado;
                    }
                }
            }
            return null;
        }

        private void RemoveByGuid(string guid)
        {
            this.vista.Mantenimientos.RemoveAt(this.vista.Mantenimientos.IndexOf(this.GetByGuid(guid)));
        }

        private string GetGuid()
        {
            return Guid.NewGuid().ToString();
        }

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
                SeguridadBO seguridadBO = this.CrearObjetoSeguridad();

                //Se valida si el usuario tiene permiso a la acción principal
                if (!FacadeBR.ExisteAccion(this.dataContext, "UI INSERTAR", seguridadBO) ||
                    !FacadeBR.ExisteAccion(this.dataContext, "INSERTAR", seguridadBO) ||
                    !FacadeBR.ExisteAccion(this.dataContext, "INSERTARCOMPLETO", seguridadBO) ||
                    !FacadeBR.ExisteAccion(this.dataContext, "CONSULTAR", seguridadBO) ||
                    !FacadeBR.ExisteAccion(this.dataContext, "CONSULTARCOMPLETO", seguridadBO) ||
                    !FacadeBR.ExisteAccion(this.dataContext, "ACTUALIZAR", seguridadBO) ||
                    !FacadeBR.ExisteAccion(this.dataContext, "ACTUALIZARCOMPLETO", seguridadBO) ||
                    !FacadeBR.ExisteAccion(this.dataContext, "REGISTRARDOCUMENTO", seguridadBO))
                    this.vista.RedirigirSinPermisoAcceso();
            }
            catch (Exception ex)
            {
                throw new Exception(nombreClase + ".ValidarAcceso: " + ex.GetBaseException().Message);
            }
        }

        /// <summary>
        /// Crea un objeto de seguridad con los datos actuales de la sesión de usuario en curso
        /// </summary>
        /// <returns>Objeto de tipo seguridad</returns>
        private SeguridadBO CrearObjetoSeguridad()
        {
            UsuarioBO usuario = new UsuarioBO() { Id = this.vista.UsuarioAutenticado };
            AdscripcionBO adscripcion = new AdscripcionBO() { UnidadOperativa = new UnidadOperativaBO() { Id = this.vista.UnidadOperativaId } };
            SeguridadBO seguridadBO = new SeguridadBO(Guid.Empty, usuario, adscripcion);

            return seguridadBO;
        }

        public bool validarCalcomania(MantenimientoUnidadBO mantenimiento)
        {
            bool combustible = false;
            MantenimientoUnidadBO filtro = new MantenimientoUnidadBO
            {
                MantenimientoUnidadId = mantenimiento.MantenimientoUnidadId,
            };
            var resultado = this.mantenimientoUnidadBR.Consultar(dataContext,filtro).FirstOrDefault();
            if (resultado != null)
            {
                if (resultado.CombustibleSalida != null)
                    combustible = true;
                else
                    combustible = false;
            }
            return combustible;
        }

        public bool validarEnviarInformacionCliente(MantenimientoBOF mattoBOF)
        {
            bool finalizado = false;
            MantenimientoUnidadBO filtro = new MantenimientoUnidadBO()
            {
                MantenimientoUnidadId = mattoBOF.MantenimientoUnidad.MantenimientoUnidadId
            };
            var encontrado = this.mantenimientoUnidadBR.Consultar(dataContext, filtro).FirstOrDefault();
            if (encontrado != null)
            {
                if (encontrado.FechaSalida != null)
                    finalizado = true;
                else if (encontrado.FechaSalida == null)
                    finalizado = false;
                   
            }
            return finalizado;
        }

        /// <summary>
        /// Genera un reporte de mantenimientos y lo exporta en un archivo
        /// </summary>
        public void ExportarMantenimientos() {
            try {
                using (MemoryStream ms = new MemoryStream()) {
                    MantenimientoUnidadesRPT reporte = new MantenimientoUnidadesRPT(this.vista.Mantenimientos);
                    reporte.CreateDocument();
                    XlsxExportOptions opts = new XlsxExportOptions();

                    reporte.ExportToXlsx(ms, opts);
                    ms.Seek(0, SeekOrigin.Begin);

                    byte[] report = ms.ToArray();
                    this.vista.Reporte64 = report;
                }
            } catch (Exception exMantto) {
                throw new Exception(nombreClase + ".ExportarMantenimientosXSL: " + exMantto.GetBaseException().Message);
            }

            this.vista.RedirigeExportarReporte();
        }
    }
}
