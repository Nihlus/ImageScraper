//
//  LoadedImage.cs
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
using SixLabors.ImageSharp;

namespace ImageScraper.Pipeline.WorkUnits
{
    /// <summary>
    /// Represents an in-memory image.
    /// </summary>
    public sealed class LoadedImage : IDisposable
    {
        /// <summary>
        /// Gets the source page that the image is associated with.
        /// </summary>
        public Uri Source { get; }

        /// <summary>
        /// Gets a direct link to the image.
        /// </summary>
        public Uri Link { get; }

        /// <summary>
        /// Gets the image data.
        /// </summary>
        public Image Image { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="LoadedImage"/> class.
        /// </summary>
        /// <param name="source">The source page.</param>
        /// <param name="link">The direct link.</param>
        /// <param name="image">The image data.</param>
        public LoadedImage(Uri source, Uri link, Image image)
        {
            this.Source = source;
            this.Link = link;
            this.Image = image;
        }

        /// <inheritdoc/>
        public void Dispose()
        {
            this.Image.Dispose();
        }
    }
}