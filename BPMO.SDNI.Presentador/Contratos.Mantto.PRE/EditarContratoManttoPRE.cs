// Esta clase satisface los requerimientos del CU028 - Editar Contrato de Mantenimiento
// Satisface a la solicitud de Cambio SC0001
//Satisface al caso de uso CU003 - Calcular Monto a Facturar CM y SD
// Satisface a la solución del RI0008
using System;
using System.Collections.Generic;
using System.Data;
using BPMO.Basicos.BO;
using BPMO.Facade.SDNI.BR;
using BPMO.Patterns.Creational.DataContext;
using BPMO.Primitivos.Enumeradores;
using BPMO.SDNI.Comun.BO;
using BPMO.SDNI.Comun.BR;
using BPMO.SDNI.Comun.PRE;
using BPMO.SDNI.Comun.VIS;
using BPMO.SDNI.Contratos.BO;
using BPMO.SDNI.Contratos.BR;
using BPMO.SDNI.Contratos.Mantto.BO;
using BPMO.SDNI.Contratos.Mantto.BR;
using BPMO.SDNI.Contratos.Mantto.VIS;
using BPMO.SDNI.Facturacion.AplicacionesPago.BR;

namespace BPMO.SDNI.Contratos.Mantto.PRE
{
    /// <summary>
    /// Presentador para la vista de edición de contrato
    /// </summary>
    public class EditarContratoManttoPRE
    {
        #region Atributos
        /// <summary>
        /// Vista para la edición del contrato
        /// </summary>
        private readonly IEditarContratoManttoVIS vista;
        /// <summary>
        /// Provee la conexión a la BD
        /// </summary>
        private readonly IDataContext dctx;
        /// <summary>
        /// Nombre de la clase que se usará en los mensajes de error
        /// </summary>
        private const string nombreClase = "EditarContratoManttoPRE";
        /// <summary>
        /// Controlador que ejecutará las accciones
        /// </summary>
        private readonly ContratoManttoBR controlador;
        /// <summary>
        /// Presentador de la Barra de Herramientas
        /// </summary>
        private readonly ucHerramientasManttoPRE presentadorHerramientas;
        /// <summary>
        /// Presentador del user control de datos
        /// </summary>
        private ucContratoManttoPRE presentadorDatos;
        /// <summary>
        /// Presentador del catálogo de documentos
        /// </summary>
        private ucCatalogoDocumentosPRE presentadorDocumentos;
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor del presentador para la edición de los contratos de mantenimiento
        /// </summary>
        /// <param name="vista">vista de la edición del contrato de mantenimiento</param>
        /// <param name="viewHerramientas">Vista del user control correspondiente a la barra de herramientas</param>
        /// <param name="viewContrato">Vista del  user control general</param>
        /// <param name="viewLinea">Vista de l user control de linea de contrato</param>
        /// <param name="viewDocumentos">Vista del user control de documnetos</param>
        public EditarContratoManttoPRE(IEditarContratoManttoVIS vista, IucHerramientasManttoVIS viewHerramientas, IucContratoManttoVIS viewContrato, IucLineaContratoManttoVIS viewLinea, IucCatalogoDocumentosVIS viewDocumentos)
        {
            try
            {
                if (ReferenceEquals(vista, null))
                    throw new Exception("La vista asociada no puede ser nula");

                this.vista = vista;
                this.controlador = new ContratoManttoBR();
                this.dctx = FacadeBR.ObtenerConexion();
                this.presentadorDatos = new ucContratoManttoPRE(viewContrato, viewLinea);
                this.presentadorDocumentos = new ucCatalogoDocumentosPRE(viewDocumentos);
                this.presentadorHerramientas = new ucHerramientasManttoPRE(viewHerramientas);
            }
            catch (Exception ex)
            {
                this.vista.MostrarMensaje("Inconsistencia al crear el presentador", ETipoMensajeIU.ERROR, string.Format("{0}.EditarContratoManttoPRE:{1}{2}", nombreClase, Environment.NewLine, ex.Message));
            }
        }
        #endregion

