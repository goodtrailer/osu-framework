// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using osuTK.Graphics.ES30;

namespace osu.Framework.Graphics.OpenGL.Textures
{
    public static class GLPixelFormatExtensions
    {
        public static PixelFormat ToIntegerFormat(this PixelFormat normalizedFormat)
        {
            return normalizedFormat switch
            {
                PixelFormat.Red => PixelFormat.RedInteger,
                PixelFormat.Rg => PixelFormat.RgInteger,
                PixelFormat.Rgb => PixelFormat.RgbInteger,
                PixelFormat.Rgba => PixelFormat.RgbaInteger,
                _ => throw new ArgumentException($"Invalid {nameof(PixelFormat)} for uploading to integer format: {normalizedFormat}.", nameof(normalizedFormat)),
            };
        }
    }
}
