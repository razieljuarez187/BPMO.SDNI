using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using BPMO.Basicos.BO;
using BPMO.Facade.SDNI.BOF;
using BPMO.Facade.SDNI.BR;
using BPMO.Patterns.Creational.DataContext;
using BPMO.Primitivos.Enumeradores;
using BPMO.SDNI.Comun.BO;
using BPMO.SDNI.Comun.BR;
using BPMO.SDNI.Equipos.BOF;
using BPMO.SDNI.Equipos.BR;
using BPMO.SDNI.Flota.VIS;
using OfficeOpenXml;


namespace BPMO.SDNI.Flota.Reportes.PRE {
    public class ConsultarControlRentasPSLPRE {
        #region Propiedades

        private IConsultarControlRentasPSLVIS vista;
        private IDataContext dctx = null;
        private UnidadBR controlador;
        private string nombreClase = "ConsultarControlRentasPSLPRE";

        #endregion

        #region Constructor

        public ConsultarControlRentasPSLPRE(IConsultarControlRentasPSLVIS vistaActual) {
            try {
                if (vistaActual != null) vista = vistaActual;
                else throw new Exception("La vista proporcionada no puede ser nula.");

                dctx = Facade.SDNI.BR.FacadeBR.ObtenerConexion();
                this.controlador = new UnidadBR();
            } catch (Exception ex) {
                vista.MostrarMensaje("Inconsistencias en la construcción del presentador", ETipoMensajeIU.ERROR, nombreClase + ".ConsultarControlRentasPSLPRE: " + ex.Message);
            }

        }

        #endregion

        #region Métodos

        public void PrepararBusqueda() {
            this.DesplegarListadoEstatus();
            this.vista.LimpiarSesion();
            this.vista.PrepararBusqueda();
            this.EstablecerSeguridad();
        }

        /// <summary>
        /// Prepara el listado de Estatus para mostrarlo en la UI
        /// </summary>
        private void DesplegarListadoEstatus() {
            List<EEstatusUnidad> listado = new List<EEstatusUnidad>();
            listado = new List<EEstatusUnidad>(Enum.GetValues(typeof(EEstatusUnidad)).Cast<EEstatusUnidad>());
            vista.CargarEstatus(listado);
        }

        private void EstablecerInformacionInicial() {
            try {
                #region Configuraciones de Unidad Operativa
                //Obtener las configuraciones de la unidad operativa
                List<ConfiguracionUnidadOperativaBO> lstConfigUO = new ModuloBR().ConsultarConfiguracionUnidadOperativa(this.dctx, new ConfiguracionUnidadOperativaBO() { UnidadOperativa = new UnidadOperativaBO() { Id = this.vista.UnidadOperativaId } }, this.vista.ModuloID);
                if (lstConfigUO.Count <= 0)
                    throw new Exception("No se encontraron las configuraciones del sistema para la unidad operativa en la que trabaja.");

                #endregion
            } catch (Exception ex) {
                throw new Exception(this.nombreClase + ".EstablecerInformacionInicial: " + ex.Message);
            }
        }

        private void EstablecerSeguridad() {
            try {
                //se valida que los datos del usuario y la unidad operativa no sean nulos
                if (this.vista.UsuarioID == null) throw new Exception("El identificador del usuario no debe ser nulo");
                if (this.vista.UnidadOperativaId == null) throw new Exception("El identificador de la Unidad Operativa no debe ser nulo ");

                //crea seguridad
                UsuarioBO usuario = new UsuarioBO { Id = this.vista.UsuarioID };
                AdscripcionBO adscripcion = new AdscripcionBO { UnidadOperativa = new UnidadOperativaBO { Id = this.vista.UnidadOperativaId } };
                SeguridadBO seguridad = new SeguridadBO(Guid.Empty, usuario, adscripcion);

                //consulta lista de acciones permitidas
                this.vista.ListaAcciones = FacadeBR.ConsultarAccion(dctx, seguridad);

                //Se valida si el usuario tiene permiso para consultar
                if (!this.ExisteAccion(this.vista.ListaAcciones, "CONSULTAR"))
                    this.vista.RedirigirSinPermisoAcceso();
            } catch (Exception ex) {
                throw new Exception(this.nombreClase + ".EstablecerSeguridad:" + ex.Message);
            }
        }


