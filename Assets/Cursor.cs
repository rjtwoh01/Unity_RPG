using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cursor : MonoBehaviour {

    CameraRaycaster cameraRayCaster;

	// Use this for initialization
	void Start () {
        cameraRayCaster = GetComponent<CameraRaycaster>();
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetMouseButtonDown(0)) //0 is left click, 1 is right, 2 is middle
        {
            print(cameraRayCaster.layerHit);
        }
	}
}
