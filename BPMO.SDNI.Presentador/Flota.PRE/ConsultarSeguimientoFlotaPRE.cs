//Satisface al CU081 - Consultar Seguimiento Flota
//Satisface la solicitud de cambio SC0006

using System;
using System.Collections.Generic;
using BPMO.Basicos.BO;
using BPMO.Facade.SDNI.BOF;
using BPMO.Facade.SDNI.BR;
using BPMO.Patterns.Creational.DataContext;
using BPMO.Primitivos.Enumeradores;
using BPMO.SDNI.Equipos.BO;
using BPMO.SDNI.Equipos.BOF;
using BPMO.SDNI.Flota.BOF;
using BPMO.SDNI.Flota.BR;
using BPMO.SDNI.Flota.VIS;
using BPMO.Servicio.Catalogos.BO;
using BPMO.SDNI.Comun.BO;

namespace BPMO.SDNI.Flota.PRE {
    public class ConsultarSeguimientoFlotaPRE {
        #region Atributos
        private IConsultarSeguimientoFlotaVIS vista;
        private IDataContext dctx = null;
        private SeguimientoFlotaBR controlador;

        private string nombreClase = "ConsultarSeguimientoFlotaPRE";
        #endregion

        #region Constructores
        public ConsultarSeguimientoFlotaPRE(IConsultarSeguimientoFlotaVIS view) {
            try {
                this.vista = view;
                dctx = FacadeBR.ObtenerConexion();

                this.controlador = new SeguimientoFlotaBR();
            } catch (Exception ex) {
                vista.MostrarMensaje("Inconsistencias en la construcción del presentador", ETipoMensajeIU.ERROR, nombreClase + ".ConsultarSeguimientoFlotaPRE: " + ex.Message);
            }
        }
        #endregion

        #region Métodos
        public void PrepararBusqueda() {
            this.vista.LimpiarSesion();

            this.EstablecerInformacionInicial();

            this.EstablecerFiltros();

            this.EstablecerSeguridad();
        }
        private void EstablecerInformacionInicial() {
            #region Estatus de la Unidad
            Dictionary<int, string> lstEstatus = new Dictionary<int, string>();
            lstEstatus.Add(0, "Acta Terminada");
            lstEstatus.Add(1, "Disponible");
            lstEstatus.Add(2, "Rentada");
            lstEstatus.Add(3, "Baja");
            lstEstatus.Add(4, "Seminuevo");
            #region SC0006 - Adición de nuevo estatus de siniestro
            lstEstatus.Add(5, "Siniestro");
            #endregion

            this.vista.EstablecerOpcionesEstatus(lstEstatus);
            #endregion

            #region [RQM 13285- Modificación - Integración Construcción y Generacción]
            this.vista.EnumTipoEmpresa = (ETipoEmpresa)this.vista.UnidadOperativaID;
            if (this.vista.EnumTipoEmpresa == ETipoEmpresa.Idealease)
                this.CargarAreas(this.vista.EnumTipoEmpresa);
            if (this.vista.EnumTipoEmpresa == ETipoEmpresa.Construccion)
                this.CargarAreas(this.vista.EnumTipoEmpresa);
            if (this.vista.EnumTipoEmpresa == ETipoEmpresa.Generacion)
                this.CargarAreas(this.vista.EnumTipoEmpresa);
            if (this.vista.EnumTipoEmpresa == ETipoEmpresa.Equinova)
                this.CargarAreas(this.vista.EnumTipoEmpresa);
            #endregion

            this.vista.AreaID = null;
            this.vista.EstatusID = null;
            this.vista.FechaAltaFinal = null;
            this.vista.FechaAltaInicial = null;
            this.vista.FechaBajaFinal = null;
            this.vista.FechaBajaInicial = null;
            this.vista.MarcaID = null;
            this.vista.MarcaNombre = null;
            this.vista.ModeloID = null;
            this.vista.ModeloNombre = null;
            this.vista.NumeroEconomico = null;
            this.vista.Propietario = null;
            this.vista.SucursalID = null;
            this.vista.SucursalNombre = null;
            this.vista.TipoUnidadID = null;
            this.vista.TipoUnidadNombre = null;
            this.vista.VIN = null;
        }

