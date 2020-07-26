using System;
using Conway;
using UnityEngine;
using GraphProcessor;
using Johnson;
using UnityEngine.Rendering;


namespace Mixture
{
	[System.Serializable, NodeMenuItem("Johnson")]
	public class JohnsonNode : MixtureNode
	{
		[Output("Polyhedra")]
		public ConwayPoly poly;

		[Output("Side Angle")]
		public float sideAngle;

		public PolyHydraEnums.JohnsonPolyTypes johnsonPolyType;

		[Input("Sides"), SerializeField]
		public int sides;

		public override string	name => "Johnson";
		public override bool hasSettings => false;
		public override bool hasPreview => false;
		public override bool showDefaultInspector => true;

		protected override bool ProcessNode(CommandBuffer cmd)
		{

			sides = sides < 3 ? 3 : sides;
			sideAngle = 360f / sides;

			poly = new ConwayPoly();

			switch (johnsonPolyType)
			{
				case PolyHydraEnums.JohnsonPolyTypes.Prism:
					poly = JohnsonPoly.Prism(sides);
					break;
				case PolyHydraEnums.JohnsonPolyTypes.Antiprism:
					poly = JohnsonPoly.Antiprism(sides);
					break;
				case PolyHydraEnums.JohnsonPolyTypes.Pyramid:
					poly = JohnsonPoly.Pyramid(sides);
					break;
				case PolyHydraEnums.JohnsonPolyTypes.ElongatedPyramid:
					poly = JohnsonPoly.ElongatedPyramid(sides);
					break;
				case PolyHydraEnums.JohnsonPolyTypes.GyroelongatedPyramid:
					poly = JohnsonPoly.GyroelongatedPyramid(sides);
					break;
				case PolyHydraEnums.JohnsonPolyTypes.Dipyramid:
					poly = JohnsonPoly.Dipyramid(sides);
					break;
				case PolyHydraEnums.JohnsonPolyTypes.ElongatedDipyramid:
					poly = JohnsonPoly.ElongatedBipyramid(sides);
					break;
				case PolyHydraEnums.JohnsonPolyTypes.GyroelongatedDipyramid:
					poly = JohnsonPoly.GyroelongatedBipyramid(sides);
					break;
				case PolyHydraEnums.JohnsonPolyTypes.Cupola:
					poly = JohnsonPoly.Cupola(sides);
					break;
				case PolyHydraEnums.JohnsonPolyTypes.ElongatedCupola:
					poly = JohnsonPoly.ElongatedCupola(sides);
					break;
				case PolyHydraEnums.JohnsonPolyTypes.GyroelongatedCupola:
					poly = JohnsonPoly.GyroelongatedCupola(sides);
					break;
				case PolyHydraEnums.JohnsonPolyTypes.OrthoBicupola:
					poly = JohnsonPoly.OrthoBicupola(sides);
					break;
				case PolyHydraEnums.JohnsonPolyTypes.GyroBicupola:
					poly = JohnsonPoly.GyroBicupola(sides);
					break;
				case PolyHydraEnums.JohnsonPolyTypes.ElongatedOrthoBicupola:
					poly = JohnsonPoly.ElongatedBicupola(sides, false);
					break;
				case PolyHydraEnums.JohnsonPolyTypes.ElongatedGyroBicupola:
					poly = JohnsonPoly.ElongatedBicupola(sides, true);
					break;
				case PolyHydraEnums.JohnsonPolyTypes.GyroelongatedBicupola:
					poly = JohnsonPoly.GyroelongatedBicupola(sides, false);
					break;
				// The distinction between these two is simply one of chirality
				// case PolyHydraEnums.JohnsonPolyTypes.GyroelongatedOrthoBicupola:
				// 	poly = JohnsonPoly.GyroElongatedBicupola(sides, false);
				// 	break;
				// case PolyHydraEnums.JohnsonPolyTypes.GyroelongatedGyroBicupola:
				// 	poly = JohnsonPoly.GyroElongatedBicupola(sides, true);
				// 	break;
				case PolyHydraEnums.JohnsonPolyTypes.Rotunda:
					poly = JohnsonPoly.Rotunda();
					break;
				case PolyHydraEnums.JohnsonPolyTypes.ElongatedRotunda:
					poly = JohnsonPoly.ElongatedRotunda();
					break;
				case PolyHydraEnums.JohnsonPolyTypes.GyroelongatedRotunda:
					poly = JohnsonPoly.GyroelongatedRotunda();
					break;
				case PolyHydraEnums.JohnsonPolyTypes.GyroelongatedBirotunda:
					poly = JohnsonPoly.GyroelongatedBirotunda();
					break;
			}

			return true;

		}
    }
}