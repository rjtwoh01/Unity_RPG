using UnityEngine;

public class CameraRaycaster : MonoBehaviour
{
    public Layer[] layerPriorities = {
        Layer.Enemy,
        Layer.Walkable
    };

    //SerializeField allows the Unity inspector to set this value
    //But it keeps the variable private to prevent other classes from altering it
    [SerializeField]float distanceToBackground = 100f;
    Camera viewCamera;

    RaycastHit rayCastHit;
    public RaycastHit hit
    {
        get { return rayCastHit; }
    }

    Layer layerHit;
    public Layer currentLayerHit
    {
        get { return layerHit; }
    }

    public delegate void OnLayerChange(Layer newLayer);
    public event OnLayerChange layerChangeObservers; //insantiate a observer set
    

    void Start()
    {
        viewCamera = Camera.main;
    }

    void Update()
    {
        // Look for and return priority layer hit
        foreach (Layer layer in layerPriorities)
        {
            var hit = RaycastForLayer(layer);
            if (hit.HasValue)
            {
                rayCastHit = hit.Value;
                if (layerHit != layer)
                {
                    layerHit = layer;
                    layerChangeObservers(layer);
                }
                return;
            }
        }

        // Otherwise return background hit
        rayCastHit.distance = distanceToBackground;
        if (layerHit != Layer.RaycastEndStop)
        {
            layerHit = Layer.RaycastEndStop;
            layerChangeObservers(layerHit);
        }        
    }

    RaycastHit? RaycastForLayer(Layer layer)
    {
        // 1 << variable means bit shift by 1
        //so we're bit shifting the layer
        //from 00001 to 00010 for example where 00001 is layer 1 and 00010 is layer 2
        int layerMask = 1 << (int)layer; // See Unity docs for mask formation
        Ray ray = viewCamera.ScreenPointToRay(Input.mousePosition);

        RaycastHit hit; // used as an out parameter
        bool hasHit = Physics.Raycast(ray, out hit, distanceToBackground, layerMask);
        if (hasHit)
        {
            return hit;
        }
        return null;
    }
}
