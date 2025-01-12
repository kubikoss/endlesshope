using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Extractor : Item
{
    public ExtractorItem extractorItem;
    public float Range => extractorItem.range;
    public int Ammo
    {
        get { return extractorItem.ammo; }
        set { extractorItem.ammo = value; }
    }
    public bool CanTakeVenom
    {
        get { return extractorItem.canTakeVenom; }
        set { extractorItem.canTakeVenom = value; }
    }
    public float ReloadTime
    {
        get { return extractorItem.reloadTime; }
        set { extractorItem.reloadTime = value; }
    }
    public GameObject FilledExtractor => extractorItem.filledExtractor;

    private bool isDestroyed = false;

    private void Start()
    {
        CanTakeVenom = true;
    }

    private void Update()
    {
        TakeVenom();
    }

    public void TakeVenom()
    {
        if (Input.GetMouseButtonDown(1) && !InventoryManager.Instance.isInventoryOpened && CanTakeVenom)
        {
            RaycastHit hit;

            if (Physics.Raycast(PlayerManager.Instance.mainCamera.transform.position, PlayerManager.Instance.mainCamera.transform.forward, out hit, Range))
            {
                if (hit.collider.CompareTag("Enemy"))
                {
                    GameObject extractor = Instantiate(FilledExtractor, PlayerManager.Instance.transform.position, Quaternion.identity);
                    extractor.GetComponent<ItemPickup>().Interact();
                    isDestroyed = true;
                    Destroy(this.gameObject);
                    Destroy(InventoryManager.Instance.GetInventoryItem(this).gameObject);
                }  
            }
            CanTakeVenom = false;

            if(!isDestroyed)
                StartCoroutine(Reload(ReloadTime));
        }
    }

    private IEnumerator Reload(float timer)
    {
        yield return new WaitForSeconds(timer);
        CanTakeVenom = true;
    }
}
