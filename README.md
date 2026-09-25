
![Vaicom GitHub Banner](https://github.com/user-attachments/assets/f7522f44-efb5-427e-b99f-868a09429806)


[![Downloads](https://img.shields.io/github/downloads/Penecruz/VAICOMPRO-Community/total?logo=GitHub)](https://github.com/Penecruz/VAICOMPRO-Community/releases/latest)
[![Discord](https://img.shields.io/discord/736032844274728961?logo=Discord)](https://discord.gg/7c22BHNSCS)
[![Latest Release](https://img.shields.io/github/v/release/Penecruz/VAICOMPRO-Community?logo=GitHub)](https://github.com/Penecruz/VAICOMPRO-Community/releases/latest)

VAICOM Community Edition for DCS World

## Overview - Community Edition

On 31 OCT 2022 Hollywood_315 open sourced his awesome AI communications software for DCS Word. VAICOMPRO has been the launch pad for VR flyers in DCS to create a
immersive environment free from the constraints of keyboard or mouse-controlled radio menus.

A group of community members have patched his work to make it compatible with DCS 2.8.XXXXX and later. This is a standalone installer that will replace your previous version of VAICOM. It will not work with DCS 2.7.XXXXX or erlier.

We now have VAICOM Community Edition running well with DCS 3.0.X.X and are looking where we can take it going forward with lots of new modules coming to DCS World.
We continue to develop VAICOMPRO to keep it functioning with changes to DCS. That said, there will be issues from time to time. So please use the issues register here on GitHub to report them.

Remember this is a community group, a group that donates their time to keep this awesome software alive. Be respectful and patient, we all have real jobs too. Join our Discord Server (link Below) and become part of our community.

## Important Information

VAICOM Community is 100% free and includes all modules (Chatter, AIRIO, Kneeboard, Realistic ATC) that were available with the last paid release.

Use of this software is at your risk, we accept no liability for stuffing up your Voice Attack installation, DCS World installation, Windows installation, or any other action.

The VAICOM Community Team

## Known Issues

VAICOM Community 3.0.X.X is not designed to be backwards compatible with DCS 2.7.X If you wish to continue using VAICOMPRO for DCS 2.7, please use Hollywood_315's final release and not VAICOMPRO Community.

VAICOM Community Edition will not pass the Integrity Check on Multiplayer Servers that require Pure Client Scripts unless the AIRIO and Kneeboard extensions are deactivated via the VAICOMPRO UI.
This is because VAICOMPRO adds lines to some of DCS World's core LUA files to enable it to function. Multiplayer Server administrators must enable Pure Client Scripts as an option as it is off by default. Very few Servers require Pure Client Scripts. This is something that only ED can change.

DiCE: DCS Integrated Countermeasure Editor creates many functionality issues with VAICOM Community, and it is recommended this be uninstalled before using VAICOM Community.

Flashing Comms Menu after DCS World update is a known issue and can be resolved with a lua reset, closing DCS and voiceAttack then launching VoiceAttack again prior to launching DCS to generate DCS side files.

## Installation Instructions

#### NOTE: If this is a new VAICOM installation, you should follow the install instructions in the VAICOM manual found in the VAICOMPRO/Documentation folder.
	
#### To update from an older version of VAICOMPRO


1. Ensure DCS is not Running

2. Backup your current VoiceAttack profile by clicking "More Profile Actions" (button right of the edit in VoiceAttack) and exporting your profile to a known location (this avoids tears in the event of an issue).

3. If you are using the MSI Installer, you will need to uninstall via the Windows process It will retain your config and profile settings (You will be propted if you try running the installer)

4. If you are using the Zip file just unzip over the top of you existing VAICOMPRO folder in Program Files/ VoiceAttack /Apps folder

5. Launch VoiceAttack and exit VoiceAttack (this allows VAICOMPRO to build the required DCS files).
	
6. Launch VoiceAttack and launch the VAICOM config menu (L CTRL+L ALT+C) Check that your settings have been retained and the DCS Path details are correct.

7. Launch DCS and confirm 

8. Join our Discord at https://discord.gg/7c22BHNSCS if you have any questions or issues with the install.

## Installation Tutorial Videos

[![IMAGE ALT TEXT HERE](https://img.youtube.com/vi/-bbQf6cU2EM/0.jpg)](https://www.youtube.com/watch?v=-bbQf6cU2EM)

[![IMAGE ALT TEXT HERE](https://img.youtube.com/vi/NiP42guoKW0/0.jpg)](https://www.youtube.com/watch?v=NiP42guoKW0)

[![IMAGE ALT TEXT HERE](https://img.youtube.com/vi/TJjd0Pvccmk/0.jpg)](https://www.youtube.com/watch?v=TJjd0Pvccmk)


## Patch Notes


This update is focused on two key areas, the F-14 Tomcat and Vaicom performance. It restores previous functionality for the F-14A/B and adds a couple of changes to keywords and fixes up some non-functioning commands and removes some that were never wired in previously. It adds functionality for the F-14B(U) and as a first pass and this we will make changes in future releases based on user feedback. It also adds new coding to block the flashing of the Jester Wheel when some commands were issued.
On the performance side a massive code cleanup of the communications between Vaicom and DCS with increased use of caching and removing data not used from the network traffic. With the rise of OpenKneeboard Out Vaicom was grabbing more data from DCS, this was causing frame rate latency issues for some users, particularly on very busy missions or servers with lots of units and client slots. 

New
- Added Tomcat shutdown command and sequence handling, including F-14B(U)-specific routing, cue playback, and alias registration.
- Added module-aware AIRIO context-key flows, including EGI direct waypoint support.
- Added Flight Plan tab-aware diagnostics collection (now tied to FLT PLN tab view).
- Added expanded map-marker diagnostics support with increased marker cap (20 → 64).
- Added expanded AI CREW keyword categories in kneeboard output, including Startup, Shutdown, Walkman, and Miscellaneous etc.

Improved
- Refreshed F-14/AIRIO Jester script mappings and command coverage across radar, weapons, LANTIRN, datalink, TACAN, NAV/utility, CMDS/defensive, and GGW/JDAM behaviors.
- Modernized AIRIO command architecture: improved command wiring, labels/hints, routing/validation, and proxy timing reliability.
- Improved AIRIO option-hint selection and EGI direct-waypoint sequencing reliability.
- Improved AIRIO GGW preplanned command handling with broader spoken-number parsing, clearer pass/fail logging, explicit sequence dispatch, and queued feedback cues.
- Improved OpenKneeboard AI CREW rendering/grouping and tightened AI CREW vs GND CREW separation behavior.
- Improved profile deployment/update flow, including auto keyword finish and stronger embedded resource handling integrity.
- Improved F-14 wheel chocks/miniwheel runtime handling to reduce desync and visibility issues.
- Added caching in update/data paths to reduce runtime overhead.
- Removed redundant opposition aircraft/helicopter build pass to reduce duplicate update work.
- Simplified F-14 Jester integration by removing obsolete JesterInit patch resources and handling legacy miniwheel suppression directly in appended JesterAI page.

Fixed
- Fixed a null-pointer issue that could prevent some units from being collected for kneeboard output.

Known Issues
- C-130J Select Tunes radio command will tune radio but not change AMU or CNI-MU display.
- George AI the AH-64D M299_EMPTY racks if loaded with other missiles break direct weapon selection (still working a fix for this).

## Community Team

Pene, Special K, Sleighzy, D3adCy11nd3r, Folgers, Hornblower793, Liam8, MAXsenna, MisterOutofTime, Raskit, Hue Jass and stag1975

## Patreon Donations

If you want to donate a beer, visit the Official Vaicom Patreon.
[Vaicom Patreon Site](https://patreon.com/PeneCruz?utm_medium=unknown&utm_source=join_link&utm_campaign=creatorshare_creator&utm_content=copyLink)


#### Beta Team
104th_Aeons, GSG-3|Turbine|202, DrChainsaw, Jaeger, Nicola, Padinn, SPAZ-505, tomeye, Virus, Bonz RexExGSR, LawnBoy, Scotia, MrAxen, Haack, Contour, DragonBlade117, apogeo, ApocV, Goose, UnseenKill and Trigger.
