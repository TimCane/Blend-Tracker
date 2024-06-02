import { SongsState } from "./state/songs/songs.reducers";
import { UsersState } from "./state/users/users.reducers";

export interface AppState {
    songs: SongsState;
    users: UsersState;
}