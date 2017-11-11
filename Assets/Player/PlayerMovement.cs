using System;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;
using UnityStandardAssets.CrossPlatformInput;

[RequireComponent(typeof (ThirdPersonCharacter))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float walkMoveStopRadius = 0.2f;
    ThirdPersonCharacter m_Character;   // A reference to the ThirdPersonCharacter on the object
    CameraRaycaster cameraRaycaster;
    Vector3 currentClickTarget;

    //TODO: Consider removing the ability for WSAD altogether
    //TODO: Make controller only in case of toggle if remove WSADb 
    bool isInDirectMode = false; //TODO: Consider making static later
    [SerializeField]bool jump = false;
        
    private void Start()
    {
        cameraRaycaster = Camera.main.GetComponent<CameraRaycaster>();
        m_Character = GetComponent<ThirdPersonCharacter>();
        currentClickTarget = transform.position;
    }

    private void Update()
    {
        if (!jump)
            jump = CrossPlatformInputManager.GetButtonDown("Jump");
    }

    // Fixed update is called in sync with physics
    private void FixedUpdate()
    {
        //TODO: Allow player to map later or add to menu
        if (Input.GetKeyDown(KeyCode.G)) //G for gamepad.
        {
            isInDirectMode = !isInDirectMode; //toggle mode
        }
        if (isInDirectMode) { ProcessDirectMovement(); }
        else {
            ProcessMouseMovement();
        }
    }

    private void ProcessMouseMovement()
    {
        if (Input.GetMouseButton(0))
        {
            switch (cameraRaycaster.layerHit)
            {
                case Layer.Walkable:
                    currentClickTarget = cameraRaycaster.hit.point;
                    break;
                case Layer.Enemy:
                    print("I cannot move on to Enemy");
                    break;
                default:
                    print("Unexpected layer found");
                    return;
            }
        }
        var playerToClickPoint = currentClickTarget - transform.position;
        if (playerToClickPoint.magnitude >= walkMoveStopRadius)
        {
            m_Character.Move(playerToClickPoint, false, false);
        }
        else
        {
            m_Character.Move(Vector3.zero, false, jump);
        }
        jump = false;
    }

    /// <summary>
    /// Direct movement is something where the player has direct control of their character
    /// While they're holding a key or pushing a joystick, the character is moving in that direction
    /// Its not like a mouse where you click and they go there, you're controller them as they go there
    /// </summary>
    private void ProcessDirectMovement()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        // calculate camera relative direction to move:
        Vector3 m_CamForward = Vector3.Scale(Camera.main.transform.forward, new Vector3(1, 0, 1)).normalized;
        Vector3 m_Move = v * m_CamForward + h * Camera.main.transform.right;

        m_Character.Move(m_Move, false, jump);
        jump = false;
    }
}

