import { Component, OnInit } from '@angular/core';
import { SignalRService } from './services/signal-r.service';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {
  

  constructor(public signalRService: SignalRService) { }

  ngOnInit() {
    this.signalRService.startConnection();
  }
 
  
  send() {
    console.log('prearing..')
    this.signalRService.setReport();

  }
}
