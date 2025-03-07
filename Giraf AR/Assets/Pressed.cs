using UnityEngine;
using UnityEngine.InputSystem;

public class Pressed : MonoBehaviour
{
    public Animator animator;
    public string targetTag = "Interactable"; // Sæt dette tag på objekterne, som kan aktiveres

    private void Update()
    {
        if (Touchscreen.current.primaryTouch.press.wasPressedThisFrame)
        {
            PerformRaycast();
        }
    }

    private void PerformRaycast()
    {
        Ray ray = Camera.main.ScreenPointToRay(Touchscreen.current.primaryTouch.position.ReadValue());
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
        {
            if (hit.collider.CompareTag(targetTag)) // Kun aktiver hvis objektet har det rigtige tag
            {
                animator.SetTrigger("Pressed");
                Debug.Log("Hit object: " + hit.collider.gameObject.name);
            }
        }
    }
}
