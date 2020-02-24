//Satisface al CU089 - Bitácora de Llantas
using System;
using System.Collections.Generic;
using BPMO.Basicos.BO;
using BPMO.Facade.SDNI.BR;
using BPMO.Patterns.Creational.DataContext;
using BPMO.Primitivos.Enumeradores;
using BPMO.SDNI.Equipos.BO;
using BPMO.SDNI.Equipos.BR;
using BPMO.SDNI.Equipos.VIS;

namespace BPMO.SDNI.Equipos.PRE
{
    public class EditarLlantaPRE
    {
        #region Atributos
        private LlantaBR controlador;
        private IDataContext dctx = null;

        private IEditarLlantaVIS vista;
        private IucLlantaVIS vistaDatosLlanta;

        private ucLlantaPRE presentadorDatosLlanta;

        private string nombreClase = "EditarLlantaPRE";
        #endregion

        #region Constructor
        public EditarLlantaPRE(IEditarLlantaVIS view, IucLlantaVIS viewDatosLlantas)
        {
            try
            {
                this.vista = view;
                this.vistaDatosLlanta = viewDatosLlantas;

                this.presentadorDatosLlanta = new ucLlantaPRE(viewDatosLlantas);

                this.controlador = new LlantaBR();
                this.dctx = FacadeBR.ObtenerConexion();
            }
            catch (Exception ex)
            {
                this.vista.MostrarMensaje("Inconsistencia en los parámetros de configuración", ETipoMensajeIU.ERROR, this.nombreClase + ".EditarLlantaPRE:" + ex.Message);
            }
        }
        #endregion

        #region Métodos
        public void PreparaEdicion()
        {
            this.vista.PrepararEdicion();
            this.presentadorDatosLlanta.PrepararEdicion();
        }

