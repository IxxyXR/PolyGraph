﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GraphProcessor;
using System.Linq;

namespace Mixture
{
	[System.Serializable, NodeMenuItem("Noises/White Noise")]
	public class WhiteNoise : FixedShaderNode
	{
		public override string name => "WhiteNoise";

		public override string shaderName => "Hidden/Mixture/White Noise";

		public override bool displayMaterialInspector => true;

		// Enumerate the list of material properties that you don't want to be turned into a connectable port.
		protected override IEnumerable<string> filteredOutProperties => new string[]{};

		// Override this if you node is not compatible with all dimensions
		// public override List<OutputDimension> supportedDimensions => new List<OutputDimension>() {
		// 	OutputDimension.Texture2D,
		// 	OutputDimension.Texture3D,
		// 	OutputDimension.CubeMap,
		// };
	}
}