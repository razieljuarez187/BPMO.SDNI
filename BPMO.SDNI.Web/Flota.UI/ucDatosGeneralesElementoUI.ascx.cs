//Esta clase satisface los requerimientos especificados en el caso de uso CU006 Consultar Flota de Renta Diaria
using System;
using BPMO.SDNI.Comun.BO;
using BPMO.SDNI.Comun.PRE;
using BPMO.SDNI.Comun.UI;
using BPMO.SDNI.Flota.VIS;
using BPMO.SDNI.MapaSitio.UI;
using Newtonsoft.Json;

namespace BPMO.SDNI.Flota.UI
{
    public partial class ucDatosGeneralesElementoUI : System.Web.UI.UserControl, IucDatosGeneralesElementoVIS
    {
        #region Propiedades
        /// <summary>
        /// Obtiene o establece el identificador de la unidad 
        /// </summary>
        public string UnidadID
        {
            get { return this.hdnUnidadID.Value.Trim().ToUpper(); }
            set { this.hdnUnidadID.Value = value; }
        }
        /// <summary>
        /// Obtiene o establece el identificador del equipo de la unidad
        /// </summary>
        public string EquipoID
        {
            get { return this.hdnEquipoID.Value.Trim().ToUpper(); }
            set { this.hdnEquipoID.Value = value; }
        }
        /// <summary>
        /// Obtiene o establece el identificador de líder de la unidad
        /// </summary>
        public string LiderID
        {
            get { return this.hdnLiderID.Value.Trim().ToUpper(); }
            set { this.hdnLiderID.Value = value; }
        }
        /// <summary>
        /// Obtiene o establece el identificador de oracle de la unidad
        /// </summary>
        public string OracleID
        {
            get { return this.hdnOracleID.Value.Trim().ToUpper(); }
            set { this.hdnOracleID.Value = value; }
        }
        /// <summary>
        /// Obtiene o establece el número económico de la unidad
        /// </summary>
        public string NumeroEconomico
        {
            get { return this.txtNumeroEconomico.Text.Trim().ToUpper(); }
            set { this.txtNumeroEconomico.Text = value; }
        }
        /// <summary>
        /// Obtiene o establece el número de serie de la unidad
        /// </summary>
        public string Numeroserie
        {
            get { return this.txtNumeroSerie.Text.Trim().ToUpper(); }
            set { this.txtNumeroSerie.Text = value; }
        }
        /// <summary>
        /// Obtiene o establece las placas federales de la unidad
        /// </summary>
        public string PlacasFederales
        {
            get { return this.txtPlacasFederales.Text.Trim().ToUpper(); }
            set { this.txtPlacasFederales.Text = value; }
        }
        /// <summary>
        /// Obtiene o establece las placas estatales de la unidad
        /// </summary>
        public string PlacasEstatales
        {
            get { return this.txtPlacasEstales.Text.Trim().ToUpper(); }
            set { this.txtPlacasEstales.Text = value; }
        }
        /// <summary>
        /// Obtiene o establece la marca de la unidad
        /// </summary>
        public string Marca
        {
            get { return this.txtMarca.Text.Trim().ToUpper(); }
            set { this.txtMarca.Text = value; }
        }
        /// <summary>
        /// Obtiene o establece el modelo de la unidad
        /// </summary>
        public string Modelo
        {
            get { return this.txtModelo.Text.Trim().ToUpper(); }
            set { this.txtModelo.Text = value; }
        }
        /// <summary>
        /// Obtiene o establece la capacidad de carga de la unidad
        /// </summary>
        public string CapacidadCarga
        {
            get { return this.txtCapacidadCarga.Text.Trim().ToUpper(); }
            set
            {
                #region SC0020
                if (value != null && value != string.Empty)
                {
                    decimal valor = Convert.ToDecimal(value);
                    this.txtCapacidadCarga.Text = String.Format("{0:#,##0.00##}", valor);
                }
                #endregion SC0020
            }
        }

