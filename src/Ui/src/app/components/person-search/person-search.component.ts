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

  numericOnly(event: Event): void {
    const input = event.target as HTMLInputElement;
    input.value = input.value.replace(/[^0-9]/g, '');
    this.name = input.value;
  }

  search(e: KeyboardEvent): void {
    e.preventDefault();
    if (this.name && this.name.length >= 5) {
      this.personService.PersonSearch(this.name)
        .pipe(takeUntil(this.destroy$))
        .subscribe(res => {
          this.persons = res?.data?.persons || [];
        });
    } else {
      this.persons = [];
    }
  }

  selectPerson(p: any): void {
    this.selectedPerson = p;
    this.returnListEvent.emit(JSON.stringify(this.selectedPerson));
    this.persons = this.persons.filter(person => person.citizenshipNumber === this.selectedPerson.citizenshipNumber);
    this.name = '';
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }
}
