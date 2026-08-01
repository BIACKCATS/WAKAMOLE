namespace Wakamole.Core.Utils
{
    public struct Timer
    {
        public readonly bool Active => _remained > 0;
        public float Current { readonly get => _remained; set => _remained = value; }
        public readonly float Duration => _duration;
        public readonly float Progress => (_duration <= 0f) ? 1f : 1f - (_remained / _duration);

        private readonly float _duration;
        private float _remained;

        public Timer(float duration)
        {
            _duration = duration;
            _remained = duration;
        }

        public void Tick(float delta)
        {
            if (!Active) return;
            _remained -= delta;
        }

        public void Restart() => _remained = _duration;
        public void Stop() => _remained = 0;
    }
}