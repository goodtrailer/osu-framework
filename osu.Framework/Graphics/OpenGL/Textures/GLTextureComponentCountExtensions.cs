// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using osu.Framework.Graphics.Textures;
using osuTK.Graphics.ES30;

namespace osu.Framework.Graphics.OpenGL.Textures
{
    public static class GLTextureComponentCountExtensions
    {
        public static bool SupportsPixelFormat(this TextureComponentCount format, PixelFormat pixelFormat)
        {
            // cross-reference: https://www.khronos.org/registry/OpenGL-Refpages/es3.0/html/glTexImage2D.xhtml

            switch (pixelFormat)
            {
                case PixelFormat.Red:
                    switch (format)
                    {
                        case TextureComponentCount.R8:
                        case TextureComponentCount.R8Snorm:
                        case TextureComponentCount.R16f:
                        case TextureComponentCount.R32f:
                            return true;
                    }

                    return false;

                case PixelFormat.RedInteger:
                    switch (format)
                    {
                        case TextureComponentCount.R8ui:
                        case TextureComponentCount.R8i:
                        case TextureComponentCount.R16ui:
                        case TextureComponentCount.R16i:
                        case TextureComponentCount.R32ui:
                        case TextureComponentCount.R32i:
                            return true;
                    }

                    return false;

                case PixelFormat.Rg:
                    switch (format)
                    {
                        case TextureComponentCount.Rg8:
                        case TextureComponentCount.Rg8Snorm:
                        case TextureComponentCount.Rg16f:
                        case TextureComponentCount.Rg32f:
                            return true;
                    }

                    return false;

                case PixelFormat.RgInteger:
                    switch (format)
                    {
                        case TextureComponentCount.Rg8ui:
                        case TextureComponentCount.Rg8i:
                        case TextureComponentCount.Rg16ui:
                        case TextureComponentCount.Rg16i:
                        case TextureComponentCount.Rg32ui:
                        case TextureComponentCount.Rg32i:
                            return true;
                    }

                    return false;

                case PixelFormat.Rgb:
                    switch (format)
                    {
                        case TextureComponentCount.Rgb:
                        case TextureComponentCount.Rgb8:
                        case TextureComponentCount.Srgb8:
                        case TextureComponentCount.Rgb565:
                        case TextureComponentCount.Rgb8Snorm:
                        case TextureComponentCount.R11fG11fB10f:
                        case TextureComponentCount.Rgb9E5:
                        case TextureComponentCount.Rgb16f:
                        case TextureComponentCount.Rgb32f:
                            return true;
                    }

                    return false;

                case PixelFormat.RgbInteger:
                    switch (format)
                    {
                        case TextureComponentCount.Rgb8ui:
                        case TextureComponentCount.Rgb8i:
                        case TextureComponentCount.Rgb16ui:
                        case TextureComponentCount.Rgb16i:
                        case TextureComponentCount.Rgb32ui:
                        case TextureComponentCount.Rgb32i:
                            return true;
                    }

                    return false;

                case PixelFormat.Rgba:
                    switch (format)
                    {
                        case TextureComponentCount.Rgba:
                        case TextureComponentCount.Rgba8:
                        case TextureComponentCount.Srgb8Alpha8:
                        case TextureComponentCount.Rgb8Snorm:
                        case TextureComponentCount.Rgb5A1:
                        case TextureComponentCount.Rgba4:
                        case TextureComponentCount.Rgb10A2:
                        case TextureComponentCount.Rgba16f:
                        case TextureComponentCount.Rgba32f:
                            return true;
                    }

                    return false;

                case PixelFormat.RgbaInteger:
                    switch (format)
                    {
                        case TextureComponentCount.Rgba8ui:
                        case TextureComponentCount.Rgba8i:
                        case TextureComponentCount.Rgb10A2ui:
                        case TextureComponentCount.Rgba16ui:
                        case TextureComponentCount.Rgba16i:
                        case TextureComponentCount.Rgba32i:
                        case TextureComponentCount.Rgba32ui:
                            return true;
                    }

                    return false;

                case PixelFormat.DepthComponent:
                    switch (format)
                    {
                        case TextureComponentCount.DepthComponent16:
                        case TextureComponentCount.DepthComponent24:
                        case TextureComponentCount.DepthComponent32f:
                            return true;
                    }

                    return false;

                case PixelFormat.DepthStencil:
                    switch (format)
                    {
                        case TextureComponentCount.Depth24Stencil8:
                        case TextureComponentCount.Depth32fStencil8:
                            return true;
                    }

                    return false;

                case PixelFormat.LuminanceAlpha:
                    switch (format)
                    {
                        case TextureComponentCount.LuminanceAlpha:
                            return true;
                    }

                    return false;

                case PixelFormat.Luminance:
                    switch (format)
                    {
                        case TextureComponentCount.Luminance:
                            return true;
                    }

                    return false;

                case PixelFormat.Alpha:
                    switch (format)
                    {
                        case TextureComponentCount.Alpha:
                            return true;
                    }

                    return false;

                default:
                    throw new ArgumentException($"{pixelFormat} is not a valid {nameof(PixelFormat)}", nameof(pixelFormat));
            }
        }

