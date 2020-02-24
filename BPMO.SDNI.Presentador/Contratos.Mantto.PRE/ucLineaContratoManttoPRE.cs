//Satisface al CU027 - Registrar Contrato de Mantenimiento
using System;
using System.Collections.Generic;
using System.Linq;
using BPMO.Facade.SDNI.BR;

using BPMO.Basicos.BO;
using BPMO.Servicio.Catalogos.BO;
using BPMO.Patterns.Creational.DataContext;
using BPMO.Primitivos.Enumeradores;

using BPMO.SDNI.Equipos.BO;
using BPMO.SDNI.Equipos.BR;
using BPMO.SDNI.Tramites.BO;
using BPMO.SDNI.Tramites.BR;
using BPMO.SDNI.Contratos.Mantto.BO;
using BPMO.SDNI.Contratos.Mantto.VIS;

namespace BPMO.SDNI.Contratos.Mantto.PRE
{
    public class ucLineaContratoManttoPRE
    {
        #region Atributos
        private IDataContext dctx = null;
        private IucLineaContratoManttoVIS vista;

        private string nombreClase = "ucLineaContratoManttoPRE";
        #endregion

        #region Constructores
        public ucLineaContratoManttoPRE(IucLineaContratoManttoVIS view)
        {
            try
            {
                this.vista = view;

                this.dctx = FacadeBR.ObtenerConexion();
            }
            catch (Exception ex)
            {
                this.vista.MostrarMensaje("Inconsistencia en los parámetros de configuración", ETipoMensajeIU.ERROR, this.nombreClase + ".ucLineaContratoManttoPRE:" + ex.Message);
            }
        }
        #endregion

        #region Métodos
        /// <summary>
        /// Prepara la vista para su modalidad de registro
        /// </summary>
        public void PrepararNuevo(int? unidadID)
        {
            this.vista.ModoEdicion = true;
            this.vista.PrepararEdicion();

            LineaContratoManttoBO linea = new LineaContratoManttoBO() { Equipo = new Equipos.BO.UnidadBO() { UnidadID = unidadID } };
            this.EstablecerLineaContrato(linea);
        }
        /// <summary>
        /// Prepara la vista para su modalidad de edición
        /// </summary>
        public void PrepararEdicion(LineaContratoManttoBO linea) {
            this.vista.ModoEdicion = true;
            this.vista.PrepararEdicion();
            this.EstablecerLineaContrato(linea, true);
        }
        /// <summary>
        /// Prepara la vista para su modalidad de visualización
        /// </summary>
        public void PrepararVisualizacion(LineaContratoManttoBO linea) {
            this.vista.ModoEdicion = false;
            this.vista.PrepararVisualizacion();
            this.EstablecerLineaContrato(linea);
        }

