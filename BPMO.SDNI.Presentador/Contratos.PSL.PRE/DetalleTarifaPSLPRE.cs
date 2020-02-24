
// Construcción de Mejoras - Cobro de Rangos de Km y Horas.
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Linq;
using BPMO.Basicos.BO;
using BPMO.Facade.SDNI.BR;
using BPMO.Patterns.Creational.DataContext;
using BPMO.Primitivos.Enumeradores;
using BPMO.SDNI.Comun.BO;
using BPMO.SDNI.Comun.BR;
using BPMO.SDNI.Contratos.PSL.BO;
using BPMO.SDNI.Contratos.PSL.BR;
using BPMO.SDNI.Contratos.PSL.VIS;
using BPMO.Servicio.Catalogos.BO;

namespace BPMO.SDNI.Contratos.PSL.PRE {
    public class DetalleTarifaPSLPRE {
        #region Atributos
        private string nombreClase = "DetalleTarifaPSLPRE";
        private IDetalleTarifaPSLVIS vista;
        private IDataContext dctx;
        private ucTarifaPSLPRE presentadorTarifa;
        private TarifaPSLBR tarifaBR;
        private ModuloBR moduloBR;
        #endregion

        #region Constructor
        public DetalleTarifaPSLPRE(IDetalleTarifaPSLVIS vistaActual, IucTarifaPSLVIS vistaTarifas) {
            try {
                vista = vistaActual;
                presentadorTarifa = new ucTarifaPSLPRE(vistaTarifas);
                dctx = FacadeBR.ObtenerConexion();
            } catch (Exception ex) {
                this.MostrarMensaje("Inconsistencias al momento de configurar los datos del presentador",
                                    ETipoMensajeIU.ERROR, nombreClase + ".DetalleTarifaPSLPRE:" + ex.Message);
            }
        }
        #endregion

        #region Métodos
        public void RealizarPrimeraCarga() {
            try {
                if (this.vista.ObtenerDatosNavegacion() == null)
                    throw new Exception("Se esperaba Datos de Navegación");
                if (!(this.vista.ObtenerDatosNavegacion() is TarifaPSLBO))
                    throw new Exception("Se esperaba un objeto de TarifaPSLBO");

                TarifaPSLBO tarifa = (TarifaPSLBO)this.vista.ObtenerDatosNavegacion();

                tarifa = ConsultarTarifa(tarifa);

                this.DatoAInterfazUsuario(tarifa, ObtenerPrecioCombustible());
                #region SC0024
                this.VerificarOpRegresar();
                #endregion SC0024
                this.vista.LimpiarSesion();
                this.EstablecerSeguridad();
            } catch (Exception ex) {
                this.MostrarMensaje("Inconsistencias al momento de obtener los datos de la tarifa", ETipoMensajeIU.ERROR, nombreClase + ".RealizarPrimeraCarga:" + ex.Message);
                DatoAInterfazUsuario(new TarifaPSLBO(), null);
                this.vista.PermitirEditar(false);
            }
        }

