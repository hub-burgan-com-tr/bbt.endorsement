import {Pipe, PipeTransform} from '@angular/core';

@Pipe({
  name: 'state'
})
export class StatePipe implements PipeTransform {

  constructor() {
  }

  transform(state) {
    switch (state) {
      case 'Pending':
        return 'Beklemede';
      case 'Cancel':
        return 'İptal Edildi';
      case 'Approve':
        return 'Onaylandı';
      case 'Timeout':
        return 'Zaman Aşımı';
      case 'Reject':
        return 'Onaylanmadı';
      default:
        return '';
    }
  }

}
