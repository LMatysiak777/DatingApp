import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-homepage',
  templateUrl: './homepage.component.html',
  styleUrls: ['./homepage.component.css']
})
export class HomepageComponent implements OnInit {
  registerMode = false ; 

  constructor() { }

  ngOnInit(): void {
  }

  // flag for toggling DOM generation of certain sections on hompage component
  registerToggle() {
    this.registerMode =!this.registerMode;
  }

}
