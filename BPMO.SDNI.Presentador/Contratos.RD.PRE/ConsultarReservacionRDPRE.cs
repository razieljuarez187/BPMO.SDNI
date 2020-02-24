// Satisface al CU004 - Catálogo de Reservaciones de Unidades de Renta Diaria
using System;
using System.Collections.Generic;
using BPMO.Facade.SDNI.BOF;
using BPMO.Facade.SDNI.BR;

using BPMO.Basicos.BO;
using BPMO.Patterns.Creational.DataContext;
using BPMO.Primitivos.Enumeradores;
using BPMO.SDNI.Contratos.RD.BOF;
using BPMO.Servicio.Catalogos.BO;

using BPMO.SDNI.Comun.BO;
using BPMO.SDNI.Comun.BOF;
using BPMO.SDNI.Contratos.RD.BO;
using BPMO.SDNI.Contratos.RD.BR;
using BPMO.SDNI.Contratos.RD.VIS;

namespace BPMO.SDNI.Contratos.RD.PRE
{
    public class ConsultarReservacionRDPRE
    {
        #region Atributos
        private IDataContext dctx;
        private string nombreClase = "ConsultarReservacionRDPRE";
        private IConsultarReservacionRDVIS vista;
        private ReservacionRDBR controlador;
        #endregion

        #region Constructores
        public ConsultarReservacionRDPRE(IConsultarReservacionRDVIS view)
        {
            try
            {
                this.vista = view;
                dctx = FacadeBR.ObtenerConexion();

                this.controlador = new ReservacionRDBR();
            }
            catch (Exception ex)
            {
                throw new Exception(nombreClase + ".ConsultarReservacionRDPRE: " + ex.Message);

            }
        }
        #endregion

        #region Métodos
        public void PrepararBusqueda()
        {
            this.vista.LimpiarSesion();

            this.vista.CuentaClienteID = null;
            this.vista.CuentaClienteNombre = null;
            this.vista.FechaReservacionFinal = null;
            this.vista.FechaReservacionInicial = null;
            this.vista.HoraReservacionFinal = null;
            this.vista.HoraReservacionInicial = null;
            this.vista.ModeloID = null;
            this.vista.ModeloNombre = null;
            this.vista.Numero = null;
            this.vista.NumeroEconomico = null;
            this.vista.UsuarioReservoID = null;
            this.vista.SucursalID = null;
            this.vista.SucursalNombre = string.Empty;

            this.EstablecerFiltros();

            this.EstablecerSeguridad();

            this.CargarSucursalesAutorizadas();//SC051
        }

        /// <summary>
        /// Recupera las sucursales a las que el usuario autenticado tiene permiso e acceder 
        /// </summary>
        private void CargarSucursalesAutorizadas()
        {
            if (vista.SucursalID != null)
                return;

            if (this.vista.SucursalesAutorizadas != null)
                if (this.vista.SucursalesAutorizadas.Count > 0)
                    return;

            var lstSucursales = Facade.SDNI.BR.FacadeBR.ConsultarSucursalesSeguridad(this.dctx,
                                                                                        new SeguridadBO(Guid.Empty,
                                                                                                        new UsuarioBO{Id = this.vista.UsuarioID}, 
                                                                                                        new AdscripcionBO
                                                                                                        {
                                                                                                            UnidadOperativa = new UnidadOperativaBO { Id = this.vista.UnidadOperativaID }
                                                                                                        }));
            this.vista.SucursalesAutorizadas = lstSucursales.ConvertAll(x => (object)x); //SC00
        }

        private void EstablecerFiltros()
        {
            try
            {
                Dictionary<string, object> paquete = this.vista.ObtenerPaqueteNavegacion("FiltrosReservacion") as Dictionary<string, object>;
                if (paquete != null)
                {
                    if (paquete.ContainsKey("ObjetoFiltro"))
                    {
                        if (paquete["ObjetoFiltro"].GetType() == typeof(ReservacionRDBOF))//SC051
                            this.DatoAInterfazUsuario(paquete["ObjetoFiltro"]);
                        else
                            throw new Exception("Se esperaba un objeto ReservacionRDBO, el objeto proporcionado no cumple con esta característica, intente de nuevo por favor.");
                    }
                    if (paquete.ContainsKey("Bandera"))
                    {
                        if ((bool)paquete["Bandera"])
                            this.ConsultarReservaciones();
                    }
                }
                this.vista.LimpiarPaqueteNavegacion("FiltrosReservacion");
            }
            catch (Exception ex)
            {
                throw new Exception(this.nombreClase + ".EstablecerFiltros: " + ex.Message);
            }
        }

