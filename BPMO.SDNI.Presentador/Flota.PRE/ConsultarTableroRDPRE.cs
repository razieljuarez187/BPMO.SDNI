//Satisface CU009 – Consultar Tablero de Seguimiento Unidades Renta Diaria
using System;
using System.Collections.Generic;
using BPMO.Basicos.BO;
using BPMO.Facade.SDNI.BOF;
using BPMO.Facade.SDNI.BR;
using BPMO.Patterns.Creational.DataContext;
using BPMO.Primitivos.Enumeradores;
using BPMO.Servicio.Catalogos.BO;
using BPMO.SDNI.Comun.BO;
using BPMO.SDNI.Flota.BOF;
using BPMO.SDNI.Flota.BR;
using BPMO.SDNI.Flota.VIS;
using BPMO.SDNI.Comun.BOF;

namespace BPMO.SDNI.Flota.PRE{
    
    public class ConsultarTableroRDPRE{
        
        #region Atributos
        private IConsultarTableroRDVIS vista;
        private IDataContext dctx = null;
        private SeguimientoFlotaBR controlador;

        private string nombreClase = "ConsultarTableroRDPRE";
        #endregion

        #region Constructores
        public ConsultarTableroRDPRE(IConsultarTableroRDVIS view){
            try{
                this.vista = view;

                this.controlador = new SeguimientoFlotaBR();
                this.dctx = FacadeBR.ObtenerConexion();
            } catch (Exception ex){
                this.vista.MostrarMensaje("No se pudieron obtener los datos de conexión", ETipoMensajeIU.ERROR,
                        "No se encontraron los parámetros de conexión en la fuente de datos, póngase en contacto con el administrador del sistema." + ex.Message);
            }
        }
        #endregion

        #region Metodos
        public void PrepararBusqueda(){
            this.vista.LimpiarSesion();
            this.vista.PrepararBusqueda();
			
            this.EstablecerSeguridad(); 
        }

		private void EstablecerSeguridad(){
            try{
                //se valida que los datos del usuario y la unidad operativa no sean nulos
                if (this.vista.UsuarioAutenticado == null) throw new Exception("El identificador del usuario no debe ser nulo");
                if (this.vista.UnidadOperativaID == null) throw new Exception("El identificador de la Unidad Operativa no debe ser nulo ");

                //Se crea el objeto de seguridad
                UsuarioBO usuario = new UsuarioBO() { Id = this.vista.UsuarioAutenticado };
                AdscripcionBO adscripcion = new AdscripcionBO() { UnidadOperativa = new UnidadOperativaBO() { Id = this.vista.UnidadOperativaID } };
                SeguridadBO seguridadBO = new SeguridadBO(Guid.Empty, usuario, adscripcion);

                //Se obtienen las acciones a las cuales el usuario tiene permiso en este proceso
                List<CatalogoBaseBO> lst = FacadeBR.ConsultarAccion(this.dctx, seguridadBO);

                //Se valida si el usuario tiene permiso para consultar
                if (!this.ExisteAccion(lst, "CONSULTAR"))
                    this.vista.RedirigirSinPermisoAcceso();
                if (!this.ExisteAccion(lst, "UI RESERVARUNIDAD"))
                    this.vista.DesactivarReservaciones();
                if (!this.ExisteAccion(lst, "UI CONSULTARUNIDAD"))
                    this.vista.ActivarDetallesUnidad = false;
                if (!this.ExisteAccion(lst, "UI CONSULTARCONTRATO"))
                    this.vista.ActivarDetallesContrato = false;
            } catch (Exception ex){
                throw new Exception(this.nombreClase + ".EstablecerSeguridad:" + ex.Message);
            }
		}

		private bool ExisteAccion(List<CatalogoBaseBO> acciones, string nombreAccion){
			if (acciones != null && acciones.Count > 0 && acciones.Exists(p => p != null && p.Nombre != null && p.Nombre.Trim().ToUpper() == nombreAccion.Trim().ToUpper()))
				return true;

			return false;
		}

