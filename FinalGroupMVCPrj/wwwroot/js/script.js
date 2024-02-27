/*-----------------------------------------------------------
 * Template Name    : Oppida - Online Course & Education HTML Template
 * Author           : KreativDev
 * File Description : This file contains the JavaScript for the actual template, this
					  is the file you need to edit to change the functionality of the
					  template.
 *------------------------------------------------------------

 **	This files table contents are outlined below >>>>>

 *******************************************
 *******************************************
 ** - Sticky header
 ** - Mobile menu
 ** - Navlink active class
 ** - Image to background image
 ** - Sidebar toggle
 ** - Toggle List
 ** - Tabs mouse hover animation
 ** - Sliders
 ** - Magic Cursor
 ** - Price range
 ** - Quantity button
 ** - Sidebar scroll
 ** - Password icon toggle
 ** - Youtube popup
 ** - Go to top
 ** - Lazyload image
 ** - Nice select
 ** - Footer date
 ** - Odometer
 ** - Countdown Timer
 ** - Document on ready
 ** - Preloader
 ** - Aos animation
*/

!(function($) {
    "use strict";

    /*============================================
        Sticky header
    ============================================*/
    $(window).on("scroll", function() {
        var header = $(".header-area");
        // If window scroll down .is-sticky class will added to header
        if ($(window).scrollTop() >= 400) {
            header.addClass("is-sticky");
        } else {
            header.removeClass("is-sticky");
        }
    });


    /*============================================
        Mobile menu
    ============================================*/
    var mobileMenu = function() {
        // Variables
        var body = $("body"),
            mainNavbar = $(".main-navbar"),
            mobileNavbar = $(".mobile-menu"),
            cloneInto = $(".mobile-menu-wrapper"),
            cloneItem = $(".mobile-item"),
            menuToggler = $(".menu-toggler"),
            offCanvasMenu = $("#offcanvasMenu")

        menuToggler.on("click", function() {
            $(this).toggleClass("active");
            body.toggleClass("mobile-menu-active")
        })

        mainNavbar.find(cloneItem).clone(!0).appendTo(cloneInto);

        if (offCanvasMenu) {
            body.find(offCanvasMenu).clone(!0).appendTo(cloneInto);
        }

        mobileNavbar.find("li").each(function(index) {
            var toggleBtn = $(this).children(".toggle")
            toggleBtn.on("click", function(e) {
                $(this)
                    .parent("li")
                    .children("ul")
                    .stop(true, true)
                    .slideToggle(350);
                $(this).parent("li").toggleClass("show");
            })
        })

        // check browser width in real-time
        var checkBreakpoint = function() {
            var winWidth = window.innerWidth;
            if (winWidth <= 1199) {
                mainNavbar.hide();
                mobileNavbar.show()
            } else {
                mainNavbar.show();
                mobileNavbar.hide()
            }
        }
        checkBreakpoint();

        $(window).on('resize', function() {
            checkBreakpoint();
        });
    }
    mobileMenu();

    var getHeaderHeight = function() {
        var headerNext = $(".header-next");
        var header = headerNext.prev(".header-area");
        var headerHeight = header.height();

        headerNext.css({
            "margin-top": headerHeight
        })
    }
    getHeaderHeight();

    $(window).on('resize', function() {
        getHeaderHeight();
    });


    /*============================================
            Navlink active class
        ============================================*/
    var a = $("#mainMenu .nav-link"),
        c = window.location;

    for (var i = 0; i < a.length; i++) {
        const el = a[i];

        if (el.href == c) {
            el.classList.add("active");
        }
    }


    /*============================================
        Image to background image
    ============================================*/
    var bgImage = $(".bg-img")
    bgImage.each(function() {
        var el = $(this),
            src = el.attr("data-bg-image");

        el.css({
            "background-image": "url(" + src + ")",
            "background-repeat": "no-repeat"
        });
    });


    /*============================================
        Sidebar toggle
    ============================================*/
    $(".list-dropdown .category-toggle").on("click", function (t) {
        var i = $(this).closest("li"),
            o = i.find("ul").eq(0);

        if (i.hasClass("open")) {
            o.slideUp(300, function () {
                i.removeClass("open")
            })
        } else {
            o.slideDown(300, function () {
                i.addClass("open")
            })
        }
        t.stopPropagation(), t.preventDefault()
    })


    /*============================================
        Toggle List
    ============================================*/
    $("#toggleList").each(function (i) {
        var list = $(this).children();
        var listShow = $(this).data("toggle-show");
        var listShowBtn = $(this).next("[data-toggle-btn]");
        
        if (list.length > listShow) {
            listShowBtn.show()
            list.slice(listShow).toggle(300);
            
            listShowBtn.on("click", function () {
                list.slice(listShow).slideToggle(300);
                $(this).text($(this).text() === "Show Less" ? "Show More" : "Show Less")
            })
        } else {
            listShowBtn.hide()
        }
    })


    /*============================================
        Tabs mouse hover animation
    ============================================*/
    $("[data-hover='fancyHover']").mouseHover();


    /*============================================
        Sliders
    ============================================*/
    // Category Slider all
    $(".category-slider").each(function() {
        var id = $(this).attr("id");
        var slidePerView = $(this).data("slides-per-view");
        var loops = $(this).data("swiper-loop");
        var sliderId = "#" + id;

        // console.log(slidePerView);

        var swiper = new Swiper(sliderId, {
            loop: loops,
            spaceBetween: 24,
            speed: 1000,
            autoplay: {
                delay: 3000,
            },
            slidesPerView: slidePerView,
            pagination: true,

            pagination: {
                el: sliderId + "-pagination",
                clickable: true,
            },

            // Navigation arrows
            navigation: {
                nextEl: sliderId + "-next",
                prevEl: sliderId + "-prev",
            },

            breakpoints: {
                320: {
                    slidesPerView: 1
                },
                576: {
                    slidesPerView: 2
                },
                992: {
                    slidesPerView: 3
                },
                1440: {
                    slidesPerView: slidePerView
                },
            }
        })
    })

    // Testimonial Slider 1
    var testimonialSlider1 = new Swiper("#testimonial-slider-1", {
        speed: 1200,
        spaceBetween: 15,
        slidesPerView: 1,
        loop: true,
        grabCursor: true,

        // Pagination bullets
        pagination: {
            el: "#testimonial-slider-1-pagination",
            clickable: true,
        },

        on: {
            init: function() {
                var pagination = $('#testimonial-slider-1-pagination'),
                    paginationLength = $('#testimonial-slider-1-pagination span'),
                    currentSlide = 1,
                    totalSlide = paginationLength.length.toString().padStart(2, '0');
                pagination.append(`
                        <div class="fraction">
                            <span class='min'></span>
                            <span>/</span>
                            <span class='max'></span>
                        </div>
                    `)

                pagination.find(".min").text('0' + currentSlide)
                pagination.find(".max").text(totalSlide)
            },
        }
    });

    // Course Slider
    $(".course-slider").each(function() {
        var id = $(this).attr("id");
        var slidePerView = $(this).data("slides-per-view");
        var sliderId = "#" + id;

        var swiper = new Swiper(sliderId, {
            loop: true,
            spaceBetween: 24,
            speed: 1000,
            autoplay: {
                delay: 3000,
            },
            slidesPerView: slidePerView,
            pagination: true,

            pagination: {
                el: sliderId + "-pagination",
                clickable: true,
            },

            // Navigation arrows
            navigation: {
                nextEl: sliderId + "-next",
                prevEl: sliderId + "-prev",
            },

            breakpoints: {
                320: {
                    slidesPerView: 1
                },
                576: {
                    slidesPerView: 2
                },
                992: {
                    slidesPerView: 3
                },
                1440: {
                    slidesPerView: slidePerView
                },
            }
        })
    })
    // Course inline slider
    $(".course-inline-slider").each(function() {
        var id = $(this).attr("id");
        var slidePerView = $(this).data("slides-per-view");
        var loops = $(this).data("swiper-loop");
        var sliderId = "#" + id;

        var swiper = new Swiper(sliderId, {
            loop: loops,
            spaceBetween: 24,
            speed: 1000,
            autoplay: {
                delay: 3000,
            },
            slidesPerView: slidePerView,
            pagination: true,

            pagination: {
                el: sliderId + "-pagination",
                clickable: true,
            },

            // Navigation arrows
            navigation: {
                nextEl: sliderId + "-next",
                prevEl: sliderId + "-prev",
            },

            breakpoints: {
                320: {
                    slidesPerView: 1
                },
                576: {
                    slidesPerView: 2
                },
                992: {
                    slidesPerView: 3
                },
                1200: {
                    slidesPerView: slidePerView
                }
            }
        })
    })

    // Stop slider autoplay
    $(document).ready(function() {

        if ($(".swiper").length) {
            var mySwiper = document.querySelector(".swiper").swiper;

            $(".swiper").mouseenter(function() {
                mySwiper.autoplay.stop();
            });

            $(".swiper").mouseleave(function() {
                mySwiper.autoplay.start();
            });
        }
    });


    /*============================================
        Magic Cursor
    ============================================*/
    var cursor = function() {
        // Variables Declaration
        var cursor = $('.cursor');
        if (window.innerWidth > 1199) {
            // Adding cursor effect
            $(window).on('mousemove', function(e) {
                cursor.css({
                    'transform': "translate(" + e.clientX + "px," + e.clientY + "px)"
                })
            })
            // Add hover class
            $('a, button, .cursor-pointer').on('mouseenter', function() {
                cursor.addClass('hover');
            })
            // Remove hover class
            $('a, button, .cursor-pointer').on('mouseleave', function() {
                cursor.removeClass('hover');
            })
        } else {
            cursor.remove();
        }
    }


    /*============================================
        Price range
    ============================================*/
    var filterSliders = document.querySelector("[data-range-slider='filterPriceSlider']");
    var input0 = document.getElementById('min');
    var input1 = document.getElementById('max');
    var inputs = [input0, input1];

    // Filter price slider
    if (filterSliders) {
        noUiSlider.create(filterSliders, {
            start: [200, 500],
            connect: !0,
            step: 10,
            margin: 10,
            range: {
                min: 0,
                max: 1000
            }
        }), filterSliders.noUiSlider.on("update", function(values, handle) {
            $("[data-range-value='filterPriceSliderValue']").text("$" + values.join(" - " + "$"));
            inputs[handle].value = values[handle];
        })
        inputs.forEach(function(input, handle) {
            if (input) {
                input.addEventListener('change', function() {
                    filterSliders.noUiSlider.setHandle(handle, this.value);
                });
            }
        });
    }


    /*============================================
        Quantity button
    ============================================*/
    $(document).on('click', '.quantity-down', function() {
        var numcourse = Number($(this).next().val());
        if (numcourse > 0) $(this).next().val(numcourse - 1);
    });
    $(document).on('click', '.quantity-up', function() {
        var numcourse = Number($(this).prev().val());
        $(this).prev().val(numcourse + 1);
    })


    /*============================================
        Sidebar scroll
    ============================================*/
    $(document).ready(function() {
        $(".widget").each(function() {
            var child = $(this).find(".accordion-body.scroll-y");
            if (child.height() >= 245) {
                child.css({
                    "padding-inline-end": "10px",
                })
            }
        })
    })


    /*============================================
        Password icon toggle
    ============================================*/
    $(".show-password-field").on("click", function() {
        var showIcon = $(this).children(".show-icon");
        var passwordField = $(this).prev("input");
        showIcon.toggleClass("show");
        if (passwordField.attr("type") == "password") {
            passwordField.attr("type", "text")
        } else {
            passwordField.attr("type", "password");
        }
    })


    /*============================================
        Youtube popup
    ============================================*/
    $(".youtube-popup").magnificPopup({
        disableOn: 300,
        type: "iframe",
        mainClass: "mfp-3d-unfold",
        removalDelay: 160,
        preloader: false,
        fixedContentPos: false,
    })


    /*============================================
        Go to top
    ============================================*/
    $(window).on("scroll", function() {
        // If window scroll down .active class will added to go-top
        var goTop = $(".go-top");

        if ($(window).scrollTop() >= 200) {
            goTop.addClass("active");
        } else {
            goTop.removeClass("active")
        }
    })
    $(".go-top").on("click", function(e) {
        $("html, body").animate({
            scrollTop: 0,
        }, 0);
    });


    /*============================================
        Lazyload image
    ============================================*/
    var lazyLoad = function() {
        window.lazySizesConfig = window.lazySizesConfig || {};
        window.lazySizesConfig.loadMode = 2;
        lazySizesConfig.preloadAfterLoad = true;

        var lazyContainer = $(".lazy-container");

        if (lazyContainer.children(".lazyloaded")) {
            lazyContainer.addClass("lazy-active")
        } else {
            lazyContainer.removeClass("lazy-active")
        }
    }


    /*============================================
        Nice select
    ============================================*/
    $(".niceselect").niceSelect();

    var selectList = $(".nice-select .list")
    $(".nice-select .list").each(function() {
        var list = $(this).children();
        if (list.length > 5) {
            $(this).css({
                "height": "160px",
                "overflow-y": "scroll"
            })
        }
    })


    /*============================================
        Footer date
    ============================================*/
    var date = new Date().getFullYear();
    $("#footerDate").text(date);


    /*============================================
        Odometer
    ============================================*/
    if ($(".counter").length) {
        $(".counter").counterUp({
            delay: 10,
            time: 1000
        });
    }


    /*============================================
        Countdown Timer
    ============================================*/
    function makeTimer() {
        var endTime = new Date("May 20, 2024 17:00:00 PDT");
        var endTime = (Date.parse(endTime)) / 1000;
        var now = new Date();
        var now = (Date.parse(now) / 1000);
        var timeLeft = endTime - now;
        var days = Math.floor(timeLeft / 86400);
        var hours = Math.floor((timeLeft - (days * 86400)) / 3600);
        var minutes = Math.floor((timeLeft - (days * 86400) - (hours * 3600)) / 60);
        var seconds = Math.floor((timeLeft - (days * 86400) - (hours * 3600) - (minutes * 60)));
        if (hours < "10") {
            hours = "0" + hours;
        }
        if (minutes < "10") {
            minutes = "0" + minutes;
        }
        if (seconds < "10") {
            seconds = "0" + seconds;
        }
        $("#days .time").html(days);
        $("#hours .time").html(hours);
        $("#minutes .time").html(minutes);
        $("#seconds .time").html(seconds);
    }
    setInterval(function() {
        makeTimer()
    }, 0);


    /*============================================
        Document on ready
    ============================================*/
    $(document).ready(function() {
        lazyLoad();
        cursor()
    })

})(jQuery);

$(window).on("load", function() {
    const delay = 1000;
    /*============================================
        Preloader
    ============================================*/
    $("#preLoader").delay(delay).fadeOut();

    /*============================================
        Aos animation
    ============================================*/
    var aosAnimation = function() {
        AOS.init({
            easing: "ease",
            duration: 1200,
            once: true,
            offset: 60,
            disable: "mobile"
        });
    }
    if ($("#preLoader")) {
        setTimeout(() => {
            aosAnimation()
        }, delay);
    } else {
        aosAnimation();
    }
})
