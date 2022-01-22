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
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;
using Caliburn.Micro;
using Caliburn.Micro.Contrib;
using Caliburn.Micro.Contrib.Results;
using Gibbed.TinyTinaAoDK.GameInfo;
using Gibbed.Gearbox.WPF;
using DeserializeSettings = Gibbed.TinyTinaAoDK.FileFormats.SaveFile.DeserializeSettings;
using GameGuid = Gibbed.TinyTinaAoDK.ProtoBufFormats.WillowTwoSave.Guid;
using SaveFile = Gibbed.TinyTinaAoDK.FileFormats.SaveFile;
using SystemGuid = System.Guid;

namespace Gibbed.TinyTinaAoDK.SaveEdit
{
    [Export(typeof(ShellViewModel))]
    internal class ShellViewModel : PropertyChangedBase
    {
        #region Imports
        private GeneralViewModel _General;
        private CharacterViewModel _Character;
        private VehicleViewModel _Vehicle;
        private CurrencyOnHandViewModel _CurrencyOnHand;
        private BackpackViewModel _Backpack;
        private BankViewModel _Bank;
        private FastTravelViewModel _FastTravel;
        private AboutViewModel _About;

        [Import(typeof(GeneralViewModel))]
        public GeneralViewModel General
        {
            get { return this._General; }

            set
            {
                this._General = value;
                this.NotifyOfPropertyChange(nameof(General));
            }
        }

        [Import(typeof(CharacterViewModel))]
        public CharacterViewModel Character
        {
            get { return this._Character; }

            set
            {
                this._Character = value;
                this.NotifyOfPropertyChange(nameof(Character));
            }
        }

        [Import(typeof(VehicleViewModel))]
        public VehicleViewModel Vehicle
        {
            get { return this._Vehicle; }

            set
            {
                this._Vehicle = value;
                this.NotifyOfPropertyChange(nameof(Vehicle));
            }
        }

        [Import(typeof(CurrencyOnHandViewModel))]
        public CurrencyOnHandViewModel CurrencyOnHand
        {
            get { return this._CurrencyOnHand; }

            set
            {
                this._CurrencyOnHand = value;
                this.NotifyOfPropertyChange(nameof(CurrencyOnHand));
            }
        }

        [Import(typeof(BackpackViewModel))]
        public BackpackViewModel Backpack
        {
            get { return this._Backpack; }

            set
            {
                this._Backpack = value;
                this.NotifyOfPropertyChange(nameof(Backpack));
            }
        }

        [Import(typeof(BankViewModel))]
        public BankViewModel Bank
        {
            get { return this._Bank; }

            set
            {
                this._Bank = value;
                this.NotifyOfPropertyChange(nameof(Bank));
            }
        }

        [Import(typeof(FastTravelViewModel))]
        public FastTravelViewModel FastTravel
        {
            get { return this._FastTravel; }

            set
            {
                this._FastTravel = value;
                this.NotifyOfPropertyChange(nameof(FastTravel));
            }
        }

        [Import(typeof(AboutViewModel))]
        public AboutViewModel About
        {
            get { return this._About; }

            set
            {
                this._About = value;
                this.NotifyOfPropertyChange(nameof(About));
            }
        }
        #endregion

        #region Fields
        private SaveLoad _SaveLoad;
        private string _SavePath;
        private SaveFile _SaveFile;
        private readonly ICommand _NewSaveFromPlayerClass;

        private bool _IsGeneralSelected;
        private bool _IsFirstAboutSelection;
        private bool _IsAboutSelected;
        #endregion

        #region Properties
        public IEnumerable<PlayerClassDefinition> PlayerClasses
        {
            get
            {
                return InfoManager.PlayerClasses.Items
                                  .Select(kv => kv.Value)
                                  .Distinct()
                                  .OrderBy(dp => dp.SortOrder);
            }
        }

        public string SavePath
        {
            get { return this._SavePath; }
            private set
            {
                if (this._SavePath != value)
                {
                    this._SavePath = value;
                    this.NotifyOfPropertyChange(nameof(SavePath));
                }
            }
        }

        public SaveFile SaveFile
        {
            get { return this._SaveFile; }
            private set
            {
                if (this._SaveFile != value)
                {
                    this._SaveFile = value;
                    this.NotifyOfPropertyChange(nameof(SaveFile));
                }
            }
        }

        [Import(typeof(SaveLoad))]
        public SaveLoad SaveLoad
        {
            get { return this._SaveLoad; }

            set
            {
                this._SaveLoad = value;
                this.NotifyOfPropertyChange(nameof(SaveLoad));
            }
        }

        public bool IsGeneralSelected
        {
            get { return this._IsGeneralSelected; }
            set
            {
                if (this._IsGeneralSelected != value)
                {
                    this._IsGeneralSelected = value;
                    this.NotifyOfPropertyChange(nameof(IsGeneralSelected));
                }
            }
        }

