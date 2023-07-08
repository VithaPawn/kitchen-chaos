using System;
using UnityEngine;

public class Player : MonoBehaviour {

    public static Player Instance { get; private set; }

    [SerializeField]
    private int moveSpeed = 10;
    [SerializeField]
    private GameInput gameInput;

    private readonly float charHeight = 1f;
    private readonly float charRadius = 0.7f;
    private readonly int rotationSpeed = 15;
    private readonly float interactDistance = 1f;
    private bool isWalking;
    private Vector3 lastInteractVector;
    private Vector2 inputVector;
    private ClearCounter selectedCounter;

    public event EventHandler<OnSelectedCounterChangedEventArgs> OnSelectedCounterChanged;
    public class OnSelectedCounterChangedEventArgs : EventArgs {
        public ClearCounter selectedCounter;
    }

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("There is more than one Player instance");
        }
        Instance = this;
    }

    private void Start()
    {
        gameInput.OnInteractAction += GameInput_OnInteractAction;
    }

    private void GameInput_OnInteractAction(object sender, EventArgs e)
    {
        if (selectedCounter != null)
        {
            selectedCounter.Interact();
        }
    }

    // Update is called once per frame
    private void Update()
    {
        inputVector = gameInput.GetInputVectorNormalized();
        HandleMovement();
        HandleInteraction();
    }

    private void HandleMovement()
    {
        float moveDistance = moveSpeed * Time.deltaTime;
        Vector3 directionVector = GetDirectionVectorForMove(moveDistance);

        // Check walk status to enable/disable walk animation
        isWalking = directionVector != Vector3.zero;

        // Handle viewing direction of character
        transform.forward = Vector3.Slerp(transform.forward, directionVector, Time.deltaTime * rotationSpeed);

        // Handle movement
        bool canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * charHeight, charRadius, directionVector, moveDistance);
        if (canMove)
        {
            transform.position += directionVector * moveDistance;
        }

    }

    public bool GetIsWalking()
    {
        return isWalking;
    }

    private Vector3 GetDirectionVectorForMove(float moveDistance)
    {
        Vector3 directionVector = new Vector3(inputVector.x, 0f, inputVector.y);
        bool canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * charHeight, charRadius, directionVector, moveDistance);

        if (!canMove)
        {
            Vector3 directionVectorX = new Vector3(directionVector.x, 0f, 0f);
            canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * charHeight, charRadius, directionVectorX, moveDistance);

            if (canMove && directionVectorX != Vector3.zero)
            {
                directionVector = directionVectorX;
            }
            else
            {
                Vector3 directionVectorZ = new Vector3(0f, 0f, directionVector.z);
                canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * charHeight, charRadius, directionVectorZ, moveDistance);
                if (canMove && directionVectorZ != Vector3.zero)
                {
                    directionVector = directionVectorZ;
                }
            }
        }

        return directionVector;

    }

    private void HandleInteraction()
    {
        Vector3 directionVector = new Vector3(inputVector.x, 0f, inputVector.y);

        if (directionVector != Vector3.zero)
        {
            lastInteractVector = directionVector;
        }

        if (Physics.Raycast(transform.position, lastInteractVector, out RaycastHit hitInfo, interactDistance))
        {
            if (hitInfo.transform.TryGetComponent(out ClearCounter clearCounter))
            {
                if (selectedCounter != clearCounter)
                {
                    HandleChangeSelectedCounter(clearCounter);
                }
            }
            else
            {
                HandleChangeSelectedCounter(null);
            }
        }
        else
        {
            HandleChangeSelectedCounter(null);
        }
    }

    private void HandleChangeSelectedCounter(ClearCounter clearCounter)
    {
        selectedCounter = clearCounter;
        OnSelectedCounterChanged?.Invoke(this, new OnSelectedCounterChangedEventArgs
        {
            selectedCounter = clearCounter
        });
    }
}
