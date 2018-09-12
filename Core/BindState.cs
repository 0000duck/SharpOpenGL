﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core
{
    public class BindState
    {
        protected static BindState Instance = new BindState();
        public static BindState Get()
        {
            return Instance;
        }

        public void SetCurrentVertexBufferBound(string vertexBufferName)
        {
            CurrentVertexBufferBound = vertexBufferName;
        }

        protected string CurrentVertexBufferBound = "";
        protected string CurrentIndexBufferBound = "";
    }
}
