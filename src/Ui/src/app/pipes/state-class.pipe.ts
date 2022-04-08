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
        return 'blue';
      case 'Cancel':
        return 'black';
      case 'Approve':
        return 'success';
      case 'Timeout':
        return 'grey';
      case 'Reject':
        return 'danger';
      default:
        return '';
    }
  }

}
