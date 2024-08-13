$(document).ready(function() {
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

    $('#btn_filter').on('click', function() {
        console.log($("#authorFilterSelector").val());
        
    })

    const apiUrl = 'https://localhost:44396/api/v1/Books';

    // Khởi tạo dữ liệu với tham số mặc định
    fetchBooks();

    // Tìm kiếm
    $('#searchInput').on('keyup', function() {
        fetchBooks();
    });

    // Sắp xếp cột
    $('th').on('click', function() {
        const column = $(this).data('column');
        const order = $(this).data('order');
        $(this).data('order', order === 'asc' ? 'desc' : 'asc');
        fetchBooks();
    });

    // Hàm fetch dữ liệu sách
    function fetchBooks(page = 1, limit = 10) {
        const search = $('#searchInput').val();
        const sortColumn = $('th[data-order]').data('column');
        const sortOrder = $('th[data-order]').data('order');

        $.ajax({
            url: apiUrl,
            method: 'GET',
            // data: {
            //     search: search,
            //     sortColumn: sortColumn,
            //     sortOrder: sortOrder,
            //     page: page,
            //     limit: limit
            // },
            success: function(response) {
                renderTable(response);
                // renderPagination(response.totalPages, page);
            }
        });
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
                <td>${book.createdOn}</td>
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

    // Render phân trang
    function renderPagination(totalPages, currentPage) {
        const pagination = $('.pagination');
        pagination.empty();

        for (let i = 1; i <= totalPages; i++) {
            const pageItem = `<li class="page-item ${i === currentPage ? 'active' : ''}">
                <a class="page-link" href="#">${i}</a>
            </li>`;
            pagination.append(pageItem);
        }

        // Sự kiện click vào phân trang
        $('.pagination li').on('click', function(e) {
            e.preventDefault();
            fetchBooks($(this).text());
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

    $('#addRowButton').on('click', function () {
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

        console.log(formData.get("image"));
        
        $.ajax({
            url: 'https://localhost:44396/api/v1/Books',
            type: 'POST',
            data: formData,
            contentType: false,
            processData: false,
            success: function (response) {
                alert('Thêm sách thành công!');
                console.log(response);
                
                // Clear form hoặc thực hiện hành động khác
            },
            error: function () {
                swal("Thất bại!", "Có lỗi trong quá trình thêm sách!", {
                    icon: "error",
                    buttons: {
                      confirm: {
                        className: "btn btn-danger",
                      },
                    },
                  });
            }
        });
    });
});
