﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;

namespace MaterialEditor
{
    public class ConstantVector4Node : NodeViewModel
    {
        protected OpenTK.Vector4 vec4;

        public ConstantVector4Node()
        {
            this.OutputConnectors.Add(new ConnectorViewModel("Out"));
        }

        public ConstantVector4Node(string name)
            : base(name)
        {
            this.OutputConnectors.Add(new ConnectorViewModel("Out"));
        }

        public override string ToExpression()
        {
            return string.Format("vec4({0}, {1}, {2}, {3})", XValue, YValue, ZValue, WValue);
        }

        public float XValue
        {
            get
            {
                return vec4.X;
            }
            set
            {
                vec4.X = value;
                OnPropertyChanged("XValue");
            }
        }

        public float YValue
        {
            get
            {
                return vec4.Y;
            }
            set
            {
                vec4.Y = value;
                OnPropertyChanged("YValue");
            }
        }

        public float ZValue
        {
            get { return vec4.Z; }
            set
            {
                vec4.Z = value;
                OnPropertyChanged("ZValue");
            }
        }



        public float WValue
        {
            get
            {
                return vec4.W;
            }

            set
            {
                vec4.W = value;
                OnPropertyChanged("WValue");
            }
        }
    }
}
