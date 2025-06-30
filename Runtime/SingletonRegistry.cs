using System;
using System.Linq;
using UnityEngine;
using System.Reflection;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

namespace OSK
{
    public static class SingletonRegistry
    {
        private static readonly Dictionary<Type, MonoBehaviour> k_Instances = new Dictionary<Type, MonoBehaviour>();

        public static T RegisterOrGet<T>() where T : MonoBehaviour
        {
            Type type = typeof(T);

            if (k_Instances.TryGetValue(type, out var inst))
                return (T)inst;

            var found = UnityEngine.Object.FindObjectOfType<T>();
            if (found != null)
            {
                Register(found);
                return found;
            }

            var go = new GameObject(type.Name);
            var created = go.AddComponent<T>();
            Register(created);
            return created;
        }

        private static void Register<T>(T instance) where T : MonoBehaviour
        {
            var type = typeof(T);

            if (k_Instances.ContainsKey(type))
            {
                Debug.LogWarning(
                    $"[SingletonRegistry] Duplicate singleton of type {type.Name} detected. Old instance will be overwritten.");
            }

            k_Instances[type] = instance;

            if (type.GetCustomAttribute<GlobalSingletonAttribute>() != null)
            {
                UnityEngine.Object.DontDestroyOnLoad(instance.gameObject);
                Debug.Log($"[SingletonRegistry] Registered GlobalSingleton: {type.Name}");
            }
            else if (type.GetCustomAttribute<SceneSingletonAttribute>() is SceneSingletonAttribute sceneAttr)
            {
                var currentScene = SceneManager.GetActiveScene().name;
                if (sceneAttr.AllowedScenes is { Length: > 0 })
                {
                    if (!sceneAttr.AllowedScenes.Contains(currentScene))
                    {
                        Debug.LogError(
                            $"[SingletonRegistry] SceneSingleton {type.Name} is not allowed in scene '{currentScene}'. " +
                            $"Allowed: {string.Join(", ", sceneAttr.AllowedScenes)}");
                    }
                }

                Debug.Log($"[SingletonRegistry] Registered SceneSingleton: {type.Name} (Scene: {currentScene})");
            }
            else
            {
                Debug.Log($"[SingletonRegistry] Registered SceneSingleton: {type.Name}");
            }
        }
        
        public static void UnRegister<T>() where T : MonoBehaviour
        {
            var type = typeof(T);
            if (k_Instances.TryGetValue(type, out var instance))
            {
                Debug.Log($"[SingletonRegistry] Unregistered Singleton: {type.Name}");

                if (instance.gameObject)
                {
                    UnityEngine.Object.Destroy(instance.gameObject);
                }
                k_Instances.Remove(type);
            }
            else
            {
                Debug.LogWarning($"[SingletonRegistry] No Singleton of type {type.Name} found to unregister.");
            }
        }
        
        public static void Clear<T>() where T : MonoBehaviour
        {
            var type = typeof(T);
            if (k_Instances.ContainsKey(type))
            {
                Debug.Log($"[SingletonRegistry] Removed SceneSingleton: {type.Name}");
                k_Instances.Remove(type);
            }
            else
            {
                Debug.LogWarning($"[SingletonRegistry] No SceneSingleton of type {type.Name} found to remove.");
            }
        }

        public static void ClearAll()
        {
            var removeList = (from kv in k_Instances where kv.Key.GetCustomAttribute<GlobalSingletonAttribute>() == null select kv.Key).ToList();
            foreach (var t in removeList)
            {
                Debug.Log($"[SingletonRegistry] Removed SceneSingleton: {t.Name}");
                k_Instances.Remove(t);
            }
        }
        

        public static IEnumerable<Type> AllTypes() => k_Instances.Keys;
        public static int Count => k_Instances.Count;
    }
}