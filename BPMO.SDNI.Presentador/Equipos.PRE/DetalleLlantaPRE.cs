//Satisface al CU089 - Bitácora de Llantas
using System;
using System.Collections.Generic;
using BPMO.Basicos.BO;
using BPMO.Facade.SDNI.BR;
using BPMO.Patterns.Creational.DataContext;
using BPMO.Primitivos.Enumeradores;
using BPMO.SDNI.Comun.BO;
using BPMO.SDNI.Comun.BR;
using BPMO.SDNI.Equipos.BO;
using BPMO.SDNI.Equipos.BR;
using BPMO.SDNI.Equipos.VIS;

namespace BPMO.SDNI.Equipos.PRE
{
    public class DetalleLlantaPRE
    {
        #region Atributos

        private LlantaBR controlador;
        private IDataContext dctx = null;

        private IDetalleLlantaVIS vista;
        private IucLlantaVIS vistaLlanta;

        private ucLlantaPRE presentadorLlanta;

        private string nombreClase = "DetalleLlantaPRE";

        #endregion

        #region Constructor

        public DetalleLlantaPRE(IDetalleLlantaVIS view, IucLlantaVIS viewLlanta)
        {
            try
            {
                this.vista = view;
                this.vistaLlanta = viewLlanta;

                this.presentadorLlanta = new ucLlantaPRE(viewLlanta);

                this.controlador = new LlantaBR();
                this.dctx = FacadeBR.ObtenerConexion();
            }
            catch (Exception ex)
            {
                vista.MostrarMensaje("Inconsistencias en los parámetros de configuración", ETipoMensajeIU.ERROR, nombreClase + ".DetalleLlantaPRE:" + ex.Message);
            }
        }

        #endregion

        #region Métodos

        public void PrepararVisualizacion()
        {
            presentadorLlanta.PrepararVisualizacion();
        }

        public void RealizarPrimeraCarga()
        {
            if (vista.Actualizada) { vista.Recargar(); }
            this.PrepararVisualizacion();

            this.EstablecerTiposArchivo();
            this.DatoAInterfazUsuario(this.ConsultarCompleto(this.vista.ObtenerDatosNavegacion()));
			vista.LimpiarSesion();
            this.EstablecerSeguridad(); //SC_0008
        }

