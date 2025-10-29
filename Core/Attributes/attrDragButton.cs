using Core.Attributes.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Attributes
{
    public class DragButtonAttribute : Attribute, IButtonTemplate
    {
        public string Label { get; }

        public float Width { get; }

        public float Height { get; }

        public bool Inline { get; }

        public int Columns { get; }

        public DragButtonAttribute(string label = "Drag button", float width = 0, float height = 0, bool inline = true, int columns = 1)
        {
            Label = label;
            Width = width;
            Height = height;
            Inline = inline;
            Columns = columns;
        }
    }
}
