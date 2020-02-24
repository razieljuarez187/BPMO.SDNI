//Satisface al CU077 - Registrar Acta de Nacimiento de una Unidad
//Satisface al CU079 - Consultar Acta de Nacimiento de Unidad
//Satisface al CU080 – Editar Acta de Nacimiento de una Unidad
using System;
using System.Collections.Generic;
using System.Linq;
using BPMO.Basicos.BO;
using BPMO.Facade.SDNI.BOF;
using BPMO.Facade.SDNI.BR;
using BPMO.Patterns.Creational.DataContext;
using BPMO.Primitivos.Enumeradores;
using BPMO.SDNI.Comun.BO;
using BPMO.SDNI.Comun.PRE;
using BPMO.SDNI.Equipos.BO;
using BPMO.SDNI.Equipos.VIS;

namespace BPMO.SDNI.Equipos.PRE
{
    public class ucDatosGeneralesPRE
    {
        #region Atributos
        private IDataContext dctx = null;

        private IucDatosGeneralesVIS vista;
        private string nombreClase = "DatosGeneralesPRE";

        private readonly ucCatalogoDocumentosPRE documentosPRE;

        #endregion

        #region Constructores
        public ucDatosGeneralesPRE(IucDatosGeneralesVIS view, ucCatalogoDocumentosPRE documentos)
        {
            try
            {
                this.vista = view;
                
                this.dctx = FacadeBR.ObtenerConexion();

                documentosPRE = documentos;
                this.EstablecerTipoAdjunto();
                this.vista.EstablecerIdentificadorListaArchivos(nombreClase);
            }
            catch (Exception ex)
            {
                this.vista.MostrarMensaje("Inconsistencias en los parámetros de configuración", ETipoMensajeIU.ERROR, this.nombreClase + ".ucDatosGeneralesPRE:" + ex.Message);
            }
        }
        #endregion

        #region Métodos

        public void PrepararNuevo()
        {
           this.vista.PrepararNuevo();

           #region [RQM 13285- Integración Construcción y Generacción]

            this.vista.EnumTipoEmpresa =  (ETipoEmpresa)this.vista.UnidadOperativaId;
            if (this.vista.EnumTipoEmpresa == ETipoEmpresa.Idealease)
            {
                this.CargarAreas();
            }
            if (this.vista.EnumTipoEmpresa == ETipoEmpresa.Construccion)
            {
                this.CargarTipoRentaConstruccion();
                CargaTipoMonedas();
            }

            if (this.vista.EnumTipoEmpresa == ETipoEmpresa.Generacion)
            {
                this.CargarTipoRentaGeneracion();
                CargaTipoMonedas();
            }

            if (this.vista.EnumTipoEmpresa == ETipoEmpresa.Equinova) {
                this.CargarTipoRentaEquinova();
                CargaTipoMonedas();
            }
            #endregion
            this.vista.HabilitarModoEdicion(true);
            this.vista.HabilitarCliente(false);
            this.vista.HabilitarPropietario(false);
            this.vista.MostrarFabricante(false);
            this.vista.MostrarOrdenCompra(false);
        }

        /// <summary>
        /// Consulta los tipos de moneda en ORACLE, para el combo Tipo Monedas de Construcción y Generación
        /// </summary>
        public void CargaTipoMonedas()
        {
            //Inicia la carga de datos de Tipos de Moneda
            List<MonedaBO> lstMonedas = FacadeBR.ConsultarMoneda(this.dctx, new MonedaBO() { Activo = true });
            this.vista.EstablecerOpcionesMoneda(lstMonedas.ToDictionary(p => p.Codigo, p => p.Nombre));
        }

