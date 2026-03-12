using System;
using System.Collections.Generic;
using System.Globalization;

public interface ICloudSaveProvider 
{
    string CaptureJson();
    void ApplyJson(string json);
}
