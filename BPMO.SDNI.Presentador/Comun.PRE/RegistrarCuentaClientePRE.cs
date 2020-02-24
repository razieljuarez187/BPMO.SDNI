//Satisface al caso de uso CU068 - Catálogo de Clientes
using System;
using System.Collections.Generic;
using BPMO.Basicos.BO;
using BPMO.Facade.SDNI.BR;
using BPMO.Patterns.Creational.DataContext;
using BPMO.Primitivos.Enumeradores;
using BPMO.SDNI.Comun.BO;
using BPMO.SDNI.Comun.BR;
using BPMO.SDNI.Comun.VIS;
using System.Linq;

namespace BPMO.SDNI.Comun.PRE
{
    public class RegistrarCuentaClientePRE
    {
        #region Atributos
        private const string nombreClase = "RegistrarClientePRE";
        private readonly IDataContext dctx;
        private readonly IRegistrarCuentaClienteVIS vista;
        private CuentaClienteIdealeaseBR clienteBR;
        private ucDatosObligadoSolidarioPRE presentadorObligado;
        private ucDatosRepresentanteLegalPRE presentadorRepresentante;
        private IucDatosObligadoSolidarioVIS vistaObligado;
        private IucDatosRepresentanteLegalVIS vistaRepresentante;

        #region SC0005

        private ucDatosRepresentanteLegalPRE presentadorRepresentantesObligado;
        private IucDatosRepresentanteLegalVIS vistaRepresentantesObligado;

        #endregion

        #endregion

        #region Constructor

        public RegistrarCuentaClientePRE(IRegistrarCuentaClienteVIS vista, IucDatosObligadoSolidarioVIS vistaObligado, IucDatosRepresentanteLegalVIS vistaRepresentante, IucDatosRepresentanteLegalVIS vistaRepresentantesObligado)
        {
            try
            {
                this.vista = vista;
                this.vistaObligado = vistaObligado;
                this.vistaRepresentante = vistaRepresentante;
                this.vistaRepresentantesObligado = vistaRepresentantesObligado;
                
                presentadorObligado = new ucDatosObligadoSolidarioPRE(vistaObligado);
                presentadorRepresentante = new ucDatosRepresentanteLegalPRE(vistaRepresentante);
                presentadorRepresentantesObligado = new ucDatosRepresentanteLegalPRE(vistaRepresentantesObligado);
                if (this.vista.UnidadOperativa.Id == (int)ETipoEmpresa.Generacion || this.vista.UnidadOperativa.Id == (int)ETipoEmpresa.Equinova
                || this.vista.UnidadOperativa.Id == (int)ETipoEmpresa.Construccion)
                {
                    presentadorRepresentantesObligado.HabilitarCampos();
                }
              
                clienteBR = new CuentaClienteIdealeaseBR();
                dctx = FacadeBR.ObtenerConexion();
            }
            catch (Exception ex)
            {
                vista.MostrarMensaje("Inconsistencias en los parámetros de configuración", ETipoMensajeIU.ERROR, nombreClase + ".RegistrarClientePRE: " + ex.Message);
            }
        }

        #endregion Constructor

        #region Métodos

        #region SC0005

        public void AgregarRepresentanteLegalObligado(bool? validarEscritura = false)
        {
            string s;
            if (String.IsNullOrEmpty(s = presentadorRepresentantesObligado.ValidarCampos(true, validarEscritura)))
            {
                List<RepresentanteLegalBO> representantes = new List<RepresentanteLegalBO>(vistaObligado.RepresentantesLegales);
                RepresentanteLegalBO representante = this.presentadorRepresentantesObligado.ObtenerRepresentanteLegal();
                representante.Auditoria = new AuditoriaBO
                {
                    FC = this.vista.FC,
                    UC = this.vista.UC,
                    FUA = this.vista.FUA,
                    UUA = this.vista.UUA
                };
                representante.Activo = true;
                representantes.Add(representante);
                vistaObligado.RepresentantesLegales = representantes;
                vistaObligado.ActualizarRepresentantesLegales();
                presentadorRepresentantesObligado.PrepararNuevo();
                vista.MostrarMensaje("El representante legal se ha agregado correctamente", ETipoMensajeIU.EXITO);
                MostrarRegistro();
            }
            else
            {
                vista.MostrarMensaje("Los siguientes datos del representante son requeridas " + s.Substring(2), ETipoMensajeIU.ADVERTENCIA);
            }
        }

