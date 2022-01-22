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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Caliburn.Micro;
using Gibbed.Borderlands2.FileFormats.Items;
using Gibbed.Borderlands2.GameInfo;

namespace Gibbed.Borderlands2.SaveEdit
{
    internal class BaseWeaponViewModel : PropertyChangedBase, IBaseSlotViewModel
    {
        private static readonly EnumerableComparer<object> _AssetListComparer;

        static BaseWeaponViewModel()
        {
            _AssetListComparer = new EnumerableComparer<object>();
        }

        private readonly BaseWeapon _Weapon;
        private string _DisplayName;

        public IPackableSlot BaseSlot
        {
            get { return this._Weapon; }
        }

        public BaseWeaponViewModel(BaseWeapon weapon)
        {
            this._Weapon = weapon ?? throw new ArgumentNullException(nameof(weapon));

            this.WeaponTypeAssets = CreateAssetList(InfoManager.WeaponBalance.Items
                .Where(kv => kv.Value.WeaponType != null)
                .Select(kv => kv.Value.WeaponType.ResourcePath));
            this.BuildBalanceAssets();
            this.UpdateDisplayName();
        }

        private static string GenerateDisplayName(string weaponTypePath, string prefixPartPath, string titlePartPath)
        {
            if (titlePartPath != "None" &&
                InfoManager.WeaponNameParts.TryGetValue(titlePartPath, out var titlePart) == true &&
                string.IsNullOrEmpty(titlePart.Name) == false)
            {
                var text = titlePart.Name;

                if (prefixPartPath != "None" &&
                    InfoManager.WeaponNameParts.TryGetValue(prefixPartPath, out var prefixPart) == true &&
                    string.IsNullOrEmpty(prefixPart.Name) == false)
                {
                    text = prefixPart.Name + " " + text;
                }

                return text;
            }

            return "Unknown Weapon";
        }

        private void UpdateDisplayName()
        {
            this.DisplayName = GenerateDisplayName(this.WeaponType, this.PrefixPart, this.TitlePart);
        }

        #region Properties
        public string WeaponType
        {
            get { return this._Weapon.WeaponType; }
            set
            {
                this._Weapon.WeaponType = value;
                this.NotifyOfPropertyChange(nameof(WeaponType));
                this.BuildBalanceAssets();
                this.UpdateDisplayName();
            }
        }

        public string Balance
        {
            get { return this._Weapon.Balance; }
            set
            {
                this._Weapon.Balance = value;
                this.NotifyOfPropertyChange(nameof(Balance));
                this.BuildPartAssets();
            }
        }

        public string Manufacturer
        {
            get { return this._Weapon.Manufacturer; }
            set
            {
                this._Weapon.Manufacturer = value;
                this.NotifyOfPropertyChange(nameof(Manufacturer));
            }
        }

        public int ManufacturerGradeIndex
        {
            get { return this._Weapon.ManufacturerGradeIndex; }
            set
            {
                this._Weapon.ManufacturerGradeIndex = value;
                this.NotifyOfPropertyChange(nameof(ManufacturerGradeIndex));
            }
        }

        public string BodyPart
        {
            get { return this._Weapon.BodyPart; }
            set
            {
                this._Weapon.BodyPart = value;
                this.NotifyOfPropertyChange(nameof(BodyPart));
            }
        }

        public string GripPart
        {
            get { return this._Weapon.GripPart; }
            set
            {
                this._Weapon.GripPart = value;
                this.NotifyOfPropertyChange(nameof(GripPart));
            }
        }

        public string BarrelPart
        {
            get { return this._Weapon.BarrelPart; }
            set
            {
                this._Weapon.BarrelPart = value;
                this.NotifyOfPropertyChange(nameof(BarrelPart));
            }
        }

        public string SightPart
        {
            get { return this._Weapon.SightPart; }
            set
            {
                this._Weapon.SightPart = value;
                this.NotifyOfPropertyChange(nameof(SightPart));
            }
        }

        public string StockPart
        {
            get { return this._Weapon.StockPart; }
            set
            {
                this._Weapon.StockPart = value;
                this.NotifyOfPropertyChange(nameof(StockPart));
            }
        }

        public string ElementalPart
        {
            get { return this._Weapon.ElementalPart; }
            set
            {
                this._Weapon.ElementalPart = value;
                this.NotifyOfPropertyChange(nameof(ElementalPart));
            }
        }

        public string Accessory1Part
        {
            get { return this._Weapon.Accessory1Part; }
            set
            {
                this._Weapon.Accessory1Part = value;
                this.NotifyOfPropertyChange(nameof(Accessory1Part));
            }
        }

