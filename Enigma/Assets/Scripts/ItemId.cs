using UnityEngine;
using System.Collections;

/// <summary>
/// MonoBehaviour to add to items' sprites in the inventory to identify the item.
/// </summary>
[RequireComponent(typeof(UnityEngine.UI.Image))]
public class ItemId : MonoBehaviour 
{
    public int Id;
}
