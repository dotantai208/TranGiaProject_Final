$(document).ready(function () {
    $('.navbar-nav .nav-link').on('click', function () {
        // Loại bỏ lớp active từ tất cả các nav-link trong navbar-nav
        $('.navbar-nav .nav-link').removeClass('active');
        // Thêm lớp active vào nav-link được nhấp chuột
        $(this).addClass('active');
    });
});
