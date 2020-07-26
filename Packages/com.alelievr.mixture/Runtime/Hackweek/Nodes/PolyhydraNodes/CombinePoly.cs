using Conway;
using UnityEngine;
using GraphProcessor;
using UnityEngine.Rendering;


namespace Mixture
{
    [System.Serializable, NodeMenuItem("Combine Polyhedra")]
    public class CombinePolyNode : MixtureNode
    {

        public override string	name => "Combine Polyhedra";
        public override bool hasSettings => false;

        [Input("Polyhydra A")]
        public ConwayPoly conwayA;
        [Input("Polyhydra B")]
        public ConwayPoly conwayB;

        public Vector3 transform = Vector3.zero;
        public Vector3 rotation = Vector3.zero;
        public float scale = 1;

        [Output("Combined")]
        public ConwayPoly combined;

        public override bool hasPreview => false;
        public override bool showDefaultInspector => true;

        protected override bool ProcessNode(CommandBuffer cmd)
        {
            conwayA.Append(conwayB, transform, Quaternion.Euler(rotation), scale);
            combined = conwayA;
            return true;

        }
    }
}