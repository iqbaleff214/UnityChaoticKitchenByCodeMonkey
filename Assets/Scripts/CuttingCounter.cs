using System;
using UnityEngine;

public class CuttingCounter : BaseCounter
{

    public event EventHandler<OnProgressChangedEventArgs> OnProgressChanged;
    public class OnProgressChangedEventArgs : EventArgs
    {
        public float progressNormalized;
    }
    public event EventHandler OnCut;


    [SerializeField] private CuttingRecipeSO[] cuttingRecipes;

    private int cuttingProgress;

    public override void Interact(Player player)
    {
        if (!HasKitchenObject())
        {
            if (player.HasKitchenObject() && HasRecipeWithInput(player.GetKitchenObject().GetKitchenObjectSO()))
            {
                // Player is carrying a KitchenObject that can be cut, place it on the counter
                player.GetKitchenObject().SetKitchenObjectParent(this);
                cuttingProgress = 0;
                OnProgressChanged?.Invoke(this, new OnProgressChangedEventArgs
                {
                    progressNormalized = (float) cuttingProgress / GetCuttingRecipeSOWithInput(GetKitchenObject().GetKitchenObjectSO()).cuttingProgressMax
                });
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
                cuttingProgress++;
                OnCut?.Invoke(this, EventArgs.Empty);

                CuttingRecipeSO cuttingRecipe = GetCuttingRecipeSOWithInput(inputKitchenObjectSO);

                if (cuttingProgress >= cuttingRecipe.cuttingProgressMax)
                {
                    // Cutting is complete, perform the cutting action
                    // PerformCuttingAction(player, outputKitchenObjectSO);
                    
                    // Remove the input KitchenObject from the counter
                    GetKitchenObject().DestroySelf();

                    // Spawn the output KitchenObject and give it to the player
                    KitchenObject.SpawnKitchenObject(outputKitchenObjectSO, this);
                }

                OnProgressChanged?.Invoke(this, new OnProgressChangedEventArgs
                {
                    progressNormalized = (float) cuttingProgress / cuttingRecipe.cuttingProgressMax
                });
            }
        }
    }

    private KitchenObjectSO GetOutputForInput(KitchenObjectSO inputKitchenObjectSO)
    {
        CuttingRecipeSO cuttingRecipe = GetCuttingRecipeSOWithInput(inputKitchenObjectSO);
        return cuttingRecipe != null ? cuttingRecipe.output : null;
    }

    private bool HasRecipeWithInput(KitchenObjectSO inputKitchenObjectSO)
    {
        return GetCuttingRecipeSOWithInput(inputKitchenObjectSO) != null;
    }

    private CuttingRecipeSO GetCuttingRecipeSOWithInput(KitchenObjectSO inputKitchenObjectSO)
    {
        for (int i = 0; i < cuttingRecipes.Length; i++)
        {
            if (cuttingRecipes[i].input == inputKitchenObjectSO)
            {
                return cuttingRecipes[i];
            }
        }
        return null;
    }    
}
