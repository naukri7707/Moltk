using Naukri.InspectorMaid;
using Naukri.Moltk.DataStorage.Csv;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CsvSheetManager : MonoBehaviour
{
    public string csvFilePath;

    public bool useGoogleSheet;

    [ShowIf(nameof(useGoogleSheet))]
    public string GoogleSheetId;

    private CsvSheet csvSheet;

    public void Save()
    {
        csvSheet.Save(csvFilePath);
    }

    public void Load(CsvSheet csvSheet)
    {
        csvSheet.Load(csvFilePath);
        this.csvSheet = csvSheet;
    }

    public void UploadToGoogleSheet()
    {
        // Todo:
    }

    public void DownloadFromGoogleSheet()
    {
        // Todo:
    }
}
