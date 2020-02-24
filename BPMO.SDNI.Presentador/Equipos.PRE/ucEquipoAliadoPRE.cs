//Satisface al CU075 - Catálogo de Equipo Aliado
// Satisface a la SC0005
//Satisface a la solicitud de cambio SC0035
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using BPMO.Basicos.BO;
using BPMO.Facade.SDNI.BO;
using BPMO.Facade.SDNI.BOF;
using BPMO.Facade.SDNI.BR;
using BPMO.Patterns.Creational.DataContext;
using BPMO.Primitivos.Enumeradores;
using BPMO.SDNI.Comun.BO;
using BPMO.SDNI.Comun.BR;
using BPMO.SDNI.Equipos.BO;
using BPMO.SDNI.Equipos.BR;
using BPMO.SDNI.Equipos.VIS;
using BPMO.Servicio.Catalogos.BO;

namespace BPMO.SDNI.Equipos.PRE
{
    public class ucEquipoAliadoPRE
    {
        #region Atributos
        IucEquipoAliadoVIS vista;
        private const String nombreClase = "ucEquipoAliadoPRE";

        private readonly EquipoAliadoBR controlador;
        /// <summary>
        /// El DataContext que proveerá acceso a la base de datos
        /// </summary>
        private readonly IDataContext dctx;
        #endregion

        #region Constructores
        public ucEquipoAliadoPRE(IucEquipoAliadoVIS view)
        {
            this.vista = view;
            this.controlador = new EquipoAliadoBR();
            this.dctx = Facade.SDNI.BR.FacadeBR.ObtenerConexion();
        }
        #endregion

        #region Métodos
        private void DatosAinterfazUsuario(object obj)
        {
        }

        public void PrepararNuevo()
        {
            this.vista.CargarTiposEquipoAliado();
            this.vista.AsignarTipoEquipoAliado();
            this.vista.PrepararNuevo();
            this.EstablecerInformacionInicial();
        }
        private void EstablecerInformacionInicial()
        {
            try
            {
                #region Configuraciones de Unidad Operativa
                //Obtener las configuraciones de la unidad operativa
                List<ConfiguracionUnidadOperativaBO> lstConfigUO = new ModuloBR().ConsultarConfiguracionUnidadOperativa(this.dctx, new ConfiguracionUnidadOperativaBO() { UnidadOperativa = new UnidadOperativaBO() { Id = this.vista.UnidadOperativaID } }, this.vista.ModuloID);
                if (lstConfigUO.Count <= 0)
                    throw new Exception("No se encontraron las configuraciones del sistema para la unidad operativa en la que trabaja.");

                //Establecer las configuraciones de la unidad operativa
                this.vista.LibroActivos = lstConfigUO[0].Libro;
                #endregion
            }
            catch (Exception ex)
            {
                throw new Exception(nombreClase + ".EstablecerInformacionInicial: " + ex.Message);
            }
        }

        public string ValidarCamposConsultaUnidad()
        {
            string s = "";

            if (!(this.vista.NumeroSerie != null && this.vista.NumeroSerie.Trim().CompareTo("") != 0))
                s += "Número de Serie, ";

            if (s.Trim().CompareTo("") != 0)
                return "Los siguientes campos no pueden estar vacíos: \n" + s.Substring(0, s.Length - 2);

            return null;
        }

        private void CargarSucursales()
        {
            try
            {
                var seguridad = new SeguridadBO(Guid.Empty, vista.Usuario, new AdscripcionBO { UnidadOperativa = vista.UnidadOperativa });
                List<SucursalBO> sucursales = Facade.SDNI.BR.FacadeBR.ConsultarSucursalesSeguridad(this.dctx, seguridad);

                if (sucursales == null) sucursales = new List<SucursalBO>();

                if (sucursales.Count > 0)
                {
                    if (sucursales.Count > 1)
                    {
                        SucursalBO item = new SucursalBO { Activo = true, Id = 0, Nombre = "SELECCIONE UNA OPCIÓN" };
                        sucursales.Add(item);
                    }
                }
                else
                {
                    vista.MostrarMensaje("No cuenta con sucursales asignadas.", ETipoMensajeIU.ADVERTENCIA);
                }
            }
            catch (Exception ex)
            {
                vista.MostrarMensaje("Incosistencias al presentar la información", ETipoMensajeIU.ERROR, nombreClase + ".DesplegarSucursales: " + ex.Message);
            }
        }

