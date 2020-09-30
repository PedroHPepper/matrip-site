/* JS Document */

/******************************

[Table of Contents]

1. Vars and Inits
2. Set Header
3. Init Menu
4. Init Isotope


******************************/

jQuery(document).ready(function()
{
	"use strict";

	/* 

	1. Vars and Inits

	*/

	var header = jQuery('.header');

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
	initIsotope();

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

	4. Init Isotope

	*/

	function initIsotope()
	{
		if(jQuery('.blog_posts').length)
		{
			var grid = jQuery('.blog_posts');
			grid.isotope(
			{
				itemSelector:'.blog_post'
			});
		}
	}

});