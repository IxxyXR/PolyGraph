using Conway;
using UnityEngine;
using GraphProcessor;
using UnityEngine.Rendering;


namespace Mixture
{
    [System.Serializable, NodeMenuItem("Polyhydra/Transform Polyhedra")]
    public class TransformPolyNode : MixtureNode
    {

        public override string	name => "Transform Polyhedra";
        public override bool hasSettings => false;
        public override bool hasPreview => false;
        public override bool showDefaultInspector => true;

        [Input("Polyhydra")]
        public ConwayPoly poly;
        [Input("Position")]
        public Vector3 pos = Vector3.zero;
        [Input("Rotation")]
        public Vector3 rot = Vector3.zero;
        [Input("Scale")]
        public Vector3 scale = Vector3.one;

        [Output("Result")]
        public ConwayPoly result;

        protected override bool ProcessNode(CommandBuffer cmd)
        {
            result = poly.Transform(pos, rot, scale);
            return true;

        }
    }
}