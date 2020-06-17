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
    let id = 0;
    dataTable = $('#DT_load').DataTable({

        "ajax": {
            "url": "wallet/getall",
            "type": "GET",
            "datatype": "json",
            "deferRender": true
        },
        "columns": [
            { "data": "urlP", "width": "20%" },
            { "data": "login", "width": "20%" },
            {
                "data": "passwd",
                "render": function (data) {                    
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
                                    $('#haslo${id}').text(isClicked${id} ? password${id} : password${id++}.replace(/[^]/gi, '*'));
                                }
                            </script>
                            </td>`
                }, "width": "20%"
            },
            {
                "data": "id",
                "render": function (data) {
                    return `<div class="text-center">
                        <a href="/wallet/detail?id=${data}" class='btn btn-info text-white' style='cursor:pointer; width:75px;'>
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
        // Set rows IDs
        rowId: function (a) {
            return 'id_' + a.id;
        },
        "pagingType": "full_numbers",
        "language": {
            "decimal": ",",
            "thousands": " ",
            "emptyTable": "Nie znaleziono danych",
            "info": "Wyświetlono _START_ do _END_ z _TOTAL_ wierszy",
            "infoEmpty": "Wyświetlono 0 do 0 z 0 wierszy",
            "infoPostFix": "",
            "thousands": ",",
            "lengthMenu": "Wyświetl _MENU_ wierszy",
            "loadingRecords": "Ładowanie...",
            "processing": "Przetwarzanie...",
            "search": "Szukaj:",
            "infoFiltered": "(Przefiltrowano z _MAX_ wszystkich wierszy)",
            "zeroRecords": "Nie znaleziono pasujących rekordów",
            "paginate": {
                "first": `<i class="fas fa-angle-double-left"></i>`,
                "last": `<i class="fas fa-angle-double-right"></i>`,
                "previous": `<i class="fas fa-angle-left"></i>`,
                "next": `<i class="fas fa-angle-right"></i>`
            }
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

Edit = (url, title) => {
    $.ajax({
        type: 'GET',
        url: url,
        success: function (todo) {

            let idD = todo.id
                ? `<input name="id" value="${todo.id}" hidden />`
                : ``;
            let des = todo.description
                ? `${todo.description}`
                : ``;
            let tD = todo.targetDate
                ? `${todo.targetDate}`
                : ``;
            let isD = todo.isDone
                ? `checked="checked" `
                : ``;

            $('#form-modal .modal-body').html(`         
            <form id="editApi" method="post">
                ${idD}
                <div class="form-group row">
                    <div class="col-3">
                        <label>Description</label>
                    </div>
                    <div class="col-6">
                        <input type="text" name="description" id="description" value="${des}" class="form-control"
                               required="required"/>
                    </div>
                </div>
                <div class="form-group row">
                    <div class="col-3">
                        <label>Target Date</label>
                    </div>
                    <div class="col-6">
                        <input type="text" name="targetDate" id="targetDate" value="${tD}" class="form-control"
                               required="required"/>
                    </div>
                </div>
                <div class="form-group row">
                    <div class="col-3">
                        <label>Checked</label>
                    </div>
                    <div class="col-6">
                        <input class="display-3" type="checkbox" name="isDone" id="isDone" value="true" 
                        ${isD}/> <i class="text-success h3 fas fa-clipboard-check"></i>
                    </div>
                </div>
                <div class="form-group row">
                    <div class="col-3 offset-3">
                        <button type="submit" onclick="jQueryAjaxPost()" class="btn btn-primary form-control">
                            Save
                        </button>
                    </div>
                    </div>
                </div>
            </form>
            <script>
                $(function () {
                    $("#targetDate").datetimepicker({
                        format: 'd.m.Y H:i'
                    });
                });
            </script>
        </div>
            `);
            $('#form-modal .modal-title').html(title);
            $('#form-modal').modal('show');
        }
    })
}

jQueryAjaxPost = form => {
    try {
        $("#editApi").submit(function (e) {
            form = $(this);
            console.log(form.serialize());

            $.ajax({
                type: 'POST',
                url: '/wallet/apiupsert',
                data: form.serialize(),
                success: function (todo) {
                    console.log('success')
                    if (todo) {
                        $('#view-all').html(todo)
                        $('#form-modal .modal-body').html('');
                        $('#form-modal .modal-title').html('');
                        $('#form-modal').modal('hide');
                        dataTable.ajax.reload();
                    }
                    else
                        $('#form-modal .modal-body').html(todo.html);
                },
                error: function (err) {
                    console.log('Error Ajax')
                    console.log(err)
                }
            })
            //to prevent default form submit event
            return false;
        })
    } catch (ex) {

        console.log('Exception')
        console.log(ex)
    }
}