        private void CargarAreas(ETipoEmpresa empresa) {
            switch (empresa) {
                case ETipoEmpresa.Idealease:
                    Dictionary<string, object> tipoAreaIdealease = new Dictionary<string, object>();
                    tipoAreaIdealease.Add(EArea.CM.ToString(), (int)EArea.CM);
                    tipoAreaIdealease.Add(EArea.FSL.ToString(), (int)EArea.FSL);
                    tipoAreaIdealease.Add(EArea.RD.ToString(), (int)EArea.RD);
                    tipoAreaIdealease.Add(EArea.SD.ToString(), (int)EArea.SD);
                    tipoAreaIdealease.Add(EArea.Seminuevos.ToString(), (int)EArea.Seminuevos);
                    this.vista.CargarAreaIdealease(tipoAreaIdealease);
                    break;
                case ETipoEmpresa.Construccion:
                    Dictionary<string, object> tipoAreaConstruccion = new Dictionary<string, object>();
                    tipoAreaConstruccion.Add(EAreaConstruccion.RE.ToString(), (int)EAreaConstruccion.RE);
                    tipoAreaConstruccion.Add(EAreaConstruccion.RO.ToString(), (int)EAreaConstruccion.RO);
                    tipoAreaConstruccion.Add(EAreaConstruccion.ROC.ToString(), (int)EAreaConstruccion.ROC);
                    this.vista.CargarAreaConstruccion(tipoAreaConstruccion);
                    break;
                case ETipoEmpresa.Generacion:
                    Dictionary<string, object> tipoAreaGeneracion = new Dictionary<string, object>();
                    tipoAreaGeneracion.Add(EAreaGeneracion.RE.ToString(), (int)EAreaGeneracion.RE);
                    tipoAreaGeneracion.Add(EAreaGeneracion.RO.ToString(), (int)EAreaGeneracion.RO);
                    tipoAreaGeneracion.Add(EAreaGeneracion.ROC.ToString(), (int)EAreaGeneracion.ROC);
                    this.vista.CargarAreaGeneracion(tipoAreaGeneracion);
                    break;
                case ETipoEmpresa.Equinova:
                    Dictionary<string, object> tipoAreaEquinova = new Dictionary<string, object>();
                    tipoAreaEquinova.Add(EAreaEquinova.RE.ToString(), (int)EAreaEquinova.RE);
                    tipoAreaEquinova.Add(EAreaEquinova.RO.ToString(), (int)EAreaEquinova.RO);
                    tipoAreaEquinova.Add(EAreaEquinova.ROC.ToString(), (int)EAreaEquinova.ROC);
                    this.vista.CargarAreaEquinova(tipoAreaEquinova);
                    break;
            }
        }

        private void EstablecerFiltros() {
            try {
                Dictionary<string, object> paquete = this.vista.ObtenerPaqueteNavegacion("FiltrosSeguimientoFlota") as Dictionary<string, object>;
                if (paquete != null) {
                    if (paquete.ContainsKey("ObjetoFiltro")) {
                        if (paquete["ObjetoFiltro"].GetType() == typeof(FlotaBOF))
                            this.DatoAInterfazUsuario(paquete["ObjetoFiltro"]);
                        else
                            throw new Exception("Se esperaba un objeto FlotaBOF, el objeto proporcionado no cumple con esta característica, intente de nuevo por favor.");
                    }
                    if (paquete.ContainsKey("Bandera")) {
                        if ((bool)paquete["Bandera"])
                            this.ConsultarFlota();
                    }
                }
                this.vista.LimpiarPaqueteNavegacion("FiltrosSeguimientoFlota");
            } catch (Exception ex) {
                throw new Exception(this.nombreClase + ".EstablecerFiltros: " + ex.Message);
            }
        }

