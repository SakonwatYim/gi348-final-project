using System;
using UnityEngine;

public class Portal : MonoBehaviour
{
    public static event Action OnPortalEvent;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // play portal SFX
            if (AudioManager.Instance != null)
            {
                AudioManager.Instance.PlayPortal();
            }

            OnPortalEvent?.Invoke();
        }
    }
}
