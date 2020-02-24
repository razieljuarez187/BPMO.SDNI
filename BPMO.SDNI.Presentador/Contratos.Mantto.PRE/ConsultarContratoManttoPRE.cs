//Satisface al caso de uso CU029 - Consultar Contrato de Mantenimiento.
using System;
using System.Collections.Generic;
using System.Linq;
using BPMO.Basicos.BO;
using BPMO.Facade.SDNI.BOF;
using BPMO.Facade.SDNI.BR;
using BPMO.Patterns.Creational.DataContext;
using BPMO.Primitivos.Enumeradores;
using BPMO.SDNI.Comun.BO;
using BPMO.SDNI.Comun.BOF;
using BPMO.SDNI.Contratos.BO;
using BPMO.SDNI.Contratos.BR;
using BPMO.SDNI.Contratos.Mantto.BO;
using BPMO.SDNI.Contratos.Mantto.BOF;
using BPMO.SDNI.Contratos.Mantto.BR;
using BPMO.SDNI.Contratos.Mantto.VIS;
using BPMO.SDNI.Equipos.BOF;

namespace BPMO.SDNI.Contratos.Mantto.PRE
{
	public class ConsultarContratoManttoPRE
	{
		#region Atributos

		private readonly IConsultarContratoManttoVIS vista;

		/// <summary>
		/// Controlador de Contratos Mantto
		/// </summary>
		private readonly ContratoManttoBR controlador;

		/// <summary>
		/// El DataContext que proveerá acceso a la base de datos
		/// </summary>
		private readonly IDataContext dctx;

		/// <summary>
		/// Nombre de la clase para agregar a los mensajes de Error
		/// </summary>
		private string nombreClase = "ConsultarContratosManttoPRE";

		/// <summary>
		/// Presentador del Control de Herramientas
		/// </summary>
		private readonly ucHerramientasManttoPRE presentadorHerramientas;

		#endregion

		#region Constructores
		public ConsultarContratoManttoPRE(IConsultarContratoManttoVIS view, IucHerramientasManttoVIS viewHerramientas)
		{
			try
			{
				this.vista = view;
				this.presentadorHerramientas = new ucHerramientasManttoPRE(viewHerramientas);
				dctx = FacadeBR.ObtenerConexion();

				this.controlador = new ContratoManttoBR();
			}
			catch (Exception ex)
			{
				vista.MostrarMensaje("Inconsistencias en la construcción del presentador", ETipoMensajeIU.ERROR, nombreClase + ".ConsultarContratoManttoPRE: " + ex.Message);
			}
		}
		#endregion

		#region Métodos
		public void PrepararBusqueda()
		{
			this.vista.LimpiarSesion();

			this.EstablecerInformacionInicial();

			this.presentadorHerramientas.vista.OcultarCerrarContrato();
			this.presentadorHerramientas.vista.OcultarEditarContrato();
			this.presentadorHerramientas.vista.OcultarEliminarContrato();
			this.presentadorHerramientas.vista.OcultarEstatusContrato();
			this.presentadorHerramientas.vista.OcultarMenuImpresion();
			this.presentadorHerramientas.vista.OcultarNoContrato();

			this.EstablecerFiltros();

			this.EstablecerSeguridad();

            this.CargarSucursalesAutorizadas();//SC_0051
		}
        /// <summary>
        /// Recupera las sucursales a las que el usuario autenticado tiene permiso de acceder 
        /// </summary>
        private void CargarSucursalesAutorizadas()
        {
            if (vista.SucursalID != null)
                return;

            if (this.vista.SucursalesAutorizadas != null)
                if (this.vista.SucursalesAutorizadas.Count > 0)
                    return;

            var lstSucursales = Facade.SDNI.BR.FacadeBR.ConsultarSucursalesSeguridad(dctx,
                                                                                        new SeguridadBO(Guid.Empty, new UsuarioBO{Id =  this.vista.UsuarioID },
                                                                                                        new AdscripcionBO
                                                                                                        {
                                                                                                            UnidadOperativa
                                                                                                                = new UnidadOperativaBO{Id = this.vista.UnidadOperativaID}}));
            this.vista.SucursalesAutorizadas = lstSucursales.ConvertAll(x => (object)x); //SC00
        }

