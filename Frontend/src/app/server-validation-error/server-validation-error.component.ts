import { Component, Input } from '@angular/core';

@Component({
  selector: 'app-server-validation-error',
  templateUrl: './server-validation-error.component.html',
  styleUrls: ['./server-validation-error.component.css']
})
export class ServerValidationErrorComponent{
  @Input() errors: string[] = [];
}
