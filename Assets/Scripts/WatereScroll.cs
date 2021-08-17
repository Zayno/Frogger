using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WatereScroll : MonoBehaviour
{
    public Renderer rend;
    public float scrollSpeed = 0.5f;


    // Update is called once per frame
    void Update()
    {
        float offset = Time.time * scrollSpeed;
        rend.material.SetTextureOffset("_MainTex", new Vector2(offset, 0));
    }
}
