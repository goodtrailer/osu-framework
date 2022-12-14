// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Runtime.InteropServices;
using osu.Framework.Graphics.OpenGL.Textures;
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

        public PixelFormat Format { get; private set; } = Pixel<TPixel>.FORMAT;

        public virtual PixelType Type => Pixel<TPixel>.TYPE;

        public virtual int BytesPerPixel => Pixel<TPixel>.BYTES_PER_PIXEL;

        private bool toIntegerFormat;

        public bool ToIntegerFormat
        {
            get => toIntegerFormat;
            set
            {
                if (toIntegerFormat == value)
                    return;

                toIntegerFormat = value;
                Format = value
                    ? Pixel<TPixel>.FORMAT.ToIntegerFormat()
                    : Pixel<TPixel>.FORMAT;
            }
        }

        public abstract void Dispose();
    }
}
