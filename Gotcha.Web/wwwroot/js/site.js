"use strict";

let navLogoClickBlocked = false;

window.addEventListener("load", initialize);

function initialize() {
    const figNavLogoContainer = document.getElementsByClassName("navLogoContainer")[0];
    const menuContainer = document.querySelector(".menuContainer");

    menuContainer.addEventListener("click", function () {
        this.classList.toggle("change");
    });

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

    if (window.innerWidth >= 600) {
        widthToApply = "40vw";
    }

    if (window.innerWidth >= 800) {
        widthToApply = "38vw";
    }

    if (window.innerWidth >= 1100) {
        widthToApply = "30vw";
    }

    if (window.innerWidth >= 1200) {
        widthToApply = "26vw";
    }

    if (window.innerWidth >= 1300) {
        widthToApply = "23.5vw";
    }

    if (window.innerWidth >= 1300) {
        widthToApply = "22vw";
    }

    if (window.innerWidth >= 1700) {
        widthToApply = "20vw";
    }

    divNavLogoBackground.style.width = widthToApply;

    setTimeout(function () {
        figCapNavImgCaption.toggleOpacity();
    }, 1000);

    setTimeout(function () {
        navLogoClickBlocked = false;
    }, 1500);
}

function slideInNavImage() {
    const figCapNavImgCaption = document.getElementsByClassName("navLogoText")[0];
    const divNavLogoBackground = document.getElementsByClassName("navLogoBackground")[0];
    const imgNavLogo = document.getElementsByClassName("navLogoImage")[0];

    figCapNavImgCaption.toggleOpacity();

    setTimeout(function () {
        divNavLogoBackground.style.width = '';
        imgNavLogo.toggleRotation();
    }, 1000);

    setTimeout(function () {
        navLogoClickBlocked = false;
    }, 1500);
}


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
HTMLElement.prototype.toggleDisplayViaBootstrap = function()
{
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