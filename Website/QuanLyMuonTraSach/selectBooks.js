$(document).ready(function () {
    var selectedData = [];

    const apiUrl = 'https://localhost:44396/api/v1/Books';

    // FILTER
    $("#authorFilterSelector").select2()
    $.ajax({
        url: 'https://localhost:44396/api/v1/Authors',
        method: 'GET',
        success: function (response) {
            renderAuthorFilterSelector(response);
        }
    });
    function renderAuthorFilterSelector(authors) {
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
        success: function (response) {
            renderCategoryFilterSelector(response);
        }
    });
    function renderCategoryFilterSelector(categories) {
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

    $('#btn_filter').on('click', function () {
        pageNumber = 1;
        authorIds = $('#authorFilterSelector').val();
        categoryIds = $('#categoryFilterSelector').val();
        searchString = $('#searchBook').val();
        orderBy = $('#orderBySelect').val();
        if ($('#startPubYear').val() && $('#endPubYear').val()) {
            publishedYearRange = $('#startPubYear').val() + '-' + $('#endPubYear').val();
        }
        fetchBooks();
    })
    $('#pageSizeSelect').on('change', function () {
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
            success: function (response) {
                console.log(response);

                renderTable(response.items);

                pagination(response)
            },
            error: function (response) {
                if (response.status === 401) { // Token hết hạn
                    // Thử lấy Access Token mới bằng Refresh Token
                    refreshAccessToken()
                        .done(function () {
                            // Thử lại yêu cầu API với token mới
                            return fetchBooks();
                        })
                        .fail(function () {
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
        selectedData = $('#selectedBooks input[type="checkbox"]:checked').map(function() {
            return $(this).val();
        }).get();

        const tableBody = $('#booksTable');
        tableBody.empty();
        let index = 1;
        books.forEach(book => {
            const row = `<tr id="${book.id}">
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
                        <input
                            class="form-check-input bookSelector"
                            type="checkbox"
                            value="${book.id}"
                            ${(selectedData.includes(book.id)) ? "checked" : ""}
                            ${(book.availableQuantity === 0) ? "disabled" : ""}
                        />
                    </td>
                </tr>`;
            tableBody.append(row);
        });

        $('#booksTable .bookSelector').on('change', function() {
            const $row = $(this).closest('tr');
            if ($(this).is(':checked')) {
                // Copy và append dòng đã chọn sang bảng selectedBooks
                const $newRow = $row.clone();
                $('#selectedBooks').append($newRow);

                // Add handle cho row mới trong bảng selectedBooks
                $(`#selectedBooks tr#${$newRow.attr('id')}`).on('change', function() {
                    $(`#booksTable tr#${$newRow.attr('id')} .bookSelector`).prop('checked', false);
                    $(this).remove()
                })
            } else {
                // Xóa dòng khỏi bảng selectedBooks khi checkbox bị bỏ chọn
                const id = $row.attr('id');
                $(`#selectedBooks tr#${id}`).remove();
            }
            
        })
    }

    function pagination(response) {
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

        $('.page-item').on('click', function (e) {
            e.preventDefault()

            if ($(this).hasClass('disabled')) {
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

    $('#btn_confirm').on('click', function() {
        // Cập nhật lại selectedData
        selectedData = $('#selectedBooks input[type="checkbox"]:checked').map(function() {
            return $(this).val();
        }).get();
        console.log(selectedData);
        // window.opener.postMessage(selectedData, window.location.origin);
        localStorage.setItem('selectedData', JSON.stringify(selectedData));
        window.close();
    })
})