import { Component, EventEmitter, Input, Output } from '@angular/core';
import { ErrorModel } from 'src/app/models/error.model';

@Component({
  selector: 'app-error-modal',
  templateUrl: './error-modal.component.html',
  styleUrls: ['./error-modal.component.css']
})
export class ErrorModalComponent {
  @Input() content!: ErrorModel;
  @Output() errorModalClosed: EventEmitter<void> = new EventEmitter<void>();

  onClose(){
    this.errorModalClosed.emit();
  }

  stopProp(event: MouseEvent){
    event.stopPropagation();
  }
}
