// Satisface al caso de uso CU022 - Consultar Contratos Full Service Leasing
// Satisface al Caso de Uso CU023 - Editar Contrato Full Service Leasing
using BPMO.SDNI.Contratos.FSL.BO;
using BPMO.SDNI.Contratos.FSL.VIS;

namespace BPMO.SDNI.Contratos.FSL.PRE
{
    public class ucInformacionPagoPRE
    {
        #region Atributos
        
        /// <summary>
        /// Vista sobre la que actua el presentador
        /// </summary>
        private readonly IucInformacionPagoVIS vista;

        #endregion

        #region Propiedades

        /// <summary>
        /// Vista sobre la que actua el Presentador de solo lectura
        /// </summary>
        internal IucInformacionPagoVIS Vista { get { return vista; } }
        #endregion

        #region Constructores

        /// <summary>
        /// Constructor que recibe la vista sobre la que actuara el presentador
        /// </summary>
        /// <param name="vistaActual">vista sobre la que actuara el presentador</param>
        public ucInformacionPagoPRE(IucInformacionPagoVIS vistaActual)
        {
            if (vistaActual != null)
                vista = vistaActual;
        }

        #endregion

        #region Metodos

        /// <summary>
        /// Despliega los datos en la interfaz de usuario
        /// </summary>
        /// <param name="contrato">contrato que contiene la informacion a desplegar</param>
        public void DatosAInterfazUsuario(ContratoFSLBO contrato)
        {
            if(contrato == null) contrato = new ContratoFSLBO();
            #region Cuenta Bancaria
            //if(contrato.Banco == null) contrato.Banco = new Banco();

            //vista.Banco = contrato.Banco.Nombre != null && !string.IsNullOrEmpty(contrato.Banco.Nombre)
            //                           ? contrato.Banco.Nombre
            //                           : null;
            //vista.Beneficiario = contrato.Banco.Beneficiario != null && !string.IsNullOrEmpty(contrato.Banco.Beneficiario)
            //                           ? contrato.Banco.Beneficiario
            //                           : null;
            //vista.EstablecerCuentaBancariaSeleccionada(contrato.Banco.Id);
            #endregion
            vista.FechaInicioContrato = contrato.FechaInicioContrato;

            CalcularFechaTerminacionContrato(contrato.Plazo);
            #region Cuenta Bancaria
            //vista.Lugar = contrato.Banco.Ciudad != null && !string.IsNullOrEmpty(contrato.Banco.Ciudad)
            //                  ? contrato.Banco.Ciudad
            //                  : null;
            #endregion
            vista.Mensualidad = contrato.CalcularMensualidad();

            vista.Total = contrato.CalcularTotalAPagar();

            vista.DiasPago = contrato.DiasPago;
        }

        /// <summary>
        /// Inicializa la vista
        /// </summary>
        public void Inicializar()
        {
            #region Cuenta Bancaria
            //DesplegarListadoCuentasBancarias();
            
            //vista.Banco = null;
           
            //vista.Beneficiario = null;

            //vista.EstablecerCuentaBancariaSeleccionada(null);

            //vista.Lugar = null;
            #endregion

            vista.FechaInicioContrato = null;

            vista.FechaTerminacionContrato = null;

            vista.Mensualidad = null;

            vista.Total = null;

            vista.DiasPago = null;
        }

        /// <summary>
        /// Calcula la fecha de termincaion de contrato
        /// </summary>
        /// <param name="PlazoMeses"></param>
        private void CalcularFechaTerminacionContrato(int? PlazoMeses)
        {
            if (vista.FechaInicioContrato != null && PlazoMeses != null)
                vista.FechaTerminacionContrato = vista.FechaInicioContrato.Value.AddMonths(PlazoMeses.Value);
            else
                vista.FechaTerminacionContrato = null;
        }
        #region Cuenta Bancaria
        /*
        /// <summary>
        /// Calcula y Despliega en la Vista el Listado de Cuentas Bancarias
        /// </summary>
        private void DesplegarListadoCuentasBancarias()
        {
            List<Banco> bancos = new List<Banco> { new Banco() };
            vista.ListadoCuentasBancarias = bancos;
        }*/
        #endregion
        #endregion
    }
}
