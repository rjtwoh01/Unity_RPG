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

    RaycastHit m_hit;
    public RaycastHit hit
    {
        get { return m_hit; }
    }

    Layer m_layerHit;
    public Layer layerHit
    {
        get { return m_layerHit; }
    }

    void Start() // TODO Awake?
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
                m_hit = hit.Value;
                m_layerHit = layer;
                return;
            }
        }

        // Otherwise return background hit
        m_hit.distance = distanceToBackground;
        m_layerHit = Layer.RaycastEndStop;
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
