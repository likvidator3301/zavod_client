using Components;
using Leopotam.Ecs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Systems
{
    class EndGameSystem : IEcsRunSystem
    {
        private readonly EcsFilter<EndGameEvent> endFilter = null;
        private readonly EcsFilter<UiCanvasesComponent> uiCanvases = null;

        public void Run()
        {
            var endEvents = endFilter.Entities.Where(e => e.IsNotNullAndAlive());

            if (endEvents.Count() < 1)
                return;

            var isWin = endEvents.First().Get<EndGameEvent>().IsWin;

            if (isWin)
            {
                uiCanvases.Get1[0].WonMenu.enabled = true;
            }
            else
            {
                uiCanvases.Get1[0].LoseMenu.enabled = true;
            }
        }
    }
}
