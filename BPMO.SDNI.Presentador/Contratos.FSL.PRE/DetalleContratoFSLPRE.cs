// Satisface al Caso de uso CU022 - Consultar Contrato Full Service Leasing
// Satisface al Caso de uso CU023 - Editar Contrato Full Service Leasing
// Satisface al CU026 - Registrar Terminación de Contrato Full Service Leasing
//Satisface al caso de uso CU093 - Imprimir Pagaré Contrato FSL
using System;
using System.Collections.Generic;
using System.Linq;
using BPMO.Basicos.BO;
using BPMO.Facade.SDNI.BR;
using BPMO.Patterns.Creational.DataContext;
using BPMO.Primitivos.Enumeradores;
using BPMO.SDNI.Contratos.BO;
using BPMO.SDNI.Contratos.BR;
using BPMO.SDNI.Contratos.FSL.BO;
using BPMO.SDNI.Contratos.FSL.BR;
using BPMO.SDNI.Contratos.FSL.VIS;

namespace BPMO.SDNI.Contratos.FSL.PRE
{
    public class DetalleContratoFSLPRE
    {
        #region Atributos
        /// <summary>
        /// Vista sobre la que actua el presentador
        /// </summary>
        private readonly IDetalleContratoFSLVIS vista;
        /// <summary>
        /// El DataContext que proveerá acceso a la base de datos
        /// </summary>
        private readonly IDataContext dataContext;
        /// <summary>
        /// Nombre de la clase para agregar a los mensajes de Error
        /// </summary>
        private const string nombreClase = "DetalleContratoFSLPRE";
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

        private readonly ucDatosAdicionalesAnexoPRE datosAdicionalesPRE; //SC0007
        #endregion Atributos

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
        /// <param name="datosAdicionales">Presentador de los datos Adicionales</param>
        public DetalleContratoFSLPRE(IDetalleContratoFSLVIS vistaActual, ucHerramientasFSLPRE herramientas,
                                    ucInformacionGeneralPRE general, ucClienteContratoPRE cliente,
                                    ucDatosRentaPRE datosRenta, ucInformacionPagoPRE pago,
                                    ucLineaContratoFSLPRE lineaContrato, ucDatosAdicionalesAnexoPRE datosAdicionales)
        {
            if (vistaActual != null)
                vista = vistaActual;

            dataContext = Facade.SDNI.BR.FacadeBR.ObtenerConexion();

            herramientasPRE = herramientas;
            informacionGeneralPRE = general;
            clienteContratoPRE = cliente;
            datosRentaPRE = datosRenta;
            lineaContratoPRE = lineaContrato;
            informacionPagoPRE = pago;
            datosAdicionalesPRE = datosAdicionales;
        }


        #endregion

