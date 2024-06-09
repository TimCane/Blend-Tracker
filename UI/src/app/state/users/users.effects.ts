import { Injectable } from "@angular/core";
import { Actions, createEffect, ofType } from "@ngrx/effects";
import { Store } from "@ngrx/store";
import { switchMap, from, map, catchError, of } from "rxjs";
import { AppState } from "src/app/app.state";
import { getUsers, getUsersSuccess, getUsersFailure } from "./users.actions";
import { BlendTrackerService } from "src/app/services/blend-tracker.service";

@Injectable()
export class UsersEffects {
  constructor(
    private actions$: Actions,
    private store: Store<AppState>,
    private blendTrackerService: BlendTrackerService
  ) {}

  getUsers$ = createEffect(() =>
    this.actions$.pipe(
      ofType(getUsers),
      switchMap(() =>
        from(this.blendTrackerService.getUsers()).pipe(
          map((users) => getUsersSuccess({ users })),
          catchError((error) => of(getUsersFailure({ error })))
        )
      )
    )
  );
}