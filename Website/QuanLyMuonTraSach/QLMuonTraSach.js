if (!localStorage.getItem('accessToken'))
{
    window.location.href = '../AuthPages/Auth.html';
}

$(document).ready(function(){
    savePersonalInfor()
    const personalInfor = getPersonalInfor()
    console.log(personalInfor);

    $('#addLibrarian').val(personalInfor.fullName)

    $('#btn_addRowModal').on('click', function() {
        localStorage.removeItem('selectedData'); // Xóa sau khi sử dụng để tránh dữ liệu cũ
    })

    $('#btn_selectBooks').on('click', function() {
        const newWindow = window.open('./selectBooks.html', 'Select Books', 'width=1500,height=850');
        
        window.addEventListener('focus', function() {
            const selectedData = JSON.parse(localStorage.getItem('selectedData'));
            if (selectedData) {
                console.log('Dữ liệu đã chọn:', selectedData);

                const tableBody = $('#table_selectedBooks');
                tableBody.empty();
                let index = 1;
                selectedData.forEach(bookId => {
                    $.ajax({
                        url: 'https://localhost:44396/api/v1/Books/' + bookId,
                        method: 'GET',
                        success: function(response) {
                            const row = `
                                <tr id="${response.id}">
                                    <td>${index++}</td>
                                    <td>${response.title}</td>
                                    <td>${(response.authorName) ? response.authorName : "Không"}</td>
                                </tr>`;
                            tableBody.append(row);
                        }
                    });
                });

            }
        });
    })

    function add_new_loan()
    {
        var loanData = {
            memberEmail: $("#addMemberEmail").val(),
            bookIds: JSON.parse(localStorage.getItem('selectedData')),
            loanDate: $("#addLoanDate").val(),
            dueDate: $("#addDueDate").val()
        };

        console.log(loanData);
        

        $.ajax({
            url: 'https://localhost:44396/api/v1/Loans',
            type: 'POST',
            headers: {
                'Authorization': `Bearer ${getAccessToken()}` // Đính kèm Access Token vào header
            },
            data: loanData,
            success: function (response) {
                swal("Thành công!", "Đã thêm phiếu mượn mới!", {
                    icon: "success",
                    buttons: {
                        confirm: {
                            className: "btn btn-success",
                        },
                    },
                });
                localStorage.removeItem('selectedData'); // Xóa sau khi sử dụng để tránh dữ liệu cũ
                console.log(response);
                fetchLoanedBooks();
            },
            error: function (response) {
                if (response.status === 401) { // Token hết hạn
                    // Thử lấy Access Token mới bằng Refresh Token
                    refreshAccessToken()
                        .done(function() {
                            // Thử lại yêu cầu API với token mới
                            add_new_loan()
                        })
                        .fail(function() {
                        });
                } else {
                    console.log(response);

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
        add_new_loan();
    });

    $('#returnBookButton').on('click', function() {
        var formData = {
            memberId: $('#memberSelector').val(),
            bookId: $('#bookSelector').val(),
            returnDate: $('#returnDate').val(),
            quantity: $('#quantity').val()
        };

        $.ajax({
            url: 'https://localhost:44396/api/v1/Loans',
            method: 'POST',
            contentType: 'application/json',
            data: JSON.stringify(formData),
            success: function(response) {
                alert('Trả sách thành công!');
                console.log(response);
            },
            error: function() {
                swal("Thất bại!", "Có lỗi trong quá trình trả sách!", {
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

    fetchLoanedBooks();

    function fetchLoanedBooks() {
        $.ajax({
            url: 'https://localhost:44396/api/v1/Loans',
            method: 'GET',
            headers: {
                'Authorization': `Bearer ${getAccessToken()}` // Đính kèm Access Token vào header
            },
            success: function(response) {
                console.log(response.items);
                
                renderLoanTable(response.items);
            },
            error: function (response) {
                if (response.status === 401) { // Token hết hạn
                    // Thử lấy Access Token mới bằng Refresh Token
                    refreshAccessToken()
                        .done(function() {
                            // Thử lại yêu cầu API với token mới
                            fetchLoanedBooks()
                        })
                        .fail(function() {
                        });
                } else {
                    console.log(response);
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

    function generateStatusBadge(status)
    {
        switch (status) {
            case "OnLoan":
                return `<span class="badge badge-warning">Đang mượn</span>`

            case "Returned":
                return `<span class="badge badge-success">Đã trả</span>`

            case "Overdue":
                return `<span class="badge badge-danger">Quá hạn</span>`
        
            default:
                return;
        }
    }

    function renderLoanTable(loans) {
        const tableBody = $('#loansTable');
        tableBody.empty();
        let index = 1;
        loans.forEach(loan => {
            const row = `<tr>
                <td>${index++}</td>
                <td>${loan.loanCode}</td>
                <td>${loan.bookNames.join('; ')}</td>
                <td>${loan.memberFullName}</td>
                <td>${formatDateTime(loan.loanDate)}</td>
                <td>${formatDateTime(loan.dueDate)}</td>
                <td>${loan.librarianFullName}</td>
                <td>${generateStatusBadge(loan.status)}</td>
                <td>${(loan.returnedDate) ? formatDateTime(loan.returnedDate) : "Chưa trả"}</td>
                <td>
                    <div class="form-button-action">
                        <button
                            type="button"
                            data-bs-toggle="tooltip"
                            title=""
                            class="btn btn-link btn-primary btn-lg"
                            data-original-title="Edit Loan"
                            style="padding: 5px 8px"
                        >
                            <i class="fa fa-edit"></i>
                        </button>
                        <button
                            type="button"
                            data-bs-toggle="tooltip"
                            title=""
                            class="btn btn-link btn-danger"
                            data-original-title="Remove Loan"
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

});