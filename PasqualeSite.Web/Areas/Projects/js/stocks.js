
var stockQuote = {};
var hasList = false;

stockQuote.Quote = function (symbol, callBack) {
    this.symbol = symbol;
    this.callBack = callBack;
    this.source = "http://dev.markitondemand.com/Api/v2/Quote/jsonp"
    this.request();
};

stockQuote.Quote.prototype.request = function () {
    if (this.xhr) {
        this.xhr.abort();
    }

    this.xhr = $.ajax({
        url: this.source,
        data: { symbol: this.symbol },
        dataType: "jsonp",
        success: function (data) {
            var items = [];
            $.each(data, function (i, item) {
                $('#stockDetails').append("<tr id =" + i + "><td><b>" + i + "</b></td><td>" + item + "</td></tr>")
            });
            $('#Status').hide(); //Hides Success message that appears at the beginning of each query.
            hasList = true;
        },
    });
};

function searchStock(key) {
    $('.listHistory').removeClass('selected')
    if (hasList) {
        $('#stockDetails').empty(); //empties previous query.
    };
    var theSymbol = key;
    new stockQuote.Quote(theSymbol, function (result) { });
}

function deleteOldest() {
    console.log(($('#searchHist li').length))
    while (($('#searchHist li').length) > 15) {
        $('#searchHist li:nth-child(1)').remove();
    }
}

$(document).ready(function () {
    $('#submitStock').click(function () {
        var field = $('#stockSymbol').val();
        var trimmed = $.trim(field);
        if (trimmed.length != 0) {
            var theSymbol = $('#stockSymbol').val()
            searchStock(theSymbol);
            if (localStorage.getItem("history") != null) { //if local storage is not empty
                var tempHist = localStorage.getItem("history");
                tempHist += "|" + theSymbol;
                localStorage.setItem("history", tempHist);
                $('#searchHist').append("<li class='listHistory'>" + theSymbol + "</li>");

            }
            else {  //if local storage is empty
                var tempHist = theSymbol;
                localStorage.setItem("history", tempHist);
                $('#searchHist').append("<li class='listHistory'>" + theSymbol + "</li>");
            }
        }

        else {
            $('#stockSymbol').addClass('errorInput');
        }
    });

    $('#clearHist').click(function () {
        localStorage.clear();
        $('#searchHist').empty();
    });

    $('#stockSymbol').focus(function () {
        $(this).removeClass('errorInput');
    })

    if (localStorage.getItem('history') != null) {
        refreshHist();
    }

    function refreshHist() {
        var tempHist = localStorage.getItem('history');
        var tempHistArray = tempHist.split('|');
        for (var i = 0; i < tempHistArray.length; i++) {
            $('#searchHist').append("<li class='listHistory'>" + tempHistArray[i] + "</li>");
        };
        deleteOldest();
    }
});

$(document).on('click', '.listHistory', function () {  
    var theSymbol = $(this).html();  
    searchStock(theSymbol);
    $(this).addClass('selected');
});

$(document).on('click', '#submitStock', function () {
    deleteOldest();
});

$(document).on('mouseenter', '.listHistory', function () {
    $(this).addClass('hoveredList');
});

$(document).on('mouseleave', '.listHistory', function () {
    $(this).removeClass('hoveredList');
});

$(function () {
    $('#stockSymbol')
        .focus()
        .autocomplete({
            source: function (request, response) {
                $.ajax({
                    url: "http://dev.markitondemand.com/api/v2/Lookup/jsonp",
                    dataType: "jsonp",
                    data: {
                        input: request.term
                    },
                    success: function (data) {
						console.log(data);
                        response($.map(data, function (item) {
                            return {
                                label: item.Symbol + ": " + item.Name + " (" + item.Exchange + ")",
                                value: item.Symbol 
                            }
                        }))
                    }
                });
            }
        })
    ;
});

