// Satisface al caso de uso CU022 - Consultar Contratos Full Service Leasing
// Satisface al caso de uso CU026 - Registrar Terminación de Contrato Full Service Leasing
using System.Collections.Generic;
using BPMO.SDNI.Contratos.FSL.BO;
using BPMO.SDNI.Contratos.FSL.VIS;

namespace BPMO.SDNI.Contratos.FSL.PRE
{
    public class ucHerramientasFSLPRE
    {
        #region Atributos
        internal readonly IucHerramientasFSLVIS vista;
        #endregion

        #region Constructores
        public ucHerramientasFSLPRE(IucHerramientasFSLVIS vistaActual)
        {
            if (vistaActual != null)
                vista = vistaActual;
        }
        #endregion

        #region Metodos
        /// <summary>
        /// Despliega los datos del contrato en la interfaz de usuario
        /// </summary>
        /// <param name="contrato">Contrato FSL que contiene los datos a desplegar</param>
        public void DatosAInterfazUsuario(ContratoFSLBO contrato)
        {
            contrato = contrato ?? new ContratoFSLBO();

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
        #region sc_0008
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
        /// Deshabilita el menú Modificar Unidades Contrato de acuerdo a los permisos del usuario
        /// </summary>
        internal void DeshabilitarMenuModificarUnidadesContrato()
        {
            vista.DeshabilitarOpcionesModificarUnidadesContrato();
        }
        #endregion

        #region CU026
        /// <summary>
        /// Inicializa la barra de herramientas para el modo cerrar contrato
        /// </summary>
        public void InicializarModoCerrar()
        {
            Inicializar();
            vista.OcultarCerrarContrato();
            vista.OcultarEditarContrato();
            vista.OcultarModificarUnidadesContrato();
            vista.OcultarEliminarContrato();
            vista.OcultarFormatosContrato();
            vista.OcultarMenuImpresion();
        }
        #endregion

        #region SC0038
        /// <summary>
        /// Despliega en la vista los archivoscorrespondientes al modulo
        /// </summary>
        /// <param name="resultado">Lista con los archivos que serán cargados en la vista</param>
        public void CargarArchivos(List<object> resultado)
        {
            this.vista.CargarArchivos(resultado);
        }
        #endregion
        #endregion
    }
}
