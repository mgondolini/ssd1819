﻿var GAPinstance;
var serie;

function readGAPinstance() {
    GAPinstance = $("#gap_name").children("option:selected").val();
    $.ajax({
        url: "api/Clienti/readGAPinstance/" + GAPinstance,
        type: "GET",
        contentType: "application/json",
        data: "",
        success: function (result) {
            alert(result)
        },
        error: function (xhr, status, p3, p4) {
            var err = "Error " + " " + status + " " + p3;
            if (xhr.responseText && xhr.responseText[0] == "{")
                err = JSON.parse(xhr.responseText).message;
            alert(err);
        }
    });
}

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




function readSerie() {
    $.ajax({
        url: "api/Clienti/readSerie",
        type: "GET",
        contentType: "application/json",
        success: function (result) {
            alert(result)
        },
        error: function (xhr, status, p3, p4) {
            var err = "Error " + " " + status + " " + p3;
            if (xhr.responseText && xhr.responseText[0] == "{")
                err = JSON.parse(xhr.responseText).message;
            alert(err);
        }
    });
}

function constructSolution() {
    GAPinstance = $("#gap_name").children("option:selected").val();
    $.ajax({
        url: "api/Clienti/constructSolution" + GAPinstance ,
        type: "GET",
        contentType: "application/json",
        data: "",
        success: function (result) {
            alert(result)
        },
        error: function (xhr, status, p3, p4) {
            var err = "Error " + " " + status + " " + p3;
            if (xhr.responseText && xhr.responseText[0] == "{")
                err = JSON.parse(xhr.responseText).message;
            alert(err);
        }
    });
}

function optimization() {
    $.ajax({
        url: "api/Clienti/optimization",
        type: "GET",
        contentType: "application/json",
        success: function (result) {
            alert(result)
        },
        error: function (xhr, status, p3, p4) {
            var err = "Error " + " " + status + " " + p3;
            if (xhr.responseText && xhr.responseText[0] == "{")
                err = JSON.parse(xhr.responseText).message;
            alert(err);
        }
    });
}

function simulatedAnnealing() {
    $.ajax({
        url: "api/Clienti/simulatedAnnealing",
        type: "GET",
        contentType: "application/json",
        success: function (result) {
            alert(result)
        },
        error: function (xhr, status, p3, p4) {
            var err = "Error " + " " + status + " " + p3;
            if (xhr.responseText && xhr.responseText[0] == "{")
                err = JSON.parse(xhr.responseText).message;
            alert(err);
        }
    });
}

