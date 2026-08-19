using UnityEngine;

public class StoveCounterVisual : MonoBehaviour
{
    [SerializeField] private GameObject stoveOnGameObject;
    [SerializeField] private GameObject particleSystemGameObject;

    [SerializeField] private StoveCounter stoveCounter;

    private void Start()
    {
        stoveOnGameObject.SetActive(false);
        particleSystemGameObject.SetActive(false);

        stoveCounter.OnStateChanged += StoveCounter_OnStateChanged;
    }

    private void StoveCounter_OnStateChanged(object sender, StoveCounter.OnStateChangedEventArgs e)
    {
        bool showVisual = e.state != StoveCounter.State.Idle;
        stoveOnGameObject.SetActive(showVisual);
        particleSystemGameObject.SetActive(showVisual);
    }
}
