//Satisface al CU016 Imprimir Constancia Entrega Bienes
//Satisface al CU017 Imprimir Manual Operaciones
//Satisface al CU018 Imprimir Contrato Maestro
//Satisface al CU019 Imprimir Anexo A
//Satisface al CU021 IMprimir Anexo C Acta de Nacimiento
//Satisface al CU022 Consultar Contratos Full Service Leasing
//Satisface al CU014 Imprimir Contrato de Renta Diaria
//Satisface al CU012 - Imprimir Check List de Entrega Recepción de Unidad
//Satisface al CU031 - Imprimir Contrato de Mantenimiento
//Satisface al CU099 - Imprimir Anexo A de Mantenimiento y Servicio Dedicado
//Satisface al CU093 - Imprimir Pagaré contrato FSL
//Satisface al CU095 - Imprimir Pagaré Contrato CM
//Satisface al CU096 - Imprimir Pagaré contrato SD
//Satisface al CU094 - Imprimir Pagaré contrato RD
//Satisface al CU006 - Ver Histórico de Pagos
//Satisface al CU019 - Reporte de Flota Activa de RD Registrados
//Satisface el CU022 – Reporte Contratos de Servicio Dedicado Registrados
//Satisface el CU021 - Reporte de Contratos de Mantenimiento Registrados
//Satisface el CU020 – Reporte Contratos FSL Registrados
//Satisface el CU018 – Reporte Detallado de Renta Diaria por Sucursal
//Satisface el CU023 – Reporte Porcentaje Utilización de Renta Diaria
//Satisface el CU016 – Reporte de Renta Diaria General
//Satisface el CU025 – Reporte Porcentaje Utilización de Renta Diaria por Tipo de Unidad
//Satisface el CU026 – Reporte Porcentaje Utilización de RD Refrigerados
//Satisface el CU027 – Reporte Porcentaje Utilización de RD Refrigerados por Tipo
//Satisface el CU028 – Reporte Días de Renta
//Satisface el CU029 – Reporte Días de Renta por Tipo de Unidad
//Satisface el CU024 - Reporte de Dollar Utilization
//Satisface al CU030 - Reporte de Añejamiento de Flota
//Satisface al CU051 - Obtener Orden de Salida
//Satisface al CU066 - Reporte de Sistemas Revisados
//Satisface al CU069 - Reporte de Up Time
//Satisface al CU017 - PLEN.BEP.15.MODMTTO.CU017.Imprimir.Calcomania.Mantenimiento

using System;
using System.Collections.Generic;
using BPMO.Primitivos.Enumeradores;
using BPMO.SDNI.Buscador.VIS;
using BPMO.SDNI.Contratos.FSL.RPT;
using BPMO.SDNI.Contratos.Mantto.CM.RPT;
using BPMO.SDNI.Contratos.Mantto.SD.RPT;
using BPMO.SDNI.Contratos.RD.RPT;
using BPMO.SDNI.Contratos.RPT;
using BPMO.SDNI.Facturacion.AplicacionesFacturacion.RPT;
using BPMO.SDNI.Flota.RPT;
using BPMO.SDNI.Mantenimiento.RPT;
using BPMO.SDNI.Mantenimiento.Reportes.DA;
using BPMO.SDNI.Contratos.PSL.RPT;
using BPMO.SDNI.Contratos.PSL.BO;
using BPMO.SDNI.Contratos.BO;

namespace BPMO.SDNI.Buscador.PRE
{
    public class VisorReportePRE
    {
        #region Atributos

        private const string nombreClase = "VisorReportePRE";
        private readonly IVisorReporteVIS vista;
        private readonly string directorioReportes;

        #endregion Atributos

        #region Constructores

        public VisorReportePRE(IVisorReporteVIS view)
        {
            try
            {
                vista = view;
                directorioReportes = vista.MapRuta(vista.ObtenerDirectorioResportes());
            }
            catch (Exception ex)
            {
                vista.MostrarMensaje("Inconsistencias en los parámetros de configuración", ETipoMensajeIU.ERROR,
                    nombreClase + ".VisorReportePRE:" + ex.Message);
            }
        }

        #endregion Constructores

        #region Métodos

        public object ObtenerReporte()
        {
            if (vista.NombreReporte == null) return null;
            switch (vista.NombreReporte)
            {
                case "CU016":
                    return ReporteConstanciaEntregaBienes();
                case "CU017":
                    return ReporteManualOperaciones();
                case "CU018":
                    return ReporteContratoMaestro();
                case "CU019":
                    return ReporteAnexoA();
                case "CU022":
                    return AnexosContrato();
                case "CU021":
                    return ReporteAnexoC();
                case "CU018A":
                    return PlantillaContratoMaestro();
                case "CU014":
                    return ReporteContratoRentaDiaria();
                case "CU012": //CU012
                    return ReporteCheckList();
                case "CU099":
                    return ReporteAnexoAMantenimiento();
                case "CU029":
                    return ReporteContratoMantenimientoCompleto();
                case "CU031":
                    return ReporteContratoMantenimiento();
                case "CU093":
                    return ReportePagareFSL();
                case "CU094":
                    return ReportePagareRD();
                case "CU095":
                    return ReportePagareCM();
                case "CU096":
                    return ReportePagareSD();
                case "BEP1401.CU006":
                    return ReporteHistoricoPagos();
                // CU016 – Reporte de Renta Diaria General
                case "BEP1401.CU016":
                    return ReporteRentaDiariaGeneral();   
				// CU018 - Reporte Detallado de Renta Diaria por Sucursal
                case "BEP1401.CU018":
                    return ReporteDetalladoRDSucursal();                    
				// CU019 - Reporte de Flota Activa de RD Registrados
                case "BEP1401.CU019":
                    return ReporteFlotaActivaRD();
                // CU019 - Reporte de Flota Activa eRenta
                case "BEP1401.CU019A":
                    return ReporteFlotaActiva();
                    // CU022 – Reporte Contratos de Servicio Dedicado Registrados
                case "BEP1401.CU022":
                    return ReporteContratosSDRegistrados();
                    // CU021 - Reporte de Contratos de Mantenimiento Registrados
                case "BEP1401.CU021":
                    return ReporteContratosCMRegistrados();
                    // CU020 - Reporte de Contratos FSL Registrados
                case "BEP1401.CU020":
                    return ReporteContratosFSLRegistrados();
                    // CU023 - Reporte Porcentaje Utilización de Renta Diaria
                case "BEP1401.CU023":
                    return ReportePorcentajeUtilizacionRD();
                    // CU025 – Reporte Porcentaje Utilización de Renta Diaria por Tipo de Unidad
                case "BEP1401.CU024":
                    return ReporteDollarUtilization();
                    //CU024 - Reporte de Dollar Utilization
                case "BEP1401.CU025":
                    return ReportePorcentajeUtilizacionRDTipoUnidad();
                    // CU026 – Reporte Porcentaje Utilización de RD Refrigerados
                case "BEP1401.CU026":
                    return ReportePorcentajeUtilizacionRDRefrigerados();
                    // CU027 – Reporte Porcentaje Utilización de RD Refrigerados por Tipo
                case "BEP1401.CU027":
                    return ReportePorcentajeUtilizacionRDRefrigeradosTipo();
                    // CU028 – Reporte Días de Renta
                case "BEP1401.CU028":
                    return ReporteDiasRenta();
                    // CU029 – Reporte Días de Renta por Tipo de Unidad
                case "BEP1401.CU029":
                    return ReporteDiasRentaTipoUnidad();
                    //CU030 - Reporte de Añejamiento de Flota
                case "BEP1401.CU030":
                    return ReporteAniejamientoFlota();
                //CU051 - Reporte de Orden Salida
                case "CU051":
                    return ReporteOrdenSalida();
                //CU017-Imprimir calcomanía mantenimiento
                case "PLEN.BEP.15.MODMTTO.CU017":
                    return ReporteCalcomanias();
                //CU017-Imprimir calcomanía mantenimiento Taller externo
                case "PLEN.BEP.15.MODMTTO.CU017.TallerExterno":
                    return ReporteCalcomaniasTallerExterno();
                case "PLEN.BEP.15.MODMTTO.CU020":
                    //CU020 - Imprimir Auditoria Realizada
                    return AuditoriaRealizada();
                case "PLEN.BEP.15.MODMTTO.CU068":
                //CU068 - Reporte Mantenimiento Realizado Contra Programado
                    return MantenimientoRealizadoContraProgramado();
                //CU066 - Reporte de Sistemas Revisados
                case "PLEN.BEP.15.MODMTTO.CU047":
                    //CU068 - Reporte Mantenimientos Realizado
                    return MantenimientoRealizado();
                case "CU066":
                    return ReporteSistemasRevisados();
                    //CU069 - Reporte UpTime
                case "PLEN.BEP.15.MODMTTO.CU069":
                    return ReporteUpTime();
                case "PLEN.BEP.15.MODMTTO.CU060":
                    return RendimientoPorUnidad();
                case "PLEN.BEP.15.MODMTTO.CU071":
                    return ReporteAuditoriasRealizadas();
                case "PLEN.BEP.15.MODMTTO.CU075":
                    return ReporteComparativoMantenimiento();
                case "CheckListEntregaRO":
                    return ReporteCheckListConstruccionRO();
                case "ContratoRO":
                    return ReporteContratoRO();
                case "ContratoROC":
                    return ReporteContratoROC();
                case "PagareRO":
                    return ReportePagareRO();
                case "BajaUnidad":
                    return ReporteBajaUnidad();
                case "DetalladoPSLSucursal":
                    return this.ReporteDetalladoPSLSucursal();
                case "RentaDiariaGeneralPSL":
                    return this.ReporteRentaDiariaGeneralPSL();
                case "ReporteIngresoRentas":
                    return this.ReporteIngresoRentas();
                default:
                    vista.MostrarMensaje("no se ha especificado el reporte a mostrar", ETipoMensajeIU.ERROR,
                        "NombreReporte es nulo");
                    return null;
            }
        }

