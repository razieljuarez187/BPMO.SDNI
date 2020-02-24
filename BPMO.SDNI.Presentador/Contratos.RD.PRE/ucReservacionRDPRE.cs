// Satisface al CU004 - Catálogo de Reservaciones de Unidades de Renta Diaria
using System;
using System.Collections.Generic;
using System.Linq;
using BPMO.Basicos.BO;
using BPMO.Facade.SDNI.BOF;
using BPMO.Facade.SDNI.BR;
using BPMO.Patterns.Creational.DataContext;
using BPMO.Primitivos.Enumeradores;
using BPMO.SDNI.Comun.BOF;
using BPMO.SDNI.Contratos.RD.BO;
using BPMO.SDNI.Contratos.RD.BOF;
using BPMO.SDNI.Contratos.RD.BR;
using BPMO.SDNI.Contratos.RD.VIS;
using BPMO.SDNI.Equipos.BO;
using BPMO.SDNI.Tramites.BO;
using BPMO.Servicio.Catalogos.BO;

namespace BPMO.SDNI.Contratos.RD.PRE
{
    public class ucReservacionRDPRE
    {
        #region Atributos
        private IDataContext dctx = null;
        private ReservacionRDBR controlador;
        private IucReservacionRDVIS vista;

        private string nombreClase = "ucReservacionRDPRE";
        #endregion

        #region Constructores
        public ucReservacionRDPRE(IucReservacionRDVIS view)
        {
            try
            {
                this.vista = view;

                this.controlador = new ReservacionRDBR();
                this.dctx = FacadeBR.ObtenerConexion();
            }
            catch (Exception ex)
            {
                this.vista.MostrarMensaje("Inconsistencia en los parámetros de configuración", ETipoMensajeIU.ERROR, this.nombreClase + ".ucReservacionRDPRE:" + ex.Message);
            }
        }
        #endregion

        #region Métodos

        public void PrepararNuevo()
        {
            this.vista.PrepararNuevo();

            this.vista.HabilitarModoEdicion(true);
            this.vista.HabilitarCuentaCliente(true);
            this.vista.HabilitarModelo(true);
            this.vista.HabilitarUnidad(true);
            this.vista.HabilitarSucursal(true);//SC_0051

            this.vista.MostrarDetalleUnidad(false);
            this.vista.MostrarNumeroReservacion(false);
            this.vista.MostrarUsuarioReservo(false);
            this.vista.MostrarActivo(false);
        }

        public void PrepararEdicion()
        {
            this.vista.HabilitarModoEdicion(true);
            this.vista.HabilitarCuentaCliente(true);
            this.vista.HabilitarModelo(true);
            this.vista.HabilitarUnidad(true);
            this.vista.HabilitarSucursal(true);//SC_0051

            this.vista.MostrarDetalleUnidad(false);
            this.vista.MostrarNumeroReservacion(true);
            this.vista.MostrarUsuarioReservo(true);
            this.vista.MostrarActivo(false);
        }
        public void PrepararVisualizacion()
        {
            this.vista.HabilitarModoEdicion(false);
            this.vista.HabilitarCuentaCliente(false);
            this.vista.HabilitarModelo(false);
            this.vista.HabilitarUnidad(false);
            this.vista.HabilitarSucursal(false);//SC_0051

            this.vista.MostrarDetalleUnidad(false);
            this.vista.MostrarNumeroReservacion(false);
            this.vista.MostrarUsuarioReservo(true);
            this.vista.MostrarActivo(true);
        }
        
        public void EstablecerConfiguracionInicial(int? unidadOperativaID)
        {
            this.vista.UnidadOperativaID = unidadOperativaID;
        }