        public bool IsAboutSelected
        {
            get { return this._IsAboutSelected; }
            set
            {
                if (this._IsAboutSelected != value)
                {
                    this._IsFirstAboutSelection = false;
                    this._IsAboutSelected = value;
                    this.NotifyOfPropertyChange(nameof(IsAboutSelected));
                }
            }
        }

        public ICommand NewSaveFromPlayerClass
        {
            get { return this._NewSaveFromPlayerClass; }
        }
        #endregion

        [ImportingConstructor]
        public ShellViewModel()
        {
            this._IsAboutSelected = true;
            this._IsFirstAboutSelection = true;
            this._NewSaveFromPlayerClass = new DelegateCommand<PlayerClassDefinition>(this.DoNewSaveFromPlayerClass);
        }

        private void MaybeSwitchToGeneral()
        {
            if (this.IsAboutSelected == true && this._IsFirstAboutSelection == true)
            {
                this.IsGeneralSelected = true;
            }
        }

        private void DoNewSaveFromPlayerClass(PlayerClassDefinition playerClass)
        {
            var saveFile = new SaveFile()
            {
                Platform = Platform.PC,
                SaveGame = new ProtoBufFormats.WillowTwoSave.WillowTwoPlayerSaveGame()
                {
                    SaveGameId = 1,
                    SaveGuid = (GameGuid)SystemGuid.NewGuid(),
                    PlayerClass = playerClass.ResourcePath,
                    UIPreferences = new ProtoBufFormats.WillowTwoSave.UIPreferencesData()
                    {
                        CharacterName = Encoding.UTF8.GetBytes(playerClass.Name),
                    },
                    AppliedCustomizations = new List<string>()
                    {
                        "None",
                        "",
                        "",
                        "",
                        "None",
                    },
                },
            };

            FileFormats.SaveExpansion.ExtractExpansionSavedataFromUnloadableItemData(saveFile.SaveGame);

            this.General.ImportData(saveFile.SaveGame, saveFile.Platform);
            this.Character.ImportData(saveFile.SaveGame);
            this.Vehicle.ImportData(saveFile.SaveGame);
            this.CurrencyOnHand.ImportData(saveFile.SaveGame);
            this.Backpack.ImportData(saveFile.SaveGame, saveFile.Platform);
            this.Bank.ImportData(saveFile.SaveGame, saveFile.Platform);
            this.FastTravel.ImportData(saveFile.SaveGame);
            this.SavePath = null;
            this.SaveFile = saveFile;
            this.MaybeSwitchToGeneral();
        }

        public IEnumerable<IResult> NewSave()
        {
            yield return new DelegateResult(
                () =>
                {
                    var playerClasses =
                        InfoManager.PlayerClasses.Items.Where(pc => pc.Value.DLC == null)
                                   .Select(kv => kv.Value)
                                   .ToArray();
                    var playerClass = playerClasses[new Random().Next(playerClasses.Length)];
                    this.DoNewSaveFromPlayerClass(playerClass);
                })
                .Rescue<DllNotFoundException>().Execute(
                    x => new MyMessageBox($"Failed to create save: {x.Message}", "Error")
                             .WithIcon(MessageBoxImage.Error).AsCoroutine())
                .Rescue<FileFormats.SaveFormatException>().Execute(
                    x => new MyMessageBox($"Failed to create save: {x.Message}", "Error")
                             .WithIcon(MessageBoxImage.Error).AsCoroutine())
                .Rescue<FileFormats.SaveCorruptionException>().Execute(
                    x => new MyMessageBox($"Failed to create save: {x.Message}", "Error")
                             .WithIcon(MessageBoxImage.Error).AsCoroutine())
                .Rescue().Execute(
                    x =>
                    new MyMessageBox(
                        $"An exception was thrown (press Ctrl+C to copy):\n\n{x.ToString()}",
                        "Error")
                        .WithIcon(MessageBoxImage.Error).AsCoroutine());
        }

