//Satisface al CU077 - Registrar Acta de Nacimiento de una Unidad
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using BPMO.Primitivos.Enumeradores;

using BPMO.SDNI.MapaSitio.UI;

using BPMO.SDNI.Equipos.BO;
using BPMO.SDNI.Equipos.PRE;
using BPMO.SDNI.Equipos.VIS;

namespace BPMO.SDNI.Equipos.UI
{
    public partial class ucAsignacionLlantasUI : System.Web.UI.UserControl, IucAsignacionLlantasVIS
    {
        #region Atributos
        private ucAsignacionLlantasPRE presentador;
        private string nombreClase = "ucAsignacionLlantasUI";
        #endregion

        #region Propiedades
        public int? EnllantableID
        {
            get
            {
                return this.ucLlanta.EnllantableID;
            }
            set
            {
                this.ucLlanta.EnllantableID = value;
                this.ucRefaccion.EnllantableID = value;
            }
        }
        public int? SucursalEnllantableID {
            get {
                return this.ucLlanta.SucursalEnllantableID;
            }
            set {
                this.ucLlanta.SucursalEnllantableID = value;
                this.ucRefaccion.SucursalEnllantableID = value;
            }
        }
        public int? TipoEnllantable
        {
            get
            {
                return this.ucLlanta.TipoEnllantable;
            }
            set
            {
                this.ucLlanta.TipoEnllantable = value;
                this.ucRefaccion.TipoEnllantable = value;
            }
        }
        public string DescripcionEnllantable
        {
            get
            {
                return this.ucLlanta.DescripcionEnllantable;
            }
            set
            {
                this.ucLlanta.DescripcionEnllantable = value;
                this.ucRefaccion.DescripcionEnllantable = value;
            }
        }

        public int? LlantaID
        {
            get
            {
                return this.ucLlanta.LlantaID;
            }
            set
            {
                this.ucLlanta.LlantaID = value;
            }
        }
        public string Codigo
        {
            get
            {
                return this.ucLlanta.Codigo;
            }
            set
            {
                this.ucLlanta.Codigo = value;
            }
        }
        public int? SucursalID {
            get {
                return this.ucLlanta.SucursalID;
            }
            set {
                this.ucLlanta.SucursalID = value;
            }
        }
        public string SucursalNombre {
            get {
                return this.ucLlanta.SucursalNombre;
            }
            set {
                this.ucLlanta.SucursalNombre = value;
            }
        }
        public string Marca
        {
            get
            {
                return this.ucLlanta.Marca;
            }
            set
            {
                this.ucLlanta.Marca = value;
            }
        }
        public string Modelo
        {
            get
            {
                return this.ucLlanta.Modelo;
            }
            set
            {
                this.ucLlanta.Modelo = value;
            }
        }
        public string Medida
        {
            get
            {
                return this.ucLlanta.Medida;
            }
            set
            {
                this.ucLlanta.Medida = value;
            }
        }
        public decimal? Profundidad
        {
            get
            {
                return this.ucLlanta.Profundidad;
            }
            set
            {
                this.ucLlanta.Profundidad = value;
            }
        }
        public bool? Revitalizada
        {
            get
            {
                return this.ucLlanta.Revitalizada;
            }
            set
            {
                this.ucLlanta.Revitalizada = value;
            }
        }
        public bool? Stock
        {
            get
            {
                return this.ucLlanta.Stock;
            }
            set
            {
                this.ucLlanta.Stock = value;
            }
        }
        public bool? Activo
        {
            get
            {
                return this.ucLlanta.Activo;
            }
            set
            {
                this.ucLlanta.Activo = value;
            }
        }
        public DateTime? FC
        {
            get
            {
                return this.ucLlanta.FC;
            }
            set
            {
                this.ucLlanta.FC = value;
            }
        }
        public DateTime? FUA
        {
            get
            {
                return this.ucLlanta.FUA;
            }
            set
            {
                this.ucLlanta.FUA = value;
            }
        }
        public int? UC
        {
            get
            {
                return this.ucLlanta.UC;
            }
            set
            {
                this.ucLlanta.UC = value;
            }
        }
        public int? UUA
        {
            get
            {
                return this.ucLlanta.UUA;
            }
            set
            {
                this.ucLlanta.UUA = value;
            }
        }
        public int? Posicion
        {
            get
            {
                return this.ucLlanta.Posicion;
            }
            set
            {
                this.ucLlanta.Posicion = value;
            }
        }

        public List<LlantaBO> Llantas
        {
            get
            {
                if ((List<LlantaBO>)Session["ListaLlantas"] == null)
                    return new List<LlantaBO>();
                else
                    return (List<LlantaBO>)Session["ListaLlantas"];
            }
            set
            {
                Session["ListaLlantas"] = value;
            }
        }
        public List<LlantaBO> UltimoLlantas
        {
            get
            {
                if ((List<LlantaBO>)Session["LastListaLlantas"] == null)
                    return new List<LlantaBO>();
                else
                    return (List<LlantaBO>)Session["LastListaLlantas"];
            }
            set
            {
                Session["LastListaLlantas"] = value;
            }
        }

