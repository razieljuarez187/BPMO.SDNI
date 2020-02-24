//Satisface al caso de uso CU026 - Calendarizar Mantenimiento
//Satisface al caso de uso CU030 - Recalendarizar Mantenimiento
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
using BPMO.Primitivos.Enumeradores;

namespace BPMO.SDNI.Mantenimiento.PRE
{
    /// <summary>
    /// Presentador para el UC de los datos de una cita de mantenimiento
    /// </summary>
    public class ucDatosCitaMantenimientoPRE
    {
        #region Atributos
        /// <summary>
        /// DataContext de la aplicacion
        /// </summary>
        private IDataContext dctx;
        /// <summary>
        /// Vista para obtener datos de la UI
        /// </summary>
        private IucDatosCitaMantenimientoVIS vista;
        /// <summary>
        /// Nombre de la Clase, usado en excepciones
        /// </summary>
        private readonly string nombreClase = typeof(ucDatosCitaMantenimientoPRE).Name;
        #endregion
        #region Propiedades
        #endregion
        #region Constructor
        /// <summary>
        /// Constructor del Presentador
        /// </summary>
        public ucDatosCitaMantenimientoPRE(IucDatosCitaMantenimientoVIS vista)
        {
            try
            {
                this.vista = vista;
                dctx = FacadeBR.ObtenerConexion();
            }
            catch (Exception ex)
            {
                throw new Exception(nombreClase + ".ucDatosCitaMantenimientoPRE: " + ex.Message);
            }
        } 
        #endregion
        #region Metodos
        /// <summary>
        /// Inicia la interfaz en el primer acceso
        /// </summary>
        public void IniciarVista()
        {
            this.vista.PrepararNuevo();
            this.PresentarDetalles(this.vista.ObtenerPaqueteNavegacion("CitaMantenimientoBO") as CitaMantenimientoBO);
        }
        /// <summary>
        /// El objeto que recibe lo presenta en la interfaz
        /// </summary>
        public void ObjetoAInterfazUsuario(object objeto, List<MantenimientoProgramadoEquipoAliadoBO> manttoEquiposAliados, List<ContactoClienteBO> contactosCliente, string placaFederal, string placaEstatal)
        {
            CitaMantenimientoBO cita = (CitaMantenimientoBO)objeto;

            this.vista.VINUnidad = (cita.MantenimientoProgramado as MantenimientoProgramadoUnidadBO).Unidad.NumeroSerie;
            this.vista.ClienteID = (cita.MantenimientoProgramado as MantenimientoProgramadoUnidadBO).Unidad.Cliente != null ? (cita.MantenimientoProgramado as MantenimientoProgramadoUnidadBO).Unidad.Cliente.Id : null;
            this.vista.ClienteNombre = (cita.MantenimientoProgramado as MantenimientoProgramadoUnidadBO).Unidad.Cliente != null ? (cita.MantenimientoProgramado as MantenimientoProgramadoUnidadBO).Unidad.Cliente.NombreCompleto : String.Empty;
            this.vista.Area = (cita.MantenimientoProgramado as MantenimientoProgramadoUnidadBO).Unidad.Area.ToString();
            this.vista.NumeroEconomico = (cita.MantenimientoProgramado as MantenimientoProgramadoUnidadBO).Unidad.NumeroEconomico;
            this.vista.PlacaEstatal = placaEstatal;
            this.vista.PlacaFederal = placaFederal;
            this.vista.TipoMantenimiento = (cita.MantenimientoProgramado as MantenimientoProgramadoUnidadBO).TipoMantenimientoNombre;
            this.vista.TiempoMantenimiento = cita.TiempoMantenimiento != null ? cita.TiempoMantenimiento : this.ObtenerTiempoMantenimiento(cita.MantenimientoProgramado as MantenimientoProgramadoUnidadBO, manttoEquiposAliados);
            this.vista.EstatusCita = cita.EstatusCita == null ? EEstatusCita.CALENDARIZADA : cita.EstatusCita;
            this.vista.ListadoManttoEquiposAliados = manttoEquiposAliados;
            this.vista.ListadoContactosCliente = contactosCliente;
            this.vista.FechaCita = cita.FechaHoraCita;
            this.vista.HoraCita = cita.FechaHoraCita != null ? (TimeSpan?)new TimeSpan(cita.FechaHoraCita.Value.Hour, cita.FechaHoraCita.Value.Minute, cita.FechaHoraCita.Value.Second) : null;
            this.vista.CitaMantenimientoID = cita.CitaMantenimientoID;
            this.vista.UnidadID = (cita.MantenimientoProgramado as MantenimientoProgramadoUnidadBO).Unidad.UnidadID;
            this.vista.EquipoID = (cita.MantenimientoProgramado as MantenimientoProgramadoUnidadBO).Unidad.EquipoID;
            this.vista.MantenimientoProgramadoID = cita.MantenimientoProgramado.MantenimientoProgramadoID;
            this.vista.esExterno = false;
            this.vista.ContactoClienteID = cita.ContactoCliente != null && cita.ContactoCliente.ContactoClienteID != null ? cita.ContactoCliente.ContactoClienteID : null;
            if (cita.Sucursal != null && cita.Sucursal.Id != null)
            {
                this.vista.SucursalID = cita.Sucursal.Id;
                this.vista.SucursalNombre = cita.Sucursal.Nombre;

                this.PrepararTalleres(new List<SucursalBO>() { new SucursalBO() { Id = cita.Sucursal.Id } });
                this.vista.TallerID =cita.TallerInterno.Id;
            }
            
            this.vista.EstablecerEquiposAliados();
            this.vista.EstablecerContactosCliente();
        }
        /// <summary>
        /// Retorna un objeto de CitaMantenimientoBO
        /// </summary>
        /// <returns></returns>
        public Object InterfazUsuarioAObjeto()
        {
            CitaMantenimientoBO cita = new CitaMantenimientoBO() { Auditoria = new AuditoriaBO(), MantenimientoProgramado = new MantenimientoProgramadoUnidadBO(), ContactoCliente = new ContactoClienteBO(){ CuentaClienteIdealease = new CuentaClienteIdealeaseBO(){ Cliente = new ClienteBO() } }, Sucursal = new SucursalBO() };

            cita.CitaMantenimientoID = this.vista.CitaMantenimientoID;
            cita.EstatusCita = this.vista.EstatusCita;
            cita.FechaHoraCita = new DateTime(this.vista.FechaCita.Value.Year, this.vista.FechaCita.Value.Month, this.vista.FechaCita.Value.Day,
                this.vista.HoraCita.Value.Hours, this.vista.HoraCita.Value.Minutes, this.vista.HoraCita.Value.Seconds);
            cita.MantenimientoProgramado.MantenimientoProgramadoID = this.vista.MantenimientoProgramadoID;
            cita.MantenimientoProgramado.TipoMantenimiento = (ETipoMantenimiento)Enum.Parse(typeof(ETipoMantenimiento), this.vista.TipoMantenimiento);
            cita.MantenimientoProgramado.EquipoID = this.vista.EquipoID;
            cita.MantenimientoProgramado.EsUnidad = true;
            (cita.MantenimientoProgramado as MantenimientoProgramadoUnidadBO).Unidad = new BPMO.SDNI.Equipos.BO.UnidadBO() { UnidadID = this.vista.UnidadID };
            cita.TiempoMantenimiento = this.vista.TiempoMantenimiento;
            cita.Auditoria.FC = DateTime.Now;
            cita.Auditoria.UC = this.vista.UsuarioID;
            cita.Auditoria.FUA = DateTime.Now;
            cita.Auditoria.UUA = this.vista.UsuarioID;
            if (this.vista.esExterno == true)
            {                
            }
            else
            {
                cita.TallerInterno = new TallerBO()
                {
                    Id = this.vista.TallerID,
                    Nombre = this.vista.NombreTaller
                };
            }
            cita.ContactoCliente.ContactoClienteID = this.vista.ContactoClienteID;
            cita.ContactoCliente.CuentaClienteIdealease.Cliente.Id = this.vista.ClienteID;
            cita.ContactoCliente.CuentaClienteIdealease.Cliente.NombreCompleto = this.vista.ClienteNombre;
            cita.Sucursal.Id = this.vista.SucursalID;
            cita.Sucursal.Nombre = this.vista.SucursalNombre;

            return cita;
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
            var talleres = new List<TallerBO>();
            sucursales.ForEach(x => { talleres.AddRange(FacadeBR.ConsultarTalleresPorSucursal(dctx, x, this.vista.UnidadOperativaID)); });
                
            return talleres;
        }
        /// <summary>
        /// Obtiene el tiempo que tomara realizar el trabajo de la unidad y de sus equipos aliados
        /// </summary>
        /// <param name="mantenimientoUnidad">Mantenimiento programado de unidad</param>
        /// <param name="manttoEquiposAliados">Manteniminetos programados para los equipos aliados</param>
        /// <returns>Tiempo total de mantenimiento</returns>
        private decimal? ObtenerTiempoMantenimiento(MantenimientoProgramadoUnidadBO mantenimientoUnidad, List<MantenimientoProgramadoEquipoAliadoBO> manttoEquiposAliados)
        {
            decimal? tiempo = 0;
            var configuracionPosicionTrabajo = new ConfiguracionPosicionTrabajoBO()
            {
                ConfiguracionModeloMotorizacion = new ConfiguracionModeloMotorizacionBO()
                {
                    Modelo = mantenimientoUnidad.Unidad.Modelo
                },
                DescriptorTrabajo = new DescriptorTrabajoBO
                {
                    Nombre = mantenimientoUnidad.TipoMantenimiento.ToString()
                }
            };

            var configPosicion = FacadeBR.ConsultarConfiguracionPosicionTrabajoDetalle(dctx, configuracionPosicionTrabajo);
            configPosicion.ForEach(x => { tiempo = tiempo + x.TiempoSRT; });

            manttoEquiposAliados.Where(x => x.EquipoAliado.Modelo != null && x.EquipoAliado.Modelo.Id != null).ToList().ForEach(mantto =>
            {
                configuracionPosicionTrabajo.ConfiguracionModeloMotorizacion.Modelo = mantto.EquipoAliado.Modelo;
                configuracionPosicionTrabajo.DescriptorTrabajo.Nombre = mantto.TipoMantenimiento.ToString();

                var configEAliado = FacadeBR.ConsultarConfiguracionPosicionTrabajoDetalle(dctx, configuracionPosicionTrabajo);
                configEAliado.ForEach(x => { tiempo = tiempo + x.TiempoSRT; });
            });


            return Math.Round(tiempo.Value, 2);
        }
        /// <summary>
        /// Presenta el detalle de una Cita de Mantenimiento Seleccionada
        /// </summary>
        /// <param name="id">Identificador de la cita o el mantenimiento programado</param>
        /// <param name="esCita">Determina si el ID es para una Cita de Mantenimiento</param>
        public void PresentarDetalles(CitaMantenimientoBO citaMantenimiento)
        {
            if (citaMantenimiento != null)
            {
                CitaMantenimientoBO cita = citaMantenimiento;

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

                foreach (EquipoAliadoBO equipoAliado in equiposAliados)
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

                List<ContactoClienteBO> contactosCliente = new List<ContactoClienteBO>();

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
                            contactos.ForEach(contacto =>
                            {
                                contacto.Detalles.ForEach(detail =>
                                {
                                    detail.ContactoCliente.Sucursal = new SucursalBO() { Id = contacto.Sucursal.Id, Nombre = contacto.Sucursal.Nombre };
                                    detail.ContactoCliente.Direccion = contacto.Direccion;
                                });
                            });

                            contactosCliente.AddRange(contactos);
                        }

                        (cita.MantenimientoProgramado as MantenimientoProgramadoUnidadBO).Unidad.Cliente.NombreCompleto = cliente.Cliente.NombreCompleto;
                    }
                    else
                    {
                        var clienteServicio = FacadeBR.ConsultarCliente(dctx, new ClienteBO() { Id = (cita.MantenimientoProgramado as MantenimientoProgramadoUnidadBO).Unidad.Cliente.Id }).FirstOrDefault();
                        if (clienteServicio != null)
                        {
                            (cita.MantenimientoProgramado as MantenimientoProgramadoUnidadBO).Unidad.Cliente.NombreCompleto = clienteServicio.NombreCompleto;
                        }
                    }
                }

