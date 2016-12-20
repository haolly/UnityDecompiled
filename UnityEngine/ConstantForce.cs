﻿namespace UnityEngine
{
    using System;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;

    /// <summary>
    /// <para>A force applied constantly.</para>
    /// </summary>
    public sealed class ConstantForce : Behaviour
    {
        [MethodImpl(MethodImplOptions.InternalCall)]
        private extern void INTERNAL_get_force(out Vector3 value);
        [MethodImpl(MethodImplOptions.InternalCall)]
        private extern void INTERNAL_get_relativeForce(out Vector3 value);
        [MethodImpl(MethodImplOptions.InternalCall)]
        private extern void INTERNAL_get_relativeTorque(out Vector3 value);
        [MethodImpl(MethodImplOptions.InternalCall)]
        private extern void INTERNAL_get_torque(out Vector3 value);
        [MethodImpl(MethodImplOptions.InternalCall)]
        private extern void INTERNAL_set_force(ref Vector3 value);
        [MethodImpl(MethodImplOptions.InternalCall)]
        private extern void INTERNAL_set_relativeForce(ref Vector3 value);
        [MethodImpl(MethodImplOptions.InternalCall)]
        private extern void INTERNAL_set_relativeTorque(ref Vector3 value);
        [MethodImpl(MethodImplOptions.InternalCall)]
        private extern void INTERNAL_set_torque(ref Vector3 value);

        /// <summary>
        /// <para>The force applied to the rigidbody every frame.</para>
        /// </summary>
        public Vector3 force
        {
            get
            {
                Vector3 vector;
                this.INTERNAL_get_force(out vector);
                return vector;
            }
            set
            {
                this.INTERNAL_set_force(ref value);
            }
        }

        /// <summary>
        /// <para>The force - relative to the rigid bodies coordinate system - applied every frame.</para>
        /// </summary>
        public Vector3 relativeForce
        {
            get
            {
                Vector3 vector;
                this.INTERNAL_get_relativeForce(out vector);
                return vector;
            }
            set
            {
                this.INTERNAL_set_relativeForce(ref value);
            }
        }

        /// <summary>
        /// <para>The torque - relative to the rigid bodies coordinate system - applied every frame.</para>
        /// </summary>
        public Vector3 relativeTorque
        {
            get
            {
                Vector3 vector;
                this.INTERNAL_get_relativeTorque(out vector);
                return vector;
            }
            set
            {
                this.INTERNAL_set_relativeTorque(ref value);
            }
        }

        /// <summary>
        /// <para>The torque applied to the rigidbody every frame.</para>
        /// </summary>
        public Vector3 torque
        {
            get
            {
                Vector3 vector;
                this.INTERNAL_get_torque(out vector);
                return vector;
            }
            set
            {
                this.INTERNAL_set_torque(ref value);
            }
        }
    }
}

