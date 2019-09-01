﻿using System.Collections;
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
	[NodeCustomEditor(typeof(RandomColorNode))]
	public class RandomColorNodeView : MixtureNodeView
	{
		public override void Enable()
		{
			base.Enable();

			var node = nodeTarget as RandomColorNode;

            var h = new MinMaxSlider("H", node.minHue, node.maxHue, 0.0f, 1.0f);
            var s = new MinMaxSlider("S", node.minSat, node.maxSat, 0.0f, 1.0f);
            var v = new MinMaxSlider("V", node.minValue, node.maxValue, 0.0f, 1.0f);

            h.RegisterValueChangedCallback(e => {
				owner.RegisterCompleteObjectUndo("Updated Hue " + e.newValue);
                node.minHue = e.newValue.x;
                node.maxHue = e.newValue.y;
            });
            
            s.RegisterValueChangedCallback(e => {
				owner.RegisterCompleteObjectUndo("Updated Saturation " + e.newValue);
                node.minSat = e.newValue.x;
                node.maxSat = e.newValue.y;
            });
            
            v.RegisterValueChangedCallback(e => {
				owner.RegisterCompleteObjectUndo("Updated Value " + e.newValue);
                node.minValue = e.newValue.x;
                node.maxValue = e.newValue.y;
            });

			controlsContainer.Add(h);
			controlsContainer.Add(s);
			controlsContainer.Add(v);
		}
	}
}