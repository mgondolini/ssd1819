var GAPinstance;
var serie;
var time;
var serieResult;

google.charts.load('current', { packages: ['corechart', 'line'] });



$(document).ready(function () {
    $("#gap_name").change(function () {
        $("#solution").text("Solution: ");
        $("#optimization").text("Optimization: ");
        $("#simulatedAnnealing").text("Simulated Annealing: ");
        $("#tabuSearch").text("Tabu Search: ");
    });
});


function constructSolution() {
    GAPinstance = $("#gap_name").children("option:selected").val();
    $.ajax({
        url: "api/Clienti/constructSolution/" + GAPinstance,
        type: "GET",
        contentType: "application/json",
        success: function (result) {
            $("#solution").text("Solution: " + result);
        },
        error: function (xhr, status, p3, p4) {
            var err = "Error " + " " + status + " " + p3;
            if (xhr.responseText && xhr.responseText[0] == "{")
                err = JSON.parse(xhr.responseText).message;
            alert(err);
        }
    });
    event.preventDefault();
}

function optimization() {
    GAPinstance = $("#gap_name").children("option:selected").val();
    $.ajax({
        url: "api/Clienti/optimization/" + GAPinstance,
        type: "GET",
        contentType: "application/json",
        success: function (result) {
            $("#optimization").text("Optimization: " + result);
        },
        error: function (xhr, status, p3, p4) {
            var err = "Error " + " " + status + " " + p3;
            if (xhr.responseText && xhr.responseText[0] == "{")
                err = JSON.parse(xhr.responseText).message;
            alert(err);
        }
    });
    event.preventDefault();
}

function simulatedAnnealing() {
    GAPinstance = $("#gap_name").children("option:selected").val();
    $.ajax({
        url: "api/Clienti/simulatedAnnealing/" + GAPinstance,
        type: "GET",
        contentType: "application/json",
        success: function (result) {
            $("#simulatedAnnealing").text("Simulated Annealing: " + result);
        },
        error: function (xhr, status, p3, p4) {
            var err = "Error " + " " + status + " " + p3;
            if (xhr.responseText && xhr.responseText[0] == "{")
                err = JSON.parse(xhr.responseText).message;
            alert(err);
        }
    });
    event.preventDefault();
}

function tabuSearch() {
    GAPinstance = $("#gap_name").children("option:selected").val();
    $.ajax({
        url: "api/Clienti/tabuSearch/" + GAPinstance,
        type: "GET",
        contentType: "application/json",
        success: function (result) {
            $("#tabuSearch").text("Tabu Search: " + result);
        },
        error: function (xhr, status, p3, p4) {
            var err = "Error " + " " + status + " " + p3;
            if (xhr.responseText && xhr.responseText[0] == "{")
                err = JSON.parse(xhr.responseText).message;
            alert(err);
        }
    });
    event.preventDefault();
}

function readSerie() {
    serie = $("#serie_name").children("option:selected").val();
    $.ajax({
        url: "api/Clienti/readSerie/" + serie,
        type: "GET",
        contentType: "application/json",
        async: false,
        success: function (result) {
            $("#serie").text("Serie: " + result);
            setSerie(result);
            google.charts.setOnLoadCallback(drawChart(result, serie));
        },
        error: function (xhr, status, p3, p4) {
            var err = "Error " + " " + status + " " + p3;
            if (xhr.responseText && xhr.responseText[0] == "{")
                err = JSON.parse(xhr.responseText).message;
            alert(err);
        }
    });
    event.preventDefault();
}

function arimaForecast() {
    serie = $("#serie_name").children("option:selected").val();
    $.ajax({
        url: "api/Clienti/arimaForecast/" + serie,
        type: "GET",
        contentType: "application/json",
        success: function (result) {
            $("#arima_forecast").text("Arima Forecast: " + result);
            addLinesToChart(result, serie, "arima");
        },
        error: function (xhr, status, p3, p4) {
            var err = "Error " + " " + status + " " + p3;
            if (xhr.responseText && xhr.responseText[0] == "{")
                err = JSON.parse(xhr.responseText).message;
            alert(err);
        }
    });
    event.preventDefault();
}

function NNforecast() {
    serie = $("#serie_name").children("option:selected").val();
    $.ajax({
        url: "api/Clienti/NNforecast/" + serie,
        type: "GET",
        contentType: "application/json",
        success: function (result) {
            $("#NNforecast").text("NN Forecast: " + result);
            addLinesToChart(result, serie, "nn");
        },
        error: function (xhr, status, p3, p4) {
            var err = "Error " + " " + status + " " + p3;
            if (xhr.responseText && xhr.responseText[0] == "{")
                err = JSON.parse(xhr.responseText).message;
            alert(err);
        }
    });
    event.preventDefault();
}


function drawChart(data, serieName) {

    getSerieTime();
    var serie = data.split(',').map(Number);

    var time = [];
    for (var i = 0; i < serie.length; i++) {
        time[i] = i;
    }

    var data = new google.visualization.DataTable();
    data.addColumn('number', 'count');
    data.addColumn('number', 'serie');

    for (i = 0; i < time.length; i++)
        data.addRow([time[i], serie[i]]);

    var options = {
        title: serieName,
        curveType: 'function',
        legend: { position: 'bottom' }
    };

    var chart = new google.visualization.LineChart(document.getElementById('canvas'));
    chart.draw(data, options);
}

function addLinesToChart(result, serieName, forecastType) {

    readSerie();

    var serie = serieResult.split(',').map(Number);
    var forecast = result.split('\n').map(Number);

    getSerieTime();

    var time = [];
    for (var i = 0; i < (serie.length + forecast.length); i++) {
        time[i] = i;
    }

    var type;
    if (forecastType == "arima") type = "arima";
    else type = "neural networks";

    var data = new google.visualization.DataTable();
    data.addColumn('number', 'time');
    data.addColumn('number', 'serie');
    data.addColumn('number', type);

    for (i = 0; i < serie.length; i++) 
        data.addRow([time[i], serie[i], null]);       

    var j = 0;
    for (i = serie.length; i < time.length-1; i++) {
        data.addRow([time[i], null, forecast[j]]);   
        j++;
    }


    var options = {
        title: serieName,
        curveType: 'function',
        legend: { position: 'bottom' }
    };

    var chart = new google.visualization.LineChart(document.getElementById('canvas'));
    chart.draw(data, options);
}

function addDotsToChart() {

}


function getSerieTime() {
    $.ajax({
        url: "api/Clienti/getSerieTime",
        type: "GET",
        contentType: "application/json",
        async: false,
        success: function (result) {
            setTime(result);
        },
        error: function (xhr, status, p3, p4) {
            var err = "Error " + " " + status + " " + p3;
            if (xhr.responseText && xhr.responseText[0] == "{")
                err = JSON.parse(xhr.responseText).message;
            alert(err);
        }
    });
}

function setTime(result) {
    time = result;
}

function setSerie(result) {
    serieResult = result;
}

