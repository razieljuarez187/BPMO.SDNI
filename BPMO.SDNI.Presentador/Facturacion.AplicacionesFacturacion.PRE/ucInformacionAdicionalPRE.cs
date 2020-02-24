//Satisface al Caso de uso CU005 - Armar Paquetes Facturacion

using System;
using BPMO.eFacturacion.Procesos.BO;
using BPMO.Primitivos.Enumeradores;
using BPMO.SDNI.Facturacion.AplicacionesFacturacion.VIS;

namespace BPMO.SDNI.Facturacion.AplicacionesFacturacion.PRE
{
    /// <summary>
    /// Presentador para la vista que visualiza la información adicional de una factura
    /// </summary>
    public class ucInformacionAdicionalPRE
    {
        #region Abributos
        /// <summary>
        /// La vista que esta mostrando los datos procesados en el presentador
        /// </summary>
        private IucInformacionAdicionalVIS vista;

        /// <summary>
        /// Nombre de la clase para agregar a los mensajes de Error
        /// </summary>
        private string nombreClase = "ucInformacionAdicionalPRE";
        #endregion

        #region Constructores
        /// <summary>
        /// Constructor por default
        /// </summary>
        /// <param name="view">Vista a la que se asociada el presentador</param>
        public ucInformacionAdicionalPRE(IucInformacionAdicionalVIS view)
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

        /// <summary>
        /// Agrega un elemento de información adicional a la factura
        /// </summary>
        public void AgregarInformacionAdicional()
        {
            DatosAdicionalesFacturaBO adicional = new DatosAdicionalesFacturaBO();
            adicional.Etiqueta = this.vista.Etiqueta != null ? this.vista.Etiqueta.ToUpper() : null;
            adicional.Valor = this.vista.Valor != null ? this.vista.Valor.ToUpper() : null;

            this.vista.DatosAdicionales.Add(adicional);
            this.vista.MostrarListaDatosAdicionales();

            this.Limpiar();
        }

        /// <summary>
        /// Limpia los datos de la información adicional capturada
        /// </summary>
        private void Limpiar()
        {
            this.vista.Etiqueta = null;
            this.vista.Valor = null;
        }

        /// <summary>
        /// Elimina un elemento de información adicional a la factura
        /// </summary>
        /// <param name="id">Id del elemento a eliminar</param>
        public void EliminarInformacionAdicional(int id)
        {
            if (id >= 0 && id < this.vista.DatosAdicionales.Count)
            {
                this.vista.DatosAdicionales.RemoveAt(id);
                this.vista.MostrarListaDatosAdicionales();
            }
        }
        #endregion
    }
}