		private void EstablecerInformacionInicial()
		{
			#region Estatus del Contrato
			Dictionary<int, string> lstEstatus = new Dictionary<int, string>();

            var tipo = EEstatusContrato.Borrador;
            string texto = (tipo.GetType().GetField(tipo.ToString()).GetCustomAttributes(true)
                                 .Where(a => a.GetType().Equals(typeof(System.ComponentModel.DescriptionAttribute)))
                                 .FirstOrDefault() as System.ComponentModel.DescriptionAttribute).Description;
            lstEstatus.Add((int)EEstatusContrato.Borrador, texto);

            tipo = EEstatusContrato.EnCurso;
            texto = (tipo.GetType().GetField(tipo.ToString()).GetCustomAttributes(true)
                                 .Where(a => a.GetType().Equals(typeof(System.ComponentModel.DescriptionAttribute)))
                                 .FirstOrDefault() as System.ComponentModel.DescriptionAttribute).Description;
			lstEstatus.Add((int)EEstatusContrato.EnCurso, texto);

            tipo = EEstatusContrato.Cerrado;
            texto = (tipo.GetType().GetField(tipo.ToString()).GetCustomAttributes(true)
                                 .Where(a => a.GetType().Equals(typeof(System.ComponentModel.DescriptionAttribute)))
                                 .FirstOrDefault() as System.ComponentModel.DescriptionAttribute).Description;
			lstEstatus.Add((int)EEstatusContrato.Cerrado, texto);

            tipo = EEstatusContrato.Cancelado;
            texto = (tipo.GetType().GetField(tipo.ToString()).GetCustomAttributes(true)
                                 .Where(a => a.GetType().Equals(typeof(System.ComponentModel.DescriptionAttribute)))
                                 .FirstOrDefault() as System.ComponentModel.DescriptionAttribute).Description;
			lstEstatus.Add((int)EEstatusContrato.Cancelado, texto);

			this.vista.EstablecerOpcionesEstatus(lstEstatus);
			#endregion

			this.vista.CuentaClienteID = null;
			this.vista.CuentaClienteNombre = null;
			this.vista.EstatusID = null;
			this.vista.FechaContratoFinal = null;
			this.vista.FechaContratoInicial = null;
			this.vista.NumeroContrato = null;
			this.vista.NumeroEconomico = null;
			this.vista.NumeroSerie = null;
			this.vista.SucursalID = null;
			this.vista.SucursalNombre = null;
			this.vista.TipoContratoID = null;
		}

