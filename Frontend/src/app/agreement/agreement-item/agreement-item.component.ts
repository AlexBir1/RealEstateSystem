import { Component, Input } from '@angular/core';
import { AgreementModel } from 'src/app/models/agreement-item.model';

@Component({
  selector: 'app-agreement-item',
  templateUrl: './agreement-item.component.html',
  styleUrls: ['./agreement-item.component.css']
})
export class AgreementItemComponent {
  @Input() agreement!: AgreementModel; 
  isExpanded: boolean = false;

  onExpand(){
    this.isExpanded = !this.isExpanded;
  }
}
