using SignalR.Hubs;

namespace Areas.CommonMvc.TestWebUI.Hubs
{
    public class ChatHub : Hub
    {
        public void BroadcastMessage(string message)
        {
            Clients.writeMessage(message);
        }
    }
}