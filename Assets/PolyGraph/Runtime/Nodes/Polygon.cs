using System;
using Conway;
using UnityEngine;
using GraphProcessor;
using Johnson;
using UnityEngine.Rendering;


namespace Mixture
{
	[System.Serializable, NodeMenuItem("Polyhydra/Polygon")]
	public class PolygonNode : MixtureNode
	{
		[Output("Polyhedra")]
		public ConwayPoly poly;

		[Output("Side Angle")]
		public float sideAngle;

		[Input("Sides"), SerializeField]
		public int sides;

		public override string	name => "Polygon";
		public override bool hasSettings => false;
		public override bool hasPreview => false;
		public override bool showDefaultInspector => true;

		protected override bool ProcessNode(CommandBuffer cmd)
		{
			sides = sides < 3 ? 3 : sides;
			sideAngle = 360f / sides;
			poly = JohnsonPoly.Polygon(sides);
			return true;

		}
    }
}