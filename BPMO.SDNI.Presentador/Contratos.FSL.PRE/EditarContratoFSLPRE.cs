// Satisface al Caso de Uso CU023 - Editar Contrato Full Service Leasing
//Satisface al Caso de Uso CU026 - Registrar Terminación de Contrato Full Service Leasing
// Satisface a la solicitud de Cambio SC0001
// Satisface al caso de uso CU001 - Calcular Monto a Facturar FSL
// Satisface a la solución de la RI0008

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using BPMO.Basicos.BO;
using BPMO.Facade.SDNI.BR;
using BPMO.Patterns.Creational.DataContext;
using BPMO.Primitivos.Enumeradores;
using BPMO.SDNI.Comun.BO;
using BPMO.SDNI.Comun.BR;
using BPMO.SDNI.Comun.PRE;
using BPMO.SDNI.Contratos.BO;
using BPMO.SDNI.Contratos.BR;
using BPMO.SDNI.Contratos.FSL.BO;
using BPMO.SDNI.Contratos.FSL.BR;
using BPMO.SDNI.Contratos.FSL.VIS;
using BPMO.SDNI.Equipos.BO;
using BPMO.SDNI.Facturacion.AplicacionesPago.BR;

namespace BPMO.SDNI.Contratos.FSL.PRE
{
    /// <summary>
    /// Presentador de la Edicion de Contrato FSL
    /// </summary>
    public class EditarContratoFSLPRE
    {
        #region Atributos

        /// <summary>
        /// Vista sobre la que actua la interfaz
        /// </summary>
        private readonly IEditarContratoFSLVIS vista;

        /// <summary>
        /// El DataContext que proveerá acceso a la base de datos
        /// </summary>
        private readonly IDataContext dataContext;

        /// <summary>
        /// Nombre de la clase para agregar a los mensajes de Error
        /// </summary>
        private const string nombreClase = "EditarContratoFSLPRE";

        /// <summary>
        /// Presentador de la Barra de Herramientas
        /// </summary>
        private readonly ucHerramientasFSLPRE herramientasPRE;
        /// <summary>
        /// Presentador de la Información General
        /// </summary>
        private readonly ucInformacionGeneralPRE informacionGeneralPRE;
        /// <summary>
        /// Presentador de la Información del Cliente del Contrato
        /// </summary>
        private readonly ucClienteContratoPRE clienteContratoPRE;
        /// <summary>
        /// Presentador de los Datos de la Renta
        /// </summary>
        private readonly ucDatosRentaPRE datosRentaPRE;
        /// <summary>
        /// Presentador de la Linea de Contrato
        /// </summary>
        private readonly ucLineaContratoFSLPRE lineaContratoPRE;
        /// <summary>
        /// Presentador de la Informacion de Pago
        /// </summary>
        private readonly ucInformacionPagoPRE informacionPagoPRE;
        /// <summary>
        /// Presentador de Documentos Adjuntos al Contrato
        /// </summary>
        private readonly ucCatalogoDocumentosPRE documentosPRE;
        /// <summary>
        /// Presentador de los Datos Adicionales
        /// </summary>
        private readonly ucDatosAdicionalesAnexoPRE datosAdicionalesPRE;
        #endregion

        #region Constructores

        /// <summary>
        /// Constructor que recibe la vista sobre la que actuará el presentador
        /// </summary>
        /// <param name="vistaActual">vista sobre la que actuará el presentador</param>
        /// <param name="herramientas">Presentador de la barra de herramientas</param>
        /// <param name="general">Presentador de la Informacion General</param>
        /// <param name="cliente">Presentador de los datos del Cliente</param>
        /// <param name="datosRenta">Presentador de los datos de Renta</param>
        /// <param name="pago">Presentador de la informacion de Pago</param>
        /// <param name="lineaContrato">Presentador de las lineas de contrato</param>
        /// <param name="documentos">Presentador de las documentos</param>
        /// <param name="datosAdicionales">Presentador de los Datos Adicionales</param>
        public EditarContratoFSLPRE(IEditarContratoFSLVIS vistaActual, ucHerramientasFSLPRE herramientas,
                                    ucInformacionGeneralPRE general, ucClienteContratoPRE cliente,
                                    ucDatosRentaPRE datosRenta, ucInformacionPagoPRE pago,
                                    ucLineaContratoFSLPRE lineaContrato, ucCatalogoDocumentosPRE documentos, ucDatosAdicionalesAnexoPRE datosAdicionales)
        {
            if (vistaActual != null)
                vista = vistaActual;

            dataContext = Facade.SDNI.BR.FacadeBR.ObtenerConexion();

            try
            {
                vista = vistaActual;
                informacionGeneralPRE = general;
                clienteContratoPRE = cliente;
                datosRentaPRE = datosRenta;
                lineaContratoPRE = lineaContrato;
                informacionPagoPRE = pago;
                documentosPRE = documentos;
                herramientasPRE = herramientas;
                datosAdicionalesPRE = datosAdicionales;
            }
            catch (Exception ex)
            {
                throw new Exception(nombreClase + ".EditarContratoFSLPRE: " + ex.Message);
            }
        }

