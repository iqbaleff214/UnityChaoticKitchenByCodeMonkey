using System;
using UnityEngine;

public class StoveCounter : BaseCounter, IHasProgress
{

    public event EventHandler<IHasProgress.OnProgressChangedEventArgs> OnProgressChanged;
    public event EventHandler<OnStateChangedEventArgs> OnStateChanged;
    public class OnStateChangedEventArgs : EventArgs
    {
        public State state;
    }


    public enum State
    {
        Idle, // nothing on top
        Frying, // currently frying
        Fried, // fully cooked
        Burned // overcooked
    }

    [SerializeField] private FryingRecipeSO[] fryingRecipes;
    [SerializeField] private BurningRecipeSO[] burningRecipes;

    private State state;
    private float fryingTimer;
    private float burningTimer;
    private FryingRecipeSO fryingRecipeSO;
    private BurningRecipeSO burningRecipeSO;

    private void Start()
    {
        state = State.Idle;
        fryingTimer = 0f;
        fryingRecipeSO = null;
    }

    private void Update()
    {
        switch (state)
        {
            case State.Idle:
                // No frying in progress
                break;
            case State.Frying:
                fryingTimer += Time.deltaTime;


                OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
                {
                   progressNormalized = fryingTimer / fryingRecipeSO.fryingTimerMax 
                });


                if (fryingTimer >= fryingRecipeSO.fryingTimerMax)
                {
                    GetKitchenObject().DestroySelf();
                    KitchenObject.SpawnKitchenObject(fryingRecipeSO.output, this);

                    state = State.Fried; // Transition to Fried state
                    burningTimer = 0f; // Reset the burning timer when the item is fully cooked

                    burningRecipeSO = GetBurningRecipeSOWithInput(fryingRecipeSO.output);

                    OnStateChanged?.Invoke(this, new OnStateChangedEventArgs
                    {
                        state = state
                    });
                }

                break;
            case State.Fried:
                burningTimer += Time.deltaTime;

                OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
                {
                   progressNormalized = burningTimer / burningRecipeSO.burningTimerMax
                });

                if (burningTimer >= burningRecipeSO.burningTimerMax)
                {
                    GetKitchenObject().DestroySelf();
                    KitchenObject.SpawnKitchenObject(burningRecipeSO.output, this);

                    // Optionally, you can spawn a burned version of the item here if you have one
                    state = State.Burned; // Transition to Burned state

                    OnStateChanged?.Invoke(this, new OnStateChangedEventArgs
                    {
                        state = state
                    });

                    OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
                    {
                        progressNormalized = 0f
                    });
                }

                break;
            case State.Burned:
                // Handle logic for when the item is overcooked
                break;
        }
    }

    public override void Interact(Player player)
    {
        if (!HasKitchenObject())
        {
            if (player.HasKitchenObject() && HasRecipeWithInput(player.GetKitchenObject().GetKitchenObjectSO()))
            {
                // Player is carrying a KitchenObject that can be fried, place it on the counter
                player.GetKitchenObject().SetKitchenObjectParent(this);
                fryingRecipeSO = GetFryingRecipeSOWithInput(GetKitchenObject().GetKitchenObjectSO());
                state = State.Frying; // Transition to Frying state
                fryingTimer = 0f; // Reset the frying timer when a new object is placed

                OnStateChanged?.Invoke(this, new OnStateChangedEventArgs
                {
                    state = state
                });

                OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
                {
                   progressNormalized = fryingTimer / fryingRecipeSO.fryingTimerMax 
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
                fryingRecipeSO = null; // Clear the current frying recipe
                burningRecipeSO = null; // Clear the current burning recipe
                state = State.Idle; // Transition to Idle state
                fryingTimer = 0f; // Reset the frying timer when the object is taken

                OnStateChanged?.Invoke(this, new OnStateChangedEventArgs
                {
                    state = state
                });

                OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
                {
                    progressNormalized = 0f
                });
            }
        }
    }

    private bool HasRecipeWithInput(KitchenObjectSO inputKitchenObjectSO)
    {
        return GetFryingRecipeSOWithInput(inputKitchenObjectSO) != null;
    }

    private FryingRecipeSO GetFryingRecipeSOWithInput(KitchenObjectSO inputKitchenObjectSO)
    {
        foreach (FryingRecipeSO fryingRecipe in fryingRecipes)
        {
            if (fryingRecipe.input == inputKitchenObjectSO)
            {
                return fryingRecipe;
            }
        }
        return null;
    }

    private BurningRecipeSO GetBurningRecipeSOWithInput(KitchenObjectSO inputKitchenObjectSO)
    {
        foreach (BurningRecipeSO burningRecipe in burningRecipes)
        {
            if (burningRecipe.input == inputKitchenObjectSO)
            {
                return burningRecipe;
            }
        }
        return null;
    }
}
