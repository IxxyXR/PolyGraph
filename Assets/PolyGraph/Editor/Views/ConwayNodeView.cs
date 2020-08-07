using UnityEngine;
using GraphProcessor;

namespace Mixture
{
    [NodeCustomEditor(typeof(ConwayNode))]
    public class ConwayNodeView : MixtureNodeView
    {
        public override void Enable()
        {
            // TODO - conditional display of fields
            base.Enable();
        }
    }
}