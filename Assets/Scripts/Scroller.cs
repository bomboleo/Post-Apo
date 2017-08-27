using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scroller : MonoBehaviour {

    public float scrollSpeed;
    private Renderer rend;
    private Vector2 savedOffset;
    private string texID = "_MainTex";

    // Use this for initialization
    void Start () {
        rend = GetComponent<Renderer>();
        savedOffset = rend.material.GetTextureOffset(texID);
    }

    // Update is called once per frame
    void Update () {
        float y = Mathf.Repeat(Time.time * scrollSpeed, 1);
        Vector2 offset = new Vector2(savedOffset.x, y);
        rend.material.SetTextureOffset(texID, offset);
	}

    void onDisable() {
        rend.material.SetTextureOffset(texID, savedOffset);
    }
}