        public void Registrar()
        {
            //Se validan los campos
            string s;
            if ((s = this.ValidarCamposRegistroModificacion()) != null)
            {
                this.vista.MostrarMensaje(s, ETipoMensajeIU.ADVERTENCIA, null);
                return;
            }

            //Se recupera la inf a partir de la UI
            EquipoAliadoBO bo = (EquipoAliadoBO)this.InterfazUsuarioADato();

            //Asignamos los valores estaticos
            if (!string.IsNullOrEmpty(bo.ClaveActivoOracle) && !string.IsNullOrWhiteSpace(bo.ClaveActivoOracle))
                bo.EsActivo = true;
            else
                bo.EsActivo = false;
            bo.Estatus = EEstatusEquipoAliado.SinAsignar;

            //Se crea el objeto de seguridad
            UsuarioBO usuario = this.vista.Usuario;
            AdscripcionBO adscripcion = new AdscripcionBO() { UnidadOperativa = this.vista.UnidadOperativa };
            SeguridadBO seguridadBO = new SeguridadBO(Guid.Empty, usuario, adscripcion);

            //Se inserta el equipo aliado
            this.controlador.InsertarCompleto(this.dctx, bo, seguridadBO);

            //Se consulta lo insertado para recuperar los ID
            #region Correccion 
            EquipoAliadoBO equipoAliadoBuscar = new EquipoAliadoBO()
            {
                EquipoAliadoID = bo.EquipoAliadoID,
                EquipoID = bo.EquipoID,
            };
            #endregion         
            DataSet ds = this.controlador.ConsultarSet(this.dctx, equipoAliadoBuscar);
            if (ds.Tables[0].Rows.Count <= 0)
                throw new Exception("Al consultar lo insertado no se encontraron coincidencias.");
            if (ds.Tables[0].Rows.Count > 1)
                throw new Exception("Al consultar lo insertado se encontró más de una coincidencia.");

            bo.EquipoID = this.controlador.DataRowToEquipoAliadoBO(ds.Tables[0].Rows[0]).EquipoID;
            bo.EquipoAliadoID = this.controlador.DataRowToEquipoAliadoBO(ds.Tables[0].Rows[0]).EquipoAliadoID;

            this.VerDetalles(bo);
        }

        private void VerDetalles(object bo)
        {
            this.vista.LimpiarSesion();
            this.vista.EstablecerPaqueteNavegacion("EquipoAliadoBODetalle", bo);
            this.vista.RedirigirADetalles();
        }		
        private object InterfazUsuarioADato(bool mantenimiento = true)
        {
            int Anio = 0;
            EquipoAliadoBO bo = new EquipoAliadoBO();
            bo.ClaveActivoOracle = this.vista.OracleID;
            bo.EsActivo = this.vista.ActivoOracle;
            bo.Dimension = this.vista.Dimension;
            bo.Fabricante = vista.Fabricante;
            bo.FC = vista.FC;
            bo.FUA = vista.FUA;
            bo.IDLider = vista.EquipoLiderID;
            bo.Modelo = new ModeloBO();
            bo.Modelo.Id = this.vista.ModeloID;
            bo.Modelo.Marca = new MarcaBO();
            bo.Modelo.Marca.Id = this.vista.MarcaID;
            bo.NumeroSerie = this.vista.NumeroSerie;
            bo.PBC = this.vista.PBC;
            bo.PBV = this.vista.PBV;
            bo.Sucursal = new SucursalBO();
            bo.Sucursal.Id = this.vista.SucursalID;
            bo.Sucursal.UnidadOperativa = new UnidadOperativaBO();
            bo.Sucursal.UnidadOperativa.Id = this.vista.UnidadOperativaID;
            bo.TipoEquipoServicio = new TipoUnidadBO();
            bo.TipoEquipoServicio.Id = this.vista.TipoEquipoID;
            bo.TipoEquipoServicio.Nombre = this.vista.TipoEquipoNombre;
            bo.UC = this.vista.UC;
            bo.UUA = this.vista.UUA;
            bo.EquipoAliadoID = this.vista.EquipoAliadoID;
            bo.EquipoID = this.vista.EquipoID;

