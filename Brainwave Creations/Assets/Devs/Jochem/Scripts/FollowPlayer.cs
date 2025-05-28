using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    public Transform playerTarget;
    [SerializeField] private Vector3 offSet;
    private float smoothSpeed = 0.125f;

    private void LateUpdate()
    {
        Vector3 desiredPostion = playerTarget.position + offSet;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPostion, smoothSpeed);
        transform.position = desiredPostion;
        transform.LookAt(playerTarget);
    }
}
