﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System.Threading.Tasks;

namespace Core.Primitive
{
    public class Cone
    {
        public Cone(float radius, float height, uint count)
        {
            Radius = radius;
            Height = height;
            Count = count;
        }

        protected void GenerateVertices()
        {
            var bottomCenter = new PNC_VertexAttribute(new Vector3(0, 0, 0), -Vector3.UnitX, Color);

            for (var i = 0; i < Count; ++i)
            {
                // add center
                VertexList.Add(bottomCenter);

                var rad1 = OpenTK.MathHelper.DegreesToRadians((360 / (double)Count) * i);
                var y1 = Radius * Math.Cos(rad1);
                var z1 = Radius * Math.Sin(rad1);
                var position1 = new Vector3(0, (float)y1, (float)z1);
                var normal1 = -Vector3.UnitX;

                // add V1
                VertexList.Add(new PNC_VertexAttribute(position1, normal1, Color));

                var rad2 = OpenTK.MathHelper.DegreesToRadians((360 / (double)Count) * (i + 1));
                var y2 = Radius * Math.Cos(rad2);
                var z2 = Radius * Math.Sin(rad2);
                var position2 = new Vector3(0, (float)y2, (float)z2);
                var normal2 = -Vector3.UnitX;

                // add V2
                VertexList.Add(new PNC_VertexAttribute(position2, normal2, Color));
            }

            var topCenter = new PNC_VertexAttribute(new Vector3(Height, 0, 0), Vector3.UnitX, Color);



        }

        protected List<PNC_VertexAttribute> VertexList = new List<PNC_VertexAttribute>();

        protected float Radius = 1.0f;
        protected float Height = 10.0f;
        protected uint Count = 10;
        protected Vector3 Color = new Vector3(1, 0, 0);
    }
}
