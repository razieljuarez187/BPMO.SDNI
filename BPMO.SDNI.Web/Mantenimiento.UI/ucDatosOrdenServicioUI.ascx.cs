// Satisface al CU062 - Obtener Orden Servicio Idealease
using System;
using System.Collections.Generic;
using System.Web.UI;
using BPMO.Primitivos.Enumeradores;
using BPMO.SDNI.Comun.BO;
using BPMO.SDNI.Comun.PRE;
using BPMO.SDNI.Comun.UI;
using BPMO.SDNI.MapaSitio.UI;
using Newtonsoft.Json;

namespace BPMO.SDNI.Mantenimiento.UI {

    /// <summary>
    /// Clase que contiene los métodos para la presentación de la información de los detalles del Mantenimiento Idealease, al usuario.
    /// </summary>
    public partial class ucDatosOrdenServicioUI : System.Web.UI.UserControl {
        
        #region Atributos

        /// <summary>
        /// Nombre de la clase en curso.
        /// </summary>
        private readonly string nombreClase = typeof(ucDatosOrdenServicioUI).Name;

        /// <summary>
        /// Obtiene o establece un valor que representa el tipo de Mantenimiento Idealease seleccionado.
        /// </summary>
        public bool EsUnidad { 
            get { return (bool)Session["esUnidad"]; }
        }

        /// <summary>
        /// Obtiene o establece un valor que representa un diccionario de datos del Mantenimiento Idealease seleccionado.
        /// </summary>
        public Dictionary<string, string> datos {
            get { return Session["mantenimientoHash"] as Dictionary<string, string>; }
        }

        public int? UnidadOperativaId
        {
            get
            {
                Site masterMsj = (Site)Page.Master;

                if (masterMsj != null && (masterMsj.Adscripcion != null && masterMsj.Adscripcion.UnidadOperativa != null))
                    return masterMsj.Adscripcion.UnidadOperativa.Id;
                return null;
            }
        }
        
        #endregion

        #region Constructor

        /// <summary>
        /// Método delegado para el evento de carga de la página. Carga los detalles del Mantenimiento Idealease.
        /// </summary>
        /// <param name="sender">Objeto que desencadenó el evento.</param>
        /// <param name="e">Argumento asociado al evento.</param>
        protected void Page_Load(object sender, EventArgs e) {
            if (!IsPostBack) {
                if(datos != null && datos.Keys.Count > 0){
                    CargarDatosMantenimiento();
                    EstablecerEtiqueta();
                }
            }
        }
        
        #endregion

        #region Métodos

        /// <summary>
        /// Despliega los detalles del Mantenimiento Idealease seleccionado.
        /// </summary>
        public void CargarDatosMantenimiento() {
            this.hdnIdMantenimiento.Value = datos["id"];
            this.txtFVin.Text = datos["numeroSerie"];
            this.txtFNumEco.Text = datos["numeroEconomico"];
            this.txtFModelo.Text = datos["modelo"];
            this.txtFCliente.Text = datos["cliente"];
            this.txtFKilometraje.Text = datos["kilometraje"];
            this.txtFHorometro.Text = datos["horometro"];
            this.txtFCombusTotal.Text = datos["totalCombustible"];
            this.txtFSucursal.Text = datos["sucursal"];
            this.txtFTaller.Text = datos["taller"];
            this.txtFTipoServicio.Text = datos["tipoMantenimiento"];
            this.txtFCombusEntra.Text = datos["combustibleEntrada"];
            this.txtFCombusSalida.Text = datos["combustibleSalida"];
            this.txtFTipoOrdenServicio.Text = datos["tipoServicio"];
            this.txtFControlista.Text = datos["controlista"];
            this.txtFOperador.Text = datos["operador"];
        }

        /// <summary>
        /// Despliega un mensaje en pantalla.
        /// </summary>
        /// <param name="mensaje">Mensaje a desplegar.</param>
        /// <param name="tipo">Tipo de mensaje a desplegar.</param>
        /// <param name="msjDetalle">Muestra una cadena de texto que contiene los datalles del mensaje a desplegar.</param>
        private void MostrarMensaje(string mensaje, ETipoMensajeIU tipo, string msjDetalle = null) {
            Site masterMsj = (Site)Page.Master;
            if (tipo == ETipoMensajeIU.ERROR) {
                if (masterMsj != null)
                    masterMsj.MostrarMensaje(mensaje, tipo, msjDetalle);
            } else {
                if (masterMsj != null)
                    masterMsj.MostrarMensaje(mensaje, tipo);
            }
        }

        private void EstablecerEtiqueta()
        {
            if (this.UnidadOperativaId != null)
            {
                string etiqueta = ObtenerEtiquetadelResource("RE04", (ETipoEmpresa)this.UnidadOperativaId);
                if (!string.IsNullOrEmpty(etiqueta)) {
                    this.lblNumeroEconomico.Text = etiqueta;
                }
            }
        }

        /// <summary>
        /// Método que obtiene el nombre de la etiqueta del archivo resource correspondiente a la unidad operativa a la cual accedió el usuario.
        /// </summary>
        /// <param name="cEtiquetaBuscar">Nombre de la etiqueta que será buscada en el archivo resource</param>
        /// <param name="tipoEmpresa">Nombre de la unidad operativa a la cual accedió el usuario.</param>
        /// <returns>Retorna el nombre de la etiqueta correspondiente al valor recibido en el parámetro cEtiquetaBuscar del archivo resource.</returns>
        public string ObtenerEtiquetadelResource(string cEtiquetaBuscar, ETipoEmpresa tipoEmpresa)
        {
            string cEtiqueta = string.Empty;
            //Instanciamos la clase o webmethod que obtiene las etiquetas
            ObtenerEtiquetaEmpresas obtenerEtiqueta = new ObtenerEtiquetaEmpresas();
            string cEtiquetaObtenida = string.Empty;
            EtiquetaObtenida request = null;

            cEtiquetaObtenida = obtenerEtiqueta.ObtenerEtiquetadelResource(cEtiquetaBuscar, (int)tipoEmpresa);
            request = JsonConvert.DeserializeObject<EtiquetaObtenida>(cEtiquetaObtenida);
            if (string.IsNullOrEmpty(request.cMensaje))
            {
                cEtiquetaObtenida = request.cEtiqueta;
                if (cEtiqueta != "NO APLICA")
                {
                    cEtiqueta = cEtiquetaObtenida;
                }
            }
            return cEtiqueta;
        }
        #endregion

    }
}