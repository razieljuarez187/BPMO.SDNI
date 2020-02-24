// Satisface al CU015 - Registrar Contrato Full Service Leasing
// Satisface a la solicitud de Cambio SC0001
// Satisface al caso de uso CU001 - Calcular Monto a Facturar FSL
// Satisface a la solución de la RI0008
// Construccion durante staffing - Configuracion de INPC
using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI;
using BPMO.Basicos.BO;
using BPMO.Facade.SDNI.BR;
using BPMO.Patterns.Creational.DataContext;
using BPMO.Primitivos.Enumeradores;
using BPMO.SDNI.Comun.BO;
using BPMO.SDNI.Comun.BR;
using BPMO.SDNI.Comun.PRE;
using BPMO.SDNI.Contratos.BO;
using BPMO.SDNI.Contratos.FSL.BO;
using BPMO.SDNI.Contratos.FSL.BR;
using BPMO.SDNI.Contratos.FSL.VIS;
using BPMO.SDNI.Equipos.BO;
using BPMO.SDNI.Facturacion.AplicacionesPago.BR;

namespace BPMO.SDNI.Contratos.FSL.PRE
{
    /// <summary>
    /// Presentador del Registro de Contratos FSL
    /// </summary>
    public class RegistrarContratoFSLPRE
    {
        #region Atributos
        /// <summary>
        /// Vista del Registro de Contrato
        /// </summary>
        private readonly IRegistrarContratoFSLVIS vista;
        /// <summary>
        /// DataContext que administra las conexiones a Datos
        /// </summary>
        private readonly IDataContext dataContext;
        /// <summary>
        /// Nombre de la Clase del Presentador
        /// </summary>
        private const string nombreClase = "RegistrarContratoFSLPRE";
        /// <summary>
        /// Presentador de Linea de Contrato
        /// </summary>
        private readonly ucLineaContratoFSLPRE lineaContratoPRE;
        /// <summary>
        /// Presentador de Información General
        /// </summary>
        private readonly ucInformacionGeneralPRE informacionGeneralPRE;
        /// <summary>
        /// Presentador de los Datos del Cliente
        /// </summary>
        private readonly ucClienteContratoPRE clienteContratoPRE;
        /// <summary>
        /// Presentador de los Datos de Renta
        /// </summary>
        private readonly ucDatosRentaPRE datosRentaPRE;
        /// <summary>
        /// Presentador de la Información de Pago
        /// </summary>
        private readonly ucInformacionPagoPRE informacionPagoPRE;
        /// <summary>
        /// Presentador del Control de Documentos
        /// </summary>
        private readonly ucCatalogoDocumentosPRE documentosPRE;
        /// <summary>
        /// Presentador de Datos Adicionales
        /// </summary>
        private readonly ucDatosAdicionalesAnexoPRE datosAdicionalesPRE; 
        #endregion

        #region Contructores

        /// <summary>
        /// Constructor del Presentado
        /// </summary>
        /// <param name="vistaActual">vista sobre la que actuará el presentador</param>        
        /// <param name="general">Presentador de la Informacion General</param>
        /// <param name="cliente">Presentador de los datos del Cliente</param>
        /// <param name="datosRenta">Presentador de los datos de Renta</param>
        /// <param name="pago">Presentador de la informacion de Pago</param>
        /// <param name="lineaContrato">Presentador de las lineas de contrato</param>
        /// <param name="documentos">Presentador de las documentos</param>
        /// <param name="datosAdicionales">Presentador de los Datos Adicionales</param>
        public RegistrarContratoFSLPRE(IRegistrarContratoFSLVIS vistaActual, 
                                    ucInformacionGeneralPRE general, ucClienteContratoPRE cliente,
                                    ucDatosRentaPRE datosRenta, ucInformacionPagoPRE pago,
                                    ucLineaContratoFSLPRE lineaContrato, ucCatalogoDocumentosPRE documentos, ucDatosAdicionalesAnexoPRE datosAdicionales)
        {
            try
            {
                vista = vistaActual;

                dataContext = Facade.SDNI.BR.FacadeBR.ObtenerConexion();

                informacionGeneralPRE = general;
                clienteContratoPRE = cliente;
                datosRentaPRE = datosRenta;
                lineaContratoPRE = lineaContrato;
                informacionPagoPRE = pago;
                documentosPRE = documentos;
                datosAdicionalesPRE = datosAdicionales;
            }
            catch (Exception ex)
            {
                throw new Exception(nombreClase + ".RegistrarContratoFSLPRE: " + ex.Message);
            }
        }
        #endregion

        #region Metodos

