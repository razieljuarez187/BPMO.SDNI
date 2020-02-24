using System;
using System.Linq;
using BPMO.Basicos.BO;
using BPMO.Facade.SDNI.BR;
using BPMO.Patterns.Creational.DataContext;
using BPMO.Primitivos.Enumeradores;
using BPMO.SDNI.Comun.BO;
using BPMO.SDNI.Contratos.PSL.BO;
using BPMO.SDNI.Contratos.PSL.VIS;

namespace BPMO.SDNI.Contratos.PSL.PRE {
    public class ucResumenContratoPSLPRE {

        #region Atributos

        /// <summary>
        /// Vista sobre la que actual el presentador
        /// </summary>
        private readonly IucResumenContratoPSLVIS vista;

        /// <summary>
        /// El DataContext que proveerá acceso a la base de datos
        /// </summary>
        private readonly IDataContext dataContext;

        /// <summary>
        /// Nombre de la clase para agregar a los mensajes de Error
        /// </summary>
        private const string NombreClase = "ucInformacionContratoPSLPRE";

        #endregion

        #region Propiedades

        /// <summary>
        /// Vista sobre la que actua el Presentador de solo lectura
        /// </summary>
        internal IucResumenContratoPSLVIS Vista { get { return vista; } }

        #endregion

        #region Constructores

        /// <summary>
        /// Constructor que recibe la vista
        /// </summary>
        /// <param name="vistaActual">Vista sobre la cual actuará el presentador</param>
        public ucResumenContratoPSLPRE(IucResumenContratoPSLVIS vistaActual) {
            if (vistaActual != null)
                vista = vistaActual;

            dataContext = Facade.SDNI.BR.FacadeBR.ObtenerConexion();
        }

        #endregion

        #region Métodos

        /// <summary>
        /// Despliega la información del contrato en la Vista
        /// </summary>
        /// <param name="contrato"></param>
        public void DatosAInterfazUsuario(ContratoPSLBO contrato) {
            if (contrato == null) contrato = new ContratoPSLBO();
            if (contrato.Sucursal == null) contrato.Sucursal = new SucursalBO();
            if (contrato.Cliente == null) contrato.Cliente = new CuentaClienteIdealeaseBO();
            if (contrato.Cliente.Cliente == null) contrato.Cliente.Cliente = new ClienteBO();

            vista.FechaContrato = contrato.FechaContrato;
            vista.FechaCierreContrato = contrato.CierreContrato != null ? contrato.CierreContrato.Fecha : null;//SC0035

            if (contrato.Sucursal.Nombre != null) vista.NombreSucursal = contrato.Sucursal.Nombre;

            if (contrato.Cliente.Direcciones != null) {
                if (contrato.Cliente.Direcciones.Count > 0) {
                    foreach (DireccionClienteBO direccion in contrato.Cliente.Direcciones) {
                        if (direccion.Primaria == true) {
                            vista.DireccionCliente = direccion.Direccion;
                            break;
                        }
                    }
                }
            }

            if (contrato.Cliente.Nombre != null)
                vista.NombreCuentaCliente = contrato.Cliente.Nombre;

            if (contrato.Cliente.Cliente.RFC != null)
                vista.RFCCliente = contrato.Cliente.Cliente.RFC;

            vista.NumeroCuentaCliente = String.IsNullOrEmpty(contrato.Cliente.Numero) ? null : contrato.Cliente.Numero;

            if (contrato.FrecuenciaFacturacion != null) { //SC0035
                vista.FrecuenciaFacturacion = contrato.FrecuenciaFacturacion;
            }

            DesplegarEmpresa();
        }

        /// <summary>
        /// Inicializa la vista
        /// </summary>
        public void Inicializar() {
            vista.NombreSucursal = null;
            vista.NombreEmpresa = null;
            vista.FechaContrato = null;
            vista.FechaCierreContrato = null;
            vista.NombreCuentaCliente = null;
            vista.RFCCliente = null;
            vista.DireccionCliente = null;
            vista.NumeroCuentaCliente = null;
            vista.FrecuenciaFacturacion = null;

            DesplegarEmpresa();
        }

        /// <summary>
        /// Despliega el nombre de la Empresa
        /// </summary>
        private void DesplegarEmpresa() {
            try {
                string nombreEmpresa = string.Empty;
                if (this.vista.UnidadOperativa == null || this.vista.UnidadOperativa.Empresa == null || string.IsNullOrWhiteSpace(this.vista.UnidadOperativa.Empresa.Nombre)) {
                    //Obtener información de la Unidad Operativa
                    UnidadOperativaBO UO = FacadeBR.ConsultarUnidadOperativaCompleto(this.dataContext, new UnidadOperativaBO() { Id = this.vista.UnidadOperativa.Id }).FirstOrDefault();
                    if (UO == null || UO.Empresa == null)
                        throw new Exception("No se encontró la información completa de la unidad operativa sobre la que trabaja.");

                    nombreEmpresa = UO.Empresa.Nombre;
                } else {
                    nombreEmpresa = this.vista.UnidadOperativa.Empresa.Nombre;
                }
                this.vista.NombreEmpresa = this.vista.UnidadOperativa.Empresa.Nombre;
            } catch (Exception ex) {
                vista.MostrarMensaje("Inconsistencias al presentar la información de la empresa.", ETipoMensajeIU.ERROR, NombreClase + ".DesplegarEmpresa: " + ex.Message);
            }
        }

        #endregion
    }
}