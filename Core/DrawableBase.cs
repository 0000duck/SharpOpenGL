﻿using Core.Buffer;
using System.Linq;
using OpenTK.Graphics.OpenGL;


namespace Core
{
    public class DrawableBase<T>  where T : struct 
    {
        public DrawableBase()
        {
            VB = new StaticVertexBuffer<T>();
            IB = new IndexBuffer();
        }

        public void SetupVertexData(ref T[] VertexList)
        {
            VB.Bind();
            VB.BufferData<T>(ref VertexList);
            VertexCount = VertexList.Count();

            bReadyToDraw = true;
        }

        protected void BindVertexBuffer()
        {
            VB.Bind();
            VB.BindVertexAttribute();
        }

        protected void BindIndexBuffer()
        {
            IB.Bind();
        }

        public void BindVertexAndIndexBuffer()
        {
            BindVertexBuffer();
            BindIndexBuffer();
        }

        public void SetupData(ref T[] VertexList, ref uint[] IndexList)
        {
            VB.Bind();
            VB.BufferData<T>(ref VertexList);
            VertexCount = VertexList.Count();

            IB.Bind();
            IB.BufferData<uint>(ref IndexList);
            IndexCount = IndexList.Count();

            bReadyToDraw = true;
        }

        public virtual void Draw(uint Offset, uint Count)
        {
            //
        }

        public virtual void Draw()
        {
            
        }

        public virtual void DrawPrimitiveWithoutIndex(PrimitiveType type)
        {
            if (bReadyToDraw)
            {
                BindVertexBuffer();
                GL.DrawArrays(type, 0, VertexCount);
            }
        }

        public virtual void DrawPrimitive(PrimitiveType type)
        {
            if(bReadyToDraw)
            {
                BindVertexAndIndexBuffer();
                GL.DrawElements(type, IndexCount, DrawElementsType.UnsignedInt, 0);
            }
        }
        
        public virtual void DrawLinesPrimitive()
        {
            if(bReadyToDraw)
            {
                BindVertexAndIndexBuffer();
                GL.DrawElements(PrimitiveType.Lines, IndexCount, DrawElementsType.UnsignedInt, 0);
            }
        }

        public virtual void DrawTrianglesPrimitive()
        {
            if(bReadyToDraw)
            {
                BindVertexAndIndexBuffer();
                GL.DrawElements(PrimitiveType.Triangles, IndexCount, DrawElementsType.UnsignedInt, 0);
            }
        }

        public virtual void DrawLineStripPrimitive()
        {
            if(bReadyToDraw)
            {
                BindVertexAndIndexBuffer();
                GL.DrawElements(PrimitiveType.LineStrip, IndexCount, DrawElementsType.UnsignedInt, 0);
            }
        }
        

        protected StaticVertexBuffer<T> VB = null;
        protected IndexBuffer IB = null;

        protected int VertexCount = 0;
        protected int IndexCount = 0;
        protected bool bReadyToDraw = false;
    }
}
