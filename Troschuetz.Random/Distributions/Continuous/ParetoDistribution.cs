/*
 * Copyright � 2006 Stefan Trosch�tz (stefan@troschuetz.de)
 * Copyright � 2012-2016 Alessio Parma (alessio.parma@gmail.com)
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

namespace Troschuetz.Random.Distributions.Continuous
{
    using Core;
    using Generators;
    using PommaLabs.Thrower;
    using System;
    using System.Diagnostics;

    /// <summary>
    ///   Provides generation of pareto distributed random numbers.
    /// </summary>
    /// <remarks>
    ///   The implementation of the <see cref="ParetoDistribution"/> type bases upon information
    ///   presented on <a href="http://en.wikipedia.org/wiki/Pareto_distribution">Wikipedia - Pareto
    ///   distribution</a> and <a href="http://www.xycoon.com/par_random.htm">Xycoon - Pareto Distribution</a>.
    /// 
    ///   The thread safety of this class depends on the one of the underlying generator.
    /// </remarks>
    [Serializable]
    public sealed class ParetoDistribution : AbstractDistribution, IContinuousDistribution, IAlphaDistribution<double>, IBetaDistribution<double>
    {
        #region Constants

        /// <summary>
        ///   The default value assigned to <see cref="Alpha"/> if none is specified.
        /// </summary>
        public const double DefaultAlpha = 1;

        /// <summary>
        ///   The default value assigned to <see cref="Beta"/> if none is specified.
        /// </summary>
        public const double DefaultBeta = 1;

        #endregion Constants

        #region Fields

        /// <summary>
        ///   Stores the parameter beta which is used for generation of pareto distributed random numbers.
        /// </summary>
        double _beta;

        /// <summary>
        ///   Stores the parameter alpha which is used for generation of pareto distributed random numbers.
        /// </summary>
        double _alpha;

        /// <summary>
        ///   Gets or sets the parameter alpha which is used for generation of pareto distributed
        ///   random numbers.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException">
        ///   <paramref name="value"/> is less than or equal to zero.
        /// </exception>
        /// <remarks>
        ///   Calls <see cref="AreValidParams"/> to determine whether a value is valid and therefore assignable.
        /// </remarks>
        public double Alpha
        {
            get { return _alpha; }
            set
            {
                Raise.ArgumentOutOfRangeException.IfNot(IsValidAlpha(value), nameof(Alpha), ErrorMessages.InvalidParams);
                _alpha = value;
            }
        }

        /// <summary>
        ///   Gets or sets the parameter beta which is used for generation of pareto distributed
        ///   random numbers.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException">
        ///   <paramref name="value"/> is less than or equal to zero.
        /// </exception>
        /// <remarks>
        ///   Calls <see cref="AreValidParams"/> to determine whether a value is valid and therefore assignable.
        /// </remarks>
        public double Beta
        {
            get { return _beta; }
            set
            {
                Raise.ArgumentOutOfRangeException.IfNot(IsValidBeta(value), nameof(Beta), ErrorMessages.InvalidParams);
                _beta = value;
            }
        }

        #endregion Fields

        #region Construction

        /// <summary>
        ///   Initializes a new instance of the <see cref="ParetoDistribution"/> class, using a
        ///   <see cref="XorShift128Generator"/> as underlying random number generator.
        /// </summary>
        public ParetoDistribution() : this(new XorShift128Generator(), DefaultAlpha, DefaultBeta)
        {
            Debug.Assert(Generator is XorShift128Generator);
            Debug.Assert(Equals(Alpha, DefaultAlpha));
            Debug.Assert(Equals(Beta, DefaultBeta));
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref="ParetoDistribution"/> class, using a
        ///   <see cref="XorShift128Generator"/> with the specified seed value.
        /// </summary>
        /// <param name="seed">
        ///   An unsigned number used to calculate a starting value for the pseudo-random number sequence.
        /// </param>
        public ParetoDistribution(uint seed) : this(new XorShift128Generator(seed), DefaultAlpha, DefaultBeta)
        {
            Debug.Assert(Generator is XorShift128Generator);
            Debug.Assert(Generator.Seed == seed);
            Debug.Assert(Equals(Alpha, DefaultAlpha));
            Debug.Assert(Equals(Beta, DefaultBeta));
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref="ParetoDistribution"/> class, using the
        ///   specified <see cref="IGenerator"/> as underlying random number generator.
        /// </summary>
        /// <param name="generator">An <see cref="IGenerator"/> object.</param>
        /// <exception cref="ArgumentNullException"><paramref name="generator"/> is <see langword="null"/>.</exception>
        public ParetoDistribution(IGenerator generator) : this(generator, DefaultAlpha, DefaultBeta)
        {
            Debug.Assert(ReferenceEquals(Generator, generator));
            Debug.Assert(Equals(Alpha, DefaultAlpha));
            Debug.Assert(Equals(Beta, DefaultBeta));
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref="ParetoDistribution"/> class, using a
        ///   <see cref="XorShift128Generator"/> as underlying random number generator.
        /// </summary>
        /// <param name="alpha">
        ///   The parameter alpha which is used for generation of pareto distributed random numbers.
        /// </param>
        /// <param name="beta">
        ///   The parameter beta which is used for generation of pareto distributed random numbers.
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">
        ///   <paramref name="alpha"/> or <paramref name="beta"/> are less than or equal to zero.
        /// </exception>
        public ParetoDistribution(double alpha, double beta) : this(new XorShift128Generator(), alpha, beta)
        {
            Debug.Assert(Generator is XorShift128Generator);
            Debug.Assert(Equals(Alpha, alpha));
            Debug.Assert(Equals(Beta, beta));
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref="ParetoDistribution"/> class, using a
        ///   <see cref="XorShift128Generator"/> with the specified seed value.
        /// </summary>
        /// <param name="seed">
        ///   An unsigned number used to calculate a starting value for the pseudo-random number sequence.
        /// </param>
        /// <param name="alpha">
        ///   The parameter alpha which is used for generation of pareto distributed random numbers.
        /// </param>
        /// <param name="beta">
        ///   The parameter beta which is used for generation of pareto distributed random numbers.
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">
        ///   <paramref name="alpha"/> or <paramref name="beta"/> are less than or equal to zero.
        /// </exception>
        public ParetoDistribution(uint seed, double alpha, double beta)
            : this(new XorShift128Generator(seed), alpha, beta)
        {
            Debug.Assert(Generator is XorShift128Generator);
            Debug.Assert(Generator.Seed == seed);
            Debug.Assert(Equals(Alpha, alpha));
            Debug.Assert(Equals(Beta, beta));
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref="ParetoDistribution"/> class, using the
        ///   specified <see cref="IGenerator"/> as underlying random number generator.
        /// </summary>
        /// <param name="generator">An <see cref="IGenerator"/> object.</param>
        /// <param name="alpha">
        ///   The parameter alpha which is used for generation of pareto distributed random numbers.
        /// </param>
        /// <param name="beta">
        ///   The parameter beta which is used for generation of pareto distributed random numbers.
        /// </param>
        /// <exception cref="ArgumentNullException"><paramref name="generator"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentOutOfRangeException">
        ///   <paramref name="alpha"/> or <paramref name="beta"/> are less than or equal to zero.
        /// </exception>
        public ParetoDistribution(IGenerator generator, double alpha, double beta) : base(generator)
        {
            var vp = AreValidParams;
            Raise.ArgumentOutOfRangeException.IfNot(vp(alpha, beta), $"{nameof(alpha)}/{nameof(beta)}", ErrorMessages.InvalidParams);
            _alpha = alpha;
            _beta = beta;
        }

        #endregion Construction

        #region Instance Methods

        /// <summary>
        ///   Determines whether the specified value is valid for parameter <see cref="Alpha"/>.
        /// </summary>
        /// <param name="value">The value to check.</param>
        /// <returns><see langword="true"/> if value is greater than 0.0; otherwise, <see langword="false"/>.</returns>
        public bool IsValidAlpha(double value) => AreValidParams(value, _beta);

        /// <summary>
        ///   Determines whether the specified value is valid for parameter <see cref="Beta"/>.
        /// </summary>
        /// <param name="value">The value to check.</param>
        /// <returns><see langword="true"/> if value is greater than 0.0; otherwise, <see langword="false"/>.</returns>
        public bool IsValidBeta(double value) => AreValidParams(_alpha, value);

        #endregion Instance Methods

        #region IContinuousDistribution Members

        /// <summary>
        ///   Gets the minimum possible value of distributed random numbers.
        /// </summary>
        public double Minimum => Alpha;

        /// <summary>
        ///   Gets the maximum possible value of distributed random numbers.
        /// </summary>
        public double Maximum => double.PositiveInfinity;

        /// <summary>
        ///   Gets the mean of distributed random numbers.
        /// </summary>
        /// <exception cref="NotSupportedException">
        ///   Thrown if mean is not defined for given distribution with some parameters.
        /// </exception>
        public double Mean
        {
            get
            {
                if (_beta > 1.0)
                {
                    return Alpha * _beta / (_beta - 1.0);
                }
                throw new NotSupportedException(ErrorMessages.UndefinedMeanForParams);
            }
        }

        /// <summary>
        ///   Gets the median of distributed random numbers.
        /// </summary>
        /// <exception cref="NotSupportedException">
        ///   Thrown if median is not defined for given distribution with some parameters.
        /// </exception>
        public double Median => Alpha * Math.Pow(2.0, 1.0 / _beta);

        /// <summary>
        ///   Gets the variance of distributed random numbers.
        /// </summary>
        /// <exception cref="NotSupportedException">
        ///   Thrown if variance is not defined for given distribution with some parameters.
        /// </exception>
        public double Variance
        {
            get
            {
                if (_beta > 2.0)
                {
                    return _beta * TMath.Square(Alpha) / (TMath.Square(_beta - 1.0) * (_beta - 2.0));
                }
                throw new NotSupportedException(ErrorMessages.UndefinedVarianceForParams);
            }
        }

        /// <summary>
        ///   Gets the mode of distributed random numbers.
        /// </summary>
        /// <exception cref="NotSupportedException">
        ///   Thrown if mode is not defined for given distribution with some parameters.
        /// </exception>
        public double[] Mode => new[] { Alpha };

        /// <summary>
        ///   Returns a distributed floating point random number.
        /// </summary>
        /// <returns>A distributed double-precision floating point number.</returns>
        public double NextDouble() => Sample(Generator, _alpha, _beta);

        #endregion IContinuousDistribution Members

        #region TRandom Helpers

        /// <summary>
        ///   Determines whether pareto distribution is defined under given parameters. The default
        ///   definition returns true if alpha and beta are greater than zero; otherwise, it returns false.
        /// </summary>
        /// <remarks>
        ///   This is an extensibility point for the <see cref="ParetoDistribution"/> class.
        /// </remarks>
        public static Func<double, double, bool> AreValidParams { get; set; } = (alpha, beta) =>
        {
            return alpha > 0.0 && beta > 0.0;
        };

        /// <summary>
        ///   Declares a function returning a pareto distributed floating point random number.
        /// </summary>
        /// <remarks>
        ///   This is an extensibility point for the <see cref="ParetoDistribution"/> class.
        /// </remarks>
        public static Func<IGenerator, double, double, double> Sample { get; set; } = (generator, alpha, beta) =>
        {
            var helper1 = 1.0 / beta;
            return alpha / Math.Pow(1.0 - generator.NextDouble(), helper1);
        };

        #endregion TRandom Helpers
    }
}
