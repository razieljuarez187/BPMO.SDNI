//Satisface el CU066 - Reporte Sistemas Revisados
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BPMO.SDNI.Mantenimiento.Reportes.VIS;
using BPMO.SDNI.Mantenimiento.Reportes.BR;
using BPMO.Patterns.Creational.DataContext;
using BPMO.Basicos.BO;
using System.Collections;
using BPMO.SDNI.Mantenimiento.Reportes.DA;
using BPMO.Primitivos.Enumeradores;
using BPMO.Facade.SDNI.BR;

namespace BPMO.SDNI.Mantenimientos.Reportes.PRE
{
    /// <summary>
    /// Presentador para la UI del reporte de sistemas revisados
    /// </summary>
    public class SistemasRevisadosPRE
    {
        #region Atributos
        /// <summary>
        /// Vista de sistemas revisados
        /// </summary>
        ISistemasRevisadosVIS vista;

        /// <summary>
        /// Nombre de clase
        /// </summary>
        private const String nombreClase = "SistemasRevisadosPRE";

        /// <summary>
        /// Controlador de sistemas revisados
        /// </summary>
        private readonly SistemasRevisadosBR controlador;

        /// <summary>
        /// El DataContext que proveerá acceso a la base de datos
        /// </summary>
        private readonly IDataContext dctx;
        #endregion

        #region Constructores
        public SistemasRevisadosPRE(ISistemasRevisadosVIS view)
        {
            this.vista = view;
            this.controlador = new SistemasRevisadosBR();
            this.dctx = Facade.SDNI.BR.FacadeBR.ObtenerConexion();
        }
        #endregion

        #region Metodos

        #region Métodos para el Buscador
        /// <summary>
        /// Metodo que prepara el objeto para el buscador
        /// <param name="catalogo">Nombre del catalogo por buscar</param>
        /// </summary>
        public object PrepararBOBuscador(string catalogo)
        {
            object obj = null;

            switch (catalogo)
            {
                case "UnidadIdealease":
                    BPMO.SDNI.Equipos.BO.UnidadBO unidad = new BPMO.SDNI.Equipos.BO.UnidadBO();
                    unidad.NumeroSerie = this.vista.NumeroSerie;
                    obj = unidad;
                    break;              
            }

            return obj;
        }

        /// <summary>
        /// Metodo para desplegar resultado de buscador
        /// <param name="catalogo">Nombre del catalogo por buscar</param>
        /// <param name="selecto">Objeto obtenido por el buscador</param>
        /// </summary>
        public void DesplegarResultadoBuscador(string catalogo, object selecto)
        {
            switch (catalogo)
            {
                case "UnidadIdealease":
                    if (selecto != null)
                    {
                        var unidad = (BPMO.SDNI.Equipos.BO.UnidadBO)selecto;
                        if (unidad.UnidadID != null)
                        {
                            this.vista.UnidadID = unidad.UnidadID;
                        }
                        if (unidad.NumeroSerie != null)
                        {
                            this.vista.NumeroSerie = unidad.NumeroSerie;
                        }
                    }
                    break;               
            }
        }
        #endregion

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

        /// <summary>
        /// Metodo que realiza la consulta de sistemas revisados por medio del controlador
        /// </summary>
        public void Consultar()
        {
            string s;
            if (!String.IsNullOrEmpty((s = this.ValidarDatos())))
            {
                this.vista.MostrarMensaje("Los siguientes datos son incorrectos:" + s.Substring(1), ETipoMensajeIU.ADVERTENCIA, null);
                return;
            }
            Hashtable parameters = new Hashtable();
            parameters["UnidadOperativaID"] = this.vista.UnidadOperativaID;
            parameters["ModuloID"] = this.vista.ModuloID;
            parameters["SucursalID"] = this.vista.SucursalID;
            parameters["ClienteID"] = this.vista.ClienteID;
            parameters["NumeroSerie"] = this.vista.NumeroSerie;
            parameters["FechaInicio"] = this.vista.FechaInicio;
            parameters["FechaFin"] = this.vista.FechaFin;
            Dictionary<String, Object> resultados = controlador.ConsultarSistemasRevisados(dctx, parameters);
            if (!resultados.ContainsKey("DataSource"))
            {
                throw new ArgumentNullException("resultados.DataSource");
            }
            if (resultados["DataSource"] != null)
            {
                SistemasRevisadosDS ds = (SistemasRevisadosDS)resultados["DataSource"];
                if (ds.Tables["ServicioMantenimiento"].Rows.Count != 0)
                {
                    this.vista.EstablecerPaqueteNavegacionImprimir("CU066", resultados);
                    this.vista.IrAImprimir();
                }
                else
                {
                    this.vista.MostrarMensaje("No se encontraron coincidencias para el filtro seleccionado", ETipoMensajeIU.INFORMACION, "Sin resultado obtenido en la consulta");
                }
            }
            else
            {
                this.vista.MostrarMensaje("Error en obtención de información", ETipoMensajeIU.ERROR, "No se pudo obtener la información");
            }
        }

        private string ValidarDatos()
        {
            string s = string.Empty;
            try
            {
                if (this.vista.FechaInicio == null)
                {
                    s += ", Fecha Inicio es requerido";
                }

                if (this.vista.FechaFin == null)
                {
                    s += ", Fecha Fin es requerido";
                }

                if (this.vista.FechaInicio != null && this.vista.FechaFin != null &&
                    this.vista.FechaInicio > this.vista.FechaFin)
                {
                    s += ", Fecha Inicio no debe ser mayor a Fecha Fin";
                }
            }
            catch (Exception ex)
            {
                vista.MostrarMensaje("Inconsistencias al validar los datos", ETipoMensajeIU.ERROR, nombreClase + ".ValidarDatos: " + ex.Message);
            }

            return s;
        }

        #endregion

    }
}
