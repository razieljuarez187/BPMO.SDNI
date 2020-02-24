using System;
using BPMO.Primitivos.Enumeradores;
using BPMO.SDNI.Contratos.PSL.Reportes.PRE;
using BPMO.SDNI.Contratos.PSL.Reportes.VIS;
using ReportPage = BPMO.SDNI.Reportes.UI.Page;

namespace BPMO.SDNI.Contratos.PSL.Reportes.UI {

    /// <summary>
    /// Interfaz de usuario para Reporte Detallado por Sucursal en e-renta
    /// </summary>
    public partial class DetalladoPSLSucursalUI : ReportPage, IDetalladoPSLSucursalVIS {
        #region Constantes
        /// <summary>
        /// Nombre de la clase en curso
        /// </summary>
        private static readonly String nombreClase = typeof(DetalladoPSLSucursalUI).Name;
        #endregion

        #region Atributos
        /// <summary>
        /// Presentador que esta atendiendo las peticiones de la vista
        /// </summary>
        private DetalladoPSLSucursalPRE presentador;
        #endregion

        #region Propiedades
        /// <summary>
        /// Obtiene o establece un valor que representa el identificador del ´modelo
        /// </summary>
        /// <value>Objeto de tipo Nullable de Int32</value>
        public int? ModeloID {
            get {
                return this.Master.ModeloID;
            }
            set {
                this.Master.ModeloID = value;
            }
        }

        /// <summary>
        /// Obtiene o establece un valor que representa el nombre del modelo para las que aplica la configuración
        /// </summary>
        /// <value>Objeto de tipo String</value>
        public string ModeloNombre {
            get {
                return this.Master.ModeloNombre;
            }
            set {
                this.Master.ModeloNombre = value;
            }
        }

        /// <summary>
        /// Obtiene o establece un valor que representa el identificador de sucursal
        /// </summary>
        /// <value>Objeto de tipo Nullable de Int32</value>
        public int? SucursalID {
            get {
                return this.Master.SucursalID;
            }
            set {
                this.Master.SucursalID = value;
            }
        }

        /// <summary>
        /// Obtiene o establece un valor que representa el nombre de la sucursal para las que aplica la configuración
        /// </summary>
        /// <value>Objeto de tipo String</value>
        public string SucursalNombre {
            get {
                return this.Master.SucursalNombre;
            }
            set {
                this.Master.SucursalNombre = value;
            }
        }

        /// <summary>
        /// Obtiene o establece un valor que representa el año
        /// </summary>
        /// <value>Valor de tipo ETipoContrato</value>
        public int? Anio {
            get {
                return this.Master.Anio;
            }
            set {
                this.Master.Anio = value;
            }
        }

        /// <summary>
        /// Obtiene o establece un valor que representa el mes que se aplicará al reporte
        /// </summary>
        /// <value>Objeto de tipo Int32</value>
        public int? Mes {
            get {
                return this.Master.Mes;
            }
            set {
                this.Master.Mes = value;
            }
        }
        #endregion

        #region Métodos
        /// <summary>
        /// Método abstract que se ejecuta cuando 
        /// </summary>
        public override void Consultar() {
            try {
                this.presentador.Consultar();
            } catch (Exception ex) {
                this.MostrarMensaje("Inconsistencias en la configuracion", ETipoMensajeIU.ERROR, nombreClase + ex.Message);
            }
        }
        #endregion

        #region Controladores de eventos
        /// <summary>
        /// Evento que se ejecuta cuando se carga la página
        /// </summary>
        /// <param name="sender">Objeto que genero el evento</param>
        /// <param name="e">Argumentos asociados al evento</param>
        protected void Page_Load(object sender, EventArgs e) {
            this.presentador = new DetalladoPSLSucursalPRE(this);
            if (!this.IsPostBack) {
                this.presentador.ValidarAcceso();

                //Se definen los filtros Visibles
                this.Master.SucursalFiltroVisible = true;
                this.Master.AnioFiltroVisible = true;
                this.Master.MesFiltroVisible = true;
                this.Master.AnioEtiqueta = "Año";

                this.Master.AnioFiltroRequerido = true;
                this.Master.MesFiltroRequerido = true;
            }
        }
        #endregion
    }
}