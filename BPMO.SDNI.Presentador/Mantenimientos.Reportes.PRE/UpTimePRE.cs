//Satisface el caso de uso PLEN.BEP.15.MODMTTO.CU069.Reporte.Up.Time
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BPMO.Patterns.Creational.DataContext;
using BPMO.SDNI.Mantenimiento.Reportes.VIS;
using BPMO.Facade.SDNI.BR;
using BPMO.Primitivos.Enumeradores;
using System.Collections;
using BPMO.Facade.SDNI.BOF;
using BPMO.Basicos.BO;
using System.Globalization;
using BPMO.SDNI.Mantenimiento.Reportes.BR;
using System.Data;
using BPMO.Facade.SDNI.BO;
using BPMO.Servicio.Catalogos.BO;
using BPMO.SDNI.Comun.BO;
using BPMO.SDNI.Comun.BR;

namespace BPMO.SDNI.Mantenimientos.Reportes.PRE
{
    public class UpTimePRE
    {
        #region Constantes
        /// <summary>
        /// Nombre de la clase para agregar a los mensajes de Error
        /// </summary>
        private static readonly string NombreClase = typeof(UpTimePRE).Name;
        #endregion 

        #region Atributos
        /// <summary>
        /// El DataContext que proveerá acceso a la base de datos
        /// </summary>
        private IDataContext dctx;

        /// <summary>
        /// Vista que se esta gestionando
        /// </summary>
        private IUpTimeVIS vista;

        /// <summary>
        /// Controlador del reporte de UpTime
        /// </summary>
        private UpTimeBR controlador;
        #endregion

        #region Constructor

        /// <summary>
        /// Constructor por default del presentador
        /// </summary>
        /// <param name="vista">Vista que será gestionada por el presentador</param>
        public UpTimePRE(IUpTimeVIS vista)
        {
            try
            {
                this.vista = vista;
                this.dctx = FacadeBR.ObtenerConexion();
                this.controlador = new UpTimeBR();
            }
            catch (Exception ex)
            {
                this.vista.MostrarMensaje("Inconsistencias en la configuracion", ETipoMensajeIU.ERROR, NombreClase + ex.Message);
            }
        }

        #endregion

        #region Métodos

        /// <summary>
        /// Establece la información inicial para el buscador general
        /// </summary>
        private void EstablecerInformacionInicial()
        {
            try
            {
                #region Configuraciones de Unidad Operativa
                //Obtener las configuraciones de la unidad operativa
                List<ConfiguracionUnidadOperativaBO> lstConfigUO = new ModuloBR().ConsultarConfiguracionUnidadOperativa(this.dctx, new ConfiguracionUnidadOperativaBO() { UnidadOperativa = new UnidadOperativaBO() { Id = this.vista.UnidadOperativaID } }, this.vista.ModuloID);
                if (lstConfigUO.Count <= 0)
                    throw new Exception("No se encontraron las configuraciones del sistema para la unidad operativa en la que trabaja.");

                //Establecer las configuraciones de la unidad operativa
                this.vista.LibroActivos = lstConfigUO[0].Libro;
                #endregion
            }
            catch (Exception ex)
            {
                throw new Exception(NombreClase + ".EstablecerInformacionInicial: " + ex.Message);
            }
        }

        /// <summary>
        /// Prepara el buscador general
        /// </summary>
        public void PrepararBusqueda()
        {
            this.vista.PrepararBusqueda();
            this.EstablecerInformacionInicial();
        }

        /// <summary>
        /// Consulta los Registros para el Reporte de UpTime
        /// </summary>
        public void Consultar() {
            int _y = this.vista.Anio.Value;
            int _m = this.vista.Mes.Value;

            Hashtable parameters = new Hashtable();
            parameters["UnidadOperativaID"] = this.vista.UnidadOperativaID;
            parameters["ModuloID"] = this.vista.ModuloID;
            parameters["FechaInicio"] = new DateTime(_y, _m, 1);
            parameters["FechaFin"] = new DateTime(_y, _m, DateTime.DaysInMonth(_y, _m), 23, 59, 59);

            if (vista.Area != null)
                parameters["Area"] = vista.Area;
            if (vista.ClienteID != null)
                parameters["ClienteID"] = this.vista.ClienteID;
            if (vista.VIN != null)
                parameters["VIN"] = this.vista.VIN;
            if (this.vista.SucursalID.HasValue)
                parameters["SucursalID"] = new Int32[] { this.vista.SucursalID.Value };
            else {
                SucursalBOF sucursal = new SucursalBOF();
                sucursal.UnidadOperativa = new UnidadOperativaBO { Id = this.vista.UnidadOperativaID };
                sucursal.Usuario = new UsuarioBO { Id = this.vista.UsuarioID };
                sucursal.Activo = true;

                List<SucursalBOF> sucursalesPermitidas = FacadeBR.ConsultarSucursalesSeguridad(this.dctx, sucursal);
                if (sucursalesPermitidas.Count > 0) {
                    parameters["SucursalID"] = sucursalesPermitidas
                                                .Select(x => x.Id.Value)
                                                .ToArray();
                } else //Sino tiene sucursales asignadas al usuario en curso se manda una sucursal no existente
                    parameters["SucursalID"] = new Int32[] { -1000 };
            }

            Dictionary<String, Object> reportParameters = this.controlador.GenerarReporte(this.dctx, parameters);
            if (((DataSet)reportParameters["DataSource"]).Tables["OrdenesServicio"].Rows.Count <= 0) {
                this.vista.MostrarMensaje("No se encontraron coincidencias con los filtros especificados", ETipoMensajeIU.ADVERTENCIA);
                return;
            }

            this.vista.EstablecerPaqueteNavegacionImprimir("PLEN.BEP.15.MODMTTO.CU069", reportParameters);
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
                throw new Exception(NombreClase + ".ValidarAcceso: " + ex.GetBaseException().Message);
            }
        }

