// Satisface al caso de uso CU020 - Reporte de Contratos FSL Registrados
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using BPMO.Basicos.BO;
using BPMO.Facade.SDNI.BR;
using BPMO.Patterns.Creational.DataContext;
using BPMO.Primitivos.Enumeradores;
using BPMO.SDNI.Contratos.FSL.Reportes.BR;
using BPMO.SDNI.Contratos.FSL.Reportes.VIS;

namespace BPMO.SDNI.Contratos.FSL.Reportes.PRE
{
    /// <summary>
    /// Presentador aplicable para la gestión de las peticiones de una vista de reporte de contratos de FSL
    /// </summary>
    public class ContratosFSLRegistradosPRE
    {
        #region Constantes

        /// <summary>
        /// Nombre de la clase para agregar a los mensajes de Error
        /// </summary>
        private static readonly string nombreClase = typeof(ContratosFSLRegistradosPRE).Name;

        #endregion

        #region Atributos

        /// <summary>
        /// El DataContext que proveerá acceso a la base de datos
        /// </summary>
        private readonly IDataContext dctx;

        /// <summary>
        /// Vista que se esta gestionando
        /// </summary>
        private readonly IContratosFSLRegistradosVIS vista;

        /// <summary>
        /// Controlador de Consulta de configuración de alerta
        /// </summary>
        private readonly ContratosFSLRegistradosBR controlador;

        #endregion  

        #region Constructor
        /// <summary>
        /// Constructor predeterminado para el presentador
        /// </summary>
        /// <param name="_vista">Vista de Datos</param>
        public ContratosFSLRegistradosPRE(IContratosFSLRegistradosVIS _vista)
        {
            if(_vista == null) throw new ArgumentNullException("_vista");
            vista = _vista;
            dctx = FacadeBR.ObtenerConexion();
            controlador = new ContratosFSLRegistradosBR();
        }
        #endregion

        #region Metodos

        /// <summary>
        /// Crea un objeto de seguridad con los datos actuales de la sesión de usuario en curso
        /// </summary>
        /// <returns>Objeto de tipo seguridad</returns>
        private SeguridadBO CrearObjetoSeguridad(bool incluirSucursal = true)
        {
            var usuario = new UsuarioBO { Id = vista.UsuarioID };
            var adscripcion = new AdscripcionBO { UnidadOperativa = new UnidadOperativaBO { Id = vista.UnidadOperativaID }, Sucursal = incluirSucursal? new SucursalBO { Id = vista.SucursalID }: null };
            var seguridadBO = new SeguridadBO(Guid.Empty, usuario, adscripcion);

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
                if (vista.UsuarioID == null) throw new Exception("El identificador del usuario no debe ser nulo");
                if (vista.UnidadOperativaID == null) throw new Exception("El identificador de la Unidad Operativa no debe ser nulo");
                //Se crea el objeto de seguridad                
                SeguridadBO seguridadBO = CrearObjetoSeguridad();

                //Se valida si el usuario tiene permiso a la acción principal
                if (!FacadeBR.ExisteAccion(dctx, "CONSULTAR", seguridadBO))
                    vista.RedirigirSinPermisoAcceso();
            }
            catch (Exception ex)
            {
                throw new Exception(nombreClase + ".ValidarAcceso: " + ex.GetBaseException().Message);
            }
        }

        /// <summary>
        /// Método abstract que se ejecuta cuando 
        /// </summary>
        public void Consultar()
        {
            IDictionary parametros = InterfazUsuarioADatos();
            Dictionary<String, Object> parametrosReporte =controlador.ConsultarContratos(dctx, parametros);



            if (((DataSet)parametrosReporte["DataSource"]).Tables["ContratoFSLRegistrado"].Rows.Count <= 0)
            {
                vista.MostrarMensaje("No se encontraron coincidencias con los filtros especificados", ETipoMensajeIU.ADVERTENCIA);
                return;
            }

            vista.EstablecerPaqueteNavegacionImprimir(vista.CodigoReporte, parametrosReporte);
            vista.IrAImprimir();
        }

        /// <summary>
        /// Pasa los datos de la Vista al Diccionario de Parámetros
        /// </summary>
        /// <returns></returns>
        private IDictionary InterfazUsuarioADatos()
        {
            var datos = new Hashtable();


            datos["UnidadOperativaID"] = vista.UnidadOperativaID;
            datos["ModuloID"] = vista.ModuloID;

            if (vista.ModeloID != null)
                datos["ModeloID"] = vista.ModeloID ;

            var sucursales = new List<int?>();
            if (vista.SucursalID != null)
            {
                sucursales.Add(vista.SucursalID.Value);
            }
            else
            {
                var sucursalesUsuario = FacadeBR.ConsultarSucursalesSeguridad(dctx, CrearObjetoSeguridad(false));
                if (sucursalesUsuario.Any())
                {
                    sucursales.AddRange(sucursalesUsuario.ConvertAll(x => x.Id));
                }                
            }

            datos["Sucursales"] = sucursales;
            
            if (vista.CuentaClienteID != null)
                datos["CuentaClienteID"] = vista.CuentaClienteID;

            if (vista.Anio != null)
                datos["Anio"] = vista.Anio;

            if (vista.FechaInicioContratoInicial != null)
                datos["FechaInicioContratoInicial"] = vista.FechaInicioContratoInicial;

            if (vista.FechaInicioContratoFinal != null)
                datos["FechaInicioContratoFinal"] = vista.FechaInicioContratoFinal;

            if (vista.FechaFinContratoInicial != null)
                datos["FechaFinContratoInicial"] = vista.FechaFinContratoInicial;

            if (vista.FechaFinContratoFinal != null)
                datos["FechaFinContratoFinal"] = vista.FechaFinContratoFinal;

            //Solo se traerán las unidades activas en FS
            datos["ActivoLinea"] = true;

            return datos;
        }

        #endregion
    }
}
