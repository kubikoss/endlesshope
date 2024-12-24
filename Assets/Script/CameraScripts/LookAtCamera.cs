using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    public Transform target;

    private void LateUpdate()
    {
        transform.LookAt(target);
        transform.Rotate(0, 180, 0);
    }
}
