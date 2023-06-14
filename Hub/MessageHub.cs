using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Text.Json;

namespace SignalR_Example.Hub
{
    public class MessageHub: Hub<IMessageHub>
    {
        public async Task sendToAllConnections(List<string> message)
        {
            await Clients.All.sendToAllConnections(message);
        }

        public static Dictionary<string, string> userInfoDict = new Dictionary<string, string>();
        public async Task LoadUserInfo(dynamic message)
        {
            dynamic dynParam = JsonConvert.DeserializeObject(Convert.ToString(message));
            string userID = dynParam.userId;
            var ID = Context.ConnectionId;
            userInfoDict[userID] = ID;
            await Clients.Client(ID).StringDataTransfer("Login successfully.");
        }
        public async Task SendToConnection(string userID, string message)
        {
            if (userInfoDict.ContainsKey(userID))
            {
                await Clients.Client(userInfoDict[userID]).StringDataTransfer(message);
            }
        }

        /// <summary>
        /// Automatically obtaining the connection ID
        /// </summary>
        /// <returns></returns>
        public override Task OnConnectedAsync()
        {
            //string userId = Context.User.Identity.Name;
            string connectionId = Context.ConnectionId;
            //_userConnections[userId] = connectionId;
            return base.OnConnectedAsync();
        }

        /// <summary>
        /// Disconnecting and automatically removing the connection ID
        /// </summary>
        /// <param name="exception"></param>
        /// <returns></returns>
        public override Task OnDisconnectedAsync(Exception exception)
        {
            string ID = Context.ConnectionId;
            string userID = string.Empty;
            if (userInfoDict.ContainsValue(ID))
            {
                string key = userInfoDict.FirstOrDefault(x => x.Value == ID).Key;
                userInfoDict.Remove(key);
            }
            return base.OnDisconnectedAsync(exception);
        }
    }
}
