using System;
using System.Collections.Generic;
using System.Linq;
using Conway;
using UnityEngine;
using GraphProcessor;
using UnityEngine.Rendering;


namespace Mixture
{
    [Serializable, NodeMenuItem("Polyhydra/Filter/And")]
    public class FilterAnd : MixtureNode
    {

        public override string	name => "Filter And";
        public override bool hasSettings => false;

        [Input("Filter A")]
        public Func<FilterParams, bool> FilterA;

        [Input("Filter B")]
        public Func<FilterParams, bool> FilterB;

        [Output("Face Filter")]
        public Func<FilterParams, bool> filter;

        public override bool hasPreview => false;
        public override bool showDefaultInspector => true;

        protected override bool ProcessNode(CommandBuffer cmd)
        {
            filter = p => FilterA(p) && FilterB(p);
            return true;
        }
    }
}