import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'reversed'
})
export class ReversedPipe implements PipeTransform {

  /**Este pipe es para ejemplo nada más.
   * Para poder saber y compreder bien el uso de este 
   */

  transform(value: string, ...args: any[]): string {
    let newStr: string = "";
    for(let i = value.length - 1; i >= 0; i--){
      newStr += value.charAt(i);
    }
    
    return newStr;
  }

}
