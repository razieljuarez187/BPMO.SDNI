using System;
using System.Collections.Generic;
using System.Linq;
using BPMO.Basicos.BO;
using BPMO.Facade.SDNI.BOF;
using BPMO.Patterns.Creational.DataContext;
using BPMO.Primitivos.Enumeradores;
using BPMO.SDNI.Contratos.BO;
using BPMO.SDNI.Contratos.FSL.BO;
using BPMO.SDNI.Contratos.FSL.BR;
using BPMO.SDNI.Contratos.Mantto.BO;
using BPMO.SDNI.Contratos.Mantto.BR;
using BPMO.SDNI.Contratos.RD.BO;
using BPMO.SDNI.Contratos.RD.BR;
using BPMO.SDNI.Facturacion.AplicacionesFacturacion.VIS;
using BPMO.SDNI.Facturacion.AplicacionesPago.BR;
using BPMO.SDNI.Facturacion.InfraestructuraPagos.BO;
using BPMO.SDNI.Facturacion.InfraestructuraPagos.BOF;
using BPMO.SDNI.Facturacion.InfraestructuraPagos.BR;
using BPMO.Facade.SDNI.BR;
using System.ComponentModel;
using System.Configuration;

namespace BPMO.SDNI.Facturacion.AplicacionesFacturacion.PRE
{
    /// <summary>
    /// Presentador que ayuda en la creacion de pagos adicionales a un contrato por adelantado.
    /// </summary>
    public class GenerarPagosAdicionalesPRE
    {
        #region Constantes
        /// <summary>
        /// Nombre de la clase para agregar a los mensajes de Error
        /// </summary>
        private static readonly string NombreClase = typeof(GenerarPagosAdicionalesPRE).Name;
        #endregion
        #region Atributos
        /// <summary>
        /// DataContext que provee el acceso a la base de datos
        /// </summary>
        private IDataContext dctx;
        /// <summary>
        /// Vista que contiene la informacion a utilizar.
        /// </summary>
        private IGenerarPagosAdicionalesVIS vista;
        /// <summary>
        /// Interfaz genérica del Generador de pagos
        /// </summary>
        private IGeneradorPagosBR controlador;
        /// <summary>
        /// UltimoPago Activo del Contrato Seleccionado
        /// </summary>
        private PagoUnidadContratoBO ultimoPagoContrato;
        /// <summary>
        /// Tiempo maximo de espera para la recepcion de una unidad de RD, expresado en horas
        /// </summary>
        public string TiempoMaximoRecepcionRD
        {
            get
            {
                try
                {
                    return ConfigurationManager.AppSettings["TiempoMaximoRecepcionRD"];
                }
                catch (Exception)
                {
                    return null;
                }
            }
        }
        #endregion
        #region Constructor
        public GenerarPagosAdicionalesPRE(IGenerarPagosAdicionalesVIS view)
        {
            try
            {
                vista = view;
                this.dctx = FacadeBR.ObtenerConexion();
            }
            catch(Exception ex)
            {
                this.vista.MostrarMensaje("Inconsistencias en la configuración", ETipoMensajeIU.ERROR, NombreClase + ".GenerarPagosAdicionalesPRE: " + ex.GetBaseException().Message);
            }
        }
        #endregion
        #region Metodos
        /// <summary>
        /// Metodo que se encarga de llamar a los metodos para la primera presentacion de la UI
        /// </summary>
        public void PrepararInterfaz()
        {
            this.ConsultarSucursales();
            this.ConsultarDepartamentos();
            this.Inicializar();
        }
        /// <summary>
        /// Inicializa todas propiedades de la vista
        /// </summary>
        public void Inicializar()
        {
            this.vista.FolioContrato = null;
            this.vista.ContratoID = null;
            this.vista.SucursalID = null;
            this.vista.TipoContrato = null;
        }
        /// <summary>
        /// Consulta las sucursales que tiene permiso el usuario
        /// </summary>
        public void ConsultarSucursales()
        {
            SucursalBOF sucursal = new SucursalBOF();
            sucursal.UnidadOperativa = new UnidadOperativaBO { Id = this.vista.UnidadOperativaID };
            sucursal.Usuario = new UsuarioBO { Id = this.vista.UsuarioID };
            sucursal.Activo = true;

            List<SucursalBOF> sucursalesPermitidas = FacadeBR.ConsultarSucursalesSeguridad(this.dctx, sucursal);

            this.vista.CargarSucursales(sucursalesPermitidas);
        }
        /// <summary>
        /// Obtiene los departamentos que se presentaran en la interfaz
        /// </summary>
        public void ConsultarDepartamentos()
        {
            var items = Enum.GetValues(typeof(ETipoContrato))
                            .Cast<ETipoContrato>()
                            .Select(it => new
                            {
                                Value = (int)it,
                                Text = new Func<String>(() =>
                                {
                                    String value = Enum.GetName(typeof(ETipoContrato), it);

                                    DescriptionAttribute descriptor = typeof(ETipoContrato)
                                                                         .GetField(value)
                                                                         .GetCustomAttributes(typeof(DescriptionAttribute), true)
                                                                         .Cast<DescriptionAttribute>()
                                                                         .FirstOrDefault();

                                    if(descriptor != null)
                                        value = descriptor.Description;

                                    return value;
                                }).Invoke()
                            })
                            .ToList();

            this.vista.CargarDepartamentos(items);
        }
        /// <summary>
        /// Metodo que genera el pago adicional
        /// </summary>
        public void GenerarPagoAdicional()
        {
            try
            {
                string validacion = this.ValidarPagoAdicional();
                if(!String.IsNullOrEmpty(validacion))
                {
                    this.vista.MostrarMensaje("No se puede generar el pago Adicional: " + validacion,ETipoMensajeIU.INFORMACION);
                    return;
                }

                ContratoBO contratoBo = null;
                IGeneradorPagosBR generadorPagos = null;
                SeguridadBO seguridadBo = this.CrearObjetoSeguridad();
                List<ContratoBO> contratos = new List<ContratoBO>();
                switch(vista.TipoContrato)
                {
                    case ETipoContrato.RD:
                        contratoBo = new ContratoRDBO { ContratoID = this.vista.ContratoID };
                        var contratosRd = new ContratoRDBR().ConsultarCompleto(this.dctx, (ContratoRDBO)contratoBo, false);
                        contratos.AddRange(contratosRd);
                        generadorPagos = new GeneradorPagoRDBR();                        
                        break;
                    case ETipoContrato.FSL:
                        contratoBo = new ContratoFSLBO { ContratoID = this.vista.ContratoID };
                        var contratosFsl = new ContratoFSLBR().ConsultarCompleto(this.dctx, (ContratoFSLBO)contratoBo);
                        contratos.AddRange(contratosFsl);
                        generadorPagos = new GeneradorPagosFSLBR();
                        break;
                    case ETipoContrato.CM:
                    case ETipoContrato.SD:
                        contratoBo = new ContratoManttoBO { ContratoID = this.vista.ContratoID };
                        var contratosMantto = new ContratoManttoBR().ConsultarCompleto(this.dctx, (ContratoManttoBO)contratoBo, true, false);
                        contratos.AddRange(contratosMantto);
                        generadorPagos = new GeneradorPagosManttoBR();
                        break;
                }
                if(generadorPagos == null)
                    throw new Exception("No se pudo determinar el Tipo del Pago a Generar.");

                generadorPagos.GenerarPagoAdicional(this.dctx, contratos.First(), (ultimoPagoContrato.NumeroPago.Value + 1), seguridadBo, false, true);
                PagoUnidadContratoBO pagoBo = new PagoUnidadContratoBOF { ReferenciaContrato = new ReferenciaContratoBO() { ReferenciaContratoID = this.vista.ContratoID } };
                PagoUnidadContratoBR pagoBr = new PagoUnidadContratoBR();
                var pagos = pagoBr.Consultar(this.dctx, (PagoUnidadContratoBO)pagoBo);

                this.Inicializar();

                this.vista.MostrarMensaje("El pago " + pagos.Last().NumeroPago.ToString() + " con Fecha " + pagos.Last().FechaVencimiento.Value.ToShortDateString() + " se ha Creado con Éxito.", ETipoMensajeIU.EXITO);
            }
            catch(Exception ex)
            {
                this.vista.MostrarMensaje("Error al generar el pago Adicional", ETipoMensajeIU.ERROR, NombreClase + ".GenerarPagoAdicional: " + ex.GetBaseException().Message);
            }
        }
        /// <summary>
        /// Determina si es posible generar el pago adicional del contrato
        /// </summary>
        /// <returns>Devuelve la inconsitencia encontrada para genera el pago adicional</returns>
        private string ValidarPagoAdicional()
        {
            if(this.vista.TipoContrato == null)
                return "No se ha seleccionado del Departamento del Contrato";
            if(this.vista.SucursalID == null)
                return "No se ha seleccionado la Sucursal del Contrato";
            if(String.IsNullOrEmpty(this.vista.FolioContrato))
                return "No se ha introducido el Folio del Contrato";

            ContratoBO contratoBo = null;

            List<ContratoBO> contratos = new List<ContratoBO>();
            switch(vista.TipoContrato)
            {
                case ETipoContrato.RD:
                    contratoBo = new ContratoRDBO { NumeroContrato = vista.FolioContrato, Sucursal = new SucursalBO { Id = vista.SucursalID }, Estatus = EEstatusContrato.EnCurso };
                    var contratosRD = new ContratoRDBR().Consultar(this.dctx, (ContratoRDBO)contratoBo);
                    contratos.AddRange(contratosRD);
                    break;
                case ETipoContrato.FSL:
                    contratoBo = new ContratoFSLBO { NumeroContrato = vista.FolioContrato, Sucursal = new SucursalBO { Id = vista.SucursalID }, Estatus = EEstatusContrato.EnCurso };
                    var contratosFsl = new ContratoFSLBR().Consultar(this.dctx, (ContratoFSLBO)contratoBo);
                    contratos.AddRange(contratosFsl);
                    break;
                case ETipoContrato.CM:
                case ETipoContrato.SD:
                    contratoBo = new ContratoManttoBO { NumeroContrato = vista.FolioContrato, Tipo = vista.TipoContrato, Sucursal = new SucursalBO { Id = vista.SucursalID }, Estatus = EEstatusContrato.EnCurso };
                    var contratosMantto = new ContratoManttoBR().Consultar(this.dctx, (ContratoManttoBO)contratoBo);
                    contratos.AddRange(contratosMantto);
                    break;
            }

            if(!contratos.Any())
                return "No se encontró el Contrato solicitado";
            switch (vista.TipoContrato)
            {
                case ETipoContrato.RD:
                    var contratoRD = contratos.FirstOrDefault() as ContratoRDBO;
                    if (!String.IsNullOrEmpty(TiempoMaximoRecepcionRD))
                    {
                        if (DateTime.Now < contratoRD.FechaPromesaDevolucion.Value.AddHours(int.Parse(TiempoMaximoRecepcionRD)))
                            return "Deben pasar al menos " + TiempoMaximoRecepcionRD + " hrs para poder generar un pago adicional después de la Fecha de Promesa de Devolución.";
                    }

                    break;
            }
            this.vista.ContratoID = contratos.FirstOrDefault().ContratoID;

            PagoUnidadContratoBO pagoBo = new PagoUnidadContratoBOF { ReferenciaContrato = new ReferenciaContratoBO() { ReferenciaContratoID = this.vista.ContratoID } };
            PagoUnidadContratoBR pagoBr = new PagoUnidadContratoBR();
            var pagos = pagoBr.Consultar(this.dctx, (PagoUnidadContratoBO)pagoBo);
            if(!pagos.Any())
                return "No se han generado Pagos para el Contrato";

            var ultimoPago = pagos.Last( x=>x.Activo.Value);
            if(ultimoPago.EnviadoFacturacion != null && !ultimoPago.EnviadoFacturacion.Value)
                return "No se ha Facturado el Último Pago Activo del Contrato";

            this.ultimoPagoContrato = ultimoPago;

            return null;
        }
         /// <summary>
        /// Crea un objeto de seguridad con los datos actuales de la sesión de usuario en curso
        /// </summary>
        /// <returns>Objeto de tipo seguridad</returns>
        private SeguridadBO CrearObjetoSeguridad()
        {
            UsuarioBO usuario = new UsuarioBO() { Id = this.vista.UsuarioID };
            AdscripcionBO adscripcion = new AdscripcionBO() { UnidadOperativa = new UnidadOperativaBO() { Id = this.vista.UnidadOperativaID } };
            SeguridadBO seguridadBO = new SeguridadBO(Guid.Empty, usuario, adscripcion);

            return seguridadBO;
        }
        /// <summary>
        /// Valida el acceso a la sección en curso
        /// </summary>
        public void ValidarAcceso()
        {
            try
            {
                //Valida que usuario y la unidad operativa no sean nulos
                if (this.vista.UsuarioID == null) throw new Exception("El identificador del usuario no debe ser nulo");
                if (this.vista.UnidadOperativaID == null) throw new Exception("El identificador de la Unidad Operativa no debe ser nulo");
                //Se crea el objeto de seguridad                
                SeguridadBO seguridadBO = this.CrearObjetoSeguridad();

                //Se valida si el usuario tiene permiso a la acción principal
                if (!FacadeBR.ExisteAccion(this.dctx, "CONSULTAR", seguridadBO) || !FacadeBR.ExisteAccion(this.dctx, "GENERARPAGOADICIONAL", seguridadBO))
                    this.vista.RedirigirSinPermisoAcceso();
            }
            catch (Exception ex)
            {
                throw new Exception(NombreClase + ".ValidarAcceso: " + ex.GetBaseException().Message);
            }
        }
        #endregion
    }
}
