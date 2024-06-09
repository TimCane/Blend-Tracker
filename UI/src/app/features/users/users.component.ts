import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { Store } from '@ngrx/store';
import { AppState } from 'src/app/app.state';
import { getUsers } from 'src/app/state/users/users.actions';
import { selectUsers } from 'src/app/state/users/users.selectors';

@Component({
  selector: 'bt-users',
  templateUrl: './users.component.html',
  styleUrls: ['./users.component.scss']
})
export class UsersComponent implements OnInit {
  public users$ = this.store.select(selectUsers);

  constructor(private store: Store<AppState>, private router: Router) { }

  ngOnInit(): void {
    this.store.dispatch(getUsers());
  }

}
