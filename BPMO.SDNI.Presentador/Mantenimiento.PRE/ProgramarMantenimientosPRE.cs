//Satisface al caso de uso CU025
using System;
using System.Collections.Generic;
using System.Linq;
using BPMO.Basicos.BO;
using BPMO.Facade.SDNI.BOF;
using BPMO.Facade.SDNI.BR;
using BPMO.Patterns.Creational.DataContext;
using BPMO.SDNI.Comun.BO;
using BPMO.SDNI.Comun.BR;
using BPMO.SDNI.Equipos.BO;
using BPMO.SDNI.Equipos.BR;
using BPMO.SDNI.Mantenimiento.BO;
using BPMO.SDNI.Mantenimiento.BOF;
using BPMO.SDNI.Mantenimiento.BR;
using BPMO.SDNI.Mantenimiento.VIS;
using BPMO.SDNI.Tramites.BO;
using BPMO.SDNI.Tramites.BR;
using BPMO.Servicio.Catalogos.BO;
using System.Configuration;

namespace BPMO.SDNI.Mantenimiento.PRE
{
    /// <summary>
    /// Presentador para la ui del programa de mantenimiento
    /// </summary>
    public class ProgramarMantenimientosPRE
    {
        #region Atributos
        /// <summary>
        /// DataContext de la aplicacion
        /// </summary>
        private IDataContext dctx;
        /// <summary>
        /// Vista para obtener datos de la UI
        /// </summary>
        private IProgramarMantenimientosVIS vista;
        /// <summary>
        /// Controlador principal de la UI
        /// </summary>
        private CitaMantenimientoBR controlador;
        /// <summary>
        /// Nombre de la Clase, usado en excepciones
        /// </summary>
        private readonly string nombreClase = typeof(ProgramarMantenimientosPRE).Name;
        /// <summary>
        /// Id del Tipo de Servicio preventivo
        /// </summary>
        private int? TipoMantenimiento
        {
            get
            {
                try
                {
                    int value;
                    if(int.TryParse(ConfigurationManager.AppSettings["TipoServicioPreventivo"], out value))
                        return value;

                    return null;
                }
                catch (Exception)
                {
                    return null;
                }
            }
        }
        #endregion

        #region Constructor
        public ProgramarMantenimientosPRE(IProgramarMantenimientosVIS vista)
        {
            try
            {
                this.vista = vista;
                this.controlador = new CitaMantenimientoBR();
                dctx = FacadeBR.ObtenerConexion();
            }
            catch (Exception ex)
            {
                throw new Exception(nombreClase + ".ProgramarMantenimientosPRE: " + ex.Message);
            }            
        } 
        #endregion

