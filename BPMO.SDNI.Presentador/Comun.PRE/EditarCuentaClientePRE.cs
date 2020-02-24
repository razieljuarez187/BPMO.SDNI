//Satisface al caso de uso CU068 - Catálogo de Clientes
using System;
using System.Collections.Generic;
using System.Linq;
using BPMO.Basicos.BO;
using BPMO.Facade.SDNI.BR;
using BPMO.Patterns.Creational.DataContext;
using BPMO.Primitivos.Enumeradores;
using BPMO.SDNI.Comun.BO;
using BPMO.SDNI.Comun.BR;
using BPMO.SDNI.Comun.VIS;

namespace BPMO.SDNI.Comun.PRE
{
	public class EditarCuentaClientePRE
	{
		#region Atributos

		private CuentaClienteIdealeaseBR clienteBR;
		private IDataContext dctx;
		private string nombreClase = "EditarCuentaClientePRE";
		private ucDatosObligadoSolidarioPRE presentadorObligado;
		private ucDatosRepresentanteLegalPRE presentadorRepresentante;

		private IEditarCuentaClienteVIS vista;
		private IucDatosObligadoSolidarioVIS vistaObligado;
		private IucDatosRepresentanteLegalVIS vistaRepresentante;

		#region SC0005

		private ucDatosRepresentanteLegalPRE presentadorRepresentantesObligado;

		#endregion SC0005

		#endregion Atributos

		#region Constructor

		public EditarCuentaClientePRE(IEditarCuentaClienteVIS vista, IucDatosObligadoSolidarioVIS vistaObligado, IucDatosRepresentanteLegalVIS vistaRepresentante, IucDatosRepresentanteLegalVIS vistaRepresentantesObligado)
		{
			try
			{
				this.vista = vista;
				this.vistaObligado = vistaObligado;
				this.vistaRepresentante = vistaRepresentante;
				this.presentadorObligado = new ucDatosObligadoSolidarioPRE(vistaObligado);
				this.presentadorRepresentante = new ucDatosRepresentanteLegalPRE(vistaRepresentante);
				presentadorRepresentantesObligado = new ucDatosRepresentanteLegalPRE(vistaRepresentantesObligado);
                if (this.vista.UnidadOperativa.Id == (int)ETipoEmpresa.Generacion
                || this.vista.UnidadOperativa.Id == (int)ETipoEmpresa.Construccion || this.vista.UnidadOperativa.Id == (int)ETipoEmpresa.Equinova)
                {
                    presentadorRepresentantesObligado.HabilitarCampos();
                }
				this.clienteBR = new CuentaClienteIdealeaseBR();
				this.dctx = FacadeBR.ObtenerConexion();
			}
			catch (Exception ex)
			{
				this.MostrarMensaje("Inconsistencias en la configuración", ETipoMensajeIU.ERROR, this.nombreClase + ".EditarClientePRE: " + ex.Message);
			}
		}

		#endregion Constructor

		#region Métodos

		#region SC0005

		public void AgregarRepresentanteLegalObligado()
		{
			string s;
			if (String.IsNullOrEmpty(s = presentadorRepresentantesObligado.ValidarCampos()))
			{
				List<RepresentanteLegalBO> representantes = new List<RepresentanteLegalBO>(vistaObligado.RepresentantesLegales);
				RepresentanteLegalBO representante = this.presentadorRepresentantesObligado.ObtenerRepresentanteLegal();
				representante.Auditoria = new AuditoriaBO
				{
					FC = this.vista.FC,
					UC = this.vista.UC,
					FUA = this.vista.FUA,
					UUA = this.vista.UUA
				};
				representante.Activo = true;
				representantes.Add(representante);
				vistaObligado.RepresentantesLegales = representantes;
				vistaObligado.ActualizarRepresentantesLegales();
				presentadorRepresentantesObligado.PrepararNuevo();
				MostrarEdicion();
				vista.MostrarMensaje("El representante legal se ha agregado correctamente", ETipoMensajeIU.EXITO);
			}
			else
			{
				vista.MostrarMensaje("Los siguientes datos del representante son requeridas " + s.Substring(2), ETipoMensajeIU.ADVERTENCIA);
			}
		}

		public void LimpiarRepresentanteObligado()
		{
			presentadorRepresentantesObligado.PrepararNuevo();
		}

		public void MostrarDetalleObligado(ObligadoSolidarioBO obligado)
		{
			vista.MostrarDetalleObligado(((ObligadoSolidarioMoralBO)obligado).Representantes);
		}

