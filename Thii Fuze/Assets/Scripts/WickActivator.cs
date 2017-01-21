using UnityEngine;

public class WickActivator : MonoBehaviour
{

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Trace>().getTraceState() == Trace.TraceState.drawing)
            collision.GetComponent<Trace>().activateDeleting();
    }
}