            bo.HorasIniciales = this.vista.HorasIniciales;
            bo.KilometrajeInicial = this.vista.KilometrosIniciales;

            //Se asigna el valor del año del equipo a la propiedad bo.Anio, con la finalidad de guardarlo
            if (!string.IsNullOrEmpty(this.vista.AnioModelo) && !string.IsNullOrWhiteSpace(this.vista.AnioModelo))
                if (int.TryParse(this.vista.AnioModelo.Trim().Replace(",", ""), out Anio))
                    bo.Anio = Anio;

            if (this.vista.EstatusEquipo.HasValue)
                if (this.vista.EstatusEquipo.Value >= 0)
                    bo.Estatus = (EEstatusEquipoAliado)this.vista.EstatusEquipo;
            if (this.vista.TipoEquipoAliado.HasValue)
                if (this.vista.TipoEquipoAliado.Value >= 0)
                    bo.TipoEquipoAliado = (ETipoEquipoAliado)this.vista.TipoEquipoAliado;
            if (mantenimiento)
                bo.Activo = this.vista.Activo;
            return bo;
        }		
        private string ValidarCamposRegistroModificacion()
        {
            string s = string.Empty;
            if (!vista.UnidadOperativaID.HasValue)
                s += "Unidad Operativa, ";
            if (!vista.SucursalID.HasValue)
                s += "Sucursal, ";
            if (!vista.TipoEquipoID.HasValue)
                s += "Tipo de equipo, ";
            if (string.IsNullOrEmpty(this.vista.NumeroSerie) || string.IsNullOrWhiteSpace(this.vista.NumeroSerie))
                s += "Numero de serie, ";
            //Cuando la unidad operativa elegida por el usuario no es Generación el campo "PBC" es obligatorio
            if (!this.vista.PBC.HasValue && (this.vista.EmpresaConPermiso != ETipoEmpresa.Generacion || this.vista.EmpresaConPermiso != ETipoEmpresa.Equinova))
                s += "PBC, ";
            //Cuando la unidad operativa elegida por el usuario no es Generación el campo "PBV" es obligatorio
            if (!this.vista.PBV.HasValue && (this.vista.EmpresaConPermiso != ETipoEmpresa.Generacion || this.vista.EmpresaConPermiso != ETipoEmpresa.Equinova))
                s += "PBV, ";
            if (!this.vista.TipoEquipoAliado.HasValue)
                s += "Tipo Equipo Aliado, ";
            if (this.vista.SucursalID.HasValue)
                if (this.vista.SucursalID.Value <= 0)
                    s += "Sucursal, ";
            if (this.vista.TipoEquipoID.HasValue)
                if (this.vista.TipoEquipoID.Value <= -1)
                    s += "Tipo de equipo, ";
            #region SC0001
            if (this.vista.HorasIniciales.HasValue)
                //Cuando la unidad operativa elegida por el usuario no es Generación el campo "Horas Iniciales" es obligatorio
                if (this.vista.HorasIniciales.Value <= -1 && (this.vista.EmpresaConPermiso != ETipoEmpresa.Generacion || this.vista.EmpresaConPermiso != ETipoEmpresa.Equinova))
                    s += "Horas Iniciales, ";
            if (this.vista.KilometrosIniciales.HasValue)
                //Cuando la unidad operativa elegida por el usuario no es Generación el campo "Kilometros Iniciales" es obligatorio
                if (this.vista.KilometrosIniciales.Value <= -1 && (this.vista.EmpresaConPermiso != ETipoEmpresa.Generacion || this.vista.EmpresaConPermiso != ETipoEmpresa.Equinova))
                    s += "Kilometros Iniciales, ";
            #endregion
            

            if (s.Trim().CompareTo("") != 0)
                return "Los siguientes campos no pueden estar vacíos: \n" + s.Substring(0, s.Length - 2);

            return null;
        }

