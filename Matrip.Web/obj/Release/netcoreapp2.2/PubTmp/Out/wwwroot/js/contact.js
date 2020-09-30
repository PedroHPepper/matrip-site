/* JS Document */

/******************************

[Table of Contents]

1. Vars and Inits
2. Set Header
3. Init Menu
4. Init Google Map


******************************/

jQuery(document).ready(function()
{
	"use strict";

	/* 

	1. Vars and Inits

	*/

	var header = jQuery('.header');
	var map;

	setHeader();

	jQuery(window).on('resize', function()
	{
		setHeader();

		setTimeout(function()
		{
			jQuery(window).trigger('resize.px.parallax');
		}, 375);
	});

	jQuery(document).on('scroll', function()
	{
		setHeader();
	});

	initMenu();
	initGoogleMap();

	/* 

	2. Set Header

	*/

	function setHeader()
	{
		if(jQuery(window).scrollTop() > 91)
		{
			header.addClass('scrolled');
		}
		else
		{
			header.removeClass('scrolled');
		}
	}

	/* 

	3. Init Menu

	*/

	function initMenu()
	{
		if(jQuery('.menu').length)
		{
			var header = jQuery('.header');
			var hOverlay = jQuery('.header_overlay');
			var menu = jQuery('.menu');
			var hamb = jQuery('.hamburger');
			var sup = jQuery('.super_container_inner');
			var close = jQuery('.menu_close');
			var overlay = jQuery('.super_overlay');

			hamb.on('click', function()
			{
				header.toggleClass('active');
				sup.toggleClass('active');
				menu.toggleClass('active');
			});

			close.on('click', function()
			{
				header.toggleClass('active');
				sup.toggleClass('active');
				menu.toggleClass('active');
			});

			overlay.on('click', function()
			{
				header.toggleClass('active');
				sup.toggleClass('active');
				menu.toggleClass('active');
			});

			hOverlay.on('click', function()
			{
				header.toggleClass('active');
				sup.toggleClass('active');
				menu.toggleClass('active');
			});
		}
	}

	/* 

	4. Init Google Map

	*/

	function initGoogleMap()
	{
		var myLatlng = new google.maps.LatLng(47.495962, 19.050966);
    	var mapOptions = 
    	{
    		center: myLatlng,
	       	zoom: 14,
			mapTypeId: google.maps.MapTypeId.ROADMAP,
			draggable: true,
			scrollwheel: false,
			zoomControl: true,
			zoomControlOptions:
			{
				position: google.maps.ControlPosition.RIGHT_CENTER
			},
			mapTypeControl: false,
			scaleControl: false,
			streetViewControl: false,
			rotateControl: false,
			fullscreenControl: true,
			styles:
			[
			  {
			    "featureType": "road.highway",
			    "elementType": "geometry.fill",
			    "stylers": [
			      {
			        "color": "#ffeba1"
			      }
			    ]
			  }
			]
    	}

    	// Initialize a map with options
    	map = new google.maps.Map(document.getElementById('map'), mapOptions);

		// Re-center map after window resize
		google.maps.event.addDomListener(window, 'resize', function()
		{
			setTimeout(function()
			{
				google.maps.event.trigger(map, "resize");
				map.setCenter(myLatlng);
			}, 1400);
		});
	}

});