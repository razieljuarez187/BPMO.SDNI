// Satisface al CU089 - Bitácora de Llantas
using System;
using System.Collections.Generic;
using BPMO.Basicos.BO;
using BPMO.Facade.SDNI.BR;
using BPMO.Patterns.Creational.DataContext;
using BPMO.Primitivos.Enumeradores;
using BPMO.SDNI.Comun.BO;
using BPMO.SDNI.Comun.PRE;
using BPMO.SDNI.Comun.VIS;
using BPMO.SDNI.Equipos.BO;
using BPMO.SDNI.Equipos.BR;
using BPMO.SDNI.Equipos.VIS;
using BPMO.Facade.SDNI.BOF;
using BPMO.SDNI.Equipos.BOF;

namespace BPMO.SDNI.Equipos.PRE
{
    public class ucLlantaPRE
    {
        #region Atributos
        private IDataContext dctx = null;

        private IucLlantaVIS vista;
        private IucCatalogoDocumentosVIS vistaDocumentos;

        private ucCatalogoDocumentosPRE presentadorDocumentos;

        private string nombreClase = "ucLlantaPRE";
        #endregion

        #region Constructores
        public ucLlantaPRE(IucLlantaVIS view)
        {
            try
            {
                this.vista = view;

                this.presentadorDocumentos = new ucCatalogoDocumentosPRE(view.VistaDocumentos);

                this.dctx = FacadeBR.ObtenerConexion();
            }
            catch (Exception ex)
            {
                this.vista.MostrarMensaje("Inconsistencia en los parámetros de configuración", ETipoMensajeIU.ERROR, this.nombreClase + ".ucLlantaPRE" + ex.Message);
            }
        }
        public ucLlantaPRE(IucLlantaVIS view, IucCatalogoDocumentosVIS viewDocumentos)
        {
            try
            {
                this.vista = view;
                this.vistaDocumentos = viewDocumentos;

                this.presentadorDocumentos = new ucCatalogoDocumentosPRE(viewDocumentos);

                this.dctx = FacadeBR.ObtenerConexion();
            }
            catch (Exception ex)
            {
                this.vista.MostrarMensaje("Inconsistencia en los parámetros de configuración", ETipoMensajeIU.ERROR, this.nombreClase + ".ucLlantaPRE" + ex.Message);
            }
        }
        #endregion

        #region Métodos
        public void PrepararNuevo()
        {
            this.vista.PrepararNuevo();

            this.vista.HabilitarModoEdicion(true);
            this.vista.HabilitarSucursal(true);

            this.vista.MostrarDatosActualizacion(false);
            this.vista.MostrarDatosRegistro(false);
            this.vista.MostrarActiva(false);
            this.vista.MostrarEnllantable(false);
            this.vista.MostrarPosicion(false);
            this.vista.MostrarStock(false);
	        vista.Activo = true;
	        vista.Stock = true;
            this.vista.OcultarDocumentos(true);
            this.PermitirBusquedaCodigo(false, false, false);
        }
        public void PrepararEdicion()
        {
            this.vista.PrepararEdicion();
            this.vista.HabilitarModoEdicion(true);
            this.vista.HabilitarCodigo(false);
            if (this.vista.SucursalID.HasValue)
                this.vista.HabilitarSucursal(!this.vista.EnllantableID.HasValue);
            else
                this.vista.HabilitarSucursal(true);
            this.vista.MostrarDatosActualizacion(false);
            this.vista.MostrarDatosRegistro(false);
            this.vista.MostrarActiva(false);
            this.vista.MostrarEnllantable(false);
            this.vista.MostrarPosicion(false);
            this.vista.MostrarStock(false);

            this.presentadorDocumentos.ModoEditable(false);
            this.vista.OcultarDocumentos(true);
            this.PermitirBusquedaCodigo(false, false, false);
        }
        public void PrepararVisualizacion()
        {
            this.vista.PrepararVisualizacion();
            this.vista.HabilitarModoEdicion(false);
            this.vista.HabilitarSucursal(false);
            this.vista.MostrarDatosActualizacion(true);
            this.vista.MostrarDatosRegistro(true);
            this.vista.MostrarActiva(true);
            this.vista.MostrarEnllantable(true);
            this.vista.MostrarPosicion(false);
            this.vista.MostrarStock(true);

            this.presentadorDocumentos.ModoEditable(false);
            this.vista.OcultarDocumentos(false);
            this.PermitirBusquedaCodigo(false, false, false);
        }