        #region BEP1202

        /// <summary>
        /// Obtiene el Reporte de los Anexos del Contrato
        /// </summary>
        /// <returns></returns>
        private object AnexosContrato()
        {
            try
            {
                var DatosReporte018 = vista.DatosReporte["CU018"] as Dictionary<string, object>;
                var DatosReporte019 = vista.DatosReporte["CU019"] as Dictionary<string, object>;
                var DatosReporte021 = vista.DatosReporte["CU021"] as Dictionary<string, object>;


                var maestro = new ContratoMaestroRPT(DatosReporte018, directorioReportes);
                maestro.CreateDocument();

// ReSharper disable once PossibleNullReferenceException
                var anverso = new Contratos.FSL.RPT.AnexoARPT((Dictionary<string, Object>)DatosReporte019["anverso"],
                    directorioReportes);
                anverso.CreateDocument();

                var reverso = new Contratos.FSL.RPT.AnexoARevRPT(
                    (Dictionary<string, Object>)DatosReporte019["reverso"], directorioReportes);
                reverso.CreateDocument();

                var anexoC = new AnexoCRPT(DatosReporte021, directorioReportes);
                anexoC.CreateDocument();

                maestro.Pages.AddRange(anverso.Pages);
                maestro.Pages.AddRange(reverso.Pages);
                maestro.Pages.AddRange(anexoC.Pages);
                maestro.PrintingSystem.ContinuousPageNumbering = true;

                return maestro;
            }
            catch (Exception ex)
            {
                vista.MostrarMensaje("Ocurrió un error al generar los Anexos del Contrato Full Service Leasing",
                    ETipoMensajeIU.ERROR, nombreClase + ".VisorReportePRE:" + ex.Message);
                return null;
            }
        }

        protected object ReporteConstanciaEntregaBienes()
        {
            if (vista.DatosReporte != null)
                try
                {
                    return new ConstanciaEntregaBienesRPT(vista.DatosReporte, directorioReportes);
                }
                catch (Exception ex)
                {
                    vista.MostrarMensaje("Ocurrieron inconsistencias al mostrar el mensaje", ETipoMensajeIU.ERROR,
                        ex.Message);
                    return null;
                }

            vista.MostrarMensaje("No se han proporcionado los datos para la Constancia de Bienes", ETipoMensajeIU.ERROR);
            return null;
        }

        protected object ReporteManualOperaciones()
        {
            if (vista.DatosReporte != null)
                try
                {
                    return new ManualOperacionesRPT(vista.DatosReporte, directorioReportes);
                }
                catch (Exception ex)
                {
                    vista.MostrarMensaje("Ocurrieron inconsistencias al mostrar el mensaje", ETipoMensajeIU.ERROR,
                        ex.Message);
                    return null;
                }

            vista.MostrarMensaje("No se han proporcionado los datos para el Manual de Operaciones", ETipoMensajeIU.ERROR);
            return null;
        }

        protected object ReporteAnexoA()
        {
            try
            {
                var anverso = new Contratos.FSL.RPT.AnexoARPT(
                    (Dictionary<string, Object>)vista.DatosReporte["anverso"], directorioReportes);
                anverso.CreateDocument();

                var reverso =
                    new Contratos.FSL.RPT.AnexoARevRPT((Dictionary<string, Object>)vista.DatosReporte["reverso"],
                        directorioReportes);
                reverso.CreateDocument();

                anverso.Pages.AddRange(reverso.Pages);
                anverso.PrintingSystem.ContinuousPageNumbering = true;

                return anverso;
            }
            catch (Exception ex)
            {
                vista.MostrarMensaje("Ocurrió un error al generar el reporte del Anexo A", ETipoMensajeIU.ERROR,
                    nombreClase + ".VisorReportePRE:" + ex.Message);
                return null;
            }
        }

        protected object ReporteAnexoAMantenimiento()
        {
            try
            {
                object obj = null;
                string tipoMantenimiento = this.vista.DatosReporte["TipoMantenimiento"].ToString();
                switch (tipoMantenimiento)
                {
                    case "CM":
                        var anversoCM =
                            new Contratos.Mantto.CM.RPT.AnexoARPT(
                                (Dictionary<string, Object>)vista.DatosReporte["anverso"], directorioReportes);
                        var reversoCM =
                            new Contratos.Mantto.CM.RPT.AnexoARevRPT(
                                (Dictionary<string, Object>)vista.DatosReporte["reverso"], directorioReportes);
                        anversoCM.CreateDocument();
                        reversoCM.CreateDocument();

                        anversoCM.Pages.AddRange(reversoCM.Pages);
                        anversoCM.PrintingSystem.ContinuousPageNumbering = true;

                        obj = anversoCM;
                        break;
                    case "SD":
                        var anversoSD =
                            new Contratos.Mantto.SD.RPT.AnexoARPT(
                                (Dictionary<string, Object>)vista.DatosReporte["anverso"], directorioReportes);
                        var reversoSD =
                            new Contratos.Mantto.SD.RPT.AnexoARevRPT(
                                (Dictionary<string, Object>)vista.DatosReporte["reverso"], directorioReportes);
                        anversoSD.CreateDocument();
                        reversoSD.CreateDocument();

                        anversoSD.Pages.AddRange(reversoSD.Pages);
                        anversoSD.PrintingSystem.ContinuousPageNumbering = true;

                        obj = anversoSD;
                        break;
                    default:
                        throw new Exception("El tipo de mantenimiento del paquete de navegación no es válido.");
                }
                return obj;
            }
            catch (Exception ex)
            {
                vista.MostrarMensaje("Ocurrió un error al generar el reporte del Anexo A", ETipoMensajeIU.ERROR,
                    nombreClase + ".ReporteAnexoAMantenimiento:" + ex.Message);
                return null;
            }
        }