        #region Métodos
        /// <summary>
        /// Valida el acceso a la página de edición del contrato
        /// </summary>
        public void ValidarAcceso()
        {
            try
            {
                //Valida que usuario y la unidad operativa no sean nulos
                if (this.vista.UsuarioID == null) throw new Exception("El identificador del usuario no debe ser nulo");
                if (this.vista.UnidadOperativaID == null) throw new Exception("El identificador de la Unidad Operativa no debe ser nulo");
                //Se crea el objeto de seguridad
                UsuarioBO usuario = new UsuarioBO() { Id = this.vista.UsuarioID };
                AdscripcionBO adscripcion = new AdscripcionBO() { UnidadOperativa = new UnidadOperativaBO() { Id = this.vista.UnidadOperativaID } };
                SeguridadBO seguridadBO = new SeguridadBO(Guid.Empty, usuario, adscripcion);

                //Se valida si el usuario tiene permiso a la acción principal
                if (!FacadeBR.ExisteAccion(this.dctx, "ACTUALIZARCOMPLETO", seguridadBO))
                    this.vista.RedirigirSinPermisoAcceso();
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("{0}.ValidarAcceso:{1}{2}", nombreClase, Environment.NewLine, ex.Message));
            }
        }
        /// <summary>
        /// Prepara la edición del contrato
        /// </summary>
        public void PrepararEdicion()
        {
            try
            {
                //Obtenemos el paquete correspondiente al contrato a editar
                var contratoAnterior = this.vista.ObtenerPaqueteNavegacion("ContratoManttoBO");
                var contratoBase = this.vista.ObtenerPaqueteNavegacion("ContratoManttoBO");

                if (object.ReferenceEquals(contratoBase, null))
                    throw new Exception("Se esperaba un objeto en la navegación. No fue posible identificar el contrato que se desea editar.");
                if (!(contratoBase is ContratoManttoBO))
                    throw new Exception("Se esperaba un contrato de mantenimiento para poder continuar con la edición.");

                //Habilitamos el modo editable para el catálogo de documentos
                this.presentadorDocumentos.ModoEditable(true);
                //Le pedimos al usercontrol que se prepare para la edición
                this.presentadorDatos.PrepararEdicion();
                //Cargamos los tipos de archivo validos para adjuntar al contrato
                List<TipoArchivoBO> lstTiposArchivo = new TipoArchivoBR().Consultar(this.dctx, new TipoArchivoBO());
                this.presentadorDocumentos.EstablecerTiposArchivo(lstTiposArchivo);

                //Eliminamos el paquete de navegación
                this.vista.LimpiarPaqueteNavegacion("ContratoManttoBO");

                //Asignamos como último objeto el contrato a editar
                this.vista.UltimoObjeto = this.ObtenerContrato(contratoAnterior);
                //Obtenemos el contrato a desplegar
                var contrato = this.ObtenerContrato(contratoBase);
                //Desplegmaos el contrato obtenido
                this.DatoAInterfazUsuario(contrato);

                #region Configuraciones de Unidad Operativa
                //Obtener las configuraciones de la unidad operativa
                List<ConfiguracionUnidadOperativaBO> lstConfigUO = new ModuloBR().ConsultarConfiguracionUnidadOperativa(this.dctx, new ConfiguracionUnidadOperativaBO() { UnidadOperativa = new UnidadOperativaBO() { Id = this.vista.UnidadOperativaID } }, this.vista.ModuloID);
                if (lstConfigUO.Count <= 0)
                    throw new Exception("No se encontraron las configuraciones del sistema para la unidad operativa en la que trabaja.");

                this.vista.RepresentanteEmpresa = lstConfigUO[0].Representante;
                #endregion

                //Pedimos se configure las opciones habilitadas para la cuenta del cliente
                this.presentadorDatos.ConfigurarOpcionesPersonas();

                //Habilitamos los botones de guardado
                this.vista.HabilitarModoBorrador(true);
                this.vista.HabilitarModoTerminado(true);

                //Habilitar opciones barra de herramientas
                this.presentadorHerramientas.vista.HabilitarOpcionesEdicion();
                this.presentadorHerramientas.DeshabilitarMenuBorrar();
                this.presentadorHerramientas.DeshabilitarMenuCerrar();
                this.presentadorHerramientas.DeshabilitarMenuImprimir();
                this.presentadorHerramientas.DeshabilitarMenuDocumentos();
                this.presentadorHerramientas.vista.MarcarOpcionEditarContrato();
            }
            catch (Exception ex)
            {
                this.vista.HabilitarModoBorrador(false);
                this.vista.HabilitarModoTerminado(false);
                throw new Exception(string.Format("{0}.RealizarPrimeraCarga:{1}{2}", nombreClase, Environment.NewLine, ex.Message));
            }
        }
        /// <summary>
        /// Obtiene el contrato que se deseas editar
        /// </summary>
        /// <param name="contratoBase">Contrato que se desea editar</param>
        /// <returns>Contrato completo para su edición</returns>
        private ContratoManttoBO ObtenerContrato(object contratoBase)
        {
            var obj = (ContratoManttoBO)contratoBase;
            //Consultamos el objeto
            var contratos = this.controlador.ConsultarCompleto(this.dctx, obj, true);

            //Limpiamos la instancía del objeto
            obj = null;

            //Validamos que se haya obtenido realmente un contrato para su devolución
            if (!ReferenceEquals(contratos, null) && contratos.Count > 0) obj = contratos[0];

            return obj;
        }
        /// <summary>
        /// Despliega en pantalla la información del contrato recuperado
        /// </summary>
        /// <param name="obj">Objeto que </param>
        private void DatoAInterfazUsuario(object obj)
        {
            var bo = (ContratoManttoBO)obj;

            #region Contrato nuevo
            if (ReferenceEquals(obj, null))
                bo = new ContratoManttoBO
                {
                    Avales = new List<AvalBO>(),
                    Cliente = new CuentaClienteIdealeaseBO { Cliente = new ClienteBO() },
                    DatosAdicionalesAnexo = new List<DatoAdicionalAnexoBO>(),
                    Divisa = new DivisaBO { MonedaDestino = new MonedaBO() },
                    DocumentosAdjuntos = new List<ArchivoBO>(),
                    RepresentantesLegales = new List<PersonaBO>(),
                    LineasContrato = new List<ILineaContrato>(),
                    ObligadosSolidarios = new List<PersonaBO>(),
                    Sucursal = new SucursalBO { UnidadOperativa = new UnidadOperativaBO() }
                };
            #endregion

            #region Instancias de propiedades
            if (object.ReferenceEquals(bo.Avales, null))
                bo.Avales = new List<AvalBO>();
            if (object.ReferenceEquals(bo.Cliente, null))
                bo.Cliente = new CuentaClienteIdealeaseBO { Cliente = new ClienteBO() };
            if (object.ReferenceEquals(bo.DatosAdicionalesAnexo, null))
                bo.DatosAdicionalesAnexo = new List<DatoAdicionalAnexoBO>();
            if (object.ReferenceEquals(bo.Divisa, null))
                bo.Divisa = new DivisaBO { MonedaDestino = new MonedaBO() };
            if (object.ReferenceEquals(bo.DocumentosAdjuntos, null))
                bo.DocumentosAdjuntos = new List<ArchivoBO>();
            if (object.ReferenceEquals(bo.LineasContrato, null))
                bo.LineasContrato = new List<ILineaContrato>();
            if (object.ReferenceEquals(bo.RepresentantesLegales, null))
                bo.RepresentantesLegales = new List<PersonaBO>();
            if (object.ReferenceEquals(bo.ObligadosSolidarios, null))
                bo.ObligadosSolidarios = new List<PersonaBO>();
            if (object.ReferenceEquals(bo.Sucursal, null))
                bo.Sucursal = new SucursalBO { UnidadOperativa = new UnidadOperativaBO() };
            #endregion

            #region Datos Generales
            this.vista.TipoContrato = bo.Tipo.HasValue ? (int?)((int)bo.Tipo.Value) : null;
            this.vista.ContratoID = bo.ContratoID.HasValue ? bo.ContratoID : null;
            this.vista.NumeroContrato = !string.IsNullOrEmpty(bo.NumeroContrato) && !string.IsNullOrWhiteSpace(bo.NumeroContrato)
                                            ? bo.NumeroContrato.Trim().ToUpper()
                                            : string.Empty;
            this.vista.EstatusContrato = bo.Estatus.HasValue ? (int?)((int)bo.Estatus.Value) : null;
            this.vista.FechaContrato = bo.FechaContrato.HasValue ? bo.FechaContrato : null;
            this.vista.SucursalID = bo.Sucursal.Id.HasValue ? bo.Sucursal.Id : null;
            this.vista.SucursalNombre = !string.IsNullOrEmpty(bo.Sucursal.Nombre) && !string.IsNullOrWhiteSpace(bo.Sucursal.Nombre)
                                            ? bo.Sucursal.Nombre.Trim().ToUpper()
                                            : string.Empty;
            this.vista.RepresentanteEmpresa = !string.IsNullOrEmpty(bo.Representante) && !string.IsNullOrWhiteSpace(bo.Representante)
                                                  ? bo.Representante.Trim().ToUpper()
                                                  : string.Empty;

            this.vista.DivisaID = !string.IsNullOrEmpty(bo.Divisa.MonedaDestino.Codigo) && !string.IsNullOrWhiteSpace(bo.Divisa.MonedaDestino.Codigo)
                                    ? bo.Divisa.MonedaDestino.Codigo.ToUpper().Trim()
                                      : string.Empty;
            this.vista.EstatusContrato = bo.Estatus.HasValue ? (int?)((int)bo.Estatus.Value) : null;
            #endregion

            #region Datos Cliente
            this.vista.ClienteID = bo.Cliente.Cliente != null && bo.Cliente.Cliente.Id.HasValue ? bo.Cliente.Cliente.Id : null;
            this.vista.CuentaClienteID = bo.Cliente.Id.HasValue
                                             ? bo.Cliente.Id
                                             : null;
            this.vista.CuentaClienteTipoID = bo.Cliente.TipoCuenta.HasValue ? (int?)((int)bo.Cliente.TipoCuenta) : null;
            this.vista.CuentaClienteNombre = !string.IsNullOrEmpty(bo.Cliente.Nombre) && !string.IsNullOrWhiteSpace(bo.Cliente.Nombre)
                                                 ? bo.Cliente.Nombre.Trim().ToUpper()
                                                 : string.Empty;
            this.vista.CuentaClienteFisica = bo.Cliente.EsFisico.HasValue ? bo.Cliente.EsFisico : null;

            //Se ejecuta explicitamente la selección de la cuenta del cliente
            this.presentadorDatos.SeleccionarCuentaCliente(bo.Cliente);

            #region Direccion
            var direccion = new DireccionClienteBO { Ubicacion = new UbicacionBO { Ciudad = new CiudadBO(), Estado = new EstadoBO(), Municipio = new MunicipioBO(), Pais = new PaisBO() } };

            if (bo.Cliente.Direcciones != null && bo.Cliente.Direcciones.Count > 0)
            {
                direccion = bo.Cliente.Direcciones[0];

                if (direccion.Ubicacion == null)
                    direccion.Ubicacion = new UbicacionBO
                    {
                        Ciudad = new CiudadBO(),
                        Estado = new EstadoBO(),
                        Municipio = new MunicipioBO(),
                        Pais = new PaisBO()
                    };
                if (direccion.Ubicacion.Ciudad == null) direccion.Ubicacion.Ciudad = new CiudadBO();
                if (direccion.Ubicacion.Estado == null) direccion.Ubicacion.Estado = new EstadoBO();
                if (direccion.Ubicacion.Municipio == null) direccion.Ubicacion.Municipio = new MunicipioBO();
                if (direccion.Ubicacion.Pais == null) direccion.Ubicacion.Pais = new PaisBO();
            }

            this.vista.DireccionClienteID = direccion.Id.HasValue ? (int?)((int)direccion.Id) : null;
            this.vista.ClienteDireccionCompleta = !string.IsNullOrEmpty(direccion.Direccion) && !string.IsNullOrWhiteSpace(direccion.Direccion)
                                                      ? direccion.Direccion.Trim().ToUpper()
                                                      : string.Empty;
            this.vista.ClienteDireccionCalle = !string.IsNullOrEmpty(direccion.Calle) && !string.IsNullOrWhiteSpace(direccion.Calle)
                                                   ? direccion.Calle.Trim().ToUpper()
                                                   : string.Empty;
            this.vista.ClienteDireccionColonia = !string.IsNullOrEmpty(direccion.Colonia) && !string.IsNullOrWhiteSpace(direccion.Colonia)
                                                     ? direccion.Colonia.Trim().ToUpper()
                                                     : string.Empty;
            this.vista.ClienteDireccionCiudad = !string.IsNullOrEmpty(direccion.Ubicacion.Ciudad.Codigo) && !string.IsNullOrWhiteSpace(direccion.Ubicacion.Ciudad.Codigo)
                                                    ? direccion.Ubicacion.Ciudad.Codigo
                                                    : string.Empty;
            this.vista.ClienteDireccionEstado = !string.IsNullOrEmpty(direccion.Ubicacion.Estado.Codigo) && !string.IsNullOrWhiteSpace(direccion.Ubicacion.Estado.Codigo)
                                                    ? direccion.Ubicacion.Estado.Codigo
                                                    : string.Empty;
            this.vista.ClienteDireccionMunicipio = !string.IsNullOrEmpty(direccion.Ubicacion.Municipio.Codigo) && !string.IsNullOrWhiteSpace(direccion.Ubicacion.Municipio.Codigo)
                                                       ? direccion.Ubicacion.Municipio.Codigo
                                                       : string.Empty;
            this.vista.ClienteDireccionPais = !string.IsNullOrEmpty(direccion.Ubicacion.Pais.Codigo) && !string.IsNullOrWhiteSpace(direccion.Ubicacion.Pais.Codigo)
                                                  ? direccion.Ubicacion.Pais.Codigo
                                                  : string.Empty;
            this.vista.ClienteDireccionCodigoPostal = !string.IsNullOrEmpty(direccion.CodigoPostal) && !string.IsNullOrWhiteSpace(direccion.CodigoPostal)
                                                          ? direccion.CodigoPostal
                                                          : string.Empty;
            #endregion

            //Se asignan los representantes legales seleccionados
            this.vista.RepresentatesLegales = bo.RepresentantesLegales.ConvertAll(x => (object)x);
            this.vista.SoloRepresentantes = bo.SoloRepresentantes.HasValue ? bo.SoloRepresentantes : null;

            //se asignan los obligados solidarios seleccionados
            this.vista.ObligadosSolidarios = bo.ObligadosSolidarios.ConvertAll(x => (object)x);
            this.vista.ObligadosComoAvales = bo.ObligadosComoAvales.HasValue ? bo.ObligadosComoAvales : null;

            //Se asignan los avlaes seleccionados
            this.vista.Avales = bo.Avales.ConvertAll(x => (object)x);

            #endregion

            #region Condiciones de renta
            this.vista.Plazo = bo.Plazo.HasValue ? bo.Plazo : null;
            this.vista.FechaInicioContrato = bo.FechaInicioContrato.HasValue ? bo.FechaInicioContrato : null;
            this.vista.FechaTerminacionContrato = bo.CalcularFechaTerminacionContrato();
            this.vista.DepositoGarantia = bo.DepositoGarantia.HasValue ? bo.DepositoGarantia : null;
            this.vista.ComisionApertura = bo.ComisionApertura.HasValue ? bo.ComisionApertura : null;
            this.vista.IncluyeLavado = bo.IncluyeLavado.HasValue ? (int?)((int)bo.IncluyeLavado.Value) : null;
            this.vista.IncluyeLlantas = bo.IncluyeLlantas.HasValue ? (int?)((int)bo.IncluyeLlantas.Value) : null;
            this.vista.IncluyePinturaRotulación = bo.IncluyePinturaRotulacion.HasValue
                                                      ? (int?)((int)bo.IncluyePinturaRotulacion.Value)
                                                      : null;
            this.vista.IncluyeSeguro = bo.IncluyeSeguro.HasValue ? (int?)((int)bo.IncluyeSeguro.Value) : null;
            #endregion

            #region Otros

            this.vista.DireccionAlmacenaje = !string.IsNullOrEmpty(bo.DireccionAlmacenaje) && !string.IsNullOrWhiteSpace(bo.DireccionAlmacenaje)
                                                 ? bo.DireccionAlmacenaje.Trim().ToUpper()
                                                 : string.Empty;
            this.vista.UbicacionTaller = !string.IsNullOrEmpty(bo.UbicacionTaller) && !string.IsNullOrWhiteSpace(bo.UbicacionTaller)
                                             ? bo.UbicacionTaller.Trim().ToUpper()
                                             : string.Empty;
            this.vista.Observaciones = !string.IsNullOrEmpty(bo.Observaciones) && !string.IsNullOrWhiteSpace(bo.Observaciones) ? bo.Observaciones.Trim().ToUpper() : string.Empty;
            #endregion

            #region Unidades
            this.vista.LineasContrato = bo.LineasContrato.ConvertAll(x => (object)x);
            #endregion

            #region Datos adicionales
            this.vista.DatosAdicionales = bo.DatosAdicionalesAnexo.ConvertAll(x => (object)x);
            #endregion

            //Documentos asociados al contrato
            if (this.presentadorDocumentos.Vista.TiposArchivo == null)
                this.presentadorDocumentos.Vista.TiposArchivo = new List<TipoArchivoBO>();

            this.presentadorDocumentos.CargarListaArchivos(bo.DocumentosAdjuntos, this.presentadorDocumentos.Vista.TiposArchivo);

            //Valores para la barra de herramientas
            this.presentadorHerramientas.DatosAInterfazUsuario(bo);
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
                if (this.vista.TipoContrato != null)
                {
                    if ((ETipoContrato)Enum.Parse(typeof(ETipoContrato), this.vista.TipoContrato.ToString()) == ETipoContrato.CM)
                        plantilla.TipoPlantilla = EModulo.CM;
                    if ((ETipoContrato)Enum.Parse(typeof(ETipoContrato), this.vista.TipoContrato.ToString()) == ETipoContrato.SD)
                        plantilla.TipoPlantilla = EModulo.SD;
                }

                var lista = controlador.Consultar(this.dctx, plantilla);

                if (ReferenceEquals(lista, null))
                    lista = new List<PlantillaBO>();

                resultado = lista.ConvertAll(p => (object)p);
            }

