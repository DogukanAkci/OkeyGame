using UnityEngine;

    public class LevelController : MonoBehaviour
    {

        public static LevelController Instance; // Singleton Design pattern
        public bool isStoneInAnim;
        private void Awake()
        {
            Instance = this;
        }
    }
