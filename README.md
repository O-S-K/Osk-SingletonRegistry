# **OSK Singleton Registry**

The **OSK Singleton Registry** is a lightweight, attribute-based singleton framework for Unity. It allows you to define global or scene-specific singletons without inheriting any base class.

**Version 1.0**

---

## **🚀 Features**

1. **[GlobalSingleton]**: Keeps the instance across scenes using `DontDestroyOnLoad`.
2. **[SceneSingleton("SceneA", "SceneB")]**: Limits usage of singleton in specific scenes.
3. **Auto Registry**: Automatically finds or creates the singleton when accessed via `SingletonRegistry.Get<T>()`.
4. **Editor Tools**: Menu commands to scan, inspect, and debug singleton usage.
5. **Full Logging**: Logs when registering, detecting duplicates, or misused singletons.
6. **No Inheritance Required**: Works with any `MonoBehaviour` subclass.

---

## **📦 Installation**

Install lin git in PackageManager Unity:

- https://github.com/O-S-K/Osk-SingletonRegistry.git

---

## **🛠️ Usage**

### **1. Creating a Singleton**

```csharp
[GlobalSingleton]
public class ExampleGlobalSingleton : MonoBehaviour
{
    public static ExampleGlobalSingleton Instance => SingletonRegistry.RegisterOrGet<ExampleGlobalSingleton>();

    public void Call() => Debug.Log("[ExampleGlobalSingleton] Call");

    // ==========================
    // 🧩 Usage Example
    // ==========================
    // Anywhere in code:
    //ExampleGlobalSingleton.Instance.Call();
}
```

Or restrict to specific scenes:

```csharp
[SceneSingleton("MainMenu", "Gameplay")]
public class ExampleSceneSingleton : MonoBehaviour
{
    public static ExampleSceneSingleton Instance => SingletonRegistry.RegisterOrGet<ExampleSceneSingleton>();

    public void Call() => Debug.Log("[ExampleSceneSingleton] Call");
    
    private void OnDisable()
    {
        SingletonRegistry.Clear<ExampleSceneSingleton>();
    }

    // ==========================
    // 🧩 Usage Example
    // ==========================
    // Anywhere in code:
    //ExampleSceneSingleton.Instance.Call();
}
```

### **2. Accessing Your Singleton**
```csharp
ExampleGlobalSingleton.Instance.Call();
ExampleSceneSingleton.Instance.Call(); 
```

### **3. Editor Menu**
- `Tools → Singleton → Scan GlobalSingleton Classes`
- `Tools → Singleton → Scan SceneSingleton Classes`
- `Tools → Singleton → Print Registered Singletons`

---

## **📜 License**
MIT License — Free to use, modify, and distribute.

---

## **❤️ Contributions**
Feel free to submit issues or pull requests to enhance the system. You can also extend it to support automatic prefab instantiation or asset-based registration.

---

## **📧 Contact**
- **Email**: gamecoding1999@gmail.com
- **Facebook**: [OSK Framework](https://www.facebook.com/xOskx/)

---

> Made with ❤️ by OSK for flexible Unity development.