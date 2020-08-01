using System.Collections.Generic;
using System.Linq;
using Conway;
using UnityEngine;
using GraphProcessor;
using UnityEngine.Rendering;


namespace Mixture
{
    [System.Serializable, NodeMenuItem("Polyhydra/Array Poly On Poly")]
    public class ArrayPolyOnPolyNode : MixtureNode
    {

        public override string	name => "Array Poly On Poly";
        public override bool hasSettings => false;

        [Input("Source Polyhydra")]
        public ConwayPoly SourcePoly;
        [Input("Target Polyhydra")]
        public ConwayPoly TargetPoly;
        [Input("Face/Vertex List")]
        public List<int> IndexList;

        public TargetTypes TargetType;

        [Output("Result")]
        public ConwayPoly result;

        public override bool hasPreview => false;
        public override bool showDefaultInspector => true;

        protected override bool ProcessNode(CommandBuffer cmd)
        {
            int count = 0;
            var positionList = new List<Vector3>();
            var directionList = new List<Vector3>();
            var scaleList = new List<Vector3>();

            switch (TargetType)
            {
                case TargetTypes.Faces:
                    count = TargetPoly.Faces.Count;
                    positionList = FilterByIndex(TargetPoly.Faces.ToList(), IndexList).Select(i => i.Centroid).ToList();
                    directionList = FilterByIndex(TargetPoly.Faces.ToList(), IndexList).Select(i => i.Normal).ToList();
                    break;
                case TargetTypes.Vertices:
                    count = TargetPoly.Vertices.Count;
                    positionList = FilterByIndex(TargetPoly.Vertices.ToList(), IndexList).Select(i => i.Position).ToList();
                    directionList = FilterByIndex(TargetPoly.Vertices.ToList(), IndexList).Select(i => i.Normal).ToList();
                    break;
                case TargetTypes.Edges:
                    count = TargetPoly.Halfedges.Count;
                    positionList = FilterByIndex(TargetPoly.Halfedges.ToList(), IndexList).Select(i => i.Midpoint).ToList();
                    directionList = FilterByIndex(TargetPoly.Halfedges.ToList(), IndexList).Select(i => i.Vector).ToList();
                    break;
            }

            scaleList = Enumerable.Repeat(Vector3.one, count).ToList();
            result = SourcePoly.PolyArray(positionList, directionList, scaleList);
            return true;

        }

        private static List<T> FilterByIndex<T>(List<T> source, List<int> indexList)
        {
            // Passing in nothing means "return everything"
            if (indexList == null || indexList.Count == 0) return source;

            var result = new List<T>();
            for (var index = 0; index < source.Count; index++)
            {
                if (indexList.Contains(index)) result.Add(source[index]);
            }
            return result;
        }
    }
}