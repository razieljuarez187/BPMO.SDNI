//Satiface al caso de uso CU068 - Catáloglo de Clientes
using System;
using System.Collections.Generic;
using System.Linq;
using BPMO.Basicos.BO;
using BPMO.Facade.SDNI.BR;
using BPMO.Facade.SDNI.eFacturacion.BR;
using BPMO.Patterns.Creational.DataContext;
using BPMO.Primitivos.Enumeradores;
using BPMO.SDNI.Comun.BO;
using BPMO.SDNI.Comun.BR;
using BPMO.SDNI.Comun.VIS;

namespace BPMO.SDNI.Comun.PRE
{
	public class DetalleCuentaClientePRE
	{
		#region Atributos

		private CuentaClienteIdealeaseBR clienteBR;
		private IDataContext dctx;
		private string nombreClase = "DetalleClientePRE";
		private ucDatosRepresentanteLegalPRE presentadorRepresentante;
		private IDetalleCuentaClienteVIS vista;
		private IucDatosRepresentanteLegalVIS vistaRepresentante;
	    private SeguridadBO seguridad;

		#endregion Atributos

		#region Constructor

		public DetalleCuentaClientePRE(IDetalleCuentaClienteVIS vista, IucDatosRepresentanteLegalVIS vistaRepresentante)
		{
			try
			{
				this.vista = vista;
				this.vistaRepresentante = vistaRepresentante;
				dctx = FacadeBR.ObtenerConexion();
				clienteBR = new CuentaClienteIdealeaseBR();
				presentadorRepresentante = new ucDatosRepresentanteLegalPRE(vistaRepresentante);
			}
			catch (Exception ex)
			{
				this.MostrarMensaje("Inconsistencias en la configuración", ETipoMensajeIU.ERROR, nombreClase + ex.Message);
			}
		}

		#endregion Constructor

		#region Métodos

		#region SC0005

		public void MostrarDetalleObligado(ObligadoSolidarioBO obligado)
		{
			vista.MostrarDetalleObligado(((ObligadoSolidarioMoralBO)obligado).Representantes);
		}

		#endregion SC0005

		public void ConsultarCliente(object oBo)
		{
			try
			{
				if (oBo == null)
					throw new Exception(this.nombreClase + ".ConsultarCliente: se esperaba un objeto en la navegación. No se puede identificar qué cliente se desea consultar.");
				if (!(oBo is CuentaClienteIdealeaseBO))
					throw new Exception(this.nombreClase + ".ConsultarCliente: Se esperaba un cliente de Idealease.");

				CuentaClienteIdealeaseBO bo = (CuentaClienteIdealeaseBO)oBo;
				List<CuentaClienteIdealeaseBO> lstClientes = new List<CuentaClienteIdealeaseBO>();

				lstClientes = this.clienteBR.ConsultarCompleto(dctx, bo);
				if (lstClientes.Count < 1)
					throw new Exception("No se encontró ningún registro que corresponda a la información proporcionada.");
				if (lstClientes.Count > 1)
					throw new Exception("La consulta devolvió más de un registro.");

				if (bo.Cliente.Fisica == true) { this.vista.OcultarActaConstitutiva(); this.vista.MostrarHacienda(); }
				if (bo.Cliente.Fisica == false) { this.vista.OcultarHacienda(); this.vista.MostrarActaConstitutiva(); }
				lstClientes[0].Cliente.RFC = bo.Cliente.RFC;
				lstClientes[0].Cliente.Fisica = bo.Cliente.Fisica;
			    lstClientes[0].UnidadOperativaId = bo.UnidadOperativaId;
			    lstClientes[0].Cliente.Id = bo.Cliente.Id;
                lstClientes[0].SectorCliente = bo.SectorCliente;
				this.PrepararListasAMostrar(lstClientes[0]);
				CuentaClienteIdealeaseBO cliente = this.PrepararListasAMostrar(lstClientes[0]);
                this.ObtenerCreditosCliente(cliente.Id);
				this.vista.EstablecerDatosNavegacion("DatosCuentaClienteIdealeaseBO", cliente);
				this.DatoAInterfazUsuario(cliente);
			}
			catch (Exception ex)
			{
				throw new Exception(this.nombreClase + ".ConsultarCliente: " + ex.Message);
			}
		}

