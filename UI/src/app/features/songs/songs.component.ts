import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { Store } from '@ngrx/store';
import { AppState } from 'src/app/app.state';
import { getSongs } from 'src/app/state/songs/songs.actions';
import { selectSongs } from 'src/app/state/songs/songs.selectors';

@Component({
  selector: 'bt-songs',
  templateUrl: './songs.component.html',
  styleUrls: ['./songs.component.scss']
})
export class SongsComponent implements OnInit {
  public songs$ = this.store.select(selectSongs);

  constructor(private store: Store<AppState>, private router: Router) { }

  ngOnInit(): void {
    this.store.dispatch(getSongs());
  }

}
