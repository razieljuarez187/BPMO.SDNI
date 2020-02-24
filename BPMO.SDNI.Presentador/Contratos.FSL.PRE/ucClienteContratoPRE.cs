// Satisface al caso de uso CU022 - Consultar Contratos Full Service Leasing
// Satisface al caso de uso CU023 - Editar Contrato Full Service Leasing 
// Satisface a la solicitud de cambio SC0021
using System;
using System.Collections.Generic;
using System.Linq;
using BPMO.Basicos.BO;
using BPMO.Patterns.Creational.DataContext;
using BPMO.Primitivos.Enumeradores;
using BPMO.SDNI.Comun.BO;
using BPMO.SDNI.Comun.BOF;
using BPMO.SDNI.Comun.BR;
using BPMO.SDNI.Contratos.BO;
using BPMO.SDNI.Contratos.FSL.BO;
using BPMO.SDNI.Contratos.FSL.VIS;

namespace BPMO.SDNI.Contratos.FSL.PRE
{
	public class ucClienteContratoPRE
	{
		#region Atributos

		/// <summary>
		/// El DataContext que proveerá acceso a la base de datos
		/// </summary>
		private readonly IDataContext dataContext;

		/// <summary>
		/// Nombre de la clase para agregar a los mensajes de Error
		/// </summary>
		private const string NombreClase = "ucClienteContratoPRE";

		 /// <summary>
		/// Vista sobre la que actua el presentador
		/// </summary>       

		private readonly IucClienteContratoVIS vista;

		#endregion Atributos

		#region Propiedades

		/// <summary>
		/// Vista sobre la que actua el Presentador de solo lectura
		/// </summary>
		internal IucClienteContratoVIS Vista { get { return vista; } }

		#endregion

		#region Constructores

		/// <summary>
		/// Contructor que recibe la vista sobre la que actuara el presentador
		/// </summary>
		/// <param name="vistaActual">vista sobre la que actuara el presentador</param>
		public ucClienteContratoPRE(IucClienteContratoVIS vistaActual)
		{
			if (vistaActual != null)
				vista = vistaActual;

			dataContext = Facade.SDNI.BR.FacadeBR.ObtenerConexion();
		}

		#endregion Constructores

		#region Metodos
		/// <summary>
		/// Inicializa los datos de la vista
		/// </summary>
		public void Inicializar()
		{
			vista.ClienteID = null;
			vista.CuentaClienteID = null;
            vista.SoloRepresentantes = null;
            vista.EsFisico = null;
			vista.ListadoObligadosSolidarios = null;
			vista.ListadoRepresentantesLegales = null;
			vista.NombreCuentaCliente = null;
		    vista.ClienteNumeroCuenta = null;
			vista.RepresentantesLegalesContrato = null;
			vista.ObligadosSolidariosContrato = null;
            vista.HabilitarSoloRepresentantes(false);
			vista.ObligadosComoAvales = false;
			vista.MostrarObligadosSolidarios(true);
			vista.MostrarRepresentantesObligados(false);
			vista.AvalesTotales = null;
			vista.AvalesSeleccionados = null;
			vista.ActualizarAvales();
			vista.MostrarRepresentantesAval(false);
			vista.PermitirSeleccionarAvales(false);
			vista.PermitirAgregarAvales(false);
			vista.HabilitarObligadosComoAvales(false);
			vista.MostrarAvales(false);
		}

