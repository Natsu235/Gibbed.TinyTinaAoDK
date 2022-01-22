﻿/* Copyright (c) 2019 Rick (rick 'at' gibbed 'dot' us)
 *
 * This software is provided 'as-is', without any express or implied
 * warranty. In no event will the authors be held liable for any damages
 * arising from the use of this software.
 *
 * Permission is granted to anyone to use this software for any purpose,
 * including commercial applications, and to alter it and redistribute it
 * freely, subject to the following restrictions:
 *
 * 1. The origin of this software must not be misrepresented; you must not
 *    claim that you wrote the original software. If you use this software
 *    in a product, an acknowledgment in the product documentation would
 *    be appreciated but is not required.
 *
 * 2. Altered source versions must be plainly marked as such, and must not
 *    be misrepresented as being the original software.
 *
 * 3. This notice may not be removed or altered from any source
 *    distribution.
 */

using System.ComponentModel;
using ProtoBuf;

namespace Gibbed.Borderlands2.ProtoBufFormats.WillowTwoSave
{
    [ProtoContract]
    public class PackedWeaponData : INotifyPropertyChanged
    {
        #region Fields
        private byte[] _InventorySerialNumber;
        private QuickWeaponSlot _QuickSlot;
        private PlayerMark _Mark;
        private int _Unknown4;
        #endregion

        #region Properties
        [ProtoMember(1, IsRequired = true)]
        public byte[] InventorySerialNumber
        {
            get { return this._InventorySerialNumber; }
            set
            {
                if (value != this._InventorySerialNumber)
                {
                    this._InventorySerialNumber = value;
                    this.NotifyOfPropertyChange(nameof(InventorySerialNumber));
                }
            }
        }

        [ProtoMember(2, IsRequired = true)]
        public QuickWeaponSlot QuickSlot
        {
            get { return this._QuickSlot; }
            set
            {
                if (value != this._QuickSlot)
                {
                    this._QuickSlot = value;
                    this.NotifyOfPropertyChange(nameof(QuickSlot));
                }
            }
        }

        [ProtoMember(3, IsRequired = true)]
        public PlayerMark Mark
        {
            get { return this._Mark; }
            set
            {
                if (value != this._Mark)
                {
                    this._Mark = value;
                    this.NotifyOfPropertyChange(nameof(Mark));
                }
            }
        }

        [ProtoMember(4, IsRequired = false)]
        public int Unknown4
        {
            get { return this._Unknown4; }
            set
            {
                if (value != this._Unknown4)
                {
                    this._Unknown4 = value;
                    this.NotifyOfPropertyChange(nameof(Unknown4));
                }
            }
        }
        #endregion

        #region INotifyPropertyChanged Members
        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyOfPropertyChange(string propertyName)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}
