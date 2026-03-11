function create_simple_lamp(arg_, controller_, param_)
	local _lamp	= CreateGauge()
	_lamp.arg_number	= arg_
	_lamp.input		= {0.0, 1.0}
	_lamp.output 	= {0.0, 1.0}
	_lamp.controller	= controller_
	if param_ ~= nil then
		_lamp.params 	= {param_}
	end
	return _lamp
end

local count = -1
local function counter()
	count = count + 1
	return count
end


-- Cabin Lights
count = -1
Cabin_Lights = 
{
	FLAG_PrimaryPLT_lt                          = counter(),
	FLAG_FloodPLT_lt                            = counter(),
	FLAG_StbyInstPLT_lt                         = counter(),
	FLAG_UtilityPLT_lt							= counter(),
	FLAG_SignalWarningPLT_lt                 	= counter(),	
	FLAG_SignalPLT_lt                           = counter(),
	FLAG_PrimaryCPG_lt                        	= counter(),
	FLAG_FloodCPG_lt                          	= counter(),
	FLAG_UtilityCPG_lt                        	= counter(),
	FLAG_SignalWarningCPG_lt                 	= counter(),	
	FLAG_SignalCPG_lt                           = counter(),
}	

function create_cabin_lamp(arg_, param_)
	return create_simple_lamp(arg_, controllers.CockpitLights, param_)
end
				--TODO: arguments
PrimaryPLT_lt					= create_cabin_lamp(788, Cabin_Lights.FLAG_PrimaryPLT_lt)
FloodPLT_lt						= create_cabin_lamp(790, Cabin_Lights.FLAG_FloodPLT_lt)
StbyInstPLT_lt					= create_cabin_lamp(787, Cabin_Lights.FLAG_StbyInstPLT_lt)
--UtilityPLT_lt					= create_cabin_lamp(, Cabin_Lights.FLAG_UtilityPLT_lt)
SignalWarningPLT_lt				= create_cabin_lamp(813, Cabin_Lights.FLAG_SignalWarningPLT_lt)
SignalPLT_lt					= create_cabin_lamp(793, Cabin_Lights.FLAG_SignalPLT_lt)
PrimaryCPG_lt					= create_cabin_lamp(789, Cabin_Lights.FLAG_PrimaryCPG_lt)
FloodCPG_lt						= create_cabin_lamp(791, Cabin_Lights.FLAG_FloodCPG_lt)
--UtilityCPG_lt					= create_cabin_lamp(, Cabin_Lights.FLAG_UtilityCPG_lt)
SignalWarningCPG_lt				= create_cabin_lamp(814, Cabin_Lights.FLAG_SignalWarningCPG_lt)
SignalCPG_lt					= create_cabin_lamp(794, Cabin_Lights.FLAG_SignalCPG_lt)