        /// <summary>
        /// Invoca los Metodos de Inicializacion de la Vista
        /// </summary>
        public void Inicializar()
        {
            try
            {
                this.EstablecerSeguridad();//SC_0008
                vista.Clave = "ContratoFSLBO";
                documentosPRE.LimpiarSesion();
                DesplegarTiposArchivos();

                informacionGeneralPRE.Vista.ConfigurarModoEditar();
                informacionPagoPRE.Vista.ConfigurarModoEditar();
                clienteContratoPRE.Vista.ConfigurarModoEditar();
                datosRentaPRE.Vista.ConfigurarModoEditar();
                lineaContratoPRE.Vista.ConfigurarModoEditar();
                documentosPRE.ModoEditable(true);
                datosAdicionalesPRE.Vista.ConfigurarModoEditar(); //SC0007    

                informacionGeneralPRE.Inicializar();
                informacionGeneralPRE.DesplegarConfiguracionUnidadOperativa();
                informacionPagoPRE.Inicializar();
                clienteContratoPRE.Inicializar();
                datosRentaPRE.Inicializar();
                datosAdicionalesPRE.Inicializar();                
            }
            catch (Exception ex)
            {
                throw new Exception(nombreClase + ".Inicializar: " + ex.Message);
            }
        }
  
        /// <summary>
        /// Obtiene un Contrato a partir de los datos de la vista
        /// </summary>
        /// <returns></returns>
        public ContratoFSLBO InterfazUsuarioADatos()
        {
            try
            {
                var contrato = new ContratoFSLBO
                {
                    ContratoID = null,
                    #region Cuenta Bancaria
                    //Banco = informacionPagoPRE.Vista.CuentaBancariaSeleccionada,
                    #endregion
                    Cliente = new CuentaClienteIdealeaseBO
                    {
                        Id = clienteContratoPRE.Vista.CuentaClienteID,
                        Nombre = clienteContratoPRE.Vista.NombreCuentaCliente,
                        Cliente = new ClienteBO { Id = clienteContratoPRE.Vista.ClienteID, Fisica = clienteContratoPRE.Vista.EsFisico },
                        UnidadOperativa = new UnidadOperativaBO { Id = vista.UnidadOperativa.Id, Nombre = vista.UnidadOperativa.Nombre}
                    },
                    FechaContrato = informacionGeneralPRE.Vista.FechaContrato,
                    FechaInicioContrato = informacionPagoPRE.Vista.FechaInicioContrato,
                    Divisa = new DivisaBO { MonedaDestino = informacionGeneralPRE.Vista.MonedaSeleccionada },
                    DiasPago = informacionPagoPRE.Vista.DiasPago,
                    IncluyeLavado = datosRentaPRE.Vista.IncluyeLavadoSeleccionado,
                    IncluyeLlantas = datosRentaPRE.Vista.IncluyeLlantasSeleccionado,
                    IncluyePinturaRotulacion = datosRentaPRE.Vista.IncluyePinturaSeleccionado,
                    IncluyeSeguro = datosRentaPRE.Vista.IncluyeSeguroSeleccionado,
                    FrecuenciaSeguro = datosRentaPRE.Vista.FrecuenciaSeguro,
                    PorcentajeSeguro = datosRentaPRE.Vista.PorcentajeSeguro,
                    LineasContrato = datosRentaPRE.Vista.LineasContrato.ConvertAll(s => (ILineaContrato)s),
                    ObligadosSolidarios = clienteContratoPRE.Vista.ObligadosSolidariosContrato.ConvertAll(s => (PersonaBO)s),
                    Plazo = datosRentaPRE.Vista.PlazoMeses,
                    Representante = informacionGeneralPRE.Vista.Representante,
                    RepresentantesLegales = clienteContratoPRE.Vista.RepresentantesLegalesContrato.ConvertAll(s => (PersonaBO)s),
                    Sucursal = informacionGeneralPRE.Vista.SucursalSeleccionada,
                    Tipo = ETipoContrato.FSL,
                    UbicacionTaller = datosRentaPRE.Vista.UbicacionTaller,
                    SoloRepresentantes = clienteContratoPRE.Vista.SoloRepresentantes,
                    PorcentajePenalizacion = informacionGeneralPRE.Vista.PorcentajePenalizacion
                };

				contrato.ObligadosComoAvales = clienteContratoPRE.Vista.ObligadosComoAvales;
				if (clienteContratoPRE.Vista.AvalesSeleccionados != null)
					contrato.Avales = clienteContratoPRE.Vista.AvalesSeleccionados;
				if (contrato.ObligadosComoAvales != null && contrato.ObligadosComoAvales == true)
				{
					if (clienteContratoPRE.Vista.ObligadosSolidariosContrato != null)
						contrato.Avales = clienteContratoPRE.Vista.ObligadosSolidariosContrato.ConvertAll(s => clienteContratoPRE.ObligadoAAval(s));
				}

                List<ArchivoBO> adjuntos = documentosPRE.Vista.ObtenerArchivos() ?? new List<ArchivoBO>();
                foreach (ArchivoBO adjuntoContratoBo in adjuntos)
                {
                    adjuntoContratoBo.TipoAdjunto = ETipoAdjunto.ContratoFSL;
                }
                contrato.DocumentosAdjuntos = adjuntos;

                // Se Agregan datos de Auditoria
                contrato.FC = vista.FC;
                contrato.UC = vista.UC;
                contrato.FUA = vista.FUA;
                contrato.UUA = vista.UUA;
                contrato.Estatus = EEstatusContrato.Borrador;

                // Se agrega la Auditoria de cada uno de los documentos adjuntos
                foreach (ArchivoBO adjunto in contrato.DocumentosAdjuntos)
                {
                    if (adjunto.Id == null)
                    {
                        adjunto.Auditoria = new AuditoriaBO
                        {

                            FC = contrato.FUA,
                            UC = contrato.UUA,
                            FUA = contrato.FUA,
                            UUA = contrato.UUA
                        };
                    }
                    else
                    {
                        adjunto.Auditoria.FUA = contrato.FUA;
                        adjunto.Auditoria.UUA = contrato.UUA;
                    }
                }

                #region SC0001

                DireccionClienteBO direccion = new DireccionClienteBO
                    {
                        Id = clienteContratoPRE.Vista.DireccionClienteID,
                        Ubicacion =
                            new UbicacionBO
                                {
                                    Pais = new PaisBO {Codigo = clienteContratoPRE.Vista.Pais},
                                    Municipio = new MunicipioBO {Codigo = clienteContratoPRE.Vista.Municipio},
                                    Estado = new EstadoBO {Codigo = clienteContratoPRE.Vista.Estado},
                                    Ciudad = new CiudadBO {Codigo = clienteContratoPRE.Vista.Ciudad}
                                },
                        CodigoPostal = clienteContratoPRE.Vista.CodigoPostal,
                        Calle = clienteContratoPRE.Vista.Calle,
                        Colonia = clienteContratoPRE.Vista.Colonia
                    };

                contrato.Cliente.RemoverDirecciones();
                contrato.Cliente.Agregar(direccion);
                #endregion

                #region SC0007
                // Datos Adicionales
                List<DatoAdicionalAnexoBO> Lista = new List<DatoAdicionalAnexoBO>();
                int iterador = 1;
                foreach (DatoAdicionalAnexoBO datoAdicional in datosAdicionalesPRE.Vista.DatosAdicionales)
                {
                    datoAdicional.DatoAdicionalID = iterador++;
                    Lista.Add(datoAdicional);
                }

                contrato.DatosAdicionalesAnexo = Lista;

                if (contrato.Cliente.EsFisico != true)
                { // Para personas Morales
                    List<PersonaBO> Depositarios = contrato.RepresentantesLegales.FindAll(persona => ((RepresentanteLegalBO)persona).EsDepositario == true);
                    if (Depositarios.Count == 0) // No tiene Depositarios
                    {
                        // No esta activo "Solo Representantes"
                        if (contrato.SoloRepresentantes != true && contrato.RepresentantesLegales != null && contrato.RepresentantesLegales.Count > 0)
                            foreach (PersonaBO persona in contrato.RepresentantesLegales)
                            {
                                RepresentanteLegalBO representante = (RepresentanteLegalBO)persona;
                                representante.EsDepositario = true;
                            }
                        else if (contrato.SoloRepresentantes == true && contrato.RepresentantesLegales != null && contrato.RepresentantesLegales.Count > 0)
                            foreach (PersonaBO persona in contrato.RepresentantesLegales)
                            {
                                RepresentanteLegalBO representante = (RepresentanteLegalBO)persona;
                                representante.EsDepositario = false;
                            }
                    }
                    else
                    {
                        if (contrato.SoloRepresentantes == true && contrato.RepresentantesLegales != null && contrato.RepresentantesLegales.Count > 0)
                            foreach (PersonaBO persona in contrato.RepresentantesLegales)
                            {
                                RepresentanteLegalBO representante = (RepresentanteLegalBO)persona;
                                representante.EsDepositario = false;
                            }
                    }
                }
                #endregion

                #region RI0008

                foreach (var linea in contrato.LineasContrato)
                {
                    linea.LineaContratoID = null;
                }
                #endregion

                contrato.InpcContrato = vista.InpcContrato;
                if (contrato.InpcContrato != null)
                {
                    if (contrato.InpcContrato.Auditoria == null)
                        contrato.InpcContrato.Auditoria = new AuditoriaBO();

                    contrato.InpcContrato.Auditoria.UC = contrato.UC;
                    contrato.InpcContrato.Auditoria.FC = contrato.FC;
                    contrato.InpcContrato.Auditoria.UUA = contrato.UUA;
                    contrato.InpcContrato.Auditoria.FUA = contrato.FUA;
                }

                return contrato;
            }
            catch (Exception ex)
            {
                throw new Exception(nombreClase + ".InterfazUsuarioADatos: " + ex.Message);
            }
        }