		public void MostrarEdicion()
		{
			vista.MostrarEdicion();
		}

		public void MostrarRepresentanteObligado()
		{
			vista.MostrarRepresentanteObligado();
		}

		#endregion SC0005

		public void ActualizarCliente()
		{
			string s;
			if (String.IsNullOrEmpty(s = ValidarDatos()))
			{
				try
				{
                    
					InterfazUsuarioADato();

                    #region SC0008
                    SeguridadBO seguridad = new SeguridadBO(Guid.Empty, new UsuarioBO() { Id = this.vista.UC },
                    new AdscripcionBO() { UnidadOperativa = new UnidadOperativaBO() { Id = this.vista.UnidadOperativa.Id } });

                    clienteBR.ActualizarCompleto(dctx, vista.Cliente, vista.ClienteAnterior, seguridad);
                    #endregion                    

                    CuentaClienteIdealeaseBO cliente = new CuentaClienteIdealeaseBO();
                    cliente.Cliente = new ClienteBO();
                    cliente.Cliente.Id = vista.Cliente.Cliente.Id;
                    cliente.Id = vista.Cliente.Id;

                    cliente.Cliente.Fisica = vista.Fisica;
                    cliente.Cliente.RFC = vista.RFC;
                    cliente.UnidadOperativa = new UnidadOperativaBO();
                    cliente.UnidadOperativa.Id = vista.Cliente.UnidadOperativa.Id;

				    cliente.Observaciones = vista.Observaciones;
				    cliente.SectorCliente = vista.SectorCliente;

                    vista.EstablecerPaquete(cliente);
					vista.LimpiarSesion();
					vista.RedirigirADetalle();
				}
				catch (Exception ex)
				{
					this.MostrarMensaje("Error al intentar actualizar el cliente", ETipoMensajeIU.ERROR, this.nombreClase + ".ActualizarCliente: " + ex.Message);
				}
			}
			else
				this.MostrarMensaje("Los siguientes campos no deben estar vacíos: " + s.Substring(2), ETipoMensajeIU.ADVERTENCIA);
		}

		public void ActualizarObligadoSolidario()
		{
			string s;
			try
			{
				if (String.IsNullOrEmpty(s = presentadorObligado.ValidarDatos()))
				{
					ObligadoSolidarioBO bo = presentadorObligado.ObtenerDatos();
					ActualizarDatosObligadoSolidario(bo.Id);
					presentadorObligado.PrepararNuevo();
					HabilitarAgregarObligado();
					presentadorObligado.ModoCreacion();
					ActualizarVistaObligadosSolidarios();
				}
				else
				{
					MostrarMensaje("Se requiere los siguientes campos del Obligado Solidario:" + s.Substring(2), ETipoMensajeIU.INFORMACION);
				}
			}
			catch (Exception ex)
			{
				this.MostrarMensaje("Error al Actualizar los datos de un Obligado Solidario", ETipoMensajeIU.ERROR, this.nombreClase + ".ActualizarRepresentanteLegal:" + ex.Message);
			}
		}

		public void ActualizarRepresentanteLegal()
		{
			string s;
			try
			{
				if (String.IsNullOrEmpty(s = this.presentadorRepresentante.ValidarCampos()))
				{
					RepresentanteLegalBO bo = this.presentadorRepresentante.ObtenerRepresentanteLegal();
					this.ActualizarDatosRepresentanteLegal(bo.Id);
					this.presentadorRepresentante.PrepararNuevo();
					this.HabilitarAgregarRepresentante();
					this.ActualizarVistaRepresentantesLegales();
				}
				else
				{
					this.MostrarMensaje("Se requiere los siguientes campos del Representante Legal: " + s.Substring(2), ETipoMensajeIU.INFORMACION);
				}
			}
			catch (Exception ex)
			{
				MostrarMensaje("Error al Actualizar los datos de un Representante Legal", ETipoMensajeIU.ERROR, this.nombreClase + ".ActualizarRepresentanteLegal:" + ex.Message);
			}
		}

		public void ActualizarVistaObligadosSolidarios()
		{
			this.vista.ActualizarObligadosSolidarios();
		}

		public void ActualizarVistaRepresentantesLegales()
		{
			this.vista.ActualizarRepresentantesLegales();
		}

