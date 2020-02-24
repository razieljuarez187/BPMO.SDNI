//Satisface al CU082 - Registrar Movimiento de Flota
using System;
using System.Collections.Generic;
using BPMO.Basicos.BO;
using BPMO.Facade.SDNI.BOF;
using BPMO.Facade.SDNI.BR;
using BPMO.Patterns.Creational.DataContext;
using BPMO.Primitivos.Enumeradores;
using BPMO.SDNI.Equipos.BO;
using BPMO.SDNI.Equipos.BOF;
using BPMO.SDNI.Equipos.BR;
using BPMO.SDNI.Flota.BR;
using BPMO.SDNI.Flota.VIS;
using BPMO.Servicio.Catalogos.BO;

namespace BPMO.SDNI.Flota.PRE
{
    /// <summary>
    /// Presentador para el cambio de sucursal del equipo aliado
    /// </summary>
    public class CambiarSucursalEquipoAliadoPRE
    {
        #region Atributos
        /// <summary>
        /// Vista sobre la que actua el presentador
        /// </summary>
        private readonly ICambiarSucursalEquipoAliadoVIS vista;
        /// <summary>
        /// El DataContext que proveerá acceso a la base de datos
        /// </summary>
        private readonly IDataContext dctx;
        /// <summary>
        /// Nombre de la clase para agregar a los mensajes de Error
        /// </summary>
        private string nombreClase = "CambiarSucursalEquipoAliadoPRE";
        /// <summary>
        /// Controlador de equipos aliados
        /// </summary>
        private MantenimientoFlotaBR controlador;
        #endregion

        #region Constructores
        public CambiarSucursalEquipoAliadoPRE(ICambiarSucursalEquipoAliadoVIS view)
        {
            try
            {
                this.vista = view;

                this.controlador = new MantenimientoFlotaBR();
                this.dctx = FacadeBR.ObtenerConexion();
            }
            catch (Exception ex)
            {
                this.vista.MostrarMensaje("Inconsistencia en los parámetros de configuración", ETipoMensajeIU.ERROR, this.nombreClase + ".CambiarSucursalEquipoAliadoPRE:" + ex.Message);
            }
        }
        #endregion

        #region Métodos
        /// <summary>
        /// Realiza la primera carga para la reasignación de sucursal para el equipo aliado
        /// </summary>
        public void RealizarPrimeraCarga()
        {
            try
            {
                this.vista.PrepararVista();

                this.EstablecerSeguridad();
            }
            catch (Exception ex)
            {
                throw new Exception(this.nombreClase + ".RealizarPrimeraCarga: " + ex.Message);
            }
        }
        /// <summary>
        /// Obtiene la inforamción proporcionada por el usuario y la transforma en un objeto de negocio
        /// </summary>
        /// <returns>Objeto de negocio</returns>
        private object InterfazUsuarioADato()
        {
            EquipoAliadoBO bo = new EquipoAliadoBO();
            if (this.vista.UltimoObjeto != null)
                bo = new EquipoAliadoBO((EquipoAliadoBO)this.vista.UltimoObjeto);
            if (bo.Sucursal == null) bo.Sucursal = new SucursalBO();
            if (bo.Sucursal.UnidadOperativa == null) bo.Sucursal.UnidadOperativa = new UnidadOperativaBO();

            bo.Sucursal.Id = this.vista.SucursalDestinoID.HasValue ? (int?)this.vista.SucursalDestinoID.Value : null;
            bo.Sucursal.Nombre = !string.IsNullOrEmpty(this.vista.SucursalDestinoNombre) &&
                                 !string.IsNullOrWhiteSpace(this.vista.SucursalDestinoNombre)
                ? this.vista.SucursalDestinoNombre
                : string.Empty;
            bo.UUA = this.vista.UsuarioID;
            bo.FUA = DateTime.Now;

