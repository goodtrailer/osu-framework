// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using osuTK.Graphics.ES30;
using SixLabors.ImageSharp.PixelFormats;

namespace osu.Framework.Utils
{
    /// <summary>
    /// Utility class for getting useful layout/format info for the several <see cref="IPixel"/>
    /// implementations.
    /// </summary>
    public static class Pixel<TPixel>
        where TPixel : unmanaged, IPixel<TPixel>
    {
        public static readonly PixelFormat FORMAT = getFormat();
        public static readonly PixelType TYPE = getType();
        public static readonly int BYTES_PER_PIXEL = getBytesPerPixel();

        private static PixelFormat getFormat()
        {
            switch (typeof(TPixel))
            {
                case Type rgba32Type when rgba32Type == typeof(Rgba32):
                case Type rgbaVecType when rgbaVecType == typeof(RgbaVector):
                case Type rgba64Type when rgba64Type == typeof(Rgba64):
                    return PixelFormat.Rgba;

                case Type rgb24Type when rgb24Type == typeof(Rgb24):
                case Type rgb48Type when rgb48Type == typeof(Rgb48):
                    return PixelFormat.Rgb;

                case Type rg32Type when rg32Type == typeof(Rg32):
                    return PixelFormat.Rg;

                case Type a8Type when a8Type == typeof(A8):
                    return PixelFormat.Alpha;

                case Type l8Type when l8Type == typeof(L8):
                    return PixelFormat.Luminance;

                default:
                    throw new ArgumentException($"Unsupported IPixel type: {typeof(TPixel).Name}");
            }
        }

        private static PixelType getType()
        {
            switch (typeof(TPixel))
            {
                case Type rgbaVecType when rgbaVecType == typeof(RgbaVector):
                    return PixelType.Float;

                case Type rgba32Type when rgba32Type == typeof(Rgba32):
                case Type rgb24Type when rgb24Type == typeof(Rgb24):
                case Type a8Type when a8Type == typeof(A8):
                case Type l8Type when l8Type == typeof(L8):
                    return PixelType.UnsignedByte;

                case Type rgba64Type when rgba64Type == typeof(Rgba64):
                case Type rgb48Type when rgb48Type == typeof(Rgb48):
                case Type rg32Type when rg32Type == typeof(Rg32):
                    return PixelType.UnsignedShort;

                default:
                    throw new ArgumentException($"Unsupported IPixel type: {typeof(TPixel).Name}");
            }
        }

        private static int getBytesPerPixel()
        {
            switch (typeof(TPixel))
            {
                case Type rgbaVecType when rgbaVecType == typeof(RgbaVector):
                    return 16;

                case Type rgba64Type when rgba64Type == typeof(Rgba64):
                    return 8;

                case Type rgb48Type when rgb48Type == typeof(Rgb48):
                    return 6;

                case Type rgba32Type when rgba32Type == typeof(Rgba32):
                case Type rg32Type when rg32Type == typeof(Rg32):
                    return 4;

                case Type rgb24Type when rgb24Type == typeof(Rgb24):
                    return 3;

                case Type a8Type when a8Type == typeof(A8):
                case Type l8Type when l8Type == typeof(L8):
                    return 1;

                default:
                    throw new ArgumentException($"Unsupported IPixel type: {typeof(TPixel).Name}");
            }
        }
    }
}
