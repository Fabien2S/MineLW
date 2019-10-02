﻿using System;
 using System.Diagnostics;
 using System.Globalization;
 using System.Net;
 using System.Threading;
 using MineLW.API;
 using MineLW.Debugging;
 using MineLW.Networking;

 namespace MineLW
{
    public class GameServer : IServer
    {
        public const string Name = "MineLW " + Version;
        private const string Version = "0.1a";
        
        private const uint UpdatePerSecond = 20;
        private const float UpsWarnPercentage = 10 / 100f;
        private const uint UpsWarn = (uint) (UpdatePerSecond - UpdatePerSecond * UpsWarnPercentage);

        private const float MsPerSecond = 1_000;
        private const float DelayBetweenUpdate = MsPerSecond / UpdatePerSecond;
        
        private static readonly Logger Logger = LogManager.GetLogger<GameServer>();
        
        private readonly Stopwatch _stopWatch;
        private readonly NetworkServer _networkServer;

        private Thread _serverThread;
        private bool _running;

        public GameServer()
        {
            _stopWatch = new Stopwatch();
            _networkServer = new NetworkServer();
        }

        public void Start()
        {
            if (_running)
                return;

            _running = true;
            _serverThread = Thread.CurrentThread;
            
            _stopWatch.Start();

            Logger.Info("Starting {0} (Press Ctrl+C to quit)", Name);

            var ipEndPoint = new IPEndPoint(IPAddress.Any, 25565);
            _networkServer.Start(ipEndPoint);

            var elapsed = _stopWatch.ElapsedMilliseconds / MsPerSecond;
            var formattedElapsed = elapsed.ToString("F", CultureInfo.InvariantCulture);
            Logger.Success("Server started in {0}s (running at {1} ups)",formattedElapsed, UpdatePerSecond);

            _stopWatch.Stop();
            
            HandleUpdate();
        }

        private void HandleUpdate()
        {
            var sinceLastUpsCheck = 0f;
            var updateCount = 1;

            try
            {
                while (_running)
                {
                    var elapsedMillis = (float) _stopWatch.ElapsedMilliseconds;
                    sinceLastUpsCheck += elapsedMillis;
                    
                    // reset the timer right after getting the elapsed ms
                    _stopWatch.Restart();
                    
                    while (elapsedMillis >= DelayBetweenUpdate)
                    {
                        // compute delta time
                        var deltaTime = elapsedMillis / MsPerSecond;
                        
                        // update elapsed ms
                        elapsedMillis -= DelayBetweenUpdate;

                        // handle update
                        Update(deltaTime);
                        updateCount++;
                    }
                        
                    // check UPS
                    if (sinceLastUpsCheck <= MsPerSecond)
                    {
                        var updatePerSecond = updateCount / (sinceLastUpsCheck / MsPerSecond);

                        // handle update per second
                        if (updatePerSecond < UpsWarn)
                        {
                            var ratio = updatePerSecond / (double) UpdatePerSecond;

                            var formattedRatio = (ratio * 100f).ToString("F", CultureInfo.InvariantCulture);
                            var formattedUps = updatePerSecond.ToString("F", CultureInfo.InvariantCulture);

                            Logger.Warn("The server is running at {0}%  ({1} / {2} ups)", formattedRatio,
                                formattedUps,
                                UpdatePerSecond);
                        }

                        sinceLastUpsCheck = 0;
                        updateCount = 1;
                    }
                }
            }
            catch (Exception e)
            {
                Logger.Error("Unable to process the update of the server.\n\n\t-> {0}", e);
            }
            finally
            {
                try
                {
                    Shutdown();
                }
                catch (Exception e)
                {
                    Logger.Error("An error occurred while stopping the server.\n\n\t-> {0}", e.Message);
                }
            }
        }

        private void Update(float deltaTime)
        {
            _networkServer.Update(deltaTime);
        }

        public void Shutdown()
        {
            if (!_running)
                return;
            
            Logger.Info("Shutting down...");
            
            _networkServer.Stop();

            _serverThread = null;
            _running = false;
        }
    }
}