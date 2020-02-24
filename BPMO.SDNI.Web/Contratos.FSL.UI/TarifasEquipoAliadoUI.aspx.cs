// Satisface al Caso de uso CU015 - Registrar Contrato Full Service Leasing
// Satisface al Caso de uso CU022 - Consultar Contrato Full Service Leasing
// Satisface al Caso de uso CU023 - Editar Contrato Full Service Leasing
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Web.UI;
using System.Web.UI.WebControls;
using BPMO.Basicos.BO;
using BPMO.Primitivos.Enumeradores;
using BPMO.SDNI.Contratos.FSL.BO;
using BPMO.SDNI.Contratos.FSL.PRE;
using BPMO.SDNI.Contratos.FSL.VIS;

namespace BPMO.SDNI.Contratos.FSL.UI
{
    public partial class TarifasEquipoAliadoUI : Page, ITarifasEquipoAliadoVIS
    {
        #region Atributos
        private TarifasEquipoAliadoPRE presentador;
        private const string NombreClase = "TarifasEquipoAliadoUI";
        #endregion

        #region Propiedades
        /// <summary>
        /// Plazo del Contrato en Años
        /// </summary>
        public int? Plazo
        {
            get
            {
                int? value = null;

                if (!string.IsNullOrEmpty(Request.QueryString["Plazo"]))
                    value = int.Parse(Request.QueryString["Plazo"]);

                return value;
            }
        }
        /// <summary>
        /// Tipo de Cotizacion para las tarifas de los equipos aliados
        /// </summary>
        public ETipoCotizacion? TipoCotizacion
        {
            get
            {
                if (!string.IsNullOrEmpty(Request.QueryString["TipoCotizacion"]))
                    return (ETipoCotizacion)int.Parse(Request.QueryString["TipoCotizacion"]);

                return null;
            }
        }
        /// <summary>
        /// Identificador del Equipo Aliado
        /// </summary>
        public int? EquipoAliadoID
        {
            get
            {
                int? value = null;

                if (!string.IsNullOrEmpty(Request.QueryString["EquipoAliadoID"]))
                    value = int.Parse(Request.QueryString["EquipoAliadoID"]);

                return value;
            }
        }
        /// <summary>
        /// Identificador de Equipo del Equipo Aliado
        /// </summary>
        public int? EquipoID
        {
            get
            {
                int? value = null;

                if (!string.IsNullOrEmpty(Request.QueryString["EquipoID"]))
                    value = int.Parse(Request.QueryString["EquipoID"]);

                return value;
            }
        }
        /// <summary>
        /// Identificador de la unidad al que pertenece el equipo aliado
        /// </summary>
        public int? UnidadID
        {
            get
            {
                int? value = null;

                if (!string.IsNullOrEmpty(Request.QueryString["UnidadID"]))
                    value = int.Parse(Request.QueryString["UnidadID"]);

                return value;
            }
        }
        /// <summary>
        /// Identificador de Unidad Operativa
        /// </summary>
        public int? UnidadOperativaID
        {
            get
            {
                int? value = null;

                if (!string.IsNullOrEmpty(Request.QueryString["UnidadOperativaID"]))
                    value = int.Parse(Request.QueryString["UnidadOperativaID"]);

                return value;
            }
        }
        /// <summary>
        /// Identificador de la Sucursal
        /// </summary>
        public int? SucursalID
        {
            get
            {
                int? value = null;

                if (!string.IsNullOrEmpty(Request.QueryString["SucursalID"]))
                    value = int.Parse(Request.QueryString["SucursalID"]);

                return value;
            }
        }

