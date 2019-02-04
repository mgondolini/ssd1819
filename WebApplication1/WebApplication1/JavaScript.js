
function init() {
    alert('init')
}

function findAll() {
    $.ajax({
        url: "api/Clienti/GetAllClients",
        type: "GET",
        contentType: "application/json",
        success: function (result) {
            readResult(result);
        },
        error: function (xhr, status, p3, p4) {
            var err = "Error " + " " + status + " " + p3;
            if (xhr.responseText && xhr.responseText[0] == "{")
                err = JSON.parse(xhr.responseText).message;
            alert(err);
        }
    });
}

function findById() {
    var id = $('#txtId').val();
    $.ajax({
        url: "api/Clienti/GetCustQuantities/" + id, //api/nomeclasse/nomemetodo
        type: "GET",
        contentType: "application/json",
        data: "",
        success: function (result) {
            readResult(result);
        },
        error: function (xhr, status, p3, p4) {
            var err = "Error " + " " + status + " " + p3;
            if (xhr.responseText && xhr.responseText[0] == "{")
                err = JSON.parse(xhr.responseText).message;
            alert(err);
        }
    });
}

function AjaxHelper(baseUrl) {
    this._baseUrl = baseUrl;
    var callWebAPI = function (url, verb, data, callback) {
        var xhr = new XMLHttpRequest();
        xhr.onload = function (evt) {
            var data = JSON.parse(evt.target.responseText);
            callback(data);
        }
        xhr.onerror = function () {
            alert("Error while calling Web API");
        }
        xhr.open(verb, url);
        xhr.setRequestHeader("Content-Type", "application/json");
        if (data == null) xhr.send();
        else xhr.send(JSON.stringify(data));
    }
    this.getItem = function (id, callback) {
        callWebAPI(this._baseUrl + "/" + id, "GET", null, callback);
    }
}

// °°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°° chiamata xmlhttprequest
function getInstance() {
    var ajaxHelper = new AjaxHelper("/api/Clienti");
    var getItemCallback = function (res) {
        alert(res);
        console.log(res);
    }
    var actionCallback = function (msg) {
        alert(msg);
    }
    //GET
    let id = $('#txtId').val();
    console.log(id);
    ajaxHelper.getItem("getItem?id=" + id, getItemCallback);
}


function readResult(elem) {
    alert(elem)
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


function readGAPinstance() {
    $.ajax({
        url: "api/Clienti/readGAPinstance",
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
    $.ajax({
        url: "api/Clienti/constructSolution",
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

