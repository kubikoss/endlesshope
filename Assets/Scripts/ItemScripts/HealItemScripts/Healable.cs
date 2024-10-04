using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Healable : Item
{
    public int HealAmount => ((HealableItem)itemData).healAmount;

    Player player;

    private void Start()
    {
        player = PlayerManager.Instance.player.gameObject.GetComponent<Player>();
    }

    public void Use()
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
