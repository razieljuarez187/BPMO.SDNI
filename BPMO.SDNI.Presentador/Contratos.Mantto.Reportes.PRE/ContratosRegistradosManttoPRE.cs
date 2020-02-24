//Satisface el CU022 – Reporte Contratos de Servicio Dedicado Registrados
//Satisface al caso de uso CU021 - Reporte de Contratos de Mantenimiento Registrados
using System;
using System.Collections.Generic;
using System.Linq;
using BPMO.Patterns.Creational.DataContext;
using BPMO.SDNI.Contratos.Mantto.Reportes.VIS;
using BPMO.Primitivos.Enumeradores;
using BPMO.Facade.SDNI.BR;
using System.Collections;
using BPMO.Facade.SDNI.BOF;
using BPMO.Basicos.BO;
using BPMO.SDNI.Contratos.Mantto.Reportes.BR;
using System.Data;

namespace BPMO.SDNI.Contratos.Mantto.Reportes.PRE
{
    /// <summary>
    /// Presentador aplicable para la gestión de las peticiones de una vista de reporte de contratos de mantenimiento
    /// </summary>  
    public class ContratosRegistradosManttoPRE     
    {
        #region Constantes
        /// <summary>
        /// Nombre de la clase para agregar a los mensajes de Error
        /// </summary>
        private static readonly string nombreClase = typeof(ContratosRegistradosManttoPRE).Name;
        #endregion

        #region Atributos
        /// <summary>
        /// El DataContext que proveerá acceso a la base de datos
        /// </summary>
        private IDataContext dctx;

        /// <summary>
        /// Vista que se esta gestionando
        /// </summary>
        private IContratosRegistradosManttoVIS vista;

        /// <summary>
        /// Controlador de Consulta de Contratos de Mantenimiento y Servicio Dedicado
        /// </summary>
        private ContratosRegistradosManttoBR controlador;
        #endregion       

        #region Propiedades
        #endregion

        #region Contructor
        /// <summary>
        /// Constructor por default del presentador
        /// </summary>
        /// <param name="vista">Vista que será gestionada por el presentador</param>        
        public ContratosRegistradosManttoPRE(IContratosRegistradosManttoVIS vista)
        {
            try
            {               
                this.vista = vista;
                this.dctx = FacadeBR.ObtenerConexion();
                this.controlador = new ContratosRegistradosManttoBR();
            }
            catch (Exception ex)
            {
                this.vista.MostrarMensaje("Inconsistencias en la configuracion", ETipoMensajeIU.ERROR, nombreClase + ex.Message);
            }
        }
        #endregion

        #region Métodos
        /// <summary>
        /// Método abstract que se ejecuta cuando 
        /// </summary>
        public void Consultar()
        {            
            Hashtable parameters = new Hashtable();
            parameters["TipoContrato"] = this.vista.TipoContrato;
            parameters["UnidadOperativaID"] = this.vista.UnidadOperativaID;
            parameters["ModuloID"] = this.vista.ModuloID;

            if (this.vista.ModeloID.HasValue)
                parameters["ModeloID"] = this.vista.ModeloID.Value;

            if (this.vista.CuentaClienteID.HasValue)
                parameters["CuentaClienteID"] = this.vista.CuentaClienteID.Value;

            if (this.vista.SucursalID.HasValue)
                parameters["SucursalID"] = new Int32[] { this.vista.SucursalID.Value };
            else
            {
                SucursalBOF sucursal = new SucursalBOF();
                sucursal.UnidadOperativa = new UnidadOperativaBO { Id = this.vista.UnidadOperativaID };
                sucursal.Usuario = new UsuarioBO { Id = this.vista.UsuarioID };
                sucursal.Activo = true;

                List<SucursalBOF> sucursalesPermitidas = FacadeBR.ConsultarSucursalesSeguridad(this.dctx, sucursal);
                if (sucursalesPermitidas.Count > 0)
                {
                    parameters["SucursalID"] = sucursalesPermitidas
                                                .Select(x => x.Id.Value)
                                                .ToArray();
                }                            
                else //Sino tiene sucursales asignadas al usuario en curso se manda una sucursal no existente
                    parameters["SucursalID"] = new Int32[] { -1000 };
            }

            if (this.vista.FechaInicioContrato1.HasValue )
                parameters["FechaInicioContratoInicial"] =this.vista.FechaInicioContrato1.Value;

            if(this.vista.FechaInicioContrato2.HasValue)
                parameters["FechaInicioContratoFinal"] = this.vista.FechaInicioContrato2.Value;

            if(this.vista.FechaFinContrato1.HasValue)
                parameters["FechaFinContratoInicial"] = this.vista.FechaFinContrato1.Value;
            
            if(this.vista.FechaFinContrato2.HasValue)
                parameters["FechaFinContratoFinal"] = this.vista.FechaFinContrato2;

            if (this.vista.Anio.HasValue)
                parameters["Anio"] = this.vista.Anio;
            Dictionary<String, Object> reportParameters = this.controlador.ConsultarContratos(this.dctx, parameters);
            if (((DataSet)reportParameters["DataSource"]).Tables["ConsultarContratosRegistrados"].Rows.Count <= 0)
            {
                this.vista.MostrarMensaje("No se encontraron coincidencias con los filtros especificados", ETipoMensajeIU.ADVERTENCIA);
                return;
            }
           
            this.vista.EstablecerPaqueteNavegacionImprimir(this.vista.IdentificadorReporte, reportParameters);
            this.vista.IrAImprimir();
        }

        /// <summary>
        /// Crea un objeto de seguridad con los datos actuales de la sesión de usuario en curso
        /// </summary>
        /// <returns>Objeto de tipo seguridad</returns>
        private SeguridadBO CrearObjetoSeguridad()
        {
            UsuarioBO usuario = new UsuarioBO() { Id = this.vista.UsuarioID };
            AdscripcionBO adscripcion = new AdscripcionBO() { UnidadOperativa = new UnidadOperativaBO() { Id = this.vista.UnidadOperativaID } };
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
                if (!FacadeBR.ExisteAccion(this.dctx, "CONSULTAR", seguridadBO))
                    this.vista.RedirigirSinPermisoAcceso();
            }
            catch (Exception ex)
            {
                throw new Exception(nombreClase + ".ValidarAcceso: " + ex.GetBaseException().Message);
            }
        }
        #endregion
    }    
}
