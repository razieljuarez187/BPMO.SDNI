// Satisface al caso de uso CU022 - Consultar Contratos Full Service Leasing
// Satisface al caso de uso CU023 - Editar Contrato Full Service Leasing
//Satisface al caso de uso CU026 - Registrar Terminación de Contrato Full Service Leasing
using System;
using System.Collections.Generic;
using BPMO.Basicos.BO;
using BPMO.Facade.SDNI.BR;
using BPMO.Patterns.Creational.DataContext;
using BPMO.Primitivos.Enumeradores;
using BPMO.SDNI.Comun.BO;
using BPMO.SDNI.Comun.BR;
using BPMO.SDNI.Contratos.FSL.BO;
using BPMO.SDNI.Contratos.FSL.VIS;

namespace BPMO.SDNI.Contratos.FSL.PRE
{
    public class ucInformacionGeneralPRE
    {
        #region Atributos
        /// <summary>
        /// Vista sobre la que actua el presentador
        /// </summary>
        private readonly IucInformacionGeneralVIS vista;        
        /// <summary>
        /// El DataContext que proveerá acceso a la base de datos
        /// </summary>
        private readonly IDataContext dataContext;
        /// <summary>
        /// Nombre de la clase para agregar a los mensajes de Error
        /// </summary>
        private const string NombreClase = "ucInformacionGeneralPRE";
        #endregion

        #region Propiedades

        /// <summary>
        /// Vista sobre la que actua el Presentador de solo lectura
        /// </summary>
        internal IucInformacionGeneralVIS Vista { get { return vista; } }

        #endregion

        #region Constructores
        /// <summary>
        /// Constructor que recibe la vista
        /// </summary>
        /// <param name="vistaActual">Vista sobre la cual actuará el presentador</param>
        public ucInformacionGeneralPRE(IucInformacionGeneralVIS vistaActual)
        {
            if (vistaActual != null)
                vista = vistaActual;

            dataContext = Facade.SDNI.BR.FacadeBR.ObtenerConexion();
        }
        #endregion

        #region Metodos
        /// <summary>
        /// Despliega la informacion del contrato en la Vista
        /// </summary>
        /// <param name="contrato"></param>
        public void DatosAInterfazUsuario(ContratoFSLBO contrato)
        {
            if(contrato == null) contrato = new ContratoFSLBO();
            if (contrato.Sucursal == null) contrato.Sucursal = new SucursalBO();
            if (contrato.Divisa == null) contrato.Divisa = new DivisaBO();
            if (contrato.Divisa.MonedaDestino == null) contrato.Divisa.MonedaDestino = new MonedaBO(); 

            vista.FechaContrato = contrato.FechaContrato;          

            if(contrato.Sucursal.Id != null) vista.EstablecerSucursalSeleccionada(contrato.Sucursal.Id);

            if(contrato.Divisa.MonedaDestino.Codigo !=null) vista.EstablecerMonedaSeleccionada(contrato.Divisa.MonedaDestino.Codigo);

            vista.Representante = contrato.Representante;
	        vista.PorcentajePenalizacion = contrato.PorcentajePenalizacion;
        }

        /// <summary>
        /// Inicializa la vista
        /// </summary>
        public void Inicializar()
        {
            DesplegarSucursales();            
            DesplegarMonedas();
            

            vista.DomicilioEmpresa = null;
            vista.EmpresaID = null;
            vista.FechaContrato = null;
            vista.NombreEmpresa = null;
            vista.Representante = null;
            vista.PorcentajePenalizacion = null;
            
            DesplegarEmpresa();
            DesplegarDireccionEmpresa();
        }

        /// <summary>
        /// Despliega el listado de Sucursales en la Vista
        /// </summary>
        private void DesplegarSucursales()
        {
            try
            {
                var seguridad = new SeguridadBO(Guid.Empty, vista.Usuario, new AdscripcionBO { UnidadOperativa = vista.UnidadOperativa });
                List<SucursalBO> resultado = Facade.SDNI.BR.FacadeBR.ConsultarSucursalesSeguridad(dataContext, seguridad) ??
                                              new List<SucursalBO>();

                vista.ListadoSucursales = resultado;
            }
            catch (Exception ex)
            {
                vista.MostrarMensaje("Inconsistencias al presentar el Listado de Sucursales.", ETipoMensajeIU.ERROR, NombreClase + ".DesplegarSucursales: " + ex.Message);
            }
        }

        /// <summary>
        /// Despliega el listado de Monedas en la Vista
        /// </summary>
        private void DesplegarMonedas()
        {
            try
            {
                List<MonedaBO> resultado = Facade.SDNI.BR.FacadeBR.ConsultarMoneda(dataContext, new MonedaBO { Activo = true }) ??
                                           new List<MonedaBO>();

                vista.ListadoMonedas = resultado;
            }
            catch (Exception ex)
            {
                vista.MostrarMensaje("Inconsistencias al presentar la información.", ETipoMensajeIU.ERROR, NombreClase + ".DesplegarMonedas: " + ex.Message);
            }
        }

