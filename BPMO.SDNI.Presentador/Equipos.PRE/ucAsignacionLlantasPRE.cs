//Satisface al CU077 - Registrar Acta de Nacimiento de una Unidad
//Satisface al CU079 - Consultar Acta de Nacimiento de Unidad
//Satisface al CU080 – Editar Acta de Nacimiento de una Unidad
using System;
using System.Collections.Generic;
using BPMO.Basicos.BO;
using BPMO.Facade.SDNI.BR;
using BPMO.Patterns.Creational.DataContext;
using BPMO.Primitivos.Enumeradores;
using BPMO.SDNI.Equipos.BO;
using BPMO.SDNI.Equipos.BR;
using BPMO.SDNI.Equipos.VIS;

namespace BPMO.SDNI.Equipos.PRE
{
    public class ucAsignacionLlantasPRE
    {
        #region Atributos
        private IDataContext dctx = null;

        private IucAsignacionLlantasVIS vista;
        private IucLlantaVIS vistaLlantas;
        private IucLlantaVIS vistaRefaccion;

        private ucLlantaPRE presentadorLlantas;
        private ucLlantaPRE presentadorRefaccion;

        private string nombreClase = "ucAsignacionLlantasPRE";
        #endregion

        #region Constructores
        public ucAsignacionLlantasPRE(IucAsignacionLlantasVIS view)
        {
            try
            {
                this.vista = view;
                this.vistaLlantas = this.vista.VistaLlanta;
                this.vistaRefaccion = this.vista.VistaRefaccion;

                this.presentadorLlantas = new ucLlantaPRE(this.vista.VistaLlanta);
                this.presentadorRefaccion = new ucLlantaPRE(this.vista.VistaRefaccion);

                this.dctx = FacadeBR.ObtenerConexion();
            }
            catch (Exception ex)
            {
                this.vista.MostrarMensaje("Inconsistencias en los parámetros de configuración", ETipoMensajeIU.ERROR, this.nombreClase + ".ucAsignacionLlantasPRE:" + ex.Message);
            }
        }
        public ucAsignacionLlantasPRE(IucAsignacionLlantasVIS view, IucLlantaVIS viewLlanta, IucLlantaVIS viewRefaccion)
        {
            try
            {
                this.vista = view;
                this.vistaLlantas = viewLlanta;
                this.vistaRefaccion = viewRefaccion;

                this.presentadorLlantas = new ucLlantaPRE(viewLlanta);
                this.presentadorRefaccion = new ucLlantaPRE(viewRefaccion);

                this.dctx = FacadeBR.ObtenerConexion();
            }
            catch (Exception ex)
            {
                this.vista.MostrarMensaje("Inconsistencias en los parámetros de configuración", ETipoMensajeIU.ERROR, this.nombreClase + ".ucAsignacionLlantasPRE:" + ex.Message);
            }
        }
        #endregion

        #region Métodos
        public void PrepararNuevo()
        {
            this.vista.PrepararNuevo();
            this.PrepararNuevoLlanta();
            this.presentadorRefaccion.PrepararNuevo();

            this.presentadorLlantas.ConfigurarVista(false, false, false, true, false, false, false, true, true, true);
            this.presentadorRefaccion.ConfigurarVista(false, false, false, false, false, false, false, true, true, true);

            this.vista.HabilitarModoEdicion(true);
        }
        public void PrepararEdicion()
        {
            this.PrepararNuevoLlanta();
            this.vista.HabilitarModoEdicion(true);

            this.presentadorLlantas.ConfigurarVista(false, false, false, true, false, false, false, true, true, true);
            this.presentadorRefaccion.ConfigurarVista(false, false, false, false, false, false, false, true, true, true);
        }
        public void PrepararVisualizacion()
        {
            this.vista.HabilitarModoEdicion(false);
            this.presentadorLlantas.PrepararVisualizacion();
            this.presentadorRefaccion.PrepararVisualizacion();

            this.presentadorLlantas.ConfigurarVista(false, false, false, true, false, false, false, false, true, true);
            this.presentadorRefaccion.ConfigurarVista(false, false, false, false, false, false, false, false, true, true);
        }

