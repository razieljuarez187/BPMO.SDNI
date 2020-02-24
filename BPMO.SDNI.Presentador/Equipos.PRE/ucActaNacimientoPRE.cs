//Satisface al CU079 - Consultar Acta de Nacimiento de Unidad
using System.Collections.Generic;
using BPMO.Basicos.BO;
using BPMO.SDNI.Equipos.BO;
using BPMO.SDNI.Equipos.VIS;
using BPMO.SDNI.Comun.BO;
using BPMO.Servicio.Catalogos.BO;
using System;

namespace BPMO.SDNI.Equipos.PRE
{
    public class ucActaNacimientoPRE
    {
        #region Atributos
        IucActaNacimientoVIS vista;
        #endregion

        #region Constructores
        public ucActaNacimientoPRE(IucActaNacimientoVIS view)
        {
            this.vista = view;
        }
        #endregion

        #region Métodos
        public void CargarActaNacimiento(string xmlActaNacimiento)
        {
            if (xmlActaNacimiento == null) xmlActaNacimiento = "";

            BO.UnidadBO bo = new BO.UnidadBO();
            if (xmlActaNacimiento != null && xmlActaNacimiento.Trim().CompareTo("") != 0)
                bo = new BO.UnidadBO(xmlActaNacimiento);

            this.DatosAInterfazUsuario(bo);
        }

        private void DatosAInterfazUsuario(object obj)
        {
            BO.UnidadBO bo = (BO.UnidadBO)obj;
            #region Inicialización de Propiedades
            if (bo == null)
                bo = new BO.UnidadBO();
            if (bo.ActivoFijo == null)
                bo.ActivoFijo = new ActivoFijoBO();
            if (bo.CaracteristicasUnidad == null)
                bo.CaracteristicasUnidad = new CaracteristicasUnidadBO();
            if (bo.CaracteristicasUnidad.Ejes == null)
                bo.CaracteristicasUnidad.Ejes = new List<EjeBO>();
            if (bo.CaracteristicasUnidad.Motor == null)
                bo.CaracteristicasUnidad.Motor = new MotorBO();
            if (bo.CaracteristicasUnidad.SistemaElectrico == null)
                bo.CaracteristicasUnidad.SistemaElectrico = new SistemaElectricoBO();
            if (bo.CaracteristicasUnidad.Transmision == null)
                bo.CaracteristicasUnidad.Transmision = new TransmisionBO();
            if (bo.Cliente == null)
                bo.Cliente = new ClienteBO();
            if (bo.EquiposAliados == null)
                bo.EquiposAliados = new List<EquipoAliadoBO>();
            if (bo.Llantas == null)
                bo.LimpiarLlantas();
            if (bo.Mediciones == null)
                bo.Mediciones = new MedicionesBO();
            if (bo.Mediciones.Horometros == null)
                bo.Mediciones.Horometros = new List<HorometroBO>();
            if (bo.Mediciones.Odometros == null)
                bo.Mediciones.Odometros = new List<OdometroBO>();
            if (bo.Modelo == null)
                bo.Modelo = new ModeloBO();
            if (bo.Modelo.Marca == null)
                bo.Modelo.Marca = new MarcaBO();
            if (bo.Sucursal == null)
                bo.Sucursal = new SucursalBO();
            if (bo.Sucursal.UnidadOperativa == null)
                bo.Sucursal.UnidadOperativa = new UnidadOperativaBO();
            if (bo.TipoEquipoServicio == null)
                bo.TipoEquipoServicio = new TipoUnidadBO();
            
            if (bo.NumerosSerie == null)
                bo.NumerosSerie = new List<NumeroSerieBO>();
            
            #endregion

            this.vista.NumeroSerie = bo.NumeroSerie;
            this.vista.ClaveActivoOracle = bo.ClaveActivoOracle;
            this.vista.LiderID = bo.IDLider;
            this.vista.Anio = bo.Anio;
            this.vista.Modelo = bo.Modelo.Nombre;
            this.vista.NumeroEconomico = bo.NumeroEconomico;
            this.vista.TipoUnidad = bo.TipoEquipoServicio.Nombre;

            if (bo.Area != null)
                this.vista.Area = bo.Area.ToString();

            this.vista.Propietario = bo.Propietario;
            this.vista.Sucursal = bo.Sucursal.Nombre;
            this.vista.Cliente = bo.Cliente.Nombre;

            this.vista.Horometros = bo.Mediciones.Horometros;
            this.vista.ActualizarHorometros();
            this.vista.Odometros = bo.Mediciones.Odometros;
            this.vista.ActualizarOdometros();

            this.vista.CapacidadTanque = bo.CaracteristicasUnidad.CapacidadTanque;
            this.vista.RendimientoTanque = bo.CaracteristicasUnidad.RendimientoTanque;
            this.vista.PBCMaximoRecomendado = bo.CaracteristicasUnidad.PBCMaximoRecomendado;
            this.vista.PBVMaximoRecomendado = bo.CaracteristicasUnidad.PBVMaximoRecomendado;

            this.vista.Radiador = bo.CaracteristicasUnidad.Radiador;
            this.vista.PostEnfriador = bo.CaracteristicasUnidad.PostEnfriador;
            this.vista.SerieCompresorAire = bo.CaracteristicasUnidad.Motor.SerieCompresorAire;
            this.vista.SerieECM = bo.CaracteristicasUnidad.Motor.SerieECM;
            #region SC0030
            this.vista.SerieMotor = bo.CaracteristicasUnidad.Motor.SerieMotor;
            #endregion
            this.vista.SerieTurboCargador = bo.CaracteristicasUnidad.Motor.SerieTurboCargador;
            this.vista.SerieAlternador = bo.CaracteristicasUnidad.SistemaElectrico.SerieAlternador;
            this.vista.SerieBaterias = bo.CaracteristicasUnidad.SistemaElectrico.SerieBaterias;
            this.vista.SerieMarcha = bo.CaracteristicasUnidad.SistemaElectrico.SerieMarcha;
            this.vista.TransmisionModelo = bo.CaracteristicasUnidad.Transmision.Modelo;
            this.vista.TransmisionSerie = bo.CaracteristicasUnidad.Transmision.Serie;

            EjeBO eje = bo.CaracteristicasUnidad.ObtenerEjePorPosicion(EPosicionEje.Delantero);
            if (eje != null)
            {
                this.vista.EjeDireccionModelo = eje.Modelo;
                this.vista.EjeDireccionSerie = eje.Serie;
            }
            eje = bo.CaracteristicasUnidad.ObtenerEjePorPosicion(EPosicionEje.TraseroDelantero);
            if (eje != null)
            {
                this.vista.EjeTraseroDelanteroModelo = eje.Modelo;
                this.vista.EjeTraseroDelanteroSerie = eje.Serie;
            }
            eje = bo.CaracteristicasUnidad.ObtenerEjePorPosicion(EPosicionEje.TraseroTrasero);
            if (eje != null)
            {
                this.vista.EjeTraseroTraseroModelo = eje.Modelo;
                this.vista.EjeTraseroTraseroSerie = eje.Serie;
            }

            this.vista.Llantas = bo.ObtenerLlantas();
            this.vista.ActualizarLlantas();

            LlantaBO refaccion = bo.ObtenerRefaccion();
            if (refaccion != null)
            {
                this.vista.RefaccionCodigo = refaccion.Codigo;
                this.vista.RefaccionMarca = refaccion.Marca;
                this.vista.RefaccionMedida = refaccion.Medida;
                this.vista.RefaccionModelo = refaccion.Modelo;
                this.vista.RefaccionProfundidad = refaccion.Profundidad;
                this.vista.RefaccionRevitalizada = refaccion.Revitalizada;
            }

            this.vista.EquiposAliados = bo.EquiposAliados;
            this.vista.ActualizarEquiposAliados();
                       
            this.vista.NumerosSerie = bo.NumerosSerie;
            this.vista.ActualizarNumerosSerie();
            
        }