        private void EstablecerSeguridad() {
            try {
                //se valida que los datos del usuario y la unidad operativa no sean nulos
                if (this.vista.UsuarioID == null) throw new Exception("El identificador del usuario no debe ser nulo");
                if (this.vista.UnidadOperativaID == null) throw new Exception("El identificador de la Unidad Operativa no debe ser nulo ");

                UsuarioBO usr = new UsuarioBO { Id = this.vista.UsuarioID };
                AdscripcionBO adscripcion = new AdscripcionBO { UnidadOperativa = new UnidadOperativaBO { Id = this.vista.UnidadOperativaID } };
                SeguridadBO seguridadBO = new SeguridadBO(Guid.Empty, usr, adscripcion);

                //Se obtienen las acciones a las cuales el usuario tiene permiso en este proceso
                List<CatalogoBaseBO> lst = FacadeBR.ConsultarAccion(dctx, seguridadBO);

                //Se valida si el usuario tiene permiso para consultar
                if (!this.ExisteAccion(lst, "CONSULTAR"))
                    this.vista.RedirigirSinPermisoAcceso();
                //Se valida si el usuario tiene permiso para cambiar de sucursal los equipos aliados
                if (!this.ExisteAccion(lst, "UI ACTUALIZAREQUIPOALIADOSUCURSAL"))
                    this.vista.PermitirRealizarCambioSucursalEquipoAliado(false);
            } catch (Exception ex) {
                throw new Exception(nombreClase + ".EstablecerSeguridad:" + ex.Message);
            }
        }
        private bool ExisteAccion(List<CatalogoBaseBO> acciones, string accion) {
            if (acciones != null && acciones.Count > 0 && acciones.Exists(p => p != null && p.Nombre != null && p.Nombre.Trim().ToUpper() == accion.Trim().ToUpper()))
                return true;

            return false;
        }