        /// <summary>
        /// Obtiene o establece la capacidad del tanque de la unidad
        /// </summary>
        public string CapacidadTanque
        {
            get { return this.txtCapacidadTanque.Text.Trim().ToUpper(); }
            set
            {
                #region SC0020
                if (value != null && value != string.Empty)
                {
                    decimal valor = Convert.ToDecimal(value);
                    this.txtCapacidadTanque.Text = String.Format("{0:#,##0.00##}", valor);
                }
                #endregion SC0020
            }
        }

        /// <summary>
        /// Obtiene o establece el rendimiento del tanque de la unidad
        /// </summary>
        public string RendimientoTanque
        {
            get { return this.txtRendimientoTanque.Text.Trim().ToUpper(); }
            set
            {
                #region SC0020
                if (value != null && value != string.Empty)
                {
                    decimal valor = Convert.ToDecimal(value);
                    this.txtRendimientoTanque.Text = String.Format("{0:#,##0.00##}", valor);
                }
                #endregion SC0020
            }
        }

        /// <summary>
        /// Obtiene o establece el año de la unidad
        /// </summary>
        public string Anio
        {
            get { return this.txtAnio.Text.Trim().ToUpper(); }
            set { this.txtAnio.Text = value; }
        }
        /// <summary>
        /// Obtiene o establece la sucursal a la que pertenece la unidad
        /// </summary>
        public string Sucursal
        {
            get { return this.txtSucursal.Text.Trim().ToUpper(); }
            set { this.txtSucursal.Text = value; }
        }
        /// <summary>
        /// Clave de Producto o Servicio (SAT)
        /// </summary>
        public string ClaveProductoServicio {
            get { return (string.IsNullOrEmpty(this.txtClaveProductoServicio.Text)) ? null : this.txtClaveProductoServicio.Text.Trim().ToUpper(); }
            set { this.txtClaveProductoServicio.Text = (value != null) ? value.ToUpper() : string.Empty; }
        }
        /// <summary>
        /// Descripción de Producto o Servicio (SAT)
        /// </summary>
        public string DescripcionProductoServicio {
            get { return (string.IsNullOrEmpty(this.txtDescripcionProductoServicio.Text)) ? null : this.txtDescripcionProductoServicio.Text.Trim().ToUpper(); }
            set { this.txtDescripcionProductoServicio.Text = (value != null) ? value.ToUpper() : string.Empty; }
        }

        public int? UnidadOperativaId
        {
            get
            {
                int? id = null;
                Site masterMsj = (Site)this.Parent.Page.Master;

                if (masterMsj.Adscripcion != null && masterMsj.Adscripcion.UnidadOperativa != null)
                    id = masterMsj.Adscripcion.UnidadOperativa.Id;
                return id;
            }
        }

        private ETipoEmpresa ETipoEmpresa;

        /// <summary>
        /// Tipo de empresa logueada
        /// </summary>
        public ETipoEmpresa EnumTipoEmpresa
        {
            get
            {
                return this.ETipoEmpresa;
            }
            set
            {
                this.ETipoEmpresa = value;
            }
        }
        #endregion

