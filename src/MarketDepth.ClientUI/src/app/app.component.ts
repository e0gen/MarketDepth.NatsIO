import { Component, OnInit } from '@angular/core';
import { catchError, tap, map } from 'rxjs/operators';
import { Observable } from 'rxjs';
import { WebsocketService } from './services/websocket/websocket.service';
import { WS } from './services/websocket/websocket.events';
import { IDepthUpdate, IQuote } from './models';


@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {
  title = 'Market Depth Client UI';

  private depthUpdates$: Observable<IDepthUpdate>;

  connected = false;
  symbol: string;
  bids: IQuote[];
  asks: IQuote[];

  constructor(private wsService: WebsocketService) { }

  ngOnInit() {
    this.depthUpdates$ = this.wsService.on<IDepthUpdate>(WS.ON.DEPTH_UPDATE);
  }

  connect() {
    this.connected = true;
    this.depthUpdates$
      .subscribe(message => {

        console.log(message);

        this.symbol = message.Symbol;
        this.bids = message.BidDepthDeltas;
        this.asks = message.AskDepthDeltas;
    });
  }
}
