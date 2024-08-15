$(document).ready(function(){
    $("#memberSelector").select2();
    $("#bookSelector").select2();

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

    $("#bookFilterSelector").select2()
    $.ajax({
        url: 'https://localhost:44396/api/v1/Books',
        method: 'GET',
        success: function(response) {
            renderBookFilterSelector(response);
        }
    });
    function renderBookFilterSelector(books)
    {
        const bookSelector = $('#bookFilterSelector');
        bookSelector.empty();
        
        books.forEach(book => {
            const option = `<option value="${book.id}">${book.title}</option>`;
            bookSelector.append(option);
        });
    }

    $.ajax({
        url: "https://localhost:44396/api/v1/Members",
        method: 'GET',
        success: function(response){
            renderMemberSelector(response);
        }
    });

    $.ajax({
        url: 'https://localhost:44396/api/v1/Books',
        method: 'GET',
        success: function(response) {
            renderBookSelector(response);
        }
    });

    function renderMemberSelector(members) {
        const memberSelector = $('#memberSelector');
        memberSelector.empty();
        memberSelector.append(`<option value="" disabled selected>-- Chọn thành viên --</option>`);
        members.forEach(member => {
            const option = `<option value="${member.id}">${member.name}</option>`;
            memberSelector.append(option);
        });
    }

    function renderBookSelector(books) {
        const bookSelector = $('#bookSelector');
        bookSelector.empty();
        bookSelector.append(`<option value="" disabled selected>-- Chọn sách --</option>`);
        books.forEach(book => {
            const option = `<option value="${book.id}">${book.title}</option>`;
            bookSelector.append(option);
        });
    }

    $('#loanBookButton').on('click', function() {
        var formData = {
            memberId: $('#memberSelector').val(),
            bookId: $('#bookSelector').val(),
            loanDate: $('#loanDate').val(),
            dueDate: $('#dueDate').val(),
            quantity: $('#quantity').val()
        };

        $.ajax({
            url: 'https://localhost:44396/api/v1/Loans',
            method: 'POST',
            contentType: 'application/json',
            data: JSON.stringify(formData),
            success: function(response) {
                alert('Thêm phiếu mượn sách thành công!');
                console.log(response);
            },
            error: function() {
                swal("Thất bại!", "Có lỗi trong quá trình thêm phiếu mượn sách!", {
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
            success: function(response) {
                renderLoanTable(response);
            }
        });
    }

    function renderLoanTable(loans) {
        const tableBody = $('#loansTable');
        tableBody.empty();
        let index = 1;
        loans.forEach(loan => {
            const row = `<tr>
                <td>${index++}</td>
                <td>${loan.bookTitle}</td>
                <td>${loan.memberName}</td>
                <td>${loan.loanDate}</td>
                <td>${loan.dueDate}</td>
                <td>${loan.quantity}</td>
                <td>${loan.returnedDate ? loan.returnedDate : "Chưa trả"}</td>
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