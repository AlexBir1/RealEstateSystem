import { Pipe, PipeTransform } from "@angular/core";

@Pipe({
    name: 'makeDateTimeString',
    pure: false,
    })

export class FormattedDateTimePipe implements PipeTransform {
    transform(date: Date): string {
        var dateToConvert: Date = new Date(date);
        var dateStr = dateToConvert.toLocaleDateString();
        var timeStr = (dateToConvert.getHours() + ':' + dateToConvert.getMinutes()).toString();
        return dateStr + ' - ' + timeStr;
    }
}