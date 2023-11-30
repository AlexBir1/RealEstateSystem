import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from 'src/app/services/auth.service';

@Component({
  selector: 'app-navbar-popup',
  templateUrl: './navbar-popup.component.html',
  styleUrls: ['./navbar-popup.component.css'],
})

export class NavbarPopupComponent {
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

  stopProp(event: MouseEvent){
    event.stopPropagation();
  }
  hideNavbarPopup(){
    var elem = document.getElementById('navpopup');
    if(elem){
        elem.style.display = 'none';
    }
  }
}
