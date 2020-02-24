// Satisface al CU022 - Consultar Contratos Full Service Leasing
// Satisface al CU015 - Registrar Contrato Full Service Leasing
// Satisface al CU023 - Editar Contrato Full Service Leasing
using System;
using System.Collections.Generic;
using System.Linq;
using BPMO.Basicos.BO;
using BPMO.Patterns.Creational.DataContext;
using BPMO.Primitivos.Enumeradores;
using BPMO.SDNI.Contratos.FSL.BO;
using BPMO.SDNI.Contratos.FSL.VIS;
using BPMO.SDNI.Equipos.BO;
using BPMO.SDNI.Equipos.BR;
using BPMO.SDNI.Tramites.BO;
using BPMO.SDNI.Tramites.BR;
using BPMO.Servicio.Catalogos.BO;
using UnidadBO = BPMO.SDNI.Equipos.BO.UnidadBO;

namespace BPMO.SDNI.Contratos.FSL.PRE
{
    public class ucLineaContratoFSLPRE
    {
        #region Atributos
        /// <summary>
        /// Vista sobre la que actua el presentador
        /// </summary>
        readonly IucLineaContratoFSLVIS vista;
        /// <summary>
        /// El DataContext que proveerá acceso a la base de datos
        /// </summary>
        readonly IDataContext dataContext;
        #endregion

        #region Propiedades
        /// <summary>
        /// Vista sobre la que actua el presentador
        /// </summary>
        internal IucLineaContratoFSLVIS Vista { get { return vista; } }
        #endregion

        #region Constructores
        /// <summary>
        /// Constructor que recibe la interfaz sobre la que actuará el presentador
        /// </summary>
        /// <param name="vistaActual"></param>
        public ucLineaContratoFSLPRE(IucLineaContratoFSLVIS vistaActual)
        {
            try
            {
                vista = vistaActual;
                dataContext = Facade.SDNI.BR.FacadeBR.ObtenerConexion();
            }
            catch (Exception ex)
            {
                vista.MostrarMensaje("Inconsistencias en los parametros de configuración", ETipoMensajeIU.ERROR, ex.Message);
            }
        }
        #endregion

        #region Metodos
        /// <summary>
        /// Prepera para una nueva linea de contrato
        /// </summary>
        private void PrepararNuevaLinea()
        {
            vista.CargoFijoMes = null;
            vista.ComisionApertura = null;
            vista.DepositoGarantia = null;
            vista.KmEstimadoAnual = null;
            vista.KmInicial = null;
            vista.ListadoTiposCotizacion = null;
            vista.Modelo = null;
            vista.NumeroEconocimico = null;
            vista.PBC = null;
            vista.PBV = null;
            vista.VIN = null;
            vista.ProductoServicioId = null;
            vista.ClaveProductoServicio = null;
            vista.DescripcionProductoServicio = null;
            vista.LimpiarSesion();
        }

        /// <summary>
        ///  Inicializa la Vista
        /// </summary>
        public void Inicializar()
        {
            PrepararNuevaLinea();
            MostrarListadoCotizaciones();

            MostrarListadoMonedas();
            vista.HabilitarCompra(false);
            vista.Inicializar();
        }

        /// <summary>
        /// Despliega el listado de Monedas en la Vista
        /// </summary>
        private void MostrarListadoMonedas()
        {
            try
            {
                List<MonedaBO> resultado = Facade.SDNI.BR.FacadeBR.ConsultarMoneda(dataContext, new MonedaBO { Activo = true }) ??
                                           new List<MonedaBO>();

                vista.ListadoMonedas = resultado;
            }
            catch (Exception ex)
            {
                vista.MostrarMensaje("Inconsistencias al presentar la información.", ETipoMensajeIU.ERROR, ex.Message);
            }
        }

        /// <summary>
        ///  Inicializa la Vista
        /// </summary>
        /// <param name="unidad">Unidad a Cargar en la Vista</param>
        /// <param name="PlazoAnios">Plazo del Contrato en Años</param>
        public void Inicializar(UnidadBO unidad, int? PlazoAnios)
        {
            Inicializar();
            vista.PlazoAnio = PlazoAnios;
            vista.OpcionCompra = false;           
            vista.UnidadOperativaID = unidad.Sucursal.UnidadOperativa.Id;
            vista.ImporteCompra = null;
            DatosAInterfazUsuario(unidad);
        }