        public string ValidarCamposUnidadDisponibleReservacion()
        {
            string s = "";

            if (this.vista.UnidadOperativaID == null)
                s += "Unidad Operativa, ";
            if (this.vista.ModeloID == null)
                s += "Modelo, ";
            if (this.vista.FechaReservacionInicial == null)
                s += "Fecha Inicial, ";
            if (this.vista.FechaReservacionFinal == null)
                s += "Fecha Final, ";
            if (this.vista.HoraReservacionInicial == null)
                s += "Hora Inicial, ";
            if (this.vista.HoraReservacionFinal == null)
                s += "Hora Final, ";
            if (!this.vista.SucursalID.HasValue)
                s += "Sucursal, ";

            if (s != null && s.Trim().CompareTo("") != 0)
                return "Los siguientes campos no pueden estar vacíos: \n" + s.Substring(0, s.Length - 2);

            if ((s = this.ValidarFechasReservacion()) != null)
                return s;

            return null;
        }
        private string ValidarFechasReservacion()
        {
            DateTime? fechaInicial = this.CalcularFechaCompleta(this.vista.FechaReservacionInicial, this.vista.HoraReservacionInicial);
            DateTime? fechaFinal = this.CalcularFechaCompleta(this.vista.FechaReservacionFinal, this.vista.HoraReservacionFinal);

            if (fechaInicial != null && fechaFinal != null && fechaFinal.Value.Date < fechaInicial.Value.Date)
                return "La fecha inicial no puede ser mayor que la fecha final.";
            if (fechaInicial != null && fechaFinal != null && this.vista.HoraReservacionInicial != null && this.vista.HoraReservacionFinal != null && fechaFinal < fechaInicial)
                return "La fecha y hora inicial no puede ser mayor que la fecha y hora final.";

            if (fechaFinal != null && fechaFinal < DateTime.Today)
                return "La reservación no puede terminar antes de la fecha actual.";
            if (fechaFinal != null && this.vista.HoraReservacionFinal != null && fechaFinal < DateTime.Now)
                return "La reservación no puede terminar antes de la fecha y hora actual";

            if (fechaInicial != null && fechaInicial < DateTime.Today)
                return "La reservación no puede comenzar antes de la fecha actual.";
            if (fechaInicial != null && this.vista.HoraReservacionInicial != null && fechaInicial < DateTime.Now)
                return "La reservación no puede comenzar antes de la fecha y hora actual";

            return null;
        }


        private string ConsultarDisponible()
        {
            ReservacionRDBOF bof = new ReservacionRDBOF();
            bof.Modelo = new ModeloBO();
            bof.Unidad = new Equipos.BO.UnidadBO();
            bof.FechaInicial = new DateTime(this.vista.FechaReservacionInicial.Value.Year, this.vista.FechaReservacionInicial.Value.Month, this.vista.FechaReservacionInicial.Value.Day, this.vista.HoraReservacionInicial.Value.Hours, this.vista.HoraReservacionInicial.Value.Minutes, this.vista.HoraReservacionInicial.Value.Seconds); 
            bof.FechaFinal = new DateTime(this.vista.FechaReservacionFinal.Value.Year, this.vista.FechaReservacionFinal.Value.Month, this.vista.FechaReservacionFinal.Value.Day, this.vista.HoraReservacionFinal.Value.Hours, this.vista.HoraReservacionFinal.Value.Minutes, this.vista.HoraReservacionFinal.Value.Seconds); 
            bof.Tipo = (ETipoReservacion)Enum.Parse(typeof(ETipoReservacion), this.vista.TipoID.Value.ToString());
            bof.Modelo.Id = vista.ModeloID;
            bof.Unidad.UnidadID = vista.UnidadID;
            bof.UnidadOperativa = new UnidadOperativaBO() { Id = this.vista.UnidadOperativaID };
            bof.Sucursal = new SucursalBO {Id = this.vista.SucursalID};//SC_0051

            List<ReservacionRDBOF> Resultado = controlador.ConsultarDisponibles(dctx, bof);
            if (Resultado.Count == 0 && !this.vista.ReservacionID.HasValue)
                return "La unidad ya se encuentra reservada para ese período";
            else if(this.vista.ReservacionID.HasValue)
            {
                var lstReservaciones = this.controlador.Consultar(this.dctx, new ReservacionRDBO {ReservacionID = this.vista.ReservacionID});
                var reservacion = lstReservaciones.FirstOrDefault(x => x.ReservacionID == this.vista.ReservacionID);
                if (reservacion != null)
                {
                    if(reservacion.ReservacionID != this.vista.ReservacionID || reservacion.Unidad.UnidadID != this.vista.UnidadID ||
                        reservacion.FechaFinal.Value.Date != this.vista.FechaReservacionFinal.Value.Date ||
                        reservacion.FechaInicial.Value.Date != this.vista.FechaReservacionInicial.Value.Date ||
                        reservacion.FechaFinal.Value.TimeOfDay != this.vista.HoraReservacionFinal.Value ||
                        reservacion.FechaInicial.Value.TimeOfDay != this.vista.HoraReservacionInicial.Value)
                    {
                        if(reservacion.ReservacionID != this.vista.ReservacionID ||
                        reservacion.Unidad.UnidadID != this.vista.UnidadID ||
                        reservacion.FechaFinal.Value.Date <= this.vista.FechaReservacionFinal.Value.Date ||
                        reservacion.FechaInicial.Value.Date != this.vista.FechaReservacionInicial.Value.Date ||
                        reservacion.FechaFinal.Value.TimeOfDay <= this.vista.HoraReservacionFinal.Value ||
                        reservacion.FechaInicial.Value.TimeOfDay != this.vista.HoraReservacionInicial.Value)
                            reservacion = null;
                    }                    
                }

                if (reservacion == null && Resultado.Count == 0)
                    return "La unidad ya se encuentra reservada para ese período";
            }
            return null;
        }