        public void Consultar(){
            try {
                FlotaRDBOF bo = (FlotaRDBOF)this.InterfazUsuarioADato();

                List<FlotaRDBOF> unidadesRD = controlador.ConsultarTableroRentaDiaria(dctx, bo);

                this.vista.UnidadesRD = unidadesRD;
                this.vista.ActualizarResultado();

                if (unidadesRD.Count < 1)
                    this.vista.MostrarMensaje("La búsqueda no produjo resultados", ETipoMensajeIU.INFORMACION,
                        "No se encontraron coincidencias con los criterios ingresados. Verifique sus datos");
            } catch (Exception ex) {
                throw new Exception(this.nombreClase + ".Consultar: " + ex.Message); 
            }
        }

        private FlotaRDBOF InterfazUsuarioADato(){
            FlotaRDBOF bo = new FlotaRDBOF();
            bo.Unidad = new Equipos.BO.UnidadBO();
            bo.Unidad.Modelo = new ModeloBO();
            bo.Unidad.Modelo.Marca = new MarcaBO();
            bo.Unidad.Sucursal = new Basicos.BO.SucursalBO();
            bo.Unidad.Sucursal.UnidadOperativa = new UnidadOperativaBO();
            bo.Cliente = new CuentaClienteIdealeaseBO();
            bo.Cliente.UnidadOperativa = new UnidadOperativaBO();
            bo.Unidad.NumeroEconomico = this.vista.NumeroEconomico;
            if (vista.MarcaID != null){
                bo.Unidad.Modelo.Marca.Id = this.vista.MarcaID;
                bo.Unidad.Modelo.Marca.Nombre = this.vista.MarcaNombre;
            }
            if (vista.ModeloID != null){
                bo.Unidad.Modelo.Id = this.vista.ModeloID;
                bo.Unidad.Modelo.Nombre = this.vista.ModeloNombre;
            }
            if (vista.SucursalID != null){
                bo.Unidad.Sucursal.Id = this.vista.SucursalID;
                bo.Unidad.Sucursal.Nombre = this.vista.SucursalNombre;
            } 
            else
                bo.Sucursales = FacadeBR.ConsultarSucursalesSeguridad(this.dctx, new SeguridadBO(Guid.Empty, new UsuarioBO() { Id = this.vista.UsuarioAutenticado }, new AdscripcionBO { UnidadOperativa = new UnidadOperativaBO() { Id = this.vista.UnidadOperativaID } }));
            if (vista.CuentaClienteID != null){
                bo.Cliente.Id = this.vista.CuentaClienteID;
                bo.Cliente.Nombre = this.vista.CuentaClienteNombre;
            }
            

            if (this.vista.EstatusUnidad.HasValue)
                bo.Estatus = this.vista.EstatusUnidad; 
                
            bo.EstaEnTaller = this.vista.EstaEnTaller;
            bo.EstaReservada = this.vista.EstaReservada;
            bo.NumeroContrato = this.vista.NumeroContrato;
            bo.FechaContratoInicial = this.vista.FechaContratoInicial;
            bo.FechaContratoFinal = this.vista.FechaContratoFinal;

            return bo;
        }

        public void CambiarPaginaResultado(int p){
            this.vista.IndicePaginaResultado = p;
            this.vista.ActualizarResultado();
        }

        public void VerDetallesUnidad(int? unidadID) {
            if (unidadID.HasValue) {
                this.vista.EstablecerPaqueteNavegacion("DetalleFlotaUI", unidadID);
                this.vista.RedirigirADetallesUnidad();
            } else
            {
                this.vista.MostrarMensaje("No se ha proporcionado el identificador de la unidad que se quiere consultar a detalle.", ETipoMensajeIU.ADVERTENCIA);
            }
        }