        #endregion

        #region Metodos

        /// <summary>
        /// Inicializa la vista para editar un contrato
        /// </summary>
        public void Inicializar()
        {
            try
            {
                informacionGeneralPRE.Inicializar();
                informacionPagoPRE.Inicializar();
                clienteContratoPRE.Inicializar();
                datosRentaPRE.Inicializar();
                herramientasPRE.Inicializar();
                datosAdicionalesPRE.Inicializar();

                vista.CodigoUltimoObjeto = "UltimoContratoFSLBO";

                DesplegarTiposArchivos();

                ContratoFSLBO contrato = vista.UltimoObjeto;

                informacionGeneralPRE.Vista.ConfigurarModoEditar();
                informacionPagoPRE.Vista.ConfigurarModoEditar();
                clienteContratoPRE.Vista.ConfigurarModoEditar();
                datosRentaPRE.Vista.ConfigurarModoEditar();
                lineaContratoPRE.Vista.ConfigurarModoEditar();
                documentosPRE.ModoEditable(true);
                datosAdicionalesPRE.Vista.ConfigurarModoEditar(); //SC0007

                if (contrato != null && contrato.DocumentosAdjuntos != null && contrato.DocumentosAdjuntos.Count > 0)
                    contrato.DocumentosAdjuntos =
                        contrato.DocumentosAdjuntos.Where(archivo => archivo.Activo == true).ToList();

                if (contrato.Cliente != null)
                {
                    clienteContratoPRE.Vista.HabilitarConsultaDireccionCliente(contrato.Cliente.Id != null);
                }
                else
                    clienteContratoPRE.Vista.HabilitarConsultaDireccionCliente(false);

                DatosAInterfazUsuario(contrato);

                #region SC0002
                herramientasPRE.vista.HabilitarOpcionesEdicion();
                herramientasPRE.vista.DeshabilitarOpcionesAgregarDoc();
                #endregion
                herramientasPRE.vista.MarcarOpcionEditarContrato();
                herramientasPRE.vista.OcultarFormatosContrato();

                this.EstablecerSeguridad();//SC_0008
            }
            catch (Exception ex)
            {
                throw new Exception(nombreClase + ".Inicializar: " + ex.Message);
            }
        }

