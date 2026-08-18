using UnityEngine;

public class BaseCounter : MonoBehaviour, IKitchenObjectParent
{
    [SerializeField] private Transform counterSpawnPoint;

    private KitchenObject kitchenObject;

    public virtual void Interact(Player player)
    {
        // This method is meant to be overridden by derived classes.
        Debug.Log("BaseCounter Interact method called. Override this method in derived classes.");
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
