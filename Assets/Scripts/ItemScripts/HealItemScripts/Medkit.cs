using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class Medkit : Healable
{
    float distance;
    Player player;

    private void Start()
    {
        player = PlayerManager.Instance.player.gameObject.GetComponent<Player>();
    }

    private void Update()
    {
        distance = Vector3.Distance(transform.position, PlayerManager.Instance.player.transform.position);

        if (Input.GetKeyDown(KeyCode.F) && distance <= 2f)
        {
            Use();
        }
    }

    public override void Use()
    {
        HealPlayer();
    }

    public void HealPlayer()
    {
        player.Heal(HealAmount);
        Destroy(gameObject);
    }
    
}
