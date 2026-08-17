using UnityEngine;
using UnityEngine.InputSystem;

public class ClearCounter : MonoBehaviour
{

    [SerializeField] private KitchenObjectSO kitchenObjectSO;
    [SerializeField] private Transform counterSpawnPoint;
    [SerializeField] private ClearCounter secondCounter;
    [SerializeField] bool isTesting;

    private KitchenObject kitchenObject;

    private void Update()
    {
        if (isTesting && Keyboard.current.tKey.wasPressedThisFrame && secondCounter != null)
        {
            if (kitchenObject != null)
            {
                kitchenObject.SetClearCounter(secondCounter);
                Debug.Log("Kitchen object moved to second counter");
            }
        }
    }

    public void Interact()
    {
        if (kitchenObject == null)
        {
            Transform spawnedObject = Instantiate(kitchenObjectSO.prefab, counterSpawnPoint);
            spawnedObject.GetComponent<KitchenObject>().SetClearCounter(this);

        }
        else
        {
            Debug.Log("Counter already has a kitchen object!");
            Debug.Log(kitchenObject.GetClearCounter());
        }
    }

    public Transform GetKitchenObjectSpawnPoint()
    {
        return counterSpawnPoint;
    }

    public void SetKitchenObject(KitchenObject kitchenObject)
    {
        this.kitchenObject = kitchenObject;
    }

    public KitchenObject GetKitchenObject()
    {
        return kitchenObject;
    }

    public void ClearKitchenObject()
    {
        kitchenObject = null;
    }

    public bool HasKitchenObject()
    {
        return kitchenObject != null;
    }
}
