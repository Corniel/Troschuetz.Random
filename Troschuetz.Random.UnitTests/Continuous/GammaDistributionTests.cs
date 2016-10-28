﻿/*
 * Copyright © 2012 Alessio Parma (alessio.parma@gmail.com)
 *
 * This file is part of Troschuetz.Random.Tests Class Library.
 *
 * Troschuetz.Random is free software; you can redistribute it and/or
 * modify it under the terms of the GNU Lesser General Public
 * License as published by the Free Software Foundation; either
 * version 2.1 of the License, or (at your option) any later version.
 * This library is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
 * Lesser General Public License for more details.
 * You should have received a copy of the GNU Lesser General Public
 * License along with this library; if not, write to the Free Software
 * Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston, MA  02110-1301  USA
 */

namespace Troschuetz.Random.Tests.Continuous
{
    using Distributions.Continuous;
    using NUnit.Framework;
    using System;

    public sealed class GammaDistributionTests : ContinuousDistributionTests<GammaDistribution>
    {
        protected override GammaDistribution GetDist(GammaDistribution other = null)
        {
            return new GammaDistribution { Alpha = GetAlpha(other), Beta = GetBeta(other) };
        }

        protected override GammaDistribution GetDist(uint seed, GammaDistribution other = null)
        {
            return new GammaDistribution(seed) { Alpha = GetAlpha(other), Beta = GetBeta(other) };
        }

        protected override GammaDistribution GetDist(IGenerator gen, GammaDistribution other = null)
        {
            return new GammaDistribution(gen) { Alpha = GetAlpha(other), Beta = GetBeta(other) };
        }

        protected override GammaDistribution GetDistWithParams(GammaDistribution other = null)
        {
            return new GammaDistribution(GetAlpha(other), GetBeta(other));
        }

        protected override GammaDistribution GetDistWithParams(uint seed, GammaDistribution other = null)
        {
            return new GammaDistribution(seed, GetAlpha(other), GetBeta(other));
        }

        protected override GammaDistribution GetDistWithParams(IGenerator gen, GammaDistribution other = null)
        {
            return new GammaDistribution(gen, GetAlpha(other), GetBeta(other));
        }

        [TestCase(double.NaN)]
        [TestCase(0)]
        [TestCase(TinyNeg)]
        [TestCase(SmallNeg)]
        [TestCase(LargeNeg)]
        public void Alpha_WrongValues(double d)
        {
            Assert.False(GammaDistribution.AreValidParams(d, 1));
            Assert.False(Dist.IsValidAlpha(d));
            Assert.Throws<ArgumentOutOfRangeException>(() => { Dist.Alpha = d; });
        }

        [TestCase(double.NaN)]
        [TestCase(0)]
        [TestCase(TinyNeg)]
        [TestCase(SmallNeg)]
        [TestCase(LargeNeg)]
        public void Beta_WrongValues(double d)
        {
            Assert.False(GammaDistribution.AreValidParams(1, d));
            Assert.False(Dist.IsValidBeta(d));
            Assert.Throws<ArgumentOutOfRangeException>(() => { Dist.Beta = d; });
        }

        // alpha > 0
        double GetAlpha(IAlphaDistribution<double> d)
        {
            return d == null ? Rand.NextDouble(0.1, 10) : d.Alpha;
        }

        // beta > 0
        double GetBeta(IBetaDistribution<double> d)
        {
            return d == null ? Rand.NextDouble(0.1, 10) : d.Beta;
        }
    }
}