            this.presentadorHerramientas.CargarArchivos(resultado);
        }
        /// <summary>
        /// Cancela la edición del contrato
        /// </summary>
        public void CancelarEdicion()
        {
            this.presentadorDatos.LimpiarSesion();
            this.presentadorDocumentos.LimpiarSesion();
            this.vista.LimpiarSesion();
            this.vista.LimpiarPaqueteNavegacion("ContratoManttoBO");
            var contratoId = this.vista.ContratoID;
            if (contratoId != null)
                this.vista.EstablecerPaqueteNavegacion("ContratoManttoBO", new ContratoManttoBO { ContratoID = contratoId.Value });
            this.vista.RedirigirADetalles();
        }
        /// <summary>
        /// Edita un contrato en modo borrador
        /// </summary>
        public void EditarBorrador()
        {
            string s;

            if ((s = this.ValidarCamposBorrador()) != null)
            {
                this.vista.MostrarMensaje(s, ETipoMensajeIU.ADVERTENCIA, null);
                return;
            }
            //Asignamos el estatus correspondiente al contrato
            this.vista.EstatusContrato = (int)EEstatusContrato.Borrador;

            this.Editar();

            this.vista.LimpiarSesion();
            this.vista.LimpiarPaqueteNavegacion("ContratoManttoBO");
            var contratoId = this.vista.ContratoID;
            if (contratoId != null)
                this.vista.EstablecerPaqueteNavegacion("ContratoManttoBO", new ContratoManttoBO { ContratoID = contratoId.Value });
            this.vista.RedirigirADetalles();
        }
        /// <summary>
        /// Ejecuta la llamada al método que actualiza el contrato en la base de datos
        /// </summary>
        private void Editar()
        {
            #region Se inicia la Transaccion
            dctx.SetCurrentProvider("Outsourcing");
            Guid firma = Guid.NewGuid();
            try
            {
                dctx.OpenConnection(firma);
                dctx.BeginTransaction(firma);
            }

            catch(Exception)
            {
                if(dctx.ConnectionState == ConnectionState.Open)
                    dctx.CloseConnection(firma);
                throw new Exception("Se encontraron inconsistencias al insertar el Contrato.");
            }
            #endregion

            try
            {
                //Se obtiene la información del contrato a partir de la vista
                var bo = (ContratoManttoBO)this.InterfazUsuarioADato();

                //Se crea el objeto de seguridad
                UsuarioBO usuario = new UsuarioBO() { Id = this.vista.UsuarioID };
                AdscripcionBO adscripcion = new AdscripcionBO() { UnidadOperativa = new UnidadOperativaBO() { Id = this.vista.UnidadOperativaID } };
                SeguridadBO seguridadBO = new SeguridadBO(Guid.Empty, usuario, adscripcion);

                //Se actualiza en la base de datos
                this.controlador.ActualizarCompleto(dctx, bo, (ContratoManttoBO)this.vista.UltimoObjeto, seguridadBO);

                #region SC0001 BEP1401 - Registra los pagos del Contrato

                if (bo.Estatus == EEstatusContrato.EnCurso)
                {
                    GeneradorPagosManttoBR generadorPagos = new GeneradorPagosManttoBR();
                    generadorPagos.GenerarPagos(dctx, bo, seguridadBO, true);
                }

                dctx.CommitTransaction(firma);
                #endregion
            }catch (Exception ex){
                dctx.RollbackTransaction(firma);
                throw new Exception(string.Format("{0}.Editar:{1}{2}", nombreClase, Environment.NewLine, ex.Message));
            }
            finally
            {
                if(dctx.ConnectionState == ConnectionState.Open)
                    dctx.CloseConnection(firma);
            }
        }
        /// <summary>
        /// Obtiene la información porporcionada por el ususario en la base de datos y la transforma en un contrato
        /// </summary>
        /// <returns>Contrato genenerado por los datos</returns>
        private object InterfazUsuarioADato()
        {
            #region Creacion BO's
            var bo = new ContratoManttoBO();
            bo.Avales = new List<AvalBO>();
            bo.CierreContrato = new CierreContratoManttoBO();
            bo.Cliente = new CuentaClienteIdealeaseBO { Cliente = new ClienteBO(), UnidadOperativa = new UnidadOperativaBO() };
            bo.Divisa = new DivisaBO { MonedaDestino = new MonedaBO() };
            bo.Sucursal = new SucursalBO { UnidadOperativa = new UnidadOperativaBO() };
            bo.LineasContrato = new List<ILineaContrato>();
            #endregion

            #region Información General
            if (this.vista.ContratoID.HasValue)
                bo.ContratoID = this.vista.ContratoID.Value;
            if (this.vista.TipoContrato.HasValue)
                bo.Tipo = (ETipoContrato)this.vista.TipoContrato.Value;
            if (!string.IsNullOrEmpty(this.vista.NumeroContrato) && !string.IsNullOrWhiteSpace(this.vista.NumeroContrato))
                bo.NumeroContrato = this.vista.NumeroContrato.Trim().ToUpper();
            if (this.vista.EstatusContrato.HasValue)
                bo.Estatus = (EEstatusContrato)this.vista.EstatusContrato.Value;
            if (this.vista.FechaContrato.HasValue)
                bo.FechaContrato = this.vista.FechaContrato.Value;
            if (this.vista.SucursalID.HasValue)
                bo.Sucursal.Id = this.vista.SucursalID.Value;
            if (!String.IsNullOrEmpty(this.vista.SucursalNombre))
                bo.Sucursal.Nombre = this.vista.SucursalNombre;
            if (this.vista.UnidadOperativaID.HasValue)
                bo.Sucursal.UnidadOperativa.Id = this.vista.UnidadOperativaID.Value;
            if(!String.IsNullOrEmpty(this.vista.UnidadOperativaNombre))
                bo.Sucursal.UnidadOperativa.Nombre = this.vista.UnidadOperativaNombre;
            if (!string.IsNullOrEmpty(this.vista.RepresentanteEmpresa) && !string.IsNullOrWhiteSpace(this.vista.RepresentanteEmpresa))
                bo.Representante = this.vista.RepresentanteEmpresa.Trim().ToUpper();
            if (!string.IsNullOrEmpty(this.vista.DivisaID) && !string.IsNullOrWhiteSpace(this.vista.DivisaID))
                bo.Divisa.MonedaDestino.Codigo = this.vista.DivisaID;
            #endregion

            #region Informacion Cliente
            if (this.vista.CuentaClienteID.HasValue)
                bo.Cliente.Id = this.vista.CuentaClienteID.Value;
            if (!string.IsNullOrEmpty(this.vista.CuentaClienteNombre) && !string.IsNullOrWhiteSpace(this.vista.CuentaClienteNombre))
                bo.Cliente.Nombre = this.vista.CuentaClienteNombre;
            if (this.vista.SoloRepresentantes.HasValue)
                bo.SoloRepresentantes = this.vista.SoloRepresentantes.Value;
            if (this.vista.RepresentatesLegales != null && this.vista.RepresentatesLegales.Count > 0)
                bo.RepresentantesLegales = this.vista.RepresentatesLegales.ConvertAll(x => (PersonaBO)x);
            if (this.vista.ObligadosSolidarios != null && this.vista.ObligadosSolidarios.Count > 0)
                bo.ObligadosSolidarios = this.vista.ObligadosSolidarios.ConvertAll(x => (PersonaBO)x);
            if (this.vista.ObligadosComoAvales.HasValue)
                bo.ObligadosComoAvales = this.vista.ObligadosComoAvales.Value;
            if (this.vista.Avales != null && this.vista.Avales.Count > 0)
                bo.Avales = this.vista.Avales.ConvertAll(x => (AvalBO)x);
            if (bo.ObligadosComoAvales != null && bo.ObligadosComoAvales == true)
            {
                if (this.vista.ObligadosSolidarios != null)
                    bo.Avales = this.vista.ObligadosSolidarios.ConvertAll(x => this.ObligadoAAval((ObligadoSolidarioBO)x));
            }
            if (this.vista.UnidadOperativaID.HasValue)
                bo.Cliente.UnidadOperativa.Id = this.vista.UnidadOperativaID.Value;

            #region Dirección del cliente
            var direccion = new DireccionClienteBO();
            direccion.Ubicacion = new UbicacionBO { Ciudad = new CiudadBO(), Estado = new EstadoBO(), Municipio = new MunicipioBO(), Pais = new PaisBO() };
            if (this.vista.DireccionClienteID.HasValue)
                direccion.Id = this.vista.DireccionClienteID.Value;
            if (!string.IsNullOrEmpty(this.vista.ClienteDireccionCalle) && !string.IsNullOrWhiteSpace(this.vista.ClienteDireccionCalle))
                direccion.Calle = this.vista.ClienteDireccionCalle.Trim().ToUpper();
            if (!string.IsNullOrEmpty(this.vista.ClienteDireccionCiudad) && !string.IsNullOrWhiteSpace(this.vista.ClienteDireccionCiudad))
                direccion.Ubicacion.Ciudad.Codigo = this.vista.ClienteDireccionCiudad.Trim().ToUpper();
            if (!string.IsNullOrEmpty(this.vista.ClienteDireccionCodigoPostal) && !string.IsNullOrWhiteSpace(this.vista.ClienteDireccionCodigoPostal))
                direccion.CodigoPostal = this.vista.ClienteDireccionCodigoPostal.Trim().ToUpper();
            if (!string.IsNullOrEmpty(this.vista.ClienteDireccionColonia) && !string.IsNullOrWhiteSpace(this.vista.ClienteDireccionColonia))
                direccion.Colonia = this.vista.ClienteDireccionColonia.Trim().ToUpper();
            if (!string.IsNullOrEmpty(this.vista.ClienteDireccionEstado) && !string.IsNullOrWhiteSpace(this.vista.ClienteDireccionEstado))
                direccion.Ubicacion.Estado.Codigo = this.vista.ClienteDireccionEstado.Trim().ToUpper();
            if (!string.IsNullOrEmpty(this.vista.ClienteDireccionMunicipio) && !string.IsNullOrWhiteSpace(this.vista.ClienteDireccionMunicipio))
                direccion.Ubicacion.Municipio.Codigo = this.vista.ClienteDireccionMunicipio;
            if (!string.IsNullOrEmpty(this.vista.ClienteDireccionPais) && !string.IsNullOrWhiteSpace(this.vista.ClienteDireccionPais))
                direccion.Ubicacion.Pais.Codigo = this.vista.ClienteDireccionPais;

            bo.Cliente.RemoverDirecciones();
            bo.Cliente.Agregar(direccion);
            #endregion

            #endregion

            #region Condiciones de Renta
            if (this.vista.Plazo.HasValue)
                bo.Plazo = this.vista.Plazo.Value;
            if (this.vista.FechaInicioContrato.HasValue)
                bo.FechaInicioContrato = this.vista.FechaInicioContrato.Value;
            if (this.vista.DepositoGarantia.HasValue)
                bo.DepositoGarantia = this.vista.DepositoGarantia.Value;
            if (this.vista.ComisionApertura.HasValue)
                bo.ComisionApertura = this.vista.ComisionApertura.Value;
            if (this.vista.IncluyeLavado.HasValue)
                bo.IncluyeLavado = (ETipoInclusion)this.vista.IncluyeLavado.Value;
            if (this.vista.IncluyeLlantas.HasValue)
                bo.IncluyeLlantas = (ETipoInclusion)this.vista.IncluyeLlantas.Value;
            if (this.vista.IncluyePinturaRotulación.HasValue)
                bo.IncluyePinturaRotulacion = (ETipoInclusion)this.vista.IncluyePinturaRotulación.Value;
            if (this.vista.IncluyeSeguro.HasValue)
                bo.IncluyeSeguro = (ETipoInclusion)this.vista.IncluyeSeguro;
            #endregion

            #region Otros
            if (!string.IsNullOrEmpty(this.vista.UbicacionTaller) && !string.IsNullOrWhiteSpace(this.vista.UbicacionTaller))
                bo.UbicacionTaller = this.vista.UbicacionTaller.Trim().ToUpper();
            if (!string.IsNullOrEmpty(this.vista.DireccionAlmacenaje) && !string.IsNullOrWhiteSpace(this.vista.DireccionAlmacenaje))
                bo.DireccionAlmacenaje = this.vista.DireccionAlmacenaje.Trim().ToUpper();
            if (!string.IsNullOrEmpty(this.vista.Observaciones) && !string.IsNullOrWhiteSpace(this.vista.Observaciones))
                bo.Observaciones = this.vista.Observaciones.Trim().ToUpper();
            #endregion

            #region Datos Adicionales
            if (this.vista.DatosAdicionales != null && this.vista.DatosAdicionales.Count > 0)
                bo.DatosAdicionalesAnexo = this.vista.DatosAdicionales.ConvertAll(x => (DatoAdicionalAnexoBO)x);
            #endregion

            #region Unidades
            if (this.vista.LineasContrato != null && this.vista.LineasContrato.Count > 0)
                bo.LineasContrato = this.vista.LineasContrato.ConvertAll(x => (ILineaContrato)x);
            #endregion

            #region Documentos adjuntos
            List<ArchivoBO> adjuntos = presentadorDocumentos.Vista.ObtenerArchivos() ?? new List<ArchivoBO>();
            foreach (ArchivoBO adjunto in adjuntos)
            {
                if (bo.Tipo != null && bo.Tipo == ETipoContrato.CM)
                    adjunto.TipoAdjunto = ETipoAdjunto.ContratoMantenimiento;
                if (bo.Tipo != null && bo.Tipo == ETipoContrato.SD)
                    adjunto.TipoAdjunto = ETipoAdjunto.ContratoServicioDedicado;
                adjunto.Auditoria = new AuditoriaBO
                {

                    FC = this.vista.FC,
                    UC = this.vista.UC,
                    FUA = DateTime.Now,
                    UUA = this.vista.UsuarioID.Value
                };
            }
            bo.DocumentosAdjuntos = adjuntos;
            #endregion

            bo.UC = this.vista.UC;
            bo.FC = this.vista.FC;
            bo.UUA = this.vista.UsuarioID.Value;
            bo.FUA = DateTime.Now;

            return bo;
        }
        /// <summary>
        /// Valida los campos mínimos para guardar un contrato en modo borrador
        /// </summary>
        /// <returns></returns>
        private string ValidarCamposBorrador()
        {
            string s = string.Empty;

            if ((s = this.presentadorDatos.ValidarCamposBorrador()) != null)
                return s;

            if (this.vista.UnidadOperativaID == null)
                s += "Unidad Operativa, ";
            if (this.vista.SucursalID == null)
                s += "Sucursal, ";
            if (string.IsNullOrEmpty(this.vista.DivisaID))
                s += "Moneda, ";
            if (this.vista.CuentaClienteID == null)
                s += "Cuenta del Cliente, ";
            if (this.vista.CuentaClienteFisica == null)
                s += "Tipo de Contribuyente del Cliente, ";

            if (s != null && s.Trim().CompareTo("") != 0)
                return "Los siguientes campos no pueden estar vacíos: \n" + s.Substring(0, s.Length - 2);

            if (this.vista.FechaContrato.HasValue && this.vista.FechaContrato.Value.Date < DateTime.Today)
                return "La fecha del contrato no puede ser menor a la fecha actual.";
            if (this.vista.FechaInicioContrato.HasValue && this.vista.FechaInicioContrato.Value.Date < DateTime.Today)
                return "La fecha de inicio del contrato no puede ser menor a la fecha actual.";

            return null;
        }
        /// <summary>
        /// Edita un contrato en modo "EN CURSO"
        /// </summary>
        public void EditarTerminada()
        {
            string s;
            if ((s = this.ValidarCamposRegistro()) != null)
            {
                this.vista.MostrarMensaje(s, ETipoMensajeIU.ADVERTENCIA, null);
                return;
            }
            //Asignamos el estatus correspondiente al contrato
            this.vista.EstatusContrato = (int)EEstatusContrato.EnCurso;

            this.Editar();

            this.vista.LimpiarSesion();
            this.vista.LimpiarPaqueteNavegacion("ContratoManttoBO");
            var contratoId = this.vista.ContratoID;
            if (contratoId != null)
                this.vista.EstablecerPaqueteNavegacion("ContratoManttoBO", new ContratoManttoBO { ContratoID = contratoId.Value });
            this.vista.RedirigirADetalles();
        }
        /// <summary>
        /// Valida si los campos son 
        /// </summary>
        /// <returns>Campos que no hayan sido porporcionados y sean necesarios para la ejecución del sistema</returns>
        private string ValidarCamposRegistro()
        {
            string s = string.Empty;

            if ((s = this.presentadorDatos.ValidarCamposRegistro()) != null)
                return s;

            if (this.vista.TipoContrato == null)
                s += "Tipo de Contrato";

            if (s != null && s.Trim().CompareTo("") != 0)
                return "Los siguientes campos no pueden estar vacíos: \n" + s.Substring(0, s.Length - 2);

            return null;
        }
        /// <summary>
        /// Convierte un obligado solidario en aval
        /// </summary>
        /// <param name="obligado">obligado solidario que se desea transformar</param>
        /// <returns>Aval resultante de la ocnversion</returns>
        private AvalBO ObligadoAAval(ObligadoSolidarioBO obligado)
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

        #region Seguridad
        /// <summary>
        /// Verifica los permisos de los usuarios y establece las opciones a las cuales tiene permiso el acceso
        /// </summary>
        public void EstablecerSeguridad()
        {
            try
            {
                //Valida que el usuario y la unidad operativa no sean nulos
                if (this.vista.UsuarioID == null) throw new Exception("El identificador del usuario no debe ser nulo");
                if (this.vista.UnidadOperativaID == null)
                    throw new Exception("El identificador de la Unidad Operativa no debe ser nulo");
                // Creación del objeto seguridad
                UsuarioBO usuario = new UsuarioBO { Id = this.vista.UsuarioID };
                AdscripcionBO adscripcion = new AdscripcionBO
                {
                    UnidadOperativa = new UnidadOperativaBO { Id = this.vista.UnidadOperativaID }
                };
                SeguridadBO seguridad = new SeguridadBO(Guid.Empty, usuario, adscripcion);

                //consulta de acciones a la cual el usuario tiene permisos
                List<CatalogoBaseBO> acciones = FacadeBR.ConsultarAccion(dctx, seguridad);

                // se valida si el usuario tiene permisos para registrar
                if (!this.ExisteAccion(acciones, "INSERTARCOMPLETO"))
                    this.vista.PermitirRegistrar(false);

                #region RI0008
                // se valida si el usuario tiene permisos para guardar el contrato en curso
                if (!this.ExisteAccion(acciones, "GENERARPAGOS"))
                    this.vista.PermitirGuardarTerminado(false); 
                #endregion
            }
            catch (Exception ex)
            {

                throw new Exception(nombreClase + ".EstablecerSeguridad:" + ex.Message);
            }
        }
        /// <summary>
        /// Verifica si la acción que el usuario desea ejecutar esta configurada
        /// </summary>
        /// <param name="acciones">Listado de acciones configuradas</param>
        /// <param name="nombreAccion">Nombre de la acción que se desea validar</param>
        /// <returns>Verdadero si la accion esta configurada; en otro caso falso</returns>
        private bool ExisteAccion(List<CatalogoBaseBO> acciones, string nombreAccion)
        {
            if (acciones != null && acciones.Count > 0 &&
                acciones.Exists(
                    p => p != null && p.Nombre != null && p.Nombre.Trim().ToUpper() == nombreAccion.Trim().ToUpper()))
                return true;

            return false;
        }
        #endregion
        #endregion
    }
}