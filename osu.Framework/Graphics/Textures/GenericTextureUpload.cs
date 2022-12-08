// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Runtime.InteropServices;
using osu.Framework.Graphics.Primitives;
using osu.Framework.Utils;
using osuTK.Graphics.ES30;
using SixLabors.ImageSharp.PixelFormats;

namespace osu.Framework.Graphics.Textures
{
    /// <summary>
    /// Helper base class to simplify implementing <see cref="ITextureUpload.Format"/>,
    /// <see cref="ITextureUpload.Type"/>, and <see cref="ITextureUpload.BytesPerPixel"/>
    /// to support the several <see cref="IPixel"/> implementations, such as
    /// <see cref="Rgba32"/>.
    /// </summary>
    public abstract class GenericTextureUpload<TPixel> : ITextureUpload
        where TPixel : unmanaged, IPixel<TPixel>
    {
        public abstract RectangleI Bounds { get; set; }

        public abstract int Level { get; set; }

        public abstract ReadOnlySpan<TPixel> Data { get; }

        public ReadOnlySpan<byte> ByteData => MemoryMarshal.Cast<TPixel, byte>(Data);

        public PixelFormat Format => format;

        public PixelType Type => type;

        public int BytesPerPixel => bytes_per_pixel;

        private static readonly PixelFormat format = Pixel.GetFormat<TPixel>();
        private static readonly PixelType type = Pixel.GetType<TPixel>();
        private static readonly int bytes_per_pixel = Pixel.GetBytesPerPixel<TPixel>();

        public abstract void Dispose();
    }
}