        /// <summary>
        /// Despliega el Listado de Tipo de Costizaciones
        /// </summary>
        private void MostrarListadoCotizaciones()
        {
            var tipos = new List<ETipoCotizacion>(Enum.GetValues(typeof(ETipoCotizacion)).Cast<ETipoCotizacion>());

            vista.ListadoTiposCotizacion = tipos;
        }

        /// <summary>
        /// Despliega los datos de la unidad a la interfaz
        /// </summary>
        /// <param name="unidad"></param>
        private void DatosAInterfazUsuario(UnidadBO unidad)
        {
            UnidadBO consultada = ObtenerUnidad(unidad);
            if (consultada == null) throw new Exception("No se ha proporcionado una unidad a agregar");

            vista.UnidadID = consultada.UnidadID;

            vista.EquipoID = consultada.EquipoID;

            vista.VIN = !string.IsNullOrEmpty(consultada.NumeroSerie) ? consultada.NumeroSerie : string.Empty;

            vista.NumeroEconocimico = !string.IsNullOrEmpty(consultada.NumeroEconomico) ? consultada.NumeroEconomico : string.Empty;

            vista.Anio = consultada.Anio;

            if (consultada.Modelo == null) consultada.Modelo = new ModeloBO();
            vista.Modelo = consultada.Modelo.Nombre;

            if (consultada.CaracteristicasUnidad != null)
            {
                vista.PBC = consultada.CaracteristicasUnidad.PBCMaximoRecomendado;
                vista.PBV = consultada.CaracteristicasUnidad.PBVMaximoRecomendado;
            }

            if (consultada.Mediciones != null && consultada.Mediciones.Odometros != null)
            {
                int? primerOdometroID = consultada.Mediciones.Odometros.Min(o => o.OdometroID);
                
                if (primerOdometroID != null)
                {
                    OdometroBO odometro = consultada.Mediciones.Odometros.Find(odo => odo.OdometroID == primerOdometroID);

                    if (odometro != null) vista.KmInicial = odometro.KilometrajeInicio;
                }

            }

            vista.ListadoEquiposAliados = consultada.EquiposAliados;
        }

        /// <summary>
        /// Obtiene la Unidad con sus datos completos
        /// </summary>
        /// <param name="unidad"></param>
        /// <returns></returns>
        private UnidadBO ObtenerUnidad(UnidadBO unidad)
        {
            if (unidad != null && (unidad.UnidadID != null || unidad.EquipoID != null))
            {
                var unidadBR = new UnidadBR();

                List<UnidadBO> listado = unidadBR.ConsultarCompleto(dataContext, unidad, true);

                UnidadBO resultado = listado.Find(
                    unid => unidad.UnidadID == unid.UnidadID && unid.EquipoID == unidad.EquipoID);

                return resultado;
            }
            throw new Exception("Se requiere proporcionar una Unidad para desplegar su información.");
        }

        /// <summary>
        /// Validacion de los datos de la Nueva Linea de Contrato
        /// </summary>
        /// <returns></returns>
        public string ValidarDatos()
        {
            string mensaje = string.Empty;

            if (vista.KmEstimadoAnual == null) mensaje += " Kilometro Estimado Anual, ";
            if (vista.DepositoGarantia == null) mensaje += " Depósito Garantia, ";
            if (vista.ComisionApertura == null) mensaje += " Comisión por Apertura, ";
            if (vista.CargoFijoMes == null) mensaje += " Cargo Fijo por Mes, ";
            if(vista.TipoCotizacionSeleccionada == null) mensaje +=" Tipo de Cotización, ";
            if (vista.CobroKilometrosHoras == null) mensaje += " Cobro por Kilómetros u Horas, ";
            if (string.IsNullOrWhiteSpace(vista.ClaveProductoServicio)) mensaje += " Producto o Servicio, ";

            if (vista.OpcionCompra)
            {
                if (vista.MonedaSeleccionada == null) mensaje += " Moneda de Compra, ";
                if (vista.ImporteCompra == null) mensaje += " Importe de Compra, ";
            }

            if (vista.TipoCotizacionSeleccionada != null)
            {
                var unidadNoConfigurada = false;
                if (vista.TarifasAdicionales == null)
                    unidadNoConfigurada = true;
                else
                {
                    if (vista.TarifasAdicionales.Count() < 1)
                        unidadNoConfigurada = true;
                    else
                    {
                        if (vista.TarifasAdicionales.First() != null && vista.TarifasAdicionales.First().Rangos == null)
                            unidadNoConfigurada = true;
                        else
                        {
                            if (vista.TarifasAdicionales.First() != null && vista.TarifasAdicionales.First().Rangos.Count() < 1)
                                unidadNoConfigurada = true;
                        }
                    }
                }
                if(unidadNoConfigurada)
                    mensaje += " Configurar las tarifas de la Unidad, ";
                if (vista.ListadoEquiposAliados.Count > 0)
                {
                    if (vista.CargosAdicionalesEquiposAliados.Count != vista.ListadoEquiposAliados.Count)
                        mensaje += " Configurar las tarifas de todos los equipos aliados, ";
                }
            }

            if (!string.IsNullOrEmpty(mensaje))
                return "Los siguientes campos no pueden estar vacíos: \n" + mensaje.Substring(0, mensaje.Length - 2);

            return mensaje;
        }