        private Object InterfazUsuarioADato() {
            FlotaBOF bo = new FlotaBOF();
            bo.Unidad = new Equipos.BO.UnidadBO();
            bo.Unidad.Sucursal = new SucursalBO();
            bo.Unidad.Sucursal.UnidadOperativa = new UnidadOperativaBO();
            bo.Unidad.TipoEquipoServicio = new TipoUnidadBO();
            bo.Unidad.Modelo = new ModeloBO();
            bo.Unidad.Modelo.Marca = new MarcaBO();
            bo.EstatusUnidad = new List<EEstatusUnidad>();
            bo.Areas = new List<Enum>();

            if (this.vista.VIN != null)
                bo.Unidad.NumeroSerie = this.vista.VIN;
            if (this.vista.NumeroEconomico != null)
                bo.Unidad.NumeroEconomico = this.vista.NumeroEconomico;

            if (this.vista.SucursalID != null) {
                bo.Unidad.Sucursal.Id = this.vista.SucursalID;
                bo.Unidad.Sucursal.Nombre = this.vista.SucursalNombre;
            } else
                bo.Sucursales = FacadeBR.ConsultarSucursalesSeguridad(this.dctx, new SeguridadBO(Guid.Empty, new UsuarioBO() { Id = this.vista.UsuarioID }, new AdscripcionBO { UnidadOperativa = new UnidadOperativaBO() { Id = this.vista.UnidadOperativaID } }));

            if (this.vista.UnidadOperativaID != null)
                bo.Unidad.Sucursal.UnidadOperativa.Id = this.vista.UnidadOperativaID;
            if (this.vista.MarcaID != null)
                bo.Unidad.Modelo.Marca.Id = this.vista.MarcaID;
            if (this.vista.MarcaNombre != null)
                bo.Unidad.Modelo.Marca.Nombre = this.vista.MarcaNombre;
            if (this.vista.ModeloID != null)
                bo.Unidad.Modelo.Id = this.vista.ModeloID;
            if (this.vista.ModeloNombre != null)
                bo.Unidad.Modelo.Nombre = this.vista.ModeloNombre;
            if (this.vista.TipoUnidadID != null)
                bo.Unidad.TipoEquipoServicio.Id = this.vista.TipoUnidadID;
            if (this.vista.TipoUnidadNombre != null)
                bo.Unidad.TipoEquipoServicio.Nombre = this.vista.TipoUnidadNombre;
            if (this.vista.AreaID != null) {
                #region [RQM 13285- Integración Construcción y Generacción]

                this.vista.EnumTipoEmpresa = (ETipoEmpresa)this.vista.UnidadOperativaID;
                if (this.vista.EnumTipoEmpresa == ETipoEmpresa.Idealease)
                    bo.Areas.Add((EArea)Enum.Parse(typeof(EArea), this.vista.AreaID.ToString()));
                if (this.vista.EnumTipoEmpresa == ETipoEmpresa.Generacion)
                    bo.Areas.Add((EAreaGeneracion)Enum.Parse(typeof(EAreaGeneracion), this.vista.AreaID.ToString()));
                if (this.vista.EnumTipoEmpresa == ETipoEmpresa.Construccion)
                    bo.Areas.Add((EAreaConstruccion)Enum.Parse(typeof(EAreaConstruccion), this.vista.AreaID.ToString()));
                if (this.vista.EnumTipoEmpresa == ETipoEmpresa.Equinova)
                    bo.Areas.Add((EAreaEquinova)Enum.Parse(typeof(EAreaEquinova), this.vista.AreaID.ToString()));

                #endregion
            }

            if (this.vista.Propietario != null)
                bo.Unidad.Propietario = this.vista.Propietario;
            if (this.vista.EstatusID != null) {
                switch (this.vista.EstatusID) {
                    case 0:
                        bo.EstatusUnidad.Add(EEstatusUnidad.Terminada);
                        break;
                    case 1:
                        bo.EstatusUnidad.Add(EEstatusUnidad.Disponible);
                        break;
                    case 2:
                        bo.EstatusUnidad.Add(EEstatusUnidad.ConCliente);
                        bo.EstatusUnidad.Add(EEstatusUnidad.Entregada);
                        bo.EstatusUnidad.Add(EEstatusUnidad.EsperandoSalida);
                        bo.EstatusUnidad.Add(EEstatusUnidad.Pedida);
                        break;
                    case 3:
                        bo.EstatusUnidad.Add(EEstatusUnidad.Baja);
                        break;
                    case 4:
                        bo.EstatusUnidad.Add(EEstatusUnidad.Seminuevo);
                        break;
                    #region SC0006 - Adición de filtro de búsqueda para estatus de Siniestro
                    case 5:
                        bo.EstatusUnidad.Add(EEstatusUnidad.Siniestro);
                        break;
                    #endregion
                }
            }

            bo.FechaAltaFinal = this.vista.FechaAltaFinal;
            bo.FechaAltaInicial = this.vista.FechaAltaInicial;
            bo.FechaBajaFinal = this.vista.FechaBajaFinal;
            bo.FechaBajaInicial = this.vista.FechaBajaInicial;
            bo.Unidad.Sucursal.UnidadOperativa.Id = this.vista.UnidadOperativaID;

            return bo;
        }
        private void DatoAInterfazUsuario(Object obj) {
            FlotaBOF bo = (FlotaBOF)obj;
            if (bo == null) bo = new FlotaBOF();
            if (bo.Unidad == null) bo.Unidad = new Equipos.BO.UnidadBO();
            if (bo.Unidad.Sucursal == null) bo.Unidad.Sucursal = new SucursalBO();
            if (bo.Unidad.Modelo == null) bo.Unidad.Modelo = new ModeloBO();
            if (bo.Unidad.Modelo.Marca == null) bo.Unidad.Modelo.Marca = new MarcaBO();
            if (bo.Unidad.TipoEquipoServicio == null) bo.Unidad.TipoEquipoServicio = new TipoUnidadBO();

            if (!String.IsNullOrEmpty(bo.Unidad.NumeroSerie) && !String.IsNullOrWhiteSpace(bo.Unidad.NumeroSerie))
                this.vista.VIN = bo.Unidad.NumeroSerie;
            else
                this.vista.VIN = null;
            if (!String.IsNullOrEmpty(bo.Unidad.NumeroEconomico) && !String.IsNullOrWhiteSpace(bo.Unidad.NumeroEconomico))
                this.vista.NumeroEconomico = bo.Unidad.NumeroEconomico;
            else
                this.vista.NumeroEconomico = null;

            this.vista.SucursalID = bo.Unidad.Sucursal.Id;
            if (!String.IsNullOrEmpty(bo.Unidad.Sucursal.Nombre) && !String.IsNullOrWhiteSpace(bo.Unidad.Sucursal.Nombre))
                this.vista.SucursalNombre = bo.Unidad.Sucursal.Nombre;
            else
                this.vista.SucursalNombre = null;

            this.vista.ModeloID = bo.Unidad.Modelo.Id;
            if (!String.IsNullOrEmpty(bo.Unidad.Modelo.Nombre) && !String.IsNullOrWhiteSpace(bo.Unidad.Modelo.Nombre))
                this.vista.ModeloNombre = bo.Unidad.Modelo.Nombre;
            else
                this.vista.ModeloNombre = null;

            this.vista.MarcaID = bo.Unidad.Modelo.Marca.Id;
            if (!String.IsNullOrEmpty(bo.Unidad.Modelo.Marca.Nombre) && !String.IsNullOrWhiteSpace(bo.Unidad.Modelo.Marca.Nombre))
                this.vista.MarcaNombre = bo.Unidad.Modelo.Marca.Nombre;
            else
                this.vista.MarcaNombre = null;

            this.vista.TipoUnidadID = bo.Unidad.TipoEquipoServicio.Id;
            if (!String.IsNullOrEmpty(bo.Unidad.TipoEquipoServicio.Nombre) && !String.IsNullOrWhiteSpace(bo.Unidad.TipoEquipoServicio.Nombre))
                this.vista.TipoUnidadNombre = bo.Unidad.TipoEquipoServicio.Nombre;
            else
                this.vista.TipoUnidadNombre = null;

            if (bo.Areas != null && bo.Areas.Count > 0) {
                #region [RQM 13285- Integración Construcción y Generacción]

                this.vista.EnumTipoEmpresa = (ETipoEmpresa)this.vista.UnidadOperativaID;
                if (this.vista.EnumTipoEmpresa == ETipoEmpresa.Idealease)
                    this.vista.AreaID = (int)(EArea)bo.Areas[0];
                if (this.vista.EnumTipoEmpresa == ETipoEmpresa.Generacion)
                    this.vista.AreaID = (int)(EAreaGeneracion)bo.Areas[0];
                if (this.vista.EnumTipoEmpresa == ETipoEmpresa.Construccion)
                    this.vista.AreaID = (int)(EAreaConstruccion)bo.Areas[0];
                if (this.vista.EnumTipoEmpresa == ETipoEmpresa.Equinova)
                    this.vista.AreaID = (int)(EAreaEquinova)bo.Areas[0];
                #endregion
            }

            if (!String.IsNullOrEmpty(bo.Unidad.Propietario) && !String.IsNullOrWhiteSpace(bo.Unidad.Propietario))
                this.vista.Propietario = bo.Unidad.Propietario;
            else
                this.vista.Propietario = null;

            if (bo.EstatusUnidad != null) {
                if (bo.EstatusUnidad.Contains(EEstatusUnidad.Terminada))
                    this.vista.EstatusID = 0;
                else {
                    if (bo.EstatusUnidad.Contains(EEstatusUnidad.Disponible))
                        this.vista.EstatusID = 1;
                    else {
                        if (bo.EstatusUnidad.Contains(EEstatusUnidad.ConCliente))
                            this.vista.EstatusID = 2;
                        else {
                            if (bo.EstatusUnidad.Contains(EEstatusUnidad.Baja))
                                this.vista.EstatusID = 3;
                            else {
                                if (bo.EstatusUnidad.Contains(EEstatusUnidad.Seminuevo))
                                    this.vista.EstatusID = 4;
                                else
                                    this.vista.EstatusID = null;
                            }
                        }
                    }
                }
            } else
                this.vista.EstatusID = null;

            this.vista.FechaAltaFinal = bo.FechaAltaFinal;
            this.vista.FechaAltaInicial = bo.FechaAltaInicial;
            this.vista.FechaBajaFinal = bo.FechaBajaFinal;
            this.vista.FechaBajaInicial = bo.FechaBajaInicial;
        }

