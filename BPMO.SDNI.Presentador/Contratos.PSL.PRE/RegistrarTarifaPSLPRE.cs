// Satisface al caso de uso CU005 - Catálogo de Tarifas de Renta Comercial Diaria.
// Construcción de Mejoras - Cobro de Rangos de Km y Horas.
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    public class RegistrarTarifaPSLPRE {
        #region Atributos
        private string nombreClase = "RegistrarTarifaPSLPRE";
        private IDataContext dctx;
        private IRegistrarTarifaPSLVIS vista;
        private ucTarifaPSLPRE presentadorTarifas;
        private TarifaPSLBR tarifaPSLBR;
        #endregion

        #region Constructor
        public RegistrarTarifaPSLPRE(IRegistrarTarifaPSLVIS vistaActual, IucTarifaPSLVIS vistaTarifas) {
            this.vista = vistaActual;
            presentadorTarifas = new ucTarifaPSLPRE(vistaTarifas);
            dctx = FacadeBR.ObtenerConexion();
            tarifaPSLBR = new TarifaPSLBR();
        }
        #endregion

        #region Métodos
        public void PrepararNuevo() {
            this.vista.LimpiarSesion();
            this.vista.PrecioCombustible = null;
            this.vista.ModoEdicion(true);
            this.vista.MostrarCapturaTarifas(false);
            this.vista.ModeloID = null;
            this.vista.NombreModelo = null;
            this.vista.ListaSucursalSeleccionada = null;

            this.presentadorTarifas.PrepararNuevo();
            this.vista.EstablecerOpcionesMoneda(ObtenerMonedas());
            this.vista.EstablecerOpcionesTarifaTurno(this.ObtenerTurnos());
            this.vista.EstablecerOpcionesPeriodoTarifa(this.ObtenerPeriodos());
            this.vista.PrecioCombustible = ObtenerPrecioCombustible();

            this.MostrarLeyendaSucursales(false);

            this.EstablecerSeguridad();
        }

        public void EstablecerSeguridad() {
            try {
                //Valida que el usuario y la unidad operativa no sean nulos
                if (this.vista.UC == null) throw new Exception("El identificador del usuario no debe ser nulo");
                if (this.vista.UnidadOperativaID == null)
                    throw new Exception("El identificador de la Unidad Operativa no debe ser nulo");
                // Creación del objeto seguridad
                UsuarioBO usuario = new UsuarioBO { Id = this.vista.UC };
                AdscripcionBO adscripcion = new AdscripcionBO
                {
                    UnidadOperativa = new UnidadOperativaBO { Id = this.vista.UnidadOperativaID }
                };
                SeguridadBO seguridad = new SeguridadBO(Guid.Empty, usuario, adscripcion);

                //consulta de acciones a la cual el usuario tiene permisos
                List<CatalogoBaseBO> acciones = FacadeBR.ConsultarAccion(dctx, seguridad);

                //Se valida si el usuario tiene permisos para registrar una nueva tarifa
                if (!this.ExisteAccion(acciones, "INSERTARCOMPLETO"))
                    this.vista.RedirigirSinPermisoAcceso();

                //Se valida si el usuario tiene permisos para aplicar tipos de tarifas
                this.vista.EstablecerOpcionesTipoTarifa(this.ObtenerTiposTarifa(!this.ExisteAccion(acciones, "UI APLICAR")));
            } catch (Exception ex) {
                throw new Exception(this.nombreClase + ".EstablecerAccionesSeguridad:" + ex.Message);
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
                if (this.vista.UC == null)
                    throw new Exception("No se puede identificar al usuario");
                if (this.vista.UnidadOperativaID == null)
                    throw new Exception("No se puede identificar la Unidad Operativa");

                var seguridad = new SeguridadBO(Guid.Empty, new UsuarioBO { Id = this.vista.UC },
                                                new AdscripcionBO { UnidadOperativa = new UnidadOperativaBO { Id = this.vista.UnidadOperativaID } });

                return FacadeBR.ConsultarSucursalesSeguridad(dctx, seguridad);
            } catch (Exception ex) {
                throw new Exception(this.nombreClase + ".SucursalesSeguridad: " + ex.Message);
            }
        }
        private Dictionary<string, string> ObtenerMonedas() {
            try {
                Dictionary<string, string> listaMonedas = new Dictionary<string, string>();
                listaMonedas.Add("-1", "Seleccione una opción");
                List<MonedaBO> monedas = FacadeBR.ConsultarMoneda(dctx, new MonedaBO { Activo = true });
                if (monedas != null) {
                    foreach (var monedaBo in monedas) {
                        listaMonedas.Add(monedaBo.Codigo, monedaBo.Nombre);
                    }
                }
                return listaMonedas;
            } catch (Exception ex) {

                throw new Exception(this.nombreClase + ".ListaMonedas:Error al consultar las monedas." + ex.Message);
            }
        }
        private Dictionary<int, string> ObtenerTiposTarifa(bool aplicarSoloEspecial) {
            Dictionary<int, string> TiposTarifas = new Dictionary<int, string>();
            TiposTarifas.Add(-1, "Seleccione una opción");
            foreach (var tipo in Enum.GetValues(typeof(ETipoTarifa))) {
                if (((int)tipo) != 3) {
                    if (aplicarSoloEspecial) {
                        if (((int)tipo) == 2)
                            TiposTarifas.Add(((int)tipo), Enum.GetName(typeof(ETipoTarifa), tipo));
                    } else
                        TiposTarifas.Add(((int)tipo), Enum.GetName(typeof(ETipoTarifa), tipo));
                }
            }
            return TiposTarifas;
        }

        /// <summary>
        /// Método para obtener un diccionario con los valores de los turnos que se envía como parámetro para el llenado del combo correspondiente
        /// </summary>
        /// <returns>Diccionario de tipo string,string</returns>
        private Dictionary<string, string> ObtenerTurnos() {
            try {
                Dictionary<string, string> listaTurnos = new Dictionary<string, string>();
                listaTurnos.Add("-1", "SELECCIONE UNA OPCIÓN");
                Type type = this.vista.UnidadOperativaID == (int)ETipoEmpresa.Construccion ? typeof(ETarifaTurnoConstruccion) : 
                    this.vista.UnidadOperativaID == (int)ETipoEmpresa.Generacion ? typeof(ETarifaTurnoGeneracion) : typeof(ETarifaTurnoEquinova);
                Array values = Enum.GetValues(type);
                foreach (int value in values) {
                    var memInfo = type.GetMember(type.GetEnumName(value));
                    var display = memInfo[0]
                        .GetCustomAttributes(typeof(DescriptionAttribute), false)
                        .FirstOrDefault() as DescriptionAttribute;

                    if (display != null) {
                        listaTurnos.Add(value.ToString(), display.Description);
                    }
                }

                return listaTurnos;
            } catch (Exception ex) {

                throw new Exception(this.nombreClase + ".ListaTurnos:Error al consultar los turnos");
            }
        }

        /// <summary>
        /// Método para obtener un diccionario con los valores de los periodos que se envía como parámetro para el llenado del combo correspondiente
        /// </summary>
        /// <returns>Diccionario de tipo string,string</returns>
        private Dictionary<string, string> ObtenerPeriodos() {
            try {
                Dictionary<string, string> listaPeriodos = new Dictionary<string, string>();
                listaPeriodos.Add("-1", "SELECCIONE UNA OPCIÓN");
                Type type = typeof(EPeriodosTarifa);
                Array values = Enum.GetValues(type);
                foreach (int value in values) {
                    var memInfo = type.GetMember(type.GetEnumName(value));
                    var display = memInfo[0]
                        .GetCustomAttributes(typeof(DescriptionAttribute), false)
                        .FirstOrDefault() as DescriptionAttribute;

                    if (display != null) {
                        listaPeriodos.Add(value.ToString(), display.Description);
                    }
                }

                return listaPeriodos;
            } catch (Exception ex) {

                throw new Exception(this.nombreClase + ".ListaPeriodos:Error al consultar los períodos");
            }
        }

        public void Cancelar() {
            this.vista.RedirigirAConsulta();
        }
        public void Guardar() {
            try {
                string s;
                if ((s = this.ValidarCamposConfiguracion()) != null) {
                    this.vista.MostrarMensaje(s, ETipoMensajeIU.ADVERTENCIA, null);
                    return;
                }
                if ((s = this.ValidarCampos()) != null) {
                    this.vista.MostrarMensaje(s, ETipoMensajeIU.ADVERTENCIA, null);
                    return;
                }
                if ((s = this.presentadorTarifas.ValidarDatos()) != null) {
                    this.vista.MostrarMensaje(s, ETipoMensajeIU.ADVERTENCIA, null);
                    return;
                }

                #region Logica de Inserción
                List<TarifaPSLBO> lst = this.InterfazUsuarioADato();
                tarifaPSLBR.InsertarCompleto(dctx, lst, new SeguridadBO(Guid.Empty, new UsuarioBO() { Id = this.vista.UC }, new AdscripcionBO() { UnidadOperativa = new UnidadOperativaBO() { Id = this.vista.UnidadOperativaID } }));
                TarifaPSLBO tarifaTemp = lst.FindLast(t => t.Sucursal.Id == this.vista.SucursalID);
                List<TarifaPSLBO> lstTemp = tarifaPSLBR.Consultar(dctx, tarifaTemp);

                if (lstTemp.Count != 0)
                    tarifaTemp.TarifaPSLID = lstTemp[0].TarifaPSLID;
                #endregion

                this.vista.LimpiarSesion();
                this.vista.EstablecerPaqueteNavegacion((object)tarifaTemp);
                this.vista.RedirigirADetalle();
            } catch (Exception ex) {
                this.MostrarMensaje("Inconsistencias al momento de registrar la tarifa configurada",
                                    ETipoMensajeIU.ERROR, this.nombreClase + ".Guardar:" + ex.Message);
            }
        }

        private List<TarifaPSLBO> InterfazUsuarioADato() {
            try {
                if (this.vista.UnidadOperativaID == null) throw new Exception("El identificador de la unidad no puede ser nulo");
                if (this.vista.AplicarOtrasSucursales == null)
                    throw new Exception("No se puede obtener el valor del control Aplicar a otras sucursales");

                List<TarifaPSLBO> lstTarifas = new List<TarifaPSLBO>();
                TarifaPSLBO tarifa = this.presentadorTarifas.InterfazUsuarioADato();
                ETipoTarifa tipo = (ETipoTarifa)Enum.Parse(typeof(ETipoTarifa), this.vista.TipoTarifaSeleccionada.ToString());
                if (tipo == ETipoTarifa.ESPECIAL) {
                    //tarifa.Cliente = new CuentaClienteIdealeaseBO { Id = this.vista.ClienteID };
                }
                tarifa.TipoTarifaID = tipo.ToString();
                tarifa.Modelo = new ModeloBO { Id = this.vista.ModeloID };
                tarifa.Divisa = new DivisaBO
                {
                    MonedaDestino = new MonedaBO { Codigo = this.vista.MonedaSeleccionada }
                };
                Type type = this.vista.UnidadOperativaID == (int)ETipoEmpresa.Construccion ? typeof(ETarifaTurnoConstruccion) :
                    this.vista.UnidadOperativaID == (int)ETipoEmpresa.Generacion ? typeof(ETarifaTurnoGeneracion) : typeof(ETarifaTurnoEquinova);
                tarifa.TarifaTurno = (Enum)Enum.ToObject(type, this.vista.TarifaTurnoSeleccionada);
                tarifa.PeriodoTarifa = (EPeriodosTarifa)this.vista.PeriodoTarifaSeleccionada;
                tarifa.Tarifa = this.vista.Tarifa;
                tarifa.TarifaHrAdicional = this.vista.TarifaHrAdicional;
                tarifa.Activo = true;
                tarifa.Auditoria = new AuditoriaBO
                {
                    UC = this.vista.UC,
                    FC = this.vista.FC,
                    FUA = this.vista.FC,
                    UUA = this.vista.UC
                };
                //tarifa.Vigencia = this.vista.Vigencia;

                if (this.vista.AplicarOtrasSucursales != null && this.vista.AplicarOtrasSucursales == true) {
                    List<SucursalBO> sucursalesAplicar = SucursalesSeguridad();
                    foreach (SucursalBO suc in this.vista.ListaSucursalSeleccionada) {
                        if (sucursalesAplicar.Find(s => suc.Id == s.Id) != null)
                            sucursalesAplicar.RemoveAll(s => s.Id == suc.Id);

                    }
                    foreach (SucursalBO suc in sucursalesAplicar) {
                        TarifaPSLBO tarifaTemp = new TarifaPSLBO(tarifa) { Sucursal = suc };
                        lstTarifas.Add(tarifaTemp);
                    }
                } else {
                    tarifa.Sucursal = new SucursalBO
                    {
                        Id = this.vista.SucursalID,
                        UnidadOperativa = new UnidadOperativaBO { Id = this.vista.UnidadOperativaID }
                    };
                    lstTarifas.Add(tarifa);
                }
                return lstTarifas;
            } catch (Exception ex) {
                throw new Exception(this.nombreClase + ".InterfazUsuarioADato:Error al intentar obtener obtener los datos a guardar. " + ex.Message);
            }
        }

        public string ValidarCamposConsultaCliente() {
            string s = null;
            if (!String.IsNullOrEmpty(s))
                s = "Los Siguientes campos no pueden estar vacíos" + s;
            return s;
        }
        public void CapturarTarifas() {
            string s;
            if ((s = this.ValidarCamposConfiguracion()) != null) {
                this.vista.MostrarMensaje(s, ETipoMensajeIU.ADVERTENCIA, null);
                return;
            }
            if (this.ExisteTarifa()) {
                this.MostrarMensaje("Ya existe una tarifa configurado con los datos proporcianados", ETipoMensajeIU.ADVERTENCIA);
                return;
            }

            this.vista.MostrarCapturaTarifas(true);
            this.vista.ModoRegistrarTarifa(true);

            //Si la sucursal seleccionada es matriz entonces puede aplicar tarifas a varias sucursales
            List<SucursalBO> lst = FacadeBR.ConsultarSucursal(this.dctx, new SucursalBO() { Id = this.vista.SucursalID });
            bool esMatriz = (lst.Count > 0 && lst[0].Matriz != null && lst[0].Matriz == true);

            this.vista.PermitirAgregarSucursales(esMatriz);
            this.MostrarLeyendaSucursales(esMatriz);
            this.vista.AplicarOtrasSucursales = esMatriz;
        }
        private bool ExisteTarifa() {
            try {
                TarifaPSLBO tarifaTemp = new TarifaPSLBO();
                tarifaTemp.Sucursal = new SucursalBO { Id = this.vista.SucursalID };
                tarifaTemp.Modelo = new ModeloBO { Id = this.vista.ModeloID };
                tarifaTemp.Divisa = new DivisaBO { MonedaDestino = new MonedaBO { Codigo = this.vista.MonedaSeleccionada } };
                tarifaTemp.Tipo = (ETipoTarifa)Enum.Parse(typeof(ETipoTarifa), this.vista.TipoTarifaSeleccionada.ToString());
                tarifaTemp.PeriodoTarifa = (EPeriodosTarifa)this.vista.PeriodoTarifaSeleccionada;
                Type type = this.vista.UnidadOperativaID == (int)ETipoEmpresa.Construccion ? typeof(ETarifaTurnoConstruccion) :
                    this.vista.UnidadOperativaID == (int)ETipoEmpresa.Generacion ? typeof(ETarifaTurnoGeneracion) : typeof(ETarifaTurnoEquinova);
                tarifaTemp.TarifaTurno = (Enum)Enum.ToObject(type, this.vista.TarifaTurnoSeleccionada);
                List<TarifaPSLBO> lstTarifas = tarifaPSLBR.Consultar(dctx, tarifaTemp);
                if (lstTarifas.Count == 0)
                    return false;
                return true;
            } catch (Exception ex) {
                throw new Exception(this.nombreClase + ".ExisteTarifa: " + ex.Message);
            }
        }

        public string ValidarCamposConfiguracion() {
            string s = "";
            if (this.vista.SucursalID == null)
                s += "Sucursal, ";
            if (this.vista.ModeloID == null && String.IsNullOrEmpty(this.vista.NombreModelo))
                s += "Modelo, ";
            if (String.IsNullOrEmpty(this.vista.MonedaSeleccionada))
                s += "Moneda, ";
            if (this.vista.TipoTarifaSeleccionada == null)
                s += "Tipo de Tarifa, ";
            if (this.vista.TarifaTurnoSeleccionada == null)
                s += "Turno, ";
            if (this.vista.PeriodoTarifaSeleccionada == null)
                s += "Periodo, ";

            if (s != null && s.Trim().CompareTo("") != 0)
                return "Los siguientes campos no pueden estar vacíos: \n" + s.Substring(0, s.Length - 2);

            return null;
        }
        public string ValidarCampos() {
            string s = "";
            if (this.vista.TipoTarifaSeleccionada != null) {
                if (((ETipoTarifa)Enum.Parse(typeof(ETipoTarifa), this.vista.TipoTarifaSeleccionada.ToString())) ==
                    ETipoTarifa.ESPECIAL) {
                    if (this.vista.PeriodoTarifaSeleccionada == null)
                        s += "Periodo, ";
                }
            }

            if (s != null && s.Trim().CompareTo("") != 0)
                return "Los siguientes campos no pueden estar vacíos: \n" + s.Substring(0, s.Length - 2);

            return null;
        }
        public string ValidarCamposConsultaModelo() {
            string s = "";
            if (String.IsNullOrEmpty(this.vista.NombreModelo))
                s += "Nombre del Modelo, ";

            if (s != null && s.Trim().CompareTo("") != 0)
                return "Los siguientes campos no pueden estar vacíos: \n" + s.Substring(0, s.Length - 2);

            return null;
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
                    if (sucursalAgregar.Id == null)
                        throw new Exception("La sucursal Seleccionada no es valida");

                    if (String.IsNullOrEmpty(sucursalAgregar.Nombre))
                        throw new Exception("El nombre de la sucursal no se puede obtener");
                    if (sucursalAgregar.Id == this.vista.SucursalID) {
                        this.MostrarMensaje("No se puede agregar la sucursal, ya que es la sucursal a la cual se esta configurando la tarifa", ETipoMensajeIU.ADVERTENCIA);
                        this.vista.NombreSucursalNoAplica = null;
                        this.vista.SucursalNoAplicaID = null;
                        return;
                    }
                    if (this.vista.ListaSucursalSeleccionada.Find(suc => sucursalAgregar.Id == suc.Id) == null) {
                        var sucursales = new List<SucursalBO>(this.vista.ListaSucursalSeleccionada) { sucursalAgregar };
                        this.vista.ListaSucursalSeleccionada = sucursales;
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
                    if (this.vista.ListaSucursalSeleccionada.Find(suc => sucursal.Id == suc.Id) != null) {
                        var sucursales = new List<SucursalBO>(vista.ListaSucursalSeleccionada);
                        sucursales.Remove(sucursal);
                        this.vista.ListaSucursalSeleccionada = sucursales;
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
                    this.vista.ListaSucursalSeleccionada = new List<SucursalBO>();
                    this.vista.MostrarAplicarSucursal(false);
                } else {
                    this.vista.ListaSucursalSeleccionada = new List<SucursalBO>();
                    this.vista.NombreSucursalNoAplica = null;
                    this.vista.SucursalNoAplicaID = null;
                    this.vista.MostrarAplicarSucursal(true);
                }
            } catch (Exception ex) {

                throw new Exception(nombreClase + ".AplicarOtrasSucursales:Inconsistencia la configurar los datos a mostrar." + ex.Message);
            }
        }
        private decimal? ObtenerPrecioCombustible() {
            try {
                if (this.vista.UnidadOperativaID == null)
                    throw new Exception("El indentificador de la unidad operativa no debe ser nulo");
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

        private void MostrarMensaje(string mensaje, ETipoMensajeIU tipo, string detalle = null) {
            this.vista.MostrarMensaje(mensaje, tipo, detalle);
        }

        #region Métodos Buscador
        public object PrepararBOBuscador(string catalogo) {
            object obj = null;

            switch (catalogo.ToUpper()) {
                case "MODELO":
                    ModeloBO modelo = new ModeloBO();
                    modelo.Auditoria = new AuditoriaBO();
                    modelo.Marca = new MarcaBO();
                    modelo.Nombre = this.vista.NombreModelo;
                    modelo.Activo = true;

                    obj = modelo;
                    break;
                case "SUCURSAL":
                    Facade.SDNI.BOF.SucursalBOF sucursal = new Facade.SDNI.BOF.SucursalBOF();
                    sucursal.UnidadOperativa = new UnidadOperativaBO { Id = this.vista.UnidadOperativaID };
                    sucursal.Nombre = this.vista.NombreSucursal;
                    sucursal.Usuario = new UsuarioBO { Id = this.vista.UC };
                    obj = sucursal;
                    break;
                case "SUCURSALNOAPLICA":
                    Facade.SDNI.BOF.SucursalBOF sucursalNoAplica = new Facade.SDNI.BOF.SucursalBOF();
                    sucursalNoAplica.UnidadOperativa = new UnidadOperativaBO { Id = this.vista.UnidadOperativaID };
                    sucursalNoAplica.Nombre = this.vista.NombreSucursalNoAplica;
                    sucursalNoAplica.Usuario = new UsuarioBO { Id = this.vista.UC };
                    obj = sucursalNoAplica;
                    break;
            }

            return obj;
        }

        public void DesplegarResultadoBuscador(string catalogo, object selecto) {
            switch (catalogo.ToUpper()) {
                case "MODELO":
                    #region Desplegar Modelo
                    ModeloBO modelo = (ModeloBO)selecto;

                    this.vista.ModeloID = modelo != null && modelo.Id != null ? modelo.Id : null;

                    this.vista.NombreModelo = modelo != null && modelo.Nombre != null ? modelo.Nombre : null;

                    #endregion
                    break;
                case "SUCURSAL":
                    SucursalBO sucursal = (SucursalBO)selecto;
                    if (sucursal != null && sucursal.Id != null)
                        this.vista.SucursalID = sucursal.Id;
                    else
                        this.vista.SucursalID = null;

                    if (sucursal != null && sucursal.Nombre != null)
                        this.vista.NombreSucursal = sucursal.Nombre;
                    else
                        this.vista.NombreSucursal = null;
                    break;
                case "SUCURSALNOAPLICA":
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
                        if (this.vista.ListaSucursalSeleccionada.Find(suc => sucursalNoAplica.Id == suc.Id) != null) {
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
                    this.vista.ModoRegistrarTarifa(true);
                    break;
            }
        }

        #endregion
        #endregion
    }
}