using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Core.Events
{
    public static class EvtRevive
    {
        public static event Action OnRequestSaveFirstPlayer;
        public static event Action<Vector2> OnPlayerPositionSet;


        public static void RaiseRequestSaveFirstPlayer()
            => OnRequestSaveFirstPlayer?.Invoke();
        public static void RaisePlayerPositionSet(Vector2 newPos) 
            => OnPlayerPositionSet?.Invoke(newPos);
    }
}
