using UnityEngine;
using UnityEngine.InputSystem;

public class ClearCounter : BaseCounter
{

    [SerializeField] private KitchenObjectSO kitchenObjectSO;

    public override void Interact(Player player)
    {
        if (HasKitchenObject())
        {
            if (player.HasKitchenObject())
            {
                // Player is carrying a KitchenObject, do nothing
            }
            else
            {
                // Player is not carrying a KitchenObject, give the player the KitchenObject from the counter
                KitchenObject counterKitchenObject = GetKitchenObject();
                counterKitchenObject.SetKitchenObjectParent(player);
            }
        } else
        {
            if (player.HasKitchenObject())
            {
                // Player is carrying a KitchenObject, place it on the counter
                KitchenObject playerKitchenObject = player.GetKitchenObject();
                playerKitchenObject.SetKitchenObjectParent(this);
            }
            else
            {
                // Player is not carrying a KitchenObject
            }
        }
    }
}
