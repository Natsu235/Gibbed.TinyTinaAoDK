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

using System.Collections.Generic;
using System.ComponentModel;
using ProtoBuf;

namespace Gibbed.Borderlands2.ProtoBufFormats.WillowTwoSave
{
    [ProtoContract]
    public class MissionData : INotifyPropertyChanged
    {
        #region Fields
        private string _Mission;
        private MissionStatus _Status;
        private bool _IsFromDLC;
        private int _DLCPackageId;
        private List<int> _ObjectivesProgress = new List<int>();
        private int _ActiveObjectiveSetIndex;
        private List<int> _SubObjectiveSetIndexes = new List<int>();
        private bool _NeedsRewards;
        private int _Unknown9;
        private bool _HeardKickoff;
        private int _GameStage;
        #endregion

        #region Serialization
        [ProtoAfterDeserialization]
        private void OnDeserialization()
        {
            this._ObjectivesProgress = this._ObjectivesProgress ?? new List<int>();
            this._SubObjectiveSetIndexes = this._SubObjectiveSetIndexes ?? new List<int>();
        }

        private bool ShouldSerializeObjectivesProgress()
        {
            return this._ObjectivesProgress != null &&
                   this._ObjectivesProgress.Count > 0;
        }

        private bool ShouldSerializeSubObjectiveSetIndexes()
        {
            return this._SubObjectiveSetIndexes != null &&
                   this._SubObjectiveSetIndexes.Count > 0;
        }
        #endregion

        #region Properties
        [ProtoMember(1, IsRequired = true)]
        public string Mission
        {
            get { return this._Mission; }
            set
            {
                if (value != this._Mission)
                {
                    this._Mission = value;
                    this.NotifyOfPropertyChange(nameof(Mission));
                }
            }
        }

        [ProtoMember(2, IsRequired = true)]
        public MissionStatus Status
        {
            get { return this._Status; }
            set
            {
                if (value != this._Status)
                {
                    this._Status = value;
                    this.NotifyOfPropertyChange(nameof(Status));
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

        [ProtoMember(5, IsRequired = true, IsPacked = true)]
        public List<int> ObjectivesProgress
        {
            get { return this._ObjectivesProgress; }
            set
            {
                if (value != this._ObjectivesProgress)
                {
                    this._ObjectivesProgress = value;
                    this.NotifyOfPropertyChange(nameof(ObjectivesProgress));
                }
            }
        }

        [ProtoMember(6, IsRequired = true)]
        public int ActiveObjectiveSetIndex
        {
            get { return this._ActiveObjectiveSetIndex; }
            set
            {
                if (value != this._ActiveObjectiveSetIndex)
                {
                    this._ActiveObjectiveSetIndex = value;
                    this.NotifyOfPropertyChange(nameof(ActiveObjectiveSetIndex));
                }
            }
        }

        [ProtoMember(7, IsRequired = true, IsPacked = true)]
        public List<int> SubObjectiveSetIndexes
        {
            get { return this._SubObjectiveSetIndexes; }
            set
            {
                if (value != this._SubObjectiveSetIndexes)
                {
                    this._SubObjectiveSetIndexes = value;
                    this.NotifyOfPropertyChange(nameof(SubObjectiveSetIndexes));
                }
            }
        }

        [ProtoMember(8, IsRequired = true)]
        public bool NeedsRewards
        {
            get { return this._NeedsRewards; }
            set
            {
                if (value != this._NeedsRewards)
                {
                    this._NeedsRewards = value;
                    this.NotifyOfPropertyChange(nameof(NeedsRewards));
                }
            }
        }

        [ProtoMember(9, IsRequired = false)]
        public int Unknown9
        {
            get { return this._Unknown9; }
            set
            {
                if (value != this._Unknown9)
                {
                    this._Unknown9 = value;
                    this.NotifyOfPropertyChange(nameof(Unknown9));
                }
            }
        }

        [ProtoMember(10, IsRequired = true)]
        public bool HeardKickoff
        {
            get { return this._HeardKickoff; }
            set
            {
                if (value != this._HeardKickoff)
                {
                    this._HeardKickoff = value;
                    this.NotifyOfPropertyChange(nameof(HeardKickoff));
                }
            }
        }

        [ProtoMember(11, IsRequired = true)]
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

        #region INotifyPropertyChanged Members
        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyOfPropertyChange(string propertyName)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion

        public override string ToString()
        {
            return this.Mission ?? base.ToString();
        }
    }
}
