//Satisface al CU085 - CATALOGO INFORMACION SEGURO DE LA UNIDAD
using System;
using System.Collections.Generic;
using BPMO.Basicos.BO;
using BPMO.Facade.SDNI.BR;
using BPMO.Patterns.Creational.DataContext;
using BPMO.Primitivos.Enumeradores;
using BPMO.SDNI.Equipos.BO;
using BPMO.SDNI.Equipos.BR;
using BPMO.SDNI.Tramites.VIS;
using BPMO.SDNI.Tramites.BO;
using BPMO.SDNI.Tramites.BR;

namespace BPMO.SDNI.Tramites.PRE
{
    public class ucSeguroPRE
    {
        #region Atributos
        private IucSeguroVIS vista;
        private IucDeducibleSeguroVIS vista1;
        private IucEndosoSeguroVIS vista2;
        private IucSiniestroSeguroVIS vista3;
        private ucDeducibleSeguroPRE presentador1;
        private ucEndosoSeguroPRE presentador2;
        private ucSiniestroSeguroPRE presentador3;
        private SeguroBR controlador;
        private IDataContext dctx = null;
        private string nombreClase = "ucSeguroPRE";
        #endregion

        #region Constructores
        public ucSeguroPRE(IucSeguroVIS view)
        {
            this.vista = view;
        }
        public ucSeguroPRE(IucSeguroVIS view, IucDeducibleSeguroVIS view1, IucEndosoSeguroVIS view2, IucSiniestroSeguroVIS view3)
        {
            try
            {
                this.vista = view;
                this.vista1 = view1;
                this.vista2 = view2;
                this.vista3 = view3;
                this.vista.Activo = true;

                this.presentador1 = new ucDeducibleSeguroPRE(view1);
                this.presentador2 = new ucEndosoSeguroPRE(view2);
                this.presentador3 = new ucSiniestroSeguroPRE(view3);

                this.controlador = new SeguroBR();
                this.dctx = FacadeBR.ObtenerConexion();
            }
            catch (Exception ex)
            {
                this.vista.MostrarMensaje("Inconsistencias en los parámetros de configuración", ETipoMensajeIU.ERROR, this.nombreClase + ".ucSeguroPRE:" + ex.Message);
            }
        }
        #endregion

