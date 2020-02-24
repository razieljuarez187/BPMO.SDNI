//Esta clase satisface al CU085 - CATALOGO INFORMACION SEGURO DE LA UNIDAD
using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using BPMO.Primitivos.Enumeradores;
using BPMO.SDNI.MapaSitio.UI;
using BPMO.SDNI.Tramites.PRE;
using BPMO.SDNI.Tramites.VIS;
using BPMO.SDNI.Tramites.BO;

namespace BPMO.SDNI.Tramites.UI
{
    public partial class ucSeguroUI : System.Web.UI.UserControl, IucSeguroVIS
    {
        #region Atributos
        private ucSeguroPRE presentador;
        private string nombreClase = "ucSeguroUI";
        private bool? activo;
        #endregion

        #region Propiedades
        public int? UnidadOperativaId
        {
            get 
            { 
                int? id = null;
                Site master = (Site) Page.Master;
                if(master!= null && master.Adscripcion != null && master.Adscripcion.UnidadOperativa != null)
                id = master.Adscripcion.UnidadOperativa.Id;
                return id;
            }
        }

        public int? UsuarioId
        {
            get { return this.UC; }
        }

        public string VIN
        {
            get 
            {
                if (!string.IsNullOrEmpty(this.txtVIN.Text) && !string.IsNullOrWhiteSpace(this.txtVIN.Text))
                    return this.txtVIN.Text.Trim().ToUpper();
                else
                    return string.Empty;
            }
            set
            {
                if (value != null)
                    this.txtVIN.Text = value.ToString();
                else
                    this.txtVIN.Text = string.Empty;
            }
        }
        public string Modelo 
        {
            get 
            {
                if (!string.IsNullOrEmpty(this.txtModelo.Text) && !string.IsNullOrWhiteSpace(this.txtModelo.Text))
                    return this.txtModelo.Text.Trim().ToUpper();
                else
                    return string.Empty;
            }
            set
            {
                if (value != null)
                    this.txtModelo.Text = value;
                else
                    this.txtModelo.Text = string.Empty;
            }
        }
        public string NumeroPoliza 
        { 
            get
            {
                if (!string.IsNullOrEmpty(this.txtNumPoliza.Text) && !string.IsNullOrWhiteSpace(this.txtNumPoliza.Text))
                    return this.txtNumPoliza.Text.Trim().ToUpper();
                else
                    return string.Empty;
            }
            set
            {
                if (value != null)
                    this.txtNumPoliza.Text = value;
                else
                    this.txtNumPoliza.Text = string.Empty;
            }
        }
        public string Aseguradora 
        {
            get
            {
                if (!string.IsNullOrEmpty(this.txtAseguradora.Text) && !string.IsNullOrWhiteSpace(this.txtAseguradora.Text))
                    return this.txtAseguradora.Text.Trim().ToUpper();
                else
                    return string.Empty;
            }
            set
            {
                if (value != null)
                    this.txtAseguradora.Text = value;
                else
                    this.txtAseguradora.Text = string.Empty;
            }
        }
        public string Contacto 
        {
            get
            {
                if (!string.IsNullOrEmpty(this.txtContacto.Text) && !string.IsNullOrWhiteSpace(this.txtContacto.Text))
                    return this.txtContacto.Text.Trim().ToUpper();
                else
                    return string.Empty;
            }
            set
            {
                if (value != null)
                    this.txtContacto.Text = value;
                else
                    this.txtContacto.Text = string.Empty;
            }
        }
        public decimal? PrimaAnual 
        {
            get
            {
                decimal prima;
                if (!string.IsNullOrEmpty(this.txtPrimaAnual.Text) && !string.IsNullOrWhiteSpace(this.txtPrimaAnual.Text))
                    if (Decimal.TryParse(this.txtPrimaAnual.Text.Trim().Replace(",",""), out prima)) //RI0012
                        return prima;
                   
                return null;
            }
            set
            {
                if (value != null)
                {
					this.txtPrimaAnual.Text = string.Format("{0:#,##0.0000}", value); //RI0012
                    if (presentador == null)
                        this.presentador = new ucSeguroPRE(this, this.ucucDeducibleSeguroUI, this.ucucEndosoSeguroUI, this.ucucSiniestroSeguroUI);
                    this.presentador.PrimaAnual();
                }
                else
                    this.txtPrimaAnual.Text = string.Empty;
            }
        }
        public decimal? PrimaSemestral 
        {
            get
            {
                decimal prima;
                if (!string.IsNullOrEmpty(this.txtPrimaSemestral.Text) && !string.IsNullOrWhiteSpace(this.txtPrimaSemestral.Text))
                    if (Decimal.TryParse(this.txtPrimaSemestral.Text.Trim().Replace(",",""), out prima)) //RI0012
                        return prima;
               
                return null;
            }
            set
            {
				if (value != null)
					this.txtPrimaSemestral.Text = string.Format("{0:#,##0.0000}", value); //RI0012
				else
					this.txtPrimaSemestral.Text = string.Empty;
            }
        }
        public DateTime? VigenciaInicial 
        {
            get
            {
                DateTime date;
                if (!string.IsNullOrEmpty(this.txtVigenciaInicial.Text) && !string.IsNullOrWhiteSpace(this.txtVigenciaInicial.Text))
                    if(DateTime.TryParse(this.txtVigenciaInicial.Text.Trim().ToUpper(), out date))
                    return date;

                return null;
            }
            set
            {
                if (value != null)
                    this.txtVigenciaInicial.Text = value.Value.ToShortDateString();                    
                else
                    this.txtVigenciaInicial.Text = string.Empty;
            }
        }
        public DateTime? VigenciaFinal 
        {
            get
            {
                DateTime date;
                if (!string.IsNullOrEmpty(this.txtVigenciaFinal.Text) && !string.IsNullOrWhiteSpace(this.txtVigenciaFinal.Text))
                    if (DateTime.TryParse(this.txtVigenciaFinal.Text, out date))
                        return date;

                return null;
            }
            set
            {
                if (value != null)
                    this.txtVigenciaFinal.Text = value.Value.ToShortDateString();
                else
                    this.txtVigenciaFinal.Text = string.Empty;
            }
        }
        public string Observaciones 
        {
            get
            {
                if (!string.IsNullOrEmpty(this.txtObservacion.Text) && !string.IsNullOrWhiteSpace(this.txtObservacion.Text))
                    return this.txtObservacion.Text.Trim().ToUpper();
                else
                    return string.Empty;
            }
            set
            {
                if (value != null)
                    this.txtObservacion.Text = value;
                else
                    this.txtObservacion.Text = string.Empty;
            }
        }
        public bool? Activo
        {
            get
            {
                return this.activo;
            }
            set
            {
                this.activo = value;
            }
        }
        public DateTime? FC
        {
            get { return DateTime.Today; }
        }
        public DateTime? FUA
        {
            get { return this.FC; }
        }
        public int? UC
        {
            get
            {
                int? id = null;
                Site masterMsj = (Site)Page.Master;

                if (masterMsj.Usuario != null)
                    id = masterMsj.Usuario.Id;
                return id;
            }
        }
        public int? UUA
        {
            get { return this.UC; }
        }
        public int? TramiteID 
        {
            get
            {
                int val;
                if (!string.IsNullOrEmpty(this.hdnTramiteID.Value) && !string.IsNullOrWhiteSpace(this.hdnTramiteID.Value))
                    if (Int32.TryParse(this.hdnTramiteID.Value.Trim().ToUpper(), out val))
                        return val;

                return null;
            }
            set
            {
                if (value != null)
                    this.hdnTramiteID.Value = value.ToString();
                else
                    this.hdnTramiteID.Value = string.Empty;
            } 
        }
        public ETipoTramite? TipoTramite
        {
            get { return ETipoTramite.SEGURO; }
        }
        public int? TramitableID 
        {
            get
            {
                int val;
                if (!string.IsNullOrEmpty(this.hdnTramitableID.Value) && !string.IsNullOrWhiteSpace(this.hdnTramitableID.Value))
                    if (Int32.TryParse(this.hdnTramitableID.Value.Trim().ToUpper(), out val))
                    return val;
                return null;
            }
            set
            {
                if (value != null)
                    this.hdnTramitableID.Value = value.ToString();
                else
                    this.hdnTramitableID.Value = string.Empty;
            } 
        }
        public ETipoTramitable? TipoTramitable 
        {
            get
            {
                int val;
                if (!string.IsNullOrEmpty(this.hdnTipoTramitable.Value) && !string.IsNullOrWhiteSpace(this.hdnTipoTramitable.Value))
                    if (Int32.TryParse(this.hdnTipoTramitable.Value.Trim().ToUpper(), out val))
                    return (ETipoTramitable)val;
                return null;
            }
            set
            {
                if (value != null)
                    this.hdnTipoTramitable.Value = ((int)value).ToString();
                else
                    this.hdnTipoTramitable.Value = string.Empty;
            } 
        }
        public SeguroBO UltimoObjeto
        {
            get
            {
                if ((SeguroBO)Session["LastSeguro"] == null)
                    return new SeguroBO();
                else
                    return (SeguroBO)Session["LastSeguro"];
            }
            set
            {
                Session["LastSeguro"] = value;
            }
        }
        public int? Modo
        {
            get 
            {
                int val = 0;
                if (!string.IsNullOrEmpty(this.hdnModo.Value) && !string.IsNullOrWhiteSpace(this.hdnModo.Value))
                    if (Int32.TryParse(this.hdnModo.Value, out val))
                        return val;
                return null;
            }
            set 
            {
                if (value != null)
                    this.hdnModo.Value = value.ToString();
                else this.hdnModo.Value = string.Empty;
            }
        }
        #endregion

