const swalWithBootstrapButtons = Swal.mixin({
    customClass: {
        confirmButton: 'btn btn-success',
        cancelButton: 'btn btn-danger'
    },
    buttonsStyling: false
})

var dataTable;

$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    var id = 0;

    dataTable = $('#DT_load').DataTable({
        "ajax": {
            "url": "wallet/getall",
            "type": "GET",
            "datatype": "json"
        },
        "columns": [
            { "data": "urlP", "width": "20%" },
            { "data": "login", "width": "20%" },
            {
                "data": "passwd",
                "render": function (data) {
                    id = id+1;
                    return `<td><span id="haslo${id}">${data} </span><button id="button${id}"> <i class="fas fa-eye"></i></button>
                            <script>
                                var isClicked${id} = true;
                                var password${id} = $('#haslo${id}').text();
                                myFuncjon${id}();
                                $("#button${id}").click(function () {
                                    myFuncjon${id}();
                                });
                                function myFuncjon${id}() {
                                    isClicked${id} = !isClicked${id};
                                    $('#haslo${id}').text(isClicked${id} ? password${id} : password${id}.replace(/[^]/gi, '*'));
                                }
                            </script>
                            </td>`
                }, "width": "20%"
            },
            {
                "data": "id",
                "render": function (data) { id = data;
                    return `<div class="text-center">
                        <a href="/wallet/detail?id=${id}" class='btn btn-info text-white' style='cursor:pointer; width:75px;'>
                            Więcej
                        </a>
                        &nbsp;
                        <a href="/wallet/upsert?id=${data}" class='btn btn-success text-white' style='cursor:pointer; width:70px;'>
                            Edytuj
                        </a>
                        &nbsp;
                        <a class='btn btn-danger text-white' style='cursor:pointer; width:70px;'
                            onclick=Delete('/wallet/delete?id='+${data})>
                            Usuń
                        </a>
                        </div>`;
                }, "width": "75%"
            }
        ],
        "language": {
            "emptyTable": "Nie znaleziono danych"
        },
        "width": "100%"
    });
}

function Delete(url) {
    swal.fire({
        title: "Czy jesteś pewien?",
        text: "Po usunięciu nie będzie można odzyskać",
        icon: "warning",
        showCancelButton: true,
        confirmButtonColor: '#40be2d',
        cancelButtonColor: '#d33',
        cancelButtonText: 'Nie',
        confirmButtonText: 'Tak',
        reverseButtons: true
    }).then((result) => {
        if (result.value) {
            if (result.value) {
                $.ajax({
                    type: "DELETE",
                    url: url,
                    success: function (data) {
                        if (data.success) {
                            swalWithBootstrapButtons.fire(
                                'Usunięto',
                                'Element został usunięty.',
                                'success'
                            )
                            dataTable.ajax.reload();
                        }
                        else {
                            toastr["error"]("", "Coś poszło nie tak");
                        }
                    }
                });
            }
        } else if (
            /* Read more about handling dismissals below */
            result.dismiss === Swal.DismissReason.cancel
        ) {
            swalWithBootstrapButtons.fire(
                'Anulowano',
                'Twoje dane są bezpieczne :)',
                'error'
            )
        }
    });
}