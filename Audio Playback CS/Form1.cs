// Audio Playback C#

// Uses Windows Multimedia API for playback of multiple audio files simultaneously.

// MIT License
// Copyright(c) 2022 Joseph W. Lumbley

// Permission Is hereby granted, free Of charge, to any person obtaining a copy
// of this software And associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, And/Or sell
// copies of the Software, And to permit persons to whom the Software Is
// furnished to do so, subject to the following conditions:

// The above copyright notice And this permission notice shall be included In all
// copies Or substantial portions of the Software.

// THE SOFTWARE Is PROVIDED "AS IS", WITHOUT WARRANTY Of ANY KIND, EXPRESS Or
// IMPLIED, INCLUDING BUT Not LIMITED To THE WARRANTIES Of MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE And NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS Or COPYRIGHT HOLDERS BE LIABLE For ANY CLAIM, DAMAGES Or OTHER
// LIABILITY, WHETHER In AN ACTION Of CONTRACT, TORT Or OTHERWISE, ARISING FROM,
// OUT OF Or IN CONNECTION WITH THE SOFTWARE Or THE USE Or OTHER DEALINGS IN THE
// SOFTWARE.

// Level music by Joseph Lumbley Jr.

using System.Runtime.InteropServices;
using System.Text;

namespace Audio_Playback_CS
{
    public partial class Form1 : Form
    {

        [DllImport("winmm.dll", EntryPoint = "mciSendStringW")]
        private static extern int mciSendStringW([MarshalAs(UnmanagedType.LPTStr)] string lpszCommand,
                                                 [MarshalAs(UnmanagedType.LPWStr)] StringBuilder lpszReturnString,
                                                 uint cchReturn,
                                                 IntPtr hwndCallback);

        private  string[]?  Sounds;

        private void Form1_Load(object sender, EventArgs e)
        {
            Text = "Audio Playback CS - Code with Joe";

            CreateSoundFileFromResource();

            string FilePath = Path.Combine(Application.StartupPath, "level.mp3");

            AddSound("Music", FilePath);

            SetVolume("Music", 600);

            FilePath = Path.Combine(Application.StartupPath, "CashCollected.mp3");

            AddOverlapping("CashCollected", FilePath);

            SetVolumeOverlapping("CashCollected", 900);

            LoopSound("Music");

        }

        private void Button1_Click(object sender, EventArgs e)
        {
            PlayOverlapping("CashCollected");

        }

        private void Button2_Click(object sender, EventArgs e)
        {
            if (IsPlaying("Music"))
            {
                PauseSound("Music");

                button2.Text = "Play Loop";
            }
            else
            {
                LoopSound("Music");

                button2.Text = "Pause Loop";

            }

        }

        private void Form1_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            CloseSounds();

        }

        private bool AddSound(string SoundName, string FilePath)
        {
            // Do we have a name and does the file exist?
            if (!string.IsNullOrWhiteSpace(SoundName) && File.Exists(FilePath))
            {   // Yes, we have a name and the file exists.

                string CommandOpen = $"open \"{FilePath}\" alias {SoundName}";

                StringBuilder returnString = new(128);

                // Do we have sounds?
                if (Sounds != null)
                {   // Yes, we have sounds.

                    // Is the sound in the array already?
                    if (!Sounds.Contains(SoundName))
                    {   // No, the sound is not in the array.

                        // Did the sound file open?
                        if (mciSendStringW(CommandOpen, returnString, 0, IntPtr.Zero) == 0)
                        {   // Yes, the sound file did open.

                            // Add the sound to the Sounds array.
                            Array.Resize(ref Sounds, Sounds.Length + 1);

                            Sounds[^1] = SoundName;

                            return true; // The sound was added.

                        }
                    }

                }
                else
                {   // No, we do not have sounds.

                    // Did the sound file open?
                    if (mciSendStringW(CommandOpen, returnString, 0, IntPtr.Zero) == 0)
                    {   // Yes, the sound file did open.

                        // Start the Sounds array with the sound.
                        Sounds = [SoundName];

                        return true; // The sound was added.



                    }

                }

            }

            return false; // The sound was not added.

        }

        private bool SetVolume(string SoundName, int Level)
        {
            // Do we have sounds?
            if (Sounds != null)
            {   // Yes, we have sounds.

                // Is the sound in the sounds array?
                if (Sounds.Contains(SoundName))
                {   // Yes, the sound is the sounds array.

                    // Is the level in the valid range?
                    if (Level >= 0 && Level <= 1000)
                    {   // Yes, the sound is the sounds array.

                        string CommandVolume = $"setaudio {SoundName} volume to {Level}";

                        StringBuilder returnString = new(128);

                        if (mciSendStringW(CommandVolume, returnString, 0, IntPtr.Zero) == 0)
                        {

                            return true; // The volume was set.

                        }

                    }

                }

            }

            return false;

        }

