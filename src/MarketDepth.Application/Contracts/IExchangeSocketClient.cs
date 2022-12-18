using MarketDepth.Domain.Models;

namespace MarketDepth.Application.Contracts
{
    public interface IExchangeSocketClient
    {
        void CloseWebSocketInstance(Guid guid);
        Guid ConnectToDepthWebSocket(string symbol, Action<DepthData> handler);
    }
}