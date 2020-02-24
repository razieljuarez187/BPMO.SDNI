// Satisface el caso de uso CU015 – Carga Masiva Facturación Contratos
// Satisface a la solicitud de cambio SC0013
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using BPMO.Basicos.BO;
using BPMO.Facade.SDNI.BOF;
using BPMO.Facade.SDNI.BR;
using BPMO.Patterns.Creational.DataContext;
using BPMO.Primitivos.Enumeradores;
using BPMO.SDNI.Contratos.BO;
using BPMO.SDNI.Facturacion.AplicacionesFacturacion.BO;
using BPMO.SDNI.Facturacion.AplicacionesFacturacion.VIS;
using BPMO.SDNI.Facturacion.AplicacionesFacturacion.BR;

namespace BPMO.SDNI.Facturacion.AplicacionesFacturacion.PRE
{
    /// <summary>
    /// Presentador para la vista de carga masiva de pagos facturados de contratos
    /// </summary>
    public partial class CargaMasivaContratosPRE
    {
        #region Constantes
        /// <summary>
        /// Nombre de la clase para agregar a los mensajes de Error
        /// </summary>
        private static readonly string nombreClase = typeof(CargaMasivaContratosPRE).Name;
        #endregion

        #region Atributos
        /// <summary>
        /// El DataContext que proveerá acceso a la base de datos
        /// </summary>
        private IDataContext dctx;

        /// <summary>
        /// La vista que esta mostrando los datos procesados en el presentador
        /// </summary>
        private ICargaMasivaContratosVIS vista;       

        /// <summary>
        /// controlador para carga masiva de pagos
        /// </summary>
        private CargaMasivaContratosBR cargaMasivaContratosBR;

        /// <summary>
        /// Lista de contratos incluidos en carga masiva
        /// </summary>
        public List<ContratoBO> ContratosCargaMasiva { get; set; }
        /// <summary>
        /// Bandera que se usa para reporcesar los pagos
        /// </summary>
        public bool Reprocesar { get; set; }
        #endregion       

        #region Propiedades
        
        #endregion       

        #region Constructores
        /// <summary>
        /// Constructor por default del presentador
        /// </summary>
        /// <param name="vista">Vista que será gestionada por el presentador</param>
        public CargaMasivaContratosPRE(ICargaMasivaContratosVIS vista)
        {           
            try
            {
                this.vista = vista;
                this.cargaMasivaContratosBR = new CargaMasivaContratosBR();
                this.dctx = FacadeBR.ObtenerConexion();
            }
            catch (Exception ex)
            {
                this.vista.MostrarMensaje("Inconsistencias en la configuración", ETipoMensajeIU.ERROR, nombreClase + ".CargaMasivaContratosPRE: " + ex.GetBaseException().Message);
            }
        }
        #endregion

        #region Métodos
        /// <summary>
        /// Realiza el proceso de carga del archivo subido
        /// </summary>
        public void CargarArchivo()
        {
            if (!this.vista.ValidarCampos())
                return;

            //Se crea el objeto de seguridad                
            SeguridadBO seguridadBO = this.CrearObjetoSeguridad();
            this.vista.PagosCargados = this.ObtenerArchivoComoDataSet();
            var eventos = this.cargaMasivaContratosBR.ActualizarCompleto(this.dctx, this.vista.PagosCargados, this.vista.Departamento.GetValueOrDefault(), seguridadBO);

            this.RegistrarEvento(eventos.ToArray());
            this.vista.RedirigirAResultadoCarga();
        }

        /// <summary>
        /// Prepara un BO para la Busqueda en su respectivo catalogo
        /// </summary>
        /// <param name="catalogo">catalogo donde se realizara la busqueda</param>
        /// <returns>Objeto que define el filtro a aplicar al buscador</returns>
        public object PrepararBOBuscador(string catalogo)
        {
            object obj = null;

            switch (catalogo)
            {
                case "Sucursal":
                    SucursalBOF sucursal = new SucursalBOF();
                    sucursal.UnidadOperativa = new UnidadOperativaBO { Id = this.vista.UnidadOperativaID };
                    sucursal.Usuario = new UsuarioBO { Id = this.vista.UsuarioID };

                    sucursal.Nombre = this.vista.SucursalNombre;
                    sucursal.Activo = true;

                    obj = sucursal;
                    break;               
            }

            return obj;
        }

        /// <summary>
        /// Despliega el Resultado del Buscador
        /// </summary>
        /// <param name="catalogo">Catalogo en el que se realizo la busqueda</param>
        /// <param name="selecto">Objeto Resultante</param>
        public void DesplegarResultadoBuscador(string catalogo, object selecto)
        {
            switch (catalogo)
            {
                case "Sucursal":
                    SucursalBO sucursal = (SucursalBO)selecto;
                    if (sucursal != null)
                    {
                        if (sucursal.Id != null)                      
                            this.vista.SucursalID = sucursal.Id;                       
                        else                                          
                            this.vista.SucursalID = null;
                   
                        if (sucursal.Nombre != null)
                            this.vista.SucursalNombre = sucursal.Nombre;
                        else
                            this.vista.SucursalNombre = null;                       
                    }
                    break;               
            }
        }

        /// <summary>
        /// Realiza la primera carga y configuración
        /// </summary>
        public void RealizarPrimeraCarga()
        {
            this.vista.LimpiarSesion();
        }
        