        public void RealizarPrimeraCarga()
        {
            this.EstablecerDatosNavegacion(this.vista.DatosNavegacion);

            this.Consultar();

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
                if (!FacadeBR.ExisteAccion(this.dctx, "ACTUALIZAR", seguridadBO))
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

        private void EstablecerDatosNavegacion(object paqueteNavegacion)
        {
            if (paqueteNavegacion == null)
                throw new Exception(this.nombreClase + ".EstablecerDatosNavegacion: Se esperaba un objeto en la navegación. No se puede identificar qué llanta se desea consultar.");
            if (!(paqueteNavegacion is BO.LlantaBO))
                throw new Exception(this.nombreClase + ".EstablecerDatosNavegacion: Se esperaba una Llanta.");

            BO.LlantaBO bo = (BO.LlantaBO)paqueteNavegacion;

            this.DatoAInterfazUsuario(bo);
        }

        public void EditarLlanta()
        {
            try
            {
                string s;

                if ((s = this.ValidarCamposEdicion()) != null)
                {
                    this.vista.MostrarMensaje(s, ETipoMensajeIU.ADVERTENCIA, null);
                    return;
                }

                LlantaBO bo = (LlantaBO)InterfazUsuarioADato();

				//Actualizar BD
				this.controlador.Actualizar(dctx, bo, vista.UltimoObjetoLlanta, new SeguridadBO(Guid.Empty, new UsuarioBO() { Id = this.vista.UsuarioID }, new AdscripcionBO() { UnidadOperativa = new UnidadOperativaBO() { Id = this.vista.UnidadOperativaID } })); //SC_0008
				
				vista.MostrarMensaje("Edición Exitosa", ETipoMensajeIU.EXITO, null);

                //Mostrar datos en pantalla

                this.vista.UltimoObjetoLlanta = bo;

                this.LimpiarSesion();
                this.vista.DatosNavegacion = new LlantaBO { LlantaID = bo.LlantaID };
                this.vista.RedirigirADetalle();
            }
            catch (Exception ex)
            {
                throw new Exception(this.nombreClase + ".Editar:" + ex.Message);
            }
        }

        public void Consultar()
        {
            try
            {
                LlantaBO bo = new LlantaBO() { LlantaID = this.vista.LlantaID };

                List<BO.LlantaBO> lst = controlador.ConsultarCompleto(dctx, bo);

                if (lst.Count < 1)
                    throw new Exception("No se encontró ningún registro que corresponda a la información proporcionada.");
                if (lst.Count > 1)
                    throw new Exception("La consulta devolvió más de un registro.");

                this.DatoAInterfazUsuario(lst[0]);

                this.vista.UltimoObjetoLlanta = lst[0];
            }
            catch (Exception ex)
            {
                throw new Exception(this.nombreClase + ".Consultar:" + ex.Message);
            }
        }

        public void CancelarEdicion()
        {
            this.LimpiarSesion();
            this.vista.RedirigirAConsulta();
        }
        public void RedirigirAConsulta()
        {
            this.LimpiarSesion();
            this.vista.RedirigirAConsulta();
        }

        private void DatoAInterfazUsuario(LlantaBO bo)
        {
            if (bo.Auditoria == null)
                bo.Auditoria = new AuditoriaBO();
            if (bo.MontadoEn == null)
                bo.MontadoEn = new EnllantableProxyBO();

            this.vista.LlantaID = bo.LlantaID;
            if (bo.Sucursal != null) {
                this.vista.SucursalID = bo.Sucursal.Id;
                this.vista.SucursalNombre = bo.Sucursal.Nombre;
            }
            this.vista.Marca = bo.Marca;
            this.vista.Modelo = bo.Modelo;
            this.vista.Medida = bo.Medida;
            this.vista.Codigo = bo.Codigo;
            this.vista.Profundidad = bo.Profundidad;
            this.vista.Revitalizada = bo.Revitalizada;
            this.vista.UC = bo.Auditoria.UC;
            this.vista.FC = bo.Auditoria.FC;
            this.vista.Stock = bo.Stock;
            this.vista.Activo = bo.Activo;

            if (bo.Stock != null)
                this.presentadorDatosLlanta.HabilitarProfundidad(bo.Stock.Value);
            else
                this.presentadorDatosLlanta.HabilitarProfundidad(true);

            this.vista.Posicion = bo.Posicion;
            this.vista.EsRefaccion = bo.EsRefaccion;
            this.vista.EnllantableID = bo.MontadoEn.EnllantableID;
            if (bo.MontadoEn.TipoEnllantable != null)
                this.vista.TipoEnllantable = (int)bo.MontadoEn.TipoEnllantable;
            else
                this.vista.TipoEnllantable = null;
        }
        private object InterfazUsuarioADato()
        {
            LlantaBO bo = new LlantaBO();
            bo.Auditoria = new AuditoriaBO();
            bo.MontadoEn = new EnllantableProxyBO();
            bo.Sucursal = new SucursalBO();
            bo.Sucursal.Id = this.vista.SucursalID;
            bo.Sucursal.UnidadOperativa = new UnidadOperativaBO() { Id = this.vista.UnidadOperativaID };

            bo.LlantaID = this.vista.LlantaID;            
            bo.Codigo = this.vista.Codigo;
            bo.Marca = this.vista.Marca;
            bo.Modelo = this.vista.Modelo;
            bo.Medida = this.vista.Medida;
            bo.Profundidad = this.vista.Profundidad;
            bo.Revitalizada = this.vista.Revitalizada;
            bo.Auditoria.FUA = this.vista.FUA;
            bo.Auditoria.UUA = this.vista.UUA;
            bo.Auditoria.FC = this.vista.FC;
            bo.Auditoria.UC = this.vista.UC;
            bo.Stock = this.vista.Stock;
            bo.Activo = this.vista.Activo;
            bo.Posicion = this.vista.Posicion;
            bo.EsRefaccion = this.vista.EsRefaccion;
            ((EnllantableProxyBO)bo.MontadoEn).EnllantableID = this.vista.EnllantableID;
            if (this.vista.TipoEnllantable != null)
                ((EnllantableProxyBO)bo.MontadoEn).TipoEnllantable = (ETipoEnllantable)Enum.Parse(typeof(ETipoEnllantable), this.vista.TipoEnllantable.ToString());                

            return bo;
        }

        public string ValidarCamposEdicion()
        {
            string s = null;

            if ((s = this.presentadorDatosLlanta.ValidarCamposRegistro()) != null)
                return "Llantas:" + s;

            return null;
        }

        public void LimpiarSesion()
        {
            this.vista.LimpiarSesion();
            this.presentadorDatosLlanta.LimpiarSesion();
        }
        #endregion
    }
}
