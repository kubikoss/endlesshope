using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;

public abstract class Item : MonoBehaviour
{
    public BaseItem itemData;
    public string ItemName => itemData.name;
    public Sprite ItemIcon => itemData.itemIcon;
    private Camera playerCamera;
    public Camera PlayerCamera
    {
        get { return playerCamera; }
        set { playerCamera = value; }
    }

    private void Awake()
    {
        playerCamera = Camera.main;
    }
}