using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorAffordance : MonoBehaviour {

    [SerializeField] Texture2D walkCursor = null;
    [SerializeField] Texture2D attackCursor = null;
    [SerializeField] Texture2D questionCursor = null;
    [SerializeField] Vector2 cursorHotspot = new Vector2(96, 96);//deminsion of texture
    CameraRaycaster cameraRayCaster;

	// Use this for initialization
	void Start () {
        cameraRayCaster = GetComponent<CameraRaycaster>();
	}
	
	// Update is called once per frame
	void Update () {
        if (cameraRayCaster.layerHit == Layer.Walkable)
            Cursor.SetCursor(walkCursor, cursorHotspot, CursorMode.Auto);
        else if (cameraRayCaster.layerHit == Layer.Enemy)
            Cursor.SetCursor(attackCursor, cursorHotspot, CursorMode.Auto);
        else //the "unknown" out of map
            Cursor.SetCursor(questionCursor, cursorHotspot, CursorMode.Auto);
    }
}
