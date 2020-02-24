//Satisface al CU075 - Catálogo de Equipo Aliado
//Satisface a la SC0005
using System;
using System.Collections.Generic;
using BPMO.Basicos.BO;
using BPMO.Facade.SDNI.BO;
using BPMO.Facade.SDNI.BOF;
using BPMO.Facade.SDNI.BR;
using BPMO.Patterns.Creational.DataContext;
using BPMO.Primitivos.Enumeradores;
using BPMO.SDNI.Comun.BO;
using BPMO.SDNI.Comun.BR;
using BPMO.SDNI.Equipos.BO;
using BPMO.SDNI.Equipos.BOF;
using BPMO.SDNI.Equipos.BR;
using BPMO.SDNI.Equipos.VIS;
using BPMO.Servicio.Catalogos.BO;

namespace BPMO.SDNI.Equipos.PRE
{
    public class ConsultarEquipoAliadoPRE
    {
        #region Atributos
        private IConsultarEquipoAliadoVIS vista;
        private IDataContext dctx = null;
        private EquipoAliadoBR controlador;

        private string nombreClase = "ConsultarEquipoAliadoPRE";
        #endregion

        #region Constructores
        public ConsultarEquipoAliadoPRE(IConsultarEquipoAliadoVIS view)
        {
            try
            {
                this.vista = view;

                this.controlador = new EquipoAliadoBR();
                this.dctx = FacadeBR.ObtenerConexion();
            }
            catch (Exception ex)
            {
                this.vista.MostrarMensaje("No se pudieron obtener los datos de conexión", ETipoMensajeIU.ERROR,
                        "No se encontraron los parámetros de conexión en la fuente de datos, póngase en contacto con el administrador del sistema." + ex.Message);
            }
        }
        #endregion