        public string ValidarCampos()
        {
            string s = "";

            if (this.vista.UnidadOperativaID == null)
                s += "Unidad Operativa, ";
            if (this.vista.ModeloID == null)
                s += "Modelo, ";
            if (this.vista.FechaReservacionInicial == null)
                s += "Fecha Inicial, ";
            if (this.vista.FechaReservacionFinal == null)
                s += "Fecha Final, ";
            if (this.vista.HoraReservacionInicial == null)
                s += "Hora Inicial, ";
            if (this.vista.HoraReservacionFinal == null)
                s += "Hora Final, ";
            if (this.vista.CuentaClienteID == null)
                s += "Cliente, ";
            if (string.IsNullOrEmpty(this.vista.Observaciones) || string.IsNullOrWhiteSpace(this.vista.Observaciones))
                s += "Observaciones, ";
            if (this.vista.UsuarioReservoID == null)
                s += "Usuario, ";
            if (this.vista.TipoID == null)
                s += "Tipo, ";
            if (this.vista.SucursalID == null)
                s += "Sucursal, ";

            if (s != null && s.Trim().CompareTo("") != 0)
                return "Los siguientes campos no pueden estar vacíos: \n" + s.Substring(0, s.Length - 2);

            if ((s = this.ValidarFechasReservacion()) != null)
                return s;

            #region RI0075
            if ((s = this.ConsultarDisponible()) != null)
                return s;
            #endregion

            return null;
        }

        private DateTime? CalcularFechaCompleta(DateTime? fecha, TimeSpan? hora)
        {
            DateTime? fechaFinal = null;

            if (fecha != null)
            {
                if (hora != null)
                    fechaFinal = new DateTime(fecha.Value.Year, fecha.Value.Month, fecha.Value.Day, hora.Value.Hours, hora.Value.Minutes, hora.Value.Seconds);
                else
                    fechaFinal = fecha;
            }

            return fechaFinal;
        }