        public void InterfazUsuarioADato()
        {
            vista.Cliente = new CuentaClienteIdealeaseBO();
            vista.Cliente.Cliente = new ClienteBO();
            vista.Cliente.ActaConstitutiva = new ActaConstitutivaBO();
            vista.Cliente.AuditoriaIdealease = new AuditoriaBO();
            vista.Cliente.UnidadOperativaId = vista.UnidadOperativaId;
            vista.Cliente.Id = vista.ClienteID;
            vista.Cliente.Cliente.Id = vista.ClienteID;
            vista.Cliente.Cliente.Activo = vista.Cliente.Activo;
            vista.Cliente.ActaConstitutiva.FechaEscritura = vista.FechaEscritura;
            vista.Cliente.ActaConstitutiva.NumeroEscritura = vista.NumeroEscritura;
            vista.Cliente.ActaConstitutiva.FechaRPPC = vista.FechaRPPC;
            vista.Cliente.ActaConstitutiva.LocalidadNotaria = vista.LocalidadNotaria;
            vista.Cliente.ActaConstitutiva.LocalidadRPPC = vista.LocalidadRPPC;
            vista.Cliente.ActaConstitutiva.NombreNotario = vista.NombreNotario;
            vista.Cliente.ActaConstitutiva.NumeroNotaria = vista.NumeroNotaria;
            //vista.Cliente.AuditoriaIdealease.UC = vista.UC;
            vista.Cliente.AuditoriaIdealease.UUA = vista.UUA;
            //vista.Cliente.AuditoriaIdealease.FC = vista.FC;
            vista.Cliente.AuditoriaIdealease.FUA = vista.FUA;
            vista.Cliente.EsFisico = vista.Fisica;
            vista.Cliente.FechaRegistroHacienda = vista.FechaRegistro;
            vista.Cliente.GiroEmpresa = vista.GiroEmpresa;
            vista.Cliente.CURP = vista.CURP;
            vista.Cliente.Correo = vista.Correo;
            vista.Cliente.DiasUsoUnidad = vista.DiasUsoUnidad;
            vista.Cliente.HorasUsoUnidad = vista.DiasUsoUnidad;
            vista.Cliente.Direccion = vista.Cliente.Direccion;
            vista.Cliente.Telefonos = vista.ListaTelefonos;
            if(vista.SectorCliente != null)
                vista.Cliente.SectorCliente = vista.SectorCliente;
            vista.Cliente.TipoCuenta = vista.TipoCuenta;
            vista.Cliente.Observaciones = vista.Observaciones;
        }

		public void DatoAInterfazUsuario(CuentaClienteIdealeaseBO bo)
		{		    
            if (bo.ActaConstitutiva != null)
            {
                this.vista.FechaEscritura = bo.ActaConstitutiva.FechaEscritura;
		        this.vista.LocalidadNotaria = bo.ActaConstitutiva.LocalidadNotaria;
		        this.vista.LocalidadRPPC = bo.ActaConstitutiva.LocalidadRPPC;
		        this.vista.NombreNotario = bo.ActaConstitutiva.NombreNotario;
		        this.vista.NumeroEscritura = bo.ActaConstitutiva.NumeroEscritura;
		        this.vista.NumeroFolio = bo.ActaConstitutiva.NumeroRPPC;
		        this.vista.NumeroNotaria = bo.ActaConstitutiva.NumeroNotaria;
		        this.vista.FechaRPPC = bo.ActaConstitutiva.FechaRPPC;
            }
			
			this.vista.FechaRegistro = bo.FechaRegistroHacienda;
			this.vista.GiroEmpresa = bo.GiroEmpresa;
			
			this.vista.Fisica = bo.Cliente.Fisica;
			
			this.vista.Nombre = bo.Nombre;
		    this.vista.ClienteID = bo.Id;
			
            if (bo.ObligadosSolidarios != null)
			    this.vista.Obligados = bo.ObligadosSolidarios.ConvertAll(o => (ObligadoSolidarioBO)o);
            if (bo.RepresentantesLegales != null)
		        this.vista.Representantes = bo.RepresentantesLegales.ConvertAll(r => (RepresentanteLegalBO)r);

            if (bo.Telefonos != null)
                this.vista.ListaTelefonos = bo.Telefonos.ConvertAll(t=> (TelefonoClienteBO)t);

            
            this.vista.RFC = bo.Cliente.RFC;
			this.vista.TipoCuenta = bo.TipoCuenta;
			this.vista.CURP = bo.CURP;
            #region SC0001
            this.vista.Correo = bo.Correo;
            this.vista.DiasUsoUnidad = bo.DiasUsoUnidad;
            this.vista.HorasUsoUnidad = bo.HorasUsoUnidad;
            #endregion            
		    this.vista.NumeroCuentaOracle = bo.Numero;
		    if(bo.Observaciones != null || bo.Observaciones != string.Empty)this.vista.Observaciones = bo.Observaciones;
            if (bo.SectorCliente != null) 
                this.vista.SectorCliente = bo.SectorCliente;
		}