        private void PrepararNuevoLlanta()
        {
            this.presentadorLlantas.PrepararNuevo();
            this.presentadorLlantas.ConfigurarVista(false, false, false, true, false, false, false, true, true, true);

            this.vista.Stock = true;
            this.vista.Activo = true;
            this.vista.UC = this.vista.UsuarioAutenticado;
            this.vista.UUA = this.vista.UsuarioAutenticado;
            this.vista.FC = DateTime.Now;
            this.vista.FUA = DateTime.Now;
        }

        public void EstablecerConfiguracionInicial(int? usuarioAutenticado)
        {
            this.vista.UsuarioAutenticado = usuarioAutenticado;
        }
        public void EstablecerConfiguracionEspecialVista()
        {
            this.presentadorLlantas.HabilitarPosicion(true);
        }

        public void AgregarLlantas(List<LlantaBO> lst)
        {
            try
            {
                this.vista.Llantas = lst;

                this.vista.ActualizarLlantas();
            }
            catch (Exception ex)
            {
                throw new Exception(this.nombreClase + ".AgregarLlantas: " + ex.Message);
            }
        }
        public void AgregarLlanta()
        {
            string s;
            if ((s = this.ValidarCamposLlanta()) != null)
            {
                this.vista.MostrarMensaje(s, ETipoMensajeIU.INFORMACION, null);
                return;
            }

            #region SC_0027
            if ((s = this.VerificarExistenciaCodigo(false)) != null)
            {
                this.vista.MostrarMensaje(s, ETipoMensajeIU.CONFIRMACION, "AGREGARLLANTA");
                return;
            }
            #endregion

            try
            {
                List<LlantaBO> llantas = this.vista.Llantas;

                LlantaBO bo = new LlantaBO();
                bo.Auditoria = new AuditoriaBO();
                bo.MontadoEn = new EnllantableProxyBO();

                bo.LlantaID = this.vista.LlantaID;
                bo.Codigo = this.vista.Codigo;
                bo.EsRefaccion = false;
                bo.Activo = this.vista.Activo;
                bo.Marca = this.vista.Marca;
                bo.Medida = this.vista.Medida;
                bo.Modelo = this.vista.Modelo;
                bo.Posicion = this.vista.Posicion;
                bo.Profundidad = this.vista.Profundidad;
                bo.Revitalizada = this.vista.Revitalizada;
                bo.Stock = false;
                bo.Auditoria.FC = this.vista.FC;
                bo.Auditoria.UC = this.vista.UC;
                //Si tiene ID quiere decir que existe y va a ser actualizado, así que es necesario sobre-escribir los valores de actualización
                //Si no, es nuevo y ya tiene asignado los valores correctos
                if (bo.LlantaID != null)
                {
                    bo.Auditoria.FUA = DateTime.Now;
                    bo.Auditoria.UUA = this.vista.UsuarioAutenticado;
                }
                else
                {
                    bo.Auditoria.FUA = this.vista.FUA;
                    bo.Auditoria.UUA = this.vista.UUA;
                }
                ((EnllantableProxyBO)bo.MontadoEn).EnllantableID = this.vista.EnllantableID;
                if (this.vista.TipoEnllantable != null)
                    ((EnllantableProxyBO)bo.MontadoEn).TipoEnllantable = (ETipoEnllantable)Enum.Parse(typeof(ETipoEnllantable), this.vista.TipoEnllantable.ToString());

                if (this.vista.Llantas == null)
                    this.vista.Llantas = new List<LlantaBO>();

                llantas.Add(bo);

                this.AgregarLlantas(llantas);

                this.PrepararNuevoLlanta();
            }
            catch (Exception ex)
            {
                throw new Exception(this.nombreClase + ".AgregarLlanta: " + ex.Message);
            }
        }

