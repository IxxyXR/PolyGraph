﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GraphProcessor;
using System.Linq;

namespace Mixture
{
	[System.Serializable, NodeMenuItem("Custom/Ridged Cellular Noise")]
	public class RidgedCellularNoise : FixedNoiseNode
	{
		public override string name => "Ridged Cellular Noise";

		public override string shaderName => "Hidden/Mixture/RidgedCellularNoise";
	}
}