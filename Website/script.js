
function parseJwt (token) {
    var base64Url = token.split('.')[1];
    var base64 = base64Url.replace(/-/g, '+').replace(/_/g, '/');
    var jsonPayload = decodeURIComponent(window.atob(base64).split('').map(function(c) {
        return '%' + ('00' + c.charCodeAt(0).toString(16)).slice(-2);
    }).join(''));

    return JSON.parse(jsonPayload);
}

function getRole()
{
    return localStorage.getItem('role')
}

function getAccessToken()
{
    return localStorage.getItem('accessToken')
}

function getRefreshToken()
{
    return localStorage.getItem('refreshToken')
}

function refreshAccessToken() {
    console.log("Getting new accessToken...");
    
    return $.ajax({
        url: 'https://localhost:44396/api/v1/Auths/refresh-token', // Đường dẫn tới endpoint làm mới token của bạn
        method: 'POST',
        contentType: 'application/json',
        data: JSON.stringify({
            accessToken: localStorage.getItem('accessToken'),
            refreshToken: localStorage.getItem('refreshToken')
        }),
        success: function(response) {
            localStorage.setItem('accessToken', response.accessToken)
            localStorage.setItem('refreshToken', response.refreshToken)
            return;
        },
        error: function(response) {
            console.error('Failed to refresh token:', response);
            logout();
            return null;
        }
    });
}

function logout() {
    localStorage.removeItem('accessToken')
    localStorage.removeItem('refreshToken')
    localStorage.removeItem('role')
    localStorage.removeItem('personalInfor')

    window.location.href = '../AuthPages/Auth.html';
}

function savePersonalInfor()
{
    $.ajax({
        url: 'https://localhost:44396/api/v1/Auths',
        headers: {
            'Authorization': `Bearer ${getAccessToken()}` // Đính kèm Access Token vào header
        },
        method: 'GET',
        success: function(response) {
            localStorage.setItem('personalInfor', JSON.stringify(response))
        },
        error: function (response) {
            if (response.status === 401) { // Token hết hạn
                // Thử lấy Access Token mới bằng Refresh Token
                refreshAccessToken()
                    .done(function () {
                        // Thử lại yêu cầu API với token mới
                        return savePersonalInfor();
                    })
            } else {
                console.error('API error:', response);
            }
        }
    });
}

function getPersonalInfor()
{
    const infor = JSON.parse(localStorage.getItem('personalInfor'));
    return infor;
}

$(document).ready(function() {
    $('#logout').on('click', function() {
        logout()
    })
})