        #region Metodos
        /// <summary>
        /// Metodo Inicial para preparar la busqueda en la interfaz
        /// </summary>
        public void PrepararBusqueda()
        {
            FacadeBR.ConsultarModelosNoAuditables();
            this.vista.LimpiarSesion();
            this.vista.SucursalID = null;
            this.vista.SucursalNombre = String.Empty;
        }
        /// <summary>
        /// Carga inicial de los talleres por las sucursales permitidas
        /// </summary>
        private void PrepararTalleres(List<SucursalBO> sucursalesSeleccionadas)
        {
            List<SucursalBO> sucursales = sucursalesSeleccionadas == null ? new List<SucursalBO>() : sucursalesSeleccionadas;
            if (!sucursales.Any())
            {
                sucursales = FacadeBR.ConsultarSucursalesSeguridad(dctx, new SucursalBOF()
                {
                    UnidadOperativa = new UnidadOperativaBO { Activo = true, Id = this.vista.UnidadOperativaID },
                    Nombre = this.vista.SucursalNombre,
                    Usuario = new UsuarioBO { Activo = true, Id = this.vista.UsuarioID }
                }).Select(x => x as SucursalBO).ToList();
            }
            var talleres = this.ObtenerTalleresPorSucursales(sucursales);
            this.vista.EstablecerTalleres(talleres);
        }
        /// <summary>
        /// Consulta las configuraciones de talleres por sucursales
        /// </summary>
        /// <param name="sucursales">Lista de Sucursales</param>
        private List<TallerBO> ObtenerTalleresPorSucursales(List<SucursalBO> sucursales)
        {
            var config = new ConfiguracionTallerBOF() { Sucursales = sucursales };
            if (this.vista.TallerID != null && this.vista.esExterno != null)
            {
                if (this.vista.esExterno == true)
                { }
                else
                    config.TallerInterno = new TallerBO() { Id = this.vista.TallerID };
            }
            var talleres = new List<TallerBO>();
            sucursales.ForEach(sucursal => { talleres.AddRange(FacadeBR.ConsultarTalleresPorSucursal(dctx, sucursal, this.vista.UnidadOperativaID)); });
            
            return talleres;
        }
        /// <summary>
        /// Consulta los mantenimientos programados
        /// </summary>
        public void ConsultarMantenimientosProgramados()
        {
            try
            {
                int AnoActual = DateTime.Now.Year;
                int MesActual = DateTime.Now.Month;
                int MesAnterior = 1;

                if (MesActual > 1)
                    MesAnterior = MesActual - 1;
                else if (MesActual == 1)
                {
                    MesAnterior = 12;
                    AnoActual = AnoActual - 1;
                }

                var fecha = new DateTime(AnoActual,MesAnterior, 1);
                var fechaFinal = fecha.AddMonths(3).AddDays(-1).AddHours(23).AddMinutes(59).AddSeconds(59);
                var mantenimientoProgramadoBR = new ConsultarMantenimientoProgramadoBR();
                var mantenimientoBOF = new MantenimientoProgramadoBOF() {
                    Auditoria = new AuditoriaBO(),
                    Fecha = fecha,
                    FechaFinal = fechaFinal,
                    EsUnidad = true,
                    Sucursal = new SucursalBO () { Id = this.vista.SucursalID }
                };

                SucursalBO sucursal = new SucursalBO() { Id = this.vista.SucursalID };

                List<DateTime> diasInhabiles = new List<DateTime>();
                if (this.vista.SucursalID != null)
                    diasInhabiles.AddRange(FacadeBR.ConsultarDiasInhabiles(dctx, fecha, fechaFinal, sucursal));

                var listadoMantenimientos = mantenimientoProgramadoBR.ConsultarPreventivosSinRealizar(this.dctx, mantenimientoBOF);
                var listaCitas = new List<CitaMantenimientoBO>();
                var unidadBR = new UnidadBR();
                if (listadoMantenimientos != null && listadoMantenimientos.Count > 0)
                {
                    foreach (var mantenimientoProgramado in listadoMantenimientos)
                    {
                        var agregar = true;
                        var citaMantenimiento = new CitaMantenimientoBO() { MantenimientoProgramado = new MantenimientoProgramadoUnidadBO(), Auditoria = new AuditoriaBO() };
                        var citasUnidad = controlador.Consultar(dctx, new CitaMantenimientoBO() { MantenimientoProgramado = new MantenimientoProgramadoBO() { MantenimientoProgramadoID = mantenimientoProgramado.MantenimientoProgramadoID } });
                        if (citasUnidad != null && citasUnidad.Any())
                        {
                            citaMantenimiento = citasUnidad.FirstOrDefault();
                            citaMantenimiento.MantenimientoProgramado = new MantenimientoProgramadoUnidadBO()
                            {
                                MantenimientoProgramadoID = mantenimientoProgramado.MantenimientoProgramadoID,
                                Activo = mantenimientoProgramado.Activo,
                                EquipoID = mantenimientoProgramado.EquipoID,
                                EstatusMantenimientoProgramado = mantenimientoProgramado.EstatusMantenimientoProgramado,
                                EsUnidad = mantenimientoProgramado.EsUnidad,
                                Fecha = mantenimientoProgramado.Fecha,
                                Horas = mantenimientoProgramado.Horas,
                                Km = mantenimientoProgramado.Km,
                                TipoMantenimiento = mantenimientoProgramado.TipoMantenimiento,
                            };

                            if (this.vista.SucursalID != null && this.vista.TallerID != null)
                            //if (this.vista.TallerID != null)
                            {
                                if (citaMantenimiento.Sucursal.Id != this.vista.SucursalID && citaMantenimiento.TallerInterno.Id != this.vista.TallerID)
                                //if (citaMantenimiento.TallerInterno.Id != this.vista.TallerID)
                                    agregar = false;
                                else
                                    (citaMantenimiento.MantenimientoProgramado as MantenimientoProgramadoUnidadBO).Unidad = this.ObtenerUnidad(mantenimientoProgramado.EquipoID, null);
                            }
                            else
                            {
                                if (this.vista.SucursalID != null && this.vista.TallerID == null)
                                {
                                    if (citaMantenimiento.Sucursal.Id != this.vista.SucursalID)
                                        agregar = false;
                                    else
                                        (citaMantenimiento.MantenimientoProgramado as MantenimientoProgramadoUnidadBO).Unidad = this.ObtenerUnidad(mantenimientoProgramado.EquipoID, null);
                                }
                                else
                                {
                                    (citaMantenimiento.MantenimientoProgramado as MantenimientoProgramadoUnidadBO).Unidad = this.ObtenerUnidad(mantenimientoProgramado.EquipoID, null);
                                }
                            }
                        }
                        else
                        {
                            citaMantenimiento.MantenimientoProgramado = new MantenimientoProgramadoUnidadBO()
                            {
                                MantenimientoProgramadoID = mantenimientoProgramado.MantenimientoProgramadoID,
                                Activo = mantenimientoProgramado.Activo,
                                EquipoID = mantenimientoProgramado.EquipoID,
                                EstatusMantenimientoProgramado = mantenimientoProgramado.EstatusMantenimientoProgramado,
                                EsUnidad = mantenimientoProgramado.EsUnidad,
                                Fecha = mantenimientoProgramado.Fecha,
                                Horas = mantenimientoProgramado.Horas,
                                Km = mantenimientoProgramado.Km,
                                TipoMantenimiento = mantenimientoProgramado.TipoMantenimiento,
                            };
                            (citaMantenimiento.MantenimientoProgramado as MantenimientoProgramadoUnidadBO).Unidad = this.ObtenerUnidad(mantenimientoProgramado.EquipoID, null);
                        }

                        if (agregar)
                        {
                            if (listaCitas.Count(x => x.MantenimientoProgramado.EquipoID == mantenimientoProgramado.EquipoID && x.MantenimientoProgramado.Fecha == mantenimientoProgramado.Fecha) > 0)
                            {}
                            else{ listaCitas.Add(citaMantenimiento);}

                            
                        }
                    }

                }
                else
                {
                    this.vista.MostrarMensaje("No se encontro ningun mantenimiento programado para este mes", Primitivos.Enumeradores.ETipoMensajeIU.INFORMACION);
                }
               
                this.vista.EstablecerMantenimientos(listaCitas, diasInhabiles);
            }
            catch (Exception ex)
            {
                throw new Exception(nombreClase + ": " + ex.Message);
            }
        }
        /// <summary>
        /// Presenta el detalle de una Cita de Mantenimiento Seleccionada
        /// </summary>
        /// <param name="id">Identificador de la cita o el mantenimiento programado</param>
        /// <param name="esCita">Determina si el ID es para una Cita de Mantenimiento</param>
        public void PresentarDetalles(int id, bool esCita)
        {
            if (this.vista.CitasMantenimiento != null && this.vista.CitasMantenimiento.Any())
            {
                CitaMantenimientoBO cita = null;
                if (esCita)
                {
                    cita = this.vista.CitasMantenimiento.FirstOrDefault(x => x.CitaMantenimientoID == id);
                }
                else
                {
                    cita = this.vista.CitasMantenimiento.FirstOrDefault(x => x.MantenimientoProgramado.MantenimientoProgramadoID == id);
                }
                
                BPMO.SDNI.Equipos.BO.UnidadBO unidad = (cita.MantenimientoProgramado as MantenimientoProgramadoUnidadBO).Unidad;
                UnidadBR unidadBR = new UnidadBR();

                TramiteProxyBO filter = new TramiteProxyBO() { Activo = true, Tramitable = new BPMO.SDNI.Equipos.BO.UnidadBO { UnidadID = unidad.UnidadID } };
                TramiteBR tramiteBR = new TramiteBR();
                List<TramiteBO> lstTramites = tramiteBR.ConsultarCompleto(dctx, filter, false);
                TramiteBO tramitePlacaFederal = lstTramites.Find(p => p.Tipo != null && p.Tipo == ETipoTramite.PLACA_FEDERAL && p.Activo != null && p.Activo == true);
                TramiteBO tramitePlacaEstatal = lstTramites.Find(p => p.Tipo != null && p.Tipo == ETipoTramite.PLACA_ESTATAL && p.Activo != null && p.Activo == true);
                String placas = tramitePlacaFederal != null && !String.IsNullOrEmpty(tramitePlacaFederal.Resultado)
                                    ? tramitePlacaFederal.Resultado :
                                    (tramitePlacaEstatal != null && !String.IsNullOrEmpty(tramitePlacaEstatal.Resultado) ? tramitePlacaEstatal.Resultado : null);

                var equiposAliados = unidadBR.ConsultarEquipoAliado(dctx, unidad, true);
                List<MantenimientoProgramadoEquipoAliadoBO> mantenimientosEA = new List<MantenimientoProgramadoEquipoAliadoBO>();
                var mantenimientoProgramadoBR = new ConsultarMantenimientoProgramadoBR();

                foreach(EquipoAliadoBO equipoAliado in equiposAliados)
                {
                    var manttosEA = mantenimientoProgramadoBR.Consultar(dctx, new MantenimientoProgramadoBOF() { EsUnidad = false, EquipoID = equipoAliado.EquipoID, Activo = true, EstatusMantenimientoProgramado = EEstatusMantenimientoProgramado.PROGRAMADO });
                    if (manttosEA != null && manttosEA.Any())
                    {
                        var manttoEA = manttosEA.FirstOrDefault() as MantenimientoProgramadoEquipoAliadoBO;
                        manttoEA.EquipoAliado = equipoAliado;
                        
                        mantenimientosEA.Add(manttoEA);
                    }
                    else
                    {
                        mantenimientosEA.Add(new MantenimientoProgramadoEquipoAliadoBO() { EquipoAliado = equipoAliado });
                    }
                }

                var nombreSucursal = "";
                var nombreTaller = "";
                int? kmUltimoServicio = null;
                DateTime? fechaUltimoServicio = null;
                string clienteNombre = "";
                List<ContactoClienteBO> contactosCliente = new List<ContactoClienteBO>();

                if (cita.CitaMantenimientoID != null)
                {
                    var sucursal = FacadeBR.ConsultarSucursal(dctx, new SucursalBO() { Id = cita.Sucursal.Id }).FirstOrDefault();
                    if (sucursal != null)
                    {
                        cita.Sucursal.Nombre = sucursal.Nombre;
                        nombreSucursal = sucursal.Nombre;
                    }

                    var taller = FacadeBR.ConsultarTaller(dctx, new TallerBO() { Id = cita.TallerInterno.Id }).FirstOrDefault();
                    if (taller != null)
                    {
                        cita.TallerInterno.Nombre = taller.Nombre;
                        cita.TallerInterno.NombreCorto = taller.NombreCorto;
                        nombreTaller = taller.Nombre;
                    }
                }

                var unidadManttoBR = new MantenimientoUnidadBR();
                var mantemiento = unidadManttoBR.Consultar(dctx, new MantenimientoUnidadBO()
                {
                    IngresoUnidad = new IngresoUnidadBO()
                    {
                        Unidad = (cita.MantenimientoProgramado as MantenimientoProgramadoUnidadBO).Unidad
                    },
                    TipoServicio = new TipoServicioBO() { Id = this.TipoMantenimiento },
                    Activo = true
                }).Where(x=>x.FechaSalida != null).OrderBy(x=>x.FechaSalida).ToList().LastOrDefault();
                if (mantemiento != null)
                {
                    kmUltimoServicio = mantemiento.KilometrajeEntrada;
                    fechaUltimoServicio = mantemiento.FechaSalida.Value;
                }

                if ((cita.MantenimientoProgramado as MantenimientoProgramadoUnidadBO).Unidad.Cliente != null
                    && (cita.MantenimientoProgramado as MantenimientoProgramadoUnidadBO).Unidad.Cliente.Id != null)
                {
                    var clienteIdealeaseBR = new CuentaClienteIdealeaseBR();
                    var cliente = clienteIdealeaseBR.Consultar(dctx, new CuentaClienteIdealeaseBO()
                    {
                        Cliente = new ClienteBO() { Id = (cita.MantenimientoProgramado as MantenimientoProgramadoUnidadBO).Unidad.Cliente.Id },
                        UnidadOperativa = new UnidadOperativaBO() { Id = this.vista.UnidadOperativaID }
                    }).FirstOrDefault();

                    if (cliente != null)
                    {
                        bool? activo = cita.CitaMantenimientoID != null ? null : (bool?)true;
                        var contactos = new ContactoClienteBR().ConsultarCompleto(dctx, new ContactoClienteBO() { CuentaClienteIdealease = cliente, Activo = activo, Sucursal = cita.Sucursal });
                        if (contactos != null && contactos.Any()) 
                        {
                            contactos.ForEach(contacto => {
                                contacto.Detalles.ForEach(detail => {
                                    detail.ContactoCliente.Sucursal = new SucursalBO() { Id = contacto.Sucursal.Id, Nombre = contacto.Sucursal.Nombre };
                                    detail.ContactoCliente.Direccion = contacto.Direccion;
                                });
                            });

                            contactosCliente.AddRange(contactos);
                        }
                            
                        clienteNombre = cliente.Cliente.NombreCompleto;
                    }
                    else {
                        var clienteServicio = FacadeBR.ConsultarCliente(dctx, new ClienteBO() { Id = (cita.MantenimientoProgramado as MantenimientoProgramadoUnidadBO).Unidad.Cliente.Id}).FirstOrDefault();
                        if (clienteServicio != null)
                        {
                            clienteNombre = clienteServicio.NombreCompleto;
                        }
                    }
                }

                var fechaInicial = new DateTime();
                if(mantemiento.FechaUltimoServicio.HasValue)
                    fechaInicial = mantemiento.FechaArranque.Value > mantemiento.FechaUltimoServicio.Value 
                        && mantemiento.FechaArranque.Value < mantemiento.FechaProximoServicio.Value ? mantemiento.FechaArranque.Value : DateTime.Today;
                else
                    fechaInicial = DateTime.Today;

                var diasRetraso = Convert.ToInt32(Math.Round((fechaInicial - new DateTime(cita.MantenimientoProgramado.Fecha.Value.Year, cita.MantenimientoProgramado.Fecha.Value.Month, cita.MantenimientoProgramado.Fecha.Value.Day)).TotalDays,2));
                this.vista.ClienteNombre = clienteNombre;
                this.vista.Area = (cita.MantenimientoProgramado as MantenimientoProgramadoUnidadBO).Unidad.Area.ToString();
                this.vista.VINUnidad= (cita.MantenimientoProgramado as MantenimientoProgramadoUnidadBO).Unidad.NumeroSerie;
                this.vista.NumeroEconomico = (cita.MantenimientoProgramado as MantenimientoProgramadoUnidadBO).Unidad.NumeroEconomico;
                this.vista.KmUltimoServicio = kmUltimoServicio;
                this.vista.FechaUltimoServicio = fechaUltimoServicio;
                this.vista.PlacaEstatal = tramitePlacaEstatal != null && !String.IsNullOrEmpty(tramitePlacaEstatal.Resultado) ? tramitePlacaEstatal.Resultado : "";
                this.vista.PlacaFederal = tramitePlacaFederal != null && !String.IsNullOrEmpty(tramitePlacaFederal.Resultado) ? tramitePlacaFederal.Resultado : "" ;
                this.vista.FechaSugerida = cita.MantenimientoProgramado.Fecha;
                this.vista.TipoMantenimiento = (cita.MantenimientoProgramado as MantenimientoProgramadoUnidadBO).TipoMantenimientoNombre;
                this.vista.NombreSucursalDetalle = nombreSucursal;
                this.vista.NombreTallerDetalle = nombreTaller;
                this.vista.ListadoManttoEquiposAliados = mantenimientosEA;
                this.vista.EstatusMantenimiento = cita.CitaMantenimientoID != null ? cita.EstatusCita.ToString() : EEstatusCita.PRECALENDARIZADA.ToString();
                this.vista.DiasRetraso = diasRetraso <= 0 ? int.Parse(diasRetraso.ToString("G")) : int.Parse(diasRetraso.ToString());
                this.vista.ListadoContactosCliente = contactosCliente;

                this.vista.EstablecerEquiposAliados();
                this.vista.EstablecerContactosCliente();
            }
            else
                throw new Exception("No hay citas de mantenimiento para presentar");
            
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
                if (!FacadeBR.ExisteAccion(this.dctx, "CONSULTAR", seguridadBO) || !FacadeBR.ExisteAccion(this.dctx, "UI CONSULTAR", seguridadBO))
                    this.vista.RedirigirSinPermisoAcceso();
            }
            catch (Exception ex)
            {
                throw new Exception(nombreClase + ".ValidarAcceso: " + ex.GetBaseException().Message);
            }
        }
        /// <summary>
        /// Valida que el usuario cuente con el permiso para redirigirse a la interfaz de registro o actualizacion
        /// </summary>
        /// <param name="esRecalendarizar">Determina si es redireccion a recalendarizar una cita</param>
        public void ValidarRegistro(bool esRecalendarizar)
        {
            try
            {
                //Valida que usuario y la unidad operativa no sean nulos
                if (this.vista.UsuarioID == null) throw new Exception("El identificador del usuario no debe ser nulo");
                if (this.vista.UnidadOperativaID == null) throw new Exception("El identificador de la Unidad Operativa no debe ser nulo");
                //Se crea el objeto de seguridad                
                SeguridadBO seguridadBO = this.CrearObjetoSeguridad();

                //Se valida si el usuario tiene permiso a la acción principal
                if (!FacadeBR.ExisteAccion(this.dctx, esRecalendarizar ? "ACTUALIZAR" : "INSERTAR", seguridadBO))
                    this.vista.RedirigirSinPermisoAcceso();
            }
            catch (Exception ex)
            {
                throw new Exception(nombreClase + ".ValidarRegistro: " + ex.GetBaseException().Message);
            }
        }
        /// <summary>
        /// Consulta la unidad en la bd de idealease
        /// </summary>
        /// <param name="equipoId">Identificador del equipo</param>
        /// <param name="unidadId">identificador de la unidad</param>
        /// <returns>Objeto UnidadBO</returns>
        public BPMO.SDNI.Equipos.BO.UnidadBO ObtenerUnidad(int? equipoId, int? unidadId)
        {
            var unidadBR = new UnidadBR();
            return unidadBR.Consultar(dctx, new BPMO.SDNI.Equipos.BO.UnidadBO() { EquipoID = equipoId, UnidadID = unidadId }).FirstOrDefault();
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
        #region Métodos para el Buscador
        public object PrepararBOBuscador(string catalogo)
        {
            object obj = null;

            switch (catalogo)
            {
                case "Sucursal":
                    SucursalBOF sucursal = new SucursalBOF();
                    sucursal.UnidadOperativa = new UnidadOperativaBO { Activo = true, Id = this.vista.UnidadOperativaID };
                    sucursal.Nombre = this.vista.SucursalNombre;
                    sucursal.Usuario = new UsuarioBO { Activo = true, Id = this.vista.UsuarioID };
                    obj = sucursal;
                    break;
            }

            return obj;
        }
        public void DesplegarResultadoBuscador(string catalogo, object selecto)
        {
            switch (catalogo)
            {
                case "Sucursal":
                    #region Desplegar Sucursal
                    SucursalBO sucursal = (SucursalBO)selecto;
                    if (sucursal != null && sucursal.Id != null)
                        this.vista.SucursalID = sucursal.Id;
                    else
                        this.vista.SucursalID = null;

                    if (sucursal != null && sucursal.Nombre != null)
                        this.vista.SucursalNombre = sucursal.Nombre;
                    else
                        this.vista.SucursalNombre = null;
                    #endregion

                    if (sucursal != null && sucursal.Id != null)
                    {
                        try
                        {
                            this.PrepararTalleres(new List<SucursalBO>() { new SucursalBO() { Id = sucursal.Id } });
                        }
                        catch (Exception ex)
                        {
                            throw new Exception("DesplegarResultadoBuscador: No se pudieron recuperar los Talleres, " + ex.Message);
                        }
                    }
                    break;
            }
        }
        #endregion
        #endregion
    }
}
