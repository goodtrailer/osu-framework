// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Rendering;
using osu.Framework.Graphics.Shaders;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osuTK;

namespace osu.Framework.Tests.Visual.Graphics
{
    public partial class TestSceneUniformArray : FrameworkTestScene
    {
        private readonly TestDrawable[] drawables;

        public TestSceneUniformArray()
        {
            AddRange(drawables = new TestDrawable[]
            {
                new TestDrawableSpan(),
                new TestDrawableArray(),
                new TestDrawableValue(),
            });

            for (int i = 0; i < drawables.Length; i++)
            {
                var drawable = drawables[i];

                drawable.RelativeSizeAxes = Axes.Both;
                drawable.RelativePositionAxes = Axes.Both;
                drawable.Anchor = Anchor.TopLeft;
                drawable.Origin = Anchor.TopLeft;

                drawable.Size = new Vector2(1f / drawables.Length * 0.9f, 1f);
                drawable.Position = new Vector2((float)i / drawables.Length, 0f);
            }

            AddSliderStep("red", 0f, 1f, 1f, r => updateDrawables(0, r));
            AddSliderStep("green", 0f, 1f, 1f, g => updateDrawables(1, g));
            AddSliderStep("blue", 0f, 1f, 1f, b => updateDrawables(2, b));
        }

        private void updateDrawables(int channel, float value)
        {
            foreach (var drawable in drawables)
            {
                drawable.ColourArray[channel] = value;
                drawable.Invalidate(Invalidation.DrawNode);
            }
        }

        private abstract partial class TestDrawable : Box
        {
            public readonly float[] ColourArray = new float[3];

            [BackgroundDependencyLoader]
            private void load(ShaderManager shaderManager)
            {
                TextureShader = shaderManager.Load(VertexShaderDescriptor.TEXTURE_2, "TestUniformArray");
            }

            protected abstract override DrawNode CreateDrawNode();

            protected abstract class TestDrawNode : SpriteDrawNode
            {
                protected new TestDrawable Source => (TestDrawable)base.Source;

                private readonly float[] colourArray = new float[3];

                protected TestDrawNode(TestDrawable source)
                    : base(source)
                {
                }

                public override void ApplyState()
                {
                    base.ApplyState();

                    Source.ColourArray.CopyTo(colourArray, 0);
                }

                public override void Draw(IRenderer renderer)
                {
                    UpdateUniform(colourArray);

                    base.Draw(renderer);
                }

                protected abstract void UpdateUniform(float[] colour);
            }
        }

        private partial class TestDrawableSpan : TestDrawable
        {
            protected override DrawNode CreateDrawNode() => new TestDrawableSpanDrawNode(this);

            private class TestDrawableSpanDrawNode : TestDrawNode
            {
                public TestDrawableSpanDrawNode(TestDrawableSpan source)
                    : base(source)
                {
                }

                protected override void UpdateUniform(float[] colour)
                {
                    using (var colourSpan = TextureShader.GetUniformArray<float>("g_ColourArray").GetSpan())
                    {
                        for (int i = 0; i < colour.Length; i++)
                            colourSpan.Span[i] = colour[i];
                    }
                }
            }
        }

        private partial class TestDrawableArray : TestDrawable
        {
            protected override DrawNode CreateDrawNode() => new TestDrawableArrayDrawNode(this);

            private class TestDrawableArrayDrawNode : TestDrawNode
            {
                public TestDrawableArrayDrawNode(TestDrawableArray source)
                    : base(source)
                {
                }

                protected override void UpdateUniform(float[] colour) => TextureShader.GetUniformArray<float>("g_ColourArray").UpdateArray(colour);
            }
        }

        private partial class TestDrawableValue : TestDrawable
        {
            protected override DrawNode CreateDrawNode() => new TestDrawableValueDrawNode(this);

            private class TestDrawableValueDrawNode : TestDrawNode
            {
                public TestDrawableValueDrawNode(TestDrawableValue source)
                    : base(source)
                {
                }

                protected override void UpdateUniform(float[] colour)
                {
                    for (int i = 0; i < colour.Length; i++)
                        TextureShader.GetUniformArray<float>("g_ColourArray").UpdateValue(i, colour[i]);
                }
            }
        }
    }
}