        #region Constructores
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
            this.presentador = new ucSeguroPRE(this, this.ucucDeducibleSeguroUI, this.ucucEndosoSeguroUI, this.ucucSiniestroSeguroUI);

            if (!this.IsPostBack)
            {
                    if (this.Modo.HasValue)
                    {
                        if (this.Modo.Value == 1)
                            this.presentador.PrepararEdicion();
                        else
                            this.presentador.PrepararNuevo();
                    }
                    else
                        this.presentador.PrepararNuevo();
                }
                else
                {
                    presentador.PrimaAnual();
                }
                ucucSiniestroSeguroUI.Editar_Siniestro = this.EditarSiniestro_Click;
                ucucSiniestroSeguroUI.Siniestro_Editado = this.SiniestroEditado_Click;
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencias al presentar la información", ETipoMensajeIU.ERROR, nombreClase + ".Page_Load: " + ex.Message);
            }
        }
        #endregion

        #region Métodos
        public void PrepararNuevo()
        {         
            this.hdnTramiteID.Value = string.Empty;
            this.hdnTipoTramitable.Value = string.Empty;
            this.hdnTramitableID.Value = string.Empty;
            this.txtAseguradora.Text = string.Empty;
            this.txtContacto.Text = string.Empty;
            this.txtNumPoliza.Text = string.Empty;
            this.txtObservacion.Text = string.Empty;
            this.txtPrimaAnual.Text = string.Empty;
            this.txtPrimaSemestral.Text = string.Empty;
            this.txtVigenciaFinal.Text = string.Empty;
            this.txtVigenciaInicial.Text = string.Empty;

            this.txtAseguradora.Enabled = true;
            this.txtContacto.Enabled = true;            
            this.txtNumPoliza.Enabled = true;
            this.txtObservacion.Enabled = true;
            this.txtPrimaAnual.Enabled = true;
            this.txtPrimaSemestral.Enabled = true;
            this.txtVigenciaFinal.Enabled = true;
            this.txtVigenciaInicial.Enabled = true;
            this.txtVIN.Enabled = false;
            this.txtModelo.Enabled = false;
        }
        public void PrepararVista()
        {
            this.txtAseguradora.Enabled = false;
            this.txtContacto.Enabled = false;
            this.txtModelo.Enabled = false;
            this.txtNumPoliza.Enabled = false;
            this.txtObservacion.Enabled = false;
            this.txtPrimaAnual.Enabled = false;
            this.txtPrimaSemestral.Enabled = false;
            this.txtVigenciaFinal.Enabled = false;
            this.txtVigenciaInicial.Enabled = false;
            this.txtVIN.Enabled = false;
        }
        public void PrepararEdicion()
        {
            this.txtAseguradora.Enabled = true;
            this.txtContacto.Enabled = true;
            this.txtModelo.Enabled = false;
            this.txtNumPoliza.Enabled = true;
            this.txtObservacion.Enabled = false;
            this.txtPrimaAnual.Enabled = true;
            this.txtPrimaSemestral.Enabled = true;
            this.txtVigenciaFinal.Enabled = true;
            this.txtVigenciaInicial.Enabled = true;
            this.txtVIN.Enabled = false;
            this.txtObservacion.Enabled = true;
        }
        #region SC0004
        public void ModoRegistrar()
        {
            this.ucucSiniestroSeguroUI.ColumnasModoRegistrar();
        }
        public void ModoEditar()
        {
            this.ucucSiniestroSeguroUI.ReestablecerColumnas();
        }
        #endregion
        public void CambiarASoloLectura()
        {
            this.txtModelo.ReadOnly = true;
            this.txtVIN.ReadOnly = true;
        }
        public void Registrar()
        {
            this.presentador.Registrar();
        }
        public void Editar()
        {
            this.presentador.Editar();
        }

        public object ObtenerDatosNavegacion()
        {
            if (((SeguroBO)Session["REGISTRARSEGURO"]) != null)
                return ((SeguroBO)Session["REGISTRARSEGURO"]);
            return null;
        }
        public object ObtenerDatosNavegacion(string key)
        {
            if (((SeguroBO)Session[key]) != null)
                return ((SeguroBO)Session[key]);
            return null;
        }

        public void EstablecerPaqueteNavegacion(string Clave, int? tramiteID)
        {
            SeguroBO seguro = (SeguroBO)this.presentador.InterfazUsuarioADato();

            if (seguro != null) Session[Clave] = seguro;
            else
            {
                throw new Exception(this.nombreClase + ".EstablecerPaqueteNavegacion: El seguro proporcionado no pertence al listado de seguros encontrados.");
            }
            if (Clave.CompareTo("TramiteSeguro") == 0)
                Response.Redirect("DetalleSeguroUI.aspx", false);
            else
                Response.Redirect("EditarSeguroUI.aspx", false);

        }

        public void CargarObjeto(string key)
        {
            this.presentador.CargarObjeto(key);
        }
        public void CargarObjeto()
        {
            this.presentador.CargarObjeto();
        }

        public void IrADetalle()
        {
            int? tramiteID = 0;
            presentador.IrADetalle(tramiteID);
        }

        public void LimpiarSesion()
        {
            if (Session["Deducibles"] != null)
                Session.Remove("Deducibles");
            if (Session["Endosos"] != null)
                Session.Remove("Endosos");
            if (Session["Siniestros"] != null)
                Session.Remove("Siniestros");
            if (Session["Seguros"] != null)
                Session.Remove("Seguros");
        }

        /// <summary>
        /// Desplegar mensaje de Error con detalle
        /// </summary>
        /// <param name="mensaje">Descripción del mensaje</param>
        /// <param name="tipo">EMensaje tipo de mensaje a desplegar</param>
        /// <param name="detalle">Detalle del mensaje a desplegar</param>
        public void MostrarMensaje(string mensaje, ETipoMensajeIU tipo, string msjDetalle = null)
        {
            if (tipo.Equals(ETipoMensajeIU.CONFIRMACION))
            {
                ((HiddenField)this.Parent.FindControl("hdnTipoMensaje")).Value = ((int)tipo).ToString();
                ((HiddenField)this.Parent.FindControl("hdnMensaje")).Value = mensaje;
            }
            else
            {
                Site masterMsj = (Site)this.Parent.Page.Master;
                if (tipo == ETipoMensajeIU.ERROR)
                    masterMsj.MostrarMensaje(mensaje, tipo, msjDetalle);
                else
                    masterMsj.MostrarMensaje(mensaje, tipo);
            }
        }
        #endregion

        #region Eventos
        #region SC0004
        protected void EditarSiniestro_Click(object sender, CommandEventArgs e)
        {
            this.PrepararVista();
            this.ucucDeducibleSeguroUI.DeshabilitarControles();
            this.ucucEndosoSeguroUI.DeshabilitarControles();
        }
        protected void SiniestroEditado_Click(object sender, CommandEventArgs e)
        {
            this.PrepararEdicion();
            this.ucucDeducibleSeguroUI.HabilitarControles();
            this.ucucEndosoSeguroUI.HabilitarControles();
        }
        #endregion
        #endregion
    }
}