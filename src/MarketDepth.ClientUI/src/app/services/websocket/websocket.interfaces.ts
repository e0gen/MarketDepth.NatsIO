import { Observable } from "rxjs";

export interface IWebsocketService {
    on<T>(event: string): Observable<IWsMessage<T>>;
    send(event: string, data: any): void;
    status: Observable<boolean>;
}

export interface WebSocketConfig {
    url: string;
    reconnectInterval?: number;
    reconnectAttempts?: number;
}

export interface IWsMessage<T> {
    EventType: string;
    EventTime: Date;
    Data: T;
}