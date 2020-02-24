//Satisface al CU077 - Registrar Acta de Nacimiento de una Unidad
using System;
using System.Collections.Generic;
using BPMO.Facade.SDNI.BR;
using BPMO.Patterns.Creational.DataContext;
using BPMO.Primitivos.Enumeradores;
using BPMO.SDNI.Equipos.BO;
using BPMO.SDNI.Tramites.BR;
using BPMO.SDNI.Tramites.VIS;
using BPMO.Basicos.BO;
using BPMO.SDNI.Comun.BO;
using BPMO.SDNI.Tramites.BO;
using BPMO.SDNI.Comun.PRE;
using BPMO.SDNI.Comun.VIS;

namespace BPMO.SDNI.Tramites.PRE
{
    public class ucTramitesActivosPRE
    {
        #region Atributos
        private IDataContext dctx = null;
        private TramiteBR controlador;

        private IucTramitesActivosVIS vista;

        //RQM 13285
        private ucCatalogoDocumentosPRE presentadorDocumentos;
        public IucCatalogoDocumentosVIS vistaDocumento { get; set; }

        private string nombreClase = "ucTramitesUnidadPRE";
        #endregion

        #region Constructores
        public ucTramitesActivosPRE(IucTramitesActivosVIS view)
        {
            try
            {
                this.vista = view;               

                this.controlador = new TramiteBR();

                this.dctx = FacadeBR.ObtenerConexion();

                this.EstablecerTipoAdjunto();
                this.vista.EstablecerIdentificadorListaArchivos(nombreClase);
            }
            catch (Exception ex)
            {
                this.vista.MostrarMensaje("Inconsistencias en los parámetros de configuración", ETipoMensajeIU.ERROR, this.nombreClase + ".ucTramitesUnidadPRE:" + ex.Message);
            }
        }
        #endregion

        #region Métodos
        /// <summary>
        /// Consulta y muestra los trámites activos de un tramitable
        /// </summary>
        /// <param name="tramitableID">Identificador del tramitable</param>
        /// <param name="tipoTramitable">Tipo de tramitable</param>
        /// <param name="UnidadOperativaId">Identificador de la unidad operativa REQ: 13285</param>
        public void CargarTramites(int? tramitableID, ETipoTramitable? tipoTramitable, string descripcionEnllantable, int? UnidadOperativaId=null)
        {
            List<TramiteBO> lst = new List<TramiteBO>();

            //RQM 13285, habilitamos pedimento


            //Si se proporciona un tramitable, se obtienen sus trámites
            if (tramitableID != null && tipoTramitable != null)
            {
                TramiteBO bo = new TramiteProxyBO();
                bo.Activo = true;
                bo.Tramitable = new TramitableProxyBO() { TramitableID = tramitableID, TipoTramitable = tipoTramitable };

                lst = this.controlador.Consultar(this.dctx, bo);
            }
           
            //Se recorren los tipos de trámites para incluir los que no se hayan realizado aún
            foreach (ETipoTramite tipo in Enum.GetValues(typeof(ETipoTramite)))
            {
                if (!(lst.Exists(p => p.Tipo != null && p.Tipo == tipo)))
                {
                    TramiteBO temp = new TramiteProxyBO() { Activo = false, Tipo = tipo };
                    lst.Add(temp);
                    
                }
            }

            //Se obtiene el seguro para concatenarle su aseguradora
            foreach (TramiteBO bo in lst)
            {
                if (bo.Activo != null && bo.Activo == true && bo.Tipo != null && bo.Tipo == ETipoTramite.SEGURO && bo.TramiteID != null)
                {
                    SeguroBO seguro = new SeguroBO(bo);

                    List<SeguroBO> lstTemp = new SeguroBR().Consultar(this.dctx, seguro);
                    if (lstTemp.Count > 0)
                        seguro = lstTemp[0];
                    if (seguro.Aseguradora != null)
                    {
                        if (seguro.NumeroPoliza == null)
                            seguro.NumeroPoliza = "";
                        seguro.NumeroPoliza += " (" + seguro.Aseguradora + ")";
                    }

                    bo.Resultado = seguro.Resultado;
                }
            }

            //Se asigna la información a la vista
            this.vista.TramitableID = tramitableID;
            this.vista.TipoTramitable = tipoTramitable;
            this.vista.DescripcionEnllantable = descripcionEnllantable;
            this.vista.Tramites = lst;
            this.vista.ActualizarTramites();          
        }