        public void SeleccionarUnidad(ReservacionRDBOF bof)
        {
            try
            {
                if (bof != null && bof.Unidad != null && bof.Unidad.UnidadID != null)
                {
                    bof.ConEquiposAliados = true;
                    bof.ConTramites = true;

                    List<ReservacionRDBOF> lst = this.controlador.ConsultarDisponibles(this.dctx, bof);
                    if (lst.Count <= 0)
                        throw new Exception("No se encontró la unidad seleccionada.");

                    bof = lst[0];
                }

                if (bof == null) bof = new ReservacionRDBOF();
                if (bof.Unidad == null) bof.Unidad = new Equipos.BO.UnidadBO();
                if (bof.Unidad.CaracteristicasUnidad == null) bof.Unidad.CaracteristicasUnidad = new CaracteristicasUnidadBO();
                if (bof.Unidad.Modelo == null) bof.Unidad.Modelo = new ModeloBO();
                if (bof.Unidad.Modelo.Marca == null) bof.Unidad.Modelo.Marca = new MarcaBO();

                this.vista.UnidadID = bof.Unidad.UnidadID;
                this.vista.UnidadAnio = bof.Unidad.Anio;
                this.vista.UnidadCapacidadCarga = bof.Unidad.CaracteristicasUnidad.PBVMaximoRecomendado;
                this.vista.UnidadCapacidadTanque = bof.Unidad.CaracteristicasUnidad.CapacidadTanque;
                this.vista.UnidadEquiposAliados = bof.Unidad.EquiposAliados;
                this.vista.UnidadEstatusMantenimiento = bof.EstatusMantenimiento;
                this.vista.UnidadEstatusOperacion = bof.EstatusOperacion;
                this.vista.UnidadFechaPlaneadaLiberacion = bof.FechaPlaneadaLiberacion;
                this.vista.UnidadMarcaNombre = bof.Unidad.Modelo.Marca.Nombre;
                this.vista.UnidadPlacaEstatal = bof.ObtenerResultadoTramitePorTipo(ETipoTramite.PLACA_ESTATAL);
                this.vista.UnidadPlacaFederal = bof.ObtenerResultadoTramitePorTipo(ETipoTramite.PLACA_FEDERAL);
                this.vista.UnidadRendimientoTanque = bof.Unidad.CaracteristicasUnidad.RendimientoTanque;
                this.vista.UnidadSerie = bof.Unidad.NumeroSerie;
                this.vista.NumeroEconomico = bof.Unidad.NumeroEconomico;

                this.vista.MostrarDetalleUnidad(bof.Unidad.UnidadID != null);
            }
            catch (Exception ex)
            {
                throw new Exception(this.nombreClase + ".SeleccionarUnidad: " + ex.Message);
            }
        }
        public void SeleccionarFecha()
        {
            this.SeleccionarUnidad(null);

            if (this.vista.UnidadID != null)
                this.vista.TipoID = (int)ETipoReservacion.UNIDAD;
            else
            {
                if (this.vista.ModeloID != null)
                    this.vista.TipoID = (int)ETipoReservacion.MODELO;
                else
                    this.vista.TipoID = null;
            }

            string s;
            if ((s = this.ValidarFechasReservacion()) != null)
            {
                this.vista.MostrarMensaje(s, ETipoMensajeIU.ADVERTENCIA, null);
                return;
            }
        }

        public void LimpiarSesion()
        {
            this.vista.LimpiarSesion();
        }

        #region Métodos para el Buscador
        public object PrepararBOBuscador(string catalogo)
        {
            object obj = null;

            switch (catalogo)
            {
                case "CuentaClienteIdealease":
                    var cliente = new CuentaClienteIdealeaseBOF { Cliente = new ClienteBO() };

                    cliente.Nombre = this.vista.CuentaClienteNombre;
                    cliente.UnidadOperativa = new UnidadOperativaBO();
                    cliente.UnidadOperativa.Id = this.vista.UnidadOperativaID;

                    obj = cliente;
                    break;
                case "Modelo":
                    ModeloBO modelo = new ModeloBO();
                    modelo.Auditoria = new AuditoriaBO();
                    modelo.Marca = new MarcaBO();
                    modelo.Nombre = this.vista.ModeloNombre;
                    modelo.Activo = true;

                    obj = modelo;
                    break;
                case "UnidadDisponibleReservacion":
                    ReservacionRDBOF reservacion = new ReservacionRDBOF();
                    reservacion.Unidad = new BPMO.SDNI.Equipos.BO.UnidadBO();
                    reservacion.Unidad.Sucursal = new SucursalBO();

                    reservacion.ConEquiposAliados = false;
                    reservacion.ConTramites = false;
                    reservacion.FechaInicial = this.CalcularFechaCompleta(this.vista.FechaReservacionInicial, this.vista.HoraReservacionInicial);
                    reservacion.FechaFinal = this.CalcularFechaCompleta(this.vista.FechaReservacionFinal, this.vista.HoraReservacionFinal);
                    reservacion.Modelo = new ModeloBO();
                    reservacion.Modelo.Id = this.vista.ModeloID;
                    reservacion.Modelo.Nombre = this.vista.ModeloNombre;
                    reservacion.UnidadOperativa = new UnidadOperativaBO();
                    reservacion.UnidadOperativa.Id = this.vista.UnidadOperativaID;

                    if (this.vista.SucursalID.HasValue)
                        reservacion.Unidad.Sucursal.Id = this.vista.SucursalID;
                    else if (this.vista.SucursalesSeguridad != null)
                        reservacion.Sucursales = this.vista.SucursalesSeguridad.ConvertAll(p => new SucursalBO() { Id = p });

                    obj = reservacion;
                    break;
                case "Sucursal":
                    SucursalBOF sucursal = new SucursalBOF();
                    sucursal.UnidadOperativa = new UnidadOperativaBO { Activo = true, Id = this.vista.UnidadOperativaID };
                    sucursal.Nombre = this.vista.SucursalNombre;
                    sucursal.Usuario = new UsuarioBO { Activo = true, Id = this.vista.UsuarioReservoID };//TODO:Revisar
                    obj = sucursal;
                    break;
            }

