import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { WebsocketModule } from './services/websocket/websocket.module';
import { environment } from '../environments/environment';

@NgModule({
  declarations: [
    AppComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    WebsocketModule.config({
      url: environment.wsEndpoint
    })
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
