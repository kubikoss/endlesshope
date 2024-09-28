using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class Medkit : Healable
{
    Player player;

    private void Start()
    {
        player = PlayerManager.Instance.player.gameObject.GetComponent<Player>();
    }

    public override void Use()
    {
        HealPlayer();
    }

    public void HealPlayer()
    {
        player.Heal(HealAmount);
        InventoryManager.Instance.RemoveItem(this);
        InventoryManager.Instance.EquipFirstSlot();
        Destroy(gameObject);
    }
}
