import { HttpHeaders } from "@angular/common/http";

export function makeJWTHeader(jwt: string) {
    let headers = new HttpHeaders();
    headers = headers.set('Authorization', 'Bearer ' + jwt);
    return headers;
  }