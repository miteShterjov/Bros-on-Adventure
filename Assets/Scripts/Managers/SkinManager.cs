using UnityEngine;

namespace Managers
{
    public class SkinManager : MonoBehaviour
    {
        public static SkinManager Instance; 
        
        public int chosenSkinID;

        private void Awake()
        {
            DontDestroyOnLoad(gameObject);

            if (Instance == null) Instance = this;
            else Destroy(gameObject);
        }
    
        public void SetSkinID(int skinID) => chosenSkinID = skinID;
        public int GetSkinID() => chosenSkinID;
    }
    
}
