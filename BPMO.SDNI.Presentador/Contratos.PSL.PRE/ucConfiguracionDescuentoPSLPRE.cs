using System;
using System.Collections.Generic;
using System.Linq;
using BPMO.Basicos.BO;
using BPMO.Facade.SDNI.BOF;
using BPMO.Facade.SDNI.BR;
using BPMO.Patterns.Creational.DataContext;
using BPMO.Primitivos.Enumeradores;
using BPMO.SDNI.Comun.BO;
using BPMO.SDNI.Contratos.PSL.BO;
using BPMO.SDNI.Contratos.PSL.BR;
using BPMO.SDNI.Contratos.PSL.VIS;

namespace BPMO.SDNI.Contratos.PSL.PRE {
    public class ucConfiguracionDescuentoPSLPRE {
        #region Atributos
        private IDataContext dctx = null;
        private IucConfiguracionDescuentoPSLVIS vista;
        private string nombreClase = "ucConfiguracionDescuentoPSLPRE";

        private ConfiguracionDescuentoPSLBR controlador;
        #endregion

        #region Constructor

        public ucConfiguracionDescuentoPSLPRE(IucConfiguracionDescuentoPSLVIS view, String Accion = "") {
            try {
                this.vista = view;
                this.vista.IngresarAccion(Accion);
                this.dctx = FacadeBR.ObtenerConexion();
                this.controlador = new ConfiguracionDescuentoPSLBR();
            } catch (Exception ex) {
                this.vista.MostrarMensaje("Inconsistencias en los parámetros de configuración", ETipoMensajeIU.ERROR, this.nombreClase + ".ucConfiguracionDescuentoPSLPRE:" + ex.Message);
            }
        }

        #endregion

        #region Métodos

        public void PrepararNuevo() {
            this.vista.PrepararNuevo();
            this.vista.HabilitarModoEdicion(true);
            this.vista.MostrarBotonAgregar(true);
            this.vista.MostrarBotonCancelar(false);
        }

        public void PrepararEdicion() {
            this.vista.HabilitarModoEdicion(true);
            this.vista.HabilitaCheckSucursales(false);
            this.vista.DesHabilitarSucursal(false);
            this.vista.MostrarBotonAgregar(false);
            this.vista.MostrarBotonCancelar(true);
            this.vista.HabilitaBotonActualizar(true);
        }

        public string ValidarCampos(bool checkSucursales) {
            string s = "";

            if (!(this.vista.Cliente != null))
                s += "Cliente, ";

            if (!(this.vista.ContactoComercial != null))
                s += "Contacto Comercial, ";

            if (checkSucursales == false)
                if (!(this.vista.SucursalNombre != null))
                    s += "Sucursal, ";

            if (!(this.vista.DescuentoMaximo != null))
                s += "Máximo Descuento, ";

            if (this.vista.DescuentoMaximo > 100)
                return "El máximo descuento no puede ser mayor a 100.";

            if (!(this.vista.FechaInicio != null))
                s += "Fecha Inicio, ";

            if (!(this.vista.FechaFin != null))
                s += "Fecha Fin, ";

            if (s.Trim().CompareTo("") != 0)
                return "Los siguientes campos no pueden estar vacíos: \n" + s.Substring(0, s.Length - 2);

            return null;
        }

        public string ValidarCampoConsultarCliente() {
            string s = "";

            if (!(this.vista.Cliente != null && this.vista.Cliente.Trim().CompareTo("") != 0))
                s += "Cliente, ";

            if (s.Trim().CompareTo("") != 0)
                return "Los siguientes campos no pueden estar vacíos: \n" + s.Substring(0, s.Length - 2);

            return null;
        }

        public object InterfazUsuarioADato() {

            ConfiguracionDescuentoBO bo = new ConfiguracionDescuentoBO();

            bo.Cliente = new CuentaClienteIdealeaseBO();

            bo.Sucursal = new SucursalBO();

            bo.UnidadOperativa = new UnidadOperativaBO();

            if (this.vista.ClienteId != null)
                bo.Cliente.Id = this.vista.ClienteId;

            if (this.vista.SucursalId != null) {
                bo.sucursal.Id = this.vista.SucursalId;
            } else {
                bo.Sucursales = this.vista.SucursalesAutorizadas;
            }

            return bo;
        }

        public void CambiarPaginaResultado(int nuevoIndicePagina) {
            this.vista.IndicePaginaResultado = nuevoIndicePagina;
        }

        #region Consultar