        public void PrepararEdicion()
        {
            this.vista.EnumTipoEmpresa = (ETipoEmpresa)this.vista.UnidadOperativaId;
            if (this.vista.EnumTipoEmpresa == ETipoEmpresa.Idealease)
            {
                this.vista.HabilitarModoEdicion(true);
                this.CargarAreas();
                this.vista.MostrarFabricante(true);
                this.vista.HabilitaFechasArrendamiento(false);
                this.vista.HabilitaMontoArrendamiento(false);
                this.vista.HabilitaTipoMonedas(false);
                this.vista.MostrarTipoRentaEmpresas(false);
                this.vista.MostrarOrdenCompra(false);
                this.vista.MostrarTipoMoneda(false);
                this.vista.MostrarMontoArrendamiento(false);
            }

            #region [RQM 13285- Integración Construcción y Generacción]

            if (this.vista.EnumTipoEmpresa == ETipoEmpresa.Construccion)
            {
                this.vista.HabilitarModoEdicion(false);
                this.vista.HabilitaMontoArrendamiento(true);
                this.CargarTipoRentaConstruccion();
                this.vista.MostrarFabricante(false);
                this.vista.HabilitaTipoRentaEmpresas(true);
                this.CargaTipoMonedas();
                this.vista.HabilitaFechasArrendamiento(false);
                this.vista.HabilitarSucursal(true);
                this.vista.HabilitarUnidadBloqueada(true);
            }
            if (this.vista.EnumTipoEmpresa == ETipoEmpresa.Generacion)
            {
                this.vista.HabilitarModoEdicion(false);
                this.CargarTipoRentaGeneracion();
                this.vista.MostrarFabricante(false);
                this.vista.HabilitaTipoRentaEmpresas(true);
                this.CargaTipoMonedas();
                this.vista.HabilitaFechasArrendamiento(false);
                this.vista.HabilitarSucursal(true);
                this.vista.HabilitarUnidadBloqueada(true);
            }
            if (this.vista.EnumTipoEmpresa == ETipoEmpresa.Equinova) {
                this.vista.HabilitarModoEdicion(false);
                this.CargarTipoRentaEquinova();
                this.vista.MostrarFabricante(false);
                this.vista.HabilitaTipoRentaEmpresas(true);
                this.CargaTipoMonedas();
                this.vista.HabilitaFechasArrendamiento(false);
                this.vista.HabilitarSucursal(true);
                this.vista.HabilitarUnidadBloqueada(true);
            }
            #endregion
            this.vista.HabilitarCliente(false);
            this.vista.HabilitarPropietario(false);
        }

        public void PrepararVisualizacion()
        {
            this.vista.EnumTipoEmpresa = (ETipoEmpresa)this.vista.UnidadOperativaId;
            if (this.vista.EnumTipoEmpresa == ETipoEmpresa.Idealease)
            {
                this.CargarAreas();
                this.vista.MostrarFabricante(true);
                this.vista.HabilitaFechasArrendamiento(false);
                this.vista.HabilitaMontoArrendamiento(false);
                this.vista.HabilitaTipoMonedas(false);
                this.vista.MostrarTipoRentaEmpresas(false);
                this.vista.MostrarOrdenCompra(false);
                this.vista.MostrarTipoMoneda(false);
                this.vista.HabilitaControlesOC(false);
                this.vista.MostrarMontoArrendamiento(false);
            }
            #region [RQM 13285- Integración Construcción y Generacción]

            if (this.vista.EnumTipoEmpresa == ETipoEmpresa.Construccion)
            {
                this.CargarTipoRentaConstruccion();
                this.vista.MostrarFabricante(false);
                this.vista.HabilitaArea(false);
                this.CargaTipoMonedas();
            }
            if (this.vista.EnumTipoEmpresa == ETipoEmpresa.Generacion)
            {
                this.CargarTipoRentaGeneracion();
                this.vista.MostrarFabricante(false);
                this.vista.HabilitaArea(false);
                this.CargaTipoMonedas();
            }
            if (this.vista.EnumTipoEmpresa == ETipoEmpresa.Equinova) {
                this.CargarTipoRentaEquinova();
                this.vista.MostrarFabricante(false);
                this.vista.HabilitaArea(false);
                this.CargaTipoMonedas();
            }
            #endregion
            
            this.vista.HabilitarModoEdicion(false);             
        }

        private void CargarAreas()
        {
            Dictionary<string, object> areas = new Dictionary<string, object>();

            string[] names = Enum.GetNames(typeof(EArea));

            areas.Add(EArea.RD.ToString(), (int)EArea.RD);
            areas.Add(EArea.FSL.ToString(), (int)EArea.FSL);
            areas.Add(EArea.CM.ToString(), (int)EArea.CM);
            areas.Add(EArea.SD.ToString(), (int)EArea.SD);

            this.vista.CargarAreas(areas);
        }

        #region [RQM 13285- Integración Construcción y Generacción]
        /// <summary>
        /// Método que carga los Tipos de Renta para Construcción
        /// </summary>
        private void CargarTipoRentaConstruccion()
        {
            Dictionary<string, object> tipoRenta = new Dictionary<string, object>();

            string[] names = Enum.GetNames(typeof(EAreaConstruccion));

            tipoRenta.Add(EAreaConstruccion.RE.ToString(), (int)EAreaConstruccion.RE);
            tipoRenta.Add(EAreaConstruccion.RO.ToString(), (int)EAreaConstruccion.RO);
            tipoRenta.Add(EAreaConstruccion.ROC.ToString(), (int)EAreaConstruccion.ROC);

            this.vista.CargarTipoRentaConstruccion(tipoRenta);
        }

