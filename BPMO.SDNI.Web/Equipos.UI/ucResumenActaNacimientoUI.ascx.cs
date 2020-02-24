//Satisface al CU077 - Registrar Acta de Nacimiento de una Unidad
//Satisface al CU079 - Consultar Acta de Nacimiento de Unidad
//Satisface la solicitud de cambio SC0006

using System;
using System.Web.UI.WebControls;
using BPMO.Primitivos.Enumeradores;
using BPMO.SDNI.Equipos.BO;
using BPMO.SDNI.Equipos.PRE;
using BPMO.SDNI.Equipos.VIS;
using BPMO.SDNI.MapaSitio.UI;
using System.Collections.Generic;
using BPMO.SDNI.Comun.UI;
using BPMO.SDNI.Comun.PRE;
using BPMO.Basicos.BO;
using Newtonsoft.Json;
using DevExpress.XtraPrinting.Native;
using System.Web.UI;

namespace BPMO.SDNI.Equipos.UI
{
	public partial class ucResumenActaNacimientoUI : System.Web.UI.UserControl, IucResumenActaNacimientoVIS
    {
        #region Atributos
        private ucResumenActaNacimientoPRE presentador;

        //REQ 14150, variable global para instanciar las etiquetas que se obtendrán a través del archivo de recursos.
        private ObtenerEtiquetaEmpresas obtenerEtiqueta = null;
        private string nombreClase = "ucResumenActaNacimientoUI";
        #endregion

        #region Propiedades
        public int? EquipoID
        {
            get
            {
                int? id = null;
                if (!String.IsNullOrEmpty(this.hdnEquipoID.Value))
                    id = int.Parse(this.hdnEquipoID.Value.Trim());
                return id;
            }
            set
            {
                if (value != null)
                    this.hdnEquipoID.Value = value.ToString();
                else
                    this.hdnEquipoID.Value = string.Empty;
            }
        }
        public int? UnidadID
        {
            get
            {
                int? id = null;
                if (!String.IsNullOrEmpty(this.hdnUnidadID.Value))
                    id = int.Parse(this.hdnUnidadID.Value.Trim());
                return id;
            }
            set
            {
                if (value != null)
                    this.hdnUnidadID.Value = value.ToString();
                else
                    this.hdnUnidadID.Value = string.Empty;
            }
        }

        public string NumeroSerie
        {
            set
            {
                if (value != null)
                    this.txtVIN.Text = value;
                else
                    this.txtVIN.Text = string.Empty;
            }
        }
        public string SucursalNombre
        {
            set
            {
                if (value != null)
                    this.txtSucursal.Text = value;
                else
                    this.txtSucursal.Text = string.Empty;
            }
        }
        public EEstatusUnidad? EstatusUnidad
        {
            get
            {
                EEstatusUnidad? area = null;
                if (this.hdnEstatus.Value.Trim().CompareTo("") != 0)
                    area = (EEstatusUnidad)Enum.Parse(typeof(EEstatusUnidad), this.hdnEstatus.Value);
                return area;
            }
            set
            {
                if (value == null)
                {
                    this.txtEstatus.Text = "";
                    this.hdnEstatus.Value = "";
                }
                else
                {
                    this.txtEstatus.Text = value.ToString();
                    this.hdnEstatus.Value = ((int)value).ToString();
                }
            }
        }
        public string Area
        {
            set
            {
                if (string.IsNullOrEmpty(value))
                    this.txtAreaDepartamento.Text = string.Empty;
                else
                    this.txtAreaDepartamento.Text = value.ToString();
            }
        }

        public DateTime? FC
        {
            set
            {
                if (value != null)
                    this.txtFechaRegistro.Text = value.Value.ToString();
                else
                    this.txtFechaRegistro.Text = string.Empty;
            }
        }
        public DateTime? FUA
        {
            set
            {
                if (value != null)
                    this.txtFechaModificacion.Text = value.Value.ToString();
                else
                    this.txtFechaModificacion.Text = string.Empty;
            }
        }
        public int? UC
        {
            get
            {
                int? id = null;
                if (!String.IsNullOrEmpty(this.hdnUC.Value))
                    id = int.Parse(this.hdnUC.Value.Trim());

                return id;
            }
            set
            {
                if (value != null)
                    this.hdnUC.Value = value.ToString();
                else
                    this.hdnUC.Value = string.Empty;
            }
        }
        public int? UUA
        {
            get
            {
                int? id = null;
                if (!String.IsNullOrEmpty(this.hdnUUA.Value))
                    id = int.Parse(this.hdnUUA.Value.Trim());
                return id;
            }
            set
            {
                if (value != null)
                    this.hdnUUA.Value = value.ToString();
                else
                    this.hdnUUA.Value = string.Empty;
            }
        }
        public string UsuarioCreacion
        {
            set
            {
                if (value != null)
                    this.txtUsuarioRegistro.Text = value;
                else
                    this.txtUsuarioRegistro.Text = string.Empty;
            }
        }
        public string UsuarioModificacion
        {
            set
            {
                if (value != null)
                    this.txtUsuarioModificacion.Text = value;
                else
                    this.txtUsuarioModificacion.Text = string.Empty;
            }
        }
        #region RQM 14150
        public AdscripcionBO Adscripcion
        {
            get
            {
                return this.Session["Adscripcion"] != null ? (AdscripcionBO)this.Session["Adscripcion"] : null;
            }
            set
            {
                this.Session["Adscripcion"] = value;
            }
        }
        #endregion
        #endregion

