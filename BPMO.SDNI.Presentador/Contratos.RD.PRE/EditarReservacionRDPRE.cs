// Satisface al CU004 - Catálogo de Reservaciones de Unidades de Renta Diaria
using System;
using System.Collections.Generic;
using System.Linq;
using BPMO.Basicos.BO;
using BPMO.Facade.SDNI.BR;
using BPMO.Patterns.Creational.DataContext;
using BPMO.Primitivos.Enumeradores;
using BPMO.SDNI.Comun.BO;
using BPMO.SDNI.Contratos.RD.BO;
using BPMO.SDNI.Contratos.RD.BOF;
using BPMO.SDNI.Contratos.RD.BR;
using BPMO.SDNI.Contratos.RD.VIS;
using BPMO.Servicio.Catalogos.BO;

namespace BPMO.SDNI.Contratos.RD.PRE
{
    public class EditarReservacionRDPRE
    {
        #region Atributos
        private IDataContext dctx = null;
        private ReservacionRDBR controlador;
        private IEditarReservacionRDVIS vista;
        private ucReservacionRDPRE presentadorDetalle;

        private string nombreClase = "EditarReservacionRDPRE";
        #endregion

        #region Constructores
        public EditarReservacionRDPRE(IEditarReservacionRDVIS view, IucReservacionRDVIS viewDetail)
        {
            try
            {
                this.vista = view;
                this.presentadorDetalle = new ucReservacionRDPRE(viewDetail);

                this.controlador = new ReservacionRDBR();
                this.dctx = FacadeBR.ObtenerConexion();
            }
            catch (Exception ex)
            {
                this.vista.MostrarMensaje("Inconsistencia en los parámetros de configuración", ETipoMensajeIU.ERROR, this.nombreClase + ".EditarReservacionRDPRE:" + ex.Message);
            }
        }
        #endregion

        #region Métodos
        public void RealizarPrimeraCarga()
        {
            try
            {
                this.EstablecerDatosNavegacion(this.vista.ObtenerPaqueteNavegacion("ReservacionRDBO"));

                this.PrepararEdicion();

                this.ConsultarCompleto();

                this.EstablecerConfiguracionInicial();
                this.EstablecerSeguridad();
            }
            catch (Exception ex)
            {
                throw new Exception(this.nombreClase + ".RealizarPrimeraCarga: " + ex.Message);
            }
        }
        private void EstablecerDatosNavegacion(object paqueteNavegacion)
        {
            try
            {
                if (paqueteNavegacion == null)
                    throw new Exception("Se esperaba un objeto en la navegación. No se puede identificar qué reservación se desea consultar.");
                if (!(paqueteNavegacion is ReservacionRDBO))
                    throw new Exception("Se esperaba una Reservación.");

                ReservacionRDBO bo = (ReservacionRDBO)paqueteNavegacion;

                this.DatoAInterfazUsuario(bo);
            }
            catch (Exception ex)
            {
                this.DatoAInterfazUsuario(new ReservacionRDBO());
                throw new Exception(this.nombreClase + ".EstablecerDatosNavegacion: " + ex.Message);
            }
        }
        private void ConsultarCompleto()
        {
            try
            {
                ReservacionRDBO bo = (ReservacionRDBO)this.InterfazUsuarioADato();

                List<ReservacionRDBO> lst = this.controlador.Consultar(this.dctx, bo, true);

                if (lst.Count < 1)
                    throw new Exception("No se encontró ningún registro que corresponda a la información proporcionada.");
                if (lst.Count > 1)
                    throw new Exception("La consulta devolvió más de un registro.");

                this.DatoAInterfazUsuario(lst[0]);
                this.vista.UltimoObjeto = lst[0];

                //Se consulta y muestra la información de la unidad
                this.presentadorDetalle.SeleccionarUnidad(new ReservacionRDBOF() { Unidad = lst[0].Unidad, ConEquiposAliados = true, ConTramites = true });
            }
            catch (Exception ex)
            {
                this.DatoAInterfazUsuario(new ReservacionRDBO());
                throw new Exception(this.nombreClase + ".ConsultarCompleto:" + ex.Message);
            }
        }
        private void EstablecerConfiguracionInicial()
        {
            this.vista.FUA = DateTime.Now;
            this.vista.UUA = this.vista.UsuarioID;
            this.presentadorDetalle.EstablecerConfiguracionInicial(this.vista.UnidadOperativaID);
        }