        /// <summary>
        /// Realiza la consulta a la tabla configuracionDescuetos
        /// </summary>
        /// <returns>Lista de los resultados de la consulta</returns>
        public List<ConfiguracionDescuentoBO> ConsultarDescuentoUsuario() {
            try {

                ConfiguracionDescuentoBO bo = (ConfiguracionDescuentoBO)this.InterfazUsuarioADato();

                List<ConfiguracionDescuentoBO> lst = controlador.Consultar(dctx, bo).ConvertAll(s => (ConfiguracionDescuentoBO)s);

                return lst;
            } catch (Exception ex) {
                throw new Exception(this.nombreClase + ".Consultar:" + ex.Message);
            }

        }

        /// <summary>
        /// Valida que el descuento capturado por el usuario no se encuentre registrado en la BD
        /// </summary>
        public void ValidarDescuentosSucursales() {
            string msg = "";
            ConfiguracionDescuentoBO oDescuentos = new ConfiguracionDescuentoBO();

            oDescuentos.Sucursal = new SucursalBO();

            oDescuentos.UnidadOperativa = new UnidadOperativaBO();


            //se busca las sucursales para el usuario que registra el descuento.
            CargarSucursalesAutorizadas();

            List<SucursalBO> lstSuc = this.vista.SucursalesAutorizadas;

            //Se hace la consulta para conocer si alguna sucursal ya cuenta con el descuento capturado en el UI.
            List<ConfiguracionDescuentoBO> lstDescuentos = this.ConsultarDescuentoUsuario();

            //se asignan las id de todas las sucursales a las que el usuario tiene acceso.
            string sucursales = oDescuentos.ObtenerSucursalesID();

            // se valida si existe un registro
            if (lstDescuentos.Count > 0) {
                //Se crea la nueva lista de sucursales que no tienen registrado un descuento previamente
                List<SucursalBO> lstSucursalesDisponibles = new List<SucursalBO>();

                //se recorre cada sucursal con acceso del cliente para verificar si alguna de estas se encuentra registrada
                foreach (SucursalBO sucursal in lstSuc) {
                    #region Insertar

                    var existe = lstDescuentos.FindAll(x => x.Sucursal.Id == sucursal.Id).Count();

                    //Se recorre los datos devueltos por la consulta para indicar la sucursal que ya se encuentra registrada con el mismo descuento.

                    #region ValidarSucursales


                    if (existe > 0) {
                        msg += sucursal.Nombre + ", ";

                    } else {
                        //Se crea un nuevo objeto para las sucursales disponibles
                        SucursalBO Sucursal = new SucursalBO();

                        Sucursal.Nombre = sucursal.Nombre;

                        Sucursal.Id = sucursal.Id;

                        lstSucursalesDisponibles.Add(Sucursal);
                    }
                    #endregion

                    string SucRegistradas = "La(s) sucursal(es) " + msg.Substring(0, msg.Length - 2) + " " + "ya se encuentra(n) registrada(s) para este usuario.";

                    this.vista.MostrarMensajeRegistro(SucRegistradas, ETipoMensajeIU.INFORMACION, null);
                    #endregion

                }

                this.vista.llenaGridPorSucursal(lstSucursalesDisponibles);

            } else {

                this.vista.llenaGridPorSucursal(lstSuc);
            }


        }
        #endregion


        /// <summary>
        /// Recupera las sucursales a las que el usuario autenticado tiene permiso e acceder 
        /// </summary>
        private void CargarSucursalesAutorizadas() {

            if (this.vista.SucursalesAutorizadas != null)
                if (this.vista.SucursalesAutorizadas.Count > 0)
                    return;

            var lstSucursales = FacadeBR.ConsultarSucursalesSeguridad(this.dctx,
                             new SeguridadBO(Guid.Empty, this.vista.Usuario,
                                 new AdscripcionBO { UnidadOperativa = this.vista.UnidadOperativa }));

            this.vista.SucursalesAutorizadas = lstSucursales.ConvertAll(x => (SucursalBO)x);

        }

        #region Métodos para el buscador

        public object PrepararBOBuscador(string catalogo) {
            object obj = null;

            switch (catalogo) {
                case "Sucursal":
                    SucursalBOF sucursal = new SucursalBOF();
                    sucursal.UnidadOperativa = new UnidadOperativaBO();
                    sucursal.UnidadOperativa.Id = this.vista.UnidadOperativaId;
                    sucursal.Nombre = this.vista.SucursalNombre;
                    sucursal.Usuario = new UsuarioBO() { Id = this.vista.UsuarioAutenticado };
                    obj = sucursal;
                    break;

                case "CuentaClienteIdealease":
                    CuentaClienteIdealeaseBO cliente = new CuentaClienteIdealeaseBO();
                    cliente.Nombre = this.vista.Cliente;
                    cliente.UnidadOperativa = new UnidadOperativaBO();
                    cliente.UnidadOperativa.Id = this.vista.UnidadOperativaId;
                    cliente.Activo = true;
                    obj = cliente;
                    break;
            }