        public void LimpiarRepresentanteObligado()
        {
            presentadorRepresentantesObligado.PrepararNuevo();
        }

        public void MostrarDetalleObligado(ObligadoSolidarioBO obligado)
        {
            vista.MostrarDetalleObligado(((ObligadoSolidarioMoralBO)obligado).Representantes);
        }

        public void MostrarRegistro()
        {
            vista.MostrarRegistro();
        }

        public void MostrarRepresentanteObligado()
        {
            vista.MostrarRepresentanteObligado();
        }

        #endregion SC0005

        public void AgregarObligadoSolidario(bool? validarEscritura=true)
        {
            string s;
            if (String.IsNullOrEmpty(s = this.presentadorObligado.ValidarDatos(validarEscritura)))
            {
                List<ObligadoSolidarioBO> obligados = new List<ObligadoSolidarioBO>(this.vista.ObligadosSolidarios);
                ObligadoSolidarioBO obligado = presentadorObligado.ObtenerDatos();

                obligado.Auditoria = new AuditoriaBO
                {
                    FC = vista.FC,
                    UC = vista.UC,
                    FUA = vista.FUA,
                    UUA = vista.UUA
                };
                obligado.Activo = true;
                obligados.Add(obligado);
                vista.ObligadosSolidarios = obligados;

                vista.ActualizarObligadosSolidarios();
                presentadorObligado.EliminarRepresentantes();
                presentadorObligado.PrepararNuevo();
            }
            else
            {
                vista.MostrarMensaje("Se requiere los siguientes datos del Obligado Solidario: " + s.Substring(2), ETipoMensajeIU.ADVERTENCIA);
            }
        }

        public void AgregarRepresentanteLegal(bool? validarrfc = true, bool? validarEscritura=true)
        {
            string s;
            if (String.IsNullOrEmpty(s = presentadorRepresentante.ValidarCampos(validarrfc, validarEscritura)))
            {
                List<RepresentanteLegalBO> representantes = new List<RepresentanteLegalBO>(vista.RepresentantesLegales);
                RepresentanteLegalBO representante = presentadorRepresentante.ObtenerRepresentanteLegal();
                representante.Auditoria = new AuditoriaBO
                {
                    FC = vista.FC,
                    UC = vista.UC,
                    FUA = vista.FUA,
                    UUA = vista.UUA
                };
                representante.Activo = true;
                representantes.Add(representante);
                vista.RepresentantesLegales = representantes;
                presentadorRepresentante.PrepararNuevo();
                vista.ActualizarRepresentantesLegales();
            }
            else
            {
                vista.MostrarMensaje("Los siguientes datos del representante son requeridas " + s.Substring(2), ETipoMensajeIU.ADVERTENCIA);
            }
        }

        public bool ExisteCliente(CuentaClienteIdealeaseBO cliente)
        {
            if (cliente == null) throw new Exception("Se esperaba un Cliente");
            if (cliente.Id == null) throw new Exception("se esperaba un Cliente");
            if (cliente.Cliente == null) throw new Exception("se esperaba un Cliente");
            if (cliente.Cliente.Id == null) throw new Exception("se esperaba un Cliente");

            List<CuentaClienteIdealeaseBO> lst = this.clienteBR.Consultar(this.dctx, cliente);
            if (lst.Count > 0)
                return true;

            return false;
        }

        public void Inicializar(bool cliente)
        {
            if (cliente == false)
            {
                this.LimpiarSesion();
                this.PrepararNuevo();
                this.MostrarTipoCuenta();
                this.vista.DeshabilitarCampos();
                this.presentadorObligado.PrepararNuevo();
                this.vistaObligado.HabilitarCampos(false);
                this.presentadorRepresentante.PrepararNuevo();
                this.presentadorObligado.EstablecerAcciones(vista.ListaAcciones);
                this.vistaRepresentante.EstablecerAcciones(vista.ListaAcciones, false);
            }
            if (cliente == true)
            {
                if (this.vista.Fisica == true)
                {
                    this.vista.HabilitarCampos();
                    this.vista.OcultarActaConstitutiva();
                    this.vista.MostrarHacienda();
                }
                if (this.vista.Fisica == false)
                {
                    this.vista.HabilitarCampos();
                    this.vista.MostrarActaConstitutiva();
                    this.vista.OcultarHacienda();
                }
                if (this.vista.Fisica == null)
                {
                    this.vista.MostrarMensaje("Cliente no válido", ETipoMensajeIU.ADVERTENCIA);
                    return;
                }
                this.vistaObligado.HabilitarCampos(true);
                this.vistaRepresentante.HabilitarCampos(true);
            }
        }

