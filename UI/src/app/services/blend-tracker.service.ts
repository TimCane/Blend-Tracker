import { Injectable } from "@angular/core";
import { HttpClient } from '@angular/common/http';
import { environment } from "src/environments/environment";

@Injectable({ providedIn: 'root' })
export class BlendTrackerService {
  constructor(private http: HttpClient) {}


  authenticate(access_token: string){
    return this.http.get(environment.apiUrl + "/Authenticate", {
        headers: {
            "Authorization": "Bearer " + access_token
        }
    })
  }
}