import { Component, OnInit } from '@angular/core';
import {HttpClient} from  '@angular/common/http'; 

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})

export class AppComponent implements OnInit { //oninit happens after constructor
title = 'The Dating App'; 
description='This will be the best dating app!';


users:any; //this variable can be any type. no type safety

constructor(private http: HttpClient){}

//method needed for interface OnInit
  ngOnInit(): void {
  //  throw new Error('Method not implemented.');
  this.getUsers();
  }


getUsers(){
   this.http.get('https://localhost:5001/api/users').subscribe(
    response => {this.users=response;},
    error => {console.log(error)} 
  )  
}



}