		/// <summary>
		/// Obtiene y Muesta el listado de Representantes Legales y Obligados Solidarios
		/// </summary>
		private void DesplegarPersonasCliente()
		{
			try
			{
				// Variables
				var cciBR = new CuentaClienteIdealeaseBR();
				vista.ListadoObligadosSolidarios = null;
				vista.ListadoRepresentantesLegales = null;
				//Inicializa los Representantes Legales del Contrato
				vista.RepresentantesLegalesContrato = null;
				//Inicializa los Obligados Solidarios del Contrato
				vista.ObligadosSolidariosContrato = null;
				//Inicializar los avales
				vista.AvalesTotales = null;
				vista.AvalesSeleccionados = null;
				vista.ActualizarAvales();
				if (vista.UnidadOperativa == null) throw new Exception("Se requiere de la unidad operativa para realizar la operación");

				// Se consultan y obtiene el  Cliente Completo
				if (vista.CuentaClienteID != null)
				{
					var cliente = new CuentaClienteIdealeaseBO { Id = vista.CuentaClienteID, Cliente = new ClienteBO { Id = vista.ClienteID }, UnidadOperativa = vista.UnidadOperativa };
					List<CuentaClienteIdealeaseBO> listado = cciBR.ConsultarCompleto(dataContext, cliente);
					cliente = listado.Find(cli => cli.Id == vista.CuentaClienteID);

					if (cliente != null)
					{
						var representantesActivos = new List<PersonaBO>(cliente.RepresentantesLegales.Where(
							persona => persona.Activo == true)).ConvertAll(s => (RepresentanteLegalBO)s);

						foreach (var representantesActivo in representantesActivos.Where(representantesActivo => representantesActivo.EsDepositario == true))
						{
							representantesActivo.Nombre = "(D) " + representantesActivo.Nombre;
						}

						vista.HabilitarListadoRepresentantesLegales(representantesActivos.Count > 0);
						vista.HabilitarAgregarRepresentanteLegal(representantesActivos.Count > 0);
						vista.ListadoRepresentantesLegales = representantesActivos;


						var obligadosActivos = new List<PersonaBO>(cliente.ObligadosSolidarios.Where(persona => persona.Activo == true)).ConvertAll(s => (ObligadoSolidarioBO)s);
						vista.HabilitarListadoObligadosSolidarios(obligadosActivos.Count > 0);
						vista.HabilitarAgregarObligadoSolidario(obligadosActivos.Count > 0);
						vista.ListadoObligadosSolidarios = obligadosActivos.ConvertAll(s => (ObligadoSolidarioBO)s);
						vista.HabilitarObligadosComoAvales(obligadosActivos.Count>0);
						//Mostrar Avales
						List<AvalBO> lstAvales = null;
						if (obligadosActivos != null)
							lstAvales = obligadosActivos.ConvertAll(s => this.ObligadoAAval(s));
						this.vista.AvalesTotales = lstAvales;
						this.vista.AvalesSeleccionados = null;

						//Sólo permite seleccionar avales si el cliente ha sido seleccionado
						this.vista.PermitirSeleccionarAvales(this.vista.ModoEditar && this.vista.CuentaClienteID != null);
						//Sólo permite agregar avales si el cliente ha sido seleccionado y tiene obligados solidarios configurados
						this.vista.PermitirAgregarAvales(this.vista.ModoEditar && this.vista.AvalesTotales != null && this.vista.AvalesTotales.Count > 0);

						vista.ActualizarAvales();
						if (vista.SoloRepresentantes != null && vista.SoloRepresentantes == false)
							if (vista.ObligadosComoAvales != null && vista.ObligadosComoAvales == false)
								vista.MostrarAvales(true);
						
					}
					else
						throw new Exception("El cliente seleccionado no es valido en el sistema.");

				}
				else
					throw new Exception("Se requiere seleccionar un Cliente para obtener los listados de Representantes Legales y Obligados Solidarios.");
			}
			catch (Exception ex)
			{
				vista.MostrarMensaje("Inconsistencias al presentar la información", ETipoMensajeIU.ERROR, NombreClase + ".MostrarPersonasCliente : " + ex.Message);
			}
		}

		/// <summary>
		/// Agrega un Obligados Solidario al Contrato
		/// </summary>
		/// <param name="obligado">Obligado Solidario a Agregar al Contrato</param>
		public void AgregarObligadoSolidarioContrato(ObligadoSolidarioBO obligado)
		{
			try
			{
				if (obligado != null && obligado.Id != null)
				{
					if (vista.ObligadosSolidariosContrato.Find(obl => obligado.Id == obl.Id) == null)
					{

						if (obligado.DireccionPersona == null || string.IsNullOrEmpty(obligado.DireccionPersona.Calle))
						{
							var cciBR = new CuentaClienteIdealeaseBR();
							List<ObligadoSolidarioBO> obls = cciBR.ConsultarObligadosSolidarios(dataContext, obligado, new CuentaClienteIdealeaseBO());

							ObligadoSolidarioBO encontrado = obls.Find(ob => ob.Id == obligado.Id);

							if (encontrado != null) obligado = encontrado;
						}

						var Obligados = new List<ObligadoSolidarioBO>(vista.ObligadosSolidariosContrato) { obligado };
						vista.ObligadosSolidariosContrato = Obligados;
					}
					else
						vista.MostrarMensaje("El Obligado Solidario seleccionado ya se encuentra asignado al contrato.", ETipoMensajeIU.ADVERTENCIA);
				}
				else
					vista.MostrarMensaje("Se requiere de un Obligado Solidario seleccionado valido para agregar al contrato.", ETipoMensajeIU.ADVERTENCIA);
			}
			catch (Exception ex)
			{
				vista.MostrarMensaje("Inconsistencias al intentar agregar un Obligado Solidario al contrato.", ETipoMensajeIU.ERROR, NombreClase + ".AgregarObligadoSolidarioContrato :" + ex.Message);
			}
		}

