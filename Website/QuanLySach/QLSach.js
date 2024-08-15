if (!localStorage.getItem('accessToken'))
{
    window.location.href = '../AuthPages/Auth.html';
}



$(document).ready(function() {
    if (getRole() != 'Admin' && getRole() != 'Librarian')
    {
        // redirect to Member pages
    }
    const apiUrl = 'https://localhost:44396/api/v1/Books';

    // =========== NOTIFICATION - SignalR =========== 
    // const connection = new signalR.HubConnectionBuilder()
    // .withUrl("https://localhost:44396/api/v1/notificationHub")
    // .build();
    // connection.on("ReceiveMessage", function (user, message) {
    //     // Hiển thị thông báo cho người dùng
    //     alert(`${user}: ${message}`);
    // });
    // connection.start().catch(function (err) {
    //     return console.error(err.toString());
    // });
    // =========== END NOTIFICATION ============
    
    // FILTER
    $("#authorFilterSelector").select2()
    $.ajax({
        url: 'https://localhost:44396/api/v1/Authors',
        method: 'GET',
        success: function(response) {
            renderAuthorFilterSelector(response);
        }
    });
    function renderAuthorFilterSelector(authors)
    {
        const authorSelector = $('#authorFilterSelector');
        authorSelector.empty();
        
        authors.forEach(author => {
            const option = `<option value="${author.id}">${author.name}</option>`;
            authorSelector.append(option);
        });
    }

    $("#categoryFilterSelector").select2()
    $.ajax({
        url: 'https://localhost:44396/api/v1/Categories',
        method: 'GET',
        success: function(response) {
            renderCategoryFilterSelector(response);
        }
    });
    function renderCategoryFilterSelector(categories)
    {
        const categorySelector = $('#categoryFilterSelector');
        categorySelector.empty();
        
        categories.forEach(category => {
            const option = `<option value="${category.id}">${category.name}</option>`;
            categorySelector.append(option);
        });
    }

    var searchString;
    var orderBy;
    var publishedYearRange;
    var authorIds = [];
    var categoryIds = [];
    var pageNumber;
    var pageSize;

    $('#btn_filter').on('click', function() {
        pageNumber = 1;
        authorIds = $('#authorFilterSelector').val();
        categoryIds = $('#categoryFilterSelector').val();
        searchString = $('#searchBook').val();
        orderBy = $('#orderBySelect').val();
        if ($('#startPubYear').val() && $('#endPubYear').val())
        {
            publishedYearRange = $('#startPubYear').val() + '-' + $('#endPubYear').val();
        }
        fetchBooks();
    })
    $('#pageSizeSelect').on('change', function() {
        pageSize = $(this).val()
        fetchBooks();
    })

    // Khởi tạo dữ liệu với tham số mặc định
    fetchBooks();

    // Hàm fetch dữ liệu sách
    function fetchBooks() {
        var params = {
            searchString: searchString,
            orderBy: orderBy,
            publishedYearRange: publishedYearRange,
        };

        if (authorIds.length > 0)
            params.authorIds = authorIds;
        if (categoryIds.length > 0)
            params.categoryIds = categoryIds;
        if (pageNumber)
            params.pageNumber = pageNumber;
        if (pageSize)
            params.pageSize = pageSize;

        // Tạo query string từ object params
        var queryString = $.param(params);

        $.ajax({
            url: apiUrl + '?' + queryString.replaceAll("%5B%5D", ""),
            method: 'GET',
            headers: {
                'Authorization': `Bearer ${getAccessToken()}` // Đính kèm Access Token vào header
            },
            success: function(response) {
                console.log(response);
                
                renderTable(response.items);

                pagination(response)
            },
            error: function(response) {
                if (response.status === 401) { // Token hết hạn
                    // Thử lấy Access Token mới bằng Refresh Token
                    refreshAccessToken()
                        .done(function() {
                            // Thử lại yêu cầu API với token mới
                            return fetchBooks();
                        })
                        .fail(function() {
                        });
                } else {
                    console.error('API error:', response);
                }
            }
        });
    }

    // Chuyển định dạng DateTime
    function formatDateTime(dateTimeString) {
        var date = new Date(dateTimeString);
    
        // Lấy giờ và phút
        var hours = date.getHours().toString().padStart(2, '0');
        var minutes = date.getMinutes().toString().padStart(2, '0');
    
        // Lấy ngày, tháng, và năm
        var day = date.getDate().toString().padStart(2, '0');
        var month = (date.getMonth() + 1).toString().padStart(2, '0');
        var year = date.getFullYear();
    
        // Định dạng hh:mm dd/MM/yyyy
        return `${hours}:${minutes} - ${day}/${month}/${year}`;
    }

    // Render bảng sách
    function renderTable(books) {
        const tableBody = $('#booksTable');
        tableBody.empty();
        let index = 1;
        books.forEach(book => {
            const row = `<tr>
                <td>${index++}</td>
                <td>${book.title}</td>
                <td><img src="${book.imageUrl}"/></td>
                <td>${book.quantity}</td>
                <td>${book.availableQuantity}</td>
                <td>${book.totalPages}</td>
                <td>${(book.authorName) ? book.authorName : "Không"}</td>
                <td>${(book.categoryName) ? book.categoryName : "Không"}</td>
                <td>${(book.bookShelfName) ? book.bookShelfName : "Không"}</td>
                <td>${(book.publisher) ? book.publisher : "Không"}</td>
                <td>${(book.publishedYear) ? book.publishedYear : "Không"}</td>
                <td>${formatDateTime(book.createdOn)}</td>
                <td>
                    <div class="form-button-action">
                        <button
                            type="button"
                            data-bs-toggle="tooltip"
                            title=""
                            class="btn btn-link btn-primary btn-lg"
                            data-original-title="Edit Task"
                            style="padding: 5px 8px"
                        >
                            <i class="fa fa-edit"></i>
                        </button>
                        <button
                            type="button"
                            data-bs-toggle="tooltip"
                            title=""
                            class="btn btn-link btn-danger"
                            data-original-title="Remove"
                            style="padding: 7px 8px 5px 8px"
                        >
                            <i class="fa fa-times"></i>
                        </button>
                    </div>
                </td>
            </tr>`;
            tableBody.append(row);
        });
    }

    function pagination(response)
    {
        // Pagination
        $('.pagination li').remove();
        $('.pagination').html(`
            <li id="prevPage" class="page-item">
                <a class="page-link" href="#" aria-label="Previous">
                    <span aria-hidden="true">&laquo;</span>
                    <span class="sr-only">Previous</span>
                </a>
            </li>
            <li id="nextPage" class="page-item">
                <a class="page-link" href="#" aria-label="Next">
                    <span aria-hidden="true">&raquo;</span>
                    <span class="sr-only">Next</span>
                </a>
            </li>
        `)

        // Xác định khoảng trang muốn hiển thị
        var startPage = Math.max(1, response.pageIndex - 2);
        var endPage = Math.min(response.totalPages, response.pageIndex + 2);

        // Thêm các trang vào pagination
        for (var i = startPage; i <= endPage; i++) {
            var activeClass = (i === response.pageIndex) ? ' active' : '';
            // var pageItem = `<li class="page-item${activeClass}"><a class="page-link" href="${currentUrl}?pageNumber=${i}">${i}</a></li>`;
            var pageItem = `<li class="page-item${activeClass}"><a class="page-link" href="#">${i}</a></li>`;
            $('#nextPage').before(pageItem);
        }

        // Kiểm tra và vô hiệu hóa nút Previous và Next
        if (!response.hasPreviousPage) {
            $('#prevPage').addClass('disabled');
        } else {
            $('#prevPage').removeClass('disabled');
        }

        if (!response.hasNextPage) {
            $('#nextPage').addClass('disabled');
        } else {
            $('#nextPage').removeClass('disabled');
        }

        $('.page-item').on('click', function(e) {
            e.preventDefault()
        
            if ($(this).hasClass('disabled'))
            {
                return;
            }
            
            var currentPage = parseInt($('.pagination .page-item.active a').text());
        
            // Xác định trang mới từ nút bấm
            if ($(this).attr('id') === 'prevPage') {
                currentPage--;
            } else if ($(this).attr('id') === 'nextPage') {
                currentPage++;
            } else {
                currentPage = parseInt($(this).text());
            }
        
            pageNumber = currentPage;
            fetchBooks();
        });
    }

    // ===================== AddRowModel ========================
    $("#authorSelector").select2({ 
        dropdownParent: $("#addRowModal") 
    })
    $("#categorySelector").select2({ 
        dropdownParent: $("#addRowModal") 
    })
    $("#bookshelfSelector").select2({ 
        dropdownParent: $("#addRowModal") 
    })

    $('#btn_addRowModal').on('click', function() {
        $.ajax({
            url: 'https://localhost:44396/api/v1/Authors',
            method: 'GET',
            success: function(response) {
                renderAuthorSelector(response);
            }
        });

        $.ajax({
            url: 'https://localhost:44396/api/v1/Categories',
            method: 'GET',
            success: function(response) {
                renderCategorySelector(response);
            }
        });

        $.ajax({
            url: 'https://localhost:44396/api/v1/Bookshelves',
            method: 'GET',
            success: function(response) {
                renderBookshelfSelector(response);
            }
        });
    })

    function renderAuthorSelector(authors)
    {
        const authorSelector = $('#authorSelector');
        authorSelector.empty();
        
        authorSelector.append(`<option value="" disabled selected>-- Chọn tác giả --</option>`);
        authors.forEach(author => {
            const option = `<option value="${author.id}">${author.name}</option>`;
            authorSelector.append(option);
        });
    }

    function renderCategorySelector(categories)
    {
        const categorySelector = $('#categorySelector');
        categorySelector.empty();
        
        categorySelector.append(`<option value="" disabled selected>-- Chọn loại tài liệu --</option>`);
        categories.forEach(category => {
            const option = `<option value="${category.id}">${category.name}</option>`;
            categorySelector.append(option);
        });
    }

    function renderBookshelfSelector(bookshelves)
    {
        const bookshelfSelector = $('#bookshelfSelector');
        bookshelfSelector.empty();
        
        bookshelfSelector.append(`<option value="" disabled selected>-- Chọn ngăn để sách --</option>`);
        bookshelves.forEach(bookshelf => {
            const option = `<option value="${bookshelf.id}">${bookshelf.name}</option>`;
            bookshelfSelector.append(option);
        });
    }

    $('#authorSelector').on('change', function() {
        console.log($('#authorSelector').find(":selected").val());
    })

    // Nhấn nút Xóa ảnh
    $('#reset_img_btnAdd').on('click', function () {
        $('#selectedImageAdd').attr('src', `https://localhost:44396/images/books/null.png`)
    })

    $('#uploadImgAdd').on('change', function () {
        const [file] = uploadImgAdd.files
        console.log(file);
        
        if (file)   
            $('#selectedImageAdd').attr('src', URL.createObjectURL(file))
    })

    function add_new_book()
    {
        var formData = new FormData();
        formData.append('title', $('#addTitle').val());
        formData.append('publisher', $('#addPublisher').val());
        formData.append('publishedYear', $('#addPulishedYear').val());
        formData.append('quantity', $('#addQuantity').val());
        formData.append('totalPages', $('#addTotalPages').val());
        formData.append('description', $('#addDescription').val());
        formData.append('authorId', $('#authorSelector').find(":selected").val());
        formData.append('categoryId', $('#categorySelector').find(":selected").val());
        formData.append('bookShelfId', $('#bookshelfSelector').find(":selected").val());

        const [file] = uploadImgAdd.files
        formData.append('image', file);
        
        $.ajax({
            url: 'https://localhost:44396/api/v1/Books',
            type: 'POST',
            headers: {
                'Authorization': `Bearer ${getAccessToken()}` // Đính kèm Access Token vào header
            },
            data: formData,
            contentType: false,
            processData: false,
            success: function (response) {
                swal("Thành công!", "Đã thêm sách mới!", {
                    icon: "success",
                    buttons: {
                        confirm: {
                            className: "btn btn-success",
                        },
                    },
                });
                console.log(response);
                fetchBooks();
            },
            error: function (response) {
                if (response.status === 401) { // Token hết hạn
                    // Thử lấy Access Token mới bằng Refresh Token
                    refreshAccessToken()
                        .done(function() {
                            // Thử lại yêu cầu API với token mới
                            add_new_book()
                        })
                        .fail(function() {
                        });
                } else {
                    console.log("Add new book error: " + response.responseJSON);

                    swal("Thất bại!", "Có lỗi trong quá trình thêm sách!", {
                        icon: "error",
                        buttons: {
                            confirm: {
                                className: "btn btn-danger",
                            },
                        },
                    });
                }
                
            }
        });
    }

    $('#addRowButton').on('click', function () {
        add_new_book();
    });
});