        internal void PrepararVista()
        {
            this.vista.PrepararVista();
        }

        internal void CargarObjetoInicio()
        {
            this.vista.CargarTiposEquipoAliado();
            this.EstablecerDatosNavegacion(this.vista.ObtenerDatosNavegacion());
            this.LimpiarSesion();
            this.PrepararModificacion();
            this.ConsultarCompleto();
        }

        private void ConsultarCompleto()
        {
            try
            {				
                EquipoAliadoBO bo = (EquipoAliadoBO)this.InterfazUsuarioADato(false);				
                List<EquipoAliadoBO> lst = this.controlador.ConsultarCompleto(this.dctx, bo, true);

                if (lst.Count < 1)
                    throw new Exception("No se encontró ningún registro que corresponda a la información proporcionada.");
                if (lst.Count > 1)
                    throw new Exception("La consulta devolvió más de un registro.");

                EquipoAliadoBO nuevoEquipo = new EquipoAliadoBO(lst[0]);

                List<ActivoFijoBO> lstTemp = FacadeBR.ConsultarActivoFijo(dctx, new ActivoFijoBO() { NumeroSerie = "%" + nuevoEquipo.NumeroSerie + "%", Libro = this.vista.LibroActivos, Activo = true });
                if (lstTemp.Count > 0)
                {
                    nuevoEquipo.ActivoFijo = lstTemp[0];
                    nuevoEquipo.EsActivo = true;
                    nuevoEquipo.ClaveActivoOracle = nuevoEquipo.ActivoFijo.NumeroActivo;

                    if (nuevoEquipo.NumeroSerie.ToUpper() != nuevoEquipo.NumeroSerie.ToUpper())
                        this.vista.MostrarMensaje("El NUMERO DE SERIE de ORACLE es DIFERENTE al encontrado en E-SERVICIO. Verificar la Información.", ETipoMensajeIU.INFORMACION, null);
                }
                else
                {
                    nuevoEquipo.ActivoFijo = new ActivoFijoBO();
                    nuevoEquipo.EsActivo = false;
                    nuevoEquipo.ClaveActivoOracle = String.Empty;

                    this.vista.MostrarMensaje("NO SE ENCONTRÓ UNA CLAVE DE ORACLE PARA EL EQUIPO ALIADO.", ETipoMensajeIU.INFORMACION, null);
                }


                this.DatoAInterfazUsuario(nuevoEquipo);

                this.vista.UltimoObjeto = lst[0];
            }
            catch (Exception ex)
            {
                this.DatoAInterfazUsuario(new EquipoAliadoBO());
                throw new Exception(nombreClase + ".ConsultarCompleto:" + ex.Message);
            }
        }

        private void EstablecerDatosNavegacion(object paqueteNavegacion)
        {
            try
            {
                if (paqueteNavegacion == null)
                    throw new Exception("Se esperaba un objeto en la navegación. No se puede identificar qué acta de nacimiento se desea consultar.");
                if (!(paqueteNavegacion is EquipoAliadoBO))
                    throw new Exception("Se esperaba una Unidad de Idealease.");

                EquipoAliadoBO bo = (EquipoAliadoBO)paqueteNavegacion;

                this.DatoAInterfazUsuario(bo);
            }
            catch (Exception ex)
            {
                this.DatoAInterfazUsuario(new EquipoAliadoBO());
                throw new Exception(nombreClase + ".EstablecerDatosNavegacion: " + ex.Message);
            }
        }

