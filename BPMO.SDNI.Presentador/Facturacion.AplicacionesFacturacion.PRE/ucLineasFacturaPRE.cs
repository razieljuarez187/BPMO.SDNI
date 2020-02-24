//Satisface al Caso de uso CU005 - Armar Paquetes Facturacion

using System;
using BPMO.Primitivos.Enumeradores;
using BPMO.SDNI.Facturacion.AplicacionesFacturacion.VIS;
using BPMO.Basicos.BO;

namespace BPMO.SDNI.Facturacion.AplicacionesFacturacion.PRE
{
    /// <summary>
    /// Presentador para la vista que visualiza las líneas de concepto de una factura
    /// </summary>
    public class ucLineasFacturaPRE
    {
        #region Atributos
        /// <summary>
        /// La vista que esta mostrando los datos procesados en el presentador
        /// </summary>
        private IucLineasFacturaVIS vista;

        /// <summary>
        /// Nombre de la clase para agregar a los mensajes de Error
        /// </summary>
        private string nombreClase = "ucLineasFacturaPRE";

        #endregion

        #region Constructores
        /// <summary>
        /// Constructor por default
        /// </summary>
        /// <param name="view">Vista a la que se asociada el presentador</param>
        public ucLineasFacturaPRE(IucLineasFacturaVIS view)
        {
            try
            {
                this.vista = view;                
            }
            catch (Exception ex)
            {
                this.vista.MostrarMensaje("Inconsistencia en los parámetros de configuración", ETipoMensajeIU.ERROR, this.nombreClase + ".ucContratoManttoPRE:" + ex.Message);
            }
        }
        #endregion        

        #region Métodos
        /// <summary>
        /// Proceso que se ejecuta cuando se crea una nueva factura
        /// </summary>
        public void PrepararNuevo()
        {
        }

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
