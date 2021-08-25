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
using System.Collections.Generic;
using YamlDotNet.Core;
using YamlDotNet.Serialization.NodeDeserializers;
using YamlDotNet.Serialization.ObjectFactories;
using YamlDotNet.Serialization.Schemas;
using YamlDotNet.Serialization.TypeResolvers;
using YamlDotNet.Serialization.ValueDeserializers;

namespace YamlDotNet.Serialization
{
    /// <summary>
    /// Creates and configures instances of <see cref="Deserializer" />.
    /// This class is used to customize the behavior of <see cref="Deserializer" />. Use the relevant methods
    /// to apply customizations, then call <see cref="Build" /> to create an instance of the deserializer
    /// with the desired customizations.
    /// </summary>
    public sealed class DeserializerBuilder : BuilderSkeleton<DeserializerBuilder>
    {
        private Lazy<IObjectFactory> objectFactory;
        private readonly LazyComponentRegistrationList<Nothing, INodeDeserializer> nodeDeserializerFactories;
        private readonly Dictionary<Type, Type> typeMappings;

        /// <summary>
        /// Initializes a new <see cref="DeserializerBuilder" /> using the default component registrations.
        /// </summary>
        public DeserializerBuilder()
            : base(new StaticTypeResolver())
        {
            typeMappings = new Dictionary<Type, Type>();
            objectFactory = new Lazy<IObjectFactory>(() => new DefaultObjectFactory(typeMappings), true);
            nodeDeserializerFactories = new LazyComponentRegistrationList<Nothing, INodeDeserializer>
            {
                { typeof(NullNodeDeserializer), _ => new NullNodeDeserializer() },
                { typeof(ScalarNodeDeserializer), _ => new ScalarNodeDeserializer() },
                { typeof(ArrayNodeDeserializer), _ => new ArrayNodeDeserializer() },
                { typeof(DictionaryNodeDeserializer), _ => new DictionaryNodeDeserializer(objectFactory.Value) },
                { typeof(CollectionNodeDeserializer), _ => new CollectionNodeDeserializer(objectFactory.Value) },
                { typeof(EnumerableNodeDeserializer), _ => new EnumerableNodeDeserializer() },
            };
        }

        protected override DeserializerBuilder Self => this;

        /// <summary>
        /// Creates a new <see cref="Deserializer" /> according to the current configuration.
        /// </summary>
        public IDeserializer Build() => Deserializer.FromValueDeserializer(BuildValueDeserializer());

        /// <summary>
        /// Creates a new <see cref="IValueDeserializer" /> that implements the current configuration.
        /// This method is available for advanced scenarios. The preferred way to customize the behavior of the
        /// deserializer is to use the <see cref="Build" /> method.
        /// </summary>
        public IValueDeserializer BuildValueDeserializer() => new AliasValueDeserializer(new NodeValueDeserializer(nodeDeserializerFactories.BuildComponentList()));
    }
}
