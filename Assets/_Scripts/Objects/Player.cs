using System;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class Player : NetworkBehaviour, IKitchenObjectParent {
    [SerializeField] private int moveSpeed = 10;
    [SerializeField] private Transform KitchentObjectHoldPoint;
    [SerializeField] private LayerMask collistionsLayerMask;
    [SerializeField] private List<Vector3> spawnPositionList;

    private readonly float charRadius = 0.7f;
    private readonly int rotationSpeed = 15;
    private readonly float interactDistance = 1f;
    private bool isWalking;
    private Vector3 lastInteractVector;
    private Vector2 inputVector;
    private BaseCounter selectedCounter;
    private KitchenObject currentKitchenObject;

    [Header("Events")]
    [SerializeField] private CounterEventSO selectedCounterChanged;
    public event Action OnPickupSomething;

    private void Start()
    {
        GameInput.Instance.OnInteractAction += GameInput_OnInteractAction;
        GameInput.Instance.OnInteractAlternateAction += GameInput_OnInteractAlternateAction; ;
    }

    public override void OnDestroy()
    {
        GameInput.Instance.OnInteractAction -= GameInput_OnInteractAction;
        GameInput.Instance.OnInteractAlternateAction -= GameInput_OnInteractAlternateAction;
        base.OnDestroy();
    }

    public override void OnNetworkSpawn()
    {
        if (IsOwner)
        {
            transform.position = spawnPositionList[(int)OwnerClientId];
        }

        if (IsServer)
        {
            NetworkManager.Singleton.OnClientDisconnectCallback += NetworkManager_OnClientDisconnectCallback;
        }
    }

    private void NetworkManager_OnClientDisconnectCallback(ulong clientId)
    {
        if (clientId == OwnerClientId && HasCurrentKitchenObject())
        {
            KitchenObject.DestroyKitchenObject(GetCurrentKitchenObject());
        }
    }

    private void GameInput_OnInteractAlternateAction(object sender, EventArgs e)
    {
        if (!GameHandler.Instance.IsGamePlaying()) return;
        if (selectedCounter != null)
        {
            selectedCounter.InteractAlternate();
        }
    }

    private void GameInput_OnInteractAction(object sender, EventArgs e)
    {
        if (!GameHandler.Instance.IsGamePlaying()) return;
        if (selectedCounter != null)
        {
            selectedCounter.Interact(this);
        }
    }

    // Update is called once per frame
    private void Update()
    {
        if (!GameHandler.Instance.IsGamePlaying()) return;
        if (!IsOwner) return;
        inputVector = GameInput.Instance.GetInputVectorNormalized();
        if (Time.timeScale > 0)
        {
            HandleMovement();
            HandleInteraction();
        }
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
        bool canMove = !Physics.BoxCast(transform.position, Vector3.one * charRadius, directionVector, Quaternion.identity, moveDistance, collistionsLayerMask);
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
        bool canMove = !Physics.BoxCast(transform.position, Vector3.one * charRadius, directionVector, Quaternion.identity, moveDistance, collistionsLayerMask);

        if (!canMove)
        {
            Vector3 directionVectorX = new Vector3(directionVector.x, 0f, 0f);
            canMove = !Physics.BoxCast(transform.position, Vector3.one * charRadius, directionVectorX, Quaternion.identity, moveDistance, collistionsLayerMask);

            if (canMove && directionVectorX != Vector3.zero)
            {
                directionVector = directionVectorX;
            }
            else
            {
                Vector3 directionVectorZ = new Vector3(0f, 0f, directionVector.z);
                canMove = !Physics.BoxCast(transform.position, Vector3.one * charRadius, directionVectorZ, Quaternion.identity, moveDistance, collistionsLayerMask);
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
            if (hitInfo.transform.TryGetComponent(out BaseCounter interactingCounter))
            {
                if (selectedCounter != interactingCounter)
                {
                    HandleChangeSelectedCounter(interactingCounter);
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

    private void HandleChangeSelectedCounter(BaseCounter interactingCounter)
    {
        selectedCounter = interactingCounter;
        selectedCounterChanged.RaiseEvent(interactingCounter);
    }


    public Transform GetKitchenObjectFollowTransform()
    {
        return KitchentObjectHoldPoint;
    }

    public KitchenObject GetCurrentKitchenObject()
    {
        return currentKitchenObject;
    }

    public void SetCurrentKitchenObject(KitchenObject kitchenObject)
    {
        currentKitchenObject = kitchenObject;

        if (kitchenObject != null)
        {
            OnPickupSomething?.Invoke();
        }
    }

    public void ClearCurrentKitchenObject()
    {
        currentKitchenObject = null;
    }

    public bool HasCurrentKitchenObject()
    {
        return (currentKitchenObject != null);
    }

    public NetworkObject GetNetworkObject()
    {
        return NetworkObject;
    }
}
