using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuCamera : MonoBehaviour
{
    public CinemachineDollyCart dollyCart;
    public float moveSpeed = 2f;

    void Update()
    {
        dollyCart.m_Position += moveSpeed * Time.deltaTime;

        if(dollyCart.m_Position >= dollyCart.m_Path.PathLength)
        {
            dollyCart.m_Position = 0;
        }
    }
}
