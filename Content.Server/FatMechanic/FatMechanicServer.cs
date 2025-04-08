using Robust.Shared.GameObjects;
using Robust.Shared.IoC;

namespace Content.Server.GameMechanics
{
    public partial class FatMechanicServer : Component
    {

        private int _hunger;
        private bool _isFat;
        private float _fatTimer;

        public int Hunger
        {
            get => _hunger;
            set
            {
                _hunger = value;
                CheckFatStatus();
            }
        }

        public void Update(float frameTime)
        {
            if (_hunger >= 200)
            {
                _fatTimer += frameTime;

                if (_fatTimer >= 20 && !_isFat)
                {
                    ApplyFatStatus();
                }
            }
            else
            {
                _fatTimer = 0; // Reset timer if hunger drops below 200
            }
        }

        private void CheckFatStatus()
        {
            if (_hunger < 200 && _isFat)
            {
                RemoveFatStatus();
            }
        }

        private void ApplyFatStatus()
        {
            _isFat = true;

            // Resolve the IEntityManager
            var entityManager = IoCManager.Resolve<IEntityManager>();

            // Send a message to the client to update the sprite visuals
            entityManager.EventBus.RaiseEvent(EventSource.Local, new FatStatusAppliedEvent());
        }

        private void RemoveFatStatus()
        {
            _isFat = false;

            // Resolve the IEntityManager
            var entityManager = IoCManager.Resolve<IEntityManager>();

            // Send a message to the client to reset the sprite visuals
            entityManager.EventBus.RaiseEvent(EventSource.Local, new FatStatusRemovedEvent());
        }
    }

    public class FatStatusAppliedEvent : EntityEventArgs { }
    public class FatStatusRemovedEvent : EntityEventArgs { }
}