            return obj;
        }

        public void DesplegarResultadoBuscador(string catalogo, object selecto) {
            switch (catalogo) {

                case "Sucursal":
                    SucursalBO sucursal = (SucursalBO)selecto;
                    if (sucursal != null && sucursal.Id != null)
                        this.vista.SucursalId = sucursal.Id;
                    else
                        this.vista.SucursalId = null;

                    if (sucursal != null && sucursal.Nombre != null)
                        this.vista.SucursalNombre = sucursal.Nombre;
                    else
                        this.vista.SucursalNombre = null;
                    break;

                case "CuentaClienteIdealease":
                    CuentaClienteIdealeaseBO cliente = (CuentaClienteIdealeaseBO)selecto;

                    if (cliente != null && cliente.Id != null)
                        this.vista.ClienteId = cliente.Id;
                    else
                        this.vista.ClienteId = null;

                    if (cliente != null && cliente.Nombre != null)
                        this.vista.Cliente = cliente.Nombre;
                    else
                        this.vista.Cliente = null;
                    break;
            }
        }

        #endregion



        #endregion

        #region Metodos editar
        /// <summary>
        /// Obtiene el contacto comercial
        /// </summary>
        /// <returns>contacto comercial de la vista</returns>
        public string obtenerContactoComercial() {

            return this.vista.ContactoComercial;

        }
        /// <summary>
        /// Método que es llamado del presentador de detalle para insertar los primeros valores en la interfaz de editar
        /// </summary>
        /// <param name="lst">una lista de descuentos</param>
        public void datoAinterfazUsuarioEditar(List<ConfiguracionDescuentoBO> lst) {

            this.vista.Cliente = lst[0].Cliente.Nombre;
            this.vista.ClienteId = lst[0].Cliente.Id;
            this.vista.ContactoComercial = lst[0].ContactoComercial;
            this.vista.HabilitarCamposEditar(false);
            this.vista.Estado = true;
            this.vista.UltimoObjeto = lst[0];
            this.vista.CrearTabla(lst);

        }
        /// <summary>
        /// Valida Los campos editables por el usuario 
        /// </summary>
        /// <returns>si es null el valor significa que el usuario puso todos los campos</returns>
        public string ValidarDatosEditar() {
            string s = "";

            if (this.vista.DescuentoMaximo.ToString().Trim().Equals("") || this.vista.DescuentoMaximo == null) { s += "Máximo Descuento, "; } else {
                if (this.vista.DescuentoMaximo > 100)
                    return "El máximo descuento no puede ser mayor a 100";
            }

            if (this.vista.FechaFin.ToString().Trim().Equals("") || this.vista.FechaFin == null) {

                s += "Fecha fin, ";
            }
            if (this.vista.FechaInicio.ToString().Trim().Equals("") || this.vista.FechaInicio == null)
                s += "Fecha inicio, ";


            if (s != null && s.Trim().CompareTo("") != 0)
                return "Los siguientes campos no pueden estar vacíos: \n" + s.Substring(0, s.Length - 2);

            return null;
        }
        /// <summary>
        /// Obtiene una lista clonada de la variable de sesión
        /// </summary>
        /// <returns>lista clonada de la lista de descuentos</returns>
        public List<ConfiguracionDescuentoBO> ObtenerListaClonada() {
            List<ConfiguracionDescuentoBO> lstTemporal = this.vista.ListaDescuentos;
            List<ConfiguracionDescuentoBO> lstClonada = new List<ConfiguracionDescuentoBO>();

            foreach (ConfiguracionDescuentoBO descuento in lstTemporal) {
                lstClonada.Add(descuento.Clone());
            }

            return lstClonada;

        }
        /// <summary>
        /// Método para actualizar el grid de la tabla
        /// </summary>
        /// <param name="todasSucursales">Recibe si esta activado el checkbox de todas las sucursales</param>
        /// <param name="index">indica el índice del registro seleccionado por el usuario</param>
        public void Actualizar(bool todasSucursales, int index, string Accion) {
            string s;
            if ((s = this.ValidarDatosEditar()) != null) {
                this.vista.MostrarMensaje(s, ETipoMensajeIU.ADVERTENCIA, null);

                this.vista.HabilitarBotonesParaEditar(true);
                return;
            }

            if ((s = this.ValidarFechas()) != null) {
                this.vista.MostrarMensaje(s, ETipoMensajeIU.ADVERTENCIA, null);

                this.vista.HabilitarBotonesParaEditar(true);
                return;
            }

            try {
                List<ConfiguracionDescuentoBO> lstTemporal = this.ObtenerListaClonada();

                if (todasSucursales) {

                    ConfiguracionDescuentoBO bo = (ConfiguracionDescuentoBO)this.InterfazUsuarioDatoParaActualizadoDescuento();
                    if (lstTemporal.Count > 0) {

                        for (int i = 0; i < lstTemporal.Count; i++) {
                            lstTemporal[i].Estado = bo.Estado;
                            lstTemporal[i].FechaFin = bo.FechaFin;
                            lstTemporal[i].FechaInicio = bo.FechaInicio;
                            lstTemporal[i].DescuentoMaximo = bo.DescuentoMaximo;
                        }


                        this.vista.CrearTabla(lstTemporal);
                        this.vista.HabilitarBotonesParaEditar(true);

                    }

                } else {

                    ConfiguracionDescuentoBO bo = (ConfiguracionDescuentoBO)this.InterfazUsuarioDatoParaActualizadoDescuento();
                    if (lstTemporal.Count > 0 && index >= 0) {


                        lstTemporal[index] = bo;
                        this.vista.CrearTabla(lstTemporal);
                        this.vista.HabilitarBotonesParaEditar(false);

                    }

                }
                this.limpiarCampos(Accion);
            } catch (Exception ex) {

                throw new Exception(this.nombreClase + ".Editar: " + ex.Message);


            }

        }
        /// <summary>
        /// Obtener un objeto de descuento clonado con los campos que puede editar el usuario
        /// </summary>
        /// <returns>Un objeto actualizado de descuentos</returns>
        public ConfiguracionDescuentoBO InterfazUsuarioDatoParaActualizadoDescuento() {
            ConfiguracionDescuentoBO boTemporal = this.vista.UltimoObjeto;
            ConfiguracionDescuentoBO boClonado = new ConfiguracionDescuentoBO();
            boClonado = boTemporal.Clone();
            boClonado.Estado = this.vista.Estado;
            boClonado.FechaFin = this.vista.FechaFin;
            boClonado.FechaInicio = this.vista.FechaInicio;
            boClonado.DescuentoMaximo = this.vista.DescuentoMaximo;

            return boClonado;
        }
        /// <summary>
        /// Limpia los campos editables por el usuario
        /// </summary>
        /// <param name="accion">Parámetro para saber que acción se esta realizando</param>
        public void limpiarCampos(String accion) {

            if (accion.ToUpper().Equals("EDITAR")) {
                this.vista.SucursalNombre = null;
                this.vista.DescuentoMaximo = null;
                this.vista.Estado = true;

                this.vista.Inicializarfechas();

            } else {

                if (this.vista.EsCancelar() && this.vista.EsActualizarEnRegistro() != true) {
                    this.vista.SucursalNombre = null;
                    this.vista.Inicializarfechas();
                    this.vista.DescuentoMaximo = null;
                    this.vista.Estado = true;
                } else {
                    this.vista.Cliente = null;
                    this.vista.ContactoComercial = null;
                    this.vista.SucursalNombre = null;
                    this.vista.Inicializarfechas();
                    this.vista.DescuentoMaximo = null;
                    this.vista.Estado = true;
                }

            }
        }
        /// <summary>
        /// Valida si el campo de contacto comercial esta vacío
        /// </summary>
        /// <returns></returns>
        public bool EstaCampoContactoComercialVacio() {
            string s = null;

            if (this.vista.ContactoComercial == null)
                s = "Contacto Comercial. ";

            if (s != null && s.Trim().CompareTo("") != 0)
                s += "Los siguientes campos no pueden estar vacíos: \n" + s.Substring(0, s.Length - 2);

            if (s != null) {
                this.vista.MostrarMensaje(s, ETipoMensajeIU.ADVERTENCIA, null);
                this.vista.HabilitarBotonesParaEditar(false);
                return true;
            } else {
                return false;
            }
        }
        #endregion

        #region Validar máxido descuento
        public string ValidarMaximoDescuento() {
            if (!this.vista.DescuentoMaximo.HasValue)
                return "Es necesario proporcionar el máximo descuento.";
            if (this.vista.DescuentoMaximo.Value < 0)
                return "Es necesario proporcionar el máximo descuento.";
            if (this.vista.DescuentoMaximo.Value > 100)
                return "El máximo descuento no puede ser mayor a 100";
            return null;
        }
        #endregion

        #region Validar Fechas
        public string ValidarFechas() {
            if (!this.vista.FechaInicio.HasValue)
                return "Es necesario proporcionar una fecha de inicio para el descuento.";
            if (!this.vista.FechaFin.HasValue)
                return "Es necesario proporcionar una fecha final para el descuento.";

            var result = DateTime.Compare(((DateTime)this.vista.FechaInicio), ((DateTime)this.vista.FechaFin));

            if (result > 0)
                return "La fecha de inicio no puede ser mayor a la fecha final.";
            return null;
        }
        #endregion
    }
}