        /// <summary>
        /// Identificador de la Sucursal
        /// </summary>
        public string CheckTarifaSelelect {
            get {
                string value = null;

                if (!string.IsNullOrEmpty(Request.QueryString["checkTarifa"]))
                    value = Request.QueryString["checkTarifa"];

                return value;
            }
        }
        /// <summary>
        /// Obtiene el cargo adicional generado
        /// </summary>
        public CargoAdicionalEquipoAliadoBO CargoAdicional
        {
            get
            {
                var cargo = new CargoAdicionalEquipoAliadoBO
                    {
                        CobrableID = CargoAdicionalID,
                        TipoCotizacion = TipoCotizacion,
                        EquipoAliado =
                            new Equipos.BO.EquipoAliadoBO
                                {
                                    EquipoAliadoID = EquipoAliadoID,
                                    EquipoID = EquipoID,
                                    NumeroSerie = NumeroSerie,
                                    Sucursal = new SucursalBO{ Id = SucursalID }
                                },
                        Tarifas = !cbSinTarifas.Checked ? ucTarifasEquipoAdicional.ObtenerTarifas() : new List<TarifaFSLBO>()
                    };
                return cargo;
            }
        }
        /// <summary>
        /// Establece el Nombre del Modelo del EquipoAliado
        /// </summary>
        public string NombreModelo
        {
            set { lblNombreModelo.Text = value; }
        }
        /// <summary>
        /// Establece el Numero de Serie o Codigo VIN a despleagar
        /// </summary>
        public string NumeroSerie
        {
            get { return lblVIN.Text.ToUpper(); }
            set { lblVIN.Text = value; }
        }
        /// <summary>
        /// Nombre de la variable de seccion que contiene la coleaccion de cargos adicionales de equipos aliados
        /// </summary>
        public string NombreSessiondeColeccion
        {
            get { return Request["Variable"]; }
        }
        /// <summary>
        /// Coleccion de cargos adicionales de equipos aliados
        /// </summary>
        public List<CargoAdicionalEquipoAliadoBO> ListadoCargosAdicionalesEquipoAliado
        {
            get
            {
                List<CargoAdicionalEquipoAliadoBO> listado;

                if (Session[NombreSessiondeColeccion] != null)
                    listado = (List<CargoAdicionalEquipoAliadoBO>)Session[NombreSessiondeColeccion];
                else
                    listado = new List<CargoAdicionalEquipoAliadoBO>();

                return listado;
            }
            set { Session[NombreSessiondeColeccion] = value ?? new List<CargoAdicionalEquipoAliadoBO>(); }
        }
        /// <summary>
        /// Indica si la pagina esta en modo consultar
        /// </summary>
        public bool ModoConsultar
        {
            get { return Request.QueryString["Consultar"] == true.ToString(); }
        }
        /// <summary>
        /// Identificador del CargoAdicional del EquipoAliado
        /// </summary>
        public int? CargoAdicionalID
        {
            get
            {
                if (!string.IsNullOrEmpty(Request.QueryString["CargoAdicionalID"]))
                    return int.Parse(Request.QueryString["CargoAdicionalID"]);
                return null;
            }
        }
        /// <summary>
        /// Regresa el estado del Check que revisa si tiene o no tarifas para el equipo aliado
        /// </summary>
        public bool SinTarifas
        {
            get { return cbSinTarifas.Checked;  }
            set { cbSinTarifas.Checked = value;  }
        }

        #endregion

        #region Metodos
        /// <summary>
        /// Realiza el registro de un script javascript para su ejecucion
        /// </summary>
        /// <param name="key">Clave del script</param>
        /// <param name="script">script a ejecutar</param>
        public void RegistrarScript(string key, string script)
        {
            ScriptManager.RegisterStartupScript(Page, typeof(Page), key, script, true);
        }

        /// <summary>
        /// Cerrar el Dialog
        /// </summary>
        /// <param name="valorRetorno">Valor retorno</param>
        public void CerrarDialog(string valorRetorno) {
            RegistrarScript("closeWindow", "closeParentUI('" + valorRetorno + "')");
        }

