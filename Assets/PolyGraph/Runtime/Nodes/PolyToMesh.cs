using Conway;
using UnityEngine;
using GraphProcessor;
using UnityEngine.Rendering;


namespace Mixture
{
    [System.Serializable, NodeMenuItem("Polyhydra/Poly To Mesh")]
    public class PolyToMeshNode : MixtureNode
    {

        public override string	name => "Poly To Mesh";
        public override bool hasSettings => false;
        public override bool hasPreview => false;
        public override bool showDefaultInspector => true;

        [Input("Polyhydra")]
        public ConwayPoly poly;

        [Output("Mesh")]
        public MixtureMesh meshOutput;

        protected override bool ProcessNode(CommandBuffer cmd)
        {

            if (poly == null) return false;
            var mesh = PolyMeshBuilder.BuildMeshFromConwayPoly(poly, false);
            meshOutput = new MixtureMesh {mesh = mesh, localToWorld = Matrix4x4.TRS(Vector3.zero, Quaternion.identity, Vector3.one)};

            return true;

        }
    }
}