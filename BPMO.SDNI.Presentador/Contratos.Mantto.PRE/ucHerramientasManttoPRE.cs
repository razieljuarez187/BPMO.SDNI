//Satisface al caso de uso CU029 - Consultar Contratos de Mantenimiento
using System.Collections.Generic;

using BPMO.SDNI.Contratos.Mantto.BO;
using BPMO.SDNI.Contratos.Mantto.VIS;

namespace BPMO.SDNI.Contratos.Mantto.PRE
{
    public class ucHerramientasManttoPRE
    {
        #region Atributos

        internal readonly IucHerramientasManttoVIS vista;

        #endregion

        #region Constructores

        public ucHerramientasManttoPRE(IucHerramientasManttoVIS vistaActual)
        {
            if (vistaActual != null)
                vista = vistaActual;
        }

        #endregion

        #region Métodos

        /// <summary>
        /// Despliega los datos del contrato en la interfaz de usuario
        /// </summary>
        /// <param name="contrato">Contrato RD que contiene los datos a desplegar</param>
        public void DatosAInterfazUsuario(ContratoManttoBO contrato)
        {
            contrato = contrato ?? new ContratoManttoBO();

            vista.NumeroContrato = contrato.NumeroContrato != null && !string.IsNullOrEmpty(contrato.NumeroContrato)
                                       ? contrato.NumeroContrato
                                       : null;
            vista.EstatusContrato = contrato.Estatus;

            vista.Configurar(contrato.Estatus);
        }

        /// <summary>
        /// Inicializa el control
        /// </summary>
        public void Inicializar()
        {
            vista.EstatusContrato = null;
            vista.NumeroContrato = null;
        }

        /// <summary>
        /// Deshabilita el menú de impresión de acuerdo a los permisos del usuario
        /// </summary>
        public void OcultarMenuImpresion()
        {
            this.vista.DeshabilitarOpcionImpresion();
        }

        /// <summary>
        /// Deshabilita el menú de CerrarContrato de acuerdo a los permisos del usuario
        /// </summary>
        internal void DeshabilitarMenuCerrar()
        {
            this.vista.DeshabilitarOpcionCerrar();
        }

        /// <summary>
        /// Deshabilita el menú Editar de acuerdo a los permisos del usuario
        /// </summary>
        internal void DeshabilitarMenuEditar()
        {
            this.vista.DeshabilitarOpcionEditar();
        }

        /// <summary>
        /// Deshabilitar el menú Borrar de acuerdo a los permisos del usuario
        /// </summary>
        internal void DeshabilitarMenuBorrar()
        {
            this.vista.DeshabilitarOpcionBorrar();
        }

        /// <summary>
        /// Deshabilita el menú Imprimir de acuerdo a los permisos del usuario
        /// </summary>
        internal void DeshabilitarMenuImprimir()
        {
            this.vista.DeshabilitarMenuImpresion();
        }

        /// <summary>
        /// Deshabilita el menú Agregar documentos de acuerdo a los permisos del usuario
        /// </summary>
        internal void DeshabilitarMenuDocumentos()
        {
            this.vista.DeshabilitarOpcionesAgregarDoc();
        }

        /// <summary>
        /// Despliega en la vista las plantillas correspondientes al modulo
        /// </summary>
        /// <param name="resultado">Lista con las plantillas que se van a desplegar</param>
        public void CargarArchivos(List<object> resultado)
        {
            this.vista.CargarArchivos(resultado);
        }
        #endregion
    }
}