        public CuentaClienteIdealeaseBO InterfazUsuarioADato()
        {
            
            CuentaClienteIdealeaseBO cliente;
            cliente = vista.Cliente;			
            cliente.QuitarActas();
            cliente.Agregar(this.vista.ActasConstitutivas);
			
            cliente.AuditoriaIdealease = new AuditoriaBO
            {
                FC = vista.FC,
                FUA = vista.FUA,
                UC = vista.UC,
                UUA = vista.UUA
            };
            cliente.Activo = true;
            cliente.ObligadosSolidarios = new List<PersonaBO>();
            cliente.RepresentantesLegales = new List<PersonaBO>();
            cliente.FechaRegistroHacienda = vista.FechaRegistro;
            cliente.GiroEmpresa = vista.GiroEmpresa;
            cliente.CURP = vista.CURP;
            #region SC0001
            cliente.DiasUsoUnidad = vista.DiasUsoUnidad;
            cliente.HorasUsoUnidad = vista.HorasUsoUnidad;
            cliente.Correo = vista.Correo;
            #endregion
            cliente.TipoCuenta = vista.TipoCuenta;
            cliente.ObligadosSolidarios = vista.ObligadosSolidarios.ConvertAll(s => (PersonaBO)s);
            cliente.RepresentantesLegales = vista.RepresentantesLegales.ConvertAll(s => (PersonaBO)s);
            cliente.Cliente.Fisica = this.vista.Fisica;
            cliente.SectorCliente = this.vista.SectorCliente;
            cliente.UnidadOperativaId = this.vista.UnidadOperativa.Id;
            cliente.Observaciones = this.vista.Observaciones;

            if (this.vista.UnidadOperativa.Id == (int)ETipoEmpresa.Generacion || this.vista.UnidadOperativa.Id == (int)ETipoEmpresa.Equinova
                || this.vista.UnidadOperativa.Id == (int)ETipoEmpresa.Construccion)
            {
                cliente.Agregar((List<TelefonoClienteBO>)this.InterfazUsuarioADatoTelefonos());
            }
            
            return cliente;
        }

        public object InterfazUsuarioADatoTelefonos()
        {
            List<TelefonoClienteBO> lstTelefonos = new List<TelefonoClienteBO>();
            TelefonoClienteBO telefonos;
            CuentaClienteIdealeaseBO Cliente;
            
            foreach(TelefonoClienteBO TelefonoBO in this.vista.ListaTelefonos)
            {
                telefonos = new TelefonoClienteBO();
                Cliente = new CuentaClienteIdealeaseBO();
                telefonos.Telefono = TelefonoBO.Telefono;
                telefonos.CuentaClienteID = this.vista.Cliente.Id;
                telefonos.Auditoria = new AuditoriaBO
                {
                    FC = this.vista.FC,
                    FUA = this.vista.FUA,
                    UC = this.vista.UC,
                    UUA = this.vista.UUA
                };
                telefonos.Activo = true;
                lstTelefonos.Add(telefonos);
            }
            return lstTelefonos;           
        }        

        public void LimpiarSesion()
        {
            this.vista.LimpiarSesion();
        }

        public void MostrarTipoCuenta()
        {
            this.vista.MostrarTipoCuenta();
        }

        public void PrepararNuevo()
        {
            this.LimpiarSesion();
            this.vista.NombreCliente = null;
            this.vista.Fisica = null;
            this.vista.RFC = null;			
            this.vista.ActaConstitutivaSeleccionada = null;
            this.vista.ActasConstitutivas = null;			
            this.vista.CURP = null;
            this.vista.NumeroCuentaOracle = null;
            this.vista.ActualizarObligadosSolidarios();
            this.vista.ActualizarRepresentantesLegales();
            this.EstablecerSeguridad();
            this.EstablecerAcciones();
            this.vista.MostrarObservaciones();
            this.vista.ReiniciarCampos();
        }

