﻿namespace UnityEngine.Experimental.Director
{
    using System;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using UnityEngine;
    using UnityEngine.Scripting;

    [StructLayout(LayoutKind.Sequential), UsedByNativeCode]
    internal struct AnimationOffsetPlayable
    {
        internal AnimationPlayable handle;
        internal Playable node
        {
            get
            {
                return this.handle.node;
            }
        }
        public static AnimationOffsetPlayable Create()
        {
            AnimationOffsetPlayable that = new AnimationOffsetPlayable();
            InternalCreate(ref that);
            return that;
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void InternalCreate(ref AnimationOffsetPlayable that);
        public void Destroy()
        {
            this.node.Destroy();
        }

        public int inputCount
        {
            get
            {
                return Playables.GetInputCountValidated(*((Playable*) this), base.GetType());
            }
        }
        public unsafe Playable GetInput(int inputPort)
        {
            return Playables.GetInputValidated(*((Playable*) this), inputPort, base.GetType());
        }

        public int outputCount
        {
            get
            {
                return Playables.GetOutputCountValidated(*((Playable*) this), base.GetType());
            }
        }
        public unsafe Playable GetOutput(int outputPort)
        {
            return Playables.GetOutputValidated(*((Playable*) this), outputPort, base.GetType());
        }

        public unsafe float GetInputWeight(int index)
        {
            return Playables.GetInputWeightValidated(*((Playable*) this), index, base.GetType());
        }

        public unsafe void SetInputWeight(int inputIndex, float weight)
        {
            Playables.SetInputWeightValidated(*((Playable*) this), inputIndex, weight, base.GetType());
        }

        public PlayState state
        {
            get
            {
                return Playables.GetPlayStateValidated(*((Playable*) this), base.GetType());
            }
            set
            {
                Playables.SetPlayStateValidated(*((Playable*) this), value, base.GetType());
            }
        }
        public double time
        {
            get
            {
                return Playables.GetTimeValidated(*((Playable*) this), base.GetType());
            }
            set
            {
                Playables.SetTimeValidated(*((Playable*) this), value, base.GetType());
            }
        }
        public double duration
        {
            get
            {
                return Playables.GetDurationValidated(*((Playable*) this), base.GetType());
            }
            set
            {
                Playables.SetDurationValidated(*((Playable*) this), value, base.GetType());
            }
        }
        public static bool operator ==(AnimationOffsetPlayable x, Playable y)
        {
            return Playables.Equals((Playable) x, y);
        }

        public static bool operator !=(AnimationOffsetPlayable x, Playable y)
        {
            return !Playables.Equals((Playable) x, y);
        }

        public override unsafe bool Equals(object p)
        {
            return Playables.Equals(*((Playable*) this), p);
        }

        public override int GetHashCode()
        {
            return this.node.GetHashCode();
        }

        public static implicit operator Playable(AnimationOffsetPlayable b)
        {
            return b.node;
        }

        public static implicit operator AnimationPlayable(AnimationOffsetPlayable b)
        {
            return b.handle;
        }

        public unsafe bool IsValid()
        {
            return Playables.IsValid(*((Playable*) this));
        }

        public T CastTo<T>() where T: struct
        {
            return this.handle.CastTo<T>();
        }

        public unsafe int AddInput(Playable input)
        {
            return AnimationPlayableUtilities.AddInputValidated(*((AnimationPlayable*) this), input, base.GetType());
        }

        public unsafe bool RemoveInput(int index)
        {
            return AnimationPlayableUtilities.RemoveInputValidated(*((AnimationPlayable*) this), index, base.GetType());
        }

        public unsafe bool RemoveAllInputs()
        {
            return AnimationPlayableUtilities.RemoveAllInputsValidated(*((AnimationPlayable*) this), base.GetType());
        }

        private static Vector3 GetPosition(ref AnimationOffsetPlayable that)
        {
            Vector3 vector;
            INTERNAL_CALL_GetPosition(ref that, out vector);
            return vector;
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        private static extern void INTERNAL_CALL_GetPosition(ref AnimationOffsetPlayable that, out Vector3 value);
        private static void SetPosition(ref AnimationOffsetPlayable that, Vector3 value)
        {
            INTERNAL_CALL_SetPosition(ref that, ref value);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        private static extern void INTERNAL_CALL_SetPosition(ref AnimationOffsetPlayable that, ref Vector3 value);
        private static Quaternion GetRotation(ref AnimationOffsetPlayable that)
        {
            Quaternion quaternion;
            INTERNAL_CALL_GetRotation(ref that, out quaternion);
            return quaternion;
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        private static extern void INTERNAL_CALL_GetRotation(ref AnimationOffsetPlayable that, out Quaternion value);
        private static void SetRotation(ref AnimationOffsetPlayable that, Quaternion value)
        {
            INTERNAL_CALL_SetRotation(ref that, ref value);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        private static extern void INTERNAL_CALL_SetRotation(ref AnimationOffsetPlayable that, ref Quaternion value);
        public Vector3 position
        {
            get
            {
                return GetPosition(ref this);
            }
            set
            {
                SetPosition(ref this, value);
            }
        }
        public Quaternion rotation
        {
            get
            {
                return GetRotation(ref this);
            }
            set
            {
                SetRotation(ref this, value);
            }
        }
    }
}

