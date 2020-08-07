using System;
using System.Collections.Generic;
using Conway;
using UnityEngine;
using GraphProcessor;
using UnityEngine.Rendering;


namespace Mixture
{
    [Serializable, NodeMenuItem("Polyhydra/Filter/Faces by Index List")]
    public class FacesByIndexList : MixtureNode
    {

        public override string	name => "Filter Faces by Index List";
        public override bool hasSettings => false;

        [Input("Index"), SerializeField]
        public List<int> indexList;

        [Output("Face Filter")]
        public Func<FilterParams, bool> filter;

        public override bool hasPreview => false;
        public override bool showDefaultInspector => true;

        protected override bool ProcessNode(CommandBuffer cmd)
        {
            filter = p => indexList.Contains(p.index);
            return true;
        }
    }
}