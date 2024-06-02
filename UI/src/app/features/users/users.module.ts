import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { UsersRoutingModule } from './users-routing.module';
import { UsersComponent } from './users.component';
import { EffectsModule } from '@ngrx/effects';
import { StoreModule } from '@ngrx/store';
import { UsersEffects } from 'src/app/state/users/users.effects';
import { usersReducer } from 'src/app/state/users/users.reducers';
import { DurationModule } from 'src/app/pipes/duration/duration.module';


@NgModule({
  declarations: [
    UsersComponent
  ],
  imports: [
    CommonModule,
    UsersRoutingModule,
    StoreModule.forFeature('usersFeature', usersReducer),
    EffectsModule.forFeature([UsersEffects]),
    DurationModule
  ]
})
export class UsersModule { }
