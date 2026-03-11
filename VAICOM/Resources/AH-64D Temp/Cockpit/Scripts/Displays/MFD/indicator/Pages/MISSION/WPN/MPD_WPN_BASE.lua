dofile(LockOn_Options.script_path.."Displays/MFD/indicator/Pages/MISSION/WPN/MPD_WPN_Symbology_defs.lua")

-----------------------------------------------------------
-------------------- Prepare variables --------------------
-----------------------------------------------------------


-----------------------------------------------------------
--------------------- Draw something ----------------------
-----------------------------------------------------------

Draw_SAFE_ARM_Icons()
DrawCommonInfoWindows()

Draw_HellfireMissiles()
Draw_RocketLaunchers()
Draw_GUN()

Add_Plane_Icon()

--AddDBGGrid()
--AddDBGPylonAxis()

-----------------------------------------------------------
---------------------- Finally ----------------------------
-----------------------------------------------------------