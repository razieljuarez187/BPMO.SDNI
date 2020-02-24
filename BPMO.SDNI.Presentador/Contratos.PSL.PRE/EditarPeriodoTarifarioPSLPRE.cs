using System;
using System.Collections.Generic;
using System.Linq;
using BPMO.Basicos.BO;
using BPMO.Facade.SDNI.BR;
using BPMO.Patterns.Creational.DataContext;
using BPMO.Primitivos.Enumeradores;
using BPMO.SDNI.Comun.BO;
using BPMO.SDNI.Comun.BR;
using BPMO.SDNI.Contratos.PSL.VIS;

namespace BPMO.SDNI.Contratos.PSL.PRE {
    public class EditarPeriodoTarifarioPSLPRE {
        #region Atributos
        private string nombreClase = "EditarPeriodoTarifarioPSLPRE";
        private IEditarPeriodoTarifarioPSLVIS vista;
        private ucPeriodoTarifarioPSLPRE presentadorPeriodoTarifario;
        private DiaPeriodoTarifaBR periodoTarifaBR;
        private IDataContext dctx;
        #endregion

        #region Constructor
        public EditarPeriodoTarifarioPSLPRE(IEditarPeriodoTarifarioPSLVIS vistaActual, IucPeriodoTarifarioPSLVIS vistaTarifa) {
            try {
                this.vista = vistaActual;
                presentadorPeriodoTarifario = new ucPeriodoTarifarioPSLPRE(vistaTarifa);
                periodoTarifaBR = new DiaPeriodoTarifaBR();
                dctx = FacadeBR.ObtenerConexion();
            } catch (Exception ex) {
                this.MostrarMensaje("Inconsistencias al configurar el presentador", ETipoMensajeIU.ERROR, nombreClase + ".EditarTarifaPSLPRE:" + ex.Message);
            }
        }
        #endregion

        #region Métodos
        public void RealizarPrimeraCarga() {
            this.vista.EstablecerOpcionesUnidadesOperativas(this.ObtenerUnidadesOperativas());
            this.presentadorPeriodoTarifario.PrepararNuevo();
            this.ConsultarTarifas();
            this.EstablecerConfiguracionInicial();
        }

        /// <summary>
        /// Método para obtener un diccionario con los valores de los Tipos de Autorizacion que se envía como parámetro para el llenado del combo correspondiente
        /// </summary>
        /// <returns>Diccionario de tipo string,string</returns>
        private Dictionary<int, string> ObtenerUnidadesOperativas() {
            try {
                Dictionary<int, string> listaUnidadesOperativas = new Dictionary<int, string>();
                List<AdscripcionBO> lstAdscripcion = FacadeBR.ConsultarAdscripcionSeguridad(dctx, new AdscripcionBO(), this.vista.Usuario);

                var grupoUnidadOperativa = lstAdscripcion.GroupBy(ad => ad.UnidadOperativa.Id);
                foreach (var unidadID in grupoUnidadOperativa) {
                    if (unidadID.Key == null)
                        continue;

                    UnidadOperativaBO unidadBO = lstAdscripcion.FirstOrDefault(ad => ad.UnidadOperativa.Id == unidadID.Key.Value).UnidadOperativa;
                    if (unidadBO.Id != null && !string.IsNullOrWhiteSpace(unidadBO.Nombre) && unidadBO.Id != (int)ETipoEmpresa.Idealease) {
                        listaUnidadesOperativas.Add((int)unidadBO.Id, unidadBO.Nombre);
                    }
                }
                return listaUnidadesOperativas;
            } catch (Exception ex) {

                throw new Exception(this.nombreClase + ".ListaTiposAutorizacion:Error al consultar las Unidades Operativas");
            }
        }

        private void EstablecerConfiguracionInicial() {
            this.vista.FUA = DateTime.Now;
            this.vista.UUA = this.vista.UsuarioID;
        }

