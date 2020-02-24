//Satisface al CU079 - Consultar Acta de Nacimiento de Unidad
using System;
using System.Collections.Generic;
using BPMO.SDNI.Comun.BO;
using BPMO.SDNI.Comun.PRE;
using BPMO.SDNI.Comun.UI;
using BPMO.SDNI.Equipos.BO;
using BPMO.SDNI.Equipos.PRE;
using BPMO.SDNI.Equipos.VIS;
using BPMO.SDNI.MapaSitio.UI;
using Newtonsoft.Json;

namespace BPMO.SDNI.Equipos.UI
{
    public partial class ucActaNacimientoUI : System.Web.UI.UserControl, IucActaNacimientoVIS
    {
        #region Atributos
        ucActaNacimientoPRE presentador;
        #endregion

        #region Propiedades
        public string NumeroSerie
        {
            get
            {
                return (String.IsNullOrEmpty(this.txtNumSerie.Text)) ? null : this.txtNumSerie.Text.Trim().ToUpper();
            }
            set
            {
                if (value != null)
                    this.txtNumSerie.Text = value;
                else
                    this.txtNumSerie.Text = string.Empty;
            }
        }
        public string ClaveActivoOracle
        {
            get
            {
                return (String.IsNullOrEmpty(this.txtClaveOracle.Text)) ? null : this.txtClaveOracle.Text.Trim().ToUpper();
            }
            set
            {
                if (value != null)
                    this.txtClaveOracle.Text = value;
                else
                    this.txtClaveOracle.Text = string.Empty;
            }
        }
        public int? LiderID
        {
            get
            {
                int? id = null;
                if (!String.IsNullOrEmpty(this.txtIDLeader.Text))
                    id = int.Parse(this.txtIDLeader.Text.Trim());
                return id;
            }
            set
            {
                if (value != null)
                    this.txtIDLeader.Text = value.ToString();
                else
                    this.txtIDLeader.Text = string.Empty;
            }
        }
        public string NumeroEconomico
        {
            get
            {
                return (String.IsNullOrEmpty(this.txtNumEconomico.Text)) ? null : this.txtNumEconomico.Text.Trim().ToUpper();
            }
            set
            {
                if (value != null)
                    this.txtNumEconomico.Text = value;
                else
                    this.txtNumEconomico.Text = string.Empty;
            }
        }
        public string TipoUnidad
        {
            get
            {
                return (String.IsNullOrEmpty(this.txtTipoUnidad.Text)) ? null : this.txtTipoUnidad.Text.Trim().ToUpper();
            }
            set
            {
                if (value != null)
                    this.txtTipoUnidad.Text = value;
                else
                    this.txtTipoUnidad.Text = string.Empty;
            }
        }
        public string Modelo
        {
            get
            {
                return (String.IsNullOrEmpty(this.txtModelo.Text)) ? null : this.txtModelo.Text.Trim().ToUpper();
            }
            set
            {
                if (value != null)
                    this.txtModelo.Text = value;
                else
                    this.txtModelo.Text = string.Empty;
            }
        }
        public int? Anio
        {
            get
            {
                int? id = null;
                if (!String.IsNullOrEmpty(this.txtAnio.Text))
                    id = int.Parse(this.txtAnio.Text.Trim());
                return id;
            }
            set
            {
                if (value != null)
                    this.txtAnio.Text = value.ToString();
                else
                    this.txtAnio.Text = string.Empty;
            }
        }
        public DateTime? FechaCompra
        {
            get
            {
                DateTime? temp = null;
                if (!String.IsNullOrEmpty(this.txtFechaCompra.Text))
                    temp = DateTime.Parse(this.txtFechaCompra.Text.Trim());
                return temp;
            }
            set
            {
                if (value != null)
                    this.txtFechaCompra.Text = value.ToString();
                else
                    this.txtFechaCompra.Text = string.Empty;
            }
        }
        public decimal? MontoFactura
        {
            get
            {
                decimal? temp = null;
                if (!String.IsNullOrEmpty(this.txtMontoFactura.Text))
                    temp = decimal.Parse(this.txtMontoFactura.Text.Trim().Replace(",","")); //RI0012
                return temp;
            }
            set
            {
                if (value != null)
					this.txtMontoFactura.Text = string.Format("{0:#,##0.0000}", value);//RI0012
                else
                    this.txtMontoFactura.Text = string.Empty;
            }
        }

