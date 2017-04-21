using System;
using System.Net;
using System.Text;
using System.Threading;

namespace SimpleWebServer
{
    public class WebServer
    {
        private HttpListener Listener { get; } = new HttpListener();
        private Func<HttpListenerRequest, string> ResponderMethod { get; }

        public WebServer(string[] uriPrefixes, Func<HttpListenerRequest, string> method)
        {
            if (!HttpListener.IsSupported)
            {
                throw new NotSupportedException("Http Listener is not supported with current operating system");
            }

            if (uriPrefixes == null || uriPrefixes.Length == 0)
            {
                throw new ArgumentException("uriPrefix argument is incorrect");
            }

            if (method == null)
            {
                throw new ArgumentException("method argument is incorrect");
            }

            foreach (string uriPrefix in uriPrefixes)
            {
                Listener.Prefixes.Add(uriPrefix);
            }

            ResponderMethod = method;
            Listener.Start();
        }

        public WebServer(Func<HttpListenerRequest, string> method, params string[] prefixes)
            : this(prefixes, method) { }

        public void Run()
        {
            ThreadPool.QueueUserWorkItem(workItem => StartServer());
        }

        private void StartServer()
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("\nStatus: Webserver running...");
            Console.ResetColor();

            try
            {
                while (Listener.IsListening)
                {
                    ThreadPool.QueueUserWorkItem(HttpCallBack, Listener.GetContext());
                }
            }
            catch
            {
            }
        }

        private void HttpCallBack(object context)
        {
            var listenerContext = context as HttpListenerContext;

            if (listenerContext == null) throw new ArgumentNullException(nameof(listenerContext));

            try
            {
                string responderMethod = ResponderMethod(listenerContext.Request);

                byte[] buffer = Encoding.UTF8.GetBytes(responderMethod);

                listenerContext.Response.ContentLength64 = buffer.Length;

                listenerContext.Response.OutputStream.Write(buffer, 0, buffer.Length);
            }
            catch
            {
            }
            finally
            {
                listenerContext.Response.OutputStream.Close();
            }
        }

        public void Stop()
        {
            Listener.Stop();
            Listener.Close();
        }
    }
}