        public void Consultar() {
            try {
                UnidadBOF bo = (UnidadBOF)this.InterfazUsuarioADato();

                DataSet ds = this.controlador.ConsultarReporteRentas(dctx, bo, this.vista.UnidadOperativaId);

                if (ds.Tables[0].Rows.Count <= 0) {
                    this.vista.MostrarMensaje("La búsqueda no produjo resultados", ETipoMensajeIU.INFORMACION,
                        "No se encontraron coincidencias con los criterios ingresados. Verifique sus datos");
                    this.vista.LimpiarSesion();
                    this.vista.ActualizarResultado(this.vista.UnidadRentas);
                } else {
                    this.ArmarReporteCompleto(ds);
                    DataTable dtb = ReporteRentas(ds);
                    this.vista.UnidadRentas = dtb;
                    this.vista.ActualizarResultado(dtb);
                }
            } catch (Exception ex) {
                throw new Exception(this.nombreClase + ".Consultar: " + ex.Message);
            }
        }


        private UnidadBOF InterfazUsuarioADato() {
            UnidadBOF bo = new UnidadBOF();
            bo.Sucursal = new SucursalBO();
            bo.Sucursal.UnidadOperativa = new UnidadOperativaBO();

            if (this.vista.Estatus.HasValue)
                if (this.vista.Estatus.Value >= 0)
                    bo.EstatusActual = (EEstatusUnidad)this.vista.Estatus;

            bo.Sucursal.Id = this.vista.SucursalID;
            bo.Sucursal.UnidadOperativa.Id = this.vista.UnidadOperativaId;

            if (vista.SucursalID != null) {
                bo.Sucursal.Id = this.vista.SucursalID;
                bo.Sucursal.Nombre = this.vista.SucursalNombre;
            }

            return bo;
        }

        #region Métodos para el Buscador
        public object PrepararBOBuscador(string catalogo) {
            object obj = null;

            switch (catalogo) {
                case "Sucursal":
                    SucursalBOF sucursal = new SucursalBOF();
                    sucursal.UnidadOperativa = new UnidadOperativaBO();
                    sucursal.UnidadOperativa.Id = this.vista.UnidadOperativaId;
                    sucursal.Nombre = this.vista.SucursalNombre;
                    sucursal.Usuario = new UsuarioBO() { Id = this.vista.UsuarioID };
                    obj = sucursal;
                    break;
            }

            return obj;
        }

        public void DesplegarResultadoBuscador(string catalogo, object selecto) {
            switch (catalogo) {
                case "Sucursal":
                    SucursalBO sucursal = (SucursalBO)selecto;
                    if (sucursal != null && sucursal.Id != null)
                        this.vista.SucursalID = sucursal.Id;
                    else
                        this.vista.SucursalID = null;

                    if (sucursal != null && sucursal.Nombre != null)
                        this.vista.SucursalNombre = sucursal.Nombre;
                    else
                        this.vista.SucursalNombre = null;
                    break;
            }
        }
        #endregion

        private bool ExisteAccion(List<CatalogoBaseBO> acciones, string nombreAccion) {
            if (acciones != null && acciones.Count > 0 &&
                acciones.Exists(
                    p => p != null && p.Nombre != null && p.Nombre.Trim().ToUpper() == nombreAccion.Trim().ToUpper()))
                return true;

            return false;
        }

        public void EstablecerAcciones() {
            ETipoEmpresa EmpresaConPermiso = ETipoEmpresa.Idealease;
            switch (this.vista.UnidadOperativaId) {
                case (int)ETipoEmpresa.Generacion:
                    if (ExisteAccion(this.vista.ListaAcciones, "CONSULTAR")) {
                        EmpresaConPermiso = ETipoEmpresa.Generacion;
                    }
                    break;
                case (int)ETipoEmpresa.Equinova:
                    if (ExisteAccion(this.vista.ListaAcciones, "CONSULTAR")) {
                        EmpresaConPermiso = ETipoEmpresa.Equinova;
                    }
                    break;
                case (int)ETipoEmpresa.Construccion:
                    if (ExisteAccion(this.vista.ListaAcciones, "CONSULTAR")) {
                        EmpresaConPermiso = ETipoEmpresa.Construccion;
                    }
                    break;
            }
            this.vista.EstablecerAcciones(EmpresaConPermiso);
        }