		/// <summary>
		/// Agrega un Representante Legal al Contrato al Contrato
		/// </summary>
		/// <param name="representante">Representante Lega a agregar</param>
		public void AgregarRepresentanteLegalContrato(RepresentanteLegalBO representante)
		{
			try
			{
				if (representante != null && representante.Id != null)
				{
					if (vista.RepresentantesLegalesContrato.Find(obl => representante.Id == obl.Id) == null)
					{
						var Representantes = new List<RepresentanteLegalBO>(vista.RepresentantesLegalesContrato);

						var cliente = new CuentaClienteIdealeaseBO
						{
							Id = vista.CuentaClienteID,
							Cliente =
								new ClienteBO
								{
									Id = vista.ClienteID
								}
						};

						var cciBR = new CuentaClienteIdealeaseBR();
						representante = cciBR.ConsultarRepresentantesLegales(dataContext, new RepresentanteLegalBO{Id = representante.Id}, cliente
																			 )
											 .Find(rep => rep.Id == representante.Id);

                        // Si es Depositario
                        if (representante.EsDepositario == true)
                        {
                            // Reestablece el valor de es depositario en los representantes legales.
                            foreach (var item in vista.ListadoRepresentantesLegales)
                            {
                                var replegal = Representantes.Find(rep => rep.Id == item.Id);

                                if (replegal != null) replegal.EsDepositario = item.EsDepositario;
                            }
                        }


						Representantes.Add(representante);

						vista.RepresentantesLegalesContrato = Representantes;
					}
					else
						vista.MostrarMensaje("El Representante Legal seleccionado ya se encuentra asignado al contrato.", ETipoMensajeIU.ADVERTENCIA);
				}
				else
					vista.MostrarMensaje("Se requiere de un Representante Legal seleccionado valido para agregar al contrato.", ETipoMensajeIU.ADVERTENCIA);
			}
			catch (Exception ex)
			{
				vista.MostrarMensaje("Inconsistencias al intertar agregar un Representante Legal al Contrato", ETipoMensajeIU.ERROR, NombreClase + ".AgregarRepresentanteLegalContrato: " + ex.Message);
			}
		}

		/// <summary>
		/// Remueve un Obligado Solidario al Contrato
		/// </summary>
		/// <param name="obligado">Obligado Solidario a remover del contrato</param>
		public void RemoverObligadoSolidarioContrato(ObligadoSolidarioBO obligado)
		{
			try
			{
				if (obligado != null && obligado.Id != null)
				{
					if (vista.ObligadosSolidariosContrato.Find(obl => obligado.Id == obl.Id) != null)
					{
						var Obligados =
							new List<ObligadoSolidarioBO>(vista.ObligadosSolidariosContrato);
						Obligados.Remove(obligado);
						vista.ObligadosSolidariosContrato = Obligados;
					}
					else
						throw new Exception("El Obligado Solidario proporcionado no se encuentra asignado al contrato.");
				}
				else
					throw new Exception(
						"Se requiere de un Obligado Solidario seleccionado valido para realizar la operación.");
			}
			catch (Exception ex)
			{
				vista.MostrarMensaje("Inconsistencias al intertar remover un Obligado Solidario del Contrato", ETipoMensajeIU.ERROR, NombreClase + ".RemoverObligadoSolidarioContrato: " + ex.Message);
			}
		}

		/// <summary>
		/// Remueve un Representante Legal del Contrato
		/// </summary>
		/// <param name="representante">Representante Lega a Remover del Contrato</param>
		public void RemoverRepresentanteLegalContrato(RepresentanteLegalBO representante)
		{
			try
			{

				if (representante != null && representante.Id != null)
				{
					if (vista.RepresentantesLegalesContrato.Find(obl => representante.Id == obl.Id) != null)
					{
						var Representantes = new List<RepresentanteLegalBO>(vista.RepresentantesLegalesContrato);
						Representantes.Remove(representante);
						vista.RepresentantesLegalesContrato = Representantes;
					}
					else
						throw new Exception("El Representante Legal proporcionado no se encuentra asignado al contrato.");
				}
				else
					throw new Exception("Se requiere de un Representante Legal seleccionado valido para realizar la operación.");

			}
			catch (Exception ex)
			{
				vista.MostrarMensaje("Inconsistencias al intertar remover un Representante Legal del Contrato", ETipoMensajeIU.ERROR, NombreClase + ".RemoverRepresentanteLegalContrato: " + ex.Message);
			}
		}

		/// <summary>
		/// Prepara un BO para la Busqueda en su respectivo catalogo
		/// </summary>
		/// <param name="catalogo">catalogo donde se realizara la busqueda</param>
		/// <returns></returns>
		public object PrepararBOBuscador(string catalogo)
		{
			object obj = null;