		public void AgregarObligadosolidario()
		{
			string s;
			if (String.IsNullOrEmpty(s = this.presentadorObligado.ValidarDatos()))
			{
				List<ObligadoSolidarioBO> obligados = new List<ObligadoSolidarioBO>(this.vista.Obligados);
				ObligadoSolidarioBO obligado = presentadorObligado.ObtenerDatos();
				obligado.Auditoria = new AuditoriaBO
				{
					FC = vista.FC,
					UC = vista.UC,
					FUA = vista.FUA,
					UUA = vista.UUA
				};
				obligado.Activo = true;
				obligados.Add(obligado);
				vista.Obligados = obligados;
				presentadorObligado.PrepararNuevo();
				vista.ActualizarObligadosSolidarios();
			}
			else
			{
				this.vista.MostrarMensaje("Se requiere los siguientes datos del Obligado Solidario: " + s.Substring(2), ETipoMensajeIU.ADVERTENCIA);
			}
		}

		public void AgregarRepresentanteLegal()
		{
			string s;
			if (String.IsNullOrEmpty(s = this.presentadorRepresentante.ValidarCampos()))
			{
				List<RepresentanteLegalBO> representantes = new List<RepresentanteLegalBO>(this.vista.Representantes);
				RepresentanteLegalBO representante = this.presentadorRepresentante.ObtenerRepresentanteLegal();
				representante.Auditoria = new AuditoriaBO
				{
					FC = vista.FC,
					UC = vista.UC,
					FUA = vista.FUA,
					UUA = vista.UUA
				};
				representante.Activo = true;
				representantes.Add(representante);
				vista.Representantes = representantes;
				presentadorRepresentante.PrepararNuevo();
				vista.ActualizarRepresentantesLegales();
			}
			else
			{
				vista.MostrarMensaje("Los siguientes datos del representante no pueden estar vacíos " + s.Substring(2), ETipoMensajeIU.ADVERTENCIA);
			}
		}    

		public void Cancelar()
		{
			this.vista.LimpiarSesion();
			this.vista.RedirigirAConsulta();
		}

		public void CancelarObligadoSolidario()
		{
			this.presentadorObligado.PrepararNuevo();
			this.HabilitarAgregarObligado();
			this.ActualizarVistaObligadosSolidarios();
			presentadorObligado.ModoCreacion();
		}

		public void CancelarRepresentante()
		{
			this.presentadorRepresentante.PrepararNuevo();
			this.HabilitarAgregarRepresentante();
			this.ActualizarVistaRepresentantesLegales();
		}

		public void EditarObligadoSolidario(ObligadoSolidarioBO bo)
		{
			try
			{
				this.presentadorRepresentante.PrepararNuevo();
				this.HabilitarEdicionObligado();
				this.DatoAInterfazObligado(bo);
				presentadorObligado.ModoEdicion();
			}
			catch (Exception ex)
			{
				this.MostrarMensaje("Error al editar un Obligado Solidario", ETipoMensajeIU.ERROR, nombreClase + ".EditarObligadoSolidario" + ex.Message);
			}
		}

		public void EditarRepresentanteLegal(RepresentanteLegalBO bo)
		{
			try
			{
				this.presentadorObligado.PrepararNuevo();
				this.HabilitarEdicionRepresentante();
				this.DatoAInterfazRepresentante(bo);
			}
			catch (Exception ex)
			{
				this.MostrarMensaje("Error al editar un Representante", ETipoMensajeIU.ERROR, nombreClase + ".EditarObligadoSolidario" + ex.Message);
			}
		}

		public void Inicializar()
		{
			if (this.ValidarDatosNavegacion() == true)
			{
				this.PrepararNuevo();
				CuentaClienteIdealeaseBO cliente = (CuentaClienteIdealeaseBO)this.ObtenerDatosNavegacion();
				this.vista.Cliente = cliente;
				List<CuentaClienteIdealeaseBO> LastCliente = (List<CuentaClienteIdealeaseBO>)clienteBR.ConsultarCompleto(dctx, cliente);
				if (LastCliente.Count < 1)
					throw new Exception("No se encontró al cliente que desea editar");
				if (LastCliente.Count > 1)
					throw new Exception("Se encontró mas de un registro del cliente que desea editar");

				this.vista.ClienteAnterior = LastCliente[0];
				this.vista.Representantes = cliente.RepresentantesLegales.ConvertAll(r => (RepresentanteLegalBO)r);
				this.vista.Obligados = cliente.ObligadosSolidarios.ConvertAll(o => (ObligadoSolidarioBO)o);
                this.vista.ListaTelefonos = cliente.Telefonos.ConvertAll(t => (TelefonoClienteBO)t);
				this.PrepararNuevo();
				this.DatoAInterfazUsuarioCliente();
				this.ActualizarVistaObligadosSolidarios();
				this.ActualizarVistaRepresentantesLegales();
                this.EstablecerSeguridad();
			}
			else
			{
				this.MostrarMensaje("Se esperaba un cliente Idealease", ETipoMensajeIU.ADVERTENCIA);
			}
		}

