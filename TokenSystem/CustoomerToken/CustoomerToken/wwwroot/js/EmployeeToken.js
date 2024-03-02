// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

var url = "https://localhost:7107/"
var interval = 0
var pageSize = 10;
var pageNo = 1;
$(document).ready(function () {

        LoadGrid(pageSize, pageNo);
    });

function getPainationUrl(pageSize, pageNo) {
    return `https://localhost:7107/Employee/dynamic/dashBoard?pageSize=${pageSize}&pageNo=${pageNo}`
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
            } else {
                return Promise.reject("unable to get data at moment")
            }
        })
        .then(response => {

            var element = document.getElementById("tokentablebody");
            element.innerHTML = "";

            var innerHtml = ""

            response.data.forEach(x => {
                innerHtml += getTokenRowHtml(x)
            })

            element.innerHTML = innerHtml
            loadPagination(response.pagesize, response.pageNumber, response.totalCount)
        })
        .catch(error => {
            var element = document.getElementById("tokentablebody");
            element.innerHTML = `
            <p>The following is <strong>${error}</strong>.</p>
            `;
            console.log(error)
        });
}


function loadPagination(psize, pno, totoalCount) {
    var element = document.getElementById("tokenPagination");
    var innerHtml = ""
    var totalPages = Math.ceil(totoalCount / psize)
    if (pno > 1) {
        innerHtml += `
                <li class="page-item">
                    <a class="page-link" role="button" onclick="LoadGrid(${psize},${pno - 1})" >&laquo;</a>
                </li>`
    } else {
        innerHtml += `
                <li class="page-item disabled">
                    <a class="page-link" role="button">&laquo;</a>
                </li>`
    }

    for (var i = pno - 2; i < pno - 2 + 5; i++) {
        if (i > 0 && i <= totalPages) {
            if (pno == i) {

                pageSize = psize
                pageNo = pno

                innerHtml += `
                    <li class="page-item active">
                            <a class="page-link" role="button">${i}</a>
                     </li>`
            } else {
                innerHtml += `
                    <li class="page-item">
                            <a class="page-link" role="button" onclick="LoadGrid(${psize},${i})">${i}</a>
                     </li>`
            }
        }
    }

    if (pno + 2 < totalPages) {
        innerHtml += `
                <li class="page-item disabled">
                    <a class="page-link" role="button">...</a>
                </li>`
    }

    if (pno >= totalPages) {
        innerHtml += `
                <li class="page-item disabled">
                    <a class="page-link" role="button">&raquo;</a>
                </li>`
    } else {
        innerHtml += `
                <li class="page-item">
                    <a class="page-link" role="button" onclick="LoadGrid(${psize},${pno + 1})">&raquo;</a>
                </li>`
    }
    element.innerHTML = innerHtml
}

function getTokenRowHtml(data) {
    return `<tr class="${getStatusCssClass(data.status)}">
                    <th scope="row">${padZeroNumber(data.id, 3)}</th>
                    <td>${getQueryName(data.query)}</td>
                    <td>${getStatusName(data.status)}</td>
                    <td>
                        ${(data.status == "2" || data.status == "3" ? '<button type="button" class="btn btn-danger" onclick="setTokeToPending(' + data.id + ')">Change to pending</button> <button type="button" class="btn btn-success" onclick="handleResolvePopup(' + data.id + ')">Resolve</button>' : "")}
                        ${(data.status == "1" ? '<button type="button" class="btn btn-primary" onclick="setTokeToProcess(' + data.id + ')">Change to Processing</button> <button type="button" class="btn btn-success" onclick="handleResolvePopup(' + data.id + ')">Resolve</button>' : "")}
                    </td>
                </tr>`;
}


function getStatusCssClass(status) {
    if (status == "2")
        return "table-warning";
    if (status == "3")
        return "table-success";
    return "table-primary";
}

