//Satisface el CU018 – Reporte Detallado de Renta Diaria por Sucursal

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BPMO.Patterns.Creational.DataContext;
using BPMO.SDNI.Contratos.RD.Reportes.VIS;
using BPMO.Facade.SDNI.BR;
using BPMO.Primitivos.Enumeradores;
using System.Collections;
using BPMO.Facade.SDNI.BOF;
using BPMO.Basicos.BO;
using BPMO.SDNI.Contratos.RD.Reportes.BR;
using System.Data;

namespace BPMO.SDNI.Contratos.RD.Reportes.PRE
{
    /// <summary>
    /// Presentador aplicable para la gestión de las peticiones de una vista de Reporte Contratos de Servicio Dedicado Registrados
    /// </summary>
    public class DetalladoRDSucursalPRE
    {
        #region Constantes
        /// <summary>
        /// Nombre de la clase para agregar a los mensajes de Error
        /// </summary>
        private static readonly string nombreClase = typeof(DetalladoRDSucursalPRE).Name;
        #endregion

        #region Atributos
        /// <summary>
        /// El DataContext que proveerá acceso a la base de datos
        /// </summary>
        private IDataContext dctx;

        /// <summary>
        /// Vista que se esta gestionando
        /// </summary>
        private IDetalladoRDSucursalVIS vista;

        /// <summary>
        /// Controlador de Consulta de Contratos de Mantenimiento y Servicio Dedicado
        /// </summary>
        private DetalladoRDSucursalBR controlador;
        #endregion       

        #region Contructor
        /// <summary>
        /// Constructor por default del presentador
        /// </summary>
        /// <param name="vista">Vista que será gestionada por el presentador</param>        
        public DetalladoRDSucursalPRE(IDetalladoRDSucursalVIS vista)
        {
            try
            {               
                this.vista = vista;
                this.dctx = FacadeBR.ObtenerConexion();
                this.controlador = new DetalladoRDSucursalBR();
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
            parameters["UnidadOperativaID"] = this.vista.UnidadOperativaID;
            parameters["ModuloID"] = this.vista.ModuloID;

            if (this.vista.ModeloID.HasValue)
                parameters["ModeloID"] = this.vista.ModeloID.Value;
           
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

            if (!String.IsNullOrEmpty(this.vista.SucursalNombre))
                parameters["NombreSucursal"] = this.vista.SucursalNombre;

            if (this.vista.Anio.HasValue)
                parameters["Anio"] = this.vista.Anio;

            if (this.vista.Mes.HasValue)
                parameters["Mes"] = this.vista.Mes;

            Dictionary<String, Object> reportParameters = this.controlador.ConsultarContratos(this.dctx, parameters);
            if (((DataSet)reportParameters["DataSource"]).Tables["ConsultarDetalladoRDSucursal"].Rows.Count <= 0)
            {
                this.vista.MostrarMensaje("No se encontraron coincidencias con los filtros especificados", ETipoMensajeIU.ADVERTENCIA);
                return;
            }

            this.vista.EstablecerPaqueteNavegacionImprimir("BEP1401.CU018", reportParameters);
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