        public string Propietario
        {
            get
            {
                return (String.IsNullOrEmpty(this.txtPropietario.Text)) ? null : this.txtPropietario.Text.Trim().ToUpper();
            }
            set
            {
                if (value != null)
                    this.txtPropietario.Text = value;
                else
                    this.txtPropietario.Text = string.Empty;
            }
        }
        public string Cliente
        {
            get
            {
                return (String.IsNullOrEmpty(this.txtCliente.Text)) ? null : this.txtCliente.Text.Trim().ToUpper();
            }
            set
            {
                if (value != null)
                    this.txtCliente.Text = value;
                else
                    this.txtCliente.Text = string.Empty;
            }
        }
        public string Sucursal
        {
            get
            {
                return (String.IsNullOrEmpty(this.txtSucursal.Text)) ? null : this.txtSucursal.Text.Trim().ToUpper();
            }
            set
            {
                if (value != null)
                    this.txtSucursal.Text = value;
                else
                    this.txtSucursal.Text = string.Empty;
            }
        }
        public string Area
        {
            get
            {
                string area = null;
                if (!String.IsNullOrEmpty(this.txtArea.Text))
                    area = this.txtArea.Text;
                return area;
            }
            set
            {
                if (value == null)
                    this.txtArea.Text = "";
                else
                    this.txtArea.Text = value.ToString();
            }
        }
        
        public List<HorometroBO> Horometros
        {
            get
            {
                if ((List<HorometroBO>)Session["ListaHorometrosOriginal"] == null)
                    return new List<HorometroBO>();
                else
                    return (List<HorometroBO>)Session["ListaHorometrosOriginal"];
            }
            set
            {
                Session["ListaHorometrosOriginal"] = value;
            }
        }
        public List<OdometroBO> Odometros
        {
            get
            {
                if ((List<OdometroBO>)Session["ListaOdometrosOriginal"] == null)
                    return new List<OdometroBO>();
                else
                    return (List<OdometroBO>)Session["ListaOdometrosOriginal"];
            }
            set
            {
                Session["ListaOdometrosOriginal"] = value;
            }
        }
        public decimal? PBVMaximoRecomendado
        {
            get
            {
                decimal? temp = null;
                if (!String.IsNullOrEmpty(this.txtPBV.Text))
                    temp = decimal.Parse(this.txtPBV.Text.Trim().Replace(",","")); //RI0012
                return temp;
            }
            set
            {
                if (value != null)
					this.txtPBV.Text = string.Format("{0:#,##0.0000}", value);//RI0012
                else
                    this.txtPBV.Text = string.Empty;
            }
        }
        public decimal? PBCMaximoRecomendado
        {
            get
            {
                decimal? temp = null;
                if (!String.IsNullOrEmpty(this.txtPBC.Text))
                    temp = decimal.Parse(this.txtPBC.Text.Trim().Replace(",","")); //RI0012
                return temp;
            }
            set
            {
                if (value != null)
					this.txtPBC.Text = string.Format("{0:#,##0.0000}", value);//RI0012
                else
                    this.txtPBC.Text = string.Empty;
            }
        }
        public decimal? CapacidadTanque
        {
            get
            {
                decimal? temp = null;
                if (!String.IsNullOrEmpty(this.txtCapacidadTanque.Text))
                    temp = decimal.Parse(this.txtCapacidadTanque.Text.Trim().Replace(",","")); //RI0012
                return temp;
            }
            set
            {
                if (value != null)
					this.txtCapacidadTanque.Text = string.Format("{0:#,##0.0000}", value);//RI0012
                else
                    this.txtCapacidadTanque.Text = string.Empty;
            }
        }
        public decimal? RendimientoTanque
        {
            get
            {
                decimal? temp = null;
                if (!String.IsNullOrEmpty(this.txtRendimientoTanque.Text))
                    temp = decimal.Parse(this.txtRendimientoTanque.Text.Trim().Replace(",","")); //RI0012
                return temp;
            }
            set
            {
                if (value != null)
					this.txtRendimientoTanque.Text = string.Format("{0:#,##0.0000}", value);//RI0012 
                else
                    this.txtRendimientoTanque.Text = string.Empty;
            }
        }

