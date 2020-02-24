using System;
using System.Collections.Generic;
using System.Linq;
using BPMO.Basicos.BO;
using BPMO.Patterns.Creational.DataContext;
using BPMO.Primitivos.Enumeradores;
using BPMO.SDNI.Comun.BO;
using BPMO.SDNI.Contratos.PSL.BO;
using BPMO.SDNI.Contratos.PSL.BR;
using BPMO.SDNI.Contratos.PSL.VIS;

namespace BPMO.SDNI.Contratos.PSL.PRE {
    public class TarifaPersonalizadaPSLPRE {
        #region Atributos
        private string nombreClase = "TarifaPersonalizadaPSLPRE";
        /// <summary>
        /// Vista sobre la que actúa el presentador
        /// </summary>
        readonly ITarifaPersonalizadaPSLVIS vista;
        /// <summary>
        /// El DataContext que proveerá acceso a la base de datos
        /// </summary>
        readonly IDataContext dataContext;
        #endregion

        #region Propiedades
        /// <summary>
        /// Vista sobre la que actúa el presentador
        /// </summary>
        internal ITarifaPersonalizadaPSLVIS Vista { get { return vista; } }
        #endregion

        #region Constructores
        /// <summary>
        /// Constructor que recibe la interfaz sobre la que actuará el presentador
        /// </summary>
        /// <param name="vistaActual"></param>
        public TarifaPersonalizadaPSLPRE(ITarifaPersonalizadaPSLVIS vistaActual) {
            try {
                vista = vistaActual;
                dataContext = Facade.SDNI.BR.FacadeBR.ObtenerConexion();
            } catch (Exception ex) {
                vista.MostrarMensaje("Inconsistencias en los parámetros de configuración", ETipoMensajeIU.ERROR, ex.Message);
            }
        }
        #endregion

        #region Metodos

        /// <summary>
        ///  Inicializa la Vista
        /// </summary>
        public void Inicializar() {
            this.vista.Inicializar();
            this.EstablecerDatosNavegacion(this.vista.ObtenerPaqueteNavegacion("TarifaPersonalizaPSLEnviada"));
            vista.PermitirValidarCodigoAutorizacion(false);
            vista.PermitirSolicitarCodigoAutorizacion(true);
            this.PrepararDescuentoMaximo();
        }
        /// <summary>
        /// Establece le paquete de datos de navegación
        /// </summary>
        /// <param name="paqueteNavegacion">Paquete de Navegación</param>
        private void EstablecerDatosNavegacion(object paqueteNavegacion) {
            try {
                if (paqueteNavegacion == null)
                    throw new Exception("Se esperaba un objeto en la navegación. No se puede identificar qué contrato se desea consultar.");
                if (!(paqueteNavegacion is TarifaPersonalizadaPSLModel))
                    throw new Exception("Se esperaba un Contrato de Renta Ordinaría.");

                TarifaPersonalizadaPSLModel bo = (TarifaPersonalizadaPSLModel)paqueteNavegacion;

                this.DatoAInterfazUsuario(bo);
            } catch (Exception ex) {
                this.DatoAInterfazUsuario(new TarifaPersonalizadaPSLModel());
                throw new Exception(this.nombreClase + ".EstablecerDatosNavegacion: " + ex.Message);
            }
        }

        /// <summary>
        /// Despliega los datos de la unidad a la interfaz
        /// </summary>
        /// <param name="unidad"></param>
        private void DatoAInterfazUsuario(TarifaPersonalizadaPSLModel tarifa) {
            vista.UnidadOperativaID = tarifa.UnidadOperativaID;
            vista.SucursalID = tarifa.SucursalID;
            vista.ModeloID = tarifa.ModeloID;
            vista.ModuloID = tarifa.ModuloID;
            vista.CuentaClienteID = tarifa.CuentaClienteID;
            vista.UsuarioID = tarifa.UsuarioID;
            vista.TarifaPersonalizadaTarifa = tarifa.TarifaPersonalizadaTarifa;
            vista.TarifaPersonalizadaTipoTarifa = tarifa.TarifaPersonalizadaTipoTarifa;//Se asigna el get de la misma propiedad, esta regresa un string con el tipo tarifa seleccionada
            vista.TarifaPersonalizadaTurno = tarifa.TarifaPersonalizadaTurno;//Se asigna el get de la misma propiedad, esta regresa un string con el turno seleccionado
            vista.TarifaPersonalizadaTarifaHrAdicional = tarifa.TarifaPersonalizadaTarifaHrAdicional;
            vista.TarifaPersonalizadaDescuentoMax = tarifa.TarifaPersonalizadaDescuentoMax;

            vista.TarifaPersonalizadaTarifaConDescuento = tarifa.TarifaPersonalizadaTarifaConDescuento;
            vista.TarifaPersonalizadaPorcentajeDescuento = tarifa.TarifaPersonalizadaPorcentajeDescuento;
            vista.TarifaBase = tarifa.TarifaPersonalizadaTarifa;
            vista.DescuentoBase = tarifa.TarifaPersonalizadaPorcentajeDescuento;
            vista.TarifaPersonalizadaPorcentajeSeguro = tarifa.TarifaPersonalizadaPorcentajeSeguro;
        }

