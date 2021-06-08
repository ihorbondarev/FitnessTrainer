$(document).ready(function(){
    $(".header_paragraph_block").on("click","a", function (event) {
        event.preventDefault();
        var id  = $(this).attr('href'),
            top = $(id).offset().top - $('.navbar').height();
        $('body,html').animate({scrollTop: top}, 1500);
    });
});