			switch (catalogo)
			{
				case "CuentaClienteIdealease":
					var cliente = new CuentaClienteIdealeaseBOF { Nombre = vista.NombreCuentaCliente, UnidadOperativa = vista.UnidadOperativa, Cliente = new ClienteBO(), Activo = true};
					obj = cliente;
					break;
				case "DireccionCliente":
					var bo = new CuentaClienteIdealeaseBO { UnidadOperativa = vista.UnidadOperativa, Cliente = new ClienteBO { Id = vista.ClienteID }, Id = vista.CuentaClienteID };
					var bof = new DireccionCuentaClienteBOF { Cuenta = bo, Direccion = new DireccionClienteBO{Facturable = true} };
					obj = bof;
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
				case "CuentaClienteIdealease":
					CuentaClienteIdealeaseBOF cliente = (CuentaClienteIdealeaseBOF)selecto ??
														new CuentaClienteIdealeaseBOF();

					if (cliente.Cliente == null)
						cliente.Cliente = new ClienteBO();

					vista.CuentaClienteID = cliente.Id;

					vista.ClienteID = cliente.Cliente.Id;

					vista.NombreCuentaCliente = !string.IsNullOrEmpty(cliente.Nombre) ? cliente.Nombre : string.Empty;
			        vista.ClienteNumeroCuenta = cliente.Numero;
                    vista.EsFisico = cliente.EsFisico;

					if (vista.CuentaClienteID != null && vista.ClienteID != null)
					{
						DesplegarPersonasCliente();
						InicializarDireccionCliente();
						vista.HabilitarConsultaDireccionCliente(true);

                        if (vista.EsFisico == true)
                            vista.ConfigurarClienteFisico();
                        else
                            vista.ConfigurarClienteMoral();

					}
					else
					{
						InicializarPersonasCliente();
						InicializarDireccionCliente();
                        Inicializar();
					}
					break;
				case "DireccionCliente":

			        if (selecto == null)
                    {
                        vista.MostrarMensaje("No se ha seleccionado una dirección Facturable.", ETipoMensajeIU.INFORMACION);
                    }
                    DireccionCuentaClienteBOF bof = (DireccionCuentaClienteBOF)selecto ?? new DireccionCuentaClienteBOF();
					EstablecerDatosDireccionCliente(bof);
					break;
			}
		}

		/// <summary>
		/// Inicializa los datos de las personas relacionadas al cliente
		/// </summary>
		private void InicializarPersonasCliente()
		{
			vista.ObligadosSolidariosContrato = null;
			vista.RepresentantesLegalesContrato = null;
			vista.ListadoObligadosSolidarios = null;
			vista.ListadoRepresentantesLegales = null;
			vista.HabilitarListadoObligadosSolidarios(false);
			vista.HabilitarListadoRepresentantesLegales(false);
			vista.HabilitarAgregarObligadoSolidario(false);
			vista.HabilitarAgregarRepresentanteLegal(false);
			vista.AvalesSeleccionados = null;
			vista.AvalesTotales = null;
			vista.HabilitarObligadosComoAvales(false);
			vista.PermitirSeleccionarAvales(false);
			vista.PermitirAgregarAvales(false);
			vista.ActualizarAvales();
		}

		///<summary>
		///Inicializa los datos de la direccion del cliente
		///</summary>
		private void InicializarDireccionCliente()
		{
			vista.Direccion = string.Empty;
			vista.Calle = string.Empty;
			vista.Ciudad = string.Empty;
			vista.CodigoPostal = string.Empty;
			vista.Colonia = string.Empty;
			vista.Estado = string.Empty;
			vista.Municipio = string.Empty;
			vista.Pais = string.Empty;
			vista.HabilitarConsultaDireccionCliente(false);
            vista.DireccionClienteID = null;
		}
		/// <summary>
		/// Despliega los datos del contrato en la interfaz de usuario
		/// </summary>
		/// <param name="contrato">Contrato que contiene los datos a desplegar</param>
		public void DatosAInterfazUsuario(ContratoFSLBO contrato)
		{
			if(contrato == null) contrato = new ContratoFSLBO();
			if(contrato.Cliente == null) contrato.Cliente = new CuentaClienteIdealeaseBO();
			if(contrato.Cliente.Cliente == null) contrato.Cliente.Cliente = new ClienteBO();

			vista.CuentaClienteID = contrato.Cliente.Id;
			vista.ClienteID = contrato.Cliente.Cliente.Id;
			vista.NombreCuentaCliente = contrato.Cliente.Nombre;
		    vista.ClienteNumeroCuenta = contrato.Cliente.Numero;
            vista.EsFisico = contrato.Cliente.EsFisico;
            vista.SoloRepresentantes = contrato.SoloRepresentantes;
			vista.ObligadosComoAvales = contrato.ObligadosComoAvales;
            
			if (contrato.Cliente.Direcciones.Count > 0)
			{
				if (contrato.Cliente.Direcciones.Count == 1)
				{
					var bof= new DireccionCuentaClienteBOF { Direccion = contrato.Cliente.Direcciones[0] };
					EstablecerDatosDireccionCliente(bof);
				}
				else
				{
					vista.MostrarMensaje("Inconsistencias en las direcciones del cliente, la consulta regreso mas de una dirección", ETipoMensajeIU.ADVERTENCIA);
				}
			}

			if (vista.CuentaClienteID != null && vista.ClienteID != null && !string.IsNullOrEmpty(vista.NombreCuentaCliente) && vista.ModoEditar)
				DesplegarPersonasCliente();
            
            if (vista.EsFisico != true)
            {
                vista.RepresentantesLegalesContrato = contrato.RepresentantesLegales.ConvertAll(s => (RepresentanteLegalBO)s);
                vista.ConfigurarClienteMoral();
            }
            else
            {
                vista.HabilitarAgregarRepresentanteLegal(false);
                vista.HabilitarListadoRepresentantesLegales(false);
                vista.HabilitarSoloRepresentantes(false);
				vista.PermitirAgregarAvales(false);
				vista.PermitirSeleccionarAvales(false);
				vista.HabilitarObligadosComoAvales(false);
                vista.ConfigurarClienteFisico();
            }

            if (vista.SoloRepresentantes != true)
            {
                vista.ObligadosSolidariosContrato = contrato.ObligadosSolidarios.ConvertAll(s => (ObligadoSolidarioBO)s);
                vista.HabilitarAgregarObligadoSolidario(true);
                vista.HabilitarListadoObligadosSolidarios(true);
				if(vista.ObligadosComoAvales!=true)
				{
					vista.AvalesSeleccionados = new List<AvalBO>(contrato.Avales);
					vista.PermitirAgregarAvales(true);
					vista.PermitirSeleccionarAvales(true);
					vista.HabilitarObligadosComoAvales(true);
					vista.ActualizarAvales();
					Vista.MostrarAvales(true);
				}
            }
            else
            {
                vista.HabilitarAgregarObligadoSolidario(false);
                vista.HabilitarListadoObligadosSolidarios(false);
                vista.MostrarObligadosSolidarios(false);
				vista.MostrarAvales(false);
				vista.PermitirAgregarAvales(false);
				vista.PermitirSeleccionarAvales(false);
            }
		}

