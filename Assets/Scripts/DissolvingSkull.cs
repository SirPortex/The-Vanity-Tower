using System.Collections;
using UnityEngine;

public class DissolvingSkull : MonoBehaviour
{
    public SkinnedMeshRenderer skullMesh;

    public Material skullMaterial; 

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        skullMaterial.SetFloat("_DissolveAmount", 0f);
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.instance.readyToExit)
        {
            skullMaterial.SetFloat("_DissolveAmount", Mathf.Lerp(skullMaterial.GetFloat("_DissolveAmount"), 1f, Time.deltaTime));

        }
    }
}