        #region Metodos
        /// <summary>
        /// Inicializa el Detalla del Contrato para visualizacion
        /// </summary>
        public void Inicializar()
        {
            informacionGeneralPRE.Inicializar();
            informacionPagoPRE.Inicializar();
            clienteContratoPRE.Inicializar();
            datosRentaPRE.Inicializar();
            herramientasPRE.Inicializar();
            datosAdicionalesPRE.Inicializar(); //SC0007            
            DatosAIntefazUsuario();
            vista.InicializarControles();
            this.EstablecerSeguridad();//SC_0008
        }
        /// <summary>
        /// Obtiene el Contrato del Paquete de Navegacion
        /// </summary>
        /// <returns></returns>
        private ContratoFSLBO ObtenerContrato()
        {
            try
            {
                var contrato = vista.Contrato;

                var contratoBR = new ContratoFSLBR();

                List<ContratoFSLBO> resultados = contratoBR.ConsultarDetalle(dataContext, contrato, false);

                contrato = resultados.Find(cont => cont.ContratoID == contrato.ContratoID);

                if (contrato != null && contrato.DocumentosAdjuntos != null && contrato.DocumentosAdjuntos.Count > 0)
                    contrato.DocumentosAdjuntos = contrato.DocumentosAdjuntos.Where(archivo => archivo.Activo == true).ToList();


                return contrato;
            }
            catch (Exception ex)
            {
                throw new Exception(nombreClase + ".ObtenerContrato: " + ex.Message);
            }
        }
        /// <summary>
        /// Despliega los datos en la Interfaz de Usuario
        /// </summary>
        private void DatosAIntefazUsuario()
        {
            ContratoFSLBO contrato = ObtenerContrato();
            if (contrato.Sucursal == null) contrato.Sucursal = new SucursalBO();
            if (contrato.Sucursal.UnidadOperativa == null) contrato.Sucursal.UnidadOperativa = new UnidadOperativaBO();

            vista.Contrato = contrato;
            vista.ContratoID = contrato.ContratoID;
            vista.EstatusContrato = contrato.Estatus;
            vista.UnidadOperativaContratoID = contrato.Sucursal.UnidadOperativa.Id;

            informacionGeneralPRE.DatosAInterfazUsuario(contrato);
            informacionPagoPRE.DatosAInterfazUsuario(contrato);
            clienteContratoPRE.DatosAInterfazUsuario(contrato);
            datosRentaPRE.DatosAInterfazUsuario(contrato);
            herramientasPRE.DatosAInterfazUsuario(contrato);

            datosAdicionalesPRE.DatosAInterfazUsuario(contrato); //SC0007

            vista.DatosAInterfazUsuario(contrato);

            if (contrato.InpcContrato != null)
            {
                if (contrato.InpcContrato.Fijo != null)
                    vista.InpcFijo = contrato.InpcContrato.Fijo;
                if (contrato.InpcContrato.FechaInicio != null)
                    vista.FechaInicioINPC = contrato.InpcContrato.FechaInicio;
                if (contrato.InpcContrato.Valor != null)
                    vista.ValorInpc = contrato.InpcContrato.Valor;
            }
        }
        /// <summary>
        /// Prepara la Linea de contrato para visualizacion
        /// </summary>
        /// <param name="linea">Linea de Contrato que contiene los datos a mostrar</param>
        public void PrepararLinea(LineaContratoFSLBO linea)
        {
            if (linea != null)
            {
                if (vista.Contrato.Plazo != null && vista.Contrato.Plazo > 0)
                {
                    lineaContratoPRE.EstablecerUltimoObjeto(linea);
                    lineaContratoPRE.DatosAInterfazUsuario(linea, vista.Contrato.CalcularPlazoEnAños(), vista.Contrato.IncluyeSeguro);
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
        /// <summary>
        /// Elimina la linea de contrato desplegada en la Interfaz de Usuario (Con estatus Borrador)
        /// </summary>
        public void EliminarContrato()
        {
            try
            {
                ContratoFSLBO contratoEliminar = InterfazUsuarioADatos();

                if (contratoEliminar.Estatus != EEstatusContrato.Borrador)
                {
                    vista.MostrarMensaje("El contrato debe tener estatus Borrador para ser eliminado.", ETipoMensajeIU.INFORMACION);
                    return;
                }

                contratoEliminar.FUA = vista.FUA;
                contratoEliminar.UUA = vista.UUA;

                var contratoBR = new ContratoFSLBR();
                //SC_0008
                SeguridadBO seguridadBO = new SeguridadBO(Guid.Empty, new UsuarioBO { Id = this.vista.UsuarioID }, new AdscripcionBO
                {
                    Departamento = new DepartamentoBO(),
                    Sucursal = new SucursalBO(),
                    UnidadOperativa = new UnidadOperativaBO { Id = this.vista.UnidadAdscripcionID }
                });
                //END SC_0008
                contratoBR.BorrarCompleto(dataContext, contratoEliminar, seguridadBO);

                vista.MostrarMensaje("El contrato ha sido eliminado exitosamente.", ETipoMensajeIU.EXITO);

                vista.RegresarAConsultar();
            }
            catch (Exception ex)
            {
                vista.MostrarMensaje("Inconsistencias al intentar eliminar el contrato", ETipoMensajeIU.ERROR, nombreClase + ".EliminarContrato: " + ex.Message);
            }
        }
        /// <summary>
        /// Pasa los datos de la Interfaz de Usuario a un Contrato
        /// </summary>
        /// <returns></returns>
        private ContratoFSLBO InterfazUsuarioADatos()
        {
            return new ContratoFSLBO
                {
                    ContratoID = vista.ContratoID,
                    Estatus = vista.EstatusContrato,
                    Sucursal = new SucursalBO { UnidadOperativa = new UnidadOperativaBO { Id = vista.UnidadOperativaContratoID } }
                };
        }
        /// <summary>
        /// Envia el contrato a Editar Contrato
        /// </summary>
        public void EditarContrato()
        {
            try
            {
                ContratoFSLBO contrato = InterfazUsuarioADatos();
                vista.Contrato = contrato;
                contrato = ObtenerContrato();
                vista.EstablecerPaqueteNavegacionEditar("UltimoContratoFSLBO", contrato);
                vista.IrAEditar();
            }
            catch (Exception ex)
            {
                vista.MostrarMensaje("Inconsistencias al intentar editar el contrato.", ETipoMensajeIU.ERROR, nombreClase + ".EditarContrato: " + ex.Message);
            }
        }
        /// <summary>
        /// Obtiene el Contrato presentado y se redirige a la interfaz de Modificacion de Lineas
        /// </summary>
        public void ModificarUnidadesContrato()
        {
            try
            {
                ContratoFSLBO contrato = InterfazUsuarioADatos();
                vista.EstablecerPaqueteNavegacionEditar("ContratoFSLEditar", contrato);
                vista.IrAModificarLineas();
            }
            catch(Exception ex)
            {
                vista.MostrarMensaje("Inconsistencias al intentar Modificar Lineas del contrato.", ETipoMensajeIU.ERROR, nombreClase + ".ModificarUnidadesContrato: " + ex.Message);
            }
        }

        #region SC0002
        /// <summary>
        /// Envia el contrato a Agregar Documentos
        /// </summary>
        public void AgregarDocumentos()
        {
            try
            {
                ContratoFSLBO contrato = InterfazUsuarioADatos();
                vista.Contrato = contrato;
                contrato = ObtenerContrato();
                vista.EstablecerPaqueteNavegacionEditar("UltimoContratoFSLBO", contrato);
                vista.IrAAgregarDocs();
            }
            catch (Exception ex)
            {
                vista.MostrarMensaje("Inconsistencias al intentar agregar documentos al contrato.", ETipoMensajeIU.ERROR, nombreClase + ".AgregarDocumentos: " + ex.Message);
            }
        }
        #endregion

        /// <summary>
        /// Impresion de la Constancia de Entrega de Bienes
        /// </summary>
        public void ImprimirConstanciaBienes()
        {
            try
            {

                if (vista.ContratoID != null)
                {
                    var contratoBR = new ContratoFSLBR();
                    var DatosReporte = contratoBR.ConsultarConstanciaEntregaBienes(dataContext, vista.ContratoID.Value);
                    vista.EstablecerPaqueteNavegacionImprimir("CU016", DatosReporte);
                    vista.IrAImprimir();
                    return;
                }
                vista.MostrarMensaje("No se cuenta con el Identificador del Contrato", ETipoMensajeIU.ADVERTENCIA);

            }
            catch (Exception ex)
            {
                vista.MostrarMensaje("Inconsistencias al Intentar  desplegar la Constancia de Bienes", ETipoMensajeIU.ERROR, nombreClase + ".ImprimirConstanciaBienes: " + ex.Message);
            }
        }
        /// <summary>
        /// Impresion del Manual de Operaciones
        /// </summary>
        public void ImprimirManualOperaciones()
        {
            try
            {

                if (vista.ContratoID != null)
                {
                    if (vista.UnidadOperativaContratoID != null)
                    {
                        var contratoBR = new ContratoFSLBR();
                        var DatosReporte = contratoBR.ConsultarManualOperaciones(dataContext, vista.ContratoID.Value,
                                                                                 vista.UnidadOperativaContratoID.Value);
                        vista.EstablecerPaqueteNavegacionImprimir("CU017", DatosReporte);
                        vista.IrAImprimir();

                    }
                    else
                        vista.MostrarMensaje("No se cuenta con la Unidad Operativa del Contrato", ETipoMensajeIU.ADVERTENCIA);
                }
                else
                    vista.MostrarMensaje("No se cuenta con el Identificador del Contrato", ETipoMensajeIU.ADVERTENCIA);

            }
            catch (Exception ex)
            {
                vista.MostrarMensaje("Inconsistencias al Intentar  desplegar el Manual de Operaciones", ETipoMensajeIU.ERROR, nombreClase + ".ImprimirManualOperaciones: " + ex.Message);
            }
        }
        /// <summary>
        /// Impresion del Anexo A
        /// </summary>
        public void ImprimirAnexoA()
        {
            try
            {

                if (vista.ContratoID != null)
                {
                    var contratoBR = new ContratoFSLBR();
                    var DatosReporte = contratoBR.ObtenerDatosAnexoA(dataContext, vista.ContratoID.Value);
                    vista.EstablecerPaqueteNavegacionImprimir("CU019", DatosReporte);
                    vista.IrAImprimir();
                }
                else
                    vista.MostrarMensaje("No se cuenta con el Identificador del Contrato", ETipoMensajeIU.ADVERTENCIA);

            }
            catch (Exception ex)
            {
                vista.MostrarMensaje("Inconsistencias al Intentar  desplegar el Anexo A", ETipoMensajeIU.ERROR, nombreClase + ".ImprimirAnexoA: " + ex.Message);
            }
        }
        /// <summary>
        /// Impresion del Anexo C
        /// </summary>
        public void ImprimirAnexoC()
        {
            try
            {

                if (vista.Contrato != null)
                {
                    var contratoBR = new ContratoFSLBR();
                    var DatosReporte = contratoBR.ObtenerDatosAnexoC(dataContext, vista.Contrato);
                    vista.EstablecerPaqueteNavegacionImprimir("CU021", DatosReporte);
                    vista.IrAImprimir();
                }
                else
                    vista.MostrarMensaje("No se cuenta con información necesaria para imprimir el Anexo C", ETipoMensajeIU.ADVERTENCIA);

            }
            catch (Exception ex)
            {
                vista.MostrarMensaje("Inconsistencias al Intentar  desplegar el Anexo C", ETipoMensajeIU.ERROR, nombreClase + ".ImprimirAnexoC: " + ex.Message);
            }
        }
        /// <summary>
        /// Impresion del Contrato Maestro
        /// </summary>
        public void ImprimirContratoMaestro()
        {
            try
            {

                if (vista.Contrato != null)
                {
                    var contratoBR = new ContratoFSLBR();
                    var DatosReporte = contratoBR.ObtenerDatosContratoMaestro(dataContext, vista.Contrato);
                    vista.EstablecerPaqueteNavegacionImprimir("CU018", DatosReporte);
                    vista.IrAImprimir();
                }
                else
                    vista.MostrarMensaje("No se cuenta con la información necesaria del para imprimir el Contrato Maestro", ETipoMensajeIU.ADVERTENCIA);

            }
            catch (Exception ex)
            {
                vista.MostrarMensaje("Inconsistencias al Intentar  desplegar el Contrato Maestro", ETipoMensajeIU.ERROR, nombreClase + ".ImprimirContratoMaestro: " + ex.Message);
            }
        }
        /// <summary>
        /// Imprimir la plantilla del contrato maestro
        /// </summary>
        /// <param name="tipo">//True si quieres que sea fisica false si squieres que sea moral. bool</param>
        public void ImprimirPlantillaContratoMaestro(bool tipo)
        {
            try
            {
                var contratoBR = new ContratoFSLBR();
                //True si quieres que sea fisica false si squieres que sea moral
                var datos = contratoBR.ObtenerPlantillaContratoMaestro(tipo);
                vista.EstablecerPaqueteNavegacionImprimir("CU018A", datos);
                vista.IrAImprimir();
            }
            catch (Exception ex)
            {
                vista.MostrarMensaje("Inconsistencias al Intentar  desplegar el Contrato Maestro", ETipoMensajeIU.ERROR, nombreClase + ".ImprimirPlantillaContratoMaestro: " + ex.Message);
            }
        }
        /// <summary>
        /// Impresion de los Anexos del Contrato
        /// </summary>
        public void ImprimirAnexosContrato()
        {
            try
            {

                if (vista.ContratoID != null)
                {
                    if (vista.Contrato != null)
                    {
                        var contratoBR = new ContratoFSLBR();
                        var DatosReporte = new Dictionary<string, object>
                            {
                                {"CU018", contratoBR.ObtenerDatosContratoMaestro(dataContext, vista.Contrato)},
                                {"CU019", contratoBR.ObtenerDatosAnexoA(dataContext, vista.ContratoID.Value)},
                                {"CU021", contratoBR.ObtenerDatosAnexoC(dataContext, vista.Contrato)}
                            };

                        vista.EstablecerPaqueteNavegacionImprimir("CU022", DatosReporte);

                        vista.IrAImprimir();
                    }
                    else
                        vista.MostrarMensaje("No se cuenta con información necesaria para imprimir los anexos del contrato", ETipoMensajeIU.ADVERTENCIA);
                }
                else
                    vista.MostrarMensaje("No se cuenta con el Identificador del Contrato", ETipoMensajeIU.ADVERTENCIA);

            }
            catch (Exception ex)
            {
                vista.MostrarMensaje("Inconsistencias al Intentar  desplegar los Anexos del Contrato",
                                     ETipoMensajeIU.ERROR, nombreClase + ".ImprimirAnexosContrato: " + ex.Message);
            }
        }

        #region SC_0008
        public void ValidarAcceso()
        {
            try
            {
                //se valida que los datos del usuario y la unidad operativa no sean nulos
                if (this.vista.UsuarioID == null) throw new Exception("El identificador del usuario no debe ser nulo");
                if (this.vista.UnidadAdscripcionID == null) throw new Exception("El identificador de la Unidad Operativa no debe ser nulo ");

                //Se crea el objeto de seguridad
                UsuarioBO usuario = new UsuarioBO() { Id = this.vista.UsuarioID };
                AdscripcionBO adscripcion = new AdscripcionBO() { UnidadOperativa = new UnidadOperativaBO() { Id = this.vista.UnidadAdscripcionID } };
                SeguridadBO seguridadBO = new SeguridadBO(Guid.Empty, usuario, adscripcion);

                //Se valida si el usuario tiene permiso a la acción principal
                if (!FacadeBR.ExisteAccion(this.dataContext, "CONSULTAR", seguridadBO))
                    this.vista.RedirigirSinPermisoAcceso();
            }
            catch (Exception ex)
            {
                throw new Exception(nombreClase + ".ValidarAcceso:" + ex.Message);
            }
        }
        /// <summary>
        /// Establece la seguridad para los controles de la interfaz de usuario
        /// </summary>
        public void EstablecerSeguridad()
        {
            try
            {
                //se valida que los datos del usuario y la unidad operativa no sean nulos
                if (this.vista.UsuarioID == null) throw new Exception("El identificador del usuario no debe ser nulo");
                if (this.vista.UnidadAdscripcionID == null) throw new Exception("El identificador de la Unidad Operativa no debe ser nulo ");

                //Se crea el objeto de seguridad
                UsuarioBO usuario = new UsuarioBO() { Id = this.vista.UsuarioID };
                AdscripcionBO adscripcion = new AdscripcionBO() { UnidadOperativa = new UnidadOperativaBO() { Id = this.vista.UnidadAdscripcionID } };
                SeguridadBO seguridadBO = new SeguridadBO(Guid.Empty, usuario, adscripcion);

                //Se obtienen las acciones a las cuales el usuario tiene permiso en este proceso
                List<CatalogoBaseBO> lst = FacadeBR.ConsultarAccion(this.dataContext, seguridadBO);

                //Se valida si el usuario tiene permiso para registrar
                if (!this.ExisteAccion(lst, "UI INSERTAR"))
                    this.vista.PermitirRegistrar(false);
                //Se valida si el usuario tiene permiso para editar
                if (!this.ExisteAccion(lst, "UI ACTUALIZAR"))
                    this.PermitirEditar();
                //Se valida si el usuario tiene permiso para terminar el documento
                if (!this.ExisteAccion(lst, "UI TERMINARDOCUMENTO"))
                    this.PermitirCerrar();
                //Se valida si el usuario tiene permiso para borrar
                if (!this.ExisteAccion(lst, "BORRARCOMPLETO"))
                    this.PermitirBorrar();
                //Se valida si el usuario tiene permiso para imprimir
                if (!this.ExisteAccion(lst, "UI IMPRIMIR"))
                    this.PermitirImprimir();
                //De acuerdo al perfil habilita y deshabilita las opciones de impresion
                if (!FacadeBR.ExisteAccion(this.dataContext, "UI IMPRIMIR", seguridadBO))
                    herramientasPRE.OcultarMenuImpresion();
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
        private bool ExisteAccion(List<CatalogoBaseBO> acciones, string accion)
        {
            if (acciones != null && acciones.Count > 0 && acciones.Exists(p => p != null && p.Nombre != null && p.Nombre.Trim().ToUpper() == accion.Trim().ToUpper()))
                return true;

            return false;
        }

        /// <summary>
        /// Deshabilita la opción Editar de la barra de herramientas
        /// </summary>
        private void PermitirEditar()
        {
            herramientasPRE.DeshabilitarMenuEditar();
        }
        /// <summary>
        /// Deshabilita la opción Borrar Contrato de la barra de herramientas
        /// </summary>
        private void PermitirBorrar()
        {
            herramientasPRE.DeshabilitarMenuBorrar();
        }
        /// <summary>
        /// Deshabilita la opción Cerrar Contrato de la barra de herramientas
        /// </summary>
        private void PermitirCerrar()
        {
            herramientasPRE.DeshabilitarMenuCerrar();
        }
        /// <summary>
        /// Deshabilita las opciones de Imprimir de la barra de herramientas
        /// </summary>
        private void PermitirImprimir()
        {
            herramientasPRE.DeshabilitarMenuImprimir();
        }
        #endregion

        #region CU026

        /// <summary>
        /// Envía el Contrato a Cerrar Contrato
        /// </summary>
        public void CerrarContrato()
        {
            try
            {
                ContratoFSLBO contrato = InterfazUsuarioADatos();
                vista.Contrato = contrato;
                contrato = ObtenerContrato();
                vista.EstablecerPaqueteNavegacionEditar("UltimoContratoFSLBO", contrato);
                vista.IrACerrarContrato();
            }
            catch (Exception ex)
            {
                vista.MostrarMensaje("Inconsistencias al intentar cerrar el contrato.", ETipoMensajeIU.ERROR, nombreClase + ".CerrarContrato: " + ex.Message);
            }
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

		public void ImprimirPagare()
		{
			try
			{
				if (this.vista.ContratoID == null)
					throw new Exception("No se puede imprimir el pagaré sin el identificador del contrato");
				ContratoFSLBR controlador = new ContratoFSLBR();
				Dictionary<string, Object> datosPagare = controlador.ObtenerDatosPagare(FacadeBR.ObtenerConexion(), new ContratoFSLBO() { ContratoID = this.vista.ContratoID });
				vista.EstablecerPaqueteNavegacionImprimir("CU093", datosPagare);
				vista.IrAImprimir();

			}
			catch (Exception ex)
			{
				throw new Exception("DetalleContratoFSLPRE.ImprimirPagare: " + ex.Message);
			}
		}
        #endregion
    }
}