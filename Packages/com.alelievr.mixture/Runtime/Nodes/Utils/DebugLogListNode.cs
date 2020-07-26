using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GraphProcessor;
using System.Linq;
using UnityEngine.Rendering;

namespace Mixture
{
	[System.Serializable, NodeMenuItem("Utils/Debug Log List")]
	public class DebugLogListNode : MixtureNode, INeedsCPU
	{
		[Input("Boolean")]
		public List<Vector3> boolInput;
		[Input("Float")]
		public List<Vector3> floatInput;
		[Input("Integer")]
		public List<Vector3> intInput;
		[Input("Vector3")]
		public List<Vector3> vectorInput;
		[Input("String")]
		public List<Vector3> stringInput;

		public override string name => "Debug Log List";

		protected override bool ProcessNode(CommandBuffer cmd)
		{
			foreach (var i in boolInput) Debug.Log($"Bool: {i}");
			foreach (var i in floatInput) Debug.Log($"Float: {i}");
			foreach (var i in intInput) Debug.Log($"Int: {i}");
			foreach (var i in vectorInput) Debug.Log($"Vector3: {i}");
			foreach (var i in stringInput) Debug.Log($"String: {i}");
			return true;
		}
	}
}