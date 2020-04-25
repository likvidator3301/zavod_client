using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerCommunication
{
    public class SessionInfo
    {
        public bool IsCreator = false;
        public SessionDto CurrentSessionInfo;
        public Guid CurrentSessionGuid = Guid.Empty;
        public List<SessionDto> AllSessions = new List<SessionDto>();
    }
}
