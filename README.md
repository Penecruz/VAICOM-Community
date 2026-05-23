
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


This is a major milestone for Vaicom. It brings Jester 2.0 integration for the F-4E, adds a new method of communication with DCS that allows for future opportunities and adds an export path for the Vaicom kneeboard that gives greater flexibility if design, better data and no longer changes core DCS files. OpenKneeboard OUT unlocks the full potential of Vaicom’s real time access to the live DCS lua environment.
Below the waterline there are many bug fixes and enhancements across the entire software package.

-	Add ‘Boots’ AIWSO for the F-4E Phantom II our jester 2.0 implementation.
-	Add OpenKneeboard OUT a vaicom Web Console for the amazing OpenKneeboard application.
-	Add new Vaicom HTML Keywords reference system, with plugin for OpenKneeboard.
-	Add F-100D Super Sabre as supported module.
-	Add new F10 menu handling to eliminate connection issues on servers with large logistic menu systems e.g. Foothold missions.
-	Remove all Pro licencing code from Vaicom and any graphical reference in the UI.
-	Add ICS Hot Mic logic for AH-64D Apache and F-4E Phantom II.
-	Add new Hot Mic listening state handler for all supported AI CREW modules.
-	Add common AI CREW UI interfaces and adopt shared code.
-	Fixes F-14 Tomcat “Jester Options” mini wheel not exiting cleanly after message sent.
-	Adds F-14 Tomcat Pilot/RIO monitoring on Vaicom UI to reflect connection.
-	Adds F-4E Phantom II Pilot/WSO monitoring on Vaicom UI to reflect connection.
-	Adds E-7A as supported AWACS recipient and Kneeboard unit.
-	Adds BRA to Kneeboard units and airfields.
-	Adds TACAN information to Kneeboard Tankers and AWACS.
-	Many small bug fixes and enhancements


Known Issues

-	C-130J Select Tunes radio command will tune radio but not change AMU or CNI-MU display.
-	George AI the AH-64D M299_EMPTY racks if loaded with other missiles break direct weapon selection (still working a fix for this).


## Community Team

Pene, Special K, Sleighzy, D3adCy11nd3r, Folgers, Hornblower793, Liam8, MAXsenna, MisterOutofTime, Raskit, Hue Jass and stag1975

## Patreon Donations

If you want to donate a beer, visit the Official Vaicom Patreon.
[Vaicom Patreon Site](https://patreon.com/PeneCruz?utm_medium=unknown&utm_source=join_link&utm_campaign=creatorshare_creator&utm_content=copyLink)


#### Beta Team
104th_Aeons, GSG-3|Turbine|202, DrChainsaw, Jaeger, Nicola, Padinn, SPAZ-505, tomeye, Virus, Bonz RexExGSR, LawnBoy, Scotia and MrAxen 
