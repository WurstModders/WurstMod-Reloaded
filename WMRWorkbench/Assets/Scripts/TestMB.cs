#if H3VR_IMPORTED
using UnityEngine;

public class TestMB : MonoBehaviour 
{
    void Start()
    {
        Debug.Log("Hello World!");
        if (GetComponent<FistVR.FVRPointable>() != null)
        {
            Debug.Log("I exist, and so does FVRPointable!");
        }
        else
        {
            Debug.Log("I exist, but I can't find FVRPointable :(");
        }
    }
}
#endif