        private void EstablecerSeguridad()
        {
            try
            {
                //se valida que los datos del usuario y la unidad operativa no sean nulos
                if (this.vista.UsuarioID == null) throw new Exception("El identificador del usuario no debe ser nulo");
                if (this.vista.UnidadOperativaID == null) throw new Exception("El identificador de la Unidad Operativa no debe ser nulo ");

                UsuarioBO usr = new UsuarioBO { Id = this.vista.UsuarioID };
                AdscripcionBO adscripcion = new AdscripcionBO { UnidadOperativa = new UnidadOperativaBO { Id = this.vista.UnidadOperativaID } };
                SeguridadBO seguridadBO = new SeguridadBO(Guid.Empty, usr, adscripcion);

                //Se obtienen las acciones a las cuales el usuario tiene permiso en este proceso
                List<CatalogoBaseBO> lst = FacadeBR.ConsultarAccion(dctx, seguridadBO);

                //Se valida si el usuario tiene permiso para consultar
                if (!this.ExisteAccion(lst, "CONSULTAR"))
                    this.vista.RedirigirSinPermisoAcceso();
                //Se valida si el usuario tiene permiso para registrar
                if (!this.ExisteAccion(lst, "INSERTARCOMPLETO"))
                    this.vista.PermitirRegistrar(false);
            }
            catch (Exception ex)
            {
                throw new Exception(nombreClase + ".EstablecerSeguridad:" + ex.Message);
            }
        }
        private bool ExisteAccion(List<CatalogoBaseBO> acciones, string accion)
        {
            if (acciones != null && acciones.Count > 0 && acciones.Exists(p => p != null && p.Nombre != null && p.Nombre.Trim().ToUpper() == accion.Trim().ToUpper()))
                return true;

            return false;
        }