        /// <summary>
        /// Indica si existe un tipo de trámite específico (activo) en la lista de trámites
        /// </summary>
        /// <param name="tipoTramite">Tipo de trámite a comprobar</param>
        /// <returns>Verdadero si existe, falso en caso contrario</returns>
        public bool ExisteTramite(ETipoTramite tipoTramite)
        {
            if (this.vista.Tramites != null && this.vista.Tramites.Exists(p => p.Tipo != null && p.Tipo == tipoTramite && p.Activo != null && p.Activo == true))
                return true;

            return false;
        }

        /// <summary>
        /// Permite ocultar la opción de redirección hacia el catálogo de trámites del tramitable en cuestión
        /// </summary>
        /// <param name="ocultar">Indica si se desea ocultar o mostrar la opción</param>
        public void OcultarRedireccionTramites(bool ocultar)
        {
            this.vista.OcultarRedireccionTramites(ocultar);
        }


        /// <summary>
        ///
        /// </summary>
        /// <param name="ocultar">Indica si se desea ocultar o mostrar la opción</param>
        public void PresentarDocumentos()
        {

                //RQM 13285               
                this.vista.VistaDocumentos.ModoEdicion = false;
                this.vista.VistaDocumentos.DespliegaArchivos();
                this.vista.VistaDocumentos.OcultarCarga();
                
        }

        /// <summary>
        /// Redirige al Catálogo de Trámites con el tramitable seleccionado
        /// </summary>
        public void VerCatalogoTramites()
        {
            ITramitable bo = new TramitableProxyBO() { TramitableID = this.vista.TramitableID, TipoTramitable = this.vista.TipoTramitable, DescripcionTramitable = this.vista.DescripcionEnllantable };

            this.vista.LimpiarSesion();

            this.vista.EstablecerPaqueteNavegacion("DatosTramitable", bo); 

            this.vista.RedirigirACatalogoTramites();
        }
        #region RQM 13285
        /// <summary>
        /// Método que establece las acciones que se realizarán en la vista de trámites
        /// </summary>
        /// <param name="ListaAcciones">Lista de acciones a los que tiene permisos el usuario</param>
        public void EstablecerAcciones(List<CatalogoBaseBO> ListaAcciones)
        {
            this.vista.ListaAcciones = ListaAcciones;
            this.vista.EstablecerAcciones();
        }

        /// <summary>
        /// Permite ocultar la opción de redirección hacia el catálogo de trámites del tramitable en cuestión
        /// </summary>
        /// <param name="ocultar">Indica si se desea ocultar o mostrar la opción</param>
        public void HabilitarPedimento(bool habilitar)
        {
            this.vista.HabilitarPedimento(habilitar);
        }

        public void HabilitarCargaArchivo(bool habilitar)
        {
            this.vista.HabilitarCargaArchivo(habilitar);
        }

        public void EstablecerTiposdeArchivo(List<TipoArchivoBO> tipos)
        {
            this.vista.TiposArchivo = tipos;
        }

        public void EstablecerTipoAdjunto()
        {
            this.vista.EstablecerTipoAdjunto(ETipoAdjunto.ActaNacimiento);
        }

        public void ModoEdicion(bool habilitar)
        {
            this.vista.ModoEdicion(habilitar);
        }

        #endregion
        #endregion
    }
}
