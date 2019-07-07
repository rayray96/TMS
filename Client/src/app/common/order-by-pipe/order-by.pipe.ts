import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'orderBy'
})
export class OrderByPipe implements PipeTransform {

  transform(array: any, field: string, orderType: string): any[] {
    if (!array) return null;
    const data = array.filteredData === undefined || array.filteredData === null ? array : array.filteredData;
    if (!Array.isArray(data)) {
      return;
    }
    console.log(orderType);
    console.log(field);
    const orderBool = orderType === 'desc' ? true : false;
    const orderedData = data.sort((a: any, b: any) => {
      let ae = a[field];
      let be = b[field];
      if (ae == undefined && be == undefined) return 0;
      if (ae == undefined && be != undefined) return orderBool ? 1 : -1;
      if (ae != undefined && be == undefined) return orderBool ? -1 : 1;
      if (ae == be) return 0;
      return orderBool ? (ae.toString().toLowerCase() > be.toString().toLowerCase() ? -1 : 1) : (be.toString().toLowerCase() > ae.toString().toLowerCase() ? -1 : 1);
    });
    console.log(orderedData);

    return orderedData;
  }
}