        private Object InterfazUsuarioADato()
        {
            ReservacionRDBOF bo = new ReservacionRDBOF();
            bo.Modelo = new ModeloBO();
            bo.Unidad = new Equipos.BO.UnidadBO();
            bo.UnidadOperativa = new UnidadOperativaBO();
            bo.UsuarioReservo = new UsuarioBO();
            bo.Cliente = new CuentaClienteIdealeaseBO();
            bo.Sucursal = new SucursalBO();

            bo.Modelo.Id = this.vista.ModeloID;
            bo.UnidadOperativa.Id = this.vista.UnidadOperativaID;
            bo.UsuarioReservo.Id = this.vista.UsuarioReservoID;
            bo.Cliente.Id = this.vista.CuentaClienteID;
            bo.Activo = this.vista.Activo;

            if (!string.IsNullOrEmpty(this.vista.ModeloNombre) && !string.IsNullOrWhiteSpace(this.vista.ModeloNombre))
                bo.Modelo.Nombre = this.vista.ModeloNombre;
            if (!string.IsNullOrEmpty(this.vista.UsuarioReservoNombre) && !string.IsNullOrWhiteSpace(this.vista.UsuarioReservoNombre))
                bo.UsuarioReservo.Nombre = this.vista.UsuarioReservoNombre;
            if (!string.IsNullOrEmpty(this.vista.CuentaClienteNombre) && !string.IsNullOrWhiteSpace(this.vista.CuentaClienteNombre))
                bo.Cliente.Nombre = this.vista.CuentaClienteNombre;
            if (!string.IsNullOrEmpty(this.vista.NumeroEconomico) && !string.IsNullOrWhiteSpace(this.vista.NumeroEconomico))
                bo.Unidad.NumeroEconomico = this.vista.NumeroEconomico;
            if (!string.IsNullOrEmpty(this.vista.Numero) && !string.IsNullOrWhiteSpace(this.vista.Numero))
                bo.Numero = this.vista.Numero;

            bo.FechaInicial = this.CalcularFechaCompleta(this.vista.FechaReservacionInicial, this.vista.HoraReservacionInicial);
            bo.FechaFinal = this.CalcularFechaCompleta(this.vista.FechaReservacionFinal, this.vista.HoraReservacionFinal);

            if (this.vista.SucursalID.HasValue)
            {
                bo.Sucursal.Id = this.vista.SucursalID;
                bo.Sucursal.Nombre = this.vista.SucursalNombre;
            }
            else
            {
                bo.Sucursales = this.vista.SucursalesAutorizadas.ConvertAll(x => (SucursalBO) x);
            }

            return bo;
        }
        private void DatoAInterfazUsuario(Object obj)
        {
            ReservacionRDBO bo = (ReservacionRDBO)obj;

            if (!String.IsNullOrEmpty(bo.Numero) && !String.IsNullOrWhiteSpace(bo.Numero))
                this.vista.Numero = bo.Numero;
            else
                this.vista.Numero = null;

            if (bo.Cliente != null && !String.IsNullOrEmpty(bo.Cliente.Nombre) && !String.IsNullOrWhiteSpace(bo.Cliente.Nombre))
                this.vista.CuentaClienteNombre = bo.Cliente.Nombre;
            else
                this.vista.CuentaClienteNombre = null;

            if (bo.Cliente != null && bo.Cliente.Id != null)
                this.vista.CuentaClienteID = bo.Cliente.Id;
            else
                this.vista.CuentaClienteID = null;

            if (bo.Modelo != null && !String.IsNullOrEmpty(bo.Modelo.Nombre) && !String.IsNullOrWhiteSpace(bo.Modelo.Nombre))
                this.vista.ModeloNombre = bo.Modelo.Nombre;
            else
                this.vista.ModeloNombre = null;

            if (bo.Modelo != null && bo.Modelo.Id != null)
                this.vista.ModeloID = bo.Modelo.Id;
            else
                this.vista.ModeloID = null;

            if (bo.Unidad != null && !String.IsNullOrEmpty(bo.Unidad.NumeroEconomico) && !String.IsNullOrWhiteSpace(bo.Unidad.NumeroEconomico))
                this.vista.NumeroEconomico = bo.Unidad.NumeroEconomico;
            else
                this.vista.NumeroEconomico = null;

            if (bo.UsuarioReservo != null && bo.UsuarioReservo.Id != null)
                this.vista.UsuarioReservoID = bo.UsuarioReservo.Id;
            else
                this.vista.UsuarioReservoID = null;

            if (bo.UsuarioReservo != null && !String.IsNullOrEmpty(bo.UsuarioReservo.Nombre) && !String.IsNullOrWhiteSpace(bo.UsuarioReservo.Nombre))
                this.vista.UsuarioReservoNombre = bo.UsuarioReservo.Nombre;
            else
                this.vista.UsuarioReservoNombre = null;

            if (bo.FechaInicial != null)
            {
                this.vista.FechaReservacionInicial = bo.FechaInicial.Value.Date;
                this.vista.HoraReservacionInicial = bo.FechaInicial.Value.TimeOfDay;
            }
            else
            {
                this.vista.FechaReservacionInicial = null;
                this.vista.HoraReservacionInicial = null;
            }

            if (bo.FechaFinal != null)
            {
                this.vista.FechaReservacionFinal = bo.FechaFinal.Value.Date;
                this.vista.HoraReservacionFinal = bo.FechaFinal.Value.TimeOfDay;
            }
            else
            {
                this.vista.FechaReservacionFinal = null;
                this.vista.HoraReservacionFinal = null;
            }

            if (bo.Activo != null)
                this.vista.Activo = bo.Activo;
            else
                this.vista.Activo = null;

            this.vista.SucursalID = bo.Sucursal != null ? (bo.Sucursal.Id.HasValue ? bo.Sucursal.Id : null) : null;//SC051
            this.vista.SucursalNombre = bo.Sucursal != null ? (!string.IsNullOrEmpty(bo.Sucursal.Nombre) ? bo.Sucursal.Nombre : string.Empty) : string.Empty;//SC051
        }

