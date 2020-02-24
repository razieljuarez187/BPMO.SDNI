//Esta clase satisface los requerimientos especificados en el caso de uso CU006 Consultar Flota de Renta Diaria
//Esta clase satisface los requerimientos especificados en el caso de uso CU008 - Consultar Entrega Recepcion de Unidad
using System;
using System.Collections.Generic;
using BPMO.Basicos.BO;
using BPMO.Facade.SDNI.BR;
using BPMO.Patterns.Creational.DataContext;
using BPMO.SDNI.Equipos.BO;
using BPMO.SDNI.Flota.BO;
using BPMO.SDNI.Flota.BOF;
using BPMO.SDNI.Flota.BR;
using BPMO.SDNI.Flota.VIS;

namespace BPMO.SDNI.Flota.PRE
{
    /// <summary>
    /// Presentador del detalle de flota
    /// </summary>
    public class DetalleFlotaPRE
    {
        #region Propiedades
        /// <summary>
        /// Vista del detalle
        /// </summary>
        private readonly IDetalleFlotaVIS vista;
        /// <summary>
        /// Vista del UC de datos generales
        /// </summary>
        private readonly IucDatosGeneralesElementoVIS vistaDG;
        /// <summary>
        /// Vista del UC de equipo aliado de unidad
        /// </summary>
        private readonly IucEquiposAliadosUnidadVIS vistaEA;
        /// <summary>
        /// Data contex que proeveerá el acceso a los datos
        /// </summary>
        private readonly IDataContext dctx;
        /// <summary>
        /// Controlador para ejecutar las consultas a la flota
        /// </summary>
        private readonly SeguimientoFlotaBR controlador;
        /// <summary>
        /// Presentador del UC de datos generales
        /// </summary>
        private readonly ucDatosGeneralesElementoPRE presentadorDG;
        /// <summary>
        /// Presentador del UC de equipos aliados
        /// </summary>
        private readonly ucEquiposAliadosUnidadPRE presentadorEA;
        /// <summary>
        /// Nombre de la clase
        /// </summary>
        private const string nombreClase = "DetalleFlotaPRE";
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor de la clase
        /// </summary>
        /// <param name="vista">Interfaz de detalle</param>
        /// <param name="vistadg">Interfaz del UC de Información general</param>
        /// <param name="vistaea">Interfaz de usuario del UC de Equipos aliados</param>
        public DetalleFlotaPRE(IDetalleFlotaVIS vista, IucDatosGeneralesElementoVIS vistadg, IucEquiposAliadosUnidadVIS vistaea)
        {
            if (ReferenceEquals(vista, null))
                throw new Exception(String.Format("{0}: La vista proporcionada no puede ser nula", this.ToString()));
            this.vista = vista;
            this.vistaDG = vistadg;
            this.vistaEA = vistaea;
            this.presentadorDG = new ucDatosGeneralesElementoPRE(this.vistaDG);
            this.presentadorEA = new ucEquiposAliadosUnidadPRE(this.vistaEA);
            this.controlador = new SeguimientoFlotaBR();
            this.dctx = Facade.SDNI.BR.FacadeBR.ObtenerConexion();
        }
        #endregion