        private void DatoAInterfazUsuario(object bo)
        {
            EquipoAliadoBO obj = (EquipoAliadoBO)bo;
            if (obj.Anio.HasValue)
                this.vista.AnioModelo = obj.Anio.Value.ToString();
            if (!string.IsNullOrEmpty(obj.ClaveActivoOracle) && !string.IsNullOrWhiteSpace(obj.ClaveActivoOracle))
                this.vista.OracleID = obj.ClaveActivoOracle;
            #region SC0035
            if (obj.EsActivo.HasValue)
            {
                this.vista.ActivoOracle = obj.EsActivo.Value;
            }
            #endregion
            if (!string.IsNullOrEmpty(obj.Dimension) && !string.IsNullOrWhiteSpace(obj.Dimension))
                this.vista.Dimension = obj.Dimension;
            if (obj.EquipoAliadoID.HasValue)
                this.vista.EquipoAliadoID = obj.EquipoAliadoID.Value;
            if (obj.EquipoID.HasValue)
                this.vista.EquipoID = obj.EquipoID;
            if (!string.IsNullOrEmpty(obj.Fabricante) && !string.IsNullOrWhiteSpace(obj.Fabricante))
                this.vista.Fabricante = obj.Fabricante;

            #region SC0001
            if (obj.HorasIniciales.HasValue)
                this.vista.HorasIniciales = obj.HorasIniciales.Value;
            if (obj.KilometrajeInicial.HasValue)
                this.vista.KilometrosIniciales = obj.KilometrajeInicial.Value;
            #endregion
        

            if (obj.IDLider.HasValue)
                this.vista.EquipoLiderID = obj.IDLider;
            if (obj.Modelo != null)
            {
                if (obj.Modelo.Id.HasValue)
                    this.vista.ModeloID = obj.Modelo.Id.Value;
                if (!string.IsNullOrEmpty(obj.Modelo.Nombre) && !string.IsNullOrWhiteSpace(obj.Modelo.Nombre))
                    this.vista.Modelo = obj.Modelo.Nombre;
                if (obj.Modelo.Marca != null)
                {
                    if (obj.Modelo.Marca.Id.HasValue)
                        this.vista.MarcaID = obj.Modelo.Marca.Id;
                    if (!string.IsNullOrEmpty(obj.Modelo.Marca.Nombre) && !string.IsNullOrWhiteSpace(obj.Modelo.Marca.Nombre))
                        this.vista.Marca = obj.Modelo.Marca.Nombre;
                }
            }
            if (!string.IsNullOrEmpty(obj.NumeroSerie) && !string.IsNullOrWhiteSpace(obj.NumeroSerie))
                this.vista.NumeroSerie = obj.NumeroSerie;
            if (obj.PBC.HasValue)
                this.vista.PBC = obj.PBC.Value;
            if (obj.PBV.HasValue)
                this.vista.PBV = obj.PBV.Value;
            if (obj.Sucursal != null)
            {
                if (obj.Sucursal.Id.HasValue)
                    this.vista.SucursalID = obj.Sucursal.Id.Value;
                if (!string.IsNullOrEmpty(obj.Sucursal.Nombre) && !string.IsNullOrWhiteSpace(obj.Sucursal.Nombre))
                    this.vista.SucursalNombre = obj.Sucursal.Nombre;
                if (obj.Sucursal.UnidadOperativa != null)
                {
                    if (obj.Sucursal.UnidadOperativa.Id.HasValue)
                        if (obj.Sucursal.UnidadOperativa.Id.Value == this.vista.UnidadOperativa.Id)
                            this.vista.UnidadOperativaID = obj.Sucursal.UnidadOperativa.Id.Value;
                }
            }

            if (obj.TipoEquipoServicio != null)
            {
                this.vista.TipoEquipoID = obj.TipoEquipoServicio.Id;
                this.vista.TipoEquipoNombre = obj.TipoEquipoServicio.Nombre;
            }
            if (obj.Estatus.HasValue)
                this.vista.EstatusEquipo = (int)obj.Estatus.Value;
            if (obj.TipoEquipoAliado.HasValue)
                this.vista.TipoEquipoAliado = (int?)obj.TipoEquipoAliado.Value;			
            this.vista.Activo = obj.Activo;			
        }

