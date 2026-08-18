using UnityEngine;
using UnityEngine.InputSystem;

public class ClearCounter : BaseCounter
{

    [SerializeField] private KitchenObjectSO kitchenObjectSO;

    public override void Interact(Player player)
    {
        Debug.Log("Interacting with ClearCounter");
    }
}
