using System;
using System.Collections.Generic;

namespace Game.Physics.Newton
{
    public delegate void ContactIterator(Contact contact);

    public struct ContactJoint
    {
        internal ContactJoint(IntPtr newtonContactJoint)
        {
            this.handle = newtonContactJoint;
        }

        public void RemoveContact(Contact contact)
        {
            NativeAPI.ContactJointRemoveContact(handle, contact.handle);
        }

        public IEnumerator<Contact> Contacts
        {
            get
            {
                IntPtr newtonContact = NativeAPI.ContactJointGetFirstContact(handle);
                while (newtonContact != IntPtr.Zero)
                {
                    yield return new Contact(newtonContact);
                    newtonContact = NativeAPI.ContactJointGetNextContact(handle, newtonContact);
                }
            }
        }

        public int ContactJointCount
        {
            get
            {
                return NativeAPI.ContactJointGetContactCount(handle);
            }
        }

        IntPtr handle;
    }
}