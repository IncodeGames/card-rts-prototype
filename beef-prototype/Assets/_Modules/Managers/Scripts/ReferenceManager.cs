using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Incode.Prototype
{
    public class ReferenceManager : MonoBehaviour
    {
        private static ReferenceManager _instance = null;
        public static ReferenceManager Instance { get { return _instance; } }

        //When disabling domain reloading in editor
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        static void Reinit()
        {
            _instance = null;
        }

        [SerializeField]
        private Component[] referenceComponents;
        private Dictionary<System.Type, Object> componentLookup = new Dictionary<System.Type, Object>();

        public bool TryGetReference<T>(out T component) where T : Component
        {
            if (_instance == null)
            {
                component = null;
                return false;
            }

            Object obj = null;
            if (componentLookup.TryGetValue(typeof(T), out obj) && obj as T != null)
            {
                component = obj as T;
                return true;
            }
            Debug.LogError("Failed to get reference, reference may need to be added to the referenceComponents array in the inspector. Type of: " + typeof(T));
            component = null;
            return false;
        }

        void Awake()
        {
            if (_instance == null)
            {
                _instance = this;
            }
            else if (_instance != null && _instance != this)
            {
                Destroy(gameObject);
            }

            for (int i = 0; i < referenceComponents.Length; ++i)
            {
                if (!componentLookup.ContainsKey(referenceComponents[i].GetType()))
                {
                    componentLookup.Add(referenceComponents[i].GetType(), referenceComponents[i]);
                }
            }
        }
    }
}
