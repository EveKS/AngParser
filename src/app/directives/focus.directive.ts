import { Directive, ElementRef, Renderer, OnInit } from "@angular/core";

@Directive({ selector: '[tmFocus]' })
export class myFocus implements OnInit {
  constructor(private _el: ElementRef, private _renderer: Renderer) {
  }

  ngOnInit() {
    this._renderer.invokeElementMethod(this._el.nativeElement, 'focus', []);
  }
}
