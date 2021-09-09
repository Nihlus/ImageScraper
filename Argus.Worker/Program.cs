﻿//
//  Program.cs
//
//  Author:
//       Jarl Gullberg <jarl.gullberg@gmail.com>
//
//  Copyright (c) 2021 Jarl Gullberg
//
//  This program is free software: you can redistribute it and/or modify
//  it under the terms of the GNU Affero General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  (at your option) any later version.
//
//  This program is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU Affero General Public License for more details.
//
//  You should have received a copy of the GNU Affero General Public License
//  along with this program.  If not, see <http://www.gnu.org/licenses/>.
//

using System;
using System.IO;
using System.Threading.Tasks;
using Argus.Common.Configuration;
using Argus.Worker.Configuration;
using Argus.Worker.MassTransit.Consumers;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Puzzle;
using Remora.Extensions.Options.Immutable;
using Serilog;

namespace Argus.Worker
{
    /// <summary>
    /// The main class of the program.
    /// </summary>
    internal class Program
    {
        private static async Task Main(string[] args)
        {
            using var host = CreateHostBuilder(args).Build();
            var log = host.Services.GetRequiredService<ILogger<Program>>();

            await host.RunAsync();
            log.LogInformation("Shutting down...");
        }

        private static IHostBuilder CreateHostBuilder(string[] args) => Host.CreateDefaultBuilder(args)
            .UseConsoleLifetime()
            .UseSerilog((_, logging) =>
            {
                logging
                    .MinimumLevel.Information()
                    .WriteTo.Console();
            })
        #if DEBUG
            .UseEnvironment("Development")
        #else
            .UseEnvironment("Production")
        #endif
            .ConfigureAppConfiguration((_, configuration) =>
            {
                var configFolder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
                var systemConfigFile = Path.Combine(configFolder, "argus", "worker.json");
                configuration.AddJsonFile(systemConfigFile, true);
            })
            .ConfigureServices((hostContext, services) =>
            {
                var options = new WorkerOptions(0);

                hostContext.Configuration.Bind(nameof(WorkerOptions), options);
                services.Configure(() => options);

                var brokerOptions = new BrokerOptions
                (
                    new Uri("about:blank"),
                    string.Empty,
                    string.Empty
                );

                hostContext.Configuration.Bind(nameof(BrokerOptions), brokerOptions);
                services.Configure(() => brokerOptions);

                // MassTransit
                services.AddMassTransit(busConfig =>
                {
                    busConfig.SetKebabCaseEndpointNameFormatter();
                    busConfig.UsingRabbitMq((context, cfg) =>
                    {
                        cfg.Host(brokerOptions.Host, "/argus", h =>
                        {
                            h.Username(brokerOptions.Username);
                            h.Password(brokerOptions.Password);
                        });

                        cfg.ConfigureEndpoints(context);
                    });

                    busConfig.AddConsumer<CollectedImageConsumer>();
                });

                services.AddMassTransitHostedService();

                // Signature generation
                services.AddSingleton<SignatureGenerator>();
            });
    }
}
