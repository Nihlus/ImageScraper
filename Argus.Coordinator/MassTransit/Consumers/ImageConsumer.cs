//
//  ImageConsumer.cs
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
using System.Threading.Tasks;
using Argus.Common;
using Argus.Common.Messages.BulkData;
using Argus.Common.Services.Elasticsearch;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace Argus.Coordinator.MassTransit.Consumers
{
    /// <summary>
    /// Consumers fingerprinted images, indexing them.
    /// </summary>
    public class ImageConsumer : IConsumer<FingerprintedImage>
    {
        private readonly IBus _bus;
        private readonly NESTService _nestService;
        private readonly ILogger<ImageConsumer> _log;

        /// <summary>
        /// Initializes a new instance of the <see cref="ImageConsumer"/> class.
        /// </summary>
        /// <param name="bus">The message bus.</param>
        /// <param name="nestService">The elasticsearch service.</param>
        /// <param name="log">The logging instance.</param>
        public ImageConsumer(IBus bus, NESTService nestService, ILogger<ImageConsumer> log)
        {
            _bus = bus;
            _nestService = nestService;
            _log = log;
        }

        /// <inheritdoc />
        public async Task Consume(ConsumeContext<FingerprintedImage> context)
        {
            var fingerprintedImage = context.Message;

            // Save to database
            var signature = new ImageSignature(fingerprintedImage.Fingerprint);

            var indexedImage = new IndexedImage
            (
                fingerprintedImage.ServiceName,
                DateTimeOffset.UtcNow,
                fingerprintedImage.Image.ToString(),
                fingerprintedImage.Source.ToString(),
                signature.Signature,
                signature.Words
            );

            var indexImage = await _nestService.IndexImageAsync(indexedImage, context.CancellationToken);
            if (!indexImage.IsSuccess)
            {
                _log.LogWarning
                (
                    "Failed to index image from {Source} at {Link}: {Reason}",
                    fingerprintedImage.Source,
                    fingerprintedImage.Image,
                    indexImage.Error.Message
                );

                return;
            }

            var statusReport = new StatusReport
            (
                DateTime.UtcNow,
                fingerprintedImage.ServiceName,
                fingerprintedImage.Source,
                fingerprintedImage.Image,
                ImageStatus.Indexed,
                string.Empty
            );

            await _bus.Send(statusReport, context.CancellationToken);

            _log.LogInformation
            (
                "Indexed fingerprinted image from service \"{Service}\"",
                fingerprintedImage.ServiceName
            );
        }
    }
}