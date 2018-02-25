﻿using OpenTK.Graphics.OpenGL;
using Core.Texture;
using System.Diagnostics;
using System;
using Core.CustomEvent;

namespace Core.Buffer
{
    public class GBuffer
    {        
        public GBuffer(int width, int height)
        {
            BufferHeight = height;
            BufferWidth = width;
              
        }

        private void CreateGBuffer()
        {
            // 
            FrameBufferObject = new FrameBuffer();
            FrameBufferObject.Bind();

            PositionAttachment = new RenderTargetTexture(BufferWidth, BufferHeight);
            PositionAttachment.Bind();

            ColorAttachment = new RenderTargetTexture(BufferWidth, BufferHeight);
            ColorAttachment.Bind();

            NormalAttachment = new RenderTargetTexture(BufferWidth, BufferHeight);
            NormalAttachment.Bind();

            DepthAttachment = new DepthTargetTexture(BufferWidth, BufferHeight);
            DepthAttachment.Bind();

            GL.FramebufferTexture2D(FramebufferTarget.DrawFramebuffer, FramebufferAttachment.ColorAttachment0, TextureTarget.Texture2D, PositionAttachment.GetTextureObject, 0);
            GL.FramebufferTexture2D(FramebufferTarget.DrawFramebuffer, FramebufferAttachment.ColorAttachment1, TextureTarget.Texture2D, ColorAttachment.GetTextureObject, 0);
            GL.FramebufferTexture2D(FramebufferTarget.DrawFramebuffer, FramebufferAttachment.ColorAttachment2, TextureTarget.Texture2D, NormalAttachment.GetTextureObject, 0);
            GL.FramebufferTexture2D(FramebufferTarget.DrawFramebuffer, FramebufferAttachment.DepthStencilAttachment, TextureTarget.Texture2D, DepthAttachment.GetTextureObject, 0);

            var status = GL.CheckFramebufferStatus(FramebufferTarget.Framebuffer);

            Debug.Assert(status == FramebufferErrorCode.FramebufferComplete);

            FrameBufferObject.Unbind();
        }

        public void OnResourceCreate(object sender, EventArgs e)
        {
            CreateGBuffer();
        }

        public void OnWindowResized(object sender, ScreenResizeEventArgs e)
        {
            Resize(e.Width, e.Height);
        }

        public void Bind()
        {
            FrameBufferObject.Bind();
        }

        public void Unbind()
        {
            FrameBufferObject.Unbind();
        }

        public int ColorBufferObject => ColorAttachment.GetTextureObject;
        public int PositionBufferObject => PositionAttachment.GetTextureObject;
        public int NormalBufferObject => NormalAttachment.GetTextureObject;
        public int DepthBufferObject => DepthAttachment.GetTextureObject;

        public void PrepareToDraw()
        {
            GL.Viewport(0,0,BufferWidth, BufferHeight);
      
            var attachments = new DrawBuffersEnum[]
            {
                DrawBuffersEnum.ColorAttachment0,
                DrawBuffersEnum.ColorAttachment1,
                DrawBuffersEnum.ColorAttachment2,
            };
            
            GL.DrawBuffers(3, attachments);
        }

        protected void Clear()
        {
            GL.ClearColor(1, 1, 1, 1);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
        }

        private void Resize(int newWidth, int newHeight)
        {
            Debug.Assert(newWidth > 0 && newHeight > 0);

            FrameBufferObject.Bind();

            BufferWidth = newWidth;
            BufferHeight = newHeight;

            PositionAttachment.Resize(BufferWidth, BufferHeight);
            PositionAttachment.Bind();

            ColorAttachment.Resize(BufferWidth, BufferHeight);
            ColorAttachment.Bind();

            NormalAttachment.Resize(BufferWidth, BufferHeight);
            NormalAttachment.Bind();

            DepthAttachment.Resize(BufferWidth, BufferHeight);
            DepthAttachment.Bind();

            GL.FramebufferTexture2D(FramebufferTarget.DrawFramebuffer, FramebufferAttachment.ColorAttachment0, TextureTarget.Texture2D, PositionAttachment.GetTextureObject, 0);
            GL.FramebufferTexture2D(FramebufferTarget.DrawFramebuffer, FramebufferAttachment.ColorAttachment1, TextureTarget.Texture2D, ColorAttachment.GetTextureObject, 0);
            GL.FramebufferTexture2D(FramebufferTarget.DrawFramebuffer, FramebufferAttachment.ColorAttachment2, TextureTarget.Texture2D, NormalAttachment.GetTextureObject, 0);
            GL.FramebufferTexture2D(FramebufferTarget.DrawFramebuffer, FramebufferAttachment.DepthStencilAttachment, TextureTarget.Texture2D, DepthAttachment.GetTextureObject, 0);

            var status = GL.CheckFramebufferStatus(FramebufferTarget.Framebuffer);

            Debug.Assert(status == FramebufferErrorCode.FramebufferComplete);

            FrameBufferObject.Unbind();
        }

        protected RenderTargetTexture PositionAttachment = null;
        protected RenderTargetTexture ColorAttachment = null;
        protected RenderTargetTexture NormalAttachment = null;
        protected DepthTargetTexture DepthAttachment = null;

        protected Core.Buffer.FrameBuffer FrameBufferObject = null;

        protected int BufferWidth = 0;
        protected int BufferHeight = 0;
    }
}