		public object ObtenerDatosNavegacion()
		{
			return this.vista.ObtenerDatos();
		}			
		public void PrepararNuevo()
		{
		
            this.vista.ActaConstitutivaSeleccionada = null;
            this.vista.ActasConstitutivas = null;			
			vista.FechaRegistro = null;			
			vista.Fisica = null;
			vista.GiroEmpresa = null;			
			vista.Nombre = null;			
			vista.RFC = null;
			vista.TipoCuenta = null;
			vista.CURP = null;
		    this.vista.NumeroCuentaOracle = null;
			presentadorObligado.PrepararNuevo();
			presentadorRepresentante.PrepararNuevo();
			HabilitarAgregarObligado();
			HabilitarAgregarRepresentante();
            EstablecerSeguridad();
            EstablecerAcciones();
		}
		
		public void QuitarObligadoSolidario(ObligadoSolidarioBO obligado)
		{
			try
			{
				if (obligado != null)
				{
					if (obligado.Id != null)
					{
						ObligadoSolidarioBO bo = this.vista.Obligados.Find(o => o.Id == obligado.Id);
						if (bo != null)
						{
							List<ObligadoSolidarioBO> boActivos = this.vista.Obligados;
							List<ObligadoSolidarioBO> boInactivos = this.vista.ObligadosInactivos;
							boActivos.Remove(bo);
							bo.Activo = false;
							bo.Auditoria.FUA = this.vista.FUA;
							bo.Auditoria.UUA = this.vista.UUA;
							boInactivos.Add(bo);
							this.vista.Obligados = boActivos;
							this.vista.ObligadosInactivos = boInactivos;
							this.vista.ActualizarObligadosSolidarios();
						}
						else
							throw new Exception("El Obligado Solidario proporcionado no se encuentra en la lista");
					}
					else
					{
						if (this.vista.Obligados.Contains(obligado))
						{
							List<ObligadoSolidarioBO> obligados = new List<ObligadoSolidarioBO>(this.vista.Obligados);
							obligados.Remove(obligado);
							this.vista.Obligados = obligados;
							this.vista.ActualizarObligadosSolidarios();
						}
						else
							throw new Exception("El Obligado Solidario proporcionado no se encuentra en la lista");
					}
				}
				else
					throw new Exception("Se requiere un Obligado Solidario válido para la operación");
			}
			catch (Exception ex)
			{
				this.vista.MostrarMensaje("Inconsistencias al intentar quitar el Obligado Solidario de la lista", ETipoMensajeIU.ERROR, nombreClase + ".QuitarObligadoSolidario: " + ex.Message);
			}
		}

		public void QuitarRepresentanteLegal(RepresentanteLegalBO representante)
		{
			try
			{
				if (representante != null)
				{
					if (representante.Id != null)
					{
						RepresentanteLegalBO bo = this.vista.Representantes.Find(r => r.Id == representante.Id);
						if (bo != null)
						{
							List<RepresentanteLegalBO> boActivos = vista.Representantes;
							List<RepresentanteLegalBO> boInactivos = vista.RepresentantesInactivos;
							boActivos.Remove(bo);
							bo.Activo = false;
							bo.Auditoria.FUA = vista.FUA;
							bo.Auditoria.UUA = vista.UUA;
							boInactivos.Add(bo);
							vista.Representantes = boActivos;
							vista.RepresentantesInactivos = boInactivos;
							vista.ActualizarRepresentantesLegales();
						}
						else
							throw new Exception("El Representante Legal proporcionado no se encuentra en la lista");
					}
					else
					{
						if (this.vista.Representantes.Contains(representante))
						{
							List<RepresentanteLegalBO> representantes = new List<RepresentanteLegalBO>(this.vista.Representantes);
							representantes.Remove(representante);
							this.vista.Representantes = representantes;
							this.vista.ActualizarRepresentantesLegales();
						}
						else
							throw new Exception("El Representante Legal proporcionado no se encuentra en la lista");
					}
				}
				else
					throw new Exception("Se requiere un Representante Legal válido para la operación");
			}
			catch (Exception ex)
			{
				this.vista.MostrarMensaje("Inconsistencias al intentar quitar el Representante Legal de la lista", ETipoMensajeIU.ERROR, nombreClase + ".QuitarRepresentanteLegal: " + ex.Message);
			}
		}

