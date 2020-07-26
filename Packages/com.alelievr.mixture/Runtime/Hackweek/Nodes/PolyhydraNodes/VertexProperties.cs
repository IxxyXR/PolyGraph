using System.Collections.Generic;
using System.Linq;
using Conway;
using UnityEngine;
using GraphProcessor;
using UnityEngine.Rendering;


namespace Mixture
{
    [System.Serializable, NodeMenuItem("Vertex Properties")]
    public class VertexPropertiesNode : MixtureNode
    {

        public override string	name => "Vertex Properties";
        public override bool hasSettings => false;

        [Input("Source Polyhydra")]
        public ConwayPoly sourcePoly;

        [Output("Vertex Count")]
        public int VertexCount;

        [Output("Vertices")]
        public List<Vector3> Vertices;

        [Output("Vertex Normals")]
        public List<Vector3> VertexNormals;

        public override bool hasPreview => false;
        public override bool showDefaultInspector => true;

        protected override bool ProcessNode(CommandBuffer cmd)
        {
            VertexCount = sourcePoly.Vertices.Count;

            Vertices = sourcePoly.Vertices.Select(x=>x.Position).ToList();
            VertexNormals = sourcePoly.Vertices.Select(x=>x.Normal).ToList();
            return true;

        }
    }
}