﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Texture;
using Core.CustomEvent;
using OpenTK.Graphics;
using OpenTK;

namespace SharpOpenGL.PostProcess
{
    public class DeferredLight : SharpOpenGL.PostProcess.PostProcessBase
    {
        public DeferredLight()
            : base()
        {
        }

        public override void OnWindowResized(object sender, ScreenResizeEventArgs e)
        {
            base.OnWindowResized(sender, e);            
        }

        public override void OnResourceCreate(object sender, EventArgs e)
        {
            base.OnResourceCreate(sender, e);

            PostProcessMaterial = new SharpOpenGL.LightMaterial.LightMaterial();
            m_LightInfo.LightAmbient = new OpenTK.Vector3(0.3f, 0.3f, 0.3f);
            m_LightInfo.LightDiffuse = new OpenTK.Vector3(1, 0.0f, 0.0f);
            m_LightInfo.LightDir = new OpenTK.Vector3(1,1, 1);
        }

        public override void Render(TextureBase positionInput, TextureBase colorInput, TextureBase normalInput)
        {
            PostProcessMaterial.Setup();
            PostProcessMaterial.SetTexture("PositionTex", positionInput);
            PostProcessMaterial.SetTexture("DiffuseTex", colorInput);
            PostProcessMaterial.SetTexture("NormalTex", normalInput);

            PostProcessMaterial.SetUniformBufferValue<SharpOpenGL.LightMaterial.Light>("Light", ref m_LightInfo);           

            Output.PrepareToDraw();

            BlitToScreenSpace();

            Output.Unbind();
        }

        //

        protected SharpOpenGL.LightMaterial.Light m_LightInfo = new LightMaterial.Light();
    }
}