        #region SC0001
        public void EstablecerDatosDireccionCliente(DireccionCuentaClienteBOF bof)
		{
			if (bof.Direccion == null)
				bof.Direccion = new DireccionClienteBO();
			if (bof.Direccion.Ubicacion == null)
				bof.Direccion.Ubicacion = new UbicacionBO();
			if (bof.Direccion.Ubicacion.Ciudad == null)
				bof.Direccion.Ubicacion.Ciudad = new CiudadBO();
			if (bof.Direccion.Ubicacion.Estado == null)
				bof.Direccion.Ubicacion.Estado = new EstadoBO();
			if (bof.Direccion.Ubicacion.Municipio == null)
				bof.Direccion.Ubicacion.Municipio = new MunicipioBO();
			if (bof.Direccion.Ubicacion.Pais == null)
				bof.Direccion.Ubicacion.Pais = new PaisBO();

				vista.Calle = bof.Direccion.Calle;
				vista.Direccion = bof.Direccion.Direccion;
				vista.Colonia = bof.Direccion.Colonia;
				vista.CodigoPostal = bof.Direccion.CodigoPostal;
				vista.Ciudad = bof.Direccion.Ubicacion.Ciudad.Codigo;
				vista.Estado = bof.Direccion.Ubicacion.Estado.Codigo;
				vista.Municipio = bof.Direccion.Ubicacion.Municipio.Codigo;
				vista.Pais = bof.Direccion.Ubicacion.Pais.Codigo;
                vista.DireccionClienteID = bof.Direccion.Id;
			
		}
        #endregion

        #region SC0005
        /// <summary>
        /// Agrega un Obligados Solidario al Contrato
        /// </summary>
        /// <param name="obligado">Obligado Solidario a Agregar al Contrato</param>
        /// <param name="representantes">Representantes Legales del Obligado Solidario</param>
        public void AgregarObligadoSolidarioContrato(ObligadoSolidarioBO obligado, List<RepresentanteLegalBO> representantes)
        {
            try
            {
                if (representantes == null)
                    vista.MostrarMensaje("Es necesario seleccionar al menos un representante legal, para el obligado solidarío.", ETipoMensajeIU.ADVERTENCIA);
                if (representantes.Count <= 0)
                    vista.MostrarMensaje("Es necesario seleccionar al menos un representante legal, para el obligado solidarío.", ETipoMensajeIU.ADVERTENCIA);

                if (obligado != null && obligado.Id != null)
                {
                    if (vista.ObligadosSolidariosContrato.Find(obl => obligado.Id == obl.Id) == null)
                    {

                        if (obligado.DireccionPersona == null || string.IsNullOrEmpty(obligado.DireccionPersona.Calle))
                        {
                            var cciBR = new CuentaClienteIdealeaseBR();
                            List<ObligadoSolidarioBO> obls = cciBR.ConsultarObligadosSolidarios(dataContext, obligado, new CuentaClienteIdealeaseBO());

                            ObligadoSolidarioBO encontrado = obls.Find(ob => ob.Id == obligado.Id);

                            if (encontrado != null) obligado = encontrado;
                        }
                        //SC0005
                        ((ObligadoSolidarioMoralBO)obligado).Representantes = representantes;

                        var Obligados = new List<ObligadoSolidarioBO>(vista.ObligadosSolidariosContrato) { obligado };
                        vista.ObligadosSolidariosContrato = Obligados;
                    }
                    else
                        vista.MostrarMensaje("El Obligado Solidario seleccionado ya se encuentra asignado al contrato.", ETipoMensajeIU.ADVERTENCIA);
                }
                else
                    vista.MostrarMensaje("Se requiere de un Obligado Solidario seleccionado valido para agregar al contrato.", ETipoMensajeIU.ADVERTENCIA);
            }
            catch (Exception ex)
            {
                vista.MostrarMensaje("Inconsistencias al intentar agregar un Obligado Solidario al contrato.", ETipoMensajeIU.ERROR, NombreClase + ".AgregarObligadoSolidarioContrato :" + ex.Message);
            }
        }