        public void QuitarObligadoSolidario(ObligadoSolidarioBO obligado)
        {
            try
            {
                if (obligado != null)
                {
                    if (this.vista.ObligadosSolidarios.Contains(obligado))
                    {
                        List<ObligadoSolidarioBO> obligados = new List<ObligadoSolidarioBO>(this.vista.ObligadosSolidarios);
                        obligados.Remove(obligado);
                        this.vista.ObligadosSolidarios = obligados;
                        this.vista.ActualizarObligadosSolidarios();
                    }
                    else
                        throw new Exception("El Obligado Solidario proporcionado no se encuentra en la lista");
                }
                else
                    throw new Exception("Se requiere un Obligado Solidario válido para la operación");
            }
            catch (Exception ex)
            {
                this.vista.MostrarMensaje("Inconsistencias al intentar quitar un Obligado Solidario de la lista", ETipoMensajeIU.ERROR, nombreClase + ".QuitarObligadoSolidario: " + ex.Message);
            }
        }

        public void QuitarRepresentanteLegal(RepresentanteLegalBO representante)
        {
            try
            {
                if (representante != null)
                {
                    if (this.vista.RepresentantesLegales.Contains(representante))
                    {
                        List<RepresentanteLegalBO> representantes = new List<RepresentanteLegalBO>(this.vista.RepresentantesLegales);
                        representantes.Remove(representante);
                        this.vista.RepresentantesLegales = representantes;
                        this.vista.ActualizarRepresentantesLegales();
                    }
                    else
                        throw new Exception("El Representante Legal proporcionado no se encuentra en la lista");
                }
                else
                    throw new Exception("Se requiere un Representante Legal válido para la operación");
            }
            catch (Exception ex)
            {
                this.vista.MostrarMensaje("Inconsistencias al intentar quitar un Representante Legal de la lista", ETipoMensajeIU.ERROR, nombreClase + ".QuitarRepresentanteLegal: " + ex.Message);
            }
        }

        public void RedirigirADetalle()
        {
            try
            {
                if (this.vista.Cliente != null)
                {
                    this.vista.EstablecerDatosNavegacion("DatosCuentaClienteIdealeaseBO", this.vista.Cliente);
                    this.LimpiarSesion();
                    this.vista.RedirigirADetalle();
                }
            }
            catch (Exception ex)
            {
                this.vista.MostrarMensaje("Inconsistencias al mostrar la información", ETipoMensajeIU.ERROR, nombreClase + ".DatoAInterfazUsuario:" + ex.Message);
            }
        }

        public void RegistrarCliente()
        {
            string s;
            if (!String.IsNullOrEmpty((s = this.ValidarDatos())))
            {
                this.vista.MostrarMensaje("Los siguientes datos no pueden estar vacíos " + s.Substring(2), ETipoMensajeIU.ADVERTENCIA, null);
                return;
            }

            try
            {
                ETipoEmpresa empresa = (ETipoEmpresa) vista.UnidadOperativa.Id;
                
                CuentaClienteIdealeaseBO cliente = (CuentaClienteIdealeaseBO)this.InterfazUsuarioADato();
                if (this.ExisteCliente(cliente) == true)
                    this.vista.MostrarMensaje("La información del Cliente que proporcionó ya se encuentra registrado en el sistema. Favor de verificar.", ETipoMensajeIU.INFORMACION, null);
                #region SC0008
                SeguridadBO seguridad = new SeguridadBO(Guid.Empty, new UsuarioBO() { Id = this.vista.UC },
                    new AdscripcionBO() { UnidadOperativa = new UnidadOperativaBO() { Id = this.vista.UnidadOperativa.Id } });

                this.clienteBR.InsertarCompleto(dctx, cliente, seguridad);
                #endregion SC0008


                List<CuentaClienteIdealeaseBO> lst = this.clienteBR.ConsultarCompleto(dctx, cliente);
                if (lst.Count <= 0)
                    throw new Exception("Al consultar lo insertado no se encontraron coincidencias.");
                if (lst.Count > 1)
                    throw new Exception("Al consultar lo insertado se encontró más de una coincidencia.");


                this.vista.RepresentantesLegales =
                    lst[0].RepresentantesLegales.ConvertAll(r => (RepresentanteLegalBO) r);
                this.vista.ObligadosSolidarios =
                    lst[0].ObligadosSolidarios.ConvertAll(o => (ObligadoSolidarioBO) o);
                CuentaClienteIdealeaseBO clienteTemp = new CuentaClienteIdealeaseBO();
                clienteTemp = lst[0];
                clienteTemp.UnidadOperativa = this.vista.UnidadOperativa;

                //this.vista.Cliente = lst[0];
                List<CuentaClienteIdealeaseBO> lstTemp = this.clienteBR.Consultar(dctx, clienteTemp);
                if (lstTemp.Count == 1)
                {
                    this.vista.Cliente = lstTemp[0];
                    this.RedirigirADetalle();
                }
                else
                    throw new Exception("Error al recuperar los datos del cliente");
                
            }
            catch (Exception ex)
            {
                this.vista.MostrarMensaje("Error al Registrar los datos del Cliente", ETipoMensajeIU.ERROR, nombreClase + ".RegistrarCliente: " + ex.Message);
            }
        }

