using System;
using System.Collections.Generic;
using System.Linq;
using Conway;
using UnityEngine;
using GraphProcessor;
using UnityEngine.Rendering;


namespace Mixture
{

    [Serializable, NodeMenuItem("Polyhydra/Filter/Faces by Position")]
    public class FacesByPosition : MixtureNode
    {

        public override string	name => "Filter Faces by Position";
        public override bool hasSettings => false;

        [Input("Distance"), SerializeField]
        public float distance;

        [SerializeField]
        public Axes axis;

        public FloatConditions condition;

        [Output("Face Filter")]
        public Func<FilterParams, bool> filter;

        public override bool hasPreview => false;
        public override bool showDefaultInspector => true;

        protected override bool ProcessNode(CommandBuffer cmd)
        {
            var comparison = FloatComparisonsHelper.Comparisons[condition];
            filter = p => comparison((p.poly.Faces[p.index].Centroid[(int)axis], distance));
            return true;
        }
    }
}