        private void EstablecerLineaContrato(LineaContratoManttoBO linea, bool edicion = false)
        {
            try
            {
                if (linea == null) linea = new LineaContratoManttoBO();
                if (linea.Equipo == null) linea.Equipo = new Equipos.BO.UnidadBO();
                if (!(linea.Equipo is Equipos.BO.UnidadBO))
                    throw new Exception("El equipo de la línea de contrato no es una unidad.");

                List<TramiteBO> lstTramites = new List<TramiteBO>();

                #region Se obtiene la información completa de la unidad y sus trámites
                if (((Equipos.BO.UnidadBO)linea.Equipo).UnidadID != null)
                {
                    Equipos.BO.UnidadBO bo = (Equipos.BO.UnidadBO)linea.Equipo;
                    //SC_0051: cambie de consultar completo a consultar detalle
                    List<Equipos.BO.UnidadBO> lst = new UnidadBR().ConsultarDetalle(this.dctx, new Equipos.BO.UnidadBO() { UnidadID = bo.UnidadID, EquipoID = bo.EquipoID }, true);
                    if (lst.Count <= 0)
                        throw new Exception("No se encontró la información completa de la unidad seleccionada.");
                    if (!edicion)
                        bo = lst[0];

                    if (bo.Sucursal != null)//SC_0051
                    {
                        if (bo.Sucursal.Id.HasValue)
                        {
                            var lstUnis = FacadeBR.ConsultarSucursal(dctx, bo.Sucursal);
                            var sucUni = lstUnis.FirstOrDefault(x => x.Id == bo.Sucursal.Id);

                            if (sucUni != null)
                                bo.Sucursal = sucUni;
                        }
                    }

                    lstTramites = new TramiteBR().ConsultarCompleto(this.dctx, new TramiteProxyBO() { Activo = true, Tramitable = bo }, false);

                    linea.Equipo = bo;
                }
                #endregion

                #region Se completa la información de la sub-línea de contrato con los equipos aliados de la unidad
                if (linea.SubLineasContrato != null)
                {
                    foreach (SubLineaContratoManttoBO sublinea in linea.SubLineasContrato)
                    {
                        if (sublinea.EquipoAliado == null) sublinea.EquipoAliado = new EquipoAliadoBO();

                        if (sublinea.EquipoAliado.EquipoAliadoID != null && ((Equipos.BO.UnidadBO)linea.Equipo).EquiposAliados != null)
                        {
                            EquipoAliadoBO eaTemp = ((Equipos.BO.UnidadBO)linea.Equipo).EquiposAliados.Find(p => p.EquipoAliadoID == sublinea.EquipoAliado.EquipoAliadoID);
                            if (eaTemp != null)
                                sublinea.EquipoAliado = eaTemp;
                        }
                    }
                }
                else
                {
                    if (linea.Equipo != null && linea.Equipo is Equipos.BO.UnidadBO && ((Equipos.BO.UnidadBO)linea.Equipo).EquiposAliados != null)
                    {
                        linea.SubLineasContrato = new List<SubLineaContratoManttoBO>();
                        foreach (EquipoAliadoBO ea in ((Equipos.BO.UnidadBO)linea.Equipo).EquiposAliados)
                        {
                            SubLineaContratoManttoBO subLinea = new SubLineaContratoManttoBO();
                            subLinea.EquipoAliado = ea;
                            subLinea.Mantenimiento = false;
                            linea.SubLineasContrato.Add(subLinea);
                        }
                    }
                }
                #endregion

                this.DatoAInterfazUsuario(lstTramites);
                this.DatoAInterfazUsuario(linea);
            }
            catch (Exception ex)
            {
                throw new Exception(this.nombreClase + ".EstablecerLineaContrato: " + ex.Message);
            }
        }
        public LineaContratoManttoBO ObtenerLineaContrato()
        {
            LineaContratoManttoBO linea = new LineaContratoManttoBO();
            TarifaManttoBO cobrable = new TarifaManttoBO();
            Equipos.BO.UnidadBO equipo = new Equipos.BO.UnidadBO();
            equipo.Modelo = new ModeloBO();
            equipo.Modelo.Marca = new MarcaBO();
            equipo.CaracteristicasUnidad = new CaracteristicasUnidadBO();
            equipo.Sucursal = new SucursalBO();
            linea.ProductoServicio = new ProductoServicioBO();

            linea.LineaContratoID = this.vista.LineaContratoID;
            linea.KmEstimadoAnual = this.vista.KmEstimadoAnual;
            linea.SubLineasContrato = this.vista.SubLineasContrato;
            linea.ProductoServicio.Id = this.vista.ProductoServicioId;
            linea.ProductoServicio.NombreCorto = this.vista.ClaveProductoServicio;
            linea.ProductoServicio.Nombre = this.vista.DescripcionProductoServicio;

            equipo.UnidadID = this.vista.UnidadID;
            equipo.EquipoID = this.vista.EquipoID;
            equipo.Anio = this.vista.Anio;
            equipo.CaracteristicasUnidad.PBCMaximoRecomendado = this.vista.CapacidadCarga;
            equipo.CaracteristicasUnidad.CapacidadTanque = this.vista.CapacidadTanque;
            equipo.Modelo.Marca.Nombre = this.vista.MarcaNombre;
            equipo.Modelo.Nombre = this.vista.ModeloNombre;
            equipo.NumeroEconomico = this.vista.NumeroEconomico;
            equipo.CaracteristicasUnidad.RendimientoTanque = this.vista.RendimientoTanque;
            equipo.NumeroSerie = this.vista.VIN;
            equipo.Sucursal.Nombre = equipo.Sucursal.Nombre;

            cobrable.CargoFijoMensual = this.vista.CargoFijoMensual;
            cobrable.KilometrosLibres = this.vista.KilometrosLibres;
            cobrable.CargoKmRecorrido = this.vista.CostoKmRecorrido;
            cobrable.HorasLibres = this.vista.HorasLibres;
            cobrable.CargoHorasRefrigeradas = this.vista.CostoHorasRefrigeradas;
            cobrable.TarifaID = this.vista.CobrableID;
            cobrable.PeriodoTarifaHR = this.vista.PeriodoTarifaHRS.HasValue && this.vista.PeriodoTarifaHRS >= 0 ? (EPeriodoTarifa?)this.vista.PeriodoTarifaHRS : null;
            cobrable.PeriodoTarifaKM = this.vista.PeriodoTarifaKM.HasValue && this.vista.PeriodoTarifaKM >= 0 ? (EPeriodoTarifa?)this.vista.PeriodoTarifaKM : null;

            linea.Equipo = equipo;
            linea.Cobrable = cobrable;

            return linea;
        }

