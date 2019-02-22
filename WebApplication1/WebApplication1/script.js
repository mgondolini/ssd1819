var GAPinstance;
var serie;
var time;

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
        success: function (result) {
            $("#serie").text("Serie: " + result);
            insertSerieGraph(result, serie);
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


function insertSerieGraph(data, serieName) {

    getSerieTime();
    var serie = data.split(',').map(Number);
    var timeSerie = time.split(',').map(Number);

    if ($("#line-chart"))
        $("#line-chart").remove();

    $("#canvas").append('<canvas id="line-chart"></canvas>');

    var canvas = $("#line-chart");
    var context = canvas[0].getContext('2d');

    chart = new Chart($("#line-chart"), {
        type: 'line',
        labels: timeSerie * 10,
        data: {
            datasets: [{
                data: serie,
                label: serieName,
                borderColor: "#3e95cd",
                fill: false
            },
            ]
        },
        options: {
            title: {
                display: true,
                text: serieName
            }
        }
    });

    $('#serie-controls').attr('hidden', false);
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

