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
    public class RegistrarLlantaPRE
    {
        #region Atributos
        private LlantaBR controlador;
        private IDataContext dctx = null;

        private IRegistrarLlantaVIS vista;
        private IucLlantaVIS vistaDatosLlanta;

        private ucLlantaPRE presentadorDatosLlanta;

        private string nombreClase = "RegistrarLlantaPRE";
        #endregion

        #region Constructores
        public RegistrarLlantaPRE(IRegistrarLlantaVIS view, IucLlantaVIS viewDatosLlantas)
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
                this.vista.MostrarMensaje("Inconsistencia en los parámetros de configuración", ETipoMensajeIU.ERROR, this.nombreClase + ".RegistrarLlantaPRE:" + ex.Message);
            }
        }
        #endregion

        #region Métodos
        public void PrepararNuevo()
        {
            this.presentadorDatosLlanta.PrepararNuevo();

            this.EstablecerSeguridad(); //SC_0008
        }

        #region SC_0008
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

        public void RegistrarLlanta()
        {
            try
            {
                string s;

                if ((s = this.ValidarCamposRegistro()) != null)
                {
                    this.vista.MostrarMensaje(s, ETipoMensajeIU.ADVERTENCIA, null);
                    return;
                }

                BO.LlantaBO bo = (BO.LlantaBO)this.InterfazUsuarioADato();

                #region SC_0027
                string codigoNuevo;
                if (this.presentadorDatosLlanta.VerificarExistenciaCodigo(out codigoNuevo))
                {
                    this.vista.MostrarMensaje("El código " + this.vista.Codigo + " ya se encuentra registrado y se ha generado el código " + codigoNuevo, ETipoMensajeIU.CONFIRMACION, null);
                    this.vista.Codigo = codigoNuevo;
                    return;
                }
                #endregion

                #region SC008
                this.controlador.Insertar(this.dctx, bo, new SeguridadBO(Guid.Empty, new UsuarioBO() { Id = this.vista.UsuarioID }, new AdscripcionBO() { UnidadOperativa = new UnidadOperativaBO() { Id = this.vista.UnidadOperativaID } }));
                #endregion
                List<LlantaBO> llantasBO = controlador.Consultar(dctx, bo);

                if (llantasBO.Count != 1)
                    throw new Exception("No se encontró la llanta guardada");

                bo.LlantaID = llantasBO[0].LlantaID;

                this.LimpiarSesion();
                this.vista.EstablecerPaqueteNavegacion("LlantaBO", bo);
                this.vista.MostrarMensaje("Registro Exitoso", ETipoMensajeIU.EXITO, null);
                this.vista.RedirigirADetalles();
            }
            catch (Exception ex)
            {
                throw new Exception(this.nombreClase + ".RegistrarLlanta: " + ex.Message);
            }
        }
        public void CancelarRegistro()
        {
            this.LimpiarSesion();
            this.vista.RedirigirAConsulta();
        }

        private object InterfazUsuarioADato()
        {
            LlantaBO bo = new LlantaBO();
            bo.Marca = this.vista.Marca;
            bo.Modelo = this.vista.Modelo;
            bo.Medida = this.vista.Medida;
            bo.Codigo = this.vista.Codigo;
            bo.Profundidad = this.vista.Profundidad;
            bo.Revitalizada = this.vista.Revitalizada;
            bo.Activo = this.vista.Activo;
            bo.Stock = this.vista.Stock;
            bo.Auditoria = new AuditoriaBO();
            bo.Auditoria.FC = this.vista.FC;
            bo.Auditoria.FUA = this.vista.FUA;
            bo.Auditoria.UC = this.vista.UC;
            bo.Auditoria.UUA = this.vista.UUA;
            bo.EsRefaccion = null;
            bo.Posicion = null;
            bo.MontadoEn = new EnllantableProxyBO();
            ((EnllantableProxyBO)bo.MontadoEn).EnllantableID = null;
            ((EnllantableProxyBO)bo.MontadoEn).TipoEnllantable = null;
            bo.Sucursal = new SucursalBO() { UnidadOperativa = new UnidadOperativaBO() };
            bo.Sucursal.Id = this.vista.SucursalID;
            bo.Sucursal.UnidadOperativa.Id = this.vista.UnidadOperativaID;

            return bo;
        }

        public string ValidarCamposRegistro()
        {
            string s = null;

            if ((s = this.presentadorDatosLlanta.ValidarCamposRegistro()) != null)
                return "Llantas:" + s;

            return null;
        }

        public void LimpiarSesion()
        {
            this.presentadorDatosLlanta.LimpiarSesion();
        }
        #endregion
    }
}