        /// <summary>
        /// Configura las opciones de la vista
        /// </summary>
        /// <param name="datosActualizacion">Mostrar Datos de Actualización (FUA, UUA)</param>
        /// <param name="datosRegistro">Mostrar Datos de Registro (FC, UC)</param>
        /// <param name="enllantable">Mostrar la Descripción del Enllantable (Número de Serie)</param>
        /// <param name="posicion">Mostrar la Posición de la Llanta</param>
        /// <param name="documentos">Mostrar la sección de documentos</param>
        /// <param name="activa">Mostrar el Estatus de la Llanta</param>
        /// <param name="stock">Mostrar el Stock de la Llanta</param>
        /// <param name="busquedaCodigo">Permitir realizar búsqueda por Código</param>
        public void ConfigurarVista(bool datosActualizacion, bool datosRegistro, bool enllantable, bool posicion, bool documentos, bool activa, bool stock, bool busquedaCodigo, bool buscarSoloActivos, bool buscarSoloStock)
        {
            this.vista.MostrarDatosActualizacion(datosActualizacion);
            this.vista.MostrarDatosRegistro(datosRegistro);
            this.vista.MostrarEnllantable(enllantable);
            this.vista.MostrarPosicion(posicion);
            this.vista.OcultarDocumentos(!documentos);
            this.vista.MostrarActiva(activa);
            this.vista.MostrarStock(stock);
            this.vista.PermitirBusquedaCodigo(busquedaCodigo, buscarSoloActivos, buscarSoloStock);
        }

        public void HabilitarProfundidad(bool habilitar)
        {
            this.vista.HabilitarProfundidad(habilitar);
        }
        public void HabilitarPosicion(bool habilitar)
        {
            this.vista.HabilitarPosicion(habilitar);
        }

        public void PermitirBusquedaCodigo(bool permitir, bool soloActivos, bool soloStock)
        {
            this.vista.PermitirBusquedaCodigo(permitir, soloActivos, soloStock);
        }

        public void EstablecerTiposArchivo(List<TipoArchivoBO> lst)
        {
            this.presentadorDocumentos.EstablecerTiposArchivo(lst);
        }

        public string ValidarCamposRegistro()
        {
            string s = "";

			if (string.IsNullOrEmpty(vista.Codigo))
                s += "Código, ";
            if (vista.SucursalID == null)
                s += "Sucursal, ";
            if (string.IsNullOrEmpty(vista.Marca))
                s += "Marca, ";
			if (string.IsNullOrEmpty(vista.Medida))
                s += "Medida, ";
            if (string.IsNullOrEmpty(vista.Modelo ))
                s += "Modelo, ";
            if ( vista.Profundidad == null)
                s += "Profundidad, ";
            if (vista.Revitalizada == null)
                s += "Revitalizada, ";
            if (vista.Activo == null)
                s += "Activo, ";
            if (vista.Stock == null)
                s += "Stock, ";

            if (s.Trim().CompareTo("") != 0)
                return "Los siguientes campos no pueden estar vacíos: \n" + s.Substring(0, s.Length - 2);

            return null;
        }

        public void LimpiarSesion()
        {
            this.presentadorDocumentos.LimpiarSesion();
        }

        #region Métodos para el Buscador
        public object PrepararBOBuscador(string catalogo)
        {
            object obj = null;

            switch (catalogo)
            {
                case "Llanta":
                    LlantaBOF llanta = new LlantaBOF();
                    llanta.Auditoria = new AuditoriaBO();
                    llanta.MontadoEn = new EnllantableProxyBO();
                    llanta.Sucursal = new SucursalBO();
                    llanta.Sucursales = new List<SucursalBO>();
                    if (this.vista.SucursalEnllantableID != null)
                        llanta.Sucursales.Add(new SucursalBO() { Id = this.vista.SucursalEnllantableID });
                    llanta.ConsultarCompleto = true;
                    llanta.Codigo = this.vista.Codigo;                    
                    llanta.Stock = this.vista.Stock;
                    llanta.Activo = this.vista.Activo;

                    obj = llanta;
                    break;
                case "Sucursal":
                    SucursalBOF sucursal = new SucursalBOF();
                    sucursal.UnidadOperativa = new UnidadOperativaBO();
                    sucursal.Nombre = this.vista.SucursalNombre;
                    sucursal.UnidadOperativa.Id = this.vista.UnidadOperativaID;
                    sucursal.Usuario = new UsuarioBO() { Id = this.vista.UsuarioAutenticado };
                    sucursal.Activo = true;
                    obj = sucursal;
                    break;
            }

