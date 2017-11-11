using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {

    GameObject player;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    //We do this in late update to make sure that the camera position is calculated after all other updates are done
    //That way, the camera position is calculated after the the player position is finished being calculated in update()
    private void LateUpdate()
    {
        transform.position = player.transform.position; //camera's position is set to the player's position
    }
}