        protected object ReporteContratoMaestro()
        {
            try
            {
                if (vista.DatosReporte != null)
                    return new ContratoMaestroRPT(vista.DatosReporte, directorioReportes);

                vista.MostrarMensaje("No se han proporcionado los datos para el reporte", ETipoMensajeIU.INFORMACION);
                return null;
            }
            catch (Exception ex)
            {
                vista.MostrarMensaje("Ocurrió un error al generar el reporte Maestro", ETipoMensajeIU.ERROR,
                    nombreClase + ".VisorReportePRE:" + ex.Message);
                return null;
            }
        }

        protected object ReporteAnexoC()
        {
            try
            {
                if (vista.DatosReporte != null)
                    return new AnexoCRPT(vista.DatosReporte, directorioReportes);

                vista.MostrarMensaje("No se han proporcionado los datos para el reporte", ETipoMensajeIU.INFORMACION);
                return null;
            }
            catch (Exception ex)
            {
                vista.MostrarMensaje("Ocurrieron inconsistencias al desplegar el reporte", ETipoMensajeIU.ERROR,
                    ex.Message);
                return null;
            }
        }

        protected object PlantillaContratoMaestro()
        {
            try
            {
                if (vista.DatosReporte != null)
                    return new ContratoMaestroRPT(vista.DatosReporte, directorioReportes);

                vista.MostrarMensaje("No se han proporcionado los datos para el reporte", ETipoMensajeIU.INFORMACION);
                return null;
            }
            catch (Exception ex)
            {
                vista.MostrarMensaje("Ocurrieron inconsistencias al desplegar el reporte", ETipoMensajeIU.ERROR,
                    ex.Message);
                return null;
            }
        }

        protected object ReporteContratoRentaDiaria()
        {
            try
            {
                if (vista.DatosReporte != null)
                {
                    ContratoRDRPT anverso = new ContratoRDRPT(vista.DatosReporte, directorioReportes);
                    anverso.CreateDocument();
                    ContratoRDRevRPT reverso = new ContratoRDRevRPT(vista.DatosReporte, directorioReportes);
                    reverso.CreateDocument();

                    anverso.Pages.AddRange(reverso.Pages);
                    anverso.PrintingSystem.ContinuousPageNumbering = true;
                    return anverso;
                }

                vista.MostrarMensaje("No se han proporcionado los datos para el reporte", ETipoMensajeIU.INFORMACION);

                return null;
            }
            catch (Exception ex)
            {
                vista.MostrarMensaje("Inconsistencias al desplegar el reporte", ETipoMensajeIU.ERROR, ex.Message);
                return null;
            }
        }

        /// <summary>
        /// Obtiene el Reporte del check list
        /// </summary>
        /// <returns>Reporte de Check List</returns>
        protected object ReporteCheckList()
        {
            try
            {
                if (vista.DatosReporte != null)
                    return new ListadosVerificacionRDRPT(vista.DatosReporte, directorioReportes);

                vista.MostrarMensaje("No se han proporcionado los datos para el reporte", ETipoMensajeIU.INFORMACION);
                return null;
            }
            catch (Exception ex)
            {
                vista.MostrarMensaje("Ocurrieron inconsistencias al desplegar el reporte", ETipoMensajeIU.ERROR,
                    ex.Message);
                return null;
            }
        }

        /// <summary>
        /// Crea un reporte del contrato de mantenimiento
        /// </summary>
        /// <returns></returns>
        protected object ReporteContratoMantenimiento()
        {
            try
            {
                if (vista.DatosReporte != null)
                {
                    if (vista.DatosReporte.ContainsKey("TipoContrato"))
                        switch (vista.DatosReporte["TipoContrato"].ToString())
                        {
                            case "CM":
                                return new ContratoCMRPT(vista.DatosReporte, directorioReportes);
                            case "SD":
                                return new ContratoSDRPT(vista.DatosReporte, directorioReportes);
                        }
                }

                vista.MostrarMensaje("No se han proporcionado los datos para el reporte", ETipoMensajeIU.INFORMACION);
                return null;
            }
            catch (Exception ex)
            {
                vista.MostrarMensaje("Ocurrieron inconsistencias al desplegar el reporte", ETipoMensajeIU.ERROR,
                    ex.Message);
                return null;
            }
        }

        protected object ReporteContratoMantenimientoCompleto()
        {
            try
            {
                object obj = null;
                //Se obtienen los paquetes de datos de cada reporte
                Dictionary<string, object> datosContrato = vista.DatosReporte["CU031"] as Dictionary<string, object>;
                Dictionary<string, object> datosAnexoA = vista.DatosReporte["CU099"] as Dictionary<string, object>;
                Dictionary<string, object> datosAnexoC = null;
                Dictionary<string, object> datosManualOperaciones = null;
                Dictionary<string, object> DatosPagare = null;

                if (this.vista.DatosReporte.ContainsKey("CU017"))
                    datosManualOperaciones = vista.DatosReporte["CU017"] as Dictionary<string, object>;
                if (this.vista.DatosReporte.ContainsKey("CU021"))
                    datosAnexoC = vista.DatosReporte["CU021"] as Dictionary<string, object>;
                if (vista.DatosReporte.ContainsKey("CU095"))
                    DatosPagare = vista.DatosReporte["CU095"] as Dictionary<string, object>;
                if (vista.DatosReporte.ContainsKey("CU096"))
                    DatosPagare = vista.DatosReporte["CU096"] as Dictionary<string, object>;
                //Según el tipo de contrato de mantenimiento se crean los reportes
                string tipoMantenimiento = this.vista.DatosReporte["TipoMantenimiento"].ToString();
                switch (tipoMantenimiento)
                {
                    case "CM":
                        var reporteContratoCM = new ContratoCMRPT(datosContrato, directorioReportes);
                        reporteContratoCM.CreateDocument();

                        //Anexo A
                        var anversoCM =
                            new Contratos.Mantto.CM.RPT.AnexoARPT((Dictionary<string, Object>)datosAnexoA["anverso"],
                                directorioReportes);
                        var reversoCM =
                            new Contratos.Mantto.CM.RPT.AnexoARevRPT(
                                (Dictionary<string, Object>)datosAnexoA["reverso"], directorioReportes);
                        anversoCM.CreateDocument();
                        reversoCM.CreateDocument();
                        reporteContratoCM.Pages.AddRange(anversoCM.Pages);
                        reporteContratoCM.Pages.AddRange(reversoCM.Pages);

                        if (datosAnexoC != null)
                        {
                            var anexoC = new AnexoCRPT(datosAnexoC, directorioReportes);
                            anexoC.CreateDocument();
                            reporteContratoCM.Pages.AddRange(anexoC.Pages);
                        }
                        if (datosManualOperaciones != null)
                        {
                            var manualOperaciones = new ManualOperacionesRPT(datosManualOperaciones, directorioReportes);
                            manualOperaciones.CreateDocument();
                            reporteContratoCM.Pages.AddRange(manualOperaciones.Pages);
                        }
                        if (DatosPagare != null)
                        {
                            var Pagare = new Contratos.Mantto.CM.RPT.PagareRPT(DatosPagare, directorioReportes);
                            Pagare.CreateDocument();
                            reporteContratoCM.Pages.AddRange(Pagare.Pages);
                        }
                        reporteContratoCM.PrintingSystem.ContinuousPageNumbering = true;
                        obj = reporteContratoCM;
                        break;
                    case "SD":
                        var reporteContratoSD = new ContratoSDRPT(datosContrato, directorioReportes);
                        reporteContratoSD.CreateDocument();

                        //Anexo A
                        var anversoSD =
                            new Contratos.Mantto.SD.RPT.AnexoARPT((Dictionary<string, Object>)datosAnexoA["anverso"],
                                directorioReportes);
                        var reversoSD =
                            new Contratos.Mantto.SD.RPT.AnexoARevRPT(
                                (Dictionary<string, Object>)datosAnexoA["reverso"], directorioReportes);
                        anversoSD.CreateDocument();
                        reversoSD.CreateDocument();
                        reporteContratoSD.Pages.AddRange(anversoSD.Pages);
                        reporteContratoSD.Pages.AddRange(reversoSD.Pages);

                        if (datosAnexoC != null)
                        {
                            var anexoC = new AnexoCRPT(datosAnexoC, directorioReportes);
                            anexoC.CreateDocument();
                            reporteContratoSD.Pages.AddRange(anexoC.Pages);
                        }
                        if (datosManualOperaciones != null)
                        {
                            var manualOperaciones = new ManualOperacionesRPT(datosManualOperaciones, directorioReportes);
                            manualOperaciones.CreateDocument();
                            reporteContratoSD.Pages.AddRange(manualOperaciones.Pages);
                        }
                        if (DatosPagare != null)
                        {
                            var Pagare = new Contratos.Mantto.SD.RPT.PagareRPT(DatosPagare, directorioReportes);
                            Pagare.CreateDocument();
                            reporteContratoSD.Pages.AddRange(Pagare.Pages);
                        }
                        reporteContratoSD.PrintingSystem.ContinuousPageNumbering = true;
                        obj = reporteContratoSD;
                        break;
                }
                return obj;
            }
            catch (Exception ex)
            {
                vista.MostrarMensaje("Ocurrió un error al generar el contrato junto con sus documentos",
                    ETipoMensajeIU.ERROR, nombreClase + ".ReporteContratoMantenimientoCompleto:" + ex.Message);
                return null;
            }
        }