        public void MostrarDetalleObligado(ObligadoSolidarioBO obligado)
        {
            vista.MostrarDetalleObligado(((ObligadoSolidarioMoralBO)obligado).Representantes);
        }

        public void ObtenerRepresentanteOS(ObligadoSolidarioBO obligado)
        {
            var cuentaBR = new CuentaClienteIdealeaseBR();
            try
            {
                if (obligado != null && obligado.Id != null)
                {
                        List<ObligadoSolidarioBO> obligs = cuentaBR.ConsultarObligadosSolidarios(dataContext, obligado, new CuentaClienteIdealeaseBO { Id = vista.CuentaClienteID });

                        if (obligs == null)
                            return;
                        if (obligs.Count <= 0)
                            return;
                        if (obligs.Count > 1)
                            throw new Exception("La consulta ha devuelto más de un valor, esto podría significar un conflicto en el sistema, verifique su información por favor");

                        ObligadoSolidarioBO oblig = obligs[0];
                        if (oblig.TipoObligado == ETipoObligadoSolidario.Moral)
                        {
                            vista.RepresentantesObligado = cuentaBR.ConsultarRepresentantesLegales(dataContext,new CuentaClienteIdealeaseBO{Id = vista.CuentaClienteID},obligado,new RepresentanteLegalBO{Activo = true});
                            vista.MostrarRepresentantesObligados(true);
                        }
                        else
                        {
                            vista.RepresentantesObligado = new List<RepresentanteLegalBO>();
                        }
                    }
                
            }
            catch (Exception ex)
            {
                vista.MostrarMensaje("Inconsistencias al intentar agregar un Obligado Solidario al contrato.", ETipoMensajeIU.ERROR, NombreClase + ".ObtenerRepresentanteOS :" + ex.Message);
            }
        }

        public ObligadoSolidarioBO ConsultarObligadoSolidario(ObligadoSolidarioBO obligado)
        {
            ObligadoSolidarioBO obligadoa = null;
            try
            {
                if (obligado != null && obligado.Id != null)
                {
                    if (vista.ObligadosSolidariosContrato.Find(obl => obligado.Id == obl.Id) == null)
                    {

                        if (obligado.DireccionPersona == null || string.IsNullOrEmpty(obligado.DireccionPersona.Calle))
                        {
                            var cciBR = new CuentaClienteIdealeaseBR();
                            List<ObligadoSolidarioBO> obls = cciBR.ConsultarObligadosSolidarios(dataContext, obligado, new CuentaClienteIdealeaseBO());

                            ObligadoSolidarioBO encontrado = obls.Find(ob => ob.Id == obligado.Id);

                            if (encontrado != null) obligadoa = encontrado;
                        }
                    }
                    else
                        vista.MostrarMensaje("El Obligado Solidario seleccionado ya se encuentra asignado al contrato.", ETipoMensajeIU.ADVERTENCIA);
                }
                else
                    vista.MostrarMensaje("Se requiere de un Obligado Solidario seleccionado valido para agregar al contrato.", ETipoMensajeIU.ADVERTENCIA);
                
                return obligadoa;
            }
            catch (Exception ex)
            {
                throw new Exception("Inconsistencias al intentar consultar un Obligado Solidario al contrato." + ex.Message);
            }
        }
        #endregion

        #region SC0007
        public void ActivarObligadosSolidarios()
        {
            vista.HabilitarAgregarObligadoSolidario(true);
            vista.HabilitarListadoObligadosSolidarios(true);
            vista.MostrarObligadosSolidarios(true);     
        }

        public void DesactivarObligadosSolidarios()
        {
            vista.HabilitarAgregarObligadoSolidario(false);
            vista.HabilitarListadoObligadosSolidarios(false);
            vista.MostrarObligadosSolidarios(false);
            vista.ObligadosSolidariosContrato = null;
        }