        public int? RefaccionID
        {
            get
            {
                return this.ucRefaccion.LlantaID;
            }
            set
            {
                this.ucRefaccion.LlantaID = value;
            }
        }
        public string RefaccionCodigo
        {
            get
            {
                return this.ucRefaccion.Codigo;
            }
            set
            {
                this.ucRefaccion.Codigo = value;
            }
        }
        public int? RefaccionSucursalID {
            get {
                return this.ucRefaccion.SucursalID;
            }
            set {
                this.ucRefaccion.SucursalID = value;
            }
        }
        public string RefaccionSucursalNombre {
            get {
                return this.ucRefaccion.SucursalNombre;
            }
            set {
                this.ucRefaccion.SucursalNombre = value;
            }
        }
        public string RefaccionMarca
        {
            get
            {
                return this.ucRefaccion.Marca;
            }
            set
            {
                this.ucRefaccion.Marca = value;
            }
        }
        public string RefaccionModelo
        {
            get
            {
                return this.ucRefaccion.Modelo;
            }
            set
            {
                this.ucRefaccion.Modelo = value;
            }
        }
        public string RefaccionMedida
        {
            get
            {
                return this.ucRefaccion.Medida;
            }
            set
            {
                this.ucRefaccion.Medida = value;
            }
        }
        public decimal? RefaccionProfundidad
        {
            get
            {
                return this.ucRefaccion.Profundidad;
            }
            set
            {
                this.ucRefaccion.Profundidad = value;
            }
        }
        public bool? RefaccionRevitalizada
        {
            get
            {
                return this.ucRefaccion.Revitalizada;
            }
            set
            {
                this.ucRefaccion.Revitalizada = value;
            }
        }
        public bool? RefaccionStock
        {
            get
            {
                return this.ucRefaccion.Stock;
            }
            set
            {
                this.ucRefaccion.Stock = value;
            }
        }
        public bool? RefaccionActivo
        {
            get
            {
                return this.ucRefaccion.Activo;
            }
            set
            {
                this.ucRefaccion.Activo = value;
            }
        }
        public DateTime? RefaccionFC
        {
            get
            {
                return this.ucRefaccion.FC;
            }
            set
            {
                this.ucRefaccion.FC = value;
            }
        }
        public DateTime? RefaccionFUA
        {
            get
            {
                return this.ucRefaccion.FUA;
            }
            set
            {
                this.ucRefaccion.FUA = value;
            }
        }
        public int? RefaccionUC
        {
            get
            {
                return this.ucRefaccion.UC;
            }
            set
            {
                this.ucRefaccion.UC = value;
            }
        }
        public int? RefaccionUUA
        {
            get
            {
                return this.ucRefaccion.UUA;
            }
            set
            {
                this.ucRefaccion.UUA = value;
            }
        }

        public IucLlantaVIS VistaLlanta { get { return this.ucLlanta; } }
        public IucLlantaVIS VistaRefaccion { get { return this.ucRefaccion; } }

        public int? UsuarioAutenticado
        {
            get
            {
                int? id = null;
                if (!String.IsNullOrEmpty(this.hdnUsuarioAutenticado.Value))
                    id = int.Parse(this.hdnUsuarioAutenticado.Value.Trim());
                return id;
            }
            set
            {
                if (value != null)
                    this.hdnUsuarioAutenticado.Value = value.ToString();
                else
                    this.hdnUsuarioAutenticado.Value = string.Empty;
            }
        }
        #endregion

        #region Constructores
        protected void Page_Load(object sender, EventArgs e)
        {
            this.presentador = new ucAsignacionLlantasPRE(this, this.ucLlanta, this.ucRefaccion);
        }
        #endregion

        #region Métodos
        public void PrepararNuevo()
        {
            this.grvLlantas.DataSource = null;
            this.grvLlantas.DataBind();
        }

        public void HabilitarModoEdicion(bool habilitar)
        {
            this.grvLlantas.Enabled = habilitar;
            this.btnAgregar.Enabled = habilitar;
        }

        public void ActualizarLlantas()
        {
            this.grvLlantas.DataSource = this.Llantas;
            this.grvLlantas.DataBind();
        }

        public void LimpiarSesion()
        {
            if (Session["ListaLlantas"] != null)
                Session.Remove("ListaLlantas");
            if (Session["LastListaLlantas"] != null)
                Session.Remove("LastListaLlantas");
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

                string botonID = "";
                switch (msjDetalle)
                {
                    case "AGREGARLLANTA":
                        botonID = this.btnAgregar.UniqueID;
                        break;
                }
                this.RegistrarScript("Confirm", "abrirConfirmacion('" + mensaje + "', '" + botonID + "');");
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
        /// <summary>
        /// Registra Script en el cliente
        /// </summary>
        /// <param name="key">Key del script</param>
        /// <param name="script">script a registrar</param>
        private void RegistrarScript(string key, string script)
        {
            ScriptManager.RegisterStartupScript(Page, typeof(Page), key, script, true);
        }
        #endregion

        #region Eventos
        protected void btnAgregar_Click(object sender, EventArgs e)
        {
            try
            {
                this.presentador.AgregarLlanta();
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencia al agregar el equipo aliado a la unidad", ETipoMensajeIU.ERROR, this.nombreClase + ".btnAgregar_Click:" + ex.Message);
            }
        }

        protected void grvLlantas_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int index;

            if (e.CommandName.ToString().ToUpper() == "PAGE") return;

            try
            {
                index = Convert.ToInt32(e.CommandArgument.ToString());
                switch (e.CommandName.ToString())
                {
                    case "Eliminar":
                        this.presentador.QuitarLlanta(index);
                        break;
                }
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencia al ejecutar la acción sobre el equipo aliado", ETipoMensajeIU.ERROR, this.nombreClase + ".grvEquiposAliados_RowCommand:" + ex.Message);
            }
        }
        #endregion
    }
}