        /// <summary>
        /// Genera un datatable con los datos del reporte para visualizarlos en el gridview
        /// </summary>
        /// <param name="ds">Recibe un dataset con los datos del reporte</param>
        /// <returns>Retorno un datatable con los datos del reporte para visualizarlos en el gridview</returns>
        public DataTable ReporteRentas(DataSet ds) {

            DataTable dt = new DataTable();
            dt.Columns.Add(new DataColumn("SUCURSAL"));
            dt.Columns.Add(new DataColumn("ECODE"));
            dt.Columns.Add(new DataColumn("DESCRIPCION"));
            dt.Columns.Add(new DataColumn("#SERIE"));
            dt.Columns.Add(new DataColumn("ANIOUNIDAD"));
            dt.Columns.Add(new DataColumn("ESTATUS"));
            dt.Columns.Add(new DataColumn("¿SUBALQUILADO?"));
            dt.Columns.Add(new DataColumn("CLIENTE"));
            dt.Columns.Add(new DataColumn("CONTRATO"));
            dt.Columns.Add(new DataColumn("ASESOR"));
            dt.Columns.Add(new DataColumn("FECHAVENCIMIENTO"));
            dt.Columns.Add(new DataColumn("HOROMETRO"));
            foreach (DataRow ea in ds.Tables[0].Rows) {

                int? usuarioID = null;
                if (!ea.IsNull("AsesorID"))
                    usuarioID = (int)Convert.ChangeType(ea["AsesorID"], typeof(int));

                UsuarioBO usuario = new UsuarioBO();
                if (usuarioID != null) {
                    UsuarioBO filtroUsuario = new UsuarioBO()
                    {
                        Id = usuarioID
                    };
                    usuario = FacadeBR.ConsultarUsuario(this.dctx, filtroUsuario).FirstOrDefault();
                }

                DataRow dr = dt.NewRow();
                dr["SUCURSAL"] = ea["Sucursal"];
                dr["ECODE"] = ea["NumeroEconomico"];
                dr["DESCRIPCION"] = ea["Descripcion"];
                dr["#SERIE"] = ea["Serie"];
                dr["ANIOUNIDAD"] = ea["Anio"];
                dr["ESTATUS"] = ea["EstatusUnidad"];
                dr["¿SUBALQUILADO?"] = ea["TipoRenta"].ToString() != "RE" ? "NO" : "SI";
                dr["CLIENTE"] = ea["Cliente"];
                dr["CONTRATO"] = ea["NumeroContrato"];
                if (usuario.Usuario != null)
                    dr["ASESOR"] = usuario.Nombre.ToString();
                else
                    dr["ASESOR"] = string.Empty;
                dr["FECHAVENCIMIENTO"] = ea["FechaVencimiento"];
                dr["HOROMETRO"] = ea["Horometro"];
                dt.Rows.Add(dr);
            }
            dt.AcceptChanges();

            return dt;
        }

        /// <summary>
        /// Método donde se ejecutan las funciones para generar el reporte
        /// </summary>
        public void ExportarReporteExcel() {
            try {
                if (this.vista.ReporteRentas != null) {
                    //se valida que los datos del usuario y la unidad operativa no sean nulos
                    if (this.vista.UsuarioID == null) throw new Exception("El identificador del usuario no debe ser nulo");
                    if (this.vista.UnidadOperativaId == null) throw new Exception("El identificador de la Unidad Operativa no debe ser nulo ");

                    //crea seguridad
                    UsuarioBO usuario = new UsuarioBO { Id = this.vista.UsuarioID };
                    AdscripcionBO adscripcion = new AdscripcionBO { UnidadOperativa = new UnidadOperativaBO { Id = this.vista.UnidadOperativaId } };
                    SeguridadBO seguridad = new SeguridadBO(Guid.Empty, usuario, adscripcion);
                    this.vista.ListaAcciones = FacadeBR.ConsultarAccion(dctx, seguridad);
                    
                    bool reporte = false;
                    
                    if (ExisteAccion(this.vista.ListaAcciones, "REPORTERENTAS"))
                    {
                        if (ExisteAccion(this.vista.ListaAcciones, "REPORTERENTASCOMPLETO"))
                        {
                            DataSet ds = this.vista.ReporteRentas;
                            DataTable dt = ds.Tables[0];
                            this.GeneraReporteExcel(dt, "RPTRENTASCOMPLETO");
                            reporte = true;
                        }
                        else
                        {
                            this.GeneraReporteExcel(this.vista.UnidadRentas, "RPTRENTAS");
                            reporte = true;
                        }
                    }
                    else if (ExisteAccion(this.vista.ListaAcciones, "REPORTERENTASCOMPLETO") && reporte == false)
                    {
                        DataSet ds = this.vista.ReporteRentas;
                        DataTable dt = ds.Tables[0];
                        this.GeneraReporteExcel(dt, "RPTRENTASCOMPLETO");
                    }
                    else
                        this.vista.MostrarMensaje("No Cuenta con permisos para exportar el reporte.", ETipoMensajeIU.ADVERTENCIA, null);


                } else
                    this.vista.MostrarMensaje("No se encontraron resultados a exportar.", ETipoMensajeIU.ADVERTENCIA, null);
            } catch (Exception ex) {
                throw new Exception(this.nombreClase + ".ExportarReporteExcel: " + ex.Message);
            }
        }