        public static bool SupportsPixelType(this TextureComponentCount format, PixelType pixelType)
        {
            // cross-reference: https://www.khronos.org/registry/OpenGL-Refpages/es3.0/html/glTexImage2D.xhtml

            switch (pixelType)
            {
                case PixelType.HalfFloat:
                    switch (format)
                    {
                        case TextureComponentCount.R16f:
                        case TextureComponentCount.Rg16f:
                        case TextureComponentCount.R11fG11fB10f:
                        case TextureComponentCount.Rgb9E5:
                        case TextureComponentCount.Rgb16f:
                        case TextureComponentCount.Rgba16f:
                            return true;
                    }

                    return false;

                case PixelType.Float:
                    switch (format)
                    {
                        case TextureComponentCount.R16f:
                        case TextureComponentCount.R32f:
                        case TextureComponentCount.Rg16f:
                        case TextureComponentCount.Rg32f:
                        case TextureComponentCount.R11fG11fB10f:
                        case TextureComponentCount.Rgb9E5:
                        case TextureComponentCount.Rgb16f:
                        case TextureComponentCount.Rgb32f:
                        case TextureComponentCount.Rgba16f:
                        case TextureComponentCount.Rgba32f:
                        case TextureComponentCount.DepthComponent32f:
                            return true;
                    }

                    return false;

                case PixelType.Float32UnsignedInt248Rev:
                    switch (format)
                    {
                        case TextureComponentCount.Depth32fStencil8:
                            return true;
                    }

                    return false;

                case PixelType.UnsignedByte:
                    switch (format)
                    {
                        case TextureComponentCount.Rgb:
                        case TextureComponentCount.Rgba:
                        case TextureComponentCount.LuminanceAlpha:
                        case TextureComponentCount.Luminance:
                        case TextureComponentCount.Alpha:
                        case TextureComponentCount.R8:
                        case TextureComponentCount.R8ui:
                        case TextureComponentCount.Rg8:
                        case TextureComponentCount.Rg8ui:
                        case TextureComponentCount.Rgb8:
                        case TextureComponentCount.Srgb8:
                        case TextureComponentCount.Rgb565:
                        case TextureComponentCount.Rgb8ui:
                        case TextureComponentCount.Rgba8:
                        case TextureComponentCount.Srgb8Alpha8:
                        case TextureComponentCount.Rgb5A1:
                        case TextureComponentCount.Rgba4:
                        case TextureComponentCount.Rgba8ui:
                            return true;
                    }

                    return false;

                case PixelType.Byte:
                    switch (format)
                    {
                        case TextureComponentCount.R8Snorm:
                        case TextureComponentCount.R8i:
                        case TextureComponentCount.Rg8Snorm:
                        case TextureComponentCount.Rg8i:
                        case TextureComponentCount.Rgb8Snorm:
                        case TextureComponentCount.Rgb8i:
                        case TextureComponentCount.Rgba8Snorm:
                        case TextureComponentCount.Rgba8i:
                            return true;
                    }

                    return false;

                case PixelType.UnsignedShort:
                    switch (format)
                    {
                        case TextureComponentCount.R16ui:
                        case TextureComponentCount.Rg16ui:
                        case TextureComponentCount.Rgb16ui:
                        case TextureComponentCount.Rgba16ui:
                        case TextureComponentCount.DepthComponent16:
                            return true;
                    }

                    return false;

                case PixelType.Short:
                    switch (format)
                    {
                        case TextureComponentCount.R16i:
                        case TextureComponentCount.Rg16i:
                        case TextureComponentCount.Rgb16i:
                        case TextureComponentCount.Rgba16i:
                            return true;
                    }

                    return false;

                case PixelType.UnsignedShort4444:
                    switch (format)
                    {
                        case TextureComponentCount.Rgba:
                        case TextureComponentCount.Rgba4:
                            return true;
                    }

                    return false;

                case PixelType.UnsignedShort5551:
                    switch (format)
                    {
                        case TextureComponentCount.Rgba:
                        case TextureComponentCount.Rgb5A1:
                            return true;
                    }

                    return false;

                case PixelType.UnsignedShort565:
                    switch (format)
                    {
                        case TextureComponentCount.Rgb:
                        case TextureComponentCount.Rgb565:
                            return true;
                    }

                    return false;

                case PixelType.UnsignedInt:
                    switch (format)
                    {
                        case TextureComponentCount.R32ui:
                        case TextureComponentCount.Rg32ui:
                        case TextureComponentCount.Rgb32ui:
                        case TextureComponentCount.Rgba32ui:
                        case TextureComponentCount.DepthComponent16:
                        case TextureComponentCount.DepthComponent24:
                            return true;
                    }

                    return false;

                case PixelType.Int:
                    switch (format)
                    {
                        case TextureComponentCount.R32i:
                        case TextureComponentCount.Rg32i:
                        case TextureComponentCount.Rgb32i:
                        case TextureComponentCount.Rgba32i:
                            return true;
                    }

                    return false;

                case PixelType.UnsignedInt10F11F11FRev:
                    switch (format)
                    {
                        case TextureComponentCount.R11fG11fB10f:
                            return true;
                    }

                    return false;

                case PixelType.UnsignedInt5999Rev:
                    switch (format)
                    {
                        case TextureComponentCount.Rgb9E5:
                            return true;
                    }

                    return false;

                case PixelType.UnsignedInt2101010Rev:
                    switch (format)
                    {
                        case TextureComponentCount.Rgb5A1:
                        case TextureComponentCount.Rgb10A2:
                        case TextureComponentCount.Rgb10A2ui:
                            return true;
                    }

                    return false;

                case PixelType.UnsignedInt248:
                    switch (format)
                    {
                        case TextureComponentCount.Depth24Stencil8:
                            return true;
                    }

                    return false;

                default:
                    throw new ArgumentException($"{pixelType} is not a valid {nameof(PixelType)}", nameof(pixelType));
            }
        }

