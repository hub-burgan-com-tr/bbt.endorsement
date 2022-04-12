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
  @Output() returnListEvent = new EventEmitter<string>();
  private destroy$: Subject<any> = new Subject<any>();
  persons;
  selectedPerson = {
    first: '',
    last: '',
    citizenshipNumber: ''
  };

  constructor(private personService: PersonService) {
  }

  ngOnInit(): void {
  }

  onChange(name) {
    if (name && name.length >= 3) {
      this.personService.PersonSearch(name).pipe(takeUntil(this.destroy$)).subscribe(res => {
        this.persons = res && res.data.persons;
      })
    }
  }
  selectPerson(p) {
    this.selectedPerson = p;
    this.returnListEvent.emit(JSON.stringify(this.selectedPerson));
  }
}
