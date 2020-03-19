// tabla Documentos //
$(document).ready(function() {
    var dataTable_documentos_legales = $("#dataTable-documentos-legales").DataTable( {      
         //"searching": false,
         "scrollX": true,
         "scrollCollapse": true,
         "autoWidth": true, 
         "paging": true, 
         "info": false,         
         "language": {
      "url": "~/assets/plugins/datatables/json/Spanish.json"
    	}
    });

    if ($("#search-button").length !== 0) {
        $('#search-button').on('click', function () {
            dataTable_documentos_legales.column(0).search($("#search-input").val()).draw();
        });
    }
} );

// tabla actas reuniones //
$(document).ready(function() {
    $("#dataTable-actas-reuniones").DataTable( {      
         "searching": false,
         "scrollX": true,
         "scrollCollapse": true,
         "autoWidth": true, 
         "paging": true, 
         "info": false,         
         "language": {
      "url": "../assets/plugins/datatables/json/Spanish.json"
        }
    } );
} );

// tabla Directorio de Suplidores //
$(document).ready(function() {
    var dataTable_directorio_suplidores_table = $("#dataTable-directorio-suplidores").DataTable( {      
         //"searching": false,
         "scrollX": true,
         "scrollCollapse": true,
         "autoWidth": true, 
         "paging": true, 
         "info": false,           
         "language": {
      "url": "../assets/plugins/datatables/json/Spanish.json"
    	}
    });

    if ($("#search-button").length !== 0) {
        $('#search-button').on('click', function () {
            dataTable_directorio_suplidores_table.column(0).search($("#search-input").val()).draw();
        });
    }
} );

// tabla Ver Suplidor //
$(document).ready(function() {
    $("#dataTable-ver-suplidor").DataTable( {      
         "searching": false,
         "scrollX": true,
         "scrollCollapse": true,
         "autoWidth": true, 
         "paging": true, 
         "info": false,         
         "lengthChange":true,    
         "language": {
      "url": "../assets/plugins/datatables/json/Spanish.json"
    	}
    } );
} );

// tabla Listado Tareas //
$(document).ready(function() {
    var dataTable_listado_tareas = $("#dataTable-listado-tareas").DataTable( {      
         //"searching": false,
         "scrollX": true,
         "scrollCollapse": true,
         "autoWidth": true, 
         "paging": true, 
         "info": false,         
         "lengthChange":true,
         "language": {
      "url": "../assets/plugins/datatables/json/Spanish.json"
    	}
    });

    if ($("#search-button").length !== 0) {
        $('#search-button').on('click', function () {
            dataTable_listado_tareas.column(0).search($("#search-input").val()).draw();
        });
    }
} );

// tabla Tareas completadas //
$(document).ready(function() {
    var dataTable_tareas_completadas = $("#dataTable-tareas-completadas").DataTable( {      
         //"searching": false,
         "scrollX": true,
         "scrollCollapse": true,
         "autoWidth": true, 
         "paging": true, 
         "info": false,         
         "lengthChange":true,
         "language": {
      "url": "../assets/plugins/datatables/json/Spanish.json"
    	}
    });

    if ($("#search-button").length !== 0) {
        $('#search-button').on('click', function () {
            dataTable_tareas_completadas.column(0).search($("#search-input").val()).draw();
        });
    }
});

// tabla Calendario de Eventos registrados //
$(document).ready(function() {
    $("#dataTable-eventos-registrados").DataTable( {      
         "searching": false,
         "scrollX": true,
         "scrollCollapse": true,
         "autoWidth": true, 
         "paging": true, 
         "info": false,         
         "lengthChange":true,
         "language": {
      "url": "../assets/plugins/datatables/json/Spanish.json"
    	}
    } );
} );

// table de prueba //
$(document).ready(function() {
    var table = $('#example').DataTable( {       
        scrollX:        true,
        scrollCollapse: true,
        autoWidth:         true,  
         paging:         true,       
        columnDefs: [
        { "width": "50px", "targets": [0,1,2] },       
        { "width": "30px", "targets": [4] }
      ]
    } );
} );