using System;
using System.Collections.Generic;
using System.Linq;
using Conway;
using UnityEngine;
using GraphProcessor;
using UnityEngine.Rendering;


namespace Mixture
{
    [System.Serializable, NodeMenuItem("Polyhydra/Face Remove")]
    public class FaceRemoveNode : MixtureNode
    {

        public override string	name => "Face Remove";
        public override bool hasSettings => false;

        [Input("Source Polyhydra")]
        public ConwayPoly sourcePoly;
        [Input("Face Filter")]
        public Func<FilterParams, bool> faceFilter;

        [Output("Result")]
        public ConwayPoly result;

        public override bool hasPreview => false;
        public override bool showDefaultInspector => true;

        protected override bool ProcessNode(CommandBuffer cmd)
        {
            result = sourcePoly.FaceRemove(new OpParams{filterFunc = faceFilter});
            return true;
        }
    }
}