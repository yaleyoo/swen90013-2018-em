/**
 * Created by Chao Li
 * Unit test for csv importer
 **/
using UnityEngine;
using UnityEditor;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;

public class CsvImporterTest {

    [Test]
    public void CsvImporterRead() {
        CsvImporter.ReadCsv();
        CsvImporter.PrintLeaves();
    }
}