        public string Radiador
        {
            get
            {
                return (String.IsNullOrEmpty(this.txtRadiador.Text)) ? null : this.txtRadiador.Text.Trim().ToUpper();
            }
            set
            {
                if (value != null)
                    this.txtRadiador.Text = value;
                else
                    this.txtRadiador.Text = string.Empty;
            }
        }
        public string PostEnfriador
        {
            get
            {
                return (String.IsNullOrEmpty(this.txtPostEnfriador.Text)) ? null : this.txtPostEnfriador.Text.Trim().ToUpper();
            }
            set
            {
                if (value != null)
                    this.txtPostEnfriador.Text = value;
                else
                    this.txtPostEnfriador.Text = string.Empty;
            }
        }
        #region SC0030
        public string SerieMotor {
            get {
                return (String.IsNullOrEmpty(this.txtSerieMotor.Text)) ? null : this.txtSerieMotor.Text.Trim().ToUpper();
            }
            set {
                if (value != null)
                    this.txtSerieMotor.Text = value;
                else
                    this.txtSerieMotor.Text = string.Empty;
            }
        }
        #endregion
        public string SerieTurboCargador
        {
            get
            {
                return (String.IsNullOrEmpty(this.txtSerieTurboCargador.Text)) ? null : this.txtSerieTurboCargador.Text.Trim().ToUpper();
            }
            set
            {
                if (value != null)
                    this.txtSerieTurboCargador.Text = value;
                else
                    this.txtSerieTurboCargador.Text = string.Empty;
            }
        }
        public string SerieCompresorAire
        {
            get
            {
                return (String.IsNullOrEmpty(this.txtCompresorAire.Text)) ? null : this.txtCompresorAire.Text.Trim().ToUpper();
            }
            set
            {
                if (value != null)
                    this.txtCompresorAire.Text = value;
                else
                    this.txtCompresorAire.Text = string.Empty;
            }
        }
        public string SerieECM
        {
            get
            {
                return (String.IsNullOrEmpty(this.txtECM.Text)) ? null : this.txtECM.Text.Trim().ToUpper();
            }
            set
            {
                if (value != null)
                    this.txtECM.Text = value;
                else
                    this.txtECM.Text = string.Empty;
            }
        }
        public string SerieAlternador
        {
            get
            {
                return (String.IsNullOrEmpty(this.txtAlternador.Text)) ? null : this.txtAlternador.Text.Trim().ToUpper();
            }
            set
            {
                if (value != null)
                    this.txtAlternador.Text = value;
                else
                    this.txtAlternador.Text = string.Empty;
            }
        }
        public string SerieMarcha
        {
            get
            {
                return (String.IsNullOrEmpty(this.txtMarcha.Text)) ? null : this.txtMarcha.Text.Trim().ToUpper();
            }
            set
            {
                if (value != null)
                    this.txtMarcha.Text = value;
                else
                    this.txtMarcha.Text = string.Empty;
            }
        }
        public string SerieBaterias
        {
            get
            {
                return (String.IsNullOrEmpty(this.txtBaterias.Text)) ? null : this.txtBaterias.Text.Trim().ToUpper();
            }
            set
            {
                if (value != null)
                    this.txtBaterias.Text = value;
                else
                    this.txtBaterias.Text = string.Empty;
            }
        }
        public string TransmisionSerie
        {
            get
            {
                return (String.IsNullOrEmpty(this.txtSerieTransmision.Text)) ? null : this.txtSerieTransmision.Text.Trim().ToUpper();
            }
            set
            {
                if (value != null)
                    this.txtSerieTransmision.Text = value;
                else
                    this.txtSerieTransmision.Text = string.Empty;
            }
        }
        public string TransmisionModelo
        {
            get
            {
                return (String.IsNullOrEmpty(this.txtModeloTransmision.Text)) ? null : this.txtModeloTransmision.Text.Trim().ToUpper();
            }
            set
            {
                if (value != null)
                    this.txtModeloTransmision.Text = value;
                else
                    this.txtModeloTransmision.Text = string.Empty;
            }
        }
        public string EjeDireccionSerie
        {
            get
            {
                return (String.IsNullOrEmpty(this.txtSerieEjeDireccion.Text)) ? null : this.txtSerieEjeDireccion.Text.Trim().ToUpper();
            }
            set
            {
                if (value != null)
                    this.txtSerieEjeDireccion.Text = value;
                else
                    this.txtSerieEjeDireccion.Text = string.Empty;
            }
        }
        public string EjeDireccionModelo
        {
            get
            {
                return (String.IsNullOrEmpty(this.txtModeloEjeDireccion.Text)) ? null : this.txtModeloEjeDireccion.Text.Trim().ToUpper();
            }
            set
            {
                if (value != null)
                    this.txtModeloEjeDireccion.Text = value;
                else
                    this.txtModeloEjeDireccion.Text = string.Empty;
            }
        }
        public string EjeTraseroDelanteroSerie
        {
            get
            {
                return (String.IsNullOrEmpty(this.txtSerieEjeTraseroDelantero.Text)) ? null : this.txtSerieEjeTraseroDelantero.Text.Trim().ToUpper();
            }
            set
            {
                if (value != null)
                    this.txtSerieEjeTraseroDelantero.Text = value;
                else
                    this.txtSerieEjeTraseroDelantero.Text = string.Empty;
            }
        }
        public string EjeTraseroDelanteroModelo
        {
            get
            {
                return (String.IsNullOrEmpty(this.txtModeloEjeTraseroDelantero.Text)) ? null : this.txtModeloEjeTraseroDelantero.Text.Trim().ToUpper();
            }
            set
            {
                if (value != null)
                    this.txtModeloEjeTraseroDelantero.Text = value;
                else
                    this.txtModeloEjeTraseroDelantero.Text = string.Empty;
            }
        }
        public string EjeTraseroTraseroSerie
        {
            get
            {
                return (String.IsNullOrEmpty(this.txtSerieEjeTraseroTrasero.Text)) ? null : this.txtSerieEjeTraseroTrasero.Text.Trim().ToUpper();
            }
            set
            {
                if (value != null)
                    this.txtSerieEjeTraseroTrasero.Text = value;
                else
                    this.txtSerieEjeTraseroTrasero.Text = string.Empty;
            }
        }
        public string EjeTraseroTraseroModelo
        {
            get
            {
                return (String.IsNullOrEmpty(this.txtModeloEjeTraseroTrasero.Text)) ? null : this.txtModeloEjeTraseroTrasero.Text.Trim().ToUpper();
            }
            set
            {
                if (value != null)
                    this.txtModeloEjeTraseroTrasero.Text = value;
                else
                    this.txtModeloEjeTraseroTrasero.Text = string.Empty;
            }
        }