        /// <summary>
        /// Valida los datos necesarios para un Contrato Borrador
        /// </summary>
        /// <returns></returns>
        public string ValidarContratoBorrador()
        {
            string mensaje = string.Empty;

            if (informacionGeneralPRE.Vista.SucursalSeleccionada == null)
                mensaje += " Sucursal, ";

            if (informacionGeneralPRE.Vista.MonedaSeleccionada == null)
                mensaje += " Moneda, ";

            if (clienteContratoPRE.Vista.ClienteID == null || clienteContratoPRE.Vista.CuentaClienteID == null || string.IsNullOrEmpty(clienteContratoPRE.Vista.NombreCuentaCliente))
                mensaje += " Cliente, ";

            if (datosRentaPRE.Vista.PlazoMeses != null && datosRentaPRE.Vista.PlazoMeses == 0)
                mensaje += " El plazo debe ser mayor a 0 meses, ";



            if (!string.IsNullOrEmpty(mensaje))
                return "Los siguientes campos no pueden estar vacíos: \n" + mensaje.Substring(0, mensaje.Length - 2);
			
			#region RI0057
			if (informacionGeneralPRE.Vista.FechaContrato != null && informacionPagoPRE.Vista.FechaInicioContrato != null) {
				if (DateTime.Compare((DateTime)informacionGeneralPRE.Vista.FechaContrato, (DateTime)informacionPagoPRE.Vista.FechaInicioContrato) > 0)
					mensaje += "La fecha de contrato no puede ser mayor que la fecha de inicio";
			}
			#endregion
			return mensaje;
        }

