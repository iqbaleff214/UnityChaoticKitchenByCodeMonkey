using UnityEngine;

public class TrashCounter : BaseCounter
{
    public override void Interact(Player player)
    {
        if (player.HasKitchenObject())
        {
            // Player is carrying a KitchenObject, destroy it
            KitchenObject playerKitchenObject = player.GetKitchenObject();
            playerKitchenObject.DestroySelf();
        }
    }
}
