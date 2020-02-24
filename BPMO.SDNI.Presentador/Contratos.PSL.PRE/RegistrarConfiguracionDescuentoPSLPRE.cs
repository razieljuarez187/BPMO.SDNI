using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using BPMO.Basicos.BO;
using BPMO.Facade.SDNI.BR;
using BPMO.Patterns.Creational.DataContext;
using BPMO.Primitivos.Enumeradores;
using BPMO.SDNI.Contratos.PSL.BO;
using BPMO.SDNI.Contratos.PSL.BR;
using BPMO.SDNI.Contratos.PSL.VIS;


namespace BPMO.SDNI.Contratos.PSL.PRE {
    public class RegistrarConfiguracionDescuentoPSLPRE {
        #region Atributos
        /// <summary>
        /// Contexto de conexión de datos
        /// </summary>
        private IDataContext dctx = null;
        /// <summary>
        /// Controlador Principal
        /// </summary>
        private ConfiguracionDescuentoPSLBR controlador;
        /// <summary>
        /// Vista del Registro de Descuento
        /// </summary>
        //private IucConfiguracionDescuentoPSLVIS vista;
        private IRegistrarConfiguracionDescuentoPSLVIS vista;
        /// <summary>
        /// Presentador de los Datos del Descuento
        /// </summary>
        private ucConfiguracionDescuentoPSLPRE presentadorUcDescuentos;
        /// <summary>
        /// instancia de la vista del user control hija.
        /// </summary>
        /// 
        ///
        private IucConfiguracionDescuentoPSLVIS UcVista;

        ConfiguracionDescuentoBO oDescuentos = new ConfiguracionDescuentoBO();

        private string nombreClase = "RegistrarConfiguracionDescuentoPSLPRE";
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor del la clase RegistrarConfiguracionDescuentoPSLPRE
        /// </summary>
        /// <param name="view">Vista de registro del Descuento</param>
        /// <param name="viewDescuentos">Vista del user control de decuentos</param>
        public RegistrarConfiguracionDescuentoPSLPRE(IRegistrarConfiguracionDescuentoPSLVIS view, IucConfiguracionDescuentoPSLVIS viewDescuentos) {
            try {
                this.vista = view;
                this.UcVista = viewDescuentos;
                this.presentadorUcDescuentos = new ucConfiguracionDescuentoPSLPRE(viewDescuentos, "AGREGAR");

                this.controlador = new ConfiguracionDescuentoPSLBR();
                this.dctx = FacadeBR.ObtenerConexion();
            } catch (Exception ex) {
                this.vista.MostrarMensaje("Inconsistencia en los parámetros de configuración", ETipoMensajeIU.ERROR, this.nombreClase + ".RegistrarConfiguracionDescuentoPSLPRE:" + ex.Message);
            }
        }
        #endregion

        #region Métodos

        /// <summary>
        /// Establece las opciones permitidas en base a la seguridad del usuario
        /// </summary>
        private void EstablecerSeguridad() {
            try {
                //se valida que los datos del usuario y la unidad operativa no sean nulos
                if (this.vista == null) throw new Exception("El identificador del usuario no debe ser nulo");
                if (this.UcVista.UnidadOperativaId == null) throw new Exception("El identificador de la Unidad Operativa no debe ser nulo ");

                //Se crea el objeto de seguridad
                UsuarioBO usr = new UsuarioBO { Id = this.vista.UsuarioID };
                AdscripcionBO sdscripcion = new AdscripcionBO { UnidadOperativa = new UnidadOperativaBO { Id = this.vista.UnidadOperativaID } };
                SeguridadBO seguridadBO = new SeguridadBO(Guid.Empty, usr, sdscripcion);

                //Se obtienen las acciones a las cuales el usuario tiene permiso en este proceso
                List<CatalogoBaseBO> lst = FacadeBR.ConsultarAccion(this.dctx, seguridadBO);

                //Se valida si el usuario tiene permiso para registrar una llanta
                if (!this.ExisteAccion(lst, "INSERTARCOMPLETO"))
                    this.vista.RedirigirSinPermisoAcceso();

            } catch (Exception ex) {
                throw new Exception(this.nombreClase + ".EstablecerSeguridad:" + ex.Message);
            }
        }

        /// <summary>
        /// Verifica si existe una acción en una lista de acciones proporcionada
        /// </summary>
        /// <param name="acciones">Lista de Acciones</param>
        /// <param name="nombreAccion">Acción a verificar</param>
        /// <returns></returns>
        private bool ExisteAccion(List<CatalogoBaseBO> acciones, string nombreAccion) {
            if (acciones != null && acciones.Count > 0 && acciones.Exists(p => p != null && p.Nombre != null && p.Nombre.Trim().ToUpper() == nombreAccion.Trim().ToUpper()))
                return true;

            return false;
        }

        //public string ValidarCampoConsultarCliente()
        //{
        //    string s = "";

        //    if (!(this.vista.Cliente != null && this.vista.Cliente.Trim().CompareTo("") != 0))
        //        s += "Cliente, ";