        /// <summary>
        /// Valida los datos para un Contrato En Curso
        /// </summary>
        /// <returns></returns>
        public string ValidarContratoEnCurso()
        {
            string mensaje = string.Empty;

            if (informacionGeneralPRE.Vista.FechaContrato == null) mensaje += " Fecha de Contrato, ";

            if (informacionGeneralPRE.Vista.SucursalSeleccionada == null) mensaje += " Sucursal, ";

            if (string.IsNullOrEmpty(informacionGeneralPRE.Vista.Representante)) mensaje += " Representante, ";

            if (informacionGeneralPRE.Vista.MonedaSeleccionada == null) mensaje += " Moneda, ";

            if (clienteContratoPRE.Vista.ClienteID == null || clienteContratoPRE.Vista.CuentaClienteID == null || string.IsNullOrEmpty(clienteContratoPRE.Vista.NombreCuentaCliente))
                mensaje += " Cliente, ";

            if (clienteContratoPRE.Vista.EsFisico != true && clienteContratoPRE.Vista.RepresentantesLegalesContrato.Count == 0) mensaje += " Representantes Legales, ";

            if (clienteContratoPRE.Vista.EsFisico == true && clienteContratoPRE.Vista.ObligadosSolidariosContrato.Count == 0) mensaje += " Obligados Solidarios, ";

			if (clienteContratoPRE.Vista.SoloRepresentantes != true)
				if (clienteContratoPRE.Vista.ObligadosComoAvales != true)
					if (clienteContratoPRE.Vista.AvalesSeleccionados == null || clienteContratoPRE.Vista.AvalesSeleccionados.Count == 0) mensaje += " Avales, ";

            if (datosRentaPRE.Vista.PlazoMeses == null) mensaje += " Plazo en Meses, ";

            if (datosRentaPRE.Vista.PlazoMeses != null && datosRentaPRE.Vista.PlazoMeses == 0)
                mensaje += " El plazo debe ser mayor a 0 meses, ";

            if (string.IsNullOrEmpty(datosRentaPRE.Vista.UbicacionTaller)) mensaje += " Ubicación de Taller de Servicio, ";

            if (datosRentaPRE.Vista.LineasContrato.Count == 0) mensaje += " Unidades a Rentar, ";

            if (datosRentaPRE.Vista.IncluyeSeguroSeleccionado == null) mensaje += " Incluye Seguro, ";
            
            if (datosRentaPRE.Vista.IncluyeSeguroSeleccionado != null && datosRentaPRE.Vista.IncluyeSeguroSeleccionado == ETipoInclusion.NoIncluidoCargoCliente && datosRentaPRE.Vista.FrecuenciaSeguro == null)
                mensaje += " Frecuencia Seguro, ";
            if (datosRentaPRE.Vista.IncluyeSeguroSeleccionado != null && datosRentaPRE.Vista.IncluyeSeguroSeleccionado == ETipoInclusion.NoIncluidoCargoCliente && datosRentaPRE.Vista.PorcentajeSeguro == null)
                mensaje += " Porcentaje Seguro, ";

            if (datosRentaPRE.Vista.IncluyeLlantasSeleccionado == null) mensaje += " Incluye Llantas, ";

            if (datosRentaPRE.Vista.IncluyePinturaSeleccionado == null) mensaje += " Incluye Pintura, ";

            if (datosRentaPRE.Vista.IncluyeLavadoSeleccionado == null) mensaje += " Incluye Lavado, ";

            if (informacionPagoPRE.Vista.FechaInicioContrato == null) mensaje += " Fecha de Inicio del Contrato, ";

            if (informacionPagoPRE.Vista.DiasPago == null) mensaje += " Días para realizar el pago, ";

            #region Cuenta Bancaria
            //if (vista.CuentaBancariaSeleccionada == null) mensaje += " Cuenta Bancaria, ";
            #endregion

            #region SC0001
            if (string.IsNullOrEmpty(clienteContratoPRE.Vista.Direccion) && string.IsNullOrWhiteSpace(clienteContratoPRE.Vista.Direccion))
                mensaje += "Dirección del cliente, ";
            else
            {
                if (string.IsNullOrEmpty(clienteContratoPRE.Vista.Calle) && string.IsNullOrWhiteSpace(clienteContratoPRE.Vista.Calle))
                    mensaje += " calle en dirección, ";
                if (string.IsNullOrEmpty(clienteContratoPRE.Vista.Ciudad) && string.IsNullOrWhiteSpace(clienteContratoPRE.Vista.Ciudad))
                    mensaje += " ciudad en dirección, ";
                if (string.IsNullOrEmpty(clienteContratoPRE.Vista.CodigoPostal) && string.IsNullOrWhiteSpace(clienteContratoPRE.Vista.CodigoPostal))
                    mensaje += " código postal en dirección, ";
                if (string.IsNullOrEmpty(clienteContratoPRE.Vista.Colonia) && string.IsNullOrWhiteSpace(clienteContratoPRE.Vista.Colonia))
                    mensaje += " colonia en dirección, ";
                if (string.IsNullOrEmpty(clienteContratoPRE.Vista.Estado) && string.IsNullOrWhiteSpace(clienteContratoPRE.Vista.Estado))
                    mensaje += " estado en dirección, ";
                if (string.IsNullOrEmpty(clienteContratoPRE.Vista.Municipio) && string.IsNullOrWhiteSpace(clienteContratoPRE.Vista.Municipio))
                    mensaje += " municipio en dirección, ";
                if (string.IsNullOrEmpty(clienteContratoPRE.Vista.Pais) && string.IsNullOrWhiteSpace(clienteContratoPRE.Vista.Pais))
                    mensaje += " país en dirección, ";
            }
            #endregion



            if (!string.IsNullOrEmpty(mensaje))
                return "Los siguientes campos no pueden estar vacíos: \n" + mensaje.Substring(0, mensaje.Length - 2);
			
			#region RI0057
			if (informacionGeneralPRE.Vista.FechaContrato != null && informacionPagoPRE.Vista.FechaInicioContrato != null) {
				if (DateTime.Compare((DateTime)informacionGeneralPRE.Vista.FechaContrato, (DateTime)informacionPagoPRE.Vista.FechaInicioContrato) > 0)
					mensaje += "La fecha de contrato no puede ser mayor que la fecha de inicio";
			}
			#endregion

            if (vista.InpcContrato == null) mensaje += "Se debe Configurar el INPC para el Contrato";

            return mensaje;
        }