        public void EstablecerSinTarifas()
        {
            ucTarifasEquipoAdicional.EstablecerSinTarifas();
            ucTarifasEquipoAdicional.VistaMensaje = this;
        }
        /// <summary>
        /// Inicializa el control
        /// </summary>
        public void Inicializar()
        {
            ucTarifasEquipoAdicional.InicializarControl(Plazo, TipoCotizacion, false);
            ucTarifasEquipoAdicional.VistaMensaje = this;
        }

        public void Inicializar(List<TarifaFSLBO> tarifasEquipoAliado)
        {
            ucTarifasEquipoAdicional.Inicializar(Plazo, TipoCotizacion, ETipoEquipo.EquipoAliado, tarifasEquipoAliado, EquipoID, false);
            ucTarifasEquipoAdicional.VistaMensaje = this;
        }
        /// <summary>
        /// Este método despliega un mensaje en pantalla
        /// </summary>
        /// <param name="mensaje">Mensaje a desplegar</param>
        /// <param name="tipoMensaje">Tipo de mensaje a desplegar</param>
        /// <param name="msjDetalle">Muestra una cadena de texto que contiene los datalles del mensaje a desplegar</param>
        public void MostrarMensaje(string mensaje, ETipoMensajeIU tipoMensaje, string msjDetalle = null)
        {
            string sError = string.Empty;
            if (tipoMensaje == ETipoMensajeIU.ERROR)
            {
                if (this.hdnMensaje == null)
                    sError += " , hdnDetalle";
                this.hdnDetalle.Value = msjDetalle;
            }
            if (this.hdnMensaje == null)
                sError += " , hdnMensaje";
            if (this.hdnTipoMensaje == null)
                sError += " , hdnTipoMensaje";
            if (sError.Length > 0)
                throw new Exception("No se pudo desplegar correctamente el error. No se encontró el control: " + sError.Substring(2) + " en la MasterPage.");

            this.hdnMensaje.Value = mensaje;
            this.hdnTipoMensaje.Value = ((int)tipoMensaje).ToString();
        }
        /// <summary>
        /// Configura la pagina en modo Consultar
        /// </summary>
        public void ConfigurarModoConsultar()
        {
            ucTarifasEquipoAdicional.EstablecerModoConsulta(true);
            btnAgregar.Enabled = false;
            cbSinTarifas.Enabled = false;
        }
        /// <summary>
        /// Configura la pagina en modo Editar
        /// </summary>
        public void ConsultarModoEditar()
        {
            ucTarifasEquipoAdicional.EstablecerModoEditar(ETipoEquipo.EquipoAliado);
            btnAgregar.Enabled = true;
            cbSinTarifas.Enabled = true;
        }

        public void ConfigurarTipoCargoConsultado(bool? cargoPorKm)
        {
            ucTarifasEquipoAdicional.EstablecerCargoPorKmEquipoAliado(cargoPorKm);
        }
        /// <summary>
        /// Despliegas la tarifas del Equipo Aliado
        /// </summary>
        /// <param name="list"></param>
        public void DesplegarTarifas(List<TarifaFSLBO> list)
        {
            ucTarifasEquipoAdicional.LimpiarSesion();
            ucTarifasEquipoAdicional.TipoCotizacion = this.TipoCotizacion;
            if (list.Count < 1)
            {
                ucTarifasEquipoAdicional.SinTarifas = true;
                ucTarifasEquipoAdicional.EstablecerSinTarifas();
                ucTarifasEquipoAdicional.Inicializar(Plazo, TipoCotizacion, ETipoEquipo.EquipoAliado, list, EquipoID, false);
                this.cbSinTarifas.Checked = true;
            }
            else
            {
                ucTarifasEquipoAdicional.SinTarifas = false;
                ucTarifasEquipoAdicional.Inicializar(Plazo, TipoCotizacion, ETipoEquipo.EquipoAliado, list, EquipoID, false);
                if(!ucTarifasEquipoAdicional.EsModoConsulta.Value)
                    ucTarifasEquipoAdicional.EstablecerModoEditar(ETipoEquipo.EquipoAliado);
            }
            ucTarifasEquipoAdicional.VistaMensaje = this;
        }
        #region Seguridad
        private string Logueo
        {
            get { return ConfigurationManager.AppSettings["Logueo"]; }
        }
        /// <summary>
        /// Verifica si el último Post registrado en la página ha superado el timeout de la session
        /// si así fue invoca a finalizar la sesión del usuario
        /// </summary>
        protected void VerificaSession()
        {
            if (Session["UltimoPost"] == null)
            {
                FinalizarSession();
            }
            else
            {
                DateTime ultimoPost = ((DateTime)Session["UltimoPost"]).AddMinutes(Session.Timeout);
                if (DateTime.Compare(ultimoPost, DateTime.Now) < 0)
                {
                    FinalizarSession();
                }
                else
                {
                    Session["UltimoPost"] = DateTime.Now;
                }
            }
        }
        /// <summary>
        /// Finaliza la sesion del usuario
        /// </summary>
        private void FinalizarSession()
        {
            this.Session.RemoveAll();
            this.Response.Redirect(this.Logueo);
        }
        #endregion
        #endregion