        private void DatoAInterfazUsuario(object obj)
        {
            if (obj is Equipos.BO.UnidadBO)
            {
                #region Unidad A Interfaz de Usuario
                Equipos.BO.UnidadBO unidad = (Equipos.BO.UnidadBO)obj;

                //Información de la Unidad
                if (unidad == null) unidad = new Equipos.BO.UnidadBO();
                if (unidad.Modelo == null) unidad.Modelo = new ModeloBO();
                if (unidad.Modelo.Marca == null) unidad.Modelo.Marca = new MarcaBO();
                if (unidad.Sucursal == null) unidad.Sucursal = new SucursalBO();
                if (unidad.CaracteristicasUnidad == null) unidad.CaracteristicasUnidad = new CaracteristicasUnidadBO();

                this.vista.UnidadID = unidad.UnidadID;
                this.vista.EquipoID = unidad.EquipoID;
                this.vista.CapacidadCarga = unidad.CaracteristicasUnidad.PBCMaximoRecomendado;
                this.vista.RendimientoTanque = unidad.CaracteristicasUnidad.RendimientoTanque;
                this.vista.CapacidadTanque = unidad.CaracteristicasUnidad.CapacidadTanque;
                this.vista.Anio = unidad.Anio;

                if (unidad.NumeroEconomico != null && unidad.NumeroEconomico.Trim().CompareTo("") != 0)
                    this.vista.NumeroEconomico = unidad.NumeroEconomico;
                else
                    this.vista.NumeroEconomico = null;

                if (unidad.NumeroSerie != null && unidad.NumeroSerie.Trim().CompareTo("") != 0)
                    this.vista.VIN = unidad.NumeroSerie;
                else
                    this.vista.VIN = null;

                if (unidad.Modelo.Nombre != null && unidad.Modelo.Nombre.Trim().CompareTo("") != 0)
                    this.vista.ModeloNombre = unidad.Modelo.Nombre;
                else
                    this.vista.ModeloNombre = null;

                if (!string.IsNullOrEmpty(unidad.Modelo.Marca.Nombre) && !string.IsNullOrWhiteSpace(unidad.Modelo.Marca.Nombre))
                    this.vista.MarcaNombre = unidad.Modelo.Marca.Nombre.Trim().ToUpper();
                else
                    this.vista.MarcaNombre = null;

                if (unidad.Sucursal.Nombre != null && unidad.Sucursal.Nombre.Trim().CompareTo("") != 0)
                    this.vista.SucursalNombre = unidad.Sucursal.Nombre;
                else
                    this.vista.SucursalNombre = null;
                #endregion
            }
            if (obj is List<TramiteBO>)
            {
                #region Trámites A Interfaz de Usuario
                List<TramiteBO> lstTramites = (List<TramiteBO>)obj;

                TramiteBO tramite = null;
                //Placa Federal
                tramite = lstTramites.Find(p => p.Tipo != null && p.Tipo == ETipoTramite.PLACA_FEDERAL && p.Activo != null && p.Activo == true);
                if (tramite != null && tramite.Resultado != null && tramite.Resultado.Trim().CompareTo("") != 0)
                    this.vista.PlacaFederal = tramite.Resultado;
                else
                    this.vista.PlacaFederal = null;

                //Placa Estatal
                tramite = lstTramites.Find(p => p.Tipo != null && p.Tipo == ETipoTramite.PLACA_ESTATAL && p.Activo != null && p.Activo == true);
                if (tramite != null && tramite.Resultado != null && tramite.Resultado.Trim().CompareTo("") != 0)
                    this.vista.PlacaEstatal = tramite.Resultado;
                else
                    this.vista.PlacaEstatal = null;
                #endregion
            }
            if (obj is LineaContratoManttoBO)
            {
                #region Línea de Contrato a Interfaz de Usuario
                LineaContratoManttoBO linea = (LineaContratoManttoBO)obj;
                if (linea.Cobrable == null) linea.Cobrable = new TarifaManttoBO();
                if (linea.ProductoServicio == null) linea.ProductoServicio = new ProductoServicioBO();

                this.vista.LineaContratoID = linea.LineaContratoID;
                this.vista.KmEstimadoAnual = linea.KmEstimadoAnual;
                this.vista.CobrableID = linea.Cobrable.CobrableID;
                this.vista.CargoFijoMensual = ((TarifaManttoBO)linea.Cobrable).CargoFijoMensual;
                this.vista.KilometrosLibres = ((TarifaManttoBO)linea.Cobrable).KilometrosLibres;//SC051
                this.vista.CostoKmRecorrido = ((TarifaManttoBO)linea.Cobrable).CargoKmRecorrido;
                this.vista.HorasLibres = ((TarifaManttoBO)linea.Cobrable).HorasLibres;//SC051
                this.vista.CostoHorasRefrigeradas = ((TarifaManttoBO)linea.Cobrable).CargoHorasRefrigeradas;//SC051
                this.vista.PeriodoTarifaHRS = ((TarifaManttoBO)linea.Cobrable).PeriodoTarifaHR.HasValue
                                                  ? (int?)((TarifaManttoBO)linea.Cobrable).PeriodoTarifaHR.Value
                                                  : null;
                this.vista.PeriodoTarifaKM = ((TarifaManttoBO)linea.Cobrable).PeriodoTarifaKM.HasValue
                                                 ? (int?)((TarifaManttoBO)linea.Cobrable).PeriodoTarifaKM.Value
                                                 : null;

                this.vista.ProductoServicioId = linea.ProductoServicio.Id;
                this.vista.ClaveProductoServicio = linea.ProductoServicio.NombreCorto;
                this.vista.DescripcionProductoServicio = linea.ProductoServicio.Nombre;
                this.DatoAInterfazUsuario(linea.Equipo);
                this.vista.SubLineasContrato = linea.SubLineasContrato;
                #endregion
            }
        }