        public void ConfigurarSoloRepresentantes()
        {
			if (vista.SoloRepresentantes == true)
			{
				DesactivarObligadosSolidarios();
				Vista.ObligadosComoAvales = true;
			}
			else
			{
				ActivarObligadosSolidarios();
				vista.RepresentantesLegalesContrato = null;
				vista.ObligadosComoAvales = false;
			}
			ConfigurarObligadosComoAvales();
	       
        }
        #endregion

		public void ConfigurarObligadosComoAvales()
		{
			bool soloRepresentantes = this.vista.SoloRepresentantes != null && this.vista.SoloRepresentantes.Value;
			bool obligadosComoAvales = this.vista.ObligadosComoAvales != null && this.vista.ObligadosComoAvales.Value;
			this.vista.PermitirAgregarAvales(this.vista.ModoEditar && !soloRepresentantes && !obligadosComoAvales);
			this.vista.PermitirSeleccionarAvales(this.vista.ModoEditar && !soloRepresentantes && !obligadosComoAvales);
			

			if (obligadosComoAvales)
			{
				this.vista.MostrarRepresentantesAval(false);

				this.vista.AvalesSeleccionados = null;
				this.vista.ActualizarAvales();
				vista.MostrarAvales(false);
			} else vista.MostrarAvales(true);
		}

		/// <summary>
		/// Realiza cálculos en base al aval seleccionado con base en, por ejemplo, su tipo
		/// </summary>
		public void SeleccionarAval()
		{
			try
			{
				this.vista.MostrarRepresentantesAval(false);
				this.vista.RepresentantesAvalTotales = null;
				this.vista.RepresentantesAvalSeleccionados = null;

				if (this.vista.AvalSeleccionadoID != null)
				{
					AvalBO bo = this.vista.AvalesTotales.Find(p => p.Id == this.vista.AvalSeleccionadoID).Clonar();
					if (bo == null)
						throw new Exception("No se encontró el aval seleccionado.");

					if (bo.TipoAval != null && bo.TipoAval == ETipoAval.Moral)
					{
						this.vista.MostrarRepresentantesAval(true);

						this.vista.RepresentantesAvalTotales = ((AvalMoralBO)bo).Representantes;
					}
				}
			}
			catch (Exception ex)
			{
				throw new Exception(NombreClase + ".SeleccionarAval: " + ex.Message);
			}
		}
		/// <summary>
		/// Agrega el aval seleccionado en la vista
		/// </summary>
		public void AgregarAval()
		{
			string s;
			if ((s = this.ValidarCamposAval()) != null)
			{
				this.vista.MostrarMensaje(s, ETipoMensajeIU.INFORMACION, null);
				return;
			}

			try
			{
				if (this.vista.AvalesSeleccionados == null)
					this.vista.AvalesSeleccionados = new List<AvalBO>();

				List<AvalBO> avalesSeleccionados = this.vista.AvalesSeleccionados;

				//Obtengo el aval de la lista de totales
				AvalBO bo = this.vista.AvalesTotales.Find(p => p.Id == this.vista.AvalSeleccionadoID).Clonar();
				if (bo == null)
					throw new Exception("No se encontró el aval seleccionado.");

				//Si el Aval es Moral, se completa el objeto antes de agregarlo a la lista
				if (bo.TipoAval != null && bo.TipoAval == ETipoAval.Moral)
				{
					((AvalMoralBO)bo).Representantes = this.vista.RepresentantesAvalSeleccionados;

					this.vista.MostrarRepresentantesAval(false);
					this.vista.RepresentantesAvalSeleccionados = null;
					this.vista.RepresentantesAvalTotales = null;
				}

				avalesSeleccionados.Add(bo);

				this.AgregarAvales(avalesSeleccionados);
				vista.ActualizarAvales();
			}
			catch (Exception ex)
			{
				throw new Exception(NombreClase + ".AgregarAval: " + ex.Message);
			}
		}
		private string ValidarCamposAval()
		{
			string s = "";

			if (this.vista.AvalSeleccionadoID == null)
				s += "Aval, ";

			if (s.Trim().CompareTo("") != 0)
				return "Los siguientes campos no pueden estar vacíos: \n" + s.Substring(0, s.Length - 2);

			if (this.vista.AvalesSeleccionados != null && this.vista.AvalesSeleccionados.Exists(p => p.Id == this.vista.AvalSeleccionadoID))
				return "El aval seleccionado ya ha sido agregado.";

			if (this.vista.AvalSeleccionadoID != null)
			{
				AvalBO bo = this.vista.AvalesTotales.Find(p => p.Id == this.vista.AvalSeleccionadoID);
				if (bo != null && bo.TipoAval != null && bo.TipoAval == ETipoAval.Moral)
					if (!(this.vista.RepresentantesAvalSeleccionados != null && this.vista.RepresentantesAvalSeleccionados.Count > 0))
						return "Es necesario seleccionar al menos un representante legal para el aval.";
			}

			return null;
		}
		/// <summary>
		/// Establece una lista de avales
		/// </summary>
		/// <param name="lst">Lista de avales a establecer</param>
		public void AgregarAvales(List<AvalBO> lst)
		{
			try
			{
				this.vista.AvalesSeleccionados = lst;

				this.vista.ActualizarAvales();
			}
			catch (Exception ex)
			{
				throw new Exception(NombreClase + ".AgregarAvales: " + ex.Message);
			}
		}
		/// <summary>
		/// Quita un aval de los seleccionados
		/// </summary>
		/// <param name="index">Índice del aval a quitar</param>
		public void QuitarAval(int index)
		{
			try
			{
				if (index >= this.vista.AvalesSeleccionados.Count || index < 0)
					throw new Exception("No se encontró el aval seleccionado.");

				List<AvalBO> avales = this.vista.AvalesSeleccionados;
				avales.RemoveAt(index);

				this.vista.AvalesSeleccionados = avales;
				this.vista.ActualizarAvales();
			}
			catch (Exception ex)
			{
				throw new Exception(NombreClase + ".QuitarAval: " + ex.Message);
			}
		}
		public void PrepararVisualizacionRepresentantesAval(int index)
		{
			try
			{
				if (index >= this.vista.AvalesSeleccionados.Count || index < 0)
					throw new Exception("No se encontró el Aval seleccionado.");

				AvalBO bo = this.vista.AvalesSeleccionados[index];
				if (bo is AvalMoralBO)
					this.vista.MostrarDetalleRepresentantesAval(((AvalMoralBO)bo).Representantes);
				else
					this.vista.MostrarDetalleRepresentantesAval(null);
			}
			catch (Exception ex)
			{
				throw new Exception(NombreClase + ".PrepararVisualizacionRepresentantesAval: " + ex.Message);
			}
		}
		public void AgregarRepresentanteAval()
		{
			if (this.vista.RepresentanteAvalSeleccionadoID == null)
			{
				this.vista.MostrarMensaje("Es necesario seleccionar un representante para el aval.", ETipoMensajeIU.INFORMACION, null);
				return;
			}

			try
			{
				if (this.vista.RepresentantesAvalSeleccionados == null)
					this.vista.RepresentantesAvalSeleccionados = new List<RepresentanteLegalBO>();

				List<RepresentanteLegalBO> seleccionados = this.vista.RepresentantesAvalSeleccionados;

				//Obtengo el representante de la lista de totales
				RepresentanteLegalBO bo = new RepresentanteLegalBO(this.vista.RepresentantesAvalTotales.Find(p => p.Id == this.vista.RepresentanteAvalSeleccionadoID));
				if (bo == null)
					throw new Exception("No se encontró el representante seleccionado.");

				seleccionados.Add(bo);

				this.vista.RepresentantesAvalSeleccionados = seleccionados;
			}
			catch (Exception ex)
			{
				throw new Exception(NombreClase + ".AgregarRepresentanteAval: " + ex.Message);
			}
		}

