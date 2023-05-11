
var mid = -1;
$(function () {

    $('#modal1').on('show.bs.modal', function (e) {

        var btn = $(e.relatedTarget); //butonda elementi yakalmış olduyorum.Tıklanılanı yakalmak için relatedTarget kullanılır.
        mid = btn.data("makaleid");

        $("#modal1_body").load("/Yorum/YorumGoster/" + mid);
    });

});
    function islemyap(btn, islem, yorumid, spanid)
    {
        var button = $(btn);
        var durum = button.data("editmod");

        if (islem == "update") {
            if (durum == false) {
                button.data("editmod", true);
                button.removeClass("btn-warning");
                button.addClass("btn-info");

                var span = button.find("span");
                span.removeClass("glyphicon-edit");
                span.addClass("glyphicon-ok");

                $(spanid).attr("contenteditable", true);
                $(spanid).focus();
            }

            else {
                button.data("editmod", false);
                button.removeClass("btn-info");
                button.addClass("btn-warning");

                var span = button.find("span");
                span.removeClass("glyphicon-ok");
                span.addClass("glyphicon-edit");

                $(spanid).attr("contenteditable", false);
                var yorum = $(spanid).text();


                //update etmek için ajax metotu gerçekleştiricez
                $.ajax({
                    method: "POST",
                    url: "/Yorum/YorumGuncelle/" + yorumid,
                    data: { text: yorum }
                }).done(function (sonuc) {
                    if (sonuc.hata) {
                        alert("Yorum Güncellenemedi");
                    }
                    else {
                        //yorumlar tekrar yüklenir.
                        $("#modal1_body").load("/Yorum/YorumGoster/" + mid);
                    }
                }).fail(function () {
                    alert("Sunucu ile bağlantı kurulamadı")
                });


            }

        }
        else if (islem == "delete") {
            var onay = confirm("Yorum silinsin mi?");
            if (!onay) {
                return false;
            }
            $.ajax({
                method: "GET",
                url: "/Yorum/YorumSil/" + yorumid

            }).done(function (sonuc) {
                if (sonuc.hata) {
                    alert("Yorum Silinemedi.")
                }
                else {
                    //Yorumlar tekrar yüklensin.
                    $("#modal1_body").load("/Yorum/YorumGoster/" + mid);
                }

            }).fail(function () {
                alert("Sunucu ile Bağlantı Kurulamadı.")
            });
        }

        else if (islem == "insert")
        {
            var yorum = $("#yorum_text").val();

            $.ajax({
                method: "POST",
                url: "/Yorum/YorumEkle",
                data: { Text: yorum, id: mid }
            }).done(function (sonuc) {
                if (sonuc.hata) {
                    alert("Yorum Eklenemedi");
                }
                else {
                    //yorumlar tekrar yüklenir.
                    $("#modal1_body").load("/Yorum/YorumGoster/" + mid);
                }

            }).fail(function () {
                alert("Sunucu ile bağlantı kurulamadı")
            });
      
        }

    }
