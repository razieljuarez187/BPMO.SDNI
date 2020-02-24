//Satisface al Caso de uso CU005 - Armar Paquetes Facturacion
using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using BPMO.Primitivos.Enumeradores;
using BPMO.SDNI.Facturacion.AplicacionesFacturacion.PRE;
using BPMO.SDNI.Facturacion.AplicacionesFacturacion.VIS;
using BPMO.SDNI.MapaSitio.UI;

namespace BPMO.SDNI.Facturacion.AplicacionesFacturacion.UI
{
    /// <summary>
    /// Control que visualiza la sección de herramientas de factura para una factura
    /// </summary>
    public partial class ucHerramientasPrefacturaUI : System.Web.UI.UserControl, IucHerramientasPrefacturaVIS
    {
        #region Campos
        /// <summary>
        /// Nombre de la clase para agregar a los mensajes de Error
        /// </summary>
        private string nombreClase = "ucHerramientasPrefactura";

        /// <summary>
        /// Presentador asociado a la vista
        /// </summary>
        private ucHerramientasPrefacturaPRE presentador;
        #endregion

        #region Propiedades

        /// <summary>
        /// Obtiene o establece un valor que representa el número de contrato asociada a la factura
        /// </summary>
        /// <value>Objeto de tipo String</value>
        public string NumeroContrato
        {
            get
            {
                var txtNumeroContrato = mnContratos.Controls[0].FindControl("txtValue") as TextBox;
                if (txtNumeroContrato != null && txtNumeroContrato.Text != null && !string.IsNullOrEmpty(txtNumeroContrato.Text.Trim()))
                    return txtNumeroContrato.Text.Trim();
                return null;
            }
            set
            {
                var txtNumeroContrato = mnContratos.Controls[0].FindControl("txtValue") as TextBox;
                if (txtNumeroContrato != null)
                {
                    txtNumeroContrato.Text = value ?? string.Empty;
                }
            }
        }

        /// <summary>
        /// Obtiene o establece un valor que representa el número de pago asociada a la factura
        /// </summary>
        /// <value>Objeto de tipo Int32</value>
        public short? NumeroPago
        {
            get
            {
                var txtNumeroPago = mnContratos.Controls[1].FindControl("txtValue") as TextBox;
                short value = 0;
                if (txtNumeroPago != null && short.TryParse(txtNumeroPago.Text, out value))
                    return value;

                return null;
            }
            set
            {
                var txtNumeroPago = mnContratos.Controls[1].FindControl("txtValue") as TextBox;
                if (txtNumeroPago != null)
                {
                    txtNumeroPago.Text = value.HasValue ? value.ToString() : string.Empty;
                }
            }
        }        
        #endregion

        #region Métodos
        /// <summary>
        /// Proceso que se ejecuta cuando se crea una nueva factura
        /// </summary>
        public void PrepararNuevo()
        {
            this.NumeroContrato = String.Empty;
            this.NumeroPago = 0;          
        }

        /// <summary>
        /// Desplegar mensaje de Error con detalle
        /// </summary>
        /// <param name="mensaje">Descripción del mensaje</param>
        /// <param name="tipo">EMensaje tipo de mensaje a desplegar</param>
        /// <param name="detalle">Detalle del mensaje a desplegar</param>
        public void MostrarMensaje(string mensaje, ETipoMensajeIU tipo, string msjDetalle = null)
        {
            Site masterMsj = (Site)Page.Master;
            if (tipo == ETipoMensajeIU.ERROR)
                masterMsj.MostrarMensaje(mensaje, tipo, msjDetalle);
            else
                masterMsj.MostrarMensaje(mensaje, tipo);
        }
        #endregion

        #region Eventos
        /// <summary>
        /// Método delegado para el evento de carga de la página
        /// </summary>
        /// <param name="sender">Objeto que desencadeno el evento</param>
        /// <param name="e">Argumento asociados al evento</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                this.presentador = new ucHerramientasPrefacturaPRE(this);

                if (!this.IsPostBack)
                {                    
                }
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencia al crear la página", ETipoMensajeIU.ERROR, this.nombreClase + ".Page_Load:" + ex.Message);
            }
        }
        #endregion
    }
}