        /// <summary>
        /// Obtiene la Linea de Contrato a partir de los datos de la Vista
        /// </summary>
        /// <returns></returns>
        public LineaContratoFSLBO InterfazUsuarioADatos()
        {
            var nuevaLinea = new LineaContratoFSLBO
            {
                Equipo = new UnidadBO
                {
                    UnidadID = vista.UnidadID,
                    EquipoID = vista.EquipoID,
                    NumeroSerie = !String.IsNullOrEmpty(vista.VIN)? vista.VIN : null,
                    NumeroEconomico = !String.IsNullOrEmpty(vista.NumeroEconocimico) ? vista.NumeroEconocimico: null,
                    Modelo = new ModeloBO{ Nombre = vista.Modelo },
                    Sucursal = new SucursalBO
                    {
                        UnidadOperativa = new UnidadOperativaBO
                        {
                            Id = vista.UnidadOperativaID,                            
                        }
                    }
                },
                CargoFijoMensual = vista.CargoFijoMes,
                ComisionApertura = vista.ComisionApertura,
                ConOpcionCompra = vista.OpcionCompra,
                DepositoGarantia = vista.DepositoGarantia,
                DivisaCompra = new DivisaBO { MonedaDestino = vista.MonedaSeleccionada },
                ImporteCompra = vista.ImporteCompra,
                KmEstimadoAnual = vista.KmEstimadoAnual,
                ProductoServicio = new ProductoServicioBO(){ 
                    Id = vista.ProductoServicioId, 
                    NombreCorto = vista.ClaveProductoServicio,
                    Nombre = vista.DescripcionProductoServicio
                },
                Cobrable = new CargosAdicionalesFSLBO
                {
                    TipoCotizacion = vista.TipoCotizacionSeleccionada,
                    CargoAdicionalEquiposAliados = vista.CargosAdicionalesEquiposAliados,
                    Tarifas = vista.TarifasAdicionales
                },
                Activo = true
            };

            return nuevaLinea;
        }