-- Warning Lights
count = -1
Warning_Lights =  
{
	--pilot
	FLAG_MasterWarningPLT			= counter(),
	FLAG_MasterCautionPLT			= counter(),
	FLAG_FireEng1PLT				= counter(),
	FLAG_FireApuPLT					= counter(),
	FLAG_FireEng2PLT				= counter(),
	FLAG_RdyEng1PLT                 = counter(),
	FLAG_RdyApuPLT                  = counter(),
	FLAG_RdyEng2PLT                 = counter(),
	FLAG_DischPriPLT               	= counter(),
	FLAG_DischResPLT                = counter(),
	FLAG_EmergencyGuardPLT          = counter(),
	FLAG_EmergencyXpndrPLT          = counter(),
	FLAG_EmergencyEmergHydPLT		= counter(),
	FLAG_TailWheelUnlockPLT			= counter(),
	FLAG_ArmLTipPLT                 = counter(),
	FLAG_ArmLOutbdPLT               = counter(),
	FLAG_ArmLInbdPLT                = counter(),
	FLAG_ArmRInbdPLT                = counter(),
	FLAG_ArmROutbdPLT               = counter(),
	FLAG_ArmRTipPLT                 = counter(),
	FLAG_ArmamentASArmPLT           = counter(),
	FLAG_ArmamentASSafePLT          = counter(),
	FLAG_ArmamentGndOrideOnPLT      = counter(),
	FLAG_APUPLT                     = counter(),
	--CPG
	FLAG_MasterWarningCPG			= counter(),
	FLAG_MasterCautionCPG			= counter(),
	FLAG_FireEng1CPG				= counter(),
	FLAG_FireApuCPG					= counter(),
	FLAG_FireEng2CPG				= counter(),
	FLAG_RdyEng1CPG                 = counter(),
	FLAG_RdyApuCPG                  = counter(),
	FLAG_RdyEng2CPG                 = counter(),
	FLAG_DischPriCPG                = counter(),
	FLAG_DischResCPG                = counter(),
	FLAG_EmergencyGuardCPG          = counter(),
	FLAG_EmergencyXpndrCPG          = counter(),
	FLAG_EmergencyEmergHydCPG		= counter(),
	FLAG_TailWheelUnlockCPG			= counter(),
	FLAG_ArmLTipCPG                 = counter(),
	FLAG_ArmLOutbdCPG               = counter(),
	FLAG_ArmLInbdCPG                = counter(),
	FLAG_ArmRInbdCPG                = counter(),
	FLAG_ArmROutbdCPG               = counter(),
	FLAG_ArmRTipCPG                 = counter(),
	FLAG_ArmamentASArmCPG           = counter(),
	FLAG_ArmamentASSafeCPG          = counter(),
	FLAG_ArmamentGndOrideOnCPG      = counter(),
	FLAG_ProcessorSelectSp1CPG      = counter(),
	FLAG_ProcessorSelectSp2CPG      = counter(),
}

function create_warning_lamp(arg_, param_)
	return create_simple_lamp(arg_, controllers.WarningLights, param_)
end

				--TODO: arguments
MasterWarningPLT       			= create_warning_lamp(424,	Warning_Lights.FLAG_MasterWarningPLT)				
MasterCautionPLT       			= create_warning_lamp(425,	Warning_Lights.FLAG_MasterCautionPLT)
FireEng1PLT      				= create_warning_lamp(416,	Warning_Lights.FLAG_FireEng1PLT)
FireApuPLT			   			= create_warning_lamp(418,	Warning_Lights.FLAG_FireApuPLT)
FireEng2PLT		       			= create_warning_lamp(420,	Warning_Lights.FLAG_FireEng2PLT)
RdyEng1PLT                		= create_warning_lamp(417,	Warning_Lights.FLAG_RdyEng1PLT)
RdyApuPLT                 		= create_warning_lamp(419,	Warning_Lights.FLAG_RdyApuPLT)
RdyEng2PLT                		= create_warning_lamp(421,	Warning_Lights.FLAG_RdyEng2PLT)
DischPriPLT               		= create_warning_lamp(422,	Warning_Lights.FLAG_DischPriPLT)
DischResPLT               		= create_warning_lamp(423,	Warning_Lights.FLAG_DischResPLT)
EmergencyGuardPLT         		= create_warning_lamp(403,	Warning_Lights.FLAG_EmergencyGuardPLT)
EmergencyXpndrPLT         		= create_warning_lamp(404,	Warning_Lights.FLAG_EmergencyXpndrPLT)
EmergencyEmergHydPLT       		= create_warning_lamp(405,	Warning_Lights.FLAG_EmergencyEmergHydPLT)
TailWheelUnlockPLT       		= create_warning_lamp(402,	Warning_Lights.FLAG_TailWheelUnlockPLT)
ArmLTipPLT                		= create_warning_lamp(411,	Warning_Lights.FLAG_ArmLTipPLT)
ArmLOutbdPLT              		= create_warning_lamp(407,	Warning_Lights.FLAG_ArmLOutbdPLT)
ArmLInbdPLT               		= create_warning_lamp(408,	Warning_Lights.FLAG_ArmLInbdPLT)
ArmRInbdPLT               		= create_warning_lamp(409,	Warning_Lights.FLAG_ArmRInbdPLT)
ArmROutbdPLT              		= create_warning_lamp(410,	Warning_Lights.FLAG_ArmROutbdPLT)
ArmRTipPLT                		= create_warning_lamp(412,	Warning_Lights.FLAG_ArmRTipPLT)
ArmamentASArmPLT          		= create_warning_lamp(413,	Warning_Lights.FLAG_ArmamentASArmPLT)
ArmamentASSafePLT         		= create_warning_lamp(414,	Warning_Lights.FLAG_ArmamentASSafePLT)
ArmamentGndOrideOnPLT     		= create_warning_lamp(415,	Warning_Lights.FLAG_ArmamentGndOrideOnPLT)
APUPLT                    		= create_warning_lamp(406,	Warning_Lights.FLAG_APUPLT)

