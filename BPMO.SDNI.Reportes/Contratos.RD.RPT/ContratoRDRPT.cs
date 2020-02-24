// Satisface al Caso de uso CU014 - Imprimir Contrato de Renta Diaria
//Satisface al CU011 - Imprimir Cierre Contrato Renta Diaria
// Satisface a la solicitud de Cambio SC0001
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Xml;
using BPMO.Basicos.BO;
using BPMO.SDNI.Comun.BO;
using BPMO.SDNI.Contratos.BO;
using BPMO.SDNI.Contratos.RD.BO;
using BPMO.SDNI.Equipos.BO;
using BPMO.SDNI.Tramites.BO;
using BPMO.Servicio.Catalogos.BO;
using DevExpress.XtraPrinting;
using DevExpress.XtraReports.UI;
using FieldInfo = System.Reflection.FieldInfo;
using UnidadBO = BPMO.SDNI.Equipos.BO.UnidadBO;

namespace BPMO.SDNI.Contratos.RD.RPT
{
    public partial class ContratoRDRPT : DevExpress.XtraReports.UI.XtraReport
    {
        private string xml = "BPMO.SDNI.Reportes.Contrato.Renta.Diaria.xml";
        public ContratoRDRPT(Dictionary<string, Object> datos, string urlXML)
        {
            try
            {
                InitializeComponent();
                ImprimirReporte(datos, ObtenerXMLContenido(urlXML));
            }
            catch (Exception ex)
            {
                throw new Exception("Error al imprimir el reporte. ContratoRentaDiariaRPT: " + ex.Message);
            }

        }
        protected XmlDocument ObtenerXMLContenido(string xmlUrl)
        {
            try
            {
                XmlDocument xDoc= new XmlDocument();
                xDoc.Load(xmlUrl + xml);
                return xDoc;
            }
            catch (Exception ex)
            {
                
                throw new Exception("ContratoRentaDiariaRPT.ObtenerXMLContenido:Error al cargar el archivo XML." + ex.Message);
            }
        }
        protected void ImprimirReporte(Dictionary<string,Object> datos, XmlDocument documento)
        {
            try
            {
                #region Obtener Formato del XML
                string leyendaCondiciones = string.Empty;
                string leyendaCargoCombustible = string.Empty;
                string leyendaBitacora = string.Empty;
                string leyendaSeguro = string.Empty;
                string leyendaArrendatarios = string.Empty;
                string leyendaOperacion = string.Empty;
                string leyendaInspeccion = string.Empty;
                string leyendaTitulo = string.Empty;
                string numeroPlaca = string.Empty;
                string direccionSuc = string.Empty;
                string telefonoUO = string.Empty;
                string direccionUO = string.Empty;

                XmlNodeList textoCondiciones = documento.GetElementsByTagName("condiciones");
                if(textoCondiciones.Count < 1) throw new Exception("el formato del archivo XML es incorrecto");
                leyendaCondiciones = textoCondiciones[0].InnerText;
                XmlNodeList textoCargoCombustible = documento.GetElementsByTagName("cargoCombustible");
                if(textoCargoCombustible.Count < 1 ) throw new Exception("El formato del archivo XML es incorrecto");
                leyendaCargoCombustible = textoCargoCombustible[0].InnerText;
                XmlNodeList textoBitacora = documento.GetElementsByTagName("bitacora");
                if (textoBitacora.Count < 1) throw new Exception("El formato del archivo XML es incorrecto");
                leyendaBitacora = textoBitacora[0].InnerText;
                XmlNodeList textoSeguro = documento.GetElementsByTagName("seguro");
                if (textoSeguro.Count < 1) throw new Exception("El formato del archivo XML es incorrecto");
                leyendaSeguro = textoSeguro[0].InnerText;
                XmlNodeList textoArrendatarios = documento.GetElementsByTagName("arrendatarios");
                if (textoArrendatarios.Count < 1) throw new Exception("El formato del archivo XML es incorrecto");
                leyendaArrendatarios = textoArrendatarios[0].InnerText;
                XmlNodeList textoOperacion = documento.GetElementsByTagName("operacion");
                if (textoOperacion.Count < 1) throw new Exception("El formato del archivo XML es incorrecto");
                leyendaOperacion = textoOperacion[0].InnerText;
                XmlNodeList textoInspeccion = documento.GetElementsByTagName("inspeccion");
                if (textoInspeccion.Count < 1) throw new Exception("El formato del archivo XML es incorrecto");
                leyendaInspeccion = textoInspeccion[0].InnerText;
                XmlNodeList textoTitulo = documento.GetElementsByTagName("Titulo");
                if (textoTitulo.Count < 1) throw new Exception("El formato del archivo XML es incorrecto");
                leyendaTitulo = textoTitulo[0].InnerText;
                #endregion
                #region Iniciar Variables

                if (datos["Contrato"] == null) throw new Exception("Se esperaba un contrato");
                if (datos["Firmantes"] == null) throw new Exception("Se esperaba Firmantes");
                if (datos["Modulo"] == null) throw new Exception("Se esperaba una configuración de módulo");
                if(!(datos["Contrato"] is ContratoRDBO)) throw new Exception("Se esperaba un contrato de Renta Diaria");

                // Contrato
                ContratoRDBO contrato = (ContratoRDBO)datos["Contrato"];
                if (contrato == null)
                    contrato = new ContratoRDBO();
                if(contrato.Sucursal == null)
                    contrato.Sucursal = new SucursalBO();
                if (contrato.Sucursal.DireccionesSucursal == null)
                    direccionSuc= string.Empty;
                else
                {
                    var direcUO = contrato.Sucursal.DireccionesSucursal.Find(x => x.Primaria != null && x.Primaria.Value == true);
                    if (direcUO != null)
                    {
                        if (!string.IsNullOrEmpty(direcUO.Calle) &&
                            !string.IsNullOrWhiteSpace(direcUO.Calle))
                        {
                            direccionSuc = direcUO.Calle;
                        }

                    }
                }
                if(contrato.Cliente == null)
                    contrato.Cliente = new CuentaClienteIdealeaseBO();
                if(contrato.Cliente.Cliente == null)
                    contrato.Cliente.Cliente = new ClienteBO();
                if (contrato.Cliente.Direcciones == null)
                {
                    DireccionClienteBO direccion = new DireccionClienteBO();
                    contrato.Cliente.Agregar(direccion);
                }
                if(contrato.Operador == null)
                    contrato.Operador = new OperadorBO();
                if(contrato.Operador.Direccion == null)
                    contrato.Operador.Direccion = new DireccionPersonaBO();
                if (contrato.Operador.Licencia == null)
                    contrato.Operador.Licencia = new LicenciaBO();
                if(contrato.Operador.Direccion.Ubicacion == null)
                    contrato.Operador.Direccion.Ubicacion = new UbicacionBO();
                if(contrato.Operador.Direccion.Ubicacion.Ciudad == null)
                    contrato.Operador.Direccion.Ubicacion.Ciudad = new CiudadBO();
                if(contrato.Operador.Direccion.Ubicacion.Estado == null)
                    contrato.Operador.Direccion.Ubicacion.Estado = new EstadoBO();

                LineaContratoRDBO linea = contrato.ObtenerLineaContrato();
                if(linea == null)
                    linea = new LineaContratoRDBO();
                if (linea.Equipo == null)
                    linea.Equipo = new UnidadBO();
                if(linea.Equipo.TipoEquipoServicio == null)
                    linea.Equipo.TipoEquipoServicio = new TipoUnidadBO();
                if(linea.Equipo.ActivoFijo == null)
                    linea.Equipo.ActivoFijo = new ActivoFijoBO();
                if(linea.Equipo.Modelo == null)
                    linea.Equipo.Modelo = new ModeloBO();
                if(((UnidadBO)linea.Equipo).CaracteristicasUnidad == null)
                    ((UnidadBO)linea.Equipo).CaracteristicasUnidad = new CaracteristicasUnidadBO();
                if (linea.Equipo.TipoEquipoServicio == null)
                    linea.Equipo.TipoEquipoServicio = new TipoUnidadBO();
                if(linea.Cobrable == null)
                    linea.Cobrable = new TarifaContratoRDBO();
                contrato.LineasContrato = new List<ILineaContrato>();
                contrato.AgregarLineaContrato(linea);
                if(linea.ListadosVerificacion == null)
                    linea.ListadosVerificacion = new List<ListadoVerificacionBO>();

                // Configuración del Modulo

                ModuloBO modulo = (ModuloBO)datos["Modulo"];
                ConfiguracionUnidadOperativaBO unidadOperativaConfiguracion;
                if(modulo == null)
                    modulo = new ModuloBO();
                if(modulo.Configuracion == null)
                    modulo.Configuracion = new ConfiguracionModuloBO();
                if (contrato.Sucursal.UnidadOperativa.Id == null)
                    unidadOperativaConfiguracion = new ConfiguracionUnidadOperativaBO();
                else
                    unidadOperativaConfiguracion = modulo.ObtenerConfiguracionUO( new UnidadOperativaBO{Id = contrato.Sucursal.UnidadOperativa.Id});
                if(unidadOperativaConfiguracion == null)
                    unidadOperativaConfiguracion = new ConfiguracionUnidadOperativaBO();
                if (unidadOperativaConfiguracion.ConfiguracionModulo == null)
                    unidadOperativaConfiguracion.ConfiguracionModulo = new ConfiguracionModuloBO();
                //Tramites
                List<TramiteBO> tramites = (List<TramiteBO>) datos["Tramites"];
                if(tramites == null)
                    tramites = new List<TramiteBO>();
                
                PlacaEstatalBO placaEstatal = (PlacaEstatalBO) tramites.Find(t => t.Tipo == ETipoTramite.PLACA_ESTATAL);
                if (placaEstatal != null)
                    numeroPlaca = placaEstatal.Resultado;
                PlacaFederalBO placaFederal = (PlacaFederalBO) tramites.Find(t => t.Tipo == ETipoTramite.PLACA_FEDERAL);
                if (placaFederal != null)
                    numeroPlaca = placaFederal.Resultado;
                //Firmantes

                Tuple<IConstituible, List<PersonaBO>, List<IConstituible>> firmantes =
                    (Tuple<IConstituible, List<PersonaBO>, List<IConstituible>>) datos["Firmantes"];
                if (firmantes == null)
                {
                    IConstituible cliente = new CuentaClienteIdealeaseBO();
                    List<PersonaBO> representantes = new List<PersonaBO>();
                    List<IConstituible> depositarios = new List<IConstituible>();
                    firmantes = new Tuple<IConstituible, List<PersonaBO>, List<IConstituible>>(cliente, representantes,
                                                                                               depositarios);
                }

                //Usuarios
                UsuarioBO usuarioCreacion = (UsuarioBO) datos["UsuarioCreacion"];
                if (usuarioCreacion == null)
                    usuarioCreacion = new UsuarioBO();

                UsuarioBO usuarioCierre = (UsuarioBO)datos["UsuarioCierre"];
                if(usuarioCierre == null)
                    usuarioCierre = new UsuarioBO();

                //Sucursal Matriz
                SucursalBO matriz = (SucursalBO)datos["SucursalMatriz"];
                if(matriz == null)
                    matriz = new SucursalBO();
                if(matriz.UnidadOperativa == null)
                    matriz.UnidadOperativa = new UnidadOperativaBO();
                if (matriz.DireccionesSucursal == null)
                    direccionUO = string.Empty;
                else
                {
                    var direcUO = matriz.DireccionesSucursal.Find(x => x.Primaria != null && x.Primaria.Value == true);
                    if (direcUO != null)
                    {
                        if (!string.IsNullOrEmpty(direcUO.Telefono) &&
                            !string.IsNullOrWhiteSpace(direcUO.Telefono))
                        {
                            telefonoUO = direcUO.Telefono;
                            if (!string.IsNullOrEmpty(direcUO.Calle) &&
                            !string.IsNullOrWhiteSpace(direcUO.Calle))
                            direccionUO = direcUO.Calle;
                        }
                        else
                        {
                            if (!string.IsNullOrEmpty(direcUO.Calle) &&
                            !string.IsNullOrWhiteSpace(direcUO.Calle))
                                direccionUO = direcUO.Calle;
                        }

                    }
                }

                #endregion
                #region Asignar valores al Reporte
                #region Encabezado 
                xrlblTitulo.Html = leyendaTitulo;
                if (String.IsNullOrEmpty(unidadOperativaConfiguracion.ConfiguracionModulo.URLLogoEmpresa))
                    xrLogo.ImageUrl = modulo.Configuracion.URLLogoEmpresa;
                else
                    xrLogo.ImageUrl = unidadOperativaConfiguracion.ConfiguracionModulo.URLLogoEmpresa;
                xrlblFormatoNumero.Text = contrato.NumeroContrato ?? String.Empty;
                if (contrato.Estatus == EEstatusContrato.Borrador || contrato.Estatus == EEstatusContrato.EnPausa)
                    Watermark.Text = "BORRADOR";
                
                #endregion
                #region Seccion Izquierda
                
                xrlblUnidadOperativa.Text = matriz.UnidadOperativa.Nombre??String.Empty;
                xrlblCliente.Text = contrato.Cliente.Nombre ?? String.Empty;
                xrlblRFC.Text = contrato.Cliente.Cliente.RFC ?? String.Empty;
                xrtblCellDireccion.Text = contrato.Cliente.Direccion ?? String.Empty;

                DireccionClienteBO direccionCliente = contrato.Cliente.Direcciones.Find(c => c.Primaria != null && c.Primaria.Value) ??
                            new DireccionClienteBO();

                if (direccionCliente.Ubicacion == null)
                    direccionCliente.Ubicacion = new UbicacionBO();
                if (direccionCliente.Ubicacion.Ciudad == null)
                    direccionCliente.Ubicacion.Ciudad = new CiudadBO();
                if(direccionCliente.Ubicacion.Estado == null)
                    direccionCliente.Ubicacion.Estado = new EstadoBO();

                xrtblCellCiudad.Text = direccionCliente.Ubicacion.Ciudad.Codigo??String.Empty;
                xrtblCellEstado.Text = direccionCliente.Ubicacion.Estado.Codigo??String.Empty;
                xrtblCellCP.Text = direccionCliente.CodigoPostal??String.Empty;



                xrtblCellNumeroCuenta.Text = contrato.Cliente.Id !=null?contrato.Cliente.Id.ToString(): String.Empty;

                //SC0021 orden de compra con N/A
                if (contrato.FormaPago == null) xrtblCellOrdenCompra.Text = String.Empty;
                if (contrato.FormaPago == null && contrato.TipoConfirmacion == null) xrtblCellOrdenCompra.Text = String.Empty;
                else
                {
                    if (contrato.FormaPago != null && contrato.TipoConfirmacion != null &&
                        contrato.FormaPago == EFormaPago.CREDITO &&
                        contrato.TipoConfirmacion == ETipoConfirmacion.ORDEN_DE_COMPRA)
                        xrtblCellOrdenCompra.Text = contrato.AutorizadorOrdenCompra;
                    else
                        xrtblCellOrdenCompra.Text = "N/A";

                }
                xrtblCellNombreOperador.Text = contrato.Operador.Nombre ?? String.Empty;
                xrtblCellExperienciaOperador.Text = contrato.Operador.AñosExperiencia != null
                                                        ? contrato.Operador.AñosExperiencia.ToString()
                                                        : String.Empty;
                xrtblCellCalleOperador.Text = contrato.Operador.Direccion.Calle ?? String.Empty;
                xrtblCellCiudadOperador.Text = contrato.Operador.Direccion.Ubicacion.Ciudad.Nombre ?? String.Empty;
                xrtblCellEstadoOperador.Text = contrato.Operador.Direccion.Ubicacion.Estado.Nombre ?? String.Empty;
                xrtblCellCPOperador.Text = contrato.Operador.Direccion.CodigoPostal ?? String.Empty;
                if (contrato.Operador.Licencia.Tipo == ETipoLicencia.ESTATAL)
                    xrchkEstatal.Checked = true;
                if (contrato.Operador.Licencia.Tipo == ETipoLicencia.FEDERAL)
                    xrchkFederal.Checked = true;
                xrTableCell20.Controls.Add(xrchkEstatal);
                xrTableCell21.Controls.Add(xrchkFederal);
                xrtblCellNumeroLicencia.Text = contrato.Operador.Licencia.Numero ?? String.Empty;
                xrtblCellEstadoLicencia.Text = contrato.Operador.Licencia.Estado ?? String.Empty;
                xrtblCellFechaExpiracion.Text = contrato.Operador.Licencia.FechaExpiracion != null
                                                    ? contrato.Operador.Licencia.FechaExpiracion.Value.ToString("dd/MM/yyyy")
                                                    : String.Empty;
                xrtblCellFechaNacimiento.Text = contrato.Operador.FechaNacimiento != null
                                                    ? contrato.Operador.FechaNacimiento.Value.ToString("dd/MM/yyyy")
                                                    : String.Empty;
                xrlblLeyendaCondiciones.Html = leyendaCondiciones;
                xrtblCellAreaOperacion.Text = contrato.DestinoAreaOperacion ?? String.Empty;
                xrtblCellVehiculoDevuelto.Text = direccionSuc ?? String.Empty;
                xrtblCellMercanciaTrasportar.Text = contrato.MercanciaTransportar ?? String.Empty;

                if (contrato.MotivoRenta == EMotivoRenta.DEMOSTRACION)
                    xrchkDemostracion.Checked = true;
                if (contrato.MotivoRenta == EMotivoRenta.MATERIAL_PELIGROSO)
                    xrchkMaterialPeligroso.Checked = true;
                if (contrato.MotivoRenta == EMotivoRenta.SUSTITUCION_TEMPORAL)
                    xrchkSustitucionTemporal.Checked = true;
                if (contrato.MotivoRenta == EMotivoRenta.UNIDAD_EXTRA)
                    xrchkUnidadExtra.Checked = true;
                xrlblLeyendaCargosCombustible.Html = leyendaCargoCombustible;
                xrlblLeyendaBitacoraViaje.Html = leyendaBitacora;
                if (contrato.BitacoraViajeConductor == true)
                    xrchkBitacora.Checked = true;

                SeguroBO seguro = (SeguroBO)tramites.Find(t => t.Tipo == ETipoTramite.SEGURO);
                if (seguro != null)
                {
                    string aseguradoraTelefono = string.Empty;
                    aseguradoraTelefono = !String.IsNullOrEmpty(seguro.Aseguradora) ? seguro.Aseguradora : string.Empty;

                    aseguradoraTelefono = !String.IsNullOrEmpty(seguro.Contacto)
                                              ? (!String.IsNullOrEmpty(aseguradoraTelefono)
                                                     ? aseguradoraTelefono + " - " + seguro.Contacto
                                                     : aseguradoraTelefono)
                                              : aseguradoraTelefono;

                    xrtblCellCompaniaAseguradora.Text = !String.IsNullOrEmpty(aseguradoraTelefono)
                                                            ? aseguradoraTelefono
                                                            : String.Empty;
                     xrlblNumeroPoliza.Text = seguro.NumeroPoliza?? String.Empty;
                    xrlblCompania.Text = seguro.Aseguradora ?? String.Empty;
                }

                LineaContratoRDBO lineaTemp = contrato.ObtenerLineaContrato();
                if (lineaTemp.Equipo.ActivoFijo.CostoSinIva == null)
                    leyendaSeguro = leyendaSeguro.Replace("{MONTODEDUCIBLE}", "__________");
                else
                {
                    Decimal? montoDeducibleCalcuado = 0;
                    montoDeducibleCalcuado = lineaTemp.Equipo.ActivoFijo.CostoSinIva;
                    var unidad = (UnidadBO) lineaTemp.Equipo;
                    if(unidad.EquiposAliados.Count > 0)
                    {
                        montoDeducibleCalcuado = unidad.EquiposAliados.Aggregate(montoDeducibleCalcuado, (monto, equipoAliado) => equipoAliado.ActivoFijo != null ? equipoAliado.ActivoFijo.CostoSinIva != null ? monto + equipoAliado.ActivoFijo.CostoSinIva : monto : monto);
                    }
                   //SC0021 formato de decimales
                    leyendaSeguro = leyendaSeguro.Replace("{MONTODEDUCIBLE}", String.Format("{0:#,##0.00##}", contrato.CalcularMontoDeducible((Decimal)montoDeducibleCalcuado).Value));
                }
                xrlblLeyendaSeguro.Html = leyendaSeguro;

                xrlblFirmaUnidadOperativa.Text = matriz.UnidadOperativa.Nombre;
                xrlblRepresentanteUnidadOperativa.Text = unidadOperativaConfiguracion.Representante;
                xrlblTelefonoUnidadOperativa.Text = telefonoUO;
                xrlblDireccionUnidadOperativa.Text = direccionUO;
                if (contrato.Cliente.EsFisico == null || contrato.Cliente.EsFisico == true)
                {
                    RepresentanteLegalBO representante;
                    if(firmantes.Item2 == null)
                    representante = new RepresentanteLegalBO();
                    else
                    {
                        representante = (RepresentanteLegalBO)firmantes.Item2[0];
                    }
                    string cliente = representante.Nombre;
                    cliente = !String.IsNullOrEmpty(representante.Telefono)
                                  ? cliente + "/n" + representante.Telefono
                                  : cliente;
                    cliente = !String.IsNullOrEmpty(representante.Direccion)
                                  ? cliente + "/n" + representante.Direccion
                                  : cliente;
                    xrlblDatosClienteMoral.Text = cliente;

                    xrSubreport3.Visible = false;
                }
                if (contrato.Cliente.EsFisico == false)
                {
                    xrlblDatosClienteMoral.BorderWidth = 0;
                    string clienteMoral = contrato.Cliente.Nombre;
                    var direccion = contrato.Cliente.Direcciones.Find(p => p.Primaria != null && p.Primaria.Value);

                    if (direccion != null)
                    {
                        if (!String.IsNullOrWhiteSpace(direccion.Telefono) && !String.IsNullOrEmpty(direccion.Telefono))
                            clienteMoral = clienteMoral + "\n" + direccion.Telefono;
                    }
                    clienteMoral = clienteMoral + "\n" + contrato.Cliente.Direccion;
                    xrlblDatosClienteMoral.Text = clienteMoral;
                    if (firmantes.Item2 != null)
                    {
                        firmantes.Item2.ForEach(r => r.Telefono = null);
                        xrSubreport3.ReportSource.DataSource = firmantes.Item2.ConvertAll(r => (RepresentanteLegalBO) r);
                        
                    }
                    else
                    {
                        xrSubreport3.Visible = false;
                    }
                }

                #endregion
                #region Seccion Derecha
                
                xrlblFecha.Text = contrato.FechaContrato != null
                                      ? contrato.FechaContrato.Value.ToShortDateString()
                                      : String.Empty;
                
                xrlblFechaPromesaDevolucion.Text = contrato.FechaPromesaDevolucion != null
                                                       ? contrato.FechaPromesaDevolucion.Value.ToShortDateString()
                                                       : String.Empty;
                xrlblModelo.Text = lineaTemp.Equipo.Modelo.Nombre ?? String.Empty;
                xrtblCellNumeroEconomico.Text = ((UnidadBO) lineaTemp.Equipo).NumeroEconomico ?? String.Empty;
                xrtblCellNumeroSerie.Text = lineaTemp.Equipo.NumeroSerie ?? String.Empty;
                xrtblCellNumeroPlaca.Text = numeroPlaca;
                //SC0021 formato de decimales
                xrtblCellPBC.Text = ((UnidadBO) lineaTemp.Equipo).CaracteristicasUnidad.PBCMaximoRecomendado != null
                                        ? String.Format("{0:#,##0.00##}", ((UnidadBO)lineaTemp.Equipo).CaracteristicasUnidad.PBCMaximoRecomendado)
                                        : String.Empty;

                xrlblTipo.Text = lineaTemp.Equipo.TipoEquipoServicio.Nombre?? String.Empty;
                xrSubreport1.ReportSource.DataSource = ((UnidadBO) linea.Equipo).EquiposAliados;
                //xrlblModeloEquipoAliado.DataBindings.Add(new XRBinding("Text", (((UnidadBO)linea.Equipo).EquiposAliados), "Modelo.Nombre"));
                //xrlblSerieEquipoAliado.DataBindings.Add(new XRBinding("Text", (((UnidadBO)linea.Equipo).EquiposAliados), "NumeroSerie"));
                
                
                if (lineaTemp.Equipo.EquipoID != null)
                {
                    xrtblCellFechaDevolucion.Text = contrato.ObtenerFechaDevolucion((UnidadBO) lineaTemp.Equipo) != null
                                                        ? contrato.ObtenerFechaDevolucion((UnidadBO) lineaTemp.Equipo)
                                                                  .Value.ToString()
                                                        : String.Empty;
                    xrtblCellFechaSalida.Text = contrato.ObtenerFechaEntrega((UnidadBO) lineaTemp.Equipo) != null
                                                    ? contrato.ObtenerFechaEntrega((UnidadBO) lineaTemp.Equipo)
                                                              .Value.ToString()
                                                    : String.Empty;
                    xrtblCellKmTotal.Text = contrato.CalcularKilometrajeRecorrido((UnidadBO)lineaTemp.Equipo) != null
                                            ? String.Format("{0:#,##0}",contrato.CalcularKilometrajeRecorrido((UnidadBO)lineaTemp.Equipo))
                                            : String.Empty;

                    xrlblCargoKilometro.Text = 
                        ((TarifaContratoRDBO)lineaTemp.Cobrable).RangoTarifas.First().CargoKm != null
                                              ? "$ " + String.Format("{0:#,##0.00##}", ((TarifaContratoRDBO)lineaTemp.Cobrable).RangoTarifas.First().CargoKm)
                                              : "$";


                    xrtblCellTotalEquipoAliado.Text = contrato.CalcularHorasConsumidas((UnidadBO)lineaTemp.Equipo) != null
                                                          ? String.Format("{0:#,##0}", contrato.CalcularHorasConsumidas((UnidadBO)lineaTemp.Equipo))
                                                          : String.Empty;

                }
                else
                {
                    xrtblCellFechaDevolucion.Text = String.Empty;
                    xrtblCellFechaSalida.Text = String.Empty;
                    xrtblCellKmTotal.Text = String.Empty;
                    xrlblCargoKilometro.Text = "$";
                    xrtblCellTotalEquipoAliado.Text = String.Empty;

                }
                xrtblCellDias.Text = contrato.CalcularDiasTranscurridosRenta() != null
                                         ? String.Format("{0:#,##0}", contrato.CalcularDiasTranscurridosRenta())
                                         : String.Empty;
                xrtblCellHoras.Text = contrato.CalcularHorasTranscurridasRenta() != null
                                          ? String.Format("{0:#,##0.00##}", contrato.CalcularHorasTranscurridasRenta())
                                          : String.Empty;
                if (contrato.LectorKilometraje == ELectorKilometraje.HUBODOMETRO)
                    xrchkHubodometro.Checked = true;
                if (contrato.LectorKilometraje == ELectorKilometraje.ODOMETRO)
                    xrchkOdometro.Checked = true;
                ListadoVerificacionBO listadoEntrada =
                    lineaTemp.ListadosVerificacion.Find(l => l.Tipo == ETipoListadoVerificacion.RECEPCION);
                if (listadoEntrada == null)
                {
                    xrtblCellKmEntrada.Text = String.Empty;
                    xrtblCellHrEntrada.Text = String.Empty;
                }
                else
                {
                    xrtblCellKmEntrada.Text = listadoEntrada.Kilometraje != null
                                                  ? String.Format("{0:#,##0}", listadoEntrada.Kilometraje)
                                                  : String.Empty;
                    xrtblCellHrEntrada.Text = listadoEntrada.Horometro != null
                                                  ? String.Format("{0:#,##0}", listadoEntrada.Horometro)
                                                  : String.Empty;
                }
                ListadoVerificacionBO listadoSalida =
                    lineaTemp.ListadosVerificacion.Find(l => l.Tipo == ETipoListadoVerificacion.ENTREGA);
                if (listadoSalida == null)
                {
                    xrtblCellKmSalida.Text = String.Empty;
                    xrtblCellHrSalida.Text = String.Empty;
                }
                else
                {
                    xrtblCellKmSalida.Text = listadoSalida.Kilometraje != null
                                                 ? String.Format("{0:#,##0}", listadoSalida.Kilometraje)
                                                 : String.Empty;
                    xrtblCellHrSalida.Text = listadoSalida.Horometro != null
                                                 ? String.Format("{0:#,##0}", listadoSalida.Horometro)
                                                 : String.Empty;
                }
               
                //SC0021 formato de decimales
                var tarifaContrato = lineaTemp.Cobrable != null ? lineaTemp.Cobrable as TarifaContratoRDBO : new TarifaContratoRDBO();
                xrlblTarifaDiaria.Text = tarifaContrato.TarifaDiaria != null
                                                 ? "$ " + String.Format("{0:#,##0.00##}", tarifaContrato.TarifaDiaria)
                                                 : "$";

                xrlblTarifaHora.Text = tarifaContrato.RangoTarifas != null && tarifaContrato.RangoTarifas.Any() && tarifaContrato.RangoTarifas.First().CargoHr != null
                                               ? "$ " + String.Format("{0:#,##0.00##}", tarifaContrato.RangoTarifas.First().CargoHr)
                                               : "$";
                if (unidadOperativaConfiguracion.PrecioUnidadCombustible == null)
                    xrtblCellLitro.Text = "";

                xrtblCellPrecioLitro.Text = unidadOperativaConfiguracion.PrecioUnidadCombustible != null
                                                ? "$ " + String.Format("{0:#,##0.00##}",unidadOperativaConfiguracion.PrecioUnidadCombustible)
                                                : "$";

                string descripcion = string.Empty;
                if (contrato.FormaPago != null)
                {
                    
                    FieldInfo fi = contrato.FormaPago.GetType().GetField(contrato.FormaPago.ToString());

                    DescriptionAttribute[] attributes =
                        (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);

                    if (attributes != null &&
                        attributes.Length > 0)
                        descripcion = attributes[0].Description;
                    else
                        descripcion = contrato.FormaPago.ToString();
                }
                if (contrato.TipoConfirmacion != null)
                {
                    FieldInfo fi = contrato.TipoConfirmacion.GetType().GetField(contrato.TipoConfirmacion.ToString());

                    DescriptionAttribute[] attributes =
                        (DescriptionAttribute[]) fi.GetCustomAttributes(typeof (DescriptionAttribute), false);

                    if (attributes != null &&
                        attributes.Length > 0)
                        descripcion = !String.IsNullOrEmpty(descripcion)
                                          ? descripcion + " - " + attributes[0].Description
                                          : attributes[0].Description;
                    else
                        descripcion = !String.IsNullOrEmpty(descripcion)
                                          ? descripcion + " - " + contrato.TipoConfirmacion.ToString()
                                          : contrato.TipoConfirmacion.ToString();
                }

                this.xrtblCellCreditoAprobadoPor.Text = !string.IsNullOrEmpty(contrato.AutorizadorTipoConfirmacion) &&
                                                        !string.IsNullOrWhiteSpace(contrato.AutorizadorTipoConfirmacion)
                                                            ? contrato.AutorizadorTipoConfirmacion.Trim().ToUpper()
                                                            : string.Empty;

                xrtblCellTipoConfirmacion.Text = descripcion;
                
                if (contrato.FormaPago != null)
                {
                    xrchkDeposito.Checked = true;
                    if (lineaTemp.Equipo.ActivoFijo.CostoSinIva == null)
                        xrtblCellDepositoRecibido.Text = "$";
                    else
                    {
                        Decimal? montoDeducibleCalcuado = 0;
                        montoDeducibleCalcuado = lineaTemp.Equipo.ActivoFijo.CostoSinIva;
                        var unidad = (UnidadBO)lineaTemp.Equipo;
                        if(unidad.EquiposAliados.Count > 0)
                        {
                            montoDeducibleCalcuado = unidad.EquiposAliados.Aggregate(montoDeducibleCalcuado, (monto, equipoAliado) => equipoAliado.ActivoFijo != null ? equipoAliado.ActivoFijo.CostoSinIva != null ? monto + equipoAliado.ActivoFijo.CostoSinIva : monto : monto);
                        }
                        //SC0021 formato de decimales
                        xrtblCellDepositoRecibido.Text =
                            contrato.CalcularMontoDeposito((Decimal)montoDeducibleCalcuado) != null
                                ? "$" +
                                  String.Format("{0:#,##0.00##}", contrato.CalcularMontoDeposito((Decimal)montoDeducibleCalcuado))
                                : "$";
                    }
                    
                }
                if (contrato.TipoConfirmacion != null && contrato.TipoConfirmacion == ETipoConfirmacion.ORDEN_DE_COMPRA)
                {
                    xrchkDeposito.Checked = false;
                    xrtblCellDepositoRecibido.Text = "$0.00";
                }
                xrtblCellPreparadoPor.Text = usuarioCreacion.Nombre??String.Empty;
                xrtblCellCompletadoPor.Text = usuarioCierre.Nombre ?? String.Empty;

                #region CU011 – Imprimir Cierre de Contrato de Renta Diaria

                //se obtiene los datos de finalizacion del contrato
                object finalizacion = contrato.ObtenerFinalizacionContratoRD();
                CierreContratoRDBO cierreContrato = null;
                if (finalizacion != null && typeof(CierreContratoRDBO) == finalizacion.GetType() && lineaTemp != null)
                    cierreContrato = (CierreContratoRDBO)finalizacion;

                if (cierreContrato != null) {
                    // se realizan los calculos
                    decimal? importeRenta = contrato.CalcularDiasTranscurridosRenta() * ((TarifaContratoRDBO)lineaTemp.Cobrable).TarifaDiaria;
                    decimal? importeKmAdicional = contrato.CalcularMontoPorKilometrosExcedidos((UnidadBO)lineaTemp.Equipo);
                    decimal? importeHrAdicional = contrato.CalcularMontoPorHorasExcedidas((UnidadBO)lineaTemp.Equipo);
                    decimal? subtotalTarifa = importeRenta + importeKmAdicional + importeHrAdicional;
                    int? litrosUnidad = contrato.CalcularDiferenciaCombustible();
                    decimal? importeLitros = contrato.CalcularMontoPorCombustible(unidadOperativaConfiguracion.PrecioUnidadCombustible);
                    decimal? subtotalCargos = unidadOperativaConfiguracion.PrecioUnidadCombustible.HasValue ? contrato.CalcularSubTotalCargos(unidadOperativaConfiguracion.PrecioUnidadCombustible.Value) : null;
                    decimal? importeSinIva = subtotalTarifa + subtotalCargos;
                    decimal? importeDelIva = (importeSinIva * (contrato.Sucursal.Impuesto != null ? contrato.Sucursal.Impuesto.PorcentajeImpuesto : null)) / 100;
                    decimal? cargoNeto = importeSinIva + importeDelIva;
                    //decimal? montoDeposito = contrato.CalcularMontoDeposito(lineaTemp.Equipo.ActivoFijo.CostoSinIva.Value);
                    decimal? totalPagar = cargoNeto;

                    // se asignan valores a los campos del reporte
                    xrtblCellTarifaDiaria.Text = importeRenta != null ? "$ " + String.Format("{0:#,##0.00##}", importeRenta) : string.Empty;
                    xrtblCellCargoKilometro.Text = importeKmAdicional != null ? "$ " + String.Format("{0:#,##0.00##}", importeKmAdicional) : string.Empty;
                    xrtblCellTarifaHora.Text = importeHrAdicional != null ? "$ " + String.Format("{0:#,##0.00##}", importeHrAdicional) : string.Empty;
                    xrtblCellSubtotalTarifa.Text = subtotalTarifa != null ? "$ " + String.Format("{0:#,##0.00##}", subtotalTarifa) : string.Empty;
                    xrtblCellLitros.Text = litrosUnidad != null ? String.Format("{0:#,##0.00##}", litrosUnidad) : string.Empty;
                    xrtblCellImporteLitros.Text = importeLitros != null ? "$ " + String.Format("{0:#,##0.00##}", importeLitros) : string.Empty;
                    xrtblCellCargoAbuso.Text = cierreContrato.CargoAbusoOperacion != null ? "$ " + String.Format("{0:#,##0.00##}", cierreContrato.CargoAbusoOperacion) : string.Empty;
                    xrtblCellCargoBasura.Text = cierreContrato.CargoDisposicionBasura != null ? "$ " + String.Format("{0:#,##0.00##}", cierreContrato.CargoDisposicionBasura) : string.Empty;
                    xrtblCellSubtotalCargos.Text = subtotalCargos != null ? "$ " + String.Format("{0:#,##0.00##}", subtotalCargos) : string.Empty;
                    xrtblCellPorcentajeIva.Text = contrato.Sucursal.Impuesto.PorcentajeImpuesto.HasValue ? contrato.Sucursal.Impuesto.PorcentajeImpuesto.Value.ToString() + " %" : string.Empty;
                    xrtblCellCargoNeto.Text = cargoNeto != null ? "$ " + String.Format("{0:#,##0.00##}", cargoNeto) : string.Empty;
                    xrtblCellRembolso.Text = cierreContrato.ImporteReembolso != null ? "$ " + String.Format("{0:#,##0.00##}", cierreContrato.ImporteReembolso) : string.Empty;
                    xrtblCellTotalPagar.Text = totalPagar != null ? "$ " + String.Format("{0:#,##0.00##}", totalPagar) : string.Empty;
                    xrtblCellRembolsoRecibidoPor.Text = cierreContrato.PersonaRecibeReembolso;
                    xrtblCellImporteIVA.Text = importeDelIva != null ? "$ " + String.Format("{0:#,##0.00##}", importeDelIva) : string.Empty;
                }
                #endregion
                #endregion
                #region Pie de Reporte
                xrlblLeyendaArrendatarios.Html = leyendaArrendatarios;
                xrlblLeyendaOperacion.Html = leyendaOperacion;
                xrlblLeyendaInspeccion.Html = leyendaInspeccion;
                xrlblNumeroCheckList.Text = contrato.CalcularNumeroListadoVerificacion() ?? String.Empty;
                #endregion
                #endregion
            }
            catch (Exception ex)
            {
                throw new Exception("ContratoRentaDiariaRPT.ImprimirReporte:Error al intentar generar el reporte." + ex.Message);
            }
        }
    }
}