		public bool ValidarDatosNavegacion()
		{
			if (ObtenerDatosNavegacion() == null)
				throw new Exception(nombreClase + ".ValidarDatosNavegación: se esperaba un objeto en la navegación. No se puede identificar qué cliente desea editar");
			if (this.ObtenerDatosNavegacion() is CuentaClienteIdealeaseBO)
				return true;
			return false;
		}

		private void ActualizarDatosObligadoSolidario(int? id)
		{
			ObligadoSolidarioBO obligadoSolidarioList = vista.Obligados.Find(o => o.Id == id);
			ObligadoSolidarioBO obligadoSolidarioTmp = presentadorObligado.ObtenerDatos();
			if (obligadoSolidarioList.TipoObligado == ETipoObligadoSolidario.Moral)
				((ObligadoSolidarioMoralBO)obligadoSolidarioList).Representantes =
					((ObligadoSolidarioMoralBO)obligadoSolidarioTmp).Representantes;

			obligadoSolidarioList.Nombre = obligadoSolidarioTmp.Nombre;
			obligadoSolidarioList.DireccionPersona.Calle = obligadoSolidarioTmp.DireccionPersona.Calle;
			obligadoSolidarioList.Telefono = obligadoSolidarioTmp.Telefono;
			obligadoSolidarioList.ActaConstitutiva = obligadoSolidarioTmp.ActaConstitutiva;
		    obligadoSolidarioList.RFC = obligadoSolidarioTmp.RFC;
		}

		private void ActualizarDatosRepresentanteLegal(int? id)
		{
			if (id == null)
				throw new Exception("Se esperaba un identificador del Representante Legal");
			RepresentanteLegalBO boTemp = this.vista.Representantes.Find(o => o.Id == id);
			if (boTemp == null)
				throw new Exception("No se encontró al Representante Legal en las lista");

			RepresentanteLegalBO boA = (RepresentanteLegalBO)this.presentadorRepresentante.ObtenerRepresentanteLegal();
			foreach (RepresentanteLegalBO r in this.vista.Representantes)
			{
				if (r.Id == boA.Id)
				{
					r.Nombre = boA.Nombre;
					r.DireccionPersona.Calle = boA.DireccionPersona.Calle;
					r.Telefono = boA.Telefono;
					r.EsDepositario = boA.EsDepositario;
					r.ActaConstitutiva.NumeroEscritura = boA.ActaConstitutiva.NumeroEscritura;
					r.ActaConstitutiva.FechaEscritura = boA.ActaConstitutiva.FechaEscritura;
					r.ActaConstitutiva.NombreNotario = boA.ActaConstitutiva.NombreNotario;
					r.ActaConstitutiva.NumeroNotaria = boA.ActaConstitutiva.NumeroNotaria;
					r.ActaConstitutiva.LocalidadNotaria = boA.ActaConstitutiva.LocalidadNotaria;
					r.ActaConstitutiva.NumeroRPPC = boA.ActaConstitutiva.NumeroRPPC;
					r.ActaConstitutiva.FechaRPPC = boA.ActaConstitutiva.FechaRPPC;
					r.ActaConstitutiva.LocalidadRPPC = boA.ActaConstitutiva.LocalidadRPPC;
				    r.RFC = boA.RFC;
				}
			}
		}

		private void DatoAInterfazObligado(ObligadoSolidarioBO obligado)
		{
			presentadorObligado.MostrarDatos(obligado);
		}

		private void DatoAInterfazRepresentante(RepresentanteLegalBO representante)
		{
			presentadorRepresentante.MostrarDatosRepresentanteLegal(representante);
		}
		
