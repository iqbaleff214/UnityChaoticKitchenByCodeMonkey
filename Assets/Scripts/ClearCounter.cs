using UnityEngine;

public class ClearCounter : MonoBehaviour
{

    [SerializeField] private KitchenObjectSO kitchenObject;
    [SerializeField] private Transform counterSpawnPoint;

    public void Interact()
    {
        Debug.Log("Interacting with ClearCounter");
        if (kitchenObject != null)
        {
            Transform spawnedObject = Instantiate(kitchenObject.prefab, counterSpawnPoint);
            spawnedObject.localPosition = Vector3.zero;

            Debug.Log(spawnedObject.GetComponent<KitchenObject>().GetKitchenObjectSO());
        }
    }
}
