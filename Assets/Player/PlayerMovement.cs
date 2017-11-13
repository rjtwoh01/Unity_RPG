using System;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;
using UnityStandardAssets.CrossPlatformInput;

[RequireComponent(typeof (ThirdPersonCharacter))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float walkMoveStopRadius = 0.2f;
    [SerializeField] float attackStopMoveRadius = 1f;
    ThirdPersonCharacter thirdPersonCharacter;   // A reference to the ThirdPersonCharacter on the object
    CameraRaycaster cameraRaycaster;
    Vector3 currentDestination;
    Vector3 clickPoint;

    //TODO: Consider removing the ability for WSAD altogether
    //TODO: Make controller only in case of toggle if remove WSADb 
    bool isInDirectMode = false;
    [SerializeField]bool jump = false;
        
    private void Start()
    {
        cameraRaycaster = Camera.main.GetComponent<CameraRaycaster>();
        thirdPersonCharacter = GetComponent<ThirdPersonCharacter>();
        currentDestination = transform.position;
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
        var playerPosition = transform.position;
        if (Input.GetMouseButton(0))
        {
            clickPoint = cameraRaycaster.hit.point;
            switch (cameraRaycaster.currentLayerHit)
            {
                case Layer.Walkable:
                    currentDestination = ShortDestination(clickPoint, walkMoveStopRadius);
                    break;
                case Layer.Enemy:
                    currentDestination = ShortDestination(clickPoint, attackStopMoveRadius);
                    break;
                default:
                    print("Unexpected layer found");
                    return;
            }
        }
        var playerToClickPoint = currentDestination - playerPosition;
        if (playerToClickPoint.magnitude >= walkMoveStopRadius)
        {
            thirdPersonCharacter.Move(playerToClickPoint, false, false);
        }
        else
        {
            thirdPersonCharacter.Move(Vector3.zero, false, jump);
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

        thirdPersonCharacter.Move(m_Move, false, jump);
        currentDestination = transform.position; //sets the click target to this new location just in case we toggle back
        jump = false;
    }

    Vector3 ShortDestination(Vector3 destination, float shortening)
    {
        Vector3 reductionVector = (destination - transform.position).normalized * shortening;
        return destination - reductionVector;
    }
    
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.black;
        Gizmos.DrawLine(transform.position, currentDestination);
        Gizmos.DrawSphere(currentDestination, 0.1f);
        Gizmos.DrawSphere(clickPoint, 0.15f);
    }
}

