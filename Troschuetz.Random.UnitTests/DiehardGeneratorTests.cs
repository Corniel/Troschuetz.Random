/*
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

using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Troschuetz.Random.Tests
{
    /// <summary>
    ///   Unit tests taken from the DIEHARD test suite.
    /// </summary>
    public abstract partial class GeneratorTests
    {
        /// <summary>
        ///   Choose random points on a large interval. The spacings between the points should be
        ///   asymptotically exponentially distributed. The name is based on the birthday paradox.
        /// </summary>
        [Test, Category(Category.Diehard)]
        public void BirthdaySpacings()
        {
            const int days = 365;
            const int sampleCount = 1000;

            var samples = _generator.Integers(days).Take(sampleCount).ToArray();
            var distances = new List<double>(sampleCount * sampleCount);

            for (var i = 0; i < samples.Length; ++i)
            {
                for (var j = 0; j < samples.Length; ++j)
                {
                    if (i == j)
                    {
                        continue;
                    }
                    distances.Add(Math.Abs(samples[i] - samples[j]));
                }
            }

            distances.Sort();

            var mean = ComputeMean(distances);
            var lambda = (distances.Count - 2) / (mean * distances.Count);
            var lambdaLow = lambda * (1 - 1.96 / Math.Sqrt(distances.Count));
            var lambdaUpp = lambda * (1 + 1.96 / Math.Sqrt(distances.Count));

            var median = ComputeMedian(distances);
            var medianLow = Math.Log(2.0) / lambdaUpp;
            var medianUpp = Math.Log(2.0) / lambdaLow;

            const double adj = 1.28; // Factor found while testing...
            Console.WriteLine("Generator {0} had median {1}", GeneratorName, median);
            Assert.True(ApproxEquals(median / adj, medianLow), $"Generator {GeneratorName} failed: {median} < {medianLow}");
            Assert.True(ApproxEquals(median / adj, medianUpp), $"Generator {GeneratorName} failed: {median} > {medianUpp}");
        }

        [Test, Category(Category.Diehard)]
        public void FlipACoin()
        {
            const int sampleCount = 20000000;

            var heads = 0;
            var last = false;
            var sequance = 1;

            var sequances = new int[100];

            for(var i = 0; i < sampleCount;i++)
            {
                var current = _generator.NextBoolean();
                if (last != current)
                {
                    sequances[sequance]++;
                    sequance = 1;
                }
                else
                {
                    sequance++;
                }
                if(current)
                {
                    heads++;
                }
                last = current;
            }

            var distributions = new double[sequances.Length];
            distributions[1] = 0.25 * sampleCount;
            for(var i = 2; i < distributions.Length;i++)
            {
                distributions[i] = distributions[i - 1] / 2;
            }

            Console.WriteLine(GeneratorName);
            Console.WriteLine("  heads: {0:0.000%}", heads / (double)sampleCount);
            Console.WriteLine("  distributions:");
            for (var i = 1; i < sequances.Length; i++)
            {
                var delta = sequances[i] - distributions[i];
                Console.WriteLine("{0,3}: {1,8}  {2:0.000%}", i, sequances[i], delta / distributions[i]);
            }

            Assert.That(heads / (double)sampleCount, Is.EqualTo(0.5).Within(0.001), $"Generator {GeneratorName} failed on heads.");
        }
    }
}