        public void ValidarAcceso() {
            try {
                //se valida que los datos del usuario y la unidad operativa no sean nulos
                if (this.vista.UsuarioID == null) throw new Exception("El identificador del usuario no debe ser nulo");
                if (this.vista.UnidadOperativaID == null) throw new Exception("El identificador de la Unidad Operativa no debe ser nulo ");

                //Se crea el objeto de seguridad
                UsuarioBO usuario = new UsuarioBO() { Id = this.vista.UsuarioID };
                AdscripcionBO adscripcion = new AdscripcionBO() { UnidadOperativa = new UnidadOperativaBO() { Id = this.vista.UnidadOperativaID } };
                SeguridadBO seguridadBO = new SeguridadBO(Guid.Empty, usuario, adscripcion);

                //Se valida si el usuario tiene permiso a la acción principal
                if (!FacadeBR.ExisteAccion(dctx, "CONSULTAR", seguridadBO))
                    this.vista.RedirigirSinPermisoAcceso();
            } catch (Exception ex) {
                throw new Exception(nombreClase + ".ValidarAcceso:" + ex.Message);
            }
        }
        public void EstablecerSeguridad() {
            try {
                //se valida que los datos del usuario y la unidad operativa no sean nulos
                if (this.vista.UsuarioID == null) throw new Exception("El identificador del usuario no debe ser nulo");
                if (this.vista.UnidadOperativaID == null) throw new Exception("El identificador de la Unidad Operativa no debe ser nulo ");

                //Se crea el objeto de seguridad
                UsuarioBO usuario = new UsuarioBO() { Id = this.vista.UsuarioID };
                AdscripcionBO adscripcion = new AdscripcionBO() { UnidadOperativa = new UnidadOperativaBO() { Id = this.vista.UnidadOperativaID } };
                SeguridadBO seguridadBO = new SeguridadBO(Guid.Empty, usuario, adscripcion);

                //Se obtienen las acciones a las cuales el usuario tiene permiso en este proceso
                List<CatalogoBaseBO> lst = FacadeBR.ConsultarAccion(dctx, seguridadBO);

                //Se valida si el usuario tiene permiso para registrar
                if (!this.ExisteAccion(lst, "INSERTARCOMPLETO"))
                    this.vista.PermitirRegistrar(false);
                //Se valida si el usuario tiene permiso para editar
                if (!this.ExisteAccion(lst, "ACTUALIZARCOMPLETO"))
                    this.vista.PermitirEditar(false);
                //Se valida si el usuario tiene permiso para pe
                if (!this.ExisteAccion(lst, "UI APLICAR"))
                    this.PermitirAplicarSoloEspecial(false);
            } catch (Exception ex) {
                throw new Exception(nombreClase + ".EstablecerSeguridad:" + ex.Message);
            }
        }
        private bool ExisteAccion(List<CatalogoBaseBO> acciones, string accion) {
            if (acciones != null && acciones.Count > 0 && acciones.Exists(p => p != null && p.Nombre != null && p.Nombre.Trim().ToUpper() == accion.Trim().ToUpper()))
                return true;

            return false;
        }

