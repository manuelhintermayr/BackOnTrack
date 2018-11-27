using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using BackOnTrack.WebProxy.Exceptions;
using Titanium.Web.Proxy;
using Titanium.Web.Proxy.EventArguments;
using Titanium.Web.Proxy.Http;
using Titanium.Web.Proxy.Models;


namespace BackOnTrack.WebProxy
{
    public class LocalWebProxy : IDisposable
    {
        private readonly ProxyServer _proxyServer;
        private ExplicitProxyEndPoint explicitEndPoint;
        private List<string> ListOfBlockedSites;
        public static bool ProxyRunning { get; set; }
        private readonly ExplicitProxyEndPoint endPoint = new ExplicitProxyEndPoint(IPAddress.Loopback, 8000, true);

        public LocalWebProxy()
        {
            _proxyServer = new ProxyServer();
            ListOfBlockedSites = new List<string>();

            ListOfBlockedSites.Add("google.com");
        }

        public void StartProxy()
        {
            SetRequestConfiguration(); //set Configuration

            if (!PortInUse(endPoint))
            {
                StartProxyWithEndPoint(); //set end point
            }
            else
            {
                throw new WebProxyPortProblemException($"Cannot create a WebProxy. Port {endPoint.Port} is beeing used.");
            }

            //todo rework
            _proxyServer.SetAsSystemHttpProxy(explicitEndPoint); //set as system proxy
            _proxyServer.SetAsSystemHttpsProxy(explicitEndPoint);
            //todo end

            foreach (var endPoint in _proxyServer.ProxyEndPoints)
                Console.WriteLine("Listening on '{0}' endpoint at Ip {1} and port: {2} ",
                    endPoint.GetType().Name, endPoint.IpAddress, endPoint.Port);

            ProxyRunning = _proxyServer.ProxyRunning;
        }

        private void StartProxyWithEndPoint()
        {
            SetProxyEndPoint(endPoint);
            _proxyServer.Start();
            if (!PortInUse(endPoint))
            {
                QuitProxy();//Port cannot be used
                throw new WebProxyPortProblemException($"Cannot create a WebProxy. Port {endPoint.Port} cannot be used.");
            }
        }



        public static bool PortInUse(ExplicitProxyEndPoint endPointToCheck)
        {
            bool inUse = false;

            IPGlobalProperties ipProperties = IPGlobalProperties.GetIPGlobalProperties();
            IPEndPoint[] ipEndPoints = ipProperties.GetActiveTcpListeners();


            foreach (IPEndPoint endPoint in ipEndPoints)
            {
                if (endPoint.Address.ToString() == endPointToCheck.IpAddress.ToString() &&
                    endPoint.Port == endPointToCheck.Port)
                {
                    inUse = true;
                    break;
                }
            }


            return inUse;
        }

        private void SetProxyEndPoint(ExplicitProxyEndPoint endPoint)
        {
            if (explicitEndPoint != null)
            {
                _proxyServer.RemoveEndPoint(explicitEndPoint);
            }

            explicitEndPoint = endPoint;

            _proxyServer.AddEndPoint(explicitEndPoint);
        }

        private void SetRequestConfiguration()
        {
            _proxyServer.BeforeRequest += OnRequest;
        }

        private async Task OnRequest(object sender, SessionEventArgs e)
        {
            Console.WriteLine(e.WebSession.Request.Url);

            foreach (string blockedSite in ListOfBlockedSites)
            {
                if ((e.WebSession.Request.RequestUri.AbsoluteUri.Contains(blockedSite)))
                {
                    e.Ok("<!DOCTYPE html>" +
                               "<html><body><h1>" +
                               "Website Blocked" +
                               "</h1>" +
                               "<p>Blocked by BackOnTrack.</p>" +
                               "</body>" +
                               "</html>", null);

                    //e.Respond(new Response());
                }

                if (e.WebSession.Request.RequestUri.AbsoluteUri.Contains("wikipedia.org"))
                {
                    e.Redirect("https://www.apple.com");
                }

            }
        }

        

       

        public void QuitProxy()
        {
            //Unsubscribe & Quit
            _proxyServer.BeforeRequest -= OnRequest;
            _proxyServer.Stop();
            explicitEndPoint = null;
            ProxyRunning = false;
            ListOfBlockedSites = new List<string>();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                _proxyServer?.Dispose();
                ProxyRunning = false;
                ListOfBlockedSites = new List<string>();
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
