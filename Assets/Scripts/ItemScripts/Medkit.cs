using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class Medkit : Item
{
    [SerializeField]
    public float healAmount;

    Player player;

    private void Start()
    {
        player = FindObjectOfType<Player>();
    }
    private void Update()
    {
        if(Input.GetKey(KeyCode.F))
        {
            HealPlayer();
        }
    }

    public void HealPlayer()
    {
        player.Heal(healAmount);
        Destroy(gameObject);
    }
    
}
