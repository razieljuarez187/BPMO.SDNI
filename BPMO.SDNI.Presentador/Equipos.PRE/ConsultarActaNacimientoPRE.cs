//Satisface al CU079 - Consultar Acta de Nacimiento de Unidad
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
using BPMO.SDNI.Equipos.BOF;
using BPMO.SDNI.Equipos.BR;
using BPMO.SDNI.Equipos.VIS;
using BPMO.Servicio.Catalogos.BO;

namespace BPMO.SDNI.Equipos.PRE
{
    public class ConsultarActaNacimientoPRE
    {
        #region propiedades
        private IConsultarActaNacimientoVIS vista;
        private IDataContext dataContext = null;
        private UnidadBR controlador;

        private string nombreClase = "RegistrarActaNacimientoPRE";
        #endregion

        #region Constructor
        public ConsultarActaNacimientoPRE(IConsultarActaNacimientoVIS vista)
        {
            try
            {
                this.vista = vista;

                this.controlador = new UnidadBR();
                this.dataContext = FacadeBR.ObtenerConexion();
            }
            catch (Exception ex)
            {
                this.vista.MostrarMensaje("No se pudieron obtener los datos de conexión", ETipoMensajeIU.ERROR,
                        "No se encontraron los parámetros de conexión en la fuente de datos, póngase en contacto con el administrador del sistema.");
            }

        }
        #endregion

        #region Métodos
        public void PrepararBusqueda()
        {
            this.vista.LimpiarSesion();

            this.vista.PrepararBusqueda();
            this.EstablecerInformacionInicial();

            this.EstablecerSeguridad(); //SC_0008
            this.EstablecerAcciones();
        }
        private void EstablecerInformacionInicial()
        {
            try
            {
                #region Configuraciones de Unidad Operativa
                //Obtener las configuraciones de la unidad operativa
                List<ConfiguracionUnidadOperativaBO> lstConfigUO = new ModuloBR().ConsultarConfiguracionUnidadOperativa(this.dataContext, new ConfiguracionUnidadOperativaBO() { UnidadOperativa = new UnidadOperativaBO() { Id = this.vista.UnidadOperativaId } }, this.vista.ModuloID);
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
                if (this.vista.UnidadOperativaId == null) throw new Exception("El identificador de la Unidad Operativa no debe ser nulo ");

                //crea seguridad
                UsuarioBO usuario = new UsuarioBO { Id = this.vista.UsuarioAutenticado };
                AdscripcionBO adscripcion = new AdscripcionBO { UnidadOperativa = new UnidadOperativaBO { Id = this.vista.UnidadOperativaId } };
                SeguridadBO seguridad = new SeguridadBO(Guid.Empty, usuario, adscripcion);

                //consulta lista de acciones permitidas
                this.vista.ListaAcciones = FacadeBR.ConsultarAccion(dataContext, seguridad);

                //Se valida si el usuario tiene permiso para consultar
                if (!this.ExisteAccion(this.vista.ListaAcciones, "CONSULTAR"))
                    this.vista.RedirigirSinPermisoAcceso();
                //Se valida si el usuario tiene permiso para insertar cuenta cliente
                if (!this.ExisteAccion(this.vista.ListaAcciones, "REGISTRARDOCUMENTO"))
                    this.vista.PermitirRegistrar(false);
            }
            catch (Exception ex)
            {
                throw new Exception(this.nombreClase + ".EstablecerSeguridad:" + ex.Message);
            }
        }
        private bool ExisteAccion(List<CatalogoBaseBO> acciones, string nombreAccion)
        {
            if (acciones != null && acciones.Count > 0 &&
                acciones.Exists(
                    p => p != null && p.Nombre != null && p.Nombre.Trim().ToUpper() == nombreAccion.Trim().ToUpper()))
                return true;

            return false;
        }
        #endregion

        public void Consultar()
        {
            try
            {
                UnidadBOF bo = (UnidadBOF)this.InterfazUsuarioADato();

                List<BO.UnidadBO> lst = controlador.Consultar(dataContext, bo).ConvertAll(s => (BO.UnidadBO)s);

                this.vista.Resultado = this.ComplementarDatos(lst);
                this.vista.ActualizarResultado();

                if (lst.Count < 1)
                    this.vista.MostrarMensaje("La búsqueda no produjo resultados", ETipoMensajeIU.INFORMACION,
                        "No se encontraron coincidencias con los criterios ingresados. Verifique sus datos");
            }
            catch (Exception ex)
            {
                throw new Exception(this.nombreClase + ".Consultar:" + ex.Message);
            }
        }
        private List<BO.UnidadBO> ComplementarDatos(List<BO.UnidadBO> lst)
        {
            //Esto filtra los resultados si se elige ver las actas terminadas
            if (this.vista.EstatusActa != null && (EEstatusUnidad)int.Parse(this.vista.EstatusActa.Value.ToString()) != EEstatusUnidad.NoDisponible)
            {
                List<BO.UnidadBO> unidadDisponible = new List<BO.UnidadBO>();
                foreach (BO.UnidadBO unidadTemp in lst)
                {
                    if (unidadTemp.EstatusActual != EEstatusUnidad.NoDisponible)
                        unidadDisponible.Add(unidadTemp);
                }
                lst = unidadDisponible;
            }

            //Se hace la consulta una vez de TODAS las sucursales para optimizar el tiempo
            List<SucursalBO> lstSucursales = FacadeBR.ConsultarSucursal(dataContext, new SucursalBO());

            List<BO.UnidadBO> lstFinal = new List<BO.UnidadBO>();

            foreach (BO.UnidadBO unidadTemp in lst)
            {
                //Obtener los datos completos de la Sucursal correspondiente
                unidadTemp.Sucursal = lstSucursales.Find(p => p.Id == unidadTemp.Sucursal.Id);

                if (unidadTemp.Sucursal == null)
                    unidadTemp.Sucursal = new SucursalBO();

                ///Obtener los clientes
                ClienteBO clienteTemp = new ClienteBO();

                if (unidadTemp.Cliente.Id != null)
                {
                    clienteTemp.Id = unidadTemp.Cliente.Id;
                    List<ClienteBO> clientesTemp = FacadeBR.ConsultarCliente(dataContext, clienteTemp);

                    clienteTemp = clientesTemp.Find(p => p.Id == clienteTemp.Id);
                }

                unidadTemp.Cliente = clienteTemp;

                lstFinal.Add(unidadTemp);
            }

            return lstFinal;
        }

