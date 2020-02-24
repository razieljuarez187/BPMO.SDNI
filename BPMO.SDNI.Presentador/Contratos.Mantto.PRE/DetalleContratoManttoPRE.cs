//Satisface al caso de uso CU029 - Consultar Contratos de Mantenimiento
//Satisface al caso de uso CU095 - Imprimir Pagaré Contrato CM
//Satisface al caso de uso CU096 - Imprimir Pagaré Contrato SD

using System;
using System.Collections.Generic;
using System.Linq;

using BPMO.Facade.SDNI.BR;

using BPMO.Basicos.BO;
using BPMO.Patterns.Creational.DataContext;
using BPMO.Primitivos.Enumeradores;

using BPMO.SDNI.Comun.BO;
using BPMO.SDNI.Comun.PRE;
using BPMO.SDNI.Comun.VIS;
using BPMO.SDNI.Contratos.BO;
using BPMO.SDNI.Contratos.BR;
using BPMO.SDNI.Contratos.Mantto.BO;
using BPMO.SDNI.Contratos.Mantto.BR;
using BPMO.SDNI.Contratos.Mantto.VIS;

namespace BPMO.SDNI.Contratos.Mantto.PRE
{
    public class DetalleContratoManttoPRE
    {
        #region Atributos

        /// <summary>
        /// Vista sobre la que actua el presentador
        /// </summary>
        private readonly IDetalleContratoManttoVIS vista;

        /// <summary>
        /// El DataContext que proveerá acceso a la base de datos
        /// </summary>
        private readonly IDataContext dctx;

        /// <summary>
        /// Nombre de la clase para agregar a los mensajes de Error
        /// </summary>
        private string nombreClase = "DetalleContratoManttoPRE";

        /// <summary>
        /// Presentador de la Barra de Herramientas
        /// </summary>
        private readonly ucHerramientasManttoPRE presentadorHerramientas;

        /// <summary>
        /// Presentador de la Información Contrato
        /// </summary>
        private readonly ucContratoManttoPRE presentadorContrato;

        private ucCatalogoDocumentosPRE presentadorDocumentos;

        private ContratoManttoBR controlador;

        #endregion

        #region Constructores

        public DetalleContratoManttoPRE(IDetalleContratoManttoVIS view, IucHerramientasManttoVIS viewHerramientas, IucContratoManttoVIS viewContrato, IucLineaContratoManttoVIS viewLinea, IucCatalogoDocumentosVIS viewDocs)
        {
            try
            {
                this.vista = view;

                this.presentadorContrato = new ucContratoManttoPRE(viewContrato, viewLinea);
                this.presentadorHerramientas = new ucHerramientasManttoPRE(viewHerramientas);
                this.presentadorDocumentos = new ucCatalogoDocumentosPRE(viewDocs);

                this.controlador = new ContratoManttoBR();
                this.dctx = FacadeBR.ObtenerConexion();
            }
            catch (Exception ex)
            {
                this.vista.MostrarMensaje("Inconsistencia en los parámetros de configuración", ETipoMensajeIU.ERROR, this.nombreClase + ".DetalleContratoManttoPRE:" + ex.Message);
            }
        }

        #endregion

