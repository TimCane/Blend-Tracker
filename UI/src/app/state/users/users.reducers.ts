import { createReducer, on } from "@ngrx/store";
import { getUsersSuccess, getUsersFailure } from "./users.actions";
import { User } from "src/app/models/user.interface";

export interface UsersState {
    error: string | null;
    users: User[];
}

export const initialState: UsersState = {
    error: null,
    users: []
}


export const usersReducer = createReducer(
    initialState,

    on(getUsersSuccess, (state, { users }) => ({
        ...state,
        users: [...state.users, ...users]
    })),

    on(getUsersFailure, (state, { error }) => ({
        ...state,
        users: [],
        error
    })),
);