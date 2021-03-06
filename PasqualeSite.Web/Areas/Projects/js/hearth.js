var results = [];
var resultsCount = 0
var fullResults = [];
var currentDeck = [];
var currentPage = null;
var deckClass = "";
var lastResult = "";
var query = "";
var maxPages = 0;
var PERPAGE = 6;

// Anything within this block of code runs once the DOM is ready. 
$(document).ready(function() {	
	$('#search').autocomplete({
		source: function (request, response) {
			$.ajax({
				url: "https://omgvamp-hearthstone-v1.p.mashape.com/cards/search/" + request.term,
				dataType: "json",
				headers: {
					"X-Mashape-Key" : "LVW5A36fzNmshCkr69aAwNfYpLwmp1axJZQjsnjfPDjqTVIv9u"
				},
				data: {
					collectible: 1
				},
				success: function (data) {
					response($.map(data, function (item) {
						if ((item.type == "Minion" || item.type == "Spell") && item.name.toUpperCase().startsWith(request.term.toUpperCase())) {
							return {
								label: item.name,
								value: item.name 
							} 
						}
						else {
							return null;
						}
					}))
				}
			});
		}
	});
});		

function getCardByName(searchName) {
    var url = searchName && searchName != "" ? "https://omgvamp-hearthstone-v1.p.mashape.com/cards/search/" + searchName : "https://omgvamp-hearthstone-v1.p.mashape.com/cards";
	$.ajax({
		  url: url,
		  headers: {
			"X-Mashape-Key" : "LVW5A36fzNmshCkr69aAwNfYpLwmp1axJZQjsnjfPDjqTVIv9u"
		  }, // Headers carry information along with the HTTP Request. Some requests require specific headers in order to work. In this case, we are sending an API Key so they can track who uses the API. We probably would want this part to be done in server-side code, as this method exposes our key.
		  dataType: 'json', // May not be required, typically this can be implied but it's good to explicitly note what kind of response you expect back (e.g. json, xml, html, script, etc.)
		  type: 'GET', // This is the default value. 
		  beforeSend: function( xhr ) {
			$('#resultSet').empty();
			$('#resultSet').html('<img class="img-responsive img-center" src="' + approot + 'img/loading.gif" />');
		  },
		  data: {
			collectible: 1
		  },
		  success: function (data) {		      
		      onSearchSuccess(searchName, data);
		  },
		  error: function(xhr, options, error) { onError(xhr, options, error); }
	});		
}

function SubmitSearch(refresh) {
    var $searchValue = $('#search').val().trim() != "" ? $('#search').val().trim() : "";
    query = $searchValue;
    $("#search").blur();
    getCardByName($searchValue);
}

function onSearchSuccess(search, data, page) {
    clearValidator();
    currentPage = page ? page : 1;
    results.length = 0; //clears result array	
    fullResults.length = 0; // clears full results
	resultsCount = data.length;
	$.each(data, function (i, value) {
	    if ($.isArray(value)) {
	        $.each(value, function (i, value2) {
	            if ((deckClass == "" || !value2.playerClass || deckClass.toUpperCase() == value2.playerClass.toUpperCase()) && typeof (value2.img) != 'undefined' && value2.img != null && (value2.type == "Minion" || value2.type == "Spell") && value2.name.toUpperCase().startsWith(search.toUpperCase())) {
	                fullResults.push(value2);
	            }
	        });
	    }
	    else {
	        if ((deckClass == "" || !value.playerClass || deckClass.toUpperCase() == value.playerClass.toUpperCase()) && typeof (value.img) != 'undefined' && value.img != null && (value.type == "Minion" || value.type == "Spell") && value.name.toUpperCase().startsWith(search.toUpperCase())) {
	            fullResults.push(value);
	        }
	    }
	    	
	});

	maxPages = Math.ceil(fullResults.length / PERPAGE);
	fillPage();
}

function incrementPage() {
    if (currentPage < maxPages) {        
        currentPage++;
        fillPage();
    }    
}

function decrementPage() {
    if (currentPage > 1) {
        currentPage--;
        fillPage();
    }   
}

function fillPage() {
    var counter = 1;
    var previousString = '<li class="paging"><a onclick="decrementPage()" data-original-title="Prev" title="">Prev</a></li>';
    var nextString = '<li class="paging"><a onclick="incrementPage()" data-original-title="Next" title="">Next</a></li>';

    $('.pagination').empty();
    if (currentPage > 1) {
        $('.pagination').append(previousString);
    }

    if (currentPage < maxPages) {
        $('.pagination').append(nextString);
    }
    
    $('#resultSet').empty();
    var startCard = ((currentPage - 1) * PERPAGE + 1);
    $.each(fullResults, function (i, value) {
        if ((i+1) >= startCard && counter <= PERPAGE) {
            results.push(value);
            var card = "<div class='flex-item'><img id='" + value.cardId + "' onclick=\'addCard(" + (results.length - 1) + ")\' class='flex-img hvr-bob' src='" + value.img + "' /></div>";
            $('#resultSet').append(card);
            counter++;
        }                   
    });
}