        #region Métodos para la Personalización de Tarifas
        /// <summary>
        /// Método que realiza la solicitud del código de autorización
        /// </summary>
        public void SolicitarAutorizacionTarifaPersonalizada() {
            string s;
            if ((s = this.ValidarCamposTarifaPersonalizada()) != null) {
                this.vista.MostrarMensaje(s, ETipoMensajeIU.ADVERTENCIA, null);
                this.vista.PermitirValidarCodigoAutorizacion(false);
                this.vista.PermitirSolicitarCodigoAutorizacion(true);
                return;
            }

            try {
                this.vista.TarifaPersonalizadaCodigoAutorizacion = null;

                ////Interfaz de usuario a dato
                Dictionary<string, string> datos = new Dictionary<string, string>();
                datos["etiqueta"] = this.vista.TarifaPersonalizadaEtiqueta;
                datos["tarifa"] = this.vista.TarifaPersonalizadaTarifa.GetValueOrDefault().ToString();
                datos["porcentajeDescuentoAplicar"] = this.vista.TarifaPersonalizadaPorcentajeDescuento.GetValueOrDefault().ToString();
                datos["tarifaConDescuento"] = this.vista.TarifaPersonalizadaTarifaConDescuento.GetValueOrDefault().ToString();
                datos["tipoTarifa"] = this.vista.TarifaPersonalizadaTipoTarifa;
                datos["turno"] = this.vista.TarifaPersonalizadaTurno;
                datos["tarifaHoraAdicional"] = this.vista.TarifaPersonalizadaTarifaHrAdicional.GetValueOrDefault().ToString();

                this.vista.TarifaPersonalizadaCodigoAutorizacion = new ContratoPSLBR().SolicitarAutorizacionTarifaPersonalizada(this.dataContext, datos, this.vista.ModuloID, this.vista.UnidadOperativaID, this.vista.UsuarioID, this.vista.SucursalID, this.vista.CuentaClienteID);
                vista.PermitirValidarCodigoAutorizacion(true);
                vista.PermitirSolicitarCodigoAutorizacion(false);
            } catch (Exception ex) {
                throw new Exception(this.nombreClase + ".SolicitarAutorizacionTarifaPersonalizada: " + ex.Message);
            }
        }
        /// <summary>
        /// Valida que los campos requeridos para el envío de la solicitud de autorización se encuentren capturados.
        /// </summary>
        /// <returns>Devuelve una cadena con el error en caso de existir</returns>
        private string ValidarCamposTarifaPersonalizada() {
            string s = string.Empty;
            if (this.vista.TarifaPersonalizadaTarifa == null)
                s += "Tarifa, ";
            if (this.vista.TarifaPersonalizadaPorcentajeDescuento == null)
                s += "Porcentaje de descuento a aplicar , ";
            if (this.vista.TarifaPersonalizadaTarifaConDescuento == null)
                s += "Tarifa con descuento, ";
            if (this.vista.TarifaPersonalizadaTipoTarifa == string.Empty)
                s += "Tipo de Tarifa, ";
            if (this.vista.TarifaPersonalizadaTurno == string.Empty)
                s += "Turno, ";
            if (this.vista.TarifaPersonalizadaTarifaHrAdicional == null)
                s += "Tarifa Hora Adicional, ";

            if (s.Trim().CompareTo("") != 0)
                return "Los siguientes campos no pueden estar vacíos: \n" + s.Substring(0, s.Length - 2);

            return null;
        }

