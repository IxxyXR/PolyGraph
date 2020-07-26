using System.Collections.Generic;
using UnityEngine;
using GraphProcessor;
using System;
using UnityEngine.Rendering;

namespace Mixture
{
	[System.Serializable]
	public class OutputNode : MixtureNode, IUseCustomRenderTextureProcessing
	{
		[Input(name = "In")]
		public Texture			input;
		public bool				hasMips = false;

		public Shader			customMipMapShader;

		// We use a temporary renderTexture to display the result of the graph
		// in the preview so we don't have to readback the memory each time we change something
		[NonSerialized, HideInInspector]
		public CustomRenderTexture	tempRenderTexture;

		// A second temporary render texture with mip maps is needed to generate the custom mip maps.
		// It's needed because we can't read/write to the same render target even between different mips
		[NonSerialized, HideInInspector]
		public CustomRenderTexture	mipmapRenderTexture;

		// Serialized properties for the view:
		public int					currentSlice;

		public event Action			onTempRenderTextureUpdated;

		public override string		name => "Output Texture Asset";
		public override Texture 	previewTexture => graph.isRealtime ? graph.outputTexture : tempRenderTexture;
		public override float		nodeWidth => 350;

		Material					_finalCopyMaterial;
		protected Material			finalCopyMaterial
		{
			get
			{
				if (_finalCopyMaterial == null)
					_finalCopyMaterial = CoreUtils.CreateEngineMaterial(Shader.Find("Hidden/Mixture/FinalCopy"));
				return _finalCopyMaterial;
			}
		}

		Material					_customMipMapMaterial;
		Material					customMipMapMaterial
		{
			get
			{
				if (_customMipMapMaterial == null || _customMipMapMaterial.shader != customMipMapShader)
				{
					if (_customMipMapMaterial != null)
						Material.DestroyImmediate(_customMipMapMaterial, false);
					_customMipMapMaterial = new Material(customMipMapShader);
				}

				return _customMipMapMaterial;
			}
		}

		MaterialPropertyBlock				mipMapPropertyBlock;

		// Compression settings
		public MixtureCompressionFormat		compressionFormat = MixtureCompressionFormat.DXT5;
		public MixtureCompressionQuality	compressionQuality = MixtureCompressionQuality.Best;
		public bool							enableCompression = false;

		// TODO: move this to NodeGraphProcessor
		[NonSerialized]
		protected HashSet< string > uniqueMessages = new HashSet< string >();

		protected override MixtureRTSettings defaultRTSettings
        {
            get => new MixtureRTSettings()
            {
                widthMode = OutputSizeMode.Fixed,
                heightMode = OutputSizeMode.Fixed,
                depthMode = OutputSizeMode.Fixed,
                width = 512,
                height = 512,
                sliceCount = 1,
				potSize = POTSize._1024,
                editFlags = EditFlags.POTSize | EditFlags.Width | EditFlags.Height | EditFlags.Depth | EditFlags.Dimension | EditFlags.TargetFormat
            };
        }

        protected override void Enable()
        {
			// Sanitize the RT Settings for the output node, they must contains only valid information for the output node
			if (rtSettings.targetFormat == OutputFormat.Default)
				rtSettings.targetFormat = OutputFormat.RGBA_Float;
			if (rtSettings.dimension == OutputDimension.Default)
				rtSettings.dimension = OutputDimension.Texture2D;
			rtSettings.editFlags |= EditFlags.POTSize;

			if (graph.isRealtime)
			{
				tempRenderTexture = graph.outputTexture as CustomRenderTexture;
			}
			else
			{
				UpdateTempRenderTexture(ref tempRenderTexture, hasMips, customMipMapShader == null);
				graph.onOutputTextureUpdated += () => {
					UpdateTempRenderTexture(ref tempRenderTexture, hasMips, customMipMapShader == null);
				};
			}
			tempRenderTexture.material = finalCopyMaterial;

			// SRP mip generation:
			RenderPipelineManager.beginFrameRendering += BeginFrameRendering;
		}