        private void DatoAInterfazUsuario(TarifaPSLBO tarifa, decimal? precioCombustible) {
            try {
                if (tarifa.TarifaPSLID != null)
                    this.vista.TarifaPSLID = tarifa.TarifaPSLID;
                if (tarifa.Modelo != null && tarifa.Modelo.Id != null) {
                    this.vista.NombreModelo = tarifa.Modelo.Nombre;
                    this.vista.ModeloID = tarifa.Modelo.Id;
                }
                if (tarifa.Divisa != null && tarifa.Divisa.MonedaDestino != null &&
                    !String.IsNullOrEmpty(tarifa.Divisa.MonedaDestino.Codigo)) {
                    this.vista.NombreMoneda = tarifa.Divisa.MonedaDestino.Nombre;
                    this.vista.CodigoMoneda = tarifa.Divisa.MonedaDestino.Codigo;
                }
                if (tarifa.Sucursal != null && tarifa.Sucursal.Id != null) {
                    this.vista.NombreSucursal = tarifa.Sucursal.Nombre;
                    this.vista.SucursalID = tarifa.Sucursal.Id;
                }
                if (tarifa.TipoTarifaID != null) {
                    this.vista.NombreTipoTarifa = tarifa.TipoTarifaID;
                    foreach (var tipo in Enum.GetValues(typeof(ETipoTarifa))) {
                        if (Enum.GetName(typeof(ETipoTarifa), tipo) == tarifa.TipoTarifaID) {
                            this.vista.TipoTarifa = (int)tipo;
                            break;
                        }
                    }
                }
                if (tarifa.Auditoria != null) {
                    this.vista.FechaRegistro = tarifa.Auditoria.FC;
                    this.vista.FechaModificacion = tarifa.Auditoria.FUA;
                    if (tarifa.Auditoria.UC != null)
                        this.vista.UsuarioRegistro = ObtenerNombreEmpleado(tarifa.Auditoria.UC);
                    if (tarifa.Auditoria.UUA != null)
                        this.vista.UsuarioModificacion = ObtenerNombreEmpleado(tarifa.Auditoria.UUA);
                }
                this.vista.Estatus = tarifa.Activo != null
                                         ? tarifa.Activo.ToString()
                                                 .ToUpper()
                                                 .Replace("TRUE", "ACTIVO")
                                                 .Replace("FALSE", "INACTIVO")
                                         : String.Empty;
                if (tarifa.TarifaTurno != null) {
                    Type type = this.vista.UnidadOperativaID == (int)ETipoEmpresa.Construccion ? typeof(ETarifaTurnoConstruccion) : 
                        this.vista.UnidadOperativaID == (int)ETipoEmpresa.Generacion ? typeof(ETarifaTurnoGeneracion) :
                        typeof(ETarifaTurnoEquinova);
                    var memInfo = type.GetMember(type.GetEnumName(tarifa.TarifaTurno));
                    var display = memInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false).FirstOrDefault() as DescriptionAttribute;

                    if (display != null) {
                        this.vista.NombreTarifaTurno = display.Description;
                    }
                    switch ((ETipoEmpresa)this.vista.UnidadOperativaID) {
                        case ETipoEmpresa.Construccion:
                            this.vista.TarifaTurno = (ETarifaTurnoConstruccion)tarifa.TarifaTurno;
                            break;
                        case ETipoEmpresa.Generacion:
                            this.vista.TarifaTurno = (ETarifaTurnoGeneracion)tarifa.TarifaTurno;
                            break;
                        case ETipoEmpresa.Equinova:
                            this.vista.TarifaTurno = (ETarifaTurnoEquinova)tarifa.TarifaTurno;
                            break;
                    }
                }

                if (tarifa.PeriodoTarifa != null) {
                    Type type = typeof(EPeriodosTarifa);
                    var memInfo = type.GetMember(type.GetEnumName(tarifa.PeriodoTarifa));
                    var display = memInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false).FirstOrDefault() as DescriptionAttribute;

                    if (display != null) {
                        this.vista.NombrePeriodoTarifa = display.Description;
                    }
                    this.vista.PeriodoTarifa = tarifa.PeriodoTarifa;
                }
                this.vista.PrecioCombustible = precioCombustible;
                presentadorTarifa.ModoConsulta(true);
                presentadorTarifa.DatosAInterfazUsuario(tarifa);
            } catch (Exception ex) {
                throw new Exception(nombreClase + ".DatoAInterfazUsuario:Inconsistencia al presentar los datos de la tarifa" + ex.Message);
            }
        }
        private TarifaPSLBO InterfazUsuarioADato() {
            TarifaPSLBO tarifa = new TarifaPSLBO();
            tarifa.TarifaPSLID = this.vista.TarifaPSLID;
            tarifa.TarifaTurno = this.vista.TarifaTurno;
            tarifa.PeriodoTarifa = this.vista.PeriodoTarifa;
            tarifa.Divisa = new DivisaBO { MonedaDestino = new MonedaBO { Codigo = this.vista.CodigoMoneda } };
            tarifa.Modelo = new ModeloBO { Id = this.vista.ModeloID };
            tarifa.Sucursal = new SucursalBO { Id = this.vista.SucursalID, UnidadOperativa = new UnidadOperativaBO { Id = this.vista.UnidadOperativaID } };
            tarifa.Tipo = (ETipoTarifa)Enum.Parse(typeof(ETipoTarifa), this.vista.TipoTarifa.ToString());

            return tarifa;
        }

        public void IrAEditar() {
            try {
                TarifaPSLBO tarifaTemp = InterfazUsuarioADato();
                this.vista.EstablecerDatosNavegacion(tarifaTemp);
                this.vista.RedirigirAEditar();
            } catch (Exception ex) {
                this.MostrarMensaje("Error al intentar ir a editar la tarifa", ETipoMensajeIU.ERROR, nombreClase + "IrAEditar" + ex.Message);
            }
        }
        private string ObtenerNombreEmpleado(int? numeroEmpleado) {
            try {
                if (numeroEmpleado == null)
                    return "";

                List<EmpleadoBO> empleadosBO = FacadeBR.ConsultarEmpleadoCompleto(FacadeBR.ObtenerConexion(),
                                                                                  new EmpleadoBO()
                                                                                      {
                                                                                          Numero = numeroEmpleado
                                                                                      });

                if (empleadosBO.Count == 0)
                    return "";

                return (empleadosBO[0].NombreCompleto ?? "");
            } catch (Exception ex) {
                throw new Exception(nombreClase + ".ObtenerNombreEmpleado:Error al consultar los datos del empleado." + ex.Message);
            }
        }
        private decimal? ObtenerPrecioCombustible() {
            try {
                if (this.vista.UnidadOperativaID == null)
                    throw new Exception("El indentificador de la unidad operativa no debe ser nulo");
                AppSettingsReader n = new AppSettingsReader();
                ConfiguracionUnidadOperativaBO configUO = null;
                int moduloID = (int)this.vista.ModuloID;

                ModuloBO modulo = new ModuloBO() { ModuloID = moduloID };
                ModuloBR moduloBR = new ModuloBR();
                List<ModuloBO> modulos = moduloBR.ConsultarCompleto(dctx, modulo);

                if (modulos.Count > 0) {
                    modulo = modulos[0];

                    List<ConfiguracionUnidadOperativaBO> lstConfigUO = new ModuloBR().ConsultarConfiguracionUnidadOperativa(this.dctx, new ConfiguracionUnidadOperativaBO() { UnidadOperativa = new UnidadOperativaBO() { Id = this.vista.UnidadOperativaID } }, this.vista.ModuloID);
                    configUO = lstConfigUO.FirstOrDefault();
                }
                if (configUO != null)
                    return configUO.PrecioUnidadCombustible;
                return null;
            } catch (Exception ex) {
                throw new Exception(nombreClase + ".ObtenerPrecioCombustible: Error al consultar el precio del combustible. " + ex.Message);
            }
        }

        private TarifaPSLBO ConsultarTarifa(TarifaPSLBO tarifa) {
            try {
                if (tarifa == null)
                    throw new Exception("Se esperaba el objeto TarifaPSLBO");
                if (tarifa.TarifaPSLID == null)
                    throw new Exception("Se necesita el identificador de la tarifa");

                List<TarifaPSLBO> lstTemp = new List<TarifaPSLBO>();
                tarifaBR = new TarifaPSLBR();
                lstTemp = tarifaBR.ConsultarCompleto(dctx, tarifa);
                if (lstTemp.Count == 0)
                    throw new Exception("No se encontro ningún registro en la base datos");
                if (lstTemp.Count > 1)
                    throw new Exception("Inconsistencias en los registros de la base datos, se encontro mas de un registro de la tarifa que desea buscar");
                return lstTemp[0];
            } catch (Exception ex) {
                throw new Exception(nombreClase + ".ConsultarTarifa:Inconsistencias al consultar los datos de la Tarifa." + ex.Message);
            }
        }

        private void PermitirAplicarSoloEspecial(bool aplicarSoloEspecial) {
            try {
                ETipoTarifa tipo =
                    (ETipoTarifa)Enum.Parse(typeof(ETipoTarifa), this.vista.TipoTarifa.ToString());
                if (tipo == ETipoTarifa.ESPECIAL) {
                    this.vista.PermitirEditar(aplicarSoloEspecial);
                }

            } catch (Exception ex) {

                throw new Exception(nombreClase + ".PermitirAplicarSoloEspecial:Error al configurar la accion permitir Editar una Tarifa Especial." + ex.Message);
            }
        }

        private void MostrarMensaje(string mensaje, ETipoMensajeIU tipo, string detalle = null) {
            this.vista.MostrarMensaje(mensaje, tipo, detalle);
        }
        #region SC0024
        /// <summary>
        /// SC0024
        /// Redirige a la página principal de consulta
        /// </summary>
        public void RetrocederPagina() {
            this.vista.RegresarAConsultar();
        }
        /// <summary>
        /// SC0024
        /// Deshabilita el botón de Regresar a consulta con filtros
        /// </summary>
        public void PermitirRegresar(bool permiso) {
            this.vista.PermitirRegresar(permiso);
        }
        /// <summary>
        /// SC0024
        /// Verifica si hay en sesión un diccionario de retorno, de no haberlo deshabilita la opción de regresar
        /// </summary>
        public void VerificarOpRegresar() {
            Dictionary<string, object> elementosFiltro = this.vista.ObtenerFiltrosConsulta() as Dictionary<string, object>;
            if (elementosFiltro == null)
                this.PermitirRegresar(false);
            else
                this.PermitirRegresar(true);
        }
        #endregion
        #endregion
    }
}