        #region Métodos
        public void RealizarPrimeraCarga()
        {
            try
            {
                this.PrepararVisualizacion();

                this.EstablecerDatosNavegacion(this.vista.ObtenerPaqueteNavegacion("ContratoManttoBO"));
                this.vista.PermitirRegresar(this.vista.ObtenerPaqueteNavegacion("FiltrosContratoMantto") != null);

                this.ConsultarCompleto();

                this.EstablecerConfiguracionInicial();
                this.EstablecerSeguridad();
            }
            catch (Exception ex)
            {
                throw new Exception(this.nombreClase + ".RealizarPrimeraCarga: " + ex.Message);
            }
        }
        private void EstablecerDatosNavegacion(object paqueteNavegacion)
        {
            try
            {
                if (paqueteNavegacion == null)
                    throw new Exception("Se esperaba un objeto en la navegación. No se puede identificar qué Contrato se desea consultar.");
                if (!(paqueteNavegacion is ContratoManttoBO))
                    throw new Exception("Se esperaba un Contrato.");

                ContratoManttoBO bo = new ContratoManttoBO { ContratoID = ((ContratoManttoBO)paqueteNavegacion).ContratoID };

                this.DatoAInterfazUsuario(bo);
            }
            catch (Exception ex)
            {
                this.DatoAInterfazUsuario(new ContratoManttoBO());
                throw new Exception(this.nombreClase + ".EstablecerDatosNavegacion: " + ex.Message);
            }
        }
        private void ConsultarCompleto()
        {
            try
            {
                //Se consulta la información del contrato
                ContratoManttoBO bo = (ContratoManttoBO)this.InterfazUsuarioADato();

                List<ContratoManttoBO> lst = this.controlador.ConsultarCompleto(this.dctx, bo,true);

                if (lst.Count < 1)
                    throw new Exception("No se encontró ningún registro que corresponda a la información proporcionada.");
                if (lst.Count > 1)
                    throw new Exception("La consulta devolvió más de un registro.");

                if (lst[0] != null && lst[0].DocumentosAdjuntos != null && lst[0].DocumentosAdjuntos.Count > 0)
                    lst[0].DocumentosAdjuntos = lst[0].DocumentosAdjuntos.Where(archivo => archivo.Activo == true).ToList();

                this.DatoAInterfazUsuario(lst[0]);
                this.presentadorHerramientas.DatosAInterfazUsuario(lst[0]);
                this.vista.UltimoObjeto = lst[0];
            }
            catch (Exception ex)
            {
                this.DatoAInterfazUsuario(new ContratoManttoBO());
                throw new Exception(this.nombreClase + ".ConsultarCompleto:" + ex.Message);
            }
        }
        private void EstablecerConfiguracionInicial()
        {
            try
            {
                this.vista.UUA = this.vista.UsuarioID;
                this.vista.FUA = DateTime.Now;

                this.presentadorDocumentos.EstablecerTiposArchivo(new List<TipoArchivoBO>());
            }
            catch (Exception ex)
            {
                throw new Exception(this.nombreClase + ".EstablecerConfiguracionInicial: " + ex.Message);
            }
        }
        
        public void ValidarAcceso()
        {
            try
            {
                //se valida que los datos del usuario y la unidad operativa no sean nulos
                if (this.vista.UsuarioID == null) throw new Exception("El identificador del usuario no debe ser nulo");
                if (this.vista.UnidadOperativaID == null) throw new Exception("El identificador de la Unidad Operativa no debe ser nulo ");

                //Se crea el objeto de seguridad
                UsuarioBO usuario = new UsuarioBO() { Id = this.vista.UsuarioID };
                AdscripcionBO adscripcion = new AdscripcionBO() { UnidadOperativa = new UnidadOperativaBO() { Id = this.vista.UnidadOperativaID } };
                SeguridadBO seguridadBO = new SeguridadBO(Guid.Empty, usuario, adscripcion);

                //Se valida si el usuario tiene permiso a la acción principal
                if (!FacadeBR.ExisteAccion(dctx, "CONSULTAR", seguridadBO))
                    this.vista.RedirigirSinPermisoAcceso();
            }
            catch (Exception ex)
            {
                throw new Exception(nombreClase + ".ValidarAcceso:" + ex.Message);
            }
        }
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
                List<CatalogoBaseBO> lst = FacadeBR.ConsultarAccion(dctx, seguridad);

