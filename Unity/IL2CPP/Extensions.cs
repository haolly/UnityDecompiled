﻿namespace Unity.IL2CPP
{
    using Mono.Cecil;
    using mscorlib;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Diagnostics;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using System.Threading;
    using Unity.IL2CPP.Common;
    using Unity.IL2CPP.ILPreProcessor;
    using Unity.IL2CPP.IoC;
    using Unity.IL2CPP.IoCServices;
    using Unity.IL2CPP.Metadata;
    using Unity.IL2CPP.WindowsRuntime;

    public static class Extensions
    {
        [CompilerGenerated]
        private static Func<MethodDefinition, bool> <>f__am$cache0;
        [CompilerGenerated]
        private static Func<MethodDefinition, bool> <>f__am$cache1;
        [CompilerGenerated]
        private static Func<CustomAttribute, bool> <>f__am$cache10;
        [CompilerGenerated]
        private static Func<CustomAttribute, bool> <>f__am$cache11;
        [CompilerGenerated]
        private static Func<FieldDefinition, bool> <>f__am$cache2;
        [CompilerGenerated]
        private static Func<TypeDefinition, bool> <>f__am$cache3;
        [CompilerGenerated]
        private static Func<TypeDefinition, bool> <>f__am$cache4;
        [CompilerGenerated]
        private static Func<TypeDefinition, bool> <>f__am$cache5;
        [CompilerGenerated]
        private static Func<FieldDefinition, bool> <>f__am$cache6;
        [CompilerGenerated]
        private static Func<CustomAttribute, bool> <>f__am$cache7;
        [CompilerGenerated]
        private static Func<CustomAttribute, TypeReference> <>f__am$cache8;
        [CompilerGenerated]
        private static Func<CustomAttribute, TypeReference> <>f__am$cache9;
        [CompilerGenerated]
        private static Func<CustomAttribute, TypeReference> <>f__am$cacheA;
        [CompilerGenerated]
        private static Func<TypeReference, IEnumerable<MethodReference>> <>f__am$cacheB;
        [CompilerGenerated]
        private static Func<CustomAttribute, bool> <>f__am$cacheC;
        [CompilerGenerated]
        private static Func<CustomAttribute, bool> <>f__am$cacheD;
        [CompilerGenerated]
        private static Func<FieldDefinition, bool> <>f__am$cacheE;
        [CompilerGenerated]
        private static Func<AssemblyDefinition, string> <>f__am$cacheF;
        [CompilerGenerated]
        private static Func<MethodDefinition, bool> <>f__mg$cache0;
        [CompilerGenerated]
        private static Func<TypeReference, bool> <>f__mg$cache1;
        [CompilerGenerated]
        private static Func<MethodDefinition, bool> <>f__mg$cache2;
        [CompilerGenerated]
        private static Func<TypeReference, IEnumerable<TypeReference>> <>f__mg$cache3;
        [CompilerGenerated]
        private static Dictionary<string, int> <>f__switch$map1;
        [Inject]
        public static IAssemblyDependencies AssemblyDependencies;
        [Inject]
        public static IGuidProvider GuidProvider;
        [Inject]
        public static INamingService Naming;
        [Inject]
        public static ITypeProviderService TypeProvider;
        [Inject]
        public static IWindowsRuntimeProjections WindowsRuntimeProjections;

        [CompilerGenerated]
        private static T <Chunk`1>m__14<T>(ChunkItem<T> x) => 
            x.Value;

        [CompilerGenerated]
        private static ChunkItem<T> <Chunk`1>m__C<T>(T value, int index) => 
            new ChunkItem<T> { 
                Index = index,
                Value = value
            };

        [CompilerGenerated]
        private static List<T> <Chunk`1>m__D<T>(IGrouping<int, ChunkItem<T>> g) => 
            (from x in g select x.Value).ToList<T>();

        private static void AddInterfacesRecursive(TypeReference type, HashSet<TypeReference> interfaces)
        {
            if (!type.IsArray)
            {
                Unity.IL2CPP.ILPreProcessor.TypeResolver resolver = Unity.IL2CPP.ILPreProcessor.TypeResolver.For(type);
                foreach (InterfaceImplementation implementation in type.Resolve().Interfaces)
                {
                    TypeReference item = resolver.Resolve(implementation.InterfaceType);
                    if (interfaces.Add(item))
                    {
                        AddInterfacesRecursive(item, interfaces);
                    }
                }
            }
        }

        private static bool AreGenericArgumentsValidForWindowsRuntimeType(GenericInstanceType genericInstance)
        {
            foreach (TypeReference reference in genericInstance.GenericArguments)
            {
                if (!reference.IsValidForWindowsRuntimeType())
                {
                    return false;
                }
            }
            return true;
        }

        public static bool CanBoxToWindowsRuntime(this TypeReference type)
        {
            if (TypeProvider.IReferenceType == null)
            {
                return false;
            }
            if (type.MetadataType == MetadataType.Object)
            {
                return false;
            }
            if (type.IsWindowsRuntimePrimitiveType())
            {
                return true;
            }
            TypeReference reference = WindowsRuntimeProjections.ProjectToWindowsRuntime(type);
            if (!reference.IsValueType)
            {
                return false;
            }
            return ((reference != type) || type.Resolve().IsWindowsRuntime);
        }

        public static List<List<T>> Chunk<T>(this IEnumerable<T> foo, int size)
        {
            <Chunk>c__AnonStorey8<T> storey = new <Chunk>c__AnonStorey8<T> {
                size = size
            };
            return foo.Select<T, ChunkItem<T>>(new Func<T, int, ChunkItem<T>>(Extensions.<Chunk`1>m__C<T>)).GroupBy<ChunkItem<T>, int>(new Func<ChunkItem<T>, int>(storey.<>m__0)).Select<IGrouping<int, ChunkItem<T>>, List<T>>(new Func<IGrouping<int, ChunkItem<T>>, List<T>>(Extensions.<Chunk`1>m__D<T>)).ToList<List<T>>();
        }

        public static bool ContainsGenericParameters(this MethodReference method)
        {
            if (method.DeclaringType.ContainsGenericParameters())
            {
                return true;
            }
            GenericInstanceMethod method2 = method as GenericInstanceMethod;
            if (method2 != null)
            {
                foreach (TypeReference reference in method2.GenericArguments)
                {
                    if (reference.ContainsGenericParameters())
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public static bool ContainsGenericParameters(this TypeReference typeReference)
        {
            if (typeReference is GenericParameter)
            {
                return true;
            }
            ArrayType type = typeReference as ArrayType;
            if (type != null)
            {
                return type.ElementType.ContainsGenericParameters();
            }
            PointerType type2 = typeReference as PointerType;
            if (type2 != null)
            {
                return type2.ElementType.ContainsGenericParameters();
            }
            ByReferenceType type3 = typeReference as ByReferenceType;
            if (type3 != null)
            {
                return type3.ElementType.ContainsGenericParameters();
            }
            SentinelType type4 = typeReference as SentinelType;
            if (type4 != null)
            {
                return type4.ElementType.ContainsGenericParameters();
            }
            PinnedType type5 = typeReference as PinnedType;
            if (type5 != null)
            {
                return type5.ElementType.ContainsGenericParameters();
            }
            RequiredModifierType type6 = typeReference as RequiredModifierType;
            if (type6 != null)
            {
                return type6.ElementType.ContainsGenericParameters();
            }
            GenericInstanceType type7 = typeReference as GenericInstanceType;
            if (type7 != null)
            {
                if (<>f__mg$cache1 == null)
                {
                    <>f__mg$cache1 = new Func<TypeReference, bool>(Extensions.ContainsGenericParameters);
                }
                return type7.GenericArguments.Any<TypeReference>(<>f__mg$cache1);
            }
            if (typeReference is TypeSpecification)
            {
                throw new NotSupportedException();
            }
            return false;
        }

        public static bool DerivesFrom(this TypeReference type, TypeReference potentialBaseType, bool checkInterfaces = true)
        {
            while (type != null)
            {
                if (Unity.IL2CPP.Common.TypeReferenceEqualityComparer.AreEqual(type, potentialBaseType, TypeComparisonMode.Exact))
                {
                    return true;
                }
                if (checkInterfaces)
                {
                    foreach (TypeReference reference in type.GetInterfaces())
                    {
                        if (Unity.IL2CPP.Common.TypeReferenceEqualityComparer.AreEqual(reference, potentialBaseType, TypeComparisonMode.Exact))
                        {
                            return true;
                        }
                    }
                }
                type = type.GetBaseType();
            }
            return false;
        }

        public static bool DerivesFromObject(this TypeReference typeReference)
        {
            TypeReference baseType = typeReference.GetBaseType();
            return (baseType?.MetadataType == MetadataType.Object);
        }

        public static TypeReference ExtractDefaultInterface(this TypeDefinition type)
        {
            if (!type.IsWindowsRuntime)
            {
                throw new ArgumentException($"Extracting default interface is only valid for Windows Runtime types. {type.FullName} is not a Windows Runtime type.");
            }
            foreach (InterfaceImplementation implementation in type.Interfaces)
            {
                foreach (CustomAttribute attribute in implementation.CustomAttributes)
                {
                    if (attribute.AttributeType.FullName == "Windows.Foundation.Metadata.DefaultAttribute")
                    {
                        return implementation.InterfaceType;
                    }
                }
            }
            throw new InvalidProgramException($"Windows Runtime class {type} has no default interface!");
        }

        public static IEnumerable<TypeReference> GetActivationFactoryTypes(this TypeReference type)
        {
            TypeDefinition definition = type.Resolve();
            if (!definition.IsWindowsRuntime || definition.IsValueType)
            {
                return Enumerable.Empty<TypeReference>();
            }
            if (<>f__am$cache9 == null)
            {
                <>f__am$cache9 = delegate (CustomAttribute attribute) {
                    CustomAttributeArgument argument = attribute.ConstructorArguments[0];
                    if (argument.Type.IsSystemType())
                    {
                        return (TypeReference) argument.Value;
                    }
                    return TypeProvider.IActivationFactoryTypeReference;
                };
            }
            return definition.GetTypesFromSpecificAttribute("Windows.Foundation.Metadata.ActivatableAttribute", <>f__am$cache9);
        }

        [DebuggerHidden]
        public static IEnumerable<GenericInstanceType> GetAllAssignableWindowsRuntimeTypes(this GenericInstanceType clrType) => 
            new <GetAllAssignableWindowsRuntimeTypes>c__Iterator2 { 
                clrType = clrType,
                $PC = -2
            };

        public static IEnumerable<TypeReference> GetAllFactoryTypes(this TypeReference type)
        {
            TypeDefinition definition = type.Resolve();
            if (!definition.IsWindowsRuntime || definition.IsValueType)
            {
                return Enumerable.Empty<TypeReference>();
            }
            return definition.GetActivationFactoryTypes().Concat<TypeReference>(definition.GetComposableFactoryTypes()).Concat<TypeReference>(definition.GetStaticFactoryTypes()).Distinct<TypeReference>(new Unity.IL2CPP.Common.TypeReferenceEqualityComparer());
        }

        public static TypeReference GetBaseType(this TypeReference typeReference)
        {
            if (typeReference is TypeSpecification)
            {
                if (typeReference.IsArray)
                {
                    return TypeProvider.SystemArray;
                }
                if ((typeReference.IsGenericParameter || typeReference.IsByReference) || typeReference.IsPointer)
                {
                    return null;
                }
                SentinelType type = typeReference as SentinelType;
                if (type != null)
                {
                    return type.ElementType.GetBaseType();
                }
                PinnedType type2 = typeReference as PinnedType;
                if (type2 != null)
                {
                    return type2.ElementType.GetBaseType();
                }
                RequiredModifierType type3 = typeReference as RequiredModifierType;
                if (type3 != null)
                {
                    return type3.ElementType.GetBaseType();
                }
            }
            return Unity.IL2CPP.ILPreProcessor.TypeResolver.For(typeReference).Resolve(typeReference.Resolve().BaseType);
        }

        public static IEnumerable<TypeReference> GetComposableFactoryTypes(this TypeReference type)
        {
            TypeDefinition definition = type.Resolve();
            if (!definition.IsWindowsRuntime || definition.IsValueType)
            {
                return Enumerable.Empty<TypeReference>();
            }
            if (<>f__am$cacheA == null)
            {
                <>f__am$cacheA = attribute => (TypeReference) attribute.ConstructorArguments[0].Value;
            }
            return definition.GetTypesFromSpecificAttribute("Windows.Foundation.Metadata.ComposableAttribute", <>f__am$cacheA);
        }

        public static IEnumerable<CustomAttribute> GetConstructibleCustomAttributes(this ICustomAttributeProvider customAttributeProvider)
        {
            if (<>f__am$cache11 == null)
            {
                <>f__am$cache11 = delegate (CustomAttribute ca) {
                    TypeDefinition definition = ca.AttributeType.Resolve();
                    return (definition != null) && !definition.IsWindowsRuntime;
                };
            }
            return customAttributeProvider.CustomAttributes.Where<CustomAttribute>(<>f__am$cache11);
        }

        public static Guid GetGuid(this TypeReference type) => 
            GuidProvider.GuidFor(type);

        public static ReadOnlyCollection<TypeReference> GetInterfaces(this TypeReference type)
        {
            HashSet<TypeReference> interfaces = new HashSet<TypeReference>(new Unity.IL2CPP.Common.TypeReferenceEqualityComparer());
            AddInterfacesRecursive(type, interfaces);
            return interfaces.ToList<TypeReference>().AsReadOnly();
        }

        public static ReadOnlyCollection<MethodReference> GetMethods(this TypeReference type)
        {
            if (<>f__am$cache1 == null)
            {
                <>f__am$cache1 = m => true;
            }
            return type.GetMethods(<>f__am$cache1);
        }

        private static ReadOnlyCollection<MethodReference> GetMethods(this TypeReference type, Func<MethodDefinition, bool> filter)
        {
            Unity.IL2CPP.ILPreProcessor.TypeResolver resolver = Unity.IL2CPP.ILPreProcessor.TypeResolver.For(type);
            List<MethodReference> list = new List<MethodReference>();
            foreach (MethodDefinition definition in type.Resolve().Methods.Where<MethodDefinition>(filter))
            {
                list.Add(resolver.Resolve(definition));
            }
            return list.AsReadOnly();
        }

        public static TypeReference GetNonPinnedAndNonByReferenceType(this TypeReference type)
        {
            type = Naming.RemoveModifiers(type);
            TypeReference elementType = type;
            ByReferenceType type2 = type as ByReferenceType;
            if (type2 != null)
            {
                elementType = type2.ElementType;
            }
            PinnedType type3 = type as PinnedType;
            if (type3 != null)
            {
                elementType = type3.ElementType;
            }
            return elementType;
        }

        public static MethodReference GetOverridenInterfaceMethod(this MethodReference overridingMethod, IEnumerable<TypeReference> candidateInterfaces)
        {
            <GetOverridenInterfaceMethod>c__AnonStorey7 storey = new <GetOverridenInterfaceMethod>c__AnonStorey7 {
                overridingMethod = overridingMethod
            };
            MethodDefinition definition = storey.overridingMethod.Resolve();
            if (definition.Overrides.Count > 0)
            {
                if (definition.Overrides.Count != 1)
                {
                    throw new InvalidOperationException($"Cannot choose overriden method for '{storey.overridingMethod.FullName}'");
                }
                return Unity.IL2CPP.ILPreProcessor.TypeResolver.For(storey.overridingMethod.DeclaringType, storey.overridingMethod).Resolve(definition.Overrides[0]);
            }
            if (<>f__am$cacheB == null)
            {
                <>f__am$cacheB = iface => iface.GetMethods();
            }
            return candidateInterfaces.SelectMany<TypeReference, MethodReference>(<>f__am$cacheB).FirstOrDefault<MethodReference>(new Func<MethodReference, bool>(storey.<>m__0));
        }

        public static IEnumerable<TypeReference> GetStaticFactoryTypes(this TypeReference type)
        {
            TypeDefinition definition = type.Resolve();
            if (!definition.IsWindowsRuntime || definition.IsValueType)
            {
                return Enumerable.Empty<TypeReference>();
            }
            if (<>f__am$cache8 == null)
            {
                <>f__am$cache8 = attribute => (TypeReference) attribute.ConstructorArguments[0].Value;
            }
            return definition.GetTypesFromSpecificAttribute("Windows.Foundation.Metadata.StaticAttribute", <>f__am$cache8);
        }

        [DebuggerHidden]
        private static IEnumerable<TypeReference[]> GetTypeCombinations(TypeReference[][] types, int level = 0) => 
            new <GetTypeCombinations>c__Iterator4 { 
                types = types,
                level = level,
                $PC = -2
            };

        [DebuggerHidden]
        public static IEnumerable<TypeDefinition> GetTypeHierarchy(this TypeDefinition type) => 
            new <GetTypeHierarchy>c__Iterator0 { 
                type = type,
                <$>type = type,
                $PC = -2
            };

        private static IEnumerable<TypeReference> GetTypesFromSpecificAttribute(this TypeDefinition type, string attributeName, Func<CustomAttribute, TypeReference> customAttributeSelector)
        {
            <GetTypesFromSpecificAttribute>c__AnonStorey6 storey = new <GetTypesFromSpecificAttribute>c__AnonStorey6 {
                attributeName = attributeName
            };
            return type.CustomAttributes.Where<CustomAttribute>(new Func<CustomAttribute, bool>(storey.<>m__0)).Select<CustomAttribute, TypeReference>(customAttributeSelector);
        }

        public static TypeReference GetUnderlyingEnumType(this TypeReference type)
        {
            TypeDefinition definition = type.Resolve();
            if (definition == null)
            {
                throw new Exception("Failed to resolve type reference");
            }
            if (!definition.IsEnum)
            {
                throw new ArgumentException("Attempting to retrieve underlying enum type for non-enum type.", "type");
            }
            if (<>f__am$cache2 == null)
            {
                <>f__am$cache2 = f => !f.IsStatic && (f.Name == "value__");
            }
            return definition.Fields.Single<FieldDefinition>(<>f__am$cache2).FieldType;
        }

        public static ReadOnlyCollection<MethodReference> GetVirtualMethods(this TypeReference type)
        {
            if (<>f__am$cache0 == null)
            {
                <>f__am$cache0 = m => m.IsVirtual && !m.IsStatic;
            }
            return type.GetMethods(<>f__am$cache0);
        }

        public static IEnumerable<TypeReference> GetWindowsRuntimeCovariantTypes(this TypeReference type) => 
            GetWindowsRuntimeCovariantTypesWithDuplicates(type).Distinct<TypeReference>(new Unity.IL2CPP.Common.TypeReferenceEqualityComparer());

        [DebuggerHidden]
        private static IEnumerable<TypeReference> GetWindowsRuntimeCovariantTypesWithDuplicates(TypeReference type) => 
            new <GetWindowsRuntimeCovariantTypesWithDuplicates>c__Iterator3 { 
                type = type,
                $PC = -2
            };

        public static string GetWindowsRuntimePrimitiveName(this TypeReference type)
        {
            switch (type.MetadataType)
            {
                case MetadataType.Boolean:
                    return "Boolean";

                case MetadataType.Char:
                    return "Char16";

                case MetadataType.Byte:
                    return "UInt8";

                case MetadataType.Int16:
                    return "Int16";

                case MetadataType.UInt16:
                    return "UInt16";

                case MetadataType.Int32:
                    return "Int32";

                case MetadataType.UInt32:
                    return "UInt32";

                case MetadataType.Int64:
                    return "Int64";

                case MetadataType.UInt64:
                    return "UInt64";

                case MetadataType.Single:
                    return "Single";

                case MetadataType.Double:
                    return "Double";

                case MetadataType.String:
                    return "String";

                case MetadataType.ValueType:
                    if (type.FullName != "System.Guid")
                    {
                        break;
                    }
                    return "Guid";

                case MetadataType.Object:
                    return "Object";
            }
            return null;
        }

        public static bool HasActivationFactories(this TypeReference type)
        {
            TypeDefinition definition = type.Resolve();
            if (!definition.IsWindowsRuntime || definition.IsValueType)
            {
                return false;
            }
            if (<>f__am$cache7 == null)
            {
                <>f__am$cache7 = ca => ((ca.AttributeType.FullName == "Windows.Foundation.Metadata.ActivatableAttribute") || (ca.AttributeType.FullName == "Windows.Foundation.Metadata.StaticAttribute")) || (ca.AttributeType.FullName == "Windows.Foundation.Metadata.ComposableAttribute");
            }
            return definition.CustomAttributes.Any<CustomAttribute>(<>f__am$cache7);
        }

        public static bool HasCLSID(this TypeDefinition type)
        {
            if (!type.IsInterface)
            {
            }
            return ((<>f__am$cache10 == null) && type.CustomAttributes.Any<CustomAttribute>(<>f__am$cache10));
        }

        public static bool HasCLSID(this TypeReference type)
        {
            if ((type is TypeSpecification) || (type is GenericParameter))
            {
                return false;
            }
            return type.Resolve().HasCLSID();
        }

        public static bool HasFinalizer(this TypeDefinition type)
        {
            if (type.IsInterface)
            {
                return false;
            }
            if (type.MetadataType == MetadataType.Object)
            {
                return false;
            }
            if (type.BaseType == null)
            {
                return false;
            }
            if (!type.BaseType.Resolve().HasFinalizer())
            {
            }
            return ((<>f__mg$cache0 != null) || (type.Methods.SingleOrDefault<MethodDefinition>(<>f__mg$cache0) != null));
        }

        public static bool HasIID(this TypeReference type) => 
            (type.IsComOrWindowsRuntimeInterface() && !type.HasGenericParameters);

        public static bool HasStaticConstructor(this TypeReference typeReference)
        {
            TypeDefinition definition = typeReference.Resolve();
            if (definition == null)
            {
            }
            return ((<>f__mg$cache2 == null) && (definition.Methods.SingleOrDefault<MethodDefinition>(<>f__mg$cache2) != null));
        }

        public static bool HasStaticFields(this TypeReference typeReference)
        {
            if (typeReference.IsArray)
            {
                return false;
            }
            if (<>f__am$cache6 == null)
            {
                <>f__am$cache6 = f => f.IsStatic;
            }
            return typeReference.Resolve().Fields.Any<FieldDefinition>(<>f__am$cache6);
        }

        public static IEnumerable<TypeReference> ImplementedComOrWindowsRuntimeInterfaces(this TypeReference type)
        {
            List<TypeReference> list = new List<TypeReference>();
            Unity.IL2CPP.ILPreProcessor.TypeResolver resolver = Unity.IL2CPP.ILPreProcessor.TypeResolver.For(type);
            TypeDefinition definition = type.Resolve();
            foreach (InterfaceImplementation implementation in definition.Interfaces)
            {
                TypeReference reference = resolver.Resolve(implementation.InterfaceType);
                if (reference.IsComOrWindowsRuntimeInterface())
                {
                    list.Add(reference);
                }
            }
            return list;
        }

        [DebuggerHidden]
        public static IEnumerable<TypeReference> ImplementedWindowsRuntimeProjectedInterfaces(this TypeReference type) => 
            new <ImplementedWindowsRuntimeProjectedInterfaces>c__Iterator1 { 
                type = type,
                $PC = -2
            };

        public static bool IsAttribute(this TypeReference type)
        {
            if (type.FullName == "System.Attribute")
            {
                return true;
            }
            TypeDefinition definition = type.Resolve();
            return (((definition != null) && (definition.BaseType != null)) && definition.BaseType.IsAttribute());
        }

        public static bool IsComInterface(this TypeReference type)
        {
            if (type.IsArray)
            {
                return false;
            }
            if (type.IsGenericParameter)
            {
                return false;
            }
            TypeDefinition definition = type.Resolve();
            return ((((definition != null) && definition.IsInterface) && definition.IsImport) && !definition.IsWindowsRuntimeProjection());
        }

        public static bool IsComOrWindowsRuntimeInterface(this MethodDefinition method)
        {
            if (!method.IsComOrWindowsRuntimeMethod())
            {
                return false;
            }
            return method.DeclaringType.IsInterface;
        }

        public static bool IsComOrWindowsRuntimeInterface(this MethodReference method) => 
            method.Resolve().IsComOrWindowsRuntimeInterface();

        public static bool IsComOrWindowsRuntimeInterface(this TypeReference type)
        {
            if (<>f__am$cache5 == null)
            {
                <>f__am$cache5 = typeDef => typeDef.IsInterface && typeDef.IsComOrWindowsRuntimeType();
            }
            return IsComOrWindowsRuntimeType(type, <>f__am$cache5);
        }

        public static bool IsComOrWindowsRuntimeMethod(this MethodDefinition method)
        {
            TypeDefinition declaringType = method.DeclaringType;
            if (declaringType.IsWindowsRuntime)
            {
                return true;
            }
            if (declaringType.IsIl2CppComObject() || declaringType.IsIl2CppComDelegate())
            {
                return true;
            }
            if (!declaringType.IsImport)
            {
                return false;
            }
            return ((method.IsInternalCall || method.IsFinalizerMethod()) || declaringType.IsInterface);
        }

        public static bool IsComOrWindowsRuntimeType(this TypeDefinition type)
        {
            if (type.IsValueType)
            {
                return false;
            }
            if (type.IsDelegate())
            {
                return false;
            }
            return ((type.IsIl2CppComObject() || type.IsIl2CppComDelegate()) || (type.IsImport || type.IsWindowsRuntime));
        }

        private static bool IsComOrWindowsRuntimeType(TypeReference type, Func<TypeDefinition, bool> predicate)
        {
            if (type.IsArray)
            {
                return false;
            }
            if (type.IsGenericParameter)
            {
                return false;
            }
            TypeDefinition arg = type.Resolve();
            if (arg == null)
            {
                return false;
            }
            if (!predicate(arg))
            {
                return false;
            }
            GenericInstanceType genericInstance = type as GenericInstanceType;
            if (genericInstance != null)
            {
                return AreGenericArgumentsValidForWindowsRuntimeType(genericInstance);
            }
            return true;
        }

        public static bool IsDefinedInMscorlib(this MemberReference memberReference) => 
            (memberReference.Module.Assembly.Name.Name == "mscorlib");

        public static bool IsDefinedInUnityEngine(this MemberReference memberReference) => 
            memberReference.Module.Assembly.Name.Name.Contains("UnityEngine");

        public static bool IsDelegate(this TypeDefinition type) => 
            ((type.BaseType != null) && (type.BaseType.FullName == "System.MulticastDelegate"));

        public static bool IsEnum(this TypeReference type)
        {
            if (type.IsArray)
            {
                return false;
            }
            if (type.IsGenericParameter)
            {
                return false;
            }
            TypeDefinition definition = type.Resolve();
            return definition?.IsEnum;
        }

        public static bool IsFinalizerMethod(this MethodDefinition method) => 
            ((((method.Name == "Finalize") && (method.ReturnType.MetadataType == MetadataType.Void)) && !method.HasParameters) && (((ushort) (method.Attributes & (MethodAttributes.CompilerControlled | MethodAttributes.Family))) != 0));

        public static bool IsGenericParameter(this TypeReference typeReference)
        {
            if (typeReference is ArrayType)
            {
                return false;
            }
            if (typeReference is PointerType)
            {
                return false;
            }
            if (typeReference is ByReferenceType)
            {
                return false;
            }
            return typeReference.GetElementType().IsGenericParameter;
        }

        public static bool IsIActivationFactory(this TypeReference typeReference) => 
            Unity.IL2CPP.Common.TypeReferenceEqualityComparer.AreEqual(typeReference, TypeProvider.IActivationFactoryTypeReference, TypeComparisonMode.Exact);

        public static bool IsIl2CppComDelegate(this TypeReference typeReference) => 
            Unity.IL2CPP.Common.TypeReferenceEqualityComparer.AreEqual(typeReference, TypeProvider.Il2CppComDelegateTypeReference, TypeComparisonMode.Exact);

        public static bool IsIl2CppComObject(this TypeReference typeReference) => 
            Unity.IL2CPP.Common.TypeReferenceEqualityComparer.AreEqual(typeReference, TypeProvider.Il2CppComObjectTypeReference, TypeComparisonMode.Exact);

        public static bool IsIntegralPointerType(this TypeReference typeReference) => 
            ((typeReference.MetadataType == MetadataType.IntPtr) || (typeReference.MetadataType == MetadataType.UIntPtr));

        public static bool IsIntegralType(this TypeReference type) => 
            (type.IsSignedIntegralType() || type.IsUnsignedIntegralType());

        public static bool IsInterface(this TypeReference type)
        {
            if (type.IsArray)
            {
                return false;
            }
            if (type.IsGenericParameter)
            {
                return false;
            }
            TypeDefinition definition = type.Resolve();
            return ((definition != null) && definition.IsInterface);
        }

        public static bool IsNativeIntegralType(this TypeReference typeReference) => 
            (Unity.IL2CPP.Common.TypeReferenceEqualityComparer.AreEqual(typeReference, TypeProvider.NativeIntTypeReference, TypeComparisonMode.Exact) || Unity.IL2CPP.Common.TypeReferenceEqualityComparer.AreEqual(typeReference, TypeProvider.NativeUIntTypeReference, TypeComparisonMode.Exact));

        public static bool IsNormalStatic(this FieldReference field)
        {
            FieldDefinition definition = field.Resolve();
            if (definition.IsLiteral)
            {
                return false;
            }
            if (!definition.IsStatic)
            {
                return false;
            }
            if (!definition.HasCustomAttributes)
            {
                return true;
            }
            if (<>f__am$cacheC == null)
            {
                <>f__am$cacheC = ca => ca.AttributeType.Name != "ThreadStaticAttribute";
            }
            return definition.CustomAttributes.All<CustomAttribute>(<>f__am$cacheC);
        }

        public static bool IsNullable(this TypeReference type)
        {
            if (type.IsArray)
            {
                return false;
            }
            if (type.IsGenericParameter)
            {
                return false;
            }
            GenericInstanceType type2 = type as GenericInstanceType;
            return (type2?.ElementType.FullName == "System.Nullable`1");
        }

        public static bool IsPrimitiveCppType(this string typeName)
        {
            if (typeName != null)
            {
                int num;
                if (<>f__switch$map1 == null)
                {
                    Dictionary<string, int> dictionary = new Dictionary<string, int>(14) {
                        { 
                            "bool",
                            0
                        },
                        { 
                            "char",
                            0
                        },
                        { 
                            "wchar_t",
                            0
                        },
                        { 
                            "size_t",
                            0
                        },
                        { 
                            "int8_t",
                            0
                        },
                        { 
                            "int16_t",
                            0
                        },
                        { 
                            "int32_t",
                            0
                        },
                        { 
                            "int64_t",
                            0
                        },
                        { 
                            "uint8_t",
                            0
                        },
                        { 
                            "uint16_t",
                            0
                        },
                        { 
                            "uint32_t",
                            0
                        },
                        { 
                            "uint64_t",
                            0
                        },
                        { 
                            "double",
                            0
                        },
                        { 
                            "float",
                            0
                        }
                    };
                    <>f__switch$map1 = dictionary;
                }
                if (<>f__switch$map1.TryGetValue(typeName, out num) && (num == 0))
                {
                    return true;
                }
            }
            return false;
        }

        public static bool IsPrimitiveType(this MetadataType type)
        {
            switch (type)
            {
                case MetadataType.Boolean:
                case MetadataType.Char:
                case MetadataType.SByte:
                case MetadataType.Byte:
                case MetadataType.Int16:
                case MetadataType.UInt16:
                case MetadataType.Int32:
                case MetadataType.UInt32:
                case MetadataType.Int64:
                case MetadataType.UInt64:
                case MetadataType.Single:
                case MetadataType.Double:
                    return true;
            }
            return false;
        }

        public static bool IsSameType(this TypeReference a, TypeReference b) => 
            Unity.IL2CPP.Common.TypeReferenceEqualityComparer.AreEqual(a, b, TypeComparisonMode.Exact);

        public static bool IsSignedIntegralType(this TypeReference type) => 
            ((((type.MetadataType == MetadataType.SByte) || (type.MetadataType == MetadataType.Int16)) || (type.MetadataType == MetadataType.Int32)) || (type.MetadataType == MetadataType.Int64));

        public static bool IsSpecialSystemBaseType(this TypeReference typeReference) => 
            (((typeReference.FullName == "System.Object") || (typeReference.FullName == "System.ValueType")) || (typeReference.FullName == "System.Enum"));

        public static bool IsStaticConstructor(this MethodReference methodReference)
        {
            MethodDefinition definition = methodReference.Resolve();
            return ((definition?.IsConstructor && definition.IsStatic) && (definition.Parameters.Count == 0));
        }

        public static bool IsStripped(this MethodReference method) => 
            method.Name.StartsWith("$__Stripped");

        public static bool IsStructWithNoInstanceFields(this TypeReference typeReference)
        {
            System.Boolean ReflectorVariable0;
            if (!typeReference.IsValueType() || typeReference.IsEnum())
            {
                return false;
            }
            TypeDefinition definition = typeReference.Resolve();
            if (definition != null)
            {
                if (definition.HasFields)
                {
                }
                ReflectorVariable0 = true;
            }
            else
            {
                ReflectorVariable0 = false;
            }
            return (ReflectorVariable0 ? ((<>f__am$cacheE != null) || definition.Fields.All<FieldDefinition>(<>f__am$cacheE)) : false);
        }

        public static bool IsSystemArray(this TypeReference typeReference) => 
            ((typeReference.FullName == "System.Array") && (typeReference.Resolve().Module.Name == "mscorlib.dll"));

        public static bool IsSystemObject(this TypeReference typeReference) => 
            (typeReference.MetadataType == MetadataType.Object);

        public static bool IsSystemType(this TypeReference typeReference) => 
            ((typeReference.FullName == "System.Type") && (typeReference.Resolve().Module.Name == "mscorlib.dll"));

        public static bool IsThreadStatic(this FieldReference field)
        {
            FieldDefinition definition = field.Resolve();
            if (definition.IsStatic && definition.HasCustomAttributes)
            {
            }
            return ((<>f__am$cacheD == null) && definition.CustomAttributes.Any<CustomAttribute>(<>f__am$cacheD));
        }

        public static bool IsUnsignedIntegralType(this TypeReference type) => 
            ((((type.MetadataType == MetadataType.Byte) || (type.MetadataType == MetadataType.UInt16)) || (type.MetadataType == MetadataType.UInt32)) || (type.MetadataType == MetadataType.UInt64));

        public static bool IsValidForWindowsRuntimeType(this TypeReference type)
        {
            if (type.IsWindowsRuntimePrimitiveType())
            {
                return true;
            }
            if (type.IsAttribute())
            {
                return false;
            }
            if (type.IsGenericInstance)
            {
                GenericInstanceType type2 = (GenericInstanceType) WindowsRuntimeProjections.ProjectToWindowsRuntime(type);
                if (<>f__am$cache3 == null)
                {
                    <>f__am$cache3 = typeDef => typeDef.IsWindowsRuntime && (typeDef.IsInterface || typeDef.IsDelegate());
                }
                if (!IsComOrWindowsRuntimeType(type2, <>f__am$cache3))
                {
                    return false;
                }
                return AreGenericArgumentsValidForWindowsRuntimeType(type2);
            }
            if (type.IsGenericParameter || (type is TypeSpecification))
            {
                return false;
            }
            return WindowsRuntimeProjections.ProjectToWindowsRuntime(type.Resolve()).IsWindowsRuntime;
        }

        public static bool IsValueType(this TypeReference typeReference)
        {
            if (typeReference is ArrayType)
            {
                return false;
            }
            if (typeReference is PointerType)
            {
                return false;
            }
            if (typeReference is ByReferenceType)
            {
                return false;
            }
            if (typeReference is GenericParameter)
            {
                return false;
            }
            PinnedType type = typeReference as PinnedType;
            if (type != null)
            {
                return type.ElementType.IsValueType();
            }
            RequiredModifierType type2 = typeReference as RequiredModifierType;
            if (type2 != null)
            {
                return type2.ElementType.IsValueType();
            }
            OptionalModifierType type3 = typeReference as OptionalModifierType;
            if (type3 != null)
            {
                return type3.ElementType.IsValueType();
            }
            return typeReference.Resolve().IsValueType;
        }

        public static bool IsVoid(this TypeReference type) => 
            (type.MetadataType == MetadataType.Void);

        public static bool IsVolatile(this FieldReference fieldReference) => 
            (((fieldReference != null) && fieldReference.FieldType.IsRequiredModifier) && ((RequiredModifierType) fieldReference.FieldType).ModifierType.Name.Contains("IsVolatile"));

        public static bool IsWindowsRuntimeDelegate(this TypeReference type)
        {
            if (<>f__am$cache4 == null)
            {
                <>f__am$cache4 = typeDef => typeDef.IsDelegate() && typeDef.IsWindowsRuntime;
            }
            return IsComOrWindowsRuntimeType(type, <>f__am$cache4);
        }

        public static bool IsWindowsRuntimePrimitiveType(this TypeReference type)
        {
            switch (type.MetadataType)
            {
                case MetadataType.Boolean:
                case MetadataType.Char:
                case MetadataType.Byte:
                case MetadataType.Int16:
                case MetadataType.UInt16:
                case MetadataType.Int32:
                case MetadataType.UInt32:
                case MetadataType.Int64:
                case MetadataType.UInt64:
                case MetadataType.Single:
                case MetadataType.Double:
                case MetadataType.String:
                case MetadataType.Object:
                    return true;

                case MetadataType.ValueType:
                    return (type.FullName == "System.Guid");
            }
            return false;
        }

        public static bool IsWindowsRuntimeProjection(this TypeDefinition type) => 
            type.IsWindowsRuntimeProjection;

        public static bool IsWindowsRuntimeProjection(this TypeReference type) => 
            type.GetElementType().IsWindowsRuntimeProjection;

        public static bool NeedsComCallableWrapper(this TypeReference type)
        {
            if (type.IsArray)
            {
                return (((((ArrayType) type).Rank == 1) && ArrayCCWWriter.IsArrayCCWSupported()) && type.GetWindowsRuntimeCovariantTypes().Any<TypeReference>());
            }
            TypeDefinition definition = type.Resolve();
            if (definition.CanBoxToWindowsRuntime())
            {
                return true;
            }
            if (!definition.IsInterface && !definition.IsComOrWindowsRuntimeType())
            {
                if (type.ImplementedComOrWindowsRuntimeInterfaces().Any<TypeReference>())
                {
                    return true;
                }
                if (type.ImplementedWindowsRuntimeProjectedInterfaces().Any<TypeReference>())
                {
                    return true;
                }
                while (definition.BaseType != null)
                {
                    type = Unity.IL2CPP.ILPreProcessor.TypeResolver.For(type).Resolve(definition.BaseType);
                    definition = type.Resolve();
                    if (type.ImplementedComOrWindowsRuntimeInterfaces().Any<TypeReference>() || definition.IsComOrWindowsRuntimeType())
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public static bool References(this AssemblyDefinition assemblyDoingTheReferencing, AssemblyDefinition assemblyBeingReference)
        {
            if (<>f__am$cacheF == null)
            {
                <>f__am$cacheF = eachAssemblyReference => eachAssemblyReference.Name.Name;
            }
            return AssemblyDependencies.GetReferencedAssembliesFor(assemblyDoingTheReferencing).Select<AssemblyDefinition, string>(<>f__am$cacheF).Contains<string>(assemblyBeingReference.Name.Name);
        }

        public static bool ShouldProcessAsInternalCall(this MethodReference methodReference)
        {
            MethodDefinition definition = methodReference.Resolve();
            return ((definition != null) && (definition.IsInternalCall && !definition.HasGenericParameters));
        }

        public static bool StoresNonFieldsInStaticFields(this TypeReference type) => 
            type.HasActivationFactories();

        public static string ToInitializer(this Guid guid)
        {
            byte[] buffer = guid.ToByteArray();
            uint num = BitConverter.ToUInt32(buffer, 0);
            ushort num2 = BitConverter.ToUInt16(buffer, 4);
            ushort num3 = BitConverter.ToUInt16(buffer, 6);
            return ('{' + $" 0x{num:x}, 0x{num2:x}, 0x{num3:x}, 0x{buffer[8]:x}, 0x{buffer[9]:x}, 0x{buffer[10]:x}, 0x{buffer[11]:x}, 0x{buffer[12]:x}, 0x{buffer[13]:x}, 0x{buffer[14]:x}, 0x{buffer[15]:x} " + '}');
        }

        [CompilerGenerated]
        private sealed class <Chunk>c__AnonStorey8<T>
        {
            internal int size;

            internal int <>m__0(Extensions.ChunkItem<T> x) => 
                (x.Index / this.size);
        }

        [CompilerGenerated]
        private sealed class <GetAllAssignableWindowsRuntimeTypes>c__Iterator2 : IEnumerable, IEnumerable<GenericInstanceType>, IEnumerator, IDisposable, IEnumerator<GenericInstanceType>
        {
            internal GenericInstanceType $current;
            internal bool $disposing;
            internal IEnumerator<TypeReference[]> $locvar0;
            internal TypeReference[] $locvar1;
            internal int $locvar2;
            internal int $PC;
            internal TypeDefinition <clrTypeDefinition>__1;
            internal TypeReference[] <combination>__2;
            internal IEnumerable<TypeReference[]> <genericArgumentTypeCombinations>__1;
            internal TypeReference[][] <genericArgumentTypes>__1;
            internal GenericInstanceType <result>__3;
            internal TypeDefinition <windowsRuntimeTypeDefinition>__1;
            internal GenericInstanceType clrType;

            [DebuggerHidden]
            public void Dispose()
            {
                uint num = (uint) this.$PC;
                this.$disposing = true;
                this.$PC = -1;
                switch (num)
                {
                    case 1:
                        try
                        {
                        }
                        finally
                        {
                            if (this.$locvar0 != null)
                            {
                                this.$locvar0.Dispose();
                            }
                        }
                        break;
                }
            }

            public bool MoveNext()
            {
                uint num = (uint) this.$PC;
                this.$PC = -1;
                bool flag = false;
                switch (num)
                {
                    case 0:
                        this.<clrTypeDefinition>__1 = this.clrType.Resolve();
                        this.<windowsRuntimeTypeDefinition>__1 = Extensions.WindowsRuntimeProjections.ProjectToWindowsRuntime(this.<clrTypeDefinition>__1);
                        if (this.<windowsRuntimeTypeDefinition>__1 != this.<clrTypeDefinition>__1)
                        {
                            this.<genericArgumentTypes>__1 = new TypeReference[this.clrType.GenericArguments.Count][];
                            for (int i = 0; i < this.<genericArgumentTypes>__1.Length; i++)
                            {
                                TypeReference type = this.clrType.GenericArguments[i];
                                GenericParameter parameter = this.<clrTypeDefinition>__1.GenericParameters[i];
                                GenericParameterAttributes attributes = (GenericParameterAttributes) ((ushort) (parameter.Attributes & (GenericParameterAttributes.Contravariant | GenericParameterAttributes.Covariant)));
                                switch (attributes)
                                {
                                    case GenericParameterAttributes.NonVariant:
                                        this.<genericArgumentTypes>__1[i] = !type.IsValidForWindowsRuntimeType() ? new TypeReference[0] : new TypeReference[] { type };
                                        break;

                                    case GenericParameterAttributes.Covariant:
                                        this.<genericArgumentTypes>__1[i] = type.GetWindowsRuntimeCovariantTypes().ToArray<TypeReference>();
                                        break;

                                    case GenericParameterAttributes.Contravariant:
                                        throw new NotSupportedException($"'{this.clrType.FullName}' type contains unsupported contravariant generic parameter '{parameter.Name}'.");

                                    default:
                                        throw new Exception($"'{parameter.Name}' generic parameter in '{this.clrType.FullName}' type contains invalid variance value '{attributes}'.");
                                }
                                if (this.<genericArgumentTypes>__1[i].Length == 0)
                                {
                                    goto Label_0294;
                                }
                            }
                            this.<genericArgumentTypeCombinations>__1 = Extensions.GetTypeCombinations(this.<genericArgumentTypes>__1, 0);
                            this.$locvar0 = this.<genericArgumentTypeCombinations>__1.GetEnumerator();
                            num = 0xfffffffd;
                            break;
                        }
                        goto Label_0294;

                    case 1:
                        break;

                    default:
                        goto Label_0294;
                }
                try
                {
                    while (this.$locvar0.MoveNext())
                    {
                        this.<combination>__2 = this.$locvar0.Current;
                        this.<result>__3 = new GenericInstanceType(this.<windowsRuntimeTypeDefinition>__1);
                        this.$locvar1 = this.<combination>__2;
                        this.$locvar2 = 0;
                        while (this.$locvar2 < this.$locvar1.Length)
                        {
                            TypeReference item = this.$locvar1[this.$locvar2];
                            this.<result>__3.GenericArguments.Add(item);
                            this.$locvar2++;
                        }
                        this.$current = this.<result>__3;
                        if (!this.$disposing)
                        {
                            this.$PC = 1;
                        }
                        flag = true;
                        return true;
                    }
                }
                finally
                {
                    if (!flag)
                    {
                    }
                    if (this.$locvar0 != null)
                    {
                        this.$locvar0.Dispose();
                    }
                }
                this.$PC = -1;
            Label_0294:
                return false;
            }

            [DebuggerHidden]
            public void Reset()
            {
                throw new NotSupportedException();
            }

            [DebuggerHidden]
            IEnumerator<GenericInstanceType> IEnumerable<GenericInstanceType>.GetEnumerator()
            {
                if (Interlocked.CompareExchange(ref this.$PC, 0, -2) == -2)
                {
                    return this;
                }
                return new Extensions.<GetAllAssignableWindowsRuntimeTypes>c__Iterator2 { clrType = this.clrType };
            }

            [DebuggerHidden]
            IEnumerator IEnumerable.GetEnumerator() => 
                this.System.Collections.Generic.IEnumerable<Mono.Cecil.GenericInstanceType>.GetEnumerator();

            GenericInstanceType IEnumerator<GenericInstanceType>.Current =>
                this.$current;

            object IEnumerator.Current =>
                this.$current;
        }

        [CompilerGenerated]
        private sealed class <GetOverridenInterfaceMethod>c__AnonStorey7
        {
            internal MethodReference overridingMethod;

            internal bool <>m__0(MethodReference interfaceMethod) => 
                ((this.overridingMethod.Name == interfaceMethod.Name) && VirtualMethodResolution.MethodSignaturesMatchIgnoreStaticness(interfaceMethod, this.overridingMethod));
        }

        [CompilerGenerated]
        private sealed class <GetTypeCombinations>c__Iterator4 : IEnumerable, IEnumerable<TypeReference[]>, IEnumerator, IDisposable, IEnumerator<TypeReference[]>
        {
            internal TypeReference[] $current;
            internal bool $disposing;
            internal TypeReference[] $locvar0;
            internal int $locvar1;
            internal IEnumerator<TypeReference[]> $locvar2;
            internal TypeReference[] $locvar3;
            internal int $locvar4;
            internal int $PC;
            internal TypeReference[] <combination>__5;
            internal IEnumerable<TypeReference[]> <combinations>__4;
            internal TypeReference[] <levelTypes>__1;
            internal TypeReference[] <result>__3;
            internal TypeReference[] <result>__7;
            internal TypeReference <type>__2;
            internal TypeReference <type>__6;
            internal int level;
            internal TypeReference[][] types;

            [DebuggerHidden]
            public void Dispose()
            {
                uint num = (uint) this.$PC;
                this.$disposing = true;
                this.$PC = -1;
                switch (num)
                {
                    case 2:
                        try
                        {
                        }
                        finally
                        {
                            if (this.$locvar2 != null)
                            {
                                this.$locvar2.Dispose();
                            }
                        }
                        break;
                }
            }

            public bool MoveNext()
            {
                uint num = (uint) this.$PC;
                this.$PC = -1;
                bool flag = false;
                switch (num)
                {
                    case 0:
                        this.<levelTypes>__1 = this.types[this.level];
                        if ((this.level + 1) != this.types.Length)
                        {
                            this.<combinations>__4 = Extensions.GetTypeCombinations(this.types, this.level + 1);
                            this.$locvar2 = this.<combinations>__4.GetEnumerator();
                            num = 0xfffffffd;
                            goto Label_011F;
                        }
                        this.$locvar0 = this.<levelTypes>__1;
                        this.$locvar1 = 0;
                        break;

                    case 1:
                        this.$locvar1++;
                        break;

                    case 2:
                        goto Label_011F;

                    default:
                        goto Label_0215;
                }
                if (this.$locvar1 < this.$locvar0.Length)
                {
                    this.<type>__2 = this.$locvar0[this.$locvar1];
                    this.<result>__3 = new TypeReference[this.types.Length];
                    this.<result>__3[this.types.Length - 1] = this.<type>__2;
                    this.$current = this.<result>__3;
                    if (!this.$disposing)
                    {
                        this.$PC = 1;
                    }
                    goto Label_0217;
                }
                goto Label_020E;
            Label_011F:
                try
                {
                    switch (num)
                    {
                        case 2:
                            goto Label_01BA;
                    }
                    while (this.$locvar2.MoveNext())
                    {
                        this.<combination>__5 = this.$locvar2.Current;
                        this.$locvar3 = this.<levelTypes>__1;
                        this.$locvar4 = 0;
                        while (this.$locvar4 < this.$locvar3.Length)
                        {
                            this.<type>__6 = this.$locvar3[this.$locvar4];
                            this.<result>__7 = (TypeReference[]) this.<combination>__5.Clone();
                            this.<result>__7[this.level] = this.<type>__6;
                            this.$current = this.<result>__7;
                            if (!this.$disposing)
                            {
                                this.$PC = 2;
                            }
                            flag = true;
                            goto Label_0217;
                        Label_01BA:
                            this.$locvar4++;
                        }
                    }
                }
                finally
                {
                    if (!flag)
                    {
                    }
                    if (this.$locvar2 != null)
                    {
                        this.$locvar2.Dispose();
                    }
                }
            Label_020E:
                this.$PC = -1;
            Label_0215:
                return false;
            Label_0217:
                return true;
            }

            [DebuggerHidden]
            public void Reset()
            {
                throw new NotSupportedException();
            }

            [DebuggerHidden]
            IEnumerator<TypeReference[]> IEnumerable<TypeReference[]>.GetEnumerator()
            {
                if (Interlocked.CompareExchange(ref this.$PC, 0, -2) == -2)
                {
                    return this;
                }
                return new Extensions.<GetTypeCombinations>c__Iterator4 { 
                    types = this.types,
                    level = this.level
                };
            }

            [DebuggerHidden]
            IEnumerator IEnumerable.GetEnumerator() => 
                this.System.Collections.Generic.IEnumerable<Mono.Cecil.TypeReference[]>.GetEnumerator();

            TypeReference[] IEnumerator<TypeReference[]>.Current =>
                this.$current;

            object IEnumerator.Current =>
                this.$current;
        }

        [CompilerGenerated]
        private sealed class <GetTypeHierarchy>c__Iterator0 : IEnumerable, IEnumerable<TypeDefinition>, IEnumerator, IDisposable, IEnumerator<TypeDefinition>
        {
            internal TypeDefinition $current;
            internal bool $disposing;
            internal int $PC;
            internal TypeDefinition <$>type;
            internal TypeDefinition type;

            [DebuggerHidden]
            public void Dispose()
            {
                this.$disposing = true;
                this.$PC = -1;
            }

            public bool MoveNext()
            {
                uint num = (uint) this.$PC;
                this.$PC = -1;
                switch (num)
                {
                    case 0:
                        break;

                    case 1:
                        this.type = this.type.BaseType?.Resolve();
                        break;

                    default:
                        goto Label_0087;
                }
                if (this.type != null)
                {
                    this.$current = this.type;
                    if (!this.$disposing)
                    {
                        this.$PC = 1;
                    }
                    return true;
                }
                this.$PC = -1;
            Label_0087:
                return false;
            }

            [DebuggerHidden]
            public void Reset()
            {
                throw new NotSupportedException();
            }

            [DebuggerHidden]
            IEnumerator<TypeDefinition> IEnumerable<TypeDefinition>.GetEnumerator()
            {
                if (Interlocked.CompareExchange(ref this.$PC, 0, -2) == -2)
                {
                    return this;
                }
                return new Extensions.<GetTypeHierarchy>c__Iterator0 { type = this.<$>type };
            }

            [DebuggerHidden]
            IEnumerator IEnumerable.GetEnumerator() => 
                this.System.Collections.Generic.IEnumerable<Mono.Cecil.TypeDefinition>.GetEnumerator();

            TypeDefinition IEnumerator<TypeDefinition>.Current =>
                this.$current;

            object IEnumerator.Current =>
                this.$current;
        }

        [CompilerGenerated]
        private sealed class <GetTypesFromSpecificAttribute>c__AnonStorey6
        {
            internal string attributeName;

            internal bool <>m__0(CustomAttribute ca) => 
                (ca.AttributeType.FullName == this.attributeName);
        }

        [CompilerGenerated]
        private sealed class <GetWindowsRuntimeCovariantTypesWithDuplicates>c__Iterator3 : IEnumerable, IEnumerable<TypeReference>, IEnumerator, IDisposable, IEnumerator<TypeReference>
        {
            internal TypeReference $current;
            internal bool $disposing;
            internal IEnumerator<TypeReference> $locvar0;
            internal IEnumerator<TypeReference> $locvar1;
            internal IEnumerator<TypeReference> $locvar2;
            internal int $PC;
            internal TypeReference <baseType>__3;
            internal TypeReference <covariantType>__2;
            internal TypeReference <covariantType>__4;
            internal TypeReference <covariantType>__5;
            internal TypeDefinition <iEnumerableType>__1;
            internal GenericInstanceType <iEnumerableTypeTypeGenericInstanceType>__3;
            internal TypeReference type;

            [DebuggerHidden]
            public void Dispose()
            {
                uint num = (uint) this.$PC;
                this.$disposing = true;
                this.$PC = -1;
                switch (num)
                {
                    case 2:
                        try
                        {
                        }
                        finally
                        {
                            if (this.$locvar0 != null)
                            {
                                this.$locvar0.Dispose();
                            }
                        }
                        break;

                    case 3:
                        try
                        {
                        }
                        finally
                        {
                            if (this.$locvar1 != null)
                            {
                                this.$locvar1.Dispose();
                            }
                        }
                        break;

                    case 4:
                        try
                        {
                        }
                        finally
                        {
                            if (this.$locvar2 != null)
                            {
                                this.$locvar2.Dispose();
                            }
                        }
                        break;
                }
            }

            public bool MoveNext()
            {
                uint num = (uint) this.$PC;
                this.$PC = -1;
                bool flag = false;
                switch (num)
                {
                    case 0:
                        if (!this.type.IsValidForWindowsRuntimeType())
                        {
                            break;
                        }
                        this.$current = this.type;
                        if (!this.$disposing)
                        {
                            this.$PC = 1;
                        }
                        goto Label_02E4;

                    case 1:
                        break;

                    case 2:
                        goto Label_00D9;

                    case 3:
                        goto Label_01B7;

                    case 4:
                        goto Label_0267;

                    default:
                        goto Label_02E2;
                }
                if (this.type.IsValueType() || this.type.IsSystemObject())
                {
                    goto Label_02E2;
                }
                if (!this.type.IsArray)
                {
                    TypeReference baseType = this.type.GetBaseType();
                    if (baseType == null)
                    {
                    }
                    this.<baseType>__3 = Extensions.TypeProvider.ObjectTypeReference;
                    this.$locvar1 = this.<baseType>__3.GetWindowsRuntimeCovariantTypes().GetEnumerator();
                    num = 0xfffffffd;
                    goto Label_01B7;
                }
                this.<iEnumerableType>__1 = Extensions.TypeProvider.Corlib.MainModule.GetType("System.Collections.Generic.IEnumerable`1");
                this.$locvar0 = ((ArrayType) this.type).ElementType.GetWindowsRuntimeCovariantTypes().GetEnumerator();
                num = 0xfffffffd;
            Label_00D9:
                try
                {
                    while (this.$locvar0.MoveNext())
                    {
                        this.<covariantType>__2 = this.$locvar0.Current;
                        this.<iEnumerableTypeTypeGenericInstanceType>__3 = new GenericInstanceType(this.<iEnumerableType>__1);
                        this.<iEnumerableTypeTypeGenericInstanceType>__3.GenericArguments.Add(this.<covariantType>__2);
                        this.$current = this.<iEnumerableTypeTypeGenericInstanceType>__3;
                        if (!this.$disposing)
                        {
                            this.$PC = 2;
                        }
                        flag = true;
                        goto Label_02E4;
                    }
                }
                finally
                {
                    if (!flag)
                    {
                    }
                    if (this.$locvar0 != null)
                    {
                        this.$locvar0.Dispose();
                    }
                }
                goto Label_02E2;
            Label_01B7:
                try
                {
                    while (this.$locvar1.MoveNext())
                    {
                        this.<covariantType>__4 = this.$locvar1.Current;
                        this.$current = this.<covariantType>__4;
                        if (!this.$disposing)
                        {
                            this.$PC = 3;
                        }
                        flag = true;
                        goto Label_02E4;
                    }
                }
                finally
                {
                    if (!flag)
                    {
                    }
                    if (this.$locvar1 != null)
                    {
                        this.$locvar1.Dispose();
                    }
                }
                if (Extensions.<>f__mg$cache3 == null)
                {
                    Extensions.<>f__mg$cache3 = new Func<TypeReference, IEnumerable<TypeReference>>(Extensions.GetWindowsRuntimeCovariantTypes);
                }
                this.$locvar2 = this.type.GetInterfaces().SelectMany<TypeReference, TypeReference>(Extensions.<>f__mg$cache3).GetEnumerator();
                num = 0xfffffffd;
            Label_0267:
                try
                {
                    while (this.$locvar2.MoveNext())
                    {
                        this.<covariantType>__5 = this.$locvar2.Current;
                        this.$current = this.<covariantType>__5;
                        if (!this.$disposing)
                        {
                            this.$PC = 4;
                        }
                        flag = true;
                        goto Label_02E4;
                    }
                }
                finally
                {
                    if (!flag)
                    {
                    }
                    if (this.$locvar2 != null)
                    {
                        this.$locvar2.Dispose();
                    }
                }
                this.$PC = -1;
            Label_02E2:
                return false;
            Label_02E4:
                return true;
            }

            [DebuggerHidden]
            public void Reset()
            {
                throw new NotSupportedException();
            }

            [DebuggerHidden]
            IEnumerator<TypeReference> IEnumerable<TypeReference>.GetEnumerator()
            {
                if (Interlocked.CompareExchange(ref this.$PC, 0, -2) == -2)
                {
                    return this;
                }
                return new Extensions.<GetWindowsRuntimeCovariantTypesWithDuplicates>c__Iterator3 { type = this.type };
            }

            [DebuggerHidden]
            IEnumerator IEnumerable.GetEnumerator() => 
                this.System.Collections.Generic.IEnumerable<Mono.Cecil.TypeReference>.GetEnumerator();

            TypeReference IEnumerator<TypeReference>.Current =>
                this.$current;

            object IEnumerator.Current =>
                this.$current;
        }

        [CompilerGenerated]
        private sealed class <ImplementedWindowsRuntimeProjectedInterfaces>c__Iterator1 : IEnumerable, IEnumerable<TypeReference>, IEnumerator, IDisposable, IEnumerator<TypeReference>
        {
            internal TypeReference $current;
            internal bool $disposing;
            internal IEnumerator<TypeReference> $locvar0;
            internal IEnumerator<GenericInstanceType> $locvar1;
            private <ImplementedWindowsRuntimeProjectedInterfaces>c__AnonStorey5 $locvar2;
            internal int $PC;
            private static Func<GenericInstanceType, bool> <>f__am$cache0;
            internal TypeReference <clrInterface>__2;
            internal GenericInstanceType <genericType>__3;
            internal GenericInstanceType <windowsRuntimeInterface>__4;
            internal TypeReference <windowsRuntimeInterface>__5;
            internal TypeReference type;

            private static bool <>m__0(GenericInstanceType i) => 
                Extensions.WindowsRuntimeProjections.IsSupportedProjectedInterfaceWindowsRuntime(i);

            [DebuggerHidden]
            public void Dispose()
            {
                uint num = (uint) this.$PC;
                this.$disposing = true;
                this.$PC = -1;
                switch (num)
                {
                    case 1:
                    case 2:
                        try
                        {
                            switch (num)
                            {
                                case 1:
                                    try
                                    {
                                    }
                                    finally
                                    {
                                        if (this.$locvar1 != null)
                                        {
                                            this.$locvar1.Dispose();
                                        }
                                    }
                                    return;
                            }
                        }
                        finally
                        {
                            if (this.$locvar0 != null)
                            {
                                this.$locvar0.Dispose();
                            }
                        }
                        break;
                }
            }

            public bool MoveNext()
            {
                uint num = (uint) this.$PC;
                this.$PC = -1;
                bool flag = false;
                switch (num)
                {
                    case 0:
                        this.$locvar2 = new <ImplementedWindowsRuntimeProjectedInterfaces>c__AnonStorey5();
                        this.$locvar2.typeResolver = Unity.IL2CPP.ILPreProcessor.TypeResolver.For(this.type);
                        this.$locvar0 = this.type.Resolve().Interfaces.Select<InterfaceImplementation, TypeReference>(new Func<InterfaceImplementation, TypeReference>(this.$locvar2.<>m__0)).GetEnumerator();
                        num = 0xfffffffd;
                        break;

                    case 1:
                    case 2:
                        break;

                    default:
                        goto Label_0210;
                }
                try
                {
                    switch (num)
                    {
                        case 1:
                            goto Label_00FE;

                        case 2:
                            goto Label_01D8;
                    }
                    while (this.$locvar0.MoveNext())
                    {
                        this.<clrInterface>__2 = this.$locvar0.Current;
                        this.<genericType>__3 = this.<clrInterface>__2 as GenericInstanceType;
                        if (this.<genericType>__3 == null)
                        {
                            goto Label_0178;
                        }
                        if (<>f__am$cache0 == null)
                        {
                            <>f__am$cache0 = new Func<GenericInstanceType, bool>(Extensions.<ImplementedWindowsRuntimeProjectedInterfaces>c__Iterator1.<>m__0);
                        }
                        this.$locvar1 = this.<genericType>__3.GetAllAssignableWindowsRuntimeTypes().Where<GenericInstanceType>(<>f__am$cache0).GetEnumerator();
                        num = 0xfffffffd;
                    Label_00FE:
                        try
                        {
                            while (this.$locvar1.MoveNext())
                            {
                                this.<windowsRuntimeInterface>__4 = this.$locvar1.Current;
                                this.$current = this.<windowsRuntimeInterface>__4;
                                if (!this.$disposing)
                                {
                                    this.$PC = 1;
                                }
                                flag = true;
                                goto Label_0212;
                            }
                        }
                        finally
                        {
                            if (!flag)
                            {
                            }
                            if (this.$locvar1 != null)
                            {
                                this.$locvar1.Dispose();
                            }
                        }
                        continue;
                    Label_0178:
                        this.<windowsRuntimeInterface>__5 = Extensions.WindowsRuntimeProjections.ProjectToWindowsRuntime(this.<clrInterface>__2);
                        if ((this.<windowsRuntimeInterface>__5 != this.<clrInterface>__2) && Extensions.WindowsRuntimeProjections.IsSupportedProjectedInterfaceWindowsRuntime(this.<windowsRuntimeInterface>__5))
                        {
                            this.$current = this.<windowsRuntimeInterface>__5;
                            if (!this.$disposing)
                            {
                                this.$PC = 2;
                            }
                            flag = true;
                            goto Label_0212;
                        }
                    Label_01D8:;
                    }
                }
                finally
                {
                    if (!flag)
                    {
                    }
                    if (this.$locvar0 != null)
                    {
                        this.$locvar0.Dispose();
                    }
                }
                this.$PC = -1;
            Label_0210:
                return false;
            Label_0212:
                return true;
            }

            [DebuggerHidden]
            public void Reset()
            {
                throw new NotSupportedException();
            }

            [DebuggerHidden]
            IEnumerator<TypeReference> IEnumerable<TypeReference>.GetEnumerator()
            {
                if (Interlocked.CompareExchange(ref this.$PC, 0, -2) == -2)
                {
                    return this;
                }
                return new Extensions.<ImplementedWindowsRuntimeProjectedInterfaces>c__Iterator1 { type = this.type };
            }

            [DebuggerHidden]
            IEnumerator IEnumerable.GetEnumerator() => 
                this.System.Collections.Generic.IEnumerable<Mono.Cecil.TypeReference>.GetEnumerator();

            TypeReference IEnumerator<TypeReference>.Current =>
                this.$current;

            object IEnumerator.Current =>
                this.$current;

            private sealed class <ImplementedWindowsRuntimeProjectedInterfaces>c__AnonStorey5
            {
                internal Unity.IL2CPP.ILPreProcessor.TypeResolver typeResolver;

                internal TypeReference <>m__0(InterfaceImplementation i) => 
                    this.typeResolver.Resolve(i.InterfaceType);
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct ChunkItem<T>
        {
            public int Index;
            public T Value;
        }
    }
}
