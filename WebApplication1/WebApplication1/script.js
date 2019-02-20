var GAPinstance;
var serie;

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
