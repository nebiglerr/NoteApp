$(function () {
    $('#model_notedetail').on('show.bs.modal', function (e) {

        var btn = $(e.relatedTarget); //tıklanan butonu yakalama

        noteid = btn.data("note-id"); // tıklanan buttonun data özelliğindeki note-id ile id yakalandı

        $("#model_notedetail_body").load("/Note/GetNoteText/" + noteid);
    })

});