        #region Métodos
        public void PrepararNuevo()
        {
            this.vista.PrepararNuevo();
            this.CargaInicial();
            this.vista.CambiarASoloLectura();
        }
        public void PrepararEdicion()
        {
            this.vista.PrepararEdicion();
            this.vista.ModoEditar();
        }
        public void PrepararVista()
        {
            this.vista.PrepararVista();
            this.vista.ModoRegistrar();
        }
        public string ValidarCampos()
        {
            string s = "";

            if (!this.vista.Activo.HasValue)
                s += "Estatus del trámite, ";
            if (string.IsNullOrEmpty(this.vista.Aseguradora) || string.IsNullOrWhiteSpace(this.vista.Aseguradora))
                s += "Aseguradora, ";
            if (string.IsNullOrEmpty(this.vista.Contacto) || string.IsNullOrWhiteSpace(this.vista.Contacto))
                s += "Contacto del seguro, ";
            if (!this.vista.FC.HasValue)
                s += "Fecha de creación, ";            
            if (string.IsNullOrEmpty(this.vista.NumeroPoliza) || string.IsNullOrWhiteSpace(this.vista.NumeroPoliza))
                s += "Número de poliza, ";
            if(!this.vista.PrimaAnual.HasValue)
                s += "Prima Anual, ";
            if (!this.vista.PrimaSemestral.HasValue)
                s += "Prima Semestral, ";
            if (!this.vista.TipoTramitable.HasValue)
                s += "Tipo del trámite, ";
            if (!this.vista.TipoTramite.HasValue)
                s += "Tipo de tramite, ";
            if (!this.vista.TramitableID.HasValue)
                s += "Identificador del tramitable, ";
            if (!this.vista.UC.HasValue)
                s += "Usuario que crea el seguro, ";
            if (!this.vista.VigenciaFinal.HasValue)
                s += "Vigencia Final, ";
            if (!this.vista.VigenciaInicial.HasValue)
                s += "Vigencia Inicial, ";
            if (string.IsNullOrEmpty(this.vista.VIN) || string.IsNullOrWhiteSpace(this.vista.VIN))
                s += "Número de serie, ";
            if (string.IsNullOrEmpty(this.vista.Observaciones) || string.IsNullOrWhiteSpace(this.vista.Observaciones))
                s += "Observaciones, ";                     
            if (s.Trim().CompareTo("") != 0)
                return "Los siguientes campos no pueden estar vacíos: \n" + s.Substring(0, s.Length - 2);

            return null;
        }
        private string ValidarFechas()
        {
            string s = string.Empty;

            if (this.vista.VigenciaFinal.HasValue)
                if (this.vista.VigenciaFinal.Value < DateTime.Now.Date)
                    s += "La vigencia final no puede ser menor a la fecha actual, ";
            if (this.vista.VigenciaInicial.HasValue && this.vista.VigenciaFinal.HasValue)
                if (this.vista.VigenciaInicial.Value.Date > this.vista.VigenciaFinal.Value.Date)
                    s += "La vigencia inicial no puede ser mayor a la vigencia final, ";

            if (s.Trim().CompareTo("") != 0)
                return s.Substring(0, s.Length - 2);

            return null;
        }
        private bool ValidarVigente()
        {
            TramitableProxyBO tramitable = new TramitableProxyBO();
            tramitable.TipoTramitable = this.vista.TipoTramitable.Value;
            tramitable.TramitableID = this.vista.TramitableID.Value;
            SeguroBO seguro = new SeguroBO { Activo = true, Tramitable = tramitable };
            List<SeguroBO> seguros = controlador.Consultar(this.dctx, seguro);

            if (seguros == null)
                return false;

            if(seguros.Count > 0)
                if(seguros.Count > 1)
                    throw new Exception("Inconcistencia en los seguros registrados, por favor revisa la informacion proporcionada");

            if(seguros.Count <= 0)
                return false;
            
            seguro = seguros[0];

            if(seguro.VigenciaFinal.HasValue)
                if(seguro.VigenciaFinal.Value.Date <  DateTime.Now.Date)
                    return false;
            return true;
        }
        public void Registrar()
        {
            try
            {
                string s;
                if ((s = this.ValidarCampos()) != null)
                {
                    this.vista.MostrarMensaje(s, ETipoMensajeIU.INFORMACION, null);
                    return;
                }

                if ((s = this.ValidarFechas()) != null)
                {
                    this.vista.MostrarMensaje(s, ETipoMensajeIU.INFORMACION, null);
                    return;
                }

                if (this.ValidarVigente())
                {
                    this.vista.MostrarMensaje("La unidad cuenta actualmente con un seguro vigente.",
                                              ETipoMensajeIU.INFORMACION, null);
                    return;
                }
                //Se obtiene la información a partir de la Interfaz de Usuario
                SeguroBO bo = (SeguroBO) this.InterfazUsuarioADato();

                #region SC0008

                //Se obtiene el objeto SeguridadBO
                SeguridadBO seguridad = ObtenerSeguridad();
                if (seguridad == null)
                    throw new Exception(nombreClase + ".Registrar():El objeto de SeguridadBO no debe se nulo");

                #endregion

                //Se inserta en la base de datos
                this.controlador.InsertarCompleto(this.dctx, bo, seguridad);
                this.vista.IrADetalle();
                this.vista.MostrarMensaje("Registro Exitoso", ETipoMensajeIU.EXITO, null);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public void Editar()
        {
            try
            {
                string s;
                if ((s = this.ValidarCampos()) != null)
                {
                    this.vista.MostrarMensaje(s, ETipoMensajeIU.INFORMACION, null);
                    return;
                }

                if ((s = this.ValidarFechas()) != null)
                {
                    this.vista.MostrarMensaje(s, ETipoMensajeIU.INFORMACION, null);
                    return;
                }

                //Se obtiene la información a partir de la Interfaz de Usuario
                SeguroBO bo = (SeguroBO) this.InterfazUsuarioADato();

                #region SC0008

                //Se obtiene el objeto SeguridadBO
                SeguridadBO seguridad = ObtenerSeguridad();
                if (seguridad == null)
                    throw new Exception(nombreClase + ".Editar():El objeto de SeguridadBO no debe se nulo");

                #endregion

                //Se inserta en la base de datos
                this.controlador.ActualizarCompleto(dctx, bo, this.vista.UltimoObjeto, seguridad);

                this.vista.MostrarMensaje("Actualizacion Exitosa", ETipoMensajeIU.EXITO, null);
                this.vista.IrADetalle();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            
        }

        public void CargaInicial()
        {
            this.EstablecerDatosNavegacion(this.vista.ObtenerDatosNavegacion());
            this.LimpiarSesion();
            this.ConsultarTramitable();
        }

        private void EstablecerDatosNavegacion(object paqueteNavegacion)
        {
            if (paqueteNavegacion == null)
                throw new Exception(this.nombreClase + ".EstablecerDatosNavegacion: Se esperaba un objeto en la navegación. No se puede identificar qué seguro se desea consultar.");
            if (!(paqueteNavegacion is SeguroBO))
                throw new Exception(this.nombreClase + ".EstablecerDatosNavegacion: Se esperaba un seguro.");
            
            SeguroBO bo = (SeguroBO)paqueteNavegacion;

            if(bo.Tramitable == null)
                throw new Exception(this.nombreClase + ".EstablecerDatosNavegacion: Se esperaba el tramitable para el seguro.");
            if (!bo.Tramitable.TramitableID.HasValue)
            {
                if (!bo.Tramitable.TipoTramitable.HasValue)
                    throw new Exception(this.nombreClase + ".EstablecerDatosNavegacion: Se esperaba el tramitable para el seguro.");
                if (string.IsNullOrEmpty(bo.Tramitable.DescripcionTramitable) || string.IsNullOrWhiteSpace(bo.Tramitable.DescripcionTramitable))
                    throw new Exception(this.nombreClase + ".EstablecerDatosNavegacion: Se esperaba el tramitable para el seguro.");            
            }
            
            this.DatoAInterfazUsuario(bo);
        }

        private void DatoAInterfazUsuario(SeguroBO bo)
        {
            this.vista.Activo = bo.Activo;
            this.vista.Aseguradora = bo.Aseguradora;
            this.vista.Contacto = bo.Contacto;
            this.vista.NumeroPoliza = bo.NumeroPoliza;
            this.vista.Observaciones = bo.Observaciones;
            this.vista.PrimaAnual = bo.PrimaAnual;
            this.vista.PrimaSemestral = bo.PrimaSemestral;
            if(bo.Tramitable != null)
            {
                if(bo.Tramitable.TipoTramitable.HasValue)
                    this.vista.TipoTramitable = bo.Tramitable.TipoTramitable.Value;
                if (bo.Tramitable is UnidadBO)
                {
                    if (((UnidadBO)bo.Tramitable) != null)
                    {
                        UnidadBO uni = (UnidadBO)bo.Tramitable;
                        if(uni.Modelo != null)
                            if(!string.IsNullOrEmpty(uni.Modelo.Nombre) && !string.IsNullOrWhiteSpace(uni.Modelo.Nombre))
                                this.vista.Modelo = ((UnidadBO)bo.Tramitable).Modelo.Nombre;
                    }                        
                }   
                if(bo.Tramitable.TramitableID.HasValue)
                    this.vista.TramitableID = bo.Tramitable.TramitableID;
                if (!string.IsNullOrEmpty(bo.Tramitable.DescripcionTramitable) && !string.IsNullOrWhiteSpace(bo.Tramitable.DescripcionTramitable))
                    this.vista.VIN = bo.Tramitable.DescripcionTramitable;
            }
            this.vista.TramiteID = bo.TramiteID;
            this.vista.VigenciaFinal = bo.VigenciaFinal;
            this.vista.VigenciaInicial = bo.VigenciaInicial;  
            if(bo.Deducibles != null)
                if(bo.Deducibles.Count > 0)
                {
                    this.vista1.Deducibles = bo.Deducibles;
                    this.vista1.ActualizarLista();
                }
            if (bo.Endosos != null)
                if (bo.Endosos.Count > 0)
                {
                    this.vista2.Endosos = bo.Endosos;
                    this.vista2.ActualizarLista();
                    this.presentador2.CalcularTotales();//RI0015
                }
            if(bo.Siniestros != null)
                if (bo.Siniestros.Count > 0)
                {
                    this.vista3.Siniestros = bo.Siniestros;
                    this.vista3.ActualizarLista();
                }
        }
        public object InterfazUsuarioADato()
        {
            SeguroBO bo = new SeguroBO();
            TramitableProxyBO tramitable = new TramitableProxyBO();
            bo.Activo = this.vista.Activo;
            bo.Aseguradora = this.vista.Aseguradora;
            bo.Contacto = this.vista.Contacto;
            if (this.vista1 != null)
            {
                if (this.vista1.Deducibles != null)
                    if (this.vista1.Deducibles.Count > 0)
                        bo.Deducibles = this.vista1.Deducibles;
            }
            if (this.vista2 != null)
            {
                if (this.vista2.Endosos != null)
                    if (this.vista2.Endosos.Count > 0)
                        bo.Endosos = this.vista2.Endosos;

            }
            if (this.vista3 != null)
            {
                if (this.vista3.Siniestros != null)
                    if (this.vista3.Siniestros.Count > 0)
                        bo.Siniestros = this.vista3.Siniestros;
            }

            if (this.vista.Modo.HasValue)
            {
                if (this.vista.Modo.Value == 1)
                {
                    if (this.vista1 != null)
                        if (this.vista1.DeduciblesBorrados != null)
                            if (this.vista1.DeduciblesBorrados.Count > 0)
                                bo.Deducibles.AddRange(this.vista1.DeduciblesBorrados);
                    if (this.vista2 != null)
                        if (this.vista2.EndososBorrados != null)
                            if (this.vista2.EndososBorrados.Count > 0)
                                bo.Endosos.AddRange(this.vista2.EndososBorrados);
                    if (this.vista3 != null)
                        if (this.vista3.SiniestrosBorrados != null)
                            if (this.vista3.SiniestrosBorrados.Count > 0)
                                bo.Siniestros.AddRange(this.vista3.SiniestrosBorrados);
                }
            }

            bo.NumeroPoliza = this.vista.NumeroPoliza;
            bo.Observaciones = this.vista.Observaciones;
            bo.PrimaAnual = this.vista.PrimaAnual;
            bo.PrimaSemestral = this.vista.PrimaSemestral;
            bo.Resultado = this.vista.NumeroPoliza;
            bo.Tipo = this.vista.TipoTramite;
            if (this.vista.TipoTramitable.HasValue)
                tramitable.TipoTramitable = (ETipoTramitable)this.vista.TipoTramitable;
            tramitable.TramitableID = this.vista.TramitableID;
            bo.Tramitable = tramitable;
            bo.Auditoria = new AuditoriaBO();
            bo.Auditoria.UC = this.vista.UC;
            bo.Auditoria.UUA = this.vista.UUA;
            bo.Auditoria.FC = this.vista.FC;
            bo.Auditoria.FUA = this.vista.FUA;
            bo.VigenciaFinal = this.vista.VigenciaFinal;
            bo.VigenciaInicial = this.vista.VigenciaInicial;
            bo.TramiteID = this.vista.TramiteID;
            return bo;
        }

        private void LimpiarSesion()
        {
            this.vista.LimpiarSesion();
            this.vista1.LimpiarSesion();
            this.vista2.LimpiarSesion();
            this.vista3.LimpiarSesion();
        }

        private void ConsultarTramitable()
        {
            try
            {
                SeguroBO bo = (SeguroBO)this.InterfazUsuarioADato();
                List<UnidadBO> lst = null;
                if (bo.Tramitable != null)
                {
                    if (bo.Tramitable.TipoTramitable.HasValue)
                    {
                        if (bo.Tramitable.TipoTramitable.Value == ETipoTramitable.Unidad)//Consultamos unidades
                        {
                            UnidadBR unidadBR = new UnidadBR();
                            UnidadBO unidad = new UnidadBO();
                            unidad.UnidadID = bo.Tramitable.TramitableID;

                            lst = unidadBR.ConsultarCompleto(this.dctx, unidad, true);
                        }
                    }
                    else if (bo.Tramitable.TramitableID.HasValue)
                    {
                        UnidadBR unidadBR = new UnidadBR();
                        UnidadBO unidad = new UnidadBO();
                        unidad.UnidadID = bo.Tramitable.TramitableID;

                        lst = unidadBR.ConsultarCompleto(this.dctx, unidad, true);
                    }
                }

                if (lst.Count < 1)
                    throw new Exception("No se encontró ningún registro que corresponda a la información proporcionada.");
                if (lst.Count > 1)
                    throw new Exception("La consulta devolvió más de un registro.");
                SeguroBO temp = new SeguroBO();
                temp.Tramitable = lst[0];
                this.DatoAInterfazUsuario(temp);
            }
            catch (Exception ex)
            {
                throw new Exception(this.nombreClase + ".ConsultarTramitable:" + ex.Message);
            }
        }

        public void CargarObjeto()
        {
            this.EstablecerDatosNavegacion(this.vista.ObtenerDatosNavegacion());
            this.LimpiarSesion();
            this.ConsultarTramitable();

            //Se obtiene la información a partir de la Interfaz de Usuario
            SeguroBO bo = (SeguroBO)this.InterfazUsuarioADato();
            List<SeguroBO> seguros = this.controlador.ConsultarCompleto(this.dctx, bo);
            if (seguros.Count > 0)
            {
                this.vista.UltimoObjeto = seguros[0];
                this.DatoAInterfazUsuario(seguros[0]);
            }
        }
        public void CargarObjeto(string key)
        {
            this.EstablecerDatosNavegacion(this.vista.ObtenerDatosNavegacion(key));
            this.LimpiarSesion();

            //Se obtiene la información a partir de la Interfaz de Usuario
            SeguroBO bo = (SeguroBO)this.InterfazUsuarioADato();
            List<SeguroBO> seguros = this.controlador.ConsultarCompleto(this.dctx, bo);
            if (seguros.Count > 0)
            {
                this.vista.UltimoObjeto = seguros[0];
                this.DatoAInterfazUsuario(seguros[0]);
            }
        }

        public void PrimaAnual()
        {
            this.vista2.PrimaAnual = this.vista.PrimaAnual;
        }

        public void IrADetalle(int? tramiteID)
        {
            if (tramiteID.HasValue)
            {
                vista.EstablecerPaqueteNavegacion("TramiteSeguro", tramiteID);                
            }
            else
            {
                vista.MostrarMensaje("No se ha proporcionado un seguro para visualizar en detalle.", ETipoMensajeIU.ADVERTENCIA);
            }
        }
        #region SC0008
        private SeguridadBO ObtenerSeguridad()
        {
            try
            {
                SeguridadBO seguridad = null;
                if (this.vista.UsuarioId == null)
                    throw new Exception(nombreClase + ".ObtenerSeguridad(): Error al intentar obtener el usuario autenticado");
                if (this.vista.UnidadOperativaId == null)
                    throw new Exception(nombreClase + ".ObtenerSeguridad(): Error al intentar obtener la Unidad Operativa de la Adscripción");

                var unidadOperativaAdscripcion = new AdscripcionBO
                {
                    UnidadOperativa = new UnidadOperativaBO { Id = this.vista.UnidadOperativaId }
                };

                var usuarioLogueado = new UsuarioBO { Id = this.vista.UsuarioId };
                seguridad = new SeguridadBO(Guid.Empty, usuarioLogueado, unidadOperativaAdscripcion);
                return seguridad;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }
        #endregion
        #endregion
    }
}