using System.Collections.Generic;
using System.Linq;
using Conway;
using UnityEngine;
using GraphProcessor;
using UnityEngine.Rendering;


namespace Mixture
{
    [System.Serializable, NodeMenuItem("Polyhydra/Face Properties")]
    public class FacePropertiesNode : MixtureNode
    {

        public override string	name => "Face Properties";
        public override bool hasSettings => false;

        [Input("Source Polyhydra")]
        public ConwayPoly sourcePoly;

        [Output("Face Count")]
        public int FaceCount;

        [Output("Face Centroids")]
        public List<Vector3> FaceCentroids;

        [Output("Face Normals")]
        public List<Vector3> FaceNormals;

        [Output("Face Sides")]
        public List<int> FaceSides;

        [Output("Face Indices")]
        public List<int> FaceIndices;

        public override bool hasPreview => false;
        public override bool showDefaultInspector => true;

        protected override bool ProcessNode(CommandBuffer cmd)
        {
            FaceCount = sourcePoly.Faces.Count;
            FaceCentroids = sourcePoly.Faces.Select(x=>x.Centroid).ToList();
            FaceNormals = sourcePoly.Faces.Select(x=>x.Normal).ToList();
            FaceSides = sourcePoly.Faces.Select(x=>x.Sides).ToList();
            FaceIndices = Enumerable.Range(0, sourcePoly.Faces.Count).ToList();
            return true;

        }
    }
}