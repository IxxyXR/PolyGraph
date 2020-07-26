using System;
using System.Collections.Generic;
using System.Linq;
using Conway;
using UnityEngine;
using GraphProcessor;
using UnityEngine.Rendering;


namespace Mixture
{
    [System.Serializable, NodeMenuItem("Filter Faces by # Sides")]
    public class FacesByNumberOfSidesNode : MixtureNode
    {

        public override string	name => "Filter Faces by # Sides";
        public override bool hasSettings => false;

        [Input("Polyhedra")]
        public ConwayPoly poly;
        [Input("Sides"), SerializeField]
        public int sides;

        public Conditions condition;

        [Output("Face List")]
        public List<int> result;

        public override bool hasPreview => false;
        public override bool showDefaultInspector => true;

        protected override bool ProcessNode(CommandBuffer cmd)
        {
            var comparator = ComparisonsHelper.Comparisons[condition];
            result.Clear();
            for (var i = 0; i < poly.Faces.Count; i++)
            {
                if (comparator((poly.Faces[i].Sides, sides))) result.Add(i);
            }
            return true;
        }
    }
}