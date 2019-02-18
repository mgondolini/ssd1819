﻿var GAPinstance;
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

/*

function postSomething(id, dato) {
    var options = {};
    options.url = "/api/Clienti/postSomething";
    options.type = "POST";
    options.data = "{\"id\":" + id + ",\"dato\":\"" + dato + "\"}";
    options.dataType = "json";
    options.contentType = "application/json";
    options.success = function (msg) {
        alert(msg);
    };
    options.error = function (err) {
        alert(err.statusText);
    };
    $.ajax(options); // chiamata http
}


function readGAPinstance() {
    GAPinstance = $("#gap_name").children("option:selected").val();
    $.ajax({
        url: "api/Clienti/readGAPinstance/" + GAPinstance,
        type: "GET",
        contentType: "application/json",
        data: "",
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
*/