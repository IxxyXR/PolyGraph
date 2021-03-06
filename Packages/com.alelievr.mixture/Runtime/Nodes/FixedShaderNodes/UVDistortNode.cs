﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GraphProcessor;
using System.Linq;

namespace Mixture
{
	[System.Serializable, NodeMenuItem("Coordinates/UV Distort")]
	public class UVDistortNode : FixedShaderNode
	{
		public override string name => "UV Distort";

		public override string shaderName => "Hidden/Mixture/UVDistort";

		public override bool displayMaterialInspector => true;

        public override bool hasSettings => true;

        protected override MixtureRTSettings defaultRTSettings => new MixtureRTSettings()
        {
            dimension = OutputDimension.Texture2D,
            widthMode = OutputSizeMode.Default,
            heightMode = OutputSizeMode.Default,
            depthMode = OutputSizeMode.Fixed,
            sliceCount = 1,
            targetFormat = OutputFormat.RGBA_Float,
            doubleBuffered = false,
            filterMode = FilterMode.Bilinear,
            wrapMode = TextureWrapMode.Repeat,
            editFlags = EditFlags.WidthMode | EditFlags.HeightMode | EditFlags.Width | EditFlags.Height,
        };

		protected override bool ProcessNode()
        {
            if (!base.ProcessNode())
                return false;
            
			var uvPort = inputPorts.Find(p => p.portData.identifier.Contains("_UV_"));
            if (uvPort == null)
                return false;
            
            material.SetKeywordEnabled("USE_CUSTOM_UV", uvPort.GetEdges().Count != 0);
            return true;
        }

    }
}