        /// <summary>
        /// Crea un objeto de seguridad con los datos actuales de la sesión de usuario en curso
        /// </summary>
        /// <returns>Objeto de tipo seguridad</returns>
        private SeguridadBO CrearObjetoSeguridad()
        {
            UsuarioBO usuario = new UsuarioBO() { Id = this.vista.UsuarioID };
            AdscripcionBO adscripcion = new AdscripcionBO() { UnidadOperativa = new UnidadOperativaBO() { Id = this.vista.UnidadOperativaID }, Sucursal = new SucursalBO { Id = this.vista.SucursalID } };
            SeguridadBO seguridadBO = new SeguridadBO(Guid.Empty, usuario, adscripcion);

            return seguridadBO;
        }

        /// <summary>
        /// Valida el acceso a la sección en curso
        /// </summary>
        public void ValidarAcceso()
        {
            try
            {
                //Valida que usuario y la unidad operativa no sean nulos
                if (this.vista.UsuarioID == null) throw new Exception("El identificador del usuario no debe ser nulo");
                if (this.vista.UnidadOperativaID == null) throw new Exception("El identificador de la Unidad Operativa no debe ser nulo");
                //Se crea el objeto de seguridad                
                SeguridadBO seguridadBO = this.CrearObjetoSeguridad();

                //Se valida si el usuario tiene permiso a la acción principal
                if (!FacadeBR.ExisteAccion(this.dctx, "CONSULTAR", seguridadBO) ||                
                    !FacadeBR.ExisteAccion(this.dctx, "ACTUALIZARCOMPLETO", seguridadBO))
                    this.vista.RedirigirSinPermisoAcceso();
            }
            catch (Exception ex)
            {
                throw new Exception(nombreClase + ".ValidarAcceso: " + ex.GetBaseException().Message);
            }
        }

        /// <summary>
        /// Registra una relación de eventos a la lista de control en curso
        /// </summary>
        /// <param name="eventos">Relación de eventos a registrar</param>
        public void RegistrarEvento(params EventoCargaContratoBO[] eventos)
        {
            if (this.vista.Eventos == null)
                this.vista.Eventos = new List<EventoCargaContratoBO>();
            
            foreach (EventoCargaContratoBO evento in eventos)
                this.vista.Eventos.Add(evento);            
        }

        /// <summary>
        /// Obtiene el archivo subido como un Dataset
        /// </summary>
        /// <returns>Objeto de tipo DataSet</returns>
        private DataSet ObtenerArchivoComoDataSet()
        {
            String extensionFile = Path.GetExtension(this.vista.NombreArchivo);
            String tempFile = Path.Combine(Path.GetTempPath(), this.GetType().GUID.ToString());
            if (!Directory.Exists(tempFile))
                Directory.CreateDirectory(tempFile);

            tempFile = Path.Combine(tempFile, Guid.NewGuid() + extensionFile);
            File.WriteAllBytes(tempFile, this.vista.Archivo);

            String connectionString = null;

            if (String.Equals(extensionFile, ".xlsx", StringComparison.InvariantCultureIgnoreCase))
                connectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties=""Excel 12.0;HDR=YES;IMEX=1""";
            else
                if (String.Equals(extensionFile, ".xls", StringComparison.InvariantCultureIgnoreCase))
                    connectionString = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source={0}; Extended Properties=""Excel 8.0;HDR=YES;IMEX=1""";
                else
                    throw new Exception("El archivo proporcionado no tiene el formato correcto");

            connectionString = String.Format(connectionString, tempFile);
            String[] sheets = null;
            DataSet result = new DataSet();

            using (OleDbConnection oledbConnection = new OleDbConnection(connectionString))
            {
                oledbConnection.Open();

                DataTable excelSet = oledbConnection.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                sheets = (from
                            r in excelSet.AsEnumerable()
                          let
                             tableName = new Func<String>(() =>
                             {
                                 String value = r.Field<String>("TABLE_NAME");
                                 if (value.StartsWith("'") && value.EndsWith("'"))
                                     value = value.Substring(1, value.Length - 2);

                                 return value;
                             }).Invoke()
                          where
                             tableName.EndsWith("$")
                          select
                             tableName).ToArray();

                if (sheets.Length == 0)
                    throw new Exception("El archivo a cargar no contiene ninguna hoja con datos para cargar");

                foreach (String sheet in sheets)
                {
                    using (OleDbCommand command = new OleDbCommand())
                    {
                        command.Connection = oledbConnection;
                        command.CommandText = String.Format("SELECT * FROM [{0}]", sheet);

                        OleDbDataAdapter adapter = new OleDbDataAdapter();
                        adapter.SelectCommand = command;

                        DataTable table = new DataTable();

                        String tableName = sheet;
                        if (tableName.EndsWith("$"))
                            tableName = tableName.Substring(0, tableName.Length - 1).Trim() + "$";

                        table.TableName = tableName.ToUpper();
                        result.Tables.Add(table);

                        adapter.Fill(table);

                        //Fix ColumnNames
                        foreach (DataColumn column in table.Columns)
                            column.ColumnName = column.ColumnName.ToUpper()
                                                    .Replace("NO#", "NO.")                                                   
                                                    .Replace("KM#", "KM.")                                                    
                                                    .Replace("HR#", "HR.")                                                    
                                                    .Trim();
                    }
                }
            }

            return result;
        }
        #endregion
    }
}
