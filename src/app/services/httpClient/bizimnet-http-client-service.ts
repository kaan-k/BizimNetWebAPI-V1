import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Inject, Injectable, PLATFORM_ID } from '@angular/core';
import { JwtHelperService } from '@auth0/angular-jwt';
import { ToastrService } from 'ngx-toastr';
import { Observable } from 'rxjs';
import { isPlatformBrowser } from '@angular/common';

@Injectable({
  providedIn: 'root'
})
export class BizimNetHttpClientService {
  apiUrl: string = "default";

  constructor(
    private httpClient: HttpClient,
    protected jwtHelperService: JwtHelperService,
    protected toastrService: ToastrService,
    @Inject(PLATFORM_ID) private platformId: Object
  ) {
    if (isPlatformBrowser(this.platformId)) {
      const customWindow = window as Window & {
        env?: {
          apiUrl?: string;
        };
      };

      this.apiUrl = customWindow.env?.apiUrl || "default";
    }
  }

  private url(requestParameter: Partial<RequestParameters>): string {
    return `${requestParameter.baseUrl ?? this.apiUrl}${requestParameter.controller}${requestParameter.action ? `/${requestParameter.action}` : ""}`;
  }

  protected get<T>(requestParameter: Partial<RequestParameters>, id?: string): Observable<T> {
    const url = requestParameter.fullEndPoint ??
      `${this.url(requestParameter)}${id ?? ""}${requestParameter.queryString ? `?${requestParameter.queryString}` : ""}`;
    return this.httpClient.get<T>(url, {
      headers: requestParameter.headers,
      responseType: requestParameter.responseType as 'json'
    });
  }

  protected post<T>(requestParameter: Partial<RequestParameters>, body: Partial<T>): Observable<T> {
    const url = requestParameter.fullEndPoint ??
      `${this.url(requestParameter)}${requestParameter.queryString ? `?${requestParameter.queryString}` : ""}`;
    return this.httpClient.post<T>(url, body, {
      headers: requestParameter.headers,
      responseType: requestParameter.responseType as 'json'
    });
  }

  protected put<T>(requestParameter: Partial<RequestParameters>, body: Partial<T>): Observable<T> {
    const url = requestParameter.fullEndPoint ??
      `${this.url(requestParameter)}${requestParameter.queryString ? `?${requestParameter.queryString}` : ""}`;
    return this.httpClient.put<T>(url, body, {
      headers: requestParameter.headers,
      responseType: requestParameter.responseType as 'json'
    });
  }

  protected delete<T>(requestParameter: Partial<RequestParameters>, id: string): Observable<T> {
    const url = requestParameter.fullEndPoint ??
      `${this.url(requestParameter)}${id}${requestParameter.queryString ? `?${requestParameter.queryString}` : ""}`;
    return this.httpClient.delete<T>(url, {
      headers: requestParameter.headers,
      responseType: requestParameter.responseType as 'json'
    });
  }

  localStorageGet(key: string): string | null {
    if (isPlatformBrowser(this.platformId)) {
      return localStorage.getItem(key);
    }
    return null;
  }

  localStorageSet(key: string, value: string): void {
    if (isPlatformBrowser(this.platformId)) {
      localStorage.setItem(key, value);
    }
  }
}

export class RequestParameters {
  controller?: string;
  action?: string;
  queryString?: string;
  headers?: HttpHeaders;
  baseUrl?: string;
  fullEndPoint?: string;
  responseType?: string = 'json';
}
