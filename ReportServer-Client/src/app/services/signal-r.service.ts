import { Injectable } from '@angular/core';
import * as signalR from "@aspnet/signalr";

@Injectable({
  providedIn: 'root'
})
export class SignalRService {

  private hubConnection: signalR.HubConnection
  private connection: signalR.HubConnection;

  public startConnection = () => {
    this.hubConnection = new signalR.HubConnectionBuilder()
      .withUrl('https://localhost:44391/reporthub')
      .build();
    this.hubConnection
      .start()
      .catch(err => console.log('Error while starting connection: ' + err))

    this.connectIdListener();
    this.reportgiveListener();

  }

  public connectIdListener = () => {
    this.hubConnection.on('getConnectionId', (data) => {
      this.connection = data;
      console.log(data);
    });
  }

  public setReport = () => {
    this.hubConnection.invoke('setreport', 'GorevTakip', 'GorevEmri', 'pdf', 'GorevEmriId=1', this.connection)
      .catch(err => console.error(err));
  }
  public reportgiveListener = () => {
    this.hubConnection.on('reportgive', (data) => {
      console.log(data);
    });
  }
 
}
