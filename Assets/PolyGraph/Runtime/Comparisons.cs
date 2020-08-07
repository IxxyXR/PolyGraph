using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Mixture
{

    public enum Axes
    {
        X,
        Y,
        Z,
    }

    public enum TargetTypes
    {
        Vertices,
        Faces,
        Edges,
    }

    public enum FloatConditions
    {
        Equal,
        NotEqual,
        Less,
        LessOrEqual,
        Greater,
        GreaterOrEqual,
    }

    public enum IntConditions
    {
        Equal,
        NotEqual,
        Less,
        LessOrEqual,
        Greater,
        GreaterOrEqual,
        Modulo,
        NotModulo,
    }

    public enum ListConditions
    {
        Equal,
        NotEqual,
        ContainsAll,
        ContainsSome,
        ContainsNone,
        SameLength,
        DifferentLength,
        Shorter,
        ShorterOrSame,
        Longer,
        LongerOrSame,
    }

    public class FloatComparisonsHelper
    {
        public static Dictionary<FloatConditions, Func<(float, float), bool>> Comparisons =
            new Dictionary<FloatConditions, Func<(float, float), bool>>
            {
                {FloatConditions.Equal, t => t.Item1 == t.Item2},
                {FloatConditions.NotEqual, t => t.Item1 != t.Item2},
                {FloatConditions.Less, t => t.Item1 < t.Item2},
                {FloatConditions.LessOrEqual, t => t.Item1 <= t.Item2},
                {FloatConditions.Greater, t => t.Item1 > t.Item2},
                {FloatConditions.GreaterOrEqual, t => t.Item1 >= t.Item2},
            };
    }

    public class IntComparisonsHelper
    {
        public static Dictionary<IntConditions, Func<(int, int), bool>> Comparisons =
            new Dictionary<IntConditions, Func<(int, int), bool>>
            {
                {IntConditions.Equal, t => t.Item1 == t.Item2},
                {IntConditions.NotEqual, t => t.Item1 != t.Item2},
                {IntConditions.Less, t => t.Item1 < t.Item2},
                {IntConditions.LessOrEqual, t => t.Item1 <= t.Item2},
                {IntConditions.Greater, t => t.Item1 > t.Item2},
                {IntConditions.GreaterOrEqual, t => t.Item1 >= t.Item2},
                {IntConditions.Modulo, t => t.Item1 % t.Item2 == 0},
                {IntConditions.NotModulo, t => t.Item1 % t.Item2 != 0},
            };
    }

    public class ListComparisonsHelper
    {
        public static Dictionary<ListConditions, Func<(IList, IList), bool>> Comparisons =
            new Dictionary<ListConditions, Func<(IList, IList), bool>>
            {
                {ListConditions.Equal, t => t.Item1 == t.Item2},
                {ListConditions.NotEqual, t => t.Item1 != t.Item2},
                {ListConditions.ContainsAll, t =>
                    {
                        bool all = true;
                        foreach (var i in t.Item2)
                        {
                            if (!t.Item1.Contains(i))
                            {
                                all = false;
                                break;
                            }
                        }
                        return all;
                    }
                },
                {ListConditions.ContainsSome, t =>
                    {
                        bool some = false;
                        foreach (var i in t.Item2)
                        {
                            if (t.Item1.Contains(i))
                            {
                                some = true;
                                break;
                            }
                        }
                        return some;
                    }
                },
                {ListConditions.ContainsNone, t =>
                    {
                        bool none = true;
                        foreach (var i in t.Item2)
                        {
                            if (t.Item1.Contains(i))
                            {
                                none = true;
                                break;
                            }
                        }
                        return none;
                    }
                },
                {ListConditions.SameLength, t => t.Item1.Count == t.Item2.Count},
                {ListConditions.DifferentLength, t => t.Item1.Count != t.Item2.Count},
                {ListConditions.Shorter, t => t.Item1.Count < t.Item2.Count},
                {ListConditions.ShorterOrSame, t => t.Item1.Count <= t.Item2.Count},
                {ListConditions.Longer, t => t.Item1.Count > t.Item2.Count},
                {ListConditions.LongerOrSame, t => t.Item1.Count >= t.Item2.Count},
            };
    }

}