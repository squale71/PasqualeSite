$( document ).ready(function() {
	var APIKey = 'AYGIBWRIBOTV5EEX0';
	var tableHeading = "<tr><th>#</th><th>Artist</th><th>Title</th></tr>"
	
	var URLtag = 'http://developer.echonest.com/api/v4/playlist/static?api_key=' + APIKey;
    $('#search-button').click(function() {
		$('#music-list').empty();		
		var type = '';
		var criteria = $('#criteria').val();
		var $searchField = $('#search-item').val();
		switch (criteria) {
			case 'artist':  // If user wants to get a playlist based on searched artist
				type = 'artist-radio';
				$.ajax({
					beforeSend: function() {
						$('#music-list').append('<img src="Content/ajax-loader.gif" alt="This will display an animated GIF" />')
					},
					type: "GET",
					url: URLtag,
					data: { artist: $searchField, type: type },
					dataType: 'json',
				})
				.done(function (data) {	
					$('#music-list').empty();
					var songsArray = data.response.songs;
					if (typeof songsArray !== 'undefined') {
						$('#music-list').append(tableHeading);
						$(songsArray).each(function(i, song) {							
							$('#music-list').append('<tr><td>' + (i+1) + '</td><td>' + song.artist_name + '</td><td>' + song.title + '</td></tr>')
						});
					}
					else {
						$('#music-list').append("No results found");
					}
				})
				.error(function (data) {
					$('#music-list').empty();
					$('#music-list').append("No results found");
				});
				break;
			case 'genre':  // If user wants to get a playlist based on searched genre
				type = 'genre-radio';
				$.ajax({
					beforeSend: function() {
						$('#music-list').append('<img src="Content/ajax-loader.gif" alt="This will display an animated GIF" />')
					},
					type: "GET",
					url: URLtag,
					data: { genre: $searchField, type: type },
					dataType: 'json',
				})
				.done(function (data) {
					$('#music-list').empty();
					var songsArray = data.response.songs;
					if (typeof songsArray !== 'undefined') {
						$('#music-list').append(tableHeading);
						$(songsArray).each(function(i, song) {
							$('#music-list').append('<tr><td>' + (i+1) + '</td><td>' + song.artist_name + '</td><td>' + song.title + '</td></tr>')
						});
					}
					else {
						$('#music-list').append("No results found");
					}
				})
				.error(function (data) {
					$('#music-list').empty();
					$('#music-list').append("No results found");
				});
				break;		
		}
	});
});