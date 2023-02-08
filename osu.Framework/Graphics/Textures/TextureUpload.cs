// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

#nullable disable

using System;
using System.IO;
using System.Runtime.InteropServices;
using osu.Framework.Extensions;
using osu.Framework.Extensions.ImageExtensions;
using osu.Framework.Graphics.Primitives;
using osu.Framework.Graphics.Rendering;
using osu.Framework.Logging;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using StbiSharp;

namespace osu.Framework.Graphics.Textures
{
    /// <summary>
    /// Low level class for queueing texture uploads to the GPU.
    /// Should be manually disposed if not queued for upload via <see cref="Texture.SetData(ITextureUpload)"/>.
    /// </summary>
    public class TextureUpload<TPixel> : GenericTextureUpload<TPixel>
        where TPixel : unmanaged, IPixel<TPixel>
    {
        public override int Level { get; set; }

        public override RectangleI Bounds { get; set; }

        public override ReadOnlySpan<TPixel> Data => pixelMemory.Span;

        public int Width => image?.Width ?? 0;

        public int Height => image?.Height ?? 0;

        /// <summary>
        /// The backing texture. A handle is kept to avoid early GC.
        /// </summary>
        private readonly Image<TPixel> image;

        private ReadOnlyPixelMemory<TPixel> pixelMemory;

        /// <summary>
        /// Create an upload from a <see cref="Image{TPixel}"/>. This is the preferred method.
        /// </summary>
        /// <param name="image">The texture to upload.</param>
        public TextureUpload(Image<TPixel> image)
        {
            this.image = image;
            pixelMemory = image.CreateReadOnlyPixelMemory();
        }

        /// <summary>
        /// Create an upload from an arbitrary image stream.
        /// Note that this bypasses per-platform image loading optimisations.
        /// Use <see cref="TextureLoaderStore"/> as provided from GameHost where possible.
        /// </summary>
        /// <param name="stream">The image content.</param>
        public TextureUpload(Stream stream)
            : this(LoadFromStream(stream))
        {
        }

        internal static Image<TPixel> LoadFromStream(Stream stream)
        {
            if (TextureUploadSharedStatics.StbiNotFound)
                return Image.Load<TPixel>(stream);

            long initialPos = stream.Position;

            try
            {
                using (var buffer = SixLabors.ImageSharp.Configuration.Default.MemoryAllocator.Allocate<byte>((int)stream.Length))
                {
                    stream.ReadToFill(buffer.Memory.Span);

                    using (var stbiImage = Stbi.LoadFromMemory(buffer.Memory.Span, 4))
                        return Image.LoadPixelData(MemoryMarshal.Cast<byte, TPixel>(stbiImage.Data), stbiImage.Width, stbiImage.Height);
                }
            }
            catch (Exception e)
            {
                if (e is DllNotFoundException)
                    TextureUploadSharedStatics.StbiNotFound = true;

                Logger.Log($"Texture could not be loaded via STB; falling back to ImageSharp: {e.Message}");
                stream.Position = initialPos;
                return Image.Load<TPixel>(stream);
            }
        }

        /// <summary>
        /// Create an empty upload. Used by <see cref="IFrameBuffer"/> for initialisation.
        /// </summary>
        internal TextureUpload()
        {
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

            image?.Dispose();
            pixelMemory.Dispose();

            disposed = true;
        }

        #endregion
    }

    internal static class TextureUploadSharedStatics
    {
        public static bool StbiNotFound;
    }

    public class TextureUpload : TextureUpload<Rgba32>
    {
        /// <inheritdoc/>
        public TextureUpload(Image<Rgba32> image)
            : base(image)
        {
        }

        /// <inheritdoc/>
        public TextureUpload(Stream stream)
            : base(stream)
        {
        }

        /// <inheritdoc/>
        public TextureUpload()
        {
        }
    }
}
