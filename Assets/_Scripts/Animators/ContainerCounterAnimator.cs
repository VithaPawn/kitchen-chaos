using UnityEngine;

public class ContainerCounterAnimator : MonoBehaviour {
    private const string OPEN_CLOSE = "OpenClose";

    [SerializeField]
    private ContainerCounter containerCounter;

    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        containerCounter.OnGrabbedKitchenObject += ContainerCounter_OnGrabbedKitchenObject;
    }

    private void ContainerCounter_OnGrabbedKitchenObject(object sender, System.EventArgs e)
    {
        animator.SetTrigger(name: OPEN_CLOSE);
    }
}
