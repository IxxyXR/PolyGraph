using UnityEngine;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;
using System.Collections.Generic;
using GraphProcessor;
using System.IO;
using System.Text.RegularExpressions;
using System.Linq;
using UnityEditor.Rendering;
using System;

namespace Mixture
{
	[NodeCustomEditor(typeof(ComputeShaderNode))]
	public class ComputeShaderNodeView : MixtureNodeView
	{
		VisualElement		shaderCreationUI;
		ComputeShaderNode	computeShaderNode => nodeTarget as ComputeShaderNode;

		ObjectField			debugCustomRenderTextureField = null;
		ObjectField			shaderField;

		int					materialCRC;
		DateTime			lastModified;
		[NonSerialized]
		string				computePath = null;

		bool				isDerived;

		public override void Enable()
		{
			base.Enable();

			InitializeDebug();

			var detector = schedule.Execute(DetectComputeShaderChanges);
			detector.Every(200);

			isDerived = nodeTarget.GetType() != typeof(ComputeShaderNode);

			// If the user created a custom version of compute, he doesn't need the selector + UI
			if (!isDerived)
            {
                shaderField = new ObjectField
                {
                    value = computeShaderNode.computeShader,
                    objectType = typeof(ComputeShader),
                };

                shaderField.RegisterValueChangedCallback((v) =>
                {
                    SetComputeShader((ComputeShader)v.newValue);
                });

                controlsContainer.Add(shaderField);

				shaderCreationUI = new VisualElement();
				controlsContainer.Add(shaderCreationUI);
				UpdateShaderCreationUI();

				if (computeShaderNode.computeShader != null)
					UpdateComputeShaderData(computeShaderNode.computeShader);
			}
		}

		void InitializeDebug()
		{
			// computeShaderNode.onProcessed += () => {
			// 	debugCustomRenderTextureField.value = computeShaderNode.output;
			// };

			// debugCustomRenderTextureField = new ObjectField("Output")
			// {
			// 	value = computeShaderNode.output
			// };

			debugContainer.Add(debugCustomRenderTextureField);
		}

		void UpdateShaderCreationUI()
		{
			shaderCreationUI.Clear();

			if (computeShaderNode.computeShader == null)
			{
				shaderCreationUI.Add(new Button(CreateNewComputeShader) {
					text = "New Compute Shader"
				});
			}
			else
			{
				shaderCreationUI.Add(new Button(OpenCurrentComputeShader){
					text = "Open"
				});

				shaderCreationUI.Add(new PopupField<string>("Kernel Name", computeShaderNode.kernelNames, computeShaderNode.kernelNames[computeShaderNode.kernelIndex]));
			}

			void CreateNewComputeShader()
			{
				SetComputeShader(MixtureEditorUtils.CreateComputeShader(owner.graph, title));
			}

			void OpenCurrentComputeShader()
			{
				AssetDatabase.OpenAsset(computeShaderNode.computeShader);
			}
		}

		void DetectComputeShaderChanges()
		{
			if (computeShaderNode.computeShader == null)
				return;

			if (computePath == null)
				computePath = AssetDatabase.GetAssetPath(computeShaderNode.computeShader);
			
			var modificationDate = File.GetLastWriteTime(computePath);

			if (lastModified != modificationDate)
			{
				Application.runInBackground = true;
				schedule.Execute(() => {
					// Reimport the compute shader:
					AssetDatabase.ImportAsset(computePath);
					NotifyNodeChanged();
				}).ExecuteLater(100);
				lastModified = modificationDate;

				UpdateComputeShaderData(computeShaderNode.computeShader);
			}
		}

		void SetComputeShader(ComputeShader newShader)
		{
			owner.RegisterCompleteObjectUndo("Updated Shader of Compute Shader Node");
			computeShaderNode.computeShader = newShader;
			shaderField.value = newShader;
			computePath = AssetDatabase.GetAssetPath(newShader);

			title = newShader?.name ?? "New Compute";

			if (newShader != null)
				UpdateComputeShaderData(newShader);

			UpdateShaderCreationUI();

			// We fore the update of node ports
			ForceUpdatePorts();
		}

