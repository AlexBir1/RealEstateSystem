import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from '../services/auth.service';

@Component({
  selector: 'app-navbar',
  templateUrl: './navbar.component.html',
  styleUrls: ['./navbar.component.css'],
})
export class NavbarComponent implements OnInit {
  authorizedUserId!: string | undefined;
  authorizedUserRole: string | undefined;
  constructor(private authService: AuthService, private router: Router)
  {
    
  }

  ngOnInit() {
    this.authService.authorizedUser$.subscribe(user=>
      {
        this.authorizedUserId = user.userId;
        this.authorizedUserRole = user.role;
      });
  }

  onLogOut(){
    this.authService.signOut().subscribe(result=>this.router.navigateByUrl('/'));
  }

  showNavbarPopup(){
    var elem = document.getElementById('navpopup');
    if(elem){
      if(elem?.style.display === 'flex'){
        elem.style.display = 'none';
      }
      else
        elem.style.display = 'flex';
    }
  }
  hideNavbarPopup(){
    var elem = document.getElementById('navpopup');
    if(elem){
        elem.style.display = 'none';
    }
  }
}
