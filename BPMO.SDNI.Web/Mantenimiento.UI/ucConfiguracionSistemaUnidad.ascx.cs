// Satisface al CU073 - Catálogo Configuración Sistemas Unidad
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BPMO.SDNI.Mantenimiento.BO;

namespace BPMO.SDNI.Mantenimiento.UI {

    /// <summary>
    /// Clase que contiene los métodos para la presentación de la información del Formulario Configuración Sistema de Unidad Idealease, 
    /// al usuario.
    /// </summary
    public partial class ucConfiguracionSistemaUnidad : System.Web.UI.UserControl {

        #region Atributos

        /// <summary>
        /// Obtiene o establece un valor que representa la Configuración Sistema de Unidad Idealease seleccionada.
        /// </summary>
        public ConfiguracionSistemaUnidadBO ConfiguracionSeleccionada {
            get { return Session["configuracionSeleccionada"] as ConfiguracionSistemaUnidadBO; }
            set { Session["configuracionSeleccionada"] = value; }
        }

            #region Seguridad

        /// <summary>
        /// Obtiene o establece un valor que representa la Configuración de la Unidad Operativa que indica a que libro corresponden los activos.
        /// </summary>
        public string LibroActivos {
            get {
                string valor = null;
                if (this.hdnLibroActivos.Value.Trim().Length > 0)
                    valor = this.hdnLibroActivos.Value.Trim().ToUpper();
                return valor;

            }
            set {
                if (value != null)
                    this.hdnLibroActivos.Value = value.ToString();
                else
                    this.hdnLibroActivos.Value = string.Empty;
            }
        }

            #endregion

        #endregion

        #region Constructor

        /// <summary>
        /// Método delegado para el evento de carga de la página.
        /// </summary>
        /// <param name="sender">Objeto que desencadenó el evento.</param>
        /// <param name="e">Argumento asociado al evento.</param>
        protected void Page_Load(object sender, EventArgs e) { }

        #endregion

        #region Métodos

        /// <summary>
        /// Establece los campos como solo lectura o escritura.
        /// </summary>
        /// <param name="bloquear">Indica si es de solo lectura o escritura.</param>
        public void BloquearCampos(bool bloquear) {
            txtClave.ReadOnly = bloquear;
            txtClave.Enabled = !bloquear;
            txtNombre.ReadOnly = bloquear;
            txtNombre.Enabled = !bloquear;
        }

        /// <summary>
        /// Obtiene la Configuración Sistema de Unidad del formulario.
        /// </summary>
        public void FormToConfiguracionSistemaUnidad() {
            ConfiguracionSeleccionada.Nombre = txtNombre.Text.ToUpper();
            ConfiguracionSeleccionada.Clave = txtClave.Text.ToUpper();
        }

        /// <summary>
        /// Carga los datos de la Configuracion Sistema de Unidad al formulario.
        /// </summary>
        public void ConfiguracionSistemaUnidadToForm() {
            txtNombre.Text = ConfiguracionSeleccionada.Nombre;
            txtClave.Text = ConfiguracionSeleccionada.Clave;
        }

        #endregion
    }
}