        public bool VerificarExistenciaEquipo(object equipo)
        {
            string numeroSerie = null;
            if (equipo == null) return false;
            if (!(equipo is EquipoBepensaBO) && !(equipo is Servicio.Catalogos.BO.UnidadBO) && !(equipo is ActivoFijoBO))
                throw new Exception("Se esperaba un EquipoBepensa, una Unidad de Servicio o un Activo Fijo.");

            if (equipo is EquipoBepensaBO && !(((EquipoBepensaBO)equipo).Unidad != null && ((EquipoBepensaBO)equipo).Unidad.Id != null))
                return true;

            if (equipo is EquipoBepensaBO)
                numeroSerie = ((EquipoBepensaBO)equipo).NumeroSerie;
            if (equipo is Servicio.Catalogos.BO.UnidadBO)
                numeroSerie = ((Servicio.Catalogos.BO.UnidadBO)equipo).NumeroSerie;
            if (equipo is ActivoFijoBO)
                numeroSerie = ((ActivoFijoBO)equipo).NumeroSerie;

            if (numeroSerie == null)
                throw new Exception("El número de serie es requerido.");

            #region RI0041
            bool existe = this.controlador.VerificarExistenciaEquipo(this.dctx, numeroSerie, null);
            #endregion RI0041
            return existe;
        }

        internal void CancelarRegistro()
        {
            this.LimpiarSesion();
            this.vista.RedirigirAConsulta();
        }

        private void LimpiarSesion()
        {
            this.vista.LimpiarSesion();
        }

        internal void CancelarEdicion()
        {
            this.LimpiarSesion();
            this.vista.RedirigirAConsulta();
        }

        internal void PrepararModificacion()
        {
            this.vista.PrepararModificacion();
            this.EstablecerInformacionInicial();
        }

        internal void Editar()
        {
            string s;
            if ((s = this.ValidarCamposRegistroModificacion()) != null)
            {
                this.vista.MostrarMensaje(s, ETipoMensajeIU.ADVERTENCIA, null);
                return;
            }

            //Se obtiene la información a partir de la Interfaz de Usuario
            EquipoAliadoBO obj = (BO.EquipoAliadoBO)this.InterfazUsuarioADato();

            //Se crea el objeto de seguridad
            UsuarioBO usuario = this.vista.Usuario;
            AdscripcionBO adscripcion = new AdscripcionBO() { UnidadOperativa = this.vista.UnidadOperativa };
            SeguridadBO seguridadBO = new SeguridadBO(Guid.Empty, usuario, adscripcion);

            this.controlador.ActualizarCompleto(dctx, obj, this.vista.UltimoObjeto, seguridadBO);

            this.vista.MostrarMensaje("Edición Exitosa", ETipoMensajeIU.EXITO, null);

            this.VerDetalles(obj);
        }


        #region Métodos para el Buscador
        public object PrepararBOBuscador(string catalogo)
        {
            object obj = null;

            switch (catalogo)
            {
                case "Unidad":
                    EquipoBepensaBO ebBO = new EquipoBepensaBO();
                    ebBO.ActivoFijo = new ActivoFijoBO();
                    ebBO.Unidad = new Servicio.Catalogos.BO.UnidadBO();

                    ebBO.Unidad.NumeroSerie = this.vista.NumeroSerie;
                    ebBO.Unidad.ConfiguracionModeloMotorizacion = new ConfiguracionModeloMotorizacionBO();
                    ebBO.Unidad.ConfiguracionModeloMotorizacion.Modelo = new ModeloBO();
                    ebBO.Unidad.ConfiguracionModeloMotorizacion.Modelo.Marca = new MarcaBO();
                    ebBO.Unidad.Activo = true;

                    ebBO.Unidad.NumeroSerie = this.vista.NumeroSerie;
                    ebBO.ActivoFijo.NumeroSerie = this.vista.NumeroSerie;
                    ebBO.ActivoFijo.Activo = true;
                    ebBO.ActivoFijo.Libro = this.vista.LibroActivos;

                    obj = ebBO;
                    break;
                case "Sucursal":
                    SucursalBOF sucursal = new SucursalBOF();
                    sucursal.UnidadOperativa = new UnidadOperativaBO();
                    sucursal.UnidadOperativa.Id = this.vista.UnidadOperativaID;
                    sucursal.Nombre = this.vista.SucursalNombre;
                    sucursal.Usuario = new UsuarioBO() { Id = this.vista.Usuario.Id };
                    obj = sucursal;
                    break;
            }