        //    if (s.Trim().CompareTo("") != 0)
        //        return "Los siguientes campos no pueden estar vacíos: \n" + s.Substring(0, s.Length - 2);

        //    return null;
        //}

        public void PrepararNuevo() {
            this.presentadorUcDescuentos.PrepararNuevo();

            this.EstablecerSeguridad();
        }

        #region Métodos para el buscador

        //public object PrepararBOBuscador(string catalogo)
        //{
        //    object obj = null;

        //    switch (catalogo)
        //    {
        //        case "Sucursal":
        //            SucursalBOF sucursal = new SucursalBOF();
        //            sucursal.UnidadOperativa = new UnidadOperativaBO();
        //            sucursal.UnidadOperativa.Id = this.vista.UnidadOperativaId;
        //            sucursal.Nombre = this.vista.SucursalNombre;
        //            sucursal.Usuario = new UsuarioBO() { Id = this.vista.UsuarioAutenticado };
        //            obj = sucursal;
        //            break;

        //        case "Cliente":
        //            ClienteBO cliente = new ClienteBO();
        //            cliente.Nombre = this.vista.Cliente;
        //            obj = cliente;
        //            break;
        //    }

        //    return obj;
        //}

        //public void DesplegarResultadoBuscador(string catalogo, object selecto)
        //{
        //    switch (catalogo)
        //    {

        //        case "Sucursal":
        //            SucursalBO sucursal = (SucursalBO)selecto;
        //            if (sucursal != null && sucursal.Id != null)
        //                this.vista.SucursalId = sucursal.Id;
        //            else
        //                this.vista.SucursalId = null;

        //            if (sucursal != null && sucursal.Nombre != null)
        //                this.vista.SucursalNombre = sucursal.Nombre;
        //            else
        //                this.vista.SucursalNombre = null;
        //            break;

        //        case "Cliente":
        //            ClienteBO cliente = (ClienteBO)selecto;

        //            if (cliente != null && cliente.Id != null)
        //                this.vista.ClienteId = cliente.Id;
        //            else
        //                this.vista.ClienteId = null;

        //            if (cliente != null && cliente.Nombre != null)
        //                this.vista.Cliente = cliente.Nombre;
        //            else
        //                this.vista.Cliente = null;
        //            break;
        //    }
        //}

        #endregion


        public void GuardarRegistros() {

            if (this.UcVista.ExisteDatosEnGrid() == true) {
                #region Se inicia la Transaccion
                this.dctx.SetCurrentProvider("Outsourcing");
                Guid firma = Guid.NewGuid();
                try {
                    this.dctx.OpenConnection(firma);
                    this.dctx.BeginTransaction(firma);
                } catch (Exception) {
                    if (this.dctx.ConnectionState == ConnectionState.Open)
                        this.dctx.CloseConnection(firma);
                    throw new Exception("Se encontraron inconsistencias al insertar el Descuento.");
                }
                #endregion

                #region Registrar
                try {
                    //se obtienen los datos del grid con base a la variable de sesión.
                    var listaDesuentos = this.UcVista.ListaDescuentos;

                    ConfiguracionDescuentoBO oDescuentos = new ConfiguracionDescuentoBO();

                    //Se crea el objeto de seguridad
                    UsuarioBO usuario = new UsuarioBO() { Id = this.vista.UsuarioID };
                    AdscripcionBO adscripcion = new AdscripcionBO() { UnidadOperativa = new UnidadOperativaBO() { Id = this.vista.UnidadOperativaID } };
                    SeguridadBO seguridadBO = new SeguridadBO(Guid.Empty, usuario, adscripcion);

                    int contador = 0;
                    //se recorre la lista de descuentos para hacer la inserción por cada una.
                    foreach (ConfiguracionDescuentoBO iDescuentos in listaDesuentos) {
                        oDescuentos = listaDesuentos[contador];

                        oDescuentos.Auditoria = new AuditoriaBO
                        {
                            FC = this.vista.FC,
                            UC = this.vista.UC,
                            FUA = this.vista.FUA,
                            UUA = this.vista.UUA,

                        };

                        //Se inserta en la base de datos
                        this.controlador.InsertarCompleto(this.dctx, oDescuentos, seguridadBO);
                        contador++;
                    }
                    this.dctx.SetCurrentProvider("Outsourcing");
                    this.dctx.CommitTransaction(firma);
                } catch (Exception ex) {
                    this.dctx.SetCurrentProvider("Outsourcing");
                    this.dctx.RollbackTransaction(firma);

                    throw new Exception(nombreClase + ".Registrar:" + ex.Message);
                }

                this.vista.EstablecerPaqueteNavegacion("ConfiguracionDescuentoBO", this.UcVista.ListaDescuentos.Last());
                this.vista.LimpiarSesion();
                this.vista.RedirigirADetalles();

                #endregion
            } else {
                this.UcVista.MostrarMensajeRegistro("No hay descuentos configurados para insertar.", ETipoMensajeIU.ADVERTENCIA, null);
            }
        }
        #endregion
    }
}