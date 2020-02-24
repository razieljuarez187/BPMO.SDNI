//Esta clase satisface al CU085 - CATALOGO INFORMACION SEGURO DE LA UNIDAD
using System;
using System.Collections.Generic;
using BPMO.Basicos.BO;
using BPMO.Facade.SDNI.BR;
using BPMO.Patterns.Creational.DataContext;
using BPMO.Primitivos.Enumeradores;
using BPMO.SDNI.Equipos.BOF;
using BPMO.SDNI.Tramites.BO;
using BPMO.SDNI.Tramites.BR;
using BPMO.SDNI.Tramites.VIS;

namespace BPMO.SDNI.Tramites.PRE
{    
    public class ConsultarSeguroPRE
    {
        #region Atributos
        private IConsultarSeguroVIS vista;
        private SeguroBR controlador;
        private IDataContext dctx = null;
        private string nombreClase = "ConsultarSeguroPRE";
        #endregion

        #region Constructores
        public ConsultarSeguroPRE(IConsultarSeguroVIS view)
        {
            this.vista = view;
            this.controlador = new SeguroBR();
            this.dctx = FacadeBR.ObtenerConexion();
        }
        #endregion

        #region Métodos
        public void PrepararBusqueda()
        {
            this.vista.PrepararBusqueda();

            this.EstablecerSeguridad(); //SC_0008
        }

        public void Consultar()
        {
            try
            {
                //Se obtiene la información a partir de la Interfaz de Usuario
                SeguroBO bo = (SeguroBO)this.InterfazUsuarioADato();
                List<SeguroBO> seguros = this.controlador.ConsultarCompleto(this.dctx, bo, this.vista.Vencido != null ? this.vista.Vencido : null);

                this.vista.Seguros = seguros;
                this.vista.ActualizarLista();

                if (seguros.Count < 1)
                    this.vista.MostrarMensaje("La búsqueda no produjo resultados", ETipoMensajeIU.INFORMACION,
                        "No se encontraron coincidencias con los criterios ingresados. Verifique sus datos");
            }
            catch (Exception ex)
            {
                throw new Exception(this.nombreClase + ".Consultar: " + ex.Message);
            }
        }

        private object InterfazUsuarioADato()
        {
            SeguroBO bo = new SeguroBO();
            BPMO.SDNI.Equipos.BO.UnidadBO tramitable = new BPMO.SDNI.Equipos.BO.UnidadBO();

            bo.Aseguradora = this.vista.Aseguradora;
            bo.NumeroPoliza = this.vista.NumeroPoliza;   
            tramitable.NumeroSerie = this.vista.VIN;
            tramitable.UnidadID = this.vista.TramitableID;;
            bo.Tramitable = tramitable;
            bo.Activo = true;
            return bo;
        }

        public void LimpiarSession()
        {
            this.vista.LimpiarSesion();
        }

        public void IrADetalle(int? tramiteID)
        {
            if (tramiteID.HasValue)
            {
                vista.EstablecerPaqueteNavegacion("TramiteSeguro", tramiteID);
                vista.IrADetalle();
            }
            else
            {
                vista.MostrarMensaje("No se ha proporcionado un seguro para visualizar en detalle.", ETipoMensajeIU.ADVERTENCIA);
            }
        }

        public string ValidarCamposConsultaUnidad()
        {
            string s = "";

            if (!(this.vista.VIN != null && this.vista.VIN.Trim().CompareTo("") != 0))
                s += "Número de Serie, ";

            if (s.Trim().CompareTo("") != 0)
                return "Los siguientes campos no pueden estar vacíos: \n" + s.Substring(0, s.Length - 2);

            return null;
        }

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

        #region Métodos para el Buscador
        public object PrepararBOBuscador(string catalogo)
        {
            object obj = null;

            switch (catalogo)
            {
                case "Unidad":
                    UnidadBOF unidad = new UnidadBOF();
                    unidad.Sucursal = new SucursalBO();
                    unidad.Sucursal.UnidadOperativa = new UnidadOperativaBO();

                    if (!string.IsNullOrEmpty(this.vista.VIN))
                        unidad.NumeroSerie = this.vista.VIN;
                    unidad.Sucursal.UnidadOperativa.Id = this.vista.UnidadOperativaId;

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
                    UnidadBOF unidad = (UnidadBOF)selecto;

                    if (unidad != null && unidad.NumeroSerie != null)
                    {
                        this.vista.VIN = unidad.NumeroSerie;
                        this.vista.TramitableID = unidad.TramitableID;
                    }
                    else
                    {
                        this.vista.VIN = null;
                        this.vista.TramitableID = null;
                    }
                    break;
            }
        }
        #endregion
        #endregion
    }
}