        protected object ReportePagareCM()
        {
            try
            {
                if (vista.DatosReporte != null)
                {
                    return new Contratos.Mantto.CM.RPT.PagareRPT(vista.DatosReporte, directorioReportes);
                }

                vista.MostrarMensaje("No se han proporcionado los datos para el reporte", ETipoMensajeIU.INFORMACION);
                return null;
            }
            catch (Exception ex)
            {
                vista.MostrarMensaje("Ocurrió un error al generar el pagaré.", ETipoMensajeIU.ERROR,
                    nombreClase + ".ReportePagareCM:" + ex.Message);
                return null;
            }
        }

        protected object ReportePagareSD()
        {
            try
            {
                if (vista.DatosReporte != null)
                {
                    return new Contratos.Mantto.SD.RPT.PagareRPT(vista.DatosReporte, directorioReportes);
                }

                vista.MostrarMensaje("No se han proporcionado los datos para el reporte", ETipoMensajeIU.INFORMACION);
                return null;
            }
            catch (Exception ex)
            {
                vista.MostrarMensaje("Ocurrió un error al generar el pagaré.", ETipoMensajeIU.ERROR,
                    nombreClase + ".ReportePagareSD:" + ex.Message);
                return null;
            }
        }

        protected object ReportePagareFSL()
        {
            try
            {
                if (vista.DatosReporte != null)
                {
                    return new Contratos.FSL.RPT.PagareRPT(vista.DatosReporte, directorioReportes);
                }

                vista.MostrarMensaje("No se han proporcionado los datos para el reporte", ETipoMensajeIU.INFORMACION);
                return null;
            }
            catch (Exception ex)
            {
                vista.MostrarMensaje("Ocurrió un error al generar el pagaré.", ETipoMensajeIU.ERROR,
                    nombreClase + ".ReportePagareFSL:" + ex.Message);
                return null;
            }
        }

        protected object ReportePagareRD()
        {
            try
            {
                if (vista.DatosReporte != null)
                {
                    return new Contratos.RD.RPT.PagareRPT(vista.DatosReporte, directorioReportes);
                }

                vista.MostrarMensaje("No se han proporcionado los datos para el reporte", ETipoMensajeIU.INFORMACION);
                return null;
            }
            catch (Exception ex)
            {
                vista.MostrarMensaje("Ocurrió un error al generar el pagaré.", ETipoMensajeIU.ERROR,
                    nombreClase + ".ReportePagareRD:" + ex.Message);
                return null;
            }
        } 
        #endregion

        #region BEP1401

        #region CU006 - Ver Historico de Pagos

        /// <summary>
        /// Generador de Reporte de Historico de Pagos
        /// </summary>
        /// <returns>Objeto de reporte a visualizar</returns>
        protected object ReporteHistoricoPagos()
        {
            try
            {
                if (vista.DatosReporte != null)
                {
                    return new HistoricoPagosRPT(vista.DatosReporte);
                }

                vista.MostrarMensaje("No se han proporcionado los datos para el reporte", ETipoMensajeIU.INFORMACION);
                return null;
            }
            catch (Exception ex)
            {
                vista.MostrarMensaje("Ocurrió un error al generar la impresion del Histórico de Pagos.",
                    ETipoMensajeIU.ERROR, nombreClase + ".ReporteHistoricoPagos:" + ex.Message);
                return null;
            }
        }

        #endregion

        #region CU016 – Reporte de Renta Diaria General
        /// <summary>
        /// Generador de Reporte Renta Diaria General
        /// </summary>
        /// <returns>Objeto de reporte a visualizar</returns>
        protected object ReporteRentaDiariaGeneral()
        {
            try
            {
                if(vista.DatosReporte != null)
                {
                    this.vista.EstablecerFormatosAExportar("pdf", "xls", "xlsx");
                    return new BPMO.SDNI.Flota.RPT.RentaDiariaGeneralRPT(vista.DatosReporte);
                }

                vista.MostrarMensaje("No se han proporcionado los datos para el reporte", ETipoMensajeIU.INFORMACION);
                return null;
            }
            catch(Exception ex)
            {
                vista.MostrarMensaje("Ocurrió un error al generar el Reporte.", ETipoMensajeIU.ERROR,
                    nombreClase + ".ReporteRentaDiariaGeneral:" + ex.Message);
                return null;
            }
        }
        #endregion

		#region CU018 – Reporte Detallado de Renta Diaria por Sucursal
        /// <summary>
        /// Generador de Reporte de Flota Activa de RD Registrados
        /// </summary>
        /// <returns>Objeto de reporte a visualizar</returns>
        protected object ReporteDetalladoRDSucursal()
        {
            try
            {
                if (vista.DatosReporte != null)
                {
                    this.vista.EstablecerFormatosAExportar("pdf", "xls", "xlsx");
                    return new BPMO.SDNI.Contratos.RD.RPT.DetalladoRDSucursalRPT(vista.DatosReporte);
                }

                vista.MostrarMensaje("No se han proporcionado los datos para el reporte", ETipoMensajeIU.INFORMACION);
                return null;
            }
            catch (Exception ex)
            {
                vista.MostrarMensaje("Ocurrió un error al generar el Reporte.", ETipoMensajeIU.ERROR,
                    nombreClase + ".ReporteDetalladoRDSucursal:" + ex.Message);
                return null;
            }
        }
        #endregion
		
