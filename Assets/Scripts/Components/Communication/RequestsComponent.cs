using Leopotam.Ecs;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Components.Communication
{
    public class RequestsComponent
    {
        [EcsIgnoreNullCheck]
        public List<MoveUnitDto> moveRequests;
    }
}
