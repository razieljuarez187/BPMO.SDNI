// Satisface al caso de uso CU026 - Registrar Terminación de Contrato Full Service Leasing
// Satisface a la solicitud de cambio SC0010
// BEP1401 Satisface a la SC0026

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using BPMO.Basicos.BO;
using BPMO.Facade.SDNI.BR;
using BPMO.Patterns.Creational.DataContext;
using BPMO.Primitivos.Enumeradores;
using BPMO.SDNI.Comun.BO;
using BPMO.SDNI.Contratos.BO;
using BPMO.SDNI.Contratos.FSL.BO;
using BPMO.SDNI.Contratos.FSL.BR;
using BPMO.SDNI.Contratos.FSL.VIS;
using BPMO.SDNI.Facturacion.AplicacionesPago.BR;
using BPMO.SDNI.Facturacion.InfraestructuraPagos.BO;
using BPMO.SDNI.Facturacion.InfraestructuraPagos.BOF;
using BPMO.SDNI.Facturacion.InfraestructuraPagos.BR;

namespace BPMO.SDNI.Contratos.FSL.PRE {

	public class CerrarContratoFSLPRE {
		#region Atributos

		/// <summary>
		/// Nombre de la clase para agregar a los mensajes de Error
		/// </summary>
		private const string nombreClase = "CerrarContratoFSLPRE";

		/// <summary>
		/// Presentador de la Información del Cliente del Contrato
		/// </summary>
		private readonly ucClienteContratoPRE clienteContratoPRE;

		/// <summary>
		/// El DataContext que proveerá acceso a la base de datos
		/// </summary>
		private readonly IDataContext dataContext;

		/// <summary>
		/// Presentador de los Datos de la Renta
		/// </summary>
		private readonly ucDatosRentaPRE datosRentaPRE;

		private readonly ucFinalizacionContratoFSLPRE finalizacionPRE;

		/// <summary>
		/// Presentador de la Barra de Herramientas
		/// </summary>
		private readonly ucHerramientasFSLPRE herramientasPRE;

		/// <summary>
		/// Presentador de la Información General
		/// </summary>
		private readonly ucInformacionGeneralPRE informacionGeneralPRE;

		/// <summary>
		/// Presentador de la Información de Pago
		/// </summary>
		private readonly ucInformacionPagoPRE informacionPagoPRE;

		/// <summary>
		/// Presentador de la Línea de Contrato
		/// </summary>
		private readonly ucLineaContratoFSLPRE lineaContratoPRE;

		/// <summary>
		/// Vista sobre la que actual la interfaz
		/// </summary>
		private readonly ICerrarContratoFSLVIS vista;

		#endregion Atributos

		#region Constructores

		/// <summary>
		/// Constructor que recibe la vista sobre la que actuará el presentador
		/// </summary>
		/// <param name="vistaActual">vista sobre la que actuará el presentador</param>
		/// <param name="herramientas">Presentador de la barra de herramientas</param>
		/// <param name="general">Presentador de la Información General</param>
		/// <param name="cliente">Presentador de los datos del Cliente</param>
		/// <param name="datosRenta">Presentador de los datos de Renta</param>
		/// <param name="pago">Presentador de la información de Pago</param>
		/// <param name="lineaContrato">Presentador de las líneas de contrato</param>
		/// <param name="finalizacionContrato">Presentador de los datos de cierre </param>
		public CerrarContratoFSLPRE(ICerrarContratoFSLVIS vistaActual, ucHerramientasFSLPRE herramientas,
									ucInformacionGeneralPRE general, ucClienteContratoPRE cliente,
									ucDatosRentaPRE datosRenta, ucInformacionPagoPRE pago,
									ucLineaContratoFSLPRE lineaContrato, ucFinalizacionContratoFSLPRE finalizacionContrato) {
			if (vistaActual != null)
				vista = vistaActual;

			dataContext = FacadeBR.ObtenerConexion();

			try {
				vista = vistaActual;
				informacionGeneralPRE = general;
				clienteContratoPRE = cliente;
				datosRentaPRE = datosRenta;
				lineaContratoPRE = lineaContrato;
				informacionPagoPRE = pago;
				herramientasPRE = herramientas;
				finalizacionPRE = finalizacionContrato;
			} catch (Exception ex) {
				throw new Exception(nombreClase + ".CerrarContratoFSLPRE: " + ex.Message);
			}
		}

		#endregion Constructores

