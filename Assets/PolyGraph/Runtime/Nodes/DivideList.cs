using System.Collections.Generic;
using System.Linq;
using Conway;
using UnityEngine;
using GraphProcessor;
using UnityEngine.Rendering;


namespace Mixture
{
    [System.Serializable, NodeMenuItem("Polyhydra/Divide List")]
    public class DivideListNode : MixtureNode
    {

        public override string	name => "Divide List";
        public override bool hasSettings => false;

        [Input("A")]
        public List<float> a;
        [Input("B")]
        public List<float> b;

        [SerializeField] public float constant = 1f;

        [Output("Result")]
        public List<float> result;

        public override bool hasPreview => false;
        public override bool showDefaultInspector => true;

        protected override bool ProcessNode(CommandBuffer cmd)
        {
            if (b == null || b.Count==0)
            {
                result = a.Select(i => constant==0 ? 0 : i/constant).ToList();
            }
            else
            {
                ListUtils.Normalize(a, b);
                result = a.Zip(b, (x,y) => y==0 ? 0 : x / y).ToList();
            }

            return true;

        }
    }
}