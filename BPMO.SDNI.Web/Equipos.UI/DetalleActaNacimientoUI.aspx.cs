//Satisface al CU079 - Consultar Acta de Nacimiento de Unidad
//Satisface la solicitud de cambio SC0006

using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using BPMO.Basicos.BO;
using BPMO.Primitivos.Enumeradores;
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
	public partial class DetalleActaNacimientoUI : System.Web.UI.Page, IDetalleActaNacimientoVIS
	{
		#region Atributos
		private DetalleActaNacimientoPRE presentador;
		private string nombreClase = "DetalleActaNacimientoUI";
        //14150, variable global para instanciar las etiquetas que se obtendrán a través del archivo de recursos.
	    private ObtenerEtiquetaEmpresas obtenerEtiqueta = null;
		/// <summary>
		/// Enumerador de Catalogos para el Buscador
		/// </summary>
		public enum ECatalogoBuscador
		{
			HistorialUnidad = 0
		}
		#endregion

		#region Propiedades
		public int? EquipoID
		{
			get
			{
				int? id = null;
				if (!String.IsNullOrEmpty(this.hdnEquipoId.Value))
					id = int.Parse(this.hdnEquipoId.Value.Trim());
				return id;
			}
			set
			{
				if (value != null)
					this.hdnEquipoId.Value = value.ToString();
				else
					this.hdnEquipoId.Value = string.Empty;
			}
		}
		public int? UnidadID
		{
			get
			{
				int? id = null;
				if (!String.IsNullOrEmpty(this.hdnUnidadId.Value))
					id = int.Parse(this.hdnUnidadId.Value.Trim());
				return id;
			}
			set
			{
				if (value != null)
					this.hdnUnidadId.Value = value.ToString();
				else
					this.hdnUnidadId.Value = string.Empty;
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
				
					if (masterMsj != null && masterMsj.Usuario != null)
						id = masterMsj.Usuario.Id;
				
				return id;
			}
		}
		public int? UUA
		{
			get { return this.UC; }
		}
		public EEstatusUnidad? EstatusUnidad
		{
			get
			{
				EEstatusUnidad? area = null;
				if (this.hdnEstatusUnidad.Value.Trim().CompareTo("") != 0)
					area = (EEstatusUnidad)Enum.Parse(typeof(EEstatusUnidad), this.hdnEstatusUnidad.Value);
				return area;
			}
			set
			{
				if (value == null)
					this.hdnEstatusUnidad.Value = "";
				else
					this.hdnEstatusUnidad.Value = ((int)value).ToString();
			}
		}

		public string NumeroSerie
		{
			get
			{
				return (String.IsNullOrEmpty(this.txtEstaticoNumSerie.Text)) ? null : this.txtEstaticoNumSerie.Text.Trim().ToUpper();
			}
			set
			{
				if (value != null)
					this.txtEstaticoNumSerie.Text = value;
				else
					this.txtEstaticoNumSerie.Text = string.Empty;
			}
		}
		public string ClaveActivoOracle
		{
			get
			{
				return (String.IsNullOrEmpty(this.txtEstaticoClaveOracle.Text)) ? null : this.txtEstaticoClaveOracle.Text.Trim().ToUpper();
			}
			set
			{
				if (value != null)
					this.txtEstaticoClaveOracle.Text = value;
				else
					this.txtEstaticoClaveOracle.Text = string.Empty;
			}
		}
		public int? LiderID
		{
			get
			{
				int? id = null;
				if (!String.IsNullOrEmpty(this.txtEstaticoIDLeader.Text))
					id = int.Parse(this.txtEstaticoIDLeader.Text.Trim());
				return id;
			}
			set
			{
				if (value != null)
					this.txtEstaticoIDLeader.Text = value.ToString();
				else
					this.txtEstaticoIDLeader.Text = string.Empty;
			}
		}
		public string NumeroEconomico
		{
			get
			{
				return (String.IsNullOrEmpty(this.txtEstaticoNumEconomico.Text)) ? null : this.txtEstaticoNumEconomico.Text.Trim().ToUpper();
			}
			set
			{
				if (value != null)
					this.txtEstaticoNumEconomico.Text = value;
				else
					this.txtEstaticoNumEconomico.Text = string.Empty;
			}
		}

		public string TipoUnidadNombre
		{
			get
			{
				return (String.IsNullOrEmpty(this.txtEstaticoTipoUnidad.Text)) ? null : this.txtEstaticoTipoUnidad.Text.Trim().ToUpper();
			}
			set
			{
				if (value != null)
					this.txtEstaticoTipoUnidad.Text = value;
				else
					this.txtEstaticoTipoUnidad.Text = string.Empty;
			}
		}
		public int? TipoUnidadId
		{
			get
			{
				int? id = null;
				if (!String.IsNullOrEmpty(this.hdnTipoUnidadID.Value))
					id = int.Parse(this.hdnTipoUnidadID.Value.Trim());
				return id;
			}
			set
			{
				if (value != null)
					this.hdnTipoUnidadID.Value = value.ToString();
				else
					this.hdnTipoUnidadID.Value = string.Empty;
			}
		}

		public int? MarcaId
		{
			get
			{
				int? id = null;
				if (!String.IsNullOrEmpty(this.hdnMarcaID.Value))
					id = int.Parse(this.hdnMarcaID.Value.Trim());
				return id;
			}
			set
			{
				if (value != null)
					this.hdnMarcaID.Value = value.ToString();
				else
					this.hdnMarcaID.Value = string.Empty;
			}
		}

		public string ModeloNombre
		{
			get
			{
				return (String.IsNullOrEmpty(this.txtEstaticoModelo.Text)) ? null : this.txtEstaticoModelo.Text.Trim().ToUpper();
			}
			set
			{
				if (value != null)
					this.txtEstaticoModelo.Text = value;
				else
					this.txtEstaticoModelo.Text = string.Empty;
			}
		}
		public int? ModeloId
		{
			get
			{
				int? id = null;
				if (!String.IsNullOrEmpty(this.hdnModeloID.Value))
					id = int.Parse(this.hdnModeloID.Value.Trim());
				return id;
			}
			set
			{
				if (value != null)
					this.hdnModeloID.Value = value.ToString();
				else
					this.hdnModeloID.Value = string.Empty;
			}
		}

		public int? Anio
		{
			get
			{
				int? id = null;
				if (!String.IsNullOrEmpty(this.txtEstaticoAnio.Text))
					id = int.Parse(this.txtEstaticoAnio.Text.Trim());
				return id;
			}
			set
			{
				if (value != null)
					this.txtEstaticoAnio.Text = value.ToString();
				else
					this.txtEstaticoAnio.Text = string.Empty;
			}
		}
		public DateTime? FechaCompra
		{
			get
			{
				DateTime? temp = null;
				if (!String.IsNullOrEmpty(this.txtEstaticoFechaCompra.Text))
					temp = DateTime.Parse(this.txtEstaticoFechaCompra.Text.Trim());
				return temp;
			}
			set
			{
				if (value != null)
                    this.txtEstaticoFechaCompra.Text = value.Value.ToShortDateString();
				else
					this.txtEstaticoFechaCompra.Text = string.Empty;
			}
		}
		public decimal? MontoFactura
		{
			get
			{
				decimal? temp = null;
				if (!String.IsNullOrEmpty(this.txtEstaticoMontoFactura.Text))
					temp = int.Parse(this.txtEstaticoMontoFactura.Text.Trim().Replace(",",""));
				return temp;
			}
			set
			{
				if (value != null)
					this.txtEstaticoMontoFactura.Text = string.Format("{0:#,##0.0000}", value);
				else
					this.txtEstaticoMontoFactura.Text = string.Empty;
			}
		}

		//Página 1
		public string Propietario
		{
			get
			{
				return this.ucDatosGeneralesUI.Propietario;
			}
			set
			{
				this.ucDatosGeneralesUI.Propietario = value;
			}
		}
		public string Cliente
		{
			get
			{
				return this.ucDatosGeneralesUI.Cliente;
			}
			set
			{
				this.ucDatosGeneralesUI.Cliente = value;
			}
		}
		public int? ClienteId
		{
			get
			{
				return this.ucDatosGeneralesUI.ClienteId;
			}
			set
			{
				this.ucDatosGeneralesUI.ClienteId = value;
			}
		}
        #region SC0002
        public bool? EntraMantenimiento
        {
            get
            {
                return this.ucDatosGeneralesUI.EntraMantenimiento;
            }
            set
            {

                this.ucDatosGeneralesUI.EntraMantenimiento = value;

            }
        }
        #endregion
        public int? UnidadOperativaId
		{
			get
			{
				int? id = null;
				Site masterMsj = (Site)Page.Master;

				if (masterMsj != null && (masterMsj.Adscripcion != null && masterMsj.Adscripcion.UnidadOperativa != null))
					id = masterMsj.Adscripcion.UnidadOperativa.Id;
				return id;
			}
		}
		public string SucursalNombre
		{
			get
			{
				return this.ucDatosGeneralesUI.SucursalNombre;
			}
			set
			{
				this.ucDatosGeneralesUI.SucursalNombre = value;
			}
		}
		public int? SucursalId
		{
			get
			{
				return this.ucDatosGeneralesUI.SucursalId;
			}
			set
			{
				this.ucDatosGeneralesUI.SucursalId = value;
			}
		}
		public Enum Area
		{
            get
            {
                Enum area = null;
                switch (this.UnidadOperativaId)
                {
                    case (int)ETipoEmpresa.Generacion:
                        area = this.ucDatosGeneralesUI.ETipoRentaGeneracion;
                        break;
                    case (int)ETipoEmpresa.Construccion:
                        area = this.ucDatosGeneralesUI.ETipoRentaConstruccion;
                        break;
                    case (int)ETipoEmpresa.Equinova:
                        area = this.ucDatosGeneralesUI.ETipoRentaEquinova;
                        break;
                    default:
                        area = this.ucDatosGeneralesUI.Area;
                        break;
                }
                return area;
            }
            set
            {
                switch (this.UnidadOperativaId)
                {
                    case (int)ETipoEmpresa.Generacion:
                        this.ucDatosGeneralesUI.ETipoRentaGeneracion = (EAreaGeneracion)value;
                        break;
                    case (int)ETipoEmpresa.Construccion:
                        this.ucDatosGeneralesUI.ETipoRentaConstruccion = (EAreaConstruccion)value;
                        break;
                    case (int)ETipoEmpresa.Equinova:
                        this.ucDatosGeneralesUI.ETipoRentaEquinova = (EAreaEquinova)value;
                        break;
                    default:
                        this.ucDatosGeneralesUI.Area = (EArea)value;
                        break;
                }
            }
		}
		public string FabricanteNombre
		{
			get { return this.ucDatosGeneralesUI.Fabricante; }
			set { this.ucDatosGeneralesUI.Fabricante = value; }
		}

        //RQM 14150, Pagina 1, datos generales, generación y construcción
        /// <summary>
        /// Orden de compra del proveedor
        /// </summary>
        public string OrdenCompraProveedor
        {
            get { return this.ucDatosGeneralesUI.OrdenCompraProveedor; }
            set { this.ucDatosGeneralesUI.OrdenCompraProveedor = value; }
        }

        /// <summary>
        /// Monto del arrendamiento
        /// </summary>
        public decimal? MontoArrendamiento
        {
            get { return this.ucDatosGeneralesUI.MontoArrendamiento; }
            set { this.ucDatosGeneralesUI.MontoArrendamiento = value; }

        }

        /// <summary>
        /// Código de la moneda
        /// </summary>
        public string CodigoMoneda
        {
            get { return this.ucDatosGeneralesUI.CodigoMoneda; }
            set { this.ucDatosGeneralesUI.CodigoMoneda = value; }
        }

        /// <summary>
        /// Fecha de inicio del arrendamiento
        /// </summary>
        public DateTime? FechaInicioArrendamiento
        {
            get { return this.ucDatosGeneralesUI.FechaInicioArrendamiento; }
            set { this.ucDatosGeneralesUI.FechaInicioArrendamiento = value; }
        }

        /// <summary>
        /// Fecha de fin del arrendamiento
        /// </summary>
        public DateTime? FechaFinArrendamiento
        {
            get { return this.ucDatosGeneralesUI.FechaFinArrendamiento; }
            set { this.ucDatosGeneralesUI.FechaFinArrendamiento = value; }
        }

        /// <summary>
        /// Identificador del proveedor
        /// </summary>
        public int? ProveedorID
        {
            get { return this.ucDatosGeneralesUI.ProveedorID; }
            set { this.ucDatosGeneralesUI.ProveedorID = value; }
        }

		//Página 2
		public List<HorometroBO> Horometros
		{
			get
			{
				return this.ucDatosTecnicosUI.Horometros;
			}
			set
			{
				this.ucDatosTecnicosUI.Horometros = value;
			}
		}
		public List<OdometroBO> Odometros
		{
			get
			{
				return this.ucDatosTecnicosUI.Odometros;
			}
			set
			{
				this.ucDatosTecnicosUI.Odometros = value;
			}
		}
		public decimal? PBVMaximoRecomendado
		{
			get
			{
				return this.ucDatosTecnicosUI.PBVMaximoRecomendado;
			}
			set
			{
				this.ucDatosTecnicosUI.PBVMaximoRecomendado = value;
			}
		}
		public decimal? PBCMaximoRecomendado
		{
			get
			{
				return this.ucDatosTecnicosUI.PBCMaximoRecomendado;
			}
			set
			{
				this.ucDatosTecnicosUI.PBCMaximoRecomendado = value;
			}
		}
		public decimal? CapacidadTanque
		{
			get
			{
				return this.ucDatosTecnicosUI.CapacidadTanque;
			}
			set
			{
				this.ucDatosTecnicosUI.CapacidadTanque = value;
			}
		}
		public decimal? RendimientoTanque
		{
			get
			{
				return this.ucDatosTecnicosUI.RendimientoTanque;
			}
			set
			{
				this.ucDatosTecnicosUI.RendimientoTanque = value;
			}
		}

		//Página 3
		public string Radiador
		{
			get
			{
				return this.ucNumerosSerieUI.Radiador;
			}
			set
			{
				this.ucNumerosSerieUI.Radiador = value;
			}
		}
		public string PostEnfriador
		{
			get
			{
				return this.ucNumerosSerieUI.PostEnfriador;
			}
			set
			{
				this.ucNumerosSerieUI.PostEnfriador = value;
			}
		}
        
        public List<NumeroSerieBO> NumerosSerie {
            get {
                return this.ucNumerosSerieUI.NumerosSerie;
            }
            set {
                this.ucNumerosSerieUI.NumerosSerie = value;
            }
        }
        
		#region SC0030
		public string SerieMotor {
			get {
				return this.ucNumerosSerieUI.SerieMotor;
			}
			set {
				this.ucNumerosSerieUI.SerieMotor = value;
			}
		}
		#endregion
		public string SerieTurboCargador
		{
			get
			{
				return this.ucNumerosSerieUI.SerieTurboCargador;
			}
			set
			{
				this.ucNumerosSerieUI.SerieTurboCargador = value;
			}
		}
		public string SerieCompresorAire
		{
			get
			{
				return this.ucNumerosSerieUI.SerieCompresorAire;
			}
			set
			{
				this.ucNumerosSerieUI.SerieCompresorAire = value;
			}
		}
		public string SerieECM
		{
			get
			{
				return this.ucNumerosSerieUI.SerieECM;
			}
			set
			{
				this.ucNumerosSerieUI.SerieECM = value;
			}
		}
		public string SerieAlternador
		{
			get
			{
				return this.ucNumerosSerieUI.SerieAlternador;
			}
			set
			{
				this.ucNumerosSerieUI.SerieAlternador = value;
			}
		}
		public string SerieMarcha
		{
			get
			{
				return this.ucNumerosSerieUI.SerieMarcha;
			}
			set
			{
				this.ucNumerosSerieUI.SerieMarcha = value;
			}
		}
		public string SerieBaterias
		{
			get
			{
				return this.ucNumerosSerieUI.SerieBaterias;
			}
			set
			{
				this.ucNumerosSerieUI.SerieBaterias = value;
			}
		}
		public string TransmisionSerie
		{
			get
			{
				return this.ucNumerosSerieUI.TransmisionSerie;
			}
			set
			{
				this.ucNumerosSerieUI.TransmisionSerie = value;
			}
		}
		public string TransmisionModelo
		{
			get
			{
				return this.ucNumerosSerieUI.TransmisionModelo;
			}
			set
			{
				this.ucNumerosSerieUI.TransmisionModelo = value;
			}
		}
		public string EjeDireccionSerie
		{
			get
			{
				return this.ucNumerosSerieUI.EjeDireccionSerie;
			}
			set
			{
				this.ucNumerosSerieUI.EjeDireccionSerie = value;
			}
		}
		public string EjeDireccionModelo
		{
			get
			{
				return this.ucNumerosSerieUI.EjeDireccionModelo;
			}
			set
			{
				this.ucNumerosSerieUI.EjeDireccionModelo = value;
			}
		}
		public string EjeTraseroDelanteroSerie
		{
			get
			{
				return this.ucNumerosSerieUI.EjeTraseroDelanteroSerie;
			}
			set
			{
				this.ucNumerosSerieUI.EjeTraseroDelanteroSerie = value;
			}
		}
		public string EjeTraseroDelanteroModelo
		{
			get
			{
				return this.ucNumerosSerieUI.EjeTraseroDelanteroModelo;
			}
			set
			{
				this.ucNumerosSerieUI.EjeTraseroDelanteroModelo = value;
			}
		}
		public string EjeTraseroTraseroSerie
		{
			get
			{
				return this.ucNumerosSerieUI.EjeTraseroTraseroSerie;
			}
			set
			{
				this.ucNumerosSerieUI.EjeTraseroTraseroSerie = value;
			}
		}
		public string EjeTraseroTraseroModelo
		{
			get
			{
				return this.ucNumerosSerieUI.EjeTraseroTraseroModelo;
			}
			set
			{
				this.ucNumerosSerieUI.EjeTraseroTraseroModelo = value;
			}
		}

		//Página 4
		public int? EnllantableID
		{
			get
			{
				return this.ucAsignacionLlantasUI.EnllantableID;
			}
			set
			{
				this.ucAsignacionLlantasUI.EnllantableID = value;
			}
		}
		public int? TipoEnllantable
		{
			get
			{
				return this.ucAsignacionLlantasUI.TipoEnllantable;
			}
			set
			{
				this.ucAsignacionLlantasUI.TipoEnllantable = value;
			}
		}
		public string DescripcionEnllantable
		{
			get
			{
				return this.ucAsignacionLlantasUI.DescripcionEnllantable;
			}
			set
			{
				this.ucAsignacionLlantasUI.DescripcionEnllantable = value;
			}
		}
		public List<LlantaBO> Llantas
		{
			get
			{
				return this.ucAsignacionLlantasUI.Llantas;
			}
			set
			{
				this.ucAsignacionLlantasUI.Llantas = value;
			}
		}
		public int? RefaccionID
		{
			get
			{
				return this.ucAsignacionLlantasUI.RefaccionID;
			}
			set
			{
				this.ucAsignacionLlantasUI.RefaccionID = value;
			}
		}
		public string RefaccionCodigo
		{
			get
			{
				return this.ucAsignacionLlantasUI.RefaccionCodigo;
			}
			set
			{
				this.ucAsignacionLlantasUI.RefaccionCodigo = value;
			}
		}
        public int? RefaccionSucursalID {
            get {
                return this.ucAsignacionLlantasUI.RefaccionSucursalID;
            }
            set {
                this.ucAsignacionLlantasUI.RefaccionSucursalID = value;
            }
        }
        public string RefaccionSucursalNombre {
            get {
                return this.ucAsignacionLlantasUI.RefaccionSucursalNombre;
            }
            set {
                this.ucAsignacionLlantasUI.RefaccionSucursalNombre = value;
            }
        }
        public string RefaccionMarca
		{
			get
			{
				return this.ucAsignacionLlantasUI.RefaccionMarca;
			}
			set
			{
				this.ucAsignacionLlantasUI.RefaccionMarca = value;
			}
		}
		public string RefaccionModelo
		{
			get
			{
				return this.ucAsignacionLlantasUI.RefaccionModelo;
			}
			set
			{
				this.ucAsignacionLlantasUI.RefaccionModelo = value;
			}
		}
		public string RefaccionMedida
		{
			get
			{
				return this.ucAsignacionLlantasUI.RefaccionMedida;
			}
			set
			{
				this.ucAsignacionLlantasUI.RefaccionMedida = value;
			}
		}
		public decimal? RefaccionProfundidad
		{
			get
			{
				return this.ucAsignacionLlantasUI.RefaccionProfundidad;
			}
			set
			{
				this.ucAsignacionLlantasUI.RefaccionProfundidad = value;
			}
		}
		public bool? RefaccionRevitalizada
		{
			get
			{
				return this.ucAsignacionLlantasUI.RefaccionRevitalizada;
			}
			set
			{
				this.ucAsignacionLlantasUI.RefaccionRevitalizada = value;
			}
		}
		public bool? RefaccionStock
		{
			get
			{
				return this.ucAsignacionLlantasUI.RefaccionStock;
			}
			set
			{
				this.ucAsignacionLlantasUI.RefaccionStock = value;
			}
		}
		public bool? RefaccionActivo
		{
			get
			{
				return this.ucAsignacionLlantasUI.RefaccionActivo;
			}
			set
			{
				this.ucAsignacionLlantasUI.RefaccionActivo = value;
			}
		}
		public DateTime? RefaccionFC
		{
			get
			{
				return this.ucAsignacionLlantasUI.RefaccionFC;
			}
			set
			{
				this.ucAsignacionLlantasUI.RefaccionFC = value;
			}
		}
		public DateTime? RefaccionFUA
		{
			get
			{
				return this.ucAsignacionLlantasUI.RefaccionFUA;
			}
			set
			{
				this.ucAsignacionLlantasUI.RefaccionFUA = value;
			}
		}
		public int? RefaccionUC
		{
			get
			{
				return this.ucAsignacionLlantasUI.RefaccionUC;
			}
			set
			{
				this.ucAsignacionLlantasUI.RefaccionUC = value;
			}
		}
		public int? RefaccionUUA
		{
			get
			{
				return this.ucAsignacionLlantasUI.RefaccionUUA;
			}
			set
			{
				this.ucAsignacionLlantasUI.RefaccionUUA = value;
			}
		}

		//Página 5
		public List<EquipoAliadoBO> EquiposAliados
		{
			get
			{
				return this.ucAsignacionEquiposAliadosUI.EquiposAliados;
			}
			set
			{
				this.ucAsignacionEquiposAliadosUI.EquiposAliados = value;
			}
		}

        //RQM 14150
        public List<CatalogoBaseBO> ListaAcciones { get; set; }

	    public List<UnidadBO> lstUnidades
	    {
	        get
	        {
                return (List<UnidadBO>)Session["unidades"];
	        }
	        set	{
                Session["unidades"] = value;
	        }
	    }


        //RQM 13285
        /// <summary>
        /// Lista de archivos relacionados con la unidad
        /// </summary>
        public List<ArchivoBO> Archivos
        {
            get
            {
                if (this.ucTramitesActivosUI.Archivos != null)
                    return this.ucTramitesActivosUI.Archivos;
                else
                    return new List<ArchivoBO>();
            }
            set { this.ucTramitesActivosUI.Archivos = value; }
        }

        /// <summary>
        /// Lista de archivos relacionados con la unidad
        /// </summary>
        public List<ArchivoBO> ArchivosOC
        {
            get
            {
                if (this.ucDatosGeneralesUI.ArchivosOC != null)
                    return this.ucDatosGeneralesUI.ArchivosOC;
                else
                    return new List<ArchivoBO>();
            }
            set { this.ucDatosGeneralesUI.ArchivosOC = value; }
        }

        /// <summary>
        /// Archivos originales relacionados con la unidad.
        /// </summary>
        public List<ArchivoBO> UltimoObjetoArchivoOC
        {
            get
            {
                if ((List<ArchivoBO>)Session["UltimaLstArchivos"] == null)
                    return new List<ArchivoBO>();
                else
                    return (List<ArchivoBO>)Session["UltimaLstArchivos"];
            }
            set
            {
                Session["UltimaLstArchivos"] = value;
            }
        }

	    //Administración del Wizard
		public int? PaginaActual
		{
			get
			{
				int? id = null;
				if (!String.IsNullOrEmpty(this.hdnPaginaActual.Value))
					id = int.Parse(this.hdnPaginaActual.Value.Trim());
				return id;
			}
		}

        /// <summary>
        /// Indica la Unidad Operativa que contiene los permisos de modificaciones en la UI, su valor default es Idealease.
        /// </summary>
        public ETipoEmpresa EmpresaConPermiso
        {
            get
            {
                int val;
                if (!string.IsNullOrEmpty(this.hdnEmpresaConPermiso.Value) && !string.IsNullOrWhiteSpace(this.hdnEmpresaConPermiso.Value))
                    if (Int32.TryParse(this.hdnEmpresaConPermiso.Value, out val))
                        return (ETipoEmpresa)val;
                return ETipoEmpresa.Idealease;
            }
            set
            {
                this.hdnEmpresaConPermiso.Value = ((int)value).ToString();
            }
        }

        /// <summary>
        /// Contiene los índices de los Tabs que serán ocultados.
        /// </summary>
        public string ValoresTabs
        {
            get
            {
                string Valor = string.Empty;
                if (!string.IsNullOrEmpty(this.hdnValoresTab.Value))
                    Valor = this.hdnValoresTab.Value.Trim().ToUpper();
                return Valor;

            }
            set
            {
                if (value != null)
                    this.hdnValoresTab.Value = value.ToString();
                else
                    this.hdnValoresTab.Value = string.Empty;
            }
        }

        #region Propiedades Visor
        public string ViewState_Guid
		{
			get
			{
				if (ViewState["GuidSession"] == null)
				{
					Guid guid = Guid.NewGuid();
					ViewState["GuidSession"] = guid.ToString();
				}
				return ViewState["GuidSession"].ToString();
			}
		}
		protected object Session_BOSelecto
		{
			get
			{
				object objeto = null;
				string nombreSession = String.Format("BOSELECTO_{0}", ViewState_Guid);
				if (Session[nombreSession] != null)
					objeto = (Session[nombreSession]);

				return objeto;
			}
			set
			{
				string nombreSession = String.Format("BOSELECTO_{0}", ViewState_Guid);
				if (value != null)
					Session[nombreSession] = value;
				else
					Session.Remove(nombreSession);
			}
		}
		protected object Session_ObjetoBuscador
		{
			get
			{
				object objeto = null;
				if (Session[ViewState_Guid] != null)
					objeto = (Session[ViewState_Guid]);

				return objeto;
			}
			set
			{
				if (value != null)
					Session[ViewState_Guid] = value;
				else
					Session.Remove(ViewState_Guid);
			}
		}
		public ECatalogoBuscador ViewState_Catalogo
		{
			get
			{
				return (ECatalogoBuscador)ViewState["BUSQUEDA"];
			}
			set
			{
				ViewState["BUSQUEDA"] = value;
			}
		}
		#endregion Propiedades Buscador
		#endregion

		#region Constructores
		protected void Page_Load(object sender, EventArgs e)
		{
			try
			{
                //SC0006 los cambios de la solicitud de cambio se reflejan en el UserContro ucResumenActaNacimientoUI y en el presentador asociado
				this.presentador = new DetalleActaNacimientoPRE(this, this.ucDatosGeneralesUI, this.ucDatosTecnicosUI, this.ucNumerosSerieUI, this.ucAsignacionLlantasUI, this.ucAsignacionEquiposAliadosUI, this.ucTramitesActivosUI, this.ucResumenActaNacimientoUI, this.ucActaNacimientoUI);

				if (!this.IsPostBack)
				{
					//Se valida el acceso a la vista
					this.presentador.ValidarAcceso(); //SC_0008
					this.presentador.RealizarPrimeraCarga();
				}
			    else
				{
				    //RQM 14150, cada vez que que haga PostBack se debe de aplicar las acciones
                    this.EstablecerAcciones();
				}
			}
			catch (Exception ex)
			{
				this.MostrarMensaje("Inconsistencia al crear la página", ETipoMensajeIU.ERROR, this.nombreClase + ".Page_Load:" + ex.Message);
			}
		}
		#endregion

		#region Métodos

        #region RQM 14150
        /// <summary>
        /// Método implementado desde la vista IDetalleActaNacimientoVIS, que se encarga buscar dentro del archivo de recursos las etiquetas y 
        /// configuraciones de cada unidad operativa.
	    /// </summary>
	    public void EstablecerAcciones()
        {
            string Valor = string.Empty;
            string RecibeValor = string.Empty;
            string ValoresTabsAuxiliar = string.Empty;

            #region EtiquetasPrincipales

            //Se cambia el valor de las etiquetas.
            Valor = string.Empty;
            RecibeValor = ObtenerConfiguracionResource("RE01", true);
            if (!string.IsNullOrEmpty(RecibeValor))
                Valor = RecibeValor + ",";

            RecibeValor = string.Empty;
            RecibeValor = ObtenerConfiguracionResource("RE02", true);
            if (!string.IsNullOrEmpty(RecibeValor))
                Valor += RecibeValor + ",";

            RecibeValor = string.Empty;
            RecibeValor = ObtenerConfiguracionResource("RE03", true);
            if (!string.IsNullOrEmpty(RecibeValor))
                Valor += RecibeValor + ",";

            RecibeValor = string.Empty;
            RecibeValor = ObtenerConfiguracionResource("RE04", true);
            if (!string.IsNullOrEmpty(RecibeValor))
                Valor += RecibeValor + ",";

            RecibeValor = string.Empty;
            RecibeValor = ObtenerConfiguracionResource("RE05", true);
            if (!string.IsNullOrEmpty(RecibeValor))
                Valor += RecibeValor + ",";

            RecibeValor = string.Empty;
            RecibeValor = ObtenerConfiguracionResource("RE07", true);
            if (!string.IsNullOrEmpty(RecibeValor))
                Valor += RecibeValor + ",";

            RecibeValor = string.Empty;
            RecibeValor = ObtenerConfiguracionResource("RE08", true);
            if (!string.IsNullOrEmpty(RecibeValor))
                Valor += RecibeValor + ",";

            RecibeValor = string.Empty;
            RecibeValor = ObtenerConfiguracionResource("RE09", true);
            if (!string.IsNullOrEmpty(RecibeValor))
                Valor += RecibeValor + ",";

            RecibeValor = string.Empty;
            RecibeValor = ObtenerConfiguracionResource("RE12", true);
            if (!string.IsNullOrEmpty(RecibeValor))
                Valor += RecibeValor + ",";

            Valor.TrimEnd(',');
            #endregion

            #region Se obtiene la configuracion de los Tabs
            ValoresTabsAuxiliar = string.Empty;
            if (!this.ValoresTabs.Contains("1"))
            {
                if (ObtenerConfiguracionResource("RE28", false) != "1")
                    ValoresTabsAuxiliar = "1,";
            }
            else
                ValoresTabsAuxiliar = "1,";

            if (!this.ValoresTabs.Contains("2"))
            {
                if (ObtenerConfiguracionResource("RE29", false) != "1")
                    ValoresTabsAuxiliar += "2,";
            }
            else
                ValoresTabsAuxiliar += "2,";

            if (!this.ValoresTabs.Contains("3"))
            {
                if (ObtenerConfiguracionResource("RE30", false) != "1")
                    ValoresTabsAuxiliar += "3,";
            }
            else
                ValoresTabsAuxiliar += "3,";

            if (!this.ValoresTabs.Contains("4"))
            {
                if (ObtenerConfiguracionResource("RE31", false) != "1")
                    ValoresTabsAuxiliar += "4,";
            }
            else
                ValoresTabsAuxiliar += "4,";

            ValoresTabsAuxiliar = ValoresTabsAuxiliar.TrimEnd(',');
            this.ValoresTabs = ValoresTabsAuxiliar;
            #endregion

            bool Habilitar = false;
            ScriptManager.RegisterStartupScript(Page, typeof(Page), "HabilitaFechasDesflote", "HabilitaFechasDesflote('" + Habilitar + "');", true);
            this.RegistrarScript("Inicio", "InicializarControlesEmpresas('" + this.ValoresTabs + "','" + Valor.TrimEnd(',') + "');");
	    }

        /// <summary>
        /// Método que obtiene la configuración de una etiqueta desde el archivo resource correspondiente a la unidad operativa a la cual accedió el usuario.
        /// </summary>
        /// <param name="etiquetaBuscar">Nombre de la etiqueta que será buscada en el archivo resource</param>
        /// <param name="esEtiqueta">Indica sí el valor a obtener es una etiqueta, en caso contrario se considera un TAB o CHECKBOX.</param>
        /// <returns>Retorna la configuración correspondiente al valor recibido en el parámetro etiquetaBuscar del archivo resource.</returns>
        private string ObtenerConfiguracionResource(string etiquetaBuscar, bool esEtiqueta)
        {
            string Configuracion = string.Empty;
            //Instanciamos la clase o webmethod que obtiene las etiquetas
            ObtenerEtiquetaEmpresas obtenerEtiqueta = new ObtenerEtiquetaEmpresas();
            string ConfiguracionObtenida = string.Empty;
            EtiquetaObtenida request = null;

            ConfiguracionObtenida = obtenerEtiqueta.ObtenerEtiquetadelResource(etiquetaBuscar, esEtiqueta ? (int)this.EmpresaConPermiso : (int)this.UnidadOperativaId);
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
            else
            {
                this.MostrarMensaje("Inconsistencia al buscar la etiqueta en el archivo de recursos", ETipoMensajeIU.ERROR, nombreClase + " ObtenerConfiguracionResource " + request.cMensaje);
            }
            return Configuracion;
        }

	    /// <summary>
	    /// Método implementado desde la vista IDetalleActaNacimientoVIS,
	    /// que se encarga validar que botones se habilitarán o no
	    /// </summary>
        /// <param name="habilitado">Valor de booleano que determina el comportamiento del botón</param>
        /// <param name="boton">botón a habilitar o no</param>
	    public void HabilitaBoton(bool habilitado, string boton)
	    {
            switch (boton)
	        {
	            case "C":
	                this.btnContinuar.Enabled        = habilitado;
	                break;
	            case "H":
	                this.btnHistorial.Enabled        = habilitado;
	                break;
	            case "E":
	                this.btnEditar.Enabled           = habilitado;
	                break;
	            case "A":
	                this.btnAnterior.Enabled         = habilitado;
	                break;
	            case "AC":
	                this.btnActualizarOracle.Enabled = habilitado;
	                break;
                default:
                    break;
	        }
	    }

	    /// <summary>
	    /// Método implementado desde la vista IDetalleActaNacimientoVIS,
	    /// que se encarga validar si el botón de actualizar tiene permisos, de lo contrario lo inhabilita
	    /// </summary>
        /// <param name="actualizar">Valor de booleano que determina si el botón debe ser o no habilitado</param>
        public void PermitirActualizar(bool actualizar)
	    {
            this.HabilitaBoton(actualizar, "AC");
	    }

        #endregion
	    public void PrepararVisualizacion()
		{
			this.txtEstaticoAnio.Enabled = false;
			this.txtEstaticoClaveOracle.Enabled = false;
			this.txtEstaticoFechaCompra.Enabled = false;
			this.txtEstaticoIDLeader.Enabled = false;
			this.txtEstaticoModelo.Enabled = false;
			this.txtEstaticoMontoFactura.Enabled = false;
			this.txtEstaticoNumEconomico.Enabled = false;
			this.txtEstaticoNumSerie.Enabled = false;
			this.txtEstaticoTipoUnidad.Enabled = false;
		}

		public object ObtenerDatosNavegacion()
		{
			return Session["UnidadBO"];
		}
		public void EstablecerDatosNavegacion(string nombre, object valor)
		{
			Session[nombre] = valor;
		}

		public void PermitirRegresar(bool habilitar)
		{
			this.btnAnterior.Enabled = habilitar;
		}
		public void PermitirContinuar(bool habilitar)
		{
			this.btnContinuar.Enabled = habilitar;
		}
		public void PermitirRedirigirAEdicion(bool habilitar)
		{
			this.btnEditar.Enabled = habilitar;
		}
		public void PermitirConsultarHistorial(bool habilitar)
		{
			this.btnHistorial.Enabled = habilitar;
		}

		public void MostrarActaNacimientoOriginal(bool mostrar)
		{
			this.btnActaOriginal.Visible = mostrar;
		}

		public void RedirigirAConsulta()
		{
			this.Response.Redirect(this.ResolveUrl("~/Equipos.UI/ConsultarActaNacimientoUI.aspx"));
		}
		public void RedirigirAEdicion()
		{
			this.Response.Redirect(this.ResolveUrl("~/Equipos.UI/EditarActaNacimientoUI.aspx"));
		}
		public void LimpiarSesion()
		{
			if (Session["UnidadBO"] != null)
				this.Session.Remove("UnidadBO");
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
				this.hdnTipoMensaje.Value = ((int)tipo).ToString();
				this.hdnMensaje.Value = mensaje;
			}
			else
			{
				Site masterMsj = (Site)Page.Master;
				if (tipo == ETipoMensajeIU.ERROR)
				{
					if (masterMsj != null)
						masterMsj.MostrarMensaje(mensaje, tipo, msjDetalle);
				}
				else
				{
					if (masterMsj != null)
						masterMsj.MostrarMensaje(mensaje, tipo);
				}
			}
		}

		public void EstablecerPagina(int numeroPagina)
		{
			this.mvCU079.SetActiveView((View)this.mvCU079.FindControl("vwPagina" + numeroPagina.ToString()));
			this.hdnPaginaActual.Value = numeroPagina.ToString();
		}

		#region SC0008
		public void PermitirRegistrar(bool habilitar)
		{
				this.hlkRegistroActaNacimiento.Enabled = habilitar;
		}
		public void RedirigirSinPermisoAcceso()
		{
			this.Response.Redirect(this.ResolveUrl("~/MapaSitio.UI/PaginaSinAccesoUI.aspx"));
		}
		#endregion

		#region Métodos para el Visor
		/// <summary>
		/// Ejecuta el visor
		/// </summary>
		/// <param name="catalogo">Nombre del xml</param>
		/// <param name="tipoBusqueda">Nombre catalogo</param>
		private void EjecutaVisor(string catalogo, ECatalogoBuscador catalogoBusqueda)
		{
			ViewState_Catalogo = catalogoBusqueda;
			this.Session_ObjetoBuscador = presentador.PrepararBOVisor(catalogoBusqueda.ToString());
			this.Session_BOSelecto = null;
			this.RegistrarScript("Events", "BtnVisualizar('" + ViewState_Guid + "','" + catalogo + "');");
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
		#endregion

		#region Eventos
		protected void btnBrincarPagina_Click(object sender, EventArgs e)
		{
			try
			{
				int numeroPagina = int.Parse(this.hdnPaginaBrinco.Value);
				this.presentador.IrAPagina(numeroPagina);
			}
			catch (Exception ex)
			{
				this.MostrarMensaje("Inconsistencia al cambiar de página", ETipoMensajeIU.ERROR, this.nombreClase + ".btnBrincarPagina_Click:" + ex.Message);
			}
		}

		protected void btnContinuar_Click(object sender, EventArgs e)
		{
			try
			{
				this.presentador.AvanzarPagina();
			}
			catch (Exception ex)
			{
				this.MostrarMensaje("Inconsistencia al cambiar de página", ETipoMensajeIU.ERROR, this.nombreClase + ".btnContinuar_Click:" + ex.Message);
			}
		}
		protected void btnAnterior_Click(object sender, EventArgs e)
		{
			try
			{
				this.presentador.RetrocederPagina();
			}
			catch (Exception ex)
			{
				this.MostrarMensaje("Inconsistencia al cambiar de página", ETipoMensajeIU.ERROR, this.nombreClase + ".btnAnterior_Click:" + ex.Message);
			}
		}
		protected void btnEditar_Click(object sender, EventArgs e)
		{
			try
			{
				this.presentador.Editar();
			}
			catch (Exception ex)
			{
				this.MostrarMensaje("Inconsistencia al cambiar a edición", ETipoMensajeIU.ERROR, this.nombreClase + ".btnEditar_Click:" + ex.Message);
			}
		}

		#region Eventos para el Visor
		protected void btnHistorial_Click(object sender, EventArgs e)
		{
			try
			{
				this.EjecutaVisor("HistorialUnidad&hidden=0", ECatalogoBuscador.HistorialUnidad);
			}
			catch (Exception ex)
			{
				this.MostrarMensaje("Inconsistencia al buscar Historial de Unidad", ETipoMensajeIU.ADVERTENCIA, nombreClase + ".btnHistorial_Click" + ex.Message);
			}
		}
		#endregion

	    #region Evento para actualizar los datos de Oracle

	    /// <summary>
	    /// Botón que actualiza dos datos de Oracle en eRental
	    /// </summary>
        /// <param name="sender">objeto que dispara el evento</param>
        /// <param name="e">evento</param>
        protected void btnActualizarOracle_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.presentador.ActualizarDatosActivoFijo()){
                    this.MostrarMensaje("Se actualizaron los datos del activo fijo correctamente", ETipoMensajeIU.CONFIRMACION, nombreClase + ".btnActualizarOracle_Click");
                }
                else{
                    this.MostrarMensaje("Inconsistencia al actualizar los datos de activo fijo", ETipoMensajeIU.ADVERTENCIA, nombreClase + ".btnActualizarOracle_Click no se actualizo correctamente");
                }
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencia al actualizar los datos de activo fijo " + ex.Message, ETipoMensajeIU.ADVERTENCIA, nombreClase + ".btnActualizarOracle_Click" + ex.Message);
            }
        }
	    #endregion
        #endregion
    }
}
