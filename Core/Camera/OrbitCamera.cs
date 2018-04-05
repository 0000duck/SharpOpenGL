﻿using OpenTK;
using System;
using Core.CustomEvent;


namespace Core.Camera
{
    public class OrbitCamera : CameraBase
    {
        public OrbitCamera()
        { }

        public OrbitCamera(float fFOV, float fAspectRatio, float fNear, float fFar)            
            : base(fFOV, fAspectRatio, fNear, fFar)
        {            
        }

        public override void Tick(double fDeltaSeconds)
        {
            var vDir = (DestEyeLocation - EyeLocation).Normalized();

            if(vDir.Length > 1)
            {
                EyeLocation = vDir * 0.01f + EyeLocation;                
            }
            else
            {
                EyeLocation = DestEyeLocation;
            }

            UpdateViewMatrix();
            UpdateProjMatrix();
        }

        public override void UpdateViewMatrix()
        {
            ViewMatrix = Matrix4.LookAt(EyeLocation, LookAtLocation, Vector3.UnitY);
        }

        public void MoveForward(float fAmount)
        {
            var vDir = (LookAtLocation - EyeLocation).Normalized();

            EyeLocation = vDir * fAmount + EyeLocation;
            UpdateViewMatrix();            
        }

        public float CameraDistance = 10;
        public float DestCameraDistance = 10;

        public Vector3 DestLocation
        {
            get { return DestEyeLocation; }
            set { DestEyeLocation = value;}
        }

        public void OnWindowResized(object sender, ScreenResizeEventArgs eventArgs)
        {
            var Width = eventArgs.Width;
            var Height = eventArgs.Height;

            float fAspectRatio = Width / (float)Height;

            AspectRatio = fAspectRatio;
            FOV = OpenTK.MathHelper.PiOver6;
            Near = 1;
            Far = 10000;

            EyeLocation = new Vector3(5, 5, 5);
            DestLocation = new Vector3(5, 5, 5);
            LookAtLocation = new Vector3(0, 0, 0);
        }

        protected Vector3 DestEyeLocation = new Vector3();
    }    
}
