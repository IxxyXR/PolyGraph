using System.Collections.Generic;
using System.Linq;
using Conway;
using UnityEngine;
using GraphProcessor;
using UnityEngine.Rendering;


namespace Mixture
{
    [System.Serializable, NodeMenuItem("Edge Properties")]
    public class EdgePropertiesNode : MixtureNode
    {

        public override string	name => "Edge Properties";
        public override bool hasSettings => false;

        [Input("Source Polyhydra")]
        public ConwayPoly sourcePoly;

        public int EdgeDivisions = 2;

        public bool OuterEdgesOnly;

        [Output("Edge Count")]
        public int EdgeCount;

        [Output("Dihedral Angles")]
        public List<float> DihedralAngles;

        [Output("Points along edges")]
        public List<Vector3> EdgePoints;

        [Output("Edge Vectors")]
        public List<Vector3> EdgeVectors;

        [Output("Edge Indices")]
        public List<int> EdgeIndices;



        public override bool hasPreview => false;
        public override bool showDefaultInspector => true;

        protected override bool ProcessNode(CommandBuffer cmd)
        {
            EdgeCount = sourcePoly.Halfedges.Count / 2;
            return true;

        }
    }
}