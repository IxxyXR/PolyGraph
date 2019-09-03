﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GraphProcessor;
using System.Linq;

namespace Mixture
{
	[System.Serializable, NodeMenuItem("Operators/Vector Frac")]
	public class VectorFracNode : MixtureNode
	{
		// TODO: multi VectorFrac port

		[Input("A")]
		public Vector4	a;
		
		[Output("Out")]
		public Vector4	o;

		public override string name => "Frac";

		protected override bool ProcessNode()
		{
			o = new Vector4(a.x % 1.0f, a.y % 1.0f, a.z % 1.0f, a.w % 1.0f);
			return false;
		}
	}
}