// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

#nullable disable

using System;
using System.Buffers;
using osu.Framework.Graphics.Primitives;
using SixLabors.ImageSharp.PixelFormats;

namespace osu.Framework.Graphics.Textures
{
    public class ArrayPoolTextureUpload<TPixel> : GenericTextureUpload<TPixel>
        where TPixel : unmanaged, IPixel<TPixel>
    {
        public override int Level { get; set; }

        public override RectangleI Bounds { get; set; }

        public override ReadOnlySpan<TPixel> Data => data;

        public Span<TPixel> RawData => data;

        private readonly ArrayPool<TPixel> arrayPool;

        private readonly TPixel[] data;

        /// <summary>
        /// Create an empty raw texture with an efficient shared memory backing.
        /// </summary>
        /// <param name="width">The width of the texture.</param>
        /// <param name="height">The height of the texture.</param>
        /// <param name="arrayPool">The source pool to retrieve memory from. Shared default is used if null.</param>
        public ArrayPoolTextureUpload(int width, int height, ArrayPool<TPixel> arrayPool = null)
        {
            this.arrayPool = arrayPool ?? ArrayPool<TPixel>.Shared;
            data = this.arrayPool.Rent(width * height);
        }

        public override void Dispose()
        {
            arrayPool.Return(data);
        }
    }

    public class ArrayPoolTextureUpload : ArrayPoolTextureUpload<Rgba32>
    {
        /// <inheritdoc/>
        public ArrayPoolTextureUpload(int width, int height, ArrayPool<Rgba32> arrayPool = null)
            : base(width, height, arrayPool)
        {
        }
    }
}