        #region CU019 - Reporte de Flota Activa de RD Registrados

        /// <summary>
        /// Generador de Reporte de Flota Activa de RD Registrados
        /// </summary>
        /// <returns>Objeto de reporte a visualizar</returns>
        protected object ReporteFlotaActivaRD()
        {
            try
            {
                if (vista.DatosReporte != null)
                {
                    this.vista.EstablecerFormatosAExportar("pdf", "xls", "xlsx");
                    return new FlotaActivaRDRPT(vista.DatosReporte);
                }

                vista.MostrarMensaje("No se han proporcionado los datos para el reporte", ETipoMensajeIU.INFORMACION);
                return null;
            }
            catch (Exception ex)
            {
                vista.MostrarMensaje("Ocurrió un error al generar el Reporte.", ETipoMensajeIU.ERROR,
                    nombreClase + ".ReporteFlotaActivaRD:" + ex.Message);
                return null;
            }
        }

        #endregion
        #region CU019A - Reporte de Flota Activa eRenta

        /// <summary>
        /// Generador de Reporte de Flota Activa
        /// </summary>
        /// <returns>Objeto de reporte a visualizar</returns>
        protected object ReporteFlotaActiva() {
            try {
                if (vista.DatosReporte != null) {
                    this.vista.EstablecerFormatosAExportar("pdf", "xls", "xlsx");
                    return new FlotaActivaRPT(vista.DatosReporte);
                }

                vista.MostrarMensaje("No se han proporcionado los datos para el reporte", ETipoMensajeIU.INFORMACION);
                return null;
            } catch (Exception ex) {
                vista.MostrarMensaje("Ocurrió un error al generar el Reporte.", ETipoMensajeIU.ERROR,
                    nombreClase + ".ReporteFlotaActiva:" + ex.Message);
                return null;
            }
        }

        #endregion

        #region CU022 - Reporte de Contratos de Servicio Dedicado Registrados

        /// <summary>
        /// Generador de Reporte de Contratos de Servicios Dedicados registrados
        /// </summary>
        /// <returns>Objeto de reporte a visualizar</returns>
        protected object ReporteContratosSDRegistrados()
        {
            try
            {
                if (vista.DatosReporte != null)
                {
                    this.vista.EstablecerFormatosAExportar("pdf", "xls", "xlsx");
                    return new BPMO.SDNI.Contratos.Mantto.SD.RPT.ContratoSDRegistradoRPT(vista.DatosReporte);
                }

                vista.MostrarMensaje("No se han proporcionado los datos para el reporte", ETipoMensajeIU.INFORMACION);
                return null;
            }
            catch (Exception ex)
            {
                vista.MostrarMensaje("Ocurrió un error al generar el Reporte.", ETipoMensajeIU.ERROR,
                    nombreClase + ".ReporteContratosSDRegistrados:" + ex.Message);
                return null;
            }
        }

        #endregion

        #region CU021 - Reporte de Contratos de CM Registrados

        /// <summary>
        /// Generador de Reporte de Contratos de CM Registrados
        /// </summary>
        /// <returns>Objeto de reporte a visualizar</returns>
        protected object ReporteContratosCMRegistrados()
        {
            try
            {
                if (vista.DatosReporte != null)
                {
                    this.vista.EstablecerFormatosAExportar("pdf", "xls", "xlsx");
                    return new BPMO.SDNI.Contratos.Mantto.CM.RPT.ContratoCMRegistradosRPT(vista.DatosReporte);
                }

                vista.MostrarMensaje("No se han proporcionado los datos para el reporte", ETipoMensajeIU.INFORMACION);
                return null;
            }
            catch (Exception ex)
            {
                vista.MostrarMensaje("Ocurrió un error al generar el Reporte.", ETipoMensajeIU.ERROR,
                    nombreClase + ".ReporteContratosCMRegistrados:" + ex.Message);
                return null;
            }
        }

        #endregion

        #region CU020 - Reporte de Contratos FSL Registrados

        /// <summary>
        /// Generador de Reporte de Contratos FSL registrados
        /// </summary>
        /// <returns>Objeto de reporte a visualizar</returns>
        protected object ReporteContratosFSLRegistrados()
        {
            try
            {
                if (vista.DatosReporte != null)
                {
                    this.vista.EstablecerFormatosAExportar("pdf", "xls", "xlsx");
                    return new ContratoFSLRegistradoRPT(vista.DatosReporte);
                }

                vista.MostrarMensaje("No se han proporcionado los datos para el reporte", ETipoMensajeIU.INFORMACION);
                return null;
            }
            catch (Exception ex)
            {
                vista.MostrarMensaje("Ocurrió un error al generar el Reporte.", ETipoMensajeIU.ERROR,
                    nombreClase + ".ReporteContratosFSLRegistrados:" + ex.Message);
                return null;
            }
        }

        #endregion

        #region CU023 - Reporte de Porcentaje de Utilización de Renta Diaría
        /// <summary>
        /// Generador de Reporte de Porcentaje de Utilización de Renta Diaría
        /// </summary>
        /// <returns>Objeto de reporte a visualizar</returns>
        protected object ReportePorcentajeUtilizacionRD()
        {
            try
            {
                if (vista.DatosReporte != null)
                {
                    this.vista.EstablecerFormatosAExportar("pdf", "xls", "xlsx");
                    return new PorcentajeUtilizacionRDRPT(vista.DatosReporte);
                }

                vista.MostrarMensaje("No se han proporcionado los datos para el reporte", ETipoMensajeIU.INFORMACION);
                return null;
            }
            catch (Exception ex)
            {
                vista.MostrarMensaje("Ocurrió un error al generar el Reporte.", ETipoMensajeIU.ERROR,
                    nombreClase + ".ReportePorcentajeUtilizacionRD:" + ex.Message);
                return null;
            }
        }

        #endregion

        #region CU024 - Reporte de Dollar Utilization

        /// <summary>
        /// Generador de Reporte de Dollar Utilization
        /// </summary>
        /// <returns>Objeto de reporte a visualizar</returns>
        protected object ReporteDollarUtilization()
        {
            try
            {
                if (vista.DatosReporte != null)
                {
                    this.vista.EstablecerFormatosAExportar("pdf");
                    return new DollarUtilizationRPT(vista.DatosReporte);
                }

                vista.MostrarMensaje("No se han proporcionado los datos para el reporte", ETipoMensajeIU.INFORMACION);
                return null;
            }
            catch (Exception ex)
            {
                vista.MostrarMensaje("Ocurrió un error al generar el Reporte,", ETipoMensajeIU.ERROR,
                    nombreClase + ".ReporteDollarUtilization:" + ex.Message);
                return null;
            }
        }

        #endregion
        
        #region CU025 – Reporte Porcentaje Utilización de Renta Diaria por Tipo de Unidad

        /// <summary>
        /// Generador de Reporte de Porcentaje de Utilización de Renta Diaría por Tipo Unidad
        /// </summary>
        /// <returns>Objeto de reporte a visualizar</returns>
        protected object ReportePorcentajeUtilizacionRDTipoUnidad()
        {
            try
            {
                if (vista.DatosReporte != null)
                {
                    this.vista.EstablecerFormatosAExportar("pdf", "xls", "xlsx");
                    return new PorcentajeUtilizacionRDTipoUnidadRPT(vista.DatosReporte);
                }

                vista.MostrarMensaje("No se han proporcionado los datos para el reporte", ETipoMensajeIU.INFORMACION);
                return null;
            }
            catch (Exception ex)
            {
                vista.MostrarMensaje("Ocurrió un error al generar el Reporte.", ETipoMensajeIU.ERROR,
                    nombreClase + ".ReportePorcentajeUtilizacionRDTipoUnidad:" + ex.Message);
                return null;
            }
        }

