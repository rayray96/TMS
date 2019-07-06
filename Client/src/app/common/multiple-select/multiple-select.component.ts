import { FormControl } from '@angular/forms';
import {
  Component,
  EventEmitter,
  Input,
  OnChanges,
  OnInit,
  Output
} from '@angular/core';

@Component({
  selector: 'app-multiple-select',
  templateUrl: './multiple-select.component.html',
  styleUrls: ['./multiple-select.component.css']
})
export class MultipleSelectComponent implements OnInit {

  items = new FormControl();

  ngOnInit() {
  }

  newOption: any;
  emptyDataSelection: any = ['Select'];

  @Input() options: any;
  @Input() placeholder: string;
  @Output() currentSelectionsChange = new EventEmitter<object>();
  _currentSelections: any[];

  get currentSelections() {
    return this._currentSelections;
  }

  @Input()
  set currentSelections(values) {
    this._currentSelections =
      values === null || values === undefined
        ? this.emptyDataSelection
        : values;
  }

  constructor() {
    this.newOption = '';
  }

  setCurrentSelection(options) {
    this.currentSelections = options;
    this.currentSelectionsChange.emit(options);
  }

  addNewData() { }

}
