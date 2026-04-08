using System.Collections.Generic;
using VAICOM.Servers;


namespace VAICOM
{
    namespace FileManager
    {

        public static partial class FileHandler
        {

            public static partial class Lua
            {

                public static Dictionary<string, Server.LuaFile> LuaFiles = new Dictionary<string, Server.LuaFile>()
                {

                // ---- Export.lua  ----------------------------------

                {"2.8 Export.lua",new Server.LuaFile
                { fileid = "4E6E20D3-3BE1-4045-982C-D84CB7E35922",
                  filename = "Export.lua",
                  installfolder = "Scripts",
                  installfolder_legacy = "Scripts",
                  append = true,
                  root = false,
                  hardreset = false,
                  orig = Properties.Resources.Orig_Core_Export,
                  orig_legacy = Properties.Resources.Orig_Core_Export,
                  source = Properties.Resources.Append_Core_Export,
                  source_legacy = Properties.Resources.Append_Core_Export,
                  version ="2.8",
                  canremove = true,
                  reset = false,
                  //ACTIVE :
                  install = true,
                  export = true,
                  autoremove  = false,
                  quiet  = false,
                  AIRIO = false,
                  kneeboard = false,

                } },

                // ---- VAICOMPRO.export.lua  -------------------------

                {"2.8 VAICOMPRO.export.lua",new Server.LuaFile
                { fileid = "F33823D7-69B4-4A7A-8993-9DCF7494003C",
                  filename = "VAICOMPRO.export.lua",
                  installfolder = "Scripts\\VAICOMPRO",
                  installfolder_legacy = "Scripts\\VAICOMPRO",
                  append = false,
                  root = false,
                  hardreset = true,
                  orig = Properties.Resources.Orig_Core_VAICOMPRO_export,
                  orig_legacy = Properties.Resources.Orig_Core_VAICOMPRO_export,
                  source = Properties.Resources.Append_Core_VAICOMPRO_export,
                  source_legacy = Properties.Resources.Append_Core_VAICOMPRO_export,
                  version ="2.8",
                  canremove = true,
                  reset = false,
                  //ACTIVE:
                  install = true,
                  export = true,
                  autoremove  = false,
                  quiet  = false,
                  AIRIO = false,
                  kneeboard = false,
                } },

                // ---- RadioCommandDialogsPanel.lua  ------------------

                {"2.8 RadioCommandDialogsPanel.lua",new Server.LuaFile
                { fileid = "D6A3E78B-CA66-4C8B-94CC-69A63B90E5B2",
                  filename = "RadioCommandDialogsPanel.lua",
                  installfolder = "Scripts\\UI\\RadioCommandDialogPanel",
                  installfolder_legacy = "Scripts\\UI\\RadioCommandDialogPanel",
                  append = true,
                  root = true,
                  hardreset = true,
                  orig = Properties.Resources.Orig_Core_RadioCommandDialogsPanel,
                  orig_legacy = Properties.Resources.Orig_Core_RadioCommandDialogsPanel,
                  source = Properties.Resources.Append_Core_RadioCommandDialogsPanel,
                  source_legacy = Properties.Resources.Append_Core_RadioCommandDialogsPanel,
                  version ="2.8",
                  canremove = true,
                  reset = false,
                  //(ACTIVE)
                  install = true,
                  export = true,
                  autoremove  = false,
                  quiet  = false,
                  AIRIO = false,
                  kneeboard = false,
                  } },

                // ---- Speech.lua: --------------------------------------

                {"2.8 speech.lua",new Server.LuaFile
                { fileid = "8B5DD910-4821-4748-A24D-7ADE09346B49",
                  filename = "speech.lua",
                  installfolder = "Scripts\\Speech",
                  installfolder_legacy = "Scripts\\Speech",
                  append = true,
                  root = true,
                  hardreset = true,
                  orig = Properties.Resources.Orig_Core_speech,
                  orig_legacy = Properties.Resources.Orig_Core_speech,
                  source = Properties.Resources.Append_Core_speech,
                  source_legacy = Properties.Resources.Append_Core_speech,
                  version ="2.8",
                  canremove = true,
                  reset = false,
                  //(ACTIVE)
                  install = true,
                  export = true,
                  autoremove = false,
                  quiet  = false,
                  AIRIO = false,
                  kneeboard = false,
                  } },

                // ---- common.lua: --------------------------------------

                {"2.8 common.lua",new Server.LuaFile
                { fileid = "F8ECF2E5-CD93-47C4-B728-2EAA290D9B67",
                  filename = "common.lua",
                  installfolder = "Scripts\\Speech",
                  installfolder_legacy = "Scripts\\Speech",
                  append = true,
                  root = true,
                  hardreset = true,
                  orig = Properties.Resources.Orig_Core_common,
                  orig_legacy = Properties.Resources.Orig_Core_common,
                  source = Properties.Resources.Append_Core_common,
                  source_legacy = Properties.Resources.Append_Core_common,
                  version ="2.8",
                  canremove = true,
                  reset = false,
                  //(ACTIVE)
                  install = true,
                  export = true,
                  autoremove = false,
                  quiet  = false,
                  AIRIO = false,
                  kneeboard = false,
                  } },

                // ---- TabSheetBar.lua  ----------------------------------

                {"2.8 TabSheetBar.lua",new Server.LuaFile
                { fileid = "173C0B42-5CD0-4929-88C1-46B5AC1FF668",
                  filename = "TabSheetBar.lua",
                  installfolder = "Scripts\\UI\\RadioCommandDialogPanel",
                  installfolder_legacy = "Scripts\\UI\\RadioCommandDialogPanel",
                  append = true,
                  root = true,
                  hardreset = true,
                  orig = Properties.Resources.Orig_Core_TabSheetBar,
                  orig_legacy = Properties.Resources.Orig_Core_TabSheetBar,
                  source = Properties.Resources.Append_Core_TabSheetBar,
                  source_legacy = Properties.Resources.Append_Core_TabSheetBar,
                  version ="2.8",
                  canremove = true,
                  reset = false,
                  //(ACTIVE)
                  install = true,
                  export = true,
                  autoremove  = false,
                  quiet  = false,
                  AIRIO = false,
                  kneeboard = false,
                  } },

                // ---- gameMessages.lua  ----------------------------------

                {"2.8 gameMessages.lua",new Server.LuaFile
                { fileid = "9DDCD66F-BBC6-4B7B-8335-5BEEB1DB521A",
                  filename = "gameMessages.lua",
                  installfolder = "Scripts\\UI",
                  installfolder_legacy = "Scripts\\UI",
                  append = false, // <---,
                  root = true,
                  hardreset = true,
                  orig = Properties.Resources.Orig_Core_gameMessages,
                  orig_legacy = Properties.Resources.Orig_Core_gameMessages,
                  source = Properties.Resources.Append_Core_gameMessages,
                  source_legacy = Properties.Resources.Append_Core_gameMessages,
                  version ="2.8",
                  canremove = true,
                  reset = !State.activeconfig.HideOnScreenText, // <-----
                  //ACTIVE :
                  install = true,
                  export = true,
                  autoremove  = false,
                  quiet  = false,
                  AIRIO = false,
                  kneeboard = false,

                } },

                // ---- JesterAI_page.lua  -------------------------

                {"2.8 JesterAI_Page.lua",new Server.LuaFile
                { fileid = "D784B941-D625-4C9F-B124-B38CD6EB41AD",
                  filename = "JesterAI_Page.lua",
                  installfolder = "Mods\\aircraft\\F14\\Cockpit\\Scripts\\JesterAI",
                  installfolder_legacy = "Mods\\aircraft\\F14\\Cockpit\\Scripts\\JesterAI",
                  append = false,
                  root = true,
                  hardreset = true,
                  orig = Properties.Resources.Orig_F14_JesterAI_Page,
                  source = Properties.Resources.Append_F14_JesterAI_Page, //,
                  stringreplace = false,
                  stringorig    = Properties.Resources.Orig_ReplaceString_F14_JesterAI_Page,
                  stringsource  = Properties.Resources.Append_ReplaceString_F14_JesterAI_Page,
                  version ="2.8",
                  canremove = true,
                  reset = false,
                  //ACTIVE:
                  install = true,
                  export = false,
                  autoremove  = false,
                  quiet  = true,
                  AIRIO = true,
                  kneeboard = false,
                } },

                {"2.8 JesterInit.lua",new Server.LuaFile
                { fileid = "D60BC4B9-FD1B-4EE7-B095-2964DDD0095E",
                  filename = "init.lua",
                  installfolder = "Mods\\aircraft\\F14\\Cockpit\\Scripts\\JesterAI",
                  installfolder_legacy = "Mods\\aircraft\\F14\\Cockpit\\Scripts\\JesterAI",
                  append = false,
                  root = true,
                  hardreset = true,
                  orig = Properties.Resources.Orig_F14_JesterInit,
                  source = Properties.Resources.Append_F14_JesterInit,
                  stringreplace = false,
                  stringorig    = Properties.Resources.Orig_F14_JesterInit,
                  stringsource  = Properties.Resources.Append_F14_JesterInit,
                  version ="2.8",
                  canremove = true,
                  reset = false,
                  //ACTIVE:
                  install = true,
                  export = false,
                  autoremove  = false,
                  quiet  = true,
                  AIRIO = true,
                  kneeboard = false,
                } },

                // kneeboard page
                {"1.lua",new Server.LuaFile
                { fileid = "5B148082-1834-4E8E-8010-D0ED6AD22A80",
                  filename = "1.lua",
                  installfolder = "Scripts\\Aircrafts\\_Common\\Cockpit\\KNEEBOARD\\indicator\\CUSTOM",
                  installfolder_legacy = "Scripts\\Aircrafts\\_Common\\Cockpit\\KNEEBOARD\\indicator\\CUSTOM",
                  append = false,
                  root = true,
                  hardreset = false,
                  orig = Properties.Resources.Append_Kneeboard_1,
                  source = Properties.Resources.Append_Kneeboard_1,
                  version ="2.8",
                  canremove = true,
                  reset = false,
                  //ACTIVE:
                  install = true,
                  export = false,
                  autoremove = false,
                  quiet  = true,
                  AIRIO = false,
                  kneeboard = true,
                } },

                // kneeboard declare device
                {"declare_VAICOMPRO_device.lua",new Server.LuaFile
                { fileid = "CD9ECF99-5EE6-477D-AAFF-4182B0FB40E4",
                  filename = "declare_VAICOMPRO_device.lua",
                  installfolder = "Scripts\\Aircrafts\\_Common\\Cockpit\\VAICOMPRO",
                  installfolder_legacy = "Scripts\\Aircrafts\\_Common\\Cockpit\\VAICOMPRO",
                  append = false,
                  root = true,
                  hardreset = true,
                  orig = Properties.Resources.Append_Kneeboard_declare_VAICOMPRO_device,
                  source = Properties.Resources.Append_Kneeboard_declare_VAICOMPRO_device,
                  version ="2.8",
                  canremove = true,
                  reset = false,
                  //ACTIVE:
                  install = true,
                  export = false,
                  autoremove = false,
                  quiet  = true,
                  AIRIO = false,
                  kneeboard = true,
                } },

                 // common
                {"VAICOMPRO_Common.lua",new Server.LuaFile
                { fileid = "CD9ECF99-5EE6-477D-AAFF-4182B0FB40E4",
                  filename = "VAICOMPRO_Common.lua",
                  installfolder = "Scripts\\Aircrafts\\_Common\\Cockpit\\VAICOMPRO\\device",
                  installfolder_legacy = "Scripts\\Aircrafts\\_Common\\Cockpit\\VAICOMPRO\\device",
                  append = false,
                  root = true,
                  hardreset = true,
                  orig = Properties.Resources.Append_Kneeboard_VAICOMPRO_Common,
                  source = Properties.Resources.Append_Kneeboard_VAICOMPRO_Common,
                  version ="2.8",
                  canremove = true,
                  reset = false,
                  //ACTIVE:
                  install = true,
                  export = false,
                  autoremove = false,
                  quiet  = true,
                  AIRIO = false,
                  kneeboard = true,
                } },

                // device
                {"VAICOMPRO_Device.lua",new Server.LuaFile
                { fileid = "70158138-E302-4EDD-95F2-3956574E1012",
                  filename = "VAICOMPRO_Device.lua",
                  installfolder = "Scripts\\Aircrafts\\_Common\\Cockpit\\VAICOMPRO\\device",
                  installfolder_legacy = "Scripts\\Aircrafts\\_Common\\Cockpit\\VAICOMPRO\\device",
                  append = false,
                  root = true,
                  hardreset = true,
                  orig = Properties.Resources.Append_Kneeboard_VAICOMPRO_Device,
                  source = Properties.Resources.Append_Kneeboard_VAICOMPRO_Device,
                  version ="2.8",
                  canremove = true,
                  reset = false,
                  //ACTIVE:
                  install = true,
                  export = false,
                  autoremove = false,
                  quiet  = true,
                  AIRIO = false,
                  kneeboard = true,
                } },

                // command_defs
                {"command_defs",new Server.LuaFile
                { fileid = "3F71546B-A12F-41C5-9547-5BB2E85AD016",
                  filename = "command_defs.lua",
                  installfolder = "Scripts\\Aircrafts\\_Common\\Cockpit\\VAICOMPRO\\device",
                  installfolder_legacy = "Scripts\\Aircrafts\\_Common\\Cockpit\\VAICOMPRO\\device",
                  append = false,
                  root = true,
                  hardreset = true,
                  orig = Properties.Resources.Append_Kneeboard_command_defs,
                  source = Properties.Resources.Append_Kneeboard_command_defs,
                  version ="2.8",
                  canremove = true,
                  reset = false,
                  //ACTIVE:
                  install = true,
                  export = false,
                  autoremove = false,
                  quiet  = true,
                  AIRIO = false,
                  kneeboard = true,
                } },


                // png files
                {"Tabs_ALL.png",new Server.LuaFile
                {
                  filename = "Tabs_ALL.png",
                  installfolder = "Scripts\\Aircrafts\\_Common\\Cockpit\\VAICOMPRO\\indicator\\Textures",
                  binarysource = Properties.Resources.Tabs_ALL_png,
                  root = true,
                  hardreset = true,
                  canremove = true,
                  quiet  = true,
                  kneeboard = true,
                  binary = true,
                } },
                {"Tabs_AWACS.png",new Server.LuaFile
                {
                  filename = "Tabs_AWACS.png",
                  installfolder = "Scripts\\Aircrafts\\_Common\\Cockpit\\VAICOMPRO\\indicator\\Textures",
                  binarysource = Properties.Resources.Tabs_AWACS_png,
                  root = true,
                  hardreset = true,
                  canremove = true,
                  quiet  = true,
                  kneeboard = true,
                  binary = true,
                } },
                {"Tabs_ATC.png",new Server.LuaFile
                {
                  filename = "Tabs_ATC.png",
                  installfolder = "Scripts\\Aircrafts\\_Common\\Cockpit\\VAICOMPRO\\indicator\\Textures",
                  binarysource = Properties.Resources.Tabs_ATC_png,
                  root = true,
                  hardreset = true,
                  canremove = true,
                  quiet  = true,
                  kneeboard = true,
                  binary = true,
                } },
                {"Tabs_AOCS.png",new Server.LuaFile
                {
                  filename = "Tabs_AOCS.png",
                  installfolder = "Scripts\\Aircrafts\\_Common\\Cockpit\\VAICOMPRO\\indicator\\Textures",
                  binarysource = Properties.Resources.Tabs_AOCS_png,
                  root = true,
                  hardreset = true,
                  canremove = true,
                  quiet  = true,
                  kneeboard = true,
                  binary = true,
                } },
                {"Tabs_FLIGHT.png",new Server.LuaFile
                {
                  filename = "Tabs_FLIGHT.png",
                  installfolder = "Scripts\\Aircrafts\\_Common\\Cockpit\\VAICOMPRO\\indicator\\Textures",
                  binarysource = Properties.Resources.Tabs_FLIGHT_png,
                  root = true,
                  hardreset = true,
                  canremove = true,
                  quiet  = true,
                  kneeboard = true,
                  binary = true,
                } },
                {"Tabs_JTAC.png",new Server.LuaFile
                {
                  filename = "Tabs_JTAC.png",
                  installfolder = "Scripts\\Aircrafts\\_Common\\Cockpit\\VAICOMPRO\\indicator\\Textures",
                  binarysource = Properties.Resources.Tabs_JTAC_png,
                  root = true,
                  hardreset = true,
                  canremove = true,
                  quiet  = true,
                  kneeboard = true,
                  binary = true,
                } },
                {"Tabs_LOG.png",new Server.LuaFile
                {
                  filename = "Tabs_LOG.png",
                  installfolder = "Scripts\\Aircrafts\\_Common\\Cockpit\\VAICOMPRO\\indicator\\Textures",
                  binarysource = Properties.Resources.Tabs_LOG_png,
                  root = true,
                  hardreset = true,
                  canremove = true,
                  quiet  = true,
                  kneeboard = true,
                  binary = true,
                } },
                {"Tabs_NOTES.png",new Server.LuaFile
                {
                  filename = "Tabs_NOTES.png",
                  installfolder = "Scripts\\Aircrafts\\_Common\\Cockpit\\VAICOMPRO\\indicator\\Textures",
                  binarysource = Properties.Resources.Tabs_NOTES_png,
                  root = true,
                  hardreset = true,
                  canremove = true,
                  quiet  = true,
                  kneeboard = true,
                  binary = true,
                } },
                {"Tabs_REF.png",new Server.LuaFile
                {
                  filename = "Tabs_REF.png",
                  installfolder = "Scripts\\Aircrafts\\_Common\\Cockpit\\VAICOMPRO\\indicator\\Textures",
                  binarysource = Properties.Resources.Tabs_REF_png,
                  root = true,
                  hardreset = true,
                  canremove = true,
                  quiet  = true,
                  kneeboard = true,
                  binary = true,
                } },
                {"Tabs_TANKER.png",new Server.LuaFile
                {
                  filename = "Tabs_TANKER.png",
                  installfolder = "Scripts\\Aircrafts\\_Common\\Cockpit\\VAICOMPRO\\indicator\\Textures",
                  binarysource = Properties.Resources.Tabs_TANKER_png,
                  root = true,
                  hardreset = true,
                  canremove = true,
                  quiet  = true,
                  kneeboard = true,
                  binary = true,
                } },
                {"Notepad.png",new Server.LuaFile
                {
                  filename = "Notepad.png",
                  installfolder = "Scripts\\Aircrafts\\_Common\\Cockpit\\VAICOMPRO\\indicator\\Textures",
                  binarysource = Properties.Resources.Notepad_png,
                  root = true,
                  hardreset = true,
                  canremove = true,
                  quiet  = true,
                  kneeboard = true,
                  binary = true,
                } },
                {"Notepad_watermark.png",new Server.LuaFile
                {
                  filename = "Notepad_watermark.png",
                  installfolder = "Scripts\\Aircrafts\\_Common\\Cockpit\\VAICOMPRO\\indicator\\Textures",
                  binarysource = Properties.Resources.Notepad_watermark_png,
                  root = true,
                  hardreset = true,
                  canremove = true,
                  quiet  = true,
                  kneeboard = true,
                  binary = true,
                } },
                {"Notepad_smudge.png",new Server.LuaFile
                {
                  filename = "Notepad_smudge.png",
                  installfolder = "Scripts\\Aircrafts\\_Common\\Cockpit\\VAICOMPRO\\indicator\\Textures",
                  binarysource = Properties.Resources.Notepad_smudge_png,
                  root = true,
                  hardreset = true,
                  canremove = true,
                  quiet  = true,
                  kneeboard = true,
                  binary = true,
                } },
                // Add radio.lua terrain files that are left incomplete by the terrain developer these are files that are located in the Mods/terrains/ folder and are not part of the core game.
                {"2.8 Sinai_radio.lua",new Server.LuaFile
                {
                  filename = "radio.lua",
                  installfolder = "Mods\\terrains\\Sinai",
                  installfolder_legacy = "Mods\\terrains\\Sinai",
                  append = false, // <--- Do Not append must replace,
                  root = true,
                  hardreset = true,
                  orig = Properties.Resources.Orig_Terrain_Siani_radio,
                  source = Properties.Resources.Append_Terrain_Siani_radio,
                  stringreplace = false,
                  stringorig = Properties.Resources.Orig_Terrain_Siani_radio,
                  stringsource = Properties.Resources.Append_Terrain_Siani_radio,                  
                  version ="2.8",
                  canremove = true,
                  reset = false,
                  //ACTIVE:
                  install = true,
                  export = false,
                  autoremove  = true,
                  quiet  = false,
                  AIRIO = true,
                  kneeboard = false,
                } },

                {"2.8 Nevada_radio.lua",new Server.LuaFile
                {
                  filename = "radio.lua",
                  installfolder = "Mods\\terrains\\Nevada",
                  installfolder_legacy = "Mods\\terrains\\Nevada",
                  append = false, // <--- Do Not append must replace,
                  root = true,
                  hardreset = true,
                  orig = Properties.Resources.Orig_Terrain_Nevada_Radio,
                  source = Properties.Resources.Append_Terrain_Nevada_Radio,
                  stringreplace = false,
                  stringorig = Properties.Resources.Orig_Terrain_Nevada_Radio,
                  stringsource = Properties.Resources.Append_Terrain_Nevada_Radio,
                  version ="2.8",
                  canremove = true,
                  reset = false,
                  //ACTIVE:
                  install = true,
                  export = false,
                  autoremove  = true,
                  quiet  = false,
                  AIRIO = true,
                  kneeboard = false,
                } },

                // Currently unused.
                // Template to be used for future releases of terrains needing radio.lua replacement.
                // {"2.8 Kola_radio.lua",new Server.LuaFile
                // {
                //   filename = "radio.lua",
                //   installfolder = "Mods\\terrains\\Kola",
                //   installfolder_legacy = "Mods\\terrains\\Kola",
                //   append = false, // <--- Do Not append must replace,
                //   root = true,
                //   hardreset = true,
                //   orig = Properties.Resources.Orig_Terrain_Kola_radio,
                //   source = Properties.Resources.Append_Terrain_Kola_radio,
                //   stringreplace = false,
                //   stringorig = Properties.Resources.Orig_Terrain_Kola_radio,
                //   stringsource = Properties.Resources.Append_Terrain_Kola_radio,
                //   version ="2.8",
                //   canremove = true,
                //   reset = false,
                //   //ACTIVE:
                //   install = true,
                //   export = false,
                //   autoremove  = true,
                //   quiet  = false,
                //   AIRIO = true,
                //   kneeboard = false,
                // } },
                };

            }
        }
    }
}
