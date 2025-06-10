using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LolHubMexico.Domain.Entities.Scrims;

namespace LolHubMexico.Application.dessingPatterns
{
    public class ScrimProcessingQueue
    {
        private readonly PriorityQueue<Scrim, DateTime> _queue = new();

        public void Enqueue(Scrim scrim)
        {
            _queue.Enqueue(scrim, scrim.scheduled_date);
        }

        public bool TryDequeue(out Scrim? scrim)
        {
            if (_queue.TryDequeue(out scrim, out _))
                return true;

            scrim = null;
            return false;
        }

        public bool IsEmpty => _queue.Count == 0;
    }
}