        /// <summary>
        /// Valida el acceso a las funcionalidades del Sistema
        /// </summary>
        public void ValidarAcceso() {
            try {
                //Valida que usuario y la unidad operativa no sean nulos
                if (this.vista.UsuarioID == null) throw new Exception("El identificador del usuario no debe ser nulo");
                if (this.vista.UnidadOperativaID == null) throw new Exception("El identificador de la Unidad Operativa no debe ser nulo");
                //Se crea el objeto de seguridad
                UsuarioBO usuario = new UsuarioBO() { Id = this.vista.UsuarioID };
                AdscripcionBO adscripcion = new AdscripcionBO() { UnidadOperativa = new UnidadOperativaBO() { Id = this.vista.UnidadOperativaID } };
                SeguridadBO seguridadBO = new SeguridadBO(Guid.Empty, usuario, adscripcion);
            } catch (Exception ex) {
                throw new Exception(this.nombreClase + ".ValidarAcceso: " + ex.Message);
            }
        }
        private bool ExisteAccion(List<CatalogoBaseBO> acciones, string nombreAccion) {
            if (acciones != null && acciones.Count > 0 &&
                acciones.Exists(
                    p => p != null && p.Nombre != null && p.Nombre.Trim().ToUpper() == nombreAccion.Trim().ToUpper()))
                return true;

            return false;
        }

        private List<SucursalBO> SucursalesSeguridad() {
            try {
                if (this.vista.UsuarioID == null)
                    throw new Exception("No se puede identificar al usuario");
                if (this.vista.UnidadOperativaID == null)
                    throw new Exception("No se puede identificar la Unidad Operativa");
                var seguridad = new SeguridadBO(Guid.Empty, new UsuarioBO { Id = this.vista.UsuarioID }, new AdscripcionBO { UnidadOperativa = new UnidadOperativaBO { Id = this.vista.UnidadOperativaID } });
                return FacadeBR.ConsultarSucursalesSeguridad(dctx, seguridad);
            } catch (Exception ex) {
                throw new Exception(this.nombreClase + ".SucursalesSeguridad:Inconsistencias al consultar las sucursales a las que tiene permiso el usuario." + ex.Message);
            }

        }
        public void AgregarTurnoTarifa() {
            try {
                if (this.presentadorPeriodoTarifario.vista.TurnoTarifaID == null) {
                    this.MostrarMensaje("Se debe seleccionar un turno a agregar.", ETipoMensajeIU.ADVERTENCIA);
                } else {
                    this.presentadorPeriodoTarifario.AgregarHorasTurnoAPeriodoTarifario();
                }
            } catch (Exception ex) {
                this.MostrarMensaje("Inconsistencias al agregar una sucursal", ETipoMensajeIU.ERROR, this.nombreClase + ".AgregarSucursalNoAplicar: " + ex.Message);
            }
        }

        private void ConsultarTarifas() {
            try {
                if (this.vista.ObtenerDatosNavegacion() == null)
                    throw new Exception("Se esperaba un objeto de navegación");
                if (!(this.vista.ObtenerDatosNavegacion() is DiaPeriodoTarifaBO))
                    throw new Exception("Se esperaba un PeriodoTarifarioPSLBO en el paquete de navegación");

                List<DiaPeriodoTarifaBO> lst = periodoTarifaBR.ConsultarCompleto(dctx, new DiaPeriodoTarifaBO { UnidadOperativaID = this.vista.UnidadOperativaSeleccionada });
                this.DatoAInterfazUsuario(lst[0]);
                this.vista.UltimoObjeto = lst[0];
            } catch (Exception ex) {
                throw new Exception(nombreClase + ".ConsultarPeriodoTarifario:Error al obtener los datos." + ex.Message);
            }
        }

