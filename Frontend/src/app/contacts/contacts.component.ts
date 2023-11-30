import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { AuthorizedUser } from '../models/authorized-user.model';
import { CallModel } from '../models/call.model';
import { ContactModel } from '../models/contact.model';
import { RequestCallModel } from '../models/request-call.model';
import { CallService } from '../services/call.service';
import { ContactsService } from '../services/contacts.service';
import { LocalStorageService } from '../services/local-storage.service';

@Component({
  selector: 'app-contacts',
  templateUrl: './contacts.component.html',
  styleUrls: ['./contacts.component.css']
})
export class ContactsComponent implements OnInit{
  authorizedUser: AuthorizedUser;
  contacts: ContactModel[] = [];
  calls: CallModel[] = [];

  isCreationModeActive: boolean = false;

  requestCallForm!: FormGroup;
  contactForm!: FormGroup;

  constructor(private localStorage: LocalStorageService, private contactsService: ContactsService, private callService: CallService){
    this.authorizedUser = localStorage.getAuthorizedUser();
    this.setupRequestCallForm();
    this.setupContactForm();
  }
  ngOnInit(): void {
    if(this.authorizedUser.role === 'Admin')
      this.getCalls();
    this.getContacts();
  }

  createContact(){
    this.contactsService.createContact(this.contactForm.value).subscribe(result=>{
      this.contacts.push(result.data);
    });
  }

  changeCallStatusToCompleted(callId: string){
  }

  deleteContact(contactId: string){
    this.contactsService.deleteContact(contactId).subscribe(result=>{
      var index = this.contacts.findIndex(x=>x.id === result.data.id);
      this.contacts.splice(index);
    });
  }

  changeCreationModeStatus()
  {
    this.isCreationModeActive = !this.isCreationModeActive
  }

  getCalls(){
    this.callService.getCalls().subscribe(result=>{
      this.calls = result.data ? result.data : [];
    });
  }
  getContacts(){
    this.contactsService.getContacts().subscribe(result=>{
      this.contacts = result.data ? result.data : [];
    });
  }
  requestCall(){
    var requestModel: RequestCallModel = this.requestCallForm.value;
    this.callService.requestCall(requestModel).subscribe(result=>{});
  }

  setupContactForm(){
    this.contactForm = new FormGroup({
      contactOptionName: new FormControl('', Validators.required),
      contactOptionValue: new FormControl('', Validators.required),
    });
  }

  setupRequestCallForm(){
    this.requestCallForm = new FormGroup({
      firstName: new FormControl('', Validators.required),
      mobilePhone: new FormControl('', Validators.required),
    });
  }
}