        public void CargarDatos() {
            this.vista.GvUnidadesRentas.DataSource = this.vista.UnidadRentas;
            this.vista.GvUnidadesRentas.DataBind();
        }

        /// <summary>
        /// Arma en un DataTable con los resultados del reporte
        /// </summary>
        /// <param name="dts">Recibe un dataset con los resultados del reporte</param>
        public void ArmarReporteCompleto(DataSet dts) {

            DataTable dt = new DataTable();
            dt.Columns.Add(new DataColumn("SUCURSAL"));
            dt.Columns.Add(new DataColumn("ECODE"));
            dt.Columns.Add(new DataColumn("FECHAINICIORE"));
            dt.Columns.Add(new DataColumn("FECHAFINRE"));
            dt.Columns.Add(new DataColumn("DESCRIPCION"));
            dt.Columns.Add(new DataColumn("#SERIE"));
            dt.Columns.Add(new DataColumn("MARCA"));
            dt.Columns.Add(new DataColumn("ANIOUNIDAD"));
            dt.Columns.Add(new DataColumn("FECHADEALTA"));
            dt.Columns.Add(new DataColumn("VALORENLIBROS"));
            dt.Columns.Add(new DataColumn("ESTATUS"));
            dt.Columns.Add(new DataColumn("CLIENTE"));
            dt.Columns.Add(new DataColumn("CONTRATO"));
            dt.Columns.Add(new DataColumn("ASESOR"));
            dt.Columns.Add(new DataColumn("FECHAINICIO"));
            dt.Columns.Add(new DataColumn("FECHAVENCIMIENTO"));
            dt.Columns.Add(new DataColumn("CODIGOORACLE"));
            dt.Columns.Add(new DataColumn("MESESENLAFLOTA"));
            dt.Columns.Add(new DataColumn("HOROMETRO"));
            dt.Columns.Add(new DataColumn("GPS"));
            dt.Columns.Add(new DataColumn("TIPO"));

            foreach (DataRow ea in dts.Tables[0].Rows) {
                int? usuarioID = null;
                if (!ea.IsNull("AsesorID"))
                    usuarioID = (int)Convert.ChangeType(ea["AsesorID"], typeof(int));

                UsuarioBO usuario = new UsuarioBO();
                if (usuarioID != null) {
                    UsuarioBO filtroUsuario = new UsuarioBO()
                    {
                        Id = usuarioID
                    };
                    usuario = FacadeBR.ConsultarUsuario(this.dctx, filtroUsuario).FirstOrDefault();
                }

                DataRow dr = dt.NewRow();
                dr["SUCURSAL"] = ea["Sucursal"];
                dr["ECODE"] = ea["NumeroEconomico"];
                dr["FECHAINICIORE"] = ea["FechaInicioRE"];
                dr["FECHAFINRE"] = ea["FechaFinRE"];
                dr["DESCRIPCION"] = ea["Descripcion"];
                dr["#SERIE"] = ea["Serie"];
                dr["MARCA"] = ea["marca"];
                dr["ANIOUNIDAD"] = ea["Anio"];
                dr["FECHADEALTA"] = ea["FechaAlta"];
                dr["VALORENLIBROS"] = ea["ValorEnLibros"];
                dr["ESTATUS"] = ea["EstatusUnidad"];
                dr["CLIENTE"] = ea["Cliente"];
                dr["CONTRATO"] = ea["NumeroContrato"];
                if (usuario.Usuario != null)
                    dr["ASESOR"] = usuario.Nombre.ToString();
                else
                    dr["ASESOR"] = string.Empty;
                dr["FECHAINICIO"] = ea["FechaInicio"];
                dr["FECHAVENCIMIENTO"] = ea["FechaVencimiento"];
                dr["CODIGOORACLE"] = ea["CodigoOracle"];
                dr["MESESENLAFLOTA"] = ea["MesesEnFlota"];
                dr["HOROMETRO"] = ea["Horometro"];
                dr["GPS"] = ea["GPS"];
                dr["TIPO"] = ea["TipoEquipo"];

                dt.Rows.Add(dr);
            }
            dt.AcceptChanges();

            DataSet dataTable = new DataSet();
            dataTable.Tables.Add(dt);

            this.vista.ReporteRentas = dataTable;
        }

