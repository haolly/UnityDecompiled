﻿namespace UnityEngine
{
    using mscorlib;
    using System;
    using System.Runtime.InteropServices;
    using UnityEngine.Scripting;

    /// <summary>
    /// <para>Skinning bone weights of a vertex in the mesh.</para>
    /// </summary>
    [StructLayout(LayoutKind.Sequential), UsedByNativeCode]
    public struct BoneWeight
    {
        private float m_Weight0;
        private float m_Weight1;
        private float m_Weight2;
        private float m_Weight3;
        private int m_BoneIndex0;
        private int m_BoneIndex1;
        private int m_BoneIndex2;
        private int m_BoneIndex3;
        /// <summary>
        /// <para>Skinning weight for first bone.</para>
        /// </summary>
        public float weight0
        {
            get => 
                this.m_Weight0;
            set
            {
                this.m_Weight0 = value;
            }
        }
        /// <summary>
        /// <para>Skinning weight for second bone.</para>
        /// </summary>
        public float weight1
        {
            get => 
                this.m_Weight1;
            set
            {
                this.m_Weight1 = value;
            }
        }
        /// <summary>
        /// <para>Skinning weight for third bone.</para>
        /// </summary>
        public float weight2
        {
            get => 
                this.m_Weight2;
            set
            {
                this.m_Weight2 = value;
            }
        }
        /// <summary>
        /// <para>Skinning weight for fourth bone.</para>
        /// </summary>
        public float weight3
        {
            get => 
                this.m_Weight3;
            set
            {
                this.m_Weight3 = value;
            }
        }
        /// <summary>
        /// <para>Index of first bone.</para>
        /// </summary>
        public int boneIndex0
        {
            get => 
                this.m_BoneIndex0;
            set
            {
                this.m_BoneIndex0 = value;
            }
        }
        /// <summary>
        /// <para>Index of second bone.</para>
        /// </summary>
        public int boneIndex1
        {
            get => 
                this.m_BoneIndex1;
            set
            {
                this.m_BoneIndex1 = value;
            }
        }
        /// <summary>
        /// <para>Index of third bone.</para>
        /// </summary>
        public int boneIndex2
        {
            get => 
                this.m_BoneIndex2;
            set
            {
                this.m_BoneIndex2 = value;
            }
        }
        /// <summary>
        /// <para>Index of fourth bone.</para>
        /// </summary>
        public int boneIndex3
        {
            get => 
                this.m_BoneIndex3;
            set
            {
                this.m_BoneIndex3 = value;
            }
        }
        public override int GetHashCode() => 
            (((((((this.boneIndex0.GetHashCode() ^ (this.boneIndex1.GetHashCode() << 2)) ^ (this.boneIndex2.GetHashCode() >> 2)) ^ (this.boneIndex3.GetHashCode() >> 1)) ^ (this.weight0.GetHashCode() << 5)) ^ (this.weight1.GetHashCode() << 4)) ^ (this.weight2.GetHashCode() >> 4)) ^ (this.weight3.GetHashCode() >> 3));

        public override bool Equals(object other)
        {
            Vector4 vector;
            System.Boolean ReflectorVariable0;
            if (!(other is BoneWeight))
            {
                return false;
            }
            BoneWeight weight = (BoneWeight) other;
            if ((this.boneIndex0.Equals(weight.boneIndex0) && this.boneIndex1.Equals(weight.boneIndex1)) && (this.boneIndex2.Equals(weight.boneIndex2) && this.boneIndex3.Equals(weight.boneIndex3)))
            {
                vector = new Vector4(this.weight0, this.weight1, this.weight2, this.weight3);
                ReflectorVariable0 = true;
            }
            else
            {
                ReflectorVariable0 = false;
            }
            return (ReflectorVariable0 ? vector.Equals(new Vector4(weight.weight0, weight.weight1, weight.weight2, weight.weight3)) : false);
        }

        public static bool operator ==(BoneWeight lhs, BoneWeight rhs) => 
            ((((lhs.boneIndex0 == rhs.boneIndex0) && (lhs.boneIndex1 == rhs.boneIndex1)) && ((lhs.boneIndex2 == rhs.boneIndex2) && (lhs.boneIndex3 == rhs.boneIndex3))) && (new Vector4(lhs.weight0, lhs.weight1, lhs.weight2, lhs.weight3) == new Vector4(rhs.weight0, rhs.weight1, rhs.weight2, rhs.weight3)));

        public static bool operator !=(BoneWeight lhs, BoneWeight rhs) => 
            !(lhs == rhs);
    }
}