MasterWarningCPG       			= create_warning_lamp(806,	Warning_Lights.FLAG_MasterWarningCPG)
MasterCautionCPG       			= create_warning_lamp(808,	Warning_Lights.FLAG_MasterCautionCPG)
FireEng1CPG       				= create_warning_lamp(441,	Warning_Lights.FLAG_FireEng1CPG)
FireApuCPG       				= create_warning_lamp(443,	Warning_Lights.FLAG_FireApuCPG)
FireEng2CPG       				= create_warning_lamp(445,	Warning_Lights.FLAG_FireEng2CPG)
RdyEng1CPG                		= create_warning_lamp(442,	Warning_Lights.FLAG_RdyEng1CPG)
RdyApuCPG                 		= create_warning_lamp(444,	Warning_Lights.FLAG_RdyApuCPG)
RdyEng2CPG                		= create_warning_lamp(446,	Warning_Lights.FLAG_RdyEng2CPG)
DischPriCPG               		= create_warning_lamp(447,	Warning_Lights.FLAG_DischPriCPG)
DischResCPG               		= create_warning_lamp(448,	Warning_Lights.FLAG_DischResCPG)
EmergencyGuardCPG         		= create_warning_lamp(427,	Warning_Lights.FLAG_EmergencyGuardCPG)
EmergencyXpndrCPG         		= create_warning_lamp(428,	Warning_Lights.FLAG_EmergencyXpndrCPG)
EmergencyEmergHydCPG       		= create_warning_lamp(429,	Warning_Lights.FLAG_EmergencyEmergHydCPG)
TailWheelUnlockCPG       		= create_warning_lamp(426,	Warning_Lights.FLAG_TailWheelUnlockCPG)
ArmLTipCPG                		= create_warning_lamp(434,	Warning_Lights.FLAG_ArmLTipCPG)
ArmLOutbdCPG              		= create_warning_lamp(430,	Warning_Lights.FLAG_ArmLOutbdCPG)
ArmLInbdCPG               		= create_warning_lamp(431,	Warning_Lights.FLAG_ArmLInbdCPG)
ArmRInbdCPG               		= create_warning_lamp(432,	Warning_Lights.FLAG_ArmRInbdCPG)
ArmROutbdCPG              		= create_warning_lamp(433,	Warning_Lights.FLAG_ArmROutbdCPG)
ArmRTipCPG                		= create_warning_lamp(435,	Warning_Lights.FLAG_ArmRTipCPG)
ArmamentASArmCPG          		= create_warning_lamp(438,	Warning_Lights.FLAG_ArmamentASArmCPG)
ArmamentASSafeCPG         		= create_warning_lamp(439,	Warning_Lights.FLAG_ArmamentASSafeCPG)
ArmamentGndOrideOnCPG     		= create_warning_lamp(440,	Warning_Lights.FLAG_ArmamentGndOrideOnCPG)
ProcessorSelectSp1CPG     		= create_warning_lamp(436,	Warning_Lights.FLAG_ProcessorSelectSp1CPG)
ProcessorSelectSp2CPG     		= create_warning_lamp(437,	Warning_Lights.FLAG_ProcessorSelectSp2CPG)

