using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof(CameraRaycaster))]
public class CursorAffordance : MonoBehaviour {

    [SerializeField] Texture2D walkCursor = null;
    [SerializeField] Texture2D attackCursor = null;
    [SerializeField] Texture2D questionCursor = null;
    [SerializeField] Vector2 cursorHotspot = new Vector2(0, 0);//deminsion of texture
    CameraRaycaster cameraRayCaster;

	// Use this for initialization
	void Start () {
        cameraRayCaster = GetComponent<CameraRaycaster>();
        cameraRayCaster.layerChangeObservers += OnLayerChange;
	}
	
	void OnLayerChange(Layer layer) {
        print(layer.ToString());
        if (layer == Layer.Walkable)
            Cursor.SetCursor(walkCursor, cursorHotspot, CursorMode.Auto);
        else if (layer == Layer.Enemy)
            Cursor.SetCursor(attackCursor, cursorHotspot, CursorMode.Auto);
        else //the "unknown" out of map
            Cursor.SetCursor(questionCursor, cursorHotspot, CursorMode.Auto);
    }
}