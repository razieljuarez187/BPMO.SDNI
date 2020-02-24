//Satisface al CU010 - Catálogo de Documentos
using System;
using System.Collections.Generic;
using BPMO.Facade.SDNI.BR;
using BPMO.Patterns.Creational.DataContext;
using BPMO.Primitivos.Enumeradores;
using BPMO.SDNI.Comun.BO;
using BPMO.SDNI.Comun.VIS;

namespace BPMO.SDNI.Comun.PRE
{
	public class ucCatalogoDocumentosPRE
    {
        #region Atributos
        private IDataContext dctx = null;
        private IucCatalogoDocumentosVIS vista;
        #endregion

        #region Propiedades
        /// <summary>
        /// Vista sobre la que actua el presentador
        /// </summary>
        internal IucCatalogoDocumentosVIS Vista { get { return vista; } }
        #endregion

        #region Constructor
        public ucCatalogoDocumentosPRE(IucCatalogoDocumentosVIS iUcAgregarDocumentosVis)
		{
			this.vista = iUcAgregarDocumentosVis;
            this.dctx = FacadeBR.ObtenerConexion();
		}
        #endregion

        #region Métodos

        public void CargarListaArchivos(List<ArchivoBO> listaArchivos, List<TipoArchivoBO> listaTiposArchivos)
        {
            if (listaArchivos != null)
            {
                this.vista.OriginalesArchivos = listaArchivos;
                this.vista.NuevosArchivos = listaArchivos;
            }

            if (listaTiposArchivos != null)
                this.vista.TiposArchivo = listaTiposArchivos;
        }

        public void RequiereObservaciones(bool? tieneObservaciones)
        {
            if (tieneObservaciones != null)
                this.vista.TieneObservaciones = tieneObservaciones;
        }

        public void ModoEditable(bool? edicion)
        {
            if (edicion != null)
                this.vista.ModoEdicion = edicion;
        }

        public void EstablecerTiposArchivo(List<TipoArchivoBO> lst)
        {
            this.vista.TiposArchivo = lst;
        }

        public void AgregarArchivo()
		{
            if (!vista.ValidaArchivo()) return;
			ArchivoBO archivoBO = new ArchivoBO();
            archivoBO.Nombre = vista.ObtenerNombreArchivo();
            archivoBO.TipoAdjunto = vista.TipoAdjunto;
			archivoBO.Activo = true;
            archivoBO.Archivo = vista.ObtenerArchivosBytes();
            archivoBO.Observaciones = vista.ObtenerObservaciones();
            String tipo = vista.ObtenerExtension();
            TipoArchivoBO tipoArchivo = ValidarArchivo(tipo);
            if (tipoArchivo != null)
            {
                archivoBO.TipoArchivo = tipoArchivo;
                vista.AgregarArchivo(archivoBO);
            }
            vista.LimpiaCampos();
        }

        private TipoArchivoBO ValidarArchivo(String tipo)
        {
            List<TipoArchivoBO> tiposArchivo = this.vista.TiposArchivo;
            if (tiposArchivo != null)
            {
                TipoArchivoBO tipoArchivoTemp = tiposArchivo.Find(delegate(TipoArchivoBO ta) { return ta.Extension.ToUpper() == tipo.ToUpper(); });
                if (tipoArchivoTemp != null)
                {
                    return tipoArchivoTemp;
                }
                else
                {
                    this.vista.MostrarMensaje("El archivo no fué cargado.", ETipoMensajeIU.ERROR, 
                        "La extensión del archivo no se encuentra en la lista de tipos de archivo permitidos");
                }
            }
            else 
            {
                this.vista.MostrarMensaje("El archivo no fué cargado.", ETipoMensajeIU.ERROR, "No hay una lista de tipos de archivo cargada");
            }
            return null;
        }

        public void LimpiarSesion()
        {
            this.vista.LimpiarSesion();
        }
        #endregion
    }
}