            return obj;
        }
        public void DesplegarResultadoBuscador(string catalogo, object selecto)
        {
            switch (catalogo)
            {
                case "Unidad":
                    #region Desplegar Unidad
                    EquipoBepensaBO ebBO = new EquipoBepensaBO();
                    if (selecto != null)
                        ebBO = (EquipoBepensaBO)selecto;

                    if (ebBO.NumeroSerie != null)
                        this.vista.NumeroSerie = ebBO.NumeroSerie;
                    else
                        this.vista.NumeroSerie = null;

                    if (ebBO.Unidad != null)
                    {
                        if (ebBO.Unidad.Id != null)
                        {
                            this.vista.EquipoLiderID = ebBO.Unidad.Id;
                        }
                        else
                        {
                            this.vista.EquipoLiderID = null;
                        }

                        #region ConfiguracionModeloMotorizacion
                        if (ebBO.Unidad.ConfiguracionModeloMotorizacion != null)
                        {
                            #region Modelo
                            if (ebBO.Unidad.ConfiguracionModeloMotorizacion.Modelo != null)
                            {
                                if (ebBO.Unidad.ConfiguracionModeloMotorizacion.Modelo.Id != null)
                                    this.vista.ModeloID = ebBO.Unidad.ConfiguracionModeloMotorizacion.Modelo.Id;
                                else
                                    this.vista.ModeloID = null;

                                if (ebBO.Unidad.ConfiguracionModeloMotorizacion.Modelo.Nombre != null)
                                    this.vista.Modelo = ebBO.Unidad.ConfiguracionModeloMotorizacion.Modelo.Nombre;
                                else
                                    this.vista.Modelo = null;
                                #region Marca
                                if (ebBO.Unidad.ConfiguracionModeloMotorizacion.Modelo.Marca != null)
                                {
                                    if (ebBO.Unidad.ConfiguracionModeloMotorizacion.Modelo.Marca.Id != null)
                                        this.vista.MarcaID = ebBO.Unidad.ConfiguracionModeloMotorizacion.Modelo.Marca.Id;
                                    else
                                        this.vista.MarcaID = null;
                                    if (ebBO.Unidad.ConfiguracionModeloMotorizacion.Modelo.Marca.Nombre != null)
                                        this.vista.Marca = ebBO.Unidad.ConfiguracionModeloMotorizacion.Modelo.Marca.Nombre;
                                    else
                                        this.vista.Marca = null;
                                }
                                else
                                {
                                    this.vista.Modelo = null;
                                    this.vista.Marca = null;
                                }
                                #endregion
                            }
                            else
                            {
                                this.vista.Modelo = null;
                                this.vista.Marca = null;
                            }
                            #endregion
                        }
                        else
                        {
                            this.vista.Modelo = null;
                            this.vista.Marca = null;
                        }
                        #endregion
                        #region Distribuidor
                        if (ebBO.Unidad.Distribuidor != null)
                        {
                            if (ebBO.Unidad.Distribuidor.Nombre != null)
                                this.vista.Fabricante = ebBO.Unidad.Distribuidor.Nombre;
                            else
                                this.vista.Fabricante = null;
                        }
                        else
                        {
                            this.vista.Fabricante = null;
                        }
                        #endregion
                        #region Activo

                        if (ebBO.ActivoFijo.NumeroActivo != null && ebBO.ActivoFijo.NumeroActivo.Trim().CompareTo("") != 0)
                        {
                            this.vista.OracleID = ebBO.ActivoFijo.NumeroActivo;
                            this.vista.ActivoOracle = true;//SC0035
                        }
                        else
                        {
                            this.vista.OracleID = null;
                            this.vista.ActivoOracle = false;//SC0035
                        }
                        #endregion
                        #region TipoUnidad
                        if (ebBO.Unidad.TipoUnidad != null)
                        {
                            this.vista.TipoEquipoID = ebBO.Unidad.TipoUnidad.Id;
                            this.vista.TipoEquipoNombre = ebBO.Unidad.TipoUnidad.Nombre;
                        }
                        else
                        {
                            this.vista.TipoEquipoID = null;
                            this.vista.TipoEquipoNombre = null;
                        }
                        #endregion
                        #region SC0001

                        if (ebBO.Unidad.KmHrs == true)
                        {
                            this.vista.HorasIniciales = ebBO.Unidad.KmHrsInicial;
                            
                        }
                        else if (ebBO.Unidad.KmHrs == false)
                        {
                            this.vista.KilometrosIniciales = ebBO.Unidad.KmHrsInicial;
                        }

                        this.vista.AbilitarKMHRS((bool)ebBO.Unidad.KmHrs);

                        #endregion

                        //Se Asigna el valor del año a la propiedad respectiva de la vista.
                        if (ebBO.Unidad.Anio != null)
                            this.vista.AnioModelo = ebBO.Unidad.Anio.ToString();

                    }
                    else
                    {
                        this.vista.AnioModelo = null;
                        this.vista.Dimension = null;
                        this.vista.EquipoAliadoID = null;
                        this.vista.Fabricante = null;
                        this.vista.Marca = null;
                        this.vista.Modelo = null;
                        this.vista.NumeroSerie = null;
                        this.vista.PBC = null;
                        this.vista.PBV = null;
                        this.vista.TipoEquipoID = null;
                        this.vista.TipoEquipoNombre = null;
                        this.vista.UnidadOperativaID = null;
                        this.vista.OracleID = null;
                        this.vista.ActivoOracle = null;
                    }
                  
                    #endregion
                    break;
                case "Sucursal":
                    #region Desplegar TipoUnidad
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
            }
        }
        #endregion

