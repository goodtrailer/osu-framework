// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

namespace osu.Framework.Graphics.Textures
{
    public enum TextureFormat
    {
        // Default texture format

        SRGBA8,

        // Misc

        L8,
        A8,

        // RG

        RG16UI,

        // RGB

        RGB8,
        SRGB8,
        RGB8UI,
        RGB16UI,

        // RGBA

        RGBA8,
        // SRGBA8, (moved to top as default)
        RGBA16F,
        RGBA32F,
        RGBA8UI,
        RGBA16UI,
        RGBA32UI,
    }
}
