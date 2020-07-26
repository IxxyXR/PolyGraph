using System.Collections.Generic;
using UnityEngine;
using GraphProcessor;
using System.Linq;
using UnityEngine.Rendering;
using System;

namespace Mixture
{
	[System.Serializable, NodeMenuItem("Branch"), NodeMenuItem("Condition")]
	public class ConditionNode : MixtureNode
	{
		[Input]
		public float a;
		[Input]
		public float b;

        [Output]
        public bool output;

        public enum Conditions
        {
	        Equal,
	        NotEqual,
	        Less,
	        LessOrEqual,
	        Greater,
	        GreaterOrEqual,
        }

        [Input]
        public Conditions condition;

        [SerializeField]
        SerializableType inputType = new SerializableType(typeof(object));

		public override string	name => "Condition";

		public override bool    hasPreview => false;
		public override bool	showDefaultInspector => true;

		protected override void Enable()
		{
		}


		protected override bool ProcessNode(CommandBuffer cmd)
		{
			switch (condition)
			{
				case Conditions.Equal:
					output = a == b;
					break;
				case Conditions.NotEqual:
					output = a != b;
					break;
				case Conditions.Less:
					output = a < b;
					break;
				case Conditions.LessOrEqual:
					output = a <= b;
					break;
				case Conditions.Greater:
					output = a > b;
					break;
				case Conditions.GreaterOrEqual:
					output = a >= b;
					break;
			}
			return true;
		}
    }
}
