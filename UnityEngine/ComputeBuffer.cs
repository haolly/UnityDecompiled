﻿namespace UnityEngine
{
    using System;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using System.Security;
    using UnityEngine.Scripting;

    /// <summary>
    /// <para>GPU data buffer, mostly for use with compute shaders.</para>
    /// </summary>
    public sealed class ComputeBuffer : IDisposable
    {
        internal IntPtr m_Ptr;

        /// <summary>
        /// <para>Create a Compute Buffer.</para>
        /// </summary>
        /// <param name="count">Number of elements in the buffer.</param>
        /// <param name="stride">Size of one element in the buffer. Has to match size of buffer type in the shader. See for cross-platform compatibility information.</param>
        /// <param name="type">Type of the buffer, default is ComputeBufferType.Default (structured buffer).</param>
        public ComputeBuffer(int count, int stride) : this(count, stride, ComputeBufferType.Default, 3)
        {
        }

        /// <summary>
        /// <para>Create a Compute Buffer.</para>
        /// </summary>
        /// <param name="count">Number of elements in the buffer.</param>
        /// <param name="stride">Size of one element in the buffer. Has to match size of buffer type in the shader. See for cross-platform compatibility information.</param>
        /// <param name="type">Type of the buffer, default is ComputeBufferType.Default (structured buffer).</param>
        public ComputeBuffer(int count, int stride, ComputeBufferType type) : this(count, stride, type, 3)
        {
        }

        internal ComputeBuffer(int count, int stride, ComputeBufferType type, int stackDepth)
        {
            if (count <= 0)
            {
                throw new ArgumentException("Attempting to create a zero length compute buffer", "count");
            }
            if (stride < 0)
            {
                throw new ArgumentException("Attempting to create a compute buffer with a negative stride", "stride");
            }
            this.m_Ptr = IntPtr.Zero;
            InitBuffer(this, count, stride, type);
            this.SaveCallstack(stackDepth);
        }

        /// <summary>
        /// <para>Copy counter value of append/consume buffer into another buffer.</para>
        /// </summary>
        /// <param name="src">Append/consume buffer to copy the counter from.</param>
        /// <param name="dst">A buffer to copy the counter to.</param>
        /// <param name="dstOffset">Target byte offset in dst.</param>
        [MethodImpl(MethodImplOptions.InternalCall), GeneratedByOldBindingsGenerator]
        public static extern void CopyCount(ComputeBuffer src, ComputeBuffer dst, int dstOffset);
        [MethodImpl(MethodImplOptions.InternalCall), GeneratedByOldBindingsGenerator]
        private static extern void DestroyBuffer(ComputeBuffer buf);
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                DestroyBuffer(this);
            }
            else if (this.m_Ptr != IntPtr.Zero)
            {
                UnityEngine.Debug.LogWarning($"GarbageCollector disposing of ComputeBuffer allocated in {this.GetFileName()} at line {this.GetLineNumber()}. Please use ComputeBuffer.Release() or .Dispose() to manually release the buffer.");
            }
            this.m_Ptr = IntPtr.Zero;
        }

        ~ComputeBuffer()
        {
            this.Dispose(false);
        }

        /// <summary>
        /// <para>Read data values from the buffer into an array.</para>
        /// </summary>
        /// <param name="data">An array to receive the data.</param>
        [SecuritySafeCritical]
        public void GetData(Array data)
        {
            this.InternalGetData(data, Marshal.SizeOf(data.GetType().GetElementType()));
        }

        [MethodImpl(MethodImplOptions.InternalCall), ThreadAndSerializationSafe, GeneratedByOldBindingsGenerator]
        internal extern string GetFileName();
        [MethodImpl(MethodImplOptions.InternalCall), ThreadAndSerializationSafe, GeneratedByOldBindingsGenerator]
        internal extern int GetLineNumber();
        /// <summary>
        /// <para>Retrieve a native (underlying graphics API) pointer to the buffer.</para>
        /// </summary>
        /// <returns>
        /// <para>Pointer to the underlying graphics API buffer.</para>
        /// </returns>
        public IntPtr GetNativeBufferPtr()
        {
            IntPtr ptr;
            INTERNAL_CALL_GetNativeBufferPtr(this, out ptr);
            return ptr;
        }

        [MethodImpl(MethodImplOptions.InternalCall), GeneratedByOldBindingsGenerator]
        private static extern void InitBuffer(ComputeBuffer buf, int count, int stride, ComputeBufferType type);
        [MethodImpl(MethodImplOptions.InternalCall), GeneratedByOldBindingsGenerator]
        private static extern void INTERNAL_CALL_GetNativeBufferPtr(ComputeBuffer self, out IntPtr value);
        [MethodImpl(MethodImplOptions.InternalCall), SecurityCritical, GeneratedByOldBindingsGenerator]
        private extern void InternalGetData(Array data, int elemSize);
        [MethodImpl(MethodImplOptions.InternalCall), SecurityCritical, GeneratedByOldBindingsGenerator]
        private extern void InternalSetData(Array data, int elemSize);
        /// <summary>
        /// <para>Release a Compute Buffer.</para>
        /// </summary>
        public void Release()
        {
            this.Dispose();
        }

        internal void SaveCallstack(int stackDepth)
        {
            StackFrame frame = new StackFrame(stackDepth, true);
            this.SaveCallstack_Internal(frame.GetFileName(), frame.GetFileLineNumber());
        }

        [MethodImpl(MethodImplOptions.InternalCall), GeneratedByOldBindingsGenerator]
        private extern void SaveCallstack_Internal(string fileName, int lineNumber);
        /// <summary>
        /// <para>Sets counter value of append/consume buffer.</para>
        /// </summary>
        /// <param name="counterValue">Value of the append/consume counter.</param>
        [MethodImpl(MethodImplOptions.InternalCall), GeneratedByOldBindingsGenerator]
        public extern void SetCounterValue(uint counterValue);
        /// <summary>
        /// <para>Set the buffer with values from an array.</para>
        /// </summary>
        /// <param name="data">Array of values to fill the buffer.</param>
        [SecuritySafeCritical]
        public void SetData(Array data)
        {
            this.InternalSetData(data, Marshal.SizeOf(data.GetType().GetElementType()));
        }

        /// <summary>
        /// <para>Number of elements in the buffer (Read Only).</para>
        /// </summary>
        public int count { [MethodImpl(MethodImplOptions.InternalCall), GeneratedByOldBindingsGenerator] get; }

        /// <summary>
        /// <para>Size of one element in the buffer (Read Only).</para>
        /// </summary>
        public int stride { [MethodImpl(MethodImplOptions.InternalCall), GeneratedByOldBindingsGenerator] get; }
    }
}

