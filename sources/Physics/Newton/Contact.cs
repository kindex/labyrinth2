using System;

namespace Game.Physics.Newton
{
    public struct Contact
    {
        internal Contact(IntPtr contact)
        {
            this.handle = contact;
        }

        public MaterialCollision material
        {
            get
            {
                return new MaterialCollision(NativeAPI.ContactGetMaterial(handle));
            }
        }

        internal IntPtr handle;
    }
}
