```markdown
# UnityUaalMaui

This repository demonstrates how to embed a **Unity Android activity** inside a **.NET MAUI** application using **Unity as a Library (UAAL)** integration. The Unity activity is launched from MAUI and communicates back and forth using a bridge layer.

## 📌 Features

- Unity embedded as a secondary Android activity in a .NET MAUI app.
- Unity exported as an `.aar` and included under `Platforms/Android/jars`.
- Complete lifecycle management (pause, resume, destroy).
- Communication bridge between MAUI and Unity using Unity’s `UnitySendMessage`.
- Permission request support (e.g., camera).
- Input handling (touch, key events).

---

## 🛠️ Prerequisites

- **Visual Studio 2022/2023** with .NET MAUI workload.
- **Unity Editor 2021+**.
- **Android SDK/NDK** correctly set up.
- Basic understanding of:
  - MAUI and Xamarin.Android internals
  - Unity Android builds using UAAL
  - Android native activity management

---

## 📂 Project Structure

---

## 🧩 Setup Instructions

### Step 1: Export Unity Project as AAR

1. Open Unity.
2. Switch platform to **Android**.
3. Under `File > Build Settings`, enable **Export as a Google Android Project** or build as an **Android Library (.aar)** using Unity's `UnityLibrary` template.
4. Build and locate the generated `.aar` (usually in `/build/outputs/aar`).
5. Copy the `.aar` into `Platforms/Android/jars/`.

> ✅ No need to use Gradle. The `.aar` file will be packaged with your MAUI app as a direct reference.

---

### Step 2: Register Unity Activity

The `UnityActivity` is a native Android activity that loads UnityPlayer.

Add it to `AndroidManifest.xml`:

```xml
<activity
    android:name=".UnityActivity"
    android:label="UnityActivity"
    android:configChanges="keyboardHidden|orientation|screenSize"
    android:launchMode="singleTask"
    android:resizeableActivity="false">
    <meta-data android:name="unityplayer.UnityActivity" android:value="true"/>
    <meta-data android:name="notch_support" android:value="true"/>
</activity>
````

> 📍 Required to support lifecycle management, screen rotation, and correct Unity context binding.

---

### Step 3: Launch Unity from MAUI

Use a platform-specific service or dependency injection to launch Unity:

```csharp
var intent = new Intent(Android.App.Application.Context, typeof(UnityActivity));
intent.SetFlags(ActivityFlags.NewTask);
Android.App.Application.Context.StartActivity(intent);
```

---

### Step 4: Communicate with Unity

The `UnityBridge` and `MessageDispatcher` help communicate between MAUI and Unity.

To send a message to Unity:

```csharp
UnityBridge.SendContent("GameEvent", "PlayerJoined");
```

Inside Unity, a GameObject named `Bridge` should have a script like:

```csharp
public class Bridge : MonoBehaviour
{
    public void ReceiveContent(string message)
    {
        Debug.Log("Message from MAUI: " + message);
    }
}
```

Make sure the `Bridge` GameObject is present in the Unity scene and loaded at start.

---

### Step 5: Handle Permissions (Optional)

UnityActivity requests permissions like camera at runtime:

```csharp
if (CheckSelfPermission(Android.Manifest.Permission.Camera) != Permission.Granted)
{
    RequestPermissions(new string[] { Android.Manifest.Permission.Camera }, 1);
}
```

Add other permissions as needed.

---

## 🔁 Lifecycle Handling

Your custom `UnityActivity` ensures UnityPlayer follows Android’s activity lifecycle:

* `OnCreate`, `OnStart`, `OnResume`, `OnPause`, `OnStop`, `OnDestroy`
* Configuration changes
* Window focus
* Input events (motion, keypress, touch)

These are passed properly to Unity’s `UnityPlayer`.

---

## 🧪 Testing

1. Run the MAUI app on an Android emulator or device.
2. Use a button or gesture to start Unity.
3. Interact with the Unity content.
4. Check that messages are sent from MAUI to Unity and vice versa.

---

## 📚 Resources

* [Unity: Unity as a Library](https://docs.unity3d.com/Manual/UnityasaLibrary.html)
* [.NET MAUI Documentation](https://learn.microsoft.com/en-us/dotnet/maui/)
* [Unity SendMessage](https://docs.unity3d.com/ScriptReference/GameObject.SendMessage.html)

---

## 🙋‍♂️ Author

**Kwanele**
[GitHub](https://github.com/kwanele09) | [LinkedIn](https://linkedin.com/in/your-profile)

---

## 📄 License

This project is licensed under the MIT License.

```

---

Would you like me to also generate:

- 📸 Example screenshots or GIFs of Unity inside MAUI?
- 📦 CI/CD pipeline instructions (e.g., GitHub Actions to build AAR and inject it)?
- 🧪 Unit testing guidance for the MAUI–Unity interaction?

Let me know what you’d like added or adjusted!
```