function setTokeToProcess(id) {
    fetch(`${url}Process/${id}`,
        {
            method: "PUT",
            headers: {
                "Content-Type": "application/json",
        }
        }).then(response => {

            console.log(response.status)

            if (response.ok) {

                LoadGrid(pageSize, pageNo)

                return response.json()

            } else {
                return new Promise.reject("Something went wrong")
            }
        })
}

function setTokeToPending(id) {
    fetch(`${url}Pending/${id}`,
        {
            method: "PUT",
            headers: {
                "Content-Type": "application/json",
        }
        }).then(response => {

            console.log(response.status)

            if (response.ok) {

                LoadGrid(pageSize, pageNo)

                return response.json()

            } else {
                return new Promise.reject("Something went wrong")
            }
        })
}

// data must have address, phoneNumber and id
function resolveToken(data, callback) {

    reqdata = {
        id: data.id
        , address: data.address
        , phoneNumber: data.phoneNumber
        , query: parseInt(data.query)
    }

    fetch(`${url}Resolve`,
        {
            method: "POST",
            headers: {
                "Content-Type": "application/json",
            },
            body: JSON.stringify(reqdata)
        }).then(response => {

            console.log(response.status)

            if (response.ok) {

                callback(response)

            } else {
                return new Promise.reject("Something went wrong")
            }
        })
}

function handleResolvePopup(id) {
    const myModal = new bootstrap.Modal('#resolveModel')
    myModal.show()

    emptyTokenModel()
    getTokenById(id, generateTokenModel)
}

function emptyTokenModel() {
    const myModal = document.getElementById('resolveModelbody');
    myModal.innerHTML = "";
    myModal.innerHTML = `
        <div class="text-center">
            <div class="spinner-border" role="status">
            <span class="visually-hidden">Loading...</span>
            </div>
        </div>`
}

function generateTokenModel(response) {
    const myModal = document.getElementById('resolveModelbody');
    myModal.innerHTML = "";

    const queries = getAllQueryName();
    let options = ""
    queries.forEach((dict, index) => {
        options += `<option value="${dict.key}" ${(response.queryId == dict.key) ? 'selected' : ''}>${dict.value}</option>`
    })

    let innethtml = `
    <form onsubmit="submitResolveModel(event,${response.id})" id="resolveModelForm">
        <div class="form-group">
          <label for="exampleSelect1" class="form-label mt-4">Query Status</label>
          <select class="form-select" id="resolveModelQuery" disabled="">
            ${options}
          </select>
        </div>

        <div class="form-group">
          <label for="exampleInputEmail1" class="form-label mt-4">Address</label>
          <input type="text" class="form-control" id="resolveModelAddress" aria-describedby="emailHelp" placeholder="Address" value="${response.address}">
        </div>

        <div class="form-group">
          <label for="exampleInputEmail1" class="form-label mt-4">Phone number</label>
          <input type="text" class="form-control" id="resolveModelPhone" aria-describedby="emailHelp" placeholder="Phone number" value="${response.phone}">
        </div>
         <div class="modal-footer">
                <button type="submit" class="btn btn-primary" >Resolve</button>
                <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Close</button>
         </div>
    </form>
    `
    myModal.innerHTML = innethtml
}

function getTokenById(id, callback) {
    fetch(`${url}Token/${id}`)
        .then(result => {
            if (result.ok) {
                return result.json()
            } else {
                return Promise.reject("unable to get data at moment")
            }
        })
        .then(
            response => {
                callback(response)
            }
        )

}

function submitResolveModel(e, id) {
    e.preventDefault();
    resolveToken({
        id: id
        , address: document.getElementById("resolveModelAddress")
            .value
        , phoneNumber: document.getElementById("resolveModelPhone")
            .value
        , query: document.getElementById("resolveModelQuery")
            .value
    }
        , HandleSuccessModelSave)

}

function HandleSuccessModelSave(response) {
    debugger
    LoadGrid(pageSize, pageNo);

    const container = document.getElementById("resolveModel");
    const modal = new bootstrap.Modal(container);
    modal.hide();

}
