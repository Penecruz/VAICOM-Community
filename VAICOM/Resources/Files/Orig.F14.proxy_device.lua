dofile(LockOn_Options.script_path.."paths.lua")

dofile(base_script_path.."devices.lua")
dofile(base_script_path.."command_defs.lua")

local gettext = require("i_18n")
_ = gettext.translate

local dev = GetSelf()

local update_time_step = 0.1
make_default_activity(update_time_step)

local cmdfifo={}

-- listen for all the pertinent command IDs
for i=10000,device_commands.PROXY_last_command_sentinel do
  dev:listen_command(i)
end

function post_initialize()
    -- init device objects (names in devices.lua)
    weaponssystem = GetDevice(devices.WEAPONS)
    hotas = GetDevice(devices.HOTAS)
    hud = GetDevice(devices.HUD)
    vdi = GetDevice(devices.VDI)
    hsd = GetDevice(devices.HSD)
    radar = GetDevice(devices.RADAR)
    gearhook = GetDevice(devices.GEARHOOK)
    wingsweep = GetDevice(devices.WINGSWEEP)
    afcs = GetDevice(devices.AFCS)
    nav = GetDevice(devices.NAV_INTERFACE)
    lantirn = GetDevice(devices.LANTIRN)
    jester = GetDevice(devices.JESTERAI)
end

