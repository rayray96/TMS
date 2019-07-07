import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'enumToArray'
})
export class EnumToArrayPipe implements PipeTransform {

  transform(data: Object, keys: boolean) {
    return keys ? Object.keys(data) : Object.values(data);
  }

}
