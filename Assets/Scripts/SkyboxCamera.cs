using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyboxCamera : MonoBehaviour {
 
   
    // set the main camera in the inspector
    public Camera MainCamera;
   
    // set the sky box camera in the inspector
    public Camera SkyCamera;
   
    // the additional rotation to add to the skybox
    // can be set during game play or in the inspector
    public Vector3 SkyBoxRotation;
    Quaternion mainCameraRotationPrev;
    Quaternion mainCameraRotationDisplacement;
 
    // Use this for initialization
    void Start()
    {
        if (SkyCamera.depth >= MainCamera.depth)
        {
            Debug.Log("Set skybox camera depth lower "+
                " than main camera depth in inspector");
        }
        if (MainCamera.clearFlags != CameraClearFlags.Nothing)
        {
            Debug.Log("Main camera needs to be set to dont clear" +
                "in the inspector");
        }
        mainCameraRotationPrev = MainCamera.transform.rotation;
    }
   
    // if you need to rotate the skybox during gameplay
    // rotate the skybox independently of the main camera
    public void SetSkyBoxRotation(Vector3 rotation)
    {
        this.SkyBoxRotation = rotation;
    }
 
    // Update is called once per frame
    void Update()
    {
       
        SkyCamera.transform.position = MainCamera.transform.position;
        mainCameraRotationDisplacement = mainCameraRotationPrev * Quaternion.Inverse(MainCamera.transform.rotation);
        mainCameraRotationPrev = MainCamera.transform.rotation;
        SkyCamera.transform.Rotate(mainCameraRotationDisplacement.eulerAngles);
        SkyCamera.transform.Rotate(SkyBoxRotation);
    }
}
