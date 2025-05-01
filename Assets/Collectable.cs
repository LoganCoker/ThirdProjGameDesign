using UnityEngine;

public class Collectable : MonoBehaviour
{
    [Header("Collectable Settings")]
    public string collectableID = "item_001";
    public string collectableName = "Test Item";

    [Header("Optional Effects")]
    public bool rotateObject = true;
    public float rotationSpeed = 50f;
    public AudioClip pickupSound;

    private bool canBeCollected = false;
    private GameObject promptUI;

    private void Start()
    {
        promptUI = GameObject.Find("PickupPromptUI");

        if (promptUI != null)
        {
            promptUI.SetActive(false);
        }
    }

    private void Update()
    {
        if (rotateObject)
        {
            transform.Rotate(Vector3.up, rotationSpeed *  Time.deltaTime);
        }

        if (canBeCollected && Input.GetKeyDown(KeyCode.E))
        {
            Collect(); 
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            canBeCollected = true;

            if (promptUI != null)
            {
                promptUI.SetActive(false);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            canBeCollected = false;

            if (promptUI != null)
            {
                promptUI.SetActive(false);
            }
        }
    }

    private void Collect()
    {
        if (pickupSound != null)
        {
            AudioSource.PlayClipAtPoint(pickupSound, transform.position);
        }

        CollectableManager.Instance.AddCollectable(collectableID, collectableName); 

        if (promptUI != null)
        {
            promptUI.SetActive(false);
        }

        Destroy(gameObject); 
    }
}
