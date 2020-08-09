using System;
using Conway;
using UnityEngine;
using GraphProcessor;
using UnityEngine.Rendering;


namespace Mixture
{

    [Serializable, NodeMenuItem("Polyhydra/Filter/Filter Faces by Direction")]
    public class FacesByDirection : MixtureNode
    {

        public override string	name => "Filter Faces by Direction";
        public override bool hasSettings => false;

        [Input("Angle"), SerializeField]
        public float Angle;

        [SerializeField]
        public Axes Axis;

        [SerializeField] public bool Absolute;
        [SerializeField] public float MarginOfError;

        public FloatConditions condition;

        [Output("Face Filter")]
        public Func<FilterParams, bool> filter;

        public override bool hasPreview => false;
        public override bool showDefaultInspector => true;

        protected override bool ProcessNode(CommandBuffer cmd)
        {
            var axisVector = Axis == Axes.Z ? Vector3.forward : Axis == Axes.Y ? Vector3.up : Vector3.right;

            if (condition == FloatConditions.Equal)
            {
                filter = p =>
                {
                    var a2 = Vector3.Angle(axisVector, p.poly.Faces[p.index].Normal);
                    return Math.Abs(a2 - Angle) < MarginOfError;
                };
            }
            else if (condition == FloatConditions.NotEqual)
            {
                filter = p =>
                {
                    var a2 = Vector3.Angle(axisVector, p.poly.Faces[p.index].Normal);
                    return Math.Abs(a2 - Angle) > MarginOfError;
                };
            }
            else
            {
                var comparison = FloatComparisonsHelper.Comparisons[condition];
                filter = p =>
                {
                    var a2 = Vector3.Angle(axisVector, p.poly.Faces[p.index].Normal);
                    return comparison((a2, Angle));
                };
            }
            return true;
        }
    }
}