		public void Editar()
		{
			CuentaClienteIdealeaseBO cliente = (CuentaClienteIdealeaseBO)this.vista.ObtenerDatos();

			this.LimpiarSesion();
			this.vista.EstablecerDatosNavegacion("DatosCuentaClienteIdealeaseBO", cliente);

			this.vista.RedirigirAEdicion();
		}

		public void Inicializar()
		{
			this.PrepararNuevo();
			this.presentadorRepresentante.PrepararNuevo();
			this.ConsultarCliente(this.vista.ObtenerDatos());
			this.vista.MostrarDatos();
			this.ModoPresentarInformacion();
            this.EstablecerSeguridad();
		    this.presentadorRepresentante.EstablecerAcciones(vista.ListaAcciones);
		    this.vistaRepresentante.EstablecerAcciones(vista.ListaAcciones, false);
		}

	    /// <summary>
	    /// Método que establece la lista de acciones de los user controls, e inicializa el método de la vista.
	    /// </summary>
	    private void EstablecerAcciones()
	    {
	        this.presentadorRepresentante.EstablecerAcciones(this.vista.ListaAcciones);
	        this.vista.EstablecerAcciones();
	    }

		public void LimpiarSesion()
		{
			this.vista.LimpiarSesion();
		}

		public void ModoPresentarInformacion()
		{
			this.vista.DeshabilitarCampos();
		}

		public void MostrarDetalleRepresentante(RepresentanteLegalBO representante)
		{
			this.presentadorRepresentante.PrepararNuevo();
			this.presentadorRepresentante.MostrarDatosRepresentanteLegal(representante);
			this.vistaRepresentante.HabilitarCampos(false,false);
		}

		public void MostrarMensaje(string mensaje, ETipoMensajeIU tipo, string detalle = null)
		{
			this.vista.MostrarMensaje(mensaje, tipo, detalle);
		}

		public void PrepararNuevo()
		{
			this.vista.TipoCuenta = null;
			this.vista.FechaEscritura = null;
			this.vista.FechaRegistro = null;
			this.vista.FechaRPPC = null;
			this.vista.LocalidadNotaria = null;
			this.vista.LocalidadRPPC = null;
			this.vista.NombreNotario = null;
			this.vista.NumeroEscritura = null;
			this.vista.NumeroFolio = null;
			this.vista.NumeroNotaria = null;
			this.vista.CURP = null;
            #region SC0001
            this.vista.Correo = null;
            this.vista.DiasUsoUnidad = null;
            this.vista.HorasUsoUnidad = null;
           #endregion
            this.vista.NumeroCuentaOracle = null;
            this.EstablecerSeguridad();
		    this.EstablecerAcciones();
		    this.vista.MostrarObservaciones();
		}

		private CuentaClienteIdealeaseBO PrepararListasAMostrar(CuentaClienteIdealeaseBO cliente)
		{
			List<RepresentanteLegalBO> representantes = new List<RepresentanteLegalBO>();
			foreach (RepresentanteLegalBO r in cliente.RepresentantesLegales)
			{
				if (r.Activo == true)
				{
					representantes.Add(r);
				}
			}
			cliente.RepresentantesLegales = representantes.ConvertAll(r => (PersonaBO)r);
			List<ObligadoSolidarioBO> obligados = new List<ObligadoSolidarioBO>();
			foreach (ObligadoSolidarioBO o in cliente.ObligadosSolidarios)
			{
				if (o.Activo == true)
				{
					obligados.Add(o);
				}
			}
			cliente.ObligadosSolidarios = obligados.ConvertAll(o => (PersonaBO)o);

			return cliente;
		}
        /// <summary>
        /// Consulta los creditos activos del Cliente y los envía a la interfaz
        /// </summary>
        /// <param name="cuentaClienteId">Identificador de CuentaCliente para busqueda del Credito</param>
        private void ObtenerCreditosCliente(int? cuentaClienteId)
        {
            var listaMonedas = FacadeBR.ConsultarMoneda(this.dctx, new MonedaBO() { Activo = true });
            if(listaMonedas == null) { this.vista.MostrarCreditoCliente(new List<CreditoClienteBO>()); return; }
            var listaCreditosPresentar = new List<CreditoClienteBO>();
            foreach(var moneda in listaMonedas)
            {
                var creditosCliente = FacadeEFacturacionBR.ConsultarCreditoCliente(this.dctx, new CreditoClienteBO()
                {
                    CuentaCliente = new CuentaClienteBO() { Id = cuentaClienteId },
                    Moneda = moneda,
                    Activo = true
                });
                if(creditosCliente != null && creditosCliente.Any())
                {
                    creditosCliente.First().Moneda = moneda;
                    listaCreditosPresentar.Add(creditosCliente.First());
                }
            }
            this.vista.MostrarCreditoCliente(listaCreditosPresentar);
        }