        /// <summary>
        /// Calcula y Despliega la direccion de la sucursal
        /// </summary>
        public void DesplegarDireccionEmpresa()
        {
            try
            {
                var sucursal = new SucursalBO {Matriz = true, UnidadOperativa = vista.UnidadOperativa};

                
                List<SucursalBO> lstCatalogo = Facade.SDNI.BR.FacadeBR.ConsultarSucursal(dataContext, sucursal);
                if (lstCatalogo.Count > 0)
                {
                    sucursal = lstCatalogo.Find(suc => suc.Matriz == true);
                    var dirSucursal = new DireccionSucursalBO {Primaria = true};

                    sucursal.Agregar(dirSucursal);
                    
                    lstCatalogo = Facade.SDNI.BR.FacadeBR.ConsultarSucursalCompleto(dataContext, sucursal);

                    if (lstCatalogo.Count > 0 &&
                        (lstCatalogo[0].DireccionesSucursal != null && lstCatalogo[0].DireccionesSucursal.Count > 0))
                    {
                        vista.DomicilioEmpresa = lstCatalogo[0].DireccionesSucursal[0].Calle;
                        return;
                    }
                }

                vista.DomicilioEmpresa = string.Empty;
            }
            catch (Exception ex)
            {
                vista.MostrarMensaje("Inconsistencias al presentar la información.", ETipoMensajeIU.ERROR, NombreClase + ".DesplegarDireccionSucursal: " + ex.Message);
            }
        }

        /// <summary>
        /// Despiega el nombre de la Empresa
        /// </summary>
        private void DesplegarEmpresa()
        {
            try
            {
                // Obtener la Unidad Operativa Completa
                List<UnidadOperativaBO> unidadesOperativas = Facade.SDNI.BR.FacadeBR.ConsultarUnidadOperativa(dataContext, vista.UnidadOperativa);//SC_0051
                
                UnidadOperativaBO unidadOperativaBO = unidadesOperativas.Find(
                    unidOperativa => vista.UnidadOperativa.Id == unidOperativa.Id);

                if (unidadOperativaBO != null)
                {
                    if (unidadOperativaBO.Empresa != null)
                    {
                        //Consultamos el nombre de la empresa
                        var resultEmps = FacadeBR.ConsultarEmpresa(this.dataContext, unidadOperativaBO.Empresa);
                        if (resultEmps.Count > 0)
                            unidadOperativaBO.Empresa = resultEmps[0];

                        if (!string.IsNullOrEmpty(unidadOperativaBO.Empresa.Nombre)) vista.NombreEmpresa = unidadOperativaBO.Empresa.Nombre;
                        if (unidadOperativaBO.Empresa.Id != null) vista.EmpresaID = unidadOperativaBO.Empresa.Id;
                        if (unidadOperativaBO.Empresa.Nombre != null &&
                            !string.IsNullOrEmpty(unidadOperativaBO.Empresa.Nombre.Trim()))
                            vista.NombreEmpresa = unidadOperativaBO.Empresa.Nombre;
                    }
                }
            }
            catch (Exception ex)
            {
                vista.MostrarMensaje("Inconsistencias al presentar la información de la empresa.", ETipoMensajeIU.ERROR, NombreClase + ".DesplegarEmpresa: " + ex.Message);
            }
        }

        /// <summary>
        /// Calcula y despliega las configuraciones de la unidad operativa en la vista
        /// </summary>
        public void DesplegarConfiguracionUnidadOperativa()
        {
            try
            {
                var moduloBR = new ModuloBR();

                var configuracion = new ConfiguracionUnidadOperativaBO
                {
                    UnidadOperativa = vista.UnidadOperativa
                };

                int? ModuloID = vista.ModuloID;

                if (vista.ModuloID != null)
                {
                    List<ConfiguracionUnidadOperativaBO> configuraciones =
                        moduloBR.ConsultarConfiguracionUnidadOperativa(dataContext, configuracion, ModuloID);

                    if (configuraciones != null && configuraciones.Count > 0)
                    {
                        configuracion =
                            configuraciones.Find(conf => configuracion.UnidadOperativa.Id == vista.UnidadOperativa.Id);
                        if (configuracion != null)
                        {
                            if (!string.IsNullOrEmpty(configuracion.Representante))
                            {
                                vista.Representante = configuracion.Representante.Trim();
                            }
                            else
                            {
                                vista.MostrarMensaje("No se ha configurado el representante de la unidad operativa.", ETipoMensajeIU.ADVERTENCIA);
                                vista.Representante = string.Empty;
                            }

                            if (configuracion.PorcentajePenalizacion != null)
                            {
                                vista.PorcentajePenalizacion = configuracion.PorcentajePenalizacion;
                            }
                            else
                            {
                                vista.MostrarMensaje("No se ha configurado el porcentaje de penalización en la unidad operativa.", ETipoMensajeIU.ADVERTENCIA);
                                vista.PorcentajePenalizacion = null;
                            }
                        }
                        else
                        {
                            vista.MostrarMensaje("No se ha configurado la unidad operativa.", ETipoMensajeIU.ADVERTENCIA);
                            vista.Representante = string.Empty;
                        }
                    }
                    else
                    {
                        vista.MostrarMensaje("No se ha configurado la unidad operativa.", ETipoMensajeIU.ADVERTENCIA);
                        vista.Representante = string.Empty;
                    }
                }
                else
                {
                    vista.MostrarMensaje("No se ha configurado el identificador del Modulo.", ETipoMensajeIU.ADVERTENCIA);
                    vista.Representante = string.Empty;
                }
            }
            catch (Exception ex)
            {
                vista.Representante = string.Empty;
                throw new Exception(NombreClase + ".MostrarRepresentante: " + ex.Message);
            }
        }
        #endregion Metodos
    }
}
