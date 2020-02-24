Sys.WebForms.PageRequestManager.getInstance().add_beginRequest(BeginRequestHandler);
Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);

/*Inicio de Request*/
function BeginRequestHandler(sender, args) {
    $.blockUI({ 
        fadeIn: 1000, 
        message: '<img src="../Contenido/Imagenes/Cargando.gif" />  Espere por favor...',
        css: {
            border: 'none',
            padding: '15px',
            backgroundColor: '#000',
            '-webkit-border-radius': '10px',
            '-moz-border-radius': '10px',
            opacity: .5,
            color: '#fff'
        }
    }); 
}
/*Fin de Request*/
function EndRequestHandler(sender, args) {
    $.unblockUI();
    if(typeof InitControls === "function")
        InitControls();
}