            return obj;
        }
        public void DesplegarResultadoBuscador(string catalogo, object selecto)
        {
            switch (catalogo)
            {
                case "Modelo":
                    ModeloBO modelo = (ModeloBO)selecto;

                    this.vista.ModeloID = modelo != null && modelo.Id != null ? modelo.Id : null;
                    this.vista.ModeloNombre = modelo != null && modelo.Nombre != null ? modelo.Nombre : null;

                    if (modelo != null && modelo.Id != null)
                        this.vista.TipoID = (int)ETipoReservacion.MODELO;
                    else
                        this.vista.TipoID = null;

                    this.SeleccionarUnidad(null);
                    break;
                case "CuentaClienteIdealease":
                    CuentaClienteIdealeaseBOF cliente = (CuentaClienteIdealeaseBOF)selecto ?? new CuentaClienteIdealeaseBOF();

                    if (cliente.Cliente == null)
                        cliente.Cliente = new ClienteBO();

                    this.vista.CuentaClienteID = cliente.Id ?? null;
                    this.vista.CuentaClienteNombre = !string.IsNullOrEmpty(cliente.Nombre) ? cliente.Nombre : string.Empty;
                    break;
                case "UnidadDisponibleReservacion":
                    this.SeleccionarUnidad((ReservacionRDBOF)selecto);

                    if (this.vista.UnidadID != null)
                        this.vista.TipoID = (int)ETipoReservacion.UNIDAD;
                    else
                    {
                        if (this.vista.ModeloID != null)
                            this.vista.TipoID = (int)ETipoReservacion.MODELO;
                        else
                            this.vista.TipoID = null;
                    }
                    break;
                case "Sucursal"://SC_0051
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
                    this.SeleccionarUnidad(null);
                    #endregion
                    break;  
            }
        }
        #endregion
        #endregion

        public string ValidarCamposModeloDisponibleReservacion()
        {
            string s = "";

            if (this.vista.UnidadOperativaID == null)
                s += "Unidad Operativa, ";
            if (this.vista.ModeloNombre == null)
                s += "Modelo, ";
            if (this.vista.FechaReservacionInicial == null)
                s += "Fecha Inicial, ";
            if (this.vista.FechaReservacionFinal == null)
                s += "Fecha Final, ";
            if (this.vista.HoraReservacionInicial == null)
                s += "Hora Inicial, ";
            if (this.vista.HoraReservacionFinal == null)
                s += "Hora Final, ";
            if (!this.vista.SucursalID.HasValue)
                s += "Sucursal, ";

            if (s != null && s.Trim().CompareTo("") != 0)
                return "Los siguientes campos no pueden estar vacíos: \n" + s.Substring(0, s.Length - 2);

            if ((s = this.ValidarFechasReservacion()) != null)
                return s;

            return null;
        }
    }
}