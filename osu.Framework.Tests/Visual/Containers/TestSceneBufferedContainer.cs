// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

#nullable disable

using System.Collections.Generic;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osuTK;

namespace osu.Framework.Tests.Visual.Containers
{
    public partial class TestSceneBufferedContainer : TestSceneMasking
    {
        private List<BufferedContainer> bufferedContainers = new List<BufferedContainer>();
        private List<Container> containers = new List<Container>();

        private float blur;
        private float blurRotation;
        private Vector2 fboScale;

        public TestSceneBufferedContainer()
        {
            Remove(TestContainer, false);

            containers.Add(TestContainer);

            var buffer = createBufferedContainer(TestContainer);
            bufferedContainers.Add(buffer);
            Add(buffer);

            AddSliderStep("blur", 0f, 20f, 0f, b =>
            {
                blur = b;
                updateBlur();
            });

            AddSliderStep("blur rotation (only blurs x)", 0f, 360f, 0f, r =>
            {
                blurRotation = r;
                updateBlur();
                updateBlurAngle();
            });

            AddSliderStep("fbo scale (x)", 0.01f, 4f, 1f, s =>
            {
                fboScale.X = s;
                updateFboScale();
            });

            AddSliderStep("fbo scale (y)", 0.01f, 4f, 1f, s =>
            {
                fboScale.Y = s;
                updateFboScale();
            });

            AddSliderStep("instance count", 1, 200, 1, count =>
            {
                if (count == bufferedContainers.Count)
                    return;

                while (bufferedContainers.Count > count)
                {
                    Remove(bufferedContainers[^1], true);
                    bufferedContainers.RemoveAt(bufferedContainers.Count - 1);
                }

                while (bufferedContainers.Count < count)
                {
                    var newContainer = new Container
                    {
                        RelativeSizeAxes = Axes.Both,
                    };
                    containers.Add(newContainer);

                    var newBuffer = createBufferedContainer(newContainer);
                    bufferedContainers.Add(newBuffer);
                    Add(newBuffer);
                }

                foreach (var c in containers)
                    ApplyTest(c);

                updateBlur();
                updateBlurAngle();
                updateFboScale();
            });
        }

        protected override void LoadTest(MaskingTest test)
        {
            base.LoadTest(test);

            for (int i = 1; i < containers.Count; i++)
                ApplyTest(containers[i]);
        }

        private void updateBlur()
        {
            foreach (var bc in bufferedContainers)
                bc.BlurTo(new Vector2(blur, blurRotation == 0f ? blur : 0f));
        }

        private void updateBlurAngle()
        {
            foreach (var bc in bufferedContainers)
                bc.BlurRotation = blurRotation;
        }

        private void updateFboScale()
        {
            foreach (var bc in bufferedContainers)
                bc.FrameBufferScale = fboScale;
        }

        private BufferedContainer createBufferedContainer(Drawable child) => new BufferedContainer()
        {
            RelativeSizeAxes = Axes.Both,
            Child = child,
        };
    }
}
