using System;
using System.Collections.Generic;
using Conway;
using UnityEngine;
using GraphProcessor;
using UnityEngine.Rendering;


namespace Mixture
{
	[System.Serializable, NodeMenuItem("Polyhydra/Conway")]
	public class ConwayNode : MixtureNode
	{

		public override string	name => "Conway";
		public override bool hasSettings => false;

		[Input("Polyhydra")]
		public ConwayPoly poly;

		
		public PolyHydraEnums.Ops op = PolyHydraEnums.Ops.Identity;
		[SerializeField] public float amountVal = .25f;
		[SerializeField] public float amount2Val = .25f;

		// [Input("Amount")]
		// public Func<FilterParams, float> amountFunc;
		// [Input("Amount2")]
		// public Func<FilterParams, float> amount2Func;

		[Input("Amount")]
		public List<float> amountList;
		[Input("Amount2")]
		public List<float> amount2List;

		[Input("Face Filter")]
		public Func<FilterParams, bool> faceFilter;

		// public FaceSelections faceSelections = FaceSelections.All;
		public bool disabled;
		public bool randomize;
		public string tags;

		[Output("Polyhedra Result")]
		public ConwayPoly polyResult;

		public override bool hasPreview => false;
		public override bool showDefaultInspector => true;
		public override Texture previewTexture => UnityEditor.AssetPreview.GetAssetPreview(mesh) ?? Texture2D.blackTexture;