        /// <summary>
        /// Calcula la Fecha de Fin de Contrato
        /// </summary>
        public void CalcularFechaFinContrato()
        {
            try
            {

                if (informacionPagoPRE.Vista.FechaInicioContrato != null && datosRentaPRE.Vista.PlazoMeses != null)
                    informacionPagoPRE.Vista.FechaTerminacionContrato =
                        informacionPagoPRE.Vista.FechaInicioContrato.Value.AddMonths(
                            datosRentaPRE.Vista.PlazoMeses.Value);
                else
                    informacionPagoPRE.Vista.FechaTerminacionContrato = null;

            }
            catch (Exception ex)
            {

                throw new Exception(nombreClase + ".CalcularFechaFinContrato: " + ex.Message);
            }
        }
        /// <summary>
        /// Guarda un Contrato con Estatus de Borrador
        /// </summary>
        public void GuardarContratoBorrador()
        {
            string resultado = ValidarContratoBorrador();

            if (!string.IsNullOrEmpty(resultado))
            {
                vista.MostrarMensaje(resultado, ETipoMensajeIU.INFORMACION);
                return;
            }

            try
            {
                ContratoFSLBO contrato = InterfazUsuarioADatos();
                contrato.Estatus = EEstatusContrato.Borrador;


                int? contratoID = GuardarContrato(contrato);
                if (contratoID != null)
                {
                    vista.EstablecerPaqueteNavegacion(vista.Clave, contrato);
                    vista.MostrarMensaje("Se ha guardado del contrato exitosamente.", ETipoMensajeIU.EXITO);
                    vista.IrADetalle();
                }
                else
                    vista.MostrarMensaje("Inconsistencias al intentar guardar el contrato.", ETipoMensajeIU.ADVERTENCIA);
            }
            catch (Exception ex)
            {
                throw new Exception(nombreClase + ".GuardarContratoBorrador:" + ex.Message);
            }
        }
        /// <summary>
        /// Guarda un contrato con estatus de En Curso
        /// </summary>
        public void GuardarContratoEnCurso()
        {
            string resultado = ValidarContratoEnCurso();

            if (!string.IsNullOrEmpty(resultado))
            {
                vista.MostrarMensaje(resultado, ETipoMensajeIU.INFORMACION);
                return;
            }

            #region Se inicia la Transaccion
            dataContext.SetCurrentProvider("Outsourcing");
            Guid firma = Guid.NewGuid();
            try
            {
                dataContext.OpenConnection(firma);
                dataContext.BeginTransaction(firma);
            }

            catch(Exception)
            {
                if(dataContext.ConnectionState == ConnectionState.Open)
                    dataContext.CloseConnection(firma);
                throw new Exception("Se encontraron inconsistencias al insertar el Contrato.");
            }
            #endregion

            try
            {
                ContratoFSLBO contrato = InterfazUsuarioADatos();
                contrato.Estatus = EEstatusContrato.EnCurso;
                
                int? contratoID=  GuardarContrato(contrato);
                if (contratoID != null)
                {
                    #region SC0001 BEP1401 - Registra los pagos del Contrato

                    contrato.ContratoID = contratoID;
                    GeneradorPagosFSLBR generadorPagosFsl = new GeneradorPagosFSLBR();

                    SeguridadBO seguridadBo = new SeguridadBO(Guid.Empty, new UsuarioBO { Id = this.vista.UsuarioID }, new AdscripcionBO
                    {
                        Departamento = new DepartamentoBO(),
                        Sucursal = new SucursalBO(),
                        UnidadOperativa = new UnidadOperativaBO { Id = this.vista.UnidadAdscripcionID }
                    });

                    generadorPagosFsl.GenerarPagos(dataContext, contrato, seguridadBo,true);

                    #endregion

                    dataContext.CommitTransaction(firma);

                    vista.EstablecerPaqueteNavegacion(vista.Clave, contrato);
                    vista.MostrarMensaje("Se ha guardado del contrato exitosamente.", ETipoMensajeIU.EXITO);
                    vista.IrADetalle();
                }
                else
                    vista.MostrarMensaje("Inconsistencias al intentar guardar el contrato.", ETipoMensajeIU.ADVERTENCIA);
            }
            catch (Exception ex)
            {
                dataContext.RollbackTransaction(firma);
                throw new Exception(nombreClase + ".GuardarContratoEnCurso:" + ex.Message);
            }
            finally
            {
                if(dataContext.ConnectionState == ConnectionState.Open)
                    dataContext.CloseConnection(firma);
            }
        }
        /// <summary>
        /// Registra el nuevo contrato en la base de datos
        /// </summary>
        /// <param name="contrato">Contrato a Registrar</param>
        /// <returns>Retorna el Identificador del Nuevo Contrato</returns>
        private int? GuardarContrato(ContratoFSLBO contrato)
        {
            try
            {
                //SC_0008
                SeguridadBO seguridadBO = new SeguridadBO(Guid.Empty, new UsuarioBO { Id = this.vista.UsuarioID }, new AdscripcionBO
                {
                    Departamento = new DepartamentoBO(),
                    Sucursal = new SucursalBO(),
                    UnidadOperativa = new UnidadOperativaBO { Id = this.vista.UnidadAdscripcionID }
                });
                //END SC_0008

                var contratoBR = new ContratoFSLBR();
                contratoBR.InsertarCompleto(dataContext, contrato, seguridadBO);
                DataSet ds = contratoBR.ConsultarSet(dataContext, contrato);

                if (ds.Tables[0].Rows.Count > 0)
                {
                    return ds.Tables[0].Rows[0].Field<int>("ContratoID");
                }
                return null;
            }
            catch (Exception ex)
            {
                throw new Exception(nombreClase + ".GuardarContrato:" + ex.Message);
            }

        }
  
