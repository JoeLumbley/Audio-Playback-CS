# Audio Playback C#

Audio Playback C# is a powerful and versatile tool for managing audio playback using the Windows Multimedia API.
This application provides a comprehensive set of features for playing audio, making it an essential resource for developers and enthusiasts alike.




![002](https://github.com/user-attachments/assets/4e243026-9f35-487b-ad6a-0f8d455c858b)


Key Features:
- Simultaneous Playback: Harness the full potential of the Windows Multimedia API to play multiple audio files simultaneously, allowing for rich and immersive audio experiences.
- Volume Control: Customize the volume levels of individual audio tracks with precision, ensuring an optimal audio balance tailored to your specific requirements.
- Looping and Overlapping: Seamlessly loop audio tracks and play overlapping sounds, enabling the creation of captivating and dynamic audio compositions.
- MCI Integration: Leverage the power of the Media Control Interface (MCI) to interact with multimedia devices, providing a standardized and platform-independent approach to controlling multimedia hardware.
- User-Friendly Interface: Enjoy a user-friendly and intuitive interface, designed to streamline the process of managing and controlling audio playback operations.

With its robust functionality and seamless integration with the Windows Multimedia API, this application empowers users to create engaging multimedia applications with ease. Whether you are a seasoned developer or an aspiring enthusiast, the Audio Playback Application is your gateway to unlocking the full potential of audio playback on the Windows platform.

Clone the repository now and embark on a transformative audio playback experience!





---






# Code Walkthrough

In this walkthrough, we will break down the code that implements an `AudioPlayer` struct and a `Form1` class to manage audio playback.

 [Index](#index)

---


## Namespaces and Struct Definition

```csharp
using System.Runtime.InteropServices;
using System.Text;
using System.Diagnostics;

namespace Audio_Playback_CS
```

- **Namespaces**: These are used to organize code and avoid naming conflicts. Here, we are importing `System.Runtime.InteropServices`, `System.Text`, and `System.Diagnostics`.
- **Struct Definition**: The `AudioPlayer` struct is defined to encapsulate the functionalities related to audio playback.

### DllImport Attribute
```csharp
[DllImport("winmm.dll", EntryPoint = "mciSendStringW")]
private static extern int mciSendStringW([MarshalAs(UnmanagedType.LPTStr)] string lpszCommand,
                                         [MarshalAs(UnmanagedType.LPWStr)] StringBuilder lpszReturnString,
                                         uint cchReturn, IntPtr hwndCallback);
```

- **DllImport**: This attribute allows us to call functions from unmanaged libraries. Here, we are importing `mciSendStringW` from `winmm.dll`, which is used for multimedia control.
- **Parameters**:
  - `lpszCommand`: The command string to send to the MCI (Media Control Interface).
  - `lpszReturnString`: A StringBuilder to store the return string from the command.
  - `cchReturn`: The size of the return string.
  - `hwndCallback`: A handle to a callback window (not used here).

 [Index](#index)

---


## Adding Sounds

```csharp
private string[]? Sounds;

public bool AddSound(string SoundName, string FilePath)
{
    if (!string.IsNullOrWhiteSpace(SoundName) && File.Exists(FilePath))
    {
        string CommandOpen = $"open \"{FilePath}\" alias {SoundName}";

        if (Sounds == null)
        {
            if (SendMciCommand(CommandOpen, IntPtr.Zero))
            {
                Sounds = new string[1];
                Sounds[0] = SoundName;
                return true;
            }
        }
        else if (!Sounds.Contains(SoundName))
        {
            if (SendMciCommand(CommandOpen, IntPtr.Zero))
            {
                Array.Resize(ref Sounds, Sounds.Length + 1);
                Sounds[Sounds.Length - 1] = SoundName;
                return true;
            }
        }
    }

    Debug.Print($"The sound was not added {SoundName}");
    return false;
}
```

- **Private Field**: `private string[]? Sounds;` is an array that will hold the names of the sounds we have added.
- **Method `AddSound`**:
  - Checks if `SoundName` is not empty and if the file exists.
  - Constructs a command to open the sound file and assigns it an alias.
  - If `Sounds` is null (no sounds added yet), it opens the sound file and initializes the array.
  - If sounds already exist, it checks if the sound is not already in the array before adding it.
  - Returns `true` if the sound was successfully added; otherwise, it logs a message and returns `false`.

 [Index](#index)

---


## Setting Volume

```csharp
public bool SetVolume(string SoundName, int Level)
{
    if (Sounds != null && Sounds.Contains(SoundName) && Level >= 0 && Level <= 1000)
    {
        string CommandVolume = $"setaudio {SoundName} volume to {Level}";
        return SendMciCommand(CommandVolume, IntPtr.Zero);
    }

    Debug.Print($"The volume was not set {SoundName}");
    return false;
}
```

- **Method `SetVolume`**:
  - Checks if `Sounds` is not null, if the sound exists, and if the volume level is within the valid range (0 to 1000).
  - Constructs a command to set the audio volume for the specified sound.
  - Sends the command using `SendMciCommand` and returns the result.
  - Logs a message and returns `false` if the conditions are not met.

 [Index](#index)

---


## Looping Sounds

```csharp
public bool LoopSound(string SoundName)
{
    if (Sounds != null && Sounds.Contains(SoundName))
    {
        string CommandSeekToStart = $"seek {SoundName} to start";
        string CommandPlayRepeat = $"play {SoundName} repeat";
        return SendMciCommand(CommandSeekToStart, IntPtr.Zero) &&
               SendMciCommand(CommandPlayRepeat, IntPtr.Zero);
    }

    Debug.Print($"The sound is not looping {SoundName}");
    return false;
}
```

- **Method `LoopSound`**:
  - Checks if the sound exists in the `Sounds` array.
  - Constructs commands to seek to the start of the sound and play it in repeat mode.
  - Sends both commands and returns `true` if successful; otherwise, logs a message and returns `false`.

 [Index](#index)

---


## Playing Sounds

```csharp
private bool PlaySound(string SoundName)
{
    if (Sounds != null && Sounds.Contains(SoundName))
    {
        string CommandSeekToStart = $"seek {SoundName} to start";
        string CommandPlay = $"play {SoundName} notify";
        return SendMciCommand(CommandSeekToStart, IntPtr.Zero) &&
               SendMciCommand(CommandPlay, IntPtr.Zero);
    }

    Debug.Print($"The sound is not playing {SoundName}");
    return false;
}
```

- **Method `PlaySound`**:
  - Similar to `LoopSound`, but constructs commands to play the sound once.
  - Uses `notify` to allow the program to receive notification when the sound finishes playing.
  - Returns `true` if the commands were successful; otherwise, it logs a message and returns `false`.

 [Index](#index)

---


## Pausing Sounds

```csharp
public bool PauseSound(string SoundName)
{
    if (Sounds != null && Sounds.Contains(SoundName))
    {
        string CommandPause = $"pause {SoundName} notify";
        return SendMciCommand(CommandPause, IntPtr.Zero);
    }

    Debug.Print($"The sound is not paused {SoundName}");
    return false;
}
```

- **Method `PauseSound`**:
  - Checks if the sound exists.
  - Constructs a command to pause the sound and sends it.
  - Returns `true` if successful; otherwise, logs a message and returns `false`.

 [Index](#index)

---


## Managing Overlapping Sounds

### Adding Overlapping Sounds
```csharp
public void AddOverlapping(string SoundName, string FilePath)
{
    foreach (string Suffix in new[] { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L" })
    {
        AddSound(SoundName + Suffix, FilePath);
    }
}
```

- **Method `AddOverlapping`**:
  - Adds multiple sounds with suffixes (A to L) to allow overlapping playback.
  - Calls `AddSound` for each suffixed name.

### Playing Overlapping Sounds
```csharp
public void PlayOverlapping(string SoundName)
{
    foreach (string Suffix in new[] { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L" })
    {
        if (!IsPlaying(SoundName + Suffix))
        {
            PlaySound(SoundName + Suffix);
            return;
        }
    }
}
```

- **Method `PlayOverlapping`**:
  - Plays the first sound that is not currently playing among the suffixed sounds.

### Setting Volume for Overlapping Sounds
```csharp
public void SetVolumeOverlapping(string SoundName, int Level)
{
    foreach (string Suffix in new[] { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L" })
    {
        SetVolume(SoundName + Suffix, Level);
    }
}
```

- **Method `SetVolumeOverlapping`**:
  - Sets the volume for all suffixed sounds using the `SetVolume` method.

 [Index](#index)

---


## Sending MCI Commands

```csharp
private bool SendMciCommand(string command, IntPtr hwndCallback)
{
    StringBuilder ReturnString = new StringBuilder(128);

    try
    {
        return mciSendStringW(command, ReturnString, 0, hwndCallback) == 0;
    }
    catch (Exception ex)
    {
        Debug.Print($"Error: {ex.Message}");
        return false;
    }
}
```

- **Method `SendMciCommand`**:
  - Sends a command to the MCI and checks for errors.
  - Returns `true` if the command was successful; otherwise, logs the error and returns `false`.

 [Index](#index)

---


## Getting Sound Status

```csharp
private string GetStatus(string SoundName, string StatusType)
{
    try
    {
        if (Sounds != null && Sounds.Contains(SoundName))
        {
            string CommandStatus = $"status {SoundName} {StatusType}";
            StringBuilder StatusReturn = new StringBuilder(128);
            mciSendStringW(CommandStatus, StatusReturn, 128, IntPtr.Zero);
            return StatusReturn.ToString().Trim().ToLower();
        }
    }
    catch (Exception ex)
    {
        Debug.Print($"Error getting status: {ex.Message}");
    }

    return string.Empty;
}
```

- **Method `GetStatus`**:
  - Retrieves the status of a sound (e.g., whether it is playing).
  - Constructs a status command and returns the result as a string.

 [Index](#index)

---


## Closing Sounds

```csharp
public void CloseSounds()
{
    if (Sounds != null)
    {
        foreach (string Sound in Sounds)
        {
            string CommandClose = $"close {Sound}";
            SendMciCommand(CommandClose, IntPtr.Zero);
        }
    }
}
```

- **Method `CloseSounds`**:
  - Closes all open sounds by sending a close command for each sound in the `Sounds` array.

 [Index](#index)

---


## Form Class and Event Handlers

```csharp
public partial class Form1 : Form
{
    private AudioPlayer Player;

    private void Form1_Load(object sender, EventArgs e)
    {
        Text = "Audio Playback CS - Code with Joe";

        CreateSoundFiles();

        string FilePath = Path.Combine(Application.StartupPath, "level.mp3");
        Player.AddSound("Music", FilePath);
        Player.SetVolume("Music", 600);

        FilePath = Path.Combine(Application.StartupPath, "CashCollected.mp3");
        Player.AddOverlapping("CashCollected", FilePath);
        Player.SetVolumeOverlapping("CashCollected", 900);

        Player.LoopSound("Music");

        Debug.Print($"Running... {DateTime.Now}");
    }
```

- **Form1 Class**: Inherits from `Form`, which is part of Windows Forms for creating GUI applications.
- **Player Field**: An instance of `AudioPlayer` is created to manage audio playback.
- **Form1_Load Method**:
  - Sets the form title.
  - Calls `CreateSoundFiles` to ensure the sound files exist.
  - Adds sounds and sets their volumes.
  - Loops the background music and logs the current time.

### Button Click Events
```csharp
private void Button1_Click(object sender, EventArgs e)
{
    Player.PlayOverlapping("CashCollected");
}

private void Button2_Click(object sender, EventArgs e)
{
    if (Player.IsPlaying("Music"))
    {
        Player.PauseSound("Music");
        button2.Text = "Play Loop";
    }
    else
    {
        Player.LoopSound("Music");
        button2.Text = "Pause Loop";
    }
}
```
- **Button1_Click**: Plays the overlapping "CashCollected" sound when the button is clicked.
- **Button2_Click**: Toggles between pausing the music and looping it, updating the button text accordingly.

### Form Closing Event
```csharp
private void Form1_Closing(object sender, System.ComponentModel.CancelEventArgs e)
{
    Player.CloseSounds();
}
```
- **Form1_Closing**: Ensures all sounds are closed when the form is closing.

 [Index](#index)

---


## Creating Sound Files

```csharp
private void CreateSoundFiles()
{
    string filePath = Path.Combine(Application.StartupPath, "level.mp3");
    CreateFileFromResource(filePath, Audio_Playback_CS.Resource1.level);

    filePath = Path.Combine(Application.StartupPath, "CashCollected.mp3");
    CreateFileFromResource(filePath, Audio_Playback_CS.Resource1.CashCollected);
}
```

- **Method `CreateSoundFiles`**:
  - Creates sound files from resources if they do not already exist.

### Creating Files from Resources
```csharp
private void CreateFileFromResource(string filePath, byte[] resource)
{
    try
    {
        if (!File.Exists(filePath))
        {
            File.WriteAllBytes(filePath, resource);
        }
    }
    catch (Exception ex)
    {
        Debug.Print($"Error creating file: {ex.Message}");
    }
}
```
- **Method `CreateFileFromResource`**:
  - Writes byte arrays from resources to files on disk.
  - Catches exceptions and logs errors if file creation fails.

---


This code provides a comprehensive example of how to create an audio playback application in C#. We covered everything from adding sounds to managing their playback and volume. By understanding each part of this code, you can build a solid foundation for working with audio in your applications.

Feel free to experiment with the code and modify it to enhance your learning experience!


---

## Index





 [Namespaces and Struct Definition](#namespaces-and-struct-definition)
 
 [Adding Sounds](#adding-sounds)
 
 [Setting Volume](#setting-volume)
 
 [Looping Sounds](#looping-sounds)
 
 [Playing Sounds](#playing-sounds)
 
 [Pausing Sounds](#pausing-sounds)
 
 [Managing Overlapping Sounds](#managing-overlapping-sounds)
 
 [Sending MCI Commands](#sending-mci-commands)
 
 [Getting Sound Status](#getting-sound-status)
 
 [Closing Sounds](#closing-sounds)
 
 [Form Class and Event Handlers](#form-class-and-event-handlers)
 
 [Creating Sound Files](#creating-sound-files)


---







![003](https://github.com/user-attachments/assets/2c97331f-3adb-4e6f-aafa-78a7de57165c)







## Adding Resources

To add an existing MP3 file to the resource file `Resource1`, follow these steps:

1. **Open the Resource File**:
   - In your Visual Studio project, locate the `Resource1.resx` file. This file is usually found in the **"Solution Explorer"** panel of your project.

2. **Edit the Resource File**:
   - Double-click on `Resource1.resx` to open the resource editor.

3. **Add Existing File**:
   - In the resource editor, click on the **"Green Plus Sign"** to add a new resource.
   - Select the type **"File"** and then choose **"Add Existing File..."**.

4. **Select Your MP3 File**:
   - Navigate to the location of your MP3 file in the file dialog that appears.
   - Select the MP3 file you wish to add and click **"Open"**.

5. **Verify the Addition**:
   - Ensure that your MP3 file appears in the list of resources in the resource editor. It should now be accessible via the `Resource1` class in your code.

6. **Accessing the Resource in Code**:
   - You can access the added MP3 file in your code using the following syntax:
     ```csharp
     byte[] audioData = Resource1.yourMp3FileName; // Replace 'yourMp3FileName' with the name of your MP3 file
     ```

7. **Save Changes**:
   - Save the changes to the `Resource1.resx` file.

By following these steps, you can easily add any existing MP3 file to your resources and use it within your Audio Playback application.













