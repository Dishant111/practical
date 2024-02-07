// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.


function getAllQueryName() {
    return [
        {
            key: "1",
            value: getQueryName("1")
        },
        {
            key: "2",
            value: getQueryName("2")
        },
        {
            key: "3",
            value: getQueryName("3")
        },
    ]
}

function getQueryName(queryId) {

    if (queryId == "1")
        return "General";
    if (queryId == "2")
        return "New Product";
    if (queryId == "3")
        return "Other";
    return "";
}

function getStatusName(statusId) {
    if (statusId == "1")
        return "Pending";
    if (statusId == "2")
        return "Processing";
    if (statusId == "3")
        return "Resolved";
    return "";
}

var padZeroNumber = function (n, length) {
    var str = "" + n;
    if (str.length < length) str = new Array(length - str.length + 1).join("0") + str;
    return str;
}