		#region Métodos
		/// <summary>
		/// Actualiza el cierre de un contrato Full Service Leasing
		/// </summary>
		public void CerrarContrato() {
            Guid firma = Guid.NewGuid();

            try
            {
                string resultado = finalizacionPRE.ValidarDatosCierre();
                if (!string.IsNullOrEmpty(resultado))
                {
                    vista.MostrarMensaje(resultado, ETipoMensajeIU.INFORMACION);
                    return;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(nombreClase + ".CerrarContrato:" + ex.Message);
            }
		    try
		    {
		        #region Transaccion

		        dataContext.SetCurrentProvider("Outsourcing");
		        dataContext.OpenConnection(firma);
		        dataContext.BeginTransaction(firma);

		        #endregion

		        ContratoFSLBO anterior = vista.UltimoObjeto;
		        ContratoFSLBO actual = InterfazUsuarioADatos();
		        actual.FC = anterior.FC;
		        actual.UC = anterior.UC;
		        actual.Estatus = EEstatusContrato.EnCurso;
		        var contratoBR = new ContratoFSLBR();

		        var seguridadBO = new SeguridadBO(Guid.Empty, new UsuarioBO {Id = vista.UsuarioID}, new AdscripcionBO
		        {
		            Departamento = new DepartamentoBO(),
		            Sucursal = new SucursalBO(),
		            UnidadOperativa = new UnidadOperativaBO {Id = vista.UnidadAdscripcionID}
		        });

		        // Determinar si es Cierre anticipado
		        bool cierreAnticipado = actual.CierreAnticipado();

		        // Terminar el Contrato
		        contratoBR.Terminar(dataContext, actual, anterior, seguridadBO, cierreAnticipado);


		        if (cierreAnticipado)
		        {
		            // Cancelar los pagos restantes
		            var pagosFaltantes = ObtenerPagos(anterior, (DateTime) actual.CierreContrato.Fecha);
		            var pagoBR = new PagoUnidadContratoBR();
		            foreach (var faltante in pagosFaltantes)
		            {
                        //SC0026, Generación de clase concreta segun el tipo de contrato
		                var cancelado = new PagoUnidadContratoFSLBO(faltante) {Activo = false};
		                cancelado.Auditoria.FUA = vista.FUA;
		                cancelado.Auditoria.UUA = vista.UUA;
		                pagoBR.Actualizar(dataContext, cancelado, faltante, seguridadBO);
		            }
		        }


		        var generadorPagoBR = new GeneradorPagosFSLBR();

		        // Calcular Ultimo Pago
		        var pago = UltimoPago(anterior.ContratoID);
		        var nuevoPago = ++pago;

		        // Generar los pagos adicionales
		        generadorPagoBR.GenerarPagoAdicional(dataContext, actual, nuevoPago, seguridadBO, true,true);

		        dataContext.CommitTransaction(firma);

		        RegresarADetalles();
		    }
		    catch (Exception ex)
		    {
		        dataContext.RollbackTransaction(firma);
		        throw new Exception(nombreClase + ".CerrarContrato:" + ex.Message);
		    }
		    finally
		    {
                if(dataContext.ConnectionState == ConnectionState.Open)
		            dataContext.CloseConnection(firma);
		    }
		}

        private List<PagoUnidadContratoFSLBO> ObtenerPagos(ContratoFSLBO contrato, DateTime FechaCierre)
	    {
            var pagoBR = new PagoUnidadContratoBR();

            var pago = new PagoUnidadContratoBOF
            {
                ReferenciaContrato = new ReferenciaContratoBO { ReferenciaContratoID = contrato.ContratoID, UnidadOperativa = new UnidadOperativaBO{ Id = vista.UnidadOperativaContratoID}},
                Facturado = false,
                FechaVencimientoInicial = FechaCierre, Sucursales = new List<SucursalBO>{ contrato.Sucursal},
            };

            var pagos = pagoBR.ConsultarFiltroSinCuentas(dataContext, pago, true);

	        return pagos.Cast<PagoUnidadContratoFSLBO>().ToList();
	    } 

	    private short UltimoPago(int? contratoId)
	    {
	        var pagoBR = new PagoUnidadContratoBR();
            
            //SC0026, Utilización de clase concreta segun el tipo de contrato
	        var pago = new PagoUnidadContratoBOF
	        {
	            ReferenciaContrato = new ReferenciaContratoBO {ReferenciaContratoID = contratoId}
	        };

	        var pagos = pagoBR.Consultar(dataContext, pago);

	        var max = pagos.Max(p => p.NumeroPago);
	        return (max ?? 0);
	    }

		/// <summary>
		/// Despliega los datos del contrato en la interfaz de usuario
		/// </summary>
		/// <param name="contrato">Contrato que contiene los datos</param>
		public void DatosAInterfazUsuario(ContratoFSLBO contrato) {
			try {
				vista.ContratoID = contrato.ContratoID;
				vista.Estatus = contrato.Estatus;
				if (contrato.Sucursal != null && contrato.Sucursal.UnidadOperativa != null)
					vista.UnidadOperativaContratoID = contrato.Sucursal.UnidadOperativa.Id;
				herramientasPRE.DatosAInterfazUsuario(contrato);
				informacionGeneralPRE.DatosAInterfazUsuario(contrato);
				clienteContratoPRE.DatosAInterfazUsuario(contrato);
				datosRentaPRE.DatosAInterfazUsuario(contrato);
				informacionPagoPRE.DatosAInterfazUsuario(contrato);
				finalizacionPRE.Inicializar(contrato);
			} catch (Exception ex) {
				throw new Exception(nombreClase + ".DatosAInterfazUsuario:" + ex.Message);
			}
		}

		/// <summary>
		/// Inicializa la vista para Cerrar un contrato
		/// </summary>
		public void Inicializar() {
			try {
				informacionGeneralPRE.Inicializar();
				informacionPagoPRE.Inicializar();
				clienteContratoPRE.Inicializar();
				datosRentaPRE.Inicializar();
				herramientasPRE.Inicializar();

				vista.CodigoUltimoObjeto = "UltimoContratoFSLBO";
				ValidarEstadoContrato();
				ContratoFSLBO contrato = vista.UltimoObjeto;
				DatosAInterfazUsuario(contrato);
				informacionGeneralPRE.Vista.ConfigurarModoConsultar();
				informacionPagoPRE.Vista.ConfigurarModoConsultar();
				clienteContratoPRE.Vista.ConfigurarModoConsultar();
				datosRentaPRE.Vista.ConfigurarModoConsultar();
				lineaContratoPRE.Vista.ConfigurarModoConsultar();
				finalizacionPRE.EstablecerModoEdicion(true);
				clienteContratoPRE.Vista.HabilitarConsultaDireccionCliente(false);
				herramientasPRE.vista.HabilitarOpciones(false);
				herramientasPRE.vista.OcultarFormatosContrato();
                herramientasPRE.vista.OcultarPlantillas();
				
				EstablecerSeguridad();
			} catch (Exception ex) {
				throw new Exception(nombreClase + ".Inicializar: " + ex.Message);
			}
		}

		/// <summary>
		/// Obtiene un Contrato a partir de los datos de la vista
		/// </summary>
		/// <returns></returns>
		public ContratoFSLBO InterfazUsuarioADatos() {
			try {
				var contrato = new ContratoFSLBO
				{
					ContratoID = vista.ContratoID,

					#region Cuenta Bancaria

					//Banco = informacionPagoPRE.Vista.CuentaBancariaSeleccionada,

					#endregion Cuenta Bancaria

					Cliente = new CuentaClienteIdealeaseBO
					{
						Id = clienteContratoPRE.Vista.CuentaClienteID,
						Cliente = new ClienteBO { Id = clienteContratoPRE.Vista.ClienteID, Fisica = clienteContratoPRE.Vista.EsFisico },
						UnidadOperativa = new UnidadOperativaBO { Id = vista.UnidadOperativaContratoID }
					},
					FechaContrato = informacionGeneralPRE.Vista.FechaContrato,
					FechaInicioContrato = informacionPagoPRE.Vista.FechaInicioContrato,
					Divisa = new DivisaBO { MonedaDestino = informacionGeneralPRE.Vista.MonedaSeleccionada },
					DiasPago = informacionPagoPRE.Vista.DiasPago,
					IncluyeLavado = datosRentaPRE.Vista.IncluyeLavadoSeleccionado,
					IncluyeLlantas = datosRentaPRE.Vista.IncluyeLlantasSeleccionado,
					IncluyePinturaRotulacion = datosRentaPRE.Vista.IncluyePinturaSeleccionado,
					IncluyeSeguro = datosRentaPRE.Vista.IncluyeSeguroSeleccionado,
					LineasContrato = datosRentaPRE.Vista.LineasContrato.ConvertAll(s => (ILineaContrato)s),
					ObligadosSolidarios = clienteContratoPRE.Vista.ObligadosSolidariosContrato.ConvertAll(s => (PersonaBO)s),
					Plazo = datosRentaPRE.Vista.PlazoMeses,
					Representante = informacionGeneralPRE.Vista.Representante,
					RepresentantesLegales = clienteContratoPRE.Vista.RepresentantesLegalesContrato.ConvertAll(s => (PersonaBO)s),
					Sucursal = informacionGeneralPRE.Vista.SucursalSeleccionada,
					Tipo = ETipoContrato.FSL,
					UbicacionTaller = datosRentaPRE.Vista.UbicacionTaller,
					SoloRepresentantes = clienteContratoPRE.Vista.SoloRepresentantes,
					PorcentajePenalizacion = informacionGeneralPRE.Vista.PorcentajePenalizacion,
				};

				// Se Agregan datos de Auditoria
				contrato.FUA = vista.FUA;
				contrato.UUA = vista.UUA;
				contrato.Estatus = EEstatusContrato.EnCurso;

				DireccionClienteBO direccion = new DireccionClienteBO
				{
                    Id = clienteContratoPRE.Vista.DireccionClienteID,
					Ubicacion = new UbicacionBO
					{
						Pais = new PaisBO { Codigo = clienteContratoPRE.Vista.Pais },
						Municipio = new MunicipioBO { Codigo = clienteContratoPRE.Vista.Municipio },
						Estado = new EstadoBO { Codigo = clienteContratoPRE.Vista.Estado },
						Ciudad = new CiudadBO { Codigo = clienteContratoPRE.Vista.Ciudad }
					},
					CodigoPostal = clienteContratoPRE.Vista.CodigoPostal,
					Calle = clienteContratoPRE.Vista.Calle,
					Colonia = clienteContratoPRE.Vista.Colonia
				};

				contrato.Cliente.RemoverDirecciones();
				contrato.Cliente.Agregar(direccion);

				// Datos Adicionales

				if (contrato.Cliente.EsFisico != true) { // Para personas Morales
					List<PersonaBO> Depositarios = contrato.RepresentantesLegales.FindAll(persona => ((RepresentanteLegalBO)persona).EsDepositario == true);
					if (Depositarios.Count == 0) // No tiene Depositarios
                    {
						// No esta activo "Solo Representantes"
						if (contrato.SoloRepresentantes != true && contrato.RepresentantesLegales != null && contrato.RepresentantesLegales.Count > 0)
							foreach (PersonaBO persona in contrato.RepresentantesLegales) {
								RepresentanteLegalBO representante = (RepresentanteLegalBO)persona;
								representante.EsDepositario = true;
							} else if (contrato.SoloRepresentantes == true && contrato.RepresentantesLegales != null && contrato.RepresentantesLegales.Count > 0)
							foreach (PersonaBO persona in contrato.RepresentantesLegales) {
								RepresentanteLegalBO representante = (RepresentanteLegalBO)persona;
								representante.EsDepositario = false;
							}
					} else {
						if (contrato.SoloRepresentantes == true && contrato.RepresentantesLegales != null && contrato.RepresentantesLegales.Count > 0)
							foreach (PersonaBO persona in contrato.RepresentantesLegales) {
								RepresentanteLegalBO representante = (RepresentanteLegalBO)persona;
								representante.EsDepositario = false;
							}
					}
				}

				contrato.CierreContrato = vista.DatosCierre;
				contrato.CierreContrato.Usuario = new UsuarioBO { Id = vista.UUA };

				return contrato;
			} catch (Exception ex) {
				throw new Exception(nombreClase + ".InterfazUsuarioADatos: " + ex.Message);
			}
		}

		/// <summary>
		/// Regresar a la pantalla de Detalles
		/// </summary>
		public void RegresarADetalles() {
			try {
				ContratoFSLBO contrato=new ContratoFSLBO{ContratoID = vista.ContratoID};
				ContratoFSLBR ContratoBR=new ContratoFSLBR();
				contrato = ContratoBR.ConsultarCompleto(dataContext, contrato)[0];
				vista.EstablecerPaqueteNavegacion(vista.Codigo, contrato);
				vista.LimpiarSesion();
				vista.IrADetalleContrato();
			} catch (Exception ex) {
				throw new Exception(nombreClase + ".RegresarADetalles: " + ex.Message);
			}
		}

		/// <summary>
		/// Valida el acceso a la página de edición
		/// </summary>
		public void ValidarAcceso() {
			try {

				//se valida que los datos del usuario y la unidad operativa no sean nulos
				if (this.vista.UsuarioID == null) throw new Exception("El identificador del usuario no debe ser nulo");
				if (this.vista.UnidadAdscripcionID == null) throw new Exception("El identificador de la Unidad Operativa no debe ser nulo ");

				UsuarioBO usr = new UsuarioBO { Id = this.vista.UsuarioID };
				AdscripcionBO adscripcion = new AdscripcionBO { UnidadOperativa = new UnidadOperativaBO { Id = this.vista.UnidadAdscripcionID } };
				SeguridadBO seguridadBo = new SeguridadBO(Guid.Empty, usr, adscripcion);

				if (!FacadeBR.ExisteAccion(this.dataContext, "TERMINAR", seguridadBo))
					this.vista.RedirigirSinPermisoAcceso();
			} catch (Exception ex) {
				throw new Exception(nombreClase + ".ValidarAcceso:" + ex.Message);
			}
		}

		/// <summary>
		/// Valida que el contrato a cerrar se encuentre en curso.
		/// </summary>
		public void ValidarEstadoContrato() {
			ContratoFSLBO contrato = vista.UltimoObjeto;
			if (contrato == null || contrato.Estatus != EEstatusContrato.EnCurso) vista.RedirigirSinPermisoAcceso();
		}

		/// <summary>
		/// Permite configurar el comportamiento de los controles de la vista de acuerdo a los permisos configurados
		/// </summary>
		private void EstablecerSeguridad() {
			try {

				//se valida que los datos del usuario y la unidad operativa no sean nulos
				if (this.vista.UsuarioID == null) throw new Exception("El identificador del usuario no debe ser nulo");
				if (this.vista.UnidadAdscripcionID == null) throw new Exception("El identificador de la Unidad Operativa no debe ser nulo ");

				//Se crea el objeto de seguridad
				UsuarioBO usr = new UsuarioBO { Id = this.vista.UsuarioID };
				AdscripcionBO adscripcion = new AdscripcionBO { UnidadOperativa = new UnidadOperativaBO { Id = this.vista.UnidadAdscripcionID } };
				SeguridadBO seguridadBO = new SeguridadBO(Guid.Empty, usr, adscripcion);

				//Se obtiene las acciones a las cuales el usuario tiene permiso en este proceso
				List<CatalogoBaseBO> lst = FacadeBR.ConsultarAccion(this.dataContext, seguridadBO);

				if (!this.ExisteAcccion(lst, "UI INSERTAR"))
					this.vista.PermitirRegistrar(false);
			} catch (Exception ex) {
				throw new Exception(nombreClase + ".EstablecerSeguridad:" + ex.Message);
			}
		}

		/// <summary>
		/// Valida una acción en especifico dentro de la lista de acciones permitidas para la pagina
		/// </summary>
		/// <param name="acciones">Listado de acciones permitidas para la página</param>
		/// <param name="accion">Acción que se desea validar</param>
		/// <returns>si la acción a evaluar se encuentra dentro de la lista de acciones permitidas se devuelve true. En caso contario false. bool</returns>
		private bool ExisteAcccion(List<CatalogoBaseBO> acciones, string accion) {
			if (acciones != null && acciones.Count > 0 && acciones.Exists(p => p != null && p.Nombre != null && p.Nombre.Trim().ToUpper() == accion.Trim().ToUpper()))
				return true;

			return false;
		}

		/// <summary>
		/// Prepara la Linea de contrato para visualizacion
		/// </summary>
		/// <param name="linea">Linea de Contrato que contiene los datos a mostrar</param>
		public void PrepararLinea(LineaContratoFSLBO linea) {
			if (linea != null) {
				if (vista.UltimoObjeto.Plazo != null && vista.UltimoObjeto.Plazo > 0) {
					lineaContratoPRE.EstablecerUltimoObjeto(linea);
					lineaContratoPRE.DatosAInterfazUsuario(linea, vista.UltimoObjeto.CalcularPlazoEnAños(), vista.UltimoObjeto.IncluyeSeguro);
					vista.CambiarALinea();
				} else {
					vista.MostrarMensaje("No ha proporcionado un plazo en meses valido o mayor a 0",
										 ETipoMensajeIU.ADVERTENCIA);
				}
			} else {
				vista.MostrarMensaje("No ha proporcionado un plazo en meses valido o mayor a 0",
										 ETipoMensajeIU.ADVERTENCIA);
			}
		}
		#endregion Métodos
	}
}