        /// <summary>
        /// Valida la confirmación del código con el código enviado en la autorización, en caso de que código se encuentre vacío validará
        /// el descuento máximo permitido contra el descuento capturado
        /// </summary>
        /// <param name="confirmacionCodigo">Confirmación del código de autorización</param>
        /// <returns>Retorna un mensaje en caso de haber error</returns>
        public string ValidarPersonalizarTarifa(string confirmacionCodigo) {
            string mensaje = string.Empty;
            if (string.IsNullOrEmpty(this.vista.TarifaPersonalizadaCodigoAutorizacion)) {
                decimal descuento = this.vista.TarifaPersonalizadaPorcentajeDescuento.GetValueOrDefault();
                decimal tarifaMaximaDescuento = this.vista.TarifaPersonalizadaDescuentoMax.GetValueOrDefault();
                if (descuento > tarifaMaximaDescuento) {
                    mensaje = "El descuento capturado excede al m&aacute;ximo de descuento permitido para el cliente, por favor verifique";
                }
            } else {
                if (confirmacionCodigo.Trim().CompareTo(this.vista.TarifaPersonalizadaCodigoAutorizacion) != 0)
                    mensaje = "El c&oacute;digo de autorizaci&oacute;n es inv&aacute;lido, por favor verifique";
                else
                    this.vista.TarifaPersonalizadaDescuentoMax = this.vista.TarifaPersonalizadaPorcentajeDescuento;
            }

            return mensaje;
        }

        public void ValidarAumentoTarifa() {
            try {
                if (this.vista.TarifaBase < this.vista.TarifaPersonalizadaTarifa){
                    this.vista.TarifaPersonalizadaPorcentajeDescuento = 0;
                    this.vista.TarifaPersonalizadaTarifaConDescuento = 0;             
                
                    this.vista.EstablecerEtiquetaBoton("Aplicar");
                     //Habilitaremos el botón de procesar y se bloqueara el control de código de autorización
                    this.vista.PermitirAplicarSinCodigoAutorizacion(true);
                }else {
                    this.vista.MostrarMensaje ("La tarifa no puede ser menor a la tarifa minima de " + this.vista.TarifaBase.ToString(), ETipoMensajeIU.ADVERTENCIA);
                }
            } catch (Exception ex) {                
                 throw new Exception(this.nombreClase + ".ActualizarAumentoTarifa: " + ex.Message);
            }        
        }