        public List<LlantaBO> Llantas
        {
            get
            {
                if ((List<LlantaBO>)Session["ListaLlantasOriginal"] == null)
                    return new List<LlantaBO>();
                else
                    return (List<LlantaBO>)Session["ListaLlantasOriginal"];
            }
            set
            {
                Session["ListaLlantasOriginal"] = value;
            }
        }
        public string RefaccionCodigo
        {
            get
            {
                return (String.IsNullOrEmpty(this.txtRefaccionCodigo.Text)) ? null : this.txtRefaccionCodigo.Text.Trim().ToUpper();
            }
            set
            {
                if (value != null)
                    this.txtRefaccionCodigo.Text = value;
                else
                    this.txtRefaccionCodigo.Text = string.Empty;
            }
        }
        public string RefaccionMarca
        {
            get
            {
                return (String.IsNullOrEmpty(this.txtRefaccionMarca.Text)) ? null : this.txtRefaccionMarca.Text.Trim().ToUpper();
            }
            set
            {
                if (value != null)
                    this.txtRefaccionMarca.Text = value;
                else
                    this.txtRefaccionMarca.Text = string.Empty;
            }
        }
        public string RefaccionModelo
        {
            get
            {
                return (String.IsNullOrEmpty(this.txtRefaccionModelo.Text)) ? null : this.txtRefaccionModelo.Text.Trim().ToUpper();
            }
            set
            {
                if (value != null)
                    this.txtRefaccionModelo.Text = value;
                else
                    this.txtRefaccionModelo.Text = string.Empty;
            }
        }
        public string RefaccionMedida
        {
            get
            {
                return (String.IsNullOrEmpty(this.txtRefaccionMedida.Text)) ? null : this.txtRefaccionMedida.Text.Trim().ToUpper();
            }
            set
            {
                if (value != null)
                    this.txtRefaccionMedida.Text = value;
                else
                    this.txtRefaccionMedida.Text = string.Empty;
            }
        }
        public decimal? RefaccionProfundidad
        {
            get
            {
                decimal? temp = null;
                if (!String.IsNullOrEmpty(this.txtRefaccionProfundidad.Text))
                    temp = decimal.Parse(this.txtRefaccionProfundidad.Text.Trim().Replace(",","")); //RI0012
                return temp;
            }
            set
            {
                if (value != null)
					this.txtRefaccionProfundidad.Text = string.Format("{0:#,##0.0000}", value);//RI0012
                else
                    this.txtRefaccionProfundidad.Text = string.Empty;
            }
        }
        public Boolean? RefaccionRevitalizada
        {
            get
            {
                if (!String.IsNullOrEmpty(this.txtRefaccionRevitalizada.Text))
                {
                    if (this.txtRefaccionRevitalizada.Text.Trim().CompareTo("SI") == 0)
                        return true;
                    if (this.txtRefaccionRevitalizada.Text.Trim().CompareTo("NO") == 0)
                        return false;
                }
                return null;
            }
            set
            {
                if (value != null)
                {
                    if (value == true)
                        this.txtRefaccionRevitalizada.Text = "SI";
                    else
                        this.txtRefaccionRevitalizada.Text = "NO";
                }
                else
                    this.txtRefaccionRevitalizada.Text = string.Empty;
            }
        }

