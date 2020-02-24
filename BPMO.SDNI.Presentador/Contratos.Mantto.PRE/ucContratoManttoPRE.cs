//Satisface al CU027 - Registrar Contrato de Mantenimiento
// Satisface a la solicitud de cambio SC0021
using System;
using System.Collections.Generic;
using System.Linq;
using BPMO.Basicos.BO;
using BPMO.Facade.SDNI.BOF;
using BPMO.Facade.SDNI.BR;
using BPMO.Patterns.Creational.DataContext;
using BPMO.Primitivos.Enumeradores;
using BPMO.SDNI.Comun.BO;
using BPMO.SDNI.Comun.BOF;
using BPMO.SDNI.Comun.BR;
using BPMO.SDNI.Contratos.BO;
using BPMO.SDNI.Contratos.Mantto.BO;
using BPMO.SDNI.Contratos.Mantto.VIS;
using BPMO.SDNI.Equipos.BOF;

namespace BPMO.SDNI.Contratos.Mantto.PRE
{
    public class ucContratoManttoPRE
    {
        #region Atributos
        private IDataContext dctx = null;
        private IucContratoManttoVIS vista;
        private ucLineaContratoManttoPRE presentadorLinea;

        private string nombreClase = "ucContratoManttoPRE";
        #endregion

        #region Constructores
        public ucContratoManttoPRE(IucContratoManttoVIS view, IucLineaContratoManttoVIS viewLinea)
        {
            try
            {
                this.vista = view;
                this.presentadorLinea = new ucLineaContratoManttoPRE(viewLinea);

                this.dctx = FacadeBR.ObtenerConexion();
            }
            catch (Exception ex)
            {
                this.vista.MostrarMensaje("Inconsistencia en los parámetros de configuración", ETipoMensajeIU.ERROR, this.nombreClase + ".ucContratoManttoPRE:" + ex.Message);
            }
        }
        #endregion

        #region Métodos
        /// <summary>
        /// Prepara la vista para su modalidad de registro
        /// </summary>
        public void PrepararNuevo()
        {
            this.LimpiarSesion();

            this.vista.ModoEdicion = true;

            this.vista.PrepararNuevo();
            this.presentadorLinea.PrepararNuevo(null);

            this.vista.MostrarLineaContrato(false);
            this.vista.MostrarRepresentantesAval(false);
            this.vista.MostrarRepresentantesObligado(false);
            this.vista.MostrarNumeroEconomico(true);

            this.EstablecerInformacionInicial();

            this.CalcularOpcionesHabilitadas();
        }
        /// <summary>
        /// Prepara la vista para su modalidad de edición
        /// </summary>
        public void PrepararEdicion()
        {
            this.vista.ModoEdicion = true;
            this.vista.PrepararEdicion();
            this.presentadorLinea.PrepararNuevo(null);

            this.vista.MostrarLineaContrato(false);
            this.vista.MostrarRepresentantesAval(false);
            this.vista.MostrarRepresentantesObligado(false);
            this.vista.MostrarNumeroEconomico(true);

            this.EstablecerInformacionInicial();
            this.CalcularOpcionesHabilitadas();
        }
        /// <summary>
        /// Prepara la vista para su modalidad de visualización
        /// </summary>
        public void PrepararVisualizacion()
        {
            this.vista.ModoEdicion = false;
            this.vista.PrepararVisualizacion();
            this.presentadorLinea.PrepararNuevo(null);

            this.vista.MostrarLineaContrato(false);
            this.vista.MostrarRepresentantesAval(false);
            this.vista.MostrarRepresentantesObligado(false);
            this.vista.MostrarNumeroEconomico(false);

            this.EstablecerInformacionInicial();
            this.CalcularOpcionesHabilitadas();
        }

        private void CalcularOpcionesHabilitadas()
        {
            try
            {
                //No debe permitir seleccionar una dirección de cliente a menos que se haya seleccionado una cuenta de cliente
                this.vista.PermitirSeleccionarDireccionCliente(this.vista.ModoEdicion && this.vista.CuentaClienteID != null);

                //No mostrar representantes legales, obligados solidarios ni avales si no hay una cuenta de cliente seleccionada
                this.vista.MostrarPersonasCliente(this.vista.CuentaClienteID != null);

                //Sólo permite seleccionar representantes legales si el cliente ha sido seleccionado y es moral
                this.vista.PermitirSeleccionarRepresentantes(this.vista.CuentaClienteID != null && this.vista.ClienteEsFisica != null && this.vista.ClienteEsFisica == false);
                //Sólo permite agregar representantes legales si el cliente ha sido seleccionado y tiene representantes configurados y es moral
                this.vista.PermitirAgregarRepresentantes(this.vista.ModoEdicion && this.vista.RepresentantesTotales != null && this.vista.RepresentantesTotales.Count > 0 && this.vista.ClienteEsFisica != null && this.vista.ClienteEsFisica == false);
                
                //Sólo permite seleccionar obligados solidarios si el cliente ha sido seleccionado
                this.vista.PermitirSeleccionarObligadosSolidarios(this.vista.ModoEdicion && this.vista.CuentaClienteID != null);
                //Sólo permite agregar obligados solidarios si el cliente ha sido seleccionado y tiene obligados solidarios configurados
                this.vista.PermitirAgregarObligadosSolidarios(this.vista.ModoEdicion && this.vista.ObligadosSolidariosTotales != null && this.vista.ObligadosSolidariosTotales.Count > 0);

                //Sólo permite seleccionar avales si el cliente ha sido seleccionado
                this.vista.PermitirSeleccionarAvales(this.vista.ModoEdicion && this.vista.CuentaClienteID != null);
                //Sólo permite agregar avales si el cliente ha sido seleccionado y tiene obligados solidarios configurados
                this.vista.PermitirAgregarAvales(this.vista.ModoEdicion && this.vista.AvalesTotales != null && this.vista.AvalesTotales.Count > 0);

                //No debe permitir seleccionar una unidad a menos sea modo edición
                this.vista.MostrarNumeroEconomico(this.vista.ModoEdicion);
                this.vista.PermitirSeleccionarUnidad(this.vista.ModoEdicion);

                //No debe permitir editar la tarifa a menso que sea modo edición
                this.vista.PermitirAsignarTarifasEA(this.vista.ModoEdicion);
            }
            catch (Exception ex)
            {
                throw new Exception(this.nombreClase + ".CalcularOpcionesHabilitadas: " + ex.Message);
            }
        }
        /// <summary>
        /// Configura las opciones entre obligados solidarios, avales y representantes legales con base en 'SoloRepresentantes' y 'ObligadosComoAvales'
        /// </summary>
        public void ConfigurarOpcionesPersonas()
        {
            #region Sólo Representantes
            bool soloRepresentantes = this.vista.SoloRepresentantes != null ? this.vista.SoloRepresentantes.Value : false;

            this.vista.PermitirAgregarObligadosSolidarios(this.vista.ModoEdicion && !soloRepresentantes);
            this.vista.PermitirSeleccionarObligadosSolidarios(this.vista.ModoEdicion && !soloRepresentantes);
            this.vista.PermitirAgregarAvales(this.vista.ModoEdicion && !soloRepresentantes);
            this.vista.PermitirSeleccionarAvales(this.vista.ModoEdicion && !soloRepresentantes);
            this.vista.MostrarAvales(!soloRepresentantes);
            this.vista.MostrarObligadosSolidarios(!soloRepresentantes);

            if (soloRepresentantes)
            {
                this.vista.MostrarRepresentantesObligado(false);
                this.vista.MostrarRepresentantesAval(false);

                this.vista.ObligadosSolidariosSeleccionados = null;
                this.vista.ActualizarObligadosSolidarios();

                this.vista.AvalesSeleccionados = null;
                this.vista.ActualizarAvales();

                this.vista.ObligadosComoAvales = null;
            }
            #endregion

            #region Obligados Solidarios Como Avales
            bool obligadosComoAvales = this.vista.ObligadosComoAvales != null ? this.vista.ObligadosComoAvales.Value : false;
            this.vista.PermitirAgregarAvales(this.vista.ModoEdicion && !soloRepresentantes && !obligadosComoAvales);
            this.vista.PermitirSeleccionarAvales(this.vista.ModoEdicion && !soloRepresentantes && !obligadosComoAvales);
            this.vista.MostrarAvales(!soloRepresentantes && !obligadosComoAvales);

            if (obligadosComoAvales)
            {
                this.vista.MostrarRepresentantesAval(false);

                this.vista.AvalesSeleccionados = null;
                this.vista.ActualizarAvales();
            }
            #endregion
        }
        /// <summary>
        /// Oculta la sección de representantes legales, obligados solidarios y representantes legales
        /// </summary>
        /// <param name="ocultar">Indica si se quiere o no ocultar</param>
        public void OcultarPersonasCliente(bool ocultar)
        {
            this.vista.MostrarPersonasCliente(!ocultar);
        }
        /// <summary>
        /// Oculta la sección de datos adicionales
        /// </summary>
        /// <param name="ocultar">Indica si se quiere o no ocultar</param>
        public void OcultarDatosAdicionales(bool ocultar)
        {
            this.vista.MostrarDatosAdicionales(!ocultar);
        }