        #region SC_0008
        public void ValidarAcceso()
        {
            try
            {
                //se valida que los datos del usuario y la unidad operativa no sean nulos
                if (this.vista.UsuarioID == null) throw new Exception("El identificador del usuario no debe ser nulo");
                if (this.vista.UnidadOperativaID == null) throw new Exception("El identificador de la Unidad Operativa no debe ser nulo ");

                //Se crea el objeto de seguridad
                UsuarioBO usuario = new UsuarioBO() { Id = this.vista.UsuarioID };
                AdscripcionBO adscripcion = new AdscripcionBO() { UnidadOperativa = new UnidadOperativaBO() { Id = this.vista.UnidadOperativaID } };
                SeguridadBO seguridadBO = new SeguridadBO(Guid.Empty, usuario, adscripcion);

                //Se valida si el usuario tiene permiso a la acción principal
                if (!FacadeBR.ExisteAccion(this.dctx, "CONSULTAR", seguridadBO))
                    this.vista.RedirigirSinPermisoAcceso();
            }
            catch (Exception ex)
            {
                throw new Exception(this.nombreClase + ".ValidarAcceso:" + ex.Message);
            }
        }
        private void EstablecerSeguridad()
        {
            try
            {
                //se valida que los datos del usuario y la unidad operativa no sean nulos
                if (this.vista.UsuarioID == null) throw new Exception("El identificador del usuario no debe ser nulo");
                if (this.vista.UnidadOperativaID == null) throw new Exception("El identificador de la Unidad Operativa no debe ser nulo ");

                //Se crea el objeto de seguridad
                UsuarioBO usuario = new UsuarioBO() { Id = this.vista.UsuarioID };
                AdscripcionBO adscripcion = new AdscripcionBO() { UnidadOperativa = new UnidadOperativaBO() { Id = this.vista.UnidadOperativaID } };
                SeguridadBO seguridadBO = new SeguridadBO(Guid.Empty, usuario, adscripcion);

                //Se obtienen las acciones a las cuales el usuario tiene permiso en este proceso
                List<CatalogoBaseBO> lst = FacadeBR.ConsultarAccion(this.dctx, seguridadBO);

                //Se valida si el usuario tiene permiso para dar de baja una llanta
                if (!this.ExisteAccion(lst, "UI BORRAR"))
                    this.vista.PermitirEliminar(false);
                //Se valida si el usuario tiene permiso para editar una llanta
                if (!this.ExisteAccion(lst, "ACTUALIZAR"))
                    this.vista.PermitirEditar(false);
                //Se valida si el usuario tiene permiso para registrar una llanta
                if (!this.ExisteAccion(lst, "INSERTAR"))
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

        private LlantaBO ConsultarCompleto(LlantaBO bo)
        {
            try
            {
                List<LlantaBO> lst = controlador.ConsultarCompleto(dctx, bo);

                if (lst.Count < 1)
                    throw new Exception("No se encontró ningún registro que corresponda a la información proporcionada.");
                if (lst.Count > 1)
                    throw new Exception("La consulta devolvió más de un registro.");
                return lst[0];

            }
            catch (Exception ex)
            {
                throw new Exception(nombreClase + ".ConsultarCompleto:" + ex.Message);
            }
        }

        public void Editar()
        {
            LlantaBO bo = InterfazUsuarioADato();

            vista.LimpiarSesion();

            vista.EstablecerDatosNavegacion("LlantaBO", bo);

            vista.RedirigirAEdicion();
        }

        private void DatoAInterfazUsuario(object obj)
        {
            LlantaBO bo = (LlantaBO)obj;

            if (bo.MontadoEn == null)
                bo.MontadoEn = new EnllantableProxyBO();
            if (bo.Auditoria == null)
                bo.Auditoria = new AuditoriaBO();

            vista.LlantaID = bo.LlantaID;
            vista.Codigo = bo.Codigo;
            if (bo.Sucursal != null) {
                vista.SucursalID = bo.Sucursal.Id;
                vista.SucursalNombre = bo.Sucursal.Nombre;
            }
            vista.Marca = bo.Marca;
            vista.Modelo = bo.Modelo;
            vista.Medida = bo.Medida;
            vista.Profundidad = bo.Profundidad;
            vista.Revitalizada = bo.Revitalizada;

            vista.Stock = bo.Stock;
            vista.DescripcionEnllantable = bo.MontadoEn.DescripcionEnllantable;

            vista.UC = bo.Auditoria.UC;
            vista.UUA = bo.Auditoria.UUA;
            vista.UsuarioCreacion = ObtenerNombreEmpleado(bo.Auditoria.UC);
            vista.UsuarioEdicion = ObtenerNombreEmpleado(bo.Auditoria.UUA);
            vista.FC = bo.Auditoria.FC;
            vista.FUA = bo.Auditoria.FUA;
            vista.Activo = bo.Activo;

            vista.ArchivosAdjuntos = bo.ArchivosAdjuntos;

            if (bo.Activo != null)
                vistaLlanta.OcultarDocumentos(bo.Activo.Value);
            else
                vistaLlanta.OcultarDocumentos(true);

            if (bo.Activo != null && bo.Activo == true && bo.Stock != null && bo.Stock == true)
                vista.PermitirEliminar(true);
            else
                vista.PermitirEliminar(false);

        }

        public void Eliminar()
        {
            if (vista.Stock == false)
                vista.MostrarMensaje("La llanta no se encuentra en stock", ETipoMensajeIU.ADVERTENCIA, null);
            else
                vista.RedirigirAPopUp();
        }

        private LlantaBO InterfazUsuarioADato()
        {
            BO.LlantaBO bo = new BO.LlantaBO();

            bo.MontadoEn = new EnllantableProxyBO();
            bo.Auditoria = new AuditoriaBO();

            bo.LlantaID = vista.LlantaID;

            return bo;
        }

        private string ObtenerNombreEmpleado(int? numeroEmpleado)
        {
            if (numeroEmpleado == null)
                return "";

            List<EmpleadoBO> empleadosBO = FacadeBR.ConsultarEmpleadoCompleto(FacadeBR.ObtenerConexion(), new EmpleadoBO() { Numero = numeroEmpleado });

            if (empleadosBO.Count == 0)
                return "";

            return (empleadosBO[0].NombreCompleto != null ? empleadosBO[0].NombreCompleto : "");
        }

        public void LimpiarSesion()
        {
            vista.LimpiarSesion();
            presentadorLlanta.LimpiarSesion();
        }

        #region Métodos Documentos

        /// <summary>
        /// Desplegar tipos de archivos
        /// </summary>
        private void EstablecerTiposArchivo()
        {
            var tipoBR = new TipoArchivoBR();
            var tipoBO = new TipoArchivoBO { EsImagen = false };

            presentadorLlanta.EstablecerTiposArchivo(tipoBR.Consultar(dctx, tipoBO));
        }

        #endregion

        #endregion
    }
}