﻿public class Global
{
    public static int DownloadingMaxNum => 10;
    public static int FailedTryAgain => 3;    

    public static string GetHostServerURL()
    {
        string hostServerIP = "http://127.0.0.1:8080";
        string runtimePlatform = GetRuntimePlatform();

        return $"{hostServerIP}/CDN/{runtimePlatform}";
    }

    static string GetRuntimePlatform()
    {
        string runtimePlatform = "StandaloneWindows64";
#if UNITY_EDITOR
        runtimePlatform = UnityEditor.EditorUserBuildSettings.activeBuildTarget.ToString();
#else
        switch (Application.platform)
        {            
            case RuntimePlatform.Android:
                runtimePlatform = "Android";
                break;
            case RuntimePlatform.IPhonePlayer:
                runtimePlatform = "iOS";
                break;
        }
#endif
        return runtimePlatform;
    }

}
