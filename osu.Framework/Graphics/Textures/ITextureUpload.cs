// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

#nullable disable

using System;
using osu.Framework.Graphics.Primitives;
using osuTK.Graphics.ES30;

namespace osu.Framework.Graphics.Textures
{
    public interface ITextureUpload : IDisposable
    {
        /// <summary>
        /// The raw byte data to be uploaded.
        /// </summary>
        ReadOnlySpan<byte> ByteData { get; }

        /// <summary>
        /// The target mipmap level to upload into.
        /// </summary>
        int Level { get; }

        /// <summary>
        /// The target bounds for this upload. If not specified, will assume to be (0, 0, width, height).
        /// </summary>
        RectangleI Bounds { get; set; }

        /// <summary>
        /// The texture format for this upload.
        /// </summary>
        PixelFormat Format { get; }

        /// <summary>
        /// The storage type for this upload.
        /// </summary>
        PixelType Type { get; }

        /// <summary>
        /// The bytes per pixel for this upload.
        /// </summary>
        int BytesPerPixel { get; }

        /// <summary>
        /// The number of pixels in the data to be uploaded, calculated
        /// based on <see cref="ByteData"/> and <see cref="BytesPerPixel"/>.
        /// </summary>
        sealed int PixelCount => ByteData.Length / BytesPerPixel;
    }
}
