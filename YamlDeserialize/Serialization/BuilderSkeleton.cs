// This file is part of YamlDotNet - A .NET library for YAML.
// Copyright (c) Antoine Aubry and contributors
//
// Permission is hereby granted, free of charge, to any person obtaining a copy of
// this software and associated documentation files (the "Software"), to deal in
// the Software without restriction, including without limitation the rights to
// use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies
// of the Software, and to permit persons to whom the Software is furnished to do
// so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.

using System;
using YamlDeserializer.Serialization.NamingConventions;

namespace YamlDeserializer.Serialization
{
    /// <summary>
    /// Common implementation of <see cref="SerializerBuilder" /> and <see cref="DeserializerBuilder" />.
    /// </summary>
    public abstract class BuilderSkeleton<TBuilder> where TBuilder : BuilderSkeleton<TBuilder>
    {
        internal INamingConvention namingConvention = NullNamingConvention.Instance;

        protected abstract TBuilder Self { get; }

        /// <summary>
        /// Sets the <see cref="INamingConvention" /> that will be used by the (de)serializer.
        /// </summary>
        public TBuilder WithNamingConvention(INamingConvention namingConvention)
        {
            this.namingConvention = namingConvention ?? throw new ArgumentNullException(nameof(namingConvention));
            return Self;
        }
    }

    /// <summary>
    /// A factory that creates instances of <typeparamref name="TComponent" /> based on an existing <typeparamref name="TComponentBase" />.
    /// </summary>
    /// <typeparam name="TComponentBase">The type of the wrapped component.</typeparam>
    /// <typeparam name="TComponent">The type of the component that this factory creates.</typeparam>
    /// <param name="wrapped">The component that is to be wrapped.</param>
    /// <returns>Returns a new instance of <typeparamref name="TComponent" /> that is based on <paramref name="wrapped" />.</returns>
    public delegate TComponent WrapperFactory<TComponentBase, TComponent>(TComponentBase wrapped) where TComponent : TComponentBase;

    /// <summary>
    /// A factory that creates instances of <typeparamref name="TComponent" /> based on an existing <typeparamref name="TComponentBase" /> and an argument.
    /// </summary>
    /// <typeparam name="TArgument">The type of the argument.</typeparam>
    /// <typeparam name="TComponentBase">The type of the wrapped component.</typeparam>
    /// <typeparam name="TComponent">The type of the component that this factory creates.</typeparam>
    /// <param name="wrapped">The component that is to be wrapped.</param>
    /// <param name="argument">The argument of the factory.</param>
    /// <returns>Returns a new instance of <typeparamref name="TComponent" /> that is based on <paramref name="wrapped" /> and <paramref name="argument" />.</returns>
    public delegate TComponent WrapperFactory<TArgument, TComponentBase, TComponent>(TComponentBase wrapped, TArgument argument) where TComponent : TComponentBase;
}
