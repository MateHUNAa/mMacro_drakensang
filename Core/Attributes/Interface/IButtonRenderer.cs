using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Core.Attributes.Interface
{
    public interface IButtonRenderer
    {
        void Draw(string label, Action onClick, Vector2 size);
    }
}
