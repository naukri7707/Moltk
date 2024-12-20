﻿using System;
using System.Collections;
using System.Diagnostics;
using Naukri.InspectorMaid;
using Naukri.InspectorMaid.Layout;
using Naukri.Moltk.UnitTree;
using Naukri.Moltk.UnitTree.Events;
using Naukri.Physarum;
using UnityEngine;
using UnityEngine.Events;

namespace Naukri.Moltk.DataStorage
{
    public partial class SavedataService : Provider.Behaviour
    {
        #region fields

        [SerializeField]
        private FileDirectory fileDirectory = FileDirectory.PersistentDataPath;

        [SerializeField]
        private string fileName = "savedata.json";

        [SerializeField]
        private bool format;

        [SerializeField]
        private int version;

        [SerializeField]
        private UnityEvent<int, int, Savedata> versionConflictHandler;

        [SerializeField]
        private bool loadSavedataIfNull = true;

        [SerializeField]
        private AutoSaving autoSaving;

        [SerializeField]
        private int autoSavingInterval = 10;

        private Coroutine autoSavingCoroutine;

        [SerializeField]
        private UnitTreeController autoSavingTree;

        private Savedata savedata;
        #endregion

        #region properties

        [Template]
        public string FilePath
        {
            get
            {
                var fileDirectoryPath = fileDirectory switch
                {
                    FileDirectory.ConsoleLogPath => Application.consoleLogPath,
                    FileDirectory.DataPath => Application.dataPath,
                    FileDirectory.PersistentDataPath => Application.persistentDataPath,
                    FileDirectory.StreamingAssetsPath => Application.streamingAssetsPath,
                    FileDirectory.TemporaryCachePath => Application.temporaryCachePath,
                    _
                        => throw new ArgumentException(
                            $"Invalid value for {nameof(fileDirectory)}: {fileDirectory}"
                        ),
                };
                return $"{fileDirectoryPath}/{fileName}";
            }
        }

        public Savedata Savedata
        {
            get
            {
                if (savedata == null || savedata.IsDestroy)
                {
                    if (loadSavedataIfNull)
                    {
                        Load();
                    }
                    else
                    {
                        throw new InvalidOperationException(
                            "Savedata is null. Please load it first."
                        );
                    }
                }
                return savedata;
            }
            private set => savedata = value;
        }

        private bool IsRuntime => Application.isPlaying;
        private string FilePathPreview => $"Savedata full file path:\n {FilePath}";
        #endregion

        #region methods

        [Template]
        public virtual void Load()
        {
            savedata?.Destroy();
            savedata = Savedata.LoadFromJsonFile(FilePath);
            if (savedata.version != version)
            {
                versionConflictHandler.Invoke(savedata.version, version, savedata);
            }
            print("Savedata loaded.");
        }

        [Template]
        public virtual void Save()
        {
            if (savedata == null)
            {
                savedata = Savedata.LoadFromJsonFile(FilePath);
            }
            savedata.version = version;
            Savedata.SaveToJsonFile(savedata, FilePath, format);
            print("Savedata saved.");
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            if (autoSaving == AutoSaving.ByTime)
            {
                autoSavingCoroutine = StartCoroutine(SaveEveryInterval());
            }
            else if (autoSaving == AutoSaving.OnUnitTreeNodeChanged)
            {
                autoSavingTree.EventHandler.AddListener(SaveOnNodeChanged);
            }
            else
            {
                // Do nothing
            }
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            if (autoSaving == AutoSaving.ByTime)
            {
                if (autoSavingCoroutine != null)
                {
                    StopCoroutine(autoSavingCoroutine);
                }
            }
            else if (autoSaving == AutoSaving.OnUnitTreeNodeChanged)
            {
                if (autoSavingTree != null)
                {
                    autoSavingTree.EventHandler.RemoveListener(SaveOnNodeChanged);
                }
            }
            else
            {
                // Do nothing
            }
        }

        private void SaveOnNodeChanged(UnitTreeEvent evt)
        {
            if (evt is NodeChangedEvent)
            {
                Save();
            }
        }

        private IEnumerator SaveEveryInterval()
        {
            while (true)
            {
                yield return new WaitForSeconds(autoSavingInterval);
                Save();
            }
        }

        [Template]
        private void Print()
        {
            var json = Savedata.SaveToJson(Savedata, true);
            print(json);
        }

        [Template]
        private void Open()
        {
            Process.Start(FilePath);
        }

        private void CopyFilePath()
        {
            GUIUtility.systemCopyBuffer = FilePath;
            print("file path copied.");
        }

        #endregion

        public enum AutoSaving
        {
            None,

            ByTime,

            OnUnitTreeNodeChanged,
        }

        public enum FileDirectory
        {
            ConsoleLogPath,

            DataPath,

            PersistentDataPath,

            StreamingAssetsPath,

            TemporaryCachePath,
        }
    }

    [
          ScriptField,
          Base,
          GroupScope("Service Settings", true),
              Slot(
              nameof(loadSavedataIfNull),
              nameof(autoSaving)
              ),
              ColumnScope, ShowIf(nameof(autoSaving), AutoSaving.ByTime),
                  Slot(nameof(autoSavingInterval)),
              EndScope,
              ColumnScope, ShowIf(nameof(autoSaving), AutoSaving.OnUnitTreeNodeChanged),
                  Slot(nameof(autoSavingTree)),
              EndScope,
          EndScope,
          GroupScope("Metadata", true),
              Slot(
              nameof(fileDirectory),
              nameof(fileName),
              nameof(format),
              nameof(version),
              nameof(versionConflictHandler)
              ),
              ColumnScope, Style(borderColorAll: "#000000", borderWidthAll: "1", borderRadiusAll: "5", marginVertical: "3", paddingVertical: "4", paddingHorizontal: "5"),
                  Label(binding: nameof(FilePathPreview)),
                  RowScope,
                      Button("Copy", binding: nameof(CopyFilePath)), Style(flexGrow: "1"),
                      Button("Open", binding: nameof(Open)), Style(flexGrow: "1"),
                  EndScope,
              EndScope,
              RowScope, Style(height: "30"),
                  Button("Save", binding: nameof(Save)), Style(flexGrow: "1"),
                  Button("Load", binding: nameof(Load)), Style(flexGrow: "1"),
                  Button("Print", binding: nameof(Print)), Style(flexGrow: "1"),
              EndScope,
          EndScope
      ]
    partial class SavedataService
    { }
}
