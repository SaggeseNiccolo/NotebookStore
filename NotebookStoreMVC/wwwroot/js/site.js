﻿// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

console.log('Script loaded');
class HighlightEffect {
  
  constructor(el) {
    // Validates the input element to ensure it's an HTML element.
    if (!el || !(el instanceof HTMLElement)) {
      throw new Error('Invalid element provided.');
    }
    
    this.highlightedElement = el;
    this.flipElementWrap = this.highlightedElement.closest('.content__inner').querySelector('.hx-flip');
    this.flipElement = this.flipElementWrap.querySelector('.hx-flip__inner');
    this.paragraph = this.highlightedElement.parentNode;
    // Calls the method to set up the initial effect.
    this.initializeEffect(this.wrapElement);
  }
  
  // Sets up the initial effect on the provided element.
  initializeEffect(element) {
    // Scroll effect.
    this.scroll();
  }

  

  // Defines the scroll effect logic for the element.
  scroll() {
    // Temporarily capture the final state
    gsap.set(this.flipElement, {willChange: 'filter', filter: 'blur(0px)'});
    this.highlightedElement.appendChild(this.flipElement);
    const flipstate = Flip.getState([this.flipElementWrap, this.flipElement], {props: 'font-size, filter'} );
    // Back to the original state
    gsap.set(this.flipElement, {filter: 'blur(6px)'});
    this.highlightedElement.removeChild(this.flipElement);
    this.flipElementWrap.appendChild(this.flipElement);

    const scrollTrigger = {
      trigger: this.flipElement,
      start: 'bottom bottom',
      end: '+=100%',
      scrub: true
    };
    
    // Create the Flip animation
    Flip.to(flipstate, {
      ease: 'sine.inOut',
      absoluteOnLeave: true,
      scrollTrigger: scrollTrigger
    })
    .fromTo(this.paragraph, {
      scale: 0.9,
      filter: 'blur(3px)',
      willChange: 'filter'
    }, {
      ease: 'sine.inOut',
      scale: 1,
      filter: 'blur(0px)',
      scrollTrigger: scrollTrigger
    }, 0);
  }
}
// Write your JavaScript code.
// import HighlightEffect from './highlightEffect';
// Registers the ScrollTrigger and Flip plugins with GSAP
gsap.registerPlugin(ScrollTrigger, Flip);

const highlightedElements = document.querySelectorAll('.hx');
highlightedElements.forEach(el => {
  // Exclude the 11th example (Flip example) by checking if the element has the class 'hx-11'
  if ( !el.classList.contains('hx-11') ) {
    el.dataset.splitting = '';
  }
});
Splitting();

const init = () => {
  const effects = [
    { selector: '.hx-11', effect: HighlightEffect },
  ];

  // Iterate over each effect configuration and apply the effect to all matching elements
  effects.forEach(({ selector, effect }) => {
    document.querySelectorAll(selector).forEach(el => {
      new effect(el);
    });
  });
};

init();