        public void ConsultarFlota() {
            try {
                string s;
                if ((s = this.ValidarCampos()) != null) {
                    this.vista.MostrarMensaje(s, ETipoMensajeIU.ADVERTENCIA, null);
                    return;
                }

                FlotaBOF bo = (FlotaBOF)this.InterfazUsuarioADato();
                List<ElementoFlotaBOF> lst = this.controlador.ConsultarSeguimientoFlota(this.dctx, bo);

                if (lst.Count < 1)
                    this.vista.MostrarMensaje("La búsqueda no produjo resultados", ETipoMensajeIU.INFORMACION,
                        "No se encontraron coincidencias con los criterios ingresados. Verifique sus datos");

                this.vista.EstablecerResultado(lst);
            } catch (Exception ex) {
                throw new Exception(this.nombreClase + ".ConsultarFlota: " + ex.Message);
            }
        }
        private string ValidarCampos() {
            string s = "";

            if (this.vista.UnidadOperativaID == null)
                s += "UnidadOperativaID, ";

            if (s != null && s.Trim().CompareTo("") != 0)
                return "Los siguientes campos no pueden estar vacíos: \n" + s.Substring(0, s.Length - 2);

            if (this.vista.FechaAltaInicial != null && this.vista.FechaAltaFinal != null && this.vista.FechaAltaFinal < this.vista.FechaAltaInicial)
                return "La fecha de alta inicial no puede ser mayor que la fecha de alta final.";
            if (this.vista.FechaBajaInicial != null && this.vista.FechaBajaFinal != null && this.vista.FechaBajaFinal < this.vista.FechaBajaInicial)
                return "La fecha de baja inicial no puede ser mayor que la fecha de baja final.";

            return null;
        }

