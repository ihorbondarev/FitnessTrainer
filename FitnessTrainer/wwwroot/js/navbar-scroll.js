$(window).scroll(function(){
    if ($(window).scrollTop() > 200) {
        $('.navbar').addClass('scroll');
    }
    else {
        $('.navbar').removeClass('scroll')
    }
});