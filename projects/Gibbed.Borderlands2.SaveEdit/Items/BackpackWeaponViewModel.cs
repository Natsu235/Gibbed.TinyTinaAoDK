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

using Gibbed.Borderlands2.FileFormats.Items;
using Gibbed.Borderlands2.ProtoBufFormats.WillowTwoSave;

namespace Gibbed.Borderlands2.SaveEdit
{
    internal class BackpackWeaponViewModel : BaseWeaponViewModel, IBackpackSlotViewModel
    {
        private readonly BackpackWeapon _BackpackWeapon;

        public BackpackWeaponViewModel(BackpackWeapon weapon)
            : base(weapon)
        {
            this._BackpackWeapon = weapon;
        }

        #region Properties
        public IBackpackSlot BackpackSlot
        {
            get { return this._BackpackWeapon; }
        }

        public BackpackWeapon BackpackWeapon
        {
            get { return this._BackpackWeapon; }
        }

        public QuickWeaponSlot QuickSlot
        {
            get { return this._BackpackWeapon.QuickSlot; }
            set
            {
                this._BackpackWeapon.QuickSlot = value;
                this.NotifyOfPropertyChange(nameof(QuickSlot));
                this.NotifyOfPropertyChange(nameof(DisplayGroup));
            }
        }

        public PlayerMark Mark
        {
            get { return this._BackpackWeapon.Mark; }
            set
            {
                this._BackpackWeapon.Mark = value;
                this.NotifyOfPropertyChange(nameof(Mark));
            }
        }
        #endregion

        #region Display Properties
        public override string DisplayGroup
        {
            get
            {
                if (this.QuickSlot != QuickWeaponSlot.None)
                {
                    return "Equipped";
                }

                return base.DisplayGroup;
            }
        }
        #endregion
    }
}
