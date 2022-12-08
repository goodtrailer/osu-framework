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
    public static class Pixel
    {
        public static PixelFormat GetFormat<TPixel>()
            where TPixel : unmanaged, IPixel<TPixel>
        {
            switch (typeof(TPixel))
            {
                case Type rgba32Type when rgba32Type == typeof(Rgba32):
                case Type rgbaVecType when rgbaVecType == typeof(RgbaVector):
                    return PixelFormat.Rgba;

                case Type rgba64Type when rgba64Type == typeof(Rgba64):
                    return PixelFormat.RgbaInteger;

                case Type rgb24Type when rgb24Type == typeof(Rgb24):
                    return PixelFormat.Rgb;

                case Type rgb48Type when rgb48Type == typeof(Rgb48):
                    return PixelFormat.RgbInteger;

                case Type rg32Type when rg32Type == typeof(Rg32):
                    return PixelFormat.RgInteger;

                case Type a8Type when a8Type == typeof(A8):
                    return PixelFormat.Alpha;

                case Type l8Type when l8Type == typeof(L8):
                    return PixelFormat.Luminance;

                default:
                    throw new ArgumentException($"Unsupported IPixel type: {typeof(TPixel).Name}");
            }
        }

        public static PixelType GetType<TPixel>()
            where TPixel : unmanaged, IPixel<TPixel>
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

        public static int GetBytesPerPixel<TPixel>()
            where TPixel : unmanaged, IPixel<TPixel>
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
