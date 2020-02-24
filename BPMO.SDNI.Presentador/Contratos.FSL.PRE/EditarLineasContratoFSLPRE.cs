//Construccion durante staffing - Eliminar unidades de un contrato en curso

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using BPMO.Basicos.BO;
using BPMO.Facade.SDNI.BR;
using BPMO.Patterns.Creational.DataContext;
using BPMO.Primitivos.Enumeradores;
using BPMO.SDNI.Contratos.FSL.BO;
using BPMO.SDNI.Contratos.FSL.BR;
using BPMO.SDNI.Contratos.FSL.VIS;
using BPMO.SDNI.Equipos.BO;
using BPMO.SDNI.Equipos.BOF;
using BPMO.SDNI.Equipos.BR;
using BPMO.SDNI.Facturacion.AplicacionesPago.BR;
using BPMO.SDNI.Tramites.BO;
using BPMO.SDNI.Tramites.BR;
using EFrecuencia = BPMO.SDNI.Contratos.FSL.BO.EFrecuencia;

namespace BPMO.SDNI.Contratos.FSL.PRE
{
    public class EditarLineasContratoFSLPRE
    {
        #region Constantes
        /// <summary>
        /// Nombre de la clase del Presentador, usada para excepciones
        /// </summary>
        private readonly string NombreClase = typeof (EditarContratoFSLPRE).Name;
        /// <summary>
        /// El DataContext que provee el acceso a la base de datos
        /// </summary>
        private readonly IDataContext dataContext;
        /// <summary>
        /// Vista que contiene la union con la UI
        /// </summary>
        private readonly IEditarLineasContratoFSLVIS vista;
        /// <summary>
        /// BR de Contrato de FSL
        /// </summary>
        private readonly ContratoFSLBR controlador;
        #endregion
        #region Constructor
        /// <summary>
        /// Constructor del Presentador
        /// </summary>
        /// <param name="vista">Vista con la cual interactuara el Presentador</param>
        public EditarLineasContratoFSLPRE(IEditarLineasContratoFSLVIS vista)
        {
            try
            {
                if(vista == null)
                    throw new Exception("La vista no puede ser nula");

                this.dataContext = FacadeBR.ObtenerConexion();
                this.vista = vista;
                controlador = new ContratoFSLBR();
            }
            catch (Exception ex)
            {
                throw new Exception(NombreClase + ".EditarLineasContratoFSLPRE: " + ex.Message, ex.InnerException);
            }
        }
        #endregion
        #region Metodos
        /// <summary>
        /// Inicia la Edicion de las Lineas de Contrato de FSL
        /// </summary>
        public void Inicializar()
        {
            try
            {
                PrepararEdicion();
            }
            catch (Exception ex)
            {
                throw new Exception(NombreClase+ ".Inicializar(): " + ex.Message, ex.InnerException);
            }
        }
        /// <summary>
        /// Prepara la Edicion en la seccion de Lineas de Contrato
        /// </summary>
        private void PrepararEdicion()
        {
            vista.LimpiarSesion();
            vista.LimpiarInterfazLineas();
            DeshabilitarCamposVistaLineas();
            
            var diccionarioTipoCotizacion = new Dictionary<string, string>();
            diccionarioTipoCotizacion.Add(((Int32)ETipoCotizacion.Average).ToString(), ETipoCotizacion.Average.ToString());
            diccionarioTipoCotizacion.Add(((Int32)ETipoCotizacion.Step).ToString(), ETipoCotizacion.Step.ToString());
            vista.PresentarTipoCotizacion(diccionarioTipoCotizacion);

            var contratoFslBo = (ContratoFSLBO)vista.ObtenerPaqueteNavegacion();
            if(contratoFslBo == null)
                throw new Exception("No se pudo obtener el contrato a Editar.");
            if (contratoFslBo.ContratoID == null)
                throw new Exception("No se pudo obtener el Identificador del contrato a Editar.");

            var listaContratosFsl = new List<ContratoFSLBO>();
            try { listaContratosFsl = controlador.ConsultarCompleto(dataContext, contratoFslBo); }
            catch (Exception ex) { throw new Exception("Ocurrio un Problema al consultar el Contrato", ex.InnerException); }

            if (!listaContratosFsl.Any())
                throw new Exception("No se encontro el Contrato que será Editado");
            if(listaContratosFsl.Count > 1)
                throw new Exception("La Consulta del Contrato devolvio más de una Coincidencia");

            var contratoFsl = listaContratosFsl.FirstOrDefault();
            DatoAInterfazUsuario(contratoFsl);

            var lineasContratoConsultadas = contratoFsl.LineasContrato.Where(x=>x.Activo == true).Cast<LineaContratoFSLBO>().ToList();
            if(lineasContratoConsultadas.Count < 1)
                throw new Exception("No se puedieron Obtener las Unidades del Contrato");
            
            vista.LineasContrato = new List<LineaContratoFSLBO>();
            vista.LineasContrato.AddRange(lineasContratoConsultadas.Select(linea=>new LineaContratoFSLBO(linea)).ToList());
            vista.PresentarLineasContrato(vista.LineasContrato);

            vista.ContratoAnterior = contratoFsl;
            vista.PermitirNumeroSerie(true);
        }
        /// <summary>
        /// Deshabilita los controladores que se encuentran en la vista de Lineas de Contrato
        /// </summary>
        private void DeshabilitarCamposVistaLineas()
        {
            vista.PermitirContrato(false);
            vista.PermitirFechaContrato(false);
            vista.PermitirNombreCliente(false);
            vista.PermitirNumeroSerie(false);
            vista.PermitirAgregarUnidad(false);
        }
        /// <summary>
        /// Deshabilita los campos de la seccion de Configuracion de Cobros de la Unidad
        /// </summary>
        private void DeshabilitarElementosVistaUnidad()
        {
            vista.PermitirVinUnidad(false);
            vista.PermitirNumeroEconomico(false);
            vista.PermitirNombreModelo(false);
            vista.PermitirAnio(false);
            vista.PermitirPBV(false);
            vista.PermitirPBC(false);
            vista.PermitirKmInicial(false);
            vista.PermitirPolizaSeguro(false);
            vista.PermitirMonedaCompra(false);
            vista.PermitirImporteCompra(false);
        }
        /// <summary>
        /// Actualiza las lineas de contrato y los pagos segun sea necesario
        /// </summary>
        public void Actualizar()
        {
            Guid firma = Guid.NewGuid();
            try
            {
                #region Conexión BD

                dataContext.SetCurrentProvider("Outsourcing");
                try
                {
                    dataContext.OpenConnection(firma);
                    dataContext.BeginTransaction(firma);
                }
                catch (Exception ex)
                {
                    if (dataContext.ConnectionState == ConnectionState.Open)
                        dataContext.CloseConnection(firma);
                    throw new Exception("Se encontraron inconsistencias al realizar la actualización.", ex.InnerException);
                }

                #endregion

                var contratoModificado = InterfazUsuarioADato();
                var contratoAntiguo = vista.ContratoAnterior;
                var listaEquiposLiberar = new List<EquipoBO>();
                var seguridadBo = CrearObjetoSeguridad();

                //Actualizar Lineas de Contrato
                controlador.ActualizarUnidadesContrato(dataContext, contratoModificado, contratoAntiguo, listaEquiposLiberar, seguridadBo);
                //Actualizar Pagos de Contrato
                IGeneradorPagosBR generadorPagos = new GeneradorPagosFSLBR();
                generadorPagos.CambiarUnidadesPagos(dataContext, contratoModificado, contratoAntiguo, seguridadBo, true);

                #region Liberar Unidades y/o equipos Aliados

                var unidadBR = new UnidadBR();
                if (vista.UnidadesLiberar != null && vista.UnidadesLiberar.Any())
                {
                    foreach (var unidadBo in vista.UnidadesLiberar)
                    {
                        unidadBo.EstatusActual = EEstatusUnidad.Disponible;
                        var comentario = vista.ObservacionesUnidad.ContainsKey(unidadBo.EquipoID.ToString()) ? vista.ObservacionesUnidad[unidadBo.EquipoID.ToString()] : "SIN COMENTARIO";
                        var lineaAnterior = (vista.ContratoAnterior.LineasContrato.Find(linea => linea.Equipo.EquipoID == unidadBo.EquipoID));
                        if (lineaAnterior == null)
                            throw new Exception("No se encontro la Unidad en la Lista de Unidades Originales del Contrato");
                        var unidadAnterior = unidadBR.ConsultarCompleto(dataContext, new UnidadBOF() { UnidadID = ((UnidadBO)lineaAnterior.Equipo).UnidadID }, true).FirstOrDefault();
                        unidadBR.CambiarUnidadesContratos(dataContext, unidadBo, unidadAnterior, comentario, EMovimiento.LIBERADA_DE_CONTRATO, seguridadBo);
                    }
                }
                if (vista.UnidadesCambioEquipos != null && vista.UnidadesCambioEquipos.Any())
                {
                    foreach (var unidadBo in vista.UnidadesCambioEquipos)
                    {
                        var comentario = vista.ObservacionesUnidad != null && vista.ObservacionesUnidad.ContainsKey(unidadBo.EquipoID.ToString()) ? vista.ObservacionesUnidad[unidadBo.EquipoID.ToString()] : "SIN COMENTARIO";
                        var lineaAnterior = (contratoModificado.LineasContrato.Find(linea => linea.Equipo.EquipoID == unidadBo.EquipoID));
                        if (lineaAnterior == null)
                            throw new Exception("No se encontro la Unidad en la Lista de Unidades Originales del Contrato");
                        var unidadAnterior = unidadBR.ConsultarCompleto(dataContext, new UnidadBOF() {UnidadID = ((UnidadBO) lineaAnterior.Equipo).UnidadID}, true).FirstOrDefault();
                        var noCambiaronUnidades = true;
                        foreach (var equipoAliado in unidadBo.EquiposAliados)
                        {
                            if (!unidadAnterior.EquiposAliados.Any(bo => bo.EquipoAliadoID == equipoAliado.EquipoAliadoID))
                                noCambiaronUnidades = false;
                        }
                        foreach (var equipoAliadoBo in unidadAnterior.EquiposAliados)
                        {
                            if (unidadBo.EquiposAliados.All(bo => bo.EquipoAliadoID != equipoAliadoBo.EquipoAliadoID))
                                noCambiaronUnidades = false;
                        }

                        if(!noCambiaronUnidades)
                            unidadBR.CambiarUnidadesContratos(dataContext, unidadBo, unidadAnterior, comentario, EMovimiento.CAMBIO_DE_EQUIPO_ALIADO, seguridadBo);
                    }
                }

                #endregion

                dataContext.CommitTransaction(firma);

                vista.MostrarMensaje("LOS CAMBIOS FUERON REALIZADOS CON ÉXITO",ETipoMensajeIU.EXITO);
                BloquearInterfaz();
            }
            catch (Exception ex)
            {
                dataContext.RollbackTransaction(firma);
                throw new Exception(NombreClase + ".Actualizar(): " + ex.Message, ex.InnerException);
            }
            finally
            {
                if (dataContext.ConnectionState == ConnectionState.Open)
                    dataContext.CloseConnection(firma);
            }
        }
        /// <summary>
        /// Bloqueda la Interfaz despues de Guardar los cambios de las Lineas
        /// </summary>
        private void BloquearInterfaz()
        {
            vista.PermitirGuardarCambios(false);
            vista.PermitirCancelarCambios(false);
            vista.PermitirNumeroSerie(false);
            vista.PermitirAgregarUnidad(false);
            vista.PermitirVerDetalleLineas(false);
        }
        /// <summary>
        /// Obtiene los datos de la vista para regresar el Contrato de FSL
        /// </summary>
        /// <returns>El contrato de FSL que contiene las información de la vista</returns>
        private ContratoFSLBO InterfazUsuarioADato()
        {
            var contratoFsl = controlador.ConsultarCompleto(dataContext, new ContratoFSLBO() {ContratoID = vista.ContratoAnterior.ContratoID}).FirstOrDefault();
            foreach (var lineaBo in vista.LineasContrato)
            {
                var lineaAnterior = contratoFsl.LineasContrato.Find(x => x.Equipo.EquipoID == lineaBo.Equipo.EquipoID && x.Activo == true);
                if (lineaAnterior != null)
                {
                    contratoFsl.LineasContrato.Remove(lineaAnterior);
                    contratoFsl.LineasContrato.Add(lineaBo);
                }
                else
                    contratoFsl.LineasContrato.Add(lineaBo);
            }
            foreach (LineaContratoFSLBO fslbo in contratoFsl.LineasContrato.Cast<LineaContratoFSLBO>())
            {
                var linea = vista.LineasContrato.Find(x => x.LineaContratoID == fslbo.LineaContratoID);
                if (linea == null)
                    fslbo.Activo = false;
            }

            return contratoFsl;
        }
        /// <summary>
        /// Presenta los Datos de La Seccion de Datos Generales
        /// </summary>
        /// <param name="contratoFsl">Contrato Fsl con la Información a Presentar</param>
        private void DatoAInterfazUsuario(ContratoFSLBO contratoFsl)
        {
            vista.NumeroContrato = contratoFsl.NumeroContrato;
            vista.FechaInicioContrato = contratoFsl.FechaInicioContrato;
            vista.NombreCliente = contratoFsl.Cliente.Nombre;
        }
        /// <summary>
        /// Presenta los Datos de la Linea Seleccionada
        /// </summary>
        /// <param name="linea">Linea con la información a presentar</param>
        private void DatoAInterfazUsuario(LineaContratoFSLBO linea)
        {
            if (linea != null)
            {
                vista.VinUnidad = linea.Equipo.NumeroSerie;
                vista.NumeroEconomicoUnidad = ((UnidadBO)linea.Equipo).NumeroEconomico;
                vista.NombreModelo = linea.Equipo.Modelo != null ? linea.Equipo.Modelo.Nombre : "";
                vista.AnioUnidad = linea.Equipo.Anio;
                vista.PBVMaximoRecomendado = ((UnidadBO)linea.Equipo).CaracteristicasUnidad != null ? ((UnidadBO)linea.Equipo).CaracteristicasUnidad.PBVMaximoRecomendado : null;
                vista.PBCMaximoRecomendado = ((UnidadBO)linea.Equipo).CaracteristicasUnidad != null ? ((UnidadBO)linea.Equipo).CaracteristicasUnidad.PBCMaximoRecomendado : null;
                if (((UnidadBO)linea.Equipo).Mediciones != null && ((UnidadBO)linea.Equipo).Mediciones.Odometros != null)
                {
                    int? primerOdometroID = ((UnidadBO)linea.Equipo).Mediciones.Odometros.Min(o => o.OdometroID);
                    if (primerOdometroID != null)
                    {
                        OdometroBO odometro = ((UnidadBO)linea.Equipo).Mediciones.Odometros.Find(odo => odo.OdometroID == primerOdometroID);
                        if (odometro != null) vista.KmInicial = odometro.KilometrajeInicio;
                    }
                }
                vista.PolizaSeguro = ObtenerPoliza();
                vista.KmEstimadoAnual = linea.KmEstimadoAnual;
                vista.DepositoGarantia = linea.DepositoGarantia;
                vista.ComisionApertura = linea.ComisionApertura;
                vista.CargoFijoMensual = linea.CargoFijoMensual;
                vista.ConOpcionCompra = linea.ConOpcionCompra;
                vista.MonedaCompra = linea.DivisaCompra != null ? linea.DivisaCompra.MonedaDestino ?? new MonedaBO() : null;
                vista.ImporteCompra = linea.ImporteCompra;
                if (linea.Cobrable != null)
                {
                    var cargoAdicional = linea.Cobrable as CargosAdicionalesFSLBO;
                    vista.TipoCotizacion = cargoAdicional.TipoCotizacion;
                    vista.PresentarCargosEquiposAliados(cargoAdicional.CargoAdicionalEquiposAliados);
                }
                if (linea.ProductoServicio != null) {
                    vista.ProductoServicioId = linea.ProductoServicio.Id;
                    vista.ClaveProductoServicio = linea.ProductoServicio.NombreCorto;
                    vista.DescripcionProductoServicio = linea.ProductoServicio.Nombre;
                }
            }
        }
        /// <summary>
        /// Presenta los cargos adicionales de la Unidad o del Equipo aliado
        /// </summary>
        /// <param name="cargosAdicionales">Cargos Adicionales que se presentaran</param>
        private void DatoAInterfazUsuario(CargosAdicionalesFSLBO cargosAdicionales)
        {
            vista.PermitirSeleccionarSinTarifaAdiciona(cargosAdicionales is CargoAdicionalEquipoAliadoBO);
            if (cargosAdicionales.Tarifas.Any())
            {
                if (cargosAdicionales.Tarifas.FirstOrDefault().CobraKm != null && !cargosAdicionales.Tarifas.FirstOrDefault().CobraKm.Value)
                    vista.CargoPorKm = false;
                else
                    vista.CargoPorKm = true;

                var cantidadAnios = new Dictionary<string, string>();
                int? numeroAnios = vista.ContratoAnterior.CalcularPlazoEnAños();
                if (vista.TipoCotizacion == ETipoCotizacion.Step)
                {
                    for(int i = 0; i < numeroAnios; i++)
                    {
                        cantidadAnios.Add(i.ToString(), (i + 1).ToString());
                    }
                }
                else
                {
                    cantidadAnios.Add("0", "1");
                }
                
                vista.PresentarListaAnios(cantidadAnios);
                vista.PresentarRangosTarifa(new List<RangoTarifaFSLBO>());
                vista.PresentarAniosConfigurados(cargosAdicionales.Tarifas);
                if (cargosAdicionales is CargoAdicionalEquipoAliadoBO)
                {
                    vista.EquipoAliadoIdTarifa = (cargosAdicionales as CargoAdicionalEquipoAliadoBO).EquipoAliado.EquipoAliadoID;
                    vista.TipoEquipo = ETipoEquipo.EquipoAliado;
                }
                else
                {
                    vista.UnidadIdTarifa = (vista.LineaContratoEnEdicion.Equipo as UnidadBO).UnidadID;
                    vista.TipoEquipo = ETipoEquipo.Unidad;
                }
                
                vista.TarifasEnConfiguracion = cargosAdicionales.Tarifas.Select(tar => new TarifaFSLBO(tar)).ToList(); 
                vista.RangosEnConfiguracion = new List<RangoTarifaFSLBO>();
            }
            if (cargosAdicionales is CargoAdicionalEquipoAliadoBO)
            {
                var cargoEA = cargosAdicionales as CargoAdicionalEquipoAliadoBO;
                vista.EquipoAliadoIdTarifa = cargoEA.EquipoAliado.EquipoAliadoID;
                vista.TipoEquipo = ETipoEquipo.EquipoAliado;
                if (cargoEA.AplicaCargosAdicionales != null && !cargoEA.AplicaCargosAdicionales.Value)
                {
                    vista.PermitirTarifasAdicionales(false);
                    vista.NoAplicaCargosAdicionales = true;
                }
                vista.TarifasEnConfiguracion = cargoEA.Tarifas.Select(tar => new TarifaFSLBO(tar)).ToList();
            }
            if (vista.TarifasEnConfiguracion.Any() && vista.CargoPorKm != null)
            {
                if (vista.TarifasEnConfiguracion.Any(x => x.CobraKm == null))
                {
                    foreach (var tarifa in vista.TarifasEnConfiguracion)
                        tarifa.CobraKm = vista.CargoPorKm;
                }
            }
        }
        /// <summary>
        /// La información de una Linea la convierte en un objeto LineaContratoFSLBO
        /// </summary>
        /// <param name="linea">Linea que sera Modificada, en caso contratio se creara un a nueva</param>
        /// <returns>Regresa el objeto LineaContratoFSLBO con la informaicon e la interfaz</returns>
        private LineaContratoFSLBO InterfazUsuarioADato(LineaContratoFSLBO linea)
        {
            var lineaEditada = linea ?? new LineaContratoFSLBO() {Equipo = new UnidadBO(), Cobrable = new CargosAdicionalesFSLBO()};
            var lineaAnterior = vista.ContratoAnterior.LineasContrato.Find(x => x.Equipo.EquipoID == lineaEditada.Equipo.EquipoID);

            var cargosAdicionales = vista.LineaContratoEnEdicion.Cobrable as CargosAdicionalesFSLBO;

            if(vista.UnidadesCambioEquipos == null)
                vista.UnidadesCambioEquipos = new List<UnidadBO>();
            if(vista.ObservacionesUnidad == null)
                vista.ObservacionesUnidad = new Dictionary<string, string>();

            var unidad = lineaEditada.Equipo as UnidadBO;
            if(unidad.EquiposAliados == null)
                unidad.EquiposAliados = new List<EquipoAliadoBO>();
            List<EquipoAliadoBO> equiposAliadosAnteriores;
            if(lineaAnterior != null)
            {
                var lineaContratoAnterior = lineaAnterior as LineaContratoFSLBO;
                equiposAliadosAnteriores = (lineaContratoAnterior.Cobrable as CargosAdicionalesFSLBO).CargoAdicionalEquiposAliados != null &&
                                                   (lineaContratoAnterior.Cobrable as CargosAdicionalesFSLBO).CargoAdicionalEquiposAliados.Any() ?
                                                   (lineaContratoAnterior.Cobrable as CargosAdicionalesFSLBO).CargoAdicionalEquiposAliados.Select(x => x.EquipoAliado).ToList() : new List<EquipoAliadoBO>();
            }
            else
                equiposAliadosAnteriores = new List<EquipoAliadoBO>();

            var equiposAliados = cargosAdicionales.CargoAdicionalEquiposAliados.Select(x => x.EquipoAliado).ToList();
            if(!unidad.EquiposAliados.Any() && !equiposAliadosAnteriores.Any())
            { }
            else
            {
                if (lineaAnterior != null)
                {
                    if (equiposAliadosAnteriores.Any() && !equiposAliados.Any())
                    {
                        string observacion = "";
                        if (vista.ObservacionesUnidad.ContainsKey(vista.LineaContratoEnEdicion.Equipo.EquipoID.ToString()))
                            observacion = String.Copy(vista.ObservacionesUnidad[vista.LineaContratoEnEdicion.Equipo.EquipoID.ToString()]);
                        if (!String.IsNullOrEmpty(observacion)) observacion = observacion + ", ";
                        observacion = equiposAliadosAnteriores.Aggregate(observacion, (current, equipo) => current + ("SE LIBERA EL EQUIPO ALIADO " + equipo.NumeroSerie + " DE LA UNIDAD, "));
                        if(vista.UnidadesCambioEquipos.All(x => x.UnidadID != unidad.UnidadID))
                        {
                            if(vista.ObservacionesUnidad.ContainsKey(vista.LineaContratoEnEdicion.Equipo.EquipoID.ToString()))
                            {
                                if(!String.IsNullOrEmpty(observacion) && observacion.Length > vista.ObservacionesUnidad[vista.LineaContratoEnEdicion.Equipo.EquipoID.ToString()].Length)
                                    vista.ObservacionesUnidad[vista.LineaContratoEnEdicion.Equipo.EquipoID.ToString()] = observacion.Substring(0, (observacion.Length - 2));
                            }
                            else
                            {
                                if(observacion.Length > 0)
                                    vista.ObservacionesUnidad[vista.LineaContratoEnEdicion.Equipo.EquipoID.ToString()] = observacion.Substring(0, (observacion.Length - 2));
                            }
                            vista.UnidadesCambioEquipos.Add(unidad);
                        }
                    }
                    else
                    {
                        if (equiposAliados.Any() && !equiposAliadosAnteriores.Any())
                        {
                            string observacion = "";
                            if (vista.ObservacionesUnidad.ContainsKey(vista.LineaContratoEnEdicion.Equipo.EquipoID.ToString()))
                                observacion = String.Copy(vista.ObservacionesUnidad[vista.LineaContratoEnEdicion.Equipo.EquipoID.ToString()]);
                            if (!String.IsNullOrEmpty(observacion)) observacion = observacion + ", ";
                            observacion = equiposAliados.Aggregate(observacion, (current, equipo) => current + ("SE AGREGA EL EQUIPO ALIADO " + equipo.NumeroSerie + " A LA UNIDAD, "));
                            if(vista.UnidadesCambioEquipos.All(x => x.UnidadID != unidad.UnidadID))
                            {
                                if (vista.ObservacionesUnidad.ContainsKey(vista.LineaContratoEnEdicion.Equipo.EquipoID.ToString()))
                                {
                                    if (!String.IsNullOrEmpty(observacion) && observacion.Length > vista.ObservacionesUnidad[vista.LineaContratoEnEdicion.Equipo.EquipoID.ToString()].Length)
                                        vista.ObservacionesUnidad[vista.LineaContratoEnEdicion.Equipo.EquipoID.ToString()] = observacion.Substring(0, (observacion.Length - 2));
                                }
                                else
                                {
                                    if(observacion.Length > 0)
                                        vista.ObservacionesUnidad[vista.LineaContratoEnEdicion.Equipo.EquipoID.ToString()] = observacion.Substring(0, (observacion.Length - 2));
                                }
                                vista.UnidadesCambioEquipos.Add(unidad);
                            }
                        }
                        else
                        {
                            if (equiposAliados.Any() && equiposAliadosAnteriores.Any())
                            {
                                string observacion = "";
                                if(vista.ObservacionesUnidad.ContainsKey(vista.LineaContratoEnEdicion.Equipo.EquipoID.ToString()))
                                    observacion = String.Copy(vista.ObservacionesUnidad[vista.LineaContratoEnEdicion.Equipo.EquipoID.ToString()]);
                                if(!String.IsNullOrEmpty(observacion)) observacion = observacion + ", ";
                                foreach (var equipo in equiposAliados)
                                {
                                    var equipoExiste = equiposAliadosAnteriores.Any(x => x.EquipoID == equipo.EquipoID);
                                    if(!equipoExiste)
                                        observacion += "SE AGREGA EL EQUIPO ALIADO " + equipo.NumeroSerie + " A LA UNIDAD, ";   
                                }
                                foreach (var equipo in equiposAliadosAnteriores)
                                {
                                    var equipoExiste = equiposAliados.Any(x => x.EquipoID == equipo.EquipoID);
                                    if(!equipoExiste)
                                        observacion += "SE LIBERA EL EQUIPO ALIADO " + equipo.NumeroSerie + " DE LA UNIDAD, ";
                                }
                                if (vista.UnidadesCambioEquipos.All(x => x.UnidadID != unidad.UnidadID))
                                {
                                    if (vista.ObservacionesUnidad.ContainsKey(vista.LineaContratoEnEdicion.Equipo.EquipoID.ToString()))
                                    {
                                        if (!String.IsNullOrEmpty(observacion) && observacion.Length > vista.ObservacionesUnidad[vista.LineaContratoEnEdicion.Equipo.EquipoID.ToString()].Length)
                                            vista.ObservacionesUnidad[vista.LineaContratoEnEdicion.Equipo.EquipoID.ToString()] = observacion.Substring(0, (observacion.Length - 2));
                                    }
                                    else
                                    {
                                        if(observacion.Length > 0)
                                            vista.ObservacionesUnidad[vista.LineaContratoEnEdicion.Equipo.EquipoID.ToString()] = observacion.Substring(0, (observacion.Length - 2));
                                    }
                                    vista.UnidadesCambioEquipos.Add(unidad);
                                }
                            }
                        }
                    }
                }

                if (cargosAdicionales.CargoAdicionalEquiposAliados.Any(x => x.CobrableID == null))
                {
                    if (vista.UnidadesCambioEquipos.All(x => x.UnidadID != unidad.UnidadID))
                        vista.UnidadesCambioEquipos.Add(unidad);
                }
            }

            if(unidad.EquiposAliados != null && unidad.EquiposAliados.Count > 0)
                unidad.EquiposAliados.Clear();

            unidad.EquiposAliados.AddRange(equiposAliados);

            lineaEditada.KmEstimadoAnual = vista.KmEstimadoAnual;
            lineaEditada.DepositoGarantia = vista.DepositoGarantia;
            lineaEditada.ComisionApertura = vista.ComisionApertura;
            lineaEditada.CargoFijoMensual = vista.CargoFijoMensual;
            if (lineaEditada.DivisaCompra == null)
                lineaEditada.DivisaCompra = new DivisaBO() {MonedaOrigen = new MonedaBO(), MonedaDestino = new MonedaBO()};

            if (vista.ConOpcionCompra != null && vista.ConOpcionCompra.Value)
            {
                lineaEditada.DivisaCompra.MonedaDestino = vista.MonedaCompra;
                lineaEditada.ImporteCompra = vista.ImporteCompra;
                lineaEditada.ConOpcionCompra = true;
            }
            else
            {
                lineaEditada.DivisaCompra.MonedaDestino = new MonedaBO();
                lineaEditada.ImporteCompra = vista.ImporteCompra;
                lineaEditada.ConOpcionCompra = false;
            }
            if (lineaEditada.ProductoServicio == null) linea.ProductoServicio = new ProductoServicioBO();
            lineaEditada.ProductoServicio.Id = vista.ProductoServicioId;
            lineaEditada.ProductoServicio.NombreCorto = vista.ClaveProductoServicio;
            lineaEditada.ProductoServicio.Nombre = vista.DescripcionProductoServicio;

            (lineaEditada.Cobrable as CargosAdicionalesFSLBO).TipoCotizacion = vista.TipoCotizacion;
           
            return lineaEditada;
        }
        /// <summary>
        /// Crea un objeto de seguridad con los datos actuales de la sesión de usuario en curso
        /// </summary>
        /// <returns>Objeto de tipo seguridad</returns>
        private SeguridadBO CrearObjetoSeguridad()
        {
            var usuario = new UsuarioBO() { Id = this.vista.UsuarioID };
            var adscripcion = new AdscripcionBO()
            {
                UnidadOperativa = new UnidadOperativaBO() { Id = this.vista.UnidadOperativaID }, 
                Sucursal = new SucursalBO()
            };
            var seguridadBo = new SeguridadBO(Guid.Empty, usuario, adscripcion);

            return seguridadBo;
        }
        /// <summary>
        /// Valida el permiso de acceso a la página
        /// </summary>
        public void EstablecerSeguridad()
        {
            try
            {
                //se valida que los datos del usuario y la unidad operativa no sean nulos
                if(this.vista.UsuarioID == null) throw new Exception("El identificador del usuario no debe ser nulo");
                if(this.vista.UnidadOperativaID == null) throw new Exception("El identificador de la Unidad Operativa no debe ser nulo ");

                //Se crea el objeto de seguridad
                UsuarioBO usr = new UsuarioBO { Id = this.vista.UsuarioID };
                AdscripcionBO sdscripcion = new AdscripcionBO { UnidadOperativa = new UnidadOperativaBO { Id = this.vista.UnidadOperativaID } };
                SeguridadBO seguridadBO = new SeguridadBO(Guid.Empty, usr, sdscripcion);

                //Se obtienen las acciones a las cuales el usuario tiene permiso en este proceso
                List<CatalogoBaseBO> lst = FacadeBR.ConsultarAccion(this.dataContext, seguridadBO);

                //Se valida si el usuario tiene permiso para acceder a la interfaz
                if(!this.ExisteAccion(lst, "EDITARLINEASCONTRATOFSLUI"))
                    this.vista.RedirigirSinPermisoAcceso();
                
                //Se valida si el usuario tiene permiso para Actualizar un contrato
                if(!this.ExisteAccion(lst, "ACTUALIZARUNIDADESCONTRATO"))
                    this.vista.RedirigirSinPermisoAcceso();

                //Se valida si el usuario tiene permiso para Actualizar las unidades del contrato
                if(!this.ExisteAccion(lst, "CAMBIARUNIDADESCONTRATOS"))
                    this.vista.RedirigirSinPermisoAcceso();

                //Se valida si el usuario tiene permiso para registrar nuevos pagos
                if(!this.ExisteAccion(lst, "CAMBIARUNIDADESPAGOS"))
                    this.vista.RedirigirSinPermisoAcceso();
            }
            catch(Exception ex)
            {
                throw new Exception(NombreClase + ".EstablecerSeguridad:" + ex.Message);
            }
        }
        /// <summary>
        /// Obtiene la Unidad que se agregará al Contrato, la consulta con sus equipos Aliados y la agrega a la Lista de Unidades del Contrato
        /// </summary>
        public void AgregarUnidadBuscadorAContrato()
        {
            try
            {
                UnidadBR unidadBr = new UnidadBR();
                var listaUnidades = unidadBr.ConsultarCompleto(dataContext, new UnidadBOF() {UnidadID = vista.LineaUnidadId, EquipoID = vista.LineaEquipoId, EstatusActual = EEstatusUnidad.Disponible, Area = EArea.FSL, EsActivo = true});
                if(!listaUnidades.Any()) throw new Exception("No se encontro la Unidad que sera añadida al contrato");
                if(listaUnidades.Count > 1) throw new Exception("La consulta de la Unidad regreso más de una coincidencia");

                var unidad = listaUnidades.FirstOrDefault();
                var lineaContratoFsl = new LineaContratoFSLBO()
                {
                    Activo = true,
                    Cobrable = new CargosAdicionalesFSLBO()
                    {
                        Tarifas = new List<TarifaFSLBO>(),
                        CargoAdicionalEquiposAliados = unidad.EquiposAliados != null && unidad.EquiposAliados.Any() ? unidad.EquiposAliados.Select(bo => new CargoAdicionalEquipoAliadoBO() {EquipoAliado = bo, Tarifas = new List<TarifaFSLBO>()}).ToList() : new List<CargoAdicionalEquipoAliadoBO>()
                    },
                    Equipo = unidad,
                    ProductoServicio = new ProductoServicioBO()
                };

                vista.NumeroSerie = null;
                vista.LineaEquipoId = null;
                vista.LineaUnidadId = null;
                PresentarLineaUnidad(lineaContratoFsl);
                vista.PermitirEditarTarifa(false);
            }
            catch (Exception ex)
            {
                throw new Exception(NombreClase + ".AgregarUnidadBuscadorAContrato: " + ex.Message, ex.InnerException);
            }
        }
        /// <summary>
        /// Agrega el Equipo Aliado a la lista de Cargos de Equipo Aliado
        /// </summary>
        public void AgregarEquipoAliadoBuscadorAContrato()
        {
            try
            {
                var equipoAliadoBr = new EquipoAliadoBR();
                var listaEquiposAliados = equipoAliadoBr.ConsultarCompleto(dataContext, new EquipoAliadoBOF() {EquipoID = vista.LineaEquipoId, NumeroSerie = vista.NumeroSerieEquipoAliado}, true);
                if(!listaEquiposAliados.Any()) throw new Exception("No se encontro el Equipo Aliado que sera añadido al contrato");
                if(listaEquiposAliados.Count > 1) throw new Exception("La consulta del Equipo Aliado regreso más de una coincidencia");

                var equipoAliado = listaEquiposAliados.FirstOrDefault();
                var cargoEquipoAliado = new CargoAdicionalEquipoAliadoBO()
                {
                    EquipoAliado = equipoAliado,
                    AplicaCargosAdicionales = null,
                    Tarifas = new List<TarifaFSLBO>(),
                    TipoCotizacion = vista.TipoCotizacion
                };

                int? año = vista.ContratoAnterior.CalcularPlazoEnAños();
                if (vista.TipoCotizacion == ETipoCotizacion.Average)
                {
                    cargoEquipoAliado.Tarifas.Add(new TarifaFSLBO(){Rangos = new List<RangoTarifaFSLBO>()});
                }
                else
                {
                    for (int i = 0; i < año; i++)
                    {
                        cargoEquipoAliado.Tarifas.Add(new TarifaFSLBO(){Año = i+1, Rangos = new List<RangoTarifaFSLBO>()});
                    }
                }

                if((vista.LineaContratoEnEdicion.Cobrable as CargosAdicionalesFSLBO).CargoAdicionalEquiposAliados == null)
                    (vista.LineaContratoEnEdicion.Cobrable as CargosAdicionalesFSLBO).CargoAdicionalEquiposAliados = new List<CargoAdicionalEquipoAliadoBO>();

                (vista.LineaContratoEnEdicion.Cobrable as CargosAdicionalesFSLBO).CargoAdicionalEquiposAliados.Add(cargoEquipoAliado);
                vista.PresentarCargosEquiposAliados((vista.LineaContratoEnEdicion.Cobrable as CargosAdicionalesFSLBO).CargoAdicionalEquiposAliados);
                (vista.LineaContratoEnEdicion.Equipo as UnidadBO).EquiposAliados.Add(equipoAliado);

                vista.NumeroSerieEquipoAliado = null;
                vista.LineaEquipoId = null;
                vista.PermitirAgregarEquipoAliado(false);
            }
            catch(Exception ex)
            {
                throw new Exception(NombreClase + ".AgregarEquipoAliadoBuscadorAContrato: " + ex.Message, ex.InnerException);
            }
        }
        /// <summary>
        /// Presenta la Linea de Contrato que sera Editada
        /// </summary>
        /// <param name="lineaContrato">Linea de Contrato que sera editada</param>
        public void PresentarLineaUnidad(LineaContratoFSLBO lineaContrato)
        {
            try
            {
                var listaMonedas = FacadeBR.ConsultarMoneda(dataContext, new MonedaBO() {Activo = true});
                if(!listaMonedas.Any()) throw new Exception("No se pudieron obtener las Monedas para la Opcion de Compra.");
                Dictionary<string,string> monedas = new Dictionary<string, string>();
                for (int i = 0; i < listaMonedas.Count; i++)
                {
                    monedas[i.ToString()] = listaMonedas[i].Nombre;
                }
                vista.MonedasDisponibles = listaMonedas;
                vista.PresentarMonedasDisponibles(monedas);
                vista.LimpiarLineaUnidad();
                DeshabilitarElementosVistaUnidad();
                vista.LineaContratoEnEdicion = new LineaContratoFSLBO(lineaContrato);
                DatoAInterfazUsuario(vista.LineaContratoEnEdicion);
                if (vista.ConOpcionCompra == true)
                {
                    vista.PermitirMonedaCompra(true);
                    vista.PermitirImporteCompra(true);
                }
                vista.CambiarInterfaz("Linea");
            }
            catch (Exception ex)
            {
                throw new Exception(NombreClase + ".PresentarLineaUnidad: " + ex.Message, ex.InnerException);
            }
        }
        /// <summary>
        /// Se encarga de realizar la presentacion de la seccion para capturar las tarifas
        /// </summary>
        /// <param name="tipoEquipo">Tipo de Tarifa que sera presentada</param>
        /// <param name="cargoAdicional">Cargo Adicional que se esta editando</param>
        public void PresentarTarifa(ETipoEquipo tipoEquipo, CargosAdicionalesFSLBO cargoAdicional)
        {
            try
            {
                var frecuencias = new Dictionary<string, string>();
                frecuencias.Add(((Int32)EFrecuencia.Anual).ToString(),EFrecuencia.Anual.ToString());
                frecuencias.Add(((Int32)EFrecuencia.Mensual).ToString(), EFrecuencia.Mensual.ToString());
                vista.PresentarFrecuencias(frecuencias);

                vista.PermitirSeleccionarSinTarifaAdiciona(tipoEquipo == ETipoEquipo.EquipoAliado);
                vista.LimpiarInterfazTarifas();
                vista.PermitirTarifasAdicionales(true);
                DatoAInterfazUsuario(cargoAdicional);
                vista.PermitirConfiguracionTarifa(false);
            }
            catch (Exception ex)
            {
                throw new Exception(NombreClase + ".PresentarTarifa: " + ex.Message, ex.InnerException);
            }
        }
        /// <summary>
        /// Activa o desactiva los campos de opcion de compra
        /// </summary>
        /// <param name="opcionCompra">Determina si tiene o no la opcion de compra</param>
        public void CambiarOpcionCompra(bool opcionCompra)
        {
            try
            {
                vista.PermitirMonedaCompra(opcionCompra);
                vista.PermitirImporteCompra(opcionCompra);
                vista.MonedaCompra = null;
                vista.ImporteCompra = null;
            }
            catch (Exception ex)
            {
                throw new Exception(NombreClase + ".CambiarOpcionCompra(): " + ex.Message, ex.InnerException);
            }
        }
        /// <summary>
        /// Cambia el Tipo de Cotizacion de las Tarifas de las Unidades y Equipos Aliados
        /// </summary>
        /// <param name="tipoCotizacion">Tipo de Cotizacion para las Tarifas</param>
        public void CambiarTipoCotizacion(ETipoCotizacion? tipoCotizacion)
        {
            try
            {
                if (tipoCotizacion == null)
                {
                    vista.PermitirEditarTarifa(false);
                    vista.PresentarCargosEquiposAliados(((CargosAdicionalesFSLBO)vista.LineaContratoEnEdicion.Cobrable).CargoAdicionalEquiposAliados);
                }
                else
                {
                    vista.PermitirEditarTarifa(true);
                    var cargoAdicional = vista.LineaContratoEnEdicion.Cobrable as CargosAdicionalesFSLBO;
                    int? años = vista.ContratoAnterior.CalcularPlazoEnAños();
                    cargoAdicional.TipoCotizacion = tipoCotizacion;
                    if (tipoCotizacion == ETipoCotizacion.Step)
                    {
                        cargoAdicional.CobrableID = null;
                        cargoAdicional.Tarifas = new List<TarifaFSLBO>();
                        foreach(var cargoAdicionalEquipoAliadoBo in cargoAdicional.CargoAdicionalEquiposAliados)
                        {
                            cargoAdicionalEquipoAliadoBo.CobrableID = null;
                            cargoAdicionalEquipoAliadoBo.AplicaCargosAdicionales = null;
                            cargoAdicionalEquipoAliadoBo.TipoCotizacion = tipoCotizacion;
                            cargoAdicionalEquipoAliadoBo.Tarifas = new List<TarifaFSLBO>();
                        }
                        for(var i = 0; i < años; i++)
                        {
                            cargoAdicional.Tarifas.Add(new TarifaFSLBO() {Año = i + 1, Rangos = new List<RangoTarifaFSLBO>()});
                            foreach (var cargoAdicionalEquipoAliadoBo in cargoAdicional.CargoAdicionalEquiposAliados)
                            {
                                cargoAdicionalEquipoAliadoBo.Tarifas.Add(new TarifaFSLBO() {Año = i + 1, Rangos = new List<RangoTarifaFSLBO>()});
                            }
                        }
                    }
                    else
                    {
                        cargoAdicional.CobrableID = null;
                        cargoAdicional.Tarifas = new List<TarifaFSLBO> {new TarifaFSLBO() {Rangos = new List<RangoTarifaFSLBO>()}};
                        foreach (var cargoAdicionalEquipoAliadoBo in cargoAdicional.CargoAdicionalEquiposAliados)
                        {
                            cargoAdicionalEquipoAliadoBo.CobrableID = null;
                            cargoAdicionalEquipoAliadoBo.AplicaCargosAdicionales = null;
                            cargoAdicionalEquipoAliadoBo.TipoCotizacion = tipoCotizacion;
                            cargoAdicionalEquipoAliadoBo.Tarifas = new List<TarifaFSLBO>(){new TarifaFSLBO(){Rangos = new List<RangoTarifaFSLBO>()}};
                        }
                    }
                    vista.PresentarCargosEquiposAliados(cargoAdicional.CargoAdicionalEquiposAliados);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(NombreClase + ".CambiarTipoCotizacion(): " + ex.Message, ex.InnerException);
            }
        }
        /// <summary>
        /// Cambia el tipo de cobro de las Tarifas por Kilometros u Horas
        /// </summary>
        /// <param name="cobraKm">Determina si se cobran horas o Kilometros</param>
        public void CambiarTipoCargo(bool? cobraKm)
        {
            try
            {
                foreach(var tarifaFslbo in vista.TarifasEnConfiguracion)
                {
                    tarifaFslbo.CobraKm = cobraKm;
                    tarifaFslbo.Frecuencia = null;
                    tarifaFslbo.KmLibres = null;
                    tarifaFslbo.HrLibres = null;
                    tarifaFslbo.Rangos = new List<RangoTarifaFSLBO>();
                }

                if (cobraKm == null)
                {
                    vista.PermitirTarifasAdicionales(false);
                    vista.PermitirTipoCargo(true);
                }
                else
                {
                    vista.PermitirSeleccionarAnio(true);
                    vista.PermitirConfiguracionTarifa(false);
                }

                vista.FrecuenciaTarifa = null;
                vista.KilometrosHorasLibres = null;
                vista.KmHrMinima = null;
                vista.RangosEnConfiguracion = new List<RangoTarifaFSLBO>();
                vista.PresentarRangosTarifa(new List<RangoTarifaFSLBO>());
                vista.PresentarAniosConfigurados(vista.TarifasEnConfiguracion);
            }
            catch (Exception ex)
            {
                throw new Exception(NombreClase + ".CambiarTipoCargo(): "+ ex.Message, ex.InnerException);
            }
        }
        /// <summary>
        /// Coloca en la interfaz la configuracion del Año seleccionado
        /// </summary>
        /// <param name="anioConfigurar">Año a presentar</param>
        public void PresentarDatosAnioTarifa(int? anioConfigurar)
        {
            try
            {
                if (anioConfigurar == null)
                {
                    vista.FrecuenciaTarifa = null;
                    vista.KilometrosHorasLibres = null;
                    vista.KmHrMinima = null;
                    vista.RangoInicialTarifa = null;
                    vista.RangoFinalTarifa = null;
                    vista.CostoKmHr = null;
                    vista.PresentarRangosTarifa(new List<RangoTarifaFSLBO>());
                    vista.PermitirConfiguracionTarifa(false);
                    return;
                }
                var tarifas = vista.TarifasEnConfiguracion;
                var tarifa = anioConfigurar != 1 ? tarifas[anioConfigurar.Value - 1] : tarifas.FirstOrDefault();
                if (tarifa != null)
                {
                    vista.FrecuenciaTarifa = tarifa.Frecuencia;
                    vista.KilometrosHorasLibres = tarifa.CobraKm != null ? tarifa.CobraKm.Value ? tarifa.KmLibres : tarifa.HrLibres : null;
                    vista.KmHrMinima = tarifa.CantidadMinima;
                    vista.RangosEnConfiguracion = new List<RangoTarifaFSLBO>();
                    if (tarifa.Rangos == null)
                    {
                        tarifa.Rangos = new List<RangoTarifaFSLBO>();
                    }
                    else
                        vista.RangosEnConfiguracion.AddRange(tarifa.Rangos.Select(rangoBo=>new RangoTarifaFSLBO(rangoBo)).ToList());
                    
                    vista.PresentarRangosTarifa(vista.RangosEnConfiguracion);
                    vista.PermitirConfiguracionTarifa(true);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(NombreClase + ".PresentarDatosAnioTarifa: " + ex.Message, ex.InnerException);
            }
        }
        /// <summary>
        /// Agrega el cargo adicional del Equipo Aliado pero sin Ninguna Tarifa
        /// </summary>
        /// <param name="sinTarifa">Determina si tendra o no tarifas adicionales</param>
        public void SinTarifasAdicionales(bool sinTarifa)
        {
            try
            {
                vista.PermitirTarifasAdicionales(!sinTarifa);
                vista.TarifasEnConfiguracion = new List<TarifaFSLBO>();
                vista.RangosEnConfiguracion = new List<RangoTarifaFSLBO>();
                if (sinTarifa)
                {
                    vista.CargoPorKm = null;
                    vista.PresentarAniosConfigurados(vista.TarifasEnConfiguracion);
                }
                else
                {
                    if (vista.TipoCotizacion == ETipoCotizacion.Average)
                    {
                        vista.TarifasEnConfiguracion.Add(new TarifaFSLBO(){Rangos = new List<RangoTarifaFSLBO>()});
                    }
                    else
                    {
                        int? numeroAnios = vista.ContratoAnterior.CalcularPlazoEnAños();
                        vista.TarifasEnConfiguracion = new List<TarifaFSLBO>();
                        for (int i = 0; i < numeroAnios; i++)
                        {
                            vista.TarifasEnConfiguracion.Add(new TarifaFSLBO(){Año = i+1,Rangos = new List<RangoTarifaFSLBO>()});
                        }
                    }
                    vista.PermitirTarifasAdicionales(false);
                    vista.PermitirTipoCargo(true);
                    vista.PresentarAniosConfigurados(vista.TarifasEnConfiguracion);
                }
                vista.PresentarRangosTarifa(new List<RangoTarifaFSLBO>());
            }
            catch(Exception ex)
            {
                throw new Exception(NombreClase + ".SinTarifasAdicionales: " + ex.Message, ex.InnerException);
            }
        }
        /// <summary>
        /// Agrega un Rango a la Tarifa que se esta configurando
        /// </summary>
        public void AgregarRangoTarifa()
        {
            try
            {
                int? año = vista.AnioTarifa;
                if(año == null)
                    throw new Exception("No se puede agregar una tarifa sino ha seleccionado un Año a Configurar");

                var tarifa = vista.TarifasEnConfiguracion[año.Value - 1];
                if(tarifa == null)
                    throw new Exception("No se encontro la Tarifa a la que se estan editando los Rangos");
                if (vista.RangosEnConfiguracion == null)
                    vista.RangosEnConfiguracion = new List<RangoTarifaFSLBO>();

                var nuevoRango = new RangoTarifaFSLBO();
                if (vista.CargoPorKm.Value)
                {
                    nuevoRango.CargoKm = vista.CostoKmHr;
                    nuevoRango.KmRangoInicial = vista.RangoInicialTarifa;
                    nuevoRango.KmRangoFinal = vista.RangoFinalTarifa;
                }
                else
                {
                    nuevoRango.CargoHr = vista.CostoKmHr;
                    nuevoRango.HrRangoInicial = vista.RangoInicialTarifa;
                    nuevoRango.HrRangoFinal = vista.RangoFinalTarifa;
                }

                var s = ValidarAgregarRango(tarifa, vista.RangosEnConfiguracion, nuevoRango);
                if (!String.IsNullOrEmpty(s))
                {
                    vista.MostrarMensaje(s, ETipoMensajeIU.ADVERTENCIA);
                    return;
                }

                vista.RangosEnConfiguracion.Add(nuevoRango);
                vista.PresentarRangosTarifa(vista.RangosEnConfiguracion);

                vista.RangoInicialTarifa = null;
                vista.UltimoRango = false;
                vista.RangoFinalTarifa = null;
                vista.CostoKmHr = null;
            }
            catch (Exception ex)
            {
                throw new Exception(NombreClase + ".AgregarRangoTarifa: " + ex.Message, ex.InnerException);
            }
        }
        /// <summary>
        /// Elimina una Linea de Contrato del Contrato Modificandose
        /// </summary>
        /// <param name="linea">Linea que será eliminada</param>
        public void EliminarLineaContrato(LineaContratoFSLBO linea)
        {
            try
            {
                if(vista.ObservacionesUnidad == null)
                    vista.ObservacionesUnidad = new Dictionary<string, string>();

                string observacion = "";
                if(vista.ObservacionesUnidad.ContainsKey(linea.Equipo.EquipoID.ToString()))
                    observacion = vista.ObservacionesUnidad[linea.Equipo.EquipoID.ToString()];
                if(!String.IsNullOrEmpty(observacion)) observacion = observacion + ", ";

                observacion += "SE LIBERA LA UNIDAD Y SUS EQUIPOS ALIADOS DEL CONTRATO " + vista.ContratoAnterior.NumeroContrato;
                vista.ObservacionesUnidad[linea.Equipo.EquipoID.ToString()] = observacion;

                if(vista.UnidadesLiberar == null)
                    vista.UnidadesLiberar = new List<UnidadBO>();
                
                if(vista.UnidadesLiberar.All(x => x.EquipoID != linea.Equipo.EquipoID))
                    vista.UnidadesLiberar.Add(linea.Equipo as UnidadBO);

                vista.LineasContrato.Remove(linea);
                vista.PresentarLineasContrato(vista.LineasContrato);
            }
            catch (Exception ex)
            {
                throw new Exception(NombreClase + "EliminarLineaContrato: " + ex.Message, ex.InnerException);
            }
        }
        /// <summary>
        /// Elimina un Equipo aliado de la Configuracion de Linea
        /// </summary>
        /// <param name="cargoEliminado">Cargo que contiene al equipo Aliado</param>
        public void EliminarEquipoAliado(CargoAdicionalEquipoAliadoBO cargoEliminado)
        {
            try
            {
                (vista.LineaContratoEnEdicion.Cobrable as CargosAdicionalesFSLBO).CargoAdicionalEquiposAliados.Remove(cargoEliminado);
                var equipoAliado = (vista.LineaContratoEnEdicion.Equipo as UnidadBO).EquiposAliados.Find(x => x.EquipoAliadoID == cargoEliminado.EquipoAliado.EquipoAliadoID);
                (vista.LineaContratoEnEdicion.Equipo as UnidadBO).EquiposAliados.Remove(equipoAliado);

                vista.PresentarCargosEquiposAliados((vista.LineaContratoEnEdicion.Cobrable as CargosAdicionalesFSLBO).CargoAdicionalEquiposAliados);
            }
            catch (Exception ex)
            {
                throw new Exception(NombreClase + ".EliminarEquipoAliado: " + ex.Message, ex.InnerException);
            }
        }
        /// <summary>
        /// Elimina el Rango de la Tarifa
        /// </summary>
        /// <param name="indice">Indice del Rango a Eliminar</param>
        public void EliminarRangoTarifa(int? indice)
        {
            try
            {
                if (vista.RangosEnConfiguracion.Count == 1 && indice == 0)
                    vista.RangosEnConfiguracion.RemoveAt(0);
                else
                {
                    if ((vista.RangosEnConfiguracion.Count - 1) == indice)
                    {
                        var rangosTarifas = vista.CargoPorKm.Value ? vista.RangosEnConfiguracion.OrderBy(x => x.KmRangoInicial).ToList() : vista.RangosEnConfiguracion.OrderBy(x => x.HrRangoInicial).ToList();
                        rangosTarifas.RemoveAt(indice.Value);
                        vista.RangosEnConfiguracion = rangosTarifas;
                    }
                    else
                        vista.MostrarMensaje("Se deben eliminar los rangos del último hasta el primero", ETipoMensajeIU.ADVERTENCIA);
                }
                vista.PresentarRangosTarifa(vista.RangosEnConfiguracion);
            }
            catch (Exception ex)
            {
                throw new Exception(NombreClase + ".EliminarRangoTarifa: " + ex.Message, ex.InnerException);
            }
        }
        /// <summary>
        /// Guarda los cambios realizados al año de una Tarifa
        /// </summary>
        public void GuardarConfiguracionAnioTarifa()
        {
            try
            {
                int? año = vista.AnioTarifa;
                if(año == null)
                    throw new Exception("No se puede Guardar una tarifa sino ha seleccionado un Año a Configurar");

                var tarifa = vista.TarifasEnConfiguracion[año.Value - 1];
                if(tarifa == null)
                    throw new Exception("No se encontro la Tarifa a la que se esta editando");

                string mesage = ValidarGuardarAnioTarifa();
                if (!String.IsNullOrEmpty(mesage))
                {
                    vista.MostrarMensaje(mesage, ETipoMensajeIU.ADVERTENCIA);
                    return;
                }
                

                if(vista.CargoPorKm.Value)
                {
                    tarifa.KmLibres = vista.KilometrosHorasLibres;
                    tarifa.CobraKm = true;
                }
                else
                {
                    tarifa.HrLibres = vista.KilometrosHorasLibres;
                    tarifa.CobraKm = false;
                }
                tarifa.CantidadMinima = vista.KmHrMinima;
                tarifa.Frecuencia = vista.FrecuenciaTarifa;
                tarifa.Rangos = vista.RangosEnConfiguracion;

                vista.PresentarAniosConfigurados(vista.TarifasEnConfiguracion);
            }
            catch(Exception ex)
            {
                throw new Exception(NombreClase + ".GuardarConfiguracionAnioTarifa: " + ex.Message, ex.InnerException);
            }
        }
        /// <summary>
        /// Guarda los cambios realizados a la tarifa
        /// </summary>
        public void GuardarConfiguracionTarifas()
        {
            try
            {
                var tarifas = vista.TarifasEnConfiguracion.Select(tarifa => new TarifaFSLBO(tarifa)).ToList();

                if (vista.TipoEquipo == ETipoEquipo.Unidad)
                {
                    (vista.LineaContratoEnEdicion.Cobrable as CargosAdicionalesFSLBO).Tarifas = tarifas;
                }
                else
                {
                    var cargoAdicionalEA = (vista.LineaContratoEnEdicion.Cobrable as CargosAdicionalesFSLBO).CargoAdicionalEquiposAliados.Where(x => x.EquipoAliado.EquipoAliadoID == vista.EquipoAliadoIdTarifa).ToList();
                    var cargoEA = cargoAdicionalEA.FirstOrDefault();
                    cargoEA.AplicaCargosAdicionales = tarifas.Any();
                    cargoEA.Tarifas = tarifas;
                }

                vista.PresentarCargosEquiposAliados((vista.LineaContratoEnEdicion.Cobrable as CargosAdicionalesFSLBO).CargoAdicionalEquiposAliados);
                vista.TipoEquipo = null;
                vista.UnidadIdTarifa = null;
                vista.EquipoAliadoIdTarifa = null;
                vista.TarifasEnConfiguracion = null;
                vista.RangosEnConfiguracion = null;
            }
            catch (Exception ex)
            {
                throw new Exception(NombreClase + ".GuardarConfiguracionTarifas: " + ex.Message, ex.InnerException);
            }
        }
        /// <summary>
        /// Guarda los cambios realizados en la Linea
        /// </summary>
        public void GuardarCambiosLinea()
        {
            try
            {
                var lineaEditada = InterfazUsuarioADato(vista.LineaContratoEnEdicion);

                var lineaAnterior = vista.LineasContrato.FirstOrDefault(x => x.Equipo.EquipoID == lineaEditada.Equipo.EquipoID);
                if(lineaAnterior != null)
                    vista.LineasContrato.Remove(lineaAnterior);
                
                vista.LineasContrato.Add(lineaEditada);

                vista.PresentarLineasContrato(vista.LineasContrato);
            }
            catch (Exception ex)
            {
                throw new Exception(NombreClase + ".GuardarCambiosLinea: " + ex.Message, ex.InnerException);
            }
        }
        /// <summary>
        /// Valida que se agregue un rango a la lista de rangos de la Tarifa en configuracion
        /// </summary>
        /// <param name="tarifa">Tarifa que se esta configurando</param>
        /// <param name="rangosTarifa">Rangos Actuales</param>
        /// <param name="nuevoRango">Nuevo rango a insertar</param>
        /// <returns>Regresa un comentario en caso de que haya un error</returns>
        private string ValidarAgregarRango(TarifaFSLBO tarifa, List<RangoTarifaFSLBO> rangosTarifa, RangoTarifaFSLBO nuevoRango)
        {
            if(rangosTarifa == null)
                rangosTarifa = new List<RangoTarifaFSLBO>();
            if(vista.RangoInicialTarifa == null)
                return "Es requerido capturar el rango de Inicio";
            if(vista.RangoFinalTarifa == null && !vista.UltimoRango.Value)
                return "Es requerido capturar el rango Final";
            if(vista.CostoKmHr == null)
                return "Es requerido ingresar el valor del Cargo. Sino se cobrará cargo, llenar el campo con 0";

            if(!rangosTarifa.Any())
            {
                if(tarifa.CobraKm.Value)
                {
                    if(tarifa.KmLibres > nuevoRango.KmRangoInicial)
                        return "El Rango Inicial debe ser Mayor a la los Kilometros Libres";
                    if(tarifa.KmLibres == nuevoRango.KmRangoInicial)
                        return "El Rango Inicial debe ser Mayor a la los Kilometros Libres";
                    if((nuevoRango.KmRangoInicial - tarifa.KmLibres) > 1)
                        return "El Rango inicial debe ser los Kilometros Libres MAS Uno";
                    if(nuevoRango.KmRangoFinal == null && !vista.UltimoRango.Value)
                        return "Debe Existir un Rango Final";
                    if(nuevoRango.KmRangoFinal != null && nuevoRango.KmRangoInicial >= nuevoRango.KmRangoFinal)
                        return "El Rango final debe ser Mayor al Rango Inicial";
                }
                else
                {
                    if(tarifa.HrLibres > nuevoRango.HrRangoInicial)
                        return "El Rango Inicial debe ser Mayor a la los Kilometros Libres";
                    if(tarifa.HrLibres == nuevoRango.HrRangoInicial)
                        return "El Rango Inicial debe ser Mayor a la los Kilometros Libres";
                    if((nuevoRango.HrRangoInicial - tarifa.HrLibres) > 1)
                        return "El Rango inicial debe ser las Horas Libres MAS Una";
                    if(nuevoRango.HrRangoFinal == null && !vista.UltimoRango.Value)
                        return "Debe Existir un Rango Final";
                    if(nuevoRango.HrRangoFinal != null && nuevoRango.HrRangoInicial >= nuevoRango.HrRangoFinal)
                        return "El Rango final debe ser Mayor al Rango Inicial";
                }
            }
            else
            {
                var rangos = rangosTarifa.OrderBy(x => x.KmRangoInicial).ToList();
                var ultimoRango = rangos.Last();
                if(tarifa.CobraKm.Value)
                {
                    if(ultimoRango.KmRangoFinal == null)
                        return "El Rango Anterior al que se quiere agregar no tiene un valor de 'Rango Final'";
                    if(ultimoRango.KmRangoFinal >= nuevoRango.KmRangoInicial)
                        return "El Rango Inicial debe ser Mayor al Ultimo Rango Final";
                    if((nuevoRango.KmRangoInicial - ultimoRango.KmRangoFinal) > 1)
                        return "El Rango inicial debe ser el Ultimo Rango Final mas uno";
                    if(nuevoRango.KmRangoFinal == null && !vista.UltimoRango.Value)
                        return "Debe Existir un Rango Final";
                    if(nuevoRango.KmRangoFinal != null && nuevoRango.KmRangoInicial >= nuevoRango.KmRangoFinal)
                        return "El Rango final debe ser Mayor al Rango Inicial";
                }
                else
                {
                    if(ultimoRango.HrRangoFinal == null)
                        return "El Rango Anterior al que se quiere agregar no tiene un valor de 'Rango Final'";
                    if(ultimoRango.HrRangoFinal >= nuevoRango.HrRangoInicial)
                        return "El Rango Inicial debe ser Mayor al Ultimo Rango Final";
                    if((nuevoRango.HrRangoInicial - ultimoRango.HrRangoFinal) > 1)
                        return "El Rango inicial debe ser el Ultimo Rango Final mas uno";
                    if(nuevoRango.HrRangoFinal == null && !vista.UltimoRango.Value)
                        return "Debe Existir un Rango Final";
                    if(nuevoRango.HrRangoFinal != null && nuevoRango.HrRangoInicial >= nuevoRango.HrRangoFinal)
                        return "El Rango final debe ser Mayor al Rango Inicial";
                }
            }
            return "";
        }
        /// <summary>
        /// Valida los campos para guardar el Año de una Tarifa
        /// </summary>
        /// <returns>Regresa un comentario en caso de que haya un error</returns>
        private string ValidarGuardarAnioTarifa()
        {
            if (vista.FrecuenciaTarifa == null)
                return "Debe seleccionarse la Frecuencia de Kms/Hrs Libres";
            if (vista.KilometrosHorasLibres == null)
                return "Se debe especificar la cantidad de Kms/Hrs Libres de la Unidad";
            if (vista.RangosEnConfiguracion == null)
                return "No se han configurado Rangos para la Tarifa";
            if(!vista.RangosEnConfiguracion.Any())
                return "No se han configurado Rangos para la Tarifa";
            if (vista.CargoPorKm == true)
            {
                if (!vista.RangosEnConfiguracion.Any(x => x.KmRangoInicial != null && x.KmRangoFinal == null))
                    return "No se ha configurado un rango que sea de un KM incial EN ADELANTE";
            }
            else
            {
                if(!vista.RangosEnConfiguracion.Any(x => x.HrRangoInicial != null && x.HrRangoFinal == null))
                    return "No se ha configurado un rango que sea de una Hr incial EN ADELANTE";
            }

            return null;
        }
        /// <summary>
        /// Verifica si exista una acción en un listado de acciones proporcionado
        /// </summary>
        /// <param name="acciones">Listado de Acciones</param>
        /// <param name="nombreAccion">Nombre de la Acción a Verificar</param>
        /// <returns></returns>
        private bool ExisteAccion(List<CatalogoBaseBO> acciones, string nombreAccion)
        {
            if(acciones != null && acciones.Count > 0 && acciones.Exists(p => p != null && p.Nombre != null && p.Nombre.Trim().ToUpper() == nombreAccion.Trim().ToUpper()))
                return true;

            return false;
        }
        /// <summary>
        /// Obtiene las sucursales a las que tiene acceso el usuario
        /// </summary>
        /// <returns>Lista de Sucursales Disponibles para Consulta</returns>
        private List<SucursalBO> ObtenerSucursalesUsuario()
        {
            if(vista.ListaSucursalesPermitidas == null)
                vista.ListaSucursalesPermitidas = new List<SucursalBO>();

            if(!vista.ListaSucursalesPermitidas.Any())
                vista.ListaSucursalesPermitidas.AddRange(FacadeBR.ConsultarSucursalesSeguridad(dataContext, CrearObjetoSeguridad()));

            return vista.ListaSucursalesPermitidas;
        }
        /// <summary>
        /// Consulta la Poliza de seguro que esté vigente
        /// </summary>
        /// <returns>Numero de Poliza encontrado</returns>
        private string ObtenerPoliza()
        {
            var seguroBR = new SeguroBR();
            List<SeguroBO> seguros = seguroBR.Consultar(dataContext, new SeguroBO { Activo = true, Tramitable = new UnidadBO { UnidadID = ((UnidadBO)vista.LineaContratoEnEdicion.Equipo).UnidadID } }) ?? new List<SeguroBO>();

            SeguroBO seguro = seguros.FindLast(seg => seg.Tramitable.TramitableID == ((UnidadBO)vista.LineaContratoEnEdicion.Equipo).UnidadID && seg.Tramitable.TipoTramitable == ETipoTramitable.Unidad);

            return seguro != null ? seguro.NumeroPoliza : string.Empty;
        }
        #region Metodos para el Buscador
        /// <summary>
        /// Prepara un BO para la Busqueda en su respectivo catalogo
        /// </summary>
        /// <param name="catalogo">catalogo donde se realizara la busqueda</param>
        /// <returns></returns>
        public object PrepararBOBuscador(string catalogo)
        {
            object obj = null;

            switch(catalogo)
            {
                case "UnidadIdealease":
                    var unidad = new UnidadBOF();

                    if(!string.IsNullOrEmpty(vista.NumeroSerie))
                        unidad.NumeroSerie = vista.NumeroSerie;

                    unidad.EstatusActual = EEstatusUnidad.Disponible;
                    unidad.Area = EArea.FSL;
                    unidad.Sucursales = ObtenerSucursalesUsuario();

                    obj = unidad;
                    break;
                case "EquipoAliado":
                    var equipoAliado = new EquipoAliadoBOF();

                    if (!String.IsNullOrEmpty(vista.NumeroSerieEquipoAliado))
                        equipoAliado.NumeroSerie = vista.NumeroSerieEquipoAliado;

                    equipoAliado.Estatus = EEstatusEquipoAliado.SinAsignar;
                    equipoAliado.Sucursales = ObtenerSucursalesUsuario();

                    obj = equipoAliado;
                    break;
                case "ProductoServicio":
                    ProductoServicioBO producto = new ProductoServicioBO() { Activo = true };

                    if (!string.IsNullOrEmpty(vista.ClaveProductoServicio)) {
                        int auxNum = 0;
                        if (Int32.TryParse(vista.ClaveProductoServicio, out auxNum))
                            producto.NombreCorto = vista.ClaveProductoServicio;
                        else
                            producto.Nombre = vista.ClaveProductoServicio;
                    }

                    obj = producto;
                    break;
            }

            return obj;
        }
        /// <summary>
        /// Despliega el Resultado del Buscador
        /// </summary>
        /// <param name="catalogo">Catalogo en el que se realizo la busqueda</param>
        /// <param name="selecto">Objeto Resultante</param>
        public void DesplegarResultadoBuscador(string catalogo, object selecto)
        {
            switch(catalogo)
            {
                case "UnidadIdealease":
                    var unidad = (UnidadBOF)selecto ?? new UnidadBOF();

                    vista.NumeroSerie = unidad.NumeroSerie ?? string.Empty;

                    vista.LineaUnidadId = unidad.UnidadID;

                    vista.LineaEquipoId = unidad.EquipoID;

                    vista.PermitirAgregarUnidad(vista.LineaUnidadId != null);
                    break;
                case "EquipoAliado":
                    var equipoAliado = (EquipoAliadoBOF)selecto ?? new EquipoAliadoBOF();

                    vista.NumeroSerieEquipoAliado = equipoAliado.NumeroSerie;
                    vista.LineaEquipoId = equipoAliado.EquipoID;

                    vista.PermitirAgregarEquipoAliado(vista.LineaEquipoId != null);

                    break;
                case "ProductoServicio":
                    ProductoServicioBO producto = (ProductoServicioBO)selecto ?? new ProductoServicioBO();
                    vista.ProductoServicioId = producto.Id;
                    vista.ClaveProductoServicio = producto.NombreCorto;
                    vista.DescripcionProductoServicio = producto.Nombre;
                    break;
            }
        }
        #endregion
        #endregion
    }
}