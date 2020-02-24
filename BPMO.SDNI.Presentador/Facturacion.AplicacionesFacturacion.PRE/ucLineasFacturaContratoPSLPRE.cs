using System;
using BPMO.Basicos.BO;
using BPMO.Primitivos.Enumeradores;
using BPMO.SDNI.Facturacion.AplicacionesFacturacion.VIS;

namespace BPMO.SDNI.Facturacion.AplicacionesFacturacion.PRE {
    /// <summary>
    /// Presentador para la vista que visualiza las líneas de concepto de una factura
    /// </summary>
    public class ucLineasFacturaContratoPSLPRE {
        #region Atributos
        /// <summary>
        /// La vista que esta mostrando los datos procesados en el presentador
        /// </summary>
        private IucLineasFacturaContratoPSLVIS vista;

        /// <summary>
        /// Nombre de la clase para agregar a los mensajes de Error
        /// </summary>
        private string nombreClase = "ucLineasFacturaContratoPSLPRE";

        #endregion

        #region Constructores
        /// <summary>
        /// Constructor por default
        /// </summary>
        /// <param name="view">Vista a la que se asociada el presentador</param>
        public ucLineasFacturaContratoPSLPRE(IucLineasFacturaContratoPSLVIS view) {
            try {
                this.vista = view;
            } catch (Exception ex) {
                this.vista.MostrarMensaje("Inconsistencia en los parámetros de configuración", ETipoMensajeIU.ERROR, this.nombreClase + ".ucContratoManttoPRE:" + ex.Message);
            }
        }
        #endregion

        #region Métodos

        #region Métodos Buscador
        public object PrepararBOBuscador(string catalogo) {
            object obj = null;

            switch (catalogo) {
                case "UsoCFDI":
                    UsoCFDIBO uso = new UsoCFDIBO() { Activo = true };
                    if (!string.IsNullOrEmpty(vista.ClaveUsoCFDI)) {
                        if (vista.ClaveUsoCFDI.Length <= 3)
                            uso.NombreCorto = vista.ClaveUsoCFDI;
                        else
                            uso.Nombre = vista.ClaveUsoCFDI;
                    }

                    obj = uso;
                    break;
            }

            return obj;
        }
        public void DesplegarResultadoBuscador(string catalogo, object selecto) {
            switch (catalogo) {
                case "UsoCFDI":
                    UsoCFDIBO uso = (UsoCFDIBO)selecto ?? new UsoCFDIBO();
                    vista.UsoCFDIId = uso.Id;
                    vista.ClaveUsoCFDI = uso.NombreCorto;
                    vista.DescripcionUsoCFDI = uso.Nombre;
                    break;
            }
        }
        #endregion
        #endregion
    }
}