using UnityEngine;

public class Player : MonoBehaviour {

    [SerializeField]
    private int moveSpeed = 10;
    [SerializeField]
    private GameInput gameInput;

    private float charHeight = 1f;
    private float charRadius = 0.7f;
    private int rotationSpeed = 15;
    private bool isWalking;
    private bool canMove;
    private Vector2 inputVector;

    // Update is called once per frame
    void Update()
    {
        inputVector = gameInput.GetInputVectorNormalized();
        HandleMovement();
    }

    private void HandleMovement()
    {
        Vector3 directionVector = new Vector3(inputVector.x, 0f, inputVector.y);
        float moveDistance = moveSpeed * Time.deltaTime;

        canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * charHeight, charRadius, directionVector, moveDistance);

        if (!canMove)
        {
            Vector3 directionVectorX = new Vector3(directionVector.x, 0f, 0f);
            canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * charHeight, charRadius, directionVectorX, moveDistance);

            if (canMove)
            {
                directionVector = directionVectorX;
            }
            else
            {
                Vector3 directionVectorZ = new Vector3(0f, 0f, directionVector.z);
                canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * charHeight, charRadius, directionVectorZ, moveDistance);
                if (canMove)
                {
                    directionVector = directionVectorZ;
                }
            }
        }

        if (canMove)
        {
            transform.position += directionVector * moveDistance;
        }
        transform.forward = Vector3.Slerp(transform.forward, directionVector, Time.deltaTime * rotationSpeed);

        isWalking = directionVector != Vector3.zero;
    }

    public bool GetIsWalking()
    {
        return isWalking;
    }

    //private void HandleCollision()
    //{
    //    Vector3 dirVector = new Vector3(inputVector.x, 0f, inputVector.y);

    //}
}