        #region REQ 13596 Métodos relacionado con las acciones dependiendo de la unidad operativa.

        /// <summary>
        /// Determina las acciones relacionadas con el comportamiento de las vistas.
        /// </summary>
        /// <param name="listaAcciones">Lista de objetos de tipo CatalogoBaseBO que contiene las acciones a las cuales el usuario tiene permiso.</param>
        public void EstablecerAcciones(List<CatalogoBaseBO> listaAcciones) {

            switch (this.vista.UnidadOperativaID) {
                case (int)ETipoEmpresa.Generacion:
                    if (ExisteAccion(listaAcciones, "UI EQUIPO GENERACION")) {
                        this.vista.EmpresaConPermiso = ETipoEmpresa.Generacion;
                    }
                    break;
                case (int)ETipoEmpresa.Equinova:
                    if (ExisteAccion(listaAcciones, "UI EQUIPO GENERACION")) {
                        this.vista.EmpresaConPermiso = ETipoEmpresa.Equinova;
                    }
                    break;
                case (int)ETipoEmpresa.Construccion:
                    if (ExisteAccion(listaAcciones, "UI EQUIPO CONSTRUCCION")) {
                        this.vista.EmpresaConPermiso = ETipoEmpresa.Construccion;
                    }
                    break;
                default:
                    this.vista.EmpresaConPermiso = ETipoEmpresa.Idealease;
                    break;
            }
            this.vista.EstablecerAcciones();
        }

        /// <summary>
        /// Verifica que una acción a avaluar exista en el listado de acciones asignadas al usuario.
        /// </summary>
        /// <param name="acciones">Lista de acciones asignadas al usuario.</param>
        /// <param name="nombreAccion">Acción a evaluar</param>
        /// <returns>Devuelve true en caso de existir la acción a evaluar en el listado de acciones, en caso contrario regresa false.</returns>
        private bool ExisteAccion(List<CatalogoBaseBO> acciones, string nombreAccion)
        {
            if (acciones != null && acciones.Count > 0 && acciones.Exists(p => p != null && p.Nombre != null && p.Nombre.Trim().ToUpper() == nombreAccion.Trim().ToUpper()))
                return true;

            return false;
        }

        #endregion
        #endregion
    }
}