        public void QuitarLlanta(int index)
        {
            try
            {
                if (index >= this.vista.Llantas.Count || index < 0)
                    throw new Exception("No se encontró la llanta seleccionada");

                List<LlantaBO> llantas = this.vista.Llantas;
                llantas.RemoveAt(index);

                this.vista.Llantas = llantas;
                this.vista.ActualizarLlantas();
            }
            catch (Exception ex)
            {
                throw new Exception(this.nombreClase + ".QuitarLlanta: " + ex.Message);
            }
        }

        private string ValidarCamposLlanta()
        {
            string s = "";

            if ((s = this.presentadorLlantas.ValidarCamposRegistro()) != null)
                return "Llanta:" + s;

            s = "";

            if (this.vista.Posicion == null)
                s += "Posición, ";

            if (this.vista.LlantaID != null)
            {
                if (this.vista.FC == null)
                    s += "Fecha de Creación, ";
                if (this.vista.FUA == null)
                    s += "Fecha de Última Actualización, ";
                if (this.vista.UC == null)
                    s += "Usuario de Creación, ";
                if (this.vista.UUA == null)
                    s += "Usuario de Última Actualización, ";
            }

            if (s.Trim().CompareTo("") != 0)
                return "Los siguientes campos no pueden estar vacíos: \n" + s.Substring(0, s.Length - 2);

            if (this.vista.Posicion != null && (this.vista.Posicion <= 0 || this.vista.Posicion >= 11))
                return "La posición debe ser del 1 a 10";
            if (this.vista.LlantaID != null && this.vista.Stock != null && this.vista.Stock == false)
                return "La llanta seleccionada ya se encuentra asignada (no se encuentra en Stock)";
            if (this.vista.LlantaID != null && this.vista.Activo != null && this.vista.Activo == false)
                return "La llanta seleccionada ya fue dada de baja y no se puede asignar";

            if (this.vista.Llantas != null)
            {
                if (this.vista.Llantas.Count >= 10)
                    return "Ya se asignó la cantidad máxima de llantas (10)";
                if (this.vista.Llantas.Exists(p => p.Posicion != null && p.Posicion == this.vista.Posicion))
                    return "Ya se encuentra asignada una llanta en esa posición";
            }

            if (this.vista.RefaccionID != null && this.vista.LlantaID != null && this.vista.RefaccionID == this.vista.LlantaID)
                return "La llanta ya se encuentra asignada como refacción";
            if (this.vista.Codigo != null && this.vista.RefaccionCodigo != null && this.vista.Codigo.Trim() == this.vista.RefaccionCodigo.Trim())
                return "Ya se encuentra asignada una refacción con el mismo código de llanta";

            if (this.vista.Llantas != null && this.vista.Llantas.Exists(p => p.Codigo != null && this.vista.Codigo != null && p.Codigo == this.vista.Codigo))
                return "Ya existe una llanta asignada con ese código";

            if (this.vista.SucursalEnllantableID.HasValue && this.vista.SucursalEnllantableID != this.vista.SucursalID)
                return "La llanta debe pertenecer a la sucursal de la unidad.";

            return null;
        }
        private string ValidarCamposRefaccion()
        {
            string s = "";

            if (this.vista.RefaccionCodigo != null && this.vista.RefaccionCodigo.Trim().CompareTo("") != 0 && (s = this.presentadorRefaccion.ValidarCamposRegistro()) != null)
                return "Refacción:" + s;

            s = "";

            if (this.vista.RefaccionID != null)
            {
                if (this.vista.RefaccionFC == null)
                    s += "Fecha de Creación (Refacción), ";
                if (this.vista.RefaccionFUA == null)
                    s += "Fecha de Última Actualización (Refacción), ";
                if (this.vista.RefaccionUC == null)
                    s += "Usuario de Creación (Refacción), ";
                if (this.vista.RefaccionUUA == null)
                    s += "Usuario de Última Actualización (Refacción), ";
            }

            if (s.Trim().CompareTo("") != 0)
                return "Los siguientes campos no pueden estar vacíos: \n" + s.Substring(0, s.Length - 2);

            if (this.vista.RefaccionID != null && this.vista.RefaccionActivo != null && this.vista.RefaccionActivo == false)
                return "La refacción seleccionada ya fue dada de baja y no se puede asignar";

            if (this.vista.Llantas != null)
            {
                if (this.vista.RefaccionCodigo != null && this.vista.Llantas.Exists(p => p.Codigo != null && p.Codigo.Trim() == this.vista.RefaccionCodigo.Trim()))
                    return "Ya se encuentra asignada una llanta con el mismo código de refacción";
                if (this.vista.RefaccionID != null && this.vista.Llantas.Exists(p => p.LlantaID != null && p.LlantaID == this.vista.RefaccionID))
                    return "La refacción ya se encuentra asignada como llanta";
            }

            //Si la llanta es nueva, se valida en la BD que el código no se repita
            if (this.vista.RefaccionID == null && !string.IsNullOrEmpty(this.vista.RefaccionCodigo))
            {
                List<LlantaBO> lstTemp = new LlantaBR().Consultar(this.dctx, new LlantaBO() { Codigo = this.vista.RefaccionCodigo });
                if (lstTemp.Count > 0)
                    return "El código que proporcionó para la refacción ya se encuentra registrado";
            }

            return null;
        }

