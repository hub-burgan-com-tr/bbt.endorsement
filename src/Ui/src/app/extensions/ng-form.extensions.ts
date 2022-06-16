import {Form, NgForm} from '@angular/forms';

declare module "@angular/forms/" {
  interface NgForm {
    resetValidation(this: Form): void;
  }
}

NgForm.prototype.resetValidation = function (this: NgForm) {
  Object.keys(this.controls).forEach((key) => {
    const control = this.controls[key];
    control.markAsPristine();
    control.markAsUntouched();
  });
}
