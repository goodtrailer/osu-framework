// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

#nullable disable

using System;
using System.Buffers;
using osu.Framework.Graphics.Primitives;
using SixLabors.ImageSharp.Memory;
using SixLabors.ImageSharp.PixelFormats;

namespace osu.Framework.Graphics.Textures
{
    public class MemoryAllocatorTextureUpload<TPixel> : GenericTextureUpload<TPixel>
        where TPixel : unmanaged, IPixel<TPixel>
    {
        public override int Level { get; set; }

        public override RectangleI Bounds { get; set; }

        public override ReadOnlySpan<TPixel> Data => RawData;

        public Span<TPixel> RawData => memoryOwner.Memory.Span;

        private readonly IMemoryOwner<TPixel> memoryOwner;

        /// <summary>
        /// Create an empty raw texture with an efficient shared memory backing.
        /// </summary>
        /// <param name="width">The width of the texture.</param>
        /// <param name="height">The height of the texture.</param>
        /// <param name="memoryAllocator">The source to retrieve memory from. Shared default is used if null.</param>
        public MemoryAllocatorTextureUpload(int width, int height, MemoryAllocator memoryAllocator = null)
        {
            memoryOwner = (memoryAllocator ?? SixLabors.ImageSharp.Configuration.Default.MemoryAllocator).Allocate<TPixel>(width * height);
        }

        #region IDisposable Support

        private bool disposed;

        public override void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool isDisposing)
        {
            if (disposed)
                return;

            memoryOwner?.Dispose();

            disposed = true;
        }

        #endregion
    }

    public class MemoryAllocatorTextureUpload : MemoryAllocatorTextureUpload<Rgba32>
    {
        /// <inheritdoc/>
        public MemoryAllocatorTextureUpload(int width, int height, MemoryAllocator memoryAllocator = null)
            : base(width, height, memoryAllocator)
        {
        }
    }
}
