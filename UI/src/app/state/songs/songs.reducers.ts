import { createReducer, on } from "@ngrx/store";
import { getSongsSuccess, getSongsFailure } from "./songs.actions";
import { Song } from "src/app/models/song.interface";

export interface SongsState {
    error: string | null;
    songs: Song[];
}

export const initialState: SongsState = {
    error: null,
    songs: []
}


export const songsReducer = createReducer(
    initialState,

    on(getSongsSuccess, (state, { songs }) => ({
        ...state,
        songs: [...state.songs, ...songs]
    })),

    on(getSongsFailure, (state, { error }) => ({
        ...state,
        songs: [],
        error
    })),
);