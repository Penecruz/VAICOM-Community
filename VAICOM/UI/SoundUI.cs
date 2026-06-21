using NAudio.Wave;
using System.IO;
using System.Media;

namespace VAICOM
{

    namespace UI
    {

        public static class Playsound
        {

            public static void AddSoundToMixer(object playbackfile)
            {
                try
                {
                    if (State.ttsmixer == null || !State.currentaudiodevicevalid)
                    {
                        return;
                    }

                    Stream fragment = (Stream)playbackfile;
                    WaveFileReader reader = new WaveFileReader(fragment);
                    var upsampler = new WaveFormatConversionStream(new WaveFormat(22050, 16, 2), reader);
                    var vol = new NAudio.Wave.SampleProviders.VolumeSampleProvider(upsampler.ToSampleProvider());
                    vol.Volume = 1.0f;
                    State.ttsmixer.AddMixerInput(vol);
                }
                catch
                {
                }
            }

            public static void TestNoise()
            {

                if (State.activeconfig.Redirect_World_Speech)
                {
                    AddSoundToMixer(VAICOM.Properties.Resources.Radio_Noise_Test);
                }
                else
                {
                    SoundPlayer audio = new SoundPlayer(VAICOM.Properties.Resources.Radio_Noise_Test);
                    audio.Play();
                }
            }

            public static void Startup()
            {

                if (State.activeconfig.Redirect_World_Speech)
                {
                    AddSoundToMixer(VAICOM.Properties.Resources.UI_Confirmation);
                }
                else
                {
                    SoundPlayer audio = new SoundPlayer(VAICOM.Properties.Resources.UI_Confirmation);
                    audio.Play();
                }
            }

            public static void Commandstart()
            {
                if (State.activeconfig.UIsounds)
                {
                    if (State.activeconfig.Redirect_World_Speech)
                    {
                        AddSoundToMixer(VAICOM.Properties.Resources.UI_Confirmation);
                    }
                    else
                    {
                        SoundPlayer audio = new SoundPlayer(VAICOM.Properties.Resources.UI_Confirmation);
                        audio.Play();
                    }
                }
            }

            public static void Commandcomplete()
            {
                if (State.activeconfig.UIsounds)
                {
                    if (State.activeconfig.Redirect_World_Speech)
                    {
                        AddSoundToMixer(VAICOM.Properties.Resources.UI_Confirmation);
                    }
                    else
                    {
                        SoundPlayer audio = new SoundPlayer(VAICOM.Properties.Resources.UI_Confirmation);
                        audio.Play();
                    }
                }
            }

            public static void Recipientna()
            {

                if (State.activeconfig.UIsounds & State.activeconfig.UIaddhints)
                {
                    if (State.activeconfig.Redirect_World_Speech)
                    {
                        AddSoundToMixer(VAICOM.Properties.Resources.UI_Recipient_NA);
                    }
                    else
                    {
                        SoundPlayer audio = new SoundPlayer(VAICOM.Properties.Resources.UI_Recipient_NA);
                        audio.Play();
                    }
                }
            }

            public static void Pttnoise(bool press)
            {
                if (!State.activeconfig.DisableSquelch && !(State.currentstate.viewexternal && !State.currentstate.soundsallowexternal))
                {
                    if (State.activeconfig.Redirect_World_Speech)
                    {
                        if (press)
                        {
                            AddSoundToMixer(VAICOM.Properties.Resources.UI_RadioClickBefore_Alt);
                        }
                        else
                        {
                            AddSoundToMixer(VAICOM.Properties.Resources.UI_RadioClickAfter_Alt);
                        }
                    }
                    else
                    {
                        SoundPlayer audio;
                        if (press)
                        {
                            audio = new SoundPlayer(VAICOM.Properties.Resources.UI_RadioClickBefore_Alt);
                        }
                        else
                        {
                            audio = new SoundPlayer(VAICOM.Properties.Resources.UI_RadioClickAfter_Alt);
                        }
                        audio.Play();
                    }
                }
            }

