// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using osu.Framework.Graphics.Rendering;

namespace osu.Framework.Graphics.Shaders
{
    public class UniformArray<T> : IUniformWithArray<T>
        where T : unmanaged, IEquatable<T>
    {
        public IShader Owner { get; }
        public string Name { get; }
        public int Location { get; }

        public IReadOnlyList<T> Array => arr;
        public int Count => arr.Length;

        public ref T ChangedRef
        {
            get
            {
                if (changedBegin >= arr.Length)
                    throw new InvalidOperationException("The uniform array has not been changed");

                return ref arr[changedBegin];
            }
        }

        public int ChangedCount => changedEnd - changedBegin;

        private T[] arr;
        private int changedBegin;
        private int changedEnd;
        private IRenderer renderer;

        public UniformArray(IRenderer renderer, IShader owner, string name, int uniformLocation, int count)
        {
            this.renderer = renderer;
            Owner = owner;
            Name = name;
            Location = uniformLocation;
            arr = new T[count];
            changedBegin = arr.Length;
            changedEnd = 0;
        }

        public void UpdateValue(int index, T newValue) => UpdateValue(index, ref newValue);

        public void UpdateValue(int index, ref T newValue)
        {
            if (newValue.Equals(arr[index]))
                return;

            arr[index] = newValue;

            if (index < changedBegin)
                changedBegin = index;

            if (index + 1 > changedEnd)
                changedEnd = index + 1;

            if (Owner.IsBound)
                Update();
        }

        public void UpdateArray(T[] newArray, int begin = 0)
        {
            if (newArray.Length > arr.Length)
                throw new ArgumentOutOfRangeException(nameof(newArray), $"{nameof(newArray)} is longer than ${nameof(Array)}");

            newArray.CopyTo(arr, begin);
            int arrayEnd = begin + newArray.Length;

            if (begin < changedBegin)
                changedBegin = begin;

            if (arrayEnd > changedEnd)
                changedEnd = arrayEnd;

            if (Owner.IsBound)
                Update();
        }

        public UniformSpan<T> GetSpan() => GetSpan(arr.Length);

        public UniformSpan<T> GetSpan(int count, int begin = 0) => new UniformSpan<T>(this, count, begin);

        public void Update()
        {
            if (changedBegin >= arr.Length)
                return;

            renderer.SetUniform(this);
            changedBegin = arr.Length;
            changedEnd = 0;
        }

        public ref struct UniformSpan<Type>
            where Type : unmanaged, IEquatable<Type>
        {
            public Span<Type> Span { get; }

            private UniformArray<Type> parent;
            private int begin;

            internal UniformSpan(UniformArray<Type> parent, int count, int begin)
            {
                this.parent = parent;
                this.begin = begin;
                Span = parent.arr.AsSpan(begin, count);
            }

            public void Dispose()
            {
                if (begin < parent.changedBegin)
                    parent.changedBegin = begin;

                int end = begin + Span.Length;
                if (end > parent.changedEnd)
                    parent.changedEnd = end;

                if (parent.Owner.IsBound)
                    parent.Update();
            }
        }
    }
}
