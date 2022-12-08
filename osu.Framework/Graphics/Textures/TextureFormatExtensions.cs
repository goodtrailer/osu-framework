// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using osuTK.Graphics.ES30;

namespace osu.Framework.Graphics.Textures
{
    public static class TextureFormatExtensions
    {
        public static TextureComponentCount ToGLTextureComponentCount(this TextureFormat format)
        {
            return format switch
            {
                // Misc (unsigned normalized)

                TextureFormat.L8 => TextureComponentCount.Luminance,
                TextureFormat.A8 => TextureComponentCount.Alpha,

                // RGB (unsigned normalized)

                TextureFormat.RGB8 => TextureComponentCount.Rgb8,
                TextureFormat.SRGB8 => TextureComponentCount.Srgb8,

                // RGBA (unsigned normalized)

                TextureFormat.RGBA4 => TextureComponentCount.Rgba4,
                TextureFormat.RGBA8 => TextureComponentCount.Rgba8,
                TextureFormat.SRGBA8 => TextureComponentCount.Srgb8Alpha8,
                TextureFormat.RGB5A1 => TextureComponentCount.Rgb5A1,

                // RGBA (float)

                TextureFormat.RGBA16F => TextureComponentCount.Rgba16f,
                TextureFormat.RGBA32F => TextureComponentCount.Rgba32f,

                _ => throw new ArgumentException($"Invalid {nameof(TextureFormat)}: {format}", nameof(format)),
            };
        }

        /// <summary>
        /// Gets the number of bytes per pixel in internal (GPU) storage. For unsized
        /// formats, this is only a guess. Even for sized formats, there is no guarantee
        /// that the driver will use the exact given internal format; the driver will choose
        /// the closest one available. So even for sized formats, this function may be
        /// unreliable.
        /// </summary>
        public static int GetBytesPerPixel(this TextureFormat format)
        {
            switch (format)
            {
                // Misc (unsigned normalized)

                case TextureFormat.L8:
                case TextureFormat.A8:
                    return 1;

                // RGB (unsigned normalized)

                case TextureFormat.RGB8:
                case TextureFormat.SRGB8:
                    return 3;

                // RGBA (unsigned normalized)

                case TextureFormat.RGBA4:
                case TextureFormat.RGB5A1:
                    return 2;

                case TextureFormat.RGBA8:
                case TextureFormat.SRGBA8:
                    return 4;

                // RGBA (float)

                case TextureFormat.RGBA16F:
                    return 8;

                case TextureFormat.RGBA32F:
                    return 16;

                default:
                    throw new ArgumentException($"Invalid {nameof(TextureFormat)}: {format}", nameof(format));
            }
        }
    }
}
