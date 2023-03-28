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
                RainbowUtils.SetRainbowVisor(Renderer);
            }
            if (RainbowUtils.IsRuby(Id))
            {
                RainbowUtils.SetRubyVisor(Renderer);
            }
            if (RainbowUtils.IsAmber(Id))
            {
                RainbowUtils.SetAmberVisor(Renderer);
            }
            if (RainbowUtils.IsEmerald(Id))
            {
                RainbowUtils.SetEmeraldVisor(Renderer);
            }
            if (RainbowUtils.IsLarimar(Id))
            {
                RainbowUtils.SetLarimarVisor(Renderer);
            }
            if (RainbowUtils.IsSapphir(Id))
            {
                RainbowUtils.SetSapphirVisor(Renderer);
            }
            if (RainbowUtils.IsQuartz(Id))
            {
                RainbowUtils.SetQuartzVisor(Renderer);
            }
            if (RainbowUtils.IsNormalColor(Id))
            {
                RainbowUtils.SetNormalColorVisor(Renderer);
            }
        }

        public RainbowBehaviour(IntPtr ptr) : base(ptr) { }
    }
}
