using System;
using System.Collections.Generic;
using System.Linq;
using Conway;
using UnityEngine;
using GraphProcessor;
using UnityEngine.Rendering;


namespace Mixture
{
    [Serializable, NodeMenuItem("Polyhydra/Filter/Filter Faces by # Sides")]
    public class FacesByNumberOfSidesNode : MixtureNode
    {

        public override string	name => "Faces by # Sides";
        public override bool hasSettings => false;

        [Input("Sides"), SerializeField]
        public int sides;

        public IntConditions condition;

        [Output("Face Filter")]
        public Func<FilterParams, bool> filter;

        public override bool hasPreview => false;
        public override bool showDefaultInspector => true;

        protected override bool ProcessNode(CommandBuffer cmd)
        {
            var comparison = IntComparisonsHelper.Comparisons[condition];
            filter = p => comparison((p.poly.Faces[p.index].Sides, sides));
            return true;
        }
    }
}