        #region REQ 13285 Métodos relacionado con las acciones dependiendo de la unidad operativa.

        /// <summary>
        /// Determina las acciones relacionadas con el comportamiento de las vistas.
        /// </summary>
        /// <param name="listaAcciones">Lista de objetos de tipo CatalogoBaseBO que contiene las acciones a las cuales el usuario tiene permiso.</param>
        public void EstablecerAcciones(List<CatalogoBaseBO> listaAcciones)
        {
            ETipoEmpresa EmpresaConPermiso = ETipoEmpresa.Idealease;
            string ValoresTabs = string.Empty;
            switch (this.vista.UnidadOperativaID)
            {
                case (int)ETipoEmpresa.Generacion:
                    if (ExisteAccion(listaAcciones, "UI ACTA GENERACION"))
                    {
                        EmpresaConPermiso = ETipoEmpresa.Generacion;
                    }
                    break;
                case (int)ETipoEmpresa.Equinova:
                    if (ExisteAccion(listaAcciones, "UI ACTA GENERACION")) {
                        EmpresaConPermiso = ETipoEmpresa.Equinova;
                    }
                    break;
                case (int)ETipoEmpresa.Construccion:
                    if (ExisteAccion(listaAcciones, "UI ACTA CONSTRUCCION"))
                    {
                        EmpresaConPermiso = ETipoEmpresa.Construccion;
                    }
                    break;
            }

            if (!ExisteAccion(listaAcciones, "UI DATOS TECNICOS"))
            {
                ValoresTabs = "1,";
            }
            if (!ExisteAccion(listaAcciones, "UI NUMEROSERIE"))
            {
                ValoresTabs += "2,";
            }
            if (!ExisteAccion(listaAcciones, "UI LLANTAS"))
            {
                ValoresTabs += "3,";
            }
            if (!ExisteAccion(listaAcciones, "UI EQUIPOALIADO"))
            {
                ValoresTabs += "4";
            }

            ValoresTabs = ValoresTabs.TrimEnd(',');

            this.vista.EstablecerAcciones(EmpresaConPermiso, ValoresTabs);
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
