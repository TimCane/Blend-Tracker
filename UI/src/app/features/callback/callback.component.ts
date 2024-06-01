import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { BlendTrackerService } from 'src/app/services/blend-tracker.service';
import { SpotifyAuthStateKey } from 'src/app/utils/constants';

@Component({
  selector: 'bt-callback',
  templateUrl: './callback.component.html',
  styleUrls: ['./callback.component.scss']
})
export class CallbackComponent implements OnInit {
  constructor(private router: Router, private service: BlendTrackerService) {
  }

  ngOnInit(): void {
    var params = this.getHashParams();

    var access_token = params.access_token,
      state = params.state,
      storedState = localStorage.getItem(SpotifyAuthStateKey);

    if (access_token && (state == null || state !== storedState)) {
      console.log('There was an error during the authentication');
      this.router.navigate(["/"]);
    } else {
      localStorage.removeItem(SpotifyAuthStateKey);
      if (access_token) {

        this.service.authenticate(access_token).subscribe({
          next: () => {
            this.router.navigate(["/home"]);
          }
        })
      } else {
        this.router.navigate(["/"]);
        // HELP
      }
    }
  }

  getHashParams() {
    var hashParams: any = {};
    var e, r = /([^&;=]+)=?([^&;]*)/g,
      q = window.location.hash.substring(1);
    while (e = r.exec(q)) {
      hashParams[e[1]] = decodeURIComponent(e[2]);
    }
    return hashParams;
  }
}