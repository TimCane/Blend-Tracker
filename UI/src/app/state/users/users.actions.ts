import { createAction, props } from "@ngrx/store"
import { User } from "src/app/models/user.interface"

export const getUsers = createAction(
    "[Blend Tracker Page] Get Users"
)

export const getUsersSuccess = createAction(
    "[Blend Tracker Api] Get Users Success",
    props<{ users: User[] }>()
)

export const getUsersFailure = createAction(
    "[Blend Tracker Api] Get Users Failure",
    props<{ error: string }>()
)