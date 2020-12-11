var comment = {
    init: function () {
        comment.registerEvents();
    },
    registerEvents: function () {
        $('.btn-add-comment').click(function () {
            var txt = $(".form-input").val();
            if (txt.length == 0) {
                alert("Nội dung bị trống");
                return;
            }
            var id = $(this).data('id');
            $.ajax({
                url: "/CongViecs/AddComment",
                data: { macv: id, noidung: txt },
                dataType: "json",
                type: "POST",
                success: function (res) {
                    console.log(res);
                    //var newcomment =+"<div class='be-img-comment'>" +"<a href='blog-detail-2.html'>" +"<img src='https://bootdey.com/img/Content/avatar/avatar1.png'  class='be-ava-comment'>" +"</a>" +"</div>" +"<div class='be-comment-content'>" +"<span class='be-comment-name'>" +"<a>" + res.ten + "</a>" +"</span>" +"<span class='be-comment-time'>" +"<i class='fa fa-clock-o'></i>"  +"</span>" +"<p class='be-comment-text'>" +res.noidung +"</p>" +"</div>" +"</div>";
                    var newcomment = "<div class='be-comment'><div class='be-img-comment'><img src='https://bootdey.com/img/Content/avatar/avatar1.png'  class='be-ava-comment'></div><div class='be-comment-content'><span class='be-comment-name'>" + res.ten + "</span><span class='be-comment-time'><i class='fa fa-clock-o'></i></span><p class='be-comment-text'>" + res.noidung + "</p></div></div>";
                    $(".comments").append(newcomment);

                }
            });

        });
    }
}
comment.init();