        public List<EquipoAliadoBO> EquiposAliados
        {
            get
            {
                if ((List<EquipoAliadoBO>)Session["ListaEquiposAliadosOriginal"] == null)
                    return new List<EquipoAliadoBO>();
                else
                    return (List<EquipoAliadoBO>)Session["ListaEquiposAliadosOriginal"];
            }
            set
            {
                Session["ListaEquiposAliadosOriginal"] = value;
            }
        }
        
        public List<NumeroSerieBO> NumerosSerie {
            get {
                if ((List<NumeroSerieBO>)Session["ListaNumerosSerie"] == null)
                return new List<NumeroSerieBO>();
                else
                return (List<NumeroSerieBO>)Session["ListaNumerosSerie"];
            }
            set
            {
                Session["ListaNumerosSerie"] = value;
                this.grdNumerosSerie.DataSource = value;
                this.grdNumerosSerie.DataBind();
            }
        }

        #region 13285 Acta de nacimiento 

        /// <summary>
        /// Identificador de la unidad operativa a la que accedió el usuario.
        /// </summary>
        public int? UnidadOperativaID
        {
            get
            {
                int val = 0;
                if (!string.IsNullOrEmpty(this.hdnUnidadOperativaID.Value) && !string.IsNullOrWhiteSpace(this.hdnUnidadOperativaID.Value))
                {
                    if (Int32.TryParse(this.hdnUnidadOperativaID.Value, out val))
                        return val;
                    else
                    {
                        Site masterMsj = (Site)Page.Master;

                        if (masterMsj != null && (masterMsj.Adscripcion != null && masterMsj.Adscripcion.UnidadOperativa != null))
                            return masterMsj.Adscripcion.UnidadOperativa.Id.Value;
                    }
                }
                else
                {
                    Site masterMsj = (Site)Page.Master;

                    if (masterMsj != null && (masterMsj.Adscripcion != null && masterMsj.Adscripcion.UnidadOperativa != null))
                        return masterMsj.Adscripcion.UnidadOperativa.Id.Value;
                }
                return null;
            }
            set
            {
                if (value != null)
                    this.hdnUnidadOperativaID.Value = value.Value.ToString();
                else
                    this.hdnUnidadOperativaID.Value = string.Empty;
            }
        }

