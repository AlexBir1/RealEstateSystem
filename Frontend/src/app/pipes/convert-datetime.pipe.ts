import { Pipe, PipeTransform } from "@angular/core";

@Pipe({
    name: 'makeDateTimeString',
    pure: false,
    })

export class FormattedDateTimePipe implements PipeTransform {
    transform(date: Date): string {
        var dateToConvert: Date = new Date(date);
        var dateStr = dateToConvert.toLocaleDateString();
        var dateTimeStr = dateToConvert.getMinutes() < 9 ? `0${dateToConvert.getMinutes()}` : dateToConvert.getMinutes();
        var timeStr = (dateToConvert.getHours() + ':' + dateTimeStr).toString();
        return dateStr + ' - ' + timeStr;
    }
}