        /// <summary>
        /// Método que carga los Tipos de Renta para Generación
        /// </summary>
        private void CargarTipoRentaGeneracion()
        {
            Dictionary<string, object> tipoRenta = new Dictionary<string, object>();

            string[] names = Enum.GetNames(typeof(EAreaGeneracion));

            tipoRenta.Add(EAreaGeneracion.RE.ToString(), (int)EAreaGeneracion.RE);
            tipoRenta.Add(EAreaGeneracion.RO.ToString(), (int)EAreaGeneracion.RO);
            tipoRenta.Add(EAreaGeneracion.ROC.ToString(), (int)EAreaGeneracion.ROC);

            this.vista.CargarTipoRentaGeneracion(tipoRenta);
        }

        /// <summary>
        /// Método que carga los Tipos de Renta para Equinova
        /// </summary>
        private void CargarTipoRentaEquinova() {
            Dictionary<string, object> tipoRenta = new Dictionary<string, object>();

            string[] names = Enum.GetNames(typeof(EAreaEquinova));

            tipoRenta.Add(EAreaEquinova.RE.ToString(), (int)EAreaEquinova.RE);
            tipoRenta.Add(EAreaEquinova.RO.ToString(), (int)EAreaEquinova.RO);
            tipoRenta.Add(EAreaEquinova.ROC.ToString(), (int)EAreaEquinova.ROC);

            this.vista.CargarTipoRentaEquinova(tipoRenta);
        }

        #endregion

        public void BloquearCamposPorEstatus(EEstatusUnidad? estatus)
        {
            if (estatus == null) return;

            if (estatus != EEstatusUnidad.NoDisponible)
            {
                this.vista.HabilitarSucursal(false);
                this.vista.HabilitarArea(false);
            }
        }

        public void SeleccionarArea(EArea? area)
        {
            int? valor = null;
            if (area != null)
                valor = (int)area;

            this.SeleccionarArea(valor);
        }

