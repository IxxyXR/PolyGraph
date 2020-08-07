using System.Collections.Generic;
using System.Linq;
using Conway;
using UnityEngine;
using GraphProcessor;
using UnityEngine.Rendering;


namespace Mixture
{
    [System.Serializable, NodeMenuItem("Polyhydra/PolyArray")]
    public class PolyArrayNode : MixtureNode
    {

        public override string	name => "PolyArray";
        public override bool hasSettings => false;

        [Input("Source Polyhydra")]
        public ConwayPoly sourcePoly;
        [Input("Position List")]
        public List<Vector3> PositionList;
        [Input("Direction List")]
        public List<Vector3> DirectionList;
        [Input("Scale List")]
        public List<Vector3> ScaleList;

        [Output("Result")]
        public ConwayPoly result;

        public override bool hasPreview => false;
        public override bool showDefaultInspector => true;

        protected override bool ProcessNode(CommandBuffer cmd)
        {
            ListUtils.PadConstant(DirectionList, PositionList, Vector3.zero);
            ListUtils.PadConstant(ScaleList, PositionList, Vector3.one);
            result = sourcePoly.PolyArray(PositionList, DirectionList, ScaleList);
            return true;

        }
    }
}