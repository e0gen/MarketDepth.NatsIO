import { IQuote } from "./quote";

export interface IDepthUpdate {
    Symbol: string;
    UpdateId: number;
    BidDepthDeltas: IQuote[];
    AskDepthDeltas: IQuote[];
}