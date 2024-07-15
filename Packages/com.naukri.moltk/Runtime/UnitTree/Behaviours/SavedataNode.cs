using Naukri.InspectorMaid;
using Naukri.InspectorMaid.Layout;
using Naukri.Moltk.DataStorage;
using Naukri.Physarum;
using UnityEngine;

namespace Naukri.Moltk.UnitTree.Behaviours
{
    public partial class SavedataNode : UnitTreeBehaviour
    {
        private CellProperty _dataCell = CellProperty.Null;

        public CellProperty DataCell
        {
            get
            {
                if (_dataCell.IsNull)
                {
                    var path = GetRelativePath();

                    var savedataService = ctx.Read<SavedataService>();

                    _dataCell = savedataService.Savedata[path];
                    _dataCell.CreateIfNotExist();
                }
                return _dataCell;
            }
        }
    }

    [
        ScriptField,
        Base,
        ColumnScope,
            ColumnScope, HideIf(nameof(isEditing)),
                Label("Data Cell Json"),
                Label(binding: nameof(DataJson)), Style(backgroundColor: "#252525"),
                Button("Edit", nameof(StartEditing)),
            EndScope,
            ColumnScope, ShowIf(nameof(isEditing)),
                Slot(nameof(dataCellJson)),
                RowScope, Style(height: "30"),
                    RowScope, Style(flexGrow: "1"),
                        Button("Clear", binding: nameof(ClearJson)), Style(flexGrow: "1"),
                        Button("Reset", binding: nameof(SaveJsonToText)), Style(flexGrow: "1"),
                    EndScope,
                    Spacer("0.1"),
                    RowScope, Style(flexGrow: "1"),
                        Button("Cancel", binding: nameof(CancelEditing)), Style(flexGrow: "1"),
                        Button("Save", binding: nameof(LoadJsonFromText)), Style(flexGrow: "1"),
                    EndScope,
                EndScope,
            EndScope,
        EndScope
    ]
    partial class SavedataNode
    {
        [SerializeField]
        [TextArea(3, 50)]
        private string dataCellJson;

        private bool isEditing = false;

        private bool IsRuntime => Application.isPlaying;

        private void StartEditing()
        {
            isEditing = true;
            SaveJsonToText();
        }

        private void ClearJson()
        {
            dataCellJson = "{}";
        }

        private void LoadJsonFromText()
        {
            DataCell.SetCell(dataCellJson);
            CancelEditing();
        }

        private void CancelEditing()
        {
            dataCellJson = "";
            isEditing = false;
        }

        private void SaveJsonToText()
        {
            dataCellJson = DataCell.ToJson(true);
        }

        private string DataJson()
        {
            var json = DataCell.ToJson(true);
            return json;
        }
    }
}
