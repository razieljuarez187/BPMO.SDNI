// Satisface al caso de uso CU005 - Catálogo de Tarifas de Renta Comercial Diaria.
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
    public class EditarTarifaPSLPRE {
        #region Atributos
        private string nombreClase = "EditarTarifaPSLPRE";
        private IEditarTarifaPSLVIS vista;
        private ucTarifaPSLPRE presentadorTarifas;
        private TarifaPSLBR tarifaBR;
        private IDataContext dctx;
        #endregion

        #region Constructor
        public EditarTarifaPSLPRE(IEditarTarifaPSLVIS vistaActual, IucTarifaPSLVIS vistaTarifa) {
            try {
                this.vista = vistaActual;
                presentadorTarifas = new ucTarifaPSLPRE(vistaTarifa);
                tarifaBR = new TarifaPSLBR();
                dctx = FacadeBR.ObtenerConexion();
            } catch (Exception ex) {
                this.MostrarMensaje("Inconsistencias al configurar el presentador", ETipoMensajeIU.ERROR, nombreClase + ".EditarTarifaPSLPRE:" + ex.Message);
            }
        }
        #endregion

        #region Métodos
        public void RealizarPrimeraCarga() {
            this.PrepararNuevo();

            //Si la sucursal seleccionada es matriz entonces puede aplicar tarifas a varias sucursales
            List<SucursalBO> lst = FacadeBR.ConsultarSucursal(this.dctx, new SucursalBO() { Id = this.vista.SucursalID });
            bool esMatriz = (lst.Count > 0 && lst[0].Matriz != null && lst[0].Matriz == true);

            this.vista.PermitirAgregarSucursales(esMatriz);
            this.MostrarLeyendaSucursales(esMatriz);
            this.vista.AplicarOtrasSucursales = esMatriz;

            TarifaPSLBO tarifa = this.ConsultarTarifas();

            this.DatoAInterfazUsuario(tarifa);
            this.vista.PrecioCombustible = ObtenerPrecioCombustible();

            this.EstablecerSeguridad();
        }
        public void PrepararNuevo() {
            this.vista.TarifaPSLID = null;
            this.vista.ModeloID = null;
            this.vista.CodigoMoneda = null;
            this.vista.SucursalID = null;
            this.vista.SucursalNoAplicaID = null;
            this.vista.TipoTarifa = null;

            this.vista.NombreModelo = null;
            this.vista.NombreMoneda = null;
            this.vista.NombreSucursal = null;
            this.vista.NombreSucursalNoAplica = null;
            this.vista.NombreTipoTarifa = null;

            this.vista.SessionListaSucursalSeleccionada = null;
            this.vista.AplicarOtrasSucursales = false;

            this.vista.MostrarAplicarSucursal(true);

            this.MostrarLeyendaSucursales(false);
        }

        public void EstablecerSeguridad() {
            try {
                //Valida que el usuario y la unidad operativva no sean nulos
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

                //Consulta de lista de acciones a las que tiene permiso el usuario
                List<CatalogoBaseBO> acciones = FacadeBR.ConsultarAccion(dctx, seguridad);

                // se valida si el usuario tiene permisos para registrar
                if (!this.ExisteAccion(acciones, "INSERTARCOMPLETO"))
                    this.vista.PermitirRegistrar(false);
                // se valida si el usuario tiene a editar una tarifa especial
                if (!this.ExisteAccion(acciones, "UI APLICAR"))
                    this.PermitirAplicarSoloEspecial(false);
            } catch (Exception ex) {
                throw new Exception(this.nombreClase + ".EstablecerSeguridad:" + ex.Message);
            }

        }
        public void ValidarAcceso() {
            try {
                //Valida que usuario y la unidad operativa no sean nulos
                if (this.vista.UsuarioID == null) throw new Exception("El identificador del usuario no debe ser nulo");
                if (this.vista.UnidadOperativaID == null) throw new Exception("El identificador de la Unidad Operativa no debe ser nulo");
                //Se crea el objeto de seguridad
                UsuarioBO usuario = new UsuarioBO() { Id = this.vista.UsuarioID };
                AdscripcionBO adscripcion = new AdscripcionBO() { UnidadOperativa = new UnidadOperativaBO() { Id = this.vista.UnidadOperativaID } };
                SeguridadBO seguridadBO = new SeguridadBO(Guid.Empty, usuario, adscripcion);

                //Se valida si el usuario tiene permiso a la acción principal
                if (!FacadeBR.ExisteAccion(this.dctx, "ACTUALIZARCOMPLETO", seguridadBO))
                    this.vista.RedirigirSinPermisoAcceso();
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
        public void AgregarSucursalNoAplicar() {
            try {
                if (this.vista.SucursalNoAplicaID == null) {
                    this.MostrarMensaje("Se debe seleccionar una sucursal a agregar", ETipoMensajeIU.ADVERTENCIA);
                } else {
                    SucursalBO sucursalAgregar = new SucursalBO
                    {
                        Id = this.vista.SucursalNoAplicaID,
                        Nombre = this.vista.NombreSucursalNoAplica
                    };

                    if (String.IsNullOrEmpty(sucursalAgregar.Nombre))
                        throw new Exception("El nombre de la sucursal no se puede obtener");
                    if (sucursalAgregar.Id == this.vista.SucursalID) {
                        this.MostrarMensaje("No se puede agregar la sucursal, ya que es la sucursal a la cual se esta configurando la tarifa", ETipoMensajeIU.ADVERTENCIA);
                        this.vista.NombreSucursalNoAplica = null;
                        this.vista.SucursalNoAplicaID = null;
                        return;
                    }
                    if (this.vista.SessionListaSucursalSeleccionada.Find(suc => sucursalAgregar.Id == suc.Id) == null) {
                        var sucursales = new List<SucursalBO>(this.vista.SessionListaSucursalSeleccionada) { sucursalAgregar };
                        this.vista.SessionListaSucursalSeleccionada = sucursales;
                        this.vista.NombreSucursalNoAplica = null;
                        this.vista.SucursalNoAplicaID = null;
                    } else {
                        this.vista.NombreSucursalNoAplica = null;
                        this.vista.SucursalNoAplicaID = null;
                        this.MostrarMensaje("La sucursal ya se encuentra agregada", ETipoMensajeIU.ADVERTENCIA);
                    }
                }
            } catch (Exception ex) {
                this.MostrarMensaje("Inconsistencias al agregar una sucursal", ETipoMensajeIU.ERROR, this.nombreClase + ".AgregarSucursalNoAplicar: " + ex.Message);
            }
        }
        public void QuitarSucursal(SucursalBO sucursal) {
            try {
                if (sucursal != null && sucursal.Id != null) {
                    if (this.vista.SessionListaSucursalSeleccionada.Find(suc => sucursal.Id == suc.Id) != null) {
                        var sucursales = new List<SucursalBO>(vista.SessionListaSucursalSeleccionada);
                        sucursales.Remove(sucursal);
                        this.vista.SessionListaSucursalSeleccionada = sucursales;
                    } else
                        throw new Exception(
                            "La sucursal seleccionada no se encuentra en la lista de de las sucursales de no aplica");
                } else
                    throw new Exception("Se requiere una Sucursal valido para realizar la operación");
            } catch (Exception ex) {
                this.MostrarMensaje("Inconsistencias al intentar quitar la Sucursal seleccionada", ETipoMensajeIU.ERROR, this.nombreClase + ".QuitarSucursal:" + ex.Message);
            }
        }
        public void AplicarOtrasSucursales() {
            try {
                if (this.vista.AplicarOtrasSucursales == true) {
                    this.vista.SessionListaSucursalSeleccionada = new List<SucursalBO>();
                    this.vista.MostrarAplicarSucursal(false);
                } else {
                    this.vista.SessionListaSucursalSeleccionada = new List<SucursalBO>();
                    this.vista.NombreSucursalNoAplica = null;
                    this.vista.SucursalNoAplicaID = null;
                    this.vista.MostrarAplicarSucursal(true);
                }
            } catch (Exception ex) {

                throw new Exception(nombreClase + ".AplicarOtrasSucursales:Inconsistencia al configurar las secciones a mostrar." + ex.Message);
            }
        }
        private TarifaPSLBO ConsultarTarifas() {
            try {
                if (this.vista.AplicarOtrasSucursales == null)
                    throw new Exception("El valor del aplicar a otras sucursales no puede ser nulo");
                if (this.vista.UsuarioID == null)
                    throw new Exception("El identificador del usuario no puede ser nulo");
                if (this.vista.UnidadOperativaID == null)
                    throw new Exception("El identificador de la unidad operativa no puede ser nulo");
                if (this.vista.ObtenerDatosNavegacion() == null)
                    throw new Exception("Se esperaba un objeto de navegación");
                if (!(this.vista.ObtenerDatosNavegacion() is TarifaPSLBO))
                    throw new Exception("Se esperaba un TarifaPSLBO en el paquete de navegación");
                TarifaPSLBO tarifaTemp = this.vista.ObtenerDatosNavegacion() as TarifaPSLBO;

                if (this.vista.AplicarOtrasSucursales == false) {
                    tarifaTemp = ConsultarTarifaSucursal(new TarifaPSLBO { TarifaPSLID = tarifaTemp.TarifaPSLID });
                    List<TarifaPSLBO> lst = new List<TarifaPSLBO>();
                    lst.Add(new TarifaPSLBO(tarifaTemp));
                    this.vista.TarifasAnteriores = lst;
                    return tarifaTemp;
                } else {
                    tarifaTemp = ConsultarTarifaSucursal(new TarifaPSLBO { TarifaPSLID = tarifaTemp.TarifaPSLID });
                    List<TarifaPSLBO> lstTemp = ConsultarTarifasSucursales(tarifaTemp);
                    this.vista.TarifasAnteriores = lstTemp;
                    return tarifaTemp;
                }
                return null;
            } catch (Exception ex) {
                throw new Exception(nombreClase + ".ConsultarTarifas:Error al obtener los datos de la tarifa." + ex.Message);
            }
        }
        private TarifaPSLBO ConsultarTarifaSucursal(TarifaPSLBO tarifa) {
            try {
                if (Object.ReferenceEquals(tarifa, null)) throw new Exception("El objeto no puede ser nulo");
                if (tarifa.TarifaPSLID == null) throw new Exception("El identificador de la tarifa no puede ser nulo");
                List<TarifaPSLBO> lstTarifas = tarifaBR.ConsultarCompleto(dctx, new TarifaPSLBO { TarifaPSLID = tarifa.TarifaPSLID });
                if (lstTarifas.Count == 0) throw new Exception("No se encontro la tarifa especificada");
                TarifaPSLBO tarifaTemp = new TarifaPSLBO(lstTarifas[0]);
                return tarifaTemp;
            } catch (Exception ex) {
                throw new Exception(nombreClase + ".ConsultarTarifaSucursal:Error al consultar la tarifa." + ex.Message);
            }
        }
        private List<TarifaPSLBO> ConsultarTarifasSucursales(TarifaPSLBO tarifa) {
            try {
                //validacion de los filtros obligatorios para buscar las sucursales que tienen la misma tarifa.
                if (Object.ReferenceEquals(tarifa, null)) throw new Exception("El objeto no puede ser nulo");
                if (Object.ReferenceEquals(tarifa.Modelo, null)) throw new Exception("El objeto Modelo de la tarifa no puede ser nulo");
                if (tarifa.Divisa == null || tarifa.Divisa.MonedaDestino == null)
                    throw new Exception(" El objeto de divisa de la tarifa no puede ser nulo");
                if (tarifa.Tipo == null) throw new Exception("el tipo de tarifa no puede ser nulo");

                if (this.vista.UnidadOperativaID == null)
                    throw new Exception("La unidad Operativa de la tarifa no puede ser nulo");
                if (tarifa.Modelo.Id == null) throw new Exception("el identificador del modelo de la tarifa no puede ser nulo");
                if (String.IsNullOrEmpty(tarifa.Divisa.MonedaDestino.Codigo)) throw new Exception("El código de la moneda no puede ser vacío");


                //seccion de consulta
                TarifaPSLBO tarifaTemp = new TarifaPSLBO
                    {
                        Sucursal = new SucursalBO { UnidadOperativa = new UnidadOperativaBO { Id = this.vista.UnidadOperativaID } },
                        Modelo = new ModeloBO { Id = tarifa.Modelo.Id },
                        Divisa = new DivisaBO { MonedaDestino = new MonedaBO { Codigo = tarifa.Divisa.MonedaDestino.Codigo } },
                        Tipo = tarifa.Tipo,
                        Activo = true,
                        SucursalesConsulta = SucursalesSeguridad()
                    };
                List<TarifaPSLBO> lstTarifa = tarifaBR.Consultar(dctx, tarifaTemp);
                return lstTarifa;
            } catch (Exception ex) {
                throw new Exception(nombreClase + ".ConsultarTarifasSucursales:Error al consultar las sucursales que tiene las mismas tarifas." + ex.Message);
            }
        }

        private void MostrarLeyendaSucursales(bool mostrar) {
            string leyenda = "";

            if (mostrar) {
                List<SucursalBO> lst = this.SucursalesSeguridad();
                foreach (SucursalBO sucursal in lst) {
                    if (!string.IsNullOrEmpty(sucursal.Nombre) && !string.IsNullOrWhiteSpace(sucursal.Nombre))
                        leyenda += sucursal.Nombre + ", ";
                }

                if (leyenda != null && leyenda.Trim().CompareTo("") != 0)
                    leyenda = leyenda.Substring(0, leyenda.Length - 2);
            }

            this.vista.MostrarLeyendaSucursales(mostrar, leyenda);
        }

        private void DatoAInterfazUsuario(TarifaPSLBO tarifa) {
            if (Object.ReferenceEquals(tarifa, null))
                tarifa = new TarifaPSLBO();
            if (Object.ReferenceEquals(tarifa.Sucursal, null))
                tarifa.Sucursal = new SucursalBO();
            if (Object.ReferenceEquals(tarifa.Modelo, null))
                tarifa.Modelo = new ModeloBO();
            if (Object.ReferenceEquals(tarifa.Divisa, null))
                tarifa.Divisa = new DivisaBO();
            if (object.ReferenceEquals(tarifa.Divisa.MonedaDestino, null))
                tarifa.Divisa.MonedaDestino = new MonedaBO();

            this.vista.TarifaPSLID = tarifa.TarifaPSLID;
            this.vista.SucursalID = tarifa.Sucursal.Id;
            this.vista.NombreSucursal = tarifa.Sucursal.Nombre;
            this.vista.ModeloID = tarifa.Modelo.Id;
            this.vista.NombreModelo = tarifa.Modelo.Nombre;
            this.vista.CodigoMoneda = tarifa.Divisa.MonedaDestino.Codigo;
            this.vista.NombreMoneda = tarifa.Divisa.MonedaDestino.Nombre;
            this.vista.NombreTipoTarifa = tarifa.Tipo.ToString();
            if (tarifa.TipoTarifaID != null) {
                this.vista.NombreTipoTarifa = tarifa.TipoTarifaID;
                foreach (var tipo in Enum.GetValues(typeof(ETipoTarifa))) {
                    if (Enum.GetName(typeof(ETipoTarifa), tipo) == tarifa.TipoTarifaID) {
                        this.vista.TipoTarifa = (int)tipo;
                        break;
                    }
                }
            }
            this.vista.Estatus = tarifa.Activo;
            if (tarifa.TarifaTurno != null) {
                Type type = this.vista.UnidadOperativaID == (int)ETipoEmpresa.Construccion ? typeof(ETarifaTurnoConstruccion) : 
                    this.vista.UnidadOperativaID == (int)ETipoEmpresa.Generacion ? typeof(ETarifaTurnoGeneracion) : typeof(ETarifaTurnoEquinova);
                var memInfo = type.GetMember(type.GetEnumName(tarifa.TarifaTurno));
                var display = memInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false).FirstOrDefault() as DescriptionAttribute;

                if (display != null) {
                    this.vista.NombreTarifaTurno = display.Description;
                }
                this.vista.TarifaTurno = tarifa.TarifaTurno;
            }

            if (tarifa.PeriodoTarifa != null) {
                this.vista.NombrePeriodoTarifa = Enum.GetName(typeof(EPeriodosTarifa), tarifa.PeriodoTarifa).Replace("_", "-");
                this.vista.PeriodoTarifa = tarifa.PeriodoTarifa;
            }
            this.vista.Estatus = tarifa.Activo;

            presentadorTarifas.DatosAInterfazUsuario(tarifa);
            this.vista.NombreSucursalNoAplica = null;
            this.vista.SucursalNoAplicaID = null;
            this.vista.SessionListaSucursalSeleccionada = null;
        }
        private List<TarifaPSLBO> InterfazUsuarioADato() {
            try {
                if (this.vista.UnidadOperativaID == null)
                    throw new Exception("El identificador de la Unidad Operativa no puede ser nulo");
                if (this.vista.AplicarOtrasSucursales == null)
                    throw new Exception("No aplicar a otras sucursales no puede ser nulo");
                if (this.vista.Estatus == null)
                    throw new Exception("El estatus no debe ser nulo");

                List<TarifaPSLBO> lstTarifas = new List<TarifaPSLBO>();
                List<TarifaPSLBO> tarifasAnteriores = this.vista.TarifasAnteriores;

                TarifaPSLBO tarifa = this.presentadorTarifas.InterfazUsuarioADato();
                ETipoTarifa tipoTarifa = (ETipoTarifa)Enum.Parse(typeof(ETipoTarifa), this.vista.TipoTarifa.ToString());
                tarifa.Tipo = tipoTarifa;
                tarifa.Modelo = new ModeloBO { Id = this.vista.ModeloID };
                tarifa.Divisa = new DivisaBO { MonedaDestino = new MonedaBO { Codigo = this.vista.CodigoMoneda } };
                tarifa.Activo = this.vista.Estatus;
                tarifa.Auditoria = new AuditoriaBO
                {
                    FUA = this.vista.FUA,
                    UUA = this.vista.UsuarioID
                };

                if (this.vista.AplicarOtrasSucursales != null && this.vista.AplicarOtrasSucursales == true) {
                    foreach (SucursalBO suc in this.vista.SessionListaSucursalSeleccionada) {
                        if (tarifasAnteriores.Find(t => suc.Id == t.Sucursal.Id) != null)
                            tarifasAnteriores.RemoveAll(s => s.Sucursal.Id == suc.Id);

                    }
                    this.vista.TarifasAnteriores = tarifasAnteriores;
                    foreach (TarifaPSLBO t in tarifasAnteriores) {
                        TarifaPSLBO tarifaTemp = new TarifaPSLBO(tarifa) { Sucursal = t.Sucursal, TarifaPSLID = t.TarifaPSLID };
                        lstTarifas.Add(tarifaTemp);
                    }
                } else {
                    this.vista.TarifasAnteriores.RemoveAll(t => t.Sucursal.Id != this.vista.SucursalID);

                    tarifa.Sucursal = tarifasAnteriores.FindLast(t => t.Sucursal.Id == this.vista.SucursalID).Sucursal;
                    tarifa.TarifaPSLID = this.vista.TarifaPSLID;
                    lstTarifas.Add(tarifa);
                }

                return lstTarifas;
            } catch (Exception ex) {
                throw new Exception(nombreClase + ".InterfazUsuarioADato:Error al obtener los datos a actualizar." + ex.Message);
            }
        }

        public void Cancelar() {
            this.vista.RedirigirAConsulta();
        }
        public void Guardar() {
            try {
                string s;
                if ((s = this.presentadorTarifas.ValidarDatos()) != null) {
                    this.vista.MostrarMensaje(s, ETipoMensajeIU.ADVERTENCIA, null);
                    return;
                }
                if ((s = this.ValidarDatos()) != null) {
                    this.vista.MostrarMensaje(s, ETipoMensajeIU.ADVERTENCIA, null);
                    return;
                }

                List<TarifaPSLBO> lstTarifas = InterfazUsuarioADato();

                tarifaBR.ActualizarCompleto(dctx, lstTarifas, this.vista.TarifasAnteriores, new SeguridadBO(Guid.Empty, new UsuarioBO() { Id = this.vista.UsuarioID }, new AdscripcionBO() { UnidadOperativa = new UnidadOperativaBO() { Id = this.vista.UnidadOperativaID } }));
                this.vista.LimpiarSesion();
                this.vista.EstablecerPaqueteNavegacion(new TarifaPSLBO { TarifaPSLID = this.vista.TarifaPSLID });
                this.vista.RedirigirADetalle();
            } catch (Exception ex) {
                this.MostrarMensaje("Error al intentar actualizar los datos", ETipoMensajeIU.ERROR,
                                    nombreClase + ".Guardar:" + ex.Message);
            }
        }

        private string ValidarDatos() {
            string s = "";

            if (this.vista.TarifaPSLID == null)
                s += "Identificardor de la Tarifa";
            if (this.vista.TipoTarifa == null)
                s += "Tipo de Tarifa, ";
            if (this.vista.SucursalID == null)
                s += "Sucursal, ";
            if (String.IsNullOrEmpty(this.vista.CodigoMoneda))
                s += "Moneda, ";
            if (this.vista.ModeloID == null)
                s += "Modelo, ";
            if (this.vista.Estatus == null)
                s += "Estatus, ";

            if (s != null && s.Trim().CompareTo("") != 0)
                return "Los siguientes campos no pueden estar vacíos: \n" + s.Substring(0, s.Length - 2);

            return null;
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
        private void PermitirAplicarSoloEspecial(bool soloEspecial) {
            try {
                ETipoTarifa tipo =
                   (ETipoTarifa)Enum.Parse(typeof(ETipoTarifa), this.vista.TipoTarifa.ToString());
                if (tipo == ETipoTarifa.ESPECIAL && !soloEspecial)
                    this.vista.RedirigirSinPermisoAcceso();
            } catch (Exception ex) {

                throw new Exception("");
            }
        }

        private void MostrarMensaje(string mensaje, ETipoMensajeIU tipo, string detalle = null) {
            this.vista.MostrarMensaje(mensaje, tipo, detalle);
        }

        #region Métodos para el buscador
        public object PrepararBOBuscador(string catalogo) {
            object obj = null;

            switch (catalogo.ToUpper()) {
                case "SUCURSAL":
                    Facade.SDNI.BOF.SucursalBOF sucursal = new Facade.SDNI.BOF.SucursalBOF();
                    sucursal.UnidadOperativa = new UnidadOperativaBO { Id = this.vista.UnidadOperativaID };
                    sucursal.Nombre = this.vista.NombreSucursalNoAplica;
                    sucursal.Usuario = new UsuarioBO { Id = this.vista.UsuarioID };
                    obj = sucursal;
                    break;
            }

            return obj;
        }

        public void DesplegarResultadoBuscador(string catalogo, object selecto) {
            switch (catalogo.ToUpper()) {
                case "SUCURSAL":
                    SucursalBO sucursalNoAplica = (SucursalBO)selecto;
                    if (sucursalNoAplica != null && sucursalNoAplica.Id != null) {
                        if (sucursalNoAplica.Id == this.vista.SucursalID) {
                            this.MostrarMensaje(
                                "No se puede agregar la sucursal, ya que es la sucursal a la cual se esta configurando la tarifa",
                                ETipoMensajeIU.ADVERTENCIA);
                            this.vista.SucursalNoAplicaID = null;
                            this.vista.NombreSucursalNoAplica = null;
                            break;
                        }
                        if (this.vista.SessionListaSucursalSeleccionada.Find(suc => sucursalNoAplica.Id == suc.Id) != null) {
                            this.MostrarMensaje("La sucursal ya se encuentra agregada", ETipoMensajeIU.ADVERTENCIA);
                            this.vista.SucursalNoAplicaID = null;
                            this.vista.NombreSucursalNoAplica = null;
                            break;
                        }

                        this.vista.SucursalNoAplicaID = sucursalNoAplica.Id;
                    } else
                        this.vista.SucursalNoAplicaID = null;

                    if (sucursalNoAplica != null && sucursalNoAplica.Nombre != null)
                        this.vista.NombreSucursalNoAplica = sucursalNoAplica.Nombre;
                    else
                        this.vista.NombreSucursalNoAplica = null;
                    break;
            }
        }
        #endregion
        #endregion
    }
}