using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OSK
{
    using System;

    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public class GlobalSingletonAttribute : Attribute
    {
        // This attribute is used to mark classes that should be treated as global singletons.
    }

    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public class SceneSingletonAttribute : Attribute
    {
        // This attribute is used to mark classes that should be treated as singletons within a specific scene.
        public string[] AllowedScenes { get; }

        public SceneSingletonAttribute(params string[] allowedScenes)
        {
            AllowedScenes = allowedScenes;
        }
    }
}

