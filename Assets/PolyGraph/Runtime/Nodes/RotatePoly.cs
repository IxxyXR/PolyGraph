using Conway;
using UnityEngine;
using GraphProcessor;
using UnityEngine.Rendering;


namespace Mixture
{
    [System.Serializable, NodeMenuItem("Polyhydra/Rotate Polyhedra")]
    public class RotatePolyNode : MixtureNode
    {

        public override string	name => "Rotate Polyhedra";
        public override bool hasSettings => false;

        [Input("Polyhydra")]
        public ConwayPoly poly;
        [Input("X"), SerializeField]
        public float x;
        [Input("Y"), SerializeField]
        public float y;
        [Input("Z"), SerializeField]
        public float z;

        [Output("Result")]
        public ConwayPoly result;

        public override bool hasPreview => false;
        public override bool showDefaultInspector => true;

        protected override bool ProcessNode(CommandBuffer cmd)
        {
            result = poly.Transform(Vector3.zero, new Vector3(x, y, z), Vector3.one);
            return true;

        }
    }
}