        #endregion

        #region CU026 – Reporte Porcentaje Utilización de RD Refrigerados

        /// <summary>
        /// Generador de Reporte de Porcentaje de Utilización de Renta Diaría de refirgerados
        /// </summary>
        /// <returns>Objeto de reporte a visualizar</Rreturns>
        protected object ReportePorcentajeUtilizacionRDRefrigerados()
        {
            try
            {
                if (vista.DatosReporte != null)
                {
                    this.vista.EstablecerFormatosAExportar("pdf", "xls", "xlsx");
                    return new PorcentajeUtilizacionRDRefrigeradosRPT(vista.DatosReporte);
                }

                vista.MostrarMensaje("No se han proporcionado los datos para el reporte", ETipoMensajeIU.INFORMACION);
                return null;
            }
            catch (Exception ex)
            {
                vista.MostrarMensaje("Ocurrió un error al generar el Reporte.", ETipoMensajeIU.ERROR,
                    nombreClase + ".ReportePorcentajeUtilizacionRDRefrigerados:" + ex.Message);
                return null;
            }
        }

        #endregion

        #region CU027 – Reporte Porcentaje Utilización de RD Refrigerados por Tipo

        /// <summary>
        /// Generador de Reporte de Porcentaje de Utilización de Renta Diaría de Refrigerados por Tipo Unidad
        /// </summary>
        /// <returns>Objeto de reporte a visualizar</returns>
        protected object ReportePorcentajeUtilizacionRDRefrigeradosTipo()
        {
            try
            {
                if (vista.DatosReporte != null)
                {
                    this.vista.EstablecerFormatosAExportar("pdf", "xls", "xlsx");
                    return new PorcentajeUtilizacionRDRefrigeradosTipoRPT(vista.DatosReporte);
                }

                vista.MostrarMensaje("No se han proporcionado los datos para el reporte", ETipoMensajeIU.INFORMACION);
                return null;
            }
            catch (Exception ex)
            {
                vista.MostrarMensaje("Ocurrió un error al generar el Reporte.", ETipoMensajeIU.ERROR,
                    nombreClase + ".ReportePorcentajeUtilizacionRDRefrigeradosTipo:" + ex.Message);
                return null;
            }
        }

        #endregion

        #region CU028 – Reporte Días de Renta

        /// <summary>
        /// Generador de Reporte de Dias de Renta
        /// </summary>
        /// <returns>Objeto de reporte a visualizar</returns>
        protected object ReporteDiasRenta()
        {
            try
            {
                if (vista.DatosReporte != null)
                {
                    this.vista.EstablecerFormatosAExportar("pdf", "xls", "xlsx");
                    return new DiasRentaRPT(vista.DatosReporte);
                }

                vista.MostrarMensaje("No se han proporcionado los datos para el reporte", ETipoMensajeIU.INFORMACION);
                return null;
            }
            catch (Exception ex)
            {
                vista.MostrarMensaje("Ocurrió un error al generar el Reporte.", ETipoMensajeIU.ERROR,
                    nombreClase + ".ReporteDiasRenta:" + ex.Message);
                return null;
            }
        }

        #endregion

        #region CU029 – Reporte Días de Renta por Tipo de Unidad

        /// <summary>
        /// Generador de Reporte de Dias de Renta por tipo de unidad
        /// </summary>
        /// <returns>Objeto de reporte a visualizar</returns>
        protected object ReporteDiasRentaTipoUnidad()
        {
            try
            {
                if (vista.DatosReporte != null)
                {
                    this.vista.EstablecerFormatosAExportar("pdf", "xls", "xlsx");
                    return new DiasRentaTipoUnidadRPT(vista.DatosReporte);
                }

                vista.MostrarMensaje("No se han proporcionado los datos para el reporte", ETipoMensajeIU.INFORMACION);
                return null;
            }
            catch (Exception ex)
            {
                vista.MostrarMensaje("Ocurrió un error al generar el Reporte.", ETipoMensajeIU.ERROR,
                    nombreClase + ".ReporteDiasRentaTipoUnidad:" + ex.Message);
                return null;
            }
        }

        #endregion

        #region CU030 - Reporte de Añejamiento de Flota
        /// <summary>
        /// Generador de Reporte de Añejamiento de Flota
        /// </summary>
        /// <returns>Objeto de reporte a visualizar</returns>
        protected object ReporteAniejamientoFlota()
        {
            try
            {
                if(vista.DatosReporte != null)
                {
                    this.vista.EstablecerFormatosAExportar("pdf", "xls", "xlsx");
                    return new AniejamientoFlotaRPT(vista.DatosReporte);
                }

                vista.MostrarMensaje("No se han proporcionado los datos para el reporte", ETipoMensajeIU.INFORMACION);
                return null;
            }
            catch(Exception ex)
            {
                vista.MostrarMensaje("Ocurrió un error al generar el Reporte.", ETipoMensajeIU.ERROR, nombreClase + ".ReporteAnijamientoFlota:" + ex.Message);
                return null;
            }
        }

        #endregion

        #region PLEN.BEP.15.MODMTTO.CU069 - Imprimir calcomanía de mantto realizado
        /// <summary>
        /// Generador de Reporte de calcomanías
        /// </summary>
        /// <returns>Objeto de reporte a visualizar</returns>
        protected object ReporteCalcomanias()
        {
            try
            {
                if (vista.DatosReporte != null)
                {
                    this.vista.EstablecerFormatosAExportar("pdf", "xls", "xlsx");
                    return new ReporteCalcomaniasRPT(vista.DatosReporte);
                }

                vista.MostrarMensaje("No se han proporcionado los datos para el reporte", ETipoMensajeIU.INFORMACION);
                return null;
            }
            catch (Exception ex)
            {
                vista.MostrarMensaje("Ocurrió un error al generar reporte", ETipoMensajeIU.ERROR, nombreClase + ".ReporteCalcomanias" + ex.Message);
                return null;
            }
        }

        /// <summary>
        /// Reporte de calcomanías que se genera cuando la unidad es ingresado a un taller externo
        /// </summary>
        /// <returns></returns>
        protected object ReporteCalcomaniasTallerExterno()
        {
            try
            {
                if (vista.DatosReporte != null)
                {
                    this.vista.EstablecerFormatosAExportar("pdf", "xls", "xlsx");
                    return new ReporteCalcomaniasTallerExternoRPT(vista.DatosReporte);
                }

                vista.MostrarMensaje("No se han proporcionado los datos para el reporte", ETipoMensajeIU.INFORMACION);
                return null;

            }
            catch (Exception ex)
            {
                vista.MostrarMensaje("Ocurrió un error al generar reporte", ETipoMensajeIU.ERROR, nombreClase + ".ReporteCalcomanias" + ex.Message);
                return null;
            }
        }

        #endregion

        #region PLEN.BEP.15.MODMTTO.CU020 - Imprimir Auditoria Realizada
        protected object AuditoriaRealizada()
        {
            try
            {
                if (this.vista.DatosReporte != null)
                {
                    this.vista.EstablecerFormatosAExportar("pdf");
                    return new AuditoriaRealizadaRPT(vista.DatosReporte);
                }

                vista.MostrarMensaje("No se han proporcionado los datos para el reporte", ETipoMensajeIU.INFORMACION);
                return null;
            }
            catch (Exception ex)
            {
                vista.MostrarMensaje("Ocurrió un error al generar el Reporte.", ETipoMensajeIU.ERROR, nombreClase + ".AuditoriaRealizada:" + ex.Message);
                return null;
            }
        }
        #endregion