        public void ConsultarReservaciones()
        {
            try
            {
                string s;
                if ((s = this.ValidarCampos()) != null)
                {
                    this.vista.MostrarMensaje(s, ETipoMensajeIU.ADVERTENCIA, null);
                    return;
                }

                ReservacionRDBOF bo = (ReservacionRDBOF)this.InterfazUsuarioADato();
                List<ReservacionRDBO> lst = this.controlador.Consultar(this.dctx, bo, true);

                if (lst.Count <= 0)
                    this.vista.MostrarMensaje("No se han encontrado reservaciones con los filtros proporcionados", ETipoMensajeIU.INFORMACION);

                this.vista.EstablecerResultado(lst);
            }
            catch (Exception ex)
            {
                throw new Exception(this.nombreClase + ".ConsultarReservaciones: " + ex.Message);
            }
        }

        private string ValidarCampos()
        {
            string s = "";
            
            if (this.vista.UnidadOperativaID == null)
                s += "UnidadOperativaID, ";

            if (s != null && s.Trim().CompareTo("") != 0)
                return "Los siguientes campos no pueden estar vacíos: \n" + s.Substring(0, s.Length - 2);

            DateTime? fechaInicial = this.CalcularFechaCompleta(this.vista.FechaReservacionInicial, this.vista.HoraReservacionInicial);
            DateTime? fechaFinal = this.CalcularFechaCompleta(this.vista.FechaReservacionFinal, this.vista.HoraReservacionFinal);

            if (fechaInicial != null && fechaFinal != null && fechaFinal < fechaInicial)
                return "La fecha y hora inicial no puede ser mayor que la fecha y hora final.";

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

        public void IrADetalle(int? reservacionID)
        {
            try
            {
                if (reservacionID == null)
                    throw new Exception("No se encontró el registro seleccionado.");

                ReservacionRDBO bo = new ReservacionRDBO { ReservacionID = reservacionID };

                this.vista.LimpiarSesion();
                
                Dictionary<string, object> paquete = new Dictionary<string, object>();
                paquete.Add("ObjetoFiltro", this.InterfazUsuarioADato());
                paquete.Add("Bandera", true);

                this.vista.EstablecerPaqueteNavegacion("FiltrosReservacion", paquete);
                this.vista.EstablecerPaqueteNavegacion("ReservacionRDBO", bo);

                this.vista.RedirigirADetalle();
            }
            catch (Exception ex)
            {
                throw new Exception(this.nombreClase + ".IrADetalle: " + ex.Message);
            }
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
                case "Empleado":
                    EmpleadoBO empleado = new EmpleadoBO();
                    empleado.NombreCompleto = this.vista.UsuarioReservoNombre;
                    empleado.Activo = true;

                    obj = empleado;
                    break;
                case "Sucursal"://SC051
                    SucursalBOF sucursal = new SucursalBOF();
                    sucursal.UnidadOperativa = new UnidadOperativaBO { Activo = true, Id = this.vista.UnidadOperativaID };
                    sucursal.Nombre = this.vista.SucursalNombre;
                    sucursal.Usuario = new UsuarioBO { Activo = true, Id = this.vista.UsuarioID };//TODO:Revisar
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
                    break;
                case "CuentaClienteIdealease":
                    CuentaClienteIdealeaseBOF cliente = (CuentaClienteIdealeaseBOF)selecto ?? new CuentaClienteIdealeaseBOF();

                    if (cliente.Cliente == null)
                        cliente.Cliente = new ClienteBO();

                    this.vista.CuentaClienteID = cliente.Id ?? null;
                    this.vista.CuentaClienteNombre = !string.IsNullOrEmpty(cliente.Nombre) ? cliente.Nombre : string.Empty;
                    break;
                case "Empleado":
                    EmpleadoBO empleado = (EmpleadoBO)selecto;

                    if (empleado != null && empleado.Numero != null)
                        this.vista.UsuarioReservoID = empleado.Numero;
                    else
                        this.vista.UsuarioReservoID = null;

                    if (empleado != null && empleado.NombreCompleto != null)
                        this.vista.UsuarioReservoNombre = empleado.NombreCompleto;
                    else
                        this.vista.UsuarioReservoNombre = null;
                    break;
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
                    break; 
            }
        }
        #endregion
        #endregion
    }
}