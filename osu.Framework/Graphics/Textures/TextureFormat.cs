// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

namespace osu.Framework.Graphics.Textures
{
    public enum TextureFormat
    {
        // Default texture format
        SRGBA8,

        // Misc (unsigned normalized)
        L8,
        A8,

        // RGB (unsigned normalized)
        RGB8,
        SRGB8,
        RGB565,

        // RGBA (unsigned normalized)
        RGBA4,
        RGBA8,
        RGB5A1,

        // RGBA (float)
        RGBA16F,
        RGBA32F,
    }
}