        /// <summary>
        /// Despliega los datos del contrato en la interfaz de usuario
        /// </summary>
        /// <param name="contrato">Contrato que contiene los datos</param>
        public void DatosAInterfazUsuario(ContratoFSLBO contrato)
        {
            try
            {
                vista.ContratoID = contrato.ContratoID;
                vista.Estatus = contrato.Estatus;
                if (contrato.Sucursal != null && contrato.Sucursal.UnidadOperativa != null)
                    vista.UnidadOperativaContratoID = contrato.Sucursal.UnidadOperativa.Id;
                herramientasPRE.DatosAInterfazUsuario(contrato);
                informacionGeneralPRE.DatosAInterfazUsuario(contrato);
                clienteContratoPRE.DatosAInterfazUsuario(contrato);
                datosRentaPRE.DatosAInterfazUsuario(contrato);
                informacionPagoPRE.DatosAInterfazUsuario(contrato);
                documentosPRE.CargarListaArchivos(contrato.DocumentosAdjuntos, documentosPRE.Vista.TiposArchivo);
                #region CU026
                if (contrato.CierreContrato == null)
                    contrato.CierreContrato = new CierreAnticipadoContratoFSLBO();
                vista.FechaCierre = contrato.CierreContrato.Fecha;
                vista.UsuarioCierre = contrato.CierreContrato.Usuario == null ? null : contrato.CierreContrato.Usuario.Id;
                vista.ObservacionesCierre = contrato.CierreContrato.Observaciones;
                vista.CantidadPenalizacion = ((CierreAnticipadoContratoFSLBO)contrato.CierreContrato).CantidadPenalizacion;
                vista.MotivoCierre = ((CierreAnticipadoContratoFSLBO)contrato.CierreContrato).Motivo;
                if (vista.Estatus == EEstatusContrato.Borrador)
                {
                    informacionGeneralPRE.DesplegarConfiguracionUnidadOperativa();
                }
                #endregion

                this.vista.UC = contrato.UC;
                this.vista.FC = contrato.FC;

                datosAdicionalesPRE.DatosAInterfazUsuario(contrato);

                INPCContratoBO inpc = new INPCContratoBO(){Auditoria = new AuditoriaBO()};
                if (contrato.InpcContrato != null)
                {
                    inpc.Fijo = contrato.InpcContrato.Fijo;
                    inpc.FechaInicio = contrato.InpcContrato.FechaInicio;
                    inpc.Valor = contrato.InpcContrato.Valor;
                    inpc.Auditoria.FC = contrato.InpcContrato.Auditoria.FC;
                    inpc.Auditoria.UC = contrato.InpcContrato.Auditoria.UC;
                    inpc.Auditoria.FUA = contrato.InpcContrato.Auditoria.FUA;
                    inpc.Auditoria.UUA = contrato.InpcContrato.Auditoria.UUA;
                }
                vista.InpcContrato = inpc;
            }
            catch (Exception ex)
            {
                throw new Exception(nombreClase + ".DatosAInterfazUsuario:" + ex.Message);
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

                var tipo = new TipoArchivoBO();

                vista.CargarTiposArchivos(tipoBR.Consultar(dataContext, tipo));
            }
            catch (Exception ex)
            {
                throw new Exception(nombreClase + ".DesplegarTiposArchivos:" + ex.Message);
            }
        }

