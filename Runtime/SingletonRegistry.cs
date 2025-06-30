using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace OSK
{
    public static class SingletonRegistry
    {
        private static Dictionary<Type, MonoBehaviour> instances = new Dictionary<Type, MonoBehaviour>();

        public static T Get<T>() where T : MonoBehaviour
        {
            Type type = typeof(T);

            if (instances.TryGetValue(type, out var inst))
                return (T)inst;

            var found = UnityEngine.Object.FindObjectOfType<T>();
            if (found != null)
            {
                Register(found);
                return found;
            }

            GameObject go = new GameObject(type.Name);
            var created = go.AddComponent<T>();
            Register(created);
            return created;
        }

        public static void Register<T>(T instance) where T : MonoBehaviour
        {
            var type = typeof(T);

            if (instances.ContainsKey(type))
            {
                Debug.LogWarning(
                    $"[SingletonRegistry] Duplicate singleton of type {type.Name} detected. Old instance will be overwritten.");
            }

            instances[type] = instance;

            if (type.GetCustomAttribute<GlobalSingletonAttribute>() != null)
            {
                UnityEngine.Object.DontDestroyOnLoad(instance.gameObject);
                Logg.Log($"[SingletonRegistry] Registered GlobalSingleton: {type.Name}");
            }
            else if (type.GetCustomAttribute<SceneSingletonAttribute>() is SceneSingletonAttribute sceneAttr)
            {
                string currentScene = SceneManager.GetActiveScene().name;
                if (sceneAttr.AllowedScenes != null && sceneAttr.AllowedScenes.Length > 0)
                {
                    if (!System.Linq.Enumerable.Contains(sceneAttr.AllowedScenes, currentScene))
                    {
                        Logg.LogError(
                            $"[SingletonRegistry] SceneSingleton {type.Name} is not allowed in scene '{currentScene}'. " +
                            $"Allowed: {string.Join(", ", sceneAttr.AllowedScenes)}");
                    }
                }

                Logg.Log($"[SingletonRegistry] Registered SceneSingleton: {type.Name} (Scene: {currentScene})");
            }
            else
            {
                Logg.Log($"[SingletonRegistry] Registered SceneSingleton: {type.Name}");
            }
        }

        public static void ClearSceneSingletons()
        {
            var removeList = new List<Type>();
            foreach (var kv in instances)
            {
                if (kv.Key.GetCustomAttribute<GlobalSingletonAttribute>() == null)
                {
                    removeList.Add(kv.Key);
                }
            }

            foreach (var t in removeList)
            {
                Logg.Log($"[SingletonRegistry] Removed SceneSingleton: {t.Name}");
                instances.Remove(t);
            }
        }

        public static IEnumerable<Type> AllTypes() => instances.Keys;
        public static int Count => instances.Count;
    }
}