        #region Métodos
        /// <summary>
        /// Los controles son desplegados en modo lectura
        /// </summary>
        public void EstablecerModoLectura()
        {
            this.txtAnio.ReadOnly = true;
            this.txtAnio.Enabled = false;
            this.txtCapacidadCarga.ReadOnly = true;
            this.txtCapacidadCarga.Enabled = false;
            this.txtCapacidadTanque.ReadOnly = true;
            this.txtCapacidadTanque.Enabled = false;
            this.txtMarca.ReadOnly = true;
            this.txtMarca.Enabled = false;
            this.txtModelo.ReadOnly = true;
            this.txtModelo.Enabled = false;
            this.txtNumeroEconomico.ReadOnly = true;
            this.txtNumeroEconomico.Enabled = false;
            this.txtNumeroSerie.ReadOnly = true;
            this.txtNumeroSerie.Enabled = false;
            this.txtPlacasEstales.ReadOnly = true;
            this.txtPlacasEstales.Enabled = false;
            this.txtPlacasFederales.ReadOnly = true;
            this.txtPlacasFederales.Enabled = false;
            this.txtRendimientoTanque.ReadOnly = true;
            this.txtRendimientoTanque.Enabled = false;
            this.txtSucursal.ReadOnly = true;
            this.txtSucursal.Enabled = false;
            this.txtClaveProductoServicio.ReadOnly = true;
            this.txtClaveProductoServicio.Enabled = false;
        }
        /// <summary>
        /// Muestra u oculta la información de producto-Servicio
        /// </summary>
        /// <param name="mostrar"></param>
        public void MostrarProductoServicio(bool mostrar) {
            this.lblClaveProductoServicio.Visible = mostrar;
            this.txtClaveProductoServicio.Visible = mostrar;
            this.txtDescripcionProductoServicio.Visible = mostrar;
        }

        /// <summary>
        /// Método que obtiene el nombre de la etiqueta del archivo resource correspondiente a la unidad operativa a la cual accedió el usuario.
        /// </summary>
        /// <param name="etiquetaBuscar">Nombre de la etiqueta que será buscada en el archivo resource</param>
        /// <param name="tipoEmpresa">Nombre de la unidad operativa a la cual accedió el usuario.</param>
        /// <returns>Retorna el nombre de la etiqueta correspondiente al valor recibido en el parámetro etiquetaBuscar del archivo resource.</returns>
        private string ObtenerEtiquetadelResource(string etiquetaBuscar, ETipoEmpresa tipoEmpresa)
        {
            string Etiqueta = string.Empty;
            //Instanciamos la clase o webmethod que obtiene las etiquetas
            ObtenerEtiquetaEmpresas obtenerEtiqueta = new ObtenerEtiquetaEmpresas();
            string EtiquetaObtenida = string.Empty;
            EtiquetaObtenida request = null;

            EtiquetaObtenida = obtenerEtiqueta.ObtenerEtiquetadelResource(etiquetaBuscar, (int)tipoEmpresa);
            request = JsonConvert.DeserializeObject<EtiquetaObtenida>(EtiquetaObtenida);
            if (string.IsNullOrEmpty(request.cMensaje))
            {
                EtiquetaObtenida = request.cEtiqueta;
                if (Etiqueta != "NO APLICA")
                {
                    Etiqueta = EtiquetaObtenida;
                }
            }
            return Etiqueta;
        }

        /// <summary>
        /// Prepara los controles (etiquetas y visualización) que serán válidos para la unidad operativa Generación.
        /// </summary>
        /// <param name="tipoEmpresa">Indica la unidad operativa, este valor determina el comportamiento de los controles.</param>
        public void EstablecerAcciones()
        {
            this.EnumTipoEmpresa = (ETipoEmpresa)this.UnidadOperativaId;

            if (this.EnumTipoEmpresa == ETipoEmpresa.Construccion || this.EnumTipoEmpresa == ETipoEmpresa.Generacion || this.EnumTipoEmpresa == ETipoEmpresa.Equinova)
            {
                string eCode = ObtenerEtiquetadelResource("RE04", this.EnumTipoEmpresa);
                string serieUnidad = ObtenerEtiquetadelResource("RE01", this.EnumTipoEmpresa);
                string ft = ObtenerEtiquetadelResource("EQ01", this.EnumTipoEmpresa);
                this.lblNumeroEconomico.Text = eCode;
                this.lblNumeroSerie.Text = serieUnidad;
                this.lblPBC.Text = ft;

                this.txtPlacasFederales.Visible = false;
                this.txtRendimientoTanque.Visible = false;
                this.lblPlacasFederales.Visible = false;
                this.lblRendimientoTanque.Visible = false;
            }
        }
        #endregion

        #region Eventos
        protected void Page_Load(object sender, EventArgs e)
        {

            this.EstablecerAcciones();
        }
        #endregion
    }
}