        #endregion
        #endregion

        #region Constructores
        protected void Page_Load(object sender, EventArgs e)
        {
            this.presentador = new ucActaNacimientoPRE(this);
        }
        #endregion

        #region Métodos
        public void ActualizarOdometros()
        {
            this.grdOdometros.DataSource = this.Odometros;
            this.grdOdometros.DataBind();
        }
        public void ActualizarHorometros()
        {
            this.grdHorometros.DataSource = this.Horometros;
            this.grdHorometros.DataBind();
        }
        public void ActualizarLlantas()
        {
            this.grdLlantas.DataSource = this.Llantas;
            this.grdLlantas.DataBind();
        }
        public void ActualizarEquiposAliados()
        {
            this.grdEquiposAliados.DataSource = this.EquiposAliados;
            this.grdEquiposAliados.DataBind();
        }
        

        public void ActualizarNumerosSerie() {

            this.grdNumerosSerie.DataSource = this.NumerosSerie;
            this.grdNumerosSerie.DataBind();
        }
        
        public void LimpiarSesion()
        {
            if (Session["ListaHorometrosOriginal"] != null)
                Session.Remove("ListaHorometrosOriginal");
            if (Session["ListaOdometrosOriginal"] != null)
                Session.Remove("ListaOdometrosOriginal");
            if (Session["ListaLlantasOriginal"] != null)
                Session.Remove("ListaLlantasOriginal");
            if (Session["ListaEquiposAliadosOriginal"] != null)
                Session.Remove("ListaEquiposAliadosOriginal");
            
            if (Session["ListaNumerosSerie"] != null)
                Session.Remove("ListaNumerosSerie");
            
        }

        #region REQ 13285 Métodos relacionado con las acciones dependiendo de la unidad operativa.

