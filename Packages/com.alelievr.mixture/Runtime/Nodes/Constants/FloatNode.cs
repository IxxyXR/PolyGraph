using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GraphProcessor;
using System;

namespace Mixture
{
	[System.Serializable, NodeMenuItem("Constants/Float")]
	public class FloatNode : BaseConstantNode
	{
		[Output(name = "Float")]
		public float Float = 1.0f;

		public override string	name => "Float";
	}
}