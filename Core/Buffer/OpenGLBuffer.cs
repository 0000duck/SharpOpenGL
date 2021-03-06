﻿using OpenTK.Graphics.OpenGL;
using System;
using System.Runtime.InteropServices;
using OpenTK;

namespace Core.Buffer
{
    public abstract class OpenGLBuffer : IDisposable, IBindable
    {
        public OpenGLBuffer()
        {
            bufferObject = GL.GenBuffer();
        }

        public OpenGLBuffer(string debugName)
         : this()
        {
            DebugName = debugName;
        }

        public void Dispose()
        {
            Unbind();
            GL.DeleteBuffer(bufferObject);
        }

        public void Bind()
        {
            GL.BindBuffer(bufferTarget, bufferObject);
            bBind = true;
        }

        public bool IsBind
        {
            get { return bBind; }
        }

        public void Unbind()
        {
            GL.BindBuffer(bufferTarget, 0);
            bBind = false;
        }

        public void BufferData(IntPtr Size, IntPtr Data)
        {
            Bind();
            GL.BufferData(bufferTarget, Size, Data, hint);            
        }

        public void BufferData<T>(ref T Data) where T : struct
        {   
            Bind();
            var Size = new IntPtr(Marshal.SizeOf(Data));
            GL.BufferData<T>(bufferTarget, Size, ref Data, hint);            
        }
     
        public void BufferWholeData<T>(ref T Data) where T: struct
        {            
            Bind();
            GL.BufferSubData<T>(bufferTarget, new IntPtr(0), Marshal.SizeOf(Data), ref Data);
	    }

        public void BufferSubData<T>(ref T Data, int Offset) where T : struct
        {
            Bind();
            GL.BufferSubData<T>(bufferTarget, new IntPtr(Offset), Marshal.SizeOf(Data), ref Data);
        }

        public void GetBufferWholeData<T>(ref T Data) where T : struct
        {
            Bind();
            GL.GetBufferSubData<T>(bufferTarget, new IntPtr(0), Marshal.SizeOf(Data), ref Data);
        }

        public T GetBufferWholeData<T>() where T: struct
        {
            T result = new T();

            Bind();

            GL.GetBufferSubData<T>(bufferTarget, new IntPtr(0), Marshal.SizeOf(result), ref result);

            return result;
        }

        public void BufferData<T>(ref T[] Data) where T: struct
        {
            Bind();
            if (Data != null)
            {
                var Size = new IntPtr(Marshal.SizeOf(Data[0]) * Data.Length);
                GL.BufferData<T>(bufferTarget, Size, Data, hint);
            }
        }

        public IntPtr MapBuffer(BufferAccess access)
        {
            Bind();
            return GL.MapBuffer(bufferTarget, access);
        }
        
        public BufferUsageHint UsageHint
        {
            get { return hint; }
            set { hint = value; }
        }

        public BufferTarget Target 
        { 
            get { return bufferTarget; }
            protected set { bufferTarget = value; }
        }

        public int BufferObject
        {
            get { return bufferObject; }
            protected set { bufferObject = value; }
        }

        protected BufferTarget bufferTarget;

        protected BufferUsageHint hint;

        protected int bufferObject = 0;

        protected bool bBind = false;

        public string DebugName = "";
    }
}