            return obj;
        }
        public void DesplegarResultadoBuscador(string catalogo, object selecto)
        {
            switch (catalogo)
            {
                case "Llanta":
                    LlantaBO llanta = (LlantaBO)selecto;

                    bool noSeleccionado = (llanta == null) ? true : false;
                    if (!noSeleccionado && this.vista.SucursalEnllantableID.HasValue && llanta.Sucursal != null && llanta.Sucursal.Id != this.vista.SucursalEnllantableID) {
                        this.vista.MostrarMensaje("La sucursal de la llanta seleccionada no pertenece a la de la unidad. Por favor verifique.", ETipoMensajeIU.ADVERTENCIA);
                    } 
                    else {
                        if (llanta == null)
                            llanta = new LlantaBO();
                        if (llanta.Auditoria == null)
                            llanta.Auditoria = new AuditoriaBO();
                        if (llanta.MontadoEn == null)
                            llanta.MontadoEn = new EnllantableProxyBO();
                        if (llanta.Sucursal == null)
                            llanta.Sucursal = new SucursalBO();

                        this.vista.SucursalID = llanta.Sucursal.Id;
                        this.vista.SucursalNombre = llanta.Sucursal.Nombre;
                        this.vista.LlantaID = llanta.LlantaID;
                        this.vista.Codigo = llanta.Codigo;                        
                        this.vista.Marca = llanta.Marca;
                        this.vista.Modelo = llanta.Modelo;
                        this.vista.Medida = llanta.Medida;
                        this.vista.Profundidad = llanta.Profundidad;
                        this.vista.Revitalizada = llanta.Revitalizada;

                        this.vista.Stock = llanta.Stock;
                        this.vista.Posicion = llanta.Posicion;
                        this.vista.EsRefaccion = llanta.EsRefaccion;

                        this.vista.FC = llanta.Auditoria.FC;
                        this.vista.UC = llanta.Auditoria.UC;
                        this.vista.FUA = llanta.Auditoria.FUA;
                        this.vista.UUA = llanta.Auditoria.UUA;
                        this.vista.Activo = llanta.Activo;

                        if (this.vista.LlantaID != null) 
                        {
                            this.vista.HabilitarModoEdicion(false);
                            this.vista.HabilitarCodigo(true);
                        } else
                            this.vista.HabilitarModoEdicion(true);
                        this.vista.HabilitarSucursal(!llanta.Sucursal.Id.HasValue);

                        if (noSeleccionado && this.vista.BuscarSoloActivos)
                            this.vista.Activo = true;
                        if (noSeleccionado && this.vista.BuscarSoloStock)
                            this.vista.Stock = true;
                    }                    
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
            }
        }
        #endregion

        #region SC_0027
        /// <summary>
        /// Verifica que el código de la vista exista o no en el sistema y, si aplica, genera uno nuevo
        /// </summary>
        /// <param name="codigoNuevo">Código generado de manera aleatoria en el caso de que el de la vista exista</param>
        /// <returns>Verdadero en caso de que el código si exista ya en el sistema, falso en caso de que no</returns>
        public bool VerificarExistenciaCodigo(out string codigoNuevo)
        {
            try
            {
                codigoNuevo = null;

                //Si la llanta ya está registrada, no hay nada que verificar
                if (this.vista.LlantaID != null)
                    return false;

                //Si el código es nulo o vacío, no hay nada que verificar
                if (string.IsNullOrEmpty(this.vista.Codigo) || string.IsNullOrWhiteSpace(this.vista.Codigo))
                    return false;

                //Se verifica la existencia del código en la BD
                List<LlantaBO> lst = new LlantaBR().Consultar(this.dctx, new LlantaBO() { Codigo = this.vista.Codigo });
                if (lst.Count <= 0)
                    return false;

                //A esta altura significa que el código si existe y debe ser cambiado
                codigoNuevo = this.GenerarCodigoAleatorio(this.vista.Codigo, 1);

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(this.nombreClase + ".ValidarExistenciaCodigo :" + ex.Message);
            }
        }
        private String GenerarCodigoAleatorio(string codigoBase, int consecutivo)
        {
            string codigo = codigoBase + consecutivo.ToString();

            List<LlantaBO> lst = new LlantaBR().Consultar(this.dctx, new LlantaBO() { Codigo = codigo });

            if (lst.Count > 0)
                return this.GenerarCodigoAleatorio(codigoBase, consecutivo + 1);
            else
                return codigo;
        }
        #endregion CS_0027
        #endregion
    }
}