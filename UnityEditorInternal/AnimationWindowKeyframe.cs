﻿namespace UnityEditorInternal
{
    using System;
    using UnityEditor;
    using UnityEngine;

    internal class AnimationWindowKeyframe
    {
        private AnimationWindowCurve m_curve;
        private int m_Hash;
        public float m_InTangent;
        public float m_OutTangent;
        public int m_TangentMode;
        private float m_time;
        public int m_TimeHash;
        private object m_value;

        public AnimationWindowKeyframe()
        {
        }

        public AnimationWindowKeyframe(AnimationWindowKeyframe key)
        {
            this.time = key.time;
            this.value = key.value;
            this.curve = key.curve;
            this.m_InTangent = key.m_InTangent;
            this.m_OutTangent = key.m_OutTangent;
            this.m_TangentMode = key.m_TangentMode;
            this.m_curve = key.m_curve;
        }

        public AnimationWindowKeyframe(AnimationWindowCurve curve, ObjectReferenceKeyframe key)
        {
            this.time = key.time;
            this.value = key.value;
            this.curve = curve;
        }

        public AnimationWindowKeyframe(AnimationWindowCurve curve, Keyframe key)
        {
            this.time = key.time;
            this.value = key.value;
            this.curve = curve;
            this.m_InTangent = key.inTangent;
            this.m_OutTangent = key.outTangent;
            this.m_TangentMode = key.tangentMode;
            this.m_curve = curve;
        }

        public int GetHash()
        {
            if (this.m_Hash == 0)
            {
                this.m_Hash = this.curve.GetHashCode();
                this.m_Hash = (0x21 * this.m_Hash) + this.time.GetHashCode();
            }
            return this.m_Hash;
        }

        public int GetIndex()
        {
            for (int i = 0; i < this.curve.m_Keyframes.Count; i++)
            {
                if (this.curve.m_Keyframes[i] == this)
                {
                    return i;
                }
            }
            return -1;
        }

        public AnimationWindowCurve curve
        {
            get => 
                this.m_curve;
            set
            {
                this.m_curve = value;
                this.m_Hash = 0;
            }
        }

        public float inTangent
        {
            get => 
                this.m_InTangent;
            set
            {
                this.m_InTangent = value;
            }
        }

        public bool isPPtrCurve =>
            this.curve.isPPtrCurve;

        public float outTangent
        {
            get => 
                this.m_OutTangent;
            set
            {
                this.m_OutTangent = value;
            }
        }

        public float time
        {
            get => 
                this.m_time;
            set
            {
                this.m_time = value;
                this.m_Hash = 0;
                this.m_TimeHash = value.GetHashCode();
            }
        }

        public object value
        {
            get => 
                this.m_value;
            set
            {
                this.m_value = value;
            }
        }
    }
}

