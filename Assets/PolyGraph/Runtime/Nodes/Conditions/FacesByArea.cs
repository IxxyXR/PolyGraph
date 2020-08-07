using System;
using System.Collections.Generic;
using System.Linq;
using Conway;
using UnityEngine;
using GraphProcessor;
using UnityEngine.Rendering;


namespace Mixture
{
    [Serializable, NodeMenuItem("Polyhydra/Filter/Faces by Area")]
    public class FacesByArea : MixtureNode
    {

        public override string	name => "Filter Faces by Area";
        public override bool hasSettings => false;

        [Input("Area"), SerializeField]
        public float area;

        public FloatConditions condition;

        [Output("Face Filter")]
        public Func<FilterParams, bool> filter;

        public override bool hasPreview => false;
        public override bool showDefaultInspector => true;

        protected override bool ProcessNode(CommandBuffer cmd)
        {
            var comparison = FloatComparisonsHelper.Comparisons[condition];
            filter = p => comparison((p.poly.Faces[p.index].GetArea(), area));
            return true;
        }
    }
}