        #region PLEN.BEP.15.MODMTTO.CU068 - Reporte Mantenimiento Realizado VS Programado
        protected object MantenimientoRealizadoContraProgramado()
        {
            try
            {
                if (this.vista.DatosReporte != null)
                {
                    this.vista.EstablecerFormatosAExportar("pdf", "xls", "xlsx");
                    return new ReporteMantenimientoRealizadoContraProgramadoRPT(vista.DatosReporte);
                }

                vista.MostrarMensaje("No se han proporcionado los datos para el reporte", ETipoMensajeIU.INFORMACION);
                return null;
            }
            catch (Exception ex)
            {
                vista.MostrarMensaje("Ocurrió un error al generar el Reporte.", ETipoMensajeIU.ERROR, nombreClase + ".AuditoriaRealizada:" + ex.Message);
                return null;
            }
        }
        #endregion

        #region PLEN.BEP.15.MODMTTO

        #region PLEN.BEP.15.MODMTTO.CU060 - Reporte de Rendimiento por Unidad
        /// <summary>
        /// Reporte de Rendimiento por Unidad
        /// </summary>
        /// <returns>Reporte de rendimiento por unidad</returns>
        protected object RendimientoPorUnidad()
        {
            try
            {
                if (this.vista.DatosReporte != null)
                {
                    this.vista.EstablecerFormatosAExportar("pdf", "xls", "xlsx");
                    return new ReporteRendimientoUnidadAreasRPT(vista.DatosReporte);
                }

                vista.MostrarMensaje("No se han proporcionado los datos para el reporte", ETipoMensajeIU.INFORMACION);
                return null;
            }
            catch (Exception ex)
            {
                vista.MostrarMensaje("Ocurrió un error al generar el Reporte.", ETipoMensajeIU.ERROR, nombreClase + ".RendimientoPorUnidad:" + ex.Message);
                return null;
            }
        } 
        #endregion

        #region PLEN.BEP.15.MODMTTO.CU071 - Reporte de Auditorias Realizadas
        /// <summary>
        /// Generador de Reporte de Orden de Salida
        /// </summary>
        /// <returns>Objeto de reporte a visualizar</returns>
        protected object ReporteAuditoriasRealizadas()
        {
            try
            {
                if (vista.DatosReporte != null)
                {
                    this.vista.EstablecerFormatosAExportar("pdf", "xls", "xlsx");
                    return new ReporteAuditoriasRealizadasRPT(vista.DatosReporte);
                }

                vista.MostrarMensaje("No se han proporcionado los datos para el reporte", ETipoMensajeIU.INFORMACION);
                return null;
            }
            catch (Exception ex)
            {
                vista.MostrarMensaje("Ocurrió un error al generar el Reporte.", ETipoMensajeIU.ERROR, nombreClase + ".ReporteAuditoriasRealizadas:" + ex.Message);
                return null;
            }
        }
        #endregion
        #endregion

        #region CU051 - Obtener Orden de Salida
        /// <summary>
        /// Generador de Reporte de Orden de Salida
        /// </summary>
        /// <returns>Objeto de reporte a visualizar</returns>
        protected object ReporteOrdenSalida()
        {
            try
            {
                if (vista.DatosReporte != null)
                {
                    this.vista.EstablecerFormatosAExportar("pdf");
                    return new ReporteOrdenSalidaRPT(vista.DatosReporte);
                }

                vista.MostrarMensaje("No se han proporcionado los datos para el reporte", ETipoMensajeIU.INFORMACION);
                return null;
            }
            catch (Exception ex)
            {
                vista.MostrarMensaje("Ocurrió un error al generar el Reporte.", ETipoMensajeIU.ERROR, nombreClase + ".ReporteOrdenSalida:" + ex.Message);
                return null;
            }
        }
        #endregion

        #region CU066 - Sistemas Revisados
        /// <summary>
        /// Generador de Reporte de Sistemas Revisados
        /// </summary>
        /// <returns>Objeto de reporte a visualizar</returns>
        protected object ReporteSistemasRevisados()
        {
            try
            {
                if (vista.DatosReporte != null)
                {
                    this.vista.EstablecerFormatosAExportar("pdf", "xls", "xlsx");
                    return new ReporteSistemasRevisadosRPT(vista.DatosReporte);
                }

                vista.MostrarMensaje("No se han proporcionado los datos para el reporte", ETipoMensajeIU.INFORMACION);
                return null;
            }
            catch (Exception ex)
            {
                vista.MostrarMensaje("Ocurrió un error al generar el Reporte.", ETipoMensajeIU.ERROR, nombreClase + ".ReporteOrdenSalida:" + ex.Message);
                return null;
            }
        }

        #endregion

        #region CU075 - Reporte Comparativo Mantenimiento
        /// <summary>
        /// Generador de Reporte Comparativo de Mantenimientos de Servicio
        /// </summary>
        /// <returns>Objeto de reporte a visualizar</returns>
        protected object ReporteComparativoMantenimiento() {
            try {
                if (vista.DatosReporte != null) {
                    vista.EstablecerFormatosAExportar("pdf", "xls", "xlsx");
                    return new ReporteComparativoMantenimientoRPT(vista.DatosReporte);
                }
                vista.MostrarMensaje("No se han proporcionado los datos para el reporte", ETipoMensajeIU.INFORMACION);
                return null;
            }catch(Exception ex){
                vista.MostrarMensaje("Ocurrió un error al generar el Reporte.", ETipoMensajeIU.ERROR, nombreClase + ".ReporteOrdenSalida: " + ex.Message);
                return null;
            }

        }
            #endregion

        #endregion

        #region Reporte de UpTime

        protected object ReporteUpTime()
        {
            try
            {
                if (vista.DatosReporte != null)
                {
                    this.vista.EstablecerFormatosAExportar("pdf", "xls", "xlsx");
                    return new UpTimeRPT(vista.DatosReporte);
                }

                vista.MostrarMensaje("No se han proporcionado los datos para el reporte", ETipoMensajeIU.INFORMACION);
                return null;
            }
            catch (Exception ex)
            {
                vista.MostrarMensaje("Ocurrió un error al generar reporte", ETipoMensajeIU.ERROR, nombreClase + ".ReporteUpTime" + ex.Message);
                return null;
            }
        }

        #endregion

        #region Reporte Mantenimientos Realizados
        
        private object MantenimientoRealizado()
        {
            try
            {
                if (vista.DatosReporte != null)
                {
                    this.vista.EstablecerFormatosAExportar("pdf", "xls", "xlsx");
                    return new ReporteMantenimientosRealizadosRPT(vista.DatosReporte);
                }

                vista.MostrarMensaje("No se han proporcionado los datos para el reporte", ETipoMensajeIU.INFORMACION);
                return null;
            }
            catch (Exception ex)
            {
                vista.MostrarMensaje("Ocurrió un error al generar reporte", ETipoMensajeIU.ERROR, nombreClase + ".ReporteUpTime" + ex.Message);
                return null;
            }
        }
        
        #endregion

        #region Reporte de Check List Construcción RO

