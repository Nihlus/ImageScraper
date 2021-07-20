//
//  ProcessedImage.cs
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
using ImageScraper.Services.Elasticsearch;

namespace ImageScraper.Pipeline.WorkUnits
{
    /// <summary>
    /// Represents a processed image, ready for indexing.
    /// </summary>
    public sealed class ProcessedImage
    {
        /// <summary>
        /// Gets the name of the service the image is associated with.
        /// </summary>
        public string Service { get; }

        /// <summary>
        /// Gets the source page that the image is associated with.
        /// </summary>
        public Uri Source { get; }

        /// <summary>
        /// Gets a direct link to the image.
        /// </summary>
        public Uri Link { get; }

        /// <summary>
        /// Gets the computed signature of the image.
        /// </summary>
        public ImageSignature Signature { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ProcessedImage"/> class.
        /// </summary>
        /// <param name="service">The service the image is associated with.</param>
        /// <param name="source">The source page.</param>
        /// <param name="link">The direct link.</param>
        /// <param name="signature">The image signature.</param>
        public ProcessedImage(string service, Uri source, Uri link, ImageSignature signature)
        {
            this.Source = source;
            this.Link = link;

            this.Signature = signature;
            this.Service = service;
        }
    }
}