        /// <summary>
        /// Genera el reporte a un archivo de Excel
        /// </summary>
        /// <param name="dtb"></param>
        /// <param name="tipoPermiso"></param>
        public void GeneraReporteExcel(DataTable dtb, string tipoPermiso) {
            using (ExcelPackage excel = new ExcelPackage()) {

                DataTable dtTable = ((DataTable)dtb).Copy();

                //Set some properties of the Excel document
                excel.Workbook.Properties.Author = "Bepensa";
                excel.Workbook.Properties.Title = "Reporte - Centro de Control de Rentas";
                excel.Workbook.Properties.Subject = "Reporte Sistema Idealease - Rental";
                excel.Workbook.Properties.Created = DateTime.Now;
                excel.Workbook.Worksheets.Add("Reporte Rentas Unidades");

                #region Encabezado del reporte

                var headerRow = new List<string[]>()
                {
                    new string[] { "NO DISPONIBLE" }
                };

                string headerRange = String.Format("A4:{0}4", Char.ConvertFromUtf32(headerRow[0].Length + 64));

                if (tipoPermiso == "RPTRENTAS") {
                    headerRow = new List<string[]>()
                    {
                        new string[] { "SUCURSAL", "ECODE", "DESCRIPCIÓN", "# SERIE", "AÑO UNIDAD", "ESTATUS", 
                                    "¿SUBALQUILADO?", "CLIENTE", "CONTRATO", "ASESOR", "FECHA VENCIMIENTO", "HÓROMETRO" }
                    };

                    headerRange = String.Format("A4:{0}4", Char.ConvertFromUtf32(headerRow[0].Length + 64));
                }

                if (tipoPermiso == "RPTRENTASCOMPLETO") {
                    headerRow = new List<string[]>()
                    {
                        new string[] { "SUCURSAL", "ECODE", "FECHA INICIO RE", "FECHA FIN RE", "DESCRIPCIÓN", 
                            "# SERIE", "MARCA", "AÑO UNIDAD", "FECHA DE ALTA", "VALOR EN LIBROS", "ESTATUS",
                            "CLIENTE", "CONTRATO", "ASESOR", "FECHA ARRENDAMIENTO", "FECHA VENCIMIENTO", 
                            "CÓDIGO ORACLE","MESES EN LA FLOTA","HORÓMETRO" , "GPS", "TIPO"
                        }
                    };

                    headerRange = String.Format("A4:{0}4", Char.ConvertFromUtf32(headerRow[0].Length + 64));
                }
                #endregion

                var worksheet = excel.Workbook.Worksheets["Reporte Rentas Unidades"];

                var modelRows = dtTable.Rows.Count + 3;

                string modelRange = "A4:L" + (modelRows + 1);

                if (tipoPermiso == "RPTRENTASCOMPLETO")
                    modelRange = "A4:U" + (modelRows + 1);

                var modelTable = worksheet.Cells[modelRange];

                modelTable.Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                modelTable.Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                modelTable.Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                modelTable.Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                modelTable.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Top;

                worksheet.Cells[1, 1].Value = "Distribuidora Megamak S.A de C.V.";
                worksheet.Cells[1, 5].Value = "Fecha: " + DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString();
                worksheet.Cells[2, 1].Value = "Reporte de Control de Rentas";

                if (dtTable.Rows.Count > 0) {
                    worksheet.Cells["A4"].LoadFromDataTable(dtTable, true);
                }

                worksheet.Cells[headerRange].LoadFromArrays(headerRow);
                worksheet.Cells[headerRange].Style.Font.Bold = true;
                worksheet.Cells[headerRange].Style.Font.Size = 12;
                worksheet.Cells[headerRange].Style.Font.Color.SetColor(System.Drawing.Color.White);
                worksheet.Cells[headerRange].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                worksheet.Cells[headerRange].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(0, 0, 255));
                worksheet.Cells[headerRange].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;

                worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();

                ////Wrap text in Guia column
                worksheet.Cells["G2:U" + modelRows].Style.WrapText = true;

                excel.Workbook.Properties.Title = "Reporte Centro de Control de Rentas";
                byte[] rpt = excel.GetAsByteArray();

                ///Download file
                this.vista.EstablecerPaqueteNavegacion("ReporteRentasExcel", rpt);
                this.vista.RegistraScript("EventOcultar", "RedireccionarPagina();");

                this.vista.MostrarMensaje("El reporte se ha exportado correctamente.", ETipoMensajeIU.INFORMACION, null);
            }

        }
        #endregion
    }
}