                this.ObjetoAInterfazUsuario(cita, mantenimientosEA, contactosCliente,
                    tramitePlacaFederal != null && !String.IsNullOrEmpty(tramitePlacaFederal.Resultado) ? tramitePlacaFederal.Resultado : String.Empty,
                    tramitePlacaEstatal != null && !String.IsNullOrEmpty(tramitePlacaEstatal.Resultado) ? tramitePlacaEstatal.Resultado : String.Empty);
                
            }
            else
                throw new Exception("No hay citas de mantenimiento para presentar");

        }
        /// <summary>
        /// Valida los campos Requeridos para Registrar un Contrato en Curso
        /// </summary>
        /// <returns></returns>
        public string ValidarCamposRegistro()
        {
            string s = string.Empty;

            if (this.vista.FechaCita == null)
                s += "Fecha de Cita, ";
            if (this.vista.TallerID == null)
                s += "Taller, ";
            if (this.vista.HoraCita == null)
                s += "Hora de la Cita, ";
            if (this.vista.SucursalID == null)
                s += "Sucursal, ";

            if (s != null && s.Trim().CompareTo("") != 0)
                return "Los siguientes campos no pueden estar vacíos: \n" + s.Substring(0, s.Length - 2);

            return null;
        }
        /// <summary>
        /// Obtiene nuevamente el cliente de la unidad y consulta los contactos del cliente
        /// </summary>
        public void ActualizarContactosCliente()
        {
            int? clienteId = this.vista.ClienteID;
            if (clienteId != null)
            {
                List<ContactoClienteBO> contactosCliente = new List<ContactoClienteBO>();
                var clienteIdealeaseBR = new CuentaClienteIdealeaseBR();
                var cliente = clienteIdealeaseBR.Consultar(dctx, new CuentaClienteIdealeaseBO()
                {
                    Cliente = new ClienteBO() { Id = clienteId },
                    UnidadOperativa = new UnidadOperativaBO() { Id = this.vista.UnidadOperativaID }
                }).FirstOrDefault();

                if (cliente != null)
                {
                    bool? activo = this.vista.CitaMantenimientoID != null ? null : (bool?)true;
                    var contactos = new ContactoClienteBR().ConsultarCompleto(dctx, new ContactoClienteBO() { CuentaClienteIdealease = cliente, Activo = activo, Sucursal = new SucursalBO() { Id = this.vista.SucursalID } });
                    if (contactos != null && contactos.Any())
                    {
                        contactos.ForEach(contacto =>
                        {
                            contacto.Detalles.ForEach(detail =>
                            {
                                detail.ContactoCliente.Sucursal = new SucursalBO() { Id = contacto.Sucursal.Id, Nombre = contacto.Sucursal.Nombre };
                                detail.ContactoCliente.Direccion = contacto.Direccion;
                            });
                        });

                        contactosCliente.AddRange(contactos);
                    }
                }

                this.vista.ListadoContactosCliente = contactosCliente;
                this.vista.EstablecerContactosCliente();
            }
        }
        /// <summary>
        /// Permite validar si el modelo se encuantra en la lista de los no auditables
        /// </summary>
        /// <param name="modelo"></param> modelo a validar
        /// <returns></returns>
        public bool ValidarModeloAuditable(ModeloBO modelo)
        {
            List<ModeloBO> modelosNoAuditables = FacadeBR.ConsultarModelosNoAuditables();
            bool ModeloAuditable = modelosNoAuditables.Exists(x => x.Id == modelo.Id);

            return ModeloAuditable;
        }
        /// <summary>
        /// Determina si la fecha seleccionada es un dia inhabil
        /// </summary>
        /// <returns>Retorna TRUE si el dia es inhabil</returns>
        public bool validarFechaHabil()
        {
            bool Habil = false;

            if (this.vista.FechaCita.Value.DayOfWeek == DayOfWeek.Sunday)
                return Habil;

            SucursalBO sucursal = new SucursalBO() { Id = this.vista.SucursalID };
            Habil = FacadeBR.ValidarDiaHabil(dctx,this.vista.FechaCita.Value, sucursal);

            return Habil;
        }
        /// <summary>
        /// Determina si por el modelo de la unidad se debe enviar a un taller externo
        /// </summary>
        /// <returns>Retorna TRUE si el mantto se realiza en un taller externo</returns>
        public bool EsTallerExterno()
        {
            UnidadBR unidadBR = new UnidadBR();
            var unidad = unidadBR.ConsultarUnidadBOPorModelo(dctx, new BPMO.SDNI.Equipos.BO.UnidadBO() { NumeroSerie = this.vista.VINUnidad, Modelo = new ModeloBO() }).FirstOrDefault();

            var modelosSinAuditoria = FacadeBR.ConsultarModelosNoAuditables();
            if (modelosSinAuditoria.Any())
            {
                if (modelosSinAuditoria.Any(x => x.Id == unidad.Modelo.Id))
                    return true;
            }

            return false;
        }
        /// <summary>
        /// Determina de acuerdo a la capacidad del taller si el limite ya se ha superado
        /// </summary>
        /// <param name="fecha"></param>
        /// <returns></returns>
        public bool LimiteTallerSuperado(DateTime? fecha)
        {
            var tipoMantto = (ETipoMantenimiento)Enum.Parse(typeof(ETipoMantenimiento), this.vista.TipoMantenimiento);
            var fechaInicial = new DateTime(fecha.Value.Year, fecha.Value.Month, fecha.Value.Day);
            var fechaFinal = new DateTime(fecha.Value.Year, fecha.Value.Month, fecha.Value.Day, 23, 59, 59);

            do
            {
                if (fechaInicial.DayOfWeek != DayOfWeek.Monday)
                    fechaInicial = fechaInicial.AddDays(-1);
            } while (fechaInicial.DayOfWeek != DayOfWeek.Monday);
            do
            {
                if (fechaFinal.DayOfWeek != DayOfWeek.Sunday)
                    fechaFinal = fechaFinal.AddDays(1);
            } while (fechaFinal.DayOfWeek != DayOfWeek.Sunday);

            decimal tiempoPorDia = FacadeBR.ConsultarTiempoDisponibleTaller(dctx, this.vista.TallerID, this.vista.SucursalID, this.vista.UnidadOperativaID, fechaInicial, fechaFinal, fecha.Value.DayOfWeek == DayOfWeek.Saturday);

            CitaMantenimientoBR citaMantenimientoBR = new CitaMantenimientoBR();
            var citas = new List<CitaMantenimientoBO>();
            citas.AddRange(citaMantenimientoBR.ConsultarPorRangoFechas(dctx, new CitaMantenimientoBOF()
            {
                FechaInicio = new DateTime(fecha.Value.Year, fecha.Value.Month, fecha.Value.Day),
                FechaFin = new DateTime(fecha.Value.Year, fecha.Value.Month, fecha.Value.Day, 23, 59, 59),
                Estatus = EEstatusCita.CALENDARIZADA
            }));
            citas.AddRange(citaMantenimientoBR.ConsultarPorRangoFechas(dctx, new CitaMantenimientoBOF()
            {
                FechaInicio = new DateTime(fecha.Value.Year, fecha.Value.Month, fecha.Value.Day),
                FechaFin = new DateTime(fecha.Value.Year, fecha.Value.Month, fecha.Value.Day, 23, 59, 59),
                Estatus = EEstatusCita.RECALENDARIZADA
            }));
            citas.AddRange(citaMantenimientoBR.ConsultarPorRangoFechas(dctx, new CitaMantenimientoBOF()
            {
                FechaInicio = new DateTime(fecha.Value.Year, fecha.Value.Month, fecha.Value.Day),
                FechaFin = new DateTime(fecha.Value.Year, fecha.Value.Month, fecha.Value.Day, 23, 59, 59),
                Estatus = EEstatusCita.INICIADA
            }));

            UnidadBR unidadBR = new UnidadBR();
            ConsultarMantenimientoProgramadoBR mantenimientoProgramadoBR = new ConsultarMantenimientoProgramadoBR();

            decimal tiempoConsumido = 0;

            foreach (var cita in citas.Where(x=>(x.MantenimientoProgramado as MantenimientoProgramadoUnidadBO).Unidad.UnidadID != this.vista.UnidadID).ToList())
            {
                var manttoProgramado = mantenimientoProgramadoBR.Consultar(dctx, new MantenimientoProgramadoUnidadBO(){MantenimientoProgramadoID = cita.MantenimientoProgramado.MantenimientoProgramadoID}).FirstOrDefault();
                (manttoProgramado as MantenimientoProgramadoUnidadBO).Unidad = unidadBR.Consultar(dctx, new Equipos.BO.UnidadBO() { EquipoID = (manttoProgramado as MantenimientoProgramadoUnidadBO).EquipoID }).FirstOrDefault();
                var equiposAliados = unidadBR.ConsultarEquipoAliado(dctx, (manttoProgramado as MantenimientoProgramadoUnidadBO).Unidad, false);
                List<MantenimientoProgramadoEquipoAliadoBO> manttoEquiposAliados = new List<MantenimientoProgramadoEquipoAliadoBO>();
                foreach (var equipoAliado in equiposAliados)
                {
                    var manttoEA = mantenimientoProgramadoBR.Consultar(dctx, new MantenimientoProgramadoEquipoAliadoBO() { EquipoID = equipoAliado.EquipoID, Activo = true, EstatusMantenimientoProgramado = EEstatusMantenimientoProgramado.PROGRAMADO }).FirstOrDefault();
                    if (manttoEA != null)
                        manttoEquiposAliados.Add(manttoEA as MantenimientoProgramadoEquipoAliadoBO);
                }

                var tiempoMantto = this.ObtenerTiempoMantenimiento(manttoProgramado as MantenimientoProgramadoUnidadBO, manttoEquiposAliados);

                tiempoConsumido = tiempoConsumido + tiempoMantto != null ? tiempoMantto.Value : 0;
            }

            BPMO.SDNI.Equipos.BO.UnidadBO unidad;

            if (citas.Any(x => (x.MantenimientoProgramado as MantenimientoProgramadoUnidadBO).Unidad.UnidadID == this.vista.UnidadID))
            {
                var cita = citas.FirstOrDefault(x => (x.MantenimientoProgramado as MantenimientoProgramadoUnidadBO).Unidad.UnidadID == this.vista.UnidadID);
                unidad = (cita.MantenimientoProgramado as MantenimientoProgramadoUnidadBO).Unidad;
            }
            else
                unidad = unidadBR.Consultar(dctx, new BPMO.SDNI.Equipos.BO.UnidadBO() { UnidadID = this.vista.UnidadID }).FirstOrDefault();

            decimal? tiempo = 0;
            var configuracionPosicionTrabajo = new ConfiguracionPosicionTrabajoBO()
            {
                ConfiguracionModeloMotorizacion = new ConfiguracionModeloMotorizacionBO()
                {
                    Modelo = unidad.Modelo
                },
                DescriptorTrabajo = new DescriptorTrabajoBO
                {
                    Nombre = tipoMantto.ToString()
                }
            };

            var configPosicion = FacadeBR.ConsultarConfiguracionPosicionTrabajoDetalle(dctx, configuracionPosicionTrabajo);
            configPosicion.ForEach(x => { tiempo = tiempo + x.TiempoSRT; });

            return tiempo + tiempoConsumido > tiempoPorDia;
        }
        /// <summary>
        /// Valida que el dia que se seleccion no sea una fecha anterior a la seleccionada por el mantto programado
        /// </summary>
        /// <returns>Retorna TRUE si el dia es anterior al programado</returns>
        public bool ValidarDiaAnteriorManttoProgramado()
        {
            var manttoProgramadoBR = new ConsultarMantenimientoProgramadoBR();
            var manttoProgramado = manttoProgramadoBR.Consultar(dctx, new MantenimientoProgramadoBO() { MantenimientoProgramadoID = this.vista.MantenimientoProgramadoID }).FirstOrDefault();

            if (manttoProgramado == null)
            {
                this.vista.MostrarMensaje("NO SE ENCONTRÓ EL ÚLTIMO MANTENIMIENTO PROGRAMADO.", ETipoMensajeIU.INFORMACION);
                return false;
            }

            var fechaManttoProgramado = new DateTime(manttoProgramado.Fecha.Value.Year, manttoProgramado.Fecha.Value.Month, manttoProgramado.Fecha.Value.Day);
            var fechaManttoSeleccionado = new DateTime(this.vista.FechaCita.Value.Year, this.vista.FechaCita.Value.Month, this.vista.FechaCita.Value.Day);

            return fechaManttoSeleccionado < fechaManttoProgramado;
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
                            this.ActualizarContactosCliente();
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
