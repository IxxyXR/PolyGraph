using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using GraphProcessor;
using UnityEngine.Rendering;


namespace Mixture
{
    [System.Serializable, NodeMenuItem("Filter List")]
    public class FilterListNode : MixtureNode
    {

        public override string	name => "Filter List";
        public override bool hasSettings => false;

        [Input("Input")]
        public List<Vector3> input;
        [Input("Conditions")]
        public List<bool> conditions;

        [Output("Result")]
        public List<Vector3> result;

        public override bool hasPreview => false;
        public override bool showDefaultInspector => true;

        protected override bool ProcessNode(CommandBuffer cmd)
        {
            result.Clear();
            for (var i = 0; i < conditions.Count; i++)
            {
                if (conditions[i]) result.Add(input[i]);
            }
            return true;
        }
    }
}