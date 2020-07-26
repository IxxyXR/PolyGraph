using Conway;
using UnityEngine;
using GraphProcessor;
using UnityEngine.Rendering;


namespace Mixture
{
    [System.Serializable, NodeMenuItem("Grid")]
    public class GridNode : MixtureNode
    {
        [Output("Polyhedra")]
        public ConwayPoly poly;

        [SerializeField]
        public PolyHydraEnums.GridTypes GridType;

        [SerializeField]
        public PolyHydraEnums.GridShapes GridShape;

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

            switch (GridType)
            {
                case PolyHydraEnums.GridTypes.Square:
                    poly = Grids.Grids.MakeGrid(Rows, Columns, RowScale, ColumnScale);
                    break;
                case PolyHydraEnums.GridTypes.Isometric:
                    poly = Grids.Grids.MakeUnitileGrid(Rows, Columns);
                    break;
                default:
                    poly = Grids.Grids.MakeUnitileGrid(Rows, Columns);
                    break;
            }
            return true;

        }
    }
}