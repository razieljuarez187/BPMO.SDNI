using BPMO.SDNI.Contratos.PSL.BO;
using BPMO.SDNI.Contratos.PSL.VIS;

namespace BPMO.SDNI.Contratos.PSL.PRE {
    public class ucHerramientasPSLPRE {
        #region Atributos

        internal readonly IucHerramientasPSLVIS vista;

        #endregion

        #region Constructores

        public ucHerramientasPSLPRE(IucHerramientasPSLVIS vistaActual) {
            if (vistaActual != null) {
                vista = vistaActual;
                if (vista.EstatusContrato != null)
                    vista.EstatusContrato = vista.EstatusContrato;
            }
        }

        #endregion

        #region Métodos

        /// <summary>
        /// Despliega los datos del contrato en la interfaz de usuario
        /// </summary>
        /// <param name="contrato">Contrato PSL que contiene los datos a desplegar</param>
        public void DatosAInterfazUsuario(ContratoPSLBO contrato) {
            contrato = contrato ?? new ContratoPSLBO();

            vista.NumeroContrato = contrato.NumeroContrato != null && !string.IsNullOrEmpty(contrato.NumeroContrato)
                                       ? contrato.NumeroContrato
                                       : null;
            vista.EstatusContrato = contrato.Estatus;
            vista.Configurar(contrato.Estatus);
        }

        /// <summary>
        /// Inicializa el control
        /// </summary>
        public void Inicializar() {
            vista.EstatusContrato = null;
            vista.NumeroContrato = null;
        }

        /// <summary>
        /// Deshabilita el menú de impresión de acuerdo a los permisos del usuario
        /// </summary>
        public void OcultarMenuImpresion() {
            this.vista.DeshabilitarOpcionImpresion();
        }

        /// <summary>
        /// Deshabilita el menú de CerrarContrato de acuerdo a los permisos del usuario
        /// </summary>
        internal void DeshabilitarMenuCerrar() {
            //this.vista.DeshabilitarOpcionCerrar();
        }

        /// <summary>
        /// Deshabilita el menú Editar de acuerdo a los permisos del usuario
        /// </summary>
        internal void DeshabilitarMenuEditar() {
            this.vista.DeshabilitarOpcionEditar();
        }

        /// <summary>
        /// Deshabilitar el menú Borrar de acuerdo a los permisos del usuario
        /// </summary>
        internal void DeshabilitarMenuBorrar() {
            this.vista.DeshabilitarOpcionBorrar();
        }

        /// <summary>
        /// Deshabilita el menú Imprimir de acuerdo a los permisos del usuario
        /// </summary>
        internal void DeshabilitarMenuImprimir() {
            this.vista.DeshabilitarMenuImpresion();
        }

        /// <summary>
        /// Deshabilita el menú Agregar documentos de acuerdo a los permisos del usuario
        /// </summary>
        internal void DeshabilitarMenuDocumentos() {
            this.vista.DeshabilitarOpcionesAgregarDoc();
        }

        /// <summary>
        /// Oculta el botón solicitud de pago
        /// </summary>
        public void OcultarSolicitudPago() {
            this.vista.OcultarSolicitudPago();
        }

        #endregion
    }
}