        /// <summary>
        /// Despliega la linea en la interfaz de usuario
        /// </summary>
        /// <param name="lineaContrato"></param>
        /// <param name="plazoAnio"></param>
        /// <param name="IncluyeSeguro"></param>
        public void DatosAInterfazUsuario(LineaContratoFSLBO lineaContrato, int? plazoAnio, ETipoInclusion? IncluyeSeguro)
        {
            DatosAInterfazUsuario((UnidadBO)lineaContrato.Equipo);
            vista.PlazoAnio = plazoAnio;
            vista.CargoFijoMes = lineaContrato.CargoFijoMensual;
            vista.ComisionApertura = lineaContrato.ComisionApertura;
            vista.DepositoGarantia = lineaContrato.DepositoGarantia;
            vista.OpcionCompra = lineaContrato.ConOpcionCompra == true;
            vista.KmEstimadoAnual = lineaContrato.KmEstimadoAnual;            
            vista.EstablecerMonedaCompra(lineaContrato.DivisaCompra.MonedaDestino);
            vista.ImporteCompra = lineaContrato.ImporteCompra;
            vista.ProductoServicioId = lineaContrato.ProductoServicio.Id;
            vista.ClaveProductoServicio = lineaContrato.ProductoServicio.NombreCorto;
            vista.DescripcionProductoServicio = lineaContrato.ProductoServicio.Nombre;

            if (lineaContrato.Equipo != null && lineaContrato.Equipo.Sucursal != null &&
                lineaContrato.Equipo.Sucursal.UnidadOperativa != null)
                vista.UnidadOperativaID = lineaContrato.Equipo.Sucursal.UnidadOperativa.Id;
            if (lineaContrato.Cobrable != null)
            {
                var cargosAdicionales = ((CargosAdicionalesFSLBO) lineaContrato.Cobrable);
                vista.EstablecerTipoCotizacion(((CargosAdicionalesFSLBO) lineaContrato.Cobrable).TipoCotizacion);
                vista.EstablecerKmsHrs(cargosAdicionales.Tarifas != null && cargosAdicionales.Tarifas.Any() ? cargosAdicionales.Tarifas.First().CobraKm.Value ? 0 : 1 : (-1));
                vista.EstablecerTarifas(cargosAdicionales.Tarifas, cargosAdicionales.Tarifas != null && cargosAdicionales.Tarifas.Count > 0 ? cargosAdicionales.Tarifas.First().CobraKm : null);
                vista.EstablecerCargosAdicionalesEquiposAliados(((CargosAdicionalesFSLBO)lineaContrato.Cobrable).CargoAdicionalEquiposAliados ?? new List<CargoAdicionalEquipoAliadoBO>());
            }

            if(IncluyeSeguro == ETipoInclusion.Incluido)
                vista.NumeroPoliza = CalcularNumeroPoliza();
        }

        /// <summary>
        /// Establece el Objecto de Linea de Contrato antes de editar
        /// </summary>
        /// <param name="lineaContrato"></param>
        public void EstablecerUltimoObjeto(LineaContratoFSLBO lineaContrato)
        {
            vista.UltimoObjeto = lineaContrato;
        }

        /// <summary>
        /// Calcula el Numero de la Poliza de la Unidad
        /// </summary>
        /// <returns></returns>
        private string CalcularNumeroPoliza()
        {
            var seguroBR = new SeguroBR();
            //SC_0051
            List<SeguroBO> seguros = seguroBR.Consultar(dataContext, new SeguroBO { Activo = true, Tramitable = new UnidadBO { UnidadID = vista.UnidadID } }) ?? new List<SeguroBO>();

            SeguroBO seguro =
                seguros.FindLast(
                    seg => seg.Tramitable.TramitableID == vista.UnidadID && seg.Tramitable.TipoTramitable == ETipoTramitable.Unidad);

            return seguro != null ? seguro.NumeroPoliza: string.Empty;
        }

        #region Buscador
        /// <summary>
        /// Prepara un BO para la Busqueda en su respectivo catalogo
        /// </summary>
        /// <param name="catalogo">catalogo donde se realizara la busqueda</param>
        /// <returns></returns>
        public object PrepararBOBuscador(string catalogo) {
            object obj = null;

            switch (catalogo) {
                case "ProductoServicio":
                    ProductoServicioBO producto = new ProductoServicioBO() { Activo = true };

                    if (!string.IsNullOrEmpty(vista.ClaveProductoServicio)) {
                        int auxNum = 0;
                        if (Int32.TryParse(vista.ClaveProductoServicio, out auxNum))
                            producto.NombreCorto = vista.ClaveProductoServicio;
                        else
                            producto.Nombre = vista.ClaveProductoServicio;
                    }

                    obj = producto;
                    break;
            }

            return obj;
        }

        /// <summary>
        /// Despliega el Resultado del Buscador
        /// </summary>
        /// <param name="catalogo">Catalogo en el que se realizo la busqueda</param>
        /// <param name="selecto">Objeto Resultante</param>
        public void DesplegarResultadoBuscador(string catalogo, object selecto) {
            switch (catalogo) {
                case "ProductoServicio":
                    ProductoServicioBO producto = (ProductoServicioBO)selecto ?? new ProductoServicioBO();
                    vista.ProductoServicioId = producto.Id;
                    vista.ClaveProductoServicio = producto.NombreCorto;
                    vista.DescripcionProductoServicio = producto.Nombre;
                    break;
            }
        } 
        #endregion
        #endregion
    }
}
