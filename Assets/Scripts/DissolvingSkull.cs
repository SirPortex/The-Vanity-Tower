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
            Invoke(nameof(DissolveMaterial), 0.5f);

        }
    }

    void DissolveMaterial()
    {
        skullMaterial.SetFloat("_DissolveAmount", Mathf.Lerp(skullMaterial.GetFloat("_DissolveAmount"), 1f, Time.deltaTime));
    }
}