        public string Accessory2Part
        {
            get { return this._Weapon.Accessory2Part; }
            set
            {
                this._Weapon.Accessory2Part = value;
                this.NotifyOfPropertyChange(nameof(Accessory2Part));
            }
        }

        public string MaterialPart
        {
            get { return this._Weapon.MaterialPart; }
            set
            {
                this._Weapon.MaterialPart = value;
                this.NotifyOfPropertyChange(nameof(MaterialPart));
            }
        }

        public string PrefixPart
        {
            get { return this._Weapon.PrefixPart; }
            set
            {
                this._Weapon.PrefixPart = value;
                this.NotifyOfPropertyChange(nameof(PrefixPart));
                this.UpdateDisplayName();
            }
        }

        public string TitlePart
        {
            get { return this._Weapon.TitlePart; }
            set
            {
                this._Weapon.TitlePart = value;
                this.NotifyOfPropertyChange(nameof(TitlePart));
                this.UpdateDisplayName();
            }
        }

        public int GameStage
        {
            get { return this._Weapon.GameStage; }
            set
            {
                this._Weapon.GameStage = value;
                this.NotifyOfPropertyChange(nameof(GameStage));
            }
        }

        public int UniqueId
        {
            get { return this._Weapon.UniqueId; }
            set
            {
                this._Weapon.UniqueId = value;
                this.NotifyOfPropertyChange(nameof(UniqueId));
            }
        }

        public int AssetLibrarySetId
        {
            get { return this._Weapon.AssetLibrarySetId; }
            set
            {
                this._Weapon.AssetLibrarySetId = value;
                this.NotifyOfPropertyChange(nameof(AssetLibrarySetId));
            }
        }
        #endregion

        #region Display Properties
        public string AssetLibrarySetName
        {
            get
            {
                if (this.AssetLibrarySetId == 0)
                {
                    return "Base Game";
                }

                var package = InfoManager.DownloadablePackages.Items
                                         .Select(kv => kv.Value)
                                         .FirstOrDefault(dp => dp.Id == this.AssetLibrarySetId);
                if (package == null)
                {
                    return $"(unknown #{this.AssetLibrarySetId})";
                }

                return $"{package.DisplayName} (#{this.AssetLibrarySetId})";
            }
        }

        public virtual string DisplayName
        {
            get { return this._DisplayName; }
            private set
            {
                this._DisplayName = value;
                this.NotifyOfPropertyChange(nameof(DisplayName));
            }
        }

        public virtual string DisplayGroup
        {
            get { return "Weapons"; }
        }
        #endregion

        #region Assets
        private static IEnumerable<string> CreateAssetList(IEnumerable<string> items)
        {
            var list = new List<string>()
            {
                "None",
            };

            if (items != null)
            {
                object convert(string str)
                {
                    if (int.TryParse(str, out var value) == true)
                    {
                        return value;
                    }
                    return str;
                }
                list.AddRange(items
                    .Distinct()
                    .OrderBy(s => Regex.Split(s.Replace(" ", ""), "([0-9]+)").Select(convert), _AssetListComparer));
            }

            return list;
        }

        #region Fields
        private IEnumerable<string> _BalanceAssets;
        private IEnumerable<string> _ManufacturerAssets;
        private IEnumerable<string> _BodyPartAssets;
        private IEnumerable<string> _GripPartAssets;
        private IEnumerable<string> _BarrelPartAssets;
        private IEnumerable<string> _SightPartAssets;
        private IEnumerable<string> _StockPartAssets;
        private IEnumerable<string> _ElementalPartAssets;
        private IEnumerable<string> _Accessory1PartAssets;
        private IEnumerable<string> _Accessory2PartAssets;
        private IEnumerable<string> _MaterialPartAssets;
        #endregion

        #region Properties
        public IEnumerable<string> WeaponTypeAssets { get; private set; }

        public IEnumerable<string> BalanceAssets
        {
            get { return this._BalanceAssets; }
            private set
            {
                this._BalanceAssets = value;
                this.NotifyOfPropertyChange(nameof(BalanceAssets));
            }
        }

        public IEnumerable<string> ManufacturerAssets
        {
            get { return this._ManufacturerAssets; }
            private set
            {
                this._ManufacturerAssets = value;
                this.NotifyOfPropertyChange(nameof(ManufacturerAssets));
            }
        }

        public IEnumerable<string> BodyPartAssets
        {
            get { return this._BodyPartAssets; }
            private set
            {
                this._BodyPartAssets = value;
                this.NotifyOfPropertyChange(nameof(BodyPartAssets));
            }
        }

