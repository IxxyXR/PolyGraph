using Conway;
using UnityEngine;
using GraphProcessor;
using UnityEngine.Rendering;


namespace Mixture
{
    [System.Serializable, NodeMenuItem("Polyhydra/Divide Float")]
    public class DivideFloatNode : MixtureNode
    {

        public override string	name => "Divide Float";
        public override bool hasSettings => false;

        [Input("A"), SerializeField]
        public float a;
        [Input("B"), SerializeField]
        public float b;

        [Output("Result")]
        public float result;

        public override bool hasPreview => false;
        public override bool showDefaultInspector => true;

        protected override bool ProcessNode(CommandBuffer cmd)
        {
            result = b == 0 ? 0 : a / b;
            return true;

        }
    }
}