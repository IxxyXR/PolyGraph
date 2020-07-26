using UnityEngine;
using UnityEngine.Rendering;

[ExecuteAlways]
public class FocusDOF : MonoBehaviour
{
    Volume vol;
    //DepthOfField dof;

    public GameObject focus;
    public GameObject cam;

    void OnEnable()
    {
        vol = GetComponent<Volume>();
    }

    // Update is called once per frame
    void Update()
    {
        //vol.sharedProfile.TryGet<DepthOfField>(out var dof);
        //dof.focusDistance.value = Vector3.Distance(cam.transform.position, focus.transform.position);
    }
}
