// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

#nullable disable

using System.Collections.Generic;
using osuTK;
using osuTK.Graphics;
using osu.Framework.Graphics.Primitives;
using osu.Framework.Graphics.Shaders;
using System;
using osu.Framework.Graphics.Colour;
using osu.Framework.Graphics.Rendering;
using osu.Framework.Utils;

namespace osu.Framework.Graphics.Containers
{
    public partial class BufferedContainer<T>
    {
        private class BufferedContainerDrawNode : BufferedDrawNode, ICompositeDrawNode
        {
            private const int max_kernel_radius = 200;

            protected new BufferedContainer<T> Source => (BufferedContainer<T>)base.Source;

            protected new CompositeDrawableDrawNode Child => (CompositeDrawableDrawNode)base.Child;

            private bool drawOriginal;
            private ColourInfo effectColour;
            private BlendingParameters effectBlending;
            private EffectPlacement effectPlacement;

            private Vector2 blurSigma;
            private Vector2I blurRadius;
            private float blurRotation;

            private long updateVersion;

            private IShader blurShader;

            public BufferedContainerDrawNode(BufferedContainer<T> source, BufferedContainerDrawNodeSharedData sharedData)
                : base(source, new CompositeDrawableDrawNode(source), sharedData)
            {
            }

            public override void ApplyState()
            {
                base.ApplyState();

                updateVersion = Source.updateVersion;

                effectColour = Source.EffectColour;
                effectBlending = Source.DrawEffectBlending;
                effectPlacement = Source.EffectPlacement;

                drawOriginal = Source.DrawOriginal;
                blurSigma = Source.BlurSigma;
                blurRadius = new Vector2I(Math.Min(max_kernel_radius, Blur.KernelSize(blurSigma.X)), Math.Min(max_kernel_radius, Blur.KernelSize(blurSigma.Y)));
                blurRotation = Source.BlurRotation;

                blurShader = Source.blurShader;
            }

            protected override long GetDrawVersion() => updateVersion;

            protected override void PopulateContents(IRenderer renderer)
            {
                base.PopulateContents(renderer);

                if (blurRadius.X > 0 || blurRadius.Y > 0)
                {
                    renderer.PushScissorState(false);
                    blurShader.Bind();

                    if (blurRadius.X > 0)
                        drawBlurredFrameBuffer(renderer, blurRadius.X, blurSigma.X, blurRotation);

                    if (blurRadius.Y > 0)
                        drawBlurredFrameBuffer(renderer, blurRadius.Y, blurSigma.Y, blurRotation + 90, blurSigma.X == blurSigma.Y);

                    blurShader.Unbind();
                    renderer.PopScissorState();
                }
            }

            protected override void DrawContents(IRenderer renderer)
            {
                if (drawOriginal && effectPlacement == EffectPlacement.InFront)
                    base.DrawContents(renderer);

                renderer.SetBlend(effectBlending);

                ColourInfo finalEffectColour = DrawColourInfo.Colour;
                finalEffectColour.ApplyChild(effectColour);

                renderer.DrawFrameBuffer(SharedData.CurrentEffectBuffer, DrawRectangle, finalEffectColour);

                if (drawOriginal && effectPlacement == EffectPlacement.Behind)
                    base.DrawContents(renderer);
            }

            /// <summary>
            /// <see cref="blurShader"/> must be bound before calling this method.
            /// </summary>
            private void drawBlurredFrameBuffer(IRenderer renderer, int kernelRadius, float sigma, float blurRotation, bool reuseGaussianRadii = false)
            {
                var currentBuffer = SharedData.CurrentEffectBuffer;
                var targetBuffer = SharedData.GetNextEffectBuffer();

                renderer.SetBlend(BlendingParameters.None);

                float radians = -MathUtils.DegreesToRadians(blurRotation);
                var blurDirection = new Vector2(MathF.Cos(radians), MathF.Sin(radians));
                var normalizedBlurDirection = Vector2.Divide(blurDirection, currentBuffer.Size);
                blurShader.GetUniform<Vector2>("g_BlurDirection").UpdateValue(ref normalizedBlurDirection);

                if (!reuseGaussianRadii)
                    updateGaussianRadii(kernelRadius, sigma);

                using (BindFrameBuffer(targetBuffer))
                    renderer.DrawFrameBuffer(currentBuffer, new RectangleF(0, 0, currentBuffer.Texture.Width, currentBuffer.Texture.Height), ColourInfo.SingleColour(Color4.White));
            }

            private void updateGaussianRadii(int kernelRadius, float sigma)
            {
                int gaussianRadiiCount = Math.Clamp(kernelRadius / 2, 1, max_kernel_radius / 2);
                blurShader.GetUniform<int>("g_GaussianRadiiCount").UpdateValue(ref gaussianRadiiCount);

                float gaussianIntegral = 0f;

                using (var gaussianRadii = blurShader.GetUniformArray<float>("g_GaussianRadii").GetSpan(gaussianRadiiCount))
                using (var gaussianFactors = blurShader.GetUniformArray<float>("g_GaussianFactors").GetSpan(gaussianRadiiCount))
                {
                    // halve the weight of the center texel, since it'll be hit twice by the blur shader algorithm
                    float gaussian0 = 0.5f * MathUtils.Gaussian(0, sigma);
                    float gaussian1 = MathUtils.Gaussian(1, sigma);

                    gaussianFactors.Span[0] = gaussian0 + gaussian1;
                    gaussianRadii.Span[0] = gaussian1 / gaussianFactors.Span[0];
                    gaussianIntegral += gaussianFactors.Span[0];

                    for (int i = 1; i < gaussianRadiiCount; i++)
                    {
                        float currentRadius = 2 * i;

                        gaussian0 = MathUtils.Gaussian(currentRadius, sigma);
                        gaussian1 = MathUtils.Gaussian(currentRadius + 1, sigma);

                        gaussianFactors.Span[i] = gaussian0 + gaussian1;
                        gaussianRadii.Span[i] = currentRadius + gaussian1 / gaussianFactors.Span[i];
                        gaussianIntegral += gaussianFactors.Span[i];
                    }
                }

                float gaussianInvIntegral = 0.5f / gaussianIntegral;
                blurShader.GetUniform<float>("g_GaussianInverseIntegral").UpdateValue(ref gaussianInvIntegral);
            }

            public List<DrawNode> Children
            {
                get => Child.Children;
                set => Child.Children = value;
            }

            public bool AddChildDrawNodes => RequiresRedraw;
        }

        private class BufferedContainerDrawNodeSharedData : BufferedDrawNodeSharedData
        {
            public BufferedContainerDrawNodeSharedData(RenderBufferFormat[] formats, bool pixelSnapping, bool clipToRootNode)
                : base(2, formats, pixelSnapping, clipToRootNode)
            {
            }
        }
    }
}