        #region Eventos
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                this.VerificaSession();//SC_0008
                presentador = new TarifasEquipoAliadoPRE(this);
                ucTarifasEquipoAdicional.VistaMensaje = this;
                if (!Page.IsPostBack)
                {
                    presentador.Inicializar();
                }
            }
            catch (Exception ex)
            {
                MostrarMensaje("Inconsistencias al presentar la información.", ETipoMensajeIU.ERROR, NombreClase + ".Page_Load :" + ex.Message);
            }
        }
        protected void btnAgregar_Click(object sender, EventArgs e)
        {
            try
            {
                if (ucTarifasEquipoAdicional.ObtenerTarifas().Count == 0 && !cbSinTarifas.Checked)
                {
                    MostrarMensaje("Se deben Configurar Tarifas Para el Equipo Aliado.", ETipoMensajeIU.ADVERTENCIA);
                    return;
                }

                var tarifas = ucTarifasEquipoAdicional.ObtenerTarifas();
                string sError = "";
                foreach (var tarifa in tarifas)
                {
                    if (tarifa.Rangos == null)
                    {
                        sError = "No se ha configurado los rangos de la Tarifa" + (TipoCotizacion == ETipoCotizacion.Average ? " del Equipo Aliado" : "del Año " + tarifa.Año + " del Equipo Alido");
                        break;
                    }
                    if (tarifa.Rangos.Count == 0)
                    {
                        sError = "No se ha configurado los rangos de la Tarifa" + (TipoCotizacion == ETipoCotizacion.Average ? " del Equipo Aliado" : "del Año " + tarifa.Año + " del Equipo Alido");
                        break;
                    }
                }

                if (!String.IsNullOrEmpty(sError))
                {
                    MostrarMensaje(sError, ETipoMensajeIU.ADVERTENCIA);
                    return;
                }

                presentador.AgregarCargoAdicionalEquipoAliado();
                this.CerrarDialog(this.CheckTarifaSelelect);
            }
            catch (Exception ex)
            {
                MostrarMensaje("Inconsistencias al presentar la información.", ETipoMensajeIU.ERROR, NombreClase + ".btnAgregar_Click :" + ex.Message);
            }
        }

        protected void cbSinTarifas_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if ((sender as CheckBox).Checked)
                    ucTarifasEquipoAdicional.EstablecerSinTarifas();
                else
                {
                    ucTarifasEquipoAdicional.Inicializar(Plazo, TipoCotizacion, ETipoEquipo.EquipoAliado, new List<TarifaFSLBO>(), EquipoID, false );
                    ucTarifasEquipoAdicional.PermitirTipoCargo(true);
                }
            }
            catch(Exception ex)
            {
                MostrarMensaje("Inconsistencias al Deshabilitar las Tarifas.", ETipoMensajeIU.ERROR, NombreClase + ".cbSinTarifas_CheckedChanged :" + ex.Message);
            }
        }
        #endregion
    }
}
