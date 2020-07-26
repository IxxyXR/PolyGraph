using Conway;
using UnityEngine;
using GraphProcessor;
using UnityEngine.Rendering;


namespace Mixture
{
	[System.Serializable, NodeMenuItem("Conway")]
	public class ConwayNode : MixtureNode
	{

		public override string	name => "Conway";
		public override bool hasSettings => false;

		[Input("Polyhydra")]
		public ConwayPoly conway;

		
		public PolyHydraEnums.Ops op = PolyHydraEnums.Ops.Identity;
		[Input("Amount"), SerializeField]
		public float amount = .25f;
		[Input("Amount"), SerializeField, VisibleIf(nameof(op), PolyHydraEnums.Ops.Identity)]
		public float amount2 = .25f;
		public FaceSelections faceSelections = FaceSelections.All;
		public bool disabled;
		public bool randomize;
		public string tags;



		[Output("Polyhedra")]
		public ConwayPoly conwayOutput;

		public override bool hasPreview => false;
		public override bool showDefaultInspector => true;
		public override Texture previewTexture => UnityEditor.AssetPreview.GetAssetPreview(mesh) ?? Texture2D.blackTexture;

		private Mesh mesh;

		
		protected override bool ProcessNode(CommandBuffer cmd)
		{
			if (!disabled)
			{
				switch (op)
				{
					case PolyHydraEnums.Ops.Identity:
						break;
					case PolyHydraEnums.Ops.Kis:
						conway = conway.Kis(amount, faceSelections, tags, randomize);
						break;
					case PolyHydraEnums.Ops.Dual:
						conway = conway.Dual();
						break;
					case PolyHydraEnums.Ops.Ambo:
						conway = conway.Ambo();
						break;
					case PolyHydraEnums.Ops.Zip:
						conway = conway.Zip(amount);
						break;
					case PolyHydraEnums.Ops.Expand:
						conway = conway.Expand(amount);
						break;
					case PolyHydraEnums.Ops.Bevel:
						conway = conway.Bevel(amount, amount2);
						break;
					case PolyHydraEnums.Ops.Join:
						conway = conway.Join(amount);
						break;
					case PolyHydraEnums.Ops.Needle:
						conway = conway.Needle(amount, randomize);
						break;
					case PolyHydraEnums.Ops.Ortho:
						conway = conway.Ortho(amount, randomize);
						break;
					case PolyHydraEnums.Ops.Meta:
						conway = conway.Meta(amount, amount2, randomize);
						break;
					case PolyHydraEnums.Ops.Truncate:
						conway = conway.Truncate(amount, faceSelections, randomize);
						break;
					case PolyHydraEnums.Ops.Gyro:
						conway = conway.Gyro(amount, amount2);
						break;
					case PolyHydraEnums.Ops.Snub:
						conway = conway.Gyro(amount);
						conway = conway.Dual();
						break;
					case PolyHydraEnums.Ops.Exalt:
						// TODO return a correct VertexRole array
						// I suspect the last vertices map to the original shape verts
						conway = conway.Dual();
						conway = conway.Kis(amount, faceSelections, tags, randomize);
						conway = conway.Dual();
						conway = conway.Kis(amount, faceSelections, tags, randomize);
						break;
					case PolyHydraEnums.Ops.Yank:
						conway = conway.Kis(amount, faceSelections, tags, randomize);
						conway = conway.Dual();
						conway = conway.Kis(amount, faceSelections, tags, randomize);
						conway = conway.Dual();
						break;
					case PolyHydraEnums.Ops.Subdivide:
						conway = conway.Subdivide(amount);
						break;
					case PolyHydraEnums.Ops.Loft:
						conway = conway.Loft(amount, amount2, faceSelections, tags, randomize);
						break;
					case PolyHydraEnums.Ops.Chamfer:
						conway = conway.Chamfer(amount);
						break;
					case PolyHydraEnums.Ops.Quinto:
						conway = conway.Quinto(amount, amount2, randomize);
						break;
					case PolyHydraEnums.Ops.JoinedLace:
						conway = conway.JoinedLace(amount, amount2, randomize);
						break;
					case PolyHydraEnums.Ops.OppositeLace:
						conway = conway.OppositeLace(amount, amount2, randomize);
						break;
					case PolyHydraEnums.Ops.Lace:
						conway = conway.Lace(amount, faceSelections, tags, amount2, randomize);
						break;
					case PolyHydraEnums.Ops.JoinKisKis:
						conway = conway.JoinKisKis(amount, amount2);
						break;
					case PolyHydraEnums.Ops.Stake:
						conway = conway.Stake(amount, faceSelections, tags);
						break;
					case PolyHydraEnums.Ops.JoinStake:
						conway = conway.Stake(amount, faceSelections, tags, true);
						break;
					case PolyHydraEnums.Ops.Medial:
						conway = conway.Medial((int)amount, amount2);
						break;
					case PolyHydraEnums.Ops.EdgeMedial:
						conway = conway.EdgeMedial((int)amount, amount2);
						break;
					// case Ops.JoinedMedial:
					// 	conway = conway.JoinedMedial((int)amount, amount2);
					// 	break;
					case PolyHydraEnums.Ops.Propeller:
						conway = conway.Propeller(amount);
						break;
					case PolyHydraEnums.Ops.Whirl:
						conway = conway.Whirl(amount);
						break;
					case PolyHydraEnums.Ops.Volute:
						conway = conway.Volute(amount);
						break;
					case PolyHydraEnums.Ops.Cross:
						conway = conway.Cross(amount);
						break;
					case PolyHydraEnums.Ops.Squall:
						conway = conway.Squall(amount, false);
						break;
					case PolyHydraEnums.Ops.JoinSquall:
						conway = conway.Squall(amount, true);
						break;
					case PolyHydraEnums.Ops.Shell:
						// TODO do this properly with shared edges/vertices
						conway = conway.Extrude(amount, false, randomize);
						break;
					case PolyHydraEnums.Ops.Skeleton:
						conway = conway.FaceRemove(faceSelections, tags);
						if ((faceSelections==FaceSelections.New || faceSelections==FaceSelections.NewAlt) && op == PolyHydraEnums.Ops.Skeleton)
						{
							// Nasty hack until I fix extrude
							// Produces better results specific for PolyMidi
							conway = conway.FaceScale(0f, FaceSelections.All);
						}
						conway = conway.Extrude(amount, false, randomize);
						break;
					case PolyHydraEnums.Ops.Extrude:
						conway = conway.Loft(0, amount, faceSelections, tags, randomize);
						break;
					case PolyHydraEnums.Ops.VertexScale:
						conway = conway.VertexScale(amount, faceSelections, randomize);
						break;
					case PolyHydraEnums.Ops.FaceSlide:
						conway = conway.FaceSlide(amount, amount2, faceSelections, tags, randomize);
						break;
					case PolyHydraEnums.Ops.FaceMerge:
						conway = conway.FaceMerge(faceSelections);
						break;
					case PolyHydraEnums.Ops.VertexRotate:
						conway = conway.VertexRotate(amount, faceSelections, tags, randomize);
						break;
					case PolyHydraEnums.Ops.VertexFlex:
						conway = conway.VertexFlex(amount, faceSelections, tags, randomize);
						break;
					case PolyHydraEnums.Ops.FaceOffset:
						// TODO Faceroles ignored. Vertex Roles
						// Split faces
						var origRoles = conway.FaceRoles;
						conway = conway.FaceScale(0, FaceSelections.All, tags, false);
						conway.FaceRoles = origRoles;
						conway = conway.Offset(amount, faceSelections, tags, randomize);
						break;
					case PolyHydraEnums.Ops.FaceScale:
						conway = conway.FaceScale(amount, faceSelections, tags, randomize);
						break;
					case PolyHydraEnums.Ops.FaceRotate:
						conway = conway.FaceRotate(amount, faceSelections, tags, 0, randomize);
						break;
		//					case Ops.Ribbon:
		//						conway = conway.Ribbon(amount, false, 0.1f);
		//						break;
		//					case Ops.FaceTranslate:
		//						conway = conway.FaceTranslate(amount, faceSelections);
		//						break;
		//					case Ops.FaceRotateX:
		//						conway = conway.FaceRotate(amount, faceSelections, 1);
		//						break;
		//					case Ops.FaceRotateY:
		//						conway = conway.FaceRotate(amount, faceSelections, 2);
		//						break;
					case PolyHydraEnums.Ops.FaceRemove:
						conway = conway.FaceRemove(faceSelections, tags);
						break;
					case PolyHydraEnums.Ops.FaceKeep:
						conway = conway.FaceKeep(faceSelections, tags);
						break;
					case PolyHydraEnums.Ops.VertexRemove:
						conway = conway.VertexRemove(faceSelections, false);
						break;
					case PolyHydraEnums.Ops.VertexKeep:
						conway = conway.VertexRemove(faceSelections, true);
						break;
					case PolyHydraEnums.Ops.FillHoles:
						conway.FillHoles();
						break;
					case PolyHydraEnums.Ops.Hinge:
						conway = conway.Hinge(amount);
						break;
					case PolyHydraEnums.Ops.AddDual:
						conway = conway.AddDual(amount);
						break;
					case PolyHydraEnums.Ops.AddCopyX:
						conway = conway.AddCopy(Vector3.right, amount, faceSelections, tags);
						break;
					case PolyHydraEnums.Ops.AddCopyY:
						conway = conway.AddCopy(Vector3.up, amount, faceSelections, tags);
						break;
					case PolyHydraEnums.Ops.AddCopyZ:
						conway = conway.AddCopy(Vector3.forward, amount, faceSelections, tags);
						break;
					case PolyHydraEnums.Ops.AddMirrorX:
						conway = conway.AddMirrored(Vector3.right, amount, faceSelections, tags);
						break;
					case PolyHydraEnums.Ops.AddMirrorY:
						conway = conway.AddMirrored(Vector3.up, amount, faceSelections, tags);
						break;
					case PolyHydraEnums.Ops.AddMirrorZ:
						conway = conway.AddMirrored(Vector3.forward, amount, faceSelections, tags);
						break;
					// case PolyHydraEnums.Ops.Stash:
					// 	stash = conway.Duplicate();
					// 	stash = stash.FaceKeep(faceSelections);
					// 	break;
					// case PolyHydraEnums.Ops.Unstash:
					// 	if (stash == null) return conway;
					// 	var dup = conway.Duplicate();
					// 	var offset = Vector3.up * amount2;
					// 	dup.Append(stash.FaceKeep(faceSelections, tags), offset, Quaternion.identity, amount);
					// 	conway = dup;
					// 	break;
					// case PolyHydraEnums.Ops.UnstashToFaces:
					// 	if (stash == null) return conway;
					// 	conway = conway.AppendMany(stash, faceSelections, tags, amount, 0, amount2, true);
					// 	break;
					// case PolyHydraEnums.Ops.UnstashToVerts:
					// 	if (stash == null) return conway;
					// 	conway = conway.AppendMany(stash, faceSelections, tags, amount, 0, amount2, false);
					// 	break;
					case PolyHydraEnums.Ops.TagFaces:
						conway.TagFaces(tags, faceSelections);
						break;
					case PolyHydraEnums.Ops.Layer:
						conway = conway.Layer(4, 1f - amount, amount / 10f, faceSelections, tags);
						break;
					case PolyHydraEnums.Ops.Canonicalize:
						conway = conway.Canonicalize(0.1f, 0.1f);
						break;
					case PolyHydraEnums.Ops.Spherize:
						conway = conway.Spherize(amount, faceSelections);
						break;
					case PolyHydraEnums.Ops.Recenter:
						conway.Recenter();
						break;
					case PolyHydraEnums.Ops.SitLevel:
						conway = conway.SitLevel(amount);
						break;
					case PolyHydraEnums.Ops.Stretch:
						conway = conway.Stretch(amount);
						break;
					case PolyHydraEnums.Ops.Slice:
						conway = conway.Slice(amount, amount2);
						break;
					case PolyHydraEnums.Ops.Stack:
						conway = conway.Stack(Vector3.up, amount, amount2, 0.1f, faceSelections, tags);
						conway.Recenter();
						break;
					case PolyHydraEnums.Ops.Weld:
						conway = conway.Weld(amount);
						break;
				}

			}

			conwayOutput = conway;

			return true;

		}
    }
}