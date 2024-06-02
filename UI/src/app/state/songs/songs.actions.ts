import { createAction, props } from "@ngrx/store"
import { Song } from "src/app/models/song.interface"

export const getSongs = createAction(
    "[Blend Tracker Page] Get Songs"
)

export const getSongsSuccess = createAction(
    "[Blend Tracker Api] Get Songs Success",
    props<{ songs: Song[] }>()
)

export const getSongsFailure = createAction(
    "[Blend Tracker Api] Get Songs Failure",
    props<{ error: string }>()
)