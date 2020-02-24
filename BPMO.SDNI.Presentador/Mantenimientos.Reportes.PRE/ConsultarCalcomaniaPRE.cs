//Satisface al caso de uso PLEN.BEP.15.MODMTTO.CU017.Imprimir.Calcomania.Mantenimiento
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BPMO.SDNI.Mantenimiento.Reportes.VIS;
using BPMO.Patterns.Creational.DataContext;
using BPMO.SDNI.Equipos.BR;
using BPMO.Facade.SDNI.BR;
using BPMO.Primitivos.Enumeradores;
using BPMO.Facade.SDNI.BO;
using BPMO.Basicos.BO;
using BPMO.Servicio.Catalogos.BO;
using BPMO.SDNI.Comun.BO;
using BPMO.SDNI.Comun.BR;
using BPMO.SDNI.Mantenimiento.Reportes.BR;
using System.Collections;
using BPMO.Facade.SDNI.BOF;
using System.Data;
using BPMO.SDNI.Equipos.BOF;

namespace BPMO.SDNI.Mantenimiento.Reportes.PRE
{
    public class ConsultarCalcomaniaPRE
    {

        #region Propiedades
        /// <summary>
        /// Vista que se está gestionando
        /// </summary>
        private IConsultarCalcomaniaVIS vista;
        /// <summary>
        /// El DataContext que proveerá acceso a la base de datos
        /// </summary>
        private IDataContext dataContext = null;
        /// <summary>
        /// Controlador de consulta de calcomanías
        /// </summary>
        private CalcomaniasMantenimientoBR manttoController;
        /// <summary>
        /// Nombre de la clase
        /// </summary>
        private string nombreClase = "ConsultarCalcomaniaPRE";
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor de la clase
        /// </summary>
        /// <param name="vista">Objecto que crea la instancia de la Interfaz IConsultarCalcomaniaVIS</param>
        public ConsultarCalcomaniaPRE(IConsultarCalcomaniaVIS vista)
        {
            try
            {
                this.vista = vista;
                this.dataContext = FacadeBR.ObtenerConexion();
                this.manttoController = new CalcomaniasMantenimientoBR();

            }
            catch (Exception)
            {
                this.vista.MostrarMensaje("No se pudieron obtener los datos de conexión", ETipoMensajeIU.ERROR,
                        "No se encontraron los parámetros de conexión en la fuente de datos, póngase en contacto con el administrador del sistema.");
            }
        }

        #endregion

        #region Métodos

        /// <summary>
        /// Prepara el buscador general
        /// </summary>
        public void PrepararBusqueda()
        {
            this.vista.PrepararBusqueda();
            this.EstablecerInformacionInicial();
        }

        /// <summary>
        /// Establece la información inicial para el buscador
        /// </summary>
        private void EstablecerInformacionInicial()
        {
            try
            {
                #region Configuraciones de Unidad Operativa
                //Obtener las configuraciones de la unidad operativa
                List<ConfiguracionUnidadOperativaBO> lstConfigUO = new ModuloBR().ConsultarConfiguracionUnidadOperativa(this.dataContext, new ConfiguracionUnidadOperativaBO() { UnidadOperativa = new UnidadOperativaBO() { Id = this.vista.UnidadOperativaID } }, this.vista.ModuloID);
                if (lstConfigUO.Count <= 0)
                    throw new Exception("No se encontraron las configuraciones del sistema para la unidad operativa en la que trabaja.");

                //Establecer las configuraciones de la unidad operativa
                this.vista.LibroActivos = lstConfigUO[0].Libro;
                #endregion
            }
            catch (Exception ex)
            {
                throw new Exception(this.nombreClase + ".EstablecerInformacionInicial: " + ex.Message);
            }
        }

        #region Métodos para el Buscador

        /// <summary>
        /// Método que se utiliza para traer los datos para el buscador
        /// </summary>
        /// <param name="catalogo">propedad de tipo string</param>
        /// <returns>Devuelve un tipo objecto</returns>
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

                    ebBO.Unidad.NumeroSerie = this.vista.NumeroVIN;
                    ebBO.Unidad.Activo = true;
                    ebBO.ActivoFijo.NumeroSerie = this.vista.NumeroVIN;
                    ebBO.ActivoFijo.Libro = this.vista.LibroActivos;
                    obj = ebBO;
                    break;

                case "UnidadIdealease":
                    UnidadBOF unidad = new UnidadBOF();