        #region Metodos
        public void PrepararBusqueda()
        {
            this.vista.CargarEstatusEquipos();
            this.vista.CargarTiposEquipoAliado(); //SC0005
            this.vista.LimpiarSesion();
            this.vista.PrepararBusqueda();
            this.EstablecerInformacionInicial();
			this.EstablecerSeguridad(); //SC_0008
        }
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
                throw new Exception(this.nombreClase + ".EstablecerInformacionInicial: " + ex.Message);
            }
        }

		#region SC_0008
		private void EstablecerSeguridad()
		{
            try
            {
                //se valida que los datos del usuario y la unidad operativa no sean nulos
                if (this.vista.UsuarioAutenticado == null) throw new Exception("El identificador del usuario no debe ser nulo");
                if (this.vista.UnidadOperativaID == null) throw new Exception("El identificador de la Unidad Operativa no debe ser nulo ");

                //Se crea el objeto de seguridad
                UsuarioBO usuario = new UsuarioBO() { Id = this.vista.UsuarioAutenticado };
                AdscripcionBO adscripcion = new AdscripcionBO() { UnidadOperativa = new UnidadOperativaBO() { Id = this.vista.UnidadOperativaID } };
                SeguridadBO seguridadBO = new SeguridadBO(Guid.Empty, usuario, adscripcion);

                //Se obtienen las acciones a las cuales el usuario tiene permiso en este proceso
                List<CatalogoBaseBO> lst = FacadeBR.ConsultarAccion(this.dctx, seguridadBO);

                //Se valida si el usuario tiene permiso para consultar
                if (!this.ExisteAccion(lst, "CONSULTAR"))
                    this.vista.RedirigirSinPermisoAcceso();
                //Se valida si el usuario tiene permiso para registrar una llanta
                if (!this.ExisteAccion(lst, "INSERTARCOMPLETO"))
                    this.vista.PermitirRegistrar(false);
            }
            catch (Exception ex)
            {
                throw new Exception(this.nombreClase + ".EstablecerSeguridad:" + ex.Message);
            }
		}

		private bool ExisteAccion(List<CatalogoBaseBO> acciones, string nombreAccion)
		{
			if (acciones != null && acciones.Count > 0 && acciones.Exists(p => p != null && p.Nombre != null && p.Nombre.Trim().ToUpper() == nombreAccion.Trim().ToUpper()))
				return true;

			return false;
		}
		#endregion

        public void Consultar()
        {
            try 
            {
                EquipoAliadoBOF bo = (EquipoAliadoBOF)this.InterfazUsuarioADato();

                List<EquipoAliadoBO> equipos = controlador.Consultar(dctx, bo).ConvertAll(s => (EquipoAliadoBO)s);

                this.vista.Equipos = equipos;
                this.vista.ActualizarResultado();

                if (equipos.Count < 1)
                    this.vista.MostrarMensaje("La búsqueda no produjo resultados", ETipoMensajeIU.INFORMACION,
                        "No se encontraron coincidencias con los criterios ingresados. Verifique sus datos");
            }
            catch (Exception ex) 
            {
                throw new Exception(this.nombreClase + ".Consultar: " + ex.Message); 
            }
        }

        private EquipoAliadoBOF InterfazUsuarioADato()
        {
            EquipoAliadoBOF bo = new EquipoAliadoBOF();
            bo.Sucursal = new Basicos.BO.SucursalBO();
            bo.Sucursal.UnidadOperativa = new UnidadOperativaBO();
            bo.Modelo = new ModeloBO();
            bo.Modelo.Marca = new MarcaBO();

            bo.EsActivo = this.vista.ActivoOracle;
            if(this.vista.Estatus.HasValue)
                if(this.vista.Estatus.Value >= 0)
                    bo.Estatus = (EEstatusEquipoAliado)this.vista.Estatus;
            bo.Modelo.Marca.Id = this.vista.MarcaID;
            bo.Modelo.Marca.Nombre = this.vista.Marca;
            bo.NumeroSerie = this.vista.NumeroSerie;
            bo.Sucursal.Id = this.vista.SucursalID;
            bo.Sucursal.UnidadOperativa.Id = this.vista.UnidadOperativaID;
            #region SC0005
            if (vista.TipoEquipoAliado.HasValue)
                bo.TipoEquipoAliado = (ETipoEquipoAliado) vista.TipoEquipoAliado;
            #endregion

            if (vista.SucursalID != null)
            {
                bo.Sucursal.Id = this.vista.SucursalID;
                bo.Sucursal.Nombre = this.vista.SucursalNombre;
            }
            else
                bo.Sucursales = FacadeBR.ConsultarSucursalesSeguridad(this.dctx, new SeguridadBO(Guid.Empty, new UsuarioBO() { Id = this.vista.UsuarioAutenticado }, new AdscripcionBO { UnidadOperativa = new UnidadOperativaBO() { Id = this.vista.UnidadOperativaID } }));

            return bo;
        }

        public void CambiarPaginaResultado(int p)
        {
            this.vista.IndicePaginaResultado = p;
            this.vista.ActualizarResultado();
        }

        public void VerDetalles(int index)
        {
            if (index >= this.vista.Equipos.Count || index < 0)
                throw new Exception("No se encontró el acta de nacimiento seleccionado.");

            EquipoAliadoBO bo = this.vista.Equipos[index];

            this.vista.LimpiarSesion();
            this.vista.EstablecerPaqueteNavegacion("EquipoAliadoBODetalle", bo);

            this.vista.RedirigirADetalles();
        }

        #region Métodos para el Buscador
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

                    ebBO.Unidad.NumeroSerie = this.vista.NumeroSerie;
                    ebBO.Unidad.Activo = true;
                    ebBO.ActivoFijo.NumeroSerie = this.vista.NumeroSerie;
                    ebBO.ActivoFijo.Libro = this.vista.LibroActivos;
                    obj = ebBO;
                    break;

                case "Sucursal":
                    SucursalBOF sucursal = new SucursalBOF();
                    sucursal.UnidadOperativa = new UnidadOperativaBO();
                    sucursal.UnidadOperativa.Id = this.vista.UnidadOperativaID;
                    sucursal.Nombre = this.vista.SucursalNombre;
                    sucursal.Usuario = new UsuarioBO() { Id = this.vista.UsuarioAutenticado };
                    obj = sucursal;
                    break;

                case "Marca":
                    MarcaBO marca = new MarcaBO();

                    marca.Nombre = this.vista.Marca;
                    marca.Activo = true;

                    obj = marca;
                    break;
            }

            return obj;
        }
        public void DesplegarResultadoBuscador(string catalogo, object selecto)
        {
            switch (catalogo)
            {
                case "Unidad":
                    EquipoBepensaBO ebBO = (EquipoBepensaBO)selecto;
                    if (ebBO == null) ebBO = new EquipoBepensaBO();

                    if (ebBO.NumeroSerie != null)
                        this.vista.NumeroSerie = ebBO.NumeroSerie;
                    else
                        this.vista.NumeroSerie = null;
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

                case "Marca":
                    MarcaBO marca = (MarcaBO)selecto;

                    if (marca != null && marca.Id != null)
                        this.vista.MarcaID = marca.Id;
                    else
                        this.vista.MarcaID = null;
                    if (marca != null && marca.Nombre != null)
                        this.vista.Marca = marca.Nombre;
                    else
                        this.vista.Marca = null;
                    break;
            }
        }
        #endregion
        #endregion
    }
}
