using System;
using System.Collections.Generic;
using System.Linq;
using Conway;
using UnityEngine;
using GraphProcessor;
using UnityEngine.Rendering;


namespace Mixture
{
    [Serializable, NodeMenuItem("Polyhydra/Filter/Faces by Bare Edges")]
    public class FacesByBareEdges : MixtureNode
    {

        public override string	name => "Filter Faces by Bare Edges";
        public override bool hasSettings => false;

        [Input("Bare Edges"), SerializeField]
        public int BareEdges;

        public IntConditions condition;

        [Output("Face Filter")]
        public Func<FilterParams, bool> filter;

        public override bool hasPreview => false;
        public override bool showDefaultInspector => true;

        protected override bool ProcessNode(CommandBuffer cmd)
        {
            var comparison = IntComparisonsHelper.Comparisons[condition];
            filter = p => comparison((p.poly.Faces[p.index].NakedEdges().Count(), BareEdges));
            return true;
        }
    }
}