        private bool LoopSound(string SoundName)
        {
            // Do we have sounds?
            if (Sounds != null)
            {   // Yes, we have sounds.

                // Is the sound in the array?
                if (!Sounds.Contains(SoundName))
                {   // No, the sound is not in the array.

                    return false;

                }

                string CommandSeekToStart = $"seek {SoundName} to start";

                string CommandPlayRepeat = $"play {SoundName} repeat";

                StringBuilder returnString = new(128);

                mciSendStringW(CommandSeekToStart, returnString, 0, IntPtr.Zero);

                if (mciSendStringW(CommandPlayRepeat, returnString, 0, Handle) != 0)
                {
                    return false; // The sound is not playing.

                }

            }

            return true; // The sound is playing.

        }

        private bool PlaySound(string SoundName)
        {
            // Do we have sounds?
            if (Sounds != null)
            {   // Yes, we have sounds.

                // Is the sound in the array?
                if (Sounds.Contains(SoundName))
                {   // Yes, the sound is in the array.

                    string CommandSeekToStart = $"seek {SoundName} to start";

                    string CommandPlay = $"play {SoundName} notify";

                    StringBuilder returnString = new(128);

                    mciSendStringW(CommandSeekToStart, returnString, 0, IntPtr.Zero);

                    if (mciSendStringW(CommandPlay, returnString, 0, Handle) == 0)
                    {
                        return true; // The sound is playing.

                    }

                }

            }

            return false; // The sound is not playing.

        }

        private bool PauseSound(string SoundName)
        {
            // Do we have sounds?
            if (Sounds != null)
            {   // Yes, we have sounds.

                // Is the sound in the array?
                if (Sounds.Contains(SoundName))
                {   // Yes, the sound is in the array.

                    string CommandPause = $"pause {SoundName} notify";

                    StringBuilder returnString = new(128);

                    if (mciSendStringW(CommandPause, returnString, 0, Handle) == 0)
                    {
                        return true; // The sound is playing.

                    }

                }

            }

            return false; // The sound is not playing.

        }

        private bool IsPlaying(string SoundName)
        {
            return GetStatus(SoundName, "mode") == "playing";

        }

