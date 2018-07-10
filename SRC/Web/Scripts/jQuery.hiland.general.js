(function ($) {
    /**/
    $.fn.quickAlign = function () {
        var max = 0;
        this.each(function () {
            if ($(this).width() > max) {
                max = $(this).width();
            }
        });
        this.width(max);
    };

    /*1.此"实例"方法可以指定某(些)ul产生效果*/
    $.fn.effectListItem = function (options) {
        var opts = $.extend({}, $.fn.effectListItem.defaults, options);

        this.each(function () {
            var thisContainer = $(this);
            //添加奇偶行颜色 
            $(thisContainer).find("li:odd").css("backgroundColor", opts.oddColor);
            $(thisContainer).find("li:even").css("backgroundColor", opts.evenColor);
            //添加活动行颜色 
            var originalBackGroundColor;
            $(thisContainer).find("li").bind("mouseover", function () {
                originalBackGroundColor = $(this).css("backgroundColor");
                $(this).css("backgroundColor", opts.overColor);
            });
            $(thisContainer).find("li").bind("mouseout", function () {
                $(this).css("backgroundColor", originalBackGroundColor);
            });
        });
    };

    $.fn.effectListItem.defaults = {
        oddColor: "#C4E1FF",
        evenColor: "#F2F9FD",
        overColor: "#C7C7E2",
        selColor: "#336666"
    };



    /*2.此"静态"方法将会在页面上的所有ul上产生效果*/
    $.extend({
        effectListItem: function (options) {
            var opts = $.extend({}, $.fn.effectListItem.defaults, options);
            $.fn.effectListItem.options = opts;

            //奇偶异色
            $("li:odd").css("backgroundColor", opts.oddColor);
            $("li:even").css("backgroundColor", opts.evenColor);

            //MouseOver
            var originalBackGroundColor;
            $("li").mouseover(function () {
                originalBackGroundColor = $(this).css("backgroundColor");
                $(this).css("backgroundColor", opts.overColor);
            });

            //MouseOut
            $("li").mouseout(function () {
                $(this).css("backgroundColor", originalBackGroundColor);
            });
        }
    });

    /*1.此"实例"方法可以指定某(些)table产生效果*/
    $.fn.effectRow = function (options) {
        var opts = $.extend({}, $.fn.effectRow.defaults, options);

        this.each(function () {
            var thisTable = $(this);
            //添加奇偶行颜色 
            $(thisTable).find('tr:odd > td').css('backgroundColor', opts.oddColor);
            $(thisTable).find('tr:even > td').css('backgroundColor', opts.evenColor);
            //添加活动行颜色 
            var originalBackGroundColor;
            $(thisTable).find("tr").bind("mouseover", function () {
                originalBackGroundColor = $(this).find('td').css('backgroundColor');
                $(this).find('td').css('backgroundColor', opts.overColor);
            });
            $(thisTable).find("tr").bind("mouseout", function () {
                $(this).find('td').css('backgroundColor', originalBackGroundColor);
            });
        });
    };

    $.fn.effectRow.defaults = {
        oddColor: '#C4E1FF',
        evenColor: '#F2F9FD',
        overColor: '#C7C7E2',
        selColor: '#336666'
    };



    /*2.此"静态"方法将会在页面上的所有table上产生效果*/
    $.extend({
        effectRow: function (options) {
            var opts = $.extend({}, $.fn.effectRow.defaults, options);
            $.fn.effectRow.options = opts;

            //奇偶异色
            $('tr:odd > td').css('backgroundColor', opts.oddColor);
            $('tr:even > td').css('backgroundColor', opts.evenColor);

            //MouseOver
            var originalBackGroundColor;
            $("tr").mouseover(function () {
                originalBackGroundColor = $(this).find('td').css('backgroundColor');
                $(this).find('td').css('backgroundColor', opts.overColor);
            });

            //MouseOut
            $("tr").mouseout(function () {
                $(this).find('td').css('backgroundColor', originalBackGroundColor);
            });
        }
    });



})(jQuery)