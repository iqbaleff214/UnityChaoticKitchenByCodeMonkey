using UnityEngine;

public class CuttingCounter : BaseCounter
{

    [SerializeField] private KitchenObjectSO kitchenObjectSO;

    public override void Interact(Player player)
    {
        if (!HasKitchenObject())
        {
            if (player.HasKitchenObject())
            {
                player.GetKitchenObject().SetKitchenObjectParent(this);
            }
        } 
        else
        {
            if (!player.HasKitchenObject())
            {
                // Player is not carrying a KitchenObject, give the player the KitchenObject from the counter
                KitchenObject counterKitchenObject = GetKitchenObject();
                counterKitchenObject.SetKitchenObjectParent(player);
            }
        }
    }

    public override void Action(Player player)
    {
        if (HasKitchenObject() && !player.HasKitchenObject())
        {
            GetKitchenObject().DestroySelf();

            KitchenObject.SpawnKitchenObject(kitchenObjectSO, this);
        }
    }
}
