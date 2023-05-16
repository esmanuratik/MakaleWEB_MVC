

$(function () {

    var makaledizi = [];

    $("div[data-makaleid]").each(function (i, e) {//i=indeks e=element
        makaledizi.push($(e).data("makaleid"));
    });

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
                    var span = btn.find("span.like-kalp-"+id);
                    span.removeClass("glyphicon-heart-empty");
                    span.addClass("glyphicon-heart");

                }
            }

        }).fail(function(){
            alert("Sunucu ile Bağlantı kurulamadı");
        });



        $("button[data-like]").click(function () {

            var btn = $(this);
            var like = btn.data("like");
            var mid = btn.data("mid");
            var spanlike = $("span.like-kalp-"+mid);
            var likecount = $("span.like-"+mid);
                       
            $.ajax({
                method: "POST",
                url: "/Makale/MakaleBegen",
                data: { makaleid: mid, begeni: !like }
            }).done(function (sonuc) {

                if (sonuc.hata) {
                    alert("Beğeni işlenmi gerçekleştirilemedi");
                }
                else {
                    var begeni = !like;
                    btn.data("like", !like);//begeni true ise false false ise true olsun

                    likecount.text(sonuc.begenisayisi);

                    //ilk hem doluyu hem boşu sildim
                    spanlike.removeClass("glyphicon-heart-empty");
                    spanlike.removeClass("glyphicon-heart");

                    //ikisini kontrol etmek için doluysa boş boşsa dolu olsun.
                    if (begeni) {
                        spanlike.addClass("glyphicon-heart");
                    }
                    else {
                        spanlike.addClass("glyphicon-heart-empty");
                    }

                }
            }).fail(function () {
                alert("Sunucu ile bağlantı kurulamadı");


            });

    });



    });