        private void AddOverlapping(string SoundName, string FilePath)
        {
            foreach (var suffix in new[] { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L" })
            {
                AddSound(SoundName + suffix, FilePath);

            }

        }

        private void PlayOverlapping(string SoundName)
        {
            foreach (var suffix in new[] { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L" })
            {
                if (!IsPlaying(SoundName + suffix))
                {
                    PlaySound(SoundName + suffix);

                    break;

                }

            }

        }

        private void SetVolumeOverlapping(string SoundName, int Level)
        {
            foreach (var suffix in new[] { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L" })
            {
                SetVolume(SoundName + suffix, Level);

            }

        }

        private string GetStatus(string SoundName, string StatusType)
        {
            if (Sounds != null)
            {
                if (Sounds.Contains(SoundName))
                {
                    string CommandStatus = $"status {SoundName} {StatusType}";

                    StringBuilder StatusReturn = new(128);

                    mciSendStringW(CommandStatus, StatusReturn, 128, IntPtr.Zero);

                    return StatusReturn.ToString().Trim().ToLower();

                }

            }

            return string.Empty;

        }

        private void CloseSounds()
        {
            if (Sounds != null)
            {
                foreach (var sound in Sounds)
                {
                    string CommandClose = $"close {sound}";

                    StringBuilder returnString = new(128);

                    mciSendStringW(CommandClose, returnString, 0, IntPtr.Zero);

                }

            }

        }

        private void CreateSoundFileFromResource()
        {
            string FilePath = Path.Combine(Application.StartupPath, "level.mp3");

            if (!File.Exists(FilePath))
            {
                File.WriteAllBytes(FilePath, Audio_Playback_CS.Resource1.level);

            }

            FilePath = Path.Combine(Application.StartupPath, "CashCollected.mp3");

            if (!File.Exists(FilePath))
            {
                File.WriteAllBytes(FilePath, Audio_Playback_CS.Resource1.CashCollected);

            }

        }

        public Form1()
        {
            InitializeComponent();
        }


    }
}


//'Windows Multimedia

//'Windows Multimedia refers to the collection of technologies and APIs (Application Programming Interfaces)
//'provided by Microsoft Windows for handling multimedia tasks on the Windows operating system.

//'It includes components for playing audio and video, recording sound, working with MIDI devices, managing
//'multimedia resources, and controlling multimedia hardware.

//'Windows Multimedia APIs like DirectShow, DirectX, Media Control Interface, and others enable developers
//'to create multimedia applications that can interact with various multimedia devices and perform tasks
//'related to multimedia playback, recording, and processing.

//'https://learn.microsoft.com/en-us/windows/win32/multimedia/windows-multimedia-start-page


//'Media Control Interface

//'The Media Control Interface (MCI) is a high-level programming interface provided by Microsoft Windows
//'for controlling multimedia devices such as CD-ROM drives, audio and video devices, and other multimedia
//'hardware.

//'MCI provides a standard way for applications to interact with multimedia devices without needing to know
//'the specific details of each device's hardware or communication protocols.

//'By using MCI commands and functions, applications can play, record, pause, stop, and otherwise control
//'multimedia playback and recording devices in a consistent and platform-independent manner.

//'https://learn.microsoft.com/en-us/windows/win32/multimedia/mci


//'mciSendStringW Function

//'mciSendStringW is a function that is used to send a command string to an MCI device.

//'The "W" at the end of the function name indicates that it is the wide-character version of the function,
//'which means it accepts Unicode strings.

//'This function allows applications to control multimedia devices and perform operations such as playing
//'audio or video, recording sound, and managing multimedia resources by sending commands in the form of
//'strings to MCI devices.

//'https://learn.microsoft.com/en-us/previous-versions//dd757161(v=vs.85)


//'open Command

//'The "open" command is used in the Windows Multimedia API to open or initialize an MCI device for playback,
//'recording or other multimedia operations.

//'By sending an MCI command string with the "open" command using mciSendStringW, applications can specify
//'the type of multimedia device to open (such as a CD-ROM drive, sound card, or video device), the file or
//'resource to be accessed and any additional parameters required for the operation.

//'This command is essential for preparing a multimedia device for use before performing playback, recording,
//'or other actions on it.

//'https://learn.microsoft.com/en-us/windows/win32/multimedia/open


//'setaudio Command

//'The "setaudio" command is used to set the audio parameters for a multimedia device.

//'When sending an MCI command string with the "setaudio" command using the mciSendStringW function,
//'applications can adjust settings such as volume, balance, speed, and other audio-related properties of the
//'specified multimedia device.

//'This command allows developers to control and customize the audio playback characteristics of the device
//'to meet specific requirements or user preferences.

//'https://learn.microsoft.com/en-us/windows/win32/multimedia/setaudio


//'seek Command

//'The "seek" command is used to move the current position of playback or recording to a specified location
//'within a multimedia resource.

//'When sending an MCI command string with the "seek" command using the mciSendStringW function,
//'applications can specify the position or time where playback should start or resume within the multimedia
//'content.

//'This command allows developers to navigate to a specific point in audio or video playback, facilitating
//'precise control over multimedia playback operations.

//'https://learn.microsoft.com/en-us/windows/win32/multimedia/seek


//'play Command

//'The "play" command is used to start or resume playback of a multimedia resource.

//'When sending an MCI command string with the "play" command using the mciSendStringW function, applications
//'can instruct the multimedia device to begin playing the specified audio or video content from the current
//'position.

//'This command is essential for initiating playback of multimedia files, allowing developers to control the
//'start and continuation of audio or video playback operations using MCI commands.

//'https://learn.microsoft.com/en-us/windows/win32/multimedia/play


//'status Command

//'The "status" command is used to retrieve information about the current status of a multimedia device or
//'resource.

//'When sending an MCI command string with the "status" command using the mciSendStringW function,
//'applications can query various properties and states of the specified multimedia device, such as playback
//'position, volume level, mode (playing, paused, stopped), and other relevant information.

//'This command allows developers to monitor and obtain real-time feedback on the status of multimedia
//'playback or recording operations, enabling them to make informed decisions based on the device's current
//'state.

//'https://learn.microsoft.com/en-us/windows/win32/multimedia/status


//'close Command

//'The "close" command is used to close or release a multimedia device that was previously opened for
//'playback, recording, or other operations.

//'When sending an MCI command string with the "close" command using the mciSendStringW function,
//'applications can instruct the multimedia device to release any resources associated with the device and
//'prepare it for shutdown.

//'This command is essential for properly closing and cleaning up after using a multimedia device, ensuring
//'that resources are properly released and the device is no longer in use by the application.

//'https://learn.microsoft.com/en-us/windows/win32/multimedia/close


//'pause Command

//'The pause command is used to temporarily halt the playback of media content, allowing the user to resume
//'playback from the paused position at a later time.

//'https://learn.microsoft.com/en-us/windows/win32/multimedia/pause



//'Monica is our an AI assistant.
//'https://monica.im/


//'I also make coding videos on my YouTube channel.
//'https://www.youtube.com/@codewithjoe6074