        public IEnumerable<string> GripPartAssets
        {
            get { return this._GripPartAssets; }
            private set
            {
                this._GripPartAssets = value;
                this.NotifyOfPropertyChange(nameof(GripPartAssets));
            }
        }

        public IEnumerable<string> BarrelPartAssets
        {
            get { return this._BarrelPartAssets; }
            private set
            {
                this._BarrelPartAssets = value;
                this.NotifyOfPropertyChange(nameof(BarrelPartAssets));
            }
        }

        public IEnumerable<string> SightPartAssets
        {
            get { return this._SightPartAssets; }
            private set
            {
                this._SightPartAssets = value;
                this.NotifyOfPropertyChange(nameof(SightPartAssets));
            }
        }

        public IEnumerable<string> StockPartAssets
        {
            get { return this._StockPartAssets; }
            private set
            {
                this._StockPartAssets = value;
                this.NotifyOfPropertyChange(nameof(StockPartAssets));
            }
        }

        public IEnumerable<string> ElementalPartAssets
        {
            get { return this._ElementalPartAssets; }
            private set
            {
                this._ElementalPartAssets = value;
                this.NotifyOfPropertyChange(nameof(ElementalPartAssets));
            }
        }

        public IEnumerable<string> Accessory1PartAssets
        {
            get { return this._Accessory1PartAssets; }
            private set
            {
                this._Accessory1PartAssets = value;
                this.NotifyOfPropertyChange(nameof(Accessory1PartAssets));
            }
        }

        public IEnumerable<string> Accessory2PartAssets
        {
            get { return this._Accessory2PartAssets; }
            private set
            {
                this._Accessory2PartAssets = value;
                this.NotifyOfPropertyChange(nameof(Accessory2PartAssets));
            }
        }

        public IEnumerable<string> MaterialPartAssets
        {
            get { return this._MaterialPartAssets; }
            private set
            {
                this._MaterialPartAssets = value;
                this.NotifyOfPropertyChange(nameof(MaterialPartAssets));
            }
        }
        #endregion

        private readonly string[] _NoneAssets = new[]
        {
            "None"
        };

        private void BuildBalanceAssets()
        {
            if (InfoManager.WeaponTypes.TryGetValue(this.WeaponType, out var weaponType) == false)
            {
                this.BalanceAssets = CreateAssetList(null);
            }
            else
            {
                this.BalanceAssets = CreateAssetList(InfoManager.WeaponBalance.Items
                    .Where(bd => bd.Value.IsSuitableFor(weaponType) == true)
                    .Select(kv => kv.Key));
            }

            this.BuildPartAssets();
        }

        private void BuildPartAssets()
        {
            if (InfoManager.WeaponTypes.TryGetValue(this.WeaponType, out var weaponType) == false ||
                this.BalanceAssets.Contains(this.Balance) == false ||
                InfoManager.WeaponBalance.TryGetValue(this.Balance, out var weaponTypeBalance) == false ||
                this.Balance == "None")
            {
                this.ManufacturerAssets = _NoneAssets;
                this.BodyPartAssets = _NoneAssets;
                this.BodyPartAssets = _NoneAssets;
                this.GripPartAssets = _NoneAssets;
                this.BarrelPartAssets = _NoneAssets;
                this.SightPartAssets = _NoneAssets;
                this.StockPartAssets = _NoneAssets;
                this.ElementalPartAssets = _NoneAssets;
                this.Accessory1PartAssets = _NoneAssets;
                this.Accessory2PartAssets = _NoneAssets;
                this.MaterialPartAssets = _NoneAssets;
            }
            else
            {
                var balance = weaponTypeBalance.Create(weaponType);
                this.ManufacturerAssets = CreateAssetList(balance.Manufacturers);
                this.BodyPartAssets = CreateAssetList(balance.Parts.BodyParts);
                this.GripPartAssets = CreateAssetList(balance.Parts.GripParts);
                this.BarrelPartAssets = CreateAssetList(balance.Parts.BarrelParts);
                this.SightPartAssets = CreateAssetList(balance.Parts.SightParts);
                this.StockPartAssets = CreateAssetList(balance.Parts.StockParts);
                this.ElementalPartAssets = CreateAssetList(balance.Parts.ElementalParts);
                this.Accessory1PartAssets = CreateAssetList(balance.Parts.Accessory1Parts);
                this.Accessory2PartAssets = CreateAssetList(balance.Parts.Accessory2Parts);
                this.MaterialPartAssets = CreateAssetList(balance.Parts.MaterialParts);
            }
        }
        #endregion
    }
}
