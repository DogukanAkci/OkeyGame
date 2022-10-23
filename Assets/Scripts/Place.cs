using UnityEngine;
using UnityEngine.Serialization;

public class Place : MonoBehaviour
    {
        public static Place Instance;

        private void Awake()
        {
            Instance = this;
        }

        public Unit unitHolder;//Unit Scriptini place lere atmak için oluşturduk
        public Vector2 coords;//place koordinatlarını alıcağız
        public int searchCountSeri = 0,searchCountPer=0;
    }
