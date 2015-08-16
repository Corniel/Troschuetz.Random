﻿/*
 * Copyright © 2006 Stefan Troschütz (stefan@troschuetz.de)
 * Copyright © 2012-2016 Alessio Parma (alessio.parma@gmail.com)
 *
 * This file is part of Troschuetz.Random Class Library.
 *
 * Troschuetz.Random is free software; you can redistribute it and/or
 * modify it under the terms of the GNU Lesser General Public
 * License as published by the Free Software Foundation; either
 * version 2.1 of the License, or (at your option) any later version.
 * This library is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 *
 * See the GNU Lesser General Public License for more details.
 * You should have received a copy of the GNU Lesser General Public
 * License along with this library; if not, write to the Free Software
 * Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston, MA  02110-1301  USA
 */

using PommaLabs.Thrower;
using System;
using Troschuetz.Random.Core;

namespace Troschuetz.Random.Distributions
{
    /// <summary>
    ///   Abstract class which implements some features shared across all distributions.
    /// </summary>
    /// <typeparam name="TGen">The type of the generator used by the distribution.</typeparam>
    /// <remarks>Generator type is explictly declared to allow better performances.</remarks>
    [Serializable]
    public abstract class AbstractDistribution<TGen> where TGen : IGenerator
    {
        /// <summary>
        ///   Builds a distribution using given generator.
        /// </summary>
        /// <param name="generator">The generator that will be used by the distribution.</param>
        /// <exception cref="ArgumentNullException">Given generator is null.</exception>
        protected AbstractDistribution(TGen generator)
        {
            RaiseArgumentNullException.IfIsNull(generator, nameof(generator), ErrorMessages.NullGenerator);
            TypedGenerator = generator;
        }

        /// <summary>
        ///   Stores a <typeparamref name="TGen"/> object that can be used as underlying random
        ///   number generator.
        /// </summary>
        public TGen TypedGenerator { get; }

        #region IDistribution members

        /// <summary>
        ///   Gets a value indicating whether the random number distribution can be reset, so that
        ///   it produces the same random number sequence again.
        /// </summary>
        public bool CanReset => TypedGenerator.CanReset;

        /// <summary>
        ///   Gets a <see cref="IGenerator"/> object that can be used as underlying random number generator.
        /// </summary>
        public IGenerator Generator => TypedGenerator;

        /// <summary>
        ///   Resets the random number distribution, so that it produces the same random number
        ///   sequence again.
        /// </summary>
        /// <returns>
        ///   <see langword="true"/>, if the random number distribution was reset; otherwise, <see langword="false"/>.
        /// </returns>
        public bool Reset() => TypedGenerator.Reset();

        #endregion IDistribution members
    }
}
