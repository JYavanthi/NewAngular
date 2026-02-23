// import { Directive, ElementRef, HostListener, Renderer2 } from '@angular/core';

// @Directive({
//   selector: '[appHighlightInput]'
// })
// export class HighlightInputDirective {
//   constructor(private el: ElementRef, private renderer: Renderer2) {}
//   @HostListener('blur') onBlur() {
//     const value = this.el.nativeElement.value.trim();
//     if (!value) {
//       this.renderer.setStyle(this.el.nativeElement, 'outline', '2px solid red');
//     } else {
//       this.renderer.setStyle(this.el.nativeElement, 'outline', 'none');
//     }
//   }
// }


import { Directive, ElementRef, Renderer2, Input, DoCheck } from '@angular/core';
import { AbstractControl } from '@angular/forms';

@Directive({
  selector: '[appHighlightInput]'
})
export class HighlightInputDirective implements DoCheck {
  @Input('appHighlightInput') control!: AbstractControl | null;

  constructor(private el: ElementRef, private renderer: Renderer2) {}

  ngDoCheck() {
    if (this.control && this.control.invalid && (this.control.touched || this.control.dirty)) {
      this.renderer.setStyle(this.el.nativeElement, 'outline', '2px solid red');
    } else {
      this.renderer.setStyle(this.el.nativeElement, 'outline', 'none');
    }
  }
}
