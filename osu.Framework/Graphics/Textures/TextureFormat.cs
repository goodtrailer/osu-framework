// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

namespace osu.Framework.Graphics.Textures
{
    public enum TextureFormat
    {
        // Default texture format
        SRGBA8,

        // Misc formats
        L8,
        A8,

        // RG formats
        RG16UI,

        // RGB formats
        RGB8,
        SRGB8,
        RGB8UI,
        RGB16UI,

        // RGBA formats (SRGBA8 not below; moved to top as default)
        RGBA8,
        RGBA16F,
        RGBA32F,
        RGBA8UI,
        RGBA16UI,
        RGBA32UI,
    }
}