        protected object ReporteCheckListConstruccionRO()
        {
            try
            {
                string CheckList=string.Empty;
                if (vista.DatosReporte != null)
                {
                    if (this.vista.DatosReporte.ContainsKey("TipoListadoVerificacion"))
                        CheckList = this.vista.DatosReporte["TipoListadoVerificacion"].ToString();
                    switch (CheckList)
                    {
                        case "LV_EXCAVADORA":
                            return new ListadoVerificacionExcavadoraRPT(vista.DatosReporte);
                        
                        case "LV_RETRO_EXCAVADORA":
                            return new ListadoVerificacionRetroExcavadoraRPT(vista.DatosReporte) ;

                        case "LV_ENTREGA_RECEPCION":
                            return new ListadoVerificacionEntregaRecepcionRPT(vista.DatosReporte);
                            
                        case "LV_PISTOLA_NEUMATICA":
                            return new ListadoVerificacionPistolaNeumaticaRPT(vista.DatosReporte);

                        case "LV_MARTILLO_HIDRAULICO":
                            return new ListadoVerificacionMartilloHidraulicoRPT(vista.DatosReporte);

                        case "LV_SUBARRENDADO":
                            return new ListadoVerificacionSubArrendadosRPT(vista.DatosReporte);

                        case "LV_MOTONIVELADORA":
                            return new ListadoVerificacionMotoNiveladoraRPT(vista.DatosReporte);
                        case "LV_TORRES_LUZ":
                            return new ListadoVerificacionTorresLuzRPT(vista.DatosReporte);
                        case "LV_VIBRO_COMPACTADOR":
                            return new ListadoVerificacionVibroCompactadorRPT(vista.DatosReporte);


                        case "LV_COMPRESOR":
                            return new ListadoVerificacionCompresoresPortatilesRPT(vista.DatosReporte);

                        case "LV_MONTACARGA":
                            return new ListadoVerificacionMontaCargasRPT(vista.DatosReporte);

                        case "LV_MINICARGADOR":
                            return new ListadoVerificacionMiniCargadorRPT2(vista.DatosReporte);

                        case "LV_PLATAFORMA_TIJERAS":
                            return new ListadoVerificacionPlataformaTijerasRPT(vista.DatosReporte);

                        default:
                            break;
                    }
                    
                }

                vista.MostrarMensaje("No se han proporcionado los datos para el reporte", ETipoMensajeIU.INFORMACION);
                return null;
            }
            catch (Exception ex)
            {
                vista.MostrarMensaje("Ocurrieron inconsistencias al desplegar el reporte", ETipoMensajeIU.ERROR,
                    ex.Message);
                return null;
            }
        }

        #endregion

        /// <summary>
        /// Imprime el reporte de contrato
        /// </summary>
        /// <returns></returns>
        protected object ReportePagareRO() {
            try {
                return new ContratoPagareRPT(vista.DatosReporte);
            }
            catch (Exception ex) {
                vista.MostrarMensaje("Ocurrieron inconsistencias al desplegar el reporte", ETipoMensajeIU.ERROR,
                    ex.Message);
                return null;
            }
        }

        /// <summary>
        /// Imprime el reporte de contrato
        /// </summary>
        /// <returns></returns>
        protected object ReporteContratoRO()
        {
            try
            {
                var maestro = new ContratoRORPT(vista.DatosReporte, directorioReportes);
                int numeroAnexo = 1;
                maestro.CreateDocument();

                var contrato = (ContratoPSLBO)vista.DatosReporte["ContratoPSLBO"];
                if (!vista.DatosReporte.ContainsKey("LineaContrato"))
                    vista.DatosReporte.Add("LineaContrato", 0);
                foreach (ILineaContrato linea in contrato.LineasContrato)
                {
                    if (linea.Activo == true) {
                        vista.DatosReporte["LineaContrato"] = linea;
                        vista.DatosReporte["NumeroAnexo"] = numeroAnexo;
                        var anexo = new ContratoAnexoROCRPT(vista.DatosReporte);
                        anexo.CreateDocument();
                        maestro.Pages.AddRange(anexo.Pages);
                        numeroAnexo++;
                    }
                }
                
                var anexoContrato = new ContratoAnexoROCRPT2(vista.DatosReporte);
                anexoContrato.CreateDocument();

                maestro.Pages.AddRange(anexoContrato.Pages);
                maestro.PrintingSystem.ContinuousPageNumbering = true;

                return maestro;
            }
            catch (Exception ex)
            {
                vista.MostrarMensaje("Ocurrieron inconsistencias al desplegar el reporte", ETipoMensajeIU.ERROR,
                    ex.Message);
                return null;
            }            
        }

        /// <summary>
        /// Imprime el reporte de contrato
        /// </summary>
        /// <returns></returns>
        protected object ReporteContratoROC()
        {
            try
            {
                return new ContratoROCRPT(vista.DatosReporte);
            }
            catch (Exception ex)
            {
                vista.MostrarMensaje("Ocurrieron inconsistencias al desplegar el reporte", ETipoMensajeIU.ERROR,
                    ex.Message);
                return null;
            }
        }
        /// <summary>
        /// Imprime el reporte de ingreso rentas
        /// </summary>
        /// <returns></returns>
        protected object ReporteIngresoRentas() {
            try {
                this.vista.EstablecerFormatosAExportar("xls", "xlsx","pdf");
                return new IngresoRentasRPT(vista.DatosReporte);
            } catch (Exception ex) {
                vista.MostrarMensaje("Ocurrieron inconsistencias al desplegar el reporte", ETipoMensajeIU.ERROR,
                    ex.Message);
                return null;
            }
        }
        /// <summary>
        /// Imprime el reporte de contrato
        /// </summary>
        /// <returns></returns>
        protected object ReporteBajaUnidad()
        {
            try
            {
                return new BajaActivoFijoRPT(vista.DatosReporte);
            }
            catch (Exception ex)
            {
                vista.MostrarMensaje("Ocurrieron inconsistencias al desplegar el reporte", ETipoMensajeIU.ERROR,
                    ex.Message);
                return null;
            }
        }

        #region Reporte Detallado PSL Sucursal
        /// <summary>
        /// Reporte detallado PSL por sucursal
        /// </summary>
        /// <returns></returns>
        protected object ReporteDetalladoPSLSucursal() {
            try {
                if (vista.DatosReporte != null) {
                    this.vista.EstablecerFormatosAExportar("pdf", "xls", "xlsx");
                    return new BPMO.SDNI.Contratos.PSL.RPT.DetalladoPSLSucursalRPT(vista.DatosReporte);
                }

                vista.MostrarMensaje("No se han proporcionado los datos para el reporte", ETipoMensajeIU.INFORMACION);
                return null;
            } catch (Exception ex) {
                vista.MostrarMensaje("Ocurrió un error al generar el Reporte.", ETipoMensajeIU.ERROR,
                    nombreClase + ".ReporteDetalladoPSLSucursal:" + ex.Message);
                return null;
            }
        }
        #endregion /Reporte Detallado PSL Sucursal

        #region Reporte General de Renta Diaria RO/RE/ROC
        protected object ReporteRentaDiariaGeneralPSL() {
            try {
                if (vista.DatosReporte != null) {
                    this.vista.EstablecerFormatosAExportar("pdf", "xls", "xlsx");
                    return new BPMO.SDNI.Contratos.PSL.RPT.RentaDiariaGeneralPSLRPT(vista.DatosReporte);
                }

                vista.MostrarMensaje("No se han proporcionado los datos para el reporte", ETipoMensajeIU.INFORMACION);
                return null;
            } catch (Exception ex) {
                vista.MostrarMensaje("Ocurrió un error al generar el Reporte.", ETipoMensajeIU.ERROR,
                    nombreClase + ".ReporteRentaDiariaGeneralPSL:" + ex.Message);
                return null;
            }
        }
        #endregion


        #endregion
    }
}