		private void DatoAInterfazUsuarioCliente()
		{			
			this.vista.FechaRegistro = this.vista.Cliente.FechaRegistroHacienda;			
			this.vista.Fisica = this.vista.Cliente.Cliente.Fisica;
			this.vista.GiroEmpresa = this.vista.Cliente.GiroEmpresa;			
			this.vista.Nombre = this.vista.Cliente.Nombre;			
            this.vista.ActasConstitutivas = this.vista.Cliente.ActasConstitutivas;
            this.vista.ActaConstitutivaSeleccionada = this.vista.Cliente.ActaConstitutiva;
            this.vista.ListaTelefonos = this.vista.Cliente.Telefonos;
			this.vista.RFC = this.vista.Cliente.Cliente.RFC;
			this.vista.TipoCuenta = this.vista.Cliente.TipoCuenta;
			this.vista.CURP = this.vista.Cliente.CURP;
            #region SC0001
            this.vista.DiasUsoUnidad = this.vista.Cliente.DiasUsoUnidad;
            this.vista.HorasUsoUnidad = this.vista.Cliente.HorasUsoUnidad;
            this.vista.Correo = this.vista.Cliente.Correo;
            #endregion
            this.vista.NumeroCuentaOracle = this.vista.Cliente.Numero;
			this.vista.ClienteID = this.vista.Cliente.Id;
            this.vista.SectorCliente = this.vista.Cliente.SectorCliente;
			if (this.vista.Cliente.Cliente.Fisica == true)
			{
				this.vista.MostrarHacienda();
				this.vista.OcultarActaConstitutiva();
			}
			if (this.vista.Cliente.Cliente.Fisica == false)
			{
				this.vista.OcultarHacienda();
				this.vista.MostrarActaConstitutiva();
			}

            this.vista.Observaciones = this.vista.Cliente.Observaciones;
		}
		
		private void HabilitarAgregarObligado()
		{
			this.vista.ModoAgregarObligado();
		}

		private void HabilitarAgregarRepresentante()
		{
			this.vista.ModoAgregarRepresentante();
		}

		private void HabilitarEdicionObligado()
		{
			this.vista.ModoEdicionObligado();
		}

		private void HabilitarEdicionRepresentante()
		{
			this.vista.ModoEdicionRepresentante();
		}
		
		private void InterfazUsuarioADato()
		{			
			vista.Cliente.FechaRegistroHacienda = vista.FechaRegistro;			
			vista.Cliente.Cliente.Fisica = vista.Fisica;
			vista.Cliente.GiroEmpresa = vista.GiroEmpresa;			
			vista.Cliente.Nombre = vista.Nombre;			
            this.vista.Cliente.QuitarActas();
            this.vista.Cliente.Agregar(this.vista.ActasConstitutivas);
            
			vista.Cliente.Cliente.RFC = vista.RFC;
			vista.Cliente.TipoCuenta = vista.TipoCuenta;
			vista.Cliente.CURP = vista.CURP;
            #region SC0001
            vista.Cliente.DiasUsoUnidad = vista.DiasUsoUnidad;
            vista.Cliente.HorasUsoUnidad = vista.HorasUsoUnidad;
            vista.Cliente.Correo = vista.Correo;
            #endregion
            vista.Cliente.Numero = vista.NumeroCuentaOracle;
			vista.Cliente.AuditoriaIdealease.FUA = vista.FUA;
			vista.Cliente.AuditoriaIdealease.UUA = vista.UUA;

			List<RepresentanteLegalBO> representantes = vista.Representantes;
			List<RepresentanteLegalBO> representantesInactivos = vista.RepresentantesInactivos;
			representantes.AddRange(representantesInactivos);

			vista.Cliente.RepresentantesLegales = representantes.ConvertAll(r => (PersonaBO)r);
			List<ObligadoSolidarioBO> obligados = vista.Obligados;
			List<ObligadoSolidarioBO> obligadosInactivos = vista.ObligadosInactivos;

            //Esta información solo aplica para generación o construcción
            if (vista.UnidadOperativa.Id != (int)ETipoEmpresa.Idealease)
            {
                vista.Cliente.SectorCliente = vista.SectorCliente;
                vista.Cliente.UnidadOperativaId = vista.UnidadOperativa.Id;
                vista.Cliente.Observaciones = this.vista.Observaciones;                
                vista.Cliente.Telefonos = this.vista.ListaTelefonos;
            }

			obligados.AddRange(obligadosInactivos);
			vista.Cliente.ObligadosSolidarios = obligados.ConvertAll(r => (PersonaBO)r);
		}
		
		private void MostrarMensaje(string mensaje, ETipoMensajeIU tipo, string detalle = null)
		{
			this.vista.MostrarMensaje(mensaje, tipo, detalle);
		}
		
