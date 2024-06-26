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
using Gibbed.TinyTinaAoDK.GameInfo;
using Gibbed.Gearbox.Common;

namespace Gibbed.TinyTinaAoDK.FileFormats.Items
{
    public class PackedItem : IPackedSlot, INotifyPropertyChanged
    {
        #region Fields
        private PackedAssetReference _Item = PackedAssetReference.None;
        private PackedAssetReference _Balance = PackedAssetReference.None;
        private PackedAssetReference _Manufacturer = PackedAssetReference.None;
        private int _ManufacturerGradeIndex;
        private PackedAssetReference _AlphaPart = PackedAssetReference.None;
        private PackedAssetReference _BetaPart = PackedAssetReference.None;
        private PackedAssetReference _GammaPart = PackedAssetReference.None;
        private PackedAssetReference _DeltaPart = PackedAssetReference.None;
        private PackedAssetReference _EpsilonPart = PackedAssetReference.None;
        private PackedAssetReference _ZetaPart = PackedAssetReference.None;
        private PackedAssetReference _EtaPart = PackedAssetReference.None;
        private PackedAssetReference _ThetaPart = PackedAssetReference.None;
        private PackedAssetReference _MaterialPart = PackedAssetReference.None;
        private PackedAssetReference _PrefixPart = PackedAssetReference.None;
        private PackedAssetReference _TitlePart = PackedAssetReference.None;
        private int _GameStage;
        #endregion

        #region IPackable Members
        public void Read(BitReader reader, Platform platform)
        {
            var alm = InfoManager.AssetLibraryManager;

            this.Item = alm.Decode(reader, platform, AssetGroup.ItemTypes);
            this.Balance = alm.Decode(reader, platform, AssetGroup.BalanceDefs);
            this.Manufacturer = alm.Decode(reader, platform, AssetGroup.Manufacturers);
            this.ManufacturerGradeIndex = reader.ReadInt32(7);
            this.GameStage = reader.ReadInt32(7);
            this.AlphaPart = alm.Decode(reader, platform, AssetGroup.ItemParts);
            this.BetaPart = alm.Decode(reader, platform, AssetGroup.ItemParts);
            this.GammaPart = alm.Decode(reader, platform, AssetGroup.ItemParts);
            this.DeltaPart = alm.Decode(reader, platform, AssetGroup.ItemParts);
            this.EpsilonPart = alm.Decode(reader, platform, AssetGroup.ItemParts);
            this.ZetaPart = alm.Decode(reader, platform, AssetGroup.ItemParts);
            this.EtaPart = alm.Decode(reader, platform, AssetGroup.ItemParts);
            this.ThetaPart = alm.Decode(reader, platform, AssetGroup.ItemParts);
            this.MaterialPart = alm.Decode(reader, platform, AssetGroup.ItemParts);
            this.PrefixPart = alm.Decode(reader, platform, AssetGroup.ItemParts);
            this.TitlePart = alm.Decode(reader, platform, AssetGroup.ItemParts);
        }

        public void Write(BitWriter writer, Platform platform)
        {
            var alm = InfoManager.AssetLibraryManager;

            alm.Encode(writer, platform, AssetGroup.ItemTypes, this.Item);
            alm.Encode(writer, platform, AssetGroup.BalanceDefs, this.Balance);
            alm.Encode(writer, platform, AssetGroup.Manufacturers, this.Manufacturer);
            writer.WriteInt32(this.ManufacturerGradeIndex, 7);
            writer.WriteInt32(this.GameStage, 7);
            alm.Encode(writer, platform, AssetGroup.ItemParts, this.AlphaPart);
            alm.Encode(writer, platform, AssetGroup.ItemParts, this.BetaPart);
            alm.Encode(writer, platform, AssetGroup.ItemParts, this.GammaPart);
            alm.Encode(writer, platform, AssetGroup.ItemParts, this.DeltaPart);
            alm.Encode(writer, platform, AssetGroup.ItemParts, this.EpsilonPart);
            alm.Encode(writer, platform, AssetGroup.ItemParts, this.ZetaPart);
            alm.Encode(writer, platform, AssetGroup.ItemParts, this.EtaPart);
            alm.Encode(writer, platform, AssetGroup.ItemParts, this.ThetaPart);
            alm.Encode(writer, platform, AssetGroup.ItemParts, this.MaterialPart);
            alm.Encode(writer, platform, AssetGroup.ItemParts, this.PrefixPart);
            alm.Encode(writer, platform, AssetGroup.ItemParts, this.TitlePart);
        }
        #endregion

        #region Properties
        public PackedAssetReference Item
        {
            get { return this._Item; }
            set
            {
                if (value != this._Item)
                {
                    this._Item = value;
                    this.NotifyOfPropertyChange(nameof(Item));
                }
            }
        }

        public PackedAssetReference Balance
        {
            get { return this._Balance; }
            set
            {
                if (value != this._Balance)
                {
                    this._Balance = value;
                    this.NotifyOfPropertyChange(nameof(Balance));
                }
            }
        }

