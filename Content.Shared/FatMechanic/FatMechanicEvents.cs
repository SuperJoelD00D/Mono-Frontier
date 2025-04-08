using Robust.Shared.GameObjects;

namespace Content.Shared.GameMechanics
{
    // Event triggered when the fat status is applied
    public sealed class FatStatusAppliedEvent : EntityEventArgs { }

    // Event triggered when the fat status is removed
    public sealed class FatStatusRemovedEvent : EntityEventArgs { }
}