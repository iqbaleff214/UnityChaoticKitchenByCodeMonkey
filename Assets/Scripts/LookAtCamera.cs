using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    private enum Mode
    {
        LookAt,
        LookAtInverted,
        Forward,
        ForwardInverted,
    }

    [SerializeField] private Mode mode = Mode.Forward;


    private void LateUpdate()
    {
        switch (mode)
        {
            case Mode.Forward:
                transform.LookAt(transform.position + Camera.main.transform.forward);
                break;
            case Mode.ForwardInverted:
                transform.LookAt(transform.position - Camera.main.transform.forward);
                break;
            case Mode.LookAt:
                transform.LookAt(Camera.main.transform);
                break;
            case Mode.LookAtInverted:
                transform.LookAt(Camera.main.transform);
                transform.Rotate(0f, 180f, 0f);
                break;
        }
    }
}