        public IEnumerable<IResult> ReadSave()
        {
            string fileName = null;
            var platform = Platform.Invalid;

            foreach (var result in this.SaveLoad.OpenFile(s => fileName = s, p => platform = p))
            {
                yield return result;
            }

            if (fileName == null)
            {
                yield break;
            }

            SaveFile saveFile = null;

            yield return new DelegateResult(
                () =>
                {
                    using (var input = File.OpenRead(fileName))
                    {
                        saveFile = SaveFile.Deserialize(input, platform, DeserializeSettings.None);
                    }

                    try
                    {
                        FileFormats.SaveExpansion.ExtractExpansionSavedataFromUnloadableItemData(
                            saveFile.SaveGame);

                        this.General.ImportData(saveFile.SaveGame, saveFile.Platform);
                        this.Character.ImportData(saveFile.SaveGame);
                        this.Vehicle.ImportData(saveFile.SaveGame);
                        this.CurrencyOnHand.ImportData(saveFile.SaveGame);
                        this.Backpack.ImportData(saveFile.SaveGame, saveFile.Platform);
                        this.Bank.ImportData(saveFile.SaveGame, saveFile.Platform);
                        this.FastTravel.ImportData(saveFile.SaveGame);
                        this.SavePath = fileName;
                        this.SaveFile = saveFile;
                        this.MaybeSwitchToGeneral();
                    }
                    catch (Exception)
                    {
                        this.SaveFile = null;
                        throw;
                    }
                })
                .Rescue<DllNotFoundException>().Execute(
                    x => new MyMessageBox($"Failed to load save: {x.Message}", "Error")
                             .WithIcon(MessageBoxImage.Error).AsCoroutine())
                .Rescue<FileFormats.SaveFormatException>().Execute(
                    x => new MyMessageBox($"Failed to load save: {x.Message}", "Error")
                             .WithIcon(MessageBoxImage.Error).AsCoroutine())
                .Rescue<FileFormats.SaveCorruptionException>().Execute(
                    x => new MyMessageBox($"Failed to load save: {x.Message}", "Error")
                             .WithIcon(MessageBoxImage.Error).AsCoroutine())
                .Rescue().Execute(
                    x =>
                    new MyMessageBox(
                        $"An exception was thrown (press Ctrl+C to copy):\n\n{x.ToString()}",
                        "Error")
                        .WithIcon(MessageBoxImage.Error).AsCoroutine());


            if (saveFile != null &&
                saveFile.SaveGame.IsBadassModeSaveGame == true)
            {
                saveFile.SaveGame.IsBadassModeSaveGame = false;
                yield return
                    new MyMessageBox(
                        "Your save file was set as 'Badass Mode', and this has now been cleared.\n\n" +
                        "See http://bit.ly/graveyardsav for more details.",
                        "Information")
                        .WithIcon(MessageBoxImage.Information);
            }

            if (this.SaveFile != null &&
                this.Backpack.BrokenWeapons.Count > 0)
            {
                var result = MessageBoxResult.No;
                do
                {
                    yield return
                        new MyMessageBox(
                            "There were weapons in the backpack that failed to load. Do you want to remove them?\n\n" +
                            "If you choose not to remove them, they will remain in your save but will not be editable." +
                            (result != MessageBoxResult.Cancel
                                 ? "\n\nChoose Cancel to copy error information to the clipboard."
                                 : ""),
                            "Warning")
                            .WithButton(result != MessageBoxResult.Cancel
                                            ? MessageBoxButton.YesNoCancel
                                            : MessageBoxButton.YesNo)
                            .WithDefaultResult(MessageBoxResult.No)
                            .WithResultDo(r => result = r)
                            .WithIcon(MessageBoxImage.Warning);
                    if (result == MessageBoxResult.Yes)
                    {
                        this.Backpack.BrokenWeapons.Clear();
                    }
                    else if (result == MessageBoxResult.Cancel)
                    {
                        var sb = new StringBuilder();
                        this.Backpack.BrokenWeapons.ForEach(
                            kv =>
                            {
                                sb.AppendLine(kv.Value.ToString());
                                sb.AppendLine();
                            });
                        if (MyClipboard.SetText(sb.ToString()) != MyClipboard.Result.Success)
                        {
                            MessageBox.Show("Clipboard failure.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                }
                while (result == MessageBoxResult.Cancel);
            }

            if (this.SaveFile != null &&
                this.Backpack.BrokenItems.Count > 0)
            {
                var result = MessageBoxResult.No;
                do
                {
                    yield return
                        new MyMessageBox(
                            "There were items in the backpack that failed to load. Do you want to remove them?\n\n" +
                            "If you choose not to remove them, they will remain in your save but will not be editable." +
                            (result != MessageBoxResult.Cancel
                                 ? "\n\nChoose Cancel to copy error information to the clipboard."
                                 : ""),
                            "Warning")
                            .WithButton(result != MessageBoxResult.Cancel
                                            ? MessageBoxButton.YesNoCancel
                                            : MessageBoxButton.YesNo)
                            .WithDefaultResult(MessageBoxResult.No)
                            .WithResultDo(r => result = r)
                            .WithIcon(MessageBoxImage.Warning);
                    if (result == MessageBoxResult.Yes)
                    {
                        this.Backpack.BrokenItems.Clear();
                    }
                    else if (result == MessageBoxResult.Cancel)
                    {
                        var sb = new StringBuilder();
                        this.Backpack.BrokenItems.ForEach(
                            kv =>
                            {
                                sb.AppendLine(kv.Value.ToString());
                                sb.AppendLine();
                            });
                        if (MyClipboard.SetText(sb.ToString()) != MyClipboard.Result.Success)
                        {
                            MessageBox.Show("Clipboard failure.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                }
                while (result == MessageBoxResult.Cancel);
            }

            if (this.SaveFile != null &&
                this.Bank.BrokenSlots.Count > 0)
            {
                var result = MessageBoxResult.No;
                do
                {
                    yield return
                        new MyMessageBox(
                            "There were weapons or items in the bank that failed to load. Do you want to remove them?\n\n" +
                            "If you choose not to remove them, they will remain in your save but will not be editable." +
                            (result != MessageBoxResult.Cancel
                                 ? "\n\nChoose Cancel to copy error information to the clipboard."
                                 : ""),
                            "Warning")
                            .WithButton(result != MessageBoxResult.Cancel
                                            ? MessageBoxButton.YesNoCancel
                                            : MessageBoxButton.YesNo)
                            .WithDefaultResult(MessageBoxResult.No)
                            .WithResultDo(r => result = r)
                            .WithIcon(MessageBoxImage.Warning);
                    if (result == MessageBoxResult.Yes)
                    {
                        this.Bank.BrokenSlots.Clear();
                    }
                    else if (result == MessageBoxResult.Cancel)
                    {
                        var sb = new StringBuilder();
                        this.Bank.BrokenSlots.ForEach(
                            kv =>
                            {
                                sb.AppendLine(kv.Value.ToString());
                                sb.AppendLine();
                            });
                        if (MyClipboard.SetText(sb.ToString()) != MyClipboard.Result.Success)
                        {
                            MessageBox.Show("Clipboard failure.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                }
                while (result == MessageBoxResult.Cancel);
            }
        }

        public IEnumerable<IResult> WriteSave()
        {
            if (this.SaveFile == null || string.IsNullOrEmpty(this.SavePath) == true)
            {
                yield break;
            }

            var savePath = this.SavePath;
            var saveFile = this.SaveFile;

            yield return new DelegateResult(
                () => WriteSave(savePath, saveFile))
                .Rescue()
                .Execute(
                    x =>
                    new MyMessageBox(
                        $"An exception was thrown (press Ctrl+C to copy):\n\n{x.ToString()}",
                        "Error")
                        .WithIcon(MessageBoxImage.Error).AsCoroutine());
        }

        public IEnumerable<IResult> WriteSaveAs()
        {
            if (this.SaveFile == null)
            {
                yield break;
            }

            string savePath = null;

            foreach (var result in this.SaveLoad.SaveFile(s => savePath = s))
            {
                yield return result;
            }

            if (savePath == null)
            {
                yield break;
            }

            var saveFile = this.SaveFile;

            yield return new DelegateResult(
                () =>
                {
                    this.WriteSave(savePath, saveFile);
                    this.SavePath = savePath;
                })
                .Rescue()
                .Execute(
                    x =>
                    new MyMessageBox(
                        $"An exception was thrown (press Ctrl+C to copy):\n\n{x.ToString()}",
                        "Error")
                        .WithIcon(MessageBoxImage.Error).AsCoroutine());
        }

        private void WriteSave(string savePath, SaveFile saveFile)
        {
            this.General.ExportData(saveFile.SaveGame, out var platform);
            this.Character.ExportData(saveFile.SaveGame);
            this.Vehicle.ExportData(saveFile.SaveGame);
            this.CurrencyOnHand.ExportData(saveFile.SaveGame);
            this.Backpack.ExportData(saveFile.SaveGame, platform);
            this.Bank.ExportData(saveFile.SaveGame, platform);
            this.FastTravel.ExportData(saveFile.SaveGame);

            if (saveFile.SaveGame != null &&
                saveFile.SaveGame.WeaponData != null)
            {
                saveFile.SaveGame.WeaponData.RemoveAll(
                    wd =>
                    Blacklisting.IsBlacklistedType(wd.Type) == true ||
                    Blacklisting.IsBlacklistedBalance(wd.Balance) == true);
            }

            if (saveFile.SaveGame != null &&
                saveFile.SaveGame.ItemData != null)
            {
                saveFile.SaveGame.ItemData.RemoveAll(
                    wd =>
                    Blacklisting.IsBlacklistedType(wd.Type) == true ||
                    Blacklisting.IsBlacklistedBalance(wd.Balance) == true);
            }

            using (var output = File.Create(savePath))
            {
                FileFormats.SaveExpansion.AddExpansionSavedataToUnloadableItemData(
                    saveFile.SaveGame);
                saveFile.Platform = platform;
                saveFile.Serialize(output);
                FileFormats.SaveExpansion
                           .ExtractExpansionSavedataFromUnloadableItemData(
                               saveFile.SaveGame);
            }
        }
    }
}
