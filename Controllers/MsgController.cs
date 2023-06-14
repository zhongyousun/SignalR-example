using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json.Linq;
using SignalR_Example.Hub;
using Newtonsoft.Json;
using System.Text.Json;

namespace SignalR_Example.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MsgController : ControllerBase
    {
        private IHubContext<MessageHub, IMessageHub> messageHub;
        public MsgController(IHubContext<MessageHub, IMessageHub> _messageHub)
        {
            messageHub = _messageHub;
        }
        [HttpPost]
        [Route("toAll")]
        public string ToAll()
        {
            List<string> msgs = new List<string>();
            msgs.Add("Don't forget, the deadline for submitting your expense reports is this Friday.");
            msgs.Add("Friendly reminder, please refrain from using the conference room for personal calls or meetings without prior approval.");
            messageHub.Clients.All.sendToAllConnections(msgs);
            return "Msg sent successfully to all users!";
        }
        
        [HttpPost]
        [Route("toUser")]
        public string toUser([FromBody] JsonElement jobj)
        {
            var userID = jobj.GetProperty("userID").GetString();
            var Msg = jobj.GetProperty("msg").GetString();
            if (MessageHub.userInfoDict.ContainsKey(userID))
            {
                messageHub.Clients.Client(MessageHub.userInfoDict[userID]).StringDataTransfer(Msg);
                return "Msg sent successfully to user!";
            }
            else return "Msg sent failed to user!";

        }
    }
}