		static Regex kernelRegex = new Regex(@"^\s*#pragma\s+kernel\s+(\w+)", RegexOptions.Multiline);
		static Regex readWriteObjects = new Regex(@"^\s*(RW|Append)(\w+)(?:<(\w+)>)?\s+(\w+)\s*;", RegexOptions.Multiline);
		static Regex readOnlyObjects = new Regex(@"^\s*(Consume|)(\w+)(?:<(\w+)>)?\s+(\w+)\s*;", RegexOptions.Multiline);

		void UpdateComputeShaderData(ComputeShader shader)
		{
			if (ShaderUtil.GetComputeShaderMessages(shader).Any(m => m.severity == ShaderCompilerMessageSeverity.Error))
			{
				Debug.LogError("Compute Shader " + shader + " has errors");
				return;
			}

			var path = AssetDatabase.GetAssetPath(shader);
			string fileContent = File.ReadAllText(path);

			// First remove all the functions from the text
			int functionMarkLocation = fileContent.IndexOf("{");
			fileContent = fileContent.Substring(0, functionMarkLocation).Trim();

			// Fill kernel names
			computeShaderNode.kernelNames.Clear();
			foreach (Match match in kernelRegex.Matches(fileContent))
				computeShaderNode.kernelNames.Add(match.Groups[1].Value);
			
			// Fill output properties name
			computeShaderNode.computeOutputs.Clear();
			foreach (Match match in readWriteObjects.Matches(fileContent))
			{
				if (match.Groups[4].Value.StartsWith("__"))
					continue;
				
				if (match.Groups[4].Value == computeShaderNode.previewTexturePropertyName)
					continue;

				computeShaderNode.computeOutputs.Add(new ComputeShaderNode.ComputeParameter{
					displayName = ObjectNames.NicifyVariableName(match.Groups[4].Value),
					propertyName = match.Groups[4].Value,
					specificType = match.Groups[3].Value,
					type = ComputeShaderTypeToCSharp(match.Groups[2].Value),
				});
			}
			// Remove output properties
			fileContent = readWriteObjects.Replace(fileContent, "");

			// We can then select input properties
			computeShaderNode.computeInputs.Clear();
			foreach (Match match in readOnlyObjects.Matches(fileContent))
			{
				if (match.Groups[4].Value.StartsWith("__"))
					continue;

				computeShaderNode.computeInputs.Add(new ComputeShaderNode.ComputeParameter{
					displayName = ObjectNames.NicifyVariableName(match.Groups[4].Value),
					propertyName = match.Groups[4].Value,
					specificType = match.Groups[3].Value,
					type = ComputeShaderTypeToCSharp(match.Groups[2].Value),
				});
			}
		}
		
		Type ComputeShaderTypeToCSharp(string computeShaderType)
		{
			switch (computeShaderType)
			{
				case "bool": return typeof(bool);
				case "int": return typeof(int);
				case "float": return typeof(float);
				case "int2":
				case "float2": return typeof(Vector2);
				case "int3":
				case "float3": return typeof(Vector3);
				case "int4":
				case "float4": return typeof(Vector4);
				case "Texture2D": return typeof(Texture2D);
				case "Texture2DArray": return typeof(Texture2DArray);
				case "Texture3D": return typeof(Texture3D);
				case "TextureCube": return typeof(Cubemap);
				case "TextureCubeArray": return typeof(CubemapArray);
				case "Buffer":
				case "ByteAddressBuffer":
				case "StructuredBuffer": return typeof(ComputeBuffer);
				default: throw new Exception("Unknown compute shader type " + computeShaderType);
			}
		}

		public override void OnRemoved()
		{
			// TODO: check if compute shader is embbeded and remove it
			// owner.graph.RemoveObjectFromGraph(computeShaderNode.material);
		}
	}
}