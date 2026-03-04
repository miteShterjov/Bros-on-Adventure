using UnityEngine;

namespace GameEffects
{
    public enum BackgroundType { Blue, Brown, Gray, Green, Pink, Purple, Yellow }

    public class AnimatedBackground : MonoBehaviour
    {
        [Header("Color")]
        [SerializeField] private BackgroundType backgroundType;
        [SerializeField] private Texture2D[] textures;
        [Space][Header("Movement")]
        [SerializeField] private Vector2 movementDirection;
        
        private MeshRenderer _mesh;

        private void Awake()
        {
            _mesh = GetComponent<MeshRenderer>();
        }

        private void Start()
        {
            UpdateBackgroundTexture();
        }

        private void Update()
        {
            _mesh.sharedMaterial.mainTextureOffset += movementDirection * Time.deltaTime;
        }
    
        [ContextMenu("Update Background Texture")]
        private void UpdateBackgroundTexture()
        {
            if (_mesh == null) _mesh = GetComponent<MeshRenderer>();
            _mesh.material.mainTexture = textures[(int)backgroundType];
        }
    }
}