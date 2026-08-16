using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{

    [SerializeField] private float movementSpeed = 7f;
    [SerializeField] private GameInput gameInput;
    [SerializeField] private LayerMask countersLayerMask;

    private bool isWalking;
    private Vector3 lastInteractDir;


    private void Start()
    {
        gameInput.OnInteractAction += GameInput_OnInteractAction;
    }

    private void Update()
    {
        HandleMovement();
        HandleInteraction();
    }

    private void HandleMovement()
    {
        Vector2 inputVector = gameInput.GetMovementVectorNormalized();
        Vector3 movement = new(inputVector.x, 0, inputVector.y);

        float movementDistance = movementSpeed * Time.deltaTime;
        float playerRadius = .7f;    
        float playerHeight = 2f;
        bool canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, movement, movementDistance);

        if (!canMove)
        {
            // Cannot move towards movement direction

            // Attempt only X movement
            Vector3 movementX = new(movement.x, 0, 0);
            movementX.Normalize();
            canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, movementX, movementDistance);

            if (canMove)
            {
                // Can move only on the X
                movement = movementX;
            }
            else
            {
                // Cannot move only on the X

                // Attempt only Z movement
                Vector3 movementZ = new(0, 0, movement.z);
                movementZ.Normalize();
                canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, movementZ, movementDistance);

                if (canMove)
                {
                    // Can move only on the Z
                    movement = movementZ;
                }
                else
                {
                    // Cannot move in any direction
                }
            }
        }

        if (canMove)
        {
            transform.position += movementSpeed * Time.deltaTime * movement;
        }

        isWalking = movement != Vector3.zero;

        float rotationSpeed = 10f;
        transform.forward = Vector3.Slerp(transform.forward, movement, Time.deltaTime * rotationSpeed);
    }

    private void HandleInteraction()
    {
        Vector2 inputVector = gameInput.GetMovementVectorNormalized();
        Vector3 movement = new(inputVector.x, 0, inputVector.y);
        if (movement != Vector3.zero)
        {
            lastInteractDir = movement;
        }

        float interactionDistance = 2f;
        bool hit = Physics.Raycast(transform.position, lastInteractDir, out RaycastHit raycastHit, interactionDistance, countersLayerMask);
        if (hit)
        {
            raycastHit.transform.TryGetComponent(out ClearCounter clearCounter);
            if (clearCounter != null)
            {
                
            }
        }
    }

    private void GameInput_OnInteractAction(object sender, System.EventArgs e)
    {
        if (lastInteractDir != Vector3.zero)
        {
            float interactionDistance = 2f;
            bool hit = Physics.Raycast(transform.position, lastInteractDir, out RaycastHit raycastHit, interactionDistance, countersLayerMask);
            if (hit)
            {
                raycastHit.transform.TryGetComponent(out ClearCounter clearCounter);
                if (clearCounter != null)
                {
                    clearCounter.Interact();
                }
            }
        }
    }
    public bool IsWalking()
    {
        return isWalking;
    }
}
