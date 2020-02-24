// Satisface al CU004 - Catálogo de Reservaciones de Unidades de Renta Diaria
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using BPMO.Basicos.BO;
using BPMO.Facade.SDNI.BR;
using BPMO.Patterns.Creational.DataContext;
using BPMO.Primitivos.Enumeradores;
using BPMO.SDNI.Comun.BO;
using BPMO.SDNI.Contratos.RD.BO;
using BPMO.SDNI.Contratos.RD.BR;
using BPMO.SDNI.Contratos.RD.VIS;
using BPMO.Servicio.Catalogos.BO;

namespace BPMO.SDNI.Contratos.RD.PRE
{
    public class RegistrarReservacionRDPRE
    {
        #region Atributos
        private IDataContext dctx = null;
        private ReservacionRDBR controlador;
        private IRegistrarReservacionRDVIS vista;
        private ucReservacionRDPRE presentadorDetalle;

        private string nombreClase = "RegistrarReservacionRDPRE";
        #endregion

        #region Constructores
        public RegistrarReservacionRDPRE(IRegistrarReservacionRDVIS view, IucReservacionRDVIS viewDetail)
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
                this.vista.MostrarMensaje("Inconsistencia en los parámetros de configuración", ETipoMensajeIU.ERROR, this.nombreClase + ".RegistrarReservacionRDPRE:" + ex.Message);
            }
        }
        #endregion

        #region Métodos
        public void PrepararNuevo()
        {
            this.LimpiarSesion();

            this.vista.PrepararNuevo();
            this.presentadorDetalle.PrepararNuevo();

            this.EstablecerConfiguracionInicial();
            this.EstablecerSeguridad();
        }
        private void EstablecerConfiguracionInicial()
        {
            this.vista.FC = DateTime.Now;
            this.vista.FUA = DateTime.Now;
            this.vista.UC = this.vista.UsuarioID;
            this.vista.UUA = this.vista.UsuarioID;
            this.vista.UsuarioReservoID = this.vista.UsuarioID;
            this.vista.Activo = true;
            this.presentadorDetalle.EstablecerConfiguracionInicial(this.vista.UnidadOperativaID);
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

                //Se valida si el usuario tiene permisos para registrar un nuevo acta de nacimiento
                if (!this.ExisteAccion(acciones, "INSERTARCOMPLETO"))
                    this.vista.RedirigirSinPermisoAcceso();

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

            this.vista.Numero = bo.Numero;
            this.vista.ReservacionID = bo.ReservacionID;
        }

        private string ValidarCampos()
        {
            string s = "";

            if ((s = this.presentadorDetalle.ValidarCampos()) != null)
                return s;

            return null;
        }

        public void Registrar()
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

                this.controlador.InsertarCompleto(this.dctx, bo, seguridadBO);

                //Se consulta lo insertado para recuperar los ID
                DataSet ds = this.controlador.ConsultarSet(this.dctx, bo);
                if (ds.Tables[0].Rows.Count <= 0)
                    throw new Exception("Al consultar lo insertado no se encontraron coincidencias.");
                if (ds.Tables[0].Rows.Count > 1)
                    throw new Exception("Al consultar lo insertado se encontró más de una coincidencia.");

                bo.ReservacionID = this.controlador.DataRowToReservacionRDBO(ds.Tables[0].Rows[0]).ReservacionID;
                bo.Numero = this.controlador.DataRowToReservacionRDBO(ds.Tables[0].Rows[0]).Numero;

                //Se obtiene el nombre del usuario que reservó
                if (bo.UsuarioReservo != null && bo.UsuarioReservo.Id != null)
                {
                    List<UsuarioBO> lst = FacadeBR.ConsultarUsuario(this.dctx, bo.UsuarioReservo);
                    if (lst.Count > 0)
                        bo.UsuarioReservo = lst[0];
                }

                //Se agrega a la lista de registradas
                List<ReservacionRDBO> lstReservacionesRealizadas = new List<ReservacionRDBO>();
                if (this.vista.ReservacionesRealizadas != null)
                    lstReservacionesRealizadas = (List<ReservacionRDBO>)this.vista.ReservacionesRealizadas;
                lstReservacionesRealizadas.Add(bo);
                this.vista.ReservacionesRealizadas = lstReservacionesRealizadas;

                //Se reinicia el formulario
                this.presentadorDetalle.PrepararNuevo();
                this.EstablecerConfiguracionInicial();
            }
            catch (Exception ex)
            {
                throw new Exception(this.nombreClase + ".Registrar: " + ex.Message);
            }
        }
        public void Cancelar()
        {
            this.LimpiarSesion();
            this.vista.RedirigirAConsulta();
        }

        private void LimpiarSesion()
        {
            this.vista.LimpiarSesion();
            this.presentadorDetalle.LimpiarSesion();
        }
        #endregion
    }
}
