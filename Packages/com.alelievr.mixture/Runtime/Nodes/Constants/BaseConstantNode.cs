namespace Mixture
{
	public abstract class BaseConstantNode : MixtureNode
	{
		public override bool hasPreview => false;
		public override bool hasSettings => false;
		public override float nodeWidth => 200;
	}
}