        private void EstablecerInformacionInicial()
        {
            try
            {
                #region Inicializar Valores
                this.vista.NombreEmpresa = null;
                this.vista.DomicilioEmpresa = null;
                this.vista.RepresentanteEmpresa = null;
                #endregion

                if (this.vista.UnidadOperativaID == null)
                    throw new Exception("No se encontró el identificador de la unidad operativa sobre la que trabaja.");

                #region Unidad Operativa
                //Obtener información de la Unidad Operativa
                List<UnidadOperativaBO> lstUO = FacadeBR.ConsultarUnidadOperativaCompleto(this.dctx, new UnidadOperativaBO() { Id = this.vista.UnidadOperativaID });
                if (lstUO.Count <= 0)
                    throw new Exception("No se encontró la información completa de la unidad operativa sobre la que trabaja.");
                //Establecer la información de la Unidad Operativa
                if (lstUO[0].Empresa != null)
                    this.vista.NombreEmpresa = lstUO[0].Empresa.Nombre;
                #endregion

                #region Dirección de la Empresa
                //Obtener la dirección de la empresa
                List<SucursalBO> lstSuc = FacadeBR.ConsultarSucursal(this.dctx, new SucursalBO() { Matriz = true, UnidadOperativa = new UnidadOperativaBO() { Id = this.vista.UnidadOperativaID } });
                if (lstSuc.Count <= 0)
                    throw new Exception("No se encontró la sucursal matriz de la unidad operativa en la que trabaja.");
                if (lstSuc.Count > 1)
                    throw new Exception("Se encontró más de una sucursal marcada como matriz en la unidad operativa en la que trabaja.");
                SucursalBO sucBO = lstSuc[0];
                sucBO.Agregar(new DireccionSucursalBO() { Primaria = true });
                lstSuc = FacadeBR.ConsultarSucursalCompleto(this.dctx, sucBO);
                if (lstSuc.Count <= 0)
                    throw new Exception("No se encontró la información completa, y la dirección primaria, de la sucursal matriz de la unidad operativa en la que trabaja.");
                if (lstSuc[0].DireccionesSucursal.Count <= 0)
                    throw new Exception("No se encontró la dirección primaria de la sucursal matriz de la unidad operativa en la que trabaja.");
                if (lstSuc[0].DireccionesSucursal.Count > 1)
                    throw new Exception("Se encontró más de una dirección primaria en la sucursal matriz de la unidad operativa en la que trabaja.");

                //Establecer la dirección de la empresa
                this.vista.DomicilioEmpresa = lstSuc[0].DireccionesSucursal[0].Calle;
                #endregion

                #region Monedas
                List<MonedaBO> lstMonedas = FacadeBR.ConsultarMoneda(this.dctx, new MonedaBO() { Activo = true });
                this.vista.EstablecerOpcionesMoneda(lstMonedas.ToDictionary(p => p.Codigo, p => p.Nombre));
                #endregion

                #region Tipos de Inclución
                string key = "";
                int value = 0;
                Dictionary<int, string> lstTipos = new Dictionary<int, string>();
                foreach (var tipo in Enum.GetValues(typeof(ETipoInclusion)))
                {
                    var query = tipo.GetType().GetField(tipo.ToString()).GetCustomAttributes(true).Where(a => a.GetType().Equals(typeof(System.ComponentModel.DescriptionAttribute)));
                    value = Convert.ToInt32(tipo);
                    if (query.Any())
                    {
                        key = (tipo.GetType().GetField(tipo.ToString()).GetCustomAttributes(true)
                                 .Where(a => a.GetType().Equals(typeof(System.ComponentModel.DescriptionAttribute)))
                                 .FirstOrDefault() as System.ComponentModel.DescriptionAttribute).Description;
                    }
                    else
                    {
                        key = Enum.GetName(typeof(ETipoInclusion), value);
                    }
                    lstTipos.Add(value, key);
                }
                this.vista.EstablecerOpcionesIncluyeLavado(lstTipos);
                this.vista.EstablecerOpcionesIncluyeLlantas(lstTipos);
                this.vista.EstablecerOpcionesIncluyePinturaRotulacion(lstTipos);
                this.vista.EstablecerOpcionesIncluyeSeguro(lstTipos);
                #endregion

                //Período de frecuencia para las tarifas
                this.presentadorLinea.EstablecerOpcionesFrecuencia();
            }
            catch (Exception ex)
            {
                throw new Exception(this.nombreClase + ".EstablecerInformacionInicial: " + ex.Message);
            }
        }

