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
    public class DetallePeriodoTarifarioPSLPRE {
        #region Atributos
        private string nombreClase = "DetallePeriodoTarifarioPSLPRE";
        private IDetallePeriodoTarifarioPSLVIS vista;
        private IDataContext dctx;
        private ucPeriodoTarifarioPSLPRE presentadorPeriodoTarifario;
        private DiaPeriodoTarifaBR periodoTarifaBR;
        private ModuloBR moduloBR;
        #endregion

        #region Constructor
        public DetallePeriodoTarifarioPSLPRE(IDetallePeriodoTarifarioPSLVIS vistaActual, IucPeriodoTarifarioPSLVIS vistaPeriodoTarifario) {
            try {
                vista = vistaActual;
                presentadorPeriodoTarifario = new ucPeriodoTarifarioPSLPRE(vistaPeriodoTarifario);
                dctx = FacadeBR.ObtenerConexion();
            } catch (Exception ex) {
                this.MostrarMensaje("Inconsistencias al momento de configurar los datos del presentador",
                                    ETipoMensajeIU.ERROR, nombreClase + ".DetallePeriodoTarifarioPSLPRE:" + ex.Message);
            }
        }
        #endregion

        #region Métodos
        public void RealizarPrimeraCarga() {
            try {
                if (this.vista.ObtenerDatosNavegacion() == null)
                    throw new Exception("Se esperaba Datos de Navegación");
                if (!(this.vista.ObtenerDatosNavegacion() is DiaPeriodoTarifaBO))
                    throw new Exception("Se esperaba un objeto de PeriodoTarifaPSLBO");

                this.vista.EstablecerOpcionesUnidadesOperativas(this.ObtenerUnidadesOperativas());
                this.presentadorPeriodoTarifario.PrepararDetalle();
                DiaPeriodoTarifaBO periodoTarifa = (DiaPeriodoTarifaBO)this.vista.ObtenerDatosNavegacion();
                periodoTarifa = ConsultarTarifa(periodoTarifa);
                this.DatoAInterfazUsuario(periodoTarifa);
                this.vista.LimpiarSesion();
                this.vista.EstablecerDatosNavegacion(periodoTarifa);
                this.EstablecerSeguridad();
            } catch (Exception ex) {
                this.MostrarMensaje("Inconsistencias al momento de obtener los datos de la tarifa", ETipoMensajeIU.ERROR, nombreClase + ".RealizarPrimeraCarga:" + ex.Message);
                DatoAInterfazUsuario(new DiaPeriodoTarifaBO());
                this.vista.PermitirEditar(false);
            }
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
                if (!FacadeBR.ExisteAccion(dctx, "CONSULTARCOMPLETO", seguridadBO))
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

                //Se valida si el usuario tiene permiso para editar
                if (!this.ExisteAccion(lst, "ACTUALIZARCOMPLETO"))
                    this.vista.PermitirEditar(false);
            } catch (Exception ex) {
                throw new Exception(nombreClase + ".EstablecerSeguridad:" + ex.Message);
            }
        }
        private bool ExisteAccion(List<CatalogoBaseBO> acciones, string accion) {
            if (acciones != null && acciones.Count > 0 && acciones.Exists(p => p != null && p.Nombre != null && p.Nombre.Trim().ToUpper() == accion.Trim().ToUpper()))
                return true;

            return false;
        }

        private void DatoAInterfazUsuario(DiaPeriodoTarifaBO periodoTarifa) {
            try {
                if (Object.ReferenceEquals(periodoTarifa, null))
                    periodoTarifa = new DiaPeriodoTarifaBO();

                presentadorPeriodoTarifario.DatosAInterfazUsuario(periodoTarifa);
            } catch (Exception ex) {
                throw new Exception(nombreClase + ".DatoAInterfazUsuario:Inconsistencia al presentar los datos de la tarifa" + ex.Message);
            }
        }
        private DiaPeriodoTarifaBO InterfazUsuarioADato() {
            DiaPeriodoTarifaBO periodoTarifa = new DiaPeriodoTarifaBO();
            if (this.vista.UnidadOperativaSeleccionadaID == null)
                throw new Exception("El identificador de la Unidad Operativa no puede ser nulo");

            periodoTarifa = this.presentadorPeriodoTarifario.InterfazUsuarioADato();
            periodoTarifa.UnidadOperativaID = this.vista.UnidadOperativaSeleccionadaID;

            return periodoTarifa;
        }

        public void IrAEditar() {
            try {
                DiaPeriodoTarifaBO tarifaTemp = InterfazUsuarioADato();
                this.vista.LimpiarSesion();
                this.vista.EstablecerDatosNavegacion(tarifaTemp);
                this.vista.RedirigirAEditar();
            } catch (Exception ex) {
                this.MostrarMensaje("Error al intentar ir a editar el Período Tarifario", ETipoMensajeIU.ERROR, nombreClase + "IrAEditar" + ex.Message);
            }
        }

        private DiaPeriodoTarifaBO ConsultarTarifa(DiaPeriodoTarifaBO periodoTarifa) {
            try {
                if (periodoTarifa == null)
                    throw new Exception("Se esperaba el objeto PeriodoTarifaPSLBO");
                if (periodoTarifa.UnidadOperativaID == null)
                    throw new Exception("Se necesita el identificador de la Unidad Operativa para ver el Período Tarifario");

                List<DiaPeriodoTarifaBO> lstTemp = new List<DiaPeriodoTarifaBO>();
                periodoTarifaBR = new DiaPeriodoTarifaBR();
                lstTemp = periodoTarifaBR.ConsultarCompleto(dctx, periodoTarifa);
                if (lstTemp.Count == 0) {
                    #region Si no existe un registro en la DB, es porque se agregó la UO pero no se ha configurado
                    #region Obtener la seguridad
                    UsuarioBO usr = new UsuarioBO { Id = this.vista.UsuarioID };
                    AdscripcionBO adscripcion = new AdscripcionBO { UnidadOperativa = new UnidadOperativaBO { Id = this.vista.UnidadOperativaID } };
                    SeguridadBO seguridadBO = new SeguridadBO(Guid.Empty, usr, adscripcion);
                    #endregion
                    #region Asignar valores default para insertar
                    periodoTarifa.IncluyeSD = false;
                    periodoTarifa.DiasDuracionSemana = 0;
                    periodoTarifa.DiasDuracionMes = 0;
                    periodoTarifa.InicioPeriodoDia = 1;
                    periodoTarifa.InicioPeriodoSemana = 0;
                    periodoTarifa.InicioPeriodoMes = 0;
                    periodoTarifa.Auditoria = new AuditoriaBO() { UC = this.vista.UsuarioID, FC = DateTime.Now, UUA = this.vista.UsuarioID, FUA = DateTime.Now };
                    #endregion
                    periodoTarifaBR.InsertarCompleto(dctx, periodoTarifa, seguridadBO);
                    #endregion
                }
                if (lstTemp.Count > 1)
                    throw new Exception("Inconsistencias en los registros de la base datos, se encontro mas de un registro de la tarifa que desea buscar");
                return lstTemp[0];
            } catch (Exception ex) {
                throw new Exception(nombreClase + ".ConsultarPeriodoTarifario:Inconsistencias al consultar los datos del Período Tarifario." + ex.Message);
            }
        }

        private void MostrarMensaje(string mensaje, ETipoMensajeIU tipo, string detalle = null) {
            this.vista.MostrarMensaje(mensaje, tipo, detalle);
        }
        #endregion
    }
}