        /// <summary>
        /// Cancela el Registro de un Contrato
        /// </summary>
        public void CancelarRegistroContrato()
        {
            vista.IrAConsultar();
        }

        /// <summary>
        /// Prepara una nueva linea de contrato para capturar y agregar al contrato
        /// </summary>
        public void PrepararNuevaLinea()
        {
            try
            {
                if (datosRentaPRE.Vista.PlazoMeses != null)
                {
                    if (datosRentaPRE.Vista.UnidadID != null)
                    {
                        if (!datosRentaPRE.ExisteUnidadContrato(new UnidadBO { UnidadID = datosRentaPRE.Vista.UnidadID }))
                        {
                            if (datosRentaPRE.Vista.PlazoMeses != null && datosRentaPRE.Vista.PlazoMeses > 0)
                            {
                                UnidadBO unidad = datosRentaPRE.ObtenerUnidadAgregar();
                                unidad.Sucursal = new SucursalBO
                                {
                                    UnidadOperativa = new UnidadOperativaBO { Id = vista.UnidadOperativa.Id }
                                };
                                lineaContratoPRE.Inicializar(unidad, datosRentaPRE.Vista.PlazoAnios);
                                vista.CambiarALinea();
                            }
                            else
                            {
                                vista.MostrarMensaje("No ha proporcionado un plazo en meses valido o mayor a 0", ETipoMensajeIU.ADVERTENCIA);
                            }
                        }
                        else
                            vista.MostrarMensaje("La unidad seleccionada ya existe en el contrato.",
                                                 ETipoMensajeIU.INFORMACION);
                    }
                    else
                        vista.MostrarMensaje("No ha seleccionado una unidad valida para agregar al contrato",
                                             ETipoMensajeIU.ADVERTENCIA);

                }
                else
                    vista.MostrarMensaje("No ha proporcionado un plazo en meses valido o mayor a 0",
                                         ETipoMensajeIU.INFORMACION);
            }
            catch (Exception ex)
            {
                throw new Exception(nombreClase + ".PrepararNuevaLinea: " + ex.Message);
            }
        }