                //Se valida si el usuario tiene permiso para registrar
                if (!this.ExisteAccion(lst, "UI INSERTAR"))
                    this.vista.PermitirRegistrar(false);
                //Se valida si el usuario tiene permiso para editar
                if (!this.ExisteAccion(lst, "UI ACTUALIZAR"))
                    this.vista.PermitirEditar(false);
                //Se valida si el usuario tiene permiso para eliminar
                if (!this.ExisteAccion(lst, "BORRARCOMPLETO"))
                    this.vista.PermitirEliminar(false);
                //Se valida si el usuario tiene permiso para cerrar un contrato
                if (!this.ExisteAccion(lst, "UI TERMINARDOCUMENTO"))
                    this.vista.PermitirCerrar(false);
            }
            catch (Exception ex)
            {

                throw new Exception(this.nombreClase + ".EstablecerSeguridad:" + ex.Message);
            }
        }
        private bool ExisteAccion(List<CatalogoBaseBO> acciones, string nombreAccion)
        {
            if (acciones != null && acciones.Count > 0 &&
                acciones.Exists(
                    p => p != null && p.Nombre != null && p.Nombre.Trim().ToUpper() == nombreAccion.Trim().ToUpper()))
                return true;

            return false;
        }

        private void PrepararVisualizacion()
        {
            this.vista.PrepararVisualizacion();
            this.presentadorHerramientas.Inicializar();
            this.presentadorContrato.PrepararVisualizacion();
        }

        private object InterfazUsuarioADato()
        {
            ContratoManttoBO bo = new ContratoManttoBO();
            bo.Cliente = new CuentaClienteIdealeaseBO();
            bo.Cliente = new CuentaClienteIdealeaseBO();
            bo.Cliente.UnidadOperativa = new UnidadOperativaBO();
            bo.Divisa = new DivisaBO();
            bo.Divisa.MonedaDestino = new MonedaBO();
            bo.Sucursal = new SucursalBO();
            bo.Sucursal.UnidadOperativa = new UnidadOperativaBO();
            bo.LineasContrato = new List<ILineaContrato>();

            bo.ContratoID = this.vista.ContratoID;
            if (this.vista.TipoContratoID != null)
                bo.Tipo = (ETipoContrato)Enum.Parse(typeof(ETipoContrato), this.vista.TipoContratoID.ToString());

            return bo;
        }
        private void DatoAInterfazUsuario(object obj)
        {
            ContratoManttoBO bo = (ContratoManttoBO)obj;
            if (bo == null) bo = new ContratoManttoBO();
            if (bo.Cliente == null) bo.Cliente = new CuentaClienteIdealeaseBO();
            if (bo.Divisa == null) bo.Divisa = new DivisaBO();
            if (bo.Divisa.MonedaDestino == null) bo.Divisa.MonedaDestino = new MonedaBO();
            if (bo.Sucursal == null) bo.Sucursal = new SucursalBO();
            if (bo.Sucursal.UnidadOperativa == null) bo.Sucursal.UnidadOperativa = new UnidadOperativaBO();

            this.vista.ContratoID = bo.ContratoID;
            this.vista.NumeroContrato = bo.NumeroContrato;
            this.vista.CodigoMoneda = bo.Divisa.MonedaDestino.Codigo;
            this.vista.FechaContrato = bo.FechaContrato;
            this.vista.RepresentanteEmpresa = bo.Representante;
            if (bo.Tipo != null)
                this.vista.TipoContratoID = (int)bo.Tipo;
            else
                this.vista.TipoContratoID = null;

            this.vista.SucursalID = bo.Sucursal.Id;
            this.vista.SucursalNombre = bo.Sucursal.Nombre;

            //Cuenta de Cliente Idealease
            this.vista.CuentaClienteID = bo.Cliente.Id;
            this.vista.CuentaClienteNombre = bo.Cliente.Nombre;
            if (bo.Cliente.TipoCuenta != null)
                this.vista.CuentaClienteTipoID = (int)bo.Cliente.TipoCuenta;
            else
                this.vista.CuentaClienteTipoID = null;
            this.presentadorContrato.SeleccionarCuentaCliente(bo.Cliente);

            //Dirección del cliente
            if (bo.Cliente.Direcciones != null && bo.Cliente.Direcciones.Count > 0)
            {
                this.vista.DireccionClienteID = bo.Cliente.Direcciones[0].Id;
                this.vista.ClienteDireccionCompleta = bo.Cliente.Direcciones[0].Direccion;
                this.vista.ClienteDireccionCalle = bo.Cliente.Direcciones[0].Calle;
                this.vista.ClienteDireccionColonia = bo.Cliente.Direcciones[0].Colonia;
                this.vista.ClienteDireccionCodigoPostal = bo.Cliente.Direcciones[0].CodigoPostal;
                this.vista.ClienteDireccionCiudad = bo.Cliente.Direcciones[0].Ubicacion.Ciudad.Codigo;
                this.vista.ClienteDireccionEstado = bo.Cliente.Direcciones[0].Ubicacion.Estado.Codigo;
                this.vista.ClienteDireccionMunicipio = bo.Cliente.Direcciones[0].Ubicacion.Municipio.Codigo;
                this.vista.ClienteDireccionPais = bo.Cliente.Direcciones[0].Ubicacion.Pais.Codigo;
            }
            else
            {
                this.vista.DireccionClienteID = null;
                this.vista.ClienteDireccionCompleta = null;
                this.vista.ClienteDireccionCalle = null;
                this.vista.ClienteDireccionColonia = null;
                this.vista.ClienteDireccionCodigoPostal = null;
                this.vista.ClienteDireccionCiudad = null;
                this.vista.ClienteDireccionEstado = null;
                this.vista.ClienteDireccionMunicipio = null;
                this.vista.ClienteDireccionPais = null;
            }

            if (bo.RepresentantesLegales != null)
                this.vista.RepresentantesLegales = bo.RepresentantesLegales.ConvertAll(s => (RepresentanteLegalBO)s);
            else
                this.vista.RepresentantesLegales = null;            
            if (bo.ObligadosSolidarios != null)
                this.vista.ObligadosSolidarios = bo.ObligadosSolidarios.ConvertAll(s => (ObligadoSolidarioBO)s);
            else
                this.vista.ObligadosSolidarios = null;
            this.vista.Avales = bo.Avales;
            this.vista.SoloRepresentantes = bo.SoloRepresentantes;
            this.vista.ObligadosComoAvales = bo.ObligadosComoAvales;
            this.presentadorContrato.ConfigurarOpcionesPersonas();

            this.vista.Plazo = bo.Plazo;
            this.vista.FechaInicioContrato = bo.FechaInicioContrato;
            this.vista.FechaTerminacionContrato = bo.CalcularFechaTerminacionContrato();

            if (bo.LineasContrato != null)
                this.vista.LineasContrato = bo.LineasContrato.ConvertAll(s => (LineaContratoManttoBO)s);
            else
                this.vista.LineasContrato = null;
            this.vista.UbicacionTaller = bo.UbicacionTaller;
            this.vista.DireccionAlmacenaje = bo.DireccionAlmacenaje;
            this.vista.DepositoGarantia = bo.DepositoGarantia;
            this.vista.ComisionApertura = bo.ComisionApertura;
            if (bo.IncluyeLavado != null)
                this.vista.IncluyeLavadoID = (int)bo.IncluyeLavado;
            else
                this.vista.IncluyeLavadoID = null;
            if (bo.IncluyeLlantas != null)
                this.vista.IncluyeLlantasID = (int)bo.IncluyeLlantas;
            else
                this.vista.IncluyeLlantasID = null;
            if (bo.IncluyePinturaRotulacion != null)
                this.vista.IncluyePinturaRotulacionID = (int)bo.IncluyePinturaRotulacion;
            else
                this.vista.IncluyePinturaRotulacionID = null;
            if (bo.IncluyeSeguro != null)
                this.vista.IncluyeSeguroID = (int)bo.IncluyeSeguro;
            else
                this.vista.IncluyeSeguroID = null;

            this.vista.Observaciones = bo.Observaciones;
            this.vista.DatosAdicionales = bo.DatosAdicionalesAnexo;
            this.vista.UC = bo.UC;
            this.vista.UUA = bo.UUA;
            this.vista.FC = bo.FC;
            this.vista.FUA = bo.FUA;
            if (bo.Estatus != null)
                this.vista.EstatusID = (int)bo.Estatus;
            else
                this.vista.EstatusID = null;

            this.presentadorDocumentos.CargarListaArchivos(bo.DocumentosAdjuntos, this.presentadorDocumentos.Vista.TiposArchivo);
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

            this.presentadorHerramientas.CargarArchivos(resultado);
        }

        /// <summary>
        /// Envia el contrato a Editar Contrato
        /// </summary>
        public void EditarContrato()
        {
            try
            {
                ContratoManttoBO bo = (ContratoManttoBO)this.InterfazUsuarioADato();

                this.LimpiarSesion();
                this.vista.EstablecerPaqueteNavegacion("ContratoManttoBO", bo);

                this.vista.RedirigirAEditar();
            }
            catch (Exception ex)
            {
                throw new Exception(nombreClase + ".EditarContrato: " + ex.Message);
            }
        }
        /// <summary>
        /// Elimina el contrato desplegado con estatus de borrador
        /// </summary>
        public void EliminarContrato()
        {
            try
            {
                #region InterfazUsuarioADato Personalizado
                ContratoManttoBO bo = (ContratoManttoBO)this.vista.UltimoObjeto;

                if (bo.Estatus != EEstatusContrato.Borrador)
                {
                    vista.MostrarMensaje("El contrato debe tener estatus Borrador para ser eliminado.", ETipoMensajeIU.INFORMACION);
                    return;
                }

                //finalización del contrato
                FinalizacionContratoProxyBO finalizacionContrato = new FinalizacionContratoProxyBO();
                finalizacionContrato.Fecha = vista.FUA;
                finalizacionContrato.Usuario = new UsuarioBO { Id = vista.UUA };
                finalizacionContrato.Observaciones = vista.Observaciones;

                ContratoManttoBO previous = new ContratoManttoBO(bo);

                bo.CierreContrato = finalizacionContrato;
                bo.FUA = vista.FUA;
                bo.UUA = vista.UUA;
                #endregion

                //Se crea el objeto de seguridad
                UsuarioBO usuario = new UsuarioBO() { Id = this.vista.UsuarioID };
                AdscripcionBO adscripcion = new AdscripcionBO() { UnidadOperativa = new UnidadOperativaBO() { Id = this.vista.UnidadOperativaID } };
                SeguridadBO seguridadBO = new SeguridadBO(Guid.Empty, usuario, adscripcion);

                this.controlador.BorrarCompleto(dctx, bo, previous, seguridadBO);

                this.LimpiarSesion();
                this.vista.LimpiarPaqueteNavegacion("ContratoManttoBO");
                this.vista.RedirigirAConsulta();
            }
            catch (Exception ex)
            {
                throw new Exception(this.nombreClase + ".EliminarContrato: " + ex.Message);
            }
        }
        public void CerrarContrato()
        {
            try
            {
                ContratoManttoBO bo = (ContratoManttoBO)this.InterfazUsuarioADato();

                this.LimpiarSesion();
                this.vista.EstablecerPaqueteNavegacion("ContratoManttoBO", bo);

                this.vista.RedirigirACierre();
            }
            catch (Exception ex)
            {
                throw new Exception(nombreClase + ".EditarContrato: " + ex.Message);
            }
        }
        public void CancelarContrato()
        {
            try
            {
                ContratoManttoBO bo = (ContratoManttoBO)this.InterfazUsuarioADato();

                this.LimpiarSesion();
                this.vista.EstablecerPaqueteNavegacion("ContratoManttoBO", bo);

                this.vista.RedirigirACancelacion();
            }
            catch (Exception ex)
            {
                throw new Exception(nombreClase + ".EditarContrato: " + ex.Message);
            }
        }
        /// <summary>
        /// Envia el contrato a Agregar Documentos
        /// </summary>
        public void AgregarDocumentos()
        {
            try
            {
                ContratoManttoBO bo = (ContratoManttoBO)this.InterfazUsuarioADato();

                this.LimpiarSesion();
                this.vista.EstablecerPaqueteNavegacion("ContratoManttoBO", bo);

                this.vista.RedirigirAAgregarDocumentos();
            }
            catch (Exception ex)
            {
                throw new Exception(nombreClase + ".AgregarDocumentos: " + ex.Message);
            }
        }

        public void ImprimirContrato()
        {
            try
            {
                if (this.vista.ContratoID == null)
                    throw new Exception("No se puede imprimir el contrato sin el identificador del contrato");

                Dictionary<string, Object> datosContrato = this.controlador.ObtenerDatosContrato(dctx, new ContratoManttoBO() { ContratoID = this.vista.ContratoID });
                vista.EstablecerPaqueteNavegacion("NombreReporte", "CU031");
                vista.EstablecerPaqueteNavegacion("DatosReporte", datosContrato);
                vista.RedirigirAImprimir();
            }
            catch (Exception ex)
            {
                throw new Exception(this.nombreClase + ".ImprimirContrato: " + ex.Message);
            }
        }
		public void ImprimirManualOperaciones()
        {
            try
            {
                if (this.vista.ContratoID == null)
                    throw new Exception("No se puede imprimir el manual de operaciones sin el identificador del contrato");
                if (this.vista.UnidadOperativaID == null)
                    throw new Exception("No se puede imprimir el manual de operaciones sin el identificador de la unidad operativa");

                var datosReporte = this.controlador.ConsultarManualOperaciones(this.dctx, this.vista.ContratoID.Value, this.vista.UnidadOperativaID.Value);

                this.vista.EstablecerPaqueteNavegacion("NombreReporte", "CU017");
                this.vista.EstablecerPaqueteNavegacion("DatosReporte", datosReporte);
                this.vista.RedirigirAImprimir();
            }
            catch (Exception ex)
            {
                throw new Exception(this.nombreClase + ".ImprimirManualOperaciones: " + ex.Message);
            }
		}
        public void ImprimirAnexoA()
        {
            try
            {
                if (this.vista.ContratoID == null)
                    throw new Exception("No se puede imprimir el anexo A sin el identificador del contrato");

                var datosReporte = this.controlador.ObtenerDatosAnexoA(this.dctx, this.vista.ContratoID.Value);

                this.vista.EstablecerPaqueteNavegacion("NombreReporte", "CU099");
                this.vista.EstablecerPaqueteNavegacion("DatosReporte", datosReporte);
                this.vista.RedirigirAImprimir();
            }
            catch (Exception ex)
            {
                throw new Exception(this.nombreClase + ".ImprimirAnexoA: " + ex.Message);
            }
        }
        public void ImprimirAnexoB()
        {
            throw new Exception("Esta funcionalidad no se encuentra implementada aún.");
        }
        public void ImprimirAnexoC()
        {
            try
            {
                if (this.vista.ContratoID == null)
                    throw new Exception("No se puede imprimir el anexo C sin el identificador del contrato");

                var datosReporte = this.controlador.ObtenerDatosAnexoC(this.dctx, new ContratoManttoBO() { ContratoID = this.vista.ContratoID });

                this.vista.EstablecerPaqueteNavegacion("NombreReporte", "CU021");
                this.vista.EstablecerPaqueteNavegacion("DatosReporte", datosReporte);
                this.vista.RedirigirAImprimir();
            }
            catch (Exception ex)
            {
                throw new Exception(this.nombreClase + ".ImprimirAnexoC: " + ex.Message);
            }
        }
        public void ImprimirPagare()
        {
            try
            {
                if (this.vista.ContratoID == null)
                    throw new Exception("No se puede imprimir el pagaré sin el identificador del contrato");
                Dictionary<string, Object> datosPagare = this.controlador.ObtenerDatosPagare(dctx, new ContratoManttoBO() { ContratoID = this.vista.ContratoID });
                if (vista.TipoContratoID != null && ((ETipoContrato)vista.TipoContratoID == ETipoContrato.CM))
                    vista.EstablecerPaqueteNavegacion("NombreReporte", "CU095");
                else if (vista.TipoContratoID != null && ((ETipoContrato)vista.TipoContratoID == ETipoContrato.SD))
                    vista.EstablecerPaqueteNavegacion("NombreReporte", "CU096");
                vista.EstablecerPaqueteNavegacion("DatosReporte", datosPagare);
                vista.RedirigirAImprimir();

            }
            catch (Exception ex)
            {
                throw new Exception(this.nombreClase + ".ImprimirPagare: " + ex.Message);
            }
        }
        public void ImprimirTodo()
        {
            try
            {
                if (this.vista.ContratoID == null)
                    throw new Exception("No se pueden imprimir los documentos sin el identificador del contrato");
                if (this.vista.UnidadOperativaID == null)
                    throw new Exception("No se pueden imprimir los documentos sin el identificador de la unidad operativa");
                if (this.vista.TipoContratoID == null)
                    throw new Exception("No se pueden imprimir los documentos sin el tipo de contrato");

                string tipoMantenimiento = ((ETipoContrato)Enum.Parse(typeof(ETipoContrato), this.vista.TipoContratoID.ToString())).ToString();
                
                var datosReporte = new Dictionary<string, object>();
                datosReporte.Add("TipoMantenimiento", tipoMantenimiento);
                datosReporte.Add("CU031", this.controlador.ObtenerDatosContrato(dctx, new ContratoManttoBO() { ContratoID = this.vista.ContratoID }));
                datosReporte.Add("CU099", this.controlador.ObtenerDatosAnexoA(this.dctx, this.vista.ContratoID.Value));
                datosReporte.Add("CU021", this.controlador.ObtenerDatosAnexoC(this.dctx, new ContratoManttoBO() { ContratoID = this.vista.ContratoID }));
				if ((ETipoContrato)Enum.Parse(typeof(ETipoContrato), this.vista.TipoContratoID.ToString()) == ETipoContrato.CM)
				{
					datosReporte.Add("CU017", this.controlador.ConsultarManualOperaciones(this.dctx, this.vista.ContratoID.Value, this.vista.UnidadOperativaID.Value));
					datosReporte.Add("CU095",this.controlador.ObtenerDatosPagare(dctx,new ContratoManttoBO { ContratoID=vista.ContratoID }));
				}
				else 
                    datosReporte.Add("CU096", this.controlador.ObtenerDatosPagare(dctx, new ContratoManttoBO { ContratoID = vista.ContratoID })); 
	            this.vista.EstablecerPaqueteNavegacion("NombreReporte", "CU029");
                this.vista.EstablecerPaqueteNavegacion("DatosReporte", datosReporte);

                this.vista.RedirigirAImprimir();
            }
            catch (Exception ex)
            {
                throw new Exception(this.nombreClase + ".ImprimirTodo: " + ex.Message);
            }
        }

        public void Regresar()
        {
            this.LimpiarSesion();
            this.vista.LimpiarPaqueteNavegacion("ContratoManttoBO");
            this.vista.RedirigirAConsulta();
        }

        private void LimpiarSesion()
        {
            this.vista.LimpiarSesion();
            this.presentadorDocumentos.LimpiarSesion();
            this.presentadorContrato.LimpiarSesion();
        }
        #endregion
    }
}