        protected override void Disable()
		{
			if (!graph.isRealtime)
				CoreUtils.Destroy(tempRenderTexture);
			CoreUtils.Destroy(mipmapRenderTexture);
		}

		protected override bool ProcessNode(CommandBuffer cmd)
		{
			return true;
		}

		CommandBuffer mipchainCmd = new CommandBuffer();

		void GenerateCustomMipMaps()
		{
#if UNITY_EDITOR
			if (mipmapRenderTexture == null || tempRenderTexture == null)
				return;

			mipchainCmd.Clear();

			mipchainCmd.name = "Generate Custom MipMaps";

			if (mipMapPropertyBlock == null)
				mipMapPropertyBlock = new MaterialPropertyBlock();

			int slice = 0;
			// TODO: support 3D textures and Cubemaps
			// for (int slice = 0; slice < tempRenderTexture.volumeDepth; slice++)
			{
				for (int i = 0; i < tempRenderTexture.mipmapCount - 1; i++)
				{
					int mipLevel = i + 1;
					mipmapRenderTexture.name = "Tmp mipmap";
					mipchainCmd.SetRenderTarget(mipmapRenderTexture, mipLevel, CubemapFace.Unknown, 0);

					Vector4 textureSize = new Vector4(tempRenderTexture.width, tempRenderTexture.height, tempRenderTexture.volumeDepth, 0);
					textureSize /= 1 << (mipLevel);
					Vector4 textureSizeRcp = new Vector4(1.0f / textureSize.x, 1.0f / textureSize.y, 1.0f / textureSize.z, 0);

					mipMapPropertyBlock.SetTexture("_InputTexture_2D", tempRenderTexture);
					mipMapPropertyBlock.SetTexture("_InputTexture_3D", tempRenderTexture);
					mipMapPropertyBlock.SetFloat("_CurrentMipLevel", mipLevel - 1);
					mipMapPropertyBlock.SetFloat("_MaxMipLevel", tempRenderTexture.mipmapCount);
					mipMapPropertyBlock.SetVector("_InputTextureSize", textureSize);
					mipMapPropertyBlock.SetVector("_InputTextureSizeRcp", textureSizeRcp);
					mipMapPropertyBlock.SetFloat("_CurrentSlice", slice / (float)tempRenderTexture.width);

					MixtureUtils.SetupDimensionKeyword(customMipMapMaterial, tempRenderTexture.dimension);
					mipchainCmd.DrawProcedural(Matrix4x4.identity, customMipMapMaterial, 0, MeshTopology.Triangles, 3, 1, mipMapPropertyBlock);

					mipchainCmd.CopyTexture(mipmapRenderTexture, slice, mipLevel, tempRenderTexture, slice, mipLevel);
				}
			}

			// Dirty hack to enqueue the command buffer but it's okay because it's the builtin renderer.
			if (GraphicsSettings.renderPipelineAsset == null)
			{
				Camera.main.RemoveCommandBuffer(CameraEvent.BeforeDepthTexture, mipchainCmd);
				Camera.main.AddCommandBuffer(CameraEvent.BeforeDepthTexture, mipchainCmd);
			}
#endif
		}

		void BeginFrameRendering(ScriptableRenderContext renderContext, Camera[] cameras)
		{

		}

		[CustomPortBehavior(nameof(input))]
		protected IEnumerable< PortData > ChangeOutputPortType(List< SerializableEdge > edges)
		{
			TextureDimension dim = (GetType() == typeof(ExternalOutputNode)) ? rtSettings.GetTextureDimension(graph) : (TextureDimension)rtSettings.dimension;

			yield return new PortData{
				displayName = "input",
				displayType = TextureUtils.GetTypeFromDimension(dim),
				identifier = "input",
			};
		}

        public CustomRenderTexture GetCustomRenderTexture() => tempRenderTexture;
    }
}