        /// <summary>
        /// Despliega los tipos de archivo
        /// </summary>
        public void DesplegarTiposArchivos()
        {
            try
            {
                var tipoBR = new TipoArchivoBR();

                var tipo = new TipoArchivoBO ();

                vista.CargarTiposArchivos(tipoBR.Consultar(dataContext, tipo));
            }
            catch (Exception ex)
            {
                throw new Exception(nombreClase + ".DesplegarTiposArchivos:" + ex.Message);
            }
        }
        /// <summary>
        /// Calcula los Pagos Totales y Mensuales
        /// </summary>
        public void CalcularTotales()
        {
            try
            {
                var contrato = new ContratoFSLBO
                {
                    LineasContrato = datosRentaPRE.Vista.LineasContrato.ConvertAll(ln => (ILineaContrato)ln),
                    Plazo = datosRentaPRE.Vista.PlazoMeses
                };
                informacionPagoPRE.Vista.Total = contrato.CalcularTotalAPagar();
                informacionPagoPRE.Vista.Mensualidad = contrato.CalcularMensualidad();
            }
            catch (Exception ex)
            {
                throw new Exception(nombreClase + ".CalcularTotales:" + ex.Message);
            }
        }

        /// <summary>
        /// Prepara la Linea de contrato para visualizacion
        /// </summary>
        /// <param name="linea">Linea de Contrato que contiene los datos a mostrar</param>
        public void PrepararLinea(LineaContratoFSLBO linea)
        {
            try
            {
                if (linea != null)
                {
                    if (datosRentaPRE.Vista.PlazoMeses != null && datosRentaPRE.Vista.PlazoMeses > 0)
                    {
                        lineaContratoPRE.Vista.Inicializar();
                        lineaContratoPRE.EstablecerUltimoObjeto(linea);
                        lineaContratoPRE.DatosAInterfazUsuario(linea, datosRentaPRE.Vista.PlazoAnios,
                                                               datosRentaPRE.Vista.IncluyeSeguroSeleccionado);
                        vista.CambiarALinea();
                    }
                    else
                    {
                        vista.MostrarMensaje("No ha proporcionado un plazo en meses valido o mayor a 0",
                                             ETipoMensajeIU.ADVERTENCIA);
                    }
                }
                else
                {
                    vista.MostrarMensaje("No ha proporcionado un plazo en meses valido o mayor a 0",
                                         ETipoMensajeIU.ADVERTENCIA);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(nombreClase + ".PrepararLinea:" + ex.Message);
            }
        }

        /// <summary>
        /// Cancela el agrear una linea de contrato
        /// </summary>
        public void CancelarLineaContrato()
        {
            try
            {
                vista.CambiaAContrato();
                datosRentaPRE.InicializarAgregarUnidad();
            }
            catch (Exception ex)
            {
                throw new Exception(nombreClase + ".CancelarLineaContrato: " + ex.Message);
            }
        }

        /// <summary>
        /// Agrega una Linea de Contrato
        /// </summary>
        public void AgregarLineaContrato()
        {
            try
            {

                datosRentaPRE.AgregarLineaContrato(lineaContratoPRE.InterfazUsuarioADatos());
                datosRentaPRE.InicializarAgregarUnidad();

                CalcularTotales();

                vista.CambiaAContrato();
            }
            catch (Exception ex)
            {
                throw new Exception(nombreClase + ".AgregarLineaContrato: " + ex.Message);
            }
        }

        #region SC_0008
        /// <summary>
        /// Valida el permiso de acceso a la página
        /// </summary>
        public void EstablecerSeguridad()
        {
            try
            {
                //se valida que los datos del usuario y la unidad operativa no sean nulos
                if (this.vista.UsuarioID == null) throw new Exception("El identificador del usuario no debe ser nulo");
                if (this.vista.UnidadAdscripcionID == null) throw new Exception("El identificador de la Unidad Operativa no debe ser nulo ");

                //Se crea el objeto de seguridad
                UsuarioBO usr = new UsuarioBO {Id = this.vista.UsuarioID};
                AdscripcionBO sdscripcion = new AdscripcionBO { UnidadOperativa = new UnidadOperativaBO { Id = this.vista.UnidadAdscripcionID } };
                SeguridadBO seguridadBO = new SeguridadBO(Guid.Empty, usr, sdscripcion);

                //Se obtienen las acciones a las cuales el usuario tiene permiso en este proceso
                List<CatalogoBaseBO> lst = FacadeBR.ConsultarAccion(this.dataContext, seguridadBO);

                //Se valida si el usuario tiene permiso para registrar un contrato
                if (!this.ExisteAccion(lst, "INSERTARCOMPLETO"))
                    this.vista.RedirigirSinPermisoAcceso();

                #region RI0008
                //Se valida si el usuario tiene permiso para registrar contrato en curso
                if (!this.ExisteAccion(lst, "GENERARPAGOS"))
                    this.vista.PermitirGuardarEnCurso(false); 
                #endregion
            }
            catch (Exception ex)
            {
                throw new Exception(nombreClase + ".EstablecerSeguridad:" + ex.Message);
            }
        }
        /// <summary>
        /// Verifica si exista una acción en un listado de acciones proporcionado
        /// </summary>
        /// <param name="acciones">Listado de Acciones</param>
        /// <param name="nombreAccion">Nombre de la Acción a Verificar</param>
        /// <returns></returns>
        private bool ExisteAccion(List<CatalogoBaseBO> acciones, string nombreAccion)
        {
            if (acciones != null && acciones.Count > 0 && acciones.Exists(p => p != null && p.Nombre != null && p.Nombre.Trim().ToUpper() == nombreAccion.Trim().ToUpper()))
                return true;

            return false;
        }
        #endregion
        /// <summary>
        /// Activa o desactiva controles de acuerdo a la seleccion del Tipo de INPC
        /// </summary>
        public void CambiarSeleccionTipoINPC()
        {
            try
            {
                if (vista.InpcFijo == null)
                {
                    vista.PermitirControlesINPC(false);
                }
                else
                {
                    vista.PermitirControlesINPC(true);
                    vista.PermitirValorINPC(vista.InpcFijo.Value);
                }
                vista.FechaInicioInpc = null;
                vista.ValorInpc = null;
            }
            catch (Exception ex)
            {
                throw new Exception(nombreClase + ".CambiarSeleccionTipoINPC(): " +  ex.Message, ex.InnerException);
            }
        }
        /// <summary>
        /// Guarda o Restablece la Configuracion del INPC
        /// </summary>
        /// <param name="guardar">Determina si la configuracion se guardara o Restablecera</param>
        public void GuardarConfiguracionINPC(bool guardar)
        {
            try
            {
                if (guardar)
                {
                    string s = ValidarINPC();
                    if (!String.IsNullOrEmpty(s))
                    {
                        vista.MostrarMensaje(s, ETipoMensajeIU.ADVERTENCIA);
                        return;
                    }

                    var inpcContrato = this.vista.InpcContrato;
                    if (inpcContrato == null)
                        inpcContrato = new INPCContratoBO();

                    inpcContrato.Fijo = vista.InpcFijo;
                    inpcContrato.FechaInicio = vista.FechaInicioInpc;
                    inpcContrato.Valor = vista.ValorInpc;

                    vista.InpcContrato = inpcContrato;
                }

                vista.InpcFijo = null;
                vista.FechaInicioInpc = null;
                vista.ValorInpc = null;
                vista.PermitirControlesINPC(false);
                vista.CambiaAContrato();
            }
            catch (Exception ex)
            {
                throw new Exception(nombreClase + ".GuardarConfiguracionINPC(): " + ex.Message, ex.InnerException);
            }
        }
        /// <summary>
        /// Presenta en Pantalla la Configuracion de INPC
        /// </summary>
        public void PresentarConfiguracionINPC()
        {
            try
            {
                LlenarAnioContrato();
                var inpcContrato = this.vista.InpcContrato;
                if (inpcContrato == null)
                {
                    vista.PermitirControlesINPC(false);
                    vista.InpcFijo = null;
                    vista.FechaInicioInpc = null;
                    vista.ValorInpc = null;
                }
                else
                {
                    vista.InpcFijo = inpcContrato.Fijo;
                    vista.FechaInicioInpc = inpcContrato.FechaInicio;
                    vista.ValorInpc = inpcContrato.Valor;
                    vista.PermitirControlesINPC(inpcContrato.Fijo != null && inpcContrato.Fijo.Value);
                }
                vista.CambiarAINPC();
            }
            catch(Exception ex)
            {
                throw new Exception(nombreClase + ".PresentarConfiguracionINPC(): " + ex.Message, ex.InnerException);
            }
        }
        /// <summary>
        /// Valida si es correcta la configuracion del INPC
        /// </summary>
        /// <returns>Mensaje de Advertencia sobre la configuracion del INPC</returns>
        private string ValidarINPC()
        {
            string mensaje = "";

            if (vista.InpcFijo == null)
                return "Debe seleccionar el Tipo de INPC";
            if (vista.FechaInicioInpc == null)
                return "Se debe seleccionar la Fecha de Inicio para Aplicar el INPC";
            if (vista.InpcFijo.Value)
            {
                if (vista.ValorInpc == null)
                    return "Se debe colocar un valor para el INPC";
            }

            return mensaje;
        }
        /// <summary>
        /// Calcula la cantidad de Años del Contrato y los Presenta para Configurar el INPC
        /// </summary>
        private void LlenarAnioContrato()
        {
            var anios = datosRentaPRE.Vista.PlazoAnios;
            if (anios == 1)
                anios++;
            var listaAnios = new Dictionary<string, string>();
            for (var i = 1; i <= anios; i++)
                listaAnios.Add(i.ToString(),i.ToString());

            vista.PresentarAniosConfigurados(listaAnios);
        }
        #endregion
    }
}