        public string ValidarCamposBorrador()
        {
            string s = null;

            if ((s = this.ValidarCamposRefaccion()) != null)
                return "Refacción:" + s;

            if (!(this.vista.Llantas != null && this.vista.Llantas.Count <= 10))
                return "La unidad no puede tener más de 10 llantas asignadas";

            foreach (LlantaBO llanta in this.vista.Llantas)
            {
                if (llanta.LlantaID == null && !string.IsNullOrEmpty(llanta.Codigo))
                {
                    List<LlantaBO> lstTemp = new LlantaBR().Consultar(this.dctx, new LlantaBO() { Codigo = llanta.Codigo });
                    if (lstTemp.Count > 0)
                        return "La llanta con código " + llanta.Codigo + " ya se encuentra registrada";
                }
            }

            return null;
        }
        public string ValidarCamposRegistro()
        {
            string s = null;

            if ((s = this.ValidarCamposRefaccion()) != null)
                return "Refacción:" + s;

            if (!(this.vista.Llantas != null && this.vista.Llantas.Count >= 4))
                return "La unidad debe tener mínimo 4 llantas asignadas";
            if (!(this.vista.Llantas != null && this.vista.Llantas.Count <= 10))
                return "La unidad no puede tener más de 10 llantas asignadas";

            foreach (LlantaBO llanta in this.vista.Llantas)
            {
                if (llanta.LlantaID == null && !string.IsNullOrEmpty(llanta.Codigo))
                {
                    List<LlantaBO> lstTemp = new LlantaBR().Consultar(this.dctx, new LlantaBO() { Codigo = llanta.Codigo });
                    if (lstTemp.Count > 0)
                        return "Ya existe una llanta registrada con el " + llanta.Codigo;
                }
            }

            return null;
        }

        #region SC_0027
        public string VerificarExistenciaCodigo(bool esRefaccion)
        {
            string codigoNuevo;
            string codigoActual = esRefaccion ? this.vista.RefaccionCodigo : this.vista.Codigo;
            ucLlantaPRE preLlanta = esRefaccion ? this.presentadorRefaccion : this.presentadorLlantas;

            if (preLlanta.VerificarExistenciaCodigo(out codigoNuevo))
            {
                if (esRefaccion) this.vista.RefaccionCodigo = codigoNuevo;
                else this.vista.Codigo = codigoNuevo;

                return "El código " + codigoActual + " ya se encuentra registrado y se ha generado el código " + codigoNuevo;
            }

            return null;
        }
        #endregion
        public void LimpiarSesion()
        {
            this.vista.LimpiarSesion();
            this.presentadorLlantas.LimpiarSesion();
            this.presentadorRefaccion.LimpiarSesion();
        }
        #endregion
    }
}