        /// <summary>
        /// Valida que se tengan todos los campos requeridos para la línea de contrato
        /// </summary>
        /// <returns>Mensaje que indica las inconsistencias durante la validación</returns>
        public string ValidarCampos()
        {
            string s = string.Empty;

            if (this.vista.UnidadID == null)
                s += "Unidad ID, ";
            if (this.vista.EquipoID == null)
                s += "Equipo ID, ";
            if (this.vista.CargoFijoMensual == null)
                s += "Cargo Fijo Mensual, ";
            if (this.vista.CostoKmRecorrido == null)
                s += "Costo por Kilómetro Recorrido, ";
            if (this.vista.KmEstimadoAnual == null)
                s += "Kilometraje Estimado Anual, ";
            if (string.IsNullOrWhiteSpace(this.vista.ClaveProductoServicio))
                s += "Producto Servicio, ";

            if (s != null && s.Trim().CompareTo("") != 0)
                return "Los siguientes campos no pueden estar vacíos: \n" + s.Substring(0, s.Length - 2);

            if (this.vista.SubLineasContrato != null)
            {
                foreach (SubLineaContratoManttoBO subLinea in this.vista.SubLineasContrato)
                    if (subLinea.Mantenimiento == null)
                        return "Es necesario que se indique en todos los equipos aliados si van a requerir mantenimiento o no.";
            }

            return null;
        }

        public void LimpiarSesion()
        {
            this.vista.LimpiarSesion();
        }

