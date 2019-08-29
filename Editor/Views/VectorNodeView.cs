using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;
using GraphProcessor;
using System.Linq;

namespace Mixture
{
	[NodeCustomEditor(typeof(VectorNode))]
	public class VectorNodeView : MixtureNodeView
	{
		public override void Enable()
		{
			base.Enable();
			DrawDefaultInspector();
		}
	}
}