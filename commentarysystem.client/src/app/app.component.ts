import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})
export class AppComponent implements OnInit {

  isFormVisible: boolean = false;  // Track if the form is visible or not
  constructor(private http: HttpClient) {}

  ngOnInit() {
  }

  toggleFormVisibility() {
    this.isFormVisible = !this.isFormVisible;
  }
}