        #region Constructores
        protected void Page_Load(object sender, EventArgs e)
		{
			presentador = new ucResumenActaNacimientoPRE(this);
		}
        #endregion

        #region Métodos
        public void PrepararNuevo()
        {
            this.hdnEstatus.Value = "";
            this.hdnUC.Value = "";
            this.hdnUUA.Value = "";

            this.txtAreaDepartamento.Text = "";
            this.txtEstatus.Text = "";
            this.txtFechaRegistro.Text = "";
            this.txtSucursal.Text = "";
            this.txtUsuarioRegistro.Text = "";
            this.txtVIN.Text = "";
            this.txtUsuarioModificacion.Text = "";
            this.txtFechaModificacion.Text = "";

            this.txtAreaDepartamento.Enabled = false;
            this.txtEstatus.Enabled = false;
            this.txtFechaRegistro.Enabled = false;
            this.txtSucursal.Enabled = false;
            this.txtUsuarioRegistro.Enabled = false;
            this.txtVIN.Enabled = false;
            this.txtFechaModificacion.Enabled = false;
            this.txtUsuarioModificacion.Enabled = false;
        }
        public void PrepararVisualizacion()
        {
            this.txtAreaDepartamento.Enabled = false;
            this.txtEstatus.Enabled = false;
            this.txtFechaModificacion.Enabled = false;
            this.txtFechaRegistro.Enabled = false;
            this.txtSucursal.Enabled = false;
            this.txtUsuarioModificacion.Enabled = false;
            this.txtUsuarioRegistro.Enabled = false;
            this.txtVIN.Enabled = false;
        }

        public void MostrarDatosRegistro(bool mostrar)
        {
            this.trRegistroFC.Visible = mostrar;
            this.trRegistroUC.Visible = mostrar;
        }
        public void MostrarDatosActualizacion(bool mostrar)
        {
            this.trActualizacionFUA.Visible = mostrar;
            this.trActualziacionUUA.Visible = mostrar;
        }

        public void PermitirConfigurarMantenimientos(bool permitir)
        {
            this.btnConfiguracionMantenimientos.Enabled = permitir;
        }
        public void PermitirRegistrarMantenimientos(bool permitir)
        {
            this.btnRegistrarMantenimientos.Enabled = permitir;
        }

        public void MostrarDatosSiniestro(List<SiniestroUnidadBO> historialSiniestro)
        {
            if (historialSiniestro != null && historialSiniestro.Count > 0)
            {
                this.divSiniestros.Visible = true;
                this.gvHistoricoSiniestros.DataSource = historialSiniestro;
                this.gvHistoricoSiniestros.DataBind();
            }
            else
                this.divSiniestros.Visible = false;
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

        #region RQM 14150

        /// <summary>
        /// Método implementado desde la vista IucResumenActaNacimientoVIS, que se encarga de buscar dentro del archivo de recursos las etiquetas y 
        /// configuraciones de cada unidad operativa.
        /// </summary>
        public void EstablecerAcciones()
        {
           
            #region EtiquetasPrincipales

            //Instanciamos la clase o web method que obtiene las etiquetas
            obtenerEtiqueta = new ObtenerEtiquetaEmpresas();

            string VIN              = string.Empty;
            string AreaDepartamento = string.Empty;

            //Obteniendo el nombre de las etiquetas del archivo resource correspondiente.
            VIN              = ConfigurarEtiquetaPrincipal("RE01");
            AreaDepartamento = ConfigurarEtiquetaPrincipal("RE34");

            if (!string.IsNullOrEmpty(VIN))
                this.lblVIN.Text = VIN;

            if (!string.IsNullOrEmpty(AreaDepartamento))
                this.lblAreaDepartamento.Text = AreaDepartamento;

            #endregion
        }

        /// <summary>
        /// Método implementado desde la vista IucResumenActaNacimientoVIS,
        /// que se encarga buscar dentro del archivo de recursos las etiquetas de la pagina principal
        /// </summary>
        /// <param name="cEtiquetaBuscar">Valor de la etiqueta a buscar para cambiar el nombre</param>
        /// <returns>Regresa el valor de la etiqueta por empresa</returns>
        public string ConfigurarEtiquetaPrincipal(string cEtiquetaBuscar)
        {
            string cEtiquetaObtenida = string.Empty;
            EtiquetaObtenida request = null;
            string valorEtiqueta = string.Empty;

            cEtiquetaObtenida = obtenerEtiqueta.ObtenerEtiquetadelResource(cEtiquetaBuscar, this.Adscripcion.UnidadOperativa.Id.GetValueOrDefault());
            request = JsonConvert.DeserializeObject<EtiquetaObtenida>(cEtiquetaObtenida);

            if (request.cMensaje.IsEmpty())
            {
                valorEtiqueta = request.cEtiqueta;
            }
            else
            {
                this.MostrarMensaje("Inconsistencia al buscar la etiqueta en el archivo de recursos", ETipoMensajeIU.ERROR, nombreClase + "ConfigurarTab" + request.cMensaje);
            }
            return valorEtiqueta;
        }
        
        #endregion

      #endregion

        protected void btnConfiguracionMantenimientos_Click(object sender, EventArgs e)
        {
            this.Response.Redirect(this.ResolveUrl("~/Mantenimiento.UI/ConsultarConfiguracionParametroMantenimientoUI.aspx"));
        }

        protected void btnRegistrarMantenimientos_Click(object sender, EventArgs e)
        {
            this.Response.Redirect(this.ResolveUrl("~/Mantenimiento.UI/ProgramarMantenimientosUI.aspx"));
        }
        
    }
}