        public void ValidarAcceso()
        {
            try
            {
                //Valida que usuario y la unidad operativa no sean nulos
                if (this.vista.UsuarioID == null) throw new Exception("El identificador del usuario no debe ser nulo");
                if (this.vista.UnidadOperativaID == null) throw new Exception("El identificador de la Unidad Operativa no debe ser nulo");
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
                throw new Exception(this.nombreClase + ".ValidarAcceso: " + ex.Message);
            }
        }
        public void EstablecerSeguridad()
        {
            try
            {
                //Valida que el usuario y la unidad operativa no sean nulos
                if (this.vista.UsuarioID == null) throw new Exception("El identificador del usuario no debe ser nulo");
                if (this.vista.UnidadOperativaID == null)
                    throw new Exception("El identificador de la Unidad Operativa no debe ser nulo");
                // Creación del objeto seguridad
                UsuarioBO usuario = new UsuarioBO { Id = this.vista.UsuarioID };
                AdscripcionBO adscripcion = new AdscripcionBO
                {
                    UnidadOperativa = new UnidadOperativaBO { Id = this.vista.UnidadOperativaID }
                };
                SeguridadBO seguridad = new SeguridadBO(Guid.Empty, usuario, adscripcion);

                //consulta de acciones a la cual el usuario tiene permisos
                List<CatalogoBaseBO> acciones = FacadeBR.ConsultarAccion(dctx, seguridad);

                // se valida si el usuario tiene permisos para registrar
                if (!this.ExisteAccion(acciones, "INSERTARCOMPLETO"))
                    this.vista.PermitirRegistrar(false);

                //Establecer las sucursales permitidas en las vistas correspondientes
                List<SucursalBO> lstSucursales = FacadeBR.ConsultarSucursalesSeguridad(this.dctx, new SeguridadBO(Guid.Empty, new UsuarioBO() { Id = this.vista.UsuarioID }, new AdscripcionBO { UnidadOperativa = new UnidadOperativaBO() { Id = this.vista.UnidadOperativaID } }));
                if (lstSucursales != null && lstSucursales.Count > 0)
                    this.vista.SucursalesSeguridad =
                        lstSucursales.Where(p => p != null && p.Id != null).ToList().ConvertAll(s => s.Id);
            }
            catch (Exception ex)
            {

                throw new Exception(this.nombreClase + ".EstablecerSeguridad:" + ex.Message);
            }
        }
        private bool ExisteAccion(List<CatalogoBaseBO> acciones, string nombreAccion)
        {
            if (acciones != null && acciones.Count > 0 &&
                acciones.Exists(
                    p => p != null && p.Nombre != null && p.Nombre.Trim().ToUpper() == nombreAccion.Trim().ToUpper()))
                return true;

            return false;
        }

        private void PrepararEdicion()
        {
            this.vista.PrepararEdicion();
            this.presentadorDetalle.PrepararEdicion();
        }

        public void Cancelar()
        {
            #region RI0075
            this.vista.RedirigirADetalle();
            #endregion
        }
        public void Editar()
        {
            string s;
            if ((s = this.ValidarCampos()) != null)
            {
                this.vista.MostrarMensaje(s, ETipoMensajeIU.ADVERTENCIA, null);
                return;
            }

            try
            {
                ReservacionRDBO bo = (ReservacionRDBO)this.InterfazUsuarioADato();

                //Se crea el objeto de seguridad
                UsuarioBO usuario = new UsuarioBO() { Id = this.vista.UsuarioID };
                AdscripcionBO adscripcion = new AdscripcionBO() { UnidadOperativa = new UnidadOperativaBO() { Id = this.vista.UnidadOperativaID } };
                SeguridadBO seguridadBO = new SeguridadBO(Guid.Empty, usuario, adscripcion);

                this.controlador.Actualizar(this.dctx, bo, this.vista.UltimoObjeto as ReservacionRDBO, seguridadBO);

                this.vista.LimpiarSesion();
                this.vista.EstablecerPaqueteNavegacion("ReservacionRDBO", new ReservacionRDBO() { ReservacionID = bo.ReservacionID });
                this.vista.RedirigirADetalle();
            }
            catch (Exception ex)
            {
                throw new Exception(this.nombreClase + ".Editar: " + ex.Message);
            }
        }

        private object InterfazUsuarioADato()
        {
            ReservacionRDBO bo = new ReservacionRDBO();
            bo.Auditoria = new AuditoriaBO();
            bo.Cliente = new CuentaClienteIdealeaseBO();
            bo.Modelo = new ModeloBO();
            bo.Unidad = new Equipos.BO.UnidadBO();
            bo.UnidadOperativa = new UnidadOperativaBO();
            bo.UsuarioReservo = new UsuarioBO();
            bo.Sucursal = new SucursalBO();//SC_0051