function addCard(arrayIndex) {
	clearValidator()
	if (deckClass.trim() === "") {
		writeValidator("You must select a class before adding cards to your deck.", "warn", "bottom-center", "#deckClass");
		return;
	}
	
	if (currentDeck.length >= 30) {
		writeValidator("You have exceeded the maximum number of cards allowable the deck.", "warn", "left", "#deckCount");
		return;
	}
	
	var newCard = results[arrayIndex];
	var standardCount = 0;
	var legendaryCount = 0;
	$.each(currentDeck, function(i, value) {
		if (newCard.cardId == value.cardId && value.rarity == "Legendary") {
			legendaryCount++;
		}
		else if (newCard.cardId == value.cardId) {
			standardCount++;
		}
	});
	
	if (typeof newCard.playerClass !== 'undefined' && newCard.playerClass.toUpperCase() != deckClass.toUpperCase()) {
		writeValidator("Your cannot add " + newCard.name + " to your deck as it is specific to the " + newCard.playerClass + " class.", "warn")
	}
	
	else if (legendaryCount >= 1 || standardCount >= 2) {
		writeValidator("You have exceeded the number of " + newCard.name + " cards you can add to your deck.", "warn")	
	}
	
	else {
		currentDeck.push(newCard)
		currentDeck = sortDeckByCost(currentDeck);
		refreshDeck(currentDeck);
	}
}

function onError(xhr, options, error) {
	$('#resultSet').empty();
	$('#resultSet').html('<p>' + xhr.responseJSON.message + '</p>')	
}

function updateClass() {	
	if (deckClass !== "" && currentDeck.length > 0) {
		var updated = confirm("Are you sure you want to update your class? Updating now will remove all class specific cards from your deck.");
		if (updated) {
			SubmitSearch();
			for (var i = currentDeck.length - 1; i >= 0; i--) {
				if(typeof currentDeck[i].playerClass !== 'undefined' && currentDeck[i].playerClass.toUpperCase() === deckClass.toUpperCase()) {
				   currentDeck.splice(i, 1);
				}
			}
			deckClass = $('#deckClass').val();
			if (deckClass.trim() === "") {
				currentDeck.length = 0; // Clear deck completely if they select the select a deck option.
			}
			refreshDeck(currentDeck);
		}
		
		else {
			$('#deckClass').val(deckClass);
		}
	}
	
	else {
		SubmitSearch(true);
		deckClass = $('#deckClass').val();
	}
	
	var $filter = $('#classFilter');
	if (deckClass.trim() != "") {	
		var formattedDeckClass = deckClass[0].toUpperCase() + deckClass.slice(1)
		$filter.show();
		$filter.html("Showing cards for " + formattedDeckClass + " class.");
	}
	
	else {
		$filter.hide();
	}
	
}

function sortDeckByCost(deck) {
    deck.sort(function (a, b) {
        if (a.cost === b.cost) {
            // If cost is same, sort by name
            if (a.name < b.name)
                return -1;
            else if (a.name > b.name)
                return 1;
            else
                return 0;
        }
        else {
            if (a.cost < b.cost)
                return -1;
            else if (a.cost > b.cost)
                return 1;
            else
                return 0;
        }

    });
    return deck;
}

function refreshDeck(deck) {
	$('#cardsInDeck').empty();
	var deckToDisplay = sortDeckByCost(deck);
	for (var i = 0; i < deckToDisplay.length; i++) {
		if (typeof deckToDisplay[i+1] != 'undefined' && deckToDisplay[i].cardId == deckToDisplay[i+1].cardId) {
		    var card = "<p class='card' onclick='removeCard(\"" + deckToDisplay[i].cardId + "\")'>" + deckToDisplay[i].cost + "<img src='" + approot + "img/manacrystal.png'/>" + deckToDisplay[i].name + " <span class='label label-primary'>2</span></p><hr />";
			i++;
		}
		else {
		    var card = "<p class='card' onclick='removeCard(\"" + deckToDisplay[i].cardId + "\")'>" + deckToDisplay[i].cost + "<img src='" + approot + "img/manacrystal.png'/>" + deckToDisplay[i].name + "</p><hr />";
		}		
		$('#cardsInDeck').append(card);	
	}
	$('#deckCount').text("(" + deckToDisplay.length + "/30)")
}

function removeCard(cardId) {
    for (var i = 0; i < currentDeck.length; i++) {
        if (currentDeck[i].cardId == cardId) {
            currentDeck.splice(i, 1);          
            break;
        }
    }

    refreshDeck(currentDeck);
}

function writeValidator(message, type, position, element) {
    var options = {
        autoHideDelay: 2500,


    }
	position = position == null || typeof(position) == 'undefined' ? "top-right" : position;
	type = type == null || typeof (position) == 'undefined' ? "success" : type;

	if (element == null || typeof (element) == 'undefined') {
		$.notify(message, type);
	}

	else {
		$(element).notify(message, {position: position, className: type})
		$('html, body').animate({
			scrollTop: $(element).offset().top - 50
		}, 300);
	}  
}

function clearValidator() {
	$('#validation').empty();
}

function exportDeck() {
    if (typeof currentDeck != 'undefined' && currentDeck != null && currentDeck.length > 0) {
        $.ajax({
            url: approot + "Projects/Fun/ExportDeck",
            type: 'POST',
            data: { cards: currentDeck },
            success: function (data) {
                var formattedDeckClass = deckClass[0].toUpperCase() + deckClass.slice(1)
                var filename = "My " + formattedDeckClass + " Deck.csv"
                var blob = new Blob([data], { type: 'text/csv;charset=utf-8;' });
                if (navigator.msSaveBlob) { // IE 10+
                    navigator.msSaveBlob(blob, filename);
                } else {
                    var link = document.createElement("a");
                    if (link.download !== undefined) { // feature detection
                        // Browsers that support HTML5 download attribute
                        var url = URL.createObjectURL(blob);
                        link.setAttribute("href", url);
                        link.setAttribute("download", filename);
                        link.style.visibility = 'hidden';
                        document.body.appendChild(link);
                        link.click();
                        document.body.removeChild(link);
                    }
                }
            },
            error: function () {

            }
        })
    }

    else {
        writeValidator("Your deck is empty. There's nothing to export", "info")
    }
    
}