        /// <summary>
        /// Calcula y despliega la fecha de terminación del contrato
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
        /// Prepara una nueva linea de contrato
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
                                        UnidadOperativa = new UnidadOperativaBO { Id = vista.UnidadOperativaContratoID }
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
        /// Guarda el Contrato con estatus Borrador
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
                //SC_0008
                SeguridadBO seguridadBO = new SeguridadBO(Guid.Empty, new UsuarioBO { Id = this.vista.UsuarioID }, new AdscripcionBO
                {
                    Departamento = new DepartamentoBO(),
                    Sucursal = new SucursalBO(),
                    UnidadOperativa = new UnidadOperativaBO { Id = this.vista.UnidadAdscripcionID }
                });
                //End SC_0008
                ContratoFSLBO contrato = InterfazUsuarioADatos();
                // Se actualiza el Contrato
                var contratoBR = new ContratoFSLBR();
                contratoBR.ActualizarCompleto(dataContext, contrato, vista.UltimoObjeto, seguridadBO);
                vista.EstablecerPaqueteNavegacion(vista.Codigo, contrato);
                vista.IrADetalleContrato();
                vista.MostrarMensaje("Se ha guardado el borrador del contrato exitosamente.", ETipoMensajeIU.EXITO);
            }
            catch (Exception ex)
            {
                throw new Exception(nombreClase + ".GuardarContratoBorrador:" + ex.Message);
            }
        }

        /// <summary>
        /// Guarda el contrato con Estatus En Curso
        /// </summary>
        public void GuardarContratoTermino()
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
                throw new Exception("Se encontraron inconsistencias al editar el Contrato.");
            }
            #endregion

            try
            {
                //SC_0008
                SeguridadBO seguridadBO = new SeguridadBO(Guid.Empty, new UsuarioBO { Id = this.vista.UsuarioID }, new AdscripcionBO
                {
                    Departamento = new DepartamentoBO(),
                    Sucursal = new SucursalBO(),
                    UnidadOperativa = new UnidadOperativaBO { Id = this.vista.UnidadAdscripcionID }
                });
                //End SC_0008
                ContratoFSLBO contrato = InterfazUsuarioADatos();
                contrato.Estatus = EEstatusContrato.EnCurso;

                var contratoBR = new ContratoFSLBR();

                contratoBR.ActualizarCompleto(dataContext, contrato, vista.UltimoObjeto, seguridadBO);

                #region SC0001 BEP1401 - Registra los pagos del Contrato

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

                vista.MostrarMensaje("Se ha guardado el borrador del contrato exitosamente.", ETipoMensajeIU.EXITO);
                vista.EstablecerPaqueteNavegacion(vista.Codigo, contrato);
                vista.IrADetalleContrato();
            }
            catch (Exception ex)
            {
                dataContext.RollbackTransaction(firma);
                throw new Exception(nombreClase + ".GuardarContratoTermino:" + ex.Message);
            }
            finally
            {
                if(dataContext.ConnectionState == ConnectionState.Open)
                    dataContext.CloseConnection(firma);
            }
        }

        /// <summary>
        /// Regresar a la pantalla de Detalles
        /// </summary>
        public void RegresarADetalles()
        {
            try
            {
                vista.EstablecerPaqueteNavegacion(vista.Codigo, vista.UltimoObjeto);
                vista.LimpiarSesion();
                vista.IrADetalleContrato();
            }
            catch (Exception ex)
            {
                throw new Exception(nombreClase + ".RegresarADetalles: " + ex.Message);
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
            if (informacionGeneralPRE.Vista.FechaContrato != null && informacionPagoPRE.Vista.FechaInicioContrato != null)
            {
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
            if (String.IsNullOrEmpty(clienteContratoPRE.Vista.Direccion))
            {
                mensaje += "Dirección del cliente, ";
            }
            else
            {
                if (String.IsNullOrEmpty(clienteContratoPRE.Vista.Calle))
                    mensaje += "Calle en la dirección, ";
                if (String.IsNullOrEmpty(clienteContratoPRE.Vista.Ciudad))
                    mensaje += "Ciudad en la dirección, ";
                if (String.IsNullOrEmpty(clienteContratoPRE.Vista.CodigoPostal))
                    mensaje += "Código Postal en la dirección, ";
                if (String.IsNullOrEmpty(clienteContratoPRE.Vista.Colonia))
                    mensaje += "Colonia en la dirección, ";
                if (String.IsNullOrEmpty(clienteContratoPRE.Vista.Estado))
                    mensaje += "Estado en la dirección, ";
                if (String.IsNullOrEmpty(clienteContratoPRE.Vista.Municipio))
                    mensaje += "Municipio en la dirección, ";
                if (String.IsNullOrEmpty(clienteContratoPRE.Vista.Pais))
                    mensaje += "País en la dirección, ";
            }
            if (clienteContratoPRE.Vista.EsFisico != true && clienteContratoPRE.Vista.RepresentantesLegalesContrato.Count == 0) mensaje += " Representantes Legales, ";

            if (clienteContratoPRE.Vista.EsFisico == true && clienteContratoPRE.Vista.ObligadosSolidariosContrato.Count == 0) mensaje += " Obligados Solidarios, ";

			if (clienteContratoPRE.Vista.SoloRepresentantes != true)
				if (clienteContratoPRE.Vista.ObligadosComoAvales != true)
					if (clienteContratoPRE.Vista.AvalesSeleccionados==null || clienteContratoPRE.Vista.AvalesSeleccionados.Count == 0) mensaje += " Avales, ";

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
            //if (informacionPagoPRE.Vista.CuentaBancariaSeleccionada == null) mensaje += " Cuenta Bancaria, ";
            #endregion

            if (!string.IsNullOrEmpty(mensaje))
                return "Los siguientes campos no pueden estar vacíos: \n" + mensaje.Substring(0, mensaje.Length - 2);
            #region RI0057
            if (informacionGeneralPRE.Vista.FechaContrato != null && informacionPagoPRE.Vista.FechaInicioContrato != null)
            {
                if (DateTime.Compare((DateTime)informacionGeneralPRE.Vista.FechaContrato, (DateTime)informacionPagoPRE.Vista.FechaInicioContrato) > 0)
                    mensaje += "La fecha de contrato no puede ser mayor que la fecha de inicio";
            }
            #endregion
            return mensaje;
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
                        ContratoID = vista.ContratoID,
                        #region Cuenta Bancaria
                        //Banco = informacionPagoPRE.Vista.CuentaBancariaSeleccionada,
                        #endregion
                        Cliente = new CuentaClienteIdealeaseBO
                        {
                            Id = clienteContratoPRE.Vista.CuentaClienteID,
                            Nombre = clienteContratoPRE.Vista.NombreCuentaCliente,
                            Cliente = new ClienteBO { Id = clienteContratoPRE.Vista.ClienteID, Fisica = clienteContratoPRE.Vista.EsFisico },
                            UnidadOperativa = clienteContratoPRE.Vista.UnidadOperativa 
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
                        PorcentajePenalizacion = informacionGeneralPRE.Vista.PorcentajePenalizacion,
                        #region CU026
                        CierreContrato = new CierreAnticipadoContratoFSLBO { Fecha = vista.FechaCierre, Observaciones = vista.ObservacionesCierre, Usuario = new UsuarioBO { Id = vista.UsuarioCierre }, CantidadPenalizacion = vista.CantidadPenalizacion, Motivo = vista.MotivoCierre }
                        #endregion
                    };

				contrato.ObligadosComoAvales = clienteContratoPRE.Vista.ObligadosComoAvales;
				if (clienteContratoPRE.Vista.AvalesSeleccionados != null)
					contrato.Avales = clienteContratoPRE.Vista.AvalesSeleccionados;
				if (contrato.ObligadosComoAvales != null && contrato.ObligadosComoAvales == true)
				{
					if (clienteContratoPRE.Vista.ObligadosSolidariosContrato != null)
						contrato.Avales = clienteContratoPRE.Vista.ObligadosSolidariosContrato.ConvertAll(s => clienteContratoPRE.ObligadoAAval(s));
				}

                List<ArchivoBO> adjuntos = documentosPRE.Vista.ObtenerArchivos();
                if (adjuntos == null) adjuntos = new List<ArchivoBO>();
                foreach (ArchivoBO adjuntoContratoBo in adjuntos)
                {
                    adjuntoContratoBo.TipoAdjunto = ETipoAdjunto.ContratoFSL;
                }
                contrato.DocumentosAdjuntos = adjuntos;

                // Se Agregan datos de Auditoria
                contrato.FC = this.vista.FC;
                contrato.UC = this.vista.UC;
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

                if (vista.InpcContrato != null)
                {
                    contrato.InpcContrato = new INPCContratoBO(vista.InpcContrato);
                    if(contrato.InpcContrato.Auditoria.FC == null)
                        contrato.InpcContrato.Auditoria.FC = vista.FUA;
                    if(contrato.InpcContrato.Auditoria.UC == null)
                        contrato.InpcContrato.Auditoria.UC = vista.UUA;
                    contrato.InpcContrato.Auditoria.FUA = vista.FUA;
                    contrato.InpcContrato.Auditoria.UUA = vista.UUA;
                }

                return contrato;
            }
            catch (Exception ex)
            {
                throw new Exception(nombreClase + ".InterfazUsuarioADatos: " + ex.Message);
            }
        }

        #region SC_0008
        /// <summary>
        /// Valida el acceso a la página de edición
        /// </summary>
        public void ValidarAcceso()
        {
            try
            {
                //se valida que los datos del usuario y la unidad operativa no sean nulos
                if (this.vista.UsuarioID == null) throw new Exception("El identificador del usuario no debe ser nulo");
                if (this.vista.UnidadAdscripcionID == null) throw new Exception("El identificador de la Unidad Operativa no debe ser nulo ");

                UsuarioBO usr = new UsuarioBO { Id = this.vista.UsuarioID };
                AdscripcionBO adscripcion = new AdscripcionBO { UnidadOperativa = new UnidadOperativaBO { Id = this.vista.UnidadAdscripcionID } };
                SeguridadBO seguridadBo = new SeguridadBO(Guid.Empty, usr, adscripcion);

                if (!FacadeBR.ExisteAccion(dataContext, "ACTUALIZARCOMPLETO", seguridadBo))
                    this.vista.RedirigirSinPermisoAcceso();

                #region RI0008
                //Se valida si el usuario tiene permiso para registrar contrato en curso
                if (!FacadeBR.ExisteAccion(dataContext, "GENERARPAGOS", seguridadBo))
                    this.vista.PermitirGuardarEnCurso(false);
                #endregion
            }
            catch (Exception ex)
            {
                throw new Exception(nombreClase + ".ValidarAcceso:" + ex.Message);
            }
        }
        /// <summary>
        /// Permite configurar el comportamiento de los controles de la vista de acuerdo a los permisos configurados
        /// </summary>
        private void EstablecerSeguridad()
        {
            try
            {
                //se valida que los datos del usuario y la unidad operativa no sean nulos
                if (this.vista.UsuarioID == null) throw new Exception("El identificador del usuario no debe ser nulo");
                if (this.vista.UnidadAdscripcionID == null) throw new Exception("El identificador de la Unidad Operativa no debe ser nulo ");

                //Se crea el objeto de seguridad
                UsuarioBO usr = new UsuarioBO { Id = this.vista.UsuarioID };
                AdscripcionBO adscripcion = new AdscripcionBO { UnidadOperativa = new UnidadOperativaBO { Id = this.vista.UnidadAdscripcionID } };
                SeguridadBO seguridadBO = new SeguridadBO(Guid.Empty, usr, adscripcion);

                //Se obtiene las acciones a las cuales el usuario tiene permiso en este proceso
                List<CatalogoBaseBO> lst = FacadeBR.ConsultarAccion(this.dataContext, seguridadBO);

                if (!this.ExisteAcccion(lst, "ACTUALIZARCOMPLETO"))
                    this.vista.RedirigirSinPermisoAcceso();
                if (!this.ExisteAcccion(lst, "UI INSERTAR"))
                    this.vista.PermitirRegistrar(false);
                if (!this.ExisteAcccion(lst, "ACTUALIZARDOCUMENTO"))
                    this.PermitirActualizarDocumento();
                #region RI0008
                //Se valida si el usuario tiene permiso para registrar contrato en curso
                if (!this.ExisteAcccion(lst, "GENERARPAGOS"))
                    this.vista.PermitirGuardarEnCurso(false); 
                #endregion
            }
            catch (Exception ex)
            {
                throw new Exception(nombreClase + ".EstablecerSeguridad:" + ex.Message);
            }
        }
        /// <summary>
        /// Valida una acción en especifico dentro de la lista de acciones permtidas para la pagina
        /// </summary>
        /// <param name="acciones">Listado de acciones permitidas para la página</param>
        /// <param name="accion">Acción que se desea validar</param>
        /// <returns>si la acción a evaluar se encuantra dentro de la lista de acciones permitidas se devuelve true. En caso ocntario false. bool</returns>
        private bool ExisteAcccion(List<CatalogoBaseBO> acciones, string accion)
        {
            if (acciones != null && acciones.Count > 0 && acciones.Exists(p => p != null && p.Nombre != null && p.Nombre.Trim().ToUpper() == accion.Trim().ToUpper()))
                return true;

            return false;
        }

        /// <summary>
        /// Deshabilita la opción de agergar docuemntos de la barra de herramientas de acuerdo a los permisos configurados
        /// </summary>
        private void PermitirActualizarDocumento()
        {
            this.herramientasPRE.DeshabilitarMenuDocumentos();
        }
        #endregion

        #region SC0038
        /// <summary>
        /// Despliega en la vista las plantillas correspondientes al modulo
        /// </summary>
        public void CargarPlantillas()
        {
            var controlador = new PlantillaBR();

            var precargados = this.vista.ObtenerPlantillas("ucContratosFSL");
            var resultado = new List<object>();

            if (precargados != null)
                if (precargados.Count > 0)
                    resultado = precargados;

            if (resultado.Count <= 0)
            {
                var lista = controlador.Consultar(this.dataContext, new PlantillaBO { Activo = true, TipoPlantilla = EModulo.FSL });

                if (ReferenceEquals(lista, null))
                    lista = new List<PlantillaBO>();

                resultado = lista.ConvertAll(p => (object)p);
            }

            this.herramientasPRE.CargarArchivos(resultado);
        }
        #endregion

        /// <summary>
        /// Activa o desactiva controles de acuerdo a la seleccion del Tipo de INPC
        /// </summary>
        public void CambiarSeleccionTipoINPC()
        {
            try
            {
                if(vista.InpcFijo == null)
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
            catch(Exception ex)
            {
                throw new Exception(nombreClase + ".CambiarSeleccionTipoINPC(): " + ex.Message, ex.InnerException);
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
                if(guardar)
                {
                    string s = ValidarINPC();
                    if(!String.IsNullOrEmpty(s))
                    {
                        vista.MostrarMensaje(s, ETipoMensajeIU.ADVERTENCIA);
                        return;
                    }

                    var inpcContrato = this.vista.InpcContrato;
                    if(inpcContrato == null)
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
            catch(Exception ex)
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
                if(inpcContrato == null)
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

            if(vista.InpcFijo == null)
                return "Debe seleccionar el Tipo de INPC";
            if(vista.FechaInicioInpc == null)
                return "Se debe seleccionar la Fecha de Inicio para Aplicar el INPC";
            if(vista.InpcFijo.Value)
            {
                if(vista.ValorInpc == null)
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
            if(anios == 1)
                anios++;
            var listaAnios = new Dictionary<string, string>();
            for(var i = 1; i <= anios; i++)
                listaAnios.Add(i.ToString(), i.ToString());

            vista.PresentarAniosConfigurados(listaAnios);
        }
        #endregion
    }
}
