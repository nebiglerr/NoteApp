$(function () {
    var noteids = [];

    $("div[data-note-id]").each(function (i, e) {

        noteids.push($(e).data("note-id"));
    });
    $.ajax({
        method: "POST",
        url: "/Note/GetLiked",
        data: { ids: noteids }
    }).done(function (data) {

        if (data.result != null && data.result.length > 0) {

            for (var i = 0; i < data.result.length; i++) {
                var id = data.result[i];
                var likedNote = $("div[data-note-id=" + id + "]");
                var btn = likedNote.find("button[data-liked]");
                var span = btn.find("span.liked-star");

                btn.data("liked", true);
                span.removeClass("glyphicon-star-empty");
                span.addClass("glyphicon-star");
            }
        }
    }).fail(function myfunction() {
        alert("Sunucu ile bağlantı kurulamadı");
    });


    $("button[data-liked]").click(function () {
        var btn = $(this); // this o anki butonu verir yakalarsın
        var liked = btn.data("liked");
        var noteid = btn.data("note-id");
        var spanStar = btn.find("span.liked-star");
        var spanCount = btn.find("span.liked-count");

        console.log(liked);
        $.ajax({
            method: "POST",
            url: "/Note/SetLikedState",
            data: { "noteid": noteid, "liked": !liked }

        }).done(function (data) {

            console.log(data);
            console.log("like count :" + spanCount.text());

            if (data.hasError) {
                alert(data.errorMessage);
            } else {
                liked = !liked;
                btn.data("liked", liked);
                spanCount.text(data.result);

                console.log("like count after :" + spanCount.text());

                spanStar.removeClass("glyphicon-star-empty");
                spanStar.removeClass("glyphicon-star");
                if (liked) {
                    spanStar.addClass("glyphicon-star");
                } else {
                    spanStar.addClass("glyphicon-star-empty");
                }

            }
        }).fail(function () {

        });
    });
})