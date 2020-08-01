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
            if (DirectionList.Count < PositionList.Count) DirectionList.AddRange(Enumerable.Repeat(Vector3.zero, PositionList.Count - DirectionList.Count));

            if (ScaleList.Count < PositionList.Count)
            {
                // TODO Out of bounds behaviour should be definable - loop, hold, 1, 0

                // Constant 1
                //ScaleList.AddRange(Enumerable.Repeat(Vector3.one, PositionList.Count - ScaleList.Count));

                // Loop
                while (ScaleList.Count < PositionList.Count)
                {
                    ScaleList.AddRange(ScaleList);
                }
                ScaleList.Take(PositionList.Count);

            }
            result = sourcePoly.PolyArray(PositionList, DirectionList, ScaleList);
            return true;

        }
    }
}