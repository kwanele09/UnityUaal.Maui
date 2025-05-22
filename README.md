Sure, Kwanele! Here's a complete `README.md` file tailored for your Unity + MAUI Android integration project using `UnityPlayerForActivityOrService`. This guide covers the purpose, setup, and usage for other developers to understand and work with the project effectively.

---

```markdown
# UnityUaalMaui

This repository demonstrates how to embed a **Unity Android activity** inside a **.NET MAUI** application using **Unity as a Library (UAAL)** integration. The `UnityActivity.cs` file acts as a bridge to handle Unity’s lifecycle and communication with the native Android side.

## 📌 Features

- Unity embedded as a secondary activity inside a .NET MAUI project.
- Full Unity lifecycle handling (pause, resume, destroy).
- Native-to-Unity messaging bridge.
- Permission request support (e.g., camera).
- Handles input events like key presses and touch events.

---

## 🛠️ Prerequisites

- **Visual Studio 2022/2023** with .NET MAUI workload.
- **Unity Editor 2021+**.
- **Android SDK and NDK** configured.
- Basic understanding of:
  - MAUI / Xamarin.Android
  - Unity Android Build
  - Android Activities and Permissions

---

## 📂 Project Structure

```

UnityUaalMaui/
│
├── UnityActivity.cs              # Custom Android Activity to host Unity
├── UnityPlayerForActivityOrService.cs
├── UnityBridge.cs                # Handles communication with Unity
├── \[Unity Project Exported as AAR or .unitypackage]
└── README.md

````

---

## 🧩 Setup Instructions

### Step 1: Export Unity Project as Android Library

1. Open your Unity project.
2. Go to `File > Build Settings`, select **Android**.
3. Check "Export as Google Android project" (or use Unity as a Library).
4. Export the project and build a `.aar` or `.unitypackage`.

> 📁 Place the exported AAR or Unity files inside the `UnityUaalMaui` project or link it using Gradle if using a native Android module.

---

### Step 2: Add Unity Activity

- The `UnityActivity` class extends `Android.App.Activity` and embeds `UnityPlayerForActivityOrService`.
- Make sure the following metadata are set:

```csharp
[MetaData(name: "unityplayer.UnityActivity", Value = "true")]
[MetaData(name: "notch_support", Value = "true")]
````

* Declare the activity in `AndroidManifest.xml`:

```xml
<activity
    android:name=".UnityActivity"
    android:label="UnityActivity"
    android:configChanges="...full config changes here..."
    android:launchMode="singleTask"
    android:resizeableActivity="false">
    <meta-data android:name="unityplayer.UnityActivity" android:value="true"/>
    <meta-data android:name="notch_support" android:value="true"/>
</activity>
```

---

### Step 3: Handle Permissions

UnityActivity requests camera permissions:

```csharp
if (CheckSelfPermission(Android.Manifest.Permission.Camera) != Permission.Granted)
{
    RequestPermissions(new string[] { Android.Manifest.Permission.Camera }, 1);
}
```

You can extend this to support other Android runtime permissions.

---

### Step 4: Start Unity Activity from MAUI

From your .NET MAUI app, use dependency services or platform-specific logic to launch the Unity activity:

```csharp
var intent = new Intent(Android.App.Application.Context, typeof(UnityActivity));
intent.SetFlags(ActivityFlags.NewTask);
Android.App.Application.Context.StartActivity(intent);
```

---

### Step 5: Bridge Communication

`UnityBridge.SendContent(eventName, eventContent)` allows you to send data to Unity like this:

```csharp
UnityPlayer.UnitySendMessage("Bridge", "ReceiveContent", content);
```

Ensure your Unity script has the `"Bridge"` GameObject with a `ReceiveContent` method.

---

## 🔁 Lifecycle Handling

Unity lifecycle is correctly propagated:

* `OnCreate`, `OnResume`, `OnPause`, `OnStop`, `OnDestroy` map to Unity lifecycle.
* Proper `OnConfigurationChanged`, `OnWindowFocusChanged`, and `OnNewIntent` handling.
* Input events (touch, key, motion) are forwarded to Unity.

---

## 🧪 Testing

* Run the MAUI app on an Android emulator or device.
* Use a button or gesture to launch the Unity activity.
* Verify that Unity starts, responds to input, and communicates via the bridge.

---

## 📚 Reference Links

* [Unity: Unity as a Library](https://docs.unity3d.com/Manual/UnityasaLibrary.html)
* [Microsoft Docs: .NET MAUI](https://learn.microsoft.com/en-us/dotnet/maui/)
* [Xamarin.Android Activity Guide](https://learn.microsoft.com/en-us/xamarin/android/app-fundamentals/activities/)

---

## 🙋‍♂️ Author

**Kwanele**
Contact: \[LinkedIn/GitHub if applicable]

---

## 📄 License

This project is licensed under the MIT License.

```

---

Let me know if you'd like to include screenshots, a diagram of the architecture, or instructions for building with Gradle or Unity in CI/CD.
```
