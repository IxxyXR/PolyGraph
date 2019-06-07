﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GraphProcessor;
using System.Linq;
using System;
using Object = UnityEngine.Object;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Mixture
{
	public abstract class MixtureNode : BaseNode
	{
		protected new MixtureGraph	graph => base.graph as MixtureGraph;

		protected void AddObjectToGraph(Object obj) => graph.AddObjectToGraph(obj);
		protected void RemoveObjectFromGraph(Object obj) => graph.RemoveObjectFromGraph(obj);

		protected bool	UpdateTempRenderTexture(ref RenderTexture target)
		{
			if (target == null)
			{
				target = new RenderTexture(graph.outputTexture.width, graph.outputTexture.height, 0, graph.outputTexture.graphicsFormat);
				return true;
			}

			if (target.width != graph.outputTexture.width
				|| target.height != graph.outputTexture.height
				|| target.graphicsFormat != graph.outputTexture.graphicsFormat
				|| target.dimension != graph.outputTexture.dimension
				|| target.filterMode != graph.outputTexture.filterMode)
			{
				target.Release();
				target.width = graph.outputTexture.width;
				target.height = graph.outputTexture.height;
				target.graphicsFormat = graph.outputTexture.graphicsFormat;
				target.dimension = graph.outputTexture.dimension;
				target.filterMode = graph.outputTexture.filterMode;
				target.Create();
			}

			return false;
		}

#if UNITY_EDITOR
		protected Type GetPropertyType(MaterialProperty.PropType type)
		{
			switch (type)
			{
				case MaterialProperty.PropType.Color:
					return typeof(Color);
				case MaterialProperty.PropType.Float:
				case MaterialProperty.PropType.Range:
					return typeof(float);
				case MaterialProperty.PropType.Texture:
					return typeof(Texture);
				default:
				case MaterialProperty.PropType.Vector:
					return typeof(Vector4);
			}
		}

		protected IEnumerable< PortData > GetMaterialPortDatas(Material material)
		{
			if (material == null)
				yield break;

			foreach (var prop in MaterialEditor.GetMaterialProperties(new []{material}))
			{
				if (prop.flags == MaterialProperty.PropFlags.HideInInspector
					|| prop.flags == MaterialProperty.PropFlags.NonModifiableTextureData
					|| prop.flags == MaterialProperty.PropFlags.PerRendererData)
					continue;

				yield return new PortData{
					identifier = prop.name,
					displayName = prop.displayName,
					displayType = GetPropertyType(prop.type),
				};
			}
		}

		protected void AssignMaterialPropertiesFromEdges(List< SerializableEdge > edges, Material material)
		{
			// Update material settings when processing the graph:
			foreach (var edge in edges)
			{
				var prop = MaterialEditor.GetMaterialProperty(new []{material}, edge.inputPort.portData.identifier);

				switch (prop.type)
				{
					case MaterialProperty.PropType.Color:
						prop.colorValue = (Color)edge.passThroughBuffer;
						break;
					case MaterialProperty.PropType.Texture:
						// TODO: texture scale and offset
						prop.textureValue = (Texture)edge.passThroughBuffer;
						break;
					case MaterialProperty.PropType.Float:
					case MaterialProperty.PropType.Range:
						prop.floatValue = (float)edge.passThroughBuffer;
						break;
					case MaterialProperty.PropType.Vector:
						prop.vectorValue = (Vector4)edge.passThroughBuffer;
						break;
				}
			}
		}
#endif
	}
}