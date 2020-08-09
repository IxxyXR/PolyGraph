using System;
using System.Collections.Generic;
using System.Linq;
using Conway;
using UnityEngine;
using GraphProcessor;
using UnityEngine.Rendering;


namespace Mixture
{
    [Serializable, NodeMenuItem("Polyhydra/Filter/Filter Faces by Role")]
    public class FacesByRole : MixtureNode
    {

        public override string	name => "Filter Faces by # Sides";
        public override bool hasSettings => false;

        [Input("Role"), SerializeField]
        public ConwayPoly.Roles role;

        [Output("Face Filter")]
        public Func<FilterParams, bool> filter;

        public override bool hasPreview => false;
        public override bool showDefaultInspector => true;

        protected override bool ProcessNode(CommandBuffer cmd)
        {
            filter = p => p.poly.FaceRoles[p.index] == role;
            return true;
        }
    }
}