        #region [RQM 13285- Integración Construcción y Generacción]
        /// <summary>
        /// Obtiene el valor del combo Tipo de Renta seleccionado
        /// </summary>
        /// <param name="ValorTipoRenta">Valor seleccionado</param>
        public void SeleccionarTipoRentaEmpresas(Enum valorTipoRenta)
        {
            int? valor = null;
            if (valorTipoRenta != null)
                valor = Convert.ToInt32(valorTipoRenta);

            this.SeleccionarTipoRentaEmpresas(valor);
        }

        
        /// <summary>
        /// Controla el formulario dependiendo el tipo de renta seleccionado
        /// </summary>
        /// <param name="valor">Valor de la opción seleccionada en el combo ddlTipoRentaEmpresas</param>
        public void SeleccionarTipoRentaEmpresas(int? valor)
        {
            
            this.vista.EnumTipoEmpresa = (ETipoEmpresa)this.vista.UnidadOperativaId;

            if (this.vista.EnumTipoEmpresa == ETipoEmpresa.Idealease)
            {
                this.vista.MostrarTipoMoneda(false);
            }

            if (this.vista.EnumTipoEmpresa == ETipoEmpresa.Construccion)
            {
                EAreaConstruccion? EnumRentaConstruccion = null;
                if (valor != null) EnumRentaConstruccion = (EAreaConstruccion)Enum.Parse(typeof(EAreaConstruccion), valor.ToString());
                switch (EnumRentaConstruccion)
                {
                    case EAreaConstruccion.ROC:
                        this.vista.HabilitarPropietario(false);
                        this.vista.HabilitarCliente(true);
                        this.vista.Propietario = this.vista.NombreClienteUnidadOperativa;
                        this.vista.Cliente = "";
                        this.vista.ClienteId = null;                      
                        this.vista.HabilitarEntraMantenimiento(true);
                        this.vista.ActivaCheckMantenimiento(false);
                        this.vista.MostrarOrdenCompra(false);
                        this.vista.HabilitarCargaArchivoOC(false);
                        this.vista.ModificaEtiquetaPropietario(false);
                        this.vista.HabilitaMontoArrendamiento(false);
                        this.vista.HabilitaFechasArrendamiento(false);
                        this.vista.MostrarMontoArrendamiento(false);
                        this.vista.HabilitaTipoMonedas(false);
                        this.vista.MostrarTipoMoneda(false);
                        this.vista.ReiniciaMontoArrendamiento();
                        break;
                    case EAreaConstruccion.RE:
                        this.vista.HabilitaMontoArrendamiento(true);
                        this.vista.HabilitaFechasArrendamiento(true);
                        this.vista.HabilitarEntraMantenimiento(true);
                        this.vista.ActivaCheckMantenimiento(false);
                        this.vista.ModificaEtiquetaPropietario(true);
                        this.vista.HabilitarPropietario(true);
                        this.vista.Propietario = "";
                        this.vista.PropietarioId = 0;
                        this.vista.HabilitaTipoMonedas(false);
                        this.vista.MostrarTipoMoneda(true);
                        int? ClienteId = this.ObtenerClienteId(this.vista.NombreClienteUnidadOperativa);
                        string ClienteNombre = (ClienteId != null) ? this.vista.NombreClienteUnidadOperativa : "";
                        this.vista.Cliente = ClienteNombre;
                        this.vista.ClienteId = ClienteId;
                        this.vista.MostrarOrdenCompra(true);
                        this.vista.HabilitarCargaArchivoOC(true);
                        this.vista.MostrarMontoArrendamiento(true);
                        this.vista.ReiniciaMontoArrendamiento();
                        break;
                    case EAreaConstruccion.RO:
                        this.vista.HabilitarPropietario(false);
                        this.vista.HabilitarCliente(false);
                        int? id = this.ObtenerClienteId(this.vista.NombreClienteUnidadOperativa);
                        string nombre = (id != null) ? this.vista.NombreClienteUnidadOperativa : "";
                        this.vista.Propietario = nombre;
                        this.vista.Cliente = nombre;
                        this.vista.PropietarioId = id;
                        this.vista.ClienteId = id;
                        this.vista.HabilitarEntraMantenimiento(false);
                        this.vista.ActivaCheckMantenimiento(true);
                        this.vista.MostrarOrdenCompra(false);
                        this.vista.HabilitarCargaArchivoOC(false);
                        this.vista.ModificaEtiquetaPropietario(false);
                        this.vista.HabilitaTipoMonedas(false);
                        this.vista.MostrarTipoMoneda(false);
                        this.vista.HabilitaFechasArrendamiento(false);
                        this.vista.HabilitaMontoArrendamiento(false);
                        this.vista.ReiniciaMontoArrendamiento();
                        this.vista.MostrarMontoArrendamiento(false);
                        break;
                }
            }
            if (this.vista.EnumTipoEmpresa == ETipoEmpresa.Generacion)
            {
                EAreaGeneracion? EnumRentaGeneracion = null;
                if (valor != null) EnumRentaGeneracion = (EAreaGeneracion)Enum.Parse(typeof(EAreaGeneracion), valor.ToString());
                switch (EnumRentaGeneracion)
                {
                    case EAreaGeneracion.ROC:
                        this.vista.HabilitarPropietario(false);
                        this.vista.HabilitarCliente(true);
                        this.vista.Propietario = this.vista.NombreClienteUnidadOperativa;
                        this.vista.Cliente = "";
                        this.vista.ClienteId = null;
                        this.vista.HabilitarEntraMantenimiento(true);
                        this.vista.ActivaCheckMantenimiento(false);
                        this.vista.MostrarOrdenCompra(false);
                        this.vista.HabilitarCargaArchivoOC(false);
                        this.vista.ModificaEtiquetaPropietario(false);
                        this.vista.HabilitaMontoArrendamiento(false);
                        this.vista.HabilitaTipoMonedas(false);
                        this.vista.MostrarTipoMoneda(false);
                        this.vista.HabilitaFechasArrendamiento(false);
                        this.vista.ReiniciaMontoArrendamiento();
                        this.vista.MostrarMontoArrendamiento(false);
                        break;
                    case EAreaGeneracion.RE:
                        this.vista.HabilitarCliente(false);
                        this.vista.HabilitaMontoArrendamiento(true);
                        this.vista.HabilitaFechasArrendamiento(true);
                        this.vista.HabilitarEntraMantenimiento(true);
                        this.vista.ActivaCheckMantenimiento(false);
                        this.vista.MostrarOrdenCompra(true);
                        this.vista.HabilitarCargaArchivoOC(true);
                        this.vista.ModificaEtiquetaPropietario(true);
                        this.vista.HabilitarPropietario(true);
                        this.vista.Propietario = "";
                        this.vista.PropietarioId = 0;
                        this.vista.MostrarTipoMoneda(true);
                        this.vista.HabilitaTipoMonedas(false);
                        int? id = this.ObtenerClienteId(this.vista.NombreClienteUnidadOperativa);
                        string nombre = (id != null) ? this.vista.NombreClienteUnidadOperativa : "";
                        this.vista.Cliente = nombre;
                        this.vista.ClienteId = id;
                        this.vista.ReiniciaMontoArrendamiento();
                        this.vista.MostrarMontoArrendamiento(true);
                        break;
                    case EAreaGeneracion.RO:
                        this.vista.HabilitarPropietario(false);
                        this.vista.ModificaEtiquetaPropietario(false);
                        this.vista.HabilitarCliente(false);
                        int? ClienteID = this.ObtenerClienteId(this.vista.NombreClienteUnidadOperativa);
                        string ClienteNombre = (ClienteID != null) ? this.vista.NombreClienteUnidadOperativa : "";
                        this.vista.Cliente = ClienteNombre;
                        this.vista.ClienteId = ClienteID;
                        this.vista.Propietario = ClienteNombre;
                        this.vista.PropietarioId = ClienteID;
                        this.vista.HabilitarEntraMantenimiento(false);
                        this.vista.ActivaCheckMantenimiento(true);
                        this.vista.MostrarOrdenCompra(false);
                        this.vista.HabilitarCargaArchivoOC(false);
                        this.vista.HabilitaMontoArrendamiento(false);
                        this.vista.HabilitaTipoMonedas(false);
                        this.vista.MostrarTipoMoneda(false);
                        this.vista.HabilitaFechasArrendamiento(false);
                        this.vista.ReiniciaMontoArrendamiento();
                        this.vista.MostrarMontoArrendamiento(false);
                        break;
                }
            }
            if (this.vista.EnumTipoEmpresa == ETipoEmpresa.Equinova) {
                EAreaEquinova? EnumRentaEquinova = null;
                if (valor != null) EnumRentaEquinova = (EAreaEquinova)Enum.Parse(typeof(EAreaEquinova), valor.ToString());
                switch (EnumRentaEquinova) {
                    case EAreaEquinova.ROC:
                        this.vista.HabilitarPropietario(false);
                        this.vista.HabilitarCliente(true);
                        this.vista.Propietario = this.vista.NombreClienteUnidadOperativa;
                        this.vista.Cliente = "";
                        this.vista.ClienteId = null;
                        this.vista.HabilitarEntraMantenimiento(true);
                        this.vista.ActivaCheckMantenimiento(false);
                        this.vista.MostrarOrdenCompra(false);
                        this.vista.HabilitarCargaArchivoOC(false);
                        this.vista.ModificaEtiquetaPropietario(false);
                        this.vista.HabilitaMontoArrendamiento(false);
                        this.vista.HabilitaTipoMonedas(false);
                        this.vista.MostrarTipoMoneda(false);
                        this.vista.HabilitaFechasArrendamiento(false);
                        this.vista.ReiniciaMontoArrendamiento();
                        this.vista.MostrarMontoArrendamiento(false);
                        break;
                    case EAreaEquinova.RE:
                        this.vista.HabilitarCliente(false);
                        this.vista.HabilitaMontoArrendamiento(true);
                        this.vista.HabilitaFechasArrendamiento(true);
                        this.vista.HabilitarEntraMantenimiento(true);
                        this.vista.ActivaCheckMantenimiento(false);
                        this.vista.MostrarOrdenCompra(true);
                        this.vista.HabilitarCargaArchivoOC(true);
                        this.vista.ModificaEtiquetaPropietario(true);
                        this.vista.HabilitarPropietario(true);
                        this.vista.Propietario = "";
                        this.vista.PropietarioId = 0;
                        this.vista.MostrarTipoMoneda(true);
                        this.vista.HabilitaTipoMonedas(false);
                        int? id = this.ObtenerClienteId(this.vista.NombreClienteUnidadOperativa);
                        string nombre = (id != null) ? this.vista.NombreClienteUnidadOperativa : "";
                        this.vista.Cliente = nombre;
                        this.vista.ClienteId = id;
                        this.vista.ReiniciaMontoArrendamiento();
                        this.vista.MostrarMontoArrendamiento(true);
                        break;
                    case EAreaEquinova.RO:
                        this.vista.HabilitarPropietario(false);
                        this.vista.ModificaEtiquetaPropietario(false);
                        this.vista.HabilitarCliente(false);
                        int? ClienteID = this.ObtenerClienteId(this.vista.NombreClienteUnidadOperativa);
                        string ClienteNombre = (ClienteID != null) ? this.vista.NombreClienteUnidadOperativa : "";
                        this.vista.Cliente = ClienteNombre;
                        this.vista.ClienteId = ClienteID;
                        this.vista.Propietario = ClienteNombre;
                        this.vista.PropietarioId = ClienteID;
                        this.vista.HabilitarEntraMantenimiento(false);
                        this.vista.ActivaCheckMantenimiento(true);
                        this.vista.MostrarOrdenCompra(false);
                        this.vista.HabilitarCargaArchivoOC(false);
                        this.vista.HabilitaMontoArrendamiento(false);
                        this.vista.HabilitaTipoMonedas(false);
                        this.vista.MostrarTipoMoneda(false);
                        this.vista.HabilitaFechasArrendamiento(false);
                        this.vista.ReiniciaMontoArrendamiento();
                        this.vista.MostrarMontoArrendamiento(false);
                        break;
                }
            }  
        }

