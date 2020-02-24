//Satisface al CU087 – Catálogo Tramite Unidad
using System;
using System.Collections.Generic;
using BPMO.Primitivos.Enumeradores;
using BPMO.SDNI.Comun.BO;
using BPMO.SDNI.Comun.PRE;
using BPMO.SDNI.Comun.VIS;
using BPMO.SDNI.Tramites.BO;
using BPMO.SDNI.Tramites.VIS;

namespace BPMO.SDNI.Tramites.PRE
{
    public class ucTramiteVerificacionPRE
    {
        #region Atributos
        private IucTramiteVerificacionVIS vista;
        private ucCatalogoDocumentosPRE presentadorDocumentos;
        private string nombreClase = "ucTramiteVerificacionPRE";
        #endregion
        #region Constructores
        public ucTramiteVerificacionPRE(IucTramiteVerificacionVIS vista, IucCatalogoDocumentosVIS vistaDocumento)
        {
            try
            {
                this.vista = vista;
                presentadorDocumentos = new ucCatalogoDocumentosPRE(vistaDocumento);

            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Error al intentar inicializar el presentador", ETipoMensajeIU.ERROR, nombreClase + ".ucTramiteVerificacionPRE: " + ex.Message);
            }
        }
        public ucTramiteVerificacionPRE(IucTramiteVerificacionVIS vista)
        {
            try
            {
                this.vista = vista;

            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Error al intentar inicializar el presentador", ETipoMensajeIU.ERROR, nombreClase + ".ucTramiteVerificacionPRE: " + ex.Message);
            }
        }
        #endregion
        #region Métodos
        public void Inicializar(ETipoVerificacion tipo)
        {
            this.LimpiarSesion();
            this.PrepararNuevo();          
            this.presentadorDocumentos = new ucCatalogoDocumentosPRE(this.vista.VistaDocumentos);
            this.EstablecerTipoVerificacion(tipo);
            this.EstablecerTipoAdjunto();
            this.vista.VistaDocumentos.Identificador = tipo.ToString();  
            presentadorDocumentos.LimpiarSesion();
            this.presentadorDocumentos.RequiereObservaciones(false);
            this.presentadorDocumentos.ModoEditable(false);
        }
        public void EstablecerIdentificadorArchivos()
        {
            this.vista.EstablecerIdentificadorListaArchivos(nombreClase+this.vista.TipoTramite);
        }
        public void EstablecerTiposdeArchivo(List<TipoArchivoBO> tipos)
        {
            this.vista.TiposArchivo = tipos;
        }
        public void EstablecerTipoVerificacion(ETipoVerificacion tipoVerificacion)
        {
            this.vista.TipoTramite = tipoVerificacion;
        }
        public void EstablecerTipoAdjunto()
        {
            this.vista.EstablecerTipoAdjunto(ETipoAdjunto.Tramite);
        }
        public void MostrarMensaje(string mensaje, ETipoMensajeIU tipo, string detalle = null)
        {
            this.vista.MostrarMensaje(mensaje,tipo,detalle);
        }
        public void ModoEdicion(bool Habilitar)
        {
            this.vista.ModoEdicion(Habilitar);
        }
        public void LimpiarSesion()
        {
            this.vista.LimpiarSesion();
        }
        public void PrepararNuevo()
        {
            this.vista.Archivos = null;
            this.vista.FechaInicio = null;
            this.vista.FechaFinal=null;
            this.vista.Folio = null;
            this.vista.UltimoObjetoVerificacion = null;
        }
        public VerificacionBO InterfazUsuarioADato()
        {
            string validar = this.validarDatos();
            if (String.IsNullOrEmpty(validar))
            {
                VerificacionBO verificacion = null; ;
                
                if (this.vista.UltimoObjetoVerificacion.TramiteID == null)
                {
                    verificacion = new VerificacionBO();

                    if (this.vista.Archivos.Count != 0)
                        verificacion.Archivos = this.vista.Archivos;
                    else
                        verificacion.Archivos = new List<ArchivoBO>();

                    verificacion.Auditoria = new Basicos.BO.AuditoriaBO { FC = this.vista.FC, FUA = this.vista.FUA, UC = this.vista.UC, UUA = this.vista.UUA };
                    verificacion.VigenciaInicial = this.vista.FechaInicio;
                    verificacion.Folio = this.vista.Folio;
                    verificacion.VigenciaFinal = this.vista.FechaFinal;
                    verificacion.Activo = true;
                    if (this.vista.TipoTramite == ETipoVerificacion.AMBIENTAL)
                    {
                        verificacion.Tipo = ETipoTramite.VERIFICACION_AMBIENTAL;
                        verificacion.TipoVerificacion = ETipoVerificacion.AMBIENTAL;
                    }
                    if (this.vista.TipoTramite == ETipoVerificacion.FISICO_MECANICO)
                    {
                        verificacion.Tipo = ETipoTramite.VERIFICACION_FISICO_MECANICA;
                        verificacion.TipoVerificacion = ETipoVerificacion.FISICO_MECANICO;
                    }
                    verificacion.Resultado = verificacion.Folio;
                    if (verificacion.Archivos != null)
                    {
                        foreach (ArchivoBO archivo in verificacion.Archivos)
                        {
                            if (archivo.Id == null)
                            {
                                archivo.Auditoria = new Basicos.BO.AuditoriaBO { FC = verificacion.Auditoria.FC, UC = verificacion.Auditoria.UC, FUA = verificacion.Auditoria.FUA, UUA = verificacion.Auditoria.UUA };
                            }
                            else
                            {
                                archivo.Auditoria = new Basicos.BO.AuditoriaBO { FUA = verificacion.Auditoria.FUA, UUA = verificacion.Auditoria.UUA, FC = archivo.Auditoria.FC, UC = archivo.Auditoria.UC };
                            }
                        }
                    }
                }
                else
                {
                    verificacion = new VerificacionBO();
                    verificacion.TramiteID = this.vista.UltimoObjetoVerificacion.TramiteID;
                    verificacion.TipoVerificacion = this.vista.UltimoObjetoVerificacion.TipoVerificacion;
                    if(this.vista.Archivos != null)
                    verificacion.Archivos = this.vista.Archivos;

                    verificacion.Auditoria = new Basicos.BO.AuditoriaBO {FUA = this.vista.FUA, UUA = this.vista.UUA, FC=this.vista.UltimoObjetoVerificacion.Auditoria.FC, UC=this.vista.UltimoObjetoVerificacion.Auditoria.UC };
                    verificacion.VigenciaInicial = this.vista.FechaInicio;
                    verificacion.Folio = this.vista.Folio;
                    verificacion.VigenciaFinal = this.vista.FechaFinal;
                    verificacion.Activo = this.vista.UltimoObjetoVerificacion.Activo;
                    verificacion.Tipo = this.vista.UltimoObjetoVerificacion.Tipo;
                    verificacion.Resultado = verificacion.Folio;
                    if (verificacion.Archivos != null)
                    {
                        foreach (ArchivoBO archivo in verificacion.Archivos)
                        {
                            if (archivo.Id == null)
                            {
                                archivo.Auditoria = new Basicos.BO.AuditoriaBO { FC = verificacion.Auditoria.FC, UC = verificacion.Auditoria.UC, FUA = verificacion.Auditoria.FUA, UUA = verificacion.Auditoria.UUA };
                            }
                            else
                            {
                                archivo.Auditoria = new Basicos.BO.AuditoriaBO { FUA = verificacion.Auditoria.FUA, UUA = verificacion.Auditoria.UUA, FC = archivo.Auditoria.FC, UC = archivo.Auditoria.UC };
                            }
                        }
                    }
                    
                }
                return verificacion;
            }
            else
                throw new Exception("Se requieren los siguientes datos : " + validar);
        }
        public void DatoAInterfazUsuario(VerificacionBO verificacion)
        {
            this.LimpiarSesion();
            this.Inicializar(verificacion.TipoVerificacion.Value);
            this.vista.UltimoObjetoVerificacion = verificacion;
            this.vista.FechaInicio = verificacion.VigenciaInicial;
            this.vista.Folio = verificacion.Folio;
            this.vista.FechaFinal = verificacion.VigenciaFinal;
            this.presentadorDocumentos.RequiereObservaciones(false);
            if (verificacion.Tipo == ETipoTramite.VERIFICACION_AMBIENTAL)
            {
                this.vista.TipoTramite = ETipoVerificacion.AMBIENTAL;
            }
            if (verificacion.Tipo == ETipoTramite.VERIFICACION_FISICO_MECANICA)
            {
                this.vista.TipoTramite = ETipoVerificacion.FISICO_MECANICO;
            }
            if (verificacion.Archivos.Count > 0)
            {
                this.presentadorDocumentos.CargarListaArchivos(verificacion.Archivos, new List<TipoArchivoBO>());
                this.presentadorDocumentos.Vista.DespliegaArchivos();
            }
        }
        public string validarDatos()
        {
            string s="";
            if (this.vista.FechaFinal == null)
                s += "Fecha Final, ";
            if (this.vista.FechaInicio == null)
                s += "Fecha Inicio, ";
            if (this.vista.Folio == null)
                s += " Folio, ";
            if (this.vista.FechaInicio != null && this.vista.FechaInicio != null)
            {
                if (this.vista.FechaFinal < this.vista.FechaInicio)
                    s += " La Fecha Final no debe ser menor que la Fecha Inicial, ";
            }
            return s;
        }
        #endregion
    }
}