        public PackedAssetReference Manufacturer
        {
            get { return this._Manufacturer; }
            set
            {
                if (value != this._Manufacturer)
                {
                    this._Manufacturer = value;
                    this.NotifyOfPropertyChange(nameof(Manufacturer));
                }
            }
        }

        public int ManufacturerGradeIndex
        {
            get { return this._ManufacturerGradeIndex; }
            set
            {
                if (value != this._ManufacturerGradeIndex)
                {
                    this._ManufacturerGradeIndex = value;
                    this.NotifyOfPropertyChange(nameof(ManufacturerGradeIndex));
                }
            }
        }

        public PackedAssetReference AlphaPart
        {
            get { return this._AlphaPart; }
            set
            {
                if (value != this._AlphaPart)
                {
                    this._AlphaPart = value;
                    this.NotifyOfPropertyChange(nameof(AlphaPart));
                }
            }
        }

        public PackedAssetReference BetaPart
        {
            get { return this._BetaPart; }
            set
            {
                if (value != this._BetaPart)
                {
                    this._BetaPart = value;
                    this.NotifyOfPropertyChange(nameof(BetaPart));
                }
            }
        }

        public PackedAssetReference GammaPart
        {
            get { return this._GammaPart; }
            set
            {
                if (value != this._GammaPart)
                {
                    this._GammaPart = value;
                    this.NotifyOfPropertyChange(nameof(GammaPart));
                }
            }
        }

        public PackedAssetReference DeltaPart
        {
            get { return this._DeltaPart; }
            set
            {
                if (value != this._DeltaPart)
                {
                    this._DeltaPart = value;
                    this.NotifyOfPropertyChange(nameof(DeltaPart));
                }
            }
        }

        public PackedAssetReference EpsilonPart
        {
            get { return this._EpsilonPart; }
            set
            {
                if (value != this._EpsilonPart)
                {
                    this._EpsilonPart = value;
                    this.NotifyOfPropertyChange(nameof(EpsilonPart));
                }
            }
        }

        public PackedAssetReference ZetaPart
        {
            get { return this._ZetaPart; }
            set
            {
                if (value != this._ZetaPart)
                {
                    this._ZetaPart = value;
                    this.NotifyOfPropertyChange(nameof(ZetaPart));
                }
            }
        }

        public PackedAssetReference EtaPart
        {
            get { return this._EtaPart; }
            set
            {
                if (value != this._EtaPart)
                {
                    this._EtaPart = value;
                    this.NotifyOfPropertyChange(nameof(EtaPart));
                }
            }
        }

        public PackedAssetReference ThetaPart
        {
            get { return this._ThetaPart; }
            set
            {
                if (value != this._ThetaPart)
                {
                    this._ThetaPart = value;
                    this.NotifyOfPropertyChange(nameof(ThetaPart));
                }
            }
        }

        public PackedAssetReference MaterialPart
        {
            get { return this._MaterialPart; }
            set
            {
                if (value != this._MaterialPart)
                {
                    this._MaterialPart = value;
                    this.NotifyOfPropertyChange(nameof(MaterialPart));
                }
            }
        }

        public PackedAssetReference PrefixPart
        {
            get { return this._PrefixPart; }
            set
            {
                if (value != this._PrefixPart)
                {
                    this._PrefixPart = value;
                    this.NotifyOfPropertyChange(nameof(PrefixPart));
                }
            }
        }

        public PackedAssetReference TitlePart
        {
            get { return this._TitlePart; }
            set
            {
                if (value != this._TitlePart)
                {
                    this._TitlePart = value;
                    this.NotifyOfPropertyChange(nameof(TitlePart));
                }
            }
        }

        public int GameStage
        {
            get { return this._GameStage; }
            set
            {
                if (value != this._GameStage)
                {
                    this._GameStage = value;
                    this.NotifyOfPropertyChange(nameof(GameStage));
                }
            }
        }
        #endregion

        #region ICloneable Members
        public virtual object Clone()
        {
            return new PackedItem()
            {
                Item = this.Item,
                Balance = this.Balance,
                Manufacturer = this.Manufacturer,
                ManufacturerGradeIndex = this.ManufacturerGradeIndex,
                AlphaPart = this.AlphaPart,
                BetaPart = this.BetaPart,
                GammaPart = this.GammaPart,
                DeltaPart = this.DeltaPart,
                EpsilonPart = this.EpsilonPart,
                ZetaPart = this.ZetaPart,
                EtaPart = this.EtaPart,
                ThetaPart = this.ThetaPart,
                MaterialPart = this.MaterialPart,
                PrefixPart = this.PrefixPart,
                TitlePart = this.TitlePart,
                GameStage = this.GameStage,
            };
        }
        #endregion

        #region INotifyPropertyChanged Members
        public event PropertyChangedEventHandler PropertyChanged;

        protected void NotifyOfPropertyChange(string propertyName)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}
