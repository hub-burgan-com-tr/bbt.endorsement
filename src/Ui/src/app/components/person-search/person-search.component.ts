import {Component, EventEmitter, Input, OnInit, Output, SimpleChanges} from '@angular/core';
import {PersonService} from "../../services/person.service";
import {Subject, takeUntil} from "rxjs";

@Component({
  selector: 'app-person-search',
  templateUrl: './person-search.component.html',
  styleUrls: ['./person-search.component.scss']
})
export class PersonSearchComponent implements OnInit {
  name;
  @Input() hasError;
  @Output() returnListEvent = new EventEmitter<string>();
  private destroy$: Subject<any> = new Subject<any>();
  persons;
  selectedPerson = {
    first: '',
    last: '',
    citizenshipNumber: '',
    customerNumber: '',
    businessLine: '',
    branchCode: ''
  };

  constructor(private personService: PersonService) {
  }

  ngOnInit(): void {
  }

  search(e?: any) {
    const value = e?.target?.value || this.name;
    if (value && value.length >= 5) {
      this.personService.PersonSearch(value)
        .pipe(takeUntil(this.destroy$))
        .subscribe(res => {
          this.persons = res?.data?.persons;
        });
    }
  }
  

  selectPerson(p) {
    this.selectedPerson = p;
    this.returnListEvent.emit(JSON.stringify(this.selectedPerson));
    this.persons = this.persons.filter(p => p.citizenshipNumber == this.selectedPerson.citizenshipNumber);
    this.name = '';
  }
  onInputChange(event: any) {
    const input = event.target;
    const sanitizedValue = input.value.replace(/[^0-9]/g, '');
    input.value = sanitizedValue;
    this.name = sanitizedValue;
  }
  
}