		private string ValidarDatos()
		{
			string s = string.Empty;
			try
			{
				if (this.vista.Cliente.Id != null)
				{
					if (this.vista.Fisica != null && this.vista.Fisica == false)
					{
                        s += this.ValidarActaConstitutiva();                        
						if (this.vista.TipoCuenta == null)
							s += ", Tipo de cuenta";
						if (this.vista.Representantes == null || this.vista.Representantes.Count == 0)
							s += ", Representantes Legales";
						if (this.vista.Obligados == null || this.vista.Obligados.Count == 0)
							s += ", Obligados Solidarios";
					}
					else if (this.vista.Fisica != null && this.vista.Fisica == true)
					{
						if (this.vista.TipoCuenta == null)
							s += ", Tipo de cuenta";
						if (this.vista.FechaRegistro == null)
							s += ", Fecha de Registro";
						if (this.vista.GiroEmpresa == null)
							s += ", Giro de la Empresa";
						if (this.vista.CURP == null)
							s += ", CURP, ";

                        #region SC0001
                        //if (this.vista.DiasUsoUnidad == null)
                        //    s += ", DiasUsoUnidad, ";
                        //if (this.vista.HorasUsoUnidad == null)
                        //    s += ", HorasUsoUnidad, ";
                        //if (this.vista.Correo == null)
                        //    s += ", Correo, ";
                        #endregion
                        
                        
						if (vista.Fisica == false)
							if (this.vista.Representantes == null || vista.Representantes.Count == 0)
								s += ", Representantes Legales";
						if (this.vista.Obligados == null || this.vista.Obligados.Count == 0)
							s += ", Obligados Solidarios";
						if (this.vista.CURP != null && this.vista.CURP.Length != 18)
							s += ", El CURP debe ser de 18 caracteres";
					}
					else if (this.vista.Fisica == null)
					{
						throw new Exception("Cliente no válido");
					}
				}
				else
				{
					throw new Exception("Se requiere un Cliente");
				}
			}
			catch (Exception ex)
			{
				this.vista.MostrarMensaje("Inconsistencias al validar los datos", ETipoMensajeIU.ERROR, nombreClase + ".ValidarDatos: " + ex.Message);
			}

			return s;
		}
		
        #region SC_0008
        public void ValidarAcceso()
        {
            try
            {
                //se valida que los datos del usuario y la unidad operativa no sean nulos
                if (this.vista.UC == null) throw new Exception("El identificador del usuario no debe ser nulo");
                if (this.vista.UnidadOperativa == null) throw new Exception("La Unidad Operativa no debe ser nula ");
                if (this.vista.UnidadOperativa.Id == null) throw new Exception("El identificador de la Unidad Operativa no debe ser nulo ");

                //Se crea el objeto de seguridad
                UsuarioBO usuario = new UsuarioBO() { Id = this.vista.UC };
                AdscripcionBO adscripcion = new AdscripcionBO() { UnidadOperativa = new UnidadOperativaBO() { Id = this.vista.UnidadOperativa.Id } };
                SeguridadBO seguridadBO = new SeguridadBO(Guid.Empty, usuario, adscripcion);

                //Se valida si el usuario tiene permiso a la acción principal
                if (!FacadeBR.ExisteAccion(this.dctx, "ACTUALIZARCOMPLETO", seguridadBO))
                    this.vista.RedirigirSinPermisoAcceso();
            }
            catch (Exception ex)
            {
                throw new Exception(this.nombreClase + ".ValidarAcceso:" + ex.Message);
            }
        }

        /// <summary>
        /// Inicializa las acciones a realizar en la vista de los controles de captura
        /// </summary>
	    private void EstablecerAcciones()
	    {
            this.presentadorRepresentante.EstablecerAcciones(this.vista.ListaAcciones);
            this.vista.EstablecerAcciones();
	    }

