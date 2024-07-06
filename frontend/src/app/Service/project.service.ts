import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';


interface ProjectRequest {
  projectName: string;
  location: string;
  framework: string;
  applicationType: string;
}


@Injectable({
  providedIn: 'root'
})

export class ProjectService 
{
  private apiUrl = 'http://localhost:5128/api/Project';  

  constructor(private http: HttpClient) { }

  generateProject(request: ProjectRequest): Observable<any> {
      return this.http.post(`${this.apiUrl}/generate`, request);
  }

}