        #endregion

        public void SeleccionarArea(int? valor)
        {
            EArea? area = null;
            if (valor != null) area = (EArea)Enum.Parse(typeof(EArea), valor.ToString());
            
            switch (area)
            {
                case null:
                    this.vista.HabilitarCliente(false);
                    this.vista.HabilitarPropietario(false);

                    this.vista.Propietario = "";
                    this.vista.Cliente = "";
                    this.vista.PropietarioId = null;
                    this.vista.ClienteId = null;
                    break;
                case EArea.CM:
                case EArea.SD:
                    this.vista.HabilitarPropietario(true);
                    this.vista.HabilitarCliente(false);

                    this.vista.Propietario = "";
                    this.vista.Cliente = "";
                    this.vista.PropietarioId = null;
                    this.vista.ClienteId = null;
                    break;
                case EArea.RD:
                    this.vista.HabilitarPropietario(false);
                    this.vista.HabilitarCliente(false);

                    int? id = this.ObtenerClienteId(this.vista.NombreClienteUnidadOperativa);
                    string nombre = (id != null) ? this.vista.NombreClienteUnidadOperativa : "";
                    
                    this.vista.Propietario = nombre;
                    this.vista.Cliente = nombre;
                    this.vista.PropietarioId = id;
                    this.vista.ClienteId = id;
                    break;
                case EArea.FSL:
                    this.vista.HabilitarPropietario(false);
                    this.vista.HabilitarCliente(true);

                    this.vista.Propietario = this.vista.NombreClienteUnidadOperativa;
                    this.vista.Cliente = "";
                    this.vista.ClienteId = null;
                    break;
            }
        }
        private int? ObtenerClienteId(string nombre)
        {
            int? id = null;

            if (!string.IsNullOrEmpty(nombre) && !string.IsNullOrWhiteSpace(nombre))
            {
                List<ClienteBO> lst = FacadeBR.ConsultarCliente(this.dctx, new ClienteBO() { Nombre = nombre });
                if (lst.Count > 0)
                    id = lst[0].Id;
            }

            return id;
        }