        /// <summary>
        /// As with <see cref="TextureFormatExtensions.GetBytesPerPixel"/>, this function
        /// may be unreliable.
        /// </summary>
        public static int GetBytesPerPixel(this TextureComponentCount format)
        {
            // cross-reference: https://www.khronos.org/registry/OpenGL-Refpages/es3.0/html/glTexImage2D.xhtml

            switch (format)
            {
                // unsized formats

                case TextureComponentCount.Luminance:
                case TextureComponentCount.Alpha:
                    return 1;

                case TextureComponentCount.LuminanceAlpha:
                    return 2;

                case TextureComponentCount.Rgb:
                    return 3;

                case TextureComponentCount.Rgba:
                    return 4;

                // sized formats

                case TextureComponentCount.R8:
                case TextureComponentCount.R8Snorm:
                case TextureComponentCount.R8ui:
                case TextureComponentCount.R8i:
                    return 1;

                case TextureComponentCount.R16f:
                case TextureComponentCount.R16ui:
                case TextureComponentCount.R16i:
                case TextureComponentCount.Rg8:
                case TextureComponentCount.Rg8Snorm:
                case TextureComponentCount.Rg8ui:
                case TextureComponentCount.Rg8i:
                case TextureComponentCount.Rgb565:
                case TextureComponentCount.Rgb5A1:
                case TextureComponentCount.Rgba4:
                case TextureComponentCount.DepthComponent16:
                    return 2;

                case TextureComponentCount.Rgb8:
                case TextureComponentCount.Srgb8:
                case TextureComponentCount.Rgb8Snorm:
                case TextureComponentCount.Rgb8ui:
                case TextureComponentCount.Rgb8i:
                case TextureComponentCount.DepthComponent24:
                    return 3;

                case TextureComponentCount.R32f:
                case TextureComponentCount.R32ui:
                case TextureComponentCount.R32i:
                case TextureComponentCount.Rg16f:
                case TextureComponentCount.Rg16ui:
                case TextureComponentCount.Rg16i:
                case TextureComponentCount.R11fG11fB10f:
                case TextureComponentCount.Rgb9E5:
                case TextureComponentCount.Rgba8:
                case TextureComponentCount.Srgb8Alpha8:
                case TextureComponentCount.Rgba8Snorm:
                case TextureComponentCount.Rgb10A2:
                case TextureComponentCount.Rgba8i:
                case TextureComponentCount.Rgba8ui:
                case TextureComponentCount.Rgb10A2ui:
                case TextureComponentCount.DepthComponent32f:
                case TextureComponentCount.Depth24Stencil8:
                    return 4;

                case TextureComponentCount.Depth32fStencil8:
                    return 5;

                case TextureComponentCount.Rgb16f:
                case TextureComponentCount.Rgb16ui:
                case TextureComponentCount.Rgb16i:
                    return 6;

                case TextureComponentCount.Rg32f:
                case TextureComponentCount.Rg32ui:
                case TextureComponentCount.Rg32i:
                case TextureComponentCount.Rgba16f:
                case TextureComponentCount.Rgba16i:
                case TextureComponentCount.Rgba16ui:
                    return 8;

                case TextureComponentCount.Rgb32f:
                case TextureComponentCount.Rgb32ui:
                case TextureComponentCount.Rgb32i:
                    return 12;

                case TextureComponentCount.Rgba32f:
                case TextureComponentCount.Rgba32i:
                case TextureComponentCount.Rgba32ui:
                    return 16;

                default:
                    throw new ArgumentException($"{format} is not a valid {nameof(TextureComponentCount)} type.", nameof(format));
            }
        }
    }
}
