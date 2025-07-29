// Copyright (c) 2023 Justin Couch / JustInvoke

namespace PowerslideKartPhysics
{
    // Class for boost item
    public class ShieldItem : Item
    {
        public float ShieldDuration = 1.0f;
        public float ShieldForce = 1.0f;

        // Award boost to kart upon activation
        public override void Activate(ItemCastProperties props)
        {
            base.Activate(props);
            if (props.castKart != null)
            {
                if (props.castKart.canBoost)
                {
                    props.castKart.AddShield(ShieldDuration);
                    props.castKart.shieldStartEvent.Invoke();
                }
            }
        }
    }
}