        public string ValidarCamposBorrador()
        {
            string s = "";

            if (s.Trim().CompareTo("") != 0)
                return "Los siguientes campos no pueden estar vacíos: \n" + s.Substring(0, s.Length - 2);

            return null;
        }

        #region [RQM 13285- Modificación para Integración Construcción y Generacción]

        public string ValidarCamposRegistro()
        {
            this.vista.EnumTipoEmpresa = (ETipoEmpresa)this.vista.UnidadOperativaId;
            string s = "";

            if (this.vista.EnumTipoEmpresa == ETipoEmpresa.Idealease)
            {
                if (this.vista.Area == null)
                    s += "Área/Departamento, ";
            }

            if(this.vista.EnumTipoEmpresa == ETipoEmpresa.Construccion)
            {
                if (this.vista.ETipoRentaConstruccion == null )
                    s += "Tipo de Renta, ";
            }

            if (this.vista.EnumTipoEmpresa == ETipoEmpresa.Generacion)
            {
                if (this.vista.ETipoRentaGeneracion == null)
                    s += "Tipo de Renta, ";
            }

            if (this.vista.EnumTipoEmpresa == ETipoEmpresa.Equinova) {
                if (this.vista.ETipoRentaEquinova == null)
                    s += "Tipo de Renta, ";
            }

            if (this.vista.SucursalId == null)
                s += "Sucursal, ";
            if (this.vista.UnidadOperativaId == null)
                s += "Unidad Operativa, ";
            if (this.vista.ClienteId == null)
                s += "Cliente, ";

            if (this.vista.EnumTipoEmpresa == ETipoEmpresa.Construccion || this.vista.EnumTipoEmpresa == ETipoEmpresa.Generacion
                || this.vista.EnumTipoEmpresa == ETipoEmpresa.Equinova)
            {
                if (this.vista.ETipoRentaConstruccion == EAreaConstruccion.RE || this.vista.ETipoRentaGeneracion == EAreaGeneracion.RE
                    || this.vista.ETipoRentaEquinova == EAreaEquinova.RE)
                {
                    if (!(this.vista.Propietario != null && this.vista.Propietario.Trim().CompareTo("") != 0))
                        s += "Proveedor, ";

                    if (this.vista.FechaInicioArrendamiento == null)
                        s += "Fecha Inicio Arrendamiento, ";

                    if (this.vista.FechaFinArrendamiento == null)
                        s += "Fecha Fin Arrendamiento, ";
                }
                else
                {
                    if (!(this.vista.Propietario != null && this.vista.Propietario.Trim().CompareTo("") != 0))
                        s += "Propietario, ";
                }

                if (this.vista.CodigoMoneda == null)
                    s += "Tipo Moneda, ";

            }
            else
            {
                if (!(this.vista.Propietario != null && this.vista.Propietario.Trim().CompareTo("") != 0))
                    s += "Propietario, ";
            }


            if (s.Trim().CompareTo("") != 0)
                return "Los siguientes campos no pueden estar vacíos: \n" + s.Substring(0, s.Length - 2);

            return null;
        }

