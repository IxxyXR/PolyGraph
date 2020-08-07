using System.Collections;
using System.Collections.Generic;


namespace Mixture
{
    public class ListUtils
    {
        public static void PadLoop<T>(List<T> list, List<T> target)
        {
            if (list.Count < target.Count)
            {
                int origLength = list.Count;
                for (int i = 0; i < target.Count; i++)
                {
                    list.Add(list[i % origLength]);
                }
            }
        }

        public static void PadLast<T>(List<T> list, List<T> target)
        {
            if (list.Count < target.Count)
            {
                int origLength = list.Count;
                for (int i = 0; i < target.Count; i++)
                {
                    list.Add(list[origLength - 1]);
                }
            }
        }

        public static void PadConstant<T>(List<T> list, List<T> target, T val)
        {
            if (list.Count < target.Count)
            {
                for (int i = 0; i < target.Count; i++)
                {
                    list.Add(val);
                }
            }
        }

        public static void Normalize<T>(List<T> a, List<T> b)
        {
            if (a.Count < b.Count)
            {
                PadLast(a, b);
            }
            else if (b.Count < a.Count)
            {
                PadLast(b, a);
            }
        }
    }
}