        public void CambiarPaginaResultado(int nuevoIndicePagina)
        {
            this.vista.IndicePaginaResultado = nuevoIndicePagina;
            this.vista.ActualizarResultado();
        }

        private object InterfazUsuarioADato()
        {
            if (this.vista.UnidadOperativaId == null) throw new Exception(".InterfazUsuarioADato: La Unidad Operativa no puede ser nulo");
            UnidadBOF bo = new UnidadBOF();
            bo.Sucursal = new SucursalBO();
            bo.Sucursal.UnidadOperativa = new UnidadOperativaBO{Id = this.vista.UnidadOperativaId};
            bo.Cliente = new ClienteBO();
            bo.ActivoFijo = new ActivoFijoBO();

            bo.NumeroSerie = this.vista.NumeroVIN;

            bo.Sucursal.Id = this.vista.SucursalID;
            bo.Sucursal.Nombre = this.vista.SucursalNombre;

            if (vista.SucursalID != null)
            {
                bo.Sucursal.Id = this.vista.SucursalID;
                bo.Sucursal.Nombre = this.vista.SucursalNombre;
            }
            else
                bo.Sucursales = FacadeBR.ConsultarSucursalesSeguridad(dataContext, new SeguridadBO(Guid.Empty, new UsuarioBO() { Id = this.vista.UsuarioAutenticado }, new AdscripcionBO { UnidadOperativa = new UnidadOperativaBO() { Id = this.vista.UnidadOperativaId } }));

            bo.NumeroEconomico = this.vista.NumeroEconomico;
            bo.Cliente.Id = this.vista.ClienteID;
            bo.Cliente.Nombre = this.vista.ClienteNombre;
            bo.ActivoFijo.FechaFacturaCompra = this.vista.FechaCompra;

            if (this.vista.Area != null)
            {
                Type TipoEnum = typeof(EArea);
                if (this.vista.UnidadOperativaId != null)
                {
                    TipoEnum = bo.obtenerEAreas((ETipoEmpresa)this.vista.UnidadOperativaId);
                }
                bo.Area = (Enum)Enum.ToObject(TipoEnum, Convert.ToInt32(this.vista.Area.Value));
            }
            if (this.vista.EstatusActa != null)
            {
                if ((EEstatusUnidad)int.Parse(this.vista.EstatusActa.Value.ToString()) == EEstatusUnidad.NoDisponible)
                    bo.EstatusActual = (EEstatusUnidad)int.Parse(this.vista.EstatusActa.Value.ToString());
            }

            return bo;
        }

        public void VerDetalles(int index)
        {
            if (index >= this.vista.Resultado.Count || index < 0)
                throw new Exception("No se encontró el acta de nacimiento seleccionado.");

            BO.UnidadBO bo = this.vista.Resultado[index];

            this.vista.LimpiarSesion();
            this.vista.EstablecerPaqueteNavegacion("UnidadBO", bo); //]Session[nombre]=value

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

                    ebBO.Unidad.NumeroSerie = this.vista.NumeroVIN;
                    ebBO.Unidad.Activo = true;
                    ebBO.ActivoFijo.NumeroSerie = this.vista.NumeroVIN;
                    ebBO.ActivoFijo.Libro = this.vista.LibroActivos;
                    obj = ebBO;
                    break;

                case "Sucursal":
                    SucursalBOF sucursal = new SucursalBOF();
                    sucursal.UnidadOperativa = new UnidadOperativaBO();
                    sucursal.UnidadOperativa.Id = this.vista.UnidadOperativaId;
                    sucursal.Nombre = this.vista.SucursalNombre;
                    sucursal.Usuario = new UsuarioBO() { Id = this.vista.UsuarioAutenticado }; 
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

        #region REQ 13285 Método relacionado con las acciones dependiendo de la unidad operativa.
        /// <summary>
        /// Invoca el método EstablecerAcciones de la vista  IConsultarActaNacimientoVIS.
        /// </summary>
        public void EstablecerAcciones()
        {
            ETipoEmpresa EmpresaConPermiso = ETipoEmpresa.Idealease;
            switch(this.vista.UnidadOperativaId)
            {
                case (int)ETipoEmpresa.Generacion:
                    if (ExisteAccion(this.vista.ListaAcciones, "UI ACTA GENERACION"))
                    {
                        EmpresaConPermiso = ETipoEmpresa.Generacion;
                    }
                    break;
                case (int)ETipoEmpresa.Equinova:
                    if (ExisteAccion(this.vista.ListaAcciones, "UI ACTA GENERACION")) {
                        EmpresaConPermiso = ETipoEmpresa.Equinova;
                    }
                    break;
                case (int)ETipoEmpresa.Construccion:
                    if (ExisteAccion(this.vista.ListaAcciones, "UI ACTA CONSTRUCCION"))
                    {
                        EmpresaConPermiso = ETipoEmpresa.Construccion;
                    }
                    break;
            }
            this.vista.EstablecerAcciones(EmpresaConPermiso);
        }
        #endregion
        #endregion

       
    }
}