        #region SC_0051
        /// <summary>
        /// Prepara la inforamción para ser desplegada en la vista
        /// </summary>
        /// <param name="eaID">Identificador del equipo aliado</param>
        internal void PrepararVisualizacionTarifaEquipoAliado(int eaID)
        {
            var tarifaEA = this.vista.SubLineasContrato.FirstOrDefault(x => x.EquipoAliado.EquipoAliadoID.Value == eaID);

            if (tarifaEA != null)
            {
                this.vista.EquipoAliadoID = tarifaEA.EquipoAliado.EquipoAliadoID.HasValue
                                                ? tarifaEA.EquipoAliado.EquipoAliadoID
                                                : null;
                this.vista.CostoFijoMensualEA = tarifaEA.CargoFijoMensual.HasValue ? tarifaEA.CargoFijoMensual : null;
                this.vista.KilometrosLibresEA = tarifaEA.KilometrosLibres.HasValue ? tarifaEA.KilometrosLibres : null;
                this.vista.CostoKilometroEA = tarifaEA.CargoKilometros.HasValue ? tarifaEA.CargoKilometros : null;
                this.vista.HorasLibresEA = tarifaEA.HorasLibres.HasValue ? tarifaEA.HorasLibres : null;
                this.vista.CostoHoraRefrigeradaEA = tarifaEA.CargoHorasRefrigeradas.HasValue ? tarifaEA.CargoHorasRefrigeradas : null;
                this.vista.MantenimientoEA = tarifaEA.Mantenimiento.HasValue ? (tarifaEA.Mantenimiento.Value ? tarifaEA.Mantenimiento : (bool?)false) : (bool?)false;
                this.vista.PeriodoTarifaKMEA = tarifaEA.PeriodoTarifaKM.HasValue ? (int?)tarifaEA.PeriodoTarifaKM.Value : null;
                this.vista.PeriodoTarifaHRSEA = tarifaEA.PeriodoTarifaHR.HasValue ? (int?)tarifaEA.PeriodoTarifaHR.Value : null;
                this.vista.PermitirAsignarTarifasEA(this.vista.ModoEdicion);
                this.vista.MostrarTarifasEquipoAliado();
            }
        }
        /// <summary>
        /// Recupera y asigna a la vista, la información recuperada de la tarifa del equipo aliado
        /// </summary>
        /// <param name="eaID">Identificador del equipo aliado</param>
        internal void AsignarTarifaEA(int eaID)
        {
            var lstEA = this.vista.SubLineasContrato;
            var tarifa = lstEA.FirstOrDefault(x => x.EquipoAliado.EquipoAliadoID == eaID);

            if (tarifa != null)
            {
                tarifa.Mantenimiento = this.vista.MantenimientoEA;
                tarifa.CargoFijoMensual = this.vista.CostoFijoMensualEA;
                tarifa.KilometrosLibres = this.vista.KilometrosLibresEA;
                tarifa.CargoKilometros = this.vista.CostoKilometroEA;
                tarifa.HorasLibres = this.vista.HorasLibresEA;
                tarifa.CargoHorasRefrigeradas = this.vista.CostoHoraRefrigeradaEA;
                tarifa.PeriodoTarifaHR = (EPeriodoTarifa?)this.vista.PeriodoTarifaHRSEA;
                tarifa.PeriodoTarifaKM = (EPeriodoTarifa?)this.vista.PeriodoTarifaKMEA;
            }

            this.vista.SubLineasContrato = lstEA;

            this.vista.EquipoAliadoID = null;
            this.vista.CostoHoraRefrigeradaEA = null;
            this.vista.CostoKilometroEA = null;
            this.vista.MantenimientoEA = null;
        }
        /// <summary>
        /// Establece las opciones para el período de frecuencia de las tarifas
        /// </summary>
        internal void EstablecerOpcionesFrecuencia()
        {
            string key = "";
            int value = 0;
            Dictionary<int, string> lstTipos = new Dictionary<int, string>();
            lstTipos.Add(-1, "Selecione");
            foreach (var tipo in Enum.GetValues(typeof(EPeriodoTarifa)))
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
                    key = Enum.GetName(typeof(EPeriodoTarifa), value);
                }
                lstTipos.Add(value, key);
            }

            this.vista.EstablecerOpcionesFrecuencia(lstTipos);
        }
        #endregion

        #region Métodos para el Buscador

        public object PrepararBOBuscador(string catalogo) {
            object obj = null;

            switch (catalogo) {
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
        public void DesplegarResultadoBuscador(string catalogo, object selecto) {
            switch (catalogo) {
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