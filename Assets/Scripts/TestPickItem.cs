using UnityEngine;

public class TestPickItem : MonoBehaviour
{
    public SphereCollider sphereCollider;
    public int index;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        sphereCollider = GetComponent<SphereCollider>();
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerLeftArm playerLeftArm = other.GetComponentInParent<PlayerLeftArm>();
            if (playerLeftArm != null && playerLeftArm.readyToItem && playerLeftArm.canItem)
            {
                Debug.Log("Item picked up: " + index);
                playerLeftArm.ItemIndex = index;
            }
        }
    }
}