        private void EstablecerSeguridad()
        {
            try
            {
                //se valida que los datos del usuario y la unidad operativa no sean nulos
                if (this.vista.UC == null) throw new Exception("El identificador del usuario no debe ser nulo");
                if (this.vista.UnidadOperativa == null) throw new Exception("La Unidad Operativa no debe ser nula ");
                if (this.vista.UnidadOperativa.Id == null) throw new Exception("El identificador de la Unidad Operativa no debe ser nulo ");

                //Se crea el objeto de seguridad
                UsuarioBO usuario = new UsuarioBO() { Id = this.vista.UC };
                AdscripcionBO adscripcion = new AdscripcionBO() { UnidadOperativa = new UnidadOperativaBO() { Id = this.vista.UnidadOperativa.Id } };
                SeguridadBO seguridadBO = new SeguridadBO(Guid.Empty, usuario, adscripcion);

                //Se obtienen las acciones a las cuales el usuario tiene permiso en este proceso
                #region Asignacion de la lista de acciones para la implementacion de las empresas de Construcción y Generación. 
                this.vista.ListaAcciones = FacadeBR.ConsultarAccion(this.dctx, seguridadBO);
                #endregion
                //Se valida si el usuario tiene permiso para registrar cuenta cliente
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

		#endregion Métodos

    
        /// <summary>
        /// Despliega información del acta constitutiva
        /// </summary>
        /// <param name="actaId">Identificador del acta a desplegar</param>
        public void MostrarActaConstitutiva(int actaId)
        {
            try
            {
                this.vista.MostrarActaConstitutiva();
                var actaEncontrada = this.vista.ActasConstitutivas.FirstOrDefault(a => a.Id == actaId);
                if (actaEncontrada != null)
                    this.vista.ActaConstitutivaSeleccionada = actaEncontrada;
                else
                    this.vista.MostrarMensaje("No se encontró el acta constitutiva buscada", ETipoMensajeIU.INFORMACION);
            }
            catch (Exception ex)
            {
                this.vista.MostrarMensaje("Inconsistencias al presentar la información", ETipoMensajeIU.ERROR, ex.Message);
            }
        }
        /// <summary>
        /// Agrega o actualiza información del acta constitutiva 
        /// </summary>
        /// <returns>Retorna TRUE si la operación se pudo realizar</returns>
        public bool AgregarActaConstitutiva()
        {
            bool result = false;
            try
            {
                string msj = this.vista.ValidarActaConstitutiva();
                if (msj != "NOAPLICA")
                {
                    if (string.IsNullOrWhiteSpace(msj))
                    {
                        List<ActaConstitutivaBO> listaActas = this.vista.ActasConstitutivas;
                        var actaConstitutiva = this.vista.ActaConstitutivaSeleccionada;
                        if (listaActas == null)
                            listaActas = new List<ActaConstitutivaBO>();
                        if (actaConstitutiva.Activo == true && ((actaConstitutiva.Id.HasValue && listaActas.Where(a => a.Id != actaConstitutiva.Id).Any(a => a.Activo == true))
                            || !actaConstitutiva.Id.HasValue && listaActas.Exists(a => a.Activo == true)))
                            this.vista.MostrarMensaje("Ya existe un acta constitutiva activa. Favor de verificar su captura", ETipoMensajeIU.INFORMACION);
                        else
                        {
                            if (actaConstitutiva.Id.HasValue)
                            {
                                var actaBO = listaActas.FirstOrDefault(a => a.Id == actaConstitutiva.Id);
                                if (actaBO == null)
                                    listaActas.Add(actaConstitutiva);
                                else
                                {
                                    actaBO.NumeroEscritura = actaConstitutiva.NumeroEscritura;
                                    actaBO.FechaEscritura = actaConstitutiva.FechaEscritura;
                                    actaBO.NombreNotario = actaConstitutiva.NombreNotario;
                                    actaBO.NumeroNotaria = actaConstitutiva.NumeroNotaria;
                                    actaBO.LocalidadNotaria = actaConstitutiva.LocalidadNotaria;
                                    actaBO.NumeroRPPC = actaConstitutiva.NumeroRPPC;
                                    actaBO.FechaRPPC = actaConstitutiva.FechaRPPC;
                                    actaBO.LocalidadRPPC = actaConstitutiva.LocalidadRPPC;
                                    actaBO.Activo = actaConstitutiva.Activo;
                                    result = true;
                                }
                            }
                            else
                            {
                                actaConstitutiva.Id = listaActas.Count > 0 ? listaActas.Max(a => a.Id) + 1 : 1;
                                listaActas.Add(actaConstitutiva);
                                result = true;
                            }
                            this.vista.ActasConstitutivas = listaActas;
                        }
                    }
                    else
                        this.vista.MostrarMensaje("Los siguientes datos no pueden estar vacíos " + msj.Substring(2), ETipoMensajeIU.ADVERTENCIA);
                }
            }
            catch (Exception ex)
            {
                this.vista.MostrarMensaje("Inconsistencias al mostrar la información", ETipoMensajeIU.ERROR, ex.Message);
            }
            return result;
        }
        /// <summary>
        /// Valida que se hayan capturado actas constitutivas
        /// </summary>
        /// <returns></returns>
        private string ValidarActaConstitutiva()
        {
            ETipoEmpresa empresa = (ETipoEmpresa)this.vista.UnidadOperativa.Id;
            if (empresa == ETipoEmpresa.Idealease)
            {


                return this.vista.ActasConstitutivas == null || this.vista.ActasConstitutivas.Count == 0
                                                             || !this.vista.ActasConstitutivas.Exists(a =>
                                                                 a.Activo == true)
                    ? ", No se ha capturado un acta constitutiva activa para la cuenta actual"
                    : string.Empty;
            }
            else return string.Empty;
        }

	}
}