        public void VerDetallesContrato(int? unidadID) {
            if (unidadID.HasValue) {
                FlotaRDBOF elementoBO = vista.UnidadesRD.Find(p => p.Unidad.UnidadID.HasValue && p.Unidad.UnidadID.Value == unidadID);
                if (elementoBO.ContratoID != null){
                this.vista.EstablecerPaqueteNavegacion("ContratoRDBO", unidadID);
                this.vista.RedirigirADetallesContrato();
                }
                else
                    this.vista.MostrarMensaje("La unidad seleccionada no cuenta con ningún contrato.", ETipoMensajeIU.ADVERTENCIA);
            } else {
                this.vista.MostrarMensaje("No se ha proporcionado el identificador de la unidad que se quiere consultar el detalle de su contrato.", ETipoMensajeIU.ADVERTENCIA);
            }
        }

        #region Métodos para el Buscador
        public object PrepararBOBuscador(string catalogo){
            object obj = null;

            switch (catalogo){
                case "Marca":
                    MarcaBO marca = new MarcaBO();
                    marca.Nombre = this.vista.MarcaNombre;
                    marca.Activo = true;
                    obj = marca;
                    break;
                
                case "Modelo":
                    ModeloBO modelo = new ModeloBO();
                    modelo.Auditoria = new AuditoriaBO();
                    modelo.Marca = new MarcaBO();
                    modelo.Marca.Id = this.vista.MarcaID;
                    modelo.Nombre = this.vista.ModeloNombre;
                    modelo.Activo = true;
                    obj = modelo;
                    break;

                case "Sucursal":
                    SucursalBOF sucursal = new SucursalBOF();
                    sucursal.UnidadOperativa = new UnidadOperativaBO();
                    sucursal.UnidadOperativa.Id = this.vista.UnidadOperativaID;
                    sucursal.Nombre = this.vista.SucursalNombre;
                    sucursal.Usuario = new UsuarioBO() { Id = this.vista.UsuarioAutenticado };
                    obj = sucursal;
                    break;

                case "CuentaClienteIdealease":
                    CuentaClienteIdealeaseBOF cliente = new CuentaClienteIdealeaseBOF();
                    cliente.Nombre = vista.CuentaClienteNombre;
                    cliente.UnidadOperativa = new UnidadOperativaBO { Id = vista.UnidadOperativaID }; 
                    cliente.Cliente = new ClienteBO();
                    obj = cliente;
                    break;
            }
            return obj;
        }
        
        public void DesplegarResultadoBuscador(string catalogo, object selecto){
            switch (catalogo){             
                case "Marca":
                    MarcaBO marca = (MarcaBO)selecto;

                    if (marca != null && marca.Id.HasValue)
                    {
                        this.vista.MarcaID = marca.Id.Value;
                        this.vista.HabilitarModelo(true);
                    }
                    else
                    {
                        this.vista.MarcaID = null;
                        this.vista.HabilitarModelo(false);
                    }

                    if (marca != null && (!string.IsNullOrEmpty(marca.Nombre) && !string.IsNullOrWhiteSpace(marca.Nombre)))
                        this.vista.MarcaNombre = marca.Nombre;
                    else
                        this.vista.MarcaNombre = string.Empty;

                    this.vista.ModeloID = null;
                    this.vista.ModeloNombre = string.Empty;
                    break;

                case "Modelo":
                    ModeloBO modelo = (ModeloBO)selecto;

                    if (modelo != null && modelo.Id != null)
                        this.vista.ModeloID = modelo.Id;
                    else
                        this.vista.ModeloID = null;

                    if (modelo != null && modelo.Nombre != null)
                        this.vista.ModeloNombre = modelo.Nombre;
                    else
                        this.vista.ModeloNombre = null;
                    break;

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
                    break;

                case "CuentaClienteIdealease":
                      CuentaClienteIdealeaseBOF cliente = (CuentaClienteIdealeaseBOF)selecto ?? new CuentaClienteIdealeaseBOF();

                    if (cliente.Cliente == null)
                        cliente.Cliente = new ClienteBO();

                    vista.CuentaClienteID = cliente.Id;

                    vista.CuentaClienteNombre = !string.IsNullOrEmpty(cliente.Nombre) ? cliente.Nombre : string.Empty;
                    break;
            }
        }
        #endregion
        #endregion
    }
}
