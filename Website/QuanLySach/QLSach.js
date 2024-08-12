$(document).ready(function() {
    const apiUrl = 'https://localhost:44396/api/v1/Books'; // Thay bằng API endpoint của bạn

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
});
