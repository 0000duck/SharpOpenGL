﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaterialEditor
{
    public class ConstantFloatNode : NodeViewModel
    {
        public ConstantFloatNode()
            : base("Float")
        { }


        public override string GetExpressionForOutput(int outputIndex)
        {
            if(outputIndex == 0)
            {
                return string.Format("({0})", Value);
            }

            return string.Empty;
        }
        protected override void CreateInputOutputConnectors()
        {
            base.CreateInputOutputConnectors();
            // output
            OutputConnectors.Add(new ConnectorViewModel("Out", ConnectorDataType.ConstantFloat,0));
        }

        protected float floatValue = 0;

        public float Value
        {
            get { return floatValue; }
            set
            {
                floatValue = value;
                OnPropertyChanged("Value");
            }
        }
    }

}
