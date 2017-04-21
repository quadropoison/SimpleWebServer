using System;
using System.Net;

namespace SimpleWebServer
{
    public static class Response
    {
        public static string SendResponse(HttpListenerRequest request)
        {
            return $"<HTML><BODY>Simple web page.<br><p>{DateTime.Now}</p></BODY></HTML>";
        }
    }
}
