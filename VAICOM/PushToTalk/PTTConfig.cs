using System.Windows.Forms;

namespace VAICOM
{

    namespace PushToTalk
    {

        public partial class PTT
        {
            public static void Update_PTT_GUI()
            {
                try
                {
                    if (State.configwindowopen && (State.configurationwindow != null))
                    {
                        State.configurationwindow.Dispatcher.BeginInvoke((MethodInvoker)delegate
                        {
                            State.configurationwindow.updatepttinfo();
                        });
                    }
                }
                catch
                {
                }
            }

            public static void PTT_ApplyNewConfig()
            {

                if (State.currentmodule != null && State.currentmodule.Singlehotkey && !State.currentmodule.UseSingleRadioSelection)
                {
                    return;
                }

                if (IsPTTModeSingle())
                {
                    PTT_SetConfigSingle();
                    Update_PTT_GUI();
                    return;
                }

                if (IsPTTModeMulti())
                {
                    PTT_SetConfigMulti();
                    Update_PTT_GUI();
                    return;
                }

            }
        }
    }
}
