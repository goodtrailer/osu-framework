// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Buffers;
using System;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using System.Runtime.InteropServices;
using osuTK.Graphics.ES30;
using osu.Framework.Utils;

namespace osu.Framework.Extensions.ImageExtensions
{
    public readonly ref struct ReadOnlyByteSpan
    {
        public readonly ReadOnlySpan<byte> Span;

        public readonly PixelFormat Format;

        public readonly PixelType Type;

        private readonly IDisposable? owner;

        internal static ReadOnlyByteSpan FromImage<TPixel>(Image<TPixel> image)
            where TPixel : unmanaged, IPixel<TPixel>
        {
            IMemoryOwner<TPixel>? owner;
            ReadOnlySpan<TPixel> span;

            if (image.DangerousTryGetSinglePixelMemory(out var memory))
            {
                owner = null;
                span = memory.Span;
            }
            else
            {
                owner = image.CreateContiguousMemory();
                span = owner.Memory.Span;
            }

            return new ReadOnlyByteSpan(MemoryMarshal.AsBytes(span), Pixel<TPixel>.FORMAT, Pixel<TPixel>.TYPE, owner);
        }

        private ReadOnlyByteSpan(ReadOnlySpan<byte> span, PixelFormat format, PixelType type, IDisposable? owner)
        {
            Span = span;
            Format = format;
            Type = type;
            this.owner = owner;
        }

        public void Dispose()
        {
            owner?.Dispose();
        }
    }
}
