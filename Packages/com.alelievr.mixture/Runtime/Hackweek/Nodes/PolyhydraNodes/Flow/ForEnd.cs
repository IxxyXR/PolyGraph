
using System.Collections.Generic;
using UnityEngine;
using GraphProcessor;
using System.Linq;
using UnityEngine.Rendering;
using System;
using Conway;

namespace Mixture
{
	[System.Serializable, NodeMenuItem("Poly For End")]
	public class PolyForEnd : ForeachEnd
	{
		[Input("Input")]
		public ConwayPoly input;

        [Output("Output")]
        public ConwayPoly output;

		public override string	name => "Poly For End";

		public override bool    hasPreview => false;
		public override bool    showDefaultInspector => true;

		protected override void Enable()
		{
		}

		// Functions with Attributes must be either protected or public otherwise they can't be accessed by the reflection code
		[CustomPortBehavior(nameof(input))]
		public IEnumerable< PortData > ListMaterialProperties(List< SerializableEdge > edges)
		{
            yield return new PortData
            {
                identifier = nameof(input),
                displayName = "Input",
                acceptMultipleEdges = false,
                displayType = typeof(object), // TODO
            };
		}


		// protected override bool ProcessNode(CommandBuffer cmd)
		// {
		// 	output = inputMesh.Clone();
		// 	return true;
		// }

		public void PrepareNewIteration()
		{
			// output = new ConwayPoly();
		}

		public void FinalIteration()
		{
			// output = CurrentResult;
		}
    }
}