        public string ValidarDatos()
        {
            string s = string.Empty;
            ETipoEmpresa empresa = (ETipoEmpresa) this.vista.UnidadOperativa.Id;
            
            try
            {
                if (this.vista.Cliente.Id != null)
                {
                    if (this.vista.Fisica != null && this.vista.Fisica == false)
                    {
						if(empresa == ETipoEmpresa.Idealease)
                            s += this.ValidarActaConstitutiva();					    
                        if (this.vista.TipoCuenta == null)
                            s += ", Tipo de cuenta";
                        if (this.vista.RepresentantesLegales == null || this.vista.RepresentantesLegales.Count == 0)
                            s += ", Representantes Legales";
                        if (this.vista.ObligadosSolidarios == null || this.vista.ObligadosSolidarios.Count == 0)
                            s += ", Obligados Solidarios";
                    }
                    else if (this.vista.Fisica != null && this.vista.Fisica == true)
                    {
                        if (this.vista.TipoCuenta == null)
                            s += ", Tipo de cuenta";
                        if (this.vista.FechaRegistro == null)
                            s += ", Fecha de Registro";
                        if (this.vista.GiroEmpresa == null)
                            s += ", Giro de la Empresa";
                        if (this.vista.CURP == null)
                            s += ", CURP";
                        #region SC0001
                        //if (this.vista.DiasUsoUnidad == null)
                        //    s += ", DiasUsoUnidad";
                        //if (this.vista.HorasUsoUnidad == null)
                        //    s += ", HorasUsoUnidad";
                        //if (this.vista.Correo == null)
                        //    s += ", Correo";
                        #endregion 
                        if (vista.Fisica == false)
                            if (this.vista.RepresentantesLegales == null || this.vista.RepresentantesLegales.Count == 0)
                                s += ", Representantes Legales";
                        if (this.vista.ObligadosSolidarios == null || this.vista.ObligadosSolidarios.Count == 0)
                            s += ", Obligados Solidarios";
                        if (this.vista.CURP != null && this.vista.CURP.Length != 18)
                            s += ", El CURP debe ser de 18 caracteres";
                    }
                    else if (this.vista.Fisica == null)
                    {
                        throw new Exception("Cliente no válido");
                    }
                }
                else
                {
                    throw new Exception("Se requiere un Cliente");
                }
            }
            catch (Exception ex)
            {
                vista.MostrarMensaje("Inconsistencias al validar los datos", ETipoMensajeIU.ERROR, nombreClase + ".ValidarDatos: " + ex.Message);
            }

            return s;
        }	
        /// <summary>
        /// Valida que se hayan capturado actas constitutivas
        /// </summary>
        /// <returns></returns>
        private string ValidarActaConstitutiva()
        {
            return this.vista.ActasConstitutivas == null || this.vista.ActasConstitutivas.Count == 0
                || !this.vista.ActasConstitutivas.Exists(a => a.Activo == true) ? 
                ", No se ha capturado un acta constitutiva activa para la cuenta actual" : string.Empty;
        }

        /// <summary>
        /// Método que establece la lista de acciones de los user controls, e inicializa el método de la vista.
        /// </summary>
        private void EstablecerAcciones()
        {
            this.presentadorRepresentante.EstablecerAcciones(this.vista.ListaAcciones);
            this.vista.EstablecerAcciones();
        }