        #endregion

        public string ValidarCamposConsultaCliente()
        {
            string s = "";

            if (!(this.vista.Cliente != null && this.vista.Cliente.Trim().CompareTo("") != 0))
                s += "Cliente, ";

            if (s.Trim().CompareTo("") != 0)
                return "Los siguientes campos no pueden estar vacíos: \n" + s.Substring(0, s.Length - 2);

            return null;
        }

        #region [RQM 13285- Modificación para Integración Construcción y Generacción]
        
        public string ValidarCamposConsultaPropietario()
        {
            this.vista.EnumTipoEmpresa = (ETipoEmpresa)this.vista.UnidadOperativaId;
            string s = "";

            if (this.vista.EnumTipoEmpresa == ETipoEmpresa.Idealease)
            {
                if (!(this.vista.Propietario != null && this.vista.Propietario.Trim().CompareTo("") != 0))
                    s += "Propietario, ";
            }
            if (this.vista.EnumTipoEmpresa == ETipoEmpresa.Construccion && this.vista.ETipoRentaConstruccion == EAreaConstruccion.RE)
            {
                if (!(this.vista.Propietario != null && this.vista.Propietario.Trim().CompareTo("") != 0))
                    s += "Proveedor, ";
            }
            if (this.vista.EnumTipoEmpresa == ETipoEmpresa.Generacion && this.vista.ETipoRentaGeneracion == EAreaGeneracion.RE)
            {
                if (!(this.vista.Propietario != null && this.vista.Propietario.Trim().CompareTo("") != 0))
                    s += "Proveedor, ";
            }
            if (this.vista.EnumTipoEmpresa == ETipoEmpresa.Equinova && this.vista.ETipoRentaEquinova == EAreaEquinova.RE) {
                if (!(this.vista.Propietario != null && this.vista.Propietario.Trim().CompareTo("") != 0))
                    s += "Proveedor, ";
            }

            if (s.Trim().CompareTo("") != 0)
                return "Los siguientes campos no pueden estar vacíos: \n" + s.Substring(0, s.Length - 2);

            return null;
        }
        #endregion

        public void LimpiarSesion()
        {
            this.vista.LimpiarSesion();
            
        }

        #region Métodos para el Buscador
        public object PrepararBOBuscador(string catalogo)
        {
            object obj = null;

            switch (catalogo)
            {
                case "Propietario":
                    ClienteBO cPropietario = new ClienteBO();

                    cPropietario.Nombre = this.vista.Propietario;
                    cPropietario.Activo = true;

                    obj = cPropietario;
                    break;
                case "Cliente":
                    ClienteBO cCliente = new ClienteBO();

                    cCliente.Nombre = this.vista.Cliente;
                    cCliente.Activo = true;

                    obj = cCliente;
                    break;
                case "Sucursal":
                    SucursalBOF sucursal = new SucursalBOF();
                    sucursal.UnidadOperativa = new UnidadOperativaBO();
                    sucursal.Usuario = new UsuarioBO();

                    sucursal.Nombre = this.vista.SucursalNombre;
                    sucursal.UnidadOperativa.Id = this.vista.UnidadOperativaId;
                    sucursal.Activo = true;
                    sucursal.Usuario.Id = this.vista.UsuarioAutenticado;

                    obj = sucursal;
                    break;
            }

