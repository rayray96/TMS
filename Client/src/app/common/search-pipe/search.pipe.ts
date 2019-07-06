import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'search'
})

export class SearchPipe implements PipeTransform {

  transform(value: any, filterText: string, args: any[]): any {
    if (!value || !args) return null;
    if (!filterText || filterText === "All") return value;

    const data = value.filteredData === undefined ? value : value.filteredData;
    filterText = filterText.toLowerCase();

    return data.filter(item =>
      args.some(element => String(item[element]).toLowerCase().includes(filterText)));
  }
}