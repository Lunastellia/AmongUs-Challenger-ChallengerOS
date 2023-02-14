using System;
using UnityEngine;

namespace ChallengerOS.RainbowPlugin
{
    public class RainbowBehaviour : MonoBehaviour
    {
        public Renderer Renderer;
        public int Id;

        public void AddRend(Renderer rend, int id)
        {
            Renderer = rend;
            Id = id;
        }

        public void Update()
        {
            if (Renderer == null) return;

            if (RainbowUtils.IsRainbow(Id))
            {
                RainbowUtils.SetRainbow(Renderer);
            }
            if (RainbowUtils.IsRed(Id))
            {
                RainbowUtils.SetColorRed(Renderer);
            }
        }

        public RainbowBehaviour(IntPtr ptr) : base(ptr) { }
    }
}