        /// <summary>
        /// Establece la selección de una sucursal
        /// </summary>
        /// <param name="sucursal">Sucursal seleccionada</param>
        public void SeleccionarSucursal(SucursalBO sucursal)
        {
            #region Dato a Interfaz de Usuario
            if (sucursal != null && sucursal.Nombre != null)
                this.vista.SucursalNombre = sucursal.Nombre;
            else
                this.vista.SucursalNombre = null;

            if (sucursal != null && sucursal.Id != null)
                this.vista.SucursalID = sucursal.Id;
            else
                this.vista.SucursalID = null;
            #endregion
            
            this.CalcularOpcionesHabilitadas();
        }
        /// <summary>
        /// Establece la selección de una cuenta de cliente
        /// </summary>
        /// <param name="cuentaCliente">Cuenta de cliente seleccionada</param>
        public void SeleccionarCuentaCliente(CuentaClienteIdealeaseBO cuentaCliente)
        {
            try
            {
                #region Dato a Interfaz de Usuario
                if (cuentaCliente.Cliente == null)
                    cuentaCliente.Cliente = new ClienteBO();

                this.vista.CuentaClienteID = cuentaCliente.Id;
                this.vista.ClienteID = cuentaCliente.Cliente.Id;
                this.vista.CuentaClienteNombre = cuentaCliente.Nombre;
                if (cuentaCliente.TipoCuenta != null)
                    this.vista.CuentaClienteTipoID = (int)cuentaCliente.TipoCuenta;
                else
                    this.vista.CuentaClienteTipoID = null;
                this.vista.ClienteEsFisica = cuentaCliente.Cliente.Fisica;
                this.vista.ClienteNumeroCuenta = cuentaCliente.Numero;

                this.vista.ClienteDireccionCalle = null;
                this.vista.ClienteDireccionCiudad = null;
                this.vista.ClienteDireccionCodigoPostal = null;
                this.vista.ClienteDireccionColonia = null;
                this.vista.ClienteDireccionCompleta = null;
                this.vista.ClienteDireccionEstado = null;
                this.vista.ClienteDireccionMunicipio = null;
                this.vista.ClienteDireccionPais = null;
                this.vista.DireccionClienteID = null;

                //Se limpian los representantes legales
                this.vista.RepresentantesSeleccionados = null;
                this.vista.RepresentantesTotales = null;
                this.vista.SoloRepresentantes = null;

                //Se limpian los obligados solidarios
                this.vista.ObligadosSolidariosSeleccionados = null;
                this.vista.ObligadosSolidariosTotales = null;
                this.vista.ObligadosComoAvales = null;
                this.vista.RepresentantesObligadoSeleccionados = null;
                this.vista.RepresentantesObligadoTotales = null;
                this.vista.ActualizarObligadosSolidarios();

                //Se limpian los avales
                this.vista.AvalesSeleccionados = null;
                this.vista.AvalesTotales = null;
                this.vista.RepresentantesAvalSeleccionados = null;
                this.vista.RepresentantesAvalTotales = null;
                this.vista.ActualizarAvales();
                #endregion

                #region Se obtiene al cliente completo
                if (this.vista.UnidadOperativaID == null)
                    throw new Exception("No se encontró la unidad operativa sobre la que trabaja.");

                CuentaClienteIdealeaseBO cCompleto = null;
                if (cuentaCliente != null && cuentaCliente.Id != null)
                {
                    var cTemp = new CuentaClienteIdealeaseBO { Id = cuentaCliente.Id, Cliente = new ClienteBO { Id = cuentaCliente.Cliente.Id }, UnidadOperativa = new UnidadOperativaBO() { Id = this.vista.UnidadOperativaID } };
                    List<CuentaClienteIdealeaseBO> lst = new CuentaClienteIdealeaseBR().ConsultarCompleto(this.dctx, cTemp);
                    cCompleto = lst.Find(p => p.Id == this.vista.CuentaClienteID);

                    if (cCompleto == null)
                        throw new Exception("El cliente seleccionado no es válido en el sistema.");
                }
                #endregion

                #region Asignar Representantes, Obligados Solidarios y Avales
                if (cCompleto != null)
                {
                    #region Representantes Legales
                    if (cuentaCliente.Cliente.Fisica != null && cuentaCliente.Cliente.Fisica == false)
                    {
                        var lstRepActivos = new List<PersonaBO>(cCompleto.RepresentantesLegales.Where(persona => persona.Activo == true)).ConvertAll(s => (RepresentanteLegalBO)s);
                        
                        this.vista.RepresentantesTotales = lstRepActivos;
                        this.vista.RepresentantesSeleccionados = null;
                    }
                    #endregion

                    #region Obligados Solidarios y Avales
                    var obligadosActivos = new List<PersonaBO>(cCompleto.ObligadosSolidarios.Where(persona => persona.Activo == true)).ConvertAll(s => (ObligadoSolidarioBO)s);

                    this.vista.ObligadosSolidariosTotales = obligadosActivos;
                    this.vista.ObligadosSolidariosSeleccionados = null;

                    List<AvalBO> lstAvales = null;
                    if (obligadosActivos != null)
                        lstAvales = obligadosActivos.ConvertAll(s => this.ObligadoAAval(s));
                    this.vista.AvalesTotales = lstAvales;
                    this.vista.AvalesSeleccionados = null;
                    #endregion
                }

                this.vista.ActualizarRepresentantesLegales();
                #endregion

                this.CalcularOpcionesHabilitadas();
                this.ConfigurarOpcionesPersonas();
            }
            catch (Exception ex)
            {
                throw new Exception(this.nombreClase + ".SeleccionarCuentaCliente: " + ex.Message);
            }
        }

        private AvalBO ObligadoAAval(ObligadoSolidarioBO obligado)
        {
            if (obligado == null) return null;

            AvalBO aval;

            switch (obligado.TipoObligado)
            {
                case ETipoObligadoSolidario.Fisico:
                    aval = new AvalFisicoBO(obligado);
                    break;
                case ETipoObligadoSolidario.Moral:
                    aval = new AvalMoralBO(obligado);
                    if (obligado is ObligadoSolidarioMoralBO && ((ObligadoSolidarioMoralBO)obligado).Representantes != null)
                        ((AvalMoralBO)aval).Representantes = new List<RepresentanteLegalBO>(((ObligadoSolidarioMoralBO)obligado).Representantes);
                    break;
                default:
                    aval = new AvalProxyBO(obligado);
                    break;
            }

            return aval;
        }

        #region Métodos para el manejo de Representantes Legales
        /// <summary>
        /// Establece una lista de representantes legales
        /// </summary>
        /// <param name="lst">Lista de representantes legales a establecer</param>
        public void AgregarRepresentantesLegales(List<RepresentanteLegalBO> lst)
        {
            try
            {
                this.vista.RepresentantesSeleccionados = lst;

                this.vista.ActualizarRepresentantesLegales();
            }
            catch (Exception ex)
            {
                throw new Exception(this.nombreClase + ".AgregarRepresentantesLegales: " + ex.Message);
            }
        }
        /// <summary>
        /// Agrega el representante legal seleccionado en la vista
        /// </summary>
        public void AgregarRepresentanteLegal()
        {
            string s;
            if ((s = this.ValidarCamposRepresentanteLegal()) != null)
            {
                this.vista.MostrarMensaje(s, ETipoMensajeIU.INFORMACION, null);
                return;
            }

            try
            {
                if (this.vista.RepresentantesSeleccionados == null)
                    this.vista.RepresentantesSeleccionados = new List<RepresentanteLegalBO>();

                List<RepresentanteLegalBO> representantesSeleccionados = this.vista.RepresentantesSeleccionados;

                //Obtengo el representante legal de la lista de totales
                RepresentanteLegalBO bo = new RepresentanteLegalBO(this.vista.RepresentantesTotales.Find(p => p.Id == this.vista.RepresentanteLegalSeleccionadoID));
                if (bo == null)
                    throw new Exception("No se encontró el representante seleccionado.");

                representantesSeleccionados.Add(bo);

                this.AgregarRepresentantesLegales(representantesSeleccionados);
            }
            catch (Exception ex)
            {
                throw new Exception(this.nombreClase + ".AgregarRepresentanteLegal: " + ex.Message);
            }
        }
        private string ValidarCamposRepresentanteLegal()
        {
            string s = "";

            if (this.vista.RepresentanteLegalSeleccionadoID == null)
                s += "Representante Legal, ";

            if (s.Trim().CompareTo("") != 0)
                return "Los siguientes campos no pueden estar vacíos: \n" + s.Substring(0, s.Length - 2);

            if (this.vista.RepresentantesSeleccionados != null && this.vista.RepresentantesSeleccionados.Exists(p => p.Id == this.vista.RepresentanteLegalSeleccionadoID))
                return "El representante legal seleccionado ya ha sido agregado.";

            return null;
        }
        /// <summary>
        /// Quita un representante legal de los seleccionados
        /// </summary>
        /// <param name="index">Índice del representante legal a quitar</param>
        public void QuitarRepresentanteLegal(int index)
        {
            try
            {
                if (index >= this.vista.RepresentantesSeleccionados.Count || index < 0)
                    throw new Exception("No se encontró el representante legal seleccionado.");

                List<RepresentanteLegalBO> representantesLegales = this.vista.RepresentantesSeleccionados;
                representantesLegales.RemoveAt(index);

                this.vista.RepresentantesSeleccionados = representantesLegales;
                this.vista.ActualizarRepresentantesLegales();
            }
            catch (Exception ex)
            {
                throw new Exception(this.nombreClase + ".QuitarRepresentanteLegal: " + ex.Message);
            }
        }
        #endregion

