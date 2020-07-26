using System;
using System.Collections.Generic;

namespace Mixture
{
    public enum Conditions
    {
        Equal,
        NotEqual,
        Less,
        LessOrEqual,
        Greater,
        GreaterOrEqual,

    }

    public static class ComparisonsHelper
    {
        public static Dictionary<Conditions, Func<(float, float), bool>> Comparisons =
            new Dictionary<Conditions, Func<(float, float), bool>>
            {
                {Conditions.Equal, t => t.Item1 == t.Item2},
                {Conditions.NotEqual, t => t.Item1 != t.Item2},
                {Conditions.Less, t => t.Item1 < t.Item2},
                {Conditions.LessOrEqual, t => t.Item1 <= t.Item2},
                {Conditions.Greater, t => t.Item1 > t.Item2},
                {Conditions.GreaterOrEqual, t => t.Item1 >= t.Item2},
            };
    }

    public enum TargetTypes
    {
        Vertices,
        Faces,
        Edges,
    }

}