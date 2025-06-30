using OSK;
using UnityEngine;


[GlobalSingleton]
public class ExampleGlobalSingleton : MonoBehaviour
{
    public static ExampleGlobalSingleton Instance => SingletonRegistry.Get<ExampleGlobalSingleton>();

    public void Call() => Debug.Log("[ExampleGlobalSingleton] Call");

    // ==========================
    // 🧩 Usage Example
    // ==========================
    // Anywhere in code:
    //ExampleGlobalSingleton.Instance.Call();
}

[SceneSingleton("MainMenu", "Gameplay")]
public class ExampleSceneSingleton : MonoBehaviour
{
    public static ExampleSceneSingleton Instance => SingletonRegistry.Get<ExampleSceneSingleton>();

    public void Call() => Debug.Log("[ExampleSceneSingleton] Call");

    // ==========================
    // 🧩 Usage Example
    // ==========================
    // Anywhere in code:
    //ExampleSceneSingleton.Instance.Call();
}
