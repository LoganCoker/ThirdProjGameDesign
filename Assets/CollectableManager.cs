using System;
using System.Collections.Generic;
using UnityEngine; 

public class CollectableManager : MonoBehaviour
{
    public static CollectableManager Instance { get; private set; }

    private Dictionary<string, string> collectedItems = new Dictionary<string, string>();

    public delegate void OnItemCollected(string itemID, string itemName);
    public event OnItemCollected onItemCollected;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void AddCollectable(string itemID, string itemName)
        // Add a collected item to the tracked list 
    {
        if (!collectedItems.ContainsKey(itemName))
        {
            collectedItems.Add(itemID, itemName);
            Debug.Log($"Collected: {itemName} (ID: {itemID})");

            onItemCollected?.Invoke(itemID, itemName);
        }
    }

    public bool HasCollectable(string itemID)
        // Check if an item has been collected 
    {
        return collectedItems.ContainsKey(itemID); 
    }

    public Dictionary<string, string> GetAllCollectables()
        // Get the list of all collected items
    {
        return collectedItems;
    }

    
    public int GetCollectablesCount()
        // Get the count of collected Items
    {
        return collectedItems.Count; 
    }

}