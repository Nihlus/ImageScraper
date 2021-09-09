//
//  MinibooruServiceCollectionExtensions.cs
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
using System.Text.Json;
using Argus.Collector.Common.Polly;
using Argus.Common.Json;
using Microsoft.Extensions.DependencyInjection;
using Polly;
using Polly.Contrib.WaitAndRetry;
using Remora.Extensions.Options.Immutable;

namespace Argus.Collector.Driver.Minibooru.Extensions
{
    /// <summary>
    /// Defines extension methods to the <see cref="IServiceCollection"/> interface.
    /// </summary>
    public static class MinibooruServiceCollectionExtensions
    {
        /// <summary>
        /// Adds a Booru driver for the given URL.
        /// </summary>
        /// <param name="services">The application services.</param>
        /// <param name="baseUrl">The base URL.</param>
        /// <param name="rateLimit">The number of requests per second the driver is permitted to make.</param>
        /// <typeparam name="TBooruDriver">The driver type to add.</typeparam>
        /// <returns>The services, with the Booru driver.</returns>
        public static IServiceCollection AddBooruDriver<TBooruDriver>
        (
            this IServiceCollection services,
            string baseUrl,
            int rateLimit = 25
        )
            where TBooruDriver : class, IBooruDriver
        {
            var retryDelay = Backoff.DecorrelatedJitterBackoffV2(TimeSpan.FromSeconds(1), 5);

            services
                .AddSingleton<TBooruDriver>()
                .AddSingleton<IBooruDriver, TBooruDriver>(s => s.GetRequiredService<TBooruDriver>())
                .Configure(typeof(TBooruDriver).Name, () => new BooruDriverOptions(new Uri(baseUrl)))
                .AddHttpClient(typeof(TBooruDriver).Name)
                .AddTransientHttpErrorPolicy
                (
                    b => b
                        .WaitAndRetryAsync(retryDelay)
                        .WrapAsync(new ThrottlingPolicy(rateLimit, TimeSpan.FromSeconds(1)))
                );

            return services;
        }
    }
}