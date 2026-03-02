"use strict";

let navLogoClickBlocked = false;

window.addEventListener("load", initialize);

function initialize() {
    const figNavLogoContainer = document.getElementsByClassName("navLogoContainer")[0];
    const menuContainer = document.querySelector(".menuContainer");
    const divMenuOpenNav = document.getElementsByClassName("menuOpenNav")[0];
    const divPageToHide = document.getElementsByClassName("pageToHide")[0];

    if (menuContainer) {
        menuContainer.addEventListener("click", function () {
            this.classList.toggle("change");
            divMenuOpenNav.slideToggle(350);
            divPageToHide.toggleDisplay();
        });
    }

    figNavLogoContainer.addEventListener("click", toggleNavImageSlider);
}

function toggleNavImageSlider() {
    if (navLogoClickBlocked) {
        return;
    }

    navLogoClickBlocked = true;

    const figCapNavImgCaption = document.getElementsByClassName("navLogoText")[0];

    if (figCapNavImgCaption.style.opacity == 0 || figCapNavImgCaption.style.opacity == '') {
        slideOutNavImage();
    }
    else {
        slideInNavImage();
    }
}

function slideOutNavImage() {
    const figCapNavImgCaption = document.getElementsByClassName("navLogoText")[0];
    const divNavLogoBackground = document.getElementsByClassName("navLogoBackground")[0];
    const imgNavLogo = document.getElementsByClassName("navLogoImage")[0];

    imgNavLogo.toggleRotation();

    let widthToApply = "45vw";

    if (window.innerWidth >= 910) {
        widthToApply = "21vw";
    }

    if (window.innerWidth >= 1300) {
        widthToApply = "20vw";
    }

    if (window.innerWidth >= 1600) {
        widthToApply = "15.5vw";
    }

    if (window.innerWidth >= 1700) {
        widthToApply = "16vw";
    }

    divNavLogoBackground.style.width = widthToApply;

    setTimeout(function () {
        figCapNavImgCaption.toggleOpacity();
    }, 1000);

    setTimeout(function () {
        divNavLogoBackground.style.boxShadow = "0 0 30px 0 var(--primary-color-blue)";
        navLogoClickBlocked = false;
    }, 2000);
}

function slideInNavImage() {
    const figCapNavImgCaption = document.getElementsByClassName("navLogoText")[0];
    const divNavLogoBackground = document.getElementsByClassName("navLogoBackground")[0];
    const imgNavLogo = document.getElementsByClassName("navLogoImage")[0];

    divNavLogoBackground.style.boxShadow = "";

    figCapNavImgCaption.toggleOpacity();

    setTimeout(function () {
        divNavLogoBackground.style.width = '';
        imgNavLogo.toggleRotation();
    }, 1000);

    setTimeout(function () {
        navLogoClickBlocked = false;
    }, 2000);
}

function slideToggle(element, duration = 300, callback = null) {
    // If already animating → skip or you can force with data attribute
    if (element.classList.contains('sliding')) return;

    const isHidden = !element.offsetHeight || getComputedStyle(element).display === 'none';

    if (isHidden) {
        // 1. Prepare for opening
        element.style.display = 'flex';
        element.style.overflow = 'hidden';
        element.style.height = '0px';
        element.classList.add('sliding');

        // Force reflow so transition works
        element.offsetHeight;

        // 2. Animate to natural height
        const targetHeight = element.scrollHeight + 'px';
        element.style.height = targetHeight;

        // 3. Clean up after animation
        setTimeout(() => {
            element.style.height = '';
            element.style.overflow = '';
            element.classList.remove('sliding');
            if (callback) callback(element);
        }, duration);

    } else {
        // 1. Prepare for closing
        element.style.overflow = 'hidden';
        element.style.height = element.offsetHeight + 'px';
        element.classList.add('sliding');

        // Force reflow
        element.offsetHeight;

        // 2. Collapse
        element.style.height = '0px';

        // 3. Clean up after animation
        setTimeout(() => {
            element.style.display = 'none';
            element.style.height = '';
            element.style.overflow = '';
            element.classList.remove('sliding');
            if (callback) callback(element);
        }, duration);
    }
}

// Optional: very convenient wrapper
HTMLElement.prototype.slideToggle = function (duration = 300, callback) {
    slideToggle(this, duration, callback);
    return this;
};

HTMLElement.prototype.toggleRotation = function () {
    if (!(this.classList.contains('rotateBack')) && !(this.classList.contains('rotateForward')))
    {
        this.classList.add("rotateForward");
    }
    else if (this.classList.contains('rotateBack')) {
        this.classList.remove('rotateBack');
        this.classList.add('rotateForward');
    }
    else
    {
        this.classList.remove('rotateForward');
        this.classList.add('rotateBack');
    }
}

HTMLElement.prototype.toggleDisplayViaBootstrap = function() {
    this.classList.toggle("d-none");
}

HTMLElement.prototype.toggleVisibilityViaBootstrap = function () {
    this.classList.toggle("invisible");
}

HTMLElement.prototype.toggleDisplay = function () {
    if (this.style.display === 'none') {
        this.style.display = '';
    }
    else {
        this.style.display = 'none';
    }
}

HTMLElement.prototype.toggleVisibility = function () {
    if (this.style.visibility === 'hidden' || this.style.visibility == '') {
        this.style.visibility = 'visible';
    }
    else {
        this.style.visibility = 'hidden';
    }
}

HTMLElement.prototype.toggleOpacity = function () {
    if (this.style.opacity === '0' || this.style.opacity === '') {
        this.style.opacity = '1';
    }
    else {
        this.style.opacity = '0';
    }
}