        #region SC_0008
        private void EstablecerSeguridad()
        {
            try
            {
                //se valida que los datos del usuario y la unidad operativa no sean nulos
                if (this.vista.UC == null) throw new Exception("El identificador del usuario no debe ser nulo");
                if (this.vista.UnidadOperativa == null) throw new Exception("La Unidad Operativa no debe ser nula ");
                if (this.vista.UnidadOperativa.Id == null) throw new Exception("El identificador de la Unidad Operativa no debe ser nulo ");

                //Se crea el objeto de seguridad
                UsuarioBO usuario = new UsuarioBO() { Id = this.vista.UC };
                AdscripcionBO adscripcion = new AdscripcionBO() { UnidadOperativa = new UnidadOperativaBO() { Id = this.vista.UnidadOperativa.Id } };
                SeguridadBO seguridadBO = new SeguridadBO(Guid.Empty, usuario, adscripcion);

                //Se obtienen las acciones a las cuales el usuario tiene permiso en este proceso
                #region Asignacion a ListaAcciones de Implementacion de cuentas Construcción y Generación.
                this.vista.ListaAcciones = FacadeBR.ConsultarAccion(this.dctx, seguridadBO);
                #endregion
                //Se valida si el usuario tiene permiso para insertar cuenta cliente
                if (!this.ExisteAccion(this.vista.ListaAcciones, "INSERTARCOMPLETO"))
                    this.vista.RedirigirSinPermisoAcceso();
            }
            catch (Exception ex)
            {
                throw new Exception(nombreClase + ".EstablecerSeguridad:" + ex.Message);
            }
        }
        private bool ExisteAccion(List<CatalogoBaseBO> acciones, string nombreAccion)
        {
            if (acciones != null && acciones.Count > 0 && acciones.Exists(p => p != null && p.Nombre != null && p.Nombre.Trim().ToUpper() == nombreAccion.Trim().ToUpper()))
                return true;

            return false;
        }
        #endregion

        #region MetodosBuscador

        /// <summary>
        /// Despliega el Resultado del Buscador
        /// </summary>
        /// <param name="catalogo">Catalogo en el que se realizo la busqueda</param>
        /// <param name="selecto">Objeto Resultante</param>
        public void DesplegarResultadoBuscador(string catalogo, object selecto)
        {
            switch (catalogo)
            {
                case "Cliente":
                    CuentaClienteBO cliente = (CuentaClienteBO)selecto ??
                                                        new CuentaClienteBO();
                    if (cliente.Cliente == null)
                        cliente.Cliente = new ClienteBO();
                    CuentaClienteIdealeaseBO vistaCliente = new CuentaClienteIdealeaseBO();
                    vistaCliente.UnidadOperativa = new UnidadOperativaBO();
                    vistaCliente.Cliente = new ClienteBO();
                    vistaCliente.UnidadOperativa = this.vista.UnidadOperativa;
                    vistaCliente.Id = cliente.Id;
                    vistaCliente.Cliente.Id = cliente.Cliente.Id;
                    vistaCliente.Activo = true;
                    vistaCliente.Numero = cliente.Numero;
                    if (vistaCliente.Id != null)
                    {
                        if (this.ExisteCliente(vistaCliente) != true)
                        {
                            vista.NombreCliente = !string.IsNullOrEmpty(cliente.Nombre) ? cliente.Nombre : string.Empty;
                            vista.RFC = cliente.Cliente.RFC;
                            vista.Fisica = cliente.Cliente.Fisica;
                            vista.NombreCliente = cliente.Nombre;
                            vista.Cliente = vistaCliente;
                            vista.NumeroCuentaOracle = vistaCliente.Numero;
                            Inicializar(true);
                        }
                        else
                        {
                            this.vista.DeshabilitarCampos();
                            this.vistaObligado.HabilitarCampos(false);
                            this.vistaRepresentante.HabilitarCampos(true);
                            vistaRepresentante.HabilitarCampos(false);
                            this.vista.MostrarMensaje("La información del Cliente que proporcionó ya se encuentra registrado en el sistema. Favor de verificar.", ETipoMensajeIU.ADVERTENCIA, null);
                        }
                    }
                    break;
            }
        }

        /// <summary>
        /// Prepara un BO para la Busqueda en su respectivo catalogo
        /// </summary>
        /// <param name="catalogo">catalogo donde se realizara la busqueda</param>
        /// <returns></returns>
        public object PrepararBOBuscador(string catalogo)
        {
            object obj = null;

