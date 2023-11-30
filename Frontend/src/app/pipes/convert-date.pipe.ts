import { Pipe, PipeTransform } from "@angular/core";

@Pipe({
    name: 'makeDateString',
    pure: false,
    })

export class FormattedDatePipe implements PipeTransform {
    transform(date: Date): string {
        var dateToConvert: Date = new Date(date);
        var dateStr = dateToConvert.toLocaleDateString();
        return dateStr;
    }
}