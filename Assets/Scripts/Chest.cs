using UnityEngine;

public class Chest : MonoBehaviour
{
    [Header("Config")]
    [SerializeField] private Transform itemPos;

    [Header("Item")]
    [SerializeField] private bool usePredefineChest;
    [SerializeField] private GameObject predefineChest;
    
    private Animator animator;
    private bool openedChest;
    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void ShowItem()
    {
        if (usePredefineChest)
        {
            Instantiate(predefineChest, transform.position, Quaternion.identity, itemPos);
        }
        else
        {
            GameObject randomItem = LevelManager.Instance.GetRandomItemForChest();
            Instantiate(randomItem, transform.position,
                Quaternion.identity, itemPos);
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (openedChest) return;
        if (other.CompareTag("Player") == false) return;
        animator.SetTrigger("OpenChest");
        ShowItem();
        openedChest = true; 
        
    }
}
