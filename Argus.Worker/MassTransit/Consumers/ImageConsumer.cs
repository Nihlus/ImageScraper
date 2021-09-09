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
using System.Security.Cryptography;
using System.Threading.Tasks;
using Argus.Common;
using Argus.Common.Messages.BulkData;
using MassTransit;
using Puzzle;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace Argus.Worker.MassTransit.Consumers
{
    /// <summary>
    /// Consumes images for fingerprinting.
    /// </summary>
    public class ImageConsumer : IConsumer<CollectedImage>
    {
        private readonly IBus _bus;
        private readonly SignatureGenerator _signatureGenerator;

        /// <summary>
        /// Initializes a new instance of the <see cref="ImageConsumer"/> class.
        /// </summary>
        /// <param name="bus">The message bus.</param>
        /// <param name="signatureGenerator">The signature generator.</param>
        public ImageConsumer(IBus bus, SignatureGenerator signatureGenerator)
        {
            _bus = bus;
            _signatureGenerator = signatureGenerator;
        }

        /// <inheritdoc />
        public async Task Consume(ConsumeContext<CollectedImage> context)
        {
            var collectedImage = context.Message;
            try
            {
                // CPU-intensive step 1
                using var image = Image.Load<L8>(collectedImage.Data);
                context.CancellationToken.ThrowIfCancellationRequested();

                // CPU-intensive step 2
                using var sha256 = new SHA256Managed();
                var hash = sha256.ComputeHash(collectedImage.Data);
                context.CancellationToken.ThrowIfCancellationRequested();

                var hashString = BitConverter.ToString(hash).ToLowerInvariant();

                // CPU-intensive step 3
                var signature = _signatureGenerator.GenerateSignature(image);
                await _bus.Send
                (
                    new FingerprintedImage
                    (
                        collectedImage.ServiceName,
                        collectedImage.Source,
                        collectedImage.Image,
                        signature,
                        hashString
                    )
                );
            }
            catch (Exception e)
            {
                var message = new StatusReport
                (
                    DateTime.UtcNow,
                    collectedImage.ServiceName,
                    collectedImage.Source,
                    collectedImage.Image,
                    ImageStatus.Faulted,
                    e.Message
                );

                await _bus.Send(message);
                throw;
            }
        }
    }
}