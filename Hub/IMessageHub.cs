using Newtonsoft.Json.Linq;

namespace SignalR_Example.Hub
{
    public interface IMessageHub
    {
        Task sendToAllConnections(List<string> message);
        Task JsonDataTransfer(dynamic message);
        Task StringDataTransfer(string message);
    }
}
