using System;
using System.Dynamic;
using System.Net;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SimpleWebServer.Tests.Unit
{
    [TestClass]
    public class WebServerTests
    {
        private string PageAddress { get; } = "http://localhost:8090/index/";

        private string SimpleHtmlCode { get; } = $"<HTML><BODY>Simple web page.<br><p>simple test</p></BODY></HTML>";

        [TestMethod]
        public void CallWebPage_ShouldReturn_ExpectedHtml()
        {
            var expectedHtml = SimpleHtmlCode;
            var webServer = InitializeWebServer();

            var actualHtml = GetActualHtml();
            webServer.Stop();

            Assert.IsTrue(expectedHtml == actualHtml);            
        }

        private string GetActualHtml()
        {
            WebClient client = new WebClient();
            string actualHtml = client.DownloadString(PageAddress);
            return actualHtml;
        }

        private WebServer InitializeWebServer()
        {
            WebServer webServer = new WebServer(SendResponse, PageAddress);
            webServer.Run();
            return webServer;
        }

        public static string SendResponse(HttpListenerRequest request)
        {
            return $"<HTML><BODY>Simple web page.<br><p>simple test</p></BODY></HTML>";
        }
    }
}
