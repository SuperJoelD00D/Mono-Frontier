using Robust.Client.GameObjects;
using Robust.Shared.GameObjects;
using Robust.Shared.IoC;
using Robust.Shared.Log;
using Content.Shared.GameMechanics;

namespace Content.Client.GameMechanics
{
    [RegisterComponent]
    public sealed class FatMechanicClient : Component
    {
        public override string Name => "FatMechanicClient";

        private SpriteComponent? _sprite;

        protected override void Initialize()
        {
            base.Initialize();

            // Try to get the SpriteComponent
            if (Owner.TryGetComponent(out SpriteComponent? sprite))
            {
                _sprite = sprite;
            }
            else
            {
                Logger.ErrorS("FatMechanicClient", $"SpriteComponent not found on entity {Owner}");
                return;
            }

            // Subscribe to events
            var entityManager = IoCManager.Resolve<IEntityManager>();
            entityManager.EventBus.SubscribeEvent<FatStatusAppliedEvent>(EventSource.Local, this, OnFatStatusApplied);
            entityManager.EventBus.SubscribeEvent<FatStatusRemovedEvent>(EventSource.Local, this, OnFatStatusRemoved);

            Logger.InfoS("FatMechanicClient", "Component initialized successfully.");
        }

        private void OnFatStatusApplied(FatStatusAppliedEvent args)
        {
            if (_sprite != null)
            {
                _sprite.Scale = (1.2f, 1.2f); // Scale the sprite
                Logger.InfoS("FatMechanicClient", "Fat status applied. Sprite scaled.");
            }
            else
            {
                Logger.ErrorS("FatMechanicClient", "SpriteComponent is null during FatStatusAppliedEvent.");
            }
        }

        private void OnFatStatusRemoved(FatStatusRemovedEvent args)
        {
            if (_sprite != null)
            {
                _sprite.Scale = (1.0f, 1.0f); // Reset sprite scale
                Logger.InfoS("FatMechanicClient", "Fat status removed. Sprite reset.");
            }
            else
            {
                Logger.ErrorS("FatMechanicClient", "SpriteComponent is null during FatStatusRemovedEvent.");
            }
        }

        protected override void Shutdown()
        {
            base.Shutdown();

            // Unsubscribe from events
            var entityManager = IoCManager.Resolve<IEntityManager>();
            entityManager.EventBus.UnsubscribeEvent<FatStatusAppliedEvent>(EventSource.Local, this);
            entityManager.EventBus.UnsubscribeEvent<FatStatusRemovedEvent>(EventSource.Local, this);

            Logger.InfoS("FatMechanicClient", "Component removed and unsubscribed from events.");
        }
    }
}