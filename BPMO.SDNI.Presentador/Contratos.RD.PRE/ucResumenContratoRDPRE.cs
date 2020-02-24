//Satisface al caso de uso CU003 - Consultar Contratos Renta Diaria
//Satisface a la solicitud de cambio SC0035
using System;
using System.Collections.Generic;
using BPMO.Basicos.BO;
using BPMO.Patterns.Creational.DataContext;
using BPMO.Primitivos.Enumeradores;
using BPMO.SDNI.Comun.BO;
using BPMO.SDNI.Contratos.RD.BO;
using BPMO.SDNI.Contratos.RD.VIS;

namespace BPMO.SDNI.Contratos.RD.PRE
{
    public class ucResumenContratoRDPRE
    {

        #region Atributos

        /// <summary>
        /// Vista sobre la que actua el presentador
        /// </summary>
        private readonly IucResumenContratoRDVIS vista;

        /// <summary>
        /// El DataContext que proveerá acceso a la base de datos
        /// </summary>
        private readonly IDataContext dataContext;

        /// <summary>
        /// Nombre de la clase para agregar a los mensajes de Error
        /// </summary>
        private const string NombreClase = "ucInformacionContratoRDPRE";

        #endregion

        #region Propiedades

        /// <summary>
        /// Vista sobre la que actua el Presentador de solo lectura
        /// </summary>
        internal IucResumenContratoRDVIS Vista { get { return vista; } }

        #endregion

        #region Constructores

        /// <summary>
        /// Constructor que recibe la vista
        /// </summary>
        /// <param name="vistaActual">Vista sobre la cual actuará el presentador</param>
        public ucResumenContratoRDPRE(IucResumenContratoRDVIS vistaActual)
        {
            if (vistaActual != null)
                vista = vistaActual;

            dataContext = Facade.SDNI.BR.FacadeBR.ObtenerConexion();
        }

        #endregion

        #region Métodos

        /// <summary>
        /// Despliega la informacion del contrato en la Vista
        /// </summary>
        /// <param name="contrato"></param>
        public void DatosAInterfazUsuario(ContratoRDBO contrato)
        {
            if (contrato == null) contrato = new ContratoRDBO();
            if (contrato.Sucursal == null) contrato.Sucursal = new SucursalBO();
            if (contrato.Cliente == null) contrato.Cliente = new CuentaClienteIdealeaseBO();
            if (contrato.Cliente.Cliente == null) contrato.Cliente.Cliente = new ClienteBO();

            vista.FechaContrato = contrato.FechaContrato;
            vista.FechaCierreContrato = contrato.CierreContrato != null ? contrato.CierreContrato.Fecha : null;//SC0035
            
            if (contrato.Sucursal.Nombre != null) vista.NombreSucursal = contrato.Sucursal.Nombre;

            if (contrato.Cliente.Direcciones.Count > 0)
            {
                foreach (DireccionClienteBO direccion in contrato.Cliente.Direcciones)
                {
                    if (direccion.Primaria == true)
                    {
                        vista.DireccionCliente = direccion.Direccion;
                        break;
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
        public void Inicializar()
        {
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
        /// Despiega el nombre de la Empresa
        /// </summary>
        private void DesplegarEmpresa()
        {
            try
            {
                // Obtener la Unidad Operativa Completa
                List<UnidadOperativaBO> unidadesOperativas = Facade.SDNI.BR.FacadeBR.ConsultarUnidadOperativaCompleto(dataContext, vista.UnidadOperativa);

                UnidadOperativaBO unidadOperativaBO = unidadesOperativas.Find(
                    unidOperativa => vista.UnidadOperativa.Id == unidOperativa.Id);

                if (unidadOperativaBO != null)
                {
                    if (unidadOperativaBO.Empresa != null)
                    {
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

        #endregion
    }
}
