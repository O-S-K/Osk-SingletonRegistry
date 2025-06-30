using OSK;
using UnityEngine;


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