		private void EstablecerFiltros()
		{
			try
			{
				Dictionary<string, object> paquete = this.vista.ObtenerPaqueteNavegacion("FiltrosContratoMantto") as Dictionary<string, object>;
				if (paquete != null)
				{
					if (paquete.ContainsKey("ObjetoFiltro"))
					{
						if (paquete["ObjetoFiltro"].GetType() == typeof(ContratoManttoBOF))
							this.DatoAInterfazUsuario(paquete["ObjetoFiltro"]);
						else
							throw new Exception("Se esperaba un objeto ContratoManttoBOF, el objeto proporcionado no cumple con esta característica, intente de nuevo por favor.");
					}
					if (paquete.ContainsKey("Bandera"))
					{
						if ((bool)paquete["Bandera"])
							this.ConsultarContratos();
					}
				}
				this.vista.LimpiarPaqueteNavegacion("FiltrosContratoMantto");
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
				if (!this.ExisteAccion(lst, "UI INSERTAR"))
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
			ContratoManttoBOF bo = new ContratoManttoBOF();
			bo.Sucursal = new SucursalBO();
			bo.Sucursal.UnidadOperativa = new UnidadOperativaBO();
			bo.Cliente = new CuentaClienteIdealeaseBO();

			bo.NumeroContrato = this.vista.NumeroContrato;
			bo.NumeroEconomico = this.vista.NumeroEconomico;
			bo.NumeroSerie = this.vista.NumeroSerie;
			bo.FechaContrato = this.vista.FechaContratoInicial;
			bo.FechaContratoFinal = this.vista.FechaContratoFinal;
			bo.Sucursal.UnidadOperativa.Id = this.vista.UnidadOperativaID;
			bo.Cliente.Id = this.vista.CuentaClienteID;
			bo.Cliente.Nombre = this.vista.CuentaClienteNombre;

			if (this.vista.EstatusID != null)
				bo.Estatus = (EEstatusContrato)Enum.Parse(typeof(EEstatusContrato), this.vista.EstatusID.ToString());
			if (this.vista.TipoContratoID != null)
				bo.Tipo = (ETipoContrato)Enum.Parse(typeof(ETipoContrato), this.vista.TipoContratoID.ToString());

		    if (vista.SucursalID != null)
		    {
		        bo.Sucursal.Id = this.vista.SucursalID;
		        bo.Sucursal.Nombre = this.vista.SucursalNombre;
		    }
		    else
		        bo.SucursalesConsulta = this.vista.SucursalesAutorizadas.ConvertAll(x => (SucursalBO)x);

			return bo;
		}
		private void DatoAInterfazUsuario(Object obj)
		{
			ContratoManttoBOF bo = (ContratoManttoBOF)obj;

			if (!String.IsNullOrEmpty(bo.NumeroContrato) && !String.IsNullOrWhiteSpace(bo.NumeroContrato))
				this.vista.NumeroContrato = bo.NumeroContrato;
			else
				this.vista.NumeroContrato = null;
			if (!String.IsNullOrEmpty(bo.NumeroEconomico) && !String.IsNullOrWhiteSpace(bo.NumeroEconomico))
				this.vista.NumeroEconomico = bo.NumeroEconomico;
			else
				this.vista.NumeroEconomico = null;
			if (!String.IsNullOrEmpty(bo.NumeroSerie) && !String.IsNullOrWhiteSpace(bo.NumeroSerie))
				this.vista.NumeroSerie = bo.NumeroSerie;
			else
				this.vista.NumeroSerie = null;

			if (bo.Cliente != null && !String.IsNullOrEmpty(bo.Cliente.Nombre) && !String.IsNullOrWhiteSpace(bo.Cliente.Nombre))
				this.vista.CuentaClienteNombre = bo.Cliente.Nombre;
			else
				this.vista.CuentaClienteNombre = null;

			if (bo.Cliente != null && bo.Cliente.Id != null)
				this.vista.CuentaClienteID = bo.Cliente.Id;
			else
				this.vista.CuentaClienteID = null;

			if (bo.Sucursal != null && bo.Sucursal.Id != null)
				this.vista.SucursalID = bo.Sucursal.Id;
			else
				this.vista.SucursalID = null;

			if (bo.Sucursal != null && bo.Sucursal.Nombre != null)
				this.vista.SucursalNombre = bo.Sucursal.Nombre;
			else
				this.vista.SucursalNombre = null;

			if (bo.Tipo != null)
				this.vista.TipoContratoID = (int)bo.Tipo;
			else
				this.vista.TipoContratoID = null;

			if (bo.Estatus != null)
				this.vista.EstatusID = (int)bo.Estatus;
			else
				this.vista.EstatusID = null;

			if (bo.FechaContrato != null)
				this.vista.FechaContratoInicial = bo.FechaContrato;
			else
				this.vista.FechaContratoInicial = null;

			if (bo.FechaContratoFinal != null)
				this.vista.FechaContratoFinal = bo.FechaContratoFinal;
			else
				this.vista.FechaContratoFinal = null;
		}

		public void ConsultarContratos()
		{
			try
			{
				string s;
				if ((s = this.ValidarCampos()) != null)
				{
					this.vista.MostrarMensaje(s, ETipoMensajeIU.ADVERTENCIA, null);
					return;
				}

				ContratoManttoBOF bo = (ContratoManttoBOF)this.InterfazUsuarioADato();
				List<ContratoManttoBOF> lst = this.controlador.Consultar(this.dctx, bo);

				if (lst.Count < 1)
					this.vista.MostrarMensaje("La búsqueda no produjo resultados", ETipoMensajeIU.INFORMACION,
						"No se encontraron coincidencias con los criterios ingresados. Verifique sus datos");

				this.vista.EstablecerResultado(lst);
			}
			catch (Exception ex)
			{
				throw new Exception(this.nombreClase + ".ConsultarContratos: " + ex.Message);
			}
		}
		/// <summary>
		/// Despliega en la vista las plantillas correspondientes al módulo
		/// </summary>
		public void CargarPlantillas()
		{
			var controlador = new PlantillaBR();

			var precargados = this.vista.ObtenerPlantillas("ucContratosMantenimiento");
			var resultado = new List<object>();

			if (precargados != null)
				if (precargados.Count > 0)
					resultado = precargados;

			if (resultado.Count <= 0)
			{
				PlantillaBO plantilla = new PlantillaBO();
				plantilla.Activo = true;
				if (this.vista.TipoContratoID != null)
				{
					if ((ETipoContrato)Enum.Parse(typeof(ETipoContrato), this.vista.TipoContratoID.ToString()) == ETipoContrato.CM)
						plantilla.TipoPlantilla = EModulo.CM;
					if ((ETipoContrato)Enum.Parse(typeof(ETipoContrato), this.vista.TipoContratoID.ToString()) == ETipoContrato.SD)
						plantilla.TipoPlantilla = EModulo.SD;
				}

				var lista = controlador.Consultar(this.dctx, plantilla);

				if (ReferenceEquals(lista, null))
					lista = new List<PlantillaBO>();

				resultado = lista.ConvertAll(p => (object)p);
			}

			this.vista.CargarArchivos(resultado);
		}

		private string ValidarCampos()
		{
			string s = "";

			if (this.vista.UnidadOperativaID == null)
				s += "UnidadOperativaID, ";

			if (s != null && s.Trim().CompareTo("") != 0)
				return "Los siguientes campos no pueden estar vacíos: \n" + s.Substring(0, s.Length - 2);

			if (this.vista.FechaContratoInicial != null && this.vista.FechaContratoFinal != null && this.vista.FechaContratoFinal < this.vista.FechaContratoInicial)
				return "La fecha y hora inicial no puede ser mayor que la fecha y hora final.";

			return null;
		}

		public void IrADetalle(int? contratoID)
		{
			try
			{
				if (contratoID == null)
					throw new Exception("No se encontró el registro seleccionado.");

				ContratoManttoBO bo = new ContratoManttoBO { ContratoID = contratoID };

				this.vista.LimpiarSesion();

				Dictionary<string, object> paquete = new Dictionary<string, object>();
				paquete.Add("ObjetoFiltro", this.InterfazUsuarioADato());
				paquete.Add("Bandera", true);

				this.vista.EstablecerPaqueteNavegacion("FiltrosContratoMantto", paquete);
				this.vista.EstablecerPaqueteNavegacion("ContratoManttoBO", bo);

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
					CuentaClienteIdealeaseBOF cliente = new CuentaClienteIdealeaseBOF();
					cliente.UnidadOperativa = new UnidadOperativaBO();
					cliente.Cliente = new ClienteBO();

					cliente.Id = this.vista.CuentaClienteID;
					cliente.Nombre = this.vista.CuentaClienteNombre;
					cliente.UnidadOperativa.Id = this.vista.UnidadOperativaID;
			        cliente.Activo = true;

					obj = cliente;
					break;
				case "Sucursal":
					SucursalBOF sucursal = new SucursalBOF();
					sucursal.UnidadOperativa = new UnidadOperativaBO();
					sucursal.Usuario = new UsuarioBO();

					sucursal.UnidadOperativa.Id = this.vista.UnidadOperativaID;
					sucursal.Nombre = this.vista.SucursalNombre;
					sucursal.Usuario.Id = this.vista.UsuarioID;

					obj = sucursal;
					break;
				case "UnidadIdealease":
					UnidadBOF unidad = new UnidadBOF();

					if (!string.IsNullOrEmpty(this.vista.NumeroSerie))
						unidad.NumeroSerie = this.vista.NumeroSerie;

					obj = unidad;
					break;
			}

			return obj;
		}
		public void DesplegarResultadoBuscador(string catalogo, object selecto)
		{
			switch (catalogo)
			{
				case "CuentaClienteIdealease":
					CuentaClienteIdealeaseBOF cliente = (CuentaClienteIdealeaseBOF)selecto ?? new CuentaClienteIdealeaseBOF();

					vista.CuentaClienteID = cliente.Id;
					vista.CuentaClienteNombre = !string.IsNullOrEmpty(cliente.Nombre) ? cliente.Nombre : string.Empty;
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
				case "UnidadIdealease":
					UnidadBOF unidad = (UnidadBOF)selecto ?? new UnidadBOF();
					if (unidad.NumeroSerie != null)
						this.vista.NumeroSerie = unidad.NumeroSerie;
					else
						this.vista.NumeroSerie = string.Empty;
					break;
			}
		}
		#endregion
		#endregion
	}
}