        public void IrAExpediente(int? unidadID) {
            try {
                if (unidadID == null)
                    throw new Exception("No se encontró el registro seleccionado.");

                Equipos.BO.UnidadBO bo = new Equipos.BO.UnidadBO { UnidadID = unidadID };

                this.vista.LimpiarSesion();

                Dictionary<string, object> paquete = new Dictionary<string, object>();
                paquete.Add("ObjetoFiltro", this.InterfazUsuarioADato());
                paquete.Add("Bandera", true);

                this.vista.EstablecerPaqueteNavegacion("FiltrosSeguimientoFlota", paquete);
                this.vista.EstablecerPaqueteNavegacion("UnidadExpedienteBO", bo);

                this.vista.RedirigirAExpediente();
            } catch (Exception ex) {
                throw new Exception(this.nombreClase + ".IrAExpediente: " + ex.Message);
            }
        }

        #region Métodos para el Buscador
        public object PrepararBOBuscador(string catalogo) {
            object obj = null;

            switch (catalogo) {
                case "Sucursal":
                    SucursalBOF sucursal = new SucursalBOF();
                    sucursal.UnidadOperativa = new UnidadOperativaBO();
                    sucursal.Usuario = new UsuarioBO();

                    sucursal.UnidadOperativa.Id = this.vista.UnidadOperativaID;
                    sucursal.Nombre = this.vista.SucursalNombre;
                    sucursal.Usuario.Id = this.vista.UsuarioID;

                    obj = sucursal;
                    break;
                case "UnidadIdealease":
                    UnidadBOF unidad = new UnidadBOF();

                    if (!string.IsNullOrEmpty(this.vista.VIN))
                        unidad.NumeroSerie = this.vista.VIN;

                    obj = unidad;
                    break;
                case "TipoUnidad":
                    TipoUnidadBO tipoUnidad = new TipoUnidadBO();

                    tipoUnidad.Nombre = this.vista.TipoUnidadNombre;
                    tipoUnidad.Activo = true;

                    obj = tipoUnidad;
                    break;
                case "Marca":
                    MarcaBO marca = new MarcaBO();

                    marca.Nombre = this.vista.MarcaNombre;
                    marca.Activo = true;

                    obj = marca;
                    break;
                case "Modelo":
                    ModeloBO modelo = new ModeloBO();
                    modelo.Auditoria = new AuditoriaBO();
                    modelo.Marca = new MarcaBO();

                    modelo.Marca.Id = this.vista.MarcaID;
                    modelo.Nombre = this.vista.ModeloNombre;
                    modelo.Activo = true;

                    obj = modelo;
                    break;
            }

