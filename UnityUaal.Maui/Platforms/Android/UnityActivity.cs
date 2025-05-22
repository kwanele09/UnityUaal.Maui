using Android.App;
using Android.Content.PM;
using Android.Content.Res;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Com.Unity3d.Player;
using UnityUaalMaui.Unity;

namespace UnityUaalMaui;

[Activity(Label = "UnityActivity",
          MainLauncher = false,
          ConfigurationChanges = ConfigChanges.Mcc | ConfigChanges.Mnc | ConfigChanges.Locale | ConfigChanges.Touchscreen | ConfigChanges.Keyboard | ConfigChanges.KeyboardHidden | ConfigChanges.Navigation | ConfigChanges.Orientation | ConfigChanges.ScreenLayout | ConfigChanges.UiMode | ConfigChanges.ScreenSize | ConfigChanges.SmallestScreenSize | ConfigChanges.FontScale | ConfigChanges.LayoutDirection | ConfigChanges.Density,
          ResizeableActivity = false, // Disable multi-window support
          LaunchMode = LaunchMode.SingleTask)]
[MetaData(name: "unityplayer.UnityActivity", Value = "true")]
[MetaData(name: "notch_support", Value = "true")]
public class UnityActivity : Activity,
                             IUnityPlayerLifecycleEvents,
                             INativeUnityBridge,
                             IUnityPermissionRequestSupport
{
    private UnityPlayerForActivityOrService player;

    private int _permissionRequestCode = 1000; // start from some number

    // Keep track of PermissionRequest by requestCode
    private readonly System.Collections.Generic.Dictionary<int, PermissionRequest> _permissionRequests = new();

    protected override void OnCreate(Bundle savedInstanceState)
    {
        if (CheckSelfPermission(Android.Manifest.Permission.Camera) != Android.Content.PM.Permission.Granted)
        {
            RequestPermissions(new string[] { Android.Manifest.Permission.Camera }, 1);
        }

        RequestWindowFeature(WindowFeatures.NoTitle);

        base.OnCreate(savedInstanceState);
        player = new UnityPlayerForActivityOrService(this, this);

        this.SetContentView(player.FrameLayout);
        player.FrameLayout.RequestFocus();

        UnityUaalMaui.Unity.UnityBridge.RegisterNativeBridge(this);
    }

    public override void OnConfigurationChanged(Configuration newConfig)
    {
        Android.Util.Log.Info(GetType().Name, nameof(OnConfigurationChanged) + "|" + GetHashCode() + "|" + newConfig);
        base.OnConfigurationChanged(newConfig);
        player.ConfigurationChanged(newConfig);
    }

    public override void OnWindowFocusChanged(bool hasFocus)
    {
        Android.Util.Log.Info(GetType().Name, nameof(OnWindowFocusChanged) + "|" + GetHashCode() + "|" + "hasFocus=" + hasFocus);
        base.OnWindowFocusChanged(hasFocus);
        player.WindowFocusChanged(hasFocus);
    }

    protected override void OnNewIntent(Android.Content.Intent intent)
    {
        Android.Util.Log.Info(GetType().Name, nameof(OnNewIntent) + "|" + GetHashCode() + "|" + "Intent=" + intent.Action + "," + intent.Flags);
        Intent = intent;
        player.NewIntent(intent);
    }

    protected override void OnStop()
    {
        Android.Util.Log.Info(GetType().Name, nameof(OnStop) + "|" + GetHashCode() + "|");
        base.OnStop();

        Android.Util.Log.Info(GetType().Name, "UnityPlayer.Pause");
        player.Pause();
    }

    protected override void OnPause()
    {
        Android.Util.Log.Info(GetType().Name, nameof(OnPause) + "|" + GetHashCode() + "|");
        base.OnPause();

        Android.Util.Log.Info(GetType().Name, "UnityPlayer.Pause");
        player.Pause();
    }

    protected override void OnStart()
    {
        Android.Util.Log.Info(GetType().Name, nameof(OnStart) + "|" + GetHashCode() + "|");
        base.OnStart();

        Android.Util.Log.Info(GetType().Name, "UnityPlayer.Resume");
        player.Resume();
    }

    protected override void OnResume()
    {
        Android.Util.Log.Info(GetType().Name, nameof(OnResume) + "|" + GetHashCode() + "|");
        base.OnResume();

        Android.Util.Log.Info(GetType().Name, "UnityPlayer.Resume");
        player.Resume();
    }

    protected override void OnDestroy()
    {
        Android.Util.Log.Info(GetType().Name, nameof(OnDestroy) + "|" + GetHashCode() + "|");
        base.OnDestroy();

        UnityUaalMaui.Unity.UnityBridge.RegisterNativeBridge(null);
    }

    public void OnUnityPlayerQuitted()
    {
        Android.Util.Log.Info(GetType().Name, nameof(OnUnityPlayerQuitted) + "|" + GetHashCode() + "|");
    }

    public void OnUnityPlayerUnloaded()
    {
        Android.Util.Log.Info(GetType().Name, nameof(OnUnityPlayerUnloaded) + "|" + GetHashCode() + "|");
        MoveTaskToBack(true);
    }

    public void SendContent(string eventName, string eventContent)
    {
        var content = eventName + "|" + (eventContent ?? string.Empty);

        UnityPlayer.UnitySendMessage("Bridge", "ReceiveContent", content);
    }

    public void RequestPermissions(PermissionRequest request)
    {
        int requestCode = _permissionRequestCode++;
        _permissionRequests[requestCode] = request;

        player.AddPermissionRequest(request);

        // Since PermissionRequest does not expose permission strings,
        // here we request the Camera permission explicitly (adjust if you need other permissions)
        var permissionsToRequest = new string[] { Android.Manifest.Permission.Camera };

        RequestPermissions(permissionsToRequest, requestCode);
    }

    public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Permission[] grantResults)
    {
        base.OnRequestPermissionsResult(requestCode, permissions, grantResults);

        if (_permissionRequests.TryGetValue(requestCode, out var request))
        {
            // Inform UnityPlayer about the permission result
            player.PermissionResponse(this, requestCode, permissions, grantResults?.Select(gr => (int)gr).ToArray() ?? System.Array.Empty<int>());
            _permissionRequests.Remove(requestCode);
        }
    }

    public override bool DispatchKeyEvent(KeyEvent e)
    {
        Android.Util.Log.Info(GetType().Name, nameof(DispatchKeyEvent) + "|" + GetHashCode() + "|" + e.Action);
        if (e.Action == KeyEventActions.Multiple)
        {
            return player.InjectEvent(e);
        }

        return base.DispatchKeyEvent(e);
    }

    public override bool OnKeyUp(Keycode keyCode, KeyEvent e)
    {
        Android.Util.Log.Info(GetType().Name, nameof(OnKeyUp));
        return player.FrameLayout.OnKeyUp(keyCode, e);
    }

    public override bool OnKeyDown(Keycode keyCode, KeyEvent e)
    {
        Android.Util.Log.Info(GetType().Name, nameof(OnKeyDown));
        return player.FrameLayout.OnKeyDown(keyCode, e);
    }

    public override bool OnTouchEvent(MotionEvent e)
    {
        Android.Util.Log.Info(GetType().Name, nameof(OnTouchEvent));
        return player.FrameLayout.OnTouchEvent(e);
    }

    public override bool OnGenericMotionEvent(MotionEvent e)
    {
        Android.Util.Log.Info(GetType().Name, nameof(OnGenericMotionEvent));
        return player.FrameLayout.OnGenericMotionEvent(e);
    }
}
