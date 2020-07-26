using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using GraphProcessor;
using UnityEngine.Rendering;


namespace Mixture
{
    [System.Serializable, NodeMenuItem("Vector List")]
    public class VectorListNode : MixtureNode
    {

        public override string	name => "Vector List";
        public override bool hasSettings => false;

        [Input("Input"), SerializeField]
        public Vector3 input;
        [Input("Length"), SerializeField]
        public int length;

        [Output("Result")]
        public List<Vector3> result;

        public override bool hasPreview => false;
        public override bool showDefaultInspector => true;

        protected override bool ProcessNode(CommandBuffer cmd)
        {

            result = Enumerable.Repeat(input, length).ToList();
            return true;

        }
    }
}