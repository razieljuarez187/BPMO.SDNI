//Satisface al CU077 - Registrar Acta de Nacimiento de una Unidad
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
    public partial class RegistrarActaNacimientoUI : System.Web.UI.Page, IRegistrarActaNacimientoVIS
    {
        #region Atributos
        private RegistrarActaNacimientoPRE presentador;
        private string nombreClase = "RegistrarActaNacimientoUI";
        //13597, variable global para instanciar las etiquetas que se obtendrán a través del archivo de recursos.
        private ObtenerEtiquetaEmpresas obtenerEtiqueta = null;

        public enum ECatalogoBuscador
        {
            Unidad,
            Marca,
            Modelo,
            TipoUnidad,
            Distribuidor,
            Motorizacion,
            Aplicacion
        }
        #endregion

        #region Propiedades
        //Manejo del buscador en la página principal
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
                    objeto = (Session[nombreSession] as object);

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
                    objeto = (Session[ViewState_Guid] as object);

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
                return  id;
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
            get { return DateTime.Now; }
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
                return (String.IsNullOrEmpty(this.txtUnidad.Text)) ? null : this.txtUnidad.Text.Trim().ToUpper();
            }
            set
            {
                if (value != null)
                    this.txtUnidad.Text = value;
                else
                    this.txtUnidad.Text = string.Empty;

                this.txtEstaticoNumSerie.Text = this.txtUnidad.Text;
            }
        }
        public string ClaveActivoOracle
        {
            get
            {
                return (String.IsNullOrEmpty(this.txtClaveActivoOracle.Text)) ? null : this.txtClaveActivoOracle.Text.Trim().ToUpper();
            }
            set
            {
                if (value != null)
                    this.txtClaveActivoOracle.Text = value;
                else
                    this.txtClaveActivoOracle.Text = string.Empty;

                this.txtEstaticoClaveOracle.Text = this.txtClaveActivoOracle.Text;
            }
        }
        public int? LiderID
        {
            get
            {
                int? id = null;
                if (!String.IsNullOrEmpty(this.txtLiderID.Text))
                    id = int.Parse(this.txtLiderID.Text.Trim());
                return id;
            }
            set
            {
                if (value != null)
                    this.txtLiderID.Text = value.ToString();
                else
                    this.txtLiderID.Text = string.Empty;

                this.txtEstaticoIDLeader.Text = this.txtLiderID.Text;
            }
        }
        public string NumeroEconomico
        {
            get
            {
                return (String.IsNullOrEmpty(this.txtNumeroEconomico.Text)) ? null : this.txtNumeroEconomico.Text.Trim().ToUpper();
            }
            set
            {
                if (value != null)
                    this.txtNumeroEconomico.Text = value;
                else
                    this.txtNumeroEconomico.Text = string.Empty;

                this.txtEstaticoNumEconomico.Text = this.txtNumeroEconomico.Text;
            }
        }

        public string TipoUnidadNombre
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

                this.txtEstaticoTipoUnidad.Text = this.txtTipoUnidad.Text;
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

        public string MarcaNombre
        {
            get
            {
                return (String.IsNullOrEmpty(this.txtMarca.Text)) ? null : this.txtMarca.Text.Trim().ToUpper();
            }
            set
            {
                if (value != null)
                    this.txtMarca.Text = value;
                else
                    this.txtMarca.Text = string.Empty;
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
                return (String.IsNullOrEmpty(this.txtModelo.Text)) ? null : this.txtModelo.Text.Trim().ToUpper();
            }
            set
            {
                if (value != null)
                    this.txtModelo.Text = value;
                else
                    this.txtModelo.Text = string.Empty;

                this.txtEstaticoModelo.Text = this.txtModelo.Text;
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

                this.txtEstaticoAnio.Text = this.txtAnio.Text;
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
                    this.txtFechaCompra.Text = value.Value.ToShortDateString();
                else
                    this.txtFechaCompra.Text = string.Empty;

                this.txtEstaticoFechaCompra.Text = this.txtFechaCompra.Text;
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

                this.txtEstaticoMontoFactura.Text = this.txtMontoFactura.Text;
            }
        }

        public string FabricanteNombre
        {
            get
            {
                return (String.IsNullOrEmpty(this.txtDistribuidor.Text)) ? null : this.txtDistribuidor.Text.Trim().ToUpper();
            }
            set
            {
                if (value != null)
                    this.txtDistribuidor.Text = value;
                else
                    this.txtDistribuidor.Text = string.Empty;

                this.ucDatosGeneralesUI.Fabricante = this.txtDistribuidor.Text;
            }
        }
        public int? FabricanteId
        {
            get
            {
                int? id = null;
                if (!String.IsNullOrEmpty(this.hdnDistribuidorID.Value))
                    id = int.Parse(this.hdnDistribuidorID.Value.Trim());
                return id;
            }
            set
            {
                if (value != null)
                    this.hdnDistribuidorID.Value = value.ToString();
                else
                    this.hdnDistribuidorID.Value = string.Empty;
            }
        }
        
        public string MotorizacionNombre
        {
            get
            {
                return (String.IsNullOrEmpty(this.txtMotorizacion.Text)) ? null : this.txtMotorizacion.Text.Trim().ToUpper();
            }
            set
            {
                if (value != null)
                    this.txtMotorizacion.Text = value;
                else
                    this.txtMotorizacion.Text = string.Empty;
            }
        }
        public int? MotorizacionId
        {
            get
            {
                int? id = null;
                if (!String.IsNullOrEmpty(this.hdnMotorizacionID.Value))
                    id = int.Parse(this.hdnMotorizacionID.Value.Trim());
                return id;
            }
            set
            {
                if (value != null)
                    this.hdnMotorizacionID.Value = value.ToString();
                else
                    this.hdnMotorizacionID.Value = string.Empty;
            }
        }

        public string AplicacionNombre
        {
            get
            {
                return (String.IsNullOrEmpty(this.txtAplicacion.Text)) ? null : this.txtAplicacion.Text.Trim().ToUpper();
            }
            set
            {
                if (value != null)
                    this.txtAplicacion.Text = value;
                else
                    this.txtAplicacion.Text = string.Empty;
            }
        }
        public int? AplicacionId
        {
            get
            {
                int? id = null;
                if (!String.IsNullOrEmpty(this.hdnAplicacionID.Value))
                    id = int.Parse(this.hdnAplicacionID.Value.Trim());
                return id;
            }
            set
            {
                if (value != null)
                    this.hdnAplicacionID.Value = value.ToString();
                else
                    this.hdnAplicacionID.Value = string.Empty;
            }
        }

        public DateTime? FechaProximoServicio
        {
            get
            {
                DateTime? temp = null;
                if (!String.IsNullOrEmpty(this.txtProximoServicio.Text))
                    temp = DateTime.Parse(this.txtProximoServicio.Text.Trim());
                return temp;
            }
            set
            {
                if (value != null)
                    this.txtProximoServicio.Text = value.Value.ToShortDateString();
                else
                    this.txtProximoServicio.Text = string.Empty;
            }
        }
        public int? KilometrajeProximoServicio
        {
            get
            {
                int? id = null;
                if (!String.IsNullOrEmpty(this.txtKMProximoServicio.Text))
                    id = int.Parse(this.txtKMProximoServicio.Text.Trim().Replace(",","")); //RI0012
                return id;
            }
            set
            {
                if (value != null)
					this.txtKMProximoServicio.Text = string.Format("{0:#,##0}", value);//RI0012
                else
                    this.txtKMProximoServicio.Text = string.Empty;
            }
        }
        public int? KilometrajeInicial
        {
            get
            {
                int? id = null;
                if (!String.IsNullOrEmpty(this.txtKMInicial.Text))
                    id = int.Parse(this.txtKMInicial.Text.Trim()); //RI0012
                return id;
            }
            set
            {
                if (value != null)
					this.txtKMInicial.Text = value.ToString();//RI0012
                else
                    this.txtKMInicial.Text = string.Empty;
            }
        }

        #region SC0001

        public int? HorasIniciales
        {
            get
            {
                int? id = null;
                if (!String.IsNullOrEmpty(this.txtHRSInicial.Text))
                    id = int.Parse(this.txtHRSInicial.Text.Trim()); //RI0012
                return id;
            }
            set
            {
                if (value != null)
                    this.txtHRSInicial.Text = value.ToString();//RI0012
                else
                    this.txtHRSInicial.Text = string.Empty;
            }
        }

        public int? CombustibleTotal
        {
            get
            {
                int? id = null;
                if (!String.IsNullOrEmpty(this.txtCombustibleTotal.Text))
                    id = int.Parse(this.txtCombustibleTotal.Text.Trim().Replace(",", "")); //RI0012
                return id;
            }
            set
            {
                if (value != null)
                    this.txtCombustibleTotal.Text = value.ToString();//RI0012
                else
                    this.txtCombustibleTotal.Text = string.Empty;
            }
        }
        

        #endregion
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

        #region

        public string OrdenCompraProveedor 
        {
            get { return this.ucDatosGeneralesUI.OrdenCompraProveedor; }
            set { this.ucDatosGeneralesUI.OrdenCompraProveedor = value; }
        }        
        public decimal? MontoArrendamiento 
        {
            get { return this.ucDatosGeneralesUI.MontoArrendamiento; }
            set { this.ucDatosGeneralesUI.MontoArrendamiento = value; }
 
        }
        public string CodigoMoneda
        {
            get { return this.ucDatosGeneralesUI.CodigoMoneda; }
            set { this.ucDatosGeneralesUI.CodigoMoneda = value; }
        }

        public DateTime? FechaInicioArrendamiento
        {
            get { return this.ucDatosGeneralesUI.FechaInicioArrendamiento; }
            set { this.ucDatosGeneralesUI.FechaInicioArrendamiento = value; }
        }

        public DateTime? FechaFinArrendamiento
        {
            get { return this.ucDatosGeneralesUI.FechaFinArrendamiento; }
            set { this.FechaFinArrendamiento = value; }
        }
        
        public int? ProveedorID
        {
            get { return this.ucDatosGeneralesUI.PropietarioId; }
            set { this.ucDatosGeneralesUI.PropietarioId = value; }
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
                return ((Site)this.Master).ModuloID;
            }
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
        /// <summary>
        /// Nombre del cliente correspondiente a la unidad operativa
        /// </summary>
        public string NombreClienteUnidadOperativa
        {
            get { return this.ucDatosGeneralesUI.NombreClienteUnidadOperativa; }
            set { this.ucDatosGeneralesUI.NombreClienteUnidadOperativa = value; }
        }

        #region[REQ:13285, Integración de Generación y Construcción, datos que se obtienen de oracle]
        public List<CatalogoBaseBO> ListaAcciones { get; set; }
        public string ValoresTabs { get; set; }

        /// <summary>
        /// Asignar, obtener el campo accesorio
        /// </summary>
        public bool? Accesorio 
        {
            get { return cbAccesorio.Checked; }
            set { cbAccesorio.Checked = value != null && value.Value; }
        }

        /// <summary>
        /// Asignar, obtener fecha de inicio depreciación
        /// </summary>
        public DateTime? FechaInicioDepreciacion 
        {             
            get
            {
                DateTime? temp = null;
                if (!String.IsNullOrEmpty(this.txtFechaInicioDepreciacion.Text.Trim()))
                    temp = DateTime.Parse(this.txtFechaInicioDepreciacion.Text.Trim());
                return temp;
            }
            set
            {
                if (value != null)
                    this.txtFechaInicioDepreciacion.Text = value.Value.ToShortDateString();
                else
                    this.txtFechaInicioDepreciacion.Text = string.Empty;
            } 
        }

        /// <summary>
        /// Asignar, establecer fecha de desflote
        /// </summary>
        public DateTime? FechaIdealDesflote 
        {
            get
            {
                DateTime? temp = null;
                if (!String.IsNullOrEmpty(this.txtFechaDesflote.Text.Trim()))
                    temp = DateTime.Parse(this.txtFechaDesflote.Text.Trim());
                return temp;
            }
            set
            {
                if (value != null)
                    this.txtFechaDesflote.Text = value.Value.ToShortDateString();
                else
                    this.txtFechaDesflote.Text = string.Empty;
            }
        }

        /// <summary>
        /// Asignar, establecer tasa depreciación
        /// </summary>
        public  decimal? TasaDepreciacion 
        {
            get
            {
                decimal? tempDepreciacion = null;
                if (!String.IsNullOrEmpty(this.txtTasaDepreciacion.Text.Trim()))
                    tempDepreciacion = decimal.Parse(this.txtTasaDepreciacion.Text.Trim().Replace(",", ""));
                return tempDepreciacion;
            }
            set 
            { 
                this.txtTasaDepreciacion.Text = value != null ? String.Format("{0:#,##0.00##}", value) : String.Empty; 
            }
        }

        /// <summary>
        /// Asignar, establecer vida util del equipo
        /// </summary>
        public int? VidaUtil 
        {
            get
            {
                int? tempVidaUtil = null;
                if (!String.IsNullOrEmpty(this.txtVidaUtil.Text.Trim()))
                    tempVidaUtil = int.Parse(this.txtVidaUtil.Text.Trim().Replace(",", ""));
                return tempVidaUtil;
            }
            set 
            { 
                this.txtVidaUtil.Text = value != null ? String.Format("{0:#,##0}", value) : String.Empty; 
            }
        }

        /// <summary>
        /// Asignar, establecer valor del porcentaje del valor residual
        /// </summary>
        public decimal? PorcentajeValorResidual 
        {
            get
            {
                decimal? tempPorcentajeVR = null;
                if (!String.IsNullOrEmpty(this.txtPorcentajeVR.Text.Trim()))
                    tempPorcentajeVR = decimal.Parse(this.txtPorcentajeVR.Text.Trim().Replace(",", ""));
                return tempPorcentajeVR;
            }
            set
            {
                this.txtPorcentajeVR.Text = value != null ? String.Format("{0:#,##0.00##}", value) : String.Empty;
            }
        }

        /// <summary>
        /// Asignar, establecer el valor residual
        /// </summary>
        public decimal? ValorResidual 
        {
            get
            {
                decimal? tempValorResidual = null;
                if (!String.IsNullOrEmpty(this.txtValorResidual.Text.Trim()))
                    tempValorResidual = decimal.Parse(this.txtValorResidual.Text.Trim().Replace(",", ""));
                return tempValorResidual;
            }
            set
            {
                this.txtValorResidual.Text = value != null ? String.Format("{0:#,##0.00##}", value) : String.Empty;
            }
        }

        /// <summary>
        /// Asignar, establecer saldo por depreciar
        /// </summary>
        public decimal? SaldoPorDepreciar 
        {
            get
            {
                decimal? tempSaldoPD = null;
                if (!String.IsNullOrEmpty(this.txtSaldoDepreciar.Text.Trim()))
                    tempSaldoPD = decimal.Parse(this.txtSaldoDepreciar.Text.Trim().Replace(",", ""));
                return tempSaldoPD;
            }
            set
            {
                this.txtSaldoDepreciar.Text = value != null ? String.Format("{0:#,##0.00##}", value) : String.Empty;
            }
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

        public int verTabDT { get; set; }
        public int verTabNS { get; set; }
        public int verTabLL { get; set; }
        public int verTabEA { get; set; }

        #endregion

        #endregion

        #region Constructores
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                this.presentador = new RegistrarActaNacimientoPRE(this, this.ucDatosGeneralesUI, this.ucDatosTecnicosUI, this.ucNumerosSerieUI, this.ucAsignacionLlantasUI, this.ucAsignacionEquiposAliadosUI, this.ucTramitesActivosUI, this.ucResumenActaNacimientoUI);
                
                this.HabilitarControlEquipoAliado();

                if (!this.IsPostBack)
                {
                    this.presentador.PrepararNuevo();
                }
                else
                {
                    this.presentador.ValidarPermisoTab();
                    this.EstablecerAcciones();
                }
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencia al crear la página", ETipoMensajeIU.ERROR, this.nombreClase + ".Page_Load:" + ex.Message);
            }
        }
        protected void Page_PreRender(object sender, EventArgs e)
        {
            this.presentador.EstablecerConfiguracionEspecialVista();
        }
        #endregion

        #region Métodos

        public void HabilitarControlEquipoAliado()
        {
            if (this.EntraMantenimiento == true)
                this.ucAsignacionEquiposAliadosUI.habilitarCheckEntrada = true;
            else if (this.EntraMantenimiento == false)
                this.ucAsignacionEquiposAliadosUI.habilitarCheckEntrada = false;
        }

        public void PrepararNuevo()
        {
            this.txtAnio.Text = "";
            this.txtClaveActivoOracle.Text = "";
            this.txtFechaCompra.Text = "";
            this.txtLiderID.Text = "";
            this.txtMarca.Text = "";
            this.txtModelo.Text = "";
            this.txtMontoFactura.Text = "";
            this.txtNumeroEconomico.Text = "";
            this.txtTipoUnidad.Text = "";
            this.txtUnidad.Text = "";

            this.hdnMarcaID.Value = "";
            this.hdnModeloID.Value = "";
            this.hdnTipoUnidadID.Value = "";
            this.hdnUnidadId.Value = "";

            this.HabilitarActivoFijo(true);
            this.HabilitarUnidad(true);
            this.HabilitarModelo(false);

            this.txtFechaCompra.Enabled = false;
            this.txtMontoFactura.Enabled = false;
            this.txtLiderID.Enabled = false;
            bool Habilitar = false;
            
            ScriptManager.RegisterStartupScript(Page, typeof(Page), "HabilitaFechasDesflote", "HabilitaFechasDesflote('" + Habilitar + "');", true);

            this.ucDatosGeneralesUI.UsuarioAutenticado = this.UC;
            this.ucAsignacionLlantasUI.UsuarioAutenticado = this.UC;
            this.ucAsignacionEquiposAliadosUI.UsuarioAutenticado = this.UC;
           
        }

        public void HabilitarActivoFijo(bool habilitar)
        {
            this.txtClaveActivoOracle.Enabled = habilitar;
        }
        public void HabilitarUnidad(bool habilitar)
        {
            this.ibtnBuscaModelo.Enabled = habilitar;
            this.ibtnBuscaMarca.Enabled = habilitar;
            this.ibtnBuscaAplicacion.Enabled = habilitar;
            this.ibtnBuscaDistribuidor.Enabled = habilitar;
            this.ibtnBuscaMotorizacion.Enabled = habilitar;
            this.ibtnBuscaTipoUnidad.Enabled = habilitar;

            this.txtMarca.Enabled = habilitar;
            this.txtModelo.Enabled = habilitar;
            this.txtAplicacion.Enabled = habilitar;
            this.txtDistribuidor.Enabled = habilitar;
            this.txtMotorizacion.Enabled = habilitar;
            this.txtTipoUnidad.Enabled = habilitar;
            this.txtNumeroEconomico.Enabled = habilitar;
            this.txtAnio.Enabled = habilitar;

            this.txtProximoServicio.Enabled = habilitar;       

            this.txtKMProximoServicio.Enabled = habilitar;
        }

        #region SC0001
        public void HabilitarKMInicial(bool habilitar)
        {
                this.txtKMInicial.Enabled = habilitar;
                
        }
        public void HabilitarHRSInicial(bool habilitar)
        {
                this.txtHRSInicial.Enabled = habilitar;
        }
	    #endregion
        #region SC0002
        //public void HabilitarEntraMantenimiento(bool habilitar)
        //{
        //    this.chbxEntraMantenimiento.Enabled = habilitar;
        //}
        #endregion

        public void HabilitarModelo(bool habilitar)
        {
            this.txtModelo.Enabled = habilitar;
            this.ibtnBuscaModelo.Enabled = habilitar;
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

        public void RedirigirAConsulta()
        {
            this.Response.Redirect(this.ResolveUrl("~/Equipos.UI/ConsultarActaNacimientoUI.aspx"));
        }
        public void LimpiarSesion()
        {

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

        public void EstablecerPagina(int numeroPagina)
        {
            this.mvCU077.SetActiveView((View)this.mvCU077.FindControl("vwPagina" + numeroPagina.ToString()));
            this.hdnPaginaActual.Value = numeroPagina.ToString();
        }
        #region SC0008
        public void RedirigirSinPermisoAcceso()
        {
            this.Response.Redirect(this.ResolveUrl("~/MapaSitio.UI/PaginaSinAccesoUI.aspx"));
        }

        public void RegistrarScriptMensaje(string key, string script)
        {
            ScriptManager.RegisterStartupScript(Page, typeof(Page), key, script, true);
        }

        #endregion

        #region Métodos para el Buscador
        /// <summary>
        /// Ejecuta el buscador
        /// </summary>
        /// <param name="catalogo">Nombre del xml</param>
        /// <param name="tipoBusqueda">Nombre catalogo</param>
        private void EjecutaBuscador(string catalogo, ECatalogoBuscador catalogoBusqueda)
        {
            ViewState_Catalogo = catalogoBusqueda;
            this.Session_ObjetoBuscador = presentador.PrepararBOBuscador(catalogoBusqueda.ToString());
            this.Session_BOSelecto = null;
            this.RegistrarScript("Events", "BtnBuscar('" + ViewState_Guid + "','" + catalogo + "', '" + this.btnResult.ClientID + "');");
        }
        /// <summary>
        /// Desplegar la información el Objeto Seleccionado
        /// </summary>
        /// <param name="sBusqueda">Nombre del catalogo</param>
        private void DesplegarBOSelecto(ECatalogoBuscador catalogo)
        {
            presentador.DesplegarResultadoBuscador(catalogo.ToString(), this.Session_BOSelecto);
            this.Session_BOSelecto = null;
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

        /// <summary>
        /// Establecer las acciones de visualización de campos de captura
        /// </summary>
        public void EstablecerAcciones()
        {
            string valor = string.Empty;

            if (this.verTabDT == 0)
                valor += "1,";
            if (this.verTabNS == 0)
                valor += "2,";
            if (this.verTabLL == 0)
                valor += "3,";
            if (this.verTabEA == 0)
                valor += "4,";

            this.ValoresTabs = valor.TrimEnd(',');

            this.RegistrarScript("InicioAcciones", "InicializarControlesEmpresas('" + valor.TrimEnd(',') + "');");

            //Validación de vistas a capturar
            bool VerIdealease = true;
            if ((int)ETipoEmpresa.Generacion == this.UnidadOperativaId || (int)ETipoEmpresa.Construccion == this.UnidadOperativaId
                || (int)ETipoEmpresa.Equinova == this.UnidadOperativaId)
                VerIdealease = false;

            this.PrepararVistaCaptura(VerIdealease);
            bool Habilitar = false;
            ScriptManager.RegisterStartupScript(Page, typeof(Page), "HabilitaFechasDesflote", "HabilitaFechasDesflote('" + Habilitar + "');", true);

            if ((int)ETipoEmpresa.Idealease == this.UnidadOperativaId)
                this.AsignarEtiquetasCaptura(ETipoEmpresa.Idealease);
            if ((int)ETipoEmpresa.Generacion == this.UnidadOperativaId)
                this.AsignarEtiquetasCaptura(ETipoEmpresa.Generacion);
            if ((int)ETipoEmpresa.Construccion == this.UnidadOperativaId)
                this.AsignarEtiquetasCaptura(ETipoEmpresa.Construccion);
            if ((int)ETipoEmpresa.Equinova == this.UnidadOperativaId)
                this.AsignarEtiquetasCaptura(ETipoEmpresa.Equinova);
        }

                                
        /// <summary>
        /// REQ:13285, Asignación de etiquetas a los campos del formulario de acta de nacimiento
        /// Integración de generación y construcción
        /// </summary>
        /// <param name="tipoempresa">Identificador del tipo de empresa</param>
        public void AsignarEtiquetasCaptura(ETipoEmpresa tipoempresa)
        {
            this.lblAccesorio.Text = this.ObtenerEtiquetadelResource("RE00", tipoempresa);
            this.lblVIN.Text = this.ObtenerEtiquetadelResource("RE01", tipoempresa);
            this.RE01.InnerHtml = this.lblVIN.Text;
            this.lblActivoOracle.Text = this.ObtenerEtiquetadelResource("RE02", tipoempresa);
            this.RE02.InnerHtml = this.lblActivoOracle.Text;
            this.lblLider.Text = this.ObtenerEtiquetadelResource("RE03", tipoempresa);
            this.RE03.InnerHtml = this.lblLider.Text;
            this.lblNumEcono.Text = this.ObtenerEtiquetadelResource("RE04", tipoempresa);
            this.RE04.InnerHtml = this.lblNumEcono.Text;
            this.lblTipoUnidad.Text = this.ObtenerEtiquetadelResource("RE05", tipoempresa);
            this.RE05.InnerHtml = this.lblTipoUnidad.Text;
            this.lblMarca.Text = this.ObtenerEtiquetadelResource("RE06", tipoempresa);
            this.lblModelo.Text = this.ObtenerEtiquetadelResource("RE07", tipoempresa);
            this.RE07.InnerHtml = this.lblModelo.Text;
            this.lblAnio.Text = this.ObtenerEtiquetadelResource("RE08", tipoempresa);
            this.RE08.InnerHtml = this.lblAnio.Text;
            this.lblFechaCompra.Text = this.ObtenerEtiquetadelResource("RE09", tipoempresa);
            this.RE09.InnerHtml = this.lblFechaCompra.Text;
            this.lblMontoFactura.Text = this.ObtenerEtiquetadelResource("RE12", tipoempresa);
            this.RE12.InnerHtml = this.lblMontoFactura.Text;
            this.lblFabricante.Text = this.ObtenerEtiquetadelResource("RE11", tipoempresa);
            this.lblMotorizacion.Text = this.ObtenerEtiquetadelResource("RE20", tipoempresa);
            this.lblAplicacion.Text = this.ObtenerEtiquetadelResource("RE21", tipoempresa);
            this.lblFechaProximoServicio.Text = this.ObtenerEtiquetadelResource("RE22", tipoempresa);
            this.lblKmProximoServicio.Text = this.ObtenerEtiquetadelResource("RE23", tipoempresa);
            this.lblKMInicial.Text = this.ObtenerEtiquetadelResource("RE24", tipoempresa);
            this.lblHrsInicial.Text = this.ObtenerEtiquetadelResource("RE25", tipoempresa);
            this.lblCombustibleTotal.Text = this.ObtenerEtiquetadelResource("RE26", tipoempresa);

            this.lblInicioDepreciacion.Text = this.ObtenerEtiquetadelResource("RE13", tipoempresa);
            this.lblFechaDesflote.Text = this.ObtenerEtiquetadelResource("RE14", tipoempresa);
            this.lblTasaDepreciacion.Text = this.ObtenerEtiquetadelResource("RE15", tipoempresa);
            this.lblVidaUtil.Text = this.ObtenerEtiquetadelResource("RE16", tipoempresa);
            this.lblPorcentajeValorRes.Text = this.ObtenerEtiquetadelResource("RE17", tipoempresa);
            this.lblValorResidual.Text = this.ObtenerEtiquetadelResource("RE18", tipoempresa);
            this.lblSaldoDepreciar.Text = this.ObtenerEtiquetadelResource("RE19", tipoempresa);

            //Cuando la unidad operativa con la cuál se accedió no es Idealease será necesario 
            //eliminar la etiqueta de requerido del control de HrsInicial
            if (tipoempresa != ETipoEmpresa.Idealease)
            {
                this.spanHrsInicial.InnerText = "";
                this.spanCombustibleTotal.InnerHtml = "";
            }
        }


        /// <summary>
        /// REQ: 13285, Método de obtención de configuración de tabs
        /// </summary>
        /// <param name="cEtiquetaBuscar">Etiqueta que se buscará</param>
        /// <param name="tabVisible">orden del tab</param>
        public int ConfigurarTab(string cEtiquetaBuscar, int tabVisible)
        {
            //Instanciamos la clase o web method que obtiene las etiquetas
            ObtenerEtiquetaEmpresas obtenerEtiqueta = new ObtenerEtiquetaEmpresas();
            string cEtiquetaObtenida = string.Empty;
            EtiquetaObtenida request = null;
            int tab = 0;
            int pagina = 0;

            cEtiquetaObtenida = obtenerEtiqueta.ObtenerEtiquetadelResource(cEtiquetaBuscar, this.UnidadOperativaId.GetValueOrDefault());
            request = JsonConvert.DeserializeObject<EtiquetaObtenida>(cEtiquetaObtenida);
            if (request.cMensaje == "")
            {
                cEtiquetaObtenida = request.cEtiqueta;
                int.TryParse(cEtiquetaObtenida, out tab);
                if (tab == 0)
                {
                    tabVisible = tabVisible + 1;
                    pagina = int.Parse(hdnPaginaActual.Value);
                    if (tabVisible == pagina)
                    {
                        pagina = pagina + 1;
                        EstablecerPagina(pagina);
                    }
                }
            }

            return tab;
        }

        /// <summary>
        /// REQ:13285, Método que obtiene el nombre de la etiqueta del archivo resource correspondiente a la unidad operativa a la cual accedió el usuario.
        /// </summary>
        /// <param name="cEtiquetaBuscar">Nombre de la etiqueta que será buscada en el archivo resource</param>
        /// <param name="tipoEmpresa">Nombre de la unidad operativa a la cual accedió el usuario.</param>
        /// <returns>Retorna el nombre de la etiqueta correspondiente al valor recibido en el parámetro cEtiquetaBuscar del archivo resource.</returns>
        public string ObtenerEtiquetadelResource(string cEtiquetaBuscar, ETipoEmpresa tipoEmpresa)
        {
            string cEtiqueta = string.Empty;
            //Instanciamos la clase o webmethod que obtiene las etiquetas
            ObtenerEtiquetaEmpresas obtenerEtiqueta = new ObtenerEtiquetaEmpresas();
            string cEtiquetaObtenida = string.Empty;
            EtiquetaObtenida request = null;

            cEtiquetaObtenida = obtenerEtiqueta.ObtenerEtiquetadelResource(cEtiquetaBuscar, (int)tipoEmpresa);
            request = JsonConvert.DeserializeObject<EtiquetaObtenida>(cEtiquetaObtenida);
            if (string.IsNullOrEmpty(request.cMensaje))
            {
                cEtiquetaObtenida = request.cEtiqueta;
                if (cEtiqueta != "NO APLICA")
                {
                    cEtiqueta = cEtiquetaObtenida;
                }
            }
            return cEtiqueta;
        }

        /// <summary>
        /// REQ:13285, Método que prepara los controles que se visualizaran en la captura del acta de nacimiento
        /// </summary>
        /// <param name="VerIdealease">Indica si se visualizan los campos de idealease</param>
        public void PrepararVistaCaptura(bool VerIdealease)
        {
            this.trAccesorio.Visible = !VerIdealease;
            this.trFechaInicioDepreciacion.Visible = !VerIdealease;
            this.trFechaDesflote.Visible = !VerIdealease;
            this.trTasaDepreciacion.Visible = !VerIdealease;
            this.trVidaUtil.Visible = !VerIdealease;
            this.trPorcentajeVR.Visible = !VerIdealease;
            this.trValorResidual.Visible = !VerIdealease;
            this.trSaldoDepreciar.Visible = !VerIdealease;
            this.trKMInicial.Visible = VerIdealease;
        }

        /// <summary>
        /// Método implementado desde la vista IDetalleActaNacimientoVIS,
        /// que se encarga validar que pestañas se ocultan o muestran, sirve para validar los datos a interfaz.
        /// </summary>
        /// <param name="cEtiquetaBuscar">Valor de la etiqueta a buscar para cambiar el nombre</param>
        /// <returns>Regresa el valor del tab por empresa</returns>
        public int ValidarTab(string cEtiquetaBuscar)
        {
            string cEtiquetaObtenida = string.Empty;
            EtiquetaObtenida request = null;
            int valorTab = 0;

            if (obtenerEtiqueta == null)
                obtenerEtiqueta = new ObtenerEtiquetaEmpresas();

            cEtiquetaObtenida = obtenerEtiqueta.ObtenerEtiquetadelResource(cEtiquetaBuscar, this.UnidadOperativaId.GetValueOrDefault());
            request = JsonConvert.DeserializeObject<EtiquetaObtenida>(cEtiquetaObtenida);

            if (string.IsNullOrEmpty(request.cMensaje))
            {
                cEtiquetaObtenida = request.cEtiqueta;
                int.TryParse(cEtiquetaObtenida, out valorTab);
            }
            else
            {
                this.MostrarMensaje("Inconsistencia al buscar la etiqueta en el archivo de recursos", ETipoMensajeIU.ERROR, nombreClase + "ConfigurarTab" + request.cMensaje);
            }
            return valorTab;
        }
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
        protected void btnBorrador_Click(object sender, EventArgs e)
        {
            try
            {
                int paquete = this.presentador.RegistrarBorrador();
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

                int paquete = this.presentador.RegistrarTerminada();
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
                this.presentador.CancelarRegistro();
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencia al cancelar el registro", ETipoMensajeIU.ERROR, this.nombreClase + ".btnCancelar_Click:" + ex.Message);
            }
        }

        #region Eventos para el Buscador
        /// <summary>
        /// TextChanged acitva el llamado al Buscador para la busqueda de Unidad
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void txtUnidad_TextChanged(object sender, EventArgs e)
        {
            try
            {
                string serieUnidad = NumeroSerie;
                this.Session_BOSelecto = null;

                this.DesplegarBOSelecto(ECatalogoBuscador.Unidad);

                NumeroSerie = serieUnidad;
                if (NumeroSerie != null)
                {
                    this.EjecutaBuscador("EquipoBepensa", ECatalogoBuscador.Unidad);
                    NumeroSerie = null;
                }

            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencia al buscar una Unidad", ETipoMensajeIU.ADVERTENCIA, this.nombreClase + ".txtUnidad_TextChanged:" + ex.Message);
            }
        }
        /// <summary>
        /// <summary>
        /// Buscar Unidad
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ibtnBuscaUnidad_Click(object sender, ImageClickEventArgs e)
        {
            string s;
            if ((s = this.presentador.ValidarCamposConsultaUnidad()) != null)
            {
                this.MostrarMensaje(s, ETipoMensajeIU.ADVERTENCIA, null);
                return;
            }

            try
            {
                this.EjecutaBuscador("EquipoBepensa&hidden=0", ECatalogoBuscador.Unidad);
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencia al buscar una Unidad", ETipoMensajeIU.ADVERTENCIA, this.nombreClase + ".ibtnBuscaUnidad_Click:" + ex.Message);
            }
        }

        /// <summary>
        /// TextChanged acitva el llamado al Buscador para la busqueda de Tipo de Unidad
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void txtTipoUnidad_TextChanged(object sender, EventArgs e)
        {
            try
            {
                string tipoUnidadNombre = this.TipoUnidadNombre;
                this.Session_BOSelecto = null;

                this.DesplegarBOSelecto(ECatalogoBuscador.TipoUnidad);

                this.TipoUnidadNombre = tipoUnidadNombre;
                if (this.TipoUnidadNombre != null)
                {
                    this.EjecutaBuscador("TipoUnidad", ECatalogoBuscador.TipoUnidad);
                    this.TipoUnidadNombre = null;
                }
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencia al buscar un Tipo de Unidad", ETipoMensajeIU.ADVERTENCIA, this.nombreClase + ".txtTipoUnidad_TextChanged:" + ex.Message);
            }
        }
        /// <summary>
        /// Buscar Tipo de Unidad
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ibtnBuscaTipoUnidad_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                this.EjecutaBuscador("TipoUnidad&hidden=0", ECatalogoBuscador.TipoUnidad);
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencia al buscar un Tipo de Unidad", ETipoMensajeIU.ADVERTENCIA, this.nombreClase + ".ibtnBuscaTipoUnidad_Click:" + ex.Message);
            }
        }
        
        /// <summary>
        /// TextChanged acitva el llamado al Buscador para la busqueda de Marca
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void txtMarca_TextChanged(object sender, EventArgs e)
        {
            try
            {
                string marcaNombre = this.MarcaNombre;
                this.Session_BOSelecto = null;

                this.DesplegarBOSelecto(ECatalogoBuscador.Marca);

                this.MarcaNombre = marcaNombre;
                if (this.MarcaNombre != null)
                {
                    this.EjecutaBuscador("Marca", ECatalogoBuscador.Marca);
                    this.MarcaNombre = null;
                }
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencia al buscar una Marca", ETipoMensajeIU.ADVERTENCIA, this.nombreClase + ".txtMarca_TextChanged:" + ex.Message);
            }
        }
        /// <summary>
        /// Buscar Marca
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ibtnBuscaMarca_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                this.EjecutaBuscador("Marca&hidden=0", ECatalogoBuscador.Marca);
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencia al buscar una Marca", ETipoMensajeIU.ADVERTENCIA, this.nombreClase + ".ibtnBuscaMarca_Click:" + ex.Message);
            }
        }

        /// <summary>
        /// TextChanged acitva el llamado al Buscador para la busqueda de Modelo
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void txtModelo_TextChanged(object sender, EventArgs e)
        {
            try
            {
                int? marcaId = this.MarcaId;
                string modeloNombre = this.ModeloNombre;
                this.Session_BOSelecto = null;

                this.DesplegarBOSelecto(ECatalogoBuscador.Modelo);

                this.MarcaId = marcaId;
                this.ModeloNombre = modeloNombre;
                if (this.ModeloNombre != null && this.MarcaId != null)
                {
                    this.EjecutaBuscador("Modelo", ECatalogoBuscador.Modelo);
                    this.ModeloNombre = null;
                }
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencia al buscar una Modelo", ETipoMensajeIU.ADVERTENCIA, this.nombreClase + ".txtModelo_TextChanged:" + ex.Message);
            }
        }
        /// <summary>
        /// Buscar Modelo
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ibtnBuscaModelo_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                this.EjecutaBuscador("Modelo&hidden=0", ECatalogoBuscador.Modelo);
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencia al buscar un Modelo", ETipoMensajeIU.ADVERTENCIA, this.nombreClase + ".ibtnBuscaModelo_Click:" + ex.Message);
            }
        }

        /// <summary>
        /// TextChanged acitva el llamado al Buscador para la busqueda de Distribuidor
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void txtDistribuidor_TextChanged(object sender, EventArgs e)
        {
            try
            {
                string distribuidorNombre = this.FabricanteNombre;
                this.Session_BOSelecto = null;

                this.DesplegarBOSelecto(ECatalogoBuscador.Distribuidor);

                this.FabricanteNombre = distribuidorNombre;
                if (this.FabricanteNombre != null)
                {
                    this.EjecutaBuscador("Distribuidor", ECatalogoBuscador.Distribuidor);
                    this.FabricanteNombre = null;
                }
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencia al buscar un Distribuidor", ETipoMensajeIU.ADVERTENCIA, this.nombreClase + ".txtDistribuidor_TextChanged:" + ex.Message);
            }
        }
        /// <summary>
        /// Buscar Distribuidor
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ibtnBuscaDistribuidor_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                this.EjecutaBuscador("Distribuidor&hidden=0", ECatalogoBuscador.Distribuidor);
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencia al buscar un Distribuidor", ETipoMensajeIU.ADVERTENCIA, this.nombreClase + ".ibtnBuscaDistribuidor_Click:" + ex.Message);
            }
        }

        /// <summary>
        /// TextChanged acitva el llamado al Buscador para la busqueda de Motorizacion
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void txtMotorizacion_TextChanged(object sender, EventArgs e)
        {
            try
            {
                string motorizacionNombre = this.MotorizacionNombre;
                this.Session_BOSelecto = null;

                this.DesplegarBOSelecto(ECatalogoBuscador.Motorizacion);

                this.MotorizacionNombre = motorizacionNombre;
                if (this.MotorizacionNombre != null)
                {
                    this.EjecutaBuscador("ConfiguracionModeloMotorizacion", ECatalogoBuscador.Motorizacion);
                    this.MotorizacionNombre = null;
                }
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencia al buscar una Motorización", ETipoMensajeIU.ADVERTENCIA, this.nombreClase + ".txtMotorizacion_TextChanged:" + ex.Message);
            }
        }
        /// <summary>
        /// Buscar Motorizacion
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ibtnBuscaMotorizacion_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                this.EjecutaBuscador("ConfiguracionModeloMotorizacion&hidden=0", ECatalogoBuscador.Motorizacion);
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencia al buscar una Motorización", ETipoMensajeIU.ADVERTENCIA, this.nombreClase + ".ibtnBuscaMotorizacion_Click:" + ex.Message);
            }
        }

        /// <summary>
        /// TextChanged acitva el llamado al Buscador para la busqueda de Aplicacion
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void txtAplicacion_TextChanged(object sender, EventArgs e)
        {
            try
            {
                string aplicacionNombre = this.AplicacionNombre;
                this.Session_BOSelecto = null;

                this.DesplegarBOSelecto(ECatalogoBuscador.Aplicacion);

                this.AplicacionNombre = aplicacionNombre;
                if (this.AplicacionNombre != null)
                {
                    this.EjecutaBuscador("ClasificadorAplicacion", ECatalogoBuscador.Aplicacion);
                    this.AplicacionNombre = null;
                }
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencia al buscar una Aplicación", ETipoMensajeIU.ADVERTENCIA, this.nombreClase + ".txtAplicacion_TextChanged:" + ex.Message);
            }
        }
        /// <summary>
        /// Buscar Aplicacion
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ibtnBuscaAplicacion_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                this.EjecutaBuscador("ClasificadorAplicacion&hidden=0", ECatalogoBuscador.Aplicacion);
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencia al buscar una Aplicación", ETipoMensajeIU.ADVERTENCIA, this.nombreClase + ".ibtnBuscaAplicacion_Click:" + ex.Message);
            }
        }


        protected void btnResult_Click(object sender, EventArgs e)
        {
            try
            {
                switch (ViewState_Catalogo)
                {
                    case ECatalogoBuscador.Unidad:
                        if (this.Session_BOSelecto != null && presentador.VerificarExistenciaActaNacimiento(this.Session_BOSelecto))
                        {
                            this.Session_BOSelecto = null;
                            #region RI0040
                            this.MostrarMensaje("La unidad que seleccionó ya está registrada en el sistema", ETipoMensajeIU.ADVERTENCIA, null);
                            #endregion RI0040
                        }
                        this.DesplegarBOSelecto(ViewState_Catalogo);
                        break;
                    case ECatalogoBuscador.Marca:
                    case ECatalogoBuscador.Modelo:
                    case ECatalogoBuscador.TipoUnidad:
                    case ECatalogoBuscador.Aplicacion:
                    case ECatalogoBuscador.Distribuidor:
                    case ECatalogoBuscador.Motorizacion:
                        this.DesplegarBOSelecto(ViewState_Catalogo);
                        break;
                }
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencias al presentar la información.", ETipoMensajeIU.ERROR, this.nombreClase + ".btnResult_Click:" + ex.Message);
            }
        }
        #endregion
        #endregion


        
    }
}
