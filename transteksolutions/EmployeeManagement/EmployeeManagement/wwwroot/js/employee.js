$(document).ready(function () {
    LoadGrid();
});

function LoadGrid() {
    fetch("/Employee/List")
        .then(response => {
            if (!response.ok) {
                throw new Error(`Error: ${response.status}`);
            }
            return response.json();
        }
        ).then(data => {
            let table = $("#table");
            table.empty();

            $.each(data,
                function(index) {

                    let html = `<tr>
                <td>
                ${index}
                </td>
                <td>
                ${this.employeeName}
                </td>
                <td>
                ${this.salary}
                </td>
                <td>
                ${this.designation}
                </td>
                    <td>
                    <a class="btn btn-danger" onclick="Delete(${this.employeeId})">Delete</a> |
                    <a class="btn btn-warning" href="/Employee/Edit/${this.employeeId}">Edit</a> |
                    </td>
                </tr>`;
                    table.append(html);
                }
            );
        })
        .catch(error => console.error('Error:', error));
}

function Delete(id){

    fetch("/Employee/Delete/"+id,
        {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',            },
        })
        .then(response => {
            if (!response.ok) {
                throw new Error(`Error: ${response.status}`);
            }
            LoadGrid();
        })
}
