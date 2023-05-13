

$(function () {

    var makaledizi = [];

    $("div[data-makaleid]").each(function (i,e) {//i=indeks e=element
        makaledizi.push($(e).data("makaleid"));

        //1,5,13,15,35,19
        //önüme bu diziler geldi bu makalelerden hangisini begendim dizinin içine bunları atıyor
        $.ajax({
            method: "POST",
            url: "/Makale/MakaleGetir",
            data: { mid: makaledizi }
        }).done(function (sonuc) { 

            if (sonuc.liste !=null && sonuc.liste.length>0)  //burada boş olan kalpleri dolduracağız.
            {
                for (var i = 0; i < sonuc.liste.length; i++)
                {
                    var id = sonuc.liste[i];
                    var btn = $("button[data-mid=" + id + "]");
                    btn.data("like", true);//like false du true ya çevirdik
                    var span = btn.find("span.like-kalp");
                    span.removeClass("glyphicon-heart-empty");
                    span.addClass("glyphicon-heart");

                }
            }

        }).fail(function(){
            alert("Sunucu ile Bağlantı kurulamadı");
        });

    });
});