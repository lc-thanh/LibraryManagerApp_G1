$(document).ready(function () {
  if (localStorage.getItem('accessToken'))
  {
    if (getRole() === 'Admin')
    {
      window.location.href = '../index.html';
    }
    if (getRole() === 'Librarian')
    {
      window.location.href = '../QuanLySach/QLSach.html';
    }
  }
    
  var panelOne = $('.form-panel.two').height(),
    panelTwo = $('.form-panel.two')[0].scrollHeight;

  $('.form-panel.two').not('.form-panel.two.active').on('click', function (e) {
    e.preventDefault();

    $('.form-toggle').addClass('visible');
    $('.form-panel.one').addClass('hidden');
    $('.form-panel.two').addClass('active');
    $('.form').animate({
      'height': panelTwo
    }, 200);
  });

  $('.form-toggle').on('click', function (e) {
    e.preventDefault();
    $(this).removeClass('visible');
    $('.form-panel.one').removeClass('hidden');
    $('.form-panel.two').removeClass('active');
    $('.form').animate({
      'height': panelOne
    }, 200);
  });

  $('#loginForm').on('submit', function (e) {
    e.preventDefault();

    var loginData = {
      email: $('#loginEmail').val(),
      password: $('#loginPassword').val()
    };

    $.ajax({
      url: 'https://localhost:44396/api/v1/Auths/login', // Địa chỉ API để xử lý đăng nhập
      method: 'POST',
      contentType: 'application/json',
      data: JSON.stringify(loginData),
      success: function (response) {
        // Xử lý khi đăng nhập thành công
        console.log('Login successful:', response);

        // Lưu Access Token và Refresh Token vào localStorage
        localStorage.setItem('accessToken', response.token);
        localStorage.setItem('refreshToken', response.refreshToken);
        localStorage.setItem('role', response.role);
        savePersonalInfor();

        // Chuyển hướng đến trang khác hoặc thực hiện các hành động khác
        redirect(response)

      },
      error: function (xhr, status, error) {
        // Xử lý khi đăng nhập thất bại
        console.error('Login failed:', error);
        alert("Đăng nhập thất bại")
      }
    });

    function redirect(response) {
      if (localStorage.getItem('role') === 'Admin')
      {
        window.location.href = '../index.html';
      }
      else if (localStorage.getItem('role') === 'Librarian')
      {
        window.location.href = '../QuanLyMuonTraSach/QLMuonTraSach.html';
      }
    }
  })
});