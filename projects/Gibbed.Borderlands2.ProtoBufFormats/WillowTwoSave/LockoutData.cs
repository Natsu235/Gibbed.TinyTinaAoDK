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
    public class LockoutData : INotifyPropertyChanged
    {
        #region Fields
        private string _Lockout;
        private int _Time;
        private bool _IsFromDLC;
        private int _DLCPackageId;
        #endregion

        #region Properties
        [ProtoMember(1, IsRequired = true)]
        public string Lockout
        {
            get { return this._Lockout; }
            set
            {
                if (value != this._Lockout)
                {
                    this._Lockout = value;
                    this.NotifyOfPropertyChange(nameof(Lockout));
                }
            }
        }

        [ProtoMember(2, IsRequired = true)]
        public int Time
        {
            get { return this._Time; }
            set
            {
                if (value != this._Time)
                {
                    this._Time = value;
                    this.NotifyOfPropertyChange(nameof(Time));
                }
            }
        }

        [ProtoMember(3, IsRequired = true)]
        public bool IsFromDLC
        {
            get { return this._IsFromDLC; }
            set
            {
                if (value != this._IsFromDLC)
                {
                    this._IsFromDLC = value;
                    this.NotifyOfPropertyChange(nameof(IsFromDLC));
                }
            }
        }

        [ProtoMember(4, IsRequired = true)]
        public int DLCPackageId
        {
            get { return this._DLCPackageId; }
            set
            {
                if (value != this._DLCPackageId)
                {
                    this._DLCPackageId = value;
                    this.NotifyOfPropertyChange(nameof(DLCPackageId));
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
