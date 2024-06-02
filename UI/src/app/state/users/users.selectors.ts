import { createFeatureSelector, createSelector } from "@ngrx/store";
import { UsersState } from "./users.reducers";

export const usersFeature =
  createFeatureSelector<UsersState>('usersFeature');
const selector = <T>(mapping: (state: UsersState) => T) =>
  createSelector(usersFeature, mapping);

export const selectUsers = selector((state) => state.users);
export const selectTotalUsers = selector((state) => state.users.length);