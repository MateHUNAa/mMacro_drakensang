using System.Numerics;

namespace App.UI.EditSession
{
    public abstract class BaseEditSession
    {
        public bool Active { get; protected set; } = false;
        public Vector4 Color { get; protected set; } = new Vector4(255, 0, 0, 255);
        public Action? OnSessionEnd { get; set; }

        /// <summary>
        /// Called every frame to draw the session;
        /// </summary>
        /// <param name="mousePos"></param>
        public abstract void Draw(Vector2 mousePos);

        /// <summary>
        /// Called on mouse click
        /// </summary>
        /// <param name="mousePos"></param>
        public abstract void OnClick(Vector2 mousePos);

        /// <summary>
        /// Starts the edit session
        /// </summary>
        /// <param name="color"></param>
        public virtual void Start(Vector4? color = null)
        {
            if (color.HasValue) Color = color.Value;

            Active=true;
        }

        /// <summary>
        /// Stops the edit session
        /// </summary>
        public virtual void Stop()
        {
            Active=false;
            OnSessionEnd?.Invoke(); 
        }
    }
}