        #region Métodos
        /// <summary>
        /// Preparar la interfaz de usuario para el despliegue
        /// </summary>
        public void Inicializar()
        {
            this.VerificarOpRegresar();//CU008
            this.presentadorEA.Inicializar();
            this.presentadorDG.Inicializar();
            this.vista.InicializarControles();

            this.EstablecerDatosNavegacion(this.vista.ObtenerDatosNavegacion());
            if (!this.vista.UnidadID.HasValue)
                throw new Exception(nombreClase + ".Inicializar: Es necesario especificar la unidad de la cual deseas obtener los equipos aliados.");

            this.LimpiarSesion();

            this.EstablecerSeguridad();
        }
        /// <summary>
        /// Establece un objeto en session que se desea visualizar en la pantalla
        /// </summary>
        /// <param name="paquete">Objeto que se desea desplegar en la interfaz de usuario</param>
        private void EstablecerDatosNavegacion(object paquete)
        {
            try
            {
                if (ReferenceEquals(paquete, null))
                    throw new Exception("Se esperaba un objeto en la navegación. No se puede identificar el elemento de la flota que se desea consultar a detalle.");
                if (!(paquete is ElementoFlotaBO))
                    throw new Exception("Se esperaba una Unidad de Idealease.");
                if (!(((ElementoFlotaBO)paquete).Unidad != null && ((ElementoFlotaBO)paquete).Unidad.UnidadID != null))
                    throw new Exception("No se pudo identificar la unidad.");

                //Se consulta el elemento enviado en la navegación
                ElementoFlotaBO elemento = (ElementoFlotaBO)paquete;
                FlotaBO flota = this.controlador.ConsultarFlotaRentaDiaria(this.dctx, new FlotaBOF() { Unidad = elemento.Unidad });

                if (!(flota.ElementosFlota != null && flota.ElementosFlota.Count > 0))
                    throw new Exception("No se encontró la unidad en la flota de renta diaria.");

                elemento = flota.ElementosFlota[0];

                //Desplegamos los resultados en pantalla
                this.DatoAInterfazUsuario(elemento);
            }
            catch (Exception ex)
            {
                this.presentadorDG.Inicializar();
                throw new Exception(nombreClase + ".EstablecerDatosNavegacion: " + ex.Message);
            }
        }
        /// <summary>
        /// Despliega en pantalla la información del elemento de flota seleccionado
        /// </summary>
        /// <param name="elementoFlotaBO">Elemento de la flota seleccionado</param>
        private void DatoAInterfazUsuario(object elementoFlotaBO)
        {
            ElementoFlotaBO elemento = elementoFlotaBO as ElementoFlotaBO;

            if (ReferenceEquals(elemento.Unidad, null))
                elemento.Unidad = new UnidadBO();

            this.vista.EquipoID = elemento.Unidad.EquipoID.HasValue ? elemento.Unidad.EquipoID.Value : (int?)null;
            this.vista.NumeroEconomico = !string.IsNullOrEmpty(elemento.Unidad.NumeroEconomico) && !string.IsNullOrWhiteSpace(elemento.Unidad.NumeroEconomico)
                ? elemento.Unidad.NumeroEconomico.Trim().ToUpper()
                : string.Empty;
            this.vista.NumeroSerie = !string.IsNullOrEmpty(elemento.Unidad.NumeroSerie) && !string.IsNullOrWhiteSpace(elemento.Unidad.NumeroSerie)
                ? elemento.Unidad.NumeroSerie.Trim().ToUpper()
                : string.Empty;
            this.vista.UnidadID = elemento.Unidad.UnidadID.HasValue ? elemento.Unidad.UnidadID.Value : (int?)null;

            this.presentadorDG.Inicializar();
            this.presentadorDG.DatoAInterfazUsuario(elemento as object);

            this.presentadorEA.Inicializar();
            this.presentadorEA.DatoAInterfazUsuario(elemento as object);
            this.presentadorEA.CargarEquiposAliados();
        }
        /// <summary>
        /// Limpia la sesion del usuario para liberar recursos
        /// </summary>
        private void LimpiarSesion()
        {
            this.vista.LimpiarSesion();
        }
        /// <summary>
        /// Valida el acceso a la página para el usuario autenticado
        /// </summary>
        public void ValidarAcceso()
        {
            try
            {
                //se valida que los datos del usuario y la unidad operativa no sean nulos
                if (ReferenceEquals(this.vista.Usuario, null))
                    throw new Exception("Es necesario proporcionar el usuario que esta autenticado en el sistema.");
                if (!this.vista.Usuario.Id.HasValue)
                    throw new Exception("Es necesario proporcionar el usuario que esta identificado en el sistema.");
                if (!this.vista.UnidadOperativa.Id.HasValue)
                    throw new Exception("Es necesario proporcionar la unidad operativa del usuario autenticado en el sistema.");

                //Se crea el objeto de seguridad
                UsuarioBO usr = new UsuarioBO { Id = this.vista.Usuario.Id.Value };
                AdscripcionBO adscripcion = new AdscripcionBO
                {
                    UnidadOperativa = new UnidadOperativaBO { Id = this.vista.UnidadOperativa.Id.Value }
                };
                SeguridadBO seguridadBO = new SeguridadBO(Guid.Empty, usr, adscripcion);

                //Se valida si el usuario tiene permiso a la acción principal
                if (!FacadeBR.ExisteAccion(dctx, "CONSULTAR", seguridadBO))
                    this.vista.RedirigirSinPermisoAcceso();
            }
            catch (Exception ex)
            {
                throw new Exception(nombreClase + ".ValidarAcceso:" + ex.Message);
            }
        }
        /// <summary>
        /// Verifica los permisos para los usuarios autenticados en el sistema
        /// </summary>
        public void EstablecerSeguridad()
        {
            try
            {
                //se valida que los datos del usuario y la unidad operativa no sean nulos
                if (ReferenceEquals(this.vista.Usuario, null))
                    throw new Exception("Es necesario proporcionar el usuario que esta autenticado en el sistema.");
                if (!this.vista.Usuario.Id.HasValue)
                    throw new Exception("Es necesario proporcionar el usuario que esta identificado en el sistema.");
                if (!this.vista.UnidadOperativa.Id.HasValue)
                    throw new Exception("Es necesario proporcionar la unidad operativa del usuario autenticado en el sistema.");

                UsuarioBO usr = new UsuarioBO { Id = this.vista.Usuario.Id.Value };
                AdscripcionBO adscripcion = new AdscripcionBO
                {
                    UnidadOperativa = new UnidadOperativaBO { Id = this.vista.UnidadOperativa.Id.Value }
                };
                SeguridadBO seguridadBO = new SeguridadBO(Guid.Empty, usr, adscripcion);

                //Se obtienen las acciones a las cuales el usuario tiene permiso en este proceso
                List<CatalogoBaseBO> lst = FacadeBR.ConsultarAccion(this.dctx, seguridadBO);

                //Se valida si el usuario tiene permiso para consultar
                if (!this.ExisteAccion(lst, "CONSULTAR"))
                    this.vista.RedirigirSinPermisoAcceso();
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("{0}.{1} Excepción interna:{2}{3}", nombreClase, "EstablecerSeguridad", Environment.NewLine, ex.Message));
            }
        }
        /// <summary>
        /// Valida una acción en especifico dentro de la lista de acciones permtidas para la pagina
        /// </summary>
        /// <param name="acciones">Listado de acciones permitidas para la página</param>
        /// <param name="accion">Acción que se desea validar</param>
        /// <returns>si la acción a evaluar se encuantra dentro de la lista de acciones permitidas se devuelve true. En caso ocntario false. bool</returns>        
        private bool ExisteAccion(List<CatalogoBaseBO> acciones, string accion)
        {
            if (acciones != null && acciones.Count > 0 && acciones.Exists(p => p != null && p.Nombre != null && p.Nombre.Trim().ToUpper() == accion.Trim().ToUpper()))
                return true;

            return false;
        }
        /// <summary>
        /// SC0019
        /// Redirige a la página principal de consulta
        /// </summary>
        public void RetrocederPagina()
        {
            this.vista.RegresarAConsultar();
        }
        /// <summary>
        /// CU008 Verifica si la opción regresar esta habilitada o no
        /// </summary>
        public void VerificarOpRegresar()
        {
            Dictionary<string, object> elementosFiltro = this.vista.ObtenerFiltrosConsulta() as Dictionary<string, object>;
            if (elementosFiltro == null)
                this.vista.PermitirRegresar(false);
            else
                this.vista.PermitirRegresar(true);
        }
        #endregion        
    }
}