		public void QuitarRepresentanteAval()
		{
			try
			{
				//Obtengo el representante de la lista de totales
				if ((this.vista.RepresentantesAvalSeleccionados.Find(p => p.Id == this.vista.RepresentanteAvalSeleccionadoID)) == null)
					throw new Exception("No se encontró el representante seleccionado.");

				int index = this.vista.RepresentantesAvalSeleccionados.FindIndex(p => p.Id == this.vista.RepresentanteAvalSeleccionadoID);

				List<RepresentanteLegalBO> representantes = this.vista.RepresentantesAvalSeleccionados;
				representantes.RemoveAt(index);

				this.vista.RepresentantesAvalSeleccionados = representantes;
			}
			catch (Exception ex)
			{
				throw new Exception(NombreClase + ".QuitarRepresentanteAval: " + ex.Message);
			}
		}

		public AvalBO ObligadoAAval(ObligadoSolidarioBO obligado)
		{
			if (obligado == null) return null;

			AvalBO aval;

			switch (obligado.TipoObligado)
			{
				case ETipoObligadoSolidario.Fisico:
					aval = new AvalFisicoBO(obligado);
					break;
				case ETipoObligadoSolidario.Moral:
					aval = new AvalMoralBO(obligado);
					if (obligado is ObligadoSolidarioMoralBO && ((ObligadoSolidarioMoralBO)obligado).Representantes != null)
						((AvalMoralBO)aval).Representantes = new List<RepresentanteLegalBO>(((ObligadoSolidarioMoralBO)obligado).Representantes);
					break;
				default:
					aval = new AvalProxyBO(obligado);
					break;
			}

			return aval;
		}

        #endregion Metodos
    }
}