            public static void Error()
            {
                try
                {
                    if (State.activeconfig.UIsounds)
                    {
                        if (State.activeconfig.Redirect_World_Speech)
                        {
                            AddSoundToMixer(VAICOM.Properties.Resources.UI_Error);
                        }
                        else
                        {
                            SoundPlayer audio = new SoundPlayer(VAICOM.Properties.Resources.UI_Error);
                            audio.Play();
                        }
                    }
                }
                catch
                {
                }
            }

            public static void Sorry()
            {
                try
                {
                    if (State.activeconfig.UIsounds)
                    {
                        if (State.activeconfig.Redirect_World_Speech)
                        {
                            AddSoundToMixer(VAICOM.Properties.Resources.UI_Ambiguous);
                        }
                        else
                        {
                            SoundPlayer audio = new SoundPlayer(VAICOM.Properties.Resources.UI_Ambiguous);
                            audio.Play();
                        }
                    }
                }
                catch
                {
                }
            }

            public static void Proceed()
            {
                try
                {

                    if (State.activeconfig.UIsounds)
                    {

                        if (State.activeconfig.Redirect_World_Speech)
                        {
                            AddSoundToMixer(VAICOM.Properties.Resources.UI_Reply);
                        }
                        else
                        {
                            SoundPlayer audio = new SoundPlayer(VAICOM.Properties.Resources.UI_Reply);
                            audio.Play();
                        }
                    }
                }
                catch
                {
                }
            }

            public static void Click()
            {
                if (State.activeconfig.Redirect_World_Speech)
                {
                    AddSoundToMixer(VAICOM.Properties.Resources.UI_Click);
                }
                else
                {
                    SoundPlayer audio = new SoundPlayer(VAICOM.Properties.Resources.UI_Click);
                    audio.Play();
                }
            }

            public static void Soft_Click()
            {
                if (State.activeconfig.Redirect_World_Speech)
                {
                    AddSoundToMixer(VAICOM.Properties.Resources.UI_Pushbutton_Soft);
                }
                else
                {
                    SoundPlayer audio = new SoundPlayer(VAICOM.Properties.Resources.UI_Pushbutton_Soft);
                    audio.Play();
                }
            }

            public static void Soft_Switch()
            {
                if (State.activeconfig.Redirect_World_Speech)
                {
                    AddSoundToMixer(VAICOM.Properties.Resources.UI_Switch_Soft);
                }
                else
                {
                    SoundPlayer audio = new SoundPlayer(VAICOM.Properties.Resources.UI_Switch_Soft);
                    audio.Play();
                }
            }

            public static void Gum_Soft()
            {
                if (State.activeconfig.Redirect_World_Speech)
                {
                    AddSoundToMixer(VAICOM.Properties.Resources.UI_Gum_Soft);
                }
                else
                {
                    SoundPlayer audio = new SoundPlayer(VAICOM.Properties.Resources.UI_Gum_Soft);
                    audio.Play();
                }
            }

            public static void Dial_Click()
            {
                if (State.activeconfig.Redirect_World_Speech)
                {
                    AddSoundToMixer(VAICOM.Properties.Resources.UI_Dial_Click);
                }
                else
                {
                    SoundPlayer audio = new SoundPlayer(VAICOM.Properties.Resources.UI_Dial_Click);
                    audio.Play();
                }
            }

            public static void Human_Comms_Active()
            {
                if (!State.activeconfig.MP_VoIPParallel && (State.activeconfig.MP_UseSRSIntegration || State.activeconfig.MP_UseVoiceChatIntegration))
                {
                    if (State.activeconfig.Redirect_World_Speech)
                    {
                        AddSoundToMixer(VAICOM.Properties.Resources.UI_Notify);
                    }
                    else
                    {
                        SoundPlayer audio = new SoundPlayer(VAICOM.Properties.Resources.UI_Notify);
                        audio.Play();
                    }
                }
            }

            public static void Endofcall()
            {
                //SoundPlayer audio = new SoundPlayer(VAICOM.Properties.Resources.UI_RadioClickAfter);
                //audio.Play();
            }
        }
    }
}
