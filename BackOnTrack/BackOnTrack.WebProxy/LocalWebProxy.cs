using System;
using System.Collections.Generic;
using System.Net;
using System.Net.NetworkInformation;
using System.Threading.Tasks;
using BackOnTrack.SharedResources.Infrastructure.Helpers;
using BackOnTrack.SharedResources.Models;
using BackOnTrack.WebProxy.Exceptions;
using Titanium.Web.Proxy;
using Titanium.Web.Proxy.EventArguments;
using Titanium.Web.Proxy.Models;


namespace BackOnTrack.WebProxy
{
    public class LocalWebProxy : IDisposable
    {
        private readonly ProxyServer _proxyServer;
        #region Configuration
        public bool ProxyIsEnabled { get; set; }
        private int _portNumber;
        private bool _isSystemProxy;
        private ProxyUserConfiguration _currentConfiguration;
        public static bool ProxyRunning { get; set; }
        private ExplicitProxyEndPoint explicitEndPoint;
        private readonly ExplicitProxyEndPoint endPoint = new ExplicitProxyEndPoint(IPAddress.Loopback, 8000, true);
        #endregion

        public LocalWebProxy(bool isSystemProxy = true)
        {
            _isSystemProxy = isSystemProxy;
            _proxyServer = new ProxyServer();
            _currentConfiguration = new ProxyUserConfiguration();
        }
        #region Set Proxy Configuration

        public void LoadProxyProfileFromFileSystem()
        {
            _currentConfiguration.LoadCurrentUserConfiguration();
        }

        public void CreateEmptyProfileConfigurationIfNotExists()
        {
            _currentConfiguration.CreateEmptyProfileConfigurationIfNotExists();
        }

        public void ApplyUserConfigurationOnProxy(CurrentUserConfiguration userConfigurations)
        {
            _currentConfiguration.ApplyUserConfiguration(userConfigurations);
        }

        public void SetPortNumber(int portNumber)
        {
            if (!ProxyRunning)
            {
                if(PortInUse(portNumber))
                {
                    throw new WebProxyPortAlreadyInUseException($"Port \"{portNumber}\" is already used.");
                }
                else
                {
                    _portNumber = portNumber;
                }
            }
        }

        public int GetPortNumber()
        {
            return _portNumber;
        }

        #endregion
        #region Proxy Start Quit and Dispose
        public void StartProxy()
        {
            //todo custom port configuration

            SetRequestConfiguration(); //set Configuration

            if (!PortInUse(endPoint.Port))
            {
                StartProxyWithEndPoint(); //set end point
            }
            else
            {
                throw new WebProxyPortAlreadyInUseException($"Cannot create a WebProxy. Port {endPoint.Port} is beeing used.");
            }

            if (_isSystemProxy)
            {
                _proxyServer.SetAsSystemHttpProxy(explicitEndPoint);
                _proxyServer.SetAsSystemHttpsProxy(explicitEndPoint);
            }

            foreach (var endPoint in _proxyServer.ProxyEndPoints)
                Console.WriteLine("Listening on '{0}' endpoint at Ip {1} and port: {2} ",
                    endPoint.GetType().Name, endPoint.IpAddress, endPoint.Port);

            ProxyRunning = _proxyServer.ProxyRunning;
        }
        public void QuitProxy()
        {
            //Unsubscribe & Quit
            _proxyServer.BeforeRequest -= OnRequest;
            _proxyServer.BeforeResponse -= OnResponse;
            _proxyServer.Stop();
            explicitEndPoint = null;
            ProxyRunning = false;
        }
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                _proxyServer?.Dispose();
                ProxyRunning = false;
            }
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion
        #region ProxyStartOperations

        private void StartProxyWithEndPoint()
        {
            SetProxyEndPoint(endPoint);
            _proxyServer.Start();
            if (!PortInUse(endPoint.Port))
            {
                QuitProxy();//Port cannot be used
                throw new WebProxyPortAlreadyInUseException($"Cannot create a WebProxy. Port {endPoint.Port} cannot be used.");
            }
        }



        public static bool PortInUse(int portNumber)
        {
            bool inUse = false;
            ExplicitProxyEndPoint endPointToCheck1 = new ExplicitProxyEndPoint(IPAddress.Loopback, portNumber, true);
            ExplicitProxyEndPoint endPointToCheck2 = new ExplicitProxyEndPoint(IPAddress.Parse("0.0.0.0"), portNumber, true);
            ExplicitProxyEndPoint endPointToCheck3 = new ExplicitProxyEndPoint(IPAddress.IPv6Loopback, portNumber, true);

            IPGlobalProperties ipProperties = IPGlobalProperties.GetIPGlobalProperties();
            IPEndPoint[] activeTcpListeners = ipProperties.GetActiveTcpListeners();

            foreach (IPEndPoint endPoint in activeTcpListeners)
            {
                if ((endPoint.Address.ToString() == endPointToCheck1.IpAddress.ToString() ||
                     endPoint.Address.ToString() == endPointToCheck2.IpAddress.ToString() ||
                     endPoint.Address.ToString() == endPointToCheck3.IpAddress.ToString()
                     ) && endPoint.Port == endPointToCheck1.Port)
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
            _proxyServer.BeforeResponse += OnResponse;
        }

        #endregion
        #region ProxyOperations

        private async Task OnRequest(object sender, SessionEventArgs e)
        {
            Console.WriteLine(e.WebSession.Request.Url);

            //foreach (string blockedSite in ListOfBlockedSites)
            //{
            //    if ((e.WebSession.Request.RequestUri.AbsoluteUri.Contains(blockedSite)))
            //    {
            //        e.Ok("<!DOCTYPE html>" +
            //             "<html><body><h1>" +
            //             "Website Blocked" +
            //             "</h1>" +
            //             "<p>Blocked by BackOnTrack.</p>" +
            //             "</body>" +
            //             "</html>", null);

            //        //e.Respond(new Response());
            //    }

            //    if (e.WebSession.Request.RequestUri.AbsoluteUri.Contains("wikipedia.org"))
            //    {
            //        e.Redirect("https://www.apple.com");
            //    }



            //}


            //if (!e.WebSession.Request.RequestUri.AbsoluteUri.Contains("manuelweb.at/test"))
            //{
            //    e.Redirect("https://manuelweb.at/test/");
            //}


        }

        //Modify response
        private async Task OnResponse(object sender, SessionEventArgs e)
        {
            // On Response
        }

        #endregion
    }
}
