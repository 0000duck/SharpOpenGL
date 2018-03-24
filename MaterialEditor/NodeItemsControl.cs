﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Collections.Specialized;
using System.Windows;

namespace MaterialEditor
{
    internal class NodeItemsControl : ListBox
    {
        public NodeItemsControl()
        {
            Focusable = false;
        }

        protected override DependencyObject GetContainerForItemOverride()
        {
            return new NodeItem();
        }

        protected override bool IsItemItsOwnContainerOverride(object item)
        {
            return item is NodeItem;
        }
    }
}
