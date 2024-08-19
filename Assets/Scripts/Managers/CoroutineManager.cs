using System.Collections;
using UnityEngine;

namespace Managers
{
    public class CoroutineManager : MonoBehaviour
    {
        private static CoroutineManager _instance;

        public static CoroutineManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    // Create a new GameObject if no instance exists
                    GameObject obj = new GameObject("CoroutineManager");
                    _instance = obj.AddComponent<CoroutineManager>();

                    // Ensure it persists across scenes
                    DontDestroyOnLoad(obj);
                }
                return _instance;
            }
        }

        public void StartManagedCoroutine(IEnumerator coroutine)
        {
            StartCoroutine(coroutine);
        } 
    }
}