        /// <summary>
        /// Obtener una lista clonada del objeto para poder guardar como UltimoObjeto
        /// </summary>
        /// <param name="lstTemporal">Lista que será clonada</param>
        /// <returns></returns>
        public List<DiaPeriodoTarifaBO> ObtenerListaClonada(List<DiaPeriodoTarifaBO> lstTemporalSeleccionada) {
            List<DiaPeriodoTarifaBO> lstTemporal = lstTemporalSeleccionada;
            List<DiaPeriodoTarifaBO> lstClonada = new List<DiaPeriodoTarifaBO>();

            foreach (DiaPeriodoTarifaBO periodoTarifa in lstTemporal) {
                lstClonada.Add(periodoTarifa.Clone());
            }

            return lstClonada;

        }

        private void DatoAInterfazUsuario(DiaPeriodoTarifaBO periodoTarifa) {
            if (Object.ReferenceEquals(periodoTarifa, null))
                periodoTarifa = new DiaPeriodoTarifaBO();

            presentadorPeriodoTarifario.DatosAInterfazUsuario(periodoTarifa);

        }

        private DiaPeriodoTarifaBO InterfazUsuarioADato() {
            try {
                if (this.vista.UnidadOperativaID == null)
                    throw new Exception("El identificador de la Unidad Operativa no puede ser nulo");

                DiaPeriodoTarifaBO tarifa = this.presentadorPeriodoTarifario.InterfazUsuarioADato();
                tarifa.UnidadOperativaID = this.vista.UnidadOperativaSeleccionada;
                tarifa.Auditoria = new AuditoriaBO
                {
                    FUA = this.vista.FUA,
                    UUA = this.vista.UsuarioID
                };

                return tarifa;
            } catch (Exception ex) {
                throw new Exception(nombreClase + ".InterfazUsuarioADato:Error al obtener los datos a actualizar." + ex.Message);
            }
        }

        public void Cancelar() {
            this.vista.RedirigirADetalle();
        }

        public void Guardar() {
            try {
                string s;
                if ((s = this.presentadorPeriodoTarifario.ValidarDatos()) != null) {
                    this.vista.MostrarMensaje(s, ETipoMensajeIU.ADVERTENCIA, null);
                    return;
                }
                if ((s = this.ValidarDatos()) != null) {
                    this.vista.MostrarMensaje(s, ETipoMensajeIU.ADVERTENCIA, null);
                    return;
                }

                DiaPeriodoTarifaBO diaPeriodoTarifa = InterfazUsuarioADato();

                #region Obtener la seguridad
                UsuarioBO usr = new UsuarioBO { Id = this.vista.UsuarioID };
                AdscripcionBO adscripcion = new AdscripcionBO { UnidadOperativa = new UnidadOperativaBO { Id = this.vista.UnidadOperativaID } };
                SeguridadBO seguridadBO = new SeguridadBO(Guid.Empty, usr, adscripcion);
                #endregion
                periodoTarifaBR.ActualizarCompleto(dctx, diaPeriodoTarifa, this.vista.UltimoObjeto as DiaPeriodoTarifaBO, seguridadBO);

                this.vista.LimpiarSesion();
                this.vista.EstablecerPaqueteNavegacion(diaPeriodoTarifa);
                this.vista.RedirigirADetalle();
            } catch (Exception ex) {
                this.MostrarMensaje("Error al intentar actualizar los datos", ETipoMensajeIU.ERROR,
                                    nombreClase + ".Guardar:" + ex.Message);
            }
        }

        private string ValidarDatos() {
            string s = "";

            if (this.vista.UnidadOperativaID == null)
                s += "Unidad Operativa, ";

            if (s != null && s.Trim().CompareTo("") != 0)
                return "Los siguientes campos no pueden estar vacíos: \n" + s.Substring(0, s.Length - 2);

            return null;
        }

        private void MostrarMensaje(string mensaje, ETipoMensajeIU tipo, string detalle = null) {
            this.vista.MostrarMensaje(mensaje, tipo, detalle);
        }

        #endregion
    }
}