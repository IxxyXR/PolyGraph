using Conway;
using UnityEngine;
using GraphProcessor;
using UnityEngine.Rendering;


namespace Mixture
{
    [System.Serializable, NodeMenuItem("Polyhydra/Copy Poly To Faces")]
    public class CopyPolyToFacesNode : MixtureNode
    {

        public override string	name => "Copy Poly To Faces";
        public override bool hasSettings => false;

        [Input("Source Polyhydra")]
        public ConwayPoly sourcePoly;
        [Input("Target Polyhydra")]
        public ConwayPoly targetPoly;

        public FaceSelections facesel;
        public string tags;
        public float offset;
        public float angle;
        public float scale = 1;

        [Output("Result")]
        public ConwayPoly result;

        public override bool hasPreview => false;
        public override bool showDefaultInspector => true;

        protected override bool ProcessNode(CommandBuffer cmd)
        {
            result = targetPoly.AppendMany(sourcePoly, facesel, tags, scale, angle, offset);
            return true;

        }
    }
}