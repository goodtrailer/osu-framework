// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using osuTK.Graphics.ES30;

namespace osu.Framework.Graphics.Textures
{
    public static class TextureFilteringModeExtensions
    {
        public static All ToGLFilteringMode(this TextureFilteringMode filteringMode)
        {
            return filteringMode switch
            {
                TextureFilteringMode.Linear => All.Linear,
                TextureFilteringMode.Nearest => All.Nearest,
                _ => throw new ArgumentException($"Unsupported filtering mode: {filteringMode}", nameof(filteringMode)),
            };
        }
    }
}
