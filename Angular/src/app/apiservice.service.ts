import { Injectable } from '@angular/core';
import { HttpClient, HttpEvent, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class ApiserviceService<T> {
  readonly apiUrl = 'https://localhost:5003/api/v1.0/employeeapigw/Employees';

  constructor(private http: HttpClient) { }


  // Employee
  getEmployeeList(): Observable<T> 
  {
    //const httpOptions = { headers: new HttpHeaders({ 'Content-Type': 'application/json' }) };
    return this.http.get<T>(this.apiUrl + '/GetAsListAsync');
  }

  addEmployee(emp: any): Observable<T> 
  {
    //const httpOptions = { headers: new HttpHeaders({ 'Content-Type': 'application/json' }) };

    var res =  this.http.post<T>(this.apiUrl + '/PostAsync', emp);

    return res;
  }

  updateEmployee(emp: any): Observable<T> {
    //const httpOptions = { headers: new HttpHeaders({ 'Content-Type': 'application/json' }) };
    return this.http.put<T>(this.apiUrl + '/PutAsync/' + emp.id, emp);
  }

  deleteEmployee(id: number): Observable<T> {
    //const httpOptions = { headers: new HttpHeaders({ 'Content-Type': 'application/json' }) };
    

    return this.http.delete<T>(this.apiUrl + '/DeleteAsync/' + id);
  }



}