            return obj;
        }
        public void DesplegarResultadoBuscador(string catalogo, object selecto) {
            switch (catalogo) {
                case "Sucursal":
                    #region Desplegar Sucursal
                    SucursalBO sucursal = (SucursalBO)selecto;
                    if (sucursal != null && sucursal.Id != null)
                        this.vista.SucursalID = sucursal.Id;
                    else
                        this.vista.SucursalID = null;

                    if (sucursal != null && sucursal.Nombre != null)
                        this.vista.SucursalNombre = sucursal.Nombre;
                    else
                        this.vista.SucursalNombre = null;
                    #endregion
                    break;
                case "UnidadIdealease":
                    #region Desplegar Unidad
                    UnidadBOF unidad = (UnidadBOF)selecto ?? new UnidadBOF();
                    if (unidad.NumeroSerie != null)
                        this.vista.VIN = unidad.NumeroSerie;
                    else
                        this.vista.VIN = string.Empty;
                    #endregion
                    break;
                case "TipoUnidad":
                    #region Desplegar TipoUnidad
                    TipoUnidadBO tipoUnidad = (TipoUnidadBO)selecto;

                    if (tipoUnidad != null && tipoUnidad.Id != null)
                        this.vista.TipoUnidadID = tipoUnidad.Id;
                    else
                        this.vista.TipoUnidadID = null;

                    if (tipoUnidad != null && tipoUnidad.Nombre != null)
                        this.vista.TipoUnidadNombre = tipoUnidad.Nombre;
                    else
                        this.vista.TipoUnidadNombre = null;
                    #endregion
                    break;
                case "Marca":
                    #region Desplegar Marca
                    MarcaBO marca = (MarcaBO)selecto;

                    if (marca != null && marca.Id != null)
                        this.vista.MarcaID = marca.Id;
                    else
                        this.vista.MarcaID = null;

                    if (marca != null && marca.Nombre != null)
                        this.vista.MarcaNombre = marca.Nombre;
                    else
                        this.vista.MarcaNombre = null;

                    this.vista.ModeloID = null;
                    this.vista.ModeloNombre = null;
                    #endregion
                    break;
                case "Modelo":
                    #region Desplegar Modelo
                    ModeloBO modelo = (ModeloBO)selecto;

                    if (modelo != null && modelo.Id != null)
                        this.vista.ModeloID = modelo.Id;
                    else
                        this.vista.ModeloID = null;

                    if (modelo != null && modelo.Nombre != null)
                        this.vista.ModeloNombre = modelo.Nombre;
                    else
                        this.vista.ModeloNombre = null;
                    #endregion
                    break;
            }
        }
        #endregion
        #endregion
    }
}