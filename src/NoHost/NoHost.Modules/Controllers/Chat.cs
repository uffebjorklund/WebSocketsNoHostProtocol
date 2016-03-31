
namespace NoHost.Modules.Controllers
{
    using XSockets.Core.XSocket;
    using XSockets.Core.XSocket.Helpers;
    using System.Threading.Tasks;

    /// <summary>
    /// Simple chat...
    /// </summary>
    public class Chat : XSocketController
    {
        public async Task Say(string message)
        {
            await this.InvokeToAll(message, "say");
        }
    }
}
