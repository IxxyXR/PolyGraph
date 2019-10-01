using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Unity.Jobs;
using Unity.Collections;
using GraphProcessor;

namespace Mixture
{
	public class MixtureProcessor : BaseGraphProcessor
	{
		List< BaseNode >		processList;
		new MixtureGraph		graph => base.graph as MixtureGraph;

		public MixtureProcessor(BaseGraph graph) : base(graph) {}

		public override void UpdateComputeOrder() {}

		public override void Run()
		{
			processList = graph.nodes.OrderBy(n => n.computeOrder).ToList();

			int count = processList.Count;

			// The process of the mixture graph will update all CRTs,
			// assign their materials and set local material values
			for (int i = 0; i < count; i++)
			{
				var node = processList[i];

				processList[i].OnProcess();

				// Temporary hack: Custom Textures are not updated when the Shader / the Material is updated
				// and inside the dependency tree of a CRT. So we need to manually update all CRTs.
				if (node is ShaderNode s)
				{
					// the CRT output will be null if there are procesing errors
					if (s.output != null)
						s.output.Update();
				}
			}

			graph.outputNode.tempRenderTexture.Update();

            var external = graph.nodes.FindAll(node => node.GetType() == typeof(ExternalOutputNode));
            foreach(var node in external)
            {
                (node as ExternalOutputNode).tempRenderTexture.Update();
            }
		}
	}
}