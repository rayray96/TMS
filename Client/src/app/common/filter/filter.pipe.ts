import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'filter'
})
export class FilterPipe implements PipeTransform {

  transform(value: any, filterText: string, arg: any): any {
    console.log(value);
    console.log(filterText);
    console.log(arg);

    if (!value) return null;
    if (!filterText || filterText === "All") return value;

    const data = value.filteredData === undefined ? value : value.filteredData;
    filterText = filterText.toLowerCase();

    return data.filter(item => JSON.stringify(item[arg]).toLowerCase().includes(filterText));
  }
}