            bo.Activo = this.vista.Activo;
            bo.Auditoria.FC = this.vista.FC;
            bo.Auditoria.FUA = this.vista.FUA;
            bo.Auditoria.UC = this.vista.UC;
            bo.Auditoria.UUA = this.vista.UUA;
            bo.Cliente.Id = this.vista.CuentaClienteID;
            bo.Cliente.Nombre = this.vista.CuentaClienteNombre;
            bo.FechaFinal = this.vista.FechaReservacionFinal;
            bo.FechaInicial = this.vista.FechaReservacionInicial;
            bo.Modelo.Id = this.vista.ModeloID;
            bo.Modelo.Nombre = this.vista.ModeloNombre;
            bo.Numero = this.vista.Numero;
            bo.Observaciones = this.vista.Observaciones;
            bo.ReservacionID = this.vista.ReservacionID;
            if (this.vista.TipoID != null)
                bo.Tipo = (ETipoReservacion)Enum.Parse(typeof(ETipoReservacion), this.vista.TipoID.Value.ToString());
            bo.Unidad.UnidadID = this.vista.UnidadID;
            bo.Unidad.NumeroEconomico = this.vista.NumeroEconomico;
            bo.UnidadOperativa.Id = this.vista.UnidadOperativaID;
            bo.UsuarioReservo.Id = this.vista.UsuarioReservoID;
            bo.UsuarioReservo.Nombre = this.vista.UsuarioReservoNombre;
            bo.Sucursal.Id = this.vista.SucursalID;//SC_0051
            bo.Sucursal.Nombre = this.vista.SucursalNombre;//SC_0051
            return bo;
        }
        private void DatoAInterfazUsuario(object obj)
        {
            ReservacionRDBO bo = (ReservacionRDBO)obj;
            if (bo.Auditoria == null) bo.Auditoria = new AuditoriaBO();
            if (bo.Cliente == null) bo.Cliente = new CuentaClienteIdealeaseBO();
            if (bo.Modelo == null) bo.Modelo = new ModeloBO();
            if (bo.Unidad == null) bo.Unidad = new Equipos.BO.UnidadBO();
            if (bo.UnidadOperativa == null) bo.UnidadOperativa = new UnidadOperativaBO();
            if (bo.UsuarioReservo == null) bo.UsuarioReservo = new UsuarioBO();

            this.vista.Numero = bo.Numero;
            this.vista.ReservacionID = bo.ReservacionID;
            this.vista.Activo = bo.Activo;
            this.vista.CuentaClienteID = bo.Cliente.Id;
            this.vista.CuentaClienteNombre = bo.Cliente.Nombre;
            this.vista.FC = bo.Auditoria.FC;
            this.vista.FechaReservacionFinal = bo.FechaFinal;
            this.vista.FechaReservacionInicial = bo.FechaInicial;
            this.vista.FUA = bo.Auditoria.FUA;
            this.vista.ModeloID = bo.Modelo.Id;
            this.vista.ModeloNombre = bo.Modelo.Nombre;
            this.vista.NumeroEconomico = bo.Unidad.NumeroEconomico;
            this.vista.Observaciones = bo.Observaciones;
            if (bo.Tipo != null)
                this.vista.TipoID = (int)bo.Tipo;
            else
                this.vista.TipoID = null;
            this.vista.UC = bo.Auditoria.UC;
            this.vista.UnidadID = bo.Unidad.UnidadID;
            this.vista.UsuarioReservoID = bo.UsuarioReservo.Id;
            this.vista.UsuarioReservoNombre = bo.UsuarioReservo.Nombre;
            this.vista.UUA = bo.Auditoria.UUA;

            if (bo.Sucursal == null)
                bo.Sucursal = new SucursalBO();

            this.vista.AnioUnidad = bo.Unidad != null ? (bo.Unidad.Anio.HasValue ? bo.Unidad.Anio : null) : null;//SC_0051
            this.vista.SucursalID = bo.Sucursal.Id;//SC_0051
            this.vista.SucursalNombre = bo.Sucursal.Nombre;//SC_0051
        }

        private string ValidarCampos()
        {
            string s = "";

            if ((s = this.presentadorDetalle.ValidarCampos()) != null)
                return s;

            return null;
        }
        #endregion
    }
}