        /// <summary>
        /// Calcula la tarifa de la unidad, dependiendo de los campos Periodo, Tarifa Turno, Tipo Tarifa, Unidad Operativa y Modelo de la unidad.
        /// </summary>
        public void ObtieneTarifaBase() {
            try {
                    TarifaPSLBO tarifaBO = new TarifaPSLBO();
                    TarifaPersonalizadaPSLModel tarifaModel = (TarifaPersonalizadaPSLModel)this.vista.ObtenerPaqueteNavegacion("TarifaPersonalizaPSLEnviada");
                    tarifaBO.PeriodoTarifa = tarifaModel.TarifaBasePeriodoTarifa;
                    tarifaBO.TipoTarifaID = tarifaModel.TarifaPersonalizadaTipoTarifa;
                    tarifaBO.Divisa = tarifaModel.TarifaBaseDivisa;
                    int valorTurno;
                    switch (this.vista.UnidadOperativaID) {
                        case (int)ETipoEmpresa.Construccion:
                            valorTurno = BPMO.Facade.SDNI.BR.FacadeBR.ObtenerValorEnumerador<ETarifaTurnoConstruccion>(tarifaModel.TarifaPersonalizadaTurno);
                            tarifaBO.TarifaTurno = (ETarifaTurnoConstruccion)valorTurno;
                            break;
                        case (int)ETipoEmpresa.Generacion:
                            valorTurno = BPMO.Facade.SDNI.BR.FacadeBR.ObtenerValorEnumerador<ETarifaTurnoGeneracion>(tarifaModel.TarifaPersonalizadaTurno);
                            tarifaBO.TarifaTurno = (ETarifaTurnoGeneracion)valorTurno;
                            break;
                        case (int)ETipoEmpresa.Equinova:
                            valorTurno = BPMO.Facade.SDNI.BR.FacadeBR.ObtenerValorEnumerador<ETarifaTurnoEquinova>(tarifaModel.TarifaPersonalizadaTurno);
                            tarifaBO.TarifaTurno = (ETarifaTurnoEquinova)valorTurno;
                            break;
                    }                     
                    tarifaBO.Modelo = new BPMO.Servicio.Catalogos.BO.ModeloBO { Id = this.vista.ModeloID };
                    tarifaBO.Sucursal = new SucursalBO { Id = this.vista.SucursalID, UnidadOperativa = new UnidadOperativaBO { Id = this.vista.UnidadOperativaID } };
                    tarifaBO.Activo = true;
                    TarifaPSLBR tarifaBR = new TarifaPSLBR();
                    List<TarifaPSLBO> lstTarifas = tarifaBR.Consultar(this.dataContext, tarifaBO);
                    if (lstTarifas!= null && lstTarifas.Count > 0 ) {
                        this.vista.TarifaBase = lstTarifas[0].Tarifa;                       
                    } else {
                        this.vista.MostrarMensaje("No se encontró una tarifa configurada.", ETipoMensajeIU.ADVERTENCIA);
                    }
            } catch  {
                throw new Exception(this.nombreClase + ".ObtieneTarifaBase: Error al obtener la tarifa");
            }
        }
        /// <summary>
        /// Obtiene el valor a mostrar en la etiqueta máximo descuento
        /// </summary>
        public void PrepararDescuentoMaximo() {
            try {
                dataContext.SetCurrentProvider("Outsourcing");
                ConfiguracionDescuentoBO descuentoBO = new ConfiguracionDescuentoBO();
                descuentoBO.Cliente = new CuentaClienteIdealeaseBO { Id = this.vista.CuentaClienteID };
                descuentoBO.Sucursal = new SucursalBO { Id = this.vista.SucursalID, UnidadOperativa = new UnidadOperativaBO { Id = this.vista.UnidadOperativaID } };
                descuentoBO.UnidadOperativa = new UnidadOperativaBO { Id = this.vista.UnidadOperativaID };
                descuentoBO.Estado = true;
                descuentoBO.FechaInicio = DateTime.Now;
                ConfiguracionDescuentoPSLBR descuentoBR = new ConfiguracionDescuentoPSLBR();
                var descuentos = descuentoBR.Consultar(this.dataContext, descuentoBO);

                //Asignamos el % de descuento máximo a la propiedad equivalente en el modal
                //Con la intención de que previamente se haya autorizado un descuento
                decimal descuento = this.vista.TarifaPersonalizadaDescuentoMax.GetValueOrDefault();
                this.vista.TarifaPersonalizadaDescuentoMax = descuento;
                if (descuentos.Any()) {
                    //Si el descuento anterior es menor al actual, el valor del descuento del modal se sobreescribe.
                    if (descuento < descuentos.First().DescuentoMaximo.GetValueOrDefault()) {
                        descuento = descuentos.First().DescuentoMaximo.GetValueOrDefault();
                        this.vista.TarifaPersonalizadaDescuentoMax = descuento;
                    }
                }
                if (descuento > 0)
                    this.vista.TarifaPersonalizadaEtiqueta = "El cliente tiene autorizado hasta un " + this.vista.TarifaPersonalizadaDescuentoMax.GetValueOrDefault().ToString("0.##") + "% de descuento";
            } catch (Exception ex) {
                throw new Exception(this.nombreClase + ".PrepararDescuentoMaximo: " + ex.Message);
            }
        }

