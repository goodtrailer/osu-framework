// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;

namespace osu.Framework.Graphics.Shaders
{
    public interface IUniformWithArray<T> : IUniform
        where T : unmanaged, IEquatable<T>
    {
        /// <summary>
        /// The current array of values of this uniform.
        /// </summary>
        IReadOnlyList<T> Array { get; }

        /// <summary>
        /// The size of this uniform's array.
        /// </summary>
        int Count { get; }

        /// <summary>
        /// A reference to the beginning of the changed portion of this uniform's array.
        /// </summary>
        ref T ChangedRef { get; }

        /// <summary>
        /// The size of the changed portion of this uniform's array.
        /// </summary>
        int ChangedCount { get; }
    }
}
