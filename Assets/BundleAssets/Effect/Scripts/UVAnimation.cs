using UnityEngine;
using System.Collections;

public class UVAnimation : MonoBehaviour
{
    public Vector2 speed;
    private Material material = null;
    private Vector2 textureOffset = Vector2.one;
    void Start()
    {
        MeshRenderer meshRender = gameObject.GetComponent<MeshRenderer>();
        if (meshRender == null)
            return;
        material = meshRender.material;
        textureOffset = material.mainTextureOffset;
    }

    // Update is called once per frame
    void Update()
    {
        if (material == null)
            return;

        textureOffset += speed * Time.deltaTime;
        material.SetTextureOffset("_MainTex", textureOffset);
    }

    //     void OnDestroy()
    //     {
    //         if (material == null)
    //             return;
    // 
    //         material.mainTextureOffset = oldOffset;
    //    }
}
