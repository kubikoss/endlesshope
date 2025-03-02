using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapCamera : MonoBehaviour
{
    public GameObject player;

    private void LateUpdate()
    {
        transform.position = new Vector3(player.transform.position.x, 78, player.transform.position.z);
    }
}