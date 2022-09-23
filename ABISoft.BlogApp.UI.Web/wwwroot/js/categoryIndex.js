$(document).ready(function () {
    const table = $('#categoryTable').DataTable({
        "bStateSave": true,

        dom:
            "<'row'<'col-sm-3'l><'col-sm-6 text-center'B><'col-sm-3'f>>" +
            "<'row'<'col-sm-12'tr>>" +
            "<'row'<'col-sm-5'i><'col-sm-7'p>>",

        buttons: [
            {
                text: 'Ekle',
                attr: {
                    id: 'btnAdd'
                },
                className: 'btn btn-success',
                action: function (e, dt, node, config) {

                }
            },
            {
                text: 'Yenile',
                className: 'btn btn-warning',
                action: function (e, dt, node, config) {
                    //When 'Yenile' is clicked this part will work...
                    $.ajax({
                        url: '/Admin/Category/GetAllCategories',
                        type: 'Get',
                        contentType: 'application/json',
                        beforeSend: function () {
                            $('#categoryTable').hide();
                            $('.card-body .spinner-border').show();
                        },
                        success: function (data) {
                            let categoryListDto = jQuery.parseJSON(data);
                            if (categoryListDto.ResultStatus === 0) {
                                let newTableBody = '';
                                let categoryList = categoryListDto.Categories.$values;
                                $.each(categoryList, function (index, category) {
                                    newTableBody +=
                                        `<tr name="row_${category.Id}">
                                                    <td>${category.Id}</td>
                                                    <td>${category.Name}</td>
                                                    <td>${category.Description}</td>
                                                    <td>${convertFirstLetterToUpperCase(category.IsActive.toString())}</td>
                                                    <td>${convertFirstLetterToUpperCase(category.IsDeleted.toString())}</td>
                                                    <td>${category.Note}</td>
                                                    <td>${convertToShortDate(category.CreatedDate)}</td>
                                                    <td>${category.CreatedByName}</td>
                                                    <td>${convertToShortDate(category.ModifiedDate)}</td>
                                                    <td>${category.ModifiedByName}</td>
                                                    <td>
                                                        <button class="btn btn-primary btn-update btn-sm" data-id="${category.Id}"><span class="fas fa-edit"></span></button>
                                                        <button class="btn btn-danger btn-delete btn-sm" data-id="${category.Id}"><span class="fas fa-minus-circle"></span></button>
                                                    </td>
                                                 </tr>`;
                                });
                                const newTableBodyObject = $(newTableBody);
                                //console.log(newTableBodyObject);
                                $('#categoryTable').DataTable().clear();
                                $('#categoryTable').DataTable().rows.add(newTableBodyObject).draw(false);
                                $('.card-body .spinner-border').hide();
                                $('#categoryTable').fadeIn(1500);     
                                $('#categoryTable').DataTable().draw(false);                                    
                            }
                            else {
                                toastr.error(`${categoryListDto.Message}`, 'İşlem Başarısız!')
                            }
                        },
                        error: function (error) {
                            $('.card-body .spinner-border').hide();
                            $('#categoryTable').fadeIn(1500);
                            toastr.error(`${error.responseText}`, 'İşlem Başarısız!'); 
                        }
                    });
                }
            }
        ],

        language: {
            "emptyTable": "Tabloda herhangi bir veri mevcut değil",
            "info": "_TOTAL_ kayıttan _START_ - _END_ arasındaki kayıtlar gösteriliyor",
            "infoEmpty": "Kayıt yok",
            "infoFiltered": "(_MAX_ kayıt içerisinden bulunan)",
            "infoThousands": ".",
            "lengthMenu": "Sayfada _MENU_ kayıt göster",
            "loadingRecords": "Yükleniyor...",
            "processing": "İşleniyor...",
            "search": "Ara:",
            "zeroRecords": "Eşleşen kayıt bulunamadı",
            "paginate": {
                "first": "İlk",
                "last": "Son",
                "next": "Sonraki",
                "previous": "Önceki"
            },
            "aria": {
                "sortAscending": ": artan sütun sıralamasını aktifleştir",
                "sortDescending": ": azalan sütun sıralamasını aktifleştir"
            },
            "select": {
                "rows": {
                    "_": "%d kayıt seçildi",
                    "1": "1 kayıt seçildi"
                },
                "cells": {
                    "1": "1 hücre seçildi",
                    "_": "%d hücre seçildi"
                },
                "columns": {
                    "1": "1 sütun seçildi",
                    "_": "%d sütun seçildi"
                }
            },
            "autoFill": {
                "cancel": "İptal",
                "fillHorizontal": "Hücreleri yatay olarak doldur",
                "fillVertical": "Hücreleri dikey olarak doldur",
                "fill": "Bütün hücreleri <i>%d<\/i> ile doldur"
            },
            "buttons": {
                "collection": "Koleksiyon <span class=\"ui-button-icon-primary ui-icon ui-icon-triangle-1-s\"><\/span>",
                "colvis": "Sütun görünürlüğü",
                "colvisRestore": "Görünürlüğü eski haline getir",
                "copySuccess": {
                    "1": "1 satır panoya kopyalandı",
                    "_": "%ds satır panoya kopyalandı"
                },
                "copyTitle": "Panoya kopyala",
                "csv": "CSV",
                "excel": "Excel",
                "pageLength": {
                    "-1": "Bütün satırları göster",
                    "_": "%d satır göster"
                },
                "pdf": "PDF",
                "print": "Yazdır",
                "copy": "Kopyala",
                "copyKeys": "Tablodaki veriyi kopyalamak için CTRL veya u2318 + C tuşlarına basınız. İptal etmek için bu mesaja tıklayın veya escape tuşuna basın.",
                "createState": "Şuanki Görünümü Kaydet",
                "removeAllStates": "Tüm Görünümleri Sil",
                "removeState": "Aktif Görünümü Sil",
                "renameState": "Aktif Görünümün Adını Değiştir",
                "savedStates": "Kaydedilmiş Görünümler",
                "stateRestore": "Görünüm -&gt; %d",
                "updateState": "Aktif Görünümün Güncelle"
            },
            "searchBuilder": {
                "add": "Koşul Ekle",
                "button": {
                    "0": "Arama Oluşturucu",
                    "_": "Arama Oluşturucu (%d)"
                },
                "condition": "Koşul",
                "conditions": {
                    "date": {
                        "after": "Sonra",
                        "before": "Önce",
                        "between": "Arasında",
                        "empty": "Boş",
                        "equals": "Eşittir",
                        "not": "Değildir",
                        "notBetween": "Dışında",
                        "notEmpty": "Dolu"
                    },
                    "number": {
                        "between": "Arasında",
                        "empty": "Boş",
                        "equals": "Eşittir",
                        "gt": "Büyüktür",
                        "gte": "Büyük eşittir",
                        "lt": "Küçüktür",
                        "lte": "Küçük eşittir",
                        "not": "Değildir",
                        "notBetween": "Dışında",
                        "notEmpty": "Dolu"
                    },
                    "string": {
                        "contains": "İçerir",
                        "empty": "Boş",
                        "endsWith": "İle biter",
                        "equals": "Eşittir",
                        "not": "Değildir",
                        "notEmpty": "Dolu",
                        "startsWith": "İle başlar",
                        "notContains": "İçermeyen",
                        "notStarts": "Başlamayan",
                        "notEnds": "Bitmeyen"
                    },
                    "array": {
                        "contains": "İçerir",
                        "empty": "Boş",
                        "equals": "Eşittir",
                        "not": "Değildir",
                        "notEmpty": "Dolu",
                        "without": "Hariç"
                    }
                },
                "data": "Veri",
                "deleteTitle": "Filtreleme kuralını silin",
                "leftTitle": "Kriteri dışarı çıkart",
                "logicAnd": "ve",
                "logicOr": "veya",
                "rightTitle": "Kriteri içeri al",
                "title": {
                    "0": "Arama Oluşturucu",
                    "_": "Arama Oluşturucu (%d)"
                },
                "value": "Değer",
                "clearAll": "Filtreleri Temizle"
            },
            "searchPanes": {
                "clearMessage": "Hepsini Temizle",
                "collapse": {
                    "0": "Arama Bölmesi",
                    "_": "Arama Bölmesi (%d)"
                },
                "count": "{total}",
                "countFiltered": "{shown}\/{total}",
                "emptyPanes": "Arama Bölmesi yok",
                "loadMessage": "Arama Bölmeleri yükleniyor ...",
                "title": "Etkin filtreler - %d",
                "showMessage": "Tümünü Göster",
                "collapseMessage": "Tümünü Gizle"
            },
            "ResuSuccessuccessuccessltStatusSSuccesstatusltStatushousands": ".",
            "datetime": {
                "amPm": [
                    "öö",
                    "ös"
                ],
                "hours": "Saat",
                "minutes": "Dakika",
                "next": "Sonraki",
                "previous": "Önceki",
                "seconds": "Saniye",
                "unknown": "BilinmeUrlen",
                "weekdays": {
                    "6": "Paz",
                    "5": "Cmt",
                    "4": "Cum",
                    "3": "Per",
                    "2": "Çar",
                    "1": "Sal",
                    "0": "Pzt"
                },
                "months": {
                    "9": "Ekim",
                    "8": "Eylül",
                    "7": "Ağustos",
                    "6": "Temmuz",
                    "5": "Haziran",
                    "4": "Mayıs",
                    "3": "Nisan",
                    "2": "Mart",
                    "11": "Aralık",
                    "10": "Kasım",
                    "1": "Şubat",
                    "0": "Ocak"
                }
            },
            "decimal": ",",
            "editor": {
                "close": "Kapat",
                "create": {
                    "button": "Yeni",
                    "submit": "Kaydet",
                    "title": "Yeni kayıt oluştur"
                },
                "edit": {
                    "button": "Düzenle",
                    "submit": "Güncelle",
                    "title": "Kaydı düzenle"
                },
                "error": {
                    "system": "Bir sistem hatası oluştu (Ayrıntılı bilgi)"
                },
                "multi": {
                    "info": "Seçili kayıtlar bu alanda farklı değerler içeriyor. Seçili kayıtların hepsinde bu alana aynı değeri atamak için buraya tıklayın; aksi halde her kayıt bu alanda kendi değerini koruyacak.",
                    "noMulti": "Bu alan bir grup olarak değil ancak tekil olarak düzenlenebilir.",
                    "restore": "Değişiklikleri geri al",
                    "title": "Çoklu değer"
                },
                "remove": {
                    "button": "Sil",
                    "confirm": {
                        "_": "%d adet kaydı silmek istediğinize emin misiniz?",
                        "1": "Bu kaydı silmek istediğinizden emin misiniz?"
                    },
                    "submit": "Sil",
                    "title": "Kayıtları sil"
                }
            },
            "stateRestore": {
                "creationModal": {
                    "button": "Kaydet",
                    "columns": {
                        "search": "Kolon Araması",
                        "visible": "Kolon Görünümü"
                    },
                    "name": "Görünüm İsmi",
                    "order": "Sıralama",
                    "paging": "Sayfalama",
                    "scroller": "Kaydırma (Scrool)",
                    "search": "Arama",
                    "searchBuilder": "Arama Oluşturucu",
                    "select": "Seçimler",
                    "title": "Yeni Görünüm Oluştur",
                    "toggleLabel": "Kaydedilecek Olanlar"
                },
                "duplicateError": "Bu Görünüm Daha Önce Tanımlanmış",
                "emptyError": "Görünüm Boş Olamaz",
                "emptyStates": "Herhangi Bir Görünüm Yok",
                "removeConfirm": "Görünümü Silmek İstediğinize Eminminisiniz?",
                "removeError": "Görünüm Silinemedi",
                "removeJoiner": "ve",
                "removeSubmit": "Sil",
                "removeTitle": "Görünüm Sil",
                "renameButton": "Değiştir",
                "renameLabel": "Görünüme Yeni İsim Ver -&gt; %s:",
                "renameTitle": "Görünüm İsmini Değiştir"
            }
        }
    });
    /*Datatable ends here*/

    $(function () {
        const url = '/Admin/Category/Add';  
        const categoryAddModalDiv = $('#categoryAddModal');
        const categoryUpdateModalDiv = $('#categoryUpdateModal');

        //Ajax Get Operations
        //Category Add Modal Show Operation
        $('#btnAdd').click(function () {
            $.get(url).done(function (data) {
                
                categoryAddModalDiv.html(data);
                categoryAddModalDiv.find('.modal').modal('show');
            });
        });

        //CategoryUpdate Modal Show Operation
        $(document).on('click', '.btn-update', function (e) {
            e.preventDefault();
            const url = '/Admin/Category/Update';
            const categoryUpdateModalDiv = $('#categoryUpdateModal');
            const id = $(this).attr('data-id'); //id of selected category
            $.get(url, { categoryId: id }).done(function (data) {
                categoryUpdateModalDiv.html(data);
                categoryUpdateModalDiv.find('.modal').modal('show');
            }).fail(function () {
                toastr.error("Bir hata oluştu!");
            });
        });

        //Ajax Post Operations
        //CategoryAdd Operation
        categoryAddModalDiv.on('click', '#btnSave', function (e) {
            e.preventDefault();
            const form = $('#formCategoryAdd');
            const actionUrl = form.attr('action'); 
            const dataToSend = form.serialize(); 
            $.post(actionUrl, dataToSend).done(function (data) {
                const categoryAddAjaxModal = jQuery.parseJSON(data);
                const newFormBody = $('.modal-body', categoryAddAjaxModal.CategoryAddPartial);
                categoryAddModalDiv.find('.modal-body').replaceWith(newFormBody);
                const isValid = newFormBody.find('[name="IsValid"]').val() === 'True'; 
                if (isValid) {
                    categoryAddModalDiv.find('.modal').modal('hide');
                    const newTableRow =
                        `<tr name="row_${categoryAddAjaxModal.CategoryDto.Category.Id}">
                                <td>${categoryAddAjaxModal.CategoryDto.Category.Id}</td>
                                <td>${categoryAddAjaxModal.CategoryDto.Category.Name}</td>
                                <td>${categoryAddAjaxModal.CategoryDto.Category.Description}</td>
                                <td>${convertFirstLetterToUpperCase(categoryAddAjaxModal.CategoryDto.Category.IsActive.toString())}</td>
                                <td>${convertFirstLetterToUpperCase(categoryAddAjaxModal.CategoryDto.Category.IsDeleted.toString())}</td>
                                <td>${categoryAddAjaxModal.CategoryDto.Category.Note}</td>
                                <td>${convertToShortDate(categoryAddAjaxModal.CategoryDto.Category.CreatedDate)}</td>
                                <td>${categoryAddAjaxModal.CategoryDto.Category.CreatedByName}</td>
                                <td>${convertToShortDate(categoryAddAjaxModal.CategoryDto.Category.ModifiedDate)}</td>
                                <td>${categoryAddAjaxModal.CategoryDto.Category.ModifiedByName}</td>
                                <td>
                                    <button class="btn btn-primary btn-update btn-sm" data-id="${categoryAddAjaxModal.CategoryDto.Category.Id}"><span class="fas fa-edit"></span></button>
                                    <button class="btn btn-danger btn-delete btn-sm" data-id="${categoryAddAjaxModal.CategoryDto.Category.Id}"><span class="fas fa-minus-circle"></span></button>
                                </td>
                            </tr>`;
                    const newTableRowObject = $(newTableRow); 
                    newTableRowObject.hide(); 
                    //$('#categoryTable tbody').append(newTableRowObject);
                    table.row.add(newTableRowObject).draw(false);
                    newTableRowObject.fadeIn(2000);
                    toastr.success(`${categoryAddAjaxModal.CategoryDto.Message}`, 'Başarılı İşlem!'); 
                }
                else {
                    const validationSummaryErrorList = $('#validationSummary > ul > li', categoryAddAjaxModal.CategoryAddPartial);
                    let errorSummary = '';
                    validationSummaryErrorList.each(function () {
                        let error = $(this).text(); 
                        errorSummary += `*${error}<br>`;
                    });
                    toastr.warning(errorSummary);
                }
            });
        });

        //CategoryDelete Operation
        $(document).on('click', '.btn-delete', function (e) {
            e.preventDefault();
            const id = $(this).attr('data-id');
            const deletedTableRow = $(`[name="row_${id}"]`);   //row_1, row_2
            const categoryName = deletedTableRow.find('td:eq(1)').text();
            Swal.fire({
                title: 'Silmek istediğinize emin misiniz?',
                text: `${categoryName} adlı kategori silinecektir!`,
                icon: 'warning',
                showCancelButton: true,
                confirmButtonColor: '#3085d6',
                cancelButtonColor: '#d33',
                confirmButtonText: 'Evet',
                cancelButtonText: 'Hayır'
            }).then((result) => {
                if (result.isConfirmed) {
                    $.ajax({
                        url: '/Admin/Category/Delete',
                        type: 'Post',
                        dataType: 'json',
                        data: { categoryId: id },
                        success: function (data) {
                            const categoryDto = jQuery.parseJSON(data);
                            if (categoryDto.ResultStatus === 0) {
                                Swal.fire(
                                    'Silindi!',
                                    `${categoryDto.Category.Name} adlı kategori başarıyla silinmiştir!`,
                                    'success'
                                );
                                deletedTableRow.fadeOut(2000, function () {
                                    //table.row($(`[name="row_${id}"]`)).remove().draw();
                                    table.row(deletedTableRow).remove().draw(false);
                                });
                            }
                            else {
                                Swal.fire({
                                    icon: 'error',
                                    title: 'Başarısız İşlem!',
                                    text: `${categoryDto.Message}`,
                                })
                            }
                        },
                        error: function (error) {
                            toastr.error(`${error.responseText}`, 'Hata!');
                        }
                    });
                }
            })
        });

        //Category Update Operation
        categoryUpdateModalDiv.on('click', '#btnUpdate', function (e) {
            e.preventDefault();
            const form = $('#formCategoryUpdate');
            const url = form.attr('action'); /* /Admin/Category/Update */
            const dataToSend = form.serialize();
            $.post(url, dataToSend).done(function (data) {
                const categoryUpdateAjaxModal = jQuery.parseJSON(data);
                const newFormBody = $('.modal-body', categoryUpdateAjaxModal.CategoryUpdatePartial);
                categoryUpdateModalDiv.find('.modal-body').replaceWith(newFormBody);
                const isValid = newFormBody.find('input[name="IsValid"]').val() === 'True';
                if (isValid) {
                    categoryUpdateModalDiv.find('.modal').modal('hide');
                    const updatedTableRow =
                        `<tr name="row_${categoryUpdateAjaxModal.CategoryDto.Category.Id}">
                                <td>${categoryUpdateAjaxModal.CategoryDto.Category.Id}</td>
                                <td>${categoryUpdateAjaxModal.CategoryDto.Category.Name}</td>
                                <td>${categoryUpdateAjaxModal.CategoryDto.Category.Description}</td>
                                <td>${convertFirstLetterToUpperCase(categoryUpdateAjaxModal.CategoryDto.Category.IsActive.toString())}</td>
                                <td>${convertFirstLetterToUpperCase(categoryUpdateAjaxModal.CategoryDto.Category.IsDeleted.toString())}</td>
                                <td>${categoryUpdateAjaxModal.CategoryDto.Category.Note}</td>
                                <td>${convertToShortDate(categoryUpdateAjaxModal.CategoryDto.Category.CreatedDate)}</td>
                                <td>${categoryUpdateAjaxModal.CategoryDto.Category.CreatedByName}</td>
                                <td>${convertToShortDate(categoryUpdateAjaxModal.CategoryDto.Category.ModifiedDate)}</td>
                                <td>${categoryUpdateAjaxModal.CategoryDto.Category.ModifiedByName}</td>
                                <td>
                                    <button class="btn btn-primary btn-update btn-sm" data-id="${categoryUpdateAjaxModal.CategoryDto.Category.Id}"><span class="fas fa-edit"></span></button>
                                    <button class="btn btn-danger btn-delete btn-sm" data-id="${categoryUpdateAjaxModal.CategoryDto.Category.Id}"><span class="fas fa-minus-circle"></span></button>
                                </td>
                            </tr>`;
                    const updatedTableRowObject = $(updatedTableRow);
                    const tableRowToUpdate = $(`.table tbody tr[name="row_${categoryUpdateAjaxModal.CategoryDto.Category.Id}"]`);
                    const tdObjects = updatedTableRowObject.find('td');
                    const dataArr = tdObjects.map(function (index) 
                    {
                        if (tdObjects.length === index + 1) 
                            return $.trim($(this).html());
                        return $(this).text();
                    }).get();
                    if (dataArr[4] === 'True') { //Category => IsDeleted(True)
                        tableRowToUpdate.hide(1200, function () {
                            table.row(tableRowToUpdate).remove().draw(false);
                        });
                    }
                    else {
                        tableRowToUpdate.replaceWith(updatedTableRowObject);
                        updatedTableRowObject.hide();
                        updatedTableRowObject.fadeIn(2000, function () {
                            table.row(tableRowToUpdate).data(dataArr).draw(false);
                        });  
                        toastr.success(`${categoryUpdateAjaxModal.CategoryDto.Message}`, 'Başarılı İşlem!'); 
                    }
                }
                else {
                    const validationSummaryErrorList = $('#validationSummary > ul > li', categoryUpdateAjaxModal.CategoryUpdatePartial);
                    let errorSummary = '';
                    validationSummaryErrorList.each(function () {
                        let error = $(this).text(); 
                        errorSummary += `*${error}<br>`;
                    });
                    toastr.warning(errorSummary);
                }
            }).fail(function (response) {
                console.log(response);
            });
        });
    });
});