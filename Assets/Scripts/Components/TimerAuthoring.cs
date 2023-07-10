using Unity.Entities;

namespace Components
{
    public struct Timer : IComponentData
    {
        public float remainingTime;
        public float interval;

        public Timer(float duration) : this()
        {
            SetDuration(duration);
        }
        
        public bool IsDone {
            get {
                return this.remainingTime <= 0;
            }
        }

        public void Reset()
        {
            SetDuration(interval);
        }
        
        private void SetDuration(float targetDuration) {
            this.remainingTime = targetDuration;
            this.interval = targetDuration;
        }
    }
}