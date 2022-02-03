var noteid = -1;
var modalCommentBodyId = "#model_comment_body";

$(function () {
    $('#model_comment').on('show.bs.modal', function (e) {

        var btn = $(e.relatedTarget); //tıklanan butonu yakalama

        noteid = btn.data("note-id"); // tıklanan buttonun data özelliğindeki note-id ile id yakalandı

        $(modalCommentBodyId).load("/Comment/ShowNoteComments/" + noteid);
    })

});
function doComment(btn, e, commentid, spanid) {

    var button = $(btn); // $(btn) alınması lazım jquery içindekileri okuması için
    var mode = button.data("edit-mode");

    if (e === "edit_clicked") {
        if (!mode) {
            button.data("edit-mode", true);
            button.removeClass("btn-warning");
            button.addClass("btn-success");

            var btnSpan = button.find("span");
            btnSpan.removeClass("glyphicon-edit");
            btnSpan.addClass("glyphicon-ok");

            $(spanid).addClass("editable");
            $(spanid).attr("contenteditable", true);
            $(spanid).focus();
        }
        else {
            button.data("edit-mode", false);
            button.addClass("btn-warning");
            button.removeClass("btn-success");

            var btnSpan = button.find("span");
            btnSpan.addClass("glyphicon-edit");
            btnSpan.removeClass("glyphicon-ok");

            $(spanid).removeClass("editable");
            $(spanid).attr("contenteditable", false);

            var txt = $(spanid).text();

            $.ajax({
                method: "POST",
                url: "/Comment/Edit/" + commentid,
                data: { text: txt }
            }).done(function (data) {
                if (data.result) {
                    // yorumlar partial tekrar yüklenir
                    $(modalCommentBodyId).load("/Comment/ShowNoteComments/" + noteid);
                }
                else {
                    alert("Yorum Güncellenemedi");
                }
            }).fail(function () {

                alert("Sunucu ile bağlantı kurulamanadı");
            });
        }


    } else if (e === "delete_clicked") {

        var dialog_res = confirm("Silmek istediğinize emin misiniz?");
        if (!dialog_res) return false;

        $.ajax({
            method: "GET",
            url: "/Comment/Delete/" + commentid
        }).done(function (data) {
            if (data.result) {
                // yorumlar partial tekrar yüklenir
                $(modalCommentBodyId).load("/Comment/ShowNoteComments/" + noteid);
            }
            else {
                alert("Yorum Silinemedi");
            }
        }).fail(function () {

            alert("Sunucu ile bağlantı kurulamanadı");
        });

    } else if (e === "new_clicked") {
        var txt = $("#new_comment_text").val();

        $.ajax({
            method: "POST",
            url: "/Comment/Create",
            data: { "text": txt, "noteid": noteid }
        }).done(function (data) {
            if (data.result) {
                // yorumlar partial tekrar yüklenir
                $(modalCommentBodyId).load("/Comment/ShowNoteComments/" + noteid);
            }
            else {
                alert("Yorum Eklenemedi");
            }
        }).fail(function () {

            alert("Sunucu ile bağlantı kurulamanadı");
        });
    }
}
