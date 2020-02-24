// Satisface al CU080 – Editar Acta de Nacimiento de una Unidad
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
    public partial class EditarActaNacimientoUI : System.Web.UI.Page, IEditarActaNacimientoVIS
    {
        #region Atributos
        private EditarActaNacimientoPRE presentador;
        private string nombreClase = "EditarActaNacimientoUI";
        #endregion

        #region Propiedades
        public UnidadBO UltimoObjeto
        {
            get
            {
                if ((UnidadBO)Session["LastUnidad"] == null)
                    return new UnidadBO();
                else
                    return (UnidadBO)Session["LastUnidad"];
            }
            set
            {
                Session["LastUnidad"] = value;
            }
        }

        //Página 0
        public int? EquipoId
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
        public int? UnidadId
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
            get
            {
                DateTime? temp = null;
                if (!String.IsNullOrEmpty(this.hdnFC.Value))
                    temp = DateTime.Parse(this.hdnFC.Value.Trim());
                return temp;
            }
            set
            {
                if (value != null)
                    this.hdnFC.Value = value.ToString();
                else
                    this.hdnFC.Value = string.Empty;
            }
        }
        public DateTime? FUA
        {
            get { return DateTime.Now; }
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
                Site masterMsj = (Site)Page.Master;

                if (masterMsj.Usuario != null)
                    id = masterMsj.Usuario.Id;
                return id;
            }
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

        public string FabricanteNombre
        {
            get
            {
                return this.ucDatosGeneralesUI.Fabricante;
            }
            set
            {
                this.ucDatosGeneralesUI.Fabricante = value;
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
                    temp = decimal.Parse(this.txtEstaticoMontoFactura.Text.Trim().Replace(",", ""));
                return temp;
            }
            set
            {
                if (value != null)
                    this.txtEstaticoMontoFactura.Text = string.Format("{0:#,#00.0000}", value);
                else
                    this.txtEstaticoMontoFactura.Text = string.Empty;
            }
        }

        public string ClaveOracleUnidad
        {
            get
            {
                string valor = null;
                if (this.hdnOracleId.Value.Trim().Length > 0)
                    valor = this.hdnOracleId.Value.Trim().ToUpper();
                return valor;

            }
            set
            {
                if (value != null)
                    this.hdnOracleId.Value = value.ToString();
                else
                    this.hdnOracleId.Value = string.Empty;
            }
        }
        public string ClaveOracleUnidadLider
        {
            get
            {
                string valor = null;
                if (this.hdnOracleIdLider.Value.Trim().Length > 0)
                    valor = this.hdnOracleIdLider.Value.Trim().ToUpper();
                return valor;

            }
            set
            {
                if (value != null)
                    this.hdnOracleIdLider.Value = value.ToString();
                else
                    this.hdnOracleIdLider.Value = string.Empty;
            }
        }




        #region SC0002

        private string mensaje;

        public string Mensaje
        {
            get
            {
                mensaje = LabelError.Text;
                return mensaje;
            }
            set
            {
                LabelError.Text = value;
            }
        }

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

        private GridView gridAliados;

        public GridView GridAliados
        {
            get
            {
                gridAliados = this.ucAsignacionEquiposAliadosUI.GridAliados;
                return gridAliados;
            }
            set
            {
                this.ucAsignacionEquiposAliadosUI.GridAliados = value;
            }
        }


        #endregion


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
        public int? UnidadOperativaId
        {
            get
            {
                int? id = null;
                Site masterMsj = (Site)Page.Master;

                if (masterMsj.Adscripcion != null && masterMsj.Adscripcion.UnidadOperativa != null)
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
                this.ucAsignacionLlantasUI.SucursalEnllantableID = value;
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

        #region REQ. 13285 Acta de nacimiento.
        //Datos generales, generación y construcción
        /// <summary>
        /// Identificador de arrendamiento
        /// </summary>
        public int? ArrendamientoId
        {
            get
            {
                int? id = null;
                if (!String.IsNullOrEmpty(this.hdnArrendamientoId.Value))
                    id = int.Parse(this.hdnArrendamientoId.Value.Trim());
                return id;
            }
            set
            {
                if (value != null)
                    this.hdnArrendamientoId.Value = value.ToString();
                else
                    this.hdnArrendamientoId.Value = string.Empty;
            }
        }

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
        /// Fecha de inicio del arrendamiento nuevo
        /// </summary>
        public DateTime? FechaInicioArrendamientoNuevo
        {
            get { return this.ucDatosGeneralesUI.FechaInicioArrendamientoNuevo; }
            set { this.ucDatosGeneralesUI.FechaInicioArrendamientoNuevo = value; }
        }

        /// <summary>
        /// Fecha de fin del arrendamiento nuevo
        /// </summary>
        public DateTime? FechaFinArrendamientoNuevo
        {
            get { return this.ucDatosGeneralesUI.FechaFinArrendamientoNuevo; }
            set { this.ucDatosGeneralesUI.FechaFinArrendamientoNuevo = value; }
        }

        /// <summary>
        /// Identificador del proveedor
        /// </summary>
        public int? ProveedorID
        {
            get { return this.ucDatosGeneralesUI.ProveedorID; }
            set { this.ucDatosGeneralesUI.ProveedorID = value; }
        }

        #endregion

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

        public string Nombre
        {
            get
            {
                return this.ucNumerosSerieUI.Nombre;
            }
            set
            {
                this.ucNumerosSerieUI.Nombre = value;
            }
        }


        public string Serie
        {
            get
            {
                return this.ucNumerosSerieUI.Serie;
            }
            set
            {
                this.ucNumerosSerieUI.Serie = value;
            }
        }


        public List<NumeroSerieBO> NumerosSerie
        {
            get
            {
                return this.ucNumerosSerieUI.NumerosSerie;
            }
            set
            {
                this.ucNumerosSerieUI.NumerosSerie = value;
            }
        }

        #region SC0030
        public string SerieMotor
        {
            get
            {
                return this.ucNumerosSerieUI.SerieMotor;
            }
            set
            {
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
        public int? RefaccionSucursalID
        {
            get
            {
                return this.ucAsignacionLlantasUI.RefaccionSucursalID;
            }
            set
            {
                this.ucAsignacionLlantasUI.RefaccionSucursalID = value;
            }
        }
        public string RefaccionSucursalNombre
        {
            get
            {
                return this.ucAsignacionLlantasUI.RefaccionSucursalNombre;
            }
            set
            {
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

        //Página 6
        /// <summary>
        /// Identificador del tramite pedimento
        /// </summary>
        public int? PedimentoId
        {
            get
            {
                int? id = null;
                if (!String.IsNullOrEmpty(this.hdnPedimentoId.Value))
                    id = int.Parse(this.hdnPedimentoId.Value.Trim());
                return id;
            }
            set
            {
                if (value != null)
                    this.hdnPedimentoId.Value = value.ToString();
                else
                    this.hdnPedimentoId.Value = string.Empty;
            }
        }

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
        /// <summary>
        /// Numero de pedimento
        /// </summary>
        public string NumeroPedimento
        {
            get { return this.ucTramitesActivosUI.NumeroPedimento; }
            set { this.ucTramitesActivosUI.NumeroPedimento = value; }
        }

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
        /// Lista de archivos en datos generales
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

        //Administración de Seguridad
        public List<int?> SucursalesSeguridad
        {
            get
            {
                return this.ucAsignacionEquiposAliadosUI.SucursalesSeguridad;
            }
            set
            {
                this.ucAsignacionEquiposAliadosUI.SucursalesSeguridad = value;
            }
        }

        //Configuraciones
        /// <summary>
        /// Identificador del Modulo desde el cual se esta accesando
        /// </summary>
        public int? ModuloID
        {
            get
            {
                return ((Site)Page.Master).ModuloID;
            }
        }
        /// <summary>
        /// Nombre del cliente correspondiente a la unidad operativa
        /// </summary>
        public string NombreClienteUnidadOperativa
        {
            get { return this.ucDatosGeneralesUI.NombreClienteUnidadOperativa; }
            set { this.ucDatosGeneralesUI.NombreClienteUnidadOperativa = value; }
        }
        /// <summary>
        /// Configuración de la unidad operativa que indica a qué libro corresponden los activos
        /// </summary>
        public string LibroActivos
        {
            get
            {
                string valor = null;
                if (this.hdnLibroActivos.Value.Trim().Length > 0)
                    valor = this.hdnLibroActivos.Value.Trim().ToUpper();
                return valor;

            }
            set
            {
                if (value != null)
                    this.hdnLibroActivos.Value = value.ToString();
                else
                    this.hdnLibroActivos.Value = string.Empty;
            }
        }

        #region REQ 13285 Acta de Nacimiento.

        /// <summary>
        /// Contiene la lista de acciones a las cuales tiene acceso el usuario.
        /// </summary>
        public List<CatalogoBaseBO> ListaAcciones { get; set; }

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

        #endregion
        #endregion

        #region Constructores
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                this.presentador = new EditarActaNacimientoPRE(this, this.ucDatosGeneralesUI, this.ucDatosTecnicosUI, this.ucNumerosSerieUI, this.ucAsignacionLlantasUI, this.ucAsignacionEquiposAliadosUI, this.ucTramitesActivosUI, this.ucResumenActaNacimientoUI);

                if (!this.IsPostBack)
                {
                    //Se valida el acceso a la vista
                    this.presentador.ValidarAcceso();//SC0008
                    this.presentador.RealizarPrimeraCarga();
                }
                else
                {
                    //RQM 13285, cada vez que que haga PostBack se debe de aplicar las acciones de visualización.
                    this.EstablecerAcciones();
                }
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencia al crear de página", ETipoMensajeIU.ERROR, this.nombreClase + ".Page_Load:" + ex.Message);
            }
        }
        protected void Page_PreRender(object sender, EventArgs e)
        {
            this.presentador.EstablecerConfiguracionEspecialVista();
        }
        #endregion

        #region Métodos
        public void PrepararEdicion()
        {
            this.ucDatosGeneralesUI.UsuarioAutenticado = this.UUA;
            this.ucAsignacionLlantasUI.UsuarioAutenticado = this.UUA;
            this.ucAsignacionEquiposAliadosUI.UsuarioAutenticado = this.UUA;
        }

        public object ObtenerDatosNavegacion()
        {
            return Session["UnidadBO"];
        }

        public void PermitirRegresar(bool habilitar)
        {
            this.btnAnterior.Enabled = habilitar;
        }
        public void PermitirContinuar(bool habilitar)
        {
            this.btnContinuar.Enabled = habilitar;
        }
        public void PermitirCancelar(bool habilitar)
        {
            this.btnCancelar.Enabled = habilitar;
        }
        public void PermitirGuardarBorrador(bool habilitar)
        {
            this.btnBorrador.Enabled = habilitar;
        }
        public void PermitirGuardarTerminada(bool habilitar)
        {
            this.btnTerminar.Enabled = habilitar;
        }

        public void OcultarContinuar(bool ocultar)
        {
            this.btnContinuar.Visible = !ocultar;
        }
        public void OcultarTerminar(bool ocultar)
        {
            this.btnTerminar.Visible = !ocultar;
        }
        public void OcultarBorrador(bool ocultar)
        {
            this.btnBorrador.Visible = !ocultar;
        }

        public void RedirigirAConsulta()
        {
            this.Response.Redirect(this.ResolveUrl("~/Equipos.UI/ConsultarActaNacimientoUI.aspx"));
        }
        public void LimpiarSesion()
        {
            if (Session["LastUnidad"] != null)
                Session.Remove("LastUnidad");
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

                #region SC_0027
                string botonID = "";
                switch (msjDetalle)
                {
                    case "BORRADOR":
                        botonID = this.btnBorrador.UniqueID;
                        break;
                    case "TERMINAR":
                        botonID = this.btnTerminar.UniqueID;
                        break;
                }

                this.RegistrarScript("Confirm", "abrirConfirmacion('" + mensaje + "', '" + botonID + "');");
                #endregion
            }
            else
            {
                Site masterMsj = (Site)Page.Master;
                if (tipo == ETipoMensajeIU.ERROR)
                    masterMsj.MostrarMensaje(mensaje, tipo, msjDetalle);
                else
                    masterMsj.MostrarMensaje(mensaje, tipo);
            }
        }

        private void RegistrarScript(string key, string script)
        {
            ScriptManager.RegisterStartupScript(Page, typeof(Page), key, script, true);
        }

        public void EstablecerPagina(int numeroPagina)
        {
            this.mvCU080.SetActiveView((View)this.mvCU080.FindControl("vwPagina" + numeroPagina.ToString()));
            this.hdnPaginaActual.Value = numeroPagina.ToString();
        }

        public void RegistrarScriptMensaje(string key, string script)
        {
            ScriptManager.RegisterStartupScript(Page, typeof(Page), key, script, true);
        }

        #region SC0008
        public void RedirigirSinPermisoAcceso()
        {
            this.Response.Redirect(this.ResolveUrl("~/MapaSitio.UI/PaginaSinAccesoUI.aspx"));
        }

        public void PermitirRegistrar(bool habilitar)
        {
            this.hlkRegistroActaNacimiento.Enabled = habilitar;
        }
        #endregion

        #region REQ 13285 Acta de Nacimiento.

        /// <summary>
        /// Prepara los controles (etiquetas y visualización) que serán válidos para la unidad a la cual el usuario accedió.
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
            this.RegistrarScript("Inicio", "InicializarControlesEmpresas('" + ValoresTabsAuxiliar + "','" + Valor + "');");


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

        #endregion
        #endregion

        #region Eventos
        protected void btnNuevaConsulta_Click(object sender, EventArgs e)
        {
            try
            {
                this.presentador.CancelarEdicion();
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencia al cancelar la edición", ETipoMensajeIU.ERROR, this.nombreClase + ".btnNuevaConsulta_Click:" + ex.Message);
            }
        }
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
        protected void btnBorrador_Click(object sender, EventArgs e)
        {
            try
            {
                int paquete = this.presentador.EditarBorrador();
                if (paquete == 1)
                {
                    string origen = ((Button)sender).ID;
                    this.RegistrarScript("Advertencia", "confirmarPaqueteServicio('" + origen + "');");
                }

            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencia al guardar el borrador del acta de nacimiento", ETipoMensajeIU.ERROR, this.nombreClase + ".btnBorrador_Click:" + ex.Message);
            }
        }
        protected void btnTerminar_Click(object sender, EventArgs e)
        {
            try
            {
                int paquete = this.presentador.EditarTerminada();
                if (paquete == 1)
                {
                    string origen = ((Button)sender).ID;
                    this.RegistrarScript("Advertencia", "confirmarPaqueteServicio('" + origen + "');");
                }
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencia al guardar el acta de nacimiento finalizada", ETipoMensajeIU.ERROR, this.nombreClase + ".btnTerminar_Click:" + ex.Message);
            }
        }
        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            try
            {
                this.presentador.CancelarEdicion();
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencia al cancelar la edición", ETipoMensajeIU.ERROR, this.nombreClase + ".btnCancelar_Click:" + ex.Message);
            }
        }
        #endregion
    }
}