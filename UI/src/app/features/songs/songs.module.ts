import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { SongsRoutingModule } from './songs-routing.module';
import { SongsComponent } from './songs.component';
import { EffectsModule } from '@ngrx/effects';
import { StoreModule } from '@ngrx/store';
import { SongsEffects } from 'src/app/state/songs/songs.effects';
import { songsReducer } from 'src/app/state/songs/songs.reducers';
import { DurationModule } from 'src/app/pipes/duration/duration.module';


@NgModule({
  declarations: [
    SongsComponent
  ],
  imports: [
    CommonModule,
    SongsRoutingModule,
    StoreModule.forFeature('songsFeature', songsReducer),
    EffectsModule.forFeature([SongsEffects]),
    DurationModule
  ]
})
export class SongsModule { }
