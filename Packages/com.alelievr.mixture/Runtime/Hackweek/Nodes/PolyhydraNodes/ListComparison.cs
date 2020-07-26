using System.Collections.Generic;
using UnityEngine;
using GraphProcessor;
using UnityEngine.Rendering;
using System;


namespace Mixture
{

    [System.Serializable, NodeMenuItem("List Comparison")]
    public class ListComparisonNode : MixtureNode
    {
        [Input]
        public List<float> a;
        [Input]
        public List<float> b;

        [Output]
        public List<bool> output;

        [SerializeField]
        public Conditions condition;

        public override string	name => "List Comparison";
        public override bool    hasPreview => false;
        public override bool	showDefaultInspector => true;

        protected override bool ProcessNode(CommandBuffer cmd)
        {
            var comparator = ComparisonsHelper.Comparisons[condition];
            output.Clear();
            for (var i = 0; i < Mathf.Max(a.Count, b.Count); i++)
            {
                // TODO Out of bounds behaviour should be definable - loop, hold, true, false
                output.Add(comparator((a[i % a.Count], b[i % b.Count])));
            }
            return true;
        }
    }
}
