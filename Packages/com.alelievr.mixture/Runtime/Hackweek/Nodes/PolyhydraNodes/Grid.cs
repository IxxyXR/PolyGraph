using Conway;
using UnityEngine;
using GraphProcessor;
using UnityEngine.Rendering;


namespace Mixture
{
    [System.Serializable, NodeMenuItem("Grid")]
    public class GridNode : MixtureNode
    {
        [Output("Mesh")]
        public MixtureMesh meshOutput;
        [Output("Polyhedra")]
        public ConwayPoly conwayOutput;

        public override string	name => "Grid";
        public override bool hasSettings => false;

        public override bool hasPreview => true;
        public override bool showDefaultInspector => true;
        public override Texture previewTexture => UnityEditor.AssetPreview.GetAssetPreview(mesh) ?? Texture2D.blackTexture;

        private Mesh mesh;

        public int Rows = 2;
        public int Columns = 2;
        public float RowScale = 1f;
        public float ColumnScale = 1f;

        protected override bool ProcessNode(CommandBuffer cmd)
        {
            var conway = ConwayPoly.MakeGrid(Rows, Columns, RowScale, ColumnScale);

            var mesh = PolyMeshBuilder.BuildMeshFromConwayPoly(conway, false);

            meshOutput = new MixtureMesh {mesh = mesh, localToWorld = Matrix4x4.TRS(Vector3.zero, Quaternion.identity, Vector3.one)};
            conwayOutput = conway;

            return true;

        }
    }
}