                    if (!string.IsNullOrEmpty(vista.NumeroEconomico))
                        unidad.NumeroEconomico = vista.NumeroEconomico;

                    obj = unidad;
                    break;
            }

            return obj;
        }

        /// <summary>
        /// Devuelve los resultados de la búsqueda
        /// </summary>
        /// <param name="catalogo"> Objeto de tipo string que utliza como parámetro el buscador</param>
        /// <param name="selecto">Objeto de tipo string que utliza como parámetro el buscador</param>
        public void DesplegarResultadoBuscador(string catalogo, object selecto)
        {
            switch (catalogo)
            {
                case "Unidad":
                    EquipoBepensaBO ebBO = (EquipoBepensaBO)selecto;
                    if (ebBO == null) ebBO = new EquipoBepensaBO();

                    if (ebBO.NumeroSerie != null)
                        this.vista.NumeroVIN = ebBO.NumeroSerie;
                    else
                        this.vista.NumeroVIN = null;
                    break;

                case "UnidadIdealease":
                    UnidadBOF unidad = (UnidadBOF)selecto ?? new UnidadBOF();
                    if (unidad.NumeroSerie != null)
                    {
                        vista.NumeroEconomico = unidad.NumeroEconomico;
                    }
                    else
                    {
                        vista.NumeroEconomico = string.Empty;
                    }
                    break;
                
            }
        }
        #endregion

        /// <summary>
        /// Método que se utiliza para cambiar los resultados del grid cuando cambia de página
        /// </summary>
        /// <param name="nuevoIndicePagina">propiedad de tipo int que se le envía al método</param>
        public void CambiarPaginaResultado(int nuevoIndicePagina)
        {
            this.vista.IndicePaginaResultado = nuevoIndicePagina;
            this.vista.ActualizarResultado();
        }

        /// <summary>
        /// Método que se utiliza para consultar los datos que se agregarán al grid
        /// </summary>
        public void Consultar()
        {
            Hashtable parameters = new Hashtable();
            parameters["UnidadOperativaID"] = this.vista.UnidadOperativaID;
            parameters["ModuloID"] = this.vista.ModuloID;

            if(this.vista.NumeroVIN != null)
                parameters["NumeroVin"] = this.vista.NumeroVIN;
            if (this.vista.NumeroEconomico != null)
                parameters["NoEconomico"] = this.vista.NumeroEconomico;
            if (this.vista.Departamento.HasValue)
                parameters["Departamento"] = (int)this.vista.Departamento;

            if (this.vista.ClienteID.HasValue)
                parameters["CuentaClienteID"] = this.vista.ClienteID.Value;

            if (this.vista.SucursalID.HasValue)
                parameters["SucursalID"] = new Int32[] { this.vista.SucursalID.Value };
            else
            {
                SucursalBOF sucursal = new SucursalBOF();
                sucursal.UnidadOperativa = new UnidadOperativaBO { Id = this.vista.UnidadOperativaID };
                sucursal.Usuario = new UsuarioBO { Id = this.vista.UsuarioID };
                sucursal.Activo = true;

                List<SucursalBOF> sucursalesPermitidas = FacadeBR.ConsultarSucursalesSeguridad(this.dataContext, sucursal);
                if (sucursalesPermitidas.Count > 0)
                {
                    parameters["SucursalID"] = sucursalesPermitidas
                                                .Select(x => x.Id.Value)
                                                .ToArray();
                }
                else //Sino tiene sucursales asignadas al usuario en curso se manda una sucursal no existente
                    parameters["SucursalID"] = new Int32[] { -1000 };
            }

            DataSet ds = this.manttoController.Consultar(this.dataContext, parameters);
            if (ds.Tables["ConsultarCalcomanias"].Rows.Count <= 0)
            {
                this.vista.MostrarMensaje("No se encontraron coincidencias con los filtros especificados", ETipoMensajeIU.ADVERTENCIA);
                this.vista.GvUnidadesCtes.DataSource = null;
                this.vista.GvUnidadesCtes.DataBind();

                return;
            }
            else
            {
                this.vista.Resultado = ds.Tables["ConsultarCalcomanias"].DataSet;
                this.vista.GvUnidadesCtes.DataSource = ds.Tables["ConsultarCalcomanias"];
                this.vista.GvUnidadesCtes.DataBind();
            }

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
                if (!FacadeBR.ExisteAccion(this.dataContext, "CONSULTAR", seguridadBO))
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
