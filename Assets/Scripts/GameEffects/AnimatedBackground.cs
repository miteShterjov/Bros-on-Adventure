using System;
using UnityEngine;

public enum BackgroundType { Blue, Brown, Gray, Green, Pink, Purple, Yellow }

public class AnimatedBackground : MonoBehaviour
{
    [Header("Color")]
    [SerializeField] private BackgroundType backgroundType;
    [SerializeField] private Texture2D[] textures;
    [Header("Movement"), Space]
    [SerializeField] private Vector2 movementDirection;
    private MeshRenderer mesh;

    private void Awake()
    {
        mesh = GetComponent<MeshRenderer>();
    }

    private void Start()
    {
        UpdateBackgroundTexture();
    }

    private void Update()
    {
        mesh.sharedMaterial.mainTextureOffset += movementDirection * Time.deltaTime;
    }
    
    [ContextMenu("Update Background Texture")]
    private void UpdateBackgroundTexture()
    {
        if (mesh == null) mesh = GetComponent<MeshRenderer>();
        mesh.material.mainTexture = textures[(int)backgroundType];
    }
}
