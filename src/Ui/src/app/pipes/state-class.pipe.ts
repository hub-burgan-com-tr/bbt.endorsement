import {Pipe, PipeTransform} from '@angular/core';

@Pipe({
  name: 'stateClass'
})
export class StateClassPipe implements PipeTransform {

  constructor() {
  }

  transform(state) {
    switch (state) {
      case 'Pending':
        return 'warning';
      case 'Cancel':
        return 'danger';
      case 'Approve':
        return 'success';
      case 'Timeout':
        return 'danger';
      case 'Reject':
        return 'danger';
      default:
        return '';
    }
  }

}
