using System;
using Conway;
using UnityEngine;
using GraphProcessor;
using UnityEngine.Rendering;


namespace Mixture
{
    [Serializable, NodeMenuItem("Polyhydra/Filter/Filter Faces by Index")]
    public class FacesByIndex : MixtureNode
    {

        public override string	name => "Filter Faces by Index";
        public override bool hasSettings => false;

        [Input("Index"), SerializeField]
        public int index;

        public IntConditions condition;

        [Output("Face Filter")]
        public Func<FilterParams, bool> filter;

        public override bool hasPreview => false;
        public override bool showDefaultInspector => true;

        protected override bool ProcessNode(CommandBuffer cmd)
        {
            var comparison = IntComparisonsHelper.Comparisons[condition];
            filter = p => comparison((p.index, index));
            return true;
        }
    }
}