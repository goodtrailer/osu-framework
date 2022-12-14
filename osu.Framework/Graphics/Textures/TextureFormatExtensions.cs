// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using osu.Framework.Extensions.ImageExtensions;
using osuTK.Graphics.ES30;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace osu.Framework.Graphics.Textures
{
    public static class TextureFormatExtensions
    {
        public static TextureComponentCount ToGLTextureComponentCount(this TextureFormat format)
        {
            return format switch
            {
                // Misc

                TextureFormat.L8 => TextureComponentCount.Luminance,
                TextureFormat.A8 => TextureComponentCount.Alpha,

                // RG

                TextureFormat.RG16UI => TextureComponentCount.Rg16ui,

                // RGB

                TextureFormat.RGB8 => TextureComponentCount.Rgb8,
                TextureFormat.SRGB8 => TextureComponentCount.Srgb8,
                TextureFormat.RGB8UI => TextureComponentCount.Rgb8ui,
                TextureFormat.RGB16UI => TextureComponentCount.Rgb16ui,

                // RGBA

                TextureFormat.RGBA8 => TextureComponentCount.Rgba8,
                TextureFormat.SRGBA8 => TextureComponentCount.Srgb8Alpha8,
                TextureFormat.RGBA16F => TextureComponentCount.Rgba16f,
                TextureFormat.RGBA32F => TextureComponentCount.Rgba32f,
                TextureFormat.RGBA8UI => TextureComponentCount.Rgba8ui,
                TextureFormat.RGBA16UI => TextureComponentCount.Rgba16ui,
                TextureFormat.RGBA32UI => TextureComponentCount.Rgba32ui,

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
                // Misc

                case TextureFormat.L8:
                case TextureFormat.A8:
                    return 1;

                // RG

                case TextureFormat.RG16UI:
                    return 4;

                // RGB

                case TextureFormat.RGB8:
                case TextureFormat.SRGB8:
                case TextureFormat.RGB8UI:
                    return 3;

                case TextureFormat.RGB16UI:
                    return 6;

                // RGBA

                case TextureFormat.RGBA8:
                case TextureFormat.SRGBA8:
                case TextureFormat.RGBA8UI:
                    return 4;

                case TextureFormat.RGBA16F:
                case TextureFormat.RGBA16UI:
                    return 8;

                case TextureFormat.RGBA32F:
                case TextureFormat.RGBA32UI:
                    return 16;

                default:
                    throw new ArgumentException($"Invalid {nameof(TextureFormat)}: {format}", nameof(format));
            }
        }

        public static ITextureUpload CreateEmptyTextureUpload(this TextureFormat format)
        {
            switch (format)
            {
                case TextureFormat.L8:
                    return new TextureUpload<L8>();

                case TextureFormat.A8:
                    return new TextureUpload<A8>();

                case TextureFormat.RG16UI:
                    return new TextureUpload<Rg32>
                    {
                        ToIntegerFormat = true
                    };

                case TextureFormat.RGB8:
                case TextureFormat.SRGB8:
                    return new TextureUpload<Rgb24>();

                case TextureFormat.RGB8UI:
                    return new TextureUpload<Rgb24>
                    {
                        ToIntegerFormat = true,
                    };

                case TextureFormat.RGB16UI:
                    return new TextureUpload<Rgb48>
                    {
                        ToIntegerFormat = true,
                    };

                case TextureFormat.RGBA8:
                case TextureFormat.SRGBA8:
                    return new TextureUpload<Rgba32>();

                case TextureFormat.RGBA16F:
                case TextureFormat.RGBA32F:
                    return new TextureUpload<RgbaVector>();

                case TextureFormat.RGBA8UI:
                    return new TextureUpload<Rgba32>
                    {
                        ToIntegerFormat = true,
                    };

                case TextureFormat.RGBA16UI:
                    return new TextureUpload<Rgba64>
                    {
                        ToIntegerFormat = true,
                    };

                case TextureFormat.RGBA32UI:
                    return new RGBA32TextureUpload
                    {
                        ToIntegerFormat = true,
                    };

                default:
                    throw new ArgumentException($"Unsupported {nameof(TextureFormat)} {format} for the creation of a {nameof(TextureUpload)}", nameof(format));
            }
        }

        public static ReadOnlyByteSpan CreateImageByteSpan(this TextureFormat format, int width, int height, Color colour)
        {
            switch (format)
            {
                case TextureFormat.L8:
                    return createBackingImage<L8>(width, height, colour).CreateReadOnlyByteSpan();

                case TextureFormat.A8:
                    return createBackingImage<A8>(width, height, colour).CreateReadOnlyByteSpan();

                case TextureFormat.RG16UI:
                    return createBackingImage<Rg32>(width, height, colour).CreateReadOnlyByteSpan();

                case TextureFormat.RGB8:
                case TextureFormat.SRGB8:
                case TextureFormat.RGB8UI:
                    return createBackingImage<Rgb24>(width, height, colour).CreateReadOnlyByteSpan();

                case TextureFormat.RGB16UI:
                    return createBackingImage<Rgb48>(width, height, colour).CreateReadOnlyByteSpan();

                case TextureFormat.RGBA8:
                case TextureFormat.SRGBA8:
                case TextureFormat.RGBA8UI:
                    return createBackingImage<Rgba32>(width, height, colour).CreateReadOnlyByteSpan();

                case TextureFormat.RGBA16F:
                case TextureFormat.RGBA32F:
                    return createBackingImage<RgbaVector>(width, height, colour).CreateReadOnlyByteSpan();

                case TextureFormat.RGBA16UI:
                    return createBackingImage<Rgba64>(width, height, colour).CreateReadOnlyByteSpan();

                case TextureFormat.RGBA32UI:
                    if (colour != default)
                        throw new ArgumentException($"Unsupported {nameof(TextureFormat)} {format} for creating coloured {nameof(ReadOnlyByteSpan)}", nameof(format));

                    return createBackingImage<Rgba64>(width * 2, height * 2, default).CreateReadOnlyByteSpan();

                default:
                    throw new ArgumentException($"Invalid {nameof(TextureFormat)} {format}", nameof(format));
            }
        }

        private static Image<TPixel> createBackingImage<TPixel>(int width, int height, Color colour)
            where TPixel : unmanaged, IPixel<TPixel>
        {
            // it is faster to initialise without a background specification if transparent black is all that's required.
            return colour == default
                ? new Image<TPixel>(width, height)
                : new Image<TPixel>(width, height, colour.ToPixel<TPixel>());
        }

        private class RGBA32TextureUpload : TextureUpload<Rgba64>
        {
            public override PixelType Type => PixelType.UnsignedInt;

            public override int BytesPerPixel => 16;
        }
    }
}