            return obj;
        }
        public void DesplegarResultadoBuscador(string catalogo, object selecto)
        {
            switch (catalogo)
            {
                case "Propietario":
                    #region Desplegar Propietario
                    ClienteBO cPropietario = (ClienteBO)selecto;

                    if (cPropietario != null && cPropietario.Nombre != null)
                        this.vista.Propietario = cPropietario.Nombre;
                    else
                        this.vista.Propietario = null;

                    if (cPropietario != null && cPropietario.Id != null)
                        this.vista.PropietarioId = cPropietario.Id;
                    else
                        this.vista.PropietarioId = null;

                    if (this.vista.Area != null && (this.vista.Area == EArea.SD || this.vista.Area == EArea.CM || this.vista.Area == EArea.RD))
                    {
                        this.vista.Cliente = this.vista.Propietario;
                        this.vista.ClienteId = this.vista.PropietarioId;
                    }
                    #endregion
                    break;
                case "Cliente":
                    #region Desplegar Cliente
                    ClienteBO cCliente = (ClienteBO)selecto;

                    if (cCliente != null && cCliente.Nombre != null)
                        this.vista.Cliente = cCliente.Nombre;
                    else
                        this.vista.Cliente = null;

                    if (cCliente != null && cCliente.Id != null)
                        this.vista.ClienteId = cCliente.Id;
                    else
                        this.vista.ClienteId = null;
                    #endregion
                    break;
                case "Sucursal":
                    #region Desplegar Sucursal
                    SucursalBO sucursal = (SucursalBO)selecto;

                    if (sucursal != null && sucursal.Nombre != null)
                        this.vista.SucursalNombre = sucursal.Nombre;
                    else
                        this.vista.SucursalNombre = null;

                    if (sucursal != null && sucursal.Id != null)
                        this.vista.SucursalId = sucursal.Id;
                    else
                        this.vista.SucursalId = null;
                    #endregion
                    break;
            }
        }
        #endregion
        

        #region [RQM 13285- Integración Construcción y Generacción]

        /// <summary>
        /// Determina las acciones relacionadas con el comportamiento de las vistas.
        /// </summary>
        /// <param name="listaAcciones">Lista de objetos de tipo CatalogoBaseBO que contiene las acciones a las cuales el usuario tiene permiso.</param>
        public void EstablecerAcciones(List<CatalogoBaseBO> listaAcciones)
        {
            
            this.EstablecerAcciones(listaAcciones);
            
        }

        /// <summary>
        /// Realiza la validación de permisos de acciones
        /// </summary>
        /// <param name="listaAcciones">Recibe lista de permisos</param>
        /// <param name="modo">Recibe el modo ejecutado</param>
        public void EstablecerAcciones(List<CatalogoBaseBO> listaAcciones, string modo)
        {
            this.vista.EnumTipoEmpresa = (ETipoEmpresa)this.vista.UnidadOperativaId;
            switch (this.vista.EnumTipoEmpresa)
            {
                case ETipoEmpresa.Generacion:
                    if (ExisteAccion(listaAcciones, "REGISTRARDOCUMENTO"))
                    {
                        this.vista.EnumTipoEmpresa = ETipoEmpresa.Generacion;
                    }
                    break;
                case ETipoEmpresa.Equinova:
                    if (ExisteAccion(listaAcciones, "REGISTRARDOCUMENTO")) {
                        this.vista.EnumTipoEmpresa = ETipoEmpresa.Equinova;
                    }
                    break;
                case ETipoEmpresa.Construccion:
                    if (ExisteAccion(listaAcciones, "REGISTRARDOCUMENTO"))
                    {
                        this.vista.EnumTipoEmpresa = ETipoEmpresa.Construccion;
                    }
                    break;
                default:
                    this.vista.EnumTipoEmpresa = ETipoEmpresa.Idealease;
                    break;
            }
            this.vista.EstablecerAcciones(modo);

        }

        /// <summary>
        /// Verifica que una acción a avaluar exista en el listado de acciones asignadas al usuario.
        /// </summary>
        /// <param name="acciones">Lista de acciones asignadas al usuario.</param>
        /// <param name="nombreAccion">Acción a evaluar</param>
        /// <returns>Devuelve true en caso de existir la acción a evaluar en el listado de acciones, en caso contrario regresa false.</returns>
        private bool ExisteAccion(List<CatalogoBaseBO> acciones, string nombreAccion)
        {
            if (acciones != null && acciones.Count > 0 && acciones.Exists(p => p != null && p.Nombre != null && p.Nombre.Trim().ToUpper() == nombreAccion.Trim().ToUpper()))
                return true;

            return false;
        }

        /// <summary>
        /// Despliega los tipos de archivo
        /// </summary>
        public void DesplegarTiposArchivos(List<TipoArchivoBO> tipos)
        {
            try
            {
                this.vista.CargarTiposArchivos(tipos);
            }
            catch (Exception ex)
            {
                throw new Exception(nombreClase + ".DesplegarTiposArchivos:" + ex.Message);
            }
        }

        public void HabilitarCargaArchivoOC(bool habilitar)
        {
            this.vista.HabilitarCargaArchivoOC(habilitar);
        }

        public void EstablecerTiposdeArchivo(List<TipoArchivoBO> tipos)
        {
            this.vista.TiposArchivo = tipos;
        }
        public void EstablecerTipoAdjunto()
        {
            this.vista.EstablecerTipoAdjunto(ETipoAdjunto.OC);
        }
        public void ModoEdicion(bool habilitar)
        {
            this.vista.ModoEdicion(habilitar);
        }
        #endregion
        #endregion
    }
}
