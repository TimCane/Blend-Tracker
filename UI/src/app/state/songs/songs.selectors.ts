import { createFeatureSelector, createSelector } from "@ngrx/store";
import { SongsState } from "./songs.reducers";

export const songsFeature =
  createFeatureSelector<SongsState>('songsFeature');
const selector = <T>(mapping: (state: SongsState) => T) =>
  createSelector(songsFeature, mapping);

export const selectSongs = selector((state) => state.songs);
export const selectTotalSongs = selector((state) => state.songs.length);