        /// <summary>
        /// Verifica si el descuento capturado por el cliente no excede al máximo descuento configurado.
        /// </summary>
        public void ValidarDescuentoPermitido() {
            decimal tarifa = 0;
            decimal descuento = this.vista.TarifaPersonalizadaPorcentajeDescuento.GetValueOrDefault();
            decimal tarifaConDescuento = 0;
            decimal tarifaMaximaDescuento = 0;

            if (descuento < 0 || descuento > 100) {
                this.vista.TarifaPersonalizadaPorcentajeDescuento = null;
                this.vista.TarifaPersonalizadaTarifaConDescuento = null;
                this.vista.MostrarMensaje("El % de descuento deberá estar entre el rango de 0 y 100", ETipoMensajeIU.ADVERTENCIA);
                this.vista.EstablecerEtiquetaBoton("Validar");
                this.vista.PermitirValidarCodigoAutorizacion(false);
                this.vista.PermitirSolicitarCodigoAutorizacion(true);
            } else {
                tarifa = this.vista.TarifaPersonalizadaTarifa.GetValueOrDefault();
                tarifaConDescuento = Math.Round(tarifa - (tarifa * descuento / 100), 2); // Se redondeamos a dos posiciones
                tarifaMaximaDescuento = this.vista.TarifaPersonalizadaDescuentoMax.GetValueOrDefault();
                this.vista.TarifaPersonalizadaTarifaConDescuento = tarifaConDescuento;
                if (descuento <= tarifaMaximaDescuento) {
                    this.vista.EstablecerEtiquetaBoton("Aplicar");
                    //Habilitaremos el botón de procesar y se bloqueara el control de código de autorización
                    this.vista.PermitirAplicarSinCodigoAutorizacion(true);
                } else {
                    this.vista.EstablecerEtiquetaBoton("Validar");
                    this.vista.PermitirValidarCodigoAutorizacion(false);
                    this.vista.PermitirSolicitarCodigoAutorizacion(true);
                }
            }
        }

        /// <summary>
        /// Actualiza la tarifa seleccionada con los datos de la tarifa personalizada
        /// </summary>
        public void ActualizarTarifa() {
            TarifaPersonalizadaPSLModel tarifa = new TarifaPersonalizadaPSLModel();
            tarifa.TarifaPersonalizadaEtiqueta = this.vista.esTarifaAlza ? null : this.vista.TarifaPersonalizadaEtiqueta;
            
            tarifa.TarifaPersonalizadaTarifa = this.vista.TarifaPersonalizadaTarifa;
            tarifa.TarifaPersonalizadaDescuentoMax = this.vista.esTarifaAlza ? null : this.vista.TarifaPersonalizadaDescuentoMax;
            tarifa.TarifaPersonalizadaPorcentajeDescuento = this.vista.esTarifaAlza ? null : this.vista.TarifaPersonalizadaPorcentajeDescuento;
            tarifa.TarifaPersonalizadaTarifaConDescuento = this.vista.esTarifaAlza ? null : this.vista.TarifaPersonalizadaTarifaConDescuento;

            this.vista.LimpiarPaqueteNavegacion("TarifaPersonalizaPSLDevuelta");
            this.vista.EstablecerPaqueteNavegacion("TarifaPersonalizaPSLDevuelta", tarifa);

            this.vista.MostrarMensaje("Se ha personalizado la tarifa con éxito", ETipoMensajeIU.EXITO);
        }
        #endregion
        #endregion
    }
    public class TarifaPersonalizadaPSLModel {
        public int? UnidadOperativaID { get; set; }
        public int? ModeloID { get; set; }
        public int? ModuloID { get; set; }
        public int? SucursalID { get; set; }
        public int? UsuarioID { get; set; }
        public int? CuentaClienteID { get; set; }


        //Tarifa personalizada
        public string TarifaPersonalizadaEtiqueta { get; set; }
        public decimal? TarifaPersonalizadaTarifa { get; set; }
        public decimal? TarifaPersonalizadaTarifaConDescuento { get; set; }
        public string TarifaPersonalizadaTurno { get; set; }
        public string TarifaPersonalizadaCodigoAutorizacion { get; set; }
        public decimal? TarifaPersonalizadaPorcentajeDescuento { get; set; }
        public string TarifaPersonalizadaTipoTarifa { get; set; }
        public decimal? TarifaPersonalizadaTarifaHrAdicional { get; set; }
        public decimal? TarifaPersonalizadaDescuentoMax { get; set; }
        public decimal? TarifaPersonalizadaPorcentajeSeguro { get; set; }
        //Obtener Tarifa Base 
        public EPeriodosTarifa TarifaBasePeriodoTarifa { get; set; }
        public DivisaBO TarifaBaseDivisa { get; set; }
        //public BPMO.Servicio.Catalogos.BO.ModeloBO TarifaBaseModelo { get; set; }
        public bool? TarifaBaseActivo { get; set; }
        public ETipoTarifa TarifaBaseTipoTarifaID { get; set; }
    }
}