            switch (catalogo)
            {
                case "Cliente":
                    CuentaClienteBO cliente = new CuentaClienteBO { Nombre = vista.NombreCliente, UnidadOperativa = vista.UnidadOperativa, Cliente = new ClienteBO(), Activo = true }; // Se agrega que solo se puedan buscar Cuentas de Cliente Activos
                    obj = cliente;
                    break;
            }

            return obj;
        }

        #endregion MetodosBuscador

        #endregion Métodos

        /// <summary>
        /// Despliega información del acta constitutiva
        /// </summary>
        /// <param name="actaId">Identificador del acta a desplegar</param>
        public void MostrarActaConstitutiva(int actaId)
        {
            try
            {
                var actaEncontrada = this.vista.ActasConstitutivas.FirstOrDefault(a => a.Id == actaId);
                if (actaEncontrada != null)
                    this.vista.ActaConstitutivaSeleccionada = actaEncontrada;
                else
                    this.vista.MostrarMensaje("No se encontró el acta constitutiva buscada", ETipoMensajeIU.INFORMACION);
            }
            catch (Exception ex)
            {
                this.vista.MostrarMensaje("Inconsistencias al presentar la información", ETipoMensajeIU.ERROR, ex.Message);
            }
        }       
        /// <summary>
        /// Agrega o actualiza información del acta constitutiva 
        /// </summary>
        /// <returns>Retorna TRUE si la operación se pudo realizar</returns>
        public bool AgregarActaConstitutiva(bool? validarrfc=true)
        {
            bool result = false;
            try
            {
                string msj = this.vista.ValidarActaConstitutiva(validarrfc);

                if (msj != "NOAPLICA")
                {
                    if (string.IsNullOrWhiteSpace(msj))
                    {
                        List<ActaConstitutivaBO> listaActas = this.vista.ActasConstitutivas;
                        var actaConstitutiva = this.vista.ActaConstitutivaSeleccionada;
                        if (listaActas == null)
                            listaActas = new List<ActaConstitutivaBO>();
                        if (actaConstitutiva.Activo == true && ((actaConstitutiva.Id.HasValue && listaActas.Where(a => a.Id != actaConstitutiva.Id).Any(a => a.Activo == true))
                            || !actaConstitutiva.Id.HasValue && listaActas.Exists(a => a.Activo == true)))
                            this.vista.MostrarMensaje("Ya existe un acta constitutiva activa. Favor de verificar su captura", ETipoMensajeIU.INFORMACION);
                        else
                        {
                            if (actaConstitutiva.Id.HasValue)
                            {
                                var actaBO = listaActas.FirstOrDefault(a => a.Id == actaConstitutiva.Id);
                                if (actaBO == null)
                                    listaActas.Add(actaConstitutiva);
                                else
                                {
                                    actaBO.NumeroEscritura = actaConstitutiva.NumeroEscritura;
                                    actaBO.FechaEscritura = actaConstitutiva.FechaEscritura;
                                    actaBO.NombreNotario = actaConstitutiva.NombreNotario;
                                    actaBO.NumeroNotaria = actaConstitutiva.NumeroNotaria;
                                    actaBO.LocalidadNotaria = actaConstitutiva.LocalidadNotaria;
                                    actaBO.NumeroRPPC = actaConstitutiva.NumeroRPPC;
                                    actaBO.FechaRPPC = actaConstitutiva.FechaRPPC;
                                    actaBO.LocalidadRPPC = actaConstitutiva.LocalidadRPPC;
                                    actaBO.Activo = actaConstitutiva.Activo;
                                    result = true;
                                }
                            }
                            else
                            {
                                actaConstitutiva.Id = listaActas.Count > 0 ? listaActas.Max(a => a.Id) + 1 : 1;
                                listaActas.Add(actaConstitutiva);
                                result = true;
                            }
                            this.vista.ActasConstitutivas = listaActas;
                        }
                    }
                    else
                        this.vista.MostrarMensaje("Los siguientes datos no pueden estar vacíos " + msj.Substring(2), ETipoMensajeIU.ADVERTENCIA);
                }
            }
            catch (Exception ex)
            {
                this.vista.MostrarMensaje("Inconsistencias al mostrar la información", ETipoMensajeIU.ERROR, ex.Message);
            }
            return result;
        }
    }
}
