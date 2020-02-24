using BPMO.SDNI.Facturacion.AplicacionesFacturacion.VIS;
using BPMO.Patterns.Creational.DataContext;
using BPMO.SDNI.Facturacion.AplicacionesFacturacion.BO;
using BPMO.SDNI.Facturacion.AplicacionesFacturacion.BR;
using BPMO.Facade.SDNI.BR;
using System;
using BPMO.Primitivos.Enumeradores;
using BPMO.Basicos.BO;
using System.Data;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace BPMO.SDNI.Facturacion.AplicacionesFacturacion.PRE {
    /// <summary>
    /// Clase que maneja el presentador del reporte
    /// </summary>
    public class ReporteIngresoRentasPRE {
        #region Atributos
        /// <summary>
        /// Objeto que establece los datos de la vista
        /// </summary>
        private IReporteIngresoRentasVIS vista;
        /// <summary>
        /// Objeto que contiene el contexto de la conexion a la base de datos
        /// </summary>
        private IDataContext dataContext;

        /// <summary>
        /// Objeto que maneje la logica de negocio
        /// </summary>
        private ReporteIngresoRentasBR reporteIngresoBR;
        #endregion

        #region Contructor
        public ReporteIngresoRentasPRE(IReporteIngresoRentasVIS vista) {
            this.vista = vista;
            dataContext = FacadeBR.ObtenerConexion();
            reporteIngresoBR = new ReporteIngresoRentasBR();
        }
       
        #endregion

        #region  Metodos     
                
        /// <summary>
        /// Copia la informacion de la vista al modelo
        /// </summary>
        /// <param name="vista">Datos que contiene la vista</param>
        /// <returns>Devuelve un objeto mapeado de la vista y el modelo</returns>
        private Dictionary<string, object> DataToModel(IReporteIngresoRentasVIS vista) {
            Dictionary<string, object> data = new Dictionary<string, object>();
            data["sucursalId"] = vista.SucursalID;
            data["sucursal"] = vista.Sucursal;
            DateTime fecha = (DateTime)vista.FechaInicio;
            DateTime fechaIni = fecha.AddHours(-fecha.Hour).AddMinutes(-fecha.Minute).AddSeconds(-fecha.Second);
            data["FechaInicio"] = fechaIni;
            fecha = (DateTime)vista.FechaFin;
            DateTime fechaFin = fecha.AddHours(23).AddMinutes(59).AddSeconds(59);
            data["FechaFin"] = fechaFin;
            data["UnidadOperativaId"] = vista.UnidadOperativaID;
            if (vista.SucursalID == null)
                data["sucursalesAutorizadas"] = this.ObtenerSucursalesAutorizadas().ConvertAll(x => (SucursalBO)x); 
            return data;
        }
        /// <summary>8
        /// Ejecuta una consulta a la base de datos
        /// </summary>
        public void Consultar() {
            try {
                #region Validar requeridos
                if (this.vista.FechaInicio == null || this.vista.FechaFin == null){
                    this.vista.MostrarMensaje("Debe definir un rango de FECHAS.", ETipoMensajeIU.ADVERTENCIA);
                    return;
                }
                if (this.vista.FechaInicio > this.vista.FechaFin){
                    this.vista.MostrarMensaje("FECHA INICIO no puede ser mayor FECHA FIN.", ETipoMensajeIU.ADVERTENCIA);
                    return;
                }
                #endregion
                var data = DataToModel(vista);
                var paquete = reporteIngresoBR.ConsultarIngresoRenta(dataContext, data);
                DataSet ds = (DataSet)paquete["datasource"];
                this.ObtenerUsuario(ds.Tables[0]);
                paquete["datasource"] = ds;
                paquete["UO"] = this.vista.Adscripcion.UnidadOperativa.Nombre;
                paquete["Logo"] = vista.URLImage;
                paquete["Usuario"] = this.ObtenerUsuarioSession(vista.UsuarioID).Nombre;
                paquete["RangoFechas"] = (data["FechaInicio"] != null && data["FechaFin"] != null) ?
                    string.Format("{0} al {1}", DateTime.Parse(data["FechaInicio"].ToString()).ToShortDateString(), DateTime.Parse(data["FechaFin"].ToString()).ToShortDateString()) :
                    string.Empty;
                paquete["Sucursal"] = (!string.IsNullOrEmpty(vista.Adscripcion.Sucursal.Nombre)) ? vista.Adscripcion.Sucursal.Nombre : ObtenerSucursalesConsulta(ds.Tables["ResumenIngreso"]);
                this.vista.EstablecerPaqueteNavegacionImprimir("ReporteIngresoRentas", paquete);
                this.vista.IrAImprimir();
            }
            catch(Exception e) {
                this.vista.MostrarMensaje("Error al consultar información.", ETipoMensajeIU.ERROR, e.Message);
            }
        }
        /// <summary>
        /// Obtiene los identificadores de las sucursales de consulta
        ///<summary>
        private string ObtenerSucursalesConsulta(DataTable sucursalesConsulta) {
            StringBuilder sucursales = new StringBuilder(string.Empty);
            string nombres = string.Empty;
            if (!ReferenceEquals(sucursalesConsulta, null)) {
                foreach (DataRow sucursal in sucursalesConsulta.Rows) {
                    sucursales.Append(", " + sucursal["sucursal"]);
                }
            }
            if (sucursales.Length > 0) {
                if (sucursales.ToString().StartsWith(", ")) {
                    nombres = sucursales.ToString().Substring(2);
                }
            }
            return nombres;
        }
        /// <summary>
        /// Recupera las sucursales a las que el usuario autenticado tiene permiso e acceder 
        /// </summary>
        private List<SucursalBO> ObtenerSucursalesAutorizadas() {
            List<SucursalBO> sucursalesAutorizadas = new List<SucursalBO>();
                sucursalesAutorizadas = FacadeBR.ConsultarSucursalesSeguridadSimple(
                    dataContext, new SeguridadBO(Guid.Empty, new UsuarioBO() { Id = this.vista.UsuarioID }, vista.Adscripcion));
            
           return sucursalesAutorizadas;
        }

        private void ObtenerUsuario(DataTable dt) {
            UsuarioBO usuario;
            dt.Columns.Add("promotor");
            foreach (DataRow row in dt.Rows) {
                usuario = new UsuarioBO();
                usuario.Id = Int32.Parse(row["UC"].ToString());
                usuario = FacadeBR.ConsultarUsuario(dataContext, usuario).FirstOrDefault();
                row["promotor"] = usuario.Nombre;
            }
            dt.AcceptChanges();
        }

        private UsuarioBO ObtenerUsuarioSession(int? UsuarioID) {
            UsuarioBO usuario = new UsuarioBO(){
                Id = UsuarioID
            };
            return FacadeBR.ConsultarUsuario(dataContext, usuario).FirstOrDefault();
        }

        private UnidadOperativaBO ObtenerUnidadOperativa(int? UnidadOperativaID) {
            UnidadOperativaBO unidadOperativa = new UnidadOperativaBO() { Id = UnidadOperativaID };
            return FacadeBR.ConsultarUnidadOperativa(dataContext, unidadOperativa).FirstOrDefault();
        }
        #endregion
    }
}
