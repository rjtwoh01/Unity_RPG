using System;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;

[RequireComponent(typeof (ThirdPersonCharacter))]
public class PlayerMovement : MonoBehaviour
{
    ThirdPersonCharacter m_Character;   // A reference to the ThirdPersonCharacter on the object
    CameraRaycaster cameraRaycaster;
    Vector3 currentClickTarget;
        
    private void Start()
    {
        cameraRaycaster = Camera.main.GetComponent<CameraRaycaster>();
        m_Character = GetComponent<ThirdPersonCharacter>();
        currentClickTarget = transform.position;
    }

    // Fixed update is called in sync with physics
    private void FixedUpdate()
    {
        if (cameraRaycaster.layerHit == Layer.Walkable)
        {
            if (Input.GetMouseButton(0))
            {
                currentClickTarget = cameraRaycaster.hit.point;  // So not set in default case
            }
            m_Character.Move(currentClickTarget - transform.position, false, false);
            if (m_Character.transform.position == (currentClickTarget - transform.position))
            {
                m_Character.Move(new Vector3(0,0,0), false, false); // set move to 0 ie stop moving
            }
        }
    }
}

