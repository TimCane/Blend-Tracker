import { Injectable } from "@angular/core";
import { HttpClient } from '@angular/common/http';
import { environment } from "src/environments/environment";
import { Song } from "../models/song.interface";

@Injectable({ providedIn: 'root' })
export class BlendTrackerService {
  constructor(private http: HttpClient) {}


  authenticate(access_token: string){
    return this.http.get(environment.apiUrl + "/Authenticate", {
        headers: {
            "Authorization": "Bearer " + access_token
        },
        withCredentials: true
    })
  }


  getSongs(skip: number = 0, take: number = 1000){
    return this.http.get<Song[]>(environment.apiUrl + `/GetSongs?skip=${skip}&take=${take}`, {withCredentials: true})
  }
}