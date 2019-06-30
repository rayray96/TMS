import {
  Component,
  EventEmitter,
  Input,
  OnChanges,
  OnInit,
  Output
} from '@angular/core';

@Component({
  selector: 'app-dropdownlist',
  templateUrl: './dropdownlist.component.html',
  styleUrls: ['./dropdownlist.component.scss']
})
export class DropdownlistComponent implements OnInit {

  newOption: any;
  emptyDataSelection = {
    name: 'Select'
  };

  @Input() options: any;
  @Output() currentSelectionChange = new EventEmitter<object>();
  _currentSelection: any;
  get currentSelection() {
    return this._currentSelection;
  }
  @Input()
  set currentSelection(value) {
    this._currentSelection =
      value === '' || value === null || value === undefined
        ? this.emptyDataSelection
        : value;
  }

  constructor() {
    this.newOption = '';
  }

  ngOnInit() { }

  setCurrentSelection(option) {
    this.currentSelection = option;
    this.currentSelectionChange.emit(option);
  }

  addNewData() { }
}