        #region Métodos para el manejo de Obligados Solidarios
        /// <summary>
        /// Realiza cálculos en base al obligado solidario seleccionado con base en, por ejemplo, su tipo
        /// </summary>
        public void SeleccionarObligadoSolidario()
        {
            try
            {
                this.vista.MostrarRepresentantesObligado(false);
                this.vista.RepresentantesObligadoTotales = null;
                this.vista.RepresentantesObligadoSeleccionados = null;

                if (this.vista.ObligadoSolidarioSeleccionadoID != null)
                {
                    ObligadoSolidarioBO bo = this.vista.ObligadosSolidariosTotales.Find(p => p.Id == this.vista.ObligadoSolidarioSeleccionadoID).Clonar();
                    if (bo == null)
                        throw new Exception("No se encontró el obligado solidario seleccionado.");

                    if (bo.TipoObligado != null && bo.TipoObligado == ETipoObligadoSolidario.Moral)
                    {
                        this.vista.MostrarRepresentantesObligado(true);

                        this.vista.RepresentantesObligadoTotales = ((ObligadoSolidarioMoralBO)bo).Representantes;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(this.nombreClase + ".SeleccionarObligadoSolidario: " + ex.Message);
            }
        }
        /// <summary>
        /// Establece una lista de obligados solidarios
        /// </summary>
        /// <param name="lst">Lista de obligados solidarios a establecer</param>
        public void AgregarObligadosSolidarios(List<ObligadoSolidarioBO> lst)
        {
            try
            {
                this.vista.ObligadosSolidariosSeleccionados = lst;

                this.vista.ActualizarObligadosSolidarios();
            }
            catch (Exception ex)
            {
                throw new Exception(this.nombreClase + ".AgregarObligadosSolidarios: " + ex.Message);
            }
        }
        /// <summary>
        /// Agrega el obligado solidario seleccionado en la vista
        /// </summary>
        public void AgregarObligadoSolidario()
        {
            string s;
            if ((s = this.ValidarCamposObligadoSolidario()) != null)
            {
                this.vista.MostrarMensaje(s, ETipoMensajeIU.INFORMACION, null);
                return;
            }

            try
            {
                if (this.vista.ObligadosSolidariosSeleccionados == null)
                    this.vista.ObligadosSolidariosSeleccionados = new List<ObligadoSolidarioBO>();

                List<ObligadoSolidarioBO> obligadosSeleccionados = this.vista.ObligadosSolidariosSeleccionados;

                //Obtengo el obligado solidario de la lista de totales
                ObligadoSolidarioBO bo = this.vista.ObligadosSolidariosTotales.Find(p => p.Id == this.vista.ObligadoSolidarioSeleccionadoID).Clonar();
                if (bo == null)
                    throw new Exception("No se encontró el obligado solidario seleccionado.");

                //Si el Obligado Solidario es Moral, se completa el objeto antes de agregarlo a la lista
                if (bo.TipoObligado != null && bo.TipoObligado == ETipoObligadoSolidario.Moral)
                {
                    ((ObligadoSolidarioMoralBO)bo).Representantes = this.vista.RepresentantesObligadoSeleccionados;

                    this.vista.MostrarRepresentantesObligado(false);
                    this.vista.RepresentantesObligadoSeleccionados = null;
                    this.vista.RepresentantesObligadoTotales = null;
                }

                obligadosSeleccionados.Add(bo);

                this.AgregarObligadosSolidarios(obligadosSeleccionados);
            }
            catch (Exception ex)
            {
                throw new Exception(this.nombreClase + ".AgregarObligadoSolidario: " + ex.Message);
            }
        }
        private string ValidarCamposObligadoSolidario()
        {
            string s = "";

            if (this.vista.ObligadoSolidarioSeleccionadoID == null)
                s += "Obligado Solidario, ";

            if (s.Trim().CompareTo("") != 0)
                return "Los siguientes campos no pueden estar vacíos: \n" + s.Substring(0, s.Length - 2);

            if (this.vista.ObligadosSolidariosSeleccionados != null && this.vista.ObligadosSolidariosSeleccionados.Exists(p => p.Id == this.vista.ObligadoSolidarioSeleccionadoID))
                return "El obligado solidario seleccionado ya ha sido agregado.";

            if (this.vista.ObligadoSolidarioSeleccionadoID != null)
            {
                ObligadoSolidarioBO bo = this.vista.ObligadosSolidariosTotales.Find(p => p.Id == this.vista.ObligadoSolidarioSeleccionadoID);
                if (bo != null && bo.TipoObligado != null && bo.TipoObligado == ETipoObligadoSolidario.Moral)
                    if (!(this.vista.RepresentantesObligadoSeleccionados != null && this.vista.RepresentantesObligadoSeleccionados.Count > 0))
                        return "Es necesario seleccionar al menos un representante legal para el obligado solidario.";
            }

            return null;
        }
        /// <summary>
        /// Quita un obligado solidario de los seleccionados
        /// </summary>
        /// <param name="index">Índice del obligado solidario a quitar</param>
        public void QuitarObligadoSolidario(int index)
        {
            try
            {
                if (index >= this.vista.ObligadosSolidariosSeleccionados.Count || index < 0)
                    throw new Exception("No se encontró el obligado solidario seleccionado.");

                List<ObligadoSolidarioBO> obligadosSolidarios = this.vista.ObligadosSolidariosSeleccionados;
                obligadosSolidarios.RemoveAt(index);

                this.vista.ObligadosSolidariosSeleccionados = obligadosSolidarios;
                this.vista.ActualizarObligadosSolidarios();
            }
            catch (Exception ex)
            {
                throw new Exception(this.nombreClase + ".QuitarObligadoSolidario: " + ex.Message);
            }
        }

        public void AgregarRepresentanteObligado()
        {
            if (this.vista.RepresentanteObligadoSeleccionadoID == null)
            {
                this.vista.MostrarMensaje("Es necesario seleccionar un representante para el obligado solidario.", ETipoMensajeIU.INFORMACION, null);
                return;
            }

            try
            {
                if (this.vista.RepresentantesObligadoSeleccionados == null)
                    this.vista.RepresentantesObligadoSeleccionados = new List<RepresentanteLegalBO>();

                List<RepresentanteLegalBO> seleccionados = this.vista.RepresentantesObligadoSeleccionados;

                //Obtengo el representante de la lista de totales
                RepresentanteLegalBO bo = new RepresentanteLegalBO(this.vista.RepresentantesObligadoTotales.Find(p => p.Id == this.vista.RepresentanteObligadoSeleccionadoID));
                if (bo == null)
                    throw new Exception("No se encontró el representante seleccionado.");

                seleccionados.Add(bo);

                this.vista.RepresentantesObligadoSeleccionados = seleccionados;
            }
            catch (Exception ex)
            {
                throw new Exception(this.nombreClase + ".AgregarRepresentanteObligado: " + ex.Message);
            }
        }
        public void QuitarRepresentanteObligado()
        {
            try
            {
                //Obtengo el representante de la lista de totales
                if ((this.vista.RepresentantesObligadoSeleccionados.Find(p => p.Id == this.vista.RepresentanteObligadoSeleccionadoID)) == null)
                    throw new Exception("No se encontró el representante seleccionado.");

                int index = this.vista.RepresentantesObligadoSeleccionados.FindIndex(p => p.Id == this.vista.RepresentanteObligadoSeleccionadoID);

                List<RepresentanteLegalBO> representantes = this.vista.RepresentantesObligadoSeleccionados;
                representantes.RemoveAt(index);

                this.vista.RepresentantesObligadoSeleccionados = representantes;
            }
            catch (Exception ex)
            {
                throw new Exception(this.nombreClase + ".QuitarRepresentanteObligado: " + ex.Message);
            }
        }
        public void PrepararVisualizacionRepresentantesObligado(int index)
        {
            try
            {
                if (index >= this.vista.ObligadosSolidariosSeleccionados.Count || index < 0)
                    throw new Exception("No se encontró el obligado solidario seleccionado.");

                ObligadoSolidarioBO bo = this.vista.ObligadosSolidariosSeleccionados[index];
                if (bo is ObligadoSolidarioMoralBO)
                    this.vista.MostrarDetalleRepresentantesObligado(((ObligadoSolidarioMoralBO)bo).Representantes);
                else
                    this.vista.MostrarDetalleRepresentantesObligado(null);
            }
            catch (Exception ex)
            {
                throw new Exception(this.nombreClase + ".PrepararVisualizacionRepresentantesObligado: " + ex.Message);
            }
        }
        #endregion

        #region Métodos para el manejo de Avales
        /// <summary>
        /// Realiza cálculos en base al aval seleccionado con base en, por ejemplo, su tipo
        /// </summary>
        public void SeleccionarAval()
        {
            try
            {
                this.vista.MostrarRepresentantesAval(false);
                this.vista.RepresentantesAvalTotales = null;
                this.vista.RepresentantesAvalSeleccionados = null;

                if (this.vista.AvalSeleccionadoID != null)
                {
                    AvalBO bo = this.vista.AvalesTotales.Find(p => p.Id == this.vista.AvalSeleccionadoID).Clonar();
                    if (bo == null)
                        throw new Exception("No se encontró el aval seleccionado.");

                    if (bo.TipoAval != null && bo.TipoAval == ETipoAval.Moral)
                    {
                        this.vista.MostrarRepresentantesAval(true);

                        this.vista.RepresentantesAvalTotales = ((AvalMoralBO)bo).Representantes;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(this.nombreClase + ".SeleccionarAval: " + ex.Message);
            }
        }
        /// <summary>
        /// Establece una lista de avales
        /// </summary>
        /// <param name="lst">Lista de avales a establecer</param>
        public void AgregarAvales(List<AvalBO> lst)
        {
            try
            {
                this.vista.AvalesSeleccionados = lst;

                this.vista.ActualizarAvales();
            }
            catch (Exception ex)
            {
                throw new Exception(this.nombreClase + ".AgregarAvales: " + ex.Message);
            }
        }
        /// <summary>
        /// Agrega el aval seleccionado en la vista
        /// </summary>
        public void AgregarAval()
        {
            string s;
            if ((s = this.ValidarCamposAval()) != null)
            {
                this.vista.MostrarMensaje(s, ETipoMensajeIU.INFORMACION, null);
                return;
            }

            try
            {
                if (this.vista.AvalesSeleccionados == null)
                    this.vista.AvalesSeleccionados = new List<AvalBO>();

                List<AvalBO> avalesSeleccionados = this.vista.AvalesSeleccionados;

                //Obtengo el aval de la lista de totales
                AvalBO bo = this.vista.AvalesTotales.Find(p => p.Id == this.vista.AvalSeleccionadoID).Clonar();
                if (bo == null)
                    throw new Exception("No se encontró el aval seleccionado.");

                //Si el Aval es Moral, se completa el objeto antes de agregarlo a la lista
                if (bo.TipoAval != null && bo.TipoAval == ETipoAval.Moral)
                {
                    ((AvalMoralBO)bo).Representantes = this.vista.RepresentantesAvalSeleccionados;

                    this.vista.MostrarRepresentantesAval(false);
                    this.vista.RepresentantesAvalSeleccionados = null;
                    this.vista.RepresentantesAvalTotales = null;
                }

                avalesSeleccionados.Add(bo);

                this.AgregarAvales(avalesSeleccionados);
            }
            catch (Exception ex)
            {
                throw new Exception(this.nombreClase + ".AgregarAval: " + ex.Message);
            }
        }
        private string ValidarCamposAval()
        {
            string s = "";

            if (this.vista.AvalSeleccionadoID == null)
                s += "Aval, ";

            if (s.Trim().CompareTo("") != 0)
                return "Los siguientes campos no pueden estar vacíos: \n" + s.Substring(0, s.Length - 2);

            if (this.vista.AvalesSeleccionados != null && this.vista.AvalesSeleccionados.Exists(p => p.Id == this.vista.AvalSeleccionadoID))
                return "El aval seleccionado ya ha sido agregado.";

            if (this.vista.AvalSeleccionadoID != null)
            {
                AvalBO bo = this.vista.AvalesTotales.Find(p => p.Id == this.vista.AvalSeleccionadoID);
                if (bo != null && bo.TipoAval != null && bo.TipoAval == ETipoAval.Moral)
                    if (!(this.vista.RepresentantesAvalSeleccionados != null && this.vista.RepresentantesAvalSeleccionados.Count > 0))
                        return "Es necesario seleccionar al menos un representante legal para el aval.";
            }

            return null;
        }
        /// <summary>
        /// Quita un aval de los seleccionados
        /// </summary>
        /// <param name="index">Índice del aval a quitar</param>
        public void QuitarAval(int index)
        {
            try
            {
                if (index >= this.vista.AvalesSeleccionados.Count || index < 0)
                    throw new Exception("No se encontró el aval seleccionado.");

                List<AvalBO> avales = this.vista.AvalesSeleccionados;
                avales.RemoveAt(index);

                this.vista.AvalesSeleccionados = avales;
                this.vista.ActualizarAvales();
            }
            catch (Exception ex)
            {
                throw new Exception(this.nombreClase + ".QuitarAval: " + ex.Message);
            }
        }

        public void AgregarRepresentanteAval()
        {
            if (this.vista.RepresentanteAvalSeleccionadoID == null)
            {
                this.vista.MostrarMensaje("Es necesario seleccionar un representante para el aval.", ETipoMensajeIU.INFORMACION, null);
                return;
            }

            try
            {
                if (this.vista.RepresentantesAvalSeleccionados == null)
                    this.vista.RepresentantesAvalSeleccionados = new List<RepresentanteLegalBO>();

                List<RepresentanteLegalBO> seleccionados = this.vista.RepresentantesAvalSeleccionados;

                //Obtengo el representante de la lista de totales
                RepresentanteLegalBO bo = new RepresentanteLegalBO(this.vista.RepresentantesAvalTotales.Find(p => p.Id == this.vista.RepresentanteAvalSeleccionadoID));
                if (bo == null)
                    throw new Exception("No se encontró el representante seleccionado.");

                seleccionados.Add(bo);

                this.vista.RepresentantesAvalSeleccionados = seleccionados;
            }
            catch (Exception ex)
            {
                throw new Exception(this.nombreClase + ".AgregarRepresentanteAval: " + ex.Message);
            }
        }
        public void QuitarRepresentanteAval()
        {
            try
            {
                //Obtengo el representante de la lista de totales
                if ((this.vista.RepresentantesAvalSeleccionados.Find(p => p.Id == this.vista.RepresentanteAvalSeleccionadoID)) == null)
                    throw new Exception("No se encontró el representante seleccionado.");

                int index = this.vista.RepresentantesAvalSeleccionados.FindIndex(p => p.Id == this.vista.RepresentanteAvalSeleccionadoID);

                List<RepresentanteLegalBO> representantes = this.vista.RepresentantesAvalSeleccionados;
                representantes.RemoveAt(index);

                this.vista.RepresentantesAvalSeleccionados = representantes;
            }
            catch (Exception ex)
            {
                throw new Exception(this.nombreClase + ".QuitarRepresentanteAval: " + ex.Message);
            }
        }
        public void PrepararVisualizacionRepresentantesAval(int index)
        {
            try
            {
                if (index >= this.vista.AvalesSeleccionados.Count || index < 0)
                    throw new Exception("No se encontró el Aval seleccionado.");

                AvalBO bo = this.vista.AvalesSeleccionados[index];
                if (bo is AvalMoralBO)
                    this.vista.MostrarDetalleRepresentantesAval(((AvalMoralBO)bo).Representantes);
                else
                    this.vista.MostrarDetalleRepresentantesAval(null);
            }
            catch (Exception ex)
            {
                throw new Exception(this.nombreClase + ".PrepararVisualizacionRepresentantesAval: " + ex.Message);
            }
        }
        #endregion
        
        #region Métodos para el manejo de Líneas de Contrato
        /// <summary>
        /// Prepara la vista para agregar una nueva línea de contrato en base a la unidad seleccionada
        /// </summary>
        public void PrepararNuevaLinea()
        {
            try
            {
                this.presentadorLinea.PrepararNuevo(this.vista.UnidadID);
                this.vista.MostrarLineaContrato(true);
                this.vista.MostrarNumeroEconomico(true);
                this.vista.PermitirSeleccionarUnidad(true);
            }
            catch (Exception ex)
            {
                throw new Exception(this.nombreClase + ".PrepararNuevaLinea: " + ex.Message);
            }
        }
        /// <summary>
        /// Prepara la vista para modificar la información de una línea de contrato seleccionada
        /// </summary>
        public void PrepararEdicionLinea()
        {
            try
            {
                if (this.vista.LineaContratoSeleccionadaIndex == null)
                    throw new Exception("No se encontró el índice de la línea del contrato a modificar.");
                if (this.vista.LineasContrato == null)
                    throw new Exception("El contrato no tiene líneas para editar.");
                if (this.vista.LineasContrato.Count <= this.vista.LineaContratoSeleccionadaIndex)
                    throw new Exception("El índice de la línea de contrato seleccionada no existe.");

                this.presentadorLinea.PrepararEdicion(this.vista.LineasContrato[this.vista.LineaContratoSeleccionadaIndex.Value]);
                this.vista.MostrarLineaContrato(true);
                this.vista.MostrarNumeroEconomico(true);
                this.vista.PermitirSeleccionarUnidad(false);
            }
            catch (Exception ex)
            {
                throw new Exception(this.nombreClase + ".PrepararEdicionLinea: " + ex.Message);
            }
        }
        /// <summary>
        /// Prepara la vista para mostrar la información de una línea de contrato seleccionada
        /// </summary>
        public void PrepararVisualizacionLinea()
        {
            try
            {
                if (this.vista.LineaContratoSeleccionadaIndex == null)
                    throw new Exception("No se encontró el índice de la línea del contrato a mostrar.");
                if (this.vista.LineasContrato == null)
                    throw new Exception("El contrato no tiene líneas para mostrar.");
                if (this.vista.LineasContrato.Count <= this.vista.LineaContratoSeleccionadaIndex)
                    throw new Exception("El índice de la línea de contrato seleccionada no existe.");

                this.presentadorLinea.PrepararVisualizacion(this.vista.LineasContrato[this.vista.LineaContratoSeleccionadaIndex.Value]);
                this.vista.MostrarLineaContrato(true);
                this.vista.MostrarNumeroEconomico(true);
                this.vista.PermitirSeleccionarUnidad(false);
            }
            catch (Exception ex)
            {
                throw new Exception(this.nombreClase + ".PrepararVisualizacionLinea: " + ex.Message);
            }
        }
        /// <summary>
        /// Cancela la edición o visualización de una línea de contrato
        /// </summary>
        public void CancelarLinea()
        {
            this.vista.UnidadID = null;
            this.vista.EquipoID = null;
            this.vista.NumeroSerie = null;

            this.vista.MostrarLineaContrato(false);
            this.CalcularOpcionesHabilitadas();
        }
        /// <summary>
        /// Agrega la línea de contrato nueva o reemplaza la existente con la nueva información
        /// </summary>
        public void AplicarLineaContrato()
        {
            string s;
            if ((s = this.presentadorLinea.ValidarCampos()) != null)
            {
                this.vista.MostrarMensaje(s, ETipoMensajeIU.INFORMACION, null);
                return;
            }

            try
            {
                if (this.vista.LineasContrato == null)
                    this.vista.LineasContrato = new List<LineaContratoManttoBO>();

                List<LineaContratoManttoBO> lineas = this.vista.LineasContrato;
                LineaContratoManttoBO bo = this.presentadorLinea.ObtenerLineaContrato();

                //Se obtiene la línea de contrato en base a su ID o al ID de la Unidad (en el caso de que sean nuevos)
                LineaContratoManttoBO lcTemp = this.vista.LineasContrato.Find(p => (p.LineaContratoID != null && bo.LineaContratoID != null && p.LineaContratoID == bo.LineaContratoID)
                                                                                    ||
                                                                                    (p.Equipo != null && ((Equipos.BO.UnidadBO)p.Equipo).UnidadID != null && ((Equipos.BO.UnidadBO)p.Equipo).UnidadID == ((Equipos.BO.UnidadBO)bo.Equipo).UnidadID)
                                                                              );
                //Si existe la línea de contrato en la lista (osea, fue modificada) se reemplaza
                if (lcTemp != null)
                {
                    //Encontramos el index
                    int index = this.vista.LineasContrato.FindIndex(p => (p.LineaContratoID != null && bo.LineaContratoID != null && p.LineaContratoID == bo.LineaContratoID)
                                                                        ||
                                                                        (p.Equipo != null && ((Equipos.BO.UnidadBO)p.Equipo).UnidadID != null && ((Equipos.BO.UnidadBO)p.Equipo).UnidadID == ((Equipos.BO.UnidadBO)bo.Equipo).UnidadID)
                                                                    );
                    this.vista.LineasContrato[index] = bo;
                }
                else
                    lineas.Add(bo);

                this.AgregarLineasContrato(lineas);

                this.vista.MostrarLineaContrato(false);
                this.vista.NumeroSerie = null;
                this.vista.UnidadID = null;
                this.vista.EquipoID = null;
                this.CalcularOpcionesHabilitadas();
            }
            catch (Exception ex)
            {
                throw new Exception(this.nombreClase + ".AplicarLineaContrato: " + ex.Message);
            }
        }
        /// <summary>
        /// Establece una lista de líneas de contrato
        /// </summary>
        /// <param name="lst">Lista de líneas de contrato a establecer</param>
        public void AgregarLineasContrato(List<LineaContratoManttoBO> lst)
        {
            try
            {
                this.vista.LineasContrato = lst;

                this.vista.ActualizarLineasContrato();
            }
            catch (Exception ex)
            {
                throw new Exception(this.nombreClase + ".AgregarLineasContrato: " + ex.Message);
            }
        }
        /// <summary>
        /// Quita una línea de contrato
        /// </summary>
        /// <param name="index">Índice de la línea de contrato a quitar</param>
        public void QuitarLineaContrato()
        {
            try
            {
                if (this.vista.LineaContratoSeleccionadaIndex == null)
                    throw new Exception("No se encontró el índice de la línea del contrato a modificar.");
                if (this.vista.LineasContrato == null)
                    throw new Exception("El contrato no tiene líneas para editar.");
                if (this.vista.LineasContrato.Count <= this.vista.LineaContratoSeleccionadaIndex)
                    throw new Exception("El índice de la línea de contrato seleccionada no existe.");

                List<LineaContratoManttoBO> lineas = this.vista.LineasContrato;
                lineas.RemoveAt(this.vista.LineaContratoSeleccionadaIndex.Value);

                this.vista.LineasContrato = lineas;
                this.vista.ActualizarLineasContrato();
            }
            catch (Exception ex)
            {
                throw new Exception(this.nombreClase + ".QuitarAval: " + ex.Message);
            }
        }
        #endregion

        #region Métodos para el manejo de los Datos Adicionales
        /// <summary>
        /// Establece una lista de datos adicionales
        /// </summary>
        /// <param name="lst">Lista de datos adicionales a establecer</param>
        public void AgregarDatosAdicionales(List<DatoAdicionalAnexoBO> lst)
        {
            try
            {
                this.vista.DatosAdicionales = lst;

                this.vista.ActualizarDatosAdicionales();
            }
            catch (Exception ex)
            {
                throw new Exception(this.nombreClase + ".AgregarDatosAdicionales: " + ex.Message);
            }
        }
        /// <summary>
        /// Agrega el dato adicional seleccionado en la vista
        /// </summary>
        public void AgregarDatoAdicional()
        {
            string s;
            if ((s = this.ValidarCamposDatoAdicional()) != null)
            {
                this.vista.MostrarMensaje(s, ETipoMensajeIU.INFORMACION, null);
                return;
            }

            try
            {
                if (this.vista.DatosAdicionales == null)
                    this.vista.DatosAdicionales = new List<DatoAdicionalAnexoBO>();

                List<DatoAdicionalAnexoBO> datosAdicionales = this.vista.DatosAdicionales;

                DatoAdicionalAnexoBO bo = (DatoAdicionalAnexoBO)this.InterfazUsuarioADato("DatoAdicional");
                
                datosAdicionales.Add(bo);

                this.AgregarDatosAdicionales(datosAdicionales);

                this.DatoAInterfazUsuario(new DatoAdicionalAnexoBO(), "DATOADICIONAL");
            }
            catch (Exception ex)
            {
                throw new Exception(this.nombreClase + ".AgregarDatoAdicional: " + ex.Message);
            }
        }
        private string ValidarCamposDatoAdicional()
        {
            string s = "";

            if (this.vista.DatoAdicionalDescripcion == null)
                s += "Descripción, ";
            if (this.vista.DatoAdicionalEsObservacion == null)
                s += "¿Es Observación?, ";
            if (this.vista.DatoAdicionalEsObservacion != null && this.vista.DatoAdicionalEsObservacion != true && this.vista.DatoAdicionalTitulo == null)
                s += "Título, ";

            if (s.Trim().CompareTo("") != 0)
                return "Los siguientes campos no pueden estar vacíos: \n" + s.Substring(0, s.Length - 2);

            return null;
        }
        /// <summary>
        /// Quita un dato adicional de los seleccionados
        /// </summary>
        /// <param name="index">Índice del dato adicional a quitar</param>
        public void QuitarDatoAdicional(int index)
        {
            try
            {
                if (index >= this.vista.DatosAdicionales.Count || index < 0)
                    throw new Exception("No se encontró el dato adicional seleccionado.");

                List<DatoAdicionalAnexoBO> datosAdicionales = this.vista.DatosAdicionales;
                datosAdicionales.RemoveAt(index);

                this.vista.DatosAdicionales = datosAdicionales;
                this.vista.ActualizarDatosAdicionales();
            }
            catch (Exception ex)
            {
                throw new Exception(this.nombreClase + ".QuitarDatoAdicional: " + ex.Message);
            }
        }
        #endregion

        /// <summary>
        /// Calcula y establece la fecha de terminación del contrato con base en el plazo y la fecha de inicio
        /// </summary>
        public void CalcularFechaTerminacionContrato()
        {
            DateTime? fechaTerminacion = null;

            if (this.vista.Plazo != null && this.vista.FechaInicioContrato != null)
                fechaTerminacion = this.vista.FechaInicioContrato.Value.AddMonths(this.vista.Plazo.Value);

            this.vista.FechaTerminacionContrato = fechaTerminacion;
        }

        private void DatoAInterfazUsuario(object obj, string tipo)
        {
            switch (tipo.ToUpper())
            {
                case "DATOADICIONAL":
                    DatoAdicionalAnexoBO datoAdicional = (DatoAdicionalAnexoBO)obj;

                    this.vista.DatoAdicionalDescripcion = datoAdicional.Descripcion;
                    this.vista.DatoAdicionalEsObservacion = datoAdicional.EsObservacion;
                    this.vista.DatoAdicionalTitulo = datoAdicional.Titulo;
                    break;
            }
        }
        private object InterfazUsuarioADato(string tipo)
        {
            object obj = null;
            switch (tipo.ToUpper())
            {
                case "DATOADICIONAL":
                    DatoAdicionalAnexoBO datoAdicional = new DatoAdicionalAnexoBO();

                    datoAdicional.Descripcion = this.vista.DatoAdicionalDescripcion;
                    datoAdicional.EsObservacion = this.vista.DatoAdicionalEsObservacion;
                    datoAdicional.Titulo = this.vista.DatoAdicionalTitulo;

                    obj = datoAdicional;
                    break;
            }
            return obj;
        }

        public string ValidarCamposBorrador()
        {
            string s = string.Empty;

            if (this.vista.UnidadOperativaID == null)
                s += "Unidad Operativa, ";
            if (this.vista.SucursalID == null)
                s += "Sucursal, ";
            if (string.IsNullOrEmpty(this.vista.CodigoMoneda))
                s += "Moneda, ";
            if (this.vista.CuentaClienteID == null)
                s += "Cuenta del Cliente, ";
            if (this.vista.ClienteEsFisica == null)
                s += "Tipo de Contribuyente del Cliente, ";
            
            if (s != null && s.Trim().CompareTo("") != 0)
                return "Los siguientes campos no pueden estar vacíos: \n" + s.Substring(0, s.Length - 2);

            if (this.vista.FechaContrato.HasValue && this.vista.FechaContrato.Value.Date < DateTime.Today)
                return "La fecha del contrato no puede ser menor a la fecha actual.";
            if (this.vista.FechaInicioContrato.HasValue && this.vista.FechaInicioContrato.Value.Date < DateTime.Today)
                return "La fecha de inicio del contrato no puede ser menor a la fecha actual.";
            if (this.vista.FechaContrato.HasValue && this.vista.FechaInicioContrato.HasValue && this.vista.FechaInicioContrato < this.vista.FechaContrato)
                return "La fecha de inicio del contrato no puede ser menor a la fecha del contrato.";

            return null;
        }
        public string ValidarCamposRegistro()
        {
            string s = string.Empty;

            #region Información General
            if (this.vista.UnidadOperativaID == null)
                s += "Unidad Operativa, ";
            if (this.vista.SucursalID == null)
                s += "Sucursal, ";
            if (string.IsNullOrEmpty(this.vista.CodigoMoneda))
                s += "Moneda, ";
            if (this.vista.FechaContrato == null)
                s += "Fecha del Contrato, ";
            if (string.IsNullOrEmpty(this.vista.RepresentanteEmpresa))
                s += "Representante de la Empresa, ";
            #endregion

            #region Datos del Cliente
            if (this.vista.CuentaClienteID == null)
                s += "Cuenta del Cliente, ";
            if (this.vista.ClienteEsFisica == null)
                s += "Tipo de Contribuyente del Cliente, ";

            if (string.IsNullOrEmpty(this.vista.ClienteDireccionCalle) || string.IsNullOrEmpty(this.vista.ClienteDireccionCiudad)
                || string.IsNullOrEmpty(this.vista.ClienteDireccionEstado) || string.IsNullOrEmpty(this.vista.ClienteDireccionCodigoPostal)
                || string.IsNullOrEmpty(this.vista.ClienteDireccionPais) || string.IsNullOrEmpty(this.vista.ClienteDireccionMunicipio))
                s += "Dirección del Cliente, ";
            #endregion

            #region Información del Contrato
            if (this.vista.Plazo == null)
                s += "Plazo, ";
            if (this.vista.FechaInicioContrato == null)
                s += "Fecha de Inicio del Contrato, ";
            #endregion

            #region Unidades del Contrato
            if (!(this.vista.LineasContrato != null && this.vista.LineasContrato.Count > 0))
                s += "Unidad del Contrato, ";
            if (string.IsNullOrEmpty(this.vista.UbicacionTaller) || string.IsNullOrWhiteSpace(this.vista.UbicacionTaller))
                s += "Ubicación del Taller, ";
            if (this.vista.DepositoGarantia == null)
                s += "Depósito en Garantía, ";
            if (this.vista.ComisionApertura == null)
                s += "Comisión por Apertura, ";
            if (this.vista.IncluyeSeguroID == null)
                s += "¿Incluye Seguro?, ";
            if (this.vista.IncluyeLavadoID == null)
                s += "¿Incluye Lavado Exterior?, ";
            if (this.vista.IncluyePinturaRotulacionID == null)
                s += "¿Incluye Rotulación y Pintura?, ";
            if (this.vista.IncluyeLlantasID == null)
                s += "¿Incluye Llantas?, ";
            if (string.IsNullOrEmpty(this.vista.DireccionAlmacenaje) || string.IsNullOrWhiteSpace(this.vista.DireccionAlmacenaje))
                s += "Dirección de Almacenaje, ";
            #endregion

            if (s != null && s.Trim().CompareTo("") != 0)
                return "Los siguientes campos no pueden estar vacíos: \n" + s.Substring(0, s.Length - 2);

            if (this.vista.ClienteEsFisica != null && this.vista.ClienteEsFisica == false && !(this.vista.RepresentantesSeleccionados != null && this.vista.RepresentantesSeleccionados.Count > 0))
                return "El cliente seleccionado es Moral y, por lo tanto, requiere representantes legales";
            if (this.vista.FechaContrato.HasValue && this.vista.FechaContrato.Value.Date < DateTime.Today)
                return "La fecha del contrato no puede ser menor a la fecha actual.";
            if (this.vista.FechaInicioContrato.HasValue && this.vista.FechaInicioContrato.Value.Date < DateTime.Today)
                return "La fecha de inicio del contrato no puede ser menor a la fecha actual.";
            if (this.vista.FechaContrato.HasValue && this.vista.FechaInicioContrato.HasValue && this.vista.FechaInicioContrato < this.vista.FechaContrato)
                return "La fecha de inicio del contrato no puede ser menor a la fecha del contrato.";

            return null;
        }

        public void LimpiarSesion()
        {
            this.vista.LimpiarSesion();
            this.presentadorLinea.LimpiarSesion();
        }

        #region SC_0051
        /// <summary>
        /// Prepara la inforamción para su despliegue en la vista
        /// </summary>
        /// <param name="eaID">Identificador del equipo aliado</param>
        public void PrepararVisualizacionTarifaEquipoAliado(int eaID)
        {
            this.presentadorLinea.PrepararVisualizacionTarifaEquipoAliado(eaID);
        }
        /// <summary>
        /// Obtiene y establece la información de la tarifa correspondiente al equipo aliado seleccionado
        /// </summary>
        /// <param name="eaID">Identificador del  equipo aliado</param>
        public void AsignarTarifaEA(int eaID)
        {
            this.presentadorLinea.AsignarTarifaEA(eaID);
        }
        #endregion

        #region Métodos para el Buscador
        public object PrepararBOBuscador(string catalogo)
        {
            object obj = null;

            switch (catalogo)
            {
                case "Sucursal":
                    SucursalBOF sucursal = new SucursalBOF();
                    sucursal.UnidadOperativa = new UnidadOperativaBO();
                    sucursal.Usuario = new UsuarioBO();

                    sucursal.Nombre = this.vista.SucursalNombre;
                    sucursal.UnidadOperativa.Id = this.vista.UnidadOperativaID;
                    sucursal.Activo = true;
                    sucursal.Usuario.Id = this.vista.UsuarioID;

                    obj = sucursal;
                    break;
                case "CuentaClienteIdealease":
                    var cliente = new CuentaClienteIdealeaseBOF { Cliente = new ClienteBO() };

                    cliente.Nombre = this.vista.CuentaClienteNombre;
                    cliente.UnidadOperativa = new UnidadOperativaBO();
                    cliente.UnidadOperativa.Id = this.vista.UnidadOperativaID;
                    cliente.Activo = true;

                    obj = cliente;
                    break;
                case "DireccionCliente":
                    var cuentaCliente = new CuentaClienteIdealeaseBO();

                    cuentaCliente.Cliente = new ClienteBO();
                    cuentaCliente.Cliente.Id = this.vista.ClienteID;
                    cuentaCliente.Id = this.vista.CuentaClienteID;
                    cuentaCliente.UnidadOperativa = new UnidadOperativaBO();
                    cuentaCliente.UnidadOperativa.Id = this.vista.UnidadOperativaID;
                    
					var direccionCuentaCliente = new DireccionCuentaClienteBOF { Cuenta = cuentaCliente, Direccion = new DireccionClienteBO{Facturable = true}};

                    obj = direccionCuentaCliente;
                    break;
                case "Unidad":
                    UnidadBOF unidad = new UnidadBOF();
                    unidad.Sucursal = new SucursalBO();
                    unidad.Sucursal.UnidadOperativa = new UnidadOperativaBO();

                    if (!string.IsNullOrEmpty(this.vista.NumeroSerie))
                        unidad.NumeroSerie = this.vista.NumeroSerie;

                    unidad.EstatusActual = EEstatusUnidad.Disponible;
                    unidad.Sucursal.UnidadOperativa.Id = this.vista.UnidadOperativaID;

                    if (this.vista.TipoContratoID != null)
                        unidad.Area = (EArea)Enum.Parse(typeof(EArea), this.vista.TipoContratoID.ToString());

                    obj = unidad;
                    break;
                case "ProductoServicio":
                    obj = presentadorLinea.PrepararBOBuscador(catalogo);
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
                case "CuentaClienteIdealease":
                    CuentaClienteIdealeaseBOF cliente = (CuentaClienteIdealeaseBOF)selecto ?? new CuentaClienteIdealeaseBOF();
                    this.SeleccionarCuentaCliente(cliente);
                    break;
                case "DireccionCliente":
                    DireccionCuentaClienteBOF bof = (DireccionCuentaClienteBOF)selecto ?? new DireccionCuentaClienteBOF();
                    if (bof.Direccion == null)
                    {
                        this.vista.MostrarMensaje("No se ha seleccionado una dirección Facturable.", ETipoMensajeIU.INFORMACION, null);
                        bof.Direccion = new DireccionClienteBO();
                    }
                    if (bof.Direccion.Ubicacion == null)
                        bof.Direccion.Ubicacion = new UbicacionBO();
                    if (bof.Direccion.Ubicacion.Ciudad == null)
                        bof.Direccion.Ubicacion.Ciudad = new CiudadBO();
                    if (bof.Direccion.Ubicacion.Estado == null)
                        bof.Direccion.Ubicacion.Estado = new EstadoBO();
                    if (bof.Direccion.Ubicacion.Municipio == null)
                        bof.Direccion.Ubicacion.Municipio = new MunicipioBO();
                    if (bof.Direccion.Ubicacion.Pais == null)
                        bof.Direccion.Ubicacion.Pais = new PaisBO();

                    this.vista.ClienteDireccionCompleta = bof.Direccion.Direccion;
                    this.vista.ClienteDireccionCalle = bof.Direccion.Calle;
                    this.vista.ClienteDireccionColonia = bof.Direccion.Colonia;
                    this.vista.ClienteDireccionCodigoPostal = bof.Direccion.CodigoPostal;
                    this.vista.ClienteDireccionCiudad = bof.Direccion.Ubicacion.Ciudad.Codigo;
                    this.vista.ClienteDireccionEstado = bof.Direccion.Ubicacion.Estado.Codigo;
                    this.vista.ClienteDireccionMunicipio = bof.Direccion.Ubicacion.Municipio.Codigo;
                    this.vista.ClienteDireccionPais = bof.Direccion.Ubicacion.Pais.Codigo;
                    this.vista.DireccionClienteID = bof.Direccion.Id;
                    break;
                case "Unidad":
                    Equipos.BO.UnidadBO unidad = (Equipos.BO.UnidadBO)selecto;
                    if (unidad == null) unidad = new Equipos.BO.UnidadBO();

                    //Validamos que no la unidad no se encuentre ya seleccionada    
                    if (unidad.UnidadID != null && this.vista.LineasContrato != null && this.vista.LineasContrato.Exists(p => p.Equipo != null && ((Equipos.BO.UnidadBO)p.Equipo).UnidadID != null && ((Equipos.BO.UnidadBO)p.Equipo).UnidadID == unidad.UnidadID))
                    {
                        this.vista.MostrarMensaje("La unidad ya está asignada al contrato", ETipoMensajeIU.INFORMACION, null);
                        unidad = new Equipos.BO.UnidadBO();
                    }
                    
                    this.vista.UnidadID = unidad.UnidadID;
                    this.vista.EquipoID = unidad.EquipoID;

                    if (unidad.NumeroSerie != null && unidad.NumeroSerie.Trim().CompareTo("") != 0)
                        this.vista.NumeroSerie = unidad.NumeroSerie;
                    else
                        this.vista.NumeroSerie = null;

                    this.vista.MostrarLineaContrato(false);
                    break;
                case "ProductoServicio":
                    presentadorLinea.DesplegarResultadoBuscador(catalogo, selecto);
                    break;
            }
        }
        #endregion
        #endregion
    }
}
