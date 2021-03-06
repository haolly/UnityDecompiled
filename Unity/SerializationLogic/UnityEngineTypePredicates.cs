﻿namespace Unity.SerializationLogic
{
    using Mono.Cecil;
    using System;
    using System.Collections.Generic;
    using Unity.CecilTools.Extensions;

    public class UnityEngineTypePredicates
    {
        private const string AnimationCurve = "UnityEngine.AnimationCurve";
        protected const string Color32 = "UnityEngine.Color32";
        private const string Gradient = "UnityEngine.Gradient";
        private const string GUIStyle = "UnityEngine.GUIStyle";
        protected const string Matrix4x4 = "UnityEngine.Matrix4x4";
        public const string MonoBehaviour = "UnityEngine.MonoBehaviour";
        private const string RectOffset = "UnityEngine.RectOffset";
        public const string ScriptableObject = "UnityEngine.ScriptableObject";
        private static string[] serializableStructs;
        private const string SerializeFieldAttribute = "UnityEngine.SerializeField";
        private static readonly HashSet<string> TypesThatShouldHaveHadSerializableAttribute;
        protected const string UnityEngineObject = "UnityEngine.Object";

        static UnityEngineTypePredicates()
        {
            HashSet<string> set = new HashSet<string> { 
                "Vector3",
                "Vector2",
                "Vector4",
                "Rect",
                "Quaternion",
                "Matrix4x4",
                "Color",
                "Color32",
                "LayerMask",
                "Bounds"
            };
            TypesThatShouldHaveHadSerializableAttribute = set;
            serializableStructs = new string[] { "UnityEngine.AnimationCurve", "UnityEngine.Color32", "UnityEngine.Gradient", "UnityEngine.GUIStyle", "UnityEngine.RectOffset", "UnityEngine.Matrix4x4" };
        }

        public static bool IsColor32(TypeReference type) => 
            type.IsAssignableTo("UnityEngine.Color32");

        public static bool IsGradient(TypeReference type) => 
            type.IsAssignableTo("UnityEngine.Gradient");

        public static bool IsGUIStyle(TypeReference type) => 
            type.IsAssignableTo("UnityEngine.GUIStyle");

        public static bool IsMatrix4x4(TypeReference type) => 
            type.IsAssignableTo("UnityEngine.Matrix4x4");

        private static bool IsMonoBehaviour(TypeDefinition typeDefinition) => 
            typeDefinition.IsSubclassOf("UnityEngine.MonoBehaviour");

        public static bool IsMonoBehaviour(TypeReference type) => 
            IsMonoBehaviour(type.CheckedResolve());

        public static bool IsRectOffset(TypeReference type) => 
            type.IsAssignableTo("UnityEngine.RectOffset");

        private static bool IsScriptableObject(TypeDefinition temp) => 
            temp.IsSubclassOf("UnityEngine.ScriptableObject");

        public static bool IsScriptableObject(TypeReference type) => 
            IsScriptableObject(type.CheckedResolve());

        public static bool IsSerializableUnityStruct(TypeReference type)
        {
            foreach (string str in serializableStructs)
            {
                if (type.IsAssignableTo(str))
                {
                    return true;
                }
            }
            return false;
        }

        public static bool IsSerializeFieldAttribute(TypeReference attributeType) => 
            (attributeType.FullName == "UnityEngine.SerializeField");

        public static bool IsUnityEngineObject(TypeReference type)
        {
            if (type.IsArray)
            {
                return false;
            }
            TypeDefinition definition = type.Resolve();
            if (definition == null)
            {
                return false;
            }
            return ((type.FullName == "UnityEngine.Object") || definition.IsSubclassOf("UnityEngine.Object"));
        }

        public static bool IsUnityEngineValueType(TypeReference type) => 
            ((type.SafeNamespace() == "UnityEngine") && TypesThatShouldHaveHadSerializableAttribute.Contains(type.Name));

        public static bool ShouldHaveHadSerializableAttribute(TypeReference type) => 
            IsUnityEngineValueType(type);
    }
}

