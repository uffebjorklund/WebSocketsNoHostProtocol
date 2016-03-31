namespace NoHost.Modules.Protocols
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Threading.Tasks;
    using XSockets.Core.Common.Protocol;
    using XSockets.Core.Common.Utility.Logging;
    using XSockets.Plugin.Framework;
    using XSockets.Plugin.Framework.Attributes;
    using XSockets.Protocol.Rfc6455;

    /// <summary>
    /// A subprotocol for allowing websockets without a valid URI
    /// This means that you can connect with a browser from c:\somepath\somefile.html
    /// </summary>
    [Export(typeof(IXSocketProtocol), Rewritable = Rewritable.No)]
    public class NoHostProtocol : Rfc6455Protocol, IRfc6455Protocol
    {
        /// <summary>
        /// Check that the client is using websocket and the sub-protocol for NoHost
        /// </summary>
        /// <param name="handshake"></param>
        /// <returns></returns>
        public override async Task<bool> Match(IList<byte> handshake)
        {            
            var s = Encoding.UTF8.GetString(handshake.ToArray());
            Composable.GetExport<IXLogger>().Debug("NoHost-Handshake: {s}", s);
            return
                Regex.Match(s, @"(^Sec-WebSocket-Version:\s13)", RegexOptions.Multiline).Success
                &&
                Regex.Match(s, @"(^Sec-WebSocket-Protocol:\sNoHost)", RegexOptions.Multiline).Success;
        }

        /// <summary>
        /// We do not have a calid request uri, set it to be localhost
        /// </summary>
        public override void SetUri()
        {            
            ConnectionContext.RequestUri = new Uri("http://localhost");
        }        

        public override IXSocketProtocol NewInstance()
        {
            return new NoHostProtocol();
        }
    }
}