		private Mesh mesh;

		
		protected override bool ProcessNode(CommandBuffer cmd)
		{

			if (poly == null) return false;

			if (!disabled)
			{
				// Construct lambdas from lists or constants
				var amountFunc = amountList == null || amountList.Count == 0
					? (Func<FilterParams, float>) (x => amountVal)
					: x => amountList[x.index];
				var amount2Func = amount2List == null || amount2List.Count == 0
					? (Func<FilterParams, float>) (x => amount2Val)
					: x => amount2List[x.index];

				switch (op)
				{
					case PolyHydraEnums.Ops.Identity:
						break;
					case PolyHydraEnums.Ops.Kis:
						poly = poly.Kis(new OpParams{funcA = amountFunc, tags=tags, randomize = randomize, filterFunc=faceFilter});
						break;
					case PolyHydraEnums.Ops.Dual:
						poly = poly.Dual();
						break;
					case PolyHydraEnums.Ops.Ambo:
						poly = poly.Ambo();
						break;
					case PolyHydraEnums.Ops.Zip:
						poly = poly.Zip(new OpParams{funcA = amountFunc});
						break;
					case PolyHydraEnums.Ops.Expand:
						poly = poly.Expand(new OpParams{funcA = amountFunc});
						break;
					case PolyHydraEnums.Ops.Bevel:
						poly = poly.Bevel(new OpParams{funcA = amountFunc, funcB = amount2Func});
						break;
					case PolyHydraEnums.Ops.Join:
						poly = poly.Join(new OpParams{funcA = amountFunc});
						break;
					case PolyHydraEnums.Ops.Needle:
						poly = poly.Needle(new OpParams{randomize = randomize, funcA = amountFunc});
						break;
					case PolyHydraEnums.Ops.Ortho:
						poly = poly.Ortho(new OpParams{randomize = randomize, funcA = amountFunc});
						break;
					case PolyHydraEnums.Ops.Meta:
						poly = poly.Meta(new OpParams{randomize = randomize, funcA = amountFunc, funcB = amount2Func});
						break;
					case PolyHydraEnums.Ops.Truncate:
						poly = poly.Truncate(new OpParams{randomize = randomize, funcA = amountFunc, filterFunc = faceFilter});
						break;
					case PolyHydraEnums.Ops.Gyro:
						poly = poly.Gyro(new OpParams{funcA = amountFunc, funcB = amount2Func});
						break;
					case PolyHydraEnums.Ops.Snub:
						poly = poly.Gyro(new OpParams{funcA = amountFunc});
						poly = poly.Dual();
						break;
					case PolyHydraEnums.Ops.Exalt:
						// TODO return a correct VertexRole array
						// I suspect the last vertices map to the original shape verts
						poly = poly.Dual();
						poly = poly.Kis(new OpParams{tags = tags, randomize = randomize, funcA = amountFunc, filterFunc = faceFilter});
						poly = poly.Dual();
						poly = poly.Kis(new OpParams{tags = tags, randomize = randomize, filterFunc = faceFilter});
						break;
					case PolyHydraEnums.Ops.Yank:
						poly = poly.Kis(new OpParams{tags = tags, randomize = randomize, funcA = amountFunc, filterFunc = faceFilter});
						poly = poly.Dual();
						poly = poly.Kis(new OpParams{tags = tags, randomize = randomize, funcA = amountFunc, filterFunc = faceFilter});
						poly = poly.Dual();
						break;
					case PolyHydraEnums.Ops.Subdivide:
						poly = poly.Subdivide(new OpParams{funcA = amountFunc});
						break;
					case PolyHydraEnums.Ops.Loft:
						poly = poly.Loft(new OpParams{tags=tags, randomize = randomize, funcA = amountFunc, funcB = amount2Func, filterFunc = faceFilter});
						break;
					case PolyHydraEnums.Ops.Chamfer:
						poly = poly.Chamfer(new OpParams{funcA = amountFunc});
						break;
					case PolyHydraEnums.Ops.Quinto:
						poly = poly.Quinto(new OpParams{funcA = amountFunc, funcB = amount2Func, randomize = randomize});
						break;
					case PolyHydraEnums.Ops.JoinedLace:
						poly = poly.JoinedLace(new OpParams{randomize = randomize, funcA = amountFunc, funcB = amount2Func});
						break;
					case PolyHydraEnums.Ops.OppositeLace:
						poly = poly.OppositeLace(new OpParams{funcA = amountFunc, funcB = amount2Func, randomize = randomize});
						break;
					case PolyHydraEnums.Ops.Lace:
						poly = poly.Lace(new OpParams{tags = tags, randomize = randomize, funcA = amountFunc, funcB = amount2Func, filterFunc = faceFilter});
						break;
					case PolyHydraEnums.Ops.JoinKisKis:
						poly = poly.JoinKisKis(new OpParams{funcA = amountFunc, funcB = amount2Func});
						break;
					case PolyHydraEnums.Ops.Stake:
						poly = poly.Stake(new OpParams{tags = tags, funcA = amountFunc, filterFunc = faceFilter});
						break;
					case PolyHydraEnums.Ops.JoinStake:
						poly = poly.Stake(new OpParams{tags = tags, funcA = amountFunc}, true);
						break;
					case PolyHydraEnums.Ops.Medial:
						poly = poly.Medial((int)amountVal, amount2Val);
						break;
					case PolyHydraEnums.Ops.EdgeMedial:
						poly = poly.EdgeMedial((int)amountVal, amount2Val);
						break;
					// case Ops.JoinedMedial:
					// 	conway = conway.JoinedMedial((int)0, amount2);
					// 	break;
					case PolyHydraEnums.Ops.Propeller:
						poly = poly.Propeller(amountVal);
						break;
					case PolyHydraEnums.Ops.Whirl:
						poly = poly.Whirl(amountVal);
						break;
					case PolyHydraEnums.Ops.Volute:
						poly = poly.Volute(amountVal);
						break;
					case PolyHydraEnums.Ops.Cross:
						poly = poly.Cross(new OpParams{funcA = amountFunc});
						break;
					case PolyHydraEnums.Ops.Squall:
						poly = poly.Squall(new OpParams{funcA = amountFunc}, false);
						break;
					case PolyHydraEnums.Ops.JoinSquall:
						poly = poly.Squall(new OpParams{funcA = amountFunc}, true);
						break;
					case PolyHydraEnums.Ops.Shell:
						// TODO do this properly with shared edges/vertices
						poly = poly.Shell(amountVal, false, randomize);
						break;
					case PolyHydraEnums.Ops.Skeleton:
						// poly = poly.FaceRemove(new OpParams{tags = tags});
						// if ((faceSelections==FaceSelections.New || faceSelections==FaceSelections.NewAlt) && op == PolyHydraEnums.Ops.Skeleton)
						// {
						// 	// Nasty hack until I fix extrude
						// 	// Produces better results specific for PolyMidi
						// 	poly = poly.FaceScale(new OpParams{valueA = 0f, facesel = FaceSelections.All});
						// }
						// poly = poly.Shell(amountVal, false, randomize);
						break;
					case PolyHydraEnums.Ops.Extrude:
						poly = poly.Loft(new OpParams{tags = tags, randomize = randomize, funcA = amountFunc, filterFunc = faceFilter});
						break;
					case PolyHydraEnums.Ops.VertexScale:
						poly = poly.VertexScale(new OpParams{randomize = randomize, funcA = amountFunc, filterFunc = faceFilter});
						break;
					case PolyHydraEnums.Ops.FaceSlide:
						////poly = poly.FaceSlide(amountVal, amount2Val, faceSelections, tags, randomize);
						break;
					case PolyHydraEnums.Ops.FaceMerge:
						////poly = poly.FaceMerge(faceSelections);
						break;
					case PolyHydraEnums.Ops.VertexRotate:
						poly = poly.VertexRotate(new OpParams{tags = tags, randomize = randomize, funcA = amountFunc, filterFunc = faceFilter});
						break;
					case PolyHydraEnums.Ops.VertexFlex:
						poly = poly.VertexFlex(new OpParams{tags = tags, randomize = randomize, funcA = amountFunc, filterFunc = faceFilter});
						break;
					case PolyHydraEnums.Ops.FaceOffset:
						// TODO Faceroles ignored. Vertex Roles
						// Split faces
						var origRoles = poly.FaceRoles;
						// Split faces
						poly = poly.FaceScale(new OpParams{tags = tags});
						poly.FaceRoles = origRoles;
						////poly = poly.Offset(amountVal, faceSelections, tags, randomize);
						break;
					case PolyHydraEnums.Ops.FaceScale:
						poly = poly.FaceScale(new OpParams{tags = tags, randomize = randomize, funcA = amountFunc, filterFunc = faceFilter});
						break;
					case PolyHydraEnums.Ops.FaceRotate:
						poly = poly.FaceRotate(new OpParams{tags = tags, randomize = randomize, funcA = amountFunc, filterFunc = faceFilter}, 0);
						break;
		//					case Ops.Ribbon:
		//						conway = conway.Ribbon(new OpParams{false, 0.1f);
		//						break;
		//					case Ops.FaceTranslate:
		//						conway = conway.FaceTranslate(new OpParams{filterFunc = faceFilter});
		//						break;
		//					case Ops.FaceRotateX:
		//						conway = conway.FaceRotate(new OpParams{1, filterFunc = faceFilter});
		//						break;
		//					case Ops.FaceRotateY:
		//						conway = conway.FaceRotate(new OpParams{2, filterFunc = faceFilter});
		//						break;
					case PolyHydraEnums.Ops.FaceRemove:
						poly = poly.FaceRemove(new OpParams{tags = tags, filterFunc = faceFilter});
						break;
					case PolyHydraEnums.Ops.FaceKeep:
						poly = poly.FaceKeep(new OpParams{tags = tags, filterFunc = faceFilter});
						break;
					case PolyHydraEnums.Ops.VertexRemove:
						poly = poly.VertexRemove(new OpParams{tags = tags, filterFunc = faceFilter}, false);
						break;
					case PolyHydraEnums.Ops.VertexKeep:
						poly = poly.VertexRemove(new OpParams{tags = tags, filterFunc = faceFilter}, true);
						break;
					case PolyHydraEnums.Ops.FillHoles:
						poly.FillHoles();
						break;
					case PolyHydraEnums.Ops.Hinge:
						poly = poly.Hinge(amountVal);
						break;
					case PolyHydraEnums.Ops.AddDual:
						poly = poly.AddDual(amountVal);
						break;
					case PolyHydraEnums.Ops.AddCopyX:
						////poly = poly.AddCopy(Vector3.right, amountVal, faceSelections, tags);
						break;
					case PolyHydraEnums.Ops.AddCopyY:
						////poly = poly.AddCopy(Vector3.up, amountVal, faceSelections, tags);
						break;
					case PolyHydraEnums.Ops.AddCopyZ:
						////poly = poly.AddCopy(Vector3.forward, amountVal, faceSelections, tags);
						break;
					case PolyHydraEnums.Ops.AddMirrorX:
						////poly = poly.AddMirrored(Vector3.right, amountVal, faceSelections, tags);
						break;
					case PolyHydraEnums.Ops.AddMirrorY:
						////poly = poly.AddMirrored(Vector3.up, amountVal, faceSelections, tags);
						break;
					case PolyHydraEnums.Ops.AddMirrorZ:
						////poly = poly.AddMirrored(Vector3.forward, amountVal, faceSelections, tags);
						break;
					// case PolyHydraEnums.Ops.Stash:
					// 	stash = conway.Duplicate();
					// 	stash = stash.FaceKeep(faceSelections, filterFunc = faceFilter});
					// 	break;
					// case PolyHydraEnums.Ops.Unstash:
					// 	if (stash == null) return conway;
					// 	var dup = conway.Duplicate();
					// 	var offset = Vector3.up * amount2;
					// 	dup.Append(stash.FaceKeep(faceSelections, tags), offset, Quaternion.identity, 0, filterFunc = faceFilter});
					// 	conway = dup;
					// 	break;
					// case PolyHydraEnums.Ops.UnstashToFaces:
					// 	if (stash == null) return conway;
					// 	conway = conway.AppendMany(stash, faceSelections, tags, 0, 0, true, filterFunc = faceFilter});
					// 	break;
					// case PolyHydraEnums.Ops.UnstashToVerts:
					// 	if (stash == null) return conway;
					// 	conway = conway.AppendMany(stash, faceSelections, tags, 0, 0, false, filterFunc = faceFilter});
					// 	break;
					case PolyHydraEnums.Ops.TagFaces:
						////poly.TagFaces(tags, faceSelections);
						break;
					case PolyHydraEnums.Ops.Layer:
						////poly = poly.Layer(4, 1f - amountVal, amountVal / 10f, faceSelections, tags);
						break;
					case PolyHydraEnums.Ops.Canonicalize:
						poly = poly.Canonicalize(0.1f, 0.1f);
						break;
					case PolyHydraEnums.Ops.Spherize:
						////poly = poly.Spherize(amountVal, faceSelections);
						break;
					case PolyHydraEnums.Ops.Recenter:
						poly.Recenter();
						break;
					case PolyHydraEnums.Ops.SitLevel:
						poly = poly.SitLevel(amountVal);
						break;
					case PolyHydraEnums.Ops.Stretch:
						poly = poly.Stretch(amountVal);
						break;
					case PolyHydraEnums.Ops.Slice:
						poly = poly.Slice(amountVal, amount2Val);
						break;
					case PolyHydraEnums.Ops.Stack:
						////poly = poly.Stack(Vector3.up, amountVal, amount2Val, 0.1f, faceSelections, tags);
						poly.Recenter();
						break;
					case PolyHydraEnums.Ops.Weld:
						poly = poly.Weld(amountVal);
						break;
				}

			}

			polyResult = poly;

			return true;

		}
    }
}