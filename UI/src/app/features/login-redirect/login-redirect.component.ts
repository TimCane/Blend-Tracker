import { Component, OnInit } from '@angular/core';
import { SpotifyAuthStateKey } from 'src/app/utils/constants';
import { generateRandomString } from 'src/app/utils/generate-random-string';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'bt-login-redirect',
  templateUrl: './login-redirect.component.html',
  styleUrls: ['./login-redirect.component.scss']
})
export class LoginRedirectComponent implements OnInit {

  ngOnInit(): void {

    var client_id = '433d8b53218643c8982a70d9eef4efaf'; // Your client id
    var redirect_uri = environment.appUrl + '/callback'; // Your redirect uri

    var state = generateRandomString(16);

    localStorage.setItem(SpotifyAuthStateKey, state);
    var scope = 'playlist-read-private';

    var url = 'https://accounts.spotify.com/authorize';
    url += '?response_type=token';
    url += '&client_id=' + encodeURIComponent(client_id);
    url += '&scope=' + encodeURIComponent(scope);
    url += '&redirect_uri=' + encodeURIComponent(redirect_uri);
    url += '&state=' + encodeURIComponent(state);

    window.location.href = url;
  }

}