            return bo;
        }
        /// <summary>
        /// Valida el acceso al cambio de sucursal para el equipo aliado
        /// </summary>
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
                if (!FacadeBR.ExisteAccion(dctx, "ACTUALIZAREQUIPOALIADOSUCURSAL", seguridadBO))
                    this.vista.RedirigirSinPermisoAcceso();
            }
            catch (Exception ex)
            {
                throw new Exception(nombreClase + ".ValidarAcceso:" + ex.Message);
            }
        }
        /// <summary>
        /// Establece la seguridad para la vista de reasignación
        /// </summary>
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
                List<CatalogoBaseBO> lst = FacadeBR.ConsultarAccion(dctx, seguridad);

                //Se valida si el usuario tiene permiso para ver el detalle del acta de nacimiento
                if (!this.ExisteAccion(lst, "ACTUALIZAREQUIPOALIADOSUCURSAL"))
                    this.vista.PermitirRegistrar(false);
                //Se valida si el usuario tiene permiso para ver el detalle del contrato
                if (!this.ExisteAccion(lst, "UI CONSULTAR"))
                    this.vista.PermitirConsultar(false);
            }
            catch (Exception ex)
            {

                throw new Exception(this.nombreClase + ".EstablecerSeguridad:" + ex.Message);
            }
        }
        /// <summary>
        /// Evalua si existe una accion especifica dentro del listado de acciones configuradas
        /// </summary>
        /// <param name="acciones">Lista de acciones configuradas apra el usuario autenticado en el sistema</param>
        /// <param name="nombreAccion">Acción que se desea evaluar</param>
        /// <returns></returns>
        private bool ExisteAccion(List<CatalogoBaseBO> acciones, string nombreAccion)
        {
            if (acciones != null && acciones.Count > 0 &&
                acciones.Exists(
                    p => p != null && p.Nombre != null && p.Nombre.Trim().ToUpper() == nombreAccion.Trim().ToUpper()))
                return true;

            return false;
        }
        /// <summary>
        /// Cancela la reasignación de la sucursal
        /// </summary>
        public void Cancelar()
        {
            this.vista.RedirigirAConsultarSeguimiento();
        }
        /// <summary>
        /// Cmabia de sucursal el equipo aliado
        /// </summary>
        public void CambiarSucursal()
        {
            string s;
            if ((s = this.ValidarCamposRegistro()) != null)
            {
                this.vista.MostrarMensaje(s, ETipoMensajeIU.ADVERTENCIA, null);
                return;
            }

            //Editamos la el equipo aliado
            this.Editar();
            this.vista.RedirigirAConsultarSeguimiento();
        }
        /// <summary>
        /// Edita el equipo aliado
        /// </summary>
        private void Editar()
        {
            //Se obtiene la información del contrato a partir de la vista
            var bo = (EquipoAliadoBO)this.InterfazUsuarioADato();

            //Se crea el objeto de seguridad
            UsuarioBO usuario = new UsuarioBO() { Id = this.vista.UsuarioID };
            AdscripcionBO adscripcion = new AdscripcionBO() { UnidadOperativa = new UnidadOperativaBO() { Id = this.vista.UnidadOperativaID } };
            SeguridadBO seguridadBO = new SeguridadBO(Guid.Empty, usuario, adscripcion);

            var temp = new EquipoAliadoBO
            {
                EquipoAliadoID = bo.EquipoAliadoID,
            };
            EquipoAliadoBR equipoAliadoBR = new EquipoAliadoBR();
            var elementos = equipoAliadoBR.Consultar(dctx, temp);
            if (elementos != null && elementos.Count > 0)
                temp = elementos[0];


            this.controlador.ActualizarEquipoAliadoSucursal(dctx, bo, temp, seguridadBO);
        }
        /// <summary>
        /// Valida los campos requeridos para el registro de la reasignación de sucursal
        /// </summary>
        /// <returns></returns>
        private string ValidarCamposRegistro()
        {
            string s = string.Empty;

            if (!this.vista.EquipoAliadoID.HasValue)
                s += "Identificador del Equipo Aliado, ";
            if (!this.vista.SucursalDestinoID.HasValue)
                s += "Identificador Sucursal Destino, ";
            if (this.vista.NumeroSerie == null)
                s += "Número de Serie, ";
            if (this.vista.SucursalDestinoNombre == null)
                s += "Nombre de Sucursal Destino, ";

            if (s != null && s.Trim().CompareTo("") != 0)
                return "Los siguientes campos no pueden estar vacíos: \n" + s.Substring(0, s.Length - 2);

            if (this.vista.SucursalActualID == this.vista.SucursalDestinoID)
                return "La Sucursal seleccionada como destino, corresponde a la sucursal actual, intente de nuevo por favor.";

            return null;
        }

        #region Métodos para el Buscador
        public void SeleccionarSucursal(SucursalBO sucursal)
        {
            #region Instancia de propiedades
            if (sucursal == null) sucursal = new SucursalBO();
            if (sucursal.UnidadOperativa == null) sucursal.UnidadOperativa = new UnidadOperativaBO();
            if (sucursal.UnidadOperativa.Empresa == null) sucursal.UnidadOperativa.Empresa = new EmpresaBO();
            #endregion

            #region Dato a Interfaz de Usuario
            if (sucursal != null && sucursal.Nombre != null)
                this.vista.SucursalDestinoNombre = sucursal.Nombre;
            else
                this.vista.SucursalDestinoNombre = null;

            if (sucursal != null && sucursal.Id != null)
                this.vista.SucursalDestinoID = sucursal.Id;
            else
                this.vista.SucursalDestinoID = null;

            if (sucursal.UnidadOperativa != null)
            {
                if (sucursal.UnidadOperativa.Empresa != null)
                {
                    if (!string.IsNullOrEmpty(sucursal.UnidadOperativa.Empresa.Nombre) &&
                        !string.IsNullOrWhiteSpace(sucursal.UnidadOperativa.Empresa.Nombre))
                        this.vista.NombreEmpresaDestino = sucursal.UnidadOperativa.Empresa.Nombre.Trim().ToUpper();
                    else this.vista.NombreEmpresaDestino = string.Empty;
                }
            }

            #endregion

            #region Consultar Completo para obtener la Dirección
            var empresa = new EmpresaBO();
            if (sucursal != null && sucursal.Id != null)
            {
                List<SucursalBO> lst = FacadeBR.ConsultarSucursalCompleto(this.dctx, sucursal);

                #region Empresa
                if (lst.Count > 0)
                {
                    var sucTemp = (SucursalBO)lst[0];
                    if (sucTemp.UnidadOperativa != null)
                    {
                        if (sucTemp.UnidadOperativa.Empresa != null)
                        {
                            if (!string.IsNullOrEmpty(sucTemp.UnidadOperativa.Empresa.Nombre) && !string.IsNullOrWhiteSpace(sucTemp.UnidadOperativa.Empresa.Nombre))
                                this.vista.NombreEmpresaDestino = sucTemp.UnidadOperativa.Empresa.Nombre.Trim().ToUpper();
                            else
                            {
                                #region Unidad Operativa
                                //Obtener información de la Unidad Operativa
                                List<UnidadOperativaBO> lstUO = FacadeBR.ConsultarUnidadOperativaCompleto(this.dctx,
                                                                                                          new UnidadOperativaBO()
                                                                                                          {
                                                                                                              Id =
                                                                                                                  this.vista
                                                                                                                      .UnidadOperativaID
                                                                                                          });
                                if (lstUO.Count <= 0)
                                    throw new Exception(
                                        "No se encontró la información completa de la unidad operativa sobre la que trabaja.");
                                //Establecer la información de la Unidad Operativa
                                if (lstUO[0].Empresa != null)
                                    empresa = lstUO[0].Empresa;
                                #endregion
                                this.vista.NombreEmpresaDestino = empresa.Nombre;
                            }
                        }
                        else
                        {
                            #region Unidad Operativa
                            //Obtener información de la Unidad Operativa
                            List<UnidadOperativaBO> lstUO = FacadeBR.ConsultarUnidadOperativaCompleto(this.dctx,
                                                                                                      new UnidadOperativaBO()
                                                                                                      {
                                                                                                          Id =
                                                                                                              this.vista
                                                                                                                  .UnidadOperativaID
                                                                                                      });
                            if (lstUO.Count <= 0)
                                throw new Exception(
                                    "No se encontró la información completa de la unidad operativa sobre la que trabaja.");
                            //Establecer la información de la Unidad Operativa
                            if (lstUO[0].Empresa != null)
                                empresa = lstUO[0].Empresa;
                            #endregion
                            this.vista.NombreEmpresaDestino = empresa.Nombre;
                        }
                    }
                }
                #endregion

                #region Direcciones
                DireccionSucursalBO direccion = null;
                if (lst.Count > 0 && lst[0].DireccionesSucursal != null)
                    direccion = lst[0].DireccionesSucursal.Find(p => p.Primaria != null && p.Primaria == true);

                if (direccion != null)
                {
                    string dir = "";
                    if (!string.IsNullOrEmpty(direccion.Calle))
                        dir += (direccion.Calle + " ");
                    if (!string.IsNullOrEmpty(direccion.Colonia))
                        dir += (direccion.Colonia + " ");
                    if (!string.IsNullOrEmpty(direccion.CodigoPostal))
                        dir += (direccion.CodigoPostal + " ");
                    if (direccion.Ubicacion != null)
                    {
                        if (direccion.Ubicacion.Municipio != null && !string.IsNullOrEmpty(direccion.Ubicacion.Municipio.Nombre))
                            dir += (direccion.Ubicacion.Municipio.Nombre + " ");
                        if (direccion.Ubicacion.Ciudad != null && !string.IsNullOrEmpty(direccion.Ubicacion.Ciudad.Nombre))
                            dir += (direccion.Ubicacion.Ciudad.Nombre + " ");
                        if (direccion.Ubicacion.Estado != null && !string.IsNullOrEmpty(direccion.Ubicacion.Estado.Nombre))
                            dir += (direccion.Ubicacion.Estado.Nombre + " ");
                        if (direccion.Ubicacion.Pais != null && !string.IsNullOrEmpty(direccion.Ubicacion.Pais.Nombre))
                            dir += (direccion.Ubicacion.Pais.Nombre + " ");
                    }

                    if (dir != null && dir.Trim().CompareTo("") != 0)
                        this.vista.DomicilioSucursalDestino = dir;
                    else
                        this.vista.DomicilioSucursalDestino = null;
                }
                else
                    this.vista.DomicilioSucursalDestino = null;
                #endregion
            }
            else
                this.vista.DomicilioSucursalDestino = null;
            #endregion
        }
        public object PrepararBOBuscador(string catalogo)
        {
            object obj = null;

            switch (catalogo)
            {
                case "Sucursal":
                    SucursalBOF sucursal = new SucursalBOF();
                    sucursal.UnidadOperativa = new UnidadOperativaBO();
                    sucursal.Usuario = new UsuarioBO();

                    sucursal.UnidadOperativa.Id = this.vista.UnidadOperativaID;
                    sucursal.Nombre = this.vista.SucursalDestinoNombre;
                    sucursal.Usuario.Id = this.vista.UsuarioID;
                    sucursal.Activo = true;

                    obj = sucursal;
                    break;

                case "EquipoAliado":
                    EquipoAliadoBOF ea = new EquipoAliadoBOF();
                    ea.Sucursal = new SucursalBO();
                    ea.Sucursal.UnidadOperativa = new UnidadOperativaBO();
                    ea.Sucursal.UnidadOperativa.Empresa = new EmpresaBO();

                    ea.NumeroSerie = this.vista.NumeroSerie;
                    ea.Sucursal.UnidadOperativa.Id = this.vista.UnidadOperativaID;
                    ea.Estatus = EEstatusEquipoAliado.SinAsignar;

                    obj = ea;
                    break;
            }

            return obj;
        }
        public void DesplegarResultadoBuscador(string catalogo, object selecto)
        {
            switch (catalogo)
            {
                case "Sucursal":
                    this.SeleccionarSucursal((SucursalBO)selecto);
                    break;
                case "EquipoAliado":
                    #region Desplegar Equipo Aliado
                    EquipoAliadoBOF ea = (EquipoAliadoBOF)selecto;
                    if (ea == null) ea = new EquipoAliadoBOF();
                    if (ea.Modelo == null) ea.Modelo = new ModeloBO();
                    if (ea.Modelo.Marca == null) ea.Modelo.Marca = new MarcaBO();
                    if (ea.Sucursal == null) ea.Sucursal = new SucursalBO();
                    if (ea.Sucursal.UnidadOperativa == null) ea.Sucursal.UnidadOperativa = new UnidadOperativaBO();
                    if (ea.TipoEquipoServicio == null) ea.TipoEquipoServicio = new TipoUnidadBO();

                    //Información del equipo aliado
                    this.vista.AnioModelo = ea.Anio.ToString();
                    this.vista.OracleID = ea.ClaveActivoOracle;
                    this.vista.EquipoAliadoID = ea.EquipoAliadoID;
                    this.vista.EquipoID = ea.EquipoID;
                    this.vista.EstatusNombre = ea.Estatus.ToString();
                    this.vista.EstatusID = (int?)ea.Estatus;
                    this.vista.HorasIniciales = ea.HorasIniciales;
                    this.vista.EquipoLiderID = ea.IDLider;
                    this.vista.ModeloID = ea.Modelo.Id;
                    this.vista.Modelo = ea.Modelo.Nombre;
                    this.vista.PBC = ea.PBC;
                    this.vista.PBV = ea.PBV;
                    this.vista.Marca = ea.Modelo.Marca.Nombre;
                    this.vista.NumeroSerie = ea.NumeroSerie;
                    this.vista.TipoEquipoID = ea.TipoEquipoServicio.Id;
                    this.vista.TipoEquipoNombre = ea.TipoEquipoServicio.Nombre;

                    //Información de la sucursal Actual
                    bool completa = false;
                    this.vista.SucursalActualID = ea.Sucursal.Id;
                    this.vista.SucursalActualNombre = ea.Sucursal.Nombre;

                    #region Empresa
                    var empresa = new EmpresaBO();
                    if (ea.Sucursal.UnidadOperativa != null)
                    {
                        this.vista.EmpresaActualID = ea.Sucursal.UnidadOperativa.Id;


                        if (ea.Sucursal.UnidadOperativa.Empresa == null)
                        {
                            #region Unidad Operativa
                            //Obtener información de la Unidad Operativa
                            List<UnidadOperativaBO> lstUO = FacadeBR.ConsultarUnidadOperativaCompleto(this.dctx,
                                                                                                      new UnidadOperativaBO()
                                                                                                      {
                                                                                                          Id =
                                                                                                              this.vista
                                                                                                                  .UnidadOperativaID
                                                                                                      });
                            if (lstUO.Count <= 0)
                                throw new Exception(
                                    "No se encontró la información completa de la unidad operativa sobre la que trabaja.");
                            //Establecer la información de la Unidad Operativa
                            if (lstUO[0].Empresa != null)
                                empresa = lstUO[0].Empresa;
                            #endregion

                            this.vista.NombreEmpresaActual = empresa.Nombre;
                        }
                        else
                        {
                            if (!string.IsNullOrEmpty(ea.Sucursal.UnidadOperativa.Empresa.Nombre) && !string.IsNullOrWhiteSpace(ea.Sucursal.UnidadOperativa.Empresa.Nombre))
                                this.vista.NombreEmpresaActual = ea.Sucursal.UnidadOperativa.Empresa.Nombre;
                            else
                            {
                                #region Unidad Operativa
                                //Obtener información de la Unidad Operativa
                                List<UnidadOperativaBO> lstUO = FacadeBR.ConsultarUnidadOperativaCompleto(this.dctx,
                                                                                                          new UnidadOperativaBO()
                                                                                                          {
                                                                                                              Id =
                                                                                                                  this.vista
                                                                                                                      .UnidadOperativaID
                                                                                                          });
                                if (lstUO.Count <= 0)
                                    throw new Exception(
                                        "No se encontró la información completa de la unidad operativa sobre la que trabaja.");
                                //Establecer la información de la Unidad Operativa
                                if (lstUO[0].Empresa != null)
                                    empresa = lstUO[0].Empresa;

                                #endregion
                                this.vista.NombreEmpresaActual = empresa.Nombre;
                            }
                        }
                    }
                    #endregion

                    #region Domicilio Sucursal
                    if (ea.Sucursal.DireccionesSucursal != null)
                    {
                        if (ea.Sucursal.DireccionesSucursal.Count > 0)
                        {
                            var direccionActual = ea.Sucursal.DireccionesSucursal.Find(p => p.Primaria != null && p.Primaria == true);
                            if (direccionActual != null)
                            {
                                string dir = "";
                                if (!string.IsNullOrEmpty(direccionActual.Calle))
                                    dir += (direccionActual.Calle + " ");
                                if (!string.IsNullOrEmpty(direccionActual.Colonia))
                                    dir += (direccionActual.Colonia + " ");
                                if (!string.IsNullOrEmpty(direccionActual.CodigoPostal))
                                    dir += (direccionActual.CodigoPostal + " ");
                                if (direccionActual.Ubicacion != null)
                                {
                                    if (direccionActual.Ubicacion.Municipio != null && !string.IsNullOrEmpty(direccionActual.Ubicacion.Municipio.Nombre))
                                        dir += (direccionActual.Ubicacion.Municipio.Nombre + " ");
                                    if (direccionActual.Ubicacion.Ciudad != null && !string.IsNullOrEmpty(direccionActual.Ubicacion.Ciudad.Nombre))
                                        dir += (direccionActual.Ubicacion.Ciudad.Nombre + " ");
                                    if (direccionActual.Ubicacion.Estado != null && !string.IsNullOrEmpty(direccionActual.Ubicacion.Estado.Nombre))
                                        dir += (direccionActual.Ubicacion.Estado.Nombre + " ");
                                    if (direccionActual.Ubicacion.Pais != null && !string.IsNullOrEmpty(direccionActual.Ubicacion.Pais.Nombre))
                                        dir += (direccionActual.Ubicacion.Pais.Nombre + " ");
                                }

                                if (dir != null && dir.Trim().CompareTo("") != 0)
                                {
                                    this.vista.DomicilioSucursalActual = dir;
                                    completa = true;
                                }
                                else
                                    this.vista.DomicilioSucursalActual = null;
                            }
                            else
                                this.vista.DomicilioSucursalActual = null;
                        }
                        else
                            this.vista.DomicilioSucursalActual = null;
                    }
                    #endregion

                    this.vista.UltimoObjeto = ea;
                    #endregion
                    break;
            }
        }
        #endregion
        #endregion
    }
}