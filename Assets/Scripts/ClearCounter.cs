using UnityEngine;
using UnityEngine.InputSystem;

public class ClearCounter : MonoBehaviour, IKitchenObjectParent
{

    [SerializeField] private KitchenObjectSO kitchenObjectSO;
    [SerializeField] private Transform counterSpawnPoint;

    private KitchenObject kitchenObject;

    public void Interact(Player player)
    {
        if (kitchenObject == null)
        {
            Transform spawnedObject = Instantiate(kitchenObjectSO.prefab, counterSpawnPoint);
            spawnedObject.GetComponent<KitchenObject>().SetKitchenObjectParent(this);

        }
        else
        {
            kitchenObject.SetKitchenObjectParent(player);
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
