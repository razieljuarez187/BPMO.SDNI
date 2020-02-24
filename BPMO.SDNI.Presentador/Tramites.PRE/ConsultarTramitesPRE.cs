//Satisface al CU087 – Catálogo Tramite Unidad
using System;
using System.Collections.Generic;
using BPMO.Basicos.BO;
using BPMO.Facade.SDNI.BR;
using BPMO.Patterns.Creational.DataContext;
using BPMO.Primitivos.Enumeradores;
using BPMO.SDNI.Tramites.BR;
using BPMO.SDNI.Tramites.VIS;
using BPMO.Servicio.Catalogos.BO;

namespace BPMO.SDNI.Tramites.PRE
{
    public class ConsultarTramitesPRE
    {
        #region Atributos
        private string nombreClase = "ConsultarTramitesPRE";
        private IConsultarTramitesVIS vista;
        private IDataContext dctx;
        private TramiteBR tramiteBR;
        #endregion

        #region Constructor
        public ConsultarTramitesPRE(IConsultarTramitesVIS vista)
        {
            try
            {
                this.vista = vista;
                dctx = FacadeBR.ObtenerConexion();
                tramiteBR = new TramiteBR();

            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencias en la configuración", ETipoMensajeIU.ERROR, this.nombreClase + ".Page_Load: " + ex.Message);
            }
        }
        #endregion

        #region Métodos
        public void Inicializar()
        {
            this.LimpiarSesion();

            this.EstablecerSeguridad(); //SC_0008
        }

        public void ConsultarTramitables()
        {
            try
            {
                int tipoTramitable = (int)ETipoTramitable.Unidad;
                int? tramitableID = null;
                if (this.vista.NumeroSerie == null)
                    this.vista.TramitableID = null;
                if (this.vista.TramitableID != null)
                    tramitableID = this.vista.TramitableID;

                List<Equipos.BO.UnidadBO> lstTramitables = tramiteBR.ConsultarUnidadTramite(dctx, tipoTramitable, tramitableID);

                this.vista.Tramitables = lstTramitables;
                this.vista.MostrarDatos();

                if (lstTramitables.Count < 1)
                    this.vista.MostrarMensaje("La búsqueda no produjo resultados", ETipoMensajeIU.INFORMACION,
                        "No se encontraron coincidencias con los criterios ingresados. Verifique sus datos");
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Error al intentar consultar los Unidades con tramites", ETipoMensajeIU.ERROR, nombreClase + ".ConsultarTramitables: " + ex.Message);
            }
        }
        public void VerDetalles(int index)
        {
            if (index >= this.vista.Tramitables.Count || index < 0)
                throw new Exception("No se encontró la unidad seleccionada.");
            Equipos.BO.UnidadBO bo = this.vista.Tramitables[index];
            this.LimpiarSesion();
            this.vista.EstablecerPaqueteNavegacion("DatosTramitable", bo);
            this.vista.RedirigirADetalles();
        }

        private void LimpiarSesion()
        {
            this.vista.LimpiarSesion();
        }

        private void MostrarMensaje(string mensaje, ETipoMensajeIU tipo, string detalle = null)
        {
            this.vista.MostrarMensaje(mensaje, tipo, detalle);
        }

        #region Métodos para el Buscador
        public object PrepararBOBuscador(string catalogo)
        {
            object obj = null;
            switch (catalogo)
            {
                case "Unidad":
                    var unidad = new Equipos.BO.UnidadBO();
                    if (!string.IsNullOrEmpty(this.vista.NumeroSerie))
                        unidad.NumeroSerie = this.vista.NumeroSerie;
                    obj = unidad;
                    break;
            }
            return obj;
        }

        public void DesplegarResultadoBuscador(string catalogo, object selecto)
        {
            switch (catalogo)
            {
                case "Unidad":
                    Equipos.BO.UnidadBO unidad = (Equipos.BO.UnidadBO)selecto;
                    if (unidad == null)
                        unidad = new Equipos.BO.UnidadBO();
                    if (unidad.Modelo == null)
                        unidad.Modelo = new ModeloBO();
                    if (unidad.Modelo.Marca == null)
                        unidad.Modelo.Marca = new MarcaBO();
                    this.vista.NumeroSerie = unidad.NumeroSerie;
                    this.vista.TramitableID = unidad.UnidadID;
                    break;
            }
        }
        #endregion

        #region SC_0008
        private void EstablecerSeguridad()
        {
            try
            {
                //se valida que los datos del usuario y la unidad operativa no sean nulos
                if (this.vista.UsuarioId == null) throw new Exception("El identificador del usuario no debe ser nulo");
                if (this.vista.UnidadOperativaId == null) throw new Exception("El identificador de la Unidad Operativa no debe ser nulo ");

                //Se crea el objeto de seguridad
                UsuarioBO usuario = new UsuarioBO() { Id = this.vista.UsuarioId };
                AdscripcionBO adscripcion = new AdscripcionBO() { UnidadOperativa = new UnidadOperativaBO() { Id = this.vista.UnidadOperativaId } };
                SeguridadBO seguridadBO = new SeguridadBO(Guid.Empty, usuario, adscripcion);

                //Se obtienen las acciones a las cuales el usuario tiene permiso en este proceso
                List<CatalogoBaseBO> lst = FacadeBR.ConsultarAccion(this.dctx, seguridadBO);

                //Se valida si el usuario tiene permiso para consultar
                if (!this.ExisteAccion(lst, "CONSULTAR"))
                    this.vista.RedirigirSinPermisoAcceso();
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
        #endregion
    }
}
