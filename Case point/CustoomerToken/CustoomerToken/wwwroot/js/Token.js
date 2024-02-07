// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
var url = "https://localhost:7107/"
var interval = 0
$(document).ready(function () {
    var pageSize = 10;
    var pageNo = 1;

    LoadGrid(pageSize, pageNo);
});
function getPainationUrl(pageSize, pageNo) {
    return `https://localhost:7107/dynamic/dashBoard?pageSize=${pageSize}&pageNo=${pageNo}`
}

function LoadGrid(pageSize, pageNo) {
    if (pageNo == 1) {
        if (interval == 0) interval = setInterval(() => LoadGrid(pageSize, pageNo), 4000);
    } else {
        clearInterval(interval)
        interval = 0
    }

    fetch(`${getPainationUrl(pageSize, pageNo)}`)
        .then(result => {
            if (result.ok) {
                return result.json()
            } 
            else {
                return Promise.reject("unable to get data at moment")
            }
        })
        .then(response => {

            console.log(response);

            var element = document.getElementById("tokentablebody");
            element.innerHTML = "";

            var innerHtml = ""

            response.data.forEach(x => {
                innerHtml += getTokenRowHtml(x)
            })

            element.innerHTML = innerHtml
            loadPagination(response.pagesize, response.pageNumber, response.totalCount)
        }).catch(error => {
            var element = document.getElementById("tokentablebody");
            element.innerHTML = `
            <p>The following is <strong>${error}</strong>.</p>
            `;
            console.log(error)
        });
}


function loadPagination(pageSize, pageNo, totoalCount) {
    var element = document.getElementById("tokenPagination");
    var innerHtml = ""
    var totalPages = Math.ceil(totoalCount / pageSize)
    if (pageNo > 1) {
        innerHtml += `
                <li class="page-item">
                    <a class="page-link" role="button" onclick="LoadGrid(${pageSize},${pageNo - 1})" >&laquo;</a>
                </li>`
    } else {
        innerHtml += `
                <li class="page-item disabled">
                    <a class="page-link" role="button">&laquo;</a>
                </li>`
    }

    for (var i = pageNo - 2; i < pageNo - 2 + 5; i++) {
        if (i > 0 && i <= totalPages) {
            if (pageNo == i) {
                innerHtml += `
                    <li class="page-item active">
                            <a class="page-link" role="button">${i}</a>
                     </li>`
            } else {
                innerHtml += `
                    <li class="page-item">
                            <a class="page-link" role="button" onclick="LoadGrid(${pageSize},${i})">${i}</a>
                     </li>`
            }
        }
    }

    if (pageNo + 2 < totalPages) {
        innerHtml += `
                <li class="page-item disabled">
                    <a class="page-link" role="button">...</a>
                </li>`
    }

    if (pageNo >= totalPages) {
        innerHtml += `
                <li class="page-item disabled">
                    <a class="page-link" role="button">&raquo;</a>
                </li>`
    } else {
        innerHtml += `
                <li class="page-item">
                    <a class="page-link" role="button" onclick="LoadGrid(${pageSize},${pageNo + 1})">&raquo;</a>
                </li>`
    }
    element.innerHTML = innerHtml
}

function getTokenRowHtml(data) {
    return `<tr class="${getStatusCssClass(data.status)}">
                    <th scope="row">${padZeroNumber(data.id, 3)}</th>
                    <td>${getQueryName(data.query)}</td>
                    <td>${getStatusName(data.status)}</td>
                </tr>`;
}

function getStatusCssClass(status) {
    if (status == "2")
        return "table-warning";
    if (status == "3")
        return "table-success";
    return "table-primary";
}
