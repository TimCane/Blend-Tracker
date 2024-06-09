import { Injectable } from "@angular/core";
import { Actions, createEffect, ofType } from "@ngrx/effects";
import { Store } from "@ngrx/store";
import { switchMap, from, map, catchError, of } from "rxjs";
import { AppState } from "src/app/app.state";
import { getSongs, getSongsSuccess, getSongsFailure } from "./songs.actions";
import { BlendTrackerService } from "src/app/services/blend-tracker.service";

@Injectable()
export class SongsEffects {
  constructor(
    private actions$: Actions,
    private store: Store<AppState>,
    private blendTrackerService: BlendTrackerService
  ) {}

  getSongs$ = createEffect(() =>
    this.actions$.pipe(
      ofType(getSongs),
      switchMap(() =>
        from(this.blendTrackerService.getSongs()).pipe(
          map((songs) => getSongsSuccess({ songs })),
          catchError((error) => of(getSongsFailure({ error })))
        )
      )
    )
  );
}