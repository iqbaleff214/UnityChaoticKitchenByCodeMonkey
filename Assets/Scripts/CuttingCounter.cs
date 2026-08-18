using UnityEngine;

public class CuttingCounter : BaseCounter
{

    [SerializeField] private CuttingRecipeSO[] cuttingRecipes;

    public override void Interact(Player player)
    {
        if (!HasKitchenObject())
        {
            if (player.HasKitchenObject() && HasRecipeWithInput(player.GetKitchenObject().GetKitchenObjectSO()))
            {
                // Player is carrying a KitchenObject that can be cut, place it on the counter
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
            KitchenObjectSO inputKitchenObjectSO = GetKitchenObject().GetKitchenObjectSO();
            KitchenObjectSO outputKitchenObjectSO = GetOutputForInput(inputKitchenObjectSO);

            if (outputKitchenObjectSO != null)
            {
                // Remove the input KitchenObject from the counter
                GetKitchenObject().DestroySelf();

                // Spawn the output KitchenObject and give it to the player
                KitchenObject.SpawnKitchenObject(outputKitchenObjectSO, this);
            }
        }
    }

    private KitchenObjectSO GetOutputForInput(KitchenObjectSO inputKitchenObjectSO)
    {
        for (int i = 0; i < cuttingRecipes.Length; i++)
        {
            if (cuttingRecipes[i].input == inputKitchenObjectSO)
            {
                return cuttingRecipes[i].output;
            }
        }
        return null;
    }

    private bool HasRecipeWithInput(KitchenObjectSO inputKitchenObjectSO)
    {
        for (int i = 0; i < cuttingRecipes.Length; i++)
        {
            if (cuttingRecipes[i].input == inputKitchenObjectSO)
            {
                return true;
            }
        }
        return false;
    }
}