        /// <summary>
        /// Prepara los controles (etiquetas y visualización) que serán válidos para la unidad operativa a la cual accedió el usuario.
        /// </summary>
        /// <param name="tipoEmpresa">Indica la unidad operativa, este valor determina el comportamiento de los controles.</param>
        /// <param name="valoresTabs">Indica el identificador de los Tabs, los cuales serán ocultos.</param>
        public void EstablecerAcciones(ETipoEmpresa tipoEmpresa, string valoresTabs)
        {

            //obteniendo configuración de los controles.
            string VIN = ObtenerConfiguracionResource("RE01", tipoEmpresa, true);
            string Leader = ObtenerConfiguracionResource("RE03", tipoEmpresa, true);
            string Economico = ObtenerConfiguracionResource("RE04", tipoEmpresa, true);
            string AreaDepartamento = ObtenerConfiguracionResource("RE34", tipoEmpresa, true);
            string DatosTecnicos = string.Empty;
            string NumerosSerie = string.Empty;
            string Llantas = string.Empty;
            string EquiposAliados = string.Empty;

            //Se válida si la variable "VIN" está vacía, si es el caso se oculta el control, en caso contrario el valor será asignado a la etiqueta lblVIN
            if (string.IsNullOrEmpty(VIN))
                this.divVIN.Visible = false;
            else
                this.lblVIN.Text = VIN;

            //Se válida si la variable "Leader" está vacía, si es el caso se oculta el control, en caso contrario el valor será asignado a la etiqueta lblVIN
            if (string.IsNullOrEmpty(Leader))
                this.divLeader.Visible = false;
            else
                this.lblLeader.Text = Leader;

            //Se válida si la variable "Economico" está vacía,  si es el caso se oculta el control, en caso contrario el valor será asignado a la etiqueta lblEconomico
            if (string.IsNullOrEmpty(Economico))
                this.divEconomico.Visible = false;
            else
                this.lblEconomico.Text = Economico;

            //Se válida si la variable AreaDepartamento está vacía,  si es el caso se oculta el control, en caso contrario el valor será asignado a la etiqueta lblAreaDepartamento
            if (string.IsNullOrEmpty(AreaDepartamento))
                this.divAreaDepartamento.Visible = false;
            else
                this.lblAreaDepartamento.Text = AreaDepartamento;

            //Se válida la visibilidad de la sección "Datos Técnicos"
            if (!valoresTabs.Contains("1"))
            {
                DatosTecnicos = ObtenerConfiguracionResource("RE28", tipoEmpresa, false);
                if (string.IsNullOrEmpty(DatosTecnicos) || DatosTecnicos == "0")
                    this.divDatosTecnicos.Visible = false;
            }
            else
                this.divDatosTecnicos.Visible = false;
                  
            //Se válida la visibilidad de la sección "Números Serie"
            if (!valoresTabs.Contains("2"))
            {
                NumerosSerie = ObtenerConfiguracionResource("RE29", tipoEmpresa, false);
                if (string.IsNullOrEmpty(NumerosSerie) || NumerosSerie == "0")
                {
                    this.divNumerosSerie.Visible = false;
                    this.divNumerosSerieAdicional.Visible = false;
                }
            }
            else
            {
                this.divNumerosSerie.Visible = false;
                this.divNumerosSerieAdicional.Visible = false;
            }

            //Se válida la visibilidad de la sección "Llantas"
            if (!valoresTabs.Contains("3"))
            {
                Llantas = ObtenerConfiguracionResource("RE30", tipoEmpresa, false); 
                if (string.IsNullOrEmpty(Llantas) || Llantas == "0")
                    this.divLlantas.Visible = false;
            }
            else
                this.divLlantas.Visible = false;

            //Se válida la visibilidad de la sección "Equipos Aliados"
            if (!valoresTabs.Contains("4"))
            {
                EquiposAliados = ObtenerConfiguracionResource("RE31", tipoEmpresa, false);
                if (string.IsNullOrEmpty(EquiposAliados) || EquiposAliados == "0")
                    this.divEquiposAliados.Visible = false;
            }
            else
                this.divEquiposAliados.Visible = false;

        }

        /// <summary>
        /// Método que obtiene la configuración de una etiqueta desde el archivo resource correspondiente a la unidad operativa a la cual accedió el usuario.
        /// </summary>
        /// <param name="etiquetaBuscar">Nombre de la etiqueta que será buscada en el archivo resource</param>
        /// <param name="tipoEmpresa">Nombre de la unidad operativa a la cual accedió el usuario.</param>
        /// <param name="esEtiqueta">Indica sí el valor a obtener es una etiqueta, en caso contrario se considera un TAB o CHECKBOX.</param>
        /// <returns>Retorna la configuración correspondiente al valor recibido en el parámetro etiquetaBuscar del archivo resource.</returns>
        private string ObtenerConfiguracionResource(string etiquetaBuscar, ETipoEmpresa tipoEmpresa, bool esEtiqueta)
        {
            string Configuracion = string.Empty;
            //Instanciamos la clase o webmethod que obtiene las etiquetas
            ObtenerEtiquetaEmpresas obtenerEtiqueta = new ObtenerEtiquetaEmpresas();
            string ConfiguracionObtenida = string.Empty;
            EtiquetaObtenida request = null;

            ConfiguracionObtenida = obtenerEtiqueta.ObtenerEtiquetadelResource(etiquetaBuscar, esEtiqueta?(int)tipoEmpresa:(int)this.UnidadOperativaID);
            request = JsonConvert.DeserializeObject<EtiquetaObtenida>(ConfiguracionObtenida);
            if (string.IsNullOrEmpty(request.cMensaje))
            {
                ConfiguracionObtenida = request.cEtiqueta;
                if (esEtiqueta)
                { 
                    if (ConfiguracionObtenida != "NO APLICA")
                    {
                        Configuracion = ConfiguracionObtenida;
                    }
                }
                else
                {
                    Configuracion = ConfiguracionObtenida;
                }
            }
            return Configuracion;
        }

        #endregion
        #endregion
    }
}