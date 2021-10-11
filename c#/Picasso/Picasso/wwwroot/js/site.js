$(document).ready(function () {
    $("[data-page]").click(function () {
        let page = isNaN(parseInt($(this).attr("data-page"))) ? -1 : parseInt($(this).attr("data-page")), text = $("#search").val().trim();
        page = page - 1;
        var but = $(this); but.prop('disabled', true);
        $.get("/Home/PageUrl/", { page: page, limit: 30, text: text }).done(data => { $("#view-search").html();});
    });
    $("#search-api").click(function () {
        let text = $("#search").val().trim();
        if (text.length < 2) return false;
        var but = $(this); but.prop('disabled', true); but.find("span").fadeIn();
        $.get("/Home/Search/", { text: text})
            .always(() => { but.prop('disabled', false); but.find("span").fadeOut(); })
            .done(data => { $("#view-search").html(); });
    });
    $("#search-in").click(function () {
        let text = $("#search").val().trim();
        if (text.length < 2) return false;
        var but = $(this); but.prop('disabled', true); but.find("span").fadeIn();
        $.get("/Home/PageUrl/", { text: text})
            .always(() => { but.prop('disabled', false); but.find("span").fadeOut(); })
            .done(data => { $("#view-search").html(); });
    });
});