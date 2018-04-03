﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaterialEditor.Operator
{
    public class Vector3MultiplyFloat : NodeViewModel
    {
        public Vector3MultiplyFloat()
            : base("Vector3 x Float")
        {

        }

       

        protected override bool IsConnectionValidForEvaluation()
        {
            if (InputConnectors[0].AttachedConnections.Count == 1 && InputConnectors[1].AttachedConnections.Count == 1)
            {
                return true;
            }

            return false;
        }

        public override string GetExpressionForOutput(int outputIndex)
        {
            if (IsConnectionValidForEvaluation())
            {
                var expressionA = GetExpressionForInput(0);
                var expressionB = GetExpressionForInput(1);
                return string.Format("{0}*{1}", expressionA, expressionB);
            }

            return string.Empty;
        }

        protected override void CreateInputOutputConnectors()
        {
            // input
            this.InputConnectors.Add(new ConnectorViewModel("Vec3", ConnectorDataType.ConstantVector3, 0));
            this.InputConnectors.Add(new ConnectorViewModel("Float", ConnectorDataType.ConstantFloat, 1));

            // output
            this.OutputConnectors.Add(new ConnectorViewModel("Out", ConnectorDataType.ConstantVector3, 0));
        }
    }
}
