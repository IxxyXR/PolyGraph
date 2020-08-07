using System.Collections.Generic;
using System.Linq;
using GraphProcessor;
using UnityEngine;


namespace Mixture
{
    public class CustomConversions : ITypeAdapter

    {
        public static string ConvertFloatToString(float from) => from.ToString();
        public static string ConvertBoolToString(bool from) => from?"True":"False";

        public static double ConvertFloatToDouble(float from) => (double) from;

        public static Color ConvertFloatToColor(float from) => Color.white * from;

        public static float ConvertStringToFloat(string from) => float.Parse(from);

        //public static float ConvertIntToFloat(int from) => (float) from;
        public static float ConvertDoubleToFloat(double from) => (float) from;

        //public static float ConvertVector3ToFloat(Vector3 from) => from.x;
        public static float ConvertColorToFloat(Color from) => from.grayscale;
        public static float ConvertVectorListToFloat(List<Vector3> from) => from[0].x;
        public static float ConvertBoolToFloat(bool from) => from?1f:0f;

        //public static Vector3 ConvertFloatToVector3(float from) => Vector3.one * from;
        public static Vector3 ConvertIntToVector3(int from) => Vector3.one * from;
        public static Vector3 ConvertVectorListToVector3(List<Vector3> from) => from[0];
        public static Vector3 ConvertBoolToVector3(bool from) => from?Vector3.one:Vector3.zero;
        public static Vector3 ConvertVector4ToVector3(Vector4 from) => new Vector3(from.x, from.y, from.z);

        public static Vector4 ConvertIntToVector4(int from) => Vector4.one * from;
        public static Vector4 ConvertVectorListToVector4(List<Vector3> from) => from[0];
        public static Vector4 ConvertBoolToVector4(bool from) => from?Vector4.one:Vector4.zero;
        public static Vector4 ConvertVector3ToVector4(Vector3 from) => new Vector4(from.x, from.y, from.z, 0);

        //public static int ConvertFloatToint(float from) => Mathf.FloorToInt(from);
        public static int ConvertVector3ToInt(Vector3 from) => Mathf.FloorToInt(from.x);
        public static int ConvertVectorListToInt(List<Vector3> from) => Mathf.FloorToInt(from[0].x);
        public static int ConvertBoolToInt(bool from) => from?1:0;

        public static List<Vector3> ConvertIntListToVectorList(List<int> from) => from.Select(x=>Vector3.one * x).ToList();
        public static List<Vector3> ConvertVector3ToVectorList(Vector3 from) => new List<Vector3> {from};
        public static List<Vector3> ConvertFloatToVectorList(float from) => new List<Vector3> {Vector3.one * from};
        public static List<Vector3> ConvertIntToVectorList(int from) => new List<Vector3> {ConvertIntToVector3(from)};
        public static List<Vector3> ConvertBoolToVectorList(bool from) => new List<Vector3> {from?Vector3.one:Vector3.zero};

        public static List<float> ConvertIntListToFloatList(List<int> from) => from.Select(x=>(float)x).ToList();
        public static List<float> ConvertVector3ListToFloatList(List<Vector3> from) => from.Select(x=>x.x).ToList();
        public static List<float> ConvertVector3ToFloatList(Vector3 from) => new List<float> {from.x};
        public static List<float> ConvertFloatToFloatList(float from) => new List<float> {from};
        public static List<float> ConvertIntToFloatList(int from) => new List<float> {from};
        public static List<float> ConvertBoolToFloatList(bool from) => new List<float> {from?1f:0f};

        public static List<int> ConvertFloatListToIntList(List<float> from) => from.Select(Mathf.FloorToInt).ToList();
        public static List<int> ConvertVectorListToIntList(List<Vector3> from) => from.Select(x => Mathf.FloorToInt(x.x)).ToList();
        public static List<int> ConvertVector3ToIntList(Vector3 from) => new List<int> {Mathf.FloorToInt(from.x)};
        public static List<int> ConvertFloatToIntList(float from) => new List<int> {Mathf.FloorToInt(from)};
        public static List<int> ConvertIntToIntList(int from) => new List<int> {from};
        public static List<int> ConvertBoolToIntList(bool from) => new List<int> {from?1:0};

        public static bool ConvertFloatListToBool(List<float> from) => from.Count!=0 && from.TrueForAll(x=>x!=0);
        public static bool ConvertVectorListToBool(List<Vector3> from) => from.Count!=0 && from.TrueForAll(x=>x!=Vector3.zero);
        public static bool ConvertVector3ToBool(Vector3 from) => from!=Vector3.zero;
        public static bool ConvertFloatToBool(float from) => from != 0;
        public static bool ConvertIntToBool(int from) => from != 0;
        public static bool ConvertStringToBool(string from) => from != "" && from !="false" && from != null;
    }
}