        #region SC_0008
        public void ValidarAcceso()
        {
            try
            {
                //se valida que los datos del usuario y la unidad operativa no sean nulos
                if (this.vista.UsuarioId == null) throw new Exception("El identificador del usuario no debe ser nulo");
                if (this.vista.UnidadOperativaId == null) throw new Exception("El identificador de la Unidad Operativa no debe ser nulo ");

                //Se crea el objeto de seguridad
                UsuarioBO usuario = new UsuarioBO() { Id = this.vista.UsuarioId };
                AdscripcionBO adscripcion = new AdscripcionBO() { UnidadOperativa = new UnidadOperativaBO() { Id = this.vista.UnidadOperativaId } };
                SeguridadBO seguridadBO = new SeguridadBO(Guid.Empty, usuario, adscripcion);

                //Se valida si el usuario tiene permiso a la acción principal
                if (!FacadeBR.ExisteAccion(this.dctx, "CONSULTAR", seguridadBO))
                    this.vista.RedirigirSinPermisoAcceso();
            }
            catch (Exception ex)
            {
                throw new Exception(this.nombreClase + ".ValidarAcceso:" + ex.Message);
            }
        }
        private void EstablecerSeguridad()
        {
            try
            {
                //se valida que los datos del usuario y la unidad operativa no sean nulos
                if (this.vista.UsuarioId == null) throw new Exception("El identificador del usuario no debe ser nulo");
                if (this.vista.UnidadOperativaId == null) throw new Exception("El identificador de la Unidad Operativa no debe ser nulo ");

                //Se crea el objeto de seguridad
                UsuarioBO usuario = new UsuarioBO() { Id = this.vista.UsuarioId };
                AdscripcionBO adscripcion = new AdscripcionBO() { UnidadOperativa = new UnidadOperativaBO() { Id = this.vista.UnidadOperativaId } };
                SeguridadBO seguridadBO = new SeguridadBO(Guid.Empty, usuario, adscripcion);
                seguridad = seguridadBO;
                //Se obtienen las acciones a las cuales el usuario tiene permiso en este proceso
                this.vista.ListaAcciones = FacadeBR.ConsultarAccion(this.dctx, seguridadBO);
                
                //Se valida si el usuario tiene permiso para editar cuenta cliente
                if (!this.ExisteAccion(this.vista.ListaAcciones, "ACTUALIZARCOMPLETO"))
                    this.vista.PermitirEditar(false);
                //Se valida si el usuario tiene permiso para insertar cuenta cliente
                if (!this.ExisteAccion(this.vista.ListaAcciones, "INSERTARCOMPLETO"))
                    this.vista.PermitirRegistrar(false);
            }
            catch (Exception ex)
            {
                throw new Exception(this.nombreClase + ".EstablecerSeguridad:" + ex.Message);
            }
        }
        private bool ExisteAccion(List<CatalogoBaseBO> acciones, string nombreAccion)
        {
            if (acciones != null && acciones.Count > 0 && acciones.Exists(p => p != null && p.Nombre != null && p.Nombre.Trim().ToUpper() == nombreAccion.Trim().ToUpper()))
                return true;

            return false;
        }
        #endregion

        /// <summary>
        /// Obtiene información de los datos de navegación
        /// </summary>
        /// <returns></returns>
	    public object ObtenerDatosNavegacion()
	    {
	        return this.vista.ObtenerDatos();
	    }

        /// <summary>
        /// Actualiza la información de los datos del cliente con la información de oracle
        /// </summary>
	    public void ActualizarClienteOracle()
        {
            InterfazUsuarioADato();
            CuentaClienteIdealeaseBO clientes = (CuentaClienteIdealeaseBO)this.ObtenerDatosNavegacion();
            List<CuentaClienteIdealeaseBO> LastCliente = (List<CuentaClienteIdealeaseBO>)clienteBR.ConsultarCompleto(dctx, clientes);
            this.vista.ClienteAnterior = LastCliente[0];
            this.vista.Cliente.Activo = clientes.Cliente.Activo;
            #region SC0008
	        SeguridadBO seguridad = new SeguridadBO(Guid.Empty, new UsuarioBO() { Id = this.vista.UC },
	            new AdscripcionBO() { UnidadOperativa = new UnidadOperativaBO() { Id = this.vista.UnidadOperativaId } });
            clienteBR.ActualizarCompleto(dctx, vista.Cliente, vista.ClienteAnterior, seguridad);
	        #endregion

            vista.EstablecerPaquete(this.vista.ClienteAnterior);
            DatoAInterfazUsuario(this.vista.ClienteAnterior);
            vista.EstablecerAcciones();
	    }

		#endregion
	}
}