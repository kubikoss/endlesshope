using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Crafting Recipe", menuName = "ScriptableItem/Crafting/Recipe")]
public class CraftingRecipes : ScriptableObject
{
    public string recipeName;
    public Item[] requiredItems;
    public Item resultItem;
}