function ProcCommand(command, value)
	value = value or 1.0
    if command == device_commands.PROXY_Enable_ACM_guns_AA then -- 10000
        -- [lift ACM cover,] lift master arm, enable master arm switch, switch to guns, display mode air-to-air, TID repeat
        -- weaponssystem:performClickableAction(device_commands.WEAP_ACM_Cover, 1.0, false)
        weaponssystem:performClickableAction(device_commands.WEAP_Master_Arm_And_Cover, 1.0, false)
        hotas:performClickableAction(device_commands.STICK_Weapon_Selector_up, 1.0, false) -- select guns
        hud:performClickableAction(device_commands.DISP_mode_air2air, 1.0, false)
        hsd:performClickableAction(device_commands.HSD_Display_Mode_Cycle, 0.0, false) -- TID repeat
        --hotas:performClickableAction(device_commands.THROTTLE_CAGE_SEAM, 1.0, false) -- RTGS
        --hotas:performClickableAction(device_commands.THROTTLE_CAGE_SEAM, 0.0, false) -- release pushbutton
        afcs:performClickableAction(device_commands.AFCS_Stability_Roll, 0.0, false) -- disable roll SAS
    elseif command == device_commands.PROXY_carrier_landing then -- 10001
        gearhook:performClickableAction(device_commands.HOOK_Handle, 1.0, false)
        hud:performClickableAction(device_commands.DISP_mode_landing, 1.0, false)
        wingsweep:performClickableAction(device_commands.WINGSWEEP_FwdButton, 1.0, false)
    elseif command == device_commands.PROXY_ground_landing then -- 10002
        hud:performClickableAction(device_commands.DISP_mode_landing, 1.0, false)
        gearhook:performClickableAction(device_commands.HOOK_Handle, 0.0, false)
    elseif command == device_commands.PROXY_case1_recovery then --10003
        gearhook:performClickableAction(device_commands.HOOK_Handle, 1.0, false)
        hud:performClickableAction(device_commands.DISP_mode_landing, 1.0, false)
        wingsweep:performClickableAction(device_commands.WINGSWEEP_AftButton, 1.0, false)
        hud:performClickableAction(device_commands.DISP_HUD_AWL_mode, 1.0, false)
        vdi:performClickableAction(device_commands.DISP_VDI_landing_mode, 1.0, false)
        nav:performClickableAction(device_commands.NAV_Btn_Steer_AWL_PCD, 1.0, false)
    elseif command == device_commands.PROXY_pilot_VSL then --10004
        radar:performClickableAction(device_commands.WEAP_Target_designate_down, 1.0, false) -- pilot VSL
    elseif command == device_commands.PROXY_wings_swept then --10005
        wingsweep:performClickableAction(device_commands.WINGSWEEP_AftButton, 1.0, false)
        hud:performClickableAction(device_commands.DISP_mode_landing, 1.0, false)
    elseif command == device_commands.PROXY_a2g then --10006
        weaponssystem:performClickableAction(device_commands.WEAP_Master_Arm_And_Cover, 1.0, false)
        hud:performClickableAction(device_commands.DISP_mode_air2ground, 1.0, false)
        vdi:performClickableAction(device_commands.DISP_VDI_display_mode, 1.0, false)
    elseif command == device_commands.PROXY_parkingbrake then -- 10007
        gearhook:performClickableAction(device_commands.BRAKE_ParkingBrake, 0.0, false)
    elseif command == device_commands.PROXY_radarwow then -- 10008
        radar:performClickableAction(device_commands.RADAR_bypass_wow, 1.0, false)
	elseif command == device_commands.PROXY_Jester_LANTIRN_inhibit_auto_designate then
        jester:performClickableAction(device_commands.JESTER_LANTIRN_inhibit_auto_designate, value, false)
    elseif command == device_commands.PROXY_Jester_silent then
        jester:performClickableAction(device_commands.JESTER_Quiet, value, false)
    elseif command == device_commands.PROXY_Jester_pause then
        jester:performClickableAction(device_commands.JESTER_Pause, value, false)
    elseif command == device_commands.PROXY_Jester_LANTIRN_track_target_id then
        jester:performClickableAction(device_commands.JESTER_LANTIRN_track_target_id, value, false)
    elseif command == device_commands.PROXY_Jester_LANTIRN_track_zone_id then
        jester:performClickableAction(device_commands.JESTER_LANTIRN_track_zone_id, value, false)
    elseif command == device_commands.PROXY_Jester_LANTIRN_designate then
        jester:performClickableAction(device_commands.JESTER_LANTIRN_designate, value, false)
    elseif command == device_commands.PROXY_Jester_tune_ARC_182_channel then
        jester:performClickableAction(device_commands.Jester_tune_ARC_182_channel, value, false)
    elseif command == device_commands.PROXY_Jester_tune_TACAN_channel then
        jester:performClickableAction(device_commands.Jester_tune_TACAN_channel, value, false)
    elseif command == device_commands.PROXY_Jester_DEST_knob then
        jester:performClickableAction(device_commands.Jester_DEST_knob, value, false)
    elseif command == device_commands.PROXY_Jester_active_wpt_entry then
        jester:performClickableAction(device_commands.Jester_active_wpt_entry, value, false)
    elseif command == device_commands.PROXY_Jester_active_wpt_from_trigger_zone then
        jester:performClickableAction(device_commands.Jester_active_wpt_from_trigger_zone, value, false)
    elseif command == device_commands.PROXY_Jester_WCS then
        jester:performClickableAction(device_commands.Jester_WCS, value, false)
    elseif command == device_commands.PROXY_Jester_RWR_BIT then
        jester:performClickableAction(device_commands.Jester_RWR_BIT, value, false)
    elseif command == device_commands.PROXY_Jester_TID_range then
        jester:performClickableAction(device_commands.Jester_TID_range, value, false)
    elseif command == device_commands.PROXY_Jester_eject_RIO then
        jester:performClickableAction(device_commands.Jester_eject_RIO, value, false)
    elseif command == device_commands.PROXY_Jester_radar_track_target_id then
        jester:performClickableAction(device_commands.Jester_radar_track_target_id, value, false)
    elseif command == device_commands.PROXY_Jester_radar_drop_track then
        jester:performClickableAction(device_commands.Jester_radar_drop_track, value, false)
    elseif command == device_commands.PROXY_Jester_wpn_station_state then
        jester:performClickableAction(device_commands.Jester_wpn_station_state, value, false)
    elseif command == device_commands.PROXY_Jester_wpn_bomb_type_selector then
        jester:performClickableAction(device_commands.Jester_wpn_bomb_type_selector, value, false)
    elseif command == device_commands.PROXY_Jester_wpn_attk_mode then
        jester:performClickableAction(device_commands.Jester_wpn_attk_mode, value, false)
    elseif command == device_commands.PROXY_Jester_wpn_dlvy_mode then
        jester:performClickableAction(device_commands.Jester_wpn_dlvy_mode, value, false)
    elseif command == device_commands.PROXY_Jester_wpn_dlvy_optns_interval then
        jester:performClickableAction(device_commands.Jester_wpn_dlvy_optns_interval, value, false)
    elseif command == device_commands.PROXY_Jester_wpn_dlvy_optns_qty then
        jester:performClickableAction(device_commands.Jester_wpn_dlvy_optns_qty, value, false)
    elseif command == device_commands.PROXY_Jester_wpn_aa_launch then
        jester:performClickableAction(device_commands.Jester_wpn_aa_launch, value, false)
    elseif command == device_commands.PROXY_Jester_radar_azimuth then
        jester:performClickableAction(device_commands.Jester_radar_azimuth, value, false)
    elseif command == device_commands.PROXY_Jester_radar_elevation then
        jester:performClickableAction(device_commands.Jester_radar_elevation, value, false)
    -- LANTIRN zoom + F-14B(U) backup-avionics nav commands. These forward the fixed-value
    -- PROXY id straight through (no separate internal command); JesterAI/JesterAIBU::ParseCommand
    -- switch on the PROXY_* enum directly.
    elseif command == device_commands.PROXY_Jester_LANTIRN_zoom then
        jester:performClickableAction(device_commands.PROXY_Jester_LANTIRN_zoom, value, false)
    elseif command == device_commands.PROXY_Jester_BU_new_or_update_wpt_from_zone then
        jester:performClickableAction(device_commands.PROXY_Jester_BU_new_or_update_wpt_from_zone, value, false)
    elseif command == device_commands.PROXY_Jester_BU_select_waypoint then
        jester:performClickableAction(device_commands.PROXY_Jester_BU_select_waypoint, value, false)
    elseif command == device_commands.PROXY_Jester_BU_set_special_designation then
        jester:performClickableAction(device_commands.PROXY_Jester_BU_set_special_designation, value, false)
    elseif command == device_commands.PROXY_Jester_BU_transfer_JDAM_station then
        jester:performClickableAction(device_commands.PROXY_Jester_BU_transfer_JDAM_station, value, false)
    elseif command == device_commands.PROXY_Jester_BU_set_lts then
        jester:performClickableAction(device_commands.PROXY_Jester_BU_set_lts, value, false)
    elseif command == device_commands.PROXY_Jester_BU_hook_selected then
        jester:performClickableAction(device_commands.PROXY_Jester_BU_hook_selected, value, false)
    elseif command == device_commands.PROXY_Jester_BU_direct_to_selected then
        jester:performClickableAction(device_commands.PROXY_Jester_BU_direct_to_selected, value, false)
    elseif command == device_commands.PROXY_Jester_BU_steering_mode then
        jester:performClickableAction(device_commands.PROXY_Jester_BU_steering_mode, value, false)
    elseif command == device_commands.PROXY_Jester_BU_BDHI_mode then
        jester:performClickableAction(device_commands.PROXY_Jester_BU_BDHI_mode, value, false)
    elseif command == device_commands.PROXY_Jester_BU_TCN_EGI_select then
        jester:performClickableAction(device_commands.PROXY_Jester_BU_TCN_EGI_select, value, false)
    elseif command == device_commands.PROXY_Jester_BU_wpt_from_ggw_target then
        jester:performClickableAction(device_commands.PROXY_Jester_BU_wpt_from_ggw_target, value, false)
    elseif command == device_commands.PROXY_Jester_BU_izlid_point_zone then
        jester:performClickableAction(device_commands.PROXY_Jester_BU_izlid_point_zone, value, false)
    elseif command == device_commands.PROXY_Iceman_set_heading_absolute then
        jester:performClickableAction(device_commands.Iceman_set_heading_absolute, value, false)
    elseif command == device_commands.PROXY_Iceman_set_heading_relative then
        jester:performClickableAction(device_commands.Iceman_set_heading_relative, value, false)
    elseif command == device_commands.PROXY_Iceman_set_speed_absolute then
        jester:performClickableAction(device_commands.Iceman_set_speed_absolute, value, false)
    elseif command == device_commands.PROXY_Iceman_set_speed_relative then
        jester:performClickableAction(device_commands.Iceman_set_speed_relative, value, false)
    elseif command == device_commands.PROXY_Iceman_set_altitude_absolute then
        jester:performClickableAction(device_commands.Iceman_set_altitude_absolute, value, false)
    elseif command == device_commands.PROXY_Iceman_set_altitude_relative then
        jester:performClickableAction(device_commands.Iceman_set_altitude_relative, value, false)
    elseif command == device_commands.PROXY_Iceman_nav_fly_to_steerpoint then
        jester:performClickableAction(device_commands.Iceman_nav_fly_to_steerpoint, value, false)
    elseif command == device_commands.PROXY_Iceman_nav_orbit_steerpoint then
        jester:performClickableAction(device_commands.Iceman_nav_orbit_steerpoint, value, false)
    elseif command == device_commands.PROXY_Iceman_hold_current then
        jester:performClickableAction(device_commands.Iceman_hold_current, value, false)
    else
        print_message_to_user("Unhandled proxy command "..tostring(command)..", val="..tostring(value))
    end
end

function update()
    while (#cmdfifo > 0) do
        local entry = table.remove(cmdfifo,1)
        ProcCommand( entry.command_id, entry.command_value )
    end
end

function SetCommand(command,value)
    table.insert(cmdfifo, { command_id = command, command_value = value } )
end

need_to_be_closed = false -- close lua state after initialization