        #region Métodos para el Buscador
        /// <summary>
        /// Prepara e bucador general
        /// </summary>
        /// <param name="catalogo"></param>
        /// <returns></returns>
        public object PrepararBOBuscador(string catalogo)
        {
            object obj = null;

            switch (catalogo)
            {
                case "Unidad":
                    EquipoBepensaBO ebBO = new EquipoBepensaBO();
                    ebBO.ActivoFijo = new ActivoFijoBO();
                    ebBO.ActivoFijo.Auditoria = new AuditoriaBO();
                    ebBO.Unidad = new Servicio.Catalogos.BO.UnidadBO();
                    ebBO.Unidad.Auditoria = new AuditoriaBO();
                    ebBO.Unidad.ClasificadorAplicacion = new ClasificadorAplicacionBO();
                    ebBO.Unidad.ClasificadorAplicacion.Auditoria = new AuditoriaBO();
                    ebBO.Unidad.Cliente = new ClienteBO();
                    ebBO.Unidad.Cliente.Auditoria = new AuditoriaBO();
                    ebBO.Unidad.ConfiguracionModeloMotorizacion = new ConfiguracionModeloMotorizacionBO();
                    ebBO.Unidad.ConfiguracionModeloMotorizacion.Auditoria = new AuditoriaBO();
                    ebBO.Unidad.ConfiguracionModeloMotorizacion.ClasificadorMotorizacion = new ClasificadorMotorizacionBO();
                    ebBO.Unidad.ConfiguracionModeloMotorizacion.ClasificadorMotorizacion.Auditoria = new AuditoriaBO();
                    ebBO.Unidad.ConfiguracionModeloMotorizacion.Modelo = new ModeloBO();
                    ebBO.Unidad.ConfiguracionModeloMotorizacion.Modelo.Auditoria = new AuditoriaBO();
                    ebBO.Unidad.ConfiguracionModeloMotorizacion.Modelo.Marca = new MarcaBO();
                    ebBO.Unidad.ConfiguracionModeloMotorizacion.Modelo.Marca.Auditoria = new AuditoriaBO();
                    ebBO.Unidad.Distribuidor = new DistribuidorBO();
                    ebBO.Unidad.Distribuidor.Auditoria = new AuditoriaBO();
                    ebBO.Unidad.TipoUnidad = new TipoUnidadBO();
                    ebBO.Unidad.TipoUnidad.Auditoria = new AuditoriaBO();

                    ebBO.Unidad.NumeroSerie = this.vista.VIN;
                    ebBO.Unidad.Activo = true;
                    ebBO.ActivoFijo.NumeroSerie = this.vista.VIN;
                    ebBO.ActivoFijo.Libro = this.vista.LibroActivos;
                    obj = ebBO;
                    break;

                case "Sucursal":
                    SucursalBOF sucursal = new SucursalBOF();
                    sucursal.UnidadOperativa = new UnidadOperativaBO();
                    sucursal.UnidadOperativa.Id = this.vista.UnidadOperativaID;
                    sucursal.Nombre = this.vista.SucursalNombre;
                    sucursal.Usuario = new UsuarioBO() { Id = this.vista.UsuarioID };
                    obj = sucursal;
                    break;

                case "Cliente":
                    ClienteBO cliente = new ClienteBO();
                    cliente.Nombre = this.vista.ClienteNombre;
                    obj = cliente;
                    break;
            }

            return obj;
        }

        /// <summary>
        /// Devuelve el resultado de la búsqueda
        /// </summary>
        /// <param name="catalogo"></param>
        /// <param name="selecto"></param>
        public void DesplegarResultadoBuscador(string catalogo, object selecto)
        {
            switch (catalogo)
            {
                case "Unidad":
                    EquipoBepensaBO ebBO = (EquipoBepensaBO)selecto;
                    if (ebBO == null) ebBO = new EquipoBepensaBO();

                    if (ebBO.NumeroSerie != null)
                        this.vista.VIN = ebBO.NumeroSerie;
                    else
                        this.vista.VIN = null;
                    break;

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

                case "Cliente":
                    ClienteBO cliente = (ClienteBO)selecto;

                    if (cliente != null && cliente.Id != null)
                        this.vista.ClienteID = cliente.Id;
                    else
                        this.vista.ClienteID = null;

                    if (cliente != null && cliente.Nombre != null)
                        this.vista.ClienteNombre = cliente.Nombre;
                    else
                        this.vista.ClienteNombre = null;
                    break;
            }
        }
        #endregion

        #endregion
    }
}
