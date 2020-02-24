//Satisface al Caso de uso CU005 - Armar Paquetes Facturación

using System;
using System.Collections.Generic;
using BPMO.Basicos.BO;
using BPMO.Facade.SDNI.BR;
using BPMO.Patterns.Creational.DataContext;
using BPMO.Primitivos.Enumeradores;
using BPMO.SDNI.Facturacion.AplicacionesFacturacion.BR;
using BPMO.SDNI.Facturacion.AplicacionesFacturacion.VIS;
using BPMO.SDNI.Facturacion.InfraestructuraPagos.BO;
using BPMO.SDNI.Facturacion.InfraestructuraPagos.BR;

namespace BPMO.SDNI.Facturacion.AplicacionesFacturacion.PRE
{
    /// <summary>
    /// Presentador para la vista que visualiza la información de la cabecera de una factura
    /// </summary>
    public class ucInformacionGeneralPSLPRE
    {
        #region Atributos
        /// <summary>
        /// El DataContext que proveerá acceso a la base de datos
        /// </summary>
        private IDataContext dctx = null;

        /// <summary>
        /// La vista que esta mostrando los datos procesados en el presentador
        /// </summary>
        private IucInformacionGeneralPSLVIS vista;

        /// <summary>
        /// Nombre de la clase para agregar a los mensajes de Error
        /// </summary>
        private string nombreClase = "ucInformacionCabeceraPSLPRE";

        /// <summary>
        /// Controlador para los pagos de unidad
        /// </summary>
        private PagoUnidadContratoBR pagoUnidadContratoBR;

        /// <summary>
        /// Controlador para las formas de pago
        /// </summary>
        private FormaPagoFacturacionBR formaPagoFacturacionBR;
        #endregion

        #region Constructores
        /// <summary>
        /// Constructor por default
        /// </summary>
        /// <param name="view">Vista a la que se asociada el presentador</param>
        public ucInformacionGeneralPSLPRE(IucInformacionGeneralPSLVIS view)
        {
            try
            {
                this.vista = view;
                this.dctx = FacadeBR.ObtenerConexion();
                this.pagoUnidadContratoBR = new PagoUnidadContratoBR();
                this.formaPagoFacturacionBR = new FormaPagoFacturacionBR();
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
            this.vista.MostrarFormasPago(this.formaPagoFacturacionBR.Consultar(this.vista.ObtenerCarpetaRaiz()));
        }

        /// <summary>
        /// Despliega la información de la descripción de la linea
        /// </summary>
        /// <param name="tipolinea">tipo línea 'HORA' o 'DESCRIPCIONLINEA'</param>
        /// <param name="PagolineaID">Identificador del pago</param>
        public void DesplegarDescripcionLinea(string tipolinea, int PagolineaID)
        {
            object modelolista = this.vista.InformacionLineasFacturaModel;

            List<LineasFacturaModel> listModel = modelolista as List<LineasFacturaModel>;
            this.vista.DescripcionMotivoLinea = "";
            foreach (LineasFacturaModel lineaFactura in listModel)
            {
                if (lineaFactura.PagoContratoPSLID == PagolineaID) {
                    if (tipolinea == "HORA")
                        this.vista.DescripcionMotivoLinea = lineaFactura.DescripcionhoraAdicional;
                    else
                        this.vista.DescripcionMotivoLinea = lineaFactura.DescripcionLinea;
                }
            }
        }

        #endregion
    }
}
